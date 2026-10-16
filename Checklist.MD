# Master Checklist Produksi Layar Terakhir

Revisi 15 Oktober 2026. Setiap kotak adalah pekerjaan yang belum ditandai selesai, bukan klaim fitur telah diimplementasikan. Nomor Babak mengikuti Phase1–Phase5. Acuan aturan: Rancangan.MD; acuan dialog dan layout: Prolog.MD serta Phase terkait.

## Ringkasan babak

| Babak | Lokasi | Boss | Menit |
| --- | --- | --- | --- |
| 0 | Rumah Keluarga Atmaja | Nyonya Sri Atmaja | 90 |
| 1 | Rusunawa Griya Asri | Genderuwo Arang | 60 |
| 2 | RSUD Lama Sido Waras | Kuntilanak Merah Lastri | 82 |
| 3 | Pabrik Gula Belanda Sindang Laut | Wewe Gombel | 75 |
| 4 | Desa Mati Karang Setra | Banaspati dan arwah Mbah Suro | 78 |
| 5 | Pasar Setan | Penonton_Terakhir | 75 |

## Sistem bersama

- [ ] **CORE-01 Controller FPP dan interaksi** — WASD, sprint, crouch, slide, vault; pintu, drawer, panel, hiding spot modular; input dapat dipetakan ulang.
- [ ] **CORE-02 Smartphone** — Satu rig tangan/HP; layar terpisah dari koneksi siaran; kamera belakang/Selfie; AR/UV eksklusif; overlay suhu/EVP.
- [ ] **CORE-03 Daya dan persediaan** — Drain profil, foto 1%, Blitz 15%, powerbank +80, warning daya rendah, titik recharge arena, kapasitor finale 5%.
- [ ] **CORE-04 Bukti dan Jurnal** — Tiga ID bukti unik; validasi pusaka; reticle foto/Blitz jelas; konfirmasi identitas; halaman Arsip Saksi.
- [ ] **CORE-05 AI dan eksorsisme** — Patrol, investigate, chase, hiding check, recovery, shielded, exposed, sealed; telegraph sebelum serangan; satu event per hit.
- [ ] **CORE-06 Siaran dan ekonomi** — Chat/viewer lokal berbasis event; petunjuk objek cadangan; wallet sah/terikat, transaksi idempotent, saweran opsional.
- [ ] **CORE-07 Save dan checkpoint** — Snapshot inventori/arena, collectible atomik, schema_version, recovery snapshot; event replay tidak memberi hadiah dua kali.
- [ ] **CORE-08 Sanity dan aksesibilitas** — Sanity bukan HP; drain zona, kemenyan, area netral; subtitle; warna+simbol; pengurangan glitch/kilatan.
- [ ] **SEC-00 Arsip Saksi dan empat ending** — Lima ID unik membuka jalur secret; catatan lore opsional, validasi cermin sebelum boss atau saat finale, enum ending tunggal, cutscene empat cabang.

## Prolog Rumah Atmaja

- [ ] **P0-SYS** — Puzzle laci empat roda 1408 (14-08, revisi playtest 16 Oktober), jam 03:45, tiga foto tragedi, unlock AR, tutorial matikan layar, daya 12% sekali, grand chase dan Blitz 4 detik.
- [ ] **P0-3D** — Kos 4×3 meter, rumah dua lantai, dapur/sumur, koridor lukisan, basement U, dua lemari, meja kolong, cermin, rig Nyonya Sri Atmaja dan gunting.
- [ ] **P0-AV** — Surat kos tanpa VO ibu kos, panggilan Damar otomatis di HP, pesan/dana di Pesan/Bank, catatan 1998, chat per trigger, hujan/derit/gesekan gunting, title drop, simbol mata tertutup pada bingkai.
- [ ] **P0-QA** — Jalur prolog selesai tanpa collectible; puzzle tidak memakai Blitz mahal; fail chase memulihkan checkpoint dan sumber daya.

## Babak 1 Rusunawa Griya Asri

