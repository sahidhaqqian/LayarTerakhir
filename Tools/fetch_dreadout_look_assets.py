"""Fetch the CC0 asphalt used by the reference look; keep source maps in ignored Builds.

DreadOut screenshots are visual references only and are never imported into Assets.
The material is photographed by Rob Tuytel: https://polyhaven.com/a/asphalt_02
"""
import hashlib
import io
import json
import urllib.request
from pathlib import Path

from PIL import Image

ROOT = Path(__file__).resolve().parents[1]


def fetch(url):
    with urllib.request.urlopen(url, timeout=90) as response:
        return response.read()


def main():
    cache = ROOT / 'Builds/ArtSources/DreadOutReferences'
    target = ROOT / 'Assets/_LayarTerakhir/assets/Art/Review/Textures'
    cache.mkdir(parents=True, exist_ok=True)
    target.mkdir(parents=True, exist_ok=True)
    manifest = json.loads(fetch('https://api.polyhaven.com/files/asphalt_02'))
    records, maps = [], {}
    for kind in ('Diffuse', 'nor_gl', 'Rough', 'AO'):
        entry = manifest[kind]['1k']['jpg']
        data = fetch(entry['url'])
        if hashlib.md5(data).hexdigest() != entry['md5']:
            raise ValueError('Source checksum mismatch: ' + kind)
        name = entry['url'].rsplit('/', 1)[-1]
        (cache / name).write_bytes(data)
        if kind in ('Diffuse', 'nor_gl'):
            (target / name).write_bytes(data)
        maps[kind] = Image.open(io.BytesIO(data)).convert('L')
        records.append(dict(map=kind, url=entry['url'], md5=entry['md5'], bytes=len(data)))
    smoothness = maps['Rough'].point(lambda value: round(255 - value * .57))
    zero = Image.new('L', smoothness.size, 0)
    Image.merge('RGBA', (zero, maps['AO'], zero, smoothness)).save(target / 'asphalt_02_wet_mask.png')
    receipt = dict(source='https://polyhaven.com/a/asphalt_02', author='Rob Tuytel', license='CC0-1.0',
                   maps=records, adaptation='URP mask R=0 G=AO B=0 A=1-0.57*roughness; source JPG maps at 1K')
    (ROOT / 'Audit/dreadout-look-asset-sources.json').write_text(json.dumps(receipt, indent=2) + '\n')
    print('Asphalt maps downloaded, checksum verified, mask packed')


if __name__ == '__main__':
    main()
