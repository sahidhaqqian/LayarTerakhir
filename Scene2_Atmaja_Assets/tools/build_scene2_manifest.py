"""Build the reviewed Scene 2 deliverable documents from the download record,
verification report, measured geometry and the curation notes below.

Outputs (package root):
  asset-manifest.json   machine-readable, maps every asset to its Scene 2 usage
  asset-licenses.md     license summary + attribution obligations (CC-BY items)
  missing-assets.md     honest gaps and recommended workarounds
  Previews/             category contact sheets built from per-asset previews

Usage:  /usr/bin/python3 Scene2_Atmaja_Assets/tools/build_scene2_manifest.py
"""
from pathlib import Path
import json

from PIL import Image, ImageDraw, ImageFont

REPO = Path(__file__).resolve().parents[2]
PACKAGE = REPO / 'Scene2_Atmaja_Assets'
SOURCE = PACKAGE / 'manifest' / 'source-manifest.json'
VERIFICATION = PACKAGE / 'manifest' / 'verification-report.json'
MEASURED = PACKAGE / 'manifest' / 'measured-bounds.json'
OUT_JSON = PACKAGE / 'asset-manifest.json'
OUT_LICENSES = PACKAGE / 'asset-licenses.md'
OUT_MISSING = PACKAGE / 'missing-assets.md'

TARGET = ('Prolog Adegan 2 - Halaman & Teras Rumah Keluarga Atmaja '
          '(exterior, Rumah Jawa-Belanda, ~23:52 WIB, hujan)')

