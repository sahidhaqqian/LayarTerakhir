# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5372 nodes · 20415 edges · 150 communities (112 shown, 38 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1283 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `36d6db35`
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
- Community 82
- Community 83
- Community 84
- Community 85
- Community 86
- Community 87
- Community 88
- Community 89
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
- Community 116
- Community 117
- Community 118
- Community 119
- Community 120
- Community 121
- Community 122
- Community 123
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

## God Nodes (most connected - your core abstractions)
1. `Phase5Director` - 129 edges
2. `CampaignState` - 128 edges
3. `Phase4Director` - 122 edges
4. `Phase2Director` - 116 edges
5. `LayarTerakhir.Core` - 115 edges
6. `Phase3Director` - 114 edges
7. `AtmajaPhoneView` - 106 edges
8. `HudController` - 104 edges
9. `AtmajaDirector` - 101 edges
10. `LayarTerakhir.Player` - 97 edges

## Surprising Connections (you probably didn't know these)
- `build()` --calls--> `export_fbx()`  [INFERRED]
  Tools/retarget_nyonya_motion.py → Tools/prepare_designer_vrm.py
- `delete_fill_tagged()` --indirect_call--> `mesh()`  [INFERRED]
  Tools/repair_nyonya_appearance.py → Tools/author_atmaja_character.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py

## Import Cycles
- None detected.

## Communities (150 total, 38 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (12): Phase5Content, KosDialogue, LayarTerakhir.Review, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Phase4, LayarTerakhir.Core, LayarTerakhir.Phase5 (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.03
Nodes (11): DesignerRevisionTests, KosBathroomRevisionTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase4SceneTests, Phase5SceneTests (+3 more)

### Community 2 - "Community 2"
Cohesion: 0.04
Nodes (51): asset(), fetch(), fetch(), main(), sha(), fetch(), fetch(), main() (+43 more)

### Community 4 - "Community 4"
Cohesion: 0.04
Nodes (21): DesignerVrmBuilder, Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests, PlayerFeelTests, PrologEasySceneRepair, PrologPresentationBuilder (+13 more)

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (4): Phase5Ids, Phase5CampaignTests, Slot, Phase5Campaign

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (12): AtmajaGeometryAudit, Entry, Inventory, CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, Check (+4 more)

### Community 8 - "Community 8"
Cohesion: 0.06
Nodes (12): CrosshairDotTests, FoundationAuditTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (18): CampaignData, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data (+10 more)

### Community 10 - "Community 10"
Cohesion: 0.05
Nodes (25): ClockPhoto, DreadoutVisualRevision, Report, SceneReport, InterludeStreetCapture, Report, View, KosPhoto (+17 more)

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (33): sample_surface(), solve(), evaluate(), jacobian(), residual(), child(), clusters(), encoded() (+25 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (37): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+29 more)

### Community 13 - "Community 13"
Cohesion: 0.06
Nodes (28): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+20 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (6): Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (18): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatManifest (+10 more)

### Community 17 - "Community 17"
Cohesion: 0.05
Nodes (27): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cylinder() (+19 more)

### Community 18 - "Community 18"
Cohesion: 0.06
Nodes (9): KosGroundWallRepair, KosRoomSurvey, Phase2NavigationProbe, PlaytestReviewValidation, Record, RumahAtmajaDiningTests, TempShaftProbe, Phase4ForestHazard (+1 more)

### Community 19 - "Community 19"
Cohesion: 0.05
Nodes (18): AmbientAudioLoop, LevelMultiplier, FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps (+10 more)

### Community 20 - "Community 20"
Cohesion: 0.09
Nodes (9): FeastPhoto, MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4World (+1 more)

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (4): foley(), main(), snapshot(), main()

### Community 22 - "Community 22"
Cohesion: 0.05
Nodes (20): Phase1WorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosInteractableItem, InteractableId (+12 more)

### Community 23 - "Community 23"
Cohesion: 0.09
Nodes (12): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, Phase4MaterialProbe (+4 more)

### Community 24 - "Community 24"
Cohesion: 0.05
Nodes (20): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+12 more)

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (10): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, RecoveryPresentationProbe, RecoveryRuntimeScenario (+2 more)

### Community 26 - "Community 26"
Cohesion: 0.10
Nodes (16): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorKind, FragileDrop (+8 more)

### Community 27 - "Community 27"
Cohesion: 0.07
Nodes (11): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+3 more)

