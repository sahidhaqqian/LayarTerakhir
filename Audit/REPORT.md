Audit of the existing LayarTerakhir foundation, 15 October 2026.

This audit is limited to the implemented Kamar Kos milestone. No subsequent gameplay milestone was started. Unity 6000.3.26f1 and URP 17.3.0 were retained. All execution used `-batchmode -nographics`; no graphical or human interactive playtest took place.

The requested documents are present: [Prolog.MD](../Prolog.MD), [Rancangan.MD](../Rancangan.MD), and [Checklist.MD](../Checklist.MD). Phase1–Phase5 and the DOCX GDD are also present. The checklist explicitly says unchecked entries describe planned work, not implemented features. Requirements were taken from these documents, including the separate kos preparation opening and the Prolog-first production order.

All 13 original project C# files were inspected: nine gameplay scripts, two foundation editor utilities, and two Unity template Readme scripts. Both existing scenes were inspected: `SampleScene.unity` and `Prolog_KamarKos.unity`. The original validation log and package-update log were inspected. There were no project NUnit test classes or test assemblies before this audit; the Unity Test Framework package was already installed.

The historical claim of 23/23 is not supported by the supplied log. `milestone1_validation.log` contains **20 PASS entries**: 19 structural/configuration checks and one JSON save/load round trip. A fresh execution of the original validator reproduced 20 PASS entries and process exit 0. It opens the scene in Edit Mode; it never runs the opening sequence, movement, interaction input, buttons, puzzle, donation, depletion/recharge, or departure. Its save check does not simulate interruption or establish atomicity. Exit 0 therefore established that its limited checks passed, not that gameplay worked.

Confirmed issues and fixes:

| Issue | Effect and correction |
| --- | --- |
| Saved UI lost runtime listeners | All nine saved buttons had empty persistent callback lists. `EnsureRuntimeUIBuilt` returned early when serialized panels existed, so `Awake` never rebound listeners. Documents could not close and the drawer could not be solved. Bind owned button callbacks for both saved and newly constructed HUDs; rebinding removes the previous owned delegate. |
| Interaction ray hit Raka's own controller | Headless runtime raycasts hit `Player_RakaWijaya` before the door. Ignore the player's own collider hierarchy and select the nearest remaining hit, preserving obstruction by world geometry. Use a reusable hit buffer with a full-results fallback if saturated. |
| Padlock completion could invoke its callback repeatedly | Clear the callback on close/cancel and invoke a captured callback once after a successful submission. Ignore submission when the panel is closed. |
| Phone positioned in world coordinates under a camera parent | The saved local offset was `(1.04, -1.83, 0.13)` instead of `(0.24, -0.18, 0.38)`, a 1.85 m error. Correct the existing scene and builder; initially hide the handheld rig and disable its flashlight. |
| Starting capsule overlapped the bed | Shift spawn x from `-0.8` to `-0.4`, into the aisle. Preserve the room, furniture, controller parameters, and scene GUID. |
| Build launched SampleScene | Put the kos scene first and enabled. Preserve SampleScene and other registered scenes. Apply the same ordering when building the kos scene again. |
| Recharge input unreachable with the phone stored or screen off | The early return occurred before handling B. Handle recharge before that return, retaining screen-off behavior until F is pressed. |
| Remapping accepted keys the input reader could not read | Replace the defaults-only fallback with keyboard enum mapping, explicit numeric/keypad/modifier aliases, and the Input System's five mouse buttons. |
| Save implementation deleted the previous save before moving the temporary file | Write and flush a same-directory temporary file, then replace with `File.Replace` and retain the previous snapshot as `.bak`. First saves use a same-directory move. Failed writes restore the in-memory checkpoint ID. |
| Loaded data accepted unsafe values | Validate schema, checkpoint ID, ranges, wallet values, ending enum, and nonempty unique list IDs before replacing live data. Parse into a candidate and leave live data unchanged on failure. Provide explicit backup loading rather than silently accepting an older snapshot. |
| Validator overwrote the player save | Its historical `VALIDATOR_TEST_CHECKPOINT` was present in the actual persistent save directory. Give validation and tests isolated temporary save directories in the Editor. The pre-audit persistent file was backed up for comparison and remains byte-for-byte unchanged by the audit. |
| Startup discarded checkpoint state | Restore the existing `PROLOG_KOS_COMPLETE` checkpoint to ReadyToDepart, including collected object states, phone ownership/battery, drawer state, and wallet. Do not replay the donation or regenerate consumed powerbanks. Only the existing kos completion checkpoint is restored; no future checkpoint system was implemented. |
| Departure completed even if saving failed | Commit Completed only after a successful save. Keep the door retry available on failure. Lock movement and reject further world interactions after successful departure. |
| Notification timeout and fade ordering | Track the actual notification coroutine so an older timeout cannot erase a newer notification. Move the fade image to the last Canvas sibling so the black fade covers the HUD. |
| Legacy installer could overwrite fixes | Add an existing-sources guard. The installer was syntax checked and was not executed to regenerate scripts. Its original bootstrap payload was not rewritten. |

