"""Serial, recorded Unity invocations for the Golden reference scene.
Player captures are real standalone output, not evidence of Windows/Iris Xe performance.
"""
import argparse, json, os, shutil, struct, subprocess, sys, tempfile, time
import xml.etree.ElementTree as ET
from datetime import datetime, timezone
from pathlib import Path

ROOT=Path(__file__).resolve().parents[1]
UNITY='/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity'
parser=argparse.ArgumentParser()
parser.add_argument('--stage',required=True,choices=['apply','polish','validate','test','linux','windows','player','atmaja-build','atmaja-validate','atmaja-player','review-build','review-validate','review-player'])
parser.add_argument('--width',type=int,default=1280)
parser.add_argument('--height',type=int,default=720)
parser.add_argument('--graphics',choices=['glcore','vulkan'],default='glcore')
parser.add_argument('--fast',action='store_true',help='Explicitly accelerate smoke story waits; never a timing/FPS test')
args=parser.parse_args();stage=args.stage
if stage in ('atmaja-player','player'):
    print(stage+' now runs the reviewed three-scene audit (review-player)',flush=True)
    stage='review-player'
command=[UNITY,'-batchmode','-nographics','-projectPath',str(ROOT)]
record_path=ROOT/('Audit/golden-'+stage+'-invocation.json')
log=ROOT/('Audit/golden-'+stage+'.log')
is_player=stage in ['player','atmaja-player','review-player']
if stage.startswith(('atmaja-','review-')):
    record_path=ROOT/('Audit/'+stage+'-invocation.json');log=ROOT/('Audit/'+stage+'.log')
if stage=='test':
    command+=['-runTests','-testPlatform','EditMode','-testFilter','LayarTerakhir.Editor.Tests','-testResults',str(ROOT/'Audit/golden-tests.xml')]
elif is_player:
    command=['xvfb-run','-a','-s','-screen 0 1280x720x24',str(ROOT/'Builds/LinuxAudit/LayarTerakhir.x86_64'),
             '-force-'+args.graphics,'-screen-fullscreen','0','-screen-width',str(args.width),'-screen-height',str(args.height),
             '-reviewAudit' if stage=='review-player' else '-atmajaAudit' if stage=='atmaja-player' else '-goldenAudit',str(ROOT/('Audit/Visuals/Review-Linux' if stage=='review-player' else 'Audit/Visuals/Atmaja-Linux' if stage=='atmaja-player' else 'Audit/Visuals/Golden-Linux')),'-logFile',str(log)]
    if args.fast and stage=='player':command+=['-goldenAuditFast']
else:
    methods={'apply':'GoldenSceneProduction.Apply','polish':'CompactKosPolish.Apply','validate':'GoldenSceneValidation.Run','linux':'GoldenLinuxBuild.Build','windows':'WindowsDevelopmentBuild.Build','atmaja-build':'PlaytestRevisionBuilder.Build','atmaja-validate':'AtmajaSceneValidation.Run','review-build':'PlaytestRevisionBuilder.Build','review-validate':'PlaytestReviewValidation.Run'}
    if stage in ['linux','windows']:command+=['-buildTarget','Linux64' if stage=='linux' else 'Win64']
    if stage=='windows':
        prior=ROOT/'Builds/Previous'/('before-golden-'+datetime.now(timezone.utc).strftime('%Y%m%d-%H%M%S'))
        prior.mkdir(parents=True,exist_ok=False)
        for name in ['Windows','LayarTerakhir-Windows-x86_64-Development.zip','LayarTerakhir-Windows-x86_64-Development.zip.sha256']:
            old=ROOT/'Builds'/name
            if old.exists():shutil.move(str(old),str(prior/name))
        record_path=ROOT/'Builds/Logs/windows-build-invocation.json';log=ROOT/'Builds/Logs/windows-development.log'
    command+=['-quit','-executeMethod','LayarTerakhir.Editor.'+methods[stage],'-logFile',str(log)]
