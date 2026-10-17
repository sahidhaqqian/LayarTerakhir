# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1485 nodes · 5019 edges · 60 communities (45 shown, 15 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 254 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `14008947`
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
- Community 48
- Community 49
- Community 50
- Community 51
- Community 52
- Community 53
- Community 54
- Community 55
- Community 56

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 77 edges
2. `HudController` - 72 edges
3. `KamarKosPrologDirector` - 51 edges
4. `FirstPersonController` - 50 edges
5. `AtmajaChaseSequence` - 46 edges
6. `AtmajaBasementHorror` - 45 edges
7. `LayarTerakhir.Prolog` - 43 edges
8. `PhoneDesktop` - 42 edges
9. `LayarTerakhir.Player` - 40 edges
10. `CampaignState` - 39 edges

## Surprising Connections (you probably didn't know these)
- `KosVisualCapture` --references--> `InteractionInteractor`  [EXTRACTED]
  Assets/_LayarTerakhir/Editor/KosVisualCapture.cs → Assets/_LayarTerakhir/Scripts/Player/InteractionInteractor.cs
- `ExplorationCommentary` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/ExplorationCommentary.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs
- `KosInteractableItem` --implements--> `IInteractable`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/KosInteractableItem.cs → Assets/_LayarTerakhir/Scripts/Player/IInteractable.cs
- `KosTravelItem` --references--> `KosCorridorDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/KosTravelItem.cs → Assets/_LayarTerakhir/Scripts/Prolog/KosCorridorDirector.cs
- `AtmajaBasementHorror` --references--> `AtmajaChaseSequence`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaChaseSequence.cs

## Import Cycles
- None detected.

## Communities (60 total, 15 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (19): GoldenLinuxBuild, Record, GoldenSceneValidation, Record, GoldenSceneTests, ScanVisualMode, AR, Normal (+11 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (20): AtmajaSceneTests, DesignerRevisionTests, FoundationAuditTests, CampaignData, CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint (+12 more)

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (13): KosCorridorBuilder, PlaytestReviewValidation, Record, PlaytestReviewTests, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, SceneAudioProfile (+5 more)

### Community 3 - "Community 3"
Cohesion: 0.08
Nodes (9): BloodWriting, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsHandsetNotification, IsModalOpen (+1 more)

### Community 4 - "Community 4"
Cohesion: 0.12
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 5 - "Community 5"
Cohesion: 0.09
Nodes (13): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), export() (+5 more)

### Community 6 - "Community 6"
Cohesion: 0.11
Nodes (5): AtmajaChaseSequence, Campaign, Hud, Phone, AtmajaSynth

### Community 7 - "Community 7"
Cohesion: 0.12
Nodes (12): AtmajaDirector, Busy, Campaign, CanBeginLive, HasSurvey, Hud, LiveConnected, Phone (+4 more)

### Community 8 - "Community 8"
Cohesion: 0.13
Nodes (4): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (23): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+15 more)

### Community 13 - "Community 13"
Cohesion: 0.09
Nodes (13): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+5 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (10): AmbientAudioLoop, LevelMultiplier, IInteractable, InteractableId, ClockAmbience, HingedDoor, InteractableId, IsOpen (+2 more)

### Community 15 - "Community 15"
Cohesion: 0.19
Nodes (4): KosVisualUpgrade, VisualAudit, KosInteractableItem, InteractableId

### Community 16 - "Community 16"
Cohesion: 0.14
Nodes (4): AtmajaBasementHorror, Campaign, Hud, Phone

### Community 17 - "Community 17"
Cohesion: 0.09
Nodes (4): box(), cylinder(), material(), tube()

### Community 18 - "Community 18"
Cohesion: 0.16
Nodes (3): KamarKosPrologDirector, PrologSceneFlow, CanContinue

### Community 19 - "Community 19"
Cohesion: 0.16
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 20 - "Community 20"
Cohesion: 0.18
Nodes (4): DesignerWorldRevision, KosRoomSurvey, TempShaftProbe, KosRoomCorrections

### Community 21 - "Community 21"
Cohesion: 0.14
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 22 - "Community 22"
Cohesion: 0.19
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 23 - "Community 23"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 25 - "Community 25"
Cohesion: 0.20
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 26 - "Community 26"
Cohesion: 0.23
Nodes (3): GoldenHudBuilder, HudStyle, UiWidgets

### Community 27 - "Community 27"
Cohesion: 0.13
Nodes (6): FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 30 - "Community 30"
Cohesion: 0.14
Nodes (10): ball(), box(), cloth(), cylinder(), mat(), tube(), cube(), cyl() (+2 more)

### Community 31 - "Community 31"
Cohesion: 0.16
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 32 - "Community 32"
Cohesion: 0.14
Nodes (3): foley(), render(), write_pcm()

### Community 33 - "Community 33"
Cohesion: 0.18
Nodes (4): PrologKosSceneBuilder, PrologMilestoneValidator, SmartphoneController, Instance

### Community 34 - "Community 34"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 36 - "Community 36"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 38 - "Community 38"
Cohesion: 0.13
Nodes (9): AtmajaPhoneView, Apps, FeedCamera, HasGraphicsDevice, Hud, IsCameraApp, LiveButton, Panel (+1 more)

### Community 39 - "Community 39"
Cohesion: 0.22
Nodes (4): FoundationAuditCoroutineHost, DevelopmentScreenshotCapture, InteractionInteractor, CurrentTarget

### Community 40 - "Community 40"
Cohesion: 0.22
Nodes (5): ShoulderCameraRig, Avatar, CinematicVisible, CurrentDistance, Riding

### Community 44 - "Community 44"
Cohesion: 0.23
Nodes (4): box(), cylinder(), finish(), tube()

### Community 45 - "Community 45"
Cohesion: 0.18
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 46 - "Community 46"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 47 - "Community 47"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 48 - "Community 48"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 49 - "Community 49"
Cohesion: 0.33
Nodes (5): ball(), box(), cylinder(), mat(), tube()

### Community 51 - "Community 51"
Cohesion: 0.25
Nodes (8): Mind, Done, Dormant, Emerge, Inspect, Patrol, Pursue, Suspicion

### Community 52 - "Community 52"
Cohesion: 0.25
Nodes (8): Run, Breaking, Ending, GateRun, Idle, Pursuit, Stunned, WindowStandoff

### Community 53 - "Community 53"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 54 - "Community 54"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 55 - "Community 55"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **186 isolated node(s):** `AR`, `Normal`, `UV`, `BackupSaveFilePath`, `Data` (+181 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 332 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **15 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaDirector` connect `Community 7` to `Community 0`, `Community 1`, `Community 2`, `Community 4`, `Community 6`, `Community 8`, `Community 12`, `Community 13`, `Community 14`, `Community 16`, `Community 19`, `Community 20`, `Community 22`, `Community 28`, `Community 34`, `Community 35`, `Community 39`, `Community 50`, `Community 56`, `Community 59`?**
  _High betweenness centrality (0.085) - this node is a cross-community bridge._
- **What connects `AR`, `Normal`, `UV` to the rest of the system?**
  _186 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07762916338840808 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 3` to `Community 0`, `Community 1`, `Community 33`, `Community 36`, `Community 38`, `Community 7`, `Community 39`, `Community 9`, `Community 11`, `Community 12`, `Community 14`, `Community 22`, `Community 26`, `Community 31`?**
  _High betweenness centrality (0.066) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.053613053613053616 - nodes in this community are weakly interconnected._
- **Why does `GameShell` connect `Community 21` to `Community 0`, `Community 3`, `Community 39`, `Community 9`, `Community 12`, `Community 45`, `Community 26`?**
  _High betweenness centrality (0.064) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.05874125874125874 - nodes in this community are weakly interconnected._
