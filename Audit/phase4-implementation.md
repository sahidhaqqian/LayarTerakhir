# Phase 4 — implementasi Bara di Karang Setra

Tanggal: 25 Oktober 2026. Lingkup: lima adegan Phase 4, dari persiapan Kos sampai boundary fisik gerbang Pasar Setan. Status validasi akhir di bagian bawah harus dibaca bersama hasil tes; daftar fitur bukan bukti seluruh 180 QA lulus.

## Perubahan yang diterapkan

- Adapter `Phase4Campaign` menggunakan `CampaignState`, inventori instance, transaksi checkpoint dan jurnal yang sudah ada. Schema save naik dari 4 ke 5 melalui rantai migrasi sebelumnya. Tidak ada save/event/inventori/AR paralel.
- Kos sore memakai identitas Kos yang disetujui: forum 1997, upgrade wajib gratis, toko opsional, pengisian baterai sekali, pilihan istirahat eksplisit dan keberangkatan CP1. Sanity carry-over tidak direset diam-diam.
- Scene `Babak4_KarangSetra`: hutan bambu tiga simpang, petunjuk fisik UV versus chat penyesat, Pendopo, makam Utara, gubuk Timur, sumur Barat, sektor Selatan, dan tangga portal ke vestibule bawah tanah. Pendopo tetap terpicu bila pemain menemukan jalan di luar rute tiga simpang.
- Investigasi bebas urutan: Thermo +66°C stabil 2 detik, foto AR altar, rekaman mantra EVP 6 detik, tiga pusaka terpisah, dokumen Damar/Kitab, konfirmasi jurnal dan persembahan.
- Cermin Saksi opsional: ketukan EVP, mata tertutup UV, roda dan laci, lalu pickup atomik. Tidak menjadi syarat boss.
- Sanity, kejernihan kemenyan, petunjuk palsu, penglihatan api, bahaya hutan berperingatan, persembunyian fisik dan AI Banaspati memakai navigasi/LOS. Shrine, dinamo dan air gratis tersedia di semua sektor untuk mencegah ketergantungan pada pembelian.
- Ritual empat penjuru: N/E/W bebas urutan dengan tiga Blitz, Selatan memakai air, kemudian satu Blitz inti di Pendopo. Sektor berhasil tersimpan pada CP6; CP7 memberi tepat satu segel lokasi. Air aktual dan layout obor tetap tersimpan; refill tidak terbatas.
- Epilog donasi terikat, viewer/error wallet naratif, Damar melalui subtitle, konfirmasi CP8 dan boundary pada malam yang sama. Babak 5 penuh belum dibuat; Continue tidak diarahkan ke scene yang tidak tersedia.
- Kamera dan input memakai controller/HP sebelumnya: RMB hold, OTS/FP, Selfie, AR/UV/Thermo/EVP, powerbank, remap dan fitur pengurangan glitch. Sistem Phase 1–3 tetap dipertahankan.

## Aset dan presentasi

116 sumber model baru ditempatkan dengan tujuan yang tercatat, terdiri dari 107 model Poly Haven dan 9 sumber hero. Setelah revisi scene terakhir: 632 penempatan. Jumlah ini tidak menghitung duplikasi, saluran tekstur, material, Kos, Raka, HP atau UI bersama. Tujuh surface baru dan sumber audio/glyph/partikel tambahan dicatat terpisah.

Sumber, pembuat, lisensi, URL dan operasi konversi ada di `phase4-asset-credits.md`, `phase4-audio-credits.md` dan manifest JSON. Model/tekstur diunduh; tidak ada aset proprietary DreadOut yang disalin dan tidak ada model/tekstur/animasi baru hasil menggambar atau generasi AI. Pemilihan bagian geometri sumber, konversi, pengurangan poligon dan pemasangan material adalah turunan berlisensi, bukan karya pemodelan baru.

Perbaikan presentasi meliputi pemetaan material per sumber/mesh, konversi channel PBR ke URP, opacity daun, pencahayaan malam eksplisit, lampu praktis, papan petunjuk berukuran fisik, kabut dan color grading. Tangga portal memakai unduhan tangga batu lurus baru beserta collision mesh-nya. LOD sumber dipertahankan; collider tidak digandakan pada setiap tingkat LOD.

## Penyesuaian rancangan

Register REPO01–REPO16 di `Phase4.MD` menjelaskan keputusan implementasi terhadap kontrak global. Yang utama: schema 5, Sanity carry-over, kemenyan pulih 100 setelah aksi, kecepatan AI disesuaikan dengan controller aktual, satu Kendi dengan air aktual, empat Blitz total, checkpoint rolling, serta boundary P5 sementara yang benar-benar dapat dimasuki di scene P4. Angka cerita 1997, tiga segel sebelumnya dan donasi terikat dipertahankan.

## Validasi dan batas bukti

