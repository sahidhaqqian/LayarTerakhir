# Penyelesaian prolog — 17 Oktober 2026

Permintaan: lanjutkan bagian prolog yang sudah ada hingga penutup, dengan DreadOut
sebagai sasaran penyajian horor. `goal.md` diabaikan sesuai arahan pengguna karena
revisi di sana sudah selesai. Kelanjutan mengikuti `Prolog.MD`; Kos dan lorong Kos
yang telah disetujui dipertahankan persis byte-nya.

## Perubahan yang diterapkan

- Lantai dua: tiga foto bukti dengan syarat dokumen/tirai/AR yang benar, guratan
  yang mengikuti muka pintu, cangkir retak yang tetap ada sesudah difoto, foto
  keluarga rusak pada bingkai, refleksi kamar mandi dan aparisi khusus cermin.
- Jalur fisik diperbaiki: bukaan pintu ke dalam kamar, akses samping tempat tidur,
  wastafel, lampu gantung, titik turun basement dan jalan keluar pintu jebakan.
  CharacterController dipakai untuk navigasi biasa; animasi lintasan khusus hanya
  mengambil alih pada naik tangga basement, vault meja dan menerobos jendela.
- Basement: glitch baterai 12% sekali, powerbank besar atomik minimal 80% yang
  mempertahankan cadangan sebelumnya, powerbank kecil tetap ada, kabel darurat
  35%, sawer sekali dan handycam yang dapat diputar ulang setelah interupsi.
- Nyonya: patroli dengan penghalang pandangan, pendengaran dan rute mengitari
  perabot; mengambil powerbank tidak mereset posisi musuh. Lemari mempunyai
  pandangan dari dalam, peringatan 1,25 detik, pemeriksaan layar gelap empat detik,
  patroli yang tetap berjalan dan keluar manual setelah musuh menjauh.
- Model Nyonya baru: kebaya gading, rok panjang, wajah, rambut, tangan/jari,
  kepala miring, gunting pada tangan serta pose ancaman dan reaksi Blitz.
  Hierarki sendi dan gerak prosedural menggantikan sosok berdiri lama.
- Rekaman Damar benar-benar dirender di engine: pasar malam di balik cermin,
  sepuluh tangan meraih, kamera ditarik masuk, REC/timecode dan dialog yang
  sinkron. Set dan kamera rekaman mati di luar playback. Checkpoint chase baru
  disimpan sesudah rekaman berakhir; save lama yang terputus bisa melanjutkan.
- Pelarian: rel 3D 31 titik mengikuti elevasi tangga, pintu jebakan menutup,
  vault meja roboh, slow motion dengan tenggat Blitz tiga detik yang berhenti
  ketika pause, biaya 15% sekali dan jendela kabur selama stun empat detik.
  Kegagalan memulihkan kontrol, kamera, musik, baterai dan checkpoint chase.
  Pemeriksaan tangkapan chase juga memakai garis pandang sehingga kedekatan
  posisi pada sisi lain tangga/perabot tidak memicu tangkapan menembus benda.
  Sisa stun empat detik tetap berlaku selama transisi keluar jendela dan tidak
  dibatalkan ketika fase lari halaman dimulai.
- Ending: jalan raya, kilat/siluet, 54.820 penonton, chat yang akhirnya berhenti,
  kontrak pada HP yang sama dengan scroll/tombol simpan, shutter dan title drop.
  Menu utama dan Continue memulihkan layar tamat tanpa menggandakan uang.
  Total Rp25.450.000 mencakup sawer terikat, dana sah dan tagihan yang sudah
  dibayar. Prolog tetap tidak memberikan segel lokasi.
- HUD, Jurnal/Tas/Pesan dan pointer sesudah pause mengikuti fase sinematik.
  Input ditampilkan sesuai binding; komentar eksplorasi tidak menyela pengejaran.

## Verifikasi akhir

| Pemeriksaan | Hasil |
| --- | --- |
| Sintaks C# (AST lokal) | 79 file, 0 error |
| Tes Python, termasuk discovery/ignore Graphify sesungguhnya | 17 lulus, 0 dilewati |
| Suite Unity | 74/74 lulus, 0 gagal, 0 dilewati; invocation selesai dengan exit 0 |
| Kelanjutan standalone Linux | 119 pemeriksaan lulus, 25 screenshot, 129,06 m rute terhubung, tanpa reset checkpoint tersembunyi |
| Build Linux dan Windows x86_64 Development/Mono | Keduanya berhasil, 0 error, 0 warning |
| Validasi scene Atmaja terpisah | 4.183 pemeriksaan, 0 gagal; 282.413 segitiga, 1.170 renderer, maksimum tekstur 1024 |
| Graphify lokal | 118 file kode, 1.653 node, 5.667 edge, 0 file aset; update dan pemeriksaan hash lulus |

