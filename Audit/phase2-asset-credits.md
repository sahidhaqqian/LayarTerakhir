# Phase 2 — sumber aset Sesi 1

Lingkungan rumah sakit menggunakan unduhan baru, bukan pemilahan aset Prolog/Phase 1. Semua mesh/tekstur berasal dari sumber berikut; penyesuaian berupa konversi format, fitting skala, ekstraksi bagian geometri existing, optimasi dan packing kanal PBR. Tidak ada model, tekstur, gambar, atau klip gerak original yang dibuat untuk sesi ini.

- **Madduck modular hospital** — Madduck. [Sumber asli](https://madduck.itch.io/modular-3d-hospital-environment). Author license: commercial game use and adaptation; no standalone asset redistribution.
- **AutopsyLab** — Evan Petrov. [Autopsy Lab - Assets](https://sketchfab.com/3d-models/5ceaaa4482a84774b9922366cfb228e7). CC-BY-4.0. [Ketentuan lisensi](https://creativecommons.org/licenses/by/4.0/).
- **Neonatal** — centronina3d. [Isola Neonatale](https://sketchfab.com/3d-models/b27fc2f948c34c6084488d9d3e3b64db). CC-BY-4.0. [Ketentuan lisensi](https://creativecommons.org/licenses/by/4.0/).
- **Scissors** — Skript47. [Scissors Stationery (Low Poly, Game Ready)](https://sketchfab.com/3d-models/c67efef1abeb41969eef84c06311fac6). CC-BY-4.0. [Ketentuan lisensi](https://creativecommons.org/licenses/by/4.0/).
- **Safe** — pixelgrapher. [Antique Iron Safe](https://sketchfab.com/3d-models/196cf0d3d5094b50874dbf51064a7d3e). CC-BY-4.0. [Ketentuan lisensi](https://creativecommons.org/licenses/by/4.0/).
- **Generator** — Alpha One. [Portable Generator](https://sketchfab.com/3d-models/222717d27f0b4a4f84106260c1fd0d7e). CC-BY-4.0. [Ketentuan lisensi](https://creativecommons.org/licenses/by/4.0/).
- **Kenney stair module** — Kenney. [Sumber asli](https://kenney.nl/assets/prototype-kit). CC0-1.0.
- **chemistry_set** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/chemistry_set). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).
- **medical_tape** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/medical_tape). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).
- **metal_tool_chest** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/metal_tool_chest). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).
- **bunsen_burner** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/bunsen_burner). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).
- **tool_cart** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/tool_cart). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).
- **large_iron_gate** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/large_iron_gate). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).
- **binder_notebook** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/binder_notebook). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).
- **red_brick_03** — Poly Haven, kontributor sebagaimana halaman sumber. [Sumber asli](https://polyhaven.com/a/red_brick_03). CC0-1.0. [Ketentuan lisensi](https://polyhaven.com/license).

Karakter Lastri memakai **Merah.vrm milik desainer**, dikonversi menjadi FBX, disederhanakan dan memakai retarget klip existing. SHA-256 sumber serta perubahan ada di `phase2-lastri-import.json`. Raka, genggaman HP, UI, font, shader teks, pipeline render, dan Kos existing dipertahankan sebagai fondasi bersama. Kos tidak dibangun ulang atau diperkecil.

Madduck mengizinkan pemakaian/penyesuaian dalam game komersial, tetapi bukan distribusi sebagai aset lepas. Karena repositori publik, binary impor Madduck diabaikan Git; sidecar GUID, konfigurasi material, sumber/lisensi dan alat reproduksi tetap tersedia. Jalankan `python3 Tools/fetch_phase2_assets.py`, `blender -b -t 2 --python Tools/import_phase2_assets.py -- --keep-lastri`, lalu `python3 Tools/pack_phase2_surfaces.py` sebelum membuka/build clean checkout. `--keep-lastri` mempertahankan FBX/tekstur Lastri yang sudah disertakan Git; hapus opsi ini hanya bila VRM asli desainer tersedia dan memang hendak dikonversi ulang. Build player menyertakan hasil impor dalam game.

Kabut di pintu Ruang Operasi memakai `smoke_01.png` yang tidak diubah dari [Kenney Particle Pack](https://kenney.nl/assets/particle-pack), CC0-1.0, diatur sebagai particle mist lokal dengan maksimum 32 partikel.

Permukaan cold-storage memakai [Metal Plate 02](https://polyhaven.com/a/metal_plate_02), Poly Haven / Rob Tuytel, CC0-1.0. Alat pengambilan aset: **Powered by Poly Haven**.

## Audio baru

- **Moans.zip** — qubodup, [sumber](https://opengameart.org/content/ghost-monster-voice-moaning-growling), CC0-1.0. Konversi mono 32 kHz, pemotongan/filter/gain untuk penempatan ruang; tidak ada dialog AI baru.
- **Breathing.zip** — primbal, [sumber](https://opengameart.org/content/horror-breathing), CC0-1.0. Konversi mono 32 kHz, pemotongan/filter/gain untuk penempatan ruang; tidak ada dialog AI baru.
- **Mechanical.zip** — LEGIT Audio, [sumber](https://opengameart.org/content/the-shop), CC0-1.0. Konversi mono 32 kHz, pemotongan/filter/gain untuk penempatan ruang; tidak ada dialog AI baru.
- **Rain.wav** — alxl, [sumber](https://opengameart.org/content/rain-on-window-loop), CC0-1.0. Konversi mono 32 kHz, pemotongan/filter/gain untuk penempatan ruang; tidak ada dialog AI baru.
- **Baby.ogg** — TokyoGeisha, [sumber](https://opengameart.org/content/baby-sound-effect), CC0-1.0. Konversi mono 32 kHz, pemotongan/filter/gain untuk penempatan ruang; tidak ada dialog AI baru.

Dialog Raka, Damar dan kalimat EVP kanon disajikan sebagai subtitle; rekaman pemeran manusia berbahasa Indonesia belum tersedia. Ambience mekanikal, hujan dan cue nonverbal memakai sumber di atas. Kredensial dan URL unduhan sementara tidak dicatat. Tidak ada aset/kode DreadOut yang disalin.
