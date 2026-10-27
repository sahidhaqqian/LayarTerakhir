# Guideline Playtest Lengkap — Prolog hingga Babak 5 (Tamat)
Diperbarui 27 Oktober 2026.

Build Development ini memuat Prolog, Babak 1 Rusunawa, Babak 2 RSUD, Babak 3 Pabrik Gula,
Babak 4 Karang Setra, dan Babak 5 Pasar Setan sampai credits.
Dokumen ini menggabungkan rute playtest keenam bagian. File `phase1-windows-playtest.md`,
`phase2-session1-review.md`, `phase3-windows-review.md`, `phase4-windows-review.md`, dan
`phase5-windows-review.md` tetap disertakan sebagai rujukan terpisah per babak.

## Persiapan

Jalankan `LayarTerakhir.exe` tanpa flag audit. Main menu: NEW GAME (save lama diarsipkan),
CONTINUE (checkpoint terakhir), SETTINGS, EXIT. ESC saat bermain membuka pause menu.
Jangan menghapus save untuk mengulang: gunakan NEW GAME. Arsip tetap ada di ArchivedCampaigns.

Alur normal memakai satu campaign dan checkpoint yang sama dari Prolog sampai Babak 5.
Kehilangan fokus jendela saat bermain otomatis membuka pause; Alt-Tab tidak membiarkan
karakter berjalan sendiri.

Untuk menilai satu babak tanpa mengulang dari awal, jalankan launcher review yang sesuai.
Launcher memakai slot save terpisah dan tidak mengubah campaign normal:
- `Review-Phase1.cmd`: `ReviewSaves/Phase1`, fixture 100% baterai tanpa powerbank awal.
- `Review-Phase2.cmd`: slot `Phase2Review`, fixture sesudah Babak 1 dengan saldo belanja nol.
- `Review-Phase3.cmd`: slot `Phase3Review`, fixture P2 selesai, HP 37%, saldo uji Rp2.100.000;
  saldo fixture bukan hadiah pada campaign asli.
- `Review-Phase4.cmd`: slot `Phase4Review`, run baru dimulai di Kos sore dengan 37% baterai,
  Sanity 23 dan saldo 0 untuk menguji jalur gratis (tanpa belanja tetap tamat).
- `Review-Phase5.cmd`: slot `Phase5Review`, fixture PHASE4_CP8 dari route native Phase 4;
  untuk campaign sendiri jalankan `LayarTerakhir.exe` dan Continue dari CP4 sah. Tidak perlu
  kembali ke Kos — Babak 5 dimulai malam yang sama di tangga bawah Pendopo.

Fixture review bukan bukti carry-over campaign dan bukan perubahan aturan New Game.
Untuk mengulang Phase3, cadangkan lalu hapus folder `Phase3Review` di lokasi save aplikasi.

## Kontrol

- WASD bergerak, Shift lari, Ctrl jongkok, Space vault, E interaksi. Beberapa panel daya,
  dinamo, dan katup perlu **menahan E**.
- Tahan **RMB** untuk viewfinder HP: HP terangkat dua tangan dalam bingkai landscape, kamera
  aktif, dan lepas RMB mengembalikan aplikasi/perspektif sebelumnya. Ini berlaku juga saat HP
  dibuka dari Jurnal/Tas. Di Prolog, saat bersembunyi di lemari dan pada standoff tiga foto,
  kamera menjadi first-person sementara lalu memulihkan pilihan.
- **TAB** angkat/simpan HP, **Z** beralih aplikasi/kamera, **M** peta, **F** layar, **J** Jurnal,
  **B** powerbank, **G** flare, **H** kemenyan.
- **Q** AR, **R** UV, **X** Selfie, **T** ThermoKlenik, **C** EVP (pada Babak 3: C singkat
  mengganti analyzer, C ditahan merekam EVP), **LMB** foto/Blitz. Ketersediaan modul mengikuti
  babak; label aktual ada di HP dan Pengaturan. Selfie selalu **X**, bukan V (V = perspektif).
- **V** mengganti perspektif first-person/over-the-shoulder. Saat kamera terdesak dinding,
  tubuh disembunyikan sementara agar kepala/punggung tidak menutup pandangan. Pada pengejaran
  Kuntilanak di Babak 1, kamera tetap terkunci ke viewfinder; mundur sambil menjaga sosok dalam bingkai.
- **G** flare: **ditolak di Babak 4** ("Api asing ditolak desa", stok tidak berkurang, tetap
  terbawa ke Babak 5) dan **aktif lagi di Babak 5** (stok 0 → turunkan Lentera Panggung reusable).
- Settings menyediakan volume, sensitivitas, subtitle, reduksi glitch, pilihan kamera, mode layar,
  dan remap sembilan aksi utama. M/Z/ESC adalah shortcut UI tetap. F12 mengambil screenshot.
  Alt+Enter/Setelan mengganti fullscreen borderless/windowed; pilihan tersimpan antar-launch.

## Prolog — Kos–Atmaja

1. NEW GAME: bangun dari kasur, monolog Raka, **tiga ketukan sungguhan lalu langkah menjauh**.
   Tidak ada caption SFX atau suara ibu kos. Baca surat bawah pintu, lalu ambil HP:
   panggilan Damar langsung tersambung tanpa dokumen/tombol putar pesan. Setelahnya HP bergetar,
   dana tercatat di Bank; HP tidak dibuka paksa. Angkat HP untuk melihat banner notifikasi,
   buka Pesan sendiri, lalu tunggu 10 detik waktu permainan sebelum reaksi Raka.
   Ketukan harus keras dan Raka menoleh; surat baru masuk setelah ketukan. Sebelum pickup ada ringtone;
   sebelum telepon putus ada teriakan Damar. HP boleh tetap dibuka sambil WASD. Saat permainan biasa, tahan RMB untuk langsung
   membidik kamera meskipun menu HP sedang terbuka; HP terangkat dua tangan dalam bingkai landscape,
   lalu lepas RMB untuk kembali ke aplikasi/status HP sebelumnya.
   Cocokkan sticky note dan kalender untuk **empat roda tanggal/bulan**; petunjuk tidak mencetak jawaban.
   Ambil kunci yang terlihat di meja, powerbank/dompet/buku harian dari laci dan jaket di gantungan. Baca buku harian melalui Jurnal/Tas. Setelah menutup
   gembok/modal, WASD/E harus tetap bekerja; tidak ada daftar tugas di HUD atau Jurnal. Raka mulai dengan kaos; pengambilan jaket mengubah pakaian modelnya.
