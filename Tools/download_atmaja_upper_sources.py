from pathlib import Path
import requests,json,hashlib,concurrent.futures,time
ROOT=Path('/tmp/atmaja-upper-sources')
if Path(__file__).exists():
    OUT=json.loads((Path(__file__).parent.parent/'Prolog.MD').read_text('utf-8')[:0] or 'null')
(ROOT).mkdir(exist_ok=True)
S=requests.Session();S.headers['User-Agent']='LayarTerakhir-UpperFloorStaging/1.0'
models=[
 'hanging_picture_frame_01','hanging_picture_frame_03','fancy_picture_frame_01','fancy_picture_frame_02',
 'ornate_mirror_01','painted_wooden_shelves','painted_wooden_nightstand','old_bed_frame','vintage_cabinet_01',
 'Chandelier_01','Lantern_01','pot_enamel_01','alarm_clock_01','vintage_wooden_drawer_01','wooden_stool_01',
 'Rockingchair_01','throw_pillows_01','wooden_crate_01','wooden_crate_02','steel_frame_shelves_02',
 'book_encyclopedia_set_01','vintage_video_camera','wooden_candlestick','metal_tool_chest','vintage_oil_lamp'
]
textures=['decrepit_wallpaper','old_wood_floor','dirty_tiles','quatrefoil_jacquard_fabric','stained_pine','ceiling_interior','painted_concrete']
meta={}
def load_meta(kind):
    p=Path(f'/tmp/ph-{kind}-full.json')
    if not p.exists():
        d=S.get('https://api.polyhaven.com/assets?t='+kind,timeout=60).json()
        p.write_text(json.dumps(d))
        time.sleep(0.5)
    return json.loads(p.read_text())
meta.update(load_meta('models'));meta.update(load_meta('textures'))
def download(url,path,md5=None):
    path.parent.mkdir(parents=True,exist_ok=True)
    r=S.get(url,timeout=120);r.raise_for_status()
    if md5: assert hashlib.md5(r.content).hexdigest()==md5,(url,'md5 mismatch')
    path.write_bytes(r.content)
    return {'url':url,'localSource':str(path.relative_to(ROOT)),'bytes':len(r.content),'sourceMd5':md5,'sha256':hashlib.sha256(r.content).hexdigest()}
def asset(i,is_model):
    d=S.get('https://api.polyhaven.com/files/'+i,timeout=60).json()
    (ROOT/'metadata').mkdir(exist_ok=True)
    (ROOT/'metadata'/f'{i}.json').write_text(json.dumps(d,indent=2))
    folder=ROOT/'PolyHaven'/i;files=[]
    if is_model:
        f=d['fbx']['1k']['fbx'];files.append(download(f['url'],folder/(i+'.fbx'),f.get('md5')))
        inc=d['gltf']['1k']['gltf']['include']
        for name,fi in inc.items():
            if name.lower().endswith(('.jpg','.png')):
                files.append(download(fi['url'],folder/Path(name).name,fi.get('md5')))
    else:
        for channel,key in [('diff','Diffuse'),('nor','nor_gl'),('arm','arm')]:
            f=d[key]['1k']['jpg'];files.append(download(f['url'],folder/(i+'_'+key+'_1k.jpg'),f.get('md5')))
    m=meta[i]
    record={'id':i,'name':m['name'],'authors':m.get('authors',{}),'sourcePage':'https://polyhaven.com/a/'+i,
            'license':'CC0-1.0','licenseUrl':'https://polyhaven.com/license','kind':'model' if is_model else 'PBR texture',
            'sourcePolygons':m.get('polycount'),'sourceDimensions':m.get('dimensions'),
            'files':files}
    print('Downloaded',i,sum(f['bytes'] for f in files),'bytes',flush=True)
    return record
with concurrent.futures.ThreadPoolExecutor(max_workers=3) as e:
    futures=[e.submit(asset,i,True) for i in models]+[e.submit(asset,i,False) for i in textures]
    records=[f.result() for f in futures]
(ROOT/'download-manifest.json').write_text(json.dumps(records,indent=2)+'\n')
lic=S.get('https://polyhaven.com/license',timeout=60);(ROOT/'evidence').mkdir(exist_ok=True)
(ROOT/'evidence'/'polyhaven-license.html').write_text(lic.text)
total=sum(r['files'][0]['bytes'] for r in records)
print('COMPLETE',len(records),'CC0 verified assets, first-file bytes',total,flush=True)
