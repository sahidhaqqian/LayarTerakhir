# Rancangan Gameplay Layar Terakhir

Revisi 15 Oktober 2026. Dokumen ini menetapkan urutan cerita, aturan HP, prasyarat ritual, dan sistem collectible yang digunakan semua naskah serta GDD.

## Urutan cerita dan progresi

| Babak | Lokasi | Boss | Menit |
| --- | --- | --- | --- |
| 0 | Rumah Keluarga Atmaja | Nyonya Sri Atmaja | 90 |
| 1 | Rusunawa Griya Asri | Genderuwo Arang | 60 |
| 2 | RSUD Lama Sido Waras | Kuntilanak Merah Lastri | 82 |
| 3 | Pabrik Gula Belanda Sindang Laut | Wewe Gombel | 75 |
| 4 | Desa Mati Karang Setra | Banaspati dan arwah Mbah Suro | 78 |
| 5 | Pasar Setan | Penonton_Terakhir | 75 |

## Aturan bersama untuk semua babak

File Phase1–Phase5 adalah babak cerita. Di dalam setiap babak, Fase Gameplay A berarti eksplorasi dan puzzle, Fase Gameplay B berarti investigasi dan survival, dan Fase Gameplay C berarti eksorsisme. Tahap persiapan di kos adalah pembuka terpisah. Prolog mengajarkan eksplorasi, puzzle, stealth, dan pelarian; tidak menghasilkan segel boss. Babak 5 mempunyai tiga tahap boss A–C di dalam bagian klimaksnya.

Lima gerbang Panca-Sengkala berada di Rusunawa, RSUD, Pabrik Gula, Karang Setra, dan Pasar Setan. Rumah Atmaja adalah pintu penculikan yang terhubung ke jaringan, bukan gerbang keenam. Babak 1–4 menghasilkan empat segel lokasi; gerbang kelima terbuka di akhir Babak 4. Istilah segel lokasi berbeda dari tiga atau empat ikatan yang dihancurkan dalam satu boss fight.

Kontrol baku: WASD bergerak, SHIFT sprint, CTRL crouch atau slide saat sprint, SPACE vault, E interaksi dunia, TAB angkat/simpan HP, F tekan singkat mematikan/menyalakan layar, Q AR, R UV, X kamera Selfie, V OTS/FP, T ThermoKlenik, C EVP, J Jurnal, B powerbank, G flare, H kemenyan, dan LMB foto atau Blitz sesuai mode bidikan yang tertulis. Semua tombol dapat dipetakan ulang. Interaksi E tidak membuka ThermoKlenik.

AR dan UV memakai satu slot pemindaian visual dan saling menggantikan pada gameplay biasa. Kamera depan/belakang adalah pilihan terpisah; suhu dan EVP dapat muncul sebagai overlay. Kombinasi AR dan UV pada ending adalah mode sinkronisasi ritual khusus yang diaktifkan berurutan, bukan menekan tiga tombol bersamaan.

Mematikan layar atau menyimpan HP menghemat baterai dan menyembunyikan cahaya, sementara koneksi siaran masih berjalan dalam mode gelap. Tombol F tahan 5 detik hanya memutus siaran pada state pilihan ending. Dengan demikian stealth tidak secara otomatis melanggar ancaman Penonton_Terakhir.

Identifikasi boss membutuhkan tiga bukti unik yang dipindai, konfirmasi Jurnal, dan pusaka sesuai babak. Sebelum itu boss kebal terhadap damage penyegelan. Stun darurat tidak mengurangi segel; Banaspati tidak dapat distun dengan flare. Saat cahaya ritual membuka perisai, jendela Blitz adalah 5 detik. Jika gagal, alat arena pulih setelah cooldown; sumber cahaya baru habis setelah satu ikatan berhasil dihancurkan.

Angka baterai untuk prototipe: kamera biasa 1% per 20 detik, AR atau UV 1% per 10 detik, foto bukti 1%, Blitz 15% dengan cooldown 6 detik, dan powerbank besar menambah 80 poin hingga maksimum 100. Overlay Selfie, suhu, dan EVP memakai profil mode aktif, tanpa penjumlahan drain tersembunyi. Layar mati menghentikan drain untuk kejelasan prototipe. Semua angka merupakan parameter tuning yang perlu playtest.

