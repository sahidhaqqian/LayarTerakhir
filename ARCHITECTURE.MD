# Arsitektur LayarTerakhir

Dokumen ini menjelaskan fondasi `Prolog_KamarKos` yang sudah approved serta kelanjutan `Prolog_RumahAtmaja` (Adegan 2–3, sampai pintu tangga), Unity 6000.3.26f1 + URP 17.3.0, Windows x86_64 Mono. Dokumen cerita/gameplay tetap menjadi acuan: `Rancangan.MD`, `Prolog.MD`, `Phase1.MD`–`Phase5.MD`, dan `Checklist.MD`.

Revisi playtest 16 Oktober 2026: scene/aset kamar Golden tetap byte-identical, tetapi runtime pembuka/keluar diperluas sesuai arahan pengguna. Lihat `Audit/playtest-fixes-2026-10-16.md` untuk batasan dan bukti terbaru. Bagian fondasi di bawah menjelaskan sistem asal; tambahan revisi berikut menggantikan alur keluar/HP lama.

`GameShell` adalah satu UI persisten sebelum scene pertama: menu awal/jeda/pengaturan, pemuatan New Game/Continue, volume/sensitivitas/subtitle/reduksi glitch/remap. New Game mengarsipkan slot lama byte-for-byte sebelum membuat campaign baru. `UiFocus` memisahkan menu/jam (hard modal), browsing HP dan cutscene; TAB/F tetap dapat dipakai ketika browsing HP. `KamarKosPrologDirector` menunggu pilihan menu, memainkan bangun dari kasur, ketukan/langkah, surat dan panggilan Damar otomatis. Ratna tidak bersuara. Persiapan memakai gembok empat roda, kunci terpisah di meja, jaket terlihat dan powerbank di laci; pintu memuat koridor, bukan langsung Atmaja. `KosRoomCorrections` adalah pass runtime terarah yang tidak menyimpan ulang scene Golden. Modal benda dunia menurunkan HP agar fokus aplikasi tidak menyisakan softlock gerak/interaksi.

`PrologSceneFlow`: Kos (index 0) → KoridorKos (index 2) → Atmaja (index 1). Index dipertahankan untuk kompatibilitas build lama; urutan cerita memakai nama scene. `KosCorridorDirector` menangani checkpoint lantai 2/halaman dan perjalanan: peta dapat dilihat dari mana saja, tetapi departure hanya di dekat gerbang/motor. Motor, kamera, tangan first-person dan gerbang bergerak nyata sebelum fade/load Atmaja. Binding input dibawa lewat transisi; singleton campaign scene baru membaca save atomik yang sama.

`AtmajaPhoneView` dipasang pada HUD semua scene, dengan `PhoneDesktop` (Kamera/Peta/Pesan/Jurnal/Tas/Bank/Setelan/Telepon), `PhoneSkin` dan ikon vektor `PhoneGlyph`. RT portrait 256×448 hanya aktif saat aplikasi Kamera dan layar HP hidup; layer 29 hanya kamera HP, bukan kamera dunia. Status HUD eksternal disembunyikan ketika handset diangkat agar tidak bertumpuk dengan status bar. Pesan donasi tampil di HP; feedback dunia/save tetap berupa toast ringkas yang terlihat meski HP mati. Jurnal/inventori memakai scroll; saldo hanya di Bank. Peta memakai PNG jalan Jakarta CC0 dengan UV crop/pin/rute cerita fiktif. `PresentationProfile` di Resources mengikat font, shader-feed, mixer dan 15 WAV VO neural; `HudController` menyatukan antrean subtitle/audio serta priority call agar panggilan tidak tertunda komentar opsional. `ClockAmbience` menyediakan tik-tok spasial, `HingedDoor` pintu eksplorasi, `ClockPuzzleView` kontrol jam/menit. Tulisan AR adalah mesh guratan merah pada plane dinding; shader-feed mempertahankan warna merahnya. `AtmajaDirector` mensyaratkan petunjuk benar-benar dibidik melalui AR sebelum puzzle jam terbuka; event tetap idempotent dan rollback bila save gagal.

`PlaytestRevisionBuilder` meregenerasi scene Atmaja, bank presentasi dan koridor tanpa menyimpan ulang scene kamar. `PlaytestReviewValidation` memeriksa serialisasi komponen, geometri, kapsul jalur meja, dua lantai dan VO. `ReviewPlayerAudit` adalah opt-in development-only dengan save terisolasi, menu aktual, perjalanan motor, jalan CharacterController kontinu dan capture aktual; timer dipercepat/capture freeze dicatat, bukan bukti performa/pacing manual.

