# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1996 nodes · 7060 edges · 79 communities (57 shown, 22 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 331 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `6ffb227a`
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
- Community 71
- Community 72
- Community 73
- Community 74
- Community 75
- Community 76
- Community 77

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 95 edges
2. `HudController` - 83 edges
3. `LayarTerakhir.Prolog` - 65 edges
4. `AtmajaPlayTests` - 63 edges
5. `FirstPersonController` - 62 edges
6. `AtmajaChaseSequence` - 56 edges
7. `AtmajaBasementHorror` - 52 edges
8. `ShoulderCameraRig` - 52 edges
9. `KamarKosPrologDirector` - 52 edges
10. `AtmajaPhoneView` - 51 edges

## Surprising Connections (you probably didn't know these)
- `AtmajaPhoneView` --references--> `PhoneDesktop`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaPhoneView.cs → Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs
- `PhoneDesktop` --references--> `SmartphoneController`  [EXTRACTED]
  Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs → Assets/_LayarTerakhir/src/Smartphone/SmartphoneController.cs
- `KosSceneAudio` --references--> `AmbientAudioLoop`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Audio/KosSceneAudio.cs → Assets/_LayarTerakhir/src/Audio/AmbientAudioLoop.cs
- `KosSceneAudio` --references--> `SceneAudioProfile`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Audio/KosSceneAudio.cs → Assets/_LayarTerakhir/src/Audio/SceneAudioProfile.cs
- `PresentationProfile` --references--> `SceneAudioProfile`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Core/PresentationProfile.cs → Assets/_LayarTerakhir/src/Audio/SceneAudioProfile.cs

## Import Cycles
- None detected.

## Communities (79 total, 22 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (12): GoldenLinuxBuild, Record, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core, LayarTerakhir.UI (+4 more)

### Community 2 - "Community 2"
Cohesion: 0.12
Nodes (3): CompactKosPolish, KosVisualV2, FloorY

### Community 3 - "Community 3"
Cohesion: 0.08
Nodes (21): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), dialogue_literals() (+13 more)

### Community 4 - "Community 4"
Cohesion: 0.10
Nodes (12): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceBank (+4 more)

### Community 5 - "Community 5"
Cohesion: 0.10
Nodes (9): Authoring, PrologCompletionBuilder, PrologPresentationBuilder, Receipt, MotorcycleRiderAlignment, AtmajaEvidenceAtmosphere, AtmajaThreatPresentation, HasCharacterRig (+1 more)

### Community 6 - "Community 6"
Cohesion: 0.06
Nodes (10): box(), cyl(), finish(), tube(), export(), meshes(), box(), cylinder() (+2 more)

### Community 7 - "Community 7"
Cohesion: 0.12
Nodes (7): FoundationPlayAuditTests, FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 9 - "Community 9"
Cohesion: 0.13
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 10 - "Community 10"
Cohesion: 0.14
Nodes (4): KosVisualUpgrade, VisualAudit, TempShaftProbe, KosRoomCorrections

### Community 11 - "Community 11"
Cohesion: 0.13
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 12 - "Community 12"
Cohesion: 0.09
Nodes (9): AmbientAudioLoop, LevelMultiplier, SceneAudioProfile, GoldenSceneProduction, GoldenSceneValidation, Record, GoldenSceneTests, PlaytestRevisionBuilder (+1 more)

### Community 13 - "Community 13"
Cohesion: 0.12
Nodes (10): AtmajaChaseSequence, AwaitingBlitz, Campaign, FollowsPlayerInBasement, Hud, IsGateRun, IsPursuing, IsStunned (+2 more)

### Community 14 - "Community 14"
Cohesion: 0.10
Nodes (8): PresentationProfile, DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 15 - "Community 15"
Cohesion: 0.11
Nodes (8): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsHidden, IsHunting, Phone

### Community 16 - "Community 16"
Cohesion: 0.14
Nodes (6): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision, DoubleLeafGate, IsOpen

### Community 17 - "Community 17"
Cohesion: 0.12
Nodes (8): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath

### Community 18 - "Community 18"
Cohesion: 0.11
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (5): foley(), render(), write_pcm(), main(), snapshot()

### Community 20 - "Community 20"
Cohesion: 0.09
Nodes (9): IInteractable, InteractableId, HingedDoor, InteractableId, IsOpen, KosTravelItem, InteractableId, MelatiShopkeeper (+1 more)

### Community 21 - "Community 21"
Cohesion: 0.13
Nodes (4): MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report

