# Babak 4 Bara di Karang Setra

Revisi desain 15 Oktober 2026. Naskah, acuan mekanik, dan jalur rahasia berada dalam satu dokumen. Detail parameter bersama mengikuti Rancangan.MD.

## Acuan Babak 4 Bara di Karang Setra

Lokasi: **Desa Mati Karang Setra**. Boss: **Banaspati dan arwah Mbah Suro**. Estimasi jalur utama: **78 menit**, di luar pencarian item rahasia dan retry.

**Modul dan ancaman:** Sanity aktif, verifikasi chat, UV v2 dan air ruwatan; map pendopo dengan empat jalur penjuru.

**Urutan area:** Kos → tiga simpang hutan bambu → pendopo → Utara kuburan → Timur gubuk dukun → Barat sumur → Selatan lumbung → pendopo dan gerbang Pasar Setan.

**Tiga bukti wajib:** Anomali +66°C pada makam tumbal; aura altar di gubuk Mbah Suro; EVP mantra empat penjuru di sumur.

**Pusaka dan prasyarat:** Kendi Air Sumur Keramat, Belati Perak Ruwatan, dan Kantong Tanah Kubur Keramat.

**Eksorsisme:** Letakkan tiga pusaka di pendopo. Di tiap penjuru, UV membedakan satu obor asli dari dua umpan. Padamkan obor asli dengan air ruwatan lalu hancurkan satu ikatan melalui Blitz. Pada obor keempat, inti menyatu di pendopo; Blitz penutup di pendopo dihitung sebagai tembakan keempat, bukan kelima.

**Hasil dan transisi:** Empat segel di HP membuka gerbang kelima. Babak 5 langsung dimulai pada malam yang sama.

**Aturan khusus:** Simbol baku: obor asli memiliki lingkar segel utuh dengan inti ungu; obor palsu memiliki silang merah; tengkorak ungu pada jalan selalu berarti bahaya. Kendi dapat diisi ulang di sumur; persediaan belanja bukan satu-satunya sumber air. Sanity 0 memicu perburuan intens, bukan kematian otomatis.

## Item rahasia Babak 4

Nama: **Keping Cermin Saksi**. ID simpan: `SECRET_P4_CERMIN_SAKSI`. Status: opsional, satu dari lima bagian Arsip Saksi.

Lokasi: Relung kering di dinding Sumur Keramat sektor Barat, terpisah dari Kantong Tanah Kubur wajib.

Petunjuk lingkungan: EVP mantra menyisakan kalimat "Jangan siram mata yang menyala, dengarkan yang tidak memanggil"; pindaian UV memperlihatkan mata tertutup pada relung.

Langkah pengambilan: Saat patroli lewat, matikan layar, pindah ke relung aman, lalu rekam ketukan asli dengan EVP. Sorot simbol mata tertutup memakai UV dan putar roda sumur ke arah simbol itu. Ambil keping dari laci dinding; tidak memerlukan menyiram obor.

Isi kesaksian: Cermin merekam penolakan warga terhadap ritual 1997. Catatan di belakang keping menjelaskan bahwa Mbah Suro meninggalkan desa dan wafat seusai ritual Rusunawa 2004; arwahnya kemudian terikat di desa.

Trigger produksi: interaksi pengambilan hanya aktif setelah langkah puzzle benar. Saat diambil, tambah ID unik ke `secret_items`, tampilkan Arsip Saksi 4/5 sesuai jumlah aktual, lalu autosave. Panel status tidak boleh menganggap nomor babak sebagai jumlah item. Item tetap tersimpan setelah mati atau reload. Tidak ada saweran yang menentukan aksesnya.

## NASKAH PENUH BABAK 4: "BARA DI KARANG SETRA" (DESA MATI 'KARANG SETRA')

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
* **Indikator `Sanity & Filter Anti-Sengkala` (Otomatis di UI HP):** Menampilkan persentase kewarasan Raka (`100%`) serta fitur **Detektor Akun Janggal** di kolom *Live Chat* (komentar dari hantu penyusup akan memiliki sedikit efek *glitch* huruf atau ketidaksesuaian dengan hasil pindaian **Lensa UV Rajah**).

