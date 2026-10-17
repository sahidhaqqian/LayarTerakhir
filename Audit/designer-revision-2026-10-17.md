# Revisi desainer — 17 Oktober 2026

Target: worktree `LayarTerakhir-main`, branch `main`. Daftar desain di `goal.md`
mengikuti 23 catatan playtest terbaru. Screenshot Bugs lama tidak digunakan.
Kelanjutan prolog dari commit `078aafd`/`5b8be78` dipertahankan melalui merge;
WIP worktree DeepSeek tidak dimasukkan. Hasil tes/build akhir dicatat di bawah,
bukan disimpulkan dari bukti build historis sebelum revisi.

## Perubahan sesuai desain

| No. | Implementasi |
| --- | --- |
| 1 | Spotlight HP menerangi dunia, range 16 m/cone 88°, tambahan spill dekat tangan; lampu mengikuti pivot pemain, bukan kamera bahu. |
| 2 | Panel handset diperkecil menjadi sekitar 20% lebar layar dan 71% tinggi. |
| 3 | Kamera bahu kanan dengan spherecast antitembus dinding; tubuh disembunyikan ketika kamera terdesak hingga <0,9 m agar kepala/punggung tidak menutup pandangan. Tubuh/gerak Raka prosedural, termasuk jalan, merunduk, HP dan motor; belum rig/model karakter final. |
| 4 | Subtitle dibungkus dan tinggi kotak/backing mengikuti isi; menyempit ketika HP terbuka. |
| 5 | Dapur, ruang makan bersama, foyer kamar mandi, bilik PRIA/WANITA dengan pintu terpisah, toilet/bak/shower. |
| 6 | Tidak menghasilkan VO sintetis baru. CSV/JSON berisi 60 baris beserta karakter/pemicu/arahan rekaman tersedia di `Audit/Recording`, termasuk kedua respons transfer dan komentar kejutan. Damar (rekaman) memakai aktor Damar yang sama. VO lama masih placeholder; dialog baru menunggu rekaman pengguna. |
| 7 | Donasi tidak membuka HP/pesan paksa. Getaran/SFX, banner saat handset dibuka, Pesan dibuka sendiri, reaksi setelah 10 detik waktu game sejak dibaca. |
| 8 | Peta offline ilustratif baru: distrik, jaringan jalan, sungai, rute, pin lokasi, drag dan zoom. Lokasi/rute cerita fiktif, bukan navigasi GPS nyata. |
| 9–10 | Percakapan Ibu sakit dan Bu Haji Ratna; biaya rumah sakit Rp1,5 juta dan kos Rp3,5 juta terbuka sesudah panggilan masing-masing. Konfirmasi, bukti lunas, save atomik dan perlindungan pembayaran ganda. |
| 11 | Interactor, pintu dan interaksi keberangkatan tidak lagi diblokir oleh aplikasi HP; menu/modal tetap memblokir. |
| 12 | Fasad kos berwarna, delapan jendela berbingkai, ambang, langit malam prosedural dengan bulan/bintang. |
| 13 | Gerbang kos dan Atmaja memakai dua daun dengan engsel luar dan bukaan dari sambungan tengah. |
| 14 | Catatan Damar dipindah ke dalam rumah, tidak menjadi interaksi motor lagi; urutan pencarian tidak mensyaratkan catatan sebelum masuk. |
| 15 | Percobaan pintu depan: gerakan tertahan, bunyi latch dan komentar Raka sebelum mencari jalur lain. |
| 16 | Nameplate Keluarga Atmaja di luar dihapus. |
| 17 | Panjat jendela memakai Space secara default, tetap bisa diremap. |
| 18 | Tujuan tampil 7 detik, tersimpan di Jurnal; entri dapat diklik untuk membaca detail dan kembali ke daftar. |
| 19 | Komentar sekali per area di kos/rumah/lantai atas/basement, dengan jeda antarkomentar dan tidak menyela panggilan/cinematic. |
| 20 | Tujuan pencarian menyebut keberadaan/petunjuk tentang Damar, tidak langsung membocorkan benda dan lokasi ruang makan. |
| 21 | Boneka di kursi ujung dikembalikan dalam dunia biasa, ukuran/posisi duduk disesuaikan dan wajah porselen serta gaun pudar dibuat terbaca; terpisah dari sosok gaib AR. |
| 22 | Posisi/sudut pohon, tanah akar dan cabang penopang diperbaiki; jalur merunduk fisik tetap ada, akar dijauhkan dari kursi. |
| 23 | Regresi tambahan: foto bukti memakai lensa HP, permukaan target bukan penghalang palsu, kasur tidak menutup cangkir, clue darah tidak tertutup tangga/dinding, checkpoint catatan dan aktivasi AR tidak memundurkan stage, state pintu restore konsisten, uang terpakai tidak diberikan ulang saat ending. Tangga Atmaja diberi dua flight yang benar-benar muat, bordes dan lantai koridor utuh; dinding gudang/sekat kos yang memotong jalur dihapus dari lintasan. Ventilasi kamar anak punya lubang fisik, bukan tembok utuh di balik efek AR. |

