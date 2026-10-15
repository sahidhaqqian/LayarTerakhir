"""Fetch the verified CC0/CC-BY asset set for LayarTerakhir Prolog Adegan 2
(Halaman & Teras Rumah Keluarga Atmaja, exterior, ~23:52 WIB).

Sources:
  * Poly Haven (CC0-1.0) models + PBR textures via the public API.
  * OpenGameArt items with page-verified licenses:
      - wooden-door-0 (CC0)           - old wooden door
      - breakable-doors-and-windows (CC0) - doors/windows incl. broken glass
      - chain (CC-BY 4.0)             - rusty chain (attribution required)
      - small-old-house (CC-BY 3.0)   - weathered house shell (credit Vinrax)

Every downloaded byte is checked against the API-provided MD5 (Poly Haven) or
recorded SHA-256 (OpenGameArt archives), and the shipped files are recorded in
manifest/source-manifest.json. OGA textures are resized to <=1024 px; the
processing is recorded per file. Nothing under Assets/ or ProjectSettings/ is
touched.

Usage:  /usr/bin/python3 Scene2_Atmaja_Assets/tools/fetch_scene2_assets.py [asset_id ...]
"""
from io import BytesIO
from pathlib import Path
import hashlib
import io
import json
import re
import sys
import time
import zipfile

import requests
from PIL import Image

REPO = Path(__file__).resolve().parents[2]
PACKAGE = REPO / 'Scene2_Atmaja_Assets'
CACHE = PACKAGE / '.cache'
MANIFEST = PACKAGE / 'manifest' / 'source-manifest.json'
USER_AGENT = 'LayarTerakhir-AssetResearch/2.0'
MAX_TEX = 1024

SESSION = requests.Session()
SESSION.headers['User-Agent'] = USER_AGENT

PH_MODELS = [
    # id, category, scene target
    ('large_iron_gate', 'Architecture'),
    ('gate_latch_01', 'Architecture'),
    ('modular_metal_gutter', 'Architecture'),
    ('modular_pipes_plastic_01', 'Architecture'),
    ('modular_electricity_poles', 'Architecture'),
    ('water_manhole_cover', 'Architecture'),
    ('exterior_aircon_unit', 'Architecture'),
    ('security_light', 'Architecture'),
    ('industrial_wall_lamp', 'Architecture'),
    ('street_lamp_02', 'Architecture'),
    ('pachira_aquatica_01', 'Vegetation'),
    ('shrub_02', 'Vegetation'),
    ('shrub_03', 'Vegetation'),
    ('shrub_04', 'Vegetation'),
    ('fern_02', 'Vegetation'),
    ('weed_plant_02', 'Vegetation'),
    ('nettle_plant', 'Vegetation'),
    ('grass_bermuda_01', 'Vegetation'),
    ('moss_01', 'Vegetation'),
    ('dead_tree_trunk', 'Vegetation'),
    ('root_cluster_01', 'Vegetation'),
    ('dry_branches_medium_01', 'Vegetation'),
    ('tree_stump_01', 'Vegetation'),
    ('rock_moss_set_01', 'Vegetation'),
    ('rock_07', 'Vegetation'),
    ('plastic_monobloc_chair_01', 'Props'),
    ('metal_trash_can', 'Props'),
    ('trashbag', 'Props'),
    ('watering_can_metal_01', 'Props'),
    ('planter_pot_clay', 'Props'),
    ('planter_box_02', 'Props'),
    ('garden_hose_wall_mounted_01', 'Props'),
    ('rusted_spade_01', 'Props'),
    ('machete', 'Props'),
    ('old_tyre', 'Props'),
    ('rusted_wheel_rim_01', 'Props'),
    ('wooden_bucket_01', 'Props'),
    ('cement_bag', 'Props'),
    ('small_lpg_tank', 'Props'),
    ('oil_tin', 'Props'),
    ('can_rusted', 'Props'),
    ('wooden_lantern_01', 'Props'),
    ('pastic_torch_6v', 'Props'),
]

