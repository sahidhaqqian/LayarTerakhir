# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4511 nodes · 16494 edges · 147 communities (108 shown, 39 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1162 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `1bcb2152`
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
- Community 144
- Community 145
- Community 146

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 126 edges
2. `Phase4Director` - 120 edges
3. `LayarTerakhir.Core` - 118 edges
4. `Phase2Director` - 112 edges
5. `AtmajaPhoneView` - 106 edges
6. `AtmajaDirector` - 99 edges
7. `Phase3Director` - 85 edges
8. `HudController` - 85 edges
9. `ShoulderCameraRig` - 85 edges
10. `LayarTerakhir.Prolog` - 85 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase4SceneBuilder` --references--> `Phase4Director`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/Phase4Materials.cs → Assets/_LayarTerakhir/src/Phase4/Phase4Director.cs

## Import Cycles
- None detected.

## Communities (147 total, 39 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.06
Nodes (15): SceneManifest, MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase5SceneBuilder, Report (+7 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (44): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+36 more)

### Community 3 - "Community 3"
Cohesion: 0.05
Nodes (8): asset(), download(), load_meta(), main(), snapshot(), main(), main(), sha()

### Community 4 - "Community 4"
Cohesion: 0.10
Nodes (7): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 7 - "Community 7"
Cohesion: 0.04
Nodes (16): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+8 more)

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (11): AtmajaSceneValidation, OpeningBaseline, Report, CompactKosPolish, GoldenSceneValidation, Record, Check, KosSceneValidation (+3 more)

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (24): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+16 more)

### Community 10 - "Community 10"
Cohesion: 0.05
Nodes (7): Phase1Content, Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase1, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 11 - "Community 11"
Cohesion: 0.04
Nodes (9): DesignerRevisionTests, GoldenSceneTests, KosVisualV2Tests, Count, Phase3SceneTests, Phase5SceneTests, PlayerFeelTests, Phase5HumanMotion (+1 more)

### Community 12 - "Community 12"
Cohesion: 0.08
Nodes (5): Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 13 - "Community 13"
Cohesion: 0.06
Nodes (27): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+19 more)

### Community 14 - "Community 14"
Cohesion: 0.05
Nodes (18): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, AtmajaSceneTests, PlaytestReviewTests (+10 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (20): Phase4Content, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+12 more)

### Community 16 - "Community 16"
Cohesion: 0.07
Nodes (7): DreadoutVisualRevision, Report, SceneReport, Phase3NavInspection, PlaytestRepairBuilder, Phase1LightBudget, Phase4FearPresentation

### Community 17 - "Community 17"
Cohesion: 0.06
Nodes (16): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry (+8 more)

### Community 18 - "Community 18"
Cohesion: 0.10
Nodes (8): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1SceneBuilder, Phase1Anchor, Phase1World, MotorcycleRiderAlignment

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (15): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+7 more)

### Community 20 - "Community 20"
Cohesion: 0.06
Nodes (31): Phase3Ids, Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign (+23 more)

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (12): DesignerVrmBuilder, PlaytestReviewValidation, Record, PrologPresentationBuilder, Receipt, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics (+4 more)

### Community 22 - "Community 22"
Cohesion: 0.05
Nodes (15): box(), cylinder(), finish(), tube(), export(), meshes(), sample_surface(), solve() (+7 more)

### Community 23 - "Community 23"
Cohesion: 0.05
Nodes (26): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+18 more)

### Community 24 - "Community 24"
Cohesion: 0.09
Nodes (6): KosVisualUpgrade, VisualAudit, MelatiStreetRevision, Report, TempShaftProbe, KosRoomCorrections

### Community 25 - "Community 25"
Cohesion: 0.11
Nodes (9): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey (+1 more)

### Community 26 - "Community 26"
Cohesion: 0.05
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 27 - "Community 27"
Cohesion: 0.08
Nodes (29): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+21 more)

### Community 28 - "Community 28"
Cohesion: 0.12
Nodes (4): Phase4Data, Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (9): FoundationAuditTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+1 more)

### Community 30 - "Community 30"
Cohesion: 0.09
Nodes (16): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+8 more)

### Community 32 - "Community 32"
Cohesion: 0.11
Nodes (7): ImportedManifest, ImportedMat, Phase3SceneBuilder, Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 33 - "Community 33"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 34 - "Community 34"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 35 - "Community 35"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 36 - "Community 36"
Cohesion: 0.17
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 37 - "Community 37"
Cohesion: 0.11
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 38 - "Community 38"
Cohesion: 0.07
Nodes (17): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+9 more)

### Community 39 - "Community 39"
Cohesion: 0.09
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 40 - "Community 40"
Cohesion: 0.07
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 42 - "Community 42"
Cohesion: 0.12
Nodes (4): BuildRecord, Phase3SceneBuilder, Usage, Phase3SceneBuilder

### Community 43 - "Community 43"
Cohesion: 0.11
Nodes (10): Receipt, ReviewPlayerAudit, Hud, Phone, View, DoubleLeafGate, IsOpen, KosCorridorDirector (+2 more)

### Community 44 - "Community 44"
Cohesion: 0.09
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 45 - "Community 45"
Cohesion: 0.07
Nodes (25): AtmajaPlayerAudit, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock (+17 more)

### Community 46 - "Community 46"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 47 - "Community 47"
Cohesion: 0.06
Nodes (24): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+16 more)

### Community 48 - "Community 48"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 51 - "Community 51"
Cohesion: 0.10
Nodes (7): Frame, LocationLightingAudit, Receipt, AbandonedLocationLightingTests, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 52 - "Community 52"
Cohesion: 0.07
Nodes (7): dialogue_literals(), export(), opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 53 - "Community 53"
Cohesion: 0.11
Nodes (7): Phase4Banaspati, B, LungeProgress, Pursuing, State, W, Phase4World

### Community 54 - "Community 54"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 55 - "Community 55"
Cohesion: 0.25
Nodes (4): Phase2SceneBuilder, ImportedMaterial, SceneRecord, Phase2Anchor

### Community 56 - "Community 56"
Cohesion: 0.12
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 58 - "Community 58"
Cohesion: 0.18
Nodes (6): Phase3PlayerAudit, Result, ScanVisualMode, AR, Normal, UV

### Community 59 - "Community 59"
Cohesion: 0.17
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 60 - "Community 60"
Cohesion: 0.10
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 61 - "Community 61"
Cohesion: 0.07
Nodes (3): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (7): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 63 - "Community 63"
Cohesion: 0.08
Nodes (22): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+14 more)

### Community 66 - "Community 66"
Cohesion: 0.15
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 67 - "Community 67"
Cohesion: 0.11
Nodes (5): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource

### Community 68 - "Community 68"
Cohesion: 0.16
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 70 - "Community 70"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 71 - "Community 71"
Cohesion: 0.13
Nodes (7): fetch(), main(), main(), prepare(), foley(), render(), write_pcm()

### Community 72 - "Community 72"
Cohesion: 0.13
Nodes (10): Phase3Content, Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 73 - "Community 73"
Cohesion: 0.12
Nodes (9): Phase5Director, FinalHint, FinalReadout, NearEdge, NearPortal, EvpProgress, SelectedSource, ThermalReadout (+1 more)

### Community 75 - "Community 75"
Cohesion: 0.13
Nodes (9): Phase3Wewe, B, Head, Pursuing, State, W, Phase3Anchor, Phase3World (+1 more)

### Community 77 - "Community 77"
Cohesion: 0.15
Nodes (9): Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 78 - "Community 78"
Cohesion: 0.14
Nodes (8): Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape, Voice

### Community 79 - "Community 79"
Cohesion: 0.21
Nodes (5): Phase3Director, AlcoveClosed, InAlcove, Phase3WorldItem, InteractableId

### Community 80 - "Community 80"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 81 - "Community 81"
Cohesion: 0.18
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 82 - "Community 82"
Cohesion: 0.10
Nodes (10): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph (+2 more)

### Community 84 - "Community 84"
Cohesion: 0.20
Nodes (3): Phase1Director, Phase1WorldItem, InteractableId

### Community 85 - "Community 85"
Cohesion: 0.16
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 86 - "Community 86"
Cohesion: 0.13
Nodes (8): DevelopmentScreenshotCapture, FoundationAuditCoroutineHost, Phase2ThermalTarget, Phase3Gate, IsOpen, Phase3UvPresentation, Phase4Lighting, Phase5AssetSource

### Community 88 - "Community 88"
Cohesion: 0.14
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 89 - "Community 89"
Cohesion: 0.15
Nodes (6): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, MelatiShopkeeper, InteractableId

### Community 90 - "Community 90"
Cohesion: 0.14
Nodes (4): AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 91 - "Community 91"
Cohesion: 0.14
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 95 - "Community 95"
Cohesion: 0.13
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 96 - "Community 96"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 98 - "Community 98"
Cohesion: 0.17
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 99 - "Community 99"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 101 - "Community 101"
Cohesion: 0.21
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 102 - "Community 102"
Cohesion: 0.23
Nodes (3): VoiceLine, Phase4Sound, AtmajaSynth

### Community 107 - "Community 107"
Cohesion: 0.17
Nodes (5): IPhoneChapterContent, CameraLocked, CameraStatus, CanBeginLive, Viewers

### Community 108 - "Community 108"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 109 - "Community 109"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 110 - "Community 110"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 111 - "Community 111"
Cohesion: 0.23
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 113 - "Community 113"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 115 - "Community 115"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 116 - "Community 116"
Cohesion: 0.35
Nodes (7): digest(), get(), main(), objaverse(), ph(), main(), ph()

### Community 118 - "Community 118"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 120 - "Community 120"
Cohesion: 0.36
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 121 - "Community 121"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 122 - "Community 122"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 123 - "Community 123"
Cohesion: 0.28
Nodes (4): VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 124 - "Community 124"
Cohesion: 0.22
Nodes (8): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneRitualPower, RitualCameraPower

### Community 125 - "Community 125"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 127 - "Community 127"
Cohesion: 0.25
Nodes (6): PresentationProfile, Phase1Balance, Phase2Balance, Phase3Balance, Phase4Balance, Phase5Balance

### Community 128 - "Community 128"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 129 - "Community 129"
Cohesion: 0.29
Nodes (3): MelatiShop, InteractableId, IsOpen

### Community 132 - "Community 132"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 133 - "Community 133"
Cohesion: 0.33
Nodes (6): EndingId, BAD, NONE, NORMAL, SECRET, TRUE

### Community 135 - "Community 135"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 136 - "Community 136"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 137 - "Community 137"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **610 isolated node(s):** `CameraLocked`, `CameraStatus`, `CanBeginLive`, `Viewers`, `Arrival` (+605 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 998 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **39 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 40` to `Community 0`, `Community 2`, `Community 134`, `Community 7`, `Community 8`, `Community 9`, `Community 139`, `Community 140`, `Community 13`, `Community 14`, `Community 15`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 23`, `Community 24`, `Community 26`, `Community 29`, `Community 32`, `Community 34`, `Community 38`, `Community 39`, `Community 41`, `Community 43`, `Community 46`, `Community 48`, `Community 50`, `Community 51`, `Community 56`, `Community 57`, `Community 59`, `Community 62`, `Community 72`, `Community 77`, `Community 80`, `Community 85`, `Community 86`, `Community 90`, `Community 92`, `Community 94`, `Community 102`?**
  _High betweenness centrality (0.197) - this node is a cross-community bridge._
- **What connects `CameraLocked`, `CameraStatus`, `CanBeginLive` to the rest of the system?**
  _610 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05710162853019996 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 6` to `Community 5`, `Community 7`, `Community 9`, `Community 11`, `Community 12`, `Community 19`, `Community 20`, `Community 26`, `Community 28`, `Community 29`, `Community 30`, `Community 37`, `Community 39`, `Community 61`, `Community 66`, `Community 68`, `Community 72`, `Community 77`, `Community 85`, `Community 86`, `Community 88`, `Community 117`, `Community 124`?**
  _High betweenness centrality (0.063) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.049886621315192746 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 9` to `Community 0`, `Community 97`, `Community 102`, `Community 10`, `Community 11`, `Community 21`, `Community 86`, `Community 23`, `Community 24`, `Community 62`?**
  _High betweenness centrality (0.051) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.08906030855539972 - nodes in this community are weakly interconnected._