Standar produksi visual/audio/UI ringkas ada di `Docs/GOLDEN_SCENE_STANDARD.md`. Dokumen ini berfokus pada struktur kode, kepemilikan tanggung jawab, aliran data, dan pipeline produksi.

Revisi immersion Kos berikutnya: `UiFocus.BlocksMovement` hanya mengunci menu/jam/cutscene, bukan aplikasi HP. Browsing tetap memiliki pointer dan memblokir klik objek dunia; WASD berjalan, tahan RMB untuk mouse-look. `CanStandUp` memeriksa overlap seluruh kapsul berdiri (mengabaikan hull sendiri/trigger), bukan SphereCast dari titik yang mungkin sudah menembus meja. `KosCorridorBuilding` melengkapi builder dengan 14 sel kamar tertutup, fondasi, guardrail dalam tangga, fasad/atap/teras, pintu utama `HingedDoor`, wainscot/ventilasi/kelistrikan dan detail penghuni. Pintu utama diserialisasi di director; checkpoint lantai bawah mensyaratkan kaki benar-benar grounded dan recovery bawah bangunan hanya sebagai pengaman terakhir.

Pembuka kini menyembunyikan surat hingga tiga ketukan keras, mengarahkan pandangan Raka, memainkan ringtone, distress cry Damar, jeda pesan dan respons kontrak. Lima SFX original ada di `Resources/KosAudio`; bank neural berisi 17 baris. Retake memakai reference-only controllable cloning: mode transkrip lama ternyata mengabaikan kontrol emosi. Laci menambah `DOMPET_RAKA`/`DIARY_RAKA` tanpa memberi uang; isi buku harian dibaca dari Jurnal. `PlaytestRevisionBuilder.RefineKos` mengimpor presentasi dan membangun koridor saja, tidak menulis ulang scene Golden maupun Atmaja.

---

## 1. Ringkasan teknis

| Aspek | Nilai |
| --- | --- |
| Engine | Unity `6000.3.26f1` (`ProjectSettings/ProjectVersion.txt`) |
| Render pipeline | Universal RP `17.3.0` |
| Input | Input System `1.20.0` (dengan fallback legacy di kode) |
| Testing | Unity Test Framework `1.6.0` (EditMode + PlayMode) |
| Target rilis | `StandaloneWindows64`, backend `Mono2x`, opsi `Development` |
| Scene startup | `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity` (index 0) |
| Scene kelanjutan | `Prolog_RumahAtmaja.unity` (index 1), `Prolog_KoridorKos.unity` (index 2); SampleScene tidak dipakai |
| Host pengembangan | Ubuntu 24.04 headless, `-batchmode -nographics`, tanpa GPU diskrit |
| Hardware target pemain | Intel i5-1155G7, Intel Iris Xe, 8 GB RAM, Windows 11 |
| Target performa | 30 FPS stabil (target, bukan hasil terukur di host ini) |

Namespace runtime memakai prefiks `LayarTerakhir.*` (`Core`, `Player`, `Prolog`, `Smartphone`, `UI`, `Audio`, `Diagnostics`) dan editor-only di `LayarTerakhir.Editor`.

---

## 2. Peta repositori

```
Assets/
  Scenes/                         SampleScene (template, tidak dipakai)
  Readme.asset                    aset Readme bawaan template Unity
  _LayarTerakhir/                 seluruh konten proyek
    Scenes/Prolog_KamarKos.unity  scene Golden (startup build)
    Scripts/                      kode runtime (lihat §4)
    Editor/                       alat produksi & validasi (lihat §7)
    Art/
      Imported/<provider>/<id>/   model & tekstur pihak ketiga (PolyHaven, Kenney)
      Original/Models|Textures    FBX/tekstur buatan proyek
      Generated/Materials|Meshes  material/mesh hasil authoring editor
    Audio/
      Imported/                   sumber audio mentah
      Prepared/                   15 WAV mono 32 kHz siap pakai
      GoldenScene.mixer           bus Ambience / Effects / UI
      KosAudioProfile.asset       SceneAudioProfile scene kos
    UI/Fonts|Prefabs              Barlow (OFL) + GoldenHUD.prefab
    Production/Prefabs|Presets    prefab & preset impor yang dapat dipakai ulang
Docs/GOLDEN_SCENE_STANDARD.md     standar produksi scene emas
Tools/*.py                        orkestrasi download, authoring, validasi, build, packaging
Audit/*.md, *.json, visual        bukti validasi, kredit aset, log build
Builds/                           hasil build (di-gitignore), ZIP Windows, LinuxAudit
```