2. Pintu kamar harus menuju **lorong lantai 2**, dengan kamar penghuni lain tertutup.
   Turuni dua flight tangga, coba pintu dapur, ruang makan bersama dan dua bilik WC/kamar mandi
   terpisah PRIA/WANITA di bawah. Pintu luar kamar mandi bergeser pada rel.
   Periksa guardrail sisi dalam tangga; jangan sampai jatuh ke void. Buka pintu utama bangunan secara
   fisik, lalu berjalan ke teras, halaman dan motor/gerbang. Lihat kembali fasad/atap kos.
   Tidak boleh langsung pindah ke Atmaja. Kamar tertutup mempunyai lantai/dinding/plafon di belakangnya.
3. TAB angkat HP. Z beralih aplikasi/kamera; M membuka peta jalan dari mana saja.
   Kamera, Peta dengan pin Kos/Atmaja, Pesan, Jurnal, Tas, Bank, Telepon dan Setelan dapat diperiksa.
   Tas hanya barang, bukan saldo. Status HUD lama tidak boleh menumpuk di bezel/status bar HP.
   Telepon mempunyai Damar, Ibu dan Bu Haji Ratna. Sesudah donasi, hubungi Ibu dan Ratna:
   ada percakapan tentang biaya rumah sakit dan janji membayar kos. Setelah masing-masing panggilan,
   tagihan terkait terbuka di Bank (rumah sakit Rp1.500.000, kos Rp3.500.000).
   Pembayaran memakai konfirmasi, tersimpan, dan tidak bisa ditagih dua kali.
   Ini percakapan game lokal, bukan panggilan ke nomor dunia nyata.
   Tujuan lain masih terkunci. Berangkat ke Atmaja **hanya dekat motor/gerbang**, bukan dari kamar.
   Setelah pilih tujuan, lihat animasi mendekat/naik motor, tangan di setang, gerbang kos tetap terbuka,
   lampu depan menyorot jalan dari lensa motor, dan motor melaju melewati gerbang sebelum
   langsung menuju rumah Atmaja. Sebelum naik motor, perumahan Kampung Melati dapat dijelajahi dengan jalan kaki dari gerbang kos yang terbuka. Temui Bu Sri di balik konter warung untuk berdialog tentang Damar atau membeli bekal. Belanja memerlukan saldo yang cukup; membayar dua tagihan dahulu bisa menghabiskan dana. Barang yang dibeli masuk Tas dan tersimpan. Kembali ke motor kos untuk berangkat langsung ke Atmaja, atau ikuti penunjuk jalan di ujung kompleks. Kamera eksplorasi dimulai dengan over-the-shoulder; tekan **V** untuk
   berpindah ke first-person dan kembali, atau pilih di Settings. Raka memakai model
   berkerangka dengan pose jalan/lari/jongkok/HP/motor. Coba E pintu dalam kedua pandangan
   sambil aplikasi HP terbuka. Pilihan kamera tersimpan antar-scene dan antar-launch.
   Saat kamera terdesak dinding hingga terlalu dekat, tubuh disembunyikan sementara
   agar kepala/punggung tidak menutup pandangan; kembali terlihat ketika ada ruang.
4. Di Atmaja: E kedua daun gerbang; coba pintu depan yang macet dan lihat gerakan percobaannya.
   Catatan Damar ditemukan di dalam rumah, bukan di motor. Tidak perlu catatan untuk mulai live.
   TAB → Kamera → MULAI LIVE. Awalnya 14 penonton; TAB/F tidak memutus siaran.
   Lewat sisi kanan/timur; sisi kiri diblokir reruntuhan. Tahan CTRL melewati batang kamboja.
   Dekati jendela dapur dan hadap ke dalam, SPACE untuk vault kontekstual.
5. Jendela menutup; Raka memanggil Damar, lalu terdengar jeritan Damar dari lantai atas. Interior gelap:
   pakai cahaya HP. Sumur mempunyai foley dan bisikan; pandangan Raka menoleh ke sumbernya. E pada papan membuka sumur dan memicu adegan ditarik ke bawah, lalu menu utama. Foto celah papan
   opsional memberi bonus sah 150k sekali. Buka pintu dapur, lewati **samping** meja,
   bukan di atas meja. Kursi kayu harus menghadap meja dan tidak ditembus akar.
6. Merunduk di samping ransel bawah meja; arahkan ke layar retak, E ambil chip HP Damar.
   Perhatikan adegan pemindahan chip, progres instalasi MataBatin.apk, ikon aplikasi dan koneksi Kamera. Tutorial AR muncul sekali. Kursi ujung meja kosong di dunia biasa; perempuan berambut panjang dengan leher patah muncul dan menatap Raka melalui AR. Sosok gaib dan tulisan darah hanya di feed HP,
   bukan kamera dunia. Tulisan harus tampak merah dan menempel dinding, bukan teks putih melayang.
   F mematikan layar/kamera tambahan; broadcast tetap berjalan.
