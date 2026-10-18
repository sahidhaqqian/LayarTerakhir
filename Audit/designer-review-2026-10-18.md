# Revisi hasil playtest desainer — 18 Oktober 2026

Status: revisi diterapkan, pemeriksaan lulus, build Windows dikemas dan upload R2 terverifikasi.

Dasar penerimaan: `goal.md` milik desainer, SHA-256 `85db12a5351cd7e9fc931826d6a90f47c6d6dc2f62fc9d34be6344aaec998af5`.
File arahan tidak ditulis ulang oleh agen.

## Implementasi

- Piring keramik retak/berjamur, makanan busuk dan bangkai tikus memakai mesh baru.
- Sofa, meja, karpet, kabinet/radio menambah fungsi ruang keluarga; kedua sisi ruang makan dibuka.
- Tangga dipertahankan. `designer-review-stairs-preserved.json` membandingkan struktur sebelum/sesudah authoring.
- Kamar anak ditata ulang; lorong basement terhubung bertambah, termasuk patrol route dan penerangan.
- Kamera mempertahankan first-person/over-the-shoulder, dengan pakaian kaos/jaket terpisah.
  Rig Raka memiliki 55 joint, adaptasi jari Nera lokal, dan 12 klip termasuk interaksi kartu.
  Duduk motor memakai pelana tertutup serta penyelarasan pelvis, tangan dan kaki ke geometri kendaraan.
- APK memiliki ikon vektor, adegan transfer kartu dan installer/progres koneksi Kamera.
  Tutorial AR satu kali; kursi normal kosong dan perempuan berleher patah hanya muncul di AR.
- To-do/tujuan HUD dan jurnal dihapus. Bukti foto memakai kilau kecil di AR.
- Dokumen diarsipkan dengan isi lengkap ke HP dan tanda baca tersimpan saat dibuka.
  Shutter memberi bunyi mekanis, pulsa lampu dan kilatan layar; gangguan sinyal tetap di HP.
- Dialog darkweb, panggilan Raka dan teriakan Damar dari atas memakai bank suara baru.
- Lemari menerima E untuk keluar dan menolak masuk ulang dari keypress yang sama.
  Kematian membuka menu; Continue eksplisit memuat save. Dunia lama dibekukan selama transisi.
- Sumur berengsel memiliki lubang fisik/visual dan adegan ditarik masuk; bisikan menolehkan pandangan.
- Kompleks Melati memakai 8 varian rumah unduhan, jalan/persimpangan, warung dan hujan terus-menerus.
  Perjalanan pertama berhenti untuk eksplorasi. Belanja memeriksa harga, stok, saldo dan save atomik.
- Komentar live diantrekan satu per satu dengan interval 2–3 detik dan variasi ruangan.
  Musik chase memakai kendang/saron sintetis 186 BPM dan sirine glitch.

## Pemeriksaan kode dan scene

- Syntax: 93 berkas C# dan 45 berkas Python, tanpa kesalahan parse.
- Tooling: 17 tes Python lulus, termasuk integrasi allowlist Graphify.
- Suite Unity: 85/85 lulus, tanpa skipped; laporan lengkap disimpan di
  `designer-review-suite-tests.xml` dan `designer-review-suite-invocation.json`.
  Suite ini dijalankan setelah perbaikan cutscene sumur dan acuan kontak motor terakhir.
  Setelah perapian ruang keluarga, tes lintasan meja makan → ruang duduk → jam
  dijalankan lagi dan lulus; bukti ada di `designer-review-lounge-tests.xml`.
- Validasi Atmaja: 4.917 pemeriksaan, 0 kegagalan, 290.959 triangle termasuk
  Raka runtime dan hunter, 1.391 renderer, tekstur maksimum 1K.
- Tangga: signature sebelum/sesudah authoring sama;
  `designer-review-stairs-preserved.json` mencatat `unchanged: true`.
- Scene kamar kos awal tetap memakai SHA-256 terlindung dari manifest sebelumnya.

