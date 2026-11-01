"""Download the CC0 Poly Haven Dining Table for the Rumah Atmaja banquet.

The bundled LongDiningTable.fbx renders as a tiny chair-sized blob, so the
Bug-39 feast floats. This fetches Aron Lyczek's CC0 'Dining Table'
(rustic wooden table, worn grain) and records provenance in
Audit/dining-table-cc0-sources.json. No original artwork is modeled.
"""
from pathlib import Path
import hashlib
import json
import urllib.request

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/DiningTable'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/dining_table'
CACHE.mkdir(parents=True, exist_ok=True)
OUT.mkdir(parents=True, exist_ok=True)

BASE = 'https://dl.polyhaven.org/file/ph-assets/Models'
FILES = {
    'dining_table_1k.fbx': f'{BASE}/fbx/1k/dining_table/dining_table_1k.fbx',
    'dining_table_diff_1k.jpg': f'{BASE}/jpg/1k/dining_table/dining_table_diff_1k.jpg',
    'dining_table_nor_gl_1k.jpg': f'{BASE}/jpg/1k/dining_table/dining_table_nor_gl_1k.jpg',
}

records = dict(
    name='Dining Table',
    page='https://polyhaven.com/a/dining_table',
    author='Aron Lyczek', license='CC0-1.0',
    licenseUrl='https://polyhaven.com/license',
    files=[],
)
for fname, url in FILES.items():
    cached = CACHE / fname
    if not cached.exists() or cached.stat().st_size == 0:
        req = urllib.request.Request(url, headers={'User-Agent': 'LayarTerakhir-CC0-fetch/1.0'})
        with urllib.request.urlopen(req, timeout=120) as r, open(cached, 'wb') as f:
            f.write(r.read())
    target = OUT / fname
    target.write_bytes(cached.read_bytes())
    records['files'].append(dict(
        path=target.relative_to(ROOT).as_posix(),
        url=url,
        sha256=hashlib.sha256(target.read_bytes()).hexdigest(),
        bytes=target.stat().st_size,
    ))

(ROOT / 'Audit/dining-table-cc0-sources.json').write_text(json.dumps([records], indent=2) + '\n')
print('DINING_TABLE_SOURCES', len(records['files']), 'files')
for f in records['files']:
    print(' ', f['bytes'], f['path'])
