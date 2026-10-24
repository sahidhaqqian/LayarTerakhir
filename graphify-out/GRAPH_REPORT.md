# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 3493 nodes · 12531 edges · 125 communities (95 shown, 30 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 864 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `a8b13407`
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
- Community 95
- Community 96
- Community 97
- Community 98
- Community 99
- Community 100
- Community 101
- Community 102
- Community 103
- Community 105
- Community 106
- Community 107
- Community 109
- Community 110
- Community 111
- Community 112
- Community 113
- Community 114
- Community 115
- Community 116
- Community 117
- Community 118
- Community 119
- Community 120
- Community 121
- Community 122
- Community 124

## God Nodes (most connected - your core abstractions)
1. `Phase2Director` - 112 edges
2. `AtmajaPhoneView` - 103 edges
3. `AtmajaDirector` - 100 edges
4. `LayarTerakhir.Core` - 92 edges
5. `Phase3Director` - 86 edges
6. `HudController` - 83 edges
7. `LayarTerakhir.Prolog` - 81 edges
8. `ShoulderCameraRig` - 80 edges
9. `AtmajaPlayTests` - 78 edges
10. `LayarTerakhir.Player` - 74 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `PhoneDesktop` --references--> `SmartphoneController`  [EXTRACTED]
  Assets/_LayarTerakhir/src/UI/PhoneDesktop.cs → Assets/_LayarTerakhir/src/Smartphone/SmartphoneController.cs

## Import Cycles
- None detected.

## Communities (125 total, 30 thin omitted)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (42): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), dialogue_literals() (+34 more)

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (14): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, AtmajaSceneValidation, OpeningBaseline (+6 more)

### Community 3 - "Community 3"
Cohesion: 0.11
Nodes (5): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Smartphone, LayarTerakhir.Diagnostics

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (5): DesignerRevisionTests, Phase2SceneTests, Count, Phase3SceneTests, PlayerFeelTests

### Community 6 - "Community 6"
Cohesion: 0.12
Nodes (3): CompactKosPolish, KosVisualV2, FloorY

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (26): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+18 more)

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (11): AtmajaStairRevisionBuilder, BandVertex, BloodWriting, DesignerWorldRevision, MelatiNeighborhoodBuilder, MelatiResidenceFactory, ImportedManifest, ImportedMat (+3 more)

### Community 9 - "Community 9"
Cohesion: 0.11
Nodes (6): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1SceneBuilder, Phase1Anchor

### Community 10 - "Community 10"
Cohesion: 0.10
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 11 - "Community 11"
Cohesion: 0.10
Nodes (7): KosVisualUpgrade, VisualAudit, MelatiStreetRevision, Report, KosInteractableItem, InteractableId, KosRoomCorrections

### Community 12 - "Community 12"
Cohesion: 0.10
Nodes (11): Phase3PlayerAudit, Result, Phase3Director, AlcoveClosed, InAlcove, Phase3WorldItem, InteractableId, ScanVisualMode (+3 more)

### Community 13 - "Community 13"
Cohesion: 0.11
Nodes (9): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey (+1 more)

### Community 14 - "Community 14"
Cohesion: 0.05
Nodes (21): AtmajaMirrorReflection, AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText (+13 more)

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (16): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId, MelatiDeparture, InteractableId (+8 more)

### Community 16 - "Community 16"
Cohesion: 0.05
Nodes (14): cube(), cyl(), finish(), lathe(), box(), cylinder(), finish(), tube() (+6 more)

### Community 17 - "Community 17"
Cohesion: 0.07
Nodes (20): CampaignData, EndingType, Bad, None, Normal, Secret, True, Phase1Challenge (+12 more)

### Community 18 - "Community 18"
Cohesion: 0.09
Nodes (13): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+5 more)

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (4): DesignerVrmBuilder, PrologPresentationBuilder, Receipt, MotorcycleRiderAlignment