7. Buka ruang tamu depan. Ambil jarum kuningan di meja kecil pojok; telusuri lounge dan ceruk lemari terbuka.
   Arahkan AR ke tulisan dinding sampai tercatat di Jurnal. Jam menghadap ke ruang dan
   punggungnya menempel dinding; cek angka Romawi dan bunyi tik-tok saat mendekat.
   E jam; kontrol **jam/menit** (bukan gembok 3 digit). Waktu salah tidak membuka.
   Atur 03:45, pasang; baca koran 1998. HP yang sedang dipegang tetap terlihat dan
   kameranya tetap hidup ketika membaca catatan. Panel bacaan dan HP tidak bertumpuk. Isi dokumen lengkap otomatis tersimpan di Jurnal untuk dibaca lagi.
   Dari lounge tangga harus tersembunyi oleh dinding dan pintu kayu tertutup; tidak ada
   pintu ruang penyimpanan. Gunakan kunci: pintu berayun ke dalam, bukan menghilang ke lantai.
8. Naiki dua flight tangga berbidang utuh ke lantai 2, lalu coba turun kembali.
   Periksa anak tangga, bordes, penopang, pegangan dan benturan samping: tidak melayang,
   tidak tembus, tidak jatuh dari celah railing. Ikuti jejak tangan berdarah. Kamar anak terkunci: nyalakan
   AR dan merunduklah menembus celah ventilasi di balik lemari. Baca **buku harian Bima**,
   lalu potret cangkir susu yang retak dengan Kamera AR (rantai 1) — pintu kamar anak terbuka.
   Kamar mandi: buka tirai bak, potret cakaran jari kecil (rantai 2). Balkon dalam:
   baca surat wasiat Nyonya, lalu potret tali tambang yang berayun (rantai 3).
   Cermin kamar mandi menampilkan refleksi dan dapat memperlihatkan sosok di belakang Raka
   sekali; sosok itu tidak tampil ketika pemain menoleh ke dunia biasa. Pintu Kamar Tidur Utama terbuka setelah tiga rantai.
9. Kamar utama: angkat karpet merah, buka pintu jebakan kayu, turun ke ruang bawah tanah.
   Daya HP jatuh ke 12% sekali: ambil **Powerbank Besar** di meja jahit (minimal 80%;
   daya lama di atas 80 dipulihkan). Powerbank kecil bawaan tidak dihapus. Kabel di meja
   tetap bisa dipakai untuk pemulihan darurat ke 35% setelah daya habis.
   Basement memiliki tambahan lorong yang saling terhubung dan ruang untuk berlari. Nyonya Sri Atmaja bangun dan berpatroli — hindari garis pandang,
   sembunyi di lemari [E] dan **matikan layar HP [F]** saat dia memeriksa celah. Warning
   memberi 1,25 detik untuk merespons, lalu layar harus gelap selama empat detik.
   Setelah dia menjauh, tekan E sendiri untuk keluar; patroli harus tetap bergerak saat bersembunyi.
   Tertangkap membuka menu utama. Continue harus dipilih untuk memuat checkpoint, tanpa donasi ulang. Ambil **Handycam**
   di depan cermin besar (merangkak lewat kolong meja) dan putar rekaman terakhir Damar: pasar malam terlihat di balik cermin, tangan meraih
   lalu kamera ditarik masuk. Keluar game saat rekaman berlangsung harus tetap memungkinkan replay.
10. Chase: lari mengikuti rute — kembali ke tangga batu sudut basement, tahan SHIFT
    (atau E) untuk menaikinya. Di atas, merunduk/meluncur [CTRL] lewat pintu jebakan yang menutup.
    Turuni tangga tengah; lompati meja makan yang roboh dengan SPACE,
    lewati lampu yang pecah, lalu berhenti di jendela dapur. Slow-motion: **klik kiri**
    untuk mengambil **tiga foto** Nyonya. Arahkan lensa HP ke tubuhnya: bidikan yang
    meleset atau terhalang dinding tidak dihitung. Tahan RMB bila ingin membidik seluas layar
    dengan genggaman dua tangan; lepas RMB mengembalikan kamera sebelumnya.
11. Foto ketiga memicu Flash Beam, baterai menjadi 10%, dan Nyonya menghilang.
    Setelah flash selesai, tekan SPACE untuk menerobos jendela. Hantu yang sudah
    menghilang tidak boleh bergerak atau menangkap pemain lagi. Lari lewat halaman
    samping, merunduk di bawah kamboja, kemudian ke gerbang dan jalan raya.
    Ending: siluet di jendela lantai 2 saat kilat, 54.820 penonton, chat cepat yang berhenti,
    dan kontrak Penonton_Terakhir di HP. Pesannya bisa di-scroll; klik SIMPAN PESAN.
    Shutter → title **LAYAR TERAKHIR** → PROLOG SELESAI → MENU UTAMA. Total sawer
    Rp25.450.000 mencakup bonus sumur dan dana yang telah dipakai untuk tagihan, tanpa memberikan ulang pembayaran.
12. ESC → Menu Utama → Continue: harus kembali di checkpoint Atmaja dengan uang/item/baterai
    yang sama dan tanpa donasi ulang. Setelah tamat, Continue memulihkan layar PROLOG SELESAI.
    Coba pause/Settings saat kontrak akhir dan title; sesudah Resume pointer tombol harus tetap dapat dipakai.
    Save lama boleh dilanjutkan; gunakan NEW GAME untuk menguji pembuka dari awal.

Transisi ke Babak 1: di layar PROLOG SELESAI, tombol **MALAM BERIKUTNYA — RUSUNAWA** memulai
Babak 1. Tombol hanya muncul jika checkpoint berhasil disimpan; baterai diisi ke 100% sekali saat
transisi. Uang, inventaris, dan pilihan kamera tetap terbawa. Jika tombol tidak muncul karena save
gagal, gunakan **COBA SIMPAN LAGI**; Continue setelah tamat memulihkan layar PROLOG SELESAI ini.

