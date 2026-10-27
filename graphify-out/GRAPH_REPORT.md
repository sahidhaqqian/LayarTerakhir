# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4426 nodes · 16814 edges · 142 communities (108 shown, 34 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1169 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `af85402d`
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
- Community 135
- Community 136
- Community 137
- Community 138
- Community 139
- Community 140

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 138 edges
2. `LayarTerakhir.Core` - 135 edges
3. `Phase5Director` - 129 edges
4. `Phase4Director` - 122 edges
5. `AtmajaDirector` - 120 edges
6. `Phase2Director` - 116 edges
7. `Phase3Director` - 114 edges
8. `ShoulderCameraRig` - 111 edges
9. `AtmajaPhoneView` - 111 edges
10. `LayarTerakhir.Player` - 99 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase3SceneTests` --references--> `Phase3Director`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/Phase3SceneTests.cs → Assets/_LayarTerakhir/src/Phase3/Phase3Director.cs

## Import Cycles
- None detected.

## Communities (142 total, 34 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (42): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+34 more)

### Community 1 - "Community 1"
Cohesion: 0.10
Nodes (7): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (8): asset(), download(), load_meta(), main(), snapshot(), main(), main(), sha()

### Community 4 - "Community 4"
Cohesion: 0.08
Nodes (4): Phase5Ids, Phase5CampaignTests, Slot, Phase5Campaign

### Community 6 - "Community 6"
Cohesion: 0.05
Nodes (6): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (20): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+12 more)

### Community 9 - "Community 9"
Cohesion: 0.07
Nodes (30): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+22 more)

### Community 10 - "Community 10"
Cohesion: 0.09
Nodes (9): CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, Check, KosSceneValidation, Report, KosVisualV2 (+1 more)

### Community 11 - "Community 11"
Cohesion: 0.07
Nodes (13): BloodWriting, KosGroundWallRepair, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage (+5 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (26): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+18 more)

### Community 13 - "Community 13"
Cohesion: 0.08
Nodes (13): MatManifest, Phase5SceneBuilder, SourceEntry, SourceList, MatManifest, MatRow, MeshBinding, Report (+5 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (5): DesignerRevisionTests, FoundationAuditTests, KosVisualV2Tests, Phase5SceneTests, Phase5AssetSource

### Community 15 - "Community 15"
Cohesion: 0.08
Nodes (15): GoldenHudBuilder, Phase2Content, Phase4Content, HudStyle, PhoneApp, Bank, Call, Camera (+7 more)

### Community 16 - "Community 16"
Cohesion: 0.08
Nodes (7): KosVisualUpgrade, VisualAudit, Phase2NavigationProbe, PlaytestReviewValidation, Record, TempShaftProbe, KosRoomCorrections

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (8): DreadoutVisualRevision, Report, SceneReport, Phase3NavInspection, PlaytestRepairBuilder, Phase1LightBudget, Phase4FearPresentation, PracticalLightPresentation

### Community 18 - "Community 18"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 19 - "Community 19"
Cohesion: 0.12
Nodes (5): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1Anchor

### Community 20 - "Community 20"
Cohesion: 0.06
Nodes (16): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId, MelatiDeparture, InteractableId (+8 more)

### Community 21 - "Community 21"
Cohesion: 0.05
Nodes (23): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+15 more)

### Community 22 - "Community 22"
Cohesion: 0.07
Nodes (15): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+7 more)

### Community 23 - "Community 23"
Cohesion: 0.13
Nodes (8): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4World, Phase4Zone

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (31): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+23 more)

### Community 25 - "Community 25"
Cohesion: 0.10
Nodes (9): FoundationPlayAuditTests, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 26 - "Community 26"
Cohesion: 0.13
Nodes (10): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor (+2 more)

### Community 27 - "Community 27"
Cohesion: 0.05
Nodes (15): ball(), box(), cylinder(), mat(), tube(), box(), cylinder(), finish() (+7 more)

### Community 28 - "Community 28"
Cohesion: 0.06
Nodes (19): GraphicsReferenceAudit, Receipt, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar (+11 more)

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 30 - "Community 30"
Cohesion: 0.12
Nodes (9): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry (+1 more)

### Community 31 - "Community 31"
Cohesion: 0.09
Nodes (15): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance, Phase5Boss, Active, B, Exposed (+7 more)

### Community 32 - "Community 32"
Cohesion: 0.07
Nodes (21): Phase1Navigator, Phase1Projectile, KuntiState, Dormant, Manifested, Pursuit, Resolved, Phase1Threats (+13 more)

### Community 33 - "Community 33"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 34 - "Community 34"
Cohesion: 0.11
Nodes (5): Phase2Ids, Phase3Ids, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign

### Community 35 - "Community 35"
Cohesion: 0.10
Nodes (10): PresentationProfile, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+2 more)

### Community 36 - "Community 36"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Hub, CameraLocked (+14 more)

### Community 37 - "Community 37"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 38 - "Community 38"
Cohesion: 0.12
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 39 - "Community 39"
Cohesion: 0.05
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 41 - "Community 41"
Cohesion: 0.05
Nodes (33): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 42 - "Community 42"
Cohesion: 0.07
Nodes (11): DevelopmentScreenshotCapture, Frame, LocationLightingAudit, Receipt, FoundationAuditCoroutineHost, Phase4Lighting, ExplorationCommentary, Zone (+3 more)

### Community 43 - "Community 43"
Cohesion: 0.07
Nodes (27): AtmajaPlayerAudit, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock (+19 more)

### Community 44 - "Community 44"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 45 - "Community 45"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 46 - "Community 46"
Cohesion: 0.12
Nodes (3): Phase4Torch, Authentic, PermanentlyOff

### Community 47 - "Community 47"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 48 - "Community 48"
Cohesion: 0.13
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 49 - "Community 49"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 50 - "Community 50"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 51 - "Community 51"
Cohesion: 0.15
Nodes (8): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, HingedDoor, InteractableId, IsOpen

### Community 52 - "Community 52"
Cohesion: 0.09
Nodes (4): Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 53 - "Community 53"
Cohesion: 0.14
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 54 - "Community 54"
Cohesion: 0.11
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 55 - "Community 55"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 56 - "Community 56"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 57 - "Community 57"
Cohesion: 0.08
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 58 - "Community 58"
Cohesion: 0.08
Nodes (7): dialogue_literals(), export(), opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 59 - "Community 59"
Cohesion: 0.17
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 60 - "Community 60"
Cohesion: 0.10
Nodes (8): AmbientAudioLoop, LevelMultiplier, ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape, Phase4Sound, Phase4Soundscape

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (5): MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 62 - "Community 62"
Cohesion: 0.10
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 63 - "Community 63"
Cohesion: 0.15
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 65 - "Community 65"
Cohesion: 0.13
Nodes (7): DesignerReviewPlayerAudit, Hud, Phone, Receipt, InteractionInteractor, ContextResolver, CurrentTarget

### Community 68 - "Community 68"
Cohesion: 0.13
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 69 - "Community 69"
Cohesion: 0.11
Nodes (6): KosGroundWallTests, DoubleLeafGate, IsOpen, KosCorridorDirector, Busy, CanDepart

### Community 72 - "Community 72"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 74 - "Community 74"
Cohesion: 0.17
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 75 - "Community 75"
Cohesion: 0.11
Nodes (4): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Ids

### Community 76 - "Community 76"
Cohesion: 0.19
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 77 - "Community 77"
Cohesion: 0.14
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 78 - "Community 78"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 79 - "Community 79"
Cohesion: 0.09
Nodes (19): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+11 more)

### Community 80 - "Community 80"
Cohesion: 0.13
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 81 - "Community 81"
Cohesion: 0.13
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 82 - "Community 82"
Cohesion: 0.13
Nodes (10): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph (+2 more)

### Community 83 - "Community 83"
Cohesion: 0.14
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 84 - "Community 84"
Cohesion: 0.12
Nodes (10): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+2 more)

### Community 86 - "Community 86"
Cohesion: 0.15
Nodes (9): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape (+1 more)

### Community 88 - "Community 88"
Cohesion: 0.15
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 89 - "Community 89"
Cohesion: 0.19
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 90 - "Community 90"
Cohesion: 0.16
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 91 - "Community 91"
Cohesion: 0.14
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 92 - "Community 92"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 93 - "Community 93"
Cohesion: 0.22
Nodes (11): get(), main(), obj(), ph(), digest(), get(), main(), objaverse() (+3 more)

### Community 94 - "Community 94"
Cohesion: 0.21
Nodes (9): Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 95 - "Community 95"
Cohesion: 0.12
Nodes (6): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime, ClockAmbience

### Community 98 - "Community 98"
Cohesion: 0.16
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 99 - "Community 99"
Cohesion: 0.19
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 102 - "Community 102"
Cohesion: 0.19
Nodes (4): AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 104 - "Community 104"
Cohesion: 0.23
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 106 - "Community 106"
Cohesion: 0.23
Nodes (3): AtmajaSceneValidation, OpeningBaseline, Report

### Community 108 - "Community 108"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 109 - "Community 109"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 110 - "Community 110"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 112 - "Community 112"
Cohesion: 0.25
Nodes (3): AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 113 - "Community 113"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 114 - "Community 114"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 115 - "Community 115"
Cohesion: 0.33
Nodes (5): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric

### Community 117 - "Community 117"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 118 - "Community 118"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 119 - "Community 119"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 120 - "Community 120"
Cohesion: 0.36
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 121 - "Community 121"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 124 - "Community 124"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 125 - "Community 125"
Cohesion: 0.53
Nodes (3): RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 127 - "Community 127"
Cohesion: 0.60
Nodes (3): Metric, NyonyaMotionRevisionBuilder, Report

### Community 128 - "Community 128"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 129 - "Community 129"
Cohesion: 0.40
Nodes (3): Phase4MaterialProbe, Report, Row

### Community 130 - "Community 130"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **614 isolated node(s):** `FloorY`, `CanContinue`, `ContractRead`, `IsCompletionVisible`, `IsContractVisible` (+609 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 956 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **34 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 57` to `Community 1`, `Community 5`, `Community 7`, `Community 8`, `Community 9`, `Community 10`, `Community 11`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 19`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 25`, `Community 26`, `Community 28`, `Community 29`, `Community 33`, `Community 35`, `Community 36`, `Community 37`, `Community 38`, `Community 39`, `Community 41`, `Community 42`, `Community 46`, `Community 49`, `Community 53`, `Community 60`, `Community 64`, `Community 65`, `Community 68`, `Community 71`, `Community 74`, `Community 77`, `Community 78`, `Community 83`, `Community 85`, `Community 89`, `Community 90`, `Community 91`, `Community 94`, `Community 104`?**
  _High betweenness centrality (0.235) - this node is a cross-community bridge._
