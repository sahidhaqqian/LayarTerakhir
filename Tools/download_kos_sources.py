from pathlib import Path
import requests,json,hashlib,concurrent.futures,shutil
ROOT=Path('/tmp/kos-asset-sources');ROOT.mkdir(exist_ok=True)
S=requests.Session();S.headers['User-Agent']='LayarTerakhir-EnvironmentProduction/1.0'
models=['vintage_day_bed','wooden_table_02','painted_wooden_chair_01','painted_wooden_cabinet_02','wooden_bookshelf_worn','plastic_bottle_gallon','desk_lamp_arm_01','binder_notebook']
textures=['worn_plaster_wall','painted_plaster_wall','floor_tiles_04','wood_table_worn','denim_fabric','rusty_metal']
meta=json.loads(Path('/tmp/polyhaven-models.json').read_text())
meta.update(json.loads((ROOT/'ph-textures.json').read_text()))
def download(url,path,md5=None):
 path.parent.mkdir(parents=True,exist_ok=True)
 r=S.get(url,timeout=100);r.raise_for_status()
 if md5: assert hashlib.md5(r.content).hexdigest()==md5,(url,'hash mismatch')
 path.write_bytes(r.content)
 return {'url':url,'localSource':str(path.relative_to(ROOT)),'bytes':len(r.content),'sourceMd5':md5,'sha256':hashlib.sha256(r.content).hexdigest()}
def asset(i,is_model):
 cached=Path('/tmp/ph-'+i+'.json')
 if cached.exists(): d=json.loads(cached.read_text())
 else:
  r=S.get('https://api.polyhaven.com/files/'+i,timeout=40);r.raise_for_status();d=r.json()
 (ROOT/'metadata').mkdir(exist_ok=True)
 (ROOT/'metadata'/f'{i}.json').write_text(json.dumps(d,indent=2))
 folder=ROOT/'PolyHaven'/i;files=[]
 if is_model:
  f=d['fbx']['1k']['fbx'];files.append(download(f['url'],folder/(i+'.fbx'),f.get('md5')))
  includes=d['gltf']['1k']['gltf']['include']
  for name,f in includes.items():
   if name.lower().endswith(('.jpg','.png')):
    files.append(download(f['url'],folder/Path(name).name,f.get('md5')))
 else:
  for channel in ['diff','nor_gl','arm']:
   f=d[{'diff':'Diffuse'}.get(channel,channel)]['1k']['jpg'];files.append(download(f['url'],folder/(i+'_'+channel+'_1k.jpg'),f.get('md5')))
 record={'id':i,'name':meta[i]['name'],'authors':meta[i].get('authors',{}),'sourcePage':'https://polyhaven.com/a/'+i,'license':'CC0-1.0','licenseUrl':'https://polyhaven.com/license','kind':'model' if is_model else 'PBR texture','sourcePolygons':meta[i].get('polycount'),'files':files}
 print('Downloaded',i,sum(f['bytes'] for f in files),'bytes',flush=True)
 return record
with concurrent.futures.ThreadPoolExecutor(max_workers=3) as e:
 futures=[e.submit(asset,i,True) for i in models]+[e.submit(asset,i,False) for i in textures]
 records=[f.result() for f in futures]
(ROOT/'download-manifest.json').write_text(json.dumps(records,indent=2)+'\n')
print('COMPLETE',len(records),'verified CC0 assets',flush=True)
