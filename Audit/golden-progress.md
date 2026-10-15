Golden Scene production — 15 October 2026 UTC

| Phase | Actual progress |
| --- | --- |
| 1 Environment | Implemented: workstation moved 27cm into room with its colliders/items/cables; worn bedside stool/mat, original streaming mic and single-mesh keycaps. 6 × 4.5m unchanged; all eight interactions reachable. |
| 2 Lighting | Implemented: practical key/fill/monitor rebalance, window specular fix, existing restrained URP/ACES/FXAA/half-resolution SSAO preserved. Reviewed actual standalone Linux captures, corrected over-dark mat material and verified upward mesh normals. Windows review pending. |
| 3 Audio / HUD | Implemented: 15 prepared clips, three ambient loops, real mixer buses, existing-event SFX, modal duck/departure fades. Shared Barlow HUD/style, clipped scrolling document, restrained labels, remap-aware cached hints. |
| 4 Regression / build | Final 35/35 Unity tests passed, including enter/exit Play Mode tests. Scene validation: 1,070 base + 16 Golden checks, no failures; art root 93,635 triangles / 193 renderers / textures ≤1024. Standalone smoke completed 12 checks and 13 raw 640×360 captures with explicitly accelerated story timers. Windows Mono development build succeeded, 293 files packaged and ZIP CRC/SHA verified (108.6 MiB). Personal saves untouched; no FPS claim. |
| 5 Reuse | Native HUD, audio, lighting and prop prefabs; four import presets; existing pipeline/renderer/volume assets reused. Practical standard authored in Docs/GOLDEN_SCENE_STANDARD.md. Final developer acceptance is still pending. |

Headless capture note: initial unclocked ALSA-null attempt was stopped without completed smoke evidence.
Use the private clocked PulseAudio virtual sink in the runner; no physical audio hardware or Windows GPU is present.
The normal 1280×720 OpenGL software-rendered attempt saved one real opening capture but timed out waiting
for story progression; it is retained as a failed attempt, not counted as a complete smoke. Successful
low-resolution accelerated smoke does not verify normal story/audio timing, full-resolution HUD or Windows performance.
