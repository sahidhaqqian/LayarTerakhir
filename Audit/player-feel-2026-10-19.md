# Revisi core player feel — 19 Oktober 2026

Baseline implementasi: `c9de35d`. Commit pengguna `f9a2783` (bundle review di src) dipertahankan; `git pull --ff-only` sudah memastikan branch sinkron sebelum commit revisi ini. Fokus hanya gerak, jongkok, genggaman/transisi HP, dan kamera pada tiga scene prolog yang sudah ada.

## Sebelum dan sesudah

| Bagian | Sebelum | Perubahan yang diterapkan |
| --- | --- | --- |
| Jalan/lari | Kecepatan horizontal langsung penuh ketika input muncul | Akselerasi singkat, pengereman cepat, steering lebih responsif; maksimum aktual tetap jalan 1,7 / lari 4,2 m/s |
| Kontak kaki | Model mengikuti origin kapsul yang memiliki skin clearance dari lantai | Raycast visual membaca jarak lantai aktual, dibatasi skin width; skala seragam menjaga proporsi/framing kepala dan playback menyesuaikan panjang langkah dunia. Root/collider tidak digeser |
| Berhenti/terhalang | Tidak ada kurva respons kecepatan | Kecepatan berhenti secara bertahap; momentum dibuang saat menu, gerak terkunci, controller dinonaktifkan atau tertahan dinding |
| Animasi gerak | Crossfade 0,16 detik; fase siklus tidak dipindahkan | Fase langkah diteruskan antar-klip gerak/HP, ambang transisi diberi hysteresis, arah mundur membalik playback, tubuh mengikuti arah gerak secara terbatas; ayunan tangan dikoreksi agar berlawanan kaki dan siku lari tetap menekuk |
| Jongkok | Lerp postur bergantung langkah frame; pose rendah masih kaku | Respons postur eksponensial dan tinggi pandang halus. Pose biasa sekitar 0,98–0,99 m dan varian rendah sekitar 0,80–0,81 m menyesuaikan kapsul existing; langkah lebih pendek dan arah lutut dibetulkan, playback mengikuti panjang langkah; kecepatan tetap 0,8 m/s |
| Collider | Kapsul berdiri 1,75 m dan jongkok kos 1,0 m / Atmaja 0,78 m; cek headroom | Dimensi, gravitasi, slide, vault dan cek headroom dipertahankan |
| Mengangkat HP | Panel dan handset langsung muncul/hilang | Transisi 0,24 detik pada panel dan perpindahan pose, dengan input mengikuti status perangkat sebenarnya |
| Genggaman | Pose kanan tetap dan tampilan first-person tanpa tangan authored | Telapak, pergelangan, dan tiap jari diposisikan terhadap sisi handset: empat jari menopang bagian belakang/tepi, ibu jari beroposisi. Mesh tangan yang sama mengikuti HP first-person; handset dunia menunggu grasp tersampel dan mempertahankan grasp selama disimpan |
| Bidikan | Kamera bahu dan kamera HP memakai garis sejajar yang terpisah | Kamera bahu menuju titik pada garis bidik fisik kamera HP; lensa HP mengikuti pivot tanpa tertinggal update visual |
| Kamera dekat dinding | Penarikan/pemulihan jarak langsung | Penarikan saat terhalang tetap segera; pemulihan jarak diperlunak, tanpa tambahan shake. Basis lengan first-person dikoreksi agar masuk dari kanan bawah dan tidak menutupi tengah pandangan |

Gambar sebelum di `Visuals/PlayerFeel-Before/` adalah capture native build sebelumnya yang sudah tersedia, bukan pengukuran baru atau frame animasi identik. `baseline.json` menjelaskan asalnya. Hasil sesudah memakai fixture input aktual yang dijelaskan di bawah; baseline tidak memiliki telemetry yang identik.

## Acuan gerak DreadOut

