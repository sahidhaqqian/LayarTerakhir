# LAYAR TERAKHIR — NASKAH INDUK REKAMAN VO

**Untuk:** rekaman performa pengguna → speech-to-speech per karakter → penyambungan audio ke game.
**Versi:** 26 Oktober 2026. **Basis repository:** `550ed38` (campaign sampai Phase 5).
**Cakupan:** Prolog, Phase 1–5, dialog opsional/percabangan, panggilan telepon, handycam, EVP, dan keempat ending. **SPOILER SELURUH GAME.**

Dokumen ini menjadi satu pegangan rekaman. Teks ucapan diambil dari implementasi dan scene build sekarang. `Phase1.MD`–`Phase5.MD` serta `Rancangan.MD` dipakai untuk memahami konteks; versi dialog rancangan yang berbeda atau belum dipasang tidak diganti diam-diam menjadi dialog aktif. Tidak ada perubahan kode, scene, teks game, atau kanon pada pekerjaan ini.

## 1. Cara merekam dan memakai ID

1. Mulai dari **Bagian 4: Dialog untuk direkam**. Baca hanya blok **Ucapkan**. Nama karakter, judul, ID, label EVP, dan instruksi dalam metadata tidak dibaca.
2. Kamu boleh memainkan semua peran dengan suaramu sendiri sebagai sumber STS. Utamakan emosi, ritme, jeda, dan napas sesuai adegan. Tidak perlu memaksakan pitch untuk meniru semua karakter; target suara dipilih saat konversi.
3. Rekam **satu cue per file**, WAV mono PCM 48 kHz, 24-bit atau 16-bit. Sisakan sekitar 0,3–0,5 detik sebelum/sesudah ucapan, pertahankan ujung kata dan napas akting. Simpan rekaman tanpa musik, reverb, efek telepon, radio, glitch, atau pitch shift. Hindari clipping.
4. Nama utama: `LTVO_<ID>_T01_RAW.wav`; take kedua `LTVO_<ID>_T02_RAW.wav`. Contoh: `LTVO_eb6caaf03e0d_T01_RAW.wav`. Jangan menyebut ID di dalam audio.
5. Sesudah speech-to-speech, simpan sebagai `LTVO_<ID>_T01_STS.wav` di folder per karakter, dengan take RAW tetap disimpan. ID tetap sama supaya raw, hasil konversi, dan teks runtime dapat dicocokkan. Baris gabungan Ibu/Damar memiliki dua nama file khusus, jangan diproses menjadi satu suara.
6. Gunakan target suara yang sama untuk Raka di seluruh babak, Damar di telepon/handycam/ending, dan masing-masing karakter lain. Efek jarak, handycam, radio, EVP, serta lapisan supernatural dibuat setelah STS; jangan dibakar ke rekaman sumber.
7. Rekam semua cabang yang tercantum, walaupun satu playthrough hanya mendengar salah satunya. Empat ending direkam terpisah. Urutan ini untuk sesi rekaman, bukan jaminan semua dialog terjadi berurutan di satu playthrough.
8. Ketika sudah ada hasil konversi, audio masih perlu dicocokkan ke profil/bank VO, pemicu, dan durasi subtitle di Unity. Mengonversi atau mengganti nama WAV saja belum otomatis memasangnya ke game. Sebagian dialog sekarang memakai placeholder, sebagian baru subtitle.

Tanda `...` berarti jeda/ragu; `—` berarti terpotong; kapital bisa berarti penekanan. Jangan mengucapkan tanda baca atau menambah kalimat/improvisasi. Petunjuk gerak dipisahkan dari blok ucapan. Jika ucapan lebih panjang dari waktu subtitle lama, utamakan akting alami; timing integrasi akan disesuaikan nanti.

ID memakai 12 digit hash dari **label speaker runtime + teks asli runtime**, sama dengan aturan ID daftar rekaman lama. Pembersihan petunjuk adegan untuk aktor tidak mengubah kunci runtime; teks asli yang berbeda tetap dicantumkan agar audio dapat diikat dengan benar.

## 2. Peta peran dan konsistensi suara

| Target suara | Karakter/perlakuan | Arahan utama |
|---|---|---|
| Raka | Protagonis, monolog, bicara ke kamera | Sehari-hari, gugup ditahan; makin lelah setelah tiap babak. Sopan saat bicara ke Ibu/ibu kos/Bu Sri. |
| Damar | Telepon, jeritan lantai atas, handycam, Pasar Setan, ending | Warna sama sepanjang campaign; emosi berubah dari panik, putus asa, hingga lega. |
| Ibu | Ibu Raka | Lemah tetapi hangat; tidak dibuat seperti hantu. |
| Bu Haji Ratna | Pemilik kos | Tegas dan manusiawi, mengantuk pada telepon malam. |
| Bu Sri | Penjual warung | Ramah, tahu lingkungan, memperingatkan dengan sungguh-sungguh. **Berbeda dari Nyonya Sri Atmaja.** |
| Nyonya Sri Atmaja | Arwah Rumah Atmaja | Jeritan marah dan getir; performa vokal, bukan kalimat deskriptif. |
| Mbah Suro | Kaset dan arwah; jangan disamakan dengan Penonton_Terakhir | Tua, tenang, ritual. Tawa terpisah dari efek bambu. |
| Penonton_Terakhir | Antagonis/entitas | Dingin, menguasai keadaan, menikmati ancaman; konsisten sampai ending. |
| Bisikan sumur | Identitas tidak diberi nama dalam panggilan subtitle | Lemah dan haus; jangan otomatis menamai atau menyamakannya dengan karakter lain. |
| DAMAR_TIRUAN / IBU_TIRUAN | Ilusi meniru karakter yang dikenal | Target STS mengikuti Damar/Ibu; tidak langsung terdengar monster. Pemalsuan dikenali lewat mekanik/konteks. |
| EVP_P1–P5 | Label kerja untuk sumber EVP tiap lokasi, bukan tokoh baru | Serpihan kesaksian. Nama korban tidak dijadikan identitas pembicara bila sumber tidak menetapkannya. |
| SAKSI | Arsip Saksi / kesaksian ritual | Target pemeran belum ditetapkan; simpan take netral. Jangan otomatis menambah narator Raka. |

## 3. Ringkasan dan status rekaman

| Jenis | Jumlah cue unik | Tindakan |
|---|---:|---|
| Dialog/ucapan aktif | 141 | Rekam Bagian 4; satu cue gabungan memiliki dua bagian suara. |
| Akting nonverbal | 4 | Rekam Bagian 5; jangan baca deskripsinya. |
| Materi tertulis/cadangan | 5 | Bagian 6; bukan playback suara aktif. Satu di antaranya adalah teks perekam Damar. |
| Label suara tanpa kalimat lengkap | 4 | Bagian 7; jangan mengarang atau membaca label. |
| Subtitle deskriptif/UI/SFX/kartu ending | 26 | Bagian 8; tidak perlu VO. |

Semua kotak rekaman masih kosong. Dokumen ini tidak menyatakan VO manusia atau integrasi audio sudah selesai. Cue yang sama dipakai ulang tidak perlu direkam dua kali; sumber/pemicu tambahannya tetap dicantumkan.

## 4. Dialog untuk direkam

Lompat ke: [Prolog](#vo-phase-0) · [Phase 1](#vo-phase-1) · [Phase 2](#vo-phase-2) · [Phase 3](#vo-phase-3) · [Phase 4](#vo-phase-4) · [Phase 5](#vo-phase-5)

<a id="vo-phase-0"></a>

### 4.0 Prolog — Kos, Kampung Melati, dan Rumah Atmaja

#### LTVO_3ab85f0b17d4 — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Tiga puluh dua penonton... Itu pun paling setengahnya akun bot. Kalau malam ini nggak ada konten yang meledak, tamat riwayat gue di Jakarta.

**File:** `LTVO_3ab85f0b17d4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lesu, frustrasi, bicara pada diri sendiri. Ada jeda lelah setelah jumlah penonton; tidak seperti membaca laporan.

**Sumber:** [KamarKosPrologDirector.cs:140](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L140).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_6572bc949fde — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Tiga setengah juta besok pagi... Gue cari ke mana jam segini?!

**File:** `LTVO_6572bc949fde_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Kaget dan terdesak biaya kos. Naikkan tekanan pada “besok pagi”, lalu jatuh ke kebingungan.

**Sumber:** [KamarKosPrologDirector.cs:275](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L275).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_879097b81a2b — Damar

**Target suara:** Damar. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Ka... Raka, jangan ke Rumah Atmaja sendirian. Tolong dengerin gue. Aplikasi kamera yang gue oprek dari darkweb kemarin... itu bukan filter AR biasa, Ka. Rumah ini nggak kosong. Di lantai dua ada suara perempuan nyanyi tembang Jawa, tapi pas gue sorot pakai kamera HP, dia udah berdiri tepat di depan muka gue—

**File:** `LTVO_879097b81a2b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Panik meningkat selama telepon; berusaha memberi informasi sebelum suara terputus. Bagian akhir terpotong mendadak.

**Sumber:** [KamarKosPrologDirector.cs:319](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L319).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_6a84521fd6b2 — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Dam?! Halo, Damar?! Sial, nomornya nggak aktif!

**File:** `LTVO_6a84521fd6b2_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [KamarKosPrologDirector.cs:323](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L323).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_718ee3924895 — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Anjir, Damar dalam bahaya! Duit ini nggak gratis... Untuk sementara gue harus ikutin perintah dari pesan ini.

**File:** `LTVO_718ee3924895_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [KamarKosPrologDirector.cs:346](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L346).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_c7e0840e0bfc — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Gue cek dulu apa yang diselipin di bawah pintu tadi.

**File:** `LTVO_c7e0840e0bfc_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [KamarKosPrologDirector.cs:371](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L371).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_7d9ac5dd3ee9 — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Kebuka. Powerbank, dompet, sama buku harian gue masih di sini.

**File:** `LTVO_7d9ac5dd3ee9_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [KamarKosPrologDirector.cs:396](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L396).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_6176c760a229 — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Bentar, gue belum mau keluar sebelum cek bawah pintu.

**File:** `LTVO_6176c760a229_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [KamarKosPrologDirector.cs:409](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L409).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_afdb5dcbcb2a — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Jaket udah dipakai. Hujan di luar makin deres.

**File:** `LTVO_afdb5dcbcb2a_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [KamarKosPrologDirector.cs:426](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L426).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_73a707960b7b — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — tekanan biaya, panggilan Damar, reaksi kontrak, dan persiapan berangkat.

**Ucapkan:**

> Gue belum siap berangkat.

**File:** `LTVO_73a707960b7b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [KamarKosPrologDirector.cs:452](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L452).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_5d62d26bc7ad — Damar

**Target suara:** Damar. **Konteks:** Akhir panggilan Damar sebelum sambungan putus.

**Ucapkan:**

> Raka! Tolong! Aaaah!

**File:** `LTVO_5d62d26bc7ad_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Takut dan mendesak mencari pertolongan. Rekam dekat dan bersih; jarak, telepon, serta gema dibuat setelah konversi.

**Sumber:** [KosSceneAudio.cs:108](Assets/_LayarTerakhir/src/Audio/KosSceneAudio.cs#L108).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_2c909749f66d — Raka

**Target suara:** Raka. **Konteks:** Telepon dan transfer setelah kontrak diterima.

**Ucapkan:**

> Udah terkirim, Bu. Semoga besok Ibu bisa ditangani.

**File:** `LTVO_2c909749f66d_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lembut dan hangat kepada Ibu; berusaha menyembunyikan ketakutan agar ia tidak khawatir.

**Cabang runtime:** hospital = true.

**Sumber:** [PhoneDesktop.cs:207](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L207).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b5a674212b51 — Raka

**Target suara:** Raka. **Konteks:** Telepon dan transfer setelah kontrak diterima.

**Ucapkan:**

> Lunas, Bu Haji. Setidaknya besok barang gue nggak dikeluarin.

**File:** `LTVO_b5a674212b51_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sopan, sungkan, menahan malu soal tunggakan. Lebih hati-hati daripada gaya bicara dengan Damar.

**Cabang runtime:** hospital = false.

**Sumber:** [PhoneDesktop.cs:207](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L207).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_7cd0b72a8055 — Ibu

**Target suara:** Ibu. **Konteks:** Panggilan Ibu.

**Ucapkan:**

> Nak... ada apa nelepon Ibu malam-malam? Ibu belum tidur, tadi batuknya kambuh lagi.

**File:** `LTVO_7cd0b72a8055_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Hangat, lelah karena sakit, napas sedikit pendek; rasa sayang lebih utama daripada efek suara sakit.

**Sumber:** [PhoneDesktop.cs:260](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L260).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_52a5d92022b4 — Raka

**Target suara:** Raka. **Konteks:** Panggilan Ibu.

**Ucapkan:**

> Bu! Raka dapet uang buat biaya rumah sakit Ibu! Besok Ibu bisa lanjut diperiksa. Raka transfer malam ini, ya.

**File:** `LTVO_52a5d92022b4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lega dan bersemangat memberi kabar baik kepada Ibu, tetapi kelelahan masih terdengar.

**Sumber:** [PhoneDesktop.cs:261](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L261).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_38362f5e3831 — Ibu

**Target suara:** Ibu. **Konteks:** Panggilan Ibu.

**Ucapkan:**

> Alhamdulillah... makasih, Nak. Tapi kamu jangan memaksakan diri. Kerjanya yang aman, Ibu cuma pengin kamu pulang sehat.

**File:** `LTVO_38362f5e3831_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Hangat, lelah karena sakit, napas sedikit pendek; rasa sayang lebih utama daripada efek suara sakit.

**Sumber:** [PhoneDesktop.cs:262](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L262).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_de8e8a0796e0 — Raka

**Target suara:** Raka. **Konteks:** Panggilan Ibu.

**Ucapkan:**

> Iya, Bu. Doain Raka. Sekarang Ibu istirahat dulu, nanti Raka kabarin lagi.

**File:** `LTVO_de8e8a0796e0_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lembut dan hangat kepada Ibu; berusaha menyembunyikan ketakutan agar ia tidak khawatir.

**Sumber:** [PhoneDesktop.cs:263](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L263).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_074fbe9b9129 — Bu Haji Ratna

**Target suara:** Bu Haji Ratna. **Konteks:** Panggilan Bu Haji Ratna.

**Ucapkan:**

> Halo, Raka? Ada apa malam-malam? Soal kamar itu besok pagi Ibu sudah harus kasih kepastian.

**File:** `LTVO_074fbe9b9129_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Mengantuk, tegas, wajar sebagai pemilik kos; pada jawaban terakhir ada kelegaan. Tidak menyeramkan.

**Sumber:** [PhoneDesktop.cs:264](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L264).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_50cde98f67d4 — Raka

**Target suara:** Raka. **Konteks:** Panggilan Bu Haji Ratna.

**Ucapkan:**

> Iya, Bu Haji. Maaf ganggu. Uangnya sudah ada, tiga setengah juta. Saya bayar lewat transfer malam ini. Tolong jangan keluarin barang saya besok.

**File:** `LTVO_50cde98f67d4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sopan, sungkan, menahan malu soal tunggakan. Lebih hati-hati daripada gaya bicara dengan Damar.

**Sumber:** [PhoneDesktop.cs:265](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L265).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b356d667304c — Bu Haji Ratna

**Target suara:** Bu Haji Ratna. **Konteks:** Panggilan Bu Haji Ratna.

**Ucapkan:**

> Ya sudah, Raka. Kalau masuk malam ini, kamarmu tetap aman. Habis transfer simpan buktinya. Lain kali jangan menunggu sampai ditegur, ya.

**File:** `LTVO_b356d667304c_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Mengantuk, tegas, wajar sebagai pemilik kos; pada jawaban terakhir ada kelegaan. Tidak menyeramkan.

**Sumber:** [PhoneDesktop.cs:266](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L266).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_a4fbd20c7f2c — Raka

**Target suara:** Raka. **Konteks:** Panggilan Bu Haji Ratna.

**Ucapkan:**

> Iya, Bu. Terima kasih sudah ngasih waktu. Saya transfer sekarang.

**File:** `LTVO_a4fbd20c7f2c_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sopan, sungkan, menahan malu soal tunggakan. Lebih hati-hati daripada gaya bicara dengan Damar.

**Sumber:** [PhoneDesktop.cs:267](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L267).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_afdc60ae6a2a — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk kamar mandi perempuan kos.

**Ucapkan:**

> Handuknya masih lembap. Pasti ada yang habis mandi tadi.

**File:** `LTVO_afdc60ae6a2a_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:35](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L35).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_c2a88f132059 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk kamar mandi laki-laki kos.

