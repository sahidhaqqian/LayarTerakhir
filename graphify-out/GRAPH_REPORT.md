# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5363 nodes · 20407 edges · 153 communities (115 shown, 38 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1321 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `4de18944`
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
- Community 137
- Community 138
- Community 139
- Community 140
- Community 141
- Community 142
- Community 144
- Community 145
- Community 146
- Community 147
- Community 148
- Community 149
- Community 150
- Community 151
- Community 152

## God Nodes (most connected - your core abstractions)
1. `Phase5Director` - 129 edges
2. `CampaignState` - 128 edges
3. `Phase4Director` - 122 edges
4. `Phase2Director` - 116 edges
5. `LayarTerakhir.Core` - 115 edges
6. `Phase3Director` - 114 edges
7. `AtmajaPhoneView` - 106 edges
8. `HudController` - 105 edges
9. `AtmajaDirector` - 102 edges
10. `LayarTerakhir.Prolog` - 98 edges

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

## Communities (153 total, 38 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (51): asset(), fetch(), fetch(), main(), sha(), fetch(), fetch(), main() (+43 more)

### Community 1 - "Community 1"
Cohesion: 0.07
Nodes (13): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+5 more)

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (18): CampaignData, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data (+10 more)

### Community 3 - "Community 3"
Cohesion: 0.05
Nodes (25): AtmajaGeometryAudit, Entry, Inventory, DesignerVrmBuilder, Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests (+17 more)

### Community 4 - "Community 4"
Cohesion: 0.06
Nodes (7): Phase1Content, Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Phase1, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (11): HidingAwareness, CrosshairDotTests, DesignerRevisionTests, GoldenSceneTests, HidingAwarenessTests, InterludeCampaignTests, KosBathroomRevisionTests, KosVisualV2Tests (+3 more)

### Community 7 - "Community 7"
Cohesion: 0.07
Nodes (15): GoldenLinuxBuild, Record, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt (+7 more)

### Community 8 - "Community 8"
Cohesion: 0.11
Nodes (6): LayarTerakhir.Review, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Diagnostics

### Community 9 - "Community 9"
Cohesion: 0.08
Nodes (8): CompactKosPolish, GoldenSceneValidation, Record, Check, KosSceneValidation, Report, KosVisualV2, FloorY

### Community 10 - "Community 10"
Cohesion: 0.07
Nodes (7): Phase2Ids, Phase3Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (33): sample_surface(), solve(), evaluate(), jacobian(), residual(), child(), clusters(), encoded() (+25 more)

### Community 12 - "Community 12"
Cohesion: 0.11
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 13 - "Community 13"
Cohesion: 0.08
Nodes (4): Phase4Ids, Phase4CampaignTests, Phase4Campaign, Phase4Content

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 16 - "Community 16"
Cohesion: 0.07
Nodes (10): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen (+2 more)

### Community 18 - "Community 18"
Cohesion: 0.05
Nodes (37): PhoneDocument, Phase3Content, Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus (+29 more)

### Community 19 - "Community 19"
Cohesion: 0.07
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 20 - "Community 20"
Cohesion: 0.04
Nodes (7): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase5SceneTests, Phase1Ids, Phase5AssetSource, Phase5HumanMotion

### Community 21 - "Community 21"
Cohesion: 0.06
Nodes (28): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+20 more)

### Community 22 - "Community 22"
Cohesion: 0.06
Nodes (30): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 23 - "Community 23"
Cohesion: 0.08
Nodes (14): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey (+6 more)

### Community 24 - "Community 24"
Cohesion: 0.05
Nodes (27): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cylinder() (+19 more)

### Community 25 - "Community 25"
Cohesion: 0.05
Nodes (24): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+16 more)

### Community 26 - "Community 26"
Cohesion: 0.07
Nodes (18): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+10 more)

### Community 27 - "Community 27"
Cohesion: 0.07
Nodes (4): foley(), main(), snapshot(), main()

### Community 28 - "Community 28"
Cohesion: 0.06
Nodes (7): KosGroundWallRepair, MelatiStreetRevision, Report, Phase2NavigationProbe, PickupBlinkTests, TempShaftProbe, KosRoomCorrections

### Community 29 - "Community 29"
Cohesion: 0.05
Nodes (20): InterludeNpc, InteractableId, Phase1WorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId (+12 more)

### Community 30 - "Community 30"
Cohesion: 0.12
Nodes (3): AtmajaPlayTests, ExplorationCommentary, Zone

### Community 31 - "Community 31"
Cohesion: 0.06
Nodes (9): PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, KosGroundWallTests, RecoveryTestInputSettings, KosCorridorDirector, Busy (+1 more)

