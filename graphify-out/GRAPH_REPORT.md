# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4449 nodes · 16888 edges · 151 communities (115 shown, 36 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1169 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `a784cc2b`
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
- Community 52
- Community 53
- Community 54
- Community 55
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

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 138 edges
2. `LayarTerakhir.Core` - 135 edges
3. `Phase5Director` - 129 edges
4. `Phase4Director` - 122 edges
5. `AtmajaDirector` - 120 edges
6. `Phase2Director` - 116 edges
7. `Phase3Director` - 114 edges
8. `AtmajaPhoneView` - 111 edges
9. `ShoulderCameraRig` - 111 edges
10. `LayarTerakhir.Player` - 99 edges

## Surprising Connections (you probably didn't know these)
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase2RitualTransactionTests` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/Phase2RitualTransactionTests.cs → Assets/_LayarTerakhir/src/Core/CampaignState.cs

## Import Cycles
- None detected.

## Communities (151 total, 36 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (53): asset(), download(), load_meta(), asset(), download(), fetch(), main(), sha() (+45 more)

### Community 1 - "Community 1"
Cohesion: 0.08
Nodes (8): KosDialogue, LayarTerakhir.Phase3, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 4 - "Community 4"
Cohesion: 0.09
Nodes (3): Phase5CampaignTests, Slot, Phase5Campaign

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (31): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+23 more)

### Community 6 - "Community 6"
Cohesion: 0.07
Nodes (20): MotorcycleRiderAlignment, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+12 more)

### Community 7 - "Community 7"
Cohesion: 0.05
Nodes (8): DesignerRevisionTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Count, Phase3SceneTests, Phase4SceneTests, Phase5SceneTests

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (26): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+18 more)

### Community 9 - "Community 9"
Cohesion: 0.05
Nodes (18): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+10 more)

### Community 10 - "Community 10"
Cohesion: 0.09
Nodes (4): Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase2Campaign

### Community 11 - "Community 11"
Cohesion: 0.10
Nodes (17): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorKind, FragileDrop (+9 more)

### Community 12 - "Community 12"
Cohesion: 0.07
Nodes (4): foley(), main(), snapshot(), main()

### Community 13 - "Community 13"
Cohesion: 0.05
Nodes (24): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+16 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (10): PresentationProfile, FoundationAuditTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+2 more)

### Community 15 - "Community 15"
Cohesion: 0.11
Nodes (6): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1Anchor, Phase1World

### Community 16 - "Community 16"
Cohesion: 0.12
Nodes (3): CompactKosPolish, KosVisualV2, FloorY

### Community 17 - "Community 17"
Cohesion: 0.11
Nodes (4): Phase4Data, Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 18 - "Community 18"
Cohesion: 0.12
Nodes (10): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor (+2 more)

### Community 19 - "Community 19"
Cohesion: 0.06
Nodes (14): DevelopmentScreenshotCapture, GoldenAudioTap, GoldenPlayerAudit, Receipt, FirstPersonController, CrouchAmount, IsCrouching, IsSliding (+6 more)

### Community 20 - "Community 20"
Cohesion: 0.08
Nodes (5): Phase5AssetSource, Phase5Content, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 21 - "Community 21"
Cohesion: 0.10
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 22 - "Community 22"
Cohesion: 0.12
Nodes (12): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+4 more)

### Community 23 - "Community 23"
Cohesion: 0.06
Nodes (29): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+21 more)

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (15): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+7 more)

### Community 25 - "Community 25"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 26 - "Community 26"
Cohesion: 0.10
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 27 - "Community 27"
Cohesion: 0.09
Nodes (10): Receipt, ReviewPlayerAudit, Hud, Phone, View, DoubleLeafGate, IsOpen, KosCorridorDirector (+2 more)

### Community 28 - "Community 28"
Cohesion: 0.14
Nodes (9): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceBank, VoiceEntry (+1 more)

### Community 29 - "Community 29"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 30 - "Community 30"
Cohesion: 0.11
Nodes (4): Phase1Content, LayarTerakhir.Phase2, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 32 - "Community 32"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 33 - "Community 33"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 34 - "Community 34"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 35 - "Community 35"
Cohesion: 0.09
Nodes (9): AmbientAudioLoop, LevelMultiplier, VoiceLine, Phase2Sound, Phase2Soundscape, Phase4Sound, Phase4Soundscape, AtmajaSynth (+1 more)

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 37 - "Community 37"
Cohesion: 0.06
Nodes (31): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 38 - "Community 38"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 39 - "Community 39"
Cohesion: 0.09
Nodes (13): Bindings, MatRow, MeshBinding, Phase4SceneBuilder, SourceBinding, SceneRecord, Bindings, SourceBinding (+5 more)

### Community 40 - "Community 40"
Cohesion: 0.15
Nodes (10): MatManifest, Phase5SceneBuilder, SourceEntry, SourceList, MatManifest, MatRow, MeshBinding, Phase5Anchor (+2 more)

### Community 41 - "Community 41"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 42 - "Community 42"
Cohesion: 0.10
Nodes (10): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+2 more)

### Community 43 - "Community 43"
Cohesion: 0.06
Nodes (31): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 44 - "Community 44"
Cohesion: 0.07
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 45 - "Community 45"
Cohesion: 0.07
Nodes (25): AtmajaPlayerAudit, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock (+17 more)

### Community 46 - "Community 46"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 47 - "Community 47"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 48 - "Community 48"
Cohesion: 0.09
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (5): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, ARPhotoGlimmer, AtmajaWorldText

### Community 50 - "Community 50"
Cohesion: 0.10
Nodes (10): KosGroundWallRepair, KosRoomSurvey, MelatiStreetRevision, Report, Phase3NavInspection, Phase4MaterialProbe, Report, Row (+2 more)

### Community 52 - "Community 52"
Cohesion: 0.18
Nodes (6): Phase3PlayerAudit, Result, ScanVisualMode, AR, Normal, UV

### Community 53 - "Community 53"
Cohesion: 0.10
Nodes (16): Phase2Balance, LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri (+8 more)

### Community 54 - "Community 54"
Cohesion: 0.16
Nodes (8): FoundationPlayAuditTests, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 55 - "Community 55"
Cohesion: 0.19
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 57 - "Community 57"
Cohesion: 0.13
Nodes (10): DesignerReviewPlayerAudit, Hud, Phone, Receipt, InteractionInteractor, ContextResolver, CurrentTarget, AtmajaItem (+2 more)

### Community 59 - "Community 59"
Cohesion: 0.18
Nodes (4): Authoring, PrologCompletionBuilder, Phase1LightBudget, AtmajaEvidenceAtmosphere

### Community 61 - "Community 61"
Cohesion: 0.09
Nodes (18): Phase3Wewe, B, Head, Pursuing, State, W, Phase3WeweState, Ambush (+10 more)

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (18): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+10 more)

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (9): GoldenSceneValidation, Record, GoldenSceneTests, PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer (+1 more)

### Community 64 - "Community 64"
Cohesion: 0.14
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 65 - "Community 65"
Cohesion: 0.15
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 66 - "Community 66"
Cohesion: 0.16
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 67 - "Community 67"
Cohesion: 0.13
Nodes (3): CampaignRouting, Phase3CampaignTests, Phase3Campaign

### Community 68 - "Community 68"
Cohesion: 0.16
Nodes (3): AtmajaStairRevisionBuilder, BandVertex, PracticalLightPresentation

### Community 70 - "Community 70"
Cohesion: 0.09
Nodes (15): Phase1Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining (+7 more)

### Community 71 - "Community 71"
Cohesion: 0.12
Nodes (9): dialogue_literals(), export(), compact(), material_records(), opening_structure_sha256(), verify_opening_source(), OpeningPresentationIdentityTests, main() (+1 more)

### Community 72 - "Community 72"
Cohesion: 0.18
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 74 - "Community 74"
Cohesion: 0.19
Nodes (4): GoldenHudBuilder, HudStyle, PhoneGlyph, StoryMapGraphic

### Community 75 - "Community 75"
Cohesion: 0.09
Nodes (13): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+5 more)

### Community 78 - "Community 78"
Cohesion: 0.11
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewValidation, Record, PlaytestReviewTests

### Community 79 - "Community 79"
Cohesion: 0.17
Nodes (4): SceneAudioProfile, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry

### Community 80 - "Community 80"
Cohesion: 0.14
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 81 - "Community 81"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 83 - "Community 83"
Cohesion: 0.14
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 85 - "Community 85"
Cohesion: 0.15
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 86 - "Community 86"
Cohesion: 0.13
Nodes (5): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 87 - "Community 87"
Cohesion: 0.14
Nodes (5): AtmajaGeometryAudit, Entry, Inventory, NyonyaMotionTests, Phase2NavigationProbe

### Community 90 - "Community 90"
Cohesion: 0.14
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 91 - "Community 91"
Cohesion: 0.16
Nodes (10): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport, Metric (+2 more)

### Community 92 - "Community 92"
Cohesion: 0.19
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 93 - "Community 93"
Cohesion: 0.20
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 94 - "Community 94"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 95 - "Community 95"
Cohesion: 0.15
Nodes (5): Phase4Lighting, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 96 - "Community 96"
Cohesion: 0.13
Nodes (7): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached, PrologSceneFlow, CanContinue

### Community 97 - "Community 97"
Cohesion: 0.26
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 98 - "Community 98"
Cohesion: 0.19
Nodes (9): Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 99 - "Community 99"
Cohesion: 0.19
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 100 - "Community 100"
Cohesion: 0.17
Nodes (8): SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance, RitualCameraPower

### Community 101 - "Community 101"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 103 - "Community 103"
Cohesion: 0.18
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 104 - "Community 104"
Cohesion: 0.15
Nodes (9): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+1 more)

### Community 105 - "Community 105"
Cohesion: 0.18
Nodes (3): Phase4Content, Phase4WorldItem, InteractableId

### Community 106 - "Community 106"
Cohesion: 0.25
Nodes (13): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+5 more)

### Community 109 - "Community 109"
Cohesion: 0.17
Nodes (4): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId

### Community 110 - "Community 110"
Cohesion: 0.19
Nodes (5): Phase3Gate, IsOpen, Phase3Sound, Phase3Soundscape, Voice

### Community 113 - "Community 113"
Cohesion: 0.29
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 114 - "Community 114"
Cohesion: 0.14
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 115 - "Community 115"
Cohesion: 0.26
Nodes (3): Phase5Balance, Phase5Replicas, B

### Community 116 - "Community 116"
Cohesion: 0.21
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 118 - "Community 118"
Cohesion: 0.23
Nodes (6): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic

### Community 121 - "Community 121"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 125 - "Community 125"
Cohesion: 0.20
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 126 - "Community 126"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 128 - "Community 128"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 129 - "Community 129"
Cohesion: 0.20
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 130 - "Community 130"
Cohesion: 0.31
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 133 - "Community 133"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 134 - "Community 134"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 135 - "Community 135"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 136 - "Community 136"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 137 - "Community 137"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 138 - "Community 138"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 140 - "Community 140"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 141 - "Community 141"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 143 - "Community 143"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 146 - "Community 146"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 147 - "Community 147"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **614 isolated node(s):** `BlitzCooldownRemaining`, `CameraLocked`, `ChapterContent`, `DisplayBatteryPercent`, `HasCameraPower` (+609 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 959 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **36 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 44` to `Community 1`, `Community 130`, `Community 2`, `Community 6`, `Community 8`, `Community 10`, `Community 11`, `Community 13`, `Community 14`, `Community 15`, `Community 16`, `Community 144`, `Community 18`, `Community 19`, `Community 21`, `Community 22`, `Community 23`, `Community 27`, `Community 32`, `Community 33`, `Community 35`, `Community 36`, `Community 37`, `Community 39`, `Community 43`, `Community 46`, `Community 50`, `Community 52`, `Community 54`, `Community 57`, `Community 63`, `Community 66`, `Community 68`, `Community 72`, `Community 74`, `Community 75`, `Community 77`, `Community 80`, `Community 81`, `Community 85`, `Community 86`, `Community 88`, `Community 92`, `Community 95`, `Community 98`, `Community 99`, `Community 100`, `Community 107`, `Community 113`?**
  _High betweenness centrality (0.261) - this node is a cross-community bridge._