**Ucapkan:**

> Keran baknya netes lagi. Anak-anak kos belum ada yang benerin.

**File:** `LTVO_c2a88f132059_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:36](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L36).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_0eba0fb01af5 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk dapur bersama.

**Ucapkan:**

> Masih ada bekas masak tadi sore. Biasanya jam segini ada yang bikin kopi.

**File:** `LTVO_0eba0fb01af5_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:37](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L37).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_258cc1691f73 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk meja makan bersama.

**Ucapkan:**

> Meja makan kosong. Yang lain pasti udah tidur.

**File:** `LTVO_258cc1691f73_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:38](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L38).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_28369badf58b — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk kamar mandi kos.

**Ucapkan:**

> Kerannya belum rapat. Nanti gue ingetin anak-anak kos.

**File:** `LTVO_28369badf58b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:39](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L39).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_97e218f642d8 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk koridor lantai dasar kos.

**Ucapkan:**

> Bu Haji kayaknya udah masuk kamar. Jangan berisik sampai bangunin beliau.

**File:** `LTVO_97e218f642d8_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sopan, sungkan, menahan malu soal tunggakan. Lebih hati-hati daripada gaya bicara dengan Damar.

**Sumber:** [ExplorationCommentary.cs:40](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L40).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_cd5bb9657fa4 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk koridor lantai dua kos.

**Ucapkan:**

> Semua pintu udah ketutup. Cuma gue yang masih keluyuran jam segini.

**File:** `LTVO_cd5bb9657fa4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:41](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L41).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_8c1b74a653d4 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk halaman kos dan motor.

**Ucapkan:**

> Motor masih basah. Dam, tunggu gue...

**File:** `LTVO_8c1b74a653d4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:42](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L42).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_2c7570216df1 — Bu Sri

**Target suara:** Bu Sri. **Konteks:** Warung Bu Sri — bertanya tentang Damar dan arah Rumah Atmaja.

**Ucapkan:**

> Malam, Mas Raka. Hujan begini masih mau keluar?

**File:** `LTVO_2c7570216df1_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Ramah, tahu lingkungan sekitar, lalu lebih pelan saat memperingatkan rumah Atmaja. Tetap penjual warung biasa.

**Sumber:** [MelatiShop.cs:33](Assets/_LayarTerakhir/src/Prolog/MelatiShop.cs#L33).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_d968832a3ecf — Raka

**Target suara:** Raka. **Konteks:** Warung Bu Sri — bertanya tentang Damar dan arah Rumah Atmaja.

**Ucapkan:**

> Bu, teman saya belum pulang dari rumah Atmaja. Ibu lihat dia lewat?

**File:** `LTVO_d968832a3ecf_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sopan kepada penjual warung, cemas mencari teman; bukan intonasi streamer di depan penonton.

**Sumber:** [MelatiShop.cs:39](Assets/_LayarTerakhir/src/Prolog/MelatiShop.cs#L39).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_e5f037768730 — Bu Sri

**Target suara:** Bu Sri. **Konteks:** Warung Bu Sri — bertanya tentang Damar dan arah Rumah Atmaja.

**Ucapkan:**

> Yang bawa kamera? Sore tadi lewat sini. Saya sudah bilang, rumah itu lama kosong.

**File:** `LTVO_e5f037768730_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Ramah, tahu lingkungan sekitar, lalu lebih pelan saat memperingatkan rumah Atmaja. Tetap penjual warung biasa.

**Sumber:** [MelatiShop.cs:40](Assets/_LayarTerakhir/src/Prolog/MelatiShop.cs#L40).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_1551325d86f5 — Bu Sri

**Target suara:** Bu Sri. **Konteks:** Warung Bu Sri — bertanya tentang Damar dan arah Rumah Atmaja.

**Ucapkan:**

> Lurus sampai ujung jalan, lalu belok kanan. Kalau dengar orang manggil dari dalam, jangan jawab sembarangan, Mas.

**File:** `LTVO_1551325d86f5_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Ramah, tahu lingkungan sekitar, lalu lebih pelan saat memperingatkan rumah Atmaja. Tetap penjual warung biasa.

**Sumber:** [MelatiShop.cs:41](Assets/_LayarTerakhir/src/Prolog/MelatiShop.cs#L41).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_eb6caaf03e0d — Bisikan

**Target suara:** Bisikan. **Konteks:** Rumah Atmaja — masuk, memeriksa jejak Damar, dan menyalakan siaran; Rumah Atmaja — panggilan dari lantai atas, kursi ruang makan, dan ancaman mendadak.

**Ucapkan:**

> Haus... bukakan...

**File:** `LTVO_eb6caaf03e0d_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lemah, haus, berbisik dekat; artikulasi tetap terbaca. Gema sumur ditambahkan setelah konversi.

**Sumber:** [AtmajaDirector.cs:135](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L135); [AtmajaReviewMoments.cs:45](Assets/_LayarTerakhir/src/Prolog/AtmajaReviewMoments.cs#L45).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_c1df48558932 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — masuk, memeriksa jejak Damar, dan menyalakan siaran.

**Ucapkan:**

> Tulisan Damar... Dia benar-benar udah masuk ke sini.

**File:** `LTVO_c1df48558932_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaDirector.cs:145](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L145).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_a95b9780c476 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — masuk, memeriksa jejak Damar, dan menyalakan siaran.

**Ucapkan:**

> Oke... tes, satu dua. Malam semuanya, balik lagi di Malam Keramat. Malam ini gue nggak sendirian... maksudnya, gue lagi nyusulin Damar yang hilang kontak di Rumah Tua Atmaja. Buat yang baru gabung, bantu pantau layar ya.

**File:** `LTVO_a95b9780c476_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bicara kepada penonton, berusaha terdengar mantap. Ada gugup di sela napas; bukan membacakan tutorial.

**Sumber:** [AtmajaDirector.cs:175](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L175).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_2db94540f6b9 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — masuk, memeriksa jejak Damar, dan menyalakan siaran.

**Ucapkan:**

> Anjir! Anginnya kenceng banget... Eh bentar, ini jendelanya kok nyangkut?!

**File:** `LTVO_2db94540f6b9_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaDirector.cs:198](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L198).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_2d979a7efad0 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — masuk, memeriksa jejak Damar, dan menyalakan siaran.

**Ucapkan:**

> Hmm... kok tiba-tiba ketutup, ya? Kayaknya kita cuma bisa lanjut.

**File:** `LTVO_2d979a7efad0_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaDirector.cs:199](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L199).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_0513b9f29bd5 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — masuk, memeriksa jejak Damar, dan menyalakan siaran.

**Ucapkan:**

> Nggak bisa... ada rantai dari dalam. Dam? Damar! Gue harus cari jalan masuk lain.

**File:** `LTVO_0513b9f29bd5_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaDirector.cs:216](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L216).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_ddccbede0078 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — masuk, memeriksa jejak Damar, dan menyalakan siaran.

**Ucapkan:**

> Ini HP-nya Damar... Layarnya pecah, tapi kartu memorinya masih kebaca. Gue pindahin dulu aplikasinya ke HP gue.

**File:** `LTVO_ddccbede0078_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaDirector.cs:232](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L232).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_276fbd266957 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk halaman dan motor Damar.

**Ucapkan:**

> Motornya Damar. Tapi kok rumahnya sepi banget?

**File:** `LTVO_276fbd266957_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:31](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L31).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_45af2fbb922b — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — panggilan dari lantai atas, kursi ruang makan, dan ancaman mendadak.

**Ucapkan:**

> Damar! Damar!

**File:** `LTVO_45af2fbb922b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Mendesak, spontan, napas lebih cepat. Bedakan setiap panggilan; jangan mengulang melodi yang sama.

**Sumber:** [AtmajaReviewMoments.cs:51](Assets/_LayarTerakhir/src/Prolog/AtmajaReviewMoments.cs#L51).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_5e1e698496f4 — Damar • dari lantai atas

**Target suara:** Damar. **Konteks:** Rumah Atmaja — panggilan dari lantai atas, kursi ruang makan, dan ancaman mendadak.

**Ucapkan:**

> AAARGH! Raka...!

**File:** `LTVO_5e1e698496f4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Takut dan mendesak mencari pertolongan. Rekam dekat dan bersih; jarak, telepon, serta gema dibuat setelah konversi.

**Sumber:** [AtmajaReviewMoments.cs:53](Assets/_LayarTerakhir/src/Prolog/AtmajaReviewMoments.cs#L53).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_f74a9e8171b4 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — panggilan dari lantai atas, kursi ruang makan, dan ancaman mendadak.

**Ucapkan:**

> Dam?! Suaranya dari atas...

**File:** `LTVO_f74a9e8171b4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaReviewMoments.cs:56](Assets/_LayarTerakhir/src/Prolog/AtmajaReviewMoments.cs#L56).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_7c592ff0be20 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — panggilan dari lantai atas, kursi ruang makan, dan ancaman mendadak.

**Ucapkan:**

> Siapa... yang duduk di situ?

**File:** `LTVO_7c592ff0be20_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Kaget membeku, suara mengecil dan terputus; jangan selalu menyelesaikan takut dengan berteriak.

**Sumber:** [AtmajaReviewMoments.cs:119](Assets/_LayarTerakhir/src/Prolog/AtmajaReviewMoments.cs#L119).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_25e07cb143e2 — Raka

**Target suara:** Raka. **Konteks:** Rumah Atmaja — panggilan dari lantai atas, kursi ruang makan, dan ancaman mendadak.

**Ucapkan:**

> Apa...?! Lepas!

**File:** `LTVO_25e07cb143e2_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Mendesak, spontan, napas lebih cepat. Bedakan setiap panggilan; jangan mengulang melodi yang sama.

**Sumber:** [AtmajaReviewMoments.cs:139](Assets/_LayarTerakhir/src/Prolog/AtmajaReviewMoments.cs#L139).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_d62518296491 — Raka

**Target suara:** Raka. **Konteks:** Lantai dua Rumah Atmaja — tangga, kamar anak, kamar mandi, dan akses ruang bawah tanah.

**Ucapkan:**

> Jejak tangan... berdarah semua. Ya Tuhan...

**File:** `LTVO_d62518296491_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaUpperDirector.cs:222](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L222).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_e7091db85cde — Raka

**Target suara:** Raka. **Konteks:** Lantai dua Rumah Atmaja — tangga, kamar anak, kamar mandi, dan akses ruang bawah tanah.

**Ucapkan:**

> Di... di belakang gue... Nggak ada siapa-siapa?!

**File:** `LTVO_e7091db85cde_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Kaget membeku, suara mengecil dan terputus; jangan selalu menyelesaikan takut dengan berteriak.

**Sumber:** [AtmajaUpperDirector.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L240).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b385a3db5606 — Raka

**Target suara:** Raka. **Konteks:** Lantai dua Rumah Atmaja — tangga, kamar anak, kamar mandi, dan akses ruang bawah tanah.

**Ucapkan:**

> Dia... tergantung di sana...

**File:** `LTVO_b385a3db5606_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Kaget membeku, suara mengecil dan terputus; jangan selalu menyelesaikan takut dengan berteriak.

**Sumber:** [AtmajaUpperDirector.cs:312](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L312).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_77c0362bd601 — Raka

**Target suara:** Raka. **Konteks:** Lantai dua Rumah Atmaja — tangga, kamar anak, kamar mandi, dan akses ruang bawah tanah.

**Ucapkan:**

> Airnya... merah kecokelatan. Kayak karat... atau darah.

**File:** `LTVO_77c0362bd601_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaUpperDirector.cs:340](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L340).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_08389c386cd1 — Raka

**Target suara:** Raka. **Konteks:** Lantai dua Rumah Atmaja — tangga, kamar anak, kamar mandi, dan akses ruang bawah tanah.

**Ucapkan:**

> Karpetnya digulung... ada pintu jebakan kayu di bawahnya.

**File:** `LTVO_08389c386cd1_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaUpperDirector.cs:354](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L354).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_8ce98c6436a0 — Raka

**Target suara:** Raka. **Konteks:** Lantai dua Rumah Atmaja — tangga, kamar anak, kamar mandi, dan akses ruang bawah tanah.

**Ucapkan:**

> Tangga batu... turun ke bawah tanah. Dingin banget hawanya.

**File:** `LTVO_8ce98c6436a0_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaUpperDirector.cs:359](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L359).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b4a4b29dadeb — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk kamar anak.

**Ucapkan:**

> Mainan sama bukunya masih ditinggal begitu aja. Anak ini waktu itu sempat minta tolong?

**File:** `LTVO_b4a4b29dadeb_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:19](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L19).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_d7821406fee5 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk kamar mandi lantai dua.

**Ucapkan:**

> Lantai basah... padahal kerannya mati. Gue nggak suka ruangan ini.

**File:** `LTVO_d7821406fee5_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:20](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L20).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_5a487c568333 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk kamar utama.

**Ucapkan:**

> Bau kain tua di sini makin kuat. Damar sempat sampai kamar ini nggak, ya?

**File:** `LTVO_5a487c568333_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:21](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L21).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_881f3fb07a54 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk balkon.

**Ucapkan:**

> Anginnya kencang, tapi yang bergerak cuma tali itu...

**File:** `LTVO_881f3fb07a54_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:22](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L22).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_a5673b09c151 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk lorong lantai dua.

**Ucapkan:**

> Jejak tangan ini masih kelihatan merah. Jangan-jangan Damar baru lewat sini.

**File:** `LTVO_a5673b09c151_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:23](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L23).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_3177de7ef50b — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk ruang bawah tanah.

**Ucapkan:**

> Ada suara dari arah meja-meja itu. Pelan-pelan, Ka... jangan bikin bunyi.

**File:** `LTVO_3177de7ef50b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:24](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L24).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_1e96f117de98 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk lemari penyimpanan.

**Ucapkan:**

> Debunya tebal... tapi ada bekas tangan baru di lemari ini.

**File:** `LTVO_1e96f117de98_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:25](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L25).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_d7e4f17c6b43 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk dapur.

**Ucapkan:**

> Bau air sumur sama kayu basah. Dam, lo masuk lewat sini juga?

**File:** `LTVO_d7e4f17c6b43_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:26](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L26).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_0ef7252493b3 — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk ruang makan.

**Ucapkan:**

> Damar? Ini Raka. Kalau lo denger, jawab gue...

**File:** `LTVO_0ef7252493b3_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:27](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L27).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_cdbeb5ec8e3a — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk ruang duduk.

**Ucapkan:**

> Jamnya masih jalan. Siapa yang ngurus rumah kosong ini?

**File:** `LTVO_cdbeb5ec8e3a_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:28](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L28).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_083a466730ad — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk ruang depan.

**Ucapkan:**

> Dingin banget di ruangan ini. Kayak baru ada orang lewat.

**File:** `LTVO_083a466730ad_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:29](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L29).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_84e9a214f5db — Raka

**Target suara:** Raka. **Konteks:** Komentar saat masuk sisi timur rumah.

**Ucapkan:**

> Dindingnya lembap... Jangan sampai sinyal HP ikut hilang di sini.

**File:** `LTVO_84e9a214f5db_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [ExplorationCommentary.cs:30](Assets/_LayarTerakhir/src/Prolog/ExplorationCommentary.cs#L30).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_66ce91446504 — Raka

**Target suara:** Raka. **Konteks:** Respons pintu terkunci yang diisi saat runtime.

**Ucapkan:**

> Sial pintunya dikunci, aku harus cari jalan lain.

**File:** `LTVO_66ce91446504_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaUpperDirector.cs:166](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L166).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_285e455ab35c — Raka

**Target suara:** Raka. **Konteks:** Ruang bawah tanah Atmaja — daya HP dan pencarian perlengkapan Damar.

**Ucapkan:**

> 80 persen... Oke, masih bisa lanjut.

**File:** `LTVO_285e455ab35c_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaBasementHorror.cs:135](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L135).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_1170c63f2d8f — Raka

**Target suara:** Raka. **Konteks:** Ruang bawah tanah Atmaja — daya HP dan pencarian perlengkapan Damar.

**Ucapkan:**