## Babak 1 — Rusunawa Griya Asri

Masuk dari tombol Malam Berikutnya, atau jalankan **`Review-Phase1.cmd`** lalu **Continue**.
Transisi pertama setelah Prolog mengisi baterai menjadi 100% sekali; Continue/retry mempertahankan
snapshot dan tidak memberikan powerbank awal tambahan. Review-Phase1 memakai save uji terpisah
di `ReviewSaves/Phase1` dalam folder build dan hanya menyalin fixture jika folder belum memiliki save.

1. Periksa log satpam, mulai Live di aplikasi Kamera, temukan ruang pompa di sisi barat.
   Charger manual di pompa dapat dipakai bila baterai habis.
2. Scan tembok K102 dalam AR. Masuk melalui pintu, belok ke kaki tangga dan naik. Jalur menjadi
   terbuka dua arah. Hindari Tuyul lewat AR + Blitz atau terima kehilangan daya/item yang tersedia.
3. Kotak 207 memakai kode 644; panel status dan dokumen memberi petunjuk. Hadiah kunci/powerbank
   hanya satu kali. Periksa arsip penghuni dengan AR untuk rahasia Tari.
4. Gunakan lift barang. Di L3 tersedia panel daya sebelum cutter. Ambil alat sambil memantau
   Kuntilanak; mundur ke ujung timur, E rantai lalu naik tangga tertutup.
5. Di L4: foto cakaran 402, rekam EVP 7 detik di kamar mandi, foto aura AR tungku dan ambil keris.
   Gunakan lemari/bilik; layar menyala membocorkan persembunyian. Konfirmasi identitas di Jurnal.
6. Aktifkan altar 404. Pancing tubuh Genderuwo ke tiga titik halogen, gunakan tuas, Blitz ke kepala
   dalam 5 detik. Blitz 15% / cooldown 6 detik. Panel daya arena hanya aman saat recovery.
   Kalah memuat ulang CP5 persis, tanpa grant.
7. Setelah segel, periksa kain dan kaset. Transmisi Damar berlangsung di HP; bila daya habis gunakan
   panel arena lalu coba kembali. Setelah epilog, turun ke L3 lalu lift ke L1 atau L2 (opsional arsip).
   Motor menampilkan konfirmasi sebelum CP7. **Lanjutkan — Kamar Kos** membuka persiapan Babak 2.

Batas Babak 1: ini build review implementasi, belum visual/audio freeze. Dialog/EVP manusia belum
direkam; subtitle bukan pengganti acting final. Durasi 60 menit, kenyamanan input, framing karakter,
seluruh 74 QA, GPU Windows, target FPS/1% low dan variasi save perlu playtest Windows. Tes otomatis
serta gambar Linux tidak membuktikan semuanya.

Revisi visual: bangunan/furnitur/material memakai sumber unduhan terpisah. Periksa fasad, galeri,
tangga, area pompa, K403 dan dapur; bandingkan dengan `phase1-environment-revision.md`. Minta
pencahayaan tetap terbaca saat layar HP padam, periksa benturan perabot dan laporkan celah geometri.
Tambahan 30 model unduhan mempunyai sumber dan penempatan terpisah; periksa koridor, pintu, tangga,
keris, titik charging dan bilik sembunyi. Capture visual tersusun tidak membuktikan boss/CP7 atau
seluruh 74 QA telah lulus.

## Babak 2 — RSUD Lama Sido Waras

Alur normal: setelah segel Rusunawa, pilih **Lanjutkan — Kamar Kos**. Kos dipakai untuk persiapan
malam kedua: buka PC untuk riset dan pemasangan modul gratis, gunakan colokan (satu pengisian
persiapan setelah interaksi sadar), lalu berangkat dari kunci motor. Berangkat tidak otomatis
mengisi baterai. Paket Pasar Gaib opsional dan hanya memakai saldo sah.

Untuk review langsung, jalankan **`Review-Phase2.cmd`** lalu **Continue**. Launcher memakai slot
terpisah `Phase2Review`, dimulai dari fixture sesudah Babak 1 dengan saldo belanja nol agar jalur
tanpa belanja bisa dicoba. Ini bukan pemberian uang/segel pada save produksi, dan EXE biasa tetap
memakai campaign normal.

Lingkup: Kos pukul 14:00 → RSUD pukul 23:50 → IGD, Poli 03, basement/kamar mayat, Arsip,
Pos Petugas, Inkubator, Ruang Jaga Suster → ritual Lastri tiga siklus → epilog Damar →
kembali ke motor → transisi Pabrik Gula.

- Buku Jaga + jadwal + Thermo Poli 03 → kode basement. Laporan otopsi memberi kunci L2.
  C3 memerlukan pembacaan suhu stabil; Gunting dan foto aura AR merupakan dua progres terpisah.
  Arsip D1 opsional.
- Di L2, Inkubator dan Ruang Jaga Suster boleh didatangi dalam urutan bebas; kode brankas dapat
  disimpulkan dari catatan atau gema AR. EVP memerlukan framing sumber yang tepat selama enam detik.
  Tiga bukti perlu **konfirmasi eksplisit di Jurnal**, selain tiga pusaka. Bukaan AR yang sudah
  dilewati tetap terbuka dua arah saat baterai nol.
- Pintu Ruang Operasi terbuka ketika tiga pusaka dibawa. Ritual dimulai hanya lewat **E di meja**
  setelah tiga bukti dan konfirmasi Jurnal lengkap. CP5 menyimpan kondisi sebelum attempt.
