# Phase 2 — Sesi 1: panduan review build

Lingkup: Kos pukul 14:00 → RSUD Lama Sido Waras pukul 23:50 → seluruh investigasi IGD, Poli 03, basement/kamar mayat, Arsip, Pos Petugas, Inkubator dan Ruang Jaga Suster → pintu Ruang Operasi. Boss, pemasangan ritual, epilog, segel kedua dan perjalanan keluar adalah **Sesi 2 dan belum diterapkan**.

## Membuka jalur review

Ekstrak seluruh ZIP Windows ke satu folder, lalu jalankan **`Review-Phase2.cmd`**. Launcher pengembangan ini memakai slot terpisah `Phase2Review`, dimulai dari fixture sesudah P1, tanpa mengubah campaign normal. Pilih **Continue**. Fixture dimulai dengan saldo belanja nol agar jalur tanpa belanja bisa dicoba; ini bukan pemberian uang/segel pada save produksi. Menjalankan EXE biasa tetap memakai campaign normal. Dari akhir P1, tombol lanjut kini menuju Kos persiapan P2.

Kos memakai scene existing yang sudah diperbesar oleh desainer; ukuran dan tata letaknya tidak dibangun ulang. Buka PC untuk riset dan pemasangan modul gratis. Colokan memberi satu pengisian persiapan **setelah interaksi sadar**. Berangkat tidak otomatis mengisi baterai. Paket Pasar Gaib opsional dan hanya memakai saldo sah.

## Kontrol

- WASD berjalan, Shift lari, Ctrl jongkok; binding mengikuti Pengaturan.
- Tahan **RMB** untuk kamera fisik; lepaskan untuk kembali ke pose sebelumnya. **V** mengganti perspektif, **X** Selfie, **T** ThermoKlenik, **Q** AR, **C** EVP, LMB foto. Lihat label aktual pada HP/pengaturan jika binding diubah.
- **F** layar, **Tab** simpan/keluarkan HP, **E** interaksi. Dinamo harus ditahan E. Menu/jeda dan dokumen menghentikan clock investigasi; aplikasi HP biasa tetap membawa risiko.
- Lastri hanya terlihat pada feed Selfie. Saat menempel, pantau sambil benar-benar bergerak maju; menoleh langsung berbahaya setelah jendela belajar. Alternatif: E masuk persembunyian, F matikan layar, tunggu enam detik, E keluar. Baterai nol tetap bisa memakai persembunyian dan dinamo.

## Jalur yang perlu direview

Buku Jaga + jadwal + Thermo Poli 03 → kode basement. Laporan otopsi memberi kunci L2. C3 memerlukan pembacaan suhu stabil; Gunting dan foto aura AR merupakan dua progres terpisah. Arsip D1 opsional. Di L2, Inkubator dan Ruang Jaga Suster boleh didatangi dalam urutan bebas; kode brankas dapat disimpulkan dari catatan atau gema AR. EVP memerlukan framing sumber yang tepat selama enam detik. Tiga bukti perlu **konfirmasi eksplisit di Jurnal**, selain tiga pusaka. Bukaan AR yang sudah dilewati tetap terbuka dua arah saat baterai nol.

Pintu Ruang Operasi menyimpan batas akhir Sesi 1 dan menampilkan status persiapan; tidak memulai boss atau menyatakan Babak 2 tamat. Pemain masih bisa kembali mencari bukti/rahasia.

## Batas penerimaan kualitas

Model Lastri berasal dari **Merah.vrm milik desainer**, dengan konversi/optimasi dan retarget gerak existing. Lingkungan menggunakan unduhan baru berlisensi; sumber dan atribusi tersedia dalam `phase2-asset-credits.md`. Tidak memakai aset/kode DreadOut.

Rekaman dialog manusia Indonesia untuk Raka, Damar dan kalimat EVP belum tersedia: teks kanon disajikan sebagai subtitle dengan cue nonverbal berlisensi. Model neonatal memakai inkubator kaca unduhan dengan material transparan yang dipertahankan; freezer memakai modul penyimpanan logam unduhan yang diadaptasi. Properti gelang, rantai dan perekam masih memerlukan penilaian visual desainer. Ini belum penerimaan mutu final setara DreadOut/Outlast.

Bukti pengujian dan keterbatasannya dicatat terpisah pada `phase2-session1-implementation.md`. Build berhasil dan directed smoke bukan pengganti seluruh 100 QA, playtest manusia, atau pengukuran performa Windows pada GPU sasaran.
