"""Fetch realistic CC0 bathroom/kitchen assets from the internet for the kos.

Sources (all CC0-1.0, verified on the download page):
- OpenGameArt: toilets, freestand sink, oven+fridge, bathroom set,
  kitchen stuff, Brazilian electric shower (chuveiro).
- PolyHaven (API): photorealistic stove, kettle, pots, shelves, dining
  set, trash can, plunger, cleaner, jug, cutting board, spoon, hose tap.

Archives stay under Builds/ArtSources/KosRealWorld (git-ignored inputs);
only hand-picked FBX/OBJ + textures are later copied into
Assets/_LayarTerakhir/assets/Art/Imported/KosRealWorld. Provenance goes to
Audit/kos-realworld-source.json (mirrors the interlude-street convention).
"""
import concurrent.futures
import hashlib
import json
import re
import urllib.request
import zipfile
from datetime import datetime, timezone
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/KosRealWorld'
UA = {'User-Agent': 'LayarTerakhirAssetTool/1.0'}


def get(url, timeout=180):
    req = urllib.request.Request(url, headers=UA)
    with urllib.request.urlopen(req, timeout=timeout) as r:
        return r.read()


OGA = {
    'oga-toilets': ('https://opengameart.org/content/toilets',
                    'https://opengameart.org/sites/default/files/toilets_fbx_gltf_blend.zip'),
    'oga-freestand-sink': ('https://opengameart.org/content/freestand-sink',
                           'https://opengameart.org/sites/default/files/FreestandSink.zip'),
    'oga-oven-and-fridge': ('https://opengameart.org/content/oven-and-fridge',
                            'https://opengameart.org/sites/default/files/ovensandfridges.blend'),
    'oga-bathroom': ('https://opengameart.org/content/bathroom',
                     'https://opengameart.org/sites/default/files/bathroom_0.zip'),
    'oga-kitchen-stuff': ('https://opengameart.org/content/kitchen-stuff',
                          'https://opengameart.org/sites/default/files/kitchen%20stuff.blend'),
    'oga-chuveiro': ('https://opengameart.org/content/low-poly-chuveiro-brazilian-electric-shower',
                     'https://opengameart.org/sites/default/files/chuveiro.zip'),
}

PH_MODELS = ['electric_stove', 'vintage_electric_kettle', 'pot_enamel_01',
             'steel_frame_shelves_01', 'dining_chair_02', 'dining_table',
             'metal_trash_can', 'plunger', 'drain_cleaner', 'metal_jug',
             'wooden_cutting_board', 'wooden_spoon',
             'garden_hose_wall_mounted_01']


def sha256(data):
    return hashlib.sha256(data).hexdigest()


def fetch_oga(name, page, url):
    data = get(url)
    dest = CACHE / (name + Path(url.split('?')[0]).suffix)
    dest.parent.mkdir(parents=True, exist_ok=True)
    dest.write_bytes(data)
    record = {'page': page, 'url': url, 'license': 'CC0-1.0',
              'licenseUrl': 'https://creativecommons.org/publicdomain/zero/1.0/',
              'archive': str(dest.relative_to(ROOT)), 'bytes': len(data),
              'sha256': sha256(data), 'extracted': []}
    if dest.suffix == '.zip':
        with zipfile.ZipFile(dest) as pack:
            assert pack.testzip() is None
            names = [n for n in pack.namelist()
                     if not Path(n).is_absolute() and '..' not in Path(n).parts
                     and not n.endswith('/')]
            pack.extractall(CACHE / name)
            record['extracted'] = sorted(names)[:80]
            record['extractedTruncated'] = len(names) > 80
    print('OGA', name, len(data), 'bytes', flush=True)
    return name, record


def fetch_ph(slug):
    meta = json.loads(get('https://api.polyhaven.com/files/' + slug, timeout=60))
    folder = CACHE / 'PolyHaven' / slug
    folder.mkdir(parents=True, exist_ok=True)
    files = []
    entries = [('fbx', meta['fbx']['1k']['fbx'])]
    includes = meta.get('gltf', {}).get('1k', {}).get('gltf', {}).get('include', {})
    for inc_name, f in includes.items():
        if inc_name.lower().endswith(('.jpg', '.png')):
            entries.append(('tex-' + Path(inc_name).name, f))
    for label, f in entries:
        data = get(f['url'])
        if f.get('md5'):
            assert hashlib.md5(data).hexdigest() == f['md5'], (slug, label)
        dest = folder / (slug + '.fbx' if label == 'fbx' else Path(label[4:]).name)
        dest.write_bytes(data)
        files.append({'file': str(dest.relative_to(ROOT)), 'bytes': len(data),
                      'sha256': sha256(data), 'sourceMd5': f.get('md5')})
    print('PH', slug, sum(f['bytes'] for f in files), 'bytes', flush=True)
    return slug, {'page': 'https://polyhaven.com/a/' + slug,
                  'license': 'CC0-1.0',
                  'licenseUrl': 'https://polyhaven.com/license',
                  'files': files}


def main():
    CACHE.mkdir(parents=True, exist_ok=True)
    manifest = {'fetchedUtc': datetime.now(timezone.utc).isoformat(),
                'sources': {}}
    with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
        oga = dict(pool.map(lambda kv: fetch_oga(kv[0], kv[1][0], kv[1][1]), OGA.items()))
    manifest['sources'].update(oga)
    with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
        ph = dict(pool.map(fetch_ph, PH_MODELS))
    manifest['sources'].update({'polyhaven-' + k: v for k, v in ph.items()})
    out = ROOT / 'Audit/kos-realworld-source.json'
    out.write_text(json.dumps(manifest, indent=2) + '\n')
    print('MANIFEST', out, flush=True)


if __name__ == '__main__':
    main()