* **Kalibrasi `Lensa UV Rajah v2.0` (`[R]`):** Kini mampu membedakan **Api Santet Asli** (inti ungu dengan lingkar segel utuh) dan **Api Jebakan Ilusi** (inti merah dengan rajah silang).

2. **Belanja Persediaan Ekspedisi:**
* **5x Kemenyan Bakar** (Sangat wajib di Babak 4 untuk menahan penurunan *Sanity* dan menghentikan halusinasi visual/audio; tekan **`[H]`** untuk membakar kemenyan).

* **3x Baterai Cadangan / Powerbank Besar**.

* **2x Kantong Air Keramat / Garam Ruwatan** (Digunakan untuk memadamkan Obor Santet Mbah Suro di fase klimaks).

* Pemain berinteraksi dengan pintu keluar kamar kos → memilih **`[BERANGKAT KE DESA MATI KARANG SETRA - PUKUL 23:45 WIB]`**.

---

### ADEGAN 2: Hutan Bambu Karang Setra — Distorsi Sanity & Infiltrasi Live Chat (Menit 10:00 – 28:00)

**Kondisi Awal:** Pukul **23:53 WIB**. Raka memarkir motornya di ujung jembatan kayu lapuk yang membatasi jalan tanah dengan **Hutan Bambu Desa Karang Setra**. Angin malam berhembus kencang menggesek batang-batang bambu tinggi (**`"KREK... KREEEK..."`**), sementara abu sisa pembakaran berterbangan di udara seperti salju hitam.

#### 1. Menyalakan Siaran & Kontrak Saweran Keempat

* Pemain menekan **`[TAB]`** untuk mengangkat Smartphone dan menekan **`[GO LIVE]`**.

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
3. Di **Persimpangan Kedua & Ketiga**, hantu di *Live Chat* bahkan meniru nama *username* penonton asli (misal `@rizky_pratama_` dengan tambahan *underscore*) untuk menyuruh Raka masuk ke gubuk bambu yang salah. Pemain wajib terus memverifikasi setiap saran *chat* menggunakan **Lensa UV Rajah (`[R]`)** atau **Lensa AR (`[Q]`)** dan menekan **`[H]` (Bakar Kemenyan)** untuk mengembalikan *Sanity* ke **100%**!

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
1. **Radar Detak Jantung / Sanity:** Berbeda dengan hantu lain yang mencari cahaya senter, Banaspati tertarik pada **getaran rasa takut (*Sanity* di bawah 40%)** dan **suara langkah kaki di ranting bambu kering**. Jika *Sanity* Raka drop dan terdengar bunyi detak jantung keras, Banaspati akan langsung terbang cepat ke arah posisi Raka! Pemain wajib menjaga *Sanity* dengan membakar **Kemenyan (`[H]`)**.

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
* Pemain membakar **Kemenyan Terakhir (`[H]`)** untuk menjernihkan pandangan, menyorotkan **`Lensa UV Rajah` (`[R]`)** untuk menemukan **Obor Asli ke-4** di balik kepulan asap, lalu memadamkannya! Kendi dapat diisi ulang di sumur; apabila kemenyan habis, celah perlindungan netral dekat lumbung menjaga jalur tetap bisa diselesaikan.

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

## Trigger dan kriteria selesai Babak 4

- [ ] Semua jalur wajib dapat dilewati dengan perlengkapan tetap, tanpa membeli item opsional.
- [ ] Tiga bukti unik tersimpan dan dapat dibaca setelah reload; foto duplikat tidak menaikkan hitungan.
- [ ] Arena terkunci hanya setelah Jurnal dan pusaka babak lengkap; keluar dari inspeksi tidak memulai boss.
- [ ] Gagal Blitz saat perisai terbuka mengulang kesempatan cahaya; tidak menghabiskan seluruh arena.
- [ ] Item `SECRET_P4_CERMIN_SAKSI` dapat diperoleh, disimpan sekali, dan tetap ada setelah mati/reload.
- [ ] Sebelum transisi, status collectible babak terlihat; petunjuk tetap tersedia jika chat terlewat.
- [ ] Save akhir babak memuat segel lokasi, upgrade, inventori, uang tersedia, Sanity, dan Arsip Saksi secara konsisten.
