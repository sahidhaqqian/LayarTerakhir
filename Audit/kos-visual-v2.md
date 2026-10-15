Kos visual v2 — hasil aktual, 15 Oktober 2026 UTC

Scene furnished `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity` diperbaiki secara inkremental. Ukuran collider ruangan tetap **6 × 4,5 meter**, tinggi 3,1 meter. Model furnitur yang sudah ada dipertahankan; tidak ada pembangunan ulang greybox, perubahan cerita/GDD, konten Rumah Atmaja, atau fitur gameplay baru.

Kursi kini tegak dan menghadap meja, dengan tinggi dudukan terukur 0,403 m. Meja memiliki jarak dari dinding; lemari menghadap area akses. Buku bertumpu pada kabinet/rak dan buku di bawahnya, bukan melayang di balik pintu kabinet. Ponsel bertumpu pada segitiga mesh kain kasur; collider kasur dan delapan target interaksi diselaraskan. Jaket dipindahkan dari bidang pintu. Kesalahan dinding yang menutup sekitar 43 cm pintu serta celah vertikal di atas jamb diperbaiki. Bukaan pintu terukur 0,98 m; mekanisme interaksi/transisi yang sudah ada dipertahankan.

Tambahan yang digunakan: wadah penyimpanan plastik dan keranjang rotan Poly Haven, PBR katun untuk gorden, serta model orisinal kipas meja dengan pelindung kawat, tas ransel, sandal jepit, gorden berlipit dan router. Ada rak kipas, engsel/handle, stopkontak, adaptor, charger dan kabel yang mengikuti permukaan. Jendela Kenney yang ada diperbaiki dengan memisahkan kaca/kayu/logam dan membuang panel dinding bawaan kit yang menutupi bukaan. Arah normal kedua panel kain juga dikoreksi. Sumber dan lisensi CC0 aset unduhan diverifikasi melalui [Poly Haven](https://polyhaven.com/license); rincian author, URL dan checksum terdapat di [kredit aset](asset-credits.md) dan [manifest V2](kos-v2-asset-manifest.json). Hanya sekitar 6,3 MB sumber tambahan yang diunduh, dengan map prop diperkecil ke 512 px.

Diagnosis screenshot gelap menggunakan kamera, pose, pencahayaan dan scene awal yang sama. [Kontrol render](Visuals/V2-Diagnosis/metrics.json) menunjukkan dua masalah yang terpisah: target HDR linear yang dibaca langsung ke PNG dan scene yang terlalu redup setelah grading. Nilai berikut adalah luma piksel PNG ternormalisasi; ambang gelap 0,08.

| Kontrol pada scene awal | Rata-rata luma | Piksel gelap |
| --- | ---: | ---: |
| HDR linear, post-processing aktif | 0,000036 | 99,99% |
| Target sRGB eksplisit, post-processing aktif | 0,002517 | 99,56% |
| Target sRGB, post-processing dimatikan | 0,115243 | 34,58% |
| Target sRGB, Split Toning dimatikan | 0,013641 | 96,02% |
| Target sRGB, Split Toning dan SMH dimatikan | 0,017002 | 93,64% |

URP ternyata sudah menggunakan **PerPixel**; dugaan awal PerVertex tidak terbukti. Menonaktifkan bayangan bohlam juga tidak memperbaiki kontrol gelap. VPS mampu merender scene: keterbatasan headless bukan satu-satunya penjelasan gambar hitam. Penyelesaian mencakup intensitas sumber cahaya dan bounce yang lebih masuk akal, sumber jendela dipindah ke sisi dalam dinding, grading netral yang ringan, serta capture melalui satu `StandardRequest` ke ARGB32 sRGB. Capture menunggu frame inisialisasi dan tiga frame setelah perpindahan kamera, memakai kamera interaksi pemain, mengembalikan render target sebelumnya dan menolak hasil yang tidak terbaca. Tidak ada manipulasi kecerahan PNG.

Tujuh capture final dari Editor Play Mode/Xvfb/llvmpipe lolos dan diperiksa secara visual: [awal](Visuals/V2/01-startup.png), [ruangan](Visuals/V2/02-wide-room.png), [meja](Visuals/V2/03-desk.png), [jendela](Visuals/V2/04-window.png), [kasur](Visuals/V2/05-bed.png), [pintu](Visuals/V2/06-door.png), [penyimpanan](Visuals/V2/07-storage.png). Luma rata-rata 0,061–0,289; piksel gelap 15,7–61,2%, dengan jendela tetap menjadi sudut paling redup. [Telemetry final](Visuals/V2/visual-metrics.json) menyimpan GPU, API, kamera, pose dan format capture. Overlay HUD tidak termasuk dalam capture kamera Editor ini.

Anggaran aktual: **88.001 triangle, 179 renderer, tekstur terpakai maksimal 1024 px**. URP Forward memakai FXAA, SSAO setengah resolusi, shadow atlas 1024 dan jarak shadow 10 m. Hanya satu sumber lingkungan memakai bayangan; realtime reflection probe, fog, grain, chromatic aberration dan split toning dimatikan. Bloom/vignette dipertahankan ringan. Ini pengaturan konservatif, bukan pengukuran performa Iris Xe.

Validasi headless: **979 pemeriksaan scene, 0 kegagalan**, termasuk world bounds, kontak lantai, penopang melalui segitiga mesh, tumpukan buku, arah/tinggi kursi, bukaan pintu, collider, material/model, missing scripts dan transform. Flood-fill capsule menemukan 196 sel jalur yang tersambung pada grid 25 cm; seluruh delapan interaksi memiliki raycast pertama yang tidak terhalang dari posisi pemain yang dapat dicapai. Pemeriksaan perpotongan besar memakai footprint bounds; PC yang sengaja berada di bawah meja diperiksa terhadap collider kaki meja. [Hasil scene](kos-v2-scene-validation.json). **34/34 tes otomatis lulus**: 32 tes lama, termasuk movement/headroom, puzzle, kedua urutan prolog, smartphone/MataBatin dan save/reload, ditambah pemeriksaan scene serta tes negatif yang sengaja mengangkat kursi untuk membuktikan deteksi melayang. [XML tes](kos-v2-tests.xml). Validator milestone juga exit 0. Capture final memvalidasi scene lagi sebelum build akhir.

Build final **Windows x86_64 Mono development: Succeeded, exit 0, 0 error, 0 warning**. Startup scene tetap Prolog_KamarKos pada index 0. Mono diverifikasi dari pengaturan backend, managed assemblies dan binary Mono AMD64 yang benar-benar dihasilkan. [Receipt build dan pemeriksaan](kos-v2-build-summary.json); log lengkap tersedia di `Builds/Logs/windows-development.log` dan `Audit/kos-v2-*.log`.

- Player: `Builds/Windows/LayarTerakhir.exe`, direktori lengkap 290 file / 226.538.205 byte (216,0 MiB).
- ZIP WinSCP: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`, 111.847.744 byte (106,7 MiB).
- SHA-256 ZIP: `b89c0e7a2bbd2b9ba762766bd724f1ead4cf9cd58a0163ca22ff8995a75a8d63`.
- Semua 288 file BuildReport dan 112 assembly diperiksa; seluruh 290 isi ZIP cocok dengan direktori output dan lolos CRC. Kredit aset dan panduan screenshot ikut dikemas. Build sebelumnya tersimpan di `Builds/Previous/`.

Untuk mengulang di VPS, gunakan `Tools/run_kos_v2_checks.py --stage capture`, `--stage validate`, `--stage test`, atau `--stage build`; setelah build berhasil jalankan `Tools/package_windows_build.py`. Perubahan scene tersedia melalui menu `LayarTerakhir/Environment/Apply furnished Kos V2 corrections` dan sumbernya di `KosVisualV2.cs`.

**Batas verifikasi:** executable Windows belum dijalankan; FPS dan penggunaan RAM pada Intel Iris Xe/8 GB belum diukur. Render yang diperiksa berasal dari Editor dengan software GPU, sehingga visual driver Windows masih memerlukan playtest. Development build menyediakan **F12** untuk capture frame akhir yang sebenarnya, termasuk HUD; opsi `-kosCapture` mengambil startup setelah delapan detik. Lokasi dan cara penggunaan ada pada [panduan Windows](windows-kos-capture.md), juga disertakan dalam ZIP.
