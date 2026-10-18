# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 2048 nodes · 7246 edges · 82 communities (61 shown, 21 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 331 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `30d9e7e9`
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
- Community 78
- Community 79
- Community 80

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 91 edges
2. `HudController` - 81 edges
3. `AtmajaPlayTests` - 63 edges
4. `LayarTerakhir.Prolog` - 63 edges
5. `FirstPersonController` - 58 edges
6. `AtmajaChaseSequence` - 56 edges
7. `KamarKosPrologDirector` - 52 edges
8. `AtmajaBasementHorror` - 52 edges
9. `AtmajaPhoneView` - 51 edges
10. `LayarTerakhir.Player` - 50 edges

## Surprising Connections (you probably didn't know these)
- `InteractionInteractor` --references--> `IInteractable`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Player/InteractionInteractor.cs → Assets/_LayarTerakhir/src/Player/IInteractable.cs
- `AtmajaItem` --implements--> `IInteractable`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaItem.cs → Assets/_LayarTerakhir/src/Player/IInteractable.cs
- `KosInteractableItem` --implements--> `IInteractable`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/KosInteractableItem.cs → Assets/_LayarTerakhir/src/Player/IInteractable.cs
- `KosCorridorBuilder` --references--> `KosCorridorDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/DesignerWorldRevision.cs → Assets/_LayarTerakhir/src/Prolog/KosCorridorDirector.cs
- `MelatiNeighborhood` --references--> `KosCorridorDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/MelatiNeighborhood.cs → Assets/_LayarTerakhir/src/Prolog/KosCorridorDirector.cs

## Import Cycles
- None detected.

## Communities (82 total, 21 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.07
Nodes (15): GoldenLinuxBuild, Record, GoldenSceneValidation, Record, GoldenSceneTests, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio (+7 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (20): KosCorridorBuilder, IInteractable, InteractableId, DoubleLeafGate, IsOpen, HingedDoor, InteractableId, IsOpen (+12 more)

### Community 2 - "Community 2"
Cohesion: 0.12
Nodes (3): CompactKosPolish, KosVisualV2, FloorY

### Community 3 - "Community 3"
Cohesion: 0.08
Nodes (15): AtmajaChaseSequence, AwaitingBlitz, Campaign, FollowsPlayerInBasement, Hud, IsGateRun, IsPursuing, IsStunned (+7 more)

### Community 4 - "Community 4"
Cohesion: 0.06
Nodes (12): ball(), box(), cloth(), cylinder(), mat(), tube(), box(), cylinder() (+4 more)

### Community 5 - "Community 5"
Cohesion: 0.09
Nodes (15): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsHidden, IsHunting, Phone (+7 more)

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 7 - "Community 7"
Cohesion: 0.13
Nodes (6): Authoring, PrologCompletionBuilder, TempShaftProbe, MotorcycleRiderAlignment, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 8 - "Community 8"
Cohesion: 0.15
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 9 - "Community 9"
Cohesion: 0.11
Nodes (4): DreadoutVisualRevision, Report, SceneReport, PlaytestRepairBuilder

### Community 10 - "Community 10"
Cohesion: 0.12
Nodes (15): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+7 more)

### Community 11 - "Community 11"
Cohesion: 0.08
Nodes (5): foley(), render(), write_pcm(), main(), snapshot()

### Community 13 - "Community 13"
Cohesion: 0.11
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 14 - "Community 14"
Cohesion: 0.16
Nodes (3): AtmajaReviewDressing, KosVisualUpgrade, VisualAudit

### Community 16 - "Community 16"
Cohesion: 0.12
Nodes (5): MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 17 - "Community 17"
Cohesion: 0.11
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 18 - "Community 18"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 20 - "Community 20"
Cohesion: 0.12
Nodes (12): dialogue_literals(), export(), fetch(), main(), sha(), fetch(), main(), model() (+4 more)

### Community 21 - "Community 21"
Cohesion: 0.11
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 22 - "Community 22"
Cohesion: 0.19
Nodes (3): PhoneSkin, Rounded, UiWidgets

### Community 23 - "Community 23"
Cohesion: 0.12
Nodes (14): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), fetch() (+6 more)

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (12): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cylinder() (+4 more)

### Community 26 - "Community 26"
Cohesion: 0.12
Nodes (22): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+14 more)

### Community 27 - "Community 27"
Cohesion: 0.15
Nodes (8): InteractionInteractor, CurrentTarget, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 28 - "Community 28"
Cohesion: 0.16
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 29 - "Community 29"
Cohesion: 0.15
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 30 - "Community 30"
Cohesion: 0.16
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 31 - "Community 31"
Cohesion: 0.11
Nodes (12): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, Avatar, CinematicVisible, CurrentDistance, FirstPersonOverride (+4 more)

