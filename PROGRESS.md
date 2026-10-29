# Progress — Layar Terakhir

Satu baris per babak. Status = bukti terakhir, bukan target. Detail ada di
`PhaseN.MD` masing-masing dan laporan `Audit/` yang dirujuk di sana.

| Babak | Status | Bukti terakhir |
|---|---|---|
| Prolog | implemented, Linux verified; Windows acceptance pending | Audit29/30Okt (commit laporan ini): seluruh47 ID diuji visual/interaksi di Linux,47 PASS sementara;37 kasus terarah terbaru PASS,20/20 scene/continuity pada run akhir. Capture dan batas platform: `Audit/PROLOG_47_AUDIT_2026-10-29.md`. Tidak membuat build/upload baru. |
| Interlude (Prolog→P1) | implemented, Linux verified; Windows acceptance pending | Bug47: lingkungan permanen dipulihkan setelah rebuild kos, fallback prefab lengkap dan binding benar; tujuh kasus continuity PASS, capture malam/pagi/senja konsisten. `Audit/PROLOG_47_AUDIT_2026-10-29.md`. |
| Phase 1 | implemented, acceptance pending | Overhaul 28 Okt (`182695a4`): anti-spoiler diegetik, Jurnal deduksi, patroli/pendengaran/windup Genderuwo, fairness Kuntilanak QA-21/35, dinamo guard, handrail+jendela referensi rusunawa; EditMode 56/56; audit native 66 checks Act1–Act4. Windows full build + R2 verified run `20261028-083055` (ZIP 3,02 GB, sha256 `8196e20f…`). Playtest manusia/visual GPU-target belum. Lihat `Audit/PHASE1_OVERHAUL_2026-10-28.md`. |
| Phase 2 | implemented, acceptance pending | Sesi 1–2 implemented; Windows candidate + R2 verified; lihat `Phase2.MD` §status |
| Phase 3 | implemented, acceptance pending | Main route native CP7 lulus; QA matrix di `Audit/phase3-qa-matrix.md` |
| Phase 4 | implemented, acceptance pending | Build review Windows 25 Okt OK; regresi 280/280; penerimaan manusia/180 QA belum |
| Phase 5 | implemented, acceptance pending | Playable + ZIP Windows + R2 verified; 232 tes + 151 native checks lulus; 180 skenario manusia belum |

Konvensi: `done` = human acceptance selesai; `implemented, acceptance pending` =
kode + automated evidence ada, penerimaan manusia/visual/GPU belum; lihat
`goal.md` untuk catatan playtest desainer terbaru.
