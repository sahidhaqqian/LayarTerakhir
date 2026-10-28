# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5005 nodes · 18784 edges · 178 communities (129 shown, 49 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1245 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `a571f81b`
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
- Community 151
- Community 152
- Community 153
- Community 154
- Community 155
- Community 156
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

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 152 edges
2. `LayarTerakhir.Core` - 148 edges
3. `Phase5Director` - 129 edges
4. `Phase4Director` - 122 edges
5. `AtmajaDirector` - 122 edges
6. `Phase2Director` - 116 edges
7. `Phase3Director` - 114 edges
8. `AtmajaPhoneView` - 112 edges
9. `ShoulderCameraRig` - 112 edges
10. `FirstPersonController` - 110 edges

## Surprising Connections (you probably didn't know these)
- `delete_fill_tagged()` --indirect_call--> `mesh()`  [INFERRED]
  Tools/repair_nyonya_appearance.py → Tools/author_atmaja_character.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py

## Import Cycles
- None detected.

## Communities (178 total, 49 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.09
Nodes (7): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (43): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+35 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (5): Phase5Data, Phase5Ids, Phase5CampaignTests, Slot, Phase5Campaign

### Community 4 - "Community 4"
Cohesion: 0.04
Nodes (7): Phase5Content, LayarTerakhir.Review, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 5 - "Community 5"
Cohesion: 0.06
Nodes (19): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+11 more)

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (20): Bindings, MatManifest, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatManifest (+12 more)

### Community 8 - "Community 8"
Cohesion: 0.05
Nodes (19): CampaignData, CampaignRouting, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data (+11 more)

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (4): CompactKosPolish, GoldenSceneProduction, KosVisualV2, FloorY

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (4): foley(), main(), snapshot(), main()

### Community 11 - "Community 11"
Cohesion: 0.07
Nodes (18): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorKind, FragileDrop (+10 more)

### Community 12 - "Community 12"
Cohesion: 0.05
Nodes (11): AtmajaPlayerAudit, Receipt, PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, KosGroundWallTests, RecoveryTestInputSettings (+3 more)

### Community 13 - "Community 13"
Cohesion: 0.08
Nodes (6): DressingReceipt, Phase1SceneBuilder, PropPlacement, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder

### Community 14 - "Community 14"
Cohesion: 0.05
Nodes (34): cloth(), ellipsoid(), finish(), mesh(), tube(), actions(), add_armature(), at() (+26 more)

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 16 - "Community 16"
Cohesion: 0.08
Nodes (4): Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign

### Community 17 - "Community 17"
Cohesion: 0.09
Nodes (11): GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+3 more)

### Community 18 - "Community 18"
Cohesion: 0.05
Nodes (11): box(), cylinder(), finish(), tube(), export(), meshes(), sample_surface(), solve() (+3 more)

### Community 19 - "Community 19"
Cohesion: 0.06
Nodes (6): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Count, Phase3SceneTests, Phase5SceneTests

### Community 20 - "Community 20"
Cohesion: 0.07
Nodes (19): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+11 more)

### Community 21 - "Community 21"
Cohesion: 0.11
Nodes (4): Phase4Data, Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 22 - "Community 22"
Cohesion: 0.09
Nodes (12): FoundationAuditCoroutineHost, FoundationPlayAuditTests, RecoveryPresentationProbe, RecoveryRuntimeScenario, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+4 more)

### Community 23 - "Community 23"
Cohesion: 0.12
Nodes (8): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase4Anchor, Phase4World, Phase4Zone

### Community 24 - "Community 24"
Cohesion: 0.13
Nodes (10): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor (+2 more)

### Community 25 - "Community 25"
Cohesion: 0.09
Nodes (6): BloodWriting, MelatiInterludeAlleyBuilder, MelatiNeighborhoodBuilder, MelatiResidenceFactory, ARPhotoGlimmer, AtmajaWorldText

### Community 26 - "Community 26"
Cohesion: 0.10
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 27 - "Community 27"
Cohesion: 0.08
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 28 - "Community 28"
Cohesion: 0.09
Nodes (17): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+9 more)

### Community 29 - "Community 29"
Cohesion: 0.07
Nodes (5): KosGroundWallRepair, Phase2NavigationProbe, Phase4SceneTests, TempShaftProbe, Phase4AssetSource

### Community 30 - "Community 30"
Cohesion: 0.06
Nodes (28): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+20 more)

### Community 31 - "Community 31"
Cohesion: 0.08
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 32 - "Community 32"
Cohesion: 0.10
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 33 - "Community 33"
Cohesion: 0.06
Nodes (11): dialogue_literals(), export(), compact(), material_records(), opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests (+3 more)

### Community 34 - "Community 34"
Cohesion: 0.08
Nodes (25): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+17 more)

### Community 36 - "Community 36"
Cohesion: 0.05
Nodes (33): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+25 more)

### Community 37 - "Community 37"
Cohesion: 0.08
Nodes (3): InterludeExterior, DoubleLeafGate, IsOpen