Dokumen akar: `goal.md` (brief produksi), `Rancangan.MD` (aturan gameplay), `Prolog.MD` (naskah prolog), `Phase1.MD`–`Phase5.MD` (naskah babak), `Checklist.MD` (checklist), `Review-Argon.md` (catatan review), `Agents.MD` (kesepakatan kerja agen), `Docs/GOLDEN_SCENE_STANDARD.md`, `ARCHITECTURE.MD`, `README.MD`.

---

## 3. Arsitektur runtime

Satu scene runtime berisi satu root sistem, satu rig pemain, dan satu root lingkungan. Hierarki aktual `Prolog_KamarKos`:

```
Prolog_KamarKos
├── GameSystems
│   ├── GameInputConfig            binding tombol yang bisa di-remap
│   ├── CampaignState              singleton state + save/load atomik
│   ├── KamarKosPrologDirector     state machine cerita & puzzle kos
│   ├── GoldenHUD (prefab)         HudController + HudPresentation + Canvas
│   └── GoldenAudioRig             KosSceneAudio + 9 AudioSource + 3 loop
├── KamarKos_4x3m                  root fisik (nama lama dipertahankan)
│   ├── Floor_4x3m, Ceiling_4x3m, Wall_North/South/East/West   collider 5×3,8×2,8 m
│   ├── KamarKos_4x3m furniture    collider & titik interaksi lama
│   └── Kos_Environment_Art
│       ├── Kos_V2_Details         pass koreksi V2 (clutter, socket, kabel)
│       ├── Golden_Workstation     grup meja kerja yang digeser bersama
│       └── Golden_Dressing        stool, matras, mikrofon, keycaps
├── Player_RakaWijaya              tag Player
│   ├── CharacterController        radius .32, tinggi 1.75
│   ├── FirstPersonController      gerak, postur, mouse look
│   ├── InteractionInteractor      raycast interaksi 2,6 m
│   ├── SmartphoneController       HP, mode, baterai
│   └── CameraPivot/MainCamera     FOV 70, AudioListener, HandSmartphoneRig + flashlight
└── GoldenPracticalLighting (prefab) 5 sumber cahaya praktis
```

Prinsip penting: **collider fisik, root interaksi, dan state cerita tidak dihapus oleh pass visual**. Nama lama seperti `Floor_4x3m` dan `KamarKos_4x3m` dipertahankan untuk kompatibilitas meskipun kos approved kini 5 × 3,8 m. Jangan menjalankan pass visual lama pada Golden yang sudah disetujui.

---

## 4. Komponen runtime

### 4.1 Core

**`LayarTerakhir.Core.GameInputConfig`** (`Scripts/Core/GameInputConfig.cs`)
- Singleton pada `GameSystems`; memegang 20 `KeyCode` untuk `GameAction` (gerak, sprint, crouch, vault, interaksi, HP, mode pemindaian, jurnal, powerbank, flare, kemenyan, tangkap/Blitz).
- `RemapAction`, `GetKeyForAction`, `GetActionDown/Held/Up`, `GetMovementInput`, `GetLookDelta`.
- Membaca Input System (`Keyboard`/`Mouse`) dengan pemetaan numerik/keypad/modifier, dan fallback ke legacy `Input` bila `ENABLE_LEGACY_INPUT_MANAGER` aktif. Label prompt HUD selalu mengikuti binding aktif.

**`LayarTerakhir.Core.CampaignState`** (`Scripts/Core/CampaignState.cs`)
- Singleton state campaign; `CampaignData` adalah kontrak data resmi (`chapter_id`, `checkpoint_id`, `evidence_ids`, `relic_ids`, `location_seals`, `upgrades`, `inventory`, `battery`, `sanity`, `wallet_legitimate`, `wallet_bound`, `spent`, `secret_items`, `secret_notes_read`, `archive_prepared`, `ending`, `schema_version`).
- `SaveCheckpointAtomic(id)`: validasi dulu, tulis `layar_terakhir_save.json.tmp` + flush, lalu `File.Replace` ke `layar_terakhir_save.json` sambil menyimpan `.bak`; kegagalan mengembalikan `checkpoint_id` lama.
- `LoadCheckpoint()` / `LoadBackupCheckpoint()` memvalidasi schema, rentang nilai (`battery`/`sanity` 0–100, wallet ≥ 0), enum ending, dan ID unik sebelum menimpa data hidup.
- Jalur simpan: `Application.persistentDataPath`. Di Editor/development build, environment `LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY` mengarahkan simpan ke direktori uji terisolasi sehingga save pemain tidak pernah tersentuh oleh validasi atau tes.

### 4.2 Player

