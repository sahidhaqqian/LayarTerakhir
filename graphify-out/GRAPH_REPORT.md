# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5025 nodes · 18862 edges · 151 communities (122 shown, 29 thin omitted)
- Extraction: 93% EXTRACTED · 7% INFERRED · 0% AMBIGUOUS · INFERRED: 1261 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `5d6d5095`
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
- Community 122
- Community 123
- Community 124
- Community 125
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

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 152 edges
2. `LayarTerakhir.Core` - 150 edges
3. `Phase5Director` - 129 edges
4. `Phase4Director` - 122 edges
5. `AtmajaDirector` - 120 edges
6. `Phase2Director` - 116 edges
7. `Phase3Director` - 114 edges
8. `FirstPersonController` - 113 edges
9. `AtmajaPhoneView` - 109 edges
10. `LayarTerakhir.Player` - 109 edges

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

## Communities (151 total, 29 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (8): KosDialogue, LayarTerakhir.Review, LayarTerakhir.Core, LayarTerakhir.UI, LayarTerakhir.Player, LayarTerakhir.Smartphone, LayarTerakhir.Prolog, LayarTerakhir.Diagnostics

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (15): AtmajaGeometryAudit, Entry, Inventory, CompactKosPolish, GoldenSceneValidation, Record, GoldenSceneTests, Check (+7 more)

### Community 3 - "Community 3"
Cohesion: 0.05
Nodes (42): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+34 more)

### Community 5 - "Community 5"
Cohesion: 0.05
Nodes (4): foley(), main(), snapshot(), main()

### Community 6 - "Community 6"
Cohesion: 0.06
Nodes (33): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+25 more)

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (16): Bindings, SourceBinding, Phase5SceneBuilder, SourceEntry, SourceList, Bindings, MatRow, MeshBinding (+8 more)

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (7): HidingAwareness, HidingAwarenessTests, KosVisualV2Tests, Phase5CampaignTests, Slot, Phase5SceneTests, Phase5AssetSource

### Community 9 - "Community 9"
Cohesion: 0.06
Nodes (30): PhoneDocument, Phase2Ids, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (16): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, SceneRecord, PlaytestReviewValidation, Record (+8 more)

### Community 11 - "Community 11"
Cohesion: 0.07
Nodes (23): KosGroundWallRepair, ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Balance (+15 more)

### Community 12 - "Community 12"
Cohesion: 0.05
Nodes (5): Phase5Content, LayarTerakhir.Phase3, LayarTerakhir.Audio, LayarTerakhir.Phase4, LayarTerakhir.Phase5

### Community 13 - "Community 13"
Cohesion: 0.07
Nodes (12): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+4 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (27): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+19 more)

### Community 15 - "Community 15"
Cohesion: 0.09
Nodes (5): CampaignData, Phase5Ids, Phase5ReviewFixture, Phase5SaveCrashProbe, Phase5Campaign

### Community 16 - "Community 16"
Cohesion: 0.07
Nodes (10): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Authoring, PrologCompletionBuilder, Phase1LightBudget (+2 more)

### Community 17 - "Community 17"
Cohesion: 0.08
Nodes (11): AtmajaStairRevisionBuilder, BandVertex, CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport (+3 more)

### Community 18 - "Community 18"
Cohesion: 0.06
Nodes (20): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+12 more)

### Community 19 - "Community 19"
Cohesion: 0.04
Nodes (24): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+16 more)

### Community 20 - "Community 20"
Cohesion: 0.10
Nodes (10): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1Anchor, Phase1World (+2 more)

### Community 21 - "Community 21"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 22 - "Community 22"
Cohesion: 0.07
Nodes (33): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow() (+25 more)

### Community 23 - "Community 23"
Cohesion: 0.10
Nodes (11): MatManifest, MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, MatManifest, Phase4Lighting (+3 more)

### Community 24 - "Community 24"
Cohesion: 0.07
Nodes (11): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts, PrologKosSceneBuilder, PrologMilestoneValidator (+3 more)

### Community 25 - "Community 25"
Cohesion: 0.09
Nodes (4): Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign

### Community 26 - "Community 26"
Cohesion: 0.07
Nodes (32): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+24 more)

### Community 27 - "Community 27"
Cohesion: 0.06
Nodes (15): GameplayRecoveryRuntimeTests, KosGroundWallTests, TempShaftProbe, DoubleLeafGate, IsOpen, KosCorridorDirector, Busy, CanDepart (+7 more)

### Community 28 - "Community 28"
Cohesion: 0.07
Nodes (6): DreadoutVisualRevision, Report, SceneReport, PlaytestRepairBuilder, InterludeExterior, Phase4FearPresentation

### Community 29 - "Community 29"
Cohesion: 0.05
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 30 - "Community 30"
Cohesion: 0.12
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 31 - "Community 31"
Cohesion: 0.08
Nodes (6): Phase1PlayerAudit, Receipt, PlayerFeelAudit, Receipt, Sample, RecoveryTestInputSettings

### Community 32 - "Community 32"
Cohesion: 0.11
Nodes (4): KosVisualUpgrade, VisualAudit, MelatiInterludeAlleyBuilder, AtmajaWorldText

### Community 33 - "Community 33"
Cohesion: 0.08
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 34 - "Community 34"
Cohesion: 0.05
Nodes (25): InterludeContent, InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+17 more)

### Community 35 - "Community 35"
Cohesion: 0.06
Nodes (5): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Balance, Phase1Ids

### Community 36 - "Community 36"
Cohesion: 0.08
Nodes (15): GoldenAudioTap, GoldenPlayerAudit, Receipt, Receipt, ReviewPlayerAudit, Hud, Phone, View (+7 more)

### Community 37 - "Community 37"
Cohesion: 0.13
Nodes (3): AtmajaReviewDressing, AtmajaUpperBuilder, ClockAmbience

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (22): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, Phase2Hub, CameraLocked (+14 more)

### Community 39 - "Community 39"
Cohesion: 0.07
Nodes (29): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+21 more)

### Community 40 - "Community 40"
Cohesion: 0.11
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 41 - "Community 41"
Cohesion: 0.11
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 42 - "Community 42"
Cohesion: 0.10
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 43 - "Community 43"
Cohesion: 0.10
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 44 - "Community 44"
Cohesion: 0.12
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 45 - "Community 45"
Cohesion: 0.06
Nodes (24): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+16 more)

### Community 46 - "Community 46"
Cohesion: 0.08
Nodes (27): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+19 more)

### Community 47 - "Community 47"
Cohesion: 0.09
Nodes (5): CampaignRouting, ReviewDirectBoot, ReviewDirectBootTests, Slot, Phase3Campaign

### Community 48 - "Community 48"
Cohesion: 0.08
Nodes (19): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+11 more)

### Community 49 - "Community 49"
Cohesion: 0.10
Nodes (9): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen (+1 more)

### Community 50 - "Community 50"
Cohesion: 0.13
Nodes (4): FoundationAuditCoroutineHost, FoundationPlayAuditTests, RecoveryPresentationProbe, RecoveryRuntimeScenario

### Community 51 - "Community 51"
Cohesion: 0.09
Nodes (8): DeveloperReviewModeTests, ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 53 - "Community 53"
Cohesion: 0.10
Nodes (12): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse, Phase1MoneyReceipt (+4 more)

### Community 54 - "Community 54"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 55 - "Community 55"
Cohesion: 0.11
Nodes (3): InterludeCampaignTests, InterludeCampaign, InterludeIds

### Community 56 - "Community 56"
Cohesion: 0.08
Nodes (11): InterludeExteriorAudit, View, FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting (+3 more)

### Community 57 - "Community 57"
Cohesion: 0.11
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 58 - "Community 58"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 59 - "Community 59"
Cohesion: 0.06
Nodes (30): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+22 more)

### Community 60 - "Community 60"
Cohesion: 0.09
Nodes (7): AtmajaPlayerAudit, Receipt, RecoveryVisualEvidence, InteractionInteractor, ContextResolver, CurrentTarget, AtmajaMirrorReflection

