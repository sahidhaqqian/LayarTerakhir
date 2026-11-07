# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5448 nodes · 20977 edges · 172 communities (117 shown, 55 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1322 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `76caa47f`
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
- Community 73
- Community 74
- Community 75
- Community 77
- Community 78
- Community 79
- Community 80
- Community 81
- Community 82
- Community 83
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
- Community 135
- Community 136
- Community 137
- Community 138
- Community 140
- Community 141
- Community 142
- Community 143
- Community 144
- Community 145
- Community 147
- Community 148
- Community 149
- Community 150
- Community 151
- Community 152
- Community 153
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
- Community 167
- Community 168
- Community 169
- Community 170
- Community 171

## God Nodes (most connected - your core abstractions)
1. `LayarTerakhir.Core` - 156 edges
2. `CampaignState` - 155 edges
3. `Phase5Director` - 129 edges
4. `Phase4Director` - 122 edges
5. `FirstPersonController` - 121 edges
6. `Phase2Director` - 116 edges
7. `HudController` - 115 edges
8. `ShoulderCameraRig` - 114 edges
9. `Phase3Director` - 114 edges
10. `AtmajaPhoneView` - 113 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `AtmajaBasementHorror` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/src/Core/CampaignState.cs

## Import Cycles
- None detected.

## Communities (172 total, 55 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (27): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+19 more)

### Community 1 - "Community 1"
Cohesion: 0.04
Nodes (58): asset(), download(), asset(), fetch(), dialogue_literals(), export(), copy(), sha() (+50 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (7): LayarTerakhir.Phase2, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (11): asset(), download(), load_meta(), export(), foley(), main(), snapshot(), note() (+3 more)

### Community 4 - "Community 4"
Cohesion: 0.05
Nodes (17): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, PlaytestRepairBuilder, RumahAtmajaDiningTests (+9 more)

### Community 5 - "Community 5"
Cohesion: 0.07
Nodes (13): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+5 more)

### Community 6 - "Community 6"
Cohesion: 0.07
Nodes (8): CompactKosPolish, Check, KosSceneValidation, Report, KosVisualV2, FloorY, PickupBlinkPhoto, Phase4ForestHazard

### Community 9 - "Community 9"
Cohesion: 0.05
Nodes (16): VoiceLine, AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem (+8 more)

### Community 10 - "Community 10"
Cohesion: 0.04
Nodes (11): HidingAwareness, DesignerRevisionTests, HidingAwarenessTests, KosBathroomRevisionTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests (+3 more)

### Community 11 - "Community 11"
Cohesion: 0.03
Nodes (33): PlayerFeelAudit, Receipt, Sample, PrologKosSceneBuilder, PrologMilestoneValidator, FirstPersonController, CrouchAmount, CursorLockRequested (+25 more)

### Community 13 - "Community 13"
Cohesion: 0.05
Nodes (21): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+13 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (22): CampaignData, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data (+14 more)

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (7): PhoneDocument, Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 17 - "Community 17"
Cohesion: 0.05
Nodes (6): Phase5Content, LayarTerakhir.Review, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 18 - "Community 18"
Cohesion: 0.04
Nodes (28): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+20 more)

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (10): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor, Phase1World (+2 more)

### Community 20 - "Community 20"
Cohesion: 0.04
Nodes (28): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+20 more)

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 22 - "Community 22"
Cohesion: 0.07
Nodes (11): ClockPhoto, InterludeStreetCapture, Report, View, KosPhoto, Phase3NavInspection, VentPryPhoto, InterludeExterior (+3 more)

### Community 23 - "Community 23"
Cohesion: 0.06
Nodes (37): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+29 more)

### Community 24 - "Community 24"
Cohesion: 0.05
Nodes (22): _fbx_of_U(), main(), sample_surface(), solve(), evaluate(), jacobian(), residual(), tip() (+14 more)

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (32): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+24 more)

### Community 26 - "Community 26"
Cohesion: 0.09
Nodes (11): Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, Report, Usage, Phase5MechanicalPresentation (+3 more)

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 28 - "Community 28"
Cohesion: 0.08
Nodes (11): CrosshairDotTests, FoundationAuditTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+3 more)

### Community 29 - "Community 29"
Cohesion: 0.07
Nodes (7): PrologCompletionAudit, Campaign, Hud, Phone, Receipt, GameplayRecoveryRuntimeTests, RecoveryTestInputSettings

