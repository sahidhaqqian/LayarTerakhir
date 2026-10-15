# Golden Scene final refinement — 15 October 2026

Current `Prolog_KamarKos` playtest candidate; supersedes the first compact pass. No rebuild,
Scene 2, new gameplay system, HUD/audio replacement or architecture refactor.

- **Room:** retained **5 × 3.8 m**, ceiling **2.8 m**; original furniture proportions unchanged.
- **Green screen:** genuinely lowered/folded cloth and collapsed frame, **0.54 × 1.62 × 0.24 m**,
  parked beside the north-wall workstation. Retained equipment/prefab, fitted stored collision;
  no longer divides the room or intersects the opening camera-to-bed sightline.
- **Clutter:** larger pale socks at the bed edge; warmer, more visible folded shirt; controller on
  the mattress clear of the phone; light newsprint beside the bed with printed-detail self-shadows
  removed. Spare stool newspaper replaced with one notepad stack and a modest thermos.
- **Surfaces:** less glossy/more muted floor, eight irregular wear/dust patches concentrated at
  the entry, chair and bedside, still one combined mesh. Existing lighting retained, no new lights.
- **Reviewed imports:** Office Notepads (Ulan Cabanilla), Plastic Thermos (PierreB3D), Gamepad
  (Josh Dean), all [Poly Haven CC0-1.0](https://polyhaven.com/license). Actual FBXs opened in Blender;
  original files checked against staging SHA-256 and live source API MD5. Only one pad mesh placed;
  512px runtime albedo/normal/URP masks. Sources/hashes: `kos-final-source-verification.json`;
  authors/changes and decisions for skipped candidates: `asset-credits.md`. Rejected assets not imported.
- **Cost inventory:** 114,455 art-root triangles (previous 101,747), 208 renderers (previous 207),
  whole-scene bound textures ≤1024; new props ≤512. One camera/eight authored light components
  retained, no simulation/reflections/extra render camera. Renderer counts are not measured draw calls.
- **Layout validation:** zero failures; all eight interactions reachable, 95 connected capsule-clear
  25cm grid cells (previous 75). Mesh/material/reference/contact/bed-prop separation checks included.

Verification: **1,151 base scene checks + 31 production checks, zero failures; 35/35 Unity regression
tests passed** (including real enter/exit-PlayMode routines and both pickup orders).
Standalone Linux smoke **14/14 passed**, checkpoint saved/loaded, **13 fresh raw game-camera/HUD
captures** in `Visuals/Golden-Linux`. Reviewed opening/wide/bedside: bed visible, green screen stored,
paper/clothes/socks/controller/flask distinguishable. Receipts: `golden-tests.xml`,
`golden-test-invocation.json`, `golden-scene-validation.json`, `golden-player-invocation.json`.

Limitations: Linux/Mesa software renderer at 640 × 360, posed camera and explicitly accelerated
story timers after opening; not manual-navigation coverage or normal-timing audio acceptance.
Windows manual visual acceptance and Iris Xe FPS are not claimed. Personal saves untouched.

## Windows playtest delivery

Windows x86_64 Mono Development build **Succeeded, 0 errors / 0 warnings**.
Complete ZIP for WinSCP: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`,
**115,591,961 bytes (110.2 MiB)**; 294 files including credits, OFL and playtest/capture guides.
All 289 Unity-reported files and 112 scripting assemblies verified; full ZIP CRC passed.
SHA-256: `814e486fbe8f9230bf9cd52fd1397c56376e05be99f1bef47d3422d90093fc40`.
Receipts: `golden-build-summary.json`, `golden-windows-invocation.json` and full manifest under
`Builds/Logs/windows-build-verification.json`. Prior player/ZIP preserved under `Builds/Previous`.