PH_TEXTURES = [
    ('clay_roof_tiles_02', 'Architecture'),
    ('clay_roof_tiles_03', 'Architecture'),
    ('rusty_corrugated_iron', 'Architecture'),
    ('corrugated_iron_03', 'Architecture'),
    ('worn_mossy_plasterwall', 'Architecture'),
    ('damaged_plaster', 'Architecture'),
    ('grey_plaster_03', 'Architecture'),
    ('peeling_painted_wall', 'Architecture'),
    ('brick_moss_001', 'Architecture'),
    ('mossy_stone_wall', 'Architecture'),
    ('concrete_moss', 'Architecture'),
    ('wood_peeling_paint_weathered', 'Architecture'),
    ('wood_planks_grey', 'Architecture'),
    ('rusty_metal_sheet', 'Architecture'),
    ('bark_brown_02', 'Vegetation'),
    ('brown_mud_03', 'Ground_Materials'),
    ('brown_mud_02', 'Ground_Materials'),
    ('stony_dirt_path', 'Ground_Materials'),
    ('mossy_cobblestone', 'Ground_Materials'),
    ('grassy_cobblestone', 'Ground_Materials'),
    ('dirty_concrete', 'Ground_Materials'),
    ('concrete_floor_damaged_01', 'Ground_Materials'),
]

OGA_PAGES = {
    'wooden-door-0': 'https://opengameart.org/content/wooden-door-0',
    'breakable-doors-and-windows': 'https://opengameart.org/content/breakable-doors-and-windows',
    'chain': 'https://opengameart.org/content/chain',
    'small-old-house': 'https://opengameart.org/content/small-old-house',
}


def sha256_bytes(payload: bytes) -> str:
    return hashlib.sha256(payload).hexdigest()


def fetch(url: str) -> requests.Response:
    response = SESSION.get(url, timeout=300)
    response.raise_for_status()
    return response


def fetch_bytes(url: str, expect_md5: str = None) -> bytes:
    content = fetch(url).content
    if expect_md5:
        digest = hashlib.md5(content).hexdigest()
        assert digest == expect_md5, f'MD5 mismatch {url}: {digest} != {expect_md5}'
    return content


def save(path: Path, content: bytes, record: dict, source: str, processing: str = None):
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_bytes(content)
    entry = {
        'path': str(path.relative_to(PACKAGE)),
        'bytes': len(content),
        'sha256': sha256_bytes(content),
        'source': source,
    }
    if processing:
        entry['processing'] = processing
    record['files'].append(entry)


def source_preview_url(info: dict) -> str:
    url = info['thumbnail_url']
    return re.sub(r'width=\d+&height=\d+', 'width=512&height=512', url)


# ---------------------------------------------------------------- Poly Haven
def fetch_ph_model(asset_id: str, category: str) -> dict:
    info = fetch('https://api.polyhaven.com/info/' + asset_id).json()
    files = fetch('https://api.polyhaven.com/files/' + asset_id).json()
    folder = PACKAGE / category / asset_id
    record = {
        'id': asset_id,
        'category': category,
        'source': 'polyhaven',
        'sourcePage': 'https://polyhaven.com/a/' + asset_id,
        'name': info['name'],
        'authors': info['authors'],
        'license': 'CC0-1.0',
        'licenseUrl': 'https://polyhaven.com/license',
        'attributionRequired': False,
        'polygonsSourceApi': info.get('polycount'),
        'sourceDimensionsMm': info.get('dimensions'),
        'maxResolution': info.get('max_resolution'),
        'files': [],
    }
    entry = files['fbx']['1k']['fbx']
    name = asset_id + '_1k.fbx'
    save(folder / name, fetch_bytes(entry['url'], entry['md5']), record, entry['url'])
    for include_name, include_entry in files['gltf']['1k']['gltf']['include'].items():
        if include_name.endswith('.bin') or not include_name.lower().endswith(('.jpg', '.png')):
            continue
        tex_name = Path(include_name).name
        save(folder / 'textures' / tex_name,
             fetch_bytes(include_entry['url'], include_entry['md5']), record, include_entry['url'])
    save(folder / 'source-preview.png', fetch_bytes(source_preview_url(info)),
         record, source_preview_url(info))
    print('PH MODEL', asset_id, flush=True)
    return record


