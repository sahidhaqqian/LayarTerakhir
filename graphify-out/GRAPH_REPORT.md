# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5210 nodes · 19679 edges · 168 communities (120 shown, 48 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1246 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `c2b40890`
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
- Community 136
- Community 137
- Community 138
- Community 139
- Community 140
- Community 141
- Community 142
- Community 143
- Community 144
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
- Community 161
- Community 162
- Community 163
- Community 164
- Community 165
- Community 166
- Community 167

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 140 edges
2. `Phase5Director` - 129 edges
3. `Phase4Director` - 122 edges
4. `LayarTerakhir.Core` - 120 edges
5. `Phase2Director` - 116 edges
6. `AtmajaPhoneView` - 116 edges
7. `Phase3Director` - 114 edges
8. `FirstPersonController` - 113 edges
9. `LayarTerakhir.Player` - 105 edges
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

## Communities (168 total, 48 thin omitted)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (47): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+39 more)

### Community 3 - "Community 3"
Cohesion: 0.09
Nodes (7): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Diagnostics

### Community 4 - "Community 4"
Cohesion: 0.08
Nodes (4): Phase5Ids, Phase5CampaignTests, Slot, Phase5Campaign

### Community 5 - "Community 5"
Cohesion: 0.04
Nodes (11): HidingAwareness, HidingAwarenessTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase2SceneTests, Count (+3 more)

### Community 6 - "Community 6"
Cohesion: 0.05
Nodes (4): foley(), main(), snapshot(), main()

### Community 7 - "Community 7"
Cohesion: 0.07
Nodes (18): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+10 more)

### Community 8 - "Community 8"
Cohesion: 0.09
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 9 - "Community 9"
Cohesion: 0.05
Nodes (9): PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, KosGroundWallTests, RecoveryTestInputSettings, KosCorridorDirector, Busy (+1 more)

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (10): CompactKosPolish, GoldenSceneProduction, GoldenSceneValidation, Record, GoldenSceneTests, Check, KosSceneValidation, Report (+2 more)

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 12 - "Community 12"
Cohesion: 0.09
Nodes (14): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor (+6 more)

### Community 13 - "Community 13"
Cohesion: 0.06
Nodes (5): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 14 - "Community 14"
Cohesion: 0.09
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 15 - "Community 15"
Cohesion: 0.09
Nodes (12): GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+4 more)

### Community 16 - "Community 16"
Cohesion: 0.05
Nodes (4): ReviewDirectBoot, DeveloperReviewModeTests, ReviewDirectBootTests, Slot

### Community 17 - "Community 17"
Cohesion: 0.09
Nodes (4): Phase2CampaignTests, Phase2RitualTransactionTests, Phase2Campaign, Phase3Campaign

### Community 18 - "Community 18"
Cohesion: 0.09
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder

### Community 19 - "Community 19"
Cohesion: 0.07
Nodes (11): Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext, RecoveryPresentationProbe (+3 more)

### Community 20 - "Community 20"
Cohesion: 0.06
Nodes (32): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+24 more)

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (21): CampaignData, PhoneDocument, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered (+13 more)

### Community 22 - "Community 22"
Cohesion: 0.07
Nodes (18): VoiceLine, AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden (+10 more)

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (27): Phase2Ids, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+19 more)

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 25 - "Community 25"
Cohesion: 0.11
Nodes (9): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, Prolog47MeshRepair (+1 more)

### Community 26 - "Community 26"
Cohesion: 0.09
Nodes (32): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+24 more)

### Community 28 - "Community 28"
Cohesion: 0.06
Nodes (27): sample_surface(), solve(), evaluate(), jacobian(), residual(), add_phone(), apparition(), bind() (+19 more)

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 30 - "Community 30"
Cohesion: 0.12
Nodes (11): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Gate, IsOpen (+3 more)

### Community 31 - "Community 31"
Cohesion: 0.11
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 32 - "Community 32"
Cohesion: 0.06
Nodes (29): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+21 more)

### Community 33 - "Community 33"
Cohesion: 0.09
Nodes (4): Phase1Content, LayarTerakhir.Phase2, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 34 - "Community 34"
Cohesion: 0.09
Nodes (6): Phase3NavInspection, Authoring, PrologCompletionBuilder, InterludeExterior, Phase1LightBudget, PracticalLightPresentation

### Community 35 - "Community 35"
Cohesion: 0.07
Nodes (8): KosGroundWallRepair, KosRoomSurvey, MelatiStreetRevision, Report, PlaytestReviewValidation, Record, TempShaftProbe, KosRoomCorrections

