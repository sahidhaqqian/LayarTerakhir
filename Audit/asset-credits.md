Asset provenance for Kamar Kos and the Atmaja continuation, with the 2026-10-16 playtest revision.

## Kos immersion follow-up — 16 October 2026

`KosCorridorBuilding.cs` is original Unity-native architectural/dressing geometry (room shells,
stair guards, facade/roof/porch, main door, conduit, vents, slipper pairs). Materials, gate, scooter,
planters and ferns reuse already-credited assets below; no additional environment pack was downloaded.
`Tools/improve_kos_audio.py` authors five original mathematical/noise PCM effects: forceful wooden
door knock, incoming ringtone, outgoing local call tone, metal scrape and glass/cutoff. No external
recordings, melodies or human voice identity are sampled for these effects.

Six targeted neural retakes use the same original fictional Raka/Damar references and the official
OpenBMB VoxCPM2 public demo, reference-only controllable cloning. The previous transcript-continuation
mode explicitly omitted style control in the service implementation; the corrected mode now applies
the performance direction. The [official source](https://huggingface.co/spaces/openbmb/VoxCPM-Demo/raw/main/app.py)
and [Apache-2.0 model card](https://huggingface.co/openbmb/VoxCPM2) were checked again. Added distress
cry and contract reaction are synthetic; acting quality remains subject to human approval. The current
26-entry manifest covering both floors, the basement tape and the ending is authoritative; no synthesis
service or model runs inside the shipped game.

## Playtest revision — 16 October 2026

No new environment asset pack was downloaded. `Tools/repair_playtest_art.py` authors worn wooden chairs,
a longer dining table and a small corner table, repairs the existing grandfather clock, and removes the
oversized recovery root plate from the tree. Wooden chairs replace the earlier plastic-chair instances;
the original unused import and its credit below are retained. Leaves/flowers are reduced, card transforms
are baked before joining, and the existing licensed source meshes/materials retain their source licenses.
Kos corridor art reuses the existing floor/plaster/wood, gate, scooter and bucket, with native original geometry.
The red AR blood-writing mesh is original vector-stroke artwork, not an imported font or extracted game texture.

Clock dial numerals are mesh outlines generated from **DejaVu Serif**, copyright (c) 2003 Bitstream, Inc.;
DejaVu changes are public domain. The original Bitstream Vera permission notice accompanies the project
and Windows build in `Audit/dejavu-font-license.txt` / `DejaVu-Font-License.txt`. No font is renamed as Bitstream Vera.

Current dialogue is **synthetic Indonesian neural speech**, allowed by the developer, from
[OpenBMB VoxCPM2](https://huggingface.co/openbmb/VoxCPM2), whose released model is Apache-2.0.
Original model-designed samples distinguish Raka, Damar, the well whisper, Damar's broken-phone
recording (same Damar reference, telephone-band processing) and Nyonya Sri Atmaja's enraged scream
(Bisikan reference); no real actor/person was cloned. The landlady has no spoken line. Reference samples were authored locally, then the
dialogue clips were rendered through the [official public demo](https://huggingface.co/spaces/openbmb/VoxCPM-Demo)
and normalized with FFmpeg. Only fictional script/synthetic reference audio was sent, never saves
or personal recordings. The game uses ordinary rendered WAV/Vorbis audio and remains offline;
no model weights or Python/cloud inference dependencies are included. The exact two-word whisper
uses its original local voice-design sample for clearer, slower delivery. Acting quality still needs listening approval.
`Tools/author_voice_from_official_demo.py`, `Tools/neural_prolog_voice.py` and
`Audit/prolog-voice-manifest.json` record exact lines, references, processing, durations and SHA-256.
The former eSpeak source clips are historical/unreferenced, not the current bank. No NC Piper or OmniVoice
pretrained voice is included; the latter was rejected after reading its full model license.
Clock tick/tock is an original short mechanical waveform generated at runtime.

Phone map: [Jakarta Indonesia street map](https://commons.wikimedia.org/wiki/File:Jakarta_Indonesia_street_map.svg)
by Kirill Shrayber, CC0 1.0, downloaded as Wikimedia's PNG rendering. Image pixels are retained;
phone UV cropping, fictional Kos/Atmaja pins and illustrative route are original UI overlays.
Attribution is shown in the phone. These are not real household addresses or real navigation directions.
Phone bezel, rounded UI panels, app glyphs, desk key and four-wheel lock are original code-native artwork.

## Atmaja continuation — source staging reused, no new asset downloads

**Required credit: Wooden Overhang Modular Kit by Trey Ramm (minime453), [OpenGameArt source](https://opengameart.org/content/wooden-overhang-modular-kit), [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/).** Changes: kit parts rescaled/repositioned, base-color darkened, combined into the recovery house; Unity derivative joins parts by material and cuts the kitchen passage. This third-party component remains CC BY 4.0, not CC0 or relicensed as project code. License/source captures are in `Scene2_Atmaja_Recovery/Licenses/`.

Other selected staged artwork is CC0-1.0, under the [Poly Haven asset license](https://polyhaven.com/license). Voluntary credits (source IDs link to the original item):

| Source asset | Creator | Use / changes |
| --- | --- | --- |
| [large_iron_gate](https://polyhaven.com/a/large_iron_gate) | Josh Dean | Decimated, separate hinge + physical collider |
| [security_light](https://polyhaven.com/a/security_light) | Maximilian Schuster | Porch fixture, reduced mesh |
| [shrub_03](https://polyhaven.com/a/shrub_03), [grass_bermuda_01](https://polyhaven.com/a/grass_bermuda_01) | Rico Cilliers | Courtyard vegetation, shrub reduced |
| [fern_02](https://polyhaven.com/a/fern_02) | Rico Cilliers, Rob Tuytel | Reduced east-path foliage |
| [plastic_monobloc_chair_01](https://polyhaven.com/a/plastic_monobloc_chair_01) | Kuutti Siitonen | Porch/dining chairs |
| [planter_pot_clay](https://polyhaven.com/a/planter_pot_clay) | Amal Kumar | Porch pots |
| [wooden_bucket_01](https://polyhaven.com/a/wooden_bucket_01) | James Ray Cock | Reduced kitchen bucket |
| [brown_mud_03](https://polyhaven.com/a/brown_mud_03), [dirty_concrete](https://polyhaven.com/a/dirty_concrete), [concrete_floor_damaged_01](https://polyhaven.com/a/concrete_floor_damaged_01) | Rob Tuytel | Ground/floor PBR, Unity packed masks |
| [stony_dirt_path](https://polyhaven.com/a/stony_dirt_path) | eye-candy.xyz | Front path |
| [mossy_cobblestone](https://polyhaven.com/a/mossy_cobblestone) | Sơn Nguyễn | Wall caps |
| [root_cluster_01](https://polyhaven.com/a/root_cluster_01) | Jenelle van Heerden, Rico Cilliers | Recovery tree/root PBR |
| [dead_tree_trunk_02](https://polyhaven.com/a/dead_tree_trunk_02) | Rob Tuytel | Recovery frangipani source, heavily decimated; arch adjusted for crouch clearance |
| [clay_roof_tiles_02](https://polyhaven.com/a/clay_roof_tiles_02), [clay_roof_tiles_03](https://polyhaven.com/a/clay_roof_tiles_03), [worn_mossy_plasterwall](https://polyhaven.com/a/worn_mossy_plasterwall), [damaged_plaster](https://polyhaven.com/a/damaged_plaster), [rusty_metal_sheet](https://polyhaven.com/a/rusty_metal_sheet) | Amal Kumar | Recovery house PBR, reduced to 1K |
| [grey_plaster_03](https://polyhaven.com/a/grey_plaster_03), [concrete_moss](https://polyhaven.com/a/concrete_moss), [wood_planks_grey](https://polyhaven.com/a/wood_planks_grey) | Rob Tuytel | Recovery house PBR |
| [brick_moss_001](https://polyhaven.com/a/brick_moss_001) | Dimitrios Savva, Rico Cilliers | Recovery house base |
| [wood_peeling_paint_weathered](https://polyhaven.com/a/wood_peeling_paint_weathered) | Rob Tuytel, Dimitrios Savva | Recovery house joinery |

The procedural recovery scooter/house details/frangipani leaves and flowers are project-authored; derivative repairs correct scooter wheel axes/seat and physical window openings. Original added meshes: banquet table, grandfather clock/hands/drawer, seated bent-neck AR figure, linked chains, well boards, cracked phone details and kitchen fittings. The backpack reuses the original Kos FBX without changing that source asset. These original works follow the project license. No branded motorcycle model, ripped game art, or new download is used.

`Audit/atmaja-art-manifest.json` records hashes of consumed staged sources, derivative model counts and material paths; `Tools/prepare_atmaja_art.py`, `Tools/pack_atmaja_masks.py`, `Tools/author_atmaja_details.py` preserve the workflow. Selected model/material imports are at `Assets/_LayarTerakhir/Art/Atmaja`. Existing Kos audio clips, Barlow OFL, and CC0 art remain credited below. The later synthetic dialogue revision is described above.

## Kamar Kos (earlier passes, preserved)

All downloaded artwork used in this pass is **CC0 1.0 Universal**. Poly Haven explicitly permits commercial use, modification, and redistribution, without mandatory attribution ([asset license](https://polyhaven.com/license)); Kenney labels the Furniture Kit CC0 and includes the same commercial-use permission in its bundled License.txt ([asset page](https://kenney.nl/assets/furniture-kit)). Voluntary credits follow.

| Imported asset | Creator credited by source | Original source | Use / license |
| --- | --- | --- | --- |
| Vintage Day Bed | Aron Łyczek | [vintage_day_bed](https://polyhaven.com/a/vintage_day_bed) | Bed frame, mattress, pillow and draped quilt; CC0-1.0 |
| Wooden Table 02 | Serhii Khromov | [wooden_table_02](https://polyhaven.com/a/wooden_table_02) | Work desk; CC0-1.0 |
| Painted Wooden Chair 01 | Kuutti Siitonen | [painted_wooden_chair_01](https://polyhaven.com/a/painted_wooden_chair_01) | Worn wooden desk chair; CC0-1.0 |
| Painted Wooden Cabinet 02 | Kirill Sannikov | [painted_wooden_cabinet_02](https://polyhaven.com/a/painted_wooden_cabinet_02) | Blue cupboard and shelves; CC0-1.0 |
| Wooden Bookshelf Worn | Ulan Cabanilla | [wooden_bookshelf_worn](https://polyhaven.com/a/wooden_bookshelf_worn) | Wall storage; CC0-1.0 |
| Plastic Bottle Gallon | Rahul Chaudhary | [plastic_bottle_gallon](https://polyhaven.com/a/plastic_bottle_gallon) | Cleaning jug; CC0-1.0 |
| Desk Lamp Arm 01 | Yann Kervran, Kuutti Siitonen | [desk_lamp_arm_01](https://polyhaven.com/a/desk_lamp_arm_01) | Desk lamp; CC0-1.0 |
| Binder Notebook | DaDrood | [binder_notebook](https://polyhaven.com/a/binder_notebook) | Notebook / desk clutter; CC0-1.0 |
| Worn Plaster Wall | Dimitrios Savva | [worn_plaster_wall](https://polyhaven.com/a/worn_plaster_wall) | Tiling PBR material; CC0-1.0 |
| Painted Plaster Wall | Amal Kumar | [painted_plaster_wall](https://polyhaven.com/a/painted_plaster_wall) | Tiling PBR material; CC0-1.0 |
| Floor Tiles 04 | Rob Tuytel | [floor_tiles_04](https://polyhaven.com/a/floor_tiles_04) | Tiling PBR material; CC0-1.0 |
| Wood Table Worn | Dimitrios Savva, Rico Cilliers | [wood_table_worn](https://polyhaven.com/a/wood_table_worn) | Tiling PBR material; CC0-1.0 |
| Denim Fabric | Rob Tuytel | [denim_fabric](https://polyhaven.com/a/denim_fabric) | Tiling PBR material; CC0-1.0 |
| Rusty Metal | Rob Tuytel | [rusty_metal](https://polyhaven.com/a/rusty_metal) | Tiling PBR material; CC0-1.0 |

Kenney Furniture Kit 2.0 — Kenney / Kenney.nl. Imported parts: computerScreen, computerKeyboard, computerMouse, speakerSmall, bookcaseClosedDoors, cabinetBedDrawer, doorwayFront, wallWindow, books, cardboardBoxOpen, trashcan, ceilingFan and coatRack. The last two are available in the project but not placed in the scene. All are CC0-1.0. The simple props were assigned new URP PBR materials in this project. The wardrobe, drawer, door and window meshes were converted from the same kit's OBJ variants and re-unwrapped for wood grain instead of their original palette UVs; see `Tools/retexture_kenney_wood.py`.

Verified original ZIP download: [kenney_furniture-kit.zip](https://kenney.nl/media/pages/assets/furniture-kit/440e0608a4-1677580847/kenney_furniture-kit.zip). Downloaded 5,130,729 bytes; SHA-256 `e67652d0932cee41683f74711c03d3e192a2af9979ef8e6b237711f5482d46b0`. The source archive was opened and its included License.txt inspected. A copy is retained at `Assets/_LayarTerakhir/Art/Imported/Kenney/License.txt`.

The exact Poly Haven model and texture download URLs, source MD5 values, SHA-256 digests, file sizes, authors, and imported paths are recorded in [asset-download-manifest.json](asset-download-manifest.json). All downloads were checked against the API-supplied MD5. The live API was used only by the production downloader with the User-Agent `LayarTerakhir-EnvironmentProduction/1.0`; the game makes no Poly Haven API requests. [Current API terms](https://github.com/Poly-Haven/Public-API/blob/master/ToS.md) permit commercial API access and distinguish it from the unconditional CC0 asset license.

All downloaded texture variants are 1K or smaller. Masks were repacked from glTF ARM (R ambient occlusion, G roughness, B metalness) to Unity R metalness / G ambient occlusion / A smoothness. Normal maps use the OpenGL convention and Unity NormalMap import. Small-prop maps were reduced to 512 pixels. Some source meshes were decimated; the before/after counts are in [model-processing.json](model-processing.json). No source assets were obtained from games, unauthorized mirrors, or models with unknown license terms.

Original project-specific art was authored for this pass: hanging jacket, enamel mug, refill-water gallon, computer tower, smartphone shell, combination padlock, electrical fittings, wall surfaces and trims; August calendar, birthday circle, rent warning, creator dashboard, generic noodle packaging, kos rules, poster, water label, woven mat, grime and contact masks. `Tools/author_kos_details.py` and `Tools/prepare_kos_art.py` retain the authoring steps. These original works follow the project license; the downloaded CC0 works retain their CC0 status. The printed labels use the system DejaVu fonts for rasterization; no font software is bundled by these tools ([DejaVu license](https://dejavu-fonts.github.io/License.html)).

The calendar and sticky note preserve the existing puzzle clue: 14 August / 14-08, with the existing 148 lock logic unchanged. The monitor texture preserves the established offline channel and 32-viewer story. DreadOut 2 was used only as an atmospheric direction; no DreadOut assets, screenshots, logos, sounds, or textures were imported.

V2 additions — verified against the live source/license on 2026-10-15 UTC:

| Asset | Author | Source / license | Scene use |
| --- | --- | --- | --- |
| Plastic Container | PierreB3D | [Poly Haven source](https://polyhaven.com/a/plastic_container), [CC0-1.0](https://polyhaven.com/license) | Compact personal storage at the foot of the bed; 512px maps |
| Wicker Basket 02 | Kuutti Siitonen | [Poly Haven source](https://polyhaven.com/a/wicker_basket_02), [CC0-1.0](https://polyhaven.com/license) | Lidded laundry basket beside the wardrobe; 512px maps |
| Fabric Pattern 07 (col_1) | Rob Tuytel | [Poly Haven source](https://polyhaven.com/a/fabric_pattern_07), [CC0-1.0](https://polyhaven.com/license) | Sewn window curtains; 1024px maps |

These CC0 assets allow commercial use, modification and redistribution; attribution is voluntary. [kos-v2-asset-manifest.json](kos-v2-asset-manifest.json) records the exact download URLs, API-provided MD5 checks, original SHA-256 digests, imported SHA-256 digests and source sizes. The V2 production downloader uses the User-Agent `LayarTerakhir-EnvironmentProduction/2.0`; neither the game nor its screenshot tool calls the asset API. Only selected 1K source variants were downloaded (about 6.3 MB total), then small-prop maps were reduced to 512px. No preview images or oversized packs were imported.

V2 original modeled artwork: an unbranded wire-guard desk fan, canvas backpack with zippers/straps, a pair of rubber flip-flops, folded/sewn curtain panels, and a small router. The original mesh authoring source is `Tools/author_kos_v2.py`; these works follow the project license. Door hardware, shelf brackets, wall sockets, charger, power strip, and connected leads are lightweight scene geometry. The original V1 furniture, printed clues, props and attribution above are retained.

The existing Kenney `wallWindow.fbx` was repaired from the same verified source kit: its prefab wall panel was removed so it no longer covers the scene's own opening, and the original glass, wood and metal surfaces were separated for appropriate materials. `Tools/repair_kos_window.py` records that derivative workflow. The window derivative remains CC0-1.0, attributed to Kenney above.

## Golden reference additions — 2026-10-15

The earlier CC0 statement concerns artwork, **not all Golden audio/fonts**. These additions have their own licenses:

| Imported source | Author | Source / license | Actual derivative/use |
| --- | --- | --- | --- |
| Painted Wooden Stool | Kirill Sannikov | [Poly Haven](https://polyhaven.com/a/painted_wooden_stool), [CC0](https://polyhaven.com/license) | Worn bedside stool, original 676-triangle model, 512px maps |
| Dirty Carpet | Rohit Seervi | [Poly Haven](https://polyhaven.com/a/dirty_carpet), [CC0](https://polyhaven.com/license) | Muted bedside mat material, 512px runtime import |
| Rain on Tin | Srirachasocks | [Freesound 832559](https://freesound.org/people/Srirachasocks/sounds/832559/), [CC0-1.0](https://creativecommons.org/publicdomain/zero/1.0/) | RainRoof: mono, filtered, normalized, resampled, crossfaded loop |
| Extractor Fan 03 | JarredGibb | [Freesound 219489](https://freesound.org/people/JarredGibb/sounds/219489/), [CC0-1.0](https://creativecommons.org/publicdomain/zero/1.0/) | PCFan: low-passed ventilation recording adapted as old computer fan; not a recording of this PC |
| ApartmentLivingRoom2 | leonelmail | [Freesound 329533](https://freesound.org/people/leonelmail/sounds/329533/), [CC0-1.0](https://creativecommons.org/publicdomain/zero/1.0/) | Quiet mono room-tone loop |
| Clothing Rustling 2 | Sheyvan | [Freesound 476804](https://freesound.org/people/Sheyvan/sounds/476804/), [CC0-1.0](https://creativecommons.org/publicdomain/zero/1.0/) | Trimmed cloth pickup; also adapted as quiet document-handling rustle |
| Motorcycle start | roman_cgr | [Freesound 415038](https://freesound.org/people/roman_cgr/sounds/415038/), [CC BY 4.0](https://creativecommons.org/licenses/by/4.0/) | DepartureEngine: excerpted, mono downmix, normalized, resampled 32 kHz. Changes made by LayarTerakhir; no author endorsement implied. |
| Impact Sounds | Kenney | [Official asset page](https://kenney.nl/assets/impact-sounds), CC0-1.0 | Selected wood/metal/bell and concrete-footstep recordings, trimmed/normalized; source filenames in processing tool |
| UI Audio | Kenney | [Official asset page](https://kenney.nl/assets/ui-audio), CC0-1.0 | click1 and switch1, lowered level, mono 32k |
| Barlow Regular / SemiBold | The Barlow Project Authors | [Official Google Fonts source](https://github.com/google/fonts/tree/main/ofl/barlow), [SIL OFL 1.1](https://openfontlicense.org/) | Unmodified bundled font software; full copyright/license retained in `UI/Fonts/OFL.txt` and Windows distribution |

The five Freesound downloads are the official, freely served high-quality MP3 previews, **not** the login-protected WAV masters. Source-page licenses were checked before downloading. Their exact CDN URLs, source links, authors, licenses, hashes and sizes are in `golden-source-manifest.json`. The CC BY 4.0 motorcycle recording permits commercial adaptation and redistribution subject to attribution, license link and indication of changes; these notices accompany the compiled game. No DRM or additional restriction is imposed on that recording. Kenney license files are retained under `Audio/Imported/`; only selected clips were extracted, not the full packs.

`Tools/prepare_golden_audio.py` records mono conversion, sample rates, gain/filtering, crossfaded loop seams, peaks, RMS and SHA-256 in `golden-audio-processing.json`. The phone-vibration clip is **original synthesized mechanical vibration**, not a field recording. The microphone and curled bedside-mat mesh are original unbranded project artwork authored with `Tools/author_golden_details.py`; the 56 keyboard keycaps share one generated mesh. These original works follow the project license; third-party works retain the licenses above.

## Compact city-streamer polish — original project art

`Tools/author_compact_kos.py` authors the new portable green screen (sewn/creased cloth, spring clips,
telescoping poles and low T feet), monitor-clip budget webcam, dropped cotton socks, folded T-shirt,
and creased old newspaper with coarse printed columns. These are original, unbranded static meshes,
not downloads, extracted game assets or new story documents. They follow the project license.
Existing original DampStain alpha is reused for small tracked-floor-dirt patches (one combined mesh),
and the existing CC0 fabric normal map is reused on the matte green cloth. No new textures, external
asset packs, fonts or audio were downloaded for this visual-only pass; prior third-party licenses above remain unchanged.

## Final Golden refinement — reviewed staging imports (15 October 2026)

Independent integration review by Codex: exact staged FBXs opened in Blender; eight approved candidate
previews inspected. The following three assets were selected, with source pages and the current
[Poly Haven CC0 license](https://polyhaven.com/license) checked independently. Original FBX/JPG bytes
match both the staging SHA-256 and current source API MD5. No unrelated replacement search/download.

| Integrated asset | Author | Source / license | Use and modifications |
| --- | --- | --- | --- |
| Office Notepads | Ulan Cabanilla | [Poly Haven](https://polyhaven.com/a/office_notepads), CC0-1.0 | One A4 stack mesh on the bedside stool, replaces the spare newspaper; no full collection scatter |
| Plastic Thermos | PierreB3D | [Poly Haven](https://polyhaven.com/a/plastic_thermos), CC0-1.0 | Modest drink flask on the bedside stool/notepad; natural 32cm height |
| Gamepad | Josh Dean | [Poly Haven](https://polyhaven.com/a/gamepad), CC0-1.0 | Wired budget controller left on the mattress, reinforcing Raka's streamer routine |

Attribution is voluntary for these CC0 assets. URP/Lit materials use 512px runtime imports, OpenGL
normal maps and derived masks (R metallic / G AO / A inverse roughness); no maps misbound as ARM.
Full source hashes and URLs: `Audit/kos-final-source-verification.json`; repeatable import:
`Tools/prepare_kos_refinement.py`. No official website previews are included in the playable build.
Clipboard/stationery/crate would duplicate current desk/storage clutter; wall clock and broom were
not needed for this composition. Rejected industrial container, multimeter and cassette player
were not imported. No additional fonts/audio or gameplay dependencies.

## Prolog Adegan 4–7 upper floor, basement, chase and ending — 16 October 2026

New staging for the second floor, basement and chase. All 25 model sources and 7 PBR texture sets are
Poly Haven **CC0-1.0** ([asset license](https://polyhaven.com/license)); attribution is voluntary.
`Tools/download_atmaja_upper_sources.py` fetched exact 1K FBX/JPG variants via the public API with
User-Agent `LayarTerakhir-UpperFloorStaging/1.0` and verified every file against the API-provided MD5.
Exact URLs, authors, MD5, SHA-256 and sizes: [atmaja-upper-source-verification.json](atmaja-upper-source-verification.json).

| Source asset | Creator | Use / changes |
| --- | --- | --- |
| [hanging_picture_frame_01](https://polyhaven.com/a/hanging_picture_frame_01), [hanging_picture_frame_03](https://polyhaven.com/a/hanging_picture_frame_03) | James Ray Cock | Corridor portrait frames + AR gouge overlays; capped, glass kept |
| [fancy_picture_frame_01](https://polyhaven.com/a/fancy_picture_frame_01), [fancy_picture_frame_02](https://polyhaven.com/a/fancy_picture_frame_02) | Rob Tuytel, Rico Cilliers | Master bedroom portrait frames; capped |
| [ornate_mirror_01](https://polyhaven.com/a/ornate_mirror_01) | James Ray Cock | Bathroom mirror (standing apparition flash); capped |
| [painted_wooden_shelves](https://polyhaven.com/a/painted_wooden_shelves) | Kirill Sannikov | Child bedroom wall shelves |
| [painted_wooden_nightstand](https://polyhaven.com/a/painted_wooden_nightstand) | Kirill Sannikov | Master nightstand with alarm clock |
| [old_bed_frame](https://polyhaven.com/a/old_bed_frame) | Luca B | Child and master bed frames; capped |
| [vintage_cabinet_01](https://polyhaven.com/a/vintage_cabinet_01) | Rico Cilliers | Corridor vent cabinet + basement wardrobe |
| [Chandelier_01](https://polyhaven.com/a/Chandelier_01) | Kirill Sannikov | Balcony void chandelier over dining; capped |
| [Lantern_01](https://polyhaven.com/a/Lantern_01) | Rajil Jose Macatangay | Corridor and stair lanterns; capped |
| [pot_enamel_01](https://polyhaven.com/a/pot_enamel_01) | Kuutti Siitonen | Bathroom enamel bucket |
| [alarm_clock_01](https://polyhaven.com/a/alarm_clock_01) | Yann Kervran, James Ray Cock | Master nightstand clock; hands disabled |
| [vintage_wooden_drawer_01](https://polyhaven.com/a/vintage_wooden_drawer_01) | James Ray Cock | Master dresser; capped |
| [wooden_stool_01](https://polyhaven.com/a/wooden_stool_01) | Kuutti Siitonen | Basement sewing stool |
| [Rockingchair_01](https://polyhaven.com/a/Rockingchair_01) | Jorge Camacho | Balcony and master rocking chairs; capped |
| [throw_pillows_01](https://polyhaven.com/a/throw_pillows_01) | Serhii Khromov | Bed pillows; capped |
| [wooden_crate_01](https://polyhaven.com/a/wooden_crate_01), [wooden_crate_02](https://polyhaven.com/a/wooden_crate_02) | James Ray Cock, Jurita Burger | Basement storage; capped |
| [steel_frame_shelves_02](https://polyhaven.com/a/steel_frame_shelves_02) | James Ray Cock | Basement racks; capped |
| [book_encyclopedia_set_01](https://polyhaven.com/a/book_encyclopedia_set_01) | John Malcolm | Shelf/rack books, 20 volumes; capped to 120 tris per volume |
| [vintage_video_camera](https://polyhaven.com/a/vintage_video_camera) | James Ray Cock, Jenelle van Heerden | Basement handycam prop (played clip, not a real recording); capped |
| [wooden_candlestick](https://polyhaven.com/a/wooden_candlestick) | Josh Dean | Dresser candle; capped |
| [metal_tool_chest](https://polyhaven.com/a/metal_tool_chest) | Yann Kervran, John Hutcheson | Master tool chest; capped |
| [vintage_oil_lamp](https://polyhaven.com/a/vintage_oil_lamp) | Monsta3D | Basement teplok oil lamp + flame planes; capped |
| [decrepit_wallpaper](https://polyhaven.com/a/decrepit_wallpaper) | Rob Tuytel | Child room wallpaper PBR |
| [old_wood_floor](https://polyhaven.com/a/old_wood_floor) | Guillaume Monsergent | Second-floor boards PBR |
| [dirty_tiles](https://polyhaven.com/a/dirty_tiles) | Matterfield, Jenelle van Heerden | Bathroom tile PBR |
| [quatrefoil_jacquard_fabric](https://polyhaven.com/a/quatrefoil_jacquard_fabric) | colormass, Rico Cilliers | Master quilt/curtain fabric PBR |
| [stained_pine](https://polyhaven.com/a/stained_pine) | Dario Barresi, Jenelle van Heerden | Stair/basement timber PBR |
| [ceiling_interior](https://polyhaven.com/a/ceiling_interior) | Dimitrios Savva | Second-floor ceiling PBR |
| [painted_concrete](https://polyhaven.com/a/painted_concrete) | Dario Barresi, Dimitrios Savva | Basement walls/floor PBR |

Source FBX files are imported under `Assets/_LayarTerakhir/Art/Imported/PolyHaven/`; 1K maps under
`Assets/_LayarTerakhir/Art/Atmaja/Textures/`. `Tools/prepare_upper_decimate.py` applies absolute
per-mesh polygon caps (e.g. encyclopedia volumes 120, tool chest 400, picture frames 600) so the
two-story scene keeps an integrated-GPU static budget; caps are recorded in that tool and are
idempotent. No source mesh is relicensed; only CC0 terms apply.

Ten original Blender props authored with `Tools/author_atmaja_upper.py`
(`Assets/_LayarTerakhir/Art/Original/Models/`): bathtub, sink pedestal, standing mirror, standing
Nyonya figure, hanged Nyonya figure, rusted shears, sewing machine, sewing bench, hanging rag, and
trapdoor slab. They use project materials (enamel, mirror glass, old rope, black iron, rust, dirty
kebaya) following the original project license — not downloads and not extracted game art. Basement
audio is runtime-synthesized original waveforms; the Nyonya hum is generated `AtmajaSynth` tones,
not a vocal recording. The handycam "playback" is a synthetic static clip, not real footage.

The original backdrop's new folded storage mesh is authored by
`Tools/author_compact_kos.py --stored-only`; it retains the green cloth and collapsed frame,
with no cloth simulation. It follows the original project's license, not Poly Haven's CC0.