## Verifikasi dan batas cakupan

Tambahan dari playtest saat pengerjaan: mode fullscreen borderless/windowed di
Settings dan Alt+Enter, ukuran window kembali saat keluar fullscreen, window
resizable/maximize, mode tersimpan antar-launch. Default peluncuran normal adalah
fullscreen resolusi monitor; mode editor/audit/flag layar eksplisit tidak ditimpa.
Shortcut native Unity dimatikan agar tidak beradu/dobel toggle dengan handler game.

- Suite Unity terbaru: **67/67 Passed**, 0 gagal, 0 dilewati, di
  `Audit/golden-tests.xml`; wrapper mengecek XML baru dan menyimpan invocation.
  Termasuk lintasan fisik kos/tangga Atmaja/ventilasi, pembayaran atomik,
  kamera foto bukti, checkpoint dan regresi input/UI.
- Syntax C#: 71 file, 0 kesalahan parse. Tes Python tooling: **17/17 lulus**,
  termasuk integrasi ignore Graphify dan kelengkapan ekspor naskah rekaman.
- Build Linux diagnostik terbaru: Succeeded, 0 error/0 warning. Identitas sumber
  82 file dibekukan di `Audit/designer-build-sources.json`.
- Build Windows Unity 6000.3.26f1: **Succeeded**, StandaloneWindows64/Mono2x,
  Development, **0 error/0 warning**, ukuran keluaran Unity 313.024.126 byte.
  Selesai 17 Oktober 2026 06:12:37 UTC; bukan klaim telah dijalankan di Windows.
- Standalone final (`Designer-2026-10-17-Release`): **completed true, exit 0**,
  **209 PASS, 0 gagal, 50 screenshot 640×360** diverifikasi baru oleh wrapper.
  New Game → kos dua lantai/fasilitas → motor melewati gerbang → Atmaja sampai
  checkpoint pintu tangga → menu → Continue. Walk capsule kontinu **156,63 m**,
  kaki interior maksimum **0,592 m**, bukan di atas meja. Jalur lantai atas/ventilasi
  dan foto bukti lanjutan juga dicakup tes Unity, bukan klaim seluruh ending telah
  di-playtest manual pada Windows.
- Fullscreen Settings dan Alt+Enter, UI fokus tombol, pembayaran dua tagihan,
  input pintu sambil HP terbuka, panjat dengan Space, clue darah merah di feed,
  dan culling tubuh saat kamera terdesak lulus di player. Review gambar final
  menegaskan pandangan meja bersih serta boneka tampak tanpa AR.
- Tap audio listener aktual: RMS **0,03916**, peak **0,69235**, tanpa clipping
  pada sampel pembukaan. Ini bukan persetujuan kualitas voice acting.
- Suite final memakai sumber yang sama, selesai **06:17:43 UTC**, wrapper
  Finished/exit 0. Runner player Finished/exit 0 **06:21:19 UTC**.
- Paket Windows: **303 file**, 294 keluaran Unity dan 112 assembly diverifikasi,
  native PE AMD64/Mono, seluruh ZIP lolos CRC. Ukuran ZIP **174.516.108 byte**
  (166,43 MiB). Panduan playtest, kredit/lisensi dan CSV rekaman ikut dibundel.
