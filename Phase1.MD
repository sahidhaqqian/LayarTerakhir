# PHASE 1 — PENGHUNI LANTAI EMPAT

> **Project:** Layar Terakhir · **Babak:** 1 · **Lokasi:** Rusunawa Griya Asri · **Versi:** 2.2-r1, penyelarasan audit repositori Codex, 23 Oktober 2026 (basis naskah v2.2 Claude + Gemini)  
> **Jenis dokumen:** *Single source of truth* untuk naskah, level design, mekanik, implementasi Unity, penyimpanan, dan QA.  
> **Status:** **Implementasi diizinkan 23 Oktober 2026; sedang dikerjakan dan diuji**, **belum phase-freeze** dan belum menjadi bukti seluruh penerimaan gameplay lulus. Status implementasi sesudah audit dijelaskan pada §0.6. Aturan global yang sudah jelas disalin ke §0.4; proposal yang berbenturan ditahan, bukan otomatis dijadikan kanon. Seluruh 74 QA tetap acceptance yang perlu dicocokkan dengan hasil aktual.
> **Rujukan lintas babak:** `Rancangan.MD` (parameter global dan kontinuitas cerita). Bila ketentuan global belum diketahui, jangan membuat asumsi diam-diam.

## 0. Cara membaca dan batas keputusan

Dokumen ini menggabungkan **naskah asli Phase1.MD** dan **spesifikasi teknis tambahan** yang diberikan secara terpisah. Dialog, lokasi, urutan enam adegan, angka-angka cerita, ID item rahasia, dan mekanik inti dipertahankan. Informasi teknis yang semula berupa tabel dengan format rusak dirapikan dan dipindahkan ke satu bagian implementasi. Untuk membedakan sumber dari penambahan:

- **[KANON]** = ketentuan yang sudah ada pada dua dokumen sumber, dipakai sebagai acuan resmi babak ini.
- **[KLARIFIKASI]** = penjabaran implementasi untuk menghindari interpretasi berbeda/softlock, tanpa mengubah alur kanon.
- **[USUL BARU]** = tambahan desain, UX, produksi, atau QA dari proses penggabungan; boleh diterima, disetel, atau dihapus sebelum produksi.
- **[PERLU KEPUTUSAN]** = masih memerlukan putusan penulis atau verifikasi `Rancangan.MD`.
- **[BASELINE UJI]** = nilai prototipe agar bisa dikode dan playtest, bukan kanon final.
- **[REVISI EDITORIAL]** = perbaikan teks lore/puzzle untuk menyambungkan kontradiksi sumber.

**Prioritas bila terjadi benturan:** keputusan eksplisit penulis dalam `Rancangan.MD` dan dokumen desain terbaru > babak kanon di sini > rekomendasi teknis di sini. Bagian *Naskah Adegan* mempertahankan ucapan/kejadian aslinya; pelaksanaan logikanya mengikuti aturan terperinci pada *Kontrak Implementasi*.

### 0.1 Ringkasan babak (KANON)

| Elemen | Ketentuan |
|---|---|
| Judul | Babak 1 — **Penghuni Lantai Empat** |
| Lokasi | **Rusunawa Griya Asri**, beton 4 lantai, koridor melingkar berbentuk O, void/lapangan jemuran terbuka di tengah |
| Waktu pembuka | **23:55 WIB**; update `MataBatin.apk` pukul **23:58 WIB** |
| Panjang jalur utama | **60 menit**, tidak termasuk pengambilan rahasia dan pengulangan setelah kalah |
| Karakter | **Raka**; Damar hadir lewat jejak, barang, rekaman/video; Tari dan Mbah Suro terkait misteri |
| Boss | **Genderuwo Arang**, juga disebut **Si Jagal Arang** dalam dialog/produksi |
| Ancaman pendukung | **Tuyul** (baterai/powerbank) dan **Kuntilanak Putih** (pantau lewat kamera, mundur) |
| Modul HP | **AR penuh**, **Jurnal Klenik v2.0**, **EVP Recorder** dasar, **Blitz/Flash** |
| Bukti wajib | **Cakaran Kamar 402**, **EVP kamar mandi umum**, **aura keris di tungku dapur** |
| Pusaka tunggal | **Pecahan Keris Penumbal** |
| Boss fight | Pecahkan perisai di **halogen Utara–Timur–Barat**, jendela **5 detik**, **1 Blitz valid tiap lampu**, **3 keberhasilan** |
| Rahasia | **Negatif Foto Tari**, ID `SECRET_P1_NEGATIF_TARI`, bagian **Arsip Saksi 1 dari 5** |
| Hasil | **Kain Jaket Damar**, **Kaset Pita Mbah Suro**, **segel pertama**, menuju **RSUD Lama Sido Waras** |

### 0.2 Alur level dan sasaran waktu (KANON + BASELINE UJI)

| Adegan | Target | Area dan tindakan utama | Keluaran/gate |
|---|---:|---|---|
| 1 | 00–08 menit | Parkir, motor, pos satpam, update HP, live stream | Temukan jalur ruang pompa |
| 2 | 08–22 menit | Ruang pompa, ilusi Kamar 102, Tuyul, kotak surat 207 | **Kode 644** → kunci lift; item rahasia opsional |
| 3 | **22–27 menit** [BASELINE UJI] | Lift barang macet, lorong 40 m, petunjuk Kuntilanak, cutter dan rantai; klimaks kejar ±10–15 detik | **Bolt cutter** → tangga ke L4 |
| 4 | **27–48 menit** [BASELINE UJI] | L4: orientasi ancaman, 402, 403, kamar mandi/EVP, dapur/keris, kembali ke 404; tahapan observasi dan stealth konkret §3.4A | **3 bukti unik + konfirmasi Jurnal + Pecahan Keris** |
| 5 | 48–56 menit | Kamar 404 + koridor loop; perisai dan halogen | **3 Flash segel** → Genderuwo tersegel |
| 6 | **56–60 menit** [TARGET UJI] | K404: kain, kaset, epilog → tangga L4→L3 → lift pulih langsung ke L1 (atau opsional singgah L2) → motor parkiran | Panel Arsip → konfirmasi `P1_COMPLETE` → save + fade; variasi waktu dinilai lewat playtest |

**Jalur kritis:** Parkir/pos satpam → ruang pompa → Kamar 102 (AR) → Lantai 2/kotak surat 207 (`644`) → lift barang → Lantai 3/Kuntilanak + bolt cutter → Lantai 4/3 bukti + konfirmasi Jurnal + keris → altar 404 → 3 halogen → kain dan kaset → epilog tanpa fade → L4→L3 → lift pulih ke L1 (opsional L2) → motor parkiran → `P1_COMPLETE`.

**[BASELINE UJI — pembagian durasi, bukan perubahan target satu jam]** Karena pengejaran L3 hanya ±10 detik, target Adegan 3 dipadatkan dari 10 menit menjadi 5 menit. Alokasi 5 menit dipindahkan ke investigasi L4 agar enam adegan tetap menargetkan 60 menit. Adegan 3 memuat ±2–3 menit inspeksi lift, suara, pencarian lokasi cutter, dan pengenalan aturan; hindari puzzle/filler artifisial hanya untuk menahan pemain. Tinjau ulang distribusi setelah playtest bertanda waktu.

**[KLARIFIKASI v2.2 — perjalanan pulang]** Elevator barang memiliki titik servis L1 di sisi **ruang pompa**, bukan di lobi utama yang dilas. Setelah `P1_BOSS_WIN`, panel lift di L3 memungkinkan **L1 (rute utama cepat)** atau **L2 (rute arsip opsional)**; lift **tidak** melewati L4. Jalur L2↔L1 lewat K102 tetap terbuka permanen. Perjalanan keluar ditargetkan sekitar **60–90 detik** setelah epilog untuk pemain yang tidak kembali mencari rahasia; target 60 menit adalah sasaran *playtest*, bukan batas waktu paksa, dan pemain opsional boleh lebih lama.


### 0.3 Pilar pengalaman (KLARIFIKASI)

1. **Investigasi lebih penting daripada combat:** musuh tidak bisa diselesaikan dengan menembak flash sembarangan; bukti dan pemahaman ruang membuka ritual.
2. **Horor Indonesia yang bisa dipahami lewat lingkungan:** bekas kebakaran, penghuni rusun, dokumen 2004, dan lokasi benda dibuat terasa satu peristiwa.
3. **Ponsel sebagai alat sekaligus risiko:** AR/EVP memecahkan puzzle, layar menarik Genderuwo, baterai diganggu Tuyul.
4. **Ketegangan dengan aturan adil:** musuh punya telegraph audio/visual, tindakan wajib dapat dilakukan, kematian tidak menghapus progres permanen.
5. **Jalur rahasia menghargai rasa ingin tahu, bukan uang:** Arsip Saksi tidak dijual lewat saweran; semua progress utama dapat diselesaikan tanpa donasi.

---

### 0.4 Kontrak repositori yang mengikat implementasi (AUDIT r1)

**Baseline diperiksa:** commit `e5ab282d204838aee07b78797dd9d5eeab0809d5`, Unity `6000.3.26f1`, URP `17.3.0`. Build hanya memuat `Prolog_KamarKos`, `Prolog_RumahAtmaja`, dan `Prolog_KoridorKos`. **Rusunawa, AI P1, EVP, dinamo, halogen ritual, collectible P1 dan routing CP1–CP7 belum diimplementasikan.** Kode yang sudah ada adalah fondasi, bukan bukti Babak 1 playable. Detail bukti dan 74 QA: [Audit/PHASE1_V2_2_REPOSITORY_AUDIT.md](Audit/PHASE1_V2_2_REPOSITORY_AUDIT.md).

Dokumen ini adalah acuan implementasi P1 di root. `Assets/_LayarTerakhir/src/Docs/Phase1.MD` merupakan salinan historis 15 Oktober, **bukan** acuan kedua. Jangan memakai isinya untuk menimpa versi ini. Nama class pada §9 adalah peran usulan; gunakan peta kode §14.

| Kontrak bersama yang sudah ditetapkan | Keputusan kerja r1 |
|---|---|
| Kamera dan kontrol | Pertahankan desain pemain terbaru: OTS + first-person melalui `ShoulderCameraRig`; `V` perspektif, `X` Selfie (modul masih terkunci). WASD/Shift/Ctrl/Space/E/Tab/F/Q/R/T/C/J/B/G/H/LMB mengikuti `GameInputConfig`. `Rancangan.MD` masih menulis V=Selfie dan prototipe first-person; ini utang sinkronisasi dokumen lama, bukan alasan menghapus OTS yang diminta desainer. |
| Energi HP global | Kamera normal **0,05 poin persen/detik**, AR/UV **0,10 total/detik**, foto bukti **1%**, Blitz **15%**, cooldown **6 detik**, powerbank besar **+80 poin** (kecil kos **+35** adalah implementasi existing). EVP overlay tidak menambah drain tersembunyi. HP disimpan/layar mati menghentikan drain dan tidak memutus live. |
| Baterai nol | Default: layar mati, AR/EVP/Flash normal tidak berfungsi, bukan Game Over. `WATCH_ONLY` L3 adalah **proposal pengecualian D01**, bukan fitur existing atau izin mengubah aturan semua scene. Top-up CP3 20% juga baseline sekali yang perlu ratifikasi carry-over. |
| Boss dan energi | Default global: Blitz 15%/6s dan **titik daya darurat arena** yang terjangkau saat recovery. Sumber cahaya habis hanya setelah hit sukses; miss/timeout dapat diulang. Flash bertenaga halogen pada 0% adalah **alternatif D01 yang ditahan**; jangan implementasikan bersama charger sebagai dua ekonomi default. |
| Ritual dan Jurnal | **3 bukti unik + konfirmasi eksplisit Jurnal + pusaka** sebelum damage penyegelan/ritual. Tiga scan tidak otomatis dianggap konfirmasi. `P1_JOURNAL_CONFIRMED` menjadi receipt P1; bukan memindahkan bukti ke inventori event sebagai tiga screenshot. |
| Sanity | `Rancangan.MD` menetapkan Sanity gameplay **mulai Babak 4**. P1 mempertahankan nilai save yang diwariskan; **tidak menguras, meregenerasi, atau menambah radius napas berdasar Sanity**. Usul Sanity P1 §8.1A ditahan sebagai D02. Tidak membuat sistem health/Sanity paralel. |
| Kanon lokasi | Lima gerbang = Rusunawa, RSUD, Pabrik Gula, Karang Setra, Pasar Setan; **empat segel lokasi dari Babak 1–4**. Rumah Atmaja bukan gerbang keenam. Ini sudah diputuskan `Rancangan.MD`, tidak perlu persetujuan ulang. Nubuat kaset 1998 tetap D05. |
| Arsip Saksi | ID P1 **`SECRET_P1_NEGATIF_TARI`**; ID unik dalam campaign, autosave atomik sebelum indikator sukses, tetap ada saat kematian/reload checkpoint terkini. `New Game` kosong. Chapter select backlog; setelah keluar, ulang lewat save relevan. Memulihkan backup lama secara sadar perlu menjelaskan potensi rollback data, bukan mengarang secret yang tidak ada di backup. |
| Uang | Rp78.200.000 tetap **milestone naratif kumulatif**, terpisah dari saldo. Dana kontrak terikat Rp500 juta termasuk muka Rp5 juta; jangan menambah nominal milestone ke wallet untuk menutup selisih. Sumber ledger dan status transfer tersisa D03. |
| Save dan event | Perluas **`CampaignState/CampaignData` yang sama**, file `.json/.tmp/.bak` yang sama. Sekarang schema **1**, list ID unik, event P0 banyak berada di `inventory`; belum ada event bus generik. Jangan membuat save/event/AR/inventori paralel atau migrasi massal flag P0. Contoh schema 2 §6.2 adalah desain masa depan, bukan format yang saat ini dapat dimuat. |
| Target performa | 60 FPS global masih sasaran sementara; 720p Low 30 FPS/1% low 24 pada Iris Xe adalah **usulan tier minimum D07**, belum mengganti target global dan belum terukur. |

**Pagar keputusan:** Ikuti kontrak global/default di atas. D01 (pengecualian energi), D02 (Sanity/dua-hit), D03 (ledger/transfer), D05 (kaset nubuat), D06 (carry-over/grant), D07 (tier performa), dan D08 (ratifikasi retry boss), dan D09 (urutan intro/endpoint chapter) harus tercatat sebelum freeze bagian terkait. Mekanik inti cerita tetap; prototype angka boleh diuji setelah milestone implementasi diizinkan. Jangan membaca label “siap blockout” sebagai izin coding dalam sesi audit.

### 0.5 Keputusan implementasi dari desainer — 23 Oktober 2026

**Arahan terbaru:** “tahan dulu sementara pakai global dulu aja nanti kalau gak sesuai aku ubah”. Bagian ini mengungguli baseline yang belum disetujui pada bagian berikutnya. Implementasi tidak boleh mengurangi kualitas visual, atmosfer, atau gameplay untuk mengejar checklist.

- **Revisi desainer terbaru:** perpindahan pertama `PROLOG_COMPLETE` → Phase 1 mengisi baterai ke **100%** sekali. Inventori/Sanity/uang tetap diwarisan yang sebenarnya. Continue/checkpoint/retry tidak mengisi ulang baterai. Tidak ada refill 100% saat Continue/retry, powerbank awal tambahan, top-up otomatis CP3 20%, ataupun floor CP5 40%.
- **Tidak ada WATCH_ONLY gratis pada 0%.** Baterai nol tetap mematikan layar. Tambahkan panel isi ulang manual fisik di landing lift L3 sebelum mengambil cutter. Panel memakai mekanisme dinamo existing P1 (25 poin/8 detik sekali, crank darurat hingga 10% bila habis), bukan energi otomatis. Retry memuat snapshot, bukan menambah energi.
- Sanity belum aktif di P1. Serangan langsung tetap fatal; jangan implementasikan proposal dua-hit/8 detik atau sistem health paralel.
- Blitz tetap 15% dengan cooldown 6 detik. Boss memakai titik daya arena fisik selama recovery, tanpa Blitz bertenaga halogen pada 0%. Pengisian arena tetap tersedia setelah boss agar transmisi HP tidak terhalang baterai nol.
- Retry boss memulihkan CP5 dengan baterai tepat snapshot dan tiga lampu utuh. CP6 disimpan atomik setelah kemenangan. Semua penulisan biasa ditolak selama attempt.
- Sumber ekonomi mengikuti perilaku existing: dana Penonton_Terakhir terikat, donasi manusia sah. Hanya nominal peristiwa yang benar-benar ditulis boleh dibukukan satu kali. Rp78,2 juta tetap capaian naratif, tanpa menciptakan transaksi penutup selisih.
- Status pembayaran kos/RS mengikuti receipt save, tidak dilunaskan diam-diam untuk menyesuaikan monolog.
- Nubuat kaset 1998 masih ditahan. Untuk implementasi sementara, rekaman hanya menyampaikan kanon global lima gerbang/empat segel; jangan mengklaim nubuat sudah disahkan. Dialog/performa manusia yang belum direkam harus dicatat sebagai belum final.
- Babak berakhir pada konfirmasi motor dan CP7; simpan tujuan RSUD tanpa membuat scene Babak 2. Video Damar memakai set rekaman non-playable di dalam scene P1.
- **Aset wajib existing atau unduhan internet berlisensi.** Tidak membuat model, gambar, tekstur, atau gerak original sendiri. Konversi/optimasi VRM pengguna, retargeting animasi existing dan konversi channel PBR diperbolehkan sebagai langkah impor, tanpa mengarang artwork. Sumber/lisensi ada di `Audit/phase1-asset-credits.md` dan `Audit/phase1-audio-credits.md`.
- Target 60 FPS global tetap dipertahankan. Headless/Linux software rendering tidak menggantikan pengukuran Windows pada GPU sasaran.

**Penyesuaian oracle QA:** QA-59 menguji baterai masuk **L3** 0/1/19/85 tetap sesuai snapshot dan panel manual dapat dipakai; secara terpisah, perpindahan pertama Prolog→P1 mengisi baterai ke 100% sekali; QA-60 memuat snapshot tepat tanpa grant berulang; QA-61 memverifikasi layar mati pada 0% dan pemulihan/retry melalui panel fisik; QA-62 memastikan Kuntilanak tetap resolved tanpa kamera gratis. QA-39 tidak boleh dianggap menyetujui nubuat. QA-41 memakai serangan fatal global. Status QA lainnya tetap rencana sampai hasil aktual dicatat pada laporan implementasi.

### 0.6 Status implementasi — build review, bukan phase freeze

Sesudah audit, desainer mengizinkan implementasi. Baseline pada §0.4 dan laporan audit tetap catatan historis. Implementasi sekarang memperluas `CampaignState` schema 2, routing Continue, handset/interactor existing, dan scene `Babak1_Rusunawa` pada index 3; tiga scene Prolog dipertahankan. Sumber runtime babak berada di `Assets/_LayarTerakhir/src/Phase1`; builder di `src/Editor/Phase1*Builder.cs`.

Laporan pelaksanaan dan status penerimaan berada di [Audit/PHASE1_IMPLEMENTATION_REVIEW.md](Audit/PHASE1_IMPLEMENTATION_REVIEW.md). Daftar QA §10 adalah acceptance, **bukan daftar yang otomatis sudah lulus**. Uji aturan/editor, native smoke yang diarahkan, playtest manusia, kualitas visual/audio akhir, pacing satu jam dan pengukuran GPU Windows mempunyai tingkat bukti berbeda. Versi implementasi wajib menyebut keterbatasan tersebut.

**Revisi visual setelah playtest desainer, 23 Oktober 2026:** build review pertama ditolak karena mutu bangunan/lingkungan. Phase 1 memakai pustaka lingkungan unduhan tersendiri, bukan furnitur, permukaan bangunan atau housing lampu Prolog. Gunakan modul beton/pintu/jendela berlisensi, UV pada skala nyata, tekstur PBR 2K dan dressing ruang sesuai fungsi. Sistem pemain/HP, karakter desainer dan motor yang dibawa dari babak sebelumnya tetap digunakan untuk kontinuitas. Dilarang membuat aset visual sendiri. Sumber dan checksum wajib dicatat di `Audit/phase1-environment-sources.json`. Penerimaan visual dan 74 QA tetap harus dibuktikan, bukan diasumsikan dari build berhasil.

### 0.7 Revisi playtest terbaru — baterai, kamera dan kepadatan lingkungan

- Baterai awal Phase 1 **100%** hanya pada transisi babak setelah Prolog. Ini menggantikan arahan carry-over baterai awal sebelumnya. Tidak ada refill otomatis CP3/CP5, dan retry/Continue tetap snapshot.
- Klik kanan **ditahan** untuk mengangkat kamera; lepas kembali ke keadaan/perspektif sebelumnya. Kamera fokus memperlihatkan HP horizontal dengan dua tangan dan bingkai fisik, berdasarkan referensi pengguna `r2:layarterakhir/Layarterakhir/dreadout-referensi/dread9.png`. App menu HP dialihkan ke Kamera selama tombol ditahan. Override hiding dan scripted pursuit tetap dipertahankan.
- Tambahkan minimal **30 model prop baru yang berbeda** dari sumber internet berlisensi, di luar modul bangunan/tekstur/duplikasi. Model harus benar-benar ditempatkan sesuai fungsi ruang. Sumber/hash: `Audit/phase1-additional-prop-sources.json`; posisi/penopang/ukuran: `Audit/phase1-additional-prop-placement.json`. Jalur koridor, tangga, petunjuk, keris, charging dan hiding harus tetap dapat dipakai.
- Gunakan aset yang sudah ada/unduhan. Kedua grasp FP memakai mesh tangan dan cuff karakter Raka berlisensi CC0 yang sudah ada, dengan pose horizontal melalui fitter geometri yang sudah tersedia dan konversi impor; tidak membuat model/tekstur tangan sendiri. Gambar DreadOut hanya referensi, tidak dimasukkan ke aset game.

## 1. Naskah enam adegan (KANON; bagian yang diubah diberi catatan audit)

> **[REVISI EDITORIAL]** Beberapa teks dokumen/dialog di bawah dikoreksi untuk menutup kontradiksi. Perubahan itu bukan kutipan verbatim versi pertama; periksa daftar perubahan §13 sebelum menetapkan kanon final.

### ADEGAN 1: Pelataran Parkir Rusunawa Griya Asri (Menit 00:00 – 08:00)

**Kondisi Awal:** Pukul **23:55 WIB**. Hujan gerimis dan kabut tipis menyelimuti halaman Rusunawa Terbengkalai Griya Asri—bangunan beton 4 lantai berbentuk huruf 'O' dengan lapangan jemuran di tengah (*void* terbuka) yang kumuh dan gelap gulita. Raka berdiri di samping motornya, sementara di jok belakang terikat Handycam Sony milik Damar. **[KEPUTUSAN DESAINER]** Baterai terisi **100%** sekali saat perpindahan setelah Prolog. Inventori berasal dari save sebenarnya; tidak ada powerbank awal tambahan. Continue/retry tidak melakukan pengisian ulang.

#### 1. Monolog Pembuka & Status Finansial

Raka menatap layar Smartphone yang menampilkan bukti transfer lunas ke rekening Bu Haji Ratna dan kuitansi deposit Rumah Sakit ibunya dari uang saweran Rp25.450.000 semalam.

