# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5339 nodes · 20275 edges · 157 communities (120 shown, 37 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1319 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `711228e8`
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
- Community 130
- Community 131
- Community 132
- Community 133
- Community 134
- Community 135
- Community 136
- Community 137
- Community 138
- Community 139
- Community 140
- Community 141
- Community 142
- Community 143
- Community 144
- Community 145
- Community 146
- Community 147
- Community 148
- Community 149
- Community 150
- Community 151
- Community 152
- Community 153
- Community 154

## God Nodes (most connected - your core abstractions)
1. `Phase5Director` - 129 edges
2. `CampaignState` - 128 edges
3. `Phase4Director` - 122 edges
4. `Phase2Director` - 116 edges
5. `LayarTerakhir.Core` - 115 edges
6. `Phase3Director` - 114 edges
7. `HudController` - 110 edges
8. `AtmajaPhoneView` - 107 edges
9. `AtmajaDirector` - 102 edges
10. `LayarTerakhir.Prolog` - 102 edges

## Surprising Connections (you probably didn't know these)
- `delete_fill_tagged()` --indirect_call--> `mesh()`  [INFERRED]
  Tools/repair_nyonya_appearance.py → Tools/author_atmaja_character.py
- `build()` --calls--> `export_fbx()`  [INFERRED]
  Tools/retarget_nyonya_motion.py → Tools/prepare_designer_vrm.py
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py

## Import Cycles
- None detected.

## Communities (157 total, 37 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (52): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5Boss, Active (+44 more)

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (8): Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, PhaseRestartTests, Slot, Phase2Campaign, Phase3Campaign

### Community 3 - "Community 3"
Cohesion: 0.07
Nodes (13): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+5 more)

### Community 5 - "Community 5"
Cohesion: 0.04
Nodes (10): HidingAwareness, DeveloperReviewModeTests, HidingAwarenessTests, KosBathroomRevisionTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests (+2 more)

### Community 6 - "Community 6"
Cohesion: 0.09
Nodes (8): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Phase2, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Diagnostics

### Community 7 - "Community 7"
Cohesion: 0.07
Nodes (12): CrosshairDotTests, GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 8 - "Community 8"
Cohesion: 0.05
Nodes (4): foley(), main(), snapshot(), main()

### Community 9 - "Community 9"
Cohesion: 0.05
Nodes (39): asset(), fetch(), fetch(), main(), sha(), fetch(), fetch(), main() (+31 more)

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (19): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatManifest (+11 more)

### Community 11 - "Community 11"
Cohesion: 0.07
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (19): DesignerVrmBuilder, Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests, PlayerFeelTests, PrologPresentationBuilder, Receipt (+11 more)

### Community 13 - "Community 13"
Cohesion: 0.08
Nodes (8): CampaignData, CampaignRouting, Phase4Data, Phase4Ids, Phase4CampaignTests, ReviewDirectBootTests, Slot, Phase4Campaign

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (33): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 15 - "Community 15"
Cohesion: 0.11
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 16 - "Community 16"
Cohesion: 0.05
Nodes (5): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 18 - "Community 18"
Cohesion: 0.05
Nodes (34): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+26 more)

### Community 19 - "Community 19"
Cohesion: 0.13
Nodes (4): Prolog47SceneRepair, Phase1Anchor, Phase1World, ClockAmbience

### Community 20 - "Community 20"
Cohesion: 0.08
Nodes (9): CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, Check, KosSceneValidation, Report, KosVisualV2 (+1 more)

### Community 21 - "Community 21"
Cohesion: 0.10
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder

### Community 22 - "Community 22"
Cohesion: 0.06
Nodes (13): AmbientAudioLoop, LevelMultiplier, SceneAudioProfile, PresentationProfile, VoiceLine, GoldenSceneProduction, Phase2Sound, Phase2Soundscape (+5 more)

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (6): KosGroundWallRepair, PlaytestReviewValidation, Record, RumahAtmajaDiningTests, TempShaftProbe, KosRoomCorrections

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (12): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, RecoveryPresentationProbe, RecoveryRuntimeScenario (+4 more)

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 26 - "Community 26"
Cohesion: 0.07
Nodes (13): KosGroundWallTests, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Bike, InterludeTraffic, Phase1LightBudget (+5 more)

### Community 27 - "Community 27"
Cohesion: 0.09
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 29 - "Community 29"
Cohesion: 0.13
Nodes (7): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4Zone

### Community 31 - "Community 31"
Cohesion: 0.06
Nodes (30): PhoneDocument, Phase2NavigationProbe, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 32 - "Community 32"
Cohesion: 0.07
Nodes (19): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+11 more)

### Community 33 - "Community 33"
Cohesion: 0.07
Nodes (13): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+5 more)

### Community 34 - "Community 34"
Cohesion: 0.05
Nodes (10): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+2 more)

### Community 36 - "Community 36"
Cohesion: 0.07
Nodes (18): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+10 more)