Hasil Unity tersimpan di [`golden-tests.xml`](golden-tests.xml) dan
[`golden-test-invocation.json`](golden-test-invocation.json). Validator scene ada
di [`atmaja-scene-validation.json`](atmaja-scene-validation.json). Rekaman sumber
sebelum tes ada di [`prolog-test-sources.json`](prolog-test-sources.json).
Identitas sumber diperiksa kembali sesudah tes dan kedua build. File snapshot
final mencakup kode, scene, aset/importer, settings dan package lock secara lokal;
snapshot ini terpisah dari Graphify yang hanya mengindeks kode.

Standalone final menggunakan llvmpipe 640×360 dan menyelesaikan seluruh kelanjutan
dari checkpoint tangga sampai title, reload checkpoint tamat, total donasi dan
ketiadaan segel. Bukti ada di
[`player-audit.json`](Visuals/Prolog-Completion-Final/player-audit.json),
[`golden-prolog-player-invocation.json`](golden-prolog-player-invocation.json) dan
[`25 screenshot final`](Visuals/Prolog-Completion-Final/).
Build Windows final selesai 16:17:17 UTC, 313.944.027 byte menurut Unity BuildReport.
Snapshot [`prolog-build-sources.json`](prolog-build-sources.json) cocok dengan
1.992 file sesudah tes, build Linux dan build Windows. Hasil iterasi yang gagal
tidak digunakan sebagai bukti kelulusan final; capture pengembangan dipindah ke
`Builds/Previous/Prolog-Development-Captures-20261017` yang diabaikan git.

ZIP final: [`LayarTerakhir-Windows-x86_64-Development.zip`](../Builds/LayarTerakhir-Windows-x86_64-Development.zip),
175.151.448 byte (167,04 MiB). Verifikasi PE AMD64, file player/assembly, marker
scene/runtime, kelengkapan ZIP dan CRC semuanya lulus. SHA-256:
`e60ea4fcf3de65daeb7eae1d03bc8b8c63adfece79e0bf9536390e64131643dd`.
Ringkasan ada di [`atmaja-build-summary.json`](atmaja-build-summary.json).

Upload ke `r2:layarterakhir/Layarterakhir/` berhasil pada 16:20 UTC. Seluruh objek
dibaca kembali dari R2; jumlah byte dan SHA-256 cocok dengan ZIP lokal.
Lihat [`r2-windows-build-upload.json`](r2-windows-build-upload.json).
Build Windows diverifikasi sebagai paket native yang lengkap; runtime Windows
dan FPS perangkat sasaran belum diuji di host Linux ini.

## Batas pembuktian dan kualitas

Audit kelanjutan dimulai dari save terisolasi `PROLOG_ATMAJA_STAIRS_UNLOCKED`
dengan progres Kos/lantai dasar yang sudah diselesaikan. Ia menguji naik tangga,
tiga bukti, basement, stealth, rekaman, pelarian dan ending secara berurutan.
Navigasi diarahkan dengan CharacterController dalam sublangkah maksimal 0,12 m,
menggunakan deltaTime serta kecepatan jalan/merunduk/lari pemain sebenarnya;
interaksi memakai binding E/F/Ctrl/Space/klik kiri. Waktu dibekukan hanya saat
capture. Hasilnya membuktikan keterhubungan rute pada kecepatan permainan,
tetapi tidak menggantikan penilaian kesulitan pengejaran oleh pemain manusia.
Tombol kontrak dipanggil melalui callback UI; Continue diperiksa melalui
pemuatan checkpoint dan pemulihan dunia, bukan klik menu manual.
Ini bukan playtest manual 90 menit atau bukti FPS Windows/Iris Xe.

Alur tamat dan ketahanan checkpoint dapat diverifikasi secara teknis; kesetaraan
artistik dengan DreadOut belum dapat dinyatakan. Raka dan sebagian aparisi lama
masih sederhana, animasi Nyonya masih prosedural, bank suara sintetis lama tetap
placeholder, serta dialog baru tertentu masih subtitle. Penerimaan akting,
pacing/tensi, gerak karakter dan performa pada Windows sasaran memerlukan
playtest manusia. Tidak ada rekaman suara sintetis baru pada pass ini.

## Sumber aset dan reproduksi

Foto keluarga fiktif disimpan di
[`AtmajaFamilyScratched-v1.png`](../Assets/_LayarTerakhir/Art/Atmaja/Textures/AtmajaFamilyScratched-v1.png).
Prompt lengkap, metode dan SHA-256 ada di
[`prolog-generated-art.json`](prolog-generated-art.json); dibuat dengan skill
imagegen dan tool gambar bawaan, tanpa gambar referensi dari game lain.
Karakter dibuat dari geometri Blender original melalui
[`author_atmaja_character.py`](../Tools/author_atmaja_character.py), 7.648
segitiga/22 kelompok mesh. Lihat
[`atmaja-character-authoring.json`](atmaja-character-authoring.json) dan
[`asset-credits.md`](asset-credits.md).

Builder incremental `PrologCompletionBuilder.Apply` memeriksa hash Kos dan
lorong Kos sebelum/sesudah. Graphify dibatasi ke kode first-party melalui
allowlist yang ada; aset game tidak dimasukkan. Panduan kontrol dan pemeriksaan
manual Windows ada di [`atmaja-windows-playtest.md`](atmaja-windows-playtest.md).
