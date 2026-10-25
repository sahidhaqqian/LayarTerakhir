# PHASE 5 — LAYAR TERAKHIR (PASAR SETAN)

> **Project:** Layar Terakhir · **Babak:** 5 (final) · **Lokasi:** Pasar Setan · **Versi:** 1.3-REPO-IMPLEMENTATION (25 Oktober 2026)
> **Basis naskah:** `Phase5.MD` (revisi 15 Oktober 2026): Acuan Babak 5, Item Rahasia, Naskah Penuh lima adegan, dan Trigger/kriteria selesai. **Acuan struktur dan kontrak lintas babak:** `Phase4.MD` v2.1-REPO-IMPLEMENTATION (25 Oktober 2026); P1–P3 hanya diwarisi melalui apa yang Phase4 nyatakan.
> **Status:** **Implementasi playable dan ZIP Windows tersedia; CRC serta baca-ulang SHA-256 R2 lulus. Penerimaan artistik dicatat terpisah.** Lihat `Audit/phase5-feasibility.md` dan `Audit/phase5-implementation.md`. Build Windows x86_64 dan Linux development berhasil; 232 tes campaign/scene dan empat probe crash nyata lulus. Route native baru dari PHASE4_CP8/0 baterai/0 Sanity/0 uang/tanpa consumable sampai CP9 lulus151 pemeriksaan; empat ending dari fixture FINAL_CHOICE juga lulus. Penerimaan manusia untuk180 skenario §10 belum diklaim lulus. Dokumen ini disusun **setara dengan Phase4 dalam cakupan dan kedalaman spesifikasi** (hierarki aturan, registry event, denah, AI, save/checkpoint, anti-softlock, QA, audit). **Kesetaraan dokumen bukan klaim kesetaraan kualitas build.**
> **Tujuan:** Spesifikasi tunggal untuk narasi, urutan adegan, denah Pasar Setan, tiga sektor distorsi, tiga bukti, boss `Penonton_Terakhir` tiga fase, state pilihan akhir, empat ending, Arsip Saksi, save/load, aset, dan kontrak penerimaan. **Kebijakan penyusunan:** kanon naskah dipertahankan; aturan yang saling bertentangan di dalam naskah ditutup eksplisit di §0.7; pola solvability, progres persisten, dan fair-horror dari Phase4 diwarisi, bukan disalin mentah.

---

## 0. Cara membaca, hierarki aturan dan audit sumber

### 0.1 Legenda keputusan

- **[KANON]**: fakta, angka, dialog, dan tujuan yang tertulis pada `Phase5.MD`: lima adegan, 75 menit, Pasar Setan, `Penonton_Terakhir`, tiga bukti, tiga inti, tiga sektor distorsi, empat ending, lima Arsip Saksi.
- **[DIWARISI P1–P4]**: kontrak campaign, HP, input, Jurnal, ekonomi, Sanity, Arsip Saksi, checkpoint rolling, aset, dan boundary Pasar Setan dari fase sebelumnya. **Terapkan kondisi repository aktual**, bukan nama class hipotetis.
- **[KLARIFIKASI]**: interpretasi yang menyambungkan konflik dan menutup softlock tanpa mengubah tujuan cerita.
- **[REVISI EDITORIAL]**: bagian naskah yang secara eksplisit dibetulkan; dicatat di §0.7.
- **[USUL BARU]**: fitur/tambahan yang boleh dihapus atau diubah bila bertentangan dengan arahan desainer.
- **[BASELINE UJI]**: angka tuning awal; wajib dapat disetel terpusat (`Phase5Balance`) dan dievaluasi dalam playtest.
- **[PERLU AUDIT]**: klaim tentang repository/engine yang harus diverifikasi sebelum coding (`P5-Rxx`, §0.9).
- **[PERLU KEPUTUSAN]**: pilihan desainer; lihat §11.2 (`P5-Dxx`).

**Prioritas:** putusan terbaru desainer + `Rancangan.MD` resmi > implementasi/kontrak global aktual P1–P4 > kanon Phase 5 > baseline/usulan di dokumen ini. Repository tidak disertakan saat dokumen ini disusun, sehingga setiap pernyataan tentang kode aktual ditandai [PERLU AUDIT]. Jangan menafsirkan QA dokumen sebagai tes yang sudah PASS.

### 0.2 Ringkasan babak dan identitas

| Elemen | Ketentuan |
|---|---|
| Judul | **Babak 5 — Layar Terakhir** |
| Sasaran waktu | **75 menit** jalur utama, di luar rahasia, pengulangan dan eksplorasi opsional; bukan timer paksa |
| Lima adegan | (1) Gerbang Pasar Malam + Kios; (2) Labirin Distorsi tiga sektor; (3) Panggung Lelang + tiga bukti; (4) Boss tiga fase; (5) Pilihan akhir dan empat ending |
| Lokasi | Boundary P4 → Gerbang Pasar Setan (Kios Tumbal, Kios Arsip Bisu) → Koridor Beku → Jembatan Gilingan Bambu → Ruang Jahit Cermin (replika ruang bawah tanah Atmaja) → Panggung Lelang Utama → Ruang Cermin Saksi → arena boss → portal cermin |
| Musuh | **`Penonton_Terakhir` (“Sang Dalang Layar”)** sebagai boss tiga inti; **Replika Memori** (Kuntilanak Merah, suara Wewe Gombel, Nyonya Sri Atmaja + Genderuwo, gelombang api Banaspati) sebagai konstruksi sektor/fase, bukan boss yang hidup kembali |
| Identitas horor | **Tontonan dan kesaksian**: satu juta penonton adalah korban sekaligus bahan bakar; kamera bisa menjadi alat saksi atau alat komodifikasi. Berbeda dari P4 (kepercayaan pada UI) |
| Modul | **Seluruh modul HP**: AR (`Q`), Selfie (`X`), Thermo (`T`), EVP (`C`), UV Rajah v2 (`R`), Flare (`G`), Kemenyan (`H`), Stealth (`F`), Blitz, Jurnal, Sanity, infiltrasi chat |
| Bukti wajib | Server batu + kabel roh (AR); Kontrak Darah bersuhu di luar rentang (`-- °C`) + rajah Mata Terbuka (Thermo + UV); EVP gabungan suara penonton |
| Prasyarat | **Empat segel** P1–P4 (syarat cerita pembuka arena); **lima ID unik Arsip Saksi** (syarat terpisah khusus Secret Ending) |
| Boss | **3 inti**: Inti 1 (UV + Generator Lampu Sorot + Blitz), Inti 2 (Selfie + Thermo + Flare/Lentera + Blitz), Inti 3 (AR putus 3 kabel + berlindung dari api + Blitz). Total **3 Blitz inti** |
| Ending | **Bad** “Tuan Rumah Baru”, **Normal** “Siaran Terputus”, **True** “Jepretan Terakhir”, **Secret** “Saksi Tanpa Layar” (lima Arsip Saksi) |
| Ekonomi | Kios Tumbal opsional; semua jalur wajib selesai pada `wallet_legitimate=0`, tanpa pembelian dan tanpa donasi |
| Rahasia | **Pita Rekaman Pertama** `SECRET_P5_PITA_PERTAMA` di Kios Arsip Bisu; satu dari lima Arsip Saksi |
| Transisi | Masuk dari P4 CP8 **tanpa Kos**; keluar ke credits setelah `P5_COMPLETE` |

### 0.3 Alokasi waktu dan pengajaran mekanik

| Adegan | Menit sasaran | Alur / fungsi belajar | Gate akhir |
|---|---:|---|---|
| 1 — Gerbang + Kios | 00–15 | Satu juta penonton, cermin wajah, chat setengah-sadar; pemulihan gratis; Kios opsional; puzzle Pita opsional | Pemain masuk koridor distorsi |
| 2 — Tiga sektor distorsi | 15–40 | Ujian ulang modul dengan pembalikan: Selfie+Thermo, EVP+UV+Kendi, Stealth+AR | Tiga sektor selesai; portal cermin |
| 3 — Panggung Lelang | 40–50 | Damar, tiga bukti bebas urutan, Jurnal, Ruang Cermin Saksi | Jurnal terkonfirmasi, `PHASE5_CP6`, arena terkunci |
| 4 — Boss tiga fase | 50–68 | Tiga inti, tiap fase menuntut kombinasi modul berbeda; progres inti persisten | Tiga inti pecah, Damar bebas, `FINAL_CHOICE` |
| 5 — Pilihan akhir | 68–75 | Tiga aksi dasar + satu aksi bersyarat; cutscene ending; credits | `P5_COMPLETE` (CP9) |

### 0.4 Diferensiasi sengaja dari Phase 1–4

**Jangan menaikkan kesulitan melalui hitpoint boss, instakill tanpa telegraph, atau menambah pertarungan yang tidak ada di naskah.** P1–P3 menguji satu modul per boss; P4 menguji kepercayaan pada UI; **P5 menguji apakah pemain benar-benar menguasai seluruh alat** — setiap modul muncul kembali dalam konteks terbalik (Thermo yang dulu mencari dingin kini membedakan `+50°C` dari `-22°C`; Kendi yang dulu menyiram obor desa kini membuka jembatan; Selfie yang dulu menahan hantu kini membuka punggung boss). Tidak ada mekanik baru yang wajib dipelajari selain **state pilihan akhir** (sinkronisasi ritual, ritual capacitor, tahan input) yang diperkenalkan lewat Jurnal pada `FINAL_CHOICE`.

**Prinsip dua lapis kebenaran (diwarisi P4):** chat, badge, dan notifikasi dapat dipalsukan; **properti dunia authored** (rajah UV, benang merah, grafik EVP authored, angka Thermo target, aura AR) tidak. Di P5 pemalsuan lebih agresif (satu juta akun, `Penonton_Terakhir` memalsukan hampir seluruh antarmuka non-input) tetapi **tidak pernah menyentuh action dispatcher, binding, atau bentuk simbol dunia**.

**Prinsip etis ending:** keputusan akhir ditentukan oleh **apa yang pemain lakukan dengan kamera** (menjual Damar, memutus siaran, menghancurkan jaringan, atau merekam kesaksian), bukan oleh angka tersembunyi atau menu dialog. Tidak ada ending yang dipicu secara tidak sengaja (§4.8).

### 0.5 Kontrak global yang tidak boleh digandakan

| Sistem | Prinsip |
|---|---|
| Mulai babak | Save `P4_COMPLETE`/`PHASE4_CP8` dan **empat segel valid** (`SEAL_RUSUNAWA_GRIYA_ASRI`, `SEAL_RSUD_LAMA_SIDO_WARAS`, `SEAL_PABRIK_GULA_SINDANG_LAUT`, `SEAL_DESA_MATI_KARANG_SETRA`). Mode skip P4 hanya `dev-only` dan tidak menulis segel palsu ke produksi. |
| Boundary | **Pakai boundary fisik P4** (tangga batu di bawah Pendopo) sebagai titik handoff; tidak membuat vestibule duplikat. Kos tidak muncul. |
| HP/input | Kamera OTS/FP, tahan RMB dua tangan, `V` (perspektif), `X` (Selfie), `Q`, `R`, `T`, `C`, `F`, `G`, `H`, `J`, `E`, `GameInputConfig` existing. **Flare (`G`) aktif kembali di P5** (nonaktif hanya di Karang Setra). |
| Energi | Profil P3/P4: normal 0,05 poin/s; AR/UV 0,10 total/s (tidak ditumpuk), foto 1 poin, Blitz 15 poin/cooldown 6 s, powerbank besar +80. UV v2 tetap filter yang sama. |
| Uang | `wallet_legitimate` belanja sah; `wallet_bound` donasi `Penonton_Terakhir` Rp66.666.666; overlay/epilog uang dipisah. Tidak ada top-up dari nominal overlay. |
| Evidence & item | `evidence_ids`, `relic_ids`, `inventory`, `secret_items`, `location_seals`, receipt events bersama. Pisahkan item ditemukan, dibawa, dan dipasang. |
| Save | Lanjutkan `CampaignState` / `TryCommit` / atomic save `.tmp/.bak`. Jangan paksakan nomor schema; rollback penuh pada gagal tulis; **tidak ada write barrier global sepanjang boss** (pola P4). |
| Sanity | Sistem P4 dilanjutkan. Nilai dari save dipertahankan, termasuk 0. Sumber drain/pemulihan P5 didefinisikan §4.1; tidak ada reset diam-diam. |
| Performa | Unity URP Windows; FPS/visual sah hanya dari build Windows laptop kelas Iris Xe, bukan headless. |
| Ending | **Satu enum** `EndingId` + kunci transisi atomik; dua cutscene tidak boleh terpicu bersamaan. |

### 0.6 Invarian yang tidak boleh dinegosiasi

1. **Tiga inti, tiga Blitz inti**, berurutan 1→2→3. Tidak ada Blitz keempat di boss; Blitz True Ending adalah aksi `FINAL_CHOICE`, bukan inti.
2. **Tiga aksi ending dasar selalu tersedia** pada `FINAL_CHOICE`; Secret hanya bila lima ID unik valid. Pemain dapat menamatkan tiga ending dasar tanpa satu pun item rahasia.
3. **Replika Memori bukan boss yang hidup kembali**: tidak mengubah `location_seals`, tidak dapat disegel, tidak dihitung bukti/pusaka, tidak punya HP bar.
4. `Sanity=0` **bukan kematian otomatis**; interaksi, refill, dinamo, altar doa, dan save tetap tersedia.
5. Kios Tumbal, Kemenyan, Flare, dan Powerbank **opsional**; pengganti gratis wajib ada: dinamo, Altar Doa Pasar, Lentera Panggung.
6. Status Arsip Saksi diturunkan dari **jumlah ID unik aktual** (`N/5`), bukan nomor babak. Item rahasia committed **sebelum** popup sukses.
7. Input ending (`hold F 5 s`, dwell TUMBAL, hold `E` 3 s pada cermin, sinkronisasi `X→Q→R`) **hanya diterima pada state `FINAL_CHOICE`**.
8. “Baterai 5%” pada pilihan akhir adalah **Kapasitor Ritual** state-lokal; baterai persisten HP tidak diubah diam-diam.
9. Tidak ada save yang menulis status sementara boss (transform, timer paparan, index chat) sebagai kebenaran persisten.
10. Tidak ada nama korban baru yang dikarang dokumen ini; kesaksian Arsip Saksi memakai data item arsip yang sudah kanon.

### 0.7 Revisi editorial dan konflik sumber yang ditutup

| ID | Ketegangan dalam `Phase5.MD` | Keputusan kerja dan status |
|---|---|---|
| E01 | Naskah memakai `[V]` untuk Selfie; P1–P4 mengikat `V`=OTS/FP dan `X`=Selfie | Selfie = **`X`** (binding aktual); label naskah diseragamkan; hint HUD dari `GameInputConfig` [REVISI EDITORIAL] |
| E02 | Kios menjual “Full Charge + 3 Powerbank + 4 Flare + 4 Kemenyan” dengan “uang saweran”, sedangkan seluruh jalur wajib harus mungkin tanpa belanja | Kios opsional, memakai `wallet_legitimate` saja; pengganti gratis wajib (dinamo, altar doa, lentera) [KLARIFIKASI] |
| E03 | “4 Pilihan Eksekusi” vs “salah satu dari 3 aksi” | **3 aksi dasar + 1 bersyarat (Secret)**; teks “3 aksi” dibaca sebagai aksi dasar [KLARIFIKASI] |
| E04 | Baterai “5%” hanya cukup 1 tindakan, tetapi Blitz memakan 15 poin; baterai persisten tidak boleh diubah diam-diam | **Kapasitor Ritual 5%**: sumber daya sekali pakai khusus `FINAL_CHOICE`, ditampilkan sebagai 5% di overlay HP; baterai persisten tidak berubah [KLARIFIKASI] |
| E05 | Bukti #2 butuh Thermo dan UV, sementara AR/UV saling menggantikan | Satu ID evidence, **dua sub-pengamatan** (Thermo stabil 2 s + UV Rajah Mata Terbuka) bebas urutan [KLARIFIKASI] |
| E06 | Fase 2 “wajib menekan `H`” | Kemenyan **opsional**; Altar Doa Pasar dan lantai Sanity menjaga solvability [REVISI EDITORIAL] |
| E07 | Fase 2 “wajib `G` Flare”, tetapi Flare dibeli opsional dan P4 menonaktifkan `G` | `G` aktif di P5; bila Flare = 0, `G` menurunkan **Lentera Panggung** (objek dunia reusable) dengan efek identik [KLARIFIKASI] |
| E08 | Hantu lama muncul lagi (Kuntilanak Merah, Wewe, Sri Atmaja, Genderuwo, Banaspati) padahal sudah tersegel (P4 §11.1: tidak muncul sebagai encounter aktif tanpa keputusan desain) | Keputusan desain di sini: **Replika Memori** dengan ID terpisah, tanpa segel/HP [KLARIFIKASI] |
| E09 | Sektor 2 memakai Kendi “dari Babak 4”, padahal item bisa hilang dari save debug | Kendi carry-over (instance tunggal P4); fallback **Cawan Air Pasar** sama fungsi bila tidak ada [KLARIFIKASI] |
| E10 | “Pengganti Safe Room: Kios” vs “checkpoint tanpa menjadi Safe Room” | Kios **bukan** Safe Room: tanpa istirahat/skip waktu, stok terbatas, gangguan chat [KLARIFIKASI] |
| E11 | Kios Arsip Bisu di gerbang, tetapi cermin arsip “sebelum arena final” memberi kesempatan kembali | **Cermin Jalan Pulang** di Ruang Cermin Saksi: jalur fisik/loading boundary ke gerbang tanpa mengubah state [KLARIFIKASI] |
| E12 | “Pusaka dan prasyarat” P5 = empat segel, tetapi trigger menulis “pusaka babak lengkap” | Pembuka arena diturunkan dari **empat segel + 3 bukti + Jurnal**; tidak ada pusaka baru [KLARIFIKASI] |
| E13 | 999.999 (akhir P4) vs 1.000.000 (P5); donasi Rp66.666.666 vs ekonomi | Viewer = overlay concurrent; donasi masuk `wallet_bound` sekali [KLARIFIKASI] |
| E14 | Epilog Bad (saldo Rp500.000.000) dan True/Secret (Rp125.000.000) vs ledger | Angka epilog adalah state sinematik; **tidak menulis wallet** [KLARIFIKASI] |
| E15 | Secret menampilkan arwah Tari, Lastri dan anaknya, sembilan anak buruh, warga Karang Setra; nama korban | Nama berasal dari data arsip yang sudah kanon; **tidak ada nama baru dikarang**; ID P1/P2 mengikuti repo [PERLU AUDIT] |
| E16 | Normal: “hancurkan Smartphone” | Efek ending-only pada cutscene; save CP8/CP9 tetap konsisten; tidak menghapus HP dari save sebelum commit |
| E17 | `F` biasa = layar mati; Normal = tahan `F` 5 s | Hold-5s hanya valid pada zona tepi panggung pada `FINAL_CHOICE`; di luar itu `F` tetap toggle layar |
| E18 | True: `X`, `Q`, `R` berurutan padahal AR/UV saling menggantikan | State **SYNC_CALIBRATION** khusus ending: urutan wajib `X→Q→R`, pengecualian lokal, tidak berlaku di gameplay biasa |
| E19 | Fase 2: Blitz via mode Selfie | Blitz via Selfie valid hanya pada jendela inti 2 dengan LOS ke target |
| E20 | Checklist menyebut Sanity di save akhir tetapi naskah tidak mendefinisikan mekaniknya di P5 | Sanity dilanjutkan dengan sumber drain “tatapan penonton” (§4.1) [USUL BARU] |
| E21 | Fase 1 “benang merah” hanya warna | Benang asli = **garis kontinu berdenyut sesuai ritme kurungan**, palsu = putus-putus; bukan warna saja [KLARIFIKASI] |

### 0.8 Register angka cerita yang tetap

**1.000.000** = penonton P5 (overlay; akhir P4 = 999.999); **Rp66.666.666** = donasi `Penonton_Terakhir` pembuka (`wallet_bound`); **Rp500.000.000** = nilai kontrak naratif (bukan uang yang otomatis diterima); **Rp125.000.000** = donasi tulus pada epilog True/Secret (narasi); **`-22°C`** = Pintu Tengah (asli), **`+50°C`** = pintu jebakan; **`-- °C`** = suhu di luar rentang pada Kontrak Darah (anomali gaib, bukan 0 K); **`-30°C`** = suhu saat `Penonton_Terakhir` di belakang leher (Fase 2); **4 sosok kembar**, **3 kabel roh**, **3 inti**; **4 m** tinggi boss; **5%** = Kapasitor Ritual; **3 detik** layar mati (Pita), **3 detik** tahan `E` (cermin), **2 detik** dwell TUMBAL, **5 detik** tahan `F` (Normal); **05:30 WIB** epilog True; **5 Arsip Saksi**, **4 segel**, **3 bukti** adalah hitungan berbeda dan tidak boleh tercampur.

### 0.9 Handoff dari Phase 4 dan kontrak repository yang harus diaudit

Butir berikut berasal dari klaim Phase4 §0.11 dan **belum diverifikasi ulang untuk P5**. Semuanya [PERLU AUDIT] sebelum coding.

