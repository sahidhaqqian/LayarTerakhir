"""Build the curated asset manifest (JSON + Markdown) from the download record
and the verification report. Curation notes explain why each asset was chosen
for the modest Indonesian kos setting and how it should be reviewed before any
Unity integration; nothing is installed into the Unity project by this drop.

Usage:  /usr/bin/python3 ArtSources/AssetResearch/tools/build_manifest.py
"""
from pathlib import Path
import json

REPO = Path(__file__).resolve().parents[3]
RESEARCH = REPO / 'ArtSources' / 'AssetResearch'
SOURCE = RESEARCH / 'manifest' / 'source-manifest.json'
VERIFICATION = RESEARCH / 'manifest' / 'verification-report.json'
MEASURED = RESEARCH / 'manifest' / 'measured-bounds.json'
OUT_JSON = RESEARCH / 'manifest' / 'asset-manifest.json'
OUT_MD = RESEARCH / 'manifest' / 'ASSET-MANIFEST.md'

CURATION = {
    'office_notepads': {
        'category': 'loose papers / notebook',
        'use': 'Loose notepads, blank sheets and sticky notes scattered on the desk or shelf edge; '
               'covers the approved "loose papers" lived-in category.',
        'placement': 'Desk corner or shelf edge, near the streaming workstation.',
        'notes': 'Scatter set: the source file spreads every pad in one row. Separate the pieces '
                 'and place only a few so the room stays believable.',
    },
    'stationery_supplies': {
        'category': 'miscellaneous small items',
        'use': 'Pens, pencils and an eraser in a worn metal cup; small desk clutter for the student routine.',
        'placement': 'Workstation desk, beside the monitor or keyboard.',
        'notes': 'Scatter set: split the cup from the laid-out pens before placing.',
    },
    'clipboard': {
        'category': 'loose papers',
        'use': 'Worn clipboard holding a sheet; document clutter that reads as study/work material.',
        'placement': 'Desk or shelf, flat on a surface so it does not float.',
    },
    'plastic_thermos': {
        'category': 'water bottle or cup',
        'use': 'Olive plastic thermos with cup lid; the standard water container for an Indonesian kos room.',
        'placement': 'Desk or bedside stool.',
    },
    'plastic_crate_02': {
        'category': 'small storage box',
        'use': 'Stackable plastic crate for personal storage (books, cables, supplies).',
        'placement': 'Under the bed or at the foot of the wardrobe, on the floor.',
    },
    'gamepad': {
        'category': 'cables / chargers (electronics)',
        'use': 'Worn wired gamepad with its coiled cable; streamer/gamer clutter and a believable cable source.',
        'placement': 'Desk surface next to the keyboard.',
        'notes': 'The long cable is part of the mesh; trim or route it per the existing desk layout.',
    },
    'wall_clock': {
        'category': 'room dressing',
        'use': 'Wall clock with worn bezel; typical kos room fixture and a quiet horror-friendly prop.',
        'placement': 'Wall, above the desk or near the door.',
    },
    'alarm_clock_01': {
        'category': 'miscellaneous small items',
        'use': 'Twin-bell bedside alarm clock; supports the 03:45 story time without new gameplay.',
        'placement': 'Bedside stool or shelf. The model is rigged but no animation is used.',
        'notes': 'Rigged: import with animation disabled; hours shown are static and not story time.',
    },
    'wooden_broom': {
        'category': 'lived-in utility',
        'use': 'Weathered straw broom (sapu ijuk); the believable cleaning tool in a modest Indonesian room.',
        'placement': 'Leaning in a corner by the door or window, with a simple fitted collider.',
        'notes': 'Leaning prop: fit the collider to the leaning pose, not an upright box.',
    },
    'vintage_flashlight': {
        'category': 'lived-in utility',
        'use': 'Worn flashlight for power cuts; atmospheric prop that fits the horror opening without adding mechanics.',
        'placement': 'Bedside shelf or under the bed edge.',
    },
}

