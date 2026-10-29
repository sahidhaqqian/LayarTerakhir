# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5067 nodes · 19254 edges · 158 communities (124 shown, 34 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1274 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `aaf9d634`
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

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 156 edges
2. `LayarTerakhir.Core` - 156 edges
3. `Phase5Director` - 129 edges
4. `AtmajaDirector` - 123 edges
5. `Phase4Director` - 122 edges
6. `ShoulderCameraRig` - 117 edges
7. `Phase2Director` - 116 edges
8. `FirstPersonController` - 114 edges
9. `AtmajaPhoneView` - 114 edges
10. `Phase3Director` - 114 edges

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

## Communities (158 total, 34 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (59): asset(), download(), asset(), fetch(), dialogue_literals(), export(), fetch(), main() (+51 more)

### Community 1 - "Community 1"
Cohesion: 0.03
Nodes (13): HidingAwareness, DesignerRevisionTests, DeveloperReviewModeTests, HidingAwarenessTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase2SceneTests (+5 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (8): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 3 - "Community 3"
Cohesion: 0.07
Nodes (13): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+5 more)

### Community 4 - "Community 4"
Cohesion: 0.05
Nodes (23): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+15 more)

### Community 7 - "Community 7"
Cohesion: 0.07
Nodes (21): MotorcycleRiderAlignment, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+13 more)

### Community 8 - "Community 8"
Cohesion: 0.06
Nodes (33): sample_surface(), solve(), evaluate(), jacobian(), residual(), child(), clusters(), encoded() (+25 more)

### Community 9 - "Community 9"
Cohesion: 0.04
Nodes (6): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Interlude, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (7): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder, DesignerWorldRevision, AtmajaVault

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (14): NyonyaMotionTests, Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, Report, Usage (+6 more)

### Community 12 - "Community 12"
Cohesion: 0.07
Nodes (12): FoundationAuditTests, GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 13 - "Community 13"
Cohesion: 0.06
Nodes (31): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+23 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (17): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+9 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (24): KosGroundWallRepair, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Balance (+16 more)

### Community 16 - "Community 16"
Cohesion: 0.04
Nodes (30): PlayerFootsteps, LastSurface, PlayedSteps, KosGroundWallTests, PlaytestReviewValidation, Record, AtmajaDirector, Busy (+22 more)

### Community 17 - "Community 17"
Cohesion: 0.07
Nodes (31): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 18 - "Community 18"
Cohesion: 0.07
Nodes (6): KosVisualUpgrade, VisualAudit, Phase2NavigationProbe, PrologReviewRevisionTests, TempShaftProbe, KosRoomCorrections

### Community 19 - "Community 19"
Cohesion: 0.06
Nodes (29): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+21 more)

### Community 20 - "Community 20"
Cohesion: 0.08
Nodes (18): Bindings, MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, SourceBinding, SceneRecord, Usage (+10 more)

### Community 21 - "Community 21"
Cohesion: 0.10
Nodes (12): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, RoomDoorState, CasedOpen (+4 more)

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (6): CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, KosVisualV2, FloorY

### Community 23 - "Community 23"
Cohesion: 0.10
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (4): Phase2Ids, Phase2RitualTransactionTests, Phase3CampaignTests, Phase3Campaign

### Community 25 - "Community 25"
Cohesion: 0.08
Nodes (8): BloodWriting, MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer, AtmajaWorldText

### Community 26 - "Community 26"
Cohesion: 0.12
Nodes (10): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor (+2 more)

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (10): FoundationPlayAuditTests, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked (+2 more)

### Community 28 - "Community 28"
Cohesion: 0.09
Nodes (4): Phase1Content, LayarTerakhir.Phase2, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 29 - "Community 29"
Cohesion: 0.10
Nodes (13): FootstepSurface, Earth, Stone, Tile, Wood, VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder (+5 more)

### Community 30 - "Community 30"
Cohesion: 0.06
Nodes (19): GraphicsReferenceAudit, Receipt, RecoveryVisualEvidence, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint (+11 more)

### Community 31 - "Community 31"
Cohesion: 0.13
Nodes (9): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry (+1 more)

### Community 32 - "Community 32"
Cohesion: 0.07
Nodes (29): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+21 more)

### Community 33 - "Community 33"
Cohesion: 0.09
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 34 - "Community 34"
Cohesion: 0.09
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 35 - "Community 35"
Cohesion: 0.08
Nodes (11): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, MelatiDeparture, InteractableId, MelatiShop, InteractableId (+3 more)

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (5): Phase3NavInspection, PlaytestRepairBuilder, Phase1LightBudget, Phase4FearPresentation, PracticalLightPresentation

