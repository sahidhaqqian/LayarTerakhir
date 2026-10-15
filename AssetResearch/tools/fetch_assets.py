"""Fetch a curated CC0 prop set from the Poly Haven public API for the
LayarTerakhir Kamar Kos environment.

Downloads each model as FBX (1K variant) plus its required 1K JPG/PNG PBR
textures, verifies every file against the API-provided MD5, records SHA-256,
and writes AssetResearch/manifest/source-manifest.json.

Usage:  /usr/bin/python3 AssetResearch/tools/fetch_assets.py [asset_id ...]
Nothing under Assets/, ProjectSettings/ or Packages/ is touched.
"""
from pathlib import Path
import hashlib
import json
import sys
import time

import requests

REPO = Path(__file__).resolve().parents[2]
RESEARCH = REPO / 'AssetResearch'
ASSETS = RESEARCH / 'assets'
MANIFEST = RESEARCH / 'manifest' / 'source-manifest.json'
USER_AGENT = 'LayarTerakhir-AssetResearch/1.0'

# Selected for the modest Indonesian kos room and its streamer/student routine.
# Every entry is a realistic, PBR, lightweight Poly Haven CC0 prop.
SELECTED = [
    'office_notepads',              # loose papers / notebooks on the desk
    'stationery_supplies',          # pens, pencils, eraser in a cup
    'clipboard',                    # worn clipboard with a sheet
    'plastic_thermos',              # water bottle with cup lid
    'plastic_crate_02',             # small stackable storage box
    'industrial_pastic_container',  # larger storage box with hinged lid
    'gamepad',                      # wired gamepad and cable (streamer desk)
    'retro_multimeter',             # analog multimeter with coiled probes
    'cassette_player',              # old radio/cassette unit
    'wall_clock',                   # wall clock
    'alarm_clock_01',               # twin-bell bedside alarm clock
    'wooden_broom',                 # sapu ijuk corner broom
    'vintage_flashlight',           # power-outage flashlight
]

SESSION = requests.Session()
SESSION.headers['User-Agent'] = USER_AGENT


def sha256_bytes(payload: bytes) -> str:
    return hashlib.sha256(payload).hexdigest()


def fetch(url: str) -> requests.Response:
    response = SESSION.get(url, timeout=180)
    response.raise_for_status()
    return response


def fetch_asset(asset_id: str) -> dict:
    info = fetch('https://api.polyhaven.com/info/' + asset_id).json()
    files = fetch('https://api.polyhaven.com/files/' + asset_id).json()
    folder = ASSETS / asset_id
    textures = folder / 'textures'
    textures.mkdir(parents=True, exist_ok=True)

    entry = files['fbx']['1k']['fbx']
    model_name = asset_id + '_1k.fbx'
    downloads = []

    def save_record(relative_path: Path, name: str, url: str, api_md5: str, content: bytes):
        target = folder / relative_path
        target.write_bytes(content)
        downloads.append({
            'file': str(Path(asset_id) / relative_path),
            'name': name,
            'url': url,
            'apiMd5': api_md5,
            'sha256': sha256_bytes(content),
            'bytes': len(content),
        })

    content = fetch(entry['url']).content
    assert hashlib.md5(content).hexdigest() == entry['md5'], entry['url']
    save_record(Path(model_name), model_name, entry['url'], entry['md5'], content)

    includes = files['gltf']['1k']['gltf']['include']
    for include_name, include_entry in includes.items():
        if include_name.endswith('.bin'):
            continue
        if not include_name.lower().endswith(('.jpg', '.png')):
            continue
        name = Path(include_name).name
        content = fetch(include_entry['url']).content
        assert hashlib.md5(content).hexdigest() == include_entry['md5'], include_entry['url']
        save_record(Path('textures') / name, name, include_entry['url'],
                    include_entry['md5'], content)

    thumb_url = info['thumbnail_url'].replace('width=256&height=256', 'width=512&height=512')
    thumb = fetch(thumb_url).content
    (folder / 'source-preview.png').write_bytes(thumb)
    downloads.append({
        'file': str(Path(asset_id) / 'source-preview.png'),
        'name': 'source-preview.png',
        'url': thumb_url,
        'apiMd5': None,
        'sha256': sha256_bytes(thumb),
        'bytes': len(thumb),
    })

    record = {
        'id': asset_id,
        'name': info['name'],
        'authors': info['authors'],
        'description': info.get('description', ''),
        'categories': info.get('categories', []),
        'tags': info.get('tags', []),
        'sourcePage': 'https://polyhaven.com/a/' + asset_id,
        'license': 'CC0-1.0',
        'licenseUrl': 'https://polyhaven.com/license',
        'attributionRequired': False,
        'polygons': info.get('polycount'),
        'dimensionsCm': info.get('dimensions'),
        'sourceMaxResolution': info.get('max_resolution'),
        'downloadedMaxResolution': 1024,
        'files': downloads,
    }
    print('FETCHED', asset_id, sum(d['bytes'] for d in downloads), 'bytes', flush=True)
    return record


def main() -> int:
    requested = sys.argv[1:] or SELECTED
    previous = {}
    if MANIFEST.exists():
        previous = {r['id']: r for r in json.loads(MANIFEST.read_text())}
    licensed = fetch('https://polyhaven.com/license')
    evidence = RESEARCH / 'evidence'
    evidence.mkdir(parents=True, exist_ok=True)
    (evidence / 'polyhaven-license.txt').write_text(
        'Fetched: ' + time.strftime('%Y-%m-%dT%H:%M:%SZ', time.gmtime()) + '\n'
        'URL: https://polyhaven.com/license\n'
        'HTTP: ' + str(licensed.status_code) + '\n\n'
        + licensed.text
    )
    for asset_id in requested:
        try:
            previous[asset_id] = fetch_asset(asset_id)
        except Exception as error:  # keep going, report at the end
            print('FAILED', asset_id, repr(error), flush=True)
    ordered = [previous[asset_id] for asset_id in SELECTED if asset_id in previous]
    MANIFEST.parent.mkdir(parents=True, exist_ok=True)
    MANIFEST.write_text(json.dumps(ordered, indent=2) + '\n')
    print('manifest:', MANIFEST, len(ordered), 'assets')
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
