"""Download new, licensed Karang Setra sources; originals stay outside Unity.

An asset means a distinct source model, not a renamed instance or a texture.
The scene manifest separately proves actual placement of at least 100 sources.
"""
import concurrent.futures
import gzip
import hashlib
import json
import shutil
import subprocess
import re
import time
from pathlib import Path

import requests

ROOT = Path(__file__).resolve().parents[1]
CACHE = ROOT / 'Builds/ArtSources/Phase4'
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase4/Imported'
MODELS = '''anthurium_botany_01 grass_medium_01 grass_medium_02 moss_01
dead_tree_trunk dead_tree_trunk_02 dry_branches_medium_01 pine_roots
root_cluster_01 root_cluster_02 single_root rock_07 rock_09 rock_face_01
rock_face_02 rock_moss_set_01 rock_moss_set_02 coast_land_rocks_02
coast_land_rocks_03 coast_land_rocks_04 coast_rocks_01 coast_rocks_02
coast_rocks_03 coast_rocks_05 sand_rocks_small_01 stone_01 shrub_01
shrub_02 shrub_04 shrub_sorrel_01 potted_plant_01 potted_plant_02
brass_candleholders brass_diya_lantern brass_goblets brass_pot_01
brass_pot_02 brass_vase_01 brass_vase_02 brass_vase_03 brass_vase_04
ceramic_vase_01 ceramic_vase_02 ceramic_vase_03 ceramic_vase_04 jug_01
wooden_bowl_01 wooden_bowl_02 wooden_bucket_02 wooden_spoon tea_set_01
chinese_stool chinese_tea_table WoodenChair_01 WoodenTable_01 WoodenTable_02
WoodenTable_03 wooden_stool_02 painted_wooden_chair_02 painted_wooden_table
painted_wooden_cabinet small_wooden_table_01 round_wooden_table_01
round_wooden_table_02 gallinera_chair dining_chair_02 GreenChair_01
SchoolChair_01 antique_ceramic_vase_01 carved_wooden_elephant sungka_board
sungka_board_02 decorative_book_set_01 hanging_picture_frame_02
standing_picture_frame_01 standing_picture_frame_02 round_spectacles
lantern_chandelier_01 wooden_lantern_01 stone_fire_pit wooden_axe
wooden_axe_02 wooden_axe_03 wooden_hammer_01 wooden_ladder wooden_ladder_02
ladder_sectioned_01 hatchet watering_can_metal_01 spinning_wheel_01
wooden_display_shelves_01 wooden_broom metal_stool_01 metal_stool_02
compost_bags compost_bag_02 rusted_wheel_rim_01 rusted_wheel_rim_02
bananas sweet_potato food_ginger_01 food_lime_01 food_lychee_01
food_pears_asian_01 food_pomegranate_01 food_avocado_01 yellow_onion'''.split()
# These are additional source surfaces, not counted towards the 100 models.
SURFACES = ['forest_ground_04', 'aerial_rocks_04', 'bamboo_wall_03', 'wood_planks_dirt', 'burned_ground_01', 'mud_cracked_dry_riverbed_002', 'clay_roof_tiles']
HEROES = [('285d403781454614b04e97b64454ff73', 'Joglo'),
          ('e9f9fa5397814f81bf85ad06acf5bf30', 'BambooSet'),
          ('2a0dbcdf8f5d48f5ad79987c7a8170ce', 'Banyan'),
          ('3fc4c1f9d0d3480db1bc1c0d3e3c31aa', 'Keris'),
          ('e1c518eae5144e0c83c2d9dc45a825ae', 'StoneWell'),
          ('81bcb1dae13b4a5bb7b142de935b6a09', 'HumanSkeleton'),
          ('9d4331e7b7524146a5a01aeb9a302a06', 'Robe'),
          ('386cceb251f54ff2827694f7351147c9', 'ElderHead'),
          ('683f795e1ebd4016ad3832c64ded3dd6', 'ForestStairs')]


def digest(path):
    with path.open('rb') as file:
        return hashlib.file_digest(file, 'sha256').hexdigest()


def get(url, path):
    path.parent.mkdir(parents=True, exist_ok=True)
    if path.exists() and path.stat().st_size:
        return digest(path)
    for attempt in range(3):
        try:
            with requests.get(url, timeout=(20, 180), stream=True) as response:
                response.raise_for_status()
                temp = path.with_suffix(path.suffix + '.download')
                with temp.open('wb') as file:
                    for chunk in response.iter_content(1024 * 1024):
                        file.write(chunk)
                temp.replace(path)
                return digest(path)
        except requests.RequestException:
            if attempt == 2:
                raise
            time.sleep(2 * (attempt + 1))


