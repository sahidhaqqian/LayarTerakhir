# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4453 nodes · 16265 edges · 141 communities (109 shown, 32 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1161 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `550ed383`
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

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 126 edges
2. `LayarTerakhir.Core` - 123 edges
3. `Phase4Director` - 120 edges
4. `Phase2Director` - 112 edges
5. `AtmajaPhoneView` - 107 edges
6. `AtmajaDirector` - 100 edges
7. `HudController` - 86 edges
8. `Phase3Director` - 85 edges
9. `LayarTerakhir.Prolog` - 84 edges
10. `ShoulderCameraRig` - 80 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `Phase5PlayerAudit` --references--> `Phase5Director`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Diagnostics/Phase5PlayerAudit.cs → Assets/_LayarTerakhir/src/Phase5/Phase5Cinematics.cs

## Import Cycles
- None detected.

## Communities (141 total, 32 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (35): Phase5Director, Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign (+27 more)

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (40): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+32 more)

### Community 3 - "Community 3"
Cohesion: 0.10
Nodes (7): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 5 - "Community 5"
Cohesion: 0.07
Nodes (7): CampaignRouting, Phase2Ids, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (10): CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, KosVisualV2, FloorY, Phase4ForestHazard, Phase5Anchor (+2 more)

### Community 7 - "Community 7"
Cohesion: 0.05
Nodes (6): DesignerRevisionTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase4SceneTests, Phase5SceneTests

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (7): BuildRecord, Phase3SceneBuilder, Usage, Phase3SceneBuilder, Phase5SceneBuilder, Usage, Phase5MechanicalPresentation

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (6): Phase1Content, Phase5Content, LayarTerakhir.Audio, LayarTerakhir.Phase1, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 10 - "Community 10"
Cohesion: 0.07
Nodes (5): main(), prepare(), main(), snapshot(), main()

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (18): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+10 more)

### Community 12 - "Community 12"
Cohesion: 0.07
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 13 - "Community 13"
Cohesion: 0.11
Nodes (8): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4World, Phase4Zone

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (10): AtmajaStairRevisionBuilder, BandVertex, BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, ImportedManifest, ImportedMat, Phase3SceneBuilder (+2 more)

### Community 15 - "Community 15"
Cohesion: 0.05
Nodes (9): SceneAudioProfile, PresentationProfile, VoiceBank, GoldenSceneProduction, SceneManifest, Report, PlaytestRevisionBuilder, VoiceEntry (+1 more)

### Community 16 - "Community 16"
Cohesion: 0.10
Nodes (9): GoldenLinuxBuild, Record, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1SceneBuilder, Phase1Anchor (+1 more)

### Community 17 - "Community 17"
Cohesion: 0.10
Nodes (15): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+7 more)

### Community 18 - "Community 18"
Cohesion: 0.07
Nodes (9): FoundationAuditTests, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+1 more)

### Community 19 - "Community 19"
Cohesion: 0.08
Nodes (9): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, KamarKosPrologDirector, PrologSceneFlow (+1 more)

### Community 20 - "Community 20"
Cohesion: 0.10
Nodes (9): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5Ids, Phase5CampaignTests (+1 more)

### Community 21 - "Community 21"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (9): FoundationPlayAuditTests, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 23 - "Community 23"
Cohesion: 0.05
Nodes (12): export(), meshes(), box(), cylinder(), finish(), tube(), export(), meshes() (+4 more)

### Community 24 - "Community 24"
Cohesion: 0.06
Nodes (22): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+14 more)

### Community 25 - "Community 25"
Cohesion: 0.13
Nodes (10): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+2 more)

### Community 26 - "Community 26"
Cohesion: 0.06
Nodes (15): Phase2Data, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation (+7 more)

### Community 27 - "Community 27"
Cohesion: 0.08
Nodes (16): Phase1Balance, Phase2Balance, Phase3Balance, Phase4Balance, Phase5Balance, Phase5Boss, Active, B (+8 more)

### Community 28 - "Community 28"
Cohesion: 0.06
Nodes (27): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+19 more)

### Community 29 - "Community 29"
Cohesion: 0.16
Nodes (7): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2Anchor, Phase2Room, Phase2World

### Community 30 - "Community 30"
Cohesion: 0.05
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 31 - "Community 31"
Cohesion: 0.11
Nodes (4): Phase4Content, MotorcycleRiderAlignment, PhoneSkin, Rounded

