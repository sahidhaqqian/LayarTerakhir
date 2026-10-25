# PHASE 4 — BARA DI KARANG SETRA

> **Project:** Layar Terakhir · **Babak:** 4 · **Lokasi:** Desa Mati Karang Setra · **Versi:** 2.1-REPO-IMPLEMENTATION (25 Oktober 2026)
> **Basis naskah:** `Phase4(1).MD` (revisi 15 Oktober 2026). **Sumber merger:** rancangan ChatGPT v1.0 (arsip `Phase4_pre_merge.md`) dan rancangan Claude `Phase4 (1).MD` v1.0; **acuan produksi lintas babak:** `Phase3(5).MD` v2.3-REPO-IMPLEMENTATION (24 Oktober 2026).
> **Status:** Build review Windows berhasil25 Oktober2026: tujuh scene,0 error/0 warning. Implementasi memuat116 sumber model baru; regresi280/280 dan directed native route128 pemeriksaan lulus. Bukti serta batasnya ada di `Audit/phase4-implementation.md`. Penerimaan visual/pacing oleh desainer, VO manusia, benchmark Windows dan seluruh180QA manusia belum selesai; bukan klaim180QA PASS.
> **Tujuan:** Spesifikasi tunggal untuk narasi, urutan adegan, denah, Sanity, chat palsu, Banaspati, arwah Mbah Suro, ritual empat penjuru, save/load, aset, dan kontrak penerimaan. **Kebijakan merger:** kekuatan Claude pada angka dan encounter authored digabung dengan progres boss persisten dan fair-horror versi ChatGPT; tidak menyalin dua aturan yang saling bertentangan.

---

## 0. Cara membaca, hierarki aturan dan audit sumber

### 0.1 Legenda keputusan

- **[KANON]**: fakta dan tujuan yang dinyatakan pada `Phase4(1).MD`, khususnya lima adegan, 78 menit, Desa Mati Karang Setra, Sanity, tiga bukti/pusaka, 4 obor asli + 8 palsu, empat tembakan Blitz total, Keping Cermin Saksi, dan gerbang Pasar Setan.
- **[DIWARISI P1–P3]**: kontrak campaign, HP, input, Jurnal, ekonomi, Arsip Saksi, aset, checkpoint yang masih relevan dari fase sebelumnya. **Terapkan kondisi repository aktual**, bukan nama class hipotetis.
- **[KLARIFIKASI]**: interpretasi yang menyambungkan konflik dan menutup softlock tanpa mengubah tujuan cerita.
- **[REVISI EDITORIAL]**: bagian sumber yang secara eksplisit dibetulkan dalam naskah yang diadaptasi; dicatat dalam §0.7.
- **[USUL BARU]**: fitur/tambahan yang boleh dihapus atau diubah bila bertentangan dengan arahan desainer.
- **[BASELINE UJI]**: angka tuning awal; diwajibkan bisa disetel terpusat, dievaluasi dalam playtest.
- **[PERLU KEPUTUSAN]**: pilihan yang butuh keputusan atau verifikasi repository; lihat §11.2 (`P4-Dxx`).

**Prioritas:** putusan terbaru desainer + `Rancangan.MD` resmi > implementasi/kontrak global aktual P1–P3 > kanon Phase 4 > baseline/usulan di dokumen ini. Pada merger awal repo belum disertakan. Audit produksi25Oktober2026 sudah memeriksa `Rancangan.MD` dan kode aktual; register§0.11 adalah kontrak implementasi yang berlaku, sedangkan catatan merger historis di§13 tidak menyatakan status produksi terbaru. Jangan menafsirkan QA dokumen sebagai tes yang sudah PASS.

### 0.2 Ringkasan babak dan identitas

| Elemen | Ketentuan |
|---|---|
| Judul | **Babak 4 — Bara di Karang Setra** |
| Sasaran waktu | **78 menit** jalur utama, di luar rahasia, pengulangan dan eksplorasi opsional; bukan timer paksa |
| Lima adegan | (1) Kos 16:45; (2) Hutan Bambu 23:53; (3) Pendopo + tiga sektor investigasi; (4) Perburuan & ritual empat penjuru; (5) Gerbang Pasar Setan |
| Lokasi | Kos Raka (Golden Scene) → Hutan Bambu 3 simpang → Pendopo hub → Utara Kuburan Tua; Timur Gubuk Mbah Suro; Barat Sumur Keramat; Selatan Lumbung Padi → Pendopo → gerbang bawah tanah |
| Musuh | **Banaspati** sebagai pemangsa patroli bola api dan inti kerangka hangus; **arwah Mbah Suro** sebagai pengatur ritual, chat, dan ilusi |
| Identitas horor | **Ketidakpercayaan terhadap UI dan penonton**: sanity, chat palsu, ilusi perintah, fakta dunia fisik yang harus diverifikasi; berbeda dari P1–3 yang mengandalkan tiga putaran boss satu arena |
| Modul | Sanity aktif; detektor akun janggal / Filter Anti-Sengkala; UV Rajah v2 (R), EVP v2 (C), Thermo (T), AR (Q); HP/kamera/perspektif existing |
| Bukti wajib | `+66°C` makam utara (Thermo); aura altar gubuk timur (AR); mantra empat penjuru sumur barat (EVP) |
| Tiga pusaka | Kendi Air Sumur Keramat (utara; isi ulang di sumur barat), Belati Perak Ruwatan (timur), Kantong Tanah Kubur Keramat (barat) |
| Boss | **4 obor benar dari 12 kandidat**: masing-masing penjuru 1 asli + 2 palsu. Tiga sektor awal memiliki **1 Blitz per sektor** setelah air ruwatan; sektor keempat hanya memadamkan obor → inti berkumpul di Pendopo → **Blitz keempat sekaligus terakhir** |
| Ekonomi | Beli kemenyan/powerbank/air opsional; semua jalur wajib bisa selesai pada `wallet_legitimate=0`, tanpa pembelian dan tanpa donasi |
| Rahasia | Keping Cermin Saksi `SECRET_P4_CERMIN_SAKSI`, relung kering Sumur Keramat, 1 dari 5 Arsip Saksi |
| Transisi | Segel keempat tersimpan; Pasar Setan adalah gerbang **kelima**; langsung P5 pada malam yang sama **tanpa pulang Kos**, sesudah checkpoint/commit |

### 0.3 Alokasi waktu dan pengajaran mekanik

| Adegan | Menit sasaran | Alur / fungsi belajar | Gate akhir |
|---|---:|---|---|
| 1 — Safe Hub Kos | 00–10 | Baca forum 1997; identitas Raka yang lelah; pengenalan Sanity dan UV v2; toko opsional; baterai via colokan fisik | Upgrade/kalibrasi terdaftar; berangkat |
| 2 — Hutan Bambu | 10–28 | 3 simpang, chat menyesatkan, UV fisik di jalan, Sanity 70/40/0, membedakan sumber clue | Pendopo terjangkau tanpa wajib mengikuti chat |
| 3 — Investigasi Pendopo | 28–52 | Boss tampak kebal; tiga sektor bebas urutan, Thermo/AR/EVP, 3 pusaka; rahasia sumur opsional | Tiga bukti unik, konfirmasi Jurnal dan penyerahan pusaka |
| 4 — Empat Penjuru | 52–72 | **Distributed boss hunt** terbuka, bukan arena 3 fase; UV bedakan 3 obor tiap sektor, air isi ulang, 3 segel per sektor dan klimaks sektor 4 → inti | 4 Blitz total berhasil; segel keempat dikomit |
| 5 — Gerbang Kelima | 72–78 | Twist empat segel membuka Pasar Setan; 999.999 penonton, UI uang error, kesaksian Damar; gerbang menurun | Panel arsip; `P4_COMPLETE`; scene P5 pada malam yang sama |

### 0.4 Diferensiasi sengaja dari Phase 1–3

**Jangan menaikkan kesulitan melalui hitpoint boss, instakill tanpa telegraph, atau empat salinan generator+Blitz.** P1 memperkenalkan penyegelan, P2 memaksa melihat ancaman lewat Selfie/Thermo, P3 menguji suara dan pijakan. P4 menguji **apakah pemain benar-benar memahami mana petunjuk yang dapat dipercaya**. Tiga bukti dan tiga pusaka tetap karena kanon, tetapi menjadi *pengetahuan/crafting ritual*; pengejaran ritual tersebar pada empat ruang, dengan **progres sektor persisten** dan urutan sektor pertama yang bisa berubah. Sektor Selatan tetap klimaks terakhir menurut naskah. Keberhasilan ditentukan verifikasi dan rute, bukan empat musuh dengan HP berlapis.

**Prinsip dua lapis kebenaran:** `MataBatin.apk` dapat menampilkan **chat, badge, notifikasi, dan overlay yang dipalsukan** oleh entitas pada Sanity rendah. **Sinar UV fisik yang menyinari rajah pada properti dunia serta kondisi fisik langkah/tanah** tetap dapat diandalkan. Pemain memverifikasi fakta menggunakan dunia, bukan mempercayai banner. Kamera tetap berguna; ini tidak menghapus fungsi HP yang dibangun P1–3. Sejak awal tampilkan contoh benar dan palsu agar twist adil.

### 0.5 Kontrak global yang tidak boleh digandakan

| Sistem | Prinsip |
|---|---|
| Mulai babak | Save `P3_COMPLETE` dan tiga segel valid. Mode skip P3 hanya `dev-only` dan tidak menulis segel palsu ke produksi. |
| Golden Kos | **Pakai scene Kos aktual yang sudah ada** dan ukurannya sebagaimana disetujui proyek, bukan membuat duplikat. Tambahkan state visual lelah/kopi/peta saja. |
| HP/input | Sistem kamera OTS/FP, tahan RMB dua tangan, `V`, `X`, `Q`, `R`, `T`, `C`, `F`, `G`, `J`, `E`, dan `GameInputConfig` existing. `H` untuk kemenyan hanya bila belum konflik binding nyata. |
| Energi | Profil P3: normal 0,05 poin/s; AR/UV 0,10 total/s, foto 1 poin, Blitz 15 poin/cooldown 6 s, powerbank besar +80. **UV v2 adalah kemampuan tambahan di filter UV yang sama**, bukan kamera/render kedua. |
| Uang | `wallet_legitimate` belanja sah; `wallet_bound` donasi `Penonton_Terakhir`; overlay cerita dipisah. Tidak ada top-up dari nominal overlay. |
| Evidence & item | `evidence_ids`, `relic_ids`, `inventory`, `secret_items`, `location_seals`, receipt events bersama. Pisahkan item ditemukan, dibawa, dan dipasang. |
| Save | Lanjutkan `CampaignState` / `TryCommit` / atomic save `.tmp/.bak` dari build. Jangan paksakan schema version tertentu tanpa audit loader; rollback penuh pada gagal tulis. |
| Performa | Unity 6.3 LTS URP Windows; visual autentik, sumber aset berlisensi; FPS dan visual hanya sah diverifikasi build Windows laptop kelas Iris Xe, bukan headless. |
| Status Sanity | P4 adalah **babak pertama yang mengubah Sanity sebagai sistem gameplay**. Nilai awal perlu policy yang diverifikasi; tidak diam-diam meregenerasi atau mengurangi sanity P3. |

### 0.6 Invarian yang tidak boleh dinegosiasi

1. `P4_TORCH_AUTH_x` **satu per sektor**; 2 fake per sektor. Tidak ada randomisasi yang menaruh semua obor asli di satu sisi atau menyembunyikannya tanpa tanda benar.
2. `P4_BLITZ_N`, `P4_BLITZ_E`, `P4_BLITZ_W` **tiga** keberhasilan sektoral; sesudah obor selatan dipadamkan, `P4_BLITZ_CORE` adalah tembakan **keempat**, bukan kelima. Sektor Selatan **tidak** punya local Blitz sukses terpisah.
3. `Sanity=0` **bukan kematian otomatis** dan tidak menghapus kemampuan pemain memahami tanda dasar, melakukan interaksi, refill, atau menyimpan permainan.
4. Kemenyan dan paket air **opsional**. Ada pemulihan Sanity dan air yang bisa dicapai tanpa uang ataupun HP aktif.
5. Status Arsip Saksi diturunkan dari jumlah ID unik aktual, bukan dibakukan menjadi 4/5. Item rahasia harus committed **sebelum** popup sukses.
6. Upaya boss sudah berjalan bukan alasan membuat save data P1–3 tidak aman; gunakan transaksi dan barrier di area yang sesuai, jangan menulis status sementara musuh sebagai kebenaran persisten.
7. P4 melanjutkan P3 namun P5 **dimulai malam yang sama tanpa kunjungan Kos**. Pertahankan inventori/baterai/Sanity aktual dan beri recovery fisik yang dapat dijangkau untuk pembuka P5.

### 0.7 Revisi editorial dan konflik sumber yang ditutup

| ID | Ketegangan dalam `Phase4(1).MD` | Keputusan kerja dan status |
|---|---|---|
| E01 | Kemenyan disebut “sangat wajib”, sedangkan seluruh jalur harus mungkin tanpa belanja | Kemenyan = opsi pemulihan cepat. Zona perlindungan fisik gratis dan pemulihan non-consumable tersedia [KLARIFIKASI]. |
| E02 | Teks klimaks meminta “Kemenyan terakhir” | Cabang bila pemain membawa kemenyan; jika stok nol gunakan perlindungan lumbung + verifikasi UV [REVISI EDITORIAL]. |
| E03 | Kendi ditemukan di Kuburan Utara, tetapi isi ulang hanya di Sumur Barat | Kendi adalah **wadah pusaka** ditemukan di utara; air ritual diperoleh/diisi ulang dari Sumur Barat setelah akses; rute sektor desain mengantisipasi dua area [KLARIFIKASI]. |
| E04 | “Letakkan tiga pusaka” tetapi air kendi dipakai pada empat obor | Pemasangan mencatat tiga pusaka secara ritual; **kendi menjadi peralatan ritual yang dapat diambil kembali dari dudukan tanpa mencabut offering/receipt**, satu instans aktif saja [KLARIFIKASI]. |
| E05 | Mengharuskan empat obor dan empat Blitz, tetapi obor keempat langsung menggabungkan inti | 3 local Blitz N/E/W + 1 core Blitz di Pendopo setelah memadamkan obor S = **4 total** [KANON, ditegaskan]. |
| E06 | Salah menyiram obor palsu bisa membuat air habis permanen | Air dapat diperbarui secara fisik (sumur dan pengisian bantu yang dibuka lewat ritual), salah siram memberi penalti bukan menghapus satu-satunya sumber [KLARIFIKASI]. |
| E07 | P4 dimulai Kos sore, P3 ending malam sebelumnya; “Babak 5 langsung malam sama” | Kos P4 adalah **hari berikutnya setelah P3**; P4→P5 langsung malam sama [KLARIFIKASI]. |
| E08 | 780.000 awal dan 999.999 akhir vs 666.000 akhir P3 | Angka adalah **concurrent viewers** per siaran; overlay `Rp999.999.999 [ERROR]` bukan saldo yang dibayar [KLARIFIKASI]. |
| E09 | Petunjuk pengguna chat angka “97” dapat dibaca sebagai classifier absolut | Angka/username hanyalah red flag. Validasi selalu dari UV objek/EVP/fakta; chat manusia pun bisa salah, entitas dapat meniru username [KLARIFIKASI]. |
| E10 | Smartphone yang dibuat pengikut Mbah Suro diungkap, tetapi UV harus mengungkap obor asli | **Rendering/teks UI bisa bohong; rajah UV fisik di world tidak**. Usulan aturan keadilan dan foreshadow aplikasi berkompromi [USUL BARU]. |
| E11 | Keping Cermin disebut “Arsip Saksi 4/5” dan pickup popup sebelum autosave | Arsip N/5 aktual; **atomic commit sebelum feedback** [KLARIFIKASI]. |
| E12 | Sanity 0 “perburuan intens” tapi bisa berkepanjangan membuat pemain softlock | Ada jalur recovery gratis & tempat berlindung aksesibel saat 0; tidak ada stun-control permanen atau objective palsu yang tak bisa ditolak [KLARIFIKASI]. |
| E13 | Teks tahun 1997, Rusunawa 2004, Mbah Suro arwah sekarang | 1997 tragedi/ritual Karang Setra; ia meninggalkan desa dan wafat seusai ritual Rusunawa 2004 menurut **Cermin Saksi**; kemunculan kini arwah, bukan tubuh hidup [KANON]. |
| E14 | “4 Obor Santet” vs “3 obor berjejer pada masing-masing penjuru” | Empat obor **asli**, **8 palsu** = 12 properti kandidat di empat sektor [KLARIFIKASI]. |
| E15 | Setelah cutscene baterai “1 bar” | Efek UI sinematik/glitch satu bar; **baterai persisten tidak boleh diam-diam diubah**. Jika dibutuhkan tenaga P5, sediakan charger diegetik [REVISI EDITORIAL]. |
| E16 | Penonton di chat palsu dapat mengajak lompat dan player mengikuti | Jebakan fatal punya warning/escape yang masuk akal; tidak ada game-over tanpa pembedaan dunia dan peluang reaksi. |
| E17 | Implementasi `Sanity`, `UV`, `chat`, `boss` bisa menduplikasi sistem lama | Semua merupakan perluasan modul existing; arsitektur, ID, schema, dan daftar binding diverifikasi di repo [DIWARISI]. |

### 0.8 Register angka cerita yang tetap

**1997** = tragedi Karang Setra; **2004** = kematian Mbah Suro setelah peristiwa Rusunawa menurut Cermin Saksi; **+66°C** = bukti makam; **780.000** = penonton pembuka P4; **999.999** = penonton akhir; **Rp6.666.000** = donasi `Penonton_Terakhir` pembuka; **Rp999.999.999 [ERROR]** = overlay rusak epilog, bukan saldo; **Rp500.000.000** = nilai kontrak naratif yang diucapkan antagonis, **bukan** otomatis uang yang Raka terima; **3 bukti + 3 pusaka + 4 obor asli + 4 Blitz total** adalah hitungan yang berbeda dan tidak boleh tercampur.

---

### 0.9 Review Claude vs ChatGPT dan keputusan integrasi v2.0

**Penilaian desain internal (subjektif; belum playtest):** Claude **8,7/10**, ChatGPT v1.0 **9,0/10**. Claude unggul pada rincian **tiga simpang berurutan**, **posisi 12 obor**, **state Banaspati berskala m/s**, **kontak berulang**, **katalog dan waktu cooldown**, serta QA regresi yang mudah dieksekusi. Versi ChatGPT unggul pada **rute boss N/E/W bebas**, **penyimpanan progres permanen per sektor**, **pemulihan sumber daya fisik** yang tidak mensyaratkan uang, **dua lapis kepercayaan terhadap UI**, dan **handoff langsung ke P5** tanpa merusak save lama. Skor bukan metrik objektif atau bukti game.

| Isu yang berbeda | Claude v1.0 | ChatGPT v1.0 | **Kontrak gabungan v2.0** |
|---|---|---|---|
| Checkpoint boss | CP5 sekali; kalah ulang empat sektor; CP6 hanya kemenangan, CP7 keluar | CP5 awal, CP6 *rolling* sektor, CP7 kemenangan, CP8 portal | **Pakai CP1–CP8 dan progres per sektor atomik.** Ini perubahan desain sengaja dari pola P1–P3; diterima sebagai baseline sebelum Codex audit |
| Kapasitas kendi | 3 muatan | 2 muatan | **3 muatan** untuk menghindari terlalu banyak bolak-balik, tetapi sumur tetap dibutuhkan untuk obor keempat tanpa Garam |
| Pembelian kemenyan | Pemulihan Sanity sampai 100%; cooldown 5 s | +35; cooldown 12 s; sumber gratis | **Ke100 mengikuti Rancangan.MD global pada audit2.1**; sumber gratis tetap tersedia, cooldown12s sebagai baseline |
| Air tambahan | Sumur sebagai satu-satunya refill gratis | Sumur + tempayan fisik tersalur | Sumur sumber tak terbatas, **tempayan yang punya sambungan/katup nyata** boleh membantu jalur bos; tidak ada refill dari UI atau reset gratis |
| Susunan obor asli | N tengah, E kiri, W kanan, S kiri | Preset seeded yang bisa bervariasi | **Preset Claude sebagai default**; variasi hanya jika seeded sebelum CP5 dan tetap konsisten saat reload |
| Flare warisan P3 | Tombol G ditolak, stok tidak habis | Bisa dipakai tetapi menarik perhatian | **Flare nonaktif di P4** sesuai peringatan kanon “JANGAN GUNAKAN FLARE API”; carry-over tidak hilang |
| Chat dan indikator | Hantu lebih sering berkedip/dideteksi | Username/glitch hanya petunjuk, UI dapat ditipu tetapi UV world reliabel | **Chat tidak pernah oracle; hanya rajah authored world yang otoritatif.** Distorsi tidak boleh membuat clue wajib tidak terbaca |
| Banaspati contact | 2 kontak dalam 10 s menjadi fatal | Lunge dengan telegraph dan eskalasi | Terapkan **dua-hit window sebagai baseline**, dengan telegraph, i-frame, dan ruang menghindar yang divalidasi |
| Baterai 0% saat hutan | Mencegah lewat baterai penuh dari Kos | Butuh charger fisik di setiap rute kritis | **Charger darurat sebelum J1** agar save bawaan 0 dan gagal isi Kos tidak softlock |
| Progres setelah mati | Seluruh boss reset | Sector receipts tetap | **Progres sukses sector tetap**, tetapi charge/sanity/air/Blitz harus committed konsisten; no duplication |

**Status audit2.1:** arahan implementasi terbaru mengizinkan pilihan teknis §0.11. Default preset tetap, harga baseline875rb/750rb/400rb, wujud arwah manusia dan loader4→5 dipilih secara eksplisit. Penyetelan setelah playtest serta penerimaan mutu/VO manusia tetap terbuka; tidak ada perubahan kanon diam-diam.

### 0.10 Perubahan yang tidak diadopsi mentah-mentah dari Claude

- **Reset penuh boss dari CP5 setelah kematian:** ditolak agar operasi empat penjuru tidak kembali terasa seperti tiga boss awal dan untuk menghargai tiap kemenangan lokal.
- **Kemenyan sebagai satu-satunya solusi Sanity:** ditolak karena bertentangan dengan syarat tidak wajib belanja dan stok 0 harus solvable.
- **Baterai penuh otomatis sebagai satu-satunya mitigasi Hutan:** ditolak; colokan Kos adalah kesempatan, bukan jaminan setiap Continue/retry; charger sebelum J1 ditambah sebagai mitigasi nyata.
- **`schema_version: 5` dipaksakan:** ditolak; schema harus diukur dari loader repo, dengan migrasi backward-compatible P0–P3.
- **Obor sektor dinyatakan padam permanen walau Blitz meleset tanpa receipt:** ditolak; kebenaran setelah reload harus diturunkan dari transaksi yang benar-benar committed.

### 0.11 Kontrak repositori yang diaudit (otoritatif untuk implementasi2.1)

Audit awal: `Audit/phase4-feasibility.md`; basis kode `ba029cd`. Draft asli pengguna disimpan tanpa perubahan pada cache produksi. Arahan terbaru mengizinkan pelaksanaan seluruh Phase4 dan revisi bentrokan mekanik; daftar P4-Dxx bukan permintaan persetujuan ulang untuk keputusan teknis ini.