def fetch_ph_texture(asset_id: str, category: str) -> dict:
    info = fetch('https://api.polyhaven.com/info/' + asset_id).json()
    files = fetch('https://api.polyhaven.com/files/' + asset_id).json()
    folder = PACKAGE / category / asset_id
    record = {
        'id': asset_id,
        'category': category,
        'source': 'polyhaven',
        'type': 'texture',
        'sourcePage': 'https://polyhaven.com/a/' + asset_id,
        'name': info['name'],
        'authors': info['authors'],
        'license': 'CC0-1.0',
        'licenseUrl': 'https://polyhaven.com/license',
        'attributionRequired': False,
        'maxResolution': info.get('max_resolution'),
        'maps': [],
        'files': [],
    }
    for key, suffix in (('Diffuse', 'diff'), ('nor_gl', 'nor_gl'), ('arm', 'arm')):
        entry = files[key]['1k']['jpg']
        name = f'{asset_id}_{suffix}_1k.jpg'
        save(folder / name, fetch_bytes(entry['url'], entry['md5']), record, entry['url'])
        record['maps'].append(suffix)
    preview = fetch_bytes(source_preview_url(info))
    save(folder / 'source-preview.png', preview, record, source_preview_url(info))
    diffuse = (folder / f'{asset_id}_diff_1k.jpg').read_bytes()
    image = Image.open(BytesIO(diffuse)).convert('RGB')
    image.thumbnail((512, 512))
    buffer = BytesIO()
    image.save(buffer, 'PNG', optimize=True)
    path = folder / 'preview.png'
    path.write_bytes(buffer.getvalue())
    record['files'].append({
        'path': str(path.relative_to(PACKAGE)),
        'bytes': len(buffer.getvalue()),
        'sha256': sha256_bytes(buffer.getvalue()),
        'source': 'derived from ' + record['files'][0]['source'],
        'processing': 'diffuse resized to 512 px',
    })
    print('PH TEXTURE', asset_id, flush=True)
    return record


# --------------------------------------------------------------- OpenGameArt
def cached_archive(url: str) -> bytes:
    CACHE.mkdir(parents=True, exist_ok=True)
    name = url.rsplit('/', 1)[-1]
    path = CACHE / name
    if path.is_file():
        return path.read_bytes()
    content = fetch_bytes(url)
    path.write_bytes(content)
    return content


def resize_save(source: bytes, destination: Path, record: dict, origin: str, forced_format: str = None):
    image = Image.open(io.BytesIO(source))
    original_size = image.size
    image.thumbnail((MAX_TEX, MAX_TEX), Image.LANCZOS)
    fmt = forced_format or image.format or 'PNG'
    buffer = io.BytesIO()
    if fmt.upper() in ('JPG', 'JPEG'):
        if image.mode not in ('RGB', 'L'):
            image = image.convert('RGB')
        image.save(buffer, 'JPEG', quality=90, optimize=True)
    else:
        if image.mode not in ('RGB', 'RGBA', 'L'):
            image = image.convert('RGB')
        image.save(buffer, 'PNG', optimize=True)
    processing = f'resized {original_size[0]}x{original_size[1]} -> {image.size[0]}x{image.size[1]}'
    if not forced_format and image.format is None:
        processing += ' (PNG re-encode)'
    save(destination, buffer.getvalue(), record, origin, processing)


def fetch_oga_wooden_door() -> dict:
    url = 'https://opengameart.org/sites/default/files/Wooden%20Door.zip'
    content = cached_archive(url)
    record = {
        'id': 'wooden-door-0',
        'category': 'Story_Objects',
        'source': 'opengameart',
        'sourcePage': OGA_PAGES['wooden-door-0'],
        'name': 'Wooden Door',
        'authors': ['Lucian Pavel'],
        'license': 'CC0-1.0',
        'licenseUrl': 'https://creativecommons.org/publicdomain/zero/1.0/',
        'attributionRequired': False,
        'sourceArchive': {'url': url, 'bytes': len(content), 'sha256': sha256_bytes(content)},
        'files': [],
    }
    folder = PACKAGE / 'Story_Objects' / 'wooden-door-0'
    with zipfile.ZipFile(io.BytesIO(content)) as archive:
        members = {Path(n).name: n for n in archive.namelist() if not n.startswith('__MACOSX')}
        save(folder / 'Wooden_Door.fbx', archive.read(members['Wooden Door.fbx']), record,
             url + ' :: Wooden Door.fbx')
        for name, out in [('Wooden Door.png', 'Wooden_Door_basecolor.png'),
                          ('Wooden Door NRM.png', 'Wooden_Door_normal.png'),
                          ('Wooden Door SPEC.png', 'Wooden_Door_specular.png')]:
            resize_save(archive.read(members[name]), folder / out, record,
                        url + ' :: ' + name,
                        forced_format='PNG' if out.endswith('.png') else None)
    save(folder / 'source-preview.png',
         fetch_bytes('https://opengameart.org/sites/default/files/Wooden%20Door%20Preview.png'),
         record, 'https://opengameart.org/sites/default/files/Wooden%20Door%20Preview.png')
    print('OGA wooden-door-0', flush=True)
    return record