Jalur wajib selalu menyediakan isi ulang tetap. Arena menyediakan titik daya darurat yang terjangkau saat boss recovery agar kegagalan Blitz tidak menyebabkan softlock. Tuyul tidak mengambil pusaka, kunci, item rahasia, atau daya terakhir yang diperlukan tutorial. Battery 0 mengaktifkan mode layar mati, bukan Game Over otomatis. Finale menyimpan 5% dalam kapasitor ritual; tindakan akhir memakai biaya khusus 5%, berbeda dari Blitz normal.

Sanity dipantau sebagai nilai 0–100 mulai Babak 4; barang kemenyan yang ditemukan lebih awal disimpan untuk digunakan di sana. Kabut berbahaya menguras 1 poin per 10 detik, area netral menghentikan drain, dan kemenyan memulihkan ke 100 saat interaksi selesai. Pada 70 muncul gangguan, di bawah 40 Banaspati lebih agresif. Sanity berbeda dari HP: sistem luka terpisah tidak menjadi kebutuhan prototipe. Jebakan obor palsu mengurangi 40 Sanity dan memberi jeda pemulihan; serangan langsung boss tetap dapat membunuh.

Siaran, chat, viewer, dan saweran seluruhnya simulasi lokal. Petunjuk wajib tersedia pada objek atau Jurnal meskipun chat belum terbaca. Warna dilengkapi simbol dan teks. Efek glitch dan kilatan dapat dikurangi tanpa menghilangkan petunjuk. Kamera kedua dirender hanya saat layar HP aktif; cermin memakai render berbatas dan efek visual, tanpa rekursi kamera tanpa batas.

Saldo naratif kumulatif tidak sama dengan uang belanja tersedia. Pisahkan donasi manusia yang sah, dana kontrak terikat Rp500.000.000, dan pengeluaran. Uang muka Rp5.000.000 termasuk kontrak, sehingga sisa janji awal Rp495.000.000. Angka Rp25.450.000, Rp78.200.000, Rp164.800.000, dan Rp312.500.000 adalah capaian kumulatif siaran, bukan saldo yang direset atau dijumlah ulang. Semua belanja wajib dan modul cerita dijamin tersedia meski pemain melewatkan saweran opsional. Rp125.000.000 pada epilog True dan Secret adalah sisa donasi manusia sah setelah pengeluaran dan pembatalan dana terikat.

Kos Raka dan lokasi ekspedisi berada pada wilayah kota fiksi yang mengambil suasana Indonesia; nama dan jarak lokasi bukan rekonstruksi tempat nyata. Pada lore, Mbah Suro memulai ritual desa 1997, kemudian terkait Atmaja 1998, pabrik 1999, RSUD 2001, dan Rusunawa 2004. Ia wafat setelah ritual terakhir; arwahnya kembali terikat ke desa.

## Jalur item rahasia dan ending keempat

Secret Ending bernama **Saksi Tanpa Layar**. Satu item rahasia tersedia di masing-masing Babak 1–5; Prolog hanya memberi petunjuk simbol mata tertutup, tanpa collectible wajib tambahan. Lima item merupakan **Arsip Saksi**, terpisah dari tiga bukti identifikasi, pusaka eksorsisme, dan empat segel lokasi. Pemain dapat menamatkan tiga ending dasar tanpa mengambil item rahasia.

Setiap item memberi satu potongan kesaksian korban dan ikon di halaman Arsip Saksi pada Jurnal. Mengambil kelima ID unik dalam campaign yang sama membuka jalur Secret Ending. Membaca catatan memberi konteks lore dan tidak menjadi syarat tambahan. Pemain menyiapkan ritual dengan memvalidasi arsip pada Cermin Saksi di ruang sebelum panggung lelang; persiapan tidak otomatis memilih ending. Jika persiapan terlewat, validasi tetap dapat dilakukan pada cermin yang sama saat pilihan akhir.

Sebelum meninggalkan tiap babak, Jurnal menampilkan status Arsip Saksi 0/1 atau 1/1 dan lokasi yang masih dapat diperiksa. Pintu keluar memerlukan interaksi sadar; item tetap bisa diambil selama pemain belum menyelesaikan transisi. Melewatkan item babak terdahulu menutup secret pada campaign itu sampai babak tersebut dimainkan ulang melalui save yang relevan. Chapter select merupakan pengembangan lanjutan, bukan syarat MVP.

