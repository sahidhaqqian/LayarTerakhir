# PHASE 3 — NYANYIAN DARI KETEL UAP

> **Project:** Layar Terakhir · **Babak:** 3 · **Lokasi:** Pabrik Gula Belanda "Sindang Laut" · **Versi:** 2.3-REPO-IMPLEMENTATION, konsolidasi `Phase3.md` v2.1 + telaah Argon tiga bagian (24 Oktober 2026); diselaraskan dengan `Phase1 (1).MD` v2.2-r1 dan `Phase2(1).md` v2.1-MERGED
> **Jenis dokumen:** *Single source of truth* untuk naskah, level design, mekanik, implementasi Unity, penyimpanan, dan QA Babak 3.
> **Status:** **Candidate implementasi tersedia: main route native sampai CP7 lulus; belum penerimaan kualitas final.** Arahan desainer 24 Oktober 2026 mengizinkan implementasi dan penyesuaian kontradiksi. Hasil aktual per acceptance case ada di [Audit/phase3-qa-matrix.md](Audit/phase3-qa-matrix.md), bukti dan batas pengujian pada [laporan implementasi](Audit/phase3-implementation.md). Daftar 146 QA di §10 adalah spesifikasi; label historis NOT_RUN di naskah tidak menggantikan matriks hasil tersebut. Kode/scene, render fixture, uji native, dan penerimaan Windows dibedakan.
> **Rujukan lintas babak:** `Phase1.MD` (kontrak global §0.4–§0.5, §8.0), `Phase2.md` (kontrak §0.4 dan §8.0 yang diwarisi) dan `Rancangan.MD` (parameter global dan kontinuitas cerita). Bila ketentuan global belum diketahui, jangan membuat asumsi diam-diam; catat sebagai keputusan terbuka (§11.2).

## 0. Cara membaca dan batas keputusan

Dokumen ini menggabungkan **naskah asli Babak 3 (revisi 15 Oktober 2026)**, acuan mekanik dan jalur rahasia dari file lama, serta kontrak implementasi yang sudah mapan di Phase 1 dan Phase 2. File `Phase3.MD` lama hanya berisi naskah lima adegan, ringkasan acuan, satu item rahasia, dan tujuh baris kriteria selesai; ia **tidak** memiliki stage machine, registry event, state machine AI, skema save, checkpoint, parameter terpusat, matriks QA, atau peta integrasi. Seluruh bagian itu ditambahkan di sini. Dialog, lokasi, urutan lima adegan, angka cerita (tahun 1912 dan 1999, 9 anak, tiga tumbal, 185.000 dan 666.000 penonton, donasi Rp3.333.333 dan Rp9.999.999, total Rp312.500.000), ID item rahasia, dan mekanik inti dipertahankan. Bagian yang secara logis kontradiktif, ambigu, atau tidak dapat dikode diperbaiki dan **ditandai** (daftar lengkap koreksi editorial ada di §0.7). Untuk membedakan sumber dari penambahan:

- **[KANON]** = ketentuan yang sudah ada pada naskah 15 Oktober atau pada kontrak global Phase 1/Phase 2, dipakai sebagai acuan resmi babak ini.
- **[KLARIFIKASI]** = penjabaran implementasi untuk menghindari interpretasi berbeda/softlock, tanpa mengubah alur kanon.
- **[USUL BARU]** = tambahan desain, UX, produksi, atau QA; boleh diterima, disetel, atau dihapus sebelum produksi.
- **[PERLU KEPUTUSAN]** = masih memerlukan putusan penulis/desainer atau verifikasi `Rancangan.MD`. Diberi ID `P3-Dxx` (§11.2) agar tidak tertukar dengan D01–D09 pada Phase 1 atau `P2-Dxx` pada Phase 2.
- **[BASELINE UJI]** = nilai prototipe agar bisa dikode dan playtest, bukan kanon final.
- **[REVISI EDITORIAL]** = perbaikan teks lore/puzzle/urutan untuk menyambungkan kontradiksi sumber.
- **[DIWARISI P1/P2]** = ketentuan global yang sudah ditetapkan Phase 1 atau Phase 2 dan berlaku sama di Babak 3 sampai desainer mengubahnya.

**Prioritas bila terjadi benturan:** keputusan eksplisit penulis dalam `Rancangan.MD` dan dokumen desain terbaru > kontrak global Phase 1 §0.4–§0.5 dan Phase 2 §0.4 > babak kanon di sini > rekomendasi teknis di sini. Bagian *Naskah Adegan* mempertahankan ucapan/kejadian aslinya; pelaksanaan logikanya mengikuti aturan terperinci pada *Kontrak Implementasi*.

### 0.1 Ringkasan babak (KANON)

| Elemen | Ketentuan |
|---|---|
| Judul | Babak 3 — **Nyanyian dari Ketel Uap** |
| Lokasi | **Pabrik Gula Belanda "Sindang Laut"**: Halaman Timbangan Tebu dan Pos Timbang, Gudang Gilingan Utama (panggung kayu dan *catwalk* 8 m), Bangsal Ketel Uap dua lantai (Kantor Mandor di L2, Gudang Ampas Tebu di sudut barat L1), Aula Tungku Utama |
| Waktu pembuka | **15:30 WIB** di Kamar Kos (Safe Hub); berangkat **23:45 WIB**; tiba di halaman pabrik **23:54 WIB** |
| Panjang jalur utama | **75 menit**, tidak termasuk pencarian item rahasia dan pengulangan setelah kalah |
| Karakter | **Raka**; Damar hadir lewat **suara tiruan** (jebakan), kartu pers, sepatu kiri, tulisan arang, dan video tungku Babak 2; **Mandor 1999** (dokumen), **Mbah Suro** (catatan, tawa di akhir) dan **Wewe Gombel/Mbok Gombel** terkait misteri; `Penonton_Terakhir` sebagai antagonis siaran |
| Boss | **Wewe Gombel** (*Ibu Asuh Semu*, dipanggil **Mbok Gombel** oleh Mandor 1999) |
| Ancaman pendukung | Wewe sendiri dalam tiga mode (umpan suara tiruan di gilingan, patroli stealth di bangsal, boss di aula) dan **jebakan lantai runtuh** (rapuh, bertanda rajah silang ungu). **Tidak ada Tuyul/Genderuwo/Kuntilanak/ancaman baru lain** di babak ini tanpa keputusan (§11.2) |
| Modul HP | **Lensa UV Rajah** dan **EVP Recorder v2.0** (baru, terbuka otomatis); AR penuh, Jurnal Klenik, Blitz sejak Babak 1; **Flare Industri** sebagai item lempar |
| Bukti wajib | **Jejak gendongan tumbal** (AR, ayunan kain batik); **EVP nina bobo** (interkom Kantor Mandor); **rajah UV penyembunyi anak** (dinding Gudang Ampas Tebu) |
| Pusaka | **Boneka Jerami Tebu** (ayunan gendongan), **Sabit Tebu Berukir Rajah** (meja kontrol Kantor Mandor), **Selendang Batik Terbakar** (Gudang Ampas Tebu) |
| Boss fight | Pasang tiga pusaka di dudukan altar tungku, periksa **Karung Damar Palsu** sebagai pemicu; **3 siklus**: navigasi lantai rapuh dengan UV → buka **Perisai Kabut Hitam** dengan **generator lampu sorot** (Utara/Selatan/Timur) atau **flare** → jendela **5 detik** → **Blitz** valid; **3 Blitz berhasil** menyegel Wewe |
| Rahasia | **Pelat Nomor Buruh 09**, ID `SECRET_P3_PELAT_BURUH`, bagian **Arsip Saksi N dari 5** (hitungan aktual ID unik; bukan nomor babak) |
| Hasil | **Kartu pers dan sepatu kiri Damar**, **segel ketiga**, menuju **Desa Mati "Karang Setra"**; chat palsu mulai menyusup |

### 0.2 Alur level dan sasaran waktu (KANON + BASELINE UJI)

| Adegan | Target | Area dan tindakan utama | Keluaran/gate |
|---|---:|---|---|
| 1 | 00–10 menit | Kamar Kos: riset forum di PC, Pasar Gaib, upgrade modul, berangkat | UV Rajah + EVP v2 terbuka; berangkat 23:45 |
| 2 | 10–28 menit | Halaman Timbangan, live stream, Gudang Gilingan, jebakan suara #1, Generator Akses #1, ayunan gendongan | **Bukti 1** + **Pusaka 1** + **Dokumen #1**; gerbang magnetik Bangsal terbuka |
| 3 | 28–52 menit | Bangsal Ketel Uap: Wewe stealth, Kantor Mandor (L2), Gudang Ampas Tebu, Jurnal Deduksi | **Bukti 2 + 3**, **Pusaka 2 + 3**, **Dokumen #2** + konfirmasi Jurnal; rahasia opsional (balkon servis) |
| 4 | 52–68 menit | Aula Tungku Utama: dudukan altar, Karung Damar Palsu, tiga siklus penyegelan | **3 Blitz segel** → Wewe tersegel |
| 5 | 68–75 menit | Epilog: chat 666.000, Rp312,5 juta, infiltrasi chat, pesan `Penonton_Terakhir` → kembali ke motor → keluar | Panel Arsip → konfirmasi `P3_COMPLETE` → save + `CKREK!` + hitam |

