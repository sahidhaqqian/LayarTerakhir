# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4617 nodes · 17136 edges · 144 communities (114 shown, 30 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1173 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `fd370245`
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
- Community 89
- Community 90
- Community 91
- Community 92
- Community 93
- Community 94
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
- Community 124
- Community 125
- Community 126
- Community 127
- Community 128
- Community 129
- Community 130
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
- Community 143

## God Nodes (most connected - your core abstractions)
1. `Phase4Director` - 121 edges
2. `Phase2Director` - 115 edges
3. `Phase3Director` - 113 edges
4. `LayarTerakhir.Core` - 113 edges
5. `CampaignState` - 109 edges
6. `AtmajaPhoneView` - 106 edges
7. `Phase5Director` - 103 edges
8. `AtmajaDirector` - 102 edges
9. `Phase1Director` - 89 edges
10. `LayarTerakhir.Player` - 88 edges

## Surprising Connections (you probably didn't know these)
- `delete_fill_tagged()` --indirect_call--> `mesh()`  [INFERRED]
  Tools/repair_nyonya_appearance.py → Tools/author_atmaja_character.py
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py

## Import Cycles
- None detected.

## Communities (144 total, 30 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (59): asset(), download(), load_meta(), asset(), download(), fetch(), main(), sha() (+51 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (29): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+21 more)

### Community 3 - "Community 3"
Cohesion: 0.06
Nodes (8): CompactKosPolish, KosRoomSurvey, KosVisualV2, FloorY, Phase2NavigationProbe, PlaytestReviewValidation, Record, KosRoomCorrections

### Community 4 - "Community 4"
Cohesion: 0.05
Nodes (32): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+24 more)

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (18): DesignerVrmBuilder, Authoring, PrologCompletionBuilder, PrologPresentationBuilder, Receipt, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics (+10 more)

### Community 6 - "Community 6"
Cohesion: 0.11
Nodes (3): KosGroundWallRepair, Phase3NavInspection, LayarTerakhir.Editor

### Community 7 - "Community 7"
Cohesion: 0.10
Nodes (7): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (15): NyonyaMotionTests, Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, Report, Usage (+7 more)

### Community 9 - "Community 9"
Cohesion: 0.04
Nodes (11): DesignerRevisionTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase2SceneTests, Count, Phase3SceneTests, Phase5SceneTests (+3 more)

### Community 10 - "Community 10"
Cohesion: 0.09
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (12): GoldenLinuxBuild, Record, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1SceneBuilder, RoomDressingPlacement (+4 more)

### Community 12 - "Community 12"
Cohesion: 0.05
Nodes (5): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 13 - "Community 13"
Cohesion: 0.05
Nodes (34): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+26 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (14): BloodWriting, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorTile (+6 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (3): main(), snapshot(), main()

### Community 16 - "Community 16"
Cohesion: 0.09
Nodes (14): Bindings, MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, SourceBinding, SceneRecord, Usage (+6 more)

### Community 17 - "Community 17"
Cohesion: 0.07
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 18 - "Community 18"
Cohesion: 0.05
Nodes (20): Phase1WorldItem, InteractableId, Phase5WorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId (+12 more)

### Community 19 - "Community 19"
Cohesion: 0.07
Nodes (9): AtmajaStairRevisionBuilder, BandVertex, DesignerWorldRevision, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer (+1 more)

### Community 20 - "Community 20"
Cohesion: 0.04
Nodes (14): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+6 more)

### Community 21 - "Community 21"
Cohesion: 0.05
Nodes (24): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+16 more)

### Community 22 - "Community 22"
Cohesion: 0.08
Nodes (10): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+2 more)

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (31): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+23 more)

### Community 24 - "Community 24"
Cohesion: 0.10
Nodes (5): DreadoutVisualRevision, Report, SceneReport, PlaytestRepairBuilder, Phase1LightBudget

### Community 25 - "Community 25"
Cohesion: 0.08
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 26 - "Community 26"
Cohesion: 0.10
Nodes (9): FoundationPlayAuditTests, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance (+1 more)

### Community 27 - "Community 27"
Cohesion: 0.07
Nodes (10): Receipt, ReviewPlayerAudit, Hud, Phone, View, KosGroundWallTests, TempShaftProbe, KosCorridorDirector (+2 more)

