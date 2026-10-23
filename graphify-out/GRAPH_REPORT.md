# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 2585 nodes · 9259 edges · 101 communities (72 shown, 29 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 534 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `1294c949`
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
- Community 84
- Community 85
- Community 86
- Community 87
- Community 88
- Community 89
- Community 90
- Community 91
- Community 92
- Community 93
- Community 94
- Community 96
- Community 97
- Community 98
- Community 100

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 100 edges
2. `AtmajaPhoneView` - 88 edges
3. `Phase1Director` - 86 edges
4. `HudController` - 84 edges
5. `ShoulderCameraRig` - 83 edges
6. `AtmajaPlayTests` - 78 edges
7. `FirstPersonController` - 76 edges
8. `LayarTerakhir.Prolog` - 72 edges
9. `LayarTerakhir.Player` - 70 edges
10. `AtmajaChaseSequence` - 65 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase1SceneBuilder` --references--> `Phase1Director`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/Phase1AdditionalDressing.cs → Assets/_LayarTerakhir/src/Phase1/Phase1Director.cs

## Import Cycles
- None detected.

## Communities (101 total, 29 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.07
Nodes (12): Phase1Content, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1, LayarTerakhir.Core, LayarTerakhir.UI (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (33): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), dialogue_literals() (+25 more)

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (13): DreadoutVisualRevision, Report, SceneReport, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+5 more)

### Community 3 - "Community 3"
Cohesion: 0.08
Nodes (12): PresentationProfile, GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 4 - "Community 4"
Cohesion: 0.12
Nodes (3): CompactKosPolish, KosVisualV2, FloorY

### Community 5 - "Community 5"
Cohesion: 0.08
Nodes (13): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+5 more)

### Community 6 - "Community 6"
Cohesion: 0.05
Nodes (14): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+6 more)

### Community 7 - "Community 7"
Cohesion: 0.12
Nodes (8): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 8 - "Community 8"
Cohesion: 0.09
Nodes (8): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Phase1SceneBuilder, ARPhotoGlimmer, AtmajaWorldText

### Community 10 - "Community 10"
Cohesion: 0.14
Nodes (7): GoldenLinuxBuild, Record, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1Anchor

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (23): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, FocusHandsVisible (+15 more)

### Community 13 - "Community 13"
Cohesion: 0.14
Nodes (3): PhoneSkin, Rounded, UiWidgets

### Community 14 - "Community 14"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 15 - "Community 15"
Cohesion: 0.08
Nodes (15): GraphicsReferenceAudit, Receipt, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked (+7 more)

### Community 17 - "Community 17"
Cohesion: 0.11
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 18 - "Community 18"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 19 - "Community 19"
Cohesion: 0.12
Nodes (15): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+7 more)

### Community 20 - "Community 20"
Cohesion: 0.09
Nodes (3): foley(), main(), snapshot()

### Community 21 - "Community 21"
Cohesion: 0.11
Nodes (10): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+2 more)

### Community 22 - "Community 22"
Cohesion: 0.13
Nodes (8): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, HingedDoor, InteractableId, IsOpen

### Community 23 - "Community 23"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 24 - "Community 24"
Cohesion: 0.08
Nodes (18): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+10 more)

### Community 25 - "Community 25"
Cohesion: 0.10
Nodes (4): DesignerRevisionTests, GoldenSceneTests, KosVisualV2Tests, PlayerFeelTests

### Community 26 - "Community 26"
Cohesion: 0.09
Nodes (17): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+9 more)

### Community 28 - "Community 28"
Cohesion: 0.11
Nodes (15): Phase1Balance, Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti (+7 more)

### Community 29 - "Community 29"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 31 - "Community 31"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 32 - "Community 32"
Cohesion: 0.16
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 33 - "Community 33"
Cohesion: 0.15
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 35 - "Community 35"
Cohesion: 0.10
Nodes (19): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+11 more)

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 37 - "Community 37"
Cohesion: 0.13
Nodes (5): GoldenSceneValidation, Record, KosRoomSurvey, TempShaftProbe, KosRoomCorrections

### Community 39 - "Community 39"
Cohesion: 0.12
Nodes (6): CampaignData, UiFocus, BlocksMovement, BlocksWorld, HardModal, Phase1Ids

### Community 40 - "Community 40"
Cohesion: 0.09
Nodes (4): CampaignRouting, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests

### Community 41 - "Community 41"
Cohesion: 0.17
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 42 - "Community 42"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 43 - "Community 43"
Cohesion: 0.12
Nodes (11): AmbientAudioLoop, LevelMultiplier, FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps (+3 more)

### Community 45 - "Community 45"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 46 - "Community 46"
Cohesion: 0.21
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 47 - "Community 47"
Cohesion: 0.16
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 48 - "Community 48"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 49 - "Community 49"
Cohesion: 0.14
Nodes (9): ScanVisualMode, AR, Normal, UV, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+1 more)

### Community 50 - "Community 50"
Cohesion: 0.24
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 51 - "Community 51"
Cohesion: 0.20
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 52 - "Community 52"
Cohesion: 0.18
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 53 - "Community 53"
Cohesion: 0.14
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 56 - "Community 56"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 58 - "Community 58"
Cohesion: 0.18
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 59 - "Community 59"
Cohesion: 0.18
Nodes (7): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, InteractionInteractor, ContextResolver, CurrentTarget

### Community 60 - "Community 60"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 61 - "Community 61"
Cohesion: 0.19
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 66 - "Community 66"
Cohesion: 0.35
Nodes (3): Check, KosSceneValidation, Report

### Community 68 - "Community 68"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 69 - "Community 69"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 70 - "Community 70"
Cohesion: 0.23
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 72 - "Community 72"
Cohesion: 0.23
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 74 - "Community 74"
Cohesion: 0.20
Nodes (4): FoundationAuditCoroutineHost, ClockAmbience, MelatiShopkeeper, InteractableId

### Community 77 - "Community 77"
Cohesion: 0.22
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 78 - "Community 78"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 80 - "Community 80"
Cohesion: 0.27
Nodes (8): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse, Phase1MoneyReceipt

### Community 82 - "Community 82"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 83 - "Community 83"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 84 - "Community 84"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 85 - "Community 85"
Cohesion: 0.25
Nodes (4): AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 87 - "Community 87"
Cohesion: 0.32
Nodes (3): PlaytestReviewValidation, Record, PlaytestReviewTests

### Community 89 - "Community 89"
Cohesion: 0.29
Nodes (3): MelatiShop, InteractableId, IsOpen

### Community 91 - "Community 91"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 92 - "Community 92"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 93 - "Community 93"
Cohesion: 0.33
Nodes (6): EndingType, Bad, None, Normal, Secret, True

### Community 96 - "Community 96"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 97 - "Community 97"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **316 isolated node(s):** `CameraLocked`, `CameraStatus`, `CanBeginLive`, `Viewers`, `Apps` (+311 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 571 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **29 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 12` to `Community 0`, `Community 3`, `Community 4`, `Community 5`, `Community 8`, `Community 9`, `Community 10`, `Community 11`, `Community 13`, `Community 14`, `Community 15`, `Community 17`, `Community 19`, `Community 24`, `Community 27`, `Community 28`, `Community 29`, `Community 30`, `Community 33`, `Community 34`, `Community 35`, `Community 36`, `Community 37`, `Community 38`, `Community 42`, `Community 43`, `Community 46`, `Community 47`, `Community 48`, `Community 51`, `Community 66`, `Community 70`, `Community 71`, `Community 74`, `Community 76`, `Community 95`, `Community 98`?**
  _High betweenness centrality (0.268) - this node is a cross-community bridge._
- **What connects `CameraLocked`, `CameraStatus`, `CanBeginLive` to the rest of the system?**
  _316 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.06940480334145492 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 19` to `Community 0`, `Community 2`, `Community 3`, `Community 4`, `Community 5`, `Community 7`, `Community 10`, `Community 11`, `Community 12`, `Community 13`, `Community 15`, `Community 17`, `Community 21`, `Community 22`, `Community 25`, `Community 29`, `Community 32`, `Community 33`, `Community 37`, `Community 41`, `Community 42`, `Community 43`, `Community 47`, `Community 48`, `Community 49`, `Community 50`, `Community 55`, `Community 56`, `Community 64`, `Community 70`, `Community 71`, `Community 74`, `Community 85`, `Community 87`, `Community 98`?**
  _High betweenness centrality (0.073) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05906553041434029 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 43` to `Community 0`, `Community 37`, `Community 71`, `Community 74`, `Community 10`, `Community 15`, `Community 79`, `Community 53`, `Community 24`, `Community 25`?**
  _High betweenness centrality (0.065) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.05926251097453907 - nodes in this community are weakly interconnected._
