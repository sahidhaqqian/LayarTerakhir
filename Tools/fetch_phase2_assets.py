"""Fetch new licensed hospital assets, not a scan of previous chapter art.

Madduck permits distribution in a game, not as loose assets; its imported binaries
are deliberately excluded from the public Git repository. Run this, then
blender -b --python Tools/import_phase2_assets.py to reconstruct them locally.
No artwork is generated. Downloads use author links / licensed Objaverse mirrors.
"""
import concurrent.futures
import gzip
import hashlib
import json
import re
import zipfile
from pathlib import Path
import requests

ROOT = Path(__file__).resolve().parents[1]
SOURCE = ROOT / 'Builds/ArtSources/Phase2'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase2/Imported'
SOURCE.mkdir(parents=True, exist_ok=True)
OUT.mkdir(parents=True, exist_ok=True)
RECORDS = []


def download(url, path):
    if not path.exists():
        r = requests.get(url, timeout=180); r.raise_for_status(); path.write_bytes(r.content)
    return hashlib.sha256(path.read_bytes()).hexdigest()


def itch():
    url = 'https://madduck.itch.io/modular-3d-hospital-environment'
    for uid, name in [('10988827', 'Hospital_Source.zip')]:
        target = SOURCE/name
        if not target.exists():
            s = requests.Session(); page = s.get(url); page.raise_for_status()
            csrf = re.search('name="csrf_token" value="([^"]+)', page.text).group(1)
            response = s.post(url+'/file/'+uid, data={'csrf_token': csrf}, headers={'Referer': url}); response.raise_for_status()
            download(response.json()['url'], target)
        with zipfile.ZipFile(target) as z: z.extractall(SOURCE/'Madduck')
        RECORDS.append(dict(id='Madduck modular hospital', author='Madduck', sourcePage=url,
            license='Author license: commercial game use and adaptation; no standalone asset redistribution',
            sourceSha256=hashlib.sha256(target.read_bytes()).hexdigest(), rawBinariesInGit=False))


def objaverse(uid, name):
    mapping = SOURCE/'object-paths.json.gz'
    download('https://huggingface.co/datasets/allenai/objaverse/resolve/main/object-paths.json.gz', mapping)
    paths = json.loads(gzip.decompress(mapping.read_bytes())); path = paths[uid]
    part = path.split('/')[1]; annotation = SOURCE/(part+'.json.gz')
    download('https://huggingface.co/datasets/allenai/objaverse/resolve/main/metadata/'+part+'.json.gz', annotation)
    meta = json.loads(gzip.decompress(annotation.read_bytes()))[uid]
    assert meta['license'] in ['by','cc0'], (name,meta['license'])
    url = 'https://huggingface.co/datasets/allenai/objaverse/resolve/main/'+path
    digest = download(url, SOURCE/(name+'.glb'))
    RECORDS.append(dict(id=name, title=meta['name'], author=meta['user']['displayName'],
        sourcePage=meta['viewerUrl'], license='CC-BY-4.0' if meta['license']=='by' else 'CC0-1.0',
        licenseUrl='https://creativecommons.org/licenses/by/4.0/' if meta['license']=='by' else 'https://creativecommons.org/publicdomain/zero/1.0/',
        mirror=url, sourceSha256=digest, operation='offline format conversion, material extraction and scale fitting'))


def polyhaven(name, model=True):
    path=SOURCE/(name+'.json')
    download('https://api.polyhaven.com/files/'+name,path)
    meta=json.loads(path.read_text()); folder=OUT/'PolyHaven'/name;folder.mkdir(parents=True,exist_ok=True)
    wanted=[(name+'.fbx',meta['fbx']['1k']['fbx'])] if model else []
    for suffix,keys in [('Diffuse',['Diffuse','diff']),('Normal',['nor_gl']),('ARM',['arm'])]:
        key=next((key for key in keys if key in meta),None)
        if not key: continue
        entry=meta[key]['1k']; ext='jpg' if 'jpg' in entry else 'png';wanted.append((name+suffix+'.'+ext,entry[ext]))
    files=[]
    for filename,item in wanted:
        dest=folder/filename;digest=download(item['url'],dest)
        assert not item.get('md5') or hashlib.md5(dest.read_bytes()).hexdigest()==item['md5']
        files.append(dict(file=str(dest.relative_to(ROOT)),url=item['url'],sha256=digest))
    RECORDS.append(dict(id=name, sourcePage='https://polyhaven.com/a/'+name, license='CC0-1.0',licenseUrl='https://polyhaven.com/license',files=files))


def particles():
    url='https://kenney.nl/media/pages/assets/particle-pack/f8fe0f8cb8-1677578741/kenney_particle-pack.zip'
    archive=SOURCE/'KenneyParticles.zip';digest=download(url,archive)
    folder=OUT/'KenneyParticles';folder.mkdir(exist_ok=True)
    with zipfile.ZipFile(archive) as z:
        (folder/'Smoke.png').write_bytes(z.read('PNG (Transparent)/smoke_01.png'))
        (folder/'License.txt').write_bytes(z.read('License.txt'))
    RECORDS.append(dict(id='Kenney particle smoke',author='Kenney',sourcePage='https://kenney.nl/assets/particle-pack',
        license='CC0-1.0',sourceSha256=digest,operation='Unmodified downloaded transparent smoke texture for local particle mist'))


def main():
    itch()
    for uid,name in [('5ceaaa4482a84774b9922366cfb228e7','AutopsyLab'),
                     ('b27fc2f948c34c6084488d9d3e3b64db','Neonatal'),
                     ('c67efef1abeb41969eef84c06311fac6','Scissors'),
                     ('196cf0d3d5094b50874dbf51064a7d3e','Safe'),
                     ('222717d27f0b4a4f84106260c1fd0d7e','Generator')]: objaverse(uid,name)
    url='https://kenney.nl/media/pages/assets/prototype-kit/4d3b7073ed-1724832076/kenney_prototype-kit.zip'
    digest=download(url,SOURCE/'KenneyPrototype.zip')
    with zipfile.ZipFile(SOURCE/'KenneyPrototype.zip') as z:z.extractall(SOURCE/'Kenney')
    RECORDS.append(dict(id='Kenney stair module',sourcePage='https://kenney.nl/assets/prototype-kit',license='CC0-1.0',sourceSha256=digest,
        operation='existing stair mesh conversion and application of downloaded hospital material'))
    # Newly fetched sources for this hospital; no search through old chapter assets.
    for name in ['chemistry_set','medical_tape','metal_tool_chest','bunsen_burner','tool_cart','large_iron_gate','binder_notebook']:
        polyhaven(name)
    polyhaven('red_brick_03',model=False)
    polyhaven('metal_plate_02',model=False)
    particles()
    (ROOT/'Audit/phase2-asset-sources.json').write_text(json.dumps(RECORDS,indent=2)+'\n')
    print('PHASE2_NEW_ASSET_SOURCES',len(RECORDS))

if __name__=='__main__':main()