**`FirstPersonController`** (`Scripts/Player/FirstPersonController.cs`)
- Berbasis `CharacterController`. Kecepatan: walk 2,8 / sprint 4,6 / crouch 1,6 / slide 5,5; tinggi berdiri 1,75, crouch 1,0; gravitasi −19,62.
- Slide dipicu crouch saat sprint dengan input maju; Space adalah impuls vertikal umum, tetapi `AtmajaVault` mengonsumsi lebih dahulu saat konteks jendela valid. Atmaja memakai crouch height .78 m untuk akses kolong meja.
- `MovementLocked` dan modal HUD membekukan input; saat terkunci hanya gravitasi yang berjalan. Browsing HP tetap mengizinkan gerak. `CanStandUp()` memeriksa overlap kapsul berdiri agar kepala tidak menembus meja/langit-langit rendah.
- Mouse look memakai `GameInputConfig.GetLookDelta()`; pitch dibatasi ±80°.

**`InteractionInteractor`** (`Scripts/Player/InteractionInteractor.cs`)
- Raycast dari kamera, jarak 2,6 m, buffer `RaycastNonAlloc` 16 dengan fallback `RaycastAll`.
- Menyaring collider milik pemain sendiri, memilih hit terdekat, mengambil `IInteractable` via `GetComponentInParent`, lalu menampilkan prompt `[KEY] label`.
- Tombol `Interact` memanggil `IInteractable.Interact()`.

**`IInteractable`** (`Scripts/Player/IInteractable.cs`)
- Kontrak: `InteractableId`, `GetInteractionPrompt()`, `CanInteract()`, `Interact()`.

### 4.3 Prolog

**`KamarKosPrologDirector`** (`Scripts/Prolog/KamarKosPrologDirector.cs`)
- State machine `KosPrologStage`: `WakeUpAndKnocks → WaitReadSuratIbuKos → WaitPickupSmartphone → PreparationAndDrawerPuzzle → ReadyToDepart → Completed`.
- `Start()` memuat checkpoint; jika `PROLOG_KOS_COMPLETE` valid dan inventori berisi `SMARTPHONE_RAKA` + `KUNCI_MOTOR` + `JAKET_RAKA`, scene dipulihkan langsung ke `ReadyToDepart` (objek yang sudah diambil dinonaktifkan, baterai HP dipulihkan).
- Merangkai bangun, tiga ketukan lalu langkah menjauh (bukan caption), surat tanpa VO ibu kos, panggilan Damar otomatis, pesan/dana Rp5.000.000, puzzle `1408`, kunci di meja, jaket dan powerbank, lalu checkpoint pintu keluar ke koridor.
- ID inventori: `DOC_PROLOG_01_SURAT_KOS`, `SMARTPHONE_RAKA`, `KUNCI_MOTOR`, `POWERBANK_KECIL_KOS`, `JAKET_RAKA`. Checkpoint: `PROLOG_KOS_COMPLETE`.
- `OnObjectInteracted(item)` adalah satu-satunya pintu masuk aksi interaksi; semua efek cerita (audio, subtitle, HUD, save) dikelola di sini agar tidak ada script lain yang menduplikasi logika.

**`KosInteractableItem`** (`Scripts/Prolog/KosInteractableItem.cs`)
- Implementasi `IInteractable` yang meneruskan `Interact()` ke director. Field: `itemId`, `promptLabel`, `isInteractable`, `director`.

ID interaksi scene kos: `PC_MONITOR`, `STICKY_NOTE`, `LACI_MEJA`, `KALENDER_DINDING`, `SURAT_KOS`, `SMARTPHONE_BED`, `PINTU_KOS`, `JAKET_PINTU`.

### 4.4 Smartphone

**`SmartphoneController`** (`Scripts/Smartphone/SmartphoneController.cs`)
- State perangkat: dimiliki/tidak, diangkat, layar hidup, live broadcast, kamera selfie, `ScanVisualMode.Normal|AR|UV`, serta unlock modul (`arModuleUnlocked`, `uvModuleUnlocked`, `selfieModuleUnlocked`).
- Parameter baterai mengikuti `Rancangan.MD`: normal 1%/20 dtk, AR/UV 1%/10 dtk, foto 1%, Blitz 15% dengan cooldown 6 dtk; powerbank besar +80, kecil +35.
- Baterai 0 otomatis mematikan layar dan mengembalikan mode ke Normal. `TryUsePowerbank()` menangani recharge walau HP tersimpan atau layar mati (perbaikan audit).
- `SyncVisualState()` mengatur rig tangan dan lampu flashlight. AR lantai dasar dirender lewat kamera `AtmajaPhoneView`; UV/Selfie dan mekanik babak lanjutan belum playable.