- Saat persiapan (minimum 30 detik) dan jeda (minimum 20 detik), generator NE/SW memberi
  **+25 daya dengan tahan E selama 8 detik**, dapat diulang. Lanjut siklus hanya lewat E lagi di meja;
  mengangkat HP tidak memulai serangan. Tiga sumber LO/UV-N/UV-S masing-masing habis setelah satu
  hit sukses. Pancing Lastri ke bawah sorotan lalu E di sakelar. Saat **PERISAI HANCUR**, bidik kepala
  dan LMB dalam lima detik: Selfie atau balik badan dengan kamera belakang. G memakai flare opsional.
  Thermo membedakan asli −20°C dan ilusi −6°C. Jika daya tidak cukup untuk Blitz, ancaman masuk
  recovery dan generator dapat dipakai lagi.
- Lastri hanya terlihat pada feed Selfie. Saat menempel, pantau sambil benar-benar bergerak maju;
  menoleh langsung berbahaya setelah jendela belajar. Alternatif: E masuk persembunyian, F matikan
  layar, tunggu enam detik, E keluar. Baterai nol tetap bisa memakai persembunyian dan dinamo.
- Menu/jeda dan dokumen menghentikan clock investigasi; aplikasi HP biasa tetap membawa risiko.
- Kalah atau quit sebelum kemenangan memuat CP5 utuh; tidak menyimpan hit parsial atau memberikan
  isi baterai gratis. Setelah kemenangan, segel kedua dan perpindahan pusaka disimpan bersama.
  Video Damar berdurasi delapan detik memakai feed HP yang sama dan dapat diputar ulang melalui
  **Pesan → Putar arsip video Damar**, tanpa donasi atau progres tambahan.
- Setelah pesan selesai, kontrol kembali: turun ke motor atau kembali ke basement untuk arsip.
  Panel motor dapat dibatalkan. Hanya **Lanjutkan** yang menyimpan CP7 dan memutar CKREK + hitam.
  Dari menu utama, Continue membuka Kos persiapan Babak 3.

Batas Babak 2: model Lastri berasal dari **Merah.vrm milik desainer**, dengan konversi/optimasi dan
retarget gerak existing. Lingkungan memakai unduhan berlisensi; sumber dan atribusi ada di
`phase2-asset-credits.md`. Tidak memakai aset/kode DreadOut. Rekaman dialog manusia Indonesia untuk
Raka, Damar dan kalimat EVP belum tersedia: teks kanon disajikan sebagai subtitle dengan cue
nonverbal berlisensi. Model neonatal memakai inkubator kaca unduhan dengan material transparan;
freezer memakai modul penyimpanan logam unduhan yang diadaptasi. Properti gelang, rantai dan perekam
masih memerlukan penilaian visual desainer. Ini belum penerimaan mutu final setara DreadOut/Outlast.
Build berhasil dan directed smoke bukan pengganti seluruh 100 QA, playtest manusia, atau pengukuran
performa Windows pada GPU sasaran.

## Babak 3 — Pabrik Gula Sindang Laut

Alur normal: setelah menyelesaikan RSUD, tinggalkan lewat motor dan pilih **Lanjutkan**.
Dari menu utama, Continue membuka Kos persiapan Babak 3. Buka PC, baca forum, pasang modul gratis,
periksa toko, dan gunakan colokan yang mengisi 100% sekali untuk persiapan. Pintu/kunci motor
membuka keberangkatan 23:45; tiba 23:54. Melewatkan toko/forum tidak mengunci permainan.

Untuk review langsung, jalankan **`Review-Phase3.cmd`** lalu **Continue**. Launcher memakai slot
terpisah `Phase3Review`; slot awal adalah fixture P2 selesai, HP 37%, serta saldo uji Rp2.100.000
agar ketiga paket toko dapat diperiksa. Saldo fixture bukan hadiah pada campaign asli. Launcher
mempertahankan progres review yang sudah ada. Continue dari P2 yang benar memerlukan dua segel
sebelumnya; Babak 3 tidak menciptakannya sendiri.

1. Kos: buka PC, baca forum, pasang modul gratis, periksa toko. Colokan mengisi 100% sekali untuk
   persiapan. Pintu/kunci motor membuka keberangkatan 23:45; tiba 23:54.
2. Gilingan: suara kiri adalah tiruan. Silang UV menandai papan rapuh; jalur pipa kanan aman.
   Engkol G-ACC tiga kali, tunggu tiap aksi selesai. Foto ayunan dengan AR dan bidik stabil satu detik.
   Boneka serta buku mandor diambil terpisah.
3. Bangsal: tangga menuju kantor. Rekam interkom enam detik, termasuk minimal dua detik ASLI;
   mulai saat segmen ASLI agar mudah. Sabit diambil terpisah. Gudang ampas memiliki foto rajah UV
   dan selendang. Kartu pers serta sepatu Damar adalah dua interaksi terpisah. Dokumen yang diambil
   saat dikejar disimpan tanpa membuka modal.
4. Rahasia opsional: rekam ketukan ventilasi balkon, lihat petunjuk UV, tahan E pada katup 09,
   lalu periksa loker 09. Area ini dapat dikunjungi lagi setelah ritual.
5. Konfirmasi identitas di Jurnal setelah tiga bukti. Gerbang aula tetap menunggu tiga pusaka dan
   konfirmasi itu. Pasang ketiga dudukan, lalu periksa sosok terikat untuk memulai ritual.
6. Pancing Wewe ke sorotan N/S/E, nyalakan generator, lalu Blitz dada/wajah selama exposure lima detik.
   Lampu korslet hanya setelah hit sukses. Flare opsional. Pada tarikan terakhir, bidik simpul dengan
   UV satu detik atau gunakan sorotan yang masih tersedia.
7. Rak daya manual tersedia selama jeda. Jika jatahnya habis, masuk alcove, tutup pintu dari dalam,
   tahan engkol reset, lalu gunakan rak daya. Tidak perlu menang satu hit agar bisa pulih setelah miss.
