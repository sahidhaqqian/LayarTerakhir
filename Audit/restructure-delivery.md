# Restrukturisasi repository — 18 Oktober 2026

Instruksi: migrasi lokasi file sesuai `restruktur.md`, tanpa refactor atau perubahan gameplay. Baseline: `b347745e917940afd5f8f1470b2049dbdf76329a`. Checkpoint lokal: `checkpoint/restructure-before-20261018`.

## Struktur akhir

```text
Assets/
  _LayarTerakhir/
    assets/
      Art/         # model, material, tekstur, animasi; pengelompokan impor tetap
      Audio/
      Production/  # Prefabs dan Presets
      Scenes/
      UI/
    src/
      Audio/, Core/, Diagnostics/, Player/, Prolog/, Smartphone/, UI/
      Editor/      # builder, validasi, build, tes editor yang sudah ada
  Resources/       # lokasi dan Resources.Load tidak berubah
  Settings/, Scenes/, TutorialInfo/  # template/engine tetap
Packages/, ProjectSettings/, Tools/, Docs/
```

Tidak membuat folder kosong atau memisahkan isi paket aset yang saling terkait. Assembly runtime/editor tetap `Assembly-CSharp` dan `Assembly-CSharp-Editor`; tidak ada asmdef/asmref first-party untuk diubah.

## Perpindahan

**2.300 file dipindahkan**, termasuk file `.meta` dan metadata folder; perpindahan menggunakan `git mv`. Satu metadata baru dibuat untuk container `assets/`, tanpa mengganti GUID lama.

| Folder lama | Folder baru | File termasuk .meta |
| --- | --- | ---: |
| `Assets/_LayarTerakhir/Scripts` | `Assets/_LayarTerakhir/src` | 128 |
| `Assets/_LayarTerakhir/Editor` | `Assets/_LayarTerakhir/src/Editor` | 71 |
| `Assets/_LayarTerakhir/Art` | `Assets/_LayarTerakhir/assets/Art` | 1930 |
| `Assets/_LayarTerakhir/Audio` | `Assets/_LayarTerakhir/assets/Audio` | 128 |
| `Assets/_LayarTerakhir/Production` | `Assets/_LayarTerakhir/assets/Production` | 23 |
| `Assets/_LayarTerakhir/Scenes` | `Assets/_LayarTerakhir/assets/Scenes` | 7 |
| `Assets/_LayarTerakhir/UI` | `Assets/_LayarTerakhir/assets/UI` | 13 |

Mapping per file: [restructure-file-map.json](restructure-file-map.json). Folder Tests terpisah tidak dibuat karena tes existing berada dalam Editor.

## Perubahan referensi lokasi

Semua perubahan C# hanya substitusi literal path; 60 skrip runtime tidak berubah satu byte pun. Daftar kode/config/tooling yang diubah:

