"""Launch Linux Unity in its own memory-bounded systemd service, outside tmux.

Example: python3 Tools/run_unity_isolated.py --record Audit/unity-run.json --
         -nographics -executeMethod LayarTerakhir.Editor.Phase4SceneBuilder.Build -quit
The launcher returns immediately. Poll the record and printed service name.
Memory/CPU limits contain failures; they are not game performance measurements.
"""
import argparse
import datetime
import json
import os
import shlex
from pathlib import Path
import subprocess
import sys
import tempfile
import time

ROOT = Path(__file__).resolve().parents[1]


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--record', required=True)
    parser.add_argument('--unity', default=os.environ.get('UNITY_EDITOR', '/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity'))
    parser.add_argument('--child', action='store_true', help=argparse.SUPPRESS)
    parser.add_argument('--finalize', action='store_true', help=argparse.SUPPRESS)
    parser.add_argument('arguments', nargs=argparse.REMAINDER)
    args = parser.parse_args()
    arguments = args.arguments[1:] if args.arguments[:1] == ['--'] else args.arguments
    record = (ROOT / args.record).resolve()
    if not record.is_relative_to(ROOT):
        parser.error('Invocation record must be inside this project')
    if args.finalize:
        result = json.loads(record.read_text())
        service_result = os.environ.get('SERVICE_RESULT', 'unknown')
        result['systemdServiceResult'] = service_result
        if result.get('status') != 'Finished':
            result.update(status='Finished', endedUtc=datetime.datetime.now(datetime.timezone.utc).isoformat(),
                          exitCode=137 if service_result == 'oom-kill' else 143,
                          error='Unity service interrupted: ' + service_result)
        record.write_text(json.dumps(result, indent=2) + '\n')
        return 0
    if not Path(args.unity).is_file():
        parser.error('Unity executable missing; supply --unity or UNITY_EDITOR')
    record.parent.mkdir(parents=True, exist_ok=True)
    now = lambda: datetime.datetime.now(datetime.timezone.utc).isoformat()
    if not args.child:
        unit = 'lt-unity-' + datetime.datetime.now().strftime('%Y%m%d-%H%M%S') + '-' + str(os.getpid())
        record.write_text(json.dumps(dict(status='Queued', queuedUtc=now(), unit=unit), indent=2) + '\n')
        command = ['systemd-run', '--user', '--collect', '--unit=' + unit,
                   '--property=MemoryMax=5G', '--property=MemorySwapMax=128M',
                   '--property=OOMPolicy=stop', '--property=CPUQuota=300%',
                   '--property=WorkingDirectory=' + str(ROOT), '--setenv=LT_UNITY_UNIT=' + unit,
                   '--property=ExecStopPost=' + shlex.join([sys.executable, str(Path(__file__).resolve()), '--finalize', '--record', str(record)]),
                   sys.executable, str(Path(__file__).resolve()), '--child', '--record', str(record),
                   '--unity', args.unity, '--'] + arguments
        subprocess.run(command, check=True)
        print('Unity isolated service:', unit, '\nInvocation record:', record)
        return 0
    with tempfile.TemporaryDirectory(prefix='lt-editor-audio-') as tmp:
        sock = Path(tmp) / 'native'
        env = dict(os.environ, PULSE_RUNTIME_PATH=tmp, PULSE_SERVER='unix:' + str(sock),
                   LIBGL_ALWAYS_SOFTWARE='1', LP_NUM_THREADS='3',
                   DBUS_SESSION_BUS_ADDRESS='unix:path=' + tmp + '/none')
        pulse = subprocess.Popen(['pulseaudio', '-n', '--daemonize=no', '--use-pid-file=no',
                                  '--exit-idle-time=-1', '--log-level=error',
                                  '--load=module-null-sink sink_name=lt_editor rate=48000 channels=2',
                                  '--load=module-native-protocol-unix socket=' + str(sock) + ' auth-anonymous=1'],
                                 env=env, stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL)
        command = ['xvfb-run', '-a', '-s', '-screen 0 1280x720x24', args.unity,
                   '-batchmode', '-job-worker-count', '1', '-projectPath', str(ROOT)] + arguments
        result = dict(command=command, startedUtc=now(), status='Running',
                      unit=os.environ.get('LT_UNITY_UNIT'), memoryLimitBytes=5 * 1024**3)
        record.write_text(json.dumps(result, indent=2) + '\n')
        code = 1
        try:
            for _ in range(100):
                if sock.exists():
                    break
                time.sleep(.05)
            if not sock.exists():
                raise RuntimeError('Private PulseAudio socket did not start')
            code = subprocess.run(command, cwd=ROOT, env=env, stdout=subprocess.DEVNULL,
                                  stderr=subprocess.STDOUT).returncode
        except Exception as error:
            result['error'] = str(error)
        finally:
            pulse.terminate()
            try:
                pulse.wait(timeout=10)
            except subprocess.TimeoutExpired:
                pulse.kill()
                pulse.wait()
            result.update(status='Finished', endedUtc=now(), exitCode=code)
            record.write_text(json.dumps(result, indent=2) + '\n')
        return code


if __name__ == '__main__':
    raise SystemExit(main())
