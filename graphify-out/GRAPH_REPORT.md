# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5043 nodes · 18900 edges · 166 communities (124 shown, 42 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1261 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `d17d845b`
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
- Community 164
- Community 165

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 151 edges
2. `LayarTerakhir.Core` - 146 edges
3. `Phase5Director` - 129 edges
4. `Phase4Director` - 122 edges
5. `Phase2Director` - 116 edges
6. `Phase3Director` - 114 edges
7. `FirstPersonController` - 112 edges
8. `AtmajaDirector` - 111 edges
9. `AtmajaPhoneView` - 107 edges
10. `ShoulderCameraRig` - 105 edges

## Surprising Connections (you probably didn't know these)
- `build()` --calls--> `export_fbx()`  [INFERRED]
  Tools/retarget_nyonya_motion.py → Tools/prepare_designer_vrm.py
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

## Communities (166 total, 42 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (29): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+21 more)

### Community 1 - "Community 1"
Cohesion: 0.07
Nodes (8): KosDialogue, LayarTerakhir.Phase2, LayarTerakhir.Interlude, LayarTerakhir.Audio, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Smartphone, LayarTerakhir.Diagnostics

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (47): dialogue_literals(), export(), fetch(), main(), sha(), fetch(), main(), model() (+39 more)

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (9): DeveloperReviewModeTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase5SceneTests, Phase1Ids, Phase1World (+1 more)

### Community 4 - "Community 4"
Cohesion: 0.08
Nodes (13): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+5 more)

### Community 6 - "Community 6"
Cohesion: 0.06
Nodes (19): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+11 more)

### Community 7 - "Community 7"
Cohesion: 0.05
Nodes (23): CampaignData, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data (+15 more)

### Community 9 - "Community 9"
Cohesion: 0.08
Nodes (10): AtmajaReviewDressing, CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, Check, KosSceneValidation, Report (+2 more)

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (4): foley(), main(), snapshot(), main()

### Community 12 - "Community 12"
Cohesion: 0.05
Nodes (12): DreadoutVisualRevision, Report, SceneReport, InterludeStreetCapture, Report, View, Phase3NavInspection, PlaytestRepairBuilder (+4 more)

### Community 13 - "Community 13"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (5): DesignerWorldRevision, KosCorridorBuilder, PlaytestReviewValidation, Record, KosRoomCorrections

### Community 15 - "Community 15"
Cohesion: 0.09
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 16 - "Community 16"
Cohesion: 0.08
Nodes (14): SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, SourceBinding, Report (+6 more)

### Community 17 - "Community 17"
Cohesion: 0.09
Nodes (12): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, RoomDoorState, CasedOpen (+4 more)

### Community 18 - "Community 18"
Cohesion: 0.04
Nodes (20): ball(), box(), cloth(), cylinder(), mat(), tube(), box(), cyl() (+12 more)

### Community 19 - "Community 19"
Cohesion: 0.07
Nodes (28): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+20 more)

### Community 20 - "Community 20"
Cohesion: 0.09
Nodes (10): AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport (+2 more)

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (6): MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 22 - "Community 22"
Cohesion: 0.06
Nodes (4): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 23 - "Community 23"
Cohesion: 0.08
Nodes (17): KosGroundWallRepair, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorKind (+9 more)

### Community 24 - "Community 24"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 25 - "Community 25"
Cohesion: 0.09
Nodes (4): Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase2Campaign

### Community 26 - "Community 26"
Cohesion: 0.08
Nodes (11): GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+3 more)

### Community 27 - "Community 27"
Cohesion: 0.11
Nodes (12): Bindings, MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Bindings (+4 more)

### Community 28 - "Community 28"
Cohesion: 0.12
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 29 - "Community 29"
Cohesion: 0.12
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 30 - "Community 30"
Cohesion: 0.08
Nodes (8): AtmajaPlayerAudit, Receipt, DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 31 - "Community 31"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 32 - "Community 32"
Cohesion: 0.08
Nodes (10): GraphicsReferenceAudit, Receipt, GameplayRecoveryRuntimeTests, KosGroundWallTests, TempShaftProbe, DoubleLeafGate, IsOpen, KosCorridorDirector (+2 more)

### Community 33 - "Community 33"
Cohesion: 0.12
Nodes (30): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+22 more)

### Community 34 - "Community 34"
Cohesion: 0.10
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 35 - "Community 35"
Cohesion: 0.12
Nodes (11): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+3 more)

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 37 - "Community 37"
Cohesion: 0.06
Nodes (24): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+16 more)

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (11): ball(), box(), cylinder(), mat(), tube(), export(), meshes(), asset() (+3 more)