### 4.5 UI

**`HudController`** (`Scripts/UI/HudController.cs`) — logika & state
- Singleton; `Awake()` memastikan UI runtime dibangun (`EnsureRuntimeUIBuilt`) atau memakai UI tersimpan, lalu selalu mengikat ulang callback button (`Scene serialization does not retain AddListener delegates`).
- Elemen: objective, subtitle, interaction prompt, status HP, toast donasi, `BlackFadeOverlay`, crosshair.
- Modal: `DocumentModalPanel` (judul + body + tombol `CloseDocumentButton`, scroll) dan `PadlockModalPanel` (empat digit Kos; panjang dinamis tetap mendukung tes kompatibilitas tiga digit; button `DigitUp_i`/`DigitDown_i`, `UnlockPadlockBtn`, `CancelPadlockBtn`).
- API publik: `SetObjective`, `SetInteractionPrompt`, `EnqueueSubtitle` (antrean dengan durasi, format speaker/SFX), `ShowBroadcastNotification`, `OpenDocumentModal(title, body, onClosed)`, `OpenPadlockModal(code, onSolved)`, `SetFadeAlpha`, `FadeBlack`, `UpdatePhoneStatusUI`.
- `IsModalOpen` dipakai bersama oleh controller pemain, smartphone, dan audio untuk membekukan/menduck input dan suara.

**`HudPresentation`** (`Scripts/UI/HudPresentation.cs`) — presentasi murni
- Menghidupkan/memudarkan `CanvasGroup` (objective, interaction, phone, subtitle, toast), mengatur reticle, battery fill, 3 digit display, dokument scroll, modal backdrop, dan suara klik UI.
- Objective diredupkan setelah 12 detik tanpa perubahan; semua fade memakai `Time.unscaledDeltaTime` agar tetap bekerja saat modal pause.

**`HudStyle`** (`Scripts/UI/HudStyle.cs`) — aset ScriptableObject font Barlow + warna aksen (brass) + `fadeDuration`.

**`GoldenHudBuilder`** (editor) membangun `UI/Prefabs/GoldenHUD.prefab` dan `GoldenHudStyle.asset`, lalu memasang instance ke `GameSystems`. Prefab dipakai sebagai satu-satunya sumber HUD runtime scene.

### 4.6 Audio

- **`SceneAudioProfile`** (`Scripts/Audio/SceneAudioProfile.cs`): ScriptableObject berisi bus mixer (`Ambience`, `Effects`, `UI`), 12 clip pendek, 3 clip loop, array footstep, gain dan durasi fade.
- **`AmbientAudioLoop`**: loop dengan `gain`, `fadeSeconds`, dan `LevelMultiplier` (dipakai untuk ducking modal dan fade keberangkatan).
- **`KosSceneAudio`**: playback yang menempel pada event cerita yang sudah ada. Menangani footstep berbasis jarak (menolak teleport/load), suara jaket/kertas/unlock, ketukan pintu, getaran HP, notifikasi donasi, UI click, dan `Depart()` (latch + motor + fade loop). Tidak ada timer horor acak atau trigger cerita baru.
- **Mixer** `Audio/GoldenScene.mixer` dengan bus `Ambience / Effects / UI`; 9 `AudioSource` pada `GoldenAudioRig` (3 loop + 6 source efek), semuanya di-route, tanpa autoplay, Doppler 0. 15 clip WAV mono 32 kHz (Vorbis untuk ambience, ADPCM untuk SFX pendek).

### 4.7 Diagnostics & development

- **`GoldenPlayerAudit`** (`Scripts/Diagnostics/GoldenPlayerAudit.cs`, hanya Editor/Development): aktif hanya dengan flag `-goldenAudit <dir>`. Menjalankan smoke test terarah dengan save terisolasi (`IsolatedSave-<GUID>`), 13 screenshot `ScreenCapture`, tap audio listener nyata ke `actual-opening-mix.wav`, dan menulis `player-audit.json`. `-goldenAuditFast` mempercepat timer cerita untuk renderer software (dicatat di receipt).
- **`DevelopmentScreenshotCapture`**: F12 menyimpan screenshot frame final (termasuk HUD/post-processing) ke `persistentDataPath/Screenshots`; `-kosCapture` mengambil screenshot otomatis saat startup. Hanya aktif di Editor/Development build.

---

## 5. Kontrak data, save, dan pemulihan