### Community 28 - "Community 28"
Cohesion: 0.12
Nodes (4): Phase4Data, Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 29 - "Community 29"
Cohesion: 0.06
Nodes (28): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+20 more)

### Community 30 - "Community 30"
Cohesion: 0.14
Nodes (8): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor, Phase2Room, Phase2World

### Community 31 - "Community 31"
Cohesion: 0.07
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 32 - "Community 32"
Cohesion: 0.09
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 33 - "Community 33"
Cohesion: 0.07
Nodes (7): CampaignData, PhoneDocument, Phase3Data, UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 34 - "Community 34"
Cohesion: 0.12
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 35 - "Community 35"
Cohesion: 0.10
Nodes (4): Phase3Ids, Phase2RitualTransactionTests, Phase3CampaignTests, Phase3Campaign

### Community 36 - "Community 36"
Cohesion: 0.09
Nodes (4): Phase1Content, LayarTerakhir.Phase2, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 37 - "Community 37"
Cohesion: 0.06
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 39 - "Community 39"
Cohesion: 0.13
Nodes (8): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 40 - "Community 40"
Cohesion: 0.10
Nodes (9): PresentationProfile, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+1 more)

### Community 41 - "Community 41"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 42 - "Community 42"
Cohesion: 0.11
Nodes (6): CampaignRouting, ReviewDirectBoot, PhaseRestartTests, Slot, ReviewDirectBootTests, Slot

### Community 43 - "Community 43"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 44 - "Community 44"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 45 - "Community 45"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 47 - "Community 47"
Cohesion: 0.20
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 48 - "Community 48"
Cohesion: 0.08
Nodes (11): PlayerFeelTests, PrologKosSceneBuilder, PrologMilestoneValidator, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting (+3 more)

### Community 49 - "Community 49"
Cohesion: 0.09
Nodes (7): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone, ExplorationCommentary, Zone

### Community 50 - "Community 50"
Cohesion: 0.06
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 51 - "Community 51"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 52 - "Community 52"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 53 - "Community 53"
Cohesion: 0.12
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 54 - "Community 54"
Cohesion: 0.09
Nodes (17): MotorcycleRiderAlignment, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity (+9 more)

### Community 56 - "Community 56"
Cohesion: 0.08
Nodes (7): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1Ids

### Community 57 - "Community 57"
Cohesion: 0.11
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 58 - "Community 58"
Cohesion: 0.09
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 60 - "Community 60"
Cohesion: 0.07
Nodes (25): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+17 more)

### Community 61 - "Community 61"
Cohesion: 0.09
Nodes (6): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource, Phase4ForestHazard

### Community 62 - "Community 62"
Cohesion: 0.15
Nodes (4): Phase1MoneyReceipt, Phase2Data, Phase2CampaignTests, Phase2Campaign

### Community 66 - "Community 66"
Cohesion: 0.14
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 67 - "Community 67"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 69 - "Community 69"
Cohesion: 0.12
Nodes (12): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Wewe, B (+4 more)

### Community 70 - "Community 70"
Cohesion: 0.13
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 71 - "Community 71"
Cohesion: 0.09
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 72 - "Community 72"
Cohesion: 0.13
Nodes (13): AtmajaGeometryAudit, Entry, Inventory, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+5 more)

### Community 73 - "Community 73"
Cohesion: 0.12
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 74 - "Community 74"
Cohesion: 0.18
Nodes (5): Phase4PlayerAudit, Result, Phase4Torch, Authentic, PermanentlyOff

### Community 75 - "Community 75"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 76 - "Community 76"
Cohesion: 0.10
Nodes (12): Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining (+4 more)

### Community 77 - "Community 77"
Cohesion: 0.14
Nodes (11): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+3 more)

### Community 79 - "Community 79"
Cohesion: 0.18
Nodes (3): Phase1DynamoUse, Phase5Data, Phase5Ids

### Community 80 - "Community 80"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 81 - "Community 81"
Cohesion: 0.15
Nodes (10): Phase3Content, Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 82 - "Community 82"
Cohesion: 0.15
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 83 - "Community 83"
Cohesion: 0.13
Nodes (6): DevelopmentScreenshotCapture, FoundationAuditCoroutineHost, Phase4UvPresentation, PracticalLightPresentation, DoubleLeafGate, IsOpen

### Community 84 - "Community 84"
Cohesion: 0.15
Nodes (5): AmbientAudioLoop, LevelMultiplier, Phase4Sound, Phase4Soundscape, ClockAmbience