### Community 20 - "Community 20"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (13): PresentationProfile, VoiceLine, AtmajaReviewMoments, Hud, Phone, HudController, CurrentDialogueSpeaker, CurrentObjective (+5 more)

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (5): BuildRecord, Phase3SceneBuilder, SceneManifest, Usage, Phase3SceneBuilder

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (18): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+10 more)

### Community 24 - "Community 24"
Cohesion: 0.10
Nodes (3): foley(), main(), snapshot()

### Community 25 - "Community 25"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 27 - "Community 27"
Cohesion: 0.12
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 28 - "Community 28"
Cohesion: 0.09
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 31 - "Community 31"
Cohesion: 0.09
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 32 - "Community 32"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 33 - "Community 33"
Cohesion: 0.12
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 34 - "Community 34"
Cohesion: 0.05
Nodes (31): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 35 - "Community 35"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 38 - "Community 38"
Cohesion: 0.11
Nodes (15): Phase1Balance, Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti (+7 more)

### Community 39 - "Community 39"
Cohesion: 0.10
Nodes (13): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+5 more)

### Community 40 - "Community 40"
Cohesion: 0.12
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 42 - "Community 42"
Cohesion: 0.24
Nodes (3): Phase2SceneBuilder, ImportedMaterial, Phase2Anchor

### Community 43 - "Community 43"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 44 - "Community 44"
Cohesion: 0.11
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 45 - "Community 45"
Cohesion: 0.09
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 46 - "Community 46"
Cohesion: 0.11
Nodes (4): Phase1Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase1

### Community 47 - "Community 47"
Cohesion: 0.12
Nodes (6): PlayerFeelAudit, Receipt, Sample, KosCorridorDirector, Busy, CanDepart

### Community 48 - "Community 48"
Cohesion: 0.12
Nodes (4): CampaignRouting, Phase3Ids, Phase3CampaignTests, Phase3Campaign

### Community 49 - "Community 49"
Cohesion: 0.18
Nodes (3): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 50 - "Community 50"
Cohesion: 0.12
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 51 - "Community 51"
Cohesion: 0.13
Nodes (6): SceneAudioProfile, VoiceBank, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 53 - "Community 53"
Cohesion: 0.19
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 54 - "Community 54"
Cohesion: 0.20
Nodes (3): AtmajaSceneBuilder, Manifest, MaterialRecord

### Community 55 - "Community 55"
Cohesion: 0.10
Nodes (4): GoldenLinuxBuild, Record, Phase2NavigationProbe, SceneRecord

### Community 57 - "Community 57"
Cohesion: 0.10
Nodes (10): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph (+2 more)

### Community 59 - "Community 59"
Cohesion: 0.11
Nodes (10): GraphicsReferenceAudit, Receipt, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked (+2 more)

### Community 60 - "Community 60"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 62 - "Community 62"
Cohesion: 0.14
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 64 - "Community 64"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 65 - "Community 65"
Cohesion: 0.11
Nodes (10): PrologKosSceneBuilder, PrologMilestoneValidator, InteractionInteractor, ContextResolver, CurrentTarget, SmartphoneController, BlitzCooldownRemaining, CameraLocked (+2 more)

### Community 66 - "Community 66"
Cohesion: 0.11
Nodes (4): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Ids

### Community 67 - "Community 67"
Cohesion: 0.13
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 68 - "Community 68"
Cohesion: 0.12
Nodes (3): TempShaftProbe, LayarTerakhir.Player, LayarTerakhir.Prolog

### Community 69 - "Community 69"
Cohesion: 0.14
Nodes (10): Phase3Content, Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 70 - "Community 70"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 71 - "Community 71"
Cohesion: 0.19
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 72 - "Community 72"
Cohesion: 0.18
Nodes (10): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+2 more)

### Community 73 - "Community 73"
Cohesion: 0.15
Nodes (11): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+3 more)

