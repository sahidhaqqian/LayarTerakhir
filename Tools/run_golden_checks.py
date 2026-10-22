"""Serial, recorded Unity invocations for the Golden reference scene.
Player captures are real standalone output, not evidence of Windows/Iris Xe performance.
"""
import argparse, json, os, re, shutil, struct, subprocess, sys, tempfile, time
import xml.etree.ElementTree as ET
from datetime import datetime, timezone
from pathlib import Path

ROOT=Path(__file__).resolve().parents[1]
UNITY='/root/Unity/Hub/Editor/6000.3.26f1/Editor/Unity'
parser=argparse.ArgumentParser()
parser.add_argument('--stage',required=True,choices=['feel-player','graphics-look','graphics-validate','graphics-player','melati-street','apply','polish','validate','test','linux','windows','player','atmaja-build','atmaja-validate','atmaja-player','review-build','review-validate','review-player','kos-refine','prolog-complete','prolog-presentation','stair-revision','designer-review','lounge-refine','playtest-repair','raka-repair','melati-repair','review-survey','prolog-player','rider-player','designer-player','designer-models'])
parser.add_argument('--test-filter',default='LayarTerakhir.Editor.Tests')
parser.add_argument('--chase-only',action='store_true',help='Diagnostic prolog chase checkpoint fixture; final delivery must also run the full continuation')
parser.add_argument('--width',type=int,default=1280)
parser.add_argument('--height',type=int,default=720)
parser.add_argument('--graphics',choices=['glcore','vulkan'],default='glcore')
parser.add_argument('--fast',action='store_true',help='Explicitly accelerate smoke story waits; never a timing/FPS test')
parser.add_argument('--review-output',default='Audit/Visuals/Review-Linux',help='Separate reviewed-player evidence directory under Audit/Visuals')
args=parser.parse_args();stage=args.stage
review_output=(ROOT/args.review_output).resolve()
assert review_output.is_relative_to((ROOT/'Audit/Visuals').resolve()),'Review captures must stay under Audit/Visuals'
if stage in ('atmaja-player','player'):
    print(stage+' now runs the reviewed three-scene audit (review-player)',flush=True)
    stage='review-player'
command=[UNITY,'-batchmode','-nographics','-projectPath',str(ROOT)]
record_path=ROOT/('Audit/golden-'+stage+'-invocation.json')
log=ROOT/('Audit/golden-'+stage+'.log')
is_player=stage in ['feel-player','graphics-player','player','atmaja-player','review-player','prolog-player','rider-player','designer-player']
if stage.startswith(('atmaja-','review-')):
    record_path=ROOT/('Audit/'+stage+'-invocation.json');log=ROOT/('Audit/'+stage+'.log')
if stage=='test':
    command+=['-runTests','-testPlatform','EditMode','-testFilter',args.test_filter,'-testResults',str(ROOT/'Audit/golden-tests.xml')]
elif is_player:
    command=['xvfb-run','-a','-s','-screen 0 1280x720x24',str(ROOT/'Builds/LinuxAudit/LayarTerakhir.x86_64'),
             '-force-'+args.graphics,'-screen-fullscreen','0','-screen-width',str(args.width),'-screen-height',str(args.height),
             '-feelAudit' if stage=='feel-player' else '-graphicsAudit' if stage=='graphics-player' else '-designerAudit' if stage=='designer-player' else '-riderAudit' if stage=='rider-player' else '-prologAudit' if stage=='prolog-player' else '-reviewAudit' if stage=='review-player' else '-atmajaAudit' if stage=='atmaja-player' else '-goldenAudit',str(review_output if stage in ('feel-player','graphics-player','review-player','prolog-player','rider-player','designer-player') else ROOT/('Audit/Visuals/Atmaja-Linux' if stage=='atmaja-player' else 'Audit/Visuals/Golden-Linux')),'-logFile',str(log)]
    if args.fast and stage=='player':command+=['-goldenAuditFast']
    if args.chase_only:
        assert stage=='prolog-player'
        command+=['-prologChaseOnly']
