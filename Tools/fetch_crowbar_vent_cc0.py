"""Download the CC0 Poly Haven Crowbar and rusty grate texture for the child's
ventilation cover in Rumah Atmaja.

The old wooden VentCabinet looks brand new and does not fit the vent hole.
This fetches the CC0 'Crowbar 01' model (1k) the player uses to pry the vent
open, plus the CC0 'metal_grate_rusty' PBR texture (1k diffuse + normal) used
to dress the refitted vent cover so it reads old and rusted, not new.
Provenance is recorded in Audit/crowbar-vent-cc0-sources.json. No original
artwork is modeled.
"""
from pathlib import Path
import hashlib
import json
import urllib.request

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/CrowbarVent'
OUT_MODEL = ROOT / 'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/crowbar'
OUT_TEX = ROOT / 'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/metal_grate_rusty'
CACHE.mkdir(parents=True, exist_ok=True)
OUT_MODEL.mkdir(parents=True, exist_ok=True)
OUT_TEX.mkdir(parents=True, exist_ok=True)

BASE = 'https://dl.polyhaven.org/file/ph-assets/Models'
TBASE = 'https://dl.polyhaven.org/file/ph-assets/Textures'

MODEL_FILES = {
    'crowbar_01_1k.fbx': f'{BASE}/fbx/1k/crowbar_01/crowbar_01_1k.fbx',
    'crowbar_01_diff_1k.jpg': f'{BASE}/jpg/1k/crowbar_01/crowbar_01_diff_1k.jpg',
    'crowbar_01_nor_gl_1k.jpg': f'{BASE}/jpg/1k/crowbar_01/crowbar_01_nor_gl_1k.jpg',
    'crowbar_01_arm_1k.jpg': f'{BASE}/jpg/1k/crowbar_01/crowbar_01_arm_1k.jpg',
}
TEX_FILES = {
    'metal_grate_rusty_diff_1k.jpg': f'{TBASE}/jpg/1k/metal_grate_rusty/metal_grate_rusty_diff_1k.jpg',
    'metal_grate_rusty_nor_gl_1k.jpg': f'{TBASE}/jpg/1k/metal_grate_rusty/metal_grate_rusty_nor_gl_1k.jpg',
    'metal_grate_rusty_arm_1k.jpg': f'{TBASE}/jpg/1k/metal_grate_rusty/metal_grate_rusty_arm_1k.jpg',
}


def fetch(fname, url, out):
    cached = CACHE / fname
    if not cached.exists() or cached.stat().st_size == 0:
        req = urllib.request.Request(url, headers={'User-Agent': 'LayarTerakhir-CC0-fetch/1.0'})
        with urllib.request.urlopen(req, timeout=180) as r, open(cached, 'wb') as f:
            f.write(r.read())
    target = out / fname
    target.write_bytes(cached.read_bytes())
    return dict(
        path=target.relative_to(ROOT).as_posix(),
        url=url,
        sha256=hashlib.sha256(target.read_bytes()).hexdigest(),
        bytes=target.stat().st_size,
    )


records = [
    dict(name='Crowbar 01', page='https://polyhaven.com/a/crowbar_01',
         license='CC0-1.0', licenseUrl='https://polyhaven.com/license',
         files=[fetch(f, u, OUT_MODEL) for f, u in MODEL_FILES.items()]),
    dict(name='Metal Grate Rusty', page='https://polyhaven.com/a/metal_grate_rusty',
         license='CC0-1.0', licenseUrl='https://polyhaven.com/license',
         files=[fetch(f, u, OUT_TEX) for f, u in TEX_FILES.items()]),
]

(ROOT / 'Audit/crowbar-vent-cc0-sources.json').write_text(json.dumps(records, indent=2) + '\n')
print('CROWBAR_VENT_SOURCES', sum(len(r['files']) for r in records), 'files')
for r in records:
    for f in r['files']:
        print(' ', f['bytes'], f['path'])
