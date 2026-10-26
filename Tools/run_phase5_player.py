"""Run the opt-in Phase5 development-player audit with isolated display/audio.
Never touches campaign saves; the player creates a fresh fixture per output.
"""
import argparse,datetime,json,os,subprocess,tempfile,time
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
p=argparse.ArgumentParser(description=__doc__);p.add_argument('--output',required=True);p.add_argument('--visual',action='store_true');p.add_argument('--headless',action='store_true');p.add_argument('--player-feel',action='store_true');p.add_argument('--lighting-review',action='store_true');p.add_argument('--lighting-scene',choices=['Prolog_RumahAtmaja','Babak1_Rusunawa','Babak2_RSUD','Babak3_PabrikGula','Babak4_KarangSetra','Babak5_PasarSetan']);p.add_argument('--final',choices=['BAD','NORMAL','TRUE','SECRET']);p.add_argument('--resume');p.add_argument('--probe',choices=['core','ending']);p.add_argument('--point',choices=['temp-flushed','replaced']);p.add_argument('--verify',action='store_true');a=p.parse_args()
out=(ROOT/a.output).resolve();out.mkdir(parents=True,exist_ok=True)
assert out.is_relative_to(ROOT/'Builds'),out
binary=ROOT/'Builds/LinuxPhase5/LayarTerakhir.x86_64';assert binary.is_file()
with tempfile.TemporaryDirectory(prefix='lt-p5-audio-') as tmp:
 sock=Path(tmp)/'native';env=dict(os.environ,PULSE_RUNTIME_PATH=tmp,PULSE_SERVER='unix:'+str(sock),LIBGL_ALWAYS_SOFTWARE='1',LP_NUM_THREADS='3',DBUS_SESSION_BUS_ADDRESS='unix:path='+tmp+'/none')
 pulse=subprocess.Popen(['pulseaudio','-n','--daemonize=no','--use-pid-file=no','--exit-idle-time=-1','--log-level=error','--load=module-null-sink sink_name=lt_p5 rate=48000 channels=2','--load=module-native-protocol-unix socket='+str(sock)+' auth-anonymous=1'],env=env,stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL,start_new_session=True)
 try:
  for i in range(100):
   if sock.exists():break
   time.sleep(.05)
  assert sock.exists()
  cmd=['xvfb-run','-a','-s','-screen 0 1280x720x24',str(binary),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-glcore','-phase5Audit',str(out),'-logFile',str(out/'player.log')]
  if a.player_feel or a.lighting_review:
   assert not (a.probe or a.final or a.resume or a.visual)
   assert not (a.player_feel and a.lighting_review)
   cmd[cmd.index('-phase5Audit')]='-feelAudit' if a.player_feel else '-lightingAudit'
  if a.lighting_scene:
   assert a.lighting_review
   cmd+=['-lightingScene',a.lighting_scene]
  if a.probe:
   assert a.point
   pos=cmd.index('-phase5Audit');cmd[pos:pos+2]=['-phase5Probe',str(out),'-phase5CrashAction',a.probe,'-phase5CrashPoint',a.point]
   if a.verify:cmd+=['-phase5Verify','yes']
  if a.visual:cmd+=['-phase5Visual']
  if a.headless:cmd+=['-batchmode','-nographics']
  if a.final:cmd+=['-phase5Final',a.final]
  if a.resume:cmd+=['-phase5Resume',str((ROOT/a.resume).resolve())]
  now=lambda:datetime.datetime.now(datetime.timezone.utc).isoformat();record=dict(command=cmd,startedUtc=now(),status='Running');(out/'invocation.json').write_text(json.dumps(record,indent=2)+'\n')
  code=subprocess.run(cmd,cwd=ROOT,env=env,start_new_session=True,stdout=subprocess.DEVNULL,stderr=subprocess.STDOUT).returncode
  record.update(status='Finished',endedUtc=now(),exitCode=code);(out/'invocation.json').write_text(json.dumps(record,indent=2)+'\n');print(json.dumps(record));raise SystemExit(code)
 finally:
  pulse.terminate();pulse.wait(timeout=10)