### Community 32 - "Community 32"
Cohesion: 0.07
Nodes (12): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, PrologMilestoneValidator, KamarKosPrologDirector (+4 more)

### Community 33 - "Community 33"
Cohesion: 0.08
Nodes (16): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+8 more)

### Community 34 - "Community 34"
Cohesion: 0.07
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 35 - "Community 35"
Cohesion: 0.11
Nodes (3): FoundationAuditTests, FoundationPlayAuditTests, PrologKosSceneBuilder

### Community 36 - "Community 36"
Cohesion: 0.05
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 37 - "Community 37"
Cohesion: 0.06
Nodes (19): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+11 more)

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 39 - "Community 39"
Cohesion: 0.05
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 40 - "Community 40"
Cohesion: 0.11
Nodes (7): VoiceBank, DesignerWorldRevision, KosVisualUpgrade, VisualAudit, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 41 - "Community 41"
Cohesion: 0.07
Nodes (16): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+8 more)

### Community 42 - "Community 42"
Cohesion: 0.11
Nodes (9): PresentationProfile, VoiceLine, AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud (+1 more)

### Community 44 - "Community 44"
Cohesion: 0.12
Nodes (6): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage

### Community 45 - "Community 45"
Cohesion: 0.12
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 46 - "Community 46"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 47 - "Community 47"
Cohesion: 0.15
Nodes (7): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4Zone

### Community 48 - "Community 48"
Cohesion: 0.10
Nodes (24): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+16 more)

### Community 50 - "Community 50"
Cohesion: 0.08
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 51 - "Community 51"
Cohesion: 0.18
Nodes (5): Phase2SceneBuilder, ImportedMaterial, SceneRecord, Phase2ThermalTarget, Phase2Anchor

### Community 53 - "Community 53"
Cohesion: 0.07
Nodes (15): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatManifest (+7 more)

### Community 54 - "Community 54"
Cohesion: 0.07
Nodes (20): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+12 more)

### Community 55 - "Community 55"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 56 - "Community 56"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 57 - "Community 57"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 58 - "Community 58"
Cohesion: 0.10
Nodes (12): ClockPhoto, FeastPhoto, InterludeStreetCapture, Report, View, KosPhoto, VentPryPhoto, InterludeExterior (+4 more)

### Community 59 - "Community 59"
Cohesion: 0.08
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 60 - "Community 60"
Cohesion: 0.08
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 61 - "Community 61"
Cohesion: 0.10
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (7): FoundationAuditCoroutineHost, PrologEasyPlayTests, Director, RenderContext, RecoveryPresentationProbe, RecoveryRuntimeScenario, RecoveryVisualEvidence

### Community 63 - "Community 63"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 64 - "Community 64"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 65 - "Community 65"
Cohesion: 0.13
Nodes (5): MelatiInterludeAlleyBuilder, PlaytestReviewValidation, Record, PrologEasySceneRepair, ClockAmbience

### Community 66 - "Community 66"
Cohesion: 0.10
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 67 - "Community 67"
Cohesion: 0.13
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 68 - "Community 68"
Cohesion: 0.11
Nodes (10): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+2 more)

### Community 71 - "Community 71"
Cohesion: 0.12
Nodes (4): AtmajaPlayerAudit, Receipt, Receipt, RiderPresentationAudit

### Community 72 - "Community 72"
Cohesion: 0.11
Nodes (4): Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 74 - "Community 74"
Cohesion: 0.14
Nodes (19): copy(), sha(), checked(), export(), image_path(), material_record(), source_texture(), upstream_image() (+11 more)

### Community 75 - "Community 75"
Cohesion: 0.15
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 76 - "Community 76"
Cohesion: 0.10
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 77 - "Community 77"
Cohesion: 0.12
Nodes (8): Phase3Gate, IsOpen, Phase3Wewe, B, Head, Pursuing, State, W

### Community 79 - "Community 79"
Cohesion: 0.14
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 80 - "Community 80"
Cohesion: 0.16
Nodes (4): GoldenHudBuilder, HudStyle, PhoneGlyph, StoryMapGraphic

### Community 81 - "Community 81"
Cohesion: 0.15
Nodes (3): MelatiNeighborhoodBuilder, MelatiResidenceFactory, ARPhotoGlimmer

### Community 84 - "Community 84"
Cohesion: 0.09
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 85 - "Community 85"
Cohesion: 0.11
Nodes (6): AmbientAudioLoop, LevelMultiplier, ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 87 - "Community 87"
Cohesion: 0.17
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 88 - "Community 88"
Cohesion: 0.12
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 89 - "Community 89"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 90 - "Community 90"
Cohesion: 0.08
Nodes (24): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, Crowbar (+16 more)

