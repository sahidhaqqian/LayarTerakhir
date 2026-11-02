"""Download the CC0 Poly Haven Vintage Grandfather Clock for Rumah Atmaja.

The bundled Atmaja GrandfatherClock.fbx renders invisible in play (blades and
pendulum were found buried at the case base, dial unreadable), so the sitting
room reads as having no clock at all. This fetches the CC0
'Vintage Grandfather Clock 01' (Poly Haven, CC0-1.0) at 1k and records
provenance in Audit/grandfather-clock-cc0-sources.json. No original artwork
is modeled.
"""
from pathlib import Path
import hashlib
import json
import urllib.request

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/GrandfatherClock'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/grandfather_clock'
CACHE.mkdir(parents=True, exist_ok=True)
OUT.mkdir(parents=True, exist_ok=True)

BASE = 'https://dl.polyhaven.org/file/ph-assets/Models'
FILES = {
    'vintage_grandfather_clock_01_1k.fbx': f'{BASE}/fbx/1k/vintage_grandfather_clock_01/vintage_grandfather_clock_01_1k.fbx',
    'vintage_grandfather_clock_01_diff_1k.jpg': f'{BASE}/jpg/1k/vintage_grandfather_clock_01/vintage_grandfather_clock_01_diff_1k.jpg',
    'vintage_grandfather_clock_01_nor_gl_1k.jpg': f'{BASE}/jpg/1k/vintage_grandfather_clock_01/vintage_grandfather_clock_01_nor_gl_1k.jpg',
    'vintage_grandfather_clock_01_arm_1k.jpg': f'{BASE}/jpg/1k/vintage_grandfather_clock_01/vintage_grandfather_clock_01_arm_1k.jpg',
}

records = dict(
    name='Vintage Grandfather Clock 01',
    page='https://polyhaven.com/a/vintage_grandfather_clock_01',
    author='Poly Haven contributors (see page)',
    license='CC0-1.0',
    licenseUrl='https://polyhaven.com/license',
    files=[],
)
for fname, url in FILES.items():
    cached = CACHE / fname
    if not cached.exists() or cached.stat().st_size == 0:
        req = urllib.request.Request(url, headers={'User-Agent': 'LayarTerakhir-CC0-fetch/1.0'})
        with urllib.request.urlopen(req, timeout=180) as r, open(cached, 'wb') as f:
            f.write(r.read())
    target = OUT / fname
    target.write_bytes(cached.read_bytes())
    records['files'].append(dict(
        path=target.relative_to(ROOT).as_posix(),
        url=url,
        sha256=hashlib.sha256(target.read_bytes()).hexdigest(),
        bytes=target.stat().st_size,
    ))

(ROOT / 'Audit/grandfather-clock-cc0-sources.json').write_text(json.dumps([records], indent=2) + '\n')
print('GRANDFATHER_CLOCK_SOURCES', len(records['files']), 'files')
for f in records['files']:
    print(' ', f['bytes'], f['path'])