Pembuatan scene terbaru: `phase4-scene-generation-run.json`, exit code 0. Verifikasi sumber/hasil konversi: `phase4-asset-verification.json`, 116 model / 632 penempatan / 1.465 pemeriksaan hash, tanpa kegagalan. Ini memverifikasi provenance dan keberadaan aset, bukan kualitas artistik.

Tes scene terbaru: **34/34 PASS**, invokasi exit0, `phase4-scene-tests.xml` dan `phase4-scene-tests-run.json` (25 Oktober2026 12:08–12:09 UTC). Cakupannya meliputi route navmesh, portal setelah carving diperbarui, raycast target AR/Thermo/12obor, ukuran properti tipis, area perlindungan dan tiga dudukan pusaka. Ini belum membuktikan keseluruhan encounter atau seluruh180QA. Hasil tes lama dan render awal tidak digunakan sebagai penerimaan untuk scene terbaru. Rincian per skenario ada di `phase4-qa-matrix.md`; NOT_RUN tetap berarti belum dibuktikan.

Belum dapat diklaim selesai: akting VO manusia pada 12 cue di `Recording/phase4-dialogue-recording.csv`, penerimaan visual/pacing oleh desainer, pengukuran FPS pada perangkat Windows target, dan playtest manusia seluruh 180 QA. Subtitle serta efek/musik berlisensi tersedia; itu tidak setara dengan rekaman dialog manusia final. Kesetaraan kualitas dengan DreadOut adalah target kreatif yang memerlukan review build.

## Menjalankan review

Pakai `Review-Phase4.cmd` pada paket Windows, lalu Continue. Save review terpisah dari kampanye utama. Instruksi, kontrol dan rute ada di `phase4-windows-review.md`.

Pengujian editor di mesin Linux ini dijalankan melalui `Tools/run_unity_isolated.py`, di layanan systemd terpisah dengan batas memori 5 GiB. Kernel sebelumnya menghentikan Unity karena kehabisan RAM di scope tmux; hasil invokasi yang terputus tidak dianggap sukses. Gunakan `-nographics` untuk pembuatan scene/tes yang tidak memerlukan render. Jangan menjalankan editor dan player berat bersamaan.

Perbaikan dari directed run: navmesh mengitari dinamo/properti alih-alih menuntut langkah75cm; permukaan gubuk memakai empat anak tangga sesuai controller30cm. Radius EVP3m mengikuti titik berdiri fisik di depan sumur dan memberi petunjuk mendekat. Perbaikan render: sisi slab menghadap atas, pengulangan material mengikuti UV sumber, faktor metallic/roughness glTF ikut dipacking, genteng memakai surface unduhan baru, serta hutan memiliki beberapa lapisan bambu dan tumbuhan bawah. Motor identitas kampanye dipintu masuk hutan tidak dihitung sebagai aset baru.

Pemeriksaan tambahan memisahkan seluruh12 titik siram dari ward penolak ancaman. Jarak optical/Blitz memakai mata pemain, sedangkan viewport/LOS memakai lensa HP, agar OTS dan FP mendapat jangkauan fisik yang sama. Thermo tidak menjadi oracle kedua untuk obor.

## Hasil validasi rute dan regresi

- **280/280 PASS**, invokasi exit0, regresi Phase1/2/3/4 + player feel + foundation. Bukti `phase4-regression.xml` dan `phase4-regression-run.json`,12:59–13:02UTC.
- **128 pemeriksaan rute native PASS**, invokasi exit0,12:31–12:35UTC. Sekitar874,75meter gerak CharacterController; investigasiW→E→N, ritualW→N→E, Selatan, satu core, segel4, CP8 dan penurunan tangga fisik. AI tetap aktif, tanpa pembelian, tanpa memindah musuh atau menulis hit palsu. Driver memancing ancaman melalui gerak fisik dan menunggu inti benar-benar terbingkai.
- Satu reload **scene CP6** sungguh dijalankan setelah Barat; receiptW dan Cermin Saksi dipulihkan. Ini lebih luas dari hanya membaca data, tetapi tidak membuktikan seluruh variasi process-kill/kematian/seed.
- Rute memakai Linux standalone headless; bukan bukti render, emosi VO, benchmark IrisXe atau penerimaan Windows. Native result/run dan final snapshot fixture disimpan terpisah. Hasil render dan build Windows dicatat di bawah; penerimaan manusia tetap terpisah.

## Perbaikan berdasarkan hasil uji