### Community 28 - "Community 28"
Cohesion: 0.08
Nodes (6): MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 29 - "Community 29"
Cohesion: 0.08
Nodes (15): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+7 more)

### Community 30 - "Community 30"
Cohesion: 0.10
Nodes (9): AtmajaHandycamPlayback, IsPlaying, Progress, PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer (+1 more)

### Community 31 - "Community 31"
Cohesion: 0.07
Nodes (16): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+8 more)

### Community 32 - "Community 32"
Cohesion: 0.11
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder

### Community 34 - "Community 34"
Cohesion: 0.06
Nodes (19): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+11 more)

### Community 35 - "Community 35"
Cohesion: 0.11
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (14): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+6 more)

### Community 37 - "Community 37"
Cohesion: 0.15
Nodes (8): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor, Phase2Room, Phase2World

### Community 38 - "Community 38"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 39 - "Community 39"
Cohesion: 0.07
Nodes (29): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+21 more)

### Community 40 - "Community 40"
Cohesion: 0.08
Nodes (13): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+5 more)

### Community 41 - "Community 41"
Cohesion: 0.10
Nodes (10): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, Bike, InterludeTraffic, KosCorridorDirector (+2 more)

### Community 42 - "Community 42"
Cohesion: 0.11
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 43 - "Community 43"
Cohesion: 0.06
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 44 - "Community 44"
Cohesion: 0.14
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 45 - "Community 45"
Cohesion: 0.06
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 46 - "Community 46"
Cohesion: 0.10
Nodes (24): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+16 more)

### Community 48 - "Community 48"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase2Hub, CameraLocked (+14 more)

### Community 49 - "Community 49"
Cohesion: 0.09
Nodes (19): KuntiState, Dormant, Manifested, Pursuit, Resolved, Phase1Threats, Balance, BossStatus (+11 more)

### Community 50 - "Community 50"
Cohesion: 0.08
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 51 - "Community 51"
Cohesion: 0.09
Nodes (5): PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, RecoveryTestInputSettings

### Community 52 - "Community 52"
Cohesion: 0.11
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 53 - "Community 53"
Cohesion: 0.11
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 54 - "Community 54"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 55 - "Community 55"
Cohesion: 0.05
Nodes (32): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 56 - "Community 56"
Cohesion: 0.08
Nodes (9): AtmajaPlayerAudit, Receipt, AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone, ExplorationCommentary (+1 more)

### Community 57 - "Community 57"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 58 - "Community 58"
Cohesion: 0.11
Nodes (6): GoldenHudBuilder, HudStyle, PhoneGlyph, PhoneSkin, Rounded, StoryMapGraphic

### Community 59 - "Community 59"
Cohesion: 0.11
Nodes (4): InterludeCampaignTests, InterludeCampaign, InterludeContent, InterludeIds

### Community 60 - "Community 60"
Cohesion: 0.06
Nodes (31): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 62 - "Community 62"
Cohesion: 0.13
Nodes (7): Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext

### Community 63 - "Community 63"
Cohesion: 0.08
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 64 - "Community 64"
Cohesion: 0.10
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 65 - "Community 65"
Cohesion: 0.10
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 66 - "Community 66"
Cohesion: 0.13
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 67 - "Community 67"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 69 - "Community 69"
Cohesion: 0.10
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 71 - "Community 71"
Cohesion: 0.12
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 72 - "Community 72"
Cohesion: 0.11
Nodes (7): DeveloperReviewModeTests, InteractionInteractor, ContextResolver, CurrentTarget, ReviewFlyCamera, Active, Eye

### Community 74 - "Community 74"
Cohesion: 0.10
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 75 - "Community 75"
Cohesion: 0.14
Nodes (19): copy(), sha(), checked(), export(), image_path(), material_record(), source_texture(), upstream_image() (+11 more)

### Community 76 - "Community 76"
Cohesion: 0.14
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 78 - "Community 78"
Cohesion: 0.15
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 80 - "Community 80"
Cohesion: 0.10
Nodes (9): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 83 - "Community 83"
Cohesion: 0.13
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 84 - "Community 84"
Cohesion: 0.11
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 85 - "Community 85"
Cohesion: 0.11
Nodes (3): GraphicsReferenceAudit, Receipt, KosGroundWallTests

