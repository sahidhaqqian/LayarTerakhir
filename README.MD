# Layar Terakhir

Game horor psikologis first-person Indonesia. Pemain berperan sebagai **Raka Wijaya**, livestreamer muda yang terjebak kontrak dengan entitas misterius **Penonton_Terakhir**: nyalakan siaran di Rumah Keluarga Atmaja sebelum tengah malam, atau nyawa kameramennya, Damar, taruhannya.

Permainan berjalan dalam enam babak bercerita (Babak 0–5) dengan mekanik eksorsisme berbasis kamera smartphone — AR, UV, Selfie, ThermoKlenik, EVP — serta empat ending: **Bad, Normal, True, dan Secret "Saksi Tanpa Layar"**. Acuan lengkap ada di [`Rancangan.MD`](Rancangan.MD).

---

## Status saat ini

**Milestone: Golden Reference Scene `Prolog_KamarKos` (Kamar Kos Raka).**

Scene kos 6 × 4,5 m sudah dibangun ulang sebagai referensi produksi seluruh game: furnitur & material PBR asli (PolyHaven/Kenney, CC0), pencahayaan horor + post-processing URP, ambience audio ber-mixer, dan HUD produksi (Barlow, bukan lagi teks debug). Alur cerita pembuka — surat ibu kos, voice note Damar, donasi Rp5.000.000, puzzle gembok `148`, pengambilan jaket/kunci motor, hingga keberangkatan — berfungsi lengkap dengan save/checkpoint atomik.

Yang **belum** dibangun: Adegan 2 (Rumah Keluarga Atmaja) dan seterusnya. Logika AR/UV/Selfie, Jurnal, stealth/chase, ritual, dan ending masih berupa fondasi state yang menunggu implementasi. Uji visual dan performa akhir wajib dilakukan pengembang di Windows (Iris Xe) — host pengembangan headless tidak bisa mengklaim hasil itu.

| Item | Status |
| --- | --- |
| Scene startup | `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity` |
| Tes otomatis | 32/32 lulus (EditMode + PlayMode), lihat `Audit/foundation-tests.xml` |
| Validasi scene | `KosSceneValidation` + `GoldenSceneValidation` lulus, lihat `Audit/golden-scene-validation.json` |
| Build Windows | Development build Mono x86_64 berhasil, ZIP tersedia di `Builds/` (lokal, tidak di-commit) |
| Pengujian manual Windows | **Belum dilakukan** — menunggu pengembang |

---

## Teknologi

| Komponen | Versi |
| --- | --- |
| Unity | `6000.3.26f1` |
| Universal RP | `17.3.0` |
| Input System | `1.20.0` |
| Unity Test Framework | `1.6.0` |
| Target build | Windows x86_64, Mono, Development |
| Perangkat sasaran | Intel i5-1155G7 · Iris Xe · 8 GB RAM · Windows 11 (target 30 FPS) |

---

## Struktur repositori

```
Assets/_LayarTerakhir/
  Scenes/      Prolog_KamarKos.unity (scene Golden)
  Scripts/     Core, Player, Prolog, Smartphone, UI, Audio, Diagnostics
  Editor/      alat produksi: builder scene, pass visual, validasi, build
  Art/         model & tekstur (Imported / Original / Generated)
  Audio/       WAV siap pakai, mixer, SceneAudioProfile
  UI/          font Barlow (OFL), prefab GoldenHUD
  Production/  prefab & preset impor yang dapat dipakai ulang
Docs/          GOLDEN_SCENE_STANDARD.md (standar produksi)
Tools/         skrip Python: aset, validasi, build, packaging
Audit/         bukti tes/build, kredit aset, laporan
Builds/        hasil build (di-gitignore)
```

Dokumentasi lengkap arsitektur kode ada di [`ARCHITECTURE.MD`](ARCHITECTURE.MD).

---

## Menjalankan proyek

**Unity Editor**
1. Buka folder proyek ini dengan Unity `6000.3.26f1` (URP; Windows Build Support Mono diperlukan untuk build).
2. Buka `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity`.
3. Tekan Play. Scene sudah berisi seluruh sistem; tidak ada langkah setup manual.

**Menu editor `LayarTerakhir/*`** dipakai untuk produksi scene. Jangan menjalankan `Build Prolog Kamar Kos Scene` (greybox builder) di atas scene berfurnitur — pass produksi berikutnya bersifat inkremental.

**Headless (CI/validation host)**

```sh
python3 Tools/run_golden_checks.py --stage validate
python3 Tools/run_golden_checks.py --stage test
```

---

## Kontrol (mengikuti `Rancangan.MD`)

| Tombol | Aksi | Tombol | Aksi |
| --- | --- | --- | --- |
| `WASD` | Gerak | `T` | ThermoKlenik |
| `SHIFT` | Sprint | `C` | EVP Recorder |
| `CTRL` | Crouch / slide saat sprint | `J` | Jurnal |
| `SPACE` | Vault | `B` | Powerbank |
| `E` | Interaksi dunia | `G` | Flare |
| `TAB` | Angkat/simpan HP | `H` | Kemenyan |
| `F` | Matikan/nyalakan layar HP | `LMB` | Foto / Blitz |
| `Q` / `R` | AR / UV | `V` | Kamera Selfie |

