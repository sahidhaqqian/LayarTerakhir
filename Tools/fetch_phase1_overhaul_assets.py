"""Fetch 100+ additional CC0 Poly Haven environment models for the Phase 1
visual overhaul. No textures/models are authored here; ARM channels are only
repacked into the channel layout the existing import pipeline expects."""
import concurrent.futures
import hashlib
import json
from pathlib import Path

import requests
from PIL import Image, ImageOps

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Phase1/Imported'
IDS = (
    # Exterior and arrival
    'concrete_road_barrier concrete_road_barrier_02 modular_chainlink_fence large_iron_gate '
    'street_lamp_01 street_lamp_02 fire_hydrant old_tyre rusted_wheel_rim_01 wooden_barrels_01 '
    'Barrel_01 Barrel_02 barrel_03 planter_box_01 planter_box_02 exterior_aircon_unit '
    'modular_electricity_poles security_light '
    # Floor 1 pump / service
    'modular_industrial_pipes_01 modular_pipes modular_pipes_plastic_01 modular_airduct_rectangular_01 '
    'modular_airduct_circular_01 old_military_compressor portable_welding_cart bench_vice_01 '
    'drill_press_01 metal_tool_chest tool_cart hand_truck wooden_ladder ladder_sectioned_01 '
    'crowbar_01 pipe_wrench sledgehammer_01 korean_fire_extinguisher_01 '
    # Floor 2 residential / archive
    'plastic_monobloc_chair_01 painted_wooden_chair_01 painted_wooden_chair_02 painted_wooden_table '
    'painted_wooden_sofa painted_wooden_cabinet painted_wooden_cabinet_02 painted_wooden_shelves '
    'painted_wooden_nightstand wooden_bookshelf_worn vintage_day_bed old_bed_frame television_02 '
    'boombox vintage_suitcase wicker_basket_02 plastic_crate_01 plastic_crate_03 standing_chalkboard_01 sungka_board '
    # Floor 3 cleaning / sealed corridor
    'WetFloorSign_01 dustpan plunger all_purpose_cleaner multi_cleaner_bottle multi_cleaner_5_litre '
    'drain_cleaner cleaner_tin_01 wooden_broom spray_paint_bottles industrial_storage_cart '
    'hanging_industrial_lamp industrial_caged_sconce pull_chain_light_socket lightbulb_led security_camera_01 '
    # Floor 4 rooms / communal / ritual / boss
    'GothicCabinet_01 GothicBed_01 GothicCommode_01 gothic_coffee_table brass_candleholders '
    'wooden_candlestick brass_diya_lantern Lantern_01 vintage_oil_lamp ornate_mirror_01 '
    'hanging_picture_frame_01 hanging_picture_frame_02 hanging_picture_frame_03 standing_picture_frame_01 '
    'fancy_picture_frame_01 antique_ceramic_vase_01 ceramic_vase_01 ceramic_vase_02 brass_vase_01 '
    'carved_wooden_elephant tea_set_01 pot_enamel_01 electric_stove brass_pot_01 brass_pot_02 '
    'jug_01 wooden_bowl_01 wooden_bowl_02 '
    # Shared architecture / signage / support
    'modular_metal_gutter modular_electric_cables rollershutter_door rollershutter_window_01 '
    'security_camera_02 industrial_wall_lamp industrial_wall_sconce modern_ceiling_lamp_01 '
    'mantel_clock_01 alarm_clock_01 book_encyclopedia_set_01 decorative_book_set_01 binder_notebook '
    'clipboard postcard_set_01 megaphone_01 gate_latch_01 water_manhole_cover '
    # Buffer (used if any primary is multi-material / unavailable)
    'SchoolChair_01 SchoolDesk_01 WoodenChair_01 dining_table side_table_01 steel_frame_shelves_01 '
    'steel_frame_shelves_02 drawer_cabinet round_wooden_table_01 small_wooden_table_01 metal_stool_01 metal_stool_02'
).split()
HEADERS = {'User-Agent': 'Mozilla/5.0 LayarTerakhir Offline Asset Import'}
catalog = json.loads(Path('/tmp/opencode/ph-models.json').read_text())


def metadata(name):
    if (OUT / name / (name + '.fbx')).exists():
        return ('existing', name, None)
    try:
        response = requests.get('https://api.polyhaven.com/files/' + name, headers=HEADERS, timeout=120)
        response.raise_for_status()
        data = response.json()
    except Exception as error:
        print('SKIP_FETCH', name, error, flush=True)
        return None
    if not all(any(k in data for k in keys) for keys in [('Diffuse', 'diff'), ('nor_gl',), ('arm',)]):
        print('SKIP_MULTIMATERIAL', name, flush=True)
        return None
    if 'fbx' not in data or '1k' not in data.get('fbx', {}):
        print('SKIP_NO_FBX', name, flush=True)
        return None
    return ('new', name, data)


with concurrent.futures.ThreadPoolExecutor(max_workers=4) as pool:
    selected = [item for item in pool.map(metadata, IDS) if item]
print('SELECTED', len(selected), flush=True)


def download(item):
    kind, name, data = item
    folder = OUT / name
    folder.mkdir(parents=True, exist_ok=True)
    if kind == 'existing':
        return dict(id=name, alreadyInProject=True, sourcePage='https://polyhaven.com/a/' + name,
                    license='CC0-1.0', licenseUrl='https://polyhaven.com/license',
                    authors=catalog[name].get('authors'), sourceTriangles=catalog[name].get('polycount'), files=[])
    wanted = [(name + '.fbx', data['fbx']['1k']['fbx'])]
    for slot, keys in [('Diffuse', ['Diffuse', 'diff']), ('Normal', ['nor_gl']), ('ARM', ['arm'])]:
        key = next(k for k in keys if k in data)
        entry = data[key]['1k']
        extension = 'jpg' if 'jpg' in entry else 'png'
        wanted.append((name + slot + '.' + extension, entry[extension]))
    files = []
    for filename, entry in wanted:
        path = folder / filename
        if not path.exists():
            response = requests.get(entry['url'], headers=HEADERS, timeout=180)
            response.raise_for_status()
            path.write_bytes(response.content)
        payload = path.read_bytes()
        assert not entry.get('md5') or hashlib.md5(payload).hexdigest() == entry['md5']
        files.append(dict(file=str(path.relative_to(ROOT)), url=entry['url'], sha256=hashlib.sha256(payload).hexdigest()))
    ao, rough, metal = Image.open(next(p for p in folder.glob(name + 'ARM.*') if p.suffix != '.meta')).convert('RGB').split()
    empty = Image.new('L', ao.size)
    Image.merge('RGBA', (metal, empty, empty, ImageOps.invert(rough))).save(folder / (name + 'MetalSmooth.png'))
    Image.merge('RGB', (ao, ao, ao)).save(folder / (name + 'Occlusion.png'))
    print('OVERHAUL_PROP', name, flush=True)
    return dict(id=name, sourcePage='https://polyhaven.com/a/' + name, license='CC0-1.0',
                licenseUrl='https://polyhaven.com/license', authors=catalog[name].get('authors'),
                sourceTriangles=catalog[name].get('polycount'), files=files)


with concurrent.futures.ThreadPoolExecutor(max_workers=3) as pool:
    records = list(pool.map(download, selected))
(ROOT / 'Audit/phase1-overhaul-asset-sources.json').write_text(json.dumps(dict(
    minimumDistinctModels=100, downloadedDistinctModels=len(records),
    attribution='Powered by Poly Haven', records=records), indent=2) + '\n')
print('PHASE1_OVERHAUL_MODELS_DOWNLOADED', len(records))