- **REPO-01 / data:** loader aktual schema4, `CampaignState.TryCommit` dan nested `Phase3Data` terbukti. P4 menambah `Phase4Data` serta migrasi4→5; jalur1→2→3→4 dipertahankan. Receipt tunggal menjadi kebenaran sektor N/E/W dan S; contoh JSON di §6.2 konseptual, bukan field ganda yang wajib dibuat.
- **REPO-02 / input:** runtime V=OTS/FP, X=Selfie; Q/R/T/C/F/B/G/H/E melalui `GameInputConfig`. Satu HP/feed/energi dan provider `IPhoneChapterContent`, bukan sistem kamera/AR/inventori baru. Metadata EVP memakai sumber authored existing.
- **REPO-03 / Sanity:** nilai masuk P3 dipertahankan, termasuk0. Sumber pemulihan gratis Kos/hutan/desa memperbolehkan persiapan tanpa reset diam-diam. **Kemenyan ke100% sesuai Rancangan.MD**, mengganti baseline+35 merger; cooldown12s tetap baseline. P4-D02/D03 dipakai sesuai keputusan ini.
- **REPO-04 / resource:** kapasitas Kendi3, volume aktual0–3 disimpan. Offering tidak otomatis memberi3air; sumur/saluran fisik mengisinya. Satu Kendi tetap alat, dua pusaka lain dipindah ke altar saat CP5 sukses. Semua consumable, air, hit dan secret atomik; tidak memakai barrier sepanjang ritualP3.
- **REPO-05 / progres:** N/E/W bebas urutan, tiga receipt lokal disimpan pada CP6 rolling. S hanya pemadaman, lalu satu Blitz inti: tepatempat. Actor tidak diteleport ke anchor setiap menang; anchor dipakai untuk pemulihan save. CP7 kemenangan, CP8 konfirmasi keluar.
- **REPO-06 / akhir:** kode/scene Phase5 belum tersedia. CP8 tetap merepresentasikan penyelesaianP4 dan tujuanPasarSetan; pemain dapat menuruni tangga batu fisik ke vestibule nyata di bawah pendopo pada levelP4, lengkap pemulihan fisik, malam sama. Continue menuju boundary itu, tanpa Kos, tanpa mengubah `chapter_id` ke scene5 kosong dan tanpa mengklaim seluruhP5 playable. Router siap dihubungkan ke P5 penuh pada implementasi babak berikutnya. QA-120/150/177 disesuaikan dengan batas nyata ini.
- **REPO-07 / aset:** minimum100 **identitas model sumber baru** yang dipakai dalam level, bukan100 salinan/variasi material. Lingkungan Desa/Bambu/ritual/audio baru dari sumber komersial berlisensi; sumber lama hanya identitas kampanyeRaka/HP/motor/Kos dan kode/sistem bersama. Sumber unduhan, hash, author, lisensi, konversi/LOD dan penempatan dicatat. Tidak membuat model, lukisan/tekstur atau klip gerak original; assembly/retarget/konversi sumber berlisensi dijelaskan.
- **REPO-08 / mutu:** target DreadOut adalah evaluasi lingkungan lokal, readability, kamera, ancaman, audio dan pacing. Jumlah100 atau headless tests bukan bukti kesetaraan. Nilai pacing78menit/FPSIrisXe dan VO manusia final harus dibedakan dari hasil struktur/runtime/render yang sungguh diuji.
- **REPO-09 / jendela bidik:** kecepatan aktual controller setelah Awake adalah walk1,7/sprint4,2m/s, sehingga baseline3/4,5/5,2/5m/s diganti1,5/2,7/3,5/3,7. Sebelum menerima siraman, **semua kandidat** memerlukan inti Banaspati dalam7,5m dan LOS; tidak membocorkan keaslian. Ini memberi kesempatan fisik memakai jendela5s tanpa teleport aktor. Pesan meminta memancing api berlaku pada obor asli maupun palsu.
- **REPO-10 / sumber konkret:** implementasi memakai `Phase4Director` (provider/koordinator), `Phase4Campaign` (adapter transaksi existing), `Phase4Ritual`, `Phase4Banaspati`, `Phase4World`, `Phase4Balance`, `Phase4Hub`, dan `Phase4FearPresentation`. Thermo mengikuti P2: bidik target stabil2s, tidak memerlukan klik foto. Layer24=rajah UV, layer30=dinding ilusi yang dikecualikan hanya oleh AR. `Phase3EvpSource` dipakai ulang sebagai metadata, bukan menyalin audio/aset P3.
- **REPO-11 / konten dan penerimaan:** dialog manusia Indonesia belum boleh ditandai final hanya karena subtitle/efek sudah tersedia. List rekaman terpisah berisi seluruh cue; Windows manual, akting, pacing78menit, dan FPS IrisXe tetap syarat penerimaan manusia. Render native, tes transaksi, dan jalur otomatis dilaporkan sesuai cakupannya.

- **REPO-12 / eksplorasi fisik:** J1–J3 tetap rute utama dengan tiga petunjuk/umpan. Jika pemain mengeksplorasi luar jalur dan benar-benar mencapai Pendopo, CP3/reveal tetap aktif; receipt simpang tidak dipalsukan dan chat yang terlewat bukan kunci progres tersembunyi. Ritual memerlukan Pendopo/reveal selain bukti/pusaka/Jurnal. Ini mencegah aktor tetap dormant bila rute hutan dilewati dari samping tanpa memasang invisible wall.

- **REPO-13 / kontinuitas siaran:** adegan pembuka menyambungkan siaran otomatis ketika HP pertama kali memiliki daya; receipt donasi tetap atomik dan hanya sekali. Baterai0 tidak diisi diam-diam: setelah pemain mengengkol dinamo, koneksi baru dicoba lagi. F hanya mematikan layar, bukan membatalkan cerita siaran yang sudah berjalan. Retry otomatis save dibatasi, bukan menulis tiap frame saat disk bermasalah.


- **REPO-14 / keterjangkauan rekaman:** uji capsule native menemukan titik berdiri navmesh di depan sumur berjaraksekitar2,6m dari sumber EVP. Radius rekamP4 disetel3m (tetap close-range) dan readout meminta mendekat bila di luar radius. Batas6detik/mantra dan1,5detik/ketukan tidak berubah. Navigasi mengitari properti dan memakai tangga gubuk sesuai kemampuan langkah30cm Raka; konfigurasi agent global75cm untuk babak lain tidak diubah.

- **REPO-15 / framing dan ward:** jarak optical/Blitz dihitung dari posisi mata Raka agar offset OTS tidak memendekkan jangkauan dibandingFP; framing viewport, sudut dan LOS tetap memakai lensa HP. Thermo tidak membocorkan keaslian obor lewat suhu berbeda. Titik siram seluruh12 kandidat dipisahkan dari volume perlindungan agar api bisa mendekat; bentuk UV tetap satu-satunya penentu keaslian.

- **REPO-16 / ukuran dan ruang interaksi:** sumber model mendatar seperti keris, congklak dan sendok diukur menurut panjangnya bila rasio panjang/tebal>5; tidak diperbesar menurut ketebalan. Nisan tambahan tidak ditempatkan bertumpuk dengan target Thermo. Batu aktivasi ritual dipisahkan dari tiga dudukan agar trigger konfirmasi tidak menutupi interaksi persembahan. Geometri dan urutan cerita tetap memakai sumber unduhan serta transaksi yang sama.

Preset awal memakai N=Tengah/E=Kiri/W=Kanan/S=Kiri; variasi seeded hanya sebelum CP5, tidak berubah saat reload. Harga toko875rb/750rb/400rb tetap baseline yang dapat disetel. Tubuh MbahSuro tetap arwah manusia, bukan goblin atau tokoh game proprietary. Tidak mengarang nama korban tambahan atau mengganti rangkaian tahun1997–2004.

## 1. Naskah lengkap lima adegan (KANON + catatan implementasi)

### ADEGAN 1: Tahap Siang Hari — Kamar Kos Raka / Safe Hub (Menit 00:00 – 10:00)

**Kondisi Awal:** Pukul **16:45 WIB** (Sore menjelang malam). Kamar Kos Raka tampak semakin berantakan. Mata Raka merah dan tangannya sedikit gemetar saat memegang cangkir kopi dingin. Di layar monitor PC, berita televisi lokal menayangkan liputan viral tentang siaran langsung kanal **"Malam Keramat"** yang ditonton ratusan ribu orang namun dianggap sebagai rekayasa CGI tingkat tinggi.

#### 1. Riset Forum Urban Legend: Kutukan Desa Karang Setra

* **Objektif Layar:** `Buka PC untuk meriset Desa Mati Karang Setra dan menyiapkan perlengkapan Sanity.`

* Pemain mengakses utas arsip rahasia di forum *urban legend*:

> **TEKS ARTIKEL FORUM (TRAGEDI DESA KARANG SETRA - 1997):**
> "Desa Karang Setra dulunya adalah kampung terpencil di kaki bukit yang dikelilingi hutan bambu lebat dan kuburan tua. Pada malam gerhana tahun 1997, tetua adat sekaligus dukun santet bernama **Mbah Suro** membakar hidup-hidup seluruh warga desa dalam ritual pembukaan gerbang **Pasar Setan**.
> **Peringatan bagi penjelajah:**
>
>
> 1. Desa ini dijaga oleh arwah **Mbah Suro** dan **Banaspati**—bola api hidup haus darah yang terbang menyusuri hutan bambu.
>
>
> 2. Kabut asap di Karang Setra menyerang kewarasan (*Sanity*). Jika pikiranmu goyah, **arwah warga desa akan menyusup ke dalam alat komunikasimu untuk menuntunmu ke dalam api**.
>
>
> 3. Kekuatan abadi Banaspati dan Mbah Suro terikat pada **4 Obor Santet** di empat penjuru desa. Padamkan keempat obor yang memiliki rajah asli untuk menghancurkan perisai api mereka."
>
>
>
>

* **Raka (Monolog Suara):** "Mbah Suro... nama dukun itu selalu muncul sejak malam pertama di Rumah Atmaja. Berarti Desa Karang Setra ini pusat dari semua ritualnya. Kalau gue berhasil nyegel dia malam ini, gue bisa tau cara ngeluarin Damar dari Pasar Setan."

#### 2. Upgrade Modul Smartphone & Belanja Persediaan Klenik

Pemain membuka menu *upgrade* Smartphone dan Toko Online Klenik menggunakan uang saweran dari malam ketiga:

1. **Upgrade Modul Smartphone (Terbuka di Babak 4):**
* **Indikator `Sanity & Filter Anti-Sengkala` (Otomatis di UI HP):** Menampilkan persentase kewarasan Raka aktual dari save (`100%` adalah contoh UI, bukan reset otomatis) serta fitur **Detektor Akun Janggal** di kolom *Live Chat* (komentar dari hantu penyusup akan memiliki sedikit efek *glitch* huruf atau ketidaksesuaian dengan hasil pindaian **Lensa UV Rajah**).

* **Kalibrasi `Lensa UV Rajah v2.0` (`[R]`):** Kini mampu membedakan **Api Santet Asli** (inti ungu dengan lingkar segel utuh) dan **Api Jebakan Ilusi** (inti merah dengan rajah silang).

2. **Belanja Persediaan Ekspedisi:**
* **5x Kemenyan Bakar** (paket opsional; menahan penurunan *Sanity* dan membersihkan halusinasi dengan `[H]`. **[REVISI E01]** Zona perlindungan di dunia adalah alternatif gratis; pembelian tidak boleh menjadi syarat).

* **3x Baterai Cadangan / Powerbank Besar**.

* **2x Kantong Air Keramat / Garam Ruwatan** (Digunakan untuk memadamkan Obor Santet Mbah Suro di fase klimaks).

* Pemain berinteraksi dengan pintu keluar kamar kos → memilih **`[BERANGKAT KE DESA MATI KARANG SETRA - PUKUL 23:45 WIB]`**.

---

### ADEGAN 2: Hutan Bambu Karang Setra — Distorsi Sanity & Infiltrasi Live Chat (Menit 10:00 – 28:00)

**Kondisi Awal:** Pukul **23:53 WIB**. Raka memarkir motornya di ujung jembatan kayu lapuk yang membatasi jalan tanah dengan **Hutan Bambu Desa Karang Setra**. Angin malam berhembus kencang menggesek batang-batang bambu tinggi (**`"KREK... KREEEK..."`**), sementara abu sisa pembakaran berterbangan di udara seperti salju hitam.

#### 1. Menyalakan Siaran & Kontrak Saweran Keempat

* Pemain menekan **`[TAB]`** untuk mengangkat Smartphone dan menekan **`[MULAI LIVE]`** (label kanon awal `[GO LIVE]` diseragamkan dengan antarmuka fase sebelumnya).

* Indikator penonton langsung meroket ke **[LIVE | 👁️ 780.000 Penonton]**.
* **Raka (Dialog Suara ke Kamera):** "Ekspedisi segel keempat. Gue udah sampai di Desa Mati Karang Setra. Ingat pesan gue barusan: malam ini jangan gampang percaya sama semua komentar di chat. Kalau gue kelihatan mulai ngaco, ingetin gue buat bakar kemenyan."

* **Rentetan Live Chat #1:**
* `@horor_nusantara`: *"780 RIBU PENONTON LIVE! Satu Indonesia begadang nontonin bang Raka!"*
* `@mbah_dukun_asli`: *"Hati-hati bang, Karang Setra itu sarangnya Banaspati! Jangan lari kalau liat bola api!"*

* `Penonton_Terakhir` **[DONASI Rp6.666.000]**: "Selamat datang di kampung halaman guru kami, Raka. Damar baru saja diseret melewati pendopo desa menuju gerbang Pasar Setan. Tapi sebelum kau menyusulnya, bermainlah dulu dengan warga desa yang sudah menunggumu di kolom komentar."

#### 2. Labirin Hutan Bambu: Kombinasi `Infiltrasi Live Chat` × `Lensa UV Rajah`

Untuk mencapai **Pendopo Desa Karang Setra**, pemain harus melewati **Labirin Hutan Bambu** yang memiliki **3 Persimpangan Jalan Setapak**. Kabut hitam di hutan ini mulai menurunkan indikator **Sanity** Raka sebesar **1% setiap 10 detik**.

* **Mekanik Distorsi Psikologis (Sanity) & Infiltrasi Live Chat:**
* Saat *Sanity* Raka turun ke **70%**, tepi layar mulai berdenyut dengan urat hitam dan terdengar suara bisikan puluhan warga desa dari balik pohon bambu.

* Di **Persimpangan Pertama (Kiri: Jalur Batu Nisan vs Kanan: Jalur Pohon Beringin)**, kolom *Live Chat* mulai disusupi oleh akun-akun hantu yang menyamar sebagai penonton:

* `@warga_karang97` *(Akun Hantu — teks sedikit berkedip)*: *"Bang Raka belok KIRI aja lewat batu nisan! Tadi gue liat bayangan bang Damar lari ke kiri kok, aman gak ada jebakan!"*

* `@sari_puspita97` *(Akun Hantu)*: *"Iya bang ke KIRI aja cepetan sebelum apinya dateng!"*

* `@rizky_pratama` *(Penonton Manusia Asli)*: *"Woi jangan dengerin akun yang ada angka 97-nya! Cek dulu jalannya pakai Sinar UV bang Raka!"*

* **Eksekusi Gameplay (Verifikasi dengan `Lensa UV Rajah` `[R]`):**
1. Pemain mengangkat HP dan menekan **`[R]` (`Lensa UV Rajah`)** ke arah **Jalur Kiri**: di bawah sorotan sinar ungu UV, tanah dan batang bambu di jalur kiri penuh dengan **Lukisan Rajah Tengkorak Ungu** dan kawat jerat santet yang akan langsung membakar Raka jika dilewati!

2. Pemain mengarahkan **`Lensa UV Rajah`** ke **Jalur Kanan**: tampak **Jejak Kaki Asli Damar** dan tanda anak panah aman.
3. Di **Persimpangan Kedua & Ketiga**, hantu di *Live Chat* bahkan meniru nama *username* penonton asli (misal `@rizky_pratama_` dengan tambahan *underscore*) untuk menyuruh Raka masuk ke gubuk bambu yang salah. Pemain wajib terus memverifikasi setiap saran *chat* menggunakan **Lensa UV Rajah (`[R]`)** atau **Lensa AR (`[Q]`)** dan, bila membawa Kemenyan, memakai **`[H]`** untuk memulihkan *Sanity*; alternatif gratis berupa titik perlindungan fisik ada di jalur. Pemulihan satu kemenyan menuju100% mengikuti rancangan global sesudah penggunaan selesai; satu instance dikonsumsi atomik.

---

### ADEGAN 3: Pendopo Desa & Kuburan Tua — Fase Investigasi 3 Bukti & Stealth Banaspati (Menit 28:00 – 52:00)

Menggunakan mekanik Fase Gameplay B (Investigasi & Stealth — Phasmophobia × Outlast) di mana Boss 100% KEBAL dan pemain harus mengumpulkan 3 bukti klenik serta 3 benda pusaka.

**Kondisi Awal:** Raka keluar dari hutan bambu dan tiba di **Alun-Alun / Pendopo Desa Mati Karang Setra**. Di tengah pendopo kayu yang hangus, terdapat lingkaran batu ritual besar yang dikelilingi oleh **4 Obor Santet Raksasa** (menyala dengan api merah darah) di empat penjuru desa: **Utara (Kuburan Tua)**, **Timur (Gubuk Dukun Mbah Suro)**, **Barat (Sumur Keramat)**, dan **Selatan (Lumbung Padi)**.

#### 1. Menemukan Kacamata Damar & Kemunculan Banaspati + Mbah Suro