| Item | Detail |
| --- | --- |
| Lokasi save | `Application.persistentDataPath/layar_terakhir_save.json` |
| Override uji | Env `LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY` (Editor/Development saja) |
| Penulisan | `.tmp` → flush → `File.Replace` → `.json`, snapshot lama menjadi `.json.bak` |
| Validasi muat | `schema_version == 1`, ID tidak kosong/unik, rentang battery/sanity, wallet ≥ 0, enum `EndingType` valid |
| Fallback | `LoadBackupCheckpoint()` eksplisit; pemain tetap memegang keputusan memakai snapshot lama |
| Enum ending | `None`, `Bad`, `Normal`, `True`, `Secret` — perubahan ending kelak memakai satu state tunggal |

Aturan desain data (dari `Rancangan.MD`/`Checklist.MD`): event cerita memakai ID unik sekali jalan (`EVIDENCE_CAPTURED`, `RELIC_COLLECTED`, `JOURNAL_CONFIRMED`, `RITUAL_STARTED`, `SHIELD_OPEN`, `SEAL_HIT`, `SECRET_COLLECTED`, `SECRET_NOTE_READ`, `ARCHIVE_PREPARED`, `ENDING_COMMITTED`), collectible atomik, dan reload tidak boleh menggandakan hadiah.

---

## 6. Alur scene Prolog (kondisi saat ini)

```
Start
 └─ LoadCheckpoint() ── cocok PROLOG_KOS_COMPLETE ──► ReadyToDepart (restore inventori/baterai)
 └─ gagal ─► StartNewGame ─► WakeUpAndKnocks (fade, hujan, kipas, ketukan)
      └─ WaitReadSuratIbuKos        objek: SURAT_KOS        → DOC_PROLOG_01_SURAT_KOS
           └─ WaitPickupSmartphone  objek: SMARTPHONE_BED  → SMARTPHONE_RAKA + donasi terikat 5 juta
                └─ PreparationAndDrawerPuzzle
                     ├─ LACI_MEJA  → gembok 1408 → POWERBANK_KECIL_KOS
                     ├─ KUNCI_MOTOR_MEJA → KUNCI_MOTOR
                     └─ JAKET_PINTU → JAKET_RAKA
                          └─ ReadyToDepart (objective pintu)
                               └─ PINTU_KOS → SaveCheckpointAtomic(PROLOG_KOS_COMPLETE)
                                    → Completed (MovementLocked, fade 3,5 dtk)
```

Objek pendukung opsional: `PC_MONITOR` (dashboard kanal), `STICKY_NOTE` (petunjuk password), `KALENDER_DINDING` (14–08). Fade keberangkatan sekarang memuat scene Atmaja; endpoint playtest terbaru adalah pintu tangga lantai dasar (lihat `Audit/atmaja-prolog-continuation.md`).

---

## 7. Pipeline produksi Editor-only

Semua skrip di `Assets/_LayarTerakhir/Editor/` dibungkus `#if UNITY_EDITOR` dan dipakai lewat CLI/`-executeMethod` atau menu `LayarTerakhir/*`.

| Skrip | Peran | Aturan pemakaian |
| --- | --- | --- |
| `PrologKosSceneBuilder` | Membangun greybox + player + 8 interaksi + HUD + director, menyimpan scene, menaruh scene di index 0 build settings | Hanya untuk membangun ulang dari nol; **jangan dijalankan di atas scene berfurnitur** |
| `KosVisualUpgrade` | Pass visual V1: arsitektur, furnitur, material PBR, cahaya, post-processing | Pass historis; scene saat ini sudah melewatinya |
| `KosVisualV2` | Koreksi inkremental V2 (clutter, socket, kabel, prop tambahan) | Hanya meregenerasi `Kos_V2_Details` miliknya sendiri |
| `GoldenSceneProduction` | Pass produksi inkremental final: workstation digeser bersama collider/interaksi, dressing emas, lighting, HUD, audio rig | Aman diulang; **tidak** memanggil greybox/V2 builder |
| `GoldenHudBuilder` | Membuat & memasang `GoldenHUD.prefab` + style | Dipanggil dari `GoldenSceneProduction` |
| `KosSceneValidation` | Validasi geometri & gameplay dasar (dimensi ruangan aktual, floor-contact, tabrakan footprint, arah sandaran kursi, tekstur ≤ 1024, sel reachable 25 cm, interaksi wajib) | Menulis `Audit/kos-v2-scene-validation.json` |
| `GoldenSceneValidation` | Validasi tambahan scene emas (audio rig, prefab, font, HUD tunggal, scroll dokumen) | Menulis `Audit/golden-scene-validation.json`; juga diekspos sebagai test `GoldenSceneTests` |
| `KosRenderDiagnosis`, `KosVisualCapture` | Diagnosis render & capture Play Mode untuk bukti visual | Editor Play Mode |
| `FoundationAuditTests` | Suite EditMode: callback HUD, gembok 148, offset rig HP, urutan scene, spawn bebas furnitur, missing reference, save/backup/korupsi, remap keyboard | `Audit/*.log|xml` |
| `FoundationPlayAuditTests` | Suite PlayMode: dua urutan persiapan, raycast E, surat, HP, donasi, puzzle, keberangkatan, restore checkpoint, recharge, drain mode, collision/crouch | `Audit/*.log|xml` |
| `WindowsDevelopmentBuild` | Build Windows x64 Mono Development, verifikasi report & EXE | Dipanggil runner `--stage windows` |
| `GoldenLinuxBuild` | Build player Linux untuk smoke test Xvfb | Bukti perilaku player nyata, **bukan** bukti Windows/Iris Xe |