CURATION = {
    # ---------------------------------------------------------- Architecture
    'large_iron_gate': {
        'usage': 'Rusty iron front gate where Raka stands at 23:52; the boundary gate to the street.',
        'placement': 'Front boundary line, centered on the path to the terrace.',
        'notes': 'Plain dark iron material; combine with rusty_metal_sheet / rust_coarse tint if a '
                 'more corroded look is needed. 38k tris raw.',
    },
    'gate_latch_01': {
        'usage': 'Gate latch and hinge closeup; Raka trying the gate before walking to the side path.',
        'placement': 'On the iron gate leaf.',
    },
    'modular_metal_gutter': {
        'usage': 'Roof gutter runs along the eaves of the house and the kitchen annex.',
        'placement': 'Eaves line, follows the roof slope; the set contains several profile pieces.',
    },
    'modular_pipes_plastic_01': {
        'usage': 'PVC drain pipes (paralon) along the wet side path and kitchen wall.',
        'placement': 'Wall runs with 2-3 elbows; the set contains pipe pieces and fittings.',
    },
    'modular_electricity_poles': {
        'usage': 'Wooden utility poles with cross-arms for the dark residential street.',
        'placement': 'Roadside, outside the boundary wall. 200k tris raw - use one pole and '
                 'decimate hard, or rebuild the pole from the included profile pieces.',
        'heavy': True,
    },
    'water_manhole_cover': {
        'usage': 'Round water/drain cover on the yard or street edge; drainage detail Raka walks past.',
        'placement': 'Set flat into the ground; add a thin dirt/moss decal around it.',
    },
    'exterior_aircon_unit': {
        'usage': 'Weathered AC unit on the NEIGHBOURING house facade for street realism.',
        'placement': 'Neighbour facade only - the Atmaja house is abandoned since 1998.',
    },
    'security_light': {
        'usage': 'Old exterior wall light beside the kitchen window / side path; unlit or flickering.',
        'placement': 'High on the side wall above the kitchen window.',
    },
    'industrial_wall_lamp': {
        'usage': 'Wall lamp by the front terrace/door; dark and unlit fixture.',
        'placement': 'Terrace wall next to the front door.',
    },
    'street_lamp_02': {
        'usage': 'Distant street lamp across the road; the light pool at the prolog ending.',
        'placement': 'Opposite side of the road, outside the gate.',
    },
    'clay_roof_tiles_02': {
        'usage': 'Main roof tiles (genteng) for the two-story house and terrace awning.',
        'placement': 'Tiling material; recommended 512-1024 px with normal map.',
    },
    'clay_roof_tiles_03': {
        'usage': 'Aged/alternate tile set for the annex or second roof plane.',
        'placement': 'Tiling material; vary tint per roof plane.',
    },
    'rusty_corrugated_iron': {
        'usage': 'Rusty corrugated sheets: kitchen annex roof, well cover, gate repair panels.',
        'placement': 'Tiling material, sheets and small roof patches.',
    },
    'corrugated_iron_03': {
        'usage': 'Grey corrugated sheets for the secondary shed/roof near the side path.',
        'placement': 'Tiling material.',
    },
    'worn_mossy_plasterwall': {
        'usage': 'Damp mossy plaster for the lower facade walls of the old house.',
        'placement': 'Tiling material; strongest at wall bases and shaded faces.',
    },
    'damaged_plaster': {
        'usage': 'Damaged plaster with exposed brick for weathered facade patches.',
        'placement': 'Tiling material; broken wall near the kitchen window and gate corner.',
    },
    'grey_plaster_03': {
        'usage': 'Neutral base plaster for upper walls, columns and the terrace.',
        'placement': 'Tiling material; tint slightly warm-grey.',
    },
    'peeling_painted_wall': {
        'usage': 'Peeling old paint for shutters, door frames and skirtings (aged cream/salmon).',
        'placement': 'Trim material with adjusted UV scale.',
    },
    'brick_moss_001': {
        'usage': 'Mossy brick for the foundation, plinth and boundary wall base.',
        'placement': 'Tiling material.',
    },
    'mossy_stone_wall': {
        'usage': 'Mossy stone for the garden boundary wall and well surround.',
        'placement': 'Tiling material.',
    },
    'concrete_moss': {
        'usage': 'Damp concrete for drain walls, well surround and side-path retaining edges.',
        'placement': 'Tiling material.',
    },
    'wood_peeling_paint_weathered': {
        'usage': 'Whitewashed peeling wood for old doors and window frames (kolonial white).',
        'placement': 'Door/window frame material.',
    },
    'wood_planks_grey': {
        'usage': 'Weathered grey planks for boards, kitchen window frame and plank fencing.',
        'placement': 'Plank material; also board patches over the broken window.',
    },
    'rusty_metal_sheet': {
        'usage': 'Rusty sheet metal for gate repairs, door plates and chain plates.',
        'placement': 'Metal detail material at 512 px.',
    },
    'bark_brown_02': {
        'usage': 'Bark material for the fallen tree; tint lighter - frangipani bark is smooth pale grey.',
        'placement': 'Retexture atlas for dead_tree_trunk / root_cluster_01.',
    },
    'small-old-house': {
        'usage': 'Base massing for the old house: single-story shell with tiled roof and weathered '
                 'plaster. Kitbash base for the two-story Javanese-Dutch house.',
        'placement': 'Ground the shell on the lot; remove the chimney, add a project-authored second '
                     'story, front terrace (stoep) and window/door openings using the delivered '
                     'textures and window/doors pack.',
        'notes': 'CC-BY 3.0 - credit Vinrax (vinrax@gmail.com) when used. 586 tris.',
    },
    # ------------------------------------------------------------ Vegetation
    'pachira_aquatica_01': {
        'usage': 'Tropical trees for the front yard and surroundings (4 variants in one set).',
        'placement': 'Yard and neighbouring lots; scale/rotate variants. Generic tropical tree, '
                     'NOT the frangipani.',
    },
    'shrub_02': {
        'usage': 'Front-yard bushes around the parked scooter and overgrown beds.',
        'placement': 'Along the boundary and house base.',
    },
    'shrub_03': {
        'usage': 'Small dry shrub for gaps and wall bases.',
        'placement': 'Path edges, wall bases.',
    },
    'shrub_04': {
        'usage': 'Broadleaf shrub for the overgrown garden.',
        'placement': 'Garden beds, under windows.',
    },
    'fern_02': {
        'usage': 'Damp undergrowth along the side path and wall bases.',
        'placement': 'Shaded damp spots along the side corridor.',
    },
    'weed_plant_02': {
        'usage': 'Overgrown weeds between path stones and pavement cracks.',
        'placement': 'Scatter sets; split the row into separate tufts.',
    },
    'nettle_plant': {
        'usage': 'Weedy overgrowth patches in the garden.',
        'placement': 'Garden beds; split scatter rows.',
    },
    'grass_bermuda_01': {
        'usage': 'Grass tufts for the overgrown yard.',
        'placement': 'Scatter; card geometry, very cheap (941 tris) to instance.',
    },
    'moss_01': {
        'usage': 'Moss carpets on damp ground and wall bases.',
        'placement': 'Scatter or scale to patches; only 204 tris.',
    },
    'dead_tree_trunk': {
        'usage': 'The fallen old frangipani trunk blocking the side path - the crouch obstacle (CTRL).',
        'placement': 'Across the side corridor at head height for a 1.6-1.8 m player; clear one '
                     'crouch-height gap. 101k tris raw; decimate or use as the hero piece.',
        'heavy': True,
    },
    'root_cluster_01': {
        'usage': 'Upended root plate at the fallen tree base.',
        'placement': 'At the trunk base against the house side. 225k tris raw - decimate heavily '
                     'or use only the front shell.',
        'heavy': True,
    },
    'dry_branches_medium_01': {
        'usage': 'Fallen crown branches scattered around the trunk and along the path.',
        'placement': 'Scatter set: split into 3-6 branches.',
    },
    'tree_stump_01': {
        'usage': 'Old tree stump at the yard edge.',
        'placement': 'Garden corner.',
    },
    'rock_moss_set_01': {
        'usage': 'Wet mossy stones around the well, drain and path edges.',
        'placement': 'Scatter set; use 3-5 stones.',
    },
    'rock_07': {
        'usage': 'Additional wet stone for the path/garden edge.',
        'placement': 'Path edge.',
    },
    # -------------------------------------------------------- Ground_Materials
    'brown_mud_03': {
        'usage': 'The muddy side pathway (wet, with footprints) Raka walks with the phone light.',
        'placement': 'Ground material for the side corridor.',
    },
    'brown_mud_02': {
        'usage': 'Wet garden soil under bushes and around the well.',
        'placement': 'Ground material for beds.',
    },
    'stony_dirt_path': {
        'usage': 'Stony dirt for the front path and yard transition.',
        'placement': 'Ground material; blend with mud at edges.',
    },
    'mossy_cobblestone': {
        'usage': 'Mossy cobblestone pavement for the front yard path and terrace edge.',
        'placement': 'Ground material.',
    },
    'grassy_cobblestone': {
        'usage': 'Overgrown pavement for the gate threshold and neglected corners.',
        'placement': 'Ground material.',
    },
    'dirty_concrete': {
        'usage': 'Dirty concrete for the terrace slab and drain edges.',
        'placement': 'Ground material.',
    },
    'concrete_floor_damaged_01': {
        'usage': 'Damaged wet concrete for the yard/kitchen apron seen through the broken window.',
        'placement': 'Ground material.',
    },
    # ------------------------------------------------------------------ Props
    'plastic_monobloc_chair_01': {
        'usage': 'Weathered monobloc chair on the terrace/porch.',
        'placement': 'Terrace beside the front door.',
    },
    'metal_trash_can': {
        'usage': 'Dented bin at the side path / kitchen service corner.',
        'placement': 'Against the side wall.',
    },
    'trashbag': {
        'usage': 'Trash bag left near the gate/path.',
        'placement': 'Gate corner or path edge.',
    },
    'watering_can_metal_01': {
        'usage': 'Galvanized watering can by the well/garden.',
        'placement': 'Well edge or garden bed.',
    },
    'planter_pot_clay': {
        'usage': 'Old clay pot on the terrace edge.',
        'placement': 'Terrace/porch edge.',
    },
    'planter_box_02': {
        'usage': 'Wooden planter box at the terrace/garden.',
        'placement': 'Terrace or garden bed.',
    },
    'garden_hose_wall_mounted_01': {
        'usage': 'Wall-mounted garden hose on the side wall (garden clutter).',
        'placement': 'Side wall near the kitchen corner.',
    },
    'rusted_spade_01': {
        'usage': 'Rusty spade leaning by the well/drain; garden-tool motif (no gameplay).',
        'placement': 'Leaning on the well surround or wall.',
    },
    'machete': {
        'usage': 'Old parang/machete on the woodpile or well edge.',
        'placement': 'Flat on the woodpile/bench.',
    },
    'old_tyre': {
        'usage': 'Old tyre in the yard corner.',
        'placement': 'Flat on the ground, dirt patch under.',
    },
    'rusted_wheel_rim_01': {
        'usage': 'Rusted wheel rim leaning on the boundary wall.',
        'placement': 'Boundary wall.',
    },
    'wooden_bucket_01': {
        'usage': 'Wooden bucket by the well (ties into the Adegan 3 well).',
        'placement': 'Well edge.',
    },
    'cement_bag': {
        'usage': 'Leftover cement sacks near the side path (abandoned repairs).',
        'placement': 'Stack of 1-2 near the kitchen corner.',
    },
    'small_lpg_tank': {
        'usage': '3 kg LPG cylinder by the kitchen service door (leftover clutter).',
        'placement': 'Kitchen side, optional.',
    },
    'oil_tin': {
        'usage': 'Kerosene tin by the kitchen wall.',
        'placement': 'Side wall base.',
    },
    'can_rusted': {
        'usage': 'Small rusted can clutter.',
        'placement': 'Scatter 2-3 near the wall.',
    },
    'wooden_lantern_01': {
        'usage': 'Kerosene lantern (lampu teplok) on the terrace/well edge.',
        'placement': 'Terrace or well edge.',
    },
    'pastic_torch_6v': {
        'usage': 'Plastic torch left on the kitchen windowsill / near Damar scooter area.',
        'placement': 'Windowsill or scooter seat area.',
    },
    # --------------------------------------------------------- Story_Objects
    'wooden-door-0': {
        'usage': 'Old weathered wooden door: front door or back kitchen door of the house.',
        'placement': 'Door opening; 1.04 x 2.14 m. CC0 with diffuse/normal/specular maps.',
    },
    'breakable-doors-and-windows': {
        'usage': 'Wooden window frames including partially broken glass: the kitchen window Raka '
                 'vaults through (SPACE), plus facade windows and alternate doors with frames.',
        'placement': 'Kitchen window on the side wall at vault height; facade windows. '
                     'Materials are stylized flat-color variants - apply the delivered wood/plaster '
                     'textures or a grime pass in Unity to match the PBR surroundings.',
    },
    'chain': {
        'usage': 'Rusty chain: front door chained from inside (Damar note), well cover chain, '
                 'gate wrap.',
        'placement': 'Duplicate the 20 cm segment into door/well/gate runs.',
    },
}

