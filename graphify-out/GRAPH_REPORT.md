# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5050 nodes · 19178 edges · 154 communities (117 shown, 37 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1274 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `8a397abf`
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
- Community 81
- Community 82
- Community 83
- Community 84
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
- Community 128
- Community 129
- Community 130
- Community 131
- Community 133
- Community 134
- Community 135
- Community 136
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
- Community 149
- Community 150
- Community 151
- Community 152

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 156 edges
2. `LayarTerakhir.Core` - 156 edges
3. `Phase5Director` - 129 edges
4. `AtmajaDirector` - 123 edges
5. `Phase4Director` - 122 edges
6. `ShoulderCameraRig` - 117 edges
7. `Phase2Director` - 116 edges
8. `Phase3Director` - 114 edges
9. `AtmajaPhoneView` - 114 edges
10. `FirstPersonController` - 114 edges

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

## Communities (154 total, 37 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.03
Nodes (31): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+23 more)

### Community 1 - "Community 1"
Cohesion: 0.04
Nodes (50): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+42 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (9): Phase5Content, KosDialogue, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Core, LayarTerakhir.Phase5, LayarTerakhir.UI, LayarTerakhir.Smartphone (+1 more)

### Community 3 - "Community 3"
Cohesion: 0.03
Nodes (10): HidingAwareness, DesignerRevisionTests, HidingAwarenessTests, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase2SceneTests, Phase4SceneTests (+2 more)

### Community 4 - "Community 4"
Cohesion: 0.10
Nodes (5): KosGroundWallRepair, KosRoomSurvey, Phase3NavInspection, TempShaftProbe, LayarTerakhir.Editor

### Community 6 - "Community 6"
Cohesion: 0.08
Nodes (4): Phase5Ids, Phase5CampaignTests, Slot, Phase5Campaign

### Community 7 - "Community 7"
Cohesion: 0.05
Nodes (23): FoundationPlayAuditTests, PrologKosSceneBuilder, PrologMilestoneValidator, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding (+15 more)

### Community 8 - "Community 8"
Cohesion: 0.06
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 9 - "Community 9"
Cohesion: 0.07
Nodes (7): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder, DesignerWorldRevision, ClockAmbience

### Community 10 - "Community 10"
Cohesion: 0.07
Nodes (5): Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (18): NyonyaMotionTests, Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings (+10 more)

### Community 12 - "Community 12"
Cohesion: 0.08
Nodes (7): CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, KosVisualV2, FloorY, PlayerFeelTests

### Community 13 - "Community 13"
Cohesion: 0.07
Nodes (13): AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport (+5 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (6): LayarTerakhir.Review, LayarTerakhir.Interlude, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Player, LayarTerakhir.Prolog

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (4): foley(), main(), snapshot(), main()

### Community 16 - "Community 16"
Cohesion: 0.06
Nodes (9): CampaignData, CampaignRouting, Phase3Ids, Phase5ReviewFixture, Phase5SaveCrashProbe, ReviewDirectBoot, DeveloperReviewModeTests, ReviewDirectBootTests (+1 more)

### Community 17 - "Community 17"
Cohesion: 0.07
Nodes (7): DreadoutVisualRevision, Report, SceneReport, PlaytestRepairBuilder, InterludeExterior, Phase1LightBudget, AtmajaMirrorReflection

### Community 18 - "Community 18"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 19 - "Community 19"
Cohesion: 0.04
Nodes (20): ball(), box(), cloth(), cylinder(), mat(), tube(), box(), cyl() (+12 more)

### Community 20 - "Community 20"
Cohesion: 0.07
Nodes (7): KosVisualUpgrade, VisualAudit, Phase2NavigationProbe, PlaytestReviewValidation, Record, Phase4UvPresentation, KosRoomCorrections

### Community 21 - "Community 21"
Cohesion: 0.09
Nodes (11): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Torch, Authentic, PermanentlyOff (+3 more)

### Community 22 - "Community 22"
Cohesion: 0.04
Nodes (36): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, InterludeContent, InterludeHub (+28 more)

### Community 23 - "Community 23"
Cohesion: 0.10
Nodes (8): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor, Phase1World

### Community 24 - "Community 24"
Cohesion: 0.06
Nodes (17): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId, MelatiDeparture, InteractableId (+9 more)

### Community 25 - "Community 25"
Cohesion: 0.09
Nodes (13): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorTile, Collapsed (+5 more)

### Community 26 - "Community 26"
Cohesion: 0.07
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 27 - "Community 27"
Cohesion: 0.09
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 28 - "Community 28"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 29 - "Community 29"
Cohesion: 0.13
Nodes (8): Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor, Phase2Room, Phase2World

### Community 30 - "Community 30"
Cohesion: 0.07
Nodes (18): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+10 more)

