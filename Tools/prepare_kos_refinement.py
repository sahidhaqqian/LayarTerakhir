"""Import only three approved staging props, verifying original bytes against Poly Haven.
No new asset search/downloads. Original FBX/JPG bytes are copied unchanged; derived URP mask
packs R=metallic, G=AO, A=1-roughness. Unity limits the runtime textures to 512px.
"""
from concurrent.futures import ThreadPoolExecutor
from datetime import datetime, timezone
from pathlib import Path
import hashlib, json, shutil
import requests
from PIL import Image, ImageChops

ROOT=Path(__file__).resolve().parents[1]
STAGE=ROOT/'ArtSources/AssetResearch'
DEST=ROOT/'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven'
SELECTED=('office_notepads','plastic_thermos','gamepad')
records=json.loads((STAGE/'manifest/source-manifest.json').read_text())

def prepare(asset_id):
    record=next(r for r in records if r['id']==asset_id)
    session=requests.Session();session.headers['User-Agent']='LayarTerakhir-KosFinalRefinement/1.0'
    live=session.get('https://api.polyhaven.com/files/'+asset_id,timeout=45)
    live.raise_for_status();files=live.json()
    expected={asset_id+'_1k.fbx':files['fbx']['1k']['fbx']}
    expected.update({Path(n).name:e for n,e in files['gltf']['1k']['gltf']['include'].items() if n.endswith('.jpg')})
    selected=[e for e in record['files'] if e['name'].endswith('.fbx') or any(s in e['name'] for s in ('_diff_','_nor_gl_','_arm_'))]
    assert len(selected)==4 and record['license']=='CC0-1.0'
    folder=DEST/asset_id;folder.mkdir(parents=True,exist_ok=True);checks=[]
    for entry in selected:
        source=STAGE/'assets'/entry['file'];data=source.read_bytes();sha=hashlib.sha256(data).hexdigest()
        assert len(data)==entry['bytes'] and sha==entry['sha256'],source
        assert hashlib.md5(data).hexdigest()==expected[entry['name']]['md5'],source
        target=folder/entry['name']
        if target.exists():assert target.read_bytes()==data,'Refusing to overwrite a changed asset: '+str(target)
        else:shutil.copy2(source,target)
        checks.append(dict(path=str(target.relative_to(ROOT)),sha256=sha,bytes=len(data),sourceMd5Verified=True,url=expected[entry['name']]['url']))
    arm=Image.open(folder/(asset_id+'_arm_1k.jpg')).convert('RGB');arm.thumbnail((512,512))
    ao,rough,metal=arm.split()
    mask=folder/(asset_id+'_mask.png')
    Image.merge('RGBA',(metal,ao,Image.new('L',ao.size,0),ImageChops.invert(rough))).save(mask)
    return dict(id=asset_id,sourcePage=record['sourcePage'],authors=record['authors'],license='CC0-1.0',
                licenseUrl=record['licenseUrl'],attributionRequired=False,reviewer='Codex environment integration',
                originalFiles=checks,derivedMask=str(mask.relative_to(ROOT)),runtimeTextureMaximum=512,
                note='Inspected exact FBX in Blender and local preview; source/license page independently checked. Only a subset of the notepad meshes is placed.')

with ThreadPoolExecutor(max_workers=3) as pool:results=list(pool.map(prepare,SELECTED))
receipt=dict(utc=datetime.now(timezone.utc).isoformat(),assets=results,
             rejectedNotImported=['industrial_pastic_container','retro_multimeter','cassette_player'])
(ROOT/'Audit/kos-final-source-verification.json').write_text(json.dumps(receipt,indent=2)+'\n')
for r in results:print('VERIFIED_STAGED_IMPORT',r['id'],len(r['originalFiles']))
