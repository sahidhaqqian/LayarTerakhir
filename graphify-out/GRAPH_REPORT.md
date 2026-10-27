# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4800 nodes · 17761 edges · 157 communities (118 shown, 39 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1192 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `2786756e`
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
- Community 146
- Community 148
- Community 149
- Community 150
- Community 151
- Community 152
- Community 154
- Community 155
- Community 156

## God Nodes (most connected - your core abstractions)
1. `Phase4Director` - 121 edges
2. `Phase2Director` - 115 edges
3. `Phase3Director` - 113 edges
4. `LayarTerakhir.Core` - 110 edges
5. `CampaignState` - 109 edges
6. `AtmajaPhoneView` - 107 edges
7. `Phase5Director` - 103 edges
8. `AtmajaDirector` - 102 edges
9. `LayarTerakhir.Prolog` - 92 edges
10. `Phase1Director` - 88 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `delete_fill_tagged()` --indirect_call--> `mesh()`  [INFERRED]
  Tools/repair_nyonya_appearance.py → Tools/author_atmaja_character.py

## Import Cycles
- None detected.

## Communities (157 total, 39 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.03
Nodes (12): AtmajaSceneTests, DesignerRevisionTests, GoldenSceneTests, InterludeCampaignTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase2SceneTests (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.04
Nodes (40): box(), cylinder(), finish(), tube(), sample_surface(), solve(), evaluate(), jacobian() (+32 more)

### Community 2 - "Community 2"
Cohesion: 0.04
Nodes (8): Phase1Content, Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Audio, LayarTerakhir.Phase1, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 5 - "Community 5"
Cohesion: 0.06
Nodes (19): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+11 more)

### Community 6 - "Community 6"
Cohesion: 0.09
Nodes (4): Phase5Data, Phase5CampaignTests, Slot, Phase5Campaign

### Community 7 - "Community 7"
Cohesion: 0.10
Nodes (6): LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 8 - "Community 8"
Cohesion: 0.06
Nodes (19): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatManifest (+11 more)

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (16): GoldenLinuxBuild, Record, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1SceneBuilder, RoomDressingPlacement (+8 more)

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (28): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+20 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (6): asset(), fetch(), foley(), main(), snapshot(), main()

### Community 13 - "Community 13"
Cohesion: 0.07
Nodes (18): Phase4Content, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+10 more)

### Community 14 - "Community 14"
Cohesion: 0.04
Nodes (28): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+20 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (5): Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 16 - "Community 16"
Cohesion: 0.06
Nodes (17): VoiceLine, Phase3Sound, AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud (+9 more)

### Community 17 - "Community 17"
Cohesion: 0.07
Nodes (21): FoundationPlayAuditTests, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+13 more)

### Community 18 - "Community 18"
Cohesion: 0.06
Nodes (29): PhoneDocument, Phase1MoneyReceipt, Phase2Ids, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign (+21 more)

### Community 19 - "Community 19"
Cohesion: 0.11
Nodes (5): CompactKosPolish, GoldenSceneValidation, Record, KosVisualV2, FloorY

### Community 20 - "Community 20"
Cohesion: 0.05
Nodes (38): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5Director, Phase5EndingShot (+30 more)

### Community 21 - "Community 21"
Cohesion: 0.12
Nodes (11): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Torch, Authentic, PermanentlyOff (+3 more)

### Community 22 - "Community 22"
Cohesion: 0.09
Nodes (12): PresentationProfile, GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 23 - "Community 23"
Cohesion: 0.12
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 24 - "Community 24"
Cohesion: 0.06
Nodes (10): CampaignData, Phase3Ids, Phase5Ids, UiFocus, BlocksMovement, BlocksWorld, HardModal, Phase5ReviewFixture (+2 more)

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (7): DreadoutVisualRevision, Report, SceneReport, Phase3NavInspection, PlaytestRepairBuilder, Phase4FearPresentation, PracticalLightPresentation

### Community 26 - "Community 26"
Cohesion: 0.05
Nodes (24): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection, AtmajaPhoneView, Apps, Chapter, ChatFrozen (+16 more)

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (10): KosGroundWallRepair, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Anchor (+2 more)

### Community 28 - "Community 28"
Cohesion: 0.07
Nodes (14): CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+6 more)

### Community 29 - "Community 29"
Cohesion: 0.11
Nodes (6): KosVisualUpgrade, VisualAudit, PlaytestReviewValidation, Record, TempShaftProbe, KosRoomCorrections

### Community 30 - "Community 30"
Cohesion: 0.10
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 31 - "Community 31"
Cohesion: 0.07
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 32 - "Community 32"
Cohesion: 0.07
Nodes (17): export(), meshes(), fetch(), main(), main(), prepare(), foley(), render() (+9 more)