`KosVisualV2.FloorY`, `BoundsOf`, `Resize`, `SetCenter`, `FitBox`, `SurfaceHeight` dipakai lintas pass dan validasi; jangan diubah tanpa menyesuaikan validator.

---

## 8. Konvensi aset & impor

- **Folder**: `Art/Imported/<provider>/<source-id>` untuk aset legal pihak ketiga; `Art/Original/Models|Textures` untuk buatan proyek; `Art/Generated/Materials|Meshes` untuk hasil authoring; `Production/Prefabs|Presets` untuk aset produksi yang dapat dipakai ulang. Kredit sumber wajib dicatat di `Audit/asset-credits.md` (URL, penulis, lisensi, perubahan) dan manifest di `Audit/asset-download-manifest.json`.
- **Material**: URP/Lit. Albedo sRGB; normal OpenGL (+Y) diimpor sebagai Normal Map; mask linear **R metallic, G AO, A 1−roughness** (biru tidak dipakai), direpack ke slot Metallic/Smoothness URP — jangan pasang tekstur ORM langsung.
- **Tekstur**: preset `PropAlbedo512`/`PropNormal512` untuk prop kecil, maksimum 1024 untuk permukaan besar; mipmap aktif, anisotropi 2, kompresi desktop.
- **Model**: tanpa light/camera/animasi impor, tanpa mesh collider otomatis, kompresi mesh Low, `isReadable` tidak diperlukan runtime.
- **Prefab produksi**: `WornBedsideStool`, `WornBedMat`, `BudgetMicrophone`, `GoldenPracticalLighting`, `KosEnvironmentalAudio`, `GoldenHUD`.
- **Meter dan orientasi**: satuan meter, Y up; kos approved 5 × 3,8 m dipertahankan. Jalur pemain ke pintu, kasur, meja, dan lemari harus tetap mudah dilalui.
- Audio: `Prepared/*.wav` mono 32 kHz; ambience Vorbis in-memory, SFX pendek ADPCM decompress-on-load.

---

## 9. Validasi, bukti, dan build

Runner serial: `Tools/run_golden_checks.py --stage <apply|validate|test|linux|player|windows>` (satu instance Unity saja).

```sh
python3 Tools/run_golden_checks.py --stage apply      # jalankan GoldenSceneProduction.Apply (saat authoring)
python3 Tools/run_golden_checks.py --stage validate   # KosSceneValidation + GoldenSceneValidation
python3 Tools/run_golden_checks.py --stage test       # EditMode + enter/exit PlayMode (NUnit)
python3 Tools/run_golden_checks.py --stage linux      # build player Linux (smoke Xvfb)
python3 Tools/run_golden_checks.py --stage player     # jalankan player Linux + -goldenAudit
python3 Tools/run_golden_checks.py --stage windows    # build Windows Development + arsipkan build lama
python3 Tools/package_windows_build.py                # verifikasi report lalu ZIP lengkap
```

- Setiap invokasi menulis receipt JSON + log ke `Audit/` (atau `Builds/Logs/` untuk build Windows).
- `--stage player` memakai Xvfb + PulseAudio sink privat dan memverifikasi receipt, 13 screenshot, RMS/peak mix, dan checksum; ini **bukan** bukti performa Windows/Iris Xe.
- `package_windows_build.py` memastikan report tidak basi, target/backend benar, semua file dan assembly ada, header PE32+ AMD64 valid, lalu membuat ZIP + SHA-256.
- Output build Windows: `Builds/Windows/` dan `Builds/LayarTerakhir-Windows-x86_64-Development.zip` (keduanya di-gitignore; dipindahkan ke Windows via WinSCP).
- Skrip Python lain di `Tools/` menangani sumber aset (`download_*`, `prepare_*`, `author_*`, `optimize_*`, `retexture_*`) dan runner lama `run_kos_v2_checks.py`.

---