### Community 32 - "Community 32"
Cohesion: 0.13
Nodes (8): VoiceLine, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 33 - "Community 33"
Cohesion: 0.07
Nodes (18): GraphicsReferenceAudit, Receipt, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar (+10 more)

### Community 36 - "Community 36"
Cohesion: 0.07
Nodes (14): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, MelatiDeparture, InteractableId, MelatiNeighborhood, Busy (+6 more)

### Community 37 - "Community 37"
Cohesion: 0.07
Nodes (11): sample_surface(), solve(), evaluate(), jacobian(), residual(), adapt(), WeightOnlyLinear, Info (+3 more)

### Community 38 - "Community 38"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 39 - "Community 39"
Cohesion: 0.09
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 40 - "Community 40"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 41 - "Community 41"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 42 - "Community 42"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 44 - "Community 44"
Cohesion: 0.13
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 45 - "Community 45"
Cohesion: 0.08
Nodes (17): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+9 more)

### Community 46 - "Community 46"
Cohesion: 0.07
Nodes (13): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatManifest (+5 more)

### Community 47 - "Community 47"
Cohesion: 0.18
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 48 - "Community 48"
Cohesion: 0.06
Nodes (30): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 49 - "Community 49"
Cohesion: 0.08
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 50 - "Community 50"
Cohesion: 0.11
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 51 - "Community 51"
Cohesion: 0.14
Nodes (3): PlaytestRepairBuilder, Phase1LightBudget, Phase4FearPresentation

### Community 52 - "Community 52"
Cohesion: 0.10
Nodes (19): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+11 more)

### Community 53 - "Community 53"
Cohesion: 0.15
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 54 - "Community 54"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 56 - "Community 56"
Cohesion: 0.15
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 57 - "Community 57"
Cohesion: 0.08
Nodes (19): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+11 more)

### Community 58 - "Community 58"
Cohesion: 0.10
Nodes (8): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape, Phase4Sound, Phase4Soundscape, Phase5Sound, Phase5Soundscape

### Community 60 - "Community 60"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (22): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+14 more)

### Community 64 - "Community 64"
Cohesion: 0.11
Nodes (8): MelatiStreetRevision, Report, Phase2NavigationProbe, Phase3NavInspection, Phase4MaterialProbe, Report, Row, Phase4AssetSource

### Community 65 - "Community 65"
Cohesion: 0.10
Nodes (17): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+9 more)

### Community 66 - "Community 66"
Cohesion: 0.11
Nodes (5): PlayerFeelAudit, Receipt, Sample, DoubleLeafGate, IsOpen

### Community 68 - "Community 68"
Cohesion: 0.10
Nodes (12): ScanVisualMode, AR, Normal, UV, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+4 more)

### Community 69 - "Community 69"
Cohesion: 0.20
Nodes (4): KosVisualUpgrade, VisualAudit, KosInteractableItem, InteractableId

### Community 70 - "Community 70"
Cohesion: 0.15
Nodes (16): asset(), download(), load_meta(), get(), main(), obj(), ph(), digest() (+8 more)

### Community 71 - "Community 71"
Cohesion: 0.17
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 74 - "Community 74"
Cohesion: 0.12
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 75 - "Community 75"
Cohesion: 0.17
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 77 - "Community 77"
Cohesion: 0.09
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 78 - "Community 78"
Cohesion: 0.12
Nodes (11): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+3 more)

### Community 79 - "Community 79"
Cohesion: 0.08
Nodes (22): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+14 more)

### Community 80 - "Community 80"
Cohesion: 0.14
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 81 - "Community 81"
Cohesion: 0.13
Nodes (10): Phase3Content, Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 82 - "Community 82"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 85 - "Community 85"
Cohesion: 0.21
Nodes (5): Phase3Director, AlcoveClosed, InAlcove, Phase3WorldItem, InteractableId

### Community 87 - "Community 87"
Cohesion: 0.20
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 88 - "Community 88"
Cohesion: 0.18
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 90 - "Community 90"
Cohesion: 0.20
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 91 - "Community 91"
Cohesion: 0.11
Nodes (13): Phase1Ids, Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak (+5 more)

### Community 92 - "Community 92"
Cohesion: 0.16
Nodes (9): InteractionInteractor, ContextResolver, CurrentTarget, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud (+1 more)

### Community 94 - "Community 94"
Cohesion: 0.16
Nodes (8): Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape, Voice