def ph(name):
    meta_path = CACHE / 'Metadata' / (name + '.json')
    get('https://api.polyhaven.com/files/' + name, meta_path)
    metadata = json.loads(meta_path.read_text())
    folder = CACHE / 'PolyHaven' / name
    files = []
    if name in MODELS:
        resolutions = metadata['fbx']
        resolution = '2k' if '2k' in resolutions else next(iter(resolutions))
        source = resolutions[resolution]['fbx']
        path = folder / (name + '.fbx')
        value = get(source['url'], path)
        files.append(dict(role='source-model', path=str(path.relative_to(ROOT)), url=source['url'], sha256=value))
    for key, resolutions in metadata.items():
        if not any(key.lower() == suffix or key.lower().endswith('_' + suffix)
                   for suffix in ['diff', 'diffuse', 'nor_gl', 'arm', 'alpha', 'rough', 'metal', 'ao']):
            continue
        if '2k' not in resolutions:
            continue
        formats = resolutions['2k']
        ext = 'jpg' if 'jpg' in formats else 'png' if 'png' in formats else None
        if not ext:
            continue
        entry = formats[ext]
        path = folder / Path(entry['url']).name
        value = get(entry['url'], path)
        destination = OUT / 'Textures' / name / path.name
        destination.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(path, destination)
        files.append(dict(role='texture', channel=key, path=str(destination.relative_to(ROOT)),
                          sourcePath=str(path.relative_to(ROOT)), url=entry['url'], sha256=value))
    print('PHASE4_FETCHED', name, len(files), flush=True)
    return dict(id=name, kind='model' if name in MODELS else 'surface',
                sourcePage='https://polyhaven.com/a/' + name,
                license='CC0-1.0', licenseUrl='https://polyhaven.com/license',
                providerCredit='Powered by Poly Haven', files=files)


def objaverse(uid, name):
    # The public mirror preserves the creator's original CC attribution metadata.
    index = ROOT / 'Builds/ArtSources/Phase3/object-paths.json.gz'
    if not index.exists():
        index = CACHE / 'object-paths.json.gz'
        get('https://huggingface.co/datasets/allenai/objaverse/resolve/main/object-paths.json.gz', index)
    paths = json.loads(gzip.decompress(index.read_bytes()))
    relative = paths[uid]
    part = relative.split('/')[1]
    metadata_path = CACHE / 'Metadata' / (part + '.json.gz')
    get('https://huggingface.co/datasets/allenai/objaverse/resolve/main/metadata/' + part + '.json.gz', metadata_path)
    metadata = json.loads(gzip.decompress(metadata_path.read_bytes()))[uid]
    assert metadata['license'] in ['cc0', 'by'], (name, metadata['license'])
    url = 'https://huggingface.co/datasets/allenai/objaverse/resolve/main/' + relative
    path = CACHE / 'Models' / (name + '.glb')
    value = get(url, path)
    return dict(id=name, sourceUid=uid, kind='model', title=metadata['name'],
                author=metadata['user']['displayName'], sourcePage=metadata['viewerUrl'],
                license='CC0-1.0' if metadata['license'] == 'cc0' else 'CC-BY-4.0',
                files=[dict(role='source-model', path=str(path.relative_to(ROOT)), url=url, sha256=value)])


def main():
    CACHE.mkdir(parents=True, exist_ok=True)
    old = set()
    for manifest in (ROOT / 'Audit').glob('*asset-sources.json'):
        if manifest.name.startswith('phase4-'):
            continue
        old.update(name.lower() for name in re.findall(r'https://polyhaven.com/a/([A-Za-z0-9_-]+)', manifest.read_text()))
    # Inspect tracked identities only, never read or index previous art files.
    for filename in subprocess.check_output(['git', 'ls-files', 'Assets/_LayarTerakhir/assets/Art'], cwd=ROOT, text=True).splitlines():
        if '/Phase4/' not in filename and filename.endswith('.fbx'):
            old.add(Path(filename).stem.lower().removesuffix('_2k').removesuffix('_1k'))
    (CACHE / 'previous-ph-identities.json').write_text(json.dumps(sorted(old)) + '\n')
    assert len({name.lower() for name in MODELS}) == len(MODELS)
    assert not old.intersection(name.lower() for name in MODELS), 'Previous phase model selected'
    records, failures = [], []
    with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
        jobs = {pool.submit(ph, name): name for name in MODELS + SURFACES}
        for job in concurrent.futures.as_completed(jobs):
            try:
                records.append(job.result())
            except Exception as error:
                failures.append(dict(id=jobs[job], error=str(error)))
                print('PHASE4_FETCH_FAILED', jobs[job], str(error), flush=True)
    for uid, name in HEROES:
        try:
            records.append(objaverse(uid, name))
        except Exception as error:
            failures.append(dict(id=name, error=str(error)))
    report = dict(sources=sorted(records, key=lambda item: item['id']), failures=failures,
                  uniqueNewModelSources=sum(item['kind'] == 'model' for item in records),
                  policy='100 distinct newly downloaded, licensed model sources, excluding common campaign actor/phone/Kos; actual placement is checked separately')
    (ROOT / 'Audit/phase4-asset-sources.json').write_text(json.dumps(report, indent=2) + '\n')
    print('PHASE4_FETCH_COMPLETE', report['uniqueNewModelSources'], 'models', len(failures), 'failures', flush=True)


if __name__ == '__main__':
    main()