### Community 23 - "Community 23"
Cohesion: 0.11
Nodes (14): AnimatedRakaAvatar, Installing, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, Avatar, CinematicVisible (+6 more)

### Community 24 - "Community 24"
Cohesion: 0.14
Nodes (22): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+14 more)

### Community 25 - "Community 25"
Cohesion: 0.08
Nodes (25): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+17 more)

### Community 27 - "Community 27"
Cohesion: 0.18
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 28 - "Community 28"
Cohesion: 0.10
Nodes (13): CampaignData, EndingType, Bad, None, Normal, Secret, True, PhoneDocument (+5 more)

### Community 29 - "Community 29"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 31 - "Community 31"
Cohesion: 0.18
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 32 - "Community 32"
Cohesion: 0.13
Nodes (10): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cylinder() (+2 more)

### Community 33 - "Community 33"
Cohesion: 0.13
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 34 - "Community 34"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (14): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, GlitchStrength, HasGraphicsDevice (+6 more)

### Community 40 - "Community 40"
Cohesion: 0.21
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 41 - "Community 41"
Cohesion: 0.14
Nodes (7): FoundationAuditCoroutineHost, InteractionInteractor, CurrentTarget, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 44 - "Community 44"
Cohesion: 0.20
Nodes (5): AtmajaSceneValidation, Report, Check, KosSceneValidation, Report

### Community 45 - "Community 45"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 47 - "Community 47"
Cohesion: 0.15
Nodes (4): AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 48 - "Community 48"
Cohesion: 0.12
Nodes (5): BloodWriting, PlaytestReviewValidation, Record, AtmajaWorldText, ClockAmbience

### Community 49 - "Community 49"
Cohesion: 0.15
Nodes (3): KosCorridorDirector, Busy, CanDepart

### Community 50 - "Community 50"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 51 - "Community 51"
Cohesion: 0.21
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 52 - "Community 52"
Cohesion: 0.19
Nodes (6): ScanVisualMode, AR, Normal, UV, SmartphoneController, Instance

### Community 53 - "Community 53"
Cohesion: 0.22
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 60 - "Community 60"
Cohesion: 0.21
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 62 - "Community 62"
Cohesion: 0.22
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 63 - "Community 63"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 65 - "Community 65"
Cohesion: 0.20
Nodes (10): Run, Breaking, Complete, Ending, GateRun, Idle, Playback, Pursuit (+2 more)

### Community 66 - "Community 66"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 67 - "Community 67"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 68 - "Community 68"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 69 - "Community 69"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 70 - "Community 70"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 71 - "Community 71"
Cohesion: 0.29
Nodes (3): MelatiShop, InteractableId, IsOpen

### Community 72 - "Community 72"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 74 - "Community 74"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 75 - "Community 75"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 77 - "Community 77"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **233 isolated node(s):** `CallActive`, `ContractReceived`, `CurrentApp`, `DialedContact`, `IsCalling` (+228 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 436 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **22 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 36` to `Community 0`, `Community 1`, `Community 2`, `Community 7`, `Community 9`, `Community 11`, `Community 12`, `Community 14`, `Community 16`, `Community 21`, `Community 22`, `Community 30`, `Community 32`, `Community 33`, `Community 39`, `Community 40`, `Community 41`, `Community 52`, `Community 53`, `Community 59`, `Community 73`?**
  _High betweenness centrality (0.287) - this node is a cross-community bridge._
- **What connects `CallActive`, `ContractReceived`, `CurrentApp` to the rest of the system?**
  _233 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07585470085470085 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 9` to `Community 0`, `Community 1`, `Community 2`, `Community 4`, `Community 5`, `Community 7`, `Community 8`, `Community 10`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 17`, `Community 20`, `Community 22`, `Community 25`, `Community 27`, `Community 30`, `Community 31`, `Community 35`, `Community 36`, `Community 37`, `Community 40`, `Community 41`, `Community 44`, `Community 45`, `Community 46`, `Community 48`, `Community 52`, `Community 53`, `Community 59`, `Community 73`?**
  _High betweenness centrality (0.141) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.10558558558558559 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 30` to `Community 0`, `Community 1`, `Community 2`, `Community 7`, `Community 8`, `Community 9`, `Community 12`, `Community 13`, `Community 14`, `Community 15`, `Community 17`, `Community 22`, `Community 26`, `Community 27`, `Community 31`, `Community 36`, `Community 39`, `Community 40`, `Community 41`, `Community 44`, `Community 48`, `Community 50`, `Community 53`, `Community 55`, `Community 64`?**
  _High betweenness centrality (0.074) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.11751412429378531 - nodes in this community are weakly interconnected._