### Community 33 - "Community 33"
Cohesion: 0.09
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 34 - "Community 34"
Cohesion: 0.14
Nodes (8): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey

### Community 35 - "Community 35"
Cohesion: 0.10
Nodes (20): asset(), download(), download(), metadata(), download(), itch(), main(), objaverse() (+12 more)

### Community 36 - "Community 36"
Cohesion: 0.05
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 37 - "Community 37"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 38 - "Community 38"
Cohesion: 0.08
Nodes (18): dialogue_literals(), export(), digest(), get(), main(), objaverse(), ph(), main() (+10 more)

### Community 39 - "Community 39"
Cohesion: 0.09
Nodes (20): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+12 more)

### Community 40 - "Community 40"
Cohesion: 0.08
Nodes (7): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone, ExplorationCommentary, Zone

### Community 41 - "Community 41"
Cohesion: 0.08
Nodes (13): Phase1WorldItem, InteractableId, Phase5WorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId (+5 more)

### Community 42 - "Community 42"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 43 - "Community 43"
Cohesion: 0.08
Nodes (17): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+9 more)

### Community 46 - "Community 46"
Cohesion: 0.10
Nodes (7): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer, AtmajaWorldText

### Community 47 - "Community 47"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 48 - "Community 48"
Cohesion: 0.09
Nodes (4): KosGroundWallTests, KosCorridorDirector, Busy, CanDepart

### Community 49 - "Community 49"
Cohesion: 0.11
Nodes (5): AtmajaPlayerAudit, Receipt, PlayerFeelAudit, Receipt, Sample

### Community 52 - "Community 52"
Cohesion: 0.06
Nodes (30): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 53 - "Community 53"
Cohesion: 0.11
Nodes (21): download(), oga_file(), sha(), source(), checked(), export(), image_path(), material_record() (+13 more)

### Community 54 - "Community 54"
Cohesion: 0.14
Nodes (3): KamarKosPrologDirector, PrologSceneFlow, CanContinue

### Community 55 - "Community 55"
Cohesion: 0.24
Nodes (3): Phase2SceneBuilder, ImportedMaterial, Phase2Anchor

### Community 56 - "Community 56"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 57 - "Community 57"
Cohesion: 0.16
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 58 - "Community 58"
Cohesion: 0.10
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 59 - "Community 59"
Cohesion: 0.09
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 60 - "Community 60"
Cohesion: 0.08
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (6): PrologKosSceneBuilder, Phase4Lighting, PhoneGlyph, PhoneSkin, Rounded, StoryMapGraphic

### Community 64 - "Community 64"
Cohesion: 0.09
Nodes (4): Phase5Director, EvpProgress, SelectedSource, ThermalReadout

### Community 65 - "Community 65"
Cohesion: 0.09
Nodes (16): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+8 more)

### Community 66 - "Community 66"
Cohesion: 0.11
Nodes (5): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource

### Community 67 - "Community 67"
Cohesion: 0.08
Nodes (19): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+11 more)

### Community 68 - "Community 68"
Cohesion: 0.14
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 70 - "Community 70"
Cohesion: 0.10
Nodes (14): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+6 more)

### Community 71 - "Community 71"
Cohesion: 0.14
Nodes (6): SceneAudioProfile, VoiceBank, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 72 - "Community 72"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 73 - "Community 73"
Cohesion: 0.17
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 74 - "Community 74"
Cohesion: 0.13
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 75 - "Community 75"
Cohesion: 0.17
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 76 - "Community 76"
Cohesion: 0.15
Nodes (16): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+8 more)

### Community 77 - "Community 77"
Cohesion: 0.26
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 78 - "Community 78"
Cohesion: 0.14
Nodes (6): AmbientAudioLoop, LevelMultiplier, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 80 - "Community 80"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 82 - "Community 82"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 83 - "Community 83"
Cohesion: 0.11
Nodes (4): WeightOnlyLinear, Info, read(), optimize_model()

### Community 86 - "Community 86"
Cohesion: 0.11
Nodes (12): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+4 more)

### Community 87 - "Community 87"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 88 - "Community 88"
Cohesion: 0.19
Nodes (4): AnimatedRakaAvatar, CinematicCrouch, Installing, MotorcycleRiderAlignment

### Community 89 - "Community 89"
Cohesion: 0.14
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 90 - "Community 90"
Cohesion: 0.12
Nodes (7): Phase3Wewe, B, Head, Pursuing, State, W, ClockAmbience

### Community 91 - "Community 91"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 92 - "Community 92"
Cohesion: 0.14
Nodes (9): DevelopmentScreenshotCapture, FoundationAuditCoroutineHost, Phase2Drawer, Prompt, State, Phase2ThermalTarget, Phase2Room, Phase2World (+1 more)

