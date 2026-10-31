# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5180 nodes · 19515 edges · 179 communities (128 shown, 51 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1248 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `018fd910`
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
- Community 150
- Community 151
- Community 152
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
- Community 169
- Community 170
- Community 171
- Community 172
- Community 173
- Community 175
- Community 176
- Community 177

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 140 edges
2. `Phase5Director` - 129 edges
3. `LayarTerakhir.Core` - 123 edges
4. `Phase4Director` - 122 edges
5. `Phase2Director` - 116 edges
6. `Phase3Director` - 114 edges
7. `AtmajaPhoneView` - 113 edges
8. `FirstPersonController` - 109 edges
9. `LayarTerakhir.Player` - 101 edges
10. `AtmajaDirector` - 100 edges

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

## Communities (179 total, 51 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (66): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), fetch() (+58 more)

### Community 1 - "Community 1"
Cohesion: 0.03
Nodes (15): DeveloperReviewModeTests, GoldenSceneTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase2SceneTests, Count (+7 more)

### Community 3 - "Community 3"
Cohesion: 0.07
Nodes (8): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Smartphone, LayarTerakhir.Diagnostics

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (24): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+16 more)

### Community 6 - "Community 6"
Cohesion: 0.05
Nodes (20): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+12 more)

### Community 7 - "Community 7"
Cohesion: 0.09
Nodes (10): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5CampaignTests, Slot (+2 more)

### Community 8 - "Community 8"
Cohesion: 0.08
Nodes (5): Phase2Ids, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (13): FoundationPlayAuditTests, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked (+5 more)

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (9): BloodWriting, MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, PrologEasySceneRepair, ARPhotoGlimmer (+1 more)

### Community 13 - "Community 13"
Cohesion: 0.08
Nodes (12): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder, RoomDoorState (+4 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (3): main(), snapshot(), main()

### Community 15 - "Community 15"
Cohesion: 0.04
Nodes (14): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+6 more)

### Community 16 - "Community 16"
Cohesion: 0.11
Nodes (5): CompactKosPolish, GoldenSceneValidation, Record, KosVisualV2, FloorY

### Community 17 - "Community 17"
Cohesion: 0.08
Nodes (6): KosGroundWallRepair, KosVisualUpgrade, VisualAudit, TempShaftProbe, Phase3UvPresentation, KosRoomCorrections

### Community 18 - "Community 18"
Cohesion: 0.07
Nodes (26): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+18 more)

### Community 19 - "Community 19"
Cohesion: 0.06
Nodes (30): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+22 more)

### Community 20 - "Community 20"
Cohesion: 0.10
Nodes (16): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorKind, FragileDrop (+8 more)

### Community 21 - "Community 21"
Cohesion: 0.08
Nodes (11): FoundationAuditCoroutineHost, Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext (+3 more)

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (12): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey (+4 more)

### Community 23 - "Community 23"
Cohesion: 0.09
Nodes (11): GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+3 more)

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (3): LayarTerakhir.Interlude, LayarTerakhir.Player, LayarTerakhir.Prolog

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (4): Phase5Content, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 26 - "Community 26"
Cohesion: 0.08
Nodes (10): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+2 more)

### Community 27 - "Community 27"
Cohesion: 0.12
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 28 - "Community 28"
Cohesion: 0.09
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 29 - "Community 29"
Cohesion: 0.05
Nodes (35): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+27 more)

### Community 30 - "Community 30"
Cohesion: 0.10
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 31 - "Community 31"
Cohesion: 0.05
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 32 - "Community 32"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 33 - "Community 33"
Cohesion: 0.11
Nodes (28): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+20 more)

### Community 34 - "Community 34"
Cohesion: 0.11
Nodes (11): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+3 more)

### Community 35 - "Community 35"
Cohesion: 0.07
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 36 - "Community 36"
Cohesion: 0.14
Nodes (3): Prolog47MeshRepair, Vertex, Prolog47SceneRepair

### Community 37 - "Community 37"
Cohesion: 0.12
Nodes (6): VoiceBank, DesignerWorldRevision, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 39 - "Community 39"
Cohesion: 0.10
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 40 - "Community 40"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 41 - "Community 41"
Cohesion: 0.10
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 42 - "Community 42"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 44 - "Community 44"
Cohesion: 0.10
Nodes (4): CampaignRouting, Phase2CampaignTests, PhaseRestartTests, Slot

