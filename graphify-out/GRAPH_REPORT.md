# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 4511 nodes · 16992 edges · 142 communities (109 shown, 33 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1170 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `20f07bfc`
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
- Community 86
- Community 87
- Community 88
- Community 89
- Community 90
- Community 91
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
- Community 123
- Community 124
- Community 125
- Community 126
- Community 127
- Community 128
- Community 129
- Community 131
- Community 132
- Community 133
- Community 134
- Community 135
- Community 136
- Community 137
- Community 138
- Community 141

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 137 edges
2. `LayarTerakhir.Core` - 134 edges
3. `Phase5Director` - 129 edges
4. `AtmajaDirector` - 122 edges
5. `Phase4Director` - 122 edges
6. `Phase2Director` - 116 edges
7. `Phase3Director` - 114 edges
8. `AtmajaPhoneView` - 110 edges
9. `ShoulderCameraRig` - 110 edges
10. `LayarTerakhir.Player` - 97 edges

## Surprising Connections (you probably didn't know these)
- `delete_fill_tagged()` --indirect_call--> `mesh()`  [INFERRED]
  Tools/repair_nyonya_appearance.py → Tools/author_atmaja_character.py
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

## Communities (142 total, 33 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.04
Nodes (59): asset(), download(), load_meta(), asset(), download(), fetch(), main(), sha() (+51 more)

### Community 1 - "Community 1"
Cohesion: 0.09
Nodes (7): KosDialogue, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 4 - "Community 4"
Cohesion: 0.05
Nodes (16): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+8 more)

### Community 5 - "Community 5"
Cohesion: 0.04
Nodes (8): DesignerRevisionTests, KosVisualV2Tests, Phase2SceneTests, Phase4SceneTests, Phase5SceneTests, PlayerFeelTests, Phase4AssetSource, Phase5AssetSource

### Community 6 - "Community 6"
Cohesion: 0.09
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 7 - "Community 7"
Cohesion: 0.05
Nodes (6): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (6): Phase1MoneyReceipt, Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (6): CompactKosPolish, Check, KosSceneValidation, Report, KosVisualV2, FloorY

### Community 10 - "Community 10"
Cohesion: 0.06
Nodes (31): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+23 more)

### Community 11 - "Community 11"
Cohesion: 0.07
Nodes (12): Phase5SceneBuilder, SourceEntry, SourceList, MatRow, MeshBinding, Report, Usage, Phase5HumanMotion (+4 more)

### Community 12 - "Community 12"
Cohesion: 0.08
Nodes (11): AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport (+3 more)

### Community 13 - "Community 13"
Cohesion: 0.10
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 14 - "Community 14"
Cohesion: 0.08
Nodes (8): BloodWriting, DesignerWorldRevision, DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1Anchor, AtmajaWorldText

### Community 15 - "Community 15"
Cohesion: 0.07
Nodes (8): DreadoutVisualRevision, Report, SceneReport, Phase3NavInspection, PlaytestRepairBuilder, Phase1LightBudget, Phase4FearPresentation, PracticalLightPresentation

### Community 16 - "Community 16"
Cohesion: 0.07
Nodes (3): main(), snapshot(), main()

### Community 17 - "Community 17"
Cohesion: 0.09
Nodes (14): Bindings, MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, SourceBinding, SceneRecord, Usage (+6 more)

### Community 18 - "Community 18"
Cohesion: 0.07
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 19 - "Community 19"
Cohesion: 0.04
Nodes (14): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+6 more)

### Community 20 - "Community 20"
Cohesion: 0.09
Nodes (12): PresentationProfile, GoldenHudBuilder, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen (+4 more)

### Community 21 - "Community 21"
Cohesion: 0.05
Nodes (24): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+16 more)

### Community 22 - "Community 22"
Cohesion: 0.05
Nodes (25): Phase4Lighting, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection, AtmajaPhoneView, Apps, Chapter (+17 more)

### Community 23 - "Community 23"
Cohesion: 0.11
Nodes (4): Phase4Data, Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 24 - "Community 24"
Cohesion: 0.05
Nodes (23): GraphicsReferenceAudit, Receipt, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar (+15 more)

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (31): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+23 more)

### Community 26 - "Community 26"
Cohesion: 0.09
Nodes (8): KosGroundWallRepair, KosVisualUpgrade, VisualAudit, MelatiStreetRevision, Report, Phase2NavigationProbe, Phase5SpectralPresentation, KosRoomCorrections

