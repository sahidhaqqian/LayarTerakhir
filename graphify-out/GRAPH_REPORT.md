# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 3063 nodes · 11061 edges · 116 communities (88 shown, 28 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 748 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `2272510a`
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
- Community 102
- Community 103
- Community 104
- Community 105
- Community 106
- Community 108
- Community 109
- Community 110
- Community 112
- Community 114
- Community 115

## God Nodes (most connected - your core abstractions)
1. `Phase2Director` - 113 edges
2. `AtmajaPhoneView` - 100 edges
3. `AtmajaDirector` - 100 edges
4. `HudController` - 83 edges
5. `ShoulderCameraRig` - 81 edges
6. `LayarTerakhir.Core` - 80 edges
7. `LayarTerakhir.Prolog` - 79 edges
8. `AtmajaPlayTests` - 78 edges
9. `LayarTerakhir.Player` - 77 edges
10. `FirstPersonController` - 69 edges

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

## Communities (116 total, 28 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.06
Nodes (14): Phase2AssetImport, Phase1Content, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Phase2, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1 (+6 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (15): DreadoutVisualRevision, Report, SceneReport, GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials (+7 more)

### Community 2 - "Community 2"
Cohesion: 0.07
Nodes (32): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+24 more)

### Community 3 - "Community 3"
Cohesion: 0.11
Nodes (5): CompactKosPolish, GoldenSceneValidation, Record, KosVisualV2, FloorY

### Community 4 - "Community 4"
Cohesion: 0.07
Nodes (12): PresentationProfile, GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 5 - "Community 5"
Cohesion: 0.06
Nodes (27): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+19 more)

### Community 6 - "Community 6"
Cohesion: 0.07
Nodes (7): asset(), download(), load_meta(), convert(), foley(), main(), snapshot()

### Community 7 - "Community 7"
Cohesion: 0.08
Nodes (18): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+10 more)

### Community 9 - "Community 9"
Cohesion: 0.11
Nodes (12): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+4 more)

### Community 10 - "Community 10"
Cohesion: 0.05
Nodes (21): AtmajaMirrorReflection, AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText (+13 more)

### Community 12 - "Community 12"
Cohesion: 0.10
Nodes (5): KosVisualUpgrade, VisualAudit, Phase2NavigationProbe, TempShaftProbe, KosRoomCorrections

### Community 13 - "Community 13"
Cohesion: 0.11
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 14 - "Community 14"
Cohesion: 0.12
Nodes (9): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry (+1 more)

### Community 15 - "Community 15"
Cohesion: 0.14
Nodes (6): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1Anchor, Phase1World

### Community 16 - "Community 16"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (12): export(), meshes(), box(), cylinder(), finish(), tube(), export(), meshes() (+4 more)

### Community 18 - "Community 18"
Cohesion: 0.07
Nodes (16): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cylinder() (+8 more)

### Community 19 - "Community 19"
Cohesion: 0.10
Nodes (7): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Phase1SceneBuilder, ARPhotoGlimmer

### Community 21 - "Community 21"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 22 - "Community 22"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 23 - "Community 23"
Cohesion: 0.18
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 25 - "Community 25"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 26 - "Community 26"
Cohesion: 0.11
Nodes (12): Phase2Content, MotorcycleRiderAlignment, PhoneApp, Bank, Call, Camera, Home, Inventory (+4 more)

### Community 27 - "Community 27"
Cohesion: 0.07
Nodes (10): AmbientAudioLoop, LevelMultiplier, FoundationAuditCoroutineHost, Phase2Sound, Phase2Soundscape, Phase2ThermalTarget, AtmajaVault, ClockAmbience (+2 more)

### Community 28 - "Community 28"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 29 - "Community 29"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 30 - "Community 30"
Cohesion: 0.14
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 31 - "Community 31"
Cohesion: 0.12
Nodes (3): GoldenSceneTests, Phase2SceneTests, PlayerFeelTests

### Community 32 - "Community 32"
Cohesion: 0.11
Nodes (11): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+3 more)

### Community 33 - "Community 33"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 34 - "Community 34"
Cohesion: 0.16
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 35 - "Community 35"
Cohesion: 0.09
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 36 - "Community 36"
Cohesion: 0.13
Nodes (7): VoiceBank, DesignerVrmBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, PrologPresentationBuilder, Receipt

### Community 37 - "Community 37"
Cohesion: 0.14
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 38 - "Community 38"
Cohesion: 0.09
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 39 - "Community 39"
Cohesion: 0.09
Nodes (10): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph (+2 more)

### Community 40 - "Community 40"
Cohesion: 0.18
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 42 - "Community 42"
Cohesion: 0.11
Nodes (13): ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible, CurrentDistance, FirstPersonOverride, IsFirstPerson (+5 more)

### Community 43 - "Community 43"
Cohesion: 0.11
Nodes (12): CampaignData, EndingType, Bad, None, Normal, Secret, True, UiFocus (+4 more)

### Community 44 - "Community 44"
Cohesion: 0.13
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 46 - "Community 46"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 47 - "Community 47"
Cohesion: 0.13
Nodes (9): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, InteractionInteractor, ContextResolver, CurrentTarget, AtmajaItem (+1 more)

### Community 48 - "Community 48"
Cohesion: 0.19
Nodes (3): Phase1Director, Phase1WorldItem, InteractableId

### Community 49 - "Community 49"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 50 - "Community 50"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 52 - "Community 52"
Cohesion: 0.27
Nodes (3): FoundationPlayAuditTests, KosInteractableItem, InteractableId

### Community 53 - "Community 53"
Cohesion: 0.15
Nodes (9): Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 54 - "Community 54"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 55 - "Community 55"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 56 - "Community 56"
Cohesion: 0.11
Nodes (3): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests

### Community 59 - "Community 59"
Cohesion: 0.18
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 61 - "Community 61"
Cohesion: 0.18
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 62 - "Community 62"
Cohesion: 0.16
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 65 - "Community 65"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 66 - "Community 66"
Cohesion: 0.18
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 67 - "Community 67"
Cohesion: 0.15
Nodes (8): FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld, PlanarVelocity

### Community 68 - "Community 68"
Cohesion: 0.14
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 69 - "Community 69"
Cohesion: 0.18
Nodes (4): AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 70 - "Community 70"
Cohesion: 0.17
Nodes (6): Phase1Zone, IPhoneChapterContent, CameraLocked, CameraStatus, CanBeginLive, Viewers

### Community 71 - "Community 71"
Cohesion: 0.14
Nodes (3): dialogue_literals(), export(), DialogueRecordingTests

### Community 72 - "Community 72"
Cohesion: 0.17
Nodes (4): opening_structure_sha256(), verify_opening_source(), GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 73 - "Community 73"
Cohesion: 0.25
Nodes (10): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+2 more)

### Community 76 - "Community 76"
Cohesion: 0.27
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 77 - "Community 77"
Cohesion: 0.20
Nodes (5): KosCorridorDirector, Busy, CanDepart, KosTravelItem, InteractableId

### Community 78 - "Community 78"
Cohesion: 0.21
Nodes (8): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse, Phase1MoneyReceipt

### Community 79 - "Community 79"
Cohesion: 0.19
Nodes (5): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, Instance

### Community 81 - "Community 81"
Cohesion: 0.23
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 83 - "Community 83"
Cohesion: 0.24
Nodes (5): GraphicsReferenceAudit, Receipt, PlayerPerspective, FirstPerson, OverShoulder

### Community 85 - "Community 85"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 86 - "Community 86"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 87 - "Community 87"
Cohesion: 0.21
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 89 - "Community 89"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 92 - "Community 92"
Cohesion: 0.22
Nodes (3): PlaytestReviewValidation, Record, AtmajaWorldText

### Community 93 - "Community 93"
Cohesion: 0.31
Nodes (3): AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 94 - "Community 94"
Cohesion: 0.22
Nodes (4): AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 95 - "Community 95"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 96 - "Community 96"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 97 - "Community 97"
Cohesion: 0.31
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 98 - "Community 98"
Cohesion: 0.25
Nodes (3): AtmajaSceneValidation, OpeningBaseline, Report

### Community 99 - "Community 99"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 102 - "Community 102"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 103 - "Community 103"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 104 - "Community 104"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 108 - "Community 108"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 109 - "Community 109"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 112 - "Community 112"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **395 isolated node(s):** `Apps`, `Chapter`, `ChatFrozen`, `CinematicHandsetVisible`, `ClockText` (+390 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 680 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **28 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 10` to `Community 0`, `Community 1`, `Community 3`, `Community 4`, `Community 5`, `Community 8`, `Community 9`, `Community 11`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 18`, `Community 19`, `Community 21`, `Community 24`, `Community 27`, `Community 28`, `Community 29`, `Community 30`, `Community 32`, `Community 38`, `Community 46`, `Community 51`, `Community 52`, `Community 53`, `Community 55`, `Community 57`, `Community 58`, `Community 59`, `Community 76`, `Community 77`, `Community 83`, `Community 97`, `Community 101`, `Community 107`, `Community 110`?**
  _High betweenness centrality (0.280) - this node is a cross-community bridge._
- **What connects `Apps`, `Chapter`, `ChatFrozen` to the rest of the system?**
  _395 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05750199333989963 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 81` to `Community 0`, `Community 67`, `Community 74`, `Community 15`, `Community 18`, `Community 54`, `Community 23`, `Community 27`, `Community 93`, `Community 30`?**
  _High betweenness centrality (0.056) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05290072297654735 - nodes in this community are weakly interconnected._
- **Why does `Phase2Director` connect `Community 5` to `Community 0`, `Community 1`, `Community 3`, `Community 10`, `Community 12`, `Community 15`, `Community 16`, `Community 19`, `Community 23`, `Community 24`, `Community 26`, `Community 27`, `Community 31`, `Community 32`, `Community 36`, `Community 39`, `Community 44`, `Community 45`, `Community 53`, `Community 66`, `Community 91`, `Community 107`, `Community 113`, `Community 115`?**
  _High betweenness centrality (0.056) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.06773211567732115 - nodes in this community are weakly interconnected._
