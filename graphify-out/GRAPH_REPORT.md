# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1653 nodes · 5667 edges · 70 communities (49 shown, 21 thin omitted)
- Extraction: 96% EXTRACTED · 4% INFERRED · 0% AMBIGUOUS · INFERRED: 250 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `5c9edef0`
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
- Community 16
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
- Community 68
- Community 69

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 81 edges
2. `HudController` - 77 edges
3. `FirstPersonController` - 59 edges
4. `AtmajaChaseSequence` - 56 edges
5. `KamarKosPrologDirector` - 53 edges
6. `LayarTerakhir.Prolog` - 51 edges
7. `AtmajaBasementHorror` - 50 edges
8. `CampaignState` - 42 edges
9. `PhoneDesktop` - 42 edges
10. `AtmajaPlayTests` - 41 edges

## Surprising Connections (you probably didn't know these)
- `KosSceneAudio` --references--> `FirstPersonController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Audio/KosSceneAudio.cs → Assets/_LayarTerakhir/Scripts/Player/FirstPersonController.cs
- `GoldenPlayerAudit` --references--> `FirstPersonController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Diagnostics/GoldenPlayerAudit.cs → Assets/_LayarTerakhir/Scripts/Player/FirstPersonController.cs
- `ReviewPlayerAudit` --references--> `FirstPersonController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Diagnostics/ReviewPlayerAudit.cs → Assets/_LayarTerakhir/Scripts/Player/FirstPersonController.cs
- `AtmajaDirector` --references--> `FirstPersonController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs → Assets/_LayarTerakhir/Scripts/Player/FirstPersonController.cs
- `KosCorridorDirector` --references--> `FirstPersonController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/KosCorridorDirector.cs → Assets/_LayarTerakhir/Scripts/Player/FirstPersonController.cs

## Import Cycles
- None detected.

## Communities (70 total, 21 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (12): GoldenLinuxBuild, Record, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core, LayarTerakhir.UI (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (19): FoundationPlayAuditTests, FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld, ShoulderCameraRig (+11 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (19): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+11 more)

### Community 4 - "Community 4"
Cohesion: 0.08
Nodes (10): AtmajaChaseSequence, AwaitingBlitz, Campaign, Hud, IsGateRun, IsPursuing, IsStunned, Phone (+2 more)

### Community 5 - "Community 5"
Cohesion: 0.07
Nodes (11): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport, AtmajaHandycamPlayback (+3 more)

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 7 - "Community 7"
Cohesion: 0.13
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 8 - "Community 8"
Cohesion: 0.15
Nodes (4): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (23): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+15 more)

### Community 11 - "Community 11"
Cohesion: 0.11
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsHandsetNotification, IsModalOpen, HudPresentation

### Community 12 - "Community 12"
Cohesion: 0.08
Nodes (8): box(), cylinder(), finish(), tube(), box(), cylinder(), material(), tube()

### Community 13 - "Community 13"
Cohesion: 0.12
Nodes (12): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), fetch() (+4 more)

### Community 15 - "Community 15"
Cohesion: 0.13
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 16 - "Community 16"
Cohesion: 0.19
Nodes (3): GoldenHudBuilder, HudStyle, UiWidgets

### Community 17 - "Community 17"
Cohesion: 0.18
Nodes (3): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 18 - "Community 18"
Cohesion: 0.16
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 19 - "Community 19"
Cohesion: 0.11
Nodes (8): cloth(), ellipsoid(), finish(), mesh(), tube(), foley(), render(), write_pcm()

### Community 20 - "Community 20"
Cohesion: 0.08
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 21 - "Community 21"
Cohesion: 0.13
Nodes (7): GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, SceneAudioProfile, PresentationProfile, VoiceLine

### Community 22 - "Community 22"
Cohesion: 0.17
Nodes (8): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath

### Community 24 - "Community 24"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 25 - "Community 25"
Cohesion: 0.12
Nodes (4): DesignerRevisionTests, KosVisualV2Tests, DoubleLeafGate, IsOpen

### Community 26 - "Community 26"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 27 - "Community 27"
Cohesion: 0.11
Nodes (8): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsHidden, IsHunting, Phone

### Community 28 - "Community 28"
Cohesion: 0.16
Nodes (4): DesignerWorldRevision, KosRoomSurvey, VisualAudit, KosRoomCorrections

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 33 - "Community 33"
Cohesion: 0.15
Nodes (6): IInteractable, InteractableId, InteractionInteractor, CurrentTarget, AtmajaItem, InteractableId

### Community 34 - "Community 34"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 35 - "Community 35"
Cohesion: 0.12
Nodes (11): CampaignData, EndingType, Bad, None, Normal, Secret, True, UiFocus (+3 more)

### Community 36 - "Community 36"
Cohesion: 0.11
Nodes (11): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, FeedCamera, HasGraphicsDevice, Hud, IsCameraApp (+3 more)

### Community 37 - "Community 37"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 38 - "Community 38"
Cohesion: 0.14
Nodes (8): box(), cyl(), finish(), tube(), cube(), cyl(), finish(), lathe()

### Community 39 - "Community 39"
Cohesion: 0.15
Nodes (5): FoundationAuditCoroutineHost, DevelopmentScreenshotCapture, AtmajaThreatPresentation, HasCharacterRig, ClockAmbience

### Community 40 - "Community 40"
Cohesion: 0.17
Nodes (7): PlaytestReviewValidation, Record, KosCorridorDirector, Busy, CanDepart, KosTravelItem, InteractableId

### Community 41 - "Community 41"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 43 - "Community 43"
Cohesion: 0.15
Nodes (4): AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 44 - "Community 44"
Cohesion: 0.29
Nodes (5): GoldenSceneValidation, Record, Check, KosSceneValidation, Report

### Community 46 - "Community 46"
Cohesion: 0.21
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 50 - "Community 50"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 52 - "Community 52"
Cohesion: 0.20
Nodes (10): Run, Breaking, Complete, Ending, GateRun, Idle, Playback, Pursuit (+2 more)

### Community 53 - "Community 53"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 54 - "Community 54"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 55 - "Community 55"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 56 - "Community 56"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 57 - "Community 57"
Cohesion: 0.33
Nodes (5): ball(), box(), cylinder(), mat(), tube()

### Community 59 - "Community 59"
Cohesion: 0.39
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 61 - "Community 61"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 62 - "Community 62"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 64 - "Community 64"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **211 isolated node(s):** `IsCrouching`, `IsSliding`, `IsSprinting`, `MovementLocked`, `PhoneLookHeld` (+206 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 379 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **21 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaDirector` connect `Community 2` to `Community 0`, `Community 1`, `Community 3`, `Community 4`, `Community 5`, `Community 7`, `Community 8`, `Community 10`, `Community 11`, `Community 16`, `Community 17`, `Community 21`, `Community 22`, `Community 25`, `Community 26`, `Community 27`, `Community 28`, `Community 33`, `Community 36`, `Community 37`, `Community 39`, `Community 40`, `Community 49`, `Community 58`, `Community 63`, `Community 65`, `Community 69`?**
  _High betweenness centrality (0.082) - this node is a cross-community bridge._
- **What connects `IsCrouching`, `IsSliding`, `IsSprinting` to the rest of the system?**
  _211 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08418568056648308 - nodes in this community are weakly interconnected._
- **Why does `AtmajaChaseSequence` connect `Community 4` to `Community 0`, `Community 3`, `Community 5`, `Community 6`, `Community 39`, `Community 10`, `Community 14`, `Community 48`, `Community 17`, `Community 16`, `Community 52`, `Community 23`, `Community 58`, `Community 27`?**
  _High betweenness centrality (0.066) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.059018367961457395 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 11` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 4`, `Community 7`, `Community 9`, `Community 10`, `Community 16`, `Community 21`, `Community 26`, `Community 27`, `Community 36`, `Community 39`, `Community 41`, `Community 42`, `Community 44`, `Community 48`, `Community 51`, `Community 58`, `Community 60`, `Community 63`, `Community 67`?**
  _High betweenness centrality (0.043) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.08198757763975155 - nodes in this community are weakly interconnected._