REJECTED = [
    ('jug_01', 'Glossy porcelain with pink tulips; too ornate and luxurious for a modest kos.'),
    ('tea_set_01', 'Victorian tea set; western and luxurious, and 40k triangles.'),
    ('ceramic_vase_01', 'Modern glossy decorative vase; no routine purpose in the room.'),
    ('brass_goblets', 'Ornate brass goblets; western antique styling.'),
    ('throw_pillows_01', 'African zigzag print; visually inconsistent with the room.'),
    ('postcard_set_01', 'Western travel postcards; visually inconsistent.'),
    ('vintage_suitcase', 'Travel stickers read as random decor without a routine purpose.'),
    ('metal_jug', 'Farmhouse milk jug; not a believable Indonesian room prop.'),
    ('vintage_electric_kettle', 'Specialty gooseneck design fights the modest kos styling.'),
    ('electric_stove', 'Western mini oven; heavier (13.9k tris) and no approved placement.'),
    ('boombox', 'Redundant with cassette_player at almost double the triangles.'),
    ('plastic_broom', 'Plastic push broom; the wooden straw broom is the credible local choice.'),
    ('modified_thermos', 'Redundant variant of plastic_thermos.'),
    ('modular_electric_cables', '42k-tri industrial outdoor cables; too heavy and stylistically wrong for an interior.'),
    ('ceramic_pot', 'Rustic western crock; no routine need identified yet.'),
    ('dustpan', 'Could pair with the broom, but not part of the approved clutter list.'),
    ('potted_plant_04', 'Plausible but purely decorative; deferred to avoid random props.'),
    ('classic_laptop', 'Redundant with the existing desktop workstation; deferred.'),
]

# Already downloaded, then rejected during the 2026-10-15 review (deepseek.md /
# goal.md) and removed from the drop together with their textures and previews.
REJECTED_IN_REVIEW = [
    ('industrial_pastic_container',
     'Rejected on review: the industrial storage tub does not fit the modest kos; '
     'plastic_crate_02 already covers storage. Files, textures and previews deleted; not re-downloaded.'),
    ('retro_multimeter',
     'Rejected on review: technician tool reads as unneeded clutter next to the existing '
     'desk electronics. Files, textures and previews deleted; not re-downloaded.'),
    ('cassette_player',
     'Rejected on review: retro music device is not part of the approved clutter list. '
     'Files, textures and previews deleted; not re-downloaded.'),
]

GAPS = [
    ('Used socks, flip-flops and worn clothing',
     'Poly Haven has no believable CC0 clothing items. The project already authors '
     'KosDroppedSocks, KosSandals, KosFoldedTshirt and HangingJacket, so downloads were not forced.'),
    ('Realistic USB charger, power strip and loose cables',
     'Only modular_electric_cables exists and it is industrial (42k tris). The room already '
     'contains fitted charger/lead geometry; the gamepad and multimeter cover cable clutter.'),
    ('Indonesian printed newspapers and documents',
     'No CC0 realistic source found; the project-authored newspaper and paper props remain in use.'),
    ('Indonesia-specific appliances (rice cooker, gas hob, bamboo mat)',
     'Not available under CC0. Research deliberately stopped at sources with verified licenses '
     'instead of guessing at Sketchfab/OpenGameArt licenses.'),
]


def relative(path: Path) -> str:
    return str(path.relative_to(RESEARCH))


