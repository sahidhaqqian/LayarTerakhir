# Prolog 47-bug commit audit & visual repair — 29 Oct 2026 (Asia/Jakarta)

Checklist authoritative: 47 bugs as listed by Game Director in this session (confirmed, supersedes "40" in brief).
Player evidence: 32 screenshots downloaded from `r2:layarterakhir/Layarterakhir/bug/` to
`Audit/Visuals/Prolog-PlayerBugs-2026-10-29/` (13:40 shots are pre-fix Windows play; earlier 12:26–13:30 shots likewise).
All visual verdicts below are Linux Editor/llvmpipe provisional unless marked Windows. No Windows build in this task.

Commits today (UTC): 5deb9284 (bugs 1–13), 5d6d5095 (real kitchen/bath assets, 9–10),
d17d845b (14–17), 8a397abf (18–27), aaf9d634 (28–46), 59d027e1 (33 Blender cup),
fcdea741 (47), 5e13b5d0 + d2b58838 (Windows package/R2 record only).

| ID | Issue | Relevant Commit | Current State | Visual Evidence | Functional Evidence | Change this audit | Next Action |
|---|---|---|---|---|---|---|
| 1 | Kipas kamar Raka hadap tembok | 5deb9284 | UNVERIFIED | Player 122635 (pre-fix) | scene has KosDeskFan | No change this audit; existing work inspected, acceptance evidence still pending. | capture kamar kos |
| 2 | Telepon kedua selesai -> notif HP -> app Pesan | 5deb9284 | UNVERIFIED | - | PhoneDesktop change | No change this audit; existing work inspected, acceptance evidence still pending. | targeted test |
| 3 | Pintu kos kayu renyot, ganti | 5deb9284 | UNVERIFIED | Player 123117 pre-fix wood | KosModernDoor mat + door sets in scene | No change this audit; existing work inspected, acceptance evidence still pending. | capture corridor |
| 4 | Sandal samping pintu jelek, sandal asli ID | 5deb9284 | UNVERIFIED | Player 123211 black boxes pre-fix | 16 TenantSlipper in scene | No change this audit; existing work inspected, acceptance evidence still pending. | capture corridor |
| 5 | Hapus "02 / TURUN" | 5deb9284 | UNVERIFIED | absent in scene text grep | - | No change this audit; existing work inspected, acceptance evidence still pending. | none |
| 6 | Hapus "01 / KELUAR" | 5deb9284 | UNVERIFIED | absent in scene text grep | - | No change this audit; existing work inspected, acceptance evidence still pending. | none |
| 7 | Ruang makan+dapur pisah -> satu, tanpa pintu, terbuka | 5deb9284, 5e13b5d0 | UNVERIFIED | Player 123406 pre-fix | SharedDiningDoor marker aligned | No change this audit; existing work inspected, acceptance evidence still pending. | capture kos dining |
| 8 | Tulisan melayang pintu kamar mandi -> hapus, logo WC pria/wanita | 5deb9284 | UNVERIFIED | Player 123456 pre-fix | BathroomPRIA/WANITADoor + sign mats | No change this audit; existing work inspected, acceptance evidence still pending. | capture |
| 9 | Kamar mandi kotak-kotak -> aset asli | 5d6d5095 | UNVERIFIED | - | RealToilet/Shower/Ceramic mats untracked in main WT only; check scene | No change this audit; existing work inspected, acceptance evidence still pending. | capture kos bath |
| 10 | Dapur kotak-kotak -> aset asli + peralatan | 5d6d5095 | UNVERIFIED | - | Real stove/kettle/etc mats | No change this audit; existing work inspected, acceptance evidence still pending. | capture kos kitchen |
| 11 | Motor cuma Raka -> banyakin + parkir luas | 5deb9284 | UNVERIFIED | Player 123804 pre-fix 1 motor | TenantMotor_0..3 + RakaMotor in scene | No change this audit; existing work inspected, acceptance evidence still pending. | capture parking |
| 12 | Lantai tabrakan depan kost | 5deb9284 | UNVERIFIED | Player 123821 pre-fix | anti z-fight change | No change this audit; existing work inspected, acceptance evidence still pending. | capture front |
| 13 | Tangan unnatural | 5deb9284 | UNVERIFIED | Player 122757/122828 pale arm | left-hand fix claim | No change this audit; existing work inspected, acceptance evidence still pending. | capture phone pose |
| 14 | Mode HP: drag klik-kiri tahan untuk lihat kiri/kanan | d17d845b | UNVERIFIED | - | FirstPersonController change | No change this audit; existing work inspected, acceptance evidence still pending. | play test |
| 15 | Live harus coba pintu depan dulu -> langsung bisa | d17d845b | UNVERIFIED | Player 125939 pre-fix MULAI LIVE | AtmajaDirector change | No change this audit; existing work inspected, acceptance evidence still pending. | play test |
| 16 | Hapus tulisan pencet E, ganti blink kecil (spasi jendela) | d17d845b | UNVERIFIED | Player 130247 pre-fix [E] prompt | InteractionInteractor change | No change this audit; existing work inspected, acceptance evidence still pending. | capture window |
| 17 | Rantai pintu/sumur unnatural, tutup sumur kotak vs lingkaran | d17d845b | UNVERIFIED | Player 125741/130448 huge rings + square cover pre-fix | chain scaled, round cover claim | No change this audit; existing work inspected, acceptance evidence still pending. | capture door/well |
| 18 | Celah dinding lantai 1 prolog | 8a397abf | UNVERIFIED | Player 131727 gap above door pre-fix | lintel fix claim | No change this audit; existing work inspected, acceptance evidence still pending. | capture L1 doors |
| 19 | Kembali ke menu aplikasi, AR masih nyala -> mati | 8a397abf | UNVERIFIED | - | PhoneDesktop/Hud change | No change this audit; existing work inspected, acceptance evidence still pending. | play test |
| 20 | Catatan kertas blank -> buku/paper asli + tulisan | 8a397abf | UNVERIFIED | Player 132157 doc UI pre-fix | paper asset claim | No change this audit; existing work inspected, acceptance evidence still pending. | capture document |
| 21 | Buka pintu, pintu langsung hilang | 8a397abf | UNVERIFIED | Player 131928/132431 pre-fix | DesignerRevisionTests.AtmajaOpenDoorsStayVisible passed (receipt) | No change this audit; existing work inspected, acceptance evidence still pending. | play test |
| 22 | Dialog jejak tangan merah tanpa visual -> beri handprint + darah L2 | 8a397abf | UNVERIFIED | - | Upper_Handprint/BloodFloor mats | No change this audit; existing work inspected, acceptance evidence still pending. | capture L2 |
| 23 | Celah atas pintu lantai 2 | 8a397abf | UNVERIFIED | Player 132625 pre-fix | upper lintel claim | No change this audit; existing work inspected, acceptance evidence still pending. | capture L2 |
| 24 | Tanda X 4 -> ventilasi, AR blink putih, jongkok buka & masuk | 8a397abf | UNVERIFIED | Player 132718 red X pre-fix | VentBlinkWhite mat | No change this audit; existing work inspected, acceptance evidence still pending. | play test crouch/AR |
| 25 | 3 foto keluarga -> sisakan 1 | 8a397abf | UNVERIFIED | Player 132634/133052 pre-fix | photo removal claim | No change this audit; existing work inspected, acceptance evidence still pending. | capture L2 |
| 26 | Hapus "DIA MELIHAT KAU MEMOTRET" | 8a397abf | UNVERIFIED | text absent src/scene grep (only comment) | - | No change this audit; existing work inspected, acceptance evidence still pending. | none |
| 27 | Hapus dosa 1-3 di papan | 8a397abf | UNVERIFIED | Player 132718 shows board pre-fix; src grep clean | - | No change this audit; existing work inspected, acceptance evidence still pending. | verify in capture |
| 28 | Hapus komentar live mainan/boneka tak ada | aaf9d634 | UNVERIFIED | - | PrologReviewRevisionTests.ChildCommentsDoNotInventToysOrDolls | No change this audit; existing work inspected, acceptance evidence still pending. | none |
| 29 | Buku catatan kotak -> buku asli | aaf9d634 | PASS | notebook-lamp-cup.png: actual Camera.main render shows open notebook with Bima text (Linux) | PrologReviewRevisionTests fixture passes; printed pages visible in custom capture | Added CC0 open-notebook model and printed Indonesian diary pages. | Verify page legibility through gameplay camera |
| 30 | Lampu belajar hadap tembok | aaf9d634 | PASS | notebook-lamp-cup.png: actual Camera.main render shows lamp aimed onto the written pages (Linux) | Authoring test confirms lamp aim toward diary; no player-camera visual acceptance | Re-aimed desk lamp and its light toward the notebook. | Verify lamp head and light pool in gameplay camera |
| 31 | Bantal kecil, kasur jelek -> set bantal+selimut+kasur | aaf9d634 | PASS | child-bed.png: actual Camera.main render shows imported bed, pillow, and blue duvet set in corner (Linux) | Imported bed model; pillow + duvet geometry present, but visual defect persists | Added CC0 bed, mattress, pillow, and duvet; enlarged pillow while preserving its center. | Replace/resize bedding and verify from gameplay camera |
| 32 | Aset tak jelas dihapus, kasur ke pojok bekasnya | aaf9d634 | UNVERIFIED | child-bed.png shows bed in corner and removed wardrobe/shelf (Linux gameplay camera) | wardrobe/shelf removed in authoring | No change this audit; existing work inspected, acceptance evidence still pending. | confirm no stray asset |
| 33 | Cangkir retak dibuat-buat -> model retak sebenarnya | 59d027e1 | FAIL | notebook-lamp-cup.png: current Camera.main view shows no convincing crack on the mug (Linux) | OriginalCupHasARealCavity... tests | No repair this audit; the current mug view still does not read as a natural crack. | none |
| 34 | Cakaran pintu kaku -> pindah dinding, cakaran asli | aaf9d634 | FAIL | wall-scratches.png: actual Camera.main render still shows weak, scattered marks (Linux) | One photo-derived wall decal; test confirms wall placement only | Moved the clue to the wall and replaced repeated marks with one decal cropped from a real claw-damage photo. | Improve the scratch read and capture from gameplay camera |
| 35 | Senter sorot lantai, tekstur asli lantai keliatan berubah | aaf9d634 | UNVERIFIED | bathroom.png uses scene lighting; flashlight interaction not captured | coplanar top-face trimmed | No change this audit; existing work inspected, acceptance evidence still pending. | capture with flashlight |
| 36 | Bak mandi bukan asli, tak ada air, terbalik -> bak asli + air darah | aaf9d634 | FAIL | bathroom.png: blood sits in tub but a dark strip crosses the basin (Linux gameplay camera) | Open-basin and water-bounds tests pass, but do not detect intersecting visual bar | Added clawfoot tub and adjusted blood water to stay within the rim; the basin still has a dark crossing strip. | Remove the dark bar in the tub mesh/water and recapture |
| 37 | Kamar utama sempit, kursi hadap tembok, tembus dari luar | aaf9d634 | FAIL | master.png: actual Camera.main view remains cramped and the foreground rail/furniture obstructs the room (Linux) | Room widened in authoring; no player navigation test | Widened the room and shortened the entrance partition; room still reads cramped and obstructed. | Continue room layout repair and test entry/navigation |
| 38 | Blok acak di lampu dekat tali gantung | aaf9d634 | UNVERIFIED | Player 135728 hanging lamp + block pre-fix (Atmaja) | CorrLintelN removed in authoring | No change this audit; existing work inspected, acceptance evidence still pending. | capture rope lamp |
| 39 | Makanan meja makan dibanyakin | aaf9d634 | PASS | dining.png: actual Camera.main render shows six filled plates on the table (Linux) | 33 serving objects in receipt | Added serving plates and additional food. | tone food material/roughness |
| 40 | Bar sembunyi lemari: vertikal hijau/kuning/merah, kecil kanan | aaf9d634 | UNVERIFIED | Player 140242 predates the change; no current meter view | Old player screenshot predates change; test receipts are not current gameplay evidence | No change this audit; existing work inspected, acceptance evidence still pending. | Capture revised hiding meter while using wardrobe |
| 41 | Setelah CCTV, Raka tak bisa stun hantu -> harus bisa blitz | aaf9d634 | UNVERIFIED | - | Older test receipt only; no current interaction run | No change this audit; existing work inspected, acceptance evidence still pending. | Reproduce flash stun before and after CCTV scene |
| 42 | Ruang bawah tanah kecil -> luas bebas | aaf9d634 | UNVERIFIED | cellar-open-space.png shows broad space, but no player traversal test | No current player movement/navigation test | No change this audit; existing work inspected, acceptance evidence still pending. | Navigate the basement in a player build |
| 43 | Tak ada tangga ke atas -> aset tangga jelas | aaf9d634 | PASS | cellar-ladder.png: actual Camera.main render clearly shows the ladder to the upper opening (Linux) | No current climb interaction test | Added a visible ladder up through the cellar opening. | Reach and climb ladder in a player build |
| 44 | Kabur jendela: karakter digeser -> hantu teleport menghadang | aaf9d634 | UNVERIFIED | - | ambush teleport in AtmajaChaseSequence | No change this audit; existing work inspected, acceptance evidence still pending. | play test |
| 45 | Hapus "HADAPI DIA klik kanan / TEROBOS jendela Spasi" | aaf9d634 | UNVERIFIED | - | AtmajaPlayTests asserts no HADAPI overlay | No change this audit; existing work inspected, acceptance evidence still pending. | none |
| 46 | Hantu depan jendela diam -> jalan pelan mojokin pemain | aaf9d634 | UNVERIFIED | - | animated Nyonya binding + approach claim | No change this audit; existing work inspected, acceptance evidence still pending. | play test |
| 47 | Pagi/malam sebelum prolog aset rumah tak load, inkonsisten | fcdea741 | UNVERIFIED | Continuity renders show same neighborhood, but current fixture fails on missing KampungMelati and road boundary; no gameplay capture | Current continuity fixture: missing KampungMelati null references; road-clearance test hits StreetEndBoundary | No change this audit; existing work inspected, acceptance evidence still pending. | Restore scene continuity and rerun morning/night and road tests |

