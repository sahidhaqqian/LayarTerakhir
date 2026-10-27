---
name: dev-workflow
description: Agent productivity workflow for Layar Terakhir Unity development — smart validation ladder, subagent delegation, targeted exploration, cache preservation, regression-test rule, and duration tracking. Muse Spark 1.3 stays the primary implementation agent.
---

# dev-workflow

Productivity rules for agents working on this repo. They supplement AGENTS.md:
follow AGENTS.md first, use this skill to choose the cheapest safe way to do
the work. Phase-by-phase development and review (Prolog, Phase 1–5) stays as
is; workflow must never delay phase work (see "Priorities" below).

## 1. Smart validation — cheapest relevant check first

Run validation as a ladder, stop at the first failure, never jump ahead:

1. **Syntax (seconds):** `bash -n` for shell, `python3 -m py_compile` for
   Python, brace/paren balance scan for C#. Catches typos before Unity.
2. **Targeted EditMode tests (minutes):** `run_golden_checks.py --stage
   test --test-filter <Fixture>` for exactly the changed area. One run per
   completed change-set, not per edit.
3. **Scene/data validation:** open-scene EditMode tests only when builders
   or geometry changed (same `-testFilter`, they open the scene headless).
4. **Native smoke (tens of minutes, background tmux):** `run_phase1_checks.py
   player` (or the phase equivalent) only when runtime behavior changed and
   EditMode is green. Never gate a commit on smoke when EditMode already
   covers the change — record smoke as follow-up instead.
5. **Full regression / Windows build:** only on explicit user request, a
   milestone, or a critical shared-system change (per AGENTS.md).

Do not trigger Windows builds or R2 uploads unless the user explicitly
requests them — the `windows-build-r2` skill owns that pipeline and already
picks scripts-only vs full automatically.

## 2. Persistent context

- Keep AGENTS.md concise: workflow detail lives here, not there.
- Keep `PROGRESS.md` (repo root) current: one line per phase with status
  (done / in-progress / blocked) plus the latest commit. Update it in the
  same commit that completes a phase milestone — never a separate docs-only
  commit, never stale.
- `goal.md` remains the designer's playtest notes; do not duplicate them
  into progress docs, link instead.

## 3. Subagent delegation

Muse Spark 1.3 is the primary implementation agent; subagents are scouts and
parallel workers, never owners. Delegate when ALL of these hold:

- the task splits into pieces touching **disjoint file sets**;
- each piece states its return contract (files read, facts found, no edits
  unless asked);
- exploration (read-only) goes to `explore` subagents with a token budget;
  implementation stays with the primary agent.

Conflict rules: one writer per file set; separate worktrees when another
agent's WIP is present (AGENTS.md); never `git add .`; verify the other
agent's files are untouched before committing (`git status`).

## 4. Targeted exploration

- Navigate via Graphify first (`graphify query "<topic>" --budget 900`),
  then read only the returned files. Never re-scan the whole repo when a
  previous query already answered the question.
- Prefer `grep`/`glob` over full-file reads for locating symbols; read
  full files only for the 1–3 files being changed.
- Scene/prefab questions go to targeted YAML greps or the existing
  open-scene EditMode tests — never dump whole scenes into context.

## 5. Preserve incremental caches

- Never delete `Library/`, `Builds/` outputs used as build bases,
  `windows-build-r2` workspace/last-good, or `Audit/` receipts. No tool or
  skill in this repo may offer cache deletion.
- Reuse the persistent Windows-build workspace (see `windows-build-r2`);
  fix forward on failure instead of wiping state.
- Keep the Unity Editor single-instance per project; keep `Library/`
  warm between sessions.

## 6. Regression tests for reproducible bugs

Every fixed reproducible bug gets a targeted regression test in the same
change-set:

- Prefer EditMode tests that open the scene headless (fast, deterministic):
  e.g. doorway passability volumes, guard-rail existence, interaction-ray
  clearance, balance-value pins.
- Use the native audit driver only for behavior EditMode cannot observe
  (timing, pursuit, multi-step flows).
- Name the test after the bug (`ArchiveRoomDoorwayStaysPassable`,
  `ClosedRoomDoorsSealTheirDoorways`); a failing test must point at the
  cause, not just assert green.

## 7. Track durations, find bottlenecks

- Log every measured validation/test/build/agent step:
  `python3 Tools/agent_metrics.py log --kind <validation|test|build|upload|explore|implement> --label <what> --seconds <s>`.
- `backfill` imports existing `Audit/*invocation*.json` receipts (run once;
  idempotent). `report` prints per-kind totals/averages and the slowest
  runs. All entries are measured wall-clock — never estimates.
- Report improvements as before/after numbers from this log, not guesses.
  If a change cannot be measured yet (e.g. a build optimization with no
  build requested), say so explicitly instead of projecting savings.

## Priorities

Completing Phase 1–5 beats perfecting this workflow. If a workflow step
costs more than the phase work it supports, skip it and note why in the
final report.