### Community 32 - "Community 32"
Cohesion: 0.14
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 33 - "Community 33"
Cohesion: 0.14
Nodes (7): PrologPresentationBuilder, Receipt, AnimatedRakaAvatar, Installing, AtmajaThreatPresentation, HasCharacterRig, ClockAmbience

### Community 34 - "Community 34"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 35 - "Community 35"
Cohesion: 0.20
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 36 - "Community 36"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 37 - "Community 37"
Cohesion: 0.12
Nodes (11): CampaignData, EndingType, Bad, None, Normal, Secret, True, UiFocus (+3 more)

### Community 38 - "Community 38"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 40 - "Community 40"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 41 - "Community 41"
Cohesion: 0.18
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 42 - "Community 42"
Cohesion: 0.11
Nodes (14): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, GlitchStrength, HasGraphicsDevice (+6 more)

### Community 43 - "Community 43"
Cohesion: 0.22
Nodes (3): SceneAudioProfile, PresentationProfile, GoldenSceneProduction

### Community 45 - "Community 45"
Cohesion: 0.17
Nodes (3): KosCorridorDirector, Busy, CanDepart

### Community 47 - "Community 47"
Cohesion: 0.19
Nodes (4): GoldenAudioTap, GoldenPlayerAudit, Receipt, FoundationAuditCoroutineHost

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 52 - "Community 52"
Cohesion: 0.16
Nodes (4): AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 53 - "Community 53"
Cohesion: 0.24
Nodes (4): KosRoomSurvey, PlaytestReviewValidation, Record, KosRoomCorrections

### Community 54 - "Community 54"
Cohesion: 0.17
Nodes (6): FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 55 - "Community 55"
Cohesion: 0.21
Nodes (6): ScanVisualMode, AR, Normal, UV, SmartphoneController, Instance

### Community 56 - "Community 56"
Cohesion: 0.19
Nodes (3): AtmajaSceneValidation, OpeningBaseline, Report

### Community 57 - "Community 57"
Cohesion: 0.29
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 60 - "Community 60"
Cohesion: 0.23
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 63 - "Community 63"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 64 - "Community 64"
Cohesion: 0.27
Nodes (4): VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 65 - "Community 65"
Cohesion: 0.20
Nodes (10): Run, Breaking, Complete, Ending, GateRun, Idle, Playback, Pursuit (+2 more)

### Community 66 - "Community 66"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 67 - "Community 67"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 68 - "Community 68"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 69 - "Community 69"
Cohesion: 0.33
Nodes (5): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric

### Community 70 - "Community 70"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 73 - "Community 73"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 74 - "Community 74"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 77 - "Community 77"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 79 - "Community 79"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **233 isolated node(s):** `InteractableId`, `InteractableId`, `IsOpen`, `InteractableId`, `InteractableId` (+228 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 439 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **21 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 42` to `Community 0`, `Community 2`, `Community 3`, `Community 6`, `Community 10`, `Community 13`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 24`, `Community 30`, `Community 32`, `Community 35`, `Community 38`, `Community 41`, `Community 47`, `Community 54`, `Community 55`, `Community 58`, `Community 60`, `Community 71`, `Community 80`?**
  _High betweenness centrality (0.272) - this node is a cross-community bridge._
- **What connects `InteractableId`, `InteractableId`, `IsOpen` to the rest of the system?**
  _233 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07205298013245033 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 10` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 5`, `Community 7`, `Community 8`, `Community 9`, `Community 13`, `Community 14`, `Community 18`, `Community 25`, `Community 27`, `Community 28`, `Community 33`, `Community 35`, `Community 38`, `Community 39`, `Community 41`, `Community 42`, `Community 43`, `Community 47`, `Community 49`, `Community 50`, `Community 53`, `Community 54`, `Community 55`, `Community 58`, `Community 60`, `Community 71`, `Community 80`?**
  _High betweenness centrality (0.081) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05333333333333334 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 13` to `Community 0`, `Community 2`, `Community 3`, `Community 5`, `Community 6`, `Community 10`, `Community 14`, `Community 19`, `Community 22`, `Community 27`, `Community 35`, `Community 38`, `Community 40`, `Community 41`, `Community 42`, `Community 43`, `Community 47`, `Community 51`, `Community 54`, `Community 71`, `Community 72`, `Community 75`?**
  _High betweenness centrality (0.048) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.11634056054997356 - nodes in this community are weakly interconnected._