### Community 39 - "Community 39"
Cohesion: 0.11
Nodes (26): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+18 more)

### Community 40 - "Community 40"
Cohesion: 0.10
Nodes (11): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+3 more)

### Community 41 - "Community 41"
Cohesion: 0.18
Nodes (5): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, Phase2Anchor, Phase2Room

### Community 42 - "Community 42"
Cohesion: 0.07
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 43 - "Community 43"
Cohesion: 0.10
Nodes (4): Phase3CampaignTests, PhaseRestartTests, Slot, Phase3Campaign

### Community 44 - "Community 44"
Cohesion: 0.12
Nodes (6): VoiceBank, KosVisualUpgrade, VisualAudit, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 45 - "Community 45"
Cohesion: 0.09
Nodes (20): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), DesignerHeadroom, digest(), export_fbx() (+12 more)

### Community 46 - "Community 46"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 47 - "Community 47"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 48 - "Community 48"
Cohesion: 0.12
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 49 - "Community 49"
Cohesion: 0.12
Nodes (3): Phase1PlayerAudit, Receipt, RecoveryTestInputSettings

### Community 50 - "Community 50"
Cohesion: 0.11
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 53 - "Community 53"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 54 - "Community 54"
Cohesion: 0.13
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 55 - "Community 55"
Cohesion: 0.11
Nodes (8): PrologCompletionAudit, Campaign, Hud, Phone, Receipt, InteractionInteractor, ContextResolver, CurrentTarget

### Community 57 - "Community 57"
Cohesion: 0.08
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 58 - "Community 58"
Cohesion: 0.12
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 59 - "Community 59"
Cohesion: 0.06
Nodes (31): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 60 - "Community 60"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 61 - "Community 61"
Cohesion: 0.14
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 62 - "Community 62"
Cohesion: 0.09
Nodes (3): LayarTerakhir.Review, LayarTerakhir.Player, LayarTerakhir.Prolog

### Community 63 - "Community 63"
Cohesion: 0.10
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 64 - "Community 64"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 65 - "Community 65"
Cohesion: 0.12
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 66 - "Community 66"
Cohesion: 0.11
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 68 - "Community 68"
Cohesion: 0.10
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 70 - "Community 70"
Cohesion: 0.10
Nodes (6): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource, Phase4ForestHazard

### Community 71 - "Community 71"
Cohesion: 0.08
Nodes (11): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+3 more)

### Community 72 - "Community 72"
Cohesion: 0.15
Nodes (5): BloodWriting, DesignerVrmBuilder, PrologPresentationBuilder, Receipt, AtmajaWorldText

### Community 73 - "Community 73"
Cohesion: 0.11
Nodes (10): PrologKosSceneBuilder, PrologMilestoneValidator, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower (+2 more)

### Community 74 - "Community 74"
Cohesion: 0.11
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 75 - "Community 75"
Cohesion: 0.10
Nodes (5): opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 76 - "Community 76"
Cohesion: 0.13
Nodes (13): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+5 more)

### Community 77 - "Community 77"
Cohesion: 0.08
Nodes (22): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+14 more)

### Community 78 - "Community 78"
Cohesion: 0.13
Nodes (7): fetch(), main(), main(), prepare(), foley(), render(), write_pcm()

### Community 79 - "Community 79"
Cohesion: 0.19
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 80 - "Community 80"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 82 - "Community 82"
Cohesion: 0.13
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 83 - "Community 83"
Cohesion: 0.14
Nodes (6): AmbientAudioLoop, LevelMultiplier, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 84 - "Community 84"
Cohesion: 0.12
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 85 - "Community 85"
Cohesion: 0.11
Nodes (8): SceneRecord, Phase2Sound, Phase2Soundscape, Phase2ThermalTarget, Phase2World, Phase4UvPresentation, Phase5HumanMotion, Phase5SpectralPresentation

### Community 86 - "Community 86"
Cohesion: 0.12
Nodes (7): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection, ReviewFlyCamera, Active, Eye

### Community 87 - "Community 87"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 88 - "Community 88"
Cohesion: 0.12
Nodes (12): cloth(), ellipsoid(), finish(), mesh(), tube(), is_arm(), main(), delete_fill_tagged() (+4 more)

### Community 89 - "Community 89"
Cohesion: 0.13
Nodes (9): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 90 - "Community 90"
Cohesion: 0.11
Nodes (12): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+4 more)

