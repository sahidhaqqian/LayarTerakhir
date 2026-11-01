---
name: windows-build-partial
description: Build Layar Terakhir for Windows x86_64 Mono with a selectable scene subset (prolog + phases 1-5), package the tagged ZIP and upload it to its own Cloudflare R2 key in a detached tmux session. Use ONLY when the user explicitly requests a partial/custom Windows build. Never touches the full-release pipeline (skill windows-build-r2), its last-good player, or its R2 release object.
---

# windows-build-partial

Separate, experimental pipeline for **partial Windows development players**:
prolog (always included, it is the startup scene) plus a user-picked subset
of phases 1–5. Faster to build and smaller to download than the full player,
for playtesting a slice of the game.

It reuses the persistent workspace Library/Bee cache of `windows-build-r2`
(read-only sharing: sync + cache only) but keeps everything else separate:

- own output dir `Builds/Windows-<TAG>/` (never `Builds/Windows/`),
- own player base `last-good-<tag>/` (the full `last-good/` is never read or written),
- own ZIP `LayarTerakhir-Windows-x86_64-<TAG>.zip` and **own R2 key**
  (`r2:layarterakhir/Layarterakhir/<same-name>`), so a partial build can
  never overwrite the full release,
- own receipts under `Audit/PartialBuilds/<TAG>/` (never the release receipts).

Use ONLY when the user explicitly asks for a partial/custom Windows build.
Never trigger automatically after gameplay or code tasks.

## Commands

Run from the repo root. Every script supports `--help`.

```bash
# Interactive picker when --scenes is omitted (prolog always included):
.opencode/skills/windows-build-partial/scripts/start.sh [--scenes 1,2] [--overwrite-r2] [--skip-upload]

# Poll status and inspect (same exit codes as windows-build-r2):
.opencode/skills/windows-build-partial/scripts/status.sh [--all] [--json]
.opencode/skills/windows-build-partial/scripts/logs.sh [--list] [-f] [pipeline|sync|build|unity|package|upload]

# After Succeeded: copy tagged receipts (+ optionally the ZIP) into the live tree:
.opencode/skills/windows-build-partial/scripts/collect.sh [--with-zip] [--force]
```

Scene picker (groups; prolog is mandatory and always built first):

```text
0 = Prolog saja (KamarKos + RumahAtmaja + KoridorKos, startup)
1 = Phase 1 Babak1_Rusunawa
2 = Phase 2 Babak2_RSUD
3 = Phase 3 Babak3_PabrikGula
4 = Phase 4 KarangSetra
5 = Phase 5 PasarSetan
```

Examples: `--scenes 1,2` builds prolog + phase 1 + phase 2 (`TAG=Prolog-P1P2`);
`--scenes 3` builds prolog + phase 3 (`TAG=Prolog-P3`); `--scenes 0` builds
prolog only (`TAG=Prolog`). `full`/`all` is refused here — that is the job of
`windows-build-r2`.

## How it works

1. `start.sh` validates the spec, refuses while any build runs (shared
   active-run marker + shared tmux session with `windows-build-r2`, so the
   two skills can never build concurrently), then launches
   `pipeline-partial.sh` detached.
2. Same workspace sync as the full pipeline (additive Library merge, live
   tree only ever read). If the per-tag player base is missing, it is
   restored from `last-good-<tag>/` when available.
3. `run_golden_checks.py --stage windows` with `WINDOWS_BUILD_SCENES` and
   `WINDOWS_BUILD_TAG` exported. Partial builds always cook full assets
   (scripts-only patching is disabled for subsets: there is no guarantee a
   matching-content base exists).
4. `package_windows_build.py --scenes-spec … --build-tag …` validates only
   the included scenes/markers, then zips the tagged player.
5. Upload goes to the tagged R2 key with readback verification. Without
   `--overwrite-r2` an existing tagged release blocks the upload (same
   guard as the full pipeline).
6. Only the per-tag `last-good-<tag>/` is refreshed on success; the full
   `last-good/` is never touched.

## Guards

- One active build at a time across both skills (shared lock + session).
- Partial output, last-good, ZIP, R2 key and receipts are all tagged; the
  full release paths are never written.
- `collect.sh` writes only under `Audit/PartialBuilds/<TAG>/` (and, with
  `--with-zip`, the tagged ZIP into `Builds/`); it never touches release
  receipts or the full archive.
- A partial player ends where its scenes end: progressing past the last
  included phase fails (missing scene). Playtest only the included slice.

Config changes take effect after quitting and restarting opencode.