### Community 30 - "Community 30"
Cohesion: 0.06
Nodes (15): InterludeWorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId (+7 more)

### Community 31 - "Community 31"
Cohesion: 0.06
Nodes (14): KosGroundWallTests, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Bike, InterludeTraffic, KosCorridorDirector (+6 more)

### Community 32 - "Community 32"
Cohesion: 0.12
Nodes (8): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4World, Phase4Zone

### Community 33 - "Community 33"
Cohesion: 0.07
Nodes (26): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+18 more)

### Community 34 - "Community 34"
Cohesion: 0.06
Nodes (32): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+24 more)

### Community 35 - "Community 35"
Cohesion: 0.05
Nodes (32): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 36 - "Community 36"
Cohesion: 0.11
Nodes (9): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Anchor, Phase3World (+1 more)

### Community 37 - "Community 37"
Cohesion: 0.13
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 38 - "Community 38"
Cohesion: 0.10
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 39 - "Community 39"
Cohesion: 0.08
Nodes (19): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+11 more)

### Community 40 - "Community 40"
Cohesion: 0.10
Nodes (11): FoundationAuditCoroutineHost, Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext (+3 more)

### Community 41 - "Community 41"
Cohesion: 0.06
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 42 - "Community 42"
Cohesion: 0.06
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 44 - "Community 44"
Cohesion: 0.08
Nodes (13): AmbientAudioLoop, LevelMultiplier, SceneAudioProfile, PresentationProfile, Phase1Balance, Phase3Sound, Phase3Soundscape, Voice (+5 more)

### Community 45 - "Community 45"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 46 - "Community 46"
Cohesion: 0.09
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 47 - "Community 47"
Cohesion: 0.18
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 48 - "Community 48"
Cohesion: 0.10
Nodes (11): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+3 more)

### Community 49 - "Community 49"
Cohesion: 0.05
Nodes (31): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 51 - "Community 51"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 53 - "Community 53"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 54 - "Community 54"
Cohesion: 0.13
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 55 - "Community 55"
Cohesion: 0.09
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 56 - "Community 56"
Cohesion: 0.10
Nodes (14): AtmajaGeometryAudit, Entry, Inventory, KosGroundWallRepair, BoundsReport, Entry, KosRenderDiagnosis, Metric (+6 more)

### Community 57 - "Community 57"
Cohesion: 0.09
Nodes (4): Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 58 - "Community 58"
Cohesion: 0.10
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 59 - "Community 59"
Cohesion: 0.09
Nodes (16): Phase2Balance, LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri (+8 more)

### Community 60 - "Community 60"
Cohesion: 0.16
Nodes (4): FeastPhoto, Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 61 - "Community 61"
Cohesion: 0.11
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 62 - "Community 62"
Cohesion: 0.15
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 63 - "Community 63"
Cohesion: 0.14
Nodes (6): GoldenAudioTap, Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 64 - "Community 64"
Cohesion: 0.17
Nodes (5): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 65 - "Community 65"
Cohesion: 0.07
Nodes (13): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+5 more)

### Community 68 - "Community 68"
Cohesion: 0.15
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 71 - "Community 71"
Cohesion: 0.08
Nodes (23): Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State (+15 more)

### Community 73 - "Community 73"
Cohesion: 0.19
Nodes (18): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+10 more)

### Community 74 - "Community 74"
Cohesion: 0.11
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 77 - "Community 77"
Cohesion: 0.13
Nodes (13): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+5 more)

### Community 78 - "Community 78"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 79 - "Community 79"
Cohesion: 0.15
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 80 - "Community 80"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 83 - "Community 83"
Cohesion: 0.12
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 85 - "Community 85"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 86 - "Community 86"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 87 - "Community 87"
Cohesion: 0.18
Nodes (6): DesignerVrmBuilder, AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 88 - "Community 88"
Cohesion: 0.17
Nodes (5): Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests, Phase2NavigationProbe

### Community 89 - "Community 89"
Cohesion: 0.22
Nodes (3): GameInputConfig, Instance, SystemShortcutHeld

### Community 90 - "Community 90"
Cohesion: 0.15
Nodes (9): GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos (+1 more)

### Community 93 - "Community 93"
Cohesion: 0.21
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 94 - "Community 94"
Cohesion: 0.19
Nodes (4): AnimatedRakaAvatar, CinematicCrouch, Installing, MotorcycleRiderAlignment

