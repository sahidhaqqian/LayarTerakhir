"""Opt-in Phase3 native checks; keeps user saves isolated and records invocation."""
import argparse,datetime,hashlib,json,os,subprocess,tempfile,time
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
p=argparse.ArgumentParser(description=__doc__);p.add_argument('--mode',choices=['complete','visual','budget'],required=True);p.add_argument('--output',type=Path,required=True);p.add_argument('--fast',action='store_true');p.add_argument('--no-secret',action='store_true');p.add_argument('--renderer',choices=['glcore','glcore33','vulkan'],default='glcore');p.add_argument('--gfx-direct',action='store_true');a=p.parse_args()
out=a.output.resolve();out.mkdir(parents=True,exist_ok=True)
cmd=[str(ROOT/'Builds/LinuxPhase3/LayarTerakhir.x86_64')]
cmd+=['-force-'+a.renderer,'-screen-width','1280','-screen-height','720','-screen-fullscreen','0'] if a.mode=='visual' else ['-batchmode','-nographics']
if a.mode=='visual':cmd+=['-phase3Visual']
if a.mode=='budget':cmd+=['-phase3Budget']
if a.gfx_direct:cmd+=['-force-gfx-direct']
if a.fast:cmd+=['-phase3Fast']
if a.no_secret:cmd+=['-phase3NoSecret']
cmd+=['-phase3Audit',str(out),'-logFile',str(out/'player.log')]
cmd=['xvfb-run','-a','-s','-screen 0 1280x720x24']+cmd
env=dict(os.environ,LIBGL_ALWAYS_SOFTWARE='1',LP_NUM_THREADS='3')
pulse_temp=tempfile.TemporaryDirectory(prefix='lt-phase3-pulse-');socket=Path(pulse_temp.name)/'native'
env.update(PULSE_RUNTIME_PATH=pulse_temp.name,PULSE_SERVER='unix:'+str(socket))
pulse_env=dict(env,DBUS_SESSION_BUS_ADDRESS='unix:path='+pulse_temp.name+'/no-session-bus')
pulse=subprocess.Popen(['pulseaudio','-n','--daemonize=no','--use-pid-file=no','--exit-idle-time=-1','--log-level=error','--load=module-null-sink sink_name=phase3 rate=48000 channels=2','--load=module-native-protocol-unix socket='+str(socket)+' auth-anonymous=1'],env=pulse_env,stdout=subprocess.DEVNULL,stderr=subprocess.DEVNULL)
deadline=time.monotonic()+5
while not socket.exists() and pulse.poll() is None and time.monotonic()<deadline:time.sleep(.05)
assert socket.exists(),'Private clocked audio backend did not start'
now=lambda:datetime.datetime.now(datetime.timezone.utc).isoformat()
r=dict(command=cmd,startedUtc=now(),status='Running',graphical=a.mode=='visual',fast=a.fast,assemblySha256=hashlib.sha256((ROOT/'Builds/LinuxPhase3/LayarTerakhir_Data/Managed/Assembly-CSharp.dll').read_bytes()).hexdigest(),buildReport=json.loads((ROOT/'Audit/phase3-linux-build.json').read_text()))
record=out/'invocation.json';record.write_text(json.dumps(r,indent=2)+'\n')
try:
 with (out/'console.log').open('w') as log:done=subprocess.run(cmd,cwd=ROOT,env=env,stdout=log,stderr=subprocess.STDOUT)
finally:
 pulse.terminate();pulse.wait(timeout=10);pulse_temp.cleanup()
r.update(endedUtc=now(),status='Finished',exitCode=done.returncode);record.write_text(json.dumps(r,indent=2)+'\n')
result=out/'result.json'
if result.exists():
 d=json.loads(result.read_text());print(json.dumps({k:d.get(k) for k in ['completed','platform','gpu','walkedMeters','simulationTimeScale','rendered']}));print('CHECKS',len(d.get('checks',[])));print('\n'.join(x for x in d.get('checks',[]) if x.startswith(('FAIL','ERROR'))))
else:print('MISSING_RESULT',out)
valid=done.returncode==0 and result.exists()
if valid:
 valid=d.get('completed') and d.get('actualStandalonePlayer') and bool(d.get('checks')) and all(x.startswith('PASS ') for x in d['checks'])
 if a.mode=='visual':valid=valid and d.get('rendered') and len(d.get('screenshots',[]))>=12 and all((out/n).stat().st_size>1000 for n in d['screenshots'])
 r['receiptVerified']=bool(valid);record.write_text(json.dumps(r,indent=2)+'\n')
raise SystemExit(0 if valid else done.returncode or 1)
