"""Fetch selected CC0 review props from the publishers' public downloads.

Raw archives stay in the ignored Builds cache; only selected derived models,
1K maps and the exact source/license receipt are delivered.
"""
import hashlib
import json
import re
import urllib.request
import zipfile
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/Review'
DEST = ROOT / 'Assets/_LayarTerakhir/assets/Art/Review'
HEADERS = {'User-Agent': 'LayarTerakhirAssetTool/1.0 (selected CC0 game asset import)'}
MODELS = ('painted_wooden_sofa', 'street_rat', 'food_apple_01', 'hamburger_buns')


def fetch(url, path):
    path.parent.mkdir(parents=True, exist_ok=True)
    if not path.exists():
        request = urllib.request.Request(url, headers=HEADERS)
        with urllib.request.urlopen(request, timeout=90) as response:
            data = response.read()
        path.with_suffix(path.suffix + '.partial').write_bytes(data)
        path.with_suffix(path.suffix + '.partial').replace(path)
    return path.read_bytes()


def model(name):
    files = json.loads(fetch('https://api.polyhaven.com/files/' + name, CACHE / (name + '.files.json')))
    jobs = [(files['fbx']['1k']['fbx'], CACHE / name / (name + '.fbx'))]
    # A few models use material-prefixed map names; collect every available 1K map.
    for kind, value in files.items():
        if any(token in kind.lower() for token in ('diffuse', 'nor_gl', 'arm')):
            entry = value.get('1k', {}).get('jpg')
            if entry:
                jobs.append((entry, DEST / 'Textures' / entry['url'].rsplit('/', 1)[1]))
    records = []
    for entry, path in jobs:
        data = fetch(entry['url'], path)
        assert hashlib.md5(data).hexdigest() == entry['md5'], 'Publisher checksum mismatch: ' + name
        records.append(dict(url=entry['url'], path=path.relative_to(ROOT).as_posix(), bytes=len(data),
                            sha256=hashlib.sha256(data).hexdigest(), publisherMd5=entry['md5']))
    return dict(name=name, page='https://polyhaven.com/a/' + name, license='CC0-1.0',
                licenseSource='https://polyhaven.com/license', files=records)


def main():
    CACHE.mkdir(parents=True, exist_ok=True)
    with ThreadPoolExecutor(max_workers=3) as pool:
        records = list(pool.map(model, MODELS))
    page = 'https://kenney.nl/assets/city-kit-suburban'
    html = fetch(page, CACHE / 'kenney-city-kit-suburban.html').decode()
    url = re.search(r"href='([^']+\.zip)'", html).group(1)
    archive = CACHE / 'kenney_city-kit-suburban_20.zip'
    data = fetch(url, archive)
    with zipfile.ZipFile(archive) as pack:
        assert pack.testzip() is None
        for name in pack.namelist():
            assert not Path(name).is_absolute() and '..' not in Path(name).parts
        pack.extractall(CACHE / 'KenneySuburban')
        license_name = next(n for n in pack.namelist() if Path(n).name.lower() == 'license.txt')
        (ROOT / 'Audit/kenney-suburban-CC0.txt').write_bytes(pack.read(license_name))
    records.append(dict(name='Kenney City Kit Suburban 2.0', page=page, url=url,
                        license='CC0-1.0', bytes=len(data), sha256=hashlib.sha256(data).hexdigest()))
    (ROOT / 'Audit/designer-review-asset-sources.json').write_text(json.dumps(records, indent=2) + '\n')
    print('REVIEW_CC0_SOURCES_VERIFIED', len(records), flush=True)


if __name__ == '__main__':
    main()