Semua tombol dapat dipetakan ulang (`GameInputConfig`); prompt HUD selalu menampilkan binding aktif. AR dan UV saling menggantikan pada satu slot pemindaian.

---

## Alur Prolog Kamar Kos (milestone saat ini)

1. **Bangun** — monolog, suara hujan & ketukan pintu.
2. **Periksa bawah pintu** — ambil surat peringatan ibu kos (`DOC_PROLOG_01_SURAT_KOS`).
3. **Ambil smartphone di kasur** — voice note Damar, lalu notifikasi donasi terikat Rp5.000.000 dari `Penonton_Terakhir`.
4. **Persiapan berangkat** — baca sticky note + kalender, pecahkan gembok laci `148` (kunci motor + powerbank), ambil jaket.
5. **Keluar** — save checkpoint `PROLOG_KOS_COMPLETE`, fade hitam menuju Rumah Atmaja (belum dibangun).

Checkpoint dapat dimuat kembali; state inventori, baterai HP, dan objek yang sudah diambil dipulihkan tanpa menggandakan hadiah.

---

## Validasi dan build

```sh
python3 Tools/run_golden_checks.py --stage apply      # pass produksi visual (saat authoring saja)
python3 Tools/run_golden_checks.py --stage validate   # validasi geometri, audio, HUD, prefab
python3 Tools/run_golden_checks.py --stage test       # EditMode + PlayMode
python3 Tools/run_golden_checks.py --stage player     # smoke player Linux via Xvfb (bukan bukti Windows)
python3 Tools/run_golden_checks.py --stage windows    # build Windows Development
python3 Tools/package_windows_build.py                # verifikasi + ZIP untuk WinSCP
```

Bukti (log, XML tes, JSON validasi, screenshot, checksum) tersimpan di `Audit/`. Panduan pengujian manual Windows ada di `Audit/windows-kos-capture.md` dan `Builds/Windows/Screenshot-Guide.txt`.

Saat menjalankan build player dengan flag `-goldenAudit <dir>`, game menjalankan smoke test terarah dengan folder save terisolasi dan menulis 13 screenshot + rekaman mix audio nyata. Flag ini tidak aktif pada permainan normal.

---

## Aset dan lisensi

- Kode proyek: lihat [`LICENSE`](LICENSE) (GPL-2.0).
- Aset pihak ketiga: PolyHaven dan Kenney (CC0), font Barlow (SIL OFL). URL sumber, penulis, lisensi, dan checksum dicatat di [`Audit/asset-credits.md`](Audit/asset-credits.md) dan [`Audit/asset-download-manifest.json`](Audit/asset-download-manifest.json).
- Build Windows menyertakan `Asset-Credits.txt` dan `Barlow-OFL.txt`; jangan mendistribusikan build tanpa berkas tersebut.

---

## Dokumentasi

| Dokumen | Isi |
| --- | --- |
| [`Rancangan.MD`](Rancangan.MD) | Aturan gameplay, HP, baterai, ritual, collectible, kontrak data |
| [`Prolog.MD`](Prolog.MD) | Naskah lengkap Prolog Malam Pertama di Rumah Atmaja |
| [`Phase1.MD`](Phase1.MD)–[`Phase5.MD`](Phase5.MD) | Naskah Babak 1–5 dan jalur secret |
| [`Checklist.MD`](Checklist.MD) | Checklist produksi per babak |
| [`goal.md`](goal.md) | Brief produksi Golden Reference Scene |
| [`Docs/GOLDEN_SCENE_STANDARD.md`](Docs/GOLDEN_SCENE_STANDARD.md) | Standar produksi scene berikutnya |
| [`ARCHITECTURE.MD`](ARCHITECTURE.MD) | Arsitektur runtime, data, pipeline produksi |
| [`Audit/REPORT.md`](Audit/REPORT.md) | Hasil audit fondasi dan batasan yang tersisa |

---

## Konvensi kerja

- Setiap pekerjaan yang selesai wajib di-commit dan di-push (`Agents.MD`).
- Fokus pengujian utama adalah sintaks/kompilasi, dengan suite Unity dijalankan bertahap.
- Jangan menulis ulang sistem yang sudah bekerja (controller, interaksi, puzzle, save/load); perubahan bersifat aditif dan menjaga ID interaksi serta inventori.
- Jangan menyentuh save pemain: validasi dan tes memakai `LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY`.
- Bukti visual/performa hanya boleh diklaim dari hasil nyata; kebijakan lengkap ada di `Docs/GOLDEN_SCENE_STANDARD.md`.

---

## Lisensi

GPL-2.0 — lihat [`LICENSE`](LICENSE).