### Community 37 - "Community 37"
Cohesion: 0.09
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 38 - "Community 38"
Cohesion: 0.15
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 39 - "Community 39"
Cohesion: 0.11
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 40 - "Community 40"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+14 more)

### Community 41 - "Community 41"
Cohesion: 0.06
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 42 - "Community 42"
Cohesion: 0.10
Nodes (27): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+19 more)

### Community 43 - "Community 43"
Cohesion: 0.06
Nodes (10): dialogue_literals(), export(), opening_structure_sha256(), verify_opening_source(), included_phases(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests (+2 more)

### Community 44 - "Community 44"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 45 - "Community 45"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 46 - "Community 46"
Cohesion: 0.07
Nodes (12): sample_surface(), solve(), evaluate(), jacobian(), residual(), adapt(), WeightOnlyLinear, Info (+4 more)

### Community 47 - "Community 47"
Cohesion: 0.14
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 48 - "Community 48"
Cohesion: 0.12
Nodes (6): VoiceBank, DesignerWorldRevision, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 49 - "Community 49"
Cohesion: 0.13
Nodes (7): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Sound

### Community 50 - "Community 50"
Cohesion: 0.10
Nodes (8): FoundationAuditCoroutineHost, Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext

### Community 51 - "Community 51"
Cohesion: 0.13
Nodes (28): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+20 more)

### Community 53 - "Community 53"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 54 - "Community 54"
Cohesion: 0.08
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 55 - "Community 55"
Cohesion: 0.12
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 56 - "Community 56"
Cohesion: 0.08
Nodes (17): MotorcycleRiderAlignment, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity (+9 more)

### Community 57 - "Community 57"
Cohesion: 0.10
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 58 - "Community 58"
Cohesion: 0.08
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 59 - "Community 59"
Cohesion: 0.11
Nodes (9): PrologCompletionAudit, Campaign, Hud, Phone, Receipt, PrologMilestoneValidator, InteractionInteractor, ContextResolver (+1 more)

### Community 61 - "Community 61"
Cohesion: 0.08
Nodes (12): ScanVisualMode, AR, Normal, UV, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+4 more)

### Community 62 - "Community 62"
Cohesion: 0.10
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (11): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse, Phase1MoneyReceipt (+3 more)

### Community 64 - "Community 64"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 65 - "Community 65"
Cohesion: 0.15
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 66 - "Community 66"
Cohesion: 0.10
Nodes (14): ClockPhoto, FeastPhoto, InterludeStreetCapture, Report, View, KosPhoto, BoundsReport, Diagnosis (+6 more)

### Community 67 - "Community 67"
Cohesion: 0.09
Nodes (5): Phase4SceneTests, Phase4AssetSource, Phase4ForestHazard, Phase4WorldItem, InteractableId

### Community 68 - "Community 68"
Cohesion: 0.11
Nodes (21): copy(), sha(), checked(), export(), image_path(), material_record(), source_texture(), upstream_image() (+13 more)

### Community 69 - "Community 69"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 70 - "Community 70"
Cohesion: 0.24
Nodes (4): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, Phase2Anchor

### Community 71 - "Community 71"
Cohesion: 0.12
Nodes (22): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+14 more)

### Community 72 - "Community 72"
Cohesion: 0.11
Nodes (4): AtmajaPlayerAudit, Receipt, Receipt, RiderPresentationAudit

### Community 74 - "Community 74"
Cohesion: 0.06
Nodes (27): Phase1Balance, Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering (+19 more)

### Community 75 - "Community 75"
Cohesion: 0.09
Nodes (10): GoldenLinuxBuild, Record, SceneRecord, Phase2ThermalTarget, Phase2Room, Phase2World, Phase4Lighting, AtmajaWorldText (+2 more)

### Community 77 - "Community 77"
Cohesion: 0.08
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 78 - "Community 78"
Cohesion: 0.15
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 82 - "Community 82"
Cohesion: 0.11
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 83 - "Community 83"
Cohesion: 0.11
Nodes (7): AtmajaGeometryAudit, Entry, Inventory, KosRoomSurvey, Phase3NavInspection, Prolog47MeshRepair, Vertex

### Community 84 - "Community 84"
Cohesion: 0.10
Nodes (3): Phase5ReviewFixture, Phase5SaveCrashProbe, ReviewDirectBoot

### Community 85 - "Community 85"
Cohesion: 0.15
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 86 - "Community 86"
Cohesion: 0.12
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 87 - "Community 87"
Cohesion: 0.10
Nodes (14): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+6 more)

### Community 88 - "Community 88"
Cohesion: 0.11
Nodes (9): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 89 - "Community 89"
Cohesion: 0.09
Nodes (18): Phase3Wewe, B, Head, Pursuing, State, W, Phase3WeweState, Ambush (+10 more)

### Community 90 - "Community 90"
Cohesion: 0.08
Nodes (24): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, Crowbar (+16 more)