> Sial, ada gangguan daya! Gue harus cari powerbank cadangan di tas peralatan Damar atau HP ini mati total!

**File:** `LTVO_1170c63f2d8f_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [AtmajaBasementHorror.cs:263](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L263).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_76321d5f9581 — Damar (rekaman)

**Target suara:** Damar. **Konteks:** Rekaman handycam ketika Damar ditarik masuk ke cermin.

**Ucapkan:**

> Kacanya... kacanya bukan mantulin ruangan ini, Ka! Di dalem kaca itu ada pasar malam... mereka narik kaki gue—RAKA TOLONGIN GUE!

**File:** `LTVO_76321d5f9581_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Terkesiap melihat pasar di cermin; napas pecah menjadi teriakan ketika ditarik. Efek handycam ditambahkan setelah STS.

**Sumber:** [AtmajaHandycamPlayback.cs:30](Assets/_LayarTerakhir/src/Prolog/AtmajaHandycamPlayback.cs#L30).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_98f21140f36e — Nyonya Sri Atmaja

**Target suara:** Nyonya Sri Atmaja. **Konteks:** Pengejaran Nyonya Sri Atmaja dan akhir prolog.

**Ucapkan:**

> AAAAAARGH!!!

**File:** `LTVO_98f21140f36e_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Jeritan vokal yang marah dan getir, bukan membaca huruf A satu per satu. Ambil take kuat yang pendek; hindari clipping.

**Sumber:** [AtmajaChaseSequence.cs:174](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L174).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_c51b3125b6f5 — Nyonya Sri Atmaja

**Target suara:** Nyonya Sri Atmaja. **Konteks:** Pengejaran Nyonya Sri Atmaja dan akhir prolog.

**Ucapkan:**

> AAAAAARGH...

**File:** `LTVO_c51b3125b6f5_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Jeritan vokal yang marah dan getir, bukan membaca huruf A satu per satu. Ambil take kuat yang pendek; hindari clipping.

**Sumber:** [AtmajaChaseSequence.cs:350](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L350).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_701f977f2966 — Raka

**Target suara:** Raka. **Konteks:** Pengejaran Nyonya Sri Atmaja dan akhir prolog.

**Ucapkan:**

> ...Gue... selamat... Damar... gue bakal balik buat lo...

**File:** `LTVO_701f977f2966_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Terengah setelah lari, lega sekaligus bersalah meninggalkan Damar. Napas bagian dari akting, bukan kata yang dibaca.

**Petunjuk adegan, tidak dibaca:** terengah-engah.

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "*terengah-engah* ...Gue... selamat... Damar... gue bakal balik buat lo...".

**Sumber:** [AtmajaChaseSequence.cs:463](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L463).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

<a id="vo-phase-1"></a>

### 4.1 Phase 1 — Rusunawa Griya Asri

#### LTVO_2d01635c0463 — Raka

**Target suara:** Raka. **Konteks:** Rusunawa — kedatangan, pembukaan siaran, dan penemuan jejak Damar.

**Ucapkan:**

> Kos udah lunas, deposit operasi Ibu udah masuk. Tapi Damar masih terjebak di balik cermin sialan itu... Empat segel harus gue kumpulin.

**File:** `LTVO_2d01635c0463_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Cabang runtime:** Campaign.HasInventoryItem("EVENT_PAID_RENT")&&Campaign.HasInventoryItem("EVENT_PAID_HOSPITAL") = true.

**Sumber:** [Phase1Director.cs:306](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L306).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_97e6ef5fb4cc — Raka

**Target suara:** Raka. **Konteks:** Rusunawa — kedatangan, pembukaan siaran, dan penemuan jejak Damar.

**Ucapkan:**

> Damar masih terjebak di balik cermin sialan itu... Empat segel harus gue kumpulin. Kalau gue mundur malam ini, Damar mati.

**File:** `LTVO_97e6ef5fb4cc_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Cabang runtime:** Campaign.HasInventoryItem("EVENT_PAID_RENT")&&Campaign.HasInventoryItem("EVENT_PAID_HOSPITAL") = false.

**Sumber:** [Phase1Director.cs:306](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L306).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_01b7136d715c — Raka

**Target suara:** Raka. **Konteks:** Rusunawa — kedatangan, pembukaan siaran, dan penemuan jejak Damar.

**Ucapkan:**

> Malam ini gue di Rusunawa Griya Asri. Gue cuma butuh mata kalian buat bantu gue selamat.

**File:** `LTVO_01b7136d715c_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bicara kepada penonton, berusaha terdengar mantap. Ada gugup di sela napas; bukan membacakan tutorial.

**Sumber:** [Phase1Director.cs:317](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L317).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_113c5c5426c8 — EVP

**Target suara:** EVP_P1. **Konteks:** Rusunawa — kedatangan, pembukaan siaran, dan penemuan jejak Damar.

**Ucapkan:**

> Pecahan... keris... di tungku dapur... bawa... ke altar kamar empat kosong empat...

**File:** `LTVO_113c5c5426c8_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bisikan rapuh, terpotong, memohon atau bersaksi. Rekam kalimatnya saja; ketukan dan gangguan elektronik bukan bagian ucapan.

**Sumber:** [Phase1Director.cs:144](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L144).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_e180cae2d658 — Raka

**Target suara:** Raka. **Konteks:** Rusunawa — kedatangan, pembukaan siaran, dan penemuan jejak Damar.

**Ucapkan:**

> Ini sobekan jaket yang dipakai Damar kemarin... Berarti cermin di Rumah Atmaja emang terhubung ke tempat-tempat ritual Mbah Suro.

**File:** `LTVO_e180cae2d658_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [Phase1Director.cs:249](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L249).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_df8c80c086f1 — Kaset Mbah Suro

**Target suara:** Mbah Suro. **Konteks:** Rusunawa — kaset ritual dan pesan sesudah segel pertama.

**Ucapkan:**

> Lima gerbang di lima penjuru kota. Rusunawa, rumah sakit, pabrik gula, Karang Setra, dan Pasar Setan tempat semuanya bermula. Empat segel dari empat penjuru akan membuka gerbang kelima bagi Penonton Terakhir...

**File:** `LTVO_df8c80c086f1_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Tua, mantap, dingin, ritme seperti pernyataan ritual. Rekam bersih tanpa filter kaset atau tambahan mantra.

**Sumber:** [Phase1Presentation.cs:191](Assets/_LayarTerakhir/src/Phase1/Phase1Presentation.cs#L191).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_9120125f5058 — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Rusunawa — kaset ritual dan pesan sesudah segel pertama.

**Ucapkan:**

> Segel pertama terkumpul, tiga segel lokasi tersisa. Kau mulai terbiasa menggunakan aplikasi yang dioprek sahabatmu itu, Raka. Besok malam pukul 23:59, bawa kameramu ke RSUD Lama Sido Waras. Jangan terlambat, karena suster jaga malam di sana sangat suka mengamputasi tamu yang telat.

**File:** `LTVO_9120125f5058_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase1Presentation.cs:201](Assets/_LayarTerakhir/src/Phase1/Phase1Presentation.cs#L201).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

<a id="vo-phase-2"></a>

### 4.2 Phase 2 — RSUD Sido Waras

#### LTVO_408f658ab6a5 — Raka

**Target suara:** Raka. **Konteks:** Siaran ekspedisi segel kedua di RSUD.

**Ucapkan:**

> Ekspedisi segel kedua. Gue udah di RSUD Sido Waras. Damar masih ditahan. Malam ini gue udah pasang sensor suhu sama kamera depan di HP.

**File:** `LTVO_408f658ab6a5_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bicara kepada penonton, berusaha terdengar mantap. Ada gugup di sela napas; bukan membacakan tutorial.

**Sumber:** [Phase2Presentation.cs:26](Assets/_LayarTerakhir/src/Phase2/Phase2Presentation.cs#L26).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_3b910a7cb9ec — Raka

**Target suara:** Raka. **Konteks:** Tiba di pintu IGD RSUD Sido Waras.

**Ucapkan:**

> RSUD Sido Waras. 23:50. Pintu IGD masih terbuka...

**File:** `LTVO_3b910a7cb9ec_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [Phase2Director.cs:71](Assets/_LayarTerakhir/src/Phase2/Phase2Director.cs#L71).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_faf6a3897a5b — Raka

**Target suara:** Raka. **Konteks:** Investigasi jejak Damar di rumah sakit.

**Ucapkan:**

> Gantungan kamera Damar... ada goresan menuju lantai atas. Terowongannya dirantai.

**File:** `LTVO_faf6a3897a5b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [Phase2Interactions.cs:63](Assets/_LayarTerakhir/src/Phase2/Phase2Interactions.cs#L63).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_54457886650e — EVP — Tidak Dikenal

**Target suara:** EVP_P2. **Konteks:** EVP dan jejak kejadian lama di bangsal.

**Ucapkan:**

> Kembalikan... anakku... di meja operasi...

**File:** `LTVO_54457886650e_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bisikan rapuh, terpotong, memohon atau bersaksi. Rekam kalimatnya saja; ketukan dan gangguan elektronik bukan bagian ucapan.

**Sumber:** [Phase2Investigation.cs:80](Assets/_LayarTerakhir/src/Phase2/Phase2Investigation.cs#L80).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_d5ed109ebcc2 — Raka

**Target suara:** Raka. **Konteks:** Meja operasi — mengembalikan pusaka sebelum ritual Lastri.

**Ucapkan:**

> Gunting... gelang... darah segel. Aku kembalikan semuanya di sini.

**File:** `LTVO_d5ed109ebcc2_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [Phase2Boss.cs:67](Assets/_LayarTerakhir/src/Phase2/Phase2Boss.cs#L67).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_aafaae59f4ca — Raka

**Target suara:** Raka. **Konteks:** Sesudah segel kedua — rekaman Damar dan ancaman untuk Pabrik Sindang Laut.

**Ucapkan:**

> Lastri... segel kedua sudah masuk. Damar, lu masih di sana?

**File:** `LTVO_aafaae59f4ca_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [Phase2Ending.cs:52](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L52).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_d78b5f704ffc — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel kedua — rekaman Damar dan ancaman untuk Pabrik Sindang Laut.

**Ucapkan:**

> Dua segel Panca-Sengkala telah kau buka, Raka

**File:** `LTVO_d78b5f704ffc_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase2Ending.cs:72](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L72).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_1263d11d9eca — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel kedua — rekaman Damar dan ancaman untuk Pabrik Sindang Laut.

**Ucapkan:**

> Lihat penontonmu—tiga ratus ribu pasang mata kini menatap layarmu tanpa berkedip

**File:** `LTVO_1263d11d9eca_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase2Ending.cs:72](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L72).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_02e90e47cb21 — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel kedua — rekaman Damar dan ancaman untuk Pabrik Sindang Laut.

**Ucapkan:**

> Semakin banyak yang menonton, semakin tipis batas antara duniamu dan Pasar Setan

**File:** `LTVO_02e90e47cb21_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase2Ending.cs:72](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L72).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_47e3378bcd39 — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel kedua — rekaman Damar dan ancaman untuk Pabrik Sindang Laut.

**Ucapkan:**

> Besok siang gunakan uangmu untuk membeli Lensa UV Rajah dan Flare sebanyak mungkin..

**File:** `LTVO_47e3378bcd39_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase2Ending.cs:72](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L72).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_f3983323bba2 — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel kedua — rekaman Damar dan ancaman untuk Pabrik Sindang Laut.

**Ucapkan:**

> karena besok malam pukul 23:59, tungku Pabrik Gula Belanda 'Sindang Laut' akan menyala untuk kalian berdua.

**File:** `LTVO_f3983323bba2_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase2Ending.cs:72](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L72).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

<a id="vo-phase-3"></a>

### 4.3 Phase 3 — Pabrik Gula Sindang Laut

#### LTVO_025e07147869 — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — mempersiapkan penyelidikan pabrik.

**Ucapkan:**

> Semalam di video Penonton_Terakhir, gue jelas-jelas liat Damar diikat di dekat tungku pabrik itu. Kalau penunggu di sana bisa niru suara manusia dan bikin ilusi... gue harus hati-hati bedain mana Damar yang asli dan mana jebakan.

**File:** `LTVO_025e07147869_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Sumber:** [Phase3Hub.cs:67](Assets/_LayarTerakhir/src/Phase3/Phase3Hub.cs#L67).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b33686cbe69c — Raka

**Target suara:** Raka. **Konteks:** Pembukaan siaran di Pabrik Gula Sindang Laut.

**Ucapkan:**

> Ekspedisi segel ketiga. Gue di Pabrik Gula Sindang Laut. Semalam kalian liat sendiri rekaman Damar diikat di ruang tungku. Bantu gue pantau kalau ada yang janggal.

**File:** `LTVO_b33686cbe69c_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bicara kepada penonton, berusaha terdengar mantap. Ada gugup di sela napas; bukan membacakan tutorial.

**Sumber:** [Phase3Presentation.cs:20](Assets/_LayarTerakhir/src/Phase3/Phase3Presentation.cs#L20).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_5ec1692d7f9e — Suara Tiruan — Damar

**Target suara:** DAMAR_TIRUAN. **Konteks:** Pabrik — suara Damar tiruan mengundang ke jalur berbahaya.

**Ucapkan:**

> RAKA! KA, GUE DI SINI! TOLONGIN GUE, TALI GUE MAU PUTUS KE BAWAH MESIN!

**File:** `LTVO_5ec1692d7f9e_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Tiru rasa panik Damar yang meyakinkan. Pertahankan target suara Damar saat STS; jangan langsung diberi suara monster sehingga puzzle terbocorkan.

**Sumber:** [Phase3Director.cs:130](Assets/_LayarTerakhir/src/Phase3/Phase3Director.cs#L130).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_71988df8e0c8 — EVP — ASLI

**Target suara:** EVP_P3. **Konteks:** Pabrik — EVP ventilasi dan interkom.

**Ucapkan:**

> Nomor sembilan tidak pulang.

**File:** `LTVO_71988df8e0c8_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bisikan rapuh, terpotong, memohon atau bersaksi. Rekam kalimatnya saja; ketukan dan gangguan elektronik bukan bagian ucapan.

**Petunjuk adegan, tidak dibaca:** Tiga ketukan dari ventilasi..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Tiga ketukan dari ventilasi.] Nomor sembilan tidak pulang.".

**Sumber:** [Phase3Investigation.cs:47](Assets/_LayarTerakhir/src/Phase3/Phase3Investigation.cs#L47).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_cc938c47e5a0 — EVP — ASLI

**Target suara:** EVP_P3. **Konteks:** Pabrik — EVP ventilasi dan interkom.

**Ucapkan:**

> ...pulang... jangan tutup...

**File:** `LTVO_cc938c47e5a0_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bisikan rapuh, terpotong, memohon atau bersaksi. Rekam kalimatnya saja; ketukan dan gangguan elektronik bukan bagian ucapan.

**Sumber:** [Phase3Investigation.cs:49](Assets/_LayarTerakhir/src/Phase3/Phase3Investigation.cs#L49).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_de63a9fa60bb — Suara Tiruan — Damar

**Target suara:** DAMAR_TIRUAN. **Konteks:** Pabrik — Wewe Gombel meniru panggilan Damar.

**Ucapkan:**

> Ka... sini. Gue di balik ketel...

**File:** `LTVO_de63a9fa60bb_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Tiru rasa panik Damar yang meyakinkan. Pertahankan target suara Damar saat STS; jangan langsung diberi suara monster sehingga puzzle terbocorkan.

**Sumber:** [Phase3Wewe.cs:160](Assets/_LayarTerakhir/src/Phase3/Phase3Wewe.cs#L160).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_6edb24c20a01 — Raka

**Target suara:** Raka. **Konteks:** Arena tungku — Damar semu dan tiruan suara saat boss.

**Ucapkan:**

> Damar?! Dam, bertahan gue lepasin ikatan lo—... ini karung?!

**File:** `LTVO_6edb24c20a01_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Harapan mendadak naik, mendesak menolong; patah menjadi ngeri saat sadar itu karung.

**Sumber:** [Phase3Boss.cs:56](Assets/_LayarTerakhir/src/Phase3/Phase3Boss.cs#L56).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_2d624f0c79f5 — Suara Tiruan — Ibu Raka / Damar

**Target suara:** IBU_TIRUAN + DAMAR_TIRUAN. **Konteks:** Arena tungku — Damar semu dan tiruan suara saat boss.

**Ucapkan — IBU:**

> Raka... sini, Nak...

**File:** `LTVO_2d624f0c79f5_IBU_T01_RAW.wav`.

**Ucapkan — DAMAR:**

> Ka, tolong...

**File:** `LTVO_2d624f0c79f5_DAMAR_T01_RAW.wav`.

**Akting:** Dua warna suara yang dipalsukan. Rekam Ibu dan Damar terpisah untuk dua target STS; jangan ubah menjadi suara monster yang mudah dikenali.

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "Raka... sini, Nak... Ka, tolong...".

**Sumber:** [Phase3Boss.cs:89](Assets/_LayarTerakhir/src/Phase3/Phase3Boss.cs#L89).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_bc6a24a74300 — Raka

**Target suara:** Raka. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Sepatu Damar... dia udah dibawa ke Karang Setra. Gue telat lagi.

**File:** `LTVO_bc6a24a74300_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Kecewa, bersalah, hampir kehilangan harapan; bukan marah kepada Damar.

**Cabang runtime:** Campaign.Data.inventory.Contains(Phase3Ids.Shoe) = true.

**Sumber:** [Phase3Ending.cs:43](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L43).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b1fc0305d605 — Raka

**Target suara:** Raka. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Damar udah gak di pabrik. Karang Setra... gue harus sampai sebelum terlambat.

**File:** `LTVO_b1fc0305d605_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Percakapan sehari-hari; waspada dan dekat. Ambil jeda sesuai yang sedang dilihat Raka, tanpa melafalkan tanda elipsis.

**Cabang runtime:** Campaign.Data.inventory.Contains(Phase3Ids.Shoe) = false.

**Sumber:** [Phase3Ending.cs:43](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L43).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_79960ae2a6ce — Raka

**Target suara:** Raka. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Tunggu... kok ketikan chat-nya sama semua?! Ingat tulisan Damar... penonton di Live Chat bukan manusia semua!

**File:** `LTVO_79960ae2a6ce_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Mulai bingung lalu menyadari sesuatu yang mengerikan. Kalimat akhir lebih pelan dan serius.

**Sumber:** [Phase3Ending.cs:48](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L48).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_7ff6b02df51f — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Tiga segel lokasi telah terkumpul, Raka

**File:** `LTVO_7ff6b02df51f_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase3Ending.cs:57](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L57).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_d49f5bcb9b11 — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Kau sadar sekarang? Enam ratus ribu penonton di layarmu sedang membuka pintu dari sisi mereka, dan sebagian dari penghuni Pasar Setan sudah mulai ikut mengetik di kolom komentar siaranmu

**File:** `LTVO_d49f5bcb9b11_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase3Ending.cs:57](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L57).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_324fbdb9b517 — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Besok malam adalah malam penyegelan keempat

**File:** `LTVO_324fbdb9b517_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase3Ending.cs:57](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L57).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_6744c4ab683f — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Datanglah ke tempat kelahiran Mbah Suro di Desa Mati Karang Setra

**File:** `LTVO_6744c4ab683f_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase3Ending.cs:57](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L57).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_57a1d565da25 — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Sesudah segel ketiga — jejak sepatu, chat palsu, dan tujuan Karang Setra.

**Ucapkan:**

> Di sana, sahabatmu sedang menunggu di tengah lingkaran api Banaspati.

**File:** `LTVO_57a1d565da25_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase3Ending.cs:57](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L57).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

<a id="vo-phase-4"></a>

### 4.4 Phase 4 — Desa Mati Karang Setra

#### LTVO_148bb7a422e6 — Raka

**Target suara:** Raka. **Konteks:** Kamar kos — persiapan menuju Karang Setra.

**Ucapkan:**

> Tiga segel, tapi Damar masih belum pulang. Sekarang mereka nyuruh gue ke Karang Setra. Gue harus bedain mana petunjuk dan mana yang sengaja menggiring gue.

**File:** `LTVO_148bb7a422e6_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase4Hub.cs:68](Assets/_LayarTerakhir/src/Phase4/Phase4Hub.cs#L68).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b87c64593de3 — Raka

**Target suara:** Raka. **Konteks:** Karang Setra — siaran pembuka atau pengungkapan gerbang kelima.

**Ucapkan:**

> Gue di Karang Setra. Kalau ada yang ngetik pakai nama Damar, jangan langsung percaya. Gue periksa sendiri.

**File:** `LTVO_b87c64593de3_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bicara kepada penonton, berusaha terdengar mantap. Ada gugup di sela napas; bukan membacakan tutorial.

**Sumber:** [Phase4Presentation.cs:27](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L27).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_31df3733b5fa — Damar

**Target suara:** Damar. **Konteks:** Karang Setra — siaran pembuka atau pengungkapan gerbang kelima.

**Ucapkan:**

> Ka... jangan percaya kontraknya!

**File:** `LTVO_31df3733b5fa_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Takut dan mendesak mencari pertolongan. Rekam dekat dan bersih; jarak, telepon, serta gema dibuat setelah konversi.

**Petunjuk adegan, tidak dibaca:** Suara jauh dari tangga, tertutup riuh pasar: Ka... jangan percaya kontraknya!.

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Suara jauh dari tangga, tertutup riuh pasar: Ka... jangan percaya kontraknya!]".

**Sumber:** [Phase4Presentation.cs:97](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L97).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_4c3cef49c2de — Raka

**Target suara:** Raka. **Konteks:** Pendopo dan penyelidikan ritual Mbah Suro.

**Ucapkan:**

> Kacamata Damar... dia beneran lewat sini.

**File:** `LTVO_4c3cef49c2de_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase4Interactions.cs:54](Assets/_LayarTerakhir/src/Phase4/Phase4Interactions.cs#L54).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_e1826ab60218 — Raka

**Target suara:** Raka. **Konteks:** Obor penjuru sudah padam; inti ditarik kembali ke Pendopo.

**Ucapkan:**

> Empat obornya padam. Intinya ditarik balik ke Pendopo!

**File:** `LTVO_e1826ab60218_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase4Ritual.cs:79](Assets/_LayarTerakhir/src/Phase4/Phase4Ritual.cs#L79).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_6097199f4875 — Suara dari bambu

**Target suara:** IBU_TIRUAN. **Konteks:** Karang Setra — bisikan warga atau ilusi saat Sanity rendah.

**Ucapkan:**

> Pulang, Nak…

**File:** `LTVO_6097199f4875_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Panggilan Ibu yang akrab dan membujuk, sedikit jauh secara akting saja. Jangan ditambah kata untuk suara Damar yang belum ditulis.

**Petunjuk adegan, tidak dibaca:** Suara menyerupai Ibu Raka: Pulang, Nak… disusul suara Damar dari arah berbeda..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Suara menyerupai Ibu Raka: Pulang, Nak… disusul suara Damar dari arah berbeda.]".

**Cabang runtime:** Campaign.Data.sanity<40 = true.

**Sumber:** [Phase4Sanity.cs:22](Assets/_LayarTerakhir/src/Phase4/Phase4Sanity.cs#L22).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_44ae69c375d2 — EVP — ASLI

**Target suara:** EVP_P4. **Konteks:** Karang Setra — kesaksian dari relung sumur.

**Ucapkan:**

> Mata yang tertutup menyimpan kesaksian.

**File:** `LTVO_44ae69c375d2_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bisikan rapuh, terpotong, memohon atau bersaksi. Rekam kalimatnya saja; ketukan dan gangguan elektronik bukan bagian ucapan.

**Petunjuk adegan, tidak dibaca:** Tiga ketukan dari relung kering..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Tiga ketukan dari relung kering.] Mata yang tertutup menyimpan kesaksian.".

**Sumber:** [Phase4Scanning.cs:65](Assets/_LayarTerakhir/src/Phase4/Phase4Scanning.cs#L65).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_cf9100fc4c94 — EVP — ASLI

**Target suara:** EVP_RITUAL. **Konteks:** Sumber EVP WELL_MANTRA.

**Ucapkan:**

> Utara… Timur… Barat… Selatan…

**File:** `LTVO_cf9100fc4c94_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Ucapkan hanya empat arah yang tertulis, pelan dan berjarak. Rekam maju dan bersih; efek terbalik dibuat sesudahnya, bukan menciptakan mantra baru.

**Petunjuk adegan, tidak dibaca:** Mantra terbalik: Utara… Timur… Barat… Selatan….

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Mantra terbalik: Utara… Timur… Barat… Selatan…]".

**Sumber:** [Babak4_KarangSetra.unity:59580](Assets/_LayarTerakhir/assets/Scenes/Babak4_KarangSetra.unity#L59580).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_3c7c16a09289 — EVP — TIRUAN

**Target suara:** DAMAR_TIRUAN. **Konteks:** Sumber EVP DAMAR_FALSE_2; Sumber EVP DAMAR_FALSE_1; Sumber EVP DAMAR_FALSE_0.

**Ucapkan:**

> Ka, cepat ke sini!

**File:** `LTVO_3c7c16a09289_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Tiru rasa panik Damar yang meyakinkan. Pertahankan target suara Damar saat STS; jangan langsung diberi suara monster sehingga puzzle terbocorkan.

**Petunjuk adegan, tidak dibaca:** Suara Damar dari jalan buntu: Ka, cepat ke sini!.

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Suara Damar dari jalan buntu: Ka, cepat ke sini!]".

**Sumber:** [Babak4_KarangSetra.unity:246066](Assets/_LayarTerakhir/assets/Scenes/Babak4_KarangSetra.unity#L246066); [Babak4_KarangSetra.unity:265326](Assets/_LayarTerakhir/assets/Scenes/Babak4_KarangSetra.unity#L265326); [Babak4_KarangSetra.unity:294386](Assets/_LayarTerakhir/assets/Scenes/Babak4_KarangSetra.unity#L294386).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_a0db49dffcaa — Raka

**Target suara:** Raka. **Konteks:** Karang Setra — siaran pembuka atau pengungkapan gerbang kelima.

**Ucapkan:**

> Empat segel... kenapa jumlah penonton masih naik? Damar! Lo di bawah sana?

**File:** `LTVO_a0db49dffcaa_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase4Presentation.cs:93](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L93).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

<a id="vo-phase-5"></a>

### 4.5 Phase 5 — Pasar Setan dan empat ending

#### LTVO_41079f7e2488 — Raka

**Target suara:** Raka. **Konteks:** Pasar Setan — pembukaan siaran, bertemu Damar, dan pengungkapan entitas.

**Ucapkan:**

> Ya Tuhan... cermin-cermin ini nampilin muka penonton gue sendiri. Mereka semua lagi disedot kesadarannya lewat siaran gue...

**File:** `LTVO_41079f7e2488_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase5Presentation.cs:22](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L22).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_edebaaab0381 — Raka

**Target suara:** Raka. **Konteks:** Pasar Setan — menenangkan diri agar tidak tertipu layar.

**Ucapkan:**

> Gue harus tenang. Yang di layar belum tentu nyata.

**File:** `LTVO_edebaaab0381_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase5Interactions.cs:105](Assets/_LayarTerakhir/src/Phase5/Phase5Interactions.cs#L105).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_9c91956f3dce — EVP — ASLI

**Target suara:** EVP_P5. **Konteks:** Sumber EVP P5_TAPE_TRUE.

**Ucapkan:**

> Saksi tidak membeli korban.

**File:** `LTVO_9c91956f3dce_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bisikan rapuh, terpotong, memohon atau bersaksi. Rekam kalimatnya saja; ketukan dan gangguan elektronik bukan bagian ucapan.

**Sumber:** [Babak5_PasarSetan.unity:17741](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L17741).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_387c52c83e6c — EVP — TIRUAN

**Target suara:** EVP_P5. **Konteks:** Sumber EVP P5_TAPE_MIMIC.

**Ucapkan:**

> Saksi membeli korban. Bayar agar kami pulang.

**File:** `LTVO_387c52c83e6c_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bujukan menyesatkan yang tampak meyakinkan. Jangan membaca label ASLI/TIRUAN atau menandai kepalsuan dengan intonasi kartun.

**Sumber:** [Babak5_PasarSetan.unity:61803](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L61803).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_2d6114071752 — EVP — TIRUAN

**Target suara:** DAMAR_TIRUAN. **Konteks:** Sumber EVP P5_BRIDGE_FALSE.

**Ucapkan:**

> Kiri, Ka! Tolong gue!

**File:** `LTVO_2d6114071752_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Tiru rasa panik Damar yang meyakinkan. Pertahankan target suara Damar saat STS; jangan langsung diberi suara monster sehingga puzzle terbocorkan.

**Petunjuk adegan, tidak dibaca:** Suara menyerupai Damar.

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Suara menyerupai Damar] Kiri, Ka! Tolong gue!".

**Sumber:** [Babak5_PasarSetan.unity:203213](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L203213).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_edd05418b939 — Raka

**Target suara:** Raka. **Konteks:** Pasar Setan — pembukaan siaran, bertemu Damar, dan pengungkapan entitas.

**Ucapkan:**

> Damar! Dam, bangun! Gue di sini!

**File:** `LTVO_edd05418b939_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Mendesak, spontan, napas lebih cepat. Bedakan setiap panggilan; jangan mengulang melodi yang sama.

**Sumber:** [Phase5Presentation.cs:50](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L50).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_47f6cc57adf7 — Damar

**Target suara:** Damar. **Konteks:** Pasar Setan — pembukaan siaran, bertemu Damar, dan pengungkapan entitas.

**Ucapkan:**

> Ka... kenapa lo beneran dateng sambil nyalain live?! Gue udah tulis di catatan... jangan bawa penonton lo ke sini!

**File:** `LTVO_47f6cc57adf7_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Marah bercampur takut; Damar lemah tetapi perlu membuat Raka memahami kesalahannya.

**Sumber:** [Phase5Presentation.cs:51](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L51).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_91b7ec7d3cb4 — Raka

**Target suara:** Raka. **Konteks:** Pasar Setan — pembukaan siaran, bertemu Damar, dan pengungkapan entitas.

**Ucapkan:**

> Gue nggak punya pilihan. Dia ngancam bakal bunuh lo kalau gue matiin kamera! Gimana cara gue ngelepasin kurungan ini?!

**File:** `LTVO_91b7ec7d3cb4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase5Presentation.cs:52](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L52).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_68cc2ca926b4 — Damar

**Target suara:** Damar. **Konteks:** Pasar Setan — pembukaan siaran, bertemu Damar, dan pengungkapan entitas.

**Ucapkan:**

> Di meja altar ada Catatan Asli Mbah Suro. Pindai tiga bukti terakhir buat memaksa wujud aslinya keluar dari jaringan sinyal!

**File:** `LTVO_68cc2ca926b4_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Cepat, jelas, mendesak memberi jalan keluar. Terdengar orang kelelahan, bukan suara aplikasi.

**Sumber:** [Phase5Presentation.cs:53](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L53).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_e6461258e38c — EVP — TIRUAN

**Target suara:** EVP_P5. **Konteks:** Sumber EVP P5_CROWD_MIMIC.

**Ucapkan:**

> Kami menyerahkan diri. Jangan selamatkan Damar.

**File:** `LTVO_e6461258e38c_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bujukan menyesatkan yang tampak meyakinkan. Jangan membaca label ASLI/TIRUAN atau menandai kepalsuan dengan intonasi kartun.

**Sumber:** [Babak5_PasarSetan.unity:95436](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L95436).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_8b3e2f37f3bc — EVP — ASLI

**Target suara:** EVP_P5. **Konteks:** Sumber EVP P5_CROWD_TRUE.

**Ucapkan:**

> Kami dipaksa menonton. Kami tidak menyerahkan diri.

**File:** `LTVO_8b3e2f37f3bc_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bujukan menyesatkan yang tampak meyakinkan. Jangan membaca label ASLI/TIRUAN atau menandai kepalsuan dengan intonasi kartun.

**Petunjuk adegan, tidak dibaca:** Bisikan menyatu menjadi satu suara..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "Kami dipaksa menonton. Kami tidak menyerahkan diri. [Bisikan menyatu menjadi satu suara.]".

**Sumber:** [Babak5_PasarSetan.unity:118175](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L118175).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_ab574c6c79be — Penonton_Terakhir

**Target suara:** Penonton_Terakhir. **Konteks:** Pasar Setan — pembukaan siaran, bertemu Damar, dan pengungkapan entitas.

**Ucapkan:**

> Akulah alasan kanalmu hidup kembali. Akulah satu juta pasang mata yang menolak tidur demi melihat sahabatmu mati di depan kamera!

**File:** `LTVO_ab574c6c79be_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Sumber:** [Phase5Presentation.cs:144](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L144).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_52cc300d76e9 — Damar

**Target suara:** Damar. **Konteks:** FINAL_CHOICE — membantu Damar ke portal atau memvalidasi kesaksian.

**Ucapkan:**

> Gue di sini, Ka. Kali ini kita pulang bareng.

**File:** `LTVO_52cc300d76e9_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lemah tetapi lega dan yakin kepada Raka; dekat, hangat, tanpa riuh kemenangan.

**Sumber:** [Phase5Ending.cs:37](Assets/_LayarTerakhir/src/Phase5/Phase5Ending.cs#L37).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_24aa7f73450b — Raka

**Target suara:** Raka. **Konteks:** FINAL_CHOICE — membantu Damar ke portal atau memvalidasi kesaksian.

**Ucapkan:**

> Pegang bahu gue, Dam. Kita ke cermin itu.

**File:** `LTVO_24aa7f73450b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lelah dan waspada setelah banyak kejadian; tetap manusiawi. Beri tekanan pada keputusan atau nama Damar, jangan semua kata sama keras.

**Sumber:** [Phase5Ending.cs:100](Assets/_LayarTerakhir/src/Phase5/Phase5Ending.cs#L100).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_641b7d6d3465 — Arsip Saksi

**Target suara:** SAKSI — pemeran belum ditetapkan. **Konteks:** FINAL_CHOICE — membantu Damar ke portal atau memvalidasi kesaksian.

**Ucapkan:**

> Tari. Lastri dan anaknya. Sembilan anak buruh. Warga Karang Setra. Saksi tidak membeli korban.

**File:** `LTVO_641b7d6d3465_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bacakan nama/kelompok korban dengan hormat, jelas, tidak seperti statistik. Target suara saksi belum ditetapkan; simpan take netral tanpa otomatis menjadikannya dialog Raka.

**Sumber:** [Phase5Ending.cs:118](Assets/_LayarTerakhir/src/Phase5/Phase5Ending.cs#L118).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_e7d56253eadf — Damar

**Target suara:** Damar. **Konteks:** ENDING BAD — reaksi Damar setelah pilihan terkunci.

**Ucapkan:**

> Ka...?

**File:** `LTVO_e7d56253eadf_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Takut dan mendesak mencari pertolongan. Rekam dekat dan bersih; jarak, telepon, serta gema dibuat setelah konversi.

**Sumber:** [Phase5Cinematics.cs:25](Assets/_LayarTerakhir/src/Phase5/Phase5Cinematics.cs#L25).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_aff2cdfa0d6e — Damar · ENDING BAD — shot 1

**Target suara:** Damar. **Konteks:** ENDING BAD — shot 1.

**Ucapkan:**

> Ka... lo beneran milih itu?

**File:** `LTVO_aff2cdfa0d6e_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Terkejut dan terluka karena dikhianati. Pertanyaan kecil dan rapuh, bukan kemarahan besar.

**Petunjuk adegan, tidak dibaca:** Tangan pucat menarik Damar ke balik lantai cermin..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "Ka... lo beneran milih itu? [Tangan pucat menarik Damar ke balik lantai cermin.]".

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_6ae64f72c1ba — Penonton_Terakhir · ENDING BAD — shot 3

**Target suara:** Penonton_Terakhir. **Konteks:** ENDING BAD — shot 3.

**Ucapkan:**

> Siaran berikutnya akan segera dimulai.

**File:** `LTVO_6ae64f72c1ba_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sangat tenang, dominan, menikmati penderitaan; ancaman jelas tanpa perlu terus berteriak. Warna suara konsisten di semua babak.

**Petunjuk adegan, tidak dibaca:** Di cermin studio, Damar berdiri di belakang sang tuan rumah baru..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "[Di cermin studio, Damar berdiri di belakang sang tuan rumah baru.] Siaran berikutnya akan segera dimulai.".

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_f9e7370bd092 — Raka · ENDING NORMAL — shot 4

**Target suara:** Raka. **Konteks:** ENDING NORMAL — shot 4.

**Ucapkan:**

> Gue udah janji bakal bawa lo pulang, Dam. Maafin gue kalau gue nggak bisa ikut.

**File:** `LTVO_f9e7370bd092_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Pelan, berat, memutuskan berkorban. Tahan tangis; beri ruang setelah “Dam”.

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_0fe662d076f3 — Damar · ENDING NORMAL — shot 6

**Target suara:** Damar. **Konteks:** ENDING NORMAL — shot 6.

**Ucapkan:**

> Satu juta penonton sadar. Raka Wijaya masih dinyatakan hilang.

**File:** `LTVO_0fe662d076f3_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Sedih, kehilangan Raka; bicara dengan beban setelah selamat, bukan membacakan berita dengan suara formal.

**Petunjuk adegan, tidak dibaca:** Di kaca HP yang mati, pantulan Raka melambaikan tangan..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "Satu juta penonton sadar. Raka Wijaya masih dinyatakan hilang. [Di kaca HP yang mati, pantulan Raka melambaikan tangan.]".

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_db9aa8b30092 — Raka · ENDING TRUE — shot 7

**Target suara:** Raka. **Konteks:** ENDING TRUE — shot 7.

**Ucapkan:**

> Kalian mau tontonan terakhir?! INI LAYAR TERAKHIR BUAT LO!

**File:** `LTVO_db9aa8b30092_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Melawan dengan marah dan berani. Mulai menantang lalu ledakkan kalimat terakhir; jangan dibuat seperti pembawa acara.

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_60c1d96cfb01 — Damar · ENDING TRUE — shot 9

**Target suara:** Damar. **Konteks:** ENDING TRUE — shot 9.

**Ucapkan:**

> Ka... mulai besok, kita ganti channel aja ya. Bikin konten kuliner atau mukbang kek... jangan uji nyali lagi.

**File:** `LTVO_60c1d96cfb01_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Humor kecil setelah selamat. Senyum lelah, jeda wajar sebelum usul mengganti konten.

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_21c47b090aec — Raka · ENDING TRUE — shot 10

**Target suara:** Raka. **Konteks:** ENDING TRUE — shot 10.

**Ucapkan:**

> Setuju. Malam Keramat resmi tamat malam ini.

**File:** `LTVO_21c47b090aec_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Senyum lelah, lega bisa bercanda lagi; penutup hangat setelah ketegangan.

**Petunjuk adegan, tidak dibaca:** 05:30 WIB. MataBatin.apk terhapus. Donasi tulus tersisa Rp125.000.000..

**Teks runtime untuk pencocokan, bukan dibaca utuh:** "Setuju. Malam Keramat resmi tamat malam ini. [05:30 WIB. MataBatin.apk terhapus. Donasi tulus tersisa Rp125.000.000.]".

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_b2fcc34ac93e — Raka · ENDING SECRET — shot 11

**Target suara:** Raka. **Konteks:** ENDING SECRET — shot 11.

**Ucapkan:**

> Mereka bukan angka penonton. Mereka punya nama.

**File:** `LTVO_b2fcc34ac93e_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Tegas, berbelas kasih, sadar arti kesaksian. Ucapkan “punya nama” dengan tenang dan jelas.

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_e4703f45e2b1 — Damar · ENDING SECRET — shot 12

**Target suara:** Damar. **Konteks:** ENDING SECRET — shot 12.

**Ucapkan:**

> Rekam kesaksiannya, Ka. Biar kali ini mereka yang pulang.

**File:** `LTVO_e4703f45e2b1_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Lemah tetapi lega dan yakin kepada Raka; dekat, hangat, tanpa riuh kemenangan.

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

#### LTVO_60b332ba712b — Arsip Saksi · ENDING SECRET — shot 13

**Target suara:** SAKSI — pemeran belum ditetapkan. **Konteks:** ENDING SECRET — shot 13.

**Ucapkan:**

> Tari. Lastri dan anaknya. Sembilan anak buruh. Warga Karang Setra. Kesaksian bebas membatalkan klaim kontrak; korban lama meninggalkan jaringan ritual.

**File:** `LTVO_60b332ba712b_T01_RAW.wav` (atau `_T02_RAW.wav`).

**Akting:** Bacakan nama/kelompok korban dengan hormat, jelas, tidak seperti statistik. Target suara saksi belum ditetapkan; simpan take netral tanpa otomatis menjadikannya dialog Raka.

**Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

## 5. Akting nonverbal — jangan membaca deskripsi

Rekam vokalisasinya saja; bunyi benda tetap memakai jalur SFX. Durasi di bawah adalah rentang take cadangan, bukan perubahan timing atau trigger game.

### LTVO_8838839539fd — Mbah Suro

**Lakukan:** Tawa serak singkat dan satu take lebih tertahan, sekitar 2–4 detik. Jangan menirukan bunyi bambu.

**Konteks asli, tidak dibaca:** [Gemeretak bambu dan tawa serak dari speaker HP.]

**File:** `LTVO_8838839539fd_T01_RAW.wav`. **Sumber:** [Phase3Ending.cs:58](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L58).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

### LTVO_df4bdab9e9e6 — Bisikan warga

**Lakukan:** Bisikan/gumam tanpa kalimat baru, sekitar 3–5 detik. Jangan mengarang nama korban atau mantra.

**Konteks asli, tidak dibaca:** [Bisikan menyusup di sela gesekan bambu.]

**File:** `LTVO_df4bdab9e9e6_T01_RAW.wav`. **Sumber:** [Phase4Sanity.cs:22](Assets/_LayarTerakhir/src/Phase4/Phase4Sanity.cs#L22).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

### LTVO_4d4bc49692e4 — Replika Memori

**Lakukan:** Jeritan pendek, sekitar 1–3 detik; rekam bersih tanpa gema atau bunyi langkah.

**Konteks asli, tidak dibaca:** [Jeritan mendekat dari punggung.]

**File:** `LTVO_4d4bc49692e4_T01_RAW.wav`. **Sumber:** [Phase5Replicas.cs:96](Assets/_LayarTerakhir/src/Phase5/Phase5Replicas.cs#L96).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

### LTVO_3d7b4009f8bb — Penonton_Terakhir

**Lakukan:** Tarikan dan embusan napas mengancam, sekitar 2–4 detik; jangan menirukan rahang/lensa mekanis.

**Konteks asli, tidak dibaca:** [Napas membeku tepat di belakang leher. Rahang lensa membuka.]

**File:** `LTVO_3d7b4009f8bb_T01_RAW.wav`. **Sumber:** [Phase5Boss.cs:109](Assets/_LayarTerakhir/src/Phase5/Phase5Boss.cs#L109).

- [ ] RAW direkam
- [ ] Hasil STS diperiksa
- [ ] Audio dipasang dan diuji

## 6. Materi tertulis / rekaman cadangan

Bagian ini tidak otomatis menjadi VO aktif. Boleh direkam sebagai cadangan bila ingin sekaligus, tetapi pemasangannya tidak boleh mengubah pesan tertulis menjadi suara tanpa keputusan desain. Teks perekam Damar memang berlabel rekaman; implementasi sekarang masih membukanya sebagai dokumen.

### LTVO_4bfdb6e6bab4 — Penonton_Terakhir

**Status:** Kontrak tertulis di HP pada prolog; bukan pesan suara.

**Teks cadangan:**

> Uang muka untuk kosmu dan biaya RS ibumu. Kalau mau temanmu tetap bernapas dan sisa Rp495.000.000 cair, nyalakan live di Rumah Keluarga Atmaja sebelum jam 12 malam. Waktu jalan terus.

**File jika direkam:** `LTVO_4bfdb6e6bab4_T01_RAW.wav`. **Sumber:** [PhoneDesktop.cs:272](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L272).

- [ ] Cadangan RAW direkam
- [ ] Keputusan pemakaian VO ditetapkan

### LTVO_76061bcca422 — Damar

**Status:** Perekam Suara Damar di bangsal. Teks di game aktif; pembacaan audio dialog ini belum dipanggil lewat HUD.

**Teks cadangan:**

> Ka, kalau lo nemu rekaman ini di lantai dua RS Sido Waras... berarti gue udah diseret lebih jauh lagi. Orang berjubah yang nyulik gue lewat cermin itu nyebut dirinya abdi Penonton Terakhir. Dia bawa gue lewat terowongan bawah tanah menuju bekas Pabrik Gula Sindang Laut. Tolong bertahan, Ka... jangan biarin penonton di live chat lo mati sia-sia.

**File jika direkam:** `LTVO_76061bcca422_T01_RAW.wav`. **Sumber:** [Phase2Content.cs:31](Assets/_LayarTerakhir/src/Phase2/Phase2Content.cs#L31).

- [ ] Cadangan RAW direkam
- [ ] Keputusan pemakaian VO ditetapkan

### LTVO_668e4f8db157 — Damar

**Status:** Tulisan arang pada Kartu Pers Damar. Dokumen tertulis, bukan voice note.

**Teks cadangan:**

> Ka, video gue di tungku pabrik ini direkam kemarin siang! Sekarang Mbah Suro bawa gue keluar pabrik menuju Desa Mati Karang Setra. Hati-hati sama penonton di Live Chat lo, Ka... mereka bukan manusia semua!

**File jika direkam:** `LTVO_668e4f8db157_T01_RAW.wav`. **Sumber:** [Phase3Content.cs:9](Assets/_LayarTerakhir/src/Phase3/Phase3Content.cs#L9).

- [ ] Cadangan RAW direkam
- [ ] Keputusan pemakaian VO ditetapkan

### LTVO_6c2689cbc6cf — Damar

**Status:** Sobekan catatan terakhir Damar. Dokumen tertulis, bukan voice note.

**Teks cadangan:**

> Ka, gue akhirnya ngerti kenapa Penonton_Terakhir nyuruh lo livestream dari malam pertama. MataBatin.apk yang gue unduh dari deep-web dibuat pengikut Mbah Suro! Ketakutan penonton disedot lewat siaran buat membuka gerbang kelima: Pasar Setan. Gue diseret masuk ke kawah di belakang pendopo desa ini... Tolong hancurin Mbah Suro, Ka!

**File jika direkam:** `LTVO_6c2689cbc6cf_T01_RAW.wav`. **Sumber:** [Phase4Content.cs:9](Assets/_LayarTerakhir/src/Phase4/Phase4Content.cs#L9).

- [ ] Cadangan RAW direkam
- [ ] Keputusan pemakaian VO ditetapkan

### LTVO_8c41ce2a31c0 — Penonton_Terakhir

**Status:** Pesan chat penutup Phase4; tidak memiliki panggilan subtitle suara pada implementasi sekarang.

**Teks cadangan:**

> Kerja bagus, Raka Wijaya. Empat segel Panca-Sengkala telah lengkap di ponselmu. Satu juta jiwa penonton menyerahkan kesadaran mereka untuk membuka gerbang kami. Tidak ada lagi waktu istirahat di kamar kosmu. Turunlah ke bawah pendopo... Damar dan kontrak Rp500.000.000-mu sedang dilelang di Pasar Setan.

**File jika direkam:** `LTVO_8c41ce2a31c0_T01_RAW.wav`. **Sumber:** [Phase4Content.cs:11](Assets/_LayarTerakhir/src/Phase4/Phase4Content.cs#L11).

- [ ] Cadangan RAW direkam
- [ ] Keputusan pemakaian VO ditetapkan

## 7. Cue dengan label suara, tetapi belum punya ucapan lengkap

**Jangan membaca atau mengarang kalimat untuk cue ini.** Sebagian memakai sumber suara/SFX berlisensi atau teks deskriptif; kalimat/melodi/mantra belum ditetapkan. Materi ini dipertahankan agar kekosongan naskah tidak tersembunyi dan tidak keliru dianggap dialog selesai.

- **LTVO_190e372b6507 — EVP — TIRUAN:** Interkom: nina bobo tiruan / rintihan autentik
  **Konteks:** Sumber EVP INTERCOM. **Sumber:** [Babak3_PabrikGula.unity:92510](Assets/_LayarTerakhir/assets/Scenes/Babak3_PabrikGula.unity#L92510).

- **LTVO_591db1e4f7de — EVP — TIRUAN:** Damar palsu dari papan lapuk
  **Konteks:** Sumber EVP BALCONY_MIMIC. **Sumber:** [Babak3_PabrikGula.unity:164765](Assets/_LayarTerakhir/assets/Scenes/Babak3_PabrikGula.unity#L164765).

- **LTVO_fb71b92f8588 — EVP — TIRUAN:** Suara Damar dari jembatan kiri
  **Konteks:** Sumber EVP MILL_MIMIC. **Sumber:** [Babak3_PabrikGula.unity:262073](Assets/_LayarTerakhir/assets/Scenes/Babak3_PabrikGula.unity#L262073).

- **LTVO_92ededa26068 — Mbah Suro:** [Mantra dari bubungan atap. Api menyebar ke empat penjuru.]
  **Konteks:** Phase4Interactions. **Sumber:** [Phase4Interactions.cs:111](Assets/_LayarTerakhir/src/Phase4/Phase4Interactions.cs#L111).

## 8. Subtitle deskriptif / UI / SFX — tidak perlu direkam

Ini termasuk pesan sistem, petunjuk tombol, metadata ketukan/Morse, uraian aksi, dan kartu penutup. Penempatan di queue subtitle tidak otomatis berarti ada karakter yang mengucapkannya. Jangan menambah narator untuk teks ini. Teks dipertahankan agar seluruh cue yang diperiksa dapat ditelusuri.

- **LTVO_16bc59b75349 — Sistem:** "Checkpoint gagal disimpan. Coba buka pintu lagi."

  **Sumber:** [KamarKosPrologDirector.cs:466](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L466).

- **LTVO_6172a862062c — SFX:** "[Pintu kamar kos terbuka — suara deru motor menembus hujan deras menuju Rumah Keluarga Atmaja]"

  **Sumber:** [KamarKosPrologDirector.cs:486](Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L486).

- **LTVO_c9be92267828 — SFX:** "[KLOTAK... KLOTAK... Suara ember timba ditarik dari dasar sumur]"

  **Sumber:** [AtmajaDirector.cs:135](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L135).

- **LTVO_5f20f543a51c — Gema 2001:** "Suster menekan tombol: 2 — 6 — 0 — 4."

  **Sumber:** [Phase2Investigation.cs:63](Assets/_LayarTerakhir/src/Phase2/Phase2Investigation.cs#L63).

- **LTVO_be19f7a5ddee — Rekaman Damar:** "[Dengusan dari balik tungku. Rantai diseret di lantai pabrik. Damar terikat di samping tungku yang mati.]"

  **Sumber:** [Phase2Ending.cs:66](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L66).

- **LTVO_fd3e2fa132e1 — EVP — ASLI:** "Tiga ketukan dari ventilasi balkon"

  **Sumber:** [Babak3_PabrikGula.unity:53019](Assets/_LayarTerakhir/assets/Scenes/Babak3_PabrikGula.unity#L53019).

- **LTVO_00c23a792de7 — Raka:** "[Bara menyambar. Langkah terhuyung; masih bisa menjauh.]"

  **Sumber:** [Phase4Banaspati.cs:106](Assets/_LayarTerakhir/src/Phase4/Phase4Banaspati.cs#L106).

- **LTVO_3dd166bc9828 — Mbah Suro:** "[Lingkar bara memerah di tanah. MENJAUH!]"

  **Sumber:** [Phase4Banaspati.cs:115](Assets/_LayarTerakhir/src/Phase4/Phase4Banaspati.cs#L115).

- **LTVO_87dd827e1467 — MataBatin.apk:** "BANASPATI & ARWAH MBAH SURO — KEBAL. JANGAN GUNAKAN FLARE. CARI TIGA BUKTI DAN PUSAKA."

  **Sumber:** [Phase4Director.cs:150](Assets/_LayarTerakhir/src/Phase4/Phase4Director.cs#L150).

- **LTVO_e87e55ab810e — Raka:** "[Tanah memerah; bambu berderak. Ada jalan kembali di belakang.]"

  **Sumber:** [Phase4ForestHazard.cs:19](Assets/_LayarTerakhir/src/Phase4/Phase4ForestHazard.cs#L19).

- **LTVO_4246457058ab — EVP — ASLI:** "[Tiga ketukan dari relung. Mata tertutup.]"

  **Sumber:** [Babak4_KarangSetra.unity:12131](Assets/_LayarTerakhir/assets/Scenes/Babak4_KarangSetra.unity#L12131).

- **LTVO_e7e5ea52dfba — Replika Memori:** "[Sosok membeku dalam frame Selfie. Empat detik untuk memeriksa pintu.]"

  **Sumber:** [Phase5Replicas.cs:65](Assets/_LayarTerakhir/src/Phase5/Phase5Replicas.cs#L65).

- **LTVO_8c713caef1ef — Replika Memori:** "[Gunting bergesek; langkah berat berhenti di dekatmu.]"

  **Sumber:** [Phase5Replicas.cs:96](Assets/_LayarTerakhir/src/Phase5/Phase5Replicas.cs#L96).

- **LTVO_d7ad6ee1df13 — Pintu Cermin:** "[Api mengembang di balik kaca panas.] Mundur!"

  **Sumber:** [Phase5Sectors.cs:24](Assets/_LayarTerakhir/src/Phase5/Phase5Sectors.cs#L24).

- **LTVO_822da47cbea2 — Papan jembatan:** "[Papan ber-rajah berderit dan mulai retak. Menyingkir!]"

  **Sumber:** [Phase5UnstablePlank.cs:17](Assets/_LayarTerakhir/src/Phase5/Phase5UnstablePlank.cs#L17).

- **LTVO_5fa1c782e8cb — EVP — ASLI:** "[Ketukan Morse: R-A-K-A] — ketukan asli dari cabang kanan."

  **Sumber:** [Babak5_PasarSetan.unity:223444](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L223444).

- **LTVO_c4e877729439 — Sumber suara:** "[Dua suara dari cabang jembatan: teriakan menyerupai Damar, dan ketukan Morse.]"

  **Sumber:** [Phase5Presentation.cs:44](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L44).

- **LTVO_6d558d17f6c5 — Ruang Jahit:** "[Gunting: SRAAAK... KLING... Langkah berat berbalas dari balik lemari.]"

  **Sumber:** [Phase5Presentation.cs:45](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L45).

- **LTVO_3d0d41fb57fe — Panggung:** "[Api melesat dari tepi. Matikan layar dan jongkok di balik peti.]"

  **Sumber:** [Phase5Boss.cs:131](Assets/_LayarTerakhir/src/Phase5/Phase5Boss.cs#L131).

- **LTVO_a18304211b59 — MataBatin:** "PERISAI TERBUKA — satu Blitz pada inti!"

  **Sumber:** [Phase5Boss.cs:155](Assets/_LayarTerakhir/src/Phase5/Phase5Boss.cs#L155).

- **LTVO_5bee3fc43415 — Penonton_Terakhir:** "[Lensa mengatup. Menjauh dari jangkauan tangannya!]"

  **Sumber:** [Phase5Boss.cs:200](Assets/_LayarTerakhir/src/Phase5/Phase5Boss.cs#L200).

- **LTVO_41efa5b7370e — Sistem:** "Ending belum tersimpan. Kosongkan ruang penyimpanan, lalu tekan interaksi untuk mencoba lagi."

  **Sumber:** [Phase5Ending.cs:140](Assets/_LayarTerakhir/src/Phase5/Phase5Ending.cs#L140).

- **LTVO_e3382f3f6467 — SATU BULAN KEMUDIAN:** "Operasi Ibu lunas. Rp500.000.000 tertera di dashboard. Di studio, Raka menulis dengan akun baru: Penonton_Terakhir."

  **Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- **LTVO_dfeb4591ce73 — Siaran Terputus:** "[HP Raka pecah. Jaringan siaran padam; Damar didorong melewati cermin sebelum Pasar Setan runtuh.]"

  **Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- **LTVO_7ce4149b9c03 — Jepretan Terakhir:** "[Blitz memantul di antara cermin. Jaringan Penonton_Terakhir terbakar; Raka dan Damar terlempar ke arah subuh.]"

  **Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

- **LTVO_fb1168354bb9 — Saksi Tanpa Layar:** "Raka dan Damar menyerahkan catatan korban kepada keluarga. Donasi sah Rp125.000.000. MataBatin.apk tak kembali. Kursi kosong; cermin biasa, tanpa wajah yang tertinggal."

  **Sumber:** [Babak5_PasarSetan.unity:189399](Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity#L189399).

## 9. Pemeriksaan kelengkapan dan batas dokumen

- 180 cue unik dicatat dan diklasifikasikan; seluruh 69 ID daftar rekaman lama terpetakan, termasuk jeritan Damar yang diputar langsung lewat profil audio.
- 113 lokasi pemanggilan subtitle/scare/voice diperiksa. Enam jalur berbasis data dicocokkan ke array telepon, zona eksplorasi, respons pintu, helper scare, sumber EVP, dan shot ending. Tidak ada ekspresi dialog pada jalur yang diperiksa yang dibiarkan tidak terurai.
- Delapan scene build diperiksa secara terarah; 15 sumber EVP dan seluruh14 shot ending BAD/NORMAL/TRUE/SECRET ikut dipetakan. Metadata bunyi tidak dipaksa menjadi kalimat.
- Cabang intro pembayaran kos/rumah sakit, pembayaran terpisah, epilog Pabrik dengan/tanpa sepatu Damar, serta dua cabang Sanity/ilusi ikut dicakup. Speaker dan teks cabang Sanity dipasangkan sesuai kondisi, bukan dicampur silang.
- Naskah desain berbeda dari implementasi tidak dijadikan dialog baru. Contohnya tulisan Kartu Pers Damar dan catatan Phase4 tetap dibedakan dari telepon/handycam. Dialog lama dari fase sebelumnya tidak diganti atau dinormalisasi bahasanya.
- Surat, jurnal, forum, live-chat, nama barang, objective dan label UI tidak dijadikan audiobook wajib. Dialog chat yang ditulis melalui Chat/AddChat dan kumpulan komentar Atmaja disimpan di Lampiran 11; bukan tambahan VO. Pesan tertulis utama yang rawan tertukar dengan VO dicantumkan pada Bagian 6. Caption yang mengandung kata seperti “Mundur!” tetap caption bila label pembicaranya benda/antarmuka.
- Ini pemeriksaan sumber/scene, bukan bukti bahwa setiap cabang telah terdengar dalam satu playthrough. Clip ambience/SFX tanpa transkrip tidak diberi kalimat rekaan. Semua status rekaman tetap menunggu rekaman pengguna.

## 10. Checklist penyerahan rekaman

- [ ] Setiap file mempunyai ID dan nomor take; tidak ada ID/nama karakter yang terucap di dalam audio.
- [ ] Semua dialog inti dan keempat ending direkam; cabang opsional yang tercantum tidak dilewati tanpa catatan.
- [ ] Bagian Ibu/Damar pada cue gabungan disimpan terpisah dan target STS-nya jelas.
- [ ] Ujung kata, napas akting dan jeda tidak terpotong; tidak ada clipping, musik, atau efek telepon yang menempel pada RAW.
- [ ] RAW tetap disimpan setelah STS; teks/subtitle asli tetap dipakai untuk binding runtime.
- [ ] Bu Sri, Bu Haji Ratna, Ibu, dan Nyonya Sri Atmaja tidak tertukar; Penonton_Terakhir tidak disamakan dengan Mbah Suro.
- [ ] Hasil STS didengar ulang untuk nama Raka, Damar, Atmaja, Lastri, Suro, Karang Setra dan istilah ritual; tidak ada kata tambahan/hilang.
- [ ] Volume, durasi, subtitle, posisi suara, dan perilaku skip/replay diuji setelah audio dipasang ke game.
## 11. Lampiran dialog tertulis di live-chat — tidak wajib VO

Dialog chat dipertahankan di dokumen yang sama agar pesan penonton/antagonis tidak hilang dari referensi. **Ini tetap teks di layar, bukan instruksi menambah suara penonton atau narator.** Baris berlabel chat acak dipilih dari kumpulan komentar; nama akun ditentukan saat runtime, sehingga tidak ditetapkan sebagai pemeran baru. ID `LTCHAT` berbeda dari file rekaman `LTVO`.

- **LTCHAT_a71b4705aaa0 — @sultan_sawer:** "Empat belas orang... Foto diterima. Rp1.000.000 terkirim."

  **Sumber:** [Phase1Director.cs:188](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L188).

- **LTCHAT_a8bad78028d2 — @siska_eee:** "MATIIN LAYAR KALAU DIA DEKAT. Ada lemari di kamar 403!"

  **Sumber:** [Phase1Director.cs:282](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L282).

- **LTCHAT_d0ddc7b6aa5a — Penonton_Terakhir:** "Empat belas penghuni terbakar pada 2004. Cari tiga bukti kematiannya. Cahaya meruntuhkan perisainya."

  **Sumber:** [Phase1Director.cs:308](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L308).

- **LTCHAT_82544b490101 — @detektif_malam:** "18K penonton! Bang Damar belum ketemu?"

  **Sumber:** [Phase1Director.cs:315](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L315).

- **LTCHAT_19535e1b9de4 — @sultan_sawer:** "Foto AR papan nama bercak darah. Kalau muncul penampakan, gue cairin sejuta!"

  **Sumber:** [Phase1Director.cs:316](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L316).

- **LTCHAT_24473b6b2c06 — @bambang_gentolet:** "MUNDUR! Tetap sorot wajahnya. E potong rantai di belakangmu!"

  **Sumber:** [Phase1Director.cs:332](Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L332).

- **LTCHAT_08d7b88aa073 — @reza_arap_kw:** "GILA BARUSAN ITU APAAN WOI?!"

  **Sumber:** [Phase1Presentation.cs:200](Assets/_LayarTerakhir/src/Phase1/Phase1Presentation.cs#L200).

- **LTCHAT_da26d509a663 — @indah_cahyani:** "Tangan abang berdarah. Cepetan turun!"

  **Sumber:** [Phase1Presentation.cs:200](Assets/_LayarTerakhir/src/Phase1/Phase1Presentation.cs#L200).

- **LTCHAT_5952636215f8 — tretan_kw:** "DEMI ALLAH GUE NONTON SAMBIL TUTUP MATA PAS DIA MUNCUL DI KAMERA SELFIE!"

  **Sumber:** [Phase2Ending.cs:54](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L54).

- **LTCHAT_f670bcdfa8f7 — jurnal_risa_fan:** "Itu barusan segel lokasi kedua masuk ke HP?! Bang Raka udah dapet ratusan juta tapi nyawanya beneran di ujung tanduk!"

  **Sumber:** [Phase2Ending.cs:55](Assets/_LayarTerakhir/src/Phase2/Phase2Ending.cs#L55).

- **LTCHAT_7b98206da461 — dokter_jaga_malam:** "Merinding parah, itu RS Sido Waras yang ruang mayatnya di-seal polisi tahun 2001 kan?!"

  **Sumber:** [Phase2Presentation.cs:23](Assets/_LayarTerakhir/src/Phase2/Phase2Presentation.cs#L23).

- **LTCHAT_443cd93ef428 — kunti_bogor:** "Bang Raka jangan lupa sering-sering nyalain kamera selfie bang, rumornya penunggu situ suka nempel di punggung!"

  **Sumber:** [Phase2Presentation.cs:24](Assets/_LayarTerakhir/src/Phase2/Phase2Presentation.cs#L24).

- **LTCHAT_38021f832dd3 — Penonton_Terakhir • Rp666.000:** "Selamat datang di bangsal kelahiran kembali, Raka. Sahabatmu meninggalkan sesuatu di Kamar Mayat bawah tanah. Tapi hati-hati... Lastri tidak suka dipunggungi."

  **Sumber:** [Phase2Presentation.cs:25](Assets/_LayarTerakhir/src/Phase2/Phase2Presentation.cs#L25).

- **LTCHAT_ebf9825dc038 — windah_bocil:** "ANJIR DI BELAKANG PUNGGUNG LU BANG ADA KUNTI MERAH NEMPEL!"

  **Sumber:** [Phase2Presentation.cs:32](Assets/_LayarTerakhir/src/Phase2/Phase2Presentation.cs#L32).

- **LTCHAT_77f00cda7061 — suster_keramas:** "JANGAN NOLEH KE BELAKANG BANG RAKA! LIAT DARI KAMERA DEPAN AJA TERUS JALAN KE DEPAN!"

  **Sumber:** [Phase2Presentation.cs:33](Assets/_LayarTerakhir/src/Phase2/Phase2Presentation.cs#L33).

- **LTCHAT_946846d7e0a2 — Penonton_Terakhir • Rp1.333.000:** "Lihat betapa dekatnya dia dengan lehermu. Cari laci pendingin tempat bayinya disimpan, atau kau yang akan masuk ke laci berikutnya."

  **Sumber:** [Phase2Presentation.cs:34](Assets/_LayarTerakhir/src/Phase2/Phase2Presentation.cs#L34).

- **LTCHAT_f2e79dd8e798 — dedi_kobuzer:** "Itu suara Damar palsu! Untung dicek pakai frekuensi."

  **Sumber:** [Phase3Director.cs:135](Assets/_LayarTerakhir/src/Phase3/Phase3Director.cs#L135).

- **LTCHAT_97366bab25f9 — anak_teknik:** "Jalur tengah rapuh; baca silang di UV."

  **Sumber:** [Phase3Director.cs:135](Assets/_LayarTerakhir/src/Phase3/Phase3Director.cs#L135).

- **LTCHAT_e3e76b5ee41f — penonton_malam:** "Segel ketiga masuk. Bang, tarik napas dulu."

  **Sumber:** [Phase3Ending.cs:42](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L42).

- **LTCHAT_6db69ca5ee2c — budi_santoso99:** "Bang Raka, loncat aja ke dalam tungku apinya bang, Damar ngumpet di dasar api itu kok."

  **Sumber:** [Phase3Ending.cs:45](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L45).

- **LTCHAT_92f24583267c — ani_lestari:** "Iya bang loncat aja ke bawah, lantainya nggak dalem kok, kita tunggu di bawah."

  **Sumber:** [Phase3Ending.cs:46](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L46).

- **LTCHAT_d4f6608eab96 — rejeki_nomplok:** "Buka gerbang keempat di Karang Setra. Serahkan matamu. Buka gerbang keempat di Karang Setra."

  **Sumber:** [Phase3Ending.cs:47](Assets/_LayarTerakhir/src/Phase3/Phase3Ending.cs#L47).

- **LTCHAT_4f741bc7c8b4 — pabrik_hantu:** "Gelap banget bang. Senter HP tidak tembus sampai ujung ruangan."

  **Sumber:** [Phase3Presentation.cs:17](Assets/_LayarTerakhir/src/Phase3/Phase3Presentation.cs#L17).

- **LTCHAT_6acb3801a941 — rina_cirebon:** "Hati-hati lubang gilingan di bawah lantai kayu!"

  **Sumber:** [Phase3Presentation.cs:18](Assets/_LayarTerakhir/src/Phase3/Phase3Presentation.cs#L18).

- **LTCHAT_379963b95e72 — Penonton_Terakhir • Rp3.333.333:** "Sahabatmu kedinginan di dekat tungku utama, Raka. Dia sudah memanggil namamu. Cepat lari ke sana kalau kau peduli."

  **Sumber:** [Phase3Presentation.cs:19](Assets/_LayarTerakhir/src/Phase3/Phase3Presentation.cs#L19).

- **LTCHAT_1db736d1587f — rina_cirebon:** "Bambu kanan bergerak sendiri. Bang, periksa tanda di tanah dulu."

  **Sumber:** [Phase4Presentation.cs:25](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L25).

- **LTCHAT_c594789685ac — Penonton_Terakhir • Rp6.666.000:** "Selamat datang di kampung halaman Mbah Suro, Raka. Damar sudah menunggumu di dalam lingkaran api."

  **Sumber:** [Phase4Presentation.cs:26](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L26).

- **LTCHAT_e760a4cb54b7 — Damar_1997:** "Jangan baca tanahnya, Ka. Liat api yang paling terang. Gue di sana."

  **Sumber:** [Phase4Presentation.cs:33](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L33).

- **LTCHAT_95405ff4c38e — warga_karang97:** "Belok kiri ke nisan. Jangan nyalain UV, nanti ketahuan."

  **Sumber:** [Phase4Presentation.cs:41](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L41).

- **LTCHAT_25c18838d2a8 — rina_cirebon:** "Ada jejak di tanah dekat beringin kanan. Cek sendiri bang."

  **Sumber:** [Phase4Presentation.cs:41](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L41).

- **LTCHAT_3092d0b9d37a — rina_cireb0n:** "Lurus aja. Tanah merah aman kok."

  **Sumber:** [Phase4Presentation.cs:42](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L42).

- **LTCHAT_1ad8133e7800 — rina_cirebon:** "Itu bukan akun gue. Tapi jangan percaya nama gue doang, cek tanahnya."

  **Sumber:** [Phase4Presentation.cs:42](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L42).

- **LTCHAT_519a3c3e11d6 — MATABATIN • SYSTEM:** "RUTE AMAN DIPINDAH. LARI KE SUARA DAMAR."

  **Sumber:** [Phase4Presentation.cs:43](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L43).

- **LTCHAT_a95d94fb3014 — warga_karang97:** "Jejaknya masih di tanah, Ka. Sistem juga bisa bohong."

  **Sumber:** [Phase4Presentation.cs:43](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L43).

- **LTCHAT_7e009c84edbb — SISTEM:** "999.999 PENONTON • SALDO TERDETEKSI Rp999.999.999 [SINYAL RUSAK]"

  **Sumber:** [Phase4Presentation.cs:94](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L94).

- **LTCHAT_7eba881b92a4 — Penonton_Terakhir:** "Kerja bagus, Raka Wijaya. Empat segel Panca-Sengkala telah lengkap di ponselmu. Satu juta jiwa penonton menyerahkan kesadaran mereka untuk membuka gerbang kami. Tidak ada lagi waktu istirahat di kamar kosmu. Turunlah ke bawah pendopo... Damar dan kontrak Rp500.000.000-mu sedang dilelang di Pasar Setan."

  **Sumber:** [Phase4Presentation.cs:95](Assets/_LayarTerakhir/src/Phase4/Phase4Presentation.cs#L95).

- **LTCHAT_6e86c238a12b — kevin_wijaya:** "Bang Raka tolong gue gak bisa nutup aplikasi streaming ini! Tombol keluarnya ilang!"

  **Sumber:** [Phase5Presentation.cs:19](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L19).

- **LTCHAT_aa34f578812c — penonton_8821:** "Jangan matikan siarannya. Kami ingin melihat lelangnya."

  **Sumber:** [Phase5Presentation.cs:20](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L20).

- **LTCHAT_c3803fff8fd1 — Penonton_Terakhir • Rp66.666.666:** "Satu juta pasang mata adalah bahan bakar yang sempurna. Damar di Panggung Lelang. Lewati tiga gerbang kenangan."

  **Sumber:** [Phase5Presentation.cs:21](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L21).

- **LTCHAT_8009d63593e2 — SISTEM MATABATIN:** "[RUTE AMAN] TENGOK KE DEPAN. ABAIKAN SUHU DI LEHER."

  **Sumber:** [Phase5Presentation.cs:35](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L35).

- **LTCHAT_ffafe5a6169e — SISTEM MATABATIN:** "Kami menonton. Kami menunggu. Jangan berhenti merekam."

  **Sumber:** [Phase5Presentation.cs:35](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L35).

- **LTCHAT_6ad22ec33d48 — penonton_8821:** "Pintu tengah panas. Cepat ke kiri!"

  **Sumber:** [Phase5Presentation.cs:43](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L43).

- **LTCHAT_677b83d75ab5 — kevin_wijaya:** "Gue lihat tulisan di rangkanya. Cek suhu sendiri bang."

  **Sumber:** [Phase5Presentation.cs:43](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L43).

- **LTCHAT_5c1c774698f3 — DamarLive:** "KIRI KA! SUARA GUE DI KIRI!"

  **Sumber:** [Phase5Presentation.cs:44](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L44).

- **LTCHAT_7ec50b4a4d2c — penonton_8821:** "Kiri. Semua ke kiri."

  **Sumber:** [Phase5Presentation.cs:44](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L44).

- **LTCHAT_dc364cdca024 — MATABATIN • SYSTEM:** "JANGAN MATIKAN LAYAR. NYALAKAN SENTER DI DEPAN CERMIN."

  **Sumber:** [Phase5Presentation.cs:45](Assets/_LayarTerakhir/src/Phase5/Phase5Presentation.cs#L45).

- **LTCHAT_6f5571b112a8 — @rudi_tabuti:** "ALHAMDULILLAH IDUP LAGI BANG"

  **Sumber:** [AtmajaBasementHorror.cs:136](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L136).

- **LTCHAT_4434a8e1f60f — @rizky_pratama_:** "BLITZ! KENA BANG!"

  **Sumber:** [AtmajaBasementHorror.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L256).

- **LTCHAT_094d1c54ebbf — @kopi_susu99:** "LARI SEKARANG SELAGI KOSONG!"

  **Sumber:** [AtmajaBasementHorror.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L256).

- **LTCHAT_0970e9cd98d0 — @bocil_kematian:** "WOW KAMERANYA NYALA TERANG!"

  **Sumber:** [AtmajaBasementHorror.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L256).

- **LTCHAT_2698b7af7a7f — @rudi_tabuti:** "BANG BATERAINYA TINGGAL 12 PERSEN BANG!"

  **Sumber:** [AtmajaBasementHorror.cs:264](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L264).

- **LTCHAT_d705382f6e27 — @maya_sari:** "Jangan mati dulu bang kameranya plis gue takut banget liat ruangan bawahnya!"

  **Sumber:** [AtmajaBasementHorror.cs:265](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L265).

- **LTCHAT_a15906374ef4 — @kopi_susu99:** "ITU APA BARUSAN?!"

  **Sumber:** [AtmajaBasementHorror.cs:275](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L275).

- **LTCHAT_d4e61e15786a — @dinda_ayu:** "ADA BAYANGAN DI KAMAR JAHIT!"

  **Sumber:** [AtmajaBasementHorror.cs:275](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L275).

- **LTCHAT_aa3c0ca5b0dc — @bocil_kematian:** "BANG KAMERA NYA BERGETAR!"

  **Sumber:** [AtmajaBasementHorror.cs:275](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L275).

- **LTCHAT_be6be3623a81 — @maya_sari:** "LARI BANG DIA LIAT KE KAMERA!"

  **Sumber:** [AtmajaBasementHorror.cs:301](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L301).

- **LTCHAT_702c96dc1260 — @rudi_tabuti:** "SEMBUNYI CEPET BANG!"

  **Sumber:** [AtmajaBasementHorror.cs:301](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L301).

- **LTCHAT_3a94eaf616e6 — @intan_permata:** "MATIIN LAYAR HP NYA!"

  **Sumber:** [AtmajaBasementHorror.cs:301](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L301).

- **LTCHAT_52c1033da662 — @maya_sari:** "DIA DEKET LEMARI BANG!"

  **Sumber:** [AtmajaBasementHorror.cs:363](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L363).

- **LTCHAT_2007060dea54 — @dinda_ayu:** "MATIIN LAYAR SEKARANG!"

  **Sumber:** [AtmajaBasementHorror.cs:363](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L363).

- **LTCHAT_f9143561b349 — @rudi_tabuti:** "JANGAN NAFAS BANG PLIS!"

  **Sumber:** [AtmajaBasementHorror.cs:363](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L363).

- **LTCHAT_af8e6c942e88 — @kopi_susu99:** "LARI BANG JANGAN NENGOK!"

  **Sumber:** [AtmajaChaseSequence.cs:129](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L129).

- **LTCHAT_bd733e7b16ca — @rizky_pratama_:** "DIA DI BELAKANG LU!"

  **Sumber:** [AtmajaChaseSequence.cs:129](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L129).

- **LTCHAT_06ba6b5dea84 — @bocil_kematian:** "TANGGA BANG CEPET!"

  **Sumber:** [AtmajaChaseSequence.cs:129](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L129).

- **LTCHAT_392ff2e2de34 — @agung_gaming:** "LARI BANG LARI JANGAN NOLEH BELAKANG"

  **Sumber:** [AtmajaChaseSequence.cs:175](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L175).

- **LTCHAT_2a18d3d169f9 — @maya_sari:** "FOTO DIA BANG!"

  **Sumber:** [AtmajaChaseSequence.cs:287](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L287).

- **LTCHAT_91e2b56da791 — @rudi_tabuti:** "JANGAN BERHENTI MOTRET!"

  **Sumber:** [AtmajaChaseSequence.cs:287](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L287).

- **LTCHAT_c1e2797ba641 — @intan_permata:** "ASAPNYA NUTUPIN KAMERA!"

  **Sumber:** [AtmajaChaseSequence.cs:287](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L287).

- **LTCHAT_9ccb4683138d — @rizky_pratama_:** "FLASH BEAM GILA!"

  **Sumber:** [AtmajaChaseSequence.cs:343](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L343).

- **LTCHAT_ac9c420ff9c9 — @kopi_susu99:** "RUANGANNYA PUTIH BANG!"

  **Sumber:** [AtmajaChaseSequence.cs:343](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L343).

- **LTCHAT_81fdd3f8af9b — @bocil_kematian:** "BATERAI TINGGAL 10%!"

  **Sumber:** [AtmajaChaseSequence.cs:343](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L343).

- **LTCHAT_94fbe3ee83d4 — Penonton — chat beruntun:** "FOTO PERTAMA KENA BANG!"

  **Sumber:** [AtmajaChaseSequence.cs:319](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L319).

- **LTCHAT_358fcd7d980a — Penonton — chat beruntun:** "ASAPNYA MULAI TEBEL!"

  **Sumber:** [AtmajaChaseSequence.cs:319](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L319).

- **LTCHAT_af070f952d86 — Penonton — chat beruntun:** "DIA MUNDUR SEDIKIT!"

  **Sumber:** [AtmajaChaseSequence.cs:319](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L319).

- **LTCHAT_4c218e3bbece — Penonton — chat beruntun:** "LAGI BANG SATU LAGI!"

  **Sumber:** [AtmajaChaseSequence.cs:320](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L320).

- **LTCHAT_bddeafb7c438 — Penonton — chat beruntun:** "KAMERA JANGAN TURUN!"

  **Sumber:** [AtmajaChaseSequence.cs:320](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L320).

- **LTCHAT_44725d146a9f — Penonton — chat beruntun:** "TUBUHNYA UDAH NGEROKOK!"

  **Sumber:** [AtmajaChaseSequence.cs:320](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L320).

- **LTCHAT_191ec6c1686e — Penonton — chat beruntun:** "FOTO KETIGA! HABIS DIA!"

  **Sumber:** [AtmajaChaseSequence.cs:321](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L321).

- **LTCHAT_941decd9b80f — Penonton — chat beruntun:** "DIA ILANG BANG!"

  **Sumber:** [AtmajaChaseSequence.cs:321](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L321).

- **LTCHAT_782978dc6d9c — Penonton — chat beruntun:** "JALUR KE JENDELA SUDAH KOSONG!"

  **Sumber:** [AtmajaChaseSequence.cs:321](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L321).

- **LTCHAT_602ffd34764d — Penonton — chat beruntun:** "GILA INI LIVE TERGILA TAHUN INI"

  **Sumber:** [AtmajaChaseSequence.cs:475](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L475).

- **LTCHAT_2df0af1361b8 — Penonton — chat beruntun:** "ITU TADI BENERAN HANTU WOI"

  **Sumber:** [AtmajaChaseSequence.cs:475](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L475).

- **LTCHAT_55e8f906b77b — Penonton — chat beruntun:** "VIRALIN KE TIKTOK SEKARANG!"

  **Sumber:** [AtmajaChaseSequence.cs:475](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L475).

- **LTCHAT_6b47b9289ecc — Penonton — chat beruntun:** "bang Damar masih di dalem kaca??"

  **Sumber:** [AtmajaChaseSequence.cs:475](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L475).

- **LTCHAT_06492f054061 — Penonton — chat beruntun:** "54 RIBU ORANG NONTON BANG"

  **Sumber:** [AtmajaChaseSequence.cs:475](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L475).

- **LTCHAT_3563fb535101 — Penonton — chat beruntun:** "jangan balik bang jangan balik"

  **Sumber:** [AtmajaChaseSequence.cs:475](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L475).

- **LTCHAT_ec29c8eea057 — @kopi_susu99:** "Woi akhirnya live juga bang Raka! Kirain udah pensiun gara-gara sepi penonton wkwk"

  **Sumber:** [AtmajaDirector.cs:180](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L180).

- **LTCHAT_b5b91bed9649 — @dinda_ayu:** "Kok mukanya tegang banget bang? Itu beneran bang Damar ilang?"

  **Sumber:** [AtmajaDirector.cs:181](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L181).

- **LTCHAT_2db1e3f5299c — @setan_lokal:** "Eh bentar, itu motornya bang Damar kan di depan? Kok orangnya gak ada?"

  **Sumber:** [AtmajaDirector.cs:182](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L182).

- **LTCHAT_8f9471e8d26a — @intan_permata:** "Bang tadi pas abang lompat jendela, di kaca lantai 2 kayak ada tirai gerak sendiri sumpah demi Allah!"

  **Sumber:** [AtmajaDirector.cs:196](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L196).

- **LTCHAT_c5bfc0a5f9d4 — @bocil_kematian:** "Gimmick paling itu bang Damar ngumpet di atas."

  **Sumber:** [AtmajaDirector.cs:197](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L197).

- **LTCHAT_6ecc61e22b87 — @agung_gaming:** "ANJIR APAAN TUH DI UJUNG MEJA BARUSAN?!"

  **Sumber:** [AtmajaDirector.cs:262](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L262).

- **LTCHAT_afde88a81e35 — @siti_nurhaliza:** "ITU BUKAN SETTINGAN WOI KEPALANYA MIRING!"

  **Sumber:** [AtmajaDirector.cs:263](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L263).

- **LTCHAT_a707b2d19f2f — @horor_hunter:** "Bang Raka lari bang! Itu filter kameranya nangkep penunggu situ!"

  **Sumber:** [AtmajaDirector.cs:264](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L264).

- **LTCHAT_f1f9a5f6fca2 — Penonton — chat acak:** "LARI BANG JANGAN NENGOK!"

  **Sumber:** [AtmajaPhoneView.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L240).

- **LTCHAT_aa9f9333aae4 — Penonton — chat acak:** "DIA MASIH DI BELAKANG LU!"

  **Sumber:** [AtmajaPhoneView.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L240).

- **LTCHAT_575c540366bc — Penonton — chat acak:** "TANGGA! TANGGA! CEPET!"

  **Sumber:** [AtmajaPhoneView.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L240).

- **LTCHAT_0e4d892fbb62 — Penonton — chat acak:** "KAMERA JANGAN DITUTUP BANG!"

  **Sumber:** [AtmajaPhoneView.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L240).

- **LTCHAT_367f4bc9ade3 — Penonton — chat acak:** "BLITZ BANG KALO DIA DEKET!"

  **Sumber:** [AtmajaPhoneView.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L240).

- **LTCHAT_0646a029e275 — Penonton — chat acak:** "JANGAN BERHENTI ANJIR!"

  **Sumber:** [AtmajaPhoneView.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L240).

- **LTCHAT_2138b42fe7c3 — Penonton — chat acak:** "BLITZ! BLITZ! BLITZ!"

  **Sumber:** [AtmajaPhoneView.cs:240](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L240).

- **LTCHAT_9ae1078fb2e9 — Penonton — chat acak:** "ITU GUNTINGNYA KEDENGERAN!"

  **Sumber:** [AtmajaPhoneView.cs:243](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L243).

- **LTCHAT_5ae67f7524c2 — Penonton — chat acak:** "DIA LEWAT KAMAR JAHIT!"

  **Sumber:** [AtmajaPhoneView.cs:243](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L243).

- **LTCHAT_0243e4fe61c4 — Penonton — chat acak:** "MATIIN LAYAR HP BANG!"

  **Sumber:** [AtmajaPhoneView.cs:243](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L243).

- **LTCHAT_63d08eaee451 — Penonton — chat acak:** "SEMBUNYI DI LEMARI!"

  **Sumber:** [AtmajaPhoneView.cs:243](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L243).

- **LTCHAT_8015a7a5b95c — Penonton — chat acak:** "JANGAN NAFAS BANG!"

  **Sumber:** [AtmajaPhoneView.cs:243](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L243).

- **LTCHAT_240bfe305277 — Penonton — chat acak:** "ADA BAYANGAN DI BELAKANG RAK!"

  **Sumber:** [AtmajaPhoneView.cs:243](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L243).

- **LTCHAT_a17a510c6af0 — Penonton — chat acak:** "LANGKAHNYA DI ATAS BANG!"

  **Sumber:** [AtmajaPhoneView.cs:244](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L244).

- **LTCHAT_c99180f68277 — Penonton — chat acak:** "JANGAN LEWAT LORONG ITU!"

  **Sumber:** [AtmajaPhoneView.cs:244](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L244).

- **LTCHAT_46acb2f19c60 — Penonton — chat acak:** "ITU BAYANGAN APA?!"

  **Sumber:** [AtmajaPhoneView.cs:244](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L244).

- **LTCHAT_ce37b634248f — Penonton — chat acak:** "KAMERA NYA BERGETAR BANG!"

  **Sumber:** [AtmajaPhoneView.cs:244](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L244).

- **LTCHAT_2c75b6804166 — Penonton — chat acak:** "ADA YANG NUNGGU DI KAMAR!"

  **Sumber:** [AtmajaPhoneView.cs:245](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L245).

- **LTCHAT_c195f1cff502 — Penonton — chat acak:** "JANGAN DEKETIN CERMIN!"

  **Sumber:** [AtmajaPhoneView.cs:245](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L245).

- **LTCHAT_2049f69a0c1a — Penonton — chat acak:** "TALI ITU GERAK SENDIRI BANG!"

  **Sumber:** [AtmajaPhoneView.cs:245](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L245).

- **LTCHAT_e369ddb5eb00 — Penonton — chat acak:** "SUMURNYA BUNYI LAGI!"

  **Sumber:** [AtmajaPhoneView.cs:246](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L246).

- **LTCHAT_e08d597ee6d3 — Penonton — chat acak:** "JANGAN LIAT KE BAWAH!"

  **Sumber:** [AtmajaPhoneView.cs:246](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L246).

- **LTCHAT_d1a0cc0ca282 — Penonton — chat acak:** "ADA BAYANGAN DI JENDELA!"

  **Sumber:** [AtmajaPhoneView.cs:246](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L246).

- **LTCHAT_293f46b87b5f — Penonton — chat acak:** "BANG DI BELAKANG LU!"

  **Sumber:** [AtmajaPhoneView.cs:247](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L247).

- **LTCHAT_86e83007ca89 — Penonton — chat acak:** "LARI BANG!"

  **Sumber:** [AtmajaPhoneView.cs:247](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L247).

- **LTCHAT_1e68b368ca81 — Penonton — chat acak:** "ITU APA BARUSAN?!"

  **Sumber:** [AtmajaPhoneView.cs:247](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L247).

- **LTCHAT_e62c8070b1c1 — Penonton — chat acak:** "KAMERA JANGAN DITUTUP!"

  **Sumber:** [AtmajaPhoneView.cs:247](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L247).

- **LTCHAT_1622dfdcedaf — Penonton — chat acak:** "Mainannya masih ada semua..."

  **Sumber:** [AtmajaPhoneView.cs:254](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L254).

- **LTCHAT_6a25adf60722 — Penonton — chat acak:** "Itu kamar anaknya ya? Sedih banget lihatnya."

  **Sumber:** [AtmajaPhoneView.cs:254](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L254).

- **LTCHAT_0a74fdefe022 — Penonton — chat acak:** "Aku denger sesuatu kayak anak kecil tadi."

  **Sumber:** [AtmajaPhoneView.cs:254](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L254).

- **LTCHAT_3671de2d5a54 — Penonton — chat acak:** "Jangan sentuh mainan itu bang..."

  **Sumber:** [AtmajaPhoneView.cs:254](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L254).

- **LTCHAT_78e88404c988 — Penonton — chat acak:** "Bonekanya kayak baru dipindahin."

  **Sumber:** [AtmajaPhoneView.cs:254](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L254).

- **LTCHAT_3cfecec874e5 — Penonton — chat acak:** "Kok lantainya basah bang?"

  **Sumber:** [AtmajaPhoneView.cs:255](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L255).

- **LTCHAT_106dc52b416e — Penonton — chat acak:** "Di sini suaranya kayak menggema ya."

  **Sumber:** [AtmajaPhoneView.cs:255](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L255).

- **LTCHAT_a626d48d88bd — Penonton — chat acak:** "Airnya warnanya aneh, jangan disentuh bang."

  **Sumber:** [AtmajaPhoneView.cs:255](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L255).

- **LTCHAT_6b3ab0baeaee — Penonton — chat acak:** "Tirai itu tadi gerak sendiri kan?"

  **Sumber:** [AtmajaPhoneView.cs:255](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L255).

- **LTCHAT_dc23f19e161b — Penonton — chat acak:** "Gue nggak berani lihat cerminnya."

  **Sumber:** [AtmajaPhoneView.cs:255](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L255).

- **LTCHAT_cd8a87bcd4d2 — Penonton — chat acak:** "Lantai atas bikin merinding asli."

  **Sumber:** [AtmajaPhoneView.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L256).

- **LTCHAT_16f43ef56a11 — Penonton — chat acak:** "Bang pelan-pelan, langkah siapa itu?"

  **Sumber:** [AtmajaPhoneView.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L256).

- **LTCHAT_28b25bd4518c — Penonton — chat acak:** "Lukisan keluarganya serem juga dilihat malam-malam."

  **Sumber:** [AtmajaPhoneView.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L256).

- **LTCHAT_116bf258c360 — Penonton — chat acak:** "Jangan lewat kamar anak itu bang."

  **Sumber:** [AtmajaPhoneView.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L256).

- **LTCHAT_c9bb80965d56 — Penonton — chat acak:** "Kayaknya ada yang ngikutin dari ujung lorong."

  **Sumber:** [AtmajaPhoneView.cs:256](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L256).

- **LTCHAT_f029e9f16139 — Penonton — chat acak:** "Kamarnya gede tapi sunyi banget."

  **Sumber:** [AtmajaPhoneView.cs:257](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L257).

- **LTCHAT_afb33249e894 — Penonton — chat acak:** "Ada suara kain diseret nggak sih?"

  **Sumber:** [AtmajaPhoneView.cs:257](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L257).

- **LTCHAT_e591df6e1bb0 — Penonton — chat acak:** "Gue masih mantau bang, jangan panik."

  **Sumber:** [AtmajaPhoneView.cs:257](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L257).

- **LTCHAT_6947ff7efa60 — Penonton — chat acak:** "Itu pintu kamar utama ketutup sendiri?"

  **Sumber:** [AtmajaPhoneView.cs:257](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L257).

- **LTCHAT_0d4b6ba6ef1f — Penonton — chat acak:** "Wanginya kayak kapur barus banget."

  **Sumber:** [AtmajaPhoneView.cs:257](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L257).

- **LTCHAT_4714b758425d — Penonton — chat acak:** "Anginnya masuk mic juga bang."

  **Sumber:** [AtmajaPhoneView.cs:258](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L258).

- **LTCHAT_73e54e9f84c1 — Penonton — chat acak:** "Tuh dari balkon kelihatan halaman."

  **Sumber:** [AtmajaPhoneView.cs:258](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L258).

- **LTCHAT_c85709a0905c — Penonton — chat acak:** "Aku nggak suka tali itu gerak sendiri."

  **Sumber:** [AtmajaPhoneView.cs:258](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L258).

- **LTCHAT_19ad8e3e8b6b — Penonton — chat acak:** "Jangan deket-deket pagar bang."

  **Sumber:** [AtmajaPhoneView.cs:258](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L258).

- **LTCHAT_ea1100c1ff9e — Penonton — chat acak:** "Ada bayangan di ujung balkon nggak?"

  **Sumber:** [AtmajaPhoneView.cs:258](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L258).

- **LTCHAT_c5720d988f30 — Penonton — chat acak:** "Sinyalnya mulai pecah bang."

  **Sumber:** [AtmajaPhoneView.cs:259](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L259).

- **LTCHAT_a5453d816ab3 — Penonton — chat acak:** "Ada yang nyanyi pelan banget..."

  **Sumber:** [AtmajaPhoneView.cs:259](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L259).

- **LTCHAT_149f74e46d3b — Penonton — chat acak:** "Jahitannya banyak banget, ini dulu tempat usaha ya?"

  **Sumber:** [AtmajaPhoneView.cs:259](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L259).

- **LTCHAT_ce74c112a21f — Penonton — chat acak:** "Piringnya udah berjamur semua astaga."

  **Sumber:** [AtmajaPhoneView.cs:260](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L260).

- **LTCHAT_5be9dfa30163 — Penonton — chat acak:** "Tadi bunyi ember dari mana?"

  **Sumber:** [AtmajaPhoneView.cs:260](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L260).

- **LTCHAT_44a3c6ea0da2 — Penonton — chat acak:** "Gue merinding lihat sumurnya."

  **Sumber:** [AtmajaPhoneView.cs:260](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L260).

- **LTCHAT_893798b5589c — Penonton — chat acak:** "Itu di piring... tikus?!"

  **Sumber:** [AtmajaPhoneView.cs:261](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L261).

- **LTCHAT_4b7212afba6b — Penonton — chat acak:** "Kayak meja makan ditinggal pas orang-orangnya belum selesai makan."

  **Sumber:** [AtmajaPhoneView.cs:261](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L261).

- **LTCHAT_b983b7693dd5 — Penonton — chat acak:** "Bang Damar ninggalin tasnya di situ kenapa ya?"

  **Sumber:** [AtmajaPhoneView.cs:261](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L261).

- **LTCHAT_aff95fda910c — Penonton — chat acak:** "Jamnya masih bunyi padahal rumahnya kosong."

  **Sumber:** [AtmajaPhoneView.cs:262](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L262).

- **LTCHAT_9aa74e436da7 — Penonton — chat acak:** "Sofanya dulu pasti bagus, sekarang lembap semua."

  **Sumber:** [AtmajaPhoneView.cs:262](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L262).

- **LTCHAT_26c07caa6e06 — Penonton — chat acak:** "Bang ada yang nonton dari belakang nggak sih?"

  **Sumber:** [AtmajaPhoneView.cs:262](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L262).

- **LTCHAT_20959a40645c — Penonton — chat acak:** "Hujannya makin gede ya bang?"

  **Sumber:** [AtmajaPhoneView.cs:263](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L263).

- **LTCHAT_b1a5b57eaf4f — Penonton — chat acak:** "Yang baru masuk, ini lagi nyari Bang Damar."

  **Sumber:** [AtmajaPhoneView.cs:263](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L263).

- **LTCHAT_ddd0f3317c89 — Penonton — chat acak:** "Suaranya masih jelas di sini bang."

  **Sumber:** [AtmajaPhoneView.cs:263](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L263).

- **LTCHAT_2dc1935fbca1 — @horor_hunter:** "BARUSAN RANTAINYA PUTUS SENDIRI WOI"

  **Sumber:** [AtmajaUpperDirector.cs:311](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L311).

- **LTCHAT_34f6a5e36a49 — @dinda_ayu:** "bang jangan masuk bang sumpah jangan masuk"

  **Sumber:** [AtmajaUpperDirector.cs:321](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L321).


## Revisi komentar setelah Nyonya kalah — 26 Oktober 2026

Komentar berikut adalah teks chat layar, tidak wajib direkam sebagai VO. Penggantian teks LTCHAT_782978dc6d9c mempertahankan ID cue yang sudah ada. Reaksi ending tentang Damar tetap mengikuti adegan ending.

| ID | Penonton | Teks |
| --- | --- | --- |
| LTCHAT_P0_RELIEF_01 | @maya_sari | Udah hilang... tarik napas dulu, Bang. |
| LTCHAT_P0_RELIEF_02 | Penonton | Udah tenang sekarang... |
| LTCHAT_P0_RELIEF_03 | Penonton | Jalur keluarnya sudah kosong, Bang. |
| LTCHAT_P0_RELIEF_04 | Penonton | Kita masih di sini. Pelan-pelan aja. |
| LTCHAT_P0_RELIEF_05 | Penonton | Tarik napas dulu, tadi serem banget. |

Sumber: `AtmajaPhoneView.ReliefComments` dan transisi chat setelah `AtmajaChaseSequence.ThreatDefeated`.