8. Sesudah segel, tunggu pesan. Jika HP habis, gunakan pengisian manual. Jelajah kembali tetap tersedia.
    Gerbang servis Timur dan lorong lori mengarah ke motor; panel motor bisa dibatalkan. CP7 disimpan
    sebelum fade, dan menyimpan tujuan Karang Setra. **Lanjutkan — Kamar Kos** membuka persiapan Babak 4:
    Kos sore 16:45, buka PC (forum tragedi 1997), pasang modul Sanity + UV v2 gratis, toko opsional,
    colokan mengisi 100% sekali, lalu berangkat 23:45 dari kunci motor.

Batas Babak 3: periksa rasa kontrol, genggaman HP, tangga, keterbacaan UV, pengejaran/loker, luring
tiga lampu, serta rute pulang. Ukur FPS/1% low dan memori pada Windows/Iris Xe; render Linux software
bukan hasil performa mesin target. VO dialog Indonesia masih berupa subtitle, dengan cue/humming
berlisensi sebagai atmosfer. Pose sinematik menggenggam sepatu dan penyamaran Damar belum memenuhi
presentasi final naskah. Kesetaraan kualitas DreadOut belum dinyatakan lulus. Status setiap acceptance
case ada di `phase3-qa-matrix.md`; 146 baris bukan berarti 146 PASS. Sumber, pembuat, lisensi serta
perubahan konversi tercantum pada dua file kredit di paket.

## Babak 4 — Karang Setra (Bara di Karang Setra)

Alur normal: setelah segel Pabrik Gula, pilih **Lanjutkan — Kamar Kos**. Kos sore 16:45 dipakai
untuk persiapan malam keempat: baca forum tragedi 1997 di PC, pasang modul Sanity + UV Rajah v2
gratis, cek toko opsional (Kemenyan/Powerbank/Garam — semua opsional, jalur gratis tetap tamat),
gunakan colokan (satu pengisian 100% persiapan), lalu berangkat 23:45 dari kunci motor. Tiba di
Hutan Bambu 23:53. Sanity aktif pertama kali di babak ini; nilai awal = nilai save Babak 3
(tidak direset diam-diam).

Untuk review langsung, jalankan **`Review-Phase4.cmd`** lalu **Continue**. Launcher memakai save
terpisah `Phase4Review`; save kampanye utama tidak ditimpa. Fixture: Kos sore, baterai 37%,
Sanity 23, saldo 0 — untuk menguji jalur gratis.

Estimasi 78 menit jalur utama (bukan timer paksa).

1. Hutan Bambu — 3 simpang. Kabut menguras Sanity 1 poin/10 detik (tidak di bawah 35% hanya
   karena waktu). Chat disusupi akun hantu: J1 mengarahkan ke jalur kiri (batu nisan), J2 meniru
   username penonton (`@rizky_pratama_`), J3 menyamar sebagai `[PERINGATAN MATABATIN]`.
   Jangan percaya chat: sorot jalan dengan **UV [R]** — tengkorak ungu = bahaya, jejak kaki
   Damar = aman. Dinamo engkol darurat ada sebelum J1 (baterai 0 tetap bisa menyalakan UV).
   Sanity 70 = denyut tepi + bisik; 40 = Banaspati tertarik rasa takut; 0 = hunt intens tapi
   **bukan Game Over** — shelter fisik, shrine gratis, dan dinamo selalu bisa dicapai.
2. Pendopo. Ambil Kacamata Damar + Kitab Rajah di meja. Banaspati + arwah Mbah Suro reveal
   (100% kebal — jangan tembak, jangan pakai Flare). Tiga sektor investigasi bebas urutan:
   Utara/Kuburan Tua — **Thermo [T]** anomali **+66°C** stabil 2 detik → Bukti 1/3, ambil
   **Kendi** (Pusaka 1, kapasitas 3, isi ulang gratis di Sumur Barat); Timur/Gubuk — **AR [Q]**
   aura altar → Bukti 2/3, ambil **Belati** + baca Sobekan Buku Damar; Barat/Sumur — **EVP [C]**
   mantra 6 detik radius 3 m → Bukti 3/3, ambil **Kantong Tanah**. Buka Jurnal →
   **[KONFIRMASI IDENTITAS]** (wajib 3 bukti, eksplisit, tidak otomatis).
3. Rahasia opsional: Keping Cermin Saksi di relung kering Sumur Barat — matikan layar saat
   patroli lewat, rekam ketukan autentik (EVP 1,5 dtk), sorot simbol mata tertutup (UV),
   putar roda sumur, ambil dari laci. Tidak perlu menyiram obor. Jurnal menampilkan
   Arsip Babak 4 (0/1 atau 1/1) dan Arsip Global N/5 dari ID unik aktual.
4. Ritual: letakkan tiga pusaka di batu ritual, konfirmasi, interaksi batu mulai (CP5 tersimpan
   dulu — boss tidak mulai sebelum save sukses). Tiap penjuru N/E/W/S punya 3 obor
   (1 asli + 2 palsu; default N=Tengah, E=Kiri, W=Kanan, S=Kiri). **UV [R]** 2 detik: lingkar
   segel inti ungu = asli, silang merah = palsu. Siram obor asli dengan Air Kendi **[E]**,
   lalu **Blitz [LMB]** inti yang exposed (baterai ≥ 15, 5 detik jendela). N/E/W bebas urutan
   (tepat 3 Blitz sektoral); Selatan hanya disiram (tanpa Blitz lokal) setelah ketiganya sukses;
   inti gabungan di Pendopo → **Blitz keempat sekaligus terakhir**. Total tepat 4 Blitz —
   tidak ada tembakan kelima. Miss = obor menyala lagi, isi air ulang gratis, ulangi tanpa
   kehilangan sektor yang sudah committed (CP6 rolling, survive mati/reload).
