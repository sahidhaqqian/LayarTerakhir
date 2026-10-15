# Golden scene production standard

Reference: `Prolog_KamarKos`, Unity 6000.3.26f1 / URP 17.3, Windows x64 Mono.
This is a review candidate, not developer-approved or frozen. Windows/Iris Xe acceptance is mandatory.

## Environment and materials

Use metres, Y up. The latest developer-approved Kos is **5 × 3.8 m**, ceiling **2.8 m**
(supersedes the earlier 6 × 4.5 m reference). Preserve interaction IDs and move visible models,
their collision and interaction roots together. Do not rerun `PrologKosSceneBuilder` over the furnished scene.
Use `CompactKosPolish.Apply` / `--stage polish` for current visual/layout edits. Do not rerun the
historical greybox/V1/V2 builders: their original coordinates predate the approved smaller room.
The Golden production menu routes an already-compact scene back to this polish pass.
The green screen is now folded/stored beside the workstation, never a divider across the bed view.
Only three reviewed staging assets are integrated (notepad stack, thermos, gamepad); source hashes
and licenses are recorded in `Audit/kos-final-source-verification.json`. Do not import the full research drop.

Folders below `Assets/_LayarTerakhir`: `Art/Imported/<provider>/<source-id>` for legal source models;
`Art/Original/Models` for project-authored FBXs; `Art/Generated/Materials` for shared URP materials;
`Production/Prefabs` and `Production/Presets` for reusable production assets. Record actual URL,
author, license and changes in `Audit/asset-credits.md`. Do not import complete asset packs.

URP/Lit: albedo sRGB, OpenGL (+Y) normal imported as Normal Map, mask linear **R metallic,
G AO, A 1−roughness**, blue unused. Repack ORM/ARM; never plug it directly into URP's metallic slot.
Normal strength should preserve believable scale; cloth/paint stay rough, not glossy plastic.
Use `PropAlbedo512` / `PropNormal512` presets for small props, 1024 maximum for large surfaces.
Mipmaps on, anisotropy 2, desktop compressed formats. No mesh read/write requirement at runtime.
Models: no imported lights/cameras/animation, no automatic mesh colliders, low mesh compression.
Fit simple colliders to actual transformed mesh bounds, and verify real mesh support under tabletop props.

Reusable prefabs: `WornBedsideStool`, `WornBedMat`, `BudgetMicrophone`. The room is modest,
Indonesian and lived-in: prop placement needs a routine or purpose, not arbitrary clutter.
Leave a player-radius-clear, connected route to the door, bed, desk and wardrobe. The validator samples
25 cm capsule-clear cells and actual first-hit interaction raycasts; it does not replace walking the room.

## Practical lighting and integrated-GPU budget

Reuse `Art/Generated/KosPipeline.asset`, `KosRenderer.asset`, `KosAtmosphere.asset` and
`Production/Prefabs/GoldenPracticalLighting`. The lighting prefab is a starting arrangement,
not an overlay to duplicate on top of existing room lights. Reposition all sources to visible fixtures.
One shadowed ceiling spot, other sources unshadowed; additional lights per pixel, maximum 4/object.
1024 shadow atlas, 10 m shadow distance, SRP batcher, 1× MSAA + FXAA, render scale 1.
SSAO half resolution, radius .14 / strength .75; restrained ACES, exposure +.25, bloom .08,
vignette .16. Avoid extra volume effects, live reflection probes, light cookies and decorative lights.
Current probe disabled. Realtime practical light retained for reproducibility on the headless host;
no successful baked-lighting result is claimed. Bake static indirect light later only if it improves
measured target-machine costs without changing the reference look or hiding moving objects.

Room budget: ≤160k visible triangles, bound textures ≤1024, one shadow-casting practical source.
Current validation records actual triangles/renderers and reachable cells; draw calls are not equal to renderer count.
30 FPS on i5-1155G7 / Iris Xe / 8 GB is a **target**, not a measured result on this host.
Profile 1280×720 on Windows, record CPU/GPU frame times and memory, then consider resolution/SSAO reduction.

## Audio

