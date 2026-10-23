"""Run isolated native Phase 1 smoke or the Unity regression suite, with receipts."""
import argparse
import json
import os
import subprocess
import tempfile
import time
import xml.etree.ElementTree as ET
from datetime import datetime, timezone
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
UNITY = '/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity'
parser = argparse.ArgumentParser(description=__doc__)
parser.add_argument('stage', choices=['build-scene', 'build-linux', 'player', 'test'])
parser.add_argument('--seed', type=Path, help='Explicit existing checkpoint fixture, copied into a new isolated audit save')
parser.add_argument('--fast', action='store_true', help='3x synthetic smoke clock, never a feel/FPS/pacing measurement')
parser.add_argument('--visual-review', action='store_true', help='Eight arranged views only; does not pass route/puzzle acceptance')
parser.add_argument('--output', default='Audit/Visuals/Phase1-Linux')
args = parser.parse_args()
output = (ROOT / args.output).resolve()
assert output.is_relative_to((ROOT / 'Audit/Visuals').resolve())
log = ROOT / ('Audit/phase1-' + args.stage + '.log')
record_path = ROOT / ('Audit/phase1-' + args.stage + '-invocation.json')
env = os.environ.copy()
pulse = None
pulse_temp = None
if args.stage == 'player':
    output.mkdir(parents=True, exist_ok=True)
    command = ['xvfb-run', '-a', '-s', '-screen 0 1280x720x24', str(ROOT / 'Builds/LinuxAudit/LayarTerakhir.x86_64'),
               '-force-glcore', '-screen-fullscreen', '0', '-screen-width', '1280', '-screen-height', '720', '-phase1Audit', str(output), '-logFile', str(log)]
    if args.fast: command.append('-phase1AuditFast')
    if args.visual_review: command.append('-phase1VisualReview')
    if args.seed: command+=['-phase1AuditSeed',str(args.seed.resolve())]
else:
    command = [UNITY, '-batchmode', '-nographics', '-projectPath', str(ROOT)]
    if args.stage == 'test':
        command += ['-runTests', '-testPlatform', 'EditMode', '-testFilter', 'LayarTerakhir.Editor.Tests', '-testResults', str(ROOT / 'Audit/phase1-regression-tests.xml')]
    else:
        command += ['-buildTarget', 'Linux64', '-quit', '-executeMethod', 'LayarTerakhir.Editor.Phase1SceneBuilder.'+('Build' if args.stage=='build-scene' else 'BuildLinux')]
    command += ['-logFile', str(log)]
if args.stage in ('player', 'test'):
    env['LIBGL_ALWAYS_SOFTWARE'] = '1'
    env.setdefault('LP_NUM_THREADS', '2')
    pulse_temp = tempfile.TemporaryDirectory(prefix='lt-phase1-pulse-')
    socket = Path(pulse_temp.name) / 'native'
    env['PULSE_RUNTIME_PATH'] = pulse_temp.name
    env['PULSE_SERVER'] = 'unix:' + str(socket)
    pulse_env = env.copy()
    pulse_env['DBUS_SESSION_BUS_ADDRESS'] = 'unix:path=' + pulse_temp.name + '/no-session-bus'
    pulse = subprocess.Popen(['pulseaudio', '-n', '--daemonize=no', '--use-pid-file=no', '--exit-idle-time=-1', '--log-level=error',
                              '--load=module-null-sink sink_name=phase1 rate=48000 channels=2',
                              '--load=module-native-protocol-unix socket=' + str(socket) + ' auth-anonymous=1'], env=pulse_env, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
    deadline = time.monotonic() + 5
    while not socket.exists() and pulse.poll() is None and time.monotonic() < deadline:
        time.sleep(.05)
    assert socket.exists(), 'Private clocked audio backend did not start'
record = {'stage': args.stage, 'startedUtc': datetime.now(timezone.utc).isoformat(), 'command': command, 'status': 'Running'}
record_path.write_text(json.dumps(record, indent=2) + '\n')
print('PHASE1_CHECK', args.stage, 'START', flush=True)
try:
    with log.with_suffix('.console.log').open('w') as console:
        process = subprocess.Popen(command, cwd=ROOT, env=env, stdout=console, stderr=subprocess.STDOUT)
        while process.poll() is None:
            try:
                process.wait(timeout=30)
            except subprocess.TimeoutExpired:
                print('PHASE1_CHECK', args.stage, 'RUNNING', flush=True)
finally:
    if pulse:
        pulse.terminate()
        pulse.wait(timeout=10)
        pulse_temp.cleanup()
record.update(endedUtc=datetime.now(timezone.utc).isoformat(), exitCode=process.returncode, status='Finished')
try:
    if args.stage == 'test' and process.returncode == 0:
        path=ROOT/'Audit/phase1-regression-tests.xml'
        assert path.stat().st_mtime>=datetime.fromisoformat(record['startedUtc']).timestamp(), 'Stale/unfinished test XML'
        suite=ET.parse(path).getroot()
        assert suite.get('result')=='Passed' and int(suite.get('failed',1))==0 and int(suite.get('total',0))>=142
        record['testsVerified']={key:int(suite.get(key,0)) for key in ['total','passed','failed','skipped']}
    if args.stage == 'player' and process.returncode == 0:
        path=output/'player-audit.json'
        assert path.stat().st_mtime>=datetime.fromisoformat(record['startedUtc']).timestamp(), 'Stale player receipt'
        data=json.loads(path.read_text())
        assert data['completed'] and data['actualStandalonePlayer'] and data['checkpointSaved']
        assert all(check.startswith('PASS ') for check in data['checks'])
        if args.visual_review:
            assert len(data['screenshots'])==10 and 'visual review only' in data['method']
        else:
            assert data['walkedMeters'] > 100
        assert all((output/shot).stat().st_mtime>=datetime.fromisoformat(record['startedUtc']).timestamp() and (output/shot).stat().st_size>1000 for shot in data['screenshots'])
        record['checksVerified']=len(data['checks']);record['capturesVerified']=len(data['screenshots'])
except (AssertionError, OSError, ET.ParseError) as error:
    record['exitCode']=1;record['validationError']=str(error)
record_path.write_text(json.dumps(record, indent=2) + '\n')
print('PHASE1_CHECK', args.stage, 'EXIT', record['exitCode'], flush=True)
raise SystemExit(record['exitCode'])
