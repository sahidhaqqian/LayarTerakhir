# Phase 5 — implementasi dan batas verifikasi

Basis integrasi: commit Phase4 `5ab018b`. Draft asli desainer disimpan sebagai sumber lokal sebelum rekonsiliasi. `Phase5.MD` memuat keputusan REPO-01–11 dan kontrak loader aktual. Peta kode utama berada di `ARCHITECTURE.MD`.

## Diterapkan

- Scene kedelapan `Babak5_PasarSetan`, gerbang/kios, Koridor Beku, cabang Jembatan Bambu dengan papan runtuh nyata, Ruang Jahit dengan patroli/LOS dan cermin AR, Panggung Lelang, Ruang Cermin Saksi dan tiga set epilog baru. Handoff `PHASE4_CP8` + empat segel langsung ke pasar; resource/global inventory tetap aktual.
- 152 sumber model baru / 652 penempatan, diverifikasi ID/hash/provenance. Lingkungan/replika tidak memakai kit art Phase1–4. Raka/HP/UI/mixer tetap identitas dan sistem bersama; tidak dihitung sebagai model baru. Salinan mentah tekstur yang tidak dirujuk renderer dipertahankan di cache sumber; tidak ada resampling/rekompresi tekstur renderer pada langkah ini.
- Investigasi tiga bukti independen melalui AR+foto, Thermo+UV, EVP asli. Catatan/jurnal memakai sistem HP/HUD existing. Puzzle Pita: layar mati, laci AR, rekaman EVP asli, pengambilan terpisah yang tersimpan sebelum popup. Peringatan Arsip N/5 dan kesempatan kembali sebelum CP6; validasi tetap tersedia di FINAL_CHOICE dan tidak memilih SECRET.
- Replika memakai NavMesh/hearing/LOS, telegraph dan grace; Selfie menghentikan replika S1 sementara. Cover menggunakan collider aktual, termasuk perlindungan jongkok untuk gelombang dan pemulihan arena. Tidak ada teleport masuk lemari atau flag kebal saat memakai dinamo.
- Boss tiga inti: generator/benang UV; Selfie+Thermo+cahaya Lentera/Flare; tiga sapuan dan tiga Kabel Roh AR. Setiap inti membutuhkan satu Blitz sukses. Miss membuka kesempatan baru. Dinamo/Altar Doa/Lentera reusable gratis; Kemenyan dan Flare tidak wajib. Baterai/Sanity0 tidak menyebabkan game-over sendiri.
- CP7 menyimpan setiap inti dan kabel melalui `CampaignState.TryCommit` existing. CP8 menyimpan FINAL_CHOICE, bantuan Damar dan lock ending. CP9 hanya setelah cutscene selesai. Enum global existing bernama `EndingId`, nilai0–4 dan field JSON `ending` tetap sama. Migrasi5→6 dilakukan setelah audit loader; migrasi1–5 dipertahankan.
- BAD memakai dwell2detik + label konfirmasi sebelum LMB; NORMAL wajib tahan F5detik dan lepas membatalkan; TRUE sinkronisasi X→Q→R; SECRET memerlukan lima ID Arsip canonical unik, validasi dan tindakan terpisah. Lock tunggal disimpan sebelum cutscene; Continue mengulang ending terkunci yang sama. Save gagal memulihkan snapshot, tanpa ending/biaya palsu.
- Kapasitor ritual5% dihitung dari state finale dan lock ending. Tidak ditulis menjadi baterai HP. BAD/TRUE/SECRET menghabiskannya setelah lock berhasil; NORMAL tanpa Blitz. Provider energi ritual opsional tidak mengubah fitur babak lain.
- Pencahayaan ambient SH eksplisit untuk standalone, kabut, intensitas praktikal, plafon Ruang Jahit, meja dagangan di jalur pasar, material backface yang tepat dan layar tanpa tripod melayang. Laci Arsip menggunakan mesh laci asli yang bergerak, bukan marker kosong. Titik EVP berada di depan collider layar dan memiliki tes regresi.
- Ambience, rekaman pasar/gamelan, SFX air dan sprite api baru berlisensi. Tidak menambah VO AI/TTS. Subtitle, blocking kamera empat ending dan credits tersedia; daftar35 baris rekaman manusia terpisah.
- Flag `P4_*` pada Phase4Hub diperbaiki agar memakai `phase4`, termasuk forum, depart, modul dan toko. Tidak menulis progres P4 ke phase3.

## Validasi yang telah tersedia