### Community 93 - "Community 93"
Cohesion: 0.18
Nodes (8): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Soundscape, Voice

### Community 94 - "Community 94"
Cohesion: 0.14
Nodes (4): InterludeNpc, InteractableId, InterludeWorldItem, InteractableId

### Community 95 - "Community 95"
Cohesion: 0.19
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 96 - "Community 96"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 98 - "Community 98"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 99 - "Community 99"
Cohesion: 0.25
Nodes (13): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+5 more)

### Community 100 - "Community 100"
Cohesion: 0.18
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 101 - "Community 101"
Cohesion: 0.23
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 102 - "Community 102"
Cohesion: 0.17
Nodes (3): SceneRecord, Phase2Sound, Phase2Soundscape

### Community 104 - "Community 104"
Cohesion: 0.23
Nodes (6): Phase1Balance, Phase2Balance, Phase4Balance, Phase5Balance, Phase5Replicas, B

### Community 105 - "Community 105"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 107 - "Community 107"
Cohesion: 0.29
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 108 - "Community 108"
Cohesion: 0.28
Nodes (4): DesignerVrmBuilder, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 113 - "Community 113"
Cohesion: 0.21
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 115 - "Community 115"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 118 - "Community 118"
Cohesion: 0.17
Nodes (6): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers

### Community 120 - "Community 120"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 121 - "Community 121"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 122 - "Community 122"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 123 - "Community 123"
Cohesion: 0.21
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 124 - "Community 124"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 125 - "Community 125"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 126 - "Community 126"
Cohesion: 0.20
Nodes (5): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 127 - "Community 127"
Cohesion: 0.29
Nodes (3): AtmajaSceneValidation, OpeningBaseline, Report

### Community 128 - "Community 128"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 129 - "Community 129"
Cohesion: 0.25
Nodes (3): PrologMilestoneValidator, KosInteractableItem, InteractableId

### Community 130 - "Community 130"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 131 - "Community 131"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 132 - "Community 132"
Cohesion: 0.25
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 136 - "Community 136"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 138 - "Community 138"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 139 - "Community 139"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 143 - "Community 143"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 146 - "Community 146"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 148 - "Community 148"
Cohesion: 0.60
Nodes (3): Metric, NyonyaMotionRevisionBuilder, Report

### Community 149 - "Community 149"
Cohesion: 0.60
Nodes (3): AtmajaGeometryAudit, Entry, Inventory

### Community 150 - "Community 150"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 151 - "Community 151"
Cohesion: 0.40
Nodes (5): Phase2DrawerState, Closed, Gas, Spent, Telegraph

## Knowledge Gaps
- **645 isolated node(s):** `CasedOpen`, `Chained`, `Closed`, `DebrisBlocked`, `InteractableId` (+640 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1046 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **39 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 26` to `Community 128`, `Community 4`, `Community 5`, `Community 8`, `Community 10`, `Community 11`, `Community 13`, `Community 14`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 22`, `Community 27`, `Community 29`, `Community 30`, `Community 31`, `Community 33`, `Community 36`, `Community 40`, `Community 43`, `Community 44`, `Community 48`, `Community 49`, `Community 50`, `Community 52`, `Community 54`, `Community 60`, `Community 61`, `Community 73`, `Community 75`, `Community 78`, `Community 80`, `Community 84`, `Community 86`, `Community 87`, `Community 92`, `Community 95`, `Community 102`, `Community 107`, `Community 112`, `Community 118`?**
  _High betweenness centrality (0.224) - this node is a cross-community bridge._
- **What connects `CasedOpen`, `Chained`, `Closed` to the rest of the system?**
  _645 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.029867256637168143 - nodes in this community are weakly interconnected._
- **Why does `Phase4Director` connect `Community 36` to `Community 128`, `Community 2`, `Community 7`, `Community 8`, `Community 9`, `Community 10`, `Community 13`, `Community 17`, `Community 19`, `Community 21`, `Community 22`, `Community 25`, `Community 26`, `Community 28`, `Community 41`, `Community 43`, `Community 44`, `Community 46`, `Community 63`, `Community 66`, `Community 67`, `Community 69`, `Community 74`, `Community 78`, `Community 80`, `Community 90`, `Community 92`, `Community 93`, `Community 104`, `Community 107`, `Community 111`?**
  _High betweenness centrality (0.062) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.040606060606060604 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 115` to `Community 71`, `Community 7`, `Community 78`, `Community 14`, `Community 48`, `Community 17`, `Community 16`, `Community 21`, `Community 88`, `Community 89`, `Community 92`, `Community 29`?**
  _High betweenness centrality (0.052) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.04145189003436426 - nodes in this community are weakly interconnected._
