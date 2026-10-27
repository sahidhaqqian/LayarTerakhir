---
name: windows-build-r2
description: Build Layar Terakhir for Windows x86_64 Mono, package the ZIP and upload it to Cloudflare R2 in a detached tmux session. Use ONLY when the user explicitly requests a Windows build or R2 upload.
---

# windows-build-r2

Manual release pipeline for the Layar Terakhir Windows development player.
It syncs the project into a persistent workspace, builds with the existing
Unity pipeline inside a detached tmux session, validates and zips the
player, then uploads to R2 — while development continues on the live tree.

Use ONLY when the user explicitly asks for a Windows build and/or an R2
upload. Never trigger automatically after gameplay or code tasks.

## Commands

Run from the repo root. Every script supports `--help`.

```bash
# Full pipeline in detached tmux (returns immediately):
.opencode/skills/windows-build-r2/scripts/start.sh [--overwrite-r2] [--skip-upload] [--full-build]

# Poll status (exit 0 ok / 1 running / 2 failed) and inspect:
.opencode/skills/windows-build-r2/scripts/status.sh [--all] [--json]
.opencode/skills/windows-build-r2/scripts/logs.sh [--list] [-f] [pipeline|sync|build|unity|package|upload]

# After Succeeded: copy receipts (+ optionally the ZIP) into the live tree:
.opencode/skills/windows-build-r2/scripts/collect.sh [--with-zip] [--force]

# Free disk: drop legacy per-run snapshots, keep logs + results:
.opencode/skills/windows-build-r2/scripts/clean.sh [--run <id>] [--purge] [--force]
```

`start.sh --dry-run` checks prerequisites, prints the planned build mode
(scripts-only vs full, with reason) and the workspace sync stats, then exits
without starting anything.

## Build modes (automatic)

`start.sh` compares the live tree against the last successful build
(`last-good/meta.json`: source commit, Unity version) and picks a mode:

- **scripts-only** (`BuildOptions.BuildScriptsOnly`) — only when every
  change since the last good build is compatible C# (`*.cs`, `*.cs.meta`,
  `*.asmdef`) or build-irrelevant files (docs, `Tools/`, `Audit/`, receipts
  in `Builds/`). Recompiles scripts and patches them into the previous
  player; scenes, assets and settings are untouched.
- **full** — whenever scenes, prefabs, materials, models, audio, shaders,
  `ProjectSettings/`, `Packages/`, or a Unity version change is involved,
  on the first run (no previous build), or with `--full-build`.

If a scripts-only attempt fails, the pipeline automatically retries once as
a full incremental build in the same workspace (caches kept), records the
fallback in `status.json`, and only then fails the run. There is
intentionally no `--scripts-only` force flag: forcing scripts-only over
asset changes would produce a silently stale player, which the fallback
could not detect.

## How it works

1. `start.sh` records the live git commit (plus dirty state), refuses
   duplicate concurrent builds, decides the build mode, then launches
   `pipeline.sh` in a detached tmux session (`lt-winbuild`).
2. `pipeline.sh` syncs the live tree into the persistent workspace
   (`$BASE/workspace`, rsync with `--delete`) and merges the live Unity
   `Library/` into the workspace Library **additively (never `--delete`)**,
   so the Library/Bee cache survives across runs. Git-ignored build inputs
   (`*.vrm`, Madduck art) and all `.meta` files are preserved. The live
   tree, its Library and its `Builds/` output are only ever read.
3. If the workspace player base is missing but a last-good copy exists, it
   is restored first (keeps scripts-only viable without a full rebuild).
4. Inside the workspace it runs in order:
   `record_designer_sources.py` (pre-build) → `run_golden_checks.py --stage
   windows` (Unity x86_64 Mono, scripts-only when planned) →
   `record_designer_sources.py` (post-build provenance) →
   `package_windows_build.py` (marker + CRC validation, ZIP) → R2 upload
   (only after success + ZIP validation + readback check).
5. On build failure the workspace player output is restored from last-good
   (when available) so the next run still has a valid base; the run fails
   with logs, and last-good is never touched by a failed build.
6. Only after build AND package succeed, the player output refreshes
   `last-good/` (player copy + `meta.json`: commit, mode, Unity version)
   and the ZIP + receipts land in `runs/<id>/result/`.
7. Every step writes to `runs/<id>/status.json` (commit, mode, fallback,
   build report with effective options, archive SHA-256, upload state) and
   `runs/<id>/logs/`.

## Guards

- One active run at a time (lock file + tmux check); `start.sh` refuses
  while another build runs.
- Upload happens only after a successful build and ZIP validation.
- An existing R2 release is never overwritten silently: without
  `--overwrite-r2` the run stops before upload and keeps the local ZIP.
  Typical release flow: confirm with the user, then pass the flag.
- `--skip-upload` builds and packages without touching R2.
- The live tree, its Library and its `Builds/` output are never modified by
  the pipeline. `collect.sh` is the only step that writes back, limited to
  `Audit/` receipts and (with `--with-zip`) the `Builds/` archive.
- **Caches are never deleted**: neither the pipeline nor `clean.sh` has a
  flag that removes the workspace Library/Bee cache or the last-good
  output. `clean.sh` only removes legacy per-run snapshot copies and (with
  `--purge`) finished run directories.
- **A failed build never overwrites a working build**: the ZIP in
  `result/` is written only on success, and `last-good/` is refreshed only
  after build + package succeed; on failure the workspace base is restored
  from last-good.

## After a run

1. `status.sh` until Succeeded (first run seeds the workspace Library and
   does a full build; later code-only runs go scripts-only).
2. On failure, read `logs.sh <step>` and fix forward; do not patch inside the
   workspace (`$BASE/workspace`); a fresh full build is one `--full-build`
   away, caches intact.
3. `collect.sh [--with-zip]`, review `git status`/`git diff`, then commit
   and push the receipts per AGENTS.md.
4. `clean.sh` to drop the snapshot; logs and results stay for audit.

Config changes take effect after quitting and restarting opencode.
