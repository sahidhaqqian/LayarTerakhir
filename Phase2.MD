# PHASE 2 — DIAGNOSIS TENGAH MALAM

> **Project:** Layar Terakhir · **Babak:** 2 · **Lokasi:** RSUD Lama Sido Waras · **Versi:** 2.2-AUDIT-r1, audit repositori `9ca5f57` dan revisi teknis 23 Oktober 2026 (basis cerita: 15 Oktober 2026; draf masukan bertanggal 24 Oktober dipertahankan sebagai riwayat)
> **Jenis dokumen:** *Single source of truth* untuk naskah, level design, mekanik, implementasi Unity, penyimpanan, dan QA Babak 2.
> **Status:** **Audit statis repositori selesai; dokumen direvisi, implementasi P2 belum dimulai dan belum phase-freeze.** Baseline kode `9ca5f57` sama dengan `origin/main` saat pemeriksaan; draf Phase2 terbaru milik desainer di working tree menjadi masukan, bukan versi lama dari Git. Bukti dan klasifikasi fitur ada di §14–§15. Seluruh **100 QA (QA-01 s.d. QA-100)** telah diperiksa secara spesifikasi, tetapi **100/100 masih `NOT_RUN` untuk gameplay P2**. Audit dokumen tidak membuktikan build, kualitas artistik atau performa lulus.
> **Rujukan lintas babak:** `Phase1.MD` (kontrak global yang sudah ditetapkan di §0.4–§0.5 dan §8.0-nya) dan `Rancangan.MD` (parameter global dan kontinuitas cerita). Bila ketentuan global belum diketahui, jangan membuat asumsi diam-diam; catat sebagai keputusan terbuka (§11.2).

## 0. Cara membaca dan batas keputusan

Dokumen ini menggabungkan **naskah asli Babak 2 (revisi 15 Oktober 2026)**, **acuan mekanik dan jalur rahasia**, spesifikasi Phase2 versi ChatGPT, serta rancangan Claude v2.0-r1; kontrak implementasi mengikuti Phase 1 Codex versi terbaru yang dilampirkan. Dialog, lokasi, urutan enam adegan, angka cerita (suhu −16°C, −24°C, kode 1603, 17 jasad bayi, jumlah penonton, nominal donasi), ID item rahasia, dan mekanik inti dipertahankan. Bagian yang secara logis kontradiktif, ambigu, atau tidak dapat dikode diperbaiki dan **ditandai** (lihat §0.7 untuk daftar lengkap koreksi editorial). Untuk membedakan sumber dari penambahan:

- **[KANON]** = ketentuan yang sudah ada pada naskah 15 Oktober atau pada kontrak global Phase 1, dipakai sebagai acuan resmi babak ini.
- **[KLARIFIKASI]** = penjabaran implementasi untuk menghindari interpretasi berbeda/softlock, tanpa mengubah alur kanon.
- **[USUL BARU]** = tambahan desain, UX, produksi, atau QA; boleh diterima, disetel, atau dihapus sebelum produksi.
- **[PERLU KEPUTUSAN]** = masih memerlukan putusan penulis/desainer atau verifikasi `Rancangan.MD`. Diberi ID `P2-Dxx` (§11.2) agar tidak tertukar dengan D01–D09 pada Phase 1.
- **[BASELINE UJI]** = nilai prototipe agar bisa dikode dan playtest, bukan kanon final.
- **[REVISI EDITORIAL]** = perbaikan teks lore/puzzle/urutan untuk menyambungkan kontradiksi sumber.
- **[DIWARISI P1]** = ketentuan global yang sudah ditetapkan Phase 1 dan berlaku sama di Babak 2 sampai desainer mengubahnya.

**Prioritas bila terjadi benturan:** keputusan eksplisit penulis dalam `Rancangan.MD` dan dokumen desain terbaru > kontrak global Phase 1 §0.4–§0.5 > babak kanon di sini > rekomendasi teknis di sini. Bagian *Naskah Adegan* mempertahankan ucapan/kejadian aslinya; pelaksanaan logikanya mengikuti aturan terperinci pada *Kontrak Implementasi*.

### 0.1 Ringkasan babak (KANON)

| Elemen | Ketentuan |
|---|---|
| Judul | Babak 2 — **Diagnosis Tengah Malam** |
| Lokasi | **RSUD Lama Sido Waras**: IGD dan Koridor Poliklinik (L1), Instalasi Kamar Mayat (basement), Bangsal Bersalin (L2) |
| Waktu pembuka | **14:00 WIB** di Kamar Kos (Safe Hub); berangkat **23:45 WIB**; tiba di IGD **23:50 WIB** |
| Panjang jalur utama | **82 menit**, tidak termasuk pencarian item rahasia dan pengulangan setelah kalah |
| Karakter | **Raka**; Damar hadir lewat rekaman suara dan video tungku; **Dr. Hendra** (dokumen otopsi), **Mbah Suro** (pusaka dan ritual) dan **Lastri** terkait misteri; `Penonton_Terakhir` sebagai antagonis siaran |
| Boss | **Kuntilanak Merah Lastri** (Kuntilanak Merah Ruang Bersalin) |
| Ancaman pendukung | Lastri sendiri dalam tiga mode (menempel di kamar mayat, patroli agresif di L2, boss) dan **jebakan gas formalin** pada laci yang salah. **Tidak ada Tuyul/Genderuwo/ancaman baru lain** di babak ini tanpa keputusan (§11.2) |
| Modul HP | **Selfie Mode** dan **ThermoKlenik.apk** (baru); AR penuh, Jurnal Klenik, EVP Recorder dasar, Blitz sejak Babak 1 |
| Bukti wajib | **Suhu beku Poli 03 (−16°C)**; **aura toples tumbal di Laci C3**; **EVP Inkubator 04** |
| Pusaka | **Gunting Tali Pusar** (Laci C3), **Gelang Identitas Bayi Lastri** (Inkubator 04), **Botol Darah Segel Mbah Suro** (brankas obat Ruang Jaga Suster) |
| Boss fight | Letakkan tiga pusaka di meja operasi; **3 siklus penyegelan**: lacak lewat Selfie → pecahkan **Kabut Sengkala** dengan lampu operasi/UV atau flare → perisai pecah, jendela **5 detik** → **Blitz** valid; **3 Blitz berhasil** menutup pertarungan |
| Rahasia | **Label Pasien Tanpa Nama**, ID `SECRET_P2_LABEL_PASIEN`, bagian **Arsip Saksi N dari 5** (hitungan aktual ID unik; bukan nomor babak) |
| Hasil | **Rekaman suara Damar** dan **video tungku** (8 detik), **segel kedua**, menuju **Pabrik Gula Belanda "Sindang Laut"** |

### 0.2 Alur level dan sasaran waktu (KANON + BASELINE UJI)

| Adegan | Target | Area dan tindakan utama | Keluaran/gate |
|---|---:|---|---|
| 1 | 00–10 menit | Kamar Kos: riset forum di PC, Pasar Gaib, upgrade modul, berangkat | Selfie Mode + ThermoKlenik terbuka; berangkat 23:45 |
| 2 | 10–25 menit | IGD, live stream, Buku Jaga Perawat, Koridor Poli 01–04, ThermoKlenik | **Bukti 1 (−16°C)** → kode **1603** → pintu basement |
| 3 | 25–45 menit | Kamar Mayat kedap suara, Lastri menempel, tutorial Selfie, laci pendingin A1–C4 | **Bukti 2 + Pusaka 1** (Laci C3) + **Kunci Teralis L2**; rahasia opsional D1 |
| 4 | 45–65 menit | L1→L2, tembok bata AR, Inkubator 04 (EVP), Ruang Jaga Suster, Jurnal Deduksi | **Bukti 3 + Pusaka 2 + Pusaka 3** + konfirmasi Jurnal |
| 5 | 65–78 menit | Ruang Operasi: tiga pusaka di meja, tiga siklus penyegelan | **3 Blitz segel** → Lastri tersegel |
| 6 | 78–82 menit | Epilog: chat 310.500, Rp164,8 juta, video tungku, pesan `Penonton_Terakhir` → kembali ke motor → keluar | Panel Arsip → konfirmasi `P2_COMPLETE` → save + `CKREK!` + hitam |

**Jalur kritis:** Kos/PC/Pasar Gaib → berangkat → IGD (live, Buku Jaga) → Poli 03 (−16°C) → keypad **1603** → Kamar Mayat (Selfie, Laci C3 −24°C, Gunting, bukti aura, Laporan Otopsi + Kunci Teralis) → tangga L2 → tembok bata (AR) → Inkubator 04 (EVP + Gelang + rekaman Damar) → Ruang Jaga Suster (brankas obat, Botol Darah) → konfirmasi Jurnal → pintu Ruang Operasi → meja operasi (CP5) → tiga siklus → epilog → turun L2→L1→IGD→motor → `P2_COMPLETE`.

**[BASELINE UJI — alokasi menit bukan pengunci progres]** Adegan 1 (10 menit) memuat PC, forum, belanja, dan tutorial modul; pemain yang cepat dapat selesai dalam 5–6 menit dan itu **bukan bug**. Adegan 3 dan 4 masing-masing 20 menit diisi pengamatan, tekanan Selfie, pemindaian laci, dan penghindaran Lastri, bukan filler. Tinjau ulang distribusi setelah playtest bertanda waktu (§3.4A).

**[KLARIFIKASI — perjalanan pulang]** Setelah `P2_BOSS_WIN`, Lastri berstatus `RESOLVED` permanen; seluruh rute L2→L1→IGD→pelataran→motor aman. Interaksi **`[E] Tinggalkan RSUD`** hanya ada pada **motor Raka di pelataran IGD**. Perjalanan keluar ditargetkan sekitar **60–120 detik** setelah epilog untuk pemain yang tidak mencari rahasia; target 82 menit adalah sasaran playtest, bukan batas waktu paksa.

### 0.3 Pilar pengalaman (KLARIFIKASI)

1. **Investigasi lebih penting daripada combat:** Lastri tidak dapat disegel sebelum tiga bukti, konfirmasi Jurnal, dan tiga pusaka lengkap; Blitz sembarangan tidak berarti apa pun.
2. **Horor yang membalik cara melihat:** ancaman utama **tidak terlihat dengan mata biasa**; pemain belajar memercayai layar Selfie dan sensor suhu, bukan pandangan langsung. Aturan "jangan menoleh" harus diajarkan dulu, baru dihukum.
3. **Ponsel sebagai alat sekaligus beban:** ThermoKlenik dan Selfie menyelesaikan puzzle, tetapi setiap mode memakai baterai dan memaksa pemain mengalihkan layar saat Lastri menempel.
4. **Ketegangan dengan aturan adil:** setiap hukuman fatal punya telegraph audio/visual; hanya dua jenis kegagalan fatal: `POUNCE` (menoleh langsung saat Lastri menempel dan belum terkena cahaya ritual) dan `GRAB` (dibiarkan tanpa pantauan hingga rambutnya menyentuh bahu lalu dicengkeram; di boss termasuk varian diam terlalu lama atau meter out-of-frame penuh); kematian tidak menghapus progres permanen.
5. **Jalur rahasia menghargai rasa ingin tahu, bukan uang:** Arsip Saksi tidak dijual lewat saweran; semua progres utama dapat diselesaikan tanpa donasi atau belanja opsional.
6. **Tragedi manusia di balik hantu:** daftar korban yang kehilangan satu nama, gelang bayi, dan label tanpa nama membuat Lastri terasa sebagai korban yang dijadikan alat ritual, bukan monster acak.

---

### 0.4 Kontrak lintas babak yang mengikat Babak 2 [DIWARISI P1]

Bagian ini telah dicocokkan dengan `Rancangan.MD`, Phase 1 §0.5–§0.7 dan kode `9ca5f57`. Baseline audit lama pada Phase 1 §0.4/§14 bukan status runtime terbaru; sistem yang sudah dibangun dicatat pada §14.1 dokumen ini. Keputusan desainer terbaru mengungguli angka historis.

| Kontrak bersama | Ketentuan untuk Babak 2 |
|---|---|
| Prasyarat masuk | Babak 2 hanya dapat dimulai dari save yang memuat `P1_COMPLETE` dan `SEAL_RUSUNAWA_GRIYA_ASRI`. Build pengembangan boleh menyediakan jalur uji *tanpa* P1, tetapi harus menandainya **dev-only** dan tidak menciptakan segel/uang/inventori palsu di save produksi. |
| Kamera dan kontrol | Pertahankan desain pemain terbaru: OTS + first-person melalui `ShoulderCameraRig`; binding mengikuti `GameInputConfig`. **`V` = ganti perspektif, `X` = Selfie** (naskah 15 Oktober masih `V` untuk Selfie). **Tahan RMB = angkat kamera fisik horizontal dua tangan; lepas = kembalikan pose/perspektif terdahulu** sesuai P1 §0.7. Prompt UI dihasilkan dari binding aktual, bukan di-*hardcode*; jangan membuat kamera/rig input paralel. P2-D01 tetap mencatat perbedaan sejarah sumber, bukan membuka ulang keputusan binding P1. |
| Energi HP global | Kamera normal **0,05 poin persen/detik**, AR/UV **0,10 total/detik**, foto bukti **1%**, Blitz **15%**, cooldown **6 detik**, powerbank besar **+80 poin**, kecil kos **+35**. Overlay tidak menumpuk drain: profil mode aktif saja. Profil **Selfie** dan **ThermoKlenik** belum ada di global; baseline P2 ada di §8.0 (P2-D07). HP disimpan/layar mati menghentikan drain dan tidak memutus live. |
| Baterai nol | Default global: layar mati, AR/EVP/Blitz/Selfie/Thermo tidak berfungsi, bukan Game Over. **Tidak ada `WATCH_ONLY` gratis** di P2 (pengecualian itu tidak pernah diratifikasi). Pemulihan lewat dinamo/generator fisik (§8.0), bukan energi otomatis. |
| Boss dan energi | Blitz 15%/6 detik. Sumber cahaya ritual (lampu/flare) tidak habis kecuali **setelah hit sukses** (lampu) atau **setelah dijatuhkan** (flare, item). **Titik daya darurat arena** fisik tersedia saat jeda pemulihan; tidak ada Blitz bertenaga lampu pada 0%. |
| Ritual dan Jurnal | **3 bukti unik + konfirmasi eksplisit Jurnal + 3 pusaka** sebelum damage penyegelan/ritual. Tiga scan tidak otomatis menjadi konfirmasi. `P2_JOURNAL_CONFIRMED` adalah receipt satu kali. |
| Sanity | **Sanity gameplay mulai Babak 4** (`Rancangan.MD`). P2 mempertahankan nilai Sanity yang diwariskan; **tidak menguras, meregenerasi, atau mengubah radius/efek berdasar Sanity**. Tidak ada sistem health/Sanity paralel. `Kemenyan Bakar` hanya disimpan; **tidak punya fungsi di P2**. |
| Kanon lokasi | Lima gerbang = Rusunawa, **RSUD**, Pabrik Gula, Karang Setra, Pasar Setan; **empat segel lokasi dari Babak 1–4**. P2 menambah segel **kedua**. Pesan penutup "Dua segel Panca-Sengkala telah kau buka" konsisten (P2-D04 untuk diksi *Panca*). |
| Arsip Saksi | ID P2 **`SECRET_P2_LABEL_PASIEN`**; ID unik dalam campaign, autosave atomik **sebelum** indikator sukses, tetap ada saat kematian/reload checkpoint terkini. Hitungan `N/5` memakai **jumlah aktual ID unik** (1/5 jika rahasia P1 belum diambil, 2/5 jika sudah). |
| Uang | Rp164.800.000 adalah **milestone naratif kumulatif**, terpisah dari saldo (pola Rp78,2 juta di P1). Donasi `Penonton_Terakhir` adalah **dana terikat** dan tidak masuk saldo belanja; hanya donasi manusia yang sah masuk `wallet_legitimate`. Jangan menambah nominal milestone ke wallet untuk menutup selisih (§8.1A). |
| Save dan event | Perluas **`CampaignState/CampaignData` yang sama**, file `.json/.tmp/.bak` yang sama; list ID unik; transaksi atomik dengan rollback. Tidak ada save/event/AR/inventori paralel. Versi schema: P2-D10. |
| Aset | **Aset wajib existing atau unduhan internet berlisensi.** Tidak membuat model, gambar, tekstur, atau gerak original sendiri; konversi/optimasi impor dan retargeting animasi existing diperbolehkan. Sumber/lisensi dicatat (§7.5). |
| Target performa | 60 FPS global tetap sasaran; tier minimum 720p Low 30 FPS pada Iris Xe adalah usulan D07 Phase 1 yang belum diratifikasi; P2 mengikutinya (P2-D11). |

**Pagar keputusan:** Ikuti kontrak global/default di atas. P2-D01 sampai P2-D15 (§11.2) dan keputusan Phase 1 yang berlaku global (D03 ledger, D05 kaset, D06 carry-over, D07 tier performa, D08 retry boss) harus tercatat sebelum freeze bagian terkait. Mekanik inti cerita tetap; angka prototipe boleh diuji setelah milestone implementasi diizinkan. Jangan membaca label "siap blockout" sebagai izin coding.

### 0.5 Kontrak kerja P2 [DIWARISI GLOBAL + HASIL AUDIT]

**Arahan terbaru Phase 1 (23 Oktober 2026):** "tahan dulu sementara pakai global dulu aja nanti kalau gak sesuai aku ubah". Arahan itu ditulis untuk Phase 1. P2 memakai aturan bersama yang memang ditetapkan `Rancangan.MD`; **tidak mewarisi grant baterai Prolog→P1** atau menganggap proposal P2 telah diratifikasi. Pengguna mengizinkan audit/revisi dokumen ini, bukan coding. Konsekuensinya:

- Pakai **default global** untuk energi, ekonomi, Sanity, dan retry; angka P2 di §8.0 adalah baseline yang mudah diubah di satu konfigurasi.
- Tidak mengurangi kualitas visual, atmosfer, atau gameplay demi mengejar checklist.
- Retry boss memuat **snapshot CP5** (baterai persis snapshot, tiga sumber cahaya utuh, tiga pusaka belum terpasang); CP6 disimpan atomik setelah kemenangan; semua penulisan biasa ditolak selama attempt.
- Status pembayaran/ekonomi mengikuti receipt save; tidak dilunaskan atau diisi diam-diam untuk menyesuaikan monolog atau overlay.
- Nubuat kaset 1998 masih ditahan (D05); P2 tidak memutar kaset dan tidak menambah klaim nubuat.
- Babak berakhir pada konfirmasi motor dan CP7; simpan tujuan **Pabrik Gula Belanda Sindang Laut** tanpa membuat scene Babak 3. Video tungku Damar memakai set rekaman non-playable di dalam scene P2.
- **Aset wajib existing atau unduhan internet berlisensi**; Phase 2 memakai pustaka lingkungan rumah sakit tersendiri (bukan furnitur/permukaan Rusunawa), UV skala nyata, tekstur PBR, dan dressing sesuai fungsi ruang. Sistem pemain/HP, karakter desainer, dan motor yang dibawa dari babak sebelumnya tetap digunakan.
- Target 60 FPS global tetap dipertahankan. Headless/Linux software rendering tidak menggantikan pengukuran Windows pada GPU sasaran.

### 0.6 Status dokumen — audit selesai, implementasi belum dimulai

Audit membaca seluruh spesifikasi ini, `Phase1.MD`, `Rancangan.MD`, kode terkait, Build Settings, scene Kos/Rusunawa, mixer serta inventaris aset terarah. Ada empat scene build, **belum ada scene RSUD, runtime Phase2, Thermo atau render Selfie produksi**. Sistem P1 sudah tersedia tetapi penerimaan P1 belum tuntas; jangan mewarisi status lulus yang belum ada. Bukti ada di §14–§15 dan [laporan audit](Audit/PHASE2_REPOSITORY_AUDIT.md).

Audit ini **hanya mengubah dokumen**. Tidak ada Unity Editor/PlayMode/build dijalankan; tidak ada unduhan atau perubahan aset. Kelayakan teknis dibedakan dari persetujuan kreatif, kesiapan aset, dan penerimaan gameplay. QA §10 diperiksa satu per satu di §15.3, semuanya tetap `NOT_RUN`.

### 0.7 Koreksi editorial atas naskah 15 Oktober 2026 [REVISI EDITORIAL]

Setiap baris di bawah mengubah, memperjelas, atau memindahkan sesuatu dari naskah asli. Dialog dan angka cerita tidak diubah kecuali tertulis di sini. **Periksa daftar ini lebih dulu sebelum menetapkan kanon final.**

| # | Naskah 15 Oktober | Masalah | Perlakuan di dokumen ini |
|---|---|---|---|
| E1 | Selfie = `[V]` | Kontrak P1 §0.4/§0.7: `V` perspektif, `X` Selfie, RMB ditahan untuk pose HP | **Instruksi gameplay dalam naskah diselaraskan menjadi `[X]`; kutipan asli `[V]` hanya tersisa sebagai bukti sejarah perubahan.** Prompt implementasi tetap berasal dari `GameInputConfig`; **P2-D01** |
| E2 | Log Perawat: kode = "Suhu Ruangan Tempat Pasien **Terakhir Meninggal**" + "Nomor Ruang Poliklinik tempat jenazah pertama diperiksa" | Lastri meninggal di Ruang Bersalin L2, yang tidak dapat dipindai pada tahap ini; angka 16 dan 03 tidak otomatis berasal dari satu ruang | Diperjelas: **suhu ruang poliklinik tempat pasien terakhir itu pertama kali diperiksa**, digabung nomor poliklinik yang sama (§1 Adegan 2). Angka 16/03 tidak berubah |
| E3 | "Jangan membuka laci yang suhunya **di atas** −20°C" | "Di atas" ambigu (−10 lebih tinggi nilainya tetapi lebih hangat) | Diubah menjadi **"lebih hangat dari −20°C"**; tabel 12 laci di §3.3 |
| E4 | `TUMBAl` | Salah ketik | Menjadi **TUMBAL** |
| E5 | `BukuJaga` | Salah ketik | Menjadi **Buku Jaga** |
| E6 | "Arsip Saksi **2/5**" pada item rahasia | Bergantung pada rahasia P1 sudah diambil atau belum | **N/5 dari ID unik aktual**; contoh 1/5 atau 2/5 (§5.2) |
| E7 | Donasi `Penonton_Terakhir` Rp666.000: "Sahabatmu meninggalkan sesuatu di **Kamar Mayat** bawah tanah" | Rekaman Damar ditemukan di Inkubator 04 (L2), bukan kamar mayat | Baseline: tambah **jejak Damar non-gating** di kamar mayat (§3.3); alternatif ubah teks donasi. **P2-D05** |
| E8 | Viewer awal **42.300** setelah P1 berakhir pada **142.000** | Tampak turun | Ditafsirkan sebagai penonton serentak saat siaran baru dibuka, bukan puncak kumulatif (P2-D09) |
| E9 | "Panca-Sengkala" vs empat segel lokasi | *Panca* = lima; segel lokasi empat | Dijembatani: *Panca-Sengkala* = nama **ritual lima gerbang**; segel lokasi tetap empat. **P2-D04** |
| E10 | Adegan 6 langsung `CKREK!` → hitam | Tidak ada akses mengambil rahasia D1 di basement; berbeda dengan pola P1 §8.4 | Epilog mengembalikan kontrol tanpa fade; `CKREK!` + hitam dipindah ke **konfirmasi `Lanjutkan` di motor**. **P2-D06** |
| E11 | Lastri memecahkan "kaca inkubator di sekeliling ruangan" **di Ruang Operasi** | Inkubator ada di Ruang Inkubator L2 | **Prop wajib:** 4 inkubator rusak berjejer di dinding barat Ruang Operasi (§3.6, §7.2) |
| E12 | Tombol `[GO LIVE]` | P1 memakai `[MULAI LIVE]` | Label UI konsisten dengan P1; **P2-D14** |
| E13 | Acuan: "berjalan maju"; naskah: "memutari meja" | Dua deskripsi gerak | Disatukan: **berjalan maju mengitari meja** (§4.4) |
| E14 | Jurnal Bukti 1 tertulis "−16°C **s/d** −24°C" | Dua angka suhu pada satu bukti | Bukti 1 = **satu ID** (`P2_FREEZE_POLI03`, −16°C); −24°C pada Laci C3 hanya **petunjuk puzzle**, bukan bukti kedua (§5.1) |
| E15 | Dr. Hendra vs "dokter kepala" pada artikel forum | Tidak disebut sama | **Hubungan belum ditetapkan**: laporan tetap Dr. Hendra, artikel tetap dokter kepala; jangan menyamakan identitas tanpa persetujuan. **P2-D04** |
| E16 | Arsip Saksi: "buka dengan kunci arsip yang tergantung di ruangan" tanpa lokasi eksplisit | Kunci bisa tak terlihat | Kunci tergantung di papan kunci gudang, terlihat dari pintu (§3.3, §5.1) |
| E17 | Brankas obat "terbuka lewat mode AR" tanpa mekanisme | Tidak dapat dikode/diuji | Mekanisme **gema masa lalu**: AR menampilkan siluet suster 2001 menekan angka; Jurnal mencatat; jalur deduktif cadangan lewat catatan suster (§1 Adegan 4, §3.4) |
| E18 | Kos disebut **4×3 m** pada naskah 15 Oktober | Nama lama 4×3/angka draf 6×4,5 tidak mencerminkan bidang lantai tersimpan 5×3,8 m; desainer mengonfirmasi Kos memang diperbesar | Gunakan ulang scene dan transform aktual Kos yang sudah disetujui, jangan buat prefab 4×3 baru; pertahankan pembesaran yang dikonfirmasi desainer; **P2-D15** |

---


### 0.8 Hasil perbandingan dua spesifikasi dan kontrak gabungan [AUDIT DOKUMEN]

**Yang dipilih dari Claude v2.0-r1:** naskah per adegan dengan errata E1–E18, denah detil kamar mayat dan ruang operasi, AI Lastri dengan telegraph serta masa belajar, puzzle brankas alternatif `2604`, tiga sumber cahaya lingkungan, ledger donasi terikat, dan checkpoint **CP5→CP6→CP7** konsisten dengan Phase 1. **Yang dipilih dari Phase2 v1.0 ChatGPT:** kontrol RMB/OTS/FP yang paling baru dari Codex, validasi konflik ukuran Kos, matriks pengujian yang lebih luas (100 kasus), pembuktian baterai dengan input 0–100%, pelacakan sumber aset/penempatan, serta pemisahan hasil tes dan klaim implementasi. **Yang tidak dibawa:** checkpoint alternatif CP6→CP7→CP8 dan lampu A/B yang dipakai ulang setelah hit sukses; keduanya berbenturan dengan model tiga sumber lampu + satu hit per sumber pada dokumen Claude.

**Tiga hal yang tidak boleh bercabang di kode:** (1) **checkpoint CP5 sebelum altar / CP6 kemenangan / CP7 keluar**, tanpa CP8; (2) **LO, UV-N, UV-S** masing-masing korslet **setelah hit sukses**, boleh diulang bila miss, sehingga flare opsional; (3) **`V` perspektif, `X` Selfie, RMB tahan untuk HP**. Seluruh event P2 dan nama parameter P2 bersifat usulan, sedangkan objek/kelas yang sudah ada di repo harus menang setelah audit berbukti file. Jangan membiarkan kata `KANON` pada angka cerita menjustifikasi perubahan kontrak P1 atau keputusan yang belum disetujui.

**Penandaan kanon vs baseline:** angka suhu, identitas yang eksplisit, jumlah bukti/pusaka, tiga siklus, jendela 5 detik dan angka cerita tetap kanon; hubungan Dr. Hendra/dokter kepala belum ditetapkan; `2604`, label gerbang, tata letak 10×8m, kecepatan AI, meter sentuhan, titik charger, pembelian sekali per kunjungan, dan perjalanan keluar lewat motor adalah **klarifikasi/usulan** dengan status masing-masing pada §0.7 dan §11.2. `Rancangan.MD` dan repositori sudah diperiksa pada audit ini; kebutuhan implementasi dan batas bukti ada di §14–§15.

---

## 1. Naskah enam adegan (KANON; bagian yang diubah diberi catatan editorial)

> **[REVISI EDITORIAL]** Beberapa teks dokumen/dialog di bawah dikoreksi atau diperjelas untuk menutup kontradiksi (lihat §0.7). Perubahan itu bukan kutipan verbatim naskah 15 Oktober; setiap perubahan bermakna punya nomor `E#` yang dapat dilacak. Catatan `[KLARIFIKASI]` tidak mengubah dialog; ia menjelaskan perilaku yang harus dikode.

### ADEGAN 1: Tahap Siang Hari — Kamar Kos Raka / Safe Hub (Menit 00:00 – 10:00)

**Kondisi Awal:** Pukul **14:00 WIB** (siang hari). Raka terbangun di **scene Kamar Kos Raka yang sudah menjadi Golden Scene**, bukan dibuat ulang dengan ukuran 4×3 meter dari naskah lama. **Klarifikasi desainer saat audit:** Kos memang sudah diperbesar karena sebelumnya terlalu kecil; pertahankan hasil pembesaran itu. Pemeriksaan statis scene menemukan lantai utama berskala **5×0,1×3,8 m**, collider unit dan parent berskala satu; ini ukuran bidang lantai tersimpan, bukan pengukuran luas bersih atau playtest. Nama objek `Floor_4x3m` historis dan angka 6×4,5 bukan alasan mengubah geometri (P2-D15); semua furnitur, pintu, PC, dan motor mengikuti scene yang sudah disetujui. Suara bising kendaraan kota terdengar samar dari luar jendela. Raka pulih setelah beristirahat; indikator Sanity baru diperkenalkan pada Babak 4. **[KLARIFIKASI]** Inventori, uang tersedia, baterai, dan upgrade berasal dari save Babak 1 yang sebenarnya; tidak ada hibah otomatis di pintu masuk babak selain yang tertulis di §8.0 (P2-D02).

#### 1. Riset Forum Urban Legend di PC

* **Objektif Layar:** `Buka PC untuk meriset lokasi kedua dan meng-upgrade modul Smartphone.`

* Pemain duduk di meja komputer rakitan dan membuka peramban menuju forum *deep-web* lokal tempat Damar biasa mencari bahan konten.

> **TEKS ARTIKEL FORUM (TRAGEDI RSUD SIDO WARAS - 2001):**
> "RSUD Lama Sido Waras ditutup paksa tahun 2001 setelah ditemukan 17 jasad bayi dan seorang pasien bernama **Lastri (26)** tewas kehabisan darah di Ruang Bersalin Lantai 2. Dokter kepala saat itu diduga bersekutu dengan dukun **Mbah Suro** untuk melakukan ritual penumbalan janin (*Panca-Sengkala*).
> **Peringatan bagi penjelajah:** Arwah Lastri berubah menjadi **Kuntilanak Merah**. Ia selalu berdiri tepat di belakang punggung korbannya. Kamu tidak akan bisa melihatnya dengan mata biasa atau kamera belakang—**gunakan cermin atau kamera depan (Selfie)** untuk melihat apakah rambut merahnya sudah menyentuh bahumu."

* **Raka (Monolog Suara):** *"Berdiri tepat di belakang punggung... dan cuma kelihatan dari kamera depan? Gila. Untung Damar sempat bikin cetak biru modul sensor suhu sama pemancar Blitz lensa depan di komputer ini."*

