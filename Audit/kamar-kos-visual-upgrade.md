Kamar Kos visual upgrade — implementation and build result, 2026-10-15 UTC

The existing `Prolog_KamarKos` scene now uses modeled furniture and bedding, a worn desk and chair, wardrobe and shelving, computer peripherals, a modeled hanging jacket, barred window, paneled door, refill-water gallon, mug, notebooks and Indonesian room notices. Ceramic tiles, peeling green paint, limewash, worn wood, fabric and corroded metal use imported PBR maps. The floral bedding and everyday clutter establish a modest boarding room; the offline Malam Keramat screen, red rent notice and August 14 calendar retain the existing story and puzzle.

Eight Poly Haven model sets, six Poly Haven PBR sets and selected Kenney Furniture Kit pieces were downloaded under CC0. All source URLs, authors, license terms, exact download links and checksums are in [asset-credits.md](asset-credits.md) and [asset-download-manifest.json](asset-download-manifest.json). Additional unbranded props and print textures were authored locally. The project contains 28 imported FBX files. Dense props were reduced, and the kit's wood props were re-unwrapped for PBR grain.

The visible room has **57,207 triangles** and **154 renderers**. All 67 imported/generated raster maps were checked at **1024 pixels or smaller**; small-prop maps use 512 pixels. The dedicated URP Forward renderer uses half-resolution SSAO, FXAA, a short shadow distance, one shadowed warm ceiling source, inexpensive fill lights and restrained ACES/bloom/vignette grading. Windows starts in a 1280×720 window. These are conservative settings for the requested hardware, not measured Iris Xe performance results.

All eight existing interaction components, original room/furniture colliders, item IDs and director references remain. Their old cube renderers are disabled and the new visuals follow the same objects, including pickup visibility. New large props have simple solid colliders and are placed outside the established walking path. Movement, phone, puzzle, save and progression scripts, existing tests, package versions, SampleScene and GDD files were not changed. The first-person camera has a small downward framing offset; movement/controller code is unchanged.

Actual validation:

- **32/32 existing automated tests passed**, including actual Play Mode movement/headroom, interaction obstruction, both item-order prolog sequences, phone behavior and save/reload checks. [Final test XML](visual-upgrade-final-tests.xml) / [log](visual-upgrade-final-tests.log).
- The existing milestone validator completed with exit 0. [Validation log](visual-upgrade-milestone-validation.log).
- The first test run had one movement check stop short. The isolated check and subsequent complete runs passed without editing tests or gameplay code. The original failed result and isolated recheck are retained in `visual-upgrade-tests.xml` and `visual-upgrade-movement-recheck.xml`; the cause of that initial failure was not established.
- The latest scene was rendered and visually inspected in **Unity Editor Play Mode** through Xvfb/Mesa llvmpipe. The movement-path capsule cast reached the expected north wall at 1.18 m. [Play Mode capture log](Visuals/gameplay-render-final.log).
- Compare the [original greybox](Visuals/before-room.png) with the [furnished Play Mode view](Visuals/gameplay-room.png). The [startup view](Visuals/gameplay-startup.png) was also inspected.

The real Unity **Windows x86_64 Mono development build succeeded**, exit code **0**, with **0 build errors and 0 build warnings**. `Prolog_KamarKos` remains scene index 0. BuildPipeline ran from 2026-10-15T03:07:25.3459740Z to 2026-10-15T03:08:56.5633840Z (91.2 seconds, excluding Editor startup).

| Deliverable | Actual result |
| --- | --- |
| Scene | `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity` |
| Executable | `/root/Projects/LayarTerakhir/Builds/Windows/LayarTerakhir.exe` |
| Complete build | **211,246,174 bytes / 201.5 MiB**, 289 files |
| Transfer ZIP | `/root/Projects/LayarTerakhir/Builds/LayarTerakhir-Windows-x86_64-Development.zip` |
| ZIP size | **98,815,057 bytes / 94.2 MiB** |
| SHA-256 | `360ac31486247304fa573e57a1cc24d113c79be0937d949e7ba652a1b6eea07e` |

The EXE, UnityPlayer and Mono runtime are PE32+ AMD64 binaries. All 288 Unity-reported files matched their reported sizes; all 112 runtime assembly entries exist. The generated `level0` contains the upgraded environment, bed, tile-floor, calendar and lighting object names. The complete ZIP includes every output file and `Asset-Credits.txt`, passed a full CRC read, and matches the directory's file list and sizes. Download it through WinSCP, extract the complete `Windows/` folder, and launch `LayarTerakhir.exe` with its data and DLLs alongside it. The earlier greybox build is preserved under `Builds/Previous/greybox-before-visual-upgrade/`.

Build evidence: [Unity log](../Builds/Logs/windows-development.log), [native report](../Builds/Logs/windows-development.buildreport), [summary](../Builds/Logs/windows-development-build-report.json), [file/ZIP verification](../Builds/Logs/windows-build-verification.json), [ZIP checksum](../Builds/LayarTerakhir-Windows-x86_64-Development.zip.sha256). Editor build/dressing helpers and asset preparation scripts are retained in `Assets/_LayarTerakhir/Editor/` and `Tools/`.

Limitations: this is a focused furnished-room art pass. **The Windows executable was not run, a complete Windows visual playthrough was not performed, and Iris Xe frame rate/memory usage was not benchmarked.** The inspected images are actual rendered Editor Play Mode captures using software graphics. No DreadOut assets were used, and no next-scene content or gameplay features were added.
