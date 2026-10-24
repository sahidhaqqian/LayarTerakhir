# Phase 3 — implementasi dan bukti validasi

Basis integrasi `a8b1340`, arahan desainer 24 Oktober 2026. Status: **candidate untuk review, belum penerimaan kualitas final**. Tidak mengklaim setara DreadOut, pacing 75 menit, seluruh 146 QA lulus, atau performa Windows/Iris Xe telah diukur.

## Implementasi

Golden Kos melanjutkan P2 selesai dengan dua segel valid. Modul UV/EVP gratis, persiapan colokan manual, paket toko opsional, dan motor menuju pabrik. Lima adegan di satu factory terhubung: halaman timbangan, gilingan, bangsal dua lantai/kantor mandor, gudang ampas, dan aula tungku. Tangga mempunyai tread collision dan NavMesh untuk tinggi aktor Wewe, bukan hanya mesh visual.

Investigasi memakai foto AR ayunan, foto rajah UV, recorder enam detik dengan minimal dua detik ASLI, tiga pusaka, dokumen/kartu/sepatu terpisah, konfirmasi Jurnal, AI lantai/plafon, LOS/noise/layar, persembunyian, serta ventilasi–katup–loker09 opsional. Ketuk C dan tahan C tidak memberikan dua aksi sekaligus. Papan rapuh menghilangkan support fisik; papan tersebut tidak menjadi rute AI di atas lubang. Lantai bawah balkon mempunyai jalur kembali.

Ritual menyimpan CP5 sebelum owner lock, memiliki tiga siklus, sorotan N/S/E yang memerlukan overlap/cone/LOS nyata, Blitz, flare opsional, UV simpul dan reset daya alcove manual. Lampu baru korslet setelah hit sukses. CP6 menyimpan transfer pusaka, flare terpakai dan segel dalam satu transaksi. Epilog menampilkan infiltrasi chat/dana terikat tanpa mengkredit milestone sebagai uang belanja. Pemain dapat menjelajah kembali; motor mempunyai batal/konfirmasi dan menyimpan CP7 sebelum fade menuju Karang Setra. Babak4 tidak dibuat.

## Integrasi dan keputusan teknis

CampaignState existing dinaikkan schema4 melalui migrasi1→2→3→4, mempertahankan state lama. Tidak ada save, event bus, inventori, HP atau AR paralel. SmartphoneController tetap pemilik energi; IPhoneChapterContent dan input global V/X/Q/R/F/C tetap berlaku. Sanity tidak diaktifkan. Pickup/backtracking tidak menurunkan checkpoint; bukti dan pusaka tidak saling memberikan reward otomatis. Primary korup ditolak dan API backup existing digunakan secara eksplisit, bukan membuat auto recovery baru.

Factory dimuat sinkron karena pemuatan asinkron macet pada renderer software mesin uji. Renderer Forward+ produksi dipertahankan. Volume override diserialisasi sebagai subasset agar tidak menjadi referensi null saat reload/build. Konversi PBR mempertahankan albedo/normal dan mengemas metalness/smoothness dari map sumber.

## Aset dan audio

63 entri sumber art, 46 jenis prop non-arsitektur yang ditempatkan. Lingkungan factory baru diunduh dari sumber Poly Haven, Sketchfab/Objaverse berlisensi CC0/CC BY, OpenGameArt serta Kenney. Source URL, pembuat, lisensi, hash dan transformasi tercatat dalam manifest/kredit. Golden Kos, Raka, HP dan motor tetap komponen kampanye bersama; tidak memindahkan furnitur/material RSUD/Rusunawa ke factory. Tidak mengambil aset atau kode proprietary DreadOut. Tidak membuat model, tekstur/gambar lukisan, atau klip gerak original.

16 rekaman audio baru ditambah satu entri paket partikel. Humming/moan bersumber dari rekaman nonverbal manusia berlisensi; tidak menambahkan dialog AI sebagai pengganti akting. Geometri, rig dan gerak Wewe berasal dari pembuat aset, dengan helper materialless dibuang saat konversi. Material yang semula kehilangan albedo karena variasi nama `_Diffuse` telah diperbaiki.

## Perbaikan yang ditemukan dalam pengujian

- Interaksi EVP jauh tidak lagi mengalahkan sumber dekat hanya karena priority.
- G-ACC tidak mengisi baterai atau memakai resource boss. Pintu dan pembuka ritual tidak menutupi marker interaksinya sendiri.
- Pickup kain/kartu/sepatu mendapat lorong bebas dari karung, rak dan sekop. Membaca kartu tidak memberikan sepatu.
- Charger alcove memakai collider/raycast interactor sebagai acuan jika origin model terhalang trim; prompt yang sah dapat dilanjutkan dengan tahan E.
- Papan rapuh dikecualikan dari NavMesh sehingga AI tidak mempunyai jalur melayang sesudah runtuh. Pagar tungku tetap ada setelah menang; jatuh tidak sengaja sesudah menang dikembalikan ke altar.
- Simpul UV ditampilkan di depan tubuh Wewe dan collider Wewe sendiri dikecualikan dari ray counter; dinding/prop tetap menghalangi.
- Badan motor memperoleh collider mengikuti bounds visual; pemicu kecil di samping motor bukan satu-satunya cara membidiknya.
- Pencahayaan dasar dan exposure ditingkatkan setelah render memperlihatkan area terlalu gelap. Ini perubahan terukur pada scene, bukan bukti art direction final diterima.

