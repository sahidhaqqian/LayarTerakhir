# Guideline Playtest Lengkap — Prolog hingga Babak 3
Diperbarui 24 Oktober 2026.

Build Development ini memuat Prolog, Babak 1 Rusunawa, Babak 2 RSUD, dan Babak 3 Pabrik Gula.
Dokumen ini menggabungkan rute playtest keempat bagian. File `phase1-windows-playtest.md`,
`phase2-session1-review.md`, dan `phase3-windows-review.md` tetap disertakan sebagai rujukan terpisah.

## Persiapan

Jalankan `LayarTerakhir.exe` tanpa flag audit. Main menu: NEW GAME (save lama diarsipkan),
CONTINUE (checkpoint terakhir), SETTINGS, EXIT. ESC saat bermain membuka pause menu.
Jangan menghapus save untuk mengulang: gunakan NEW GAME. Arsip tetap ada di ArchivedCampaigns.

Alur normal memakai satu campaign dan checkpoint yang sama dari Prolog sampai Babak 3.
Kehilangan fokus jendela saat bermain otomatis membuka pause; Alt-Tab tidak membiarkan
karakter berjalan sendiri.

Untuk menilai satu babak tanpa mengulang dari awal, jalankan launcher review yang sesuai.
Launcher memakai slot save terpisah dan tidak mengubah campaign normal:
- `Review-Phase1.cmd`: `ReviewSaves/Phase1`, fixture 100% baterai tanpa powerbank awal.
- `Review-Phase2.cmd`: slot `Phase2Review`, fixture sesudah Babak 1 dengan saldo belanja nol.
- `Review-Phase3.cmd`: slot `Phase3Review`, fixture P2 selesai, HP 37%, saldo uji Rp2.100.000;
  saldo fixture bukan hadiah pada campaign asli.

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
  babak; label aktual ada di HP dan Pengaturan.
- **V** mengganti perspektif first-person/over-the-shoulder. Saat kamera terdesak dinding,
  tubuh disembunyikan sementara agar kepala/punggung tidak menutup pandangan. Pada pengejaran
  Kuntilanak di Babak 1, kamera tetap terkunci ke viewfinder; mundur sambil menjaga sosok dalam bingkai.
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
   sebelum fade, dan menyimpan tujuan Karang Setra. **Babak 4 belum playable.**

Batas Babak 3: periksa rasa kontrol, genggaman HP, tangga, keterbacaan UV, pengejaran/loker, luring
tiga lampu, serta rute pulang. Ukur FPS/1% low dan memori pada Windows/Iris Xe; render Linux software
bukan hasil performa mesin target. VO dialog Indonesia masih berupa subtitle, dengan cue/humming
berlisensi sebagai atmosfer. Pose sinematik menggenggam sepatu dan penyamaran Damar belum memenuhi
presentasi final naskah. Kesetaraan kualitas DreadOut belum dinyatakan lulus. Status setiap acceptance
case ada di `phase3-qa-matrix.md`; 146 baris bukan berarti 146 PASS. Sumber, pembuat, lisensi serta
perubahan konversi tercantum pada dua file kredit di paket.

## Batas umum dan pelaporan

Build ini adalah review implementasi, bukan visual/audio freeze. Voice acting manusia belum final;
dialog memakai suara sintetis/subtitle. Beberapa model, properti, animasi sinematik, dan pose
genggaman HP masih prototipe dan memerlukan penilaian visual desainer. Tes otomatis, directed smoke,
dan capture Linux tidak membuktikan seluruh QA, playtest manusia, atau performa Windows.

Catat checkpoint, baterai, mode kamera, lokasi, dan langkah reproduksi saat menemukan masalah.
Sertakan `Player.log` dan screenshot F12. Bandingkan performa pada Windows Iris Xe 1280×720;
Linux software-rendering bukan bukti performa target. Untuk detail acceptance per babak, gunakan
`phase1-windows-playtest.md`, `phase2-session1-review.md`, `phase3-windows-review.md`, dan
`phase3-qa-matrix.md`; untuk kredit aset gunakan file kredit yang disertakan di paket.