- Run luas pertama: 427/429 lulus; dua ekspektasi schema lama diperbaiki5→6. Run terarah terbaru: **232/232 lulus**, termasuk seluruh campaign suite Phase1–5 serta44 pemeriksaan scene. Lihat XML dan invocation aktual; hasil gagal awal dipertahankan sebagai bukti, tidak diubah menjadi PASS.
- Provenance: **152 sumber model baru, 652 penempatan, 914 pemeriksaan hash, nol konflik/failure**. Ini membuktikan asal/penempatan, bukan penerimaan artistik.
- Empat probe crash native lulus: temp-flushed dan replaced, masing-masing pada progres inti dan lock ending. Proses player benar-benar dihentikan dan save dimuat di proses baru; lihat `phase5-crash-probes.json`.
- Keempat ending dari fixture FINAL_CHOICE lulus lewat input aktual: BAD dwell/LMB, NORMAL hold F5detik termasuk batal4.6detik, TRUE X/Q/R dan escort, SECRET lima Arsip/validasi/escort/capture. Baterai persisten0 tetap utuh; lock tunggal sebelum cutscene, replay setelah reload, dan CP9 lulus. Route investigasi/boss dilaporkan terpisah: run baru dari PHASE4_CP8 dengan baterai/Sanity/uang0, tanpa consumable, **151 pemeriksaan native lulus sampai CP9**, termasuk kematian fase3 dan Continue yang mempertahankan inti1–2 serta reload tiga kabel. Tidak memakai resume antarpercobaan pada run berhasil ini. Runtime tersebut mendahului polish presentasi terakhir; Idle/Walk, material/pintu dan dressing epilog sesudahnya divalidasi scene dan render.
- Pemeriksaan presentasi terakhir: **44/44 scene checks** lulus setelah perbaikan retarget, validasi kedua tangan Damar di bawah kepala, serta **12 capture standalone** selesai. Lihat `phase5-presentation-tests.xml`, `phase5-human-idle-validation.json`, `phase5-native-visual.json` dan `phase5-visual-review.md`. Capture berpose bukan benchmark atau playthrough.
- Build **Windows x86_64 Development / Mono** berhasil dengan delapan scene dan nol error. Lihat `phase5-windows-build.json` dan `phase5-windows-build-run.json`. Pemeriksaan paket dan upload R2 dilaporkan melalui receipt terpisah; keberhasilan build bukan uji runtime Windows.

## Pembacaan QA dan batas kualitas

`phase5-qa-matrix.md` mencatat180 skenario desain beserta dukungan otomasi parsial. Unit/posed-render/native-driver bukan180 playtest manusia. Runtime route, empat ending input dan crash-process probe memiliki laporan tersendiri jika completed/passed=true. Fixture FINAL_CHOICE dipersiapkan eksplisit; bukan bukti bermain seluruh investigasi pada setiap ending. Probe crash memakai slot development terisolasi dan hanya menghentikan proses miliknya sendiri.

VO manusia, akting/animasi epilog yang sepenuhnya dipoles, penerimaan visual setara DreadOut, pacing75menit dan FPS perangkat Windows/IrisXe **belum boleh disebut tervalidasi**. Replika raksasa adalah proyeksi model sculpt non-rigged; replika humanoid memakai motion sumber barunya; Damar memakai Idle/Walk manusia Quaternius CC0 dengan blend sesuai gerak root, sehingga kualitas gerak tetap perlu review. Blocking, subtitle dan pose bukan klaim akting/VO sinematik final.

Build pertama yang menggabungkan pembuatan scene dan kompilasi mencapai batas memori5GiB dan dihentikan cgroup. Pembuatan scene dan build dipisah; scene editor dikosongkan sebelum kompilasi. Review Linux memakai OpenGL sesuai runner, konfigurasi API dikembalikan setelah build. Build/native berjalan berurutan dalam service terisolasi agar tidak mengganggu tmux.

Build Windows pertama mencapai batas5GiB saat serialisasi Phase4; percobaan ber-ambang4GiB dihentikan karena tekanan memori berkelanjutan. Run berhasil memakai MemoryHigh4,75GiB/MemoryMax5,5GiB setelah memeriksa RAM tersedia. Aset/tekstur dan kualitas rendering pemain tidak diturunkan. Lihat dua invocation gagal/retry yang dipertahankan.

## Pengiriman Windows

Build baru delapan scene selesai dengan **nol error/nol warning**. Paket memuat359 file dan112 scripting assemblies; PE x86_64, kelengkapan Data/Mono, seluruh marker babak dan CRC ZIP diperiksa. Ukuran ZIP **3.020.281.380 byte**, SHA-256 `a669808f5fea5941a241a2e73d7f6828856e9c0fec8afa125cd683c42709a05a`. Sumber runtime/scene/art cocok dengan snapshot13.072 file yang dibekukan sebelum build.

Upload ke `r2:layarterakhir/Layarterakhir/LayarTerakhir-Windows-x86_64-Development.zip` diverifikasi 25 Oktober 2026 23:58 UTC melalui pembacaan ulang seluruh objek dan SHA-256 yang sama. Lihat `phase5-build-summary.json`, `phase5-designer-build-sources.json` dan `phase5-r2-windows-build-upload.json`. ZIP Phase4 sebelumnya dipertahankan lokal dalam `Builds/History/`. Tidak ada upload ulang build lama.

Untuk mencoba, ekstrak ZIP lalu jalankan **Review-Phase5.cmd → Continue**. Slot review terpisah; launcher utama tetap memakai campaign pemain. Panduan di `phase5-windows-review.md`. Build/CRC/R2 bukan klaim runtime Windows/GPU sudah dimainkan. Batas VO manusia, penerimaan artistik dan180QA manual pada bagian sebelumnya tetap berlaku.