- `.graphifyignore`
- `AssetResearch/tools/build_manifest.py`
- `Assets/_LayarTerakhir/src/Editor/AtmajaReviewDressing.cs`
- `Assets/_LayarTerakhir/src/Editor/AtmajaSceneBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/AtmajaSceneValidation.cs`
- `Assets/_LayarTerakhir/src/Editor/AtmajaStairRevisionBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/AtmajaUpperBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/BloodWriting.cs`
- `Assets/_LayarTerakhir/src/Editor/CompactKosPolish.cs`
- `Assets/_LayarTerakhir/src/Editor/DesignerReviewBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/DesignerWorldRevision.cs`
- `Assets/_LayarTerakhir/src/Editor/GoldenHudBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/GoldenSceneProduction.cs`
- `Assets/_LayarTerakhir/src/Editor/GoldenSceneValidation.cs`
- `Assets/_LayarTerakhir/src/Editor/KosCorridorBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/KosCorridorBuilding.cs`
- `Assets/_LayarTerakhir/src/Editor/KosVisualUpgrade.cs`
- `Assets/_LayarTerakhir/src/Editor/KosVisualV2.cs`
- `Assets/_LayarTerakhir/src/Editor/MelatiNeighborhoodBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/PlaytestRepairBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/PlaytestRevisionBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/PrologCompletionBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/PrologKosSceneBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/PrologPresentationBuilder.cs`
- `Assets/_LayarTerakhir/src/Editor/WindowsDevelopmentBuild.cs`
- `ProjectSettings/EditorBuildSettings.asset`
- `Tools/author_atmaja_character.py`
- `Tools/author_atmaja_details.py`
- `Tools/author_atmaja_upper.py`
- `Tools/author_compact_kos.py`
- `Tools/author_designer_review_props.py`
- `Tools/author_golden_details.py`
- `Tools/author_kos_details.py`
- `Tools/author_kos_v2.py`
- `Tools/author_prolog_characters.py`
- `Tools/author_raka_scooter.py`
- `Tools/author_voice_from_official_demo.py`
- `Tools/download_kos_v2.py`
- `Tools/export_dialogue_recording.py`
- `Tools/fetch_designer_review_assets.py`
- `Tools/improve_kos_audio.py`
- `Tools/neural_prolog_voice.py`
- `Tools/pack_atmaja_masks.py`
- `Tools/package_windows_build.py`
- `Tools/prepare_atmaja_art.py`
- `Tools/prepare_golden_audio.py`
- `Tools/prepare_golden_sources.py`
- `Tools/prepare_kos_art.py`
- `Tools/prepare_kos_refinement.py`
- `Tools/prepare_upper_decimate.py`
- `Tools/repair_kos_window.py`
- `Tools/repair_playtest_art.py`
- `Tools/retexture_kenney_wood.py`
- `Tools/run_golden_checks.py`
- `Tools/tests/test_graphify_discovery.py`
- `Tools/tests/test_update_graphify.py`
- `Tools/update_graphify.py`
- `install_milestone1.sh`

Konfigurasi Graphify kini mengizinkan C# hanya dalam `Assets/_LayarTerakhir/src` dan Python/shell dalam `Tools`; aset tetap ditolak. Build settings hanya berubah path, dengan GUID dan urutan ketiga scene sama. Manifest voice-bank aktif, kredit aset, indeks dialog untuk rekaman, manifest integrasi riset, README, arsitektur dan dokumentasi produksi diperbarui pada referensi lokasinya. Tidak ada audio yang dibuat ulang.

## Bukti integritas

- 2.354 file Assets baseline ditemukan pada lokasi hasil migrasi; tidak ada file hilang.
- 2.331 file identik byte demi byte; 23 sisanya C# editor dengan substitusi path saja.
- Seluruh **1.230 GUID lama** identik; total sesudah migrasi 1.231 dengan metadata container baru.
- Tidak ada GUID duplikat, `.meta` yatim, atau aset tanpa pasangan `.meta`.
- Seluruh **12 scene/prefab** di Assets identik byte demi byte, termasuk prefab Resources.
- Semua 60 C# runtime identik; class, namespace, method, interaksi dan gameplay tidak diubah.
- Tidak ada path lama dalam kode/config/tooling aktif yang telah dimigrasi.
- `git diff --find-renames` mendeteksi 2.300 rename; perubahan penghapusan `deepseek.md` milik pengguna tidak termasuk tugas.

Snapshot baseline: [restructure-before.json](restructure-before.json). Hasil pemeriksaan: [restructure-static-validation.json](restructure-static-validation.json).

## Yang dipertahankan