### Community 85 - "Community 85"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 87 - "Community 87"
Cohesion: 0.23
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 89 - "Community 89"
Cohesion: 0.15
Nodes (5): Phase4Lighting, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 90 - "Community 90"
Cohesion: 0.14
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 91 - "Community 91"
Cohesion: 0.25
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 92 - "Community 92"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 93 - "Community 93"
Cohesion: 0.25
Nodes (13): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+5 more)

### Community 94 - "Community 94"
Cohesion: 0.16
Nodes (8): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5Sound, Phase5Soundscape

### Community 96 - "Community 96"
Cohesion: 0.17
Nodes (4): Phase5Director, EvpProgress, SelectedSource, ThermalReadout

### Community 97 - "Community 97"
Cohesion: 0.18
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 98 - "Community 98"
Cohesion: 0.26
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 101 - "Community 101"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 102 - "Community 102"
Cohesion: 0.16
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 107 - "Community 107"
Cohesion: 0.20
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 108 - "Community 108"
Cohesion: 0.24
Nodes (3): PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 109 - "Community 109"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 110 - "Community 110"
Cohesion: 0.17
Nodes (5): IPhoneChapterContent, CameraLocked, CameraStatus, CanBeginLive, Viewers

### Community 111 - "Community 111"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 112 - "Community 112"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 113 - "Community 113"
Cohesion: 0.21
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 116 - "Community 116"
Cohesion: 0.31
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 117 - "Community 117"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 118 - "Community 118"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 120 - "Community 120"
Cohesion: 0.24
Nodes (4): VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 121 - "Community 121"
Cohesion: 0.20
Nodes (6): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers

### Community 122 - "Community 122"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 124 - "Community 124"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 125 - "Community 125"
Cohesion: 0.22
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 126 - "Community 126"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 127 - "Community 127"
Cohesion: 0.42
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 128 - "Community 128"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 129 - "Community 129"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 130 - "Community 130"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 133 - "Community 133"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 135 - "Community 135"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 137 - "Community 137"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 138 - "Community 138"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 139 - "Community 139"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 140 - "Community 140"
Cohesion: 0.40
Nodes (5): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid

## Knowledge Gaps
- **620 isolated node(s):** `Busy`, `Campaign`, `CanBeginLive`, `CheckpointRestores`, `HasSurvey` (+615 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1007 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **30 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 37` to `Community 1`, `Community 2`, `Community 3`, `Community 4`, `Community 8`, `Community 10`, `Community 13`, `Community 14`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 21`, `Community 26`, `Community 27`, `Community 29`, `Community 30`, `Community 32`, `Community 34`, `Community 40`, `Community 41`, `Community 43`, `Community 49`, `Community 50`, `Community 62`, `Community 64`, `Community 65`, `Community 66`, `Community 70`, `Community 75`, `Community 77`, `Community 80`, `Community 81`, `Community 83`, `Community 84`, `Community 87`, `Community 89`, `Community 91`, `Community 97`, `Community 98`, `Community 121`, `Community 123`?**
  _High betweenness centrality (0.154) - this node is a cross-community bridge._
- **What connects `Busy`, `Campaign`, `CanBeginLive` to the rest of the system?**
  _620 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.044825396825396824 - nodes in this community are weakly interconnected._
- **Why does `Phase5Director` connect `Community 13` to `Community 3`, `Community 4`, `Community 5`, `Community 7`, `Community 8`, `Community 9`, `Community 12`, `Community 17`, `Community 18`, `Community 22`, `Community 26`, `Community 34`, `Community 37`, `Community 40`, `Community 48`, `Community 53`, `Community 75`, `Community 79`, `Community 83`, `Community 94`, `Community 97`, `Community 98`, `Community 110`, `Community 119`?**
  _High betweenness centrality (0.077) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.04525132095921962 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 22` to `Community 1`, `Community 6`, `Community 9`, `Community 13`, `Community 26`, `Community 28`, `Community 29`, `Community 33`, `Community 35`, `Community 38`, `Community 42`, `Community 43`, `Community 48`, `Community 50`, `Community 59`, `Community 62`, `Community 67`, `Community 77`, `Community 79`, `Community 80`, `Community 81`, `Community 83`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.08177744585511576 - nodes in this community are weakly interconnected._
