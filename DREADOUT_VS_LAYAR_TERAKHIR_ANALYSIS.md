# DreadOut vs Layar Terakhir — Audit Komparatif

Tanggal: 18 Oktober 2026. Baseline Layar Terakhir: commit `3c17ead55e4a41d1fe49a05b8d9993e8f392dcf8`, setelah revisi grafis terakhir. Dokumen ini adalah analisis dan usulan; tidak ada implementasi yang diotorisasi melalui laporan ini.

## 1. Executive Summary

**Layar Terakhir belum terbukti setara DreadOut secara keseluruhan.** Pada gambar yang diperiksa, DreadOut lebih konsisten dalam komposisi horor, hubungan karakter dengan pencahayaan, dan penyatuan kamera dengan dunia. Layar Terakhir sudah memiliki rangkaian prolog yang dapat diselesaikan melalui pengujian terarah, tetapi mutu antarlokasi belum konsisten. Efek rendering modern sudah tersedia; persoalan terbesar berikutnya adalah pengarahan visual, performa karakter, kedalaman interaksi, dan penyampaian emosi.

**VO Layar Terakhir merupakan masalah P0.** Pemilik proyek melaporkan suara AI terdengar datar, tanpa intonasi dan emosi. Ini adalah bukti playtest pengguna, bukan dugaan dari nama file. Naskah produksi juga masih menandai dialog sebagai menunggu rekaman. Panggilan panik Damar, respons Raka, dan ancaman Nyonya tidak akan mencapai dampak yang diinginkan hanya dengan menaikkan volume atau menambahkan reverb.

Mekanik kita bukan kosong: AR memerlukan arah bidik dan pemeriksaan penghalang; foto bukti menghabiskan daya; basement memiliki deteksi pandangan, suara gerak, dan cahaya layar; ada persembunyian, pengejaran, checkpoint, serta ekonomi saweran. Namun aksi foto umum masih terpisah dari aksi bukti dan Blitz. Blitz akhir adalah adegan terpandu dengan arah pandang yang diatur game, sehingga belum menjadi sistem pertarungan kamera yang berlaku luas.

Keunggulan yang layak dipertahankan adalah identitas streamer, tekanan ekonomi, HP dengan aplikasi fungsional, interaksi Bu Sri, lingkungan kos, dan kombinasi stealth dengan keputusan menyalakan layar. Menambah fitur sebanyak mungkin tidak otomatis meningkatkan kualitas. Prioritasnya adalah membuat satu rangkaian pendek terasa utuh, dapat dipahami, emosional, dan stabil sebelum memperluasnya.

Ada batas penting: instalasi lokal ternyata **DreadOut Remastered Collection**, bukan executable asli 2014. Kampanye DreadOut 1 dapat dibedakan dari Keepers of the Dark melalui metadata scene. Executable Windows itu belum dimainkan dalam audit ini karena Wine/Proton tidak tersedia. Karena itu, penilaian mekanik DreadOut bertumpu pada manual resmi, sumber pengembang, dan metadata pendukung; bukan klaim telah menamatkan atau menguji langsung game tersebut. FPS, respons kontrol, kualitas audio secara berdampingan, dan keandalan gameplay kedua game belum bisa diberi peringkat yang sah.

## 2. Metodologi dan keterbatasan pengujian

### 2.1 Objek yang benar-benar ditemukan

| Objek | Lokasi dan identitas | Batas penggunaan |
| --- | --- | --- |
| Lokasi yang diminta | `/project/dreadout`; direktori `/project` tidak ditemukan pada host ini | Tidak mengaku telah memeriksa path tersebut |
| Instalasi DreadOut yang tersedia | `/root/Projects/DreadOut_Remastered_Collection`; `app.info` menyebut Digital Happiness / DreadOut Remastered Collection | Dibaca lokal; executable tidak dijalankan |
| Identitas executable lokal | `DreadOut Remastered Collection.exe`; SHA-256 `c704793ceeecc2fbca126f15d375d81ac5a588e0cf8c2c3648367636caabce4c` | Pengenal sampel audit, bukan verifikasi keaslian distribusi atau nomor patch |
| Layar Terakhir | `/root/Projects/LayarTerakhir-main`; tiga scene prolog: KamarKos, KoridorKos/Melati, RumahAtmaja | Yang dibandingkan adalah kualitas prolog saat ini, bukan babak masa depan dalam dokumen desain |

Dua acuan dibedakan: **DreadOut 1 asli** untuk prinsip mekanik dan galeri resmi 2014; **kampanye DreadOut 1 dalam Remastered Collection** untuk identitas instalasi serta perubahan yang dinyatakan pengembang. Keepers of the Dark tidak dihitung sebagai fitur wajib DreadOut 1. Delapan wilayah Mirror Realm dan jumlah musuh dari produk tersebut tidak dipakai untuk menilai kelengkapan prolog kita.

### 2.2 Label kekuatan bukti

- **[T-L] Terverifikasi lokal:** isi file, metadata, konfigurasi, atau jalur kode yang benar-benar diperiksa. Membuktikan struktur/aturan tertulis; belum tentu membuktikan perilaku saat dimainkan.
- **[T-D] Terverifikasi dokumentasi resmi:** fitur yang dijelaskan pembuat game/manual. Tidak sama dengan pengujian executable yang tersedia.
- **[T-V] Terverifikasi visual:** terlihat pada gambar yang diperiksa. Penilaian bagus/buruk tetap merupakan penilaian editorial atas frame tersebut.
- **[T-A] Terverifikasi artefak pengujian terdahulu:** receipt, screenshot, dan hasil tes yang sudah tersedia. Audit ini membacanya kembali, bukan menjalankan ulang tesnya.
- **[T-U] Terverifikasi sebagai laporan pengguna:** pengalaman playtest yang dinyatakan pemilik proyek. Tidak disamarkan sebagai reproduksi langsung oleh auditor.
- **[I] Inferensi:** kemungkinan dampak atau diagnosis dari bukti yang tersedia; perlu konfirmasi tambahan.
- **[B] Belum bisa dinilai:** memerlukan eksekusi, pendengaran langsung, profiler, hardware, atau pengujian pemain.

### 2.3 Pemeriksaan yang dilakukan

1. Menelusuri kode Layar Terakhir melalui Graphify, lalu membaca bagian source yang relevan. Pemeriksaan Graphify read-only lulus: 150 file kode, 2.048 node, 7.246 edge, tanpa aset yang diindeks.
2. Memeriksa identitas instalasi DreadOut, header arsip UnityFS, BuildSettings, nama kelompok animasi, dan kunci lokalisasi secara lokal. Tidak mendekompilasi atau menyalin implementasi proprietary ke proyek.
3. Membaca manual resmi PDF 29 halaman. Halaman kontrol, kondisi Linda, Limbo, kamera, puzzle, inventori, dan galeri diperiksa secara visual karena PDF tidak memiliki lapisan teks yang dapat diekstrak.
4. Memeriksa delapan frame Layar Terakhir dari pass grafis terakhir, ditambah frame kamar anak, basement, Blitz, serta jongkok. Membandingkannya dengan contoh resmi DreadOut yang relevan.
5. Memeriksa konfigurasi pipeline dan profil visual secara terarah dengan Unity prefab parser, serta mencocokkannya dengan source authoring. Referensi yang tidak di-resolve parser tidak dianggap sebagai aset hilang.
6. Membaca receipt prolog, laporan build, dokumentasi arsitektur, dan naskah rekaman yang sudah ada. Tidak membuat build, mengubah save pemain, mengimpor aset, menjalankan builder scene, atau memperbarui konfigurasi game.

### 2.4 Sumber bukti yang dapat ditelusuri