Unity documents `AddListener` as creating a non-persistent runtime listener: [Unity 6 API reference](https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Events.UnityEvent.AddListener.html). The callback diagnosis was also reproduced directly against the saved project scene.

The scene was repaired in place, not regenerated. No architecture replacement, package installation, engine installation, or unrelated system change was performed. Unity automatically serialized a version upgrade of `PC_RPAsset.asset` during headless Play Mode; that file was restored to its exact pre-audit tracked contents afterward. The reproduction script likewise preserves that file around validation. No pipeline configuration change is retained.

Final compilation and test results:

| Check | Result |
| --- | --- |
| Unity Editor compilation | No C# compiler errors in the final validator or test logs. |
| Headless milestone validator | **20/20 PASS**, process exit **0**. [Log](final-validation.log) |
| Edit Mode regression cases | **23/23 PASS**. |
| Actual headless Play Mode scenarios | **9/9 PASS**, including both preparation orders and scene reload. |
| Combined NUnit suite | **32/32 PASS**, zero failures/skips, process exit **0**. [XML](foundation-tests.xml), [log](foundation-tests.log) |
| Broken scene references | Both scenes passed missing-script and serialized-reference checks. |
| Integrity/preservation | Original persistent save unchanged; package/configuration hashes preserved; automatic pipeline asset serialization reverted. |
| Shell scripts and tracked diff | `bash -n` and `git diff --check` passed. |

[results.json](results.json) records these results in machine-readable form. Save-failure tests deliberately obstruct the temporary save path and expect the resulting error log; these are injected failures whose preservation/retry assertions pass. No standalone player build was performed.

The audit adds meaningful regression coverage in two Editor test files. One suite checks serialized references in both scenes, spawn clearance, scene order, runtime callback rebinding, duplicate callbacks, keyboard remapping, JSON integrity, replacement/backup behavior, and failed-save preservation. The other explicitly enters Play Mode and hosts scenarios on Unity's player loop, with synthetic keyboard input. It tests both preparation orders, raycast targeting and E interaction, opening gates, letter and phone callbacks, donation, clues, incorrect/cancelled/correct drawer submissions, departure, checkpoint restoration, recharge, mode gates/drain, dark broadcast state, collision/crouch clearance, and notification lifetime.

Initial audit-harness runs are preserved separately. Timing waits and synthetic input routing required corrections for Editor-discovered tests executing in headless Play Mode. These harness failures are not reported as additional production bugs. Final pass/fail totals refer only to the corrected harness. Test input settings are cloned for the session and restored afterward.

Remaining limitations and unverified behavior:

- No graphical rendering, lighting, shadows, material appearance, handheld composition in a rendered frame, readable UI layout, actual pointer clicks, audio listening, or subjective pacing was verified. Button tests invoke `Button.onClick`; raycasts and world E interactions use synthetic input.
- Scenario tests reposition the player between interaction vantage points. Separate physics tests exercise movement/collision/crouch, but they do not establish continuous human navigation or the feel of sprint/slide/vault. SPACE currently supplies a vertical impulse; contextual obstacle vaulting is not implemented.
- The current content ends at the kos departure fade. There is no Atmaja scene or transition target. This is the current milestone boundary, not a completed Prolog. Opening sound and voice-note content are text/subtitle placeholders.
- AR/UV/Selfie state flags and battery profiles exist; camera rendering, evidence photography, Blitz, Jurnal, stealth/chase AI, the later Prolog checkpoints, collectibles, rituals, and endings are not implemented or validated here.
- Backup read/rejection and normal file-replacement/failure paths are checked. Power loss, disk exhaustion, filesystem crash recovery, and a player-facing corrupt-save recovery choice have not been validated or implemented. `.bak` recovery is an explicit data API.
- No standalone player build, graphics/GPU profiling, 60 fps claim, or minimum-memory certification was performed. Nonfatal licensing-token/debugger diagnostics in Unity logs are distinct from C# compilation and NUnit failures.

Files changed from the pre-audit working tree:

- `Assets/_LayarTerakhir/Scripts/Core/CampaignState.cs`
- `Assets/_LayarTerakhir/Scripts/Core/GameInputConfig.cs`
- `Assets/_LayarTerakhir/Scripts/Player/InteractionInteractor.cs`
- `Assets/_LayarTerakhir/Scripts/Smartphone/SmartphoneController.cs`
- `Assets/_LayarTerakhir/Scripts/UI/HudController.cs`
- `Assets/_LayarTerakhir/Scripts/Prolog/KamarKosPrologDirector.cs`
- `Assets/_LayarTerakhir/Editor/PrologKosSceneBuilder.cs`
- `Assets/_LayarTerakhir/Editor/PrologMilestoneValidator.cs`
- `Assets/_LayarTerakhir/Scenes/Prolog_KamarKos.unity`
- `ProjectSettings/EditorBuildSettings.asset`
- `install_milestone1.sh`

New files include `FoundationAuditTests.cs`, `FoundationPlayAuditTests.cs`, their Unity metadata, this report, `Audit/run-headless.sh`, machine-readable test results, and the audit diff. Logs are retained locally but ignored by the repository's existing `*.log` rule.

Most foundation scripts, the prolog scene, the installer, and design documents were already untracked before this audit. Ordinary `git diff` therefore does not describe all audit changes: against HEAD it shows only `EditorBuildSettings.asset`, with 3 inserted lines. [changes.diff](changes.diff) compares source files against a pre-audit filesystem snapshot, including the added tests: **15 files, 918 insertions, 64 deletions**, comprising 11 modified existing files and 2 new test files with their metadata. Audit documentation, runner, and results are additional artifacts. [diff-summary.json](diff-summary.json) lists each source delta. No files were staged and no commit was created. GraphicsSettings, QualitySettings, PlayerSettings, package manifest/lock, existing URP assets, and SampleScene were preserved.

Reproduce the checks with `bash Audit/run-headless.sh` using the existing installed Unity editor. The script runs the original-style structural validator and then the corrected NUnit suites. It does not build or install an engine.

The recommended next milestone, after the foundation's outstanding manual checks, is **Prolog Adegan 2: Halaman & Teras Rumah Keluarga Atmaja**: connect departure to that location, introduce the local live-broadcast tutorial, Damar's survey note, the side-path crouch obstacle, and kitchen-window vault. Continue toward the AR pickup/tutorial in Adegan 3 and the complete Prolog escape slice. Rancangan.MD and Checklist.MD require a stable Prolog slice before Babak 1; no Babak 1 work was started.
