# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4962 nodes · 18762 edges · 153 communities (120 shown, 33 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1252 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `20fc1be0`
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

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 156 edges
2. `LayarTerakhir.Core` - 156 edges
3. `Phase5Director` - 129 edges
4. `AtmajaDirector` - 122 edges
5. `Phase4Director` - 122 edges
6. `ShoulderCameraRig` - 117 edges
7. `Phase2Director` - 116 edges
8. `Phase3Director` - 114 edges
9. `AtmajaPhoneView` - 114 edges
10. `FirstPersonController` - 113 edges

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

## Communities (153 total, 33 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (24): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+16 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (9): KosDialogue, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Interlude, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.UI, LayarTerakhir.Player (+1 more)

### Community 2 - "Community 2"
Cohesion: 0.04
Nodes (29): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+21 more)

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (8): HidingAwareness, DesignerRevisionTests, DeveloperReviewModeTests, HidingAwarenessTests, Phase2SceneTests, Phase5SceneTests, PlayerFeelTests, Phase5AssetSource

### Community 4 - "Community 4"
Cohesion: 0.08
Nodes (13): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+5 more)

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (43): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+35 more)

### Community 6 - "Community 6"
Cohesion: 0.04
Nodes (21): CampaignData, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data (+13 more)

### Community 9 - "Community 9"
Cohesion: 0.07
Nodes (7): Phase2Ids, Phase3Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (6): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder, DesignerWorldRevision

### Community 11 - "Community 11"
Cohesion: 0.07
Nodes (20): MotorcycleRiderAlignment, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+12 more)

### Community 12 - "Community 12"
Cohesion: 0.05
Nodes (37): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+29 more)

### Community 13 - "Community 13"
Cohesion: 0.07
Nodes (13): NyonyaMotionTests, Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, Report, Usage (+5 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (4): foley(), main(), snapshot(), main()

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 16 - "Community 16"
Cohesion: 0.04
Nodes (27): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+19 more)

### Community 17 - "Community 17"
Cohesion: 0.05
Nodes (34): cloth(), ellipsoid(), finish(), mesh(), tube(), actions(), add_armature(), at() (+26 more)

### Community 18 - "Community 18"
Cohesion: 0.08
Nodes (12): GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+4 more)

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (14): Bindings, MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, SourceBinding, SceneRecord, Usage (+6 more)

### Community 20 - "Community 20"
Cohesion: 0.07
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 21 - "Community 21"
Cohesion: 0.10
Nodes (6): CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, KosVisualV2, FloorY

### Community 22 - "Community 22"
Cohesion: 0.05
Nodes (5): Phase1Content, Phase5Content, LayarTerakhir.Phase1, LayarTerakhir.Core, LayarTerakhir.Phase5

### Community 23 - "Community 23"
Cohesion: 0.05
Nodes (21): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+13 more)

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (28): PhoneDocument, Phase1MoneyReceipt, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+20 more)

### Community 25 - "Community 25"
Cohesion: 0.11
Nodes (8): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor, Phase1World

### Community 26 - "Community 26"
Cohesion: 0.06
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 27 - "Community 27"
Cohesion: 0.05
Nodes (11): box(), cylinder(), finish(), tube(), export(), meshes(), sample_surface(), solve() (+3 more)

### Community 28 - "Community 28"
Cohesion: 0.11
Nodes (11): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase1LightBudget, Phase2ThermalTarget (+3 more)

### Community 29 - "Community 29"
Cohesion: 0.08
Nodes (12): FoundationAuditCoroutineHost, FoundationPlayAuditTests, RecoveryPresentationProbe, RecoveryRuntimeScenario, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+4 more)

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (4): Phase4Data, Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 31 - "Community 31"
Cohesion: 0.11
Nodes (9): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Anchor, Phase3World (+1 more)

### Community 32 - "Community 32"
Cohesion: 0.06
Nodes (22): Phase4Balance, Phase4Banaspati, B, LungeProgress, Pursuing, State, W, Phase4Ritual (+14 more)

### Community 33 - "Community 33"
Cohesion: 0.10
Nodes (6): MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 34 - "Community 34"
Cohesion: 0.10
Nodes (7): KosGroundWallRepair, KosVisualUpgrade, VisualAudit, Phase2NavigationProbe, Phase3UvPresentation, Phase4UvPresentation, KosRoomCorrections

