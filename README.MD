# Layar Terakhir

Game horor psikologis Indonesia dengan kamera **over-the-shoulder third-person / first-person**, dipilih dengan **V** atau Settings. Pemain berperan sebagai **Raka Wijaya**, livestreamer muda yang terjebak kontrak dengan entitas misterius **Penonton_Terakhir**: nyalakan siaran di Rumah Keluarga Atmaja sebelum tengah malam, atau nyawa kameramennya, Damar, taruhannya.

Permainan berjalan dalam enam babak bercerita (Babak 0–5) dengan mekanik eksorsisme berbasis kamera smartphone — AR, UV, Selfie, ThermoKlenik, EVP — serta empat ending: **Bad, Normal, True, dan Secret "Saksi Tanpa Layar"**. Acuan lengkap ada di [`Rancangan.MD`](Rancangan.MD).

---

## Status saat ini

**Prolog: Kos → Rumah Atmaja → penutup Malam Pertama.**

Kos dan revisi desainer yang sudah diselesaikan dipertahankan. Kelanjutan mengikuti
`Prolog.MD`: lantai dua dan tiga foto tragedi, kamar utama/pintu jebakan, ruang jahit
bawah tanah, stealth Nyonya Sri Atmaja, rekaman handycam Damar, pelarian, Blitz di
jendela dapur, pesan Penonton_Terakhir dan title drop. `CONTINUE` memulihkan checkpoint,
termasuk layar penutup setelah prolog tamat. Babak 1–5 belum playable.

Pass penyelesaian memperbaiki deteksi ancaman/penghalang, pemeriksaan lemari dengan
waktu respons, powerbank dan donasi atomik, rekaman terputus yang dapat diputar lagi,
rute perabot/kamar utama dan transisi pelarian. Rekaman handycam sekarang menampilkan
adegan visual original di engine: pasar malam di balik cermin, tangan yang meraih,
dan kamera Damar ditarik masuk. Cermin kamar mandi memakai refleksi terbatas dan
aparisi yang hanya terlihat di cermin; petunjuk AR menggunakan guratan fisik.
Raka dan Nyonya kini memakai mesh berkulit dan kerangka 48 tulang dari data grafis
CC0 MakeHuman yang diadaptasi, dengan klip gerak buatan proyek. Raka mempunyai 11 klip
untuk berjalan, berlari, jongkok, HP dan motor; Nyonya mempunyai lima klip untuk
diam, patroli, kejar, ancaman dan recoil Blitz. Kebaya gading memakai tekstur renda
original yang dihasilkan dengan imagegen; asal dan lisensi tercatat di kredit aset.
OTS menjadi pilihan awal, V berpindah ke first-person. Lemari dan standoff Blitz
menggunakan closeup first-person sementara, lalu mengembalikan pilihan pemain.

Laporan dan bukti sesi penyelesaian ada di
[`Audit/prolog-completion-2026-10-17.md`](Audit/prolog-completion-2026-10-17.md).
Revisi kamera dan karakter berikutnya dicatat di
[`Audit/hybrid-camera-2026-10-17.md`](Audit/hybrid-camera-2026-10-17.md).
Panduan bermain dan pemeriksaan Windows ada di
[`Audit/atmaja-windows-playtest.md`](Audit/atmaja-windows-playtest.md).

DreadOut menjadi acuan penyajian horor, bukan klaim kesetaraan hasil. Raka, sebagian
aparisi, animasi karakter dan VO lama masih memerlukan penyelesaian artistik serta rekaman manusia.
Durasi 90 menit, penerimaan artistik dan target 30 FPS Iris Xe belum dibuktikan
oleh pengujian otomatis Linux.

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
  Scenes/      Prolog_KamarKos (approved), Prolog_KoridorKos, Prolog_RumahAtmaja
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
3. Tekan Play. Editor melewati menu untuk iterasi scene; standalone selalu mulai dari menu.
   Untuk memeriksa menu di Editor panggil `GameShell.Instance.OpenMain()` saat Play.

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
| `Q` / `R` | AR / UV | `V` | OTS ↔ first-person |
| `X` | Kamera Selfie (modul lanjutan) | | |

Sembilan aksi utama bisa di-remap lewat SETTINGS; binding lain tersedia di `GameInputConfig`. Prompt dunia mengikuti binding aktif. Shortcut UI tetap: `ESC` menu/jeda, `Z` aplikasi ↔ kamera HP, `M` peta HP. `J` membuka Jurnal; entri dapat diklik untuk detail. Baris modul cerita lanjutan di tabel adalah rancangan, belum semuanya playable. AR dan UV memakai satu slot pemindaian.

---

## Alur awal prolog