- [ ] **P1-SYS** — AR penuh, Jurnal Klenik, EVP Recorder dasar, halogen koridor; Tuyul dan Kuntilanak Putih sebagai ancaman pendukung.
- [ ] **P1-LEVEL** — Parkir dan pos satpam → ruang pompa → AR Kamar 102 → kotak surat 207 (kode 644) → lift barang → koridor Kuntilanak Putih → empat ruangan Lantai 4 → altar Kamar 404.
- [ ] **P1-EVID** — Cakaran Kamar 402; rekaman EVP kamar mandi umum; aura Pecahan Keris Penumbal di tungku dapur. Pusaka: Pecahan Keris Penumbal. Babak ini sengaja memakai satu pusaka utama sebagai pengenalan; tiga bukti tetap wajib.
- [ ] **P1-BOSS** — Pasang keris pada altar. Pancing Genderuwo ke halogen Utara, Timur, dan Barat. Cahaya memecahkan perisai selama 5 detik; satu Blitz tepat sasaran per titik. Tiga keberhasilan menyegel Genderuwo.
- [ ] **P1-3D** — Rusun beton empat lantai, kotak surat, lift barang, bolt cutter, keris, altar, tiga halogen; rig Genderuwo, Tuyul, Kuntilanak Putih.
- [ ] **P1-AV** — Dialog/rekaman transisi: Kain Jaket Damar dan kaset ritual mengarahkan Raka ke RSUD Lama Sido Waras. Tambahkan audio posisi, subtitle, ikon bukti, feedback perisai, dan petunjuk objek bila chat dilewatkan.
- [ ] **P1-SEC** — Negatif Foto Tari (`SECRET_P1_NEGATIF_TARI`): mesh/ikon, lokasi tersembunyi, trigger puzzle, catatan kesaksian, pickup unik dan autosave. Potret daftar tunggakan dengan AR, lalu cocokkan nama Tari pada label laci arsip. Laci yang benar membuka panel kecil berisi negatif foto; tidak memakai kode 644 dan tidak menggantikan kunci lift.
- [ ] **P1-QA** — Tiga bukti tidak menduplikasi; pintu ritual memeriksa pusaka; retry perisai tetap mungkin; item rahasia bertahan setelah reload; warning 0/1 sebelum transisi.

## Babak 2 RSUD Lama Sido Waras

- [ ] **P2-SYS** — Selfie Mode dan ThermoKlenik; AR serta EVP dasar tetap tersedia sejak Babak 1.
- [ ] **P2-LEVEL** — Kos dan riset → IGD → Poli 03 → keypad basement (1603) → kamar mayat (Laci C3, -24°C) → ruang inkubator → ruang jaga suster → ruang operasi.
- [ ] **P2-EVID** — Suhu Poli 03 sebesar -16°C; aura toples tumbal di Laci C3; EVP Inkubator 04. Pusaka: Gunting Tali Pusar, Gelang Identitas Bayi Lastri, dan Botol Darah Segel.
- [ ] **P2-BOSS** — Letakkan tiga pusaka di meja operasi. Pantau Lastri dari kamera Selfie sambil berjalan maju. Pancing ke lampu operasi atau jatuhkan flare. Saat perisai pecah, ia memadat dan boleh dipotret dari arah belakang atau depan. Tiga Blitz berhasil menutup pertarungan.
- [ ] **P2-3D** — IGD, empat poli, basement kedap suara, laci A1–C4 serta gudang arsip D1, inkubator, lampu operasi; rig Lastri.
- [ ] **P2-AV** — Dialog/rekaman transisi: Rekaman Damar dan video tungku mengarahkan Raka ke Pabrik Gula Belanda Sindang Laut. Tambahkan audio posisi, subtitle, ikon bukti, feedback perisai, dan petunjuk objek bila chat dilewatkan.
- [ ] **P2-SEC** — Label Pasien Tanpa Nama (`SECRET_P2_LABEL_PASIEN`): mesh/ikon, lokasi tersembunyi, trigger puzzle, catatan kesaksian, pickup unik dan autosave. Pindai laci arsip dengan ThermoKlenik. D1 menunjukkan 17°C dan angka 17 memantul di AR; cocokkan dengan halaman daftar korban yang kehilangan satu baris nama. Buka dengan kunci arsip yang tergantung di ruangan itu.
- [ ] **P2-QA** — Tiga bukti tidak menduplikasi; pintu ritual memeriksa pusaka; retry perisai tetap mungkin; item rahasia bertahan setelah reload; warning 0/1 sebelum transisi.