### Community 27 - "Community 27"
Cohesion: 0.09
Nodes (10): FoundationAuditCoroutineHost, FoundationPlayAuditTests, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked (+2 more)

### Community 28 - "Community 28"
Cohesion: 0.11
Nodes (12): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3FloorTile, Collapsed (+4 more)

### Community 29 - "Community 29"
Cohesion: 0.13
Nodes (10): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, Phase2ThermalTarget, Phase2Anchor (+2 more)

### Community 30 - "Community 30"
Cohesion: 0.06
Nodes (29): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+21 more)

### Community 31 - "Community 31"
Cohesion: 0.11
Nodes (12): MotorcycleRiderAlignment, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+4 more)

### Community 32 - "Community 32"
Cohesion: 0.06
Nodes (23): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase3Content, Phase3Hub (+15 more)

### Community 33 - "Community 33"
Cohesion: 0.09
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 34 - "Community 34"
Cohesion: 0.12
Nodes (6): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 35 - "Community 35"
Cohesion: 0.09
Nodes (14): Phase2Balance, Phase4Balance, Phase5Balance, Phase5Boss, Active, B, Exposed, P (+6 more)

### Community 36 - "Community 36"
Cohesion: 0.07
Nodes (29): PhoneDocument, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+21 more)

### Community 37 - "Community 37"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 38 - "Community 38"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 39 - "Community 39"
Cohesion: 0.06
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 40 - "Community 40"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 41 - "Community 41"
Cohesion: 0.10
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 42 - "Community 42"
Cohesion: 0.08
Nodes (11): KosGroundWallTests, DoubleLeafGate, IsOpen, KosCorridorDirector, Busy, CanDepart, MelatiNeighborhood, Busy (+3 more)

### Community 43 - "Community 43"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 44 - "Community 44"
Cohesion: 0.11
Nodes (16): Phase1Balance, Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti (+8 more)

### Community 45 - "Community 45"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 47 - "Community 47"
Cohesion: 0.12
Nodes (3): Phase1Content, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1

### Community 48 - "Community 48"
Cohesion: 0.08
Nodes (12): ScanVisualMode, AR, Normal, UV, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+4 more)

### Community 49 - "Community 49"
Cohesion: 0.10
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 50 - "Community 50"
Cohesion: 0.11
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 51 - "Community 51"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 52 - "Community 52"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 53 - "Community 53"
Cohesion: 0.07
Nodes (4): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Ids

### Community 54 - "Community 54"
Cohesion: 0.16
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 55 - "Community 55"
Cohesion: 0.09
Nodes (9): Phase3UvPresentation, Phase4UvPresentation, MelatiDeparture, InteractableId, MelatiShop, InteractableId, IsOpen, MelatiShopkeeper (+1 more)

### Community 56 - "Community 56"
Cohesion: 0.10
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 57 - "Community 57"
Cohesion: 0.09
Nodes (14): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+6 more)

### Community 58 - "Community 58"
Cohesion: 0.11
Nodes (4): AtmajaPlayerAudit, Receipt, Receipt, RiderPresentationAudit

### Community 59 - "Community 59"
Cohesion: 0.14
Nodes (4): CampaignRouting, ReviewDirectBoot, ReviewDirectBootTests, Slot

### Community 61 - "Community 61"
Cohesion: 0.13
Nodes (8): Receipt, ReviewPlayerAudit, Hud, Phone, View, InteractionInteractor, ContextResolver, CurrentTarget

### Community 62 - "Community 62"
Cohesion: 0.11
Nodes (5): VoiceLine, Phase4Sound, Phase4Soundscape, AtmajaSynth, ClockAmbience

### Community 66 - "Community 66"
Cohesion: 0.13
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 67 - "Community 67"
Cohesion: 0.14
Nodes (6): SceneAudioProfile, VoiceBank, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 68 - "Community 68"
Cohesion: 0.09
Nodes (4): dialogue_literals(), export(), DialogueRecordingTests, GraphifyDiscoveryTests

### Community 69 - "Community 69"
Cohesion: 0.11
Nodes (8): AmbientAudioLoop, LevelMultiplier, KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 70 - "Community 70"
Cohesion: 0.13
Nodes (13): Phase4Content, Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+5 more)

### Community 71 - "Community 71"
Cohesion: 0.17
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 72 - "Community 72"
Cohesion: 0.08
Nodes (22): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+14 more)