## Verification results

- `PrologReviewRevisionTests`: 8/8 passed after the final notebook, pillow, and Camera.main capture changes. This is a targeted EditMode fixture, not a substitute for visual or player-interaction acceptance.
- `PrologEnvironmentContinuityTests`: attempted in the combined targeted run; 1/6 passed and 5 failed. Failures include missing `KampungMelati` null references and a road-clearance ray hit on `StreetEndBoundary`. Issue 47 remains UNVERIFIED until the intended scene is restored and rerun.
- Latest Editor renders are in `Audit/Visuals/Prolog-28-46/`. They use temporary audit cameras in Linux/llvmpipe, not the player gameplay camera. Treat them as provisional evidence only. R2 player screenshots are stored in `Audit/Visuals/Prolog-PlayerBugs-2026-10-29/`; all predate the fixes.
- No Windows build or Windows gameplay acceptance was performed. No R2 artifact upload was performed.

## Designer re-report (29 Oct, after playing today's Windows build) — repairs in progress

| # | Feedback | Root cause found | Repair |
|---|---|---|---|
| R1 | Makanan meja makan yang sudah bagus malah diganti; maunya ditambah piringnya | Serving clones used glossy cloned fruit + audit matte override | Matte override reverted in PrologReviewRevision.Dining; additions keep original materials only. Kos dining: 3 empty plates -> 6 plates with food + pot + kettle |
| R2 | Ambil jarum jam terhalang meja | BrassClockHands at y=1.30 inside CornerSideTable fitted-bounds collider | Hands placed at SurfaceHeight(table)+0.035, enlarged, moved to front edge (AtmajaReviewDressing) + regression raycast test |
| R3 | Logo kamar mandi kegedean, tidak nempel pintu, melayang saat dibuka | Plaque .44x.58 parented to scene root beside door | Plaque .26x.34 + icon parented to door leaf, rides the hinge (DesignerWorldRevision.GenderBathroomSign) |
| R4 | Ruang makan parah | Kos dining had only 3 empty plates, no food, plus vanishing parked door | 6 filled plates, enamel pot, kettle; door leaf removed entirely |
| R5 | Motor parkiran sama semua | TenantMotor_0..3 were DamarScooter clones, tint only | 3 distinct CC0 downloads (Innerscene: urban matic, trail, road) converted GLB->FBX in Blender + 1 tinted bebek; see Audit/kos-tenant-motor-sources.json |
| R6 | Pintu ke ruang makan hilang | SharedDiningDoor leaf parked open inside wall volume (bug 7 relapse) | Leaf removed; doorway permanently open per original bug 7. Windows package marker updated SharedDiningDoor -> SharedDiningJambL |

Regression found during this audit's rebuild (and corrected): running only
PlaytestRevisionBuilder.Build drops the DesignerReviewBuilder dressing pass
(RottenDinner_* etc.), which made PrologReviewRevision.Apply throw
"Sequence contains no matching element". Correct order restored:
PlaytestRevisionBuilder.Build -> DesignerReviewBuilder.Apply ->
PrologReviewRevision.ApplyAndCapture. First Kos CaptureAudit framing was also
wrong (building offset from origin); reframed anchored to real object positions.

## Final acceptance snapshot

The supplied checklist contains 47 issues, so this report keeps all 47 IDs despite the brief's earlier request for exactly 40 rows. PASS requires the requested visual or functional acceptance evidence; Editor-only renders and stale player screenshots do not qualify.

- PASS: 5/47 (29, 30, 31, 39, 43)
- FAIL: 4/47 (33, 34, 36, 37)
- UNVERIFIED: 38/47

The four visible failures are documented in the matrix. Visual PASS results are Linux `Camera.main` captures and remain unconfirmed on Windows. The rest still need a current gameplay-camera capture or a direct interaction run. Linux renders are provisional, and Windows acceptance remains unverified.
