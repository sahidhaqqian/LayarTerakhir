# Babak 5 Layar Terakhir

Revisi desain 15 Oktober 2026. Naskah, acuan mekanik, dan jalur rahasia berada dalam satu dokumen. Detail parameter bersama mengikuti Rancangan.MD.

## Acuan Babak 5 Layar Terakhir

Lokasi: **Pasar Setan**. Boss: **Penonton_Terakhir**. Estimasi jalur utama: **75 menit**, di luar pencarian item rahasia dan retry.

**Modul dan ancaman:** Seluruh modul HP, kios perbekalan, tiga sektor distorsi, pertarungan tiga tahap, dan empat cabang ending.

**Urutan area:** Gerbang dan kios → koridor beku → jembatan gilingan bambu → replika ruang jahit Atmaja → panggung lelang → tiga inti boss → pilihan akhir.

**Tiga bukti wajib:** Server batu dan kabel roh melalui AR; kontrak bertanda mata terbuka melalui suhu di luar rentang serta UV; EVP gabungan suara penonton.

**Pusaka dan prasyarat:** Empat segel dari Babak 1–4 menjadi syarat cerita untuk membuka arena. Lima item rahasia menjadi syarat terpisah untuk secret ending.

**Eksorsisme:** Tahap A: UV memilih wujud asli, lampu panggung membuka perisai, Blitz menghancurkan inti pertama. Tahap B: Selfie dan ThermoKlenik melacak sosok asli, flare atau lampu cadangan membuka inti kedua. Tahap C: berlindung dari gelombang api, AR mengungkap tiga kabel, interaksi memutusnya, Blitz memecahkan inti ketiga. Setelah itu masuk state pilihan ending.

**Hasil dan transisi:** Damar bebas dari kurungan; aksi pemain menentukan Bad, Normal, True, atau Secret Ending.

**Aturan khusus:** Replika hantu adalah konstruksi memori, bukan boss yang hidup kembali. Semua sektor dan arena memiliki checkpoint, tanpa menjadi Safe Room. Sebelum arena final, cermin arsip menampilkan 0–5 item ditemukan dan memberi kesempatan kembali ke kios untuk item kelima.

## Item rahasia Babak 5

Nama: **Pita Rekaman Pertama**. ID simpan: `SECRET_P5_PITA_PERTAMA`. Status: opsional, satu dari lima bagian Arsip Saksi.

Lokasi: Kios Arsip Bisu di ceruk kanan gerbang sebelum sektor distorsi pertama; bisa diperiksa tanpa membayar.

Petunjuk lingkungan: Manekin menunjuk kursi penonton kosong, sementara EVP menangkap "Saksi tidak membeli korban". Tanda mata tertutup terlihat melalui AR.

Langkah pengambilan: Matikan layar selama 3 detik di depan kursi untuk menghentikan chat palsu, nyalakan AR untuk melihat laci tersembunyi, lalu rekam satu kalimat EVP asli. Laci terbuka dan pita dapat diambil. Kios perbekalan tetap berfungsi terpisah.

Isi kesaksian: Pita merekam pertunjukan pertama Pasar Setan dan menyatakan bahwa nama korban yang dikembalikan melalui kesaksian bebas dapat membatalkan hak kepemilikan entitas.

Trigger produksi: interaksi pengambilan hanya aktif setelah langkah puzzle benar. Saat diambil, tambah ID unik ke `secret_items`, tampilkan Arsip Saksi 5/5 sesuai jumlah aktual, lalu autosave. Panel status tidak boleh menganggap nomor babak sebagai jumlah item. Item tetap tersimpan setelah mati atau reload. Tidak ada saweran yang menentukan aksesnya.

## NASKAH PENUH BABAK 5 (KLIMAKS): "LAYAR TERAKHIR" (PASAR SETAN)

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

### ADEGAN 2: Labirin Distorsi Panca-Sengkala — Ujian Seluruh Modul HP (Menit 15:00 – 40:00)

Menggunakan mekanik Kombinasi Seluruh Mekanik (AR, Kamera Selfie, ThermoKlenik, EVP Recorder, Lensa UV Rajah, Stealth, & Infiltrasi Live Chat) dalam 3 koridor distorsi ruang-waktu.

**Kondisi Awal:** Untuk membuka jalan ke **Panggung Lelang Utama**, Raka harus melewati **Labirin Distorsi**—sebuah lorong pasar malam yang ruangannya terus berubah menggabungkan lokasi Babak 1 hingga Babak 4.