- **P5-R01 / data:** P4 memigrasikan schema 4→5 dan memakai `Phase4Data` dalam `CampaignData`. P5 menambah `Phase5Data` melalui jalur migrasi normal repo; jangan menebak nomor schema.
- **P5-R02 / input:** `V`=OTS/FP, `X`=Selfie, `Q/R/T/C/F/G/H/E` melalui `GameInputConfig`; Selfie, Thermo, EVP memakai sumber authored existing (`Phase3EvpSource` sebagai metadata).
- **P5-R03 / handoff:** P4 REPO-06 menyatakan boundary fisik di bawah Pendopo sudah ada pada level P4 dan `chapter_id` belum menunjuk scene 5. P5 harus **menghubungkan router** (Continue dari `PHASE4_CP8` → `PHASE5_CP1`) tanpa merusak save P4 yang berhenti di boundary.
- **P5-R04 / layer:** Layer UV rajah dan layer dinding ilusi AR dari P4 REPO-10 dipakai ulang; jangan menambah camera kedua.
- **P5-R05 / replika:** kit visual baru diunduh khusus P5; tidak memakai kit art P1–P4. ID replika dan empat segel tetap terpisah. Lihat §0.11 REPO-06.
- **P5-R06 / aset:** lingkungan Pasar Setan baru dari sumber komersial berlisensi, ≥100 identitas model baru; manifest dan kredit (§7.2).
- **P5-R07 / mutu:** target mutu adalah evaluasi lingkungan lokal, readability, kamera, ancaman, audio dan pacing; jumlah aset atau tes headless bukan bukti kesetaraan.
- **P5-R08 / VO:** dialog manusia Indonesia belum final hanya karena subtitle/efek tersedia; daftar rekaman terpisah. Tidak ada TTS sebagai final.
- **P5-R09 / ending pipeline:** cutscene empat ending dan credits memakai pipeline cutscene existing atau `Timeline` yang diaudit; satu enum + lock atomik.
- **P5-R10 / kelas kandidat:** `Phase5Director`, `Phase5Campaign`, `Phase5Hub`, `Phase5Sectors`, `Phase5Boss`, `Phase5Ending`, `Phase5Balance`, `Phase5FearPresentation` — **nama kandidat, bukan fakta repo**.

### 0.10 Perubahan yang tidak diadopsi mentah-mentah dari naskah

- **Menjadikan Kios sebagai satu-satunya sumber Flare/Kemenyan/Powerbank:** ditolak; melanggar solvability tanpa uang.
- **Reset seluruh boss setelah kematian:** ditolak; tiga inti dikomit satu per satu (pola P4 CP6 rolling).
- **Baterai persisten dipaksa 5% sebelum ending:** ditolak; memakai Kapasitor Ritual (E04).
- **Ending dipicu oleh dialog menu atau keberhasilan statistik:** ditolak; ending dipicu aksi fisik pada `FINAL_CHOICE`.
- **Menghidupkan Banaspati/Wewe/Atmaja sebagai boss penuh:** ditolak (E08); hanya replika memori.
- **Memaksa `schema_version` tertentu:** ditolak sampai loader diaudit.

### 0.11 Rekonsiliasi repository dan arahan desainer (25 Oktober 2026)

Audit awal selesai sebelum kode/scene berubah, basis `5ab018b`; bukti dan risiko di `Audit/phase5-feasibility.md`. Arahan terbaru pengguna **gas Phase5**, audit konflik, aset baru minimal100 menggantikan izin reuse kit art lama. Baseline teknis P5-D01–20 dikerjakan sesuai dokumen; tidak ada nama/quest/ending baru atau replay otomatis.

- **REPO-01:** loader aktual schema5; tambah `Phase5Data` lewat migrasi5→6, pertahankan1→5.
- **REPO-02:** enum global existing `EndingType` diganti nama menjadi **`EndingId {NONE,BAD,NORMAL,TRUE,SECRET}`**, mempertahankan nilai numerik0–4 dan field save `ending`. Tidak ada enum/field ending kedua.
- **REPO-03:** air tetap **`phase4.water_units`** dan `Phase4Ids.Kendi` carry-over. `kendi_water_units` contoh JSON bukan field baru. Cawan fallback memakai owner volume yang sama.
- **REPO-04:** Continue CP8 P4 valid→P5 setelah commit; pemain aktif harus menuruni tangga dan melewati boundary P4 existing. Tidak ada Kos atau vestibule duplikat.
- **REPO-05:** kapasitor finale memberi daya feed/input kamera hanya FINAL_CHOICE, tanpa mengubah baterai persisten; kamera tetap usable pada baterai0.
- **REPO-06:** **semua environment dan kit visual replika diunduh baru**; izin reuse kitP1–4 pada bagian lama dokumen dicabut oleh arahan ini. Raka/HP/Damar mempertahankan identitas campaign dan tidak dihitung sebagai100 model. Identitas/hash asal dan actual placements diaudit.
- **REPO-07:** inti/kabel memakai transaksi singkat CP7 rolling, bukan barrier selama boss. Miss inti3 mengulang paparan5s setelah cooldown tanpa mengembalikan kabel/api; tiga inti tetap tepat tiga Blitz yang berhasil.
- **REPO-08:** satu scene P5 dengan interest zones, bukan sistem streaming/save paralel. Layer24UV/29AR dan feed existing dipakai.
- **REPO-09:** angka waktu75menit, biaya/harga/balance merupakan baseline playtest; jumlah aset/tes tidak membuktikan kesetaraan DreadOut.
- **REPO-10:** VO manusia belum final, manual180QA/WindowsGPU/IrisXe/pacing memerlukan pengujian nyata; catat pending secara jujur.
- **REPO-11 / tambahan pengguna:** CP7 setiap inti/kabel; kematian fase3 tidak mengulang fase1–2; CP8 FINAL_CHOICE; CP9 ending; seluruhnya atomik/rollback. Rename enum global existing menjadi EndingId dengan nilai0–4 yang sama. NORMAL wajib tahan F5s. Test khusus0baterai/0Sanity/0uang/tanpa consumable, write-failure/crash, race input,5ID valid vs tidak lengkap, reload inti/kabel, P4handoff, regresi saveP1–4. Kapasitor diturunkan dari lock ending: tersedia saat FINAL_CHOICE/endingNONE; terpakai hanya sesudah lockBAD/TRUE/SECRET sukses.

---
## 1. Naskah lengkap lima adegan (KANON + catatan implementasi)

> **Catatan pembacaan [REVISI EDITORIAL E01]:** label tombol Selfie pada naskah asli (`[X]`) diseragamkan menjadi `[X]` sesuai binding aktual P1–P4 (V = OTS/FP, X = Selfie). Hint HUD selalu membaca `GameInputConfig`, bukan teks ini. Konflik naskah lain ditutup di §0.7; teks kanon di bawah tidak diubah selain label tersebut.


### ADEGAN 1: Gerbang Pasar Malam & Kios Penukar Nyawa (Menit 00:00 – 15:00)

**Kondisi Awal:** Tanpa jeda *Safe Room* dari akhir Babak 4, Raka menuruni tangga batu di dasar kawah Pendopo Karang Setra. Begitu ia melewati kabut merah di ujung tangga, gravitasi dan arsitektur berubah secara *surreal*. Raka kini berdiri di jalan tanah berlumpur **Pasar Setan**: di atas langit yang berwarna merah gelap tanpa bintang, tampak kincir bianglala berkarat yang berputar terbalik, deretan tenda pasar malam yang robek, serta ratusan cermin tua yang melayang di udara.

#### 1. Monolog Pembuka & Anomali Satu Juta Penonton

Raka mengangkat Smartphone miliknya (`[TAB]`):

* Indikator penonton terkunci di angka **[LIVE | 👁️ 1.000.000 Penonton]**.

* Di dalam cermin-cermin yang melayang di kiri-kanan jalan pasar, tidak tampak pantulan tubuh Raka, melainkan **wajah ribuan penonton *livestream* Raka di kamar mereka masing-masing** yang sedang menatap layar HP dengan mata kosong seperti terhipnotis!

* **Raka (Monolog Suara — tercekat):** *"Ya Tuhan... cermin-cermin ini nampilin muka penonton gue sendiri. Mereka semua lagi disedot kesadarannya lewat siaran gue..."*

* **Rentetan Live Chat #1 (Setengah Manusia Sadar, Setengah Terhipnotis):**
* `@kevin_wijaya`: *"Bang Raka tolong gue gak bisa nutup aplikasi streaming ini di HP gue! Tombol keluarnya ilang!"*

* `@penonton_8821`: *"Jangan matikan siarannya. Kami ingin melihat lelangnya. Jangan matikan siarannya."*

* `Penonton_Terakhir` **[DONASI Rp66.666.666]**: "Satu juta pasang mata adalah bahan bakar yang sempurna, Raka. Sahabatmu Damar sedang dipajang di Panggung Lelang Utama di ujung pasar ini. Lewati tiga gerbang kenangan kalau kau ingin menebus nyawanya."

#### 2. Pengganti Safe Room: Kios Tumbal Pasar Setan