- R2: upload berhasil, kemudian **seluruh objek dibaca ulang** dan SHA-256/ukuran
  cocok dengan ZIP lokal, diverifikasi **06:22:44 UTC**. Percobaan awal menerima
  respons 501; retry berhasil dan hasil tidak diterima sebelum readback cocok.
  Bukti: `Audit/r2-windows-build-upload.json`.
- Graphify terakhir: **109 file kode, 0 file aset**; graph dan hash sumber
  diverifikasi. Scene Golden asli tetap SHA-256
  `4b73b117b832fb753b92d3de885c163bc1dd3f6225101867f7b4e2437c8b793a`.

ZIP: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`.
R2: `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip`.
SHA-256: `027e9f224e34d9af82adc9015284eb231c843faf5eb24fc4edf36e843fd580a8`.

Iterasi pengujian sengaja tidak disembunyikan: suite awal 57/63 dan berikutnya
61/65 mengungkap regresi UI lama serta benturan geometri; run berikutnya 65/66
menyisakan lintasan tangga. Probe collider dan XML berlabel initial/intermediate
adalah bukti diagnosis sebelum perbaikan, bukan hasil penerimaan akhir.
`designer-stair-tests.xml` kemudian lulus tes berjalan dari lantai bawah, naik
dua flight, menyeberangi koridor dan merunduk lewat ventilasi dengan AR aktif.

Satu rerun Editor berhenti di native FMOD/TLSAllocator audio-thread startup dan
tidak menulis XML baru, meskipun proses mengembalikan exit 0. Run tersebut tidak
diterima sebagai lulus. Pengujian ulang memakai perangkat PulseAudio virtual
clocked yang terpisah, sama seperti player audit; wrapper mengecek umur XML,
jumlah tes dan hasil Passed, bukan hanya exit code. Tidak mematikan audio tes.

Percobaan standalone pertama berhenti sesudah 35 screenshot karena waypoint
audit memotong diagonal dari sisi halaman ke teras, bukan mengikuti jalan tengah.
Rute audit diperbaiki dengan waypoint jalan tengah dan diuji ulang dari New Game;
tidak memindahkan pemain melewati benturan atau menghapus obstacle dunia.
Receipt `Designer-2026-10-17/player-audit.json` yang gagal tetap disimpan.
Invocation percobaan tersebut masih berstatus Running karena wrapper sesi lama
tidak menyelesaikan penulisan akhir; ini bukan bukti penerimaan lulus.

Run berikutnya (`Designer-2026-10-17-Final`) menuntaskan 49 screenshot dan
207 pemeriksaan PASS, berjalan 156,63 m tanpa menaiki meja (kaki interior
maksimum 0,592 m), serta berhasil Continue. Dari review gambar ditemukan
kamera terlalu dekat sehingga badan Raka menutup meja; boneka biasa juga
terlalu kecil/gelap. Dua hal visual itu diperbaiki, lalu build dan audit diulang
ke direktori `Designer-2026-10-17-Release` dengan tambahan gambar boneka normal.
Sumber run sebelum perbaikan disimpan di `designer-before-camera-build-sources.json`.
Wrapper dua run headless sempat berakhir 143 meski player/Editor sudah menghasilkan
hasil lengkap; tidak menganggapnya exit 0. Runner final memakai sesi proses
terpisah serta heartbeat, dan tetap memvalidasi umur/isi hasil setelah proses keluar.

Persetujuan artistik, pacing dialog pada kecepatan normal, akting suara, gamepad,
dan performa Windows/Iris Xe tetap memerlukan playtest manusia pada build Windows.
Screenshot standalone Linux/Mesa bukan klaim performa atau rendering identik Windows.
Tidak ada klaim bahwa model Raka/animasi dan VO sudah final produksi.
Frame awal cinematic motor (`13-mounting-motorcycle`) sempat terhalang tiang
teras, sementara frame keluar gerbang (`14-riding-through-gate`) memperlihatkan
Raka/motor. Framing transisi itu masih perlu dinilai pada kecepatan normal.
