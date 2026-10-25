# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 3949 nodes · 14307 edges · 126 communities (101 shown, 25 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1028 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `ba029cd4`
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
- Community 81
- Community 82
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
- Community 95
- Community 96
- Community 97
- Community 98
- Community 99
- Community 100
- Community 101
- Community 102
- Community 103
- Community 104
- Community 105
- Community 106
- Community 107
- Community 108
- Community 109
- Community 110
- Community 111
- Community 112
- Community 113
- Community 115
- Community 116
- Community 117
- Community 118
- Community 119
- Community 120
- Community 121
- Community 122
- Community 123
- Community 125

## God Nodes (most connected - your core abstractions)
1. `Phase4Director` - 119 edges
2. `Phase2Director` - 112 edges
3. `LayarTerakhir.Core` - 108 edges
4. `AtmajaPhoneView` - 107 edges
5. `AtmajaDirector` - 100 edges
6. `CampaignState` - 95 edges
7. `Phase3Director` - 85 edges
8. `LayarTerakhir.Prolog` - 84 edges
9. `HudController` - 83 edges
10. `ShoulderCameraRig` - 80 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase2SceneBuilder` --references--> `Phase2Director`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/Phase2HospitalArchitecture.cs → Assets/_LayarTerakhir/src/Phase2/Phase2Director.cs

## Import Cycles
- None detected.

## Communities (126 total, 25 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (15): DreadoutVisualRevision, Report, SceneReport, GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials (+7 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (46): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), fetch() (+38 more)

### Community 3 - "Community 3"
Cohesion: 0.10
Nodes (6): LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (14): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, AtmajaSceneValidation, OpeningBaseline (+6 more)

### Community 6 - "Community 6"
Cohesion: 0.09
Nodes (11): Bindings, MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, SourceBinding, SceneRecord, Usage (+3 more)

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (10): BloodWriting, DesignerVrmBuilder, Phase1SceneBuilder, ImportedManifest, ImportedMat, Phase3SceneBuilder, PrologPresentationBuilder, Receipt (+2 more)

### Community 8 - "Community 8"
Cohesion: 0.11
Nodes (6): CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, KosVisualV2, FloorY

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (5): DesignerRevisionTests, FoundationAuditTests, Phase2SceneTests, Count, Phase3SceneTests

### Community 10 - "Community 10"
Cohesion: 0.07
Nodes (26): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+18 more)

### Community 11 - "Community 11"
Cohesion: 0.08
Nodes (3): main(), snapshot(), main()

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (18): CampaignData, EndingType, Bad, None, Normal, Secret, True, Phase1Data (+10 more)

### Community 13 - "Community 13"
Cohesion: 0.10
Nodes (11): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+3 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (5): BuildRecord, Phase3SceneBuilder, SceneManifest, Usage, Phase3SceneBuilder

### Community 15 - "Community 15"
Cohesion: 0.10
Nodes (15): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+7 more)

### Community 16 - "Community 16"
Cohesion: 0.13
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (21): AtmajaMirrorReflection, AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText (+13 more)

### Community 18 - "Community 18"
Cohesion: 0.05
Nodes (14): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+6 more)

### Community 19 - "Community 19"
Cohesion: 0.10
Nodes (9): FoundationPlayAuditTests, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 20 - "Community 20"
Cohesion: 0.12
Nodes (8): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey

### Community 21 - "Community 21"
Cohesion: 0.06
Nodes (20): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+12 more)

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (13): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+5 more)

### Community 23 - "Community 23"
Cohesion: 0.06
Nodes (13): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+5 more)

### Community 24 - "Community 24"
Cohesion: 0.11
Nodes (7): PlaytestReviewValidation, Record, Authoring, PrologCompletionBuilder, TempShaftProbe, MotorcycleRiderAlignment, AtmajaEvidenceAtmosphere

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (3): Phase1Content, LayarTerakhir.Phase2, LayarTerakhir.Phase1

### Community 26 - "Community 26"
Cohesion: 0.12
Nodes (7): KosVisualUpgrade, VisualAudit, MelatiStreetRevision, Report, KosInteractableItem, InteractableId, KosRoomCorrections

### Community 27 - "Community 27"
Cohesion: 0.14
Nodes (6): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1Anchor, Phase1World

### Community 28 - "Community 28"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 29 - "Community 29"
Cohesion: 0.08
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 30 - "Community 30"
Cohesion: 0.07
Nodes (11): sample_surface(), solve(), evaluate(), jacobian(), residual(), adapt(), WeightOnlyLinear, Info (+3 more)

### Community 31 - "Community 31"
Cohesion: 0.09
Nodes (10): Receipt, ReviewPlayerAudit, Hud, Phone, View, DoubleLeafGate, IsOpen, KosCorridorDirector (+2 more)

### Community 32 - "Community 32"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 34 - "Community 34"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 35 - "Community 35"
Cohesion: 0.06
Nodes (30): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (3): LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4

### Community 37 - "Community 37"
Cohesion: 0.09
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 38 - "Community 38"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 39 - "Community 39"
Cohesion: 0.12
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 40 - "Community 40"
Cohesion: 0.08
Nodes (11): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId, MelatiShop, InteractableId (+3 more)

### Community 41 - "Community 41"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 44 - "Community 44"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 45 - "Community 45"
Cohesion: 0.08
Nodes (7): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 46 - "Community 46"
Cohesion: 0.08
Nodes (18): GraphicsReferenceAudit, Receipt, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar (+10 more)

### Community 47 - "Community 47"
Cohesion: 0.07
Nodes (25): AtmajaPlayerAudit, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock (+17 more)

### Community 48 - "Community 48"
Cohesion: 0.16
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (5): MelatiNeighborhoodBuilder, MelatiResidenceFactory, Phase3NavInspection, Phase1LightBudget, ARPhotoGlimmer

### Community 50 - "Community 50"
Cohesion: 0.09
Nodes (14): PrologKosSceneBuilder, PrologMilestoneValidator, InteractionInteractor, ContextResolver, CurrentTarget, ScanVisualMode, AR, Normal (+6 more)

### Community 51 - "Community 51"
Cohesion: 0.12
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 53 - "Community 53"
Cohesion: 0.12
Nodes (22): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+14 more)

### Community 54 - "Community 54"
Cohesion: 0.12
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 55 - "Community 55"
Cohesion: 0.12
Nodes (11): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+3 more)

### Community 56 - "Community 56"
Cohesion: 0.07
Nodes (22): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+14 more)

### Community 57 - "Community 57"
Cohesion: 0.10
Nodes (17): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+9 more)

### Community 58 - "Community 58"
Cohesion: 0.11
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 59 - "Community 59"
Cohesion: 0.10
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 60 - "Community 60"
Cohesion: 0.08
Nodes (4): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Ids

### Community 61 - "Community 61"
Cohesion: 0.19
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 62 - "Community 62"
Cohesion: 0.09
Nodes (5): opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (6): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape, Phase4ForestHazard, Phase4Soundscape

### Community 65 - "Community 65"
Cohesion: 0.12
Nodes (7): CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase3Campaign

### Community 67 - "Community 67"
Cohesion: 0.14
Nodes (6): SceneAudioProfile, VoiceBank, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 68 - "Community 68"
Cohesion: 0.17
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 69 - "Community 69"
Cohesion: 0.12
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 70 - "Community 70"
Cohesion: 0.11
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 71 - "Community 71"
Cohesion: 0.19
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 72 - "Community 72"
Cohesion: 0.13
Nodes (7): fetch(), main(), main(), prepare(), foley(), render(), write_pcm()

### Community 73 - "Community 73"
Cohesion: 0.17
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 74 - "Community 74"
Cohesion: 0.12
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 75 - "Community 75"
Cohesion: 0.18
Nodes (6): Phase3Content, Phase3Director, AlcoveClosed, InAlcove, Phase3WorldItem, InteractableId

### Community 76 - "Community 76"
Cohesion: 0.16
Nodes (16): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+8 more)

### Community 77 - "Community 77"
Cohesion: 0.10
Nodes (8): DevelopmentScreenshotCapture, FoundationAuditCoroutineHost, Phase2ThermalTarget, Phase3Gate, IsOpen, Phase3UvPresentation, Phase4UvPresentation, ClockAmbience

### Community 79 - "Community 79"
Cohesion: 0.19
Nodes (3): Phase1Director, Phase1WorldItem, InteractableId

### Community 81 - "Community 81"
Cohesion: 0.11
Nodes (10): Phase1Balance, Phase2Balance, Phase3Balance, Phase3Wewe, B, Head, Pursuing, State (+2 more)

### Community 82 - "Community 82"
Cohesion: 0.15
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 84 - "Community 84"
Cohesion: 0.16
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 85 - "Community 85"
Cohesion: 0.18
Nodes (3): PhoneDocument, Phase2CampaignTests, Phase2Campaign

### Community 86 - "Community 86"
Cohesion: 0.16
Nodes (4): PresentationProfile, VoiceLine, Phase4Sound, AtmajaSynth

### Community 87 - "Community 87"
Cohesion: 0.17
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 88 - "Community 88"
Cohesion: 0.16
Nodes (8): Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape, Voice

### Community 89 - "Community 89"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 90 - "Community 90"
Cohesion: 0.20
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 91 - "Community 91"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 94 - "Community 94"
Cohesion: 0.16
Nodes (10): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed, Phase3Anchor (+2 more)

### Community 97 - "Community 97"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 98 - "Community 98"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 99 - "Community 99"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 100 - "Community 100"
Cohesion: 0.21
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 102 - "Community 102"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 103 - "Community 103"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 106 - "Community 106"
Cohesion: 0.20
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 107 - "Community 107"
Cohesion: 0.22
Nodes (4): AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 108 - "Community 108"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 109 - "Community 109"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 110 - "Community 110"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 111 - "Community 111"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 112 - "Community 112"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 113 - "Community 113"
Cohesion: 0.36
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 115 - "Community 115"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 119 - "Community 119"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 121 - "Community 121"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 122 - "Community 122"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **558 isolated node(s):** `CameraCanScan`, `CameraLocked`, `CameraStatus`, `Campaign`, `CanBeginLive` (+553 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 896 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **25 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 17` to `Community 0`, `Community 2`, `Community 3`, `Community 5`, `Community 6`, `Community 7`, `Community 8`, `Community 10`, `Community 13`, `Community 15`, `Community 19`, `Community 21`, `Community 22`, `Community 26`, `Community 27`, `Community 28`, `Community 29`, `Community 31`, `Community 32`, `Community 35`, `Community 37`, `Community 39`, `Community 40`, `Community 41`, `Community 43`, `Community 46`, `Community 48`, `Community 50`, `Community 55`, `Community 57`, `Community 63`, `Community 69`, `Community 70`, `Community 73`, `Community 77`, `Community 82`, `Community 84`, `Community 86`, `Community 87`, `Community 90`, `Community 105`, `Community 114`, `Community 118`?**
  _High betweenness centrality (0.207) - this node is a cross-community bridge._
- **What connects `CameraCanScan`, `CameraLocked`, `CameraStatus` to the rest of the system?**
  _558 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05103324348607367 - nodes in this community are weakly interconnected._
- **Why does `Phase4Director` connect `Community 41` to `Community 0`, `Community 3`, `Community 5`, `Community 6`, `Community 7`, `Community 8`, `Community 12`, `Community 13`, `Community 17`, `Community 29`, `Community 36`, `Community 39`, `Community 45`, `Community 50`, `Community 52`, `Community 55`, `Community 56`, `Community 57`, `Community 58`, `Community 63`, `Community 66`, `Community 68`, `Community 70`, `Community 77`, `Community 78`, `Community 81`, `Community 82`, `Community 105`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05710162853019996 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 23` to `Community 2`, `Community 67`, `Community 36`, `Community 6`, `Community 77`, `Community 19`, `Community 21`, `Community 86`, `Community 24`, `Community 89`, `Community 63`, `Community 31`?**
  _High betweenness centrality (0.053) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.0926899187768753 - nodes in this community are weakly interconnected._