Di tengah lingkaran batu Pendopo Desa, Raka menemukan **Kacamata Retak milik Damar** dan **Buku Kitab Rajah Mbah Suro (Item Dokumen #1)**:

> **TEKS DOKUMEN (KITAB RAJAH MBAH SURO - BAGIAN KEEMPAT):**
> "Empat obor penjuru desa adalah nyawa dari **Banaspati** peliharaanku. Selama keempat obor itu menyala, tak ada cahaya kamera maupun senjata manusia yang sanggup melukai tubuh kami. Hanya mereka yang berani mengambil **3 Tumbal Tanah Kubur** dan memadamkan **4 Obor Santet Asli** dengan air ruwatan yang bisa memutus ikatan Panca-Sengkala di desa ini. Namun awas: aku telah memasang puluhan obor palsu di sekeliling desa—siapa yang menyiram obor palsu yang ditandai **Rajah Silang Merah**, api Banaspati akan langsung melahap wajahnya!"
>
>

* **Pemicu Kemunculan Boss (Fase 100% Kebal):**
* Tiba-tiba terdengar suara batuk kakek tua dan ketukan tongkat kayu dari atap pendopo. Sosok arwah **Mbah Suro** (kakek bungkuk berjubah hitam compang-camping dengan wajah separuh tengkorak terbakar) berdiri di atas bubungan atap sambil merapalkan mantra Jawa kuno!

* Dari obor utama di belakangnya, melesat keluar sosok **Banaspati**: bola api merah raksasa seukuran tubuh manusia dengan wajah tengkorak menyeringai dan lidah api sepanjang 2 meter yang melayang berpatroli mengelilingi desa!

* **Peringatan Sistem di Layar HP (`MataBatin.apk`):**
> **`[BAHAYA KRITIS: BANASPATI & MBAH SURO TERDETEKSI — 100% KEBAL! JANGAN GUNAKAN FLARE API! JAGA SANITY DENGAN KEMENYAN & SEMBUNYI SAAT CAHAYA MERAH MENDEKAT!]`**
>

#### 2. Aturan Unik Stealth Banaspati & Pengumpulan 3 Bukti Klenik

Pemain harus menyelinap ke area **Kuburan Tua (Utara)**, **Gubuk Mbah Suro (Timur)**, dan **Sumur Keramat (Barat)** untuk mengumpulkan **3 Bukti Klenik** dan **3 Pusaka Ruwatan** sambil menghindari patroli Banaspati:

* **Aturan Unik Banaspati (Wajib Dipatuhi Pemain):**
1. **Radar Detak Jantung / Sanity:** Berbeda dengan hantu lain yang mencari cahaya senter, Banaspati tertarik pada **getaran rasa takut (*Sanity* di bawah 40%)** dan **suara langkah kaki di ranting bambu kering**. Jika *Sanity* Raka drop dan terdengar bunyi detak jantung keras, Banaspati akan langsung terbang cepat ke arah posisi Raka! Pemain dapat menjaga *Sanity* dengan **Kemenyan (`[H]`)** ATAU titik perlindungan fisik gratis. Sanity 0 bukan kematian otomatis.

2. **Tempat Sembunyi (`Outlast` Mechanics):** Saat cahaya merah Banaspati menerangi pepohonan dari kejauhan, pemain harus segera merunduk masuk ke dalam **Liang Kubur Terbuka**, **Kolong Gubuk Panggung**, atau **Gentong Air Tanah**, lalu menekan **`[F]` untuk mematikan layar HP**.

* **Lokasi 3 Bukti Klenik & 3 Pusaka di Karang Setra:**
1. **Sektor Utara — Kuburan Tua (`ThermoKlenik` + Pusaka #1):**
* Pemain merangkak di antara batu-batu nisan tua sambil mematikan HP setiap kali Banaspati melayang di atas area makam.

* Pemain mengaktifkan **`ThermoKlenik` (`[T]`)**: berbeda dengan RSUD yang bersuhu beku, makam tempat Mbah Suro menanam tumbal menunjukkan anomali suhu panas ekstrem **`+66°C`**!

* **`[✓] BUKTI KLENIK 1/3: ANOMALI SUHU BARA SANTET (+66°C)`** tercentang di Jurnal!

* Di atas makam tersebut, pemain mengambil **Pusaka #1: Kendi Air Sumur Keramat**.

2. **Sektor Timur — Gubuk Dukun Mbah Suro (`Lensa AR` + Pusaka #2 & Pesan Damar):**
* Pemain menyelinap masuk ke gubuk panggung tempat tinggal Mbah Suro. Pintu kamar ritual di dalamnya disegel tembok anyaman bambu gaib.
* Pemain mengaktifkan **Lensa AR (`MataBatin.apk` — `[Q]`)** untuk menembus dinding ilusi tersebut dan memotret **Meja Altar Tengkorak Mbah Suro** yang memancarkan aura merah darah → **`[✓] BUKTI KLENIK 2/3: AURA ALTAR PANCA-SENGKALA (LENSA AR)`** tercentang!

* Di samping altar, pemain mengambil **Pusaka #2: Belati Perak Ruwatan** dan menemukan **Sobekan Buku Catatan Terakhir Damar (Item Dokumen #2)**:

> **TEKS CATATAN DAMAR:**
> "Ka, gue akhirnya ngerti kenapa `Penonton_Terakhir` nyuruh lo livestream dari malam pertama. Aplikasi `MataBatin.apk` yang gue unduh dari deep-web itu sebenarnya dibuat sama pengikut Mbah Suro! Setiap kali ratusan ribu penonton nonton ketakutan di layar lo, energi spiritual mereka disedot lewat sinyal siaran buat ngebuka gerbang kelima: **Pasar Setan**! Gue diseret masuk ke kawah di belakang pendopo desa ini... Tolong hancurin Mbah Suro, Ka!"
>
>

3. **Sektor Barat — Sumur Keramat (`EVP Recorder` + Pusaka #3):**
* Pemain bergerak menuju Sumur Tua di barat desa dan menyalakan **`EVP Recorder`**: terekam suara mantra terbalik Mbah Suro yang menyebut keempat titik obor penjuru desa → **`[✓] BUKTI KLENIK 3/3: REKAMAN EVP MANTRA EMPAT PENJURU`** tercentang!

* Pemain mengambil **Pusaka #3: Kantong Tanah Kubur Keramat** di bibir sumur.

* **Konfirmasi Jurnal Deduksi di HP:**
Pemain membuka **Jurnal Klenik** dan menekan **`[KONFIRMASI IDENTITAS & BUKA RITUAL EKSORSISME]`**:

> **`[ENTITAS TERIDENTIFIKASI: BANASPATI & ARWAH MBAH SURO]`**
>
>
> **`[METODE EKSORSISME: Jangan tertipu petunjuk palsu di Live Chat! Gunakan LENSA UV RAJAH untuk menemukan 4 OBOR SANTET ASLI di 4 penjuru desa, padamkan dengan Air Keramat untuk memecahkan Perisai Api Banaspati, lalu segel Mbah Suro & Banaspati dengan BLITZ KAMERA!]`**
>

---

#### Jalur opsional Arsip Saksi

Sebelum melanjutkan, pemain dapat memeriksa **Keping Cermin Saksi**. Relung kering di dinding Sumur Keramat sektor Barat, terpisah dari Kantong Tanah Kubur wajib. Saat patroli lewat, matikan layar, pindah ke relung aman, lalu rekam ketukan asli dengan EVP. Sorot simbol mata tertutup memakai UV dan putar roda sumur ke arah simbol itu. Ambil keping dari laci dinding; tidak memerlukan menyiram obor. Pengambilan tidak dihitung sebagai bukti atau pusaka boss. Sebelum meninggalkan babak, status item ini ditampilkan 0/1 atau 1/1 pada Jurnal.

### ADEGAN 4: Malam Puncak — Pertarungan Taktis 4 Obor Santet (Menit 52:00 – 72:00)

Menggunakan mekanik Fase Gameplay C (Klimaks Eksorsisme Boss — Pertarungan taktis memadamkan obor santet di 4 penjuru desa sambil menghindari kejaran bola api hidup + Lensa UV Rajah + Infiltrasi Live Chat).

#### 1. Pemicu Boss Fight di Pendopo Desa

Pemain kembali ke lingkaran ritual **Pendopo Desa** dan meletakkan ketiga pusaka di tengah batu segel.

* Langit malam di atas Desa Karang Setra berubah menjadi merah membara! **Arwah Mbah Suro** melayang di atas atap pendopo sambil tertawa serak, sementara **Banaspati** membesar dua kali lipat dan mulai mengejar Raka secara agresif melintasi ke-4 jalur desa!

* Di masing-masing dari **4 Penjuru Desa (Utara, Timur, Barat, Selatan)**, Mbah Suro memunculkan **3 Obor Berjejer (1 Obor Santet Asli + 2 Obor Jebakan Palsu)**.

#### 2. Mekanik Boss Fight: "Saring Chat Palsu, Cek dengan Lensa UV, Padamkan 4 Obor, Tembak Blitz!"

Untuk menyegel Banaspati dan Mbah Suro, pemain harus berlari memutari **4 Penjuru Desa** dan menyelesaikan siklus taktis berikut di tiap penjuru:

1. **Langkah 1 — Gangguan `Infiltrasi Live Chat` Saat Dikejar:**
* Saat pemain tiba di satu penjuru (misalnya **Utara - Kuburan Tua**) dengan kondisi dikejar bola api Banaspati dari belakang, terdapat **Obor Kiri, Obor Tengah, dan Obor Kanan**.

* Kolom *Live Chat* langsung membanjiri layar dengan teriakan palsu dari hantu penyusup untuk membuat pemain panik:

* `@hantu_karang01`: *"SIRAM OBOR KIRI BANG CEPETAN APINYA UDAH DI BELAKANG LU!"*

* `@penonton_setia`: *"OBOR KANAN BANG YANG ASLI! JANGAN YANG TENGAH!"*

2. **Langkah 2 — Verifikasi dengan `Lensa UV Rajah` (`[R]`):**
* Pemain tidak boleh asal menyiram! Pemain harus mengangkat HP dan menekan **`[R]` (`Lensa UV Rajah`)** selama 2 detik ke arah ketiga obor tersebut:

* **Obor Kiri & Kanan (Palsu):** Menampilkan **Rajah Silang Merah** (jika disiram, obor akan meledak menyemburkan api dan mengurangi 40 poin *Sanity* Raka dan memberi jeda pemulihan).
* **Obor Tengah (Asli):** Memancarkan **Sinar Inti Ungu Murni dengan Simbol Segel Panca-Sengkala**!

3. **Langkah 3 — Padamkan Obor & Tembakan Segel (`DreadOut` Blitz Capture):**
* Pemain berinteraksi (`[E]`) menyiramkan **Air Sumur Keramat** ke **Obor Santet Asli** hingga apinya padam mengeluarkan asap putih tebal!

* Begitu 1 Obor Asli padam, **Perisai Api Banaspati pecah untuk sementara** dan ia jatuh terpental ke tanah memperlihatkan wujud intinya (kerangka manusia terbakar yang terhubung dengan benang gaib ke tubuh Mbah Suro)!

* Layar `MataBatin.apk` mengunci target merah: **`[PERISAI API PADAM — TEMBAKKAN BLITZ KAMERA SEKARANG!]`**.

* Pemain menekan **`[Klik Kiri Mouse]` (Jepretan Blitz)** untuk menghancurkan satu ikatan pada tiga penjuru pertama. Pada penjuru keempat, jendela tembakan dipindahkan ke inti gabungan di pendopo; tidak perlu tembakan tambahan pada obor keempat!

* **Klimaks Obor Keempat (Penjuru Selatan - Lumbung Padi):**
* Setelah 3 obor di Utara, Timur, dan Barat berhasil dipadamkan dan ditembak Blitz, *Sanity* di seluruh desa terkuras cepat dan Mbah Suro turun langsung menghadang di depan **Penjuru Selatan** sambil melempar semburan bara Banaspati!
* Jika masih memiliki Kemenyan, pemain dapat membakarnya (`[H]`) untuk menjernihkan pandangan; bila habis, pemain berlindung di zona netral Lumbung untuk mengurangi distorsi, lalu menyorotkan **`Lensa UV Rajah` (`[R]`)** untuk menemukan **Obor Asli ke-4** di balik kepulan asap, lalu memadamkannya! Kendi dapat diisi ulang di sumur; apabila kemenyan habis, celah perlindungan netral dekat lumbung menjaga jalur tetap bisa diselesaikan.

* Saat keempat obor mati total, tubuh Banaspati dan Mbah Suro tersedot menyatu di tengah Pendopo Desa tanpa pelindung api sedikit pun.

* Raka berlari ke depan Pendopo dan melepaskan **Jepretan Blitz Penutup** tepat ke wajah Mbah Suro!

* Terdengar jeritan panjang Mbah Suro saat tubuhnya hancur menjadi abu hitam dan tersedot masuk ke dalam segel keempat aplikasi `MataBatin.apk`!

---

### ADEGAN 5: Ending Cutscene Babak 4 — Terbukanya Gerbang Pasar Setan (Menit 72:00 – 78:00)

* Begitu segel keempat terkunci di dalam Smartphone Raka, keheningan justru tidak datang. Tanah di tengah **Pendopo Desa Karang Setra** tiba-tiba retak dan amblas, membentuk kawah tangga batu kuno yang memancarkan cahaya merah ungu surreal dari bawah tanah!

* **Raka (Monolog Suara — menatap layar HP dengan tangan gemetar):** *"Catatan Damar bener... Keempat segel yang gue kumpulin dari Rusunawa, Rumah Sakit, Pabrik Gula, sampai Desa ini... bukannya nutup kutukan, tapi justru jadi kunci buat ngebuka Gerbang Kelima!"*

* Raka mengangkat layar Smartphone-nya:
* Indikator penonton kini berkedip liar di angka **[LIVE | 👁️ 999.999 Penonton]**.
* Saldo saweran di layar berubah menjadi simbol angka acak yang rusak (**`Rp 999.999.999 [ERROR]`**).
* Seluruh kolom *Live Chat* kini dipenuhi jutaan akun tanpa foto profil yang mengetik satu kalimat yang sama secara berulang-ulang:
* **`"MASUK KE PASAR SETAN. KAMI MENUNGGUMU DI LAYAR TERAKHIR."`**

* Dari dalam kawah tangga yang terbuka di tengah pendopo, terdengar suara riuh rendah seperti **keramaian pasar malam tradisional**—suara gamelan sumbang, tawa pedagang gaib, dan teriakan **Damar** dari dasar kawah!

* Kotak pesan merah darah dari **`Penonton_Terakhir`** muncul memenuhi separuh layar HP Raka:

> **Penonton_Terakhir:** "Kerja bagus, Raka Wijaya. Empat segel Panca-Sengkala telah lengkap di dalam ponselmu, dan satu juta jiwa penontonmu telah menyerahkan kesadaran mereka malam ini untuk membuka gerbang kami. Tidak ada lagi waktu istirahat di kamar kosmu. Turunlah ke bawah pendopo ini sekarang... Sahabatmu Damar, dan kontrak Rp500.000.000-mu, sedang dilelang di **Pasar Setan**."
>
>

* Baterai HP Raka berkedip merah menyisakan **1 bar**, sementara Raka melangkah menuruni tangga kawah menuju dimensi **Pasar Setan** tanpa kembali ke Safe Room → Suara *shutter* kamera berbunyi keras (**`"CKREK!"`**) → *Cut to Black*.

---

**[KLARIFIKASI PASCA-NASKAH]** Kalimat sumber “3 Tumbal Tanah Kubur” pada Kitab Rajah adalah kutipan tulisan antagonis, bukan kebutuhan mengambil tiga kantong tanah. Ketiga pusaka tetap Kendi, Belati, dan Kantong. Naskah menyebut *4 obor asli* dan tepat *4 Blitz* total: pemadaman Selatan tidak menambah foto lokal; tembakan terakhir pada inti di Pendopo. Saat HP tampak menyisakan 1 bar dalam adegan penutup, indikator boleh berkedip sinematik, tetapi state baterai persisten tidak berubah secara tersembunyi. Keputusan teknis ada pada §4, §6, §8.


---

## 2. Kontrak implementasi dan event registry

### 2.1 Input dan kepemilikan aksi

| Aksi | Default dari dokumen | Semantik P4 dan aturan konflik |
|---|---|---|
| Gerak | `WASD`, `Shift`, `Ctrl`, `Space` | Sprint menimbulkan kebisingan; jangan pakai kecepatan baru tanpa konsistensi animasi P3. Jongkok mengurangi deteksi Banaspati di tempat perlindungan. |
| Angkat HP | `TAB` / tahan `RMB` | Pertahankan dua cara global; release RMB mengembalikan view sebelumnya tanpa double toggle. Satu HP dan satu baterai. |
| Perspektif | `V`, Selfie `X` | Tetap ada; bukan syarat ritual P4, jangan hapus mekanik lama. |
| `Q` | AR | Mengungkap/memotret aura altar sektor Timur; jangan menganggap UV sebagai AR. |
| `R` | UV Rajah v2 | Kerucut fisik pada objek UV, baca **segel bulat inti ungu = asli**, **silang merah = palsu**, **tengkorak ungu pada jalan = bahaya**. Satu filter UV (bukan kamera terpisah). |
| `T` | ThermoKlenik | Bukti `+66°C` Kuburan Utara; ukur target authored, bukan suhu OS/ambient nyata. |
| `C` | EVP v2 | Rekam mantra Sumur; analisis suara asli/imitasi. Input tap/hold sesuai adapter P3. |
| `F` | Mati/nyala HP | Sembunyikan cahaya saat Banaspati melintas; 0% baterai juga menghilangkan stimulus layar. Tidak menghapus live. |
| `H` | Kemenyan | Konsumsi 1 instance **jika** tersedia dan cooldown sudah selesai. Binding dikonfirmasi di `GameInputConfig`; tidak boleh mengganggu penggunaan consumable lain. |
| `E` | Interaksi | Mengambil bukti/pusaka, air Kendi, rotasi roda sumur, obor, dudukan altar, pengisian daya fisik, portal. Interaksi yang mensyaratkan air harus punya umpan balik `KENDI KOSONG` tanpa mengurangi state. |
| Klik kiri | Capture (1%) / Blitz (15%) | Hanya konteks boss valid memicu Blitz; foto bukti dan Blitz tak boleh terjadi dalam klik yang sama. Blitz butuh baterai ≥15. |
| `J` | Jurnal | Tiga bukti unik, deduksi, sektor yang selesai, panel Arsip; saat berbahaya opening Jurnal tidak otomatis memberi immunity. |
| `G` | Flare eksisting | **Dinonaktifkan selama berada di Karang Setra** (pilihan Claude): tekan G menampilkan “Api asing ditolak desa”, tidak mengonsumsi stok lama; tidak menjadi sumber 13 obor atau mekanik exploit. Flare P1–P3 tetap di inventori untuk P5. |

**Prioritas deterministik:** pause/cutscene wajib > `F` darurat saat stealth > `E` objek ritual yang sah > `H` jika tersedia > interaksi UI HP/Jurnal > capture/Blitz > aksi non-kritis. Bila UI HP terbuka dan dunia tidak di-*pause* secara global, Raka masih bisa terdeteksi; chat yang menyerupai prompt sistem **tidak pernah mempunyai akses langsung ke action dispatcher**. *Input hints* tampil dari binding nyata; tidak hardcode `H` bila telah diremap.

### 2.2 State machine perjalanan global

```text
NOT_STARTED
  -> SAFE_HUB_KOS (P3_COMPLETE + 3 segel valid; forum, pembaruan Sanity, UV v2, toko opsional)
  -> DEPARTED / FOREST_ENTRY (CP1; live 780.000; donasi dana-terikat sekali)
  -> BAMBOO_BRANCH_1 (CP2 sebelum J1; tutorial: chat palsu versus rajah dunia)
  -> BAMBOO_BRANCH_2 (tiruan username + sanity threshold)
  -> BAMBOO_BRANCH_3 (menilai ulang clue dari dunia)
  -> PENDOPO_HUB (CP3 sebelum reveal; Banaspati + arwah Mbah Suro tampak kebal)
  -> INVESTIGATE_FREE_ORDER {
       NORTH_GRAVE: THERMO_66 + KENDI,
       EAST_SHAMAN: AR_ALTAR + SILVER_BLADE + DAMAR_NOTE,
       WEST_WELL: EVP_MANTRA + SOIL_POUCH + WATER_REFILL + OPTIONAL_MIRROR
     }
  -> JOURNAL_CONFIRMED (3 bukti unik, 3 pusaka; tidak otomatis dari 3 scan)
  -> OFFERINGS_PLACED (3 pusaka dicatat; kendi bisa dipakai kembali sebagai alat air)
  -> RITUAL_ACTIVE (snapshot CP5 dahulu; sektor Utara/Timur/Barat dapat berurutan bebas)
  -> THREE_BINDINGS_CUT (tepat 3 Blitz sektoral sukses; receipts sektoral bertahan)
  -> SOUTH_REAL_TORCH_OUT (UV v2 + air, tidak ada local Blitz)
  -> CORE_MERGE_PENDOPO (dua bentuk Banaspati dan arwah Mbah Suro di inti gabungan)
  -> FOURTH_BLITZ_CORE (satu hit final, CP7 + segel keempat)
  -> FIFTH_GATE_REVEAL (999.999 viewer, UI error, Damar, panel Arsip)
  -> PHASE4_COMPLETE (konfirmasi portal, CP8 atomik)
  -> PHASE5_ENTRY (malam sama; tanpa Kos; carry-over aktual)
```

**Turunan stage** dibangun dari receipts bukti/pusaka/sektor/portal; jangan persist boolean ganda `stage=...` bersama `completed_event_ids` kecuali loader existing memiliki kebutuhan eksplisit dan invariant yang diuji. Banaspati **tidak** di-reset total pada setiap sektor seperti boss P1–3; progres penjuru persisten setelah transaksi sukses. Area penutup sektor tidak dapat memberi Blitz kedua.

### 2.3 Registry event satu-kali dan pembeda state

| Event / ID konseptual | Pemicu dan syarat | Efek / status simpan |
|---|---|---|
| `P4_ARRIVE_KOS` | P3_COMPLETE & 3 segel terverifikasi | Golden Kos, berita kanal Malam Keramat, waktu cerita 16:45; lanjutkan campaign |
| `P4_FORUM_READ` | PC forum tragedi 1997 | Simpan dokumen forum opsional, VO Raka, petunjuk 4 obor |
| `P4_MODULE_CALIBRATED` | Buka modul / tutorial Kos | Upgrade Filter Anti-Sengkala & UV v2 **gratis**; bukan pembelian |
| `P4_KOS_CHARGE_USED` | Interaksi colokan Kos fisik satu kali | Isi 100% via sistem P3 existing, receipt; bukan otomatis saat reload |
| `P4_SHOP_KEMENYAN`, `_POWERBANK`, `_WATER` | Pasar Gaib, saldo sah dan receipt valid | Atomic buy: potong saldo, tambah instance, receipt sekali; opsional |
| `P4_DEPART_KOS` | Pilih berangkat 23:45; modul ada | Travel Kos→Bambu, CP1 setelah kontrol kembali |
| `P4_START_LIVE` | HP `MULAI LIVE` di hutan | 780.000 viewer, dana terikat Rp6.666.000, scripted chat sekali |
| `P4_BAMBOO_BRANCH_1/2/3` | Melintas simpang selamat | Tutor UV, distorsi Sanity, fake-chat versi bertahap; tidak memaksa baca chat |
| `P4_PENDOPO_REACHED` | Keluar hutan | CP2/CP3, kacamata Damar & Kitab Rajah tersedia, entitas reveal |
| `P4_BANASPATI_REVEAL` | Mendekati batu ritual | Bola api muncul, Mbah Suro arwah; tidak boleh spawn kill |
| `P4_EVIDENCE_N_THERMO` | Thermo +66°C stabil pada makam target | Bukti unik 1, receipt commit dan checkpoint anchor aman |
| `P4_TAKE_KENDI` | Ambil dari makam utara | Pusaka #1; volume air awal dapat kosong sebagai baseline; isi pada sumur |
| `P4_EVIDENCE_E_AR` | Foto aura altar Mbah Suro dengan AR | Bukti unik 2, receipt |
| `P4_TAKE_BELATI` | Ambil di gubuk timur | Pusaka #2, receipt |
| `P4_DAMAR_NOTE_READ` | Ambil Sobekan Buku Damar | Twist asal MataBatin.apk; satu dokumen yang tersimpan |
| `P4_EVIDENCE_W_EVP` | Rekam mantra 4 penjuru di sumur | Bukti unik 3, receipt |
| `P4_TAKE_TANAH` | Ambil Kantong Tanah Kubur di sumur | Pusaka #3, receipt |
| `P4_WELL_REFILL` | Kendi diisi via interaksi sumur | Volume air terbatas, **sumur tidak pernah habis**; refill bukan transaksi uang |
| `P4_SECRET_KNOCK_VERIFIED` | EVP mendengar ketukan autentik relung | Receipt langkah puzzle saja, bukan secret pickup |
| `P4_SECRET_EYE_VERIFIED` | UV fisik simbol mata tertutup; arah roda valid | Relung terbuka, satu rute aman; bukan obor |
| `P4_SECRET_PICKUP` | Ambil Keping Cermin, syarat puzzle | Atomic `secret_items.Add(SECRET_P4_CERMIN_SAKSI)` lalu popup/objek hilang |
| `P4_JOURNAL_CONFIRMED` | 3 bukti unik + konfirmasi eksplisit | Deduksi entitas dan ritual, once |
| `P4_OFFERINGS_PLACED` | 3 pusaka + jurnal | Ritual siap, naskah visual altar; pintu/rute tidak terkunci prematur |
| `P4_RITUAL_TRIGGER` | Interaksi batu ritual valid; CP5 sukses | Aktifkan empat penjuru, tanpa save di cutscene berbahaya |
| `P4_TORCH_N/E/W_IDENTIFIED` | UV dua fake, satu asli per penjuru | UI knowledge; tidak harus write save sampai pemadaman sukses |
| `P4_TORCH_N/E/W_WATERED` | `E` dengan air pada obor asli | Pemadaman sementara, exposure; belum dihitung segel jika Blitz gagal |
| `P4_BLITZ_N/E/W` | Blitz valid pada inti setelah penyiraman sektor | **Commit sektor atomik**: 1 ikatan putus + obor asli padam permanen; recovery aman |
| `P4_TORCH_S_IDENTIFIED` | Sektor Selatan, 3 obor kandidat | Obor asli tidak otomatis ditampilkan di chat; ditentukan UV world |
| `P4_TORCH_S_OFF` | Siram obor asli Selatan setelah tiga ikatan selesai | Commit stage pemadaman, lanjut merge di Pendopo. **Tidak menambah sealCount/Blitz keempat di Selatan.** |
| `P4_CORE_EXPOSED` | Inti Banaspati+Mbah Suro muncul di Pendopo | Boss core rentan, empat segel gambar sebagai overlay |
| `P4_BLITZ_CORE` / `P4_BOSS_WIN` | Satu Blitz valid pada inti | **Blitz keempat**, atomik segel keempat `SEAL_DESA_MATI_KARANG_SETRA`, CP7 |
| `P4_FIFTH_GATE_REVEAL` | Kemenangan & epilog | Viewer 999.999, saldo naratif ERROR, celah Pasar Setan terbuka |
| `P4_EXIT_PANEL_OPEN` | Interaksi tangga/portal | UI runtime saja, Arsip lokal 0/1 atau 1/1 dan global N/5 |
| `P4_COMPLETE` | Konfirmasi turun portal | CP8 atomik, next destination Pasar Setan; baru shutter CKREK/cut |

**ID di atas adalah usulan namespace**, bukan nama API yang dipastikan sudah ada. Pada integrasi gunakan alias/migrasi jika kelas Phase3 memakai penamaan berbeda, bukan menyimpan dua representasi item sama.

### 2.4 Precondition, denial dan trigger yang tidak boleh membuat softlock

| Gerbang | Boleh bila | Feedback bila gagal | Perilaku ilegal |
|---|---|---|---|
| Portal hutan ke Pendopo | Tiga persimpangan dilalui melalui rute fisik aman (tidak harus mengikuti chat) | UV pada tengkorak ungu + papan jalur | Chat memaksa teleport, instakill tanpa telegraph, jalur benar hilang pada sanity 0 |
| Menilai makam | `T` aktif, titik sumber +66°C fokus/terekam sesuai existing | Bacaan kurang stabil atau target belum terbidik | Foto biasa terhitung Thermo |
| Foto altar | `Q` aktif dan target terjangkau | “Aura tidak terlihat pada mode ini” | `R` sebagai pengganti AR tanpa keputusan |
| EVP sumur | `C` aktif, authored mantra autentik, rekam cukup | Segmen salah, progress terputus, ulang tanpa penalti | Ambient/fake chat dianggap bukti EVP |
| Memulai ritual | Bukti 3/3, konfirmasi Jurnal, 3 pusaka dipasang, CP5 commit sukses | Tampilkan bagian yang masih kurang | Pintu mengunci saat inspeksi; boss mulai sebelum save |
| Siram obor | Ritual aktif, Kendi dibawa, air>0, `E` pada target | Kendi kosong → tunjuk sumur/refill terdekat; bukti visual tetap ada | 0 air mengurangi negative amount; salah siram mengunci satu-satunya obor asli |
| Blitz lokal N/E/W | Obor asli baru dipadamkan, inti muncul, baterai ≥15, LOS valid | Jika miss, obor hidup lagi setelah recovery; stok air bisa diisi ulang | Satu obor memberi 2 segel; sektor S memberi local Blitz |
| Core final | Tiga sektor tersimpan + obor S mati, inti gabungan aktif | Jika miss, core punya recovery fisik dan exposure ulang **tanpa meminta obor baru** | Memerlukan tembakan ke-5 atau memaksa mengulang tiga sektor |
| Collect mirror | EVP knock + UV mata tertutup + roda benar | Relung tidak bergeser hingga prasyarat | Rahasia hanya bisa diambil sebelum boss, atau melalui pembayaran |
| Exit portal | Boss win commit CP7, adegan penutup, player confirm | Jika save gagal, panel tetap ada untuk retry | Mengubah chapter ID ke 5 tetapi segel keempat belum tertulis |

---

## 3. Arsitektur level, rute aman dan jalur investigasi

### 3.1 Denah logis (bukan koordinat final scene)

```text
GOLDEN SCENE KOS RAKA -- motor / time skip --> BAMBOO FOREST ENTRY (CP1)
  |
  +-- SIMPANG 1: kiri batu nisan (UV skull = hazard), kanan beringin (safe)
  +-- SIMPANG 2: impersonasi nama chat; dua rute fisik, salah ada telegraph
  +-- SIMPANG 3: chat sistem palsu; landmark bambu hangus menuju Pendopo
  v
PENDOPO DESA / HUB (CP3, batu ritual, Kacamata Damar, Kitab Rajah)
  |\
  | +-- NORTH / KUBURAN TUA: THERMO +66°C; KENDI; 3 obor kandidat
  | +-- EAST  / GUBUK MBAH SURO: AR ALTAR; BELATI; DAMAR NOTE; 3 obor
  | +-- WEST  / SUMUR KERAMAT: EVP MANTRA; KANTONG TANAH; air refill;
  | |                         OPTIONAL RELUNG CERMIN; 3 obor
  | +-- SOUTH / LUMBUNG PADI: 3 obor (salah satu asli), perlindungan netral,
  |                           gerbang Pasar Setan di bawah Pendopo, tidak dapat dibuka awal
  |
  +-- RETURN LOOP: semua rute menuju Pendopo tanpa harus melewati hazard yang sama
  v
RITUAL ACTIVE: sektor N/E/W disegel bebas urutan (masing-masing +1 Blitz)
  -> SOUTH: air obor benar, tanpa Blitz lokal
  -> PENDOPO: inti bergabung, Blitz keempat --> CP7 --> portal konfirmasi CP8
```

**Kesetaraan jarak:** final world scale mengikuti controller Raka. Denah empat cabang harus memiliki rute balik yang dikenali dengan *landmark* non-HUD: makam bersusun (Utara), gubuk berpelita (Timur), sumur dan katrol (Barat), lumbung berdinding anyaman (Selatan). **Tidak mewajibkan kompas HUD**; pemain mempelajari landmark. Rute hutan bambu dengan tiga simpang memberi jawaban melalui detail lingkungan bukan angka username. Saat chat dimatikan, babak masih solvable.

### 3.2 Hutan bambu — tiga simpang dengan pelajaran berbeda

| Simpang | Tipuan | Bukti yang bisa diverifikasi | Akibat jika salah dan recovery |
|---|---|---|---|
| 1 — nisan / beringin | Akun `@warga_karang97` mengarahkan ke batu nisan dan mengaku melihat Damar | UV fisik: **tengkorak ungu = bahaya** di kiri, jejak kaki Damar benar di kanan; derit/abu sebelum jerat | Jebakan panas dengan telegraph → kehilangan Sanity/escape; jangan instant kill saat masa belajar |
| 2 — gubuk salah | Akun tiruan `@rizky_pratama_` mengaku sebagai penonton lama | Ukiran UV pada bambu asli + tanda jalan batu; `@rizky_pratama` boleh salah juga sehingga nama akun bukan jawaban | Putaran kembali ke simpang / gangguan penampakan; jalan benar tetap ada |
| 3 — pesan sistem palsu | Chat menyamar sebagai “[PERINGATAN MATABATIN]” agar pemain menonaktifkan UV | Tanda permanen di akar pohon/marker, papan arah bambu hangus yang dapat dibaca dengan cahaya biasa | Ilusi gang sempit/penurunan sanity; recovery dari waypoint rute; tidak menyeret pemain tanpa kendali |

**[MERGER Claude]** Letakkan satu dinamo engkol darurat **sebelum J1 pada rute fisik masuk hutan**, bukan hanya colokan di Kos dan dinamo Pendopo. Ini menjamin pemain dengan baterai 0 pada CP2 bisa menyalakan UV tanpa reset atau membeli powerbank. Engkol harus dapat digunakan saat layar HP mati, dengan landmark jelas tanpa UV.

**Fairness:** tanda tengkorak ungu di jalan selalu berarti bahaya; simbol itu **tidak** sama dengan inti ungu obor benar. Bentuk berbeda: tengkorak vs lingkar penuh. Tambahkan deskripsi teks Jurnal dan alternatif kontras, jangan bergantung pada warna ungu vs merah saja. Tempatkan satu platform aman untuk mengamati tiap simpang dalam jangkauan cahaya dasar tanpa baterai aktif.

### 3.3 Pendopo pusat sebagai hub, bukan arena ronde

Pendopo adalah tempat kontras: bangunan kayu arang, batu upacara, empat pilar penjuru, pintu gerbang Pasar Setan yang **belum terlihat** di bawah lantai. Kacamata retak Damar dan Kitab Rajah memberi alasan menyelidiki tiga sektor; tidak langsung mengunci player. Mbah Suro muncul di atap, Banaspati keluar dari nyala ritual untuk memperkenalkan rasa takut terhadap api dan intensitas Sanity. Reveal pertama diberi jarak aman, *sound cue* dan rute penghindaran. Setelah `P4_JOURNAL_CONFIRMED` dan ketiga pusaka, pemain memasangnya di batu ritual; interaksi ritual eksplisit menyimpan CP5 sebelum mengaktifkan empat penjuru.

### 3.4 Tiga investigasi bebas urutan

**Utara — Kuburan Tua:** makam tumbal dengan tanda bara dari bawah tanah, perubahan suhu lokal +66°C hanya pada target; batu nisan memberi *cover* saat Banaspati melintas di atas. Kendi Pusaka #1 ditemukan di sini; bila kosong, tooltip “isi dengan air sumur barat”. Satu akses balik lewat rumpun bambu dan satu jalur putar menuju Pendopo; tidak ada jalur masuk hanya untuk satu arah.

**Timur — Gubuk Dukun:** gubuk panggung dengan kolong aman, panel anyaman ilusif yang terbaca melalui AR; bukti #2 aura meja altar, Belati Perak dan Catatan Damar terpisah. Rekaman Damar memuat pengungkapan bahwa MataBatin.apk berasal dari pengikut Mbah Suro. **Tidak** secara otomatis menonaktifkan UV atau membuat semua alat tidak jujur: hanya notifikasi/UI yang bisa dipalsukan. Pemain harus bisa menolak membuka Catatan tanpa kehilangan akses bukti.

**Barat — Sumur Keramat:** sumber EVP mantra empat penjuru, Kantong Tanah di bibir sumur, titik pengisian Air Ruwatan (gratis tanpa habis), relung Keping Cermin opsional. Saat Banaspati melewati atas, tersedia dinding sumur serta ceruk aman. Tanda ketukan autentik untuk rahasia tidak boleh tertutup oleh audio ambien/umpan Mbah Suro; kalau pemain tidak memiliki EVP karena baterai 0, charger engkol dekat sumur dapat dioperasikan tanpa HP.

**Selatan — Lumbung Padi:** *landmark* belum perlu dikunjungi untuk tiga bukti. Saat ritual, menjadi zona klimaks dengan ilusi chat gabungan dan udara memerah. **Ada shelter fisik** yang melindungi dari pandangan Banaspati sementara, memungkinkan pemulihan Sanity/kamera sebelum memilih obor benar. Tidak pernah mewajibkan membawa Kemenyan terakhir.

### 3.5 Twelve-torch authoring contract

Ada **empat set prefabs sektor**, masing-masing **tiga obor** yang dapat dilihat jelas dari minimal satu titik observasi aman. Template authored menempatkan tepat satu obor dengan `authentic=true` dan dua `decoy=true` di antara label posisi kiri/tengah/kanan (jangan selalu obor tengah agar pemain tidak menebak pola). Untuk obor benar UV menampilkan **lingkar segel utuh dengan inti ungu dan pulse halus**, untuk palsu **rajah silang merah + inti retak**, *silhouette/pattern* berbeda selain warna. Gunakan **preset authored stabil** sebagai default: **Utara=Tengah** (ditulis eksplisit di naskah), **Timur=Kiri, Barat=Kanan, Selatan=Kiri** [BASELINE dari Claude, belum kanon]. Mode variasi boleh memilih dari daftar preset berlisensi/di-author dan menulis seed sekali di CP5, tetapi posisi tidak boleh berganti di tengah attempt, saat retry, atau setelah sektor dikomit dan mudah diuji dalam debug; tidak boleh berpindah ketika player memeriksa atau sedang menuang air.

Tiap sektor memerlukan: titik observasi, rute lari bebas halangan, tempat berlindung/charger lokal yang tidak memicu shortcut mengalahkan boss, interactor torches radius mengikuti `InteractionInteractor`, *target weakpoint* 5 detik **[BASELINE]** (bukan klaim jendela kanon P4), rute keluar balik pendopo yang tidak diblokir musuh sepanjang waktu, dan pengisian air terjangkau. Salah siram palsu memberi penalti Sanity **40 poin [KANON pada naskah]** serta semburan api yang di-*telegraph* (bukan instakill), lalu decoy pulih. Sektor asli tak bisa tertutup permanen akibat ledakan palsu atau animasi ragdoll.

### 3.6 Golden Scene, streaming dan batas scene

**Kos:** data dekorasi berubah tanpa membuat Kos baru; TV lokal melaporkan “Malam Keramat” dianggap CGI; peta Panca-Sengkala menyorot Karang Setra sebagai segel keempat. **P4 Desa:** prefer satu scene/streamed subscenes berdasarkan biaya memori, bukan seluruh desa sebagai satu mesh besar. Batasi *shadow-casting* dinamis Banaspati dan obor, gunakan fog berlapis volumetric palsu murah (bukan volumetric raymarch berat) agar Iris Xe mampu menggambar. Gunakan LOD, occlusion, light probes, impostor bambu; sektor berbeda harus tetap mudah dinavigasi saat FX padam.

---
### 3.7 Detail denah dan parameter blocking dari Claude (BASELINE UJI)

Denah konseptual: **Pendopo** di pusat `(0,0)`; **Utara/Kuburan** `(0,+55 m)`, **Timur/Gubuk** `(+55 m,0)`, **Barat/Sumur** `(-55 m,0)`, **Selatan/Lumbung** `(0,-55 m)`; hutan masuk dari barat-daya. Jalan tanah antar-area **3–4 m** dan lingkar luar menyediakan rute alternatif agar Banaspati tidak menutup *single choke-point*. Seluruh koordinat hanyalah *blockout targets*, bukan klaim scene Unity sudah memakai angka ini. Rute lari lintas sektor ditargetkan **25–35 detik** untuk pemain sprint sebelum gangguan.

| Titik authored | Komposisi/penempatan | Peringatan desain |
|---|---|---|
| Kuburan Utara | 3 liang persembunyian, Thermo +66°C; deret tiga obor dengan **Tengah asli** | Nisan sebagai LOS break; false hot torch tidak sama dengan tanda UV asli |
| Gubuk Timur | 2 kolong panggung, AR altar, Catatan Damar; obor **Kiri asli** | Ada pintu/kolong balik dari interaksi, bukan dead-end yang mudah dikunci Banaspati |
| Sumur Barat | 2 gentong/relung, sumber refill tak terbatas dan EVP; obor **Kanan asli** | Area interaksi sumur tidak boleh di-camp saat pemain engkol; dua jalur keluar |
| Lumbung Selatan | Gentong + celah netral; obor **Kiri asli** di balik asap | Hanya aktif setelah N/E/W, telegraph serangan Mbah Suro; satu rute bebas api ke pendopo |
| Tiap tiga obor | Kiri/Tengah/Kanan berjarak kira-kira 4 m, 1 titik UV di 6 m, lebar ruang manuver ≥12 m | Hanya 1 asli per sektor; 2 palsu; bentuk/teks dibaca pada Low 720p tanpa mengandalkan warna |

**Mekanisme tiga simpang hasil sintesis:** J1: kiri nisan berbahaya, kanan beringin benar; J2: gubuk kiri palsu, jalan lurus benar; J3: sungai kering kiri buntu/jerat bertanda dan tangga tanah kanan benar. Berbeda dari instruksi chat palsu yang berulang, *setidaknya satu pesan manusia di tiap simpang boleh akurat, tetapi tidak boleh menjadi satu-satunya bukti*. Simpang salah harus memiliki peringatan UV dan audio; untuk J3, hasil salah dapat menjadi buntu aman terlebih dahulu agar tiga jebakan tidak terasa identik. Penanda jalan/jejak Damar yang nyata tidak boleh tiba-tiba berpindah jika chat berubah.

**Pacing vs densitas level:** Rincian lokasi dan 55 m per sektor merupakan baseline; jika perlintasan dari West Well ke South Lumbung ternyata lebih lama daripada budget 20 menit boss, sederhanakan rute fisik atau aktifkan *tempayan air terhubung dari Sumur* sebagai shortcut berbasis lingkungan—**jangan** mempercepat musuh/baterai untuk memaksa 78 menit. Transisi scene/LOD harus menjaga Banaspati dan receipt sektoral tidak menghilang karena unload/reload.

## 4. Sistem horor dan AI — Sanity, chat, Banaspati, Mbah Suro, ritual

### 4.1 Sanity adalah sumber tekanan baru (bukan health kedua)

**[KANON]** Pada Babak 4, kabut Hutan Bambu mengurangi Sanity **1 poin setiap 10 detik** di zona yang terpapar; pada 70% terlihat denyut tepi layar dan bisikan, di bawah 40% Banaspati tertarik pada rasa takut, dan pada 0% ia memburu lebih intens. **Sanity 0 tidak otomatis Game Over.** Nilai bisa berkurang lagi karena salah menuang air pada obor palsu sebesar 40 poin dari naskah. Jangan menerapkan penalti berdasarkan FPS atau `Time.deltaTime` yang bergantung menu pause; gunakan game simulation time yang konsisten.

| Tier | Kondisi | Distorsi yang diizinkan | Informasi dasar yang tetap bisa diandalkan |
|---|---|---|---|
| CLEAR | 71–100 | Kabut tipis, teks HUD normal; chat biasa tapi belum tentu benar | Semua properti dunia, UV v2, audio, arah fisik |
| UNSETTLED | 41–70 | Nadi visual tepi, bisik arwah, komentar berulang palsu | Rajah UV fisik tetap tegas, prompt input tidak berubah |
| PANIC | 1–40 | Suara Damar/Ibu palsu, penonton berlipat, bayangan Banaspati ganda, radius deteksi Banaspati meningkat | Obor asli tetap dikenali **oleh bentuk segel UV world**, bukan label HUD |
| HUNT | 0 | Mbah Suro mengacaukan UI/chat, suara langkah/seruan mengejar lebih sering; AI masuk kebijakan hunt | Perintah pemulihan dan interaksi selalu tersedia; tidak ada kontrol dibajak, teleport paksa, ketukan fatal acak atau teka-teki yang mustahil dibaca |

**Baseline tuning**: `sanity.forestDrain=0.1 poin/s` saat terpapar sampai lantai 35%; area Pendopo biasa 0.04/s ketika Banaspati tidak dekat hingga lantai sektor 45%; zona perlindungan 0/s; sektor selatan klimaks dapat 0.12/s dengan telegraph visual, tidak lebih tanpa izin tuning. `sanity.min=0`, `max=100`. **Lantai drain ambien** mengikuti baseline Claude: kabut hutan tidak mengurangi di bawah **35%** hanya lewat waktu, udara sektor tidak di bawah **45%**; event Banaspati/obor palsu masih bisa menurunkan sampai 0%. Dengan demikian kelamaan membaca arsip saja tidak menghukum pemain dengan hunt tanpa kesalahan. Sanity meningkat hanya lewat **kemenyan pilihan** atau **perlindungan dunia fisik**. Kemenyan [H] memulihkan **ke100%** setelah penggunaan selesai, mengikuti `Rancangan.MD` global; baseline+35 merger diganti pada audit repositori, 2 s animasi, cooldown 12 s baseline (bukan otomatis 100); sumber gratis berupa **Batu Lingkar Ruwatan** di Pendopo serta satu **titik perlindungan di jalur hutan sebelum J1** dan **ceruk/netral** tiap jalur sektor, interaksi tahan 8–12 s menghasilkan +15 poin, cooldown 30 s gameplay, tanpa uang. **Celah netral Lumbung** boleh mempunyai pemulihan +3 poin/s hingga 70% [baseline Claude] selama pemain benar-benar berlindung, sehingga Sanity 0 dan tanpa kemenyan tetap bisa menyelesaikan Selatan. Aktivasi sumber gratis harus mengurangi pengejaran lewat *line-of-sight break* atau *protective circle* yang benar-benar dapat dijangkau pada sanity 0. Tidak boleh pemain dibunuh ketika tindakan perlindungan telah diterima tetapi animasinya mengambil kontrol di area aman.

**Sanity bukan indikator penyakit/diagnosis dunia nyata**: mekanik supernatural fiksi. Efek strobo, blur, camera shake dan suara bisikan bisa diturunkan di Settings tanpa menonaktifkan gameplay clue; tetap tampilkan subtitle. Pada 0, fallback **PEMULIHAN** di Jurnal tidak dimanipulasi chat palsu.

### 4.2 Lapisan kebenaran dan verifikasi Live Chat

Pemain telah melihat infiltrasi chat di epilog P3. Di P4, `Penonton_Terakhir` sengaja memberi petunjuk yang kelihatan membantu tetapi bermaksud membimbing menuju gerbang kelima. *Verifier* dirancang sebagai **mekanik permainan**, bukan model NLP/LLM real-time atau koneksi Twitch/YouTube. Semua pesan berasal dari event authored deterministik. `sourceTag ∈ {HUMAN, ENTITY, ANTAGONIST, SYSTEM}` adalah metadata server/game internal dan **tidak pernah dijadikan label otomatis “BENAR” bagi pemain**. Tipuan berasal dari isi pesan dan penyamaran identitas, bukan classifier ajaib yang selalu benar.

| Elemen | Implementasi | Kontra fair-horror |
|---|---|---|
| Username “97” / underscore | Red flag estetis; makhluk bisa meniru nama penonton asli | Menebak nama/angka saja **tidak** cukup memastikan benar |
| Glitch karakter | Gejala pada beberapa akun hantu, meningkat saat Sanity rendah | Tidak selalu tersedia; pemain bisa menyelesaikan tanpa membaca chat |
| Lensa UV dunia | Mengungkap tanda fisik di tanah, akar, obor, mata tertutup | Status ini **tidak berubah** akibat chat/halusinasi UI |
| Badge UI “AMAN” / “ASLI” | Boleh dipalsukan melalui event khusus setelah pemain menerima foreshadow | Tidak boleh menutupi bentuk UV asli; prompt input host tidak bisa diganti |
| Echo Damar | Audio dapat direka Mbah Suro | Jejak langkah fisik/obyek tetap alat cek kedua |
| Keputusan player | Mengikuti chat bisa memperpanjang rute/menimbulkan ancaman dan Sanity loss | Tidak ada satu pun objective wajib yang hanya ditulis dalam chat |

**Dua fase transparansi:** Simpang 1 mengajari chat bisa salah saat Sanity masih tinggi; simpang 2 menunjukkan nama manusia bisa dikloning; simpang 3 memperlihatkan UI pseudo-system bisa mengandung tipuan. Ketika catatan Damar membongkar asal `MataBatin.apk`, pemain sudah mempunyai cukup bukti bahwa UI dapat disusupi tanpa merusak kepercayaan pada prinsip UV fisik. Performa: simulasi chat dibatasi jumlah akun/teks aktif dan history ring buffer; tidak ada spam ribuan item uGUI setiap frame. Overlay 999.999 viewer adalah teks statis/sinematik, bukan render 999.999 pesan.

### 4.2A Kontrak rekayasa chat dari Claude, diselaraskan dengan prinsip non-oracle

`ChatInfiltrationDirector` boleh memakai authored metadata `accountType ∈ {HUMAN,GHOST,ANTAGONIST}`, `lookalikeOf`, `claimTarget`, `truthful`, `glitchLevel`, `priority`, dan `sourceEvent`. Hanya *authoring tool/runtime validator* yang membaca keaslian; UI pemain tidak mendapat label bahwa akun pasti benar. Untuk menguji J1–J3 dan empat obor, sediakan set pesan minimal **satu petunjuk yang masuk akal dan satu umpan per lokasi**, tetapi **teks manusia bisa salah** dan **akun hantu bisa sengaja berkata benar** agar bukan soal membalik seluruh isi chat. Tujuan adalah cek rajah fisik.

Batasi **satu pesan/1,2 s saat chase** dan batasi viewport chat agar tidak menutup reticle, tanda bahaya atau `E` interaksi. Saat memakai Kemenyan, distorsi komentar dan halusinasi boleh mereda **20 s baseline** tanpa menjadikan Kemenyan syarat: mematikan HP tidak menghapus objective yang pernah diterima dan petunjuk dunia/Jurnal tersedia. Pada Sanity rendah, **Filter Anti-Sengkala menjadi kurang andal**, bukan sinar UV dunia yang berbohong. Detektor boleh menghilangkan glitch atau memalsukan status “penonton asli”; penanda “SYSTEM” palsu tetap harus terlihat sebagai konten chat, **tidak** mengambil alih action dispatcher permainan.

### 4.3 Banaspati — patroli bola api dan inti kerangka hangus

**[KANON]** Banaspati melayang sebagai bola api hidup di hutan/desa; suara ranting, rasa takut, detak jantung, dan berlari ceroboh mengundangnya. Pada tahap ritual ia terkait empat obor. Di klimaks, wujud intinya adalah kerangka manusia hangus yang terikat ke arwah Mbah Suro. **Model visual boleh bergaya non-fotoreal** dan harus mengikuti standar karakter desain proyek; jangan mengorbankan siluet/telegraph karena partikel berlebihan.

```text
BANASPATI_DORMANT
  -> REVEAL_DISTANCE (perlihatkan dari jauh, no instant hit)
  -> PATROL_AIR (waypoints terarah di empat cabang)
     |-- stimulus_loud_step / low_sanity / red-light -> INVESTIGATE(lastKnownPos)
     |-- player_visible & hunt_conditions -> CHASE_AIR
     |-- lose_LOS -> SEARCH_RADIUS -> PATROL_AIR
     |-- player_enters_valid_hide & screen_off -> PASS_OVER -> RETURN_PATROL
     |-- player_within_grab_volume after readable telegraph -> FIRE_LUNGE -> DAMAGE/FAIL
  -> RITUAL_ACTIVE -> SECTOR_INTERCEPT(sektor aktif)
     |-- AUTH_TORCH_WATERED -> EXPOSED_SKELETON_CORE (Blitz window)
     |-- VALID_LOCAL_BLITZ -> SECTOR_SEVERED, light sector remains off
     |-- MISS -> TORCH_RECOVERABLE, AI repositions, air refill remains
  -> THREE_SECTORS_SEVERED -> SOUTH_HUNT
  -> SOUTH_TORCH_OFF -> MERGE_AT_PENDOPO
  -> CORE_BLITZ_VALID -> SEALED
```

**Stimulus [BASELINE UJI]**: langkah normal radius 4 m, sprint 9 m, crouch 1.5 m; LOS player dengan sumber cahaya 12 m; `Sanity<40` menambah radius investigasi 1.5×, `Sanity=0` 2× dengan cooldown serangan tidak lebih pendek dari 2.5 s dan trigger telegraph 1.0 s. `PATROL_AIR` memakai waypoint di atas rute + *swoop anchor* yang disiapkan level designer; **jangan membuat 3D NavMesh terbang penuh**. Beberapa menit pertama: deteksi lebih toleran sampai pemain melihat bagaimana cara berlindung. Banaspati tidak spawn tepat di anchor checkpoint, recovery/air, atau di atas tempat perlindungan ketika pemain melakukan interaksi wajib.

**Wujud 1**: api pekat berputar dengan bidang cahaya dan pusat gelap, mata/penanda supernatural abstrak untuk indikasi arah (bukan target damage); sorotan merah memberi tahu lintasan. **Wujud 2**: siluet rangka hangus di balik api yang terbelah **hanya saat exposed/merge**, berkaitan dengan empat ikatan, tidak menjadi musuh tambahan untuk dihancurkan 4× secara terpisah. VFX dibuat dari mesh/partikel berlisensi/tercatat, bukan semua dinamis 4K.

### 4.3A Kontrak AI terukur dari Claude, tanpa menggantikan sistem threat P3

| State | Kecepatan baseline | Syarat/hasil | Guardrail |
|---|---:|---|---|
| `PATROL_AIR` | 1,5 m/s | Rute waypoint empat penjuru | Reveal jauh dan tidak langsung fatal |
| `INVESTIGATE/ALERT` | 2,7 m/s | Suara ranting, tampak layar, Sanity rendah | Target ke *last known position*, bukan teleport ke player |
| `CHASE_AIR` | 3,5 m/s | LOS dan stimulus cukup | Sprint aktual4,2 m/s (`FirstPersonController.Awake`); ruang menghindar tetap diuji |
| `RITUAL_CHASE` | 3,7 m/s saat hunt | Setelah CP5 sukses | Pengamatan UV dan siram harus bisa dilakukan dengan cover/telegraph |
| `STUNNED_CORE` | 0 m/s | Air obor asli → expose | Tidak dapat menyerang selama window Blitz 5 s (baseline) |
| `SEARCH` | 6 s (durasi) | Pemain berhasil memutus LOS | Keluar dari choke-point dan memberi kesempatan pulih |

**Dua kontak dalam 10 detik [BASELINE diambil dari Claude]**: kontak pertama (jika bukan i-frame) mengurangi **25 Sanity**, memberi *knockback* dan invulnerability pendek **3 detik**; kontak kedua yang sah dalam **10 detik** dapat fatal `CAUGHT_BANASPATI`. Tidak boleh kontak-ganda dari satu collider api dalam frame berturut-turut. Area ruwatan/hide yang menerima interaksi valid harus punya *grace* sesuai animasi. Nilai jarak/fatalitas perlu playtest 30/60 FPS dan accessibility difficulty; bila dua-hit terlalu tidak adil, kurangi intensitas, bukan menambahkan instakill.

**Selatan Mbah Suro [BASELINE Claude]**: lingkaran sebelum ledakan ditandai **≥1,2 detik**. Satu semburan dikenai **−30 Sanity** dan efek stagger nonfatal. Dua semburan yang benar-benar mengenai player dalam 10 detik boleh memicu `RITUAL_OVERRUN` **hanya bila** jalur berlindung nyata masih dapat dicapai dan 30/60 FPS memberi ruang reaksi. Bukan sumber damage otomatis setelah Sanity 0. Semburan tidak boleh menutupi simbol UV wajib oleh asap buram penuh.

### 4.4 Arwah Mbah Suro — sutradara kebohongan, bukan HP bar kedua

Arwah Mbah Suro tampil di atap Pendopo saat reveal, menulis komentar terinfiltrasi, memunculkan 2 obor umpan per sektor, dan memanggil ilusi suara Damar/Ibu Raka. Ia **bukan** unit melee permanen kedua dengan HP sendiri: selama sektor N/E/W ia berfungsi sebagai director/timed audio/visual, dengan `CANT_BE_DAMAGE` sampai sektor Selatan selesai. Pada sektor Selatan ia menutup sebagian pandangan melalui asap, namun tetap menyediakan jalur shelter ke obor benar. Pada merge Pendopo, Mbah Suro dan Banaspati membentuk **satu target Blitz keempat**, sehingga pemain tidak diminta serangkaian 3+3 jepretan.

**Lore dari Cermin Saksi [KANON sumber]:** pembakaran warga 1997, Mbah Suro pergi dari Karang Setra, wafat setelah ritual Rusunawa 2004, arwah kini terikat di desa. Detail “seluruh warga” berasal dari artikel forum/antagonis dan bisa dipresentasikan sebagai klaim narasi, sementara Cermin Saksi memberi perspektif korban yang menolak ritual. Jangan menulis karakter Mbah Suro “masih hidup secara fisik pada malam P4”.

### 4.5 Stealth dan safe-recovery tanpa eksploitasi

Tempat sembunyi kanon: Liang Kubur Terbuka, Kolong Gubuk Panggung, dan Gentong Air Tanah; ditambah shelter Lumbung / ceruk Sumur yang ditunjuk melalui pola lingkungan. `F` mematikan HP; suara napas/detak jantung meningkat saat Sanity rendah tetapi tidak langsung menembus cover. **Safe spot bergantung pada collider, sudut pandang dan AI radius**, bukan flag invulnerable global. Tempat perlindungan untuk recovery memiliki entrance & exit dua arah atau waktu AI move-away agar tidak terjadi camping tanpa jalan keluar. Banaspati tak dapat menyerang dari dalam portal scene load/animasi cutscene; setelah keluar ia kembali merespon input dunia dengan telegraph.

### 4.6 Semantik 12 obor dan sumber air

Setiap obor kandidat memiliki salah satu tipe `AUTHENTIC_N/E/W/S` atau `DECOY_1/2`, bersama *UV signature* dan `lightLit` runtime. `hasAuthenticMark` **tidak boleh berasal dari warna UI chat** atau username. Penyiraman palsu memakai 1 muatan air (jika aksi accepted), memberi penalti Sanity 40 poin + semburan api, tetapi **tidak** mengubah `authenticTorch` atau `sectorComplete`. Pemadaman obor benar memakai 1 muatan, menyingkap inti yang bisa difoto. Bila pemain gagal Blitz, obor kembali menyala setelah recovery singkat; tanda UV masih bisa dicek lagi, Kendi dapat diisi ulang, generator/pelindung tidak habis.

**Kendi Air Sumur Keramat** adalah pusaka dan alat volume. Simbolik diletakkan di altar bersama Belati dan Tanah, tetapi saat `P4_RITUAL_TRIGGER` berhasil pemain menerima izin memegang **kendi yang sama** sebagai alat (bukan clone ID inventori). Saat inventory/altar diperiksa, gunakan state `RITUAL_OFFERED` dan `KENDI_IN_HAND` yang saling eksklusif secara objek visual; offering tetap sah. Kapasitas awal **3 muatan [BASELINE pilihan Claude]**, isi ulang di Sumur Barat **tanpa batas** dengan interaksi tahan E selama 4 s. Empat obor asli memastikan setidaknya satu pengisian ulang gratis, kecuali pemain memakai Garam Ruwatan opsional. Sesudah ritual aktif, saluran ruwatan alami dari sumur boleh mengalir ke **tempayan penampung di pendopo** atau alur irigasi setiap sektor agar player tidak dipaksa bolak balik menyusuri seluruh desa untuk setiap kesalahan; semua bersumber diegetik dari sumur, bukan item toko gratis misterius. Air tidak memulihkan baterai **maupun Sanity** secara otomatis.

Paket “Kantong Air Keramat/Garam Ruwatan ×2” di toko dinormalisasi sebagai **Garam Ruwatan** (satu konsumsi mengisi kendi sampai kapasitas maksimum di mana saja), memberi **dua isi cepat** atau menghemat waktu refill; bukan item keempat yang wajib. Garam dapat menjadi animasi alternatif untuk efek decoy *stagger*, tidak boleh menghalangi ritual yang secara kanon menyebut air sebagai syarat. Toko memakai katalog optional yang disetujui desainer nanti (§5.4).

### 4.7 Ritualitas boss empat penjuru — state yang membedakan P4 dari P1–3

**Sebaliknya dari pola ronde 1–3 boss ruang tertutup**, pemain bebas mengerjakan **Utara, Timur, dan Barat** dahulu sesuka hati. Tiap sektor mempunyai *encounter ringkas dan perilaku berbeda*:

- **Utara / Makam:** Keluar-masuk barisan batu nisan dengan Banaspati melayang rendah; Thermo +66°C dari bukti memperkenalkan *false hot torches* (yang paling panas belum tentu asli), jadi perlu UV fisik. Setelah siram obor benar, inti exposed; satu Blitz.
- **Timur / Gubuk Dukun:** Tutup samping gubuk dapat dipakai untuk memutus LOS. Pesan Damar dan decoy chat palsu sengaja berkonflik. Pemain harus verifikasi UV obor tanpa bergantung pada suara yang mengaku “Damar”. Satu Blitz setelah air benar.
- **Barat / Sumur:** Batas ruang dekat sumber air memberi kesempatan refill. EVP mantra memperjelas empat penjuru, salah satu sumber chat mengaku petunjuk resmi. Ruangan relatif lebih sempit; jangan membuat Banaspati menyerang saat interaksi sumur terkunci. Satu Blitz.
- **Selatan / Lumbung:** **Baru aktif sebagai konfrontasi final setelah 3 sektor selesai**. Banaspati mengejar lebih agresif, Mbah Suro menambah asap dan tiruan Ibu Raka. Pemain tetap punya shelter/air/UV; memilih obor benar + menyiramnya mematikan **obor keempat**, langsung menarik entitas menuju inti di Pendopo. **Tidak ada Blitz lokal sektor Selatan**.
- **Pendopo / Inti:** Banaspati wujud terbuka bersatu dengan Mbah Suro. Kamera memberi target jepret asli, satu **Blitz final keempat**; bila meleset, timer recovery dan kesempatan paparan ulang pada inti, **tanpa mengulang tiga sektor dan tanpa menghidupkan kembali obor Selatan**.

**Blitz akhir dihitung:** `seal_hits = {N?,E?,W?,CORE?}` → persis `4` jika semua true. Jangan menghitung `S_OFF` sebagai seal hit. `location_seals` global bertambah **satu ID** hanya pada kemenangan (`SEAL_DESA_MATI_KARANG_SETRA`), bukan 4 lokasi/4 chapter seal.

### 4.8 State mesin ritual dan retry

```text
RITUAL_PREVIEW (tiga offering terlihat; jalur ke sektor tetap terbuka)
  -> valid interact Pendopo: ATOMIC CP5 -> RITUAL_ACTIVE
  -> sector N/E/W chosen in any order
  -> IDENTIFY (UV signature stable) -> WATER_AUTHENTIC (consume 1 water in attempt)
     -> EXPOSED_5S [BASELINE]
     |-- BLITZ_MISS / camera battery<15 / timeout:
     |     AUTH_TORCH_RELIGHTS -> RETRYABLE; refill + recharge reachable
     +-- VALID_BLITZ:
           ATOMIC SECTOR_COMMIT; sector permanently cleared
           resume player at safe checkpoint anchor, no repeat required
  -> N/E/W all committed
  -> SOUTH_ACTIVE -> IDENTIFY_S -> WATER_AUTHENTIC_S -> SOUTH_OFF_COMMIT
  -> CORE_PENDOPO (intact floor and charge accessible)
     |-- MISS: exposed recovery -> CORE_RETRY (no torch reset)
     +-- VALID_FINAL_BLITZ: CP7 ATOMIC -> SEALED
  -> PORTAL_REVEAL -> optional secret/backtrack allowed until exit confirmation
  -> EXIT_PANEL -> P4_COMPLETE CP8 ATOMIC -> PHASE5
```

**Checkpoint policy yang sengaja berbeda dari P3:** Tiga ikatan N/E/W **tidak terhapus setelah kematian atau alt-tab/force quit jika commit sukses**; fase terlewat tidak diwajibkan diulang. Setelah sukses sah, aksi `P4_BLITZ_N` dilakukan bersama transaksi sektor atomik; jika gagal write, UI belum menampilkan “segel putus” dan state sementara di-*rollback* ke percobaan sektor bisa diulang. Saat terjadi crash sebelum pengesahan, restore dari CP5/sektor terakhir. Pilihan ini membuat empat lokasi terasa sebagai pengejaran bertahap, bukan reset semua saat sekali mati. **Jangan mengunci seluruh autosave selama 20 menit encounter terbuka** seperti arena P3; lakukan transaksi di momen aman/after sector.

### 4.9 Kamera, durasi paparan dan kapasitas recovery

**[BASELINE UJI]** paparan inti 5.0 detik per sektor, target chest/core meteran ≤8 m, cooldown Blitz global 6 s, baterai biaya 15 poin. P4 tidak menegaskan jendela 5 detik secara eksplisit; dipakai sebagai referensi seragam yang **boleh dituning**. Boss diam tidak menyerang saat exposed, dan waktu berhenti saat pause/unfocus bila kontrol hilang. `Sanity=0` tidak mempercepat window menjadi 0 atau memalsukan hitbox. Tidak ada tembakan valid bila baterai <15; sumber daya dapat diisi dari **dinamo manual fisik** di Pendopo, Sumur, dua titik sektor dan shelter Selatan [USUL BARU]. Recovery: +25 poin / 8 detik satu aktivitas engkol; ulang melalui mekanik cooldown dan perlindungan agar salah tembak 100× tetap bisa dituntaskan tanpa membeli powerbank. **Tidak** ada recharge otomatis pada setiap retry; save snapshot battery aktual. Satu konsekuensi balancenya: baterai dan waktu adalah tekanan, bukan softlock.

### 4.10 Failure model dan telegraph

| Kondisi | Telegraphed dan akibat | Pemulihan |
|---|---|---|
| Menerobos rajah tengkorak ungu | UV memperlihatkan tanda, abu berdesis, api mengembang dengan jeda reaksi | Kembali ke checkpoint/anchor; pelajaran tidak dihapus |
| Banaspati menangkap di tempat terbuka | Cahaya merah dari jauh, detak dan suara laju api, window menghindar | Kematian/retry restore sektor completed dan pickup atomik |
| Sanity sampai 0 | Distorsi maksimum dan hunt, **tidak otomatis fatal** | Batu perlindungan, cover, kemenyan jika punya |
| Siram obor palsu | Silang merah terlihat UV, semburan berperingatan, −40 Sanity | Kendi isi ulang, decoy pulih, tidak mengunci benar |
| Blitz meleset | Perisai kembali, air habis untuk percobaan sementara | Refill, physical recharge, retry tanpa menghilangkan sektor selesai |
| Catatan Damar belum dibaca | Twist tidak muncul di Jurnal hingga pickup; story timeline ending tetap paham gerbang | Dokumen opsional dibaca kemudian jika rute masih terbuka |
| Portal dikonfirmasi tetapi disk gagal | Error penyimpanan jelas; player tidak pindah scene | Tetap di Pendopo dan ulang commit |

---
## 5. Inventori, Jurnal Klenik, rahasia dan ekonomi

### 5.1 Master data Phase 4

| Tipe | Nama / ID konseptual | Asal | Hasil |
|---|---|---|---|
| Dokumen bonus | Forum Tragedi 1997 · `P4_DOC_FORUM` | PC Kos | Aturan empat obor; bacaan opsional |
| Dokumen #1 | Kitab Rajah Keempat · `P4_DOC_RAJAH_4` | Pendopo | Dasar tiga pusaka, ritual, obor palsu |
| Dokumen #2 | Sobekan Buku Damar · `P4_DOC_DAMAR_LAST` | Gubuk Timur | Asal MataBatin.apk dan tujuan Pasar Setan |
| Story prop | Kacamata Retak Damar · `P4_DAMAR_GLASSES` | Pendopo | Narasi; tidak dihitung sebagai bukti |
| Bukti #1 | Anomali Suhu +66°C · `P4_EVIDENCE_THERMO_66` | Kuburan Utara | Thermo target sah; evidence 1/3 |
| Bukti #2 | Aura Altar · `P4_EVIDENCE_AR_ALTAR` | Gubuk Timur | Foto AR; evidence 2/3 |
| Bukti #3 | Mantra Empat Penjuru · `P4_EVIDENCE_EVP_4DIR` | Sumur Barat | Rekaman EVP valid; evidence 3/3 |
| Pusaka #1 | Kendi Air Sumur Keramat · `P4_RELIC_KENDI` | Kuburan Utara | Offering altar + alat siram (instance sama) |
| Pusaka #2 | Belati Perak Ruwatan · `P4_RELIC_BELATI` | Gubuk Timur | Offering altar, bukan senjata melee |
| Pusaka #3 | Kantong Tanah Kubur Keramat · `P4_RELIC_TANAH` | Sumur Barat | Offering altar |
| Rahasia | Keping Cermin Saksi · `SECRET_P4_CERMIN_SAKSI` | Relung Sumur Barat | Arsip Saksi opsional |
| Consumable | Paket Kemenyan ×5 | Pasar Gaib Kos, opsional | Pulihkan Sanity, instance unik |
| Consumable | Paket Powerbank Besar ×3 | Pasar Gaib Kos, opsional | +80 baterai per unit |
| Consumable | Garam Ruwatan ×2 (nama baku paket Air Keramat) | Pasar Gaib Kos, opsional | Isi cepat; sumur gratis tetap utama |

**Receipt terpisah:** `evidence_ids` untuk scan/foto/rekaman; `document_ids` untuk dokumen; `inventory` untuk benda dibawa; `relic_ids` untuk benda ditemukan; `secret_items` untuk arsip; `completed_event_ids` untuk ritual. Foto bukti dan mengambil pusaka berdekatan tetap dua interaksi. Satu ID evidence tidak bertambah karena tombol dipencet dua kali atau reload.

### 5.2 Jurnal Klenik dan filter informasi

Jurnal mempertahankan tab P3: **Bukti**, **Dokumen**, **Arsip Saksi**, dan **Petunjuk Objektif**. Status `Bukti 0/3…3/3` dihitung dari tepat tiga ID P4, bukan jumlah seluruh evidence lintas babak. Tombol `[KONFIRMASI IDENTITAS & BUKA RITUAL]` hanya tersedia pada 3/3 dan mencatat receipt sekali. Deduksi: `[ENTITAS: BANASPATI + ARWAH MBAH SURO]`; metode: `[VERIFIKASI OBOR ASLI SECARA UV, GUNAKAN AIR RUWATAN, PUTUS TIGA IKATAN, PADAMKAN PENJURU KEEMPAT DAN SEGEL INTI DI PENDOPO]`. Untuk menghindari misleading, Jurnal boleh menjelaskan bahwa **3 sektor memberi Blitz dan sektor keempat memberi Blitz inti saja** setelah pemain memperoleh Kitab Rajah; jangan sembunyikan aturan empat tembakan sampai pemain terkena softlock.

Tab tambahan `Status Penjuru` menampilkan **Utara: terputus/aktif, Timur: terputus/aktif, Barat: terputus/aktif, Selatan: obor padam/belum**, serta **Inti: belum/tersegel**. Jangan menampilkan label “4/4 segel dunia” setelah empat obor, karena total lokasi terkutuk baru menjadi 4 setelah boss inti kalah. Pada Sanity rendah, dekorasi/glitch boleh muncul di panel chat, tetapi Jurnal faktual untuk bukti, konsumen dan sektor **tidak boleh menghapus fakta yang sudah committed**.

### 5.3 Arsip Saksi — Keping Cermin Saksi

**[KANON]** `SECRET_P4_CERMIN_SAKSI` ada pada relung kering dinding Sumur Keramat Barat, bukan di dekat Kantong Tanah wajib. Tiga langkah: tunggu patroli Banaspati lewat dan matikan layar; verifikasi **ketukan asli** dengan EVP pada saluran relung; sorot tanda UV mata tertutup; putar roda sumur ke simbol mata tertutup, buka laci dan ambil Cermin. Jangan mensyaratkan menyiram obor. Jejak korban mengatakan warga menolak ritual 1997; sisi belakang menguatkan Mbah Suro meninggalkan desa dan wafat seusai ritual Rusunawa 2004. Puzzle harus dapat dikerjakan sebelum atau setelah kemenangan inti jika rute sumur masih terbuka.

**Pickup:** `TryCommit` atomic tambah ke `secret_items`, simpan data **sebelum** popup sukses, sembunyikan prop **sesudah** committed. Jika commit gagal, prop tetap ada. Jurnal dan panel portal menampilkan `Arsip Babak 4: 0/1` atau `1/1`, dan `Arsip Saksi Global: N/5` yang dihitung dari **jumlah aktual ID unik**. Jangan memalsukan 4/5 jika pemain melewatkan rahasia P1/P2/P3. Empat rahasia pertama maksimal memberi 4/5 bila semuanya dimiliki; tujuan Arsip ke-5 milik P5 masih belum terpecahkan.

### 5.4 Pasar Gaib, carry-over dan dana terikat

Paket naskah: Kemenyan ×5, Powerbank Besar ×3, Air/Garam Ruwatan ×2. Naskah **tidak menetapkan harga**, sehingga tarif berikut hanya **[BASELINE UJI]** dan menunggu ratifikasi: Kemenyan Rp875.000 untuk 5 (Rp175.000/unit dari baseline P3), Powerbank Rp750.000 untuk 3 (Rp250.000/unit), Paket Garam Ruwatan Rp400.000 untuk 2 (Rp200.000/unit; baseline Claude). Nilai total bila dibeli semua **Rp2.025.000** (Rp875.000 + Rp750.000 + Rp400.000; hanya baseline uji). Pasar Gaib hanya memakai `wallet_legitimate` dari save dan tidak memanfaatkan dana terikat. Jika saldo 0, semua tombol beli nonaktif **tanpa memblokir upgrade gratis, berangkat, ritual atau pemulihan**. Hindari “kemenyan sangat wajib” pada UX toko agar pemain tidak salah menafsirkan paywall.

**Penonton_Terakhir** memberi **Rp6.666.000** sebagai dana terikat pada pembuka siaran; tidak menambah saldo belanja. Penonton 780.000 di awal dan 999.999 saat epilog adalah overlay naratif, sedangkan `Rp999.999.999 [ERROR]` adalah efek serangan UI yang **tidak** menambah/menghapus saldo aktual. Nominal kontrak Rp500.000.000 dalam monolog antagonis adalah janji naratif, bukan reward player yang cair tanpa transaksi. Tampilkan empat angka keuangan terpisah (saldo sah, terikat, overlay naratif, belanja sah) sesuai P3; jangan buat transaksi fiktif untuk merekonsiliasi angka cerita.

---

## 6. CampaignState, checkpoint dan fault tolerance

### 6.1 Model data tunggal dan kompatibilitas

Lanjutkan `CampaignData` existing dengan representasi Phase4 yang terbatas pada event/sector state, Sanity aktif, volume air, dan log cerita. Jangan menggandakan field global `inventory`, `battery`, `sanity`, `wallet_legitimate`, `wallet_bound`, `secret_items`, `location_seals`. Snapshot persisten menyimpan **receipts bukti/pusaka, sektor N/E/W yang benar-benar sudah disegel, status obor Selatan, boss win, story docs, chapter exit**. `Banaspati currentTransform`, `currentChatMessageIndex` animasi, `currentFalsePrompt`, `Sanity shaderNoise`, `torchExposureTimer`, dan `actualTorchToLookAt` runtime **tidak** dibuat sebagai sumber data duplikat. Random seed layout obor disimpan satu kali per campaign attempt yang sudah memulai ritual, supaya status lokasi asli tetap stabil antara Continue/retry.

**Schema save:** P3 v2.3 mencatat bahwa implementasi memakai schema 3 yang dimigrasikan ke 4 dan masih memerlukan bukti aktual. Untuk P4 **jangan langsung mewajibkan schema 5** sebelum memeriksa loader Unity terbaru. Jika migrasi dibutuhkan, lakukan `n→n+1` melalui jalur normal repo dengan snapshot backup, pertahankan P0–P3, semua consumable, rekam receipt dan ID unik. Tambah `Phase4Data` bila pola P3 memang demikian di repository; kalau API aktual telah berubah, adaptasi dan catat audit.

### 6.2 Contoh kontrak state konseptual (BUKAN JSON loader final)

```json
{
  "checkpoint_id": "PHASE4_CP5",
  "chapter_id": 4,
  "evidence_ids": ["P4_EVIDENCE_THERMO_66", "P4_EVIDENCE_AR_ALTAR", "P4_EVIDENCE_EVP_4DIR"],
  "relic_ids": ["P4_RELIC_KENDI", "P4_RELIC_BELATI", "P4_RELIC_TANAH"],
  "secret_items": ["SECRET_P3_PELAT_BURUH"],
  "location_seals": ["SEAL_RUSUNAWA_GRIYA_ASRI", "SEAL_RSUD_LAMA_SIDO_WARAS", "SEAL_PABRIK_GULA_SINDANG_LAUT"],
  "battery": 45,
  "sanity": 62,
  "phase4": {
    "completed_event_ids": ["P4_MODULE_CALIBRATED", "P4_JOURNAL_CONFIRMED", "P4_OFFERINGS_PLACED", "P4_RITUAL_TRIGGER"],
    "document_ids": ["P4_DOC_RAJAH_4"],
    "sector_committed_ids": [],
    "south_torch_off": false,
    "torch_layout_seed": 17,
    "kendi_water_units": 3,
    "kos_charge_used": true,
    "shop_receipts": [],
    "safe_restore_anchor_id": "P4_PENDOPO_WEST_ENTRY"
  }
}
```

**Penting:** isi array dan angka di contoh bukan awal kanon; `secret_items` mungkin tidak kosong dan jumlahnya tergantung pilihan P1–P3. Data ini tidak mengklaim `CampaignState` aktual sudah mendukung field di atas. Khusus `sector_committed_ids`, gunakan tiga ID unik sesuai namespace registry; status sektoral di Jurnal dihitung dari array ini. Saat menulis parser Unity yang memakai `JsonUtility`, utamakan list serializable + deduplikasi saat runtime; jangan membuat dictionary/HashSet serialisasi tanpa adapter yang terbukti.

### 6.3 Checkpoint Phase4 (proposal diselaraskan dengan empat penjuru)

| ID | Waktu/save trigger | Anchor/keadaan saat restore |
|---|---|---|
| `PHASE4_CP1` | Setelah tiba Hutan Bambu, kontrol returned | Baterai dan saldo hasil Kos, Sanity snapshot, chat pembuka idempoten |
| `PHASE4_CP2` | **Memasuki hutan sebelum Simpang J1**, mengikuti pilihan checkpoint Claude yang lebih adil | Anchor jalur masuk J1, Sanity/baterai snapshot aktual; tutorial chat bisa diulang tanpa duplikasi donor/loot |
| `PHASE4_CP3` | Pendopo pertama, sebelum Banaspati reveal | Area aman sebelum ancaman aktif, semua P3 receipt/gear utuh |
| `PHASE4_CP4` | Pickup bukti/pusaka/dokumen di tiga sektor investigasi | Item tertulis atomik dengan safe anchor yang bukan titik patrol agresif |
| `PHASE4_CP5` | Tiga pusaka + Jurnal dikonfirmasi, interaksi mulai ritual pada batu Pendopo | Snapshot pra-ritual; kapasitas kendi3 dengan isi aktual0–3 dari satu instance yang sama, Sanity dan baterai sebenarnya; layout seed disimpan; tidak ada sektor completed |
| `PHASE4_CP6` | **Setiap** sektor N/E/W sukses, atau obor S off; label sama, `sector_committed_ids` bertambah | Snapshot terbaru di safe anchor; semua sektor sebelumnya tetap selesai; air/baterai Sanity sesuai mutasi sah |
| `PHASE4_CP7` | Blitz inti keempat valid, **boss win** | Segel keempat + entitas resolved + portal terlihat, semua sektor selesai |
| `PHASE4_CP8` | **P4_COMPLETE** saat konfirmasi turun portal | Tujuan P5 Pasar Setan; Arsip Saksi & semua status carry-over; aman dari duplikasi |

`CP6` adalah **slot checkpoint bergerak** dengan set receipts sektoral, bukan CP6N/CP6E/CP6W yang harus diregistrasikan menjadi checkpoint berbeda di router. Saat pertama kali sektor sukses CP6 menyimpan `1/3`, berikutnya `2/3`, lalu `3/3`; hanya commit terkini yang boleh di-*load*. Jangan pernah menurunkan checkpoint ke CP4 saat pemain mengambil rahasia setelah tiga sektor. Bila secret pickup sesudah boss, commit data ke checkpoint CP7 dengan anchor tetap, bukan menghidupkan boss kembali. Semua checkpoint harus menghindari posisi tepat di bawah api, di dalam collider dekorasi, atau di jalur portal sebelum commit.

### 6.4 Atomic transactions dan rollback penuh

```text
TryAddEvidence(P4_EVIDENCE_*) -> mutate unique set -> TryCommitCampaign()
  on FAIL: rollback ALL fields changed, keep interactable available

TryTakeRelic(P4_RELIC_*) -> update inventory + relic_ids + quest receipt in one commit
  on FAIL: retain physical object, no notification success

TryCollectMirror():
  if EvpKnockVerified && UvEyeVerified && WellWheelAligned:
    TryCommitCampaign(addSecretOnce=SECRET_P4_CERMIN_SAKSI)
    if SUCCESS: hide mirror & show Global N/5

TryStartRitual():
  if JournalConfirmed && AllRelicsOffered && TryCommitCampaign(CP5):
    StartRitualRuntime()  // never before successful disk commit

TryLocalSectorSeal(sector in N,E,W):
  if RealTorchDoused && ValidBlitz && not AlreadyCommitted(sector):
    TryCommitCampaign(CP6, addSector=sector, setTorchOff=true,
                      waterBalance=alreadyCommittedAfterPour, batteryAfterBlitz=actual,
                      sanity=actual, safeAnchor=sectorExit)
    if FAIL: rollback temporary success and keep sector retryable

TrySouthDouse():
  if N&&E&&W committed && IsTrueSouthTorch && WaterUnits>0:
    TryCommitCampaign(CP6, southTorchOff=true)
    if SUCCESS: spawn merge core / no local Blitz

TryCoreSeal():
  if SouthOff && N&&E&&W && ValidFourthBlitz:
    TryCommitCampaign(CP7, addSeal=SEAL_DESA_MATI_KARANG_SETRA,
                      addEvent=P4_BOSS_WIN, bossResolved=true)
    if SUCCESS: trigger victory once

TryPortalExit():
  if CP7 persisted && playerConfirmed && not P4_COMPLETE:
    TryCommitCampaign(CP8, addEvent=P4_COMPLETE, nextChapter=5)
    if SUCCESS: transition to Pasar Setan
```

**Air dan snapshot selama miss:** konsumsi air dilakukan tepat sekali pada aksi siram yang diterima; transaksi Blitz berikutnya **tidak** mengurangi air lagi. Apabila siram salah atau benar tetapi Blitz meleset, penggunaan air pada attempt harus dikomit atau dibatalkan sesuai kebijakan transaksi kecil yang nyata: **jangan dua-duanya**. Baseline memilih penggunaan air tercatat segera pada aksi accepted, juga untuk salah siram, dengan safe anchor tertinggi dan sumur refill gratis. Jika player force quit tepat setelah siram dan sebelum save, ia memuat *last committed state*, sehingga tidak dapat berada dalam keadaan obor padam tanpa air ledger. Untuk Blitz yang valid sektor commit, batch baterai−15 dan receipt segel di satu transaksi; saat gagal write UI belum memberi kemenangan. Hindari *write barrier global permanen* karena P4 mengandalkan progres tiap sektor. Saat cutscene core, barrier ringkas boleh dipakai sampai CP7 atomik.

### 6.5 Recoverability setelah crash dan P5 transition

CP7 perlu menjadi titik aman untuk kembali menjelajah ke Sumur dan mengambil rahasia jika terlewat. CP8 hanya dibuat setelah pemain melihat panel Arsip dan memilih turun portal; gagal menulis tidak memulai P5. Jika `PHASE5_ENTRY` memuat data dari CP8, pastikan P4 no longer reruns epilog di scene lain. Jangan paksa baterai “1 bar” di P4 akhir: saat P5 dimulai, pemain membawa nilai baterai dan Sanity yang sah. Scene P5 harus menempatkan sumber cahaya/pemulihan diegetik di area masuk bila P4 tamat dengan 0% baterai atau Sanity agar pemain tidak softlock di frame pertama.

---
### 6.6 Orakel transaksi P4: satu keberhasilan tidak menghasilkan dua sumber kebenaran

**Perbedaan mendasar dari Phase 3:** ritual P4 memiliki *safe commit boundaries* sesudah setiap ikatan, bukan satu write barrier sepanjang boss. Sistem tidak menulis transform Banaspati atau frame paparan, tetapi *harus* mengizinkan `TryCommit` atomik pada pickup, isi air, konsumsi item, receipt sektor, dan rahasia walau `RITUAL_ACTIVE` masih berlangsung. Larangan global semua save selama boss yang berlaku pada CP5 P3 **tidak diwarisi** di sini.

**Kesepakatan retry terukur:**

1. Menyiram obor palsu mengonsumsi tepat satu muatan, mengurangi 40 Sanity, dan membuat receipt konsumsi melalui transaksi aman. Jika penulisan disk gagal, rollback seluruh konsumsi serta feedback; obor tetap dalam status sebelumnya.
2. Menyiram obor benar membuka exposure sesaat; *air berkurang tepat sekali*. Jika pemain miss atau window habis, obor kembali siap untuk diaktifkan ulang dengan **siram baru** dan pengisian tetap selalu mungkin—bukan water cost hilang di memori yang memproduksi air duplikat saat force-quit.
3. Blitz sah di N/E/W melakukan satu commit: pengurangan baterai yang terkait aksi, receipt sektor, obor lokal `off` permanen, **CP6 rolling** dan anchor aman. Jika transaksi ditolak, status menang lokal tidak diumumkan; aksi dan pengurangan sebelumnya tidak dibayar dua kali.
4. Sesudah N/E/W komit, **pintu sektor Selatan aktif**. Menyiram obor Selatan melakukan commit `south_torch_off` pada CP6. Jika crash sebelum mencapai inti, reload menaruh Raka di jalur aman menuju Pendopo dengan Selatan tetap padam; **tidak perlu Blitz Selatan**, hanya inti.
5. CP7 adalah commit final 4 segel (1 ID tambahan untuk Phase 4), boss resolved, portal opened; CP8 konfirmasi turun P5. Kegagalan CP8 tidak menghilangkan CP7 atau teleport player tanpa file save sukses.

**Biaya input yang dipisah:** bila controller global sudah mengurangi 15 baterai pada setiap Blitz *saat ditekan*, receipt sektor harus menyerap perubahan itu dalam transaksi yang tepat—jangan melakukan `deductBattery(15)` lagi di director P4. Demikian juga `UseKemenyan` dan `UseGaram` memanggil inventory owner global; event P4 tidak menambahkan atau menghapus instance kedua. Terapkan idempotency key per `attemptId:sectorId:actionSequence` jika arsitektur actual membutuhkan, bukan ID string baru setiap retry tanpa batas.

**Jaminan resource:** world `WaterWell` dan dinamo bukan reward sekali pakai. Item Kendi adalah *satu identitas persist* walau animasi offering memindah prop dari tangan ke altar lalu kembali ke tangan. Pada 0% baterai, `E/F/H` dan jalan fisik tetap bekerja; untuk memotret diperlukan charger dengan landmark dan rute aman. Setiap commit selama encounter memulihkan posisi dari **safe anchor** yang tidak berada tepat di ray serangan setelah reload.

## 7. Visual, VO, audio, atmosfer dan asset provenance

### 7.1 Shot list naratif / blocking sinematik

| Bagian | Gambar/suara utama | Arah emosi dan informasi gameplay |
|---|---|---|
| Kos sore | Sinar senja jatuh ke kopi dingin, peta bintang segel dan mata Raka lelah, berita Malam Keramat di TV | Jeda psikologis dan bukti publik tetap tak percaya |
| Perjalanan malam | Motor masuk jalan menurun berpepohonan bambu, jembatan lapuk | Isolasi; tidak membawa Kos sebagai safe room ke P5 |
| Hutan simpang 1 | Tiruan Damar dari arah nisan, tengkorak UV terlihat pada batang | Pemain menyadari petunjuk chat bisa mematikan |
| Simpang 2 | Komentar username hampir sama, bisik tidak bersumber | Kesalahan pembaca bukan kesalahan refleks |
| Simpang 3 | Pesan “sistem” palsu, bambu bergetar dan jalan kembali berulang | Patahkan kepercayaan pada HUD |
| Pendopo reveal | Abu melayang, tawa kakek di atap, bola api melintas di atas pilar | Banaspati tampak kebal; zona aman memberi waktu melihat |
| Kuburan | Cahaya merah mengitari nisan, angka +66°C Thermo di target yang benar | Bukti suhu tidak bisa diperoleh dengan kamera biasa |
| Gubuk | Dinding anyaman ilusi, sosok bayangan di altar, sobekan kertas tulisan Damar | Twist asal aplikasi, bukan instruksi menghancurkan HP |
| Sumur | Ketukan lembut tiga kali, ripple ember tua, relung kering tersembunyi | Kontras air/Api dan rahasia warga yang menolak ritual |
| Ritual N/E/W | Sorot UV pada tiga obor kandidat, mata ritual retak setelah air, inti hangus tampak | Kemenangan berasal dari verifikasi dan rute, bukan spam Blitz |
| Selatan | Kabut pekat, kain bambu terbakar, suara gabungan Ibu Raka/Damar | Intensitas tertinggi, opsi berlindung tetap tersedia |
| Inti Pendopo | Api terbelah, kerangka hangus dalam satu tubuh dengan arwah Mbah Suro | Satu Blitz final keempat; pelepasan besar |
| Gerbang Pasar Setan | Lantai pendopo pecah membentuk tangga, riuh pasar dari bawah, viewer 999.999 | Twist: empat segel justru kunci gerbang kelima |
| Exit | Arsip Saksi terlihat, kutipan “MALAM KERAMAT” tertinggal di ponsel, CKREK | Kontinuitas ke P5; bukan kemenangan heroik sederhana |

### 7.2 Identitas artistik Desa Karang Setra

Gunakan referensi **desa mati di kaki bukit Jawa**, bukan desa Eropa. Arsitektur: pendopo kayu yang pernah dibakar, batu ritual batik/rajah, pagar bambu, gubuk panggung, lumbung padi, sumur batu dengan roda besi, makam tua bersusun. *Set dressing*: daun bambu hangus, kain jarik, bekas cap tangan arang, bambu pecah, keranjang hasil panen dan payung ritual kusam. Seluruh properti diunduh dari pustaka **berlisensi dan tercatat**, atau dibuat dari transformasi/assembly aset berlisensi sesuai kebijakan proyek. Tidak mengklaim photorealism setara DreadOut 2 hanya karena ada 50 prop. Target estetika: pencahayaan diegetik berkualitas, material lembap/abu masuk akal, gradasi warna malam yang tidak terlalu gelap untuk navigasi UV.

Arah model Banaspati: api hampir tak berbentuk untuk patrol, inti kerangka hangus abstrak di merge; visual kepala dapat disederhanakan/ditutup oleh api sesuai keputusan karakter. Arwah Mbah Suro tidak perlu detail wajah hiperrealistik agar tetap mengerikan. Jangan memenuhi frame dengan bloom merah pekat sampai simbol UV merah/ungu tidak terbaca; shader UV punya kontras dan shape terpisah.

**Target mutu produksi [BASELINE]**: **minimal100 identitas model baru dari internet yang benar-benar ditempatkan dan sesuai fungsi cerita**, di luar tekstur/audio/shader dan penggandaan instance. Seluruh lingkungan Desa/Bambu baru; Raka, HP, motor dan Golden Kos adalah kontinuitas kampanye, tidak dihitung pada100. Kredit dan manifest membuktikan identitas sumber serta penempatan, bukan sekadar file unduhan. Cantumkan `Audit/phase4-asset-sources.json` dan `Audit/phase4-asset-credits.md` berisi URL, penulis, jenis lisensi, SHA-256, asset placement, izin komersial dan perubahan impor. Jangan mengunduh aset yang status lisensinya tidak jelas. Retarget animasi karakter dengan legal source dan skeleton yang kompatibel, termasuk gestures camera/HP Raka.

### 7.3 Rancangan audio dan prioritas

| Bus/cue | Material audio | Prioritas |
|---|---|---|
| `AMB_BAMBOO` | Bambus gesek “KREK… KREEK”, daun menyapu, serangga jauh; intensitas meningkat perlahan | Ambient bawah dialog |
| `AMB_PENDOPO` | Angin melintasi tiang kosong, debu abu, bunyi logam jauh | Nyaris sunyi sebelum reveal |
| `BNS_FIRE` | Pusaran api, embusan panas, letup bara mengitari posisi musuh | Lokalisasi stereo/3D jelas; pemain bisa prediksi arah |
| `MBH_MANTRA` | Mantra Jawa kuno, tawa kering, batuk kakek, tongkat memukul genteng | Voice cue besar tidak ditutupi ambience |
| `SANITY_WHISPERS` | Bisik warga terbakar/penonton terikat, dupes Damar/Ibu | Tidak mendominasi subtitle dan arahan audio asli |
| `EVP_WELL` | Mantra terbalik autentik dan ketukan rahasia; memiliki metadata EVP sah | Jika target aktif, ducking overlay audio lain untuk validasi |
| `TORCH_TELEGRAPH` | Dengung segel autentik vs desis tajam jebakan, air mendesis | Membantu aksesibilitas, **tetap perlu UV** untuk kepastian |
| `HUNT_TELEGRAPH` | Detak jantung, laju angin, sapuan lidah api + getar daun | Terdengar sebelum Banaspati lunges |
| `FIFTH_GATE` | Kayu Pendopo patah, bambu terbakar, gamelan sumbang, riuh pasar bawah tanah | Sound bridge ke P5 |
| `Raka VO` | Monolog/forum, pembuka siaran, pembacaan Damar, akhir | Bahasa Indonesia naskah, subtitle speaker jelas |

**Metadata EVP tetap authored**, bukan FFT real-time mikrofon. `Authentic` tag untuk ketukan Sumur/Saksi; `Mimic` untuk tiruan Mbah Suro. Pengaturan dynamic mix memastikan suara peringatan dekat objek terdengar walau pemain mengubah volume ambience; menyediakan subtitle untuk detak/bambu/ketukan. VO manusia Indonesia yang belum direkam tetap `TODO`, tidak digantikan text-to-speech diam-diam sebagai “final”.

### 7.4 Implementasi chat dan televisi aman performa

Timeline chat berlandaskan event author (bukan koneksi internet): intro 780k + donasi 6.666.000; simpang 1 akun hantu mendorong kiri; simpang 2 akun impersonasi; simpang 3 fake SYSTEM prompt; catatan Damar memperjelas mengapa penonton dipakai sebagai bahan bakar; ritual empat sektor makin serempak; epilog 999.999 viewers + `Rp999.999.999 [ERROR]` dan kalimat “MASUK KE PASAR SETAN. KAMI MENUNGGUMU DI LAYAR TERAKHIR.” Semua riwayat chat tetap di history sehingga tidak hilang bila Raka sedang dikejar. Usernames dan angka tidak diambil dari data pemain sesungguhnya.

### 7.5 LOD, lighting, budget rendering

Unity URP, hindari **4 directional shadow casters full-res** saat 12 obor aktif. Gunakan emissive shader, satu/dedicated shadow key light yang diprioritaskan, deferred fake projection terpilih, light cookie atau decals UV, baked non-moving torches, fog particle pooling. Banaspati api adalah gabungan billboard mesh + particulate bounded, tidak membutuhkan raymarched volumetric full screen; jaga **overdraw** pada HP layar dan efek abu. Obor autentik dan umpan tetap terbaca dalam 720p Low. Profil CPU AI setelah 4 sektor aktif dipanggil sesuai interest zone; jauh dari Raka nonaktifkan intensive LOS raycasts tanpa mengubah progres. Kriteria frame-time diuji Windows Iris Xe, bukan hanya Linux server tanpa GPU.

---

## 8. Anti-softlock, balance config, fairness dan accessibility

### 8.0 Konfigurasi tuning (single source, baseline bukan kanon)

| Parameter | Default uji | Sumber/status |
|---|---:|---|
| `phone.normalDrainPctPerSec` | 0.05 | Global P3 |
| `phone.uvArDrainPctPerSec` | 0.10 total | Global P3; tidak ditumpuk |
| `phone.photoCost`, `phone.blitzCost`, `phone.blitzCooldown` | 1%, 15%, 6 s | Global P3 |
| `phone.largePowerbankRestore` | +80 poin | Global P3 |
| `sanity.startPolicy` | Clamp/save carry-over, P4 aktif dan dapat tutorial menuju 100 bila desainer setuju | **P4-D02**; tidak otomatis bila tidak disetujui |
| `sanity.forestDrain` | 0.10/s (1% per 10 s kanon; lantai 35% hanya baseline) | **KANON drain; BASELINE lantai** |
| `sanity.pendopoDrain`, `sanity.southDrain` | 0.04/s (sektor min 45% ambien), 0.12/s (fase Selatan dengan telegraph) | BASELINE MERGER |
| `sanity.lumbungShelterRestore`, `sanity.lumbungShelterCap` | +3 poin/s, maksimum 70% | BASELINE Claude; aktif hanya jika benar-benar berlindung |
| `sanity.incenseClarityDuration` | 20 s reduksi glitch/halusinasi, bukan imunitas serangan | BASELINE Claude |
| `chat.maxDuringChase` | 1 pesan per 1,2 s | BASELINE Claude |
| `banaspati.patrol`, `alert`, `chase`, `ritualChase` | 1.5, 2.7, 3.5, 3.7 m/s; sprint runtime Raka4.2 m/s | BASELINE Claude |
| `banaspati.firstHitSanityLoss`, `iFrame`, `secondHitWindow` | −25 poin, 3 s, 10 s | BASELINE; uji collider dan FPS |
| `sanity.pendopoDrain`, `sanity.southDrain` | 0.04/s, 0.12/s | Baseline |
| `sanity.warningThreshold`, `huntThreshold` | 70, 40, 0 | Kanon thresholds |
| `sanity.incenseRestore`, `incenseCooldown` | Ke100%, 12 s | Rancangan global; pembelian tetap opsional |
| `sanity.shrineRestore`, `shrineHold`, `shrineCooldown` | +15, 10 s, 30 s; celah Lumbung +3/s hingga 70 | Usul gratis anti-softlock |
| `banaspati.lungeTelegraph` | 1.0 s | Baseline; harus bisa reaksi FPS30 |
| `banaspati.sanityLowDetectionMultiplier`, `zeroDetectionMultiplier` | 1.5×, 2.0× | Baseline; jangan infallible LOS |
| `torch.uvConfirmDwell` | 2 s dari naskah | KANON indikasi identifikasi |
| `torch.trueCandidateCountPerSector` | 1 dari 3 | KANON |
| `torch.falseWaterSanityLoss` | 40 poin | KANON |
| `water.kendiCapacity`, `water.refillSeconds` | 3 muatan, 4 s | Baseline; isi ulang tidak habis |
| `torch.exposureSeconds` | 5 s | Baseline P4; tidak tercantum eksplisit naskah asli |
| `ritual.sectorOrder` | N/E/W bebas; S terakhir | Klarifikasi berdasar klimaks naskah |
| `ritual.localBlitzRequired`, `ritual.coreBlitzRequired` | 3 + 1 = 4 | KANON |
| `battery.dynamoRestore`, `battery.dynamoSeconds` | +25 poin, 8 s | Baseline P3; reset manual aman |
| `secret.wellKnockRecordSec` | 1.5 s EVP autentik | Baseline terinspirasi puzzle P3 |
| `trap.forestWarningSec` | ≥1 s saat tutorial; ≥0.8 s kemudian | Baseline fairness |
| `performance.720pLow` | avg ≥30, 1% low ≥24 FPS; aspirasi 60 global | **Target** yang belum divalidasi |

Semua nilai angka baru terpusat pada adapter `Phase4Balance`/ScriptableObject yang konsisten dengan config global. Jangan memasukkan langsung magic numbers ke coroutine banyak objek. Bila target performa memaksa penurunan efek, prioritaskan **keterbacaan obor UV dan gestur Banaspati** di atas jumlah partikel.

### 8.1 Bukti jalur gratis untuk `wallet_legitimate=0`

Pemain tanpa toko harus memperoleh dua modul P4 gratis, membawa HP dan consumable P3 aktual (bisa nol), mengambil tiga pusaka dari tiga sektor tanpa syarat uang, menerima **Kendi** dan mengisi air di Sumur, menggunakan **Batu perlindungan / shelter** untuk pulihkan Sanity, serta **dinamo manual fisik** untuk semua capture dan empat Blitz. Semua pilihan ini terjangkau meskipun Sanity pernah menyentuh 0, bahkan jika tiga salah siram dilakukan berturut-turut. Sektor N/E/W yang sudah disegel tidak pernah me-reset karena air habis; S dikerjakan setelah tiga sektor sah. Setelah core menang, Keping Cermin masih bisa diambil bila belum diambil, lalu player masuk portal via konfirmasi.

### 8.2 Pembuktian baterai skenario ekstrim

Empat Blitz wajib minimal **60 poin** (4×15) ditambah biaya optik sebelum menghitung miss. Ini bukan bukti pemain harus memiliki 60 poin di awal: recharge fisik berulang pada tempat perlindungan, tanpa saldo atau flare, menjaga jalur tetap mungkin. Uji CP5 dengan 0%, 1%, 14%, 15%, 29%, 44%, 60%, 100%; tiap kali pastikan dinamo terdekat reachable dari rute **tanpa UV** dengan visual landmark yang cukup dan tidak disergap saat animasi accepted. Kalau memiliki baterai 0 pada pintu altar, pemain boleh ke sumur/pendopo untuk mengisi daya tanpa menyalahkan Sanity atau UI chat. Tidak ada Blitz yang ditembakkan otomatis pada 0 dan tidak ada tombol UI yang mengisi baterai gratis.

### 8.3 Perilaku Sanity 0% yang harus diuji

Di Sanity 0%, pemain tidak kehilangan kendali, tidak mengalami permadeath, tidak kehilangan aksi `E/R/F/H` sebagai akibat glitch, tidak mendapat objektif palsu dari `System` UI yang wajib diikuti, dan tidak disergap Banaspati tanpa telegraph. Player bisa masuk shelter dengan layar mati, pulihkan Sanity dari shrine secara fisik, mencapai Sumur dan mengisi air, membaca simbol obor UV, dan menuju Pendopo. Hindari target “tahan kemenyan terakhir” karena stok bisa nol. Halusinasi suara boleh meningkatkan tekanan tetapi **tak boleh menyabotase mekanik wajib**.

### 8.4 Fair-horror dan aksesibilitas

**Wajib**: UV menampilkan bentuk selain warna: lingkar penuh asli, silang merah palsu, tengkorak bahaya. EVP menampilkan warna, label ASLI/TIRUAN, dan bentuk gelombang. Chat palsu punya anomali tipografi *sebagai hint* tetapi jawaban valid tetap fakta dunia. Subtitle VO termasuk `[Batang bambu patah]`, `[Api melesat di atas]`, `[Ketukan dari sumur]` dan `[Suara menyerupai Damar]`. Settings menawarkan pengurangan camera shake, motion blur, strobe, chromatic aberration dan visual distorsi Sanity tanpa mengubah tingkat kesulitan melewati obor. Flash aksesibilitas bertahap; jangan mengharuskan pasien *photosensitive* menatap efek strobe untuk membaca petunjuk.

### 8.5 Backtrack, blocker dan ending

Jalur balik tiap sektor dan menuju portal harus fisik; tidak cukup *teleport* invisible untuk memperbaiki navmesh salah. Sesudah CP7, Banaspati dan ilusi berbahaya nonaktif, tapi relung Cermin masih terbuka. Sebelum masuk portal, panel menampilkan `Arsip Babak 4 0/1 atau 1/1` dan Arsip Global `N/5` serta pilihan **Kembali Menjelajah** dan **Turun ke Pasar Setan**. Hanya opsi kedua menulis CP8 dan memulai P5, tidak ada motor dan tidak ada Kos tambahan. Jika arsip terlewat, player bisa kembali melalui jalur aman; pascamusuh tidak ada timer penutupan portal.

---

## 9. Produksi Unity — komponen modular dan batas kepemilikan

| Tanggung jawab | Peran komponen kandidat, bukan klaim nama repo | Reuse / tambahan minimal |
|---|---|---|
| Flow P4 | `Phase4FlowController` | Gunakan event/receipt pattern dari Phase3, bukan game director global lain |
| Sanity | `Phase4SanityAdapter`, `SanityDistortionProfile` | Perluas data Sanity existing (sebelumnya dormant); game time dan efek URP low-cost |
| Chat infiltrasi | Provider `LiveStreamTimeline` P4 + `FakeChatCue` | Gunakan history, UI, donasi terikat existing; tidak membuat chat kedua |
| UV Rajah v2 | `UvRajahSignature` pada lens existing | Tanda world AUTHENTIC/DECOY/DANGER, bukan render kamera kedua |
| Thermo + AR | Target authored `ThermoEvidence`/`ArEvidence` | Reuse smartphone scanner dan evidence capture P2/P3 |
| EVP mantra | Source metadata on well `EvpAudioEmitter` | Adapter EVP v2; tidak FFT microphone OS |
| AI Banaspati | `BanaspatiBrain` + waypoint/stealth sensors | Threat interface existing, orb and exposed charred core same entity |
| Arwah Mbah Suro | `MbahSuroRitualDirector` | Timeline chat/illusion, tidak HP bar kedua |
| Obor/ritual | `FourDirectionsRitualController`, `TorchesAuthoring` | 12 authored candidates, 3 local+1 core, sector commit |
| Kendi dan air | `RuwatanWaterSource`, one `Kendi` inventory instance | Simpan capacity sekali; sumur refill; koneksi ke ritual offering receipt |
| Save/persist | `Phase4Data` adapter ke existing `CampaignState` | Receipt & CP1–CP8, migrasi dari schema yang dibuktikan |
| Portal P5 | `Phase4ExitPortalInteractable` | Scene router Continue existing; CP8 dahulu lalu scene P5 |
| QA dev debug | `Phase4DevOverlay` development builds saja | Sanity, battery, 12 torches, sector receipts, water, AI, chat source, checkpoint |

**Arsitektur target:** `Assets/_LayarTerakhir/src/` adalah petunjuk dari Phase3 v2.3. Verifikasi nama class, prefab, render pipeline dan path scene yang benar sebelum coding; apabila P3 telah diimplementasikan dengan class berbeda, prefer reuse existing dan catat alias. Tidak perlu refactor seluruh P0–P3. Konfigurasi Balance dan data naskah/chat/dokumen dapat menjadi ScriptableObject; jangan membuat file JSON save P4 mandiri.

**Tahapan kerja produksi:** (M0) audit repository dan kontrak P4, (M1) scene Kos state + forest vertical slice, (M2) modul Sanity & fake chat, (M3) tiga sektor investigasi + rahasia, (M4) Banaspati & Mbah Suro AI, (M5) ritual distributed boss 3+1, (M6) portal/P5 state, (M7) aset+audio+lighting+VO, (M8) regression dan Windows acceptance. Jangan memberi klaim PASS hanya berdasarkan validator headless/structural.

---
## 10. QA acceptance — 180 skenario (SEMUA NOT_RUN)

**Status:** QA-001–QA-180 adalah kontrak acceptance, belum ada hasil tes Unity; jangan menandai PASS tanpa bukti build dan log. Kategori A–O memuat 10 skenario per kategori; kategori P menambah 30 skenario integrasi khusus. Nomor stabil untuk handover Codex dan laporan `Audit/phase4-qa-matrix.md`.

### 10.1 A — Kontinuitas P1–P3 dan input

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-001 | Continue dari P3_COMPLETE dengan tiga segel valid | Babak 4 berawal di Golden Kos, status P0–P3 utuh |
| QA-002 | Coba mulai P4 tanpa P3_COMPLETE dalam build produksi | Ditolak; dev-only skip tidak menulis segel/inventori palsu |
| QA-003 | Muat P3 CP7 berisi Powerbank/Flare/Kemenyan yang belum digunakan | Semua carry-over ditampilkan sebagai instance sah, tidak hilang |
| QA-004 | Ganti V OTS↔FP sambil R UV v2 aktif | FOV/culling/scan konsisten dan kamera pulih |
| QA-005 | Tahan dan lepas RMB dengan AR/UV/Thermo bergantian | HP dua tangan bekerja, pose dan view kembali seperti sebelum hold |
| QA-006 | Remap H/R/C/F/G/E melalui GameInputConfig | Prompt HUD memakai binding aktual; tidak konflik |
| QA-007 | Tekan F saat membuka Jurnal dekat Banaspati | Layar mati darurat dan action lain tidak terpicu |
| QA-008 | Tekan E pada obor sambil HP UV dinaikkan | Interaksi sah dilakukan tanpa menurunkan HP paksa |
| QA-009 | Klik kiri pada AR aura altar tanpa mode boss | Hanya foto 1% jika valid, tidak memicu Blitz 15% |
| QA-010 | Alt-tab saat animasi angkat HP dan kembali ke game | Input state tidak macet; timer gameplay mengikuti policy fokus |

### 10.2 B — Safe Hub, toko dan ekonomi

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-011 | Masuk Kos P4 dari snapshot wallet_legitimate=0 | Upgrade UV v2/Sanity gratis, berangkat tetap bisa |
| QA-012 | Baca dan lewati forum tragedi 1997 pada run berbeda | Dokumen opsional; upgrade/gerbang tetap dapat dicapai kedua run |
| QA-013 | Beli paket Kemenyan/PB/Air dari saldo cukup | Debit sekali, item instance unik dan receipt persisted |
| QA-014 | Klik beli paket sama dua kali cepat | Tidak menggandakan item, saldo tak negatif |
| QA-015 | Force quit saat pembelian di tengah commit | Seluruh transaksi atau rollback; tak ada item tanpa debit |
| QA-016 | Gunakan colokan Kos sekali sebelum berangkat | Baterai menjadi 100 sesuai policy; receipt persisted |
| QA-017 | Reload Kos atau Continue dari CP1 setelah colokan | Tidak ada pengisian gratis kedua |
| QA-018 | Lihat 780.000 viewer, donasi Rp6.666.000 | Donasi tercatat sekali ke wallet_bound, bukan wallet_legitimate |
| QA-019 | Tampilkan akhir Rp999.999.999 [ERROR] | Saldo sah, terikat, dan expense tidak berubah oleh efek |
| QA-020 | Selesaikan P4 tanpa membeli ketiga paket toko | Semua evidence, 4 torches, 4 Blitz, portal berhasil |

### 10.3 C — Hutan bambu, chat palsu dan tanda

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-021 | Ikuti chat palsu ke arah batu nisan Simpang 1 | UV tengkorak ungu dan telegraph terlihat sebelum hazard |
| QA-022 | Lewati simpang 1 lewat beringin kanan tanpa membaca chat | Petunjuk lingkungan tetap memadai dan rute berlanjut |
| QA-023 | Amati jejak kaki Damar di jalur kanan menggunakan UV | Tanda aman terbedakan dari tengkorak bahaya |
| QA-024 | Periksa username @warga_karang97 tapi arah UV bertentangan | Sumber kebenaran world UV, bukan username |
| QA-025 | Simpang 2 muncul dua username hampir sama | Pemain dapat membedakan melalui clue dunia, bukan hafalan nama |
| QA-026 | Simpang 2 pemain memilih jalan salah | Peringatan/loop back ada; tidak terjebak permanen |
| QA-027 | Simpang 3 chat meniru notifikasi MATABATIN | UI host input/pause tidak dibajak oleh pesan authored |
| QA-028 | Matikan seluruh pesan chat dari HUD saat melalui tiga simpang | Jalan tetap dapat diselesaikan dengan landmark dan UV |
| QA-029 | Baterai 0% sebelum simpang pertama | Dinamo manual aman dapat dicapai tanpa UV atau item berbayar |
| QA-030 | Reload setelah dua simpang lalu kembali ke hutan | Event chat tidak menggandakan donasi atau memutar penanda salah |

### 10.4 D — Sanity tiers, recovery dan aksesibilitas

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-031 | Mulai dari Sanity 100 ke 70 di hutan terpapar | Drain 1 poin/10s game-time, cue tepi dan bisik di threshold |
| QA-032 | Turunkan Sanity dari 71 ke 69 lalu naikkan lagi | VFX transition idempoten, tidak menumpuk shader |
| QA-033 | Sanity turun melewati 40 saat Banaspati patroli | Deteksi risiko meningkat sesuai config, tidak teleport ke pemain |
| QA-034 | Biarkan Sanity tepat 0% | HUNT aktif tanpa Game Over otomatis |
| QA-035 | Pada Sanity 0%, masuk shelter dan gunakan shrine gratis | Interaksi selesai, Sanity pulih >0 tanpa uang |
| QA-036 | Sanity 0%, inventory kemenyan 0 dan baterai 0 | Masih ada jalur fisik ke shelter, charger dan objective |
| QA-037 | Gunakan H dengan kemenyan 0 unit | Tidak mengurangi slot negatif atau memicu animasi penggunaan palsu |
| QA-038 | Gunakan satu kemenyan dekat low sanity | Pulih ke100 mengikuti kontrak global, satu unit berkurang, receipt/item state konsisten |
| QA-039 | Pause/Alt-Tab 2 menit dalam hutan berkabut | Tidak terus menguras Sanity di background saat gameplay paused |
| QA-040 | Aktifkan opsi kurangi strobo/blur/camera shake | Cue hazard, obor dan informasi objektif tetap terbaca |

### 10.5 E — Tiga bukti dan pusaka

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-041 | Thermo pada makam lain yang bukan target +66°C | Tidak menambah Bukti #1 |
| QA-042 | Rekam Thermo +66°C target Utara dua kali | Hanya satu ID evidence |
| QA-043 | Ambil Kendi tanpa memindai makam | Pusaka #1 tercatat, Bukti #1 tetap belum ada |
| QA-044 | Foto altar Timur dengan mode UV bukan AR | Bukti #2 tidak sah; feedback mode salah |
| QA-045 | Foto altar Timur dengan AR, simpan lalu reload | ID Bukti #2 persisted sekali, efek aura terbaca |
| QA-046 | Ambil Belati dan baca Damar Note terpisah | Relic dan dokumen terpisah dan idempoten |
| QA-047 | Rekam audio ambience biasa di sekitar sumur | Bukti EVP tidak sah |
| QA-048 | Rekam segmen mantra empat penjuru dengan EVP yang sah | Bukti #3 tersimpan sekali, Jurnal 3/3 bila lainnya ada |
| QA-049 | Ambil Kantong Tanah tanpa merekam mantra | Pusaka tercatat sendiri, tidak memalsukan bukti |
| QA-050 | Kunjungi sektor W→E→N sebelum Ritual | Semua bukti/pusaka bisa dikumpulkan dalam urutan berbeda |

### 10.6 F — Jurnal, rahasia Cermin Saksi

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-051 | Konfirmasi Jurnal saat hanya 2 bukti | Tombol tertutup; ritual tidak terbuka |
| QA-052 | Kumpulkan 3 bukti namun tutup Jurnal tanpa konfirmasi | Tidak ada P4_JOURNAL_CONFIRMED otomatis |
| QA-053 | Konfirmasi 3 bukti lalu reload | Receipt tersimpan; dideduksi tetap satu kali |
| QA-054 | Coba buka relung Cermin tanpa mendengar ketukan | Tidak mengeluarkan item, clue visual sewajarnya |
| QA-055 | Verifikasi ketukan relung dengan EVP asli | Puzzle state verifikasi tervalidasi, bukan ambience palsu |
| QA-056 | Sorot relung dengan UV namun roda salah arah | Relung tetap tertutup, tidak menipu state |
| QA-057 | Selesaikan EVP, UV mata tertutup, roda benar, ambil cermin | Commit ID unik sebelum popup sukses |
| QA-058 | Force quit sewaktu commit pengambilan cermin | Jika gagal, mirror tersedia lagi; tidak muncul notifikasi sukses palsu |
| QA-059 | Mati/reload sesudah cermin sukses terkoleksi | Mirror hilang dari world, ID tetap di secret_items |
| QA-060 | Uji 16 kombinasi kepemilikan P1/P2/P3/P4 | Arsip N/5 aktual, status lokal 0/1 atau 1/1 |

### 10.7 G — Kendi, sumur, air dan belanja opsional

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-061 | Kendi diambil di makam Utara dalam keadaan kosong | Bisa dibawa ke Sumur Barat untuk diisi tanpa pembayaran |
| QA-062 | Isi Kendi di Sumur Keramat hingga kapasitas | Set unit tepat sesuai config, tidak overfill |
| QA-063 | Siram obor palsu dengan air terakhir | −40 Sanity, sumur dan pengisian bantu tetap tersedia |
| QA-064 | Coba siram dengan Kendi air=0 | Aksi ditolak dengan feedback, no negative amount |
| QA-065 | Taruh Kendi dalam offering lalu mulai ritual | Kendi yang sama dapat dipegang kembali untuk menyiram tanpa duplikat |
| QA-066 | Setelah Ritual, buka inventory dan altar secara bergantian | Tidak muncul dua Kendi aktif sebagai item berbeda |
| QA-067 | Gunakan paket air toko ketika memiliki item | Mengisi Kendi cepat tanpa mengganti sumber air sumur |
| QA-068 | Selesaikan 4 obor dengan 0 paket air toko | Air gratis cukup, refill dapat diulang |
| QA-069 | Force quit sesudah refill sementara sebelum commit | Restore air dan obor dalam snapshot konsisten |
| QA-070 | Banaspati mengejar saat pemain menuju sumur kosong | Ada jalur/cover ke tempat refill tanpa kematian tak terhindarkan |

### 10.8 H — Identifikasi 12 obor UV

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-071 | Render tiga obor Utara dari observasi aman | Tepat 1 bulat inti ungu dan 2 silang merah |
| QA-072 | Render tiga obor Timur dan Barat | Tepat 1 autentik dan 2 palsu pada tiap sektor |
| QA-073 | Render tiga obor Selatan saat final | Tepat 1 autentik dan 2 palsu; bukan selalu tengah |
| QA-074 | Ganti seed layout lalu ulang run baru | Lokasi obor autentik boleh berubah hanya antar seed run |
| QA-075 | Save/reload setelah pemain melihat obor asli | Layout/seed stabil pada save tersebut |
| QA-076 | Sinar UV aktif saat player Sanity=0 | Bentuk segel world tetap terbaca tanpa tertutup UI palsu |
| QA-077 | Matikan UV lalu coba tebak berdasarkan chat | Desain masih memberi gameplay, tidak memberi info 100% lewat chat |
| QA-078 | Periksa penanda tengkorak ungu pada tanah dekat obor | Tetap berarti bahaya, tak tertukar dengan lingkar UV asli |
| QA-079 | Aktifkan mode buta warna/kontras rendah 720p | Silhouette bulat/silang/tengkorak terbedakan |
| QA-080 | Player menuang air ke dua decoy berturut-turut | Obor asli tetap mungkin diidentifikasi; decoy tidak berubah tipe |

### 10.9 I — Banaspati AI, Mbah Suro dan hiding

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-081 | Pertama tiba Pendopo dan Banaspati reveal | Monster muncul jauh, telegraph, tanpa instakill spawn |
| QA-082 | Berjalan, sprint, jongkok pada jarak sama dari AI | Sensor suara berbeda sesuai baseline |
| QA-083 | Sanity 0 vs 100 saat Banaspati patroli | Radius investigasi meningkat tanpa LOS magis |
| QA-084 | Masuk liang kubur dengan layar HP mati sebelum AI melihat | AI melewati/menyelidik lalu pergi |
| QA-085 | Sembunyi di kolong gubuk saat Banaspati sudah mengejar | Player harus memutus LOS; hiding bukan invulnerability otomatis |
| QA-086 | Di dalam gentong aktifkan layar F saat Banaspati dekat | Stimulus cahaya sah memicu investigasi ancaman |
| QA-087 | Banaspati serang dari udara | Serangan punya suara/bayangan/telegraph ≥baseline |
| QA-088 | Wewe dari P3 selesai; masuk P4 | Tidak muncul AI P3 atau boss di scene P4 |
| QA-089 | Mbah Suro menulis chat “SYSTEM” palsu | Pemain tetap bisa melihat true world UV dan action binding asli |
| QA-090 | Banaspati dekat safe-anchor restore dan reload | AI spawn menjauh, ada grace untuk kontrol kembali |

### 10.10 J — Ritual precondition, sector N/E/W

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-091 | Masuk Pendopo ritual sebelum bukti/pusaka lengkap | Batu ritual dapat diperiksa tetapi boss tidak dimulai |
| QA-092 | Tiga pusaka terpasang namun Jurnal belum dikonfirmasi | Aktivasi ditolak dengan daftar prasyarat yang kurang |
| QA-093 | Konfirmasi ritual setelah semua syarat benar | CP5 commit sukses sebelum threat memulai fase aktif |
| QA-094 | Fail write CP5 sebelum cutscene ritual | Ritual tidak dimulai, player bisa retry save |
| QA-095 | Urutan pemadaman pertama N lalu E lalu W | Ketiga sektor dapat diterima; total tepat 3 local Blitz |
| QA-096 | Urutan pemadaman pertama W lalu N lalu E | Ketiga sektor dapat diterima dengan progres set sama |
| QA-097 | Siram obor autentik Utara lalu miss Blitz | Obor recoverable, boleh isi air ulang dan retry |
| QA-098 | Spam Klik Kiri berkali-kali dalam satu exposure | Maksimal satu sector hit dan biaya Blitz valid sesuai input policy |
| QA-099 | Menang sektor Timur saat sektor Utara telah committed | Sektor Utara tetap permanen off; tidak memberi seal dua kali |
| QA-100 | Coba memicu local Blitz di sektor sudah selesai | Tidak boleh menggandakan poin, baterai, atau receipt |

### 10.11 K — Klimaks Selatan dan Pendopo

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-101 | Masuk Selatan sebelum ketiga sektor N/E/W sukses | Sektor final belum mengizinkan ritual/menampilkan gate wajar |
| QA-102 | Dengan 3 sektor sukses, balik Selatan tanpa kemenyan | Shelter netral dan UV memungkinkan identifikasi asli |
| QA-103 | Dengan Sanity 0 di Selatan dan tanpa water pack | Shine UV + shelter + refill sumur tetap bisa menyelesaikan |
| QA-104 | Siram obor palsu Selatan saat kejaran | −40 sanity ter-clamp ke 0, telegraph, tidak instakill |
| QA-105 | Siram obor asli Selatan setelah tiga local hit | Hanya SOUTH_OFF, tidak memicu Blitz lokal keempat |
| QA-106 | Periksa jumlah seal hits sesudah SOUTH_OFF | Tiga local hits, core belum completed; bukan empat |
| QA-107 | Mbah Suro + Banaspati bergabung di Pendopo | Satu exposed core, bukan dua enemy health bar |
| QA-108 | Blitz miss pada core saat baterai rendah | Core exposure bisa diulang setelah charger fisik; sektor tak reset |
| QA-109 | Blitz final valid pada core | Exactly fourth hit, segel lokasi keempat dikomit CP7 atomik |
| QA-110 | Player mencoba Blitz kelima setelah menang | Tidak menambah segel/biaya Blitz/target boss; foto biasa tetap1% sesudah boss RESOLVED |

### 10.12 L — Progres sektor dan checkpoint

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-111 | Setelah sektor pertama N sukses, force quit | Continue CP6 dengan N selesai dan dua sektor lain aktif |
| QA-112 | Setelah sektor pertama W sukses, reload | W selesai, N dan E tetap bisa urutan bebas |
| QA-113 | Setelah N+W selesai, kalah oleh Banaspati | Kedua sector receipts tetap utuh; tidak kembali CP5 kosong |
| QA-114 | Setelah tiga sektor selesai, ambil relung rahasia | Pickup menulis ke checkpoint tertinggi tanpa menurunkan CP6 |
| QA-115 | Force quit tepat setelah siram obor tapi sebelum Blitz | Tidak memuat obor padam dan air bertentangan |
| QA-116 | Force quit saat TryCommit lokal sukses | Disk merepresentasikan satu keadaan penuh atau rollback |
| QA-117 | CP4 pickup dilakukan saat Banaspati chase di dekat makam | Restore anchor aman, AI tidak spawn tepat di atas Raka |
| QA-118 | Load CP5 dengan layout seed permanen | Susunan asli/palsu tidak berubah secara tidak sah |
| QA-119 | Gagal CP7 write saat final Blitz | Tidak membuka portal/segel tanpa commit, ada retry yang jujur |
| QA-120 | CP8 commit sukses lalu load Continue | Routing ke boundary fisik Pasar Setan setelah CP8 tanpa Kos/epilog ulang; full scene P5 dipasang ketika babak5 diimplementasikan |

### 10.13 M — Konsistensi save, schema, data dan ekonomi

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-121 | Migrate save valid P3 ke P4 loader terbaru | P0–P3, flags, Arsip, ekonomi, inventori dipertahankan |
| QA-122 | Uji snapshot P3 tanpa bukti rahasia terdahulu | Count Arsip P4 tidak diasumsikan 4/5 |
| QA-123 | Simulasikan disk penuh saat transaksi Kemenyan | Rollback saldo sah dan instance, tidak negatif |
| QA-124 | Simulasikan disk penuh saat pickup bukti makam | Tidak menghilangkan prop, ID belum dianggap committed |
| QA-125 | Simulasikan disk penuh saat secret Cermin | Popup tidak muncul dan prop tersedia untuk retry |
| QA-126 | Siram obor benar, simpan di sela panel Jurnal | State transient dan data persisten tidak mengandung dua sumber benar |
| QA-127 | Double click konfirmasi turun portal | Satu P4_COMPLETE, satu CP8, boundary fisik pada scene P4 tanpa scene P5 fiktif |
| QA-128 | Load setelah 0% Sanity dan 0% battery | Value tepat snapshot; charger/shrine tetap dapat dicapai |
| QA-129 | Periksa wallet_bound Rp6.666.000 setelah reload | Donasi satu kali, tidak masuk wallet_legitimate |
| QA-130 | Bandingkan save CP7 dan CP8 | CP7 boss won; CP8 exit committed; tidak ada status setengah jadi |

### 10.14 N — Visual, audio, localization, performa

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-131 | Profil 720p Low pada hutan bambu saat kabut dan Sanity LOW | Target ≥30 FPS avg, 1% low ≥24 FPS, keterbacaan terjaga |
| QA-132 | Profil 720p Low saat 12 obor dan Banaspati api aktif | Shadow/overdraw tidak melampaui budget target |
| QA-133 | Profil FPS saat UV v2 dan Live Chat bersamaan | Tidak membuat camera kedua/draw calls berlipat tak wajar |
| QA-134 | Uji subtitle ketukan sumur, bambu, Banaspati dari jarak aman | Cues lokal dan pembicara dapat dibaca tanpa audio |
| QA-135 | Uji EVP saat ambience bambu dan suara tiruan bertumpuk | Sumber authored authentik tetap bisa dipilih dengan arah kamera |
| QA-136 | Lihat simbol asli/palsu/tengkorak dengan simulasi buta warna | Dibedakan oleh bentuk dan kontras, bukan hanya hue |
| QA-137 | Matikan chromatic aberration/distorsi berat dari Settings | Sanity tetap berpengaruh tetapi clues tidak lenyap |
| QA-138 | Audit semua asset imported Desa dan Banaspati | URL, hak komersial, SHA-256, penempatan di manifest |
| QA-139 | Bandingkan visual Kos P4 dengan Golden Scene P1/P3 | Scene bukan duplikat, furniture transform/lighting identitas tetap |
| QA-140 | Windows build dengan laptop Intel Iris Xe RAM8GB, bermain ±78 menit | Kualitas visual/FPS/VO diverifikasi nyata; headless tidak dipakai mengklaim PASS |

### 10.15 O — End-to-end, cerita dan phase gate

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-141 | Selesaikan Run A tanpa toko, tanpa baca live chat, tanpa rahasia | P4_COMPLETE dan CP8 tercapai tanpa paywall |
| QA-142 | Selesaikan Run B dengan semua paket dan Cermin Saksi | Progress akhir sama secara main path, Arsip bertambah tepat 1 |
| QA-143 | Ikuti jalur bebas W→E→N saat investigasi & ritual | Tidak ada objektif mundur; Selatan tetap terakhir |
| QA-144 | Lakukan ritual dengan Sanity 0 selama beberapa sektor | Dengan recovery fisik, kemenangan mungkin; 0 bukan game over otomatis |
| QA-145 | Selesaikan empat obor dan hit counter | Tiga local + satu core = empat; tidak ada tembakan kelima |
| QA-146 | Periksa tahun lore 1997, Mbah Suro wafat sesudah 2004 | Teks forum/dokumen/cermin tidak berkontradiksi tak sengaja |
| QA-147 | Periksa pembuka 780k, penutup 999.999, donasi dan kontrak | Semua angka kanon muncul sesuai konteks dan ledger tidak terkontaminasi |
| QA-148 | Setelah final boss, kembali ke Sumur untuk Cermin jika terlewat | Jalur aman, puzzle berfungsi, tidak restart boss |
| QA-149 | Pada panel portal pilih Kembali Menjelajah lalu turun nanti | Tidak ada P4_COMPLETE saat cancel; commit hanya saat konfirmasi |
| QA-150 | P4_COMPLETE ke P5 malam sama dengan baterai 0/sanity 0 | Tidak muncul Kos palsu; boundary entry Pasar Setan mempunyai charger/perlindungan fisik tanpa softlock; seluruh gameplay P5 di luar lingkupP4 |

### 10.15A P — Tambahan audit Claude dan konflik hasil merger (QA-151–QA-180)

Kasus di bawah sengaja memeriksa detail Claude yang **belum dijadikan orakel** dalam QA-001–QA-150, ditambah resolusi konflik setelah merger. Satu hasil QA hanya boleh `PASS` apabila benar-benar dijalankan dengan bukti; `NOT_RUN` adalah default. Nomor tidak mengganti atau merombak acceptance lama.

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-151 | Coba `G` saat membawa 4 flare lama dari P3 | Flare tidak dilempar, stok tidak dikurangi, pesan “Api asing ditolak desa”; masih tersedia untuk P5 |
| QA-152 | Verifikasi layout standar obor asli N/E/W/S | Default authored N=Tengah, E=Kiri, W=Kanan, S=Kiri; tanpa duplikat tanda autentik |
| QA-153 | Muat CP5 berulang dan periksa preset/seed obor | Posisi asli tidak berpindah karena retry, kamera berubah, menu, atau force-quit |
| QA-154 | Bermain dengan preset opsional dan simpan seed | Seed dipilih sekali pra-ritual; pemilihan tidak berubah saat CP6 rolling |
| QA-155 | Uji tiga cabang hutan Claude dalam urutan J1–J3 | J1 kanan, J2 lurus, J3 kanan; jalan salah tetap punya UV + audio telegraph dan route recovery |
| QA-156 | Baterai 0 sebelum simpang J1 pada save valid | Charger darurat sebelum J1 reachable tanpa UV; tak wajib beli powerbank atau reset Kos |
| QA-157 | Sanity 0, tanpa kemenyan, di hutan sebelum Pendopo | Perlindungan fisik gratis/waypoint aman dapat dicapai walau entitas agresif |
| QA-158 | Drain Sanity pasif 20 menit tanpa kontak ancaman | Tidak turun di bawah 35% di hutan / 45% area desa karena waktu saja; kontak/event tetap bisa ke 0 |
| QA-159 | Gunakan Kemenyan sekali dengan Sanity 30 | Pulih ke100% setelah penggunaan selesai, cooldown12detik; satu instance dan transaksi sesuai kontrak global |
| QA-160 | Sanity 0 saat masuk shelter lumbung tanpa Kemenyan | Bisa naik bertahap sampai maksimal 70 menurut source physical; tidak kebal damage di luar shelter |
| QA-161 | Banaspati mengenai player dua kali dalam window 10 s | Damage pertama, knockback, i-frame 3 s; hit kedua valid sesuai konfigurasi, tidak terjadi double-hit di satu frame |
| QA-162 | Banaspati mengejar player dengan LOS terputus | SEARCH 6 s lalu lepas, tidak mengetahui lokasi player permanen |
| QA-163 | Client 30 FPS dan 60 FPS: telegraph semburan Mbah Suro | Durasi ≥1,2 s, petunjuk tetap terbaca dan dodge memungkinkan pada kedua FPS |
| QA-164 | Chat asli mengirim klaim yang salah dan hantu mengirim klaim benar | Tidak ada sistem auto-label benar dari username; UV world tetap satu-satunya oracle obor |
| QA-165 | Chat 40 pesan dan high Sanity; ukur clipping/rate limit | Maksimum 1 pesan/1,2 s saat chase; reticle dan prompt `E` tidak tertutup |
| QA-166 | Kemenyan dipakai saat chat palsu aktif | Distorsi chat mereda 20 s baseline; membaca clue dunia masih mungkin tanpa kemenyan |
| QA-167 | Kendi teraktivasi setelah offering tiga pusaka | Tepat satu ID inventori aktif, kapasitas3 dan isi aktual0–3; offering tidak menambah air, sumur mengisi secara fisik |
| QA-168 | Salah siram obor palsu saat stok 1 | Stok jadi 0 satu kali, Sanity −40, transaksi atomic; reload tidak memberi air kembali secara ajaib |
| QA-169 | Save gagal tepat ketika pemakaian Garam | Item dan volume air rollback bersama; tidak ada duplikasi garam atau air |
| QA-170 | Hit sektor N sukses kemudian mati di sektor E | N tetap committed, E masih aktif; CP6 aman; tidak memulai ulang N |
| QA-171 | Hit sektor E lalu W (N belum) dan Continue | Receipts E/W retained, N tersisa; Selatan tetap terkunci |
| QA-172 | Force-quit sesudah siram asli namun sebelum Blitz | Air/torch state dipulihkan dari commit terakhir; tidak ada obor mati gratis atau biaya ganda |
| QA-173 | Force-quit setelah obor S off dan sebelum core Blitz | `south_torch_off` tetap ada; restore menuju core; tidak ada local S Blitz |
| QA-174 | Save gagal saat sektor W valid | Tidak tampil keberhasilan persisten palsu; rollback receipt/baterai sesuai owner global |
| QA-175 | Sukses core lalu save CP7 gagal | Portal/kemenangan tidak dicatat sebagai sukses sampai commit; recover sektor yang sah dan ulang core |
| QA-176 | Sukses CP7, ambil Cermin rahasia, keluar P5 | Secret ID tetap ada dan CP8 tidak menurunkan anchor/merge boss |
| QA-177 | Dari P4 CP8 masuk P5 dengan Sanity 0% & baterai 0% | Masuk boundary fisik Pasar Setan tanpa Kos, reset nilai, atau scene load yang belum ada; charger/shrine entry tersedia |
| QA-178 | Uji tinggi/luas 4 sektor pada Intel Iris Xe target | Jalur LOS dan UV bisa dibaca di 720p Low 30 FPS target; penurunan partikel tidak menghapus telegraph |
| QA-179 | Revisi script chat pada Sanity rendah menampilkan `[SYSTEM]` palsu | Tidak ada pemanggilan action dispatcher, perubahan input binding, atau checkpoint dari teks chat |
| QA-180 | Jalankan QA lintas Phase 1–3 pascamigrasi dan CP6 rolling P4 | Save P0–P3, inventori/arsip, uang dan checkpoint lama tidak diregresi |

### 10.16 Definition of Done (gerbang penerimaan Phase4)

1. Kelima adegan beserta seluruh ucapan dan angka kanon dapat dialami tanpa membuat sistem P0–P3 duplikat.
2. Sanity aktif, tier 70/40/0 teruji; Sanity 0 tidak Game Over dan jalur gratis pulih nyata.
3. Hutan 3 simpang dapat diselesaikan tanpa chat; UV world tidak pernah berbohong walau UI terinfiltrasi.
4. Tiga bukti dan tiga pusaka benar-benar terpisah; jurnal dikonfirmasi eksplisit; rahasia Cermin tersimpan sekali.
5. Semua 12 obor authored: 1 asli + 2 palsu tiap sektor; bentuk UV permanen; salah siram tidak softlock.
6. Ritual tersebar: 3 local Blitz + 1 Pendopo core Blitz, **total empat**, tanpa tembakan kelima.
7. Water/charger/shelter gratis membuat P4 dapat dituntaskan tanpa belanja, termasuk battery 0/sanity 0/banyak miss.
8. Sector commits survive death/reload; CP7 menyimpan segel keempat atomik; CP8 hanya saat konfirmasi portal.
9. Transisi Pasar Setan terjadi malam yang sama, tanpa Kos; Arsip global N/5 aktual, semua inventori terjaga.
10. 180 QA dilaporkan dengan status dan bukti, diuji editor/native serta Windows visual/audio/performa; **tidak ada P0/P1 blocker** sebelum phase-freeze.

---

## 11. Kontinuitas cerita, risiko, dan keputusan terbuka

### 11.1 Audit kontinuitas Babak 1–4

| Topik | Ketentuan / apa yang harus diuji |
|---|---|
| Gerbang Panca-Sengkala | Rusunawa = segel 1, RSUD = segel 2, Pabrik = segel 3, **Karang Setra = segel 4**; Pasar Setan adalah **gerbang kelima**, bukan segel kelima yang sudah diperoleh |
| Rumah Atmaja | Titik awal narasi dan petunjuk, **bukan** salah satu gerbang segel tersendiri; peta Kos diperbarui sesuai gerbang 1–4 |
| Damar | Video P3, kartu pers dan sepatu di pabrik; P4 kacamata retak dan catatan terakhir di Gubuk Timur; suara Damar di hutan bisa tiruan |
| Wewe & Lastri | Sudah tersegel di babak sebelumnya; tidak muncul sebagai encounter aktif baru di Karang Setra tanpa keputusan desain |
| Mbah Suro 1997/2004 | Ritual desa 1997; menurut Cermin Saksi ia meninggalkan desa dan wafat sesudah peristiwa Rusunawa 2004; lawan P4 berupa arwah |
| Aplikasi MataBatin.apk | Terungkap berasal dari pengikut Mbah Suro; UI/chat terganggu, **fungsi fisik UV yang diuji dunia** tetap memegang ground truth |
| Viewer & dana | Angka viewer turun/naik antar live karena penonton serentak, bukan jumlah follower total. Donasi Penonton_Terakhir terikat. UI uang akhir ERROR bukan transaksi. |
| Arsip Saksi | P4 item keempat **jika** tiga rahasia terdahulu telah dikumpulkan; global N/5 aktual, bukan babak=jumlah. |
| Sanity | Mulai aktif P4; nilai P3 dipertahankan sesuaiREPO-03. Kemenyan P3 tetap ada jika belum dikonsumsi. |
| Flow waktu | P3 diakhiri malam sebelumnya; **Kos P4 sore berikutnya 16:45**, berangkat 23:45, tiba 23:53; P5 malam yang **sama** dengan P4. |

### 11.2 Register keputusan dan baseline (`P4-Dxx`)

| ID | Perkara | Baseline kerja | Risiko bila berbeda |
|---|---|---|---|
| **P4-D01** | Apakah desain P4 mengizinkan sektor N/E/W bebas urutan? | Ya untuk investigasi dan ritual; S klimaks terakhir | Mengubah urutan cerita/QA dan path backtracking |
| **P4-D02** | Sanity awal P4 dari nilai save P3 atau 100 yang disebut UI? | Pertahankan nilai save sah, gunakan tutorial pemulihan bila diperlukan, **jangan reset diam-diam** | Mengubah ekonomi kemenyan dan awal kesulitan |
| **P4-D03** | Pemulihan Sanity gratis lewat Batu Ruwatan/Shelter | Ada di jalur wajib; +15/10s, cooldown 30s [BASELINE] | Tanpa ini “tanpa belanja” bisa gagal |
| `P4-D04` | Harga Kemenyan ×5, PB ×3, Garam ×2 | Rp875rb/Rp750rb/Rp400rb; *hanya baseline* | Katalog ekonomi dan QA toko |
| `P4-D05` | Kapasitas Kendi dan mekanik item offering | 3 air, refill sumur gratis, Kendi yang sama bisa dibawa setelah offering | Potensi instance duplikat/puzzle tidak bisa diaktifkan |
| **P4-D06** | Saluran air ke Pendopo/sektor setelah ritual | Penampung fisik berasal dari Sumur, bukan refill UI gratis | Jarak backtracking dan pacing boss 20 menit |
| **P4-D07** | Sektor N/E/W bisa berprogres permanen? | Ya, CP6 rolling sector receipts | Berbeda dari kebijakan boss P3 all-or-nothing |
| **P4-D08** | Kapan CP4/CP5/CP6/CP7/CP8 ditulis? | Sesuai §6, transaksi atomik termasuk sektor | Fault-injection harus konsisten dengan save loader |
| **P4-D09** | Lama paparan inti tiap lokal/core | 5 detik sebagai baseline P3, bukan kanon P4 | Balancing/aksesibilitas ponsel |
| **P4-D10** | Penanda obor asli acak atau set posisi tetap? | 3 susunan authored/preset per seed stabil | Risiko pemain hafal posisi dan kehilangan replayability |
| **P4-D11** | Bentuk pemalsuan UI MataBatin.apk | Hanya chat/status non-kritis, world UV/objektif faktual tidak dipalsukan | Bisa jadi horor tidak adil atau tak terbaca |
| **P4-D12** | Penunjuk arah dan visual hazard non-UV | Landmark dunia + peringatan fisik; UV sebagai jawaban pasti | Risiko softlock battery 0 |
| **P4-D13** | Sektor Selatan sebagai pemadaman bukan local Blitz | Kanon, kemudian Blitz final di Pendopo = tembakan keempat | Kesalahan hit count menjadi lima |
| **P4-D14** | Definisi “Sanity 0 perburuan intens” | Deteksi meningkat, bukan auto-capture; cooldown terbatas | Kesulitan tak masuk akal |
| **P4-D15** | Kapan rahasia Cermin masih bisa dikumpulkan? | Sampai konfirmasi portal P5; pasca-boss area aman | Pola P1–P3 memberi peringatan sebelum transit |
| **P4-D16** | Nomor schema P4 dan nama class aktual | Audit aktual: schema4→5, migrasi1–4 dipertahankan; kelasREPO-10 | Save carry-over rusak / duplikat |
| **P4-D17** | Arah voice acting dan model dua wujud Banaspati | Siluet api + kerangka hangus, tidak wajib realistis wajah | Kesesuaian artistik dan lisensi aset |
| **P4-D18** | Target Windows build performa final | 720p Low avg ≥30 dan 1% low ≥24, 60 target global; Windows test wajib | Linux headless tak memadai |
| **P4-D19** | P5 menerima Sanity/baterai 0 dari P4 | Carry-over aktual + sumber recovery diegetik dekat masuk P5 | Softlock pada frame pembuka P5 |
| **P4-D20** | Apakah mayoritas forum 1997 bisa dianggap sejarah pasti? | Narator artikel dapat bias; Cermin memuat perspektif korban; tidak mengubah fakta tahun | Twist/gambaran warga korban dan Mbah Suro |

**Status produksi25Oktober2026:** pilihan teknis P4-D01–D16/D19 diterapkan sesuai§0.11 berdasarkan arahan implementasi desainer dan audit repo; tidak menunggu persetujuan ulang. P4-D17/D18 tetap memerlukan review akting/visual dan benchmark Windows, sedangkan D20 mempertahankan perspektif naskah tanpa mengubah tahun atau korban. Kondisi `KANON` (4 Blitz total, 4 obor asli, 3 bukti/pusaka, Sanity 0 bukan mati, air refill gratis, tanpa P5 Kos) tidak boleh dibalik secara diam-diam. Jika QA atau geometri memaksa penyesuaian parameter, revisi baseline beserta alasan dan bukti, bukan menghapus kriteria penting.

### 11.3 Risiko P0/P1 yang perlu dilaporkan lebih dulu

- **P0 Gameplay:** kemenyan benar-benar diwajibkan padahal stok 0; air habis dan sumur tertutup; semua pengisi daya hanya di jalur UV yang baterainya 0; final Selatan menuntut lokal Blitz lalu foto inti; Sanity=0 melepas kendali tanpa recovery.
- **P0 Save:** satu sektor tersimpan namun item/air/baterai tidak atomik; CP7 segel keempat hilang walau portal terbuka; CP8 ditulis sebelum perubahan chapter; rahasia popup muncul sebelum file berhasil.
- **P1 Presentation:** simbol warna tidak dapat dibedakan di 720p; chat palsu menutup tombol host yang benar; api Banaspati menutupi telegraph dan hitbox; kos membentuk scene paralel baru; VO penting tidak terdengar/subtitle kurang.
- **P1 Engineering:** 12 obor shadow caster membebani Iris Xe; overlap layer UV dengan AR/thermo/camera front; interaksi NPC + altar duplicate; stage derived tidak konsisten dengan checkpoint rolling.

---

## 12. Checklist produksi untuk Codex

**M0 — Audit repository sebelum coding**

- [ ] Baca `Phase4.md` ini secara penuh, `Phase3(5).MD`, dokumen Phase1/2 yang ada dalam repo dan `Rancangan.MD` bila tersedia. Catat perbedaan kontrak `READY / ADAPT / BUILD / BLOCKED` dengan bukti path + method.
- [ ] Temukan `CampaignState`/loader/`TryCommit`, mode HP UV/EVP/AR/Thermo, input & Jurnal aktual; catat versi schema dan alias receipt P3 yang ada.
- [ ] Identifikasi master scene Golden Kos, character controller Raka, live chat, motor/portal scene routing; tidak membuat duplikat.
- [ ] Tinjau 20 P4-Dxx. Perubahan kanon/story butuh persetujuan; baseline teknis boleh diuji sambil mempertahankan perilaku solvable.

**M1 — Vertical slice Kos, hutan dan Sanity**

- [ ] Dekor Kos sore dan TV lokal + forum 1997; stok P3 carry-over & toko optional; UV v2 + Filter Anti-Sengkala unlocked gratis.
- [ ] Hutan 3 simpang lengkap dengan HUD chat palsu, tanda UV fisik, 3 waypoint & checkpoint, dan pemulihan gratis yang benar-benar dapat diakses.
- [ ] Sanity tier 70/40/0 dan efek visual/audio terpusat; kontrol tetap responsif; QA first 40 dijalankan.

**M2 — Pendopo dan tiga sektor investigasi**

- [ ] Reveal Banaspati + Mbah Suro (bukan kill-on-spawn), empat landmark, rute balik, buku rajah, kacamata Damar.
- [ ] Utara bukti Thermo + Kendi, Timur foto AR + Belati/Damar, Barat EVP + tanah/sumur/Cermin; urutan bebas.
- [ ] Jurnal 3 bukti, konfirmasi eksplisit, ritual altar; pickup dan secret atomic, semua teks dapat dibaca ulang.

**M3 — Distributed ritual 3+1**

- [ ] 12 obor kandidat dengan UV signatures stabil, bentuk selain warna, layout seed persisted; 1 asli+2 palsu tiap sektor.
- [ ] Kendi sebagai satu instance dan volume air dapat diisi ulang fisik; opsi air toko bukan syarat.
- [ ] Tiga local Blitz N/E/W dalam urutan bebas dan commit sektoral; Selatan hanya siram, lalu core merge Pendopo dan Blitz final.
- [ ] Dinamo fisik dan shrine recovery, AI Banaspati legal transitions/LOS, tanpa miss/low battery hardlock.

**M4 — Epilog, P5 continuity dan art/audio**

- [ ] Empat segel di HP membuka portal kelima, 999.999 viewer dan error naratif, suara Damar, CKREK setelah CP8 dan konfirmasi.
- [ ] Arsip lokal/global N/5 dan jalan kembali ke Cermin sebelum portal.
- [ ] Aset desa berlisensi, VO Indonesia, partikel rendah overdraw, render quality, screenshot dan benchmark Windows asli.

**M5 — QA & acceptance**

- [ ] Jalankan 180 QA di §10 dengan kasus tepi khusus 0 money/0 Sanity/0 battery/no incense/no water pack/4 torches/4 hits.
- [ ] Jalankan regresi P1–3: `V/X/R/C/T/Q/H/G/F/E`, economy, Jurnal, save, preview camera, lighting Kos.
- [ ] Buat `Audit/phase4-feasibility.md`, `Audit/phase4-qa-matrix.md`, `Audit/phase4-implementation.md`, asset manifests dan screenshot Windows.
- [ ] Tandai tiap QA `PASS/FAIL/BLOCKED/NOT_RUN` disertai bukti. Jangan mengklaim 180 PASS dari build headless.

---

## 13. Catatan penyusunan: sumber, penambahan, dan yang sengaja ditolak

| Area | Dari Phase4 asli | Perluasan beralasan (dokumen ini) | Tidak diadopsi |
|---|---|---|---|
| Cerita dan lokasi | 5 adegan, 78 menit, Kos–Bambu–Pendopo–4 arah–Pasar Setan | Timeline/level beats, tanda kejelasan tiga simpang, backtracking | Memecah jadi 6 adegan demi angka sama P1/P2 |
| Mekanik Sanity | 1%/10s di hutan, threshold 70/40, 0 = hunt | Free shrine, hysteresis tuning, recovery dengan cover, accessibility | Sanity 0 otomatis mati / harus bayar kemenyan |
| Chat | Entity spoof account, notifikasi, Donasi Terakhir | Chat event authored dengan verifikasi world, host action tidak dibajak | Classifier berbasis username angka 97 saja |
| AI | Banaspati bola api, arwah Mbah Suro, hiding liang/kolong/gentong | Waypoints air patrol, state transitions, LOS, telegraph, two visual forms | Dua boss HP bar bertumpuk |
| Investigasi | Thermo +66, AR altar, EVP sumur, 3 pusaka | Event registry, receipts dan Jurnal idempoten, free-order | Menaikkan kebutuhan bukti menjadi 4 demi 4 obor |
| Ritual | 4 obor asli dalam 12 kandidat; Air + Blitz | Stabil seed, sektor N/E/W free, S final, CP6 rolling, item Kendi instans tunggal | 4 local Blitz ditambah 1 core (lima tembakan) |
| Secret | Cermin Saksi di relung, kesaksian 1997/2004 | Commit before feedback, Arsip N/5 aktual, backtrack | `4/5` hardcoded meski rahasia sebelumnya belum diambil |
| Ending | 999.999 viewers, error Rp999.999.999, empat segel buka gerbang | Atomic CP7/8 dan carry-over ke P5 malam sama | Memaksa baterai sah turun jadi 1 bar tanpa sistem gameplay |
| Engineering | Hanya 7 acceptance kanon dan mekanik ringkas | QA 180, resource solvability, registry, roles, risk register | Memaksa schema `5` tanpa audit repository |

### 13.1 Tingkat keyakinan dan bukti

Naskah lokasi, urutan adegan, dialog, nama tokoh, nominal dan item rahasia bersumber dari **`Phase4(1).MD` (naskah ringkas 15 Oktober)**. File **`Phase4 (1).MD` adalah ekspansi Claude** terhadap naskah tersebut, sedangkan **`Phase4_pre_merge.md` adalah versi ChatGPT v1.0** yang mendasari dokumen ini. Detail tambahan kedua model dibedakan dari kanon lewat label BASELINE/USUL, bukan menjadi fakta repo. Struktur 0–14, target verifikasi Unity, strategi save dan performa terinspirasi pola `Phase3(5).MD` dan merupakan **spesifikasi yang perlu audit**, bukan laporan build. Istilah target performa, nilai shrine/baterai, cooldown, harga, name ID, posisi sensor, randomness torch, dan pilihan shelter adalah **usulan teknis** yang dapat disesuaikan setelah Codex memeriksa repository dan playtest. Kalimat ini menjelaskan tahap penyusunan merger awal. Implementasi25Oktober2026 dilaporkan terpisah di `Audit/phase4-implementation.md`; hasil QA yang benar-benar dijalankan berada di `Audit/phase4-qa-matrix.md`.

---

### 13.2 Evaluasi dua spesifikasi secara berdampingan

| Kriteria (penilaian subjektif atas dokumen, belum build) | Claude | ChatGPT v1.0 | Hasil merger |
|---|---:|---:|---|
| Ketelitian naskah dan kontinuitas | 9,1 | 9,0 | Detail dialog/lore asli dipertahankan |
| Perincian parameter AI/level | **9,2** | 8,6 | Preset, m/s, layout tiga simpang diadopsi sebagai baseline |
| Inovasi gameplay P4 dibanding P1–3 | 8,2 | **9,4** | Perburuan empat sektor + perbedaan boss retry dipertahankan |
| Ketahanan softlock dan retry | 7,9 | **9,2** | Recovery + CP6 rolling + orakel transaksi ditambah |
| Kemudahan audit Codex | 8,8 | 9,0 | 180 QA, satu konfigurasi, status READY/ADAPT/BUILD/BLOCKED |

**Penilaian bukan verifikasi produksi.** Tidak ada bukti bahwa 180 QA telah dijalankan atau hasil build visual memenuhi sasaran. Dokumen merger belum menyamai kualitas game sampai tes nyata membenarkannya.

## 14. Peta integrasi teknis Unity dan handover audit

### 14.1 Reuse atau bangun secara aditif

| Kebutuhan | Referensi yang disebut pada spesifikasi Phase3 v2.3 | Implementasi aman P4 |
|---|---|---|
| Campaign & checkpoint | `Core/CampaignState.cs`, `CampaignData`, `TryCommit` (nama diverifikasi) | Tambah struktur P4 dari model P3; tidak ada singleton save baru; prioritas atomic receipt dan rolling CP6 |
| Input fisik HP & kamera | `Player/ShoulderCameraRig.cs`, `GameInputConfig`, `SmartphoneController.cs` | Satu phone controller, Sanity observer dan UV v2; `H` consumable di input resolver global |
| AR/Thermo/EVP | Mode scanner Phase1–3 pada `AtmajaPhoneView`/smartphone | Tiga bukti memakai masing-masing mode existing; metadata authored EVP di Sumur |
| Live stream & Pasar Gaib | Provider chat deterministic, shop receipt dan economy P3 | Tambah event P4, jangan koneksi streaming sungguhan atau saldo overlay palsu |
| Jurnal | `UI/PhoneDesktop.cs`/Jurnal provider (perlu verifikasi) | Tab sektor + bukti P4, status Arsip N/5; tidak simpan stage ganda |
| Actor & interaction | `IInteractable`, `InteractionInteractor`, animasi controller Raka | Interaksi dudukan, sumur, obor, portal; baterai/lampu tak memblokir `E` |
| AI | Wewe P3 memakai direktur threat dan LOS/telegraph, source harus diaudit | Banaspati pakai waypoint terbang/cover dan AI strobe spesifik, bukan mengubah AI Wewe |
| Save migrasi | P3 menyebut schema runtime aktual setelah implementasi | Derive P4 schema via migration proven; golden save P3 preserved |
| Rendering | Unity 6.3 LTS URP dan layer UV existing | World UV shape via shader/decal, cheap fog/fire, Intel Iris Xe pass |
| Routing | `GameShell`, scene `Babak3_PabrikGula` ke akhir | `Babak4_KarangSetra` + boundary fisik bawah pendopo setelah CP8; handoff scene P5 penuh menunggu implementasi babak5 |

### 14.2 Matriks prioritas audit sebelum implementasi

| Prioritas | Verifikasi minimum | Bukti yang dibutuhkan |
|---|---|---|
| **P0** | Sanity 0 bukan softlock; sumber air dan charger gratis; 3+1 Blitz; CP7/8 atomic | Uji runtime + pathfinding proof serta fault-injection |
| **P0** | Reuse CampaignState dan snapshot P3, 3 segel dari save | Test migrasi dengan save nyata multi-skenario |
| **P1** | 12 obor world UV terlihat tanpa warna tunggal, seed stabil | Screenshot/short capture real build |
| **P1** | Chat tidak mengambil prioritas input, pause/render stabil | Test input focus, viewport OTS/FP dan 30/60 FPS |
| **P1** | Banaspati AI tak spawn di anchor, sektor bebas urutan | Test navmesh/LOS and screen capture |
| **P2** | Kualitas voice, suara bambu, musik gamelan, artefak props berlisensi | Asset audit plus review audio dan pacing |

### 14.3 Format laporan Codex yang diwajibkan sebelum freeze

`Audit/phase4-feasibility.md` harus mempunyai satu baris per subsystem/fungsi: **READY / ADAPT / BUILD / BLOCKED**, bukti path dan symbol, mismatch dengan file ini, rekomendasi, serta risiko P0/P1/P2. `Audit/phase4-qa-matrix.md` berisi 180 ID yang identik dengan §10 dan status awal `NOT_RUN`. `Audit/phase4-implementation.md` mengikat commit, build, tes yang benar-benar dijalankan, keterbatasan Linux/headless, aset, tangkapan visual dan FPS Windows. Bila narasi kanon atau identitas P4 diubah karena keterbatasan engine, perubahan ditandai `[REVISI]` beserta alasan dan alternatif tanpa menghilangkan karakter horornya.

### 14.4 Persetujuan desain dan status akhir

**Dokumen ini lengkap sebagai rancangan produksi**, belum berarti kualitas *gameplay* sudah terbukti. Sebelum mulai implementasi menyeluruh, Codex melakukan audit kelayakan terhadap repo aktual, memperbarui `P4-D01–D20`, lalu mendokumentasikan perubahan. Jangan membangun ulang sistem P1–3, jangan menyatakan semua QA PASS sebelum diuji, dan jangan memperpanjang game dengan *fetch quest* tambahan hanya demi mengejar 78 menit.

---

## Lampiran A — Kriteria selesai yang diwarisi naskah Phase4 asli

1. Semua jalur wajib selesai dengan perlengkapan tetap, **tanpa membeli item opsional**.
2. Tiga bukti unik tersimpan dan dapat dibaca setelah reload; duplikat tidak menambah hitungan.
3. Arena/ritual baru dimulai setelah konfirmasi Jurnal + pusaka lengkap; keluar dari inspeksi tidak mengunci player.
4. Miss Blitz membuka kesempatan ulang dan tidak menghabiskan seluruh ritual.
5. Keping Cermin Saksi tersimpan sekali dan tahan kematian/reload.
6. Status collectible jelas sebelum transisi, clue tersedia tanpa membaca chat.
7. Save akhir memuat segel keempat, upgrade, inventory, saldo sah, Sanity dan Arsip Saksi secara konsisten.

**Tambahan invariant desain:** 4 obor asli, 8 obor palsu, **4 Blitz total**, Sanity 0 tidak mematikan otomatis, air dari Sumur tidak dapat habis permanen, dan Babak 5 berlangsung pada malam yang sama.