Pengambilan item langsung menyimpan ID unik pada campaign secara atomik. Reload checkpoint dan kematian tidak menghapus collectible yang telah disimpan. Objek yang sudah diambil tidak memberi duplikat atau uang. New Game memulai himpunan kosong. Jumlah 5 tanpa lima ID yang benar tidak cukup untuk unlock. Membaca catatan disimpan terpisah; validasi arsip dapat diulang setelah reload.

Setelah inti ketiga boss pecah, bantu Damar ke portal dengan E. Jika arsip sudah tervalidasi, Cermin Saksi memiliki simbol mata tertutup dan lima slot lengkap. Tahan E selama 3 detik pada cermin untuk memutar seluruh kesaksian; progress terlihat dan dapat dibatalkan sebelum selesai. Kamera berpindah ke mode Arsip Saksi. Arahkan ke cermin berbidikan emas dan tekan LMB untuk merekam nama korban dengan sisa daya ritual 5%. Target Damar tidak valid dalam mode ini. Aksi tersebut mengunci Secret Ending tepat satu kali.

Lima kesaksian membatalkan kontrak yang menganggap semua korban dan penonton menyerahkan diri secara sukarela. Wajah-wajah di cermin mendapatkan nama, berhenti menatap siaran, dan berubah menjadi bayangan manusia yang keluar melalui pintu pasar. Penonton_Terakhir kehilangan ikatan terhadap arsip korban; Pasar Setan runtuh menjadi lapangan pasar kosong. Raka dan Damar pulang bersama dan penonton sadar. Adegan tambahan menunjukkan arwah Tari, Lastri dan anaknya, sembilan anak buruh, serta warga Karang Setra meninggalkan jaringan ritual. True Ending tetap menyelamatkan Raka, Damar, dan penonton serta menghancurkan jaringan aktif; Secret memperluas penyelesaian ke korban lama yang terkurung dalam arsip.

Dialog klimaks: Raka berkata, "Mereka bukan angka penonton. Mereka punya nama." Damar menjawab, "Rekam kesaksiannya, Ka. Biar kali ini mereka yang pulang." Epilog memperlihatkan keduanya menyerahkan catatan korban kepada keluarga, dengan sisa donasi sah Rp125.000.000. HP tidak lagi memuat MataBatin.apk. Shot terakhir menampilkan kursi penonton kosong dan cermin biasa tanpa wajah yang tertinggal.

Pemicu ending dasar tetap tersedia bila secret terbuka. BAD: bidik Damar dalam kamera belakang, tahan bidikan kontrak 2 detik lalu LMB setelah label TUMBAL muncul. NORMAL: tahan F 5 detik untuk memutus siaran dan menghancurkan HP; Damar dan penonton selamat, Raka tertinggal. TRUE: papah Damar ke portal, pilih X (Selfie), aktifkan Q lalu R untuk mengisi kalibrasi sinkronisasi ritual, kemudian LMB pada inti entitas yang terlihat di Selfie; Raka, Damar, dan penonton selamat. SECRET: lima item lengkap, validasi Cermin Saksi, tahan E di cermin lalu LMB pada reticle emas. Input ending hanya diterima pada state FINAL_CHOICE; sebelum itu berlaku kontrol biasa. Ending memakai satu enum tunggal dan kunci transisi atomik agar dua cutscene tidak terpicu bersamaan.

| Babak | Item rahasia | ID unik |
| --- | --- | --- |
| 1 | Negatif Foto Tari | SECRET_P1_NEGATIF_TARI |
| 2 | Label Pasien Tanpa Nama | SECRET_P2_LABEL_PASIEN |
| 3 | Pelat Nomor Buruh 09 | SECRET_P3_PELAT_BURUH |
| 4 | Keping Cermin Saksi | SECRET_P4_CERMIN_SAKSI |
| 5 | Pita Rekaman Pertama | SECRET_P5_PITA_PERTAMA |

## Urutan produksi dan batas lingkup