**Jalur kritis:** Kos/PC/Pasar Gaib → berangkat → halaman timbangan (live, chat #1) → Gudang Gilingan (jebakan suara #1, UV, jalur pipa kanan) → Generator Akses #1 (engkol ×3) → ayunan gendongan (AR, Boneka Jerami, Catatan Mandor) → Bangsal Ketel (Wewe reveal) → Kantor Mandor (EVP interkom, Sabit) → Gudang Ampas Tebu (UV rajah, Selendang, Kartu Pers + Sepatu) → konfirmasi Jurnal → gerbang aula → dudukan altar (3 pusaka) → Karung Damar Palsu (CP5) → tiga siklus → epilog → pulang via gerbang servis aula → halaman → motor → `P3_COMPLETE`.

**[BASELINE UJI — alokasi menit bukan pengunci progres]** Adegan 1 (10 menit) memuat PC, forum, belanja, dan tutorial modul; pemain yang cepat dapat selesai dalam 5–6 menit dan itu **bukan bug**. Adegan 3 (24 menit) diisi pengamatan patroli, pembacaan gelombang EVP, pembacaan lantai dengan UV, dan penghindaran Wewe, bukan filler. Tinjau ulang distribusi setelah playtest bertanda waktu (§3.4A).

**[KLARIFIKASI — perjalanan pulang]** Setelah `P3_BOSS_WIN`, Wewe berstatus `RESOLVED` permanen; gerbang servis aula (dilas sejak 1999, kini terbuka karena Perisai Kabut runtuh) memberi jalur pendek aula → lorong lori → halaman timbangan. Interaksi **`[E] Tinggalkan Pabrik Gula`** hanya ada pada **motor Raka di halaman timbangan**. Perjalanan keluar ditargetkan sekitar **60–120 detik** setelah epilog untuk pemain yang tidak mencari rahasia; target 75 menit adalah sasaran playtest, bukan batas waktu paksa.

### 0.3 Pilar pengalaman (KLARIFIKASI)

1. **Investigasi dan pembacaan lebih penting daripada combat:** Wewe tidak dapat disegel sebelum tiga bukti, konfirmasi Jurnal, dan tiga pusaka lengkap; Blitz sembarangan tidak berarti apa pun.
2. **Horor yang membalik kepercayaan pada telinga dan lantai:** ancaman utama tidak membunuh lewat taring, melainkan lewat **suara yang meminta tolong** dan **lantai yang tampak aman**. Pemain belajar memercayai grafik EVP dan sinar UV, bukan naluri. Aturan "jangan langsung berlari ke suara" harus diajarkan dulu, baru dihukum.
3. **Ponsel sebagai alat sekaligus beban:** UV dan EVP menyelesaikan puzzle dan menyelamatkan nyawa, tetapi setiap mode memakai baterai dan layar yang menyala menarik perhatian Wewe.
4. **Ketegangan dengan aturan adil:** setiap hukuman fatal punya telegraph audio/visual; hanya tiga jenis kegagalan fatal: `FALL` (menginjak papan rapuh bertanda silang di atas gilingan atau bara), `CLOTH_GRAB` (tertangkap kain gendongan saat patroli atau terlalu dekat di luar jendela exposure) dan `PULL_IN` (tertarik sampai tepi tungku pada siklus 3); kematian tidak menghapus progres permanen.
5. **Jalur rahasia menghargai rasa ingin tahu, bukan uang:** Arsip Saksi tidak dijual lewat saweran; semua progres utama dapat diselesaikan tanpa donasi atau belanja opsional.
6. **Tragedi manusia di balik hantu:** sembilan nama anak buruh pada pelat nomor, catatan mandor yang menanam tumbal, dan kain gendongan yang menyembunyikan anak membuat Wewe terasa sebagai ibu asuh yang disalahgunakan ritual, bukan monster acak.

---

### 0.4 Kontrak lintas babak yang mengikat Babak 3 [DIWARISI P1/P2]

Bagian ini menyalin ketentuan global dari Phase 1 §0.4–§0.5 dan Phase 2 §0.4 yang secara langsung menentukan P3, ditambah perluasannya. **Penyusun draf ini tidak memiliki akses ke repositori maupun `Rancangan.MD`**; setiap baris di bawah menandai asalnya dan harus dicocokkan dengan sumber aslinya pada audit r1 P3.

| Kontrak bersama | Ketentuan untuk Babak 3 |
|---|---|
| Prasyarat masuk | Babak 3 hanya dapat dimulai dari save yang memuat `P2_COMPLETE` dan `SEAL_RSUD_LAMA_SIDO_WARAS` (serta `SEAL_RUSUNAWA_GRIYA_ASRI` dari P1). Build pengembangan boleh menyediakan jalur uji *tanpa* P1/P2, tetapi harus menandainya **dev-only** dan tidak menciptakan segel/uang/inventori palsu di save produksi. |
| Kamera dan kontrol | Pertahankan desain pemain terbaru: OTS + first-person melalui `ShoulderCameraRig`; binding mengikuti `GameInputConfig`. **`V` = ganti perspektif, `X` = Selfie.** **Tahan RMB = angkat kamera fisik horizontal dua tangan; lepas = kembalikan pose/perspektif terdahulu** (P1 §0.7). **`R` = Lensa UV Rajah, `G` = Flare** sudah dideklarasikan pada `GameInputConfig` menurut P1 §2.1/P2 §2.1; verifikasi `R` tidak berbenturan dengan aksi lain pada audit. Prompt UI dihasilkan dari binding aktual, bukan di-*hardcode*. |
| Energi HP global | Kamera normal **0,05 poin persen/detik**, AR/UV **0,10 total/detik**, foto bukti **1%**, Blitz **15%**, cooldown **6 detik**, powerbank besar **+80 poin**, kecil kos **+35**. Overlay tidak menumpuk drain: profil mode aktif saja. **UV Rajah memakai profil AR/UV global 0,10 total**; **EVP v2** tidak menambah drain tersembunyi (P3-D05). HP disimpan/layar mati menghentikan drain dan tidak memutus live. **Flare tidak memakai baterai.** |
| Baterai nol | Default global: layar mati, AR/UV/EVP/Blitz tidak berfungsi, bukan Game Over. **Tidak ada `WATCH_ONLY`** di P3. Pemulihan lewat dinamo/titik daya fisik (§8.0), bukan energi otomatis. |
| Boss dan energi | Blitz 15%/6 detik. Sumber cahaya ritual (generator lampu sorot) tidak habis kecuali **setelah hit sukses**; flare habis **setelah dilempar**. **Titik daya darurat arena** fisik tersedia saat jeda pemulihan; tidak ada Blitz bertenaga lampu pada 0%. |
| Ritual dan Jurnal | **3 bukti unik + konfirmasi eksplisit Jurnal + 3 pusaka** sebelum damage penyegelan/ritual. Tiga scan tidak otomatis menjadi konfirmasi. `P3_JOURNAL_CONFIRMED` adalah receipt satu kali. |
| Sanity | **Sanity gameplay mulai Babak 4** (`Rancangan.MD`). P3 mempertahankan nilai Sanity yang diwariskan; **tidak menguras, meregenerasi, atau mengubah radius/efek berdasar Sanity**. `Kemenyan Bakar` hanya dibeli dan disimpan; **tidak punya fungsi di P3**. |
| Kanon lokasi | Lima gerbang = Rusunawa, RSUD, **Pabrik Gula**, Karang Setra, Pasar Setan; **empat segel lokasi dari Babak 1–4**. P3 menambah segel **ketiga**. Rumah Atmaja bukan gerbang (lihat koreksi peta E1). Pesan penutup "Tiga segel lokasi telah terkumpul" konsisten. |
| Arsip Saksi | ID P3 **`SECRET_P3_PELAT_BURUH`**; ID unik dalam campaign, autosave atomik **sebelum** indikator sukses, tetap ada saat kematian/reload checkpoint terkini. Hitungan `N/5` memakai **jumlah aktual ID unik** (1/5 bila hanya satu rahasia terdahulu, 3/5 bila P1 dan P2 sudah diambil). |
| Uang | Rp312.500.000 adalah **milestone naratif kumulatif**, terpisah dari saldo (pola Rp78,2 juta di P1 dan Rp164,8 juta di P2). Donasi `Penonton_Terakhir` (Rp3.333.333 dan Rp9.999.999) adalah **dana terikat** dan tidak masuk saldo belanja; hanya donasi manusia yang sah masuk `wallet_legitimate`. Jangan menambah nominal milestone ke wallet untuk menutup selisih (§5.3). |
| Save dan event | Perluas **`CampaignState/CampaignData` yang sama**, file `.json/.tmp/.bak` yang sama; list ID unik; transaksi atomik dengan rollback. Tidak ada save/event/AR/inventori paralel. Versi schema harus mengikuti loader **aktual**, bukan memaksa usulan 4 (P3-D10). |
| Aset | **Aset wajib existing atau unduhan internet berlisensi.** Tidak membuat model, gambar, tekstur, atau gerak original sendiri; konversi/optimasi impor dan retargeting animasi existing diperbolehkan. Sumber/lisensi dicatat (§7.5). |
| Target performa | 60 FPS global tetap sasaran; tier minimum 720p Low 30 FPS pada Iris Xe adalah usulan D07 Phase 1 yang belum diratifikasi; P3 mengikutinya (P3-D11). |

**Pagar keputusan:** Ikuti kontrak global/default di atas. P3-D01 sampai P3-D17 (§11.2) dan keputusan Phase 1/Phase 2 yang berlaku global (D03 ledger, D05 kaset, D06 carry-over, D07 tier performa, D08 retry boss) harus tercatat sebelum freeze bagian terkait. Mekanik inti cerita tetap; angka prototipe boleh diuji setelah milestone implementasi diizinkan. Jangan membaca label "siap blockout" sebagai izin coding.

### 0.5 Keputusan implementasi dari desainer [DIWARISI P1/P2 — perlu konfirmasi untuk P3]

**Arahan terbaru Phase 1 (23 Oktober 2026):** "tahan dulu sementara pakai global dulu aja nanti kalau gak sesuai aku ubah". Arahan itu ditulis untuk Phase 1 dan **dianggap berlaku juga untuk P2**; draf ini menganggapnya berlaku juga untuk P3 sampai desainer menyatakan lain (). Konsekuensinya:

- Pakai **default global** untuk energi, ekonomi, Sanity, dan retry; angka P3 di §8.0 adalah baseline yang mudah diubah di satu konfigurasi.
- Tidak mengurangi kualitas visual, atmosfer, atau gameplay demi mengejar checklist.
- Retry boss memuat **snapshot CP5** (baterai persis snapshot, tiga generator utuh, flare sesuai inventori snapshot, tiga pusaka belum aktif); CP6 disimpan atomik setelah kemenangan; semua penulisan biasa ditolak selama attempt.
- Status pembayaran/ekonomi mengikuti receipt save; tidak dilunaskan atau diisi diam-diam untuk menyesuaikan monolog atau overlay.
- Nubuat kaset 1998 masih ditahan (D05); P3 tidak memutar kaset dan tidak menambah klaim nubuat. Catatan Mandor 1999 dan catatan Damar tidak boleh dibaca sebagai pengesahan nubuat.
- Babak berakhir pada konfirmasi motor dan CP7; simpan tujuan **Desa Mati Karang Setra** tanpa membuat scene Babak 4. Video/rekaman yang dipakai epilog (bila ada) memakai set non-playable di dalam scene P3.
- **Aset wajib existing atau unduhan internet berlisensi**; Phase 3 memakai pustaka lingkungan pabrik/industri tersendiri (bukan furnitur/permukaan Rusunawa atau RSUD), UV skala nyata, tekstur PBR 2K, dan dressing sesuai fungsi ruang. Sistem pemain/HP, karakter desainer, dan motor yang dibawa dari babak sebelumnya tetap digunakan.
- Target 60 FPS global tetap dipertahankan. Headless/Linux software rendering tidak menggantikan pengukuran Windows pada GPU sasaran.

### 0.6 Status dokumen — produksi dan validasi

Kontrak telah diperiksa terhadap `Core/CampaignState.cs`, `Phase2/Phase2Campaign.cs`, `Core/Phase1Data.cs`, `Smartphone/SmartphoneController.cs`, `Prolog/AtmajaPhoneView.cs`, dan `UI/GameShell.cs` pada basis `a8b1340`. Implementasi P3 sedang dibangun; status draf historis di bawah tidak menjadi bukti runtime. Daftar QA §10 adalah acceptance, **bukan daftar yang sudah lulus**. Uji aturan/editor, native smoke, playtest manusia, kualitas visual/audio akhir, pacing 75 menit, dan pengukuran GPU Windows mempunyai tingkat bukti berbeda; versi implementasi wajib menyebut keterbatasan tersebut. Urutan kerja engineering ada di §14.3; nama folder/class di §9 dan §14 adalah **peran usulan**, bukan klaim sudah ada.

### 0.7 Koreksi editorial atas naskah 15 Oktober 2026 [REVISI EDITORIAL]

Setiap baris di bawah mengubah, memperjelas, atau memindahkan sesuatu dari naskah asli. Dialog dan angka cerita tidak diubah kecuali tertulis di sini. **Periksa daftar ini lebih dulu sebelum menetapkan kanon final.**

| # | Naskah 15 Oktober | Masalah | Perlakuan di dokumen ini |
|---|---|---|---|
| E1 | Peta di dinding kos "menghubungkan Rumah Atmaja, Rusunawa, RSUD, dan **titik keempat**: Pabrik Gula" | Kanon global: lima gerbang = Rusunawa, RSUD, Pabrik Gula, Karang Setra, Pasar Setan; Rumah Atmaja **bukan** gerbang; Pabrik Gula adalah gerbang **ketiga** | Teks peta diubah: Rumah Atmaja titik asal petunjuk, sedangkan Rusunawa, RSUD dan Pabrik Gula adalah gerbang 1–3; dua ujung masih kosong. Ini klarifikasi global, bukan harga/keputusan `P3-D04`. |
| E2 | Tombol `[GO LIVE]` | P1 memakai `[MULAI LIVE]` | Label UI mengikuti kontrak P1/P2; label tidak di-*hardcode*, diturunkan dari binding dan lokalisasi aktual. |
| E3 | Adegan 5 langsung `CKREK!` → hitam setelah pesan | Pemain yang belum mengambil Pelat Nomor Buruh kehilangan rahasia tanpa peringatan; berbeda dengan pola P1 §8.4 dan P2 E10 | Epilog mengembalikan kontrol tanpa fade; `CKREK!` + hitam dipindah ke **konfirmasi `Lanjutkan` di motor**. Audio/visual `CKREK!` tidak diubah. **P3-D12** |
| E4 | Acuan boss: "kelima pilar mekanik" | Naskah hanya menyebut tiga langkah | Dirumuskan sebagai **lima unsur**: (1) UV baca lantai, (2) EVP bedakan suara, (3) generator/flare buka perisai, (4) Blitz segel, (5) pusaka dan jebakan Karung Damar Palsu |
| E5 | **Boneka Jerami Tebu** (pusaka) dan "boneka Damar" (pemicu, karung goni berisi jerami) | Dua benda memakai kata "boneka" | Pusaka tetap **Boneka Jerami Tebu** (boneka ritual kecil ±25 cm). Pemicu diberi nama tersendiri: **Karung Damar Palsu** (ID `P3_DAMAR_DECOY`). Dialog tidak diubah |
| E6 | Acuan boss: "generator **atau** flare"; adegan terakhir menyatakan flare "terakhir" harus dilempar | Fase 3 semula berisiko mewajibkan flare sekalipun dapat dihabiskan sebelum arena; logika 3 sumber sukses juga ambigu | **Tiga generator G-N/G-S/G-E cukup tanpa flare**, masing-masing dipakai satu kali *setelah Blitz sukses*; flare opsi tambahan dan bisa menjadi visual klimaks. Serangan kain siklus 3 memiliki **counter UV simpul / sinar generator terakhir**, selain flare. Tidak ada flare yang wajib dipungut atau dibeli. Lihat §4.4 dan `P3-D13` [REVISI EDITORIAL]. |
| E7 | "Generator Diesel Tua #1" dan "3 Generator Lampu Sorot Pabrik" | Dua jenis generator berbeda | **Generator Akses** (`G-ACC`, engkol, hanya buka kunci magnetik) berbeda dari tiga **generator arena** (`G-N`, `G-S`, `G-E`, tuas, lampu sorot) dan dari **dinamo engkol HP** eksplorasi (§8.0). Sesuai acuan "generator akses berbeda dari tiga generator arena" |
| E8 | Viewer awal **185.000** setelah P2 berakhir pada **310.500** | Tampak turun | Ditafsirkan sebagai penonton serentak saat siaran baru dibuka, bukan puncak kumulatif (pola P2 E8). Angka tidak diubah; ini jumlah penonton serentak pada siaran baru, bukan jumlah pengikut kumulatif. |
| E9 | Forum: 9 anak hilang; Catatan Mandor: **tiga** tumbal; Pelat: tiga korban awal + enam berikutnya | Tiga angka berbeda | Dijembatani: **3 tumbal ritual** ditanam di bawah tungku, **6 anak lain** disembunyikan Mbok Gombel dalam kepompong; total **9**. Pelat menamai kesembilannya |
| E10 | Item rahasia "Arsip Saksi **3/5**" | Hitungan tergantung rahasia P1/P2 sudah diambil atau belum | **N/5 dari ID unik aktual**; contoh 1/5, 2/5, atau 3/5 (§5.2, pola P2 E6) |
| E11 | Lokasi rahasia: "sebelum pintu aula tungku dikunci" | Bisa dibaca batas waktu (hilang setelah dikunci) | Dibaca sebagai **urutan rute**: balkon servis berada sebelum gerbang aula. Aula hanya terkunci **selama attempt boss**; balkon di luar aula tetap dapat dikunjungi setelah boss (jalur pulang) |
| E12 | Upgrade UV Rajah dan EVP v2 tercantum di menu "belanja" Toko Klenik; epilog P2 menyuruh "membeli Lensa UV Rajah" | Jalur wajib tidak boleh bergantung uang | Dua upgrade **tidak dijual**: terbuka otomatis (pola P2). Kata "membeli" di pesan P2 dibaca sebagai "membuka". **P3-D01** |
| E13 | Interkom menyebut "rintihan arwah" dan bukti "Nina Bobo Sengkala" | Tidak jelas suara Wewe tiruan atau kesaksian asli | **Pola audio ganda:** nina bobo `TIRUAN` (merah) diikuti rintihan anak `ASLI` (hijau), hanya segmen hijau yang memenuhi bukti; nama bukti tetap "Frekuensi EVP Nina Bobo Sengkala" sesuai sumber. Nilai durasi = baseline uji. |
| E14 | Jebakan lantai: "jebakan lantai kayu/besi yang keropos" dan "jurang ilusi" | Dua jenis kegagalan dan arti "ilusi" tidak terdefinisi | **Rajah silang ungu = rapuh (bahaya)**. Papan tanpa tanda = kokoh. Ilusi lubang hanya hiasan visual di atas lantai kokoh, tidak pernah menjadi satu-satunya jalur wajib (§3.2A). **P3-D07** |
| E15 | Naskah tidak menyebut kerugian jatuh di Bangsal Ketel | Kematian dari setiap lubang saluran uap terlalu keras | Jatuh di gilingan dan aula **fatal** (kanon); celah/lubang saluran di bangsal **tidak fatal**: jatuh ke lantai bawah + stimulus bising (§3.2A, §8.2) |
| E16 | Chat infiltrasi menyuruh "loncat ke tungku" / "loncat ke bawah" | Pemain bisa mencoba; tidak boleh ada kematian tersembunyi | Chat adalah **narasi saja**; setelah `P3_BOSS_WIN` tidak ada kill plane di tungku/jembatan aula; mencoba tidak menghasilkan Game Over (QA-82) |
| E17 | Kantor Mandor (L2) "melompati 2 celah lantai lapuk" | Tidak jelas apakah celah wajib dilompati atau dihindari | Dua celah **terlihat** di tangga/jalur; dapat dilompati (`Space`) atau dilewati lewat jalur samping yang kokoh; UV menandai papan rapuh di sekelilingnya (§3.4) |
| E18 | Kos tidak disebut ukurannya | Pola P2 E18 | Gunakan ulang **scene Kos aktual** (Golden Scene) tanpa membuat prefab baru; verifikasi ukuran saat audit. **P3-D01** |
| E19 | EVP `ASLI` pada tabel dapat dibaca sebagai rute yang pasti aman | Rekaman otentik bukan jaminan pijakan aman | Warna hijau menandai **asal suara yang autentik**, bukan keselamatan lokasi; tetap gunakan UV dan kondisi fisik lantai (§4.2). |
| E20 | CP3 dipandang mensyaratkan bukti, pusaka, dan dokumen dari ayunan | Ketiga pickup terpisah dan pemain boleh masuk Bangsal lebih dulu | CP3 adalah **anchor aman saat memasuki Bangsal**, menerima keadaan inventory aktual; kembali ke ayunan tetap dapat dilakukan (§2.2, §6.3). |
| E21 | P3 direncanakan menggunakan `schema_version: 4` | Versi loader aktual belum diperiksa; P2 juga masih dokumen draf | JSON hanya contoh; **migrasi dari versi yang terverifikasi di repo**, jangan memaksa angka literal (§6.2). |
| E22 | Daya arena terisi kembali per fase saja | Setelah cukup banyak miss, semua jatah dapat habis dan pemain pada 0% | Tambah **stasiun pemulihan fisik manual yang bisa di-reset saat recovery**, tanpa refill otomatis dan tidak bisa dipakai untuk cheese Blitz (§4.4, §8.1B). |
| E23 | Epilog Raka memegang sepatu Damar tetapi pickup kartu/sepatu terpisah dari gate boss | Pemain bisa menuntaskan ritual tanpa mengambil sepatu | Jika sepatu belum diambil, animasi epilog memakai tangan Raka pada pagar; item tetap bisa diambil sebelum keluar (§1 Adegan 5, §11). |
| E24 | Nomor QA Claude 01–100 vs ChatGPT 001–110 | Dua matriks terpisah tidak dapat dipakai sebagai satu oracle | Pada v2.1 disatukan menjadi QA-01–QA-126; audit Argon v2.2 menambah QA-127–QA-146. Semua tetap NOT_RUN sampai terbukti. |

---

### 0.8 Hasil penyusunan dan kontrak gabungan [AUDIT DOKUMEN]

**Yang dipilih dari naskah Phase3 lama:** seluruh naskah per adegan (dialog, angka, nama item, ID rahasia), acuan mekanik (UV, EVP v2, generator, flare, jebakan suara, lantai rapuh) dan kriteria selesai tujuh baris. **Yang dipilih dari Phase 1 dan Phase 2:** struktur §0–§14, label kanon/baseline, stage machine, registry event, tabel prasyarat dan kegagalan, checkpoint CP1–CP7 tanpa CP8, parameter terpusat, tiga dinamo eksplorasi, ledger dana terikat, kontrol RMB/OTS/FP, peta integrasi §14, dan matriks QA 146 kasus. **Yang tidak dibawa:** kamera `WATCH_ONLY` (tidak pernah diratifikasi), Tuyul/Genderuwo/Kuntilanak (bukan ancaman Babak 3), Selfie dan ThermoKlenik sebagai mode wajib (tetap tersedia, tidak dipakai puzzle), dan pengunci pintu arena yang menutup jalan pulang.

**Tiga hal yang tidak boleh bercabang di kode:** (1) **checkpoint CP5 sebelum pemicu aula / CP6 kemenangan / CP7 keluar**, tanpa CP8; (2) **`G-N`, `G-S`, `G-E`** masing-masing korslet **setelah hit sukses**, boleh dipakai ulang bila miss, sehingga flare opsional pada semua siklus; (3) **`V` perspektif, `X` Selfie, RMB tahan untuk HP**, serta **`R` UV, `G` flare**. Seluruh event dan nama parameter bersifat usulan, sedangkan objek/kelas yang sudah ada di repo harus menang setelah audit berbukti file. Jangan membiarkan kata `KANON` pada angka cerita menjustifikasi perubahan kontrak P1/P2 atau keputusan yang belum disetujui.

**Penandaan kanon vs baseline:** angka tahun 1912/1999, 9 anak, tiga tumbal, tiga bukti, tiga pusaka, tiga generator, tiga siklus, jendela 5 detik, jumlah penonton, nominal donasi, dan Rp312,5 juta tetap kanon; harga paket Pasar Gaib, gelombang EVP berpola, kecepatan Wewe, tata letak aula, rak flare, titik dinamo, dan perjalanan keluar lewat motor adalah **klarifikasi/usulan** dengan status masing-masing pada §0.7 dan §11.2. `Rancangan.MD` serta keadaan repo yang terbaru tetap memerlukan audit Codex.

---

### 0.9 Hasil audit Argon — keputusan merger v2.2 (24 Oktober 2026)

**Asal telaah:** tiga berkas `deepseek_markdown_20261024_ea7c3b.md` (bagian §0–§4), `deepseek_markdown_20261024_d1402e.md` (bagian §5–§10), dan `deepseek_markdown_20261024_c5bde7.md` (penutup §10–§14) adalah **satu rancangan Argon terpotong**, bukan tiga audit independen. Dokumen ini mempertahankan naskah lengkap dari v2.1, mengintegrasikan penajaman Argon ke bagian fungsional, dan menambah QA-127 sampai QA-146. **Tidak ada pengujian terhadap repo/Unity yang diklaim sudah berjalan.**

| Temuan Argon | Keputusan gabungan | Status |
|---|---|---|
| 1,0 s derit dan tepi peringatan 0,8 m pada papan gilingan; jalur aman 1,4 m | Tambah *edge warning* pembelajaran dan target lebar aman **≥1,4 m**; jangan mengganti waktu telegraph fatal umum 0,6 s tanpa playtest | **ADOPT/BASELINE** |
| Lubang dinamis tidak boleh muncul di bawah kaki pemain | Spawn papan berbahaya minimal **6 m** dari pemain, setelah memeriksa semua rute wajib aman dan dapat dibaca UV | **ADOPT/BASELINE** |
| Wewe bergerak dari rangka atap ke lantai; loker dan lubang tungku diberi staging konkret | Tambah titik transisi navigasi terotorisasi, *safe investigation interaction window* yang tidak berubah menjadi kekebalan permanen | **ADOPT/CLARIFY** |
| EVP berlabel ASLI/TIRUAN dan pencampuran ambience harus deterministik | Tag otentik/tiruan melekat pada *authored source*, bukan klasifikasi mikrofon nyata. Audio asli tidak menjamin posisi aman; prioritas sumber eksplisit | **ADOPT/CLARIFY** |
| Ketukan ventilasi Loker 09 + panah UV + Katup 09 | Pertahankan puzzle, tambahkan ketukan perlu direkam dan *fail feedback* tanpa mengungkap lokasi rahasia di awal | **ADOPT/CLARIFY** |
| Target **≥30 model prop pabrik unik** berlisensi | Tingkatkan menjadi **kriteria penerimaan visual produksi [TARGET DESAINER — menunggu ratifikasi P3-D16]**; bukan sekadar menumpuk duplikat; manifest URL/lisensi/SHA-256/penempatan | **ADOPT sebagai target mutu** |
| `C` dipakai untuk membuka analyzer sekaligus menahan rekaman | Pisahkan *tap* (toggle analyzer) dan *hold* (rekam) dengan durasi threshold yang dapat ditata ulang agar UI tidak mengunci | **ADOPT/BASELINE** |
| QA Argon berjumlah 100 dan beberapa ID item berbeda dari v2.1 | **Jangan timpa QA-01…126 atau ID save v2.1.** Tambahkan QA baru 127–146 dan alias teks Argon sebagai referensi, bukan ID kedua | **ADOPT terbatas** |
| CP2 setelah Generator Akses; mati pada tutorial kembali ke CP1 | **TOLAK:** CP2 v2.1 berada **sebelum** jebakan gilingan. Ini memberi retry yang lebih adil dan tidak mengulang perjalanan dari halaman | **REJECT** |
| EVP interkom hanya hijau tanpa segmen tiruan | **TOLAK:** tetap gunakan urutan **TIRUAN merah lalu ASLI hijau**, rekaman valid hanya jika segmen asli ikut tertangkap | **REJECT** |
| Flare mem-*flinch* Wewe di fase stealth; gagal generator diberi cooldown tetap 12 s | **TOLAK sebagai kewajiban kanon:** efek flinch belum disahkan; cooldown singkat tetap baseline yang bisa dituning, generator tidak rusak saat miss | **DEFER** |
| Dua rak daya memberikan +50% otomatis setiap putaran atau flare wajib putaran ketiga | **TOLAK:** gunakan pengisian **manual fisik**, jatah bersama per fase, alcove reset untuk miss berulang; tiga generator sendiri cukup tanpa flare | **REJECT** |
| Paksa `schema_version: 4` dan ganti seluruh ID menjadi versi Argon | **TOLAK:** perlu migrasi berdasarkan schema dan ID **aktual di repository**; contoh JSON di sini bukan loader contract | **REJECT** |

**Pagar penting:** Argon bukan sumber kanon yang mengungguli `Rancangan.MD`, keputusan desainer, atau `Phase1`/`Phase2` yang lebih baru. Penambahan teknis [BASELINE UJI] harus dapat dibatalkan tanpa merusak cerita, save, atau 146 acceptance. Lihat §13.4 untuk jejak keputusan dan §11.2 untuk register keputusan yang tetap terbuka.

---

### 0.10 Penyesuaian terhadap repositori aktual — 24 Oktober 2026

Arahan terbaru desainer mengizinkan implementasi seluruh lima adegan dan revisi kontradiksi. Tidak perlu persetujuan ulang untuk baseline teknis yang konsisten. Naskah, jumlah segel dan tujuan Karang Setra dipertahankan.

| Kontrak | Temuan kode / keputusan implementasi |
|---|---|
| READY — campaign | Loader aktual schema **3**, `TryCommit` dengan rollback lengkap dan owner write barrier sudah tersedia. P3 menambah `Phase3Data` pada **CampaignData yang sama**, migrasi **3→4** setelah 1→2→3. P0/P1/P2 dipertahankan, alias Argon dinormalisasi satu kali. Pernyataan lama di §6.4 tentang rollback parsial hanya berlaku pada API lama, bukan `TryCommit` saat ini. |
| ADAPT — checkpoint | Receipt ayunan/boneka/dokumen tetap memakai CP2 atau checkpoint tertinggi yang telah dicapai. CP3 hanya di landing Bangsal. Backtrack tidak menurunkan CP4/5/6; anchor CP5 selalu ALTAR. CP5/6/7 mengikuti write barrier produksi. |
| READY/ADAPT — input dan HP | Binding aktual V perspektif, X Selfie, R UV, C EVP, G flare. UV memakai satu `ScanVisualMode`; layer24 hanya pada feed HP ketika UV aktif. EVP tap/hold di adapter P3, bukan controller input/renderer HP kedua. Jurnal HP tidak menghentikan AI; modal dokumen tetap memakai aturan global; saat dikejar, dokumen diambil/disimpan tanpa membuka modal dan dapat dibaca ulang di Jurnal. |
| ADAPT — kos dan energi | Golden Kos dan sistem Raka/HP/motor dipertahankan sebagai komponen lintas babak. Semua lingkungan, prop, tekstur dan cue lokasi pabrik memakai pustaka unduhan P3 tersendiri. Colokan Kos mengisi100 sekali, manual seperti P2; CP1 menyimpan baterai aktual setelah persiapan, bukan menjamin refill tersembunyi saat retry. |
| BUILD — investigasi dan boss | Gilingan nyata, EVP metadata, UV, tiga bukti/pusaka, ketukan/katup09, Wewe, tiga generator, tarikan kain serta alcove dibangun dalam scene pabrik baru. Tidak ada sistem save/AR/inventori/event paralel. Rute aman memakai pijakan nyata dan dua tingkat aula dengan tangga, bukan lompatan wajib yang melampaui controller global. Baseline tarikan 10m dari tepian tidak muat pada ring; trigger memakai LOS/jangkauan8m, sedangkan fatal memeriksa tepian fisik sesudah telegraph, bukan radius abstrak. |
| ADAPT — carry-over | Powerbank dan flare yang tersisa dari kampanye tetap dapat dipakai melalui inventori existing. Kemenyan belum dapat dikonsumsi; Sanity tidak berubah di P3. Milestone Rp312,5 juta tetap overlay, dua donasi terikat dicatat sekali. |
| ADAPT — lore nama | Sembilan slot korban tetap ada dengan jumlah3+6; nama yang belum disahkan tidak dikarang. Kutipan Damar “kemarin siang” dipertahankan sebagai kesaksiannya. |
| BUILD / belum diterima | Sasaran ≥30 prop industri baru berlisensi dipakai sebagai target produksi. Kualitas visual dinilai melalui screenshot yang benar-benar diperiksa; render software Linux bukan bukti kesetaraan DreadOut, GPU Windows atau 60FPS. VO manusia Indonesia dan acceptance pacing75menit tetap membutuhkan sumber/pengujian yang sesuai. |

Register P3-D01–D17 tetap menyimpan konteks rancangan awal. D01/02/03/04/05/06/09/10/12/13/14/16/17 dioperasikan dengan keputusan teknis tabel ini; D07/08 menunggu hasil geometri/runtime; D11 menunggu perangkat Windows; D15 nama korban tetap terbuka. Seluruh146 QA harus dilaporkan dengan bukti pada `Audit/phase3-qa-matrix.md`; status global bukan sertifikat PASS. Hasil native, fault injection, pemeriksaan visual, dan acceptance Windows dibedakan.

**Penyesuaian hasil pengujian kandidat:** titik asal mesh unduhan bukan penentu tunggal jangkauan tahan E; interactor produksi yang telah memvalidasi collider, jarak dan halangan menjadi acuan. Gudang ampas memiliki lorong bebas di depan tiga clue. Papan rapuh tetap memiliki collider sebelum runtuh, tetapi dibake sebagai area non-navigable bagi AI sehingga tidak ada jalur AI melayang setelah support hilang. Halaman bawah balkon tertutup lantai fisik dan terhubung tangga kembali. Bukti visual adalah fixture berpose dengan renderer produksi, bukan klaim pacing atau kinerja Windows.

**Presentasi yang belum final:** sumber suara manusia nonverbal berlisensi telah dipasang, tetapi akting VO dialog Indonesia persis naskah belum tersedia. Pose sinematik memegang sepatu/pegangan pagar serta penyamaran Damar belum memenuhi naskah final. Jangan menandai QA-123, QA-126, atau kesetaraan DreadOut sebagai selesai hanya karena alur data dapat berjalan.

## 1. Naskah lima adegan (KANON; bagian yang diubah diberi catatan editorial)

> **[REVISI EDITORIAL]** Beberapa teks dokumen/dialog di bawah dikoreksi atau diperjelas untuk menutup kontradiksi (lihat §0.7). Perubahan itu bukan kutipan verbatim naskah 15 Oktober; setiap perubahan bermakna punya nomor `E#` yang dapat dilacak. Catatan `[KLARIFIKASI]` tidak mengubah dialog; ia menjelaskan perilaku yang harus dikode. Berbeda dari Phase 1 dan Phase 2 (enam adegan), Babak 3 tetap lima adegan sesuai naskah; bagian "kembali ke motor" menjadi sub-bagian 3 pada Adegan 5, setara Adegan 6 di babak sebelumnya.

### ADEGAN 1: Tahap Siang Hari — Kamar Kos Raka / Safe Hub (Menit 00:00 – 10:00)

**Kondisi Awal:** Pukul **15:30 WIB** (siang hari). Raka duduk di depan meja komputer di **scene Kamar Kos yang sudah menjadi Golden Scene** (E18; transform/ukuran build terbaru wajib diverifikasi Codex, P3-D01), dengan mata cekung kurang tidur. Di dinding kos, Raka telah menempelkan **peta Jawa Barat** dengan benang dan pin: **Rumah Atmaja** (titik asal petunjuk), **Rusunawa Griya Asri**, **RSUD Lama Sido Waras**, dan **Pabrik Gula Belanda "Sindang Laut"** sebagai titik ketiga yang membentuk sebagian pola bintang segel *Panca-Sengkala*; dua ujung bintang masih kosong (E1). **[KLARIFIKASI]** Inventori, uang tersedia, baterai, dan upgrade berasal dari save Babak 2 yang sebenarnya; tidak ada hibah otomatis di pintu masuk babak selain yang tertulis di §8.0 (P3-D02). Indikator Sanity baru diperkenalkan pada Babak 4.

#### 1. Riset Forum Urban Legend & Petunjuk Kelemahan Wewe Gombel

* **Objektif Layar:** `Buka PC untuk membaca arsip Pabrik Gula Sindang Laut dan meng-upgrade Lensa UV Rajah.`

* Pemain membuka forum *urban legend* di layar PC:

> **TEKS ARTIKEL FORUM (MISTERI 'MBOK GOMBEL' DI PABRIK GULA SINDANG LAUT):**
> "Pabrik Gula Sindang Laut dibangun tahun 1912 dan berhenti beroperasi tahun 1999 setelah 9 anak buruh tebu hilang tanpa jejak di malam pesta giling. Warga sekitar percaya anak-anak itu disembunyikan oleh **Wewe Gombel (Ibu Asuh Semu)** yang bersarang di rangka besi atap ruang ketel uap.
> **Aturan Bertahan Hidup:**
> 1. Jangan pernah langsung berlari ke arah suara tangisan anak atau suara teman yang meminta tolong! Wewe Gombel suka memanipulasi suara untuk memancing korban ke **jebakan lantai kayu/besi yang keropos hingga runtuh**.
> 2. Gunakan **Sinar UV Rajah** untuk melihat tanda segel di lantai dan membedakan jalan asli dengan jurang ilusi ciptaannya."

* **Raka (Monolog Suara):** *"Semalam di video `Penonton_Terakhir`, gue jelas-jelas liat Damar diikat di dekat tungku pabrik itu. Kalau penunggu di sana bisa niru suara manusia dan bikin ilusi... gue harus hati-hati bedain mana Damar yang asli dan mana jebakan."*

**[KLARIFIKASI]** Artikel forum adalah **dokumen bonus bacaan** (tidak bernomor Dokumen #1–#2) dan tersimpan di tab Dokumen Jurnal setelah dibaca. Tiga fakta dipakai ulang oleh puzzle dan harus dapat dibaca ulang dari Jurnal: **9 anak** (cocok dengan Pelat Nomor Buruh, §5.1A), **"jangan langsung berlari ke suara"** (aturan jebakan suara, §4.1) dan **"tanda segel di lantai"** (rajah silang, §3.2A). Membaca artikel tidak menjadi syarat gerbang mana pun; pemain yang melewatinya tetap diajari aturan inti lewat peringatan HP di Gudang Gilingan (Adegan 2) dan di Bangsal (Adegan 3).

#### 2. Upgrade Modul Smartphone & Belanja Toko Klenik

Pemain membuka menu modifikasi HP dan Toko Online Klenik (**Pasar Gaib**) menggunakan saldo saweran yang terkumpul dari Babak 2:

1. **Upgrade Modul Smartphone (Terbuka di Babak 3):**
* **Upgrade `Lensa UV Rajah` (tombol `[R]` saat kamera aktif):** Memancarkan sinar ultraviolet ungu dari lampu *flash* HP yang mampu menampakkan rajah kuno tak kasat mata, jejak kaki palsu, serta menandai **lantai jebakan yang akan runtuh** dengan **rajah silang ungu menyala**.

* **Upgrade `EVP Recorder v2.0` (Penganalisis Spektrum Suara):** Kini menampilkan grafik gelombang suara. **Gelombang Hijau** berarti suara manusia asli; **Gelombang Merah Bergerigi** berarti suara tiruan Wewe Gombel. **[KLARIFIKASI]** Selain warna, UI wajib menampilkan teks **`ASLI`** atau **`TIRUAN`** dan bentuk gelombang yang berbeda (hijau halus, merah bergerigi) agar tidak bergantung pada warna saja (§8.3).

2. **Belanja Persediaan Ekspedisi:**
* **3x Baterai Cadangan / Powerbank Besar** (paket Rp750.000 **[BASELINE UJI]**).

* **4x Flare Industri** (paket Rp1.000.000 **[BASELINE UJI]**; sangat krusial di area pabrik yang gelap gulita untuk membakar *Kabut Sengkala* dan menerangi area tanpa menguras baterai HP).

* **2x Kemenyan Bakar** (paket Rp350.000 **[BASELINE UJI]**; persediaan untuk sistem Sanity yang aktif di Babak 4).

* Pemain mengambil kunci motor dan memilih **`[BERANGKAT KE PABRIK GULA SINDANG LAUT - PUKUL 23:45 WIB]`** → *Fade to Black*.

**[KLARIFIKASI — aturan Pasar Gaib]**

* Dua upgrade wajib **tidak dijual**: terbuka otomatis begitu `P3_FORUM_READ` atau begitu pemain membuka menu Modul HP pertama kali di Kos (mana yang lebih dulu), supaya jalur utama tidak bergantung pada uang atau pada membaca forum (E12). Event `P3_UPGRADE_UNLOCK` adalah receipt satu kali.
* Tiga paket belanja bersifat **opsional**. Tiap paket dapat dibeli **sekali** per kunjungan Safe Hub [BASELINE UJI]; total bila semuanya diambil: **Rp2.100.000** (harga **bukan kanon**; naskah tidak menyebut harga, nominal mengikuti skala satuan P2: powerbank Rp250.000, flare Rp250.000, kemenyan Rp175.000). Pembayaran hanya dari `wallet_legitimate`; dana `Penonton_Terakhir` yang terikat tidak dapat dibelanjakan (§5.3).
* Jika saldo kurang, paket ditampilkan **tidak aktif dengan harga terlihat**, tanpa paket gratis pengganti. Seluruh jalur wajib dapat diselesaikan tanpa membeli apa pun (QA-01); satu-satunya akibat tidak membeli adalah tekanan sumber daya lebih tinggi (powerbank), hilangnya penerang flare di jalur gelap, dan hilangnya opsi flare pengganti generator pada siklus 1–2 (siklus 3 mempunyai counter UV/generator tanpa flare, §4.4).
* **Powerbank** bertipe **besar (+80 poin)**; tiga unit masuk inventori sebagai **instance berbeda** dengan ID sumber unik. **Kemenyan Bakar** masuk inventori dengan tooltip bahwa kegunaannya belum diketahui; **tidak dapat dipakai** di P3 dan **tidak boleh dikonsumsi** oleh sistem apa pun di babak ini. **Flare** dapat dilempar lewat `[G]`: di arena sebagai alat boss dan counter kain **opsional**, di luar arena sebagai **penerang area** (§4.3B). Tidak dibutuhkan di rute wajib; jika stok nol, permainan tetap solvable (§4.4).
* Pembelian dikomit **atomik** bersama pengurangan saldo dan penambahan inventori (§6.4). Memuat ulang di tengah belanja tidak menggandakan barang maupun memotong saldo dua kali.
* **Pengisian daya di Safe Hub:** colokan di meja PC mengisi baterai HP sampai penuh **satu kali per keberangkatan** sebelum `[BERANGKAT ...]` dikonfirmasi (P3-D02). Continue/retry/checkpoint **tidak** mengisi ulang.

---

### ADEGAN 2: Halaman Timbangan Tebu & Gudang Gilingan — Jebakan Suara & Lantai Runtuh (Menit 10:00 – 28:00)

**Kondisi Awal:** Pukul **23:54 WIB**. Raka berdiri di depan kompleks **Pabrik Gula Belanda 'Sindang Laut'**—sebuah bangunan industri raksasa beratap seng tinggi dengan cerobong asap bata yang menjulang ke langit malam. Suasana sangat gelap gulita, hanya terdengar derit roda lori tebu yang tertiup angin. **[KLARIFIKASI]** Pukul 23:59 adalah batas yang diucapkan `Penonton_Terakhir` di akhir Babak 2 ("pukul 23:59, tungku... akan menyala untuk kalian berdua"); Raka tiba lebih awal dan tidak ada timer jam nyata (§11.2). Motor Raka diparkir di halaman timbangan dan tetap di sana sampai akhir babak.

#### 1. Menyalakan Siaran & Kontrak Saweran

* Pemain mengangkat Smartphone (`[TAB]`) dan menekan **`[MULAI LIVE]`** (naskah: `[GO LIVE]`, E2).

* Indikator penonton langsung menembus **[LIVE | 👁️ 185.000 Penonton]** (E8).
* **Raka (Dialog Suara ke Kamera):** *"Ekspedisi segel ketiga. Gue di Pabrik Gula Sindang Laut. Semalam kalian liat sendiri rekaman Damar diikat di ruang tungku pabrik ini. Gue udah pasang Lensa UV sama filter suara di HP. Bantu gue pantau kalau ada yang janggal di layar."*
* **Rentetan Live Chat #1:**
* `@pabrik_hantu`: *"Gila gelap banget bang pabriknya! Senter HP doang mah gak tembus itu ke ujung ruangan!"*

* `@rina_cirebon`: *"Hati-hati bang, di Sindang Laut banyak lubang gilingan tebu di bawah lantai kayu!"*

* `Penonton_Terakhir` **[DONASI Rp3.333.333]**: *"Sahabatmu kedinginan di dekat tungku utama, Raka. Dengar itu? Dia sudah memanggil namamu. Cepat lari ke sana kalau kau peduli padanya."*

**[KLARIFIKASI]** Donasi Rp3.333.333 adalah **peristiwa cerita** dari sumber terikat: tampil sebagai notifikasi, tidak menambah `wallet_legitimate`, dan tidak diproses ulang saat reload. Kalimat "cepat lari ke sana" adalah **umpan naratif** yang sengaja mengarah ke jebakan (aturan forum #1); tidak ada mekanik yang memaksa pemain berlari. Tidak ada tantangan saweran opsional di Babak 3 (§5.4). Chat `@pabrik_hantu` ("senter HP doang gak tembus") menjadi pengajaran bahwa jalur wajib punya **cahaya diegetik minimum** (lampu lori, bohlam darurat, cahaya bulan) dan flare hanya memperluas, bukan menggantikan (§7.5).

#### 2. Tutorial Jebakan Suara Wewe Gombel (`EVP Recorder` + `Lensa UV Rajah`)

Pemain masuk ke **Gudang Gilingan Utama**—ruangan luas berlantai panggung kayu dan jembatan besi (*catwalk*) setinggi 8 meter di atas deretan roda gigi penggilingan tebu berkarat. **[KLARIFIKASI]** `PHASE3_CP2` disimpan **sebelum** jebakan suara #1 pada anchor pintu masuk gilingan (§6.3).

* **Pemicu Jebakan Audio #1:** Dari ujung jembatan kayu sisi kiri yang gelap, tiba-tiba terdengar suara **Damar** berteriak panik:
* **Suara "Damar" (Dari Kejauhan):** *"RAKA! KA, GUE DI SINI! TOLONGIN GUE, TALI GUE MAU PUTUS KE BAWAH MESIN!"* (Di ujung suara teriakan itu, terdengar samar suara tangisan bayi).

* **Eksekusi Gameplay (Mekanik Anti-Jebakan):**
1. Jika pemain langsung berlari lurus menuju suara "Damar" tersebut tanpa memeriksa HP, papan jembatan kayu di tengah jalan akan **runtuh seketika** dan menjatuhkan Raka ke roda gigi penggilingan di bawah (*Game Over*, `FALL`)! **[KLARIFIKASI]** Runtuh didahului **telegraph 0,6 detik** (derit kayu, serpihan jatuh, debu) dan hanya terjadi pada papan yang berstatus `FRAGILE_FATAL`; **tidak ada papan rapuh pada jalur kanan yang menjadi rute benar** (§3.2A). Pemain tidak mati karena suara, melainkan karena memilih jalur lurus yang rapuh.

2. **Cara Benar:** Pemain mengangkat HP dan melihat indikator **`EVP Recorder`**: grafik gelombang suara menunjukkan **Warna Merah Bergerigi** dengan teks peringatan: **`[ANOMALI FREKUENSI: TIRUAN SUARA VOKAL TERDETEKSI!]`** (disertai label **TIRUAN**).

3. Pemain menekan **`[R]` untuk menyalakan `Lensa UV Rajah`** ke arah lantai jembatan di depannya. Di bawah sinar ungu UV, tampak jelas **simbol Rajah Silang Ungu** pada papan jembatan lurus yang menandakan lantai tersebut adalah **Jebakan Lantai Runtuh**!

4. Dengan memantau layar **Lensa UV**, pemain berjalan memutar melewati jalur pipa besi di sebelah kanan yang berlantai kokoh.

* **Rentetan Live Chat #2:**
* `@dedi_kobuzer`: *"ANJIR ITU TADI SUARA BANG DAMAR PALSU WOI! Untung dicek pakai frekuensi suara!"*

* `@anak_teknik`: *"Liat di sinar UV bang! Lantai tengahnya rapuh semua dikasih rajah jebakan!"*

**[KLARIFIKASI — masa belajar]** Untuk **suara tiruan pertama** HP otomatis menampilkan peringatan EVP **tanpa menunggu pemain mengangkat HP**, dan petunjuk UV muncul sebagai ikon `[R]` di HUD. Selama `learnWindow` (30 detik atau sampai jebakan pertama dilewati), papan rapuh hanya memicu **telegraph 1,2 detik** (bukan 0,6) dan melempar pemain mundur tanpa mati bila ia berhenti menginjak sebelum telegraph habis. Sesudah jebakan pertama dilewati, aturan penuh berlaku (§8.2). Pemain yang mengangkat HP dan menyalakan UV sebelum suara memicu tetap valid.

#### 3. Menyalakan Generator Akses #1 & Bukti Klenik Pertama

Di ujung jalur kanan Gudang Gilingan, pintu besi menuju **Bangsal Ketel Uap** memakai **kunci magnetik ritual** yang tetap terkunci saat listrik mati.

* Pemain menemukan **Generator Diesel Tua #1** (**Generator Akses**, `G-ACC`, E7). Setelah menarik tuas engkolnya **3 kali**, lampu sorot kuning di atas pintu menyala terang dan membuka kunci magnetik gerbang. **[KLARIFIKASI]** Tiga tarikan adalah tiga aksi `E` berturut-turut dengan animasi pendek (±1,5 detik tiap tarikan); gagal/terhenti di tengah tidak mengunci progres secara permanen dan **tidak menghabiskan generator**. Generator akses tidak berkaitan dengan baterai HP.

* Di bawah cahaya lampu sorot yang baru menyala, terlihat sebuah **Ayunan Kain Gendongan Batik** yang tergantung di katrol rantai mesin giling. Di dalamnya terdapat **Buku Catatan Mandor Pabrik 1999 (Item Dokumen #1)** dan **Pusaka #1: Boneka Jerami Tebu**:

> **TEKS DOKUMEN (CATATAN MANDOR PABRIK - 1999):**
> "Mbah Suro meminta tiga tumbal anak buruh untuk ditanam di bawah tungku uap agar gilingan tahun ini menghasilkan uang berlipat. Tapi sejak ritual malam Jumat Kliwon itu, sosok perempuan raksasa berambut putih yang dipanggil **Mbok Gombel** turun dari atap pabrik. Dia menyembunyikan anak-anak itu di dalam kepompong kain goni dan menipu siapa pun yang mencarinya dengan suara tangisan palsu. Satu-satunya cara membakar sarangnya adalah menyalakan seluruh **Generator Lampu Sorot Tungku** dan membaca jalur dengan **Lensa UV Rajah**, lalu memancingnya ke cahaya lampu sorot."

* Pemain memotret ayunan kain gendongan tersebut menggunakan **Lensa AR (`[Q]`)** → **`[✓] BUKTI KLENIK 1/3: JEJAK GENDONGAN TUMBAL (LENSA AR)`** tercentang di Jurnal!

**[KLARIFIKASI — urutan kebutuhan]** Ayunan hanya dapat dijangkau **setelah** lampu sorot akses menyala (cahaya diegetik memperlihatkan ayunan); memotret ayunan memerlukan AR aktif dan target dalam frame ≥1,0 detik (§8.0). Mengambil Boneka Jerami dan membaca Catatan Mandor tidak menggantikan bukti AR; ketiganya tiga event berbeda (`P3_TAKE_BONEKA`, `P3_DOC_MANDOR`, `P3_EVIDENCE_GENDONGAN`). Catatan Mandor memuat dua petunjuk yang diulang di Jurnal: **tiga tumbal** (E9) dan **metode generator + UV + pancing** (rumus boss).

---

#### Jalur opsional Arsip Saksi

Sebelum melanjutkan ke aula, pemain dapat memeriksa **Pelat Nomor Buruh 09**. Loker buruh 09 berada di **balkon servis belakang Kantor Mandor**, sebelum pintu aula tungku dikunci (E11). Verifikasi ketukan dengan EVP v2, telusuri panah UV pada balkon yang kokoh, lalu putar katup kecil berlabel 09 untuk membuka loker. Suara Damar merah yang memanggil dari jalur rapuh adalah pengalih perhatian. Pengambilan tidak dihitung sebagai bukti atau pusaka boss. Sebelum meninggalkan babak, status item ini ditampilkan 0/1 atau 1/1 pada Jurnal. **Rincian lokasi dan puzzle ada di §5.1A dan §3.4; item ini tidak tersedia sebelum pemain mencapai Kantor Mandor, sehingga muncul secara alami pada Adegan 3.**

---

### ADEGAN 3: Bangsal Ketel Uap (Boiler Room) — Fase Investigasi & Stealth Wewe Gombel (Menit 28:00 – 52:00)

Menggunakan mekanik Fase Gameplay B (Investigasi & Stealth — *Phasmophobia × Outlast*) di mana Boss bersifat KEBAL dan pemain harus mengumpulkan sisa bukti & pusaka.

**Kondisi Awal:** Pemain memasuki **Bangsal Ketel Uap (Boiler Room)**—ruangan industri bertingkat dua yang dipenuhi tabung-tabung silinder uap raksasa, katup pipa yang menyemburkan uap panas berdesis, dan puluhan kepompong kain goni yang bergelantungan di langit-langit gelap. **[KLARIFIKASI]** `PHASE3_CP3` disimpan saat pemain melewati gerbang magnetik dan berada di landing aman Bangsal, **sebelum** `P3_WEWE_REVEAL` (§6.3).

#### 1. Kemunculan Perdana Wewe Gombel (Fase 100% Kebal)

Saat Raka berjalan di lorong di antara dua ketel uap besar, terdengar suara senandung nina bobo Jawa yang serak dari atas pipa langit-langit, diikuti bunyi gedebuk berat (**`"BRUK!"`**).

* Dari atas rangka besi, turun sosok **Wewe Gombel**: tubuhnya bungkuk menjulang setinggi hampir 3 meter, kulitnya keriput keabu-abuan tertutup jelaga tebu, rambut putihnya terurai panjang menyapu lantai, dan bagian dadanya menjuntai panjang hingga ke lutut, sambil menyeret selembar **Kain Gendongan Batik Raksasa** yang bergerak hidup seperti tentakel.

* **Peringatan Sistem di Layar HP (`MataBatin.apk`):**
> **`[PERINGATAN ENTITAS: WEWE GOMBEL TERDETEKSI — 100% KEBAL! JANGAN TERPANCING SUARA TANGISAN BAYI & HINDARI JEBAKAN LANTAI RUNTUH DENGAN LENSA UV!]`**

**[KLARIFIKASI]** Pada reveal pertama Wewe turun **di ujung koridor ketel yang jauh** (≥ 12 m) dan langsung kembali ke rangka atap; tidak ada serangan pada reveal. Peringatan HP tidak bergantung pada HP diangkat. Setelah reveal, `PATROL` aktif dengan **jendela belajar** (§4.3).

#### 2. Aturan Unik Stealth Wewe Gombel & Pengumpulan Bukti 2 & 3

Untuk membuka segel menuju **Aula Tungku Utama**, pemain harus mencari **2 Bukti Klenik** dan **2 Benda Pusaka** tersisa di Bangsal Ketel Uap sambil menghindari patroli Wewe Gombel:

* **Aturan Unik Wewe Gombel Saat Patroli:**
1. **Umpan Tangisan Bayi (*Audio Lure*):** Wewe Gombel akan sengaja memunculkan suara tangisan bayi atau suara Damar dari balik lorong sempit. Pemain **wajib mengecek layar `EVP Recorder` terlebih dahulu**—jika gelombangnya **Merah**, berarti Wewe Gombel sedang menunggu di balik belokan itu untuk menyergap dari langit-langit!

2. **Sembunyi & Matikan Cahaya (*Outlast* Mechanics):** Saat langkah berat Wewe Gombel mendekat, pemain harus masuk ke dalam **Loker Besi Pekerja** atau merangkak ke dalam **Lubang Tungku Mati**, lalu menekan **`[F]` untuk mematikan layar HP**.

3. **Jebakan Lantai Runtuh Dinamis:** Selama berpatroli, Wewe Gombel menebarkan ilusi yang menutupi lubang-lubang saluran uap di lantai. Pemain harus sesekali menyalakan **`Lensa UV Rajah` (`[R]`)** untuk memastikan lantai di depannya tidak bertanda Rajah Ungu sebelum menginjaknya.

* **Lokasi Bukti & Pusaka di Bangsal Ketel Uap:**
1. **Bukti #2 & Pusaka #2 (Kantor Mandor Lantai 2 Bangsal):**
* Pemain menaiki tangga besi (sambil melompati 2 celah lantai lapuk yang terdeteksi Lensa UV, E17) menuju ruang kontrol mandor.

* Pemain merekam suara rintihan arwah dari radio interkom pabrik menggunakan **`EVP Recorder`** → **`[✓] BUKTI KLENIK 2/3: FREKUENSI EVP NINA BOBO SENGKALA`** tercentang! (E13: bukti valid bila rekaman menangkap segmen **ASLI**.)

* Di atas meja kontrol, pemain mengambil **Pusaka #2: Sabit Tebu Berukir Rajah**.

2. **Bukti #3, Pusaka #3, & Jejak Asli Damar (Gudang Ampas Tebu / Bagasse):**
* Saat Wewe Gombel berpatroli ke lantai atas, pemain menyelinap turun ke **Gudang Ampas Tebu** di sudut barat.
* Pemain menyalakan **`Lensa UV Rajah` (`[R]`)** ke dinding bata gudang: tampak lukisan rajah darah raksasa yang menceritakan ritual penyembunyian anak oleh Mbah Suro. Pemain memotretnya → **`[✓] BUKTI KLENIK 3/3: RAJAH UV PENYEMBUNYI ANAK`** tercentang!

* Di bawah dinding tersebut, pemain mengambil **Pusaka #3: Selendang Batik Terbakar** serta menemukan **Kartu Pers & Sepatu Sebelah Kiri milik Damar (Item Dokumen #2)**:

* Di balik kartu pers Damar, terdapat tulisan tangan Damar menggunakan arang batu bara yang masih baru: *"Ka, video gue di tungku pabrik ini direkam kemarin siang! Sekarang Mbah Suro bawa gue keluar pabrik menuju **Desa Mati Karang Setra**. Hati-hati sama penonton di Live Chat lo, Ka... mereka bukan manusia semua!"*

* **Konfirmasi Jurnal Deduksi di HP:**
Pemain membuka **Jurnal Klenik** dan mengonfirmasi ketiga bukti yang terkumpul:

> **`[ENTITAS TERIDENTIFIKASI: WEWE GOMBEL (IBU ASUH SEMU)]`**
> **`[METODE EKSORSISME: Masuk ke Aula Tungku Utama. Gunakan LENSA UV RAJAH untuk menghindari jebakan lantai runtuh, nyalakan GENERATOR LAMPU SOROT / FLARE untuk membakar Perisai Kabut Hitamnya, lalu tembakkan BLITZ KAMERA saat perisainya pecah!]`**

**[KLARIFIKASI — urutan bebas]** Bukti #2 dan #3 (serta Pusaka #2 dan #3) dapat diperoleh **dalam urutan apa pun**; tidak ada gerbang di antaranya. Jalur Gudang Ampas Tebu (L1 barat) dan jalur Kantor Mandor (L2) sama-sama dapat dicapai dari lorong ketel dengan dua rute menghindar. Menutup Jurnal tanpa menekan **Konfirmasi identitas** tidak membuka gerbang aula (§2.4). Kartu pers adalah **Dokumen #2**; **Sepatu kiri Damar** adalah **story item** terpisah (`P3_SEPATU_DAMAR`) yang dipegang Raka pada epilog.

---

### ADEGAN 4: Malam Puncak — Eksorsisme Wewe Gombel di Aula Tungku Utama (Menit 52:00 – 68:00)

Menggunakan mekanik Fase Gameplay C (Klimaks Eksorsisme Boss — Fase *Alan Wake × DreadOut* × Lensa UV).

#### 1. Pemasangan Pusaka dan Jebakan Boneka Damar

Pemain membuka gerbang besi besar menuju **Aula Tungku Pembakaran Utama**—ruangan melingkar raksasa dengan tungku api batu bara di tengah, dikelilingi jembatan besi bertingkat dan **3 Generator Lampu Sorot Pabrik** di sisi **Utara, Selatan, dan Timur** (gerbang masuk di sisi Barat).

* Pemain memasang **Boneka Jerami Tebu, Sabit Tebu, dan Selendang Batik** pada tiga dudukan altar tungku; pintu masuk masih terbuka sampai interaksi **Karung Damar Palsu** dimulai (E5). **[KLARIFIKASI]** Pemasangan adalah tiga aksi `E` berbeda pada tiga dudukan, **visual runtime dan dapat dibatalkan** (`E` lagi mengambil kembali ke inventori); belum persisten (§6.3).

* Di depan tungku utama, terlihat sosok berpakaian persis seperti Damar sedang terduduk terikat di kursi besi (persis seperti di video akhir Babak 2).
* **Raka (Dialog Suara — berlari mendekat):** *"Damar?! Dam, bertahan gue lepasin ikatan lo—"*
* Begitu Raka membalik kursi tersebut, sosok itu ternyata hanyalah **Karung Goni Berisi Jerami Tebu** yang dipakaikan jaket luar Damar, sementara dari dalam perut karung itu keluar speaker radio kecil yang memutar rekaman suara tangisan bayi!
* **Pemicu Boss Fight:** Pintu gerbang membanting tertutup! Dari atas kubah tungku, **Wewe Gombel** terjun mendarat di atas tungku utama dengan tubuh diselimuti **Perisai Kabut Hitam (Kabut Sengkala)** yang pekat! Ia menjerit memekakkan telinga hingga separuh papan kayu di sekeliling jembatan aula retak!

**[KLARIFIKASI — urutan ritual]** `E` pertama yang valid pada Karung Damar Palsu (tiga bukti + Jurnal terkonfirmasi + tiga dudukan terisi + pemain di radius) **menyimpan CP5 lebih dulu**, lalu memicu pintu membanting dan boss. Keluar dari aula tanpa menyentuh karung tidak memulai boss. Pemain boleh keluar-masuk aula selama karung belum diperiksa (E11).

#### 2. Mekanik Boss Fight (3 Putaran: Lensa UV + Generator Cahaya/Flare + Blitz Kamera)

Pertarungan ini menggabungkan **lima unsur** mekanik secara penuh (E4):

1. **Langkah 1 — Navigasi Medan Lantai Runtuh (`Lensa UV Rajah` `[R]`):**
* Di setiap putaran, Wewe Gombel akan melompat ke rangka atap dan mengeluarkan gelombang suara tangisan bayi yang mengaburkan pandangan serta menciptakan **lubang-lubang lantai jebakan semu** di sepanjang jalur menuju generator.

* Pemain wajib berlari sambil menyalakan **`Lensa UV Rajah` (`[R]`)** untuk melompati papan-papan yang menyala ungu agar tidak terperosok ke bara api di bawah jembatan!

2. **Langkah 2 — Hancurkan Perisai Kabut Hitam (*Alan Wake* Light Manipulation):**
* Saat Wewe Gombel turun menghadang atau bergelantungan di dekat salah satu dari **3 Generator Lampu Sorot Pabrik**, pemain harus menarik tuas generator tersebut (atau melempar **Flare Industri `[G]`** tepat ke bawah tubuhnya).

* Sorotan lampu halogen pabrik yang menyala terang langsung membakar **Perisai Kabut Hitam** di tubuh Wewe Gombel beserta kain gendongan pelindungnya hingga ia jatuh berlutut dan meraung kesakitan!

3. **Langkah 3 — Tembakan Segel Kamera (*DreadOut* Blitz Capture):**
* Selagi perisai kabutnya hancur selama 5 detik, layar `MataBatin.apk` mengunci titik lemah di dada dan wajah Wewe Gombel.

* Pemain menekan **`[Klik Kiri Mouse]` (Jepretan Blitz)** untuk menghancurkan segelnya satu per satu!

* **Putaran Terakhir (Segel Ketiga):**
* Setelah terkena 2x Blitz, Wewe Gombel mematikan dua generator pertama dan mencoba menarik Raka dari jarak jauh menggunakan kain gendongannya sambil memutar suara palsu Ibu Raka dan Damar secara bersamaan!
* **Jika memiliki Flare Industri:** Raka melempar flare ke kain Wewe untuk memutus tarikan, lalu memancingnya ke **Generator Ketiga** dan melepaskan **Blitz Penutup**. **Tanpa flare:** sorot **simpul kain dengan UV** atau gunakan sinar generator yang masih tersedia untuk memutus tarikan; fase terakhir tetap dapat diselesaikan. [REVISI EDITORIAL E6; lihat §4.4].

* Tubuh Wewe Gombel terbakar cahaya putih menyilaukan, menyusut menjadi abu kain batik tua, dan tersedot masuk ke dalam segel ketiga di aplikasi `MataBatin.apk`!

**[REVISI EDITORIAL — E6]** "Flare terakhir" adalah cabang sinematik **jika pemain memilikinya**; tanpa flare Raka memutus kain dengan UV pada simpul atau memancingnya ke generator yang tersisa, lalu Blitz. Dua generator yang dipakai sebelumnya korslet **hanya bila hit sebelumnya memakai generator**; generator yang belum sukses tidak dihancurkan/dinonaktifkan permanen demi animasi. Tiga generator selalu cukup tanpa pembelian atau pickup flare. Aturan detail dan kontra jalur tanpa flare ada di §4.4.

---

### ADEGAN 5: Ending Cutscene Babak 3 & Foreshadowing Infiltrasi Live Chat (Menit 68:00 – 75:00)

#### 1. Epilog di Aula Tungku

* Mesin-mesin uap di aula tungku berhenti berdesis. Raka bersandar lemas di pagar besi jembatan; **jika sepatu kiri Damar sudah ditemukan**, ia menggenggam sepatu itu. Bila belum, tangan Raka tetap mencengkeram pagar (E23), tanpa memunculkan sepatu dari udara.
* Raka mengangkat layar Smartphone miliknya:
* **[LIVE | 👁️ 666.000 Penonton]**
* **Total Saweran Kumulatif:** **Rp312.500.000**. *Angka overlay naskah, bukan saldo yang dapat dibelanjakan; ledger belum lengkap (§5.3).*

* **Momen Merinding (Foreshadowing Mekanik Babak 4 — Infiltrasi Live Chat):**
* Awalnya komentar penonton berjalan normal memuji keberanian Raka. Namun tiba-tiba, puluhan akun dengan nama pengguna normal di kolom *Live Chat* mulai mengetik kalimat yang aneh dan menyesatkan secara serempak:

* `@budi_santoso99`: *"Bang Raka, loncat aja ke dalam tungku apinya bang, Damar ngumpet di dasar api itu kok."*

* `@ani_lestari`: *"Iya bang loncat aja ke bawah, lantainya nggak dalem kok, kita tunggu di bawah."*

* `@rejeki_nomplok`: *"Buka gerbang keempat di Karang Setra. Serahkan matamu. Buka gerbang keempat di Karang Setra."*

* **Raka (Bisikan Terkejut):** *"Tunggu... kok ketikan chat-nya sama semua?! Ingat tulisan Damar tadi... 'Penonton di Live Chat lo bukan manusia semua'!"*

**[KLARIFIKASI — chat infiltrasi bukan objektif (E16)]** Tiga baris di atas adalah **narasi dan telegraph untuk mekanik Babak 4**; tidak ada efek gameplay (tidak mematikan layar, tidak membuka kill plane, tidak mengubah baterai). Pemain yang "menurut" dan mencoba memanjat ke tungku/terjun ke bawah hanya mendapat penolakan lunak (collider/pagar atau prompt "Tidak ada jalan ke sana"), bukan Game Over.

#### 2. Pesan `Penonton_Terakhir` — epilog, bukan transisi paksa

* Layar HP kembali berkedip merah-hitam dan memunculkan pesan dari **`Penonton_Terakhir`** disertai saweran **`Rp9.999.999`**:

> **Penonton_Terakhir:** *"Tiga segel lokasi telah terkumpul, Raka. Kau sadar sekarang? Enam ratus ribu penonton di layarmu sedang membuka pintu dari sisi mereka, dan sebagian dari penghuni Pasar Setan sudah mulai ikut mengetik di kolom komentar siaranmu. Besok malam adalah malam penyegelan keempat. Datanglah ke tempat kelahiran Mbah Suro di **Desa Mati 'Karang Setra'**. Di sana, sahabatmu sedang menunggu di tengah lingkaran api Banaspati."*

* Terdengar suara gemeretak bambu terbakar dan tawa serak seorang kakek tua (*Mbah Suro*) dari speaker HP Raka. **[KLARIFIKASI — urutan epilog]** Tawa Mbah Suro menutup pesan dan menuntaskan `P3_EPILOGUE`; **belum** `CKREK!`, **belum** fade, **belum** `P3_COMPLETE`. Kontrol dikembalikan ke Raka dengan objektif baru: **`Tinggalkan Pabrik Gula. Opsional: kembali ke balkon servis Kantor Mandor.`** Donasi Rp9.999.999 adalah **dana terikat**, bukan saldo (§5.3). Naskah menyebut "enam ratus ribu" pada pesan dan **666.000** pada overlay; keduanya dipertahankan (pembulatan karakter `Penonton_Terakhir`).

#### 3. Transisi sebenarnya: kembali ke motor

Di **motor Raka di halaman timbangan** terdapat interaksi **`[E] Tinggalkan Pabrik Gula`**. Interaksi motor **hanya membuka panel UI sementara** (bukan event persisten `P3_LEAVE_PABRIK`); tampilkan **`Arsip Babak 3: 0/1 atau 1/1`** dan status Arsip Saksi global `N/5`, dengan pilihan **`Kembali Menjelajah`** atau **`Lanjutkan`**. Membatalkan panel tidak menulis state. Hanya pilihan kedua memicu `P3_COMPLETE` (segel ketiga, sasaran Karang Setra tersimpan atomik), lalu:

* Suara *shutter* kamera berbunyi keras (**`"CKREK!"`**) → Layar langsung hitam pekat (*Cut to Black*).

**[REVISI EDITORIAL — E3]** Dalam naskah asli `CKREK!` dan *Cut to Black* langsung mengikuti tawa Mbah Suro di aula. Dengan urutan itu pemain yang belum mengambil Pelat Nomor Buruh kehilangan rahasia tanpa peringatan. Pola P1 §8.4 dan P2 E10 dipakai: epilog tanpa fade → kontrol kembali → keluar lewat motor dengan panel sadar → `CKREK!` + hitam. Interaksi keluar tidak tersedia sebelum epilog selesai dan **tidak boleh otomatis terjadi**.

---

## 2. Kontrak implementasi dan alur trigger

### 2.1 Kontrol dasar (KANON + KLARIFIKASI)

| Input | Konteks | Aksi | Catatan implementasi |
|---|---|---|---|
| `TAB` | Eksplorasi | Angkat/turunkan smartphone; akses tombol **MULAI LIVE** (naskah: `GO LIVE`; ikuti input global) | [KLARIFIKASI] Jangan menonaktifkan HP dalam event paksa tanpa mengembalikan kontrol setelah event |
| `Q` | HP aktif | Toggle lensa AR | Ayunan gendongan (Bukti 1); jejak tersembunyi lain bila ada. AR tidak diperlukan untuk rajah UV |
| **`R`** **[GLOBAL/EXISTING pada `GameInputConfig` menurut P1/P2; verifikasi]** | HP aktif (kamera aktif) | Toggle **Lensa UV Rajah** | Overlay kamera, bukan mode kamera; memakai profil AR/UV 0,10 total (§8.0). Menampilkan **rajah silang ungu** (lantai rapuh), rajah dinding (Bukti 3), panah/teks UV balkon (rahasia). Tidak menampilkan hantu |
| `C` **[GLOBAL/EXISTING; periksa binding]** | HP aktif | EVP Recorder v2 | **Ketuk** untuk toggle analyzer, **tahan** untuk merekam target EVP (6 s interkom / 1,5 s ventilasi sebagai baseline). Rekaman berprogres hanya saat sumber/radius valid; tentukan threshold *tap vs hold* secara terpusat, bukan dua aksi terpicu bersamaan (§4.2C). |
| `Klik kiri` | HP/kamera aktif | Foto bukti (1%) atau **Blitz** (15%/6 detik) menurut konteks | Foto ayunan hanya saat AR aktif dan target valid; foto rajah hanya saat UV aktif; Blitz segel hanya valid pada window boss (§4.4) |
| `Klik kanan` ditahan **[DIWARISI P1 §0.7]** | Eksplorasi | Angkat kamera fokus; lepas kembali ke keadaan sebelumnya | App menu HP dialihkan ke Kamera selama tombol ditahan; UV/AR/EVP tetap dapat di-toggle |
| `F` | HP aktif atau bersembunyi | Matikan/nyalakan layar HP | Wajib untuk bersembunyi dari Wewe (§4.3); layar mati **dan** baterai 0 dihitung sama untuk deteksi cahaya |
| `G` **[GLOBAL/EXISTING]** | Eksplorasi dan boss | Lempar **Flare Industri** | Di arena: pemicu perisai/pemutus tarikan; di luar arena: **penerang area** (§4.3B). Satu flare per aksi; stok kosong → umpan balik, tidak ada konsumsi |
| `E` **[GLOBAL/EXISTING]** | Dekat objek | Interaksi kontekstual (engkol generator akses, tuas generator arena, loker, lubang tungku, dinamo, katup 09, dudukan, karung, motor) | `IInteractable` + `InteractionInteractor`; tuas/engkol memakai resolver radius terarah, bukan controller kedua |
| `J` **[GLOBAL/EXISTING]** | Eksplorasi | Buka Jurnal Klenik | `GameAction.OpenJournal`; isi deduksi P3 baru (3 bukti, konfirmasi, Arsip Saksi, Petunjuk) |
| `W/A/S/D`, `Shift`, `Ctrl`, `Space` | Kontrol karakter | Gerak, sprint, jongkok (merangkak ke lubang tungku), lompat (celah lapuk) | Kecepatan dan animasi global; tuning lokal tidak mengubah feel P0/P1/P2. **Lompat tidak diperlukan di jalur gilingan atau arena** (hanya opsional di L2 bangsal) |
| `X` / `T` | HP aktif | Selfie / ThermoKlenik | **Tetap tersedia** (diwariskan P2), tidak dipakai puzzle P3. Tidak ada kebutuhan wajib; tidak boleh menjadi lubang kebijakan (QA-79) |

**Aturan konteks [KLARIFIKASI]:** Satu klik tidak boleh sekaligus mengambil item dan menembak Blitz. Saat menu Jurnal/live/Pasar Gaib dibuka, prioritas input milik UI; saat animasi wajib/pause, combat diproses menurut status adegan. AR dan UV adalah filter kamera HP, bukan portal teleport. **`E` tetap dapat dipakai dengan HP terangkat** (pickup loker, katup, engkol, tuas). **`F` saat `E` ditahan pada dinamo** menghentikan progres tanpa mereset (§8.0).

**Tabel prioritas input [KLARIFIKASI]:** Pause/animasi wajib > **`F` pemadaman darurat saat stealth** (kecuali animasi tidak dapat diinterupsi) > interaksi encounter (`E` pada tuas/engkol/loker/dudukan/karung) > UI HP (Jurnal/live) > foto/Blitz/Flare > interaksi normal. Jika UI Jurnal/chat terbuka, `F` harus tetap dapat meredupkan layar lewat jalur input yang sama tanpa memicu klik atau menutup menu ganda. Mode browsing Jurnal tidak membekukan seluruh dunia; **saat Wewe patroli, membuka Jurnal tidak boleh menjadi kelemahan gratis**: layar HP yang menyala tetap menjadi stimulus cahaya (§4.3).

### 2.2 Stage machine global (KLARIFIKASI)

```text
NOT_STARTED
  -> SAFE_HUB [adegan 1: PC, forum, Pasar Gaib, upgrade, berangkat]
  -> ARRIVAL_HALAMAN [adegan 2 mulai; live; chat #1; donasi]
  -> GILINGAN_TRAP [CP2; jebakan suara #1; UV; jalur pipa kanan]
  -> ACCESS_GENERATOR [engkol x3; gerbang magnetik terbuka; ayunan terlihat]
  -> BOILER_ENTRY [CP3, landing aman; pickup ayunan tidak menjadi syarat masuk]
  -> BOILER_INVESTIGATION [Wewe patroli; Bukti 2/3 dan Pusaka 2/3 dalam urutan bebas]
  -> RITUAL_READY [3 bukti + konfirmasi Jurnal + 3 pusaka; gerbang aula terbuka permanen]
  -> ALTAR_READY [tiga dudukan terisi; Raka dalam radius E karung; CP5 disimpan pada E valid]
  -> BOSS_ACTIVE [gerbang membanting; 3 siklus; rak daya/flare aktif]
  -> BOSS_DEFEATED [3 poin segel]
  -> EPILOGUE [chat 666.000; Rp312,5 juta; infiltrasi chat; pesan; tawa; kontrol dikembalikan]
  -> RETURN_TO_EXIT [aula -> gerbang servis -> lori -> halaman; boleh singgah balkon untuk rahasia]
  -> PHASE3_COMPLETE [hanya P3_COMPLETE setelah konfirmasi final di motor; save + CKREK + hitam]
```

Stage adalah **turunan runtime** dari event, bukti, dan inventori yang persisten; bukan flag save terpisah yang dapat desinkron. Objective/checkpoint membaca stage turunan itu. Pemain boleh kembali ke balkon servis sesudah epilog untuk mengambil rahasia, dan stage tidak boleh mundur gara-gara memasuki ruangan lama. Pemain boleh naik ke Kantor Mandor **sebelum** mengambil ayunan atau Gudang Ampas (urutan bukti bebas, E-klarifikasi §1). **`ALTAR_READY` adalah kondisi turunan sementara di depan karung, bukan event auto-save saat pusaka ditemukan.** Panel keluar motor adalah UI transient; hanya `P3_COMPLETE` merupakan peristiwa akhir persisten. `P3_EPILOGUE` dan `P3_COMPLETE` adalah dua tahap berbeda. Setiap transisi wajib idempoten sehingga `OnTriggerEnter`, save/reload, atau interaksi dobel tidak mencetak ulang hadiah.

### 2.3 Registry event inti (KANON dan KLARIFIKASI)

| ID event yang disarankan | Trigger | Prasyarat | Efek satu kali / status |
|---|---|---|---|
| `P3_ARRIVE_KOS` | Spawn di Safe Hub | `P2_COMPLETE` ada | Jam cerita 15:30; objektif PC; baterai/inventori/uang dari save |
| `P3_FORUM_READ` | Buka artikel forum di PC | Dalam jangkauan PC | Dokumen bonus ke Jurnal; monolog Raka; bukan syarat gerbang |
| `P3_UPGRADE_UNLOCK` | Pertama membuka menu Modul HP **atau** `P3_FORUM_READ` | Belum ada receipt | Buka **Lensa UV Rajah** dan **EVP Recorder v2.0**; tidak berbayar; receipt satu kali |
| `P3_SHOP_PACK_POWERBANK` / `_FLARE` / `_KEMENYAN` | Beli paket Pasar Gaib | `wallet_legitimate` cukup; paket belum dibeli | Kurangi saldo, tambah instance inventori; **satu transaksi atomik, ID transaksi unik** |
| `P3_DEPART_KOS` | Pilih `[BERANGKAT KE PABRIK GULA SINDANG LAUT - PUKUL 23:45 WIB]` | `P3_UPGRADE_UNLOCK` | Isi baterai ke penuh **sekali** (P3-D02), jam cerita 23:45→23:54, fade, spawn halaman |
| `P3_ARRIVE_PABRIK` | Spawn di halaman timbangan | Berangkat | Monolog, objektif live; **PHASE3_CP1** setelah kontrol dikembalikan |
| `P3_START_LIVE` | Tombol MULAI LIVE | HP dapat dipakai | Live ON, **185.000** penonton, Chat #1, donasi `Penonton_Terakhir` Rp3.333.333 (peristiwa cerita, **dana terikat**) |
| `P3_ENTER_GILINGAN` | Lewat pintu Gudang Gilingan | Halaman selesai | **PHASE3_CP2** disimpan **sebelum** jebakan suara #1 |
| `P3_DECOY_VOICE1` | Mencapai trigger panggung kayu | Encounter belum diselesaikan | Suara "Damar" palsu; HP otomatis menampilkan peringatan EVP; jendela belajar (§4.1) |
| `P3_UV_TUTORIAL` | Pertama menyalakan `R` di gilingan | HP aktif | Ikon HUD; papan silang ungu terlihat; objektif jalur pipa |
| `P3_CROSS_CATWALK` | Mencapai ujung jalur pipa kanan | Jalur kokoh dilewati | Tandai gilingan selesai; objektif generator; **tidak fatal** bila belum ada |
| `P3_ACCESS_GENERATOR` | Engkol ke-3 selesai | Dalam jangkauan `G-ACC` | Kunci magnetik terbuka permanen; lampu sorot kuning; ayunan terlihat |
| `P3_DOC_MANDOR` | Baca Catatan Mandor | Ayunan dapat dijangkau | **Dokumen #1**; rumus metode boss; receipt satu kali |
| `P3_TAKE_BONEKA` | Ambil Boneka Jerami Tebu | Ayunan dapat dijangkau | **Pusaka #1**; `relic_ids` + inventory |
| `P3_EVIDENCE_GENDONGAN` | Foto ayunan | AR aktif; target di frame ≥1,0 s | `P3_GENDONGAN_TUMBAL` → `evidence_ids`; centang Jurnal; **commit receipt dan data item tanpa memberi CP4 prematur** (CP3 baru ditulis di landing Bangsal; lihat §6.3) |
| `P3_ENTER_BOILER` | Melewati gerbang magnetik | Ritual akses terbuka | **PHASE3_CP3**; Wewe `DORMANT`; objektif Bangsal |
| `P3_WEWE_REVEAL` | Mencapai tengah lorong ketel | Belum muncul | "BRUK!", Wewe turun di ujung jauh, peringatan HP; `PATROL` + jendela belajar |
| `P3_STAIR_L2_PASS` | Melewati dua celah/jalur samping tangga besi | Wewe tidak menempel | Tandai akses Kantor Mandor; non-persisten penting |
| `P3_EVP_DISCOVERED` | Mendekati Kantor Mandor, statis interkom terdengar | EVP belum ada | Petunjuk ke recorder tanpa bergantung chat |
| `P3_EVIDENCE_EVP` | Sesi rekam interkom selesai | Recorder aktif; jarak ≤2,0 m; **segmen ASLI tertangkap** | `P3_EVP_NINA_BOBO` → `evidence_ids`; **PHASE3_CP4** |
| `P3_TAKE_SABIT` | Ambil Sabit Tebu Berukir Rajah | Meja kontrol dapat dijangkau | **Pusaka #2** |
| `P3_EVP_SEARCH_DONE` | Wewe selesai `SEARCH` setelah rekaman | Bukti EVP selesai | Kembali ke patroli umum; **tidak diulang** setelah reload |
| `P3_ENTER_AMPAS` | Memasuki Gudang Ampas Tebu | Akses dari L1 barat | Opsional; lampu padam, rajah belum terlihat tanpa UV |
| `P3_EVIDENCE_RAJAH` | Foto dinding rajah | UV aktif; target di frame ≥1,0 s | `P3_RAJAH_ANAK` → `evidence_ids`; **PHASE3_CP4** |
| `P3_TAKE_SELENDANG` | Ambil Selendang Batik Terbakar | Dalam jangkauan | **Pusaka #3** |
| `P3_DOC_DAMAR_CARD` | Ambil Kartu Pers + baca tulisan arang | Dalam jangkauan | **Dokumen #2**; sepatu kiri → `P3_SEPATU_DAMAR` (story item) |
| `P3_JOURNAL_CONFIRMED` | Tekan **Konfirmasi identitas** di Jurnal | Tiga ID bukti tepat lengkap | Receipt satu kali; syarat ritual global, bukan scan otomatis |
| `P3_HALL_GATE_OPEN` | Konfirmasi Jurnal + 3 pusaka dimiliki | Pemain menyentuh gerbang aula | Gerbang besi aula terbuka **fisik permanen** sampai pemicu karung (E11) |
| `P3_SECRET_CLUE` | Verifikasi ketukan tiga kali dengan EVP | Dalam jangkauan ventilasi balkon; EVP aktif; segmen ASLI | Ungkap panah UV dan teks "Nomor sembilan tidak pulang" |
| `P3_DECOY_VOICE_SECRET` | Mendekati jalur rapuh balkon | Rahasia belum diambil | Suara Damar **merah** sebagai pengalih; tidak fatal bila UV/EVP dibaca |
| `P3_SECRET_PICKUP` | Putar katup 09 lalu ambil pelat | Katup benar sudah diputar; ID belum tersimpan | Tambah `SECRET_P3_PELAT_BURUH` ke `secret_items`; **autosave atomik segera** |
| `P3_ALTAR_PLACE_1/2/3` | `E` pada dudukan | Pusaka terkait dimiliki | **Visual runtime**, dapat dibatalkan; tidak persisten |
| `P3_DECOY_INTERACT` | `E` valid pada Karung Damar Palsu | 3 bukti unik AND Jurnal terkonfirmasi AND 3 dudukan terisi AND dalam radius E; **CP5 sukses di disk lebih dulu** | **Transaksi runtime nonpersisten**: gerbang membanting, Wewe turun, boss dimulai; jangan overwrite CP5 saat fight |
| `P3_GEN_N` / `_S` / `_E` | Tarik tuas generator | Boss di beam, generator `available` | **Event transien percobaan boss**, buka perisai hanya saat Wewe asli di beam; jendela **5 detik**; jangan simpan ke `completed_event_ids` |
| `P3_FLARE_THROWN` | `G` pada arena | Stok > 0 | Flare terbang/terbakar 10 s radius 3,0 m; konsumsi satu |
| `P3_PULL_BREAK` | Siklus 3: flare mengenai kain, UV simpul, atau sorotan generator asli (§4.4) | Tarikan aktif; flare / UV simpul / sorotan generator valid | Putus tarikan; transien |
| `P3_BLITZ_SUCCESS` | Blitz valid mengenai dada/wajah saat rentan | Perisai terbuka / 5 detik belum habis | 1 poin segel unik **hanya di RAM** percobaan ini; generator terkait korslet; reset seluruh generator/flare bila retry CP5 |
| `P3_BOSS_WIN` | Total 3 poin segel unik | 3 keberhasilan | Commit CP6 atomik: segel ketiga, pusaka pindah ke dudukan, flare terpakai dikurangi, gerbang servis aula terbuka, boss nonaktif |
| `P3_EPILOGUE` | Chat 666.000 + Rp312,5 juta + infiltrasi + pesan + tawa selesai | `P3_BOSS_WIN` | Notifikasi donasi Rp9.999.999 (dana terikat); **kontrol dikembalikan** |
| `P3_SEPATU_TAKEN` | Raka memegang sepatu setelah boss | `P3_DOC_DAMAR_CARD` | Story item terdaftar; tidak persisten ganda |
| `P3_EXIT_PANEL_OPEN` **(runtime/UI saja)** | E pada motor di halaman | Boss kalah dan epilog selesai | Buka panel Arsip 0/1 atau 1/1; `Kembali Menjelajah` menutup panel, **tanpa persist, tanpa mengubah stage** |
| `P3_COMPLETE` **(satu-satunya event akhir persisten)** | Pilih `Lanjutkan` pada panel keluar | Epilog lengkap; panel terbuka; belum complete | Save CP7 final atomik dan tujuan Karang Setra; **`CKREK!` + hitam hanya setelah commit berhasil**, idempoten |

**Catatan [KLARIFIKASI]:** ID di atas adalah **usulan identifier teknis**, bukan ID yang terbukti sudah tersedia dalam repository. Tabel merupakan **registry event** (termasuk event runtime), **bukan daftar field yang semuanya wajib dipersist**: `P3_EXIT_PANEL_OPEN`, `P3_GEN_*`, `P3_FLARE_THROWN`, `P3_PULL_BREAK`, `P3_BLITZ_SUCCESS`, `P3_ALTAR_PLACE_*` dan `P3_DECOY_INTERACT` tidak disimpan ke `completed_event_ids` sebelum kemenangan; `P3_BOSS_WIN` commit atomik adalah status permanennya. Cocokkan dengan sistem event/save yang sudah ada sebelum menambahkan baru.

### 2.4 Prasyarat, kondisi gagal, dan pemulihan

| Fitur | Gate positif | Saat gagal/terputus | Perilaku yang harus dicegah |
|---|---|---|---|
| Jebakan suara #1 | HP diangkat/EVP+UV dibaca; jalur pipa kanan | Mati → CP2 (anchor pintu gilingan), telegraph diulang | Suara tidak boleh memicu kematian tanpa papan rapuh; papan rapuh tidak boleh berada di jalur benar |
| Generator Akses | Tiga `E` engkol | Terhenti → ulang tanpa penalti | Generator habis karena engkol gagal; gerbang terbuka sebelum engkol |
| Ayunan gendongan | Lampu akses menyala; AR aktif untuk foto | Foto berulang tidak menambah bukti | Tiga foto objek sama dihitung tiga bukti |
| Bangsal Ketel | Gerbang magnetik terbuka | Mati → CP3, Wewe `DORMANT` | Reveal menyerang langsung; tidak ada dinamo terjangkau |
| EVP interkom | Segmen ASLI tertangkap | Hanya TIRUAN → pesan "REKAMAN TIRUAN"; ulang | Bukti tercatat dari rekaman tiruan |
| Gudang Ampas | UV menyala saat memotret | Foto tanpa UV → "tidak ada tanda terbaca" | Dinding terbaca tanpa UV (bukti tanpa alat) |
| Bukti klenik | 3 ID unik | Re-scan tidak menambah progres | Hitung 3 screenshot objek yang sama sebagai 3 bukti |
| Gerbang aula | Konfirmasi Jurnal + 3 pusaka dimiliki | Bila kurang, gerbang tetap tertutup; Jurnal memberi umpan balik | Gerbang terkunci kembali setelah terbuka, atau masuk otomatis memicu boss |
| Dudukan | Pusaka terkait dimiliki | Batalkan dengan `E` lagi | Pusaka hilang bila reload di tengah pemasangan |
| Karung Damar Palsu | 3 dudukan terisi + bukti + Jurnal + radius | Beri feedback kekurangan di Jurnal | Battle dipicu saat keluar dari inspeksi / tanpa bukti |
| Generator arena | Boss di sorotan | Jika Blitz meleset/timeout, perisai kembali, generator dapat dipakai ulang | Generator korslet saat gagal sehingga mustahil menang |
| Kemenangan | 3 sumber berbeda berhasil | Jika player mati, pulih di CP5 | Poin segel ganda dari satu generator |
| Ending | Epilog selesai; pemain berinteraksi dengan motor | Boleh kembali ke balkon, mengambil arsip, dan membatalkan panel keluar | Fade prematur saat pesan berakhir; keluar dari aula tanpa jalan pulang |

---

## 3. Perincian desain level dan navigasi

### 3.1 Denah logis (KANON + KLARIFIKASI)

```text
LUAR: Halaman Timbangan Tebu + motor Raka -- Pos Timbang (dinamo D-P3-1) -- lori tebu
   |
   v
GUDANG GILINGAN UTAMA (panggung kayu + catwalk 8 m di atas roda gigi)
   |  kiri: jembatan kayu lurus (RAPUH, silang UV)  |  kanan: jalur pipa besi (KOKOH, rute benar)
   +--> Generator Akses G-ACC (engkol x3) --> kunci magnetik --> ayunan gendongan
   |
   v
BANGSAL KETEL UAP (2 lantai; ketel uap raksasa; kepompong kain goni)
   L1: lorong antar-ketel; Loker Besi; Lubang Tungku Mati; dinamo D-P3-2; Gudang Ampas Tebu (barat)
   L2: tangga besi (2 celah) -> Kantor Mandor (interkom, meja kontrol, Sabit, dinamo D-P3-3)
       -> pintu servis -> BALKON SERVIS (loker buruh 09, rahasia)
   |
   v
GERBANG BESI AULA (barat aula) -- terbuka setelah Jurnal + 3 pusaka
   |
   v
AULA TUNGKU UTAMA (lingkaran): tungku di tengah; jembatan besi bertingkat; 3 dudukan altar;
   Karung Damar Palsu (kursi besi); G-N (Utara), G-S (Selatan), G-E (Timur); rak flare; rak daya
   Gerbang servis aula (dilas; terbuka setelah boss) --> lorong lori --> halaman timbangan
```

**Penting [KLARIFIKASI]:** Aula punya **dua jalan keluar** setelah boss: gerbang masuk barat (jalur asli, memutar bangsal dan gilingan) dan **gerbang servis aula** (jalur pendek ke halaman lewat lorong lori). Sebelum boss kalah, gerbang servis dilas dan tidak dapat dibuka; ini mencegah pemain keluar lewat jalur pendek di tengah ritual. **Jalur pipa kanan di gilingan bukan satu-satunya jalur** (ada dua rute menghindar, §3.5) agar pemain yang jatuh dari celah tidak terjebak. Koridor/arena harus memiliki empat arah yang dapat dibedakan (Utara/Selatan/Timur/Barat) melalui landmark, bukan hanya arah kompas.

### 3.2 Halaman Timbangan dan Gudang Gilingan

**Halaman Timbangan (KANON + KLARIFIKASI).** Titik spawn pemain; motor Raka; **Pos Timbang** berisi papan tulis timbangan tebu dan dinamo D-P3-1. Hujan tidak ada (P3 malam kering dengan angin). Tidak ada puzzle; fungsi: live stream, orientasi, tutorial `[TAB]`/`[R]`. Lori tebu bergerak pelan oleh angin sebagai sumber audio (derit roda).

**Gudang Gilingan (KANON).** Panggung kayu dan jembatan besi 8 m di atas roda gigi. **Jalur lurus menuju suara "Damar" = papan `FRAGILE_FATAL`**; **jalur pipa kanan = kokoh**. Pemain yang mengikuti suara tanpa HP jatuh; pemain yang membaca EVP + UV memilih rute kanan menuju pintu ke Bangsal dan Generator Akses.

### 3.2A Aturan lantai rapuh dan rajah silang (KLARIFIKASI + BASELINE UJI)

| Jenis | Lokasi | Tanda UV | Akibat injak | Peringatan non-UV |
|---|---|---|---|---|
| `FRAGILE_FATAL` | Gilingan jembatan lurus; arena jembatan/papan retak | **Rajah silang ungu** | Telegraph 0,6 s (1,2 s pada masa belajar) lalu runtuh → `FALL` fatal | Derit, debu, papan retak halus; kontras warna kayu |
| `FRAGILE_DROP` | Bangsal Ketel (L1/L2): saluran uap, celah tangga | **Rajah silang ungu** | Jatuh ke lantai bawah/saluran; **tidak fatal**; stimulus bising → Wewe `INVESTIGATE` | Desis uap, hembusan |
| `ILLUSION_VOID` | Hiasan visual Wewe di atas lantai kokoh | Tidak ada rajah (UV memperlihatkan lantai asli) | **Aman**; hanya menipu mata | Kabur/berkedip bila UV menyala |
| `SOLID` | Jalur benar | Tidak ada rajah; di balkon: **panah UV** | Aman | — |

Aturan: (1) **jalur wajib tidak pernah hanya melalui `ILLUSION_VOID`** dan tidak melalui `FRAGILE_FATAL` kecuali di arena (di mana jalur kokoh selalu ada dan disinyalir UV); (2) rajah silang adalah **satu-satunya** indikator bahaya UV (kontras tinggi, bentuk silang, bukan warna saja); (3) UV hanya terlihat melalui layar HP; tanpa UV pemain masih melihat jejak retak, mendengar derit, dan mempunyai rute kokoh; (4) papan rapuh hanya **runtuh sesudah diinjak**, bukan karena Wewe mendeteksi; (5) setiap runtuh menghasilkan debu/serpihan 3D yang murah (*pool*); (6) **lebar jalur aman minimum 1,4 m sebagai baseline Argon** dan diperiksa memakai collider pemain sesungguhnya; (7) **papan rapuh dinamis tidak boleh diaktifkan dalam radius 6,0 m pemain**, di bawah kaki pemain, tepat pada spawn/recovery anchor, atau serentak menutup kedua rute wajib.

**[ARGON — tambahan edge-warning yang dibedakan dari timer runtuh]** Pada tutorial gilingan, area tepi papan selebar **0,8 m** mengeluarkan derit/retak selama **1,0 detik** sehingga pemain yang segera mundur mendapat *flinch warning*; memasuki inti papan dan terus menerobos tetap memulai `FRAGILE_FATAL` dengan timer runtuh baseline 1,2 s (fase belajar) atau 0,6 s (setelah fase belajar). Dua timer ini **bukan satu cooldown yang ditambah otomatis**: uji di 30/60 FPS untuk mencegah dua peringatan salah hitung dan menutup celah pemain farming *grace*. Tunjukkan indikator rajah sebelum tepi berbahaya agar pemain yang berlari bisa bereaksi. Nilai 0,8 m/1,0 s/1,4 m/6,0 m adalah **BASELINE UJI**, bukan angka kanon.

### 3.2B Kontrak penempatan jebakan dinamis dan penopang rute [ARGON / BASELINE UJI]

Sebelum Wewe memasang `FRAGILE_DROP` atau `FRAGILE_FATAL` baru, director membuat kandidat dari tile *authoring* di scene (bukan memilih acak mesh collider mana pun). *Filter* spawn harus memastikan: pemain tidak berada dalam **6,0 m**, tile tidak memegang kaki/lereng tangga aktif, tidak berada di *safe restore anchor*, dan setelah seluruh tile terpasang **masih ada rute kokoh ≥1,4 m** ke setiap bukti, pusaka, tuas boss, panel daya dan pintu keluar. Implementasi boleh memakai graf jalur tangan atau solver navmesh terbatasi; jangan memerlukan runtime pathfinding yang berat untuk seluruh tile di Iris Xe. Jika tidak ada kandidat aman, **skip spawn**; jangan memindahkannya ke kaki pemain.

Di Bangsal dan balkon, `FRAGILE_DROP` berakhir pada lantai bawah dengan tangga/jalur balik yang **benar-benar terhubung** dan AI kembali ke patroli; tidak boleh menjadi game-over tanpa cutscene. Di Gilingan dan arena, `FRAGILE_FATAL` diberi UV, batas retakan fisik dan suara peringatan. Setelah CP6, Wewe berhenti menciptakan tile, ilusi hilang; kerusakan fisik tetap dapat dilihat dan jalan pulang terjamin tanpa bergantung pada baterai/UV.

### 3.3 Generator Akses dan Bangsal Ketel Uap

**Generator Akses `G-ACC`** (E7) di ujung jalur kanan gilingan: tiga `E` engkol (≈1,5 s tiap tarikan) menyalakan lampu sorot kuning di atas pintu besi dan membuka kunci magnetik. Setelah menyala, ayunan kain gendongan di katrol rantai tampak (cahaya diegetik). **Bangsal Ketel Uap** dua lantai: ketel silinder raksasa, katup uap berdesis, kepompong kain goni. Dua rute dari gerbang magnetik: **rute L1** (lorong antar-ketel menuju Gudang Ampas barat) dan **rute L2** (tangga besi ke Kantor Mandor). Keduanya terhubung lewat lorong tengah sehingga patroli Wewe dapat dihindari dari dua arah.

### 3.4 Kantor Mandor, balkon servis, Gudang Ampas, dan jalur investigasi

**[KANON]** Tiga bukti dan tiga pusaka harus diperoleh: Bukti 1 + Pusaka 1 di ayunan (Adegan 2), Bukti 2 + Pusaka 2 di Kantor Mandor (L2), Bukti 3 + Pusaka 3 + Dokumen #2 di Gudang Ampas Tebu (L1 barat). Wewe berpatroli di Bangsal; dua celah lantai lapuk ada di tangga/jalur L2 (E17).

**[KLARIFIKASI]** Ketiga bukti memakai **ID terpisah**, dapat dicapai tanpa urutan wajib kecuali bila ada alasan cerita eksplisit. Kunci gerbang aula hanya setelah semua bukti, konfirmasi Jurnal, dan tiga pusaka. Membaca Catatan Mandor atau Kartu Pers **bukan** pengganti bukti. Merekam interkom valid hanya dengan segmen ASLI; memotret rajah dinding valid hanya dengan UV aktif; mengambil pusaka dan memotret bukti adalah **flag berbeda**.

**Kantor Mandor (L2):** ruang kontrol dengan meja kontrol (Sabit di atas meja), radio interkom pabrik (sumber EVP), dinamo D-P3-3 di sudut tenang, pintu servis ke balkon. **Balkon servis (rahasia):** papan campuran: sebagian `SOLID` dengan panah UV, sebagian `FRAGILE_FATAL`? **[KLARIFIKASI]** Di balkon (ketinggian L2, di atas lantai beton L1) papan rapuh berstatus `FRAGILE_DROP` (jatuh non-fatal ke L1 dan kembali lewat tangga), agar rahasia opsional tidak menjadi kematian tersembunyi; pengalih suara Damar merah hanya menarik pemain menuju papan rapuh itu.

**Gudang Ampas Tebu (L1 barat):** tumpukan ampas tebu, dinding bata berlukis rajah darah (hanya terbaca dengan UV), Selendang Batik Terbakar dan Kartu Pers + sepatu di bawah dinding. Satu pintu masuk; **tidak boleh menjadi dead end** untuk Wewe/pemain (jalan keluar kedua lewat celah ventilasi berukuran manusia).

### 3.4A Isi konkret alokasi menit per adegan (BASELINE UJI)

Ini **budget waktu per beat berdasarkan jalur contoh**, bukan kunci progres linear. Pemain tetap bebas memilih urutan di dalam area yang sama. Pemain mahir yang selesai lebih cepat bukan bug.

| Menit global | Beat level yang nyata | Interaksi/tekanan yang terukur |
|---|---|---|
| 00–03 | Kos: PC, artikel forum | Membaca; tidak ada tekanan; riset memberi 3 fakta yang dipakai ulang |
| 03–08 | Pasar Gaib, upgrade otomatis, belanja opsional, colokan HP | Keputusan sumber daya; tidak ada gate berbayar |
| 08–10 | Kunci motor, fade ke halaman | Transisi; CP1 di halaman |
| 10–14 | Halaman: live, chat #1, donasi, orientasi | Tutorial `[TAB]`, `[R]` ikon |
| 14–20 | Gilingan: jebakan suara #1, EVP, UV, jalur pipa | Aturan dasar diajarkan lalu dipraktikkan; CP2 |
| 20–28 | Engkol `G-ACC`, ayunan, Bukti 1, Pusaka 1, Dokumen #1 | Foto AR; Catatan Mandor; receipt segera pada CP2, CP3 baru saat masuk Bangsal |
| 28–31 | Bangsal: masuk, reveal Wewe, peringatan, jendela belajar | Aturan patroli diajarkan; CP3 |
| 31–37 | L1: dinamo, loker/lubang tungku, umpan suara, jalur ke Gudang Ampas | Stealth, UV bacaan lantai |
| 37–43 | L2: tangga, celah, Kantor Mandor: EVP interkom + Sabit | Bukti 2; Pusaka 2; Wewe `SEARCH` setelah rekaman |
| 43–47 | Gudang Ampas: UV rajah, Selendang, Kartu Pers + Sepatu | Bukti 3; Pusaka 3; Dokumen #2 |
| 47–49 | Jurnal Deduksi, konfirmasi, pesan metode | `P3_JOURNAL_CONFIRMED` |
| 49–52 | Opsional: balkon rahasia; gerbang aula terbuka; jeda persiapan | Rahasia ±3–6 menit di luar target |
| 52–54 | Aula: pasang pusaka, Karung Damar Palsu, CP5, pemicu | Gerbang membanting |
| 54–66 | Tiga siklus + jeda pemulihan | UV, generator/flare, Blitz, tarikan kain |
| 66–68 | Kemenangan, CP6 | Tidak ada autosave selama fight |
| 68–75 | Epilog, chat, pesan; pulang ±60–120 s; motor; `CKREK!` | Kontrol dikembalikan |

**Batas produksi:** Jangan menambah kunci/fetch quest baru untuk mengejar durasi. Rekam waktu tiap beat di PlayMode/build Windows; bila pemain mahir menyelesaikan dalam 55–65 menit, itu bukan bug. Sasaran 75 menit dapat bergeser berdasarkan atmosfer, stealth, dan playtest; hindari memalsukan pacing dengan pintu terkunci otomatis.

### 3.5 Keselamatan navigasi dan pembacaan ruang (USUL BARU)

- Landmark visual untuk arah arena dan bangsal: warna cahaya generator (kuning akses; putih-kebiruan arena), papan nomor ketel, bercak jelaga berbeda, kain batik berbeda per sayap, panah arah rajah.
- **Dua jalur menghindar** dari patroli di Bangsal (L1 dan L2) dan dua rute dari lorong tengah ke Gudang Ampas sehingga stealth bukan jalur satu-satunya yang mudah terjebak.
- Tempat sembunyi (Loker Besi, Lubang Tungku Mati) punya batas masuk/interaksi dan state eksplisit; kamera dan collision AI tidak menembus pintu loker tertutup.
- Area kunci tidak menyembunyikan item wajib di balik objek dinamis/physics yang bisa hilang akibat load atau ledakan generator.
- Jebakan rapuh tidak ditempatkan sebagai satu-satunya jalan; tempatkan **jalur kokoh minimal selebar 1,4 m** yang terbaca dari UV maupun tanpa UV (kontras material).

### 3.6 Aula Tungku Utama: denah dan titik interaksi arena (BASELINE UJI)

Denah konseptual, **belum denah final**. Aula berbentuk lingkaran berdiameter ≈24 m; tungku di tengah (jurang bara 4 m di bawah jembatan). **Tiga generator** di Utara (`G-N`), Selatan (`G-S`), Timur (`G-E`); gerbang masuk di Barat. Dua tingkat jembatan: ring bawah (kayu, sebagian rapuh) dan ring atas (besi, lebih kokoh) dengan tangga di sektor Barat dan Timur. **Dudukan altar** (tiga) di sisi barat tungku; **Karung Damar Palsu** di kursi besi di depan tungku. **Rak flare opsional**, **dua titik daya** dan **alcove servis tertutup** di sisi barat dekat dudukan, tidak berada di jalur pancing; akses servis tetap dapat dicapai dari loop kokoh. Tuas generator: jarak sakelar ≤1,6 m dari `E`; beam radius 2,5 m dari lampu sorot. Tiap generator dapat dicapai tanpa masuk collider boss; jalur pemain mengitari tiga generator dari setidaknya dua arah, dan tidak ada dead end.

| Titik | Fungsi | Batas |
|---|---|---|
| `G-N`/`G-S`/`G-E` | Lampu sorot; tuas `E`; beam 2,5 m | Korslet setelah hit sukses; miss tidak menghabiskan |
| Dudukan ×3 | Pasang Boneka/Sabit/Selendang | Visual runtime; reversible sebelum karung |
| Karung Damar Palsu | Pemicu boss | `E` valid memicu CP5 lalu boss |
| Rak flare | Satu flare opsional dari rak per attempt (bila disetujui) | **Tidak wajib, tidak dapat di-farm**, konsumsi dicatat RAM sampai CP6 |
| Rak daya ×2 + alcove servis | Pengisian manual +25 poin / 8 s; masing-masing jatah awal per `PREP`/`INTERLUDE`; reset darurat di alcove ketika daya tidak cukup | **Tidak otomatis**; reset fisik saat recovery dan tidak memberi source damage |
| Gerbang servis | Dilas; terbuka saat `P3_BOSS_WIN` | **Berbeda dari alcove servis arena**; pintu keluar tidak dapat dibuka sebelum menang |

---

## 4. Sistem ancaman dan state machine AI

### 4.1 Jebakan suara dan lantai rapuh di Gudang Gilingan (KLARIFIKASI + BASELINE UJI)

**[KANON]** Suara "Damar" memanggil dari ujung jembatan kayu kiri; bila pemain langsung berlari lurus tanpa memeriksa HP, papan tengah runtuh dan Raka jatuh ke roda gigi (*Game Over*). Cara benar: baca EVP (merah bergerigi + `[ANOMALI FREKUENSI: TIRUAN SUARA VOKAL TERDETEKSI!]`), nyalakan UV (rajah silang ungu), lewati jalur pipa besi kanan.

```text
DORMANT -> VOICE_PLAYING (trigger panggung kayu; HP auto-warn) -> WAITING_PLAYER
   |-- PLAYER_ON_FRAGILE_PLANK --> TELEGRAPH (0,6 s; 1,2 s di masa belajar) --> COLLAPSE --> FALL_FATAL
   |-- PLAYER_ON_SAFE_ROUTE --> VOICE_DECAYS --> RESOLVED (P3_CROSS_CATWALK)
   +-- AFTER_LOAD --> replay voice as telegraph; fragile planks remain fragile; no duplicate rewards
```

**[KLARIFIKASI]** Suara tiruan **tidak membunuh**; yang berbahaya adalah **papan rapuh yang sengaja ditempatkan di jalur lurus**. Suara hanya memancing. Voice line tidak boleh diputar ulang tiap beberapa detik dengan volume yang menutup petunjuk EVP; pemain boleh mengheningkan dengan menjauh. **Pemain jangan diminta menebak papan mana yang rapuh tanpa petunjuk**: UV, derit pra-runtuh, dan kontras material harus tersedia. Setelah jebakan pertama, suara tiruan lain di Bangsal dan aula memakai aturan yang sama (merah = tiruan, §4.2).

### 4.2 Aturan gelombang EVP v2 dan umpan audio (KLARIFIKASI + BASELINE UJI)

| Sumber | Gelombang | Label | Arti permainan |
|---|---|---|---|
| Suara manusia/arwah asli (ketukan ventilasi balkon, segmen rintihan di interkom) | **Hijau**, halus | `ASLI` | Sumber audio autentik, bisa menjadi bukti; **lokasinya belum tentu aman**, tetap cek lantai dengan UV |
| Suara tiruan Wewe (Damar, ibu Raka, tangisan bayi umpan, nina bobo interkom) | **Merah bergerigi** | `TIRUAN` | Wewe di dekat sumber atau jebakan lantai; jangan ikuti tanpa UV |
| Ambient (angin, lori, uap) | Datar/abu-abu | `LATAR` | Tidak relevan |

**Aturan:** (1) warna **dan** teks **dan** bentuk gelombang berbeda, tidak hanya warna (§8.3); (2) analyzer hanya aktif bila `C` diketuk/aktif; **peringatan otomatis hanya untuk suara tiruan pertama** (jebakan #1) dan reveal Wewe; (3) analyzer memakai profil baterai global tanpa tambahan; (4) label tidak boleh salah: suara merah **selalu** terkait Wewe, suara hijau **tidak dipalsukan oleh Wewe**, tetapi tidak menjamin area aman; (5) **umpan Wewe** memutar suara merah dari titik `LURE_x` dan menunggu di atas titik itu (`AMBUSH_WAIT`); (6) Damar asli tidak ada di pabrik, sehingga **tidak ada suara "Damar" berlabel hijau**.

### 4.2C Kontrak teknis EVP v2 — metadata sumber dan interaksi `C` [ARGON / KLARIFIKASI]

EVP dalam cerita adalah **analyzer fiksi yang mengetahui tag sumber audio buatan developer**; jangan memakai FFT nyata untuk menebak suara manusia/makhluk atau mengakses mikrofon OS. Tiap *authored emitter* mempunyai `sourceId`, `tag ∈ {AUTHENTIC, MIMIC, AMBIENT}`, *priority*, volume/range, *line-of-sight/occlusion* dan *subtitle cue*. `AUTHENTIC` = hijau halus + teks `ASLI`; `MIMIC` = merah bergerigi + `TIRUAN`; `AMBIENT` = abu-abu `LATAR`. **Jangan menyalin contoh Argon yang menetapkan `EvpVoiceTag = Authentic` untuk semua emitter**, karena akan meluluskan suara jebakan sebagai asli.

**Jika beberapa suara bertumpuk:** UI analyzer mengunci *source* yang diarahkan/terdekat dalam cone kamera jika ada; sumber *scripted evidence* interkom di-radius yang benar mengungguli ambience, sedangkan umpan Wewe terdekat mengungguli sumber jauh. Tampilkan satu label utama dengan indikator ``+N sumber`` jika perlu; **jangan membaurkan merah dan hijau menjadi kategori baru yang membatalkan clue**. Interkom Kantor Mandor memakai urutan **8 s MIMIC / 3 s AUTHENTIC**; bukti 6 s hanya valid jika menangkap ≥2 s AUTHENTIC. Ketukan balkon `AUTHENTIC` butuh tahan rekam **1,5 s** pada sumber yang sesuai (nilai baseline). Rekam ulang tidak menggandakan evidence atau secret.

**Semantik input:** `C` ketuk (mis. lepas dalam ≤0,25 s; *baseline configurable*) toggles analyzer; `C` tahan (mis. melewati 0,25 s) memulai sesi perekaman jika target evidence sah, dengan *progress ring*. Setelah **hold lengkap** sumber valid, simpan receipt; melepas dini membatalkan rekam tanpa penalti. Jangan sekaligus mematikan analyzer dan mencatat bukti pada satu release. Bila `C` sudah terikat ke mekanik P1/P2 yang berbeda, *adapter* mengikuti binding yang berlaku di repository dan mempertahankan maksud semantik ini. Kursor, `F`, RMB, pause dan `E` diuji bersamaan. **Label `ASLI` menjelaskan keaslian rekaman, bukan keamanan area**: UV tetap memutuskan pijakan.

### 4.3 Wewe Gombel: stealth dan patroli (fase B) (KLARIFIKASI + BASELINE UJI)

**[KANON]** Tinggi hampir 3 m, bungkuk, rambut putih menyapu lantai, kain gendongan hidup seperti tentakel; senandung nina bobo; muncul di Bangsal dengan "BRUK!"; **100% kebal** di fase ini; memancing dengan suara tangisan/suara Damar; pemain bersembunyi di Loker Besi atau Lubang Tungku Mati dan mematikan layar `F`; menebarkan ilusi di atas lubang saluran uap.

```text
DORMANT -> REVEAL_FAR (BRUK; ≥12 m) -> PATROL_CEILING <-> PATROL_FLOOR
PATROL -> LURE_SET (suara merah di titik x) -> AMBUSH_WAIT (di atas x) -> POUNCE (bila pemain memasuki radius titik)
PATROL -> INVESTIGATE (stimulus: layar HP menyala, flare, bising, jatuh) -> SEARCH -> PATROL
PLAYER_HIDDEN (loker/lubang, layar mati) -> LOITER (6 s) -> LEAVE -> PATROL
CHASE (terdeteksi jarak ≤8 m dengan LOS) -> CLOTH_GRAB (kontak ≤1,4 m, telegraph 0,8 s)
EVP_RECORDED -> SEARCH_KANTOR (menuju Kantor Mandor 3,5 s setelah dobrak/rintihan) -> RETURN_TO_PATROL
ALTAR_STARTED -> HANDOFF_TO_BOSS
```

**[KLARIFIKASI]** (1) Layar HP menyala menghasilkan **stimulus cahaya** dalam radius 12 m dengan LOS toleran; layar mati atau baterai 0 = tidak ada stimulus cahaya (sama dengan `F`); (2) kebisingan: sprint 8 m, jalan 4 m, jongkok 2 m, jatuh/FRAGILE_DROP 10 m; (3) **tidak ada stun darurat** pada Wewe di fase B karena kanon hanya menyebut "100% kebal" dan tidak memberi Flash darurat (P3-D07); (4) `CLOTH_GRAB` didahului telegraph kain bergerak, bayangan, dan bunyi seret; **tidak ada grab instan tanpa telegraph**; (5) saat bersembunyi, Wewe berhenti di depan loker `LOITER` 6 s lalu pergi bila layar mati dan tidak ada gerakan; layar menyala sedikit saja = pintu loker dirobek (fatal); (6) `LURE_x` ditempatkan hanya di lorong sempit dengan dua pintu keluar sehingga pemain yang membaca merah dapat mundur; (7) **jendela belajar** 30 detik pertama setelah reveal: Wewe patroli lebih lambat (×0,7) dan umpan pertama berjarak ≥15 m.

**[USUL BARU]** Sensor stimulus terpisah untuk cahaya layar, flare, bising, dan LOS; simpan *last known position*. Spawn/respawn jangan menempatkan AI di atas pemain. Pemain yang terjebak di bilik/loker setelah event EVP tidak boleh dikunci: Wewe harus keluar dan membuka kembali eksplorasi.

**[ARGON — navigasi Wewe]** Gunakan *ceiling/truss anchors* untuk `PATROL_CEILING`, titik drop/land yang disiapkan art di ujung koridor, dan navmesh lantai untuk `PATROL_FLOOR`/`SEARCH`; transisi animasi hanya saat lokasi landing bebas collider pemain serta punya audio `BRUK!`. Tidak perlu navmesh bergerak 3D seluruh atap. Loker/lubang tungku menahan deteksi setelah layar mati **hanya jika pemain sungguh memutus LOS**; tidak memberi status kebal universal. Di Kantor Mandor dan depan dinding rajah Gudang Ampas, sediakan **jendela interaksi aman setidaknya sepanjang rekaman/foto penting** lewat staging patroli, titik berlindung, dan telegraph; **bukan** bubble invulnerable permanen atau AI yang selalu dimatikan. Bila pemain sengaja keluar berlindung dan menyalakan HP di depan Wewe, aturan serangan tetap berlaku. Ini target playtest, bukan ketentuan bahwa seluruh kantor kebal serangan.

### 4.3A EVP interkom Kantor Mandor dan sergapan yang bisa dihindari (KLARIFIKASI)

Saat mendekati Kantor Mandor, speaker HP menerima statis radio dan ikon EVP (tersedia juga via Jurnal). **Interkom memutar pola berulang**: **segmen TIRUAN** (nina bobo merah, ±8 detik) lalu **segmen ASLI** (rintihan anak hijau, ±3 detik, mis. "...pulang...") (E13). Pemain **menahan `C`** dalam jarak ≤2,0 m dari interkom selama sesi rekam **6,0 detik** yang harus mencakup minimal **2,0 detik segmen ASLI** (§8.0). Bila hanya TIRUAN tertangkap: pesan "REKAMAN TIRUAN" dan ulangi tanpa penalti (baterai 0 tidak mencuri; blitz tidak digunakan). Setelah rekaman valid, Wewe mengarah ke Kantor `SEARCH_KANTOR` setelah **3,5 detik** [BASELINE UJI] (waktu cukup untuk keluar lewat pintu servis, bersembunyi di kolong meja, atau mematikan layar `F`); bukti tersimpan terpisah dari `P3_EVP_SEARCH_DONE`. Bila load dilakukan di antara bukti EVP didapat dan pencarian selesai, pulihkan dari checkpoint aman di luar Kantor lalu jalankan `SEARCH_KANTOR` secara deterministik tanpa menggandakan bukti; setelah `P3_EVP_SEARCH_DONE` sergapan tidak terulang.

### 4.3B Flare Industri di luar arena: penerang area (KLARIFIKASI + USUL BARU)

Naskah belanja menyebut flare "sangat krusial di area pabrik yang gelap gulita... menerangi area tanpa menguras baterai HP". `G` di luar arena melempar flare ±8 m, menyala **20 detik** dengan **radius lampu 6 m**; **tidak diperlukan jalur wajib** (setiap area wajib punya cahaya diegetik minimum) dan **tidak mengusir Wewe**: flare di Bangsal adalah **stimulus cahaya** (radius deteksi 15 m, LOS). Dengan demikian flare memberi pilihan risiko: menerangi bacaan lantai tanpa baterai, tetapi menarik perhatian. Satu flare per aksi; stok kosong = umpan balik tanpa konsumsi. **Membuang flare di luar arena tidak boleh membuat siklus 3 mustahil**: jalur UV/generator tidak memerlukan flare (§8.0). **P3-D13 (flare opsional).**

### 4.4 Wewe Gombel: pertarungan tiga siklus — satu jalur kanon, tanpa flare wajib

**[KANON]** Tiga pusaka pada dudukan altar → pemeriksaan Karung Damar Palsu (`E`, setelah CP5 disk commit) → Wewe turun, arena terkunci → pemain memakai UV untuk membaca lantai, sumber cahaya lingkungan dan/atau flare untuk memecahkan Kabut Sengkala, lalu Blitz ketika perisai terbuka **5 detik**. Tiga keberhasilan penyegelan menutup Babak 3. Saat siklus akhir Wewe meniru suara Ibu Raka dan Damar sambil menyeret pemain dengan kain. **[REVISI EDITORIAL E6]** Teks sumber tentang flare terakhir adalah animasi pilihan saat item tersedia, **bukan** syarat untuk menang.

```text
PREVIEW (pintu terbuka; tiga dudukan dapat dibatalkan; 3 bukti + konfirmasi Jurnal)
  -> E valid pada Karung Damar Palsu
  -> CP5 COMMIT BERHASIL, posisi aman di depan karung, pusaka masih tercatat di inventory
  -> LOCK_RUNTIME -> SPAWN -> PREP (charger/inspeksi arena; tidak autosave)
  -> CYCLE_1: UV_SAFE_ROUTE -> STALK -> LIGHT_BREAK -> EXPOSED_5S
       |-- MISS/TIMEOUT -> RESHIELD -> RECOVERY -> CYCLE_1 (lampu belum habis)
       +-- VALID_BLITZ -> sealCount=1 (RAM) -> INTERLUDE
  -> CYCLE_2: trick audio + UV, sumber baru -> EXPOSED_5S -> sealCount=2 (RAM)
       |-- MISS/TIMEOUT -> RESHIELD -> RECOVERY -> CYCLE_2
  -> CYCLE_3: PULL_CLOTH + FALSE_MOTHER_DAMAR
       -> BREAK_PULL (flare OPSIONAL, UV KNOT, atau beam generator yang masih ada)
       -> LIGHT_BREAK sumber yang tersedia -> EXPOSED_5S -> VALID_BLITZ
       -> sealCount=3 -> CP6 COMMIT ATOMIK -> boss RESOLVED, pintu terbuka, epilog
```

**Aturan sumber cahaya [KLARIFIKASI].** Generator akses **`G-ACC` bukan** sumber boss. Tiga sumber arena `G-N`, `G-S`, `G-E` punya `available` → `triggered` → `used` hanya **setelah Blitz sukses**. Jika tuas ditarik ketika Wewe di luar *beam*, beri bunyi dan cooldown pendek; generator **tetap available**. Jika Wewe terkena beam tetapi Blitz meleset atau window habis, perisai pulih, generator **tetap available** sesudah cooldown. Setelah hit valid, generator pemicunya korslet; generator yang sudah `used` tidak dapat menghasilkan segel tambahan pada attempt yang sama. **Ketiganya memberikan tiga peluang sukses tanpa flare** dengan urutan bebas. Jika sebuah flare membuka perisai dan menghasilkan hit sah, poin siklus bertambah; generator yang belum digunakan tetap available. Flare adalah **consumable per lemparan**, walau Blitz meleset; flare yang sama tidak dapat menghasilkan dua poin segel.

**Keabsahan hit [BASELINE UJI].** Boss asli (bukan ilusi) harus berada dalam volume beam aktif (radius target awal 2,5 m) atau radius flare (3,0 m selama ≥0,3 s) sehingga kabut pecah. Saat `EXPOSED` Wewe berlutut, hitbox wajah/dada jelas, **diam dari menyerang, melompat, memindahkan posisi, atau membuat papan rapuh baru selama 5,0 detik**. Blitz baru dinilai bila baterai ≥15 poin, cooldown global 6 detik telah selesai, kamera aktif, LOS dan target hit sah (≤8 m dan toleransi bidik ±20° sebagai baseline). Satu window **maksimal satu** sukses; `P3_BLITZ_SUCCESS` adalah state **RAM saja**, bukan receipt permanen. Pause/alt-tab kehilangan fokus tidak menghabiskan jendela ketika input gameplay tidak tersedia. Spam klik tidak memotong dua kali biaya atau memberi dua poin.

**Siklus 3: tarikan kain [REVISI EDITORIAL + BASELINE UJI].** Setelah dua segel, kain gendongan mencoba menarik Raka ke tepi tungku dengan telegraph bunyi seret, bayangan kain, dan suara palsu Ibu Raka/Damar. Tiga counter yang setara tersedia: (1) **flare** yang dilempar tepat ke simpul kain; (2) **Lensa UV Rajah** diarahkan stabil ke simpul ritual kain selama **1,0 detik** ketika baterai cukup; (3) **sinar generator yang masih tersedia** ketika pemain mengarahkan tarikan/kain ke cone lampu. Counter UV/flare menghentikan tarikan, **tidak otomatis membuka perisai**—pemain tetap harus melakukan `LIGHT_BREAK` dengan generator/flare sesudahnya. Counter sinar generator boleh memutus kain dan memecahkan perisai **hanya bila Wewe asli juga berada dalam volume sorotan**. Tanpa baterai, counter ketiga tetap tersedia dan dapat dikerjakan dari rute kokoh sambil menuju pengisian manual; pemain tidak pernah dituntut membuang flare terakhir. `PULL_IN` fatal hanya bila pemain mengabaikan semua counter hingga benar-benar terseret ke tepian; jarak awal, kecepatan dan telegraph wajib dibuktikan lewat playtest.

**Generator yang dipakai pada siklus 1–2 memang mati.** Bila kedua poin awal diraih dengan flare sehingga masih ada tiga generator, **jangan mematikan dua generator yang belum dipakai hanya untuk memenuhi kalimat sinematik**. Visual Wewe dapat membuatnya berkedip, bukan menghilangkan peluang menang. Pada siklus 3 seluruh sumber yang `available` tetap dapat dipakai; tidak ada aturan artifisial "hanya satu generator hidup". Bila siklus 1–2 memakai N dan S, hanya E yang tersisa, persis seperti blocking asli. Sistem tidak boleh mengasumsikan siklus selalu N→S→E, karena urutan bebas/flare adalah pilihan pemain.

**Pemulihan fisik dari baterai 0 dan miss berulang [KLARIFIKASI].** Arena memiliki dua titik daya yang dicapai dari **jalur kokoh**, dan satu **alcove servis** di sisi Barat. Dalam `PREP`/`INTERLUDE`, interaksi fisik `E` selama 8 s mengisi **+25 poin** per jatah awal fase (dua titik berbagi batas fase sesuai konfigurasi). Jika pemain meleset hingga kehabisan jatah, pemain dapat mencapai alcove ketika boss sedang `RESHIELD/RECOVERY`, menutup pintu, lalu mengengkol reset **8 detik** (cooldown 20 detik gameplay setelah reset berhasil). Reset mengaktifkan kembali jatah **pengisian daya**, **bukan** generator cahaya yang telah `used`. Tidak ada energi yang muncul otomatis dari UI atau timer. Saat pintu alcove tertutup, boss tidak dapat menyerang; pemain juga **tidak dapat menembak Blitz atau melukai boss** dari dalam. Pintu dapat dibuka kembali menuju rute aman. Test wajib membuktikan pemain pada baterai 0% **tanpa flare/powerbank** dapat mencapai alcove dan menang bahkan setelah banyak miss; jika blockout membuat alcove tidak terjangkau, desain arena harus diperbaiki, bukan menaikkan baterai snapshot diam-diam. Stasiun tidak dapat mengisi HP bila boss `EXPOSED` agar jendela tidak bisa dibekukan/dieksploitasi.

**Status fight dan retry.** Semua sumber lampu, jumlah flare dipakai, papan runtuh, boss phase, tarikan kain, poin segel, dan durasi berada di **RAM attempt**. **Dilarang segala save selama `BOSS_ACTIVE`**: death/force quit/reload mengembalikan CP5 persis dengan baterai, flare, tiga pusaka, serta tiga generator sesuai snapshot praboss; tidak ada duplikasi. Setelah segel ketiga, `P3_BOSS_WIN` menulis CP6 **atomik** memindahkan pusaka ke dudukan permanen, menambah segel lokasi ketiga, mematikan Wewe, membuka pintu dan rute pulang. Jika commit CP6 gagal, victory belum ditampilkan permanen; CP5 adalah recovery yang valid. Recovery UI harus jujur dan bisa dicoba lagi.

### 4.5 Denah dan AI arena boss (BASELINE UJI)

Wewe memakai dua set lintasan: ceiling/truss untuk `ROOF_LEAP` dan jalan ring kokoh untuk `STALK`. Spawn drop di luar collision pemain dengan telegraph suara/partikel; jangan teleport langsung ke posisi player. Lampu sorot berlabel Utara, Selatan, Timur terlihat dari ring, serta rute menuju tuas masing-masing tetap lebar setelah lantai rapuh aktif. Pijakan `FRAGILE_FATAL` bertanda UV, namun **rute kokoh yang berbeda selalu tersedia**. `VOICE_WAVE` boleh mengaburkan visual tetapi tidak menutup indikator UV/EVP/Blitz yang wajib; gunakan opsi pengurangan blur/flash. Semasa interlude, gerbang luar tetap terkunci tetapi alcove servis dapat dipakai. Setelah tiga sukses, AI, damage dan ilusi berhenti; gerbang barat dan shortcut pascaboss aktif. Boss tidak pernah menyelesaikan ritual ketika satu sumber cahaya terkena dua kali.

**Baseline awal yang perlu diukur:** target tinggi Wewe hampir 3 m, kecepatan patroli sekitar 1,2 m/s, kecepatan kejar 3,0 m/s fase B, kecepatan stalk boss sekitar 2,0 m/s; telegraph kain ≥0,8 s; durasi sinar 2,5 s; jendela 5 s **kanon**; radius tuas 1,6 m; target hit ≤8 m; sinar generator 2,5 m; tarikan bergerak 1,0 m/s; baseline ≥10 m dari tepi dibatalkan karena tidak muat pada lebar ring aula. Tarikan dimulai saat Wewe berada dalam jangkauan bidik ≤8 m dan LOS pada lantai bawah, dengan telegraph ≥1,5 s serta kesempatan counter. PULL_IN memeriksa tepian fisik tungku sesudah 12 s, bukan radius kematian yang melampaui pagar. **Nilai bukan kelulusan:** uji dengan animator, navmesh, FPS 30/60, input RMB/FP/OTS, dan ukuran collider Wewe. Jika pemain mati tanpa tindakan salah, perbaiki blocking/telegraph alih-alih sekadar menambah grace instakill.

## 5. Bukti, Jurnal Klenik, inventori dan item

### 5.1 Master item dan dokumen (KANON)

| Jenis | Item / ID logis | Lokasi | Cara dapat | Fungsi |
|---|---|---|---|---|
| Dokumen bonus | Artikel Forum "Misteri 'Mbok Gombel'" — `P3_DOC_FORUM` | PC Kos | Baca | 9 anak, aturan bertahan, UV |
| Dokumen #1 | Buku Catatan Mandor Pabrik 1999 — `P3_DOC_MANDOR` | Ayunan gendongan | Baca | Tiga tumbal, Mbok Gombel, metode generator + UV |
| Dokumen #2 | Kartu Pers Damar + tulisan arang — `P3_DOC_DAMAR_CARD` | Gudang Ampas Tebu | Ambil/baca | Damar dipindah ke Karang Setra; peringatan chat |
| Story item | Sepatu Kiri Damar — `P3_SEPATU_DAMAR` | Gudang Ampas Tebu | Ambil bersama kartu | Dibawa Raka pada epilog |
| Pusaka #1 | Boneka Jerami Tebu — `P3_BONEKA_JERAMI` | Ayunan | Ambil | Dudukan altar |
| Pusaka #2 | Sabit Tebu Berukir Rajah — `P3_SABIT_RAJAH` | Meja kontrol Kantor Mandor | Ambil | Dudukan altar |
| Pusaka #3 | Selendang Batik Terbakar — `P3_SELENDANG_BATIK` | Gudang Ampas Tebu | Ambil | Dudukan altar |
| Bukti #1 | Jejak gendongan tumbal — `P3_GENDONGAN_TUMBAL` | Ayunan | Foto dengan AR | Flag Jurnal |
| Bukti #2 | Frekuensi EVP nina bobo — `P3_EVP_NINA_BOBO` | Interkom Kantor Mandor | Rekam segmen ASLI | Flag Jurnal |
| Bukti #3 | Rajah UV penyembunyi anak — `P3_RAJAH_ANAK` | Dinding Gudang Ampas | Foto dengan UV | Flag Jurnal |
| Rahasia | Pelat Nomor Buruh 09 — `SECRET_P3_PELAT_BURUH` | Loker 09, balkon servis | UV + EVP + katup + pickup | Arsip Saksi (N dari 5) |
| Consumable | Powerbank Besar (3 instance Pasar Gaib) | Inventori | Beli opsional | +80 poin |
| Consumable | Flare Industri (4 dari Pasar Gaib; rak arena) | Inventori/rak | Beli opsional / rak | Pemecah perisai, pemutus tarikan, penerang |
| Consumable | Kemenyan Bakar (2) | Inventori | Beli opsional | Tidak berfungsi di P3 |

**[KLARIFIKASI]** `Bukti #1` ialah foto AR; Boneka Jerami adalah item fisik terpisah walau satu lokasi (dua flag). `Bukti #3` ialah foto UV; Selendang adalah item fisik terpisah. `secret_items` menggunakan **list ID unik pada save / set runtime turunan** agar pengambilan ulang tidak menaikkan progres. Dua dokumen bernomor (#1 dan #2) mendukung lore dan clue; jangan gunakan jumlah dokumen sebagai pengganti tiga bukti.

### 5.1A Item rahasia Babak 3: Pelat Nomor Buruh 09 (KANON + KLARIFIKASI)

| Elemen | Ketentuan |
|---|---|
| Nama dan ID | **Pelat Nomor Buruh 09**, `SECRET_P3_PELAT_BURUH`; opsional; satu dari lima bagian Arsip Saksi |
| Lokasi | **Loker buruh 09** di **balkon servis belakang Kantor Mandor**, di luar aula (E11); dapat dikunjungi sebelum dan sesudah boss |
| Petunjuk lingkungan | **Ketukan tiga kali dari saluran ventilasi** menghasilkan **gelombang EVP hijau** (`ASLI`); tulisan UV pada pagar berbunyi **"Nomor sembilan tidak pulang"** |
| Langkah | (1) Dengar ketukan tiga kali dari ventilasi balkon. (2) Verifikasi dengan EVP v2: hijau `ASLI`. (3) Telusuri **panah UV** pada balkon yang kokoh (bukan papan rapuh bersilang). (4) Putar **katup kecil berlabel 09** (`E`, tahan 1,0 s). (5) Loker terbuka; ambil pelat. **Suara Damar merah** yang memanggil dari jalur rapuh adalah **pengalih** |
| Isi kesaksian | Pelat memuat **sembilan nama anak buruh yang hilang**; **tiga korban ritual awal** dan **enam korban berikutnya** menjelaskan total **sembilan korban tahun 1999** (E9) |
| Penerimaan | Tidak dihitung sebagai bukti atau pusaka boss; **tidak ada saweran** yang menentukan aksesnya |
| Persistensi | Saat `P3_SECRET_PICKUP`: tambah ID ke `secret_items` **sekali**, **autosave atomik segera**, baru tampilkan indikator sukses dan sembunyikan objek dunia. Bila save gagal, objek tetap ada untuk dicoba lagi |
| Tampilan | Jurnal dan panel motor: `Arsip Babak 3: 0/1` atau `1/1`; Arsip Saksi global `N/5` dari **jumlah aktual ID unik** (E10). Jangan memakai nomor babak |
| Kematian/reload | Item tetap tersimpan pada checkpoint valid terbaru; kematian tidak menghapusnya |

**[KLARIFIKASI — mengapa petunjuknya tidak bocor lewat jalur utama]** Pemain jalur utama bisa tidak pernah naik ke balkon: Kantor Mandor hanya menyimpan interkom/Sabit/dinamo, sedangkan ketukan, panah UV, teks pagar, dan katup 09 hanya ada di balkon. Tidak ada angka atau nama "09" di dokumen jalur utama selain pada dinding balkon dan label loker. Pengalih suara merah tidak fatal (`FRAGILE_DROP`, §3.4).

### 5.2 Journal data contract

**[KANON]** Jurnal menampilkan tiga kotak: `[ ] Jejak Gendongan Tumbal (AR)`, `[ ] Frekuensi EVP Nina Bobo`, `[ ] Rajah UV Penyembunyi Anak` dan status collectible `0/1` atau `1/1` pada babak ini. Arsip Saksi global adalah `N/5` dari **jumlah aktual ID unik yang sudah dimiliki**, bukan nomor babak (contoh: P1 dan P2 sudah diambil → 3/5 setelah P3; hanya P1 → 2/5; tidak ada → 1/5).

**[USUL BARU]** Jurnal memiliki empat tab konseptual: **Bukti**, **Dokumen**, **Arsip Saksi**, **Petunjuk Objektif**. Pada akhir tiga bukti, tampilkan kesimpulan metode: `[ENTITAS TERIDENTIFIKASI: WEWE GOMBEL (IBU ASUH SEMU)]` dan `[METODE EKSORSISME ...]`; pemain menekan **Konfirmasi identitas** untuk mencatat `P3_JOURNAL_CONFIRMED`; jangan otomatis menyamakan tiga scan dengan konfirmasi. Jangan menampilkan lokasi rahasia atau solusi katup 09 dari awal.

### 5.3 Status ekonomi dan live stream

**[KANON]** Angka tetap dari naskah: penonton awal **185.000** (E8), akhir epilog **666.000**; donasi `Penonton_Terakhir` **Rp3.333.333** (halaman) dan **Rp9.999.999** (epilog); overlay **Total Saweran Kumulatif Rp312.500.000**.

**[KLARIFIKASI]** Bedakan empat hal secara teknis, seperti P1 §5.3 dan P2 §5.3: (a) **uang tersedia** (`wallet_legitimate`); (b) **dana terikat** `Penonton_Terakhir` (`wallet_bound`); (c) **total saweran kumulatif untuk UI cerita**; (d) **reward yang benar-benar diperoleh**. Kedua donasi `Penonton_Terakhir` di babak ini adalah **peristiwa cerita dari sumber terikat**: tampil sebagai notifikasi, tidak menambah `wallet_legitimate`, dan tidak diproses ulang saat reload.

**Selisih matematika yang harus diketahui (bukan event donatur yang terbukti ada):**

| Komponen | Nominal |
|---|---:|
| Milestone kumulatif akhir P3 (naskah) | Rp312.500.000 |
| Milestone kumulatif akhir P2 (naskah) | Rp164.800.000 |
| **Selisih milestone P2→P3** | **Rp147.700.000** |
| Donasi `Penonton_Terakhir` yang tertulis di P3 (3.333.333 + 9.999.999) | Rp13.333.332 |
| **Sisa yang belum ditulis sebagai transaksi mana pun** | **Rp134.366.668** |

Asumsi hitungan: Rp312,5 juta mencakup Rp164,8 juta milestone P2 dan kedua donasi terikat P3 (pola yang sama dengan Rp10 juta pembuka P1 dan Rp8.665.666 P2). Belanja Pasar Gaib (maks. Rp2.100.000) adalah pengeluaran, bukan donasi. **Jangan menciptakan saldo atau transaksi penutup selisih.** Apakah overlay Rp312,5 juta adalah sinematik tetap atau angka transaksi dinamis adalah **P3-D03**.

**[KLARIFIKASI — dampak ke Pasar Gaib]** Naskah menyebut belanja memakai "uang saweran dari Babak 2". Karena ledger P1/P2 belum lengkap, saldo `wallet_legitimate` yang tiba di Kos adalah **apa pun yang ada di save**, bisa nol. Karena itu seluruh paket **opsional** dan jalur utama tidak boleh menganggap saldo cukup (QA-01, QA-43).

**[KANON + USUL BARU]** Tidak ada *paywall*; tidak perlu donasi untuk akses, bukti, pusaka, atau boss. Live chat berupa skrip lokal deterministik terhubung event (§7.4), bukan jaringan sungguhan; jalur utama tidak boleh bergantung pada audiens atau koneksi internet.

### 5.4 Katalog Pasar Gaib dan status tantangan saweran (KLARIFIKASI + BASELINE UJI)

**Tidak ada tantangan saweran opsional di Babak 3.** Naskah tidak memuat padanan `P1_SAWER_SIGNBOARD`; jangan menambah `P3_SAWER_*` tanpa keputusan baru. Satu-satunya interaksi uang yang dapat dimainkan adalah belanja Pasar Gaib.

| Paket | ID transaksi | Harga | Isi | Batas |
|---|---|---:|---|---|
| Baterai Cadangan / Powerbank Besar ×3 | `P3_SHOP_PACK_POWERBANK` | Rp750.000 | 3 instance powerbank besar (+80 poin) | Sekali per kunjungan Safe Hub |
| Flare Industri ×4 | `P3_SHOP_PACK_FLARE` | Rp1.000.000 | 4 flare lempar | Sekali per kunjungan |
| Kemenyan Bakar ×2 | `P3_SHOP_PACK_KEMENYAN` | Rp350.000 | 2 consumable tanpa fungsi di P3 | Sekali per kunjungan |
| **Total bila semua dibeli** | | **Rp2.100.000** | | |

**Aturan:** (1) dua upgrade wajib (UV Rajah, EVP v2) **tidak dijual** dan terbuka lewat `P3_UPGRADE_UNLOCK`; (2) pembayaran hanya dari `wallet_legitimate`; (3) saldo kurang berarti paket tampil tidak aktif dengan harga terlihat, **tanpa paket gratis pengganti**; (4) pembelian adalah **satu transaksi atomik**: kurangi saldo, tambah instance inventori, tulis receipt (§6.4); (5) memuat ulang di tengah belanja tidak menggandakan barang maupun memotong saldo dua kali; (6) **harga bukan kanon** (naskah tidak menyebut nominal); jumlah per paket mengikuti naskah.

---

## 6. Sistem penyimpanan dan *campaign state*

### 6.1 Prinsip penyimpanan (KANON + KLARIFIKASI)

- Tiga bukti unik, tiga pusaka, status boss, segel lokasi, dokumen, inventori, uang tersedia, dana terikat, Sanity, upgrade, dan Arsip Saksi **harus konsisten setelah reload**.
- Item `SECRET_P3_PELAT_BURUH` ditambah **sekali** ke daftar ID global `secret_items`, lalu **autosave segera**. Kematian/reload tidak boleh menghapus item yang sudah tersimpan pada checkpoint valid.
- Status objek di scene (**gerbang magnetik, ayunan, katup 09 dan loker, dudukan altar, generator arena, gerbang aula, gerbang servis, rak flare**) direkonstruksi dari data, bukan asumsi default prefab.
- **[KLARIFIKASI]** Saat menyimpan, gunakan sistem `CampaignState` dan mekanisme atomic save yang **sudah ada**; jangan membuat sistem save paralel. Data spesifik babak disimpan sebagai bagian dari save yang sama.
- **[USUL BARU]** Bedakan progress persisten (barang/bukti/rahasia/boss) dengan *transient runtime* (timer 5 s, posisi Wewe, telegraph papan rapuh, progres engkol, tarikan kain, flare terbang) agar reload tidak memuat kondisi mustahil.

### 6.2 Skema save tanpa sumber kebenaran ganda (USUL BARU)

**Simpan set ID, bukan boolean turunan ganda.** `completed_event_ids` adalah event satu kali; `evidence_ids` = bukti unik; `document_ids` = dokumen; `CampaignData.inventory` = pusaka/kunci/instance consumable yang sedang dibawa; `relic_ids` = pusaka yang sudah ditemukan; `secret_items` = collectible global. `stage`, `hasBoneka`, `hasSabit`, `hasSelendang`, `boss.started`, `local_secret_status` **diturunkan** dari data itu saat runtime. **Progres generator boss, `P3_DECOY_INTERACT`, `P3_BLITZ_SUCCESS`, status ilusi/papan rapuh baru, tarikan kain, dan jumlah flare yang terpakai pada attempt ini hanya hidup di memori percobaan**, dan harus hilang saat respawn CP5; tidak boleh disimpan ke disk bersama snapshot CP5. Event `P3_BOSS_WIN` dan `location_seals` harus dimutasi atomik agar tidak ada segel hilang setelah reload.

```json
{
  "schema_version": 4,
  "chapter_id": 3,
  "checkpoint_id": "PHASE3_CP4",
  "inventory": ["SMARTPHONE_RAKA", "P3_BONEKA_JERAMI", "P3_FLARE_A", "P3_POWERBANK_PACK_1"],
  "evidence_ids": ["P3_GENDONGAN_TUMBAL"],
  "relic_ids": ["P3_BONEKA_JERAMI"],
  "secret_items": ["SECRET_P1_NEGATIF_TARI", "SECRET_P2_LABEL_PASIEN"],
  "location_seals": ["SEAL_RUSUNAWA_GRIYA_ASRI", "SEAL_RSUD_LAMA_SIDO_WARAS"],
  "battery": 71,
  "sanity": 100,
  "wallet_legitimate": 0,
  "wallet_bound": 0,
  "spent": 0,
  "phase3": {
    "completed_event_ids": ["P3_UPGRADE_UNLOCK", "P3_DEPART_KOS", "P3_START_LIVE", "P3_ACCESS_GENERATOR", "P3_TAKE_BONEKA"],
    "document_ids": ["P3_DOC_FORUM", "P3_DOC_MANDOR"],
    "shop_receipts": [{"id": "P3_SHOP_PACK_FLARE", "tx": "P3_TX_0001"}],
    "kos_departure_charge_used": true,
    "dynamo_full_charge_used_ids": ["P3_DYNAMO_POS_TIMBANG"],
    "dynamo_emergency_last_uses": [],
    "elapsed_gameplay_seconds": 1380,
    "money_ledger": []
  }
}
```

**[AUDIT — format usulan, wajib diverifikasi]** Skema di atas bukan API/format loader yang terverifikasi. Phase 1 menyebut schema 2 dan Phase 2 mengusulkan schema 3; contoh P3 memakai **schema 4 secara ilustratif**, **bukan instruksi memaksa 3→4**. Periksa loader, skema aktual, dan kebutuhan migrasi melalui audit P3-D10. Jangan sekadar mengganti angka versi: migrasi harus mempertahankan flag P0, seluruh field P1 dan P2, dan semua ID rahasia. List receipt dipilih karena `JsonUtility` bukan serializer dictionary/HashSet; runtime boleh membangun set/index dari list, tetapi jangan persist dua salinan nilai yang sama. `evidence_ids`, `inventory`, `battery`, `sanity`, dan `checkpoint_id` tetap milik campaign existing; jangan membuat duplikat di `phase3`.

**Angka dan ID dalam JSON adalah contoh snapshot CP4 (bukan inventori awal kanon).** `kos_departure_charge_used` menandai pengisian penuh sekali per keberangkatan (P3-D02); `dynamo_full_charge_used_ids` menyimpan satu pengisian penuh per dinamo; waktu pemulihan darurat memakai *elapsed gameplay time* (bukan jam dinding OS). Timer 0,6/1,2 s papan rapuh, 5 s window, 6 s loiter, jendela belajar 30 s, dan telegraph lain tidak disimpan. Save item rahasia tetap langsung dan sekali saja. `money_ledger` memuat transaksi unik (ID event + nominal); saldo hanya turunan atau field resmi ekonomi proyek.

### 6.3 Checkpoint yang diusulkan (USUL BARU)

| Checkpoint | Waktu yang baik | State minimal | Recovery |
|---|---|---|---|
| `PHASE3_CP1` | Setelah kontrol dikembalikan di halaman (`P3_ARRIVE_PABRIK`) | Berangkat, upgrade, belanja, baterai aktual setelah colokan Kos manual sekali | Intro halaman tidak perlu diputar ulang bila tercatat |
| `PHASE3_CP2` | **Masuk Gudang Gilingan, sebelum jebakan suara #1** | Anchor pintu gilingan; papan rapuh aktif; baterai nilai saat itu | Retry memuat snapshot tanpa top-up; suara diputar ulang sebagai telegraph |
| `PHASE3_CP3` | **Melewati gerbang magnetik ke landing Bangsal, sebelum `P3_WEWE_REVEAL`** | Generator Akses selesai; bukti/pusaka/dokumen yang benar-benar diperoleh (boleh belum ada Bukti 1); Wewe `DORMANT` | Retry memuat snapshot tanpa top-up; reveal diputar ulang sebagai telegraph, bukan fatal |
| `PHASE3_CP4` | Setelah tiap bukti atau pusaka valid | Set bukti unik, pusaka, dokumen, kunci; **anchor aman**, bukan posisi saat dikejar | Hindari re-scan; Wewe `DORMANT`/patroli jauh |
| `PHASE3_CP5` | **`E` pertama yang valid pada Karung Damar Palsu** (3 bukti + konfirmasi Jurnal + 3 dudukan terisi); **sebelum** gerbang membanting dan Wewe turun | Tiga pusaka **masih di inventori**; saat restore CP5, preview dudukan dibatalkan, Raka berada **sebelum** `E` pemicu untuk memasang ulang jika diperlukan, tiga generator utuh, flare sesuai inventori, baterai aktual, Wewe belum aktif | Retry selalu dari snapshot ini; flare terpakai attempt gagal dipulihkan; tanpa floor baterai |
| `PHASE3_CP6` | **`P3_BOSS_WIN`, transaksi atomik** | Boss kalah + segel ketiga + pusaka pindah ke dudukan permanen + flare terpakai dikurangi + gerbang servis terbuka | Kembali eksplorasi tanpa boss berulang |
| `PHASE3_CP7` | `P3_COMPLETE` | Semua loot, rahasia, ekonomi, segel, tujuan Karang Setra | Memulai babak berikutnya tanpa duplikasi |

**[ARGON — koreksi urutan checkpoint CP2/CP3/CP4]** **CP1** = halaman setelah perjalanan; **CP2** = pintu masuk gilingan **sebelum jebakan suara pertama**, bukan setelah menyalakan G-ACC seperti proposal Argon; **CP3** = landing masuk Bangsal sebelum reveal; **CP4** = snapshot aman ketika memperoleh bukti/pusaka Bangsal. Bukti #1 AR, Boneka Jerami, dan Catatan Mandor yang diambil di gilingan harus di-*commit* **segera** sebagai transaksi data progress, tetapi **belum diberi ID CP4**; pada masuk Bangsal CP3 mengakumulasi seluruh receipt tersebut. Tidak boleh membuat CP4 lalu menimpa CP3 lebih tua (checkpoint regression). Jika player mati setelah mengambil Boneka namun sebelum masuk Bangsal, receipt yang sudah sukses disimpan tidak hilang; anchor pemulihan ke gilingan terdekat dan objek dunia mengikuti receipt. QA-127/QA-143 membuktikan urutan ini dan bagaimana migrasi dari checkpoint sebelumnya bekerja. Apabila sistem existing tidak dapat mempertahankan receipt terpisah dari checkpoint, Codex wajib mengusulkan satu solusi berbukti kode (mis. commit progress pada CP2 dengan anchor sama), **bukan** silently drop pickup.

**Anchor CP4 [KLARIFIKASI].** CP4 tidak boleh ditulis dengan posisi pemain saat Wewe berstatus `CHASE`/`AMBUSH_WAIT`. Receipt bukti/pusaka dan penambahan inventori tetap di-commit atomik **segera**; hanya *anchor posisi restore* yang memakai titik aman terdekat: landing Bangsal, depan Kantor Mandor, pintu Gudang Ampas, atau tangga L2. Restore dari CP4 menempatkan pemain di anchor dengan Wewe jauh/`PATROL` pada jendela belajar singkat.

**[KLARIFIKASI — atomic checkpoint CP5]** Saat pemain berdiri dalam radius karung dan menekan `E` pertama kali dengan tiga bukti + `P3_JOURNAL_CONFIRMED` + tiga dudukan terisi (`ALTAR_READY` runtime), simpan CP5 **dari posisi tepat di depan karung** ke disk **sebelum** gerbang membanting, Wewe turun, atau dudukan dikunci. Tiga pusaka **tetap tercatat di inventori**; tampilan dudukan sebelum attempt tidak perlu dipersist; saat restore CP5 tempatkan pusaka ke inventori dan berikan interaksi ulang yang aman. `P3_DECOY_INTERACT`, posisi boss, progres siklus, status generator, dan jumlah flare terpakai hanya hidup di memori percobaan. **Jangan autosave atau menulis ke disk selama `BOSS_ACTIVE`**, termasuk saat Blitz sukses 1–2 kali, pause, atau cutscene tengah fight. Kematian, restart game, atau reload manual sebelum menang selalu mengembalikan CP5. Bila penulisan CP5 gagal, pemicu tidak dimulai (UI gagal simpan, tombol coba lagi). Setelah 3 Blitz sukses, `P3_BOSS_WIN` melakukan **satu commit atomik CP6**; penulisan gagal mengembalikan snapshot terakhir yang valid (CP5/CP6) bukan state separuh jalan.

**[BASELINE UJI]** Saat kalah, reset **semua tiga generator dan seluruh flare** dari CP5 dengan baterai persis snapshot, tanpa menduplikasi item/uang. Jangan mencampur reset penuh dengan checkpoint parsial 1–2 poin dalam satu versi implementasi (P3-D08).

### 6.4 Idempoten dan transaksi sederhana (KLARIFIKASI)

```text
if EvidenceIds.Add("P3_GENDONGAN_TUMBAL") == newlyAdded:
    Journal.RecalculateUniqueEvidenceCount()
    RequestSave()

if EvidenceIds.ContainsAll(RequiredEvidenceIds) AND Completed("P3_JOURNAL_CONFIRMED") AND Inventory.ContainsAll(Pusaka3) AND DudukanFilledCount == 3 AND PlayerPressedInteractAtDecoy AND PlayerWithinDecoyRange:
    if EnsureDiskCheckpoint("CP5", playerAtDecoy=true, pusakaInInventory=true) == SUCCESS:
        StartBossAttemptInMemoryOnly()                       // gerbang membanting secara runtime

if bossIsExposed AND validChestOrFaceHit AND not runtimeSource[activeSourceId].completed:
    runtimeSource[activeSourceId].completed = true        // jangan tulis ke disk selama boss
    if CountDistinctRuntimeSources() == 3:
        CommitAtomic("CP6", bossWin=true, pusakaAtAltar=true, addLocationSeal=true, applyFlareConsumed=true)
        TriggerVictoryEffectsOnce()

if BossAttemptFailedOrReloaded:
    RestoreDiskCheckpoint("CP5")                        // 3 generator utuh; flare = snapshot

if ShopPurchase(pack):
    CommitAtomic(deductWalletLegit, addInventoryInstances, writeReceipt(pack, uniqueTx))
    // reload di tengah belanja tidak menggandakan barang maupun memotong saldo dua kali

if MotorExitPanelOpened:
    ShowArchiveStatusNoSave()                       // tutup panel = tanpa completed event
if PlayerConfirmedExit AND EpilogueComplete AND not Completed("P3_COMPLETE"):
    if CommitAtomic("CP7", addCompletedEvent="P3_COMPLETE", nextDestination="Desa Mati Karang Setra") == SUCCESS:
        FadeToBlackWithCkrek()                      // gagal commit => panel tetap bisa dicoba lagi

if TryAddSecretAndCommitSameCampaign("SECRET_P3_PELAT_BURUH") == COMMITTED:
    ShowCollectedAndHideWorldObject()
else if SAVE_FAILED:
    RestorePreviousDataAndKeepWorldObjectForRetry()
```

Pseudocode mengilustrasikan *state transition*, bukan API existing. Proyek memakai panggilan director + receipt ID di `CampaignData.inventory`, belum bus event generik. `SaveCheckpointAtomic` mengamankan file tetapi hanya mengembalikan `checkpoint_id` bila gagal; mutasi lain memerlukan rollback lengkap seperti pola `AtmajaDirector.Commit`. Perluas pola itu untuk transaksi P3 di save yang sama, bukan membuat penyimpanan paralel. Selama boss, lindungi seluruh jalur tulis (termasuk `ArchiveDocument`, pembayaran Bank, callback UI), bukan hanya autosave director. Indikator collectible sukses baru tampil setelah write berhasil; failure mengembalikan set/objek/UI untuk retry.

---

## 7. Presentasi naratif, atmosfer dan aset

### 7.1 Daftar beats sinematik (KANON + KLARIFIKASI)

| Adegan | Beat audio/visual | Tujuan dramatis |
|---|---|---|
| Kos | Peta Jawa Barat bertali, siang berdebu, layar forum | Memperlihatkan Raka makin sistematis dan lelah |
| Halaman | Cerobong bata, angin, derit lori, layar HP satu-satunya cahaya | Membuat Raka kecil di hadapan industri raksasa |
| Chat #1 + donasi | Notifikasi merah, "Sahabatmu kedinginan..." | Umpan emosional sebelum jebakan |
| Gilingan | Teriakan "Damar" dari kegelapan, gelombang merah di layar | Mengajarkan: jangan percaya telinga |
| Jalur pipa + UV | Rajah silang ungu pada papan, debu jatuh | Penghargaan membaca lantai |
| Generator Akses | Engkol diesel, lampu sorot kuning menyala, ayunan kain tampak | Cahaya sebagai akses |
| Bangsal | Nina bobo serak, "BRUK!", Wewe turun, uap | Pergeseran menjadi predator stealth |
| Kantor Mandor | Statis interkom, nina bobo merah lalu rintihan hijau | Mengikat bukti dengan pembeda suara |
| Gudang Ampas | UV menyingkap rajah darah, Kartu Pers dan sepatu | Menyentuh sisi manusiawi dan petunjuk Damar |
| Aula | Tiga pusaka di dudukan, Damar palsu, gerbang membanting | Jebakan emosional |
| Boss | Kabut terbakar, generator korslet, bidikan dada dan wajah, abu kain batik | Penghargaan pembacaan mekanik |
| Epilog | Hening, chat serempak, tawa Mbah Suro | Menarik pemain ke Karang Setra; foreshadow Babak 4 |
| Exit | Motor di halaman, panel arsip, `CKREK!`, hitam | Transisi sadar |

### 7.2 Prop dan art direction spesifik (USUL BARU)

**Halaman:** timbangan jembatan tebu, lori berkarat, papan nama pabrik 1912, cerobong bata, tumpukan karung, motor Raka. **Gilingan:** roda gigi besar, panggung kayu bertingkat, catwalk besi, jalur pipa kanan dengan pegangan, lampu darurat redup. **Bangsal:** ketel silinder raksasa berpelat paku keling, katup uap, kepompong kain goni (sebagai siluet, tidak grafis), loker besi pekerja, lubang tungku mati, tangga besi. **Kantor Mandor:** meja kontrol, radio interkom, peta pabrik tua, kalender 1999, balkon servis dengan loker bernomor 01–12. **Gudang Ampas:** tumpukan ampas, dinding bata lembap, lukisan rajah darah (hanya UV). **Aula:** tungku batu bara melingkar, jembatan besi bertingkat, tiga lampu sorot pabrik bergenerator diesel, kursi besi, rak flare/daya, gerbang servis dilas.

**[USUL BARU]** Motif berulang: **angka 9** (loker 09, sembilan nama), kain batik, kepompong goni, sepatu anak kecil, ketukan tiga kali. Jangan seluruh dinding bertuliskan petunjuk UV terang; rajah UV adalah kejutan lokal. Semua teks puzzle terbaca pada target resolusi dan cocok dengan lokalisasi bahasa Indonesia. **Rajah silang** harus punya bentuk yang jelas dan konsisten di semua area.

### 7.3 Audio dan VO (KLARIFIKASI + USUL BARU)

**WAJIB konten naskah:** monolog Raka (Kos, halaman, Gilingan, Bangsal, epilog), sapaan live, suara "Damar" tiruan, tangisan bayi, nina bobo Wewe, rintihan anak (segmen asli), suara palsu Ibu Raka dan Damar pada tarikan kain, tawa Mbah Suro, `BRUK!`, `CKREK!`, generator diesel dan lampu sorot, ledakan korslet. Suara nonverbal harus ikut berjalan walau subtitle/dialogue di-skip.

**[USUL BARU]** Gunakan 3 lapisan ambience (cuaca/ruang, mekanikal-industri, gaib) dan *priority ducking* ketika VO/EVP penting. Subtitle Indonesia dengan identitas pembicara: `[Raka]`, `[Suara Tiruan — Damar]`, `[EVP — ASLI]`, `[EVP — TIRUAN]`, `[Mbah Suro]`. **Suara tiruan Damar harus dikenali sebagai tiruan dari timbre/filter**, bukan hanya dari label (agar pemain tanpa HP masih punya petunjuk audio). Audio cue Wewe harus memberi informasi jarak/arah secara konsisten (nina bobo makin keras mendekat), bukan efek acak semata. `TelegraphBus` tidak diredam oleh ambience (pola P2).

**[ARGON — bus metadata yang disatukan]** `AmbienceBus` (lori, uap, bara), `VoiceBus` (Raka/Mbah Suro), `TelegraphBus` (retak, `BRUK!`, kain, ketukan), dan `EvpSourceBus` (**AUTHENTIC/MIMIC/AMBIENT yang benar per sumber**) adalah tanggung jawab audio yang dapat dipetakan ke mixer existing; **bukan** perintah membuat empat AudioMixer independen. Telegraph harus tetap dapat didengar ketika Ambience penuh; jika proyek belum punya bus khusus, gunakan ducking/priority event yang ada. Uji subtitle dan audio saat alt-tab/reload, bukan hanya ketika suara tunggal diputar.

### 7.4 Live stream dan teks chat (KANON + KLARIFIKASI)

Rentetan chat asli tetap berada di **bagian Naskah Adegan**. Saat implementasi, gunakan *timeline* yang terhubung ke event (bukan menembakkan semua chat di `Start()`), dengan antrian UI yang dapat dibaca ulang. Donasi `Penonton_Terakhir` merupakan kejadian cerita. **[USUL BARU]** Tambahkan *history panel* di HP agar clue yang lewat dapat dibaca kembali. **Chat infiltrasi** di epilog dirender sebagai **burst serempak** dengan teks hampir identik, nama pengguna "normal" (E16), tanpa efek sistem.

| Chat | Event | Isi / catatan | Status |
|---|---|---|---|
| Chat #1 + donasi Rp3.333.333 | `P3_START_LIVE` | Penonton **185.000**; `@pabrik_hantu`, `@rina_cirebon`; umpan "lari ke tungku" | Dana terikat; tidak diproses ulang saat reload |
| Chat #2 | `P3_CROSS_CATWALK` atau jebakan #1 dilewati | `@dedi_kobuzer`, `@anak_teknik` | Konfirmasi pembelajaran |
| Chat epilog + infiltrasi | `P3_EPILOGUE` | 666.000; Rp312,5 juta; `@budi_santoso99`, `@ani_lestari`, `@rejeki_nomplok` | Narasi; tidak ada efek gameplay |
| Pesan `Penonton_Terakhir` + Rp9.999.999 | `P3_EPILOGUE` | Tiga segel; Karang Setra; Banaspati | Dana terikat |

### 7.5 Kualitas presentasi yang disarankan untuk proyek Unity (USUL BARU)

Project menggunakan **Unity 6.3 LTS, URP, Windows x86_64**, dan perlu diuji juga pada laptop kelas **Intel Iris Xe/RAM 8GB**. Dokumen ini tidak mengklaim performa scene telah diuji. Prinsip produksinya:

- **Aset wajib existing atau unduhan internet berlisensi.** Phase 3 memakai **pustaka lingkungan pabrik/industri tersendiri** (bukan furnitur, permukaan, atau housing lampu Rusunawa/RSUD), modul beton/bata/baja/pintu/tangga berlisensi, UV skala nyata, tekstur PBR 2K, dan dressing sesuai fungsi ruang. Sumber dan checksum dicatat (misalnya `Audit/phase3-environment-sources.json`, `Audit/phase3-asset-credits.md`, `Audit/phase3-audio-credits.md`; nama berkas mengikuti pola P1). Tidak membuat model, gambar, tekstur, atau gerak original sendiri.
- **Art pass:** blockout → *composition pass* → aset nyata/material industri Indonesia → set dressing/clutter → decals kotor/berkarat → lighting → VFX/audio → gameplay polish. Jangan menambah banyak ruangan low-quality hanya untuk durasi.
- **Penerangan diegetik:** lampu darurat, lampu lori, bohlam kantor, lampu sorot generator, layar HP, flare, cahaya bulan dari jendela tinggi; hindari ruangan terang tanpa sumber lampu. Frame horor gelap tetapi navigasi dan petunjuk tetap terbaca (jalur wajib punya cahaya minimum).
- **Biaya GPU:** uap (VFX transparan sederhana atau kartu animasi), kepompong goni (instancing), debu runtuh (pool), UV sebagai **overlay/shader pada layer khusus** dengan biaya terukur (P3-D11), shadow selektif, matikan lampu realtime tak terlihat.
- **Telegraph harus murah:** audio 3D, gelombang EVP, dan rajah silang lebih penting daripada partikel mahal. Gunakan pool untuk serpihan dan kendalikan jumlah rigidbody aktif.
- **Testing performa:** profil build Windows nyata dengan scene lengkap, termasuk aula dengan tiga lampu sorot + uap + chat UI + UV, dan save/load. Validasi headless adalah tambahan untuk logika, bukan pengganti evaluasi visual/fps dengan GPU.
- **[BASELINE UJI — visual acceptance yang bisa difoto]** Untuk tiap area wajib ambil screenshot pemain normal dan, bila relevan, layar UV dan layar EVP: halaman (cerobong/lori), gilingan (jalur kiri vs kanan, rajah silang), ayunan (AR), bangsal (ketel/kepompong), Kantor Mandor (interkom/Sabit), Gudang Ampas (rajah UV), balkon (panah UV), aula (tiga generator, dudukan).
- **Asal aset & inspirasi:** Phasmophobia, Outlast, Alan Wake, DreadOut adalah **referensi rasa mekanik**, bukan izin menyalin aset, model, audio, level, UI, atau kode proprietary.


**[MERGED — visual production acceptance]** Pabrik bukan greybox yang hanya lolos collision: foto per area wajib memperlihatkan siluet cerobong dan lori/gerbong tebu, detail gilingan yang masuk akal, boiler dan rangka besi, kantor mandor berjejak 1999, gudang bagasse yang berfungsi, tiga generator dengan housing/lampu nyata, serta sumber cahaya diegetik. Minimal target asset-diversity (mis. ≥30 model prop industri berbeda) adalah **usulan mutu yang harus diverifikasi** terhadap aset tersedia, **bukan** angka kanon atau izin mengisi area dengan duplikat. Manifest lisensi, hash, bounding/placement, material dan bukti screenshot di Windows harus memungkinkan reviewer menolak hasil yang sekadar ruangan kosong. Utamakan set dressing bermakna dan jalur manusiawi ketimbang menambah ruangan untuk durasi.

**[ARGON — art acceptance dan teknik GPU]** Target produksi **≥30 model prop industri/pabrik gula yang benar-benar berbeda** dan ditempatkan di scene sebagai *stretch acceptance* (P3-D16), tidak menghitung duplikat, modul bangunan, material, serta prop pinjaman dari Rusunawa/RSUD. Manifest mencatat sumber URL, jenis lisensi, `SHA-256`, *prefab/scene usage*, skala dan posisi; jangan menganggap prop dari paket tidak berlisensi boleh dipakai. Gunakan lori tebu berel sempit, timbangan kolonial, manometer, boiler berpaku keling, loker bernomor dan fixture lampu yang ada sebab naratif/fungsional. **UV bukan otomatis gratis di GPU**: opsi spotlight + layer/shader keyword dari Argon adalah kandidat untuk diuji, bukan jaminan “biaya GPU mendekati nol”. Ukur overdraw transparansi uap, shadow tiga lampu, emissive/decal UV, serta biaya rig OTS/FP/RMB pada build Windows; pilih pendekatan rendering yang kompatibel dengan sistem kamera yang benar-benar ada di repository. Hanya deklarasikan PASS setelah *screenshot* dan performa aktual diperiksa.

**[PERLU KEPUTUSAN]** FPS final, budget draw call, VRAM, batas ukuran build, dan kualitas material belum dinyatakan dalam naskah asal. §10.12 memberikan *baseline* benchmark awal; validasi dengan build Windows nyata sebelum mengunci acceptance final (P3-D11).

---

## 8. Anti-softlock, kesulitan, aksesibilitas, dan UX

### 8.0 Parameter prototipe terpusat (BASELINE UJI; bukan kanon baru)

Semua angka yang dapat di-*tune* didefinisikan **sekali** pada `Phase3BalanceConfig` (atau asset konfigurasi yang sudah dipakai proyek). Jangan menyalin konstanta berbeda di beberapa skrip. Naskah mengunci tiga bukti, tiga pusaka, tiga generator, tiga siklus, jendela boss 5 detik, tiga tarikan engkol akses, angka penonton dan nominal; **parameter HP bersama mengikuti §0.4**, bukan default tandingan. Config P3 menyimpan angka encounter; nilai HP shared dibaca dari `SmartphoneController`/definisi global.

| Parameter | Baseline | Status |
|---|---:|---|
| `phone.startBatteryPct` | Nilai save; **100% sekali** lewat colokan Kos sebelum `[BERANGKAT]` | BASELINE UJI (P3-D02) |
| `phone.normalDrainPerSecond` | **0,05** | GLOBAL |
| `phone.arOrUvDrainPerSecond` | **0,10 total**, bukan tambahan (UV dan AR bersamaan tetap 0,10) | GLOBAL |
| `phone.evpExtraDrainPctPerSec` | **0** (analyzer dan rekaman mengikuti profil mode aktif) | GLOBAL |
| `phone.photoCostPercent` / `blitzCostPercent` / `blitzCooldownSeconds` | **1% / 15% / 6s** | GLOBAL |
| `phone.largePowerbankRestore` / `smallPowerbankRestore` | **80 / 35 poin** | 80 GLOBAL; 35 existing kecil kos |
| `phone.dynamoFullRestorePct` / `dynamoFullDurationSec` | **+25% / 8s**, sekali per dinamo, persist ID | BASELINE UJI (dipakai ulang dari P1/P2) |
| `phone.dynamoEmergencyCapPct` / `dynamoEmergencyDurationSec` | **<10% untuk mulai**, 45s akumulatif, hasil maks. 10% | BASELINE UJI |
| `phone.dynamoEmergencyCooldownSec` | **120s gameplay** per lokasi; tidak berlaku saat boss aktif | BASELINE UJI |
| `phone.zeroBatteryBehavior` | **Layar mati; tanpa `WATCH_ONLY`** | GLOBAL () |
| `player.effectiveWalkMps` / `sprintMps` / `crouchMps` | **1,7 / 4,2 / 0,8** (controller existing) | EXISTING |
| `uv.floorMarkRangeM` | **8,0 m** (rajah silang terbaca) | BASELINE UJI |
| `uv.photoDwellSec` / `ar.photoDwellSec` | **1,0 s / 1,0 s**, target di frame ≥30% | BASELINE UJI |
| `evp.analyzerRangeM` | **15 m** (sumber suara terdeteksi di gelombang) | BASELINE UJI |
| `evp.recordRangeM` / `evp.recordHoldSec` / `evp.requiredRealSegmentSec` | **≤2,0 m / 6,0 s / ≥2,0 s ASLI**; putus = reset tanpa penalti | BASELINE UJI |
| `evp.intercom.fakeSegmentSec` / `evp.intercom.realSegmentSec` | **8 / 3** (berulang) | BASELINE UJI (E13) |
| `evp.searchDelaySec` | **3,5** (Wewe mulai `SEARCH_KANTOR` setelah rekaman valid) | BASELINE UJI |
| `access.crankCount` / `access.crankSec` | **3 / 1,5** | KANON (3) / BASELINE UJI |
| `fragile.telegraphSec` / `fragile.learnTelegraphSec` | **0,6 / 1,2** | BASELINE UJI |
| `fragile.learnWindowSec` | **30** (atau sampai jebakan #1 dilewati) | BASELINE UJI |
| `fragile.dropNoiseRadiusM` | **10** (hanya `FRAGILE_DROP`) | BASELINE UJI |
| `fragile.safePathMinWidthM` | **1,4 m** (naik dari baseline 1,2 m; perlu verifikasi di collider dan geometri aktual) | BASELINE UJI Argon |
| `fragile.edgeWarningWidthM` / `edgeWarningSec` | **0,8 m / 1,0 s**; hanya *flinch warning* tepi di tutorial, tidak menumpuk ke timer runtuh | BASELINE UJI Argon |
| `fragile.dynamicMinSpawnDistM` | **6,0 m** dan *path-reachability* terjaga; jika tidak ada kandidat, skip | BASELINE UJI Argon |
| `wewe.patrolMps` / `wewe.ceilingMps` / `wewe.chaseMps` | **1,2 / 1,8 / 3,0** | BASELINE UJI |
| `wewe.learnWindowSec` / `wewe.learnSpeedMult` | **30 / ×0,7** | BASELINE UJI |
| `wewe.revealMinDistM` / `wewe.firstLureMinDistM` | **12 / 15** | BASELINE UJI |
| `wewe.screenLightRadiusM` / `wewe.flareLightRadiusM` | **12 / 15** (LOS toleran) | BASELINE UJI |
| `wewe.noiseSprintM` / `noiseWalkM` / `noiseCrouchM` | **8 / 4 / 2** | BASELINE UJI |
| `wewe.chaseDetectM` / `wewe.grabContactM` / `wewe.grabTelegraphSec` | **8 / 1,4 / 0,8** | BASELINE UJI |
| `wewe.loiterSec` | **6,0** (layar mati; loker/lubang) | BASELINE UJI |
| `wewe.lureCooldownSec` | **25** antar umpan | BASELINE UJI |
| `flare.throwRangeM` / `flare.burnSec` | **8 / 20** | BASELINE UJI, kebijakan flare `P3-D13` |
| `flare.lightRadiusM` / `flare.shieldBreakRadiusM` / `flare.lureDwellSec` | **6 / 3,0 / 0,3** | BASELINE UJI |
| `flare.arenaRackPickup` | **maksimum 1 flare bonus per attempt (opsional); bukan gate** | USUL BARU |
| `boss.prepSec` / `boss.interludeMinSec` | **30 / ≥20** | BASELINE UJI |
| `boss.exposedSec` / `boss.cycles` | **5 (KANON) / 3 (KANON)** | KANON |
| `boss.stalkMps` / `boss.stalkDistanceM` | **2,0 / 2,0–3,0** | BASELINE UJI |
| `boss.beamRadiusM` / `boss.leverRadiusM` | **2,5 / 1,6** | BASELINE UJI |
| `boss.lashRangeM` / `boss.lashTelegraphSec` | **2,2 / 0,8** (hanya saat shield aktif) | BASELINE UJI |
| `boss.voiceWaveBlurSec` / `boss.fragilePerCycle` | **3 / 3–5** papan rapuh baru bertanda UV | BASELINE UJI |
| `boss.pullMps` / `boss.pullMaxSec` / `boss.pullFatalDistM` / `boss.pullStartMinDistM` | **1,0 / 12 / 0,8 / ≥10** | BASELINE UJI |
| `boss.anchorHoldSec` / `boss.anchorUses` | **4 / 1 per tiang (2 tiang)** | USUL BARU |
| `boss.flashMaxRangeM` / `boss.headshotAimToleranceDeg` | **8 / 20°** (zona dada + wajah) | BASELINE UJI |
| `boss.reshieldGraceSec` | **0,5** | BASELINE UJI |
| `boss.powerRackRestorePct` / `boss.powerRackDurationSec` | **+25% / 8s** tindakan fisik, satu jatah per fase bersama dua titik; dapat di-reset dengan engkol servis manual saat recovery | BASELINE UJI |
| `boss.recoveryCrankSec` / `boss.recoveryCooldownSec` | **8s / 20s**, hanya di alcove servis saat `RESHIELD/RECOVERY` | BASELINE UJI |
| `boss.pullUvKnotDwellSec` | **1,0 s** saat baterai cukup; alternatif cahaya generator tersedia saat 0% | BASELINE UJI |
| `boss.generatorOrder` | **Urutan bebas**, lampu `used` hanya setelah hit sukses; tidak ada pemadaman sumber yang belum dipakai | KLARIFIKASI |
| `sanity` | Nilai campaign dipertahankan | GLOBAL: Sanity gameplay mulai Babak 4 |
| `sanity.*` (drain/regen/radius) | **Tidak aktif di P3** | GLOBAL |

**Recovery anti-softlock [BASELINE UJI].** Pada baterai nol, layar mati dan AR/UV/EVP/Blitz tidak berfungsi, tetapi dinamo masih dapat dioperasikan **tanpa HP**. Tempatkan **tiga dinamo engkol eksplorasi** (berbeda dari `G-ACC` dan generator arena, E7): (1) **D-P3-1 Pos Timbang** di halaman, sebelum Gilingan; (2) **D-P3-2** di ceruk **Bangsal L1** (zona tenang; progres dipause, bukan di-reset, saat Wewe `INVESTIGATE`/`SEARCH`); (3) **D-P3-3 Kantor Mandor** L2. Masing-masing memberi **satu kali** pengisian normal `+25%` selama `8s` dan menyimpan ID dinamo terpakai. Setelah itu, *emergency crank* di lokasi yang sama tersedia **bila baterai <10%**: **45s akumulatif** (progres hanya di RAM, dipause saat pemain melepas `E` atau dipaksa bersembunyi, tidak di-reset), **cooldown 120s waktu gameplay** antar pengisian sukses, hasil **maksimum 10%**. Bila baterai 3%, pemain boleh langsung memulai. **Dinamo eksplorasi tidak tersedia di arena boss**; **rak daya arena** (§3.6) adalah titik daya darurat global yang terjangkau pada baterai nol, **bukan** dinamo eksplorasi dan bukan bagian jalur dinamo.

**[KLARIFIKASI — baterai 0 dan Wewe].** Baterai 0 membuat layar mati; bagi aturan stimulus cahaya, **layar mati sama dengan `F`**. Karena itu pemain yang baterainya habis saat Wewe mendekat **tidak terjebak**: masuk loker/lubang tungku lalu menunggu `LOITER` 6 s adalah jalur lepas yang sah, dan dinamo dapat dioperasikan setelah itu (QA-61). Pada baterai 0 UV/EVP mati; jalur wajib tetap dapat dilalui lewat telegraph audio/visual, jalur kokoh yang kontras, dan dinamo/rak daya.

### 8.1 Mekanik baterai harus selalu bisa dipulihkan (USUL BARU)

Dokumen sumber tidak menjelaskan kapasitas awal baterai, biaya UV/EVP v2 biasa, kecepatan recharge, atau apa yang terjadi bila baterai nol. Hal ini berisiko besar karena HP dipakai untuk puzzle wajib (foto AR, rekam EVP, foto UV, Blitz). **Kontrak aman:** tidak ada situasi persisten ketika pemain tidak bisa mengaktifkan UV/EVP/Blitz padahal belum bisa mendapat energi; tiga dinamo §8.0 masing-masing punya pengisian penuh **sekali** dan opsi darurat **manual, bersyarat, hingga 10% saja**; arena memberi rak daya sesuai kontrak global. **Tidak ada `WATCH_ONLY`** dan **tidak ada Blitz bertenaga lampu** (D01 yang ditahan). Jumlah/durasi refill diuji agar tiga Blitz 15% tetap dapat dicoba ulang. Biaya mengisi ulang, cooldown, dan resource pressure harus dites, jangan mengandalkan donasi.

**[KANON]** Tiga tarikan engkol akses, 5 detik exposed. Naskah asal tidak merinci biaya boss/standby; default r1 mengikuti parameter global §0.4.

### 8.1A Damage, Sanity, uang dan upgrade (BASELINE UJI)

**Kondisi gagal Raka (hanya tiga):** `FALL` (menginjak `FRAGILE_FATAL`), `CLOTH_GRAB` (kontak kain, pintu loker dirobek saat layar menyala), dan `PULL_IN` (tertarik ke tepi tungku, siklus 3). Serangan langsung Wewe = fatal; **tidak ada sistem health bar kedua**, tidak ada dua-hit (D02 ditahan). **Sanity:** P3 tidak menguras, meregenerasi, atau memakai Sanity; `Kemenyan Bakar` hanya disimpan. **Upgrade:** P3 mewarisi upgrade antarbabak, **tidak ada toko/upgrade wajib di tengah stealth/boss**. `wallet_legitimate` tidak sama dengan `lifetime_donations`; dana terikat `Penonton_Terakhir` tidak masuk saldo.

**Ledger:** lihat tabel §5.3 (Rp134.366.668 belum ditulis sebagai transaksi). Jangan menciptakan saldo/uang palsu; tentukan apakah overlay fixed cinematic atau transaksi dinamis sebelum ekonomi dianggap final (P3-D03).

### 8.1B Bukti solvabilitas baterai dan edge case numerik (BASELINE UJI, BUKAN TES PASS)

Biaya bersama yang ditetapkan: **Blitz 15 poin persen**, foto 1 poin, UV/AR 0,10 poin/s, kamera normal 0,05 poin/s, **tanpa `WATCH_ONLY` pada 0%**. Tiga sukses Blitz sendiri membutuhkan **45 poin baterai** sebelum mempertimbangkan menyalakan kamera, UV dan miss. Itu tidak boleh dicapai dengan hibah tersembunyi dari checkpoint.

**Contoh lintasan tanpa belanja dan tanpa flare, mulai CP5 dengan baterai 0:** dari rute kokoh menuju alcove servis, engkol pengisian manual `+25`, gunakan UV secara hemat selama 30–60 detik (≈3–6 poin), lalu Blitz pertama `−15`. Setelah interlude, ulangi **aksi fisik** untuk `+25`, selesaikan siklus kedua; ulangi sekali lagi pada siklus tiga. Ini hanya model biaya **optimistis**: jika kamera normal menyala lebih lama, terjadi miss, atau pemain terseret, konsumsi menjadi lebih besar. Karena itu panel daya di alcove **dapat di-reset manual saat recovery** dan sumber generator yang belum menghasilkan hit **tidak pernah hilang hanya karena miss**. Acceptance sesungguhnya: pada semua baterai masuk 0/1/14/15/29/44/45/80/100, stok flare 0 dan powerbank 0, serta 0–20 miss, pemain masih bisa memperbaiki situasi **tanpa reload paksa** dengan aksi fisik yang aman dan menyelesaikan boss tiga hit.

**Pagar eksplorasi:** sebelum jembatan gilingan ada dinamo fisik; Bangsal memiliki dinamo yang dapat diakses setelah AI menjauh; pengambilan bukti/rahasia tidak boleh memerlukan UV untuk *menemukan satu-satunya charger*. Baterai habis selama stealth mematikan stimulus cahaya HP; pemain tetap bisa bersembunyi, menunggu Wewe pergi, lalu memompa secara manual. Baterai reload = nilai snapshot **persis**, tanpa top-up. G-ACCESS tidak mengisi HP dan generator arena tidak dihitung sebagai dinamo eksplorasi.

**Uji deterministik wajib:** kerjakan jalur zero-purchase, masuk arena pada semua kondisi tepi tadi, batasi pemain tanpa powerbank, pakai tiap urutan generator serta campuran flare, ulang setelah force quit dan CP5, cek UI foto/Blitz tak mengurangi dua kali pada satu klik. Jika satu *playthrough* buntu, **perbaiki posisi/collider charger, waktu recovery atau data drain**, bukan menaikkan baterai otomatis atau memberi flare wajib gratis.

### 8.2 Kesulitan dan aturan "fair horror" (USUL BARU)

- Setiap aturan fatal diajarkan lebih dulu melalui petunjuk lingkungan, audio, atau peringatan HP, kemudian beri pemain kesempatan mempraktikkan (jendela belajar 30 s, telegraph ganda; hanya tiga fatal).
- Suara tiruan tidak boleh menguras resource saat karakter terkunci di dialogue/cutscene/UI.
- Dalam boss fight, generator/tuas dan target dada-wajah harus mudah ditemukan secara visual namun tetap menuntut timing; *miss* tidak mengonsumsi generator selamanya.
- Jika pemain gagal berkali-kali, jangan otomatis membayar solusinya; tawarkan hint progresif opsional. Jurnal menyimpan clue penting yang terlewat.
- Jangan gunakan ruang puzzle yang memiliki collider/tempat sembunyi hanya pada satu arah yang membuat pemain tak mungkin keluar; Gudang Ampas dan balkon selalu punya jalan keluar kedua.
- **`FRAGILE_FATAL` tidak pernah diletakkan di jalur wajib selain ujian gilingan dan arena, dan selalu ada jalur kokoh yang lebih lebar daripada jalur rapuh.**

### 8.3 Aksesibilitas (USUL BARU)

- *Rebindable controls*; sensitivitas kamera dan inverted Y.
- Subtitle dan *chat history* untuk petunjuk yang disampaikan lewat suara/live chat. Suara tiruan Damar dan suara asli memiliki **label pembicara** pada subtitle.
- **EVP v2 tidak boleh bergantung warna saja:** hijau halus + teks `ASLI` vs merah bergerigi + teks `TIRUAN`; mode kontras tinggi dan opsi pola isian berbeda. **Rajah silang UV** berbentuk silang (bukan hanya ungu), dengan kontras tinggi dan variasi ketebalan.
- Opsi intensitas flashing, *camera shake*, **blur gelombang suara boss**, chromatic aberration, motion blur; generator dan flare, ledakan korslet, dan Blitz punya opsi pengurangan flash. Jangan menaruh informasi wajib hanya pada efek kedipan.
- Pembedaan generator `G-N`, `G-S`, `G-E` memakai **label dan posisi** (huruf besar di rumah lampu), bukan warna saja; reticle "PERISAI HANCUR" jelas pada kontras rendah.
- [PERLU KEPUTUSAN] Apakah mode lebih mudah boleh melonggarkan telegraph papan rapuh, jendela belajar, atau 5 detik boss. Nilai dasar naskah tetap dipertahankan.

### 8.4 Transisi akhir dan item rahasia (KLARIFIKASI)

Urutan akhir **wajib tunggal**: `P3_BOSS_WIN` (Raka tetap di aula) → `P3_EPILOGUE` (chat 666.000, Rp312,5 juta, infiltrasi chat, pesan, tawa Mbah Suro; **tanpa fade**) → pemain bebas eksplorasi → **keluar via gerbang servis aula → lorong lori → halaman** (jalur pendek) atau via gerbang barat (jalur asli) → **motor Raka**. **Cabang opsional:** dari aula kembali ke bangsal L2 → Kantor Mandor → balkon servis untuk Pelat Nomor Buruh, lalu ke motor. Gerbang servis aula tidak dapat dibuka sebelum boss kalah. Replay chat/pesan tidak diperlukan; tawa Mbah Suro sekali.

**[KLARIFIKASI]** Hanya interaksi **`Tinggalkan Pabrik Gula` di motor** yang membuka **panel sementara tanpa event save** dan menampilkan `Arsip Babak 3: 0/1 atau 1/1` dan pilihan `Kembali Menjelajah` atau `Lanjutkan`. Memilih lanjut memicu **satu-satunya event final `P3_COMPLETE`**, commit CP7 akhir, dan baru kemudian `CKREK!` + hitam. Jika gagal menyimpan, jangan fade atau memindahkan pemain; tampilkan opsi coba lagi. Pemain boleh kembali mengambil Pelat sepanjang belum konfirmasi keluar. Jika rahasia tidak diambil, status tetap 0/1. Status kesempatan chapter select setelah keluar mengikuti kebijakan global kampanye [PERLU KEPUTUSAN].

---

## 9. Produksi Unity: pembagian tanggung jawab tanpa overengineering (USUL BARU)

**Tujuan:** menerapkan desain di arsitektur proyek saat ini, **bukan** memerintahkan refactor besar-besaran. Nama komponen di bawah adalah contoh peran, bukan klaim sudah ada.

| Tanggung jawab | Kandidat modul/komponen | Perilaku utama |
|---|---|---|
| Stage, objective dan event | `Phase3FlowController` + data definisi level | Advance stage satu kali, kontrol transisi |
| Interaksi dunia | `IInteractable` / sistem proyek yang sudah ada | Requirement, prompt, pickup, engkol, tuas, katup |
| Bukti | `EvidenceInteractable`, `JournalEvidenceService` | Validasi AR/UV/EVP, pencatatan ID unik |
| UV dan lantai | `UvRajahLayer`, `FragileFloorTile` | Overlay rajah, telegraph, runtuh, `FRAGILE_FATAL/DROP` |
| EVP v2 | `EvpAnalyzer`, `EvpSource` (hijau/merah/segmen interkom) | Gelombang, label, sesi rekam, validasi segmen |
| Live | `LiveStreamTimeline` / sistem chat existing | Trigger chat, history, donasi (state sekali), infiltrasi chat |
| Energi HP | `PhoneBatterySystem` yang sudah ada | Biaya, drain, powerbank, recovery, dinamo |
| Wewe stealth | `WeweGombelBrain` | Patrol, lure, hiding, loiter, grab |
| Boss fight | `WeweBossController` | Perisai, generator, flare, tarikan, window, hit validation |
| Rig arena | `ArenaGeneratorRig`, `ArenaFlareRack`, `ArenaPowerPoint` | Generator, rak flare/daya, korslet |
| Save | Adapter ke `CampaignState` existing | Persist flag idempoten tanpa save kedua |
| QA | EditMode/PlayMode tests dan scene diagnostics | Verifikasi gate, spawn, alur, reload |

### 9.1 Data-driven lebih baik daripada hardcode per adegan

**[USUL BARU]** Tempatkan teks dokumen/chat, item IDs, event IDs, durasi 5 s, biaya 15%, cooldown, pola interkom 8/3, daftar papan rapuh, dan prefab reference dalam konfigurasi/asset definisi yang bisa ditinjau bersama penulis. Teks 1999, donasi, dan cutscene tidak perlu tersalin di banyak skrip. **Tetap gunakan definisi parameter global `Rancangan.MD`** untuk nilai yang shared lintas babak.

### 9.2 Pola implementasi bertahap

1. **Vertical slice Gilingan:** suara tiruan, EVP analyzer, UV, papan rapuh, generator akses, persistensi, retry CP2.
2. **Bangsal:** Wewe patrol, loker/lubang tungku, dinamo, AI, bukti ayunan/interkom/rajah, pusaka, Jurnal confirm, CP3/CP4.
3. **Rahasia dan hall gate:** balkon, EVP ketukan, panah UV, katup 09, gerbang aula.
4. **Boss:** dudukan, karung, CP5 write barrier, tiga generator, flare, tarikan, miss/retry, kemenangan.
5. **Naratif/UI:** chat, VO, dokumen lengkap, Pasar Gaib, pacing 75 menit, epilog, infiltrasi chat.
6. **Golden quality pass:** art, lighting, ambience, animation, optimisasi GPU nyata, automation tests + hands-on QA.

**[KLARIFIKASI]** Urutan di atas adalah prioritas engineering, **bukan mengubah urutan cerita** 1–5 atau meminta pindah ke Babak 4 sebelum Babak 3 playable dan polished.

---

### 9.3 Registri alias istilah Argon — satu ID persist yang berwenang

Argon menulis beberapa nama ID yang **berbeda** dari v2.1. Ini **bukan instruksi menggandakan item/flag**. Sampai audit repository membuktikan ID existing lain, dokumen ini memakai daftar di kiri untuk QA/spec dan memperlakukan nama kanan hanya sebagai *alias dokumentasi*. Jika kode sudah menggunakan nama kanan, siapkan *migration/normalization* sekali dari nilai lama ke nilai kanonik tanpa menghitung dua kali.

| ID kanonik spesifikasi v2.2 | Padanan yang muncul dalam naskah Argon | Aturan |
|---|---|---|
| `P3_GENDONGAN_TUMBAL` | `P3_AR_GENDONGAN` | Satu bukti #1, bukan dua foto |
| `P3_EVP_NINA_BOBO` | `P3_EVP_NINABOBO` | Satu bukti #2 |
| `P3_RAJAH_ANAK` | `P3_UV_RAJAH_ANAK` | Satu bukti #3 |
| `P3_BONEKA_JERAMI` | `P3_BONEKA_JERAMI_TEBU` | Satu pusaka #1 |
| `P3_SABIT_RAJAH` | `P3_SABIT_TEBU_RAJAH` | Satu pusaka #2 |
| `P3_SELENDANG_BATIK` | `P3_SELENDANG_BATIK_TERBAKAR` | Satu pusaka #3 |
| `P3_DOC_MANDOR` | `P3_DOC_MANDOR_1999` | Satu dokumen |
| `P3_ACCESS_GENERATOR` | `P3_GEN_ACCESS_01_ON` | Event G-ACC sama, bukan dua gerbang |
| `P3_DECOY_INTERACT` | `P3_DUMMY_TRIGGER` | **Runtime saja**; CP5 persisten, bukan event boss-start yang disimpan |
| `G-N/G-S/G-E` | `GEN-N/GEN-S/GEN-E` | Tiga objek arena yang sama |

**Penting:** Nama asli yang benar-benar digunakan di `CampaignState` produksi ditentukan **audit kode** sebelum implementasi; prioritas mencegah data ganda, bukan memaksakan pilihan literal penulis dokumen. `SECRET_P3_PELAT_BURUH` dan `SEAL_PABRIK_GULA_SINDANG_LAUT` tetap satu ID unik.

## 10. QA — 146 skenario acceptance Babak 3

**Status semua test: BELUM DIJALANKAN.** Jenis pengujian: `Unit/Automated` = test kodifikasi aturan, `Runtime` = jalankan Unity di mode relevan, `Fault injection` = gagal simpan/restore, `Playtest` = manusia bermain, `Windows visual/QA` = bukti render/performa mesin target. Struktur ini bukan sertifikat lulus; catat commit, timestamp, video/log, mesin, dan hasil per nomor.

### 10.1 A. Gate dan kontinuitas

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-01** | Save P2_COMPLETE plus segel RSUD → masuk P3 | Spawn Kos P3 hanya jika campaign valid; tidak ada grant palsu | Automated |
| **QA-02** | Save tanpa P2_COMPLETE ditolak | Release tidak dapat loncat babak; dev fixture hanya mode pengembangan | Automated |
| **QA-03** | P2_COMPLETE tanpa segel RSUD | Kondisi inkonsisten ditolak atau ditangani melalui recovery eksplisit, tidak membuat segel sendiri | Automated |
| **QA-04** | New Game vs Continue P3 | New Game tidak mewarisi P3 receipt; Continue mengambil transaksi akhir yang valid | Automated |
| **QA-05** | Kamera V pada P3 | V tetap ganti perspektif OTS/FP, tidak menjadi Selfie | Runtime |
| **QA-06** | Selfie X pada P3 | X sesuai binding, tidak menghapus UV R atau recorder C | Runtime |
| **QA-07** | Tahan lalu lepas RMB | HP horizontal dua tangan saat ditahan; pose/perspektif pulih saat dilepas | Windows visual |
| **QA-08** | Modul P3 unlock dua kali | UV dan EVP v2 dibuka sekali tanpa biaya/duplikasi inventory | Automated |
| **QA-09** | Kos Golden Scene bukan versi baru 4×3 | Scene rumah/furnitur/PC existing dipakai, tidak dibuat ulang | Repo review |
| **QA-10** | Rute motor Kos ke pabrik | Waktu 23:45→23:54, CP1 disimpan sesudah kontrol kembali | Runtime |

### 10.2 B. Ekonomi, Shop, dan HP

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-11** | Shop tanpa uang sah | Modul wajib tetap terbuka, paket opsional tidak dibeli dan tidak memblokir keberangkatan | Automated |
| **QA-12** | Paket powerbank besar 3 unit | Pembelian valid menambah jumlah/instance yang tepat sekali dan saldo dikurangi sekali | Automated |
| **QA-13** | Paket flare 4 unit | Empat flare tercatat dari satu transaksi, tidak menggandakan klik ganda | Automated |
| **QA-14** | Paket kemenyan 2 unit | Kemenyan tersimpan tanpa fungsi konsumsi Phase 3 | Automated |
| **QA-15** | Pembelian gagal saat atomic save | Saldo dan inventory sama-sama rollback, tidak terpotong sepihak | Fault injection |
| **QA-16** | 0% baterai di pelataran | Layar normal mati, charger fisik dapat diakses, tidak game over otomatis | Runtime |
| **QA-17** | AR atau UV normal drain | Total profil aktif 0,10 poin/s, bukan kamera 0,05 ditambah 0,10 lagi | Unit |
| **QA-18** | Foto bukti berulang | 1% per aksi foto valid, bukti hanya dihitung sekali | Unit |
| **QA-19** | Blitz 15% dan cooldown 6 detik | Tidak bisa serang sah jika baterai kurang atau masih cooldown | Unit |
| **QA-20** | Milestone donasi Rp312,5 juta | Overlay terpisah wallet; Penonton_Terakhir tidak dikredit sebagai dana belanja | Automated |

### 10.3 C. Tutorial pabrik dan gilingan

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-21** | Forum dilewatkan | EVP/UV tetap terajar lewat lingkungan; tidak ada gate yang membutuhkan baca forum | Playtest |
| **QA-22** | Chat/live tidak dibuka | Petunjuk rute aman dan generator tetap tersedia dari lingkungan | Playtest |
| **QA-23** | Damar palsu dari kiri | EVP merah bergerigi, TIRUAN, sound source konsisten | Runtime |
| **QA-24** | Warna EVP dimatikan/high contrast | Teks TIRUAN/ASLI dan pola gelombang berbeda tetap terbaca | Accessibility |
| **QA-25** | UV rajah X pada kiri | Semua papan rapuh punya mark X yang konsisten saat UV aktif | Visual test |
| **QA-26** | Cabang kanan tanpa lompatan wajib | Player dapat lewat tanpa melompat pada tutorial pertama | Playtest |
| **QA-27** | Spawn 0% sebelum jembatan | Charger manual di landing aman tanpa menyeberang papan rusak | Runtime |
| **QA-28** | Mati jatuh ke gilingan | Retry di CP aman, tidak spawn di lubang/collider mati | Runtime |
| **QA-29** | Panel gilingan masuk dari jalur kanan | Player sampai G-ACCESS tanpa menembus mesh/interactable | Playtest |
| **QA-30** | Jembatan kiri terpicu saat speed berbeda | Telegraph audio/visual berjalan, collider dan kill volume sinkron | Playtest |

### 10.4 D. G-ACCESS dan ayunan

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-31** | Tarikan G-ACCESS 1/3/2 ulang | Hanya tarikan ketiga valid membuka gerbang; progress tidak menghitung klik UI dobel | Unit |
| **QA-32** | G-ACCESS selesai lalu reload | Gerbang tetap aktif, lampu pintu menyala dan tarikan tidak mengulang reward | Runtime |
| **QA-33** | G-ACCESS dibanding G-N | G-ACCESS tidak menghabiskan resource lampu boss Utara | Unit |
| **QA-34** | G-ACCESS bukan pengisi baterai HP | Mengaktifkan pintu tidak menaikkan persentase baterai | Unit |
| **QA-35** | Pintu terbuka saat player melintasi ambang | Tidak menjepit player bila collider/animasi pergantian state | Runtime |
| **QA-36** | Foto ayunan sebelum mengambil boneka | Bukti 1 tercatat sekali dan boneka tetap bisa diambil | Runtime |
| **QA-37** | Ambil boneka sebelum foto ayunan | Ayunan masih dapat difoto; bukti tidak otomatis ada | Runtime |
| **QA-38** | Foto ayunan tidak fokus | Ray/dwell gagal tidak menambah set evidence | Unit |
| **QA-39** | Membaca log Mandor di ayunan | Dokumen 1999 tersimpan dan bisa dibaca kembali | Runtime |
| **QA-40** | Kembali ke gilingan setelah masuk ketel | Gerbang akses dua arah, jalur tidak menutup sendiri | Playtest |

### 10.5 E. Wewe stealth dan jalur tinggi

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-41** | Wewe reveal pertama | Audio/visual telegraph mendahului kendali ancaman fatal | Playtest |
| **QA-42** | Wewe spawn di koridor | Tidak spawn tepat di tubuh Raka atau titik checkpoint | Runtime |
| **QA-43** | Loker besi dengan F | Stimulus cahaya mati; AI boleh mencari last-known dan pergi | Runtime |
| **QA-44** | Loker dengan HP menyala | Cahaya menarik Wewe sesuai LOS; tidak invisibility gratis | Runtime |
| **QA-45** | Berdiri terang di depan Wewe walau F | LOS masih berlaku, layar mati bukan perlindungan mutlak | Runtime |
| **QA-46** | AI kehilangan player | Kembali patroli dalam batas waktu, tidak camp permanen di pintu wajib | Runtime |
| **QA-47** | Catwalk dua celah pada kontrol Raka aktual | Semua rute wajib dapat dilalui; lompat tidak melampaui physics existing | Windows playtest |
| **QA-48** | Wewe melintas 2 tingkat | Navmesh/animation tetap wajar, tidak menembus pagar/lantai | Windows visual |
| **QA-49** | Wewe mengejar saat membaca dokumen | Interaksi punya kondisi adil dan recovery; tidak memberi save fatal | Playtest |
| **QA-50** | Masuk kembali bangsal setelah boss | Wewe tetap RESOLVED, pemain bisa mencapai kantor mandor untuk arsip | Runtime |

### 10.6 F. EVP, rajah dan pusaka

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-51** | Interkom sumber benar EVP v2 | Gelombang hijau halus ASLI, sesi rekam memenuhi bukti 2 | Runtime |
| **QA-52** | Bunyi tiruan Damar di kantor | Warna merah TIRUAN tidak dihitung bukti nina bobo | Unit |
| **QA-53** | EVP terputus di tengah | Tidak menambah bukti; bisa memulai lagi tanpa softlock | Runtime |
| **QA-54** | EVP v2 tanpa mikrofon OS | Berfungsi dari authored source offline tanpa izin perangkat | Offline smoke |
| **QA-55** | Sabit diambil sebelum EVP | Pusaka dan bukti terpisah; interkom tetap bisa dipakai | Runtime |
| **QA-56** | EVP selesai sebelum sabit diambil | Bukti tidak otomatis memberikan sabit, tetap tersedia | Runtime |
| **QA-57** | Rajah cerita UV pada bagasse | Glyph lingkar bersimpul bukan silang X, foto valid mencatat bukti 3 | Runtime |
| **QA-58** | Foto ulang rajah | Counter tetap 3 ID maksimum; tidak menduplikasi dari photo album | Unit |
| **QA-59** | Selendang diambil sebelum foto | Dinding tetap bisa dipindai, bukti tetap tersedia | Runtime |
| **QA-60** | Damar card dan sepatu kiri | Dua clue dapat diakses; bila terlewat, epilog tetap jalan dan Raka tidak memegang sepatu yang belum dimiliki | Runtime |

### 10.7 G. Arsip Saksi 09

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-61** | Tiga ketukan vent tanpa EVP | Katup 09 tidak memberi collectible tanpa validasi sumber yang benar | Runtime |
| **QA-62** | EVP tiga ketukan hijau | Clue rahasia terbuka, berbeda dari interkom normal | Runtime |
| **QA-63** | Suara Damar merah di balkon | TIRUAN tidak membuka loker, tidak memaksa masuk jalur rapuh | Runtime |
| **QA-64** | UV pagar menampilkan tulisan | Nomor sembilan tidak pulang terlihat hanya di zona rahasia | Visual test |
| **QA-65** | Panah UV ke katup 09 | Jalur menuju loker kokoh, tidak memaksa lompatan fatal | Playtest |
| **QA-66** | Katup salah / E spam | Tidak membuka loker 09 atau menggandakan reward | Unit |
| **QA-67** | Pelat diambil satu kali | SECRET_P3_PELAT_BURUH masuk set unik, panel N/5 berdasar aktual | Automated |
| **QA-68** | P1/P2 rahasia dilewatkan | Setelah P3 pickup Arsip bisa 1/5, tidak otomatis 3/5 | Unit |
| **QA-69** | Autosave pelat gagal | Pickup belum sukses dan pelat tetap ada untuk dicoba kembali | Fault injection |
| **QA-70** | Ambil pelat setelah boss | Rute ke balkon tersedia sebelum motor complete dan secret tetap permanen | Runtime |

### 10.8 H. Ritual dan pre-boss checkpoint

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-71** | Pintu aula dengan kurang satu pusaka | Gate menolak dan menunjukkan kekurangan tanpa menutup area investigasi | Runtime |
| **QA-72** | Pintu aula setelah 3 pusaka | Player boleh masuk dan keluar tanpa start otomatis | Runtime |
| **QA-73** | Tiga bukti tanpa konfirmasi Jurnal | Boneka Damar tidak memulai boss; konfirmasi eksplisit diperlukan | Unit |
| **QA-74** | Duduk altar tanpa 3 pusaka | Preview ditolak jelas; tidak ada inventory hilang | Runtime |
| **QA-75** | Pasang preview lalu tutup inspeksi | Pusaka tetap tersedia dan pintu terbuka | Unit |
| **QA-76** | Boneka Damar tanpa E | Boss tetap idle walau dekat/menoleh/pergi | Runtime |
| **QA-77** | CP5 commit gagal | Tidak terjadi scene lock, tidak terjadi transfer pusaka/AI | Fault injection |
| **QA-78** | CP5 sukses lalu die midfight | Pusaka tersedia, 3 lampu utuh dan posisi safe preboss setelah reload | Fault injection |
| **QA-79** | Continue setelah crash di boss | Memuat CP5, tidak state dua lampu hancur + satu pusaka hilang | Fault injection |
| **QA-80** | CP5 save ke slot global existing | Tidak memerlukan Phase3SaveManager kedua, backup lama tetap valid | Repo+unit |

### 10.9 I. Arena tiga sumber dan serangan

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-81** | Tanpa flare sama sekali | Generator N/S/E memberi tiga hit sah dan kemenangan | Playtest |
| **QA-82** | Cahaya aktif saat Wewe di luar cone | Sumber tidak rusak, cooldown lalu bisa dicoba kembali | Unit |
| **QA-83** | Lampu mengenai ilusi suara saja | Boss tidak menjadi exposed tanpa overlap bos nyata | Unit |
| **QA-84** | Wewe masuk cone | SHIELD_BREAK 5s, boss stun dan tidak menyerang | Runtime |
| **QA-85** | Miss Blitz dalam window | Shield pulih, generator tetap tersedia | Unit |
| **QA-86** | Timeout 5 detik | Lampu utuh, player dapat lure ulang setelah cooldown | Unit |
| **QA-87** | Sukses pada lampu N | Hanya N ditandai USED pada attempt, satu seal, S/E tetap bisa | Unit |
| **QA-88** | Satu lampu dicoba untuk hit kedua | Hit sukses tidak dihitung lagi dari sumber USED | Unit |
| **QA-89** | Urutan generator E-N-S | Tiga lampu mana pun urutannya membolehkan finish | Playtest |
| **QA-90** | Blitz kurang 15% saat exposed | Tidak menyegel, charger recover path tetap tersedia | Runtime |

### 10.10 J. Boss resource, jebakan, cycle3

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-91** | Masuk boss baterai 0% | PREP manual charger bisa menjadikan baterai cukup untuk Blitz | Runtime |
| **QA-92** | Masuk boss baterai 1%, 14%, 15% | Perhitungan biaya konsisten dan tidak mengunci pemain | Param sweep |
| **QA-93** | Miss Blitz berulang sampai 0% | Ada escape/recovery path fisik yang tidak mensyaratkan hit sukses | Playtest |
| **QA-94** | Dua charger arena re-arm per interlude | Bukan regen otomatis, tidak farming tiap E saat boss chase | Unit |
| **QA-95** | Kabel kain Wewe tarik cycle3 tanpa flare | UV simpul atau generator ketiga dapat memutus, tetap bisa menang | Playtest |
| **QA-96** | Siklus 3 dengan flare tersisa | Flare boleh dipakai, consumption tepat sekali per deployment | Unit |
| **QA-97** | Flare miss/timeout | Flare habis untuk attempt ini; generator belum sukses tetap tersedia | Runtime |
| **QA-98** | Tiga segel sukses lalu CP6 gagal | Tidak klaim victory tersimpan, CP5 recoverable dan safe retry | Fault injection |
| **QA-99** | Runtuh lantai semasa pause/window | Timer tidak bergerak ketika input/gameplay tak tersedia | Runtime |
| **QA-100** | Siklus boss pada FPS 30 dan 60 | Window 5s dan overlap beam konsisten terhadap frame rate | Windows playtest |

### 10.11 K. Epilog, kualitas, dan regresi

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-101** | Boss menang menyegel sekali | Sealed locations menambah Pabrik hanya sekali setelah CP6 atomik | Unit |
| **QA-102** | Pusaka pindah ke dudukan saat CP6 | Pintu hall tetap terbuka, tidak relock setelah inventory kosong | Runtime |
| **QA-103** | Epilog menampilkan 666000 viewers | Overlay sesuai kanon dan tidak otomatis menambah wallet | Visual/unit |
| **QA-104** | Live chat palsu serempak | 3 pesan berbahaya tampil, bukan objective benar; epilog juga valid tanpa kartu/sepatu Damar | Runtime |
| **QA-105** | Pesan Penonton_Terakhir Rp9999999 | Dana terikat satu event, tidak memotong atau menambah saldo sah | Unit |
| **QA-106** | Epilog tanpa auto fade | Raka dapat keluar dan menelusuri balik pelat buruh 09 | Runtime |
| **QA-107** | Panel motor Kembali Menjelajah | Menutup panel tanpa event P3_COMPLETE/duplikasi | Unit |
| **QA-108** | Panel Lanjutkan + save gagal | Tidak fade/CKREK final sebelum commit ulang sukses | Fault injection |
| **QA-109** | Asset audit pabrik | Aset berlisensi, path/sumber/hash/placement tercatat, tidak ada model baru yang tidak jelas legalitas | Audit |
| **QA-110** | Build Windows Iris Xe + QA regresi P0/P1/P2 | FPS/1% low/memori diukur, kontrol/save lama tetap berfungsi; tes P3 bukan bukti tes lama PASS | Windows QA |


### 10.11A L. QA silang khusus dari review Claude (QA-111–QA-126)

**Tambahan ini berasal dari celah yang ditemukan saat membandingkan spesifikasi**, bukan pengulangan supaya jumlah QA terlihat besar. Semua berstatus `NOT_RUN`.

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-111** | CP3 ketika Buku Mandor/Boneka/Bukti AR ayunan dilewatkan | CP3 tetap tersimpan; bangsal dapat dicapai, lalu pemain kembali ke ayunan dan pickup selesai tanpa softlock | PlayMode |
| **QA-112** | EVP interkom 8 s `TIRUAN` + 3 s `ASLI` secara periodik | Rekaman 6 s hanya valid jika sekurangnya 2 s segmen autentik terekam; red-only ditolak; durasi baseline dapat diubah | Unit + audio |
| **QA-113** | Mengejar sumber gelombang hijau di lantai rapuh | Label `ASLI` bukan jaminan pijakan aman; rajah UV/fisik tetap indikator independen | Playtest |
| **QA-114** | Jatuh dari celah bangsal tingkat 2 / balkon 09 | `FRAGILE_DROP` memindah ke lantai bawah dengan jalur kembali; bukan Game Over tak terduga | PlayMode |
| **QA-115** | Siklus 1–2 menang via flare, 3 generator masih utuh | Siklus 3 tidak mematikan dua generator belum dipakai; pemain bebas memilih satu dari tiga | Unit |
| **QA-116** | Fase 3 dengan 0 flare serta 0 baterai sebelum tarikan | Counter sinar generator dapat memutus kain; akses recharge manual dan final Blitz tetap mungkin | Windows playtest |
| **QA-117** | Tahan UV 1,0 s pada simpul kain tanpa flare | Tarikan kain berhenti, tetapi tidak otomatis memberi poin segel atau membakar shield | Unit |
| **QA-118** | Flare digunakan untuk counter kain lalu Blitz gagal | Flare instance habis, generator belum `used`, pemain tetap dapat ulang lewat charger | PlayMode |
| **QA-119** | Wewe menemukan layar hidup saat pemain bersembunyi | Ancaman mengikuti LOS/stimulus, bukan secara ajaib menganggap bersembunyi aman | Playtest |
| **QA-120** | Save setelah bukti EVP tetapi sebelum AI Search selesai | Restore pada anchor aman; tidak menggandakan bukti atau mengunci Wewe dalam ambush permanen | Fault injection |
| **QA-121** | Baterai boss 0 setelah 20 miss dan seluruh jatah pengisian fase terpakai | Reset manual di alcove memungkinkan +25 lagi; tidak otomatis, tidak mereset generator `used` | Playtest |
| **QA-122** | Interaksi gerbang servis aula selama boss | Gerbang keluar tetap dilas; alcove servis terpisah, tidak membuka jalur skip boss | PlayMode |
| **QA-123** | Sepatu Damar tidak diambil namun boss dan epilog selesai | Cutscene memakai tangan pada pagar; pemain bisa mengambil sepatu/kartu sesudah boss; tidak muncul duplikat | Visual + unit |
| **QA-124** | Setelah boss, pemain mencoba mengikuti instruksi chat palsu loncat ke tungku | Chat tetap narasi; pagar/collider mencegah *death by misleading directive* | Playtest |
| **QA-125** | Backup `.bak` pulih setelah korup `.json` dan upgrade P2/P3 tetap ada | Primary rusak ditolak; API LoadBackupCheckpoint existing memuat .bak secara eksplisit, tidak menggandakan secret/segel dan tetap kompatibel dengan skema repo aktual | Fault injection |
| **QA-126** | Art/audio golden pass per area vs headless-only | Lighting bersumber, telegraph bisa didengar dengan ambience, asset/license/placement lengkap, Windows GPU diuji; tidak PASS hanya karena tes editor | Manual review |

### 10.11B M. QA tambahan audit Argon (QA-127–QA-146)

Semua 20 uji tambahan ini berstatus `NOT_RUN`. Beberapa menguji satu kondisi yang mirip QA sebelumnya, namun dari sisi integrasi berbeda (input timing, metadata sumber, geometri dynamic, *monotonic checkpoint*, dan mutu asset).

| ID | Pengujian / kondisi | Hasil yang diharapkan | Bukti minimal |
|---|---|---|---|
| **QA-127** | CP2 disimpan sebelum umpan Damar, CP3 setelah ayunan; cek urutan dan reload | Tidak ada urutan CP4→CP3; jebakan mati memulihkan CP2, bukan perjalanan CP1 | Automated+PlayMode |
| **QA-128** | Jurnal/chat sedang terbuka, Wewe mendekat; tekan `F` sambil RMB diangkat | Layar padam lewat input prioritas benar; tidak ada klik ganda maupun exploit pause | Runtime input |
| **QA-129** | Ketuk `C` cepat vs tahan `C` pada interkom/ventilasi | Tap toggles analyzer; hold merekam; release yang sama tidak memicu kedua aksi; setelah remap tetap benar | Automated+UX |
| **QA-130** | Audit metadata `EvpSourceBus` seluruh emitter | Source suara Wewe diberi `MIMIC`, arwah/ketukan `AUTHENTIC`, ambience `AMBIENT`; tidak semua `Authentic` | Code+audio |
| **QA-131** | Speaker interkom merah, ketukan hijau, dan ambience aktif bertumpuk | Label bersumber dari cone/priority; 6s interkom harus mengandung segmen hijau untuk valid; tak tersalah-kategori karena uap | Runtime+audio |
| **QA-132** | Spawn jebakan floor dinamis 1–6 m dari pemain dan tepat di bawah kaki | Spawn ditolak dalam radius 6m, on-foot dan anchor CP; tidak terjadi *instant trap* | Unit+scene |
| **QA-133** | Kombinasi tile semua rute ke Mandor/Gudang/Aula/charger | Minimal satu rute kokoh selebar ≥1,4 m dan akses tanpa melompat wajib; bila gagal spawn diskip | Nav test |
| **QA-134** | Tepi papan gilingan 0,8m vs inti jebakan pada FPS30/60 | Tepi memberi peringatan 1,0s; timer runtuh fase belajar 1,2s/normal 0,6s tidak menumpuk menjadi *grace* tak terbatas | Physics+Windows |
| **QA-135** | Rekaman EVP di Kantor dan scan UV Gudang selama Wewe patroli | Ada staging jendela interaksi aman; keluar dari cover tetap berisiko, bukan invulnerable bubble | Playtest |
| **QA-136** | Transisi Wewe `PATROL_CEILING`→`PATROL_FLOOR` saat karakter/prop dekat landing | Drop hanya pada anchor kosong; bunyi `BRUK!` terdengar; AI tak tembus atap/teleport di kepala | PlayMode |
| **QA-137** | Spam `E` + `R` + `C` + `F` sambil memegang RMB dekat tuas/katup | Tepat satu pemilik input per frame, tidak mengambil item sekaligus membidik/Blitz | Input automation |
| **QA-138** | Sembunyi di loker dengan layar mati tetapi pintu terbuka/LOS belum putus | Tidak mendapatkan status aman otomatis; Wewe melihat jika LOS nyata masih ada | Runtime |
| **QA-139** | Muat campaign yang mengandung alias ID Argon dan ID v2.1 sekaligus | Normalisasi menjadi satu evidence/relic/doc/event per objek; tidak 4/3 bukti atau duplikasi pelat | Migration unit |
| **QA-140** | Audit scene art terhadap manifest pabrik | Target ≥30 jenis prop unik berlisensi, non-arsitektur, dicatat hash, skala dan tempat; tidak lulus jika hanya klon atau primitif | Asset audit |
| **QA-141** | Enam nama korban tambahan belum disetujui penulis | Pelat memuat ruang/nama yang disahkan; jangan mengarang daftar nama historis tanpa label editorial | Narrative review |
| **QA-142** | Ambience uap/lori keras saat telegraph Wewe, derit, dan suara ibu palsu | TelegraphBus ducking/prioritas efektif; subtitle bukan satu-satunya petunjuk; bus tidak memaksa AudioMixer paralel | Windows audio |
| **QA-143** | Ambil Bukti #1/Boneka/Dok Mandor di gilingan, mati sebelum masuk Bangsal | Receipt yang sudah committed tidak lenyap dan tidak mengarahkan ke CP4 prematur; spawn aman di gilingan | Fault injection |
| **QA-144** | Bandingkan CP3 yang sudah tercapai tanpa Bukti1/Pusaka1 lalu kembali ke ayunan | CP3 dapat tetap dipakai, data ketika backtrack diperbarui, tidak menutup gerbang dan tak mundur stage | PlayMode |
| **QA-145** | Input UV simpul kain siklus 3 saat 0% atau cukup baterai, dibanding generator ketiga | Pada 0% UV tidak aktif; beam generator tetap dapat memutus tarikan di jalur kokoh, lalu charger manual untuk Blitz | Playtest |
| **QA-146** | Setelah CP6, pemain mengikuti komentar palsu untuk meloncat ke tungku | Tidak ada kill plane/endless fall baru; narasi tidak menjadi petunjuk tugas; panel motor dan sekret tetap bisa diakses | Playtest |

### 10.12 Definition of Done / gate penerimaan P3

- [ ] Seluruh 146 QA memiliki status **PASS/FAIL/BLOCKED/NOT_RUN**, bukti, commit dan alasan. **NOT_RUN bukan PASS**.
- [ ] Main path dapat dimainkan dari Kos ke motor tanpa item yang dibeli, dengan catwalk aman, 3 bukti, Jurnal, 3 pusaka, 3 lampu, 3 Blitz, dan tidak ada softlock.
- [ ] Wewe AI, audio EVP, UV, jebakan, dan arena punya telegraph; menu/pause tidak mematikan ancaman secara diam-diam.
- [ ] CP5 / CP6 / CP7 dan autosave secret diuji dengan kill/reload/crash dan failure injection, bukan sekadar unit mock.
- [ ] Semua prop penting berlisensi dan ditempatkan; frame final menampilkan sumber cahaya, bukan greybox yang hanya lulus collider.
- [ ] Pacing 75 menit **diukur** melalui beberapa pemain, termasuk yang belum tahu teka-teki; tidak dipaksa melalui timer tunggu.
- [ ] Windows build diuji pada Iris Xe dan laporan frame time/memori/visual; tidak hanya Linux headless.
- [ ] Replay P0/P1/P2 tidak merusak save, kontrol, chapter routing, atau upgrade yang sudah ada.

### 10.13 Telemetri dev-only untuk diagnosis

Log per event dengan timestamp gameplay, scene/zone, battery, evidence set, boss state, source light, collision trap, spawn/restore anchor, dan save transaction ID. Matikan logging verbose di release. Simpan metadata QA tanpa merekam audio/mikrofon pengguna, data personal, atau mengirim telemetri jaringan yang tidak dibutuhkan. Tidak menyimpan lokasi presisi user.

---

## 11. Kontinuitas lore, risiko, dan keputusan terbuka

### 11.1 Benang merah cerita dan masalah yang tidak boleh dikarang

**Lima gerbang, empat segel.** Urutan global yang dicatat Phase 1: Rusunawa (1), RSUD (2), Pabrik Sindang Laut (3), Karang Setra (4), Pasar Setan (gerbang kelima, bukan segel lokasi kelima). Rumah Atmaja merupakan lokasi Prolog, bukan gerbang kelima/keenam. Dalam intro Phase 3 naskah menulis empat titik di peta yaitu Rumah Atmaja, Rusunawa, RSUD dan Pabrik; “titik keempat” **bukan** “segel keempat”. Di epilog, kalimat Penonton_Terakhir “Tiga segel lokasi telah terkumpul” benar setelah P3_BOSS_WIN.

**Tiga korban dan sembilan anak.** Buku mandor menyatakan Mbah Suro meminta tiga anak tumbal untuk ritual awal. Arsip loker 09 menyatakan **9 anak hilang total**, yaitu 3 korban awal dan 6 korban berikutnya selama peristiwa tahun 1999. Jangan menyajikan “9 plus 3 = 12 korban”; dan jangan menambah daftar nama sembilan anak dari imajinasi sebagai data historis resmi tanpa label editorial dan persetujuan.

**[KLARIFIKASI anti-softlock Damar]** Kartu pers dan sepatu adalah *story clue* yang sangat dianjurkan ditemukan di bagasse, tetapi **bukan gerbang terselubung** di samping tiga bukti/tiga pusaka. Jika boss dikalahkan sebelum sepatu diambil, `P3_EPILOGUE` tetap berjalan: framing cutscene memakai tangan Raka yang menggenggam pagar (bukan memegang sepatu yang belum dimiliki). Pesan `Penonton_Terakhir` tetap menyebut Karang Setra, dan pemain dapat kembali mengambil sepatu/kartu sebelum keluar. Jika item sudah diambil, gunakan animasi Raka menggenggam sepatu sesuai naskah asli. Tidak membuat sepatu duplikat dekat boss. Verifikasi kedua cabang pada QA-60 dan QA-123.

**Damar: video, kartu, boneka palsu.** Video akhir P2 memperlihatkan Damar diikat di tungku; kartu pers dan sepatu kiri Phase 3 ditemukan di bagasse dan tulisan menyatakan ia dipindahkan ke **Desa Mati Karang Setra**. Boneka goni menyerupai Damar di aula adalah umpan Wewe, tidak membuktikan video P2 palsu atau Damar kembali. Klaim tulisan “video direkam kemarin siang” berasal dari pesan Damar, perlu diselaraskan dengan timeline antar malam pada `Rancangan.MD`; jangan mengubah hari/tanggal rekaman untuk memberi kepastian palsu.

**Sound source `ASLI`.** Ketukan hijau bukan bukti manusia yang sehat, melainkan sumber otentik/informasi benar dalam sistem EVP fiksi. Tidak menyiratkan setiap suara hijau aman diikuti; player masih memeriksa pijakan dengan UV. Hal ini esensial untuk tema penipuan dan ancaman chat di Babak 4.

**Penonton dan uang.** Viewers 185.000 ketika siaran dibuka lebih rendah dari 310.500 akhir P2; dibaca sebagai penonton serentak pada awal siaran baru, bukan penurunan followers permanen. Di akhir mencapai 666.000. Rp312.500.000 adalah angka milestone overlay; dari selisih dengan P2 tidak dapat diturunkan transaksi sah, dan sumber Penonton_Terakhir tetap terikat.

**Larangan membawa Sanity lebih awal.** Phase 3 disebut banyak suara halusinasi, tetapi **Sanity gameplay tetap dimulai Babak 4**; Wewe mengganggu persepsi dan jebakan tanpa mengurangi variabel Sanity. Kemenyan masuk inventori tetapi belum diaktifkan. Jangan menyalin rancangan lama yang membuat mental meter turun di pabrik.

**Aspek lokasi 1912/1999.** Nama pabrik, tahun dibangun dan berhenti serta hilangnya anak adalah kanon dunia game. Jangan mengklaim Pabrik Gula Belanda Sindang Laut sebagai objek sejarah nyata yang telah diverifikasi. Jika memakai aset atau tekstur dokumen berbahasa Belanda/Indonesia, pastikan kesesuaian waktu dan jangan menempel merek nyata tanpa lisensi.

### 11.2 Register keputusan `P3-D01`—`P3-D17` (menunggu audit/ratifikasi)

| ID | Keputusan terbuka | Default draf (bukan kanon baru) | Cara memverifikasi/siapa memutus |
|---|---|---|---|
| `P3-D01` | Posisi/ukuran/scene Kos dan cara travel P2→P3 | Reuse Golden Scene aktual; modul gratis | Codex audit scene/router, desainer ratifikasi |
| `P3-D02` | Kebijakan isi baterai di Kos sebelum keberangkatan | Manual outlet sekali per departure seperti P2 bila disetujui; tidak saat retry | Cocokkan kebijakan aktual P2 dan keputusan desainer |
| `P3-D03` | Definisi ledger total Rp312.500.000 dan dana terikat | Overlay milestone saja, jangan kredit wallet | Desainer + `Rancangan.MD` + state economy |
| `P3-D04` | Harga paket 3 powerbank, 4 flare, 2 kemenyan dan limit beli | Baseline Rp750.000 / Rp1.000.000 / Rp350.000 dari Claude, **bukan kanon**; dapat disetel sesuai shop existing | Desainer/ekonomi di repo |
| `P3-D05` | Mode MVP EVP v2 (jumlah source, kapan classify) | Audio authored source-ID, 6s record, label teks/pola | Audit EVP P1/P2 + playtest UX |
| `P3-D06` | Kontrol UV `R` ketika RMB ditahan dan overlay EVP `C` | Input existing P1/P2; prompt dinamis; jangan bentrok | Audit `GameInputConfig` di repo |
| `P3-D07` | Geometri 8m catwalk dan 2 celah wajib | Blockout menyesuaikan jump controller; rute aman jika mustahil | Unity playtest dan desainer |
| `P3-D08` | AI Wewe patrol/truss dan kecepatan | Parameter §8.0 hanya baseline | Codex navmesh/profile & playtest |
| `P3-D09` | Harga/kapasitas charger pabrik dan arena | Manual charger fisik mengikuti dinamo P1; dua sumber arena per recovery | Verifikasi code + uji baterai 0/1/14/15 |
| `P3-D10` | Skema dan API campaign selepas implementasi P2 | Extend schema **aktual**, bukan menetapkan versi literal | Codex audit migration P0/P1/P2 |
| `P3-D11` | Target kualitas vs budget frame Windows Iris Xe | 60fps global tetap; quality scalability diukur bukan ditebak | Profiling Windows nyata, keputusan desainer |
| `P3-D12` | Revisit arsip & final exit di motor | Epilog terpisah, shortcut servis, CP7 setelah konfirmasi | Desainer ratifikasi endpoint seperti P1/P2 |
| `P3-D13` | Mekanik penarik kain pada boss siklus 3 tanpa flare | Counter dengan UV simpul atau G-E, flare opsional | Gameplay test tanpa shop; desainer |
| `P3-D14` | Rute shortcut pascaboss dari aula ke area timbang | Pintu servis existing secara visual/level; tidak teleport acak | Blockout & desainer |
| `P3-D15` | Isi sembilan nama buruh dan klaim “kemarin siang” | Jumlah korban dan kutipan asli tetap, **nama belum ditentukan** | Penulis menyetujui lore/timeline |
| `P3-D16` | Jumlah/jenis prop final & aturan lisensi | ≥30 jenis pabrik baru sebagai sasaran mutu; sumber/hash wajib | Review aset existing, legal/license, desainer |
| `P3-D17` | Batas interaksi saat Wewe mengejar dan mode Jurnal | UI tidak membekukan AI tanpa alasan, animasi wajib terlindung secara lokal | QA feel, input, softlock |

**Pagar:** tidak semua keputusan harus diajukan ke user satu per satu saat audit; Codex boleh memilih solusi teknis yang konsisten untuk `[BASELINE UJI]` yang tidak mengubah kanon, dan harus **menandai** perubahan substantif sebelum menjadikannya final. Jika spesifikasi belum mungkin karena aset/hardware, laporkan `READY/ADAPT/BUILD/BLOCKED` dengan rujukan file dan fallback, bukan menghapus fitur diam-diam.

### 11.3 Usulan tambahan (boleh ditolak tanpa merusak main path)

- **Jejak sejarah pabrik lewat lingkungan:** bekas label pekerja, sistem shift pesta giling, jejak tebu di lori. Jangan memperbanyak dokumen wajib yang memaksa pemain berlama-lama.
- **[ARGON — kontradiksi "Ibu Asuh Semu"]** Beberapa kepompong tergantung dapat menampilkan **pakaian, mainan, atau boneka jerami** melalui inspeksi aman; ini memberi pertanyaan moral apakah Wewe awalnya menyembunyikan anak dari tumbal atau justru memenjarakan mereka. **Jangan tetapkan jawabannya sebagai kanon baru** tanpa naskah lanjutannya. Tidak perlu visual jenazah eksplisit, dan tetap gunakan aset unduhan/existing berlisensi.
- **[ARGON — petunjuk Damar yang subtle]** Kartu pers boleh memiliki bagian foto memudar/tergores sebagai bayangan tema 'Serahkan matamu' pada chat Pasar Setan di epilog. Ini *foreshadowing*, **bukan** bukti mata Damar benar-benar hilang atau Damar berada di pabrik saat boss.
- **[ARGON — infiltrasi bertahap]** Dalam adegan 2–3, satu-dua akun opsional mengulangi kalimat yang sama dengan jeda aneh. Ini hanya rasa gelisah, tidak pernah menjadi clue wajib atau UI objektif, sedangkan pengungkapan chat terkoordinasi tetap hanya setelah Wewe dikalahkan.
- **Foreshadowing chat:** beberapa akun menuliskan kalimat hampir identik sebelum epilog tetapi **tidak** memberi petunjuk wajib yang salah; reveal utama tetap sesudah boss.
- **Momen sunyi pada ketel:** ketika Wewe naik ke truss, musik mendadak hilang dan terdengar kawat goni menegang; bukan scare spam.
- **Tingkat petunjuk adaptif:** bila 4–6 menit tidak berhasil, Jurnal mengingatkan lokasi sumber audio/rajah tanpa menampilkan langkah akhir, dapat dinonaktifkan.
- **Mode foto bukti visual:** frame AR/UV menunjukkan target dengan feedback jelas tanpa membuat UI terasa arcade neon; indikator cukup halus.

---

## 12. Checklist produksi untuk Codex (spesifikasi → audit → implementasi)

### 12.1 Pra-implementasi dan dependensi

- [ ] Baca tuntas `Phase3.md`, `Phase2.md` aktual, `Phase1.md` audit Codex, `Rancangan.MD` serta commit/branch repository.
- [ ] Petakan `CampaignState`, save schema, chapter routing, `ShoulderCameraRig`, `GameInputConfig`, smartphone, AR/UV, EVP, Jurnal, shop, AI/hiding, charger, asset loader, build pipeline.
- [ ] Nyatakan mana yang benar-benar existing vs dijanjikan dalam dokumen. Catat commit dan file path, jangan klaim P2 selesai hanya karena dokumen merged.
- [ ] Tentukan status P3-D01–D17 berdasarkan bukti kode/scene/hardware. Jangan menimpa P1/P2 tanpa alasan.
- [ ] Tentukan apakah aset Raka/Wewe/Boneka/Goni/factory sudah tersedia; cari sumber legal bila kurang, dokumentasikan license/hash/URL.

### 12.2 Main-path dan fitur

- [ ] Kos: forum 1912/1999, peta bukan segel keempat, shop opsional, upgrade UV/EVP v2 gratis, motor departure.
- [ ] Area timbang: live 185.000, donasi Penonton_Terakhir dana terikat, motor exit endpoint.
- [ ] Gilingan: false Damar EVP merah TIRUAN, lantai X UV, safe right route, fail/fall checkpoint.
- [ ] G-ACCESS tiga tarikan, pintu permanen, ayunan bisa scan AR dan boneka diambil terpisah.
- [ ] Bangsal: Wewe reveal dan stealth, hiding, patrol, 2 tingkat, dua celah yang mampu dilewati karakter existing.
- [ ] Mandor: nina bobo/interkom EVP hijau ASLI + sabit; rekaman tidak minta mikrofon OS.
- [ ] Bagasse: UV glyph **lingkar** bukti ketiga, Selendang, kartu pers dan sepatu kiri Damar.
- [ ] Arsip: EVP tiga ketukan, UV pagar/panah, katup 09, loker, pickup set rahasia N/5 atomik, reentry pascaboss.
- [ ] Jurnal: 3 ID bukti unik, explicit confirm, 3 pusaka, gate pintu, tidak memulai boss dini.
- [ ] Aula: tiga dudukan preview, inspeksi boneka Damar, CP5 praboss disk commit, boss 3 lampu/3 hit/5s window, flare alternatif.
- [ ] Counter cycle3 kain Wewe tanpa flare bisa dilakukan; charger aman saat gagal berkali-kali dan baterai 0.
- [ ] CP6 setelah kemenangan memindah pusaka/segel sekali dan membuka shortcut; CP7 hanya motor Lanjutkan.
- [ ] Epilog: angka 666.000 dan Rp312,5 juta, chat palsu, pesan Karang Setra, kontrol kembali, `CKREK!` saat keluar.

### 12.3 QA, integrasi, dan kualitas

- [ ] Implementasikan/validasi QA-01 sampai QA-146; hasil berlabel PASS/FAIL/BLOCKED/NOT_RUN berbukti.
- [ ] Tes baterai 0/1/14/15/50/100 dengan dan tanpa powerbank/flare, sumber 3 lampu dalam semua urutan.
- [ ] Tes retry/save failure saat CP5/CP6 dan pickup arsip; save P0/P1/P2 tetap valid.
- [ ] Capture walkthrough dengan 3 bukti dan tiga pusaka, dan run kedua tanpa pembelian apa pun.
- [ ] Lakukan Windows build + playtest Intel Iris Xe dengan FPS, frame-time, memori, nilai grafis dan anotasi area terberat.
- [ ] Pastikan audio/grafik bukan placeholder kosong, prop pabrik berbentuk wajar dan ada penerangan diegetik.
- [ ] Dokumentasikan seluruh asset sumber/hash/penempatan dan lisensi; jangan memakai aset tanpa jelas legalitas.
- [ ] Uji 75 menit melalui gameplay nyata; jika 50 atau 95 menit, laporkan dan revisi timing baseline berdasar pengamatan, bukan paksakan timer.
- [ ] Pastikan transisi P3 → Karang Setra hanya menyimpan tujuan, **tidak** membuat Babak 4 atau mengaktifkan Sanity dini.

---


## 13. Review Claude, ChatGPT, dan Argon — audit merger v2.2

### 13.1 Yang diambil dari masing-masing dokumen

| Aspek | Hasil terbaik yang dipertahankan | Asal utama |
|---|---|---|
| Naskah, dialog, lima adegan, lore 1912/1999, 9 anak | Narasi lengkap dan errata ditandai | Claude |
| Denah dan jalur ruang | Gilingan 8m, bangsal dua tingkat, balkon servis, arena 24m, motor pulang | Claude + pemeriksaan ulang ChatGPT |
| Wewe stealth dan EVP interkom | AI, telegraph, pola merah TIRUAN + hijau ASLI, tidak memalsukan audio | Claude; perbaikan semantik ASLI dari ChatGPT |
| Boss 3 siklus | 3 generator yang cukup tanpa flare; kain punya counter flare/UV/sinar; miss tidak menghancurkan lampu | ChatGPT; tempo dan ancaman dari Claude |
| Pengisian baterai | Sistem global P1/P2; dinamo fisik; alcove reset manual untuk miss tak terbatas | ChatGPT + baseline Claude |
| CP5/6/7 dan item rahasia | Satu CampaignState, snapshot tepat, atomik dan rollback; tidak ada schema literal dipaksakan | Keduanya, disatukan |
| QA | 110 kategori teknis awal + 16 uji silang Claude + 20 uji integrasi Argon = **146 kasus unik** | Ketiganya |
| Kualitas produksi | Referensi visual pabrik, manifest lisensi, screenshot per area, Windows Iris Xe | Keduanya |

### 13.2 Konflik yang diselesaikan secara sadar

1. **Flare klimaks:** Claude semula menuliskan flare wajib pada tarikan kain di siklus ketiga; versi ChatGPT menghapus kebutuhan itu. Hasil merger mempertahankan penggunaan flare sebagai cabang sinematik **opsional**, dengan counter UV/sinar bila tidak punya flare. Ini **revisi mekanik usulan** terhadap naskah asli, bukan kanon historis yang disamarkan.
2. **EVP hijau:** Claude menjelaskan segmen interkom ASLI dan TIRUAN dengan lebih baik; frasa "aman diikuti" dinetralkan menjadi **audio otentik, bukan jaminan lantai aman**.
3. **Energi:** jatah charger sekali per fase bisa buntu sesudah miss berulang; sekarang pengisian manual alcove dapat di-reset saat recovery. Tidak ada energi otomatis/Blitz gratis.
4. **Checkpoint CP3:** masuk bangsal tidak mewajibkan ayunan sudah di-scan/diambil. CP5 selalu sebelum boss, CP6 saat menang, CP7 saat keluar; pemulihan bebas state transient.
5. **Schema save:** `schema_version: 4` hanyalah contoh; migration dari versi riil di repository yang harus Codex buktikan.
6. **Kartu pers dan sepatu:** epilog tidak membuat sepatu palsu jika story item terlewat, dan pemain boleh kembali mengambilnya sebelum keluar.
7. **Peta Kos:** Rumah Atmaja adalah titik prolog/asap petunjuk, bukan segel lokasi baru. Pabrik tetap gerbang ketiga.

### 13.3 Status dan batas kepastian

Dokumen ini **hasil audit dua dokumen**, bukan audit repository Unity. `Rancangan.MD` dan branch Unity terbaru belum dibaca langsung di sesi merger ini; status `READY/ADAPT/BUILD/BLOCKED` masih harus ditetapkan Codex dengan bukti path/commit. Balancing, asset availability, keberhasilan build, 75 menit pacing, Windows FPS, dan 146 QA belum diuji. Keputusan `[BASELINE UJI]` tidak otomatis menjadi kanon. **Tidak boleh menganggap penambahan QA sebagai kelulusan QA.**

---

### 13.4 Hasil audit Argon — pilihan terbaik dan saran yang ditolak (v2.2)

**Argon lebih kuat dalam:** spesifikasi *edge warning* 0,8 m/1,0 s, jarak aman spawn jebakan 6 m, akses minimal 1,4 m, staging Wewe vertikal, kontak audio berlabel yang dapat diuji, ketukan balkon, identitas aset industri, dan matriks silang input/remapping. Semuanya diintegrasikan dalam §2.1, §3.2A–B, §4.2C, §4.3, §7.3–7.5, §8.0, §9.3 dan QA-127–146.

**Versi v2.1 lebih kuat dan dipertahankan dalam:** CP2 sebelum tutorial agar fair, pengambilan pusaka di gilingan tidak membuat CP4 sebelum CP3, rekaman interkom yang menuntut segmen ASLI setelah TIRUAN, jalur tiga generator tanpa flare, reset daya manual ketika miss berulang, *rollback* CP5–CP6, dan versioning save sesuai repository aktual. Argon juga menyatakan “UV biaya GPU mendekati nol”: ini **tidak diperlakukan sebagai fakta**, melainkan hipotesis yang diukur pada Iris Xe.

**Konflik sumber yang masih perlu penulis:** durasi/intensitas Wewe bukan angka kanon, angka target 30 prop belum disahkan sebagai gate produksi, dan isi **sembilan nama** pada pelat belum tersedia dalam naskah. Tahan nama placeholder secara jelas, jangan mengarang sejarah korban. Jika diperlukan audit tambahan Codex, klasifikasikan READY/ADAPT/BUILD/BLOCKED berdasarkan kode/scene/aset yang dibaca langsung. Nama kelas/ID Argon di §9.3 bukan kebenaran otomatis bila save produksi memakai nama berbeda.

## 14. Peta integrasi Unity dan gerbang audit Codex

### 14.1 Fondasi bernama dari Phase 1/2 — semua wajib diperiksa ulang

*Penting: path kelas di bawah didokumentasikan pada P1/P2; bukan hasil inspeksi kode pada tanggal pembuatan Phase 3 ini. Cocokkan pada commit terbaru. Path relatif bila masih sama: `Assets/_LayarTerakhir/src/`.*

| Kebutuhan P3 | Nama / jalur yang dicatat pada dokumen P1/P2 | Integrasi yang disyaratkan |
|---|---|---|
| Campaign/save | `Core/CampaignState.cs`, `CampaignData`, API SaveCheckpointAtomic | Tambah receipt/evidence/secret P3 tanpa SaveManager paralel, migrasi idempotent |
| Scene director | Pola `AtmajaDirector.Commit/RestoreWorld`, controller Phase1/Phase2 | Director P3 boleh baru, tetapi bukan event bus global baru yang tidak perlu |
| Movement | `Player/FirstPersonController.cs`, `AnimatedRakaAvatar.cs` | Pastikan jarak jump/speed untuk 8m catwalk; jangan ubah global secara diam-diam |
| Camera rig | `Player/ShoulderCameraRig.cs` | V perspective, X Selfie, RMB HP two-hand; UV via feed existing |
| Interaksi | `Player/IInteractable.cs`, `InteractionInteractor.cs` | Semua E kontekstual; range/collider, jangan hit raycast kamera berbeda-beda |
| Baterai/phone | `Smartphone/SmartphoneController.cs`, `Prolog/AtmajaPhoneView.cs` | Pertahankan battery 0% off dan flash 15%, UV profile, EVP overlay, F screen off |
| Input map | `GameInputConfig` | R UV, C EVP, G flare, J journal; prompt runtime dari binding actual |
| Jurnal/HP UI | `UI/PhoneDesktop.cs`, `HudController.cs`, `HudPresentation.cs` | Reuse evidence, confirm, dokumen, secret N/5, chat danger |
| Camera photo/flash | `AtmajaPhoneView.CaptureFlash` dan kode Blitz existing | Sekali charge per klik, foto vs Blitz oleh kontekstual state, 5s exposed |
| Audio | `Audio/SceneAudioProfile.cs`, `AmbientAudioLoop.cs`, `ThreatAudioOcclusion.cs` | EVP source fiksi authored, TelegraphBus kalau pola equivalent ada; audio 3D source |
| AI/hiding | Pola P0 Atmaja, Phase1 Genderuwo, Phase2 Lastri (status perlu cek) | Brain Wewe sendiri dengan sensor existing, jangan `HorrorFramework` massal |
| Routing/UI | `UI/GameShell.cs`, `Prolog/PrologSceneFlow.cs` | Tambah P3 chapter/scene dan CP1–CP7, transisi P2→P3 sesuai save real |
| Validasi editor | `Editor/FoundationAuditTests.cs`, `FoundationPlayAuditTests.cs`, `Tools/run_golden_checks.py` | Tambah test P3, pertahankan semua existing regression tests |
| Prop/asset manifest | Audit source hashes Phase1, Phase2 registry jika tersedia | Log pabrik model/audio/texture/VRM import dalam `Audit/phase3-asset-sources.json` dan placement yang benar |

### 14.2 Artefak yang harus ada sebelum mengklaim implementasi selesai

1. **Phase3.md** yang sudah dipastikan tidak bertentangan dengan kode/`Rancangan.MD` dan punya daftar perubahan dari hasil audit Codex.
2. Peta `READY/ADAPT/BUILD/BLOCKED` per fitur dengan referensi file nyata (bukan janji umum).
3. Scene set pabrik dengan jalur timbang→gilingan→boiler→kantor/bagasse→aula→motor (atau pembagian scene setara yang dibuktikan bekerja).
4. Event save/CP1–CP7, koleksi 09, dua modul HP, AI Wewe, floor traps, generator G-ACCESS vs arena, tiga bukti+pusaka, boss, epilog.
5. Manifest aset berlisensi dan source/hash/placement yang dapat diperiksa.
6. Laporan QA-01–QA-146 dengan hasil aktual, bukti per kategori dan kekurangan yang masih terbuka.
7. Windows Development build yang benar-benar bisa dimainkan, pengukuran Iris Xe dan signoff kualitas visual/audio/pacing oleh user.

### 14.3 Urutan audit read-only yang disarankan sebelum implementasi

```text
1. Baca dokumen P3 + P2 + P1 + Rancangan.
2. Petakan implementasi Unity nyata dan nama API / scene / shader / prefab.
3. Telusuri jalur input R/EVP/RMB → battery → save → evidence.
4. Audit AI/hiding, navmesh dua lantai, UE/URP lighting/UV, asset license.
5. Buktikan baterai 0% dan flare 0 tetap bisa menyelesaikan boss, CP5/6 dan secret persist.
6. Klasifikasikan READY / ADAPT / BUILD / BLOCKED dan revisi langsung P3.
7. Setelah izin implementasi: eksekusi scene/mekanik, test otomatis, visual Windows.
```

**Instruksi kepada Codex:** jangan menjadikan spek ini alasan membuat refactor besar lintas Phase 1/2. Bila ada mekanik tak kompatibel, **ubah cara implementasi tanpa menurunkan standar desain**, dokumentasikan alasan serta bukti kode, tandai keputusan yang memerlukan persetujuan. Jangan mengklaim 146 QA PASS atau FPS 60 tanpa menjalankan/merekam hasilnya.

---

## Lampiran A — checklist asli Babak 3 (dipertahankan)

- [ ] Semua jalur wajib dapat dilewati dengan perlengkapan tetap, tanpa membeli item opsional.
- [ ] Tiga bukti unik tersimpan dan dapat dibaca setelah reload; foto duplikat tidak menaikkan hitungan.
- [ ] Arena terkunci hanya setelah Jurnal dan pusaka babak lengkap; keluar dari inspeksi tidak memulai boss.
- [ ] Gagal Blitz saat perisai terbuka mengulang kesempatan cahaya; tidak menghabiskan seluruh arena.
- [ ] `SECRET_P3_PELAT_BURUH` dapat diperoleh, disimpan sekali, dan tetap ada setelah mati/reload.
- [ ] Sebelum transisi, status collectible babak terlihat; petunjuk tetap tersedia jika chat terlewat.
- [ ] Save akhir babak memuat segel lokasi, upgrade, inventori, uang tersedia, Sanity, dan Arsip Saksi secara konsisten.


---

## Lampiran B — kriteria selesai naskah Babak 3 (dipertahankan)


- [ ] Semua jalur wajib dapat dilewati dengan perlengkapan tetap, tanpa membeli item opsional.
- [ ] Tiga bukti unik tersimpan dan dapat dibaca setelah reload; foto duplikat tidak menaikkan hitungan.
- [ ] Arena terkunci hanya setelah Jurnal dan pusaka babak lengkap; keluar dari inspeksi tidak memulai boss.
- [ ] Gagal Blitz saat perisai terbuka mengulang kesempatan cahaya; tidak menghabiskan seluruh arena.
- [ ] Item `SECRET_P3_PELAT_BURUH` dapat diperoleh, disimpan sekali, dan tetap ada setelah mati/reload.
- [ ] Sebelum transisi, status collectible babak terlihat; petunjuk tetap tersedia jika chat terlewat.
- [ ] Save akhir babak memuat segel lokasi, upgrade, inventori, uang tersedia, Sanity, dan Arsip Saksi secara konsisten.
