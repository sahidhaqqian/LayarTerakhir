# Revisi lingkungan Phase 1 setelah playtest

23 Oktober 2026. Build review awal ditolak desainer karena bangunan dan presentasi lingkungan tidak layak. Revisi ini merupakan pekerjaan nyata pada babak yang sama; bukan perluasan cerita atau pernyataan Phase 1 selesai.

## Perubahan yang diterapkan

- Mengunduh **Modular Concrete Interior** Axel Kulomaa / AXLplosion, **Breakable Doors and Windows** loafbrr_1, tujuh perabot/model dan lima permukaan PBR Poly Haven, serta safety chain Innerscene. Sumber primer, lisensi CC0 dan checksum dicatat di `phase1-environment-sources.json`, `phase1-stair-import.json` dan `phase1-chain-source.json`.
- Mengganti visual dinding, lantai, plafon dan slab dari blok yang diregangkan menjadi pengulangan panel sumber. Topologi/UV model unduhan dipertahankan. Konversi sentimeter FBX dibedakan dari pengulangan tekstur dalam meter; tekstur tidak lagi diulang 100 kali terlalu rapat.
- Mengganti material bangunan Prolog dengan set PBR khusus Phase 1; Kenney berpalet warna dan modul Unfinished Buildings tidak dipakai lagi. Perabot dan housing lampu Prolog juga diganti. Karakter desainer, motor yang dibawa dalam campaign, sistem HP/pemain/UI dan routing audio bersama tetap dipakai untuk kontinuitas.
- Mengganti pintu/jendela dengan model bertekstur dan variasi kaca pecah; menambahkan ritme kolom serta tepi slab fasad. Slab mempunyai bidang tepi, tidak hanya bagian atas/bawah.
- Melengkapi struktur fisik L3 di sekitar courtyard sehingga L4 tidak menggantung di atas storey kosong. Rute investigasi L3 tetap lorong tersegel 40 m sesuai spesifikasi.
- Mempertahankan rotasi sumbu impor FBX dan memisahkan fitting visual dari skala parent; ukuran renderer/collider prop harus cocok, termasuk rantai pada parent yang berskala. Aura AR mengikuti skala keris yang sebenarnya.
- Mengganti visual tangga dengan dua flight dari kit baru, mempertahankan collision ramp tertutup dan bukaan slab yang sudah diuji. Tangga tidak membuat sistem navigasi baru.
- Menata pos satpam, pompa, galeri hunian, arsip, K403 dan dapur memakai perabot unduhan. Posisi generator diturunkan ke lantai; cutter diletakkan di atas troli.
- Menghapus skybox siang bawaan dari scene malam; menjaga area lampu dapat dibaca tanpa menambah camera shake atau mengubah mekanik HP.
- Mengoreksi tangkapan kontak Genderuwo/Kuntilanak agar garis pandang terhalang dinding; ambush kamar mandi kini dapat benar-benar bergerak mengejar saat mendeteksi pemain.

Tidak ada model, gambar atau tekstur baru yang digambar/dibuat sendiri. Penempatan scene, collider, material Unity dan konversi channel PBR adalah pekerjaan integrasi aset sumber. Aset/kode DreadOut tidak disalin.

## Bukti dan batas verifikasi

- `phase1-scene-validation.json`: pemeriksaan script/material/model yang benar-benar tersimpan dalam scene.
- `phase1-regression-tests.xml`: hasil suite penuh dan rerun babak (`phase1-targeted-regression-tests.xml`), termasuk kasus dinding tipis, panel/skala UV, ukuran prop/collider dan reach keris. Jumlah dan hasil dibaca dari XML, bukan dari daftar QA desain.
- `Visuals/Phase1-Sourced-Grasp`: capture standalone Linux dengan renderer perangkat lunak. Sudut kamera ditempatkan secara eksplisit untuk inspeksi visual; bukan bukti navigasi, luring, FPS Windows atau seluruh puzzle lulus.
- `PHASE1_IMPLEMENTATION_REVIEW.md` dan `phase1-windows-playtest.md`: cakupan mekanik serta penerimaan yang belum selesai.

Mutu artistik akhir tetap membutuhkan review desainer. Native smoke gameplay sebelumnya belum menyelesaikan boss sampai CP7; kelulusan capture visual tidak mengubah status tersebut. Acting dialog Indonesia, foto arsip penghuni, luring alami dan seluruh 74 skenario QA masih perlu pekerjaan/validasi. Tidak ada klaim setara DreadOut/Outlast atau target GPU telah tercapai.

## Arahan lanjutan desainer

Sebanyak 30 model prop tambahan yang berbeda (bukan instance/tekstur/modul bangunan) diunduh dan ditempatkan menurut fungsi ruang. Semua ID dibandingkan dengan folder sumber prop Prolog; sumber yang sama dikeluarkan dari tambahan ini. Penempatan memakai ukuran prop asli, penopang lantai/meja/dinding/plafon, dan uji clearance koridor. Baterai awal transisi babak kini 100% sekali sesuai revisi desainer. Framing kamera RMB mengikuti referensi R2; catatan teknis dan batas sumber ada di `phase1-phone-reference.md`.