### Community 75 - "Community 75"
Cohesion: 0.13
Nodes (4): opening_structure_sha256(), verify_opening_source(), GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 76 - "Community 76"
Cohesion: 0.18
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 78 - "Community 78"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 79 - "Community 79"
Cohesion: 0.22
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 80 - "Community 80"
Cohesion: 0.17
Nodes (4): PlaytestReviewValidation, Record, Phase1LightBudget, PracticalLightPresentation

### Community 81 - "Community 81"
Cohesion: 0.15
Nodes (10): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed, Phase3Anchor (+2 more)

### Community 84 - "Community 84"
Cohesion: 0.19
Nodes (4): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape

### Community 85 - "Community 85"
Cohesion: 0.14
Nodes (5): AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime, ClockAmbience

### Community 86 - "Community 86"
Cohesion: 0.19
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 89 - "Community 89"
Cohesion: 0.19
Nodes (7): Phase2Balance, Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 92 - "Community 92"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 93 - "Community 93"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 94 - "Community 94"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 96 - "Community 96"
Cohesion: 0.22
Nodes (4): AtmajaItem, InteractableId, DoubleLeafGate, IsOpen

### Community 97 - "Community 97"
Cohesion: 0.24
Nodes (5): Phase1World, Phase1WorldItem, InteractableId, Phase1Zone, Phase2ThermalTarget

### Community 98 - "Community 98"
Cohesion: 0.31
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 99 - "Community 99"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 101 - "Community 101"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 102 - "Community 102"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 103 - "Community 103"
Cohesion: 0.31
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 106 - "Community 106"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 107 - "Community 107"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 110 - "Community 110"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 111 - "Community 111"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 112 - "Community 112"
Cohesion: 0.29
Nodes (5): IPhoneChapterContent, CameraLocked, CameraStatus, CanBeginLive, Viewers

### Community 113 - "Community 113"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 116 - "Community 116"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 117 - "Community 117"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 118 - "Community 118"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **482 isolated node(s):** `CallActive`, `ContractReceived`, `CurrentApp`, `DialedContact`, `IsCalling` (+477 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 797 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **30 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 14` to `Community 0`, `Community 2`, `Community 3`, `Community 6`, `Community 7`, `Community 8`, `Community 9`, `Community 10`, `Community 11`, `Community 12`, `Community 15`, `Community 18`, `Community 20`, `Community 21`, `Community 23`, `Community 25`, `Community 27`, `Community 28`, `Community 30`, `Community 33`, `Community 34`, `Community 39`, `Community 41`, `Community 44`, `Community 47`, `Community 50`, `Community 54`, `Community 55`, `Community 59`, `Community 61`, `Community 64`, `Community 65`, `Community 69`, `Community 71`, `Community 84`, `Community 87`, `Community 97`, `Community 103`?**
  _High betweenness centrality (0.228) - this node is a cross-community bridge._
- **What connects `CallActive`, `ContractReceived`, `CurrentApp` to the rest of the system?**
  _482 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.0895278167367929 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 27` to `Community 0`, `Community 3`, `Community 5`, `Community 6`, `Community 8`, `Community 9`, `Community 11`, `Community 13`, `Community 14`, `Community 18`, `Community 19`, `Community 21`, `Community 30`, `Community 31`, `Community 39`, `Community 40`, `Community 49`, `Community 51`, `Community 53`, `Community 54`, `Community 56`, `Community 59`, `Community 60`, `Community 64`, `Community 65`, `Community 71`, `Community 78`, `Community 80`, `Community 84`, `Community 85`, `Community 91`, `Community 96`, `Community 97`?**
  _High betweenness centrality (0.065) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.058672276764843385 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 73` to `Community 97`, `Community 5`, `Community 105`, `Community 9`, `Community 11`, `Community 46`, `Community 47`, `Community 51`, `Community 84`, `Community 23`, `Community 59`, `Community 30`?**
  _High betweenness centrality (0.056) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.051251956181533644 - nodes in this community are weakly interconnected._