REJECTED = [
    ('jacaranda_tree / island_tree_01-03 / tree_small_02 (Poly Haven)',
     'Realistic tropical trees are 132 MB / 1.7-4.8M triangles at 1K - impossible for the '
     'Iris Xe + 8 GB target. pachira_aquatica_01 (77k tris) delivered instead; canopy '
     'trees must be project-authored or decimated.'),
    ('modular_chainlink_fence (Poly Haven)',
     'Construction-site chainlink kit with concrete feet - wrong look for an old Javanese '
     'residential boundary. Masonry boundary wall built from delivered wall textures instead.'),
    ('large_castle_door (Poly Haven)',
     'Arched European castle door; wrong silhouette for the house. CC0 wooden-doors instead.'),
    ('modular_urban_apartments_facade / modular_factory_facade (Poly Haven)',
     'Western industrial facades; visually incompatible with Javanese-Dutch residential.'),
    ('scooters from low-poly CC0 packs (Kenney/Quaternius/Poly Pizza class)',
     'Cartoonish, visually incompatible with the PBR horror target. See missing-assets.md.'),
    ('chain-link-fence (OpenGameArt, CC0)',
     '156-tri tourist-grade chainlink; not needed, the Poly Haven iron gate + masonry '
     'textures cover the boundary.'),
]

