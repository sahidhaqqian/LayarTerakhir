"""Second exterior/shared batch for the Phase 1 visual overhaul (CC0 Poly Haven).
Appends records to Audit/phase1-overhaul-asset-sources.json."""
import concurrent.futures
import hashlib
import json
from pathlib import Path

import requests
from PIL import Image, ImageOps

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase1/Imported'
IDS = ('covered_car CoffeeCart_01 planter_box_03 garden_hose_wall_mounted_01 modular_street_seating '
       'outdoor_table_chair_set_01 watering_can_metal_01 seeding_tray_01 planter_pot_clay '
       'old_military_crate wooden_military_crate compost_bag_02').split()
HEADERS = {'User-Agent': 'Mozilla/5.0 LayarTerakhir Offline Asset Import'}
catalog = json.loads(Path('/tmp/opencode/ph-models.json').read_text())


def metadata(name):
    try:
        r = requests.get('https://api.polyhaven.com/files/' + name, headers=HEADERS, timeout=120)
        r.raise_for_status(); data = r.json()
    except Exception as e:
        print('SKIP_FETCH', name, e, flush=True); return None
    if not all(any(k in data for k in keys) for keys in [('Diffuse', 'diff'), ('nor_gl',), ('arm',)]) or 'fbx' not in data:
        print('SKIP_MULTIMATERIAL', name, flush=True); return None
    return name, data


with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
    selected = [i for i in pool.map(metadata, IDS) if i]


def download(item):
    name, data = item; folder = OUT / name; folder.mkdir(parents=True, exist_ok=True)
    wanted = [(name + '.fbx', data['fbx']['1k']['fbx'])]
    for slot, keys in [('Diffuse', ['Diffuse', 'diff']), ('Normal', ['nor_gl']), ('ARM', ['arm'])]:
        key = next(k for k in keys if k in data); entry = data[key]['1k']
        wanted.append((name + slot + '.' + ('jpg' if 'jpg' in entry else 'png'), entry['jpg' if 'jpg' in entry else 'png']))
    files = []
    for filename, entry in wanted:
        path = folder / filename
        if not path.exists():
            r = requests.get(entry['url'], headers=HEADERS, timeout=180); r.raise_for_status(); path.write_bytes(r.content)
        payload = path.read_bytes()
        assert not entry.get('md5') or hashlib.md5(payload).hexdigest() == entry['md5']
        files.append(dict(file=str(path.relative_to(ROOT)), url=entry['url'], sha256=hashlib.sha256(payload).hexdigest()))
    ao, rough, metal = Image.open(next(p for p in folder.glob(name + 'ARM.*') if p.suffix != '.meta')).convert('RGB').split()
    empty = Image.new('L', ao.size)
    Image.merge('RGBA', (metal, empty, empty, ImageOps.invert(rough))).save(folder / (name + 'MetalSmooth.png'))
    Image.merge('RGB', (ao, ao, ao)).save(folder / (name + 'Occlusion.png'))
    print('OVERHAUL_PROP2', name, flush=True)
    return dict(id=name, sourcePage='https://polyhaven.com/a/' + name, license='CC0-1.0',
                licenseUrl='https://polyhaven.com/license', authors=catalog[name].get('authors'),
                sourceTriangles=catalog[name].get('polycount'), files=files)


with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
    records = list(pool.map(download, selected))
manifest_path = ROOT / 'Audit/phase1-overhaul-asset-sources.json'
manifest = json.loads(manifest_path.read_text())
known = {r['id'] for r in manifest['records']}
manifest['records'] += [r for r in records if r['id'] not in known]
manifest['downloadedDistinctModels'] = len(manifest['records'])
manifest_path.write_text(json.dumps(manifest, indent=2) + '\n')
print('PHASE1_OVERHAUL_BATCH2', len(records), 'TOTAL', manifest['downloadedDistinctModels'])