Sebelum memasuki gerbang dalam, Raka menemukan sebuah **Kios Pasar Malam Tua** yang dijaga oleh manekin kayu berwajah topeng Jawa. Di atas meja kios terdapat mesin EDC/QRIS kuno yang menyala merah, bersama secarik **Kertas Aturan Pasar Setan (Item Dokumen #1)**:

> **TEKS DOKUMEN (ATURAN PASAR SETAN):**
> "Di Pasar Setan, uang kertas manusia tidak berlaku. Bayar dengan **Uang Saweran Penontonmu** untuk membeli perbekalan terakhir sebelum menghadap Sang Pemilik Layar."
>
>

* **Interaksi Belanja Darurat (Tanpa Safe Room):**
Pemain menempelkan Smartphone ke mesin kios untuk memotong saldo saweran dan mengisi penuh inventori terakhir:

* **Full Charge + 3x Powerbank Cadangan**.

* **4x Flare Merah Darurat**.

* **4x Kemenyan Bakar**.

---

#### Jalur opsional Arsip Saksi

Sebelum melanjutkan, pemain dapat memeriksa **Pita Rekaman Pertama**. Kios Arsip Bisu di ceruk kanan gerbang sebelum sektor distorsi pertama; bisa diperiksa tanpa membayar. Matikan layar selama 3 detik di depan kursi untuk menghentikan chat palsu, nyalakan AR untuk melihat laci tersembunyi, lalu rekam satu kalimat EVP asli. Laci terbuka dan pita dapat diambil. Kios perbekalan tetap berfungsi terpisah. Pengambilan tidak dihitung sebagai bukti atau pusaka boss. Sebelum meninggalkan babak, status item ini ditampilkan 0/1 atau 1/1 pada Jurnal.

**Catatan implementasi Adegan 1 (Gerbang & Kios):**
- Titik masuk adalah **boundary fisik P4** (tangga batu di bawah Pendopo). Scene P5 tidak membuat vestibule kedua; router Continue dari `PHASE4_CP8` diarahkan ke `PHASE5_CP1` (lihat §0.9, §6.5).
- Viewer terkunci **1.000.000** (overlay naratif, bukan 1.000.000 pesan). Donasi **Rp66.666.666** masuk `wallet_bound` sekali; tidak menambah saldo belanja (E13).
- **Kios Tumbal bukan Safe Room** (E10): hanya `wallet_legitimate`, stok terbatas, ada gangguan chat dan tatapan cermin, tanpa istirahat/skip waktu. Semua isi kios opsional; jalur gratis (dinamo + Altar Doa Pasar) wajib ada di gerbang (E02).
- **Kios Arsip Bisu** terpisah dari Kios Tumbal, gratis, dan tetap dapat dicapai walau Kios Tumbal tidak dipakai. Tiga langkah puzzle Pita Rekaman memakai verifikasi dunia (layar mati 3 s, AR laci, EVP asli), bukan chat.
- Naskah menyebut “mengisi penuh inventori terakhir”; implementasi memakai **tiga paket terpisah** dan receipt atomik, tidak ada isi-penuh otomatis (§5.4).

### ADEGAN 2: Labirin Distorsi Panca-Sengkala — Ujian Seluruh Modul HP (Menit 15:00 – 40:00)

Menggunakan mekanik Kombinasi Seluruh Mekanik (AR, Kamera Selfie, ThermoKlenik, EVP Recorder, Lensa UV Rajah, Stealth, & Infiltrasi Live Chat) dalam 3 koridor distorsi ruang-waktu.

**Kondisi Awal:** Untuk membuka jalan ke **Panggung Lelang Utama**, Raka harus melewati **Labirin Distorsi**—sebuah lorong pasar malam yang ruangannya terus berubah menggabungkan lokasi Babak 1 hingga Babak 4.

#### 1. Sektor Distorsi Pertama: Koridor Beku (Kamera Selfie `[X]` × `ThermoKlenik` `[T]`)

Tenda kain pasar tiba-tiba menyempit menjadi gabungan **Koridor Beton Rusunawa Griya Asri** dan **Kamar Mayat RSUD Sido Waras** yang kedap suara. Di ujung lorong terdapat **3 Pintu Cermin**, hanya satu pintu yang asli.

* **Eksekusi Gameplay:**
1. Bayangan **Kuntilanak Merah** muncul menempel tepat di belakang punggung Raka! Pemain wajib langsung menekan **`[X]` (Kamera Selfie)** untuk menahan pergerakannya dari belakang sambil terus berjalan maju.

2. Sambil memantau kamera Selfie, pemain mengaktifkan **`ThermoKlenik` (`[T]`)** untuk memeriksa suhu di ketiga Pintu Cermin di ujung lorong.

3. Pintu Kiri dan Kanan bersuhu `+50°C` (jebakan api Banaspati), sedangkan **Pintu Tengah** bersuhu **`-22°C`** (jalur asli). Pemain membuka Pintu Tengah dan melompat masuk tepat sebelum Kuntilanak Merah menerkam!

#### 2. Sektor Distorsi Kedua: Jembatan Gilingan Bambu (`EVP Recorder` × `Lensa UV Rajah` `[R]` × `Infiltrasi Live Chat`)

Begitu melewati cermin, Raka berada di atas jembatan besi **Pabrik Gula Sindang Laut** yang dikelilingi **Hutan Bambu Karang Setra** di atas jurang kabut tanpa dasar.

* **Eksekusi Gameplay:**
1. Dari dua cabang jembatan di depan, terdengar teriakan **Damar** meminta tolong dari cabang kiri, sementara kolom **Live Chat yang terinfiltrasi hantu** serempak menyuruh Raka berlari ke cabang kiri!

2. Pemain memeriksa grafik **`EVP Recorder`**: suara dari kiri bergelombang **Merah Bergerigi** (tiruan suara **Wewe Gombel**), sedangkan dari cabang kanan terdengar suara ketukan kode Morse asli dari Damar (**Gelombang Hijau**).

3. Pemain menekan **`[R]` (`Lensa UV Rajah`)** sambil berjalan di cabang kanan untuk melompati papan-papan jembatan yang bertanda **Rajah Ungu (Lantai Runtuh)** dan menyiram **1 Obor Santet** yang menghalangi pintu keluar dengan kendi yang dibawa dari Babak 4; titik isi ulang disediakan di sebelah gerbang.

#### 3. Sektor Distorsi Ketiga: Ruang Jahit Cermin (`Stealth Outlast` `[F]` × `Lensa AR` `[Q]`)

Raka jatuh menembus pintu keluar dan mendarat kembali di tempat semuanya bermula: replika terdistorsi dari **Ruang Bawah Tanah Rumah Keluarga Atmaja**!

* **Eksekusi Gameplay:**
1. Lampu padam dan terdengar bunyi gesekan gunting rumput (**`"SRAAAK... KLING..."`**) milik **Nyonya Sri Atmaja** yang berpatroli bersama **Genderuwo** di lorong berbentuk 'U'.

2. Pemain wajib menekan **`[F]` untuk mematikan layar HP** dalam kegelapan total dan menyelinap dari lemari ke kolong meja kerja.

3. Di ujung ruangan berdiri **Cermin Rias Antik Besar** tempat Damar ditarik oleh belasan tangan pada rekaman Handycam di Prolog.

4. Pemain menyalakan **Lensa AR (`MataBatin.apk` — `[Q]`)**: permukaan kaca cermin yang retak berubah menjadi portal cair berwarna hijau *glitch*. Raka berlari menembus masuk ke dalam cermin tersebut!

---

**Catatan implementasi Adegan 2 (Tiga Sektor Distorsi):**
- Semua hantu di sektor adalah **Replika Memori** (E08): konstruksi `Penonton_Terakhir`, tanpa HP bar, tidak dapat disegel, tidak mengubah `location_seals`, larut ketika sektor selesai. Aset visual diunduh baru sesuai arahan pengguna, dengan ID `P5_REPLICA_*` terpisah; tidak menggunakan kit art P1–P4.
- Sektor 1: Selfie (`X`) memberi **jeda tatap**, Thermo membaca pintu; chat/banner boleh berbohong, **angka Thermo dan rajah UV dunia tidak**. Sektor 2: grafik EVP dan rajah UV memutuskan jalur; **Kendi carry-over P4** dipakai pada satu Obor Santet, titik isi ulang di samping gerbang sektor (E09). Sektor 3: `F` mematikan layar dan menghentikan chat; `Q` (AR) membuka portal cermin.
- Setiap sektor punya checkpoint dan titik pemulihan fisik (dinamo, altar doa); sektor yang sudah selesai **tidak diulang** setelah kematian (progres sektor persisten, mengikuti pola P4).

### ADEGAN 3: Panggung Lelang Utama & Kebenaran `Penonton_Terakhir` (Menit 40:00 – 50:00)

**Kondisi Awal:** Raka keluar dari balik cermin dan tiba di **Alun-Alun Panggung Lelang Pasar Setan**—sebuah amfiteater melingkar raksasa yang dikelilingi ribuan layar proyeksi melayang yang menampilkan kolom *Live Chat* raksasa.

#### 1. Pertemuan Kembali dengan Damar & Bukti Terakhir

Di tengah panggung kayu, **Damar Prasetyo** tergantung lemas di dalam kurungan bambu berbentuk lensa kamera raksasa, dengan kabel-kabel urat hitam yang menancap di pelipisnya.

* **Raka (Dialog Suara — berlari menghampiri kurungan):** *"Damar! Dam, bangun! Gue di sini!"*
* **Damar (Suara lemah — membuka mata setengah sadar):** *"Ka... kenapa lo beneran dateng sambil nyalain live?! Gue udah tulis di catatan... jangan bawa penonton lo ke sini!"*

* **Raka (Dialog Suara):** "Gue nggak punya pilihan, bajingan itu ngancam bakal bunuh lo kalau gue matiin kamera! Gimana cara gue ngelepasin kurungan ini?!"

* **Damar (Dialog Suara):** *"Di meja altar depan kurungan gue ada **Catatan Asli Mbah Suro**. Pindai 3 bukti terakhir di panggung ini pakai HP lo buat memaksa wujud asli `Penonton_Terakhir` keluar dari jaringan sinyal!"*

#### 2. Memindai 3 Bukti Klenik `Penonton_Terakhir`

Pemain dengan cepat menggunakan ketiga modul HP di sekitar Panggung Lelang untuk membuka kunci eksorsisme terakhir:

1. **Bukti #1 (`Lensa AR` `[Q]`):** Memotret **Server Batu Kuno & Kabel Urat Gaib** di bawah kurungan Damar yang menyedot energi dari keempat segel sebelumnya.

2. **Bukti #2 (`ThermoKlenik` `[T]` + `Lensa UV` `[R]`):** Memindai **Kontrak Darah Rp500.000.000** di atas podium lelang yang memiliki suhu **`di luar rentang sensor (tampilan `-- °C`; anomali gaib, bukan pengukuran 0 K)`** dan simbol Rajah Mata Terbuka.

3. **Bukti #3 (`EVP Recorder`):** Merekam suara jutaan bisikan penonton yang menyatu menjadi satu suara entitas di balik layar.

> **TEKS DOKUMEN TERAKHIR (MANUSKRIP RAHASIA MBAH SURO):**
> "Kami para dukun masa lalu salah menilai **Penonton Terakhir**. Dia bukan sekadar iblis hutan atau penunggu makam. Dia adalah entitas kuno pemakan obsesi manusia yang tumbuh dari rasa haus orang-orang untuk menonton penderitaan sesamanya. Dulu kami memberinya makan lewat pertunjukan wayang darah di Pasar Malam; kini lewat aplikasi `MataBatin.apk`, satu juta manusia di balik layar ponsel telah menjadi tumbalnya secara sukarela!"
>
>

---

**Catatan implementasi Adegan 3 (Panggung Lelang):**
- Tiga bukti bebas urutan. Bukti #2 memerlukan **dua pengamatan** (Thermo `-- °C` dan UV Rajah Mata Terbuka) karena AR/UV tidak ditumpuk (E05); satu ID evidence setelah keduanya sah.
- Catatan Asli Mbah Suro (Dokumen #2) opsional untuk progres, wajib tersedia untuk dibaca ulang di Jurnal. Damar boleh diajak bicara berulang tanpa mengubah state.
- **Ruang Cermin Saksi** (antechamber sebelum arena) menampilkan Arsip N/5 aktual dan **Cermin Jalan Pulang** menuju gerbang untuk mengambil item kelima. Arena baru terkunci setelah konfirmasi Jurnal + empat segel valid + `PHASE5_CP6` sukses (§3.5, §6.4).

### ADEGAN 4: Final Boss Fight — Eksorsisme Multi-Fase `Penonton_Terakhir` (Menit 50:00 – 68:00)

Menggunakan mekanik Pertarungan Multi-Fase tanpa Safe Room yang mengombinasikan seluruh mekanik game dari Prolog hingga Babak 4.

#### 1. Kemunculan Boss Utama: Entitas `Penonton_Terakhir` ("Sang Dalang Layar")

Begitu Jurnal Klenik terkonfirmasi, seluruh layar proyeksi di langit Pasar Setan menyatu membentuk sosok mengerikan setinggi 4 meter yang turun ke atas panggung: **Entitas `Penonton_Terakhir`—sosok humanoid tinggi kurus berjubah kain layar bioskop yang terbakar, memiliki puluhan tangan pucat yang keluar dari balik jubahnya, dan bagian kepalanya berupa **tumpukan lensa kamera tua serta cermin retak yang memancarkan cahaya merah berkedip**!

* **Penonton_Terakhir (Suara Gema Ribuan Manusia):** "Kau ingin tahu siapa aku, Raka Wijaya? Akulah alasan kanalmu hidup kembali. Akulah satu juta pasang mata yang menolak tidur demi melihat sahabatmu mati di depan kamera!"

#### 2. Pertarungan 3 Fase (Ujian Puncak Seluruh Mekanik)

Untuk menghancurkan **3 Inti Segel** `Penonton_Terakhir`, pemain harus melewati 3 Fase berturut-turut di arena Panggung Lelang:

* **FASE 1 — Fase Distorsi Ilusi (`Lensa UV Rajah` `[R]` + `Generator Lampu Sorot` + `Blitz`):**
1. `Penonton_Terakhir` menyelimuti dirinya dengan **Kabut Sengkala Pekat** dan membelah diri menjadi **4 Sosok Kembar** yang mengepung Raka sambil memunculkan jebakan lantai runtuh di panggung kayu.

2. Pemain wajib menyalakan **`Lensa UV Rajah` (`[R]`)**: untuk menghindari lantai ber-rajah ungu sekaligus melihat **sosok mana yang memiliki benang merah terhubung ke kurungan Damar** (itulah wujud aslinya).

3. Pemain memancing wujud asli tersebut ke dekat **Generator Lampu Sorot Panggung**, menyalakan sakelarnya untuk membakar Kabut Sengkala-nya, lalu menembakkan **Jepretan Blitz (`[Klik Kiri]`)** tepat ke kepala lensanya! (**Inti Pertama Pecah!**)

* **FASE 2 — Fase Teror Punggung & Chat Palsu (`Kamera Selfie` `[X]` + `ThermoKlenik` `[T]` + `Flare` `[G]` + `Blitz`):**
1. `Penonton_Terakhir` menghancurkan seluruh lampu sorot panggung hingga arena gelap gulita! Ia berubah menjadi mode tak kasat mata dari depan (seperti Kuntilanak Merah) sambil membanjiri **Live Chat dengan komentar palsu** yang menyuruh Raka menoleh ke arah yang salah.

2. Pemain wajib menekan **`[H]` (Bakar Kemenyan)** untuk menjaga *Sanity*, lalu mengangkat HP dalam **Mode Kamera Selfie (`[X]`)** sambil memantau suhu di **`ThermoKlenik` (`[T]`)**.

3. Begitu suhu turun drastis ke **`-30°C`** dan sosok `Penonton_Terakhir` terlihat di layar Selfie sedang membuka rahang lensanya tepat di belakang leher Raka, pemain menekan **`[G]` untuk menjatuhkan Flare Merah Darurat**!

4. Cahaya Flare menghancurkan perisai kabutnya, dan pemain langsung menembakkan **Jepretan Blitz (`[Klik Kiri]`)** lewat mode Selfie! (**Inti Kedua Pecah!**)

* **FASE 3 — Fase Overload Sinyal (`Lensa AR` `[Q]` + `Stealth` `[F]` + `Blitz`):**
1. Dengan tubuh yang mulai retak, `Penonton_Terakhir` memanggil gelombang **Bola Api Banaspati** yang menyapu panggung.

2. Pemain harus mematikan layar HP (`[F]`) dan merunduk di balik peti kayu panggung saat gelombang api lewat, lalu menyalakan **Lensa AR (`[Q]`)** untuk melihat dan memutus **3 Kabel Roh** yang mengikat kurungan Damar.

3. Begitu ketiga kabel roh putus, kurungan Damar terbuka! `Penonton_Terakhir` menjerit murka dengan **Inti Ketiga** yang terbuka lebar di dadanya. Pemain melepaskan **Jepretan Blitz** dan menjatuhkan entitas tersebut berlutut di tengah panggung!

---

**Catatan implementasi Adegan 4 (Boss tiga fase):**
- `Penonton_Terakhir` **bukan HP bar**: ia hanya rentan pada jendela inti yang dibuka mekanik (UV+lampu, Selfie+Thermo+cahaya, AR+putus kabel). Tiga inti dikomit satu per satu (CP7 rolling), sehingga mati pada fase 3 tidak mengulang fase 1–2.
- Pengganti `H`/`G` yang diwajibkan naskah: Kemenyan opsional dan **Lentera Panggung** menggantikan Flare bila stok nol (E06, E07). Tidak ada fase yang hanya dapat diselesaikan dengan item toko.
- Blitz inti = **tepat 3** (15 poin/tembakan, cooldown 6 s). Miss tidak mengonsumsi kesempatan permanen; perisai kembali, sumber daya diisi ulang lewat dinamo.

### ADEGAN 5: Penentuan 4 Ending — "Layar Terakhir" (Menit 68:00 – 75:00)

Sesuai Master GDD, bagian ini menentukan 4 Ending permainan berdasarkan keputusan dan eksekusi mekanik pemain di momen terakhir.

**Kondisi Puncak:** `Penonton_Terakhir` berlutut sekarat namun belum musnah sepenuhnya. Damar jatuh tersungkur di lantai panggung di sebelah Raka. Smartphone di tangan kiri Raka retak parah; kapasitor ritual mengunci **Baterai tersisa 5% (Hanya cukup untuk 1 tindakan terakhir)**.

* **Penonton_Terakhir (Suara Berbisik di Kepala Raka):** "Jika kau membunuhku sekarang, uang Rp500.000.000 dan satu juta penontonmu akan lenyap... atau jika kau mematikan siaranmu, gerbang ini akan runtuh menguburmu selamanya. Pilih, Raka... siapa yang akan menjadi tontonan terakhir malam ini?"

Di layar muncul **4 Pilihan Eksekusi Mekanis (cabang keempat hanya aktif bila Arsip Saksi lengkap)** (Pemain bebas menggerakkan karakter; **tiga aksi dasar selalu tersedia, aksi keempat hanya bila arsip tervalidasi** — lihat E03):

---

#### CABANG 1: BAD ENDING — "TUAN RUMAH BARU" (Mengutamakan Uang & Viralitas)

* **Cara Memicu:** Pemain mengarahkan kamera belakang HP ke arah **Damar yang sedang terluka**, tahan bidikan kontrak selama 2 detik hingga label TUMBAL muncul, lalu menekan **`[Klik Kiri Mouse]` (Memotret Damar sebagai tumbal pengganti)** untuk menyelesaikan kontrak Rp500.000.000.

* **Cutscene Bad Ending:**
* Kilatan Blitz kamera menyambar tubuh Damar. Damar menatap Raka dengan tatapan tak percaya saat belasan tangan pucat menarik tubuh Damar masuk ke dalam lantai cermin.

* Layar berpindah ke **Kamar Kos Raka (1 Bulan Kemudian)**: Kamar itu kini telah berganti menjadi studio *streaming* mewah. Di meja terdapat kuitansi lunas operasi Rumah Sakit Ibu Raka dan saldo bank **Rp500.000.000**.

* Namun saat kamera berbalik menyorot wajah Raka di depan cermin studio, mata Raka tampak memancarkan cahaya merah berkedip, sementara di belakang kursinya berdiri bayangan Damar dengan leher miring. Raka mengetik komentar di sebuah kanal *streamer* pemula dengan akun baru bernama: **`Penonton_Terakhir`**.

---

#### CABANG 2: NORMAL / BITTERSWEET ENDING — "SIARAN TERPUTUS" (Pengorbanan Raka)

* **Cara Memicu:** Pemain tidak memotret siapa pun, melainkan berjalan ke tepi panggung, lalu **menekan dan menahan tombol `[F]` (Matikan Layar / Hancurkan Smartphone)** selama 5 detik penuh untuk memutus paksa sambungan *livestream*.

* **Cutscene Normal Ending:**
* **Raka (Dialog Suara — tersenyum tipis ke arah Damar):** *"Gue udah janji bakal bawa lo pulang, Dam. Maafin gue kalau gue nggak bisa ikut."*
* Raka menghantamkan Smartphone miliknya ke batu altar hingga hancur berkeping-keping!
* Begitu sinyal siaran mati, suplai energi spiritual dari 1.000.000 penonton terputus seketika. Entitas `Penonton_Terakhir` menjerit dan hancur menjadi debu, namun seluruh dimensi **Pasar Setan** ikut runtuh!

* Raka mendorong tubuh Damar masuk ke dalam portal cermin Rumah Atmaja tepat sebelum cermin itu pecah.

* **Epilog:** Damar terbangun sendirian di halaman Rumah Tua Atmaja di tengah hujan pagi sambil memegang **HP Raka yang sudah hancur**. Di berita nasional, satu juta penonton dilaporkan terbangun dari trans misterius, sementara Raka Wijaya dinyatakan hilang selamanya. Namun saat Damar menatap layar HP Raka yang mati, muncul pantulan siluet Raka yang melambaikan tangan dari balik kaca gelap.

---

#### CABANG 3: TRUE ENDING — "JEPRETAN TERAKHIR" (Menyelamatkan Damar & Menghancurkan Kutukan)

* **Cara Memicu (Menggunakan Seluruh Pemahaman Mekanik Game):**
1. Pemain memapah Damar ke portal dan menyiapkan sinkronisasi ritual pada reticle inti entitas.
2. Pemain berjalan memapah bahu Damar di depan portal cermin, lalu menekan **`[X]` (Mengaktifkan Kamera Depan / Selfie Mode)** lalu **`[Q]` (`Lensa AR`)** dan **`[R]` (`Lensa UV Rajah`)** secara berurutan untuk mengisi kalibrasi sinkronisasi khusus ending. AR dan UV tetap saling menggantikan di gameplay biasa!

3. Di layar Selfie HP, terlihat wajah **Raka dan Damar** di bagian depan, sementara tepat di belakang punggung mereka tampak wujud inti **`Penonton_Terakhir`** yang hendak menerkam.

4. Dengan membalik lensa ke arah dirinya sendiri dan memantulkan cahaya ke cermin di belakang entitas, Raka menekan **`[Klik Kiri Mouse]` (Jepretan Blitz Penutup — Menguras Habis Sisa Baterai ke 0%)**!

* **Cutscene True Ending:**
* **Raka (Teriakan Puncak):** *"Kalian mau tontonan terakhir?! INI LAYAR TERAKHIR BUAT LO!"*
* Kilatan cahaya putih Blitz dari Kamera Selfie memantul bolak-balik di antara ribuan cermin Pasar Setan (*Infinite Mirror Feedback Loop*), menciptakan ledakan cahaya murni yang membakar habis tubuh `Penonton_Terakhir` beserta kode sumber aplikasi `MataBatin.apk` di dalam HP Raka!

* Gelombang kejut cahaya itu melontarkan tubuh Raka dan Damar keluar menembus portal cermin!
* **Epilog (Pukul 05:30 WIB — Subuh di kota Raka):**
* Terdengar suara azan subuh samar dan deru hujan yang mulai reda. Raka dan Damar terbatuk-batuk terbangun di atas aspal basah di depan gerbang **Rumah Tua Keluarga Atmaja**.

* Raka memeriksa Smartphone-nya: aplikasi `MataBatin.apk` telah terhapus permanen (`[FILE CORRUPTED]`). Namun di aplikasi perbankan dan *dashboard* *streaming*-nya, donasi tulus dari ratusan ribu penonton manusia yang tersadar pagi itu tetap masuk sebesar **Rp125.000.000**—lebih dari cukup untuk melanjutkan perawatan Ibu Raka setelah tunggakan kos dan deposit operasi dibayar seusai Prolog.

* **Damar (Dialog Suara — merangkul bahu Raka sambil tertawa lemas):** *"Ka... mulai besok, kita ganti channel aja ya. Bikin konten kuliner atau mukbang kek... jangan uji nyali lagi."*
* **Raka (Dialog Suara — menghela napas lega dan membuang kartu memori rusak ke selokan):** *"Setuju. Malam Keramat resmi tamat malam ini."*

* Kamera perlahan naik ke arah langit pagi kota Raka saat matahari mulai terbit → *Fade to Black* diiringi musik penutup gamelan akustik yang tenang → **CREDITS ROLL: "L A Y A R   T E R A K H I R"**.

---

#### CABANG 4 SECRET ENDING SAKSI TANPA LAYAR


Secret Ending bernama **Saksi Tanpa Layar**. Satu item rahasia tersedia di masing-masing Babak 1–5; Prolog hanya memberi petunjuk simbol mata tertutup, tanpa collectible wajib tambahan. Lima item merupakan **Arsip Saksi**, terpisah dari tiga bukti identifikasi, pusaka eksorsisme, dan empat segel lokasi. Pemain dapat menamatkan tiga ending dasar tanpa mengambil item rahasia.

Setiap item memberi satu potongan kesaksian korban dan ikon di halaman Arsip Saksi pada Jurnal. Mengambil kelima ID unik dalam campaign yang sama membuka jalur Secret Ending. Membaca catatan memberi konteks lore dan tidak menjadi syarat tambahan. Pemain menyiapkan ritual dengan memvalidasi arsip pada Cermin Saksi di ruang sebelum panggung lelang; persiapan tidak otomatis memilih ending. Jika persiapan terlewat, validasi tetap dapat dilakukan pada cermin yang sama saat pilihan akhir.

Sebelum meninggalkan tiap babak, Jurnal menampilkan status Arsip Saksi 0/1 atau 1/1 dan lokasi yang masih dapat diperiksa. Pintu keluar memerlukan interaksi sadar; item tetap bisa diambil selama pemain belum menyelesaikan transisi. Melewatkan item babak terdahulu menutup secret pada campaign itu sampai babak tersebut dimainkan ulang melalui save yang relevan. Chapter select merupakan pengembangan lanjutan, bukan syarat MVP.

Pengambilan item langsung menyimpan ID unik pada campaign secara atomik. Reload checkpoint dan kematian tidak menghapus collectible yang telah disimpan. Objek yang sudah diambil tidak memberi duplikat atau uang. New Game memulai himpunan kosong. Jumlah 5 tanpa lima ID yang benar tidak cukup untuk unlock. Membaca catatan disimpan terpisah; validasi arsip dapat diulang setelah reload.

Setelah inti ketiga boss pecah, bantu Damar ke portal dengan E. Jika arsip sudah tervalidasi, Cermin Saksi memiliki simbol mata tertutup dan lima slot lengkap. Tahan E selama 3 detik pada cermin untuk memutar seluruh kesaksian; progress terlihat dan dapat dibatalkan sebelum selesai. Kamera berpindah ke mode Arsip Saksi. Arahkan ke cermin berbidikan emas dan tekan LMB untuk merekam nama korban dengan sisa daya ritual 5%. Target Damar tidak valid dalam mode ini. Aksi tersebut mengunci Secret Ending tepat satu kali.

Lima kesaksian membatalkan kontrak yang menganggap semua korban dan penonton menyerahkan diri secara sukarela. Wajah-wajah di cermin mendapatkan nama, berhenti menatap siaran, dan berubah menjadi bayangan manusia yang keluar melalui pintu pasar. Penonton_Terakhir kehilangan ikatan terhadap arsip korban; Pasar Setan runtuh menjadi lapangan pasar kosong. Raka dan Damar pulang bersama dan penonton sadar. Adegan tambahan menunjukkan arwah Tari, Lastri dan anaknya, sembilan anak buruh, serta warga Karang Setra meninggalkan jaringan ritual. True Ending tetap menyelamatkan Raka, Damar, dan penonton serta menghancurkan jaringan aktif; Secret memperluas penyelesaian ke korban lama yang terkurung dalam arsip.

Dialog klimaks: Raka berkata, "Mereka bukan angka penonton. Mereka punya nama." Damar menjawab, "Rekam kesaksiannya, Ka. Biar kali ini mereka yang pulang." Epilog memperlihatkan keduanya menyerahkan catatan korban kepada keluarga, dengan sisa donasi sah Rp125.000.000. HP tidak lagi memuat MataBatin.apk. Shot terakhir menampilkan kursi penonton kosong dan cermin biasa tanpa wajah yang tertinggal.

Pemicu ending dasar tetap tersedia bila secret terbuka. BAD: bidik Damar dalam kamera belakang, tahan bidikan kontrak 2 detik lalu LMB setelah label TUMBAL muncul. NORMAL: tahan F 5 detik untuk memutus siaran dan menghancurkan HP; Damar dan penonton selamat, Raka tertinggal. TRUE: papah Damar ke portal, pilih X (Selfie), aktifkan Q lalu R untuk mengisi kalibrasi sinkronisasi ritual, kemudian LMB pada inti entitas yang terlihat di Selfie; Raka, Damar, dan penonton selamat. SECRET: lima item lengkap, validasi Cermin Saksi, tahan E di cermin lalu LMB pada reticle emas. Input ending hanya diterima pada state FINAL_CHOICE; sebelum itu berlaku kontrol biasa. Ending memakai satu enum tunggal dan kunci transisi atomik agar dua cutscene tidak terpicu bersamaan.

**Catatan implementasi Adegan 5 (FINAL_CHOICE dan empat ending):**
- Seluruh input ending hanya hidup pada state `FINAL_CHOICE` dan memakai **satu enum ending + kunci transisi atomik** (§4.8, §6.4). “Baterai 5%” adalah **Kapasitor Ritual** (E04), bukan baterai persisten HP.
- Tiga aksi dasar selalu tersedia; Secret hanya muncul bila lima ID unik valid. Normal tidak memakai LMB; Bad dan True/Secret memakai kapasitor sekali pakai. Epilog uang (Rp500.000.000, Rp125.000.000) adalah **state sinematik**, tidak menulis `wallet_legitimate` (E14).
- Nama korban pada Cermin Saksi diambil dari data kesaksian item arsip yang sudah ada; **dokumen ini tidak mengarang nama korban baru** (E15).


---

## 2. Kontrak implementasi dan event registry

### 2.1 Input dan kepemilikan aksi

| Aksi | Default dokumen | Semantik P5 dan aturan konflik |
|---|---|---|
| Gerak | `WASD`, `Shift`, `Ctrl`, `Space` | Sprint bising; jongkok mengurangi deteksi replika. Kecepatan controller tidak diubah (walk 1,7 / sprint 4,2 m/s per P4 REPO-09). |
| Angkat HP | `TAB` / tahan `RMB` | Dua cara global tetap; release RMB mengembalikan view sebelumnya. Satu HP, satu baterai. |
| Perspektif | `V` | OTS/FP; bukan Selfie. |
| Selfie | **`X`** | Kamera depan. Sektor 1 (jeda tatap), Fase 2 (punggung boss), True Ending (sinkronisasi). |
| `Q` | AR | Kabel roh, server batu, laci Pita, portal cermin sektor 3, Fase 3. **Tidak** menggantikan UV. |
| `R` | UV Rajah v2 | Rajah lantai runtuh, benang merah asli, Rajah Mata Terbuka. Satu filter, bukan kamera kedua. |
| `T` | ThermoKlenik | Pintu cermin, Kontrak Darah `-- °C`, `-30°C` Fase 2. Bidik stabil 2 s (aturan P2/P4). |
| `C` | EVP v2 | Pita Rekaman, jembatan, bukti #3. Metadata authored `Authentic`/`Mimic`. |
| `F` | Layar mati/nyala | Menghentikan chat palsu, memutus LOS cahaya, bersembunyi di sektor 3 dan Fase 3. **Tahan 5 s hanya valid pada Zona Tepi di `FINAL_CHOICE`** (Normal). |
| `G` | Flare | Aktif di P5. Stok 0 → menurunkan Lentera Panggung bila dibawa/berada di dekatnya. |
| `H` | Kemenyan | Opsional; konsumsi 1 instance bila tersedia dan cooldown selesai; pemulihan ke 100 (Rancangan.MD), cooldown 12 s. |
| `E` | Interaksi | Dinamo, Altar Doa, Kendi/Cawan, obor, lentera, generator, kabel (tahan 2 s), memapah Damar, cermin (tahan 3 s). |
| Klik kiri | Capture (1%) / Blitz (15%) | Blitz hanya pada konteks valid (jendela inti atau aksi `FINAL_CHOICE`). Foto bukti dan Blitz tidak boleh dalam satu klik. |
| `J` | Jurnal | Bukti 0/3, dokumen, Arsip N/5, status inti, petunjuk objektif, petunjuk pilihan akhir. |

**Prioritas deterministik:** pause/cutscene > `F` darurat (stealth) > `E` objek ritual/sektor sah > `H` > interaksi UI HP/Jurnal > capture/Blitz > aksi non-kritis. Chat yang menyerupai prompt sistem **tidak pernah memiliki akses ke action dispatcher** (QA-179 P4 diwarisi). Input hint dari binding nyata.

### 2.2 State machine perjalanan global

```text
NOT_STARTED
  -> P4_HANDOFF (PHASE4_CP8 + empat segel valid; boundary fisik)
  -> GATE_ARRIVAL (CP1; live 1.000.000; donasi dana-terikat sekali; dinamo+altar doa dapat dijangkau)
  -> GATE_HUB { KIOS_TUMBAL (opsional), KIOS_ARSIP_BISU (opsional, 3 langkah), DOC_RULES }
  -> SECTOR_1_FROZEN_CORRIDOR (CP2 sebelum masuk; Selfie x Thermo; Replika Kuntilanak Merah)
  -> SECTOR_2_BAMBOO_BRIDGE   (CP3; EVP x UV x chat; Kendi/Cawan; Obor Santet penghalang)
  -> SECTOR_3_SEWING_MIRROR   (CP4; F x AR; Replika Sri Atmaja + Genderuwo)
  -> STAGE_AUCTION (CP5; Damar; tiga bukti bebas urutan; Dokumen #2)
  -> JOURNAL_CONFIRMED (3 bukti unik + empat segel valid; konfirmasi eksplisit)
  -> MIRROR_ROOM (Ruang Cermin Saksi: Arsip N/5; Cermin Jalan Pulang aktif)
  -> ARENA_LOCKED (CP6 commit sebelum boss aktif)
  -> BOSS_PHASE_1 -> CORE_1_BROKEN (CP7 rolling)
  -> BOSS_PHASE_2 -> CORE_2_BROKEN (CP7 rolling)
  -> BOSS_PHASE_3 (3 kabel) -> CORE_3_BROKEN (CP7 rolling) -> DAMAR_FREED
  -> FINAL_CHOICE (CP8; kapasitor ritual 5%)
       |-- BAD_ENDING | NORMAL_ENDING | TRUE_ENDING | SECRET_ENDING (bersyarat)
  -> ENDING_LOCKED (satu enum, atomik)
  -> ENDING_CUTSCENE -> P5_COMPLETE (CP9) -> CREDITS
```

**Turunan stage** dibangun dari receipts; jangan persist boolean ganda `stage=...` bersama `completed_event_ids` kecuali loader existing mensyaratkan dan invarian diuji. Progres sektor dan inti **persisten setelah transaksi sukses**; transform replika dan timer paparan tidak.

### 2.3 Registry event satu-kali dan pembeda state

| Event / ID konseptual | Pemicu dan syarat | Efek / status simpan |
|---|---|---|
| `P5_ARRIVE_GATE` | `PHASE4_CP8` + empat segel valid | Boundary → gerbang; `PHASE5_CP1` setelah kontrol kembali; tidak ada Kos |
| `P5_LIVE_LOCK_1M` | HP pertama kali menyala di gerbang | Viewer 1.000.000, chat #1 sekali, donasi Rp66.666.666 → `wallet_bound` sekali |
| `P5_DOC_RULES_READ` | Ambil Kertas Aturan Pasar Setan | Dokumen #1; opsional dibaca |
| `P5_SHOP_POWERBANK` / `_FLARE` / `_KEMENYAN` / `_CHARGE` | `wallet_legitimate` cukup, receipt valid | Debit atomik + instance unik; opsional |
| `P5_SECRET_CHAIR_SILENCE` | Layar mati ≥3 s di radius kursi penonton kosong | Chat palsu pada kursi berhenti; langkah puzzle, bukan pickup |
| `P5_SECRET_AR_DRAWER` | AR aktif memperlihatkan laci tanda mata tertutup | Laci terbuka secara visual |
| `P5_SECRET_EVP_TRUE` | EVP merekam kalimat `Authentic` “Saksi tidak membeli korban” | Laci siap diambil |
| `P5_SECRET_PICKUP` | Ambil Pita Rekaman, tiga langkah valid | Atomik `secret_items.Add(SECRET_P5_PITA_PERTAMA)` lalu popup/objek hilang |
| `P5_S1_CLEAR` | Masuk Pintu Tengah (`-22°C`) | Sektor 1 selesai, CP3 |
| `P5_S2_CLEAR` | Obor Santet penghalang padam dan keluar jembatan | Sektor 2 selesai, CP4 |
| `P5_S3_CLEAR` | Menembus portal Cermin Rias | Sektor 3 selesai, CP5 |
| `P5_STAGE_REACHED` | Tiba Panggung Lelang | Damar dan kurungan; Catatan Asli di meja altar |
| `P5_DOC_SURO_READ` | Ambil Catatan Asli / Manuskrip | Dokumen #2; opsional |
| `P5_EVIDENCE_SERVER_AR` | AR stabil pada Server Batu + Kabel Urat Gaib | Bukti 1/3 |
| `P5_CONTRACT_THERMO_OK`, `P5_CONTRACT_UV_OK` | Thermo `-- °C` stabil; UV Rajah Mata Terbuka | Sub-langkah; belum bukti |
| `P5_EVIDENCE_CONTRACT` | Kedua sub-langkah sah | Bukti 2/3 (bebas urutan sub-langkah) |
| `P5_EVIDENCE_EVP_CROWD` | EVP bisikan penonton menyatu, `Authentic` | Bukti 3/3 |
| `P5_JOURNAL_CONFIRMED` | 3 bukti unik + konfirmasi eksplisit + empat segel | Deduksi entitas, sekali |
| `P5_ARCHIVE_VALIDATED` | Cermin Saksi memvalidasi lima ID unik | Opsional, dapat diulang; tidak memilih ending |
| `P5_ARENA_LOCK` | `PHASE5_CP6` sukses setelah konfirmasi | Boss aktif; tidak sebelum disk commit |
| `P5_CORE_1_BROKEN` / `_2_` / `_3_` | Blitz valid pada jendela inti masing-masing | **Commit atomik**: baterai −15, receipt inti, CP7 rolling |
| `P5_CABLE_1_CUT` / `_2_` / `_3_` | Tahan `E` 2 s pada kabel roh yang terlihat via AR | Commit kecil per kabel (progres tidak hilang) |
| `P5_DAMAR_FREED` | Tiga kabel putus | Kurungan terbuka; Damar `SLUMPED` setelah inti 3 |
| `P5_FINAL_CHOICE_ENTER` | Inti 3 pecah, entitas berlutut | `PHASE5_CP8` atomik; kapasitor ritual aktif 5% |
| `P5_ENDING_LOCKED_BAD` / `_NORMAL` / `_TRUE` / `_SECRET` | Aksi valid pada `FINAL_CHOICE` | `TryLockEnding` atomik **tepat satu** |
| `P5_COMPLETE` | Cutscene ending selesai/commit | `PHASE5_CP9`, `endings_seen`, credits |

**ID di atas adalah usulan namespace**, bukan nama API yang dipastikan ada. Gunakan alias/migrasi jika repo memakai penamaan berbeda; jangan menyimpan dua representasi item yang sama.

### 2.4 Precondition, denial dan trigger yang tidak boleh membuat softlock

| Gerbang | Boleh bila | Feedback bila gagal | Perilaku ilegal |
|---|---|---|---|
| Masuk P5 | `PHASE4_CP8` + empat segel | Layar “Empat segel belum lengkap” + kembali ke boundary | Mulai tanpa segel; segel palsu |
| Kios Tumbal | Saldo sah ≥ harga | Tombol nonaktif tanpa memblokir sisa babak | Memotong `wallet_bound`; paywall |
| Pita Rekaman | Tiga langkah puzzle valid | Laci tak bergeser; clue lingkungan | Pickup tanpa puzzle; membayar |
| Pintu Cermin (S1) | Pintu Tengah (`-22°C`) | Pintu panas → api telegraph, −Sanity, kembali ke awal koridor | Instakill; pintu benar berpindah |
| Obor penghalang (S2) | Kendi/Cawan air>0, `E` pada obor | “KENDI KOSONG” → tunjuk titik isi ulang | Air negatif; obor penghalang tidak bisa dipadamkan |
| Portal Cermin (S3) | AR aktif memperlihatkan permukaan cair | “Permukaan cermin biasa” bila tanpa AR | Teleport paksa; AR digantikan UV tanpa keputusan |
| Bukti #2 | Thermo `-- °C` **dan** UV Mata Terbuka | Tampilkan sub-langkah yang kurang | Satu foto biasa dianggap bukti |
| Konfirmasi Jurnal | Bukti 3/3 + empat segel | Daftar syarat yang kurang | Arena mengunci saat inspeksi |
| Arena boss | Jurnal + `PHASE5_CP6` sukses | Gagal write → tetap di Panggung, ulang | Boss mulai sebelum save |
| Blitz inti | Jendela inti terbuka, baterai ≥15, LOS | Miss → perisai kembali; dinamo tersedia | Satu jepretan memecahkan dua inti; inti 3 sebelum kabel |
| `FINAL_CHOICE` | Inti 3 pecah, `CP8` sukses | Gagal write → ulang commit | Ending dipicu sebelum state |
| Secret | Lima ID unik valid + validasi cermin | Slot kosong menunjuk bab yang belum | Hitung 5 tanpa ID benar; memberi item gratis |

---

## 3. Arsitektur level, rute aman dan alur adegan

### 3.1 Denah logis (bukan koordinat scene final)

```text
BOUNDARY P4 (tangga batu di bawah Pendopo) --> GERBANG PASAR SETAN (CP1)
  |   jalan tanah berlumpur, bianglala terbalik, tenda robek, cermin melayang
  |   +-- KIOS TUMBAL (kiri): manekin topeng Jawa, EDC/QRIS kuno, Kertas Aturan
  |   +-- KIOS ARSIP BISU (ceruk kanan): manekin menunjuk kursi kosong, laci tersembunyi
  |   +-- DINAMO GERBANG + ALTAR DOA PASAR (gratis, tanpa UV)
  v
GERBANG DALAM (tiga gerbang kenangan; CP2)
  |
  +-- SEKTOR 1: KORIDOR BEKU (beton Rusunawa + kamar mayat RSUD) -- 3 Pintu Cermin
  +-- SEKTOR 2: JEMBATAN GILINGAN BAMBU (Pabrik Gula + bambu Karang Setra) -- dua cabang
  +-- SEKTOR 3: RUANG JAHIT CERMIN (replika bawah tanah Atmaja) -- lorong 'U'
  v
PANGGUNG LELANG UTAMA (CP5): amfiteater, layar proyeksi, kurungan lensa, podium kontrak
  +-- RUANG CERMIN SAKSI (antechamber): Arsip N/5, Cermin Jalan Pulang -> Gerbang
  +-- ARENA BOSS: generator lampu sorot, peti kayu, lentera, dinamo, altar doa
  +-- ZONA FINAL: Z_DAMAR (tengah), Z_EDGE (tepi), Z_PORTAL (cermin)
```

**Kesetaraan jarak:** world scale mengikuti controller Raka. Setiap area punya landmark non-HUD (bianglala terbalik di gerbang; kabut beku dan lampu neon putih di S1; kincir gilingan dan lonceng bambu di S2; mesin jahit dan gunting rumput di S3; layar menyatu dan kurungan lensa di panggung). Tidak mewajibkan kompas HUD. Saat chat dimatikan, babak tetap solvable.

### 3.2 Gerbang dan Kios (Adegan 1)

Raka keluar dari kabut merah di ujung tangga dan berdiri di jalan tanah **Pasar Setan**: langit merah tanpa bintang, bianglala berkarat berputar terbalik, tenda robek, ratusan cermin melayang. Cermin menampilkan wajah penonton (bukan pantulan Raka). **Aturan visual:** wajah memakai atlas wajah sintetis/berlisensi tanpa identitas nyata; jumlah wajah aktif dibatasi (§7.5).

Urutan pembelajaran: (1) HP menyala → siaran tersambung, chat #1 + donasi; (2) pemain menemukan **Dinamo Gerbang** dan **Altar Doa Pasar** sebelum ancaman pertama (jaminan P4-D19 bila masuk dengan baterai/Sanity 0); (3) Kios Tumbal (opsional); (4) Kios Arsip Bisu (opsional); (5) gerbang dalam. Chat yang menyuruh “jangan matikan siarannya” **tidak menghalangi** `F`. Tidak ada ancaman fatal di gerbang; ancaman hanya Sanity dari tatapan cermin.

### 3.3 Tiga sektor distorsi (Adegan 2)

| Sektor | Tipuan | Bukti yang dapat diverifikasi | Akibat jika salah dan recovery |
|---|---|---|---|
| 1 — Koridor Beku | Chat/banner menyebut pintu salah; replika Kuntilanak Merah menempel di belakang | **Thermo**: kiri/kanan `+50°C`, tengah `-22°C`; Selfie menahan replika | Pintu panas: semburan telegraph ≥1 s, −25 Sanity, kembali ke awal koridor; replika tidak instakill |
| 2 — Jembatan Gilingan Bambu | Teriakan “Damar” dari kiri; chat serempak ke kiri | **EVP**: kiri merah bergerigi `Mimic` (Wewe), kanan hijau Morse `Authentic` (Damar); **UV**: papan Rajah Ungu = lantai runtuh | Papan runtuh: jatuh ke anchor, −10 Sanity; jalan benar tetap ada |
| 3 — Ruang Jahit Cermin | Kegelapan total; replika patroli (gunting rumput, langkah berat) | `F` memutus LOS; **AR** membuka permukaan cermin retak menjadi portal cair | Tertangkap: −25 Sanity + knockback + i-frame 3 s; dua kontak dalam 10 s → retry sektor (bukan seluruh babak) |

**Sektor 1 detail [BASELINE UJI]:** koridor 28 m, tiga Pintu Cermin berjarak 3 m. Replika mulai 8 m di belakang, laju 1,3 m/s (< walk 1,7). **Jeda tatap:** menahan replika dalam frame Selfie 1,0 s memberi jeda 4 s (cooldown 6 s), selama jeda boleh pindah ke Thermo. Thermo butuh bidik stabil 2 s per pintu; membaca tiga pintu membutuhkan ≥2 jeda → mengajarkan mekanik. Pintu panas `+50°C` tidak menandakan keaslian obor atau chat; pintu tengah **tidak diacak** (naskah). Papan arah dunia (stensil “TENGAH” pada rangka) tersedia sebagai petunjuk non-UI untuk baterai 0.

**Sektor 2 detail:** jembatan besi panjang ±40 m, cabang kiri dan kanan. EVP menampilkan label ASLI/TIRUAN + bentuk gelombang (bukan hanya warna). Morse Damar dieja `R-A-K-A` (subtitle `[Ketukan Morse: R-A-K-A]`). Papan Rajah Ungu memakai simbol tengkorak P4 (bahaya). **Satu Obor Santet** menghalangi pintu keluar: UV menampilkan lingkar segel utuh (bukan silang), chat menyuruh tidak menyiram; siram dengan Kendi (3 muatan) atau Cawan Air Pasar; **titik isi ulang** di sebelah gerbang. Tidak ada obor umpan wajib; lentera hias non-interaktif tidak dapat disiram.

**Sektor 3 detail:** lorong berbentuk ‘U’, tiga lemari dan dua kolong meja kerja sebagai tempat sembunyi. Stimulus (baseline P4): langkah 4 m, sprint 9 m, jongkok 1,5 m, cahaya layar LOS 12 m; `F` menghilangkan stimulus cahaya. Replika Sri Atmaja 1,4 m/s, Genderuwo 1,8 m/s; keduanya patroli dengan waypoint, `SEARCH` 6 s setelah LOS putus. Cermin Rias Antik Besar di ujung; tidak ada ritual atau boss di sini.

### 3.4 Panggung Lelang (Adegan 3)

Amfiteater melingkar, ribuan layar proyeksi. Di tengah panggung: kurungan bambu bentuk lensa kamera, Damar tergantung dengan kabel urat hitam di pelipis. Di depan kurungan: meja altar (Catatan Asli Mbah Suro), **Server Batu + Kabel Urat Gaib** di bawah kurungan, **Podium Lelang** dengan Kontrak Darah, **Pilar Layar** sumber bisikan EVP. Damar berdialog sesuai naskah; dialog dapat diulang tanpa mengubah state. Pemain menyelesaikan tiga bukti bebas urutan, membuka Jurnal, lalu `[KONFIRMASI IDENTITAS & BUKA RITUAL EKSORSISME]`.

**Bukti #3 [BASELINE]:** rekam di dekat Pilar Layar (radius 3 m, 6 s). Rekaman palsu (Mimic) dari layar pinggir tidak dihitung.

### 3.5 Ruang Cermin Saksi dan Arena

**Ruang Cermin Saksi** tersambung ke panggung. Di dalamnya: Cermin Saksi (menampilkan lima slot, kosong/isi, ikon bab), Cermin Jalan Pulang (ke gerbang), dinamo dan altar doa. Sebelum `P5_ARENA_LOCK`, pemain boleh keluar-masuk antara gerbang ↔ ruang cermin ↔ panggung; setelah CP6 sukses, Cermin Jalan Pulang menutup (jelas diberi telegraph dan peringatan “Arena akan terkunci”). **Peringatan wajib sebelum konfirmasi Jurnal:** panel “Item rahasia: N/5. Kembali ke gerbang?” memberi pilihan.

**Arena:** lingkaran ±36 m; Generator Lampu Sorot (tengah-barat), 4 klaster peti kayu (cover), 3 tiang Lentera Panggung, 2 dinamo, 2 altar doa, 8 titik pemulihan anchor aman. Jalur lari bebas halangan minimal 3 m; tidak ada dead-end bagi replika api. Zona final: `Z_DAMAR` (tengah), `Z_EDGE` (tepi panggung), `Z_PORTAL` (cermin pintu keluar, 14 m dari tengah).

### 3.6 Streaming, LOD, dan batas scene

Prefer satu scene dengan subscene streamed per sektor; sektor yang sudah selesai dibongkar tanpa kehilangan receipt. Batasi shadow-casting dinamis; fog berlapis murah; cermin melayang sebagai instance dengan atlas wajah terbatas; layar proyeksi chat adalah teks statis/sinematik (bukan jutaan elemen UI). Iris Xe wajib 720p Low (§8.0).

---

## 4. Sistem horor dan AI — Sanity, chat, replika, boss, pilihan akhir

### 4.1 Sanity di Pasar Setan

Sanity dilanjutkan dari P4 (tier 70/40/0). **Sumber tekanan P5 adalah “tatapan penonton”**, bukan kabut: wajah di cermin, layar proyeksi, dan chat menekan Sanity hanya saat dilihat/dibaca di viewport. Nilai awal babak = nilai save (tidak direset). Gunakan game simulation time; pause tidak menguras.

| Tier | Kondisi | Distorsi yang diizinkan | Informasi dasar yang tetap andal |
|---|---|---|---|
| CLEAR | 71–100 | Cermin berbisik pelan; chat biasa | Semua properti dunia, UV, EVP authored, Thermo, AR |
| UNSETTLED | 41–70 | Wajah cermin menoleh, komentar berulang, tepi layar berdenyut | Rajah/benang/grafik tetap tegas |
| PANIC | 1–40 | Suara Damar/Ibu palsu, jumlah chat berlipat, radius deteksi replika naik | Bentuk simbol dunia (lingkar/silang/tengkorak, garis kontinu/putus) |
| HUNT | 0 | Chat/banner sangat kacau, langkah dan seruan lebih sering; AI replika hunt | Pemulihan, interaksi, dinamo, save selalu tersedia; tidak ada kontrol dibajak, teleport paksa, atau teka-teki tak terbaca |

**Baseline tuning [BASELINE UJI]:** `sanity.gazeDrain` gerbang/galeri cermin 0,06/s saat cermin dalam fokus (lantai ambien 50%); sektor 0,05/s (lantai 45%); panggung/arena 0,04/s (lantai 45%); zona perlindungan 0/s. **Lantai ambien** mengikuti pola P4: waktu saja tidak mengirim pemain ke 0; event kontak, replika, salah pintu/papan, dan serangan boss masih bisa. Pemulihan:

- **Penutup pemulihan arena:** kanopi rendah ber-collider dengan dua sisi masuk/keluar; pemain masuk dengan jongkok. Navigasi boss tinggi tidak melewati kanopi dan serangan memeriksa LOS; tidak ada flag kebal. Anchor CP7 berada di luar kanopi agar reload tidak menanam kapsul berdiri di atap.
- **Altar Doa Pasar** (gratis, fisik): tahan `E` 10 s → +15 poin, cooldown 30 s; ada di gerbang, dua kali per sektor, Ruang Cermin Saksi, dan arena (2).
- **Kemenyan `H`** (opsional): ke 100% setelah pemakaian selesai (Rancangan.MD), cooldown 12 s; mereda distorsi chat 20 s.
- **Layar mati (`F`)** menghentikan tatapan chat/layar tetapi **tidak** memulihkan Sanity.
- Sanity 0 tanpa kemenyan dan baterai 0 tetap solvable: altar doa dan dinamo dijangkau tanpa UV.

Penalti Sanity: kontak replika −25 (iframe 3 s), pintu panas S1 −25, papan runtuh −10, bayangan layar boss −15, serangan fase 3 tidak langsung memotong Sanity tetapi mengincar cover. **Sanity bukan indikator penyakit nyata**; efek strobo/blur/camera shake dapat diturunkan di Settings tanpa menghapus clue (aksesibilitas §8.4).

### 4.2 Lapisan kebenaran dan chat satu juta penonton

Pesan berasal dari **event authored deterministik**, bukan LLM/koneksi nyata. `sourceTag ∈ {HUMAN_TRAPPED, HYPNOTIZED, ENTITY, ANTAGONIST, SYSTEM}` hanya metadata internal; **tidak pernah menjadi label “BENAR” bagi pemain**.

| Kelompok | Contoh perilaku | Aturan fair-horror |
|---|---|---|
| `HUMAN_TRAPPED` | “tombol keluarnya ilang!” (memohon) | Manusiawi dan dapat benar; tidak menjadi satu-satunya petunjuk |
| `HYPNOTIZED` | “Jangan matikan siarannya” berulang | Tekanan emosional; **tidak menghalangi `F`** |
| `ENTITY` | Meniru username manusia, menyuruh kiri/kanan/siram | Boleh benar atau salah; verifikasi dengan dunia |
| `ANTAGONIST` | Donasi `Penonton_Terakhir`, kontrak Rp500.000.000 | Narasi; bukan reward |
| `SYSTEM` (palsu) | “[PERINGATAN MATABATIN] …” | Terlihat sebagai konten chat; tidak memanggil dispatcher/binding/checkpoint |

Prinsip tambahan P5: **layar mati selama 3 detik menghentikan chat palsu pada sumbernya** (aturan global; puzzle Pita memakai aturan yang sama). Maksimum 1 pesan/1,2 s saat chase; viewport chat tidak menutup reticle, tanda bahaya, atau prompt `E`. Riwayat chat memakai ring buffer; overlay 1.000.000 adalah teks statis. Tidak ada objective wajib yang hanya ada di chat. Wajah penonton bukan identitas nyata.

### 4.3 Replika Memori — konstruksi, bukan boss

| Replika | Lokasi | Perilaku | Guardrail |
|---|---|---|---|
| Kuntilanak Merah Replika | Sektor 1 | Menempel di punggung; melaju 1,3 m/s; lunge dengan telegraph 1,2 s | Selfie 1,0 s memberi jeda 4 s; tidak instakill; larut saat `P5_S1_CLEAR` |
| Wewe Gombel (suara) | Sektor 2 | Hanya `Mimic` EVP; tidak ada tubuh | Tidak dapat melukai; hanya menyesatkan |
| Nyonya Sri Atmaja Replika | Sektor 3 | Patroli 1,4 m/s, gunting rumput `SRAAAK… KLING…` | Telegraph suara; LOS cahaya 12 m |
| Genderuwo Replika | Sektor 3 | Patroli berat 1,8 m/s | Bergantian dengan Sri; tidak dalam satu lorong sempit tanpa bypass |
| Banaspati Replika (gelombang) | Fase 3 | Sapuan api dari tepi, 3 gelombang | Telegraph 2,0 s, dapat berlindung di peti kayu |

**Kontrak umum:** ID `P5_REPLICA_*`; tidak berubah menjadi encounter permanen; tidak membuka/menutup segel; tidak menyimpan state persisten selain sektor selesai. State machine ringkas: `DORMANT → PATROL → INVESTIGATE → CHASE → SEARCH(6 s) → PATROL`; **dua kontak dalam 10 detik** (setelah i-frame 3 s) memicu `CAUGHT_REPLICA` dan retry **sektor** dari anchor (bukan seluruh babak); satu kontak: −25 Sanity + knockback. Patroli udara tidak memerlukan NavMesh terbang penuh (waypoint + swoop anchor). Tidak spawn di anchor pemulihan atau di atas tempat berlindung saat interaksi wajib.

### 4.4 `Penonton_Terakhir` — “Sang Dalang Layar”

**Wujud:** humanoid 4 m berjubah kain layar bioskop terbakar, puluhan tangan pucat, kepala tumpukan lensa kamera tua dan cermin retak berkedip merah. **Peran:** dalang kebohongan dan sumber daya jaringan; **bukan HP bar** dan bukan unit melee permanen. Ia hanya rentan pada tiga **jendela inti** yang dibuka mekanik. Di luar jendela, `CANT_BE_DAMAGE`; tembakan Blitz di luar jendela tidak mengurangi baterai kecuali kebijakan input global meminta.

**Tiga inti:**

| Inti | Titik rentan | Syarat membuka | Pecah oleh |
|---|---|---|---|
| 1 | Kepala lensa wujud asli | UV memilih sosok berbenang kontinu; wujud asli di zona Generator; sakelar generator membakar Kabut Sengkala | Blitz (1) |
| 2 | Rahang lensa di belakang leher | Gelap total; Thermo `-30°C`; Selfie melihat rahang terbuka; Flare/Lentera menghancurkan perisai kabut | Blitz via Selfie (2) |
| 3 | Dada terbuka | AR memperlihatkan tiga Kabel Roh; semuanya diputus; kurungan terbuka | Blitz (3) |

**Dialog antagonis:** suara gema ribuan manusia; direkam sebagai layered VO manusia (bukan TTS), subtitle speaker jelas. Dialog tidak menjadi satu-satunya petunjuk objektif.

### 4.5 Boss tiga fase — kontrak mekanik dan retry

**Fase 1 — Distorsi Ilusi [BASELINE UJI]:** entitas membelah menjadi **4 sosok** (1 asli + 3 bayangan layar). Bayangan layar: kontak −15 Sanity lalu larut, respawn 6 s. Sosok asli memiliki **benang kontinu berdenyut ke kurungan Damar** (UV); bayangan memiliki benang putus-putus. Lantai ber-rajah ungu (UV) runtuh dengan telegraph ≥1,0 s. Pemain memancing sosok asli (laju 2,2 m/s, stimulus langkah/cahaya) ke **Zona Sorot Generator** (radius 6 m; indikator lantai diegetik berupa pantulan lampu panggung, bukan label HUD), menyalakan sakelar (`E`) saat sosok di zona → Kabut Sengkala terbakar → **jendela 5 detik**; Blitz tepat pada kepala lensa. Miss/timeout: perisai kembali, generator cooldown 12 s, sosok asli tetap dapat dipancing lagi; benang UV tidak berpindah. Bayangan tidak dapat memakan cooldown generator.

**Fase 2 — Teror Punggung & Chat Palsu:** entitas menghancurkan lampu sorot (generator offline); arena gelap total. Entitas tak terlihat dari depan, mengintai dari punggung (mendekat 6 s setelah telegraph napas/dingin). Chat palsu menyuruh menoleh salah arah (tidak mengambil alih input). Pemain boleh membakar Kemenyan (`H`) bila ada, atau memakai altar doa. Mengangkat HP dalam mode Selfie (`X`) memperlihatkan sosok dengan rahang terbuka; Thermo (`T`) membaca `-30°C` pada jarak ≤2,2 m di belakang. Telegraph lunge 1,2 s dimulai saat rahang terbuka. Pemain menjatuhkan **Flare (`G`)** atau, bila stok 0, **Lentera Panggung** (lentera dibawa dari tiang; reusable, cooldown 15 s di tiang) — radius cahaya 4 m selama 10 s menghancurkan perisai kabut → **jendela 5 detik**, Blitz dari mode Selfie. Miss: perisai kembali, entitas mengulang pola; pemain tetap bebas bergerak ke dinamo/altar.

**Fase 3 — Overload Sinyal:** entitas memanggil **tiga gelombang Banaspati Replika** (telegraph 2,0 s: dengung + cahaya merah dari tepi; sapuan 4 s; jeda ≥12 s). Pemain mematikan layar (`F`), merunduk di balik klaster peti kayu; peti tidak memberi invulnerability global (collider dan LOS menentukan). Di sela gelombang pemain menyalakan AR (`Q`), melihat **tiga Kabel Roh**, dan memutus masing-masing dengan tahan `E` 2 s (dapat dibatalkan); receipt per kabel (`P5_CABLE_n_CUT`). Setelah tiga kabel putus, kurungan Damar terbuka, entitas menjerit, **Inti 3** terbuka di dada → **jendela 5 detik**, Blitz → entitas berlutut → `FINAL_CHOICE`. Gelombang berhenti setelah kabel ketiga putus (tidak ada gelombang keempat).

**Komit progres inti:** Blitz valid pada tiap inti melakukan **satu transaksi atomik** (baterai −15 bila belum dikurangi kontroler global, `core_committed_ids`, CP7 rolling, anchor aman). Setelah komit, kematian tidak mengulang inti itu; fase berikutnya dimulai dari awal fase (bukan dari awal arena). Jika write gagal: UI **tidak** menampilkan “Inti pecah”, transaksi dirollback, jendela dapat diulang.

### 4.6 Stealth, perlindungan, dan pemulihan tanpa eksploitasi

Tempat sembunyi: lemari dan kolong (S3), peti kayu dan celah panggung (Fase 3), dan cekungan altar doa. `F` memutus stimulus cahaya; napas/detak jantung meningkat saat Sanity rendah tetapi tidak menembus cover. **Safe spot bergantung collider dan LOS**, bukan flag invulnerable. Setiap tempat berlindung memiliki dua jalur keluar atau waktu AI move-away. Entitas tidak menyerang dari dalam transisi scene/cutscene. Dinamo dapat digunakan saat layar mati; altar doa dapat digunakan pada Sanity 0.

### 4.7 Sumber daya: baterai, air, cahaya

- **Baterai:** tiga Blitz inti = 45 poin + biaya optik; **tidak ada recharge otomatis pada retry**. Dinamo fisik (+25 poin / 8 s) di gerbang, Ruang Cermin Saksi, dan arena (2). Kapasitor Ritual 5% dipisah (§4.8).
- **Air:** Kendi kapasitas 3 (instance P4), isi ulang gratis di titik sektor 2 (tahan `E` 4 s); `Cawan Air Pasar` fallback. Air hanya dipakai di S2; tidak ada air di boss.
- **Cahaya:** Flare konsumsi stok; Lentera Panggung reusable (3 tiang). `G` tidak mengonsumsi bila Lentera dipakai.

### 4.8 `FINAL_CHOICE`: state machine, input, dan kunci ending

```text
CORE_3_BROKEN (commit CP7) -> DAMAR_FREED -> ENTITY_KNEELING
  -> FINAL_CHOICE_ENTER (commit CP8; capacitor=5%; damar=SLUMPED; endingId=NONE)
       Zones: Z_DAMAR (tengah), Z_EDGE (tepi), Z_PORTAL (cermin)
  Actions (hanya di state ini):
    BAD:    kamera belakang pada Damar -> dwell 2.0 s -> label TUMBAL -> LMB
    NORMAL: di Z_EDGE, tahan F 5.0 s (progres terlihat, lepas = batal)
    TRUE:   papah Damar (E) ke Z_PORTAL -> X (Selfie) -> Q (AR) 1.5 s -> R (UV) 1.5 s
            -> sync 100% -> LMB pada inti di layar Selfie
    SECRET: [lima ID unik valid + validasi] Z_PORTAL, tahan E 3.0 s pada Cermin Saksi
            -> mode Arsip -> LMB pada cermin berbidikan emas
  -> TryLockEnding(id) ATOMIK (compare-and-set NONE -> id)
  -> ENDING_CUTSCENE(id) -> P5_COMPLETE (CP9) -> CREDITS
```

**Aturan:**
1. **Tidak ada timer fatal** di `FINAL_CHOICE`; entitas hanya berbisik dan menekan UI chat. Cutscene True/Secret boleh menampilkan ancaman sinematik, bukan fail state.
2. **Bad** harus dua langkah sadar: dwell 2 s + label `TUMBAL` terlihat ≥0,5 s sebelum LMB diterima; menurunkan HP membatalkan. Jurnal menampilkan petunjuk eksplisit (“pilihan akhir tidak dapat dibatalkan setelah tembakan”). Bad valid pada semua zona selama Damar terlihat, **kecuali** pada mode Arsip Secret (target Damar tidak valid).
3. **Normal** tidak memakai LMB dan tidak mengonsumsi kapasitor. Arahan tambahan desainer: tahan tombol layar **5 detik wajib**; tidak memakai alternatif tekan dua kali.
4. **True:** urutan sinkronisasi wajib `X → Q → R`; urutan salah mereset gauge tanpa penalti. Pengecualian AR/UV hanya berlaku di `SYNC_CALIBRATION`. Pada layar Selfie tampak Raka dan Damar, serta inti entitas di belakang; LMB pada inti menghabiskan Kapasitor Ritual.
5. **Secret:** tahan `E` 3 s memutar lima kesaksian (progres terlihat, dapat dibatalkan). Mode Arsip: hanya cermin berbidikan emas valid; LMB merekam nama korban dari data arsip; mengunci Secret satu kali.
6. **Validasi arsip** (`P5_ARCHIVE_VALIDATED`) dapat dilakukan di Ruang Cermin Saksi atau cermin yang sama di `FINAL_CHOICE`; tidak memilih ending.
7. Kapasitor Ritual: 5%, **satu aksi LMB**; setelah aksi apa pun yang memakai LMB diterima, kapasitor 0 dan ending terkunci.
8. Satu enum global `EndingId {NONE=0,BAD=1,NORMAL=2,TRUE=3,SECRET=4}`; field save `ending` tidak digandakan. Nilai numerik dipertahankan dari enum existing; transisi hanya `NONE → id`. Input ending ditolak jika `endingId != NONE`.

### 4.9 Failure model dan telegraph

| Kondisi | Telegraphed dan akibat | Pemulihan |
|---|---|---|
| Pintu panas S1 | Semburan 1,0 s, −25 Sanity | Awal koridor; sektor tidak reset |
| Papan runtuh S2 | Rajah Ungu + derit, jatuh, −10 Sanity | Anchor jembatan terakhir |
| Kontak replika | −25 Sanity, knockback, i-frame 3 s | Kontak ganda 10 s → retry sektor dari CP |
| Bayangan layar boss | −15 Sanity, larut | Inti tidak hilang |
| Miss Blitz inti | Perisai kembali, generator/lentera cooldown | Dinamo; retry tanpa kehilangan inti sebelumnya |
| Serangan fase 3 | Gelombang 2,0 s telegraph | Cover; kabel yang sudah putus tetap |
| Sanity 0 | HUNT + UI kacau, **tidak otomatis fatal** | Altar doa, kemenyan bila ada |
| Salah input ending | Gauge reset / tidak diterima | Coba lagi tanpa penalti |
| Gagal save `FINAL_CHOICE`/ending | Pesan error; ending belum terkunci | Retry commit |

---

## 5. Inventori, Jurnal Klenik, rahasia dan ekonomi

### 5.1 Master data Phase 5

| Tipe | Nama / ID konseptual | Asal | Hasil |
|---|---|---|---|
| Dokumen #1 | Kertas Aturan Pasar Setan · `P5_DOC_RULES` | Kios Tumbal | Aturan pasar; bacaan opsional |
| Dokumen #2 | Catatan Asli / Manuskrip Rahasia Mbah Suro · `P5_DOC_SURO_MANUSCRIPT` | Meja altar Panggung | Asal `Penonton_Terakhir`; opsional |
| Bukti #1 | Server Batu + Kabel Urat Gaib · `P5_EVIDENCE_SERVER_AR` | Bawah kurungan Damar | AR stabil; evidence 1/3 |
| Bukti #2 | Kontrak Darah (`-- °C` + Rajah Mata Terbuka) · `P5_EVIDENCE_CONTRACT` | Podium lelang | Thermo **dan** UV; evidence 2/3 |
| Bukti #3 | EVP Gabungan Penonton · `P5_EVIDENCE_EVP_CROWD` | Pilar Layar | EVP `Authentic`; evidence 3/3 |
| Prasyarat | Empat segel lokasi | Save P1–P4 | Syarat cerita; **bukan** item baru |
| Rahasia | Pita Rekaman Pertama · `SECRET_P5_PITA_PERTAMA` | Kios Arsip Bisu | Arsip Saksi opsional |
| Consumable | Powerbank Besar ×3 | Kios Tumbal, opsional | +80 poin/unit |
| Consumable | Flare Merah Darurat ×4 | Kios Tumbal, opsional | Fase 2; stok awal dari carry-over P1–P4 |
| Consumable | Kemenyan Bakar ×4 | Kios Tumbal, opsional | Pulih ke 100 |
| Layanan | Isi Daya (full charge) | Kios Tumbal, opsional | Baterai ke 100, receipt sekali |
| Alat dunia | Lentera Panggung ×3 | Tiang arena | Pengganti Flare; reusable |
| Alat dunia | Cawan Air Pasar | Sektor 2 | Pengganti Kendi bila tidak ada |
| State-lokal | Kapasitor Ritual (5%) | `FINAL_CHOICE` | Satu aksi LMB; bukan baterai persisten |

**Receipt terpisah:** `evidence_ids` untuk scan/foto/rekaman; `document_ids` untuk dokumen; `inventory` untuk benda dibawa; `secret_items` untuk arsip; `completed_event_ids` untuk jalan cerita dan sektor. Satu ID bukti tidak bertambah karena tombol dua kali atau reload.

### 5.2 Jurnal Klenik dan filter informasi

Tab P3/P4 dipertahankan: **Bukti**, **Dokumen**, **Arsip Saksi**, **Petunjuk Objektif**. `Bukti 0/3…3/3` dihitung dari tepat tiga ID P5. Tombol `[KONFIRMASI IDENTITAS & BUKA RITUAL EKSORSISME]` hanya pada 3/3 dan empat segel valid; mencatat receipt sekali. Teks deduksi [USUL BARU]: `[ENTITAS TERIDENTIFIKASI: PENONTON_TERAKHIR — SANG DALANG LAYAR]`; `[METODE: UV pilih wujud asli, lampu panggung, Blitz; Selfie + Thermo, cahaya, Blitz; AR putuskan tiga kabel, Blitz]`. Jurnal **tidak menyembunyikan** bahwa boss memakai tiga Blitz inti.

Tab tambahan **Status Arena**: `Inti 1/2/3: utuh|pecah`, `Kabel: 0/3`. Tab **Pilihan Akhir** hanya muncul pada `FINAL_CHOICE` dan menampilkan petunjuk objektif tiga aksi dasar (Secret hanya bila syarat terpenuhi). Pada Sanity rendah dekorasi chat boleh glitch, tetapi **fakta committed tidak dihapus**.

### 5.3 Arsip Saksi dan Pita Rekaman Pertama

**[KANON]** `SECRET_P5_PITA_PERTAMA` berada di **Kios Arsip Bisu**, ceruk kanan gerbang sebelum sektor distorsi pertama; dapat diperiksa gratis. Petunjuk: manekin menunjuk kursi penonton kosong; EVP menangkap “Saksi tidak membeli korban”; tanda mata tertutup terlihat via AR. Langkah: (1) layar mati 3 detik di depan kursi → chat palsu berhenti; (2) AR membuka laci tersembunyi; (3) rekam satu kalimat EVP `Authentic`; (4) ambil pita. Kalimat tiruan di cermin (Mimic) berbunyi menyimpang dan tidak valid.

**Isi kesaksian:** pita merekam pertunjukan pertama Pasar Setan dan menyatakan bahwa nama korban yang dikembalikan melalui kesaksian bebas dapat membatalkan hak kepemilikan entitas.

**Pickup:** `TryCommit` atomik tambah ke `secret_items`, simpan **sebelum** popup sukses, sembunyikan prop **sesudah** committed. Jika commit gagal, prop tetap ada. Jurnal menampilkan `Arsip Babak 5: 0/1 atau 1/1` dan `Arsip Saksi Global: N/5` dari **jumlah ID unik aktual**; nomor babak bukan jumlah item. Melewatkan item babak terdahulu menutup Secret pada campaign itu (chapter select bukan syarat MVP). Tidak ada saweran yang menentukan akses.

**Cermin Saksi:** menampilkan lima slot, ikon bab, dan slot kosong **menyebut bab yang belum** (bukan lokasi pasti). Validasi dapat diulang setelah reload. Pembacaan catatan dicatat terpisah dan tidak menjadi syarat. **Nama korban pada rekaman Secret** bersumber dari data arsip kanon; nama baru [PERLU KEPUTUSAN] (P5-D07).

### 5.4 Kios Tumbal, carry-over dan dana terikat

Naskah menyebut paket: Full Charge + Powerbank ×3, Flare ×4, Kemenyan ×4. **Harga tidak ditetapkan naskah**, sehingga berikut hanya **[BASELINE UJI]**: Powerbank Rp750.000 (3), Kemenyan Rp700.000 (4; Rp175.000/unit baseline P3), Flare Rp600.000 (4; Rp150.000/unit [PERLU AUDIT] katalog P3), Isi Daya Rp100.000. Total bila semua dibeli **Rp2.150.000** (baseline). Kios hanya memakai `wallet_legitimate`; saldo 0 → semua tombol nonaktif **tanpa memblokir** gerbang dalam, bukti, boss, atau ending. “Uang Saweran Penontonmu” pada naskah dibaca sebagai saldo sah, bukan `wallet_bound`.

**Penonton_Terakhir** memberi **Rp66.666.666** sebagai dana terikat; tidak menambah saldo belanja. Overlay 1.000.000 viewer, kontrak Rp500.000.000, saldo bank Rp500.000.000 (Bad), dan donasi Rp125.000.000 (True/Secret) adalah **state naratif**; tidak menambah/menghapus saldo dan tidak membuat transaksi fiktif untuk rekonsiliasi.

---

## 6. CampaignState, checkpoint dan fault tolerance

### 6.1 Model data tunggal dan kompatibilitas

Lanjutkan `CampaignData` dengan `Phase5Data` berisi receipt event tunggal, clock, recovery, ledger dan safe_anchor. Sektor/inti/kabel diturunkan dari receipt, air tetap `phase4.water_units`, ending tetap field global `ending: EndingId`, dan kapasitor diturunkan dari receipt FINAL_CHOICE + ending yang sukses dikunci. Tidak ada owner air/ending kedua. Jangan menggandakan field global `inventory`, `battery`, `sanity`, `wallet_legitimate`, `wallet_bound`, `secret_items`, `location_seals`. Snapshot persisten menyimpan **receipt bukti, dokumen, sektor selesai, inti pecah, kabel putus, ending, chapter exit**. Runtime-only (tidak persisten): transform replika/boss, timer paparan, index chat, bayangan layar, progres tahan-input ending, mode kamera Arsip. **Schema:** tambah `Phase5Data` melalui migrasi normal repo; jangan menetapkan nomor schema sebelum audit (P5-R01).

### 6.2 Cuplikan field loader aktual (bukan file save lengkap)

Hasil audit menetapkan migrasi **5→6** melalui `CampaignState` existing. Contoh berikut memperlihatkan owner field sebenarnya; field campaign dan data Phase 1–4 lainnya sengaja tidak dicantumkan, sehingga cuplikan ini tidak dipakai sebagai fixture loader. Nilai bukan resource awal kanon.

```json
{
  "schema_version": 6,
  "chapter_id": 5,
  "checkpoint_id": "PHASE5_CP7",
  "ending": 0,
  "archive_prepared": false,
  "battery": 52,
  "sanity": 48,
  "evidence_ids": [
    "P5_EVIDENCE_SERVER_AR",
    "P5_EVIDENCE_CONTRACT",
    "P5_EVIDENCE_EVP_CROWD"
  ],
  "secret_items": [
    "SECRET_P4_CERMIN_SAKSI"
  ],
  "location_seals": [
    "SEAL_RUSUNAWA_GRIYA_ASRI",
    "SEAL_RSUD_LAMA_SIDO_WARAS",
    "SEAL_PABRIK_GULA_SINDANG_LAUT",
    "SEAL_DESA_MATI_KARANG_SETRA"
  ],
  "phase4": {
    "water_units": 2
  },
  "phase5": {
    "completed_event_ids": [
      "P5_ARRIVE_GATE",
      "P5_ENTER_SECTORS",
      "P5_S1_CLEAR",
      "P5_BRIDGE_TORCH_OFF",
      "P5_S2_CLEAR",
      "P5_S3_CLEAR",
      "P5_CONTRACT_THERMO_OK",
      "P5_CONTRACT_UV_OK",
      "P5_JOURNAL_CONFIRMED",
      "P5_ARENA_LOCK",
      "P5_CORE_1_BROKEN"
    ],
    "recovery_last_uses": [],
    "money_ledger": [],
    "elapsed_gameplay_seconds": 2100,
    "incense_ready_at": 0,
    "safe_anchor": "ARENA_WEST"
  }
}
```

Dokumen dan item tetap pada `documents` / `inventory` global. Inti, kabel, sektor dan pembelian menggunakan receipt `completed_event_ids`; tidak ada field `boss_cores`, `cables_cut`, `water_volume`, atau ending lokal. Kapasitor dihitung dari FINAL_CHOICE + satu `ending` global. Persyaratan prerekuisit tetap divalidasi oleh loader.

### 6.3 Checkpoint Phase5

| ID | Waktu/save trigger | Anchor/keadaan saat restore |
|---|---|---|
| `PHASE5_CP1` | Tiba gerbang, kontrol kembali | Baterai/Sanity/saldo/inventori carry-over aktual; chat/donasi pembuka idempoten |
| `PHASE5_CP2` | Masuk gerbang dalam sebelum Sektor 1 | Setelah Kios/Pita opsional; stok dan receipt aktual |
| `PHASE5_CP3` | `P5_S1_CLEAR` | Awal Sektor 2; replika S1 tidak ada |
| `PHASE5_CP4` | `P5_S2_CLEAR` | Awal Sektor 3; volume Kendi aktual |
| `PHASE5_CP5` | `P5_S3_CLEAR` / tiba Panggung | Apron panggung; Damar dan podium; tiga bukti belum |
| `PHASE5_CP6` | Jurnal terkonfirmasi, arena terkunci | Snapshot pra-boss; Cermin Jalan Pulang menutup setelah commit sukses |
| `PHASE5_CP7` | **Rolling**: setiap inti pecah atau kabel putus | Snapshot terbaru di anchor aman; semua inti/kabel sebelumnya tetap |
| `PHASE5_CP8` | `FINAL_CHOICE_ENTER` | Damar `SLUMPED`; kapasitor 5%; `ending_id=NONE` |
| `PHASE5_CP9` | `P5_COMPLETE` | Ending tercatat; credits; aman dari duplikasi |

`CP7` adalah **slot bergerak** (bukan CP7a/b/c). Jangan menurunkan checkpoint ke CP6 setelah inti pertama pecah. Semua checkpoint menghindari posisi tepat di jalur sapuan api, di dalam collider dekorasi, atau di zona final sebelum `CP8`.

### 6.4 Atomic transactions dan rollback penuh

```text
TryAddEvidence(P5_EVIDENCE_*) -> mutate unique set -> TryCommitCampaign()
  on FAIL: rollback ALL, interactable tetap tersedia

TryContractEvidence():
  if ThermoOk && UvOk && !Has(P5_EVIDENCE_CONTRACT): TryAddEvidence(...)

TryCollectPita():
  if ChairSilence && ArDrawer && EvpTrue:
    TryCommitCampaign(addSecretOnce=SECRET_P5_PITA_PERTAMA)
    if SUCCESS: hide prop & show Global N/5

TryStartArena():
  if JournalConfirmed && FourSeals && TryCommitCampaign(CP6):
    StartBossRuntime()   // tidak pernah sebelum disk commit

TryBreakCore(n):
  if WindowOpen(n) && ValidBlitz && !Committed(n):
    TryCommitCampaign(CP7, addCore=n, batteryAfterBlitz=actual, sanity=actual, anchor=safe)
    if FAIL: rollback & keep window retryable

TryCutCable(k):
  if ArActive && HoldComplete(2s) && !Cut(k):
    TryCommitCampaign(CP7, addCable=k)

TryEnterFinalChoice():
  if CoreCommitted(3) && TryCommitCampaign(CP8, ritualCapacitor=5, endingId=NONE):
    EnterFinalChoice()

TryLockEnding(id):
  if state==FINAL_CHOICE && endingId==NONE && ActionValid(id):
    TryCommitCampaign(setEndingOnce=id)   // compare-and-set
    if SUCCESS: PlayCutscene(id) once
    if FAIL: input tetap valid; tidak ada cutscene

TryComplete():
  if endingLocked && cutsceneDone: TryCommitCampaign(CP9, addEvent=P5_COMPLETE, endings_seen+=id)
```

**Air dan miss:** konsumsi air tepat sekali pada aksi siram (S2); tidak ada air di boss. **Baterai:** Blitz sektor/inti sudah dikurangi 15 oleh controller global; transaksi inti **menyerap** perubahan itu, bukan mengurangi lagi (pola P4 §6.6). `UseKemenyan`/`UseFlare`/`UseGaram` memanggil inventory owner global; event P5 tidak menambah/menghapus instance kedua. Idempotency key `attemptId:phase:actionSequence` bila arsitektur actual membutuhkan. **Tidak ada write barrier global** selama boss; barrier ringkas hanya saat cutscene ending sampai `CP9`.

**Crash mid-cutscene:** jika `ending_id != NONE` tetapi `CP9` belum ditulis, Continue memulai cutscene ending itu dari awal (idempoten), **bukan** kembali ke `FINAL_CHOICE`. Replay pilihan lain [PERLU KEPUTUSAN] P5-D12.

### 6.5 Recoverability setelah crash dan handoff dari P4

`PHASE5_CP1` hanya dibuat setelah handoff valid dan kontrol kembali; gagal tulis tidak memuat scene setengah jadi. Jika save berhenti di boundary P4 (`CP8` P4) sebelum P5 diimplementasikan, Continue **menuju P5** setelah router dihubungkan; save lama tidak rusak. Nilai baterai/Sanity dari P4 dipertahankan; gerbang menyediakan dinamo dan altar doa tanpa UV. Bila P4 berakhir 0%/0%, pemain tidak softlock pada frame pertama.

### 6.6 Orakel transaksi: satu keberhasilan tidak menghasilkan dua sumber kebenaran

1. **Core:** inti pecah = satu transaksi; tampilan “Inti pecah” hanya setelah commit.
2. **Kabel:** tiap kabel punya receipt sendiri; membatalkan tahan `E` tidak mengubah state.
3. **Ending:** `ending_id` satu field; tidak ada boolean `badEnding`, `trueEnding` paralel.
4. **Secret:** Pita dan validasi arsip adalah dua receipt berbeda; validasi tidak memberi item.
5. **Kios:** receipt `P5_SHOP_*` mencegah pembelian ganda saat force-quit.
6. **Kapasitor:** `ritual_capacitor_spent` hanya boleh `false → true` sekali.

---

## 7. Visual, VO, audio, atmosfer dan asset provenance

### 7.1 Shot list naratif / blocking sinematik

| Bagian | Gambar/suara utama | Arah emosi dan informasi gameplay |
|---|---|---|
| Turun tangga | Kabut merah, gravitasi bergeser, cut ke jalan lumpur | Transisi tanpa Kos; tanda lintas dunia |
| Gerbang | Bianglala terbalik, tenda robek, cermin melayang dengan wajah penonton | Skala satu juta penonton; wajah tanpa identitas nyata |
| Kios Tumbal | Manekin topeng Jawa, EDC/QRIS kuno menyala merah | Komodifikasi; opsional |
| Kios Arsip Bisu | Manekin menunjuk kursi kosong, laci tersembunyi | Kesaksian tanpa harga |
| Koridor Beku | Beton Rusunawa + kamar mayat RSUD; uap napas; Kuntilanak Merah di punggung | Kenangan; Selfie + Thermo |
| Jembatan | Besi pabrik, bambu Karang Setra, jurang kabut; teriak Damar vs ketukan Morse | Dua suara; EVP + UV |
| Ruang Jahit | Gelap total, gunting rumput, mesin jahit, cermin rias retak | Stealth; AR + portal |
| Panggung | Amfiteater, ribuan layar chat, kurungan lensa, Damar | Reuni; bukti |
| Boss reveal | Layar menyatu, humanoid 4 m turun | Skala dan identitas |
| Fase 1 | Empat sosok, kabut sengkala, lampu sorot membakar kabut | Verifikasi UV |
| Fase 2 | Gelap total, napas dingin, rahang lensa di cermin Selfie | Teror punggung |
| Fase 3 | Gelombang api, peti kayu, kabel roh via AR | Kerja tangan dan cover |
| Damar bebas | Kurungan terbuka, Damar ambruk | Emosi |
| Pilihan akhir | Dua pilihan terbaca lewat dunia: kamera, tepi, cermin | Tanpa menu |
| Empat ending | Bad (kos studio), Normal (halaman Atmaja, pantulan), True (subuh 05:30), Secret (kursi kosong) | Resolusi berbeda nada |

### 7.2 Identitas artistik Pasar Setan

Gunakan referensi **pasar malam tradisional Jawa yang terdistorsi**: tenda terpal robek, lampu bohlam, bianglala berkarat, gamelan sumbang, topeng kayu, layar bioskop terbakar. Amfiteater panggung lelang dengan ribuan layar proyeksi. Cermin adalah motif sentral: tiap sektor menggabungkan arsitektur babak lalu sebagai **replika memori** (warna lebih pudar, tepi “glitch”). Palet: merah gelap, ungu UV, putih beku (S1), hijau glitch (portal AR). **Keterbacaan UV** (lingkar/silang/tengkorak/benang) diutamakan di atas bloom; jangan memenuhi frame dengan merah.

**Target mutu produksi [BASELINE]:** minimal **100 identitas model baru** dari sumber berlisensi, ditempatkan dan sesuai fungsi cerita, di luar tekstur/audio/shader dan duplikasi. Seluruh lingkungan Pasar Setan baru; Raka, HP, UI dan mixer bersama adalah kontinuitas sistem campaign dan **tidak dihitung pada 100**. Damar dan seluruh kit replika memakai sumber model baru; tidak menyalin lingkungan kos/Atmaja lama ke epilog. Cantumkan `Audit/phase5-asset-sources.json` dan `Audit/phase5-asset-credits.md` (URL, penulis, lisensi, SHA-256, penempatan, izin komersial, perubahan impor). Jangan mengunduh aset berlisensi tidak jelas. Retarget animasi dari sumber legal dengan skeleton kompatibel. Tidak mengklaim photorealism setara DreadOut 2 hanya karena banyak prop.

### 7.3 Rancangan audio dan prioritas

| Bus/cue | Material audio | Prioritas |
|---|---|---|
| `AMB_GATE` | Gamelan sumbang jauh, tawa pedagang gaib, bianglala berderit | Ambient bawah dialog |
| `AMB_FROZEN` | Dengung neon, napas beku, tetes air kamar mayat | Meningkat dekat pintu |
| `AMB_BRIDGE` | Besi berderit, bambu gesek, angin jurang | Ambient |
| `AMB_SEWING` | Mesin jahit jauh, jam dinding, nyaris sunyi | Dinamika sunyi |
| `REPLICA_STEP` | Langkah Sri/Genderuwo, gunting `SRAAAK… KLING…` | Terlokalisasi 3D |
| `EVP_STAGE` | Bisikan jutaan menyatu menjadi satu suara | Ducking overlay lain saat target aktif |
| `PT_VOICE` | Suara gema ribuan manusia (layered VO) | Voice cue besar |
| `FIRE_WAVE` | Dengung merah, sapuan api, derit peti | Telegraph 2,0 s |
| `HUNT_TELEGRAPH` | Detak jantung, napas dingin, getar | Sebelum lunge |
| `ENDING_*` | Bad: dengung studio + napas Raka; Normal: hujan pagi; True: azan subuh samar + gamelan akustik; Secret: bisik nama | Musik pada cutscene |
| `Raka/Damar VO` | Dialog Indonesia sesuai naskah; Morse `R-A-K-A` | Subtitle speaker jelas |

EVP tetap metadata authored, bukan FFT mikrofon. Subtitle cue: `[Ketukan Morse: R-A-K-A]`, `[Api melesat dari tepi]`, `[Suara menyerupai Damar]`. VO manusia belum direkam tetap `TODO`, tidak digantikan TTS sebagai final.

### 7.4 Implementasi chat dan layar proyeksi aman performa

Timeline chat authored: pembuka 1.000.000 + donasi Rp66.666.666; S1 chat/banner salah; S2 chat serempak ke kiri; S3 chat padam saat `F`; panggung chat raksasa; boss makin serempak; `FINAL_CHOICE` hening. Semua riwayat tersimpan agar tidak hilang saat dikejar. Overlay 1.000.000 adalah teks statis; ribuan layar adalah quad/instance berbagi material. Username tidak diambil dari data pemain sungguhan.

### 7.5 LOD, lighting, budget rendering

Hindari banyak shadow caster dinamis; gunakan satu key light prioritas, light cookie/decal UV, lampu panggung baked, fog particle pooling, atlas wajah ≤24 wajah aktif. **Infinite Mirror Feedback Loop** (True Ending) memakai teknik loop render-target murah/ilusi, bukan ratusan kamera; opsi aksesibilitas menurunkan intensitas flash (§8.4). Profil CPU AI sesuai interest zone; replika/gelombang di luar sektor nonaktif tanpa mengubah progres. Frame-time dikriteriakan pada Windows Iris Xe.

---

## 8. Anti-softlock, balance config, fairness dan accessibility

### 8.0 Konfigurasi tuning (single source, baseline bukan kanon)

| Parameter | Default uji | Sumber/status |
|---|---:|---|
| `phone.normalDrain`, `uvArDrain`, `photoCost`, `blitzCost`, `blitzCooldown` | 0,05/s; 0,10 total/s; 1; 15; 6 s | Global P3/P4 |
| `phone.largePowerbankRestore` | +80 | Global |
| `sanity.gazeDrainGate` / `Sector` / `Arena` | 0,06 / 0,05 / 0,04 per s | BASELINE |
| `sanity.floorGate` / `Sector` / `Arena` | 50 / 45 / 45 | BASELINE |
| `sanity.shrineRestore`, `Hold`, `Cooldown` | +15, 10 s, 30 s | Diwarisi P4 |
| `sanity.incenseRestore`, `Cooldown` | ke 100, 12 s | Rancangan global |
| `sanity.incenseClarityDuration` | 20 s | BASELINE P4 |
| `chat.maxDuringChase` | 1 pesan / 1,2 s | BASELINE P4 |
| `chat.screenOffSilence` | 3 s | KANON (aturan Pita) |
| `replica.contactSanityLoss`, `iFrame`, `secondHitWindow` | −25, 3 s, 10 s | BASELINE P4 |
| `replica.search` | 6 s | BASELINE |
| `s1.corridorLength`, `replicaStart`, `replicaSpeed` | 28 m, 8 m, 1,3 m/s | BASELINE |
| `s1.selfieStallLook`, `Stall`, `Cooldown` | 1,0 s, 4 s, 6 s | BASELINE |
| `thermo.dwell` | 2 s | Diwarisi P2/P4 |
| `s1.wrongDoorSanityLoss`, `telegraph` | −25, ≥1,0 s | BASELINE |
| `s2.plankFallSanityLoss` | −10 | BASELINE |
| `evp.recordSeconds`, `radius` | 6 s, 3 m | Diwarisi P4 REPO-14 |
| `water.kendiCapacity`, `refillSeconds` | 3, 4 s | Diwarisi P4 |
| `s3.sriSpeed`, `genderuwoSpeed` | 1,4; 1,8 m/s | BASELINE |
| `boss.phase1.clones`, `shadowTouchLoss`, `shadowRespawn` | 4, −15, 6 s | KANON (4 sosok) / BASELINE |
| `boss.phase1.realSpeed` | 2,2 m/s | BASELINE |
| `boss.generator.zoneRadius`, `cooldown` | 6 m, 12 s | BASELINE |
| `boss.exposureSeconds` | 5 s | BASELINE P4 |
| `boss.phase2.thermo`, `range` | −30°C, ≤2,2 m | KANON suhu / BASELINE jarak |
| `boss.phase2.jawTelegraph` | 1,2 s | BASELINE |
| `light.flareDuration`, `radius`; `lantern.cooldown` | 10 s, 4 m; 15 s | BASELINE |
| `boss.phase3.waves`, `telegraph`, `sweep`, `gap` | 3, 2,0 s, 4 s, ≥12 s | BASELINE (jumlah gelombang usulan) |
| `cable.cutHold` | 2 s | BASELINE |
| `final.badDwell`, `labelMinVisible` | 2,0 s, 0,5 s | KANON / BASELINE |
| `final.normalHold` | 5,0 s | KANON |
| `final.secretHold` | 3,0 s | KANON |
| `final.syncSegment` | 1,5 s per segmen (Q, R) | BASELINE |
| `final.capacitor` | 5% satu aksi | KANON (angka) / KLARIFIKASI |
| `performance.720pLow` | avg ≥30, 1% low ≥24 FPS | **Target** belum divalidasi |

Semua nilai baru terpusat pada `Phase5Balance`/ScriptableObject; jangan memasukkan magic number ke coroutine. Bila target performa memaksa penurunan efek, prioritaskan **keterbacaan UV, benang boss, dan telegraph** daripada jumlah partikel.

### 8.1 Bukti jalur gratis untuk `wallet_legitimate=0`

Tanpa belanja pemain tetap dapat: mengisi baterai di dinamo dan memulihkan Sanity di altar doa; menyelesaikan tiga sektor dengan Kendi/Cawan; mengumpulkan tiga bukti; melewati tiga fase memakai Lentera bila Flare 0; dan menamatkan Bad/Normal/True. Secret hanya memerlukan lima ID unik, bukan uang.

### 8.2 Pembuktian baterai skenario ekstrem

Tiga Blitz inti butuh minimal **45 poin** + biaya optik; recharge fisik berulang tanpa saldo atau flare menjaga jalur mungkin. Uji `PHASE5_CP6` dengan 0%, 1%, 14%, 15%, 29%, 44%, 60%, 100%; dinamo terdekat harus reachable dari rute **tanpa UV** dengan landmark. Tidak ada Blitz otomatis pada 0 dan tidak ada tombol UI yang mengisi baterai gratis. Kapasitor Ritual tidak mengisi baterai.

### 8.3 Perilaku Sanity 0% yang harus diuji

Di Sanity 0% pemain tidak kehilangan kendali, tidak permadeath, tidak kehilangan `E/R/F/H`, tidak mendapat objektif palsu yang wajib diikuti, dan tidak disergap tanpa telegraph. Altar doa dan dinamo reachable; simbol UV terbaca; `FINAL_CHOICE` dapat diselesaikan dengan Sanity 0.

### 8.4 Fair-horror dan aksesibilitas

**Wajib:** UV menampilkan **bentuk** selain warna (lingkar penuh, silang, tengkorak, benang kontinu vs putus-putus berdenyut); EVP menampilkan label ASLI/TIRUAN + bentuk gelombang; Thermo menampilkan angka dan teks `[Suhu di luar rentang]`. Subtitle semua cue penting. Settings: kurangi camera shake, motion blur, strobo, chromatic aberration, dan **flash cermin** (True Ending); tahan-input menampilkan progres dan dapat dibatalkan; NORMAL tetap wajib tahan tombol layar5s sesuai arahan terbaru; konten sensitif (Bad ending: mengorbankan teman) mendapat opsi peringatan konten pada menu. Flash bertahap; jangan mewajibkan menatap strobo untuk membaca petunjuk. Tidak ada elemen yang hanya dapat diselesaikan dengan audio atau warna saja.

### 8.5 Backtrack, blocker dan ending

Jalur balik gerbang↔Ruang Cermin Saksi↔panggung harus fisik/loading boundary yang tidak mengubah state; tidak cukup teleport invisible. Setelah `CP6` jalur menutup dengan peringatan jelas. Setelah `CP8`, tidak ada backtrack; semua aksi akhir dapat diselesaikan di arena. Gagal save pada tahap mana pun tidak memulai cutscene ending.

---

## 9. Produksi Unity — komponen modular dan batas kepemilikan

| Tanggung jawab | Peran komponen kandidat, bukan klaim nama repo | Reuse / tambahan minimal |
|---|---|---|
| Flow P5 | `Phase5Director` | Gunakan event/receipt pattern P4; bukan game director global lain |
| Handoff & scene | `Phase5Hub`, router `GameShell` | Hubungkan `PHASE4_CP8` → `PHASE5_CP1` |
| Sanity | `Phase5SanityAdapter` | Perluas Sanity existing; sumber gaze |
| Chat satu juta | Provider `LiveStreamTimeline` P5 + `FakeChatCue` | Gunakan history/UI existing; tidak chat kedua |
| Sektor 1–3 | `Phase5Sectors` + `ReplicaBrain` | AI waypoint, LOS, telegraph; replika bukan boss |
| UV/AR/Thermo/EVP | Target authored `UvRajahSignature`, `ArEvidence`, `ThermoEvidence`, `EvpAudioEmitter` | Reuse scanner P2/P3/P4 |
| Kendi/air | `RuwatanWaterSource` | Instance Kendi P4, Cawan fallback |
| Boss | `Phase5Boss` | State fase, jendela inti, generator, lentera, kabel |
| Cahaya | `StageLightRig`, `LanternPost` | Flare/Lentera satu efek |
| Ending | `Phase5Ending` (`EndingId`, lock, sinkronisasi) | Pipeline cutscene existing/Timeline |
| Arsip/Cermin | `ArchiveMirror`, `ArchiveValidator` | Hitung dari ID unik |
| Save/persist | `Phase5Campaign` adapter | Receipt + CP1–CP9, migrasi terbukti |
| Dev QA | `Phase5DevOverlay` development builds | Sanity, battery, sector, core, cable, ending, chat source |

**Tahapan kerja produksi:** (M0) audit repo dan kontrak P5, (M1) handoff + gerbang + kios + Pita, (M2) tiga sektor distorsi, (M3) panggung + bukti + Jurnal + Ruang Cermin, (M4) boss tiga fase + progres inti, (M5) `FINAL_CHOICE` + empat ending, (M6) aset+audio+VO, (M7) regresi P1–P4 + 180 QA, (M8) Windows acceptance. Jangan memberi klaim PASS hanya berdasarkan validator headless/structural.

---

## 10. QA acceptance — 180 skenario (SEMUA NOT_RUN)

**Status:** QA-001–QA-180 adalah kontrak acceptance, belum ada hasil tes Unity; jangan menandai PASS tanpa bukti build dan log. Kategori A–O memuat 10 skenario per kategori; kategori P menambah 30 skenario integrasi. Nomor stabil untuk handover dan laporan `Audit/phase5-qa-matrix.md`. Status yang diizinkan: `NOT_RUN / PASS / FAIL / BLOCKED`.

### 10.1 A — Kontinuitas P1–P4, handoff dan input

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-001 | Continue dari `PHASE4_CP8` dengan empat segel valid | Masuk gerbang Pasar Setan tanpa Kos; P0–P4 utuh |
| QA-002 | Mulai P5 tanpa `P4_COMPLETE` atau tanpa empat segel (build produksi) | Ditolak; skip dev-only tidak menulis segel/inventori palsu |
| QA-003 | Muat save P4 berisi Flare/Kemenyan/Powerbank belum terpakai | Semua carry-over tampil sebagai instance sah |
| QA-004 | Muat save yang berhenti di boundary P4 sebelum router terhubung | Save tidak rusak; Continue menuju `PHASE5_CP1` setelah router siap |
| QA-005 | Ganti `V` OTS↔FP sambil UV/AR aktif | FOV/culling/scan konsisten dan kamera pulih |
| QA-006 | Tekan `X` (Selfie) dan verifikasi hint | Selfie = `X`; hint HUD memakai binding aktual |
| QA-007 | Remap `X/Q/R/T/C/F/G/H/E` via `GameInputConfig` | Hint mengikuti binding; tidak ada konflik |
| QA-008 | Tahan/lepas RMB dengan AR/UV/Thermo bergantian | HP dua tangan bekerja; view kembali |
| QA-009 | `G` saat Flare 4 | Flare aktif di P5 (tidak “Api asing ditolak desa”), stok −1 |
| QA-010 | Alt-tab saat animasi angkat HP | Input tidak macet; timer sesuai policy fokus |

### 10.2 B — Gerbang, Kios Tumbal dan ekonomi

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-011 | Masuk dengan `wallet_legitimate=0`, baterai 0, Sanity 0 | Dinamo dan Altar Doa terjangkau tanpa UV; tidak softlock |
| QA-012 | Lihat viewer 1.000.000 dan donasi Rp66.666.666 | Donasi → `wallet_bound` sekali; bukan `wallet_legitimate` |
| QA-013 | Reload setelah donasi | Donasi tidak digandakan |
| QA-014 | Beli Powerbank ×3 dengan saldo cukup | Debit sekali, tiga instance unik, receipt persisted |
| QA-015 | Klik beli paket sama dua kali cepat | Tidak menggandakan item; saldo tak negatif |
| QA-016 | Force quit saat commit pembelian | Penuh atau rollback; tidak ada item tanpa debit |
| QA-017 | Saldo 0: semua tombol Kios | Nonaktif; gerbang dalam tetap dapat dicapai |
| QA-018 | Baca Kertas Aturan lalu lewati di run lain | Opsional; progres sama |
| QA-019 | Lihat kios saat chat menyuruh “jangan matikan siarannya” | `F` tetap mematikan layar; chat tidak memblokir |
| QA-020 | Selesaikan P5 tanpa membeli apa pun | Semua bukti, tiga inti, dan ending dasar tercapai |

### 10.3 C — Pita Rekaman Pertama dan Arsip Saksi

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-021 | Periksa Kios Arsip Bisu tanpa membayar | Dapat diperiksa gratis; Kios Tumbal terpisah |
| QA-022 | Layar mati 2 s di depan kursi | Chat palsu belum berhenti; langkah belum sah |
| QA-023 | Layar mati ≥3 s di depan kursi | `P5_SECRET_CHAIR_SILENCE` sah; chat palsu pada kursi berhenti |
| QA-024 | Nyalakan UV (bukan AR) di depan laci | Laci tidak terungkap; feedback mode salah |
| QA-025 | AR di depan kursi/laci | Tanda mata tertutup terlihat; laci terbuka secara visual |
| QA-026 | EVP merekam kalimat Mimic dari cermin | Tidak sah; laci tidak terbuka |
| QA-027 | EVP merekam “Saksi tidak membeli korban” Authentic | Langkah sah; pita dapat diambil |
| QA-028 | Ambil pita → `TryCommit` sukses | ID di `secret_items` sebelum popup; prop hilang setelah commit |
| QA-029 | Disk penuh saat pickup | Popup tidak muncul; prop tersedia untuk retry |
| QA-030 | Mati/reload setelah pita diambil | ID tetap; objek tidak muncul lagi; Arsip N/5 aktual |

### 10.4 D — Sanity, chat dan Replika umum

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-031 | Lihat cermin lama di gerbang | Drain 0,06/s sampai lantai 50%; tidak di bawah karena waktu saja |
| QA-032 | Sanity ke 0 di sektor | HUNT aktif; bukan Game Over |
| QA-033 | Pulihkan lewat Altar Doa pada Sanity 0 | +15 setelah tahan 10 s; cooldown 30 s |
| QA-034 | Kemenyan `H` dengan stok 0 | Tidak negatif; tidak ada animasi palsu |
| QA-035 | Kemenyan `H` dengan stok 1 | Pulih ke 100; cooldown 12 s; satu instance berkurang |
| QA-036 | Pause/alt-tab 2 menit | Tidak menguras Sanity di background |
| QA-037 | Chat `SYSTEM` palsu muncul | Tidak memanggil dispatcher/binding/checkpoint |
| QA-038 | Chat 40 pesan saat chase | ≤1 pesan/1,2 s; reticle dan prompt `E` tidak tertutup |
| QA-039 | Opsi kurangi strobo/blur/shake | Clue tetap terbaca |
| QA-040 | Replika masuk Sanity 0 | Radius deteksi naik; telegraph tetap ada |

### 10.5 E — Sektor 1 Koridor Beku

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-041 | Masuk koridor dengan replika di 8 m | Replika mendekat 1,3 m/s; tidak instakill |
| QA-042 | Selfie (`X`) menahan replika 1,0 s | Jeda 4 s; cooldown 6 s |
| QA-043 | Thermo pintu kiri | Bacaan `+50°C` setelah bidik stabil 2 s |
| QA-044 | Thermo pintu kanan | Bacaan `+50°C` |
| QA-045 | Thermo pintu tengah | Bacaan `-22°C` |
| QA-046 | Chat/banner menyebut “tengah panas” | Angka Thermo dunia tetap menang |
| QA-047 | Masuk pintu kiri/kanan | Semburan telegraph ≥1,0 s; −25 Sanity; kembali ke awal koridor |
| QA-048 | Masuk pintu tengah sebelum replika menerkam | `P5_S1_CLEAR`; `PHASE5_CP3` |
| QA-049 | Baterai 0 di awal koridor | Dinamo/papan arah tengah tersedia tanpa UV |
| QA-050 | Mati di koridor lalu reload | Mulai dari CP2; sektor 1 belum clear |

### 10.6 F — Sektor 2 Jembatan Gilingan Bambu

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-051 | EVP cabang kiri | Merah bergerigi, label TIRUAN (Wewe) |
| QA-052 | EVP cabang kanan | Hijau Morse, label ASLI, subtitle `R-A-K-A` |
| QA-053 | Chat serempak ke kiri | Tidak memaksa; jalan kanan tetap valid |
| QA-054 | UV pada papan kanan | Rajah Ungu tengkorak = lantai runtuh |
| QA-055 | Injak papan Rajah Ungu | Telegraph; jatuh ke anchor; −10 Sanity; tidak fatal |
| QA-056 | Siram Obor Santet dengan Kendi air>0 | Obor padam; air −1 tepat sekali |
| QA-057 | Siram dengan Kendi air=0 | “KENDI KOSONG”; tunjuk titik isi ulang; tidak negatif |
| QA-058 | Isi ulang di titik sebelah gerbang | Isi hingga 3; tahan `E` 4 s; tidak overfill |
| QA-059 | Save tanpa Kendi (item hilang) | Cawan Air Pasar menggantikan; tidak softlock |
| QA-060 | Force quit setelah siram sebelum commit | Air dan obor konsisten dari commit terakhir |

### 10.7 G — Sektor 3 Ruang Jahit Cermin

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-061 | Masuk gelap total dengan layar menyala | Replika mendeteksi cahaya 12 m; telegraph ada |
| QA-062 | `F` lalu merunduk di lemari | Replika lewat/investigasi lalu pergi |
| QA-063 | Sembunyi di kolong meja saat dikejar | LOS harus putus; bukan invulnerability |
| QA-064 | Sprint vs jalan vs jongkok pada jarak sama | Radius 9 / 4 / 1,5 m sesuai baseline |
| QA-065 | Kontak pertama | −25 Sanity, knockback, i-frame 3 s |
| QA-066 | Kontak kedua dalam 10 s | `CAUGHT_REPLICA`; retry sektor, bukan seluruh babak |
| QA-067 | AR di cermin tanpa mode AR | Permukaan tampak cermin biasa; tidak masuk |
| QA-068 | AR di Cermin Rias | Portal cair hijau glitch; masuk → `P5_S3_CLEAR` |
| QA-069 | Replika spawn di anchor | Tidak terjadi; ada grace |
| QA-070 | Mati lalu reload di sektor 3 | Sektor 1–2 tetap clear; mulai CP4 |

### 10.8 H — Panggung, tiga bukti dan Jurnal

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-071 | Dialog Damar diulang | Tidak mengubah state |
| QA-072 | AR pada Server Batu | `P5_EVIDENCE_SERVER_AR` sekali |
| QA-073 | Thermo `-- °C` pada Kontrak saja | Sub-langkah; bukan bukti |
| QA-074 | UV Mata Terbuka saja | Sub-langkah; bukan bukti |
| QA-075 | Thermo lalu UV (atau sebaliknya) | `P5_EVIDENCE_CONTRACT` sekali |
| QA-076 | EVP Pilar Layar (Authentic) | Bukti #3; Mimic dari layar pinggir tidak dihitung |
| QA-077 | Konfirmasi Jurnal dengan 2 bukti | Tombol tertutup |
| QA-078 | Konfirmasi dengan 3 bukti, empat segel | `P5_JOURNAL_CONFIRMED` sekali; reload tetap |
| QA-079 | Tutup Jurnal tanpa konfirmasi | Tidak ada `ARENA_LOCK` otomatis |
| QA-080 | Konfirmasi tanpa salah satu segel (dev hack) | Ditolak; daftar syarat tampil |

### 10.9 I — Ruang Cermin, arena lock dan Boss Fase 1–2

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-081 | Ruang Cermin Saksi menampilkan N/5 | Hitung dari ID unik aktual |
| QA-082 | Cermin Jalan Pulang ke gerbang sebelum CP6 | State utuh; kembali ke ruang cermin |
| QA-083 | Konfirmasi arena | Peringatan “Arena akan terkunci”; CP6 commit sebelum boss |
| QA-084 | Fail write CP6 | Boss tidak mulai; retry |
| QA-085 | Fase 1: UV pada 4 sosok | Hanya 1 benang kontinu berdenyut ke kurungan |
| QA-086 | Bayangan layar disentuh | −15 Sanity, larut, respawn 6 s |
| QA-087 | Pancing sosok asli ke Zona Generator, nyalakan sakelar | Kabut terbakar; jendela 5 s |
| QA-088 | Blitz tepat kepala lensa | Inti 1 pecah; commit atomik; CP7 |
| QA-089 | Miss Blitz Fase 1 | Perisai kembali; generator cooldown 12 s; retry |
| QA-090 | Fase 2: Thermo `-30°C`, Selfie rahang, Flare, Blitz | Inti 2 pecah via Selfie; CP7 |

### 10.10 J — Boss Fase 3, kabel dan Damar

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-091 | Fase 2 dengan Flare 0 | `G` menurunkan Lentera; efek sama |
| QA-092 | Fase 2 tanpa Kemenyan | Altar doa menjaga solvability |
| QA-093 | Gelombang api: layar mati di balik peti | Lewat setelah telegraph 2,0 s |
| QA-094 | Peti kayu bukan invulnerability | LOS/collider menentukan |
| QA-095 | AR memperlihatkan tiga Kabel Roh | Tiga target terlihat hanya dengan AR |
| QA-096 | Tahan `E` 2 s lalu batal | Tidak ada receipt; kabel tetap |
| QA-097 | Putus kabel 1, mati, reload | `P5_CABLE_1_CUT` tetap; kabel 2–3 tersisa |
| QA-098 | Putus ketiga kabel | Kurungan terbuka; gelombang berhenti |
| QA-099 | Blitz Inti 3 | Entitas berlutut; CP7 → `FINAL_CHOICE_ENTER` |
| QA-100 | Kematian di Fase 3 setelah Inti 1–2 committed | Mulai awal Fase 3, bukan Fase 1 |

### 10.11 K — `FINAL_CHOICE` dan guard input

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-101 | Masuk `FINAL_CHOICE` | CP8 commit; kapasitor 5%; `ending_id=NONE` |
| QA-102 | Tahan `F` 5 s sebelum `FINAL_CHOICE` | Hanya toggle layar; tidak ada ending |
| QA-103 | LMB pada Damar sebelum state | Tidak ada efek |
| QA-104 | Tiga aksi dasar terlihat tanpa Secret | Bad/Normal/True tersedia |
| QA-105 | Aksi Secret terlihat tanpa lima ID | Tidak ada aksi Secret; slot kosong menyebut bab |
| QA-106 | Dua aksi ending hampir bersamaan | Hanya satu `TryLockEnding` menang |
| QA-107 | Input ending setelah `endingId!=NONE` | Ditolak |
| QA-108 | Kapasitor setelah LMB ending | Menjadi 0; baterai persisten tidak berubah |
| QA-109 | Sanity 0 pada `FINAL_CHOICE` | Semua aksi tetap bisa diselesaikan |
| QA-110 | Tidak ada timer fatal | Entitas hanya bicara/menekan UI |

### 10.12 L — Bad dan Normal

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-111 | Arahkan kamera belakang ke Damar <2 s | Label TUMBAL belum; LMB tidak diterima |
| QA-112 | Dwell 2 s, label TUMBAL ≥0,5 s, LMB | Bad terkunci; cutscene Damar ditarik cermin |
| QA-113 | Turunkan HP sebelum LMB | Batal tanpa penalti |
| QA-114 | Epilog Bad: saldo Rp500.000.000 | State sinematik; wallet tidak berubah |
| QA-115 | Epilog Bad: akun `Penonton_Terakhir` | Sesuai naskah; credits |
| QA-116 | Di Z_EDGE tahan `F` 5 s | Progres terlihat; Normal terkunci |
| QA-117 | Lepas `F` sebelum 5 s | Batal tanpa penalti |
| QA-118 | Lepas F pada 4,9s lalu tekan lagi | Progres reset; Normal hanya terkunci setelah tahan5s penuh |
| QA-119 | Epilog Normal: HP hancur | Efek sinematik; save konsisten |
| QA-120 | Epilog Normal: Damar di halaman Atmaja | Pantulan Raka; credits |

### 10.13 M — True dan Secret

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-121 | Papah Damar ke Z_PORTAL dengan `E` | Escort 1,1 m/s; dapat dilepas |
| QA-122 | Urutan `X→Q→R` benar | Gauge penuh; reticle inti di layar Selfie |
| QA-123 | Urutan salah (`R` sebelum `Q`) | Gauge reset tanpa penalti |
| QA-124 | AR dan UV di gameplay biasa | Tetap saling menggantikan di luar `SYNC_CALIBRATION` |
| QA-125 | LMB pada inti di Selfie | True terkunci; baterai persisten tak berubah |
| QA-126 | Epilog True: 05:30, azan, Rp125.000.000 | Sesuai naskah; credits |
| QA-127 | Lima ID valid; validasi di cermin | `P5_ARCHIVE_VALIDATED` tanpa memilih ending |
| QA-128 | Tahan `E` 3 s di Cermin Saksi lalu batal | Tidak ada ending; progres batal |
| QA-129 | Mode Arsip: bidik Damar | Target tidak valid |
| QA-130 | Mode Arsip: cermin berbidikan emas + LMB | Secret terkunci tepat sekali |

### 10.14 N — Checkpoint, save, schema

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-131 | Migrasi save P4 valid | P0–P4, segel, arsip, ekonomi, inventori utuh |
| QA-132 | Inti 1 sukses, force quit | Continue CP7 dengan inti 1 pecah |
| QA-133 | Inti 1–2 sukses, mati Fase 3 | Receipts utuh; tidak kembali CP6 |
| QA-134 | Fail write saat inti pecah | UI tidak tampil sukses; jendela diulang |
| QA-135 | Fail write `FINAL_CHOICE_ENTER` | Tetap di arena; retry |
| QA-136 | Crash setelah `ending_id` terkunci | Continue mengulang cutscene ending, bukan `FINAL_CHOICE` |
| QA-137 | Double click ending | Satu enum, satu CP9 |
| QA-138 | Lock ending gagal tulis | Input tetap valid; tidak ada cutscene |
| QA-139 | `wallet_bound` Rp66.666.666 setelah reload | Tetap satu kali |
| QA-140 | Bandingkan save CP8 vs CP9 | CP8 `NONE`; CP9 punya `ending_id` |

### 10.15 O — Visual, audio, performa, aksesibilitas

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-141 | 720p Low gerbang dengan banyak cermin | avg ≥30 FPS; wajah aktif ≤24 |
| QA-142 | 720p Low arena dengan gelombang api | Overdraw/shadow dalam budget |
| QA-143 | UV + AR bergantian di fase 1/3 | Tidak ada kamera kedua |
| QA-144 | Simulasi buta warna | Lingkar/silang/tengkorak dan benang kontinu/putus terbedakan |
| QA-145 | Subtitle Morse/api/ketukan | Terbaca tanpa audio |
| QA-146 | Opsi kurangi flash cermin (True) | Epilog tetap terbaca; flash turun |
| QA-147 | Peringatan konten Bad ending | Opsi muncul dan dihormati |
| QA-148 | Audit semua asset Pasar Setan | URL, lisensi komersial, SHA-256, penempatan di manifest |
| QA-149 | Kit visual replika P5 baru | Lisensi/URL/hash tercatat dan identitas sumber tidak berasal dari fase1–4; ID P5 terpisah |
| QA-150 | Windows Iris Xe RAM 8GB, ±75 menit | Visual/FPS/VO diverifikasi nyata; headless tidak mengklaim PASS |

### 10.15A P — Tambahan integrasi dan konflik hasil penyusunan (QA-151–QA-180)

| ID | Langkah uji / skenario | Hasil wajib |
|---|---|---|
| QA-151 | Replika Kuntilanak Merah bukan boss | Tidak ada segel/HP; larut saat sektor clear |
| QA-152 | Segel P1–P4 setelah semua replika | `location_seals` tidak berubah |
| QA-153 | Thermo bukan oracle pintu karena chat | Hanya angka dunia |
| QA-154 | Kendi tunggal sepanjang P4→P5 | Satu instance; volume 0–3 konsisten |
| QA-155 | Dua inti dalam satu jendela | Tidak mungkin; satu Blitz = satu inti |
| QA-156 | Blitz di luar jendela | Tidak memecah inti; policy baterai global konsisten |
| QA-157 | Generator sakelar tanpa sosok di zona | Tidak membuka jendela |
| QA-158 | Lentera: ambil, jatuhkan, cooldown 15 s | Reusable; tidak menggandakan |
| QA-159 | Flare dan Lentera bersamaan | Satu efek; tidak menumpuk |
| QA-160 | Chat palsu menyuruh menoleh di Fase 2 | Tidak mengubah kontrol; Thermo/Selfie tetap benar |
| QA-161 | Layar mati 3 s menghentikan chat palsu | Aturan global; konsisten dengan puzzle Pita |
| QA-162 | Kapasitor 5% vs baterai 100% sebelum ending | Dipisah; baterai tidak berkurang |
| QA-163 | Normal tanpa LMB | Kapasitor tidak terpakai; ending sah |
| QA-164 | Bad lalu coba True | Ditolak; satu enum |
| QA-165 | True lalu coba Secret | Ditolak |
| QA-166 | Secret tanpa Pita (lima ID tidak lengkap) | Tidak tersedia |
| QA-167 | Secret dengan lima ID benar tetapi tanpa validasi | Validasi dapat dilakukan di `FINAL_CHOICE` |
| QA-168 | Arsip N/5 dengan kombinasi kepemilikan P1–P5 | Hitung ID unik; tidak asumsi 5/5 |
| QA-169 | Pita diambil setelah kembali dari ruang cermin | Cermin Jalan Pulang berfungsi; CP tidak turun |
| QA-170 | Cermin Jalan Pulang setelah CP6 | Tertutup; peringatan jelas |
| QA-171 | Baterai 0 saat CP6 | Dinamo arena reachable tanpa UV |
| QA-172 | Sanity 0, baterai 0, tanpa kemenyan, Flare 0 | Seluruh boss solvable |
| QA-173 | Satu juta wajah/chat | Tidak ada render ribuan UI per frame |
| QA-174 | Chat satu juta humanis | Pesan manusia bisa benar; tidak sebagai oracle |
| QA-175 | Nama korban baru tidak dikarang | Rekaman memakai data arsip kanon |
| QA-176 | Dua cutscene bersamaan | Tidak terjadi |
| QA-177 | `CP9` gagal tulis setelah cutscene | Pesan error; credits tidak dianggap tercatat sampai commit |
| QA-178 | Selesaikan Run A: tanpa toko, tanpa chat, tanpa rahasia | Bad/Normal/True dapat dicapai |
| QA-179 | Selesaikan Run B: semua paket + lima Arsip | Secret dapat dicapai; jalur utama sama |
| QA-180 | Regresi P1–P4 pascamigrasi dan CP7 rolling P5 | Save P0–P4, inventori, uang, arsip, checkpoint lama tidak diregresi |

### 10.16 Definition of Done (gerbang penerimaan Phase5)

1. Lima adegan, seluruh ucapan, angka kanon, dan empat ending dapat dialami tanpa menduplikasi sistem P0–P4.
2. Sanity aktif tier 70/40/0; Sanity 0 tidak Game Over dan pemulihan gratis nyata.
3. Tiga sektor dapat diselesaikan tanpa membaca chat; angka Thermo dan bentuk UV tidak pernah berbohong.
4. Tiga bukti unik (bukti #2 butuh dua sub-pengamatan) dan Jurnal dikonfirmasi eksplisit; arena tidak terkunci saat inspeksi.
5. Tiga inti committed satu per satu; kabel dan inti tidak diulang setelah mati; tiga Blitz inti tepat.
6. Replika Memori tidak mengubah segel dan tidak menjadi boss.
7. Kendi/Cawan, dinamo, altar doa, dan Lentera membuat babak solvable tanpa uang.
8. `FINAL_CHOICE` memakai satu enum dan kunci atomik; Kapasitor Ritual terpisah dari baterai; ending hanya dari aksi fisik pada state itu.
9. Bad, Normal, True selalu tersedia; Secret hanya dengan lima ID unik.
10. `PHASE5_CP9` menyimpan ending; 180 QA dilaporkan dengan status dan bukti; **tidak ada P0/P1 blocker** sebelum phase-freeze.

---

## 11. Kontinuitas cerita, risiko, dan keputusan terbuka

### 11.1 Audit kontinuitas Babak 1–5

| Topik | Ketentuan / apa yang harus diuji |
|---|---|
| Empat segel | Rusunawa = 1, RSUD = 2, Pabrik = 3, Karang Setra = 4. Pasar Setan adalah **gerbang kelima**, bukan segel kelima; P5 tidak menambah `location_seals` |
| Rumah Atmaja | Titik awal narasi; replikanya muncul di Sektor 3 dan portal cermin; bukan segel |
| Damar | Prolog (Handycam/cermin rias), P3 (kartu pers/sepatu), P4 (kacamata/catatan), P5 (kurungan, dialog, Morse `R-A-K-A`) — tidak ada kontradiksi lokasi |
| Hantu lama | Kuntilanak Merah, Wewe Gombel, Sri Atmaja, Genderuwo, Banaspati: **replika memori**, bukan encounter aktif baru (E08) |
| Mbah Suro | P4 mengakhiri arwahnya; P5 hanya manuskrip dan asal `Penonton_Terakhir` |
| `MataBatin.apk` | Terungkap berasal dari pengikut Mbah Suro (P4); P5: dihapus pada True/Secret, hancur pada Normal |
| Viewer & dana | P4 akhir 999.999 → P5 1.000.000; donasi Rp66.666.666 terikat; epilog uang sinematik |
| Arsip Saksi | P5 menyediakan butir ke-5 jika empat sebelumnya dimiliki; Secret butuh lima ID unik aktual |
| Sanity | Dilanjutkan dari P4; nilai save dipertahankan |
| Waktu | P5 malam yang sama dengan P4 tanpa Kos; epilog True pukul 05:30 WIB |
| Ibu Raka | Biaya perawatan dibahas pada epilog True/Secret/Bad sebagai narasi |

### 11.2 Register keputusan dan baseline (`P5-Dxx`)

| ID | Perkara | Baseline kerja | Risiko bila berbeda |
|---|---|---|---|
| **P5-D01** | Replika memori vs boss penuh untuk hantu lama | Replika memori, tanpa segel/HP | Kontradiksi lore/segel; ekspektasi boss ganda |
| **P5-D02** | Selfie = `X` (bukan `V`) | `X` sesuai binding aktual | Kebingungan input |
| **P5-D03** | Kapasitor Ritual 5% | State-lokal, bukan baterai persisten | Baterai persisten berubah diam-diam |
| **P5-D04** | Kios opsional dan harga | Powerbank 750rb, Kemenyan 700rb (4), Flare 600rb (4), Isi Daya 100rb | Ekonomi/QA toko |
| **P5-D05** | Pengganti Flare | Lentera Panggung reusable | Fase 2 tak solvable tanpa Flare |
| **P5-D06** | Kemenyan opsional di Fase 2 | Altar Doa + lantai Sanity | Paywall tersembunyi |
| **P5-D07** | Nama korban pada rekaman Secret | Data arsip kanon; tidak ada nama baru | Fabrikasi lore / ketidakkonsistenan P1–P4 |
| **P5-D08** | Progres inti persisten (CP7 rolling) | Ya | Berbeda dari boss P1–P3 all-or-nothing |
| **P5-D09** | Jumlah gelombang Fase 3 | 3 gelombang, telegraph 2,0 s | Terlalu panjang/mudah |
| **P5-D10** | Jendela inti | 5 s | Aksesibilitas ponsel |
| **P5-D11** | Cermin Jalan Pulang (backtrack ke kios) | Ada sebelum CP6; tutup setelah | Item kelima tak terjangkau / alur lebih panjang |
| **P5-D12** | Replay pilihan akhir (CP8) | [PERLU KEPUTUSAN]; default tidak ada replay otomatis | Lock ending vs kebebasan melihat ending lain |
| **P5-D13** | Bad ending: dwell + label + LMB | Dua langkah sadar | Terpicu tidak sengaja |
| **P5-D14** | Normal: tahan `F` 5 s | Wajib5s sesuai tambahan pengguna; progres/cancel terlihat | Jangan mengganti menjadi dua kali tekan |
| **P5-D15** | Chat satu juta humanis | Pesan manusia terjebak; bisa benar | Nada terlalu gelap/kurang |
| **P5-D16** | Wajah penonton di cermin | Sintetis/berlisensi; ≤24 aktif | Privasi/lisensi/performa |
| **P5-D17** | Peringatan konten Bad ending | Opsi menu | Aksesibilitas konten |
| **P5-D18** | Nama schema/class actual | Audit repo; jangan menebak | Save carry-over rusak |
| **P5-D19** | Target Windows Iris Xe | 720p Low avg ≥30, 1% low ≥24 | Linux headless tak memadai |
| **P5-D20** | Arah VO dan desain tiga wujud boss | Lensa/cermin/jubah layar; layered VO | Kesesuaian artistik/lisensi |

**Status:** audit awal selesai; baseline teknis di atas dipakai berdasarkan instruksi implementasi pengguna, dengan koreksi §0.11. Perubahan kanon baru tetap memerlukan keputusan desainer. Kondisi `KANON` (tiga inti, tiga bukti, empat ending, lima Arsip Saksi, Sanity 0 bukan mati, jalur gratis, tanpa Kos) tidak boleh dibalik diam-diam. Bila QA atau geometri memaksa penyesuaian parameter, revisi baseline beserta alasan dan bukti, bukan menghapus kriteria penting.

### 11.3 Risiko P0/P1 yang perlu dilaporkan lebih dulu

- **P0 Gameplay:** Kemenyan/Flare benar-benar wajib padahal stok 0; baterai 0 tanpa dinamo reachable; boss hanya bisa dikalahkan dengan Blitz keempat; replika membunuh tanpa telegraph; `FINAL_CHOICE` terpicu tidak sengaja; ending tak terjangkau pada Sanity 0.
- **P0 Save:** inti pecah tanpa atomik; ending terkunci tanpa commit; `CP9` tertulis sebelum cutscene; dua `ending_id`; Pita muncul popup sebelum tersimpan.
- **P1 Presentation:** UV/benang tidak terbaca di 720p; chat menutup reticle; flash cermin berlebihan; VO final TTS; kios mengaburkan kemewahan opsi; wajah penonton memakai identitas nyata.
- **P1 Engineering:** banyak shadow caster; jutaan UI; overlap layer UV/AR; router handoff P4→P5 merusak save lama; replika menimpa segel.

---

## 12. Checklist produksi

**M0 — Audit repository sebelum coding**

- [x] Baca `Phase5.md` ini secara penuh, `Phase4.MD`, dokumen Phase1–3 dalam repo, dan `Rancangan.MD`. Catat `READY / ADAPT / BUILD / BLOCKED` dengan bukti path + method.
- [x] Temukan `CampaignState`/loader/`TryCommit`, mode HP, input, Jurnal, router `GameShell`, boundary P4; catat versi schema dan alias receipt.
- [x] Tinjau 20 P5-Dxx; perubahan kanon/story butuh persetujuan.

**M1 — Handoff, gerbang, kios, Pita**

- [x] Router `PHASE4_CP8` → `PHASE5_CP1`; tanpa vestibule duplikat.
- [x] Dinamo Gerbang + Altar Doa; chat/donasi idempoten; Kios Tumbal opsional; Kios Arsip Bisu tiga langkah.

**M2 — Tiga sektor distorsi**

- [x] S1 Selfie×Thermo; S2 EVP×UV×Kendi/Cawan; S3 `F`×AR; replika dengan ID terpisah; CP2–CP5.

**M3 — Panggung dan Jurnal**

- [x] Tiga bukti bebas urutan; bukti #2 dua sub-langkah; Dokumen #2; Ruang Cermin Saksi; Cermin Jalan Pulang; `CP6`.

**M4 — Boss tiga fase**

- [x] Fase 1 UV/generator/Blitz; Fase 2 Selfie/Thermo/Flare-Lentera/Blitz; Fase 3 AR/kabel/cover/Blitz; CP7 rolling.

**M5 — `FINAL_CHOICE` dan ending**

- [x] Enum tunggal + lock atomik; Bad, Normal, True (`X→Q→R`), Secret; Kapasitor Ritual; cutscene + credits; `CP9`.

**M6 — Aset, audio, VO**

- [x] 152 identitas model baru berlisensi; manifest; audio/SFX baru; animasi Idle/Walk manusia CC0 baru; daftar rekaman dan render native.
- [ ] VO manusia final dan benchmark GPU Windows/IrisXe — belum tersedia sebagai bukti penerimaan.

**M7 — QA & acceptance**

- [ ] Jalankan 180 QA dengan kasus tepi 0 money/0 Sanity/0 battery/no Flare/no Kemenyan.
- [x] Regresi otomatis campaign/save P1–P4; XML memuat hasil per case.
- [ ] Penerimaan manual seluruh input, ekonomi, Jurnal dan preview camera pada perangkat Windows.
- [x] Buat `Audit/phase5-feasibility.md`, `Audit/phase5-qa-matrix.md`, `Audit/phase5-implementation.md`, asset manifests.
- [x] Tandai tiap QA `PASS/FAIL/BLOCKED/NOT_RUN` dengan bukti; jangan klaim 180 PASS dari build headless.

---

## 13. Catatan penyusunan: sumber, penambahan, dan yang sengaja ditolak

| Area | Dari `Phase5.MD` | Perluasan beralasan (dokumen ini) | Tidak diadopsi |
|---|---|---|---|
| Cerita dan lokasi | 5 adegan, 75 menit, Pasar Setan, tiga sektor, tiga inti, empat ending | Denah, ruang cermin, jalur balik, zona final | Memecah jadi 6 adegan |
| Mekanik Sanity | Disebut di checklist | Sumber gaze, lantai ambien, altar doa | Sanity 0 otomatis kalah |
| Chat | Satu juta penonton, donasi Rp66.666.666, chat terinfiltrasi | Kelompok sumber, rate limit, `F` 3 s | Classifier benar/salah |
| AI | Kuntilanak Merah, Wewe, Atmaja, Genderuwo, Banaspati | Replika memori, state, kontak 2-hit | Boss hidup kembali |
| Bukti | AR server, Thermo+UV kontrak, EVP gabungan | Sub-langkah, ID unik | Menaikkan jumlah bukti |
| Boss | Tiga fase, tiga inti, generator, Flare, kabel | Jendela 5 s, lentera, receipt inti | HP bar; reset boss penuh |
| Secret | Pita Rekaman, lima Arsip, Secret Ending | Kapasitor, validasi, N/5 aktual | Hitungan 5 tanpa ID |
| Ending | Bad/Normal/True/Secret, input fisik | Satu enum, guard, tanpa timer fatal | Menu dialog ending |
| Engineering | Hanya checklist | QA 180, registry, CP1–CP9, risiko | Memaksa schema tertentu |

### 13.1 Tingkat keyakinan dan bukti

Lokasi, urutan adegan, dialog, nama tokoh, nominal, dan item rahasia bersumber dari **`Phase5.MD` (revisi 15 Oktober 2026)**. Struktur §0–§14, strategi save, dan pola solvability mengikuti **`Phase4.MD` v2.1** dan merupakan **spesifikasi yang perlu audit**, bukan laporan build. Angka tuning, harga, nama ID, jumlah gelombang, jarak, dan pilihan fallback adalah **usulan teknis**. Dokumen ini ditulis tanpa akses repository sehingga tidak memuat klaim tentang kode aktual; setiap kontrak repo ditandai [PERLU AUDIT].

### 13.2 Evaluasi mandiri dokumen

| Kriteria (penilaian subjektif atas dokumen, belum build) | Skor | Catatan |
|---|---:|---|
| Ketelitian naskah dan kontinuitas | 9,0 | Kanon dipertahankan; 21 konflik ditutup |
| Perincian parameter AI/level | 8,4 | Baseline P4 diwarisi; perlu playtest |
| Inovasi dibanding P1–P4 | 8,6 | Ujian ulang modul + pilihan fisik |
| Ketahanan softlock dan retry | 9,0 | Progres inti, jalur gratis, lock atomik |
| Kemudahan audit | 9,0 | 180 QA, registry, checklist M0–M7 |

**Penilaian bukan verifikasi produksi.** Tidak ada bukti bahwa 180 QA telah dijalankan atau bahwa build memenuhi sasaran.

---

## 14. Peta integrasi teknis Unity dan handover audit

### 14.1 Reuse atau bangun secara aditif

| Kebutuhan | Referensi (diverifikasi saat M0) | Implementasi aman P5 |
|---|---|---|
| Campaign & checkpoint | `CampaignState`, `CampaignData`, `TryCommit` | Tambah `Phase5Data`; tidak ada singleton baru; CP1–CP9, CP7 rolling |
| Input & kamera | `GameInputConfig`, `ShoulderCameraRig`, `SmartphoneController` | Satu phone controller; Selfie `X`; sync state hanya `FINAL_CHOICE` |
| AR/Thermo/EVP/UV | Mode scanner P1–P4 | Target authored; layer UV/AR existing |
| Live stream & kios | Provider chat deterministic, shop receipt | Event P5; tanpa koneksi streaming nyata |
| Jurnal | Jurnal provider P3/P4 | Tab arena/pilihan akhir; Arsip N/5 |
| Aktor & interaksi | `IInteractable`, `InteractionInteractor` | Dinamo, altar, obor, generator, kabel, cermin |
| AI | Direktur threat P3/P4 | `ReplicaBrain` waypoint; tidak mengubah AI lama |
| Rendering | URP + layer UV | Shader/decal bentuk; fog murah; Iris Xe |
| Routing | `GameShell`, scene Babak4 | Scene `Babak5_PasarSetan`; handoff dari boundary P4 |
| Ending/Credits | Pipeline cutscene | `Phase5Ending`, enum, lock, `CP9` |

### 14.2 Matriks prioritas audit sebelum implementasi

| Prioritas | Verifikasi minimum | Bukti yang dibutuhkan |
|---|---|---|
| **P0** | Sanity 0 bukan softlock; dinamo/altar/lentera gratis; tiga Blitz inti; CP7 rolling; ending lock atomik | Uji runtime + fault-injection |
| **P0** | Reuse `CampaignState`, handoff `PHASE4_CP8`, empat segel dari save | Test migrasi save nyata |
| **P1** | UV/benang terbaca tanpa warna tunggal | Screenshot/capture real build |
| **P1** | Chat tidak mengambil prioritas input; viewport OTS/FP | Test fokus, 30/60 FPS |
| **P1** | Replika tidak spawn di anchor; sektor tidak diulang setelah clear | Test navmesh/LOS |
| **P2** | VO, gamelan, aset berlisensi, flash aksesibilitas | Audit aset + review audio/pacing |

### 14.3 Format laporan yang diwajibkan sebelum freeze

`Audit/phase5-feasibility.md`: satu baris per subsystem/fungsi **READY / ADAPT / BUILD / BLOCKED**, bukti path dan symbol, mismatch dengan file ini, rekomendasi, risiko P0/P1/P2. `Audit/phase5-qa-matrix.md`: 180 ID identik §10 dengan status awal `NOT_RUN`. `Audit/phase5-implementation.md`: commit, build, tes yang benar-benar dijalankan, keterbatasan Linux/headless, aset, tangkapan visual, FPS Windows. Bila kanon berubah karena engine, tandai `[REVISI]` beserta alasan dan alternatif tanpa menghilangkan karakter horornya.

### 14.4 Persetujuan desain dan status akhir

**Dokumen ini lengkap sebagai rancangan produksi**, belum berarti kualitas gameplay terbukti. Audit kelayakan repository awal sudah dicatat di `Audit/phase5-feasibility.md`; keputusan teknis mengikuti baseline dan §0.11 berdasarkan instruksi implementasi pengguna. Dokumentasikan perubahan. Jangan membangun ulang sistem P1–P4, jangan menyatakan QA PASS sebelum diuji, dan jangan memperpanjang game dengan fetch quest hanya demi mengejar 75 menit.

---

## Lampiran A — Kriteria selesai yang diwarisi naskah Phase5 asli

1. Semua jalur wajib selesai dengan perlengkapan tetap, **tanpa membeli item opsional**.
2. Tiga bukti unik tersimpan dan dapat dibaca setelah reload; foto duplikat tidak menaikkan hitungan.
3. Arena terkunci hanya setelah Jurnal dan syarat babak lengkap; keluar dari inspeksi tidak memulai boss.
4. Gagal Blitz saat perisai terbuka mengulang kesempatan cahaya; tidak menghabiskan seluruh arena.
5. `SECRET_P5_PITA_PERTAMA` dapat diperoleh, disimpan sekali, dan tetap ada setelah mati/reload.
6. Sebelum transisi, status collectible babak terlihat; petunjuk tetap tersedia jika chat terlewat.
7. Save akhir memuat segel lokasi, upgrade, inventori, uang tersedia, Sanity, dan Arsip Saksi secara konsisten.

**Tambahan invarian desain:** tiga inti dan tiga Blitz inti; tiga ending dasar selalu tersedia; Secret hanya dengan lima ID unik aktual; Sanity 0 tidak mematikan otomatis; Kapasitor Ritual terpisah dari baterai persisten; satu enum ending dengan kunci atomik; Replika Memori bukan boss.