def main() -> None:
    source = json.loads(SOURCE.read_text())
    verification = json.loads(VERIFICATION.read_text())
    measured = json.loads(MEASURED.read_text()) if MEASURED.exists() else {}
    verified = {entry['id']: entry['passed'] for entry in verification['assets']}

    curated = []
    for record in source:
        asset_id = record['id']
        curation = CURATION.get(asset_id, {})
        entry = dict(record)
        # API dimensions are millimetres in the source scene arrangement.
        entry['sourceDimensionsM'] = [round(value / 1000.0, 4)
                                      for value in (record.get('dimensionsCm') or [])]
        entry.pop('dimensionsCm', None)
        entry['measuredBoundsM'] = measured.get(asset_id)
        entry.update({
            'category': curation.get('category'),
            'suggestedUse': curation.get('use'),
            'suggestedPlacement': curation.get('placement'),
            'recommendedTextureSize': 512,
            'recommendedImportTarget': 'Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/' + asset_id,
            'verifiedAgainstSource': verified.get(asset_id, False),
            'previewLocal': 'assets/' + asset_id + '/preview.png',
            'previewSourceRender': 'assets/' + asset_id + '/source-preview.png',
            'reviewStatus': 'awaiting independent review; not integrated into Unity',
        })
        curated.append(entry)

    OUT_JSON.parent.mkdir(parents=True, exist_ok=True)
    OUT_JSON.write_text(json.dumps({
        'generated': verification['verifiedAt'],
        'policy': {
            'license': 'CC0-1.0 only',
            'textureLimitSource': '1024 px (1K variants downloaded; 4K avoided)',
            'recommendedRuntimeTextureLimit': '512 px for these small props',
            'unityProjectModified': False,
        },
        'assets': curated,
        'rejectedCandidates': [{'id': i, 'reason': r} for i, r in REJECTED],
        'removedOnReview': [{'id': i, 'reason': r} for i, r in REJECTED_IN_REVIEW],
        'gapsNotCoveredByDownloads': [{'topic': t, 'details': d} for t, d in GAPS],
    }, indent=2) + '\n')

    lines = [
        '# Asset manifest - Kamar Kos clutter drop',
        '',
        'Curated by the Asset Researcher role (`deepseek.md`) for the LayarTerakhir '
        'Kamar Kos environment. All 10 assets are',
        '**Poly Haven CC0-1.0** downloads verified against the live API '
        '([verification report](verification-report.json), '
        'license evidence in `../evidence/polyhaven-license.txt`).',
        'Nothing was installed into the Unity project; integration happens only after '
        'the independent review in `../REVIEW-CHECKLIST.md`.',
        '',
        'Review cleanup 2026-10-15: three previously downloaded assets '
        '(industrial_pastic_container, retro_multimeter, cassette_player) were rejected '
        'and removed from the collection, with textures and previews; see '
        '"Removed on review" below.',
        '',
        '| # | Asset | Category | Tris | Author | Local preview | Source render |',
        '| --- | --- | --- | ---: | --- | --- | --- |',
    ]
    for index, entry in enumerate(curated, 1):
        lines.append(
            f"| {index} | [{entry['name']}]({entry['sourcePage']}) | {entry['category']} "
            f"| {entry['polygons']} | {', '.join(entry['authors'])} "
            f"| <img src=\"../{entry['previewLocal']}\" width=\"160\"> "
            f"| <img src=\"../{entry['previewSourceRender']}\" width=\"160\"> |")
    lines += ['', 'Preview columns: left is a local Cycles render of the exact downloaded FBX '
              '(1K JPG maps, glass simplified); right is the official source render.', '']
    for index, entry in enumerate(curated, 1):
        dims = entry.get('sourceDimensionsM')
        bounds = entry.get('measuredBoundsM')
        dims_text = ' x '.join(f'{value:.3f}' for value in dims) + ' m' if dims else 'n/a'
        bounds_text = ' x '.join(f'{value:.3f}' for value in bounds) + ' m' if bounds else 'n/a'
        lines += [
            f"## {index}. {entry['name']}",
            '',
            f"- **Source:** {entry['sourcePage']}",
            f"- **Author:** {', '.join(entry['authors'])}",
            f"- **License:** {entry['license']} ({entry['licenseUrl']}); attribution not required",
            f"- **Geometry:** {entry['polygons']} triangles; source bounds {dims_text} "
            f"(measured FBX union {bounds_text})",
            f"- **Suggested use:** {entry['suggestedUse']}",
            f"- **Suggested placement:** {entry['suggestedPlacement']}",
        ]
        if entry.get('notes'):
            lines.append(f"- **Notes:** {entry['notes']}")
        lines += [
            f"- **Import guidance:** texture preset 512 px, mipmaps on, anisotropy 2, "
            f"simple fitted collider, no lights/cameras/animation, no automatic mesh colliders",
            f"- **Files:**",
        ]
        for file_entry in entry['files']:
            lines.append(f"  - `assets/{file_entry['file']}` - {file_entry['bytes']:,} bytes, "
                         f"sha256 `{file_entry['sha256'][:16]}...`")
            lines.append(f"    - source: {file_entry['url']}")
        lines.append('')
    lines += ['## Considered but rejected', '',
              '| Candidate | Reason |', '| --- | --- |']
    for asset_id, reason in REJECTED:
        lines.append(f'| {asset_id} | {reason} |')
    lines += ['', '## Removed on review (2026-10-15)', '',
              'Previously downloaded, then rejected by the asset review; files, textures and '
              'previews were deleted from this drop and are not downloaded again.', '',
              '| Removed asset | Reason |', '| --- | --- |']
    for asset_id, reason in REJECTED_IN_REVIEW:
        lines.append(f'| {asset_id} | {reason} |')
    lines += ['', '## Gaps not covered by downloads', '']
    for topic, details in GAPS:
        lines.append(f'- **{topic}** - {details}')
    OUT_MD.write_text('\n'.join(lines) + '\n')
    print('wrote', OUT_JSON)
    print('wrote', OUT_MD)


if __name__ == '__main__':
    main()