GAPS = [
    ('Damar automatic scooter (motor matic)',
     'No suitable model found under a verified commercial license. Marketplaces offer them only '
     'under redistribution-restrictive terms; CC0/low-poly packs are cartoonish. Must be '
     'authored, purchased, or scanned; place with a dim headlight spotlight against the wall.'),
    ('Complete two-story Javanese-Dutch house',
     'No complete model exists under CC0/CC-BY. Delivered: house shell base, iron gate, doors, '
     'windows, roof/wall/wood/rust textures. The second story, terrace (stoep), pillars and '
     'roof awning must be authored over the shell - no CC0 column/pillar models were found.'),
    ('Frangipani (kamboja) tree - living and fallen',
     'Species-specific CC0 model not available. Delivered: dead trunk + roots + branches kit '
     'as the fallen-tree obstacle and tropical tree substitutes; pale smooth bark needs a '
     'lighter bark tint, white flowers must be authored.'),
    ('Javanese carved woodwork (gebyok, ukiran, nok ridge caps)',
     'No CC0 source. Project-authored detail geometry over the delivered wood textures.'),
    ('Indonesian printed signage, house-number plaque, kretek/poster props',
     'No CC0 realistic source; project-authored print props remain the pattern (per kos drop).'),
]


def category_of(record: dict) -> str:
    return record['category']


