# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5249 nodes · 19844 edges · 154 communities (118 shown, 36 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1253 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `bd92fbb5`
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
- Community 137
- Community 138
- Community 139
- Community 140
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

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 134 edges
2. `Phase5Director` - 129 edges
3. `Phase4Director` - 122 edges
4. `LayarTerakhir.Core` - 119 edges
5. `Phase2Director` - 116 edges
6. `Phase3Director` - 114 edges
7. `AtmajaPhoneView` - 112 edges
8. `FirstPersonController` - 110 edges
9. `HudController` - 105 edges
10. `LayarTerakhir.Player` - 103 edges

## Surprising Connections (you probably didn't know these)
- `delete_fill_tagged()` --indirect_call--> `mesh()`  [INFERRED]
  Tools/repair_nyonya_appearance.py → Tools/author_atmaja_character.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py

## Import Cycles
- None detected.

## Communities (154 total, 36 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (49): Phase4Banaspati, B, LungeProgress, Pursuing, State, W, Phase4Content, Phase4Director (+41 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (20): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+12 more)

### Community 2 - "Community 2"
Cohesion: 0.04
Nodes (47): asset(), fetch(), dialogue_literals(), export(), fetch(), main(), sha(), fetch() (+39 more)

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (10): HidingAwareness, DeveloperReviewModeTests, HidingAwarenessTests, KosBathroomRevisionTests, Phase4SceneTests, Phase5SceneTests, Phase4AssetSource, Phase4WorldItem (+2 more)

### Community 6 - "Community 6"
Cohesion: 0.05
Nodes (55): sample_surface(), solve(), evaluate(), jacobian(), residual(), child(), clusters(), encoded() (+47 more)

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (9): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Phase2, LayarTerakhir.Interlude, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone (+1 more)

### Community 8 - "Community 8"
Cohesion: 0.05
Nodes (18): CampaignData, PhoneDocument, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered (+10 more)

### Community 9 - "Community 9"
Cohesion: 0.05
Nodes (22): PlayerFootsteps, LastSurface, PlayedSteps, AtmajaGeometryAudit, Entry, Inventory, DesignerVrmBuilder, NyonyaMotionTests (+14 more)

### Community 10 - "Community 10"
Cohesion: 0.05
Nodes (5): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (6): Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 12 - "Community 12"
Cohesion: 0.05
Nodes (12): KosGroundWallRepair, Entry, PlaytestReviewValidation, Record, Authoring, PrologCompletionBuilder, TempShaftProbe, Phase1Anchor (+4 more)

### Community 13 - "Community 13"
Cohesion: 0.07
Nodes (9): SceneAudioProfile, CompactKosPolish, GoldenSceneProduction, GoldenSceneValidation, Record, GoldenSceneTests, KosVisualV2, FloorY (+1 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (17): MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, MatManifest, MatRow, MeshBinding (+9 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 16 - "Community 16"
Cohesion: 0.05
Nodes (30): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+22 more)

### Community 17 - "Community 17"
Cohesion: 0.09
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder

### Community 18 - "Community 18"
Cohesion: 0.10
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 19 - "Community 19"
Cohesion: 0.07
Nodes (4): foley(), main(), snapshot(), main()

### Community 21 - "Community 21"
Cohesion: 0.08
Nodes (12): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen (+4 more)

### Community 22 - "Community 22"
Cohesion: 0.08
Nodes (14): BloodWriting, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3EvpSource (+6 more)

### Community 23 - "Community 23"
Cohesion: 0.06
Nodes (11): GameplayRecoveryRuntimeTests, KosGroundWallTests, Entry, Prolog47VisualTests, Folder, Survey, RecoveryVisualEvidence, InteractionInteractor (+3 more)

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (5): AtmajaStairRevisionBuilder, BandVertex, DesignerWorldRevision, KosVisualUpgrade, VisualAudit

### Community 25 - "Community 25"
Cohesion: 0.10
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 26 - "Community 26"
Cohesion: 0.10
Nodes (11): Bindings, MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Bindings, Phase4ForestHazard (+3 more)

### Community 27 - "Community 27"
Cohesion: 0.05
Nodes (32): Phase2NavigationProbe, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+24 more)

### Community 28 - "Community 28"
Cohesion: 0.06
Nodes (16): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+8 more)

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 30 - "Community 30"
Cohesion: 0.10
Nodes (11): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey (+3 more)

### Community 31 - "Community 31"
Cohesion: 0.06
Nodes (4): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Ids

