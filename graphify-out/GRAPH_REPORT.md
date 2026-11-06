# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5437 nodes · 20953 edges · 168 communities (124 shown, 44 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1316 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `fafd51e0`
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
- Community 145
- Community 146
- Community 147
- Community 148
- Community 149
- Community 150
- Community 152
- Community 153
- Community 154
- Community 155
- Community 156
- Community 157
- Community 158
- Community 159
- Community 160
- Community 161
- Community 162
- Community 163
- Community 165
- Community 166
- Community 167

## God Nodes (most connected - your core abstractions)
1. `LayarTerakhir.Core` - 157 edges
2. `CampaignState` - 155 edges
3. `Phase5Director` - 129 edges
4. `FirstPersonController` - 123 edges
5. `Phase4Director` - 122 edges
6. `AtmajaDirector` - 119 edges
7. `Phase2Director` - 116 edges
8. `LayarTerakhir.Player` - 116 edges
9. `HudController` - 115 edges
10. `AtmajaPhoneView` - 115 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase5CampaignTests` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/Phase5CampaignTests.cs → Assets/_LayarTerakhir/src/Core/CampaignState.cs

## Import Cycles
- None detected.

## Communities (168 total, 44 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (58): asset(), download(), asset(), fetch(), dialogue_literals(), export(), copy(), sha() (+50 more)

### Community 2 - "Community 2"
Cohesion: 0.03
Nodes (12): HidingAwareness, DesignerRevisionTests, HidingAwarenessTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase5SceneTests (+4 more)

### Community 3 - "Community 3"
Cohesion: 0.06
Nodes (9): AtmajaReviewDressing, CompactKosPolish, GoldenSceneProduction, KosRoomSurvey, KosVisualV2, FloorY, PickupBlinkPhoto, PrologEasySceneRepair (+1 more)

### Community 4 - "Community 4"
Cohesion: 0.04
Nodes (11): asset(), download(), load_meta(), export(), foley(), main(), snapshot(), note() (+3 more)

### Community 5 - "Community 5"
Cohesion: 0.04
Nodes (25): AtmajaGeometryAudit, Entry, Inventory, DesignerVrmBuilder, Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests (+17 more)

### Community 7 - "Community 7"
Cohesion: 0.05
Nodes (19): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+11 more)

### Community 8 - "Community 8"
Cohesion: 0.10
Nodes (3): KosGroundWallRepair, TempShaftProbe, LayarTerakhir.Editor

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (7): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (5): Phase5Data, Phase5Ids, Phase5CampaignTests, Slot, Phase5Campaign

### Community 11 - "Community 11"
Cohesion: 0.05
Nodes (20): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+12 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (13): PresentationProfile, CrosshairDotTests, FoundationAuditTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying (+5 more)

### Community 13 - "Community 13"
Cohesion: 0.05
Nodes (6): Phase5Content, LayarTerakhir.Review, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (12): AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport (+4 more)

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (9): CampaignData, CampaignRouting, Phase2Data, Phase5ReviewFixture, Phase5SaveCrashProbe, ReviewDirectBoot, DeveloperReviewModeTests, ReviewDirectBootTests (+1 more)

### Community 16 - "Community 16"
Cohesion: 0.06
Nodes (31): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (14): ClockPhoto, DreadoutVisualRevision, Report, SceneReport, KosPhoto, BoundsReport, Entry, KosRenderDiagnosis (+6 more)

### Community 18 - "Community 18"
Cohesion: 0.04
Nodes (28): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+20 more)

### Community 19 - "Community 19"
Cohesion: 0.06
Nodes (31): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+23 more)

### Community 20 - "Community 20"
Cohesion: 0.07
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (16): MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, MatManifest, MatRow, MeshBinding (+8 more)

### Community 22 - "Community 22"
Cohesion: 0.06
Nodes (28): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+20 more)

### Community 23 - "Community 23"
Cohesion: 0.06
Nodes (37): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+29 more)

### Community 24 - "Community 24"
Cohesion: 0.11
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor

### Community 25 - "Community 25"
Cohesion: 0.05
Nodes (22): _fbx_of_U(), main(), sample_surface(), solve(), evaluate(), jacobian(), residual(), tip() (+14 more)

### Community 26 - "Community 26"
Cohesion: 0.07
Nodes (32): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+24 more)

### Community 27 - "Community 27"
Cohesion: 0.09
Nodes (5): Phase2Ids, Phase3Ids, Phase2RitualTransactionTests, Phase3CampaignTests, Phase3Campaign

### Community 28 - "Community 28"
Cohesion: 0.10
Nodes (8): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4World, Phase4Zone

### Community 29 - "Community 29"
Cohesion: 0.08
Nodes (5): MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 31 - "Community 31"
Cohesion: 0.08
Nodes (5): InterludeCampaignTests, PhaseRestartTests, Slot, InterludeCampaign, InterludeIds

### Community 32 - "Community 32"
Cohesion: 0.08
Nodes (4): Phase1Content, LayarTerakhir.Phase2, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 33 - "Community 33"
Cohesion: 0.07
Nodes (19): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+11 more)

### Community 34 - "Community 34"
Cohesion: 0.06
Nodes (17): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, InteractionInteractor, ContextResolver, CurrentTarget, MelatiDeparture (+9 more)

### Community 35 - "Community 35"
Cohesion: 0.07
Nodes (13): AmbientAudioLoop, LevelMultiplier, KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts (+5 more)

### Community 36 - "Community 36"
Cohesion: 0.09
Nodes (10): Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext, RecoveryPresentationProbe (+2 more)

### Community 37 - "Community 37"
Cohesion: 0.15
Nodes (8): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor, Phase2Room, Phase2World

### Community 38 - "Community 38"
Cohesion: 0.07
Nodes (19): PlayerFeelAudit, Receipt, Sample, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint (+11 more)

### Community 39 - "Community 39"
Cohesion: 0.10
Nodes (6): FeastPhoto, Authoring, PrologCompletionBuilder, InterludeExterior, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 40 - "Community 40"
Cohesion: 0.09
Nodes (15): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance, Phase5Boss, Active, B, Exposed (+7 more)

### Community 41 - "Community 41"
Cohesion: 0.13
Nodes (9): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Anchor, Phase3World (+1 more)

### Community 42 - "Community 42"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 43 - "Community 43"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 44 - "Community 44"
Cohesion: 0.05
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 45 - "Community 45"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 46 - "Community 46"
Cohesion: 0.09
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 47 - "Community 47"
Cohesion: 0.06
Nodes (33): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 48 - "Community 48"
Cohesion: 0.07
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 49 - "Community 49"
Cohesion: 0.09
Nodes (5): AtmajaPlayerAudit, Receipt, GameplayRecoveryRuntimeTests, DoubleLeafGate, IsOpen

### Community 50 - "Community 50"
Cohesion: 0.07
Nodes (13): GraphicsReferenceAudit, Receipt, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting (+5 more)

### Community 51 - "Community 51"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 52 - "Community 52"
Cohesion: 0.11
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 53 - "Community 53"
Cohesion: 0.12
Nodes (5): KamarKosPrologDirector, KosInteractableItem, InteractableId, PrologSceneFlow, CanContinue

### Community 54 - "Community 54"
Cohesion: 0.07
Nodes (18): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+10 more)

### Community 55 - "Community 55"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 56 - "Community 56"
Cohesion: 0.11
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 59 - "Community 59"
Cohesion: 0.09
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 60 - "Community 60"
Cohesion: 0.09
Nodes (5): Phase4SceneTests, Phase4AssetSource, Phase4ForestHazard, Phase4WorldItem, InteractableId

### Community 61 - "Community 61"
Cohesion: 0.11
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 63 - "Community 63"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 65 - "Community 65"
Cohesion: 0.13
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 66 - "Community 66"
Cohesion: 0.17
Nodes (6): KosCorridorBuilder, KosCorridorDirector, Busy, CanDepart, KosTravelItem, InteractableId

### Community 67 - "Community 67"
Cohesion: 0.11
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 70 - "Community 70"
Cohesion: 0.12
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 71 - "Community 71"
Cohesion: 0.15
Nodes (4): Phase3Gate, IsOpen, Phase3WorldItem, InteractableId

### Community 73 - "Community 73"
Cohesion: 0.11
Nodes (18): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+10 more)

### Community 74 - "Community 74"
Cohesion: 0.08
Nodes (11): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+3 more)

### Community 77 - "Community 77"
Cohesion: 0.10
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 78 - "Community 78"
Cohesion: 0.08
Nodes (26): AtmajaAction, BathCurtain, BathScratches, CarpetLift, CellarClimb, Charge, Clock, ClockHands (+18 more)

### Community 79 - "Community 79"
Cohesion: 0.19
Nodes (18): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+10 more)

### Community 80 - "Community 80"
Cohesion: 0.17
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 81 - "Community 81"
Cohesion: 0.12
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 82 - "Community 82"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 83 - "Community 83"
Cohesion: 0.18
Nodes (4): GoldenHudBuilder, HudStyle, PhoneGlyph, StoryMapGraphic

### Community 84 - "Community 84"
Cohesion: 0.14
Nodes (9): InterludeStreetCapture, Report, View, Diagnosis, Check, KosSceneValidation, Report, KosVisualCapture (+1 more)

### Community 85 - "Community 85"
Cohesion: 0.11
Nodes (10): PrologKosSceneBuilder, PrologMilestoneValidator, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower (+2 more)

### Community 86 - "Community 86"
Cohesion: 0.08
Nodes (22): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+14 more)

### Community 87 - "Community 87"
Cohesion: 0.14
Nodes (11): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+3 more)

### Community 88 - "Community 88"
Cohesion: 0.13
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 90 - "Community 90"
Cohesion: 0.17
Nodes (3): PhoneDocument, Phase2CampaignTests, Phase2Campaign

### Community 91 - "Community 91"
Cohesion: 0.17
Nodes (6): DevelopmentScreenshotCapture, FoundationAuditCoroutineHost, HingedDoor, InteractableId, IsOpen, MazeDoor

### Community 92 - "Community 92"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 94 - "Community 94"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 95 - "Community 95"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 96 - "Community 96"
Cohesion: 0.18
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 97 - "Community 97"
Cohesion: 0.15
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 98 - "Community 98"
Cohesion: 0.18
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 99 - "Community 99"
Cohesion: 0.21
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 100 - "Community 100"
Cohesion: 0.17
Nodes (4): opening_structure_sha256(), verify_opening_source(), GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 101 - "Community 101"
Cohesion: 0.16
Nodes (7): fetch(), fetch_asset(), get(), main(), subdir(), asset(), fetch()

### Community 102 - "Community 102"
Cohesion: 0.18
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 104 - "Community 104"
Cohesion: 0.29
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 105 - "Community 105"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 109 - "Community 109"
Cohesion: 0.19
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 110 - "Community 110"
Cohesion: 0.22
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 113 - "Community 113"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 114 - "Community 114"
Cohesion: 0.20
Nodes (5): VoiceLine, VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 115 - "Community 115"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 116 - "Community 116"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 117 - "Community 117"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 118 - "Community 118"
Cohesion: 0.23
Nodes (5): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath

### Community 119 - "Community 119"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 120 - "Community 120"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 121 - "Community 121"
Cohesion: 0.35
Nodes (8): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow()

### Community 122 - "Community 122"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 123 - "Community 123"
Cohesion: 0.24
Nodes (6): apply_pose(), build_map(), fk_chain(), main(), render_to(), snapshot_action_bases()

### Community 125 - "Community 125"
Cohesion: 0.29
Nodes (6): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 126 - "Community 126"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 128 - "Community 128"
Cohesion: 0.20
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 129 - "Community 129"
Cohesion: 0.36
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 130 - "Community 130"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 131 - "Community 131"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 132 - "Community 132"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 134 - "Community 134"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 136 - "Community 136"
Cohesion: 0.36
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 137 - "Community 137"
Cohesion: 0.29
Nodes (3): PlaytestReviewValidation, Record, ClockAmbience

### Community 139 - "Community 139"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 142 - "Community 142"
Cohesion: 0.25
Nodes (8): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WallCalendar, WillNote

### Community 143 - "Community 143"
Cohesion: 0.29
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 149 - "Community 149"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 153 - "Community 153"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 154 - "Community 154"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 155 - "Community 155"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 156 - "Community 156"
Cohesion: 0.40
Nodes (3): Phase4MaterialProbe, Report, Row

### Community 157 - "Community 157"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **691 isolated node(s):** `Slot`, `AwaitingBlitz`, `AwaitingPhotos`, `Campaign`, `FollowsPlayerInBasement` (+686 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1116 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **44 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 48` to `Community 3`, `Community 6`, `Community 7`, `Community 9`, `Community 11`, `Community 12`, `Community 16`, `Community 17`, `Community 144`, `Community 19`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 24`, `Community 28`, `Community 29`, `Community 35`, `Community 36`, `Community 37`, `Community 38`, `Community 41`, `Community 43`, `Community 44`, `Community 47`, `Community 50`, `Community 53`, `Community 55`, `Community 58`, `Community 66`, `Community 73`, `Community 74`, `Community 80`, `Community 81`, `Community 82`, `Community 83`, `Community 84`, `Community 85`, `Community 87`, `Community 91`, `Community 92`, `Community 99`, `Community 102`?**
  _High betweenness centrality (0.214) - this node is a cross-community bridge._
- **What connects `Slot`, `AwaitingBlitz`, `AwaitingPhotos` to the rest of the system?**
  _691 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.039184326269492205 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 33` to `Community 2`, `Community 7`, `Community 8`, `Community 10`, `Community 11`, `Community 12`, `Community 15`, `Community 16`, `Community 22`, `Community 27`, `Community 30`, `Community 31`, `Community 36`, `Community 44`, `Community 47`, `Community 54`, `Community 58`, `Community 61`, `Community 62`, `Community 73`, `Community 74`, `Community 77`, `Community 85`, `Community 87`, `Community 89`, `Community 90`, `Community 91`, `Community 92`, `Community 99`?**
  _High betweenness centrality (0.058) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.07520661157024794 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 7` to `Community 1`, `Community 2`, `Community 3`, `Community 5`, `Community 6`, `Community 137`, `Community 9`, `Community 11`, `Community 12`, `Community 14`, `Community 17`, `Community 20`, `Community 24`, `Community 33`, `Community 35`, `Community 36`, `Community 39`, `Community 48`, `Community 49`, `Community 50`, `Community 55`, `Community 61`, `Community 80`, `Community 82`, `Community 85`, `Community 91`, `Community 93`, `Community 95`?**
  _High betweenness centrality (0.057) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.03381795195954488 - nodes in this community are weakly interconnected._
