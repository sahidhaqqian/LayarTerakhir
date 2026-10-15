# Scene2_Atmaja_Assets - Prolog Adegan 2 asset drop (2026-10-15)

Verified, downloadable 3D models and PBR materials for **Prolog Adegan 2: Halaman &
Teras Rumah Keluarga Atmaja** - the exterior of Rumah Atmaja (old Javanese-Dutch
residential house, ~23:52 WIB, rain) as scripted in `Prolog.MD`.

Research brief: `deepseek.md`. Story sources: `Prolog.MD`, `Rancangan.MD`,
`Checklist.MD` (P0-3D). Nothing here is installed in Unity; integration is a separate
reviewed step.

## Contents

```
Scene2_Atmaja_Assets/
  Architecture/            iron gate, gutter/pipes, lights, street props,
                           house shell base, roof/wall/wood/rust materials
  Vegetation/              tropical trees, shrubs, ferns, weeds, grass, moss,
                           fallen-tree kit (trunk/roots/branches), stumps, rocks, bark
  Ground_Materials/        mud (wet, footprinted), soil, stony path, mossy/grassy
                           cobblestone, concrete
  Props/                   monobloc chair, bins, garden tools, planter, LPG, oil tin,
                           tyre, bucket, lantern, torch, cement sacks
  Story_Objects/           old wooden door, breakable doors/windows pack (incl.
                           partially broken glass), rusty chain
  Previews/                category contact sheets of every preview
  manifest/                download record, live-source verification report,
                           measured bounds/triangles, raw source API JSON
  evidence/                license pages (Poly Haven + OpenGameArt) as fetched
  tools/                   the reproducible pipeline scripts
  asset-manifest.json      machine-readable; maps every asset to its Scene 2 usage
  asset-licenses.md        license summary + exact attribution obligations
  missing-assets.md        honest gaps (scooter, complete house, frangipani...)
  README.md                this file
  Scene2_Atmaja_Assets-2026-10-15.zip       packaged drop + .sha256
```

Per asset: the model file (FBX/OBJ) or 1K JPG maps, a `preview.png` rendered from
the exact downloaded files, and the official `source-preview.png`.

## License position (verified, not assumed)

- **Poly Haven** (CC0-1.0): commercial use, modification and redistribution allowed,
  attribution not required - evidence in `evidence/polyhaven-license.txt` + raw HTML.
- **OpenGameArt**: per-item license field captured from the live pages.
  - `wooden-door-0` (Lucian Pavel) - CC0.
  - `breakable-doors-and-windows` (loafbrr_1) - CC0.
  - `chain` (Kutejnikov) - **CC-BY 4.0, credit required** + link + changes noted.
  - `small-old-house` (Vinrax) - **CC-BY 3.0, credit required** ("Vinrax,
    vinrax@gmail.com") + link + changes noted.
- All 69 assets passed live re-verification: Poly Haven files against API MD5,
  OpenGameArt unmodified members against re-extracted archive members, everything
  against recorded SHA-256 (`manifest/verification-report.json`, 69/69 PASS).

## Performance notes (Intel Iris Xe / 8 GB target)

- All textures are 1K source (no 4K); recommended runtime import 512 px for props,
  512-1024 px for architecture surfaces.
- Three heavy meshes are flagged for decimation before placement:
  `modular_electricity_poles` (200,610 tris), `root_cluster_01` (225,261),
  `dead_tree_trunk` (101,802). Others are 0.8k-77k tris each.
- Vegetation (`grass_bermuda_01` 941 tris, `moss_01` 204 tris) is card-based and
  cheap to instance.

## Review before integration

1. Re-run the pipeline (network):
   ```sh
   python3 Scene2_Atmaja_Assets/tools/fetch_scene2_assets.py
   python3 Scene2_Atmaja_Assets/tools/verify_scene2_sources.py
   blender -b -P Scene2_Atmaja_Assets/tools/render_scene2_previews.py
   python3 Scene2_Atmaja_Assets/tools/build_scene2_manifest.py
   python3 Scene2_Atmaja_Assets/tools/package_scene2_zip.py
   ```
2. Open `asset-manifest.json` (`sceneUsage` per asset) and `missing-assets.md`.
3. Confirm the CC-BY credit lines in `asset-licenses.md` ship with the game credits.

## Honest gaps (details in missing-assets.md)

- **Damar's automatic scooter** - no acceptable verified-license model; author or
  purchase before integration.
- **Complete two-story Javanese-Dutch house** - assembled from a shell base + gate +
  doors/windows + materials; upper story, terrace and pillars must be authored.
- **Frangipani (kamboja) tree** - species-specific model unavailable; fallen-tree
  kit + tropical tree substitutes delivered.
- Stylized note: the CC0 doors/windows pack uses flat color variants; apply a
  grime/desaturation pass in Unity to match the PBR surroundings.

The previous Kamar Kos clutter drop was cleaned up in the same pass
(three rejected assets removed; see `AssetResearch/README.md`).
