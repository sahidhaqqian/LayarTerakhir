# Revisi playtest 18 Oktober 2026

Status pengiriman: **build dan unggahan terverifikasi**. Revisi telah diuji otomatis; penerimaan visual serta runtime Windows tetap memerlukan playtest pemilik desain.

## Perubahan implementasi

- Kompleks Melati tersambung dengan halaman kos dan bisa dijelajahi berjalan kaki sebelum keberangkatan. Penghalang lama di gerbang, ujung jalan, dan persimpangan dihapus. Motor tetap di kos; keberangkatan motor memuat rumah Atmaja langsung.
- Lingkungan berisi 43 rumah, teras, atap, jendela, lampu jalan, dan hujan. Permukaan jalan dipotong pada sambungan dan persimpangan agar tidak saling tumpang tindih.
- Bu Sri hadir sebagai karakter beranimasi di warung. Interaksi pada tubuh penjual membuka belanja dan percakapan tentang Damar serta arah rumah Atmaja. Dialog tambahan masih berupa teks/subtitel; rekaman manusia belum tersedia.
- Mesh kulit leher Raka dipulihkan. Celana dan jaket dipisahkan menurut komponen pakaian yang tersambung, bukan dipotong melintang. Kaos memiliki kelim yang menutupi pinggang saat menekuk. Pemecahan IK menjaga sendi tangan/kaki tetap tersambung; arah telapak pada fase pijakan dan tempo gerak disesuaikan.
- Kasur kamar Bima dibuat mendatar di atas rangka ranjang; bantal, selimut, meja belajar, lemari, dan rak ditata ulang. Tangga Atmaja yang sudah disetujui dilindungi oleh pemeriksaan signature builder.
- Pencahayaan kompleks dan rumah Atmaja, ambient, antialiasing, dan profil pascaproses disesuaikan. Scene kamar kos awal tetap dilindungi. Kesetaraan visual dengan Outlast belum dinyatakan atau diukur.
- Setelah rekaman handycam, hantu menavigasi ke posisi pemain di ruang bawah tanah. Di lantai atas ia mengikuti jalur tangga yang tersedia. Pemulihan sesudah Blitz memberi waktu untuk keluar jendela dan merunduk di bawah dahan; batas tindakan keluar jendela tetap empat detik.

## Bukti dan batas verifikasi

- `Visuals/Playtest-Repair-Opening/player-audit.json`: pembukaan hingga checkpoint tangga, berjalan kaki ke kompleks, interaksi Bu Sri, pembelian, dan keberangkatan motor langsung.
- `Visuals/Playtest-Repair-Rider/player-audit.json`: duduk di motor, perjalanan, dan berdiri setelah tiba.
- `Visuals/Playtest-Repair-Prolog/player-audit.json`: sumber hasil alur penuh lantai dua hingga ending; periksa nilai `completed`, jangan menggunakan hasil diagnosis pengejaran sebagai pengganti.
- `rigged-character-authoring.json`: pemeriksaan batas leher terbuka dan jarak sambungan sendi pada pose yang diperiksa.
- `melati-road-surface-validation.json`: pemeriksaan bidang permukaan jalan tanpa tumpang tindih sebidang.
- Player otomatis berjalan pada Linux dengan renderer llvmpipe; ini bukan pengujian runtime Windows, penerimaan visual manual, atau bukti target FPS.

## Hasil akhir

- Unity: **86/86 lulus**, hasil segar di `playtest-repair-suite-tests.xml`. Log Unity menyatakan `Test run completed. Exiting with code 0 (Ok)`. Launcher terminal terputus sebelum mengambil status proses; receipt mencatat keterbatasan itu, tanpa mengarang status OS.
- Python: **17/17 lulus** (`Tools/tests`).
- Pembukaan: **57** tangkapan layar; rider: **4**; lanjutan penuh: **30**, diulang **30** lagi dengan ending dan Continue sama-sama berhasil. Percobaan alur penuh sebelumnya pernah tertangkap pada awal pengejaran; dua pengulangan sumber akhir tidak mengulang kegagalan tersebut. Ini bukan jaminan bebas bug atau validasi seluruh variasi pacing pemain.
- Validasi scene Atmaja dan build Windows: berhasil; Unity keluar dengan kode **0**. Paket PE AMD64, kelengkapan file, identitas sumber, serta CRC seluruh ZIP diverifikasi oleh `Tools/package_windows_build.py`.
- ZIP: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`, **197,196,378 byte**.
- SHA-256: `625b5a30bf4d918286d69832c1e7b07a65a122a1bb4765a999bf2b64c1d0b055`.
- R2: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`. Unggahan selesai dan **seluruh objek remote dibaca kembali**, dengan ukuran dan SHA-256 sama persis. Receipt: `r2-windows-build-upload.json`, diverifikasi **2026-10-18T12:10:51.334180+00:00**.
- Graphify diperbarui setelah perubahan kode terakhir: **141 file kode, 0 aset**, kemudian lolos `--check`.

Belum diverifikasi: playtest manual pada Windows, target FPS perangkat pemain, penerimaan estetika setara Outlast, dan rekaman suara manusia untuk dialog Bu Sri.
