# Phase 1 — implementation review

Tanggal: 23 Oktober 2026. Baseline audit: `446e6a591491d7509589e2117ad07fa80f67e656`.
Spesifikasi: [Phase1.MD](../Phase1.MD), terutama arahan desainer §0.5.

**Status: build review implementasi; belum phase freeze atau klaim setara DreadOut.**
Kualitas acting manusia, pacing, luring alami, penerimaan visual dan GPU Windows
memerlukan penilaian tersendiri. Daftar 74 QA bukan 74 hasil lulus.

## 1. Yang diterapkan

| Area | Implementasi | Sumber |
|---|---|---|
| Campaign/routing | Schema 1→2 mempertahankan field Prolog; Continue P1 langsung Rusunawa; chapter asing tidak di-reset | `src/Core/CampaignState.cs`, `Phase1Data.cs`, `src/UI/GameShell.cs` |
| Transisi masuk | Tombol malam berikutnya di akhir Prolog; carry-over inventori/Sanity, baterai 100% saat masuk pertama tanpa powerbank grant atau pelunasan tagihan diam-diam | `src/Prolog/PrologFinaleView.cs`, `src/Phase1/Phase1Director.cs` |
| Level | Parkir/pos/pompa, pintu ilusi 102, tangga tertutup, mailbox 207, lift, lorong L3 dan L4 berbentuk loop, jalur pulang | `assets/Scenes/Babak1_Rusunawa.unity`, `src/Editor/Phase1*Builder.cs` |
| Investigasi | AR fisik, gembok 644 existing, cakaran/aura sebagai bukti unik, EVP 7s, keris terpisah, konfirmasi Jurnal eksplisit | `src/Phase1/Phase1Director.cs`, `Phase1Presentation.cs` |
| HP/kamera | Memakai handset/aplikasi/feed/input existing; resolver E untuk cutter/rantai sambil kamera tetap menghadap hantu | `IPhoneChapterContent`, `IContextInteractionResolver`, `AtmajaPhoneView` |
| Ancaman | Tuyul memakai stok nyata, Kuntilanak stationary sebelum cutter lalu pursuit/watch, Genderuwo patroli/LOS/search/hiding dan ritual | `src/Phase1/Phase1Threats.cs`, `Phase1Navigator.cs`, `Phase1Projectile.cs` |
| Daya | Drain/foto/Blitz/powerbank global; dinamo fisik L1/L2/K403 + panel servis L3; recovery arena fisik | `SmartphoneController.cs`, `Phase1Presentation.cs`, `Phase1Balance.cs` |
| Boss/save | CP5 write barrier, tiga halogen unik, 5s exposed, miss dapat diulang, satu CP6 untuk kemenangan/segel/pemakaian keris | `Phase1Threats.cs`, `Phase1Presentation.cs`, `CampaignState.TryCommit` |
| Epilog/keluar | Kain/kaset, video HP 5s pada set non-playable, kembali menjelajah, lift L1/L2, konfirmasi motor CP7; tujuan RSUD disimpan tanpa scene Babak 2 | `Phase1Presentation.cs`, `Phase1StoryBuilder.cs` |
| Arsip/uang | Secret ID unik, arsip dokumen existing, ledger receipt tunggal; Rp78,2 juta naratif tidak dipakai menambah wallet | `Phase1Rules.cs`, `Phase1Data.cs`, `Phase1Director.cs` |

Semua path `src/` dan `assets/` di atas relatif terhadap `Assets/_LayarTerakhir/`.
`Rancangan.MD` tidak diubah. Tidak ada save/AR/event/inventori produksi paralel.

## 2. Kontrak global yang dipertahankan

- Revisi terbaru: baterai menjadi 100% sekali saat transisi Prolog→P1; Sanity/inventori diwariskan persis. Tidak ada WATCH_ONLY di 0%, floor 40% CP5, powerbank awal tambahan atau top-up otomatis CP3.
- Panel L3 dapat dipakai sebelum cutter. Dinamo darurat tidak menyala otomatis dan tidak menaikkan daya di luar interaksi fisik.
- Blitz 15%/6s; charging arena aman saat recovery. Serangan langsung fatal; Sanity gameplay tetap belum aktif pada P1.
- CP5 memulihkan snapshot tepat. Penulisan biasa ditolak selama boss; CP6 dan secret menggunakan transaksi campaign yang sama.
- Lima gerbang/empat segel mengikuti global. Nubuat 1998 ditahan; subtitle kaset sementara hanya menyampaikan kanon yang sudah ditetapkan.
- Babak 2 tidak dimulai. Penutupan CP7 memberi endpoint aman pada menu.

## 3. Aset dan presentasi

