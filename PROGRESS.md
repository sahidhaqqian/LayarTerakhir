# Progress — Layar Terakhir

Satu baris per babak. Status = bukti terakhir, bukan target. Detail ada di
`PhaseN.MD` masing-masing dan laporan `Audit/` yang dirujuk di sana.

| Babak | Status | Bukti terakhir |
|---|---|---|
| Prolog | implemented, acceptance pending | Revisi playtest 29 Okt bug 28–46: aset kamar/bak, lantai koplanar, ruang master/basement, tangga terlihat, HUD sembunyi, blitz pascarekaman, dan ambush jendela. Bug 33 dilengkapi model original Blender (`ArtSources/Prolog/CrackedPorcelainCup.blend`), file sumber dan tes terarah: `Audit/PROLOG_BUGS_28_46.md` (commit ini). |
| Interlude (Prolog→P1) | implemented, acceptance pending | Recovery 28 Okt: jalur kapsul gerbang→warung/warteg/vendor, akses tangga, skala prop, rider lalu lintas dan sedan ukuran nyata; grup siang-only `eea7812a` dipertahankan. Tes Interlude 15/15, runtime grafis dan checkpoint lolos; penerimaan desainer belum. Lihat laporan recovery. |
| Phase 1 | implemented, acceptance pending | Overhaul 28 Okt (`182695a4`): anti-spoiler diegetik, Jurnal deduksi, patroli/pendengaran/windup Genderuwo, fairness Kuntilanak QA-21/35, dinamo guard, handrail+jendela referensi rusunawa; EditMode 56/56; audit native 66 checks Act1–Act4. Windows full build + R2 verified run `20261028-083055` (ZIP 3,02 GB, sha256 `8196e20f…`). Playtest manusia/visual GPU-target belum. Lihat `Audit/PHASE1_OVERHAUL_2026-10-28.md`. |
| Phase 2 | implemented, acceptance pending | Sesi 1–2 implemented; Windows candidate + R2 verified; lihat `Phase2.MD` §status |
| Phase 3 | implemented, acceptance pending | Main route native CP7 lulus; QA matrix di `Audit/phase3-qa-matrix.md` |
| Phase 4 | implemented, acceptance pending | Build review Windows 25 Okt OK; regresi 280/280; penerimaan manusia/180 QA belum |
| Phase 5 | implemented, acceptance pending | Playable + ZIP Windows + R2 verified; 232 tes + 151 native checks lulus; 180 skenario manusia belum |

Konvensi: `done` = human acceptance selesai; `implemented, acceptance pending` =
kode + automated evidence ada, penerimaan manusia/visual/GPU belum; lihat
`goal.md` untuk catatan playtest desainer terbaru.