def build_assets(records, verification, measured):
    verified = {entry['id']: entry['passed'] for entry in verification['assets']}
    assets = []
    for record in records:
        asset_id = record['id']
        curation = CURATION.get(asset_id)
        if curation is None:
            raise KeyError('missing curation for ' + asset_id)
        entry = {
            'id': asset_id,
            'name': record.get('name'),
            'category': category_of(record),
            'source': record['source'],
            'sourcePage': record['sourcePage'],
            'license': record['license'],
            'licenseUrl': record['licenseUrl'],
            'attributionRequired': record['attributionRequired'],
            'authors': record.get('authors'),
            'sceneUsage': curation['usage'],
            'placement': curation.get('placement'),
            'notes': curation.get('notes'),
            'heavyMesh': curation.get('heavy', False),
            'verifiedAgainstSource': verified.get(asset_id, False),
            'files': record['files'],
            'previewLocal': f"{record['category']}/{asset_id}/preview.png"
                            if (PACKAGE / record['category'] / asset_id / 'preview.png').exists()
                            else None,
        }
        if record.get('sourceArchive'):
            entry['sourceArchive'] = record['sourceArchive']
        if record.get('type') == 'texture':
            entry['type'] = 'texture'
            entry['maps'] = record.get('maps')
        else:
            entry['polygonsSourceApi'] = record.get('polygonsSourceApi')
            entry['sourceDimensionsMm'] = record.get('sourceDimensionsMm')
            measured_entry = measured.get(asset_id)
            if measured_entry:
                entry['measuredBoundsM'] = measured_entry['bounds']
                entry['trianglesMeasured'] = measured_entry['triangles']
        for optional in ('attributionNote', 'description', 'maxResolution'):
            if record.get(optional):
                entry[optional] = record[optional]
        assets.append(entry)
    return assets


