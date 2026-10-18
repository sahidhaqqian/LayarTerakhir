# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1770 nodes · 6082 edges · 72 communities (55 shown, 17 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 313 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `6457efb7`
- Commit above is the pre-commit base, not a freshness check. Verify source/artifact hashes with `python3 Tools/update_graphify.py --check`.
- Regenerate safely with `python3 Tools/update_graphify.py` before commit/push.

## Community Hubs (Navigation)
- Community 0
- Community 1
- Community 2
- Community 3
- Community 4
- Community 5
- Community 6
- Community 7
- Community 8
- Community 9
- Community 10
- Community 11
- Community 12
- Community 13
- Community 14
- Community 15
- Community 17
- Community 18
- Community 19
- Community 20
- Community 21
- Community 22
- Community 23
- Community 24
- Community 25
- Community 26
- Community 27
- Community 28
- Community 29
- Community 30
- Community 31
- Community 32
- Community 33
- Community 34
- Community 35
- Community 36
- Community 37
- Community 38
- Community 39
- Community 40
- Community 41
- Community 42
- Community 43
- Community 44
- Community 45
- Community 46
- Community 47
- Community 48
- Community 49
- Community 50
- Community 51
- Community 52
- Community 53
- Community 54
- Community 55
- Community 56
- Community 57
- Community 58
- Community 59
- Community 60
- Community 61
- Community 62
- Community 63
- Community 64
- Community 65
- Community 66
- Community 67
- Community 68
- Community 69
- Community 70

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 82 edges
2. `HudController` - 77 edges
3. `AtmajaChaseSequence` - 55 edges
4. `KamarKosPrologDirector` - 51 edges
5. `LayarTerakhir.Prolog` - 50 edges
6. `AtmajaBasementHorror` - 49 edges
7. `FirstPersonController` - 48 edges
8. `AtmajaPlayTests` - 47 edges
9. `CampaignState` - 41 edges
10. `PhoneDesktop` - 40 edges

## Surprising Connections (you probably didn't know these)
- `AtmajaBasementHorror` --references--> `AtmajaChaseSequence`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaChaseSequence.cs
- `PhoneDesktop` --references--> `SmartphoneController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/UI/PhoneDesktop.cs → Assets/_LayarTerakhir/Scripts/Smartphone/SmartphoneController.cs
- `AtmajaBasementHorror` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Core/CampaignState.cs
- `AtmajaBasementHorror` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs
- `AtmajaBasementHorror` --references--> `AtmajaUpperDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaUpperDirector.cs

## Import Cycles
- None detected.

## Communities (72 total, 17 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (13): GoldenLinuxBuild, Record, TempShaftProbe, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core (+5 more)

### Community 1 - "Community 1"
Cohesion: 0.07
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, Campaign, Hud, IsGateRun, IsPursuing, IsStunned, Phone (+6 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (11): KosCorridorBuilder, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, SceneAudioProfile, PresentationProfile, VoiceLine (+3 more)

### Community 3 - "Community 3"
Cohesion: 0.06
Nodes (19): AtmajaSceneValidation, Report, BloodWriting, PrologPresentationBuilder, Receipt, AnimatedRakaAvatar, PlayerPerspective, FirstPerson (+11 more)

### Community 4 - "Community 4"
Cohesion: 0.09
Nodes (14): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), dialogue_literals() (+6 more)

### Community 5 - "Community 5"
Cohesion: 0.12
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 6 - "Community 6"
Cohesion: 0.12
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 7 - "Community 7"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 8 - "Community 8"
Cohesion: 0.06
Nodes (27): AtmajaPlayerAudit, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock (+19 more)

### Community 11 - "Community 11"
Cohesion: 0.08
Nodes (17): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+9 more)

### Community 12 - "Community 12"
Cohesion: 0.12
Nodes (8): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsHidden, IsHunting, Phone

### Community 13 - "Community 13"
Cohesion: 0.12
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 14 - "Community 14"
Cohesion: 0.09
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 17 - "Community 17"
Cohesion: 0.13
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 18 - "Community 18"
Cohesion: 0.10
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 19 - "Community 19"
Cohesion: 0.17
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 20 - "Community 20"
Cohesion: 0.13
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 21 - "Community 21"
Cohesion: 0.25
Nodes (3): PhoneSkin, Rounded, UiWidgets

### Community 22 - "Community 22"
Cohesion: 0.20
Nodes (3): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 23 - "Community 23"
Cohesion: 0.17
Nodes (8): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath

### Community 25 - "Community 25"
Cohesion: 0.17
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 26 - "Community 26"
Cohesion: 0.15
Nodes (21): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+13 more)

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (11): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, FeedCamera, HasGraphicsDevice, Hud, IsCameraApp (+3 more)