* **Raka (Monolog Suara):** *"Kos udah lunas, deposit operasi Ibu udah masuk. Tapi Damar masih terjebak di balik cermin sialan itu... Empat segel harus gue kumpulin. Kalau gue mundur malam ini, Damar mati."*
* Tepat pukul **23:58 WIB**, HP Raka bergetar hebat. Aplikasi `MataBatin.apk` melakukan *patch* otomatis dengan bar merah bertuliskan: **`[MENGUNDUH MODUL SEGEL: JURNAL KLENIK v2.0]`**.

* **[NOTIFIKASI DONASI MASUK: Rp10.000.000 dari `Penonton_Terakhir`]**
* **Pesan Donasi:** *"Tepat waktu. Di Rusunawa ini, Mbah Suro menumbalkan empat belas penghuni dalam kebakaran tahun 2004 untuk membuka gerbang Rusunawa. Kali ini kau tidak boleh hanya kabur, Raka. Cari 3 bukti kematiannya, hancurkan perisai gelapnya dengan cahaya, dan segel kepalanya dengan kameramu."*

#### 2. Memeriksa Pos Satpam (Item Dokumen #1)

* **Objektif Layar:** `Cari akses masuk ke Gedung Rusunawa dan nyalakan Live Stream.`
* Pintu gerbang besi lobi utama dilas mati. Pemain masuk ke **Pos Satpam** di sebelah gerbang dan menemukan **Buku Laporan Piket Satpam (2004)**:

> **TEKS DOKUMEN (LOG SATPAM RUSUNAWA - 14 MEI 2004):**
> "22:10 — Penghuni Kamar 404 (Tari, penari jaipong) kembali ribut dengan penagih utang.
> 23:00 — Listrik blok timur sering konslet karena banyak warga menyambung kabel liar ke panel darurat lantai 4.
> 23:20 — Ketua RT mendengar ritual dari lantai 4; dari pos terlihat nyala api aneh di sana.
> 23:40 — Bau minyak tanah menyengat dari tangga lantai 3. Pintu darurat luar justru dikunci rantai oleh seseorang berjubah hitam dari luar. Kalau terjadi apa-apa, satu-satunya jalan masuk alternatif hanya lewat **saluran pembuangan ruang pompa di belakang gedung**."

#### 3. Menyalakan Live Stream & Tantangan Saweran Pertama

* Pemain mengangkat Smartphone (`[TAB]`) dan menekan **`[MULAI LIVE]`**.

* Karena insiden Rumah Atmaja semalam viral, begitu siaran dibuka angka penonton langsung melonjak ke **[LIVE | 👁️ 18.500 Penonton]**.

* **Raka (Dialog Suara ke Kamera):** *"Malam semuanya. Gue tahu video semalam udah nyebar ke mana-mana. Malam ini gue di Rusunawa Griya Asri... Gue nggak butuh kalian percaya ini settingan atau bukan, gue cuma butuh mata kalian buat bantu gue selamat."*

**Rentetan Live Chat #1:**

* `@detektif_malam`: *"GILA LANGSUNG 18K VIEWERS! Bang Damar beneran belum ketemu bang?"*
* `@siska_eee`: *"Rusun Griya Asri kan yang kebakar tahun 2004 itu! Jangan naik ke lantai 4 bang!"*
* `@sultan_sawer` **[MISI SAWERAN Rp1.000.000]**: *"Bang, potret papan nama Rusunawa yang ada bercak darahnya pakai mode AR! Kalau beneran muncul penampakan, gue cairin sejuta!"*

---

### ADEGAN 2: Fase Gameplay A — Eksplorasi & Puzzle AR di Lantai 1 & 2 (Menit 08:00 – 22:00)

Menggunakan mekanik Fase Gameplay A: Eksplorasi & Puzzle dengan mengombinasikan kamera HP, lensa AR untuk melihat Alam Gaib, dan interaksi Live Chat.

#### 1. Ruang Pompa & Lorong Lantai 1

Pemain masuk melewati teralis ruang pompa air yang berkarat dan tiba di **Koridor Lantai 1**. Suasana lembap, air menetes dari plafon bocor, dan deretan pintu kamar rusun tampak hangus bekas terbakar.

* Tangga utama menuju Lantai 2 ditutupi tumpukan puing beton runtuh yang tidak bisa dilewati secara fisik.

**Rentetan Live Chat #2 (Petunjuk Netizen):**

* `@anak_indigo`: *"Bang Raka, coba nyalain filter ijo (AR) di depan tembok Kamar 102! Tadi pas kamera abang lewat situ, layar gue nge-glitch!"*


**Eksekusi Mekanik AR Overlay:**

* Pemain menyalakan **Lensa AR (`[Q]`)** ke arah tembok Kamar 102 yang secara normal tampak disemen rapat.

* Di layar AR, tembok semen itu menghilang dan menampilkan pintu kayu Kamar 102 yang terbuka menuju tangga darurat bagian dalam.

* Pemain berjalan menembus tembok ilusi tersebut sambil menatap layar HP dalam mode AR dan naik ke Lantai 2.

#### 2. Gangguan Tuyul Penguras Baterai (Gimmick Hantu #1)

Saat pemain menyusuri koridor Lantai 2 yang gelap, terdengar suara cekikikan anak kecil (`"Hihihi..."`) dan langkah kaki kecil berlarian di genangan air.

* **Mekanik Tuyul:** Sesuai aturan unik hantu lokal, **Tuyul** tidak membunuh pemain, tetapi mencuri *powerbank* atau menguras baterai HP dari jauh.

* Tiba-tiba indikator baterai HP Raka berkedip merah dengan peringatan: **`[ANOMALI DRAINASE DAYA: -2% PER DETIK!]`**.
* **Cara Mengatasi:** Pemain harus menyalakan **Mode AR (`[Q]`)**, mencari sosok bocah kerdil berkepala plontos yang sedang bergelantungan di pipa plafon atau bersembunyi di balik tong sampah, lalu menembakkan **1x Flash Kamera (`[Klik Kiri]`)** untuk mengusirnya sebelum baterai terkuras habis.
* Jika pemain telat mengusirnya dalam 10 detik, Tuyul merampas **1x Powerbank Cadangan** dari inventori awal yang terjamin pada baseline; jika sudah kosong karena digunakan, ia hanya menyisakan kerugian baterai selama encounter dan kabur. Tidak pernah mengurangi stok di bawah nol atau menggandakan barang saat retry.

#### 3. Puzzle Lantai 2: Kotak Surat Penghuni & Kunci Lift Barang

Akses tangga dari Lantai 2 ke Lantai 3 ambruk total. Satu-satunya jalan naik adalah menggunakan **Lift Barang Tua** di ujung lorong yang membutuhkan **Kunci Kontak Besi**.

