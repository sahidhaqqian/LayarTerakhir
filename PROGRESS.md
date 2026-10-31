# Progress — Layar Terakhir

Satu baris per babak. Status = bukti terakhir, bukan target. Detail ada di
`PhaseN.MD` masing-masing dan laporan `Audit/` yang dirujuk di sana.

| Babak | Status | Bukti terakhir |
|---|---|---|
| Prolog | implemented, acceptance pending | 31 Okt: revisi Bug 1–47 `goal.md` diterapkan ke 3 scene Prolog via `Prolog47SceneRepair.ApplyCompleteRevision`; `-testFilter Prolog47` 9/9 lulus di Linux Editor (xvfb); capture `Audit/Visuals/Prolog47-2026-10-30/After/` dicek visual (pintu+logo WANITA, foto keluarga dalam bingkai, nightstand tak menembus kasur, Nyonya satu gunting tanpa sayap pinggang). Sisa: siluet kebaya atas Nyonya masih kasar (mesh VRM asli); GPU Windows/human acceptance belum. |
| Interlude (Prolog→P1) | implemented, acceptance pending | C2 Prolog47: scenery rumah/alley konsisten malam–pagi–sore, langit pagi dipulihkan eksplisit; identitas renderer sama pada capture transisi. 15/15 Interlude tests, detail/visual kos lulus. Windows/human acceptance belum. Lihat `Audit/PROLOG_47_ACCEPTANCE_2026-10-30.md`. |
| Phase 1 | implemented, acceptance pending | Overhaul 28 Okt (`182695a4`): anti-spoiler diegetik, Jurnal deduksi, patroli/pendengaran/windup Genderuwo, fairness Kuntilanak QA-21/35, dinamo guard, handrail+jendela referensi rusunawa; EditMode 56/56; audit native 66 checks Act1–Act4. Windows full build + R2 verified run `20261028-083055` (ZIP 3,02 GB, sha256 `8196e20f…`). Playtest manusia/visual GPU-target belum. Lihat `Audit/PHASE1_OVERHAUL_2026-10-28.md`. |
| Phase 2 | implemented, acceptance pending | Sesi 1–2 implemented; Windows candidate + R2 verified; lihat `Phase2.MD` §status |
| Phase 3 | implemented, acceptance pending | Main route native CP7 lulus; QA matrix di `Audit/phase3-qa-matrix.md` |
| Phase 4 | implemented, acceptance pending | Build review Windows 25 Okt OK; regresi 280/280; penerimaan manusia/180 QA belum |
| Phase 5 | implemented, acceptance pending | Playable + ZIP Windows + R2 verified; 232 tes + 151 native checks lulus; 180 skenario manusia belum |

Konvensi: `done` = human acceptance selesai; `implemented, acceptance pending` =
kode + automated evidence ada, penerimaan manusia/visual/GPU belum; lihat
`goal.md` untuk catatan playtest desainer terbaru.