### Community 91 - "Community 91"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 92 - "Community 92"
Cohesion: 0.09
Nodes (19): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+11 more)

### Community 93 - "Community 93"
Cohesion: 0.12
Nodes (9): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 94 - "Community 94"
Cohesion: 0.14
Nodes (9): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape (+1 more)

### Community 95 - "Community 95"
Cohesion: 0.16
Nodes (4): Entry, Prolog47VisualTests, Folder, Survey

### Community 97 - "Community 97"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 98 - "Community 98"
Cohesion: 0.16
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 99 - "Community 99"
Cohesion: 0.22
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 100 - "Community 100"
Cohesion: 0.12
Nodes (9): Phase4Lighting, Phase4Torch, Authentic, PermanentlyOff, Phase4UvPresentation, Phase4World, Phase5SpectralPresentation, DoubleLeafGate (+1 more)

### Community 101 - "Community 101"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 102 - "Community 102"
Cohesion: 0.21
Nodes (3): DreadoutVisualRevision, Report, SceneReport

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
Cohesion: 0.16
Nodes (10): ball(), box(), cloth(), cylinder(), mat(), tube(), box(), cyl() (+2 more)

### Community 107 - "Community 107"
Cohesion: 0.15
Nodes (9): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+1 more)

### Community 110 - "Community 110"
Cohesion: 0.16
Nodes (10): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed, Phase3Anchor (+2 more)

### Community 111 - "Community 111"
Cohesion: 0.20
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 113 - "Community 113"
Cohesion: 0.20
Nodes (6): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance, Phase5Replicas, B

### Community 114 - "Community 114"
Cohesion: 0.22
Nodes (5): Phase2Room, Phase2World, HingedDoor, InteractableId, IsOpen

### Community 115 - "Community 115"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 118 - "Community 118"
Cohesion: 0.18
Nodes (4): InteractionInteractor, ContextResolver, CurrentTarget, AtmajaMirrorReflection

### Community 120 - "Community 120"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 122 - "Community 122"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 123 - "Community 123"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 126 - "Community 126"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 127 - "Community 127"
Cohesion: 0.22
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 128 - "Community 128"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 129 - "Community 129"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 130 - "Community 130"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 132 - "Community 132"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 134 - "Community 134"
Cohesion: 0.36
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 138 - "Community 138"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 139 - "Community 139"
Cohesion: 0.25
Nodes (8): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WallCalendar, WillNote

### Community 140 - "Community 140"
Cohesion: 0.36
Nodes (4): box(), cylinder(), finish(), tube()

### Community 148 - "Community 148"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 149 - "Community 149"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **689 isolated node(s):** `BackupSaveFilePath`, `Data`, `HasValidCheckpoint`, `Instance`, `SaveDirectory` (+684 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1099 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **38 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 38` to `Community 129`, `Community 2`, `Community 3`, `Community 6`, `Community 7`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 21`, `Community 24`, `Community 25`, `Community 26`, `Community 28`, `Community 29`, `Community 30`, `Community 31`, `Community 36`, `Community 39`, `Community 40`, `Community 42`, `Community 44`, `Community 46`, `Community 47`, `Community 51`, `Community 53`, `Community 54`, `Community 58`, `Community 62`, `Community 66`, `Community 67`, `Community 75`, `Community 80`, `Community 83`, `Community 85`, `Community 89`, `Community 93`, `Community 95`, `Community 97`, `Community 98`, `Community 99`, `Community 100`, `Community 117`, `Community 118`, `Community 124`?**
  _High betweenness centrality (0.204) - this node is a cross-community bridge._
- **What connects `BackupSaveFilePath`, `Data`, `HasValidCheckpoint` to the rest of the system?**
  _689 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.03553719008264463 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 1` to `Community 2`, `Community 4`, `Community 5`, `Community 10`, `Community 13`, `Community 18`, `Community 20`, `Community 21`, `Community 22`, `Community 26`, `Community 32`, `Community 36`, `Community 39`, `Community 41`, `Community 49`, `Community 54`, `Community 62`, `Community 87`, `Community 97`, `Community 98`, `Community 100`?**
  _High betweenness centrality (0.062) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.0680166147455867 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 68` to `Community 3`, `Community 100`, `Community 7`, `Community 8`, `Community 42`, `Community 85`, `Community 22`, `Community 24`, `Community 25`, `Community 28`, `Community 93`, `Community 31`?**
  _High betweenness centrality (0.052) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.051511758118701005 - nodes in this community are weakly interconnected._