Aset menggunakan model existing proyek, VRM yang diberikan desainer, dan sumber
internet berlisensi. [Kredit visual](phase1-asset-credits.md), [manifest unduhan](phase1-external-assets.json),
[kredit audio](phase1-audio-credits.md), dan [manifest suara](phase1-audio-sources.json)
menyimpan sumber. Tidak ada aset/kode DreadOut yang disalin. Tidak ada foto hasil
generasi AI yang dimasukkan ke scene/build.

- Revisi setelah penolakan playtest: bangunan menggunakan panel beton CC0 Axel Kulomaa dengan UV sumber, modul tangga baru, pintu/jendela bertekstur loafbrr_1, dan permukaan 2K serta perabot Poly Haven yang diunduh tersendiri. Material permukaan/furnitur/housing lampu Prolog diganti. Sumber/hash: `phase1-environment-sources.json`.
- Cutter/lampu/panel/generator/radio/kaset/kursi roda memakai mesh unduhan.
- Tiga VRM monster dioptimasi dari sumber pengguna; laporan `phase1-*-authoring.json` menyertakan hash sumber. Gerakan ditargetkan ulang dari animasi Raka/Nyonya yang **sudah ada**, bukan gait baru hasil skrip.
- Label ruang memakai font/shader depth-tested existing. Material VRM Raka disetel secara lokal untuk mengurangi kilap; file karakter global tidak ditimpa.
- Suara tangis/geraman/tawa berasal dari rekaman berlisensi; ambience/foley menggunakan bank existing atau unduhan yang dikreditkan.

**Belum final:** acting dialog/EVP Indonesia, presentasi foto arsip 14 penghuni,
penilaian anatomi/gerakan monster setelah retarget, dressing ruangan dan penerimaan
artistik semua sudut kamera. Subtitle atau properti kecil tidak dianggap pengganti
acting/polish akhir.

## 4. Verifikasi dan batas bukti

Hasil akhir mesin dicatat di `phase1-regression-tests.xml`, `phase1-*-invocation.json`,
`phase1-scene-validation.json`, dan `Visuals/Phase1-Sourced-Grasp/player-audit.json`.
Regresi akhir: suite penuh **148/149 lulus**; satu oracle lama mengharuskan lampu karakter hanya mengenai layer tubuh, sebelum layer tangan terpisah ditambahkan. Oracle diperbarui untuk dua layer karakter yang terisolasi dari lingkungan. **38/38 tes terdampak** (35 babak + 3 kamera) lulus pada rerun. Gabungan 111 kasus lain + 38 kasus terkini memverifikasi **149 kasus unik**; ini bukti dua run, bukan klaim satu run penuh baru. XML, hash dan metode ada di `phase1-verification-summary.json`.
Native menguji rute parkir→pompa→AR102→tangga→644→lift, serta continuation CP3:
Kuntilanak/cutter/rantai, tangga L4, aura, keris, cakaran dan perekaman EVP, hiding gelap, dan konfirmasi Jurnal telah
melewati check produksi. Rute sintetis kemudian tertangkap AI sebelum altar;
ini bukan kelulusan ritual atau perjalanan keluar. **Smoke lengkap ritual→CP7 belum dinyatakan lulus.**
Build review pertama ditolak desainer karena kualitas bangunan. Revisi lingkungan berikutnya wajib diperiksa melalui capture native baru; build lama bukan bukti penerimaan visual.

Native smoke memakai save terisolasi, kapsul collision kontinu, input dan tombol
produksi. **Posisi boss untuk pengujian sakelar/beam/head disusun sebagai fixture**;
itu membuktikan transaksi/hit/geometry tertentu, bukan kelulusan luring alami.
Mode percepatan sintetis, bila dipakai, dicatat pada invocation. Gambar berasal dari
standalone Linux llvmpipe; bukan screenshot Windows atau hasil FPS perangkat sasaran.

Compiler, tes data dan build tidak membuktikan semua variasi input, seluruh 74 QA,
keamanan ritual selama 20 retry, durasi 60 menit, atau kemampuan Iris Xe.

## 5. Hal yang ditemukan saat integrasi

1. Unity tidak dapat menserialisasi komponen babak yang diletakkan pada nama file berbeda; komponen dipecah ke file yang sesuai dan builder menolak script reference kosong.
2. Teks default GUI dapat menembus dinding; memakai shader/font handler existing dan ukuran label fisik yang lebih kecil.
3. Raycast interaksi cutter/dinamo terhalang collider bendanya sendiri; hanya target interaksi tersebut dikecualikan, dinding tetap menghalangi.
4. Kapsul pemain pada target membuat semua goal graf AI tidak terjangkau; target capsule dikecualikan saat planning, collider dinding tetap diuji.
5. Jalur skrip QA perlu mengikuti kaki tangga/sekitar belakang kabin secara fisik. Driver yang menembus sisi geometri ditolak, tidak digantikan teleport diam-diam.
6. Test receipt lama tidak boleh diterima setelah proses dihentikan; runner sekarang memeriksa timestamp XML, jumlah kasus, hasil dan bukti player.