### Community 38 - "Community 38"
Cohesion: 0.08
Nodes (5): PlayerFeelTests, AnimatedRakaAvatar, CinematicCrouch, Installing, MotorcycleRiderAlignment

### Community 39 - "Community 39"
Cohesion: 0.15
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 40 - "Community 40"
Cohesion: 0.13
Nodes (28): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+20 more)

### Community 41 - "Community 41"
Cohesion: 0.08
Nodes (11): Phase1WorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, MelatiShop, InteractableId (+3 more)

### Community 42 - "Community 42"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 43 - "Community 43"
Cohesion: 0.06
Nodes (31): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 44 - "Community 44"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 45 - "Community 45"
Cohesion: 0.07
Nodes (20): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+12 more)

### Community 46 - "Community 46"
Cohesion: 0.12
Nodes (5): KamarKosPrologDirector, KosInteractableItem, InteractableId, PrologSceneFlow, CanContinue

### Community 47 - "Community 47"
Cohesion: 0.09
Nodes (17): RecoveryVisualEvidence, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity (+9 more)

### Community 48 - "Community 48"
Cohesion: 0.07
Nodes (14): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+6 more)

### Community 49 - "Community 49"
Cohesion: 0.14
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 51 - "Community 51"
Cohesion: 0.12
Nodes (12): Phase1Anchor, Phase1World, Phase4Content, PhoneApp, Bank, Call, Camera, Home (+4 more)

### Community 52 - "Community 52"
Cohesion: 0.13
Nodes (22): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+14 more)

### Community 53 - "Community 53"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 54 - "Community 54"
Cohesion: 0.11
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 56 - "Community 56"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 57 - "Community 57"
Cohesion: 0.14
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 58 - "Community 58"
Cohesion: 0.09
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 59 - "Community 59"
Cohesion: 0.13
Nodes (8): DesignerVrmBuilder, Metric, NyonyaMotionRevisionBuilder, Report, NyonyaMotionTests, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 60 - "Community 60"
Cohesion: 0.10
Nodes (16): Phase2Balance, LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri (+8 more)

### Community 61 - "Community 61"
Cohesion: 0.07
Nodes (30): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 62 - "Community 62"
Cohesion: 0.07
Nodes (13): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+5 more)

### Community 63 - "Community 63"
Cohesion: 0.18
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 64 - "Community 64"
Cohesion: 0.12
Nodes (18): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+10 more)

### Community 67 - "Community 67"
Cohesion: 0.14
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 71 - "Community 71"
Cohesion: 0.10
Nodes (14): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+6 more)

### Community 72 - "Community 72"
Cohesion: 0.16
Nodes (6): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaItem, InteractableId

### Community 73 - "Community 73"
Cohesion: 0.14
Nodes (3): Phase5ReviewFixture, Phase5SaveCrashProbe, DeveloperReviewModeTests

### Community 74 - "Community 74"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 75 - "Community 75"
Cohesion: 0.23
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 77 - "Community 77"
Cohesion: 0.14
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 79 - "Community 79"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 80 - "Community 80"
Cohesion: 0.11
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewValidation, Record, PlaytestReviewTests

### Community 81 - "Community 81"
Cohesion: 0.13
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 82 - "Community 82"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 85 - "Community 85"
Cohesion: 0.09
Nodes (17): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+9 more)

### Community 86 - "Community 86"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 87 - "Community 87"
Cohesion: 0.15
Nodes (5): AmbientAudioLoop, LevelMultiplier, Phase4Sound, Phase4Soundscape, Phase5Soundscape

### Community 92 - "Community 92"
Cohesion: 0.12
Nodes (9): AtmajaGeometryAudit, Entry, Inventory, KosRoomSurvey, ImportedSceneReceipt, Phase3NavInspection, Phase4MaterialProbe, Report (+1 more)

### Community 93 - "Community 93"
Cohesion: 0.16
Nodes (9): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Sound, Phase3Soundscape (+1 more)

### Community 94 - "Community 94"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 95 - "Community 95"
Cohesion: 0.16
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 96 - "Community 96"
Cohesion: 0.20
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 97 - "Community 97"
Cohesion: 0.11
Nodes (14): Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State (+6 more)

### Community 98 - "Community 98"
Cohesion: 0.16
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 99 - "Community 99"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 100 - "Community 100"
Cohesion: 0.14
Nodes (8): SceneAudioProfile, PresentationProfile, VoiceLine, VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, Phase1Balance

### Community 101 - "Community 101"
Cohesion: 0.17
Nodes (4): AbandonedLocationLightingTests, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 102 - "Community 102"
Cohesion: 0.20
Nodes (5): Check, KosSceneValidation, Report, KosVisualV2Tests, Phase1LightBudget