## Babak 3 Pabrik Gula Belanda Sindang Laut

- [ ] **P3-SYS** — Lensa UV Rajah dan EVP Recorder v2; generator, flare, jebakan suara, serta lantai rapuh.
- [ ] **P3-LEVEL** — Kos dan upgrade → timbangan tebu → gudang gilingan → generator akses → bangsal ketel dua lantai → kantor mandor → gudang ampas tebu → aula tungku utama.
- [ ] **P3-EVID** — Jejak gendongan tumbal melalui AR; EVP nina bobo pada interkom; rajah penyembunyi anak melalui UV. Pusaka: Boneka Jerami Tebu, Sabit Tebu Berukir Rajah, dan Selendang Batik Terbakar.
- [ ] **P3-BOSS** — Pasang tiga pusaka pada dudukan altar tungku, lalu periksa boneka Damar sebagai pemicu. UV menunjukkan lantai berbahaya; EVP membedakan suara asli dan tiruan. Aktifkan tiga generator bergantian atau gunakan flare untuk membuka perisai. Tiga Blitz menyegel Wewe Gombel.
- [ ] **P3-3D** — Catwalk kayu/besi, bangsal dua lantai, gudang ampas, generator akses serta tiga generator arena, loker 09, kain gendongan; rig Wewe Gombel.
- [ ] **P3-AV** — Dialog/rekaman transisi: Kartu pers dan sepatu Damar menunjukkan pemindahan ke Desa Mati Karang Setra; chat palsu mulai menyusup. Tambahkan audio posisi, subtitle, ikon bukti, feedback perisai, dan petunjuk objek bila chat dilewatkan.
- [ ] **P3-SEC** — Pelat Nomor Buruh 09 (`SECRET_P3_PELAT_BURUH`): mesh/ikon, lokasi tersembunyi, trigger puzzle, catatan kesaksian, pickup unik dan autosave. Verifikasi ketukan dengan EVP v2, telusuri panah UV pada balkon yang kokoh, lalu putar katup kecil berlabel 09 untuk membuka loker. Suara Damar merah yang memanggil dari jalur rapuh adalah pengalih perhatian.
- [ ] **P3-QA** — Tiga bukti tidak menduplikasi; pintu ritual memeriksa pusaka; retry perisai tetap mungkin; item rahasia bertahan setelah reload; warning 0/1 sebelum transisi.

## Babak 4 Desa Mati Karang Setra

- [ ] **P4-SYS** — Sanity aktif, verifikasi chat, UV v2 dan air ruwatan; map pendopo dengan empat jalur penjuru.
- [ ] **P4-LEVEL** — Kos → tiga simpang hutan bambu → pendopo → Utara kuburan → Timur gubuk dukun → Barat sumur → Selatan lumbung → pendopo dan gerbang Pasar Setan.
- [ ] **P4-EVID** — Anomali +66°C pada makam tumbal; aura altar di gubuk Mbah Suro; EVP mantra empat penjuru di sumur. Pusaka: Kendi Air Sumur Keramat, Belati Perak Ruwatan, dan Kantong Tanah Kubur Keramat.
- [ ] **P4-BOSS** — Letakkan tiga pusaka di pendopo. Di tiap penjuru, UV membedakan satu obor asli dari dua umpan. Padamkan obor asli dengan air ruwatan lalu hancurkan satu ikatan melalui Blitz. Pada obor keempat, inti menyatu di pendopo; Blitz penutup di pendopo dihitung sebagai tembakan keempat, bukan kelima.
- [ ] **P4-3D** — Pendopo dan empat jalur, sumur isi ulang, kuburan, gubuk, lumbung, 12 obor arena; Banaspati dengan jalur terbang dan rig arwah Mbah Suro.
- [ ] **P4-AV** — Dialog/rekaman transisi: Empat segel di HP membuka gerbang kelima. Babak 5 langsung dimulai pada malam yang sama. Tambahkan audio posisi, subtitle, ikon bukti, feedback perisai, dan petunjuk objek bila chat dilewatkan.
- [ ] **P4-SEC** — Keping Cermin Saksi (`SECRET_P4_CERMIN_SAKSI`): mesh/ikon, lokasi tersembunyi, trigger puzzle, catatan kesaksian, pickup unik dan autosave. Saat patroli lewat, matikan layar, pindah ke relung aman, lalu rekam ketukan asli dengan EVP. Sorot simbol mata tertutup memakai UV dan putar roda sumur ke arah simbol itu. Ambil keping dari laci dinding; tidak memerlukan menyiram obor.
- [ ] **P4-QA** — Tiga bukti tidak menduplikasi; pintu ritual memeriksa pusaka; retry perisai tetap mungkin; item rahasia bertahan setelah reload; warning 0/1 sebelum transisi.

