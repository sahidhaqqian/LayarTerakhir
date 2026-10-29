# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5117 nodes · 19341 edges · 163 communities (123 shown, 40 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1269 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `5e13b5d0`
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
- Community 155
- Community 156
- Community 157
- Community 158
- Community 159
- Community 160
- Community 162

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 151 edges
2. `LayarTerakhir.Core` - 151 edges
3. `Phase5Director` - 129 edges
4. `AtmajaDirector` - 123 edges
5. `Phase4Director` - 122 edges
6. `Phase2Director` - 116 edges
7. `ShoulderCameraRig` - 115 edges
8. `Phase3Director` - 114 edges
9. `AtmajaPhoneView` - 113 edges
10. `FirstPersonController` - 110 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_fbx()`  [INFERRED]
  Tools/retarget_nyonya_motion.py → Tools/prepare_designer_vrm.py

## Import Cycles
- None detected.

## Communities (163 total, 40 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (59): asset(), download(), asset(), fetch(), dialogue_literals(), export(), fetch(), main() (+51 more)

### Community 1 - "Community 1"
Cohesion: 0.08
Nodes (8): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 2 - "Community 2"
Cohesion: 0.04
Nodes (23): DreadoutVisualRevision, Report, SceneReport, InterludeStreetCapture, Report, View, BoundsReport, Diagnosis (+15 more)

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (10): HidingAwareness, DesignerRevisionTests, FoundationAuditTests, HidingAwarenessTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests (+2 more)

### Community 4 - "Community 4"
Cohesion: 0.05
Nodes (23): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+15 more)

### Community 6 - "Community 6"
Cohesion: 0.11
Nodes (3): KosGroundWallRepair, TempShaftProbe, LayarTerakhir.Editor

### Community 7 - "Community 7"
Cohesion: 0.09
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 8 - "Community 8"
Cohesion: 0.05
Nodes (17): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+9 more)

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (33): sample_surface(), solve(), evaluate(), jacobian(), residual(), child(), clusters(), encoded() (+25 more)

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (6): CompactKosPolish, Check, KosSceneValidation, Report, KosVisualV2, FloorY

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (33): Phase4Content, Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 13 - "Community 13"
Cohesion: 0.05
Nodes (19): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, GoldenAudioTap, GoldenPlayerAudit (+11 more)

### Community 14 - "Community 14"
Cohesion: 0.05
Nodes (7): Phase3NavInspection, Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 15 - "Community 15"
Cohesion: 0.08
Nodes (7): AtmajaStairRevisionBuilder, BandVertex, DesignerVrmBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, PrologPresentationBuilder, Receipt

### Community 16 - "Community 16"
Cohesion: 0.11
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor

### Community 17 - "Community 17"
Cohesion: 0.08
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 18 - "Community 18"
Cohesion: 0.07
Nodes (11): AtmajaGeometryAudit, Entry, Inventory, KosRoomSurvey, Phase4MaterialProbe, Report, Row, PlaytestReviewValidation (+3 more)

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (12): BloodWriting, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3UvPresentation (+4 more)

### Community 20 - "Community 20"
Cohesion: 0.12
Nodes (10): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor (+2 more)

### Community 21 - "Community 21"
Cohesion: 0.10
Nodes (11): MatManifest, Phase5SceneBuilder, SourceEntry, SourceList, MatManifest, MatRow, MeshBinding, Report (+3 more)

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (3): Phase2RitualTransactionTests, Phase3CampaignTests, Phase3Campaign

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (13): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, HudController, CurrentDialogueSpeaker, CurrentObjective (+5 more)

### Community 24 - "Community 24"
Cohesion: 0.06
Nodes (30): PhoneDocument, Phase2NavigationProbe, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (11): CampaignRouting, ReviewDirectBoot, AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, ReviewDirectBootTests (+3 more)

### Community 26 - "Community 26"
Cohesion: 0.12
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 28 - "Community 28"
Cohesion: 0.07
Nodes (13): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+5 more)

### Community 29 - "Community 29"
Cohesion: 0.10
Nodes (6): Authoring, PrologCompletionBuilder, Phase5World, AtmajaEvidenceAtmosphere, AtmajaMirrorReflection, NeighborhoodRain

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (6): GoldenSceneValidation, Record, GoldenSceneTests, Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 31 - "Community 31"
Cohesion: 0.09
Nodes (4): Phase2CampaignTests, PhaseRestartTests, Slot, Phase2Campaign

### Community 32 - "Community 32"
Cohesion: 0.14
Nodes (4): Phase2PlayerAudit, NoSecret, Result, KosGroundWallTests

### Community 33 - "Community 33"
Cohesion: 0.09
Nodes (17): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+9 more)

### Community 34 - "Community 34"
Cohesion: 0.08
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 35 - "Community 35"
Cohesion: 0.13
Nodes (6): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4UvPresentation

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 37 - "Community 37"
Cohesion: 0.07
Nodes (19): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+11 more)

### Community 38 - "Community 38"
Cohesion: 0.08
Nodes (25): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+17 more)

### Community 40 - "Community 40"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+14 more)

### Community 41 - "Community 41"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (8): PlayerFootsteps, LastSurface, PlayedSteps, NyonyaMotionTests, PlayerFeelTests, AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 44 - "Community 44"
Cohesion: 0.07
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 45 - "Community 45"
Cohesion: 0.07
Nodes (13): InterludeExteriorAudit, View, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting (+5 more)

### Community 46 - "Community 46"
Cohesion: 0.16
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 47 - "Community 47"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 48 - "Community 48"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 49 - "Community 49"
Cohesion: 0.07
Nodes (14): DevelopmentScreenshotCapture, PrologKosSceneBuilder, PrologMilestoneValidator, Phase1Zone, Phase4Lighting, AtmajaVault, SmartphoneController, BlitzCooldownRemaining (+6 more)

### Community 50 - "Community 50"
Cohesion: 0.06
Nodes (9): ball(), box(), cylinder(), mat(), tube(), export(), meshes(), jagged_hole() (+1 more)

### Community 51 - "Community 51"
Cohesion: 0.10
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 52 - "Community 52"
Cohesion: 0.09
Nodes (6): FoundationAuditCoroutineHost, GameplayRecoveryRuntimeTests, RecoveryPresentationProbe, RecoveryRuntimeScenario, RecoveryTestInputSettings, RecoveryVisualEvidence

### Community 53 - "Community 53"
Cohesion: 0.10
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 54 - "Community 54"
Cohesion: 0.06
Nodes (30): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 55 - "Community 55"
Cohesion: 0.16
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 57 - "Community 57"
Cohesion: 0.09
Nodes (17): MotorcycleRiderAlignment, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity (+9 more)

### Community 58 - "Community 58"
Cohesion: 0.09
Nodes (16): Phase2Balance, LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri (+8 more)

### Community 60 - "Community 60"
Cohesion: 0.10
Nodes (4): AtmajaPlayerAudit, Receipt, Receipt, RiderPresentationAudit

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 62 - "Community 62"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 63 - "Community 63"
Cohesion: 0.12
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 64 - "Community 64"
Cohesion: 0.09
Nodes (5): Phase4SceneTests, Phase4AssetSource, Phase4ForestHazard, Phase4WorldItem, InteractableId

### Community 65 - "Community 65"
Cohesion: 0.07
Nodes (20): Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State (+12 more)

### Community 66 - "Community 66"
Cohesion: 0.14
Nodes (8): AtmajaItem, InteractableId, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 67 - "Community 67"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 68 - "Community 68"
Cohesion: 0.17
Nodes (3): InteractionInteractor, ContextResolver, CurrentTarget

### Community 69 - "Community 69"
Cohesion: 0.15
Nodes (6): Phase4PlayerAudit, Result, ScanVisualMode, AR, Normal, UV

### Community 70 - "Community 70"
Cohesion: 0.22
Nodes (3): CutVertex, PrologReviewRevision, Receipt

### Community 71 - "Community 71"
Cohesion: 0.10
Nodes (14): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+6 more)

### Community 77 - "Community 77"
Cohesion: 0.14
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 78 - "Community 78"
Cohesion: 0.13
Nodes (16): build_cup(), crack_angle(), handle(), inspect_meshes(), material(), milk_surface(), outer_radius(), shell() (+8 more)

### Community 79 - "Community 79"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 80 - "Community 80"
Cohesion: 0.13
Nodes (5): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 81 - "Community 81"
Cohesion: 0.10
Nodes (5): opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 82 - "Community 82"
Cohesion: 0.10
Nodes (6): ThreatAudioOcclusion, Occluded, Phase1Navigator, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 83 - "Community 83"
Cohesion: 0.16
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 84 - "Community 84"
Cohesion: 0.12
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 85 - "Community 85"
Cohesion: 0.13
Nodes (7): fetch(), main(), main(), prepare(), foley(), render(), write_pcm()

### Community 86 - "Community 86"
Cohesion: 0.12
Nodes (6): AmbientAudioLoop, LevelMultiplier, Phase3Sound, Phase3Soundscape, Voice, ClockAmbience

### Community 87 - "Community 87"
Cohesion: 0.13
Nodes (7): PresentationProfile, VoiceLine, VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, AtmajaSynth

### Community 88 - "Community 88"
Cohesion: 0.19
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 89 - "Community 89"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 91 - "Community 91"
Cohesion: 0.15
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 92 - "Community 92"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 93 - "Community 93"
Cohesion: 0.12
Nodes (12): cloth(), ellipsoid(), finish(), mesh(), tube(), is_arm(), main(), delete_fill_tagged() (+4 more)

### Community 95 - "Community 95"
Cohesion: 0.10
Nodes (14): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+6 more)

### Community 97 - "Community 97"
Cohesion: 0.10
Nodes (12): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+4 more)

### Community 98 - "Community 98"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 99 - "Community 99"
Cohesion: 0.16
Nodes (4): Phase5Balance, Phase5Replicas, B, Phase5UnstablePlank

### Community 100 - "Community 100"
Cohesion: 0.14
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 101 - "Community 101"
Cohesion: 0.23
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 102 - "Community 102"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 103 - "Community 103"
Cohesion: 0.15
Nodes (9): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+1 more)

### Community 104 - "Community 104"
Cohesion: 0.18
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 106 - "Community 106"
Cohesion: 0.23
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 108 - "Community 108"
Cohesion: 0.21
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 109 - "Community 109"
Cohesion: 0.17
Nodes (8): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube()

### Community 110 - "Community 110"
Cohesion: 0.21
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 114 - "Community 114"
Cohesion: 0.15
Nodes (4): Phase3Ids, Phase3Content, Phase3Flare, Landed

### Community 117 - "Community 117"
Cohesion: 0.26
Nodes (6): Metric, NyonyaMotionRevisionBuilder, Report, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 118 - "Community 118"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 119 - "Community 119"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 120 - "Community 120"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 121 - "Community 121"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 122 - "Community 122"
Cohesion: 0.18
Nodes (3): MelatiNeighborhood, Busy, Reached

### Community 123 - "Community 123"
Cohesion: 0.20
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 125 - "Community 125"
Cohesion: 0.25
Nodes (6): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 128 - "Community 128"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 129 - "Community 129"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 133 - "Community 133"
Cohesion: 0.22
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 134 - "Community 134"
Cohesion: 0.31
Nodes (4): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId

### Community 135 - "Community 135"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 136 - "Community 136"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 137 - "Community 137"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 138 - "Community 138"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 139 - "Community 139"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 140 - "Community 140"
Cohesion: 0.28
Nodes (6): Phase4Torch, Authentic, PermanentlyOff, Phase4Anchor, Phase4World, Phase4Zone

### Community 141 - "Community 141"
Cohesion: 0.42
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 142 - "Community 142"
Cohesion: 0.21
Nodes (6): Phase1Balance, KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 145 - "Community 145"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 147 - "Community 147"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 149 - "Community 149"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 153 - "Community 153"
Cohesion: 0.40
Nodes (5): FootstepSurface, Earth, Stone, Tile, Wood

### Community 154 - "Community 154"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 155 - "Community 155"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

## Knowledge Gaps
- **674 isolated node(s):** `FloorY`, `InteractableId`, `IsOpen`, `InteractableId`, `ARRevealed` (+669 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1064 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **40 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 44` to `Community 1`, `Community 2`, `Community 131`, `Community 5`, `Community 8`, `Community 10`, `Community 11`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 23`, `Community 24`, `Community 27`, `Community 29`, `Community 35`, `Community 36`, `Community 38`, `Community 40`, `Community 42`, `Community 45`, `Community 49`, `Community 50`, `Community 51`, `Community 54`, `Community 55`, `Community 56`, `Community 58`, `Community 68`, `Community 69`, `Community 80`, `Community 83`, `Community 84`, `Community 86`, `Community 87`, `Community 88`, `Community 91`, `Community 95`, `Community 98`, `Community 101`, `Community 106`, `Community 110`?**
  _High betweenness centrality (0.181) - this node is a cross-community bridge._
- **What connects `FloorY`, `InteractableId`, `IsOpen` to the rest of the system?**
  _674 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.04038095238095238 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 7` to `Community 3`, `Community 6`, `Community 8`, `Community 11`, `Community 12`, `Community 22`, `Community 24`, `Community 25`, `Community 26`, `Community 27`, `Community 31`, `Community 33`, `Community 34`, `Community 40`, `Community 48`, `Community 49`, `Community 54`, `Community 56`, `Community 66`, `Community 68`, `Community 79`, `Community 91`, `Community 95`, `Community 96`, `Community 98`?**
  _High betweenness centrality (0.081) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.07977067977067977 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 95` to `Community 1`, `Community 2`, `Community 131`, `Community 3`, `Community 5`, `Community 7`, `Community 10`, `Community 141`, `Community 15`, `Community 16`, `Community 18`, `Community 23`, `Community 29`, `Community 33`, `Community 42`, `Community 44`, `Community 45`, `Community 46`, `Community 48`, `Community 49`, `Community 51`, `Community 52`, `Community 55`, `Community 60`, `Community 66`, `Community 68`, `Community 79`, `Community 80`, `Community 83`, `Community 86`, `Community 87`, `Community 88`, `Community 102`, `Community 105`, `Community 117`?**
  _High betweenness centrality (0.057) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.04400271831464492 - nodes in this community are weakly interconnected._
