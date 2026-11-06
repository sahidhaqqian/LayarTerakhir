# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5404 nodes · 20949 edges · 183 communities (125 shown, 58 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1312 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `4c69c8d5`
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
- Community 168
- Community 169
- Community 170
- Community 171
- Community 172
- Community 173
- Community 174
- Community 175
- Community 176
- Community 177
- Community 178
- Community 179
- Community 180
- Community 182

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 190 edges
2. `LayarTerakhir.Core` - 160 edges
3. `CampaignState` - 156 edges
4. `ShoulderCameraRig` - 133 edges
5. `Phase5Director` - 129 edges
6. `FirstPersonController` - 126 edges
7. `Phase4Director` - 122 edges
8. `LayarTerakhir.Player` - 119 edges
9. `AtmajaPhoneView` - 118 edges
10. `HudController` - 117 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `AtmajaStairRevisionBuilder` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/AtmajaStairRevisionBuilder.cs → Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs

## Import Cycles
- None detected.

## Communities (183 total, 58 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (14): Phase5Content, KosDialogue, LayarTerakhir.Review, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Interlude, LayarTerakhir.Audio, LayarTerakhir.Phase4 (+6 more)

### Community 1 - "Community 1"
Cohesion: 0.04
Nodes (58): asset(), download(), asset(), fetch(), dialogue_literals(), export(), copy(), sha() (+50 more)

### Community 2 - "Community 2"
Cohesion: 0.03
Nodes (12): HidingAwareness, HidingAwarenessTests, KosBathroomRevisionTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase4SceneTests (+4 more)

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (11): asset(), download(), load_meta(), export(), foley(), main(), snapshot(), note() (+3 more)

### Community 5 - "Community 5"
Cohesion: 0.07
Nodes (5): AtmajaReviewDressing, CompactKosPolish, GoldenSceneProduction, KosVisualV2, FloorY

### Community 6 - "Community 6"
Cohesion: 0.11
Nodes (4): KosGroundWallRepair, KosRoomSurvey, Phase3NavInspection, LayarTerakhir.Editor

### Community 7 - "Community 7"
Cohesion: 0.08
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 8 - "Community 8"
Cohesion: 0.05
Nodes (19): CampaignData, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data (+11 more)

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (13): PresentationProfile, CrosshairDotTests, FoundationAuditTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying (+5 more)

### Community 10 - "Community 10"
Cohesion: 0.07
Nodes (12): AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport (+4 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 13 - "Community 13"
Cohesion: 0.04
Nodes (28): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+20 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (14): DesignerVrmBuilder, Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests, PrologPresentationBuilder, Receipt, RakaPresentationRevisionBuilder (+6 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (16): FoundationPlayAuditTests, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked (+8 more)

### Community 16 - "Community 16"
Cohesion: 0.08
Nodes (9): BloodWriting, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor (+1 more)

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 18 - "Community 18"
Cohesion: 0.05
Nodes (20): PlayerFeelAudit, Receipt, Sample, KosGroundWallTests, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig (+12 more)

### Community 19 - "Community 19"
Cohesion: 0.07
Nodes (19): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+11 more)

### Community 20 - "Community 20"
Cohesion: 0.07
Nodes (15): ClockPhoto, InterludeStreetCapture, Report, View, KosPhoto, BoundsReport, Diagnosis, Entry (+7 more)

### Community 21 - "Community 21"
Cohesion: 0.06
Nodes (37): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+29 more)

### Community 22 - "Community 22"
Cohesion: 0.05
Nodes (22): _fbx_of_U(), main(), sample_surface(), solve(), evaluate(), jacobian(), residual(), tip() (+14 more)

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (32): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+24 more)

### Community 24 - "Community 24"
Cohesion: 0.08
Nodes (5): KosVisualUpgrade, VisualAudit, PlaytestReviewValidation, Record, KosRoomCorrections

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (10): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, PrologMilestoneValidator, KamarKosPrologDirector (+2 more)

### Community 26 - "Community 26"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 27 - "Community 27"
Cohesion: 0.11
Nodes (9): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Anchor, Phase3World (+1 more)

### Community 28 - "Community 28"
Cohesion: 0.09
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (3): Phase2RitualTransactionTests, Phase3CampaignTests, Phase3Campaign

### Community 30 - "Community 30"
Cohesion: 0.08
Nodes (26): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+18 more)

### Community 31 - "Community 31"
Cohesion: 0.08
Nodes (15): GoldenAudioTap, GoldenPlayerAudit, Receipt, Receipt, ReviewPlayerAudit, Hud, Phone, View (+7 more)

### Community 32 - "Community 32"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+14 more)

### Community 33 - "Community 33"
Cohesion: 0.12
Nodes (9): Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, Usage, Phase5MechanicalPresentation, Phase5Anchor (+1 more)

### Community 34 - "Community 34"
Cohesion: 0.06
Nodes (22): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+14 more)

