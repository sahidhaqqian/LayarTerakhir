"""Import only selected cached CC0 bathroom/kitchen models and 1K PBR maps.
No downloads or whole-pack import. Sources were verified in the cache/publisher receipts.
"""
from pathlib import Path
import hashlib
import json
import shutil
from PIL import Image, ImageOps

ROOT=Path(__file__).resolve().parents[1]
CACHE=ROOT/'Builds/ArtSources/KosRealWorld'
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities'
OUT.mkdir(parents=True,exist_ok=True)
records=[]

def copy(source,target):
    target.parent.mkdir(parents=True,exist_ok=True)
    shutil.copyfile(source,target)
    return dict(path=target.relative_to(ROOT).as_posix(),sha256=hashlib.sha256(target.read_bytes()).hexdigest())

for name in ['Toilet_Round_A','Sink_A']:
    entry=dict(name=name,page='https://opengameart.org/content/toilets',author='loafbrr_1',license='CC0-1.0',files=[])
    folder=OUT/name
    entry['files'].append(copy(CACHE/'oga-toilets/fbx_unity'/(name+'.fbx'),folder/(name+'.fbx')))
    groups=['Toilet_Round','Toilet_Round_Seat'] if name=='Toilet_Round_A' else ['Sink_A']
    for group in groups:
        source=CACHE/'oga-toilets/Textures'/group
        if not source.exists():continue
        for kind in ['Diffuse','Normal']:
            entry['files'].append(copy(source/(group+'_'+kind+'_1K.png'),folder/(group+'_'+kind+'.png')))
        rough=Image.open(source/(group+'_Roughness_1K.png')).convert('L')
        ao=Image.open(source/(group+'_Ambient Occlusion_1K.png')).convert('L')
        metallic=source/(group+'_Metalness_1K.png')
        metal=Image.open(metallic).convert('L') if metallic.exists() else Image.new('L',rough.size,0)
        path=folder/(group+'_Mask.png')
        Image.merge('RGBA',(metal,ao,Image.new('L',rough.size,0),ImageOps.invert(rough))).save(path)
        entry['files'].append(dict(path=path.relative_to(ROOT).as_posix(),sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    records.append(entry)

for name in ['electric_stove','vintage_electric_kettle','pot_enamel_01','wooden_cutting_board','wooden_spoon','metal_jug']:
    entry=dict(name=name,page='https://polyhaven.com/a/'+name,author='Poly Haven (publisher credit in original source)',license='CC0-1.0',files=[])
    source=CACHE/'PolyHaven'/name;folder=OUT/name
    for suffix in ['.fbx','_diff_1k.jpg','_nor_gl_1k.jpg']:
        entry['files'].append(copy(source/(name+suffix),folder/(name+suffix)))
    arm=Image.open(source/(name+'_arm_1k.jpg')).convert('RGB');ao,rough,metal=arm.split()
    path=folder/(name+'_mask.png');Image.merge('RGBA',(metal,ao,Image.new('L',arm.size,0),ImageOps.invert(rough))).save(path)
    entry['files'].append(dict(path=path.relative_to(ROOT).as_posix(),sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    records.append(entry)
(ROOT/'Audit/prolog47-cc0-sources.json').write_text(json.dumps(records,indent=2)+'\n')
print('SELECTED_CC0_SOURCES',len(records))