### Community 37 - "Community 37"
Cohesion: 0.07
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 38 - "Community 38"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 39 - "Community 39"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 40 - "Community 40"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 41 - "Community 41"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 42 - "Community 42"
Cohesion: 0.06
Nodes (31): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 43 - "Community 43"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 44 - "Community 44"
Cohesion: 0.06
Nodes (9): ball(), box(), cylinder(), mat(), tube(), export(), meshes(), jagged_hole() (+1 more)

### Community 45 - "Community 45"
Cohesion: 0.11
Nodes (4): CampaignRouting, ReviewDirectBoot, ReviewDirectBootTests, Slot

### Community 46 - "Community 46"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 47 - "Community 47"
Cohesion: 0.10
Nodes (5): PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, RecoveryTestInputSettings

### Community 48 - "Community 48"
Cohesion: 0.10
Nodes (9): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape, Phase3Sound, Phase3Soundscape, Voice, Phase4Sound (+1 more)

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 50 - "Community 50"
Cohesion: 0.09
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 52 - "Community 52"
Cohesion: 0.13
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 54 - "Community 54"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 55 - "Community 55"
Cohesion: 0.16
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 56 - "Community 56"
Cohesion: 0.13
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 57 - "Community 57"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 58 - "Community 58"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 59 - "Community 59"
Cohesion: 0.21
Nodes (3): CutVertex, PrologReviewRevision, Receipt

### Community 60 - "Community 60"
Cohesion: 0.15
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 61 - "Community 61"
Cohesion: 0.11
Nodes (4): AtmajaPlayerAudit, Receipt, Receipt, RiderPresentationAudit

### Community 62 - "Community 62"
Cohesion: 0.07
Nodes (13): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+5 more)

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 65 - "Community 65"
Cohesion: 0.12
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 66 - "Community 66"
Cohesion: 0.10
Nodes (5): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource

### Community 67 - "Community 67"
Cohesion: 0.10
Nodes (13): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+5 more)

### Community 68 - "Community 68"
Cohesion: 0.12
Nodes (7): Frame, LocationLightingAudit, Receipt, AbandonedLocationLightingTests, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 70 - "Community 70"
Cohesion: 0.11
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 71 - "Community 71"
Cohesion: 0.11
Nodes (18): Phase1Balance, Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign (+10 more)

### Community 72 - "Community 72"
Cohesion: 0.13
Nodes (16): build_cup(), crack_angle(), handle(), inspect_meshes(), material(), milk_surface(), outer_radius(), shell() (+8 more)

### Community 73 - "Community 73"
Cohesion: 0.13
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 74 - "Community 74"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 75 - "Community 75"
Cohesion: 0.18
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 76 - "Community 76"
Cohesion: 0.10
Nodes (5): opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 77 - "Community 77"
Cohesion: 0.20
Nodes (3): InteractionInteractor, ContextResolver, CurrentTarget

### Community 78 - "Community 78"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 79 - "Community 79"
Cohesion: 0.08
Nodes (22): Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining (+14 more)

### Community 80 - "Community 80"
Cohesion: 0.13
Nodes (13): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+5 more)

### Community 81 - "Community 81"
Cohesion: 0.13
Nodes (7): fetch(), main(), main(), prepare(), foley(), render(), write_pcm()

### Community 82 - "Community 82"
Cohesion: 0.12
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 83 - "Community 83"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 84 - "Community 84"
Cohesion: 0.12
Nodes (12): cloth(), ellipsoid(), finish(), mesh(), tube(), is_arm(), main(), delete_fill_tagged() (+4 more)

### Community 85 - "Community 85"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 86 - "Community 86"
Cohesion: 0.15
Nodes (10): Phase3Content, Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 88 - "Community 88"
Cohesion: 0.13
Nodes (5): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 89 - "Community 89"
Cohesion: 0.14
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 92 - "Community 92"
Cohesion: 0.25
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 93 - "Community 93"
Cohesion: 0.14
Nodes (8): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, ReviewFlyCamera, Active, Eye

### Community 95 - "Community 95"
Cohesion: 0.13
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 96 - "Community 96"
Cohesion: 0.14
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 97 - "Community 97"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 99 - "Community 99"
Cohesion: 0.19
Nodes (7): AtmajaGeometryAudit, Entry, Inventory, Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 100 - "Community 100"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 101 - "Community 101"
Cohesion: 0.21
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 103 - "Community 103"
Cohesion: 0.23
Nodes (3): SceneAudioProfile, PresentationProfile, GoldenSceneProduction

