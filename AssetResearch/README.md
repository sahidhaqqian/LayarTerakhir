# AssetResearch - Kamar Kos clutter drop (2026-10-15)

Asset research deliverable for `deepseek.md`: free, verified, lightweight 3D props for the
**existing** LayarTerakhir environment (`Prolog_KamarKos`, a modest Indonesian kos room).
The approved production brief (`goal.md`, handled separately by the scene-polish agent) lists
the lived-in clutter this drop targets: loose papers, a water bottle/cup, a small storage box,
notebook/misc small items and believable cable/electronics clutter.

**10 assets are delivered as real files** (FBX 1K + PBR maps + previews), each independently
downloadable and hash-verified against the live source. Nothing here is installed in the Unity
project: integration is gated behind the review in [`REVIEW-CHECKLIST.md`](REVIEW-CHECKLIST.md).

**Review cleanup 2026-10-15:** industrial_pastic_container, retro_multimeter and cassette_player
were rejected by the asset review and removed from the collection together with their textures and
previews (files verified absent from production content first). They are not downloaded again and
are listed under "Removed on review" in the manifest.

## Contents

```
AssetResearch/
  README.md                      this file
  REVIEW-CHECKLIST.md            independent review required before Unity integration
  assets/<asset-id>/             one folder per asset
    <id>_1k.fbx                  the actual model file
    textures/                    1K JPG PBR maps (diffuse, OpenGL normal, ARM)
    preview.png                  local Cycles render of the downloaded FBX
    source-preview.png           official source render (Poly Haven thumbnail)
  manifest/
    ASSET-MANIFEST.md            human-readable manifest with embedded previews
    asset-manifest.json          curated machine-readable manifest
    source-manifest.json         raw download record (URLs, API MD5, SHA-256, sizes)
    verification-report.json     live re-verification of every file (all PASS)
    measured-bounds.json         Blender-measured FBX bounds per asset
    api/                         raw Poly Haven API info/files JSON per asset
  evidence/
    polyhaven-license.txt        extracted CC0 license statements (fetched 2026-10-15)
    polyhaven-license-raw.html   untouched license page as fetched
    polyhaven-api-tos.md         public API terms of service
  tools/                         reproducible pipeline scripts
  LayarTerakhir-KosClutter-Assets-2026-10-15.zip     packaged deliverable
```

## License position (verified, not assumed)

All 13 assets come from **Poly Haven** and are **CC0 1.0 Universal**: commercial use allowed,
modification and redistribution allowed, attribution not required. Evidence:
`evidence/polyhaven-license.txt` (extracted statements: *"Our assets are all licensed as CC0"*,
*"You can use our assets for any purpose, including commercial work"*, *"You do not need to give
credit"*) plus the raw fetched page. Every file was re-checked against the API-provided MD5 and
the recorded SHA-256 in `manifest/verification-report.json` (13/13 PASS). Authors are credited
voluntarily in the manifest. The API was used per the public terms saved in `evidence/`.

## Asset summary

| Asset | Category | Tris | Source |
| --- | --- | ---: | --- |
| office_notepads | loose papers | 666 | polyhaven.com/a/office_notepads |
| stationery_supplies | misc small items | 4,412 | polyhaven.com/a/stationery_supplies |
| clipboard | loose papers | 6,176 | polyhaven.com/a/clipboard |
| plastic_thermos | water bottle / cup | 5,514 | polyhaven.com/a/plastic_thermos |
| plastic_crate_02 | small storage box | 5,840 | polyhaven.com/a/plastic_crate_02 |
| gamepad | cables / electronics | 9,546 | polyhaven.com/a/gamepad |
| wall_clock | room dressing | 3,658 | polyhaven.com/a/wall_clock |
| alarm_clock_01 | misc small items | 13,006 | polyhaven.com/a/alarm_clock_01 |
| wooden_broom | lived-in utility | 14,258 | polyhaven.com/a/wooden_broom |
| vintage_flashlight | lived-in utility | 10,562 | polyhaven.com/a/vintage_flashlight |

Total ~74k triangles if everything were placed; see `ASSET-MANIFEST.md` for per-asset placement
notes, rejected candidates (with reasons, including the three removed on review) and categories no
CC0 source could cover honestly (clothing, chargers, Indonesian print).

## Reproduce

```sh
python3 AssetResearch/tools/fetch_assets.py         # download + MD5-verify (network)
python3 AssetResearch/tools/verify_sources.py       # live re-verification + evidence
blender -b -P AssetResearch/tools/render_previews.py   # local previews (Cycles CPU)
blender -b -P AssetResearch/tools/measure_bounds.py    # FBX bounds report
python3 AssetResearch/tools/build_manifest.py       # curated manifest (JSON + MD)
python3 AssetResearch/tools/package_zip.py          # ZIP + SHA-256
```

## Requirements checklist (deepseek.md)

1. Free assets with verified commercial rights - Poly Haven CC0, evidence stored. Yes.
2. Prefer CC0 - all 13 are CC0 1.0. Yes.
3. Actual model files and required textures downloaded - FBX + diff/nor_gl/arm JPGs. Yes.
4. FBX/GLB/GLTF/OBJ prioritized - FBX 1K for each. Yes.
5. Reject low-quality/cartoonish/oversized/inconsistent - 18 candidates rejected with reasons
   before download, plus 3 downloaded assets removed on review. Yes.
6. No unnecessary polycounts or 4K - 666-14k tris each, 1K source maps, 512px recommended at runtime. Yes.
7. Source URLs, licenses, attribution requirements - per asset in both manifests. Yes.
8. Preview image for every asset - local render **and** official render for all 10. Yes.
9. Organized by asset name, packaged in a ZIP - see the ZIP in this folder. Yes.
10. Unity project untouched - everything lives under `AssetResearch/`. Yes.

Every asset still needs the manual review in [`REVIEW-CHECKLIST.md`](REVIEW-CHECKLIST.md)
before any Unity import.
