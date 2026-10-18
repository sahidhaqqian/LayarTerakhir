"""Small verified production sources for the Golden Scene; no bulk asset packs.
Downloads selected CC0 recordings/foley, one stool, a 1K rug material and OFL fonts.
"""
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor
from io import BytesIO
import hashlib, json, re, zipfile, requests
from PIL import Image, ImageChops

ROOT=Path(__file__).resolve().parents[1]
TMP=Path('/tmp/layar-golden-sources');TMP.mkdir(exist_ok=True)
ART=ROOT/'Assets/_LayarTerakhir/assets/Art'
AUDIO=ROOT/'Assets/_LayarTerakhir/assets/Audio/Imported';AUDIO.mkdir(parents=True,exist_ok=True)
FONT=ROOT/'Assets/_LayarTerakhir/assets/UI/Fonts';FONT.mkdir(parents=True,exist_ok=True)
SESSION=requests.Session();SESSION.headers['User-Agent']='LayarTerakhir-GoldenProduction/1.0'
records=[]
def fetch(url):
    r=SESSION.get(url,timeout=100);r.raise_for_status();return r.content
def record(url,data,path,source,author,license='CC0-1.0',note=''):
    path.parent.mkdir(parents=True,exist_ok=True);path.write_bytes(data)
    return dict(source=source,url=url,author=author,license=license,bytes=len(data),sha256=hashlib.sha256(data).hexdigest(),path=str(path.relative_to(ROOT)) if path.is_relative_to(ROOT) else str(path),note=note)

def freesound(sound_id,author,name,license='CC0-1.0'):
    page=f'https://freesound.org/people/{author}/sounds/{sound_id}/'
    html=fetch(page).decode()
    license_path='creativecommons.org/publicdomain/zero/1.0' if license=='CC0-1.0' else 'creativecommons.org/licenses/by/4.0'
    assert license_path in html, 'Unverified license: '+page
    matches=re.findall(r'https://cdn.freesound.org/previews/[^"\s<>]+-hq\.mp3',html)
    assert matches,page
    url=matches[0];data=fetch(url)
    return record(url,data,AUDIO/(name+'.mp3'),page,author,license,note='Official freely served high-quality MP3 preview; not the login-protected WAV master.')

def polyhaven(asset_id,model):
    info=json.loads(fetch('https://api.polyhaven.com/info/'+asset_id));files=json.loads(fetch('https://api.polyhaven.com/files/'+asset_id))
    folder=ART/'Imported/PolyHaven'/asset_id;folder.mkdir(parents=True,exist_ok=True)
    picks=[]
    if model:
        picks.append((asset_id+'.fbx',files['fbx']['1k']['fbx']))
        picks += [(Path(n).name,e) for n,e in files['gltf']['1k']['gltf']['include'].items() if n.endswith('.jpg') and any(c in n for c in ['_diff_','_nor_gl_','_arm_'])]
    else:
        picks=[(asset_id+'_'+c+'_1k.jpg',files[k]['1k']['jpg']) for c,k in [('diff','Diffuse'),('nor_gl','nor_gl'),('arm','arm')]]
    result=[]
    for name,entry in picks:
        data=fetch(entry['url']);assert hashlib.md5(data).hexdigest()==entry['md5']
        original=TMP/name;original.write_bytes(data)
        if name.endswith('.jpg'):
            im=Image.open(BytesIO(data)).convert('RGB');im.thumbnail((512,512) if model else (1024,1024));out=BytesIO();im.save(out,format='JPEG',quality=94);data=out.getvalue()
        result.append(record(entry['url'],data,folder/name,'https://polyhaven.com/a/'+asset_id,info['authors'],note='Source MD5 '+entry['md5']))
    arm=next(folder.glob('*_arm_*.jpg'));ao,rough,metal=Image.open(arm).convert('RGB').split()
    Image.merge('RGBA',(metal,ao,Image.new('L',ao.size,0),ImageChops.invert(rough))).save(folder/(asset_id+'_mask.png'))
    return result

tasks=[('rain',lambda:freesound(832559,'Srirachasocks','RainOnTin')),
       ('fan',lambda:freesound(219489,'JarredGibb','VentilationFan')),
       ('room',lambda:freesound(329533,'leonelmail','ApartmentRoomTone')),
       ('cloth',lambda:freesound(476804,'Sheyvan','ClothingRustle')),
       ('motorcycle',lambda:freesound(415038,'roman_cgr','MotorcycleStart','CC-BY-4.0')),
       ('stool',lambda:polyhaven('painted_wooden_stool',True)),
       ('rug',lambda:polyhaven('dirty_carpet',False))]
with ThreadPoolExecutor(max_workers=3) as pool:
    futures=[(name,pool.submit(fn)) for name,fn in tasks]
    for name,future in futures:
        value=future.result();records.extend(value if isinstance(value,list) else [value]);print('VERIFIED',name,flush=True)

for pack in ['impact-sounds','ui-audio']:
    page='https://kenney.nl/assets/'+pack
    html=fetch(page).decode();assert 'CC0' in html
    url=re.search(r'https://kenney.nl/media/[^"\s]+\.zip',html)[0]
    content=fetch(url);archive=zipfile.ZipFile(BytesIO(content))
    license_file=next(n for n in archive.namelist() if n.endswith('License.txt'))
    license_text=archive.read(license_file);assert b'CC0' in license_text
    (TMP/(pack+'.zip')).write_bytes(content)
    (AUDIO/(pack+'-License.txt')).write_bytes(license_text)
    records.append(dict(source=page,url=url,license='CC0-1.0',author='Kenney',bytes=len(content),sha256=hashlib.sha256(content).hexdigest(),note='Only selected OGG clips imported by prepare_golden_audio.py; source ZIP kept outside the Unity project.'))
    print('PACK',pack,[n for n in archive.namelist() if n.endswith('.ogg')][:22],flush=True)

for name in ['Barlow-Regular.ttf','Barlow-SemiBold.ttf','OFL.txt']:
    url='https://raw.githubusercontent.com/google/fonts/main/ofl/barlow/'+name
    records.append(record(url,fetch(url),FONT/name,'https://github.com/google/fonts/tree/main/ofl/barlow','The Barlow Project Authors','OFL-1.1'))
(ROOT/'Audit/golden-source-manifest.json').write_text(json.dumps(records,indent=2)+'\n')
print('GOLDEN_SOURCES_READY',len(records),flush=True)
