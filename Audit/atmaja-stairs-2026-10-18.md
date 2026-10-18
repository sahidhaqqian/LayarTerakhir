# Revisi tangga, lantai dua dan HP — 18 Oktober 2026

Perubahan sudah diterapkan pada Atmaja. Scene Kos dan KoridorKos tetap byte-identical.
Tidak membaca/menerapkan ulang goal.md, sesuai arahan pengguna.

## Perubahan

- Dua flight baru, masing-masing 12 anak tangga tertutup, MeshCollider, nosing, soffit,
  stringer yang menyambung tiang pegangan, guard kontinu dan bordes bertumpu.
- Tangga ditutup dinding opak di depan/samping. Pintu kayu pada depth 6.31 membuka
  90 derajat ke dalam dan tetap memiliki collider; pemulihan mengikuti flag checkpoint.
  Pintu yang sudah terbuka tidak dapat mengulang/memundurkan progres lantai atas.
- Pintu/hinge/prompt dan partisi gudang yang tidak diperlukan dihapus; lemari menjadi
  ceruk terbuka. Sambungan lantai/dinding dan tepi bordes lantai dua dirapikan.
- Akar visual tubuh menembus lantai adalah `BAND_mid`: kuboid fasad sumber 12.6 x 9.6 m,
  y=4.04–4.20, yang menutup seluruh interior tanpa collider. Cap interior dipotong pada
  override mesh beton scene; lis luar, FBX sumber dan hull rumah dipertahankan.
- Tujuh material lantai atas sebelumnya menghubungkan ARM langsung ke mask URP sehingga
  AO dibaca sebagai metallic. Kanal dikonversi ke R=metallic, G=AO, A=smoothness, linear,
  maksimal 1024px, DXT5; material lantai bertiling memakai mask yang sama.
- Catatan mempertahankan HP, layar, aplikasi, AR dan live yang sedang aktif. Panel
  bacaan berada di kiri dan HP di kanan; input dunia/aplikasi terkunci selama membaca.
  HP yang sebelumnya tersimpan/mati tidak dipaksa menyala.
- Pergelangan HP memakai orientasi telapak yang jelas; jari menekuk menurut sumbu
  knuckle masing-masing, ibu jari beroposisi, dan handset/screen berada di luar telapak.
  Detail mesh tangan ditingkatkan. Ini memperbaiki pose tangan kosong menyerupai cakar.

## Bukti dan batas pengujian

- 83 berkas C# lolos pemeriksaan sintaks; Python compile dan 17 tes tooling lolos.
- [Regresi visual 77/77](stair-visual-regression-tests.xml) mencakup versi dengan
  material dan pemotongan cap fasad. [Tiga tes akhir](stair-final-focused-tests.xml)
  dijalankan setelah pengaman interaksi pintu terakhir, termasuk pemeriksaan bahwa
  interaksi ulang tidak mengembalikan EvidenceHunt ke checkpoint tangga.
- [Validasi scene](atmaja-scene-validation.json): 4.219 pemeriksaan, nol kegagalan;
  295.989 triangle total (262.800 statis + 15.803 Nyonya + 17.386 Raka), tekstur maksimal 1024px.
- [Standalone prolog](Visuals/Atmaja-Stair-Final/player-audit.json): 134 pemeriksaan
  lolos, 29 capture 960x540, 130.67 m lewat CharacterController, lantai atas/basement/
  pelarian/Blitz/title/Continue selesai tanpa reset tersembunyi.
- [Motor](Visuals/Atmaja-Stair-Rider-Final/player-audit.json): 12 pemeriksaan, tiga capture.
- Capture [pintu tertutup](Visuals/Atmaja-Stair-Final/00-opaque-door-conceals-stairwell.png)
  adalah inspeksi terkontrol: leaf ditutup sementara dalam fixture checkpoint yang
  sudah terbuka, lalu dikembalikan; bukan klaim memainkan ulang puzzle jam.
- [Tangga atas sebelum pemotongan cap](Visuals/Atmaja-Stair-Continuation/00b-supported-return-stair-flight.png)
  dan [sesudah](Visuals/Atmaja-Stair-Final/00b-supported-return-stair-flight.png) menunjukkan
  akar masalah visual dan tubuh Raka yang sekarang terlihat utuh.
- [Catatan dengan HP tetap aktif](Visuals/Atmaja-Stair-Final/03-bima-diary.png).
- `stair-visual-build-sources.json` merekam 2.082 sumber build Linux/capture/regresi 77.
  `stair-prolog-presentation-build-sources.json` merekam 2.082 sumber build Windows akhir.
  Selisih tepat dua berkas: AtmajaItem.cs (pengaman pintu sesudah unlock) dan assert
  AtmajaPlayTests.cs. Seluruh aset/visual, jalur, UI HP dan kode lain identik;
  pengaman terakhir diuji pada Editor dengan sumber Windows akhir.
- Replay pembuka yang lebih luas dihentikan setelah pemeriksaan Kos agar fokus pada
  revisi Atmaja; tidak diklaim lulus. Capture parsialnya tidak dipertahankan.
- Satu percobaan Editor macet di native unused-asset unloading sebelum tes pertama;
  tidak dihitung lulus. Run Editor baru selesai 77/77. Catatan: stair-test-runner-retry.json.
- Capture adalah Linux llvmpipe terarah; bukan penerimaan FPS, pacing manual atau
  playtest langsung Windows. Pemeriksaan Windows langsung tetap perlu dilakukan.

## Build dan distribusi

Windows x86_64 Mono Development: sukses, nol error/warning, 331.518.369 byte keluaran
native, 23.505 detik. ZIP diverifikasi dari output PE AMD64 dan sumber terakhir,
termasuk scene, resource Raka, lisensi dan panduan playtest.

ZIP: Builds/LayarTerakhir-Windows-x86_64-Development.zip
Ukuran: 185500094 byte
SHA-256: b7534accbfe8242720d4f2960ea6d6df812ca4ce9fcf18f4b21c14cf0d3280d1
R2: r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip

Upload sukses dan seluruh objek R2 dibaca ulang: SHA-256 cocok dengan ZIP lokal.
Bukti: [receipt R2](r2-windows-build-upload.json). Satu retry jaringan otomatis pulih;
selesai hanya setelah verifikasi objek penuh berhasil.

Graphify diperbarui setelah kode terakhir: 124 berkas kode, 1.770 node, 6.082 edge,
0 berkas aset. Empat keluaran graph disimpan; graph adalah peta kode, bukan bukti gameplay.
