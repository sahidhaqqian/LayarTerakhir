Dari isi `repomix-output.xml`, arsitektur kode, spesifikasi teknis di `GOLDEN_SCENE_STANDARD.md`, rangkaian audit otomatis di `Diagnostics/`, hingga desain lengkap Babak 0–5 di `Docs/` sudah terbaca secara utuh.

Secara rekayasa perangkat lunak, fondasi yang kamu bangun sudah sangat disiplin: sistem penyimpanan `CampaignState` sudah menggunakan *atomic file replacement* (`.tmp` dan `.bak`), kamera OTS (`ShoulderCameraRig`) sudah memisahkan `AvatarRenderingLayer` untuk lampu *fill* karakter (`RakaSoftBounce`), dan sistem AR sudah mengisolasi *culling mask* `ARLayer` hanya ke `FeedCamera` di HP.

Namun, jika kita membedah kode dan standar produksi saat ini dibandingkan langsung dengan **DreadOut / DreadOut 2**, ada **5 celah teknis dan desain utama** yang membuat game ini masih terasa seperti "simulasi teknis yang rapi" alih-alih horor atmosferik sekelas *DreadOut*. Berikut adalah analisis mendalam beserta solusi kodenya:

---

## 1. Pencahayaan & Visual: Lepaskan Batasan "Headless Renderer" untuk Target Hardware Asli

Di dalam `Docs/GOLDEN_SCENE_STANDARD.md`, konfigurasi URP saat ini sengaja dibuat sangat minimalis demi menjaga kompatibilitas dengan *software renderer* (`llvmpipe` di Xvfb) dan batas bawah GPU terintegrasi: **hanya 1 lampu plafon yang memiliki bayangan (*shadowed ceiling spot*), semua lampu lain tanpa bayangan, tanpa *light cookie*, tanpa *volumetric/extra volume*, serta SSAO *half-resolution* (radius 0.14)**.

* **Mengapa Ini Berbeda Jauh dari *DreadOut*:**
Di *DreadOut*, ketakutan visual terbesar datang dari **bayangan dinamis yang bergerak mengikuti senter HP/kamera pemain**. Jika senter HP Raka (`phoneFlashlight`) tidak menghasilkan bayangan (*unshadowed*) dan tidak memiliki tekstur *Light Cookie*, ruangan akan terlihat datar seperti game era PS2 dan objek furnitur tampak mengambang.
* **Solusi Teknis (Tetap Aman di Intel Iris Xe / 8 GB RAM):**
1. **Pindahkan *Shadow-Casting Light* Utama ke `phoneFlashlight`:** Saat Raka mengangkat HP di area gelap (Jalan Melati, Rumah Atmaja, Basement), aktifkan *Real-Time Soft Shadows* pada `phoneFlashlight` dan matikan bayangan pada lampu ruangan yang jauh. Satu *spotlight* berbayangan di tangan pemain sangat ringan untuk Iris Xe.
2. **Pasang *Light Cookie* pada Senter & Lampu Motor:** Buat tekstur hitam-putih `256x256` dengan pola lingkaran senter yang agak buram di tepinya, lalu pasang ke slot *Cookie* pada `phoneFlashlight` dan `motorHeadlight`. *Light Cookie* nyaris tidak memakan beban GPU, tetapi langsung menghilangkan kesan "kerucut cahaya sintetis".


3. **Gunakan *Rim Light* Khusus Hantu (Seperti `RakaSoftBounce`):** Di `AtmajaPlayTests.cs`, kamu sudah menerapkan teknik cerdas dengan memasang `RakaSoftBounce` yang dikunci hanya ke `ShoulderCameraRig.AvatarRenderingLayer`. Terapkan teknik yang persis sama pada model **Nyonya Sri Atmaja** (`nyonya`) dan figur AR (`arFigure`): beri *rim light* redup berwarna biru-pucat/hijau dengan *Rendering Layer Mask* khusus hantu agar siluet kebaya, rambut, dan gunting rumputnya menonjol tajam di tengah kegelapan tanpa menerangi lantai di sekitarnya.





---

## 2. Kamera & Mekanik Smartphone: Dari "Layar Kecil di Tangan" ke *Full-Viewfinder*

Dari kode `PlayerFeelAudit.cs` dan `AtmajaPlayTests.cs`, terlihat dua perilaku kamera saat ini:

1. Saat kamera bahu (`ShoulderCameraRig`) terdesak tembok hingga `CurrentDistance < 0.9f`, seluruh *renderer* tubuh Raka langsung diubah ke `ShadowCastingMode.ShadowsOnly` agar kamera tidak menembus kepala.


