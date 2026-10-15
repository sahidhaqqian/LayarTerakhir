# Golden reference scene — review candidate, 15 October 2026

This earlier 6 × 4.5m delivery is superseded by the developer's compact-room revision.
For the current layout/build, see [final Kos refinement](kos-final-refinement.md).

Implemented all five production phases. **Not developer-approved or frozen.** The final Windows visual,
listening and Iris Xe performance acceptance remains with the developer.

## Delivered changes

- Room stays **6 × 4.5 m**. Workstation moved 27 cm inward together with physical/interaction roots,
  props and cables. Added a worn bedside stool/mat, original budget streaming microphone and shared-mesh
  keyboard keys. Existing bed, furniture, curtains, storage, clues and everyday belongings retained.
- Reduced broad key/fill intensity, balanced monitor practical and removed unmotivated window specular glare.
  Existing ACES/FXAA, restrained bloom/vignette and half-resolution SSAO retained. One realtime shadowed
  practical; no successful baked-lighting result is claimed.
- New real imports: Poly Haven Painted Wooden Stool (Kirill Sannikov, CC0) and Dirty Carpet material
  (Rohit Seervi, CC0). Mic, curled mat mesh and keys are original project geometry. Actual sources,
  authors, licensing and derivative changes: [asset credits](asset-credits.md), [download manifest](golden-source-manifest.json).
- **15 prepared mono 32k audio clips**: rain-on-tin, ventilation recording adapted as PC fan, quiet room tone,
  cloth/document rustle, wooden knocks/latch, drawer metal, tile steps, notification bell, UI clicks/switch,
  original synthesized phone vibration and motorcycle departure. Freesound recordings and selected Kenney
  sounds were verified before use. Motorcycle start by roman_cgr is **CC BY 4.0**, not CC0; its source,
  attribution, license link and modification notice accompany the compiled build. Freesound sources are
  official HQ MP3 previews, not login-protected WAV masters.
- Native mixer routes Ambience / Effects / UI; three faded loops, positional PC/door/phone sources,
  modal ducking and departure fades. SFX attach to existing story callbacks, with no new plot triggers.
- Shared **Barlow/OFL HUD**: quiet objective hierarchy, remap-aware prompt, phone status only after pickup,
  battery indicator, readable subtitles, restrained notification, clipped/scrolling document and three
  clear padlock digits. Existing callbacks, IDs, controls, inventory, progression and save logic retained.
  Cached phone/prompt strings avoid rebuilding unchanged status text every frame. No Adegan 2 work.
- Six native reusable prefabs, four import presets, shared HUD/audio styles and existing URP/volume assets.
  Production contract: [GOLDEN_SCENE_STANDARD](../Docs/GOLDEN_SCENE_STANDARD.md).

## Actual verification

| Check | Result / evidence |
| --- | --- |
| Unity tests | **35 passed, 0 failed/skipped**; includes actual enter/exit Play Mode scenarios for both item orders, movement/collision, raycasts/occlusion, UI callbacks, phone/battery/AR/UV gating, checkpoint/save/load and departure. [XML](golden-tests.xml), [invocation](golden-test-invocation.json) |
| Scene integrity | **1,070 base + 16 Golden checks, zero failures**; actual mesh support, collider alignment, all eight interaction raycasts accessible, 163 connected capsule-clear grid cells, materials/scripts/imports/HUD/audio references. [Golden summary](golden-scene-validation.json), [detailed checks](kos-v2-scene-validation.json) |
| Art budgets | Art root **93,635 triangles / 193 renderers / bound textures ≤1024 px**. Renderer count is not draw calls. Shadow map 1024, distance 10 m, four additional lights/object, SRP batcher. No target-GPU benchmark or runtime audio-memory measurement claimed. |
| Standalone smoke | Actual **Linux/Mesa OpenGL player**, 12 checks passed through isolated checkpoint save/load and black departure fade. **13 raw 640×360 framebuffer screenshots including HUD**, posed camera and explicitly accelerated story timers; not manual navigation, normal-timing verification or an FPS test. [Receipt](Visuals/Golden-Linux/player-audit.json), [verified invocation](golden-player-invocation.json) |
| Rendering limitation | A normal-timing 1280×720 attempt saved a genuine [opening capture](Visuals/Golden-Linux-1280-Attempt/01-opening.png) but timed out during story waits. It is retained as [failed evidence](Visuals/Golden-Linux-1280-Attempt/player-audit.json), not relabeled as success. Diagnostic stack inspection found the main thread waiting on Mesa software presentation; Vulkan was rejected by this CPU-only device. No artificial image brightening was used. |
| Audio output | Actual listener mix WAV is captured in the smoke evidence directory; peak approximately **0.192**, RMS approximately **0.033**, no measured clipping. A private clocked PulseAudio virtual sink was required; this is not physical listening or approval of normal-time audio balance/loop seams. [Source processing](golden-audio-processing.json) |

Reviewed raw wide-room, workstation, window, bedside, letter, Damar transcript end, donation/phone and
wrong/right-code captures. The over-dark mat found in the first pass was corrected in the actual material;
the final suite was recaptured. Long Damar text reaches its last line by scrolling. Personal saves were
never read/modified by these checks; automation uses fresh isolated directories, excluded from Git.

## Windows delivery

Real **Unity 6000.3.26f1 Windows x86_64 Mono development build**, **0 errors / 0 warnings**.
Startup: `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity`.

- Complete directory: `Builds/Windows/`, **293 files / 229,421,652 bytes**.
- ZIP: **`Builds/LayarTerakhir-Windows-x86_64-Development.zip`**, **113,871,013 bytes (108.6 MiB)**.
- SHA-256: `b0b1949125512a610969dc983a1f6602cfc4b520a7324102c32a92d5c95dfb5e`.
- Verified AMD64 PE executable/UnityPlayer/Mono runtime, 112 managed assemblies, all 289 Unity-reported
  files plus four credit/license/guidance files, ZIP inventory/CRC and SHA. No Editor assembly in player.
  [Build verification](golden-build-summary.json), [actual invocation](golden-windows-invocation.json).
- Existing older builds/ZIPs preserved under `Builds/Previous/`. Current ZIP includes source credits,
  Barlow's complete OFL, F12 screenshot guide and isolated Windows smoke instructions.

**Windows executable has not been run on this host.** Use [Windows review guide](golden-windows-review.md).
Still required: full-resolution and 16:10 HUD review; manual traversal and both item orders; actual
headphone/speaker listening and loop seams; Windows save reload; target i5-1155G7 / Iris Xe / 8 GB
CPU/GPU frame times, draw calls and memory. **Stable 30 FPS is a target, not a measured achievement.**
Damar's voice note remains the existing readable transcript, not newly recorded voice acting.