### Community 35 - "Community 35"
Cohesion: 0.12
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 36 - "Community 36"
Cohesion: 0.11
Nodes (5): InterludeCampaignTests, PhaseRestartTests, Slot, InterludeCampaign, InterludeIds

### Community 37 - "Community 37"
Cohesion: 0.08
Nodes (9): MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Phase4MaterialProbe, Report, Row, PrologEasySceneRepair (+1 more)

### Community 38 - "Community 38"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 39 - "Community 39"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 40 - "Community 40"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 41 - "Community 41"
Cohesion: 0.05
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 42 - "Community 42"
Cohesion: 0.09
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 43 - "Community 43"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 45 - "Community 45"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 46 - "Community 46"
Cohesion: 0.10
Nodes (4): AtmajaPlayerAudit, Receipt, GameplayRecoveryRuntimeTests, RecoveryTestInputSettings

### Community 47 - "Community 47"
Cohesion: 0.16
Nodes (7): MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, MatManifest, Phase4Anchor, Phase4Zone

### Community 48 - "Community 48"
Cohesion: 0.14
Nodes (4): FeastPhoto, Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 50 - "Community 50"
Cohesion: 0.10
Nodes (12): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+4 more)

### Community 51 - "Community 51"
Cohesion: 0.06
Nodes (31): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 52 - "Community 52"
Cohesion: 0.09
Nodes (3): Phase5ReviewFixture, Phase5SaveCrashProbe, DeveloperReviewModeTests

### Community 53 - "Community 53"
Cohesion: 0.16
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 55 - "Community 55"
Cohesion: 0.09
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 56 - "Community 56"
Cohesion: 0.09
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 57 - "Community 57"
Cohesion: 0.16
Nodes (8): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, HingedDoor, InteractableId, IsOpen

### Community 58 - "Community 58"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 59 - "Community 59"
Cohesion: 0.12
Nodes (10): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+2 more)

### Community 61 - "Community 61"
Cohesion: 0.13
Nodes (7): FoundationAuditCoroutineHost, PrologEasyPlayTests, Director, RenderContext, RecoveryPresentationProbe, RecoveryRuntimeScenario, RecoveryVisualEvidence

### Community 62 - "Community 62"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (7): AmbientAudioLoop, LevelMultiplier, SceneAudioProfile, Phase3Sound, Phase3Soundscape, Voice, Phase4Soundscape

### Community 64 - "Community 64"
Cohesion: 0.14
Nodes (8): AtmajaItem, InteractableId, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 67 - "Community 67"
Cohesion: 0.10
Nodes (14): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+6 more)

### Community 68 - "Community 68"
Cohesion: 0.07
Nodes (26): Phase1Balance, Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering (+18 more)

### Community 72 - "Community 72"
Cohesion: 0.10
Nodes (18): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+10 more)

### Community 74 - "Community 74"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 75 - "Community 75"
Cohesion: 0.13
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 76 - "Community 76"
Cohesion: 0.08
Nodes (26): AtmajaAction, BathCurtain, BathScratches, CarpetLift, CellarClimb, Charge, Clock, ClockHands (+18 more)

### Community 77 - "Community 77"
Cohesion: 0.19
Nodes (18): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+10 more)

### Community 79 - "Community 79"
Cohesion: 0.13
Nodes (13): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+5 more)

### Community 80 - "Community 80"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 81 - "Community 81"
Cohesion: 0.19
Nodes (4): GoldenHudBuilder, HudStyle, PhoneGlyph, StoryMapGraphic

### Community 82 - "Community 82"
Cohesion: 0.16
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 83 - "Community 83"
Cohesion: 0.15
Nodes (3): PhoneDocument, Phase2CampaignTests, Phase2Campaign

### Community 84 - "Community 84"
Cohesion: 0.13
Nodes (9): DevelopmentScreenshotCapture, Phase2Drawer, Prompt, State, Phase2ThermalTarget, Phase2Anchor, Phase2Room, Phase2World (+1 more)

### Community 85 - "Community 85"
Cohesion: 0.13
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 87 - "Community 87"
Cohesion: 0.15
Nodes (3): Phase2Ids, Phase2WorldItem, InteractableId

### Community 89 - "Community 89"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 90 - "Community 90"
Cohesion: 0.12
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 92 - "Community 92"
Cohesion: 0.17
Nodes (7): Entry, Prolog47VisualTests, Folder, Survey, KosCorridorDirector, Busy, CanDepart

### Community 93 - "Community 93"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 94 - "Community 94"
Cohesion: 0.16
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 95 - "Community 95"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 96 - "Community 96"
Cohesion: 0.21
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 97 - "Community 97"
Cohesion: 0.12
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 98 - "Community 98"
Cohesion: 0.18
Nodes (3): VoiceLine, Phase5Sound, AtmajaSynth

### Community 99 - "Community 99"
Cohesion: 0.15
Nodes (3): SceneRecord, Report, Phase4Sound

### Community 100 - "Community 100"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 101 - "Community 101"
Cohesion: 0.18
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 102 - "Community 102"
Cohesion: 0.16
Nodes (4): GoldenLinuxBuild, Record, Phase2NavigationProbe, SceneRecord