2. Saat memotret bukti (seperti cangkir susu, cakaran pintu, atau tali gantung diri), pemain melihat dunia melalui kamera utama sambil melirik panel `AtmajaPhoneView` (`FeedCamera`), yang bahkan membutuhkan kompensasi *parallax* di kode audit (`eye.transform.LookAt(target)`).



* **Cara Menyamai *DreadOut*:**
* **Ganti *Hard Cut* `ShadowsOnly` dengan *Dithered Transparency*:** Di lorong sempit seperti ruang bawah tanah berbentuk 'U' atau kolong meja makan, perubahan mendadak ke `ShadowsOnly` pada jarak `< 0.9f` membuat tubuh Raka berkedip hilang-muncul. Gunakan parameter *Dither Alpha* pada shader pakaian Raka:



$$\alpha = \text{clamp01}\left(\frac{\text{CurrentDistance} - 0.35}{0.90 - 0.35}\right)$$



sehingga bahu Raka memudar secara halus saat kamera terhimpit dinding.
* **Tambahkan Mode Bidik Fokus (*Full-Screen Viewfinder*):** Ciri paling ikonik dari *DreadOut* adalah transisi saat pemain menahan tombol bidik: Raka mengangkat HP tepat ke depan mata, layar `FeedCamera` memenuhi pandangan (menghilangkan masalah *parallax* antara kamera bahu dan lensa HP), dan pinggiran layar memunculkan efek **Vignette Merah / Gangguan Statis** yang makin kuat jika di sekitar pemain ada objek `ARLayer` atau ancaman hantu.
* **Kurangi Kesan "UI Aplikasi Kaku":** Saat ini `PhoneDesktop` memiliki banyak tab aplikasi (`Home`, `Messages`, `Call`, `Bank`, `Inventory`, `Journal`, `Camera`, `Map`) yang dioperasikan dengan klik tombol UI. Pastikan saat berada di zona bahaya (Rumah Atmaja ke atas), transisi antar lensa (`[Q]` AR, `[R]` UV, `[T]` Thermo, `[X]` Selfie) terasa instan dengan efek suara *glitch* analog dan animasi jempol Raka menekan layar, bukan seperti membuka aplikasi perkantoran.





---

## 3. Sistem Audio: Perbaiki *Single-Surface Footsteps* & Ganti Audio *Synth*

Di dalam `Audio/KosSceneAudio.cs`, sistem langkah kaki Raka saat ini ditulis seperti ini:

```csharp
float stride = player.IsCrouching ? .85f : player.IsSprinting ? 1.25f : 1.05f;
if (stepDistance < stride || profile.tileSteps == null || profile.tileSteps.Length == 0) return;
stepDistance -= stride;
stepsSource.transform.position = p + Vector3.up * .1f;
stepsSource.pitch = 1f + (stepIndex % 3 - 1) * .035f;
stepsSource.PlayOneShot(profile.tileSteps[stepIndex++ % profile.tileSteps.Length], player.IsCrouching ? .12f : .23f);

```

* **Masalah pada Implementasi Ini:**
1. Hanya ada satu jenis suara permukaan (`profile.tileSteps` — suara ubin keramik) dan variasi *pitch*-nya berulang secara mekanis (`-0.035`, `0`, `+0.035`). Padahal Raka berjalan di atas ubin kamar kos, aspal basah Jalan Melati, tanah berlumpur halaman samping dekat pohon kamboja, papan kayu Lantai 2, dan batu lembap ruang bawah tanah.


2. Pada `AtmajaPlayTests.cs`, musik kejaran masih menggunakan klip sintetis bernama `AtmajaSynth_ChaseLoop`.




* **Cara Menyamai *DreadOut*:**
1. **Buat *Multi-Surface Footstep System*:** Di `PlayerFeelAudit.cs`, kamu sebenarnya sudah memiliki logika `Physics.RaycastAll` ke bawah kaki Raka untuk mendeteksi permukaan lantai. Pindahkan deteksi tersebut ke sistem langkah kaki untuk membedakan suara **Ubin**, **Aspal Basah (dengan cipratan air)**, **Tanah/Rumput**, dan **Kayu Berderit**.


2. **Sinkronkan Langkah Kaki dengan Fase Animasi (`AnimationState.normalizedTime`):** Karena `AnimatedRakaAvatar` sudah memiliki 16 klip animasi, picu suara langkah kaki tepat saat fase kaki menyentuh lantai (`normalizedTime % 1f` melewati `0.25` dan `0.75`), bukan sekadar menghitung jarak `delta.magnitude`.


