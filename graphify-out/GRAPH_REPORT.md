# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 2221 nodes · 7876 edges · 90 communities (59 shown, 31 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 368 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `cdd0eaef`
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
- Community 81
- Community 82
- Community 83
- Community 85
- Community 86
- Community 87
- Community 88

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 96 edges
2. `HudController` - 82 edges
3. `AtmajaPlayTests` - 78 edges
4. `AtmajaPhoneView` - 73 edges
5. `FirstPersonController` - 71 edges
6. `ShoulderCameraRig` - 68 edges
7. `AtmajaChaseSequence` - 65 edges
8. `LayarTerakhir.Prolog` - 64 edges
9. `LayarTerakhir.Player` - 60 edges
10. `AtmajaBasementHorror` - 56 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `InteractionInteractor` --references--> `IInteractable`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Player/InteractionInteractor.cs → Assets/_LayarTerakhir/src/Player/IInteractable.cs

## Import Cycles
- None detected.

## Communities (90 total, 31 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.07
Nodes (14): GoldenLinuxBuild, Record, KosRoomSurvey, TempShaftProbe, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests (+6 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (21): KosCorridorBuilder, IInteractable, InteractableId, HingedDoor, InteractableId, IsOpen, KosCorridorDirector, Busy (+13 more)

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (16): DreadoutVisualRevision, Report, SceneReport, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+8 more)

### Community 3 - "Community 3"
Cohesion: 0.09
Nodes (15): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+7 more)

### Community 4 - "Community 4"
Cohesion: 0.05
Nodes (12): box(), cyl(), finish(), tube(), export(), meshes(), box(), cylinder() (+4 more)

### Community 5 - "Community 5"
Cohesion: 0.11
Nodes (5): KosVisualUpgrade, VisualAudit, PlaytestReviewValidation, Record, KosRoomCorrections

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (10): asset(), download(), load_meta(), main(), prepare(), foley(), render(), write_pcm() (+2 more)

### Community 8 - "Community 8"
Cohesion: 0.09
Nodes (15): InteractionInteractor, CurrentTarget, AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud (+7 more)

### Community 9 - "Community 9"
Cohesion: 0.13
Nodes (8): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 10 - "Community 10"
Cohesion: 0.09
Nodes (18): dialogue_literals(), export(), fetch(), main(), sha(), fetch(), main(), model() (+10 more)

### Community 11 - "Community 11"
Cohesion: 0.12
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 13 - "Community 13"
Cohesion: 0.09
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 14 - "Community 14"
Cohesion: 0.08
Nodes (24): solve(), evaluate(), jacobian(), residual(), add_phone(), apparition(), bind(), bind_nyonya_shears() (+16 more)

### Community 15 - "Community 15"
Cohesion: 0.10
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 17 - "Community 17"
Cohesion: 0.11
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 20 - "Community 20"
Cohesion: 0.12
Nodes (5): KamarKosPrologDirector, KosInteractableItem, InteractableId, PrologSceneFlow, CanContinue

### Community 21 - "Community 21"
Cohesion: 0.16
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (18): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+10 more)

### Community 23 - "Community 23"
Cohesion: 0.08
Nodes (20): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, GlitchStrength, HasGraphicsDevice (+12 more)

### Community 24 - "Community 24"
Cohesion: 0.13
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 25 - "Community 25"
Cohesion: 0.14
Nodes (6): MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer, NeighborhoodRain

### Community 26 - "Community 26"
Cohesion: 0.15
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 27 - "Community 27"
Cohesion: 0.14
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 28 - "Community 28"
Cohesion: 0.14
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 29 - "Community 29"
Cohesion: 0.14
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 30 - "Community 30"
Cohesion: 0.18
Nodes (3): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 31 - "Community 31"
Cohesion: 0.16
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 33 - "Community 33"
Cohesion: 0.10
Nodes (15): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+7 more)

### Community 34 - "Community 34"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 35 - "Community 35"
Cohesion: 0.18
Nodes (4): PresentationProfile, GoldenHudBuilder, HudPresentation, HudStyle

### Community 37 - "Community 37"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 38 - "Community 38"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 39 - "Community 39"
Cohesion: 0.16
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 40 - "Community 40"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 41 - "Community 41"
Cohesion: 0.19
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 43 - "Community 43"
Cohesion: 0.14
Nodes (5): asset(), download(), asset(), fetch(), prepare()

### Community 44 - "Community 44"
Cohesion: 0.12
Nodes (6): AmbientAudioLoop, LevelMultiplier, FoundationAuditCoroutineHost, ClockAmbience, ExplorationCommentary, Zone

### Community 46 - "Community 46"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 48 - "Community 48"
Cohesion: 0.15
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 50 - "Community 50"
Cohesion: 0.16
Nodes (6): CampaignData, PhoneDocument, UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 51 - "Community 51"
Cohesion: 0.15
Nodes (8): FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld, PlanarVelocity

### Community 52 - "Community 52"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 55 - "Community 55"
Cohesion: 0.26
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 58 - "Community 58"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 62 - "Community 62"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 66 - "Community 66"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 67 - "Community 67"
Cohesion: 0.27
Nodes (4): VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 69 - "Community 69"
Cohesion: 0.22
Nodes (4): AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 70 - "Community 70"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 71 - "Community 71"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 72 - "Community 72"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 74 - "Community 74"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 75 - "Community 75"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 80 - "Community 80"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 81 - "Community 81"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 82 - "Community 82"
Cohesion: 0.33
Nodes (6): EndingType, Bad, None, Normal, Secret, True

### Community 86 - "Community 86"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 88 - "Community 88"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **255 isolated node(s):** `InteractableId`, `InteractableId`, `InteractableId`, `Busy`, `Reached` (+250 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 484 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **31 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 23` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 5`, `Community 8`, `Community 11`, `Community 12`, `Community 15`, `Community 19`, `Community 20`, `Community 21`, `Community 22`, `Community 24`, `Community 25`, `Community 26`, `Community 27`, `Community 31`, `Community 35`, `Community 36`, `Community 39`, `Community 40`, `Community 44`, `Community 45`, `Community 51`, `Community 53`, `Community 56`, `Community 73`, `Community 74`?**
  _High betweenness centrality (0.262) - this node is a cross-community bridge._
- **What connects `InteractableId`, `InteractableId`, `InteractableId` to the rest of the system?**
  _255 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07316866140395552 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 58` to `Community 0`, `Community 64`, `Community 5`, `Community 27`, `Community 44`, `Community 77`, `Community 51`, `Community 54`, `Community 22`, `Community 56`, `Community 59`?**
  _High betweenness centrality (0.071) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05225576111652061 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 3` to `Community 0`, `Community 1`, `Community 2`, `Community 5`, `Community 8`, `Community 9`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 17`, `Community 21`, `Community 23`, `Community 26`, `Community 27`, `Community 30`, `Community 31`, `Community 32`, `Community 36`, `Community 39`, `Community 40`, `Community 44`, `Community 49`, `Community 51`, `Community 53`, `Community 54`, `Community 56`, `Community 60`, `Community 61`, `Community 69`, `Community 73`?**
  _High betweenness centrality (0.068) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.0547945205479452 - nodes in this community are weakly interconnected._