### Community 92 - "Community 92"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 93 - "Community 93"
Cohesion: 0.15
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 94 - "Community 94"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 95 - "Community 95"
Cohesion: 0.14
Nodes (9): BloodWriting, Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Soundscape (+1 more)

### Community 96 - "Community 96"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 97 - "Community 97"
Cohesion: 0.20
Nodes (3): GameInputConfig, Instance, SystemShortcutHeld

### Community 98 - "Community 98"
Cohesion: 0.15
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 100 - "Community 100"
Cohesion: 0.22
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 101 - "Community 101"
Cohesion: 0.18
Nodes (3): PickupBlinkPhoto, PickupBlinkTests, PickupBlink

### Community 102 - "Community 102"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 103 - "Community 103"
Cohesion: 0.14
Nodes (5): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests

### Community 104 - "Community 104"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 105 - "Community 105"
Cohesion: 0.18
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 106 - "Community 106"
Cohesion: 0.14
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 107 - "Community 107"
Cohesion: 0.22
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 108 - "Community 108"
Cohesion: 0.19
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 109 - "Community 109"
Cohesion: 0.23
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 111 - "Community 111"
Cohesion: 0.17
Nodes (10): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed, Phase3Anchor (+2 more)

### Community 113 - "Community 113"
Cohesion: 0.15
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 114 - "Community 114"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 116 - "Community 116"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 118 - "Community 118"
Cohesion: 0.15
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 120 - "Community 120"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 122 - "Community 122"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 123 - "Community 123"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 124 - "Community 124"
Cohesion: 0.20
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 126 - "Community 126"
Cohesion: 0.18
Nodes (4): Phase3Ids, Phase3Content, Phase3Flare, Landed

### Community 130 - "Community 130"
Cohesion: 0.22
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 131 - "Community 131"
Cohesion: 0.33
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 132 - "Community 132"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 133 - "Community 133"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 134 - "Community 134"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 135 - "Community 135"
Cohesion: 0.33
Nodes (4): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId

### Community 138 - "Community 138"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 140 - "Community 140"
Cohesion: 0.25
Nodes (8): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WallCalendar, WillNote

### Community 141 - "Community 141"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 142 - "Community 142"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 149 - "Community 149"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 150 - "Community 150"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 151 - "Community 151"
Cohesion: 0.40
Nodes (3): Phase4MaterialProbe, Report, Row

### Community 152 - "Community 152"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **689 isolated node(s):** `BAD`, `NONE`, `NORMAL`, `SECRET`, `TRUE` (+684 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1097 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **37 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 41` to `Community 0`, `Community 1`, `Community 130`, `Community 6`, `Community 7`, `Community 10`, `Community 11`, `Community 12`, `Community 14`, `Community 17`, `Community 18`, `Community 19`, `Community 148`, `Community 21`, `Community 22`, `Community 23`, `Community 156`, `Community 29`, `Community 31`, `Community 37`, `Community 40`, `Community 45`, `Community 49`, `Community 50`, `Community 60`, `Community 61`, `Community 63`, `Community 66`, `Community 75`, `Community 77`, `Community 78`, `Community 82`, `Community 88`, `Community 91`, `Community 92`, `Community 93`, `Community 96`, `Community 97`, `Community 100`, `Community 108`, `Community 109`, `Community 118`, `Community 127`?**
  _High betweenness centrality (0.173) - this node is a cross-community bridge._
- **What connects `BAD`, `NONE`, `NORMAL` to the rest of the system?**
  _689 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.03615257048092869 - nodes in this community are weakly interconnected._
- **Why does `Phase4Director` connect `Community 17` to `Community 3`, `Community 6`, `Community 135`, `Community 7`, `Community 11`, `Community 12`, `Community 13`, `Community 16`, `Community 19`, `Community 21`, `Community 22`, `Community 151`, `Community 28`, `Community 29`, `Community 36`, `Community 40`, `Community 41`, `Community 60`, `Community 61`, `Community 62`, `Community 63`, `Community 64`, `Community 67`, `Community 73`, `Community 74`, `Community 75`, `Community 80`, `Community 83`, `Community 88`, `Community 92`, `Community 93`, `Community 95`, `Community 108`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.08199612061364839 - nodes in this community are weakly interconnected._
- **Why does `Phase5Director` connect `Community 0` to `Community 3`, `Community 5`, `Community 7`, `Community 10`, `Community 11`, `Community 12`, `Community 16`, `Community 19`, `Community 21`, `Community 22`, `Community 40`, `Community 41`, `Community 61`, `Community 62`, `Community 63`, `Community 64`, `Community 74`, `Community 75`, `Community 76`, `Community 84`, `Community 88`, `Community 92`, `Community 93`, `Community 95`, `Community 108`?**
  _High betweenness centrality (0.034) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.05623762376237624 - nodes in this community are weakly interconnected._