- **What connects `FloorY`, `CanContinue`, `ContractRead` to the rest of the system?**
  _614 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05021929824561404 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 54` to `Community 3`, `Community 4`, `Community 7`, `Community 8`, `Community 12`, `Community 14`, `Community 18`, `Community 22`, `Community 25`, `Community 29`, `Community 34`, `Community 36`, `Community 37`, `Community 38`, `Community 39`, `Community 41`, `Community 42`, `Community 53`, `Community 70`, `Community 71`, `Community 72`, `Community 75`, `Community 90`, `Community 94`, `Community 116`?**
  _High betweenness centrality (0.066) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.09720044792833146 - nodes in this community are weakly interconnected._
- **Why does `Phase4Director` connect `Community 39` to `Community 129`, `Community 1`, `Community 6`, `Community 10`, `Community 11`, `Community 15`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 22`, `Community 23`, `Community 25`, `Community 31`, `Community 35`, `Community 36`, `Community 42`, `Community 46`, `Community 52`, `Community 54`, `Community 56`, `Community 57`, `Community 60`, `Community 61`, `Community 67`, `Community 73`, `Community 78`, `Community 79`, `Community 81`, `Community 86`, `Community 90`, `Community 91`, `Community 95`?**
  _High betweenness centrality (0.063) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.05303392259913999 - nodes in this community are weakly interconnected._