| ID | Sumber | Yang didukung |
| --- | --- | --- |
| D1 | [Halaman resmi DreadOut 2014](https://store.steampowered.com/app/269790/DreadOut/) | Identitas, fitur utama, galeri asli |
| D2 | [Manual resmi DreadOut 2.0](https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/269790/manuals/DreadOut_game_manual_2.0.pdf) | Mekanik yang didokumentasikan; nomor halaman di laporan adalah urutan halaman PDF |
| D3 | [Halaman resmi Remastered Collection](https://store.steampowered.com/app/3500800/DreadOut_Remastered_Collection/) dan [penjelasan pengembang, 30 Juni 2025](https://steamcommunity.com/app/3500800/allnews/) | Pemisahan dua produk, pembaruan visual dan kontrol yang diklaim pengembang |
| D4 | Instalasi lokal: `app.info`, header `data.unity3d`, objek BuildSettings, TextAsset `Localization` | Identitas Unity/build, pemisahan scene kampanye, keberadaan entri fitur; bukan pembuktian gameplay |
| L1 | [Frame grafis terbaru](Audit/Visuals/Dreadout-Look-Comparison.html) | Delapan pasangan sebelum/sesudah revisi kita; bukan A/B terkontrol melawan DreadOut |
| L2 | [Receipt opening](Audit/Visuals/Dreadout-Look-Opening/player-audit.json), [receipt prolog](Audit/Visuals/Dreadout-Look-Prolog/player-audit.json), [validasi akhir terdahulu](Audit/dreadout-look-final-validation.json) | Jalur terarah selesai, platform, batas pengujian |
| L3 | [ShoulderCameraRig](Assets/_LayarTerakhir/src/Player/ShoulderCameraRig.cs), [FirstPersonController](Assets/_LayarTerakhir/src/Player/FirstPersonController.cs), [AnimatedRakaAvatar](Assets/_LayarTerakhir/src/Player/AnimatedRakaAvatar.cs) | Kamera, gerak, pemilihan dan kecepatan klip animasi |
| L4 | [SmartphoneController](Assets/_LayarTerakhir/src/Smartphone/SmartphoneController.cs), [AtmajaPhoneView](Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs), [PhoneDesktop](Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs) | Daya, layar, render feed, aplikasi, shutter |
| L5 | [AtmajaDirector](Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs), [AtmajaUpperDirector](Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs) | AR, jam, bukti, syarat progres dan biaya foto |
| L6 | [AtmajaBasementHorror](Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs), [AtmajaChaseSequence](Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs) | Deteksi, pathing, persembunyian, pengejaran dan Blitz |
| L7 | [SceneAudioProfile](Assets/_LayarTerakhir/src/Audio/SceneAudioProfile.cs), [KosSceneAudio](Assets/_LayarTerakhir/src/Audio/KosSceneAudio.cs), [naskah rekaman](Audit/Recording/README.md), [daftar dialog](Audit/Recording/dialogue-recording.csv) | Infrastruktur audio serta status rekaman yang belum final |
| L8 | [CampaignState](Assets/_LayarTerakhir/src/Core/CampaignState.cs), [GameShell](Assets/_LayarTerakhir/src/UI/GameShell.cs), [arsitektur](ARCHITECTURE.MD) | Save, ekonomi, pengaturan dan hubungan sistem |
| L9 | [DreadoutVisualRevision](Assets/_LayarTerakhir/src/Editor/DreadoutVisualRevision.cs), [KosPipeline](Assets/_LayarTerakhir/assets/Art/Generated/KosPipeline.asset), [receipt authoring](Audit/dreadout-look-authoring.json) | Pipeline dan batas perubahan grafis sebelumnya |
| L10 | [AtmajaMirrorReflection](Assets/_LayarTerakhir/src/Prolog/AtmajaMirrorReflection.cs), [AtmajaHandycamPlayback](Assets/_LayarTerakhir/src/Prolog/AtmajaHandycamPlayback.cs), [HudPresentation](Assets/_LayarTerakhir/src/UI/HudPresentation.cs) | Kamera tambahan, playback dan hierarki HUD |
| L11 | [MelatiShop](Assets/_LayarTerakhir/src/Prolog/MelatiShop.cs), [MelatiShopkeeper](Assets/_LayarTerakhir/src/Prolog/MelatiShopkeeper.cs) | Bu Sri, percakapan dan transaksi |
| U1 | Umpan balik pemilik proyek dalam percakapan audit ini: suara AI tidak memiliki intonasi dan emosi | Masalah performa dialog yang dilaporkan pemain |

### 2.5 Hal yang tidak dibuktikan audit ini

- Tidak ada sesi bermain baru DreadOut, uji kontrol berdampingan, pengukuran input latency, atau pengujian sampai ending DreadOut.
- Screenshot resmi dapat berasal dari versi, kamera, dan pengaturan berbeda. Resolusinya juga berbeda dari capture kita. Tidak dilakukan perbandingan ketajaman piksel, skor kemiripan, atau klaim FPS dari gambar.
- Tidak ada sesi dengar A/B terkontrol. Kritik VO kita berasal dari U1 dan status produksi L7; kualitas akting, mixing, serta soundtrack DreadOut belum diaudit langsung.
- Receipt Layar Terakhir berasal dari Linux dengan llvmpipe software renderer. Traversal diarahkan fixture, beberapa timer opening dipercepat, dan waktu dibekukan saat capture. Ini tidak membuktikan pacing alami, kenikmatan bermain, atau performa Windows/Iris Xe.
- Test suite terdahulu 88/88, tes terarah 9/9, dan Python 19/19 tetap merupakan hasil historis dengan urutan revisi yang dijelaskan L2; bukan tes baru pada sesi laporan ini.
- Nama field seperti `sanity`, daftar mode input, nama animasi, atau entri lokalisasi tidak cukup untuk menyatakan satu sistem sudah berfungsi penuh.

## 3. Analisis grafik dan visual kedua game

### 3.1 Apa yang kuat pada acuan DreadOut

**[T-V/I]** Pada [contoh koridor resmi](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/269790/ss_5b90da59f115b062ed5b561728b0558ad39c05b5.1920x1080.jpg), susunan pintu, kusen, lantai, dan kegelapan ujung lorong menghasilkan kedalaman. Pada [contoh karakter di dekat pintu](https://shared.akamai.steamstatic.com/store_item_assets/steam/apps/269790/ss_fc8f7becf69292779af0da8fabc51c496f8f006f.1920x1080.jpg), pakaian dan kulit masih menyatu dengan warna lingkungan. Kualitas yang berguna sebagai acuan adalah konsistensi bentuk, cahaya, dan ruang negatif. Sebagian bayangan sangat gelap dan detail permukaan terbatas; tidak semua kekurangan gambar acuan patut diikuti. [D1]

**[T-D]** Pengembang menjelaskan bahwa Remastered memperbarui tekstur, shader, pencahayaan, AO, bloom, SMAA, dan pengendalian kamera. Pernyataan itu menjelaskan perubahan produk; tidak memberikan hasil profiler untuk hardware kita, dan tidak membuktikan instalasi lokal menggunakan patch terbaru. Motion blur yang tersedia pada acuan juga bukan alasan otomatis untuk menambah blur pada Layar Terakhir. [D3]

### 3.2 Layar Terakhir: temuan visual konkret

| Area | Temuan yang terlihat | Penilaian dan implikasi |
| --- | --- | --- |
| Kamar awal | [Frame kamar](Audit/Visuals/Dreadout-Look-After/01-kos-room.png): furnitur, barang meja, tirai dan noda memiliki hubungan ruang yang cukup jelas | **[T-V/I]** Salah satu area paling koheren. Cocok menjadi patokan internal untuk lokasi berikutnya |
| Koridor kos | [Frame koridor](Audit/Visuals/Dreadout-Look-After/02-kos-corridor.png): pintu dan lampu membentuk pengulangan yang rapi; avatar cukup menonjol | **[T-V/I]** Terbaca, tetapi membutuhkan variasi jejak penghuni dan pemisahan cahaya yang lebih alami. Kos berpenghuni tidak perlu digelapkan seperti rumah kosong |
| Jalan Melati | [Frame jalan](Audit/Visuals/Dreadout-Look-After/03-melati-road.png): rumah bervariasi, Bu Sri terlihat; sumbu jalan panjang dan tepi halaman relatif seragam | **[T-V/I]** Jumlah rumah sudah membantu kepadatan, tetapi pengalaman masih terasa seperti deretan fasad. Perlu aktivitas kecil, kedalaman pekarangan dan landmark |
| Samping kos | [Frame eksterior](Audit/Visuals/Dreadout-Look-After/04-kos-exterior.png): batas tanah/lingkungan jauh tampak mendadak dari sudut ini | **[T-V/I]** Kontinuitas dunia lemah pada frame tersebut. Jangan menyatakan seluruh kawasan kosong; masalahnya adalah cakupan pandangan yang belum ditata |
| Warung | [Frame Bu Sri](Audit/Visuals/Dreadout-Look-After/05-warung-phone.png): penjual dan barang sudah ada, tetapi stok rak terlihat sederhana dan tulisan dekat lampu sulit terbaca | **[T-V/I]** Fondasi fungsi terpenuhi; identitas warung perlu prop, kemasan, signage dan pencahayaan yang lebih terarah |
| Fasad Atmaja | [Frame kedatangan](Audit/Visuals/Dreadout-Look-After/06-atmaja-exterior.png): massa rumah terbaca, detail fasad tertahan dalam gelap, latar jauh minim | **[T-V/I]** Perlu komposisi kedatangan dan landmark jendela/teras. Bukan menaikkan exposure seluruh scene |
| Ruang makan | [Frame ruang makan](Audit/Visuals/Dreadout-Look-After/07-atmaja-phone.png): kayu/dinding memiliki detail, piring berulang, pertemuan beberapa permukaan masih tampak keras | **[T-V/I]** Material individual cukup membantu, tetapi skala, keausan, kontak dan cerita penempatan barang belum seragam |
| Lantai dua | [Frame lorong](Audit/Visuals/Dreadout-Look-After/08-atmaja-upper.png): bidang dekat HP menghasilkan hotspot putih besar; tekstur dinding dan lantai berulang kuat | **[T-V/I]** Ada detail yang kehilangan bentuk pada sorotan dekat. Mengurangi detail tekstur atau menaikkan kontras global bukan penyelesaian yang tepat |
| Kamar anak | [Frame kamar Bima](Audit/Visuals/Dreadout-Look-Prolog/02a-child-bedroom-layout.png): kasur merah sangat dominan, ruang sempit, palet furnitur belum menyatu | **[T-V/I]** Hierarki fokus belum mendukung cerita anak/objek bukti. Frame tidak membuktikan seluruh collider atau prop menembus |
| Ancaman akhir | [Frame Blitz](Audit/Visuals/Dreadout-Look-Prolog/19-emergency-blitz-standoff.png): wajah/pakaian sangat terang; pose dan bentuk siluet kurang mengancam | **[T-V/I]** Puncak horor membutuhkan kualitas model, pose dan cahaya lebih baik. Diagnosis penyebab tunggal belum terbukti |

Revisi grafis terakhir memang menambah HDR, grading, SMAA, material aspal dan variasi cahaya. Source serta receipt menunjukkan collider dipertahankan. **[T-L/T-A]** Akan tetapi, pada frame yang diperiksa, penambahan teknologi rendering belum menyelesaikan perbedaan mutu antararea. **[I]** [L9]

### 3.3 Karakter, animasi, kamera dan VFX

**Karakter. [T-L/T-V/I]** Raka memakai avatar authored dengan klip Idle/Walk/Run/Crouch/Phone/Riding, bukan selalu figur primitive fallback. Pemilihan klip terutama berdasarkan kecepatan dan status; kecepatan playback disesuaikan dengan gerak. Ini fondasi yang berguna, tetapi belum menunjukkan perpaduan animasi arah gerak, pivot, kaki di anak tangga, atau transisi tangan yang lengkap. Jongkok pada satu screenshot membuktikan postur muncul, bukan membuktikan siklus geraknya natural. Keluhan lama tentang leher, celana, dan sisa jaket perlu reproduksi pada build saat ini; audit ini tidak mengaku telah melihat ulang semua cacat tersebut. [L3]

**Kamera. [T-L]** Third-person menggunakan sphere cast untuk mendekat saat terhalang dan menyembunyikan render tubuh saat jarak sangat dekat. Mode first-person dapat dipilih terpisah. Ini sudah lebih dari sekadar kamera yang diletakkan di belakang kapsul. Namun posisi kamera diatur langsung tiap refresh; respons di pintu, dinding, tangga, dan perpindahan mode perlu dinilai saat bergerak. Rasa tersentak atau pusing tidak dapat dipastikan dari screenshot. [L3]

**HP. [T-L/T-V]** Panel layar dibangun sebagai UI, sementara renderer handset bawaan disembunyikan. Feed portrait 256×448 menggunakan kamera lain yang tidak merender bayangan maupun post-processing utama. Hasilnya terlihat sebagai dua presentasi ruang yang berdampingan; frame menampilkan perbedaan komposisi dan pencahayaan. **[I]** Perbedaan itu dapat mengganggu pemain saat mencari petunjuk kecil atau mengarahkan sasaran. Identitas HP streamer tetap dapat dipertahankan dengan memperbaiki keselarasan bidikan dan transisi. [L4]

**Cuaca/VFX. [T-V/I]** Hujan dan langit awan sudah hadir. Garis hujan, noda, sorotan dan highlight belum selalu membentuk satu kondisi basah yang meyakinkan. Kontak air, tepian material, tetesan lokal, dan variasi kering/basah lebih berguna daripada menambah grain. Intensitas flash, glitch dan kejutan harus tetap memiliki opsi reduksi. [L1, L9]

## 4. Analisis gameplay dan mekanik

### 4.1 Mekanik DreadOut 1 yang didokumentasikan

Ringkasan berikut adalah **[T-D]**, belum pengujian executable lokal. [D2]

| Sistem | Perilaku dalam manual resmi | Halaman PDF |
| --- | --- | --- |
| Gerak | Jalan, lari, interaksi; lari memiliki kelelahan | 10 |
| Kamera | Membidik memakai first-person; Irisphone dan DSLR memiliki fungsi berbeda | 11 |
| Kondisi pemain | Serangan mental/fisik, perubahan postur dan tampilan kondisi | 12 |
| Kematian | Limbo; perjalanan kembali berubah seiring kekalahan dan item | 13 |
| Entitas | Manifestasi, permusuhan, dan cara penanganan bervariasi | 14–15 |
| Pengamatan | Kamera memperlihatkan hal tersembunyi dan kelemahan | 15 |
| Puzzle | Pemilihan objek dalam foto dapat menentukan hasil | 16 |
| Catatan | Objektif, item, dokumen, Ghostpedia | 17 |
| Foto | Memori foto dan galeri penyimpanan | 18, 22 |

Metadata lokal mendukung pemisahan produk: BuildSettings memuat `01_Prolog_main`, sekolah, kampung, mansion dan final battle untuk kampanye utama; scene KOTD berada pada path terpisah. `Localization` memiliki kunci `Ghostpedia`, `CameraMemory`, `EnableRedVignette`, `EnableBlueVignette`, dan `FixedLimboDistance`. **[T-L]** Ini menguatkan keberadaan data terkait fitur, tetapi tidak membuktikan aturan damage, jangkauan kamera, kecerdasan musuh, maupun opsi tersebut berjalan benar pada instalasi lokal. [D4]

### 4.2 Kamera/foto kita: yang sudah berfungsi dan yang belum utuh

**[T-L] Sudah ada:**

- AR memakai layer khusus yang hanya terlihat kamera HP. Petunjuk jam harus benar-benar diamati sebelum progres terbuka.
- Tiga bukti lantai dua memeriksa syarat cerita, jarak, arah bidik, dan raycast penghalang melalui `Aimed`/`TryPhotoTick`. Bukti berhasil mengurangi baterai 1% dan memutus rantai secara persisten.
- Foto bonus sumur memiliki pemeriksaan bidik/penghalang dan biaya 1%; bukan sekadar tombol hadiah tanpa target.
- `CaptureFlash` memberi pulsa cahaya, shutter visual dan suara. Itu tidak dengan sendirinya menyimpan gambar atau mengklasifikasi isi foto.
- Blitz jendela menghabiskan 15% daya. Adegan memaksa first-person, mengarahkan pandangan, memperlambat waktu dan menunggu input dalam jendela tiga detik. Syarat keberhasilannya berupa state adegan dan kecukupan baterai, bukan pemeriksaan weak point umum.

**[I] Celah desain:** pemain belum memiliki satu bahasa foto yang konsisten: foto bebas, bukti cerita, bonus dan tindakan darurat memiliki akibat berbeda yang tersebar di beberapa sistem. Album bukti yang menampilkan hasil jepretan sebenarnya belum terlihat pada jalur dan aplikasi yang diperiksa. `blitzCooldownSeconds` ada sebagai parameter, tetapi timer yang diperiksa hanya berkurang; ini bukan bukti cooldown combat umum yang sudah terintegrasi. [L4–L6]

Usulnya adalah menyatukan feedback sasaran, hasil foto, alasan kegagalan dan konsekuensi daya. **Tidak harus membuat semua hantu bisa dibunuh dengan kamera.** Larangan memotret Nyonya sebelum memiliki segel adalah bagian desain prolog yang perlu dipertahankan sampai pemilik proyek memutuskan lain.

### 4.3 Stealth, ancaman dan pengejaran

**[T-L]** Basement sudah memiliki patrol, suspicion dan pursue, pemeriksaan garis pandang, serta pendeteksian suara berdasarkan gerak. Pada source, radius suara meningkat dari jongkok ke jalan dan sprint. Layar HP menyala juga memperpanjang jarak deteksi visual. Rak dapat memutus pandangan; lemari memiliki inspeksi dan risiko cahaya layar terlihat. [L6]

**[I]** Kombinasi ini memberi Layar Terakhir ruang diferensiasi yang kuat. Namun penjelasan deteksi banyak memakai notifikasi eksplisit. Suara langkah, perubahan sikap musuh, sorotan dan jeda inspeksi sebaiknya membuat aturan bisa dipahami sebelum teks menjelaskannya. Tidak boleh menyimpulkan AI kita lebih pintar daripada DreadOut tanpa situasi uji setara.

**[T-L]** Pengejar mengikuti pemain dengan pathing basement, lalu memakai jalur authored di atas basement. Capture memeriksa penghalang; ada traversal adegan untuk keluar basement, melompati meja dan jendela. **[I]** Jalur authored dapat menghasilkan klimaks yang kuat, tetapi transisi ke posisi standoff serta orientasi musuh harus dipresentasikan agar tidak terlihat seperti musuh sekadar pindah tujuan atau berjalan menuju jendela. Keluhan historis tentang bagian ini tetap alasan untuk playtest ulang; audit tidak menyatakan bug itu pasti masih terjadi. [L6]

### 4.4 Gerak, puzzle, progres, ekonomi dan aksesibilitas

- **Gerak [T-L/I]:** jalan/sprint/jongkok/slide serta cek ruang untuk berdiri sudah ada. Source yang diperiksa tidak menunjukkan konsumsi stamina pada keputusan sprint. Menambah stamina hanya masuk akal jika menimbulkan pilihan yang adil; perlu menguji gerak dan animasi sebelum mengubah kecepatan atau kesulitan. [L3]
- **Puzzle [T-L/I]:** jam 03:45, observasi AR dan tiga bukti sudah memberi rangkaian sebab-akibat. Kelemahannya adalah urutan bukti cukup ketat dan feedback sering menyebut langkah berikut secara langsung. Perlu mengukur apakah pemain menemukan alasan dari lingkungan atau sekadar mengikuti instruksi. [L5]
- **Baterai [T-L/I]:** parameter normal 0,05%/detik dan AR/UV 0,1%/detik berarti secara teoritis 100% setara sekitar 33,3/16,7 menit pemakaian terus-menerus, tanpa foto/Blitz/pengisian. Ini perhitungan source, bukan durasi sesi aktual. Basement menurunkan daya menjadi 12% pada beat cerita; ada powerbank dan pengisian darurat. Recovery akhir perlu diuji dengan pemain lambat dan semua suplai opsional terlewat. [L4–L6]
- **Ekonomi [T-L/T-A/I]:** uang sah/terikat, belanja Bu Sri, pembayaran dan checkpoint memberi motivasi khas Raka. Progres dapat disimpan tanpa pembayaran ganda pada fixture terdahulu. Nilai dramatisnya akan lebih kuat jika pesan penonton mengubah pilihan yang dipahami pemain, bukan hanya angka naik. [L2, L8, L11]
- **Kontinuitas [T-A]:** kampung sudah dapat diakses jalan kaki; motor menjalankan transisi ke Atmaja; Bu Sri dan dialognya sudah tersedia. Jangan merencanakan penambahan fitur tersebut seolah belum ada. [L2, L11]
- **UX [T-L/T-V/I]:** menu sudah memiliki volume, sensitivitas, subtitle, reduksi glitch, fullscreen dan pilihan perspektif. Screenshot memperlihatkan HP, chat, subtitle dan instruksi bisa bersaing. Belum terlihat kontrol grafis bertingkat, kalibrasi brightness, atau pengaturan sensitivitas bidik terpisah pada menu yang diperiksa. [L4, L8, L10]
- **Fitur masa depan [T-L/B]:** deklarasi UV/selfie/sanity dan nama mode lain tidak membuktikan konten investigasi lengkap. Mode yang belum dipakai pada prolog tidak boleh dihitung sebagai keunggulan gameplay jadi.

## 5. Analisis audio dan atmosfer horor

### 5.1 VO: masalah yang sudah diketahui

**[T-U]** Pengguna secara eksplisit menilai suara AI Layar Terakhir jelek karena tidak ada intonasi dan emosi. Kritik ini diterima sebagai temuan playtest utama. **[T-L]** Dokumen rekaman juga menyatakan placeholder bukan VO final dan dialog tambahan masih menunggu rekaman. Tidak ada dasar untuk melaporkan masalah akting sudah selesai hanya karena file audio dapat diputar. [U1, L7]

**[I] Dampak terhadap horor:**

- Panggilan Damar kehilangan urgensi bila rasa takut, napas dan terputusnya kalimat tidak berkembang.
- Raka kehilangan respons manusiawi bila keheranan, rasa bersalah dan panik memakai pola ujaran sama.
- Nyonya dapat terdengar seperti pembaca teks bila ancaman dan kemarahannya tidak memiliki niat serta perubahan tenaga.
- Bu Sri, Ibu, dan Ratna memerlukan identitas suara serta konteks sosial yang berbeda. Dialog tambahan tanpa pengarahan akting belum otomatis membuat dunia hidup.

Solusi produksi yang disarankan adalah rekaman manusia dengan pengarahan adegan, memakai daftar dialog ber-ID yang sudah tersedia. Bila suara sintetis masih dipakai sementara, statusnya tetap placeholder dan harus ditinjau per performa; mengganti penyedia TTS atau memoles EQ tidak cukup untuk menyatakan kualitas final tercapai. Belum ada rekaman/sintesis baru dalam audit ini.

### 5.2 Infrastruktur audio dan kelemahan yang perlu diuji

**[T-L]** SceneAudioProfile memisahkan ambience/effects/UI, dengan rain, room tone, kipas, benda, langkah dan notifikasi. KosSceneAudio memiliki ketukan, langkah yang menjauh, ringtone, efek telepon dan ducking saat modal. Langkah dihitung dari jarak gerak dengan variasi pitch kecil. Atmaja juga menggunakan generator tone/noise/scrape/shutter untuk sejumlah cue. Keberadaan generator tidak dengan sendirinya berarti suara buruk; hasil artistiknya harus didengar. [L5–L7]

**[I]** Ada risiko cue terasa berulang, karakter suara tiap ruang kurang berbeda, dan volume hujan/notifikasi mengurangi kejelasan dialog. Footstep berbasis jarak belum membuktikan sinkronisasi kaki atau perbedaan bahan lantai. Source yang diperiksa juga belum membuktikan propagasi suara antarruang yang meyakinkan. Perlu menilai pintu terbuka/tertutup, lorong, luar rumah dan lemari melalui pendengaran langsung.

**[T-A]** Capture opening terdahulu mencatat RMS sekitar 0,03794 dan peak 0,68529. Ini hanya level sampel rekaman tersebut, bukan integrated loudness, intelligibility, mutu intonasi, atau bukti seluruh game bebas clipping. [L2]

### 5.3 Pembandingan atmosfer dengan batas bukti

Pada acuan visual DreadOut, ruang negatif dan objek yang hanya sebagian terlihat menyediakan ketidakpastian. **[T-V/I]** Pada Layar Terakhir, chat dan notifikasi sering segera menjelaskan apa yang terjadi, sementara sorot HP dapat membuat ancaman terlalu terbuka. **[T-L/T-V/I]** Ini dapat mengurangi waktu pemain membangun dugaan. [D1, L4–L6]

Kita dapat mempertahankan streamer sebagai sumber tekanan: penonton kadang salah membaca keadaan, notifikasi tidak menimpa dialog penting, dan momen senyap memberi ruang untuk mendengar gerak musuh. Usulan tersebut belum merupakan sistem yang sudah ada. **[I]**

**[B]** Audit ini tidak memberi nilai kualitas soundtrack, akting, detail binaural, atau mixing DreadOut. Tidak ada dasar untuk menyatakan semua audio DreadOut lebih baik; kelemahan VO Layar Terakhir sudah memiliki bukti pengguna sendiri, terlepas dari perbandingan itu.

## 6. Analisis teknis dan arsitektur

### 6.1 Fakta yang dapat dibandingkan

| Aspek | DreadOut lokal | Layar Terakhir |
| --- | --- | --- |
| Engine sampel | **[T-L]** Header bundle dan BuildSettings: Unity 2022.3.28f1 | **[T-L]** ProjectVersion: Unity 6000.3.26f1; URP 17.3.0 |
| Bentuk distribusi | **[T-L]** Windows executable, UnityPlayer, GameAssembly dan metadata IL2CPP | **[T-A]** Windows x86_64 Development Mono; player Linux untuk audit |
| Source internal | **[B]** Tidak tersedia; tidak ada penilaian kebersihan kode/arsitektur internal | **[T-L]** Source first-party terbuka untuk audit, terpisah Core/Player/Smartphone/Prolog/UI/Audio/Editor/Diagnostics |
| Pembagian campaign | **[T-L]** BuildSettings membedakan kampanye utama dan KOTD | **[T-L]** Tiga scene prolog; data kampanye dipulihkan lewat checkpoint |
| Rendering | **[B]** Jalur aktif, biaya pass dan penggunaan fitur tidak diukur | **[T-L]** Forward+, HDR 64-bit, SMAA, grading, SSAO, shadow distance 38 m, dua cascade, atlas 2048 |
| Save/QA internal | **[B]** Tidak diketahui dari executable/keberadaan menu | **[T-L/T-A]** Save atomik + backup + arsip New Game, tes dan receipt terarah |
| Hardware/FPS | **[B]** Tidak diuji | **[B]** Belum ada penerimaan Windows/Iris Xe dengan profiler |

Versi Unity lebih baru, IL2CPP versus Mono, atau ukuran distribusi tidak dapat dipakai sendirian untuk menentukan mutu grafis, performa, maupun kualitas arsitektur.

### 6.2 Kekuatan dan batas arsitektur kita

**[T-L]** CampaignState memisahkan data kampanye, menulis file sementara, melakukan flush dan replace/backup, serta mengarsipkan save sebelum New Game. Beberapa aksi melakukan rollback bila save gagal. Ini membantu mencegah progres dan ekonomi tidak sinkron. Akan tetapi, tidak membuktikan semua kemungkinan crash, korupsi, migrasi versi, atau interaksi save telah diuji. [L8]

**[T-L/I]** Director cerita mengoordinasikan UI, audio, baterai, spawn dan event melalui singleton serta flag string. Untuk prolog yang sangat authored, pendekatan ini sudah memberi alur konkret. Ketika jenis foto, entitas, atau chapter bertambah, pengetahuan aturan yang tersebar berisiko menghasilkan perbedaan syarat antara HUD, input dan state cerita. Ini alasan untuk membatasi tanggung jawab pada bagian yang benar-benar berkembang setelah desain disetujui; bukan mandat refactor total.

**[T-L/I]** Builder/editor pass dapat meregenerasi konten dan beberapa koreksi terjadi saat runtime. Receipt collider serta proteksi struktur opening adalah pengaman yang baik. Risiko berikutnya adalah override authoring lama hilang saat builder yang salah dijalankan, atau tim salah menganggap graph kode sebagai bukti visual/gameplay. Source dan artefak harus tetap dipisahkan fungsinya. [L8–L9]

### 6.3 Risiko performa yang nyata sebagai kandidat pengukuran

- **[T-L]** Selain kamera utama, HP merender feed 256×448. Cermin memiliki RT 256×384 dan aktif dekat area tertentu pada frame bergantian; rekaman Damar menggunakan RT 640×360 saat playback. **[I]** Render tambahan dapat menambah draw/CPU submission meski RT kecil. Tidak diasumsikan semua kamera selalu aktif bersamaan. [L4, L10]
- **[T-L]** Pipeline menggunakan HDR, post-processing, SSAO dan soft shadows; dunia Melati memiliki banyak rumah/lampu. **[I]** Beban fill rate, shadow dan culling perlu diukur pada iGPU. Tidak ada bukti bahwa salah satunya sudah menjadi bottleneck utama. [L9]
- **[T-L]** Pencarian `FindFirstObjectByType` terjadi dalam LateUpdate HP dan lampu; beberapa status UI disusun ulang per frame. **[I]** Ini kandidat optimasi CPU/allocation setelah profiling, bukan bukti stutter terukur. [L4]
- **[T-L]** Pathing basement memiliki koneksi waypoint yang di-cache, tetapi pemilihan rute dan pemeriksaan fisik tetap dapat terjadi berulang. **[I]** Frekuensi sense/path update dan alokasi perlu diperiksa jika CPU-bound. [L6]
- **[B]** Draw calls, SetPass, GPU frame time, GC per frame, peak RAM/VRAM, shader warm-up, loading dan 1% low belum tersedia sebagai benchmark setara kedua game.

Target internal yang layak diuji adalah 30 FPS stabil pada Windows/Iris Xe yang disebut arsitektur proyek. Anggaran 33,3 ms per frame adalah target, bukan hasil. Pengujian perlu mencatat resolusi, preset, build Development/Release, driver, daya laptop, lokasi dan rute yang sama.

## 7. Tabel perbandingan dengan bukti konkret

Penilaian berikut menggunakan bukti terbatas di atas. “Lebih lemah” berarti pada aspek yang terlihat/didokumentasikan, bukan skor keseluruhan kedua game.

| Aspek | Acuan DreadOut | Bukti Layar Terakhir | Putusan saat ini |
| --- | --- | --- | --- |
| Komposisi horor | D1: koridor memiliki kedalaman dan bidang gelap terarah | L1: lorong atas memiliki hotspot dekat HP | **[T-V/I] Lebih lemah** pada konsistensi frame yang diperiksa |
| Detail lingkungan | D1: keausan dan bentuk menyatu pada contoh pintu/koridor | L1: kualitas kamar awal lebih koheren daripada jalan dan kamar anak | **[T-V/I] Belum konsisten**, bukan kekurangan resolusi semata |
| Karakter | D1: siluet/pose Linda mendukung frame | L3: avatar dan klip ada; frame Blitz menunjukkan ancaman kurang kuat | **[T-V/I] Presentasi perlu ditingkatkan**; keluwesan gerak A/B belum dinilai |
| Third/first-person | D2: kedua sudut pandang terkait fungsi kamera | L3/L8: pilihan perspektif, collision camera, override adegan | **[T-L] Fondasi ada**; rasa kontrol belum bisa diperingkat |
| Kebutuhan membidik | D2: kamera berperan dalam observasi/puzzle/ancaman | L5: aim + occlusion pada bukti dan sumur | **[T-L/T-D] Ada kesamaan fungsi nyata**; sistem kita masih spesifik target |
| Blitz/combat umum | D2: penanganan entitas bervariasi | L6: Blitz akhir tergantung state adegan dan baterai | **[T-L/T-D/I] Cakupan kita lebih sempit**; jangan menyebutnya combat umum |
| Galeri foto | D2: foto dapat ditinjau/disimpan | L4: shutter ada, aplikasi galeri belum ditemukan | **[T-L/T-D] Kesenjangan fitur pada jalur yang diperiksa** |
| Stealth | Perilaku setara tidak diuji langsung | L6: suara gerak, pandangan, layar, rak, inspeksi lemari | **[T-L] Kekuatan desain kita**, superioritas AI belum terbukti |
| Kematian/progres | D2: Limbo | L6/L8: gagal ke checkpoint, state persisten | **[T-L/T-D] Pendekatan berbeda**; tidak wajib menyalin Limbo |
| VO | Kualitas dengar A/B belum diperiksa | U1 + L7: AI datar, produksi belum final | **[T-U/T-L] Kekurangan P0 kita terbukti sebagai temuan playtest** |
| Audio ruang/mix | Tidak diukur/didengar langsung | L7: mixer, ambience, cue tersedia | **[B] Tidak ada pemenang sah**; infrastruktur bukan mutu hasil |
| Pacing horor | Tidak dimainkan langsung dalam audit | L2: timer/capture fixture dimodifikasi untuk verifikasi | **[B] Belum bisa dinilai sebagai pengalaman natural** |
| Lingkungan sosial | Bukan fokus yang diuji dari DreadOut | L11/L2: Bu Sri, belanja, berjalan kaki | **[T-L/T-A] Identitas yang sudah ada**, perlu polish |
| Ekonomi/livestream | Tidak mengasumsikan fitur identik pada acuan | L4/L8: chat, penonton, uang terikat/sah, pembayaran | **[T-L] Diferensiasi kuat**, nilai dramatis masih inferensi |
| Save | Implementasi internal tidak diketahui | L8/L2: atomik, backup, Continue terarah berhasil | **[T-L/T-A] Fondasi kita dapat dibuktikan**, bukan bukti lebih unggul dari DreadOut |
| Pengaturan | D3: pembaruan kontrol/visual menurut pengembang | L8: sensitivitas, remap tertentu, subtitle, reduksi glitch, perspektif | **[T-L] Berguna**, menu grafis/brightness belum lengkap pada source yang diperiksa |
| Keandalan lintasan | Tidak diuji pada instalasi lokal | L2: opening/prolog selesai melalui fixture | **[T-A] Jalur kita terverifikasi terbatas**, bukan bebas bug |
| Performa | Tidak ada benchmark lokal | L2: llvmpipe; Windows sudah dibuild | **[B] Tidak dapat membandingkan FPS atau efisiensi** |
| Arsitektur | Source DreadOut tidak tersedia | L8 + Graphify: hubungan source dapat ditelusuri | **[B] Tidak dapat memberi peringkat kebersihan kode dua game** |

## 8. Fitur yang sudah lebih baik di Layar Terakhir

Tidak ada dasar untuk mengklaim Layar Terakhir lebih unggul mutlak pada fidelity grafis, akting, performa, atau keandalan. Keunggulan yang dapat dipertanggungjawabkan perlu dibatasi kriterianya:

1. **Fleksibilitas perspektif untuk pemain kita [T-L/T-D].** Layar Terakhir memiliki pilihan first-person/third-person yang disimpan dan dapat diubah, di samping override adegan. Dibanding alur kamera yang dijelaskan manual DreadOut, ini menawarkan pilihan eksplorasi lebih eksplisit. Belum membuktikan Remastered tidak memiliki opsi tambahan yang belum diuji. [L3, L8, D2]
2. **Keterikatan mekanik dengan pekerjaan dan kebutuhan Raka [T-L/I].** HP, siaran, saldo dan pembayaran sudah menjadi bagian sistem. Ini keunggulan identitas untuk cerita kita dibanding sekadar mengadopsi formula kamera acuan. Efektivitas emosionalnya masih perlu playtest, terutama setelah VO diganti. [L4, L8]
3. **Trade-off layar dan persembunyian [T-L/I].** Sumber cahaya yang membantu investigasi juga memengaruhi risiko ditemukan. Aturan ini sudah tertulis dalam AI basement; layak diperdalam karena menghubungkan perangkat, ruang dan ancaman. Tidak ada klaim bahwa DreadOut tidak memiliki sistem lain dengan kedalaman sebanding. [L6]
4. **Interaksi keseharian lokal yang sudah dapat dipakai [T-L/T-A].** Bu Sri, pembelian bekal, telepon keluarga dan urusan kos memberi bahan untuk membangun kedekatan sebelum teror. Ini lebih sesuai kebutuhan cerita Layar Terakhir daripada mengganti semua lokasi menjadi ruang angker generik. [L2, L4, L11]

Save atomik, bukti QA dan pipeline yang modern adalah kekuatan produksi kita yang terverifikasi. Karena proses internal DreadOut tidak diketahui, hal-hal itu tidak diberi label “lebih baik daripada DreadOut”.

## 9. Kekurangan Layar Terakhir dibanding DreadOut

**Kesenjangan yang cukup kuat untuk ditindaklanjuti:**

- **[T-V/I] Konsistensi visual:** kualitas kamar awal tidak diteruskan secara utuh ke jalan, kamar anak, dan klimaks; kontras, skala material serta komposisi belum menyatu.
- **[T-L/T-V/I] Hubungan HP dengan tubuh/dunia:** feed UI dan sudut dunia memiliki presentasi berbeda, sementara first-person belum menampilkan performa tangan yang sebanding dengan acuan visual kamera.
- **[T-L/T-D/I] Kedalaman fotografi:** bukti dan bonus sudah nyata, tetapi foto bebas belum memberi dokumentasi/feedback yang setara fungsi sistem foto yang dijelaskan acuan.
- **[T-L/T-D/I] Ragam aturan ancaman:** prolog kita berpusat pada satu ancaman besar dan satu urutan pelarian. Ini wajar untuk prolog, tetapi belum cukup untuk menyatakan kedalaman satu game penuh setara DreadOut.
- **[T-U/T-L] Akting suara:** kualitas VO kita dinilai gagal oleh pengguna dan status produksinya belum final. Ini kekurangan mandiri yang mendesak; besar selisih dengan akting DreadOut belum diukur.
- **[T-L/T-V/I] Penyampaian horor:** notifikasi dan petunjuk eksplisit dapat terlalu cepat menggantikan pengamatan pemain; model/pose ancaman serta sorot terang dapat melemahkan klimaks.

**Yang belum boleh dipastikan:** DreadOut lebih lancar, lebih sedikit bug, animasinya selalu lebih baik, audionya selalu lebih baik, atau arsitekturnya lebih bersih. Semua itu membutuhkan bukti yang tidak tersedia dalam audit ini.

Perbaikan lama—tangga tersembunyi, akses kampung dengan jalan kaki, lokasi kos di sisi jalan, dinding kos, vendor Bu Sri, dan motor ke Atmaja—harus dipertahankan. Temuan audit bukan alasan meregenerasi seluruh lingkungan atau mengembalikan desain lama.

## 10. Rekomendasi peningkatan dengan prioritas P0/P1/P2

P0 = menutup penghalang kualitas utama atau ketidakpastian mendasar. P1 = memperdalam pengalaman setelah fondasi stabil. P2 = perluasan/polish lanjutan. Semua solusi di bawah **usulan, belum diterapkan**. Kesulitan adalah estimasi awal berdasarkan cakupan, bukan janji durasi.

### P0 — kualitas inti dan bukti penerimaan

**R01 — Ganti performa VO yang datar.**

- **Masalah:** intonasi/emosi AI melemahkan karakter dan horor.
- **Bukti:** [T-U] U1; [T-L] L7 masih menunggu rekaman.
- **Solusi:** prioritaskan rekaman manusia untuk panggilan Damar, respons Raka dan klimaks; arahkan niat, perubahan emosi, napas serta jeda per baris; tinjau beberapa take sebelum mixing. Gunakan ID dialog yang sudah ada. Tidak mewajibkan pengguna menjadi pengisi suara semua karakter.
- **Dampak:** sangat tinggi terhadap urgensi cerita, keterikatan dengan karakter dan rasa takut.
- **Kesulitan/dependensi:** tinggi; memerlukan pengisi suara, pengarahan dan editing. Lulus bila pemilik proyek menerima performa, kata jelas terdengar, dan subtitle mengikuti take final.

**R02 — Rapikan pencahayaan dan material pada titik paling lemah.**

- **Masalah:** hotspot lorong/Blitz dan material sangat menonjol memecah konsistensi visual.
- **Bukti:** [T-V] L1 serta frame lantai dua, kamar Bima dan standoff pada Bagian 3.
- **Solusi:** buat acuan exposure/material per lokasi; sesuaikan sorot dekat, albedo dan roughness; perbaiki skala tekstur serta kontak benda; review dengan HP hidup/mati. Pertahankan keterbacaan petunjuk dan layout approved.
- **Dampak:** sangat tinggi pada kesan kualitas gambar tanpa memperluas map.
- **Kesulitan/dependensi:** sedang–tinggi; perlu artist/polish pencahayaan dan capture representatif. Lulus bila hotspot tidak menghilangkan bentuk dan perpindahan ruang terasa koheren.

**R03 — Audit tubuh, pakaian, tangan dan pose ancaman.**

- **Masalah:** pose/proporsi belum meyakinkan; ada keluhan historis tentang sambungan leher dan pakaian.
- **Bukti:** [T-V] frame jongkok/Blitz; [T-L] L3. Cacat leher/celana belum direproduksi ulang pada audit ini.
- **Solusi:** validasi mesh, normals, skin weights dan batas pakaian dari seluruh sudut; perbaiki yang dapat direproduksi; poles pose jongkok/pegang HP/ancaman sebelum menambah animasi lain.
- **Dampak:** sangat tinggi karena tubuh pemain sering berada di layar dan ancaman harus terbaca.
- **Kesulitan/dependensi:** tinggi; membutuhkan akses mesh/rig dan kecakapan animasi. Lulus bila pakaian tertutup pada semua postur uji dan siluet ancaman diterima pemilik proyek.

**R04 — Selaraskan kamera eksplorasi, bidikan dan layar HP.**

- **Masalah:** framing/render feed berbeda dari tampilan dunia; collision camera dan transisi belum dinilai dinamis.
- **Bukti:** [T-L/T-V] L3–L4; feed 256×448, post/shadow berbeda; frame L1.
- **Solusi:** tetapkan pusat bidik yang jelas, hubungan posisi kamera dengan tangan, transisi angkat/turun HP dan perilaku dekat dinding; kalibrasi perbedaan feed agar disengaja. Pertahankan opsi perspektif pengguna.
- **Dampak:** sangat tinggi pada kontrol, investigasi dan rasa memegang perangkat.
- **Kesulitan/dependensi:** tinggi; tergantung R03 dan evaluasi pemain. Lulus bila sasaran yang dianggap terbidik sama dengan hasil foto/AR di pintu, lorong dan tangga.

**R05 — Uji manual jalur kritis dan kondisi gagal.**

- **Masalah:** fixture berhasil belum menjamin pemain bebas tersangkut, softlock, atau salah memahami adegan.
- **Bukti:** [T-A] L2 menyatakan traversal diarahkan dan timing tertentu diubah untuk audit; keluhan historis pengguna.
- **Solusi:** playtest Windows dengan timing alami: tangga dua arah, pintu, membaca sambil memegang HP, AR, baterai rendah, lemari, handcam, chase, kematian dan Continue. Reproduksi sebelum memperbaiki; jangan menganggap semua keluhan lama masih aktif.
- **Dampak:** sangat tinggi pada kepercayaan pemain dan kelangsungan prolog.
- **Kesulitan/dependensi:** sedang untuk audit, sedang–tinggi untuk perbaikan yang ditemukan; membutuhkan tester Windows. Lulus bila rute utama serta recovery diuji tanpa reset tersembunyi atau pembayaran ganda.

**R06 — Tetapkan baseline performa Windows.**

- **Masalah:** belum ada data yang sah untuk menargetkan fidelity atau optimasi.
- **Bukti:** [T-A/B] L2 hanya software renderer; risiko rendering pada Bagian 6.
- **Solusi:** ukur build dan rute tetap di hardware target; rekam CPU/GPU time, persentil frame time, GC, RAM, draw calls dan loading. Pisahkan overhead Development dari build Release.
- **Dampak:** tinggi pada kenyamanan dan pengambilan keputusan visual.
- **Kesulitan/dependensi:** sedang; perlu perangkat Windows/Iris Xe, profiler dan prosedur berulang. Kriteria awal: target 30 FPS/33,3 ms dengan spike dicatat, bukan sekadar FPS rata-rata.

### P1 — kedalaman dan penyatuan pengalaman

**R07 — Jadikan foto sebagai sistem investigasi yang utuh.**

- **Masalah:** shutter, bukti dan bonus belum menyatu sebagai satu pengalaman dokumentasi.
- **Bukti:** [T-L] L4–L5; D2 mendokumentasikan fungsi foto/galeri sebagai pembanding.
- **Solusi:** usulkan klasifikasi foto, thumbnail bukti, alasan gagal, hasil observasi dan penyimpanan yang dibatasi; mulai dari objek prolog yang sudah ada, bukan langsung banyak fitur kamera baru.
- **Dampak:** tinggi pada kegunaan HP, rasa menemukan sesuatu dan replay.
- **Kesulitan/dependensi:** tinggi; tergantung R04, anggaran memori/save, dan keputusan pemilik proyek tentang foto bebas versus bukti wajib. Lulus bila pemain dapat meninjau bukti yang benar-benar ia ambil.

**R08 — Perjelas aturan tiap jenis ancaman.**

- **Masalah:** kekebalan Nyonya, larangan foto dan Blitz darurat berpotensi dipahami sebagai inkonsistensi.
- **Bukti:** [T-L/I] L6 menunjukkan stealth dan Blitz berbasis tahap cerita.
- **Solusi:** komunikasikan perbedaan gangguan pasif, ancaman yang harus dihindari dan kesempatan stun melalui animasi/audio/reaksi foto; pertahankan Nyonya tidak dapat dilukai sebelum segel. Combat lebih luas hanya bila desain menghendaki.
- **Dampak:** tinggi pada keadilan, ketegangan dan pemahaman sebab-akibat.
- **Kesulitan/dependensi:** tinggi; perlu aturan kreatif yang jelas, R01/R03/R04, dan animasi feedback. Lulus bila pemain bisa menjelaskan kapan kamera membantu dan kapan membahayakan.

**R09 — Bangun cerita lingkungan dan kontinuitas kampung.**

- **Masalah:** kepadatan fasad belum menghasilkan lingkungan yang terasa dihuni; kamar anak/ruang makan kurang terkurasi.
- **Bukti:** [T-V] frame Melati, samping kos, warung dan kamar Bima pada Bagian 3.
- **Solusi:** tambah variasi bermakna pada pekarangan, jemuran, utilitas, barang warung dan tanda penghuni; susun prop interior menurut riwayat ruang; tutup pandangan menuju batas map melalui lingkungan yang masuk akal sambil menjaga jalur jalan kaki.
- **Dampak:** tinggi pada identitas lokal dan rasa percaya terhadap dunia.
- **Kesulitan/dependensi:** sedang–tinggi; butuh aset berlisensi/original, anggaran performa R06 dan validasi lintasan R05.

**R10 — Perbaiki suara ruang, foley dan mixing dinamis.**

- **Masalah:** diferensiasi ruang, bahan langkah dan keterbacaan dialog belum dibuktikan; cue dapat berulang.
- **Bukti:** [T-L/I/B] L7, perhitungan langkah dan cue sintetis; belum ada audit dengar menyeluruh.
- **Solusi:** audit pendengaran tiap lokasi; bedakan bahan langkah dan interior/eksterior; atur prioritas VO terhadap hujan/chat; rancang occlusion/reverb secukupnya dan variasi cue tanpa mengandalkan efek keras terus-menerus.
- **Dampak:** tinggi pada orientasi ancaman dan atmosfer.
- **Kesulitan/dependensi:** sedang–tinggi; membutuhkan take R01, sound designer serta headphone/speaker uji. Lulus bila dialog jelas dan arah/jarak ancaman dapat dibaca tanpa harus mengikuti notifikasi.

**R11 — Poles gerak dan transisi animasi.**

- **Masalah:** satu pemilihan klip berbasis kecepatan belum memastikan gerak samping/mundur, berbelok dan tangga natural.
- **Bukti:** [T-L] L3; tidak ada penilaian siklus animasi lengkap dari satu frame.
- **Solusi:** poles akselerasi, berhenti, arah gerak, putar di tempat dan sinkronisasi langkah; terapkan penyesuaian kaki/tangan pada titik yang membutuhkan. Evaluasi manfaat stamina sebelum menambahnya.
- **Dampak:** tinggi pada kualitas third-person dan rasa berat tubuh.
- **Kesulitan/dependensi:** tinggi; tergantung R03/R04 dan animator. Lulus melalui review video gerak, bukan screenshot diam.

**R12 — Kurangi instruksi yang menggantikan penemuan pemain.**

- **Masalah:** chat, subtitle, notifikasi dan prompt dapat bertumpuk serta terlalu cepat memberi jawaban.
- **Bukti:** [T-V/T-L] frame basement/Blitz dan L4–L6/L10.
- **Solusi:** tentukan prioritas informasi; beri petunjuk lingkungan lebih dulu, bantuan bertahap ketika buntu, kontrol ukuran teks serta sensitivitas bidik. Jaga bukti kecil tetap terbaca pada resolusi target.
- **Dampak:** tinggi pada atmosfer, keterbacaan dan rasa berhasil memecahkan masalah.
- **Kesulitan/dependensi:** sedang; perlu playtest baru R05, pengarahan puzzle dan desain UI. Lulus bila pemain baru menyelesaikan langkah tanpa terus dibimbing auditor.

**R13 — Buat skala kualitas berdasarkan hasil profiler.**

- **Masalah:** satu konfigurasi visual berpotensi terlalu mahal pada iGPU.
- **Bukti:** [T-L/I] HDR/SSAO/shadows dan kamera tambahan; belum ada bottleneck terukur.
- **Solusi:** setelah R06, prioritaskan biaya yang terbukti; sediakan preset shadow/AO/render scale dan frekuensi feed/reflection, culling/LOD yang tepat. Hindari penurunan kualitas menyeluruh tanpa data.
- **Dampak:** tinggi pada kestabilan tanpa mengorbankan arah visual.
- **Kesulitan/dependensi:** sedang–tinggi; bergantung profiler, batas readability R04/R07 dan target hardware. Lulus bila preset diuji pada rute yang sama dan petunjuk tidak hilang.

### P2 — perluasan setelah fondasi diterima

**R14 — Perdalam reaksi livestream dan ekonomi.**

- **Masalah:** chat/penonton banyak mengikuti tahap atau ruang sehingga respons bisa terasa generik.
- **Bukti:** [T-L] L4–L5, antrean komentar ruang dan hitungan penonton berbasis event.
- **Solusi:** buat respons untuk pilihan/foto/risiko yang benar-benar terjadi, batas frekuensi, serta konsekuensi ekonomi yang terbaca; penonton boleh keliru tanpa menjadi petunjuk palsu yang tidak adil.
- **Dampak:** sedang–tinggi pada identitas unik dan replay.
- **Kesulitan/dependensi:** tinggi; R07/R08/R12, penulisan dialog dan pengujian keseimbangan. Bukan sistem siaran online sungguhan.

**R15 — Tambah arsip lore yang diperoleh lewat observasi.**

- **Masalah:** dokumen teks belum selalu menghubungkan bukti visual dengan pemahaman entitas.
- **Bukti:** [T-L] L4/L8; D2 menyediakan pembanding catatan/ensiklopedia.
- **Solusi:** arsip original berisi potongan informasi yang ditemukan pemain, keterkaitan foto dan pertanyaan yang belum terjawab; gunakan budaya/lore cerita kita sendiri.
- **Dampak:** sedang pada keterikatan cerita dan motivasi eksplorasi.
- **Kesulitan/dependensi:** sedang; R07, konten naratif dan save. Tidak menyalin Ghostpedia, gambar, teks atau entitas DreadOut.

**R16 — Batasi duplikasi aturan saat fitur berkembang.**

- **Masalah:** aturan foto, baterai, UI dan stage tersebar; penambahan chapter berisiko membuatnya berbeda-beda.
- **Bukti:** [T-L/I] L4–L8, singleton/flag string dan pemrosesan input di beberapa director.
- **Solusi:** setelah perilaku baru disepakati, pisahkan tanggung jawab hanya pada jalur yang membutuhkannya; gunakan ID/event yang jelas dan verifikasi save lama. Tidak mengusulkan rewrite proyek sebagai langkah pertama.
- **Dampak:** sedang pada konsistensi dan biaya produksi berikutnya.
- **Kesulitan/dependensi:** sedang–tinggi; bergantung desain R07/R08 yang stabil serta tes regresi. Ini rencana arsitektur, belum refactor.

**R17 — Tinjau pengalaman gagal/kematian yang sesuai cerita.**

- **Masalah:** reset checkpoint dapat memutus atmosfer bila transisi dan informasi penyebab gagal lemah.
- **Bukti:** [T-L] L6/L8; D2 menunjukkan alternatif desain kematian, bukan kewajiban mengikuti.
- **Solusi:** perbaiki feedback penyebab gagal, waktu pemulihan dan kontinuitas suara/gambar. Pengalaman supernatural saat gagal boleh dieksplorasi hanya jika bermanfaat; tidak wajib menambah Limbo atau hukuman berjalan berulang.
- **Dampak:** sedang pada immersion dan frustrasi pemain.
- **Kesulitan/dependensi:** sedang untuk polish checkpoint, tinggi untuk sistem baru; R05/R10 dan keputusan naratif.

## 11. Roadmap implementasi bertahap

**Roadmap ini tidak dijalankan sebelum persetujuan pengguna.** Urutan bergantung bukti dan titik penerimaan, bukan daftar pekerjaan yang sudah selesai.

| Tahap | Cakupan rekomendasi | Hasil yang perlu dapat ditinjau | Syarat lanjut |
| --- | --- | --- | --- |
| A. Lengkapi baseline | R05, R06; reproduksi R03; sesi dengar R01/R10 | Catatan playtest timing alami, video gerak, profil hardware, daftar masalah yang benar-benar muncul | Penyebab kegagalan dan batas hardware cukup jelas; jangan menerapkan semua dugaan sebagai bug |
| B. Satu rangkaian mencapai mutu target | R01–R04 pada kedatangan Atmaja → AR → satu bukti → ancaman | Performa VO final untuk adegan contoh, bidikan konsisten, karakter/pencahayaan disetujui | Pemilik proyek menerima visual dan emosi; jalur/akses/layout approved tetap benar |
| C. Terapkan standar ke seluruh prolog | R01–R05, R09–R12 | Kos, perjalanan, lantai dua, basement dan ending memakai standar yang sama | New Game sampai Continue selesai secara manual; isu tubuh, tangga, HP dan recovery diperiksa |
| D. Perdalam fungsi HP dan ancaman | R07, R08; lanjut R12 | Hasil foto dapat dipahami/ditinjau, aturan entitas terbaca tanpa bantuan terus-menerus | Nyonya tetap sesuai desain; foto/Blitz tidak menimbulkan softlock atau saldo/save ganda |
| E. Stabilkan biaya rendering | R13 dengan pengukuran ulang R06; sebagian dapat berjalan paralel dengan B–D | Preset dan perbandingan frame time/kualitas pada perangkat target | Target performa serta readability tercapai pada preset yang dinyatakan |
| F. Perluas identitas dan produksi | R14–R17 sesuai kebutuhan | Reaksi penonton, lore dan struktur fitur yang mendukung chapter berikut | Fondasi prolog sudah diterima; penambahan fitur tidak membuka regresi lama |

Paket penerimaan sebaiknya berisi capture dari lokasi/kamera yang sama, video gerak, rekaman adegan dengan suara final, hasil playtest tanpa percepatan, serta data performa. Gambar diam saja tidak dapat membuktikan emosi, animasi, kontrol atau ketegangan.

## 12. Risiko, estimasi kompleksitas, dan dependensi

### 12.1 Estimasi awal

Rentang berikut adalah perkiraan kasar hari kerja efektif untuk satu kontributor yang memiliki keahlian sesuai, setelah akses alat tersedia. Tidak termasuk antrean aktor, pembelian aset, waktu tunggu review, atau seluruh pengujian lintas perangkat. Pekerjaan saling tumpang tindih; **jangan menjumlahkan rentang menjadi janji tanggal selesai**.

| Paket | Kompleksitas | Kisaran awal | Dependensi utama |
| --- | --- | --- | --- |
| Baseline manual dan profiler, R05–R06 | Sedang | 2–5 hari untuk baseline; perbaikan bug terpisah | Windows target, tester, profiler |
| VO satu rangkaian contoh, R01 | Tinggi | 2–5 hari produksi/editing, di luar jadwal aktor | Pemilihan suara, pengarahan, take, review pengguna |
| Lighting/material contoh, R02 | Sedang–tinggi | 3–7 hari | Acuan visual, material, batas hardware |
| Perbaikan tubuh/rig dan kamera, R03–R04 | Tinggi | 5–15 hari; lebih besar bila perlu mengganti rig | Source mesh, animator, animasi tangan, regresi traversal |
| Polish lingkungan, gerak, audio dan UX prolog, R09–R12 | Tinggi | 10–25 hari gabungan lintas keahlian | Ketersediaan aset/rekaman, hasil playtest |
| Foto investigasi dan aturan ancaman, R07–R08 | Tinggi | 8–20 hari | Keputusan desain, kamera stabil, save, feedback animasi/audio |
| Preset dan optimasi terukur, R13 | Sedang–tinggi | 3–10 hari setelah bottleneck diketahui | Profiling nyata; angka dapat berubah bila konten bertambah |
| Setiap perluasan P2 | Sedang–tinggi | 3–10 hari per fitur terbatas | Konten, desain yang disetujui, tes kompatibilitas |

### 12.2 Risiko dan pengendalian

| Risiko | Mengapa relevan | Pengendalian yang diusulkan |
| --- | --- | --- |
| Mengira metadata membuktikan gameplay | Local DreadOut hanya diperiksa statis | Pertahankan label bukti; lakukan sesi kampanye utama sebelum memfinalkan perbandingan kontrol/AI |
| Arah kreatif berubah menjadi tiruan | Referensi kamera/horor mudah disalin tanpa konteks | Pertahankan Raka, streamer, ekonomi, kos dan lore original; gunakan acuan untuk mutu dan keterbacaan |
| Mengganti AI voice tanpa memperbaiki akting | Masalah pengguna adalah intonasi/emosi | Nilai take per adegan bersama pemilik proyek; rekaman manusia dan pengarahan didahulukan |
| Visual lebih mahal tetapi tetap kurang koheren | Pipeline modern sudah ada, hardware target iGPU | Penerimaan komposisi/material dan pengukuran R06 sebelum menambah efek |
| Perbaikan kamera merusak interaksi | Aim, occlusion, HP, collision camera dan traversal saling terkait | Uji matriks perspektif × HP × pintu/tangga × resolusi |
| Pakaian kembali terbuka saat animasi lain | Satu pose tidak mencakup seluruh deformasi | Review depan/belakang/samping saat crouch, run, phone, motor dan transisi |
| Builder menimpa revisi yang disetujui | Ada authoring terprogram dan koreksi runtime | Perubahan kelak harus terarah, bertanda sumber, dengan pemeriksaan layout/collider |
| Baterai menghalangi progres wajib | AR dan Blitz memakai resource yang sama | Uji pemain lambat, foto berulang, suplai terlewat dan recovery tanpa meniadakan ketegangan |
| Galeri membengkakkan memori/save | Foto nyata menambah data di luar flag bukti | Tetapkan jumlah, resolusi, pemisahan thumbnail dan kebijakan penyimpanan sebelum implementasi |
| Hasil tes dipakai untuk klaim bebas bug | Fixture terarah tidak mencakup eksplorasi bebas | Pisahkan kelulusan otomatis, playtest manual, penerimaan kreatif dan benchmark |
| Scope membesar sebelum prolog rapi | Membandingkan prolog dengan dua game komersial dapat mendorong penambahan fitur | Selesaikan rangkaian contoh dan standar prolog sebelum chapter/musuh baru |

### 12.3 Kriteria menyatakan setara atau lebih baik

Klaim tersebut baru layak untuk lingkup yang disepakati setelah: visual representatif diterima pemilik proyek; VO benar-benar menyampaikan emosi; gerak dan kamera lolos review dinamis; pemain baru memahami investigasi serta ancaman; prolog dapat ditamatkan dan dipulihkan tanpa intervensi; serta target frame time tercapai pada hardware yang dinyatakan. Pembandingan dengan DreadOut juga memerlukan sesi bermain serta dengar yang setara pada kampanye yang tepat.

**Status akhir audit:** laporan saja. Tidak ada kode, aset game, scene, konfigurasi, atau struktur folder proyek yang diubah. Tidak ada refactor, implementasi, build baru, upload ulang ZIP, maupun aset/kode proprietary DreadOut yang dimasukkan ke Layar Terakhir. Implementasi menunggu persetujuan pengguna atas prioritas/cakupan revisi.