else:
    methods={'graphics-look':'DreadoutVisualRevision.Apply','graphics-validate':'DreadoutVisualRevision.Validate','melati-street':'MelatiStreetRevision.Apply','apply':'GoldenSceneProduction.Apply','polish':'CompactKosPolish.Apply','validate':'GoldenSceneValidation.Run','linux':'GoldenLinuxBuild.Build','windows':'WindowsDevelopmentBuild.Build','atmaja-build':'PlaytestRevisionBuilder.Build','atmaja-validate':'AtmajaSceneValidation.Run','review-build':'PlaytestRevisionBuilder.Build','review-validate':'PlaytestReviewValidation.Run','kos-refine':'PlaytestRevisionBuilder.RefineKos','prolog-complete':'PrologCompletionBuilder.Apply','prolog-presentation':'PrologPresentationBuilder.Apply','stair-revision':'AtmajaStairRevisionBuilder.Apply','designer-review':'DesignerReviewBuilder.Apply','lounge-refine':'DesignerReviewBuilder.RefineLounge','review-survey':'DesignerReviewBuilder.Survey','playtest-repair':'PlaytestRepairBuilder.Apply','raka-repair':'PrologPresentationBuilder.RebuildRakaOnly','melati-repair':'MelatiNeighborhoodBuilder.Apply'}
    methods['designer-models']='DesignerVrmBuilder.Apply'
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
if is_player or stage=='test':
    # Clocked private virtual sink: ALSA's unclocked null sink can busy-loop on hosts without a soundcard.
    env['LIBGL_ALWAYS_SOFTWARE']='1'
    # Leave CPU headroom for the editor regression suite beside software captures.
    if is_player:env.setdefault('LP_NUM_THREADS','2')
    pulse_temp=tempfile.TemporaryDirectory(prefix='layar-golden-pulse-')
    env['PULSE_RUNTIME_PATH']=pulse_temp.name
    socket=str(Path(pulse_temp.name)/'native')
    pulse_env=env.copy()
    # A second daemon must not claim the player's existing session-bus name.
    # Native-protocol Unix sockets are sufficient on this headless host.
    pulse_env['DBUS_SESSION_BUS_ADDRESS']='unix:path='+str(Path(pulse_temp.name)/'no-session-bus')
    pulse=subprocess.Popen(['pulseaudio','-n','--daemonize=no','--use-pid-file=no','--exit-idle-time=-1',
        '--log-level=error','--load=module-null-sink sink_name=golden rate=48000 channels=2',
        '--load=module-native-protocol-unix socket='+socket+' auth-anonymous=1'],env=pulse_env,stdout=subprocess.DEVNULL,stderr=subprocess.PIPE,start_new_session=True)
    deadline=time.monotonic()+5
    while not Path(socket).exists() and pulse.poll() is None and time.monotonic()<deadline:time.sleep(.05)
    if not Path(socket).exists():
        if pulse.poll() is None:pulse.terminate()
        _,diagnostic=pulse.communicate(timeout=10)
        pulse_temp.cleanup()
        raise RuntimeError('Private PulseAudio sink did not start: '+diagnostic.decode(errors='replace').strip())
    env['PULSE_SERVER']='unix:'+socket
record={'stage':stage,'command':command,'startedUtc':datetime.now(timezone.utc).isoformat()}
host_overrides={key:env[key] for key in ('UNITY_BURST_DISABLE_COMPILATION','LD_BIND_NOW') if key in env}
if host_overrides:record['hostEnvironmentOverrides']=host_overrides
if is_player:record['rendererEnvironment']={key:env[key] for key in ('LIBGL_ALWAYS_SOFTWARE','LP_NUM_THREADS')}
if pulse:record['audioBackend']='Private clocked PulseAudio null sink; '+('real listener tap' if stage=='review-player' else 'headless test audio device' if stage=='test' else 'virtual audio device; this fixture does not measure listener audio')+', no physical audio hardware'
record['status']='Running'
record_path.parent.mkdir(parents=True,exist_ok=True)
log.parent.mkdir(parents=True,exist_ok=True)
record_path.write_text(json.dumps(record,indent=2)+'\n')
print('GOLDEN_STAGE',stage,'START',flush=True)
try:
    with log.with_suffix('.console.log').open('w') as console:
        result=subprocess.Popen(command,cwd=ROOT,env=env,stdout=console,stderr=subprocess.STDOUT,start_new_session=True)
        while True:
            try:
                result.wait(timeout=30)
                break
            except subprocess.TimeoutExpired:
                print('GOLDEN_STAGE',stage,'RUNNING',flush=True)
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
        assert summary.get('result')=='Passed' and int(summary.get('failed',1))==0 and int(summary.get('total',0))>=(55 if args.test_filter=='LayarTerakhir.Editor.Tests' else 1),'Incomplete/failed Unity suite'
        record['testsVerified']={key:int(summary.get(key,0)) for key in ['total','passed','failed','skipped']}
    except (AssertionError,OSError,ET.ParseError) as error:
        record['exitCode']=1;record['validationError']=str(error)