### Community 35 - "Community 35"
Cohesion: 0.06
Nodes (13): InterludeNpc, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId (+5 more)

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (16): Phase2Balance, Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed (+8 more)

### Community 37 - "Community 37"
Cohesion: 0.13
Nodes (9): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey (+1 more)

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (20): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+12 more)

### Community 40 - "Community 40"
Cohesion: 0.06
Nodes (11): dialogue_literals(), export(), compact(), material_records(), opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests (+3 more)

### Community 41 - "Community 41"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 42 - "Community 42"
Cohesion: 0.10
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 43 - "Community 43"
Cohesion: 0.11
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 44 - "Community 44"
Cohesion: 0.13
Nodes (28): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+20 more)

### Community 45 - "Community 45"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 46 - "Community 46"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 47 - "Community 47"
Cohesion: 0.10
Nodes (8): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, ClockPuzzleView, Hour, Minute

### Community 48 - "Community 48"
Cohesion: 0.07
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 49 - "Community 49"
Cohesion: 0.08
Nodes (7): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 50 - "Community 50"
Cohesion: 0.20
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 51 - "Community 51"
Cohesion: 0.07
Nodes (5): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Balance, Phase1Ids

### Community 52 - "Community 52"
Cohesion: 0.10
Nodes (7): AtmajaPlayerAudit, Receipt, RecoveryVisualEvidence, InterludeExterior, InteractionInteractor, ContextResolver, CurrentTarget

### Community 53 - "Community 53"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 54 - "Community 54"
Cohesion: 0.13
Nodes (22): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+14 more)

### Community 55 - "Community 55"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 56 - "Community 56"
Cohesion: 0.15
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 57 - "Community 57"
Cohesion: 0.12
Nodes (8): SceneAudioProfile, PresentationProfile, VoiceLine, VoiceBank, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 60 - "Community 60"
Cohesion: 0.13
Nodes (5): BloodWriting, DesignerVrmBuilder, PrologPresentationBuilder, Receipt, AtmajaWorldText

### Community 61 - "Community 61"
Cohesion: 0.13
Nodes (4): PlayerFeelAudit, Receipt, Sample, RecoveryTestInputSettings

### Community 62 - "Community 62"
Cohesion: 0.18
Nodes (3): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 63 - "Community 63"
Cohesion: 0.15
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 64 - "Community 64"
Cohesion: 0.11
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 65 - "Community 65"
Cohesion: 0.13
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 66 - "Community 66"
Cohesion: 0.12
Nodes (18): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+10 more)

### Community 71 - "Community 71"
Cohesion: 0.22
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 72 - "Community 72"
Cohesion: 0.10
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 73 - "Community 73"
Cohesion: 0.16
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 74 - "Community 74"
Cohesion: 0.11
Nodes (8): AmbientAudioLoop, LevelMultiplier, KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 75 - "Community 75"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 76 - "Community 76"
Cohesion: 0.12
Nodes (10): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph (+2 more)

### Community 77 - "Community 77"
Cohesion: 0.18
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 78 - "Community 78"
Cohesion: 0.11
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 79 - "Community 79"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 80 - "Community 80"
Cohesion: 0.15
Nodes (6): RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics, AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 81 - "Community 81"
Cohesion: 0.14
Nodes (11): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+3 more)

### Community 82 - "Community 82"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 83 - "Community 83"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 84 - "Community 84"
Cohesion: 0.15
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 87 - "Community 87"
Cohesion: 0.19
Nodes (3): AtmajaStairRevisionBuilder, BandVertex, PracticalLightPresentation

### Community 88 - "Community 88"
Cohesion: 0.14
Nodes (9): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape (+1 more)

### Community 89 - "Community 89"
Cohesion: 0.14
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 90 - "Community 90"
Cohesion: 0.16
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 92 - "Community 92"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 93 - "Community 93"
Cohesion: 0.16
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 94 - "Community 94"
Cohesion: 0.14
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 95 - "Community 95"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 96 - "Community 96"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 97 - "Community 97"
Cohesion: 0.22
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 98 - "Community 98"
Cohesion: 0.19
Nodes (8): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto

### Community 99 - "Community 99"
Cohesion: 0.16
Nodes (9): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+1 more)

### Community 100 - "Community 100"
Cohesion: 0.17
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 101 - "Community 101"
Cohesion: 0.17
Nodes (6): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached, NeighborhoodRain

### Community 102 - "Community 102"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 103 - "Community 103"
Cohesion: 0.29
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 104 - "Community 104"
Cohesion: 0.19
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 106 - "Community 106"
Cohesion: 0.22
Nodes (3): Phase4ForestHazard, Phase4Sound, Phase4Soundscape

### Community 109 - "Community 109"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 110 - "Community 110"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 111 - "Community 111"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 112 - "Community 112"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 117 - "Community 117"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 118 - "Community 118"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 119 - "Community 119"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 120 - "Community 120"
Cohesion: 0.24
Nodes (8): InterludeStreetCapture, Report, View, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric

### Community 123 - "Community 123"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 124 - "Community 124"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 125 - "Community 125"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 127 - "Community 127"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 129 - "Community 129"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 131 - "Community 131"
Cohesion: 0.33
Nodes (5): ball(), box(), cylinder(), mat(), tube()

### Community 132 - "Community 132"
Cohesion: 0.25
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 135 - "Community 135"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 137 - "Community 137"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 140 - "Community 140"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 142 - "Community 142"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 143 - "Community 143"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 145 - "Community 145"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 146 - "Community 146"
Cohesion: 0.60
Nodes (3): Metric, NyonyaMotionRevisionBuilder, Report

### Community 147 - "Community 147"
Cohesion: 0.60
Nodes (3): AtmajaGeometryAudit, Entry, Inventory

### Community 148 - "Community 148"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 149 - "Community 149"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 152 - "Community 152"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **674 isolated node(s):** `Busy`, `Campaign`, `CanBeginLive`, `CheckpointRestores`, `HasSurvey` (+669 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1045 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **33 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 48` to `Community 0`, `Community 1`, `Community 2`, `Community 7`, `Community 10`, `Community 11`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 23`, `Community 24`, `Community 25`, `Community 26`, `Community 152`, `Community 28`, `Community 29`, `Community 31`, `Community 33`, `Community 34`, `Community 35`, `Community 36`, `Community 38`, `Community 42`, `Community 43`, `Community 45`, `Community 52`, `Community 56`, `Community 61`, `Community 64`, `Community 65`, `Community 73`, `Community 75`, `Community 81`, `Community 82`, `Community 87`, `Community 89`, `Community 100`, `Community 103`, `Community 104`, `Community 107`?**
  _High betweenness centrality (0.211) - this node is a cross-community bridge._
- **What connects `Busy`, `Campaign`, `CanBeginLive` to the rest of the system?**
  _674 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.04685942173479561 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 4` to `Community 0`, `Community 2`, `Community 3`, `Community 6`, `Community 9`, `Community 12`, `Community 20`, `Community 22`, `Community 24`, `Community 26`, `Community 28`, `Community 29`, `Community 30`, `Community 38`, `Community 43`, `Community 45`, `Community 49`, `Community 51`, `Community 58`, `Community 61`, `Community 63`, `Community 72`, `Community 77`, `Community 81`, `Community 86`, `Community 89`, `Community 121`?**
  _High betweenness centrality (0.051) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05110565110565111 - nodes in this community are weakly interconnected._
- **Why does `Phase4Director` connect `Community 45` to `Community 1`, `Community 4`, `Community 10`, `Community 11`, `Community 12`, `Community 16`, `Community 18`, `Community 19`, `Community 21`, `Community 25`, `Community 28`, `Community 29`, `Community 30`, `Community 32`, `Community 33`, `Community 34`, `Community 35`, `Community 38`, `Community 42`, `Community 47`, `Community 48`, `Community 49`, `Community 58`, `Community 59`, `Community 60`, `Community 68`, `Community 69`, `Community 82`, `Community 88`, `Community 100`, `Community 103`, `Community 106`?**
  _High betweenness centrality (0.047) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.04323232323232323 - nodes in this community are weakly interconnected._