def fetch_oga_doors_windows() -> dict:
    url = 'https://opengameart.org/sites/default/files/doorsandwindows_fbx_gltf_textures_blend.zip'
    content = cached_archive(url)
    record = {
        'id': 'breakable-doors-and-windows',
        'category': 'Story_Objects',
        'source': 'opengameart',
        'sourcePage': OGA_PAGES['breakable-doors-and-windows'],
        'name': 'Breakable Doors and Windows',
        'authors': ['loafbrr_1'],
        'license': 'CC0-1.0',
        'licenseUrl': 'https://creativecommons.org/publicdomain/zero/1.0/',
        'attributionRequired': False,
        'sourceArchive': {'url': url, 'bytes': len(content), 'sha256': sha256_bytes(content)},
        'files': [],
    }
    folder = PACKAGE / 'Story_Objects' / 'breakable-doors-and-windows'
    fbx_names = [
        'Door_DoorFrame_Single_1.fbx', 'Door_DoorFrame_Single_2.fbx',
        'Door_DoorFrame_Double.fbx',
        'Door_DoorFrame_Broken_Single_1.fbx', 'Door_DoorFrame_Broken_single_2.fbx',
        'Door_DoorFrame_Broken_Double.fbx',
        'Windows_Window_1.fbx', 'Windows_Window_2.fbx',
        'Windows_BrokenWindow_1.fbx', 'Windows_BrokenWindow_2.fbx',
    ]
    texture_names = [
        'Base _Color_1_BreakableEntrances.png', 'Base _Color_2_BreakableEntrances.png',
        'Base _Color_3_BreakableEntrances.png', 'Base _Color_4_BreakableEntrances.png',
        'BASE_COLOR_1_BreakableEntrances.png', 'BASE_COLOR_2_BreakableEntrances.png',
        'BASE_COLOR_3_BreakableEntrances.png', 'BASE_COLOR_4_BreakableEntrances.png',
        'NORMAL_BreakableEntrances.png', 'ROUGHNESS_BreakableEntrances.png',
        'METALLIC_BreakableEntrances.png', 'AO_BreakableEntrances.png',
        'DamageMask_DoorsWindows.png', 'EdgeMask1k_DoorsWindows.png',
    ]
    with zipfile.ZipFile(io.BytesIO(content)) as archive:
        for name in fbx_names:
            save(folder / 'fbx' / name, archive.read('FBXs/' + name), record,
                 url + ' :: FBXs/' + name)
        for name in texture_names:
            origin = url + ' :: Textures/'
            member = ('Textures/BreakableEntrances/' + name if 'BreakableEntrances' in name
                      else 'Textures/DoorsWindows/' + name)
            resize_save(archive.read(member), folder / 'textures' / name.replace(' ', '_'),
                        record, origin + member)
        save(folder / 'README.txt', archive.read('README.txt'), record, url + ' :: README.txt')
    save(folder / 'source-preview.png',
         fetch_bytes('https://opengameart.org/sites/default/files/godotpreview1.png'),
         record, 'https://opengameart.org/sites/default/files/godotpreview1.png')
    print('OGA breakable-doors-and-windows', flush=True)
    return record


def fetch_oga_chain() -> dict:
    import py7zr
    import tempfile
    url = 'https://opengameart.org/sites/default/files/chain.7z'
    content = cached_archive(url)
    record = {
        'id': 'chain',
        'category': 'Story_Objects',
        'source': 'opengameart',
        'sourcePage': OGA_PAGES['chain'],
        'name': 'Chain',
        'authors': ['Kutejnikov'],
        'license': 'CC-BY-4.0',
        'licenseUrl': 'https://creativecommons.org/licenses/by/4.0/',
        'attributionRequired': True,
        'attributionNote': 'Credit "Kutejnikov" (OpenGameArt, https://opengameart.org/content/chain), '
                           'license CC BY 4.0; changes: archive extraction, texture resize to 1024 px.',
        'sourceArchive': {'url': url, 'bytes': len(content), 'sha256': sha256_bytes(content)},
        'files': [],
    }
    folder = PACKAGE / 'Story_Objects' / 'chain'
    wanted = {
        'chain.FBX': 'chain.fbx',
        'DefaultMaterial_Base_Color.png': 'textures/DefaultMaterial_Base_Color.png',
        'DefaultMaterial_Normal_OpenGL.png': 'textures/DefaultMaterial_Normal_OpenGL.png',
        'DefaultMaterial_Roughness.png': 'textures/DefaultMaterial_Roughness.png',
        'DefaultMaterial_Metallic.png': 'textures/DefaultMaterial_Metallic.png',
        'DefaultMaterial_Mixed_AO.png': 'textures/DefaultMaterial_Mixed_AO.png',
    }
    with tempfile.TemporaryDirectory() as tmp:
        with py7zr.SevenZipFile(io.BytesIO(content)) as archive:
            archive.extract(path=tmp, targets=list(wanted.keys()))
        for member, out in wanted.items():
            payload = (Path(tmp) / member).read_bytes()
            if member == 'chain.FBX':
                save(folder / out, payload, record, url + ' :: ' + member)
            else:
                resize_save(payload, folder / out, record, url + ' :: ' + member)
    save(folder / 'source-preview.png',
         fetch_bytes('https://opengameart.org/sites/default/files/screenshot007_2.jpg'),
         record, 'https://opengameart.org/sites/default/files/screenshot007_2.jpg')
    print('OGA chain', flush=True)
    return record