## Babak 5 Pasar Setan

- [ ] **P5-SYS** — Seluruh modul HP, kios perbekalan, tiga sektor distorsi, pertarungan tiga tahap, dan empat cabang ending.
- [ ] **P5-LEVEL** — Gerbang dan kios → koridor beku → jembatan gilingan bambu → replika ruang jahit Atmaja → panggung lelang → tiga inti boss → pilihan akhir.
- [ ] **P5-EVID** — Server batu dan kabel roh melalui AR; kontrak bertanda mata terbuka melalui suhu di luar rentang serta UV; EVP gabungan suara penonton. Pusaka: Empat segel dari Babak 1–4 menjadi syarat cerita untuk membuka arena. Lima item rahasia menjadi syarat terpisah untuk secret ending.
- [ ] **P5-BOSS** — Tahap A: UV memilih wujud asli, lampu panggung membuka perisai, Blitz menghancurkan inti pertama. Tahap B: Selfie dan ThermoKlenik melacak sosok asli, flare atau lampu cadangan membuka inti kedua. Tahap C: berlindung dari gelombang api, AR mengungkap tiga kabel, interaksi memutusnya, Blitz memecahkan inti ketiga. Setelah itu masuk state pilihan ending.
- [ ] **P5-3D** — Kios perbekalan/Arsip Bisu, tiga sektor distorsi dari aset lama, panggung/kurungan, Cermin Saksi, kursi kosong, rig kepala lensa Penonton_Terakhir.
- [ ] **P5-AV** — Dialog/rekaman transisi: Damar bebas dari kurungan; aksi pemain menentukan Bad, Normal, True, atau Secret Ending. Tambahkan audio posisi, subtitle, ikon bukti, feedback perisai, dan petunjuk objek bila chat dilewatkan.
- [ ] **P5-SEC** — Pita Rekaman Pertama (`SECRET_P5_PITA_PERTAMA`): mesh/ikon, lokasi tersembunyi, trigger puzzle, catatan kesaksian, pickup unik dan autosave. Matikan layar selama 3 detik di depan kursi untuk menghentikan chat palsu, nyalakan AR untuk melihat laci tersembunyi, lalu rekam satu kalimat EVP asli. Laci terbuka dan pita dapat diambil. Kios perbekalan tetap berfungsi terpisah.
- [ ] **P5-QA** — Tiga bukti tidak menduplikasi; pintu ritual memeriksa pusaka; retry perisai tetap mungkin; item rahasia bertahan setelah reload; warning 0/1 sebelum transisi.

## Pengujian unlock dan ending

- [ ] **END-01** — Nol item: tiga ending dasar tersedia dan secret terkunci; campaign bisa selesai.
- [ ] **END-02** — Masing-masing dari lima item hilang secara bergantian: secret tetap terkunci. Lima duplikat satu ID juga tidak lolos.
- [ ] **END-03** — Lima item membuka jalur secret meski catatan belum dibaca; jika cermin belum divalidasi, pemain dapat menyiapkan ritual sebelum boss atau saat finale.
- [ ] **END-04** — Lima ID dan validasi lengkap: aksi secret aktif; True, Normal, dan Bad tetap bisa dipilih secara sadar.
- [ ] **END-05** — Reload setelah pickup, setelah validasi arsip, dan sebelum pilihan akhir mempertahankan keadaan yang tepat; New Game memulai koleksi kosong.
- [ ] **END-06** — Input F/E/LMB bersamaan hanya mengunci satu enum dan memainkan satu ending; setelah commit tidak menerima trigger lain.
- [ ] **END-07** — Kapasitor 5% dapat menyelesaikan semua aksi finale; biaya Blitz biasa tidak memblokir True/Secret.
- [ ] **END-08** — Secret menampilkan korban lama bebas, Raka/Damar pulang, penonton sadar, dana sah Rp125.000.000, dan aplikasi hilang.