### Community 86 - "Community 86"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 87 - "Community 87"
Cohesion: 0.09
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 88 - "Community 88"
Cohesion: 0.12
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 89 - "Community 89"
Cohesion: 0.11
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 91 - "Community 91"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 92 - "Community 92"
Cohesion: 0.15
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 93 - "Community 93"
Cohesion: 0.09
Nodes (17): Phase1Balance, Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering (+9 more)

### Community 95 - "Community 95"
Cohesion: 0.12
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 96 - "Community 96"
Cohesion: 0.16
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 97 - "Community 97"
Cohesion: 0.12
Nodes (10): Phase2Content, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+2 more)

### Community 98 - "Community 98"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 99 - "Community 99"
Cohesion: 0.14
Nodes (7): DevelopmentScreenshotCapture, FoundationAuditCoroutineHost, Phase1Zone, Phase4Lighting, AtmajaVault, DoubleLeafGate, IsOpen

### Community 100 - "Community 100"
Cohesion: 0.16
Nodes (3): Phase4Content, Phase4WorldItem, InteractableId

### Community 102 - "Community 102"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 103 - "Community 103"
Cohesion: 0.16
Nodes (10): ball(), box(), cloth(), cylinder(), mat(), tube(), box(), cyl() (+2 more)

### Community 104 - "Community 104"
Cohesion: 0.15
Nodes (9): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+1 more)

### Community 105 - "Community 105"
Cohesion: 0.15
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 106 - "Community 106"
Cohesion: 0.21
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 111 - "Community 111"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 116 - "Community 116"
Cohesion: 0.18
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 117 - "Community 117"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 118 - "Community 118"
Cohesion: 0.29
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 119 - "Community 119"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 121 - "Community 121"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 123 - "Community 123"
Cohesion: 0.25
Nodes (6): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 125 - "Community 125"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 126 - "Community 126"
Cohesion: 0.33
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 129 - "Community 129"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 130 - "Community 130"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 131 - "Community 131"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 133 - "Community 133"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 134 - "Community 134"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 135 - "Community 135"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 137 - "Community 137"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 138 - "Community 138"
Cohesion: 0.25
Nodes (8): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WallCalendar, WillNote

### Community 139 - "Community 139"
Cohesion: 0.36
Nodes (4): box(), cylinder(), finish(), tube()

### Community 145 - "Community 145"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 146 - "Community 146"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 148 - "Community 148"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **690 isolated node(s):** `InteractableId`, `ARRevealed`, `BasementEntry`, `BasementStealth`, `Chase` (+685 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1100 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **38 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 45` to `Community 0`, `Community 131`, `Community 3`, `Community 4`, `Community 8`, `Community 9`, `Community 10`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 148`, `Community 22`, `Community 24`, `Community 26`, `Community 28`, `Community 30`, `Community 31`, `Community 37`, `Community 38`, `Community 39`, `Community 42`, `Community 43`, `Community 48`, `Community 51`, `Community 55`, `Community 58`, `Community 60`, `Community 62`, `Community 66`, `Community 69`, `Community 70`, `Community 78`, `Community 80`, `Community 85`, `Community 88`, `Community 89`, `Community 96`, `Community 97`, `Community 99`, `Community 106`?**
  _High betweenness centrality (0.178) - this node is a cross-community bridge._
- **What connects `InteractableId`, `ARRevealed`, `BasementEntry` to the rest of the system?**
  _690 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.054409642644936765 - nodes in this community are weakly interconnected._
- **Why does `Phase2Director` connect `Community 12` to `Community 0`, `Community 131`, `Community 4`, `Community 8`, `Community 9`, `Community 140`, `Community 14`, `Community 142`, `Community 16`, `Community 18`, `Community 148`, `Community 22`, `Community 24`, `Community 26`, `Community 27`, `Community 30`, `Community 37`, `Community 38`, `Community 42`, `Community 45`, `Community 48`, `Community 57`, `Community 67`, `Community 69`, `Community 80`, `Community 88`, `Community 93`, `Community 97`, `Community 99`, `Community 113`?**
  _High betweenness centrality (0.058) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.02642343836373687 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 27` to `Community 1`, `Community 5`, `Community 6`, `Community 9`, `Community 12`, `Community 13`, `Community 14`, `Community 24`, `Community 25`, `Community 31`, `Community 43`, `Community 44`, `Community 48`, `Community 55`, `Community 59`, `Community 60`, `Community 86`, `Community 96`, `Community 99`, `Community 106`, `Community 115`?**
  _High betweenness centrality (0.050) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.03553719008264463 - nodes in this community are weakly interconnected._
