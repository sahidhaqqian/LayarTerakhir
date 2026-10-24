"""Run an opt-in isolated standalone check and preserve its real invocation."""
import argparse,datetime,json,os,subprocess
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
p=argparse.ArgumentParser(description=__doc__)
p.add_argument('--mode',choices=['complete','boss','recovery','visual','safety','investigation','investigation-edges'],required=True)
p.add_argument('--zero-victory',action='store_true');p.add_argument('--exit-fault',action='store_true')
p.add_argument('--output',type=Path,required=True);p.add_argument('--fast',action='store_true');p.add_argument('--no-secret',action='store_true')
a=p.parse_args();out=a.output.resolve();out.mkdir(parents=True,exist_ok=True)
cmd=[str(ROOT/'Builds/LinuxAudit/LayarTerakhir.x86_64')]
graph=a.mode=='visual'
cmd+=['-force-glcore','-screen-width','1280','-screen-height','720','-screen-fullscreen','0'] if graph else ['-batchmode','-nographics']
if a.fast:cmd+=['-phase2AuditFast']
if a.mode=='complete':cmd+=['-phase2CompleteAudit']
if a.mode in ['boss','recovery','visual']:cmd+=['-phase2BossAudit']
if a.mode=='recovery':cmd+=['-phase2BossRecoveryAudit']
if a.mode=='visual':cmd+=['-phase2ArenaVisual']
if a.mode=='safety':cmd+=['-phase2SafetyAudit']
if a.no_secret:cmd+=['-phase2NoSecret']
if a.zero_victory:cmd+=['-phase2ZeroVictory']
if a.exit_fault:cmd+=['-phase2ExitFault']
if a.mode=='investigation-edges':cmd+=['-phase2InvestigationEdges']
cmd+=['-phase2Audit',str(out),'-logFile',str(out/'player.log')]
env=dict(os.environ,DISPLAY=':94',LIBGL_ALWAYS_SOFTWARE='1',LP_NUM_THREADS='3')
now=lambda:datetime.datetime.now(datetime.timezone.utc).isoformat()
r=dict(command=cmd,startedUtc=now(),status='Running',mode=a.mode,graphical=graph,fast=a.fast,environment={'DISPLAY':':94','LIBGL_ALWAYS_SOFTWARE':'1','LP_NUM_THREADS':'3'})
record=out/'invocation.json';record.write_text(json.dumps(r,indent=2)+'\n')
with (out/'console.log').open('w') as log:done=subprocess.run(cmd,cwd=ROOT,env=env,stdout=log,stderr=subprocess.STDOUT)
r.update(endedUtc=now(),status='Finished',exitCode=done.returncode);record.write_text(json.dumps(r,indent=2)+'\n')
result=out/'result.json'
if result.exists():
 d=json.loads(result.read_text());print(json.dumps({k:d.get(k) for k in ['completed','platform','gpu','walkedMeters','simulationTimeScale','rendered']},indent=2));print('CHECKS',len(d.get('checks',[])));print('\n'.join(x for x in d.get('checks',[]) if x.startswith(('FAIL','ERROR'))))
else:print('MISSING_RESULT',out)
raise SystemExit(done.returncode)