### Community 45 - "Community 45"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 46 - "Community 46"
Cohesion: 0.15
Nodes (6): Phase5SceneBuilder, SourceEntry, MatRow, MeshBinding, Phase5Anchor, Phase5Zone

### Community 47 - "Community 47"
Cohesion: 0.14
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 48 - "Community 48"
Cohesion: 0.08
Nodes (11): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+3 more)

### Community 49 - "Community 49"
Cohesion: 0.10
Nodes (7): Receipt, RiderPresentationAudit, GameplayRecoveryRuntimeTests, KosGroundWallTests, KosCorridorDirector, Busy, CanDepart

### Community 50 - "Community 50"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 51 - "Community 51"
Cohesion: 0.13
Nodes (3): PlaytestRepairBuilder, Phase1LightBudget, Phase4FearPresentation

### Community 52 - "Community 52"
Cohesion: 0.10
Nodes (5): AtmajaPlayerAudit, Receipt, PlayerFeelAudit, Receipt, Sample

### Community 53 - "Community 53"
Cohesion: 0.19
Nodes (4): MatRow, MeshBinding, Phase4SceneBuilder, Usage

### Community 55 - "Community 55"
Cohesion: 0.21
Nodes (4): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, Phase2Anchor

### Community 56 - "Community 56"
Cohesion: 0.12
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 57 - "Community 57"
Cohesion: 0.09
Nodes (8): Frame, LocationLightingAudit, Receipt, AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 58 - "Community 58"
Cohesion: 0.15
Nodes (3): Authoring, PrologCompletionBuilder, NeighborhoodRain

### Community 59 - "Community 59"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 60 - "Community 60"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 62 - "Community 62"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (7): Phase4Banaspati, B, LungeProgress, Pursuing, State, W, Phase4ForestHazard

### Community 64 - "Community 64"
Cohesion: 0.10
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 66 - "Community 66"
Cohesion: 0.13
Nodes (8): DesignerVrmBuilder, PrologPresentationBuilder, Receipt, AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 67 - "Community 67"
Cohesion: 0.09
Nodes (14): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+6 more)

### Community 68 - "Community 68"
Cohesion: 0.14
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 69 - "Community 69"
Cohesion: 0.10
Nodes (10): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph (+2 more)

### Community 70 - "Community 70"
Cohesion: 0.08
Nodes (20): Phase1Balance, Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering (+12 more)

### Community 71 - "Community 71"
Cohesion: 0.14
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 72 - "Community 72"
Cohesion: 0.14
Nodes (18): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds() (+10 more)

### Community 75 - "Community 75"
Cohesion: 0.16
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 76 - "Community 76"
Cohesion: 0.11
Nodes (12): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Wewe, B (+4 more)

### Community 77 - "Community 77"
Cohesion: 0.09
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 79 - "Community 79"
Cohesion: 0.11
Nodes (9): GoldenLinuxBuild, Record, SceneRecord, Phase2ThermalTarget, Phase2Room, Phase2World, AtmajaEvidenceAtmosphere, DoubleLeafGate (+1 more)

### Community 80 - "Community 80"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 81 - "Community 81"
Cohesion: 0.09
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 82 - "Community 82"
Cohesion: 0.13
Nodes (6): AmbientAudioLoop, LevelMultiplier, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 83 - "Community 83"
Cohesion: 0.13
Nodes (9): AtmajaGeometryAudit, Entry, Inventory, AtmajaSceneValidation, OpeningBaseline, Report, Check, KosSceneValidation (+1 more)

### Community 84 - "Community 84"
Cohesion: 0.09
Nodes (13): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+5 more)

### Community 85 - "Community 85"
Cohesion: 0.13
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 86 - "Community 86"
Cohesion: 0.13
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 87 - "Community 87"
Cohesion: 0.13
Nodes (13): InterludeStreetCapture, Report, View, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+5 more)

### Community 89 - "Community 89"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 90 - "Community 90"
Cohesion: 0.13
Nodes (3): ReviewDirectBoot, ReviewDirectBootTests, Slot

### Community 92 - "Community 92"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 93 - "Community 93"
Cohesion: 0.12
Nodes (12): ScanVisualMode, AR, Normal, UV, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+4 more)

### Community 94 - "Community 94"
Cohesion: 0.15
Nodes (8): PlaytestReviewValidation, Record, Phase1Anchor, Phase1World, ClockAmbience, HingedDoor, InteractableId, IsOpen

### Community 95 - "Community 95"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 97 - "Community 97"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 98 - "Community 98"
Cohesion: 0.19
Nodes (10): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+2 more)

