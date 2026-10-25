"""Run the opt-in Phase4 development-player audit with isolated display/audio.
Never touches campaign saves; the player creates a fresh fixture per output.
"""
import argparse,datetime,json,os,subprocess,tempfile,time
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
p=argparse.ArgumentParser(description=__doc__);p.add_argument('--output',required=True);p.add_argument('--visual',action='store_true');p.add_argument('--headless',action='store_true');p.add_argument('--fast',action='store_true');a=p.parse_args()
out=(ROOT/a.output).resolve();out.mkdir(parents=True,exist_ok=True)
assert out.is_relative_to(ROOT/'Builds'),out
binary=ROOT/'Builds/LinuxPhase4/LayarTerakhir.x86_64';assert binary.is_file()
with tempfile.TemporaryDirectory(prefix='lt-p4-audio-') as tmp:
 sock=Path(tmp)/'native';env=dict(os.environ,PULSE_RUNTIME_PATH=tmp,PULSE_SERVER='unix:'+str(sock),LIBGL_ALWAYS_SOFTWARE='1',LP_NUM_THREADS='3',DBUS_SESSION_BUS_ADDRESS='unix:path='+tmp+'/none')
 pulse=subprocess.Popen(['pulseaudio','-n','--daemonize=no','--use-pid-file=no','--exit-idle-time=-1','--log-level=error','--load=module-null-sink sink_name=lt_p4 rate=48000 channels=2','--load=module-native-protocol-unix socket='+str(sock)+' auth-anonymous=1'],env=env,stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL,start_new_session=True)
 try:
  for i in range(100):
   if sock.exists():break
   time.sleep(.05)
  assert sock.exists()
  cmd=['xvfb-run','-a','-s','-screen 0 1280x720x24',str(binary),'-screen-width','1280','-screen-height','720','-screen-fullscreen','0','-force-glcore','-phase4Audit',str(out),'-logFile',str(out/'player.log')]
  if a.visual:cmd+=['-phase4Visual']
  if a.headless:cmd+=['-batchmode','-nographics']
  if a.fast:cmd+=['-phase4Fast']
  now=lambda:datetime.datetime.now(datetime.timezone.utc).isoformat();record=dict(command=cmd,startedUtc=now(),status='Running');(out/'invocation.json').write_text(json.dumps(record,indent=2)+'\n')
  code=subprocess.run(cmd,cwd=ROOT,env=env,start_new_session=True,stdout=subprocess.DEVNULL,stderr=subprocess.STDOUT).returncode
  record.update(status='Finished',endedUtc=now(),exitCode=code);(out/'invocation.json').write_text(json.dumps(record,indent=2)+'\n');print(json.dumps(record));raise SystemExit(code)
 finally:
  pulse.terminate();pulse.wait(timeout=10)