## Urutan produksi dan batas lingkup

Bangun satu vertical slice Prolog hingga pelarian terlebih dahulu. Jalur kerja berbasis hasil: controller dan interaksi → HP/AR/baterai → puzzle dan Jurnal → AI stealth dan chase → save/checkpoint serta chat → satu level prolog lengkap. Setelah slice stabil, buat Babak 1 untuk menguji sistem eksorsisme dan satu collectible end to end, lalu tambah modul Babak 2, 3, 4, dan integrasi Babak 5.

Definisi selesai satu babak: dapat dimainkan dari awal sampai transisi, bukti dan pusaka tidak menduplikasi, checkpoint memulihkan keadaan dengan benar, semua trigger punya petunjuk, boss bisa diselesaikan setelah satu percobaan gagal, dan collectible babak tersimpan. Audio placeholder serta greybox diperbolehkan pada milestone pertama; penggantian aset mengikuti fungsi area yang telah terbukti.

Scope prototipe: first person, satu HP, satu profil kesulitan, level cerita tetap, satu varian puzzle per babak, empat ending, lima item rahasia. Engine belum dikunci; spesifikasi menggunakan sistem generik yang bisa dipetakan ke scene/node Godot atau Fyrox. Multiplayer, siaran internet nyata, chat AI daring, prosedural tanpa batas, chapter select, dan peta desa open world penuh berada di backlog.

Jalur utama naskah berjumlah 460 menit atau 7 jam 40 menit. Tambahan pencarian lima item sekitar 15–30 menit, belum termasuk retry dan eksplorasi bebas. Angka 10–30 jam dari konsep lama tidak menjadi janji durasi versi ini. Durasi setiap adegan adalah target pacing, bukan timer yang memaksa pemain gagal. Target performa sementara 60 fps pada preset sasaran; spesifikasi perangkat minimum baru ditetapkan setelah profiling vertical slice.

## Kontrak data dan pemulihan

Campaign menyimpan `chapter_id`, `checkpoint_id`, `evidence_ids`, `relic_ids`, `location_seals`, `upgrades`, `inventory`, `battery`, `sanity`, `wallet_legitimate`, `wallet_bound`, `spent`, `secret_items`, `secret_notes_read`, `archive_prepared`, `ending`, dan `schema_version`. Posisi dan arena menyimpan keadaan lokal sesuai checkpoint. Collectible unik yang sudah autosave dipertahankan ketika geometry direset; uang dan consumable kembali sesuai snapshot sehingga reload tidak menggandakan pembelian atau hadiah.

Event cerita menggunakan ID unik dan flag sekali jalan: `EVIDENCE_CAPTURED`, `RELIC_COLLECTED`, `JOURNAL_CONFIRMED`, `RITUAL_STARTED`, `SHIELD_OPEN`, `SEAL_HIT`, `SECRET_COLLECTED`, `SECRET_NOTE_READ`, `ARCHIVE_PREPARED`, dan `ENDING_COMMITTED`. Pemberian segel lokasi terjadi setelah boss selesai, bukan setiap Blitz. Simpan ke file sementara lalu ganti secara atomik; transaksi collectible memerlukan konfirmasi sukses sebelum indikator ditemukan ditampilkan.

Unlock secret: himpunan `secret_items` memuat lima ID yang tercantum pada tabel. Pemicu cutscene juga memerlukan `archive_prepared` benar, tiga inti final telah pecah, Damar sudah dibantu ke portal, dan `ending` belum terisi. Catatan yang dibaca tidak memengaruhi unlock. Perubahan `ending` memakai pemeriksaan state tunggal agar input serentak tidak memainkan dua ending. Jika file simpan rusak, tampilkan pilihan snapshot terakhir; jangan membuka secret berdasarkan hitungan palsu.