* Di dekat lift terdapat deretan **Kotak Surat Besi (Nomor 201 – 210)** yang terkunci gembok kombinasi, dan secarik kertas pengumuman pengelola rusun (Item Dokumen #2):

> **TEKS DOKUMEN (SURAT PENGELOLA RUSUNAWA):**
> "Kunci cadangan lift barang dititipkan pada Ketua RT di Kamar 207. Karena beliau sering lupa nomor gembok kotak suratnya, ingat urutan ini: **Jumlah kamar yang menunggak iuran di lantai 2**, diikuti **Nomor lantai tempat dukun Mbah Suro biasa menggelar ritual**, lalu **Jumlah sisi koridor yang mengelilingi void pada satu lantai, sesuai denah**."


**Cara Menyelesaikan Puzzle Kotak Surat 207:**

1. Pemain mengarahkan **Lensa AR (`[Q]`)** ke papan tulis tunggakan di dinding yang sudah terhapus: AR memperlihatkan daftar **enam kamar berstatus MENUNGGAK**, masing-masing diberi **cap merah "MENUNGGAK" (bukan nama yang dicoret)** (`201, 202, 204, 206, 208, 210`) → Angka pertama: **`6`**.
2. Dari log satpam dan *chat* penonton, ritual kebakaran berpusat di **Lantai 4** → Angka kedua: **`4`**.
3. Gedung berbentuk persegi ('O') dengan **4 sisi koridor** → Angka ketiga: **`4`**.
4. Pemain memasukkan kode **`6 - 4 - 4`** pada kotak surat 207 → mendapatkan **Kunci Kontak Lift Barang** dan **1x Powerbank Cadangan**.

---

#### Jalur opsional Arsip Saksi

Sebelum melanjutkan, pemain dapat memeriksa **Negatif Foto Tari**. Ruang arsip pengelola di ceruk belakang deretan kotak surat Lantai 2, sebelum menaiki lift barang. Di ceruk arsip tersembunyi, potret **daftar tunggakan arsip yang berbeda** dengan AR (bukan papan utama berkategori enam kamar menunggak). Kata **"nama yang dicoret"** merujuk **nama Tari yang dicoret silang dengan tinta hitam pada label daftar arsip**, berbeda jelas dari **cap MERAH "MENUNGGAK"** di papan puzzle enam kamar. Cocokkan nama Tari pada laci arsip. Laci yang benar membuka panel kecil berisi negatif foto; tidak memakai kode 644 dan tidak menggantikan kunci lift. Pengambilan tidak dihitung sebagai bukti atau pusaka boss. Sebelum meninggalkan babak, status item ini ditampilkan 0/1 atau 1/1 pada Jurnal.

### ADEGAN 3: Lorong Kuntilanak Lantai 3 (Menit 22:00 – 27:00 — BASELINE UJI)

**Kondisi Awal:** Lift barang berhenti dengan hentakan keras di **Lantai 3** akibat rem pengaman terkunci dan panel yang diganggu anomali gaib (Raka semula menyangka kabel sling macet). Pintu besi terbuka memperlihatkan koridor panjang lurus sejauh 40 meter dengan lampu neon yang berkedip-kedip hampir mati.

#### 1. Mekanik Kuntilanak: dengar dulu, balik, tatap (Gimmick Hantu #2)

Koridor Lantai 3 dibuka dengan **audio terlebih dahulu**: tangisan perempuan lirih terdengar dari ujung koridor begitu lift berhenti — tanpa label nama entitas, tanpa peringatan sistem. Panel daya servis di landing dapat dipakai sebelum bergerak (mekanisme dinamo manual yang sama dengan §8.0).

* Saat pemain mencapai tengah lorong, sosok **bergauun putih hangus muncul TEPAT DI BELAKANG pemain** (±8 m di belakang arah hadap, diklem ke dalam koridor). Raka refleks mengangkat kamera HP ke arahnya; pemain membalik badan dan mendapatkan momen *look-back* orang-pertama. Pintu tangga Lantai 4 berada pada ujung seberang.
* Setelah terungkap, aturan berlaku: **tahan kamera HP ke arahnya**; memalingkan kamera atau menurunkan HP lebih dari **1,5 detik** saat pengejaran dimulai menyebabkan *warp-attack* (Game Over). Tidak ada teks yang membocorkan nama atau ancaman sebelum momen terungkap.
* **Jendela bertahan yang adil:** foto jarak dekat (≤5 m, biaya foto normal 1%) **mengejutkan sosok itu ±1 detik** — gerak, timer lepas-pandang, dan tangkapan berhenti — sehingga pemain dapat mundur menjauh. Stun ini defensif seperti stun Prolog, bukan damage.
* Pengejaran dimulai saat pemotong besi diambil; potong rantai (E radius tangan ≤1,6 m, animasi 2 detik membekukan AI) mengunci `RESOLVED` permanen.

* Pintu tangga ke Lantai 4 digembok rantai, dan alat pemotong besinya (*bolt cutter*) ada di **ceruk Ruang Kebersihan di samping jalur menuju tangga**. **[BASELINE UJI]** Sesaat setelah lift berhenti, slot daya cadangan panel lift mengangkat baterai HP menjadi minimal **20% sekali** (tidak menambah powerbank, tidak dapat dipakai ulang). Ketika baterai tetap turun ke 0% selama encounter, mode pengawasan kamera darurat di L3 masih memungkinkan pemain menatap Kuntilanak; AR/EVP/Flash tetap tidak berfungsi pada 0% (§8.0).
* Begitu Raka mengambil pemotong besi dan Kuntilanak mulai bergerak mendekat dari ujung koridor, pemain **wajib berjalan mundur (`[S]`) sambil terus menatap Kuntilanak lewat layar kamera HP** menuju pintu tangga Lantai 4.

* Jika pemain memutar badan membelakangi Kuntilanak lebih dari 1,5 detik atau menurunkan layar HP, Kuntilanak langsung melakukan *instant-warp* menerkam leher Raka (*Game Over*).


**Rentetan Live Chat #3:**

* `@bambang_gentolet`: *"Bang... kok gue ikut denger suara nangis?"*

* `@putri_melati`: *"Tangannya udah panjang banget mau nyekik dari depan itu ya Allah!"*
* `Penonton_Terakhir` **[SAWER Rp3.000.000]**: *"Bagus. Tetap buka matamu. Tapi ingat, di lantai atas nanti bukan dia penguasa utamanya."*

---

### ADEGAN 4: Fase Gameplay B — Investigasi 3 Bukti & Stealth Genderuwo di Lantai 4 (Menit 27:00 – 48:00 — BASELINE UJI)

Menggunakan mekanik Fase Gameplay B: Investigasi & Survival (Phasmophobia + Outlast). Di fase ini, boss kebal terhadap damage penyegelan, pemain harus bersembunyi, menghemat baterai, dan mencari 3 bukti klenik lewat aplikasi HP untuk membuka segel eksorsisme.

#### 1. Pengenalan Bos Babak 1 & Jurnal Deduksi (`Phasmophobia` Mechanics)

Saat memotong rantai, tepi besi menggores tangan Raka sehingga terlihat berdarah dalam live chat penutup. Ia lalu naik ke **Lantai 4**—area yang paling parah terbakar dengan jelaga hitam tebal di seluruh dinding. Pintu **Kamar 404** di salah satu sayap koridor melingkar disegel oleh kabut hitam pekat yang berdenyut. **Pada Fase B, seluruh dinding perimeter Kamar 404 masih utuh**; tidak ada celah tembus di sekitar pintu yang dapat melewati segel.

* Aplikasi `MataBatin.apk` membuka menu **Jurnal Klenik**:

* **Target Penyegelan:** `Penunggu Lantai 4 (Kebal Serangan Kamera)`.

* **Syarat Membuka Ritual Eksorsisme:** `Temukan & Pindai 3 Bukti Klenik di Lantai 4 untuk mengidentifikasi kelemahan & lokasi Benda Pusaka.`

1. `[ ] Bukti Jejak Fisik (Cakaran)`
2. `[ ] Bukti Suara Gaib (EVP Recorder)`
3. `[ ] Bukti Aura Kematian (Lensa AR)`

#### 2. Kemunculan Bos: "Si Jagal Arang" (Varian Genderuwo Rusunawa)

Saat Raka melangkah ke tengah koridor Lantai 4, terdengar suara geraman berat berfrekuensi rendah yang menggetarkan beton dan langkah kaki raksasa (**`"DUG... DUG... DUG..."`**).

* Dari jebolnya tembok Kamar 401, keluar sosok **Genderuwo Gosong ("Si Jagal Arang")** setinggi 2,5 meter dengan tubuh legam berbulu kasar yang membara seperti arang panas dan mata merah menyala.

**Aturan Unik Genderuwo (Wajib Dipatuhi Pemain):**

* Di fase ini ia **kebal terhadap damage penyegelan**.

* Dia **sangat tertarik pada cahaya layar HP**. Setiap kali ia berpatroli mendekat, pemain wajib **mematikan layar HP (`[F]`)** dan bersembunyi di tempat gelap (lemari besi rusun, kolong meja, atau bilik WC).

* *Flash* HP hanya bisa dipakai untuk *stun* darurat selama 3 detik jika kepergok, namun menguras 15% baterai sekaligus.

#### 3. Mencari 3 Bukti Klenik Sambil Stealth

Pemain harus menyelinap di antara Kamar 401, 402, 403, dan Dapur Umum Lantai 4 sambil menghindari patroli Genderuwo:

1. **Bukti #1 — Kamar 402 (Jejak Cakaran Besar & Dokumen Korban):**
   - Pemain memindai bekas cakaran lima jari raksasa yang menghancurkan pintu besi Kamar 402 menggunakan kamera HP.
   - Di sampingnya ditemukan **Catatan Harian Tari (Item Dokumen #3)**:

> **TEKS DOKUMEN (CATATAN TARI - PENGHUNI 404):**
> *"Mandor bangunan ini pengikut Mbah Suro. Seminggu sebelum kebakaran, dia menanam kepala kerbau busuk dan keris patah di bawah ubin kamar 404. Tapi keris itu kemudian dipindahkan; malam sebelum kebakaran aku melihat mandor membawa bungkusan panjang ke dapur umum. Dia bilang sosok besar berkulit arang itu benci cahaya lampu sorot, makanya seluruh bohlam di lantai 4 sengaja dipecahkan."*

2. **Bukti #2 — Kamar Mandi Umum Lantai 4 (Rekaman EVP / Suara Gaib):**
   - Pemain menyalakan fitur mikrofon di `MataBatin.apk`. Saat suara raungan muncul di radio statis HP, terdengar rekaman suara gaib: *"Pecahan... keris... di tungku dapur... bawa... ke altar kamar empat kosong empat..."*.
* Tepat setelah rekaman selesai, Genderuwo mendobrak pintu luar kamar mandi! Pemain harus langsung masuk ke salah satu bilik WC dan menekan **`[F]` (Matikan Layar HP)**. Genderuwo berjalan menyusuri depan bilik dalam kegelapan; jika pemain menyalakan layar sedikit saja, pintu bilik langsung dihancurkan.

3. **Bukti #3 — Dapur Umum Lantai 4 (Aura Keris Pusaka via Lensa AR):**
   - Pemain masuk ke Dapur Umum dan menyalakan **Lensa AR (`[Q]`)**. Di dalam tungku pembakaran sampah yang penuh abu, terlihat pancaran aura emas dari **Pecahan Keris Penumbal**.

* Pemain mengambil **Pecahan Keris Penumbal** tersebut.


**Notifikasi Aplikasi `MataBatin.apk`:**

> **`[3 BUKTI TERKUMPUL! IDENTITAS: GENDERUWO ARANG. KELEMAHAN: HANCURKAN PERISAI KABUT HITAMNYA DENGAN LAMPU SOROT KORIDOR, LALU POTRET SAAT PERISAINYA PECAH!]`**

---

### ADEGAN 5: Fase Gameplay C — Klimaks & Eksorsisme Cahaya di Kamar 404 (Menit 48:00 – 56:00)

Menggunakan mekanik Fase Gameplay C: Klimaks / Eksorsisme yang menggabungkan penghancuran perisai kabut hitam menggunakan sumber cahaya ruangan ala Alan Wake dan serangan penutup Flash kamera HP ala DreadOut.

#### 1. Memulai Ritual Penyegelan

Pemain menuju **Kamar 404**. Kabut hitam yang menutup **satu-satunya akses masuk** **lenyap dan pintu fisik K404 dapat dibuka** begitu Raka membawa Pecahan Keris Penumbal. Pemain boleh masuk, memeriksa altar, lalu keluar lagi; **tiga bukti unik + konfirmasi Jurnal + keris** tetap diperlukan agar interaksi ritual berhasil. Dinding ruangan tetap utuh sampai ritual benar-benar dimulai.

* Pada Fase B, **dinding Kamar 404 utuh**, dengan altar berada di dalam ruangan. Kain Jaket Damar dan kaset adalah loot pasca-boss.
* Pemain menancapkan Pecahan Keris ke altar untuk memulai penyegelan (event `P1_ALTAR_INTERACT`). **CP5 diambil pada posisi aman tepat di depan altar ketika pemain pertama kali menekan E dengan tiga bukti, konfirmasi Jurnal dan keris lengkap**, bukan otomatis saat tiga bukti diperoleh di dapur. Bila commit CP5 gagal, ritual tidak dimulai (§6.3).
* **Pemicu Boss Fight:** Seluruh lantai bergetar hebat. **Baru pada momen ritual dimulai**, Genderuwo menjebol panel dinding Kamar 404 yang mengarah ke dua sisi koridor sehingga arena tersambung dengan **loop Lantai 4**. Perisai Kabut Hitam aktif dan pertarungan dimulai. Kedua bukaan didesain dapat dilalui karakter/boss tanpa menyangkut collider atau memutus jalur pulang.

#### 2. Mekanik Boss Fight (3 Putaran *Alan Wake × DreadOut*)

Arena pertarungan adalah koridor melingkar Lantai 4 yang memiliki **3 Panel Sakelar Lampu Sorot Konstruksi (Halogen)** di sudut Utara, Timur, dan Barat, serta beberapa Tuyul pengganggu yang sesekali muncul mencoba mencuri baterai HP Raka.


**Langkah 1 (Hancurkan Perisai Gelap — Mekanik *Alan Wake*):**

* Pemain harus memancing Genderuwo Arang mendekati jangkauan **Lampu Sorot Konstruksi**, lalu menarik tuas sakelar dinding untuk menyalakan sorotan cahaya halogen berdaya tinggi tepat ke tubuhnya.

* Cahaya lampu sorot membakar dan menghancurkan **Perisai Kabut Hitam** di tubuh Genderuwo hingga tubuh aslinya terekspos dan ia meraung kesakitan menutupi mata!


**Langkah 2 (Serangan Penutup Kamera — Mekanik *DreadOut*):**

* Saat perisai kabutnya pecah selama 5 detik, layar `MataBatin.apk` memunculkan bidikan segel merah di kepala Genderuwo.
* Pemain mengangkat HP dan menekan **`[Klik Kiri]` (Flash Kamera)** untuk memberikan *damage* penyegelan!


**Langkah 3 (Ulangi di 3 Titik Lampu Sorot):**

* Setiap kali terkena 1x *Flash* segel, lampu sorot di titik tersebut akan meledak karena lonjakan arus gaib, memaksa pemain berlari ke titik lampu sorot berikutnya sambil menghindari lemparan puing beton dan gangguan Tuyul.

* Pada tembakan **Flash Kamera Ketiga**, tubuh Genderuwo Arang terangkat ke udara, retak menjadi serpihan bara api, dan tersedot masuk ke dalam segel aplikasi `MataBatin.apk`!

---

### ADEGAN 6: Petunjuk Damar & Ending Cutscene Babak 1 (Menit 56:00 – 60:00 — TARGET UJI)

#### 1. Memeriksa Altar Kamar 404 dan mengambil petunjuk Damar

Setelah segel berhasil, Raka **tetap berada di Lantai 4**, di dekat Altar Arang yang sudah hancur. Ia lebih dulu mengambil **Kain Jaket Damar** dan menemukan **Kaset Pita Rekaman Tua (Item Dokumen #4)** di altar. Sementara itu, anomali yang menahan rem/panel lift sirna. Lift **baru dipakai jika pemain memilih turun** seusai memeriksa altar dan menonton epilog, bukan sebelum mengambil kain dan kaset.

* **Raka (Dialog Suara — napas tersengal):** *"Ini sobekan jaket yang dipakai Damar kemarin... Berarti cermin di Rumah Atmaja emang terhubung ke tempat-tempat ritual Mbah Suro."*
* Pemain memutar kaset pita tua di radio kecil dekat altar:
* **Suara Mbah Suro (Rekaman 1998):** *"Lima gerbang di lima penjuru kota. Rusun yang kelak terbakar, rumah sakit yang kelak ditinggalkan, pabrik gula yang kelak membisu, desa yang kelak terbakar, dan Pasar Setan tempat semuanya bermula. Empat segel dari empat penjuru akan membuka gerbang kelima bagi Penonton Terakhir..."*

#### 2. Pesan `Penonton_Terakhir` — epilog, bukan transisi paksa

Tiba-tiba terdengar suara tepuk tangan lambat dari speaker Smartphone Raka.

* Raka mengangkat HP-nya:
* **[LIVE | 👁️ 142.000 Penonton]**

**Rentetan Live Chat #4:**

* `@reza_arap_kw`: *"GILA BARUSAN ITU APAAN WOI MELEDAK DI KAMERA?!"*
* `@indah_cahyani`: *"Bang Raka tangan abang berdarah itu cepetan turun dari rusun!"*

* **Total Saweran Kumulatif:** **Rp78.200.000**. *Angka overlay naskah, bukan saldo yang dapat dibelanjakan; ledger belum lengkap (§5.3).*

* Layar HP kembali membeku merah-hitam seperti di akhir Prolog. Muncul video singkat berdurasi 5 detik di layar HP Raka: memperlihatkan **Damar yang sedang terikat di kursi roda berkarat di dalam sebuah lorong rumah sakit tua**, dengan mulut dilakban, berusaha berteriak ke arah kamera.

* Pesan dari **`Penonton_Terakhir`** muncul di bawah video:

> **Penonton_Terakhir:** *"Segel pertama terkumpul, tiga segel lokasi tersisa. Kau mulai terbiasa menggunakan aplikasi yang dioprek sahabatmu itu, Raka. Lihat baik-baik tempat Damar duduk sekarang. Besok malam pukul 23:59, bawa kameramu ke **RSUD Lama Sido Waras**. Jangan terlambat, karena suster jaga malam di sana sangat suka mengamputasi tamu yang telat."*

* **[KLARIFIKASI]** Video dan pesan hanya menuntaskan `P1_EPILOGUE`; **belum Fade to Black / belum `P1_COMPLETE`**. Raka menerima objektif baru: **`Tinggalkan Rusunawa. Opsional: kembali ke arsip Lantai 2.`** Pemain turun melalui tangga L4→L3, lalu **lift yang pulih langsung ke L1** (pintu servis sisi ruang pompa), menuju parkiran. Bila belum mengambil rahasia, pemain dapat memilih berhenti **di L2** dari panel lift untuk mengakses arsip, lalu memakai lift ke L1 atau menyeberang lewat K102 yang tetap terbuka dua arah tanpa AR.

#### 3. Transisi sebenarnya: kembali ke parkiran

Di **motor Raka di pelataran parkir awal** terdapat interaksi **`[E] Tinggalkan Rusunawa`**. Interaksi motor **hanya membuka panel UI sementara** (bukan event persisten `P1_LEAVE_RUSUN`); tampilkan **`Arsip Babak 1: 0/1 atau 1/1`** dengan pilihan **`Kembali Menjelajah`** atau **`Lanjutkan`**. Membatalkan panel tidak menulis state. Hanya pilihan kedua memicu `P1_COMPLETE` (segel/sasaran RSUD tersimpan atomik), lalu *Fade to Black* dengan audio detak jantung (*"BEEP... BEEP... BEEP..."*). Interaksi keluar tidak tersedia sebelum epilog dan **tidak boleh otomatis terjadi** saat pemutar kaset selesai.

---

## 2. Kontrak implementasi dan alur trigger

### 2.1 Kontrol dasar (KANON + KLARIFIKASI)

| Input | Konteks | Aksi | Catatan implementasi |
|---|---|---|---|
| `TAB` | Eksplorasi | Angkat/turunkan smartphone; akses tombol **MULAI LIVE** | [KLARIFIKASI] Jangan otomatis menonaktifkan HP dalam event paksa tanpa mengembalikan kontrol setelah event |
| `Q` | HP aktif | Toggle lensa AR | Tembok 102, papan tunggakan, Tuyul, aura keris, petunjuk rahasia |
| `Klik kiri` | HP/kamera aktif | Flash/Blitz | Perilaku berdasarkan konteks: usir Tuyul, stun stealth, segel boss hanya saat rentan |
| `F` | HP aktif | Matikan/nyalakan layar HP | Ketika gelap, Genderuwo tidak dapat mengendus cahaya layar; input prioritas terhadap kamera harus jelas |
| `S` | Kontrol karakter | Berjalan mundur | Mekanik khusus di koridor Kuntilanak Putih, bukan tombol khusus sihir |
| `E` **[GLOBAL/EXISTING]** | Dekat objek | Interaksi kontekstual (laci, gembok, mengambil barang, altar, pemotong rantai) | `IInteractable` + `InteractionInteractor`; tambah resolver proximity terarah khusus cutter/rantai, bukan controller kedua |
| `J` **[GLOBAL/EXISTING]** | Eksplorasi | Buka Jurnal Klenik | `GameAction.OpenJournal`; binding sudah tersedia, isi deduksi P1 belum ada |

**Aturan konteks [KLARIFIKASI]:** Satu klik tidak boleh sekaligus mengambil item dan menembak Flash. Saat menu gembok/jurnal/live dibuka, prioritas input milik UI; saat animasi wajib/pause, combat diproses menurut status adegan. AR adalah filter kamera HP, bukan portal teleport yang otomatis terbuka tanpa keterlibatan pemain.

### 2.2 Stage machine global (KLARIFIKASI)

```text
NOT_STARTED
  -> ARRIVAL [adegan 1]
  -> PUMP_ACCESS [temukan jalur ruang pompa]
  -> AR_PASSAGE_102 [tembok ilusi dilewati]
  -> MAILBOX_207 [kunci lift diambil]
  -> LIFT_TO_3 [lift macet L3]
  -> KUNTILANAK_CORRIDOR [pemotong + rantai]
  -> FLOOR4_INVESTIGATION [3 bukti + konfirmasi Jurnal + keris]
  -> ALTAR_READY [Raka berada dalam radius interaksi altar di K404; 3 bukti + konfirmasi Jurnal + keris; CP5 disimpan pada E valid]
  -> BOSS_ACTIVE [interaksi menancapkan keris]
  -> BOSS_DEFEATED [3 poin segel]
  -> EPILOGUE [kain Damar + kaset, video dan pesan; kontrol dikembalikan]
  -> RETURN_TO_EXIT [L4→L3→lift L1→parkir; boleh singgah L2 untuk arsip]
  -> PHASE1_COMPLETE [hanya P1_COMPLETE setelah konfirmasi final di motor; save + fade]
```

Stage adalah **turunan runtime** dari event, bukti, dan inventori yang persisten; bukan flag save terpisah yang dapat desinkron. Objective/checkpoint membaca stage turunan itu. Player boleh kembali ke Lantai 2 sesudah epilog untuk mengambil rahasia, dan stage tidak boleh mundur gara-gara memasuki ruangan lama. **`ALTAR_READY` adalah kondisi turunan sementara di depan altar, bukan event auto-save saat keris ditemukan. Panel keluar motor adalah UI transient; hanya `P1_COMPLETE` merupakan peristiwa akhir persisten.** `P1_TAPE_PLAY`, `P1_EPILOGUE`, dan `P1_COMPLETE` adalah tiga tahap berbeda; kaset bukan tombol akhir babak. Setiap transisi wajib idempoten sehingga *OnTriggerEnter*, save/reload, atau interaksi dobel tidak mencetak ulang hadiah.

### 2.3 Registry event inti (KANON dan KLARIFIKASI)

| ID event yang disarankan | Trigger | Prasyarat | Efek satu kali / status |
|---|---|---|---|
| `P1_ARRIVAL` | Spawn di parkir | Babak dimulai | Set lokasi/jam cerita 23:55, monolog, **grant baseline 1 powerbank sekali** jika tidak ada definisi inventori global yang disetujui |
| `P1_APP_PATCH` | Sequence awal | Belum diputar | Patch Jurnal Klenik 23:58, donasi `Penonton_Terakhir` Rp10 juta (event cerita) |
| `P1_GUARD_LOG` | Baca log satpam | Dalam jangkauan pos | Tambah Dokumen #1, ungkap akses pompa |
| `P1_START_LIVE` | Tombol MULAI LIVE | Smartphone dapat dipakai | Live ON, tampil 18.500 penonton, chat pembuka |
| `P1_SAWER_SIGNBOARD` | Foto AR papan nama berdarah | Live aktif, sebelum masuk pompa | Siluet 14 korban muncul 2 detik; jika valid, hadiah Rp1 juta **sekali**; bila melewati pompa sebelum selesai = `EXPIRED` |
| `P1_ENTER_PUMP` | Lewat ruang pompa | Jalur dapat diakses | Update objective; challenge papan nama yang belum selesai menjadi `EXPIRED` |
| `P1_REVEAL_102` | Bidik pintu dengan AR | AR aktif; sudut/LOS valid | Tampilkan portal semu dan collision passage yang konsisten |
| `P1_CROSS_102` | Melintasi pintu gaib | Passage revealed | Tandai akses L2 (tidak menutup pemain di tengah dinding) |
| `P1_TUYUL_L2` | Area koridor L2 | Encounter belum diselesaikan | Spawn Tuyul dan drain baterai selama encounter |
| `P1_MAILBOX_207` | Kode `644` benar | Kotak 207 belum dibuka | Tambah kunci lift + powerbank sekali |
| `P1_SECRET_CLUE` | Potret **lembar arsip tersembunyi di ceruk**, bukan papan enam kamar | AR aktif; player mencapai ceruk arsip | Ungkap nama Tari yang disilang tinta hitam pada indeks laci |
| `P1_SECRET_PICKUP` | Ambil negatif foto | Laci benar sudah terbuka, ID belum tersimpan | Tambah `SECRET_P1_NEGATIF_TARI`, autosave segera |
| `P1_LIFT_RIDE` | Pakai kunci lift | `hasLiftKey` | Pindah ke L3, lift berhenti mendadak; **CP3 mendapat lantai baterai minimum 20% satu kali dari sel darurat lift** dan menyimpannya sebagai snapshot retry |
| `P1_KUNTI_APPEAR` | Pemain mencapai tengah lorong | Encounter belum clear | Spawn di sisi lift yang dilewati, objective kamera mundur; **kamera WATCH_ONLY tetap tersedia saat baterai 0% selama encounter L3** |
| `P1_BOLT_CUTTER` | Selesai ambil cutter di ceruk | Jarak tangan ≤1,3m; **tidak perlu kamera diarahkan ke alat** | Item kunci **runtime sementara** selama encounter; mulai pursuit. **Persist pickup bersama `P1_CUT_CHAIN`**, jangan tulis snapshot parsial CP3 |
| `P1_CUT_CHAIN` | Selesaikan interaksi cutter 2 detik | Memiliki alat; **E berbasis radius tangan ≤1,6m** dari rantai, tanpa syarat arah kamera; hantu belum menangkap sebelum E | Buka tangga ke L4; **set L3 `RESOLVED` permanen pada playthrough**; semua re-entry aman |
| `P1_GENDERUWO_REVEAL` | Tengah koridor L4 | Belum muncul | Tembok 401 jebol, AI patroli mulai |
| `P1_EVIDENCE_CLAW` | Scan Kamar 402 | Validasi scan | Set bukti fisik |
| `P1_EVP_DISCOVERED` | Mendekati kamar mandi, mendengar static HP | EVP belum ada | Petunjuk ke recorder tanpa bergantung chat |
| `P1_EVIDENCE_EVP` | Rekaman selesai | Mikrofon HP in-game aktif | Set bukti suara; ambush kamar mandi hanya sekali |
| `P1_EVP_AMBUSH_DONE` | Boss selesai mencari di bilik | Bukti EVP selesai | Kembalikan AI ke patroli, jangan replay sesudah reload |
| `P1_EVIDENCE_AURA` | Scan tungku dapur | AR aktif | Set bukti aura |
| `P1_TAKE_KERIS` | Ambil keris | Reveal aura/akses objek valid | Tambah pusaka tunggal |
| `P1_JOURNAL_CONFIRMED` | Konfirmasi deduksi di Jurnal | Tiga ID bukti tepat lengkap | Receipt satu kali; syarat ritual global, bukan scan otomatis |
| `P1_ALTAR_INTERACT` | E valid di altar K404 | 3 bukti unik AND Jurnal terkonfirmasi AND keris dimiliki AND player dalam radius E; **CP5 dari posisi depan altar sukses di disk lebih dulu** | **Transaksi runtime nonpersisten**: keris dipasang sementara, dinding 404 jebol, kunci arena dan mulai boss; jangan overwrite CP5 saat fight |
| `P1_LIGHT_N/E/W` | Menyalakan halogen pada boss | Boss aktif, boss dalam beam, lampu tersedia | **Event transien percobaan boss**, buka vulnerable window **5 detik**; jangan simpan ke `completed_event_ids` |
| `P1_BLITZ_SUCCESS` | Flash valid mengenai kepala saat rentan | Lampu aktif / 5 detik belum habis | 1 poin segel unik **hanya di RAM** percobaan ini, lampu tersebut hancur; reset seluruh lampu jika retry CP5 |
| `P1_BOSS_WIN` | Total 3 poin segel unik | 3 keberhasilan pada 3 lampu | Commit CP6 atomik: segel, keris terpasang di altar, boss nonaktif, rem/panel lift normal. Kuntilanak L3 **sudah** diselesaikan oleh `P1_CUT_CHAIN`, bukan di event ini |
| `P1_DAMAR_CLOTH` | Ambil kain di altar | Boss kalah | Ambil story item dan VO Raka |
| `P1_TAPE_PLAY` | Putar kaset di radio K404 | Boss kalah, kain diambil dan kaset tersedia | Mainkan rekaman Mbah Suro (1998); **tidak** fade/transisi |
| `P1_EPILOGUE` | Kaset selesai + sequence altar diperiksa | `P1_DAMAR_CLOTH` dan `P1_TAPE_PLAY` | Chat 142.000, Rp78,2 juta, video Damar 5 detik, pesan RSUD; **kontrol dikembalikan** |
| `P1_EXIT_PANEL_OPEN` **(runtime/UI saja)** | E pada motor di parkiran | Boss kalah dan epilog selesai | Buka panel Arsip 0/1 atau 1/1; `Kembali Menjelajah` menutup panel, **tanpa persist, tanpa mengubah stage** |
| `P1_COMPLETE` **(satu-satunya event akhir persisten)** | Pilih `Lanjutkan` pada panel keluar | Epilog lengkap; panel sedang terbuka; belum complete | Save CP7 final atomik dan tujuan RSUD; **fade hanya setelah commit berhasil**, idempoten |

**Catatan [KLARIFIKASI]:** ID di atas adalah **usulan identifier teknis**, bukan ID yang terbukti sudah tersedia dalam repository. Tabel merupakan **registry event** (termasuk event runtime), **bukan daftar field yang semuanya wajib dipersist**: `P1_EXIT_PANEL_OPEN`, `P1_LIGHT_N/E/W`, `P1_BLITZ_SUCCESS`, dan `P1_ALTAR_INTERACT` tidak disimpan ke `completed_event_ids` sebelum kemenangan; `P1_BOSS_WIN` commit atomik adalah status permanennya. Cocokkan dengan sistem event/save yang sudah ada sebelum menambahkan baru.

### 2.4 Prasyarat, kondisi gagal, dan pemulihan

| Fitur | Gate positif | Saat gagal/terputus | Perilaku yang harus dicegah |
|---|---|---|---|
| Tembok Kamar 102 | AR membidik; jalur semu terbuka | Tetap bisa mengulang bidikan | Pemain terjebak saat AR mati di dalam geometri |
| Mailbox 207 | Kombinasi `644` | Salah kode beri feedback non-hukum | Softlock karena chat petunjuk terlewat |
| Lift barang | Kunci dari Mailbox 207 | Tampilkan kebutuhan kunci | Lift berfungsi tanpa kunci atau kunci hilang saat load |
| L3 Kuntilanak | Kamera aktif termasuk `WATCH_ONLY` baterai 0%, E dekat cutter/rantai berbasis radius, berjalan mundur | Jika pelanggaran >1,5 detik: Game Over → CP3 dengan min 20% | Kamera mati saat 0%, E butuh look-at rantai, atau cutter di sisi yang mengharuskan membelakangi hantu |
| Bukti klenik | 3 ID unik | Re-scan tidak menambah progres | Hitung 3 screenshot objek yang sama sebagai 3 bukti |
| Pintu 404 | Keris dimiliki **atau** `P1_BOSS_WIN` → kabut hilang dan pintu terbuka | Jika 3 bukti belum lengkap, boleh masuk/keluar; altar memberi petunjuk syarat | Pintu mengunci kembali saat keris dipasang/CP6, atau masuk otomatis memicu boss |
| Altar 404 | Keris + **semua** bukti + interaksi eksplisit | Beri feedback kekurangan di Jurnal | Battle dipicu saat keluar dari inspeksi / tanpa bukti |
| Halogen | Boss ada di sorotan | Jika Blitz meleset/timeout, perisai kembali, lampu dapat dipakai ulang | Lampu hancur saat gagal sehingga mustahil menang |
| Kemenangan | 3 lampu berbeda berhasil | Jika player mati, pulih di checkpoint yang disepakati | Poin segel ganda dari satu lampu |
| Ending | Kain, kaset, epilog selesai; pemain berinteraksi dengan motor di parkiran | Boleh turun via L3/L2/L1, mengambil arsip, dan membatalkan panel keluar | Fade prematur ketika tape berakhir; keluar dari K404 tanpa jalan pulang |

---

## 3. Perincian desain level dan navigasi

### 3.1 Denah logis (KANON + KLARIFIKASI)

```text
LUAR: Parkir + motor Damar/Raka -- Pos Satpam (Log #1)
   | (gerbang lobi dilas; jalur alternatif di belakang)
   v
RUANG POMPA -> L1 koridor lembap -> [Kamar 102 / tembok AR]
                                      |
                                      v
L2: koridor Tuyul -> papan tunggakan -> kotak surat 201..210 [207=644]
                       |                 | 
                       +-- ceruk arsip --+-- lift barang (kunci) 
                                           |
                                           v
L3: lift --- (sisi hantu) --- tengah trigger ---- ceruk bolt cutter ---- tangga L4
                                             (pemain mundur ke arah tangga)
                                           |
                                           v
L4: Koridor loop 4 sisi; K401 (spawn boss) -- K402 (cakaran+catatan Tari)
    | kamar mandi umum (EVP, bilik sembunyi)
    | dapur umum (tungku, keris)
    | K403 (jalur stealth / lokasi persembunyian)
    + K404 (altar, Fase B dinding utuh/pintu berkabut; Fase C dinding dijebol) ↔ arena halogen Utara, Timur, Barat
```

**Penting [KLARIFIKASI]:** Koridor memiliki empat **sisi** mengelilingi void (untuk angka terakhir puzzle 644). **Lift juga memiliki akses servis L1 di sisi ruang pompa yang baru dapat dipilih setelah boss disegel**; pintu gerbang utama lobi tetap dilas, sehingga pemain tidak bisa masuk dari luar lewat lift pada awal babak. **Pintu K404 tetap dapat dilewati setelah CP6** karena kondisi buka diturunkan dari `(keris dimiliki) OR (P1_BOSS_WIN)`: memindahkan keris ke altar tidak boleh mengunci ulang pintu atau membangun kembali dinding yang dijebol. Kamar 404 tertutup dinding utuh pada Fase B dan baru berjebol pada `P1_ALTAR_INTERACT` di Fase C; arena menyambung dengan loop koridor. Konstruksi geometri perlu konsisten: jangan buat boss terjebak pada dead end atau membuat jalur pemain tidak mengitari tiga lampu. Empat ruangan yang disebut pada urutan area tidak membatasi keberadaan kamar mandi/dapur; area wajib harus dapat dicapai tanpa menembus arena sebelum waktunya.

### 3.2 Lantai 1—2: dua puzzle dan jalur rahasia

**Kamar 102 / dinding ilusi (KANON).** Normalnya terlihat tembok semen; saat AR aktif, tembok menghilang dalam tampilan kamera dan terlihat pintu kayu/tangga darurat. Pemain menembus ilusi menuju L2. [KLARIFIKASI] Gunakan representasi geometri/collider yang menjamin karakter dapat melewati pintu meskipun HP digerakkan atau AR berganti di tengah transisi. **Setelah `P1_CROSS_102` tersimpan, bukaan fisik dan collider koridor tetap terbuka dua arah, termasuk saat pulang pada baterai 0%.** Hindari mengubah collider setiap frame berdasarkan raycast sehingga karakter bisa tersangkut.

**Kotak Surat 207 (KANON).** Dokumen pengelola menyebut formula kode: jumlah kamar tunggakan **6** (201, 202, 204, 206, 208, 210), lantai ritual **4**, jumlah sisi koridor pada denah per lantai **4** (bukan total empat lantai). Masukkan **644**, hadiah **Kunci Kontak Lift Barang** dan **1x Powerbank Cadangan**.

**Negatif Foto Tari (KANON; clue diperbaiki).** Lokasi: ruang arsip pengelola di ceruk belakang kotak surat L2. AR pada **lembar arsip tersembunyi** (bukan papan status enam kamar) memunculkan petunjuk: **"Foto yang tidak terbakar disimpan di belakang nama yang dicoret."** Cari **nama Tari yang disilang tinta hitam** di label daftar arsip, cocokkan laci bernama Tari, buka panel, ambil negatif 14 penghuni hidup dengan tulisan **"Kami pernah punya nama"**. Tidak memakai kode 644, tidak menjadi bukti klenik boss, bukan syarat lift atau transisi. Persisten walau reload/kematian; penghitung `secret_items` memakai ID unik.

**[USUL BARU] Petunjuk non-chat:** dinding K102 punya anomali debu dan sketsa denah satpam; angka `4` ritual muncul dari log lokal dan satu penanda pada denah rusun; foto Tari punya satu detail visual kecil yang mengikat dokumentasi Tari, bukan panah HUD terang. Hindari spoiler otomatis di awal; gunakan tingkatan hint hanya setelah pemain lama buntu (waktu final TBD).

### 3.3 Lantai 3: tata letak Kuntilanak agar masuk akal

- **[KANON]** Koridor lurus **40 meter**. Pemain keluar dari lift, maju ke tengah; **tangisan lirih terdengar lebih dulu** dari ujung koridor. Pada pemicu tengah, sosok bergaun hangus muncul **di belakang pemain** dan Raka refleks mengangkat kamera. Tangga ke L4 ada pada **ujung seberang**. Saat kamera diarahkan ke sosok yang ada di belakang, tujuan ada **di belakang karakter**.
- **[KANON]** Setelah alat dipungut, sosok itu mendekat; pemain mundur `S` dengan kamera HP terus mengarah ke sosok itu. Berpaling atau menurunkan kamera lebih dari **1,5 detik** menyebabkan instant-warp/Game Over. Foto jarak dekat (≤5 m) mengejutkannya **±1 detik** sebagai jendela mundur. Bolt cutter berada **di ceruk Ruang Kebersihan samping jalur menuju tangga**, bukan di balik sosok itu.
- **[KLARIFIKASI]** Buat akses ceruk dan jangkauan `E` sebagai **interaksi radius tangan ≤1,3 m ke cutter dan ≤1,6 m ke rantai**, tanpa mewajibkan reticle/kamera menghadap item; gerakan pemain tetap menjaga kamera menghadap hantu. Pickup tidak boleh membutuhkan menutup HP. Bila animasi pemotongan rantai memaksa sudut kamera berpindah, *suspend* evaluasi 1,5 detik selama interaksi wajib, atau gunakan framing kamera yang tetap menampilkan hantu. Di luar animasi wajib, aturan keras tetap berlaku.
- **[KLARIFIKASI]** Pemotong rantai tidak boleh menghasilkan kondisi “hantu dekat pintu, durasi animasi lebih lama dari sisa waktu aman”. Kontrak rinci: `CHAIN_INTERACT` membekukan gerak dan damage hantu tepat selama animasi wajib 2 detik, setelah pemain berhasil menekan E sebelum jarak tangkap (§3.3A). Pastikan jalur mundur tidak memiliki meja/objek yang menghalangi kontrol hingga mustahil dilalui.
- **[USUL BARU]** Telegraph kejadian sebelum instakill: suara tangis yang makin lirih saat mendekat, lampu makin tidak stabil, frame kamera berembun/siluet. Berikan checkpoint tepat sebelum memulai encounter agar retry tidak mengulang puzzle 644.

### 3.3A Koreksi blocking Kuntilanak (BASELINE UJI)

Koridor panjang **40 m** memakai koordinat lokal *blockout*: lift `x=0`, trigger kemunculan `x=19`, sosok **muncul ±8 m di belakang pemain lalu statis** sampai cutter diambil, ceruk bolt cutter `x=22`, dan pintu tangga `x=40`. Ceruk cutter ada di sisi jalur ke tangga (tidak melewati tubuh sosok itu). Dari cutter ke rantai ada **18 m** gerak mundur. Durasi target **5 menit termasuk pengenalan, lift, inspeksi, alat, dan escape**; pengejaran murni ±10 detik, bukan 5 menit pemain dipaksa mundur.

```text
DORMANT → HEARD (tangis, tanpa visual) → REVEAL (di belakang pemain + kamera refleks)
        → MANIFEST/WATCHED_IDLE → CUTTER_PICKUP → PURSUIT → CHAIN_INTERACT → RESOLVED
                                   ↳ UNWATCHED >1,5 s → WARP_ATTACK
                                   ↳ COLLISION_CLOSE → WARP_ATTACK (dijeda stun foto ±1 dtk)
                                   ↳ PHOTO_≤5m → STUNNED_1s → PURSUIT
```

**Timer 1,5 detik hanya berjalan setelah animasi pickup cutter selesai**. Selama `WATCHED_IDLE`, Kuntilanak **dipakukan di x=9 (movement speed=0)**; pemain boleh menginspeksi ceruk. Pickup alat mengambil waktu interaksi tanpa memulai pursuit sampai animasinya selesai, sehingga waktu diam saat pickup tidak memakan buffer. Pada `PURSUIT`, kamera harus mengawasi; berpaling >1,5 detik memicu Game Over. Jika hantu benar-benar mencapai Raka, **kontak tetap fatal meskipun sedang ditatap**, sehingga pemain harus mundur, bukan hanya menatap diam. Telegraphed flicker dan jeda tangis boleh memvariasikan tekanan, tetapi teleport acak yang tiba-tiba muncul di belakang pemain tidak dianjurkan.

`TAB/F` selama pursuit **diabaikan dengan umpan balik**, bukan menurunkan kamera secara tidak sengaja. Menekan `E` saat rantai dalam **jangkauan ≤1,6 m (abaikan arah pandang kamera, tetapi tolak bila pemain dipisahkan tembok tertutup)** dan cutter dimiliki memulai **`CHAIN_INTERACT` selama tepat 2,0 detik** (tidak bisa di-*cancel* untuk mengulang grace gratis). Jika hantu **sudah** di radius tangkap sebelum `E`, Game Over tetap diproses; jika belum, **AI movement, collision damage, dan timer `UNWATCHED` dibekukan persis selama animasi wajib 2,0 detik**, sedangkan input arah kamera terkunci ke hantu. Setelah animasi selesai, rantai putus **dalam frame transisi yang sama** dan `P1_CUT_CHAIN` mengunci `RESOLVED` permanen; hantu tidak dapat menyerang saat kamera dibalik untuk masuk tangga. `P1_BOSS_WIN` tidak boleh menonaktifkan Kuntilanak lagi. Retry CP3 memulihkan posisi x=9, alat di ceruk, rantai utuh, dan chase dengan deterministik. **Pickup cutter dan chain-cut satu transaksi checkpoint**: bila mati sebelum rantai selesai, muat CP3 dengan cutter masih di ceruk; setelah `P1_CUT_CHAIN`, simpan status alat diperoleh dan rantai terpotong bersama-sama agar tidak ada keadaan cutter hilang tetapi pursuit belum pernah bisa dimulai.

**Perhitungan baseline yang wajib diuji (bukan jaminan kelulusan):** Controller existing hanya 1,7m/s dan tidak membedakan maju/mundur; angka 1,8 di model berikut belum diterapkan. Pada 1,7 konstan, 18m memerlukan 10,59s dan margin di x40 sekitar 1,83s. Akselerasi, radius E dan collision harus ikut uji nyata; jangan mengubah kecepatan global P0 diam-diam.  `kunti.pursuitMps = 2,4`, `player.backward = 1,8`, berangkat dari `x_kunti=9`, `x_player=22`, menuju `x_door=40`. Waktu pemain = `(40−22)/1,8 = 10,0 s`; posisi hantu saat itu = `9+2,4×10 = 33 m`; celah = `7 m`. Dengan radius tangkap `1,2 m`, waktu reaksi maksimum sebelum tombol E ditekan ≈ `(7−1,2)/2,4 = 2,42 s` **jika tak ada hambatan, kesalahan navigasi, atau keterlambatan pickup**. Animasi rantai tidak memakai jatah 2,42s karena AI dibekukan saat `CHAIN_INTERACT`. Tambah logging waktu dan playtest beberapa skill level; jika tester gagal tanpa kesalahan nyata, geser posisi trigger/ceruk atau turunkan kecepatan, jangan hanya menambah instakill grace secara terselubung.


**[BASELINE L3 — D01/D06 BELUM DIRATIFIKASI]** Pada `P1_LIFT_RIDE`, panel daya darurat lift memberi **satu kali top-up hingga minimum 20%**: `batteryAfter=max(batteryBefore,20)`. Peristiwa ini bagian dari **snapshot CP3** dan tidak berulang ketika pemain bolak-balik atau menekan tombol lift lagi; retry dari CP3 mengembalikan baterai snapshot, bukan menambah 20% setiap kematian. Jika pemain menunggu begitu lama hingga energi benar-benar 0%, saat `P1_KUNTI_APPEAR` aktif sampai `P1_CUT_CHAIN`, `KuntiWatchOnlyCamera` otomatis menyediakan **feed kamera monokrom untuk mengawasi hantu tanpa biaya baterai dan tanpa menambah baterai**. Dalam mode ini `Q`/AR, EVP, dan Flash **tetap nonaktif** dan UI menyatakan `DAYA KRITIS — PENGAWASAN DARURAT`. Mode kamera khusus hanya aktif di koridor L3 selama encounter; **bukan** sumber energi yang dapat dipakai pada puzzle atau boss, dan mati permanen setelah status `RESOLVED`. Frustum/timer pengawasan tetap berlaku. Unit test wajib mulai dari 0% maupun 1%.

**[KLARIFIKASI — satu sumber kebenaran]** Seluruh pemeriksaan status Kuntilanak membaca `P1_CUT_CHAIN` / turunan `RESOLVED`. Hantu tidak kembali aktif saat pemain turun sebelum maupun setelah boss; `P1_BOSS_WIN` hanya memperbaiki lift. Ini sengaja menghapus desain lama yang membuat kuntilanak baru aman setelah boss.

### 3.4 Lantai 4: urutan investigasi tidak boleh keras tanpa alasan

**[KANON]** Tiga bukti dan satu pusaka harus diperoleh: bukti fisik di **pintu besi K402**, EVP di **kamar mandi umum**, aura di **tungku dapur umum**; keris bisa diambil dari tungku. Genderuwo keluar dari K401 dan berpatroli. Kamar 403 hadir sebagai **ruang terbuka dari awal Fase B**, memiliki lemari persembunyian dan satu dinamo manual di sisi dalam, tanpa kunci baru. Pintu dapat ditutup sementara untuk mengurangi deteksi suara tetapi tak boleh terkunci permanen atau menghalangi keluar.

**[KLARIFIKASI]** Ketiga bukti memakai **ID terpisah**, bisa dicapai tanpa urutan wajib kecuali jika ada alasan cerita eksplisit. Kunci altar hanya setelah semua bukti, konfirmasi Jurnal dan keris. Membaca Catatan Tari bukan pengganti bukti cakaran; merekam EVP baru valid setelah rekaman selesai; memindai aura dan mengambil keris adalah **dua flag berbeda**.

**[USUL BARU]** Agar tidak terasa hanya ceklis, tiap bukti menawarkan sedikit konsekuensi: suara langkah memancing penjaga saat memotret cakaran, EVP menyebabkan sergapan kamar mandi sesuai naskah, tungku memunculkan motif ritual/fisik korban. Pengambilan tanpa akurasi kamera atau POV yang benar sebaiknya tidak menghasilkan status “scan sukses”.

### 3.4A Isi konkret alokasi investigasi 21 menit L4 (BASELINE UJI)

Ini **budget waktu per beat berdasarkan jalur contoh**, bukan kunci progres linear: pemain tetap bebas memilih urutan tiga bukti. Perpindahan lima menit dari Adegan 3 diisi oleh **pengamatan, penghindaran dan konsekuensi investigasi**, bukan menunggu timer kosong.

| Menit global | Beat level yang nyata | Interaksi/tekanan yang terukur |
|---|---|---|
| 27–30 | Koridor 401: kamera membingkai dinding retak; kemunculan Genderuwo; player mencari dua jalur menghindar | Tutorial *screen off*, landmark empat sisi, satu rute patrol-avoid yang berhasil |
| 30–34 | K402: mengatur sudut pemindaian cakaran dan membaca Catatan Tari; langkah berat membuat pemain berlindung | Bukti fisik + clue keris dan halogen, bukan fetch item tambahan |
| 34–38 | K403 sebagai ruang sela: pintu yang bisa ditutup, lemari dan dinamo; kesempatan mengelola HP dan mempelajari cover (Sanity P1 tidak aktif) | Pemain mempelajari *noise/line-of-sight* dan memulihkan resource bila perlu; **tidak ada gerbang baru** |
| 38–43 | Kamar mandi: dengar statis, aktifkan EVP, rekam, reaksi 3,5s dan berlindung di bilik | Bukti EVP + scripted ambush sekali; tunggu patroli menjauh berdasar AI, bukan timer tetap yang tidak bisa dipercepat |
| 43–47 | Dapur: temukan tungku di balik area berasap, pindai aura dan ambil keris; rute pulang melewati patroli | Bukti AR + pusaka yang berbeda flag, pengambilan aman meski item physics bergeser |
| 47–48 | K404: buka pintu yang kabutnya hilang karena keris, tinjau Jurnal tiga bukti, capai altar | Validasi gate tanpa memulai boss sampai E valid |

**Batas produksi:** Jangan menambahkan kunci/fetch quest baru untuk memaksa durasi tabel. Rekam waktu tiap beat dalam PlayMode/build Windows; jika pemain mahir menyelesaikan dalam 13–16 menit, itu bukan bug. Sasaran 21 menit dapat bergeser berdasarkan atmosfer, stealth dan playtest; hindari memalsukan pacing dengan pintu terkunci otomatis.

### 3.5 Lantai 4: keselamatan navigasi dan pembacaan ruang (USUL BARU)

- Landmark visual untuk empat sisi koridor: panel halogen mati, bercak jelaga berbeda, nomor kamar besar yang aus, bekas genangan, dan warna cahaya antar sayap.
- Dua jalur menghindar dari patroli menuju kamar mandi/dapur sehingga stealth tidak menjadi jalur satu-satunya yang mudah terjebak.
- Tempat sembunyi punya batas masuk/interaksi dan state yang eksplisit; kamera serta collision AI tidak bisa menembus lemari tertutup secara visual.
- Area kunci tidak menyembunyikan item wajib di balik objek dinamis/physics yang bisa hilang akibat load atau ledakan boss.

---

## 4. Sistem ancaman dan state machine AI

### 4.1 Tuyul: baterai dan resource pressure

**[KANON]** Muncul di L2 dan sesekali saat boss. Hanya terlihat via AR; audio cekikikan `"Hihihi..."` dan langkah kecil di genangan. Menguras baterai **2% per detik**, ancaman window **10 detik**. Satu Flash berhasil mengusir. Jika gagal, mencuri **1 powerbank cadangan** dan lari menembus tembok; tidak membunuh pemain.

```text
DORMANT -> TEASING_AUDIO -> DRAINING [2%/detik + timer <=10 s]
                         |              |
                         |              +--> FLASH_HIT --> FLEEING --> DESPAWN
                         |              +--> TIMEOUT --> TRY_STEAL_POWERBANK --> FLEEING
                         +--> DISABLED (area selesai)
```

**[KLARIFIKASI — celah pada naskah]:** Encounter pertama Tuyul ditempatkan **sebelum** Mailbox 207, sementara powerbank hadiah baru didapat **sesudah** kode 644. Karena itu pencurian hanya boleh terjadi jika `powerbankCount > 0` (misalnya dari inventori lintas babak); bila kosong, tidak boleh ada jumlah inventori negatif atau softlock. Simpan satu penalti yang adil (drain yang sudah terjadi sudah cukup), lalu biarkan pemain menemukan powerbank pasti di 207. Jika baterai HP terlalu rendah untuk scan kode 644, gunakan **dinamo aman** sesuai §8.0. Jangan diam-diam mengubah urutan cerita untuk memperbaiki bug.

**[USUL BARU]** Audio 3D dan partikel AR memperlihatkan arah ancaman; spawn boss dibatasi agar tidak menutupi window 5 detik yang wajib diselesaikan. Drain berhenti ketika state keluar atau setelah game pause/cutscene.

### 4.2 Kuntilanak Putih: pengawasan dan gerakan mundur

**[KANON]** Tangisan melemah saat makin dekat; bergaun hangus; tidak sama dengan Kuntilanak Merah Lastri di Babak 2. Tidak ada peringatan nama/entitas sebelum momen terungkap. View kamera HP + mundur; lebih dari **1,5 detik** membelakangi atau menurunkan HP ketika pengejaran sudah dimulai = Game Over. Foto jarak dekat (≤5 m) mengejutkan **±1 detik** (gerak, timer, dan tangkapan berhenti). [KLARIFIKASI] Pengejaran dimulai setelah cutter diambil (§3.3A).

```text
WAIT_IN_LIFT_SIDE -> MANIFEST_AFTER_MIDPOINT -> WATCHED_IDLE (x=9 static)
                                             |
                                             +-- CUTTER_PICKUP_FINISHED --> PURSUIT (2.4m/s)
                                                        |-- UNWATCHED >1.5s --> WARP_ATTACK
                                                        |-- DISTANCE <= 1.2m BEFORE E --> WARP_ATTACK
                                                        +-- E_IN_RANGE --> CHAIN_INTERACT (2s freeze AI/timer)
                                                                               |-- COMPLETED --> RESOLVED (persistent)
```

**[KLARIFIKASI]** Jangan evaluasi ancaman dari apakah hantu benar-benar tersorot raycast satu titik yang mudah gagal karena pilar kecil. Ukur kondisi kamera aktif, sudut pandang/frustum, jarak, dan *line-of-sight* secara toleran; bagian terhalang sesaat oleh objek yang harus dilalui pemain tidak boleh otomatis memicu kematian. Hentikan timer bila animasi wajib memaksa sudut pandang bergeser. **Pickup cutter hanya disimpan dalam RAM sampai rantai berhasil dipotong**; jika mati sebelum sukses, pemuatan CP3 menaruh kembali cutter di ceruk. Bila `P1_CUT_CHAIN` berhasil, status pickup dan rantai terbuka dikomit atomik.

### 4.3 Genderuwo Arang: stealth (fase B)

**[KANON]** Tinggi sekitar **2,5 m**, berbulu/hangus bak arang, mata merah, suara `"DUG... DUG... DUG..."`. Muncul setelah dinding K401 jebol. Kebal terhadap **damage penyegelan** di fase investigasi; tertarik pada cahaya layar. Pemain mematikan layar `F`, lalu sembunyi di lemari, bawah meja, atau bilik WC. Flash darurat hanya men-*stun* **3 detik** dengan biaya **15% baterai**, bukan damage segel. Sesudah EVP selesai, boss mendobrak pintu kamar mandi, lalu memeriksa bilik; menyalakan HP ketika diperiksa menyebabkan pintu dihancurkan.

```text
DORMANT -> SPAWN_REVEAL -> PATROL <-> INVESTIGATE_LIGHT_OR_SOUND
                            |                        |
                            |                        +--> CHASE -> ATTACK / SEARCH_LAST_SEEN
                            |                        +--> PLAYER_HIDDEN_SCREEN_OFF -> SEARCH -> PATROL
                            +--> EMERGENCY_FLASH_HIT -> STUNNED_3_SECONDS -> RECOVER -> PATROL/CHASE
EVP_FINISHED -> SCRIPTED_BATHROOM_AMBUSH -> SEARCH_STALLS -> RETURN_TO_GENERAL_AI
ALTAR_STARTED -> HANDOFF_TO_BOSS_PHASE
```

**[KLARIFIKASI]** Perlu prioritas AI saat event EVP, agar patroli umum tidak menghalangi boss menyelesaikan sergapan. HP dinonaktifkan berarti tidak memancarkan *light stimulus*; bukan otomatis *invisibility* jika pemain bergerak di depan wajah boss. Flash stun 15% **khusus defensif stealth**. Untuk sumber naskah awal biaya lainnya belum tertulis; r1 memakai default global: foto 1%, Blitz 15% dan cooldown 6s, powerbank besar +80. Jangan mengambil biaya 4% atau refill 50% dari baseline lama.

**[USUL BARU]** Gunakan sensor stimulus terpisah untuk *screen light*, kebisingan pemain, dan LOS; beri *last known position* supaya perilaku lebih alami. Spawn/respawn jangan menempatkan AI tepat di atas pemain. Jika pemain terjebak dalam satu bilik tanpa celah aksi sesudah event EVP, boss harus keluar dan membuka kembali eksplorasi setelah investigasi selesai.

### 4.3A Petunjuk EVP dan ambush yang bisa dihindari (KLARIFIKASI)

Saat mendekati kamar mandi, speaker HP menerima suara radio statis dan icon EVP (tersedia juga via Jurnal). Pemain menekan rekam sampai selesai; lalu **dobrakan pintu luar terjadi segera** seperti naskah, tetapi Genderuwo **baru mencapai bilik setelah 3,5 detik** [BASELINE UJI], cukup untuk menutup HP `F` dan bersembunyi. Boss punya waktu pencarian maksimum; setelah event dia pergi dan jalur investigasi terbuka lagi. Bukti tersimpan terpisah dari flag `P1_EVP_AMBUSH_DONE`. Bila load dilakukan di antara bukti EVP didapat dan search selesai, pulihkan dari checkpoint aman di luar bilik lalu jalankan ambush secara deterministik (tanpa menggandakan bukti); setelah `P1_EVP_AMBUSH_DONE`, ambush tidak terulang.


### 4.4 Genderuwo Arang: pertarungan (fase C)

**[KANON]** Arena: Kamar 404 menyatu dengan loop L4. Shield kabut hitam aktif. Boss melempar puing beton dan ada Tuyul pengganggu. **3 halogen konstruksi:** Utara, Timur, Barat. Masing-masing harus dipakai sekali **sukses**. Saat boss berada di beam, tuas menyalakan lampu → shield pecah **5 detik** → target segel merah di kepala → **1 Flash tepat sasaran** → lampu titik tersebut meledak. Tiga titik sukses = boss disegel ke aplikasi.

```text
BOSS_ACTIVE / SHIELDED
    -> LURE_INTO_BEAM
    -> LIGHT_SWITCH_ON (one of N,E,W unused)
    -> VULNERABLE_5_SECONDS
       |-> FLASH_HIT_HEAD -> MARK_LIGHT_COMPLETED -> DESTROY_THAT_LIGHT -> RE-SHIELD or WIN
       |-> MISS / TIMEOUT -> RE-SHIELD -> KEEP_LIGHT_USABLE -> RETRY
    -> AFTER_3_DISTINCT_LIGHT_SUCCESSES -> VICTORY
```

**Invariant wajib (KLARIFIKASI):**

- Hanya lampu dengan status `available` yang dapat memulai kesempatan; lampu `completed` tidak bisa dihitung lagi.
- `vulnerableUntil` dihitung dari waktu gameplay yang valid; saat pause/cutscene instruksional tidak boleh diam-diam berkurang.
- Blitz dianggap sukses ketika **boss vulnerable**, target kepala valid, lampu pemicu masih terkait window itu, dan sukses itu belum tercatat. Klik berkali-kali pada window yang sama tidak memberi poin tambahan.
- Jika Flash meleset atau window berakhir, **lampu tetap utuh** dan dapat digunakan lagi. Kekalahan tidak boleh disebabkan oleh semua lampu hancur tanpa tiga poin.
- Setelah pukulan ketiga: matikan damage/AI proyektil; simpan state boss kalah; tampilkan efek tubuh retak/tersegel; lift kembali aktif, bukan baru aktif saat ending fade.

**[USUL BARU]** Tetapkan jeda singkat setelah satu sukses untuk telegraph perpindahan fase; hindari melepas Tuyul tepat selama exposure 5 detik jika membuat aksi wajib tak mungkin. Semua lemparan puing harus mempunyai *anticipation* audio/visual. Untuk performa, pecahan lampu memakai pool/VFX low-cost dan tidak perlu ratusan rigidbody aktif.

---

### 4.5 Denah dan AI arena boss (BASELINE UJI)

Denah konseptual, **belum denah final**: halogen **Utara** dekat sayap K401 (spawn boss), **Timur** dekat sayap K402 (cakaran), **Barat** dekat dapur umum; K403 ruang terbuka bagi *stealth* Fase B dengan lemari dan satu dinamo, **tidak** ada dinamo eksplorasi dalam arena boss; default global tetap memerlukan recovery daya arena saat recovery AI. K404 mempunyai dinding penuh sampai event `P1_ALTAR_INTERACT`, kemudian dua panel dinding jebol dan menghubungkan altar ke dua ruas loop. Tiap switch dapat dicapai tanpa masuk ke collider boss. Jalur pemain mengitari tiga lampu dari setidaknya dua arah.

Mekanik `LURE`: boss mengejar *last known position* pemain, masuk volume collider sorot dengan radius baseline 2,5m, sakelar hanya aktif jika `bossInsideBeam && lightAvailable`. Saat perisai pecah, boss memasuki animasi kesakitan `EXPOSED` selama **5 detik**, **tidak** menyerang atau melempar puing; hitbox kepala stabil (jarak Flash baseline 8m, toleransi aim ±20° dan LOS). Tepat satu Flash valid menandai lampu itu selesai; miss/timeout me-reset shield **tanpa menghancurkan lampu**, dan sakelar boleh dipakai ulang. Tuyul tidak spawn pada window 5 detik. Ledakan titik hanya setelah sukses. Pukul ketiga mengakhiri AI dan memberi segel atomik.

**[PENYELARASAN GLOBAL r1]** Default implementasi: Blitz segel **15% baterai / cooldown 6s**, dengan **titik daya darurat arena** yang dapat digunakan saat boss recovery, sesuai `Rancangan.MD`. Isi ulang tidak mengonsumsi sumber cahaya sebelum hit sukses. **Alternatif D01, belum disetujui:** Blitz segel berenergi halogen pada 0%. Jangan mengaktifkan alternatif ini atau meniadakan charger default sebelum keputusan dicatat. Angka refill/durasi titik daya arena masih perlu uji; akses pada 0% wajib terbukti.


## 5. Bukti, Jurnal Klenik, inventori dan item

### 5.1 Master item dan dokumen (KANON)

| Jenis | Item / ID logis | Lokasi | Cara dapat | Fungsi |
|---|---|---|---|---|
| Dokumen #1 | Buku Laporan Piket Satpam (14 Mei 2004) | Pos satpam | Baca/interaksi | Riwayat Tari 404, korslet, minyak tanah, jalur ruang pompa |
| Dokumen #2 | Surat Pengelola Rusunawa | Deretan kotak surat L2 | Baca | Rumus kode 207 (6–4–4) |
| Dokumen #3 | Catatan Harian Tari | Samping pintu K402 | Baca | Keris awalnya disimpan di bawah ubin 404 lalu dipindah ke dapur, serta kelemahan terhadap halogen |
| Dokumen #4 | Kaset Pita Mbah Suro (rekaman 1998) | Altar 404 | Ambil dan putar pasca-boss | Lore lima gerbang; arahan penutup |
| Key item | Kunci Kontak Lift Barang | Mailbox 207 | Kode `644` | Memanggil lift ke L3 |
| Consumable | 1x Powerbank Cadangan | Mailbox 207 | Kode `644` | Cadangan baterai; bisa dicuri Tuyul |
| Key item | Pemotong Besi / bolt cutter | Ceruk Ruang Kebersihan L3 | Ambil saat encounter | Potong rantai tangga L4 |
| Bukti #1 | Cakaran lima jari | Pintu K402 L4 | Scan kamera | Flag fisik Jurnal |
| Bukti #2 | Rekaman suara EVP | Kamar mandi umum L4 | Rekam sampai selesai | Flag suara Jurnal; sergapan boss |
| Bukti #3 | Aura kematian/keris | Tungku dapur L4 | Scan via AR | Flag aura Jurnal |
| Pusaka | Pecahan Keris Penumbal | Tungku dapur L4 | Ambil setelah ditemukan | Hilangkan kabut pintu 404; ritual altar |
| Story item | Kain Jaket Damar | Altar 404 | Ambil setelah boss | Kaitan Damar dengan ritual/cermin |
| Rahasia | Negatif Foto Tari — `SECRET_P1_NEGATIF_TARI` | Arsip belakang mailbox L2 | AR + label Tari + pickup | Arsip Saksi (1 dari 5 total) |

**[KLARIFIKASI]** `Bukti #3` ialah scan aura; pusaka fisik merupakan item terpisah walau satu lokasi. `secret_items` menggunakan **list ID unik pada save / set runtime turunan** agar pengambilan ulang tidak menaikkan progres. Empat dokumen mendukung lore dan clue; jangan gunakan jumlah dokumen sebagai pengganti tiga bukti.

### 5.2 Journal data contract

**[KANON]** Jurnal menampilkan tiga kotak: `[ ] Jejak Fisik`, `[ ] Suara Gaib (EVP)`, `[ ] Aura Kematian (AR)` dan status collectible `0/1` atau `1/1` pada babak ini. Arsip Saksi global adalah `N/5` dari **jumlah aktual ID unik yang sudah dimiliki**, bukan nomor babak.

**[USUL BARU]** Jurnal memiliki empat tab konseptual: **Bukti**, **Dokumen**, **Arsip Saksi**, **Petunjuk Objektif**. Pada akhir tiga bukti, tampilkan kesimpulan kelemahan: halogen memecah perisai, Flash ke kepala menyegel. Pemain lalu menekan **Konfirmasi identitas** untuk mencatat `P1_JOURNAL_CONFIRMED` sesuai aturan global; jangan otomatis menyamakan tiga scan dengan konfirmasi. Jangan otomatis menampilkan solusi kode 644 dari awal.

### 5.3 Status ekonomi dan live stream

**[KANON]** Raka melihat pembayaran kos dan deposit operasi ibu dari Rp25.450.000 hasil malam sebelumnya. Pembuka: donasi Rp10.000.000 dari `Penonton_Terakhir`. Ada **misi opsional** donasi Rp1.000.000 (`@sultan_sawer`), donasi Rp3.000.000 di L3, angka penonton awal **18.500**, akhir **142.000**, dan teks final **Total Saweran Kumulatif Rp78.200.000**.

**[KLARIFIKASI]** Bedakan secara teknis: (a) **uang tersedia/inventori**; (b) **total saweran kumulatif untuk UI cerita**; (c) **tantangan donasi yang ditawarkan**; (d) **reward yang benar-benar diperoleh setelah diselesaikan**. Jangan otomatis menganggap misi saweran Rp1 juta sudah cair. Angka Rp78,2 juta adalah fakta naskah *cutscene*, **belum disertai ledger lengkap** yang merekonsiliasi donasi yang ditampilkan. Implementasi harus memakai data sumber uang yang konsisten atau menetapkannya sebagai angka naratif eksplisit sampai desain ekonomi final diketahui; jangan menambahkan saldo secara ganda saat reload.

**[KANON]** Tidak ada *paywall*; tidak perlu donasi untuk akses, bukti, powerbank wajib, atau boss. **[USUL BARU, DI LUAR MVP]** Saweran cosmetic/bonus sumber daya bukan kebutuhan P1 dan tidak boleh ditambahkan otomatis. Saweran naskah tetap memberi reward yang disetujui; tetapi jangan membuat 60 menit jalur wajib bergantung pada audiens atau koneksi internet. Live chat dapat berupa skrip lokal deterministic, bukan jaringan sungguhan.

---

### 5.4 Challenge saweran papan nama (KLARIFIKASI + BASELINE UJI)

`P1_SAWER_SIGNBOARD` aktif sesudah `P1_START_LIVE` dan berakhir pada `P1_ENTER_PUMP`. Foto AR valid pada papan nama berdarah di parkir memunculkan siluet **14 penghuni** selama 2 detik [BASELINE UJI]; sekali sukses membuat donor membayar Rp1.000.000. Jika terlewat, tandai `EXPIRED` tanpa akses item/jalur utama ikut terhalang. Simpan satu status enum `OFFERED|SUCCEEDED|EXPIRED`, bukan tiga boolean yang dapat sama-sama true; ledger menggunakan ID transaksi unik.


## 6. Sistem penyimpanan dan *campaign state*

### 6.1 Prinsip penyimpanan (KANON + KLARIFIKASI)

- Tiga bukti unik, pusaka, status boss, segel lokasi, dokumen, inventori, uang tersedia, Sanity, upgrade, dan Arsip Saksi **harus konsisten setelah reload**.
- Item `SECRET_P1_NEGATIF_TARI` ditambah **sekali** ke daftar ID global `secret_items`, lalu **autosave segera**. Kematian/reload tidak boleh menghapus item yang sudah tersimpan pada checkpoint valid.
- Status objek di scene (**gembok, dinding ilusi, pintu rantai, powerbank yang sudah diambil, lampu halogen**) direkonstruksi dari data, bukan asumsi default prefab.
- **[KLARIFIKASI]** Saat menyimpan, gunakan sistem `CampaignState` dan mekanisme atomic save yang **sudah ada** di proyek apabila tersedia; jangan membuat sistem save paralel tanpa alasan. Data spesifik babak disimpan sebagai bagian dari save yang sama.
- **[USUL BARU]** Bedakan progress persisten (barang/bukti/rahasia/boss) dengan *transient runtime* (waktu 5 detik shield, posisi projectile, timer deteksi hantu) agar reload tidak memuat kondisi mustahil.

### 6.2 Skema save tanpa sumber kebenaran ganda (USUL BARU)

**Simpan set ID, bukan boolean turunan ganda.** `completed_event_ids` adalah event satu kali; `evidence_ids` = bukti unik, `document_ids` = dokumen, `CampaignData.inventory` = pusaka/kunci/instance consumable, `secret_items` = collectible global. `stage`, `hasKeris`, `hasLiftKey`, `boss.started`, `local_secret_status` **diturunkan** dari data itu saat runtime. **Progres halogen dan `P1_ALTAR_INTERACT` tetap hanya di memori percobaan boss**, dan harus hilang saat respawn CP5: tidak boleh disimpan di disk bersama snapshot CP5. Event `P1_BOSS_WIN` dan `location_seals` harus dimutasi atomik agar tidak ada segel hilang setelah reload.

```json
{
  "schema_version": 2,
  "chapter_id": 1,
  "checkpoint_id": "PHASE1_CP2",
  "inventory": ["SMARTPHONE_RAKA", "P1_LIFT_KEY", "P1_POWERBANK_MAILBOX_207"],
  "evidence_ids": ["P1_CLAW_402"],
  "relic_ids": [],
  "secret_items": [],
  "location_seals": [],
  "battery": 75,
  "sanity": 100,
  "wallet_legitimate": 0,
  "wallet_bound": 0,
  "spent": 0,
  "phase1": {
    "completed_event_ids": ["P1_APP_PATCH", "P1_MAILBOX_207"],
    "document_ids": ["P1_DOC_GUARD_2004"],
    "optional_challenges": [{"id":"P1_SAWER_SIGNBOARD", "status":"EXPIRED"}],
    "dynamo_full_charge_used_ids": ["P1_DYNAMO_L1"],
    "lift_l3_emergency_topup_used": false,
    "elapsed_gameplay_seconds": 900,
    "dynamo_emergency_last_uses": [{"id":"P1_DYNAMO_L1", "at_gameplay_seconds":780}],
    "money_ledger": []
  }
}
```

**[AUDIT r1 — format usulan]** Skema di atas belum dapat dibaca loader schema 1. Migrasi `1→2` dan validasi wajib dibuat di `CampaignState` yang sama; jangan sekadar mengganti angka versi. Snapshot disingkat, field campaign lain dari `CampaignData` tetap dipertahankan. List receipt untuk challenge/waktu dinamo dipilih karena `JsonUtility` bukan serializer dictionary/HashSet. Runtime boleh memakai set/index yang dibangun ulang dari list; jangan persist dua salinan nilai yang sama. `evidence_ids`, `inventory`, `battery`, `sanity` dan `checkpoint_id` tetap milik campaign yang existing; jangan buat duplikat di `phase1`.


**Angka dan ID dalam JSON merupakan contoh snapshot CP2 (bukan inventori awal kanon).** `dynamo_full_charge_used_ids` menyimpan satu kali pemulihan penuh per dinamo; `lift_l3_emergency_topup_used` menandai top-up tunggal yang dilakukan **sebelum CP3 disimpan**, sedangkan waktu pemulihan darurat dinamo memakai *elapsed gameplay time* yang kompatibel dengan save kampanye (bukan jam dinding OS). Saat berada di `CHAIN_INTERACT`, progres dinamo tidak berhubungan dengan timer hantu. Nama final mengikuti sistem `CampaignState` proyek. `money_ledger` memuat transaksi unik (ID event + nominal); `available_money` hanya saldo turunan atau field resmi ekonomi proyek. Save schema 1 aktual **tidak memiliki field stage**; stage P0 diturunkan dari flag `EVENT_P0_*` di inventori. Migrasi harus mempertahankan flag dan seluruh data prolog; jangan mengarang migrasi stage yang tidak ada. Event P1 baru dapat disimpan di nested data P1 tanpa memindahkan seluruh event P0. Timer 1,5s, 5s, serta status luka sementara tidak disimpan. Save item rahasia tetap langsung dan sekali saja.

### 6.3 Checkpoint yang diusulkan (USUL BARU)

| Checkpoint | Waktu yang baik | State minimal | Recovery |
|---|---|---|---|
| CP1 | Keluar pos satpam / masuk pompa | Update app, dokumen pos dan live | Intro tidak perlu diputar ulang bila telah tercatat |
| CP2 | Sesudah mailbox 207 | Kunci lift + reward powerbank | Puzzle tidak perlu diulang untuk sekadar kembali ke lift |
| CP3 | **Tiba di L3 sesudah lift macet, sebelum manifest** | Posisi di lift/landing aman, **baterai `max(arrival,20%)` tepat sekali**, cutter masih di ceruk, rantai utuh, hantu belum mengejar | Retry mengembalikan **snapshot CP3** min 20%, bukan mengisi tiap kematian; bahkan 0% selama encounter masih memiliki `WATCH_ONLY` |
| CP4 | Setelah masing-masing bukti valid | Set bukti unik, posisi safe | Hindari re-scan membosankan karena stealth gagal |
| CP5 | **Saat E pertama valid di altar K404 setelah bukti + konfirmasi Jurnal + keris lengkap; sebelum menancapkan** | 3 bukti + konfirmasi Jurnal + **keris masih dalam inventori**, altar belum dikonsumsi, dinding K404 utuh, boss belum aktif | Retry selalu dari snapshot ini; baterai snapshot CP5; floor 40% hanya alternatif D06 yang belum disetujui; 3 lampu utuh, hadiah tidak diduplikasi |
| CP6 | **Pada P1_BOSS_WIN, transaksi atomik** | Boss kalah + keris pindah ke altar + segel pertama + panel lift pulih | Semua lampu tidak perlu dipersist satu-satu; kembali eksplorasi tanpa boss berulang |
| CP7 | Babak selesai | Semua loot, rahasia, ekonomi, lokasi tujuan | Memulai babak berikutnya tanpa duplikasi |

**[KLARIFIKASI — atomic checkpoint]** Saat **pemain berdiri dalam radius altar K404 dan menekan E pertama kali dengan 3 bukti + konfirmasi Jurnal + keris lengkap** (`ALTAR_READY` runtime), simpan CP5 **dari posisi tepat di depan altar** ke disk **sebelum** mengurangi inventori/menancapkan keris, menandai event ritual, mengunci arena, atau meruntuhkan dinding. `P1_ALTAR_INTERACT`, kondisi altar, posisi boss, dan `completedLightIds` hanya hidup dalam memori percobaan ini. **Jangan autosave/write ke disk selama `BOSS_ACTIVE`**, termasuk saat Flash sukses 1–2 kali, pause, atau cutscene tengah fight. Kematian, restart game, atau reload manual sebelum menang selalu mengembalikan CP5 (3 bukti + konfirmasi Jurnal, keris masih ada, altar belum aktif, dinding belum jebol, 3 lampu utuh). Setelah 3 Flash sukses, `P1_BOSS_WIN` melakukan **satu commit atomik CP6** yang memindahkan keris ke altar dan menambah segel; penulisan gagal harus mengembalikan snapshot terakhir yang valid (CP5/CP6) bukan state separuh jalan.

**[BASELINE UJI]** Saat kalah pada pertarungan, gunakan kebijakan **reset semua tiga lampu dari CP5** dengan energi baterai persis snapshot CP5 (floor 40% memerlukan D06) dan tanpa menduplikasi item/uang. Jangan mencampur reset lampu dengan skema checkpoint parsial 1–2 poin dalam satu versi implementasi. Jika ingin checkpoint antarfase setelah playtest, ganti kebijakan dengan migrasi dan QA yang jelas.

### 6.4 Idempoten dan transaksi sederhana (KLARIFIKASI)

```text
if EvidenceIds.Add("P1_CLAW_402") == newlyAdded:
    Journal.RecalculateUniqueEvidenceCount()
    RequestSave()

if EvidenceIds.ContainsAll(RequiredEvidenceIds) AND Completed("P1_JOURNAL_CONFIRMED") AND Inventory.Contains("P1_KERIS") AND PlayerPressedInteractAtAltar AND PlayerWithinAltarRange:
    if EnsureDiskCheckpoint("CP5", playerAtAltar=true, kerisInInventory=true) == SUCCESS:
        StartBossAttemptInMemoryOnly()                       // dinding 404 jebol secara runtime

if bossIsVulnerable AND validHeadShot AND not runtimeLight[activeLightId].completed:
    runtimeLight[activeLightId].completed = true          // jangan tulis ke disk selama boss
    if CountDistinctRuntimeLights() == 3:
        CommitAtomic("CP6", bossWin=true, kerisAtAltar=true, addLocationSeal=true)
        TriggerVictoryEffectsOnce()

if BossAttemptFailedOrReloaded:
    RestoreDiskCheckpoint("CP5")                        // 3 lampu utuh; keris tetap ada

if MotorExitPanelOpened:
    ShowArchiveStatusNoSave()                       // tutup panel = tanpa completed event
if PlayerConfirmedExit AND EpilogueComplete AND not Completed("P1_COMPLETE"):
    if CommitAtomic("CP7", addCompletedEvent="P1_COMPLETE", nextDestination="RSUD Lama Sido Waras") == SUCCESS:
        FadeToBlack()                               // gagal commit => panel tetap bisa dicoba lagi

if TryAddSecretAndCommitSameCampaign("SECRET_P1_NEGATIF_TARI") == COMMITTED:
    ShowCollectedAndHideWorldObject()
else if SAVE_FAILED:
    RestorePreviousDataAndKeepWorldObjectForRetry()
```

Pseudocode mengilustrasikan *state transition*, bukan API existing. Proyek memakai panggilan director + receipt ID di `CampaignData.inventory`, belum bus event generik. `SaveCheckpointAtomic` mengamankan file tetapi hanya mengembalikan `checkpoint_id` bila gagal; mutasi lain memerlukan rollback lengkap seperti pola `AtmajaDirector.Commit`. Perluas pola itu untuk transaksi P1 di save yang sama, bukan membuat penyimpanan paralel. Selama boss, lindungi seluruh jalur tulis (termasuk `ArchiveDocument`, pembayaran Bank dan callback UI), bukan hanya autosave director. Indikator collectible sukses baru tampil setelah write berhasil; failure mengembalikan set/objek/UI untuk retry.

---

## 7. Presentasi naratif, atmosfer dan aset

### 7.1 Daftar beats sinematik (KANON)

| Adegan | Beat audio/visual | Tujuan dramatis |
|---|---|---|
| Awal parkir | Gerimis, kabut, void besar gelap; motor & Handycam Damar | Membuat Raka tampak kecil terhadap rusun |
| 23:58 | HP bergetar, UI patch merah, chat pertama | Dunia digital mulai campur tangan dengan gaib |
| Pos satpam | Buku log 2004, bau minyak tanah/karat | Menguatkan lokasi sebagai bekas tragedi |
| Papan nama parkir | AR memperlihatkan siluet korban (bila misi opsional diambil) | Pengenalan opsional pertama bahwa kamera melihat hal berbeda |
| Kamar 102 | Tembok berubah di AR, pintu kayu gaib | Demonstrasi **wajib pertama** AR sebagai alat membuka jalur fisik |
| Tuyul | Cekikikan dan langkah kecil, baterai berkedip | Tegangan resource dan ancaman nonletal |
| Lift L3 | Rem pengaman terkunci akibat gangguan gaib, neon mati-nyala | Mengurung pemain di lantai yang salah |
| Kuntilanak | Tangisan makin pelan saat hantu dekat | Membalik ekspektasi arah suara |
| Genderuwo L4 | Benturan `DUG`, dinding 401 jebol, desah berat | Menggeser genre dari investigasi ke predator stealth |
| EVP kamar mandi | Statis radio, bisikan keris, dobrak pintu | Mengikat bukti dengan respons antagonis |
| Halogen | Kabut terbakar, bidikan merah, lampu meledak | Penghargaan pembacaan mekanik, bukan peluru biasa |
| Ending | Video Damar 5 detik, monitor `BEEP... BEEP...` | Menarik pemain ke rumah sakit babak selanjutnya |

### 7.2 Prop dan art direction spesifik (USUL BARU)

**Exterior/parkir:** papan nama rusun bekas bercak darah, pagar dilas, motor Raka, kabel jemuran, sampah kehujanan, bekas bencana yang tidak terlalu dekoratif. **Pos satpam:** kalender tahun 2004, buku piket dapat dibaca, meja tua, lampu berkedip, pintu servis. **L1/ruang pompa:** pipa berkarat, kebocoran, balok runtuh, sinyal AR tidak stabil. **L2:** deretan kotak surat 201–210 terbaca, daftar iuran, pintu 207, ceruk arsip bisa ditemukan tanpa panah. **L3:** pintu lift macet, lorong 40m, ruang kebersihan di sisi escape path, rantai menuju L4. **L4:** ruangan 401–404 berbeda identitas, dapur hangus dengan tungku jelas, kamar mandi bertiang/bilik valid, altar di 404, panel lampu sorot yang tampak punya sumber listrik masuk akal.

**[USUL BARU]** Visual motif berulang: angka `14`, nama penghuni yang dihapus pada daftar iuran, jelaga berbekas telapak, dan sisa kain Tari. Jangan seluruh dinding bertuliskan petunjuk merah terang; petunjuk AR menjadi kejutan lokal yang perlu dicari. Semua teks puzzle harus terbaca pada target resolusi dan tetap cocok dengan lokalisasi bahasa Indonesia.

### 7.3 Audio dan VO (KLARIFIKASI + USUL BARU)

**WAJIB konten naskah:** Raka monolog di parkir, sapaan live, dialog kain Damar, suara Mbah Suro pada kaset, suara EVP, chat penting, audio Kuntilanak/Tuyul/Genderuwo, suara lift, pecahnya halogen, dan monitor rumah sakit. Suara nonverbal harus ikut berjalan walau subtitle/dialogue di-skip.

**[USUL BARU]** Gunakan 3 lapisan ambience (cuaca/ruang, mekanikal, gaib) dan *priority ducking* ketika VO/EVP penting. Tambahkan subtitle Indonesia dengan identitas pembicara: `[Raka]`, `[Rekaman Mbah Suro]`, `[EVP — Tidak Dikenal]`; volume UI chat notifikasi dapat diatur terpisah. Audio cue Kuntilanak harus memberi informasi jarak secara konsisten, bukan efek acak semata.

### 7.4 Live stream dan teks chat (KANON + KLARIFIKASI)

Rentetan chat aslinya tetap berada di **bagian Naskah Adegan**. Saat implemen, gunakan *timeline* yang terhubung ke event (bukan menembakkan semua chat di `Start()`), dengan antrian UI yang bisa dibaca ulang bila adegan cepat. Donasi `Penonton_Terakhir` merupakan kejadian cerita; status misi `@sultan_sawer` berbeda dari transaksi yang sudah sukses. **[USUL BARU]** Tambahkan *history panel* di HP agar clue yang lewat dapat dibaca kembali.

### 7.5 Kualitas presentasi yang disarankan untuk proyek Unity (USUL BARU)

Project menggunakan **Unity 6.3 LTS, URP, Windows x86_64**, dan perlu diuji juga pada laptop kelas **Intel Iris Xe/RAM 8GB** sesuai target pengembangan. Dokumen ini tidak mengklaim performa scene telah diuji. Prinsip produksinya:

- **Art pass:** blockout → *composition pass* → aset nyata/material rusun Indonesia → set dressing/clutter → decals kotor/terbakar → lighting → VFX/audio → gameplay polish. Jangan menambah banyak ruangan low-quality hanya untuk durasi.
- **Penerangan diegetik:** neon, bohlam darurat, layar HP, halogen bangunan; hindari ruangan terang tanpa sumber lampu. Frame horor gelap tetapi navigasi dan petunjuk tetap terbaca.
- **Biaya GPU:** pakai static batching/instancing bila cocok; batas *real-time shadow* selektif; matikan lampu realtime yang tidak terlihat, *occlusion* atau area streaming jika diperlukan; prioritaskan VFX transparan sederhana untuk kabut.
- **Telegraph harus murah:** audio 3D dan siluet lebih penting dari partikel mahal. Gunakan pool untuk puing/percikan dan kendalikan jumlah rigidbody aktif.
- **Testing performa:** profil build Windows nyata dengan scene lengkap, termasuk saat ketiga halogen, banyak chat UI, VFX boss, hujan, dan save/load. Validasi headless adalah tambahan untuk logika, bukan pengganti evaluasi visual/fps dengan GPU.
- **Asal aset & inspirasi:** DreadOut/Alan Wake/Outlast/Phasmophobia adalah **referensi rasa mekanik**, bukan izin menyalin aset, model, audio, level, UI, atau kode proprietary.

**[PERLU KEPUTUSAN]** FPS final, budget draw call, VRAM, batas ukuran build, serta kualitas material belum dinyatakan dalam naskah asal. §10.3 memberikan *baseline* benchmark awal untuk laptop uji; validasi dengan build Windows nyata sebelum mengunci acceptance final.

---

## 8. Anti-softlock, kesulitan, aksesibilitas, dan UX

### 8.0 Parameter prototipe terpusat (BASELINE UJI; bukan kanon baru)

Semua angka yang dapat di-*tune* didefinisikan **sekali** pada `Phase1BalanceConfig` (atau asset konfigurasi yang sudah dipakai proyek). Jangan menyalin konstanta berbeda di beberapa skrip. Naskah babak mengunci 2%/detik drain Tuyul, 10 detik timeout, 1,5 detik Kuntilanak, 3 detik stun, jendela boss 5 detik, dan tiga lampu; **parameter HP bersama mengikuti §0.4**, bukan default tandingan. Config P1 menyimpan angka encounter; nilai HP shared tetap dibaca dari `SmartphoneController`/definisi global, tanpa duplikasi konstanta.

| Parameter | Baseline | Status |
|---|---:|---|
| `phone.startBatteryPct` | 100% saat mulai babak baru, kecuali state kampanye diwariskan | BASELINE UJI |
| `phone.normalDrainPerSecond` | **0,05** (1%/20s) | GLOBAL; existing `SmartphoneController` |
| `phone.arOrUvDrainPerSecond` | **0,10 total**, bukan tambahan | GLOBAL; saling menggantikan normal |
| `phone.evpExtraDrainPctPerSec` | **0** | GLOBAL: overlay mengikuti profil mode aktif, tidak ditumpuk |
| `phone.photoCostPercent` / `blitzCostPercent` / `blitzCooldownSeconds` | **1% foto / 15% Blitz / 6s** | GLOBAL; usir Tuyul memakai Blitz, bukan biaya 4% baru |
| `phone.emergencyStunCostPct` | 15% | KANON |
| `phone.largePowerbankRestore` / `smallPowerbankRestore` | **80 / 35 poin** | 80 GLOBAL; 35 existing kecil kos; tipe P1 besar baseline D06 |
| `inventory.phase1InitialPowerbanks` | **1x**, satu kali saat mulai P1; +1x lagi hadiah mailbox 207 | BASELINE UJI; sesuaikan carryover kampanye sebelum freeze |
| `phone.dynamoFullRestorePct` / `dynamoFullDurationSec` | **+25% / 8s**, sekali per lokasi, persist ID lokasi | BASELINE UJI |
| `phone.dynamoEmergencyCapPct` / `dynamoEmergencyDurationSec` | **Bisa mulai bila baterai <10%**, 45s interaksi **akumulatif**, hasil maksimum 10% | BASELINE UJI |
| `phone.dynamoEmergencyCooldownSec` | **120s gameplay** antar pemulihan darurat per lokasi; tidak berlaku dalam boss | BASELINE UJI |
| `phone.liftL3BatteryFloorPct` | **20% saat CP3 pertama dibuat** dari sel darurat lift; satu kali per playthrough, restore snapshot saat retry | BASELINE UJI |
| `phone.kuntiWatchOnlyAtZeroBattery` | **true**, aktif hanya selama encounter Kuntilanak L3; kamera saja, tidak mengisi daya/AR/EVP/Flash | KLARIFIKASI anti-softlock |
| `interaction.cutterRadiusM` / `interaction.chainRadiusM` | **1,3m / 1,6m**, independen arah kamera; tetap cek ruang/halangan tembok | BASELINE UJI |
| `kunti.chainInteractionSec` | **2,0s**, AI/damage/watch-timer dibekukan saat animasi tidak dapat dibatalkan | BASELINE UJI |
| `tuyul.drainPctPerSec` / `tuyul.timeoutSec` | 2%/s / 10s | KANON |
| `player.walkForwardMps` / `player.walkBackwardMps` | 2,6 / 1,8 m/s | BASELINE UJI |
| `kunti.pursuitMps` / `kunti.catchDistanceM` | **2,4 m/s** / 1,2m | BASELINE UJI |
| `kunti.unwatchedGraceSec` | 1,5s | KANON |
| `gender.patrolMps` / `gender.chaseMps` | 1,3 / 3,0 m/s | BASELINE UJI |
| `gender.emergencyStunSec` | 3s | KANON |
| `boss.exposedSec` / `boss.lightsNeeded` | 5s / 3 | KANON |
| `boss.beamRadiusM` / `boss.flashMaxRangeM` | 2,5m / 8m | BASELINE UJI |
| `boss.headshotAimToleranceDeg` | 20° | BASELINE UJI |
| `boss.secondHitWindowSec` | 8s | BASELINE UJI |
| `evp.bossReachesStallDelaySec` | 3,5s | BASELINE UJI |
| `sanity` | Nilai campaign dipertahankan | GLOBAL: mekanik Sanity baru mulai Babak 4; usul P1 D02 ditahan |
| `sanity.nearThreatLossPerSec` / `sanity.safeRecoveryPerSec` | **Tidak aktif di P1** | Angka lama 0,6/0,25 hanya alternatif D02, jangan diterapkan default |
| `sanity.lowStealthAudioRadiusBonusM` | **0 m di P1** | Tambahan +1m ditahan sebagai D02; napas/stimulus biasa tetap boleh |

**Recovery anti-softlock [BASELINE UJI]:** pada baterai nol, AR/EVP normal tidak bekerja, tetapi dinamo masih bisa dioperasikan tanpa HP. Tempatkan **3 dinamo** di (1) ruang pompa sebelum K102, (2) koridor L2 sebelum mailbox, (3) **Kamar 403** untuk Fase B. **Tidak ada dinamo eksplorasi di arena Fase C. Default global tetap membutuhkan titik daya darurat arena terpisah fungsinya, tetapi memakai resource HP yang sama; desain mekanisme fisiknya perlu disepakati.** Masing-masing memberi **satu kali** pengisian normal `+25%` selama `8s`, disimpan sebagai ID dinamo terpakai. Setelah pengisian normal habis, tersedia *emergency crank* di lokasi sama **bila baterai <10%**; butuh **45s akumulatif** (progres disimpan hanya di RAM objek selama sesi, **dipause saat pemain lepas E atau patroli memaksa bersembunyi**, tidak reset ke 0), **cooldown 120s waktu gameplay antar pengisian sukses** dan hasilnya **maksimum 10%**, bukan +10% berulang sampai 100%. Bila baterai 3% (di bawah biaya Blitz global 15%), pemain boleh langsung memulai. **Dinamo eksplorasi tidak tersedia di boss; titik daya darurat arena global bukan dinamo eksplorasi ini.** di K403 progres hanya dapat berlangsung saat player tidak dikejar dan ruang dapat ditinggali aman sebentar. Pengisian darurat tetap dapat diulang setelah cooldown sebagai *last-resort* anti-softlock, tetapi tidak pernah mendorong baterai melewati 10%; tekanan survival berupa waktu dan risiko patroli, bukan batas isi tanpa jalan keluar. Durasi/titik aman harus diuji agar pemain benar-benar dapat memakainya meski baterai habis; K403 harus memiliki momen persembunyian dari patroli.

**[BASELINE L3 — D01/D06]** CP3 mengembalikan **minimum 20% sekali dari sel darurat lift**, dan kamera `WATCH_ONLY` tetap menampilkan Kuntilanak pada 0% khusus encounter L3. **Normal AR/EVP/Flash di 0% tetap mati**. Pemain tidak dapat mencuri daya kamera L3 untuk menambah baterai atau membawa mode itu ke K403/boss.

**Default r1:** baterai 0% saat boss dipulihkan lewat titik daya darurat arena yang aman saat recovery; Blitz tetap 15%/6s. **Alternatif D01 yang ditahan:** energi halogen saat exposed untuk Flash pada 0%; mekanik lain tetap tidak mendapat energi gratis. Retry CP5 memuat baterai persis snapshot; floor 40% merupakan alternatif D06 yang ditahan dan 3 halogen utuh, **tanpa menciptakan ulang uang/powerbank**. Tidak ada regenerasi otomatis pasif. Kombinasi dinamo manual darurat yang lambat, powerbank yang bisa hilang, dan konsumsi HP saat eksplorasi menjaga tekanan resource meskipun jalur utama tetap solvable.


### 8.1 Mekanik baterai harus selalu bisa dipulihkan (USUL BARU)

Dokumen sumber belum menjelaskan **kapasitas awal baterai**, biaya penggunaan AR/EVP/Flash biasa, kecepatan recharge, atau apa yang terjadi bila baterai nol. Hal ini berisiko besar karena HP dipakai untuk *semua* puzzle wajib. **Usulan kontrak aman:** tidak ada situasi persisten ketika player tidak bisa mengaktifkan AR/EVP/flash segel padahal belum bisa mendapat energi; tiga dinamo §8.0 masing-masing punya pengisian penuh **sekali** dan opsi darurat **manual, bersyarat, hingga 10% saja**; **L3 memiliki pengecualian kamera WATCH_ONLY yang tidak membutuhkan baterai untuk mechanic wajib**. Pemain memperoleh recovery arena sesuai kontrak global; jumlah/durasi refill diuji agar tiga Blitz 15% tetap dapat dicoba ulang. Opsi halogen tanpa baterai ditahan sampai D01 disetujui. Biaya mengisi ulang, cooldown, dan resource pressure harus dites, jangan mengandalkan donasi.

**[KANON]** Drain Tuyul 2%/detik, waktu 10 detik, Flash stealth 15%, vulnerable boss 5 detik. Naskah asal tidak merinci biaya boss/standby; default r1 mengikuti parameter global §0.4. Alternatif energi halogen belum menjadi kanon.

### 8.1A Damage, Sanity, uang dan upgrade (BASELINE UJI)

**Kondisi gagal Raka:** tangkapan langsung Genderuwo dalam stealth dan tabrakan Kuntilanak = Game Over. **[D02 — PROPOSAL BELUM DISETUJUI]** Saat boss, hit puing pertama = `WOUNDED` (layar retak/napas berat), hit kedua dalam 8s = Game Over; jika aman 8s luka pulih. Proyektil perlu telegraph dan collision satu-hit-per-object. **Proposal Sanity P1 berikut ditahan, bukan instruksi implementasi default:** Sanity terpisah dari health: penurunan ketika dekat ancaman, pulih sedikit di tempat aman/terang; Sanity di bawah **30** menambah vignette/detak jantung. **[BASELINE UJI]** Pada Fase B saja, napas Raka juga lebih terdengar sehingga radius investigasi suara Genderuwo bertambah **+1,0 m**; ini tidak mengubah LOS dan tidak membuat persembunyian tertutup otomatis terdeteksi. Ini konsekuensi gameplay ringan yang dapat dipahami dan dapat dipulihkan: diam aman di K403 mengembalikan Sanity secara bertahap **hanya pada alternatif D02**. Pada 0, Sanity **tidak** memblokir AR, bukti, interaksi, atau membunuh langsung. Gelap total sendiri tidak terus menguras Sanity selama satu jam. Audit menemukan hanya field Sanity save, bukan simulasi Sanity runtime; `Rancangan.MD` mengaktifkannya mulai Babak 4. **Default P1 tidak menerapkan seluruh efek/regen/radius di paragraf ini.** Status `WOUNDED` dua-hit boss juga perlu D02 sebelum acceptance gameplay dikunci; jangan membuat health bar/sistem HP kedua.

**Upgrade:** untuk baseline Babak 1 mewarisi upgrade antarbabak, **tidak ada toko/upgrade wajib di tengah stealth/boss**; sinkronkan dengan `Rancangan.MD`. `available_money` tidak sama dengan `lifetime_donations`, dan tawaran Rp1 juta belum menjadi pemasukan sebelum challenge sukses.

**Ledger:** jika Rp78,2 juta adalah total seumur kampanye termasuk Rp25,45 juta prolog, Rp10 juta pembuka, dan Rp3 juta L3, maka ada **Rp39,75 juta donasi lain yang belum ditulis** ketika tantangan Rp1 juta gagal, atau **Rp38,75 juta** jika berhasil. Ini **selisih matematika, bukan event donatur yang terbukti ada**. Jangan menciptakan saldo/uang palsu untuk menyamakannya. Tentukan apakah teks akhir harus fixed cinematic atau angka transaksi dinamis sebelum ekonomi dianggap final.


### 8.2 Kesulitan dan aturan “fair horror” (USUL BARU)

- Setiap aturan instakill diajarkan lebih dulu melalui petunjuk lingkungan, audio, atau peringatan HP sebelum konsekuensi fatal, kemudian beri pemain kesempatan mempraktikkan.
- Hantu yang hanya terlihat lewat AR tidak boleh menguras resource ketika karakter sedang terkunci di dialogue/cutscene atau UI.
- Dalam boss fight, kamera/head target dan tuas harus mudah ditemukan secara visual namun tetap menuntut timing; *miss* tidak mengonsumsi halogen selamanya.
- Jika player gagal berkali-kali, jangan otomatis membayar solusinya; tawarkan hint progresif opsional. Jurnal menyimpan clue penting yang terlewat.
- Jangan gunakan ruang puzzle yang memiliki collider/tempat sembunyi hanya pada satu arah yang membuat pemain tak mungkin keluar.

### 8.3 Aksesibilitas (USUL BARU)

- *Rebindable controls*; sensitivitas kamera dan inverted Y.
- Subtitle dan *chat history* untuk petunjuk yang disampaikan lewat suara/live chat.
- Opsi intensitas flashing, *camera shake*, chromatic aberration, motion blur; jangan menaruh informasi wajib hanya pada efek kedipan.
- Pembedaan tiga halogen memakai label/posisi, bukan warna saja; reticle segel jelas pada kontras rendah.
- [PERLU KEPUTUSAN] Apakah mode lebih mudah boleh melonggarkan ambang **1,5 detik** Kuntilanak atau 5 detik boss. Nilai dasar naskah tetap dipertahankan; opsi aksesibilitas bisa dibuat terpisah.

### 8.4 Transisi akhir dan item rahasia (KLARIFIKASI)

Urutan akhir **wajib tunggal**: `P1_BOSS_WIN` (Raka tetap K404) → `P1_DAMAR_CLOTH` → `P1_TAPE_PLAY` → `P1_EPILOGUE` (video Damar dan pesan, **tanpa fade**) → pemain bebas eksplorasi → **L4→L3 via tangga** → **lift L3→L1 servis (rute singkat)** → pompa → parkiran → **motor Raka**. **Cabang opsional:** dari L3 pilih lift ke **L2** untuk arsip Tari, lalu lift ke L1 atau jalur K102→L1; `P1_CROSS_102` menjaga collider terbuka dua arah tanpa HP aktif. Lift tidak bisa dipakai sebagai jalan keluar sebelum boss kalah; pintu servis L1 mengarah ke koridor pompa, bukan gerbang depan yang dilas.

**[KLARIFIKASI]** Hanya interaksi **`Tinggalkan Rusunawa` di motor di parkiran** yang membuka **panel sementara tanpa event save** dan menampilkan panel `Arsip Babak 1: 0/1 atau 1/1` dan pilihan `Kembali Menjelajah` atau `Lanjutkan`. Memilih lanjut memicu **satu-satunya event final `P1_COMPLETE`**, commit CP7 akhir, dan baru kemudian *Fade to Black*. Jika gagal menyimpan, jangan fade atau memindahkan pemain; tampilkan opsi coba lagi. Replay kaset setelah epilog tidak memicu video berulang atau mengakhiri babak. Pemain boleh kembali mengambil Negatif Foto Tari di L2 sepanjang belum konfirmasi keluar. Jika rahasia tidak diambil, status tetap 0/1. Status kesempatan chapter select setelah keluar tetap mengikuti kebijakan kampanye [PERLU KEPUTUSAN].

---

## 9. Produksi Unity: pembagian tanggung jawab tanpa overengineering (USUL BARU)

**Tujuan:** menerapkan desain di arsitektur proyek saat ini, **bukan** memerintahkan refactor besar-besaran. Nama komponen di bawah adalah contoh peran, bukan klaim sudah ada.

| Tanggung jawab | Kandidat modul/komponen | Perilaku utama |
|---|---|---|
| Stage, objective dan event | `Phase1FlowController` + data definisi level | Advance stage satu kali, kontrol transisi |
| Interaksi dunia | `IInteractable` / sistem proyek yang sudah ada | Pemeriksaan requirement, prompt, pickup |
| Bukti | `EvidenceInteractable`, `JournalEvidenceService` | Validasi AR/EVP, pencatatan ID unik |
| Proyeksi AR | `ARRevealTarget` / layer AR yang sudah ada | Visual reveal dan collision aman |
| Live | `LiveStreamTimeline` / sistem chat existing | Trigger line chat, history, donasi (state sekali) |
| Energi HP | `PhoneBatterySystem` yang sudah ada | Biaya, drain, konsumsi powerbank, recovery |
| Tuyul | `TuyulBrain` | Spawn, drain, steal jika ada, flee |
| Kuntilanak Putih | `KuntilanakWatchBrain` | Camera watch timer, chase, attack, resolve |
| Genderuwo stealth | `GenderuwoStealthBrain` | Patrol, LOS/light stimulus, stun, EVP ambush |
| Boss fight | `GenderuwoBossController` | Shield, 3 halogen, window, hit validation |
| Save | Adapter ke `CampaignState` existing | Persist flags idempoten tanpa save kedua |
| QA | EditMode/PlayMode tests dan scene diagnostics | Verifikasi gate, spawn, alur, reload |

### 9.1 Data-driven lebih baik daripada hardcode per adegan

**[USUL BARU]** Tempatkan teks dokumen/chat, item IDs, event IDs, jendela 5 detik, biaya 15%, cooldown, dan prefab reference dalam konfigurasi/asset definisi yang bisa ditinjau bersama penulis. Teks 2004, donasi, dan cutscene tidak perlu tersalin di banyak skrip. **Tetap gunakan definisi parameter global `Rancangan.MD`** untuk nilai yang shared lintas babak.

### 9.2 Pola implementasi bertahap

1. **Vertical slice L2:** AR tembok 102, puzzle 644, rahasia Tari, persistensi dan load; buktikan sistem HP dasar.
2. **L3 encounter:** kamera watch, gerak mundur, bolt cutter, checkpoint, kematian/retry; lakukan playtest navigasi penuh.
3. **L4 investigasi:** AI patroli, lemari/bilik, bukti, EVP ambush, Jurnal, keris; pastikan urutan investigasi fleksibel.
4. **L4 boss:** altar gate, halogen 3 titik, hit/miss/retry, kemenangan dan reset boss state.
5. **Naratif/UI:** chat, VO, dokumen lengkap, pacing 60 menit, cutscene RSUD, ekonomi tersinkron.
6. **Golden quality pass:** art, lighting, ambience, animation, optimisasi GPU nyata, automation tests + hands-on QA.

**[KLARIFIKASI]** Urutan produksi di atas adalah prioritas engineering, **bukan mengubah urutan cerita** 1–6 atau meminta pindah ke babak berikutnya sebelum Babak 1 playable dan polished.

---

## 10. QA: matriks uji, kasus tepi, dan kriteria selesai

### 10.1 QA wajib (KANON yang dijabarkan)

**[r1] Seluruh QA berikut adalah spesifikasi yang belum diuji pada P1. Kasus yang memakai WATCH_ONLY/top-up/grant mengikuti keputusan D01/D06; Sanity/dua-hit mengikuti D02. Oracle global yang sudah diselaraskan berlaku lebih dulu. Mempertahankan ID QA bukan berarti semua proposal sudah disetujui.**

| ID | Skenario | Hasil yang diharapkan |
|---|---|---|
| QA-01 | Main tanpa saweran/item berbayar | Jalur sampai boss tetap selesai |
| QA-02 | Lewati semua chat petunjuk | Solusi K102/644 tetap tersedia dari lingkungan/dokumen |
| QA-03 | Scan cakaran K402 tiga kali | Bukti fisik tetap 1, bukan 3 |
| QA-04 | Rekam EVP lalu load ulang | Bukti EVP tetap ada; sergapan tidak terulang tanpa kontrol |
| QA-05 | Scan aura tapi tidak ambil keris | Bukti aura ada, `hasKeris=false`, altar belum aktif |
| QA-06 | Ambil keris tanpa semua bukti; buka/keluar inspeksi altar | Tidak memulai boss / tidak mengunci arena |
| QA-07 | Semua bukti + konfirmasi Jurnal + keris, interaksi altar | Boss dimulai tepat sekali sesudah commit CP5 sukses; tanpa konfirmasi Jurnal ditolak |
| QA-08 | Flash saat perisai boss utuh | Tidak memberi damage segel |
| QA-09 | Pancing boss ke lampu, gagal headshot dalam 5 detik | Shield pulih, lampu tidak rusak; retry tersedia |
| QA-10 | Headshot sukses pada satu lampu berkali-kali | Lampu memberi maksimal 1 poin segel |
| QA-11 | Berhasil N, E, W | Tepat 3 poin → menang, AI dinonaktifkan, segel tersimpan |
| QA-12 | Ambil `SECRET_P1_NEGATIF_TARI` dua kali | `secret_items` hanya berisi satu ID, jurnal tetap 1/1 |
| QA-13 | Mati/reload sesudah ambil rahasia | Rahasia tetap dimiliki; counter Arsip global berdasar total ID |
| QA-14 | Selesai boss tanpa ambil rahasia | Lift aktif dan player bisa kembali L2 sebelum fade final |
| QA-15 | Cutscene akhir | Segel, upgrade, inventori, uang, Sanity dan rahasia konsisten sesudah reload |

### 10.2 Edge case tambahan hasil audit (USUL BARU)

| ID | Uji | Hasil |
|---|---|---|
| QA-16 | Tuyul mencuri saat `powerbankCount=0` | Tidak negatif/tidak crash; player masih bisa mencapai mailbox 207 |
| QA-17 | Baterai sangat rendah sebelum AR tembok atau mailbox | Ada mekanisme recovery yang memungkinkan lanjut |
| QA-18 | HP dimatikan di tengah kamera AR K102 | Player tidak terjebak collision di tembok |
| QA-19 | Spamming tombol gembok benar dan pickup reward | Kunci dan powerbank diberikan satu kali |
| QA-20 | Koridor 40m dilalui mundur, mengambil bolt cutter, memotong rantai | Seluruhnya dapat dilakukan tanpa memutar kamera melanggar aturan secara tak terelakkan |
| QA-21 | Kamera dipaksa berpindah oleh animasi rantai | Timer Kuntilanak tidak unfairly menghukum player |
| QA-22 | Kematian di koridor Kuntilanak | Respawn di checkpoint valid, hantu/item/chain reset benar |
| QA-23 | Boss muncul ketika player sembunyi/balik dari menu jurnal | AI tidak spawn menumpuk karakter atau menyerang di cutscene |
| QA-24 | EVT EVP dipicu ulang setelah load | Tidak memberikan bukti kedua/dobrak kedua yang tidak sesuai stage |
| QA-25 | Boss gagal 20 kali berturut-turut (uji otomatis) | Halogen tidak pernah habis tanpa 3 hit, tetap ada jalan menang |
| QA-26 | FPS menurun saat boss/halogen/Tuyul/chat/hujan | Profil frame time dan memory; tidak mengandalkan headless sebagai satu-satunya hasil |
| QA-27 | Satu item rahasia lain lintas babak sudah diperoleh | Arsip menampilkan progres berdasarkan ukuran set global, bukan index babak |
| QA-28 | Aksi epilog ditekan berulang / load cutscene selesai | Tidak menambahkan donasi, kain, kaset, atau segel dua kali |
| QA-29 | Player pause/game settings saat timer 5 detik | Timer/simulasi mengikuti definisi pause dengan konsisten |
| QA-30 | Dialog/video Damar dan dokumentasi setelah reload | Tidak menggandakan reward, story item tidak hilang |

### 10.2A QA tambahan reviewer (WAJIB untuk freeze)

| ID | Skenario | Hasil wajib |
|---|---|---|
| QA-31 | Baterai 0 sebelum K102/EVP | Dinamo terjangkau tanpa HP; emergency crank di <10% dan progres dapat dilanjutkan sesudah interupsi |
| QA-32 | Baterai 0 saat boss, 3 lampu belum selesai | **Default global:** recovery arena terjangkau saat aman, lalu Blitz 15%/6s bisa menyelesaikan boss. Alternatif halogen 0% diuji hanya jika D01 disetujui |
| QA-33 | Mati boss lalu retry 20 kali | CP5 mempertahankan baterai snapshot (40% hanya jika D06 disetujui), tiga lampu utuh, uang/item tak diduplikasi |
| QA-34 | L3: kamera turun sebelum cutter | Tidak instakill pra-pursuit; setelah cutter timer 1,5s benar |
| QA-35 | L3: menatap hantu tanpa mundur | Hantu menangkap pada jarak dekat; bukan teka-teki trivial |
| QA-36 | L3: tekan F/TAB refleks dan interaksi E | Kamera tak mati tak sengaja; pickup/rantai tetap bisa dilakukan |
| QA-37 | `P1_CUT_CHAIN` lalu menang boss dan kembali L4→L3→L2 | Koridor sudah aman sejak rantai putus; lift baru aktif setelah boss kalah |
| QA-38 | Puzzle 644 tanpa membaca live chat | Angka 6-4-4 deduktif dari papan, log, denah |
| QA-39 | Baca Catatan Tari & EVP | Lokasi keris konsisten, kaset 1998 bernada masa depan |
| QA-40 | Tantangan papan nama sukses/expired lalu reload | Status eksklusif dan donasi Rp1 juta paling banyak sekali |
| QA-41 | Save Sanity 0 di P1; terkena puing boss | Sanity diwariskan tanpa mematikan fitur. Kebijakan dua-hit/8s belum menjadi oracle sampai D02 disetujui |
| QA-42 | K403, posisi sakelar, dan 5s exposed | K403 terbuka saat stealth; arena boss tanpa dinamo; boss tidak melempar selama 5s |
| QA-43 | Overlay Rp78,2 juta vs saldo | Kedua nilai tidak dicampur; ledger yang tidak diketahui ditandai terbuka |
| QA-44 | Load save lama dengan flag duplikat | Migrasi tidak menghilangkan item rahasia, bukti, segel, dan uang |


### 10.2B QA tambahan v2.1 — dipertahankan untuk validasi v2.2

| ID | Skenario | Hasil wajib |
|---|---|---|
| QA-45 | Hitung simulasi posisi L3: spawn ±8 m di belakang pemain (trigger x19 → x≈11), cutter x22, pintu x40, kecepatan 2,4/1,8m/s | Model: 18 m = 10 s; hantu mencapai x≈35; celah ≈5 m; margin reaksi ≈(5−1,2)/2,4 ≈ 1,58 s sebelum E, ditambah stun foto ±1 detik sebagai jendela adil |
| QA-46 | Hantu diam sebelum cutter diambil dan sesudah 1 menit inspeksi | `WATCHED_IDLE` tetap x9; tidak menghabiskan waktu pengejaran |
| QA-47 | E ditekan di depan rantai sebelum radius tangkap; animasi 2s | `CHAIN_INTERACT` mengunci input, freeze AI/damage/watch timer 2s, lalu `RESOLVED` pada frame chain-cut; tidak mati di tengah animasi wajib |
| QA-48 | Balik lewat L3 sesudah rantai dipotong tetapi **sebelum boss dikalahkan** | Kuntilanak tetap `RESOLVED`; `P1_BOSS_WIN` tidak diperlukan untuk menonaktifkan ancaman L3 |
| QA-49 | Coba masuk K404 dari belakang saat Fase B | Semua dinding utuh; kabut mengunci pintu. Setelah altar interact, hanya dinding yang ditentukan runtuh dan loop boss dapat dilalui |
| QA-50 | Selesai boss, kain/kaset di K404, video, turun ke L2 lalu ke parkiran | Lokasi kontinuitas benar, epilog tidak fade, exit hanya di motor parkir setelah konfirmasi |
| QA-51 | Kaset ditonton 2x/epilog selesai, pemain masih belum keluar | Tidak menduplikasi chat/donasi/video/transisi; pemain tetap bisa mengakses L2 |
| QA-52 | 3 dinamo dipakai masing-masing sekali, resource 0 pada puzzle wajib | Pengisian normal tidak berulang; *emergency crank* tersedia bila <10%, 45s akumulatif hingga 10%, cooldown 120s; tanpa dinamo eksplorasi di arena, tetapi recovery daya arena global wajib tersedia |
| QA-53 | Masuk boss lalu 1–2 lampu sukses, mati atau force quit dan load | CP5 utuh: keris masih dimiliki, 3 bukti dan konfirmasi Jurnal ada, dinding 404 belum jebol, semua lampu belum terpakai |
| QA-54 | P1_BOSS_WIN sambil simulasi disk-write gagal | Commit CP6 atomik; reload tidak menghasilkan segel didapat tetapi keris hilang tanpa boss selesai |
| QA-55 | Encounter Tuyul awal, inventory start phase, lalu mailbox 207 | Powerbank awal 1x hanya jika D06 disetujui, grant saat first arrival bukan re-entry; hadiah kotak 207 sekali, bisa dua unit tersimpan di inventory yang sama, tidak digandakan retry |
| QA-56 | Sanity save <30 dibanding 100 saat P1 | Default global: tidak ada drain/regen/radius bonus P1. Alternatif +1m hanya jika D02 disetujui lintas dokumen; hiding tetap tidak auto-terdeteksi |
| QA-57 | Profil playtest lengkap target 60 menit di build nyata | Waktu L3 ≈5m, L4 ≈21m dan pulang L3→L1 ≈60–90s dicatat; bukan filler/timer paksa |
| QA-58 | Mati setelah pickup cutter namun sebelum rantai putus | CP3 memulihkan cutter di ceruk dan hantu idle; setelah sukses cutter+rantai tersimpan bersama, tidak duplikat |

### 10.2C QA akhir v2.2 — acceptance sebelum Codex implementasi penuh

| ID | Skenario | Hasil wajib |
|---|---|---|
| QA-59 | Masuk L3 dengan baterai 0%, 1%, 19%, dan 85% | Baterai tetap 0%, 1%, 19%, 85% sesuai carry-over; panel manual L3 dapat dipakai sebelum cutter; tidak ada grant gratis |
| QA-60 | Mati/retry L3 sepuluh kali, kembali ke lift, lalu load CP3 | Baterai selalu nilai snapshot CP3; tidak naik +20% pada tiap ulang |
| QA-61 | Baterai habis setelah CP3 sebelum/sewaktu pursuit | Layar mati pada 0%, tidak ada WATCH_ONLY. Sebelum cutter gunakan panel manual; gagal saat pursuit dapat retry CP3 tanpa grant dan mengisi panel fisik |
| QA-62 | P1_CUT_CHAIN berhasil lalu kembali lagi ke L3 dengan baterai 0% | Kuntilanak tidak respawn, layar tetap mati pada 0%, pemain dapat kembali lewat jalur terbuka |
| QA-63 | Kamera menatap hantu sementara pemain membelakangi cutter/rantai lalu menekan E dari ≤1,3/1,6m | Pickup dan chain-cut berhasil tanpa memaksa membalik kamera; interaksi menolak jika ada dinding solid yang memisahkan |
| QA-64 | E tertekan saat hantu sudah ≤1,2m dan saat masih >1,2m | Yang pertama Game Over; yang kedua animasi 2s aman lalu resolve atomik |
| QA-65 | E pada altar K404 dari dapur jauh vs saat tepat di depan altar | CP5 **tidak** tercipta dari dapur; tercipta sekali saat E valid depan altar dengan tiga bukti+keris |
| QA-66 | Memiliki keris tetapi baru 2 bukti; lalu uji K404 sesudah CP6 saat keris pindah ke altar | Kabut pintu lenyap dan collider terbuka sebelum boss; altar menolak bila bukti kurang; setelah CP6 **pintu tetap terbuka** walau keris tidak di inventori |
| QA-67 | Tiga menit di K403 dan jalur ke EVP + dapur | Gameplay ada navigasi stealth dan clue yang nyata; tidak ada gate/timer artifisial untuk mengisi 21 menit |
| QA-68 | Pemain pada L3 setelah boss, pilih tombol L1 vs L2 | L1 masuk servis ruang pompa untuk keluar cepat; L2 ke arsip; kedua rute kembali ke parkiran; gerbang depan tetap dilas |
| QA-69 | Buka panel motor → pilih Kembali Menjelajah → save/reload | Tidak ada `P1_LEAVE_RUSUN` atau event setengah jadi; stage tetap RETURN_TO_EXIT; dapat kembali ke L2 |
| QA-70 | Tekan Lanjutkan di panel motor berkali-kali atau simulasikan tulis CP7 gagal | Hanya satu `P1_COMPLETE`, tidak ada duplikasi segel/uang; jika gagal tidak fade, retry commit boleh dilakukan |
| QA-71 | Setelah `P1_CROSS_102`, baterai 0% di L2 atau L1 saat pulang | Bukaan fisik dua arah tetap terbuka tanpa AR, tidak terjebak di tembok |
| QA-72 | Dinamo K403: energi 3%, mulai crank, terinterupsi Genderuwo setelah 30s, kembali | Bisa langsung emergency saat 3%; progres 30/45s dilanjutkan setelah aman; tidak reset ketika tombol dilepas, tidak menyala saat chase |
| QA-73 | Dinamo emergency 9%, pakai berkali-kali setelah cooldown | Tidak pernah lebih dari 10%; 120s waktu gameplay sebelum dipakai ulang; full charge hanya sekali/lokasi dan save idempotent |
| QA-74 | Penanda papan enam kamar vs daftar Tari pada AR | Enam nomor menggunakan cap MERAH MENUNGGAK; hanya **nama Tari** disilang tinta hitam di arsip terpisah; clue rahasia tidak otomatis bocor |


### 10.3 Definition of Done (gerbang rilis Babak 1)

**Gameplay:** jalur 60 menit secara desain dapat dimainkan dari pembuka sampai transisi; semua 3 bukti dan 1 pusaka berfungsi; Kuntilanak dapat dihindari secara adil; Genderuwo punya stealth dan boss fase terpisah; tiga lampu bisa diselesaikan tanpa softlock. **Konten:** enam adegan, empat dokumen, dialog utama, chat event, kaset, video Damar dan Negatif Foto Tari ada serta terbaca. **Data:** save/load item unik, segel, baterai, uang dan Sanity konsisten; arsip tetap persisten. **Atmosfer:** suara, pencahayaan bersumber, ruang terbaca, aset nyata sesuai setting, tidak ada primitive placeholder pada jalur wajib, sumber cahaya utama punya fixture nyata, empat sisi koridor dapat dibedakan landmark-nya, audio dan interaksi final terpasang. Target **[BASELINE UJI]** pada Windows i5-1155G7/Iris Xe/8GB: 1280×720 Low rata-rata ≥30 FPS dan 1% low ≥24 FPS di tiga run lintasan plus boss; ukur build nyata, bukan headless saja. **QA:** lulus kasus wajib dan uji manual build Windows pada hardware sasaran; bug kritis navigasi, crash, dan penyimpanan = nol sebelum *freeze*.

### 10.4 Instrumentasi pengembangan yang membantu (USUL BARU)

Sediakan debug overlay khusus developer dengan `stage`, `completedEvents`, `evidence=0..3`, `hasKeris`, `activeLight`, `bossPhase`, `shieldTimer`, `phoneBattery`, `secretItemsCount`. Tambah tombol dev reset encounter **hanya dalam build development** agar tester dapat mengulang K102, Kuntilanak, EVP, atau boss tanpa memanipulasi save produksi. Catat berapa lama pemain mencari clue untuk membandingkan pacing target 60 menit dengan hasil playtest; jangan menahan pemain dalam koridor Kuntilanak hanya untuk mengejar angka durasi.

---

## 11. Kontinuitas naratif dan keputusan yang masih terbuka

### 11.1 Perbedaan sumber yang sudah dijembatani (KLARIFIKASI)

| Area | Potensi benturan | Penafsiran aman untuk implementasi |
|---|---|---|
| Tuyul & powerbank | Encounter Tuyul L2 terjadi sebelum reward powerbank di kotak 207 | Baseline mulai dengan 1 powerbank sekali; saat dicuri benar-benar kehilangan satu, hadiah mailbox +1 tetap tersedia |
| Kuntilanak & alat | Harus menatap hantu sambil mengambil alat dan memotong rantai | Alat ada di escape path; interaksi tangan/kamera tidak menyebabkan game over paksa |
| Pintu 404 vs boss | Kamar 404 awalnya tersegel pintu, tetapi arena harus terhubung loop | Fase B dinding utuh, keris melemahkan pintu; setelah ritual altar baru dua panel dinding jebol untuk boss |
| Bukti #3 vs pusaka | Keduanya berada di satu tungku | Scan aura dan pickup pusaka dua flag; keduanya diperlukan |
| Boss hit gagal | Lampu meledak setelah *successful* hit, bukan sekadar switch aktif | Miss/timeout mengembalikan boss shield tanpa menghancurkan lampu |
| Arsip Saksi | `1/5` global bisa membingungkan dibanding `0/1` lokal | `N/5` berdasarkan ID unik seluruh game; `0/1` atau `1/1` untuk babak 1 |
| Akhir babak | Kaset/epilog di K404 tetapi collectible opsional berada di L2 | Video Damar terjadi di altar K404; tak ada fade; transisi akhir hanya konfirmasi pada motor parkiran |
| Uang | Kuitansi Rp25,45 juta, donasi pembuka dan total Rp78,2 juta | Bedakan uang tersedia, donasi bersyarat, dan total naratif; ledger sumber belum lengkap |

### 11.2 Daftar keputusan desain yang perlu persetujuan (PERLU KEPUTUSAN)

1. **Sudah diputuskan global, bukan pertanyaan baru:** empat segel Babak 1–4 dan Pasar Setan gerbang kelima. **D05 yang masih terbuka:** apakah kaset 1998 berupa nubuat eksplisit untuk tragedi Rusunawa 2004. Jangan mengubah tahun/diksi menjadi kanon baru tanpa persetujuan.
2. **Tanggal dan jam cerita.** Pukul 23:55–23:58 adalah jam **naratif**, sedangkan durasi 60 menit adalah **waktu bermain**. Apakah jam dunia bergerak real-time atau hanya event tertentu? Jangan menggunakan 23:59 tanggal awal sebagai timer nyata tanpa keputusan.
3. **Handycam Sony Damar.** Ada di jok motor saat pembuka tetapi belum memiliki peran interaksi setelah itu. Putuskan apakah hanya properti naratif atau collectible/rekaman opsional. **Usul:** biarkan prop statis pada babak ini daripada menambah sistem kamera kedua yang belum dibutuhkan.
4. **Ekonomi penuh.** Rincian ledger yang menghasilkan total Rp78.200.000 belum disediakan. Apakah total mencakup seluruh saweran sejak prolog, misi yang sukses, dan transfer tertentu? Verifikasi lewat rancangan ekonomi.
5. **Aturan baterai.** Angka baseline ditabelkan §8.0. **Belum disetujui** sebagai balancing final dan perlu sinkronisasi state baterai prolog/global.
6. **D08 — Checkpoint boss.** Kandidat reset tiga lampu dari CP5; ratifikasi sebelum freeze. Tidak ada progres 1–2 lampu di disk. Floor baterai 40% bukan default, perlu D06.
7. **Kontrol yang sudah tetap:** E/J memakai `GameInputConfig`; V perspektif/X Selfie mengikuti desain terbaru. Prioritas Q/F/TAB/RMB/aplikasi, animasi rantai L3 dan kontrak watch perlu acceptance rinci; dukungan kontroler penuh belum menjadi scope.
8. **Status item dokumen.** Apakah semua empat dokumen wajib dibaca untuk menutup babak atau hanya konten pendukung? **Usul:** dokumen clue tidak menjadi persyaratan boss; bukti dan keris tetap satu-satunya gate ritual.
9. **Sudah diputuskan global:** chapter select backlog; setelah keluar, kesempatan arsip kembali melalui save relevan. P1 wajib memberi panel sadar sebelum transisi. Jangan menambah chapter select dalam MVP.
10. **Parameter target performa.** Tetapkan FPS, memory/VRAM, scene-loading target setelah profiling build nyata.

**D09 tambahan audit repositori:** tentukan apakah log/live wajib sebelum pompa, karena registry pump saat ini hanya memerlukan jalur dapat diakses sedangkan CP1 mengasumsikan log/live. Jangan menambah wall/gate atau membuat loop baterai 0% tanpa dinamo. Tentukan endpoint aman sesudah CP7 ketika scene RSUD belum ada; jangan memulai Babak 2 dalam implementasi P1. Jam 23:55/23:58 diperlakukan sebagai beat naratif sampai ada keputusan lain.

### 11.3 Tambahan cerita opsional tanpa mengubah inti (USUL BARU)

- **Jejak Tari yang humanis:** daftar tunggakan bukan sekadar kunci angka; sisipkan nama dan catatan sepele kehidupan penghuni sebelum kebakaran. Negatif foto memperlihatkan mereka sebagai manusia, memperkuat kalimat *“Kami pernah punya nama”*.
- **Kesaksian yang saling bertentangan:** laporan satpam mengacu bau minyak tanah dan pintu rantai, sementara Catatan Tari mencatat ritual mandor. Hubungan kausalnya bisa diungkap bertahap sehingga pemain menyimpulkan bahwa kebakaran kemungkinan bukan kecelakaan biasa.
- **Kejutan boss sebagai bekas penjaga ritual:** pola patroli dan halogen yang sengaja dirusak dapat mendukung kesan bahwa Genderuwo bukan monster acak, tetapi entitas yang melindungi peristiwa 2004.
- **Chat tidak serba benar:** beri 1–2 komentar ambigu atau penonton yang salah mengira, sementara bukti di lingkungan tetap akurat. Dengan begitu, pemain tidak hanya mengikuti instruksi chat.
- **Akhir lebih mencekam:** setelah boss selesai, musik turun hampir hening, baru bunyi tape, tepuk tangan, dan video Damar muncul. Gunakan peralihan audio sebelum memaksa UI penuh chat.

---

## 12. Checklist produksi yang bisa langsung dipakai (v2.2)

**Artefak dan navigasi**

- [x] Audit dokumentasi/repositori selesai pada r1; peta sistem aktual ada di §14 dan laporan audit. Proyek belum memiliki event bus generik; jangan mengganti `CampaignState` atau membuat sistem paralel. Implementasi tetap menunggu persetujuan.
- [ ] Buat denah Rusunawa empat sisi dengan **lift servis L1 di sisi ruang pompa**, L2 mailbox, L3 koridor 40m, tangga ke L4; validasi pintu depan yang dilas tetap tidak dapat dilewati.
- [ ] Terapkan K102: AR membuka ilusi pertama kali; setelah `P1_CROSS_102`, collider jalur terbuka **dua arah dan tidak butuh baterai/AR** saat pulang.
- [ ] Puzzle 644: **cap MERAH "MENUNGGAK" enam kamar** terpisah dari arsip **nama Tari yang disilang tinta hitam**; rahasia tetap opsional dan tersimpan unik.
- [ ] Sediakan K403 sebagai ruangan stealth nyata (bukan hanya ruangan kosong), dengan hiding, door, dinamo, dan clue jalur.

**Resource, encounter, dan bos**

[r1] Checklist berikut mengikuti pagar D01/D02/D06/D08 pada §0.4: baseline pengecualian belum menjadi persetujuan. Default biaya HP/recovery/Sanity global tetap berlaku.

- [ ] Mulai babak dengan baterai 100% sekali dan inventori carry-over tanpa powerbank tambahan; +1 powerbank dari mailbox 207 **sekali**; Tuyul mencuri bila tersedia tanpa inventori negatif.
- [ ] Tiga dinamo: 1 full-charge +25%/8s tiap lokasi, emergency jika **<10%**, 45s **akumulatif**, cooldown 120s, cap 10%; stop/pause progres saat terancam. **Tidak ada dinamo di arena boss.**
- [ ] CP3 pertama dibuat setelah panel lift darurat menetapkan `battery=max(current,20%)` satu kali; retry memuat snapshot dan tidak memicu recharge baru.
- [ ] Kamera Kuntilanak `WATCH_ONLY` tetap aktif saat 0%, **hanya di L3 selama encounter**; AR/EVP/Flash tidak gratis, baterai tidak bertambah.
- [ ] Kuntilanak memakai spawn di belakang pemain (±8 m, statis pra-cutter), speed 2,4m/s saat pursuit, 1,5s unwatched, radius kontak 1,2m, dan stun foto ±1 detik ≤5 m; interaksi cutter/rantai memakai **proximity E tanpa reticle**.
- [ ] `CHAIN_INTERACT` 2s membekukan AI/watch timer/damage, satu-satunya `RESOLVED` = `P1_CUT_CHAIN`, dan CP3 menangani pickup cutter yang belum jadi commit.
- [ ] Implementasikan patroli Genderuwo, cover di 403/402/kamar mandi, 3 bukti unik, clue EVP, jeda ambush 3,5s dan dua status terpisah untuk scan keris vs ambil keris.
- [ ] Pintu K404 **benar-benar terbuka** setelah keris dimiliki, tapi altar tetap butuh 3 bukti; dinding hanya jebol saat `P1_ALTAR_INTERACT`.
- [ ] CP5 hanya dari **E valid di altar dengan bukti + konfirmasi Jurnal + keris lengkap**, snapshot posisi altar dan baterai aktual (floor 40% perlu D06), **sukses tulis disk sebelum memulai boss**; stop autosave selama fight.
- [ ] Tiga lampu dan poin Blitz hanya runtime, miss tidak menghancurkan lampu, ketiga hit commit CP6 atomik; retry memulihkan keris/dinding/lampu/posisi secara penuh.

**Epilog, penyimpanan, dan pengujian**

- [ ] Kain → kaset → epilog Damar di altar K404; sesudahnya kembalikan kontrol **tanpa fade**.
- [ ] `P1_EXIT_PANEL_OPEN` hanya UI sementara di motor parkir, **bukan completed event**, batal tidak mengubah save; **hanya `P1_COMPLETE`** merupakan commit akhir (CP7), fade setelah commit sukses.
- [ ] Lift sesudah boss memberi pilihan **L1 cepat** atau **L2 arsip opsional**; cek jalur pulang tanpa hantu reaktif yang sudah di-`RESOLVED`.
- [ ] Validasi event saweran `P1_SAWER_SIGNBOARD`: sukses/hangus eksklusif, reward uang sekali; Rp78,2 juta tetap angka naratif sampai ledger global direkonsiliasi.
- [ ] Kerjakan dialog, live chat, audio EVP, ambience, lighting diegetik, clutter rusun, VFX, jalur empat sisi, dan minimalkan placeholder pada jalur final.
- [ ] Jalankan seluruh QA-01 sampai QA-74 (termasuk edge case 0% L3, tombol E radius, CP5/CP7, dan emergency crank); jalankan build Windows dengan GPU nyata, uji 60 menit tanpa memaksakan durasi.
- [x] **Audit kesesuaian repositori selesai**, deviasi dan rencana minimal dicatat pada r1/laporan. Ini bukan kelulusan gameplay atau izin implementasi otomatis.

## 13. Ringkasan audit Claude & Gemini — status perbaikan v2.2

| Poin review | Validasi | Penanganan |
|---|---|---|
| Baterai, softlock boss | **Valid, blocker** | Tabel §8.0, dinamo eksplorasi dan recovery arena global; Blitz halogen hanya alternatif D01; CP5 mempertahankan baterai snapshot (40% hanya jika D06 disetujui) |
| Health/damage dan Sanity kosong | **Valid, blocker** | §8.1A membedakan instakill stealth, 2-hit boss, sanity non-gating |
| Kuntilanak timer/arah/cutter/kecepatan/retry | **Valid, blocker** | Blocking 40m §3.3A, state `RESOLVED`, TAB/F lock |
| Keris di ubin 404, tungku, EVP | **Valid, kontradiksi** | Catatan Tari kini menjelaskan perpindahan; EVP menyebut tungku dan altar |
| Rekaman 1998 menggambarkan tragedi 2004 | **Valid, kontradiksi** | Diksi "kelak" bernada nubuat, butuh approval lore |
| Kode 644 ambigu | **Valid** | Surat menyebut 4 sisi void, log satpam menegaskan lantai 4 |
| Misi saweran tak ada registry | **Valid** | ID `P1_SAWER_SIGNBOARD`, sukses/expired, reward sekali |
| Total Rp78,2 juta tak terrekonsiliasi | **Valid** | Ledger digolongkan isu terbuka, beda dari available balance |
| Darah Raka tidak terjadi | **Valid** | Goresan tangan saat pemotongan rantai L3 |
| Tuyul pertama tanpa powerbank | **Valid** | Baseline inventori awal P1 = 1 powerbank idempoten; bisa dicuri sebelum hadiah kotak 207 |
| Rahasia Tari terlalu jelas | **Valid** | Clue pada lembar arsip ceruk terpisah dari papan angka 6 |
| Boss beam/arah/perilaku window | **Valid** | §4.5 denah konseptual, exposure tanpa serangan, validasi aim |
| EVP tanpa discoverability | **Valid** | Petunjuk suara statis + jeda mendobrak sampai mendekat |
| Save terduplikasi/angka tersebar | **Valid** | Set event persisten vs status lampu/altar runtime §6.2; CP5 dibekukan selama battle; balance §8.0 |
| Lift macet lalu pulih, K403 tidak punya fungsi | **Valid** | Lift pulih setelah boss; K403 selalu terbuka untuk stealth+dinamo, **tanpa dinamo eksplorasi dalam arena, dengan recovery arena global** |

### 13.1 Saran reviewer yang sengaja tidak disalin persis

- **Gemini: pengisian otomatis 10%→15%** tidak dipilih. Ini bisa dieksploitasi sampai mekanik manajemen resource kehilangan arti. Recovery lewat interaksi manual 8 detik lebih bisa dikendalikan.
- **Gemini awal: hantu 2,0m/s vs pemain mundur 2,2m/s** tidak dipilih: hantu justru lebih lambat sehingga tak bisa menyusul pemain yang bergerak menjauh. **Setelah audit matematika lanjutan, baseline v2.1 menjadi 2,4m/s melawan 1,8m/s**, dengan freeze AI tegas saat memotong rantai.
- **Claude: Kuntilanak pindah sisi ketika flicker** belum diadopsi. Bisa membuat kematian tidak adil bila telegraph kurang jelas; variasi audio, flicker, dan gerak tetap ada.
- Semua default di file ini adalah **baseline prototipe**, bukan pernyataan bahwa sudah cocok dengan mekanik kampanye `Rancangan.MD` atau bahwa sudah lulus build Unity.

### 13.2 Hal yang tetap PERLU KEPUTUSAN penulis

1. **D05:** status kaset sebagai nubuat 1998. Lima gerbang/empat segel/Pasar Setan sudah kanon global, tidak menunggu persetujuan ulang.
2. **D03:** sumber ledger dan status transfer. Rp78,2 juta sudah milestone kumulatif global; jadikan overlay naratif terpisah dari saldo, jangan otomatis mengubahnya menjadi penjumlahan transaksi yang belum ditulis.
3. **D01/D06:** ratifikasi recovery dinamo, top-up CP3/WATCH_ONLY, carry-over powerbank/baterai, dan alternatif Flash bertenaga halogen. Default boss tetap biaya 15%/6s + recovery arena global. Jangan mencampur dua alternatif.
4. Denah final, lokasi switch terhadap K401–404, nilai kontrol serta sistem save/upgrade global sesuai kode proyek.

### 13.3 Audit lanjutan v2.1 — keputusan dan cakupan

Audit susulan mengoreksi alur Kuntilanak (2,4m/s dan margin 2,42s), dua pemicu RESOLVED menjadi satu, epilog dan lokasi keluar, dinamo dan powerbank, pacing L3/L4, progres halogen transien, geometri K404, checkpoint CP5/CP6, dan Sanity. **Belum phase freeze**: wajib tinjau `Rancangan.MD`, repositori Unity aktual, QA-45 sampai QA-74, dan playtest.


### 13.4 Audit susulan v2.2 — kontrak final kandidat untuk Codex

| Kritik lanjutan | Keputusan v2.2 | Status |
|---|---|---|
| L3 baterai 0% sebelum/selama kamera | CP3 top-up minimal 20% **sekali** + kamera WATCH_ONLY 0% khusus encounter; tak menambah sumber energi | Tertutup pada spesifikasi; QA-59–62 wajib |
| Teks akhir kembali lewat lift tidak rinci | Lift sesudah boss punya pilihan L1 servis dekat pompa (cepat) dan L2 arsip (opsional); motor tetap satu-satunya exit | Tertutup pada spesifikasi; QA-68 |
| `P1_LEAVE_RUSUN` bertindihan dengan `P1_COMPLETE` | `P1_EXIT_PANEL_OPEN` UI runtime; `P1_COMPLETE` **satu-satunya ID persisten** | Tertutup pada spesifikasi; QA-69–70 |
| Rantai dan cutter butuh kamera menghadap objek | E dengan radius tangan independen kamera + pemeriksaan penghalang solid | Tertutup pada spesifikasi; QA-63–64 |
| Tambahan 5 menit L4 tidak berisi kegiatan | Timeline 27–48 terurai menjadi observasi 401, cakaran 402, jeda stealth 403, EVP, dapur, altar; tanpa gate waktu palsu | Rencana playtest; QA-67 |
| CP5 bisa tercipta terlalu dini | Snapshot saat E valid **di depan altar**, bukan saat keris diperoleh di dapur | Tertutup pada spesifikasi; QA-65 |
| Kabut hanya "melemah" | Kabut **lenyap dan collider pintu membuka** ketika keris dimiliki; tiga bukti tetap gate altar | Tertutup pada spesifikasi; QA-66 |
| Cap merah papan vs nama Tari dicoret | Status menunggak cap merah; hanya nama Tari dicoret hitam di daftar arsip tersembunyi | Tertutup pada spesifikasi; QA-74 |
| Emergency crank perlu threshold dan pause yang masuk akal | `<10%`, 45s akumulatif saat aman, cooldown 120s dan maksimum baterai 10% | Baseline uji; QA-72–73 |

**Status handoff:** dokumen ini **siap diaudit Codex terhadap kode repo dan dipakai untuk blockout/test**, bukan pernyataan telah lulus integrasi atau bahwa seluruh baseline angka wajib dikunci permanen. **Jangan otomatis implementasikan semua [USUL BARU]** jika berbenturan dengan arsitektur sudah ada. Penyelarasan `Rancangan.MD`, ledger Rp78,2 juta, dan benchmark performa nyata masih keputusan global yang harus diverifikasi.


## 14. Peta integrasi aktual dan gerbang implementasi (AUDIT r1)

### 14.1 Gunakan fondasi ini; jangan buat versi paralel

Semua path kode relatif ke `Assets/_LayarTerakhir/src/`.

| Kebutuhan | Sistem aktual | Batas dan pekerjaan aditif yang diperlukan |
|---|---|---|
| Campaign, item, segel, secret | `Core/CampaignState.cs` (`CampaignData`, `AddInventoryItem`, `SaveCheckpointAtomic`, `LoadCheckpoint`, backup) | Schema 1, list ID unik, tanpa nested P1/ledger/consumable quantity. Tambah nested P1 dan migrator di sistem yang sama. Dua powerbank tidak boleh memakai ID identik yang ditolak `AddInventoryItem`; pakai receipt instance unik/descriptor consumable di inventori yang sama. |
| Event/progres | `Prolog/AtmajaDirector.cs:Commit/RestoreWorld`; pola serupa Upper/Basement | Gunakan receipt sekali + rollback snapshot. Belum ada event bus umum; controller P1 terpisah dari cerita P0, bukan bus baru. |
| Gerak/crouch | `Player/FirstPersonController.cs`, `AnimatedRakaAvatar.cs` | Walk efektif maksimum 1,7, sprint 4,2, crouch 0,8 m/s. Tidak ada kecepatan mundur terpisah; 2,6/1,8 di tabel adalah target uji, bukan setting yang sudah berlaku. Jangan mengubah feel global P0 untuk memaksa hitungan L3. Tuning encounter lokal harus tetap cocok dengan animasi. |
| Kamera OTS/FP | `Player/ShoulderCameraRig.cs` | Collision sphere cast, perspektif tersimpan, override FP hiding. Watch harus memakai lensa HP, bukan kamera bahu; restore perspektif dan cursor setelah interaksi/gagal/load. |
| Interaksi E | `Player/IInteractable.cs`, `InteractionInteractor.cs`, `Prolog/HingedDoor.cs` | Normalnya raycast kamera 2,6m. Cutter/rantai memakai resolver proximity terbatas konteks + LOS tangan; satu pemilik input per frame agar E tidak mengambil/keluar hiding/mengiris bersamaan. |
| HP/baterai/AR | `Smartphone/SmartphoneController.cs`, `Prolog/AtmajaPhoneView.cs` | Pakai satu perangkat, baterai, camera feed dan AR layer 29. `AtmajaPhoneView` juga terpasang di Kos oleh `GameShell`, tetapi chat/live/isi app masih terkait Atmaja; perlu provider P1 kecil, jangan salin seluruh view. K102 membutuhkan representasi visual normal/AR dan latch collider baru. |
| Flash | `AtmajaPhoneView.CaptureFlash`, `AtmajaBasementHorror.TryEmergencyBlitz`, photo chase/upper | `CaptureFlash` hanya presentasi, bukan transaksi energi/hit. Field cooldown smartphone ada tetapi belum menjadi otoritas cooldown seluruh handler. P1 harus mengonsumsi satu aksi berdasarkan konteks target/mode, dengan biaya/cooldown sekali. |
| EVP | `GameInputConfig.ModeEVP`/C sudah dideklarasikan | **Belum ada recorder runtime**. Tambah sesi rekam in-game ke HP yang sama; tidak meminta mikrofon OS atau audio internet. Durasi scan/reach/abort ditetapkan sebelum QA. |
| Jurnal/HUD/padlock | `UI/PhoneDesktop.cs`, `HudController.cs`, `HudPresentation.cs` | Dokumen arsip dan padlock panjang dinamis tersedia; gembok 644 reuse UI itu. Bukti P1, tombol konfirmasi, Arsip N/5, daftar consumable dan chat history perlu isi/provider, bukan HUD kedua. |
| AI/hiding | `Prolog/AtmajaBasementHorror.cs`, `AtmajaThreatPresentation.cs` | Pola stimulus/cast/path/hiding bisa dirujuk, tetapi implementasi terikat Nyonya/Atmaja, graf planar dan radius ~0,23m. Bukan AI Genderuwo 2,5m siap pakai. Kuntilanak/Tuyul/halogen/proyektil baru diperlukan dalam modul P1. Jangan ubah AI P0 menjadi framework universal. |
| Audio/cuaca | `Audio/SceneAudioProfile.cs`, `AmbientAudioLoop.cs`, `ThreatAudioOcclusion.cs`, `PlayerFootsteps.cs`; mixer dan `PresentationProfile` | Reuse routing/occlusion/footstep/voice lookup; isi VO/EVP/monster P1 tetap baru. Kuntilanak memerlukan kurva suara invers, tidak cukup menyalin rolloff linear Nyonya. |
| Continue/kematian/travel | `UI/GameShell.cs`, `Prolog/PrologSceneFlow.cs`, `KamarKosPrologDirector.Start` | Continue sekarang kembali lewat Kos dan mengenal checkpoint P0 saja; checkpoint P1 yang tidak dikenal berisiko masuk jalur `StartNewGame`. Routing chapter/save harus didahulukan sebelum menguji CP P1. Jangan pakai `EndingType` global untuk menandai babak selesai. |
| Validasi/build | `Editor/FoundationAuditTests.cs`, `FoundationPlayAuditTests.cs`, `AtmajaPlayTests.cs`, `PlayerFeelTests.cs`; `Tools/run_golden_checks.py`, packaging/upload | Pertahankan tes P0. Bukti 114 tes 22 Oktober dan native Linux yang sudah tersimpan tidak membuktikan QA P1 lulus. Build/ZIP baru hanya setelah implementasi disetujui dan divalidasi. |

### 14.2 Ketentuan state yang harus diperinci sebelum kode

1. Pisahkan **data campaign persisten**, **snapshot aman checkpoint**, **state percobaan boss runtime**, dan **UI/pose sementara**; tetap satu file save dan satu API campaign. Event tidak disamakan dengan item fisik, dan cache runtime bukan sumber kebenaran tambahan.
2. CP1–CP7 adalah label desain; gunakan checkpoint ID bernama `PHASE1_CP1` ... `PHASE1_CP7` agar tidak bentrok dengan babak lain. Definisikan anchor posisi/yaw, pintu/collider, mode HP, AI dan objective tiap restore. CP4 per bukti perlu checkpoint aman yang ditentukan, bukan posisi player yang sedang diburu.
3. CP3: top-up 20% hanya jika D06 diterima, lalu simpan satu snapshot. Cutter hanya RAM sampai chain-cut; chain+cutter commit bersamaan. Kegagalan write harus ditangani ketika AI sudah dibekukan; jangan menghidupkan kembali hantu tepat di wajah player. CP setelah chain-cut belum diberi nomor khusus: gunakan checkpoint aman landing L4 dengan receipt `P1_CUT_CHAIN`, bukan CP3 pra-encounter dengan state campuran.
4. CP5: E di altar, tiga ID bukti + Jurnal terkonfirmasi + keris, write sukses **lebih dulu**, lalu ritual RAM. Tidak ada transaksi ekonomi, dokumen baru atau autosave callback selama fight. CP6 satu commit boss+segel+keris. Jika write CP6 gagal, masuk recovery UI aman dengan retry commit atau restore CP5; jangan membagikan hadiah/menjalankan epilog dari state belum tersimpan.
5. Secret pickup melakukan write atomik **sebelum** sukses UI/objek hilang. Autosave setelah mengambil secret pasca-boss mempertahankan CP6/kemajuan epilog dan receipt; jangan menulis checkpoint CP2 hanya karena item ditemukan di L2. Tidak ada merge lintas campaign/backup lama tanpa keputusan eksplisit.
6. `P1_TAPE_PLAY` perlu membedakan **mulai** dan **selesai** secara deterministik agar restart di tengah rekaman tidak memberi epilog sebelum tape selesai. Timer runtime tidak disimpan; reload boleh memutar ulang beat yang belum selesai, tanpa reward ganda.
7. Tulis tabel prioritas input: Menu/mandatory animation > interaksi encounter > UI HP > foto/blitz > interaksi normal. Mode browsing Jurnal tidak otomatis membekukan seluruh dunia. `WATCH_ONLY` harus mengatasi semua gate 0% pada phone/view/framing/shortcut **hanya jika D01 disetujui**. Saat pursuit, tombol UI Stow, Z/M/J/Q/R/V/X, RMB release dan kehilangan fokus juga perlu kontrak; TAB/F lock saja tidak cukup.
8. Frustum Kuntilanak memakai target bounds/anchor toleran dan LOS, bukan `FramesTarget` center +1,45m untuk semua monster. Fatal catch didahulukan dari E ketika jarak ≤1,2m; setelah E valid, freeze 2s dan resolve atomik tanpa frame rentan di antaranya. Kamera bahu/frustum kamera utama tidak boleh memberi watch gratis di balik tembok.
9. K403 tersedia untuk stealth, tetapi dinamo eksplorasi nonaktif selama boss. Emergency arena global harus berupa interaksi yang terjangkau pada baterai nol; exposure 5s tetap aman dari lemparan/Tuyul. Miss/cooldown di window tidak menghabiskan halogen. Stage tidak mundur karena backtracking; altar-ready berbasis jarak hanya kondisi interaksi sementara.
10. Disk format tidak menyimpan dictionary/HashSet mentah atau `Time.time` absolut. Jam gameplay persisten berhenti sesuai kebijakan pause; challenge absent/not-offered dibedakan dari enum OFFERED/SUCCEEDED/EXPIRED. Tentukan perilaku restart pada dinamo 30/45s (default progres sesi boleh hilang sesuai baseline RAM) dan sisa cooldown 120s dipulihkan benar.

### 14.3 Urutan kerja sesudah persetujuan implementasi

**M0 — sinkronisasi kontrak:** putuskan D01/D02/D03/D05/D06/D07/D08/D09; bekukan parameter global/default dan oracle QA. **M1 — integrasi data/routing:** migrasi schema, restore, transaksi dan consumable di sistem existing; Continue P0/P1 tetap aman. **M2 — blockout lengkap:** semua rute naik/pulang, K403/K404 dan volume monster; belum art final. **M3 — slice L1–L2:** AR K102, 644, Tuyul, dinamo, secret; uji save failure dan nol baterai. **M4 — L3:** watch/cutter/chain/input/retry dengan gerak efektif dan batas nol baterai yang disetujui. **M5 — L4 investigasi:** AI, 6 urutan bukti, Jurnal confirm, keris, EVP/hiding dan CP4. **M6 — boss:** CP5 write barrier, tiga halogen, miss/retry, energi global, CP6. **M7 — epilog/exit:** kain/kaset/video, lift L1/L2, secret backtrack, panel motor dan CP7. **M8 — art/audio/performa/QA:** semua 74 kasus + tambahan audit, Windows GPU nyata, baru build ZIP dan upload sesuai AGENTS.

Tidak ada kode, scene, prefab atau config game yang diubah pada audit r1. `Rancangan.MD` dipertahankan; penyelarasan dokumen global di luar scope audit dicatat, bukan ditulis diam-diam.

### 14.4 Identitas data teknis dan aturan yang tidak boleh digandakan

Identifier di bawah adalah kontrak teknis usulan untuk implementasi, bukan fitur yang sudah ada. Nama dapat disesuaikan sekali sebelum kode; bila diganti sesudah save beredar, sertakan migrasi.

| Kategori | ID / aturan authoritative |
|---|---|
| Tiga bukti P1 | `P1_CLAW_402`, `P1_EVP_BATHROOM`, `P1_AURA_KERIS` di `CampaignData.evidence_ids`; receipt `P1_EVIDENCE_*` pada registry adalah notifikasi pemicu, bukan counter bukti kedua. |
| Keris | `P1_KERIS` di inventory berarti **sedang dibawa**; `relic_ids` bila digunakan berarti **pusaka telah ditemukan/terdaftar** dan tidak dihapus ketika dipasang di altar. Jangan memakai keduanya sebagai dua nilai kepemilikan yang harus ditebak. |
| Kunci dan cutter | `P1_LIFT_KEY` persisten; `P1_BOLT_CUTTER` hanya attempt RAM sebelum rantai selesai, baru dimasukkan pada commit chain yang sama. |
| Powerbank P1 | Instance `P1_POWERBANK_ARRIVAL` (jika D06 disetujui) dan `P1_POWERBANK_MAILBOX_207`; tipe/kegunaan ditangani daftar consumable pada inventory existing. Receipt sumber mencegah grant ulang; instance dihapus ketika digunakan/dicuri, receipt tidak dihapus. |
| Secret | `SECRET_P1_NEGATIF_TARI` di campaign `secret_items`, bukan evidence/relic; hitungan lokal memakai Contains ID tepat, global memakai lima ID sah dari rancangan kampanye. |
| Segel lokasi | Satu ID teknis konsisten untuk Rusunawa, misalnya `SEAL_RUSUNAWA_GRIYA_ASRI`, hanya ditambahkan saat CP6. Poin tiga lampu bukan tiga location_seals. |
| Dokumen | Receipt ID stabil untuk log satpam, surat pengelola, catatan Tari, dan kaset; isi tetap arsip dokumen existing. Membaca dokumen clue bukan substitusi bukti. Tape selesai dan epilog selesai memakai receipt terpisah. |
| Challenge | Absent = belum ditawarkan; setelah offered gunakan satu enum OFFERED/SUCCEEDED/EXPIRED, bukan default OFFERED untuk save yang belum mulai live. |

**Kelengkapan QA di luar 74:** uji Continue setiap CP termasuk checkpoint asing, enam permutasi bukti dan Jurnal confirm, seluruh jalur input Stow/app/RMB/remap saat L3, autosave dokumen/pembayaran selama boss, fault injection secret/crossing/chain/CP5/6/7, serta topology/clearance dan frame spike. Rinciannya ada pada laporan audit §7.1; jangan menganggap 74 kasus menutup semua kombinasi ini.
