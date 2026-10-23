# Audit Phase 1 v2.2 terhadap repositori Layar Terakhir

Tanggal: **23 Oktober 2026 (UTC)**. Baseline implementasi: **`e5ab282d204838aee07b78797dd9d5eeab0809d5`**. Dokumen hasil penyelarasan: [Phase1.MD — 2.2-r1](../Phase1.MD).

## 1. Kesimpulan eksekutif

**Phase 1 layak dibangun secara bertahap, tetapi belum siap langsung diimplementasikan penuh atau dinyatakan phase-freeze.** Repositori sudah mempunyai fondasi pemain, HP, AR, HUD, gembok, dokumen, save atomik, pola receipt event, stealth/hiding, audio dan pipeline build. **Belum ada level Rusunawa atau gameplay Babak 1 yang terintegrasi.** Tiga scene build semuanya masih Prolog. Nama komponen dan ID P1 dalam desain bukan bukti fitur sudah tersedia.

Risiko terbesar bukan membuat puzzle 644, melainkan **routing Continue P1, transaksi data dan rollback, batas autosave boss, interaksi L3 tanpa mengalihkan pandangan, serta sinkronisasi ekonomi HP global**. Memindahkan naskah menjadi satu director besar atau menyalin sistem Atmaja akan memperbesar risiko regresi.

Temuan utama:

1. **P0 — Continue P1 belum dikenali.** `GameShell.Continue` memuat Kos; bootstrap Kos hanya mengenali checkpoint P0 lalu bisa memanggil `StartNewGame` untuk checkpoint asing. Routing harus ditangani sebelum save P1 diuji melalui menu. Ini analisis jalur kode, bukan klaim ada save P1 pemain yang telah rusak.
2. **P0 — Profil HP v2.2 bertentangan dengan global.** Drain, biaya foto/Blitz, powerbank, pengecualian kamera 0%, dan energi halogen tidak boleh diterapkan bersamaan tanpa keputusan. Default r1 kembali memakai profil global dan recovery daya arena; alternatif tetap ditandai.
3. **P0 — Atomic file belum sama dengan atomic gameplay transaction.** Save existing melindungi file, tetapi rollback seluruh perubahan campaign dilakukan oleh pemanggil. CP5/CP6/CP7 dan secret perlu batas transaksi yang jelas; schema 2 contoh belum dapat dimuat.
4. **P0 — Gate Jurnal hilang dalam v2.2.** Global mensyaratkan konfirmasi Jurnal; tiga bukti + keris saja belum cukup. r1 mencantumkan receipt `P1_JOURNAL_CONFIRMED`.
5. **P0 — Raycast E dan gate kamera baterai 0% tidak memenuhi L3.** Perlu resolver proximity khusus dan kebijakan kamera encounter pada HP yang sama; TAB/F lock saja belum menutup jalur Stow, Jurnal, RMB release dan switch app.
6. **P1 — Sanity P1 bertentangan dengan global mulai Babak 4.** Default r1 mempertahankan nilai save tanpa simulasi Sanity P1. Usul napas +1 m dan dua-hit boss tetap keputusan terbuka.
7. **P1 — Kanon lima gerbang/empat segel sudah jelas.** Bukan keputusan baru. Yang belum jelas ialah nubuat kaset 1998, ledger sumber uang, carry-over resource dan beberapa baseline kesulitan.
8. **74 skenario terdaftar lengkap dan unik, tetapi belum mempunyai bukti kelulusan end-to-end P1.** Receipt lama 114 tes dan native Linux hanya membuktikan regresi/fondasi Prolog pada sesi sebelumnya. Tidak dihitung sebagai 74 QA P1 lulus.

**Hasil sesi ini:** laporan ini dan penyelarasan dokumen `Phase1.MD` menjadi **2.2-r1**, mengikuti instruksi susulan desainer. Tidak ada implementasi, refactor, scene baru, perubahan kode/aset/prefab/config game, build baru, atau upload ulang ZIP.

## 2. Metode, provenance, dan batas kesimpulan

### 2.1 Sumber yang diperiksa

- `Phase1.MD` v2.2 lokal dibaca seluruhnya, **1.162 baris**, termasuk enam adegan, registry, diagram state, CP1–CP7, 74 QA, dan review Claude/Gemini. File ini **sudah mempunyai perubahan lokal pengguna** ketika audit dimulai. SHA-256 sebelum penyelarasan: `6cbcd9ac8168e73a3d8a4c5e2d05909ae51d138715c1424d531ddaa539d13501`.
- `Rancangan.MD`, 84 baris, SHA-256 `326df6a5085fcf5a0f0004e52aabfe80d940358bd3ad579378a1a3eed4cf2bfe`; dipakai sebagai aturan global, tidak diubah.
- Salinan historis `Assets/_LayarTerakhir/src/Docs/Phase1.MD`, 283 baris, revisi 15 Oktober, SHA-256 `fb5c45d88d31455c85d2b5429627ec049ccf9899a3d1bf6b93c4708262055a55`; **bukan** sumber v2.2.
- Graphify lokal digunakan sebelum navigasi source, kemudian source relevan diperiksa langsung. Peta graph bukan bukti gameplay.
- Scene/prefab diperiksa lewat MCP `unity-prefab-parser` yang tersedia dan pemeriksaan terarah YAML/GUID komponen/build settings. Tidak menjalankan Unity atau authoring builder. Cache parser lokal dapat diperbarui oleh tool; tidak dijadikan perubahan konten game.
- Membaca implementasi Core, controller/interactor/kamera, HP/view/apps/HUD, director/AI/checkpoint Prolog, audio, validator, build dan beberapa tes terkait. Pencarian ketiadaan simbol dibatasi pada runtime first-party terkait; tidak menganggap hasil nama class saja cukup membuktikan perilaku.
- Remote `origin/main` diperiksa pada awal audit dan sama dengan baseline. Pekerjaan dokumentasi dilakukan pada worktree terpisah; aset VRM mentah, output lokal dan pekerjaan tak terkait tidak dimasukkan.

**Cara membaca bukti:** nomor baris source merujuk baseline di atas. Nomor Phase 1 sebelum revisi disebut “v2.2 asli”; tautan `Phase1.MD` saat ini menunjuk r1, sehingga gunakan judul bagian untuk mencocokkan. Nilai hash input mencatat desain yang benar-benar diaudit, bukan mengesahkan build.

### 2.2 Tingkat kepastian

| Label | Arti |
|---|---|
| **Terverifikasi statis** | Ada/tidaknya field, API, guard, scene build, isi dokumen dan jalur kode yang diperiksa. Tidak otomatis berarti kualitas gameplay lulus. |
| **Inferensi** | Risiko integrasi atau estimasi berdasarkan kontrak dan jalur kode; perlu tes terisolasi/runtime untuk membuktikan perilaku aktual. |
| **Belum dinilai runtime** | Navigasi Rusunawa, feel encounter, 60 menit pacing, kualitas VO/VFX P1, FPS/VRAM/memory Windows dan hardware sasaran. Levelnya belum ada. |
| **Bukti historis** | Receipt tes/build sebelumnya; dibaca ulang, **tidak dijalankan ulang dalam audit ini**. |

Tidak ada klaim FPS Iris Xe dari headless/Linux; tidak ada pengujian game Windows baru, fault injection runtime, compiler check C# baru, atau seluruh jalur QA P1. Tidak ada aset/kode proprietary DreadOut yang disalin atau diperlukan dalam audit ini. Pembacaan runtime tidak memerlukan pencarian internet.

### 2.3 Indeks bukti konkret

Semua path source di bawah berada di repositori yang sama. Tautan baris dapat dipakai saat meninjau lewat GitHub.