**[KLARIFIKASI]** Artikel forum adalah **dokumen bonus bacaan** (tidak bernomor Dokumen #1–#4) dan tersimpan di tab Dokumen Jurnal setelah dibaca. Tiga fakta di dalamnya dipakai ulang oleh puzzle dan harus tetap dapat dibaca ulang dari Jurnal: **Lastri (26)** dipakai sebagai petunjuk cadangan brankas obat (§3.4), **17 jasad bayi** menjadi petunjuk rahasia D1, dan kalimat **"rambut merahnya sudah menyentuh bahumu"** adalah petunjuk bahwa penantian tanpa pantauan berbahaya (§4.2). Membaca artikel tidak menjadi syarat gerbang mana pun; pemain yang melewatinya tetap diajari aturan inti lewat peringatan HP di kamar mayat (Adegan 3).

#### 2. Belanja Toko Online Klenik & Upgrade Modul Smartphone

Pemain membuka tab **Pasar Gaib (Toko Online Klenik)** menggunakan saldo saweran yang terkumpul dari malam sebelumnya:

1. **Upgrade Wajib (Otomatis Terbuka di Menu Modul HP):**
* **Fitur Kamera Depan (`Selfie Mode` — `[X]` menurut kontrak P1, prompt dinamis):** Membalik sudut pandang layar HP ke arah wajah Raka dan area tepat di belakang punggungnya.

* **Aplikasi `ThermoKlenik.apk` (Tombol `[T]`):** Sensor pendeteksi suhu dingin ekstrem (dalam satuan derajat Celcius) untuk melacak jejak arwah dan titik persembunyian pusaka.

2. **Belanja Persediaan Ekspedisi:**
* **2x Baterai Cadangan / Powerbank** (paket Rp500.000).

* **2x Kemenyan Bakar** (paket Rp350.000 — disimpan untuk pengenalan Sanity di Babak 4).

* **3x Flare Merah Darurat** (paket Rp750.000 — sumber cahaya lempar untuk membakar *Kabut Sengkala* di fase klimaks).

* Setelah selesai berbelanja, pemain berinteraksi dengan kunci motor di meja → memilih **`[BERANGKAT KE RSUD SIDO WARAS - PUKUL 23:45 WIB]`** → *Fade to Black*.

**[KLARIFIKASI — aturan Pasar Gaib]**

* Dua upgrade wajib **tidak dijual**: keduanya terbuka otomatis begitu `P2_FORUM_READ` atau begitu pemain membuka menu Modul HP pertama kali di Kos (mana yang lebih dulu); konfirmasi keberangkatan memiliki fallback unlock yang sama bila keduanya dilewati, supaya jalur utama tidak bergantung pada uang atau pada membaca forum. Event `P2_UPGRADE_UNLOCK` adalah receipt satu kali.
* Tiga paket belanja bersifat **opsional**. Tiap paket dapat dibeli **sekali** untuk persiapan P2 dalam campaign [BASELINE UJI]; harga mengikuti naskah (total bila semuanya diambil: **Rp1.600.000**). Pembayaran hanya dari `wallet_legitimate` (donasi manusia yang sah); dana `Penonton_Terakhir` yang terikat tidak dapat dibelanjakan (§8.1A).
* Jika saldo kurang, paket ditampilkan **tidak aktif dengan harga terlihat**, tanpa paket gratis pengganti. Seluruh jalur wajib dapat diselesaikan tanpa membeli apa pun (QA-01); satu-satunya akibat tidak membeli adalah tekanan sumber daya yang lebih tinggi (powerbank) dan hilangnya opsi flare pada boss (lampu lingkungan tetap cukup untuk tiga siklus).
* **Powerbank** Pasar Gaib bertipe **besar (+80 poin)** [BASELINE UJI; konfirmasi tipe]; dua unit masuk inventori sebagai **instance berbeda** dengan ID sumber unik. **Kemenyan Bakar** masuk inventori dengan tooltip bahwa kegunaannya belum diketahui; **tidak dapat dipakai** di P2 dan **tidak boleh dikonsumsi** oleh sistem apa pun di babak ini. **Flare** dapat dipakai hanya lewat aksi `[G]` saat boss aktif; di luar boss `G` tidak berfungsi dan flare tidak terkonsumsi (§2.1, §8.1).
* Pembelian dikomit **atomik** bersama pengurangan saldo dan penambahan inventori (§6.4). Memuat ulang di tengah belanja tidak menggandakan barang maupun memotong saldo dua kali.
* **Pengisian daya di Safe Hub:** interaksi sadar `E` pada colokan meja PC mengisi HP sampai penuh **sekali untuk persiapan P2 dalam campaign**; baterai dan receipt `P2_KOS_CHARGED` ditulis atomik. Pemain boleh menolak colokan dan berangkat dengan baterai aktual, termasuk 0%; dinamo IGD tetap tersedia. Berangkat, Continue, retry dan checkpoint **tidak** memicu charge. Durasi animasi pengisian adalah baseline, bukan grant pada transisi (P2-D02).

---

### ADEGAN 2: Lobi IGD & Poliklinik Lantai 1 — Investigasi Suhu Beku (Menit 10:00 – 25:00)

**Kondisi Awal:** Pukul **23:50 WIB**. Raka berdiri di depan pintu kaca geser **Instalasi Gawat Darurat (IGD) RSUD Lama Sido Waras** yang pecah dan dipenuhi brankar berkarat. Bau obat antiseptik busuk dan amis darah tercium pekat. **[KLARIFIKASI]** Pukul 23:59 adalah batas yang diucapkan `Penonton_Terakhir` di akhir Babak 1; Raka tiba lebih awal dan tidak ada timer jam nyata (§11.2).

#### 1. Menyalakan Siaran & Intervensi `Penonton_Terakhir`

* Pemain menekan **`[TAB]`** untuk mengangkat Smartphone dan menekan **`[GO LIVE]`** (label UI mengikuti P2-D14).

* Indikator penonton langsung menyentuh **[LIVE | 👁️ 42.300 Penonton]**.
* **Raka (Dialog Suara ke Kamera):** *"Ekspedisi segel kedua. Gue udah di RSUD Sido Waras. Buat kalian yang nanya kabar Damar... dia masih ditahan sama bajingan yang nyawer gue semalam. Malam ini gue udah pasang sensor suhu sama kamera depan di HP."*
* **Rentetan Live Chat #1:**
* `@dokter_jaga_malam`: *"Merinding parah, itu RS Sido Waras yang ruang mayatnya di-seal polisi tahun 2001 kan?!"*
* `@kunti_bogor`: *"Bang Raka jangan lupa sering-sering nyalain kamera selfie bang, rumornya penunggu situ suka nempel di punggung!"*

* `Penonton_Terakhir` **[DONASI Rp666.000]**: *"Selamat datang di bangsal kelahiran kembali, Raka. Sahabatmu meninggalkan sesuatu di Kamar Mayat bawah tanah. Tapi hati-hati... Lastri tidak suka dipunggungi."*

**[KLARIFIKASI]** Donasi Rp666.000 adalah **peristiwa cerita** dari sumber terikat: ia tampil sebagai notifikasi, tidak menambah `wallet_legitimate`, dan tidak diproses ulang saat reload. Kalimat "meninggalkan sesuatu di Kamar Mayat" dipenuhi oleh **jejak Damar** pada pintu terowongan servis kamar mayat (§3.3, E7); keputusan akhir ada di P2-D05. `P2_START_LIVE` hanya perlu `Smartphone` dapat dipakai; live tidak diwajibkan sebelum masuk Poliklinik (anti-softlock), tetapi Chat #1 dan donasi memberi pemain dua petunjuk lingkungan (Poli 03, bahaya punggung) yang juga tersedia di lingkungan (§3.2).

#### 2. Tutorial Aplikasi `ThermoKlenik` & Membuka Pintu Basement

Pemain menyusuri Lobi IGD menuju **Koridor Poliklinik Dalam**. Di ujung lorong terdapat pintu besi bertuliskan **"INSTALASI KAMAR MAYAT (BASEMENT)"** yang terkunci dengan **Panel Keypad Elektronik 4 Digit**, serta tangga menuju **Bangsal Bersalin Lantai 2** yang terhalang teralis besi.

* Di meja resepsionis IGD, pemain menemukan **Buku Jaga Perawat Malam (Item Dokumen #1)**:

> **TEKS DOKUMEN (LOG PERAWAT - 11 OKTOBER 2001):**
> "Kode pintu Kamar Mayat selalu diganti mengikuti **suhu ruang poliklinik tempat pasien terakhir itu pertama kali diperiksa (tanpa tanda minus)**, lalu digabung dengan **nomor ruang poliklinik yang sama**. Catatan: pasien terakhir malam itu seorang ibu hamil, Lastri, yang diperiksa di poli kandungan. Sejak ia meninggal, ruang poliklinik itu suhunya selalu membeku di bawah nol derajat."

> **[REVISI EDITORIAL — E2]** Teks asli: "Suhu Ruangan Tempat Pasien Terakhir Meninggal (tanpa tanda minus), lalu digabung dengan Nomor Ruang Poliklinik tempat jenazah itu pertama kali diperiksa". Lastri meninggal di Ruang Bersalin L2 (artikel forum), tempat yang belum dapat dipindai di tahap ini, sehingga kalimat lama membuat angka suhu dan angka ruang tampak berasal dari dua ruangan berbeda. Versi revisi menjadikan **Poli 03** satu-satunya sumber kedua angka. Angka kode **1603** tidak berubah. Frasa "poli kandungan" ditambahkan agar Poli 03 dapat dideduksi tanpa chat.

* **Cara Menyelesaikan Puzzle Keypad Kamar Mayat (Mekanik `ThermoKlenik`):**
1. **Instruksi Layar:** `Tekan [T] saat mengangkat Smartphone untuk membuka aplikasi ThermoKlenik.`

2. Layar HP menampilkan indikator termometer digital (suhu normal lorong: `24°C`).
3. Pemain berjalan memeriksa 4 ruang poliklinik (`Poli 01` sampai `Poli 04`). Saat melewati depan pintu **Poli 03 (Kandungan)**, kaca jendela ruangan tampak membeku oleh bunga es dan sensor `ThermoKlenik` berbunyi bip cepat menunjukkan suhu ekstrem **`-16°C`**!
4. Di layar Jurnal `MataBatin.apk`, kotak **`[✓] BUKTI KLENIK 1/3: SUHU BEKU EKSTREM (-16°C)`** otomatis tercentang!

5. Pemain menggabungkan angka suhu (`16`) dan nomor ruang (`03`) → memasukkan kode **`1 - 6 - 0 - 3`** pada keypad pintu Basement → pintu Kamar Mayat terbuka!

**[KLARIFIKASI — Poli 01–04 dan keypad]**

* Papan jadwal poli di dinding koridor memuat **Poli 03 = Kandungan & Kebidanan** (tanpa tanda panah HUD). Poli 01/02/04 (mis. Umum, Anak, Gigi) membaca suhu normal sehingga hanya satu ruangan yang beku (§3.2).
* **Bukti 1** valid ketika sensor membaca Poli 03 stabil **≥1,5 detik** dalam jarak ≤3 m dari kaca jendela; melintas sekilas tanpa pembacaan stabil tidak menghitungnya. Ia mencatat **satu ID** (`P2_FREEZE_POLI03`) dan tidak dapat ditambah oleh pembacaan ulang (QA-03). Tanda minus tidak dimasukkan ke keypad (hanya digit `1603`).
* Salah kode memberi umpan balik non-hukuman (bunyi *buzzer* dan lampu merah singkat); tidak ada penalti, alarm, atau pengusiran. Pemain yang melewatkan seluruh chat tetap dapat menderivasi `1603` dari log, papan poli, dan sensor (QA-02, QA-38).
* Pintu Poli 03 membeku dan **tidak dapat dimasuki** (bukan gate); interior terbaca dari kaca (kartu rekam medis berinisial L.). Teralis tangga L2 membutuhkan **Kunci Teralis Lantai 2** dari kamar mayat dan tidak dapat dibuka dengan keypad.

---

### ADEGAN 3: Kamar Mayat & Labirin Kedap Suara — Teror Kamera Selfie (Menit 25:00 – 45:00)

**Kondisi Awal:** Raka menuruni tangga menuju **Basement Kamar Mayat**. Dinding ruangan dilapisi busa peredam akustik yang rusak dan lembap—begitu pintu tertutup, **seluruh suara dari luar hilang total (Efek Ruangan Kedap Suara)**: yang terdengar hanya suara napas Raka sendiri, detak jantung, dan dengung kulkas jenazah. **[KLARIFIKASI]** Pintu basement dapat dibuka dari dalam tanpa keypad (tuas keluar); efek kedap suara mematikan bus suara luar, **bukan** mematikan telegraph Lastri, dengung kulkas, ataupun suara gameplay penting (§4.3A).

#### 1. Kemunculan Perdana Kuntilanak Merah & Tutorial Kamera Depan (`Selfie Mode`)

Saat Raka berjalan di lorong sempit di antara meja-meja otopsi stainless steel, tiba-tiba hembusan uap dingin muncul dari belakang leher Raka.

* **Peringatan Sistem di Layar HP (`MataBatin.apk`):**
> **`[PERINGATAN ANOMALI OPTIK: ENTITAS TIDAK TERLIHAT DI KAMERA BELAKANG! TEKAN [X] UNTUK MENGAKTIFKAN KAMERA DEPAN (SELFIE MODE) DAN PANTAU BELAKANG PUNGGUNGMU!]`**

* **Eksekusi Gameplay (Mekanik *Selfie Mode*):**
* Pemain menekan **`[X]`**. Layar HP berubah menampilkan wajah Raka yang pucat berkeringat dengan latar belakang lorong di belakang punggungnya.

* **Scripted Scare #1:** Di layar kamera Selfie, tepat 1 meter di belakang bahu kiri Raka, tampak sosok **Kuntilanak Merah (Lastri)** dengan gaun rumah sakit berlumuran darah merah pekat dan rambut kusut basah sedang melayang mengikuti langkah Raka sambil tersenyum lebar!
* **Aturan Unik Kuntilanak Merah (Wajib Dipatuhi Pemain):**
* Di fase investigasi ini ia **100% kebal** (belum bisa disegel).

* **JANGAN PERNAH MEMUTAR MOUSE 180 DERAJAT UNTUK MENOLEH LANGSUNG KE BELAKANG!** Jika pemain menolehkan kepala secara langsung tanpa lewat layar Selfie saat ia menempel di belakang, Lastri langsung menerkam wajah Raka (*Instant Game Over*).
* Untuk melepaskan tempelannya, pemain harus **tetap berjalan ke depan sambil menatap layar Kamera Selfie (`[X]`)** dan menjaga agar sosok Kuntilanak Merah tetap berada di dalam *frame* kamera depan selama 5 detik hingga bayangannya memudar mundur, ATAU segera masuk ke dalam lemari besi/kolong meja otopsi dan mematikan layar (`[F]`).

> **[KLARIFIKASI — aturan fair-horror untuk adegan ini]** Naskah menetapkan dua hal fatal: (1) menoleh langsung saat Lastri menempel dan belum terkena cahaya ritual; (2) secara implisit, kehadirannya yang dibiarkan tanpa pantauan (petunjuk forum "rambut merahnya sudah menyentuh bahumu"). Aturan (1) diajarkan **sebelum** dihukum oleh peringatan HP di atas dan oleh scripted scare yang tidak mematikan; kehadirannya selama *Scripted Scare #1* dan sampai lepas pertama **atau 30 detik gameplay pertama, mana yang lebih dulu, tidak dapat membunuh** (jendela belajar, §4.2). Aturan (2) dirinci sebagai *meter sentuhan rambut* pada §4.2 dengan telegraph audio dan visual [BASELINE UJI, P2-D13]. Pengaturan lengkap, ambang sudut, dan toleransi ada di §3.3A dan §4.2.

* **Rentetan Live Chat #2 (Panik Melihat Layar Selfie Raka):**
* `@windah_bocil`: *"ANJIR DI BELAKANG PUNGGUNG LU BANG ADA KUNTI MERAH NEMPEL!"*
* `@suster_keramas`: *"JANGAN NOLEH KE BELAKANG BANG RAKA! LIAT DARI KAMERA DEPAN AJA TERUS JALAN KE DEPAN!"*

* `Penonton_Terakhir` **[DONASI Rp1.333.000]**: *"Lihat betapa dekatnya dia dengan lehermu. Cari laci pendingin tempat bayinya disimpan, atau kau yang akan masuk ke laci berikutnya."*

#### 2. Puzzle Laci Pendingin Jenazah (Morgue Freezer) & Pusaka Pertama

Di ujung Kamar Mayat terdapat **12 Laci Pendingin Jenazah Besi** (bernomor `A1` hingga `C4`). Salah satu laci terkunci gembok rantai gaib dan berisi benda pusaka yang dibutuhkan untuk membuka Bangsal Bersalin Lantai 2.

* Di meja otopsi tengah, pemain menemukan **Laporan Otopsi Forensik (Item Dokumen #2)** beserta **Kunci Teralis Lantai 2**:

> **TEKS DOKUMEN (LAPORAN OTOPSI RAHASIA - DR. HENDRA):**
> "Jasad Lastri disimpan di laci B2, namun janin dan **Gunting Tali Pusar Pusaka** milik Mbah Suro saya sembunyikan di laci lain yang suhunya paling dingin di ruangan ini. Jangan pernah membuka laci yang **lebih hangat dari −20°C**, karena laci-laci lainnya sudah dipasangi jebakan gas formalin."

> **[REVISI EDITORIAL — E3]** Teks asli: "...yang suhunya di atas -20°C". Frasa "di atas" dapat dibaca sebagai nilai lebih besar (−10 lebih besar dari −20) atau lebih tinggi magnitudonya. Versi revisi tegas: hanya laci **lebih dingin** daripada −20°C yang aman, dan hanya satu laci yang memenuhinya (C3, −24°C).

* **Cara Menyelesaikan Puzzle Laci Jenazah (Kombinasi `ThermoKlenik` + `Selfie Mode`):**
1. Pemain harus memeriksa deretan laci menggunakan sensor **`ThermoKlenik` (`[T]`)** untuk mencari satu-satunya laci yang bersuhu **`-24°C`** (yaitu **Laci C3**).

2. Sambil memindai laci satu per satu, setiap 20 detik terdengar suara rintihan dari belakang punggung Raka—memaksa pemain segera menekan **`[X]` (Kamera Selfie)** untuk memantau dan menahan pergerakan Kuntilanak Merah di belakangnya agar tidak menerkam!

3. Setelah menemukan **Laci C3 (`-24°C`)**, pemain menarik laci tersebut keluar. Di dalamnya terdapat **Toples Janin Berformalin** dan **Pusaka #1: Gunting Tali Pusar Berkarat**.

4. Pemain mengaktifkan **Lensa AR (`[Q]`)** dan memotret aura merah pada Toples Janin tersebut → **`[✓] BUKTI KLENIK 2/3: RESIDU AURA TUMBAL (LENSA AR)`** tercentang di Jurnal!

**[KLARIFIKASI — puzzle laci]**

* **Daftar suhu 12 laci** ada di §3.3 (hanya C3 = −24°C; semua lainnya −6°C sampai −18°C; B2 sengaja −18°C sebagai pengecoh yang disebut dokumen). Sensor membaca satu laci per bidikan stabil; urutan pemindaian bebas.
* **Jebakan formalin** pada laci salah **tidak fatal** (§4.1): desis telegraph 1,5 detik; menutup laci kembali dengan `E` dalam jendela itu membatalkan jebakan; bila tidak, kabut formalin membuat layar buram dan batuk 6 detik serta **memanggil Lastri menempel lebih cepat**.
* Rintihan **setiap 20 detik** adalah telegraph 3 detik sebelum Lastri menempel lagi **bila ia sedang lepas**; selama ia sudah menempel atau selama pemain berada dalam loop lepas, interval tidak menumpuk. Selama memindai laci, pemain dapat berhenti dan mengangkat Selfie; **overlay Thermo tetap dapat tampil pada Selfie**, tetapi hanya membaca target di arah feed yang sedang aktif: laci di depan memerlukan kamera belakang; Selfie membaca Lastri/lingkungan di belakang. Tidak ada pembacaan tembus badan/dinding atau laci depan dari kamera belakang yang tidak aktif (§15.1).
* Pengambilan Gunting dan pemotretan aura adalah **dua flag berbeda** (`P2_TAKE_GUNTING` dan `P2_EVIDENCE_AURA`); memotret aura tanpa mengambil gunting tidak mengurangi apa pun (QA-05).
* Foto aura dibuat valid hanya bila toples berada di frame AR dengan pemindaian stabil **≥1,0 detik**; tidak menghitung ulang untuk foto kedua (QA-03). Biaya foto 1% mengikuti global.
* **Kunci Teralis Lantai 2** diambil bersama laporan otopsi (`P2_AUTOPSY_REPORT`, Dokumen #2); pengambilannya tidak bergantung pada Laci C3 sehingga pemain yang melakukan puzzle laci lebih dulu atau belakangan tidak terkunci.

---

#### Jalur opsional Arsip Saksi

Sebelum melanjutkan, pemain dapat memeriksa **Label Pasien Tanpa Nama**. Laci arsip D1 di gudang administrasi basement, terpisah dari Laci C3 yang wajib untuk cerita. Pindai laci arsip dengan ThermoKlenik. D1 menunjukkan 17°C dan angka 17 memantul di AR; cocokkan dengan halaman daftar korban yang kehilangan satu baris nama. Buka dengan kunci arsip yang tergantung di ruangan itu. Pengambilan tidak dihitung sebagai bukti atau pusaka boss. Sebelum meninggalkan babak, status item ini ditampilkan 0/1 atau 1/1 pada Jurnal. Rincian petunjuk, langkah, dan persistensi ada pada bagian "Item rahasia Babak 2" di §5.1A dan §3.3.

### ADEGAN 4: Bangsal Bersalin Lantai 2 — Bukti Terakhir & Deduksi Jurnal (Menit 45:00 – 65:00)

**Kondisi Awal:** Pemain kembali ke Lantai 1, membuka teralis tangga dengan kunci dari ruang otopsi, dan naik ke **Bangsal Bersalin Lantai 2**. Lorong lantai 2 dipenuhi inkubator bayi kosong yang kacanya pecah dan tirai rumah sakit yang bergoyang sendiri.

#### 1. Menembus Tembok Gaib Ruang Inkubator (Mekanik AR Overlay)

Pintu menuju **Ruang Operasi Bersalin Utama** di ujung lorong disegel oleh urat-urat daging merah gaib (*Kabut Sengkala*). Untuk memulai eksorsisme, pemain harus melengkapi **Bukti Klenik ke-3** dan mengambil **2 Benda Pusaka Penyegel** tersisa di lantai ini.

* Pintu **Ruang Inkubator Bayi** telah ditembok bata secara permanen oleh pihak rumah sakit.
* Pemain mengaktifkan **Lensa AR (`MataBatin.apk` — `[Q]`)**: tembok bata tersebut tampak transparan dan memperlihatkan celah pintu lama di dimensi masa lalu tahun 2001.

* Pemain berjalan menembus tembok sambil menatap layar AR dan masuk ke Ruang Inkubator.

**[KLARIFIKASI — patroli L2 dan tembok AR]**

* Di L2 Lastri berpatroli **lebih agresif**: ia memadamkan lampu lorong segmen demi segmen (flicker 2 detik sebagai telegraph) dan menempel lebih sering (§4.3). Perilaku mengikuti aturan yang sudah diajarkan di kamar mayat; tidak ada aturan baru yang fatal.
* Tembok bata Ruang Inkubator memakai pola **K102 Phase 1**: setelah `P2_CROSS_INKUBATOR_WALL`, bukaan fisik dan collider tetap terbuka **dua arah** tanpa AR dan tanpa baterai (QA-47, QA-71). Pemain yang keluar pada baterai 0% tidak terjebak.
* Pintu Ruang Operasi tetap berkabut sampai **ketiga pusaka dibawa** (§2.4); setelah itu kabut lenyap dan pintu dapat dibuka. Tiga bukti dan konfirmasi Jurnal tetap menjadi syarat meja operasi, bukan pintu.

#### 2. Merekam Bukti Ketiga (`EVP Recorder`) & Kaset Rekaman Damar

Di dalam Ruang Inkubator terdapat 6 kotak inkubator kaca. Dari salah satu inkubator terdengar suara tangisan bayi yang sumbang dan terputus-putus.

* Pemain menyalakan aplikasi **`EVP Recorder` (Perekam Suara Gaib)** di Smartphone dan mengarahkannya ke **Inkubator Nomor 04**:

* Terdengar suara perempuan menangis darah di dalam frekuensi radio: *"Kembalikan... anakku... di meja operasi..."*
* **`[✓] BUKTI KLENIK 3/3: REKAMAN EVP TANGISAN BERSALIN`** tercentang di Jurnal!

* Di dalam Inkubator Nomor 04, pemain mengambil **Pusaka #2: Gelang Identitas Bayi Lastri** serta menemukan **Perekam Suara Kecil milik Damar (Item Dokumen #3)**:
* **Suara Damar (Rekaman Audio):** *"Ka, kalau lo nemu rekaman ini di lantai dua RS Sido Waras... berarti gue udah diseret lebih jauh lagi. Orang berjubah yang nyulik gue lewat cermin itu nyebut dirinya abdi Penonton Terakhir. Dia bawa gue lewat terowongan bawah tanah menuju bekas Pabrik Gula Sindang Laut. Tolong bertahan, Ka... jangan biarin penonton di live chat lo mati sia-sia."*

**[KLARIFIKASI — EVP dan petunjuk]** Enam inkubator memiliki tangisan samar yang sama di rentang radio statis; **hanya Inkubator 04** mengeluarkan suara perempuan yang sebenarnya, dan hanya saat EVP Recorder diarahkan dengan jarak ≤2,0 m dan sesi rekam selesai (§4.3A). Rekaman Damar menjadi **Dokumen #3** (audio) dan tersedia ulang dari Jurnal; memutarnya ulang tidak menggandakan reward. Mengambil Gelang dan merekam EVP adalah dua flag berbeda (`P2_TAKE_GELANG` dan `P2_EVIDENCE_EVP`).

#### 3. Mengambil Pusaka Ketiga & Menyelesaikan Jurnal Deduksi (`Phasmophobia` Mechanics)

* Pemain keluar menuju **Ruang Jaga Suster** di seberang lorong (sambil terus menggunakan **Kamera Selfie `[X]`** untuk menghindari sergapan Kuntilanak Merah yang kini berpatroli lebih agresif dan mematikan lampu lorong).

* Di dalam brankas obat yang terbuka lewat mode AR, pemain mengambil **Pusaka #3: Botol Darah Segel Mbah Suro**.

**[REVISI EDITORIAL — E17: mekanisme brankas obat]** Naskah menyebut brankas "terbuka lewat mode AR" tanpa langkah yang dapat dikode. Baseline: brankas obat memakai **keypad 4 digit**. Dengan Lensa AR aktif di depan brankas, tampil **gema masa lalu**: siluet suster berseragam 2001 menekan empat tombol; setiap tombol menyala sekilas di brankas nyata dan Jurnal mencatat **`2 — 6 — 0 — 4`**. Jalur deduktif cadangan tanpa AR: **Catatan Suster Jaga (Dokumen #4)** di meja stasiun menyebut kode brankas "usia pasien terakhir + nomor inkubator bayinya", dan artikel forum menyebut Lastri berusia 26, sedangkan **papan penempatan bayi** di Ruang Jaga Suster mencatat inkubator 04 berinisial **L.**; `26` + `04` dapat disimpulkan **tanpa** mengunjungi Ruang Inkubator lebih dulu (R9). Kode `2604` tidak berubah oleh urutan; salah kode tidak menghukum.

> **TEKS DOKUMEN #4 (CATATAN SUSTER JAGA — LANTAI 2) [USUL BARU]:**
> "Obat-obatan dan botol dari Mbah Suro saya simpan di brankas, kodenya gampang dihafal: **usia pasien terakhir yang melahirkan di sini**, lalu **nomor inkubator bayinya**. Pasien itu Lastri, 26 tahun. Jangan sampai dokter kepala tahu botol itu masih ada."

* **Interaksi Jurnal Deduksi di HP:**
Pemain membuka tab **Jurnal Klenik** dan mencocokkan ketiga bukti yang telah dikumpulkan:

1. `Bukti 1: Suhu Beku Ekstrem (-16°C)`

2. `Bukti 2: Residu Aura Tumbal (Lensa AR)`

3. `Bukti 3: Rekaman EVP Tangisan Bersalin`

> **[REVISI EDITORIAL — E14]** Naskah asli menulis Bukti 1 sebagai "−16°C s/d −24°C". Jurnal menampilkan **−16°C** (bukti terdaftar); −24°C pada Laci C3 adalah petunjuk puzzle, bukan bukti ke-4, dan ditulis di catatan Jurnal tab Petunjuk saja.

* Pemain menekan tombol **`[KONFIRMASI IDENTITAS & BUKA RITUAL EKSORSISME]`**:

> **`[ENTITAS TERIDENTIFIKASI: KUNTILANAK MERAH RUANG BERSALIN (LASTRI)]`**
> **`[METODE EKSORSISME: Letakkan 3 Pusaka di Meja Operasi Utama. Pantau serangannya dari belakang lewat KAMERA SELFIE, hancurkan Perisai Kabut Sengkala-nya dengan LAMPU OPERASI / FLARE, lalu segel dengan BLITZ KAMERA!]`**

**[KLARIFIKASI]** Tombol konfirmasi aktif hanya bila **tiga ID bukti tepat** terdaftar (QA-06); ia mencatat `P2_JOURNAL_CONFIRMED` sekali dan tidak otomatis dipicu oleh scan ketiga. Konfirmasi boleh dilakukan **di mana saja** setelah bukti lengkap (tidak wajib di Ruang Jaga Suster), tetapi pesan metode eksorsisme tampil sebagai kartu sistem sekali saja dan tersimpan di tab Petunjuk Objektif.

---

### ADEGAN 5: Malam Puncak — Eksorsisme Kuntilanak Merah di Ruang Operasi (Menit 65:00 – 78:00)

Menggunakan mekanik Fase Gameplay C (Klimaks Eksorsisme Boss — Fase Alan Wake × DreadOut × Kamera Selfie).

#### 1. Memulai Ritual di Meja Operasi Bersalin

Pemain masuk ke **Ruang Operasi Bersalin Utama** yang luas. Di tengah ruangan terdapat meja operasi bersalin berkarat di bawah **Lampu Operasi Gantung (Surgical Light)** besar, dikelilingi oleh 2 tiang generator listrik cadangan di sudut ruangan dan cermin medis retak.

* Pemain meletakkan **Gunting Tali Pusar**, **Gelang Bayi**, dan **Botol Darah Segel** di atas meja operasi.

* **Pemicu Boss Fight:** Seluruh pintu ruangan terkunci rapat! Lampu ruangan padam menyisakan cahaya merah berkedip. Kuntilanak Merah muncul melayang di langit-langit dengan tubuh dibungkus **Kabut Sengkala Merah Pekat** (perisai kegelapan yang membuatnya kebal dari Blitz biasa) lalu menghilang ke dalam bayangan!

**[KLARIFIKASI — gerbang meja operasi dan CP5]**

* Pemain boleh masuk, memeriksa meja, lalu keluar lagi. Interaksi meja **hanya** menjalankan ritual bila: **tiga ID bukti unik + `P2_JOURNAL_CONFIRMED` + tiga pusaka dalam inventori** dan pemain berada dalam radius `E` dari meja. Bila kurang, meja menampilkan kekurangan di Jurnal (tanpa spoiler solusi) dan **tidak** mengunci pintu (QA-07, QA-11).
* **PHASE2_CP5 memakai anchor meja aman dengan yaw terdefinisi pada `E` pertama yang valid**, **sebelum** pusaka dipindahkan, pintu dikunci, atau lampu dipadamkan. Bila commit CP5 gagal, ritual tidak dimulai (§6.3).
* Setelah `P2_TABLE_INTERACT` dimulai, seluruh pintu terkunci **hanya selama attempt**; pemain tidak dapat melewati arena tanpa menang atau mati-lalu-retry. Jalur pulang tetap terbuka setelah `P2_BOSS_WIN`.

#### 2. Mekanik Boss Fight: "Pantau lewat Selfie, Bakar dengan Cahaya, Segel dengan Blitz!"

Pertarungan berlangsung dalam **3 Siklus Penyegelan** yang menguji koordinasi **Kamera Depan (`Selfie Mode`)**, **Manipulasi Cahaya (`Alan Wake`)**, dan **Eksekusi Kamera (`DreadOut`)**:

1. **Fase Lacak Posisi (Kamera Selfie `[X]`):**
* Di dalam ruang operasi yang gelap, Kuntilanak Merah **tidak terlihat jika pemain memandang ke depan**.

* Pemain wajib mengangkat HP dalam **Mode Kamera Selfie (`[X]`)** sambil bergerak memutari meja operasi. Di layar Selfie, terlihat sosok Kuntilanak Merah yang diselimuti Kabut Sengkala sedang terbang mengejar tepat dari belakang punggung Raka!

2. **Fase Hancurkan Kabut Sengkala (Manipulasi Cahaya — *Alan Wake* Mechanics):**
* Saat Kuntilanak Merah sudah berada dekat di belakang Raka (terlihat jelas di layar Selfie), pemain punya **2 cara** untuk menghancurkan perisai Kabut Sengkala-nya:

* **Cara A (Lingkungan):** Memancingnya tepat ke bawah **Lampu Operasi Gantung** atau **Lampu UV Sterilisasi**, lalu menekan sakelar dinding untuk menyalakan sorotan cahaya intensitas tinggi!
* **Cara B (Item Persediaan):** Menekan **`[G]`** untuk menyalakan dan menjatuhkan **Flare Merah Darurat** yang dibeli di Kamar Kos tadi tepat di langkah kaki Raka saat ia mengejar dari belakang!

* Begitu terkena cahaya Lampu Operasi atau Flare, **Kabut Sengkala di tubuh Kuntilanak Merah terbakar habis** dan ia menjerit tertahan sambil memegangi wajahnya di belakang Raka!

3. **Fase Tembakan Segel (Blitz Kamera — *DreadOut* Mechanics):**
* Hanya saat perisai kabutnya pecah dan ia terkena *stun* cahaya itulah wujud aslinya memadat di dunia nyata!
* Layar HP memunculkan indikator bidikan merah: **`[PERISAI HANCUR — BALIK BADAN ATAU TEMBAK BLITZ DARI KAMERA SELFIE SEKARANG!]`**

* Pemain menekan **`[Klik Kiri Mouse]` (Jepretan Blitz)**. Kilatan cahaya putih menghantam kepala Kuntilanak Merah dan menghancurkan 1 lapisan segelnya!

* **Eskalasi Siklus 2 & 3:**
* Setelah tembakan pertama dan kedua, Kuntilanak Merah akan memecahkan kaca inkubator di sekeliling ruangan dan menciptakan **2 Bayangan Palsu (Ilusi)** di layar Selfie.
* Pemain harus melirik indikator **`ThermoKlenik`** di pojok layar HP: hanya saat sosok yang mendekat di kamera Selfie menurunkan suhu ruangan ke **`-20°C`** itulah sosok yang asli!

* Pada **Jepretan Blitz Ketiga**, tubuh Kuntilanak Merah tercabik menjadi kabut abu putih yang tersedot masuk ke dalam lensa `MataBatin.apk`, meninggalkan selembar kain gendongan bayi yang terbakar habis di atas meja operasi.

> **[KLARIFIKASI — aturan khusus Babak 2, KANON]** **Larangan menoleh hanya berlaku saat Lastri berstatus menempel dan belum terkena cahaya ritual.** Setelah reticle **"PERISAI HANCUR"** muncul, menoleh aman selama jendela 5 detik. Selfie memiliki **pemancar Blitz ritual tersendiri** pada perangkat fiksi Raka; **cahaya flash belakang tidak dianggap otomatis mengenai target di belakang** (kamera belakang yang menghadap ke depan tidak menyegel apa pun di punggung pemain). Saat perisai pecah, Lastri memadat dan boleh dipotret dari belakang (lewat Selfie) atau dari depan (setelah membalik badan dan membidik dengan kamera belakang). Aturan lengkap hit, sudut, dan jarak ada di §4.4; siklus, jeda pemulihan, ilusi, dan sumber cahaya di §4.4 dan §4.5.

---

### ADEGAN 6: Ending Cutscene Babak 2 & Transisi ke Pabrik Gula Sindang Laut (Menit 78:00 – 82:00)

#### 1. Epilog di Ruang Operasi

* Suasana Ruang Operasi kembali hening. Efek kedap suara menghilang, digantikan suara hujan deras dari luar jendela rumah sakit.

* Raka jatuh berlutut di samping meja operasi sambil mengatur napasnya yang gemetar. Ia mengangkat Smartphone miliknya:
* **[LIVE | 👁️ 310.500 Penonton]**
* **Rentetan Live Chat #3 (Viral Nasional):**
* `@tretan_kw`: *"DEMI ALLAH GUE NONTON SAMBIL TUTUP MATA PAS DIA MUNCUL DI KAMERA SELFIE!"*

* `@jurnal_risa_fan`: *"Itu barusan segel lokasi kedua masuk ke HP?! Bang Raka udah dapet ratusan juta tapi nyawanya beneran di ujung tanduk!"*

* **Total Saweran Kumulatif:** **Rp164.800.000**. *Angka overlay naskah, bukan saldo yang dapat dibelanjakan; ledger belum lengkap (§5.3).*

#### 2. Video Tungku Damar dan Pesan `Penonton_Terakhir` — epilog, bukan transisi paksa

* Tiba-tiba layar HP berkedip merah tua dan mengunci seluruh kolom komentar.

* **[NOTIFIKASI DONASI MASUK: Rp6.666.666 dari `Penonton_Terakhir`]**

* Muncul rekaman video langsung berdurasi 8 detik di layar HP: memperlihatkan **Damar yang kini diikat di dekat tungku pembakaran batu bara raksasa yang mati**, sementara di kejauhan terdengar suara dengusan napas monster bertubuh besar dan bunyi rantai besi diseret di lantai pabrik.

> **Penonton_Terakhir:** *"Dua segel Panca-Sengkala telah kau buka, Raka. Lihat penontonmu—tiga ratus ribu pasang mata kini menatap layarmu tanpa berkedip. Semakin banyak yang menonton, semakin tipis batas antara duniamu dan Pasar Setan. Besok siang gunakan uangmu untuk membeli Lensa UV Rajah dan Flare sebanyak mungkin... karena besok malam pukul 23:59, tungku **Pabrik Gula Belanda 'Sindang Laut'** akan menyala untuk kalian berdua."*

* **[KLARIFIKASI — urutan epilog]** Video dan pesan hanya menuntaskan `P2_EPILOGUE`; **belum** `CKREK!`, **belum** fade, **belum** `P2_COMPLETE`. Kontrol dikembalikan ke Raka dengan objektif baru: **`Tinggalkan RSUD. Opsional: kembali ke arsip basement.`** Donasi Rp6.666.666 adalah **dana terikat**, bukan saldo (§8.1A).

#### 3. Transisi sebenarnya: kembali ke motor

Di **motor Raka di pelataran IGD** terdapat interaksi **`[E] Tinggalkan RSUD`**. Interaksi motor **hanya membuka panel UI sementara** (bukan event persisten `P2_LEAVE_RSUD`); tampilkan **`Arsip Babak 2: 0/1 atau 1/1`** dan status Arsip Saksi global `N/5`, dengan pilihan **`Kembali Menjelajah`** atau **`Lanjutkan`**. Membatalkan panel tidak menulis state. Hanya pilihan kedua memicu `P2_COMPLETE` (mempertahankan segel kedua yang sudah dikomit pada CP6, menyimpan sasaran Pabrik Gula atomik), lalu:

* Suara *shutter* kamera berbunyi keras (**`"CKREK!"`**) → Layar langsung hitam pekat (*Cut to Black*).

**[REVISI EDITORIAL — E10]** Dalam naskah asli `CKREK!` dan *Cut to Black* langsung mengikuti pesan `Penonton_Terakhir` di Ruang Operasi. Dengan urutan itu pemain yang belum mengambil Label Pasien (basement) kehilangan rahasia tanpa peringatan. Pola P1 §8.4 dipakai: epilog tanpa fade → kontrol kembali → keluar lewat motor dengan panel sadar → `CKREK!` + hitam. Audio dan visual `CKREK!` tidak diubah; hanya titik pemicunya dipindah (P2-D06). Interaksi keluar tidak tersedia sebelum epilog selesai dan **tidak boleh otomatis terjadi**.

---

## 2. Kontrak implementasi dan alur trigger

### 2.1 Kontrol dasar (KANON + KLARIFIKASI)

| Input | Konteks | Aksi | Catatan implementasi |
|---|---|---|---|
| `TAB` | Eksplorasi | Angkat/turunkan smartphone; akses tombol **MULAI LIVE** (naskah: `GO LIVE`, P2-D14) | [KLARIFIKASI] Jangan menonaktifkan HP dalam event paksa tanpa mengembalikan kontrol setelah event |
| `Q` | HP aktif | Toggle lensa AR | Tembok bata Ruang Inkubator, rantai gaib C3, aura toples, gema suster, `17` pada D1 |
| `T` **[GLOBAL/EXISTING pada `GameInputConfig`]** | HP aktif | Toggle **overlay ThermoKlenik** | Overlay sensor, bukan mode kamera: aktif di kamera belakang maupun Selfie (§4.2, §8.0) |
| **`X` (aktif; `V` hanya naskah lama)** | HP aktif | Toggle **Selfie Mode** | Ikuti `GameInputConfig`; tidak mengubah `V` (perspektif), jangan hardcode prompt |
| `C` **[GLOBAL/EXISTING]** | HP aktif | EVP Recorder | Toggle sesi rekam; tetap membidik hingga selesai (§4.3A), sesuai kontrol recorder P1; tidak menambah drain tersembunyi |
| `Klik kiri` | HP/kamera aktif | Foto bukti (1%) atau **Blitz** (15%/6 detik) menurut konteks | Foto aura hanya saat AR aktif dan target valid; Blitz segel hanya valid pada window boss (§4.4) |
| `Klik kanan` ditahan **[DIWARISI P1 §0.7]** | Eksplorasi | Angkat kamera fokus; lepas kembali ke keadaan sebelumnya | App menu HP dialihkan ke Kamera selama tombol ditahan; Selfie/Thermo tetap dapat di-toggle |
| `F` | HP aktif atau bersembunyi | Matikan/nyalakan layar HP | Wajib untuk bersembunyi dari Lastri yang menempel (§4.2); input prioritas terhadap kamera harus jelas |
| `G` **[GLOBAL/EXISTING pada `GameInputConfig`]** | Boss aktif | Jatuhkan **Flare Merah Darurat** di kaki Raka | Di luar boss, `G` tidak berfungsi dan flare tidak terkonsumsi (§8.1) |
| `E` **[GLOBAL/EXISTING]** | Dekat objek | Interaksi kontekstual (keypad, laci, kunci, tembok, brankas, meja operasi, sakelar lampu, generator, motor) | `IInteractable` + `InteractionInteractor`; sakelar/generator memakai resolver radius terarah, bukan controller kedua |
| `J` **[GLOBAL/EXISTING]** | Eksplorasi | Buka Jurnal Klenik | `GameAction.OpenJournal`; isi deduksi P2 baru (3 bukti, konfirmasi, Arsip Saksi, Petunjuk) |
| `W/A/S/D` | Kontrol karakter | Gerak; **berjalan maju** adalah syarat melepas Lastri (§4.2) | Kecepatan dan animasi global; tuning lokal tidak mengubah feel P0/P1 |

**Aturan konteks [KLARIFIKASI]:** Satu klik tidak boleh sekaligus mengambil item dan menembak Blitz. Saat menu keypad/jurnal/live dibuka, prioritas input milik UI; saat animasi wajib/pause, combat diproses menurut status adegan. AR adalah filter kamera HP, bukan portal teleport yang otomatis terbuka. **`E` tetap dapat dipakai dengan layar Selfie terangkat** (tidak boleh memaksa menurunkan HP saat mengambil kunci, menarik laci, atau menekan sakelar) agar aturan "pantau" tidak berbenturan dengan pickup (QA-23).

**Tabel prioritas input [KLARIFIKASI]:** Menu/animasi wajib > interaksi encounter (`E` pada sakelar/generator/laci) > UI HP (Jurnal/live) > foto/Blitz > interaksi normal. Mode browsing Jurnal tidak membekukan seluruh dunia; **saat Lastri menempel, membuka Jurnal tidak boleh menjadi kelemahan gratis**: meter sentuhan tetap berjalan dan peringatan HUD muncul (§4.2).

### 2.2 Stage machine global (KLARIFIKASI)

```text
NOT_STARTED
  -> SAFE_HUB [adegan 1: PC, forum, Pasar Gaib, upgrade, berangkat]
  -> ARRIVAL_IGD [adegan 2 mulai; live; Buku Jaga]
  -> POLI_INVESTIGATION [Poli 01-04; Bukti 1 -16 C]
  -> BASEMENT_OPEN [kode 1603 benar]
  -> MORGUE_SEARCH [kedap suara; Lastri menempel; laci A1-C4]
  -> MORGUE_CLEARED [Gunting + bukti aura; Laporan Otopsi + Kunci Teralis L2 dimiliki]
  -> L2_INVESTIGATION [teralis dibuka; tembok AR; Inkubator 04; Ruang Jaga Suster]
  -> RITUAL_READY [3 bukti + konfirmasi Jurnal + 3 pusaka; kabut pintu OR sudah lenyap sejak 3 pusaka dibawa]
  -> TABLE_READY [Raka dalam radius E meja; CP5 disimpan pada E valid]
  -> BOSS_ACTIVE [pusaka diletakkan; pintu terkunci; 3 siklus]
  -> BOSS_DEFEATED [3 poin segel]
  -> EPILOGUE [chat 310.500; Rp164,8 juta; video tungku; pesan; kontrol dikembalikan]
  -> RETURN_TO_EXIT [L2->L1->IGD->pelataran; boleh singgah basement untuk arsip]
  -> PHASE2_COMPLETE [hanya P2_COMPLETE setelah konfirmasi final di motor; save + CKREK + hitam]
```

Stage adalah **turunan runtime** dari event, bukti, dan inventori yang persisten; bukan flag save terpisah yang dapat desinkron. Objective/checkpoint membaca stage turunan itu. Pemain boleh kembali ke basement sesudah epilog untuk mengambil rahasia, dan stage tidak boleh mundur gara-gara memasuki ruangan lama. Pemain juga **boleh naik ke L2 sebelum Laci C3 dibuka** (Kunci Teralis diambil bersama Laporan Otopsi, bukan dari laci; E-catatan di §2.4); stage `L2_INVESTIGATION` tidak mensyaratkan Gunting, dan `RITUAL_READY` hanya terpenuhi bila ketiganya lengkap. **`TABLE_READY` adalah kondisi turunan sementara di depan meja, bukan event auto-save saat pusaka ketiga diambil. Panel keluar motor adalah UI transient; hanya `P2_COMPLETE` merupakan peristiwa akhir persisten.** `P2_EPILOGUE` dan `P2_COMPLETE` adalah dua tahap berbeda. Setiap transisi wajib idempoten sehingga *OnTriggerEnter*, save/reload, atau interaksi dobel tidak mencetak ulang hadiah.

### 2.3 Registry event inti (KANON dan KLARIFIKASI)

| ID event yang disarankan | Trigger | Prasyarat | Efek satu kali / status |
|---|---|---|---|
| `P2_ARRIVE_KOS` | Konfirmasi malam berikutnya dari akhir P1 | `phase1.Completed("P1_COMPLETE")` + segel Rusunawa | Transaksi `PHASE2_HUB`, chapter 2, anchor HUB; jam cerita 14:00; tanpa grant baterai/uang/item |
| `P2_FORUM_READ` | Buka artikel forum di PC | Dalam jangkauan PC | Dokumen bonus ke Jurnal; monolog Raka; bukan syarat gerbang |
| `P2_UPGRADE_UNLOCK` | Pertama membuka menu Modul HP **atau** `P2_FORUM_READ` | Belum ada receipt | Buka **Selfie Mode** dan **ThermoKlenik.apk**; tidak berbayar; receipt satu kali |
| `P2_SHOP_PACK_POWERBANK` / `_KEMENYAN` / `_FLARE` | Beli paket Pasar Gaib | `wallet_legitimate` cukup; paket belum dibeli | Kurangi saldo, tambah instance inventori; **satu transaksi atomik, ID transaksi unik** |
| `P2_KOS_CHARGED` | `E` pada colokan PC | Belum charge; di HUB | Isi penuh dan receipt atomik sekali; boleh dilewati |
| `P2_DEPART_KOS` | Pilih `[BERANGKAT KE RSUD SIDO WARAS - PUKUL 23:45 WIB]` | Upgrade tersedia (fallback otomatis bila PC/Modul dilewati) | Commit keberangkatan/anchor IGD dengan baterai aktual; jam 23:45→23:50; fade/load hanya sesudah write sukses |
| `P2_ARRIVE_IGD` | Spawn di pintu IGD | Berangkat | Monolog, objektif live; **PHASE2_CP1** setelah kontrol dikembalikan |
| `P2_START_LIVE` | Tombol MULAI LIVE | HP dapat dipakai | Live ON, **42.300** penonton, Chat #1, donasi `Penonton_Terakhir` Rp666.000 (peristiwa cerita, **dana terikat**) |
| `P2_NURSE_LOG` | Baca Buku Jaga Perawat | Dalam jangkauan meja IGD | **Dokumen #1**; petunjuk formula kode |
| `P2_THERMO_TUTORIAL` | Pertama menyalakan ThermoKlenik | HP aktif | Overlay on; tutorial `24°C`; objektif Poli |
| `P2_EVIDENCE_FREEZE` | Pembacaan Poli 03 stabil ≥1,5 s | Overlay Thermo aktif; jarak ≤3 m | `P2_FREEZE_POLI03` → `evidence_ids`; centang Jurnal; **PHASE2_CP4** aman |
| `P2_KEYPAD_1603` | Kode `1603` benar | Pintu belum terbuka | Pintu basement terbuka **permanen**; **PHASE2_CP2** |
| `P2_MORGUE_ENTER` | Pintu basement tertutup di belakang pemain | Berada di landing basement | Kedap suara aktif; **PHASE2_CP3** disimpan **sebelum** scare #1 |
| `P2_LASTRI_SCARE1` | Mencapai trigger lorong otopsi | Encounter belum diselesaikan | Peringatan HP; scare Selfie; Lastri **menempel** dalam jendela belajar (§4.2) |
| `P2_SELFIE_TUTORIAL` | Pertama mengangkat Selfie | Scare1 | Kedua fatal diblokir sampai lepas pertama atau 30 detik gameplay, mana yang lebih dulu; pengajaran tidak di-reset oleh buka UI |
| `P2_LASTRI_DETACH` | Hold 5 s in-frame terpenuhi **atau** sembunyi + `F` tuntas | Lastri menempel | Lastri memudar; interval re-attach berlaku (§4.2) |
| `P2_AUTOPSY_REPORT` | Ambil Laporan Otopsi + Kunci Teralis | Dalam jangkauan meja otopsi | **Dokumen #2**; `P2_KUNCI_TERALIS_L2` ke inventori |
| `P2_DRAWER_CHAIN_BREAK` | Pembacaan C3 −24°C stabil ≥1,0 s | Thermo aktif | Gembok rantai gaib C3 patah; laci dapat ditarik |
| `P2_TAKE_GUNTING` | Ambil Gunting Tali Pusar | C3 terbuka | **Pusaka #1**; **PHASE2_CP4** |
| `P2_EVIDENCE_AURA` | Foto aura toples | AR aktif; toples di frame ≥1,0 s | `P2_AURA_TUMBAL_C3` → `evidence_ids`; **PHASE2_CP4** |
| `P2_SECRET_CLUE` | Pindai D1 (Thermo 17°C) dan AR memantulkan `17` | Dalam gudang administrasi | Ungkap daftar korban yang kehilangan satu baris nama |
| `P2_SECRET_PICKUP` | Ambil Label Pasien Tanpa Nama | D1 dibuka dengan kunci arsip; ID belum tersimpan | Tambah `SECRET_P2_LABEL_PASIEN` ke `secret_items`; **autosave atomik segera** |
| `P2_STAIR_L2_UNLOCK` | `E` pada teralis dengan kunci | `P2_KUNCI_TERALIS_L2` dimiliki | Teralis terbuka permanen |
| `P2_L2_ARRIVE` | Memasuki lorong L2 | Teralis terbuka | Patroli agresif Lastri aktif (§4.3); objektif L2 |
| `P2_CROSS_INKUBATOR_WALL` | Melintasi tembok bata melalui AR | AR aktif; sudut/LOS valid | Bukaan fisik dua arah **permanen**; tidak menutup pemain di tengah dinding |
| `P2_EVP_DISCOVERED` | Mendekati Ruang Inkubator, statis HP terdengar | EVP belum ada | Petunjuk ke recorder tanpa bergantung chat |
| `P2_EVIDENCE_EVP` | Sesi rekam Inkubator 04 selesai | Jarak ≤2,0 m; recorder aktif | `P2_EVP_INKUBATOR04` → `evidence_ids`; **PHASE2_CP4** |
| `P2_TAKE_GELANG` | Ambil Gelang Identitas Bayi | Inkubator 04 dapat dijangkau | **Pusaka #2** |
| `P2_DAMAR_RECORDER` | Ambil/putar Perekam Suara Damar | Di Inkubator 04 | **Dokumen #3**; audio satu kali bisa diulang dari Jurnal |
| `P2_NURSE_NOTE` | Baca Catatan Suster Jaga | Dalam Ruang Jaga | **Dokumen #4** [USUL BARU] |
| `P2_NURSE_SAFE_OPEN` | Kode `2604` benar (gema AR atau deduksi) | Brankas belum terbuka | Brankas terbuka permanen |
| `P2_TAKE_BOTOL` | Ambil Botol Darah Segel Mbah Suro | Brankas terbuka | **Pusaka #3**; **PHASE2_CP4** |
| `P2_JOURNAL_CONFIRMED` | Konfirmasi deduksi di Jurnal | Tiga ID bukti tepat lengkap | Receipt satu kali; syarat ritual global, bukan scan otomatis |
| `P2_OR_DOOR_OPEN` | Membawa ketiga pusaka ke pintu Ruang Operasi | `Gunting` + `Gelang` + `Botol` dimiliki **atau** `P2_BOSS_WIN` | Kabut Sengkala pintu lenyap, collider terbuka; **turunan**, bukan flag baru |
| `P2_TABLE_INTERACT` | `E` valid di meja operasi | 3 bukti unik **AND** konfirmasi Jurnal **AND** 3 pusaka **AND** radius `E`; **CP5 sukses ditulis lebih dulu** | **Transaksi runtime nonpersisten**: pusaka diletakkan, pintu terkunci, lampu padam, boss dimulai; **jangan** menimpa CP5 selama fight |
| `P2_LIGHT_USE` | Menyalakan lampu atau menjatuhkan flare | Boss aktif; sumber tersedia | **Event transien percobaan boss**; bila Lastri asli berada dalam beam/radius → `P2_SHIELD_BREAK` |
| `P2_SHIELD_BREAK` | Cahaya mengenai sosok asli | Siklus berjalan | Perisai pecah; **jendela 5 detik**; ilusi lenyap; reticle `PERISAI HANCUR`; jangan disimpan ke `completed_event_ids` |
| `P2_BLITZ_SUCCESS` | Blitz valid mengenai kepala saat rentan | Window belum habis; poin siklus belum tercatat | 1 lapisan segel **hanya di RAM** percobaan ini; sumber cahaya pemicu (bila lampu) korslet; reset seluruh sumber bila retry CP5 |
| `P2_BOSS_WIN` | Total 3 lapisan segel unik | 3 keberhasilan pada 3 siklus | Commit **CP6 atomik**: segel kedua, pusaka pindah ke meja, boss nonaktif, pintu terbuka |
| `P2_EPILOGUE_CHAT` | Sequence setelah menang | `P2_BOSS_WIN` | Hujan, Chat #3, 310.500, Rp164,8 juta (overlay), satu kali |
| `P2_DAMAR_VIDEO` | Layar merah tua + donasi Rp6.666.666 | Chat selesai | Video 8 detik; **dana terikat**; satu kali |
| `P2_EPILOGUE` | Pesan `Penonton_Terakhir` selesai | Video dan chat | **Kontrol dikembalikan**, objektif keluar; tanpa fade |
| `P2_EXIT_PANEL_OPEN` **(runtime/UI saja)** | `E` pada motor di pelataran IGD | Epilog selesai | Panel Arsip 0/1 atau 1/1 dan `N/5`; `Kembali Menjelajah` menutup panel tanpa persist |
| `P2_COMPLETE` **(satu-satunya event akhir persisten)** | Pilih `Lanjutkan` pada panel keluar | Epilog lengkap; panel terbuka; belum complete | Save CP7 final atomik dan tujuan Pabrik Gula, mempertahankan segel CP6; **`CKREK!` + hitam hanya setelah commit berhasil**; idempoten |

**Catatan [KLARIFIKASI]:** ID di atas adalah **usulan identifier teknis**, bukan ID yang terbukti sudah tersedia dalam repositori. Tabel adalah **registry event** (termasuk event runtime), **bukan daftar field yang semuanya wajib dipersist**: `P2_EXIT_PANEL_OPEN`, `P2_LIGHT_USE`, `P2_SHIELD_BREAK`, `P2_BLITZ_SUCCESS`, dan `P2_TABLE_INTERACT` tidak disimpan ke `completed_event_ids` sebelum kemenangan; `P2_BOSS_WIN` commit atomik adalah status permanennya. Cocokkan dengan sistem event/save yang sudah ada sebelum menambahkan baru.

### 2.4 Prasyarat, kondisi gagal, dan pemulihan

| Fitur | Gate positif | Saat gagal/terputus | Perilaku yang harus dicegah |
|---|---|---|---|
| Keypad basement | Kode `1603` dari log + papan poli + Thermo | Salah kode: umpan balik non-hukuman; boleh coba ulang tanpa batas | Softlock karena chat petunjuk terlewat; penalti/alarm |
| Pintu basement dari dalam | Tuas keluar tanpa keypad | Pintu selalu dapat dibuka dari dalam | Pemain terkurung di kamar mayat setelah pintu menutup |
| Kamar mayat — Lastri menempel | Selfie aktif/tunda; berjalan maju 5 s in-frame atau sembunyi + `F` | Menoleh langsung (setelah jendela belajar) atau meter penuh + 2 s: Game Over → CP3 | Instakill di scare pertama; Game Over karena menurunkan HP tanpa petunjuk |
| Puzzle laci | `ThermoKlenik` membaca satu laci sampai **−24°C** (C3) | Laci lebih hangat dari −20°C memicu gas formalin (non-fatal, dapat dibatalkan) | Jebakan yang membunuh; C3 tidak dapat dicapai karena rantai tak terlihat |
| Bukti klenik | 3 ID unik: `P2_FREEZE_POLI03`, `P2_AURA_TUMBAL_C3`, `P2_EVP_INKUBATOR04` | Re-scan/duplikat tidak menambah progres | Hitung 3 foto objek yang sama sebagai 3 bukti |
| Kunci Teralis L2 | Diambil bersama Laporan Otopsi | Pemain boleh naik tanpa C3 dan kembali | Mengharuskan C3 untuk naik, atau kunci hilang saat load |
| Tembok Inkubator | AR membidik; bukaan semu terbuka | Tetap bisa mengulang bidikan | Terjebak saat AR/baterai mati di dalam geometri |
| Brankas obat | `2604` via gema AR **atau** deduksi catatan | Salah kode non-hukuman | Hanya terbuka bila AR aktif sehingga baterai 0% mengunci pemain |
| Pintu Ruang Operasi | 3 pusaka dibawa atau `P2_BOSS_WIN` | Pintu terbuka tidak mengunci ulang | Pintu menutup saat pusaka diletakkan di meja; masuk otomatis memicu boss |
| Meja operasi | 3 bukti + konfirmasi + 3 pusaka + `E` eksplisit | Kekurangan ditampilkan di Jurnal | Battle dipicu saat keluar dari inspeksi / tanpa bukti |
| Perisai Kabut Sengkala | Cahaya (lampu/flare) mengenai sosok asli | Cahaya mengenai ilusi/kosong hanya membuang ilusi atau cooldown; lampu tidak rusak | Lampu hancur saat gagal sehingga mustahil menang |
| Window 5 detik | Blitz valid (kepala, jarak, LOS) | Miss/timeout: perisai kembali, **lampu tetap utuh** | Poin ganda dari satu siklus; Blitz tembus perisai utuh |
| Kemenangan | 3 siklus berbeda berhasil | Mati → CP5 snapshot | Segel tersimpan tanpa pusaka-kondisi konsisten |
| Ending | Epilog selesai; pemain berinteraksi dengan motor | Boleh turun, mengambil arsip, membatalkan panel | Fade prematur saat video berakhir; keluar dari Ruang Operasi tanpa jalan pulang |

---

## 3. Perincian desain level dan navigasi

### 3.1 Denah logis (KANON + KLARIFIKASI)

```text
LUAR: Kamar Kos (Safe Hub: PC, Pasar Gaib, colokan, kunci motor)
   | berangkat 23:45
   v
PELATARAN IGD + motor Raka (titik exit akhir)
   |
   v
L1: IGD (Buku Jaga; dinamo IGD) --> Koridor Poliklinik Dalam
        [Poli 01 Umum | Poli 02 Anak | Poli 03 Kandungan (BEKU -16) | Poli 04 Gigi]
        |                                    |
        |                                    +-- pintu besi BASEMENT [keypad 1603]
        +-- tangga L2 [teralis; Kunci Teralis L2]
B1: landing (tuas keluar) --> Instalasi Kamar Mayat (KEDAP SUARA)
        +-- Pos Petugas (dinamo; lemari besi = hiding)
        +-- zona otopsi (2 meja tengah; Laporan Otopsi + Kunci Teralis; kolong = hiding)
        +-- zona laci A1-C4 (12 laci; C3 = -24 C)
        +-- gudang administrasi (daftar korban; papan kunci; laci arsip D1-D4)
        +-- pintu terowongan servis (dirantai; jejak Damar, non-gating)
L2: tangga --> lorong bangsal bersalin (3 segmen lampu S1-S3)
        +-- Ruang Inkubator (tembok bata + AR; 6 inkubator; Inkubator 04: EVP + Gelang + Perekam Damar)
        +-- Ruang Jaga Suster (brankas obat 2604; Catatan Suster; dinamo)
        +-- Ruang Operasi Bersalin Utama (pintu berkabut; meja; Lampu Operasi; UV-N; UV-S;
                                          2 generator; cermin medis retak; 4 inkubator rusak)
```

**Penting [KLARIFIKASI]:** Lima hal geometri wajib konsisten. (1) Pintu basement dan tuas keluar: pemain tidak dapat terkunci di kamar mayat. (2) Tembok bata Ruang Inkubator memakai pola K102 Phase 1: setelah dilintasi, bukaan terbuka dua arah permanen. (3) Kabut pintu Ruang Operasi dilepas oleh **tiga pusaka dibawa** (turunan), tidak oleh Jurnal; setelah `P2_BOSS_WIN` pintu tetap terbuka walau pusaka berada di meja. (4) Ruang Operasi terkunci **hanya selama attempt boss** (RAM) dan tidak membuat dead end bagi pemain maupun Lastri. (5) Area wajib dapat dicapai tanpa menembus arena sebelum waktunya; kamar mayat, gudang arsip, Pos Petugas, dan jalur ke L2 tidak boleh bergantung pada objek dinamis yang bisa hilang akibat load.

### 3.2 IGD, Koridor Poliklinik, dan puzzle keypad 1603

**Lobi IGD (KANON).** Pintu kaca geser pecah, brankar berkarat, bau antiseptik busuk. Meja resepsionis berisi **Buku Jaga Perawat Malam** (Dokumen #1). Pemain mengangkat HP dan menyalakan live di sini; **tidak ada gerbang** yang mewajibkan live sebelum Poliklinik (P2-D14 untuk label UI).

**Koridor Poliklinik Dalam [KLARIFIKASI + USUL BARU].** Empat ruang poliklinik berderet di satu sisi; sisi seberang berisi kursi tunggu, papan jadwal poli, dan pintu besi basement di ujung. Tangga L2 berada di sisi IGD dan terhalang teralis.

| Ruang | Nama pada papan | Suhu terbaca (°C) | Catatan |
|---|---|---:|---|
| IGD / lorong | — | 24 | Suhu normal; tutorial ThermoKlenik (`24°C`) |
| Poli 01 | Umum | 22 | Pintu dapat dilihat dari kaca; tidak beku |
| Poli 02 | Anak | 22 | Tidak beku |
| **Poli 03** | **Kandungan & Kebidanan** | **−16** | Kaca membeku, bunga es, bip cepat; **pintu membeku dan tidak dapat dimasuki** (bukan gate) |
| Poli 04 | Gigi | 21 | Tidak beku |

**Deduksi kode tanpa chat [KLARIFIKASI].** (1) Buku Jaga menyebut pasien terakhir seorang ibu hamil bernama Lastri yang diperiksa di **poli kandungan** dan hanya satu poli yang membeku; (2) papan jadwal menunjukkan **Poli 03 = Kandungan & Kebidanan**; (3) sensor membaca **−16°C** pada Poli 03 stabil; (4) tanpa tanda minus + nomor ruang = `16` + `03` = **1603**. Angka `1603` tidak boleh muncul sebagai teks lingkungan. Petunjuk tambahan (non-HUD, tidak spoiler): kartu rekam medis berinisial **L.** terbaca dari kaca Poli 03 saat zoom kamera.

**Keypad [KLARIFIKASI].** Panel 4 digit dengan padlock panjang dinamis yang sama dengan sistem Phase 1 (§14.1). Salah kode = bunyi *buzzer* + lampu merah singkat, tanpa hukuman, tanpa batas coba. Kode benar membuka pintu **permanen**; pintu dapat dibuka dari dalam tanpa keypad.

**Pintu Poli 03 dan interior [USUL BARU].** Pintu membeku tidak berfungsi sebagai ruang bermain (hemat aset dan memastikan Poli 03 tidak menjadi tempat sembunyi tak terduga). Pembacaan sensor valid dari koridor dalam jarak ≤3 m dari kaca selama ≥1,5 detik.

**Dinamo IGD [BASELINE UJI].** Satu dinamo manual di balik meja resepsionis (ruang genset mini) sesuai aturan pemulihan baterai §8.0, tersedia sebelum pemain memerlukan ThermoKlenik.

### 3.3 Kamar Mayat: struktur, 12 laci, gudang arsip, dan hiding

Kamar mayat terdiri dari zona yang dapat dibaca tanpa panah HUD: **landing basement** (luar kedap suara), **Pos Petugas** (ruangan kecil dengan pintu, dinamo, lemari besi), **zona otopsi** (dua meja stainless di tengah, lorong sempit antar meja, kolong meja), **zona laci** (dinding 12 laci), dan **gudang administrasi** (rak, papan kunci, lemari arsip). Basement ambient **12°C**; semua ruang di luar laci pendingin memiliki suhu ambient, kecuali hotspot D1 17°C dan anomali Lastri yang dibidik.

**Tabel suhu 12 laci [BASELINE UJI — angka tetap setelah blockout].** Hanya **C3** lebih dingin dari −20°C; **tidak ada laci bersuhu −20 s/d −23°C** agar ambang tidak ambigu. Sensor membaca bilangan bulat.

| Baris | 1 | 2 | 3 | 4 |
|---|---:|---:|---:|---:|
| **A** | A1 **−6** | A2 **−9** | A3 **−11** | A4 **−8** |
| **B** | B1 **−13** | B2 **−18** *(jasad Lastri disimpan di sini menurut laporan; pengecoh terdekat)* | B3 **−15** | B4 **−7** |
| **C** | C1 **−12** | C2 **−17** | **C3 −24** *(Toples Janin + Gunting Tali Pusar)* | C4 **−10** |

**Aturan laci [KLARIFIKASI].**

- Hanya **C3** aman. Ke-11 laci lain adalah **laci terjebak** (gas formalin); jebakan non-fatal dan dapat dibatalkan (§4.1).
- **Gembok rantai gaib** pada C3 terlihat **hanya lewat AR** (embun beku tebal membungkus rantai) dan patah ketika sensor membaca −24°C stabil ≥1,0 detik; prompt "Tarik laci" muncul setelah rantai patah. Ini menjaga puzzle sebagai **pemindaian suhu**, bukan tebak-tebakan, namun pemain yang menyalakan AR lebih dulu tidak diberi jalan pintas tanpa sensor.
- Satu bidikan sensor membaca satu laci pada kerucut ≤15° dan jarak ≤1,5 m; pembacaan memerlukan dwell 1,0 detik. Pemain yang memindai sambil berjalan di sepanjang dinding laci masih dihitung bila dwell terpenuhi. Pembacaan laci memerlukan **kamera belakang**; pembacaan pada Selfie menunjukkan area punggung (§4.2).
- **B2** tetap terjebak walau dokumen menyebut jasad Lastri di sana; isinya (setelah jebakan dibatalkan) hanyalah kantong jenazah kosong yang berembun (prop kecil non-gating).
- Pemain yang menarik laci salah tanpa memindai tidak ditolak; konsekuensinya (jebakan) adalah pengajaran, bukan hukuman fatal.

**Gudang administrasi dan jalur rahasia D1 [KLARIFIKASI].** Gudang berada di sisi utara zona otopsi (sejalan dengan koordinat §3.3A) dengan pintu yang selalu terbuka; dinding laci pendingin berada di sisi timur. Isi: papan kunci (kunci arsip tergantung **terlihat dari pintu**), rak kardus, **daftar korban** pada dinding (judul "Daftar Jenazah Bayi — Bangsal Bersalin 2001": **17 nomor baris, hanya 16 nama; baris ke-17 kosong**), dan lemari arsip dengan **D1–D4**. D2–D4 membaca ambient 12°C; **D1 membaca 17°C** dan AR memantulkan angka **17** di atasnya. Catatan perawat yang menjadi petunjuk berada di meja otopsi tengah di samping Laporan Otopsi (bukan di gudang), supaya pemain yang hanya mengikuti jalur utama dapat menemukan jejaknya tanpa wajib masuk gudang. Penerimaan petunjuk dan langkah ada di §5.1A.

**Jejak Damar di kamar mayat [REVISI EDITORIAL — E7, P2-D05].** Di ujung gudang terdapat **pintu terowongan servis** berantai (tidak dapat dibuka malam ini). Pada gagangnya tergantung **gantungan kunci kamera Damar**; goresan kapur putih berbentuk panah ke atas menunjuk ke arah tangga. Pemain yang memeriksa mendengar monolog Raka singkat. **Non-gating, tanpa bukti, tanpa reward, tidak disimpan sebagai collectible**; hanya memenuhi kalimat donasi `Penonton_Terakhir` ("Sahabatmu meninggalkan sesuatu di Kamar Mayat bawah tanah") dan menyiapkan rekaman Damar di L2.

**Pos Petugas [BASELINE UJI].** Ruangan 3×3 m dengan pintu yang dapat ditutup, **dinamo manual**, dan lemari besi (hiding). Digunakan untuk pemulihan baterai bila HP habis di tengah kamar mayat; progres crank dipause (bukan di-reset) ketika Lastri menempel.

**Hiding spot [KLARIFIKASI].** Dua tempat sembunyi: **kolong meja otopsi** (dua lokasi) dan **lemari besi** di Pos Petugas. Masuk dan keluar dengan `E`; layar dimatikan dengan `F`. Spot tidak boleh menjebak pemain (tanpa celah aksi): keluar selalu tersedia, dan Lastri memudar setelah `lastri.hideLoiterSec` (§8.0).

**Teks dokumen lingkungan:**

> **TEKS DOKUMEN (CATATAN PERAWAT — DI SAMPING LAPORAN OTOPSI):**
> "Yang dihapus dari daftar tidak membeku bersama jenazah."

### 3.3A Koreksi blocking Lastri di kamar mayat (BASELINE UJI)

Blockout memakai koordinat lokal meter. Landing basement `(0,0)`; pintu basement `x=2`; zona otopsi `x=6…18`; dua meja tengah `x=10…14`; **loop mengitari meja** ±16 m; zona laci `x=20…26` pada dinding timur; gudang administrasi `z=+8…+14` di sisi utara zona otopsi; Pos Petugas `x=3…6, z=−4…−1`; koordinat denah adalah XZ, Y hanya elevasi.

```text
DORMANT -> TELEGRAPH (hembusan dingin + napas) -> SCARE1_ATTACH (scripted, nonfatal)
        -> ATTACHED_WATCHED (in-frame Selfie) <-> ATTACHED_UNWATCHED (out-of-frame)
        -> DETACH_FADE (2 s) -> DETACHED (interval) -> MOAN_TELEGRAPH (3 s) -> ATTACHED_*
ATTACHED_UNWATCHED:  touchMeter naik 10%/s; penuh -> HAIR_TOUCH (HUD+heartbeat) -> 2 s -> GRAB (Game Over)
ATTACHED + BODY_TURN_TO_LASTRI (>=0,25 s) di luar jendela belajar -> POUNCE (Game Over)
ATTACHED + HIDING_SCREEN_OFF -> LOITER 6 s -> DETACH_FADE
```

**Perhitungan baseline yang wajib diuji (bukan jaminan kelulusan):** controller existing berjalan efektif **1,7 m/s** dan tidak membedakan maju/mundur. Melepas Lastri dengan berjalan maju memerlukan 5,0 s in-frame pada ≥0,8 m/s, yaitu minimal **4,0 m** dan pada 1,7 m/s tepat **8,5 m**. Loop meja ±16 m memberi ruang cukup untuk satu lepas penuh tanpa berbalik; jalan buntu pendek dihindari. Meter sentuhan penuh dari kosong memerlukan 10 s; ia turun 25%/s saat Lastri in-frame sehingga **frame yang konsisten selama 4 s mengosongkan meter dari penuh**. Pemain yang menunda mengangkat Selfie setelah telegraph rintihan 3 s mendapat **12 s setelah attach, atau 15 s sejak rintihan mulai**, sebelum `GRAB` (3 s telegraph + 10 s meter + 2 s HAIR_TOUCH), jauh lebih lama daripada waktu angkat HP normal (<2 s). Tambah logging; bila tester gagal tanpa kesalahan nyata, **perlambat meter atau perpanjang telegraph, jangan menghapus fatal**.

**Estimasi pemindaian [BASELINE UJI]:** 12 laci × ±6 s = 72 s; rata-rata 3–4 siklus Lastri (≈15 s per siklus) menambah ±50 s; **puzzle laci diperkirakan 2,5–3,5 menit** bagi pemain yang mengikuti petunjuk suhu. Setelah `MORGUE_CLEARED` (gunting + aura + laporan), interval re-attach melonggar menjadi **45 s** agar pencarian rahasia D1 tidak menghukum berlebihan.

**Jendela belajar [KLARIFIKASI].** Pada attach pertama (`SCARE1_ATTACH`) sampai lepas pertama berhasil atau 30 s berlalu (mana yang lebih dulu), kedua fatal diblokir; pelanggaran menoleh langsung **tidak membunuh**: layar bergetar, Lastri menggeram, dan HUD menampilkan petunjuk berbahasa Indonesia. Setelah itu aturan penuh berlaku. Ini menjaga prinsip §8.2 (aturan fatal diajarkan sebelum dihukum).

**Toleransi arah [KLARIFIKASI].** "Menoleh langsung" dievaluasi dari **yaw kontrol pemain** (`player.transform.forward`, tanpa pitch, smoothing avatar, kamera OTS collision, atau rotasi kamera Selfie), bukan dari kamera Selfie: Raka dianggap menoleh bila vektor hadap berada dalam **50°** dari vektor ke Lastri selama **≥0,25 s** tanpa jendela belajar dan tanpa terkena cahaya ritual. Putaran cepat <0,25 s (refleks) tidak fatal tetapi menimbulkan *flinch* dan menaikkan meter 20% **sekali saat melintasi ambang**, bukan per frame.

**Selfie dan sebagian layar [KLARIFIKASI].** "In-frame" dinilai dari bounds Lastri yang terproyeksi ke frustum Selfie dengan toleransi ukuran (≥30% projected rect di layar serta ≥3 dari 9 anchor target lolos LOS; tidak terhalang geometri solid); sesaat terhalang objek yang harus dilalui tidak menjadi pelanggaran. Jangan memakai satu raycast titik; pakai bounds + LOS toleran seperti Phase 1 §4.2.

### 3.4 Lantai 2: urutan investigasi tidak boleh keras tanpa alasan

**[KANON]** Tiga hal harus terjadi di L2: Bukti 3 (EVP Inkubator 04), Pusaka #2 (Gelang di Inkubator 04), Pusaka #3 (Botol Darah di brankas obat). Pintu Ruang Operasi disegel kabut hingga tiga pusaka dibawa. Ruang Inkubator ditembok bata; Ruang Jaga Suster berada di seberang lorong.

**[KLARIFIKASI]** Ruang Inkubator dan Ruang Jaga Suster **independen**; urutan bebas. Tiga bukti memakai ID terpisah. Kunci Teralis diambil di basement; pemain yang naik ke L2 tanpa Gunting/aura kembali ke basement bila perlu; **stage tidak mundur**. Brankas obat terbuka dengan kode `2604` dari gema AR atau deduksi; **AR tidak diperlukan untuk membuka setelah kode diketahui** (baterai 0% tidak mengunci pemain).

**Lorong L2 [BASELINE UJI].** Lorong bangsal ±36 m dibagi tiga segmen lampu **S1 (tangga) – S2 (tengah, depan Ruang Jaga Suster) – S3 (ujung, pintu Ruang Operasi)**. Ruang Inkubator berada di sisi utara antara S1–S2 (tembok bata); Ruang Jaga Suster di sisi selatan S2. Lastri berpatroli dengan memadamkan segmen yang dilewati (flicker 2 s sebagai telegraph); segmen gelap mengalikan laju meter 1,5×. Cahaya bulan dari jendela tinggi menjaga navigasi tetap terbaca (§3.5).

**Ruang Inkubator [KLARIFIKASI].** Enam inkubator kaca 01–06; semua mengeluarkan tangisan bayi samar pada statis radio; **hanya Inkubator 04** memiliki suara perempuan saat sesi EVP selesai. Gelang Identitas Bayi Lastri dan Perekam Damar berada **di dalam/di dekat Inkubator 04** dan pengambilannya **tidak dikaitkan ke bukti EVP**; keduanya hanya perlu dijangkau (hindari urutan paksa).

**Ruang Jaga Suster [KLARIFIKASI].** Meja stasiun dengan **Catatan Suster Jaga** (Dokumen #4), papan jadwal piket, **papan penempatan bayi** (Inkubator 01–06 dengan inisial ibu; baris **04 berinisial L.**), brankas obat 4 digit, dinamo manual di sisi dalam. AR di depan brankas menampilkan gema suster 2001 menekan `2-6-0-4` (§1 Adegan 4). Pintu ruangan dapat ditutup untuk meredam ambience luar; telegraph fatal tetap terbaca dan tidak ikut diredam.

### 3.4A Isi konkret alokasi menit per adegan (BASELINE UJI)

Ini **budget waktu per beat berdasarkan jalur contoh**, bukan kunci progres linear. Pemain tetap bebas memilih urutan di dalam lantai yang sama. Pemain mahir yang selesai lebih cepat bukan bug.

| Menit global | Beat level yang nyata | Interaksi/tekanan yang terukur |
|---|---|---|
| 00–03 | Kos: bangun, PC, artikel forum | Membaca; tidak ada tekanan; riset memberi 3 fakta yang dipakai ulang |
| 03–08 | Pasar Gaib, upgrade otomatis, belanja opsional, colokan HP | Keputusan sumber daya; tidak ada gate berbayar |
| 08–10 | Kunci motor, fade ke IGD | Transisi; CP1 di IGD |
| 10–14 | IGD: live, chat #1, donasi, Buku Jaga | Tutorial `[TAB]`, `[T]`; petunjuk formula |
| 14–20 | Poli 01–04: pindai, Bukti 1 | ThermoKlenik, papan jadwal, kaca beku |
| 20–25 | Keypad 1603 → basement → landing | Verifikasi deduksi; CP2; CP3 sebelum scare |
| 25–28 | Kamar mayat: kedap suara, suasana, trigger scare | Atmosfer; tutorial audio terbatas |
| 28–32 | Scare #1: peringatan HP, Selfie, jendela belajar | Aturan menoleh diajarkan lalu dipraktikkan |
| 32–36 | Zona otopsi: Laporan, Kunci Teralis, catatan perawat | Dokumen #2; petunjuk suhu −20°C |
| 36–42 | Laci: pindai 12 laci; siklus Lastri; C3; Gunting; foto aura | Thermo + Selfie bergantian; formalin edukatif |
| 42–45 | Pulang ke landing / opsional gudang; teralis | MORGUE_CLEARED; interval re-attach melonggar |
| 45–50 | L1→L2, lorong, lampu padam, patroli agresif | Aturan lama dipakai ulang di ruang baru |
| 50–55 | Tembok bata AR → Inkubator 04: EVP + Gelang + Perekam Damar | Bukti 3; Pusaka #2; Dokumen #3 |
| 55–60 | Ruang Jaga Suster: catatan, gema AR, `2604`, Botol Darah | Pusaka #3; Dokumen #4 |
| 60–65 | Jurnal Deduksi, konfirmasi, pesan metode, pintu OR | `P2_JOURNAL_CONFIRMED`; kabut pintu lenyap |
| 65–67 | Ruang Operasi: masuk, periksa meja, letakkan pusaka, trigger | CP5; pintu terkunci |
| 67–76 | Tiga siklus penyegelan + jeda pemulihan | Selfie, cahaya, Blitz, ilusi |
| 76–78 | Kemenangan, kain gendongan terbakar, CP6 | Tidak ada autosave selama fight |
| 78–82 | Epilog: hujan, chat, Rp164,8 juta, video, pesan; keluar → motor → `CKREK!` | Kontrol dikembalikan; pulang ±60–120 s |

**Batas produksi:** Jangan menambah kunci/fetch quest baru untuk mengejar durasi. Rekam waktu tiap beat di PlayMode/build Windows; bila pemain mahir menyelesaikan dalam 60–70 menit, itu bukan bug. Sasaran 82 menit dapat bergeser berdasarkan atmosfer, Selfie, dan playtest; hindari memalsukan pacing dengan pintu terkunci otomatis.

### 3.5 Keselamatan navigasi dan pembacaan ruang (USUL BARU)

- Basement dan L2 memiliki landmark visual yang dapat dibedakan tanpa HUD: warna cahaya per segmen, nomor ruang besar yang aus, genangan formalin, garis ubin, pengaturan poster/tirai.
- Dua jalur menghindar Lastri di kamar mayat (loop meja + gudang administrasi) sehingga hiding tidak menjadi satu-satunya opsi.
- Tempat sembunyi punya batas masuk/interaksi eksplisit; kamera dan collision AI tidak menembus lemari/kolong tertutup secara visual.
- Segmen L2 yang gelap tetap menunjukkan tepi dinding, pintu, dan pegangan interaksi lewat cahaya merah redup, cahaya bulan, atau layar HP; **tidak ada area yang buta total** (§8.2).
- Item wajib (Gunting, Gelang, Botol, kunci) tidak disembunyikan di balik objek dinamis/physics yang dapat hilang saat load atau saat efek boss.
- **Kedap suara tidak boleh menyembunyikan telegraph fatal**: bunyi rintihan, napas Lastri, dan hembusan dingin berasal dari bus yang tidak diredam (§4.3A).

### 3.6 Ruang Operasi: denah dan titik interaksi arena (BASELINE UJI)

Denah konseptual, **belum denah final**. Ruang ±10×8 m dengan satu pintu di sisi barat.

```text
              UTARA (UV-N)
   +--------------------------------+
   |  [inkubator rusak x4]   [G-NE] |
   |  (dinding barat)               |
 B |        ┌────────┐              | T
 A |        │ MEJA   │              | I
 R |        │OPERASI │  <- Lampu    | M
 A |        └────────┘     Operasi  | U
 T |                (gantung)       | R
   | [G-SW]    [cermin retak: timur]|
   +--------------------------------+
              SELATAN (UV-S)
```

| Elemen | Ketentuan |
|---|---|
| Meja operasi | Pusat ruangan, 2,4×1,0 m; tiga pusaka diletakkan di atasnya; sumber `E` untuk ritual |
| Lampu Operasi Gantung (LO) | Di atas meja; beam radius **3,0 m**; sakelar dinding sejauh ≤**4,0 m** dari pusat beam |
| Lampu UV Sterilisasi Utara (UV-N) dan Selatan (UV-S) | Dinding; beam radius **2,0 m**; sakelar menempel pada lampu (≤**3,0 m** dari pusat beam) |
| Generator cadangan G-NE dan G-SW | Dua tiang di sudut berseberangan; **titik daya darurat arena** (§8.0); `E` radius 1,6 m |
| Cermin medis retak | Prop; boleh merefleksikan Lastri (opsional [USUL BARU]); bukan gate |
| 4 inkubator rusak | **Prop wajib** di dinding barat (E11); dipecahkan Lastri pada eskalasi siklus 2 dan 3 |
| Jalur putar | Loop mengitari meja pada radius **2,2–3,2 m**; keliling ±14–20 m; pada 1,7 m/s ±8–12 s per putaran |

**Syarat jangkauan [KLARIFIKASI].** Tiap sakelar dapat dicapai tanpa memasuki collider Lastri; jalur pemain mengitari meja dari setidaknya dua arah; tidak ada dead end; tiga sumber cahaya lingkungan tersebar (tengah, utara, selatan) supaya tiap siklus punya opsi berbeda. **Jarak sakelar-beam** ditetapkan agar pemain yang tiba di sakelar dengan Lastri ±2,5 m di belakangnya masih dapat menempatkannya dalam beam (jarak pemain-pusat ≤ radius+2,5 m).

---


## 4. Sistem ancaman dan state machine AI

### 4.1 Jebakan gas formalin: laci salah (KLARIFIKASI + BASELINE UJI)

**[KANON]** Dokumen otopsi menyatakan laci yang lebih hangat dari −20°C dipasangi jebakan gas formalin; hanya satu laci (C3) yang aman. Naskah tidak menyebut akibatnya; baseline P2 menjadikannya **pengajaran, bukan hukuman fatal** (§8.2).

```text
CLOSED -> PULLED (laci ditarik) -> TELEGRAPH [desis 1,5 s; embun gas terlihat]
                                      |-- E (tutup laci) dalam 1,5 s --> CANCELLED -> CLOSED
                                      +-- timeout --> GAS_ACTIVE [6 s; radius 2 m]
GAS_ACTIVE -> DISSIPATE -> SPENT (jebakan laci itu tidak aktif lagi pada attempt ini)
```

- **Efek GAS_ACTIVE:** layar buram dan vignette hijau-pucat, batuk Raka, kecepatan gerak ×0,8 selama 6 s. **Tidak ada damage, tidak ada Game Over, tidak ada pengurangan baterai.**
- **Efek taktis:** Lastri yang sedang lepas langsung memulai telegraph re-attach (**1,5 s**, bukan 3 s). Bila Lastri sudah menempel, meter sentuhan **naik 1,5×** selama gas. Efek ini menghukum ketidaktelitian tanpa menjebak pemain.
- Jebakan dikembalikan aktif saat memuat checkpoint pra-boss (bukan state persisten); setelah P2_BOSS_WIN semuanya tetap nonaktif agar backtrack aman. Pembacaan sensor sebelum menarik laci adalah satu-satunya cara aman; **prompt tarik laci menampilkan suhu terakhir yang dibaca** bila laci itu sudah dipindai.
- **B2** (−18°C) tetap terjebak (E3). Setelah SPENT, laci terbuka menampilkan isi prop (kantong jenazah kosong, label beku); CANCELLED menutup laci dan mengembalikan jebakan ke CLOSED. Tidak ada item wajib di laci selain C3.
- Hindari audio gas yang menutupi telegraph Lastri; formalin memakai bus efek, bukan bus telegraph (§4.3A).

### 4.2 Lastri menempel di kamar mayat: pengawasan Selfie (KLARIFIKASI + BASELINE UJI)

**[KANON]** Lastri berdiri tepat di belakang punggung korbannya; tidak terlihat oleh mata biasa atau kamera belakang; terlihat di kamera depan (Selfie). Di fase investigasi ia kebal. **Menoleh langsung (tanpa lewat Selfie) saat ia menempel = *Instant Game Over*.** Melepas tempelan: tetap berjalan ke depan sambil menatap Selfie dan menjaganya dalam frame **5 detik** hingga memudar mundur, ATAU masuk lemari besi/kolong meja dan mematikan layar `F`. Selama memindai laci, setiap **20 detik** terdengar rintihan dari belakang punggung.

```text
DORMANT -> TELEGRAPH_SCARE1 -> ATTACHED_LEARN (jendela belajar; nonfatal)
ATTACHED_WATCHED  <->  ATTACHED_UNWATCHED
   | in-frame + maju >=0,8 m/s: holdTimer += dt   (paused saat berhenti/keluar frame; TIDAK reset)
   | holdTimer >= 5,0 s --> DETACH_FADE (2 s mundur) --> DETACHED
   | hiding + screen off --> LOITER (6 s) --> DETACH_FADE
ATTACHED_UNWATCHED:  touchMeter += 10%/s ; saat in-frame: touchMeter -= 25%/s
   meter = 100% --> HAIR_TOUCH (2 s: heartbeat, vignette, bisikan) --> GRAB (Game Over)
BODY_TURN_TO_LASTRI (>=0,25 s; luar jendela belajar; luar cahaya ritual) --> POUNCE (Game Over)
DETACHED --> (reattachInterval 20 s; 45 s setelah MORGUE_CLEARED) --> MOAN_TELEGRAPH (3 s) --> ATTACHED_*
```

**Pengajaran sebelum fatal [KLARIFIKASI].** Scripted scare #1 memicu peringatan HP; selama `ATTACHED_LEARN` kedua fatal diblokir; pelanggaran menoleh hanya memberi *flinch* (§3.3A). Setelah lepas pertama atau 30 s, aturan penuh berlaku. Pemain yang mengabaikan peringatan mendapat petunjuk diegetik (hembusan dingin di tengkuk, bayangan rambut di tepi layar, jantung berdetak) sebelum meter penuh.

**Ukuran "in-frame" [KLARIFIKASI].** Gunakan bounds target terproyeksi pada frustum Selfie (≥30% projected rect di layar, ≥3/9 LOS anchor; grace occlusion ≤0,25s), bukan raycast titik; objek sesaat yang harus dilalui (tiang kecil, tepi meja) tidak menghentikan hold. Timer `holdTimer` hanya naik bila Raka bergerak maju (≥0,8 m/s) dan Lastri in-frame; diam tidak menghukum langsung tetapi menghentikan hold.

**ThermoKlenik di Selfie [USUL BARU, mengajar mekanik boss].** Selama Lastri menempel dan Selfie aktif, overlay Thermo menampilkan **−20°C** bila reticle memilih sosok asli dengan LOS pada jarak ≤3,0 m (range boss ≤8m). Tanpa target valid tampilkan ambient atau “tidak ada pembacaan”; jangan membuat angka −20 otomatis hanya karena Lastri berada dekat di luar frame. Ini menanamkan "−20°C = Lastri asli" sebelum bos menghadirkan ilusi (§4.4). Overlay tidak menambah drain tersembunyi (profil mode aktif).

**Hiding [KLARIFIKASI].** Spot sembunyi punya kamera tersendiri; layar HP dimatikan dengan `F`. Selama `LOITER`, Lastri berdiri di luar spot dan menghadap pintu; pemain yang menyalakan layar sebelum selesai mereset `LOITER` ke 6 s tetapi **tidak** langsung fatal (konsisten dengan Phase 1: konsekuensi stealth mengikuti satu aturan jelas). Spot tidak boleh menjadi jebakan; keluar selalu tersedia dan setelah `DETACH_FADE` interval re-attach tetap berjalan.

**Fatal yang diperbolehkan [BASELINE UJI, P2-D13].** Hanya `POUNCE` (menoleh langsung) dan `GRAB` (meter penuh + 2 s) yang fatal. **Di boss, idle-lunge dan meter out-of-frame adalah varian `GRAB` dengan telegraph sendiri, bukan fatal ketiga.** **Dilarang:** teleport acak Lastri ke belakang pemain; spawn tepat di atas pemain; re-attach saat pemain terkunci dalam dialog/Jurnal pertama kali tanpa telegraph; menambah aturan fatal ketiga tanpa keputusan.

**Interaksi dengan UI [KLARIFIKASI].** Jurnal/app nonmodal tidak membekukan dunia; grace `GRAB` 4s diberikan **sekali per attach**, tidak di-reset oleh buka/tutup UI. POUNCE tetap mengikuti jendela belajar/turn yang sah. Karena dokumen/keypad hard-modal existing memblokir input dunia, **menolak pembukaan hard modal baru saat attached** dengan petunjuk lepas/sembunyi dahulu; jangan mengunci mouse/kaki lalu menghukum pemain. Saat hard modal dibuka dari keadaan aman, hentikan telegraph/attach/drain bersama sampai tutup, lalu telegraph baru penuh. Cutscene wajib dan pause membekukan gameplay clock; animasi pickup singkat bukan cinematic/invulnerability. Kebijakan focus/pause terpusat di GameShell kelak, bukan timer UI terpisah.

### 4.3 Lastri di Lantai 2: patroli agresif (KLARIFIKASI + BASELINE UJI)

**[KANON]** Pada L2 Lastri berpatroli lebih agresif dan mematikan lampu lorong.

```text
L2_PATROL: (segmen S1..S3) -> DARKEN_SEGMENT [flicker 2 s -> padam]
            -> (reattachIntervalL2 12 s) -> MOAN_TELEGRAPH (2,5 s) -> ATTACHED_* [aturan §4.2; meter 12%/s]
QUIET_ZONE (Ruang Inkubator, Ruang Jaga Suster) -> tidak ada re-attach baru; yang menempel memasuki detach aman saat kapsul melintasi ambang, juga pada baterai 0%/layar mati; tidak mensyaratkan Selfie
OR_DOOR_SEALED: Lastri tidak muncul di Ruang Operasi sebelum ritual
```

- **Konsekuensi lampu padam:** segmen gelap mengalikan laju meter 1,5× dan mengurangi jarak baca; navigasi tetap terbaca (§3.5, §8.2).
- **Zona tenang** (dua ruangan kecil) memberi ruang aman untuk EVP, brankas, dan membaca dokumen; pemain tidak boleh dihukum karena tidak dapat berjalan 8,5 m di ruang sempit.
- Lastri L2 memakai **brain yang sama** dengan kamar mayat (parameter berbeda di `Phase2BalanceConfig`); tidak ada AI baru. Tidak ada Tuyul/Genderuwo.
- **Retry:** memuat CP4/CP5 terakhir memulihkan segmen lampu, posisi Lastri, dan meter 0; tidak mengisi baterai.

### 4.3A Kedap suara, EVP, dan petunjuk yang tidak bergantung chat (KLARIFIKASI)

**Kedap suara [KANON + KLARIFIKASI].** Setelah `P2_MORGUE_ENTER`, bus **ExteriorBus** diturunkan hampir mati (≈−40 dB) selama pemain berada di zona kamar mayat; suara luar hilang total. Tetap terdengar: napas Raka, detak jantung, dengung kulkas, dan **bus Telegraph** (rintihan, hembusan, desis formalin) yang tidak melewati filter peredam. Tidak ada suara musik gelombang tinggi yang menutup telegraph. Efek kedap berhenti saat pemain melewati ambang pintu basement ke landing.

**EVP Recorder [KLARIFIKASI].** Tiga langkah: (1) **ditemukan**: mendekati Ruang Inkubator, speaker HP menerima statis radio dan ikon EVP muncul (tersedia juga via Jurnal); (2) **rekam**: arahkan ke Inkubator 04 pada jarak ≤2,0 m dan aktifkan `C` untuk sesi **6,0 s**; `C` lagi membatalkan (progres reset bila putus; tanpa penalti); (3) **hasil**: suara perempuan *"Kembalikan... anakku... di meja operasi..."* + centang Bukti 3. Rekaman tidak menambah drain di luar profil mode aktif; tidak meminta mikrofon OS atau audio internet. Tidak ada ambush Genderuwo di P2; satu-satunya "konsekuensi" EVP adalah Lastri L2 mengalami interval re-attach yang sama (zona tenang melindungi sesi rekam).

**Petunjuk lingkungan non-chat.** Selain log/papan/sensor untuk `1603` dan catatan suster/gema AR untuk `2604`, chat memberi petunjuk tambahan (Selfie, punggung) tetapi tidak satu pun puzzle wajib bergantung pada chat atau pada koneksi internet.

### 4.4 Lastri: pertarungan (fase C — Alan Wake × DreadOut × Selfie)

**[KANON]** Arena Ruang Operasi; tiga pusaka diletakkan di meja; pintu terkunci, lampu padam, Lastri muncul di langit-langit dengan **Kabut Sengkala** lalu menghilang. **3 siklus penyegelan**: lacak lewat Selfie (berjalan maju mengitari meja) → hancurkan kabut dengan lampu operasi/UV atau flare → perisai pecah, memadat, boleh dipotret dari belakang (Selfie) atau depan (setelah membalik badan), jendela **5 detik** → Blitz valid menghancurkan 1 lapisan. Setelah tembakan pertama dan kedua, kaca inkubator pecah dan muncul **2 bayangan palsu**; sosok asli = yang menurunkan suhu ke **−20°C**. Blitz ketiga menyegel Lastri.

```text
TABLE_READY --E valid + CP5 sukses--> RITUAL_START (pusaka di meja; pintu terkunci; lampu padam)
  -> PREP (minimum 30 s; Lastri tak menyerang; generator arena tersedia; lanjut lewat E sadar)
  -> CYCLE_n (n=1..3)
       TRACK (Selfie; Lastri berperisai 2,0-3,0 m di belakang; berjalan maju mengitari meja)
         |- player idle >3,0 s --> LUNGE_TELEGRAPH (1,0 s) --> GRAB (Game Over) kecuali pemain bergerak
         |- out-of-frame (meter 15%/s) --> HAIR_TOUCH --> GRAB
       -> LURE (pancing ke beam/radius flare)
       -> LIGHT_ACTIVE [lampu: burst 2,0 s | flare: 10 s, radius 3,0 m]
            |- cahaya mengenai sosok ASLI --> SHIELD_BREAK
            +- cahaya mengenai ilusi/kosong --> (ilusi lenyap | cooldown lampu 12 s) -> TRACK
       -> EXPOSED (5,0 s; Lastri memegangi wajah; tanpa serangan/ilusi baru; ilusi lenyap)
            |- BLITZ_VALID --> SEAL_LAYER++ (RAM) ; sumber lampu korslet
            |        n<3: RECOVERY_INTERLUDE | n=3: VICTORY
            +- MISS/TIMEOUT --> RESHIELD (kabut kembali) ; lampu UTUH ; flare tetap terpakai -> RECOVERY_INTERLUDE
  -> VICTORY -> CP6 atomik -> kain gendongan terbakar di meja
RECOVERY_INTERLUDE / POWER_RECOVERY: Lastri mundur lewat jalur valid tanpa damage; meter/turn fatal berhenti; ilusi dibersihkan; generator fisik repeatable; lanjut siklus hanya lewat E sadar di meja setelah waktu minimum, HP/Selfie saja tidak mengakhiri recovery
```

**Invariant wajib (KLARIFIKASI):**

- Hanya sumber dengan status `available` yang dapat memulai kesempatan; lampu yang sudah korslet tidak dihitung lagi. **Tersedia tiga sumber lingkungan** (LO, UV-N, UV-S) sehingga tiga siklus selalu dapat diselesaikan **tanpa flare**; flare adalah pengganti sumber, bukan syarat.
- `exposedUntil` dihitung dari waktu gameplay yang valid; pause/cutscene instruksional tidak mengurangi 5 s.
- **Blitz sah bila:** boss `EXPOSED`; baterai ≥15%; cooldown selesai; target kepala valid dengan toleransi ±20° dan jarak ≤8 m dengan LOS; dan **salah satu** dari: (a) **Selfie Blitz** ritual (pemancar depan; Lastri dalam frame Selfie, ≥30% bounds); (b) **Blitz belakang** setelah Raka membalik badan dan membidik Lastri dengan kamera belakang. **Blitz kamera belakang saat Raka menghadap ke depan dan Lastri di belakang tidak mengenai** (lihat catatan aturan khusus pada Adegan 5).
- Klik berkali-kali pada window yang sama tidak memberi poin tambahan; **maksimal satu Blitz valid per siklus** (window 5 s lebih pendek dari cooldown 6 s).
- **Aturan menoleh (KANON):** larangan menoleh hanya berlaku saat Lastri **menempel dan belum terkena cahaya ritual**. Selama `EXPOSED`, menoleh aman penuh. Saat `RESHIELD`, larangan aktif lagi setelah **grace 0,5 s** dan telegraph (flicker + gong) agar pemain yang masih membalik badan tidak mati tanpa peringatan.
- Setelah Blitz ketiga: matikan meter/AI; simpan state boss kalah dalam CP6; efek tubuh tercabik menjadi kabut abu masuk lensa `MataBatin.apk`; pintu terbuka; **jangan** menjadwalkan autosave selama `BOSS_ACTIVE`.

**Eskalasi siklus 2 dan 3 [KANON + KLARIFIKASI].** Setelah Blitz 1 dan 2, Lastri memecahkan **dua kaca inkubator tua** (telegraph 2 s, VFX saja; **bukan damage**) pada dinding barat dan menciptakan **2 bayangan palsu** pada awal `TRACK`. Ilusi melayang 2,0–5,0 m di belakang Raka pada sudut berbeda, terlihat di Selfie persis seperti Lastri, tetapi **tidak menyentuh/menempel**; bila mendekat ≤1,5 m ia pecah dengan jeritan dan VFX tanpa damage, lalu respawn setelah 6 s. Pembacaan Thermo di pojok layar: **−20°C hanya bila target yang dipilih reticle Selfie dan lolos LOS adalah yang asli**; ilusi terbaca **−6°C**. Cahaya yang mengenai hanya ilusi tidak memecahkan perisai apa pun, melainkan melenyapkan ilusi itu (strategi pembersihan) dan memberi cooldown 12 s pada lampu.

**Pengaturan siklus dan sumber daya [BASELINE UJI].**

- **PREP minimum 30 s** dan **RECOVERY_INTERLUDE minimum 20 s** memberi waktu memakai generator; bukan batas maksimum. Lanjut hanya lewat `E` sadar pada meja dengan baterai ≥15%, layar bisa dipakai, dan cooldown Blitz selesai. Mengangkat HP tidak memulai siklus. Selama recovery layar boleh mati (drain 0); tidak ada ancaman aktif.
- **Generator G-NE dan G-SW:** tahan `E` radius 1,6 m, LOS tangan; **+25 poin/8 s, dapat diulang secara fisik selama recovery**, termasuk setelah boss untuk epilog. Tidak ada isi otomatis, grant retry atau Blitz gratis. Cap darurat 10% eksplorasi **tidak berlaku** pada titik daya arena: 10% tidak cukup untuk Blitz 15%. Sesudah menyelesaikan resolusi hit/VICTORY pada frame itu, jika daya <15% saat TRACK atau setelah miss, masuk `POWER_RECOVERY` dengan telegraph, ancaman fatal dihentikan dan Lastri mundur lewat jalur valid; siklus/hit/lampu tidak di-reset. Generator nonaktif selama EXPOSED. Alternatif ini mengikuti pemulihan arena P1; versi sekali-per-fase tidak menjamin percobaan ulang tak terbatas (§15.1).
- **Anggaran baterai:** satu siklus ≈ 15% (Blitz) + (60–90 s × drain Selfie 0,05–0,10) ≈ 18–24%; tiga siklus ≈ **54–72%** tanpa miss. Dua generator memberi minimum 50 poin pada dua operasi normal; pengisian manual dapat diulang pada recovery bila habis karena miss. Kelayakan dari 0% harus diuji pada QA-32/83; tidak diasumsikan dari hitungan no-miss. Kebijakan ini menggantikan alternatif D01 Phase 1 (Blitz bertenaga lampu) dan tidak menjadikannya default.
- **Tiga sumber lingkungan**: setiap lampu memberi satu siklus sukses lalu korslet. **Flare** memberi radius 3,0 m selama 10 s dan **terpakai begitu dijatuhkan**, walau tidak mengenai asli; flare dipulihkan oleh snapshot CP5 saat retry.

**Pembagian peran lampu [KLARIFIKASI].** LO (tengah) mencakup jalur sempit mengitari meja (radius 2,2–3,0 m); UV-N/UV-S mencakup tepi utara/selatan; flare fleksibel tetapi terbatas. Pemain memilih lintasan, bukan sekadar menekan sakelar yang nyaris selalu mengenai.

### 4.5 Denah dan AI arena boss (BASELINE UJI)

Arena §3.6. **Gerak Lastri:** melayang, mengabaikan collision dinamis kecuali dinding; mengikuti *last known position* pemain dengan jeda 0,8 s; memelihara jarak **2,0–3,0 m** di belakang jalur; **tidak** spawn tepat di atas atau di depan pemain; menyesuaikan kecepatan ke `boss.stalkMps` (2,1 m/s) agar tetap membuntuti tetapi tidak menabrak. Pada `EXPOSED` ia **diam** menutup wajah (tidak menyerang, tidak teleport) sehingga hitbox kepala stabil.

**`LURE`:** untuk lampu, `bossInsideBeam && lampAvailable && playerNearSwitch` memenuhi aktivasi; sakelar lampu memanggil burst 2,0 s yang memeriksa overlap beam–Lastri asli. Pemain yang menarik sakelar saat Lastri di luar beam tidak mendapat apa pun selain cooldown lampu 12 s. Untuk flare, `G` menjatuhkan titik radius 3,0 m di kaki Raka; Lastri yang masuk radius selama ≥0,3 s memecahkan perisai.

**Exposure:** `EXPOSED` selama 5,0 s dengan reticle merah di kepala; **tidak ada ilusi atau serangan** selama window. Tepat satu Blitz valid menandai siklus selesai; miss/timeout me-reset perisai **tanpa menghancurkan lampu**. Setelah Blitz sukses lampu pemicu korslet (VFX murah, pool).

**Kenapa bukan combat bebas [USUL BARU].** Tidak ada proyektil, tidak ada darah Raka, tidak ada sistem health. Ketegangan datang dari ruang gelap, kamera Selfie, meter, dan waktu. Pecahan kaca inkubator hanya prop/VFX; **jangan** menambahkan damage kaca tanpa keputusan.

---

## 5. Bukti, Jurnal Klenik, inventori dan item

### 5.1 Master item dan dokumen (KANON + KLARIFIKASI)

| Jenis | Item / ID logis | Lokasi | Cara dapat | Fungsi |
|---|---|---|---|---|
| Dokumen bonus | Artikel Forum "Tragedi RSUD Sido Waras 2001" — `P2_DOC_FORUM` | PC Kos | Baca | Tiga fakta dipakai ulang: Lastri (26), 17 jasad bayi, rambut merah menyentuh bahu. **Bukan** Dokumen #1–#4 dan bukan gate |
| Dokumen #1 | Buku Jaga Perawat Malam, 11 Oktober 2001 — `P2_DOC_NURSE_LOG` | Meja resepsionis IGD | Baca/interaksi | Rumus kode keypad: suhu poli tempat pasien terakhir diperiksa + nomor poli yang sama; petunjuk "poli kandungan" |
| Dokumen #2 | Laporan Otopsi Rahasia Dr. Hendra — `P2_DOC_AUTOPSY` | Meja otopsi tengah | Baca, diambil bersama Kunci Teralis | Petunjuk laci paling dingin; peringatan laci lebih hangat dari −20°C |
| Dokumen #3 | Perekam Suara Damar — `P2_DOC_DAMAR_RECORDER` | Inkubator 04 | Ambil/putar; dapat diputar ulang dari Jurnal | Arah cerita: terowongan bawah tanah menuju Pabrik Gula Sindang Laut |
| Dokumen #4 | Catatan Suster Jaga, Lantai 2 — `P2_DOC_NURSE_NOTE` **[USUL BARU]** | Meja stasiun Ruang Jaga Suster | Baca | Rumus kode brankas obat (usia pasien terakhir + nomor inkubator bayinya) |
| Clue prop | Catatan Perawat di samping Laporan Otopsi — `P2_CLUE_MORGUE_NOTE` | Meja otopsi tengah | Baca | "Yang dihapus dari daftar tidak membeku bersama jenazah." Petunjuk rahasia D1; bukan Dokumen bernomor |
| Clue prop | Daftar Jenazah Bayi — Bangsal Bersalin 2001 | Dinding gudang administrasi | Amati/zoom | 17 nomor baris, 16 nama; baris ke-17 kosong. Pasangan clue D1 |
| Key item | Kunci Teralis Lantai 2 — `P2_KUNCI_TERALIS_L2` | Meja otopsi tengah | Ambil bersama laporan (`P2_AUTOPSY_REPORT`) | Membuka teralis tangga L2; **tidak** bergantung pada Laci C3 |
| Key item | Kunci Arsip — `P2_KUNCI_ARSIP` | Papan kunci gudang administrasi (terlihat dari pintu) | Ambil | Membuka laci arsip D1; tidak dikonsumsi |
| Consumable | Powerbank Pasar Gaib ×2 — instance `P2_POWERBANK_PASAR_GAIB_A` / `_B` | Pasar Gaib (opsional) | Beli paket | +80 poin tiap unit [BASELINE UJI]; ID instance berbeda agar tidak ditolak `AddInventoryItem` |
| Consumable | Kemenyan Bakar ×2 — `P2_KEMENYAN_A` / `_B` | Pasar Gaib (opsional) | Beli paket | **Hanya disimpan**; tidak punya fungsi dan tidak dikonsumsi di P2 |
| Consumable | Flare Merah Darurat ×3 — `P2_FLARE_A` / `_B` / `_C` | Pasar Gaib (opsional) | Beli paket | Sumber cahaya lempar `[G]` **hanya di boss**; pengganti lampu, bukan syarat |
| Bukti #1 | Suhu beku Poli 03 (−16°C) — `P2_FREEZE_POLI03` | Kaca Poli 03 | Sensor stabil ≥1,5 s, ≤3 m | Flag Jurnal; menurunkan kode `1603` |
| Bukti #2 | Residu aura tumbal toples — `P2_AURA_TUMBAL_C3` | Laci C3 | Foto AR, toples di frame ≥1,0 s | Flag Jurnal |
| Bukti #3 | EVP tangisan bersalin — `P2_EVP_INKUBATOR04` | Inkubator 04 | Rekam ≤2,0 m, toggle `C`, sesi 6,0 s | Flag Jurnal |
| Pusaka #1 | Gunting Tali Pusar Berkarat — `P2_GUNTING_TALI_PUSAR` | Laci C3 | Ambil setelah rantai gaib patah | Diletakkan di meja operasi |
| Pusaka #2 | Gelang Identitas Bayi Lastri — `P2_GELANG_BAYI_LASTRI` | Inkubator 04 | Ambil (tidak dikaitkan ke bukti EVP) | Diletakkan di meja operasi |
| Pusaka #3 | Botol Darah Segel Mbah Suro — `P2_BOTOL_DARAH_SEGEL` | Brankas obat, Ruang Jaga Suster | Ambil setelah kode `2604` | Diletakkan di meja operasi |
| Story prop | Kain gendongan bayi yang terbakar habis | Meja operasi | Terjadi saat `P2_BOSS_WIN` | **Prop, bukan item**; tidak masuk inventori |
| Story prop | Gantungan kunci kamera Damar + panah kapur | Pintu terowongan servis, gudang | Periksa | Non-gating; memenuhi kalimat donasi (E7, P2-D05) |
| Rahasia | Label Pasien Tanpa Nama — `SECRET_P2_LABEL_PASIEN` | Laci arsip D1, gudang administrasi | Puzzle suhu/AR + kunci arsip + pickup | Arsip Saksi (N dari 5, bukan nomor babak) |

**[KLARIFIKASI]** Bukti #2 adalah **foto aura**; Gunting adalah item fisik terpisah walau satu laci (dua flag: `P2_EVIDENCE_AURA` dan `P2_TAKE_GUNTING`). Hal yang sama berlaku untuk EVP dan Gelang. `evidence_ids`, arsip `CampaignData.documents` dengan receipt dokumen, `inventory`, `relic_ids`, dan `secret_items` berbeda fungsi; tidak membuat daftar `document_ids` paralel; **jumlah dokumen tidak pernah menggantikan tiga bukti**. Empat dokumen bernomor mendukung lore dan clue, tidak menjadi syarat ritual (P1 §11.2 butir 8 berlaku sama).

### 5.1A Item rahasia Babak 2: Label Pasien Tanpa Nama (KANON + KLARIFIKASI)

| Elemen | Ketentuan |
|---|---|
| Nama dan ID | **Label Pasien Tanpa Nama**, `SECRET_P2_LABEL_PASIEN`; opsional; satu dari lima bagian Arsip Saksi |
| Lokasi | Laci arsip **D1** di gudang administrasi basement; terpisah dari Laci C3 yang wajib untuk cerita |
| Petunjuk lingkungan | Catatan Perawat di samping Laporan Otopsi: *"Yang dihapus dari daftar tidak membeku bersama jenazah."* **[KLARIFIKASI]** kalimat ini tidak menyebut angka dan tidak menunjuk D1 |
| Langkah | (1) Baca daftar korban di dinding gudang: 17 nomor baris, hanya 16 nama. (2) Pindai lemari arsip dengan ThermoKlenik: **D2–D4 membaca 12°C (ambient)**, **D1 membaca 17°C** dan AR memantulkan angka **17** di atasnya. (3) Cocokkan 17 dengan baris ke-17 yang kosong. (4) Ambil Kunci Arsip dari papan kunci. (5) Buka D1, ambil label |
| Isi kesaksian | Label menyimpan identitas saksi yang sengaja dihapus dari catatan rumah sakit; stempel di belakangnya berbentuk **mata tertutup** |
| Penerimaan | Tidak dihitung sebagai bukti atau pusaka boss; **tidak ada saweran** yang menentukan aksesnya |
| Persistensi | Saat `P2_SECRET_PICKUP`: tambah ID ke `secret_items` **sekali**, **autosave atomik segera**, baru tampilkan indikator sukses dan sembunyikan objek dunia. Bila save gagal, objek tetap ada untuk dicoba lagi |
| Tampilan | Jurnal dan panel motor: `Arsip Babak 2: 0/1` atau `1/1`; Arsip Saksi global `N/5` dari **jumlah aktual ID unik** (1/5 bila rahasia P1 belum diambil, 2/5 bila sudah; E6). Jangan memakai nomor babak sebagai jumlah |
| Kematian/reload | Item tetap tersimpan pada checkpoint valid terbaru; kematian tidak menghapusnya |

**[KLARIFIKASI — mengapa petunjuknya tidak bocor lewat jalur utama]** Pemain jalur utama bisa tidak pernah masuk gudang: Laporan Otopsi dan catatan perawat berada di meja otopsi, sedangkan angka 17, daftar korban, dan D1 hanya ada di gudang. Kunci arsip tergantung terlihat dari pintu gudang (E16) supaya pemain yang sudah memecahkan petunjuk tidak berhenti karena kunci tidak tampak. Daftar korban sengaja menyisakan satu baris kosong agar clue terbaca sebagai "yang hilang", bukan sebagai instruksi.

### 5.2 Journal data contract

**[KANON]** Jurnal menampilkan tiga kotak bukti dan status collectible `0/1` atau `1/1` pada babak ini:

1. `[ ] BUKTI KLENIK 1/3: SUHU BEKU EKSTREM (−16°C)`
2. `[ ] BUKTI KLENIK 2/3: RESIDU AURA TUMBAL (LENSA AR)`
3. `[ ] BUKTI KLENIK 3/3: REKAMAN EVP TANGISAN BERSALIN`

Arsip Saksi global adalah `N/5` dari jumlah aktual ID unik yang sudah dimiliki, bukan nomor babak.

**[USUL BARU]** Jurnal memiliki empat tab konseptual: **Bukti**, **Dokumen**, **Arsip Saksi**, **Petunjuk Objektif**. Pada akhir tiga bukti, tampilkan kesimpulan kelemahan Lastri: cahaya memecah Kabut Sengkala, Blitz ke kepala menyegel. Pemain lalu menekan **`[KONFIRMASI IDENTITAS & BUKA RITUAL EKSORSISME]`** untuk mencatat `P2_JOURNAL_CONFIRMED`; jangan otomatis menyamakan tiga scan dengan konfirmasi. Jangan menampilkan solusi kode `1603` atau `2604` sejak awal.

| Tab | Isi minimum |
|---|---|
| Bukti | Tiga kotak di atas; centang hanya dari ID di `evidence_ids`; pembacaan ulang tidak menambah |
| Dokumen | Artikel Forum (bonus) dan Dokumen #1–#4 yang sudah dibaca; rekaman Damar dapat diputar ulang tanpa reward ganda |
| Arsip Saksi | `Arsip Babak 2: 0/1` atau `1/1`; `Arsip Saksi N/5` global |
| Petunjuk Objektif | Objektif aktif; kartu metode eksorsisme (sekali, setelah konfirmasi); catatan −24°C Laci C3 sebagai **petunjuk puzzle, bukan bukti ke-4** (E14); status pusaka `0/3` hingga `3/3`; kekurangan syarat meja (tanpa spoiler) |

**[KLARIFIKASI]** Tombol konfirmasi aktif **hanya bila tiga ID bukti tepat** (`P2_FREEZE_POLI03`, `P2_AURA_TUMBAL_C3`, `P2_EVP_INKUBATOR04`) ada di `evidence_ids` (QA-06). Konfirmasi boleh dilakukan di mana saja setelah bukti lengkap; tidak harus di Ruang Jaga Suster. `P2_JOURNAL_CONFIRMED` adalah receipt satu kali. Jurnal tidak memindahkan bukti ke inventori sebagai tiga screenshot.

### 5.3 Status ekonomi dan live stream

**[KANON]** Angka tetap dari naskah: penonton awal **42.300** (E8), akhir epilog **310.500**; donasi `Penonton_Terakhir` **Rp666.000** (IGD), **Rp1.333.000** (kamar mayat), **Rp6.666.666** (epilog); overlay **Total Saweran Kumulatif Rp164.800.000**.

**[KLARIFIKASI]** Bedakan empat hal secara teknis, seperti P1 §5.3: (a) **uang tersedia** (`wallet_legitimate`); (b) **dana terikat** `Penonton_Terakhir` (`wallet_bound`); (c) **total saweran kumulatif untuk UI cerita**; (d) **reward yang benar-benar diperoleh**. Ketiga donasi `Penonton_Terakhir` di babak ini adalah **peristiwa cerita dari sumber terikat**: tampil sebagai notifikasi, tidak menambah `wallet_legitimate`, dan tidak diproses ulang saat reload.

**Selisih matematika yang harus diketahui (bukan event donatur yang terbukti ada):**

| Komponen | Nominal |
|---|---:|
| Milestone kumulatif akhir P2 (naskah) | Rp164.800.000 |
| Milestone kumulatif akhir P1 (naskah) | Rp78.200.000 |
| **Selisih milestone P1→P2** | **Rp86.600.000** |
| Donasi `Penonton_Terakhir` yang tertulis di P2 (666.000 + 1.333.000 + 6.666.666) | Rp8.665.666 |
| **Sisa yang belum ditulis sebagai transaksi mana pun** | **Rp77.934.334** |

Asumsi hitungan: Rp164,8 juta mencakup Rp78,2 juta milestone P1 dan ketiga donasi terikat (pola yang sama dengan Rp10 juta pembuka P1 yang ikut dalam Rp78,2 juta). Belanja Pasar Gaib (maks. Rp1.600.000) adalah pengeluaran, bukan donasi. **Jangan menciptakan saldo atau transaksi penutup selisih.** Apakah overlay Rp164,8 juta adalah sinematik tetap atau angka transaksi dinamis adalah **P2-D03**.

**[KLARIFIKASI — dampak ke Pasar Gaib]** Naskah menyebut belanja memakai "saldo saweran yang terkumpul dari malam sebelumnya". Karena sumber ledger P1 belum lengkap, saldo `wallet_legitimate` yang tiba di Kos adalah **apa pun yang ada di save**, bisa nol. Karena itu seluruh paket **opsional** dan jalur utama tidak boleh menganggap saldo cukup (QA-01, QA-43).

**[KANON + USUL BARU]** Tidak ada *paywall*; tidak perlu donasi untuk akses, bukti, pusaka, atau boss. Live chat berupa skrip lokal deterministik terhubung event (§7.4), bukan jaringan sungguhan; jalur utama tidak boleh bergantung pada audiens atau koneksi internet.

### 5.4 Katalog Pasar Gaib dan status tantangan saweran (KLARIFIKASI + BASELINE UJI)

**Tidak ada tantangan saweran opsional di Babak 2.** Naskah tidak memuat padanan `P1_SAWER_SIGNBOARD`; jangan menambah `P2_SAWER_*` tanpa keputusan baru. Satu-satunya interaksi uang yang dapat dimainkan adalah belanja Pasar Gaib.

| Paket | ID transaksi | Harga | Isi | Batas |
|---|---|---:|---|---|
| Baterai Cadangan / Powerbank ×2 | `P2_SHOP_PACK_POWERBANK` | Rp500.000 | 2 instance powerbank besar (+80 poin) | Sekali per persiapan P2/campaign |
| Kemenyan Bakar ×2 | `P2_SHOP_PACK_KEMENYAN` | Rp350.000 | 2 consumable tanpa fungsi di P2 | Sekali per persiapan P2/campaign |
| Flare Merah Darurat ×3 | `P2_SHOP_PACK_FLARE` | Rp750.000 | 3 flare lempar untuk boss | Sekali per persiapan P2/campaign |
| **Total bila semua dibeli** | | **Rp1.600.000** | | |

**Aturan:** (1) dua upgrade wajib (Selfie Mode, ThermoKlenik.apk) **tidak dijual** dan terbuka lewat `P2_UPGRADE_UNLOCK`; (2) pembayaran hanya dari `wallet_legitimate`; (3) saldo kurang berarti paket tampil tidak aktif dengan harga terlihat, **tanpa paket gratis pengganti**; (4) pembelian adalah **satu transaksi atomik**: kurangi saldo, tambah instance inventori, tulis receipt (§6.4); (5) memuat ulang di tengah belanja tidak menggandakan barang maupun memotong saldo dua kali; (6) harga mengikuti naskah, jumlah per paket adalah baseline.

Status enum untuk paket: bila perlu dilacak, gunakan satu nilai per paket `PURCHASED` (receipt ada) atau absen; **bukan** boolean ganda yang bisa sama-sama true.

---

## 6. Sistem penyimpanan dan *campaign state*

### 6.1 Prinsip penyimpanan (KANON + KLARIFIKASI)

- Tiga bukti unik, tiga pusaka, status boss, segel lokasi, dokumen, inventori, uang tersedia, Sanity, upgrade, dan Arsip Saksi **harus konsisten setelah reload**.
- Item `SECRET_P2_LABEL_PASIEN` ditambah **sekali** ke daftar ID global `secret_items`, lalu **autosave segera**. Kematian/reload tidak menghapus item yang sudah tersimpan pada checkpoint valid.
- Status objek di scene (**gembok rantai C3, teralis tangga, pintu basement, brankas obat, tembok bata Inkubator, kabut pintu Ruang Operasi, laci terjebak**) direkonstruksi dari data, bukan asumsi default prefab. Laci terjebak **bukan** state persisten; ia aktif kembali saat checkpoint dimuat (§4.1).
- **[KLARIFIKASI]** Gunakan `CampaignState` dan mekanisme atomic save yang **sudah ada**; jangan membuat sistem save paralel. Data spesifik babak disimpan sebagai bagian dari save yang sama.
- **[USUL BARU]** Bedakan progres persisten (barang, bukti, rahasia, boss, segel) dengan *transient runtime*: `holdTimer` Selfie, `touchMeter`, timer 1,5 s formalin, jendela 5 s boss, status ilusi, cooldown lampu 12 s, progres crank dinamo. Reload tidak boleh memuat kondisi mustahil (misalnya Lastri sudah menempel di wajah pemain).
- **[KLARIFIKASI]** Jam cerita (14:00, 23:45, 23:50) adalah **beat naratif**, bukan jam dunia yang dipersist; tidak ada timer 23:59 nyata.

### 6.2 Skema save tanpa sumber kebenaran ganda (USUL BARU)

**Simpan set ID, bukan boolean turunan ganda.** `completed_event_ids` adalah event satu kali; `evidence_ids` = bukti unik; `CampaignData.documents` + receipt `P2_DOC_*` = arsip dokumen existing; `CampaignData.inventory` = kunci/pusaka yang sedang dibawa/instance consumable; `relic_ids` = pusaka yang sudah ditemukan; `secret_items` = collectible global. `stage`, `hasGunting`, `hasKunciTeralis`, `boss.started`, `local_secret_status` **diturunkan** dari data itu saat runtime. **Progres cahaya boss, `P2_TABLE_INTERACT`, `P2_SHIELD_BREAK`, `P2_BLITZ_SUCCESS`, status ilusi, dan jumlah flare yang terpakai pada attempt ini hanya hidup di memori percobaan**, dan harus hilang saat respawn CP5; tidak boleh disimpan ke disk bersama snapshot CP5. `P2_BOSS_WIN`, `location_seals`, dan pengurangan flare yang terpakai pada attempt yang menang harus dimutasi **atomik** dalam satu commit CP6.

```json
{
  "schema_version": 3,
  "chapter_id": 2,
  "checkpoint_id": "PHASE2_CP4",
  "inventory": ["SMARTPHONE_RAKA", "P2_KUNCI_TERALIS_L2", "P2_GUNTING_TALI_PUSAR", "P2_FLARE_A"],
  "evidence_ids": ["P2_FREEZE_POLI03", "P2_AURA_TUMBAL_C3"],
  "relic_ids": ["P2_GUNTING_TALI_PUSAR"],
  "secret_items": ["SECRET_P1_NEGATIF_TARI"],
  "location_seals": ["SEAL_RUSUNAWA_GRIYA_ASRI"],
  "battery": 62,
  "sanity": 100,
  "wallet_legitimate": 0,
  "wallet_bound": 0,
  "spent": 0,
  "documents": [{"title": "Buku Jaga Perawat Malam", "body": "[isi sesuai naskah]"}],
  "phase1": {"completed_event_ids": ["P1_COMPLETE"], "safe_anchor": "PARKING"},
  "phase2": {
    "completed_event_ids": ["P2_UPGRADE_UNLOCK", "P2_DEPART_KOS", "P2_START_LIVE", "P2_KEYPAD_1603", "P2_AUTOPSY_REPORT", "P2_TAKE_GUNTING", "P2_KOS_CHARGED", "P2_SHOP_PACK_FLARE"],
    "safe_anchor": "MORGUE_LANDING",
    "next_destination": "",
    "dynamo_full_charge_used_ids": ["P2_DYNAMO_IGD"],
    "dynamo_emergency_last_uses": [],
    "elapsed_gameplay_seconds": 2310,
    "money_ledger": []
  }
}
```

**[AUDIT — contoh parsial, bukan fixture dapat dimuat]** Loader saat ini memigrasikan 1→2 dan hanya menerima schema 2; contoh schema 3 ini **belum dapat dibaca**. Implementasi kelak menambahkan migrasi 1→2→3 dan validator nested `phase2` di `CampaignState` yang sama (P2-D10). Field P0/P1 yang tidak ditampilkan harus tetap dipertahankan; contoh ini tidak mengganti save riil. Jangan sekadar mengganti angka versi: migrasi harus mempertahankan flag P0, seluruh field P1, dan semua ID rahasia. List receipt dipilih karena `JsonUtility` bukan serializer dictionary/HashSet; runtime boleh membangun set/index dari list, tetapi jangan persist dua salinan nilai yang sama. `evidence_ids`, `inventory`, `battery`, `sanity`, dan `checkpoint_id` tetap milik campaign existing; jangan membuat duplikat di `phase2`.

**Angka dan ID dalam JSON adalah contoh snapshot CP4 (bukan inventori awal kanon).** receipt `P2_KOS_CHARGED` menandai interaksi colokan sekali; `P2_SHOP_PACK_*` menjadi satu-satunya status pembelian, tanpa `shop_receipts` atau boolean duplikat; `dynamo_full_charge_used_ids` menyimpan satu pengisian penuh per dinamo; waktu pemulihan darurat memakai *elapsed gameplay time* (bukan jam dinding OS). Timer 1,5 s formalin, 5 s window, 6 s loiter, jendela belajar 30 s, dan status luka sementara tidak disimpan. Save item rahasia tetap langsung dan sekali saja. `money_ledger` memuat audit transaksi unik (ID event + nominal); `wallet_legitimate`, `wallet_bound`, dan `spent` tetap field saldo authoritative existing, diperbarui dalam transaksi yang sama, bukan dihitung ulang dari ledger P2 yang tidak memuat seluruh kampanye.

### 6.3 Checkpoint yang diusulkan (USUL BARU)

| Checkpoint | Waktu yang baik | State minimal | Recovery |
|---|---|---|---|
| `PHASE2_HUB` | Konfirmasi lanjut dari CP7 P1, lalu setiap transaksi HUB | Prasyarat P1, chapter 2, anchor HUB, inventori/baterai/uang aktual | Continue ke Kos mode P2; tidak menjalankan ulang Prolog |
| `PHASE2_CP1` | Keberangkatan sukses, sebelum fade/load IGD; intro selesai dicatat idempoten | Berangkat, upgrade, belanja, baterai aktual; colokan opsional | Continue ke IGD; crash saat load tidak mengulang charge/belanja |
| `PHASE2_CP2` | Setelah kode `1603` benar | Receipt pintu; bukti/dokumen hanya yang benar-benar diperoleh | Tebak kode tidak memberi bukti gratis; sensor Poli tetap dapat dikunjungi |
| `PHASE2_CP3` | **Pintu basement menutup di belakang pemain, sebelum scare #1** | Posisi landing aman, Lastri `DORMANT`, laci terjebak aktif, baterai tepat nilai saat itu | Retry memuat snapshot tanpa top-up baterai; scare #1 diputar ulang sebagai telegraph, bukan fatal |
| `PHASE2_CP4` | Setelah tiap bukti atau pusaka valid | Set bukti unik, pusaka, dokumen, kunci; **anchor aman**, bukan posisi saat diburu | Hindari re-scan berulang; Lastri `DORMANT`, meter 0 |
| `PHASE2_CP5` | **`E` pertama yang valid di meja operasi** (3 bukti + konfirmasi Jurnal + 3 pusaka); **sebelum** pusaka dipindah, pintu dikunci, lampu dipadamkan | Tiga pusaka masih di inventori, tiga sumber cahaya utuh, flare sesuai inventori, baterai aktual, Lastri belum aktif | Retry selalu dari snapshot ini; flare yang terpakai pada attempt gagal dipulihkan; tidak ada floor baterai |
| `PHASE2_CP6` | **`P2_BOSS_WIN`, transaksi atomik** | Boss kalah + segel kedua + pusaka pindah ke meja + flare terpakai dikurangi + pintu terbuka | Kembali eksplorasi tanpa boss berulang |
| `PHASE2_CP7` | `P2_COMPLETE` | Semua loot, rahasia, ekonomi, segel, tujuan Pabrik Gula | Memulai babak berikutnya tanpa duplikasi |

**Anchor CP4 [KLARIFIKASI].** CP4 tidak boleh ditulis dengan posisi pemain saat Lastri berstatus `ATTACHED_*`. Receipt bukti/pusaka dan penambahan inventori tetap di-commit atomik **segera**; hanya *anchor posisi restore* yang memakai titik aman terdekat: landing basement, depan Pos Petugas, landing tangga L2, atau depan Ruang Jaga Suster. Restore dari CP4 selalu menempatkan pemain di anchor dengan Lastri `DORMANT`, meter 0, dan telegraph yang wajar sebelum re-attach pertama.

**[KLARIFIKASI — atomic checkpoint CP5]** Saat pemain berdiri dalam radius meja operasi dan menekan `E` pertama kali dengan tiga bukti + `P2_JOURNAL_CONFIRMED` + tiga pusaka lengkap (`TABLE_READY` runtime), simpan CP5 **dengan anchor meja aman dan yaw terdefinisi** ke disk **sebelum** memindahkan pusaka, mengunci pintu, memadamkan lampu, atau memicu Lastri. `P2_TABLE_INTERACT`, posisi boss, progres siklus, status lampu, dan jumlah flare terpakai hanya hidup di memori percobaan. **Jangan autosave atau menulis ke disk selama `BOSS_ACTIVE`**, termasuk saat Blitz sukses 1–2 kali, pause, atau cutscene tengah fight. Kematian, restart game, atau reload manual sebelum menang selalu mengembalikan CP5. Bila penulisan CP5 gagal, ritual **tidak dimulai** dan UI menawarkan coba lagi. Setelah tiga Blitz sukses, `P2_BOSS_WIN` melakukan **satu commit atomik CP6**; penulisan gagal mengembalikan snapshot terakhir yang valid (CP5), bukan state separuh jalan.

**[BASELINE UJI]** Saat kalah, reset **semua tiga sumber cahaya lingkungan dan seluruh flare** dari CP5 dengan baterai persis snapshot, tanpa menduplikasi item/uang. Jangan mencampur reset penuh dengan checkpoint parsial 1–2 poin dalam satu versi implementasi (P2-D08).

### 6.4 Idempoten dan transaksi sederhana (KLARIFIKASI)

```text
if PurchasePack(packId) AND wallet_legitimate >= price AND not HasReceipt(packId):
    CommitAtomic(deduct=price, addInventoryInstances=pack.items, receipt=packId+txId) -> SUCCESS | ROLLBACK

if not EvidenceIds.Contains("P2_FREEZE_POLI03"):
    TryCommitAtomic(addEvidence="P2_FREEZE_POLI03", anchor=SafeAnchor)
    // UI/objek berubah HANYA sesudah commit berhasil; gagal = rollback dan boleh ulang

if Keypad.Entered == "1603" AND not Completed("P2_KEYPAD_1603"):
    CommitAtomic(event="P2_KEYPAD_1603", doorBasementOpen=true, checkpoint="PHASE2_CP2")

if EvidenceIds.ContainsAll(RequiredEvidenceIds) AND Completed("P2_JOURNAL_CONFIRMED")
   AND Inventory.ContainsAll(Gunting, Gelang, Botol) AND PlayerPressedInteractAtTable AND PlayerWithinTableRange:
    if EnsureDiskCheckpoint("PHASE2_CP5", playerAtTable=true, relicsInInventory=true) == SUCCESS:
        StartBossAttemptInMemoryOnly()             // pintu terkunci, lampu padam: runtime saja

if bossIsExposed AND validBlitz AND not runtimeCycle[n].completed:
    runtimeCycle[n].completed = true                // jangan tulis ke disk selama boss
    if sourceThatFired.IsLamp: sourceThatFired.Short() // flare dikonsumsi saat dijatuhkan, ledger RAM
    if CountDistinctRuntimeCycles() == 3:
        CommitAtomic("PHASE2_CP6", bossWin=true, addLocationSeal="SEAL_RSUD_LAMA_SIDO_WARAS",
                     relicsToTable=true, consumeFlaresUsed=true)
        if CommitSucceeded: TriggerVictoryEffectsOnce()
        else: FreezeAttemptAndOfferRetryCommitOrRestoreCP5()

if BossAttemptFailedOrReloaded:
    RestoreDiskCheckpoint("PHASE2_CP5")             // tiga sumber utuh; flare pulih; pusaka belum terpasang

if MotorExitPanelOpened:
    ShowArchiveStatusNoSave()                       // tutup panel = tanpa completed event
if PlayerConfirmedExit AND EpilogueComplete AND not Completed("P2_COMPLETE"):
    if CommitAtomic("PHASE2_CP7", addCompletedEvent="P2_COMPLETE",
                    nextDestination="Pabrik Gula Belanda Sindang Laut") == SUCCESS:
        PlayShutterAndCutToBlack()                  // gagal commit => panel tetap bisa dicoba lagi

if TryAddSecretAndCommitSameCampaign("SECRET_P2_LABEL_PASIEN") == COMMITTED:
    ShowCollectedAndHideWorldObject()
else if SAVE_FAILED:
    RestorePreviousDataAndKeepWorldObjectForRetry()
```

Pseudocode mengilustrasikan *state transition*, bukan API baru yang wajib dibuat. P1 kini memakai `CampaignState.TryCommit` (rollback seluruh data), `BeginCheckpointAttempt(owner)` dan `EndCheckpointAttempt(owner)`; reuse API itu. Receipt P2 masuk nested `phase2.completed_event_ids`; arsip dokumen tetap memakai jalur existing. `SaveCheckpointAtomic` saja tidak cukup untuk mutasi yang sudah terjadi. Jangan menyalin implementasi rollback atau membuat event bus baru. Selama boss, lindungi **seluruh jalur tulis** (termasuk `ArchiveDocument`, pembayaran, callback UI, dan notifikasi donasi), bukan hanya autosave director. Indikator collectible sukses baru tampil setelah write berhasil.

**[KLARIFIKASI — flare dan CP6]** Jumlah flare terpakai pada attempt menang dihitung di RAM dan **dikurangkan dari inventori pada commit CP6 yang sama**. Pemain yang menang tanpa flare tidak kehilangan flare. Flare yang dijatuhkan pada attempt yang gagal tidak pernah terkurangi di disk, sehingga retry tidak memerlukan pemulihan khusus selain memuat CP5.

---

## 7. Presentasi naratif, atmosfer dan aset

### 7.1 Daftar beats sinematik (KANON + KLARIFIKASI)

| Adegan | Beat audio/visual | Tujuan dramatis |
|---|---|---|
| Kos, 14:00 | Siang, kendaraan kota samar dari jendela, monitor PC menyala | Jeda manusiawi setelah malam Rusunawa; Raka pulih dengan nyaman sebelum turun ke malam |
| Forum di PC | Artikel Tragedi 2001, monolog Raka tentang "kamera depan" | Menanam aturan Selfie sebagai informasi sebelum dipaksa dipakai |
| Pasar Gaib | Katalog, harga, denting belanja, flare dan kemenyan | Keputusan sumber daya; kemenyan sengaja tanpa penjelasan |
| Berangkat 23:45 | Kunci motor, fade, hujan di pelataran | Peralihan siang ke malam |
| IGD | Pintu kaca pecah, brankar berkarat, bau antiseptik busuk, bunyi live | Lokasi sebagai bekas tragedi; dunia digital ikut masuk |
| Live + donasi Rp666.000 | Notifikasi merah, kalimat "Lastri tidak suka dipunggungi" | Foreshadow bahaya punggung; donor ancaman kembali |
| Poli 03 | Kaca membeku, bunga es, bip cepat ThermoKlenik | Demonstrasi **wajib pertama** ThermoKlenik sebagai alat deduksi |
| Keypad 1603 | Bunyi tombol, kunci besi terbuka, udara dingin dari tangga | Pemain merasa pintar karena menyimpulkan sendiri |
| Kamar mayat, kedap suara | Suara luar lenyap; napas, detak jantung, dengung kulkas | Isolasi total; pemain hanya punya diri sendiri dan HP |
| Hembusan dingin + scare #1 | Uap di tengkuk, peringatan HP, sosok senyum di layar Selfie | Membalik cara melihat: layar lebih jujur daripada mata |
| Rintihan tiap 20 detik | Rintih dari belakang, Selfie diangkat refleks | Ritme tekanan yang dapat dipelajari |
| Laci C3 | Rantai bunga es patah, toples janin, aura merah di AR | Tragedi manusia di balik puzzle |
| Naik ke L2 | Lampu lorong padam segmen demi segmen, tirai bergoyang | Aturan lama dipakai di ruang baru, lebih agresif |
| Tembok bata Inkubator | Bata menjadi transparan di AR, celah pintu 2001 | AR sebagai alat membuka jalur (setelah Babak 1) |
| EVP Inkubator 04 | Statis radio, tangis bayi, suara perempuan | Bukti ketiga; Lastri sebagai ibu, bukan sekadar hantu |
| Rekaman Damar | Suara Damar di perekam kecil, bunyi hujan di latar | Menyambung rantai cerita antar babak |
| Brankas obat | Siluet suster 2001 menekan empat tombol, tombol menyala sekilas | Pengungkapan masa lalu sebagai petunjuk |
| Ruang Operasi | Pintu terkunci, lampu padam, cahaya merah berkedip, kabut merah pekat | Pergeseran genre dari investigasi ke ritual |
| Siklus penyegelan | Lacak di Selfie, lampu operasi menyala, Blitz putih | Penghargaan atas pembacaan mekanik, bukan peluru |
| Kaca inkubator pecah | Dua bayangan palsu, ThermoKlenik −20°C vs −6°C | Penonton dan pemain sama-sama menguji "mana yang asli" |
| Blitz ketiga | Tubuh menjadi kabut abu putih tersedot ke lensa `MataBatin.apk`; kain gendongan terbakar | Kemenangan yang sunyi dan sedikit sedih |
| Epilog | Hujan deras terdengar, napas Raka gemetar, 310.500 penonton | Beban popularitas di pundak Raka |
| Video tungku | Layar merah tua mengunci chat, Damar diikat, rantai diseret | Menarik pemain ke Pabrik Gula Sindang Laut |
| Ending | Pelataran IGD, panel arsip, `CKREK!`, hitam | Transisi dengan sadar, bukan paksa |

### 7.2 Prop dan art direction spesifik (USUL BARU)

**Kos:** meja komputer rakitan, jendela ke jalan, colokan di meja PC yang terlihat jelas, kunci motor di meja, kardus paket dari Pasar Gaib bila ada pembelian.

**Pelataran/IGD:** motor Raka, genangan hujan, pintu kaca geser pecah, brankar berkarat, meja resepsionis dengan Buku Jaga, lampu neon berkedip, dinamo di balik meja, poster kesehatan yang lapuk.

**Koridor Poliklinik:** kursi tunggu besi, papan jadwal poli terbaca (Poli 03 = Kandungan & Kebidanan), empat pintu dengan jendela kaca, **kaca Poli 03 dengan bunga es** dan kartu rekam medis berinisial **L.** yang terbaca saat zoom, pintu besi basement bertuliskan "INSTALASI KAMAR MAYAT (BASEMENT)" dengan keypad, tangga L2 dengan teralis besi.

**Basement/kamar mayat:** busa peredam akustik rusak dan lembap, **dua meja otopsi stainless**, kolong meja (hiding), **dinding 12 laci pendingin besi** `A1`–`C4` dengan label aus, **rantai bunga es** pada C3 (terlihat tegas di AR), kantong jenazah kosong berembun di B2, lemari besi di Pos Petugas, dinamo manual, **gudang administrasi** dengan rak kardus, papan kunci, daftar korban, lemari arsip D1–D4, **pintu terowongan servis berantai** dengan gantungan kunci kamera Damar dan panah kapur.

**L2:** lorong ±36 m dengan tiga segmen lampu, **inkubator bayi kosong berkaca pecah**, tirai rumah sakit yang bergoyang sendiri, **tembok bata Ruang Inkubator** (hanya tampak transparan di AR), enam inkubator kaca 01–06 dengan nomor aus, Ruang Jaga Suster dengan meja stasiun, papan jadwal piket, **papan penempatan bayi (baris 04 berinisial L.)**, **brankas obat 4 digit**, dinamo.

**Ruang Operasi:** meja operasi berkarat, **Lampu Operasi Gantung** besar, **UV-N** dan **UV-S** dengan sakelar yang terbaca, dua generator cadangan G-NE/G-SW, **cermin medis retak**, dan **empat inkubator rusak berjejer di dinding barat (wajib, E11)**. Sakelar, generator, dan sumber cahaya wajib punya fixture dan kabel yang masuk akal.

**[USUL BARU]** Motif berulang: **nomor yang hilang** (baris ke-17 yang kosong, nama yang dihapus, label tanpa nama), embun beku yang menjalar dari sumber dingin, dan angka suhu yang berubah. Jangan seluruh dinding bertuliskan petunjuk merah terang; petunjuk AR/Thermo menjadi kejutan lokal yang perlu dicari. Semua teks puzzle harus terbaca pada target resolusi dan cocok dengan lokalisasi bahasa Indonesia. **Dilarang** menaruh jawaban `1603` atau `2604` sebagai tulisan statis sebelum puzzle dipahami. Gema AR `2604` adalah pengungkapan kontekstual yang disengaja; jurnal boleh mencatat setelah pengamatan, bukan sejak awal.

### 7.3 Audio dan VO (KLARIFIKASI + USUL BARU)

**WAJIB konten naskah:** monolog Raka (forum, siaran, kamar mayat), sapaan live, rintihan Lastri, hembusan dingin, desis formalin, suara EVP perempuan *"Kembalikan... anakku... di meja operasi..."*, suara Damar pada perekam kecil, chat penting, jeritan Lastri saat perisai terbakar, kaca inkubator pecah, Blitz, shutter `CKREK!`, hujan epilog, video tungku Damar (dengusan napas dan rantai diseret). Suara nonverbal harus ikut berjalan walau subtitle/dialog di-skip.

**Pembagian peran audio [KLARIFIKASI — kedap suara].** Nama bus di bawah konseptual. Mixer existing hanya Master/Ambience/Effects/UI; routing Exterior/Telegraph/Voice harus diintegrasikan terlokalisasi saat implementasi (§14.1), tanpa membuat event bus baru.

| Bus | Perilaku di kamar mayat |
|---|---|
| `ExteriorBus` (hujan, kendaraan, suara lantai atas) | Turun ≈ −40 dB selama pemain di zona kamar mayat; pulih saat melewati ambang pintu basement |
| `AmbienceBus` (dengung kulkas, napas, detak jantung) | Tetap terdengar; menjadi lapisan dominan |
| **`TelegraphBus`** (rintihan, hembusan, desis formalin, gong flicker) | **Tidak diredam, tidak ter-ducking oleh musik**; prioritas tertinggi |
| `EffectsBus` (gas, kaca, VFX) | Ducking ke bawah `TelegraphBus`; formalin tidak boleh menutup telegraph Lastri |
| `VoiceBus` (Raka, EVP, Damar, VO) | *Priority ducking* terhadap ambience |
| `UIBus` (chat, notifikasi, bip Thermo) | Volume diatur terpisah; bip Thermo mengikuti jarak, bukan efek acak |

**[USUL BARU]** Tiga lapisan ambience (ruang/cuaca, mekanikal, gaib). Subtitle Indonesia dengan identitas pembicara: `[Raka]`, `[Rekaman Damar]`, `[EVP — Tidak Dikenal]`, `[Penonton_Terakhir]`. Audio cue Lastri memberi **informasi jarak yang konsisten** (rintihan makin dekat/keras, napas di tengkuk saat menempel), bukan efek acak; kurva suara invers atau non-linear diuji karena rolloff linear Nyonya (Atmaja) tidak cocok. **Telegraph harus bisa dibedakan tanpa audio**: flicker segmen, getar layar, embun layar, ikon HUD (§8.3).

### 7.4 Live stream dan teks chat (KANON + KLARIFIKASI)

Rentetan chat aslinya tetap berada di **bagian Naskah Adegan** (§1). Saat implementasi, gunakan *timeline* yang terhubung ke event (bukan menembakkan semua chat di `Start()`), dengan antrian UI yang bisa dibaca ulang bila adegan cepat.

| Blok | Pemicu event | Isi pokok | Catatan |
|---|---|---|---|
| Chat #1 + donasi Rp666.000 | `P2_START_LIVE` | Penonton **42.300**; `@dokter_jaga_malam`, `@kunti_bogor`; "Lastri tidak suka dipunggungi" | Dana terikat; tidak diproses ulang saat reload |
| Chat #2 + donasi Rp1.333.000 | `P2_LASTRI_SCARE1` | `@windah_bocil`, `@suster_keramas`; "cari laci pendingin tempat bayinya disimpan" | Dua petunjuk lingkungan juga ada di dunia; chat bukan satu-satunya sumber |
| Chat #3 + 310.500 | `P2_EPILOGUE_CHAT` | `@tretan_kw`, `@jurnal_risa_fan`; Rp164,8 juta (overlay) | Overlay naratif, bukan saldo (§5.3) |
| Donasi Rp6.666.666 + video 8 detik | `P2_DAMAR_VIDEO` | Layar merah tua mengunci kolom komentar | Dana terikat; satu kali |

**[KLARIFIKASI]** Donasi `Penonton_Terakhir` adalah kejadian cerita, bukan transaksi yang menambah saldo. Live tidak diwajibkan sebelum Poliklinik (anti-softlock), tetapi Chat #1 dan donasi memberi dua petunjuk lingkungan (Poli 03, bahaya punggung). **[USUL BARU]** Tambahkan *history panel* di HP agar clue yang lewat dapat dibaca kembali. Chat bisa memuat 1–2 komentar ambigu atau keliru (mis. penonton yang mengira Lastri bisa dilihat dengan kamera belakang) sementara bukti di lingkungan tetap akurat.

### 7.5 Kualitas presentasi yang disarankan untuk proyek Unity (USUL BARU)

Project menggunakan **Unity 6.3 LTS, URP, Windows x86_64**, dan perlu diuji pada laptop kelas **Intel Iris Xe/RAM 8GB**. Dokumen ini tidak mengklaim performa scene telah diuji. Prinsip produksi:

- **Aset wajib existing atau unduhan internet berlisensi.** Tidak membuat model, gambar, tekstur, atau gerak original sendiri; konversi/optimasi impor dan retargeting animasi existing diperbolehkan. Phase 2 memakai **pustaka lingkungan rumah sakit tersendiri** (modul dinding/lantai/pintu rumah sakit, laci pendingin, meja otopsi, brankar, inkubator, lampu operasi, UV), **bukan** furnitur atau permukaan Rusunawa. Sumber dan checksum dicatat, usulan: `Audit/phase2-environment-sources.json`, `Audit/phase2-asset-credits.md`, `Audit/phase2-audio-credits.md`. Sistem pemain/HP, karakter desainer, dan motor tetap digunakan.
- **Art pass:** blockout → *composition pass* → aset nyata → set dressing/clutter → decal (embun beku, darah, formalin) → lighting → VFX/audio → gameplay polish. Jangan menambah ruangan berkualitas rendah hanya untuk durasi.
- **Penerangan diegetik:** neon rumah sakit, bohlam darurat, cahaya bulan dari jendela tinggi, layar HP, Lampu Operasi, UV. Hindari ruangan terang tanpa sumber; frame horor gelap tetapi navigasi dan petunjuk tetap terbaca.
- **Biaya Selfie [KLARIFIKASI]:** Selfie adalah orientasi depan dari **feed HP existing**, yang sudah merupakan render tambahan terhadap kamera dunia dan kamera grip. Gunakan satu FeedCamera/RT bergantian depan-belakang; batasi resolusi/culling dan matikan feed saat layar mati. Hindari menambah render dunia ketiga permanen. Lastri dan ilusi hanya terlihat di kamera Selfie lewat layer khusus (**verifikasi bentrokan dengan layer AR 29**); jangan menduplikasi pipeline AR.
- **Biaya boss:** dua ilusi, pecahan kaca inkubator (pool, VFX saja), kabut, hujan, chat UI, lampu real-time. Batas *real-time shadow* selektif; matikan lampu tak terlihat; kabut memakai VFX transparan sederhana.
- **Telegraph harus murah:** audio 3D, flicker, dan siluet lebih penting dari partikel mahal. Gunakan pool dan batasi rigidbody aktif.
- **Testing performa:** profil build Windows nyata dengan lintasan lengkap, termasuk Selfie aktif di kamar mayat, tiga siklus boss, banyak chat UI, hujan, dan save/load. Validasi headless adalah tambahan untuk logika, bukan pengganti evaluasi visual/FPS di GPU sasaran.
- **Asal inspirasi:** DreadOut/Alan Wake/Outlast/Phasmophobia adalah referensi rasa mekanik, bukan izin menyalin aset, model, audio, level, UI, atau kode proprietary.

**[KLARIFIKASI — Golden Scene RSUD dan penerimaan visual lintas babak]** P2 harus memakai *environment library* rumah sakit yang berbeda dari kos/rusun. Jadikan **≥30 model prop rumah sakit yang berbeda** sebagai *baseline target kepadatan* (bukan 30 duplikasi objek), dihitung **di luar** arsitektur, tekstur, karakter, dan prop yang hanya muncul dalam UI. Ini mengikuti kualitas *dressing* yang diminta pada P1; perlu persetujuan/penyesuaian setelah melihat aset berlisensi yang benar-benar tersedia. Catat untuk setiap aset: sumber URL, pemegang lisensi/jenis lisensi, SHA-256, satuan/skala, material PBR, lokasi scene, koordinat serta penopang; **jangan membuat model/gambar/tekstur/animasi original** atau menggunakan gambar referensi DreadOut sebagai aset game.

**[BASELINE UJI — visual acceptance yang bisa difoto]** Untuk tiap area wajib ambil screenshot mata pemain normal dan feed Selfie saat relevan: IGD (brankar/triage/papan jadwal), Poli 03 (embun kaca dan papan 03), basement (meja otopsi, 12 laci yang terbaca, Pos Petugas, gudang D1), L2 (enam inkubator, Ruang Jaga), arena (LO/UV-N/UV-S dan dua generator). Penilai harus dapat mengidentifikasi fungsi ruangan dan jalur petunjuk **tanpa debug overlay**; collision pintu/objek dan palet material tidak boleh mengulang kesalahan ruang hampa ala greybox. Pantau draw call, VRAM, transparansi kabut, RT Selfie, dan bayangan lampu terpisah pada Windows/Iris Xe 8GB. Target global 60 FPS belum berarti profil lulus.

**[PERLU KEPUTUSAN]** FPS final, budget draw call, VRAM, batas ukuran build, dan kualitas material belum dinyatakan. §10.3 memberi *baseline* benchmark awal; validasi dengan build Windows nyata sebelum mengunci acceptance (P2-D11).

---

## 8. Anti-softlock, kesulitan, aksesibilitas, dan UX

### 8.0 Parameter prototipe terpusat (BASELINE UJI; bukan kanon baru)

Semua angka yang dapat di-*tune* didefinisikan **sekali** pada `Phase2BalanceConfig` (atau asset konfigurasi yang sudah dipakai proyek). Jangan menyalin konstanta berbeda di beberapa skrip. Naskah mengunci rintihan tiap 20 detik, hold Selfie 5 detik, jendela boss 5 detik, tiga siklus, suhu −16/−24/−20°C, dan kode `1603`/`2604`; **parameter HP bersama mengikuti §0.4**, bukan default tandingan. Config P2 menyimpan angka encounter; nilai HP shared dibaca dari `SmartphoneController`/definisi global.

| Parameter | Baseline | Status |
|---|---:|---|
| `phone.startBatteryPct` | Nilai save; **100% sekali secara opsional** lewat interaksi colokan Kos; keberangkatan tidak mengisi | BASELINE UJI (P2-D02) |
| `phone.normalDrainPerSecond` | **0,05** | GLOBAL |
| `phone.arOrUvDrainPerSecond` | **0,10 total**, bukan tambahan | GLOBAL |
| `phone.selfieDrainPerSecond` | **0,05** (profil kamera; dengan AR menjadi 0,10 total) | BASELINE UJI (P2-D07) |
| `phone.thermoExtraDrainPctPerSec` | **0** (overlay mengikuti profil mode aktif) | BASELINE UJI (P2-D07) |
| `phone.evpExtraDrainPctPerSec` | **0** | GLOBAL |
| `phone.photoCostPercent` / `blitzCostPercent` / `blitzCooldownSeconds` | **1% / 15% / 6s** | GLOBAL |
| `phone.largePowerbankRestore` / `smallPowerbankRestore` | **80 / 35 poin** | 80 GLOBAL; 35 existing kecil kos |
| `phone.dynamoFullRestorePct` / `dynamoFullDurationSec` | **+25% / 8s**, sekali per dinamo, persist ID | BASELINE UJI (dipakai ulang dari P1) |
| `phone.dynamoEmergencyCapPct` / `dynamoEmergencyDurationSec` | **<10% untuk mulai**, 45s akumulatif, hasil maks. 10% | BASELINE UJI |
| `phone.dynamoEmergencyCooldownSec` | **120s gameplay** per lokasi; tidak berlaku saat boss aktif | BASELINE UJI |
| `phone.zeroBatteryBehavior` | **Layar mati; tanpa `WATCH_ONLY`** | GLOBAL (P2-D12) |
| `player.effectiveWalkMps` | **1,7** (controller existing; tidak ada kecepatan mundur terpisah) | EXISTING |
| `thermo.ambientMorgueC` / `thermo.ambientHallC` | **12 / 24** | BASELINE UJI (24°C lorong IGD kanon, 12°C kamar mayat usulan) |
| `thermo.polyDwellSec` / `thermo.polyRangeM` | **1,5 s / 3,0 m** | BASELINE UJI |
| `thermo.drawerDwellSec` / `thermo.drawerConeDeg` / `thermo.drawerRangeM` | **1,0 s / 15° / 1,5 m** | BASELINE UJI |
| `thermo.lastriNearC` / `thermo.illusionC` / `thermo.lastriRangeM` | **−20 / −6 / 3,0 m investigasi; 8m boss target reticle** | KANON (−20); sisanya BASELINE UJI |
| `ar.auraPhotoDwellSec` | **1,0 s** | BASELINE UJI |
| `evp.rangeM` / `evp.recordSec` | **≤2,0 m / 6,0 s**; reset bila putus, tanpa penalti | BASELINE UJI |
| `formalin.telegraphSec` / `formalin.gasSec` / `formalin.radiusM` / `formalin.moveMult` | **1,5 / 6 / 2,0 / ×0,8** | BASELINE UJI |
| `formalin.reattachTelegraphSec` / `formalin.meterMult` | **1,5 / ×1,5** | BASELINE UJI |
| `lastri.learnWindowSec` | **30** (atau sampai lepas pertama) | BASELINE UJI |
| `lastri.holdRequiredSec` / `lastri.holdMinSpeedMps` | **5,0 (KANON) / 0,8** | KANON / BASELINE UJI |
| `lastri.inFrameBoundsPct` | **≥30%** projected rect + **≥3/9** LOS anchor, grace occlusion ≤0,25s | BASELINE UJI |
| `lastri.touchMeterRisePctPerSec` / `lastri.touchMeterFallPctPerSec` | **10 / 25** | BASELINE UJI |
| `lastri.hairTouchSec` | **2,0** | BASELINE UJI |
| `lastri.turnFatalDeg` / `lastri.turnFatalHoldSec` / `lastri.flinchMeterPct` | **50° / 0,25 s / +20%** | BASELINE UJI |
| `lastri.moanIntervalSec` / `lastri.moanTelegraphSec` | **20 (KANON) / 3,0** | KANON / BASELINE UJI |
| `lastri.reattachAfterClearedSec` | **45** (setelah `MORGUE_CLEARED`) | BASELINE UJI |
| `lastri.hideLoiterSec` | **6,0** | BASELINE UJI |
| `lastri.uiReadGraceSec` | **4,0** (tanpa GRAB saat Jurnal/dokumen baru dibuka) | BASELINE UJI |
| `lastri.l2.reattachIntervalSec` / `lastri.l2.moanTelegraphSec` / `lastri.l2.meterRisePctPerSec` | **12 / 2,5 / 12** | BASELINE UJI |
| `lastri.l2.darkSegmentMeterMult` / `lastri.l2.flickerSec` | **×1,5 / 2,0** | BASELINE UJI |
| `boss.prepSec` / `boss.interludeMinSec` | **30 / ≥20** | BASELINE UJI |
| `boss.exposedSec` / `boss.cycles` | **5 (KANON) / 3 (KANON)** | KANON |
| `boss.stalkMps` / `boss.stalkDistanceM` | **2,1 / 2,0–3,0** | BASELINE UJI |
| `boss.idleLungeSec` / `boss.lungeTelegraphSec` | **3,0 / 1,0** | BASELINE UJI |
| `boss.outOfFrameMeterPctPerSec` | **15** | BASELINE UJI |
| `boss.lampBurstSec` / `boss.lampCooldownSec` | **2,0 / 12** | BASELINE UJI |
| `boss.loBeamRadiusM` / `boss.uvBeamRadiusM` | **3,0 / 2,0** | BASELINE UJI |
| `boss.flareRadiusM` / `boss.flareDurationSec` / `boss.flareLureDwellSec` | **3,0 / 10 / 0,3** | BASELINE UJI |
| `boss.illusionCount` / `boss.illusionDistanceM` / `boss.illusionPopRangeM` / `boss.illusionRespawnSec` | **2 (KANON) / 2,0–5,0 / ≤1,5 / 6** | KANON / BASELINE UJI |
| `boss.flashMaxRangeM` / `boss.headshotAimToleranceDeg` | **8 / 20°** | BASELINE UJI |
| `boss.reshieldGraceSec` | **0,5** sebelum larangan menoleh aktif lagi | BASELINE UJI |
| `boss.generatorRestorePct` / `boss.generatorDurationSec` / `boss.generatorRadiusM` | **+25 poin / 8s / 1,6 m**, repeatable manual pada recovery/pasca-boss | BASELINE UJI |
| `sanity` | Nilai campaign dipertahankan | GLOBAL: Sanity gameplay mulai Babak 4 |
| `sanity.*` (drain/regen/radius) | **Tidak aktif di P2** | GLOBAL |

**Recovery anti-softlock [BASELINE UJI].** Pada baterai nol, layar mati dan AR/EVP/Selfie/Thermo/Blitz tidak berfungsi, tetapi dinamo masih dapat dioperasikan **tanpa HP**. Tempatkan **tiga dinamo eksplorasi**: (1) **IGD**, di balik meja resepsionis, sebelum pemain memerlukan Thermo; (2) **Pos Petugas** di basement (progres dipause, bukan di-reset, saat Lastri menempel); (3) **Ruang Jaga Suster** di L2 (zona tenang). Masing-masing memberi **satu kali** pengisian normal `+25%` selama `8s` dan menyimpan ID dinamo terpakai. Setelah itu, *emergency crank* di lokasi yang sama tersedia **bila baterai <10%**: **45s akumulatif** (progres hanya di RAM, dipause saat pemain melepas `E` atau dipaksa bersembunyi, tidak di-reset), **cooldown 120s waktu gameplay** antar pengisian sukses, hasil **maksimum 10%**, bukan +10% berulang sampai 100%. Bila baterai 3%, pemain boleh langsung memulai. **Dinamo eksplorasi tidak tersedia di arena boss**; arena memakai **generator G-NE dan G-SW** (titik daya darurat global) yang bukan dinamo eksplorasi. Selain itu, **colokan di meja PC Kos** bersifat opsional, mengisi penuh melalui E+receipt sekali pada persiapan P2; keberangkatan tidak mengisi (P2-D02).

**[KLARIFIKASI — baterai 0 dan Lastri].** Baterai 0 membuat layar mati; bagi aturan lepas-tempelan, **layar mati sama dengan `F`**. Karena itu pemain yang baterainya habis saat Lastri menempel **tidak terjebak**: masuk kolong meja/lemari besi lalu menunggu `LOITER` 6 s adalah jalur lepas yang sah, dan dinamo Pos Petugas dapat dioperasikan setelah itu (QA-61).

### 8.1 Mekanik baterai harus selalu bisa dipulihkan (USUL BARU)

Naskah belum menjelaskan kapasitas awal baterai, biaya Selfie/Thermo, atau perilaku baterai nol. HP dipakai untuk *semua* puzzle wajib (Thermo untuk Poli dan C3, AR untuk aura dan tembok, EVP untuk bukti ketiga), sehingga risikonya besar. **Kontrak aman:** tidak ada situasi persisten ketika pemain tidak bisa mengaktifkan alat wajib padahal tidak punya jalan mendapat energi. Tiga dinamo §8.0 + colokan Kos memberi suplai di jalur investigasi; arena boss memakai generator G-NE/G-SW. **Tidak ada `WATCH_ONLY` gratis** (pengecualian itu tidak pernah diratifikasi untuk P2). Powerbank Pasar Gaib opsional mengurangi tekanan tetapi tidak diwajibkan.

**Anggaran baterai (jalur utama, hitungan kasar).** Thermo/Selfie normal: 0,05/s. Poli+keypad ±3–4 menit ≈ 10–12%. Puzzle laci 2,5–3,5 menit dengan Selfie bergantian ≈ 8–12%, foto aura 1%. L2 (AR tembok, EVP, brankas) ≈ 10–15% termasuk 1% foto. Total sebelum boss ≈ 30–40% dari 100%. **Boss:** satu siklus ≈ 18–24% (§4.4), tiga siklus ≈ 54–72% **tanpa miss**; dua generator menyediakan +25 poin per operasi manual pada recovery, dapat diulang setelah miss; akses dari 0% harus dibuktikan QA-32/83. Angka ini harus dibuktikan oleh playtest, bukan dijanjikan.

**Flare dan Kemenyan.** Flare `[G]` **hanya berfungsi saat boss aktif**; di luar boss, `G` tidak melakukan apa pun dan flare tidak terkonsumsi (QA-42). Kemenyan Bakar tidak memiliki aksi dan tidak boleh dikonsumsi oleh sistem apa pun.

### 8.1A Damage, Sanity, uang dan upgrade (BASELINE UJI)

**Kondisi gagal Raka:** dua kematian saja — `POUNCE` (menoleh langsung saat Lastri menempel di luar jendela belajar dan cahaya ritual) dan `GRAB` (meter sentuhan penuh + 2 s). Pada boss: `GRAB` oleh `LUNGE` bila pemain diam >3 s tanpa bergerak, atau meter out-of-frame penuh. **Tidak ada health bar, tidak ada damage kaca, tidak ada proyektil, tidak ada sistem HP/Sanity paralel.** Jebakan formalin tidak pernah fatal.

**Sanity.** `Rancangan.MD` menetapkan Sanity gameplay mulai Babak 4. P2 mempertahankan nilai Sanity yang diwariskan; **tidak menguras, meregenerasi, atau mengubah radius/efek berdasar Sanity** (P1 §8.1A D02 tidak diadopsi). Kemenyan Bakar baru bermakna di Babak 4.

**Upgrade.** Selfie Mode dan ThermoKlenik.apk terbuka otomatis (`P2_UPGRADE_UNLOCK`, receipt satu kali), tidak berbayar. **Tidak ada toko atau upgrade di tengah babak** selain Pasar Gaib di Safe Hub. Saldo `wallet_legitimate` bukan `lifetime_donations`.

**Ledger:** lihat tabel §5.3. Selisih Rp77.934.334 adalah **selisih matematika, bukan donasi yang terbukti ada**; jangan menciptakan saldo/transaksi palsu untuk menyamakannya (P2-D03).

### 8.1B Bukti solvabilitas minimum dan percobaan baterai tepi (gabungan audit)

**Perhitungan minimum yang tidak bisa ditawar:** 3 Blitz sah × 15 poin = **45 poin baterai**, di luar 0,05 poin/detik Selfie, AR/EVP, miss, dan durasi interlude. *Emergency crank* eksplorasi hanya mengisi **hingga** 10%; karena itu **tidak dapat digunakan sebagai satu-satunya charger boss** (biaya tembakan 15%). Generator G-NE/G-SW pada fase PREP/INTERLUDE memberi `+25 poin / 8s` **dapat diulang dengan interaksi fisik selama recovery, tanpa isi otomatis**, tanpa membiarkan pemain mengeksploitasi isi ulang otomatis saat mati/reload. Angka tersebut baseline, bukan pengganti kode P1 yang harus diperiksa.

| Baterai awal CP5 | Harus tetap dapat dilakukan tanpa flare/powerbank | Titik rawan yang harus diuji |
|---:|---|---|
| 0% / 1% / 14% | Mengoperasikan G-NE/G-SW **tanpa HP**, memperoleh ≥15%, lalu menembak | `PREP` tidak boleh ditutup sebelum pemain punya kesempatan isi ulang; interaksi aman dari AI |
| 15% / 29% / 44% | Membagi daya antara Selfie dan Blitz dengan generator pada interlude | Tidak ada jendela 5s yang mengharuskan biaya Flash lebih dari sisa yang tersedia |
| 45% / 80% / 100% | Bisa menuntaskan tiga siklus tanpa powerbank wajib; drain tambahan terukur | Miss tidak menghancurkan sumber cahaya; sumber tidak habis sebelum tiga hit |
| Reload setelah 1–2 hit | Kembali **tepat** ke CP5 termasuk baterai, pusaka, flare, ketiga lampu | Tidak memakai `lastBattery` global yang telah dikurangi selama percobaan |
| Boss sudah kalah, baterai 0% | Pintu pulang dan panel motor tetap berfungsi tanpa smartphone | Status Arsip dari data save, bukan butuh membuka kamera |

**Formula acceptance:** pada fase tenang generator yang memenuhi biaya satu Blitz wajib dapat dicapai tanpa Lastri menerkam atau timer menutupnya secara sepihak. Dalam `EXPOSED` (5s), Lastri diam/tidak menyerang; akibatnya miss adalah kesalahan pemain, bukan karena model AI bergerak tidak stabil. Tiga keberhasilan harus mungkin dengan **LO + UV-N + UV-S** saja. Jika playtest menunjukkan tidak bisa, perbaiki penempatan generator/beam/timer dari telemetry; **jangan** menyiasati dengan reload otomatis +100%, Blitz gratis, atau membuat flare pembelian wajib.

---

### 8.2 Kesulitan dan aturan "fair horror" (USUL BARU)

- Setiap aturan fatal diajarkan lebih dulu lewat petunjuk lingkungan, audio, atau peringatan HP, lalu pemain diberi kesempatan mempraktikkan (jendela belajar §3.3A).
- Hanya dua jenis kegagalan fatal, `POUNCE` dan `GRAB` (§0.3 butir 4); di boss, idle-lunge dan meter out-of-frame adalah varian `GRAB`. **Dilarang** menambah aturan fatal ketiga, teleport acak Lastri ke belakang pemain, spawn tepat di atas pemain, atau re-attach saat pemain terkunci dalam dialog/Jurnal pertama kali tanpa telegraph (P2-D13).
- Lastri tidak boleh menguras sumber daya atau menyerang ketika pemain sedang terkunci dalam dialog/cutscene wajib; meter membeku pada cutscene wajib.
- Pada boss, Lastri, reticle kepala, sakelar, dan generator harus mudah ditemukan secara visual namun tetap menuntut timing. *Miss* tidak menghabiskan lampu; flare hanya terpakai begitu dijatuhkan dan dipulihkan oleh snapshot CP5.
- Segmen L2 yang gelap tetap menunjukkan tepi dinding, pintu, dan titik interaksi lewat cahaya bulan atau cahaya redup; **tidak ada area yang buta total** (§3.5).
- Jika pemain gagal berkali-kali, jangan otomatis membayarkan solusi; tawarkan hint progresif opsional. Jurnal menyimpan clue penting yang terlewat.
- Jangan membuat ruang puzzle yang punya collider/tempat sembunyi hanya pada satu arah sehingga pemain tak bisa keluar. Pintu basement dapat dibuka dari dalam; tembok bata dua arah.

### 8.3 Aksesibilitas (USUL BARU)

- *Rebindable controls*; sensitivitas kamera dan inverted Y. Prompt Selfie/Thermo dirender dari `GameInputConfig` (P2-D01), tidak ditulis `[V]` secara statis.
- Subtitle dan *chat history* untuk petunjuk yang disampaikan lewat suara/live chat; teks EVP dan suara Damar disubtitle.
- **Telegraph ganda:** setiap telegraph audio (rintihan, hembusan, desis formalin, gong flicker) punya padanan visual (getar layar, vignette, embun layar, ikon HUD). Kedap suara tidak boleh menjadikan telegraph tak terbaca.
- Opsi intensitas flashing, *camera shake*, chromatic aberration, motion blur; Blitz dan cahaya lampu operasi/UV memiliki opsi pengurangan flash. Jangan menaruh informasi wajib hanya pada efek kedipan.
- Pembedaan Lampu Operasi, UV-N, UV-S memakai **label dan posisi**, bukan warna saja; bacaan Thermo menampilkan **angka**, bukan hanya warna; reticle "PERISAI HANCUR" jelas pada kontras rendah.
- **[PERLU KEPUTUSAN]** Apakah mode lebih mudah boleh melonggarkan ambang 5,0 s hold, 5 s window boss, atau meter sentuhan. Nilai dasar naskah tetap dipertahankan; opsi aksesibilitas dibuat terpisah.
- Alternatif untuk *hold* 5 s: opsi "tahan" dapat dijadikan toggle untuk pemain yang tidak nyaman menahan gerak maju sambil menatap layar.

### 8.4 Transisi akhir dan item rahasia (KLARIFIKASI)

Urutan akhir **wajib tunggal**: `P2_BOSS_WIN` (Raka tetap di Ruang Operasi) → `P2_EPILOGUE_CHAT` → `P2_DAMAR_VIDEO` → `P2_EPILOGUE` (pesan `Penonton_Terakhir`, **tanpa fade**) → pemain bebas → **L2 → L1 → IGD → pelataran → motor Raka**. **Cabang opsional:** dari mana saja pemain boleh kembali ke **basement** (gudang administrasi) untuk Label Pasien. Lastri `RESOLVED` permanen sehingga seluruh rute aman. Kabut pintu Ruang Operasi tidak kembali.

**[KLARIFIKASI]** Hanya interaksi **`[E] Tinggalkan RSUD`** di motor di pelataran IGD yang membuka **panel sementara tanpa event save** dan menampilkan `Arsip Babak 2: 0/1 atau 1/1` dan status Arsip Saksi `N/5`, dengan pilihan `Kembali Menjelajah` atau `Lanjutkan`. Memilih lanjut memicu **satu-satunya event final `P2_COMPLETE`**, commit CP7, lalu `CKREK!` + hitam. Jika gagal menyimpan, jangan memutar `CKREK!`, jangan fade, jangan memindahkan pemain; tampilkan opsi coba lagi. Replay perekam Damar atau video tungku setelah epilog tidak memicu video berulang atau mengakhiri babak. Pemain boleh kembali mengambil Label Pasien sepanjang belum konfirmasi keluar. Status chapter select setelah keluar mengikuti kebijakan kampanye (P2-D06).

---

## 9. Produksi Unity: pembagian tanggung jawab tanpa overengineering (USUL BARU)

**Tujuan:** menerapkan desain di arsitektur proyek saat ini, **bukan** memerintahkan refactor besar-besaran. Nama komponen di bawah adalah contoh peran, bukan klaim sudah ada.

| Tanggung jawab | Kandidat modul/komponen | Perilaku utama |
|---|---|---|
| Stage, objective dan event | `Phase2FlowController` + data definisi level | Advance stage satu kali; stage diturunkan dari data, bukan flag terpisah |
| Interaksi dunia | `IInteractable` / sistem proyek yang sudah ada | Requirement, prompt, pickup, `E` dengan HP terangkat |
| Sensor suhu | `ThermoKlenikSensor` | Dwell, kerucut, jarak, pembacaan laci/poli, overlay di kamera belakang dan Selfie |
| Selfie Mode | `SelfieModeController` | Render kamera depan, layer Lastri/ilusi, Blitz Selfie; toggle dari `GameInputConfig` |
| Bukti | `Phase2Director` → `CampaignState.TryCommit` + provider Jurnal existing | Validasi AR/Thermo/EVP, pencatatan ID unik |
| Proyeksi AR | `ARRevealTarget` / layer AR yang sudah ada | Rantai C3, aura toples, tembok bata, gema suster, angka 17 |
| Live | provider `IPhoneChapterContent` / antrian chat existing | Trigger chat, history, donasi (state sekali, dana terikat) |
| Energi HP | `SmartphoneController` existing | Biaya, drain per mode, powerbank, dinamo, generator arena |
| Puzzle laci | `MorgueDrawerPuzzle`, `FormalinTrapController` | 12 laci, rantai C3, jebakan non-fatal yang dapat dibatalkan |
| Keypad/brankas | Padlock panjang dinamis existing | `1603` dan `2604`, umpan balik non-hukuman |
| Lastri (brain bersama) | `LastriAttachBrain` | Kamar mayat dan L2: attach, meter, hold, loiter, patroli lampu; parameter berbeda di config |
| Boss fight | `LastriRitualBossController` | Siklus, perisai, jendela, ilusi, validasi Blitz |
| Rig arena | `OperatingRoomLightRig`, `ArenaPowerPoint` | Lampu Operasi/UV, sakelar, generator, korslet |
| Audio | `SceneAudioProfile` existing + routing/sumber P2 | Kedap suara/telegraph adalah peran baru, bukan event bus kedua |
| Save | Adapter ke `CampaignState` existing | Persist flag idempoten tanpa save kedua |
| QA | EditMode/PlayMode tests dan scene diagnostics | Verifikasi gate, spawn, alur, reload |

### 9.1 Data-driven lebih baik daripada hardcode per adegan

**[USUL BARU]** Tempatkan teks dokumen/chat, item IDs, event IDs, suhu 12 laci, ambang waktu, biaya 15%, cooldown, dan prefab reference dalam konfigurasi/asset definisi yang bisa ditinjau bersama penulis. Teks 2001, donasi, dan cutscene tidak tersalin di banyak skrip. **Tabel suhu 12 laci** hidup di satu data (bukan tersebar per prefab). **Tetap gunakan definisi parameter global `Rancangan.MD`** untuk nilai shared lintas babak.

### 9.2 Pola implementasi bertahap

1. **Vertical slice L1:** Kos (PC, Pasar Gaib, colokan), IGD, live, Buku Jaga, Poli 03 + Thermo, keypad `1603`, persistensi dan load; buktikan modul Thermo dan transaksi belanja atomik.
2. **Kamar mayat:** kedap suara, Lastri menempel, Selfie, jendela belajar, hold/meter/loiter, 12 laci, formalin, Gunting, aura, Kunci Teralis; playtest navigasi penuh dan kematian/retry.
3. **L2:** patroli agresif, tembok bata AR (pola K102 P1), Inkubator 04 (EVP, Gelang, Damar), Ruang Jaga Suster (gema AR, `2604`, Botol), Jurnal Deduksi, kabut pintu OR.
4. **Boss:** meja gate, CP5 write barrier, tiga sumber cahaya, ilusi, jendela 5 s, generator, hit/miss/retry, CP6.
5. **Naratif/UI:** chat, VO, dokumen lengkap, pacing 82 menit, epilog, video tungku, ekonomi tersinkron.
6. **Golden quality pass:** art, lighting, ambience, animasi, optimasi GPU nyata, automation tests + hands-on QA.

**[KLARIFIKASI]** Urutan di atas adalah prioritas engineering, **bukan mengubah urutan cerita** 1–6 atau meminta pindah ke Babak 3 sebelum Babak 2 playable dan polished.

---

## 10. QA: matriks uji, kasus tepi, dan kriteria selesai

### 10.1 QA wajib (KANON yang dijabarkan)

**[MERGED] QA-01–QA-76 asli Claude dipertahankan dan ditambah QA-77–QA-100 pada §10.2D; seluruh 100 QA adalah spesifikasi yang belum diuji pada P2. Kasus bertanda P2-Dxx mengikuti keputusan pada §11.2. Mempertahankan ID QA bukan berarti semua proposal sudah disetujui.**

| ID | Skenario | Hasil yang diharapkan |
|---|---|---|
| QA-01 | Main tanpa belanja dan tanpa saweran | Jalur sampai boss dan kemenangan tetap selesai |
| QA-02 | Lewati semua chat petunjuk | `1603` dan `2604` tetap dapat dideduksi dari lingkungan/dokumen |
| QA-03 | Pindai Poli 03 tiga kali; foto aura tiga kali | Bukti 1 tetap 1, bukti 2 tetap 1 |
| QA-04 | Rekam EVP Inkubator 04, ambil perekam Damar secara terpisah lalu load | Bukti EVP dan arsip Dokumen #3 independen, persisten, tanpa grant silang/duplikasi |
| QA-05 | Foto aura tanpa ambil Gunting; ambil Gunting tanpa foto aura | Dua flag terpisah; masing-masing tidak mengurangi yang lain |
| QA-06 | Buka Jurnal dengan 0, 2, 3 bukti (termasuk duplikat) | Tombol konfirmasi aktif **hanya** pada tiga ID tepat; scan ketiga tidak otomatis mengonfirmasi |
| QA-07 | Tiga bukti + konfirmasi + tiga pusaka + `E` di meja | Boss dimulai tepat sekali setelah CP5 sukses; tanpa konfirmasi Jurnal ditolak |
| QA-08 | Blitz saat perisai Kabut Sengkala utuh | Tidak memberi poin segel |
| QA-09 | Cahaya tepat, Blitz miss/timeout dalam 5 detik | Perisai pulih, lampu tidak rusak; kesempatan dapat diulang |
| QA-10 | Blitz berkali-kali dalam satu window/siklus | Maksimal 1 poin per siklus |
| QA-11 | Bawa tiga pusaka tetapi bukti/Jurnal kurang; masuk OR, periksa meja, keluar | Pintu dapat diakses tetapi ritual ditolak; tanpa tiga pusaka kabut pintu tetap ada |
| QA-12 | Ambil `SECRET_P2_LABEL_PASIEN` dua kali | `secret_items` hanya memuat satu ID; Jurnal tetap 1/1 |
| QA-13 | Mati/reload sesudah ambil label | Label tetap dimiliki; Arsip global berdasar jumlah ID |
| QA-14 | Selesai boss tanpa label | Pemain bisa kembali ke basement sebelum konfirmasi motor; status tetap 0/1 |
| QA-15 | `P2_COMPLETE` | Segel kedua, upgrade, inventori, uang, Sanity, rahasia konsisten setelah reload |

### 10.2 Edge case tambahan (USUL BARU)

| ID | Uji | Hasil |
|---|---|---|
| QA-16 | Tarik laci salah tanpa memindai; tutup dengan `E` dalam 1,5 s; ulangi tanpa menutup; coba B2 | Cancel membatalkan jebakan; timeout memberi gas 6 s non-fatal (×0,8, tanpa damage/Game Over/pengurangan baterai); B2 tetap terjebak |
| QA-17 | Baterai sangat rendah sebelum Poli/C3/EVP/AR | Ada recovery (dinamo IGD, Pos Petugas, Ruang Jaga Suster, colokan Kos) sehingga bisa lanjut |
| QA-18 | Belanja: beli powerbank ×2, spam beli, reload di tengah transaksi | Dua instance ID berbeda; saldo terpotong sekali; tidak ada barang ganda |
| QA-19 | Spam kode `1603` benar dan interaksi pintu | Pintu terbuka sekali, CP2 sekali, tanpa event ganda |
| QA-20 | Attach pertama: menoleh dan biarkan meter penuh selama jendela belajar; ulang setelah lepas pertama atau 30s | POUNCE dan GRAB sama-sama nonfatal selama belajar; batas mana-yang-lebih-dulu jelas; sesudahnya telegraph penuh |
| QA-21 | Hold 5 s: berhenti, keluar frame, lalu lanjut | `holdTimer` dipause, tidak reset; Lastri memudar setelah akumulasi 5 s maju in-frame |
| QA-22 | Bersembunyi + `F`; nyalakan layar saat `LOITER` | `LOITER` 6 s; layar menyala mereset loiter tanpa fatal; keluar selalu tersedia |
| QA-23 | Selfie terangkat lalu `E` pada kunci, laci, sakelar, brankas | Interaksi bekerja tanpa menurunkan HP |
| QA-24 | Buka/tutup Jurnal berulang saat attached; coba dokumen/keypad modal | Nonmodal world/meter tetap jalan, grace 4s sekali per attach tidak dapat di-reset dengan spam; hard modal baru ditolak saat attached dengan hint, bukan mengunci pemain lalu membunuh |
| QA-25 | Mati di kamar mayat sebelum dan setelah checkpoint bukti/pusaka | Muat snapshot aman terakhir CP3/CP4; DORMANT, jebakan aktif sebelum boss; item/bukti/secret tidak hilang atau duplikat |
| QA-26 | FPS saat boss (ilusi + generator + hujan + chat + Selfie render) | Profil frame time dan memori; tidak mengandalkan headless |
| QA-27 | Rahasia P1 ada/tidak; ulangi setelah ambil Label | Arsip menghitung irisan set unik dengan lima ID kanon; 2/5 atau 1/5, ID asing ditolak/tidak dihitung |
| QA-28 | Aksi epilog/motor ditekan berulang | Tidak menambah donasi, video, segel, atau transisi dua kali |
| QA-29 | Pause/settings saat window 5 s atau interlude | Timer mengikuti definisi pause secara konsisten |
| QA-30 | Video Damar dan perekam setelah reload | Tidak menggandakan reward; Dokumen #3 tetap bisa diputar ulang |

### 10.2A QA tambahan reviewer (WAJIB untuk freeze)

| ID | Skenario | Hasil wajib |
|---|---|---|
| QA-31 | Baterai 0 sebelum Poli/EVP | Dinamo terjangkau tanpa HP; emergency crank `<10%` dan progres dapat dilanjutkan setelah interupsi |
| QA-32 | Battery 0 di PREP, TRACK, setelah miss berulang dan setelah CP6; tanpa flare/powerbank | POWER_RECOVERY menghentikan fatal; generator manual +25/8 repeatable dan pasca-boss, tanpa cap10 arena; pemain melanjutkan sadar dengan biaya Blitz 15/6 tetap |
| QA-33 | Mati boss lalu retry 20 kali | CP5: baterai snapshot, tiga sumber utuh, flare pulih, tiga pusaka belum terpasang, uang/item tak berubah |
| QA-34 | Sensor Poli: lintas sekilas vs stabil ≥1,5 s dalam ≤3 m | Hanya pembacaan stabil menghitung; satu ID |
| QA-35 | Laci C3: AR saja, Thermo saja, keduanya | Rantai hanya terlihat di AR; hanya Thermo −24°C ≥1,0 s mematahkannya; AR saja tidak membuka |
| QA-36 | Pindai laci sambil berjalan; kerucut >15°; jarak >1,5 m | Dwell terpenuhi menghitung; di luar kerucut/jarak tidak |
| QA-37 | Masuk/keluar basement, reload dan ganti area berulang | Hanya sumber Exterior diredam; telegraph tetap terbaca dan gain pulih tepat sekali; nama bus §7 konseptual, bukan asumsi grup mixer telah ada |
| QA-38 | Keypad `1603` tanpa membaca chat | Derivasi dari Buku Jaga + papan poli + sensor; angka tidak tertulis sebagai teks lingkungan |
| QA-39 | Brankas AR vs Catatan Suster (Lastri 26) + papan 04; forum/Inkubator dilewati; battery0 setelah kode diketahui | 2604 dapat dideduksi di Ruang Jaga saja; gema adalah reveal kontekstual sah; buka tanpa AR setelah tahu kode; salah kode nonhukuman |
| QA-40 | Ambil Kunci Teralis bersama laporan; naik ke L2 tanpa C3; save/load | Teralis terbuka; kunci tidak hilang; stage tidak mundur |
| QA-41 | Save Sanity 0 di P2 | Sanity diwariskan; tidak ada drain/regen/radius |
| QA-42 | Kemenyan; `G` di luar boss | Kemenyan tidak dikonsumsi/tidak berfungsi; `G` di luar boss tidak terkonsumsi |
| QA-43 | Overlay Rp164,8 juta vs saldo; donasi terikat; belanja | Dua nilai tidak dicampur; donasi terikat tidak masuk `wallet_legitimate`; selisih Rp77.934.334 ditandai terbuka |
| QA-44 | Migrasi save P0/P1 valid; file duplikat/korup/versi asing; masuk tanpa prasyarat | 1→2→3 mempertahankan semua data valid; data korup ditolak dengan opsi backup, bukan fabrikasi ID; entry butuh receipt P1_COMPLETE dan segel |

### 10.2B QA tambahan v2.1 — dipertahankan untuk validasi

| ID | Skenario | Hasil wajib |
|---|---|---|
| QA-45 | Hold maju 5s dengan kecepatan kapsul aktual ≥0,8m/s, akselerasi/crouch/collision dan loop XZ | Minimum integral 4m; 8,5m pada 1,7m/s konstan hanya model. Tidak ada putaran wajib yang memicu POUNCE; jalur diuji pada collider nyata |
| QA-46 | Attach normal setelah jendela belajar, tanpa Selfie, tanpa formalin/segmen gelap atau UI grace | Telegraph 3s + meter kosong 10s + HAIR_TOUCH 2s: 12s setelah attach, 15s sejak rintihan. Learning dan modifier diuji terpisah, bukan memakai oracle 15s untuk semua state |
| QA-47 | Tembok bata Inkubator: lintasi, matikan AR, baterai 0% | Bukaan dan collider tetap terbuka **dua arah**; tidak terjebak |
| QA-48 | Yaw kontrol melintasi cone 50° cepat dan ≥0,25s; ulang OTS/FP/crouch | POUNCE memakai posisi world Lastri dan yaw kontrol, bukan avatar/rotasi feed; flinch +20 hanya sekali per crossing, bukan per frame; grace berlaku |
| QA-49 | Selfie: sebagian bounds tertutup, objek kecil sesaat, lalu dinding solid >0,25s | ≥30% projected rect di layar dan ≥3/9 LOS anchors; grace≤0,25s hanya untuk occlusion singkat; solid wall tidak memberi watch gratis |
| QA-50 | Setelah `MORGUE_CLEARED` | Interval re-attach menjadi 45 s |
| QA-51 | Patroli L2: segmen padam, zona tenang | Flicker 2 s; gelap ×1,5; Ruang Inkubator/Jaga Suster tanpa re-attach baru; navigasi tetap terbaca |
| QA-52 | Toggle C, rekam 6s pada ≤2m dan LOS; C lagi/layar mati/jarak putus; inkubator salah | Reuse recorder P1 dengan durasi P2; putus mereset tanpa penalti, hanya 04 memberi bukti dan suara perempuan |
| QA-53 | Boss: 1–2 Blitz sukses lalu force quit dan load | CP5 utuh: tiga pusaka di inventori, bukti + konfirmasi ada, lampu belum terpakai |
| QA-54 | `P2_BOSS_WIN` dengan simulasi disk-write gagal | CP6 atomik; tidak ada segel tanpa pusaka-di-meja atau sebaliknya; UI retry commit |
| QA-55 | Pilih tiap ilusi/asli dengan reticle Thermo Selfie, cahaya palsu dan jarak dekat | Target terpilih+LOS+range boss8m: asli−20, palsu−6; bukan suhu terdekat yang selalu asli. Ilusi pecah nonfatal, respawn6s, lampu cooldown12s utuh |
| QA-56 | Blitz Selfie vs Blitz belakang | Valid hanya saat `EXPOSED`; kamera belakang menghadap depan **tidak** mengenai Lastri di belakang; setelah balik badan valid |
| QA-57 | Profil playtest target 82 menit di build nyata | Waktu per beat dicatat; bukan filler/timer paksa |
| QA-58 | Mati setelah ambil Gunting, sebelum foto aura | CP4 menyimpan Gunting; aura masih dapat diambil; tidak duplikat |

### 10.2C QA akhir — acceptance sebelum implementasi penuh

| ID | Skenario | Hasil wajib |
|---|---|---|
| QA-59 | Masuk HUB battery0/1/19/85, pilih colokan atau tolak, lalu berangkat/reload | E colokan+receipt mengisi sekali; menolak mempertahankan baterai aktual; berangkat/Continue/CP tidak refill |
| QA-60 | Mati/Continue sepuluh kali dari CP3 | Baterai selalu nilai snapshot; tidak naik tiap ulang |
| QA-61 | Baterai habis saat Lastri menempel di kamar mayat | Layar mati, tanpa `WATCH_ONLY`; sembunyi + loiter melepas; dinamo Pos Petugas dapat dipakai setelahnya; tidak ada deadlock |
| QA-62 | Setelah boss, kembali ke basement/L1 dengan baterai 0% | Lastri `RESOLVED`, tidak respawn; layar tetap mati; semua rute aman |
| QA-63 | Selfie terangkat, E pada kunci/Gunting/Gelang/sakelar; coba dari balik tembok | Resolver tangan berjangkauan dan LOS bekerja tanpa menoleh/menurunkan HP; E tidak membaca arah belakang feed sebagai tangan di belakang |
| QA-64 | E pickup ketika HAIR_TOUCH sebelum dan tepat pada deadline GRAB | Pickup sah sebelum deadline tidak menghapus timer; pada deadline fatal diproses lebih dulu. Animasi pickup opsional tidak memberi invulnerability |
| QA-65 | `E` di meja operasi dari jauh vs dalam radius | CP5 **tidak** tercipta dari jauh; tercipta sekali saat `E` valid di meja |
| QA-66 | Dua pusaka + tiga bukti di pintu OR; lalu tiga pusaka; lalu setelah CP6 | Kabut tetap bila kurang; lenyap saat tiga pusaka dibawa; setelah CP6 pintu tetap terbuka walau pusaka di meja |
| QA-67 | Tiga menit di L2: lorong, Inkubator, Jaga Suster | Navigasi dan clue nyata; tidak ada gate/timer artifisial |
| QA-68 | Pulang L2→L1→IGD→motor setelah boss | Tangga/teralis tidak menghalangi; basement tetap dapat diakses; motor titik exit satu-satunya |
| QA-69 | Panel motor → Kembali Menjelajah → save/reload | Tidak ada `P2_LEAVE_RSUD`; stage tetap `RETURN_TO_EXIT`; dapat ke basement |
| QA-70 | Tekan Lanjutkan berkali-kali atau simulasi tulis CP7 gagal | Hanya satu `P2_COMPLETE`; gagal: tanpa `CKREK!`/fade; coba ulang dapat |
| QA-71 | Setelah `P2_CROSS_INKUBATOR_WALL`, baterai 0% di L2/L1 | Bukaan fisik dua arah tetap terbuka tanpa AR |
| QA-72 | Dinamo Pos Petugas: 3%, mulai crank, Lastri menempel setelah 30 s, kembali | Progres 30/45 s dipause, tidak reset; tidak berjalan saat menempel |
| QA-73 | Emergency crank 9%, berulang setelah cooldown | Tidak pernah >10%; 120 s gameplay sebelum dipakai ulang; full charge sekali per dinamo |
| QA-74 | Daftar 17/16, D1 17°C AR17, D2-D4 12°C dan kunci; coba buka D1 dengan kunci tanpa scan | Clue tetap tersedia; kunci membuka D1 tanpa gate scan tersembunyi; sensor membantu deduksi, secret tetap opsional dan commit-sebelum-sukses |
| QA-75 | Jejak Damar di terowongan servis | Non-gating, tanpa reward; tidak dihitung bukti/rahasia/dokumen |
| QA-76 | Jalur dev tanpa `P1_COMPLETE` | Ditandai **dev-only**; build produksi menolak; tidak ada segel/uang/inventori palsu di save produksi |

### 10.2D Pengujian silang tambahan hasil merger (QA-77–QA-100; WAJIB sebelum phase freeze)

Kasus di bawah menutup cakupan yang relatif kurang eksplisit di matriks QA Claude: kontrol fisik RMB, scene Kos Golden, *fault injection* lintas kampanye, batas energi numerik, desinkronisasi save, dan mutu aset. **Status awal semua `NOT_RUN`**; nomor QA-01–QA-76 dipertahankan agar laporan lama tidak putus.

| ID | Kategori | Skenario / langkah uji | Hasil wajib |
|---|---|---|---|
| QA-77 | Kontinuitas Kos | Buka P2 dari CP7 P1 dan inspeksi scene Kos | Scene Golden yang sama digunakan, tidak di-*resize* ke 4×3 dan tidak menduplikasi furniture/PC |
| QA-78 | Audit r1 | OTS/FP tahan/lepas RMB dari app dan toggle X/T/Q; F/Tab selama hold lalu fokus hilang | Pose horizontal existing dipertahankan; release restore app/raised state, lensa/filter pilihan eksplisit tetap; F/Tab menang dan RMB perlu release/repress sebelum raise lagi |
| QA-79 | Remapping | Tukar binding X/V lewat konfigurasi input yang diizinkan | Selfie/perspektif tidak berebut aksi dan petunjuk di UI selalu sesuai binding aktual |
| QA-80 | Audit r1 | Alt-tab saat attach, dokumen, dan EXPOSED; buka pause/settings | Satu kebijakan GameShell pause gameplay clock/AI/drain; resume manual tanpa delta catch-up; beda dengan nonmodal Jurnal; belum fitur terverifikasi |
| QA-81 | Audit r1 | Normal→Selfie→Thermo→AR/EVP, layar mati dan hard modal/pause | Satu profil drain global tanpa stacking; nonmodal tidak diam-diam membekukan drain; hard pause konsisten untuk AI/drain/timer; angka tetap nonnegatif |
| QA-82 | Snapshot energi | Memulai P2 dengan battery P1=0,1,19,85 lalu menolak colokan Kos | Tidak ada grant otomatis pada transisi P1→P2; colokan fisik dapat dioperasikan; retry/Continue tetap snapshot |
| QA-83 | Audit r1 | CP5 battery0/1/14/15/29/44/45/80/100; generator sudah dipakai; miss hingga habis | Recovery manual repeatable tetap bisa mencapai ≥15 untuk tiap Blitz; min20/30 bukan timer paksa; tidak memberi grant retry atau flare wajib |
| QA-84 | Interaksi charger | Lastri menempel ketika mencoba dinamo basement setelah 30/45s crank | Progres RAM ter-pause dan dapat dilanjutkan; tidak berjalan diam-diam saat player bersembunyi |
| QA-85 | Audit r1 | Spam Jurnal/inventory/keypad/E/F/X/RMB saat attached | Satu pemilik input; nonmodal F/X dan exit tetap berfungsi, hard modal baru ditolak saat attached; grace tidak dapat diperbarui spam, tidak ada pickup+Blitz satu input |
| QA-86 | Audit r1 | Selfie dekat dinding/tepi meja, semua perspektif; rotasi cepat dan quiet zone pada battery0 | Render/LOS setuju; tidak tembus solid; cooldown attach/grace occlusion berbatas; quiet zone detach tanpa syarat layar |
| QA-87 | Audit r1 | Load schema1/2 valid, schema3 hasil migrasi, future schema dan .bak | P0/P1/secret/ledger utuh; nested P2 tervalidasi, future/korup ditolak tanpa reset; satu CampaignState/save |
| QA-88 | Idempotensi event | Trigger sensor suhu/EVP/AR/foto dua kali termasuk lewat Continue | Exactly-once receipt dan evidence set; hitungan Jurnal 3/3 bukan bertambah 4/3 |
| QA-89 | Audit r1 | Force quit sebelum/saat/setelah shop dan charge HUB, lalu Continue | Debit+spent+instance+receipt atomik; route kembali HUB, tidak reload Prolog; paket sekali/campaign, charge opsional sekali tanpa duplikasi |
| QA-90 | Latar waktu | Masuk IGD dari scene Kos 23:45→23:50 dan buka live lagi setelah reload | Jam cerita dan penonton awal konsisten; event donasi terikat tak muncul sebagai saldo sah |
| QA-91 | Audit r1 | Periksa seluruh naskah/clue/kanon dan dokumen sumber | Kode1603/2604, suhu−16/−24/−20,17 korban,Lastri26 konsisten; Dr.Hendra tidak otomatis dokter kepala; E1-E18 dipertahankan sebagai riwayat |
| QA-92 | Boss beam geometry | Tiga lampu LO/UV-N/UV-S diperiksa dari posisi sakelar dan semua arah lari | Beam sungguh dapat mengenai boss asli tanpa clipping/jalan buntu; sumber habis **hanya setelah hit sukses** |
| QA-93 | Audit r1 | Saat TRACK bidik ilusi, lalu EXPOSED bidik asli; klik cepat pada satu window | Ilusi lenyap saat EXPOSED, jadi tidak diuji sebagai target yang mustahil pada window sama; miss Blitz berbiaya15/6, palsu tidak memberi segel, satu sukses/siklus |
| QA-94 | Komit kegagalan boss | Matikan aplikasi saat 1–2 hit dan setelah `P2_BOSS_WIN` sebelum/selama tulis CP6 | Dari disk hanya CP5 utuh atau CP6 utuh; tidak pernah boss kalah tanpa segel atau pusaka hilang |
| QA-95 | Audit r1 | Dari CP6 battery0 selesaikan epilog dengan generator fisik; lalu ke motor dan D1 | Epilog tidak softlock: daya tersedia, tidak auto-charge. Panel motor baru setelah EPILOGUE; pulang/arsip tanpa HP, Lastri dan jebakan resolved |
| QA-96 | Panel motor | Pilih `Kembali Menjelajah` 3×, lalu `Lanjutkan` dan fault injection CP7 | Panel sendiri tidak dipersist; hanya `P2_COMPLETE` final yang tercatat, fade hanya setelah commit |
| QA-97 | Arsip lintas babak | Kombinasi P1 secret ada/tidak ada, P2 secret ada/tidak ada | Hitungan N/5 sesuai unique ID, panel lokal 0/1 atau 1/1; tidak mengarang 2/5 |
| QA-98 | Audit r1 | Audit aset scene RSUD saat kelak tersedia | Lisensi/URL/hash/penempatan nyata; ≥30 prop unik baseline produksi; audit sekarang menandai paket RSUD/Lastri/acting belum tersedia, bukan lulus |
| QA-99 | Audit r1 | Profil Windows GPU sasaran dengan world+feed Selfie+grip dan transparansi boss | FPS/1%low/memori/visual/audio dicatat; target global60 tetap, benchmark30 bukan ratifikasi rilis; Linux/headless bukan penerimaan |
| QA-100 | Audit r1 | Dua run HUB→CP7: tanpa live manual/belanja/secret dan dengan arsip lengkap | Keduanya selesai tanpa gate berbayar. Epilog memakai sequence siaran lokal mandiri walau live IGD dilewati; uji82menit, tanpa timer paksa |

### 10.3 Definition of Done (gerbang rilis Babak 2)

**Gameplay:** jalur 82 menit secara desain dapat dimainkan dari Kos sampai transisi; tiga bukti dan tiga pusaka berfungsi; Lastri dapat dihindari secara adil di kamar mayat dan L2; puzzle laci dapat dipecahkan lewat deduksi; tiga siklus boss dapat diselesaikan tanpa softlock dan tanpa flare. **Konten:** enam adegan, empat dokumen bernomor (+ artikel bonus), dialog utama, tiga blok chat, perekam Damar, video tungku, dan Label Pasien ada serta terbaca. **Data:** save/load item unik, segel, baterai, uang, Sanity konsisten; arsip tetap persisten; `P2_COMPLETE` satu-satunya event final. **Atmosfer:** suara, pencahayaan bersumber, ruang terbaca, aset nyata sesuai setting, tidak ada primitive placeholder pada jalur wajib, fixture nyata untuk Lampu Operasi/UV/generator, telegraph terdengar dan terlihat. **Target global 60 FPS tetap berlaku.** Benchmark **diagnostik usulan**, bukan penurunan gate rilis: Windows i5-1155G7/Iris Xe/8GB, 1280×720 Low ≥30 FPS dan 1% low ≥24 FPS, tiga run lintasan plus boss. Ratifikasi preset/hardware rilis sesudah profil nyata; headless bukan pengganti (P2-D11). **QA:** lulus **100 skenario QA-01–QA-100** dan uji manual build Windows pada hardware sasaran; bug kritis navigasi, crash, dan penyimpanan = nol sebelum *freeze*.

### 10.4 Instrumentasi pengembangan yang membantu (USUL BARU)

Debug overlay khusus developer: `stage`, `completedEvents`, `evidence=0..3`, `relics=0..3`, `lastriState`, `touchMeter`, `holdTimer`, `drawerTemp[12]`, `activeSource`, `bossCycle`, `exposedTimer`, `phoneBattery`, `flareCount`, `secretItemsCount`. Tombol dev reset encounter **hanya di build development** (Lastri kamar mayat, L2, boss) agar tester mengulang tanpa memanipulasi save produksi. Catat berapa lama pemain mencari clue per beat untuk membandingkan target 82 menit dengan playtest; jangan menahan pemain di kamar mayat hanya untuk mengejar angka durasi.

---

## 11. Kontinuitas naratif dan keputusan yang masih terbuka

### 11.1 Perbedaan sumber yang sudah dijembatani (KLARIFIKASI)

| Area | Potensi benturan | Penafsiran aman untuk implementasi |
|---|---|---|
| Selfie `[V]` vs kontrak `V`/`X` | Naskah sumber menulis V untuk Selfie; kontrak P1 menetapkan V = perspektif, X = Selfie | **Teks instruksi bermain di §1 sudah memakai `[X]`**; dokumen sumber `[V]` tetap tercatat di E1; prompt final dari `GameInputConfig` (P2-D01) |
| Kode `1603` vs lokasi kematian Lastri | Log asli menyebut "suhu ruang tempat pasien meninggal", padahal Lastri meninggal di L2 | Revisi: suhu poli tempat ia pertama diperiksa + nomor poli yang sama (E2) |
| "Di atas −20°C" | Ambigu secara numerik | "Lebih hangat dari −20°C"; hanya C3 (−24°C) yang lebih dingin (E3) |
| Bukti 1 "−16 s/d −24°C" | Dua angka pada satu bukti | Bukti 1 = −16°C; −24°C adalah petunjuk puzzle (E14) |
| Donasi "meninggalkan sesuatu di Kamar Mayat" | Rekaman Damar berada di Inkubator 04 (L2) | Jejak Damar non-gating di terowongan servis; alternatif ubah teks (E7, P2-D05) |
| Kaca inkubator di Ruang Operasi | Inkubator ada di L2 Ruang Inkubator | Empat inkubator rusak sebagai prop wajib di Ruang Operasi (E11) |
| Tombol Selfie vs kamera belakang | Blitz "dari belakang" tidak mungkin bila Raka menghadap depan | Dua jalur valid: Blitz Selfie ritual dan Blitz belakang setelah balik badan (§4.4) |
| Penonton 142.000 → 42.300 | Tampak turun | Penonton serentak saat siaran baru dibuka, bukan puncak kumulatif (E8, P2-D09) |
| Akhir babak | `CKREK!` langsung setelah pesan vs rahasia D1 di basement | Epilog tanpa fade; `CKREK!` setelah konfirmasi di motor (E10, P2-D06) |
| Brankas "terbuka lewat mode AR" | Gate/petunjuk belum terdefinisi | Gema masa lalu + kode `2604` + jalur deduktif cadangan (E17) |
| Uang | Milestone Rp164,8 juta vs saldo | Bedakan saldo, dana terikat, dan overlay naratif; selisih Rp77.934.334 terbuka (P2-D03) |
| Arsip Saksi | `2/5` di naskah bergantung P1 | `N/5` dari ID unik aktual; `0/1`–`1/1` untuk babak ini (E6) |

### 11.2 Status keputusan desain dan teknis setelah audit

Audit ini memilih alternatif teknis anti-softlock atas izin revisi dokumen. Itu **tidak meratifikasi tambahan lore**. Ketentuan berikut membedakan kontrak established, baseline teknis dan keputusan kreatif yang masih terbuka.

| ID | Status | Ketentuan / keputusan yang masih terbuka |
|---|---|---|
| P2-D01 | TERKONFIRMASI | Repo X=Selfie, V=perspektif; Rancangan lama V=Selfie adalah utang sinkronisasi global, tidak menghapus OTS |
| P2-D02 | BASELINE DIREVISI | Carry-over aktual; colokan opsional E+receipt sekali; keberangkatan/Continue/retry tidak refill. Review desainer tetap diperlukan sebelum freeze |
| P2-D03 | GLOBAL + TERBUKA | Wallet existing authoritative; overlay Rp164,8 juta naratif, selisih bukan grant. Ledger cerita lengkap masih terbuka |
| P2-D04 | SEBAGIAN TERKONFIRMASI | Lima gerbang/empat segel ditetapkan global. **Dr. Hendra = dokter kepala tidak terbukti**: jangan digabung; artikel tetap dokter kepala, laporan tetap Dr. Hendra sampai desainer menetapkan hubungan |
| P2-D05 | USUL KREATIF | Jejak Damar non-gating untuk menyambung kalimat donasi tetap usul E7; jangan mengklaim sudah disahkan atau sudah ada asetnya |
| P2-D06 | BASELINE | Motor/CP7, arsip opsional, shutter setelah commit; chapter select backlog. Tidak membuat P3 |
| P2-D07 | GLOBAL | Selfie 0,05/s; AR/UV 0,10 total; Thermo/EVP overlay tanpa stacking mengikuti Rancangan, bukan ekonomi baru |
| P2-D08 | REUSE KONTRAK P1 | Retry snapshot CP5; write barrier existing; konsumsi flare/powerbank RAM masuk CP6; titik daya repeatable saat recovery mencegah deadlock |
| P2-D09 | INTERPRETASI NARATIF | 42.300 penonton serentak pada live baru, 310.500 akhir; tidak mengubah angka naskah |
| P2-D10 | KEPUTUSAN TEKNIS | Rencana schema 3 dalam CampaignState; sekarang loader schema 2. Implementer boleh menjalankan migrasi teruji setelah izin coding, bukan meminta persetujuan ulang atas penomoran teknis |
| P2-D11 | TERBUKA UNTUK FREEZE | Global 60 FPS tetap target. 720p/30/1%24 Iris Xe hanya benchmark diagnostik, **bukan otomatis target rilis yang lebih rendah**; perlu profil GPU nyata |
| P2-D12 | TERKONFIRMASI BATAS | Gunakan kontrak global yang eksplisit, tanpa menganggap semua keputusan P1 atau grant Prolog berlaku otomatis P2 |
| P2-D13 | BASELINE | Dua jenis fatal; nilai meter/geometry/grace harus dibuktikan. Audit menutup loophole/determinisme §15.1 tanpa mengubah identitas Lastri |
| P2-D14 | TERKONFIRMASI | Label MULAI LIVE sesuai AtmajaPhoneView existing; live manual opsional, epilog tetap tersaji lewat sequence lokal |
| P2-D15 | TERKONFIRMASI DESAINER | Kos sengaja diperbesar. Pertahankan scene aktual; bidang lantai tersimpan 5×3,8 m, bukan instruksi resize. Hub chapter routing belum ada |

Persetujuan kreatif yang masih diperlukan: hubungan Dr. Hendra/dokter kepala, jejak Damar E7, diksi/penyajian gema `2604` dan pemeranan suster dari aset yang tersedia, art/acting final, serta tier performa rilis. Tidak menahan audit ini atau meminta izin ulang untuk revisi teknis yang sudah diminta.

### 11.3 Tambahan cerita opsional tanpa mengubah inti (USUL BARU)

- **Lastri sebagai korban, bukan monster acak:** Gelang bayi, label tanpa nama, dan EVP "Kembalikan... anakku" bisa disusun bertahap sehingga pemain menyimpulkan bahwa Lastri dijadikan alat ritual. Pola "hanya terlihat di layar" memperkuat gagasan bahwa ia dihapus dari catatan.
- **Nama yang hilang:** daftar 17 baris/16 nama, catatan "yang dihapus tidak membeku bersama jenazah", dan Label Pasien Tanpa Nama membentuk satu motif; sisipkan beberapa nama dan catatan kecil keseharian pasien agar kalimat penutup rahasia ("mata tertutup") terasa personal.
- **Kesaksian bertentangan:** Dr. Hendra menyembunyikan gunting, sementara catatan suster menyebut "jangan sampai dokter kepala tahu botol itu masih ada". Hubungan keduanya dengan Mbah Suro dapat diungkap bertahap tanpa teks tambahan wajib.
- **Chat tidak serba benar:** satu atau dua komentar keliru (mis. "pakai kamera belakang!") membuat pemain tidak sekadar mengikuti chat.
- **Akhir lebih mencekam:** setelah Blitz ketiga, musik turun hampir hening; baru hujan, napas Raka, lalu layar merah tua dan video tungku. Gunakan transisi audio sebelum UI chat penuh.
- **Selfie sebagai motif:** wajah Raka di layar Selfie sedikit berubah makin pucat selama babak (efek ringan, tanpa mekanik baru).

---

## 12. Checklist produksi (v2.2-AUDIT-r1)

**Artefak dan navigasi**

- [x] **Audit dokumentasi/repositori P2 selesai secara statis.** Bukti/kebutuhan reuse §14; audit 100 QA §15.3; runtime P2 belum dibuat, implementasi tetap menunggu izin.
- [ ] Buat denah RSUD: pelataran + IGD + Koridor Poliklinik (L1), basement (landing, Pos Petugas, zona otopsi, zona laci, gudang administrasi), L2 (tiga segmen lampu, Ruang Inkubator, Ruang Jaga Suster, Ruang Operasi). Validasi tuas keluar basement dan jalur pulang.
- [ ] Terapkan tembok bata Inkubator (pola K102 P1): AR membuka ilusi pertama kali; setelah `P2_CROSS_INKUBATOR_WALL` collider terbuka **dua arah dan tidak butuh baterai/AR**.
- [ ] Puzzle `1603`: papan poli, Buku Jaga, sensor Poli 03; **angka tidak boleh ditulis sebagai teks lingkungan**.
- [ ] Puzzle laci: 12 laci dengan tabel suhu di satu data; hanya C3 −24°C; tidak ada laci −20 s/d −23°C; rantai gaib hanya terlihat di AR.
- [ ] Sediakan gudang administrasi sebagai ruang nyata (daftar korban 17/16, papan kunci terlihat dari pintu, D1–D4) dan jejak Damar non-gating.

**Resource, encounter, dan boss**

- [ ] Colokan Kos opsional mengisi penuh sekali melalui interaksi fisik dan receipt campaign; Continue/retry tidak mengisi.
- [ ] Tiga dinamo (IGD, Pos Petugas, Ruang Jaga Suster): full-charge +25%/8s per lokasi, emergency `<10%`, 45s akumulatif, cooldown 120s, cap 10%; progres dipause saat terancam. **Tidak ada dinamo eksplorasi di arena.**
- [ ] Lastri di kamar mayat: scare #1 dengan jendela belajar, meter 10%/s dan turun 25%/s in-frame, hold 5,0 s maju ≥0,8 m/s, loiter 6 s, arah 50°/0,25 s, `HAIR_TOUCH` 2 s, rintihan 20 s dengan telegraph 3 s.
- [ ] Lastri di L2: patroli lampu per segmen, re-attach 12 s, zona tenang, brain yang sama dengan parameter berbeda.
- [ ] Formalin: telegraph 1,5 s, cancel dengan `E`, gas 6 s non-fatal, Lastri re-attach lebih cepat.
- [ ] EVP: toggle `C`, sesi 6,0 s, ≤2,0 m, hanya Inkubator 04; Gelang dan EVP dua flag.
- [ ] Brankas: gema AR `2604`, jalur deduktif cadangan, tidak terkunci pada baterai 0%.
- [ ] Pintu Ruang Operasi: kabut lenyap saat tiga pusaka dibawa; meja butuh tiga bukti + konfirmasi Jurnal + tiga pusaka.
- [ ] **CP5 hanya dari `E` valid di meja**, snapshot posisi dan baterai aktual, **sukses tulis disk sebelum boss**; stop autosave selama fight.
- [ ] Boss: PREP 30 s, tiga siklus, jendela 5 s, maksimal 1 Blitz valid per siklus, ilusi dari siklus 2, generator repeatable manual saat recovery, tiga sumber cahaya cukup tanpa flare.
- [ ] Cahaya dan poin Blitz hanya runtime; miss tidak menghancurkan lampu; tiga hit commit CP6 atomik; retry memulihkan pusaka/lampu/flare/posisi.

**Epilog, penyimpanan, dan pengujian**

- [ ] Epilog: hujan, Chat #3 (310.500), overlay Rp164,8 juta, video tungku 8 detik, pesan `Penonton_Terakhir`; kontrol dikembalikan **tanpa fade**.
- [ ] `P2_EXIT_PANEL_OPEN` hanya UI sementara di motor, **bukan completed event**; **hanya `P2_COMPLETE`** merupakan commit akhir (CP7); `CKREK!` dan hitam setelah commit sukses.
- [ ] Seluruh rute pulang aman karena Lastri `RESOLVED`; jebakan formalin dinonaktifkan setelah boss; basement dapat dikunjungi lagi untuk Label.
- [ ] Rp164,8 juta tetap angka naratif; donasi terikat tidak masuk saldo; selisih Rp77.934.334 tidak ditutup palsu.
- [ ] Kerjakan dialog, live chat, VO/EVP, ambience, lighting diegetik, set dressing rumah sakit, VFX, dan minimalkan placeholder pada jalur final.
- [ ] Jalankan seluruh QA-01 sampai QA-100 dan build Windows pada GPU nyata; uji 82 menit tanpa memaksakan durasi.

## 13. Ringkasan penyelarasan dan audit internal — hasil merger v2.1

Bagian ini menyimpan riwayat merger. **Audit repositori r1 sudah dilakukan**; klaim draf yang kadaluarsa digantikan §14–§15. Tabel di bawah mencatat celah yang ditemukan saat menyelaraskan P2 dengan P1 dan bagaimana penanganannya di draf ini.

| Poin | Validasi | Penanganan |
|---|---|---|
| Struktur P2 belum setara P1 (tanpa §5–§14) | **Valid, blocker dokumen** | Bagian ini melengkapi §5–§14 dengan kedalaman sepadan |
| Kontradiksi sumber 15 Oktober | **Valid** | §0.7 (E1–E18) dan §11.1 |
| Overlay Rp164,8 juta tidak terekonsiliasi | **Valid** | §5.3: selisih Rp77.934.334 ditandai, bukan ditutup; P2-D03 |
| Baterai dan softlock (0% di kamar mayat/boss) | **Valid, blocker** | §8.0: tiga dinamo, generator arena, colokan Kos, tanpa `WATCH_ONLY`; layar mati = jalur lepas sah |
| Instakill tanpa pengajaran | **Valid** | Jendela belajar 30 s, telegraph ganda, dua fatal saja (§8.2) |
| Telegraph tertutup kedap suara | **Valid** | `TelegraphBus` tidak diredam (§7.3) |
| Brankas "lewat AR" tidak dapat dikode | **Valid** | Gema AR + `2604` + jalur deduktif (E17) |
| Selisih binding Selfie | **Valid** | Prompt dari `GameInputConfig`; P2-D01 |
| Biaya render Selfie | **Valid, risiko performa** | §7.5: render kedua dibatasi, layer khusus; P2-D11 |
| Save terduplikasi/angka tersebar | **Valid** | Set ID di §6.2; parameter di §8.0; CP5 dibekukan selama fight |
| Flare dan CP6 | **Valid** | Flare terpakai pada attempt menang dikurangi saat CP6; retry memulihkan (§6.3) |
| Rahasia D1 terlalu mudah bocor | **Valid** | Clue di meja otopsi tanpa angka; angka 17 hanya di gudang (§5.1A) |
| Ketidakcocokan internal §0–§4 (R1–R9) | **Valid** | Diterapkan langsung di r1; lihat §13.4 |

**Perbandingan ringkas:** versi ChatGPT lebih ekstensif pada 100 tes, paket angka baterai dan catatan kontrol RMB; versi Claude memberi lebih banyak detail scene/AI serta hubungan cerita, termasuk E1–E18 dan errata R1–R9. Merger tidak hanya menumpuk teks: basis Claude dipertahankan, kontrak P1 Codex dan kasus QA tambahan disisipkan, **checkpoint dibuat tunggal CP5/6/7**. Tidak ada AI agent atau Codex yang menjalankan tes hanya karena dokumen ini selesai ditulis.

### 13.1 Saran yang sengaja tidak diadopsi

- **`WATCH_ONLY` gratis pada 0%** tidak dipilih untuk P2; tidak pernah diratifikasi. Alternatif: layar mati sama dengan sembunyi.
- **Blitz bertenaga lampu/generator tanpa batas** tidak dipilih; Blitz tetap 15%/6s dengan generator fisik repeatable pada recovery.
- **Teleport/spawn Lastri di belakang pemain** tidak dipilih karena membuat kematian tidak adil.
- **Menambah Tuyul/Genderuwo atau ancaman baru** tidak dipilih; tidak ada di naskah.
- **Menjadikan donasi `Penonton_Terakhir` saldo belanja** tidak dipilih; dana terikat.
- **Flare sebagai syarat boss** tidak dipilih; tiga sumber lingkungan cukup untuk tiga siklus.
- **Fungsi Kemenyan Bakar di P2** tidak dipilih; baru bermakna di Babak 4.
- **Lampu rusak saat miss** tidak dipilih; lampu korslet hanya setelah hit sukses.
- **Memulai boss saat memasuki ruangan atau saat pusaka terakhir diambil** tidak dipilih; hanya `E` valid di meja.

### 13.2 Hal yang tetap PERLU KEPUTUSAN penulis

Lihat status per ID pada §11.2: binding, schema/API dan ukuran Kos bukan keputusan kreatif yang harus dibuka ulang. Hubungan Dr. Hendra, jejak Damar, penyajian gema AR, art/acting dan tier performa masih perlu review sebelum freeze. Nubuat global yang ditahan tidak diaktifkan oleh P2; denah/beam dan tuning menunggu pengujian nyata.

### 13.3 Audit repositori r1 — batas bukti

Sudah dilakukan: audit statis kode, scene/aset terarah, `Rancangan.MD` dan seluruh 100 oracle QA. Belum dilakukan: implementasi P2, simulasi/PlayMode RSUD, pengukuran FPS Windows dan playtest manusia. Reuse visual Kos memungkinkan, tetapi routing/guard director Prolog harus ditambah sebelum dipakai sebagai HUB P2. **Belum phase freeze.**

### 13.4 Errata r0 yang sudah diterapkan di r1

Sembilan ketidakcocokan pada §0–§4 yang ditemukan saat menyusun lanjutan **sudah dikoreksi langsung pada teks** di versi r1. Tabel ini tinggal sebagai jejak audit.

| # | Lokasi | Masalah r0 | Perbaikan yang diterapkan |
|---|---|---|---|
| R1 | §3.1 denah, baris gudang administrasi | Mencantumkan "catatan perawat" di gudang, padahal §3.3 dan §5.1 menaruhnya di meja otopsi | "catatan perawat" dihapus dari baris gudang |
| R2 | §3.3 vs §3.3A | Gudang "di sisi timur", koordinat §3.3A menaruhnya di utara | §3.3 kini "sisi utara zona otopsi"; dinding laci tetap di timur |
| R3 | §3.4, Ruang Inkubator | Sisa kalimat tanya "…sebelum EVP selesai? **Tidak:**…" | Kalimat dirapikan: pengambilan Gelang/Perekam tidak dikaitkan ke bukti EVP |
| R4 | §3.3A, jeda sebelum `GRAB` | "≥13 s" tetapi dijumlahkan 15 s | **12 s setelah attach, 15 s sejak rintihan mulai** (selaras QA-46) |
| R5 | §4.4, invarian Blitz | Rujukan "(E15)" keliru | Diganti rujukan ke catatan aturan khusus Adegan 5 |
| R6 | §1 Adegan 1, flare | Frasa "area terlarang tertentu" bertentangan dengan §2.1 | `G` hanya berfungsi saat boss aktif; flare tidak terkonsumsi di luar boss |
| R7 | §0.3, §4.2, §4.4 | Idle-lunge/meter out-of-frame tampak seperti fatal ketiga | Ditegaskan sebagai varian `GRAB`, bukan fatal baru (P2-D13 tetap berlaku) |
| R8 | §4.4, anggaran baterai | "alternatif D01" ambigu | Menjadi "alternatif D01 Phase 1" |
| R9 | §1 Adegan 4, §3.4, §7.2, QA-39 | `2604` bergantung mengunjungi Inkubator 04 lebih dulu | **Papan penempatan bayi** di Ruang Jaga Suster (baris 04 berinisial L.) menutup ketergantungan urutan; QA-39 diperbarui |

---

## 14. Peta integrasi terverifikasi — audit `9ca5f57`

Seluruh path source relatif ke `Assets/_LayarTerakhir/src/`. **Langsung pakai** berarti fondasi/API tersedia, bukan konten RSUD telah dibuat. **Adaptasi** berarti perlu integrasi terlokalisasi. **Baru** berarti konten/modul babak belum ada. Tidak satu pun perubahan runtime di bawah telah diterapkan oleh audit ini.

### 14.1 Gunakan fondasi ini; jangan buat versi paralel

| Kebutuhan / kelas kelayakan | Bukti aktual | Rencana minimal dan batas reuse |
|---|---|---|
| Campaign/transaksi — langsung pakai | `Core/CampaignState.cs:TryCommit`, `BeginCheckpointAttempt`, `EndCheckpointAttempt`, `ArchiveDocument` | Reuse atomic file/rollback/write barrier; owner attempt P2; jangan menulis serializer/save/event bus kedua |
| Data/migrasi — adaptasi P0 | `CampaignData` schema 2; `TryReadCheckpoint` hanya migrasi 1→2; `IsValid` menolak versi lain | Tambah nested `phase2`, migrasi 1→2→3 dan validator; round-trip field P0/P1, backup, corrupt/unknown version; schema 3 bukan langsung usable |
| Receipt/stage — adaptasi | `Core/Phase1Data.cs`, `Phase1/Phase1Rules.cs`, `Phase1Director.Commit/RestoreWorld` | Receipt chapter terpisah, data global tetap satu; stage dari prasyarat dengan milestone monotonic dan anchor terpisah; tidak menyalin keseluruhan director P1 |
| Routing/lanjut — adaptasi P0 | `Core/Phase1Data.cs:CampaignRouting.ContinueScene`, `UI/GameShell.cs:Continue` | Saat ini hanya P0/P1; tambah `PHASE2_HUB`→Kos mode P2, `PHASE2_CP1..7`→RSUD, entry P1_COMPLETE+segel; chapter asing tetap ditolak tanpa reset |
| Kos/PC/motor — adaptasi P0 | `Prolog_KamarKos.unity`, `Prolog/KamarKosPrologDirector.cs:Awake/InitializeStory`, `Prolog/KosRoomCorrections.cs` | Reuse scene dan geometri yang desainer besarkan. Director Prolog sekarang merutekan P1 ke Rusunawa/menolak chapter 2; guard **sebelum Awake side effects/Start** dan provider HUB P2 diperlukan. Jangan hanya load scene lalu berharap aman |
| Movement/OTS/FP — langsung pakai | `Player/FirstPersonController.cs`, `ShoulderCameraRig.cs`, `AnimatedRakaAvatar.cs` | Walk efektif dibatasi 1,7 m/s; pertahankan akselerasi/crouch/collision; timer hold memakai kecepatan kapsul aktual, bukan input W atau animasi avatar |
| HP/feed/RMB — adaptasi | `Smartphone/SmartphoneController.cs`, `Smartphone/IPhoneChapterContent.cs`, `Prolog/AtmajaPhoneView.cs` | Reuse handset/grip/feed/app provider; X sudah toggle flag, **feed masih selalu menghadap depan** dan mengecualikan avatar. Tambah pose kamera depan pada **FeedCamera yang sama**, bukan HP/RT kedua permanen |
| Thermo — baru, di HP existing | `Core/GameInputConfig.cs:ModeThermo` hanya binding; smartphone belum punya sensor | Tambah overlay sensor target-authored + dwell/LOS/arah feed; bukan simulasi termodinamika atau membaca suhu hardware |
| EVP — adaptasi, tersedia | `Phase1/Phase1Director.cs:Update` recorder toggle C, `Phase1Balance.evpSeconds=7` | Reuse alur UI/rekam/receipt; P2 sesi 6s/2m/LOS dari config target dan toggle C. Tidak menyalin recorder/ambush Genderuwo atau meminta mikrofon OS |
| Energy/Blitz — adaptasi | `SmartphoneController.TryCaptureEnergy`, `TryUsePowerbank`, `Phase1Threats.TryCapture` | Reuse satu transaksi biaya 1/15 dan cooldown 6s; perlu pengenalan instance powerbank P2. Saat ini resolver hanya P1 dan ID legacy; jangan mengira ID baru otomatis terpakai. Consumable saat boss dimutasi RAM lalu tercakup CP6 |
| Charger — adaptasi | `Phase1/Phase1Presentation.cs:Charge`, `Phase1Balance.cs` | Reuse pola hold/progres/commit; fungsi masih private dan P1-bound, bukan komponen generik drop-in. Adapter P2 kecil; eksplorasi +25/8/45/120/10, arena manual repeatable +25/8 saat recovery |
| E/keypad/Jurnal — langsung pakai + isi P2 | `Player/InteractionInteractor.cs`, `IContextInteractionResolver`, `UI/HudController.cs`, `PhoneDesktop.cs`, `Phase1Presentation.PopulateApp` | Reuse keypad dinamis, arsip, Journal page provider dan resolver tangan+LOS; satu pemilik E. `Phase1WorldItem` terikat Phase1Director, buat item adapter chapter lokal, bukan sistem interaksi paralel |
| AR/passage — adaptasi | `AtmajaPhoneView.ARLayer=29`; `Phase1Director` reveal/grace/occupied latch | Reuse culling/LOS dan prinsip latch crossing atomik dua arah; P2 target/receipt baru, tidak reuse literal tembok rusun |
| Lastri/nav — baru dengan pola P1 | `Phase1Navigator.cs` flatten y, capsule radius .50 dan graph per-level; `Phase1Threats` P1-bound | Lastri attach/patrol/boss satu brain chapter, state eksklusif; reuse pola clearance/LOS/hiding, bukan menyalin semua AI atau menganggap graph P1 multi-floor siap. Graph lokal per-lantai dan transisi pintu/tangga eksplisit |
| Audio — adaptasi + konten baru | `Audio/SceneAudioProfile.cs` hanya ambience/effects/UI; `Phase1Soundscape.cs` pool/filter; `GoldenScene.mixer` tiga grup + master | Exterior/Telegraph/Voice pada §7 adalah **peran routing baru**, bukan grup existing. Reuse mixer/sumber/pool; per-source filter/gain/routing telegraph terpisah. Tidak membuat event bus hanya untuk istilah TelegraphBus |
| Video — adaptasi | `Phase1Presentation.Epilogue` RT 320×512 set Damar lokal | Reuse pola playback/set recording tanpa gameplay Babak 3; durasi P2 8s, konten tungku/Damar berlisensi, start/finished receipt dan charger pasca-boss |
| Scene/aset — baru + reuse selektif | `ProjectSettings/EditorBuildSettings.asset` empat scene, terakhir `Babak1_Rusunawa`; belum RSUD | Kos/karakter/motor/HP reuse. Wheelchair dan medical_box P1 ada; RSUD belum punya paket lengkap tervalidasi. Meja operasi/otopsi, laci, inkubator, Lastri, VO perlu sumber khusus berlisensi dan staging tersendiri |
| QA — reuse infrastruktur, kasus P2 baru | `Editor/Phase1CampaignTests.cs`, `AtmajaPlayTests.cs`, `PlayerFeelTests.cs`; `Tools/run_golden_checks.py` | Pertahankan regresi P0/P1; 100 kasus di §10 bukan hasil tes. Jalankan P2 hanya setelah coding diizinkan; Linux/headless bukan GPU Windows |

### 14.2 Ketentuan state yang harus diperinci sebelum kode

1. Pisahkan **data campaign persisten**, **snapshot aman checkpoint**, **state percobaan boss runtime**, dan **UI/pose sementara**; tetap satu file save dan satu API campaign. Event tidak disamakan dengan item fisik; cache runtime bukan sumber kebenaran tambahan.
2. CP1–CP7 memakai ID bernama `PHASE2_CP1` … `PHASE2_CP7`. Definisikan anchor posisi/yaw, pintu/collider, mode HP, AI, dan objective tiap restore. **CP4 per bukti/pusaka memakai anchor aman**, bukan posisi saat Lastri menempel.
3. **CP3 sebelum scare #1** menyimpan satu snapshot tanpa top-up baterai. Restore memutar ulang scare #1 sebagai telegraph; jangan menghidupkan Lastri tepat di wajah pemain.
4. **CP5:** `E` di meja, tiga bukti + konfirmasi Jurnal + tiga pusaka, write sukses **lebih dulu**, lalu ritual RAM. Tidak ada transaksi ekonomi, dokumen baru, atau autosave callback selama fight. **CP6:** satu commit boss + segel + pusaka ke meja + flare terpakai. Jika write CP6 gagal, masuk recovery UI dengan retry commit atau restore CP5; jangan menjalankan epilog dari state belum tersimpan.
5. **Secret pickup** melakukan write atomik sebelum sukses UI/objek hilang. Autosave setelah secret pasca-boss mempertahankan CP6/kemajuan epilog; jangan menulis CP2/CP3 hanya karena item ditemukan di basement.
6. **Rekaman Damar** dan **video tungku** perlu membedakan **mulai** dan **selesai** secara deterministik; restart di tengah rekaman boleh memutar ulang tanpa reward ganda. Timer runtime tidak disimpan.
7. Tabel prioritas input: Menu/animasi wajib > interaksi encounter (`E` sakelar/generator/laci) > UI HP (Jurnal/live) > foto/Blitz > interaksi normal. Mode browsing Jurnal tidak membekukan dunia. Saat Lastri menempel, tombol UI Stow, Q/R/V/X/T/C/J, RMB release, dan kehilangan fokus juga butuh kontrak; **layar mati dan baterai 0 harus dihitung sama dengan `F`** untuk loiter.
8. Frustum Selfie memakai target bounds toleran dan LOS, bukan titik pusat tunggal; arah "menoleh langsung" dievaluasi dari **yaw kontrol pemain**, bukan kamera Selfie. Fatal didahulukan dari `E`, tetapi dalam `HAIR_TOUCH` `E` boleh dipakai tanpa membatalkan `GRAB`.
9. Dinamo eksplorasi nonaktif selama boss; generator arena fisik dan terjangkau pada baterai nol. Exposure 5 s tetap aman dari serangan; miss/cooldown tidak menghabiskan lampu. Stage tidak mundur karena backtracking; `TABLE_READY` hanya kondisi interaksi sementara.
10. Disk format tidak menyimpan dictionary/HashSet mentah atau `Time.time` absolut. Jam gameplay persisten berhenti sesuai kebijakan pause. Tentukan perilaku restart pada crank 30/45 s (default: progres sesi boleh hilang) dan pemulihan sisa cooldown 120 s.
11. Pastikan **prasyarat masuk** (`P1_COMPLETE` + `SEAL_RUSUNAWA_GRIYA_ASRI`) diperiksa di satu tempat; jalur dev tanpa P1 ditandai dev-only dan tidak menulis segel/uang/inventori palsu ke save produksi (QA-76).

### 14.3 Urutan kerja setelah izin implementasi (dependensi dan risiko)

| Tahap | Hasil/gerbang | Risiko / kompleksitas relatif |
|---|---|---|
| M0 | Bekukan kontrak §15; simpan baseline regresi P1, terima keputusan artistik terbuka §11.2 | P0; sedang. P1 belum freeze, jangan menyebut seluruh regresinya selesai |
| M1 | Migrasi/validator/entry dan Continue HUB/RSUD, guard Prolog, transaksi shop/colokan; test crash dan backup | P0; tinggi; harus sebelum load scene produksi P2 |
| M2 | Proof-of-concept **FeedCamera** Selfie dengan wajah Raka, culling/LOS/mirroring/hit alignment, Thermo overlay dan input ownership | P0; tinggi; gate sebelum produksi encounter/aset final |
| M3 | Denah dan aset berlisensi rumah sakit; slice IGD→Poli→1603→basement; jalur pulang/clearance semua lantai | P0/P1; tinggi, tergantung aset nyata; art direction sejak awal, bukan greybox dianggap final |
| M4 | Attach/turn/hold/hiding, formalin, 12 laci, zero battery dan secret; QA terukur pada controller nyata | P0; tinggi; satu brain/healthless sesuai kanon |
| M5 | L2 quiet zones, AR crossing, EVP reuse, brankas, tiga pusaka/Jurnal; bebas urutan dan save failure | P0/P1; sedang–tinggi |
| M6 | CP5 barrier, tiga sumber cahaya, ilusi/target Thermo, pemulihan berulang, CP6; minimal 20 retry/fault injection | P0; tinggi; ray gameplay dan gambar wajib sama |
| M7 | Acting/audio/epilog/video 8s, backtrack D1, motor CP7 dan endpoint menu (tanpa scene P3) | P1; sedang–tinggi; casting/lisensi konten dapat menghambat |
| M8 | Semua 100 QA + kombinasi §15.4, visual/audio acceptance, pacing 82 menit, profil Windows nyata | P0 untuk rilis; tinggi; build/upload baru hanya ketika implementation scope disetujui |

Tidak ada estimasi jam pasti sebelum proof Selfie, sampling aset, dan profiling. Sistem pondasi dapat diintegrasikan langsung, tetapi **semua konten P2 belum dibuat**. Tidak ada kode, scene, prefab, aset atau config game diubah dalam audit ini; `Phase1.MD` dan `Rancangan.MD` dipertahankan.

### 14.4 Identitas data teknis dan aturan yang tidak boleh digandakan

Identifier di bawah adalah kontrak teknis usulan untuk implementasi, bukan fitur yang sudah ada. Nama dapat disesuaikan sekali sebelum kode; bila diganti sesudah save beredar, sertakan migrasi.

| Kategori | ID / aturan authoritative |
|---|---|
| Tiga bukti P2 | `P2_FREEZE_POLI03`, `P2_AURA_TUMBAL_C3`, `P2_EVP_INKUBATOR04` di `CampaignData.evidence_ids`; event `P2_EVIDENCE_*` pada registry adalah notifikasi pemicu, bukan counter bukti kedua |
| Tiga pusaka | `P2_GUNTING_TALI_PUSAR`, `P2_GELANG_BAYI_LASTRI`, `P2_BOTOL_DARAH_SEGEL`: di `inventory` = **sedang dibawa**; `relic_ids` = **sudah ditemukan/terdaftar**, tidak dihapus saat diletakkan di meja. Jangan memakai keduanya sebagai dua nilai kepemilikan yang harus ditebak |
| Kunci | `P2_KUNCI_TERALIS_L2` dan `P2_KUNCI_ARSIP` persisten; tidak dikonsumsi |
| Consumable | `P2_POWERBANK_PASAR_GAIB_A/B`, `P2_FLARE_A/B/C`, `P2_KEMENYAN_A/B`: instance di inventori; receipt sumber (`P2_SHOP_PACK_*`) mencegah grant ulang; instance dihapus saat dipakai, receipt tidak dihapus |
| Secret | `SECRET_P2_LABEL_PASIEN` di `secret_items`, bukan evidence/relic; hitungan lokal memakai `Contains` ID tepat, global memakai lima ID sah dari rancangan kampanye |
| Segel lokasi | `SEAL_RSUD_LAMA_SIDO_WARAS`, hanya ditambah saat CP6. Tiga Blitz **bukan** tiga `location_seals` |
| Dokumen | `P2_DOC_FORUM` (bonus), `P2_DOC_NURSE_LOG`, `P2_DOC_AUTOPSY`, `P2_DOC_DAMAR_RECORDER`, `P2_DOC_NURSE_NOTE`; membaca dokumen clue bukan substitusi bukti |
| Event runtime (tidak dipersist) | `P2_TABLE_INTERACT`, `P2_LIGHT_USE`, `P2_SHIELD_BREAK`, `P2_BLITZ_SUCCESS`, `P2_EXIT_PANEL_OPEN` |
| Event persisten penutup | `P2_BOSS_WIN` (via CP6) dan `P2_COMPLETE` (via CP7); epilog selesai dan video memakai receipt terpisah |
| Checkpoint | `PHASE2_HUB` dan `PHASE2_CP1` … `PHASE2_CP7` |
| Prasyarat | `P1_COMPLETE` + `SEAL_RUSUNAWA_GRIYA_ASRI`; jalur dev ditandai dev-only |

**Kelengkapan QA di luar 100:** uji Continue pada setiap CP termasuk checkpoint asing; enam permutasi urutan bukti dan konfirmasi Jurnal; urutan Ruang Inkubator vs Ruang Jaga Suster; seluruh jalur input Stow/app/RMB/remap saat Lastri menempel; autosave dokumen/pembayaran selama boss; fault injection untuk secret, tembok bata, CP5/CP6/CP7, dan transaksi belanja; topology/clearance arena dan jalur loop; serta frame spike saat render Selfie aktif. Jangan menganggap 100 kasus menutup semua kombinasi ini.


## 15. Kontrak koreksi audit repositori r1 (mengikat implementasi berikutnya)

### 15.1 Konflik yang ditutup dan alternatif yang dipilih

Ketentuan operasional berikut memperinci §0–§14. Cerita enam adegan, Lastri, 17 korban, tiga bukti/tiga pusaka, kode/suhu cerita, tiga siklus dan window 5s dipertahankan. Semua nilai geometry/tuning masih perlu pengujian, bukan jaminan “bebas softlock” dari audit statis.

1. **Selfie yang benar, bukan mengganti UI flag.** AtmajaPhoneView saat ini selalu mengembalikan FeedCamera ke local forward .08/rotasi identitas dan mengecualikan layer avatar 26. Tambah cabang orientasi **pada kamera/feed yang sama**, titik lensa depan terhadap HP, wajah/torso dari karakter desainer existing, clearance kepala/near-clip dan culling per lensa. Wajah tidak boleh disubstitusi gambar AI. Selfie RT boleh dimirror untuk presentasi; target ray/reticle harus meng-unmirror X **sekali**, gambar dan hitbox diuji pada sisi kiri/kanan. Grip 31 tidak ikut render ke feed; kamera depan tidak melihat dirinya atau RT secara rekursif. Jika HP foreground dan tubuh memakai representasi berbeda, cocokkan tangan/warna tanpa mengganti model dunia.
2. **Layer bukan izin menembus dinding.** Yang sudah terpakai: avatar 26, recording 28, AR29, permukaan ilusi30, grip31. TagManager belum memberi nama pada slot tersebut, namun kode memakainya. Jangan mengambil ulang slot itu untuk Lastri. Tentukan satu slot bebas setelah memindai pemakaian kode/scene saat implementasi (27 kandidat, **belum reservasi**), apply hanya pada renderer Lastri/ilusi. Collider/LOS memakai geometri fisik solid, bukan mask kamera. Main/rear feed/RT recording/grip mengecualikannya saat shielded; Selfie mengikutsertakannya, EXPOSED memperbolehkan wujud asli di world/rear. AR tidak membuat Lastri terlihat dari belakang secara gratis.
3. **Target sensor dan framing tunggal.** Thermo adalah overlay data pada lensa aktif: jarak+dwell+kerucut+LOS ke target authored, tidak mengambil nilai dari kamera nonaktif. Untuk boss, reticle memilih satu kandidat terlihat pada jarak ≤8m; asli −20°C, ilusi −6°C, jarak ilusi tidak sengaja menjadi jawaban pasti. Pembacaan investigasi Lastri ≤3m; pembacaan laci hanya rear feed. ≥30% projected rect + ≥3/9 anchor LOS, toleransi occlusion ≤0,25s; dinding solid setelah itu menghentikan watch/dwell. Jangan memakai prop renderer bounds seluruh gaun/partikel sebagai satu head target. Lensa yang dipakai foto/Blitz sama dengan yang tampak di layar, biaya satu kali; PulseShutter/flashlight existing menghadap belakang, sehingga presentasi emisi depan perlu cabang Selfie yang sesuai target tanpa mengonsumsi energi dua kali.
4. **Menoleh harus mungkin dideteksi tanpa mengejar kamera.** Lastri menapak/melayang pada lintasan world di belakang jejak kapsul; jangan parent ke player lalu memutar 180° bersamanya. Jika selalu dihitung `player.position - player.forward*d`, POUNCE mustahil; jika snapshot world tidak diberi rute, hantu memotong meja/dinding. Gunakan trail/graph lokal dengan clearance dan LOS; rotasi spontan feed/OTS/crouch tidak dihitung menoleh. Tuning yaw 50°/0,25s memakai yaw kontrol kapsul. Belokan jalur maju diperbolehkan selama tidak menghadap lokasi Lastri yang sebenarnya; flinch satu kali per threshold crossing. Selama perpindahan lantai/safe anchor, matikan serangan sampai telegraph/reposition valid, tanpa spawn di dalam player.
5. **Gerak maju bukan tombol W.** Ukur displacement kapsul horizontal pada gameplay clock, dot dengan forward kontrol dan kecepatan ≥0,8m/s. Didorong collider atau W tertahan pada dinding tidak menambah hold 5s. Timer pause saat syarat gagal, reset hanya detach/retry/new attach; loop 16m/arena 14–20m masih hipotesis. Pasang laci/sakelar dalam jangkauan tangan+LOS; E tetap milik interactor existing, tidak memakai arah kamera Selfie untuk tangan. Blueprint XZ; graph P1 flatten-Y tidak langsung dipakai melintasi tangga/lantai.
6. **RMB/F/Tab/X/T tidak saling memaksa.** RMB hanya override app Kamera/raised state/komposisi, snapshot sekali pada press dan restore pada release. X pilihan lensa dan Q/R filter eksplisit, T overlay; perubahan eksplisit itu tetap sampai diubah lagi, bukan diam-diam di-undo oleh release RMB. F/Tab mematikan/menyimpan menang atas RMB dan menekan ulang memerlukan release/repress; held RMB tidak menyalakan HP kembali frame berikutnya. CameraLocked P1 mengunci F/Tab dengan pesan Kuntilanak L3 sehingga **tidak dipakai untuk attach P2**. P2 harus mengizinkan screen-off/hiding. Override sementara restore selected OTS/FP, cursor dan app pada mati/load/scene unload/focus; jangan mengubah binding/rasa P0/P1.
7. **Fair UI/pause.** Jurnal nonmodal tetap bisa ditutup dan karakter bergerak; grace 4s sekali per attach, tidak diperbarui spam. Hard modal baru saat attach ditolak dengan hint; hard modal yang dibuka aman/pause/cinematic menghentikan clock ancaman dan drain bersama. Focus loss memakai pemilik pause GameShell yang sama dan resume sadar tanpa catch-up delta; ini kebutuhan integrasi, belum fitur teruji. Pickup tidak mematikan ancaman via Cinematic. Kedua fatal diblokir pada belajar sampai detach pertama atau 30s, mana lebih dulu. Meter/HAIR_TOUCH selama belajar tidak membawa deadline kematian; saat learning berakhir, meter kembali0 dan telegraph penuh sebelum ancaman normal, bukan GRAB langsung dari timer tersembunyi. Bila HAIR_TOUCH belum lewat deadline, pantauan sah atau hiding dapat membatalkan GRAB dan menurunkan meter; E pickup tidak membatalkannya. Sesudah deadline, fatal menang atas E. Idle-lunge boss di-reset oleh displacement nyata, bukan goyang input; tidak berjalan pada EXPOSED/recovery.
8. **Recovery boss dapat diulang.** Generator once-per-phase + cap emergency10 tidak cukup menghadapi miss arbitrer (satu Blitz15). Dipilih pola P1: +25 poin/8s manual repeatable saat PREP/RECOVERY/POWER_RECOVERY dan sesudah CP6. Miss/timeout menuju recovery dengan lampu tetap utuh; battery<15 saat TRACK memberi POWER_RECOVERY bertelegraph dan menonaktifkan fatal saat Lastri mundur, tanpa mengubah hit/siklus/lampu/flare. Minimum30/20s bukan deadline. E meja melanjutkan sadar ketika daya≥15/cooldown siap; HP diangkat tidak otomatis melanjutkan. Ini mengubah baseline pemulihan teknis, bukan jumlah siklus atau biaya. Uji apakah retreat/tempo tetap menegangkan; jangan menambah resource grant atau harga wajib untuk menutupi bug.
9. **Flare, miss, powerbank.** Satu sumber pemicu diberi token exposure; jika banyak cahaya overlap, pemicu pertama menang, satu lampu saja korslet setelah hit. Flare terpakai saat dijatuhkan; hanya drop saat TRACK/LURE dan posisi bebas sesuai collision, di PREP/EXPOSED/recovery ditolak tanpa konsumsi. Ilusi/kaca tidak memblokir ray solid wajib atau menutup jalur. Blitz combat yang miss tetap membayar15/cooldown6 (termasuk palsu saat TRACK), tanpa seal; foto bukti1 hanya saat target evidence valid. Resolusi combat/evidence satu pemilik. Tambah resolver instance powerbank P2 ke SmartphoneController existing; receipt grant tetap, instance terpakai hilang. Selama attempt semua konsumsi termasuk powerbank RAM, bukan disk; CP6 mengomit inventori/baterai akhir dan daftar flare terpakai sekali. Tidak mengurangi flare yang sudah dihapus untuk kedua kali.
10. **HUB dan transaksi antar-scene.** `P1_COMPLETE` dicek di `data.phase1`, bukan mencari flag inventory P0; segel sesuai `Phase1Ids.Seal`. Transaksi masuk chapter2 memakai `PHASE2_HUB` sebelum load Kos, menjaga seluruh data P1 dan baterai aktual. Guard director/interactable Prolog sejak Awake dan reset UI/provider chapter diperlukan agar PC/motor P2 tidak menjalankan puzzle148/pelunasan/wake-up P0. Departure commit CP1/anchor IGD sebelum fade/load; crash tetap bisa Continue IGD. Upgrade memakai daftar `upgrades` dan receipt satu kali, fallback sebelum depart, bukan softlock bila forum/menu dilewati. Paket shop dan colokan satu receipt per campaign P2; spent/debit/instance/receipt dalam satu TryCommit. Charge hanya E colokan opsional, tidak saat depart. Kos/PC/material/ukuran existing dipertahankan; hanya lighting/profil siang lokal saat implementasi.
11. **Checkpoint bukan angka urutan progres.** Bukti Poli dapat menulis CP4 sebelum keypad CP2; stage tidak memakai `max(number)` atau urutan enum tunggal. Evaluasi prioritas milestone `COMPLETE > EPILOGUE > BOSS_WIN > boss attempt > READY > L2 visited > ...`, berdasarkan receipt+prasyarat; TABLE_READY hanya kondisi jarak. Anchor/yaw aman per lokasi di nested phase2; daftar anchor sah wajib divalidasi. CP4 menyimpan bukti/loot baru atomik tetapi **tidak** menimpa CP5/CP6/CP7 saat backtracking/ambil rahasia. Mati memuat **snapshot terbaru**, tidak selalu CP3. CP2 tidak memberikan bukti/dokumen yang ditebak pemain; sensor/clue tetap accessible. CP3 hanya sebelum scare pertama; kunjungan ulang tak menimpa progres tinggi. Crossing AR menyimpan latch sebelum menutup jalur/menampilkan sukses; gagal write mempertahankan safe opening selama kapsul di aperture agar dapat retry/keluar.
12. **Data lama/korup dan kepemilikan attempt.** Rencanakan migrasi 1→2→3 di loader tunggal; validator P0/P1 tetap, P2 mencakup receipt unik, anchor, finite elapsed/dynamo timestamps≤elapsed dan nonnegative saldo. Null optional lama diisi hanya lewat migrasi yang ditentukan; duplikat/required ID palsu bukan alasan mengarang progres. Future/invalid schema ditolak dengan opsi backup tanpa NewGame otomatis. Dokumen tetap CampaignData.documents + receipt existing; tidak `document_ids`/shop boolean kedua. BeginCheckpointAttempt(owner) sesudah CP5 sukses, semua callback memakai barrier; CP6 memakai owner yang sama dan End hanya sesudah sukses, atau bersihkan pada restore. Gagal CP6: bekukan AI/clock dan tawarkan retry commit atau restore CP5; jangan meneruskan epilog/hadiah dari write gagal.
13. **Zero battery di seluruh rute.** Zona tenang L2 detach saat crossing **tanpa membutuhkan Selfie**, termasuk 0%; Pos Petugas punya hiding yang terjangkau tanpa HP. Sesudah detach, crank darurat di Pos dapat dilanjutkan, tidak berjalan sembunyi; demi tidak menjadi loop20s vs crank45s yang mustahil, reattach tidak dijadwalkan ketika player sudah aman di Pos dengan pintu tertutup dan sedang mengoperasikan crank. Hiding menuntaskan loiter saat layar benar-benar mati/battery0. C3 thermal-only latch, aura AR terpisah. D1 dapat dibuka dengan kunci tanpa mandatory scan tersembunyi (clue membantu deduksi). Semua kunci/item critical statis tidak dapat hilang oleh physics, gas/VFX atau reload.
14. **Perekam/epilog dan live opsional.** Perekam dan video8s memakai receipt STARTED/FINISHED terpisah; reload mengulang beat belum selesai tanpa reward dua kali. Player yang melewatkan live manual IGD tetap menerima plot via sequence lokal P2_EPILOGUE_CHAT/DAMAR_VIDEO (bukan jaringan); petunjuk mandatory punya padanan lingkungan. Jika CP6 battery0, generator pasca-boss tersedia, tampilkan objektif isi daya lalu lanjut video, tanpa refill terselubung/skip cerita. Setelah EPILOGUE, motor/arsip dapat dipakai tanpa layar. Lastri RESOLVED, formalin dinonaktifkan dan rute aman. Secret sesudah CP6 mempertahankan checkpoint/epilog, CP7 hanya mempertahankan segel CP6 dan menambah receipt complete/destination; shutter/fade sesudah write sukses. CP7 berakhir di endpoint menu, bukan memuat Babak3 yang belum ada.
15. **Aset dan kualitas.** Tidak ada alasan teknis menyatakan rumah sakit/Selfie mustahil di Unity/URP; masalahnya konten dan integrasi belum tersedia. Reuse fondasi/karakter/motor/HP, bukan mendandani Rusunawa menjadi RSUD. ≥30 prop unik target produksi, sampling skala/material/lisensi sebelum belanja unduh; belum ada manifest P2 tervalidasi. Bila mesh inkubator/Lastri spesifik belum ditemukan, itu dependency pengadaan, **bukan izin menghapus adegan, mengganti dengan kubus final atau menggambar aset sendiri**. Cari existing/unduhan berlisensi yang setara fungsi; jika butuh biaya/identitas karakter berbeda, catat keputusan kreatif sebelum freeze. Cermin retak opsional **prop noninteraktif** bila refleksi tambahan terlalu mahal; horor wajib tetap di Selfie, tanpa reflection camera rekursif. Audio per-source/mixer existing, telegraph tidak teredam; VO sintetis/subtitle bukan acting final yang telah disetujui.

### 15.2 Klasifikasi kelayakan dan prioritas

| Klasifikasi | Cakupan | Prioritas |
|---|---|---|
| Bisa langsung digunakan sebagai fondasi | TryCommit/barrier/save/arsip, input bindings, controller/crouch/OTS/FP, handset/grip/feed/AR, IPhoneChapterContent, keypad/HUD, audio profile/pool dan test runner | P0 integrasi tanpa sistem paralel |
| Perlu adaptasi lokal | Migration/routing HUB→RSUD, guard Prolog, EVP toggle6s/2m/LOS, powerbank P2, charger/jurnal/live, crossing latch, target-based sensor/feed, modal/focus, consumable attempt CP6 | P0 sebelum encounter produksi |
| Perlu modul/konten chapter baru | Thermo data, render Selfie orientasi/mask, Lastri attach/patrol/ritual/ilusi, formalin/laci, shop provider, level RSUD/aset/audio/VO/video | P0/P1; feasible, bukan “sudah ada” |
| Tidak bisa dipakai secara literal dengan baseline saat ini | Langsung load schema3, route chapter2 via P0, flag Selfie tanpa rotasi feed, instance P2 otomatis dikenali powerbank, graph P1 flatten-Y lintas lantai, generator once-per-phase/cap10 sebagai jaminan retry arbitrer, angka 30 FPS sebagai pengganti target global | Diganti alternatif §15.1; tidak ada mekanik inti cerita yang dihapus |
| Belum bisa dinilai/freeze | Kualitas final Lastri/rumah sakit/VO, kenyamanan hold/turn, pacing82menit, kinerja Windows/Iris Xe dan seluruh acceptance gameplay | Menunggu implementasi/aset/playtest, bukan dibilang mustahil atau lulus |

### 15.3 Validasi seluruh 100 QA — audit spesifikasi, bukan hasil runtime

Setiap ID §10 diperiksa terhadap kontrak/determinisme/dependensi dan dipertahankan tepat sekali pada tabel skenario. **34 oracle diperbaiki langsung**; 66 lainnya dipertahankan dengan kontrak §15.1 sebagai penjelas. `SESUAI` = tidak ditemukan konflik spesifikasi setelah audit, **bukan tes lulus**. `REVISI` = oracle §10 dikoreksi. Status implementasi/pengujian P2 **NOT_RUN untuk semua baris**; tes P0/P1 lama tidak dialihkan menjadi hasil P2.

Metode penerimaan kelak: **E** EditMode/data+fault injection; **P** PlayMode/input+physics/render checks; **W** standalone Windows/manual route+quality; **G** GPU/frame-time benchmark; **A** provenance/lisensi dan aset scene. P/W dapat melibatkan inspeksi visual, tidak cukup unit test yang meniru rumus.

| QA | Audit oracle | Metode penerimaan yang diperlukan | Status runtime P2 |
|---|---|---|---|
| QA-01 | SESUAI | E/P/W | NOT_RUN |
| QA-02 | SESUAI | W | NOT_RUN |
| QA-03 | SESUAI | E/P | NOT_RUN |
| QA-04 | REVISI | E/P/W | NOT_RUN |
| QA-05 | SESUAI | E/P | NOT_RUN |
| QA-06 | SESUAI | E/P | NOT_RUN |
| QA-07 | SESUAI | E/P | NOT_RUN |
| QA-08 | SESUAI | E/P | NOT_RUN |
| QA-09 | SESUAI | P/W | NOT_RUN |
| QA-10 | SESUAI | E/P | NOT_RUN |
| QA-11 | REVISI | P/W | NOT_RUN |
| QA-12 | SESUAI | E/P | NOT_RUN |
| QA-13 | SESUAI | E/P/W | NOT_RUN |
| QA-14 | SESUAI | P/W | NOT_RUN |
| QA-15 | SESUAI | E/P/W | NOT_RUN |
| QA-16 | SESUAI | P/W | NOT_RUN |
| QA-17 | SESUAI | P/W | NOT_RUN |
| QA-18 | SESUAI | E/P | NOT_RUN |
| QA-19 | SESUAI | E/P | NOT_RUN |
| QA-20 | REVISI | P/W | NOT_RUN |
| QA-21 | SESUAI | P/W | NOT_RUN |
| QA-22 | SESUAI | P/W | NOT_RUN |
| QA-23 | SESUAI | P/W | NOT_RUN |
| QA-24 | REVISI | P/W | NOT_RUN |
| QA-25 | REVISI | E/P/W | NOT_RUN |
| QA-26 | SESUAI | G/W | NOT_RUN |
| QA-27 | REVISI | E/P | NOT_RUN |
| QA-28 | SESUAI | E/P | NOT_RUN |
| QA-29 | SESUAI | P/W | NOT_RUN |
| QA-30 | SESUAI | E/P/W | NOT_RUN |
| QA-31 | SESUAI | P/W | NOT_RUN |
| QA-32 | REVISI | P/W | NOT_RUN |
| QA-33 | SESUAI | E/P/W | NOT_RUN |
| QA-34 | SESUAI | P | NOT_RUN |
| QA-35 | SESUAI | P | NOT_RUN |
| QA-36 | SESUAI | P | NOT_RUN |
| QA-37 | REVISI | P/W | NOT_RUN |
| QA-38 | SESUAI | W | NOT_RUN |
| QA-39 | REVISI | P/W | NOT_RUN |
| QA-40 | SESUAI | E/P/W | NOT_RUN |
| QA-41 | SESUAI | E/P | NOT_RUN |
| QA-42 | SESUAI | E/P | NOT_RUN |
| QA-43 | SESUAI | E/P | NOT_RUN |
| QA-44 | REVISI | E/P | NOT_RUN |
| QA-45 | REVISI | P/W | NOT_RUN |
| QA-46 | REVISI | P/W | NOT_RUN |
| QA-47 | SESUAI | E/P/W | NOT_RUN |
| QA-48 | REVISI | P/W | NOT_RUN |
| QA-49 | REVISI | P/W | NOT_RUN |
| QA-50 | SESUAI | E/P | NOT_RUN |
| QA-51 | SESUAI | P/W | NOT_RUN |
| QA-52 | REVISI | P/W | NOT_RUN |
| QA-53 | SESUAI | E/P/W | NOT_RUN |
| QA-54 | SESUAI | E/P | NOT_RUN |
| QA-55 | REVISI | P/W | NOT_RUN |
| QA-56 | SESUAI | P/W | NOT_RUN |
| QA-57 | SESUAI | W | NOT_RUN |
| QA-58 | SESUAI | E/P/W | NOT_RUN |
| QA-59 | REVISI | E/P/W | NOT_RUN |
| QA-60 | SESUAI | E/P | NOT_RUN |
| QA-61 | SESUAI | P/W | NOT_RUN |
| QA-62 | SESUAI | P/W | NOT_RUN |
| QA-63 | REVISI | P/W | NOT_RUN |
| QA-64 | REVISI | P | NOT_RUN |
| QA-65 | SESUAI | E/P | NOT_RUN |
| QA-66 | SESUAI | E/P/W | NOT_RUN |
| QA-67 | SESUAI | W | NOT_RUN |
| QA-68 | SESUAI | P/W | NOT_RUN |
| QA-69 | SESUAI | E/P/W | NOT_RUN |
| QA-70 | SESUAI | E/P | NOT_RUN |
| QA-71 | SESUAI | E/P/W | NOT_RUN |
| QA-72 | SESUAI | P/W | NOT_RUN |
| QA-73 | SESUAI | E/P | NOT_RUN |
| QA-74 | REVISI | E/P/W | NOT_RUN |
| QA-75 | SESUAI | A/P/W | NOT_RUN |
| QA-76 | SESUAI | E/P | NOT_RUN |
| QA-77 | SESUAI | A/P/W | NOT_RUN |
| QA-78 | REVISI | P/W | NOT_RUN |
| QA-79 | SESUAI | E/P/W | NOT_RUN |
| QA-80 | REVISI | P/W | NOT_RUN |
| QA-81 | REVISI | E/P/W | NOT_RUN |
| QA-82 | SESUAI | E/P/W | NOT_RUN |
| QA-83 | REVISI | P/W | NOT_RUN |
| QA-84 | SESUAI | P/W | NOT_RUN |
| QA-85 | REVISI | P/W | NOT_RUN |
| QA-86 | REVISI | P/W | NOT_RUN |
| QA-87 | REVISI | E/P | NOT_RUN |
| QA-88 | SESUAI | E/P | NOT_RUN |
| QA-89 | REVISI | E/P/W | NOT_RUN |
| QA-90 | SESUAI | E/P/W | NOT_RUN |
| QA-91 | REVISI | E/A/W | NOT_RUN |
| QA-92 | SESUAI | P/W | NOT_RUN |
| QA-93 | REVISI | E/P/W | NOT_RUN |
| QA-94 | SESUAI | E/P/W | NOT_RUN |
| QA-95 | REVISI | P/W | NOT_RUN |
| QA-96 | SESUAI | E/P/W | NOT_RUN |
| QA-97 | SESUAI | E/P | NOT_RUN |
| QA-98 | REVISI | A/W | NOT_RUN |
| QA-99 | REVISI | G/W | NOT_RUN |
| QA-100 | REVISI | W | NOT_RUN |

### 15.4 Coverage tambahan yang tidak boleh diklaim sudah tertutup oleh 100 label

Subkasus wajib: Continue HUB/CP1..7 dan chapter asing; migrasi 1/2/future serta primary/.tmp/.bak korup; enam permutasi bukti dan dua urutan ruang L2; upgrade bila forum/menu dilewati; checkpoint CP4 sebelum CP2 dan setelah CP6; save-failure crossing pada kapsul di aperture; false/real target pada kedua sisi Selfie mirrored; source cahaya overlap; 20 retry termasuk powerbank/flare RAM dan callback pembayaran/dokumen/donasi selama barrier; charge saat layar off dan attempt gagal; teacher window vs modal spam; pause/focus + remap pada semua state; secret diambil sesudah CP6 tanpa menimpa epilog; video belum selesai + zero battery; power recovery setelah banyak miss; scale/near-clip/culling avatar di FP/OTS/crouch, ultrawide dan preset low. Ini subkasus QA terkait, bukan klaim bahwa jumlah 100 membuktikan seluruh kombinasi lolos.

**Gerbang selesai audit:** semua 100 ID unik, referensi section/ID/path terverifikasi, JSON contoh dapat diparse (tetap parsial/tidak usable loader), kontrak global tidak diturunkan diam-diam, dan diff hanya dokumen plus Graphify yang diwajibkan proyek. **Gerbang implementasi/rilis:** izin coding terpisah, seluruh tes/provenance/artistik/performa nyata; tidak diselesaikan oleh laporan ini.
