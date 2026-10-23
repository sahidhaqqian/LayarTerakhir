"""Import existing CC0 assets for Phase 1. Does not model or generate artwork."""
import concurrent.futures
import hashlib
import json
import shutil
import urllib.request
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase1/Imported'
SOURCE = ROOT / 'Builds/ArtSources/Phase1External'
OUT.mkdir(parents=True, exist_ok=True)
ASSETS = ['bolt_cutters_01', 'portable_searchlight', 'cassette_player', 'vintage_radio_transceiver',
          'power_box_01', 'portable_generator', 'wheelchair_01', 'barrel_stove',
          'mounted_fluorescent_lights', 'caged_hanging_light', 'portable_cassette_player']


def fetch(url):
    request = urllib.request.Request(url, headers={'User-Agent': 'Mozilla/5.0 LayarTerakhir-AssetImport/1.0'})
    with urllib.request.urlopen(request, timeout=120) as response:
        return response.read()


catalog = json.loads(fetch('https://api.polyhaven.com/assets?t=models'))


def asset(name):
    cached=Path('/tmp/lt-p1-ph-'+name+'.json')
    metadata=json.loads(cached.read_text()) if cached.exists() else json.loads(fetch('https://api.polyhaven.com/files/' + name))
    folder = OUT / name
    folder.mkdir(parents=True, exist_ok=True)
    records = []
    wanted = [(name + '.fbx', metadata['fbx']['1k']['fbx'])]
    for suffix, channel in [('Diffuse', 'Diffuse'), ('Normal', 'nor_gl'), ('ARM', 'arm')]:
        key=channel if channel in metadata else {'Diffuse':'body_diff','nor_gl':'body_nor_gl','arm':'body_arm'}[channel]
        entry = metadata[key]['1k']
        extension = 'jpg' if 'jpg' in entry else 'png'
        wanted.append((name + suffix + '.' + extension, entry[extension]))
    for filename, info in wanted:
        target=folder/filename
        data=target.read_bytes() if target.exists() else fetch(info['url'])
        if info.get('md5'):
            assert hashlib.md5(data).hexdigest() == info['md5']
        (folder / filename).write_bytes(data)
        records.append({'file': str((folder / filename).relative_to(ROOT)), 'url': info['url'], 'sha256': hashlib.sha256(data).hexdigest()})
    print('PHASE1_ASSET', name, 'tris', catalog[name].get('polycount'), flush=True)
    return {'id': name, 'sourcePage': 'https://polyhaven.com/a/' + name, 'authors': catalog[name].get('authors'),
            'license': 'CC0-1.0', 'licenseUrl': 'https://polyhaven.com/license', 'files': records}


with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
    records = list(pool.map(asset, ASSETS))

decal_folder=OUT/'ExileBlood'
decal_folder.mkdir(exist_ok=True)
decal_url='https://opengameart.org/sites/default/files/blood_0.png'
data=fetch(decal_url)
(decal_folder/'blood.png').write_bytes(data)
records.append({'id':'Blood splatter','author':'ExileGL','sourcePage':'https://opengameart.org/content/blood-splatter','license':'CC0-1.0','files':[{'file':str((decal_folder/'blood.png').relative_to(ROOT)),'url':decal_url,'sha256':hashlib.sha256(data).hexdigest()}]})
records.append({'id': 'MrLeap low-poly weapon pack (existing kris mesh)', 'sourcePage': 'https://mrleap.itch.io/low-poly-weapon-pack', 'author': 'mrleap', 'license': 'CC0-1.0', 'archiveSha256': hashlib.sha256((SOURCE / 'MrLeapsLowPolyWeaponAssetPackv2.zip').read_bytes()).hexdigest()})
(ROOT / 'Audit/phase1-external-assets.json').write_text(json.dumps(records, indent=2) + '\n')
credits = '# Phase 1 — external visual assets\n\nAll visual sources are existing project assets, designer-supplied VRMs or downloaded assets. No generated photograph is included. Conversion, scale fitting, collision and PBR channel conversion are import steps, not newly modeled artwork.\n\n'
for record in records:
    credits += f"- **{record['id']}** — [original source]({record['sourcePage']}), {record['license']}.\n"
credits += '\nCharacter source hashes and optimization: `phase1-*-authoring.json`. Audio credits: `phase1-audio-credits.md`. Revised environment imports and credits: `phase1-environment-sources.json` and `phase1-environment-revision.md`. Campaign characters/motor retain their existing license records.\n'
(ROOT / 'Audit/phase1-asset-credits.md').write_text(credits)