if stage=='test':command+=['-logFile',str(log)]
env=os.environ.copy()
pulse=None
pulse_temp=None
if is_player:
    # Clocked private virtual sink: ALSA's unclocked null sink can busy-loop on hosts without a soundcard.
    env['LIBGL_ALWAYS_SOFTWARE']='1'
    pulse_temp=tempfile.TemporaryDirectory(prefix='layar-golden-pulse-')
    socket=str(Path(pulse_temp.name)/'native')
    pulse=subprocess.Popen(['pulseaudio','-n','--daemonize=no','--use-pid-file=no','--exit-idle-time=-1',
        '--log-level=error','--load=module-null-sink sink_name=golden rate=48000 channels=2',
        '--load=module-native-protocol-unix socket='+socket+' auth-anonymous=1'],stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL)
    deadline=time.monotonic()+5
    while not Path(socket).exists() and pulse.poll() is None and time.monotonic()<deadline:time.sleep(.05)
    assert Path(socket).exists(),'Private PulseAudio sink did not start; install pulseaudio and pulseaudio-utils'
    env['PULSE_SERVER']='unix:'+socket
record={'stage':stage,'command':command,'startedUtc':datetime.now(timezone.utc).isoformat()}
if pulse:record['audioBackend']='Private clocked PulseAudio null sink; real listener tap, no physical audio hardware'
record['status']='Running'
record_path.write_text(json.dumps(record,indent=2)+'\n')
print('GOLDEN_STAGE',stage,'START',flush=True)
try:
    with log.with_suffix('.console.log').open('w') as console:
        result=subprocess.run(command,cwd=ROOT,env=env,stdout=console,stderr=subprocess.STDOUT)
finally:
    if pulse:
        pulse.terminate();pulse.wait(timeout=10);pulse_temp.cleanup()
record.update(endedUtc=datetime.now(timezone.utc).isoformat(),exitCode=result.returncode)
record['status']='Finished'
if stage=='test' and result.returncode==0:
    try:
        tests=ROOT/'Audit/golden-tests.xml'
        assert tests.stat().st_mtime>=datetime.fromisoformat(record['startedUtc']).timestamp(),'Stale test XML'
        summary=ET.parse(tests).getroot()
        assert summary.get('result')=='Passed' and int(summary.get('failed',1))==0 and int(summary.get('total',0))>=55,'Incomplete/failed Unity suite'
        record['testsVerified']={key:int(summary.get(key,0)) for key in ['total','passed','failed','skipped']}
    except (AssertionError,OSError,ET.ParseError) as error:
        record['exitCode']=1;record['validationError']=str(error)
if is_player and result.returncode==0:
    review=stage=='review-player';atmaja=stage=='atmaja-player';expected=35 if review else 12 if atmaja else 13
    receipt=ROOT/('Audit/Visuals/Review-Linux/player-audit.json' if review else 'Audit/Visuals/Atmaja-Linux/player-audit.json' if atmaja else 'Audit/Visuals/Golden-Linux/player-audit.json')
    data=json.loads(receipt.read_text()) if receipt.exists() else {}
    try:
        assert data.get('completed') and data.get('actualStandalonePlayer') and len(data.get('screenshots',[]))==expected
        assert data.get('checkpointSaved')
        if review:
            assert data.get('transitionFromKos') and data.get('motorPassedGate') and data.get('previousSaveArchived')
            assert data.get('walkedMeters',0)>90 and data.get('maximumInteriorFootY',2)<.7
            assert all(c.startswith('PASS ') for c in data['checks'])
            assert .00001<data.get('mixRms',0) and 0<data.get('mixPeak',0)<.98
        elif atmaja:
            assert data.get('transitionFromKos') and all(c.startswith('PASS ') for c in data['checks'])
        else:
            assert data.get('phoneVibrationStarted') and data.get('phoneVibrationStopped') and data.get('documentsOverflow')
            assert .00001<data.get('mixRms',0) and 0<data.get('mixPeak',0)<.98
        after=datetime.fromisoformat(record['startedUtc']).timestamp()
        assert receipt.stat().st_mtime>=after,'Stale player receipt'
        for name in data['screenshots']:
            shot=receipt.parent/name
            assert shot.stat().st_mtime>=after and shot.stat().st_size>1000,'Missing/stale screenshot: '+name
            with shot.open('rb') as image:
                header=image.read(24)
            assert header[:8]==b'\x89PNG\r\n\x1a\n' and struct.unpack('>II',header[16:24])==(args.width,args.height),name
        record['screenCapturesVerified']=expected;record['completedSmokeReceiptVerified']=True
    except (AssertionError,OSError) as error:
        record['exitCode']=1;record['validationError']='Incomplete/stale smoke evidence: '+str(error)
record_path.write_text(json.dumps(record,indent=2)+'\n')
print('GOLDEN_STAGE',stage,'EXIT',record['exitCode'],flush=True)
sys.exit(record['exitCode'])