### Community 36 - "Community 36"
Cohesion: 0.07
Nodes (10): PrologCompletionAudit, Campaign, Hud, Phone, Receipt, Receipt, RiderPresentationAudit, InteractionInteractor (+2 more)

### Community 37 - "Community 37"
Cohesion: 0.13
Nodes (6): MatRow, MeshBinding, Phase4SceneBuilder, Usage, Phase4Anchor, Phase4Zone

### Community 38 - "Community 38"
Cohesion: 0.05
Nodes (32): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 39 - "Community 39"
Cohesion: 0.08
Nodes (5): DreadoutVisualRevision, Report, SceneReport, PlaytestRepairBuilder, Phase4FearPresentation

### Community 40 - "Community 40"
Cohesion: 0.05
Nodes (10): box(), cyl(), finish(), tube(), export(), meshes(), box(), cylinder() (+2 more)

### Community 41 - "Community 41"
Cohesion: 0.12
Nodes (9): Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, Phase5MechanicalPresentation, Phase5Anchor, Phase5World (+1 more)

### Community 42 - "Community 42"
Cohesion: 0.11
Nodes (4): MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, ARPhotoGlimmer

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (12): InterludeContent, Phase2Content, Phase4Content, PhoneApp, Bank, Call, Camera, Home (+4 more)

### Community 44 - "Community 44"
Cohesion: 0.09
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 45 - "Community 45"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase2Hub, CameraLocked (+14 more)

### Community 46 - "Community 46"
Cohesion: 0.09
Nodes (15): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance, Phase5Boss, Active, B, Exposed (+7 more)

### Community 47 - "Community 47"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 48 - "Community 48"
Cohesion: 0.11
Nodes (6): VoiceBank, DesignerWorldRevision, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 49 - "Community 49"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 50 - "Community 50"
Cohesion: 0.07
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 51 - "Community 51"
Cohesion: 0.08
Nodes (11): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosInteractableItem, InteractableId, MelatiShop, InteractableId (+3 more)

### Community 54 - "Community 54"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 55 - "Community 55"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 56 - "Community 56"
Cohesion: 0.08
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 57 - "Community 57"
Cohesion: 0.12
Nodes (23): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+15 more)

### Community 58 - "Community 58"
Cohesion: 0.08
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 59 - "Community 59"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 60 - "Community 60"
Cohesion: 0.13
Nodes (3): Phase3CampaignTests, PhaseRestartTests, Slot

### Community 61 - "Community 61"
Cohesion: 0.11
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 64 - "Community 64"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 65 - "Community 65"
Cohesion: 0.06
Nodes (30): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 66 - "Community 66"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 67 - "Community 67"
Cohesion: 0.09
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 68 - "Community 68"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 69 - "Community 69"
Cohesion: 0.10
Nodes (8): PresentationProfile, AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone, ExplorationCommentary, Zone

### Community 71 - "Community 71"
Cohesion: 0.09
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 72 - "Community 72"
Cohesion: 0.09
Nodes (14): PrologKosSceneBuilder, PrologMilestoneValidator, ScanVisualMode, AR, Normal, UV, SmartphoneController, BlitzCooldownRemaining (+6 more)

### Community 74 - "Community 74"
Cohesion: 0.11
Nodes (7): Frame, LocationLightingAudit, Receipt, AbandonedLocationLightingTests, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 75 - "Community 75"
Cohesion: 0.11
Nodes (4): PlayerFeelTests, AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 78 - "Community 78"
Cohesion: 0.09
Nodes (12): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+4 more)

### Community 80 - "Community 80"
Cohesion: 0.14
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 82 - "Community 82"
Cohesion: 0.10
Nodes (6): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource, Phase4ForestHazard

### Community 83 - "Community 83"
Cohesion: 0.16
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 84 - "Community 84"
Cohesion: 0.13
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 85 - "Community 85"
Cohesion: 0.09
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 88 - "Community 88"
Cohesion: 0.11
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 89 - "Community 89"
Cohesion: 0.15
Nodes (6): Phase2Sound, Phase2Soundscape, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 90 - "Community 90"
Cohesion: 0.12
Nodes (9): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 91 - "Community 91"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 92 - "Community 92"
Cohesion: 0.10
Nodes (10): FoundationAuditCoroutineHost, Phase3UvPresentation, Phase4Lighting, Phase4UvPresentation, Phase5SpectralPresentation, AtmajaEvidenceAtmosphere, AtmajaVault, ClockAmbience (+2 more)

