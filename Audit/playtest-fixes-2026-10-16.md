# Revisi playtest Kos–Atmaja — 16 Oktober 2026

Dasar: 16 temuan pengguna, seluruh 10 screenshot di `Bugs/`, dan arahan lanjutan bahwa kamar
Raka berada di lantai 2, dengan lorong, kamar lain, tangga, fasilitas bersama dan gerbang kos.
Suara sintetis disetujui pengguna. Scene/aset kamar approved tidak dirombak; runtime pembuka,
HP dan transisi diperluas sesuai revisi. `goal.md` dan screenshot asli tidak diubah.

## Perubahan per temuan

| No. | Perbaikan | Bukti utama |
| --- | --- | --- |
| 1 | Kursi kayu dengan arah hadap terdefinisi, empat kursi makan menghadap meja | Validasi arah mesh/root; capture 20 |
| 2 | Eksplorasi ditambah ruang tamu depan berpintu, gudang, pencarian jarum di ruang depan; petunjuk AR harus benar-benar dibidik | Tes navigasi kontinu, capture 23–24. Kesulitan/tensi tetap memerlukan penilaian manusia; belum ada musuh/chase baru |
| 3 | Collider model yang diputar dihitung dalam koordinat lokal; step-up saat merunduk diturunkan agar sweep tidak menyangkut sisi bawah meja. Pintu ruang depan membuka menjauhi jalur dalam | Uji CharacterController dari dapur sampai ransel dan ruang depan; tidak memakai waypoint di atas meja |
| 4 | Meja makan diperpanjang; meja tamu kecil dipindah ke pojok ruang depan, bukan pusat lounge/jam | Capture 20 dan 23 |
| 5 | Petunjuk AR berupa mesh guratan/drip darah merah pada permukaan dinding; material AR-only unlit dan shader HP mempertahankan merah meski rumah gelap | Capture 24 dan penghitungan piksel merah framebuffer; layer 29 tidak masuk kamera dunia |
| 6 | Root plate besar recovery tree dibuang, daun diperkecil dan transform di-bake; kursi plastik Atmaja diganti kayu | Bounds pohon seluruhnya di luar dinding; capture 17 dan 20 |
| 7 | Fill light interior tanpa sumber dihapus; dapur/ruang makan mengandalkan HP | Validasi lampu + capture 19. Lorong kos berpenghuni tetap memakai fixture lampu yang terlihat |
| 8 | Jam menghadap ke ruang, punggung flush dinding; 12 angka Romawi radial, pendulum, tik-tok spasial | Validasi komponen setelah reload, capture 25–26 |
| 9 | 16 WAV VO Indonesia generik sintetis: Raka, Damar, Ratna dan bisikan; antrean subtitle dan audio sama | `prolog-voice-manifest.json`, source audio runtime, capture 04/07. Bukan final voice acting |
| 10 | Main menu New Game/Continue/Settings/Exit; ESC pause/Resume/Settings/Main Menu/Exit | Capture 01–02/29 dan tes menu. New Game mengarsipkan save lama, bukan menghapusnya |
| 11 | New Game penuh: kamera bangun dari kasur, monolog, ketukan, suara Ratna lalu surat | Capture 03–05; cutscene menahan input dan menyembunyikan reticle |
| 12 | Jendela menutup setelah vault; Raka menyadari jalan balik tertutup dan harus lanjut | Dialog tambahan pengguna di `KosDialogue.Trapped`; VO bank dan shutter/checkpoint diuji |
| 13 | Pohon diperbaiki dari source yang ada; jalur kiri ditutup reruntuhan tembok/brace, jalur kanan punya crouch fisik | Validasi clearance dan pengujian berdiri tertahan/merunduk lolos; capture 17 |
| 14 | Pintu kamar → lorong lantai 2 → dua flight tangga → fasilitas bersama/halaman → motor/peta di gerbang; mount/ride dan sorot lampu dari lensa motor, melewati gerbang sebelum load Atmaja | Capture 08–15; checkpoint koridor/halaman. Animasi motor masih prototipe first-person, bukan karakter full-body sinematik |
| 15 | Peta skematik di HP (`M`) dan dari motor/gerbang; hanya Atmaja dapat dipilih untuk berangkat di lokasi yang benar | Capture 12; tujuan babak berikutnya terlihat terkunci, belum scene perjalanan bebas |
| 16 | Home HP berisi Kamera/Peta/Pesan/Jurnal/Tas/Setelan. Pesan Damar dapat diputar ulang; jurnal dan tas membaca campaign, dapat digulir | Capture 06–07/12 dan tes aplikasi/input TAB |

## Verifikasi

- **52/52 tes Unity lulus**, 0 gagal/skip, pada source final. Termasuk dua urutan persiapan kos,
  menu/settings, arsip New Game (termasuk slot rusak), aplikasi HP, tangga dua flight/fasilitas bersama,
  perjalanan kapsul kontinu di ruang makan, raycast HP dari samping dan pengambilan jarum di ruang depan.
