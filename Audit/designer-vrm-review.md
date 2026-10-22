# Review model VRM desainer — 22 Oktober 2026

Baseline sebelum pergantian: `cdd0eae`. Tujuan revisi: langsung memasang model
kiriman desainer untuk playtest perbandingan, tanpa menambah adegan atau sistem.
Penilaian gaya akhir tetap milik desainer; laporan ini tidak menyatakan kualitas
visual otomatis setara DreadOut/Outlast.

## Terpasang dan disiapkan

| Model | Hasil |
|---|---|
| Raka | Aktif menggantikan tubuh pemain dan tangan HP first-person |
| Nyonya Atmaja | Aktif menggantikan pemburu serta penampakan AR, cermin, gantung, dan sosok luar rumah |
| Damar | Prefab authoring siap; belum menjadi actor baru dalam rekaman yang memakai POV Damar |
| Enam VRM lainnya | Sumber tetap utuh, belum dipaksakan ke actor/adegan yang tidak ada |

File sumber tidak ditimpa. Hasil FBX, material dan tekstur siap Unity terpisah
dari VRM mentah. Lima file inti gerak/HP/chase serta dua scene kos
dibandingkan byte per byte dengan baseline (`designer-vrm-preservation.json`).
Scene Atmaja berubah pada visual actor; root AI, collider dan alur tetap dipakai.
Tiga file presentasi runtime juga berubah: keyword material diikat ulang setelah
pergantian shader kamera, serta kontak kaki saat crossfade jongkok memakai
solver dua sendi yang sudah ada dengan bidang lutut animasi tetap dipertahankan.
Pose motor juga menyelaraskan kemiringan torso dengan jangkauan lengan model
baru, sambil mempertahankan pinggul pada jok dan tangan pada setang.

## Adaptasi sebelum/sesudah

- Raka sebelumnya memakai model/garment terpisah. Kini memakai mesh kiriman
  desainer yang menyatukan jaket, kaus, celana, sepatu dan tubuh. Penampilan
  berjaket tetap; inventaris/progres pengambilan jaket dipertahankan.
- VRM tidak berisi animasi. Enam belas klip Raka memakai kontrak yang sudah
  ada, dengan pose tulang disesuaikan ke proporsi baru. Collider dan kecepatan
  permainan tidak diubah untuk menyembunyikan masalah mesh.
- Rig ibu jari kanan sumber tidak mempunyai vertex berbobot. Tiga sendinya
  disesuaikan dan 57 vertex diberi bobot. Genggaman dikalibrasi dari geometri
  tangan/HP; pose siku dipisahkan untuk berdiri, jongkok dan jongkok rendah.
- Ruang kepala jongkok disesuaikan dengan membengkokkan postur, bukan mengecilkan
  mesh secara vertikal. Siku rendah menopang HP dan HP digeser ke sisi wajah
  saat merunduk agar pergelangan/lengan serta rambut tidak masuk ke casing.
- Kepala Nyonya yang sudah miring tidak ditambah kemiringan lama. Rig lengan
  otomatis berbentuk T disesuaikan dengan lengan menekuk pada artwork. Gunting
  sumber tetap tunggal dan diberi bone untuk presentasi yang sudah ada.
- Batas artwork Atmaja dinaikkan menjadi 350 ribu triangle termasuk Raka dan
  pemburu. Albedo karakter maksimal 2048 px; environment dan map data tetap
  1024 px. Belum ada klaim FPS perangkat target.

## Validasi

- **20/20 pemeriksaan presentasi lulus** (19 geometri/player dan satu material live) pada hasil impor Unity. Genggaman diperiksa
  pada tujuh klip HP × delapan fase, ditambah mesh first-person. Ambang pengujian:
  penetrasi vertex ke casing kurang dari 1,5 mm (toleransi impor), kontak kelima
  jari dalam 4 mm. Ini bukan klaim jaminan nol intersection untuk setiap segitiga.
- Delapan klip jongkok × delapan fase memenuhi ruang kepala normal/rendah dan
  batas lantai. Tes gerak dan headroom/collider yang sudah ada juga lulus.
- Enam puluh campuran pose diam/bergerak pada jongkok normal/rendah diuji
  untuk kontak kaki dan ruang kepala. Solver mempertahankan bidang lutut dari
  animasi, serta tidak mengubah posisi root/controller. Capture awal menemukan
  penetrasi telapak hingga 11 cm saat crossfade; bukti itu dipisahkan dari hasil
  akhir yang sudah diambil ulang. Deviasi kaki maksimum pada langkah jongkok
  normal dalam capture final sekitar **0,34 mm**.
- Capture awal juga menemukan keyword lokal tidak terikat ulang ketika shader
  material berpindah ke camera fade. Normal dan metallic mask kini diaktifkan
  pada keyword space shader tujuan; fallback kain/kulit memakai metallic 0.