5. Salah siram palsu = −40 Sanity + semburan telegraph (bukan instakill); Kendi isi ulang gratis.
   Kemenyan [H] = ke 100% (cooldown 12 dtk), opsional — shelter/shrine/dinamo gratis selalu ada.
   G ditekan = "Api asing ditolak desa", stok utuh untuk Babak 5.
6. Setelah Blitz inti: viewer 999.999, overlay `Rp999.999.999 [ERROR]` (bukan saldo),
   pesan `Penonton_Terakhir`, tangga batu terbuka di bawah Pendopo. Panel Arsip → pilih
   **Kembali Menjelajah** (ambil Cermin bila terlewat, boss tetap mati) atau **Turun ke Pasar
   Setan** (CP8 atomik, malam yang sama, tanpa Kos; baterai/Sanity/inventori terbawa apa adanya).
   P5 menyediakan charger + pemulihan di pintu masuk bila datang dengan 0%.

Batas Babak 4: dialog/EVP manusia belum direkam; subtitle + cue berlisensi bukan pengganti
acting final. Durasi 78 menit, framing UV 720p Low, dua-hit Banaspati, pacing shelter, seluruh
180 QA (`phase4-qa-matrix.md`), GPU Windows, dan target FPS/1% low perlu playtest manusia.
Tes otomatis serta ZIP yang berhasil dibuka tidak membuktikan 180 PASS.

## Babak 5 — Pasar Setan (Layar Terakhir)

Alur normal: dari CP8 Babak 4, turuni tangga batu — langsung di gerbang Pasar Setan malam yang
sama, tanpa Kos. Syarat masuk: `P4_COMPLETE` + 4 segel valid. Baterai/Sanity/inventori/segel
terbawa apa adanya; dinamo + Altar Doa gratis ada di gerbang (baterai/Sanity 0 tetap tidak
softlock di frame pertama).

Untuk review langsung, jalankan **`Review-Phase5.cmd`** lalu **Continue**. Launcher memakai slot
`Phase5Review` terpisah dengan fixture PHASE4_CP8 route native; save utama tidak diganti.
Untuk campaign sendiri, `LayarTerakhir.exe` → Continue dari CP8 sah.

Estimasi 75 menit jalur utama (bukan timer paksa). Satu campaign mengunci **satu ending**;
Continue sesudah crash mengulang cutscene yang sama; CP9 setelah selesai. Jangan edit JSON
untuk mencoba ending lain di save utama — tamatkan, lalu ulangi dari save relevan.

1. Gerbang + Kios. Viewer terkunci **1.000.000** (overlay), donasi Rp66.666.666 masuk dana
   terikat (bukan saldo belanja). Kios Tumbal opsional (saldo sah saja; Full Charge/Powerbank/
   Flare/Kemenyan — semua ada pengganti gratis: dinamo +25/8 dtk, Altar Doa +15/10 dtk,
   Lentera Panggung reusable). Kios Arsip Bisu (kanan, gratis, terpisah): matikan layar 3 detik
   di depan kursi → AR lihat laci → rekam EVP asli "Saksi tidak membeli korban" 6 detik →
   ambil **Pita Rekaman Pertama** [E]. Bukan bukti/boss; Jurnal tampil 0/1 atau 1/1 + N/5.
2. Sektor 1 — Koridor Beku (**Selfie [X]** × Thermo). Replika Kuntilanak menempel di punggung
   (1,3 m/s): tahan dalam frame Selfie 1 detik → jeda tatap 4 detik untuk pindah ke Thermo.
   Tiga Pintu Cermin: kiri/kanan **+50°C** jebakan, **tengah −22°C** asli (bidik stabil 2 detik
   tiap pintu). **Pintu Tengah −22°C wajib terverifikasi Thermo sebelum bisa dibuka** —
   menebak tanpa Thermo tidak diterima. Pintu panas = semburan telegraph + −25 Sanity, kembali
   ke awal koridor (bukan instakill).
3. Sektor 2 — Jembatan Gilingan (**EVP** × UV × Kendi). Kiri = merah bergerigi TIRUAN (Wewe,
   "menyerupai Damar"); kanan = hijau Morse ASLI (`R-A-K-A`). UV: papan Rajah Ungu tengkorak
   = lantai runtuh (jatuh ke anchor, −10). Satu Obor Santet menghalangi pintu: siram dengan
   Kendi carry-over Babak 4 (atau Cawan Air Pasar bila Kendi hilang) — titik isi ulang gratis
   di samping gerbang sektor. Chat serempak ke kiri boleh diabaikan; verifikasi dengan dunia.
4. Sektor 3 — Ruang Jahit Cermin (**[F]** × AR). Gelap total: **F** matikan layar + jongkok di
   lemari/kolong meja untuk memutus LOS (cahaya layar terdeteksi 12 m). Replika Sri Atmaja
   (1,4 m/s) + Genderuwo (1,8 m/s) patroli lorong 'U'. Cermin Rias hanya terbuka lewat **AR
   [Q]** (permukaan cair hijau) → tembus → Panggung. Kontak = −25 Sanity + knockback + i-frame
   3 dtk; dua kontak 10 detik = retry sektor (bukan seluruh babak); sektor clear tidak diulang.
5. Panggung Lelang. Damar di kurungan lensa; dialog bisa diulang. Tiga bukti bebas urutan:
   Server Batu + Kabel Urat (**AR** + foto 1%), Kontrak Darah (**Thermo `-- °C`** stabil 2 dtk
   **dan UV** Rajah Mata Terbuka — dua sub-pengamatan, satu ID), Pilar Layar (**EVP** bisikan
   Authentic 6 dtk radius 3 m). Buka Jurnal → **[KONFIRMASI]** (wajib 3 bukti + 4 segel,
   eksplisit). Ruang Cermin Saksi: cek Arsip N/5, Cermin Jalan Pulang ke gerbang (ambil Pita
   bila terlewat). Panel arena memberi peringatan "Arena akan terkunci" — CP6 tersimpan dulu,
   boss tidak mulai sebelum save sukses; Cermin Jalan Pulang menutup sesudahnya.
