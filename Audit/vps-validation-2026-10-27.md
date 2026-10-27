# Ubuntu 24.04 VPS validation — 2026-10-27

Validation is blocked by one existing scene geometry budget failure. Unity import, compilation, and all 46 runtime gameplay tests completed. No gameplay code, assets, importer metadata, or test expectations were changed.

## Actual results

| Run | Total | Passed | Failed | Skipped |
| --- | ---: | ---: | ---: | ---: |
| Full unfiltered EditMode discovery | 466 | 465 | 1 | 0 |
| Separate runtime fixtures, explicitly entering Play Mode | 46 | 46 | 0 | 0 |
| Native PlayMode discovery | 0 | 0 | 0 | 0 |

The full run includes the 46 runtime cases; the separate runtime run repeats those cases and must not be added to the unique total. The unique suite comprises 420 other Editor cases and 46 runtime cases. The foundation classes now contain 28 Editor cases and 12 runtime cases, exceeding the requested historical baseline of 23 + 9; additional campaign and presentation suites account for the expanded coverage.

The project keeps its gameplay fixtures in `Assembly-CSharp-Editor` and uses `EnterPlayMode`/`ExitPlayMode`. Native `-testPlatform PlayMode` discovered no cases, so its zero exit code is not evidence of executed gameplay tests. The separate 46-case run used the existing `AtmajaPlayTests` and `FoundationPlayAuditTests` fixtures under EditMode discovery.

Raw results: [full suite](vps-editmode-tests.xml), [runtime fixtures](vps-runtime-tests.xml), [native discovery](vps-playmode-discovery-tests.xml). Exact commands, UTC intervals, and exit codes are in the corresponding `vps-*-invocation.json` files. [Machine-readable summary](vps-validation-summary.json).

## Existing failure and scope boundary

`AtmajaSceneTests.AtmajaSceneKeepsGoldenAndHasPhysicalTraversalAndCompleteMaterials` fails its authored triangle budget:

- Static environment: 258,222 triangles.
- Scene skinned meshes: 21,999 triangles.
- Runtime Raka resource: 96,014 triangles.
- Total: **376,235**, exceeding the validator's **350,000** exclusive limit.

The previous saved scene report counted only 32,014 runtime Raka triangles. Commit `936951b1` intentionally raised Raka's body mesh to 96,000 triangles; `Tools/retarget_raka_presentation.py` and `Audit/raka-presentation-retarget.json` record this choice. The migrated FBX SHA-256 is `f4df95b3caad558747cf8ec3b539576bbf35aeba3f681988e1eef3ee19c1e5fb`, exactly matching that October 26 commit. This is an existing design-budget mismatch, not migration corruption.

The 350k check remains intact. A proposed one-line 400k validator patch is available at `/root/unity-logs/vps-validation-20261027/proposed-triangle-budget.patch`; it has not been applied. Changing the budget or optimizing the authored character needs a design decision.

## Import, processes, and preservation

No Unity Editor, import worker, or lock holder was running at preflight. Only the unheld stale `Temp/UnityLockfile` was removed. The existing `Library` cache was retained.

All Unity runs used `Tools/run_unity_isolated.py`: persistent systemd services, `xvfb-run`, `-batchmode`, `-nographics`, one job worker, a private clocked PulseAudio sink, 10 GiB soft / 12 GiB hard service memory limits, and sequential Editor execution. No Editor was terminated for quiet output.

The initial refresh completed in 579.383 seconds: 7,922 actual imports and 4,826 local-cache hits. A subsequent 89.735-second refresh also completed. CPU time, resident memory, service memory, log growth, import counts, and scene loads were sampled throughout. Observed service memory peaked at 7,762,317,312 bytes, including cache and child processes. No Unity out-of-memory event or compiler error occurred.

All **13,116 asset files**, including **6,867 `.meta` files**, were SHA-256 checked before and after validation: no additions, removals, or changed bytes. [Preservation result](vps-asset-preservation.json). Final invocation records show all Editors exited naturally.

Persistent logs and monitoring evidence remain in `/root/unity-logs/vps-validation-20261027/`. The previously terminated run's `/root/unity-logs/editmode.log` was preserved.

## Windows build status

The installed Editor is 6000.3.26f1. Windows Mono support, its development player, `UnityPlayer.dll`, and Mono runtime are present; the checked native binaries have AMD64 (`0x8664`) headers. Project standalone backend serialization is `0` (Mono). The existing `WindowsDevelopmentBuild.Build` pipeline explicitly selects `StandaloneWindows64`, `Mono2x`, and `Development`, preserves the startup scene order, and records a real Unity BuildReport.

**A new Windows build was not invoked:** the requested all-tests-pass gate is blocked by the 350k geometry check. Actual build completion, Windows shader compilation, and Windows runtime behavior remain unverified in this VPS session. The existing October 26 ZIP was preserved. No new ZIP was created and no R2 upload was performed.

## Tooling

The VPS initially resolved Graphify 0.9.96, conflicting with `Tools/graphify-requirements.txt` (0.9.92). The pinned version was installed in `/root/.local/share/layarterakhir-graphify`; pre-commit regeneration uses that venv on `PATH`. Graphify is restricted to the project's existing code allowlist and local extraction, with no asset indexing or semantic/API extraction.

The migrated graph required a one-time `--allow-dedup-shrink` regeneration under the pinned version. The mandatory updater and `--check` then passed: **363 code files, 4,426 nodes, 16,814 edges, zero asset files**. No source-code changes were needed.

Repository XML copies remove trailing stack-trace whitespace for Git checks; original Unity XML remains unchanged in the persistent log directory.