def write_json(assets, verification):
    models = [a for a in assets if a.get('type') != 'texture']
    textures = [a for a in assets if a.get('type') == 'texture']
    total_tris = sum(a.get('trianglesMeasured', 0) for a in models)
    total_bytes = sum(f['bytes'] for a in assets for f in a['files'])
    payload = {
        'generated': verification['verifiedAt'],
        'target': TARGET,
        'policy': {
            'license': 'CC0-1.0 preferred; CC-BY items delivered with recorded attribution',
            'textureLimitSource': '1K source variants (Poly Haven); OGA textures resized <= 1024 px',
            'recommendedRuntimeTexture': '512 px small props; 512-1024 px architecture surfaces',
            'performanceTarget': 'Intel Iris Xe / 8 GB RAM - decimate heavy meshes (flagged) '
                                 'before placement; instancing expected for vegetation',
            'unityProjectModified': False,
        },
        'summary': {
            'assets': len(assets),
            'models': len(models),
            'textureMaterials': len(textures),
            'measuredModelTriangles': total_tris,
            'shippedBytes': total_bytes,
            'heavyMeshes': [a['id'] for a in models if a.get('heavyMesh')],
        },
        'assets': assets,
        'rejectedCandidates': [{'id': i, 'reason': r} for i, r in REJECTED],
        'gaps': 'see missing-assets.md',
    }
    OUT_JSON.write_text(json.dumps(payload, indent=2) + '\n')
    print('wrote', OUT_JSON)
    return payload


def write_licenses(assets):
    cc0 = [a for a in assets if a['license'] == 'CC0-1.0']
    ccby = [a for a in assets if a['license'] != 'CC0-1.0']
    lines = [
        '# Scene 2 asset licenses - verified, not assumed',
        '',
        f'Target: {TARGET}',
        '',
        '## Evidence',
        '',
        '- **Poly Haven** (models + textures): the site license page states assets are CC0 1.0 '
        '("use our assets for any purpose, including commercial work", "no need to give credit"). '
        'Raw capture: `evidence/polyhaven-license-raw.html`; extracted statements: '
        '`evidence/polyhaven-license.txt`. API terms retained at `evidence/polyhaven-api-tos.md` '
        '(the API is only called by these research scripts, never by the game).',
        '- **OpenGameArt** items: the per-item "License(s)" field was captured from the live pages '
        '(`evidence/opengameart-*.html`, summary in `evidence/opengameart-license-notes.txt`).',
        '- Every shipped file was re-verified against the source: Poly Haven files against the '
        'API MD5, OpenGameArt unmodified members against re-extracted archive members, all files '
        'against the recorded SHA-256 (`manifest/verification-report.json`).',
        '',
        f'## CC0-1.0 assets ({len(cc0)})',
        '',
        'No attribution required; authors credited voluntarily below.',
        '',
        '| Asset | Author(s) | Source |',
        '| --- | --- | --- |',
    ]
    for asset in sorted(cc0, key=lambda a: a['id']):
        authors = ', '.join(asset['authors']) if asset['authors'] else '-'
        lines.append(f"| {asset['id']} | {authors} | {asset['sourcePage']} |")
    lines += [
        '',
        f'## CC-BY assets ({len(ccby)}) - attribution required',
        '',
    ]
    for asset in sorted(ccby, key=lambda a: a['id']):
        lines += [
            f"### {asset['id']} - {asset['license']}",
            '',
            f"- Source: {asset['sourcePage']}",
            f"- Author: {', '.join(asset['authors'])}",
            f"- License: {asset['license']} ({asset['licenseUrl']})",
            f"- Required credit: {asset.get('attributionNote', 'credit the author and license')}",
            '',
        ]
    lines += [
        '## Practical notes',
        '',
        '- Attribution text must ship in the game credits when any CC-BY asset is integrated.',
        '- The OpenGameArt doors/windows pack README notes its Godot glass shader comes from '
        'godotshaders.com; only the CC0 meshes and textures are shipped here - implement the '
        'glass with a Unity URP shader.',
        '- No source asset was taken from unauthorized mirrors, DRM material, or unclear licenses.',
    ]
    OUT_LICENSES.write_text('\n'.join(lines) + '\n')
    print('wrote', OUT_LICENSES)