### Community 99 - "Community 99"
Cohesion: 0.18
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 101 - "Community 101"
Cohesion: 0.18
Nodes (3): InterludeExterior, Phase4Lighting, AtmajaMirrorReflection

### Community 102 - "Community 102"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 104 - "Community 104"
Cohesion: 0.18
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 105 - "Community 105"
Cohesion: 0.22
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 107 - "Community 107"
Cohesion: 0.21
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 109 - "Community 109"
Cohesion: 0.19
Nodes (3): PresentationProfile, VoiceLine, AtmajaSynth

### Community 110 - "Community 110"
Cohesion: 0.26
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 111 - "Community 111"
Cohesion: 0.19
Nodes (3): AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 112 - "Community 112"
Cohesion: 0.16
Nodes (4): Phase1WorldItem, InteractableId, KosTravelItem, InteractableId

### Community 113 - "Community 113"
Cohesion: 0.18
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 114 - "Community 114"
Cohesion: 0.21
Nodes (5): Phase3Gate, IsOpen, Phase3Sound, Phase3Soundscape, Voice

### Community 115 - "Community 115"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 116 - "Community 116"
Cohesion: 0.20
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Navigator, Phase1Projectile

### Community 118 - "Community 118"
Cohesion: 0.18
Nodes (3): SceneRecord, Report, Phase5World

### Community 121 - "Community 121"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 126 - "Community 126"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 127 - "Community 127"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 128 - "Community 128"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 131 - "Community 131"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 132 - "Community 132"
Cohesion: 0.21
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 135 - "Community 135"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 136 - "Community 136"
Cohesion: 0.24
Nodes (5): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath

### Community 137 - "Community 137"
Cohesion: 0.35
Nodes (8): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow()

### Community 140 - "Community 140"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 141 - "Community 141"
Cohesion: 0.22
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 142 - "Community 142"
Cohesion: 0.25
Nodes (3): PrologMilestoneValidator, KosInteractableItem, InteractableId

### Community 145 - "Community 145"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 146 - "Community 146"
Cohesion: 0.33
Nodes (4): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId

### Community 147 - "Community 147"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 150 - "Community 150"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 152 - "Community 152"
Cohesion: 0.36
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 155 - "Community 155"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 157 - "Community 157"
Cohesion: 0.24
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 159 - "Community 159"
Cohesion: 0.48
Nodes (3): RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 160 - "Community 160"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 163 - "Community 163"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 165 - "Community 165"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 169 - "Community 169"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

## Knowledge Gaps
- **676 isolated node(s):** `InteractableId`, `CrouchAmount`, `CursorLockRequested`, `IsCrouching`, `IsSliding` (+671 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1071 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **51 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 38` to `Community 2`, `Community 133`, `Community 5`, `Community 6`, `Community 8`, `Community 9`, `Community 10`, `Community 11`, `Community 12`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 23`, `Community 24`, `Community 153`, `Community 29`, `Community 31`, `Community 34`, `Community 35`, `Community 40`, `Community 41`, `Community 45`, `Community 52`, `Community 57`, `Community 63`, `Community 67`, `Community 68`, `Community 75`, `Community 77`, `Community 79`, `Community 82`, `Community 92`, `Community 95`, `Community 98`, `Community 100`, `Community 101`, `Community 107`, `Community 109`, `Community 112`, `Community 118`, `Community 120`?**
  _High betweenness centrality (0.191) - this node is a cross-community bridge._
- **What connects `InteractableId`, `CrouchAmount`, `CursorLockRequested` to the rest of the system?**
  _676 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.037821333595981485 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 26` to `Community 1`, `Community 4`, `Community 6`, `Community 7`, `Community 8`, `Community 10`, `Community 142`, `Community 18`, `Community 21`, `Community 27`, `Community 29`, `Community 158`, `Community 30`, `Community 31`, `Community 35`, `Community 44`, `Community 56`, `Community 79`, `Community 80`, `Community 84`, `Community 90`, `Community 92`, `Community 93`, `Community 98`, `Community 107`?**
  _High betweenness centrality (0.074) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.03016528925619835 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 121` to `Community 10`, `Community 108`, `Community 109`, `Community 111`, `Community 79`, `Community 17`, `Community 49`, `Community 82`, `Community 19`, `Community 116`, `Community 20`, `Community 25`?**
  _High betweenness centrality (0.040) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.08756534727408513 - nodes in this community are weakly interconnected._
