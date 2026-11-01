# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5230 nodes · 19759 edges · 165 communities (127 shown, 38 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1246 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `458d0939`
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
- Community 152
- Community 153
- Community 154
- Community 155
- Community 157
- Community 158
- Community 159
- Community 160
- Community 161
- Community 162
- Community 163

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 140 edges
2. `Phase5Director` - 129 edges
3. `Phase4Director` - 122 edges
4. `LayarTerakhir.Core` - 120 edges
5. `Phase2Director` - 116 edges
6. `Phase3Director` - 114 edges
7. `AtmajaPhoneView` - 113 edges
8. `FirstPersonController` - 113 edges
9. `LayarTerakhir.Player` - 105 edges
10. `HudController` - 100 edges

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

## Communities (165 total, 38 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.07
Nodes (13): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+5 more)

### Community 2 - "Community 2"
Cohesion: 0.04
Nodes (42): dialogue_literals(), export(), fetch(), main(), sha(), fetch(), main(), model() (+34 more)

### Community 4 - "Community 4"
Cohesion: 0.09
Nodes (7): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Diagnostics

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (23): FoundationPlayAuditTests, PrologKosSceneBuilder, PrologMilestoneValidator, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding (+15 more)

### Community 6 - "Community 6"
Cohesion: 0.07
Nodes (23): Phase1Anchor, Phase1World, HudStyle, PhoneApp, Bank, Call, Camera, Home (+15 more)

### Community 7 - "Community 7"
Cohesion: 0.07
Nodes (11): SceneAudioProfile, CompactKosPolish, GoldenSceneProduction, GoldenSceneValidation, Record, GoldenSceneTests, Check, KosSceneValidation (+3 more)

### Community 8 - "Community 8"
Cohesion: 0.06
Nodes (18): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatManifest (+10 more)

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (11): AtmajaGeometryAudit, Entry, Inventory, KosRoomSurvey, KosVisualUpgrade, VisualAudit, Phase3NavInspection, PlaytestReviewValidation (+3 more)

### Community 10 - "Community 10"
Cohesion: 0.05
Nodes (5): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 11 - "Community 11"
Cohesion: 0.05
Nodes (9): HidingAwareness, FoundationAuditTests, HidingAwarenessTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase5SceneTests (+1 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (30): Phase2Ids, Phase2NavigationProbe, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 13 - "Community 13"
Cohesion: 0.09
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (12): BloodWriting, KosGroundWallRepair, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage (+4 more)

### Community 16 - "Community 16"
Cohesion: 0.07
Nodes (28): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+20 more)

### Community 17 - "Community 17"
Cohesion: 0.07
Nodes (14): FoundationAuditCoroutineHost, InterludeStreetCapture, Report, View, Entry, Prolog47VisualTests, Folder, Survey (+6 more)

### Community 18 - "Community 18"
Cohesion: 0.08
Nodes (3): Phase2RitualTransactionTests, Phase3CampaignTests, Phase3Campaign

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder

### Community 20 - "Community 20"
Cohesion: 0.09
Nodes (10): VoiceLine, AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem (+2 more)

### Community 21 - "Community 21"
Cohesion: 0.08
Nodes (11): GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+3 more)

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (15): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+7 more)

### Community 24 - "Community 24"
Cohesion: 0.08
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 25 - "Community 25"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 26 - "Community 26"
Cohesion: 0.12
Nodes (8): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4World, Phase4Zone

### Community 27 - "Community 27"
Cohesion: 0.08
Nodes (3): main(), snapshot(), main()

### Community 28 - "Community 28"
Cohesion: 0.08
Nodes (6): DreadoutVisualRevision, Report, SceneReport, PlaytestRepairBuilder, Phase4FearPresentation, PracticalLightPresentation

### Community 29 - "Community 29"
Cohesion: 0.08
Nodes (7): DesignerWorldRevision, MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 30 - "Community 30"
Cohesion: 0.07
Nodes (24): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+16 more)

### Community 31 - "Community 31"
Cohesion: 0.06
Nodes (29): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+21 more)

### Community 32 - "Community 32"
Cohesion: 0.09
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 33 - "Community 33"
Cohesion: 0.08
Nodes (27): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+19 more)

### Community 34 - "Community 34"
Cohesion: 0.09
Nodes (4): Phase1Content, LayarTerakhir.Phase2, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 35 - "Community 35"
Cohesion: 0.15
Nodes (8): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor, Phase2Room, Phase2World

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (6): AtmajaPlayerAudit, Receipt, PlayerFeelAudit, Receipt, Sample, RecoveryTestInputSettings