0. **Menu** — NEW GAME (mengarsipkan save lama), CONTINUE (checkpoint valid), SETTINGS, EXIT.
1. **Bangun** — animasi dari kasur, monolog, hujan, ketukan dan suara Ratna.
2. **Periksa bawah pintu** — ambil surat peringatan ibu kos (`DOC_PROLOG_01_SURAT_KOS`).
3. **Ambil smartphone di kasur** — panggilan Damar otomatis, lalu notifikasi donasi terikat Rp5.000.000 dari `Penonton_Terakhir`.
4. **Persiapan berangkat** — baca sticky note + kalender, pecahkan gembok laci `1408` (powerbank, dompet dan buku harian); ambil kunci motor di meja dan jaket di gantungan.
5. **Keluar kamar** — checkpoint `PROLOG_KOS_COMPLETE`, lalu lorong lantai 2, tangga, fasilitas bersama dan halaman kos. Checkpoint lorong/halaman memuat kembali scene koridor.
6. **Berangkat** — dekat motor/gerbang pilih Atmaja lewat peta HP; animasi naik motor dan melaju keluar gerbang sebelum load Atmaja. Continue dengan prefiks `PROLOG_ATMAJA_` atau `PROLOG_MATABATIN_ACQUIRED` langsung ke Atmaja tanpa mengulang pembuka.

Checkpoint dapat dimuat kembali; state inventori, baterai HP, dan objek yang sudah diambil dipulihkan tanpa menggandakan hadiah.

---

## Validasi dan build

```sh
python3 Tools/run_golden_checks.py --stage validate   # validasi geometri, audio, HUD, prefab
python3 Tools/run_golden_checks.py --stage atmaja-validate
python3 Tools/run_golden_checks.py --stage review-validate
python3 Tools/run_golden_checks.py --stage test       # EditMode + PlayMode
python3 Tools/run_golden_checks.py --stage linux      # tiga scene untuk audit standalone
python3 Tools/run_golden_checks.py --stage review-player # menu, kos, tangga, motor, jalan fisik Atmaja
python3 Tools/run_golden_checks.py --stage player     # smoke player Linux via Xvfb (bukan bukti Windows)
python3 Tools/run_golden_checks.py --stage windows    # build Windows Development
python3 Tools/package_windows_build.py                # verifikasi + ZIP untuk WinSCP
```

Bukti (log, XML tes, JSON validasi, screenshot, checksum) tersimpan di `Audit/`. Panduan pengujian manual Windows ada di `Audit/windows-kos-capture.md` dan `Builds/Windows/Screenshot-Guide.txt`.

`-reviewAudit <dir>` menguji alur revisi dengan save terisolasi dan screenshot framebuffer aktual. Interaksi kamar Golden diposisikan untuk regresi; koridor/tangga/Atmaja ditempuh lewat substep CharacterController, bukan teleport. Timer cerita dipercepat dan waktu dibekukan singkat saat capture; bukan pengujian pacing/VO natural atau FPS. `-goldenAudit` lama hanya untuk smoke Kos terisolasi; `-atmajaAudit` lama bukan bukti revisi ini. Stage `atmaja-player` kini alias `review-player`. Semua flag audit tidak aktif pada permainan normal.

Authoring kelanjutan terbaru bersifat inkremental dan mempertahankan kedua scene kos:

```sh
python3 Tools/run_golden_checks.py --stage prolog-complete
python3 Tools/run_golden_checks.py --stage atmaja-validate
python3 Tools/run_golden_checks.py --stage linux
python3 Tools/run_golden_checks.py --stage prolog-player --width 640 --height 360 --review-output Audit/Visuals/Prolog-Completion
```

Flag `-prologAudit` memakai save fixture terisolasi setelah puzzle jam lantai dasar,
lalu menempuh lantai dua hingga tamat melalui substep CharacterController dan binding
interaksi asli. Screenshot membekukan waktu sebentar. Ini pemeriksaan jalur terarah,
bukan playthrough manual, validasi durasi, atau tolok ukur Windows.
Jangan menjalankan builder greybox/produksi historis untuk melanjutkan prolog ini.
Tidak ada sintesis VO baru; naskah rekaman manusia tetap tersedia di `Audit/Recording`.

---

## Aset dan lisensi

- Kode proyek: lihat [`LICENSE`](LICENSE) (GPL-2.0).
- Aset pihak ketiga: PolyHaven/Kenney (CC0), bagian Wooden Overhang Modular Kit oleh Trey Ramm (CC BY 4.0; kredit wajib), font Barlow (SIL OFL). Sumber dan perubahan dicatat di [`Audit/asset-credits.md`](Audit/asset-credits.md), [`Audit/atmaja-art-manifest.json`](Audit/atmaja-art-manifest.json).
- Build Windows menyertakan `Asset-Credits.txt`, `Barlow-OFL.txt`, dan `DejaVu-Font-License.txt` (outline angka jam); jangan menghapus pemberitahuan lisensi. VO neural sintetis dirender menjadi WAV offline; model/tool/service sintesis tidak dibundel dan tidak dibutuhkan saat bermain. SFX tambahan adalah karya original proyek.

---

## Dokumentasi

| Dokumen | Isi |
| --- | --- |
| [`Rancangan.MD`](Rancangan.MD) | Aturan gameplay, HP, baterai, ritual, collectible, kontrak data |
| [`Prolog.MD`](Prolog.MD) | Naskah lengkap Prolog Malam Pertama di Rumah Atmaja |
| [`Phase1.MD`](Phase1.MD)–[`Phase5.MD`](Phase5.MD) | Naskah Babak 1–5 dan jalur secret |
| [`Checklist.MD`](Checklist.MD) | Checklist produksi per babak |
| [`goal.md`](goal.md) | Catatan revisi desainer yang sudah diselesaikan; bukan brief sesi kelanjutan |
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