### Community 31 - "Community 31"
Cohesion: 0.07
Nodes (18): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+10 more)

### Community 32 - "Community 32"
Cohesion: 0.09
Nodes (27): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+19 more)

### Community 33 - "Community 33"
Cohesion: 0.07
Nodes (21): PrologCompletionAudit, Campaign, Hud, Phone, Receipt, PlayerPerspective, FirstPerson, OverShoulder (+13 more)

### Community 34 - "Community 34"
Cohesion: 0.07
Nodes (28): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+20 more)

### Community 35 - "Community 35"
Cohesion: 0.09
Nodes (12): Phase5Balance, Phase5Boss, Active, B, Exposed, P, Phase, RearCold (+4 more)

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (13): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse, Phase1MoneyReceipt (+5 more)

### Community 37 - "Community 37"
Cohesion: 0.08
Nodes (17): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+9 more)

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (11): dialogue_literals(), export(), compact(), material_records(), opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests (+3 more)

### Community 39 - "Community 39"
Cohesion: 0.16
Nodes (4): Phase2PlayerAudit, NoSecret, Result, KosGroundWallTests

### Community 40 - "Community 40"
Cohesion: 0.05
Nodes (33): Phase3Content, Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign (+25 more)

### Community 41 - "Community 41"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 42 - "Community 42"
Cohesion: 0.11
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 43 - "Community 43"
Cohesion: 0.05
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 44 - "Community 44"
Cohesion: 0.13
Nodes (28): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+20 more)

### Community 45 - "Community 45"
Cohesion: 0.07
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 46 - "Community 46"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 47 - "Community 47"
Cohesion: 0.10
Nodes (15): Phase1Balance, Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti (+7 more)

### Community 48 - "Community 48"
Cohesion: 0.09
Nodes (17): AtmajaGeometryAudit, Entry, Inventory, InterludeStreetCapture, Report, View, BoundsReport, Diagnosis (+9 more)

### Community 49 - "Community 49"
Cohesion: 0.12
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 50 - "Community 50"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 52 - "Community 52"
Cohesion: 0.13
Nodes (4): Authoring, PrologCompletionBuilder, PracticalLightPresentation, AtmajaEvidenceAtmosphere

### Community 53 - "Community 53"
Cohesion: 0.14
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 54 - "Community 54"
Cohesion: 0.12
Nodes (5): KamarKosPrologDirector, KosInteractableItem, InteractableId, PrologSceneFlow, CanContinue

### Community 55 - "Community 55"
Cohesion: 0.11
Nodes (5): PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, RecoveryTestInputSettings

### Community 56 - "Community 56"
Cohesion: 0.13
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 57 - "Community 57"
Cohesion: 0.16
Nodes (4): CutVertex, PrologReviewRevision, Receipt, PrologReviewRevisionTests

### Community 59 - "Community 59"
Cohesion: 0.10
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 60 - "Community 60"
Cohesion: 0.09
Nodes (9): AmbientAudioLoop, LevelMultiplier, Bike, InterludeTraffic, Phase2Sound, Phase2Soundscape, Phase5HumanMotion, Phase5SpectralPresentation (+1 more)

### Community 61 - "Community 61"
Cohesion: 0.11
Nodes (6): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 64 - "Community 64"
Cohesion: 0.11
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 65 - "Community 65"
Cohesion: 0.10
Nodes (4): AtmajaPlayerAudit, Receipt, GraphicsReferenceAudit, Receipt

### Community 69 - "Community 69"
Cohesion: 0.12
Nodes (18): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+10 more)

### Community 70 - "Community 70"
Cohesion: 0.16
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 71 - "Community 71"
Cohesion: 0.09
Nodes (9): ball(), box(), cylinder(), mat(), tube(), export(), meshes(), jagged_hole() (+1 more)

### Community 72 - "Community 72"
Cohesion: 0.16
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 73 - "Community 73"
Cohesion: 0.11
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 74 - "Community 74"
Cohesion: 0.12
Nodes (10): GoldenAudioTap, GoldenPlayerAudit, Receipt, KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone (+2 more)

### Community 75 - "Community 75"
Cohesion: 0.08
Nodes (21): Phase2Balance, Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining (+13 more)

### Community 76 - "Community 76"
Cohesion: 0.13
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 77 - "Community 77"
Cohesion: 0.13
Nodes (7): fetch(), main(), main(), prepare(), foley(), render(), write_pcm()