### Community 32 - "Community 32"
Cohesion: 0.09
Nodes (12): VoiceBank, KosCorridorBuilder, MelatiStreetRevision, Report, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, Bike (+4 more)

### Community 33 - "Community 33"
Cohesion: 0.05
Nodes (10): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+2 more)

### Community 34 - "Community 34"
Cohesion: 0.06
Nodes (14): InterludeNpc, InteractableId, Phase1WorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId (+6 more)

### Community 35 - "Community 35"
Cohesion: 0.06
Nodes (26): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+18 more)

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (3): DesignerRevisionTests, PhaseRestartTests, Slot

### Community 37 - "Community 37"
Cohesion: 0.10
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (31): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 39 - "Community 39"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 40 - "Community 40"
Cohesion: 0.10
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 41 - "Community 41"
Cohesion: 0.11
Nodes (26): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+18 more)

### Community 42 - "Community 42"
Cohesion: 0.09
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (25): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+17 more)

### Community 45 - "Community 45"
Cohesion: 0.09
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 46 - "Community 46"
Cohesion: 0.06
Nodes (9): ball(), box(), cylinder(), mat(), tube(), export(), meshes(), jagged_hole() (+1 more)

### Community 47 - "Community 47"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 48 - "Community 48"
Cohesion: 0.07
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 50 - "Community 50"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 51 - "Community 51"
Cohesion: 0.06
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 52 - "Community 52"
Cohesion: 0.12
Nodes (9): PresentationProfile, VoiceLine, AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud (+1 more)

### Community 53 - "Community 53"
Cohesion: 0.10
Nodes (6): MelatiNeighborhoodBuilder, MelatiResidenceFactory, Phase3NavInspection, Phase1LightBudget, PracticalLightPresentation, AtmajaEvidenceAtmosphere

### Community 54 - "Community 54"
Cohesion: 0.10
Nodes (22): copy(), sha(), get(), main(), checked(), export(), image_path(), material_record() (+14 more)

### Community 55 - "Community 55"
Cohesion: 0.08
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 56 - "Community 56"
Cohesion: 0.08
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 57 - "Community 57"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 59 - "Community 59"
Cohesion: 0.16
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 60 - "Community 60"
Cohesion: 0.13
Nodes (3): MelatiInterludeAlleyBuilder, PrologEasySceneRepair, ARPhotoGlimmer

### Community 61 - "Community 61"
Cohesion: 0.07
Nodes (15): Phase3Content, Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+7 more)

### Community 62 - "Community 62"
Cohesion: 0.09
Nodes (11): PrologKosSceneBuilder, PrologMilestoneValidator, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting (+3 more)

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 65 - "Community 65"
Cohesion: 0.12
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 66 - "Community 66"
Cohesion: 0.14
Nodes (6): FoundationAuditCoroutineHost, PrologEasyPlayTests, Director, RenderContext, RecoveryPresentationProbe, RecoveryRuntimeScenario

### Community 69 - "Community 69"
Cohesion: 0.14
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 70 - "Community 70"
Cohesion: 0.10
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 74 - "Community 74"
Cohesion: 0.10
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 75 - "Community 75"
Cohesion: 0.11
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 77 - "Community 77"
Cohesion: 0.09
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 78 - "Community 78"
Cohesion: 0.11
Nodes (10): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, ReviewFlyCamera, Active, Eye (+2 more)

### Community 79 - "Community 79"
Cohesion: 0.16
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 80 - "Community 80"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 81 - "Community 81"
Cohesion: 0.16
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 84 - "Community 84"
Cohesion: 0.12
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 85 - "Community 85"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 86 - "Community 86"
Cohesion: 0.18
Nodes (3): GoldenHudBuilder, PhoneGlyph, StoryMapGraphic

### Community 87 - "Community 87"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 88 - "Community 88"
Cohesion: 0.13
Nodes (12): InterludeStreetCapture, Report, View, BoundsReport, Diagnosis, KosRenderDiagnosis, Metric, KosVisualCapture (+4 more)

### Community 89 - "Community 89"
Cohesion: 0.14
Nodes (6): AmbientAudioLoop, LevelMultiplier, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 90 - "Community 90"
Cohesion: 0.17
Nodes (3): AtmajaPlayerAudit, Receipt, RumahAtmajaDiningTests

### Community 91 - "Community 91"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 92 - "Community 92"
Cohesion: 0.11
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 93 - "Community 93"
Cohesion: 0.15
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 94 - "Community 94"
Cohesion: 0.12
Nodes (7): DevelopmentScreenshotCapture, Phase3UvPresentation, Phase4Lighting, Phase4UvPresentation, Phase5SpectralPresentation, DoubleLeafGate, IsOpen