#### 1. Sektor Distorsi Pertama: Koridor Beku (Kamera Selfie `[V]` × `ThermoKlenik` `[T]`)

Tenda kain pasar tiba-tiba menyempit menjadi gabungan **Koridor Beton Rusunawa Griya Asri** dan **Kamar Mayat RSUD Sido Waras** yang kedap suara. Di ujung lorong terdapat **3 Pintu Cermin**, hanya satu pintu yang asli.

* **Eksekusi Gameplay:**
1. Bayangan **Kuntilanak Merah** muncul menempel tepat di belakang punggung Raka! Pemain wajib langsung menekan **`[V]` (Kamera Selfie)** untuk menahan pergerakannya dari belakang sambil terus berjalan maju.

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

### ADEGAN 4: Final Boss Fight — Eksorsisme Multi-Fase `Penonton_Terakhir` (Menit 50:00 – 68:00)

Menggunakan mekanik Pertarungan Multi-Fase tanpa Safe Room yang mengombinasikan seluruh mekanik game dari Prolog hingga Babak 4.

#### 1. Kemunculan Boss Utama: Entitas `Penonton_Terakhir` ("Sang Dalang Layar")

Begitu Jurnal Klenik terkonfirmasi, seluruh layar proyeksi di langit Pasar Setan menyatu membentuk sosok mengerikan setinggi 4 meter yang turun ke atas panggung: **Entitas `Penonton_Terakhir`—sosok humanoid tinggi kurus berjubah kain layar bioskop yang terbakar, memiliki puluhan tangan pucat yang keluar dari balik jubahnya, dan bagian kepalanya berupa **tumpukan lensa kamera tua serta cermin retak yang memancarkan cahaya merah berkedip**!

* **Penonton_Terakhir (Suara Gema Ribuan Manusia):** "Kau ingin tahu siapa aku, Raka Wijaya? Akulah alasan kanalmu hidup kembali. Akulah satu juta pasang mata yang menolak tidur demi melihat sahabatmu mati di depan kamera!"

#### 2. Pertarungan 3 Fase (Ujian Puncak Seluruh Mekanik)

Untuk menghancurkan **3 Inti Segel `Penonton_Terakhir`, pemain harus melewati 3 Fase berturut-turut di arena Panggung Lelang:

* **FASE 1 — Fase Distorsi Ilusi (`Lensa UV Rajah` `[R]` + `Generator Lampu Sorot` + `Blitz`):**
1. `Penonton_Terakhir` menyelimuti dirinya dengan **Kabut Sengkala Pekat** dan membelah diri menjadi **4 Sosok Kembar** yang mengepung Raka sambil memunculkan jebakan lantai runtuh di panggung kayu.

2. Pemain wajib menyalakan **`Lensa UV Rajah` (`[R]`)**: untuk menghindari lantai ber-rajah ungu sekaligus melihat **sosok mana yang memiliki benang merah terhubung ke kurungan Damar** (itulah wujud aslinya).

3. Pemain memancing wujud asli tersebut ke dekat **Generator Lampu Sorot Panggung**, menyalakan sakelarnya untuk membakar Kabut Sengkala-nya, lalu menembakkan **Jepretan Blitz (`[Klik Kiri]`)** tepat ke kepala lensanya! (**Inti Pertama Pecah!**)

* **FASE 2 — Fase Teror Punggung & Chat Palsu (`Kamera Selfie` `[V]` + `ThermoKlenik` `[T]` + `Flare` `[G]` + `Blitz`):**
1. `Penonton_Terakhir` menghancurkan seluruh lampu sorot panggung hingga arena gelap gulita! Ia berubah menjadi mode tak kasat mata dari depan (seperti Kuntilanak Merah) sambil membanjiri **Live Chat dengan komentar palsu** yang menyuruh Raka menoleh ke arah yang salah.

2. Pemain wajib menekan **`[H]` (Bakar Kemenyan)** untuk menjaga *Sanity*, lalu mengangkat HP dalam **Mode Kamera Selfie (`[V]`)** sambil memantau suhu di **`ThermoKlenik` (`[T]`)**.

3. Begitu suhu turun drastis ke **`-30°C`** dan sosok `Penonton_Terakhir` terlihat di layar Selfie sedang membuka rahang lensanya tepat di belakang leher Raka, pemain menekan **`[G]` untuk menjatuhkan Flare Merah Darurat**!

4. Cahaya Flare menghancurkan perisai kabutnya, dan pemain langsung menembakkan **Jepretan Blitz (`[Klik Kiri]`)** lewat mode Selfie! (**Inti Kedua Pecah!**)

