# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5449 nodes · 20979 edges · 178 communities (128 shown, 50 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1322 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `8214a1d1`
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
- Community 149
- Community 150
- Community 151
- Community 152
- Community 153
- Community 154
- Community 155
- Community 156
- Community 157
- Community 158
- Community 159
- Community 160
- Community 162
- Community 163
- Community 164
- Community 165
- Community 166
- Community 167
- Community 168
- Community 169
- Community 170
- Community 171
- Community 173
- Community 174
- Community 175
- Community 177

## God Nodes (most connected - your core abstractions)
1. `LayarTerakhir.Core` - 155 edges
2. `CampaignState` - 154 edges
3. `Phase5Director` - 129 edges
4. `Phase4Director` - 122 edges
5. `FirstPersonController` - 119 edges
6. `Phase2Director` - 116 edges
7. `Phase3Director` - 114 edges
8. `HudController` - 114 edges
9. `ShoulderCameraRig` - 114 edges
10. `AtmajaPhoneView` - 113 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase5WorldItem` --implements--> `IInteractable`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Phase5/Phase5WorldItem.cs → Assets/_LayarTerakhir/src/Player/IInteractable.cs

## Import Cycles
- None detected.

## Communities (178 total, 50 thin omitted)

### Community 1 - "Community 1"
Cohesion: 0.04
Nodes (58): asset(), download(), asset(), fetch(), dialogue_literals(), export(), copy(), sha() (+50 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (8): LayarTerakhir.Review, LayarTerakhir.Phase2, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (10): DesignerRevisionTests, FoundationAuditTests, KosBathroomRevisionTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase5SceneTests, RumahAtmajaDiningTests (+2 more)

### Community 4 - "Community 4"
Cohesion: 0.04
Nodes (11): asset(), download(), load_meta(), export(), foley(), main(), snapshot(), note() (+3 more)

### Community 7 - "Community 7"
Cohesion: 0.08
Nodes (6): CompactKosPolish, KosVisualV2, FloorY, PickupBlinkPhoto, PrologEasySceneRepair, Phase4ForestHazard

### Community 8 - "Community 8"
Cohesion: 0.08
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (14): AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport (+6 more)

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 11 - "Community 11"
Cohesion: 0.05
Nodes (34): Phase4Content, Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+26 more)

### Community 12 - "Community 12"
Cohesion: 0.04
Nodes (28): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+20 more)

### Community 13 - "Community 13"
Cohesion: 0.05
Nodes (5): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (34): PhoneDocument, Phase3Content, Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus (+26 more)

### Community 15 - "Community 15"
Cohesion: 0.08
Nodes (12): PresentationProfile, CrosshairDotTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 16 - "Community 16"
Cohesion: 0.08
Nodes (6): Phase3Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (17): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+9 more)

### Community 18 - "Community 18"
Cohesion: 0.06
Nodes (37): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+29 more)

### Community 19 - "Community 19"
Cohesion: 0.11
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor

### Community 20 - "Community 20"
Cohesion: 0.05
Nodes (22): _fbx_of_U(), main(), sample_surface(), solve(), evaluate(), jacobian(), residual(), tip() (+14 more)

### Community 21 - "Community 21"
Cohesion: 0.04
Nodes (31): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+23 more)

### Community 22 - "Community 22"
Cohesion: 0.07
Nodes (32): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+24 more)

### Community 23 - "Community 23"
Cohesion: 0.08
Nodes (15): ClockPhoto, InterludeStreetCapture, Report, View, KosPhoto, BoundsReport, Diagnosis, Entry (+7 more)

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (19): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+11 more)

### Community 25 - "Community 25"
Cohesion: 0.09
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 26 - "Community 26"
Cohesion: 0.08
Nodes (6): MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (12): MatManifest, Phase5SceneBuilder, SourceEntry, SourceList, MatManifest, MatRow, MeshBinding, Report (+4 more)

### Community 28 - "Community 28"
Cohesion: 0.08
Nodes (11): AtmajaGeometryAudit, Entry, Inventory, DesignerVrmBuilder, Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests (+3 more)

### Community 29 - "Community 29"
Cohesion: 0.06
Nodes (19): InterludeContent, InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel (+11 more)

### Community 30 - "Community 30"
Cohesion: 0.06
Nodes (13): InterludeNpc, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId (+5 more)

### Community 31 - "Community 31"
Cohesion: 0.12
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 32 - "Community 32"
Cohesion: 0.10
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 33 - "Community 33"
Cohesion: 0.13
Nodes (7): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4Zone

### Community 34 - "Community 34"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 35 - "Community 35"
Cohesion: 0.09
Nodes (17): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+9 more)

### Community 36 - "Community 36"
Cohesion: 0.11
Nodes (7): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3UvPresentation

### Community 37 - "Community 37"
Cohesion: 0.07
Nodes (19): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+11 more)

### Community 38 - "Community 38"
Cohesion: 0.15
Nodes (4): Phase2PlayerAudit, NoSecret, Result, KosGroundWallTests

### Community 39 - "Community 39"
Cohesion: 0.09
Nodes (19): KuntiState, Dormant, Manifested, Pursuit, Resolved, Phase1Threats, Balance, BossStatus (+11 more)

### Community 40 - "Community 40"
Cohesion: 0.10
Nodes (11): FoundationAuditCoroutineHost, Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext (+3 more)

### Community 41 - "Community 41"
Cohesion: 0.11
Nodes (8): VoiceLine, AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 42 - "Community 42"
Cohesion: 0.06
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 43 - "Community 43"
Cohesion: 0.08
Nodes (12): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+4 more)

### Community 44 - "Community 44"
Cohesion: 0.08
Nodes (6): AtmajaReviewDressing, KosGroundWallRepair, KosRoomSurvey, PlaytestReviewValidation, Record, KosRoomCorrections

### Community 45 - "Community 45"
Cohesion: 0.10
Nodes (11): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+3 more)

### Community 46 - "Community 46"
Cohesion: 0.09
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 47 - "Community 47"
Cohesion: 0.10
Nodes (5): PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, RecoveryTestInputSettings

### Community 48 - "Community 48"
Cohesion: 0.15
Nodes (4): FoundationPlayAuditTests, InteractionInteractor, ContextResolver, CurrentTarget

### Community 49 - "Community 49"
Cohesion: 0.13
Nodes (3): KosVisualUpgrade, VisualAudit, PrologKosSceneBuilder

### Community 50 - "Community 50"
Cohesion: 0.12
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 51 - "Community 51"
Cohesion: 0.10
Nodes (4): Phase2Ids, Phase2Drawer, Prompt, State

### Community 52 - "Community 52"
Cohesion: 0.08
Nodes (11): InterludeExteriorAudit, View, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting (+3 more)

### Community 53 - "Community 53"
Cohesion: 0.14
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 54 - "Community 54"
Cohesion: 0.07
Nodes (26): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+18 more)

### Community 56 - "Community 56"
Cohesion: 0.12
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 57 - "Community 57"
Cohesion: 0.13
Nodes (4): PrologMilestoneValidator, KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 58 - "Community 58"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 59 - "Community 59"
Cohesion: 0.11
Nodes (8): DeveloperReviewModeTests, ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 60 - "Community 60"
Cohesion: 0.23
Nodes (4): Phase2SceneBuilder, ImportedMaterial, Phase2ThermalTarget, Phase2Anchor

### Community 61 - "Community 61"
Cohesion: 0.09
Nodes (4): Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 63 - "Community 63"
Cohesion: 0.10
Nodes (10): Phase4Banaspati, B, LungeProgress, Pursuing, State, W, Phase4Torch, Authentic (+2 more)

### Community 64 - "Community 64"
Cohesion: 0.09
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 66 - "Community 66"
Cohesion: 0.10
Nodes (4): AtmajaPlayerAudit, Receipt, Receipt, RiderPresentationAudit

### Community 67 - "Community 67"
Cohesion: 0.10
Nodes (8): Frame, LocationLightingAudit, Receipt, AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 68 - "Community 68"
Cohesion: 0.16
Nodes (4): FeastPhoto, Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 69 - "Community 69"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 70 - "Community 70"
Cohesion: 0.15
Nodes (6): Phase4PlayerAudit, Result, ScanVisualMode, AR, Normal, UV

### Community 71 - "Community 71"
Cohesion: 0.13
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 72 - "Community 72"
Cohesion: 0.09
Nodes (9): DevelopmentScreenshotCapture, Phase4Lighting, ClockAmbience, MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached (+1 more)

### Community 74 - "Community 74"
Cohesion: 0.14
Nodes (4): CampaignRouting, ReviewDirectBoot, ReviewDirectBootTests, Slot

### Community 76 - "Community 76"
Cohesion: 0.15
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 77 - "Community 77"
Cohesion: 0.11
Nodes (12): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Wewe, B (+4 more)

### Community 78 - "Community 78"
Cohesion: 0.16
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 80 - "Community 80"
Cohesion: 0.08
Nodes (22): Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining (+14 more)

### Community 81 - "Community 81"
Cohesion: 0.14
Nodes (6): Check, KosSceneValidation, Report, KosVisualV2Tests, KosVisualCapture, VisualTelemetryReport

### Community 82 - "Community 82"
Cohesion: 0.11
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 83 - "Community 83"
Cohesion: 0.11
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 84 - "Community 84"
Cohesion: 0.08
Nodes (26): AtmajaAction, BathCurtain, BathScratches, CarpetLift, CellarClimb, Charge, Clock, ClockHands (+18 more)

### Community 85 - "Community 85"
Cohesion: 0.19
Nodes (18): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+10 more)

### Community 86 - "Community 86"
Cohesion: 0.13
Nodes (7): AmbientAudioLoop, LevelMultiplier, SceneAudioProfile, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 87 - "Community 87"
Cohesion: 0.18
Nodes (4): GoldenHudBuilder, HudStyle, PhoneGlyph, StoryMapGraphic

### Community 88 - "Community 88"
Cohesion: 0.14
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 89 - "Community 89"
Cohesion: 0.13
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 90 - "Community 90"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 91 - "Community 91"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 93 - "Community 93"
Cohesion: 0.21
Nodes (3): GameInputConfig, Instance, SystemShortcutHeld

### Community 94 - "Community 94"
Cohesion: 0.16
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 95 - "Community 95"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 96 - "Community 96"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 97 - "Community 97"
Cohesion: 0.15
Nodes (5): BloodWriting, Phase3Sound, Phase3Soundscape, Voice, AtmajaWorldText

### Community 100 - "Community 100"
Cohesion: 0.18
Nodes (8): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto

### Community 101 - "Community 101"
Cohesion: 0.14
Nodes (6): GoldenLinuxBuild, Record, Phase2NavigationProbe, SceneRecord, Phase2Room, Phase2World

### Community 102 - "Community 102"
Cohesion: 0.14
Nodes (5): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests

### Community 103 - "Community 103"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 104 - "Community 104"
Cohesion: 0.22
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 106 - "Community 106"
Cohesion: 0.15
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 107 - "Community 107"
Cohesion: 0.16
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 108 - "Community 108"
Cohesion: 0.15
Nodes (10): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed, Phase3Anchor (+2 more)

### Community 109 - "Community 109"
Cohesion: 0.21
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 110 - "Community 110"
Cohesion: 0.17
Nodes (4): opening_structure_sha256(), verify_opening_source(), GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 111 - "Community 111"
Cohesion: 0.16
Nodes (7): fetch(), fetch_asset(), get(), main(), subdir(), asset(), fetch()

### Community 114 - "Community 114"
Cohesion: 0.17
Nodes (4): InterludeTraffic, KosCorridorDirector, Busy, CanDepart

### Community 115 - "Community 115"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 118 - "Community 118"
Cohesion: 0.26
Nodes (3): AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 122 - "Community 122"
Cohesion: 0.29
Nodes (4): HingedDoor, InteractableId, IsOpen, MazeDoor

### Community 124 - "Community 124"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 125 - "Community 125"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 126 - "Community 126"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 127 - "Community 127"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 128 - "Community 128"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 130 - "Community 130"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 131 - "Community 131"
Cohesion: 0.35
Nodes (8): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow()

### Community 132 - "Community 132"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 133 - "Community 133"
Cohesion: 0.24
Nodes (6): apply_pose(), build_map(), fk_chain(), main(), render_to(), snapshot_action_bases()

### Community 135 - "Community 135"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 136 - "Community 136"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 137 - "Community 137"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 138 - "Community 138"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 139 - "Community 139"
Cohesion: 0.33
Nodes (3): RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 142 - "Community 142"
Cohesion: 0.28
Nodes (4): VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 144 - "Community 144"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 146 - "Community 146"
Cohesion: 0.36
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 147 - "Community 147"
Cohesion: 0.25
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 151 - "Community 151"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 153 - "Community 153"
Cohesion: 0.25
Nodes (8): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WallCalendar, WillNote

### Community 156 - "Community 156"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 163 - "Community 163"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 164 - "Community 164"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 166 - "Community 166"
Cohesion: 0.40
Nodes (5): Phase2DrawerState, Closed, Gas, Spent, Telegraph

## Knowledge Gaps
- **691 isolated node(s):** `InteractableId`, `ActionBusy`, `Analyzer`, `CameraCanScan`, `CameraLocked` (+686 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1117 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **50 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 42` to `Community 129`, `Community 2`, `Community 6`, `Community 7`, `Community 10`, `Community 11`, `Community 14`, `Community 15`, `Community 18`, `Community 19`, `Community 148`, `Community 21`, `Community 23`, `Community 24`, `Community 25`, `Community 26`, `Community 27`, `Community 30`, `Community 33`, `Community 34`, `Community 35`, `Community 36`, `Community 40`, `Community 41`, `Community 168`, `Community 43`, `Community 44`, `Community 47`, `Community 48`, `Community 50`, `Community 51`, `Community 52`, `Community 54`, `Community 56`, `Community 57`, `Community 63`, `Community 67`, `Community 70`, `Community 72`, `Community 78`, `Community 81`, `Community 83`, `Community 86`, `Community 87`, `Community 88`, `Community 90`, `Community 93`, `Community 94`, `Community 95`, `Community 101`, `Community 106`?**
  _High betweenness centrality (0.181) - this node is a cross-community bridge._
