# Phase 1 — Windows review build

Gunakan `LayarTerakhir.exe` untuk campaign normal. Continue mempertahankan save Prolog yang valid; tombol **Malam berikutnya — Rusunawa** tersedia setelah Prolog selesai. Transisi pertama setelah Prolog mengisi baterai menjadi 100% sekali; Continue/retry mempertahankan snapshot dan tidak memberikan powerbank awal tambahan.

Untuk menilai Rusunawa tanpa mengulang Prolog, jalankan **Review-Phase1.cmd**, lalu **Continue**. Launcher ini secara eksplisit memakai save uji terpisah di `ReviewSaves/Phase1` dalam folder build (100% baterai, tidak ada powerbank awal). Launcher hanya menyalin fixture jika folder belum memiliki save. `LayarTerakhir.exe` biasa tetap memakai lokasi save normal. Fixture ini bukan bukti carry-over campaign dan bukan perubahan aturan New Game.

## Kontrol

- WASD bergerak, Shift lari, Ctrl jongkok, E interaksi; beberapa panel daya perlu **menahan E**.
- Klik kanan ditahan: viewfinder HP; lepaskan untuk kembali. TAB membuka HP/aplikasi, F layar, Q AR, C EVP, LMB foto/Blitz, B powerbank, V perspektif.
- Pada pengejaran Kuntilanak, kamera tetap terkunci ke viewfinder; mundur sambil menjaga sosok dalam bingkai. E mengambil alat/memotong rantai di jangkauan tangan tanpa memutar kamera.

## Jalur uji

1. Periksa log satpam, mulai Live di aplikasi Kamera, temukan ruang pompa di sisi barat. Charger manual di pompa dapat dipakai bila baterai habis.
2. Scan tembok K102 dalam AR. Masuk melalui pintu, belok ke kaki tangga dan naik. Jalur menjadi terbuka dua arah. Hindari Tuyul lewat AR + Blitz atau terima kehilangan daya/item yang tersedia.
3. Kotak 207 memakai kode 644; panel status dan dokumen memberi petunjuk. Hadiah kunci/powerbank hanya satu kali. Periksa arsip penghuni dengan AR untuk rahasia Tari.
4. Gunakan lift barang. Di L3 tersedia panel daya sebelum cutter. Ambil alat sambil memantau Kuntilanak; mundur ke ujung timur, E rantai lalu naik tangga tertutup.
5. Di L4: foto cakaran 402, rekam EVP 7 detik di kamar mandi, foto aura AR tungku dan ambil keris. Gunakan lemari/bilik; layar menyala membocorkan persembunyian. Konfirmasi identitas di Jurnal.
6. Aktifkan altar 404. Pancing tubuh Genderuwo ke tiga titik halogen, gunakan tuas, Blitz ke kepala dalam 5 detik. Blitz 15% / cooldown 6 detik. Panel daya arena hanya aman saat recovery. Kalah memuat ulang CP5 persis, tanpa grant.
7. Setelah segel, periksa kain dan kaset. Transmisi Damar berlangsung di HP; bila daya habis gunakan panel arena lalu coba kembali. Setelah epilog, turun ke L3 lalu lift ke L1 atau L2 (opsional arsip). Motor menampilkan konfirmasi sebelum CP7. Babak 2 belum tersedia.

## Batas penerimaan

Ini build review implementasi, belum visual/audio freeze. Dialog/EVP manusia belum direkam; subtitle bukan pengganti acting final. Durasi 60 menit, kenyamanan input, framing karakter, seluruh 74 QA, GPU Windows, target FPS/1% low dan variasi save perlu playtest Windows. Tes otomatis serta gambar Linux tidak membuktikan semuanya.

Catat checkpoint, baterai, mode kamera, lokasi dan langkah reproduksi saat menemukan masalah. Screenshot development tersedia melalui F12 mengikuti `Screenshot-Guide.txt`.

## Revisi visual setelah penolakan build awal

Bangunan/furnitur/material Phase 1 kini memakai sumber unduhan terpisah. Periksa fasad, galeri, tangga, area pompa, K403 dan dapur; bandingkan dengan `phase1-environment-revision.md`. Minta pencahayaan tetap terbaca saat layar HP padam, periksa benturan perabot dan laporkan celah geometri. Capture visual tersusun tidak membuktikan boss/CP7 atau seluruh 74 QA telah lulus.

## Arahan terbaru

Transisi pertama setelah Prolog mengisi baterai menjadi 100%. Continue/retry harus tetap menggunakan snapshot; fixture review baru juga mulai 100%. Uji tahan RMB dari HP tersimpan maupun dari aplikasi Jurnal/Tas: HP horizontal dua tangan, kamera aktif, dan lepas mengembalikan aplikasi/perspektif sebelumnya. Periksa bahwa kamera hiding dan kunci kamera Kuntilanak tetap berlaku.

Tambahan 30 model unduhan mempunyai sumber dan penempatan terpisah: `phase1-additional-prop-sources.json` dan `phase1-additional-prop-placement.json`. Periksa koridor, pintu, tangga, keris, titik charging dan bilik sembunyi.