### Community 29 - "Community 29"
Cohesion: 0.10
Nodes (4): box(), cyl(), finish(), tube()

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 31 - "Community 31"
Cohesion: 0.12
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 32 - "Community 32"
Cohesion: 0.20
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 33 - "Community 33"
Cohesion: 0.11
Nodes (3): foley(), render(), write_pcm()

### Community 35 - "Community 35"
Cohesion: 0.12
Nodes (6): IInteractable, InteractableId, InteractionInteractor, CurrentTarget, KosTravelItem, InteractableId

### Community 36 - "Community 36"
Cohesion: 0.13
Nodes (4): DesignerRevisionTests, KosVisualV2Tests, DoubleLeafGate, IsOpen

### Community 37 - "Community 37"
Cohesion: 0.17
Nodes (3): AtmajaStairRevisionBuilder, BandVertex, KosRoomSurvey

### Community 38 - "Community 38"
Cohesion: 0.19
Nodes (4): VisualAudit, PlaytestReviewValidation, Record, KosRoomCorrections

### Community 39 - "Community 39"
Cohesion: 0.14
Nodes (9): ball(), box(), cylinder(), mat(), tube(), box(), cylinder(), finish() (+1 more)

### Community 41 - "Community 41"
Cohesion: 0.16
Nodes (9): cloth(), ellipsoid(), finish(), mesh(), tube(), cube(), cyl(), finish() (+1 more)

### Community 42 - "Community 42"
Cohesion: 0.13
Nodes (6): FoundationAuditCoroutineHost, AmbientAudioLoop, LevelMultiplier, AtmajaThreatPresentation, HasCharacterRig, ClockAmbience

### Community 43 - "Community 43"
Cohesion: 0.13
Nodes (11): CampaignData, EndingType, Bad, None, Normal, Secret, True, UiFocus (+3 more)

### Community 44 - "Community 44"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 46 - "Community 46"
Cohesion: 0.13
Nodes (4): AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 47 - "Community 47"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 50 - "Community 50"
Cohesion: 0.18
Nodes (6): FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 51 - "Community 51"
Cohesion: 0.16
Nodes (5): ExplorationCommentary, Zone, KosCorridorDirector, Busy, CanDepart

### Community 52 - "Community 52"
Cohesion: 0.21
Nodes (6): ScanVisualMode, AR, Normal, UV, SmartphoneController, Instance

### Community 55 - "Community 55"
Cohesion: 0.23
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 57 - "Community 57"
Cohesion: 0.40
Nodes (3): Check, KosSceneValidation, Report

### Community 58 - "Community 58"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 59 - "Community 59"
Cohesion: 0.20
Nodes (10): Run, Breaking, Complete, Ending, GateRun, Idle, Playback, Pursuit (+2 more)

### Community 60 - "Community 60"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 61 - "Community 61"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 62 - "Community 62"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 64 - "Community 64"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 65 - "Community 65"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 66 - "Community 66"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 67 - "Community 67"
Cohesion: 0.43
Nodes (3): fetch(), main(), sha()

### Community 69 - "Community 69"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 70 - "Community 70"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **218 isolated node(s):** `AwaitingBlitz`, `Campaign`, `Hud`, `IsGateRun`, `IsPursuing` (+213 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 396 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **17 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaChaseSequence` connect `Community 1` to `Community 0`, `Community 3`, `Community 42`, `Community 10`, `Community 12`, `Community 16`, `Community 18`, `Community 21`, `Community 54`, `Community 55`, `Community 22`, `Community 24`, `Community 59`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **What connects `AwaitingBlitz`, `Campaign`, `Hud` to the rest of the system?**
  _218 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08009395184967703 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 6` to `Community 0`, `Community 1`, `Community 3`, `Community 5`, `Community 8`, `Community 10`, `Community 12`, `Community 16`, `Community 18`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 27`, `Community 32`, `Community 36`, `Community 38`, `Community 42`, `Community 44`, `Community 51`, `Community 54`, `Community 55`?**
  _High betweenness centrality (0.048) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.06949152542372881 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 18` to `Community 0`, `Community 1`, `Community 3`, `Community 6`, `Community 7`, `Community 10`, `Community 11`, `Community 12`, `Community 15`, `Community 19`, `Community 20`, `Community 21`, `Community 27`, `Community 31`, `Community 32`, `Community 42`, `Community 45`, `Community 47`, `Community 54`, `Community 63`?**
  _High betweenness centrality (0.042) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.07957393483709273 - nodes in this community are weakly interconnected._