## 6. Penerimaan yang masih diperlukan

- Jalankan [panduan Windows](phase1-windows-playtest.md), termasuk save carry-over normal serta fixture review terpisah.
- Uji semua 74 QA §10: kombinasi urutan bukti, scene reload, kematian/retry berulang, gagal tulis CP6/secret, daya nol, 3 dinamo + panel L3, exit batal/konfirmasi dan arsip opsional.
- Uji puzzle 148, AR dan foto Prolog, departure, checkpoints dan input kamera pada Windows.
- Profil frame time/1% low/memory di perangkat sasaran; jumlah triangles seluruh level bukan ukuran jumlah yang terlihat atau hasil FPS.
- Nilai ketegangan pursuit/luring, jalur sembunyi, jendela 5s, pose tangan/HP dan tangga melalui input manusia.
- Rekam acting, lengkapi foto arsip dari aset yang disetujui, dan lakukan pass visual berdasarkan playtest. Tidak mengurangi mutu untuk mengubah kotak QA menjadi centang.

## 7. Serah-terima build review

Build pertama belum memenuhi penerimaan visual desainer. Revisi berikutnya juga bukan deklarasi seluruh Phase 1 selesai. Gunakan
`Review-Phase1.cmd` → Continue untuk save uji terpisah, atau executable biasa
untuk campaign normal. Laporan upload R2 dan SHA-256 disimpan pada
`Audit/r2-windows-build-upload.json`; verifikasi ZIP/build pada
`Audit/golden-build-summary.json`.

Risiko yang masih perlu diuji khusus: investigasi dan hiding saat ambush EVP,
contact/LOS AI di balik dinding tipis, luring alami dan charging arena, retry
boss berulang, arsip opsional, serta seluruh perjalanan pulang CP7. Jangan
menggunakan kelulusan tes data sebagai bukti area tersebut telah lulus playtest.

Hasil continuation yang belum lulus disimpan eksplisit di `Visuals/Phase1-L3-Continuation/player-audit.json`: AI menangkap rute sintetis setelah konfirmasi Jurnal. Tes tidak mematikan AI atau memberi kemenangan agar terlihat lulus.

Revisi lingkungan setelah playtest: [catatan perubahan dan batas bukti](phase1-environment-revision.md). Delapan capture tersusun telah berhasil direkam pada native Linux; penerimaan visual desainer tetap terpisah dari keberhasilan capture.

Native revisi terbaru memverifikasi kembali AR102, tangga, 644 dan lift. Mode waktu dipercepat gagal di rantai; continuation CP3 dengan waktu **normal** berhasil mengambil cutter, memotong rantai, menyimpan landing dan meresolusi Kuntilanak. Kedua receipt dipertahankan agar batas buktinya jelas.

Perbaikan fitting prop setelah continuation normal: rotasi sumbu FBX dipertahankan dan fitting/collider dihitung pada ukuran dunia. Rerun 34 kasus babak lulus. Native `Phase1-PropFit-CP3` kini berhasil memotret aura **dan mengambil keris melalui interaksi produksi**, setelah sebelumnya gagal pada keris.

Arahan terbaru diterapkan pada §0.7 Phase1.MD: baterai awal 100% pada transisi babak saja, kamera RMB horizontal dua grasp existing, dan 30 tambahan model prop yang berbeda. Hasil tes sebelumnya adalah baseline sebelum revisi ini; gunakan receipt akhir terbaru untuk penerimaan.

Status tambahan terbaru: 30 model prop baru berbeda sudah ditempatkan dan 35/35 tes khusus babak lulus, termasuk penopang/dimensi/clearance dan baterai awal 100% satu kali. Pemeriksaan kamera/placement akhir tercatat pada `phase1-final-targeted-tests.xml`; bukti gabungan akhir ada di `phase1-verification-summary.json`.

Render/input kamera akhir: `Visuals/Phase1-Sourced-Grasp/player-audit.json` dan 10 PNG. RMB tahan/lepas, override app menu, dua grasp, dan pemulihan perspektif lulus melalui input produksi. Tangan memakai unduhan CC0 para/MakeHuman dengan rig sumber; bukan model tangan baru hasil generasi. Referensi/konversi: `phase1-phone-reference.md`, `phase1-sourced-hand.json`.
