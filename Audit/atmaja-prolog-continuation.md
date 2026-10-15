# Kelanjutan Prolog — Atmaja

Implementasi 15 Oktober 2026. Mengikuti revisi `goal.md`: Kos approved tidak dikomposisi ulang. Scene baru `Prolog_RumahAtmaja` melanjutkan Kos secara async; build tetap memulai dari Kos.

## Cakupan

Adegan 2 playable: rumah Jawa-Belanda dua lantai, gerbang, motor Damar, dokumen survei #2, tombol MULAI LIVE/14 penonton, lorong timur, sawer terikat 500k idempotent, kamboja dengan penghalang fisik berdiri/celah crouch, vault jendela dapur, jendela menutup dan 250 penonton.

Adegan 3 lantai dasar: sumur berantai, bonus foto opsional sah 150k sekali dengan biaya 1% baterai, pintu ruang makan, ransel/HP retak di kolong meja, transfer modul MataBatin ke satu HP Raka, sosok berleher patah hanya di kamera AR HP, chat viral/1.420 penonton, petunjuk III:XLV, jarum kuningan dan jam 03:45, kunci tangga dan koran #3 sesuai naskah. Pintu tangga menyimpan checkpoint dan menampilkan batas build dengan jelas.

Adegan 4–7 **belum diimplementasikan**. Tidak ada klaim Prolog 90 menit atau pacing final. Transfer chip masih berupa pengambilan prop + subtitle, bukan animasi tangan/cutscene final; voice dialog/bisikan belum direkam. Foley/ambience memakai audio existing, AR memakai static pendek original. Dapur/ruang tamu masih memerlukan dressing lanjutan (termasuk bangkai tikus), refinement scare dan pengujian manual.

## Perlindungan dan produksi

- SHA-256 scene Kos approved: `4b73b117b832fb753b92d3de885c163bc1dd3f6225101867f7b4e2437c8b793a`. Tidak mengubah shared Golden HUD/prefab/material/volume/pipeline asset. Hook runtime Kos hanya untuk transisi/resume.
- Seluruh staging direview; hanya model/material relevan diimpor. Tidak mengunduh ulang aset. Wheel axis motor diperbaiki; jok terbuka/notebook; kamboja dikurangi dari 127.457 ke 28.825 triangle; rumah digabung per material, kaca dapur solid dihapus, lubang pintu dapur dibuat nyata. Table/clock/sosok AR dimodel original dengan Blender.
- Validasi scene mencatat 146.345 triangle, 130 renderer lingkungan (termasuk interior/AR tersembunyi dan dua emitter hujan), tekstur maksimum 1024px. Small props 512px. Ini budget statik, bukan ukuran frame cost/FPS.
- RT HP 320×400 tanpa post/shadow kedua, mati saat layar mati/disimpan/modal. AR layer 29 tidak masuk kamera dunia. Rain emitter dibatasi halaman/lorong agar tidak menembus atap, tanpa particle collision. Satu moon shadow; local practical tanpa shadow.
- Event/checkpoint dan hadiah disimpan atomik; perubahan hadiah/flag rollback jika gagal. Load dari Kos mengarahkan checkpoint Atmaja langsung ke spawn relevan, membawa input binding, item/baterai/uang. Koran dapat dibaca ulang bila game terputus sebelum dokumen ditutup. Port charge motor/dapur tetap tersedia setelah powerbank habis, termasuk baterai 0%.
- Kredit CC BY 4.0 **wajib** untuk Wooden Overhang Modular Kit (Trey Ramm/minime453), dengan sumber/lisensi/perubahan di `asset-credits.md`; artwork Poly Haven lain CC0. Kredit disertakan di ZIP.

## Bukti dan batas pengujian

Suite Unity final **42/42 lulus**, 0 gagal/skip (`golden-tests.xml`, invocation fresh). Meliputi 35 regresi fondasi/Kos, 5 validasi/routing Atmaja, 2 skenario Play Mode Atmaja dengan banyak assertion: dokumen/live, sawer idempotent, penghalang berdiri/crouch, binding vault aktual, pendaratan, foto sumur sekali, baterai 0%, AR satu HP, salah/benar jam, putus saat koran terbuka, dan startup dari save Atmaja. Scene validation: **0 kegagalan**, termasuk material/mesh, lintasan kapsul, window guard, routing build, dan hash Golden.

Standalone Linux actual lulus **17 pemeriksaan**, Kos → Atmaja termasuk binding remap, raycast motor/HP Damar, live/viewer/donasi, crouch fisik, vault, AR layer/layar mati, jam dan save. **12 PNG fresh 1280×720** diverifikasi header/resolusi/waktu oleh runner. Pemeriksaan visual menemukan framing AR awal kurang baik; sosok/kursi diorientasikan ulang dan petunjuk dikecilkan sebelum capture final. Render texture sekarang tidak diaktifkan pada graphics-device Null (runner headless sebelumnya crash); pengujian rendering sebenarnya tetap dilakukan di standalone, bukan diabaikan.

Preview aktual: [gerbang](Visuals/Atmaja-Linux/01-gate-arrival.png), [motor Damar](Visuals/Atmaja-Linux/02-damar-scooter.png), [kamboja](Visuals/Atmaja-Linux/05-fallen-kamboja.png), [dapur](Visuals/Atmaja-Linux/07-kitchen-well.png), [ransel](Visuals/Atmaja-Linux/08-damar-backpack.png), [sosok AR](Visuals/Atmaja-Linux/09-ar-dining-figure.png), [petunjuk AR](Visuals/Atmaja-Linux/10-ar-clock-clue.png), [endpoint jujur](Visuals/Atmaja-Linux/12-ground-floor-checkpoint.png). Receipt: `atmaja-player-invocation.json`, `Visuals/Atmaja-Linux/player-audit.json`; statistik scene: `atmaja-scene-validation.json`.

Windows x86_64 Mono Development **Succeeded, 0 error / 0 warning**, dua scene (Kos index 0, Atmaja index 1). Packaging memverifikasi **290 file keluaran Unity**, **112 assembly**, PE AMD64, marker scene native, dan seluruh **296 file ZIP** dengan CRC. ZIP lokal: `Builds/LayarTerakhir-Windows-x86_64-Development.zip` (**158.687.933 byte / 151,3 MiB**), beserta `.zip.sha256`. Kredit aset/CC BY, Barlow OFL, dan panduan playtest ikut dibundel. Ringkasan mesin: [`atmaja-build-summary.json`](atmaja-build-summary.json); BuildReport lengkap ada di `Builds/Logs/`.

SHA-256 ZIP: `8e0f94c2f22258a5d6f0ba55a43ff3c807ca0d4050478ca27310aff9875132dc`. Windows executable **belum dijalankan** di host ini; visual acceptance/frametime/FPS/memori Iris Xe masih harus diuji di Windows. ZIP/hasil player lokal di-gitignore, bukan disertakan dalam commit sumber.

Screenshot standalone adalah framebuffer aktual Linux llvmpipe, diposekan oleh audit terarah, dengan save terisolasi dan timer dipercepat. Bukan walkthrough manual, bukan bukti pacing normal, bukan hasil Windows/Iris Xe. Panduan pengujian Windows: `atmaja-windows-playtest.md`. Target 30 FPS/8 GB belum terukur.