### Community 37 - "Community 37"
Cohesion: 0.05
Nodes (32): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 38 - "Community 38"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 39 - "Community 39"
Cohesion: 0.10
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 40 - "Community 40"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+14 more)

### Community 41 - "Community 41"
Cohesion: 0.06
Nodes (11): ball(), box(), cylinder(), mat(), tube(), export(), meshes(), asset() (+3 more)

### Community 42 - "Community 42"
Cohesion: 0.08
Nodes (3): DesignerRevisionTests, PhaseRestartTests, Slot

### Community 43 - "Community 43"
Cohesion: 0.10
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 44 - "Community 44"
Cohesion: 0.09
Nodes (8): PresentationProfile, DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 45 - "Community 45"
Cohesion: 0.08
Nodes (11): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId, MelatiShop, InteractableId (+3 more)

### Community 46 - "Community 46"
Cohesion: 0.07
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 47 - "Community 47"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 48 - "Community 48"
Cohesion: 0.14
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 49 - "Community 49"
Cohesion: 0.08
Nodes (11): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+3 more)

### Community 50 - "Community 50"
Cohesion: 0.09
Nodes (17): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+9 more)

### Community 51 - "Community 51"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 52 - "Community 52"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 53 - "Community 53"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 54 - "Community 54"
Cohesion: 0.08
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 55 - "Community 55"
Cohesion: 0.06
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 57 - "Community 57"
Cohesion: 0.13
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 58 - "Community 58"
Cohesion: 0.14
Nodes (5): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 59 - "Community 59"
Cohesion: 0.08
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 60 - "Community 60"
Cohesion: 0.12
Nodes (4): CampaignRouting, ReviewDirectBoot, ReviewDirectBootTests, Slot

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 64 - "Community 64"
Cohesion: 0.10
Nodes (5): GameplayRecoveryRuntimeTests, KosGroundWallTests, KosCorridorDirector, Busy, CanDepart

### Community 65 - "Community 65"
Cohesion: 0.09
Nodes (4): InterludeExteriorAudit, View, Receipt, RiderPresentationAudit

### Community 66 - "Community 66"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 67 - "Community 67"
Cohesion: 0.14
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 68 - "Community 68"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 69 - "Community 69"
Cohesion: 0.12
Nodes (20): copy(), sha(), checked(), export(), image_path(), material_record(), source_texture(), upstream_image() (+12 more)

### Community 70 - "Community 70"
Cohesion: 0.12
Nodes (9): Phase4PlayerAudit, Result, Phase4Torch, Authentic, PermanentlyOff, ScanVisualMode, AR, Normal (+1 more)

### Community 71 - "Community 71"
Cohesion: 0.14
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 72 - "Community 72"
Cohesion: 0.10
Nodes (4): Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 73 - "Community 73"
Cohesion: 0.09
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 74 - "Community 74"
Cohesion: 0.08
Nodes (9): DevelopmentScreenshotCapture, Phase1LightBudget, Phase3UvPresentation, Phase4Lighting, Phase4UvPresentation, Phase5HumanMotion, Phase5SpectralPresentation, DoubleLeafGate (+1 more)

### Community 76 - "Community 76"
Cohesion: 0.15
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 77 - "Community 77"
Cohesion: 0.11
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 78 - "Community 78"
Cohesion: 0.17
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 80 - "Community 80"
Cohesion: 0.16
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 81 - "Community 81"
Cohesion: 0.11
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 82 - "Community 82"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 83 - "Community 83"
Cohesion: 0.11
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 84 - "Community 84"
Cohesion: 0.10
Nodes (5): opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 85 - "Community 85"
Cohesion: 0.13
Nodes (13): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+5 more)

### Community 88 - "Community 88"
Cohesion: 0.10
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 90 - "Community 90"
Cohesion: 0.13
Nodes (6): AmbientAudioLoop, LevelMultiplier, Bike, InterludeTraffic, Phase5Sound, Phase5Soundscape

### Community 91 - "Community 91"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 93 - "Community 93"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 94 - "Community 94"
Cohesion: 0.10
Nodes (19): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+11 more)

### Community 95 - "Community 95"
Cohesion: 0.16
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 96 - "Community 96"
Cohesion: 0.18
Nodes (4): AnimatedRakaAvatar, CinematicCrouch, Installing, MotorcycleRiderAlignment