### Community 61 - "Community 61"
Cohesion: 0.10
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 62 - "Community 62"
Cohesion: 0.11
Nodes (20): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+12 more)

### Community 63 - "Community 63"
Cohesion: 0.09
Nodes (8): AmbientAudioLoop, LevelMultiplier, Bike, InterludeTraffic, Phase2Sound, Phase2Soundscape, Phase4Sound, Phase4Soundscape

### Community 66 - "Community 66"
Cohesion: 0.14
Nodes (6): GoldenHudBuilder, HudStyle, PhoneGlyph, PhoneSkin, Rounded, StoryMapGraphic

### Community 67 - "Community 67"
Cohesion: 0.08
Nodes (16): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+8 more)

### Community 68 - "Community 68"
Cohesion: 0.11
Nodes (8): SceneAudioProfile, PresentationProfile, VoiceLine, VoiceBank, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 69 - "Community 69"
Cohesion: 0.08
Nodes (3): DesignerRevisionTests, DisplayPreferences, Isolated

### Community 70 - "Community 70"
Cohesion: 0.11
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 71 - "Community 71"
Cohesion: 0.09
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 72 - "Community 72"
Cohesion: 0.09
Nodes (12): DevelopmentScreenshotCapture, Phase3UvPresentation, Phase4UvPresentation, Phase5SpectralPresentation, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+4 more)

### Community 73 - "Community 73"
Cohesion: 0.10
Nodes (4): GraphicsReferenceAudit, Receipt, Receipt, RiderPresentationAudit

### Community 75 - "Community 75"
Cohesion: 0.11
Nodes (15): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph, Phase2Lastri, Attached (+7 more)

### Community 76 - "Community 76"
Cohesion: 0.14
Nodes (9): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+1 more)

### Community 77 - "Community 77"
Cohesion: 0.10
Nodes (6): Phase4MaterialProbe, Report, Row, Phase4SceneTests, Phase4AssetSource, Phase4ForestHazard

### Community 78 - "Community 78"
Cohesion: 0.12
Nodes (4): PlayerFeelTests, AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 79 - "Community 79"
Cohesion: 0.08
Nodes (19): Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State, Status (+11 more)

### Community 81 - "Community 81"
Cohesion: 0.15
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 82 - "Community 82"
Cohesion: 0.11
Nodes (7): KosTravelItem, InteractableId, MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 83 - "Community 83"
Cohesion: 0.19
Nodes (18): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+10 more)

### Community 84 - "Community 84"
Cohesion: 0.12
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 85 - "Community 85"
Cohesion: 0.13
Nodes (7): fetch(), main(), main(), prepare(), foley(), render(), write_pcm()

### Community 86 - "Community 86"
Cohesion: 0.14
Nodes (12): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+4 more)

### Community 87 - "Community 87"
Cohesion: 0.16
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 89 - "Community 89"
Cohesion: 0.15
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 90 - "Community 90"
Cohesion: 0.14
Nodes (3): Phase4Content, Phase4WorldItem, InteractableId

### Community 91 - "Community 91"
Cohesion: 0.15
Nodes (10): InterludeStreetCapture, Report, View, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+2 more)

### Community 92 - "Community 92"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 95 - "Community 95"
Cohesion: 0.17
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 96 - "Community 96"
Cohesion: 0.14
Nodes (5): AtmajaThreatPresentation, CurrentMotionClip, ExternalStun, HasCharacterRig, SampledHuntPoseTime

### Community 97 - "Community 97"
Cohesion: 0.25
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 98 - "Community 98"
Cohesion: 0.14
Nodes (5): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests

### Community 99 - "Community 99"
Cohesion: 0.18
Nodes (6): Phase3Wewe, B, Head, Pursuing, State, W

### Community 100 - "Community 100"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 102 - "Community 102"
Cohesion: 0.15
Nodes (9): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot, Phase5Sound (+1 more)

### Community 103 - "Community 103"
Cohesion: 0.17
Nodes (4): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId

### Community 104 - "Community 104"
Cohesion: 0.22
Nodes (5): Phase2Balance, Phase4Balance, Phase5Balance, Phase5Replicas, B

### Community 105 - "Community 105"
Cohesion: 0.17
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 108 - "Community 108"
Cohesion: 0.29
Nodes (4): BugReportMetadata, SelectionDto, VectorDto, ReviewHierarchy

### Community 109 - "Community 109"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 111 - "Community 111"
Cohesion: 0.19
Nodes (5): ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath

### Community 113 - "Community 113"
Cohesion: 0.26
Nodes (6): Metric, NyonyaMotionRevisionBuilder, Report, RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 114 - "Community 114"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 115 - "Community 115"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 116 - "Community 116"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 117 - "Community 117"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 118 - "Community 118"
Cohesion: 0.31
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 119 - "Community 119"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 120 - "Community 120"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 122 - "Community 122"
Cohesion: 0.22
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 123 - "Community 123"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 124 - "Community 124"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 127 - "Community 127"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 128 - "Community 128"
Cohesion: 0.28
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 129 - "Community 129"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 133 - "Community 133"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 135 - "Community 135"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 136 - "Community 136"
Cohesion: 0.36
Nodes (4): box(), cylinder(), finish(), tube()

### Community 137 - "Community 137"
Cohesion: 0.43
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 139 - "Community 139"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 141 - "Community 141"
Cohesion: 0.47
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 143 - "Community 143"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 144 - "Community 144"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 145 - "Community 145"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 147 - "Community 147"
Cohesion: 0.50
Nodes (3): Phase4Torch, Authentic, PermanentlyOff

## Knowledge Gaps
- **674 isolated node(s):** `InteractableId`, `IsOpen`, `ARRevealed`, `BasementEntry`, `BasementStealth` (+669 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1060 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **29 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 45` to `Community 0`, `Community 1`, `Community 2`, `Community 6`, `Community 7`, `Community 9`, `Community 10`, `Community 11`, `Community 14`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 23`, `Community 24`, `Community 25`, `Community 31`, `Community 32`, `Community 34`, `Community 36`, `Community 38`, `Community 39`, `Community 40`, `Community 42`, `Community 43`, `Community 44`, `Community 49`, `Community 50`, `Community 52`, `Community 53`, `Community 56`, `Community 59`, `Community 60`, `Community 63`, `Community 66`, `Community 70`, `Community 71`, `Community 72`, `Community 73`, `Community 82`, `Community 84`, `Community 86`, `Community 95`, `Community 96`, `Community 110`, `Community 121`, `Community 124`?**
  _High betweenness centrality (0.192) - this node is a cross-community bridge._
- **What connects `InteractableId`, `IsOpen`, `ARRevealed` to the rest of the system?**
  _674 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.08040033361134279 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 13` to `Community 6`, `Community 8`, `Community 9`, `Community 15`, `Community 21`, `Community 24`, `Community 25`, `Community 30`, `Community 31`, `Community 33`, `Community 34`, `Community 35`, `Community 38`, `Community 42`, `Community 44`, `Community 47`, `Community 50`, `Community 51`, `Community 53`, `Community 55`, `Community 57`, `Community 59`, `Community 67`, `Community 72`, `Community 81`, `Community 86`, `Community 88`, `Community 95`?**
  _High betweenness centrality (0.091) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.08356435643564357 - nodes in this community are weakly interconnected._
- **Why does `Phase5Director` connect `Community 6` to `Community 0`, `Community 2`, `Community 7`, `Community 8`, `Community 11`, `Community 12`, `Community 13`, `Community 15`, `Community 16`, `Community 20`, `Community 149`, `Community 150`, `Community 38`, `Community 40`, `Community 45`, `Community 49`, `Community 52`, `Community 53`, `Community 56`, `Community 72`, `Community 74`, `Community 76`, `Community 84`, `Community 86`, `Community 87`, `Community 102`, `Community 104`, `Community 124`?**
  _High betweenness centrality (0.054) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.06444444444444444 - nodes in this community are weakly interconnected._