### Community 105 - "Community 105"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 106 - "Community 106"
Cohesion: 0.18
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 107 - "Community 107"
Cohesion: 0.21
Nodes (3): AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 108 - "Community 108"
Cohesion: 0.17
Nodes (3): DevelopmentScreenshotCapture, Phase4Lighting, ClockAmbience

### Community 109 - "Community 109"
Cohesion: 0.26
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 110 - "Community 110"
Cohesion: 0.16
Nodes (4): Bike, InterludeTraffic, InterludeWorldItem, InteractableId

### Community 111 - "Community 111"
Cohesion: 0.17
Nodes (8): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube()

### Community 112 - "Community 112"
Cohesion: 0.16
Nodes (8): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Lighting

### Community 114 - "Community 114"
Cohesion: 0.19
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 118 - "Community 118"
Cohesion: 0.22
Nodes (3): Phase5Sound, Phase5Soundscape, Phase5UnstablePlank

### Community 120 - "Community 120"
Cohesion: 0.26
Nodes (8): InterludeStreetCapture, Report, View, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric

### Community 121 - "Community 121"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 122 - "Community 122"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 124 - "Community 124"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 125 - "Community 125"
Cohesion: 0.23
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 127 - "Community 127"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 128 - "Community 128"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 131 - "Community 131"
Cohesion: 0.20
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 133 - "Community 133"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 134 - "Community 134"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 135 - "Community 135"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 136 - "Community 136"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 137 - "Community 137"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 142 - "Community 142"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 144 - "Community 144"
Cohesion: 0.33
Nodes (3): FoundationAuditCoroutineHost, RecoveryPresentationProbe, RecoveryRuntimeScenario

### Community 146 - "Community 146"
Cohesion: 0.48
Nodes (3): RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 147 - "Community 147"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 149 - "Community 149"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 150 - "Community 150"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 152 - "Community 152"
Cohesion: 0.60
Nodes (3): Metric, NyonyaMotionRevisionBuilder, Report

### Community 153 - "Community 153"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 154 - "Community 154"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **674 isolated node(s):** `ExcursionActive`, `ExcursionTarget`, `Loading`, `OriginScene`, `TempDirectory` (+669 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1053 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **34 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 37` to `Community 2`, `Community 5`, `Community 7`, `Community 10`, `Community 11`, `Community 12`, `Community 13`, `Community 138`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 22`, `Community 25`, `Community 26`, `Community 27`, `Community 30`, `Community 32`, `Community 34`, `Community 42`, `Community 43`, `Community 44`, `Community 45`, `Community 47`, `Community 48`, `Community 60`, `Community 62`, `Community 63`, `Community 68`, `Community 71`, `Community 73`, `Community 77`, `Community 78`, `Community 80`, `Community 85`, `Community 86`, `Community 88`, `Community 96`, `Community 99`, `Community 104`, `Community 108`, `Community 112`, `Community 113`, `Community 114`?**
  _High betweenness centrality (0.234) - this node is a cross-community bridge._
- **What connects `ExcursionActive`, `ExcursionTarget`, `Loading` to the rest of the system?**
  _674 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.04038095238095238 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 3` to `Community 1`, `Community 129`, `Community 6`, `Community 12`, `Community 14`, `Community 16`, `Community 17`, `Community 19`, `Community 23`, `Community 24`, `Community 27`, `Community 33`, `Community 39`, `Community 42`, `Community 43`, `Community 45`, `Community 47`, `Community 50`, `Community 51`, `Community 52`, `Community 62`, `Community 67`, `Community 71`, `Community 74`, `Community 75`, `Community 77`, `Community 80`, `Community 85`, `Community 86`, `Community 90`, `Community 98`, `Community 108`?**
  _High betweenness centrality (0.087) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.02696774193548387 - nodes in this community are weakly interconnected._
- **Why does `Phase5Director` connect `Community 17` to `Community 1`, `Community 2`, `Community 3`, `Community 7`, `Community 9`, `Community 11`, `Community 12`, `Community 14`, `Community 15`, `Community 148`, `Community 21`, `Community 22`, `Community 27`, `Community 157`, `Community 37`, `Community 49`, `Community 56`, `Community 62`, `Community 64`, `Community 68`, `Community 79`, `Community 80`, `Community 96`, `Community 99`, `Community 102`, `Community 108`, `Community 112`, `Community 118`, `Community 123`?**
  _High betweenness centrality (0.070) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.08443764865783214 - nodes in this community are weakly interconnected._
