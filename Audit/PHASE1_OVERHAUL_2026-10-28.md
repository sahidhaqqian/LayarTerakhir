# Phase 1 — Production Overhaul (28 Okt 2026, Muse Wonder)

Branch: `phase1-overhaul-20261028` dari `main` a571f81b. Titik pemulihan:
tag `phase1-pre-overhaul-20261028` (= a571f81b). Worktree terpisah dipakai
karena worktree utama berisi WIP agen lain (217 file); tidak ada file WIP
itu yang disentuh.

## Temuan audit awal (mengoreksi asumsi brief)

- Lingkungan **bukan** primitif runtime: builder memakai mesh FBX berlisensi
  CC0 (ConcreteInterior, DoorsWindows, Poly Haven) + tekstur PBR 2K;
  `ValidateImportedScene` menolak mesh/material buatan sendiri
  (`Phase1AssetImport.cs:22-37`). Kredit lengkap di
  `Audit/phase1-asset-credits.md`. Karena itu overhaul tidak mengganti
  pustaka aset, melainkan memperbaiki arsitektur, AI, teks, dan pacing.
- Enam adegan, save schema, CP1–CP7, boss 3 halogen, dan epilog motor
  sudah terimplementasi; masalah utamanya adalah spoiler teks, AI yang
  terlalu sederhana, dan celah keadilan encounter (di bawah).

## Diimplementasikan

### Anti-spoiler & pengajaran diegetik (QA: tidak ada nama/aturan sebelum dialami)
- Chat Penonton_Terakhir Act 1 tidak lagi membocorkan "tiga bukti/cahaya
  perisai"; diganti foreshadowing atmosferik (`Phase1Director.cs:332`).
- Jurnal Klenik kini alat deduksi: lokasi bukti dan kelemahan halogen
  hanya tertulis **setelah** bukti ditemukan/identifikasi lengkap
  (`Phase1Ids.JournalText`, `Phase1Rules.cs`). Sebelumnya lokasi 402/
  kamar mandi/tungku dan lokasi arsip L2 terlihat sejak Act 1.
- Objective Act 4/Act 5, status kamera saat pursuit, notifikasi Tuyul,
  ambush kamar mandi, kemunculan Genderuwo, lift L3, foto Kuntilanak, dan
  prompt cutter/rantai/arena power ditulis ulang sebagai reaksi Raka,
  prompt objek kontekstual, atau feedback **setelah** aksi pemain —
  bukan instruksi sistem sebelum ancaman dialami.
- Label fisik L3/arena di builder diselaraskan
  (`Phase1StoryBuilder.cs`, scene dibangun ulang).
- Regression test: `UserFacingPhase1TextDoesNotSpoilEncounters`,
  `JournalHidesUnexploredLocationsAndWeakness`.

### Kuntilanak L3 (Act 3)
- Tetap sesuai §0.5/§0.7: tanpa WATCH_ONLY, tanpa top-up otomatis, panel
  dinamo fisik L3, stun foto 1 detik/5 m, grace 1,5 detik, rantai 2 detik.
- QA-35 tanpa melanggar QA-34: sebelum cutter ia tidak warp, tetapi
  berjalan menabraknya tetap fatal (kontak + LOS).
- QA-21: berbalik ke rantai adalah gerak kamera paksa; timer warp tidak
  berjalan selama pemain dalam jangkauan tangan rantai (radius tangkap
  QA-64 tetap berlaku). Regression test
  `ChainEscapeTurnDoesNotRunTheWarpTimer`.
- Satu peringatan diegetik Raka pada separuh jendela grace pertama.
- Neon koridor L3 kini berkedip seperti ballast hampir mati
  (`Phase1LightBudget`), sesuai naskah "lampu neon berkedip hampir mati".

### Genderuwo (Act 4–5)
- Patroli kini mengelilingi **seluruh** node authored L4 (galeri + mulut
  kamar + kamar mandi + dapur), bukan 8 node galeri pertama.
- Pendengaran menjadi stimulus nyata: sprint <9 m memicu investigasi ke
  titik suara; sprint <4,5 m terlokalisasi langsung.
- Ambush kamar mandi: cahaya layar hanya membocorkan bilik bila ia
  <4,5 m (sapuan bilik), bukan kematian instan dari jauh.
- Kontak fatal kini punya windup angkat-tangan 0,45 detik + audio
  (antisipasi yang terbaca; tetap fatal sesuai §0.5) di stealth, ambush,
  dan boss.
- Kemunculan 401 dan pecahnya segel 404 kini dipentaskan (impact audio +
  reaksi Raka), bukan toggle dinding senyap.