### Community 96 - "Community 96"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 97 - "Community 97"
Cohesion: 0.17
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 98 - "Community 98"
Cohesion: 0.18
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 99 - "Community 99"
Cohesion: 0.12
Nodes (8): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached, MelatiShop, InteractableId, IsOpen

### Community 100 - "Community 100"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 101 - "Community 101"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 102 - "Community 102"
Cohesion: 0.14
Nodes (3): Phase1Sound, Phase1Soundscape, Phase1Zone

### Community 103 - "Community 103"
Cohesion: 0.16
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 106 - "Community 106"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 108 - "Community 108"
Cohesion: 0.19
Nodes (6): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance, Phase5Replicas, B

### Community 110 - "Community 110"
Cohesion: 0.17
Nodes (8): Phase3Balance, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape, Voice

### Community 112 - "Community 112"
Cohesion: 0.21
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 113 - "Community 113"
Cohesion: 0.26
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 114 - "Community 114"
Cohesion: 0.29
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 115 - "Community 115"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 122 - "Community 122"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 123 - "Community 123"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 124 - "Community 124"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 125 - "Community 125"
Cohesion: 0.24
Nodes (3): GoldenLinuxBuild, Record, SceneRecord

### Community 127 - "Community 127"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 128 - "Community 128"
Cohesion: 0.33
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 129 - "Community 129"
Cohesion: 0.24
Nodes (3): ThreatAudioOcclusion, Occluded, Phase1Projectile

### Community 131 - "Community 131"
Cohesion: 0.20
Nodes (8): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneRitualPower, RitualCameraPower

### Community 132 - "Community 132"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 133 - "Community 133"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 134 - "Community 134"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 135 - "Community 135"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 136 - "Community 136"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 137 - "Community 137"
Cohesion: 0.36
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 140 - "Community 140"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 143 - "Community 143"
Cohesion: 0.40
Nodes (4): Phase2ThermalTarget, Phase2Anchor, Phase2Room, Phase2World

### Community 144 - "Community 144"
Cohesion: 0.40
Nodes (5): FootstepSurface, Earth, Stone, Tile, Wood

### Community 145 - "Community 145"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 146 - "Community 146"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 147 - "Community 147"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 148 - "Community 148"
Cohesion: 0.40
Nodes (5): Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 150 - "Community 150"
Cohesion: 0.40
Nodes (5): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid

## Knowledge Gaps
- **681 isolated node(s):** `B`, `LungeProgress`, `Pursuing`, `State`, `W` (+676 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1085 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **36 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 35` to `Community 0`, `Community 133`, `Community 5`, `Community 7`, `Community 9`, `Community 141`, `Community 14`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 24`, `Community 26`, `Community 27`, `Community 28`, `Community 34`, `Community 37`, `Community 38`, `Community 39`, `Community 40`, `Community 46`, `Community 51`, `Community 52`, `Community 58`, `Community 61`, `Community 62`, `Community 65`, `Community 66`, `Community 69`, `Community 70`, `Community 71`, `Community 79`, `Community 81`, `Community 85`, `Community 86`, `Community 89`, `Community 93`, `Community 94`, `Community 96`, `Community 113`, `Community 125`, `Community 126`?**
  _High betweenness centrality (0.210) - this node is a cross-community bridge._
- **What connects `B`, `LungeProgress`, `Pursuing` to the rest of the system?**
  _681 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.03589232303090728 - nodes in this community are weakly interconnected._
- **Why does `Phase5Director` connect `Community 38` to `Community 1`, `Community 3`, `Community 131`, `Community 133`, `Community 7`, `Community 9`, `Community 10`, `Community 138`, `Community 12`, `Community 14`, `Community 15`, `Community 17`, `Community 21`, `Community 22`, `Community 35`, `Community 57`, `Community 59`, `Community 61`, `Community 62`, `Community 65`, `Community 70`, `Community 72`, `Community 83`, `Community 85`, `Community 89`, `Community 93`, `Community 94`, `Community 104`, `Community 108`, `Community 113`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.061936936936936936 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 1` to `Community 0`, `Community 3`, `Community 4`, `Community 131`, `Community 8`, `Community 11`, `Community 25`, `Community 27`, `Community 28`, `Community 29`, `Community 36`, `Community 38`, `Community 40`, `Community 51`, `Community 58`, `Community 61`, `Community 62`, `Community 80`, `Community 93`, `Community 94`, `Community 96`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.03882063882063882 - nodes in this community are weakly interconnected._