Tes belanja memverifikasi pengurangan saldo, persistensi barang, penolakan beli ulang,
serta transaksi tanpa saldo cukup. Tes lain memeriksa arsip teks utuh dan read receipt
saat membuka dokumen, pakaian setelah mengambil jaket, pulsa LED dan layar saat
memotret, interval komentar, keluar lemari dengan satu keypress, koneksi lorong
basement, dan menu kematian/Continue tanpa kematian ulang selama scene loading.

## Pemeriksaan native dan penyerahan

Standalone Linux terbaru yang sudah lulus:

| Skenario | Pemeriksaan lulus | Gambar | Rute |
| --- | ---: | ---: | --- |
| Kartu/APK, AR, arsip, sumur, menu dan Continue | 40 | 13 | 28,08 m |
| Jok/setang, berhenti di kompleks dan naik motor lagi | 20 | 4 | Cinematic perjalanan asli |
| Lantai dua, basement, persembunyian, pengejaran hingga tamat | 134 | 29 | 129,07 m |
| New Game → kos → kompleks/warung → teka-teki jam → Continue | 224 | 54 | 174,85 m |

Keempat skenario lulus dan seluruh capture diverifikasi terhadap nama, dimensi,
waktu pembuatan dan receipt run tersebut: **418 pemeriksaan, 100 gambar**.
Bukti tersimpan di `Audit/Visuals/{Designer,Rider,Prolog,Opening}-Review-Final`.

Pijakan tertinggi lintasan lantai dasar terakhir adalah **0,592 m** pada lantai
0,54 m. Kedua sisi meja, ruang duduk dan jalur menuju jam dilewati tanpa naik
ke furnitur. Sofa kini menghadap ke meja, meja kopi berukuran lebih ringkas dan
kaki meja dekoratif tidak lagi menahan kapsul. Tangga tetap memiliki signature
struktur yang sama sebelum/sesudah authoring.

Headroom di bawah meja diuji sebelum pemasangan kartu; lokasi akhir cutscene
memiliki ruang berdiri. Campuran audio pada listener native menghasilkan RMS
0,04318 dan peak 0,65894, tanpa clipping pada skenario opening yang direkam.

Build Windows terakhir berhasil (Mono x86_64 Development, 0 error).
Graphify sudah diverifikasi: 138 file kode, 1.963 node, 6.924 edge, 0 aset.
Arsip Windows berukuran **196.209.502 byte** (187,12 MiB), lengkap dengan
verifikasi PE AMD64/Mono, seluruh file build, assembly, ketiga scene dan CRC ZIP.

- Lokasi: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`
- SHA-256: `1037dc709b96744316c4404d7f0e40bfc404d9c1ac8bbf3488eb4b6226efabfa`
- Readback seluruh objek R2 cocok dengan ZIP lokal; diverifikasi 18 Oktober 2026,
  08:08:05 UTC. Rclone sempat menerima respons 501, retry berikutnya berhasil.
- Bukti: `golden-build-summary.json`, `r2-windows-build-upload.json`, source snapshot
  `designer-review-prolog-presentation-build-sources.json`, dan receipt tiap skenario.

Perubahan game, aset, instruksi/brief desainer yang dipertahankan, bukti dan keempat
output Graphify disiapkan dalam commit revisi ini ke `main`.

## Batas penerimaan

Capture standalone Linux memakai llvmpipe dan navigasi terarah melalui
CharacterController/input asli. Bukti ini memeriksa gambar dan alur permainan;
bukan pengukuran FPS Windows atau playtest manual pemain baru.
Performa perangkat target, penerimaan animasi/akting dan tingkat horor setara
DreadOut masih membutuhkan penilaian desainer. Suara baru memakai bank neural lokal
VoxCPM2, bukan rekaman aktor final. Jalan kompleks dapat dijelajahi dan warung dapat
digunakan, tetapi rumah tetangga menjadi latar eksterior, bukan interior lengkap.
