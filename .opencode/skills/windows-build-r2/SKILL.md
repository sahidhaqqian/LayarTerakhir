---
name: windows-build-r2
description: Build Layar Terakhir for Windows x86_64 Mono, package the ZIP and upload it to Cloudflare R2 in a detached tmux session. Use ONLY when the user explicitly requests a Windows build or R2 upload.
---

# windows-build-r2

Manual release pipeline for the Layar Terakhir Windows development player.
It snapshots the project, builds with the existing Unity pipeline inside a
detached tmux session, validates and zips the player, then uploads to R2 —
while development continues on the live tree.

Use ONLY when the user explicitly asks for a Windows build and/or an R2
upload. Never trigger automatically after gameplay or code tasks.

## Commands

Run from the repo root. Every script supports `--help`.

```bash
# Full pipeline in detached tmux (returns immediately):
.opencode/skills/windows-build-r2/scripts/start.sh [--overwrite-r2] [--skip-upload]

# Poll status (exit 0 ok / 1 running / 2 failed) and inspect:
.opencode/skills/windows-build-r2/scripts/status.sh [--all] [--json]
.opencode/skills/windows-build-r2/scripts/logs.sh [--list] [-f] [pipeline|snapshot|build|unity|package|upload]

# After Succeeded: copy receipts (+ optionally the ZIP) into the live tree:
.opencode/skills/windows-build-r2/scripts/collect.sh [--with-zip] [--force]

# Free disk: drop snapshot copies, keep logs + results:
.opencode/skills/windows-build-r2/scripts/clean.sh [--run <id>] [--purge] [--force]
```

`start.sh --dry-run` checks prerequisites (tmux, rclone `r2:` remote, Unity,
disk) and prints the plan without starting anything.

## How it works

1. `start.sh` records the live git commit (plus dirty state), refuses
   duplicate concurrent builds, then launches `pipeline.sh` in a detached
   tmux session (`lt-winbuild`).
2. `pipeline.sh` rsyncs the live tree to `/tmp/opencode/windows-build-r2/`
   excluding only outputs/caches/VCS (`snapshot-excludes.txt`). Git-ignored
   build inputs (`*.vrm`, Madduck art) and all `.meta` files are preserved;
   the snapshot gets its own fresh Unity Library.
3. Inside the snapshot it reuses the proven pipeline in order:
   `record_designer_sources.py` (pre-build) → `run_golden_checks.py --stage
   windows` (Unity x86_64 Mono) → `record_designer_sources.py`
   (post-build provenance) → `package_windows_build.py` (marker + CRC
   validation, ZIP) → `upload_windows_build.py` (R2 + readback hash check).
4. Every step writes to `runs/<id>/status.json` (commit, exit codes, build
   report, archive SHA-256, upload state) and `runs/<id>/logs/`. The ZIP and
   receipts land in `runs/<id>/result/`.

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

## After a run

1. `status.sh` until Succeeded (large first run: fresh Library import).
2. On failure, read `logs.sh <step>` and fix forward; do not patch inside
   the snapshot.
3. `collect.sh [--with-zip]`, review `git status`/`git diff`, then commit
   and push the receipts per AGENTS.md.
4. `clean.sh` to drop the snapshot; logs and results stay for audit.

Config changes take effect after quitting and restarting opencode.
