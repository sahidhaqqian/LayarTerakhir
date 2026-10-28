# Progress — Layar Terakhir

Satu baris per babak. Status = bukti terakhir, bukan target. Detail ada di
`PhaseN.MD` masing-masing dan laporan `Audit/` yang dirujuk di sana.

| Babak | Status | Bukti terakhir |
|---|---|---|
| Prolog | implemented, acceptance pending | Recovery 28 Okt: state HP/fokus/RMB, retarget kaki dan kontak tangan diperbaiki; seleksi terarah 124/124 + grafis 5/5; 21 frame PlayMode; Alt+Tab/kursor desktop dan penerimaan visual manusia belum. Lihat `Audit/critical-gameplay-recovery-2026-10-28.md`. |
| Interlude (Prolog→P1) | implemented, acceptance pending | Recovery 28 Okt: jalur kapsul gerbang→warung/warteg/vendor, akses tangga, skala prop, rider lalu lintas dan sedan ukuran nyata; grup siang-only `eea7812a` dipertahankan. Tes Interlude 15/15, runtime grafis dan checkpoint lolos; penerimaan desainer belum. Lihat laporan recovery. |
| Phase 1 | in-progress | Review batch 6 isu diimplementasikan; EditMode 45/45; native smoke parsial (parkir→K102→mailbox 207, 28 checks) lolos; L3 encounter/L4/boss/return belum tervalidasi in-game (audit dihentikan atas permintaan) |
| Phase 2 | implemented, acceptance pending | Sesi 1–2 implemented; Windows candidate + R2 verified; lihat `Phase2.MD` §status |
| Phase 3 | implemented, acceptance pending | Main route native CP7 lulus; QA matrix di `Audit/phase3-qa-matrix.md` |
| Phase 4 | implemented, acceptance pending | Build review Windows 25 Okt OK; regresi 280/280; penerimaan manusia/180 QA belum |
| Phase 5 | implemented, acceptance pending | Playable + ZIP Windows + R2 verified; 232 tes + 151 native checks lulus; 180 skenario manusia belum |

Konvensi: `done` = human acceptance selesai; `implemented, acceptance pending` =
kode + automated evidence ada, penerimaan manusia/visual/GPU belum; lihat
`goal.md` untuk catatan playtest desainer terbaru.