| Bukti | File dan titik pemeriksaan |
|---|---|
| **E01** | [Rancangan.MD](../Rancangan.MD#L16): kontrol L22; baterai L30–32; Sanity L34; offline/performa kamera L36; ekonomi L38; lore L40; secret L44–50; scope/performa L74–76; data/transaksi L80–84. |
| **E02** | [CampaignState.cs](../Assets/_LayarTerakhir/src/Core/CampaignState.cs#L23): kontrak schema 1 L23–44; singleton L68–75; item unik L83–95; dokumen autosave L105–118; pembayaran/rollback L121–137; file atomik L140–173; load/backup/validasi L176–252. |
| **E03** | [SmartphoneController.cs](../Assets/_LayarTerakhir/src/Smartphone/SmartphoneController.cs#L18): state/unlock L18–39; input L87–137; drain L139–169; powerbank L172–193; flashlight L196–224; pulse L232–249. |
| **E04** | [GameInputConfig.cs](../Assets/_LayarTerakhir/src/Core/GameInputConfig.cs#L9): action/binding L9–60; V/X migration L76–79; remap L81 dst. |
| **E05** | [FirstPersonController.cs](../Assets/_LayarTerakhir/src/Player/FirstPersonController.cs#L61): clamp kecepatan L61–71; lock/input L80–98; akselerasi L104–112; gerak relatif badan/postur L138–195. |
| **E06** | [ShoulderCameraRig.cs](../Assets/_LayarTerakhir/src/Player/ShoulderCameraRig.cs#L12): OTS/FP dan override L12–30; input perspektif L78–98; aim/collision L125–156. |
| **E07** | [InteractionInteractor.cs](../Assets/_LayarTerakhir/src/Player/InteractionInteractor.cs#L19): modal lock L19–25; E dari raycast L28–34; kamera/jarak/LOS L37–83. [IInteractable.cs](../Assets/_LayarTerakhir/src/Player/IInteractable.cs) dan [HingedDoor.cs](../Assets/_LayarTerakhir/src/Prolog/HingedDoor.cs#L17). |
| **E08** | [AtmajaDirector.cs](../Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs#L57): bootstrap L57–78; derived stage/restore L83–101; transaksi rollback L105–112; AR L126–132; live L170–175; charger P0 L219–225; death L332 dst. |
| **E09** | [AtmajaPhoneView.cs](../Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs#L50): framing hard gate baterai/center L50–63; feed L64–105; chat L138–167; CaptureFlash L198–202; RMB hold/release L226–249; 0%/feed/app L251–327; queue bounded L350. |
| **E10** | [PhoneDesktop.cs](../Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs#L27): app shortcuts/stow L27–51; tujuan terkunci L72–97; bank L178–195; journal P0 L209–247; daftar item hardcoded L249–252. |
| **E11** | [HudController.cs](../Assets/_LayarTerakhir/src/UI/HudController.cs#L333): document membuka autosave L333–338; modal L339–369; padlock panjang dinamis L372 dst. [PresentationProfile.cs](../Assets/_LayarTerakhir/src/Core/PresentationProfile.cs#L24): UiFocus membedakan phone browsing vs hard modal. |
| **E12** | [AtmajaUpperDirector.cs](../Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs#L168): commit/restore L170–197; lensa/LOS L273–283; tiga foto P0 berurutan L285–291; biaya/hadiah L305–312. |
| **E13** | [AtmajaBasementHorror.cs](../Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs#L175): hiding/FP override L175–198; state update L200–240; Blitz 15%, stun 2s L244–258; stimulus L289–303; graf jalur planar/capsule L305–341; hiding inspection L356 dst. |
| **E14** | [AtmajaChaseSequence.cs](../Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs#L444): ending L444–484, termasuk top-up nominal naratif ke wallet_bound L449–455. [PrologFinaleView.cs](../Assets/_LayarTerakhir/src/Prolog/PrologFinaleView.cs#L78): endpoint Prolog/menu, bukan transisi Rusunawa. |
| **E15** | [GameShell.cs](../Assets/_LayarTerakhir/src/UI/GameShell.cs#L135): Continue→Kos L135–142; pause Time.timeScale=0 L93–100; settings/rebind subset L144–160. [KamarKosPrologDirector.cs](../Assets/_LayarTerakhir/src/Prolog/KamarKosPrologDirector.cs#L60): route P0 dan fallback StartNewGame L60–74. [PrologSceneFlow.cs](../Assets/_LayarTerakhir/src/Prolog/PrologSceneFlow.cs#L17): checkpoint classifier P0. |
| **E16** | [EditorBuildSettings.asset](../ProjectSettings/EditorBuildSettings.asset#L8): tiga scene P0. [ProjectVersion.txt](../ProjectSettings/ProjectVersion.txt), [manifest.json](../Packages/manifest.json): Unity 6000.3.26f1/URP17.3/Input1.20/AI Navigation2.0.15. Package terpasang bukan bukti NavMesh runtime. |
| **E17** | [SceneAudioProfile.cs](../Assets/_LayarTerakhir/src/Audio/SceneAudioProfile.cs#L9), [ThreatAudioOcclusion.cs](../Assets/_LayarTerakhir/src/Audio/ThreatAudioOcclusion.cs#L24), [PresentationProfile.cs](../Assets/_LayarTerakhir/src/Core/PresentationProfile.cs#L15); mixer/voice/profile, occlusion probe 0,1s dan LUT suara berdasarkan teks. |
| **E18** | [FoundationAuditTests.cs](../Assets/_LayarTerakhir/src/Editor/FoundationAuditTests.cs#L193): snapshot/backup, write fail, corrupt schema/duplicate ID L193–246; [FoundationPlayAuditTests.cs](../Assets/_LayarTerakhir/src/Editor/FoundationPlayAuditTests.cs), [PlayerFeelTests.cs](../Assets/_LayarTerakhir/src/Editor/PlayerFeelTests.cs): fondasi terarah, bukan suite Rusunawa. |
| **E19** | [designer-vrm-test-summary.json](designer-vrm-test-summary.json), [designer-vrm-full-tests.xml](designer-vrm-full-tests.xml): historis 114/114 suite Editor; native feel 30, rider18, Prolog153 checks; windowsRuntimeTested=false dan targetHardwareFpsMeasured=false. |
| **E20** | [AtmajaSceneValidation.cs](../Assets/_LayarTerakhir/src/Editor/AtmajaSceneValidation.cs#L68): cap 350k triangles, designer albedo2048/environment1024; [DESIGNER_VRM_MODELS.md](../Docs/DESIGNER_VRM_MODELS.md): model aktif Raka/Nyonya, Damar authoring; enam monster sumber belum aktif. |
| **E21** | [WindowsDevelopmentBuild.cs](../Assets/_LayarTerakhir/src/Editor/WindowsDevelopmentBuild.cs#L26): memasukkan scene enabled setelah startup; [package_windows_build.py](../Tools/package_windows_build.py#L31): posisi tiga scene P0 tetap; perlu validasi tambahan P1, jangan menghapus jaminan P0. |
| **E22** | [ARCHITECTURE.MD](../ARCHITECTURE.MD#L377): aturan ekstensi/reuse; beberapa angka historis 300k/1024 tidak lagi cocok validator karakter terbaru E20. Source/validator terbaru lebih kuat dari ringkasan arsitektur lama. |

## 3. Sistem tersedia, reuse, dan celah sebenarnya

| Sistem | Terverifikasi tersedia | Dapat digunakan ulang | Pekerjaan P1 yang masih dibutuhkan | Prioritas/risiko |
|---|---|---|---|---|
| Gerak dan animasi | CharacterController, acceleration/braking, run/crouch/slide, headroom, avatar VRM, gait/hand poses. E05/E06. | Pertahankan controller/pose, collider dan default P0. | Kecepatan mundur terarah/config encounter; cutter/chain animasi dan volume clearance. Tidak perlu controller pemain kedua. | P0 integrasi L3; risiko regresi feel tinggi. |
| Kamera | OTS dan FP, pilihan tersimpan, collision spherecast, override FP hiding, physical aim convergence. E06. | Rig yang sama untuk Rusunawa. | Watch dari FeedCamera, restore perspektif/cursor setelah encounter, toleransi bounds/LOS, kontrak RMB. | P0 untuk fairness; bukan alasan kembali FP-only. |
| Interaksi | IInteractable, E raycast2,6m, nearest solid hit, modal guard; HingedDoor. E07. | Semua pickup, switch, radio, lift, altar memakai kontrak ini. | Resolver proximity hanya cutter/rantai; satu pemilik aksi tiap frame, obstruction dari tangan, prioritas exit hiding. | P0. |
| HP dan AR | Satu HP, AR/UV enum eksklusif, layar/live terpisah, baterai, powerbank besar/kecil, layer29 feed. E03/E09. | State/perangkat/RenderTexture yang sama. | Provider konten P1 kecil; K102 visual normal/AR + collider latch; modul EVP; no duplicate AR world service. | P0; view masih punya coupling P0. |
| Foto/Blitz | Foto bukti P0 tervalidasi mode/sudut/LOS, framing, flash visual, emergency stun dan chase photos. E09/E12/E13. | Reuse kalkulasi lensa/feedback dan consumption HP. | Dispatch konteks tunggal, cooldown authoritative, kepala Genderuwo anchor, tiga halogen unik. CaptureFlash bukan API damage/energi. | P0. |
| Jurnal/dokumen | Arsip dokumen dengan write rollback, list/scroll dan callback modal. E02/E10/E11. | UI/penyimpanan yang sama. | Bukti P1 bebas urutan, konfirmasi identitas, Arsip N/5, clue/history. Tidak mengubah tiga rantai P0 menjadi bukti P1. | P0 gate; P1 presentasi. |
| Puzzle kode | Padlock dinamis, callback satu kali, test kode3/4 digit. E11/E18. | Konfigurasi kode 644, reward receipt. | Papan enam cap MENUNGGAK dan arsip Tari terpisah; input tak memberi dua reward. | P1, risiko rendah–sedang. |
| Inventory | List string unik dan AddInventoryItem idempoten. E02. | Tetap satu inventory campaign. | Dua powerbank memerlukan instance ID unik/descriptor consumable dalam inventory ini; UI/UsePowerbank/steal harus mengerti tipe baru. Counter kedua tanpa sinkronisasi dilarang. | P0 data. |
| Campaign/save | Data, schema 1, temp+Flush(true)+Replace+backup, newgame archive, load validasi. E02. | Wajib dipakai; tidak ada alasan save kedua. | Nested P1 serializable, migrasi1→2, event receipts, transactional rollback, checkpoint anchors/routing, clock gameplay, boss write barrier. | P0 paling awal. |
| Event cerita | Panggilan director/Commit dengan receipt banyak disimpan di inventory; stage dipulihkan dari flags. E08/E12. | Pola idempoten/rollback. | Director P1 ringkas + data; tidak ditemukan event bus generik di runtime terkait. Jangan mengarang bahwa bus sudah ada atau memperkenalkan framework bus hanya demi nama di desain. | P0, hindari refactor global. |
| AI dan hiding | Nyonya Patrol/Suspicion/Pursue, LOS/noise/screen range, locker inspection/FP, graf waypoint planar. E13. | Pola perilaku dan beberapa helper jika memang bisa dipakai tanpa coupling. | Tuyul, Kuntilanak watch, Genderuwo stealth/boss baru; tidak drop-in Nyonya terikat Atmaja. Navigation package ada, NavMeshAgent runtime terkait dan NavMeshData pada tiga scene P0 tidak ditemukan. | P0 jalur; P1 kualitas. |
| Resource recovery | Powerbank +80/+35 dan charger P0→35. E03/E08. | API energi dan feedback. | Dinamo normal/darurat kumulatif, cooldown gameplay persisten, safe windows, recovery arena global. Jangan menyalin charger35 sebagai mekanik P1 tanpa tuning. | P0 anti-softlock. |
| Siaran/chat/ekonomi | Lokal/offline, live state, queue bounded, toast, wallet legitimate/bound/spent, bank. E02/E09/E10. | UI/bus notifikasi/wallet yang sama. | Timeline P1/event idempotent, challenge exclusive, ledger receipt, milestone naratif terpisah, history. Random ambient chat P0 tidak menggantikan clue deterministic P1. | P1, transaksi P0. |
| Audio/atmosfer | Profile/mixer/ambience/footsteps/occlusion/subtitle dan voice lookup. E17. | Routing dan playback. | Materi P1: tangisan inverse-distance, Tuyul, Genderuwo, EVP, tape, radio, lift, halogen, VO emosional. Ketersediaan pipeline bukan bukti kualitas VO. | P1 kualitas; P0 cue fairness. |
| Build/test | Unity6/URP, Windows Mono Development, snapshot/packaging, Linux diagnostics, test fixtures. E16/E18/E19/E21. | Perluas pipeline existing. | Scene dan checkpoint P1, receipts/fault injection/Windows input & GPU nyata. Append scene tanpa menggeser indeks P0 yang sudah dijaga packaging. | P0 integrasi; P1 shipping. |

## 4. Kontradiksi dan keputusan penyelarasan

Perbandingan berikut memakai **v2.2 asli**. Kolom r1 menjelaskan koreksi dokumen yang dilakukan, bukan fitur yang diimplementasikan.

| ID / prioritas | Benturan dan bukti | Dampak bila diterapkan literal | Penanganan pada Phase1 2.2-r1 |
|---|---|---|---|
| **K01 / P0** | v2.2 §8.0: normal0,015/s, AR extra0,10/s, EVP extra0,20/s, flash4%, powerbank 50%; E01 L30/E03 menetapkan0,05/0,10 total, foto 1%, Blitz 15%/6s, besar 80. | Resource budget/cooldown/overlay berbeda lintas babak, biaya tersembunyi, fitur lama berubah. | Tabel memakai default global; alternatif dipisahkan. Besar80, kecil kos35 existing. Cost usir Tuyul mengikuti klasifikasi Blitz baseline yang perlu ratifikasi D01. |
| **K02 / P0** | v2.2 §4.5/8.0: Blitz halogen0%, tidak ada charger arena. Global E01 L32: recovery daya arena saat recovery; E03 dan E09 menolak camera/flash/framing0%. | Boss tetap tak dapat dibidik pada0% meski biaya digratiskan; mengganti seluruh aturan HP diam-diam. | Default15%/6s + recovery arena global. Halogen0% dan WATCH_ONLY menjadi pengecualian D01 yang ditahan. |
| **K03 / P0** | v2.2 altar hanya3 bukti + keris; E01 L28 menambahkan konfirmasi Jurnal. E10 belum mempunyai deduction confirm. | Ritual terbuka sebelum syarat global terpenuhi. | Tambah gate dan receipt P1_JOURNAL_CONFIRMED pada registry, CP5, pseudocode/QA; wajib UI explicit, bukan konfirmasi otomatis. |
| **K04 / P1** | Sanity turun/regen dan napas+1m sejak P1 §8.1A; E01 L34 mulai Babak4. E02 hanya field sanity, tidak ditemukan konsumsi runtime terkait. | Kanon baru dan sistem kedua terselubung. | Default P1 pasif/preserve; alternatif ditahan D02. Dua-hit boss juga proposal, bukan sistem health yang sudah ada. |
| **K05 / P0** | §6.2 schema 2 dan nested JSON dictionaries; E02 menerima hanya schema 1, serialisasi JsonUtility, list ID. Contoh phase1/evidence/checkpoint duplikat campaign. | Save tidak bisa dimuat; dictionary tidak menjadi kontrak serializer existing; sumber kebenaran ganda. | Contoh menjadi satu root campaign, nested P1 hanya state khusus, list receipt serializable, migrasi eksplisit. Stage schema 1 yang dibayangkan v2.2 tidak ada dalam kode aktual. |
| **K06 / P0** | Initial1powerbank + mailbox1; E02 AddInventoryItem menolak ID sama, E03 hanya mengenali dua nama powerbank existing. | Unit kedua hilang atau counter berbeda dari inventory; pencurian/konsumsi tak sesuai. | Rencana instance ID/descriptor pada inventory yang sama; grant awal ditahan D06, tidak menambah count paralel. |
| **K07 / P0** | Proximity E L3≤1,3/1,6m vs E07 wajib look-at kamera. Watch0% vs E09 feed/FramesTarget/RMB hard gate. | Encounter tidak mungkin sesuai desain jika source disalin apa adanya. | Resolver proximity terbatas konteks; satu action owner; exception watch hanya setelah D01. Semua gate dicatat. |
| **K08 / P1** | §3.3A/8.0 maju2,6/mundur1,8 vs E05 walk<=1,7 dan speed sama maju/mundur. | Hitungan10s / 2,42s bukan runtime; footslide jika speed global dipaksa. | Catat nilai efektif dan angka alternatif1,83s; retune lokal berbasis playtest, preserve P0. |
| **K09 / P1** | E01 V=Selfie/FP prototype/engine belum dikunci, tetapi desain pengguna dan E04/E06 sudah V=perspektif, X=Selfie, Unity OTS/FP. | Kontrol kamera pengguna hilang akibat dokumen global historis. | Pertahankan desain terbaru yang sudah diminta pengguna; bukan meminta persetujuan ulang OTS. Catat utang sinkronisasi global; Rancangan tidak diubah sesi ini. |
| **K10 / P1** | v2.2 §11.2/13.2 menganggap5gerbang/4segel/chapter select belum diputuskan. E01 L20/44–50 sudah jelas. | Membuang waktu atau mengubah kanon yang sudah ditetapkan. | Tutup sebagai keputusan global yang ada; hanya nubuat kaset1998 tetap D05. Chapter select backlog, bukan kebutuhan MVP. |
| **K11 / P1** | Rp78,2juta belum ledger; E01 menegaskan milestone kumulatif terpisah saldo. E14 justru menutup selisih Prolog dengan AddBoundDonation sampai25,45juta. | Dana sah/terikat dan milestone tercampur, kompensasi fiktif saat P1/reload. | Overlay milestone terpisah; jangan menyalin top-up wallet P0 ke P1. Sumber ledger dan kaitan kontrak500juta perlu D03; jangan menulis donor untuk menutup selisih. |
| **K12 / P1** | v2.2 pembuka mengklaim kos/RS lunas dari malam sebelumnya; E02 TryPayFamilyBill opsional dengan receipt pembayaran, E14 ending tidak memaksanya. | Campaign valid tanpa bayar berkontradiksi dengan kuitansi intro. | Jadikan keputusan carry-over/transfer eksplisit D03/D06; jangan diam-diam mendebit wallet atau memalsukan receipt. |
| **K13 / P0** | §6 CP5 no-write BOSS_ACTIVE vs ArchiveDocument/Bank existing langsung save E02/E11; browsing app tidak menghentikan dunia E11. | Snapshot CP5 tertimpa keris sudah dipasang/lampu1–2 saat membaca/bayar. | Pagar tulis campaign P1 semua entrypoint, alternatif tampilan dokumen read-only; CP6 satu transaksi. Bukan hanya mematikan timer autosave director. |
| **K14 / P0** | CP4 “safe” sesudah setiap bukti vs EVP tepat memicu ambush; v2.2 contoh secret Add lalu save tanpa rollback. | Restore pada posisi terjepit, ambush tak konsisten, UI secret sukses padahal disk gagal. | Anchor aman+receipt ambush terpisah; transaction rollback untuk secret; sukses UI setelah commit. |
| **K15 / P0** | CP P1 dan transisi RSUD ada di dokumen; E15 hanya route P0 dan fallback reset, E16 scene P1/P2 tidak ada. | Continue salah scene, progression gagal, endpoint memuat scene tak ada. | Routing P1 didahulukan, P1_COMPLETE tetap checkpoint/next destination; jangan mengimplementasikan RSUD pada milestone P1. Endpoint berikutnya perlu desain aman. |
| **K16 / P1** | 60FPS global sementara E01 L76 vs30FPS720pLow targetminimum v2.2 §10.3 dan E22; tak ada benchmark GPU. | Klaim performa diturunkan tanpa keputusan atau optimasi semu dari headless. | 30/24 minimum sebagai usul tier D07,60FPS aspirasi tetap. Keduanya belum hasil uji. |
| **K17 / P1** | v2.2 §9 menyebut event bus/PhoneBatterySystem/ARRevealTarget seolah reusable, source tidak memiliki API bernama demikian. | Implementer membuat sistem baru karena mencari nama fiktif. | Peta integrasi nyata §14; nama candidate hanya tanggung jawab, bukan API. Tidak ada alasan refactor luas. |
| **K18 / P1** | Dua Phase1: rootv2.2WIP vs salinan15Oktober. E22 juga memuat budget300k sedangkan E20 kini350k. | Desain/budget lama dipakai sebagai otoritas saat produksi. | Root Phase1 sumber aktif; salinan historis tidak diubah. Source/validator terbaru jadi bukti; jangan menganggap350k jaminan P1 30FPS. |

### 4.1 Yang sudah konsisten dan tidak perlu didesain ulang

Kode644 mempunyai logika6–4–4; pembedaan cap MENUNGGAK dan nama Tari mengurangi kebocoran secret. K402/EVP/aura adalah bukti berbeda; pusaka terpisah. K404 utuh sebelum ritual dan dua bukaan setelah ritual menutup bypass yang diketahui desain. Tiga lampu **hanya habis setelah hit sukses** sesuai aturan global. P1_CUT_CHAIN sebagai satu resolved flag, lift pulih setelah boss, epilog tanpa fade, exit sadar di motor, serta Arsip opsional sebelum keluar selaras dengan kampanye. Semua ini **konsisten pada desain**, belum diverifikasi pada level P1.

## 5. Kelayakan mekanik, navigasi, dan state

### 5.1 Jalur dan geometri

Rute kritis dan jalur pulang logis. Gunakan satu scene P1 terkontrol pada milestone awal dengan root area L1–L4; keputusan teknis ini rekomendasi, bukan scene yang dibuat sekarang. Hindari framework streaming baru sebelum profil menunjukkan perlu. Void terbuka membuat lantai lain berpotensi tetap terlihat: tidak boleh mematikan renderer/collider area yang terlihat atau sedang dipakai hanya berdasarkan stage.

| Area | Penilaian | Kriteria yang harus dibuktikan pada blockout |
|---|---|---|
| Parkir/pos/pompa | Layak, gate intro belum lengkap. | Jangan menambah invisible wall untuk memaksa urutan. Registry P1_ENTER_PUMP tidak mensyaratkan live/log, tetapi CP1 mengasumsikan keduanya; putuskan state bila pos/live dilewati. Baterai carry-over0 sebelum MULAI LIVE tak boleh terhalang dari dinamo pompa. |
| K102 dual visual | Layak dengan latch; belum ada komponen passage reusable. | AR menampilkan pintu hanya pada feed; main camera masih normal. Collider perlu grace saat kapsul berada di lintasan; P1_CROSS_102 sukses commit membuat jalur dua arah permanen. Uji write failure ketika crossing, bukan collider toggle setiap frame. |
| L2 mailbox/arsip | Risiko engineering rendah–sedang. | Gembok3digit reuse; clue lingkungan cukup tanpa chat. Arsip terpisah dari papan6, rute tidak bergantung secret. Pengambilan ulang tidak memberi uang/item kedua. |
| Lift barang | Risiko sedang–tinggi. | Landing L1 servis pompa, L2 arsip, L3 encounter; tidak L4. Entry L1 terkunci awal meskipun dapat didekati dari dalam, pulih sesudah kemenangan. CharacterController/HP/camera tidak jitter/tertinggal saat lift bergerak; bisa disimulasikan transisi singkat tanpa framework platform umum. Restore tak menaruh kapsul dalam pintu/lantai lift. |
| L3 40m | Layak bersyarat input/watch/tuning. | Jalur mundur bebas hambatan, cutter radius dari jalur bukan ceruk terlalu dalam; lantai datar, rantai LOS tangan aman. Desain hantu tertatap tetap mengejar dan kontak fatal konsisten. |
| L4 loop/K403 | Layak; risiko AI/nav tinggi. | Dua escape route, nomor kamar/landmark empat sisi, cover/hiding valid. Volume Genderuwo2,5 m harus melewati panel404 dan kamar mandi tanpa memakai radius Nyonya0,23m secara buta. |
| K404 sebelum/saat/sesudah boss | Layak dengan restore topology. | Dinding utuh mencegah bypass sebelum ritual; collapse terbatas panel; jalur boss/nav diperbarui saat perubahan geometri. CP5 membangun ulang dinding, CP6 mempertahankan bukaan/pintu terbuka walau keris pindah. |
| Rute pulang/exit | Desain v2.2 sudah kuat. | L3 resolved sebelum boss sekalipun re-entry; sesudah boss lift L1/L2, K1020% tetap terbuka. Panel motor hanya UI; batal tidak memanggil save/event final. |

### 5.2 Angka L3: matematika valid, asumsi runtime belum valid

Model v2.2: pemain x22, hantu x9, pintux40, pemain1,8 m/s, hantu2,4 m/s. Waktu18/1,8=10s; hantu x33; celah7m; menunggu diam sampai tangkap radius1,2 memberi (7−1,2)/2,4=**2,4167s**. Ini benar untuk model kecepatan konstan dan E di x40.

E05 membatasi walk menjadi1,7 m/s, maju/mundur sama. Pada1,7 konstan: **10,5882s**, hantu x34,4118, celah5,5882m, margin **1,8284s**. Akselerasi, collision, posisi titik interaksi dan E sudah bisa dimulai≤1,6m membuat nilai riil berbeda lagi. Karena itu QA-45 bukan tes sekadar menyamakan konstanta. Uji actual controller pada30/60FPS, frame spike, diagonal, crouch tak sengaja dan remap; catat effective backward velocity, waktu pickup selesai dan waktu E diterima. Pertahankan feel P0; konfigurasi lokal1,8 atau tuning speed/posisi hantu harus diuji, tidak otomatis mengubah global walk2,6.

### 5.3 Satu pemilik aksi dan watch

Prioritas yang direkomendasikan: menu/pause dan animasi wajib → encounter interaction → UI HP → aksi foto/Blitz → interaksi dunia normal. Gunakan GameInputConfig/action existing. Satu klik/E tidak boleh ditangani beberapa komponen dalam frame yang sama.

- `PhoneDesktop.Stow` dapat menyimpan HP langsung; Z/M/J membuka app, RMB release memulihkan app/raised state lama. Mengabaikan TAB/F saja belum menjaga kamera L3.
- Jika D01 WATCH_ONLY dipilih, seluruh guard0% di SmartphoneController, AtmajaPhoneView screen/feed/FramesTarget/RMB, input dan shader perlu scope encounter; tidak mengisi baterai, tidak membuka AR/EVP/Blitz. Jangan menset battery epsilon untuk menipu validator.
- Watch memakai frustum/bounds dan LOS lensa HP. Third-person camera dapat melihat dari samping tembok; bukan oracle. `FramesTarget` saat ini memakai titik y+1,45m, bukan target kepala2,5 m atau toleransi watch seluruh siluet.
- `WATCHED_IDLE` diam, timer baru sesudah pickup selesai. Pada PURSUIT: catch≤1,2m diproses sebelum E; E valid membekukan gerak/damage/watch2s, tidak cancel, lalu chain-cut dan resolved dalam transisi sama. Disk failure chain harus masuk state aman dengan retry, bukan mengejar lagi dari jarak nol.
- Definisikan efek kehilangan fokus jendela, pause, membuka dokumen dan Jurnal, serta selected perspective sesudah resolve. Belum ada alasan menambah rebinding layer/input framework kedua.

### 5.4 Bukti, EVP, stealth, dan boss

**Bukti:** reuse sudut/jarak/LOS dan feedback P0, tetapi jangan menyalin `TryPhotoTick` yang mengharuskan urutan cangkir→cakaran→tali. P1 perlu **enam permutasi** tiga bukti, ID tepat, scan aura terpisah dari pickup keris, plus konfirmasi Jurnal. Re-scan tidak menghabiskan reward/menambah count.

**EVP:** binding C tersedia, recorder belum ada. Implementasikan sesi in-game pada HP existing; tidak membutuhkan mikrofon OS atau audio/network eksternal. Tentukan durasi rekam, radius, syarat arah, abort/cost saat layar mati, hilang dari area, pause/reload. Bukti diberikan hanya setelah selesai. Ambush segera dobrak namun perjalanan ke bilik3,5s perlu rute nyata; search punya batas selesai yang dapat diobservasi. Restore `evidenceEVP=true && ambushDone=false` dari anchor aman, tanpa duplikasi bukti.

**Genderuwo:** pola stimulus/hiding Nyonya relevan, namun component terikat Atmaja dan graf planar. Buat AI P1 dengan prioritas reveal/ambush→stealth→boss yang eksplisit; jangan menggeneralisasi seluruh Nyonya. Screen-off menghapus stimulus cahaya, tidak menghapus LOS. Perubahan pintu/404 membatalkan cache jalur atau memperbarui link yang terkait. Agent/helper navigation di lantai4 cukup; jangan mengizinkan monster memilih waypoint lantai lain hanya karena XZ sama.

**Boss:** tiga lampu unik, available/active/completed dan activeWindow terkait satu lampu; shield/headshot check memakai satu transaksi aksi. Cooldown6s global lebih panjang dari window5s: jika Blitz belum ready ketika switch diaktifkan, pemain perlu indikator jelas dan kesempatan retry tanpa kehilangan lampu. Saat EXPOSED tidak ada lemparan, damage tertunda, atau Tuyul yang sudah aktif terus mencuri; larangan spawn baru saja tidak cukup. Pukulan ketiga mematikan AI/proyektil sebelum efek kemenangan. Transient WOUNDED perlu keputusan D02; tidak ada alasan membuat statistik HP penuh.

### 5.5 Resource budget dan softlock

V2.2 normal0,015/s berarti **0,9%/menit**, sedangkan global0,05/s = **3%/menit**. Global AR0,10/s = **6%/menit**; “AR extra0,10 + normal0,015” menjadi6,9%/menit, bukan profil global. EVP tambahan0,20/s menambah12%/menit tanpa keputusan. Pada layar normal aktif60 menit, kebutuhan teoritis180poin; AR terus-menerus360. Ini contoh budget ekstrem, **bukan prediksi kebiasaan pemain atau hasil playtest**.

Tuyul maksimal20poin/10s di luar drain mode, lalu paling banyak satu powerbank yang benar-benar ada. Cadangan awal dan mailbox perlu bisa disimpan sebagai dua unit. Recovery dinamo0% harus bisa dimulai tanpa HP, akumulasi45s harus bisa disela patroli, cooldown120s dihitung waktu gameplay yang tersimpan. Saat loaded, jangan membandingkan `Time.time` sesi baru dengan timestamp sesi lama atau jam OS.

Emergency dinamo cap10% cukup untuk AR/foto/EVP singkat, **tidak cukup untuk Blitz15%**; ini bukan softlock Tuyul jika ia nonletal dan boleh timeout. UI jangan menyuruh pemain melakukan aksi yang mustahil dibayar tanpa jalur recovery lain. Durasi rekam EVP/scan/reach harus dibatasi agar perjalanan dari recovery tidak menghabiskan10% sebelum aksi wajib. Default boss memakai recovery arena global; kondisi aman, besaran refill dan waktu cooldown recovery belum rinci dan wajib diuji. Halogen0% bukan solusi yang boleh diaktifkan diam-diam.

## 6. Kontrak state, checkpoint, dan pemulihan

### 6.1 Empat lapisan, satu sistem penyimpanan

| Lapisan | Isi | Aturan |
|---|---|---|
| Campaign persisten | ID bukti, relic, secret, item/instance consumable, segel, wallet, upgrade, receipt cerita. | `CampaignData` existing; list unik tervalidasi, bukan sumber kebenaran ganda. |
| Snapshot checkpoint | chapter/checkpoint bernamespace, anchor/yaw aman, state area dan battery snapshot, data kampanye yang sah. | Diserialisasi ke file campaign yang sama lewat atomic API. Label CP bukan posisi otomatis. |
| Attempt runtime | altar sementara, keris sementara terpasang, tiga lampu, activeWindow5s, projectile, injury timer, chain-watch timer. | Tidak ikut disk selama boss; reset penuh CP5. |
| UI/presentasi transient | exit panel, app focus, raise blend, pose/cursor, crosshair/glitch. | Dipulihkan dari konteks; buka/batal panel bukan receipt completion. |

Data `CampaignData.evidence_ids` tetap authoritative; jangan membuat `phase1.evidence_ids` kedua. Inventory, battery, sanity, checkpoint juga tetap memakai field existing. Phase1 boleh menyimpan event dan receipt khusus; jangan memigrasi massal EVENT_P0 dari inventory demi kerapian. `schema_version=2` baru dipakai setelah migrasi/validasi didukung. Unity JsonUtility memerlukan record/list serializable untuk challenge/dynamo timestamps; HashSet/dictionary runtime dibangun dari list dan tidak dipersist sebagai salinan kedua.

`CampaignState.SaveCheckpointAtomic` mengembalikan checkpoint ID saat write gagal, **bukan seluruh mutasi Data**. Pola backup JSON/FromJsonOverwrite di AtmajaDirector.Commit dapat dipakai lokal. Baru setelah write sukses: hide pickup, mark secret sukses, keluarkan reward/kutscene, atau fade. Jika gagal: data, state perangkat dan world/UI harus konsisten untuk retry. Save atomic normal bukan klaim kebal terhadap seluruh kegagalan OS/hardware; fault injection tetap perlu.

### 6.2 Checkpoint yang layak dan gap tersisa

| CP | Transaksi/restoration yang diperlukan | Gap/risk |
|---|---|---|
| **CP1** | Arrival/app/live/log sesuai kejadian yang benar-benar terjadi; spawn pompa/pos yang aman. | Tablev2.2 mengasumsikan log+live, registry membolehkan pump tanpa keduanya. D09 menentukan gate; jangan memaksa dokumen opsional atau reset receipt. |
| **CP2** | Mailbox open+kunci+powerbank reward sekali; objek reward hilang sesuai receipt. | Inventory harus dapat menyimpan dua powerbank; posisi safe lift/L2; secret pickup tidak mengganti CP tinggi menjadi CP2. |
| **CP3** | Landing sebelum manifest; top-up max(current,20) sekali **jika disetujui**, snapshot baterai tepat; cutter di ceruk/rantai utuh. | Watch-only0% D01; snapshot tidak ditambah20 tiap retry. Sesudah chain-cut, cutter+chain perlu satu commit dengan anchor landing L4 yang aman; belum diberi label CP khusus. |
| **CP4** | Sesudah masing-masing bukti, ID tepat dan anchor aman; keris/door404 terpisah; Jurnal confirm. | Safe bukan posisi scan di depan boss. Pisahkan EVP receipt dan ambushDone. Tentukan checkpoint saat keris/jurnal didapat tanpa scan baru. |
| **CP5** | E di depan altar, bukti lengkap+Jurnal+keris, disk sukses dahulu; snapshot keris masih inventory/dinding utuh/3lampu utuh. | Default baterai snapshot; minimum40 adalah D06. Perlu pagar semua write saat fight, bukan hanya director; pembayaran/dokumen/menu harus patuh. |
| **CP6** | Sekaligus bossWin+segel lokasi+keris pindah ke altar+lift pulih. AI/projectile berhenti; world restore membuka404. | Write fail: tampilkan recovery/retry commit aman atau pulihkan CP5. Tidak membagikan segel/loot sebelum commit. |
| **CP7** | Kain→kaset selesai→epilog selesai→confirm motor; simpan P1_COMPLETE+tujuanRSUD, baru fade. | Belum ada sceneRSUD; endpoint aman harus didefinisikan tanpa membuatP2. Cancel motor tidak save; replay kaset tidak memutar epilog/reward lagi. |

Receipt `P1_TAPE_PLAY` harus membedakan selesai dari sekadar mulai. Jika disimpan saat mulai lalu epilog membaca flag itu sebagai tape selesai, reload tengah rekaman dapat melewati cerita. Tetapkan receipt selesai atau state replayable; bukan timer audio disimpan di disk.

Rahasia sesudah boss wajib menyimpan **kemajuan terkini**, bukan snapshot lama ketika pertama keL2. Reload/death dari save terkini mempertahankan ID; restore backup lama adalah tindakan rollback eksplisit dan tidak boleh diam-diam mengambil secret campaign lain. Campaign ID/arsip backup policy perlu dijaga bila kelak ada merge. NewGame tetap kosong.

### 6.3 State flow yang direkomendasikan

Flow makro mengikuti enam adegan. Stage diturunkan dari receipt, bukti, keris dan konteks, tidak dari integer stage tersimpan. ALTAR_READY berbasis proximity adalah kondisi interaksi sementara; tidak boleh menghapus kemajuan investigasi ketika pemain menjauh. P1_CUT_CHAIN satu-satunya resolved L3; BOSS_WIN hanya memperbaiki lift.

Boss memakai attempt object runtime yang dapat dibuang, bukan salinan inventory/HP/save service. Director P1 memberi handoff eksplisit ke AI terkait. Pause/menu, animasi wajib dan replay restore mempunyai kebijakan gameplay clock yang sama; `Time.unscaledTime` hanya cocok untuk UI/presentasi yang memang tetap bergerak, bukan countdown5s atau drain saat pause.

## 7. Evaluasi seluruh 74 skenario QA

**Semua baris belum diverifikasi end-to-end pada P1.** Fondasi berarti kode/tes P0 dapat membantu, bukan fitur P1 sudah lulus. E = EditMode/data; P = PlayMode/scene; W = Windows standalone/input/visual; H = hardware/pacing manual; F = fault injection filesystem/quit terisolasi. Kasus dengan keputusan terbuka memakai oracle r1 setelah keputusan terkait disetujui.

| ID | Fokus dan kesiapan | Bukti fondasi / gap | Verifikasi yang diperlukan |
|---|---|---|---|
| QA-01 | Tanpa saweran: layak. | E02/E10; jalur P1 belum ada. | P+W: matikan reward opsional, tamat tanpa paywall. |
| QA-02 | Lewati chat: layak. | E11; clue P1 perlu konten. | W: K102/644 tetap dapat disimpulkan dari objek/dokumen. |
| QA-03 | Scan cakaran tiga kali: layak. | E02 unik, E12 gate P0. | E+P: exact evidence ID tetap satu, bukan jumlah screenshot. |
| QA-04 | EVP lalu reload: perlu state terpisah. | E02/E13; EVP baru. | P+F: antara rekam dan ambushDone, pulih aman; bukti satu dan replay terkontrol. |
| QA-05 | Aura tanpa pickup: layak. | E02 evidence/relic. | E+P: aura ada, keris tidak ada, ritual ditolak. |
| QA-06 | Keris tanpa bukti lengkap: layak. | E07; topology P1 baru. | P+W: masuk/keluar 404 boleh, fight dan arena lock belum dimulai. |
| QA-07 | Ritual sekali: tambah konfirmasi Jurnal. | E08 transaksi, UI confirm baru. | E+P: tiga ID + Jurnal + keris + E valid; failed CP5 tidak memulai ritual. |
| QA-08 | Flash pada shield: layak. | E09 flash hanya presentasi. | E+P: nol segel; biaya/cooldown ditangani satu action owner. |
| QA-09 | Miss/timeout lima detik: layak. | Boss baru. | P: shield pulih, lampu utuh dan dapat dicoba ulang. |
| QA-10 | Spam satu lampu: layak. | E02 pola unik; lampu baru. | E+P: window terikat satu light ID, paling banyak satu hit. |
| QA-11 | Tiga lampu sukses: layak. | E02 location_seals. | P+F: tiga ID berbeda, CP6 menyimpan boss/segel/keris sekaligus. |
| QA-12 | Duplicate secret: perlu transaksi. | E02 field ada, pickup service belum ada. | E+P: exact ID satu, UI sukses hanya sesudah write berhasil. |
| QA-13 | Secret setelah mati/reload: perlu checkpoint. | E02 full snapshot. | P+F: save terkini mempertahankan ID; New Game kosong, backup lama dijelaskan. |
| QA-14 | Menang tanpa secret: layak. | E15 pola travel; rute P1 baru. | W: kembali L2 sebelum konfirmasi motor, epilog tidak fade. |
| QA-15 | Data akhir setelah reload: perlu migrasi/routing. | E02/E15. | E+W: sentinel semua field, kedua wallet, upgrade, segel; Continue P1 aman. |
| QA-16 | Tuyul mencuri stok nol: layak. | E02/E03; Tuyul baru. | E+P: count tidak negatif, mailbox tetap dapat dicapai. |
| QA-17 | Baterai rendah sebelum puzzle: perlu recovery. | E03; dinamo baru. | P+W: mulai 0/1/di bawah biaya; dinamo pompa/L2 terjangkau tanpa HP. |
| QA-18 | AR mati di K102: perlu latch. | E09 layer/feed. | P+W: toggle/stow saat crossing tidak menjepit kapsul; jalur pulang terbuka. |
| QA-19 | Spam kode/reward: layak. | E11/E18 padlock, E08 receipt. | E+P: salah→benar→spam→load, kunci satu dan instance powerbank satu. |
| QA-20 | Mundur, cutter, rantai: risiko tinggi. | E05/E07 belum proximity. | W: input nyata, tidak wajib memutar kamera; ukur kecepatan efektif. |
| QA-21 | Kamera animasi rantai: perlu freeze. | E06 override, encounter baru. | P+W: AI/damage/watch berhenti dua detik, pose kembali, grace tidak dapat dieksploitasi. |
| QA-22 | Mati di L3: perlu CP3. | E15 pola death/Continue. | P+W: cutter kembali, rantai utuh, hantu idle, baterai snapshot. |
| QA-23 | Spawn saat hiding/menu: layak. | E13 guard/hiding. | P+W: spawn tidak overlap atau menyerang dalam cutscene/modal. |
| QA-24 | EVP dipicu ulang: perlu dua receipt. | E08 pola commit. | E+P: bukti dan ambushDone terpisah; tidak ada bukti/dobrak ganda sesudah selesai. |
| QA-25 | Dua puluh kegagalan boss: layak. | Boss baru. | P: miss/timeout/spam terarah, selalu tersisa jalan menang. |
| QA-26 | Frame drop worst case: belum dinilai. | E19 bukan FPS, E20 statis. | H+W: frame capture boss/halogen/Tuyul/chat/hujan, RAM/VRAM dan GC. |
| QA-27 | Secret lintas babak: layak sebagai fixture. | E02 secret_items. | E: ID valid P1/P2, N/5 dari himpunan sah; lokal P1 independen. |
| QA-28 | Spam epilog: perlu ledger receipt. | E08/E14. | E+P+F: donasi, kain, kaset, segel paling banyak sekali; tidak menambal saldo. |
| QA-29 | Pause saat exposure: layak. | E15 timeScale, E09 UI unscaled. | P+W: timer/drain/proyektil berhenti sesuai pause, UI boleh bergerak. |
| QA-30 | Reload video/dokumen: perlu receipt selesai. | E11/E14. | P+F: loot tidak hilang; replay beat yang belum selesai tanpa reward ganda. |
| QA-31 | Nol baterai K102/EVP: perlu dinamo. | E03 recharge pada 0 tersedia. | W: dinamo tanpa HP; progres 45 detik dapat dilanjutkan, EVP tetap terjangkau. |
| QA-32 | Nol baterai boss: oracle diselaraskan. | E01; halogen 0% ditahan D01. | W: default recovery arena→Blitz 15%/6s→menang; alternatif hanya jika disetujui. |
| QA-33 | Dua puluh retry CP5: oracle diselaraskan. | E02; CP5 P1 baru. | P+F: baterai persis snapshot, tiga lampu utuh, uang/item tidak berulang; floor40 hanya D06. |
| QA-34 | Kamera turun sebelum cutter: layak. | E09 stow; encounter baru. | P: ghost idle x9, timer belum aktif sampai pickup selesai. |
| QA-35 | Berdiri menatap hantu: layak. | E13 pola contact. | P: pursuit mencapai radius1,2 lalu fatal; tatapan tidak menghentikan gerak. |
| QA-36 | F/TAB/E refleks: perlu action owner. | E03/E07/E10 mengubah state langsung. | P+W: feedback input ditolak, E proximity bekerja dan tidak double-interact. |
| QA-37 | Chain→boss→return: layak. | E15 pola route. | W: resolved dari chain; lift baru pulih setelah boss, hantu tidak respawn. |
| QA-38 | Kode644 tanpa chat: layak. | E11 UI; konten baru. | W manual: deduksi dari papan6, log L4, denah4; bukan membaca kode source. |
| QA-39 | Konsistensi Tari/EVP/kaset: D05. | E01 chronology; naskah P1. | Review editorial+W: dapur konsisten, nubuat1998 perlu persetujuan. |
| QA-40 | Challenge sukses/expired: perlu enum. | E02 belum ledger generik. | E+P: status eksklusif, transaksi sekali, expiry saat pompa tidak memberi reward. |
| QA-41 | Sanity0 dan puing: D02. | E02 hanya field Sanity. | E+P: default Sanity pasif; oracle dua-hit8s baru berlaku jika disetujui. |
| QA-42 | K403/switch/exposure: layak bersyarat. | E07 door; boss baru. | W: K403 terbuka, dinamo eksplorasi mati saat boss; exposed tanpa lemparan/Tuyul. |
| QA-43 | Rp78,2 juta vs wallet: ledger D03. | E01/E02/E10/E14. | E+W: milestone terpisah, tidak double-add atau menutup selisih dengan uang fiktif. |
| QA-44 | Migrasi save lama: prioritas awal. | E02 reject schema/duplikat, E18 korupsi. | E+F: v1 valid→v2 dan backup; kasus ambigu tidak ditebak nominalnya. |
| QA-45 | Numerik L3: asumsi dikoreksi. | E05 clamp1,7; hitungan §5.2. | E analytic+P controller: target waktu mengikuti kecepatan efektif, akselerasi dan posisi E. |
| QA-46 | Satu menit inspeksi pra-cutter: layak. | Encounter baru. | P: 60 detik gameplay, ghost tetap x9 dan pursuit belum berjalan. |
| QA-47 | E rantai dua detik: layak. | E07 perlu resolver. | P: uji frame sebelum/sesudah freeze; resolved pada frame chain-cut, tanpa celah damage. |
| QA-48 | Return L3 sebelum boss: layak. | Resolved flag baru. | W: chain selesai cukup untuk aman meski bossWin false. |
| QA-49 | Masuk belakang404: perlu topology. | Geometri P1 belum ada. | P+W: semua sisi/crouch/vault ditutup sebelum ritual, hanya panel tertentu runtuh. |
| QA-50 | Loot/video/pulang/motor: layak. | E14 belum exit P1. | W: urutan K404→L3→L2/L1→motor, epilog tidak fade. |
| QA-51 | Replay kaset: perlu receipt selesai. | E14 pola ending. | P+F: tidak mengulang epilog/donasi, kontrol dan akses L2 tetap tersedia. |
| QA-52 | Tiga dinamo: oracle diselaraskan. | E03 satu energi, dinamo baru. | P+W: +25/8s sekali, emergency<10/45s/cap10/cool120; recovery arena global tetap ada. |
| QA-53 | Quit setelah1–2 hit: P0. | E02 file atomik, write barrier baru. | P+F: hard quit berulang; CP5 keris/Jurnal/dinding/tiga lampu utuh, attempt tidak masuk disk. |
| QA-54 | Write fail CP6: P0. | E18 hanya fondasi fault file. | F: sebelum temp/replace/efek, CP5 atau CP6 valid; tidak ada half-segel/keris. |
| QA-55 | Grant/Tuyul/mailbox: D06. | E02 unique list, E03 item type. | E+P: first-arrival grant sekali jika disetujui; dua instance, theft satu, retry tanpa duplikasi. |
| QA-56 | Sanity rendah vs100: oracle diselaraskan. | E01 mulai Babak4. | E+P: default P1 tidak mengubah nilai/radius; alternatif napas hanya setelah D02. |
| QA-57 | Pacing60 menit: belum dinilai. | E19 fixed-step bukan pacing. | H: Windows, tester tanpa hafalan, timestamp tiap beat dan rentang waktu; tanpa forced wait. |
| QA-58 | Mati sesudah cutter: perlu transaksi. | E02 pola commit. | P+F: sebelum chain→CP3 tool kembali; sesudah chain→tool dan chain tersimpan bersama. |
| QA-59 | Arrival battery0/1/19/85: D06. | E03 clamp, top-up baru. | E+P: jika disetujui menjadi20/20/20/85 sekali, tanpa powerbank tambahan. |
| QA-60 | Sepuluh retry/re-entry lift: D06. | E02 snapshot. | P+F: baterai tetap nilai CP3, bukan+20 setiap kematian. |
| QA-61 | Battery0 selama pursuit: D01 blocker. | E03/E09 menutup feed0. | W: jika approved, WATCH_ONLY+frustum/E tetap bekerja, AR/EVP/Flash mati, energi tetap0. |
| QA-62 | Return L3 pada0%: layak. | Resolved flag baru. | P+W: hantu tidak respawn, WATCH_ONLY mati, geometri dapat dilalui. |
| QA-63 | E di belakang pandangan: risiko tinggi. | E07 wajib look-at sekarang. | P+W: radius tangan1,3/1,6m, obstruction solid ditolak, satu aksi tanpa balik kamera. |
| QA-64 | E pada jarak tangkap: layak. | Prioritas catch/input baru. | P: ≤1,2 fatal lebih dahulu, >1,2 dan E valid memberi freeze2s lalu resolve. |
| QA-65 | CP5 dari altar: tambah Jurnal. | E02 tidak memvalidasi anchor. | P+F: E dapur tidak membuat CP5; E altar valid menyimpan sebelum keris dikonsumsi. |
| QA-66 | Door404 saat2bukti/CP6: layak. | E07 door, derivasi P1 baru. | P: keris membuka pintu, altar menolak; sesudah CP6 pintu tetap terbuka tanpa keris di tas. |
| QA-67 | Isi L4: belum dinilai. | Rute P1 belum ada. | H+W: aktivitas K403/EVP/dapur nyata;21 menit target,13–16 menit bukan bug. |
| QA-68 | Lift L1 vsL2: layak. | E15 pola travel, lift baru. | W: pilihan safe landing, L1 servis pompa bukan lobi las; kedua rute sampai parkir. |
| QA-69 | Cancel panel motor: prioritas data. | E11 pola UI. | E+P: bandingkan save/event sebelum-sesudah, RETURN_TO_EXIT tetap, bisa kembali L2. |
| QA-70 | Spam/fail CP7: P0. | E02, E14 urutan fade berbeda. | F+W: complete sekali; failed save tidak fade/load, retry commit tersedia. |
| QA-71 | K102 return0%: perlu latch. | E09 belum passage persisten. | P+W: dua arah saat0/stowed/load, collider tetap terbuka sesudah CROSS102. |
| QA-72 | Dinamo terputus30/45s: layak. | Dinamo baru. | P+W: mulai pada3%, berhenti saat chase, lanjut30s tersimpan RAM setelah aman. |
| QA-73 | Emergency9% berulang: layak. | E02, persistent gameplay clock baru. | E+P: cap10, cooldown120 melalui load/pause; full charge sekali/lokasi. |
| QA-74 | Papan6 vs arsip Tari: layak. | Konten visual baru. | W: normal/AR pada720p, cap dan silang berbeda teks/simbol; secret tidak bocor otomatis. |

### 7.1 Gap cakupan di luar 74 skenario

Tambahan berikut belum dijalankan dan tidak mengubah penomoran74:

- **AUD-A:** PrologComplete→P1, Continue setiap CP1–CP7, checkpoint asing, New Game, backup recovery. Sentinel P0 tidak hilang; scene tujuan yang belum ada tidak memicu reset.
- **AUD-B:** enam permutasi bukti; konfirmasi Jurnal sebelum/sesudah keris; ID tepat untuk secret lokal/global. Lima ID sembarang tidak membuka Secret Ending.
- **AUD-C:** satu frame dengan target bertumpuk, UI click/LMB, E keluar hiding/rantai, Q+LMB, TAB+F, RMB release+shortcut, V/X/remap, window focus. Tutup semua jalur Stow/app yang bisa melanggar watch.
- **AUD-D:** crash/fail pada pickup secret, CROSS102, mailbox, CP3 top-up, chain commit, Jurnal confirm, CP5 sebelum consume, CP6 sebelum efek, CP7 sebelum fade. Konsistensi retry dalam sesi sama harus diuji, bukan reload saja.
- **AUD-E:** ArchiveDocument, bayar Bank, callback autosave, pause/quit/settings selama boss tidak menulis attempt. Lampu1–2 dan altar runtime tidak bocor ke disk.
- **AUD-F:** battery tepat0, epsilon, di bawah/tepat biaya1/15; AR↔normal+EVP overlay; cooldown6s vs exposure5s; Tuyul aktif sebelum exposure; proyektil yang masih terbang saat kemenangan.
- **AUD-G:** topology404 sebelum/selama/reset/sesudah kemenangan, pintu berubah ketika AI punya cache jalur, volume monster2,5 m, waypoint lantai berbeda tetapi XZ sama, OTS peek bukan watch dari lensa HP.
- **AUD-H:** disk penuh/read-only/backup gagal, v1 valid/korup/schema masa depan; jangan menebak nominal duplikat. Gunakan filesystem uji terisolasi, jangan save pemain.
- **AUD-I:** clock dinamo ketika pause/reload, restart pada30/45s (progres RAM boleh hilang jika kebijakannya jelas), carry-over battery0/powerbank0/upgrades/tagihan belum dibayar.
- **AUD-J:** repeat load/memory growth, alokasi RT saat toggle RMB, lift, frame hitch atomic flush,720p/resize/readability, ReducedGlitch dan subtitle nonaktif tetap menyediakan petunjuk yang diperlukan.

## 8. Rekomendasi implementasi dengan prioritas, dampak, dan kompleksitas

**P0** adalah penghalang integrasi/data/softlock sebelum milestone terkait, bukan klaim seluruhnya bug Prolog sekarang. **P1** untuk kualitas/kelengkapan sebelum rilis Babak1. **P2** setelah baseline playable teruji. Kesulitan: K=kecil, S=sedang, B=besar.

| Rekomendasi | Masalah dan bukti | Solusi yang diusulkan | Dampak kualitas | Sulit / dependensi |
|---|---|---|---|---|
| R01 P0: kontrak keputusan | Angka/kanon bersilang, K01–K12. | Bekukan default global dan pengecualian D01–D09; root Phase1 sumber aktif. | Desain konsisten antarkomponen. | K–S; desainer. |
| R02 P0: routing dan migrasi | CP P1 belum dikenali, schema 2 ditolak; E02/E15. | Perluas routing/loader CampaignState yang sama; preserve P0, namespace CP, append scene kelak. | Campaign dan Continue aman. | B; R01, fixture save v1. |
| R03 P0: transaksi tunggal | File atomik belum rollback seluruh Data; E02/E11. | Reuse pola Commit lokal, full rollback, pagar semua write saat boss, CP6/7 atomik. | Tidak ada half-segel/keris atau softlock pemulihan. | B; R02, fault injection. |
| R04 P0: consumable/ledger | Dua powerbank ditolak jika ID sama; ledger belum ada; E02/E03/E14. | Instance ID/descriptor di inventory existing, receipt sumber, ledger P1 pada save yang sama. | Stok, theft dan uang benar; milestone bukan saldo palsu. | S–B; D03/D06, R02/R03. |
| R05 P0: HP/lensa/input | Feed0 ditutup, RMB mengembalikan state, flash hanya visual; E03/E09. | Provider konten P1 kecil, satu dispatch aksi, biaya/cooldown HP existing; proximity khusus. | Phone immersive tanpa merusak fitur P0. | B; D01, R01/R02. |
| R06 P0: blockout dua arah | Rusunawa belum ada, clearance/topology belum dibuktikan. | Uji semua rute naik/pulang, K403/404, kapsul dan volume boss sebelum art final. | Menghindari geometri bagus tetapi tidak dapat dimainkan. | B; R02/R05, D09. |
| R07 P0: recovery resource | AR/EVP wajib, dinamo/recovery arena belum ada. | Satu layanan energi existing, clock/cooldown persisten, safe window recovery. | Jalur tetap solvable saat baterai0. | S–B; D01/D06, R04/R06. |
| R08 P0: encounter L3 | E masih reticle, angka kecepatan berbeda; E05/E07. | FSM lokal, proximity/LOS, freeze2s, chain+cutter atomik, uji actual controller. | Tegang tetapi adil, tanpa kematian wajib. | B; R03/R05/R06/R07. |
| R09 P0: bukti/Jurnal/EVP | Foto P0 berurutan, recorder/confirm belum ada; E10/E12. | Tiga ID bebas urutan, sesi EVP pada HP, konfirmasi Jurnal, ambush receipt dan safe CP4. | Investigasi lengkap dan gate ritual benar. | B; R03/R05/R06. |
| R10 P0: boss dan CP5/6 | Shield/halogen/puing baru, kebijakan resource perlu ratifikasi. | Tiga lampu unik, miss/retry, lure, exposure aman, satu attempt RAM, snapshot/rollback. | Boss bisa dicoba ulang tanpa state ambigu. | B; R03/R07/R09, D02/D08. |
| R11 P1: epilog/exit | Tape/video/exit P1 belum ada; E14. | Receipt selesai, return control, lift L1/L2, panel transient, CP7 baru fade sesudah sukses. | Kontinuitas cerita dan kesempatan secret terjaga. | S–B; R03/R06/R10, D05/D09. |
| R12 P1: art/audio terarah | Pipeline tersedia, kualitas P1 belum dinilai; E17/E20. | Komposisi rusun/landmark/fixture, VO dengan arahan emosi, cue murah, optimasi model legal milik proyek. | Atmosfer dan telegraph terbaca, melampaui placeholder. | B untuk konten; blockout stabil, talent/lisensi. |
| R13 P1: QA/profiling nyata | Tes historis bukan QA/FPS P1; E19. | 74QA+AUD, regresi P0, fault injection, input Windows dan tiga run hardware. | Bukti acceptance dapat diperiksa. | B; seluruh milestone, D07. |
| R14 P1/P2: aksesibilitas dan UX | Remap UI subset, history khusus belum ada; E10/E15. | P1: seluruh kontrol yang dipakai dapat di-remap, cue terbaca dan history clue. P2: hint opsional berdasarkan data stuck; jangan mengubah window kanon. | Kontrol dapat diakses, frustrasi berkurang tanpa mengganti puzzle. | S; R09/R13; remap dasar mengikuti kewajiban global. |
| R15 P2: streaming/framework bila perlu | Void/four-floor berpotensi mahal, package Nav bukan fitur. | Optimasi area/culling/baked light dahulu; tambah streaming hanya bila profil memerlukan. | Menjaga biaya arsitektur sebanding masalah nyata. | B jika diperlukan; R13. |

## 9. Roadmap berbasis dependensi dan gerbang kelulusan

Rencana ini baru dikerjakan sesudah persetujuan implementasi. Setiap milestone menghasilkan slice yang dapat direview; tidak memulai Babak2.

| Milestone | Dependensi | Deliverable dan gerbang | Estimasi engineering aktif* |
|---|---|---|---|
| M0: kontrak | Desainer dan audit. | Keputusan terbuka tercatat, oracle QA jelas, Phase1 menjadi acuan aktif. |2–4 hari |
| M1: data/routing | M0. | Migrasi, inventory/receipt, transaksi pada campaign existing; Continue P0/P1, fault v1 aman. |4–7 hari |
| M2: blockout | Kontrak M1 + gerak existing. | Seluruh rute naik/pulang, K403/404/beam, clearance; tanpa art final dahulu. |3–5 hari |
| M3: L1–L2 slice | M1/M2, kontrak HP. | K102,644,Tuyul,dinamo,secret; nol baterai/duplicate/write failure/reload. |4–6 hari |
| M4: L3 | M3, keputusan input/watch. | Gerak S nyata, cutter/chain, CP3/after-chain, kebijakan0%, batas frame catch. |4–6 hari |
| M5: investigasi L4 | M2/M3/M4. | Stealth, enam urutan bukti, EVP ambush, Jurnal confirm, keris dan safe CP4. |6–10 hari |
| M6: boss | Write barrier M1 + M5. | CP5/6, tiga halogen, recovery,20retry, telegraph puing dan write failure kemenangan. |6–10 hari |
| M7: epilog/exit | M6. | Kain/kaset/video, return L1/L2, motor panel, CP7; endpoint aman tanpa sceneP2. |2–4 hari |
| M8: kualitas/acceptance | M3–M7 stabil. | Art/audio final,74QA+AUD, regresi P0, Windows GPU/pacing; build dan R2 ketika milestone delivery diizinkan. |5–10 hari |

*Total indikatif **36–62 hari kerja engineering**, bukan janji kalender atau hasil pengukuran. Pengadaan/retake VO, produksi art/animasi final, tunggu hardware/desainer belum dihitung. Konten dapat dikerjakan mengikuti ketergantungan oleh tim bila tersedia; gerbang data/navigasi/QA tetap wajib. Sesi audit ini tidak mendelegasikan subagent.

Regresi wajib: puzzle kos1408 dan kompatibilitas148, jam Atmaja03:45, AR/tiga foto bukti P0, RMB hold/release, crouch/collision/hiding, save/backup, motor→Atmaja, chase/window photos/finale/Continue. Ini perlindungan sistem yang sudah ada, bukan ekspansi cerita.

## 10. Performa dan presentasi: rencana pengujian

P1 belum dapat diprofilkan. Cap350k triangle Atmaja dan map2048/1024 adalah pemeriksaan statis scene lain, bukan jaminan P1 mencapai30FPS. Enam monster VRM sumber belum aktif/dioptimalkan; jangan mengimpor seluruh file high-poly sekaligus. Optimasi offline/LOD sesuai actor yang dipakai, periksa provenance/lisensi, dan jangan mengambil aset DreadOut.

1. Ukur blockout lalu tiap slice: main camera, feed HP on/off/focus, area terlihat melalui void, shadow, batches, skinning dan physics AI. Tetap satu perangkat/feed AR existing.
2. Gunakan pencahayaan baked/static pada geometri yang cocok dan batasi halogen realtime aktif/berbayang. Tetapkan cap setelah ukur; tiga fixture fisik tidak berarti tiga shadow mahal selalu aktif.
3. Feed existing256×448 portrait, fokus sampai960wide dan mengganti RenderTexture ketika ukuran berubah (E09). Ukur churn/hitch saat RMB ditoggle; cache/reuse hanya bila masalah terukur.
4. Prioritaskan telegraph audio/siluet, kabut transparan secukupnya, puing berpola pool dan collision satu hit per object. Lampu pecah tidak perlu ratusan rigidbody.
5. Graf/cast Nyonya mengalokasikan array (E13). Brain P1 memerlukan cadence sensor dan invalidasi cache ketika pintu/404 berubah; jangan menghitung graf semua lantai setiap frame atau memigrasi seluruh AI karena package Nav tersedia.
6. Profil Windows i5-1155G7/IrisXe/8GB,1280×720Low, tiga run lintasan plus boss. Simpan average/1%low/p95/p99, working set/peak memory, pass GPU/shadow/RT, waktu load dan raw capture. Gunakan metode1%low yang sama; integrated GPU bukan VRAM dedicated tetap.
7. Tier minimum usulan30average/24one-percent-low (D07) terpisah dari aspirasi60 global. Worst case chat/hujan/Tuyul/halogen harus diuji. Fixed1/30 simulation atau freeze screenshot bukan pengukuran FPS.

Kualitas audio memerlukan review performa emosi dan intonasi, bukan jumlah WAV atau keberhasilan pipeline. Voice lookup E17 memakai exact speaker+text: edit dialog harus memperbarui mapping agar subtitle tidak kehilangan VO. EVP wajib mempunyai subtitle/cue non-audio. Tangisan Kuntilanak yang makin pelan saat mendekat perlu arah, flicker atau UI yang konsisten agar pemain tidak bergantung pada volume saja.

## 11. Keputusan desainer

### 11.1 Sudah diputuskan; tidak perlu meminta ulang

Lima gerbang/empat segel, ID Negatif Tari, secret opsional sampai exit sadar, chapter select backlog, offline chat, tanpa paywall, default baterai global, Sanity mulai Babak4, serta OTS/FP yang sudah diminta pengguna. E/J sudah tersedia. Pertahankan sampai ada instruksi eksplisit pengganti.

### 11.2 Masih terbuka atau memerlukan ratifikasi kandidat

| ID | Keputusan | Usul kerja, belum persetujuan | Gerbang |
|---|---|---|---|
| D01: energi khusus | WATCH_ONLY0%L3, klasifikasi Blitz Tuyul15%, global charger vs halogen0%, durasi/abort EVP? | Boss default mengikuti global15%/6s + recovery arena. WATCH_ONLY dan halogen0% adalah pengecualian, bukan fitur existing. Jangan mencampur dua ekonomi default. | M3/M4/M6. |
| D02: damage/Sanity | Ratifikasi dua-hit puing8s; tetap tunda Sanity atau sengaja ubah global? | Sanity P1 pasif; dua-hit merupakan proposal attempt runtime, bukan sistem health kedua. | M5/M6. |
| D03: uang/transfer | Sumber ledger10jt/3jt/optional1jt/donasi lain, keterikatan kontrak500jt, kuitansi jika belum bayar P0? |78,2jt tetap milestone global terpisah. Jangan memalsukan paid receipt/donor atau diam-diam mendebit wallet. | M1/M3, intro final. |
| D04: kontrol terbaru | V perspektif/X Selfie/E/J sudah tetap. Acceptance app/RMB/watch/abort perlu diperinci. | Catat utang sinkronisasi Rancangan lama; jangan meminta ulang OTS atau menghapusnya. | M0/M4. |
| D05: kaset1998 | Apakah kata “kelak” adalah nubuat kanon untuk Rusunawa2004/tragedi lain? | Teks candidate editorial dipertahankan; jangan mengubah tahun/dialog diam-diam. | M7/VO final. |
| D06: carry-over | Baterai100 vs warisan, grant satu powerbank, top-up20 CP3, floor40 CP5? | Warisi campaign, grant sekali hanya jika disetujui. CP5 default persis snapshot. Tidak memanggil StartNewGame untuk inisialisasi P1. | M1/M3/M4/M6. |
| D07: performa | Ratifikasi720pLow30/24 minimum vs aspirasi60; target memory/load/quality? | Profil GPU nyata dahulu, kemudian tetapkan budget. | M2 benchmark/M8 freeze. |
| D08: retry boss | Ratifikasi reset tiga lampu CP5 dan recovery failed CP6. | Kandidat v2.2: seluruh lampu reset, tidak ada partial progress; failed commit masuk UI aman atau restore CP5. | M6. |
| D09: alur/endpoint | Intro boleh melewati pos/live? Dokumen clue wajib? Endpoint CP7 bila RSUD belum ada? | Jangan menambah wall/gate. Dokumen clue pendukung, tape epilog wajib; chapter-complete/tujuan tersimpan perlu endpoint reviewable. Jam naratif bukan timer gagal. | M0/M2/M7. |

Denah/switch final, dimensi cover, scan/record duration, tuning AI, cooldown dan lokasi recovery perlu review blockout. Developer dapat menyiapkan pilihan teknis ini; itu tidak memberi izin menambah kanon atau scene dalam audit.

## 12. Perubahan dokumentasi yang diterapkan

Atas instruksi susulan pengguna, **Phase1.MD menjadi2.2-r1**. Perubahan hanya pada spesifikasi:

- Kontrak global/existing §0.4, peta kode/API aktual dan batas integrasi §14, serta urutan dependensi implementasi.
- Tabel drain/biaya/powerbank/EVP mengikuti default global. Alternatif halogen/WATCH_ONLY dipisahkan; Sanity P1 dan floor 40% ditahan.
- Gate konfirmasi Jurnal ditambahkan ke ritual/receipt/CP5/pseudocode. Bukti, keris, lokasi, secret dan urutan cerita tidak diganti.
- Contoh schema 2 diperbaiki menjadi satu campaign dengan list receipt serializable, migrator yang belum ada dijelaskan, inventory dua unit dan ketiadaan bus generik dicatat.
- Kecepatan efektif1,7 dan keterbatasan model1,8, write barrier seluruh callback boss, secret UI sesudah commit dan risiko Continue P1 dicatat.
- Oracle QA yang berbenturan diselaraskan tanpa klaim lulus; **QA01–74 tetap lengkap**, tidak ada kasus sulit yang dihapus.
- Keputusan yang sudah jelas global ditutup, desain OTS/FP terbaru dipertahankan, pilihan kanon yang belum jelas tetap terbuka.

Rancangan.MD dan salinan historis dalam Assets tidak diubah. Kode, scene, prefab, aset, paket, konfigurasi dan struktur konten game tidak diubah. Tidak membuat save/event/AR/inventory paralel. Implementasi runtime belum dimulai.

## 13. Verifikasi audit dan batas kelulusan

Pemeriksaan dokumentasi: set QA unik01–74 pada input v2.2, r1 dan tabel laporan; tautan source/baris; hash Rancangan/salinan lama; diff hanya dokumen serta keluaran Graphify wajib. Graphify diperbarui lokal sebelum commit sesuai AGENTS; graph bukan bukti tes game.

**Unity/Windows/74QA tidak dijalankan ulang**, karena ini audit dan belum ada implementasi P1. Tidak ada ZIP baru atau upload ulang build lama. E19 tetap bukti historis. Freeze gameplay baru dapat dinilai sesudah implementasi diizinkan, milestone selesai, acceptance diuji dan Windows pada hardware sasaran diperiksa langsung.