6. Boss — `Penonton_Terakhir`, 3 inti, tepat **3 Blitz inti** (bukan HP bar; hanya rentan di
   jendela). Fase 1: 4 sosok — UV cari **benang kontinu berdenyut ke kurungan** (bayangan =
   putus-putus, sentuh −15 lalu larut); pancing asli ke Zona Generator, nyalakan sakelar,
   Blitz kepala lensa (jendela 5 dtk; miss = cooldown generator 12 dtk, ulangi). Fase 2:
   arena gelap; Selfie [X] lihat rahang terbuka di belakang + Thermo **−30°C** ≤2,2 m;
   jatuhkan **Flare [G]** (stok 0 → Lentera Panggung, reusable) lalu Blitz via Selfie.
   Fase 3: 3 gelombang api (telegraph 2 dtk, berlindung di peti — cover dihitung collider/LOS,
   bukan invulnerability); AR lihat 3 Kabel Roh, tahan **E** 2 dtk tiap kabel; kurungan terbuka
   → Blitz inti dada → berlutut → FINAL_CHOICE (CP8). Mati = mulai awal fase berjalan, inti/
   kabel committed (CP7 rolling) tidak diulang.
7. FINAL_CHOICE — tanpa timer fatal. Angka 5% = **Kapasitor Ritual** (satu aksi LMB), bukan
   baterai persisten (tidak diubah diam-diam). Tiga aksi dasar selalu tersedia; Secret hanya
   bila 5 ID Arsip unik + validasi Cermin Saksi:
   - **BAD** "Tuan Rumah Baru": kamera belakang ke Damar, dwell kontrak 2 dtk sampai label
     TUMBAL tampil, **LMB**. Epilog kos-studio + Rp500.000.000 (sinematik, bukan wallet).
   - **NORMAL** "Siaran Terputus": ke tepi panggung, **tahan F 5 detik penuh** (lepas = batal),
     tanpa LMB. Raka tertinggal; Damar + penonton selamat.
   - **TRUE** "Jepretan Terakhir": papah Damar (E) ke portal → **X** Selfie → **Q** AR stabil
     1,5 dtk → **R** UV stabil 1,5 dtk (urutan wajib; salah = gauge reset) → LMB inti di layar
     Selfie. Epilog 05:30 + Rp125.000.000 donasi tulus (narasi).
   - **SECRET** "Saksi Tanpa Layar": 5 arsip + validasi cermin + Damar di portal → **tahan E
     3 dtk** di Cermin Saksi (mode Arsip) → LMB cermin berbidikan emas (Damar bukan target).
     Nama korban dari data arsip kanon; tidak ada nama baru.
   Input ending hanya hidup di state FINAL_CHOICE; dua cutscene tidak pernah bersamaan
   (satu enum + kunci atomik).

Batas Babak 5: subtitle/ambience/SFX tersedia; VO manusia belum final. Penilaian artistik,
durasi 75 menit, seluruh 180 QA (`phase5-qa-matrix.md`), dan FPS GPU Windows tetap perlu
playtest manusia. Screenshot dev F12; cantumkan lokasi, checkpoint, baterai/Sanity/air,
input (OTS/FP), dan langkah reproduksi agar bug dapat ditelusuri.

## Batas umum dan pelaporan

Build ini adalah review implementasi, bukan visual/audio freeze. Voice acting manusia belum final;
dialog memakai suara sintetis/subtitle. Beberapa model, properti, animasi sinematik, dan pose
genggaman HP masih prototipe dan memerlukan penilaian visual desainer. Tes otomatis, directed smoke,
dan capture Linux tidak membuktikan seluruh QA, playtest manusia, atau performa Windows.

Catat checkpoint, baterai/Sanity/air, mode kamera (OTS/FP), lokasi, dan langkah reproduksi
saat menemukan masalah. Sertakan `Player.log` dan screenshot F12. Bandingkan performa pada
Windows Iris Xe 1280×720; Linux software-rendering bukan bukti performa target. Untuk detail
acceptance per babak, gunakan `phase1-windows-playtest.md`, `phase2-session1-review.md`,
`phase3-windows-review.md`, `phase4-windows-review.md`, `phase5-windows-review.md`, dan
`phase3/phase4/phase5-qa-matrix.md`; untuk kredit aset gunakan file kredit yang disertakan di paket.

## Format laporan run tamat (isi setelah main sampai credits)

Salin template ini per babak + ending, lalu kirim sebagai review:

- Babak + checkpoint terakhir (mis. PHASE4_CP6 / PHASE5_CP8 / CP9 + ending):
- Baterai / Sanity / air Kendi saat momen penting:
- Kamera (OTS/FP), modul yang dipakai (AR/UV/Thermo/EVP/Selfie), Flare/Kemenyan bawa-berapa:
- Rute yang diambil (urutan sektor N/E/W, urutan bukti, urutan inti), rahasia dapat/terlewat (N/5):
- Ending yang dicapai (Bad/Normal/True/Secret) + cara memicu yang dilakukan:
- Bug/softlock (lokasi persis, langkah reproduksi, `Player.log`, screenshot F12):
- Performa (GPU/CPU/RAM, resolusi, preset, FPS rata-rata + 1% low bila sempat ukur):
- Cerita/pacing/emosi/VO per babak (bebas, jujur — bagian ini murni selera desainer):
- Skor sementara per babak (opsional, 1–10) + satu hal yang paling mengganggu:
