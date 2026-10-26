# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4550 nodes · 16626 edges · 144 communities (111 shown, 33 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1170 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `073a0ab4`
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
- Community 114
- Community 115
- Community 116
- Community 117
- Community 118
- Community 119
- Community 120
- Community 121
- Community 122
- Community 123
- Community 124
- Community 125
- Community 126
- Community 127
- Community 128
- Community 129
- Community 130
- Community 131
- Community 132
- Community 133
- Community 134
- Community 135
- Community 136
- Community 138
- Community 139
- Community 141
- Community 143

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 124 edges
2. `Phase4Director` - 120 edges
3. `LayarTerakhir.Core` - 115 edges
4. `Phase2Director` - 112 edges
5. `AtmajaPhoneView` - 102 edges
6. `AtmajaDirector` - 100 edges
7. `LayarTerakhir.Prolog` - 86 edges
8. `Phase3Director` - 85 edges
9. `HudController` - 82 edges
10. `AtmajaPlayTests` - 82 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase1CampaignTests` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/Phase1CampaignTests.cs → Assets/_LayarTerakhir/src/Core/CampaignState.cs

## Import Cycles
- None detected.

## Communities (144 total, 33 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.03
Nodes (10): DesignerRevisionTests, GoldenSceneTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Count, Phase3SceneTests, Phase4SceneTests (+2 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (42): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+34 more)

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (8): asset(), download(), load_meta(), main(), snapshot(), main(), main(), sha()

### Community 4 - "Community 4"
Cohesion: 0.09
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (10): VoiceBank, BuildRecord, Phase3SceneBuilder, SceneManifest, Usage, Phase3SceneBuilder, Report, PlaytestRevisionBuilder (+2 more)

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (7): SceneAudioProfile, CompactKosPolish, GoldenSceneProduction, GoldenSceneValidation, Record, KosVisualV2, FloorY

### Community 7 - "Community 7"
Cohesion: 0.04
Nodes (6): Phase1Content, Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Phase1, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 8 - "Community 8"
Cohesion: 0.11
Nodes (6): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog

### Community 9 - "Community 9"
Cohesion: 0.08
Nodes (6): Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (12): CampaignData, CampaignRouting, Phase3Ids, Phase5Data, Phase5Ids, UiFocus, BlocksMovement, BlocksWorld (+4 more)

### Community 11 - "Community 11"
Cohesion: 0.10
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 12 - "Community 12"
Cohesion: 0.05
Nodes (14): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Phase5SceneBuilder, Bindings (+6 more)

### Community 13 - "Community 13"
Cohesion: 0.09
Nodes (14): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Torch, Authentic, PermanentlyOff (+6 more)

### Community 14 - "Community 14"
Cohesion: 0.08
Nodes (11): GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+3 more)

### Community 15 - "Community 15"
Cohesion: 0.08
Nodes (8): AtmajaStairRevisionBuilder, BandVertex, DesignerWorldRevision, KosVisualUpgrade, VisualAudit, ImportedManifest, ImportedMat, Phase3SceneBuilder

### Community 16 - "Community 16"
Cohesion: 0.06
Nodes (8): DreadoutVisualRevision, Report, SceneReport, Phase3NavInspection, PlaytestRepairBuilder, Phase1LightBudget, Phase4FearPresentation, PracticalLightPresentation

### Community 17 - "Community 17"
Cohesion: 0.10
Nodes (9): BloodWriting, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1SceneBuilder, Phase1Anchor, Phase1World (+1 more)

### Community 19 - "Community 19"
Cohesion: 0.06
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 20 - "Community 20"
Cohesion: 0.06
Nodes (13): KosGroundWallRepair, KosRoomSurvey, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Phase2NavigationProbe, Phase4MaterialProbe (+5 more)

### Community 21 - "Community 21"
Cohesion: 0.12
Nodes (9): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2Anchor, Phase2Room (+1 more)

### Community 22 - "Community 22"
Cohesion: 0.05
Nodes (23): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+15 more)

### Community 23 - "Community 23"
Cohesion: 0.08
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (31): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+23 more)

### Community 25 - "Community 25"
Cohesion: 0.06
Nodes (24): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+16 more)

### Community 26 - "Community 26"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 27 - "Community 27"
Cohesion: 0.06
Nodes (19): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, InteractionInteractor, ContextResolver, CurrentTarget, AtmajaItem (+11 more)

### Community 28 - "Community 28"
Cohesion: 0.05
Nodes (15): ball(), box(), cylinder(), mat(), tube(), box(), cylinder(), finish() (+7 more)

### Community 29 - "Community 29"
Cohesion: 0.05
Nodes (34): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+26 more)

### Community 30 - "Community 30"
Cohesion: 0.07
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 31 - "Community 31"
Cohesion: 0.07
Nodes (26): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+18 more)

### Community 32 - "Community 32"
Cohesion: 0.11
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 33 - "Community 33"
Cohesion: 0.06
Nodes (27): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+19 more)

### Community 34 - "Community 34"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 35 - "Community 35"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 36 - "Community 36"
Cohesion: 0.06
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 37 - "Community 37"
Cohesion: 0.11
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 38 - "Community 38"
Cohesion: 0.15
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 41 - "Community 41"
Cohesion: 0.10
Nodes (19): KuntiState, Dormant, Manifested, Pursuit, Resolved, Phase1Threats, Balance, BossStatus (+11 more)

### Community 42 - "Community 42"
Cohesion: 0.18
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 43 - "Community 43"
Cohesion: 0.08
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 44 - "Community 44"
Cohesion: 0.14
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 45 - "Community 45"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 46 - "Community 46"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 47 - "Community 47"
Cohesion: 0.15
Nodes (3): FoundationPlayAuditTests, KosInteractableItem, InteractableId

### Community 48 - "Community 48"
Cohesion: 0.10
Nodes (4): PlayerFeelAudit, Receipt, Sample, KosGroundWallTests

### Community 50 - "Community 50"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 51 - "Community 51"
Cohesion: 0.10
Nodes (8): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 52 - "Community 52"
Cohesion: 0.15
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 53 - "Community 53"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 54 - "Community 54"
Cohesion: 0.17
Nodes (6): Phase3PlayerAudit, Result, ScanVisualMode, AR, Normal, UV

### Community 55 - "Community 55"
Cohesion: 0.08
Nodes (7): dialogue_literals(), export(), opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 56 - "Community 56"
Cohesion: 0.09
Nodes (11): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse, Phase1MoneyReceipt (+3 more)

### Community 57 - "Community 57"
Cohesion: 0.13
Nodes (3): Phase2Content, Phase3Content, MotorcycleRiderAlignment

### Community 59 - "Community 59"
Cohesion: 0.16
Nodes (5): KosCorridorBuilder, KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 60 - "Community 60"
Cohesion: 0.07
Nodes (24): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+16 more)

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (7): Phase4Banaspati, B, LungeProgress, Pursuing, State, W, Phase4ForestHazard

### Community 62 - "Community 62"
Cohesion: 0.16
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 63 - "Community 63"
Cohesion: 0.13
Nodes (11): Phase5Director, FinalHint, FinalReadout, NearEdge, NearPortal, EvpProgress, SelectedSource, ThermalReadout (+3 more)

### Community 64 - "Community 64"
Cohesion: 0.13
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 68 - "Community 68"
Cohesion: 0.08
Nodes (19): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+11 more)

### Community 69 - "Community 69"
Cohesion: 0.14
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 70 - "Community 70"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 71 - "Community 71"
Cohesion: 0.17
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 72 - "Community 72"
Cohesion: 0.11
Nodes (11): Phase1Balance, Phase2Balance, Phase3Balance, Phase3Wewe, B, Head, Pursuing, State (+3 more)

### Community 73 - "Community 73"
Cohesion: 0.14
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 75 - "Community 75"
Cohesion: 0.10
Nodes (8): DevelopmentScreenshotCapture, FoundationAuditCoroutineHost, Phase2ThermalTarget, Phase3UvPresentation, Phase4UvPresentation, Phase5AssetSource, Phase5HumanMotion, ClockAmbience

### Community 77 - "Community 77"
Cohesion: 0.19
Nodes (5): Phase3Director, AlcoveClosed, InAlcove, Phase3WorldItem, InteractableId

### Community 78 - "Community 78"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 79 - "Community 79"
Cohesion: 0.13
Nodes (5): Phase4Lighting, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 80 - "Community 80"
Cohesion: 0.13
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 81 - "Community 81"
Cohesion: 0.19
Nodes (3): Phase1Director, Phase1WorldItem, InteractableId

### Community 82 - "Community 82"
Cohesion: 0.15
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 83 - "Community 83"
Cohesion: 0.22
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 84 - "Community 84"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 85 - "Community 85"
Cohesion: 0.15
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 87 - "Community 87"
Cohesion: 0.12
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 89 - "Community 89"
Cohesion: 0.15
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 90 - "Community 90"
Cohesion: 0.11
Nodes (13): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+5 more)

### Community 91 - "Community 91"
Cohesion: 0.16
Nodes (9): Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 92 - "Community 92"
Cohesion: 0.15
Nodes (8): Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape, Voice

### Community 93 - "Community 93"
Cohesion: 0.16
Nodes (3): PresentationProfile, VoiceLine, AtmajaSynth

### Community 94 - "Community 94"
Cohesion: 0.18
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 95 - "Community 95"
Cohesion: 0.19
Nodes (10): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+2 more)

### Community 96 - "Community 96"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 97 - "Community 97"
Cohesion: 0.22
Nodes (11): get(), main(), obj(), ph(), digest(), get(), main(), objaverse() (+3 more)

### Community 98 - "Community 98"
Cohesion: 0.15
Nodes (8): FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld, PlanarVelocity

### Community 99 - "Community 99"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 100 - "Community 100"
Cohesion: 0.18
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 101 - "Community 101"
Cohesion: 0.17
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 103 - "Community 103"
Cohesion: 0.16
Nodes (10): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed, Phase3Anchor (+2 more)

### Community 104 - "Community 104"
Cohesion: 0.20
Nodes (3): AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 105 - "Community 105"
Cohesion: 0.29
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 107 - "Community 107"
Cohesion: 0.19
Nodes (7): PlaytestReviewValidation, Record, KosCorridorDirector, Busy, CanDepart, KosTravelItem, InteractableId

### Community 113 - "Community 113"
Cohesion: 0.20
Nodes (3): PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 114 - "Community 114"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 115 - "Community 115"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 117 - "Community 117"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 120 - "Community 120"
Cohesion: 0.18
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 121 - "Community 121"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 122 - "Community 122"
Cohesion: 0.20
Nodes (5): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 123 - "Community 123"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 126 - "Community 126"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 128 - "Community 128"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 129 - "Community 129"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 130 - "Community 130"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 132 - "Community 132"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 133 - "Community 133"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 134 - "Community 134"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 135 - "Community 135"
Cohesion: 0.33
Nodes (6): EndingId, BAD, NONE, NORMAL, SECRET, TRUE

### Community 136 - "Community 136"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 138 - "Community 138"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **614 isolated node(s):** `BlocksMovement`, `BlocksWorld`, `HardModal`, `FragileDrop`, `FragileFatal` (+609 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1006 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **33 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 36` to `Community 6`, `Community 9`, `Community 137`, `Community 11`, `Community 140`, `Community 13`, `Community 14`, `Community 15`, `Community 139`, `Community 17`, `Community 19`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 29`, `Community 30`, `Community 31`, `Community 32`, `Community 33`, `Community 34`, `Community 37`, `Community 39`, `Community 43`, `Community 47`, `Community 51`, `Community 57`, `Community 62`, `Community 71`, `Community 75`, `Community 76`, `Community 79`, `Community 80`, `Community 82`, `Community 84`, `Community 91`, `Community 93`, `Community 94`, `Community 95`, `Community 100`, `Community 105`, `Community 107`, `Community 119`?**
  _High betweenness centrality (0.230) - this node is a cross-community bridge._
- **What connects `BlocksMovement`, `BlocksWorld`, `HardModal` to the rest of the system?**
  _614 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.02863568215892054 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 25` to `Community 98`, `Community 6`, `Community 8`, `Community 75`, `Community 48`, `Community 17`, `Community 51`, `Community 22`, `Community 87`, `Community 93`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05 - nodes in this community are weakly interconnected._
- **Why does `Phase2Director` connect `Community 33` to `Community 6`, `Community 9`, `Community 140`, `Community 17`, `Community 19`, `Community 20`, `Community 21`, `Community 34`, `Community 36`, `Community 39`, `Community 42`, `Community 44`, `Community 48`, `Community 49`, `Community 51`, `Community 57`, `Community 72`, `Community 75`, `Community 82`, `Community 83`, `Community 88`, `Community 91`, `Community 100`, `Community 105`, `Community 106`, `Community 112`, `Community 120`?**
  _High betweenness centrality (0.059) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.05303392259913999 - nodes in this community are weakly interconnected._
