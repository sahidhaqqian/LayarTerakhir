# Revisi presentasi kamera tahan RMB

Permintaan desainer: perbaiki hasil percobaan pegangan kamera yang terlihat jelek. Scope hanya presentasi kamera/HP dan regresi input; sistem gameplay yang ada tetap dipakai.

## Sebelum → sesudah

- **Ukuran fisik:** formula memakai tinggi 165 mm lalu meregangkannya mengikuti aspek landscape, sehingga lebar menjadi sekitar 314 mm. Landscape kini lebar 165 mm dan tinggi 86,8 mm. Bingkai menjaga aspek pada rasio layar berbeda.
- **Pegangan:** origin sumber sebelumnya berpusat di tangan yang digeser dari ukuran portrait. Kini origin setiap grasp adalah tepi HP. Tangan kiri dicerminkan; forearm masuk dari bawah. Pegangan FP memakai rig, mesh tangan, cuff dan material karakter Raka CC0 yang sudah ada. Fitter geometri yang sudah ada menyesuaikan kontak lima jari pada handset horizontal: ibu jari menahan sisi pendek, empat jari menopang belakang. Sampling permukaan segitiga ikut memeriksa penetrasi serta ruang di depan gambar kamera, sehingga poligon besar tidak lolos hanya karena vertex-nya berjauhan. Topology dan UV sumber dipertahankan; tidak membuat artwork baru atau menyalin aset DreadOut.
- **Transisi:** HP tersimpan kini langsung diangkat dalam pose horizontal, dengan gerak turun/naik dan roll ringan, tanpa panel portrait yang muncul lalu melebar. Menu yang sudah terbuka tetap dikembalikan ketika RMB dilepas. Kamera terkunci karena scripted encounter tetap menghormati pemilik state lama.
- **Gambar:** vignette merah tidak aktif terus saat tidak ada ancaman. Tangan dan bingkai menggunakan ukuran dan proyeksi kamera yang sama. Kamera foreground memakai FOV 28° dan pemetaan viewport ternormalisasi, dengan jarak handset sekitar 30 cm; FOV 70° kamera gameplay tetap terpisah. Pencahayaan grasp dipisahkan dari lampu dunia/sorot HP agar albedo tidak memutih. Tidak menambah shake. Notifikasi dunia/save dipindahkan ke atas selama membidik agar tidak menutup gambar HP; isinya tetap muncul dan layout dikembalikan ketika kamera diturunkan. Aplikasi portrait tetap memakai grasp portrait yang sudah ada.

## Verifikasi

Verifikasi akhir dicatat melalui XML regresi dan capture native yang terpisah. Toleransi impor geometri: penetrasi/screen clearance <1,5 mm dan kontak permukaan lima jari <4 mm. Material prefab wajib mengacu ke kulit dan pakaian Raka; renderer disimpan sebagai binding eksplisit agar ekspor ulang FBX tidak meninggalkan override material yang rusak. Capture Linux/software GPU bukan uji runtime Windows atau bukti target FPS.

## Batas

Pose ini tetap pose FP baked dari rig karakter yang sudah ada, bukan sistem IK tangan dinamis baru. Penerimaan artistik dan kenyamanan input tetap perlu review desainer. Tidak mengklaim semua animasi, seluruh Phase 1, maupun seluruh 74 QA desain lulus.

## Hasil build review

- Regresi terkait: 61 kasus unik lulus melalui dua run (61/61 awal; tujuh kasus terdampak diulang setelah perubahan terakhir, 7/7). Ini bukan satu suite penuh baru.
- Rerun mencakup kontak/material/clearance mesh impor, hold/release RMB, restore perspektif dan aplikasi, AR + baterai nol/checkpoint, evidence dari lens dan ritual tiga foto.
- `Visuals/Phone-Camera-Clothed-Final`: sepuluh capture standalone Linux; hanya pemeriksaan RMB memakai input produksi. Delapan sudut lingkungan disusun untuk inspeksi, bukan penerimaan rute.
- Performa GPU/kenyamanan Windows tetap perlu playtest desainer. Grafik keseluruhan Phase 1 dan 74 QA tidak dinyatakan selesai oleh revisi ini.