- Validasi scene revisi dan Atmaja: **0 kegagalan**. Kamar approved tetap byte-identical.
- Standalone Linux OpenGL/llvmpipe menyelesaikan **29 capture aktual 640×360**. Rute kapsul kontinu
  tercatat **121,07 m**, kaki interior maksimum **0,592 m** (bukan naik meja). Motor melewati gerbang,
  lampu pada lensa menyala, checkpoint termuat lagi melalui menu Continue, uang tetap Rp5.500.000.
- Capture AR memuat **428 piksel merah darah** pada area kamera HP; material saja tidak dijadikan bukti warna.
  Tap listener pembuka nyata: RMS 0,0291, peak 0,3562; tanpa clipping digital pada rekaman uji tersebut.
- **Windows Development Mono x86_64 berhasil**, 0 error/warning; tiga scene disertakan.
  ZIP diverifikasi CRC dan kelengkapan 300 berkas. Build lama disimpan di `Builds/Previous/`, tidak dihapus.

Bukti mesin: [invokasi tes](golden-test-invocation.json), [XML tes](golden-tests.xml),
[validasi revisi](playtest-review-validation.json), [receipt player](Visuals/Review-Linux/player-audit.json),
[invokasi player](review-player-invocation.json), [verifikasi ZIP Windows](atmaja-build-summary.json).

ZIP lokal: `Builds/LayarTerakhir-Windows-x86_64-Development.zip` — 158.417.962 byte (≈151 MiB).
SHA-256: `a8e382f2d101456f5e500437e8fef553737f06845a6ef68233cae0d96e8e8829`.

Capture pilihan: [kos lantai 2](Visuals/Review-Linux/08-kos-floor-two.png),
[tangga](Visuals/Review-Linux/09-kos-stairs.png), [peta](Visuals/Review-Linux/12-departure-map.png),
[keluar gerbang](Visuals/Review-Linux/14-riding-through-gate.png),
[jalur meja](Visuals/Review-Linux/20-wood-chairs-clear-aisle.png),
[akses ransel](Visuals/Review-Linux/21-crouched-backpack-access.png),
[darah AR](Visuals/Review-Linux/24-red-wall-ar-writing.png),
[jam di dinding](Visuals/Review-Linux/25-wall-clock-radial-numerals.png).

Metode audit: screenshot berasal dari framebuffer standalone Linux, bukan mockup atau edit gambar.
Interaksi kamar Golden diposisikan untuk regresi; lorong/tangga/Atmaja dilalui CharacterController
dengan substep ≤0,16 m, bukan memindahkan transform antar waypoint. Waktu cerita dipercepat setelah
capture bangun dan dibekukan singkat saat capture; ini bukan pengujian pacing/VO natural, playtest
manual, atau pengukuran performa Windows/Iris Xe. Capture lama `Atmaja-Linux` adalah bukti versi lama,
bukan bukti revisi ini. Percobaan awal terhenti pada deadline audit renderer software; disimpan sebagai
`Review-Linux-Attempt01`, tidak dilaporkan lulus. Attempt02 sudah turun tangga/menjelajah fasilitas/naik motor,
tetapi audit membaca transform motor yang telah dihancurkan setelah transisi terlalu cepat pada timer akselerasi;
audit diperbaiki dengan pemantauan gate di Update dan sampling sinematik terpisah, bukan dilaporkan lulus.
Vulkan host tidak didukung, sehingga memakai OpenGL.

`BeforeRedFix` dan `BeforeHeadlight` adalah rekaman regresi antara yang lolos navigasi, lalu dikoreksi
setelah inspeksi visual. Bukti versi akhir adalah folder **Review-Linux**. Capture 1280×720 percobaan
awal tidak meliputi alur final penuh; jangan menyamakan screenshot diagnostik 640×360 dengan
validasi visual/performa target 1280×720 Windows.

Kamar approved tetap SHA-256:
`4b73b117b832fb753b92d3de885c163bc1dd3f6225101867f7b4e2437c8b793a`.

## Batasan untuk handoff

Untuk melihat animasi bangun dan adegan Ratna, pilih **NEW GAME**, bukan Continue.
Save sebelumnya diarsipkan; Continue sengaja melewati pembuka yang sudah selesai.

- Batas cerita tetap pintu tangga lantai dasar Atmaja. Adegan 4–7, chase/stealth, ritual dan akhir Prolog belum dibuat.
- Ruang tambahan mengubah eksplorasi, bukan klaim bahwa game sekarang sudah sulit/menakutkan. Review rasa bermain tetap perlu pengguna.
- VO sintetis formant sementara, tanpa voice cloning; belum ekspresi/acting final. Prototipe motor adalah kamera mount + tangan first-person + motor/gerbang bergerak.
- Peta adalah diagram lokasi fiksi, bukan peta geografis/navigasi open world. Lokasi mendatang masih terkunci.
- M/Z/ESC adalah shortcut UI tetap. Settings menyediakan remap delapan aksi utama; jurnal saat ini lewat aplikasi HP, bukan J.
- Windows cross-build/ZIP bukan bukti telah dimainkan di Windows. Periksa kembali pencahayaan, audio natural, sensitivitas, FPS dan RAM pada Iris Xe.

Panduan reproduksi lengkap: [atmaja-windows-playtest.md](atmaja-windows-playtest.md).
Kredit/lisensi dan proses VO/mesh: [asset-credits.md](asset-credits.md).
