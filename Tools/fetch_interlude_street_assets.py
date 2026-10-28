"""Fetch CC0 street packs for the daytime interlude exterior. Preserves archives + licenses, records audit JSON."""
import hashlib
import json
import re
import shutil
import urllib.request
import zipfile
from pathlib import Path
from datetime import datetime, timezone

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/InterludeStreet'
PACKS = {
    'kenney-car-kit': 'https://kenney.nl/assets/car-kit',
    'kenney-food-kit': 'https://kenney.nl/assets/food-kit',
    'kenney-furniture-kit': 'https://kenney.nl/assets/furniture-kit',
    'quaternius-publictransport': 'https://quaternius.com/packs/publictransport.html',
    'quaternius-ultimatefood': 'https://quaternius.com/packs/ultimatefood.html',
}
# Only these model basenames are copied into Assets (keeps the import small).
WANT = {
    'kenney-car-kit': ['sedan', 'hatchback', 'taxi', 'van', 'truck'],
    'kenney-food-kit': ['plate', 'bowl', 'rice', 'chicken', 'drumstick', 'apple', 'banana', 'bread', 'cheese', 'sausage'],
    'kenney-furniture-kit': ['stool', 'table', 'crate', 'shelf', 'chair'],
    'quaternius-publictransport': ['minibus', 'bus', 'van', 'bicycle', 'bike'],
    'quaternius-ultimatefood': ['rice', 'chicken', 'drumstick', 'bowl', 'plate', 'apple', 'banana', 'bread'],
}


def get(url):
    return urllib.request.urlopen(urllib.request.Request(url, headers={'User-Agent': 'LayarTerakhirAssetTool/1.0'}), timeout=120).read()


def main():
    CACHE.mkdir(parents=True, exist_ok=True)
    manifest = {}
    for name, page in PACKS.items():
        html = get(page).decode('utf-8', 'replace')
        m = re.search(r'href=[\'"]([^\'"]+\.zip)[\'"]', html)
        if not m:
            print('NO_ZIP', name)
            continue
        url = m.group(1)
        if url.startswith('/'):
            base = re.match(r'(https?://[^/]+)', page).group(1)
            url = base + url
        data = get(url)
        archive = CACHE / (name + '.zip')
        archive.write_bytes(data)
        with zipfile.ZipFile(archive) as pack:
            assert pack.testzip() is None
            assert all(not Path(n).is_absolute() and '..' not in Path(n).parts for n in pack.namelist())
            pack.extractall(CACHE / name)
        lic = None
        for n in (CACHE / name).rglob('*'):
            if n.name.lower() in ('license.txt', 'licence.txt', 'license.md', 'readme.txt', 'readme.md'):
                lic = n
                break
        license_text = lic.read_bytes() if lic else b''
        audit = dict(fetchedUtc=datetime.now(timezone.utc).isoformat(), page=page, url=url,
                     sha256=hashlib.sha256(data).hexdigest(), bytes=len(data))
        (ROOT / f'Audit/interlude-street-{name}-source.json').write_text(json.dumps(audit, indent=2) + '\n')
        if lic:
            (ROOT / f'Audit/interlude-street-{name}-license.txt').write_bytes(license_text)
        manifest[name] = dict(files=len(list((CACHE / name).rglob('*'))), license=bool(lic))
        print('PACK_OK', name, len(data), manifest[name])
    # Copy wanted models into Assets.
    dest_root = ROOT / 'Assets/_LayarTerakhir/assets/Art/Imported/InterludeStreet'
    dest_root.mkdir(parents=True, exist_ok=True)
    copied = []
    for name, stems in WANT.items():
        for f in (CACHE / name).rglob('*'):
            if f.suffix.lower() not in ('.fbx', '.obj', '.gltf', '.glb', '.png'):
                continue
            low = f.stem.lower()
            if not any(w in low for w in stems):
                continue
            dest = dest_root / name / f.name
            dest.parent.mkdir(parents=True, exist_ok=True)
            shutil.copyfile(f, dest)
            copied.append(str(dest.relative_to(ROOT)))
    print('COPIED', len(copied))
    for c in sorted(copied)[:60]:
        print(' ', c)


if __name__ == '__main__':
    main()
