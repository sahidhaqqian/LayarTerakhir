"""Real SIGKILL probes, followed by loading the slot in a new native process.
Only isolated development players launched by this script are terminated.
"""
import json,subprocess,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
records=[]
for action in ['core','ending']:
 for point in ['temp-flushed','replaced']:
  out=ROOT/'Builds/QA/Phase5-Crash'/f'{action}-{point}'
  assert not out.exists(),f'Preserve earlier evidence or choose a fresh output: {out}'
  args=[sys.executable,'Tools/run_phase5_player.py','--output',str(out),'--headless','--probe',action,'--point',point]
  crashed=subprocess.run(args,cwd=ROOT,stdout=subprocess.DEVNULL).returncode
  assert crashed!=0 and (out/'kill-reached.txt').read_text()==point+' '+action,(action,point,crashed)
  (out/'invocation.json').rename(out/'killed-invocation.json')
  subprocess.run(args+['--verify'],cwd=ROOT,check=True,stdout=subprocess.DEVNULL)
  verified=json.loads((out/'verified.json').read_text());assert verified['passed'],verified
  records.append(dict(action=action,point=point,crashExit=crashed,marker=(out/'kill-reached.txt').read_text(),verification=verified))
  print('P5_CRASH_PROBE_PASS',action,point,flush=True)
report=dict(passed=True,method='Actual isolated native player process termination after flushed temp or after atomic replace; fresh native loader verification. Not physical gameplay inputs.',cases=records)
(ROOT/'Audit/phase5-crash-probes.json').write_text(json.dumps(report,indent=2)+'\n')
