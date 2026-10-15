# Scene2_Atmaja_Recovery - three completed Scene 2 assets (2026-10-15)

Recovery drop for the biggest remaining gaps of Prolog Adegan 2 (exterior
Rumah Keluarga Atmaja). Required by `deepseek.md` (asset recovery & kitbashing);
follows `Prolog.MD`, `Rancangan.MD`, `Checklist.MD` (P0-3D) and the project GDD.

**Prefer three genuinely usable models over dozens of unrelated downloads:**
all three priority assets exist here as real, import-tested files.

## Contents

```
Scene2_Atmaja_Recovery/
  01_House/                 two-story Javanese-Dutch house exterior
    Sources/                TowerOverhangKit (CC-BY 4.0) download + notes
    Blender/                build_house.py + RumahAtmaja_House.blend + stats
    Export/                 RumahAtmaja_House.glb / .fbx + Modules/*.fbx (5)
    Textures/               the 10 CC0 Poly Haven sets used (+2 generated variants)
  02_Motorcycle/            generic Indonesian automatic scooter
    Blender/                build_scooter.py + DamarScooter.blend + stats
    Export/                 DamarScooter.glb / .fbx
    Textures/               (empty - procedural PBR factors only)
  03_Frangipani_Tree/       uprooted frangipani obstacle
    Sources/                dead_tree_trunk_02 download + textures + record
    Blender/                build_frangipani.py + FallenFrangipani.blend + stats
    Export/                 FallenFrangipani.glb / .fbx
    Textures/               authored leaf.png, flower.png
  Previews/                 11 renders of the exported files (front/side/3-4/closeups)
  Licenses/                 Poly Haven license capture + credits, OpenGameArt kit page,
                            attribution lines, dependency record
  tools/                    reproducible build/validation/preview scripts
  asset_manifest.json       machine-readable: sources, licenses, files, SHAs,
                            scale, triangles, materials, creation methods
  ASSET_ASSEMBLY_REPORT.md  search log, methods, what is incomplete, risks
  README.md                 this file
  Scene2_Atmaja_Recovery.zip (+ .sha256)
```

## Quick facts

| Asset | Method | Tris | Size (m) | Origin / facing |
| --- | --- | ---: | --- | --- |
| Rumah Atmaja house | KITBASHED + PROCEDURAL | 7,930 | 14.7 x 16.3 x 10.2 | front-centre ground, facade -Y |
| Damar scooter | PROCEDURAL | 21,012 | 0.62 x 1.93 x 1.10 | ground between wheels, front -Y |
| Fallen frangipani | ADAPTED + PROCEDURAL | 127,457 | 5.7 x 4.5 x 2.2 | root base ground, falls +X |

## Unity import notes

- 1 unit = 1 m; GLB is the preferred format (embedded textures + full PBR); FBX
  variants also embed their maps.
- House: five module FBXs isolate Walls / Openings / Veranda / Roof / Annex; the
  full model keeps every part as a separate named object. Openings are real
  holes - move/swap door and window objects for layout. Broken kitchen window is
  on the east side, sill 0.95 m (vault target).
- Scooter: tilt ~8-10 degrees toward the deployed side stand and light the
  headlight against the wall for the scripted parked pose.
- Frangipani: place so the trunk crosses the side path with >= 1.1 m clearance
  under the highest point (crouch). Leaf/flower materials are alpha-clip,
  double-sided cards - keep instances low and consider shadow casting off.

## Rebuild (Blender 4.x, headless)

```sh
blender -b -P Scene2_Atmaja_Recovery/01_House/Blender/build_house.py
blender -b -P Scene2_Atmaja_Recovery/02_Motorcycle/Blender/build_scooter.py
blender -b -P Scene2_Atmaja_Recovery/03_Frangipani_Tree/Blender/build_frangipani.py
blender -b -P Scene2_Atmaja_Recovery/tools/render_recovery_previews.py
python3 Scene2_Atmaja_Recovery/tools/build_recovery_manifest.py
```

## Licensing summary

- House: **CC-BY 4.0 component required credit** - "Wooden Overhang Modular Kit
  by Trey Ramm (minime453), opengameart.org, CC BY 4.0"; Poly Haven textures are
  CC0 (credits recorded voluntarily in `Licenses/polyhaven-credits.md`).
- Scooter: fully project-authored (no third-party content).
- Frangipani: Poly Haven CC0 (`dead_tree_trunk_02`, `root_cluster_01`);
  authored leaf/flower textures are project-owned.
- The approved `Scene2_Atmaja_Assets/` collection was reused but not modified
  (see `Licenses/dependencies.md`).

## Honest limits

See `ASSET_ASSEMBLY_REPORT.md`: no interior/floor plan, no collision or LODs,
procedural scooter lacks fine detail, frangipani is an adaptation (not a
botanical scan), and the house's kitbash seam tolerance depends on tiling
materials. Nothing fabricated - every source URL and hash is recorded.
