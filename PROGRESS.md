# Progress — Layar Terakhir

Satu baris per babak. Status = bukti terakhir, bukan target. Detail ada di
`PhaseN.MD` masing-masing dan laporan `Audit/` yang dirujuk di sana.

| Babak | Status | Bukti terakhir |
|---|---|---|
| Prolog | done | Batch 15 isu + follow-up standoff (`c131d6ab`, `fd370245`); AtmajaPlayTests 34/34; R2 upload `verified` |
| Interlude (Prolog→P1) | implemented, acceptance pending | Daytime hub+director; finale→INTERLUDE_MORNING→READY→P1; InterludeCampaignTests 12/12; overhaul: authored Zone A gang + west-facing warteg w/ Kenney food etalase + 2 gerobak + 6 parked motors + sedan + angkot van + catenary wires, moving traffic, vendor NPC; street verified via 8 engine-rendered screenshots (`Audit/Visuals/Interlude-Street/`); runtime player tour still blocked on llvmpipe (root-dialog + corridor-load cost) |
| Phase 1 | in-progress | Review batch 6 isu diimplementasikan; EditMode 45/45; native smoke parsial (parkir→K102→mailbox 207, 28 checks) lolos; L3 encounter/L4/boss/return belum tervalidasi in-game (audit dihentikan atas permintaan) |
| Phase 2 | implemented, acceptance pending | Sesi 1–2 implemented; Windows candidate + R2 verified; lihat `Phase2.MD` §status |
| Phase 3 | implemented, acceptance pending | Main route native CP7 lulus; QA matrix di `Audit/phase3-qa-matrix.md` |
| Phase 4 | implemented, acceptance pending | Build review Windows 25 Okt OK; regresi 280/280; penerimaan manusia/180 QA belum |
| Phase 5 | implemented, acceptance pending | Playable + ZIP Windows + R2 verified; 232 tes + 151 native checks lulus; 180 skenario manusia belum |

Konvensi: `done` = human acceptance selesai; `implemented, acceptance pending` =
kode + automated evidence ada, penerimaan manusia/visual/GPU belum; lihat
`goal.md` untuk catatan playtest desainer terbaru.