Bangun satu vertical slice Prolog hingga pelarian terlebih dahulu. Jalur kerja berbasis hasil: controller dan interaksi → HP/AR/baterai → puzzle dan Jurnal → AI stealth dan chase → save/checkpoint serta chat → satu level prolog lengkap. Setelah slice stabil, buat Babak 1 untuk menguji sistem eksorsisme dan satu collectible end to end, lalu tambah modul Babak 2, 3, 4, dan integrasi Babak 5.

Definisi selesai satu babak: dapat dimainkan dari awal sampai transisi, bukti dan pusaka tidak menduplikasi, checkpoint memulihkan keadaan dengan benar, semua trigger punya petunjuk, boss bisa diselesaikan setelah satu percobaan gagal, dan collectible babak tersimpan. Audio placeholder serta greybox diperbolehkan pada milestone pertama; penggantian aset mengikuti fungsi area yang telah terbukti.

Scope prototipe: first person, satu HP, satu profil kesulitan, level cerita tetap, satu varian puzzle per babak, empat ending, lima item rahasia. Engine belum dikunci; spesifikasi menggunakan sistem generik yang bisa dipetakan ke scene/node Godot atau Fyrox. Multiplayer, siaran internet nyata, chat AI daring, prosedural tanpa batas, chapter select, dan peta desa open world penuh berada di backlog.

Jalur utama naskah berjumlah 460 menit atau 7 jam 40 menit. Tambahan pencarian lima item sekitar 15–30 menit, belum termasuk retry dan eksplorasi bebas. Angka 10–30 jam dari konsep lama tidak menjadi janji durasi versi ini. Durasi setiap adegan adalah target pacing, bukan timer yang memaksa pemain gagal. Target performa sementara 60 fps pada preset sasaran; spesifikasi perangkat minimum baru ditetapkan setelah profiling vertical slice.

## Kontrak data dan pemulihan

Campaign menyimpan `chapter_id`, `checkpoint_id`, `evidence_ids`, `relic_ids`, `location_seals`, `upgrades`, `inventory`, `battery`, `sanity`, `wallet_legitimate`, `wallet_bound`, `spent`, `secret_items`, `secret_notes_read`, `archive_prepared`, `ending`, dan `schema_version`. Posisi dan arena menyimpan keadaan lokal sesuai checkpoint. Collectible unik yang sudah autosave dipertahankan ketika geometry direset; uang dan consumable kembali sesuai snapshot sehingga reload tidak menggandakan pembelian atau hadiah.

Event cerita menggunakan ID unik dan flag sekali jalan: `EVIDENCE_CAPTURED`, `RELIC_COLLECTED`, `JOURNAL_CONFIRMED`, `RITUAL_STARTED`, `SHIELD_OPEN`, `SEAL_HIT`, `SECRET_COLLECTED`, `SECRET_NOTE_READ`, `ARCHIVE_PREPARED`, dan `ENDING_COMMITTED`. Pemberian segel lokasi terjadi setelah boss selesai, bukan setiap Blitz. Simpan ke file sementara lalu ganti secara atomik; transaksi collectible memerlukan konfirmasi sukses sebelum indikator ditemukan ditampilkan.

Unlock secret: himpunan `secret_items` memuat lima ID yang tercantum pada tabel. Pemicu cutscene juga memerlukan `archive_prepared` benar, tiga inti final telah pecah, Damar sudah dibantu ke portal, dan `ending` belum terisi. Catatan yang dibaca tidak memengaruhi unlock. Perubahan `ending` memakai pemeriksaan state tunggal agar input serentak tidak memainkan dua ending. Jika file simpan rusak, tampilkan pilihan snapshot terakhir; jangan membuka secret berdasarkan hitungan palsu.

## Revisi pencahayaan bangunan terbengkalai — 26 Oktober 2026

Rumah Atmaja dan lokasi Babak 1–5 tidak memiliki lampu listrik dekoratif aktif. HP menjadi sumber penerangan utama untuk menyelidiki ruangan. Cahaya bulan tipis, api/lentera, pemulihan gratis existing dan lampu yang benar-benar diaktifkan oleh puzzle/ritual tetap tersedia agar progres tidak terkunci ketika baterai habis. Kos dan lingkungan yang masih berpenghuni serta adegan fajar/epilog mengikuti kondisi ceritanya. Revisi ini tidak mengubah konsumsi baterai, aturan puzzle, checkpoint, atau ending.
