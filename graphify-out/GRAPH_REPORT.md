# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1529 nodes · 5086 edges · 74 communities (48 shown, 26 thin omitted)
- Extraction: 96% EXTRACTED · 4% INFERRED · 0% AMBIGUOUS · INFERRED: 217 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `85891161`
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
- Community 72
- Community 73

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 71 edges
2. `HudController` - 68 edges
3. `KamarKosPrologDirector` - 47 edges
4. `AtmajaChaseSequence` - 46 edges
5. `AtmajaBasementHorror` - 44 edges
6. `PhoneDesktop` - 43 edges
7. `FirstPersonController` - 42 edges
8. `ReviewPlayerAudit` - 39 edges
9. `LayarTerakhir.Player` - 38 edges
10. `FoundationPlayAuditTests` - 37 edges

## Surprising Connections (you probably didn't know these)
- `AtmajaSceneBuilder` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Editor/AtmajaSceneBuilder.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs
- `AtmajaBasementHorror` --references--> `AtmajaChaseSequence`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaChaseSequence.cs
- `AtmajaBasementHorror` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs
- `AtmajaBasementHorror` --references--> `AtmajaUpperDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaUpperDirector.cs
- `GoldenPlayerAudit` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Diagnostics/GoldenPlayerAudit.cs → Assets/_LayarTerakhir/Scripts/UI/HudController.cs

## Import Cycles
- None detected.

## Communities (74 total, 26 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (12): GoldenLinuxBuild, Record, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core, LayarTerakhir.UI (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (19): CampaignData, EndingType, Bad, None, Normal, Secret, True, DevelopmentScreenshotCapture (+11 more)

### Community 2 - "Community 2"
Cohesion: 0.10
Nodes (12): AtmajaDirector, Busy, Campaign, CanBeginLive, HasSurvey, Hud, LiveConnected, Phone (+4 more)

### Community 3 - "Community 3"
Cohesion: 0.12
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 4 - "Community 4"
Cohesion: 0.12
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 7 - "Community 7"
Cohesion: 0.16
Nodes (4): KosVisualUpgrade, VisualAudit, KosInteractableItem, InteractableId

### Community 8 - "Community 8"
Cohesion: 0.14
Nodes (5): AtmajaChaseSequence, Campaign, Hud, Phone, AtmajaSynth

### Community 9 - "Community 9"
Cohesion: 0.11
Nodes (11): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+3 more)

### Community 10 - "Community 10"
Cohesion: 0.11
Nodes (5): asset(), download(), load_meta(), main(), snapshot()

### Community 11 - "Community 11"
Cohesion: 0.16
Nodes (3): AtmajaSceneBuilder, Manifest, MaterialRecord

### Community 12 - "Community 12"
Cohesion: 0.10
Nodes (9): GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, AmbientAudioLoop, LevelMultiplier, SceneAudioProfile, PresentationProfile (+1 more)

### Community 13 - "Community 13"
Cohesion: 0.14
Nodes (4): AtmajaBasementHorror, Campaign, Hud, Phone

### Community 14 - "Community 14"
Cohesion: 0.09
Nodes (8): BloodWriting, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsHandsetNotification, IsModalOpen

### Community 15 - "Community 15"
Cohesion: 0.16
Nodes (4): KosCorridorBuilder, KosCorridorDirector, Busy, CanDepart

### Community 16 - "Community 16"
Cohesion: 0.15
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 19 - "Community 19"
Cohesion: 0.18
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 20 - "Community 20"
Cohesion: 0.10
Nodes (4): box(), cylinder(), material(), tube()

### Community 21 - "Community 21"
Cohesion: 0.16
Nodes (9): asset(), download(), asset(), fetch(), fetch(), freesound(), polyhaven(), record() (+1 more)

### Community 22 - "Community 22"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 23 - "Community 23"
Cohesion: 0.16
Nodes (8): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 28 - "Community 28"
Cohesion: 0.15
Nodes (4): PrologKosSceneBuilder, PrologMilestoneValidator, SmartphoneController, Instance

### Community 29 - "Community 29"
Cohesion: 0.10
Nodes (21): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+13 more)

### Community 30 - "Community 30"
Cohesion: 0.14
Nodes (10): ball(), box(), cloth(), cylinder(), mat(), tube(), cube(), cyl() (+2 more)

### Community 31 - "Community 31"
Cohesion: 0.14
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 32 - "Community 32"
Cohesion: 0.18
Nodes (5): KosRoomSurvey, PlaytestReviewValidation, Record, TempShaftProbe, KosRoomCorrections

### Community 33 - "Community 33"
Cohesion: 0.14
Nodes (5): ShoulderCameraRig, Avatar, CinematicVisible, CurrentDistance, Riding

### Community 34 - "Community 34"
Cohesion: 0.14
Nodes (8): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube()

### Community 35 - "Community 35"
Cohesion: 0.14
Nodes (3): foley(), render(), write_pcm()

### Community 39 - "Community 39"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 41 - "Community 41"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 42 - "Community 42"
Cohesion: 0.17
Nodes (6): FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 45 - "Community 45"
Cohesion: 0.13
Nodes (9): AtmajaPhoneView, Apps, FeedCamera, HasGraphicsDevice, Hud, IsCameraApp, LiveButton, Panel (+1 more)

### Community 47 - "Community 47"
Cohesion: 0.18
Nodes (4): IInteractable, InteractableId, InteractionInteractor, CurrentTarget

### Community 50 - "Community 50"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 51 - "Community 51"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 52 - "Community 52"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 55 - "Community 55"
Cohesion: 0.33
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 57 - "Community 57"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 58 - "Community 58"
Cohesion: 0.33
Nodes (5): ball(), box(), cylinder(), mat(), tube()

### Community 59 - "Community 59"
Cohesion: 0.29
Nodes (3): AtmajaSceneValidation, Report, AtmajaSceneTests

### Community 60 - "Community 60"
Cohesion: 0.25
Nodes (8): Mind, Done, Dormant, Emerge, Inspect, Patrol, Pursue, Suspicion

### Community 61 - "Community 61"
Cohesion: 0.25
Nodes (8): Run, Breaking, Ending, GateRun, Idle, Pursuit, Stunned, WindowStandoff

### Community 64 - "Community 64"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 66 - "Community 66"
Cohesion: 0.40
Nodes (3): FoundationAuditCoroutineHost, DoubleLeafGate, IsOpen

### Community 67 - "Community 67"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 68 - "Community 68"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 72 - "Community 72"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **187 isolated node(s):** `Bad`, `None`, `Normal`, `Secret`, `True` (+182 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 345 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **26 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaDirector` connect `Community 2` to `Community 0`, `Community 3`, `Community 5`, `Community 8`, `Community 11`, `Community 13`, `Community 15`, `Community 17`, `Community 18`, `Community 19`, `Community 23`, `Community 26`, `Community 32`, `Community 39`, `Community 46`, `Community 53`, `Community 63`, `Community 66`, `Community 69`, `Community 70`?**
  _High betweenness centrality (0.083) - this node is a cross-community bridge._
- **What connects `Bad`, `None`, `Normal` to the rest of the system?**
  _187 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08489062726350861 - nodes in this community are weakly interconnected._
- **Why does `AtmajaBasementHorror` connect `Community 13` to `Community 0`, `Community 32`, `Community 2`, `Community 66`, `Community 5`, `Community 8`, `Community 44`, `Community 46`, `Community 18`, `Community 19`, `Community 53`, `Community 60`, `Community 63`?**
  _High betweenness centrality (0.052) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.0593990216631726 - nodes in this community are weakly interconnected._
- **Why does `AtmajaChaseSequence` connect `Community 8` to `Community 0`, `Community 2`, `Community 66`, `Community 4`, `Community 5`, `Community 7`, `Community 44`, `Community 13`, `Community 46`, `Community 18`, `Community 54`, `Community 61`, `Community 63`?**
  _High betweenness centrality (0.045) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.10453283996299723 - nodes in this community are weakly interconnected._