- **What connects `InteractableId`, `ActionBusy`, `Analyzer` to the rest of the system?**
  _691 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07225396825396825 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 8` to `Community 3`, `Community 5`, `Community 10`, `Community 11`, `Community 14`, `Community 16`, `Community 17`, `Community 21`, `Community 24`, `Community 29`, `Community 31`, `Community 34`, `Community 35`, `Community 41`, `Community 43`, `Community 47`, `Community 48`, `Community 54`, `Community 56`, `Community 57`, `Community 59`, `Community 61`, `Community 65`, `Community 71`, `Community 72`, `Community 74`, `Community 88`, `Community 90`?**
  _High betweenness centrality (0.075) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.039184326269492205 - nodes in this community are weakly interconnected._
- **Why does `Phase4Director` connect `Community 11` to `Community 2`, `Community 6`, `Community 7`, `Community 8`, `Community 9`, `Community 13`, `Community 15`, `Community 17`, `Community 19`, `Community 21`, `Community 25`, `Community 26`, `Community 30`, `Community 33`, `Community 42`, `Community 52`, `Community 58`, `Community 61`, `Community 62`, `Community 63`, `Community 65`, `Community 67`, `Community 70`, `Community 72`, `Community 75`, `Community 77`, `Community 80`, `Community 81`, `Community 86`, `Community 88`, `Community 97`, `Community 106`?**
  _High betweenness centrality (0.047) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.07676161919040479 - nodes in this community are weakly interconnected._