### Community 105 - "Community 105"
Cohesion: 0.20
Nodes (4): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId

### Community 106 - "Community 106"
Cohesion: 0.19
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 107 - "Community 107"
Cohesion: 0.17
Nodes (4): opening_structure_sha256(), verify_opening_source(), GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 108 - "Community 108"
Cohesion: 0.16
Nodes (7): fetch(), fetch_asset(), get(), main(), subdir(), asset(), fetch()

### Community 109 - "Community 109"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 110 - "Community 110"
Cohesion: 0.19
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 112 - "Community 112"
Cohesion: 0.24
Nodes (6): AtmajaGeometryAudit, Entry, Inventory, Check, KosSceneValidation, Report

### Community 113 - "Community 113"
Cohesion: 0.18
Nodes (6): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached, NeighborhoodRain

### Community 116 - "Community 116"
Cohesion: 0.21
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 118 - "Community 118"
Cohesion: 0.22
Nodes (7): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase4World

### Community 121 - "Community 121"
Cohesion: 0.15
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 125 - "Community 125"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 126 - "Community 126"
Cohesion: 0.18
Nodes (5): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 127 - "Community 127"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 130 - "Community 130"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 132 - "Community 132"
Cohesion: 0.35
Nodes (8): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow()

### Community 133 - "Community 133"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 134 - "Community 134"
Cohesion: 0.24
Nodes (6): apply_pose(), build_map(), fk_chain(), main(), render_to(), snapshot_action_bases()

### Community 136 - "Community 136"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 137 - "Community 137"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 139 - "Community 139"
Cohesion: 0.24
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 140 - "Community 140"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 141 - "Community 141"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 142 - "Community 142"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 144 - "Community 144"
Cohesion: 0.22
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 145 - "Community 145"
Cohesion: 0.31
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 148 - "Community 148"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 150 - "Community 150"
Cohesion: 0.25
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 154 - "Community 154"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 156 - "Community 156"
Cohesion: 0.25
Nodes (8): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WallCalendar, WillNote

### Community 160 - "Community 160"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 161 - "Community 161"
Cohesion: 0.47
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 164 - "Community 164"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 165 - "Community 165"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 166 - "Community 166"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 168 - "Community 168"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 169 - "Community 169"
Cohesion: 0.40
Nodes (5): Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 176 - "Community 176"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **691 isolated node(s):** `ARRevealed`, `BasementEntry`, `BasementStealth`, `Chase`, `ClockUnlocked` (+686 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1103 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **58 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 34` to `Community 128`, `Community 0`, `Community 4`, `Community 5`, `Community 8`, `Community 9`, `Community 11`, `Community 12`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 151`, `Community 24`, `Community 25`, `Community 27`, `Community 28`, `Community 30`, `Community 31`, `Community 32`, `Community 162`, `Community 35`, `Community 40`, `Community 41`, `Community 44`, `Community 172`, `Community 176`, `Community 50`, `Community 51`, `Community 53`, `Community 55`, `Community 61`, `Community 63`, `Community 69`, `Community 72`, `Community 79`, `Community 80`, `Community 81`, `Community 84`, `Community 85`, `Community 89`, `Community 92`, `Community 93`, `Community 94`, `Community 98`, `Community 99`, `Community 102`?**
  _High betweenness centrality (0.195) - this node is a cross-community bridge._
- **What connects `ARRevealed`, `BasementEntry`, `BasementStealth` to the rest of the system?**
  _691 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.04665099882491187 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 7` to `Community 2`, `Community 8`, `Community 9`, `Community 12`, `Community 15`, `Community 25`, `Community 26`, `Community 28`, `Community 29`, `Community 30`, `Community 32`, `Community 35`, `Community 36`, `Community 38`, `Community 41`, `Community 46`, `Community 50`, `Community 51`, `Community 52`, `Community 64`, `Community 65`, `Community 67`, `Community 72`, `Community 74`, `Community 78`, `Community 79`, `Community 83`, `Community 84`, `Community 86`, `Community 91`, `Community 93`, `Community 111`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.039184326269492205 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 35` to `Community 128`, `Community 0`, `Community 4`, `Community 5`, `Community 7`, `Community 9`, `Community 10`, `Community 11`, `Community 14`, `Community 15`, `Community 20`, `Community 24`, `Community 27`, `Community 28`, `Community 31`, `Community 34`, `Community 37`, `Community 38`, `Community 171`, `Community 44`, `Community 172`, `Community 46`, `Community 48`, `Community 50`, `Community 53`, `Community 57`, `Community 61`, `Community 63`, `Community 64`, `Community 71`, `Community 74`, `Community 84`, `Community 85`, `Community 89`, `Community 91`, `Community 92`, `Community 94`, `Community 98`, `Community 100`, `Community 114`, `Community 126`?**
  _High betweenness centrality (0.048) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.03306939916162087 - nodes in this community are weakly interconnected._