### Community 96 - "Community 96"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 97 - "Community 97"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 98 - "Community 98"
Cohesion: 0.17
Nodes (5): Phase4Lighting, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 100 - "Community 100"
Cohesion: 0.18
Nodes (3): foley(), render(), write_pcm()

### Community 101 - "Community 101"
Cohesion: 0.19
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 102 - "Community 102"
Cohesion: 0.13
Nodes (7): FoundationAuditCoroutineHost, Phase2ThermalTarget, Phase3UvPresentation, Phase4UvPresentation, Phase5AssetSource, Phase5SpectralPresentation, ClockAmbience

### Community 103 - "Community 103"
Cohesion: 0.14
Nodes (10): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph (+2 more)

### Community 104 - "Community 104"
Cohesion: 0.16
Nodes (10): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed, Phase3Anchor (+2 more)

### Community 105 - "Community 105"
Cohesion: 0.19
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 108 - "Community 108"
Cohesion: 0.21
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 112 - "Community 112"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 113 - "Community 113"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 116 - "Community 116"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 117 - "Community 117"
Cohesion: 0.29
Nodes (3): opening_structure_sha256(), verify_opening_source(), OpeningPresentationIdentityTests

### Community 118 - "Community 118"
Cohesion: 0.22
Nodes (4): AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 119 - "Community 119"
Cohesion: 0.36
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 120 - "Community 120"
Cohesion: 0.24
Nodes (5): KosCorridorDirector, Busy, CanDepart, KosTravelItem, InteractableId

### Community 121 - "Community 121"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 122 - "Community 122"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 123 - "Community 123"
Cohesion: 0.22
Nodes (3): Phase3Ids, Phase3Flare, Landed

### Community 125 - "Community 125"
Cohesion: 0.22
Nodes (8): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneRitualPower, RitualCameraPower

### Community 126 - "Community 126"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 127 - "Community 127"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 128 - "Community 128"
Cohesion: 0.32
Nodes (3): PlaytestReviewValidation, Record, PlaytestReviewTests

### Community 132 - "Community 132"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 133 - "Community 133"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 134 - "Community 134"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 136 - "Community 136"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 137 - "Community 137"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 138 - "Community 138"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **608 isolated node(s):** `ActionBusy`, `Analyzer`, `CameraCanScan`, `CameraLocked`, `CameraStatus` (+603 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 993 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **32 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 49` to `Community 1`, `Community 3`, `Community 6`, `Community 8`, `Community 12`, `Community 13`, `Community 14`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 22`, `Community 24`, `Community 26`, `Community 27`, `Community 28`, `Community 29`, `Community 30`, `Community 31`, `Community 33`, `Community 38`, `Community 39`, `Community 40`, `Community 44`, `Community 48`, `Community 52`, `Community 53`, `Community 55`, `Community 57`, `Community 58`, `Community 64`, `Community 65`, `Community 66`, `Community 68`, `Community 75`, `Community 81`, `Community 87`, `Community 88`, `Community 98`, `Community 102`, `Community 107`, `Community 120`?**
  _High betweenness centrality (0.196) - this node is a cross-community bridge._
- **What connects `ActionBusy`, `Analyzer`, `CameraCanScan` to the rest of the system?**
  _608 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.04908835904628331 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 25` to `Community 4`, `Community 5`, `Community 7`, `Community 11`, `Community 12`, `Community 17`, `Community 18`, `Community 20`, `Community 21`, `Community 22`, `Community 30`, `Community 42`, `Community 48`, `Community 52`, `Community 57`, `Community 61`, `Community 65`, `Community 81`, `Community 92`, `Community 102`, `Community 114`, `Community 115`, `Community 125`?**
  _High betweenness centrality (0.071) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.09077878643096035 - nodes in this community are weakly interconnected._
- **Why does `Phase2Director` connect `Community 28` to `Community 0`, `Community 5`, `Community 6`, `Community 8`, `Community 14`, `Community 19`, `Community 26`, `Community 27`, `Community 29`, `Community 31`, `Community 34`, `Community 40`, `Community 47`, `Community 48`, `Community 49`, `Community 55`, `Community 57`, `Community 58`, `Community 64`, `Community 71`, `Community 74`, `Community 81`, `Community 83`, `Community 84`, `Community 95`, `Community 102`, `Community 103`, `Community 106`, `Community 114`?**
  _High betweenness centrality (0.059) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.052795031055900624 - nodes in this community are weakly interconnected._