## 10. Batasan dan keputusan yang mengikat

1. **Ruang kos 5 × 3,8 m** dengan tinggi 2,8 m sudah approved/frozen dalam revisi `goal.md`. Ukuran 6 × 4,5 m adalah tahap historis. Nama objek lama tetap dipakai untuk kompatibilitas.
2. **Gameplay yang sudah bekerja tidak ditulis ulang**: controller, interaksi, puzzle, smartphone, save/load, dan progresi. Perubahan harus aditif.
3. **Pass visual bersifat inkremental**: jangan menjalankan `PrologKosSceneBuilder` di atas scene berfurnitur; geser model bersama collider dan root interaksinya.
4. **Save pemain tidak boleh tersentuh** oleh Editor, tes, atau audit; gunakan `LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY`.
5. **Bukti harus nyata**: host headless tidak boleh mengklaim FPS, visual, atau audio yang tidak terukur. Klaim performa Iris Xe menunggu pengujian pemain di Windows.
6. **Anggaran scene**: ≤ 160k triangle terlihat, tekstur ≤ 1024, satu sumber cahaya praktis berbayang, SSAO half-res, MSAA 1× + FXAA, render scale 1, shadow atlas 1024, jarak bayangan 10 m.
7. **Scope**: Kos serta Adegan 2–3 Atmaja sampai pintu tangga sudah diimplementasikan; Adegan 4–7 dan babak berikutnya belum tersedia. Jangan menyamakan cakupan ini dengan keseluruhan Prolog.

---

## 11. Pola ekstensi untuk scene berikutnya (Rumah Atmaja dan seterusnya)

1. Buat scene baru dengan root `GameSystems` (input, campaign, HUD) dan direktori babak baru di `Scripts/` (mis. `Scripts/Atmaja/`) berisi director dengan enum stage sendiri — jangan menambah logika babak baru ke `KamarKosPrologDirector`.
2. Pakai ulang `GoldenHUD.prefab`, `HudController`/`HudPresentation`, dan bus mixer audio; ikat audio ke event director seperti `KosSceneAudio`, bukan timer acak.
3. Pakai ulang prefab/preset di `Production/`; tambahan aset baru tetap melalui `Art/Imported/<provider>` + kredit di `Audit/asset-credits.md`.
4. Semua interaksi lewat `IInteractable` + director agar prompt, lock modal, dan callback tetap konsisten.
5. Perluas `CampaignData` hanya bila kontrak `Rancangan.MD` memang menambah field; naikkan `schema_version` dan validasi di `CampaignState.IsValid`.
6. Tambahkan validasi scene khusus seperti `KosSceneValidation`, dan daftarkan scene baru di build settings di belakang `Prolog_KamarKos` (index 0 tetap prolog).
7. Ikuti `Docs/GOLDEN_SCENE_STANDARD.md` sebelum scene dinyatakan layak review.

---

## 12. Peta file kunci

| File | Peran |
| --- | --- |
| `Scripts/Core/CampaignState.cs` | Kontrak data + save/load atomik |
| `Scripts/Core/GameInputConfig.cs` | Binding input & remap |
| `Scripts/Player/FirstPersonController.cs` | Gerak, postur, mouse look |
| `Scripts/Player/InteractionInteractor.cs` | Raycast & prompt interaksi |
| `Scripts/Prolog/KamarKosPrologDirector.cs` | State machine cerita kos |
| `Scripts/Prolog/KosInteractableItem.cs` | Jembatan objek → director |
| `Scripts/Smartphone/SmartphoneController.cs` | HP, mode, baterai |
| `Scripts/UI/HudController.cs` | Logika HUD, modal, subtitle, fade |
| `Scripts/UI/HudPresentation.cs` | Presentasi HUD |
| `Scripts/Audio/KosSceneAudio.cs` | Audio terikat event cerita |
| `Scripts/Diagnostics/GoldenPlayerAudit.cs` | Smoke test player stand-alone |
| `Editor/PrologKosSceneBuilder.cs` | Greybox builder (sekali pakai) |
| `Editor/KosVisualV2.cs` | Pass koreksi V2 |
| `Editor/GoldenSceneProduction.cs` | Pass produksi inkremental final |
| `Editor/GoldenHudBuilder.cs` | Prefab HUD bersama |
| `Editor/KosSceneValidation.cs` / `GoldenSceneValidation.cs` | Validasi geometri & produksi |
| `Editor/WindowsDevelopmentBuild.cs` | Build Windows Development |
| `Tools/run_golden_checks.py` | Runner validasi/build serial |
| `Tools/package_windows_build.py` | Verifikasi & ZIP build Windows |
