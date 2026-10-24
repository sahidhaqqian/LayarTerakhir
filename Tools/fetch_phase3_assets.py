"""New licensed Phase 3 industrial library, with author/source/checksum receipts.
No previous chapter asset directories are scanned or copied.
"""
import concurrent.futures, gzip, hashlib, json, re
from pathlib import Path
import requests
ROOT=Path(__file__).resolve().parents[1]
SOURCE=ROOT/'Builds/ArtSources/Phase3'
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase3/Imported'
SOURCE.mkdir(parents=True,exist_ok=True);OUT.mkdir(parents=True,exist_ok=True)
MODELS='''modular_factory_facade modular_fire_escape modular_wooden_pier modular_industrial_pipes_01 overhead_crane portable_generator portable_searchlight hanging_industrial_lamp caged_hanging_light industrial_caged_sconce industrial_wall_lamp hand_truck old_military_compressor drill_press_01 bench_vice_01 steel_frame_shelves_01 steel_frame_shelves_02 metal_toolbox pipe_wrench water_manhole_cover power_box_01 utility_box_01 retro_multimeter wooden_crate_02 wooden_barrels_01 barrel_stove oil_tin small_oil_can_01 metal_jerrycan adjustable_wrench cross_pein_hammer pliers portable_welding_cart office_notepads industrial_pastic_container industrial_storage_cart industrial_coffee_table vintage_radio_transceiver rusted_spade_01 old_military_crate vintage_hand_drill'''.split()
HDRIS=['moonless_golf']
TEXTURES='factory_brick factory_wall corrugated_iron_02 metal_grate_rusty brown_planks_09 wood_chips concrete_floor_02 red_brick_03'.split()
def get(url,path):
 path.parent.mkdir(parents=True,exist_ok=True)
 if not path.exists():
  r=requests.get(url,timeout=180);r.raise_for_status();tmp=path.with_suffix(path.suffix+'.download');tmp.write_bytes(r.content);tmp.replace(path)
 return hashlib.sha256(path.read_bytes()).hexdigest()
def ph(name):
 p=SOURCE/(name+'.json');get('https://api.polyhaven.com/files/'+name,p);m=json.loads(p.read_text())
 folder=OUT/'PolyHaven'/name;folder.mkdir(parents=True,exist_ok=True)
 files=[]
 if name in HDRIS:
  entry=m['hdri']['2k']['hdr'];dest=folder/(name+'.hdr');h=get(entry['url'],dest);files.append(dict(file=str(dest.relative_to(ROOT)),url=entry['url'],sha256=h))
 if name in MODELS:
  entry=m['fbx']['2k']['fbx'];dest=folder/(name+'.fbx');h=get(entry['url'],dest);files.append(dict(file=str(dest.relative_to(ROOT)),url=entry['url'],sha256=h))
 # All material groups: multi-material facade/pipes cannot use one generic colour.
 for key,v in m.items():
  if not (key in ['Diffuse','diff','nor_gl','arm','Alpha'] or key.endswith(('_diff','_nor_gl','_arm','_alpha'))):continue
  if '2k' not in v:continue
  e=v['2k'];ext='jpg' if 'jpg' in e else 'png';item=e[ext];dest=folder/(name+'_'+key+'.'+ext)
  h=get(item['url'],dest);files.append(dict(file=str(dest.relative_to(ROOT)),kind=key,url=item['url'],sha256=h))
 print('FETCHED',name,len(files),flush=True)
 return dict(id=name,kind='model' if name in MODELS else 'hdri' if name in HDRIS else 'texture',sourcePage='https://polyhaven.com/a/'+name,license='CC0-1.0',licenseUrl='https://polyhaven.com/license',files=files)
def obj(uid,name):
 path=SOURCE/'object-paths.json.gz';get('https://huggingface.co/datasets/allenai/objaverse/resolve/main/object-paths.json.gz',path)
 paths=json.loads(gzip.decompress(path.read_bytes()))
 if uid not in paths:raise RuntimeError('Licensed mirror missing '+name)
 part=paths[uid].split('/')[1];p=SOURCE/(part+'.json.gz');get('https://huggingface.co/datasets/allenai/objaverse/resolve/main/metadata/'+part+'.json.gz',p)
 meta=json.loads(gzip.decompress(p.read_bytes()))[uid];assert meta['license'] in ['by','cc0'],meta['license']
 url='https://huggingface.co/datasets/allenai/objaverse/resolve/main/'+paths[uid];h=get(url,SOURCE/(name+'.glb'))
 print('FETCHED',name,meta['name'],flush=True)
 return dict(id=name,kind='character' if name=='Wewe' else 'model',author=meta['user']['displayName'],title=meta['name'],sourcePage=meta['viewerUrl'],mirror=url,license='CC-BY-4.0' if meta['license']=='by' else 'CC0-1.0',sourceSha256=h)
def main():
 records=[];failures=[]
 with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
  jobs={pool.submit(ph,n):n for n in MODELS+TEXTURES+HDRIS}
  for f in concurrent.futures.as_completed(jobs):
   try:records.append(f.result())
   except Exception as e:failures.append(dict(id=jobs[f],error=str(e)));print('FAILED',jobs[f],str(e),flush=True)
 for uid,name in [('a8cee601a6fe40f18c774262e3363461','Wewe'),('74f60f70effb486faf645519b3dae1a9','Sack'),('0dafa7aaadfd4666bb4a32776c84b14e','IndustrialBuilding'),('e0245d6f01f141f9812582a9487c3062','Doll'),('884ed11e38054aa48a3db1d9025c881d','Boiler'),('25924b6dc3a841e4b1a6ebdef52e3173','Sickle'),('35012fef3a9c41289cc3cb4771f8c829','Locker'),('e3e85978a98444bdb483ea79f2395407','Cloth'),('2beeab5f44704421bc10cd310db96860','MineCart'),('29a5ed053eab46a7af308215600985a2','Shoe'),('04ec66aa997942aa911d2d4508c7dafe','Chimneys'),('bb92a0e2e5e04ed4a62a51dde6cb854a','Gear')]:
  try:records.append(obj(uid,name))
  except Exception as e:failures.append(dict(id=name,error=str(e)));print('FAILED',name,str(e),flush=True)
 name='RajahCircle';dest=OUT/'Decals/RajahCircle.png';url='https://opengameart.org/sites/default/files/circle4.png';h=get(url,dest)
 records.append(dict(id=name,kind='image',author='Luke.RUSTLTD',title='4 summoning circles — circle4',sourcePage='https://opengameart.org/content/4-summoning-circles',license='CC0-1.0',files=[dict(file=str(dest.relative_to(ROOT)),url=url,sha256=h)]))
 report=dict(discovery=dict(google='https://www.google.com/search?q=free+abandoned+factory+industrial+boiler+3d+models+CC0+CC+BY',googleResponseSnapshot='Builds/ArtSources/Phase3/google-discovery.html'),sources=sorted(records,key=lambda x:x['id']),failures=failures)
 (ROOT/'Audit/phase3-asset-sources.json').write_text(json.dumps(report,indent=2)+'\n')
 print('PHASE3_ASSETS',len(records),'failures',len(failures),flush=True)
if __name__=='__main__':main()