def fetch_oga_house() -> dict:
    url = 'https://opengameart.org/sites/default/files/House.zip'
    content = cached_archive(url)
    record = {
        'id': 'small-old-house',
        'category': 'Architecture',
        'source': 'opengameart',
        'sourcePage': OGA_PAGES['small-old-house'],
        'name': 'Small Old House',
        'authors': ['Vinrax (vinrax@gmail.com)'],
        'license': 'CC-BY-3.0',
        'licenseUrl': 'https://creativecommons.org/licenses/by/3.0/',
        'attributionRequired': True,
        'attributionNote': 'Credit "Vinrax (vinrax@gmail.com)" (OpenGameArt, '
                           'https://opengameart.org/content/small-old-house), license CC BY 3.0; '
                           'changes: textures resized to 1024 px, MTL path normalized.',
        'sourceArchive': {'url': url, 'bytes': len(content), 'sha256': sha256_bytes(content)},
        'files': [],
    }
    folder = PACKAGE / 'Architecture' / 'small-old-house'
    with zipfile.ZipFile(io.BytesIO(content)) as archive:
        names = archive.namelist()
        save(folder / 'House.obj', archive.read('House.obj'), record, url + ' :: House.obj')
        mtl = archive.read('House.mtl').decode('utf-8', 'replace')
        mtl = mtl.replace('tex/', 'tex/')
        save(folder / 'House.mtl', mtl.encode(), record, url + ' :: House.mtl')
        for name, out in [('tex/house_tex.png', 'tex/house_tex.png'),
                          ('tex/house_nor.jpg', 'tex/house_nor.jpg'),
                          ('tex/house_spec.jpg', 'tex/house_spec.jpg')]:
            forced = 'JPEG' if out.endswith('.jpg') else 'PNG'
            resize_save(archive.read(name), folder / out, record, url + ' :: ' + name,
                        forced_format=forced)
        save(folder / 'readme.txt', archive.read('readme.txt'), record, url + ' :: readme.txt')
    print('OGA small-old-house', flush=True)
    return record


OGA_FETCHERS = {
    'wooden-door-0': fetch_oga_wooden_door,
    'breakable-doors-and-windows': fetch_oga_doors_windows,
    'chain': fetch_oga_chain,
    'small-old-house': fetch_oga_house,
}


def main() -> int:
    requested = set(sys.argv[1:])
    previous = {}
    if MANIFEST.exists():
        previous = {r['id']: r for r in json.loads(MANIFEST.read_text())}
    ph_kind = {asset_id: 'model' for asset_id, _ in PH_MODELS}
    ph_kind.update({asset_id: 'texture' for asset_id, _ in PH_TEXTURES})
    for asset_id, category in PH_MODELS + PH_TEXTURES:
        if requested and asset_id not in requested:
            continue
        try:
            if ph_kind[asset_id] == 'model':
                previous[asset_id] = fetch_ph_model(asset_id, category)
            else:
                previous[asset_id] = fetch_ph_texture(asset_id, category)
        except Exception as error:
            print('FAILED PH', asset_id, repr(error), flush=True)
    for oga_id, fetcher in OGA_FETCHERS.items():
        if requested and oga_id not in requested:
            continue
        try:
            previous[oga_id] = fetcher()
        except Exception as error:
            print('FAILED OGA', oga_id, repr(error), flush=True)

    ordered = []
    for asset_id, _ in PH_MODELS + PH_TEXTURES:
        if asset_id in previous:
            ordered.append(previous[asset_id])
    for oga_id in OGA_FETCHERS:
        if oga_id in previous:
            ordered.append(previous[oga_id])
    MANIFEST.parent.mkdir(parents=True, exist_ok=True)
    MANIFEST.write_text(json.dumps(ordered, indent=2) + '\n')
    print('manifest:', MANIFEST, len(ordered), 'assets')
    return 0


if __name__ == '__main__':
    raise SystemExit(main())
