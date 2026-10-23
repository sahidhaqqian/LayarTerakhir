# Audit repositori Phase 2 — Diagnosis Tengah Malam

23 Oktober 2026 · baseline kode **`9ca5f57`**, `origin/main` sama saat fetch. Input: draf **2.1-MERGED terbaru di working tree milik desainer**, termasuk perubahan yang belum di-commit; bukan mengembalikan Phase2 ke versi lama. SHA-256 input `54117a9e3e59023b86a64ec18606cf864557731fd8a0222a71b6864290ba645c`.

**Hasil: seluruh mekanik inti P2 layak dibangun secara bertahap; P2 belum diimplementasikan.** Sejumlah asumsi draf tidak cocok dengan sistem aktual dan beberapa aturan berpotensi softlock. [Phase2.MD](../Phase2.MD) direvisi menjadi **2.2-AUDIT-r1**, dengan fondasi reuse §14, alternatif mengikat §15.1, serta matriks audit semua 100 QA §15.3. Cerita enam adegan, Lastri, tiga bukti/tiga pusaka, angka/kode cerita dan tiga siklus 5s dipertahankan.

## Temuan dan tindakan dokumen

| Prioritas | Temuan / bukti | Revisi / kelayakan |
|---|---|---|
| P0 | Transaksi penuh dan write barrier **sudah ada**: [TryCommit/barrier](../Assets/_LayarTerakhir/src/Core/CampaignState.cs#L64); loader hanya schema 2: [loader/validator](../Assets/_LayarTerakhir/src/Core/CampaignState.cs#L264) | Reuse API, rencanakan migrasi 1→2→3/validator; tidak membuat save/rollback/event bus paralel |
| P0 | Chapter 2 belum dikenal: [CampaignRouting](../Assets/_LayarTerakhir/src/Core/Phase1Data.cs#L35); Kos punya Awake/Start dan routing Prolog: [director Kos](../Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L45) | PHASE2_HUB→Kos mode chapter 2, guard sebelum side effect, entry/departure commit sebelum load. Langsung load Kos/schema 3 dengan sistem sekarang tidak valid |
| P0 | Selfie baru flag/toggle: [binding X/V](../Assets/_LayarTerakhir/src/Core/GameInputConfig.cs#L52); [feed/pose/culling](../Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L391) selalu menghadap depan dan cull avatar | Integrasikan pose depan/face/LOS/mirroring pada FeedCamera existing; tidak menambah HP atau feed dunia permanen. Thermo target+dwell belum tersedia |
| P0 | Generator once-per-phase lalu cap 10 tidak menjamin ulang Blitz 15 setelah banyak miss; pola repeatable ada: [Charge P1](../Assets/_LayarTerakhir/src/Phase1/Phase1Presentation.cs#L113) | Recovery aman manual +25/8 repeatable, termasuk pasca-boss; no auto-refill/no Blitz gratis. Baterai nol tidak memutus epilog/CP7 |
| P0 | Resolver consumable hanya mengenali ID P1/legacy: [powerbank resolver](../Assets/_LayarTerakhir/src/Smartphone/SmartphoneController.cs#L183) | Adaptasi resolver instance P2 dan konsumsi attempt RAM→CP6, rollback CP5 tanpa debit/grant ganda |
| P0 | Parent hantu relatif yaw bisa membuat menoleh mustahil; graph P1 flattenY/radius .50: [navigator P1](../Assets/_LayarTerakhir/src/Phase1/Phase1Navigator.cs#L18) | World trail/clearance per-lantai; yaw kontrol bukan avatar/OTS; Quiet Zone detach juga di 0%; ukur perpindahan kapsul untuk hold |
| P0 | CP4 bisa terjadi sebelum CP2; QA lama selalu mewajibkan respawn CP3; menu bisa mengulang grace | Stage/milestone terpisah dari nomor checkpoint; snapshot aman terakhir; grace sekali/attach, prioritas input/modal/focus konsisten |
| P1 | Recorder EVP runtime tersedia: [EVP P1](../Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs#L127), toggle C / 7s P1 | Reuse alur, konfigurasi sesi P2 toggle C / 6s / 2m / LOS; bukan recorder kedua atau mikrofon OS |
| P1 | Audio group existing cuma Ambience/Effects/UI: [routing audio](../Assets/_LayarTerakhir/src/Audio/SceneAudioProfile.cs#L9); belum paket scene RSUD | Exterior/Telegraph/Voice adalah peran baru pada routing existing. Asingkan konten rumah sakit, manifest/lisensi dan acting; tidak menganggap 30 prop/grafik final sudah tersedia |
| P1 | Scene Kos berlabel lama 4x3 tetapi floor tersimpan 5 × 3,8, root/collider unit. Desainer mengonfirmasi pembesaran disengaja | Pertahankan geometri/material/workstation aktual; tidak resize ke 4 × 3 atau 6 × 4,5. Ukuran bidang bukan pengukuran luas bersih/runtime |
| P1 | Draf menggabungkan Dr. Hendra=dokter kepala tanpa bukti, mencampur wallet naratif, dan menjadikan 30 FPS gate | Pisahkan identitas belum disahkan; pertahankan Rp164,8 juta naratif tanpa grant selisih; target global 60 FPS, benchmark 30 FPS / 1% low 24 FPS diagnostik sampai ratifikasi |

**Langsung digunakan:** campaign/save/API, controller/crouch/OTS/FP, input, handset/grip/AR/feed, provider app, interactor/keypad/Jurnal, audio profile/pool dan infrastruktur tes. **Perlu adaptasi:** routing/HUB, migrasi, EVP, consumable, charger, crossing, modal/focus dan input kamera. **Perlu dibuat dalam chapter:** konten RSUD, render Selfie depan, overlay Thermo, satu brain Lastri, laci/formalin, shop provider, konten audio/video. Semua ini pekerjaan implementasi mendatang, bukan hasil audit yang playable.

## Validasi 100 QA dan dokumen

- **100/100 skenario dibaca dan dicocokkan**, ID 01–100 utuh/unik; matriks audit terpisah 100 baris. **34 oracle direvisi**, 66 dipertahankan dengan klarifikasi §15.1; metode E/P/W/G/A dicantumkan per kasus.
- Koreksi mencakup bukti EVP vs dokumen Damar, gate pintu OR vs meja, jendela belajar kedua fatal, snapshot CP3/CP4, LOS/mirror/thermo ilusi, colokan opsional, input RMB/F/Tab, charge setelah miss dan video 0%, serta ilusi yang sudah hilang pada EXPOSED sehingga tidak diuji sebagai target mustahil di window sama.
- **Hasil gameplay P2: 100 NOT_RUN, 0 klaim lulus.** Tidak ada scene RSUD untuk menjalankan PlayMode/end-to-end; tes P1 lama, build Windows sebelumnya dan screenshot Linux bukan penerimaan P2.
- Pemeriksaan struktur dokumen: 100 ID skenario + 100 ID matriks, jumlah 34 revisi cocok input, semua rujukan section lokal terselesaikan, JSON contoh terparse dan ditandai parsial, fenced blocks seimbang, tautan lokal valid. Kontrak prioritas Phase1 §0.5–§0.7 dan Rancangan diperiksa; bagian Phase1 audit historis tidak diperlakukan sebagai status kode sekarang.

## Batas dan urutan berikutnya

Inspeksi statis memakai Graphify untuk source, Unity prefab parser untuk hierarki Kos, serta pemeriksaan terarah YAML/build settings/mixer/manifest/aset. Build Settings berisi Kos/Atmaja/Koridor/Rusunawa; tidak ada RSUD. Tidak ada Unity Editor/PlayMode/build/download dijalankan. Kinerja, animasi/feel Selfie, acting dan mutu aset belum bisa dipastikan dari teks. P1 sendiri belum freeze, sehingga regresinya tetap dependensi.

Urutan: data/routing+HUB → proof Selfie/Thermo → sumber aset/denah slice IGD → kamar mayat → L2 → ritual/CP5–6 → epilog/CP7 → seluruh QA dan profil Windows. Risiko tertinggi: face/grip/culling Selfie, world trail/LOS dan turning, migrasi/guard Prolog, energi serta save-failure boss. Tidak menaksir jam sebelum proof/sampling aset; kompleksitas relatif ada di §14.3.

Keputusan kreatif tetap terbuka: hubungan Dr. Hendra/dokter kepala, jejak Damar E7, penampilan gema AR/aset suster, mutu acting/art dan tier performa rilis. Tidak meratifikasi kanon baru dari audit. **Tidak ada kode/scene/aset/config game diubah**; output adalah Phase2.MD dan laporan ini, ditambah pembaruan Graphify wajib sebelum commit. Tidak ada build ZIP baru, sehingga tidak upload R2 ulang.
