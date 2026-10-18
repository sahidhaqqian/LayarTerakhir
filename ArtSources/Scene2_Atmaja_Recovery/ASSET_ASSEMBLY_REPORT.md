# ASSET_ASSEMBLY_REPORT - Scene 2 recovery & kitbashing

Date: 2026-10-15. Target: Prolog Adegan 2 exterior, Rumah Keluarga Atmaja,
23:52 WIB. Engine context: Unity 6.3 LTS / URP, Intel Iris Xe, 8 GB RAM.
Nothing in the production Unity project was touched.

## Deliverables and methods

| # | Asset | Creation method | Triangles | Export files |
| --- | --- | --- | ---: | --- |
| 1 | Rumah Atmaja - two-story Javanese-Dutch house exterior | KITBASHED + PROCEDURAL | 7,930 | `01_House/Export/RumahAtmaja_House.glb` / `.fbx` + 5 module FBXs |
| 2 | Damar's automatic scooter (generic matic) | PROCEDURAL | 21,012 | `02_Motorcycle/Export/DamarScooter.glb` / `.fbx` |
| 3 | Fallen frangipani (uprooted kamboja) | ADAPTED + PROCEDURAL | 127,457 | `03_Frangipani_Tree/Export/FallenFrangipani.glb` / `.fbx` |

All three exist as real files, import cleanly, and are rendered in
`Previews/` from the exported GLBs (see validation below).

## Search log (what was found directly)

**House.** Poly Haven has no buildings. OpenGameArt candidates were reviewed and
rejected on style/quality: `family-house-collection` (US suburban siding),
`old-house-argentina-1872` (single-storey Latin adobe), `haunted-house`
(cartoon), `stucco-old-house-low-poly` (hand-painted), `modular-3d-buildings`
(Kenney cartoon), `modular-building-asset-0` (medieval Nobiax),
`ruined-house` (cartoon ruin). Sketchfab API search for downloadable CC0
"colonial house" / "indonesian house": 0 results; downloaded Sketchfab models
would additionally require authenticated OAuth, which this pipeline cannot do.
Kenney/Quaternius are stylized and were rejected per the brief. **No complete
Javanese-Dutch house exists under a usable license** -> modular kitbashing was
used as the brief directs.

**Motorcycle.** OGA has only a CC0 custom Harley (`custom-harley-davidson-flh-1972`)
- wrong class for a matic. Sketchfab: "motorcycle scooter" CC0 downloadable = 0;
"vespa" returns hornets only. Marketplaces (TurboSquid/CGTrader/Free3D) forbid
redistribution inside a game project. **No usable commuter-scooter model exists
under a verified license** -> procedural build with real-world proportions.

**Frangipani.** OGA: no plumeria/frangipani/kamboja entries; "Gnarled Old Tree"
is CC-BY-SA (share-alike + stylized - rejected). Sketchfab "plumeria" CC0
downloadable = 0. Poly Haven has no Plumeria. **No species-accurate model under
CC0/CC-BY** -> the closest CC0 dead trunk was adapted with Plumeria features.

## What was modified / assembled

**House (kitbash + procedural).** Real downloaded component: the OpenGameArt
*Wooden Overhang Modular Kit* (Trey Ramm / minime453, CC-BY 4.0) supplies the
veranda columns, louvered shutters, top trim and joists; the trim-sheet base
color was darkened to teak. Approved Poly Haven CC0 materials from
`ArtSources/Scene2_Atmaja_Assets/` were reused (10 texture sets). Everything else is
authored: wall segments with real openings, hipped tiled roof with ridge/hip
caps and rafter tails, gable-free veranda with sloped awning, annex with pent
roof, gutters + downpipes with elbows, plinth, mid-band, doors, window frames
with muntins and glass, the partially broken kitchen window (randomly missing
grid panes + sill shards + hanging shutter). The house is delivered as one
assembly with named parts plus five module FBXs (Walls / Openings / Veranda /
Roof / Annex).

**Scooter (procedural).** Authored from scratch with real commuter dimensions
(1.93 m length, ~1.25 m wheelbase, 14" wheels, 0.75 m seat). All parts are
separate named objects (wheels, fork, handlebar, mirrors, apron, floorboard,
seat, rear body, CVT cover, exhaust, fenders, side stand). Materials are PBR
factors only - no external textures, nothing missing on import.

**Frangipani (adapted + procedural).** Base: `dead_tree_trunk_02` (Poly Haven
CC0, newly downloaded for this drop) decimated to 45%, re-materialed with its
own pale weathered bark. Root plate: `root_cluster_01` from the approved
collection, decimated to 25%, original bark material. Added procedurally: five
thick blunt Plumeria canes with fork tips (bark-projected UVs, joint bump),
eight leaf rosettes using an authored `leaf.png` alpha texture, and white
pinwheel flowers from an authored `flower.png`. The trunk was reoriented to a
believable uprooted position and seated on the ground.

## What remains incomplete (honest list)

1. **House interiors** - not modelled (exterior shell only, per the brief; the
   GDD/Prolog were checked - they define exterior requirements only, so no
   floor plan is invented). The second-storey balcony variant was omitted.
2. **House collision + occlusion** - collision hulls, lightmaps and LODs are
   scene work, not delivered here. The broken kitchen window "vault" still
   needs its gameplay collider authored in Unity.
3. **Scooter fidelity** - a clean procedural block-out with believable
   proportions; it lacks cast-in dashboard detail, bolts and brand ergonomics.
   A texture/grime pass is recommended (currently flat PBR factors).
4. **Frangipani species fidelity** - not a botanical Plumeria scan; the bark
   pattern comes from a generic dead tree. Leaves are double-sided alpha cards
   (billboard style), not full leaf geometry; no soil mound around the roots.
5. **No baked normals on authored house geometry** - box-projected UVs on
   procedural meshes are tuned for tiling textures; seams are hidden by
   material choice and night lighting but exist.

## Risks affecting Unity integration / visual quality

1. **CC-BY 4.0 attribution is mandatory** for the TowerOverhangKit inside the
   house model. Ship the credit line from `Licenses/opengameart-wooden-overhang-license.md`.
2. **Alpha cards (tree leaves/flowers)** need URP alpha clip + double-sided and
   cause overdraw - keep the tree to 1-2 instances; consider disabling shadows
   from leaf cards.
3. **House draw calls** - 357 parts / 12 materials: fine for one hero building,
   but merge per material or use static batching before shipping.
4. **FBX texture embedding** - the `.fbx` files embed all maps; the GLB is the
   preferred format. Loose source maps are in each asset's `Textures/`.
5. **Scale conventions** - 1 unit = 1 m, origin conventions per manifest
   (`origin` field). Import at scale 1; do not auto-generate colliders on the
   tree (127k tris) or the scooter's small parts.
6. **Kitchen window vault** geometry assumes a 0.95 m sill and a ~1.1 m wide
   opening - keep the module at the scripted position on the east side path.

## Validation (see `asset_manifest.json` -> `validation`)

* Every expected file exists; SHA-256 recorded for all shipped files.
* All three GLBs were re-imported in Blender and rendered for the previews
  (proves the exports open and carry their materials).
* Duplicate content detected by hash: see manifest (`duplicateFilesByHash`).
* Triangle counts measured on the exported GLBs: 7,930 / 21,012 / 127,457.
* Attribution/license fields present for every third-party component.