- `AssetResearch`, `Scene2_Atmaja_Assets`, `Scene2_Atmaja_Recovery`: koleksi sumber, bukti lisensi, riset dan arsip authoring. Semua tetap di luar Assets/runtime; hanya referensi tujuan integrasi yang disesuaikan.
- Resources, template Unity, Packages, pengaturan selain path build, serta struktur impor vendor tetap pada lokasi kompatibel.
- Library, Temp, Obj, Logs dan hasil build tidak direstrukturisasi sebagai source.
- Bukti historis Audit berisi hash/timestamp/capture lama tetap menjadi catatan build lama; tidak ditulis ulang untuk mengaku sebagai bukti build baru. Bukti restrukturisasi memakai nama baru.
- Tidak ada dependency baru, optimasi, redesign, perbaikan bug gameplay atau pengembangan fitur.
- `restruktur.md` dan penghapusan `deepseek.md` sudah ada pada working tree sebelum tugas dan tidak ikut di-stage.

## Validasi dan distribusi

- Syntax Python dan shell lulus; **17/17 tes Python** lulus, termasuk pemeriksaan allowlist Graphify nyata.
- Compile Unity runtime dan editor berhasil. **86/86 tes Unity** lulus, 0 gagal, 0 dilewati; exit code 0. [XML](restructure-tests.xml), [invokasi](restructure-test-invocation.json).
- Validator kos: 0 kegagalan, 1.151 pemeriksaan dasar dan 31 tambahan. Validator Atmaja: 0 kegagalan. Keduanya memakai lokasi baru; tidak membangun ulang scene.
- Build standalone Linux berhasil. Audit rute awal: **57 screenshot, completed=true**, mencakup menu, telepon, dokumen, kamera, tangga, perumahan, Bu Sri, motor, Atmaja dan checkpoint. [Bukti](Visuals/Restructure-Opening/player-audit.json), [invokasi](restructure-opening-invocation.json).
- Audit lanjutan prolog: **30 screenshot, completed=true**, rute fisik 126.83 meter, lantai dua, basement, rekaman handycam, pengejaran, Blitz, keluar jendela, ending dan Continue tanpa pembayaran ganda. [Bukti](Visuals/Restructure-Prolog/player-audit.json), [invokasi](restructure-prolog-invocation.json).
- Runtime diuji sebagai LinuxPlayer pada llvmpipe 640×360 dengan fixture otomatis. Ini bukti alur berjalan, bukan playtest manual Windows, penilaian pacing, atau penerimaan performa perangkat target.
- Build Windows x86_64 Development **Succeeded**, **0 error**, 1 warning; Unity exit code 0. Paket memverifikasi PE AMD64, kelengkapan assembly, marker ketiga scene, seluruh file build, CRC ZIP dan snapshot **2.325 file sumber**. [Ringkasan build](atmaja-build-summary.json), [snapshot](restructure-prolog-presentation-build-sources.json).
- Windows runtime belum dijalankan secara manual di lingkungan ini. Build ini disediakan untuk playtest pengguna.
- **ZIP baru sudah diunggah dan dibaca ulang seluruhnya dari R2**, hash serta ukuran identik dengan lokal. Rclone sempat menerima 501 pada percobaan pertama, retry berhasil; pemeriksaan readback penuh tetap lulus. [Receipt R2](r2-windows-build-upload.json).
- Lokasi ZIP: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`.
- Objek R2: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`.
- Ukuran: **197,196,375 byte**.
- SHA-256: `b92e126b5b833a7351b83db2ab803f9fe47081bf7c1cd0101b9d4b120e47ecf0`.
- Verifikasi upload: `2026-10-18T12:40:45.866394+00:00`.
- Bukti perubahan tooling hanya path/config sumber: [restructure-tool-path-validation.json](restructure-tool-path-validation.json), **33 file**.
- Graphify diperbarui sesudah perubahan kode terakhir: **141 file kode**, **0 aset**. Gate `--check` lulus; empat hasil Graphify disertakan.

## Git

Branch pengiriman `main`, tanpa force-push. Checkpoint Git menunjuk baseline yang sudah tersimpan di remote. Migrasi dan bukti validasi di-commit bersama; hash commit/push aktual disampaikan pada pesan penyerahan. File milik pengguna yang sudah berubah sebelum tugas (`deepseek.md` terhapus dan `restruktur.md` belum tracked) tidak dimasukkan ke commit.
