"""Reproducible serial Unity checks with actual command, exit code and UTC timings.
Run --stage validate|test|capture|build. The build stage preserves prior player output.
"""
import argparse, json, os, shutil, subprocess, sys
from datetime import datetime, timezone
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
UNITY = '/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity'
parser = argparse.ArgumentParser()
parser.add_argument('--stage', required=True, choices=['validate', 'test', 'capture', 'build'])
stage = parser.parse_args().stage
command = [UNITY, '-batchmode', '-projectPath', str(ROOT)]
record_path = ROOT / ('Audit/kos-v2-' + stage + '-invocation.json')
if stage == 'validate':
    command += ['-nographics', '-quit', '-executeMethod', 'LayarTerakhir.Editor.PrologMilestoneValidator.RunValidation']
elif stage == 'test':
    command += ['-nographics', '-runTests', '-testPlatform', 'EditMode', '-testFilter', 'LayarTerakhir.Editor.Tests',
                '-testResults', str(ROOT / 'Audit/kos-v2-tests.xml')]
elif stage == 'capture':
    command = ['xvfb-run', '-a', '-s', '-screen 0 1280x720x24'] + command
    command += ['-force-glcore', '-executeMethod', 'LayarTerakhir.Editor.KosVisualCapture.Gameplay']
elif stage == 'build':
    prior = ROOT / 'Builds/Previous' / ('before-kos-v2-' + datetime.now(timezone.utc).strftime('%Y%m%d-%H%M%S'))
    prior.mkdir(parents=True, exist_ok=False)
    for name in ['Windows', 'LayarTerakhir-Windows-x86_64-Development.zip', 'LayarTerakhir-Windows-x86_64-Development.zip.sha256']:
        old = ROOT / 'Builds' / name
        if old.exists(): shutil.move(str(old), str(prior / name))
    command += ['-nographics', '-quit', '-buildTarget', 'Win64', '-executeMethod', 'LayarTerakhir.Editor.WindowsDevelopmentBuild.Build']
    record_path = ROOT / 'Builds/Logs/windows-build-invocation.json'
log = ROOT / ('Builds/Logs/windows-development.log' if stage == 'build' else 'Audit/kos-v2-' + stage + '.log')
log.parent.mkdir(parents=True, exist_ok=True)
command += ['-logFile', str(log)]
record = {'stage': stage, 'command': command, 'startedUtc': datetime.now(timezone.utc).isoformat()}
print('UNITY_STAGE', stage, 'START', flush=True)
with log.with_suffix('.console.log').open('w') as console:
    result = subprocess.run(command, cwd=ROOT, stdout=console, stderr=subprocess.STDOUT)
record.update(endedUtc=datetime.now(timezone.utc).isoformat(), exitCode=result.returncode)
record_path.write_text(json.dumps(record, indent=2) + '\n')
print('UNITY_STAGE', stage, 'EXIT', result.returncode, flush=True)
sys.exit(result.returncode)