### Community 103 - "Community 103"
Cohesion: 0.14
Nodes (7): KosTravelItem, InteractableId, MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 104 - "Community 104"
Cohesion: 0.19
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 105 - "Community 105"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 106 - "Community 106"
Cohesion: 0.16
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 107 - "Community 107"
Cohesion: 0.14
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 109 - "Community 109"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 111 - "Community 111"
Cohesion: 0.26
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 112 - "Community 112"
Cohesion: 0.17
Nodes (3): Phase5Sound, AtmajaSynth, ClockAmbience

### Community 113 - "Community 113"
Cohesion: 0.16
Nodes (3): DesignerRevisionTests, MelatiStreetRevision, Report

### Community 115 - "Community 115"
Cohesion: 0.21
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 116 - "Community 116"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 117 - "Community 117"
Cohesion: 0.20
Nodes (3): DevelopmentScreenshotCapture, Phase3UvPresentation, Phase4Lighting

### Community 118 - "Community 118"
Cohesion: 0.19
Nodes (5): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath

### Community 119 - "Community 119"
Cohesion: 0.19
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 125 - "Community 125"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 128 - "Community 128"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 129 - "Community 129"
Cohesion: 0.18
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 130 - "Community 130"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 131 - "Community 131"
Cohesion: 0.30
Nodes (3): Phase5Balance, Phase5Replicas, B

### Community 133 - "Community 133"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 134 - "Community 134"
Cohesion: 0.23
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 138 - "Community 138"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 139 - "Community 139"
Cohesion: 0.20
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 143 - "Community 143"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 144 - "Community 144"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 145 - "Community 145"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 146 - "Community 146"
Cohesion: 0.22
Nodes (3): Phase3Content, Phase3Flare, Landed

### Community 147 - "Community 147"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 148 - "Community 148"
Cohesion: 0.36
Nodes (5): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric

### Community 150 - "Community 150"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 151 - "Community 151"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 152 - "Community 152"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 154 - "Community 154"
Cohesion: 0.33
Nodes (5): ball(), box(), cylinder(), mat(), tube()

### Community 155 - "Community 155"
Cohesion: 0.25
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 159 - "Community 159"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 161 - "Community 161"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 163 - "Community 163"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 164 - "Community 164"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 165 - "Community 165"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 166 - "Community 166"
Cohesion: 0.47
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 170 - "Community 170"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 172 - "Community 172"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 175 - "Community 175"
Cohesion: 0.67
Nodes (3): InterludeStreetCapture, Report, View

### Community 176 - "Community 176"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **674 isolated node(s):** `NightActive`, `UnpoweredLightCount`, `InteractableId`, `Busy`, `Reached` (+669 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1060 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **49 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 45` to `Community 0`, `Community 5`, `Community 6`, `Community 7`, `Community 9`, `Community 11`, `Community 12`, `Community 13`, `Community 14`, `Community 17`, `Community 147`, `Community 20`, `Community 22`, `Community 23`, `Community 24`, `Community 25`, `Community 26`, `Community 28`, `Community 29`, `Community 30`, `Community 157`, `Community 34`, `Community 36`, `Community 37`, `Community 169`, `Community 43`, `Community 46`, `Community 47`, `Community 48`, `Community 49`, `Community 176`, `Community 51`, `Community 61`, `Community 62`, `Community 72`, `Community 77`, `Community 79`, `Community 87`, `Community 94`, `Community 95`, `Community 102`, `Community 103`, `Community 104`, `Community 112`, `Community 117`, `Community 119`?**
  _High betweenness centrality (0.200) - this node is a cross-community bridge._
- **What connects `NightActive`, `UnpoweredLightCount`, `InteractableId` to the rest of the system?**
  _674 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.09212121212121212 - nodes in this community are weakly interconnected._
- **Why does `Phase5Director` connect `Community 43` to `Community 0`, `Community 2`, `Community 131`, `Community 4`, `Community 132`, `Community 7`, `Community 9`, `Community 15`, `Community 17`, `Community 19`, `Community 147`, `Community 22`, `Community 23`, `Community 155`, `Community 45`, `Community 48`, `Community 50`, `Community 51`, `Community 53`, `Community 62`, `Community 67`, `Community 73`, `Community 76`, `Community 84`, `Community 87`, `Community 93`, `Community 95`, `Community 101`, `Community 102`, `Community 117`, `Community 120`?**
  _High betweenness centrality (0.054) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.04802631578947368 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 5` to `Community 0`, `Community 6`, `Community 9`, `Community 12`, `Community 15`, `Community 17`, `Community 20`, `Community 22`, `Community 23`, `Community 26`, `Community 29`, `Community 37`, `Community 39`, `Community 45`, `Community 48`, `Community 49`, `Community 51`, `Community 54`, `Community 59`, `Community 63`, `Community 69`, `Community 72`, `Community 74`, `Community 75`, `Community 77`, `Community 79`, `Community 80`, `Community 87`, `Community 100`, `Community 105`, `Community 110`, `Community 112`, `Community 113`, `Community 117`, `Community 119`, `Community 127`?**
  _High betweenness centrality (0.045) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.07807386629266012 - nodes in this community are weakly interconnected._