### Community 95 - "Community 95"
Cohesion: 0.16
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 97 - "Community 97"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 98 - "Community 98"
Cohesion: 0.15
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 99 - "Community 99"
Cohesion: 0.15
Nodes (4): DevelopmentScreenshotCapture, Phase4Lighting, Phase5SpectralPresentation, ClockAmbience

### Community 100 - "Community 100"
Cohesion: 0.26
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 101 - "Community 101"
Cohesion: 0.15
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 102 - "Community 102"
Cohesion: 0.21
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 103 - "Community 103"
Cohesion: 0.26
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 105 - "Community 105"
Cohesion: 0.17
Nodes (4): opening_structure_sha256(), verify_opening_source(), GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 106 - "Community 106"
Cohesion: 0.16
Nodes (7): fetch(), fetch_asset(), get(), main(), subdir(), asset(), fetch()

### Community 108 - "Community 108"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 110 - "Community 110"
Cohesion: 0.21
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 111 - "Community 111"
Cohesion: 0.16
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 112 - "Community 112"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 120 - "Community 120"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 121 - "Community 121"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 122 - "Community 122"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 125 - "Community 125"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 126 - "Community 126"
Cohesion: 0.24
Nodes (5): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath

### Community 127 - "Community 127"
Cohesion: 0.35
Nodes (8): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow()

### Community 128 - "Community 128"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 129 - "Community 129"
Cohesion: 0.24
Nodes (6): apply_pose(), build_map(), fk_chain(), main(), render_to(), snapshot_action_bases()

### Community 130 - "Community 130"
Cohesion: 0.29
Nodes (6): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 131 - "Community 131"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 132 - "Community 132"
Cohesion: 0.20
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 133 - "Community 133"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 134 - "Community 134"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 135 - "Community 135"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 136 - "Community 136"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 137 - "Community 137"
Cohesion: 0.33
Nodes (3): RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 138 - "Community 138"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 140 - "Community 140"
Cohesion: 0.36
Nodes (3): Diagnosis, KosVisualCapture, VisualTelemetryReport

### Community 141 - "Community 141"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 144 - "Community 144"
Cohesion: 0.25
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 149 - "Community 149"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 150 - "Community 150"
Cohesion: 0.25
Nodes (8): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WallCalendar, WillNote

### Community 156 - "Community 156"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 159 - "Community 159"
Cohesion: 0.47
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 161 - "Community 161"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 162 - "Community 162"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 163 - "Community 163"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **691 isolated node(s):** `Campaign`, `Detection`, `Hud`, `InspectingLocker`, `IsBlitzStunned` (+686 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1117 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **55 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 41` to `Community 0`, `Community 2`, `Community 4`, `Community 6`, `Community 8`, `Community 138`, `Community 11`, `Community 12`, `Community 13`, `Community 14`, `Community 139`, `Community 18`, `Community 19`, `Community 21`, `Community 22`, `Community 23`, `Community 26`, `Community 27`, `Community 28`, `Community 158`, `Community 30`, `Community 32`, `Community 33`, `Community 34`, `Community 35`, `Community 36`, `Community 40`, `Community 42`, `Community 44`, `Community 45`, `Community 49`, `Community 54`, `Community 56`, `Community 59`, `Community 62`, `Community 63`, `Community 65`, `Community 77`, `Community 78`, `Community 81`, `Community 86`, `Community 89`, `Community 99`, `Community 100`, `Community 102`?**
  _High betweenness centrality (0.231) - this node is a cross-community bridge._
- **What connects `Campaign`, `Detection`, `Hud` to the rest of the system?**
  _691 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.04036995375578053 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 112` to `Community 2`, `Community 99`, `Community 98`, `Community 6`, `Community 11`, `Community 44`, `Community 13`, `Community 47`, `Community 18`, `Community 19`, `Community 23`, `Community 94`?**
  _High betweenness centrality (0.047) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.039184326269492205 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 5` to `Community 0`, `Community 7`, `Community 8`, `Community 10`, `Community 11`, `Community 13`, `Community 14`, `Community 15`, `Community 18`, `Community 28`, `Community 29`, `Community 33`, `Community 35`, `Community 37`, `Community 40`, `Community 42`, `Community 43`, `Community 49`, `Community 50`, `Community 54`, `Community 65`, `Community 68`, `Community 74`, `Community 77`, `Community 86`, `Community 99`, `Community 102`?**
  _High betweenness centrality (0.047) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.07947232947232948 - nodes in this community are weakly interconnected._