Use `SceneAudioProfile` + `AmbientAudioLoop`, `GoldenScene.mixer` buses **Ambience / Effects / UI**,
and `KosEnvironmentalAudio.prefab`. Bind its player reference and the scene director after placement.
Keep exactly one AudioListener on the gameplay camera.
For Rumah Atmaja, reuse the small loop component/profile/buses, not Kos-specific story callbacks.
Only existing director events invoke knocks, phone vibration, item foley, donation and departure.
Movement-distance footsteps ignore teleport/load and modal-locked movement. No random horror timers.

`AmbientMono32k`: mono 32 kHz, Vorbis compressed in memory, quality .55, preloaded.
`ShortSfxMono32k`: mono 32 kHz, ADPCM decompressed on load. Three ambient loops have crossfaded seams;
source PCM totals about 3.94 MiB, not a measurement of actual mixer/decoder memory.
Linear spatial rolloff, min 1 m, max 5–10 m, Doppler 0. Room tone 2D, roof .25 spatial,
PC fan 3D; gain/fade settings in the shared profile. Modal ambience ducks smoothly to .55.
Keep file levels conservative; final headphone/speaker balance still needs Windows listening.
`Tools/prepare_golden_sources.py` verifies licenses and hashes; `prepare_golden_audio.py` records processing.
Do not describe the Damar transcript as recorded voice acting: it remains the existing readable transcript.

## HUD

Reuse `UI/Prefabs/GoldenHUD.prefab` and `GoldenHudStyle.asset`; retain `HudController` callback/state logic.
The scene must supply one EventSystem with InputSystemUIInputModule (as Kos already does).
Barlow Regular/SemiBold, bundled OFL. Reference 1600×900, match .5, margins 64×44;
body 26, subtitle 28, micro-label 18. Warm neutral text, muted brass accent, no sci-fi panels.
Objective upper left, phone upper right only after pickup, prompt above bottom-centre subtitles;
brief visibility fades, delayed objective dim, clipped scrollable documents and explicit modal buttons.
Normal gameplay hides debug state labels. Audio captions remain for accessibility.
Digit button names/callbacks and document close callback must remain intact. Input hints follow remaps.
Verify 16:9 and 16:10, long Indonesian text and the Damar document's last line before delivery.

## Repeatable validation and delivery

Run with no other Editor instance on this project:

```sh
python3 Tools/run_golden_checks.py --stage apply     # only when authoring the production pass
python3 Tools/run_golden_checks.py --stage polish    # current 5 × 3.8m sleep/work/stream composition
python3 Tools/run_golden_checks.py --stage validate
python3 Tools/run_golden_checks.py --stage test      # EditMode plus enter/exit-PlayMode UnityTests
python3 Tools/run_golden_checks.py --stage linux
python3 Tools/run_golden_checks.py --stage player    # Xvfb standalone smoke/captures, NOT Windows proof
python3 Tools/run_golden_checks.py --stage windows  # preserves the previous player/ZIP
python3 Tools/package_windows_build.py
```

The optional development-player flag `-goldenAudit <absolute-output-directory>` captures the actual
framebuffer/HUD and first 14 seconds of listener output, exercises existing puzzle buttons/story callbacks,
and uses a fresh `IsolatedSave-<GUID>` subdirectory. It never reads/deletes the personal checkpoint.
This scripted posed-camera smoke is not manual movement or an FPS benchmark. Without the flag it is inert.
For an exceptionally slow software renderer, the runner accepts `--width 640 --height 360 --fast`:
this explicitly accelerates story waits after the first capture and records that fact in the receipt.
It is not evidence of normal audio/story timing, full-resolution legibility or physics under manual movement.
The Linux player runner requires Xvfb and PulseAudio; it creates/stops its own clocked virtual sink,
without changing the user's audio configuration. No physical listening device is present.
Use F12 for manual Windows captures (see `Audit/windows-kos-capture.md`).

Review real output, not only pixel thresholds. Direct linear-HDR readback can produce misleadingly dark PNGs;
standalone ScreenCapture includes final tonemapping, game camera and overlay HUD without image adjustment.
Windows acceptance: traverse all routes, solve both item orders, test phone/battery modes and save reload,
inspect HUD clipping and shadows, listen for loop seams/overload, and measure target-hardware frame pacing.
Ship the complete `Builds/Windows` directory ZIP plus bundled source credits/OFL; never just the EXE.
