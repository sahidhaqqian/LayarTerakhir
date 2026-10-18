"""Selective CC0 additions, with API metadata, source hashes and 1K/512px PBR maps.
Run with /usr/bin/python3 Tools/download_kos_v2.py. No game runtime API dependency.
"""
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor
import hashlib, json, requests
from PIL import Image, ImageChops

ROOT = Path(__file__).resolve().parents[1]
ART = ROOT / 'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven'
SOURCE = Path('/tmp/kos-v2-sources')
SOURCE.mkdir(exist_ok=True)
session = requests.Session()
session.headers['User-Agent'] = 'LayarTerakhir-EnvironmentProduction/2.0'

def fetch(url):
    response = session.get(url, timeout=120)
    response.raise_for_status()
    return response

def asset(asset_id, model):
    info = fetch('https://api.polyhaven.com/info/' + asset_id).json()
    files = fetch('https://api.polyhaven.com/files/' + asset_id).json()
    folder = ART / asset_id
    folder.mkdir(parents=True, exist_ok=True)
    downloads = []
    selections = []
    if model:
        selections.append((asset_id + '.fbx', files['fbx']['1k']['fbx']))
        includes = files['gltf']['1k']['gltf']['include']
        selections += [(Path(name).name, entry) for name, entry in includes.items()
                       if name.endswith(('.png', '.jpg')) and any(channel in name for channel in ['_diff_', '_nor_gl_', '_arm_'])]
    else:
        diffuse_key = 'Diffuse' if 'Diffuse' in files else 'col_1'
        selections += [(asset_id + '_' + channel + '_1k.jpg', files[key]['1k']['jpg'])
                       for channel, key in [('diff', diffuse_key), ('nor_gl', 'nor_gl'), ('arm', 'arm')]]
    for name, entry in selections:
        content = fetch(entry['url']).content
        assert hashlib.md5(content).hexdigest() == entry['md5'], entry['url']
        source = SOURCE / name
        source.write_bytes(content)
        target = folder / name
        if name.endswith('.fbx'):
            target.write_bytes(content)
        else:
            with Image.open(source) as im:
                im = im.convert('RGB')
                im.thumbnail((512, 512) if model else (1024, 1024))
                im.save(target, quality=94)
        downloads.append({'url': entry['url'], 'sourceMd5': entry['md5'],
                          'sourceSha256': hashlib.sha256(content).hexdigest(), 'sourceBytes': len(content),
                          'importedPath': str(target.relative_to(ROOT)),
                          'importedSha256': hashlib.sha256(target.read_bytes()).hexdigest()})
    arm = next(folder.glob('*_arm_*'))
    with Image.open(arm) as im:
        ao, rough, metal = im.convert('RGB').split()
        Image.merge('RGBA', (metal, ao, Image.new('L', im.size, 0), ImageChops.invert(rough))).save(folder / (asset_id + '_mask.png'))
    record = {'id': asset_id, 'name': info['name'], 'authors': info.get('authors'),
              'sourcePage': 'https://polyhaven.com/a/' + asset_id, 'license': 'CC0-1.0',
              'licenseUrl': 'https://polyhaven.com/license', 'kind': 'model' if model else 'PBR texture',
              'sourceTriangles': info.get('polycount'), 'maxTextureSize': 512 if model else 1024,
              'downloads': downloads}
    print('VERIFIED', asset_id, sum(d['sourceBytes'] for d in downloads), flush=True)
    return record

with ThreadPoolExecutor(max_workers=3) as pool:
    records = list(pool.map(lambda args: asset(*args), [
        ('plastic_container', True), ('wicker_basket_02', True), ('fabric_pattern_07', False)]))
(ROOT / 'Audit/kos-v2-asset-manifest.json').write_text(json.dumps(records, indent=2) + '\n')