### Community 93 - "Community 93"
Cohesion: 0.13
Nodes (8): AtmajaHandycamPlayback, IsPlaying, Progress, PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 95 - "Community 95"
Cohesion: 0.15
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 96 - "Community 96"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 98 - "Community 98"
Cohesion: 0.18
Nodes (10): InterludeStreetCapture, Report, View, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+2 more)

### Community 101 - "Community 101"
Cohesion: 0.20
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 102 - "Community 102"
Cohesion: 0.12
Nodes (14): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+6 more)

### Community 103 - "Community 103"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 104 - "Community 104"
Cohesion: 0.17
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 105 - "Community 105"
Cohesion: 0.18
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 106 - "Community 106"
Cohesion: 0.15
Nodes (8): Metric, NyonyaMotionRevisionBuilder, Report, AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 107 - "Community 107"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 108 - "Community 108"
Cohesion: 0.17
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 110 - "Community 110"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 111 - "Community 111"
Cohesion: 0.27
Nodes (4): DesignerVrmBuilder, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 113 - "Community 113"
Cohesion: 0.16
Nodes (8): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Lighting

### Community 116 - "Community 116"
Cohesion: 0.16
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 117 - "Community 117"
Cohesion: 0.19
Nodes (3): AmbientAudioLoop, LevelMultiplier, SceneAudioProfile

### Community 118 - "Community 118"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 122 - "Community 122"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 124 - "Community 124"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 125 - "Community 125"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 126 - "Community 126"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 128 - "Community 128"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 129 - "Community 129"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 130 - "Community 130"
Cohesion: 0.24
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 131 - "Community 131"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 133 - "Community 133"
Cohesion: 0.22
Nodes (4): Phase1Anchor, Phase1World, Phase1WorldItem, InteractableId

### Community 134 - "Community 134"
Cohesion: 0.29
Nodes (6): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 137 - "Community 137"
Cohesion: 0.36
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 138 - "Community 138"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 139 - "Community 139"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 140 - "Community 140"
Cohesion: 0.22
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 141 - "Community 141"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 142 - "Community 142"
Cohesion: 0.22
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 143 - "Community 143"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 148 - "Community 148"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 150 - "Community 150"
Cohesion: 0.36
Nodes (4): box(), cylinder(), finish(), tube()

### Community 151 - "Community 151"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 152 - "Community 152"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 158 - "Community 158"
Cohesion: 0.60
Nodes (3): AtmajaGeometryAudit, Entry, Inventory

### Community 159 - "Community 159"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 160 - "Community 160"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **679 isolated node(s):** `FloorY`, `CameraLocked`, `CameraStatus`, `Campaign`, `CanBeginLive` (+674 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1080 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **48 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 50` to `Community 0`, `Community 3`, `Community 132`, `Community 7`, `Community 8`, `Community 9`, `Community 135`, `Community 11`, `Community 12`, `Community 15`, `Community 17`, `Community 145`, `Community 19`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 27`, `Community 157`, `Community 30`, `Community 31`, `Community 32`, `Community 35`, `Community 37`, `Community 38`, `Community 41`, `Community 42`, `Community 43`, `Community 45`, `Community 47`, `Community 49`, `Community 54`, `Community 61`, `Community 62`, `Community 65`, `Community 67`, `Community 72`, `Community 74`, `Community 76`, `Community 78`, `Community 89`, `Community 90`, `Community 92`, `Community 95`, `Community 97`, `Community 101`, `Community 113`?**
  _High betweenness centrality (0.216) - this node is a cross-community bridge._
- **What connects `FloorY`, `CameraLocked`, `CameraStatus` to the rest of the system?**
  _679 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08833047782219684 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 118` to `Community 3`, `Community 35`, `Community 136`, `Community 9`, `Community 75`, `Community 20`, `Community 117`, `Community 22`, `Community 89`, `Community 90`, `Community 92`, `Community 30`?**
  _High betweenness centrality (0.056) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.047401484865790974 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 7` to `Community 0`, `Community 132`, `Community 8`, `Community 15`, `Community 19`, `Community 22`, `Community 25`, `Community 154`, `Community 27`, `Community 30`, `Community 32`, `Community 34`, `Community 35`, `Community 36`, `Community 39`, `Community 43`, `Community 48`, `Community 50`, `Community 53`, `Community 61`, `Community 69`, `Community 78`, `Community 87`, `Community 88`, `Community 89`, `Community 90`, `Community 92`, `Community 97`, `Community 103`, `Community 106`, `Community 111`, `Community 121`, `Community 123`?**
  _High betweenness centrality (0.055) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.11804581580177653 - nodes in this community are weakly interconnected._