- Regression test: `PatrolCyclesTheFullAuthoredFloor4Loop`.

### Dinamo & baterai
- Putaran penuh tidak lagi terbuang pada baterai ≥99%
  (`Phase1Ids.FullChargeWasted`); hasil emergency crank diekstrak dan
  diuji (tidak pernah >10%). Regression tests terkait.
- Aturan §0.5 dipertahankan persis: refill 100% hanya transisi
  Prolog→P1; retry/Continue = snapshot; Blitz 15%/6 detik.

### Arsitektur dari referensi R2 (contoh_kost.jpg, contoh_lorong_kost.jpg)
- Parapet void kini dimahkotai **pipa handrail baja gelap kontinu**
  mengikuti foto rusunawa nyata (sebelumnya tepi beton polos).
- Setiap pintu unit bernomor mendapat **jendela samping kaca gelap
  tinggi + kusen** seperti foto koridor (pintu tidak lagi terasa slab).
- DreadOut dipakai hanya sebagai referensi atmosfer (pool cahaya senter,
  koridor gelap kontras tinggi) — tidak ada aset/desain yang disalin.
- Scene dibangun ulang: 5.283 renderer, 2.291.203 segitiga,
  missingScripts 0, missingMaterials 0, originalVisualMeshes 0
  (`Audit/phase1-scene-validation.json`). Catatan: builder HEAD memang
  sudah menghasilkan scene lebih besar dari scene ter-commit Oct 23
  (drift lama, bukan dari overhaul ini).

### Driver audit (bukan kode produksi)
- `Phase1PlayerAudit` kini menyimulasikan focus-gain OS awal seperti tes
  PlayMode Prolog. Di Xvfb tanpa window manager, gerbang fokus Alt+Tab
  HEAD memblokir semua input keyboard sepanjang run (K102 gagal bahkan
  pada kode HEAD murni — terbukti lewat biseksi; binary lama Oct 27
  lolos titik yang sama). Kode produksi tidak diubah.

## Verifikasi (tingkat bukti dipisah)

- **Automated-test verified:** EditMode `Phase1CampaignTests` 55/55
  (termasuk 6 grup tes baru) — `Audit/phase1-overhaul-tests.xml`.
  Build scene + build Linux player EXIT 0.
- **Runtime synthetic (llvmpipe):** audit nativeAct 1→Act 4 tuntas
  66 checks / 308,5 m berjalan: parkir→pompa→AR K102→mailbox 644→lift
  CP3→Kuntilanak watched-idle→cutter→rantai (lolos setelah fix QA-21)
  →3 bukti→ambush→Jurnal. Berhenti di titik yang **sama** dengan binary
  lama Oct 27 dan receipt historis: rute sintetis tertangkap patroli
  saat dash ke altar pasca-Jurnal. Boss fixture/epilog **belum**
  terverifikasi in-game pada run ini (historis: fixture tersusun).
  `Audit/Visuals/Phase1-Overhaul-Final2/player-audit.json`.
- **Visually verified (terbatas):** capture viewfinder Act 3
  memperlihatkan reveal look-back, sosok di koridor, prompt baru
  (`Audit/Visuals/Phase1-Overhaul-Final2/04-kuntilanak-watched-idle.png`).
  Capture arranged-view di host ini nyaris hitam **termasuk pada binary
  lama** — renderer llvmpipe host saat ini tidak mereproduksi pencahayaan
  Oct 23; perbandingan handrail/jendela terhadap referensi R2 hanya bisa
  dikonfirmasi pada build Windows/GPU nyata.
- **Manually playtested:** tidak ada. Tidak ada klaim playtest manusia,
  FPS Windows/Iris Xe, atau penerimaan desainer.

## Masih tertunda / batasan

1. Rute sintetis pasca-Jurnal→altar→boss→epilog belum lolos in-game
   (keterbatasan driver yang sama sejak Oct 23; perlu playtest manusia
   atau penyetelan rute audit tersembunyi).
2. Pencahayaan arranged-view llvmpipe host saat ini hitam; verifikasi
   visual final wajib di Windows (Iris Xe) termasuk handrail/jendela baru.
3. Animasi ghost/Raka masih Legacy clips existing; tidak ada aset gerak
   baru yang dibuat (sesuai larangan §0.5).
4. QA-01..74 belum seluruhnya dijalankan; yang kini ter-cover otomatis:
   QA-21, QA-31/52 (sebagian), QA-34/35 (logika), QA-59..66 (sebagian
   logika), anti-spoiler. Sisanya tetap acceptance terbuka.
5. Windows build/R2 **tidak** dijalankan (menunggu permintaan eksplisit).