## Bukti pengujian

- [Regresi](phase3-regression-tests.xml): 201/201, exit0; fondasi, kontrol, P1/P2, transaksi boss dan P3. [Invokasi](phase3-regression-invocation.json).
- [Pemeriksaan P3 terakhir](phase3-final-tests.xml): 57/57, exit0, sesudah perbaikan NavMesh, counter UV dan collider motor. Kasus ini overlap dengan suite201; **jangan menjumlahkan kedua angka sebagai tes berbeda**. [Invokasi](phase3-final-tests-invocation.json).
- [Pemulihan baterai](phase3-native-budget-result.json): 182 assertion PASS di standalone Linux, fixture boss eksplisit0%, 20 miss nyata, engkol/reset/charging produksi dan rollback CP5 byte-identical. Ini bukan traversal investigasi.
- [Render](phase3-native-visual-result.json): 13 screenshot dari renderer produksi, normal/UV per area, serta sumber model Wewe. [Gambar](Phase3Screenshots/08-arena-normal-fixture.png). Pose/freeze sengaja digunakan untuk komposisi; tidak membuktikan traversal, performa atau animasi gameplay.
- [Traversal utama](phase3-native-main-result.json): **77/77 PASS sampai CP7**, sekitar475m gerak capsule fisik, tanpa belanja/flare. Tiga siklus N→S→E, counter UV nyata, epilog, tiga cancel panel dan final LoadCheckpoint dari disk. [Invokasi](phase3-native-main-invocation.json) mencatat build/assembly. Main route ini memakai --no-secret. [Detour rahasia](phase3-native-secret-partial-result.json) mencapai clue/katup/pelat tetapi berakhir tertangkap AI saat kembali; jangan menyatakannya PASS end-to-end. Kegagalan counter UV dan collider motor ditemukan melalui input produksi, diperbaiki lalu uji utama diulang sampai lulus.
- Windows: [BuildReport](phase3-windows-development-build-report.json) Succeeded, enam scene, 0 error/1 warning, Mono x86_64 Development; [invokasi](phase3-windows-build-invocation.json) exit0. Snapshot6212 file first-party/paket/settings dibekukan dan dicocokkan setelah build. Build lintas platform tidak berarti executable Windows telah dimainkan di host Linux. ZIP **1.228.612.594 byte**,335 file, PE32+AMD64 dan CRC seluruh isi telah diverifikasi. [Receipt paket](phase3-windows-build-verification.json), [ringkasan](phase3-build-summary.json). SHA-256 `4c148ef7ab0c3e91ae78ae97fd0f607c400c45533e180e01445dc84419315c4e`. [Upload R2 terverifikasi](phase3-r2-upload.json): seluruh objek1.228.612.594 byte dibaca ulang; SHA-256 sama dengan ZIP lokal. Rclone sempat menerima HTTP501 pada percobaan pertama, retry bawaan berhasil lalu pemeriksaan penuh lulus. Tidak mengunggah ZIP lama kembali.

Uji native memakai capsule yang diarahkan skrip dan input produksi, save terisolasi, serta timeScale2 pada mode fast. Bukan playtest manusia, penilaian rasa kontrol, atau bukti pacing. Bukti budget memakai build Linux yang sudah memperbaiki counter UV. Render komposisi berasal dari build sebelum perbaikan NavMesh/counter terakhir; invokasi menyimpan SHA assembly dan BuildReport sebenarnya agar batas itu terlihat. Status146 kasus ada di [matriks QA](phase3-qa-matrix.md), bukan diambil dari jumlah fitur atau jumlah unit test.

## Yang masih belum memenuhi presentasi final

Akting VO Indonesia persis naskah belum tersedia. Pose sinematik memegang sepatu/pegangan pagar dan penyamaran Damar belum final; subtitle dan karung bukan pengganti kualitas sinematik itu. Enam nama korban tambahan tidak dikarang; jumlah3+6 dan slot rusak dipertahankan. Review animasi, genggaman HP, loker, fairness pengejaran, keterbacaan UV/lighting, seluruh urutan lampu, FPS30/60, memori, audio ducking, pacing75menit dan penerimaan Windows masih perlu playtest manusia.

Panduan kandidat dan slot review mandiri: [phase3-windows-review.md](phase3-windows-review.md). Penerimaan penuh Phase3 tetap terbuka sampai presentasi tersebut dan acceptance manual benar-benar lulus.