### Community 78 - "Community 78"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 79 - "Community 79"
Cohesion: 0.14
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 81 - "Community 81"
Cohesion: 0.17
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 82 - "Community 82"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 86 - "Community 86"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 87 - "Community 87"
Cohesion: 0.12
Nodes (12): cloth(), ellipsoid(), finish(), mesh(), tube(), is_arm(), main(), delete_fill_tagged() (+4 more)

### Community 88 - "Community 88"
Cohesion: 0.23
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 89 - "Community 89"
Cohesion: 0.15
Nodes (9): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape (+1 more)

### Community 90 - "Community 90"
Cohesion: 0.13
Nodes (10): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+2 more)

### Community 91 - "Community 91"
Cohesion: 0.18
Nodes (4): Phase3Gate, IsOpen, Phase3WorldItem, InteractableId

### Community 92 - "Community 92"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 93 - "Community 93"
Cohesion: 0.13
Nodes (7): DoubleLeafGate, IsOpen, ExplorationCommentary, Zone, KosCorridorDirector, Busy, CanDepart

### Community 94 - "Community 94"
Cohesion: 0.15
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 95 - "Community 95"
Cohesion: 0.18
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 96 - "Community 96"
Cohesion: 0.14
Nodes (5): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests

### Community 97 - "Community 97"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 98 - "Community 98"
Cohesion: 0.21
Nodes (9): Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 99 - "Community 99"
Cohesion: 0.19
Nodes (8): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto

### Community 100 - "Community 100"
Cohesion: 0.17
Nodes (6): PresentationProfile, VoiceLine, VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 101 - "Community 101"
Cohesion: 0.21
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 102 - "Community 102"
Cohesion: 0.19
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 108 - "Community 108"
Cohesion: 0.18
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 110 - "Community 110"
Cohesion: 0.26
Nodes (4): FoundationAuditCoroutineHost, RecoveryPresentationProbe, RecoveryRuntimeScenario, RecoveryVisualEvidence

### Community 112 - "Community 112"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 114 - "Community 114"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 115 - "Community 115"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 116 - "Community 116"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 117 - "Community 117"
Cohesion: 0.23
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 119 - "Community 119"
Cohesion: 0.20
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 120 - "Community 120"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 121 - "Community 121"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 124 - "Community 124"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 125 - "Community 125"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 126 - "Community 126"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 128 - "Community 128"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 129 - "Community 129"
Cohesion: 0.42
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 130 - "Community 130"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 134 - "Community 134"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 139 - "Community 139"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 140 - "Community 140"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 142 - "Community 142"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 143 - "Community 143"
Cohesion: 0.33
Nodes (5): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid

### Community 144 - "Community 144"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 145 - "Community 145"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 146 - "Community 146"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 151 - "Community 151"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **674 isolated node(s):** `ARApparitionActive`, `Hud`, `InstallationCompleted`, `Phone`, `Busy` (+669 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1051 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **37 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 45` to `Community 0`, `Community 2`, `Community 132`, `Community 5`, `Community 7`, `Community 8`, `Community 9`, `Community 11`, `Community 12`, `Community 17`, `Community 18`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 151`, `Community 25`, `Community 24`, `Community 27`, `Community 29`, `Community 33`, `Community 34`, `Community 36`, `Community 40`, `Community 42`, `Community 43`, `Community 47`, `Community 52`, `Community 53`, `Community 54`, `Community 55`, `Community 56`, `Community 60`, `Community 66`, `Community 71`, `Community 72`, `Community 73`, `Community 76`, `Community 78`, `Community 79`, `Community 82`, `Community 83`, `Community 93`, `Community 95`, `Community 98`, `Community 126`?**
  _High betweenness centrality (0.221) - this node is a cross-community bridge._
- **What connects `ARApparitionActive`, `Hud`, `InstallationCompleted` to the rest of the system?**
  _674 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.034886681945505475 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 26` to `Community 0`, `Community 3`, `Community 4`, `Community 6`, `Community 7`, `Community 8`, `Community 10`, `Community 16`, `Community 22`, `Community 28`, `Community 30`, `Community 33`, `Community 34`, `Community 36`, `Community 37`, `Community 40`, `Community 42`, `Community 43`, `Community 55`, `Community 56`, `Community 60`, `Community 70`, `Community 76`, `Community 95`, `Community 98`, `Community 110`?**
  _High betweenness centrality (0.078) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.04359413668379973 - nodes in this community are weakly interconnected._
- **Why does `Phase2Boss` connect `Community 49` to `Community 128`, `Community 2`, `Community 34`, `Community 36`, `Community 35`, `Community 75`, `Community 12`, `Community 52`, `Community 85`, `Community 20`, `Community 21`, `Community 153`, `Community 60`, `Community 29`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.0755193166375461 - nodes in this community are weakly interconnected._