Sumber primer yang diperiksa: **DreadOut — Official Launching Trailer 2014**, movie ID `2032137` pada [halaman resmi Steam DreadOut 1](https://store.steampowered.com/app/269790/DreadOut/). Cuplikan sekitar 25,5–26,5 detik memperlihatkan jalan dengan ayunan lengan berlawanan kaki; 30,5–33 detik memperlihatkan lari dengan siku menekuk, badan condong, dan framing bahu yang tetap terbaca saat berbelok. File video/contact sheet hanya menjadi bahan pemeriksaan sementara di `/tmp`, tidak masuk aset game atau build.

Yang diadaptasi adalah pemisahan pose jalan/lari, kesinambungan fase langkah, arah tubuh saat berbelok, dan framing tenang. Kurva akselerasi, durasi transisi, serta pose jongkok Layar Terakhir ditentukan dan diuji sendiri; trailer tidak membuktikan angka kontrol, collision, atau fitur jongkok DreadOut. Tidak ada klip animasi, mesh, tekstur, atau kode DreadOut yang dipakai di game.

Instalasi yang benar-benar tersedia di mesin adalah `DreadOut_Remastered_Collection` (Unity 2022.3.28f1), bukan executable DreadOut original 2014. Pemeriksaan lokal terbatas pada metadata read-only scene/klip kampanye utama. Nama seperti `run_1` dan `idle_1` tidak cukup untuk mengidentifikasi locomotion Linda atau menyimpulkan durasi gameplay; nilai stop-time internal legacy tidak diperlakukan sebagai panjang siklus gerak. Executable Windows DreadOut belum dijalankan di host Linux ini. Penilaian referensi geraknya berasal dari cuplikan resmi original, bukan klaim playtest instalasi lokal.

## Kontak jari dengan handset

Pemeriksaan dekat setelah arahan playtest terakhir menemukan bahwa tekukan jari generik masih memotong handset. Perbaikan final mengubah orientasi telapak terhadap handset portrait dan memakai tekukan tiap digit yang berbeda untuk menopang tepinya. Pose tetap dibake ke klip legacy yang sama dan mesh first-person berasal dari pose tersebut; tidak ada solver IK runtime baru. Keyframe ekspor dan impor dipertahankan agar kontak kaki tidak berubah karena reduksi kurva. Tinggi dan langkah jongkok diverifikasi pada delapan fase, termasuk di antara pose utama.

[Bukti mesh ekspor](player-feel-grip-evidence.json) mencatat 190 vertex kulit di dalam handset pada versi genggaman awal revisi ini (130 lebih dalam dari 2 mm), menjadi **0/1.844 vertex** setelah perbaikan. Kliping polygon juga tidak menemukan bidang kulit/jaket melewati interior handset yang diperkecil 1,5 mm. Ini pemeriksaan aset statis, bukan klaim semua frame game atau kualitas Windows sudah lulus.

Closeup memakai FBX ekspor aktual, kamera sama dan material inspeksi netral: [sebelum](Visuals/PlayerFeel-Grip/Before/front.png), [sesudah](Visuals/PlayerFeel-Grip/After/front.png), [samping sesudah](Visuals/PlayerFeel-Grip/After/side.png). Ini render Blender untuk kontak geometri, bukan screenshot gameplay. Tes Unity terpisah memeriksa kontak semua lima digit dan penetrasi kulit pada tujuh klip HP × delapan fase, serta mesh first-person.

## Yang dipertahankan

Tidak membuat scene, cerita, sistem pertarungan/inventori atau puzzle baru. Kode puzzle 148, AR, bukti foto, baterai/fitur HP, ekonomi, save/checkpoint, perjalanan motor dan logika pengejaran tetap memakai sistem existing. Mesh tangan hanya presentasi; tidak memiliki collider atau pengolah input sendiri. Data grafis berasal dari karakter CC0 yang sudah ada, tanpa aset DreadOut.

## Pengujian manual build Windows

- Coba ketukan W pendek, jalan/lari terus, lepas tombol, lalu balik arah atau strafe. Nilai respons awal, berhenti, serta kaki yang menapak, bukan hanya kecepatan maksimum.
- Jongkok sambil diam dan berjalan; lepas CTRL di bawah meja untuk memeriksa headroom. CTRL + Shift + maju tetap dapat memicu slide existing.
- TAB naik/turun HP, V ganti perspektif, Z buka aplikasi. Saat aplikasi memiliki pointer, WASD tetap berjalan dan tahan klik kanan untuk menoleh. Buka catatan saat HP masih dipegang.
- Coba framing target/foto dari bahu dan first-person, lalu save/Continue. Puzzle, AR dan foto tetap memakai fitur yang sama.

## Validasi

### Player standalone: kontrol dan presentasi

**26 pemeriksaan lulus**, dengan 265 sampel gerak dan 15 PNG aktual 640 × 360: [receipt](Visuals/PlayerFeel-After/player-audit.json), [invocation](player-feel-native-invocation.json). Fixture memakai keyboard Input System, CharacterController dan animasi runtime di jalan Melati existing. Setelah penempatan awal, perpindahan segmen ukur berasal dari input. Isolasi perangkat/fokus hanya berlaku pada fixture headless, tidak mengubah kebijakan input game.

| Pengukuran sesudah | Hasil pada fixture final |
| --- | --- |
| Akselerasi jalan, delta simulasi 1/30 | 0,3 → 0,6 → 0,9 m/detik; kemudian mencapai 1,7 |
| Lari dan pengereman | Mencapai 4,2 m/detik; sudah berhenti sebelum sembilan frame pengereman selesai |
| Jongkok berjalan | Mencapai 0,8 m/detik dan memakai CrouchWalk |
| Mesh jongkok normal dari lantai aktual | Titik tertinggi 1,0285 m; titik terendah −0,0030 m |
| Seluruh 25 frame ukur langkah jongkok normal | Deviasi kaki maksimum absolut 0,0207 m; kelebihan headroom kapsul 0 m |
| Mesh jongkok rendah Atmaja dari lantai aktual | Idle 0,8394 m; berjalan 0,8364 m; dengan HP 0,8394 m; titik terendah sekitar −0,0030 m |
| Kontak kaki berdiri | Titik terendah mesh −0,0016 m dari lantai aktual |
| HP / dokumen | Transisi naik/turun, feed ketika dipegang, tangan first-person, dan membaca dokumen sambil memegang HP lulus |
| Kamera / save | Garis bidik bahu/feed selaras, first-person pada pivot mata, save/load checkpoint lulus |

Pengukuran mesh memakai BakeMesh dengan skala impor dan koordinat dunia terhadap lantai aktual. Skin clearance membuat tinggi mesh dari lantai berbeda dari tinggi kapsul nominal. Pengujian mencakup batas kapsul dunia dan kontak lantai, tidak hanya posisi tulang.

Capture yang diperiksa: [jalan](Visuals/PlayerFeel-After/02-walk-stride.png), [lari](Visuals/PlayerFeel-After/04-running.png), [jongkok](Visuals/PlayerFeel-After/05-crouch-settled.png), [jongkok rendah](Visuals/PlayerFeel-After/06a-atmaja-low-crouch.png), [HP saat jongkok rendah](Visuals/PlayerFeel-After/06c-atmaja-low-phone.png), [HP bahu](Visuals/PlayerFeel-After/09-phone-shoulder.png), [grip first-person](Visuals/PlayerFeel-After/10-phone-first-person.png), [catatan sambil memegang HP](Visuals/PlayerFeel-After/11-document-keeps-phone.png). Capture 08/12 menangkap awal perubahan status, bukan tepat pertengahan transisi UI.

### Tes regresi dan perlindungan gameplay

- **98/98 tes Unity lulus, 0 gagal, 0 skip**: [XML](player-feel-full-regression-tests.xml), [invocation final](player-feel-full-regression-invocation.json). Mencakup gerak saat aplikasi HP aktif, collision/headroom, perspektif, puzzle 148, AR/foto, save/checkpoint, dan alur existing.
- Tes geometri impor mengukur **8 klip jongkok × 8 fase = 64 sampel** untuk lantai/headroom; tes grip mengukur **7 klip HP × 8 fase = 56 sampel**, serta mesh first-person statis, untuk penetrasi kulit dan kontak kelima jari ke handset. Kelompok pose tersebut saling tumpang tindih, bukan 120 pose unik. Kontak diuji dengan toleransi 4 mm, penetrasi interior dibatasi 1,5 mm pada fase tersampel.
- **147 pemeriksaan / 30 PNG lanjutan prolog lulus**: [receipt](Visuals/Prolog-PlayerFeel/player-audit.json), [invocation](player-feel-prolog-invocation.json). Mulai dari checkpoint tangga terisolasi dengan progres kos/ground floor selesai: AR/foto kedua perspektif, catatan, lemari, powerbank, handycam, pengejaran basement aktual, escape, Blitz, ending dan Continue. Fixture menggunakan perpindahan CharacterController bertahap dan interaksi existing; ini pengujian jalur terarah, bukan penilaian pacing manual.
- **18 pemeriksaan / 4 PNG motor lulus**: [receipt](Visuals/Rider-PlayerFeel/player-audit.json), [invocation](player-feel-rider-invocation.json). Memeriksa mount, posisi pengendara/grip, gerbang, perjalanan langsung ke Atmaja, model berjalan dan checkpoint courtyard, memakai save yard terisolasi.
- Pembukaan sampai ground floor pernah lulus **233 pemeriksaan / 59 PNG**: [receipt historis](Visuals/Review-PlayerFeel/player-audit.json). Ini preflight **sebelum koreksi grounding/grip final**, bukan pengujian ulang pembukaan final; [snapshot sumber](player-feel-opening-sources.json) disimpan terpisah.
- **19/19 tes tooling Python lulus**; syntax Python dan kompilasi C# Unity lulus. Graphify diperbarui dan diperiksa: **152 file kode, 2.091 node, 7.391 edge, 0 file aset**.
- [Perbandingan sumber](player-feel-validation-source-diff.json): seluruh **2.471 file** pada snapshot suite final dan build Windows identik. [70 file terlindungi](player-feel-preserved-systems.json), termasuk tiga scene, kode Core/save dan gameplay Prolog, package lock serta konfigurasi, tetap sama byte demi byte terhadap baseline.
- Satu percobaan startup editor sempat macet pada GC/UnloadUnusedAssets dan dihentikan, **tidak dihitung lulus**. [Catatan retry](player-feel-test-startup-retry.json) merujuk run pengganti bersih yang lulus 98 tes.

### Build Windows dan distribusi

Windows x86_64 Development, backend Mono, **Succeeded, 0 error, 2 warning BuildReport** pada 19 Oktober 2026 06:32 UTC. Log mencatat CS0252 existing di DesignerReviewPlayerAudit.cs:112; diagnostik shader ray-tracing unsupported dan impor symlink review juga ada. Pengaturan renderer/scene tidak diubah untuk menutupi diagnostik tersebut. [Ringkasan paket](golden-build-summary.json) memverifikasi seluruh file build, CRC ZIP dan executable PE; [verifikasi isi presentasi](player-feel-windows-presentation-verification.json) memeriksa simbol DLL dan resource pose/grip final. BuildReport bukan klaim seluruh log bebas diagnostik.

ZIP final `Builds/LayarTerakhir-Windows-x86_64-Development.zip`: **203.306.829 byte**, SHA-256 `431ffebea1fef585f28ae81d1a79e12ad5a7488d033e1d35443c631eaa242e07`. [Upload R2](player-feel-r2-build-upload.json) selesai dan **pembacaan ulang seluruh objek remote menghasilkan hash yang sama**. Upload awal mengalami respons 501, retry otomatis berhasil; verifikasi readback dilakukan setelah retry. Petunjuk playtest disertakan dalam ZIP sebagai Player-Feel-Review.txt.

Bukti runtime berasal dari standalone Linux/llvmpipe. Fixture feel memakai delta simulasi 1/30; UI memakai waktu unscaled. Peringatan shadow atlas URP masih muncul di Melati. Hasil ini bukan benchmark FPS, bukan jaminan tidak ada clipping pada setiap frame/angle, dan bukan penilaian subjektif bahwa gerak sudah setara DreadOut. **Executable Windows belum dimainkan secara native di host ini**; rasa kontrol, seluruh transisi pada frame rate nyata, dan performa Windows tetap perlu playtest pengguna.

## Sinkronisasi commit pengguna

Commit `f9a2783` tetap utuh. Unity mengimpor bundle review baru di `src/Docs` dan `src/Shaders`, sehingga metadata `.meta` bundle ikut dibekukan sebelum build Windows final. Symlink review ke `ProjectSettings` membuat Unity mencoba mengimpor global game manager dan menulis diagnostik impor; file konfigurasi game tetap tidak diubah. Symlink tidak ditata ulang dalam revisi kontrol pemain ini. Metadata yang sempat dibuat Unity di root `Packages`/`ProjectSettings` adalah efek impor symlink, bukan konfigurasi gameplay, dan dibersihkan sesudah editor berhenti.
