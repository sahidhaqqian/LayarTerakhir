# Melati: rumah bervariasi, kos di sisi jalan, dinding halaman kembali

Revisi desain 18 Oktober 2026. Kos dipindahkan ke petak kiri (offset x=−16 m), dengan akses pendek ke jalan utama x=0. Bangunan kos, lantai dua, tangga, fasilitas, pintu, motor dan titik spawn bergerak bersama; source scene kamar awal dan Atmaja tetap identik dengan baseline `6ffb227`.

## Lingkungan dan aset

- 45 rumah dari 12 geometri Kenney berbeda: a/b/d/f/h/k/m/p/i/l/n/q. Rumah kotak generik lama diganti model unduhan; bukan sekadar mengganti warna pada satu bentuk.
- Sumber resmi: [Kenney City Kit (Suburban) 2.0](https://kenney.nl/assets/city-kit-suburban), CC0-1.0. ZIP resmi diunduh dan lisensi dalam arsip diperiksa. SHA-256 arsip: `5869c35cf30b1c87bdb2d197b6d325eebadd2ef08ea27f04797e8e08d77a9a39`.
- Delapan model adaptasi yang sebelumnya tersimpan digunakan kembali, ditambah empat model terpilih i/l/n/q dari unduhan baru. Penyesuaian meter, UV dan daerah bahan tercatat dalam [source receipt](melati-house-kit-source.json), [lisensi arsip](melati-house-kit-CC0.txt), [authoring receipt](melati-house-authoring.json). FBX tambahan masing-masing 800/1.310/1.612/1.140 triangle.
- Bahan plester, genteng, kayu, beton dan kaca memakai tekstur PBR yang sudah dikreditkan. Rumah latar ini berupa eksterior; revisi tidak menambahkan akses interior rumah warga.
- Petak yang bertabrakan dengan kos dikosongkan; delapan rumah tambahan mengisi bagian utara. Jalan utama menerus dari z=−106 hingga 100 m, tetap terbuka bagi pejalan kaki. Jalur samping kos tersambung tanpa menumpuk bidang aspal.

## Dinding dan perjalanan

Dinding koridor/sel kamar ternyata masih ada. Dinding halaman dan return di sisi gerbang sebelumnya ikut dihapus bersama filter `YardBoundary`/`KosGateBoundary` pada revisi membuka kawasan. Revisi ini memulihkan empat bidang masonry khusus halaman kos, dengan renderer dan collider aktif. Bukaan dua daun gerbang tetap bebas; tidak membuat tembok penutup jalan warga.

`KosCorridorDirector.BuildingPoint` memetakan koordinat lokal gedung ke petak baru. Checkpoint lantai dua/halaman memakai anchor yang ikut dipindah. Motor tetap di kos, keluar melalui gerbang, menyusuri jalur akses pendek lalu belok ke jalan utama dan langsung menuju Atmaja. Fixtures navigasi mengikuti frame gedung, sedangkan waypoint perumahan tetap dalam koordinat dunia.

## Validasi dan paket

- Syntax Python lulus; **17/17 tes Python** lulus.
- Suite Unity awal **87/87 lulus**, 0 gagal/dilewati. [XML](melati-street-tests.xml), [invokasi](melati-street-test-invocation.json).
- Review visual pertama menemukan orientasi rumah tertimpa yaw. Rotasi koreksi FBX kini dipertahankan; fondasi menghubungkan rumah ke tanah. Setelah koreksi, **9/9 tes regresi DesignerRevisionTests** lulus pada versi final, termasuk validasi fisik jalan, gerbang, fondasi dan sumbu impor. [XML final](melati-street-final-focused-tests.xml), [invokasi final](melati-street-final-test-invocation.json).
- Screenshot sebelum koreksi ditandai **tidak diterima**, bukan bukti akhir. [Perbandingan visual](Visuals/Melati-Street-Orientation-Diagnostic/review.json). Dua screenshot final tata jalan/dinding dan sumbu rumah telah diperiksa langsung.
- Audit standalone final rute kos → perumahan → warung → motor → Atmaja: **59 screenshot, completed=true**, exit 0. [Receipt](Visuals/Melati-Street-Opening-Final/player-audit.json), [invokasi](melati-street-opening-invocation.json).
- Audit pengendara: **4 screenshot, completed=true**, exit 0; mount, head/grip/seat, bebas halangan kamera, melewati gerbang, perjalanan langsung dan kedatangan berdiri lulus. [Receipt](Visuals/Melati-Street-Rider/player-audit.json), [invokasi](melati-street-rider-invocation.json).
- Snapshot **2.407 file sumber** diverifikasi kembali sebelum packaging. [Snapshot](melati-street-prolog-presentation-build-sources.json).
- Build Windows64 Mono Development: **Succeeded**, **0 error**, 1 warning, Unity exit 0. Paket memverifikasi PE AMD64, assembly, marker scene, file dan CRC ZIP. Tambahan pemeriksaan level2 memastikan semua 12 nama model rumah, fondasi, dinding halaman dan akses samping ada di paket.
- Tidak ada `.meta` lama yang berubah, GUID duplikat, aset tanpa metadata atau metadata yatim. Source scene kamar awal dan Atmaja tetap identik. [Pemeriksaan akhir](melati-street-final-validation.json).
- Graphify diperbarui setelah perubahan kode terakhir: **145 file kode, 0 aset**, gate freshness lulus. Empat hasil portabel disertakan commit.
- **ZIP baru diunggah ke R2 dan seluruh objek dibaca ulang**: SHA-256/ukuran cocok dengan lokal. Rclone sempat mendapat 501, retry berhasil dan readback penuh lulus. [Receipt R2](r2-windows-build-upload.json).
- Objek remote: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`; ukuran **197,800,161 byte**.
- SHA-256: `0e2622dbd7c474d5f75b0ad5986cbac81496efa662e554a7b3d20654372997c5`.
- Verifikasi upload: `2026-10-18T15:10:46.906165+00:00`.
- Audit runtime memakai LinuxPlayer/llvmpipe, 640×360 untuk rute awal dan 960×540 untuk rider. Belum ada playtest manual Windows atau benchmark FPS perangkat target. Audit prolog penuh tidak diulang pada revisi lingkungan ini; scene Atmaja tidak berubah dan pemeriksaan cerita tetap ada dalam suite 87 tes awal.

[Validasi geometri](melati-street-revision-validation.json) memeriksa 45 rumah/12 model, kos di luar jalan utama, empat dinding dengan collision, bukaan gerbang, 146 sampel jalan berlapis permukaan dan kapsul bebas hambatan, bahan rumah serta missing script. Tes `ResidentialStreetAndKosMasonryRemainTraversable` menjalankan pemeriksaan ini sebagai regresi EditMode.

Perubahan hanya untuk revisi ini yang di-stage; penghapusan `deepseek.md` dan catatan `restruktur.md` milik pengguna tidak dimasukkan. Branch pengiriman `main`, tanpa force-push. Hash commit/push aktual disampaikan pada pesan penyerahan.