def write_missing(assets, payload):
    heavy = payload['summary']['heavyMeshes']
    lines = [
        '# Scene 2 missing assets - reported honestly',
        '',
        f'Target: {TARGET}',
        '',
        'The following Scene 2 requirements could NOT be covered by a downloadable asset with '
        'verified commercial rights and compatible quality. Nothing was fabricated or '
        'substituted with pirated/cartoonish stand-ins. Workarounds are recommended per item.',
        '',
    ]
    for title, details in GAPS:
        lines += [f'## {title}', '', details, '']
    lines += [
        '## Heavy meshes requiring decimation before placement',
        '',
        'These shipped assets exceed comfortable raw budgets for the Intel Iris Xe / 8 GB '
        'target. Decimate or use partial subsets (see per-asset notes in asset-manifest.json):',
        '',
    ]
    for asset_id in heavy:
        entry = next(a for a in assets if a['id'] == asset_id)
        detail = entry.get('notes') or entry.get('placement')
        lines.append(f"- **{asset_id}** - {entry.get('trianglesMeasured', 0):,} triangles. {detail}")
    lines += [
        '',
        '## Also required but project-authored by pattern',
        '',
        '- Documents (surat peringatan, catatan survei Damar, kliping koran) - project print props.',
        '- Live-broadcast UI, chat, donation overlays - already production systems.',
        '- Rain, wet-surface and lightning VFX - project effect work, not asset downloads.',
        '- The two-story house upper structure, terrace and pillars - author over the delivered '
        'shell and textures (see missing-assets entry above).',
    ]
    OUT_MISSING.write_text('\n'.join(lines) + '\n')
    print('wrote', OUT_MISSING)


def write_contact_sheets(records):
    previews = []
    for record in records:
        if record.get('type') == 'texture':
            preview = PACKAGE / record['category'] / record['id'] / 'preview.png'
        else:
            preview = PACKAGE / record['category'] / record['id'] / 'preview.png'
        if preview.is_file():
            previews.append((record['category'], record['id'], preview))
    fonts = '/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf'
    try:
        font = ImageFont.truetype(fonts, 11)
    except Exception:
        font = ImageFont.load_default()

    def sheet(items, path, cols=6, size=185):
        rows = (len(items) + cols - 1) // cols
        canvas = Image.new('RGB', (cols * size, rows * (size + 20)), (25, 25, 28))
        draw = ImageDraw.Draw(canvas)
        for index, (_, name, file_path) in enumerate(items):
            image = Image.open(file_path).convert('RGB').resize((size, size))
            row, column = divmod(index, cols)
            canvas.paste(image, (column * size, row * (size + 20)))
            draw.text((column * size + 3, row * (size + 20) + size + 2), name,
                      fill=(230, 230, 230), font=font)
        canvas.save(path)

    out_dir = PACKAGE / 'Previews'
    out_dir.mkdir(exist_ok=True)
    for category in ['Architecture', 'Vegetation', 'Ground_Materials', 'Props', 'Story_Objects']:
        items = [item for item in previews if item[0] == category]
        if items:
            sheet(items, out_dir / f'contact-sheet-{category}.png')
    models = [item for item in previews if item[0] not in ('Ground_Materials',)]
    sheet(models, out_dir / 'contact-sheet-all.png', cols=7, size=160)
    print('wrote contact sheets to', out_dir)


def main():
    records = json.loads(SOURCE.read_text())
    verification = json.loads(VERIFICATION.read_text())
    measured = json.loads(MEASURED.read_text())
    assets = build_assets(records, verification, measured)
    payload = write_json(assets, verification)
    write_licenses(assets)
    write_missing(assets, payload)
    write_contact_sheets(records)


if __name__ == '__main__':
    main()