- Satu percobaan editor terhenti di GC/loader internal host; pengujian diulang
  dengan Burst editor dinonaktifkan dan eager binding per proses. Override
  tercatat pada invocation; bukan perubahan pengaturan player/build. Dokumentasi
  opsi Burst: [Unity](https://docs.unity.cn/Packages/com.unity.burst%401.8/manual/getting-started.html).
- **114/114 tes regresi lengkap lulus**, tanpa tes gagal atau dilewati.
  Bukti final: `designer-vrm-full-tests.xml` dan invocation pasangannya.
- Native Linux: **30 pemeriksaan gerak/HP**, **18 pemeriksaan motor/keberangkatan**,
  serta **153 pemeriksaan lanjutan Atmaja hingga title/Continue** lulus.
  Capture motor mencakup posisi jok dan kedua tangan ke setang.
- Capture feel dibuat sebelum perubahan terakhir yang hanya menyentuh fitting
  saat naik motor. Perbandingan lengkap snapshot membuktikan sumber feel/HP,
  kamera dan artwork tetap sama (`designer-vrm-feel-continuity.json`).
  Capture motor, lanjutan prolog dan suite 114 tes memakai sumber final.
- Build Windows x86_64 Unity **Succeeded, 0 error**. Validasi native memakai
  Linux llvmpipe; Windows belum dimainkan langsung dan FPS target belum dinilai.

Bukti geometri: `designer-vrm-geometry-tests.xml`,
`designer-vrm-geometry-test-invocation.json`, `designer-raka-grip.json`.
Detail sumber dan reproduksi: [panduan model](../Docs/DESIGNER_VRM_MODELS.md).

## Perbandingan yang dapat direview

Model lama dan baru harus dilihat pada gameplay yang sama. Render authoring
Blender membantu memeriksa bentuk rig, tetapi bukan bukti tampilan/FPS Windows.
Capture gameplay final tersedia di bawah. Bukti ZIP/upload R2 dicatat pada
`r2-windows-build-upload.json` setelah pemeriksaan byte dan SHA-256 remote.

| Situasi | Model sebelumnya | Model VRM baru |
|---|---|---|
| Berjalan | [Sebelum](Visuals/DeepSeek-Review-Feel/02-walk-stride.png) | [Sesudah](Visuals/Designer-VRM-Feel/02-walk-stride.png) |
| Jongkok | [Sebelum](Visuals/DeepSeek-Review-Feel/05-crouch-settled.png) | [Sesudah](Visuals/Designer-VRM-Feel/05-crouch-settled.png) |
| HP first-person | [Sebelum](Visuals/DeepSeek-Review-Feel/10-phone-first-person.png) | [Sesudah](Visuals/Designer-VRM-Feel/10-phone-first-person.png) |

Screenshot gameplay di atas memakai jalur audit yang sama pada Linux llvmpipe.
Render authoring/source terpisah di `Visuals/Designer-VRM-Authoring/`.

## Batas review visual dan cakupan

- Model VRM sumber tidak memuat animasi; gerak adalah adaptasi klip proyek,
  bukan animasi yang disalin dari DreadOut.
- Screenshot first-person memperlihatkan siluet jari yang lebih bersudut pada
  jarak dekat dibanding model lama. Hasil tes jarak genggaman tidak membuktikan
  kualitas artistik close-up sudah final. Lihat capture HP sebelum/sesudah.
- Jaket/tas hitam mempunyai highlight basah yang cukup kuat pada pencahayaan
  jalan. Material/normal kini terbaca; kekuatan highlight tetap perlu penilaian
  desainer, bukan klaim bahwa semua permukaan sudah lebih baik.
- Outfit Raka menyatu dengan mesh dan berjaket sejak awal; interaksi mengambil
  jaket tetap berjalan tetapi belum mempunyai pergantian pakaian visual.
- Fixture prolog mulai pada checkpoint tangga Atmaja yang sudah terbuka,
  berjalan kontinu 128,75 m sampai title dan Continue; bukan playthrough manual
  New Game dari kamar. Fixture motor memeriksa keberangkatan dari halaman kos
  sampai checkpoint kedatangan Atmaja.
- Damar hanya disiapkan sebagai prefab. Enam VRM lain belum dipasang karena
  tidak mempunyai actor padanan dalam cakupan prolog ini.

## Build Windows untuk review

- ZIP: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`.
- Ukuran: **210.591.533 byte**. SHA-256:
  `915f0798cfe4e0cfc3ae5d8f1997f08fad2de0c6884e8938d07ca442371e911a`.
- PE Windows x86_64, 112 assembly scripting, ketiga scene, kelengkapan file
  dan CRC ZIP sudah diperiksa (`designer-vrm-windows-build-verification.json`).
- Panduan review disertakan di ZIP sebagai `Character-Review-Guide.txt`.
- Status upload dan pembacaan kembali seluruh objek R2 dicatat pada
  [bukti upload](r2-windows-build-upload.json).