### Community 97 - "Community 97"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 98 - "Community 98"
Cohesion: 0.19
Nodes (3): PhoneDocument, Phase2CampaignTests, Phase2Campaign

### Community 99 - "Community 99"
Cohesion: 0.16
Nodes (4): AbandonedLocationLightingTests, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 101 - "Community 101"
Cohesion: 0.16
Nodes (8): Phase3Balance, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape, Voice

### Community 102 - "Community 102"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 103 - "Community 103"
Cohesion: 0.15
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 104 - "Community 104"
Cohesion: 0.15
Nodes (8): Metric, NyonyaMotionRevisionBuilder, Report, AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 105 - "Community 105"
Cohesion: 0.16
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Navigator, Phase1Projectile

### Community 106 - "Community 106"
Cohesion: 0.26
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 109 - "Community 109"
Cohesion: 0.21
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 111 - "Community 111"
Cohesion: 0.22
Nodes (6): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance, Phase5Replicas, B

### Community 112 - "Community 112"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 113 - "Community 113"
Cohesion: 0.22
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 115 - "Community 115"
Cohesion: 0.16
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 116 - "Community 116"
Cohesion: 0.19
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 117 - "Community 117"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 118 - "Community 118"
Cohesion: 0.15
Nodes (4): Phase3Ids, Phase3Content, Phase3Flare, Landed

### Community 119 - "Community 119"
Cohesion: 0.28
Nodes (4): DesignerVrmBuilder, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 121 - "Community 121"
Cohesion: 0.22
Nodes (3): Phase4ForestHazard, Phase4Sound, Phase4Soundscape

### Community 127 - "Community 127"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 129 - "Community 129"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 130 - "Community 130"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 131 - "Community 131"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 132 - "Community 132"
Cohesion: 0.23
Nodes (5): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath

### Community 135 - "Community 135"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 136 - "Community 136"
Cohesion: 0.33
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 137 - "Community 137"
Cohesion: 0.24
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 138 - "Community 138"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 140 - "Community 140"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 141 - "Community 141"
Cohesion: 0.22
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 143 - "Community 143"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 146 - "Community 146"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 147 - "Community 147"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 148 - "Community 148"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 149 - "Community 149"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 150 - "Community 150"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 158 - "Community 158"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 159 - "Community 159"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 160 - "Community 160"
Cohesion: 0.40
Nodes (3): Phase4MaterialProbe, Report, Row

### Community 161 - "Community 161"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **679 isolated node(s):** `BackupSaveFilePath`, `Data`, `HasValidCheckpoint`, `Instance`, `SaveDirectory` (+674 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1082 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **38 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 46` to `Community 1`, `Community 4`, `Community 5`, `Community 134`, `Community 6`, `Community 8`, `Community 9`, `Community 139`, `Community 140`, `Community 13`, `Community 12`, `Community 15`, `Community 14`, `Community 17`, `Community 19`, `Community 21`, `Community 22`, `Community 23`, `Community 26`, `Community 29`, `Community 30`, `Community 32`, `Community 35`, `Community 36`, `Community 37`, `Community 40`, `Community 41`, `Community 43`, `Community 44`, `Community 45`, `Community 50`, `Community 52`, `Community 55`, `Community 56`, `Community 60`, `Community 67`, `Community 70`, `Community 73`, `Community 74`, `Community 76`, `Community 77`, `Community 80`, `Community 85`, `Community 90`, `Community 93`, `Community 116`, `Community 123`?**
  _High betweenness centrality (0.232) - this node is a cross-community bridge._
- **What connects `BackupSaveFilePath`, `Data`, `HasValidCheckpoint` to the rest of the system?**
  _679 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.070201643017177 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 0` to `Community 3`, `Community 5`, `Community 11`, `Community 12`, `Community 18`, `Community 23`, `Community 24`, `Community 25`, `Community 32`, `Community 37`, `Community 40`, `Community 42`, `Community 43`, `Community 52`, `Community 55`, `Community 60`, `Community 71`, `Community 72`, `Community 74`, `Community 82`, `Community 85`, `Community 86`, `Community 93`, `Community 98`?**
  _High betweenness centrality (0.069) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.0868185212845407 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 117` to `Community 64`, `Community 96`, `Community 26`, `Community 4`, `Community 5`, `Community 7`, `Community 9`, `Community 74`, `Community 41`, `Community 105`, `Community 90`, `Community 123`?**
  _High betweenness centrality (0.051) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.044444444444444446 - nodes in this community are weakly interconnected._