### Community 91 - "Community 91"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 92 - "Community 92"
Cohesion: 0.13
Nodes (8): ExplorationCommentary, Zone, MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached, NeighborhoodRain

### Community 93 - "Community 93"
Cohesion: 0.16
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 94 - "Community 94"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 95 - "Community 95"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 97 - "Community 97"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 99 - "Community 99"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 100 - "Community 100"
Cohesion: 0.21
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 101 - "Community 101"
Cohesion: 0.25
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 102 - "Community 102"
Cohesion: 0.23
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 106 - "Community 106"
Cohesion: 0.16
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 111 - "Community 111"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 114 - "Community 114"
Cohesion: 0.26
Nodes (3): AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 116 - "Community 116"
Cohesion: 0.26
Nodes (6): Metric, NyonyaMotionRevisionBuilder, Report, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 118 - "Community 118"
Cohesion: 0.18
Nodes (3): InterludeNpc, InteractableId, ClockAmbience

### Community 119 - "Community 119"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 121 - "Community 121"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 122 - "Community 122"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 123 - "Community 123"
Cohesion: 0.20
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 126 - "Community 126"
Cohesion: 0.24
Nodes (4): FoundationAuditCoroutineHost, RecoveryPresentationProbe, RecoveryRuntimeScenario, RecoveryVisualEvidence

### Community 127 - "Community 127"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 128 - "Community 128"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 131 - "Community 131"
Cohesion: 0.31
Nodes (4): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId

### Community 132 - "Community 132"
Cohesion: 0.20
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 133 - "Community 133"
Cohesion: 0.36
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 135 - "Community 135"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 136 - "Community 136"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 137 - "Community 137"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 138 - "Community 138"
Cohesion: 0.22
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 139 - "Community 139"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 141 - "Community 141"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 142 - "Community 142"
Cohesion: 0.42
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 143 - "Community 143"
Cohesion: 0.25
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 145 - "Community 145"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 148 - "Community 148"
Cohesion: 0.33
Nodes (5): Phase3Balance, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 149 - "Community 149"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 152 - "Community 152"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 153 - "Community 153"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 155 - "Community 155"
Cohesion: 0.33
Nodes (4): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance

### Community 157 - "Community 157"
Cohesion: 0.60
Nodes (3): AtmajaGeometryAudit, Entry, Inventory

### Community 158 - "Community 158"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **674 isolated node(s):** `AwaitingBlitz`, `AwaitingPhotos`, `Campaign`, `FollowsPlayerInBasement`, `Hud` (+669 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1063 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **42 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 37` to `Community 0`, `Community 1`, `Community 5`, `Community 6`, `Community 7`, `Community 9`, `Community 10`, `Community 139`, `Community 13`, `Community 14`, `Community 16`, `Community 17`, `Community 19`, `Community 20`, `Community 21`, `Community 147`, `Community 23`, `Community 24`, `Community 25`, `Community 26`, `Community 27`, `Community 29`, `Community 30`, `Community 31`, `Community 32`, `Community 35`, `Community 38`, `Community 48`, `Community 52`, `Community 58`, `Community 59`, `Community 61`, `Community 63`, `Community 66`, `Community 68`, `Community 71`, `Community 73`, `Community 76`, `Community 79`, `Community 80`, `Community 83`, `Community 85`, `Community 86`, `Community 89`, `Community 91`, `Community 92`, `Community 100`, `Community 101`, `Community 120`?**
  _High betweenness centrality (0.228) - this node is a cross-community bridge._
- **What connects `AwaitingBlitz`, `AwaitingPhotos`, `Campaign` to the rest of the system?**
  _674 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.04168912848158131 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 4` to `Community 0`, `Community 1`, `Community 3`, `Community 6`, `Community 7`, `Community 10`, `Community 19`, `Community 24`, `Community 25`, `Community 28`, `Community 43`, `Community 48`, `Community 49`, `Community 51`, `Community 52`, `Community 54`, `Community 55`, `Community 57`, `Community 59`, `Community 70`, `Community 71`, `Community 73`, `Community 76`, `Community 85`, `Community 90`, `Community 91`, `Community 100`?**
  _High betweenness centrality (0.068) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.07307692307692308 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 111` to `Community 32`, `Community 1`, `Community 0`, `Community 38`, `Community 14`, `Community 112`, `Community 17`, `Community 114`, `Community 83`, `Community 85`, `Community 89`, `Community 95`?**
  _High betweenness centrality (0.047) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.047474747474747475 - nodes in this community are weakly interconnected._