| Temuan teramati | Perubahan | Bukti setelah revisi |
|---|---|---|
| Unity kehabisan RAM dan menghentikan scope tmux | Layanan systemd terpisah, batas5GiB; scene/build dijalankan terpisah | Invokasi editor terbaru selesai exit0 |
| Lantai menghadap bawah; UV dikalikan hingga ribuan pengulangan | Orientasi slab dan skala material disesuaikan terhadap UV sumber | Tes geometri/UV dan render inspeksi |
| Daun berubah metallic karena faktor glTF tidak ikut dipacking | Faktor material sumber disertakan dalam mask URP | Verifikasi sumber/hasil konversi dan render |
| Keris mendatar membesar sekitar8m dan menutup altar | Prop tipis memakai panjang sebagai ukuran | Raycast scene dan foto AR native berhasil |
| Titik rekam EVP terlalu pendek terhadap ruang berdiri | Radius3m dan petunjuk mendekat | EVP6detik + puzzle Cermin native berhasil |
| Navmesh menuntut langkah di atas batas controller | Properti dihindari, gubuk diarahkan ke empat langkah nyata | Route scene dan traversal capsule native |
| Nisan bertumpuk dengan target Thermo | Nisan tambahan disingkirkan dari makam target | Raycast dan bukti+66native berhasil |
| Trigger ritual/meja menutup ruang kerja altar tengah | Batu konfirmasi terpisah dan meja Kitab di samping | Tiga raycast dari stance reachable dan persembahan native berhasil |

Koreksi render terakhir: UV modul slab memakaiU sepanjangZ danV sepanjangX. Pengulangan material sekarang mengikuti sumbu tersebut; uji fisik jalan sempit memverifikasi detail sekitar2meter per pengulangan pada kedua arah, sehingga tidak tertarik20meter/terkompres25cm. Seluruh35tes scene termasuk koreksi ini tercakup pada regresi280/280 terakhir.

## Render terakhir

Sembilan pose Linux standalone dirender denganllvmpipe; invokasi exit0, bukti `phase4-visual-review.json`/`phase4-visual-review-run.json`. [Hutan](Captures/Phase4/01-forest.png), [Pendopo](Captures/Phase4/02-pendopo.png), [Gubuk](Captures/Phase4/04-gubuk.png), [Tangga](Captures/Phase4/08-stairs-posed.png), [Simbol UV](Captures/Phase4/09-uv-torches-posed.png). Pose UV mengaktifkan tampilan obor untuk inspeksi grafis, bukan bukti ritus dimainkan; bukti gameplay tetap rute native terpisah. Jalan tidak lagi memakai UV sumbu terbalik, dan glyph asli/palsu terlihat di feed. Ini bukan skor kesetaraan artistik DreadOut atau benchmark GPU Windows.

## Build Windows untuk review

Unity6000.3.26f1 menyelesaikan Windows64 Mono Development pada25 Oktober2026,13:09:34–13:15:33UTC: **Succeeded,0 error,0 warning**, tujuh scene termasuk `Babak4_KarangSetra`. Proses terisolasi berakhir exit0 pada13:15:39UTC. Bukti: `phase4-windows-build-run.json` dan `phase4-windows-build-report.json`.

Sumber sebelum build diverifikasi kembali:10.710 file identik dengan snapshot `phase4-build-sources.json`. Satu entri `preloadedAssets` yang ditambahkan otomatis oleh paket Input System selama build dikembalikan ke konfigurasi sumber sebelum build setelah Unity berhenti; kode, scene, aset dan konfigurasi authored tidak direvisi sesudah freeze. Graphify memverifikasi301 file kode,3.949 node,14.307 edge dan0 file aset.

Paket menyertakan launcher `Review-Phase4.cmd`, panduan kontrol/rute, kredit sumber, matriks QA dan daftar12 cue untuk rekaman dialog manusia. Pemeriksaan executable x64, manifest file dan CRC ZIP dicatat dalam `phase4-windows-build-summary.json` setelah pengemasan. Unggahan ZIP dan pembacaan ulang SHA-256 objek R2 dicatat terpisah dalam `r2-windows-build-upload.json` di repository agar receipt tidak menjadi checksum melingkar di dalam ZIP.

Windows build berhasil bukan bukti bahwa player sudah diuji pada GPU Windows. VO manusia, pacing78menit, seluruh variasi180QA dan penerimaan kualitas visual masih memerlukan playtest yang dinyatakan di atas.

## Paket yang diserahkan

ZIP Windows: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`, 2,549,151,641 byte; 347 file, CRC seluruh ZIP lulus, executable/player/runtime Mono terverifikasi PE32+ AMD64. SHA-256: `33d8fc22681af37b37168c454e3c0bf74a7cf4848361065d8194b14df2d9d985`.

Objek `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip` berhasil diunggah dan seluruh isinya dibaca ulang; ukuran serta SHA-256 cocok pada `2026-10-25T13:27:39.221257+00:00`. Receipt: `r2-windows-build-upload.json`. Ini memverifikasi pengiriman paket, bukan penerimaan gameplay Windows. Jalankan `Review-Phase4.cmd`, lalu Continue; save review terpisah dari kampanye utama.