if is_player and result.returncode==0:
    feel=stage=='feel-player';graphics=stage=='graphics-player';review=stage=='review-player';atmaja=stage=='atmaja-player';prolog=stage=='prolog-player';rider=stage=='rider-player';designer=stage=='designer-player'
    source='PlayerFeelAudit.cs' if feel else 'GraphicsReferenceAudit.cs' if graphics else 'DesignerReviewPlayerAudit.cs' if designer else 'RiderPresentationAudit.cs' if rider else 'PrologCompletionAudit.cs' if prolog else 'ReviewPlayerAudit.cs'
    expected_names=re.findall(r'\bShot\("([^"\n]+)"\)',(ROOT/'Assets/_LayarTerakhir/src/Diagnostics'/source).read_text()) if feel or graphics or review or prolog or rider or designer else []
    if prolog and args.chase_only:expected_names=[name for name in expected_names if int(re.match(r'\d+',name).group())>=16]
    expected=len(expected_names) if feel or graphics or review or prolog or rider or designer else 12 if atmaja else 13
    receipt=review_output/'player-audit.json' if feel or graphics or review or prolog or rider or designer else ROOT/('Audit/Visuals/Atmaja-Linux/player-audit.json' if atmaja else 'Audit/Visuals/Golden-Linux/player-audit.json')
    data=json.loads(receipt.read_text()) if receipt.exists() else {}
    try:
        assert data.get('completed') and data.get('actualStandalonePlayer') and len(data.get('screenshots',[]))==expected
        assert graphics or data.get('checkpointSaved')
        if feel:
            assert set(data['screenshots'])=={name+'.png' for name in expected_names}
            assert all(c.startswith('PASS ') for c in data['checks']) and len(data.get('samples',[]))>180
        elif graphics:
            assert not data.get('errors') and set(data['screenshots'])=={name+'.png' for name in expected_names}
        elif prolog:
            assert set(data['screenshots'])=={name+'.png' for name in expected_names},'Missing declared prolog captures'
            assert all(c.startswith('PASS ') for c in data['checks'])
            assert data.get('walkedMeters',0)>(50 if args.chase_only else 90),'Incomplete continuous continuation/escape route'
        elif review:
            assert set(data['screenshots'])=={name+'.png' for name in expected_names},'Missing declared review screenshots'
            assert data.get('transitionFromKos') and data.get('motorPassedGate') and data.get('previousSaveArchived')
            assert data.get('walkedMeters',0)>90 and data.get('maximumInteriorFootY',2)<.7,'Incomplete route or raised interior footing: '+str(data.get('maximumInteriorFootY'))+' at '+str(data.get('maximumInteriorFootPosition'))
            assert all(c.startswith('PASS ') for c in data['checks'])
            assert .00001<data.get('mixRms',0) and 0<data.get('mixPeak',0)<.98
        elif atmaja:
            assert data.get('transitionFromKos') and all(c.startswith('PASS ') for c in data['checks'])
        elif designer:
            assert set(data['screenshots'])=={name+'.png' for name in expected_names},'Missing declared designer review captures'
            assert all(c.startswith('PASS ') for c in data['checks'])
            assert data.get('walkedMeters',0)>25,'Incomplete designer review capsule route'
        elif rider:
            assert set(data['screenshots'])=={name+'.png' for name in expected_names},'Missing declared rider captures'
            assert all(c.startswith('PASS ') for c in data['checks'])
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
