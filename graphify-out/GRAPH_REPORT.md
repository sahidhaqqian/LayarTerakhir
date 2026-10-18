# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1939 nodes · 6988 edges · 84 communities (59 shown, 25 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 329 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `b347745e`
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
- Community 72
- Community 73
- Community 74
- Community 75
- Community 76
- Community 77
- Community 78
- Community 79
- Community 80
- Community 81
- Community 82

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 113 edges
2. `HudController` - 84 edges
3. `FirstPersonController` - 66 edges
4. `LayarTerakhir.Prolog` - 65 edges
5. `AtmajaPlayTests` - 63 edges
6. `ShoulderCameraRig` - 60 edges
7. `AtmajaChaseSequence` - 56 edges
8. `LayarTerakhir.Player` - 56 edges
9. `InteractionInteractor` - 55 edges
10. `AtmajaPhoneView` - 55 edges

## Surprising Connections (you probably didn't know these)
- `DesignerReviewBuilder` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/DesignerReviewBuilder.cs → Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs
- `AtmajaBasementHorror` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/src/Core/CampaignState.cs
- `AtmajaBasementHorror` --references--> `AtmajaChaseSequence`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs
- `AtmajaBasementHorror` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs
- `AtmajaBasementHorror` --references--> `AtmajaUpperDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/src/Prolog/AtmajaUpperDirector.cs

## Import Cycles
- None detected.

## Communities (84 total, 25 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (12): GoldenLinuxBuild, Record, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core, LayarTerakhir.UI (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.12
Nodes (3): CompactKosPolish, KosVisualV2, FloorY

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (12): SceneAudioProfile, PresentationProfile, VoiceLine, VoiceBank, KosCorridorBuilder, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry (+4 more)

### Community 3 - "Community 3"
Cohesion: 0.10
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 4 - "Community 4"
Cohesion: 0.09
Nodes (20): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), dialogue_literals() (+12 more)

### Community 5 - "Community 5"
Cohesion: 0.09
Nodes (15): AtmajaChaseSequence, AwaitingBlitz, Campaign, FollowsPlayerInBasement, Hud, IsGateRun, IsPursuing, IsStunned (+7 more)

### Community 6 - "Community 6"
Cohesion: 0.09
Nodes (9): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen (+1 more)

### Community 7 - "Community 7"
Cohesion: 0.07
Nodes (8): box(), cylinder(), finish(), tube(), box(), cylinder(), material(), tube()

### Community 8 - "Community 8"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 10 - "Community 10"
Cohesion: 0.15
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 12 - "Community 12"
Cohesion: 0.11
Nodes (8): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsHidden, IsHunting, Phone

### Community 13 - "Community 13"
Cohesion: 0.08
Nodes (5): foley(), render(), write_pcm(), main(), snapshot()

### Community 14 - "Community 14"
Cohesion: 0.14
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 16 - "Community 16"
Cohesion: 0.11
Nodes (11): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+3 more)

### Community 17 - "Community 17"
Cohesion: 0.12
Nodes (7): PrologCompletionAudit, Campaign, Hud, Phone, Receipt, InteractionInteractor, CurrentTarget

### Community 18 - "Community 18"
Cohesion: 0.17
Nodes (5): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, AtmajaThreatPresentation, HasCharacterRig

### Community 19 - "Community 19"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 20 - "Community 20"
Cohesion: 0.16
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 21 - "Community 21"
Cohesion: 0.14
Nodes (3): AtmajaStairRevisionBuilder, BandVertex, KosRoomSurvey

### Community 22 - "Community 22"
Cohesion: 0.17
Nodes (4): DesignerWorldRevision, MelatiNeighborhoodBuilder, ARPhotoGlimmer, NeighborhoodRain

### Community 23 - "Community 23"
Cohesion: 0.16
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 24 - "Community 24"
Cohesion: 0.14
Nodes (22): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+14 more)

### Community 25 - "Community 25"
Cohesion: 0.21
Nodes (3): AtmajaSceneBuilder, Manifest, MaterialRecord

### Community 26 - "Community 26"
Cohesion: 0.17
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 28 - "Community 28"
Cohesion: 0.12
Nodes (13): MotorcycleRiderAlignment, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, Avatar, CinematicVisible, CurrentDistance (+5 more)

### Community 29 - "Community 29"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 30 - "Community 30"
Cohesion: 0.13
Nodes (10): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cylinder() (+2 more)

### Community 32 - "Community 32"
Cohesion: 0.17
Nodes (3): PlaytestReviewValidation, Record, KosRoomCorrections

### Community 33 - "Community 33"
Cohesion: 0.14
Nodes (7): IInteractable, InteractableId, MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 34 - "Community 34"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 35 - "Community 35"
Cohesion: 0.19
Nodes (4): GoldenHudBuilder, HudStyle, PhoneSkin, Rounded

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (14): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, GlitchStrength, HasGraphicsDevice (+6 more)

### Community 37 - "Community 37"
Cohesion: 0.17
Nodes (8): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath

### Community 38 - "Community 38"
Cohesion: 0.22
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 39 - "Community 39"
Cohesion: 0.13
Nodes (7): DoubleLeafGate, IsOpen, ExplorationCommentary, Zone, KosCorridorDirector, Busy, CanDepart

### Community 40 - "Community 40"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 43 - "Community 43"
Cohesion: 0.20
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 44 - "Community 44"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 45 - "Community 45"
Cohesion: 0.25
Nodes (4): PrologPresentationBuilder, Receipt, AnimatedRakaAvatar, Installing

### Community 46 - "Community 46"
Cohesion: 0.14
Nodes (6): AtmajaSceneValidation, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 49 - "Community 49"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 51 - "Community 51"
Cohesion: 0.19
Nodes (6): ScanVisualMode, AR, Normal, UV, SmartphoneController, Instance

### Community 54 - "Community 54"
Cohesion: 0.19
Nodes (6): CampaignData, PhoneDocument, UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 56 - "Community 56"
Cohesion: 0.20
Nodes (5): FoundationAuditCoroutineHost, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 57 - "Community 57"
Cohesion: 0.22
Nodes (6): FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 61 - "Community 61"
Cohesion: 0.29
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 63 - "Community 63"
Cohesion: 0.22
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 64 - "Community 64"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 65 - "Community 65"
Cohesion: 0.29
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 68 - "Community 68"
Cohesion: 0.20
Nodes (10): Run, Breaking, Complete, Ending, GateRun, Idle, Playback, Pursuit (+2 more)

### Community 69 - "Community 69"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 70 - "Community 70"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 72 - "Community 72"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 75 - "Community 75"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 76 - "Community 76"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 77 - "Community 77"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 78 - "Community 78"
Cohesion: 0.33
Nodes (6): EndingType, Bad, None, Normal, Secret, True

### Community 81 - "Community 81"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 82 - "Community 82"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **233 isolated node(s):** `FloorY`, `Campaign`, `Detection`, `Hud`, `InspectingLocker` (+228 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 417 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **25 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 36` to `Community 0`, `Community 1`, `Community 3`, `Community 6`, `Community 8`, `Community 9`, `Community 11`, `Community 14`, `Community 19`, `Community 20`, `Community 22`, `Community 25`, `Community 26`, `Community 30`, `Community 31`, `Community 38`, `Community 39`, `Community 43`, `Community 51`, `Community 53`, `Community 56`, `Community 59`, `Community 62`, `Community 66`?**
  _High betweenness centrality (0.315) - this node is a cross-community bridge._
- **What connects `FloorY`, `Campaign`, `Detection` to the rest of the system?**
  _233 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08366013071895424 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 3` to `Community 0`, `Community 1`, `Community 2`, `Community 5`, `Community 6`, `Community 9`, `Community 10`, `Community 11`, `Community 12`, `Community 17`, `Community 18`, `Community 21`, `Community 23`, `Community 25`, `Community 26`, `Community 31`, `Community 32`, `Community 36`, `Community 37`, `Community 38`, `Community 39`, `Community 41`, `Community 43`, `Community 44`, `Community 45`, `Community 47`, `Community 51`, `Community 52`, `Community 53`, `Community 55`, `Community 56`, `Community 57`, `Community 59`, `Community 66`, `Community 80`?**
  _High betweenness centrality (0.106) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.11748633879781421 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 6` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 5`, `Community 8`, `Community 11`, `Community 12`, `Community 17`, `Community 21`, `Community 23`, `Community 26`, `Community 31`, `Community 35`, `Community 36`, `Community 38`, `Community 43`, `Community 49`, `Community 50`, `Community 53`, `Community 55`, `Community 56`, `Community 61`, `Community 66`, `Community 67`, `Community 74`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.07957393483709273 - nodes in this community are weakly interconnected._
