# Referensi visual DreadOut dan penerapan di Layar Terakhir

Acuan: **DreadOut (2014)** dari Digital Happiness, bukan DreadOut 2 atau Remastered. Referensi diambil dari [galeri resmi Steam](https://store.steampowered.com/app/269790/DreadOut/). Penjelasan di bawah adalah pembacaan visual developer atas screenshot, bukan spesifikasi rendering dari pembuat DreadOut.

| Referensi resmi | Pengamatan |
| --- | --- |
| [Screenshot 1](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/269790/ss_d926a87bf19b4de0bb023c855bd77c7c60419e82.1920x1080.jpg) | Sumber cahaya HP dekat wajah dan tekstur kayu kusam. |
| [Screenshot 2](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/269790/ss_210be661b91998183c65ea9df7e960442d68d84e.1920x1080.jpg) | Cat mengelupas, warna dingin yang tertahan, bayangan tetap bertekstur. |
| [Screenshot 6](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/269790/ss_5b90da59f115b062ed5b561728b0558ad39c05b5.1920x1080.jpg) | Koridor berlapis kedalaman: sisi dekat terbaca, ujung gelap; HP menerangi area yang dibidik. |
| [Screenshot 10](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/269790/ss_fc8f7becf69292779af0da8fabc51c496f8f006f.1920x1080.jpg) | Detail permukaan pintu dan plester, warna pakaian tidak menyala sendiri. |
| [Screenshot 11](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/269790/ss_d37d128f5404b55e356f8675391c4ef880223423.1920x1080.jpg) | Kayu dan cahaya jendela dingin dengan jatuh cahaya yang lembut. |

Sebelas gambar resmi diperiksa secara lokal dalam `Builds/ArtSources/DreadOutReferences/` (diabaikan Git). Gambar referensi tidak menjadi tekstur, model, UI, aset build, atau input Graphify.

## Implementasi

- `DreadoutVisualRevision.Apply` mengubah presentasi ketiga scene secara incremental; fingerprint seluruh collider diperiksa sebelum/sesudah. Tidak memanggil builder yang meregenerasi layout. Pemeriksaan opening memakai identitas struktur dari commit historis `30d9e7e`; hanya profil Volume, pengaturan AA/dithering/NaN kamera, dan dua field default implisit yang boleh berbeda. Transform, collider, referensi serta data gameplay tetap dilindungi. Packaging Windows memakai pemeriksaan identitas yang sama melalui `Tools/opening_presentation_identity.py`; hash historis tidak diganti dengan hash scene baru.
- Pipeline URP: grading HDR, buffer HDR 64 bit, LUT internal 64, Forward+, shadow distance 38 m dan atlas 2048. Kamera eksplorasi memakai HDR, SMAA High dan dithering.
- Profil terpisah Kos/Melati/Atmaja: warna agak kusam, tonemapping Neutral, grain halus, vignette lembut dan bloom terbatas. Detail gelap dipertahankan; tidak menambahkan blur pada objek interaksi.
- Shader langit memakai noise awan beberapa skala dan bulan kecil berawan, menggantikan gelombang sinus berulang.
- Melati memakai aspal PBR dengan diffuse/normal dan mask roughness/AO. Cat rumah lebih terbaca, tanah tidak lagi memakai beton, lampu jalan diarahkan ke jalan dan lampu teras memiliki diffuser terlihat. Jendela terang/gelap bervariasi secara deterministik. Preview HDR pertama ditolak karena jalan/langit terlalu terang; cahaya bulan, tint aspal, fog dan radiansi langit diturunkan setelah inspeksi gambar. Pantulan lingkungan malam dibatasi menjadi 0.18 agar aspal tidak tampak seperti permukaan biru terang di seluruh jalan.
- HP memakai pusat sorot lebih fokus dengan tepi lembut, warna LED dingin, spill dekat berkurang, dan shutter tetap berkedip. Cahaya tambahan pada avatar diturunkan agar karakter menyatu dengan lingkungan.

## Sumber material

[Asphalt 02](https://polyhaven.com/a/asphalt_02) oleh Rob Tuytel, [CC0](https://polyhaven.com/license). Map 1K diverifikasi dengan checksum dari API Poly Haven; `Tools/fetch_dreadout_look_assets.py` menghasilkan mask URP R=0, G=AO, B=0, A=1−0.57×roughness. Receipt: `Audit/dreadout-look-asset-sources.json`.

## Cara menilai hasil

Gambar `Audit/Visuals/Dreadout-Look-Before/` dan `Dreadout-Look-After/` memakai delapan posisi kamera yang sama, diambil dari framebuffer standalone Linux. Ini fixture pose dengan waktu cerita dibekukan, bukan bukti traversal atau FPS. Audit perjalanan normal dan prolog penuh dicatat terpisah.

Arah visual mengikuti referensi. Kesetaraan seluruh model, animasi, komposisi dan kualitas produksi DreadOut belum dibuktikan oleh perubahan pencahayaan ini; tidak ada klaim hasil identik atau performa Windows dari screenshot software renderer.
