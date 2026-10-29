"""Selected public CC0 inputs for prolog review 28–43; no credentials required."""
import hashlib
import json
import subprocess
import zipfile
from pathlib import Path
from concurrent.futures import ThreadPoolExecutor

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/PrologRevision'
DEST = ROOT / 'Assets/_LayarTerakhir/assets/Art/PrologRevision'


def fetch(url, path, md5=None):
    path.parent.mkdir(parents=True, exist_ok=True)
    if not path.exists():
        subprocess.run(['curl', '--fail', '-L', '--retry', '3', '-sS', '-A',
                        'LayarTerakhir/1.0 (https://github.com/sahidhaqqian/LayarTerakhir; asset import)', url, '-o', str(path)], check=True)
    data = path.read_bytes()
    if md5 and hashlib.md5(data).hexdigest() != md5:
        raise ValueError('Publisher checksum mismatch: ' + url)
    return dict(url=url, file=path.relative_to(ROOT).as_posix(), bytes=len(data),
                sha256=hashlib.sha256(data).hexdigest())


def polyhaven(slug):
    meta = CACHE / (slug + '.json')
    fetch('https://api.polyhaven.com/files/' + slug, meta)
    files = json.loads(meta.read_text())
    entry = files['fbx']['1k']['fbx']
    records = [fetch(entry['url'], DEST / 'Models' / (slug + '.fbx'), entry['md5'])]
    for name, entry in files['gltf']['1k']['gltf']['include'].items():
        if name.endswith(('.jpg', '.png')):
            records.append(fetch(entry['url'], DEST / 'Textures' / Path(name).name, entry['md5']))
    return dict(name=slug, page='https://polyhaven.com/a/' + slug,
                license='CC0-1.0', licenseSource='https://polyhaven.com/license', files=records)


def main():
    with ThreadPoolExecutor(max_workers=2) as pool:
        sources = list(pool.map(polyhaven, ['binder_notebook', 'vintage_day_bed']))
    for name, page, url in [
        ('BathTube', 'bath-tube', 'BathTube.zip'),
        ('CellarLadder', 'metal-ladder-low-poly', 'ladder3_0.blend'),
        ('Mugs', 'free-mug-pack-1', 'free-mug-pack-1.zip')]:
        dest = CACHE / url.replace('%20', ' ')
        record = fetch('https://opengameart.org/sites/default/files/' + url, dest)
        if dest.suffix == '.zip':
            with zipfile.ZipFile(dest) as pack:
                assert pack.testzip() is None
                assert all(not Path(n).is_absolute() and '..' not in Path(n).parts for n in pack.namelist())
                pack.extractall(CACHE / name)
        sources.append(dict(name=name, page='https://opengameart.org/content/' + page,
                            license='CC0-1.0', licenseSource='https://creativecommons.org/publicdomain/zero/1.0/',
                            files=[record]))
    sources.append(dict(name='RealWallClawPhotograph',
                        page='https://commons.wikimedia.org/wiki/File:Bear_damage_to_fish_freezing_building_at_Brooks_Camp_(20529901541).jpg',
                        author='Katmai National Park and Preserve', license='Public Domain / PD-US-NPS',
                        licenseSource='https://www.nps.gov/subjects/digital/npsimages.htm',
                        files=[fetch('https://upload.wikimedia.org/wikipedia/commons/a/a1/Bear_damage_to_fish_freezing_building_at_Brooks_Camp_%2820529901541%29.jpg', DEST / 'Textures/RealWallClawPhoto.jpg')]))
    (ROOT / 'Audit/prolog-revision-asset-sources.json').write_text(json.dumps(sources, indent=2) + '\n')
    print('PROLOG_REVISION_SOURCES_VERIFIED', len(sources))


if __name__ == '__main__':
    main()