### Community 73 - "Community 73"
Cohesion: 0.19
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 74 - "Community 74"
Cohesion: 0.18
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 75 - "Community 75"
Cohesion: 0.19
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 76 - "Community 76"
Cohesion: 0.13
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 77 - "Community 77"
Cohesion: 0.12
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 78 - "Community 78"
Cohesion: 0.13
Nodes (5): Phase3Gate, IsOpen, Phase3Sound, Phase3Soundscape, Voice

### Community 79 - "Community 79"
Cohesion: 0.12
Nodes (6): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId

### Community 81 - "Community 81"
Cohesion: 0.13
Nodes (12): Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic, Mimic, Phase3Wewe, B (+4 more)

### Community 82 - "Community 82"
Cohesion: 0.16
Nodes (9): Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning, State (+1 more)

### Community 84 - "Community 84"
Cohesion: 0.12
Nodes (10): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+2 more)

### Community 86 - "Community 86"
Cohesion: 0.16
Nodes (10): AtmajaGeometryAudit, Entry, Inventory, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+2 more)

### Community 87 - "Community 87"
Cohesion: 0.19
Nodes (4): MelatiNeighborhoodBuilder, MelatiResidenceFactory, PlaytestReviewValidation, Record

### Community 88 - "Community 88"
Cohesion: 0.15
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 89 - "Community 89"
Cohesion: 0.15
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 90 - "Community 90"
Cohesion: 0.19
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 93 - "Community 93"
Cohesion: 0.14
Nodes (5): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests

### Community 94 - "Community 94"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 95 - "Community 95"
Cohesion: 0.14
Nodes (8): Metric, NyonyaMotionRevisionBuilder, Report, AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 96 - "Community 96"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 97 - "Community 97"
Cohesion: 0.25
Nodes (13): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+5 more)

### Community 98 - "Community 98"
Cohesion: 0.17
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 101 - "Community 101"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 104 - "Community 104"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 105 - "Community 105"
Cohesion: 0.17
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 106 - "Community 106"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 107 - "Community 107"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 111 - "Community 111"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 112 - "Community 112"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 116 - "Community 116"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 117 - "Community 117"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 118 - "Community 118"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 119 - "Community 119"
Cohesion: 0.42
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 120 - "Community 120"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 124 - "Community 124"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 126 - "Community 126"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 127 - "Community 127"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 129 - "Community 129"
Cohesion: 0.33
Nodes (3): Phase4MaterialProbe, Report, Row

### Community 131 - "Community 131"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 132 - "Community 132"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 133 - "Community 133"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 134 - "Community 134"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 135 - "Community 135"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 136 - "Community 136"
Cohesion: 0.40
Nodes (5): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid

## Knowledge Gaps
- **615 isolated node(s):** `ActionBusy`, `Analyzer`, `CameraCanScan`, `CameraLocked`, `CameraStatus` (+610 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 987 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **33 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 22` to `Community 1`, `Community 130`, `Community 3`, `Community 4`, `Community 9`, `Community 10`, `Community 11`, `Community 12`, `Community 13`, `Community 14`, `Community 137`, `Community 17`, `Community 18`, `Community 20`, `Community 21`, `Community 24`, `Community 26`, `Community 27`, `Community 28`, `Community 29`, `Community 30`, `Community 31`, `Community 32`, `Community 33`, `Community 34`, `Community 36`, `Community 37`, `Community 39`, `Community 40`, `Community 48`, `Community 50`, `Community 55`, `Community 61`, `Community 62`, `Community 66`, `Community 70`, `Community 75`, `Community 76`, `Community 77`, `Community 78`, `Community 79`, `Community 88`, `Community 90`, `Community 105`, `Community 117`, `Community 122`?**
  _High betweenness centrality (0.189) - this node is a cross-community bridge._
- **What connects `ActionBusy`, `Analyzer`, `CameraCanScan` to the rest of the system?**
  _615 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.044825396825396824 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 6` to `Community 2`, `Community 4`, `Community 5`, `Community 8`, `Community 10`, `Community 18`, `Community 23`, `Community 27`, `Community 32`, `Community 33`, `Community 36`, `Community 39`, `Community 40`, `Community 41`, `Community 46`, `Community 53`, `Community 55`, `Community 70`, `Community 71`, `Community 73`, `Community 77`, `Community 83`, `Community 88`, `Community 110`?**
  _High betweenness centrality (0.073) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.0938354723332632 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 57` to `Community 1`, `Community 67`, `Community 5`, `Community 42`, `Community 21`, `Community 55`, `Community 27`, `Community 28`, `Community 62`?**
  _High betweenness centrality (0.053) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.11886102403343783 - nodes in this community are weakly interconnected._
