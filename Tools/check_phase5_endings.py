"""Exercise ending inputs from explicit FINAL_CHOICE native fixtures.
This does not claim each run played the preceding investigation/boss route.
"""
import argparse,json,subprocess,sys
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
p=argparse.ArgumentParser(description=__doc__);p.add_argument('--only',nargs='+',choices=['BAD','NORMAL','TRUE','SECRET'],default=['BAD','NORMAL','TRUE','SECRET']);p.add_argument('--suffix',default='01');a=p.parse_args()
for ending in a.only:
 name=ending.title();out=ROOT/'Builds/QA'/('Phase5Ending'+name+a.suffix)
 result=out/'result.json'
 if not result.exists() or not json.loads(result.read_text()).get('completed'):
  subprocess.run([sys.executable,'Tools/run_phase5_player.py','--output',str(out),'--headless','--final',ending],cwd=ROOT,check=True)
 data=json.loads(result.read_text());assert data['completed'],data
 save=json.loads((out/'final-save.json').read_text());assert save['checkpoint_id']=='PHASE5_CP9' and save['ending']=={'BAD':1,'NORMAL':2,'TRUE':3,'SECRET':4}[ending]
 data.update(ending=ending,fixtureScope='Prepared FINAL_CHOICE at zero persistent battery, real input/escort/sync/hold/lock and cutscene restart. Not a complete campaign replay.')
 (ROOT/'Audit'/('phase5-ending-'+ending.lower()+'.json')).write_text(json.dumps(data,indent=2)+'\n')
 print('PHASE5_ENDING_PASS',ending,flush=True)
