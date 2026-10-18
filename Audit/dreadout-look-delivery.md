# Pengiriman revisi grafis DreadOut — 18 Oktober 2026

Revisi grafis ketiga scene mengikuti pembacaan visual atas [galeri resmi DreadOut (2014)](https://store.steampowered.com/app/269790/DreadOut/). Detail referensi dan sumber aset ada di [dreadout-visual-reference.md](dreadout-visual-reference.md). Gambar DreadOut hanya menjadi acuan; tidak disertakan sebagai aset game.

## Diterapkan

- Grading HDR, buffer 64 bit, SMAA High, dithering, grain dan vignette halus; detail gelap tidak lagi posterized seperti sebelumnya.
- Langit awan kontinu dengan bulan kecil; gelombang berulang pada langit dihapus.
- Aspal PBR 1K CC0 dengan normal/roughness/AO, tanah lembap, cat rumah lebih terbaca, variasi jendela terang/gelap, diffuser lampu terlihat dan sorot lampu jalan mengarah ke jalan.
- Pantulan lingkungan malam dikurangi agar jalan tidak tampak biru terang merata; practical hangat dan sorotan HP dingin menjadi sumber lokal.
- Cahaya khusus avatar diturunkan agar menyatu dengan lingkungan. Geometri dan seluruh collider ketiga scene tetap sama sebelum/sesudah pass authoring.
- Proteksi opening memakai identitas struktur historis dari commit `30d9e7e`, mengizinkan perubahan presentasi kamera secara terbatas sambil tetap menolak mutasi transform, ukuran collider dan kecepatan gerak.

## Diterapkan pada alat pengujian

Fixture prolog sebelumnya menyisipkan berhenti 0,45 detik setiap berganti jongkok/berdiri. Saat pengejaran, jeda tes itu menghabiskan jarak aman. Fixture sekarang menunggu input Ctrl dan tinggi kapsul yang sebenarnya selesai, dengan timeout dan assertion tambahan. Kode AI pengejaran, kecepatan musuh, jarak tangkap, grace period, controller fisik serta deadline Blitz tidak diubah.

Dua run penuh yang gagal dan satu diagnostik berukuran viewport tidak cocok disimpan terpisah di `Visuals/Dreadout-Look-Prolog-Software-Timing-Diagnostic/`. Itu bukan bukti final. Run penuh final 640×360 berhasil tanpa reset checkpoint; tidak menurunkan kesulitan game untuk meluluskan tes.

## Diverifikasi

- 88/88 tes Unity penuh setelah revisi HDR/malam dan proteksi struktur; 9/9 tes terarah setelah penyesuaian intensitas pantulan. Koreksi fixture opt-in berikutnya diverifikasi melalui kompilasi kedua platform serta prolog penuh aktual.
- 19/19 tes Python, termasuk proteksi packaging terhadap perubahan layout/gameplay opening.
- Delapan pasangan gambar pembanding; final 960×540. [Pembanding interaktif](Visuals/Dreadout-Look-Comparison.html).
- Pembuka/perjalanan: 59 gambar, 233 pemeriksaan lulus, 230.24 meter perjalanan CharacterController, motor tiba di Atmaja, checkpoint tersimpan.
- Prolog penuh final: 30 gambar pada 640×360, 147 pemeriksaan lulus, 129.82 meter, penutup dan Continue berhasil tanpa pembayaran ganda/reset tersembunyi.
- Windows x86_64 Development: 0 error, 1 warning. ZIP lengkap, CRC/PE/DLL/ketiga scene dan identitas sumber diverifikasi.
- Snapshot 2.430 file cocok setelah kedua build; Graphify 150 file kode, 2.048 node, 7.246 edge, 0 aset.

## ZIP dan R2

`Builds/LayarTerakhir-Windows-x86_64-Development.zip` — 201,488,856 byte.

SHA-256: `29130471ac7619700a91176102e9f632812cc3bfacd6bf71136dbcadf152e0aa`.

Tujuan: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`. Upload berhasil dan objek remote dibaca kembali seluruhnya; SHA-256 sama dengan ZIP lokal pada `2026-10-18T17:17:35.899138+00:00`. Receipt: `dreadout-look-r2-upload.json`.

## Batas verifikasi

Eksekusi native memakai Linux llvmpipe software renderer. Executable Windows sudah dibuat dan dikemas, tetapi belum dimainkan manual pada Windows/Iris Xe; tidak ada klaim FPS hardware tersebut. Arah visual mengikuti referensi, sementara kesetaraan seluruh model, animasi, komposisi dan kualitas produksi DreadOut belum dibuktikan. Pengguna tetap menentukan penerimaan kreatif melalui playtest.

Ringkasan pemeriksaan mesin: `dreadout-look-final-validation.json`.
