"""Download licensed Phase 1 environment sources and convert texture channels only.

No modeling, texture painting, image generation or Prolog environment asset reuse.
The publisher archives are retained outside Assets; selected source files retain UVs.
"""
import concurrent.futures
import hashlib
import json
import shutil
import zipfile
from pathlib import Path

import requests
from PIL import Image, ImageOps

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/Phase1External'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase1/Imported'
records = []


def download(url, path):
    path.parent.mkdir(parents=True, exist_ok=True)
    if not path.exists():
        response = requests.get(url, headers={'User-Agent': 'Mozilla/5.0'}, timeout=180)
        response.raise_for_status()
        path.write_bytes(response.content)
    return path


def channels(folder, prefix, rough, metallic=None, ao=None):
    rough = Image.open(rough).convert('L')
    metal = Image.open(metallic).convert('L') if metallic else Image.new('L', rough.size)
    empty = Image.new('L', rough.size)
    Image.merge('RGBA', (metal, empty, empty, ImageOps.invert(rough))).save(folder / (prefix+'MetalSmooth.png'))
    if ao:
        Image.open(ao).convert('RGB').save(folder / (prefix+'Occlusion.png'))


def publisher_pack(name, url, page, author):
    archive = download(url, CACHE / (name+'.zip'))
    extracted = CACHE / name
    if not extracted.exists():
        zipfile.ZipFile(archive).extractall(extracted)
    records.append(dict(id=name, sourcePage=page, author=author, license='CC0-1.0',
                        download=url, archiveSha256=hashlib.sha256(archive.read_bytes()).hexdigest()))
    return extracted


kit = publisher_pack('ConcreteInterior', 'https://opengameart.org/sites/default/files/modular_concrete_interior.zip',
                     'https://axel-kulomaa.itch.io/modular-concrete-interior', 'Axel Kulomaa / AXLplosion')
source = kit / 'Modular Concrete Interior'
dest = OUT / 'ConcreteInterior'
dest.mkdir(parents=True, exist_ok=True)
shutil.copy2(source / 'readme.txt', dest / 'License.txt')
for path in (source / 'Assets/Meshes/Modular').glob('*.fbx'):
    shutil.copy2(path, dest / path.name)
for category, prefix in [('Wall', 'ConcreteWall'), ('Floor', 'ConcreteFloor'), ('Metal', 'RustyMetal')]:
    textures = source / 'Assets/Textures' / category
    for slot, suffix in [('Diffuse', 'Color'), ('Normal', 'Normal')]:
        path = next(textures.glob(prefix+'_'+suffix+'_2k.*'))
        shutil.copy2(path, dest / (category+slot+path.suffix))
    channels(dest, category, next(textures.glob(prefix+'_Roughness_2k.*')),
             next(textures.glob(prefix+'_Metallic_2k.*'), None))

doors = publisher_pack('DoorsWindows', 'https://opengameart.org/sites/default/files/doorsandwindows_fbx_gltf_textures_blend.zip',
                       'https://opengameart.org/content/breakable-doors-and-windows', 'loafbrr_1')
dest = OUT / 'DoorsWindows'
dest.mkdir(exist_ok=True)
shutil.copy2(doors/'README.txt', dest/'License.txt')
for path in (doors/'FBXs').glob('*.fbx'):
    shutil.copy2(path, dest/path.name)
textures = doors/'Textures/BreakableEntrances'
for source_name, target in [('BASE_COLOR_1_BreakableEntrances.png', 'DoorDiffuse.png'),
                            ('NORMAL_BreakableEntrances.png', 'DoorNormal.png')]:
    shutil.copy2(textures/source_name, dest/target)
channels(dest, 'Door', textures/'ROUGHNESS_BreakableEntrances.png', textures/'METALLIC_BreakableEntrances.png', textures/'AO_BreakableEntrances.png')

catalog = json.loads(Path('/tmp/lt-p1-polyhaven-models.json').read_text())
models = ['metal_office_desk', 'painted_wooden_bench', 'folding_wooden_stool',
          'worn_metal_rack', 'metal_trash_can', 'trashbag', 'modern_wooden_cabinet']
surfaces = ['worn_cracked_plaster', 'worn_mossy_plasterwall', 'blue_floor_tiles_01', 'asphalt_02', 'rust_coarse_01']


def polyhaven(item):
    model = item in models
    metadata = requests.get('https://api.polyhaven.com/files/'+item, headers={'User-Agent':'Mozilla/5.0'}, timeout=90)
    metadata.raise_for_status()
    data = metadata.json()
    folder = OUT / (item if model else 'Surfaces/'+item)
    folder.mkdir(parents=True, exist_ok=True)
    resolution = '1k' if model else '2k'
    prefix = '' if 'diff' in data or 'Diffuse' in data else 'body_'
    wanted = []
    if model:
        wanted.append((item+'.fbx', data['fbx'][resolution]['fbx']))
    for slot, candidates in [('Diffuse', ['diff', 'Diffuse', prefix+'diff']), ('Normal', ['nor_gl', prefix+'nor_gl']), ('ARM', ['arm', prefix+'arm'])]:
        key = next(k for k in candidates if k in data)
        entry = data[key][resolution]
        extension = 'jpg' if 'jpg' in entry else 'png'
        wanted.append((item+slot+'.'+extension, entry[extension]))
    files = []
    for filename, entry in wanted:
        path = download(entry['url'], folder/filename)
        assert not entry.get('md5') or hashlib.md5(path.read_bytes()).hexdigest() == entry['md5']
        files.append(dict(file=str(path.relative_to(ROOT)), url=entry['url'], sha256=hashlib.sha256(path.read_bytes()).hexdigest()))
    arm = next(p for p in folder.glob(item+'ARM.*') if p.suffix != '.meta')
    ao, rough, metal = Image.open(arm).convert('RGB').split()
    empty = Image.new('L', ao.size)
    Image.merge('RGBA', (metal, empty, empty, ImageOps.invert(rough))).save(folder/(item+'MetalSmooth.png'))
    Image.merge('RGB', (ao, ao, ao)).save(folder/(item+'Occlusion.png'))
    print('ENVIRONMENT_DOWNLOADED', item, flush=True)
    return dict(id=item, sourcePage='https://polyhaven.com/a/'+item, license='CC0-1.0',
                licenseUrl='https://polyhaven.com/license', authors=catalog.get(item, {}).get('authors'), files=files)


with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
    records.extend(pool.map(polyhaven, models+surfaces))
for record in records[:2]:
    folder = OUT/record['id']
    record['importedFiles'] = [dict(file=str(p.relative_to(ROOT)), sha256=hashlib.sha256(p.read_bytes()).hexdigest())
                               for p in sorted(folder.iterdir()) if p.is_file() and p.suffix != '.meta']
(ROOT/'Audit/phase1-environment-sources.json').write_text(json.dumps(records, indent=2)+'\n')
print('PHASE1_ENVIRONMENT_SOURCES_READY', len(records))