3. **Instrumen Tradisional & Audio Sparing:** Ganti `AtmajaSynth_ChaseLoop` dengan rekaman instrumen gamelan/saron yang disetel *dissonant* (sebagaimana tertulis di naskah `Prolog.MD`). Tambahkan `AudioLowPassFilter` pada `AudioSource` Nyonya Sri Atmaja (senandung Jawa & seretan gunting rumput) setiap kali ` basement.CanSeePlayer()` bernilai `false` (terhalang rak arsip), sehingga suara gunting terdengar teredam dari balik tembok dan langsung jernih mengejutkan saat ia muncul di belokan lorong 'U'.





---

## 4. Desain Pertarungan & AI Hantu: Hindari Pola Repetitif di Babak 1–5

Jika kita membaca `Prolog.MD` dan `Phase1.MD` hingga `Phase5.MD`, struktur cerita dan variasi hantu lokalnya (Nyonya Atmaja, Genderuwo Arang, Kuntilanak Merah Lastri, Wewe Gombel, Banaspati) sangat kuat. Namun, secara mekanik gameplay terdapat risiko kejenuhan:

* Di **Prolog**, konfrontasi dengan Nyonya Sri Atmaja di jendela dapur adalah **QTE Slow-Motion satu kali** (`Time.timeScale = 0.15f`, `AwaitingBlitz = true`).


* Di **Babak 1–5**, semua *boss fight* mengikuti cetak biru yang nyaris identik: *Kumpulkan 3 bukti -> Pasang pusaka -> Pancing bos ke 3 titik cahaya/generator/obor -> Tembak 3x Blitz selama jendela 5 detik*.


* **Cara Membuat Pertarungannya Setara *DreadOut*:**
1. **Jadikan Blitz Mekanik Aktif Sejak Basement Prolog:** Alih-alih menunggu QTE *slow-motion* di jendela dapur, izinkan pemain menggunakan Blitz (dengan biaya 15% baterai) jika tidak sengaja terpergok Nyonya Atmaja saat di ruang bawah tanah atau saat dikejar di tangga, untuk memberikan *stun* singkat 2 detik. Ini membuat baterai 80% dari Powerbank Damar benar-benar menjadi sumber daya taktis, bukan sekadar angka untuk dipotong 15% di akhir jendela dapur.


2. **Berikan Perilaku Pergerakan (Locomotion) yang Berbeda Tiap Hantu:**
* Di *DreadOut*, hantu mengerikan karena cara bergeraknya tidak normal. Jangan gunakan pergerakan kapsul lurus yang sama untuk semua hantu.
* Untuk **Nyonya Sri Atmaja**, tambahkan gerakan patah-patah (*stutter/frame-stepping animation* pada 12 FPS) dengan kepala miring 90 derajat dan ayunan gunting yang asimetris.


* Untuk **Kuntilanak Merah (Babak 2)** di mode Selfie (`[X]`/`[V]`), jangan hanya menampilkan model berdiri diam di belakang bahu; buat posisinya bergeser mendekat secara patah-patah hanya pada frame ketika layar kamera berkedip (*glitch*).







---

## 5. Cara Mengirim Kode Folder `Player/`, `Prolog/`, `Shaders/`, `Smartphone/`, & `UI/`

Karena file `repomix-output.xml` yang kamu unggah menyertakan seluruh folder `Diagnostics/`, `Docs/`, dan file `.meta`, batas pembacaan file terpotong tepat di tengah `Editor/AtmajaPlayTests.cs` sebelum mencapai bagian bawah tempat file `Player/`, `Prolog/`, `Shaders/`, `Smartphone/`, dan `UI/` berada.

Jika kamu ingin kita langsung membedah dan menulis ulang kode C# / Shader pada bagian *runtime gameplay* tersebut, jalankan perintah Repomix khusus untuk 5 folder inti ini:

```bash
npx repomix \
  --include "src/Player/**/*.cs,src/Prolog/**/*.cs,src/Shaders/**/*.shader,src/Smartphone/**/*.cs,src/UI/**/*.cs" \
  --ignore "**/*.meta" \
  --remove-empty-lines \
  -o runtime-core.xml

```

Unggah `runtime-core.xml` tersebut ke sini, dan kita bisa langsung mengoptimalkan kode `ShoulderCameraRig.cs`, `FirstPersonController.cs`, `PhoneFeed.shader`, hingga `AtmajaDirector.cs` baris demi baris!