- **What connects `BlitzCooldownRemaining`, `CameraLocked`, `ChapterContent` to the rest of the system?**
  _614 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.044295684375445094 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 42` to `Community 3`, `Community 4`, `Community 131`, `Community 7`, `Community 9`, `Community 10`, `Community 14`, `Community 17`, `Community 19`, `Community 21`, `Community 22`, `Community 23`, `Community 26`, `Community 29`, `Community 33`, `Community 37`, `Community 43`, `Community 54`, `Community 65`, `Community 66`, `Community 67`, `Community 75`, `Community 85`, `Community 98`, `Community 99`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.08296703296703296 - nodes in this community are weakly interconnected._
- **Why does `Phase4Director` connect `Community 33` to `Community 1`, `Community 130`, `Community 6`, `Community 7`, `Community 11`, `Community 14`, `Community 15`, `Community 16`, `Community 17`, `Community 19`, `Community 20`, `Community 35`, `Community 36`, `Community 39`, `Community 42`, `Community 44`, `Community 49`, `Community 50`, `Community 56`, `Community 58`, `Community 60`, `Community 70`, `Community 73`, `Community 75`, `Community 81`, `Community 83`, `Community 85`, `Community 87`, `Community 100`, `Community 105`, `Community 109`, `Community 113`, `Community 118`, `Community 124`?**
  _High betweenness centrality (0.053) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.08118811881188119 - nodes in this community are weakly interconnected._
