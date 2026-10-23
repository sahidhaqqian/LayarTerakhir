"""Fetch 30 additional CC0 prop models. No textures/models are authored here."""
import concurrent.futures
import hashlib
import json
from pathlib import Path

import requests
from PIL import Image, ImageOps

ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Phase1/Imported'
IDS='medical_box stationery_supplies wall_clock small_plastic_torch metal_toolbox utility_box_01 adjustable_wrench combination_wrench cross_pein_hammer measuring_tape_01 cement_bag plastic_crate_02 cardboard_box_01 bleach_bottle plastic_broom brass_pan_01 ceramic_pot vintage_electric_kettle wooden_cutting_board carved_wooden_plate metal_jug rubber_boots vintage_suitcase ceiling_fan fire_alarm small_lpg_tank oil_tin metal_jerrycan_green wicker_basket_01 folding_wooden_stool metal_stool_03 potted_plant_04'.split()
HEADERS={'User-Agent':'Mozilla/5.0 LayarTerakhir Offline Asset Import'}
catalog=json.loads(Path('/tmp/lt-p1-polyhaven-models.json').read_text())

def metadata(name):
    response=requests.get('https://api.polyhaven.com/files/'+name,headers=HEADERS,timeout=120);response.raise_for_status()
    data=response.json()
    if not all(any(k in data for k in keys) for keys in [('Diffuse','diff'),('nor_gl',),('arm',)]):
        print('SKIP_MULTIMATERIAL',name,flush=True);return None
    return name,data

with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
    selected=[item for item in pool.map(metadata,IDS) if item][:30]
assert len(selected)==30, 'Need 30 usable independent models, not repeated props or texture channels'

def download(item):
    name,data=item;folder=OUT/name;folder.mkdir(parents=True,exist_ok=True)
    wanted=[(name+'.fbx',data['fbx']['1k']['fbx'])]
    for slot,keys in [('Diffuse',['Diffuse','diff']),('Normal',['nor_gl']),('ARM',['arm'])]:
        key=next(k for k in keys if k in data);entry=data[key]['1k'];extension='jpg' if 'jpg' in entry else 'png'
        wanted.append((name+slot+'.'+extension,entry[extension]))
    files=[]
    for filename,entry in wanted:
        path=folder/filename
        if not path.exists():
            response=requests.get(entry['url'],headers=HEADERS,timeout=180);response.raise_for_status();path.write_bytes(response.content)
        payload=path.read_bytes();assert not entry.get('md5') or hashlib.md5(payload).hexdigest()==entry['md5']
        files.append(dict(file=str(path.relative_to(ROOT)),url=entry['url'],sha256=hashlib.sha256(payload).hexdigest()))
    ao,rough,metal=Image.open(next(p for p in folder.glob(name+'ARM.*') if p.suffix!='.meta')).convert('RGB').split();empty=Image.new('L',ao.size)
    Image.merge('RGBA',(metal,empty,empty,ImageOps.invert(rough))).save(folder/(name+'MetalSmooth.png'))
    Image.merge('RGB',(ao,ao,ao)).save(folder/(name+'Occlusion.png'))
    print('NEW_DRESSING_PROP',name,flush=True)
    return dict(id=name,sourcePage='https://polyhaven.com/a/'+name,license='CC0-1.0',licenseUrl='https://polyhaven.com/license',
                authors=catalog[name].get('authors'),sourceTriangles=catalog[name].get('polycount'),files=files)

with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
    records=list(pool.map(download,selected))
(ROOT/'Audit/phase1-additional-prop-sources.json').write_text(json.dumps(dict(minimumDistinctModels=30,downloadedDistinctModels=len(records),
    attribution='Powered by Poly Haven',records=records),indent=2)+'\n')
print('PHASE1_THIRTY_ADDITIONAL_MODELS_DOWNLOADED',len(records))