* **FASE 3 — Fase Overload Sinyal (`Lensa AR` `[Q]` + `Stealth` `[F]` + `Blitz`):**
1. Dengan tubuh yang mulai retak, `Penonton_Terakhir` memanggil gelombang **Bola Api Banaspati** yang menyapu panggung.

2. Pemain harus mematikan layar HP (`[F]`) dan merunduk di balik peti kayu panggung saat gelombang api lewat, lalu menyalakan **Lensa AR (`[Q]`)** untuk melihat dan memutus **3 Kabel Roh** yang mengikat kurungan Damar.

3. Begitu ketiga kabel roh putus, kurungan Damar terbuka! `Penonton_Terakhir` menjerit murka dengan **Inti Ketiga** yang terbuka lebar di dadanya. Pemain melepaskan **Jepretan Blitz** dan menjatuhkan entitas tersebut berlutut di tengah panggung!

---

### ADEGAN 5: Penentuan 4 Ending — "Layar Terakhir" (Menit 68:00 – 75:00)

Sesuai Master GDD, bagian ini menentukan 4 Ending permainan berdasarkan keputusan dan eksekusi mekanik pemain di momen terakhir.

**Kondisi Puncak:** `Penonton_Terakhir` berlutut sekarat namun belum musnah sepenuhnya. Damar jatuh tersungkur di lantai panggung di sebelah Raka. Smartphone di tangan kiri Raka retak parah; kapasitor ritual mengunci **Baterai tersisa 5% (Hanya cukup untuk 1 tindakan terakhir)**.

* **Penonton_Terakhir (Suara Berbisik di Kepala Raka):** "Jika kau membunuhku sekarang, uang Rp500.000.000 dan satu juta penontonmu akan lenyap... atau jika kau mematikan siaranmu, gerbang ini akan runtuh menguburmu selamanya. Pilih, Raka... siapa yang akan menjadi tontonan terakhir malam ini?"

Di layar muncul **4 Pilihan Eksekusi Mekanis (cabang keempat hanya aktif bila Arsip Saksi lengkap)** (Pemain bebas menggerakkan karakter dan melakukan salah satu dari 3 aksi berikut):

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
2. Pemain berjalan memapah bahu Damar di depan portal cermin, lalu menekan **`[V]` (Mengaktifkan Kamera Depan / Selfie Mode)** lalu **`[Q]` (`Lensa AR`)** dan **`[R]` (`Lensa UV Rajah`)** secara berurutan untuk mengisi kalibrasi sinkronisasi khusus ending. AR dan UV tetap saling menggantikan di gameplay biasa!

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

Pemicu ending dasar tetap tersedia bila secret terbuka. BAD: bidik Damar dalam kamera belakang, tahan bidikan kontrak 2 detik lalu LMB setelah label TUMBAL muncul. NORMAL: tahan F 5 detik untuk memutus siaran dan menghancurkan HP; Damar dan penonton selamat, Raka tertinggal. TRUE: papah Damar ke portal, pilih V, aktifkan Q lalu R untuk mengisi kalibrasi sinkronisasi ritual, kemudian LMB pada inti entitas yang terlihat di Selfie; Raka, Damar, dan penonton selamat. SECRET: lima item lengkap, validasi Cermin Saksi, tahan E di cermin lalu LMB pada reticle emas. Input ending hanya diterima pada state FINAL_CHOICE; sebelum itu berlaku kontrol biasa. Ending memakai satu enum tunggal dan kunci transisi atomik agar dua cutscene tidak terpicu bersamaan.


## Trigger dan kriteria selesai Babak 5

- [ ] Semua jalur wajib dapat dilewati dengan perlengkapan tetap, tanpa membeli item opsional.
- [ ] Tiga bukti unik tersimpan dan dapat dibaca setelah reload; foto duplikat tidak menaikkan hitungan.
- [ ] Arena terkunci hanya setelah Jurnal dan pusaka babak lengkap; keluar dari inspeksi tidak memulai boss.
- [ ] Gagal Blitz saat perisai terbuka mengulang kesempatan cahaya; tidak menghabiskan seluruh arena.
- [ ] Item `SECRET_P5_PITA_PERTAMA` dapat diperoleh, disimpan sekali, dan tetap ada setelah mati/reload.
- [ ] Sebelum transisi, status collectible babak terlihat; petunjuk tetap tersedia jika chat terlewat.
- [ ] Save akhir babak memuat segel lokasi, upgrade, inventori, uang tersedia, Sanity, dan Arsip Saksi secara konsisten.
