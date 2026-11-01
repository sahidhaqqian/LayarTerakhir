# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 5269 nodes · 19924 edges · 168 communities (128 shown, 40 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 1255 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `a7719ae0`
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
- Community 157
- Community 158
- Community 159
- Community 160
- Community 162
- Community 163
- Community 165
- Community 166
- Community 167

## God Nodes (most connected - your core abstractions)
1. `CampaignState` - 134 edges
2. `Phase5Director` - 129 edges
3. `Phase4Director` - 122 edges
4. `LayarTerakhir.Core` - 119 edges
5. `Phase2Director` - 116 edges
6. `Phase3Director` - 114 edges
7. `AtmajaPhoneView` - 112 edges
8. `HudController` - 112 edges
9. `FirstPersonController` - 110 edges
10. `LayarTerakhir.Player` - 103 edges

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

## Communities (168 total, 40 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (12): Phase5Content, KosDialogue, LayarTerakhir.Review, LayarTerakhir.Phase3, LayarTerakhir.Phase2, LayarTerakhir.Phase4, LayarTerakhir.Core, LayarTerakhir.Phase5 (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.04
Nodes (11): HidingAwareness, DesignerRevisionTests, FoundationAuditTests, HidingAwarenessTests, KosBathroomRevisionTests, KosVisualV2Tests, AdditionalPropManifest, AdditionalPropSource (+3 more)

### Community 2 - "Community 2"
Cohesion: 0.05
Nodes (50): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), fetch() (+42 more)

### Community 4 - "Community 4"
Cohesion: 0.05
Nodes (22): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+14 more)

### Community 5 - "Community 5"
Cohesion: 0.08
Nodes (4): Phase5Ids, Phase5CampaignTests, Slot, Phase5Campaign

### Community 7 - "Community 7"
Cohesion: 0.06
Nodes (5): Phase2CampaignTests, Phase2RitualTransactionTests, Phase3CampaignTests, Phase2Campaign, Phase3Campaign

### Community 8 - "Community 8"
Cohesion: 0.06
Nodes (11): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen (+3 more)

### Community 9 - "Community 9"
Cohesion: 0.07
Nodes (10): CompactKosPolish, GoldenSceneProduction, GoldenSceneValidation, Record, GoldenSceneTests, Check, KosSceneValidation, Report (+2 more)

### Community 10 - "Community 10"
Cohesion: 0.05
Nodes (18): DreadoutVisualRevision, Report, SceneReport, InterludeStreetCapture, Report, View, BoundsReport, Diagnosis (+10 more)

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (4): foley(), main(), snapshot(), main()

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (29): Phase2Ids, Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning (+21 more)

### Community 13 - "Community 13"
Cohesion: 0.06
Nodes (38): sample_surface(), solve(), evaluate(), jacobian(), residual(), add_phone(), apparition(), bind() (+30 more)

### Community 14 - "Community 14"
Cohesion: 0.07
Nodes (14): DesignerVrmBuilder, NyonyaMotionTests, Authoring, PrologCompletionBuilder, PrologPresentationBuilder, Receipt, Phase1Anchor, Phase1World (+6 more)

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (20): Phase2Content, PhoneApp, Bank, Call, Camera, Home, Inventory, Journal (+12 more)

### Community 16 - "Community 16"
Cohesion: 0.06
Nodes (35): Phase5Director, ActionBusy, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+27 more)

### Community 17 - "Community 17"
Cohesion: 0.05
Nodes (34): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+26 more)

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (7): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, RoomDressingPlacement, RoomDressingReceipt, Phase1SceneBuilder

### Community 20 - "Community 20"
Cohesion: 0.10
Nodes (14): GoldenLinuxBuild, Record, Phase2SceneBuilder, ImportedMaterial, ImportedMaterials, Phase2NavigationProbe, SceneRecord, Phase2ThermalTarget (+6 more)

### Community 21 - "Community 21"
Cohesion: 0.11
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 22 - "Community 22"
Cohesion: 0.09
Nodes (10): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, Phase4MaterialProbe (+2 more)

### Community 23 - "Community 23"
Cohesion: 0.11
Nodes (9): MatRow, MeshBinding, Phase4SceneBuilder, SceneRecord, Usage, Phase1Navigator, Phase4Anchor, Phase4World (+1 more)

### Community 24 - "Community 24"
Cohesion: 0.08
Nodes (15): CampaignData, Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data, Phase1DynamoUse (+7 more)

### Community 25 - "Community 25"
Cohesion: 0.06
Nodes (15): KosGroundWallTests, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Phase3NavInspection, PlaytestReviewValidation, Record (+7 more)

### Community 26 - "Community 26"
Cohesion: 0.09
Nodes (9): FoundationPlayAuditTests, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent, DisplayBatteryPercent, HasCameraPower, Instance (+1 more)

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (11): MatManifest, Phase5SceneBuilder, SourceEntry, SourceList, MatManifest, MatRow, MeshBinding, Report (+3 more)

### Community 28 - "Community 28"
Cohesion: 0.11
Nodes (3): Phase4Ids, Phase4CampaignTests, Phase4Campaign

### Community 29 - "Community 29"
Cohesion: 0.09
Nodes (10): Entry, Prolog47VisualTests, Folder, Survey, PrologEasyPlayTests, Director, RenderContext, RecoveryPresentationProbe (+2 more)

### Community 30 - "Community 30"
Cohesion: 0.07
Nodes (32): Phase3Director, Altar, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+24 more)

### Community 31 - "Community 31"
Cohesion: 0.11
Nodes (9): ImportedManifest, ImportedMat, Phase3SceneBuilder, BuildRecord, SceneManifest, Usage, Phase3Anchor, Phase3World (+1 more)

### Community 32 - "Community 32"
Cohesion: 0.08
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+6 more)

### Community 33 - "Community 33"
Cohesion: 0.06
Nodes (7): AtmajaGeometryAudit, Entry, Inventory, KosGroundWallRepair, KosRoomSurvey, TempShaftProbe, KosRoomCorrections

### Community 34 - "Community 34"
Cohesion: 0.06
Nodes (29): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+21 more)

### Community 35 - "Community 35"
Cohesion: 0.07
Nodes (5): AtmajaReviewMoments, ARApparitionActive, Hud, InstallationCompleted, Phone

### Community 36 - "Community 36"
Cohesion: 0.05
Nodes (10): box(), cyl(), finish(), tube(), export(), meshes(), box(), cylinder() (+2 more)

### Community 37 - "Community 37"
Cohesion: 0.10
Nodes (16): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+8 more)

### Community 38 - "Community 38"
Cohesion: 0.06
Nodes (26): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText, DisplayedChatCount (+18 more)

### Community 39 - "Community 39"
Cohesion: 0.09
Nodes (10): VoiceLine, Phase5Sound, ARPhotoGlimmer, AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken (+2 more)

### Community 40 - "Community 40"
Cohesion: 0.07
Nodes (19): dialogue_literals(), export(), digest(), get(), main(), objaverse(), ph(), main() (+11 more)

### Community 41 - "Community 41"
Cohesion: 0.06
Nodes (32): Phase4Director, Analyzer, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClarityActive (+24 more)

### Community 42 - "Community 42"
Cohesion: 0.09
Nodes (11): Phase5Boss, Active, B, Exposed, P, Phase, RearCold, Status (+3 more)

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (5): PlayerFeelAudit, Receipt, Sample, GameplayRecoveryRuntimeTests, RecoveryTestInputSettings

### Community 44 - "Community 44"
Cohesion: 0.11
Nodes (15): Phase2Boss, Active, Agent, B, CanCharge, Cooldowns, Exposed, Layers (+7 more)

### Community 45 - "Community 45"
Cohesion: 0.09
Nodes (11): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+3 more)

### Community 46 - "Community 46"
Cohesion: 0.10
Nodes (7): PhoneDocument, Phase3Wewe, B, Head, Pursuing, State, W

### Community 47 - "Community 47"
Cohesion: 0.11
Nodes (4): InterludeCampaignTests, InterludeCampaign, InterludeContent, InterludeIds

### Community 48 - "Community 48"
Cohesion: 0.09
Nodes (5): AtmajaPlayerAudit, Receipt, RumahAtmajaDiningTests, ExplorationCommentary, Zone

### Community 49 - "Community 49"
Cohesion: 0.09
Nodes (17): Phase3Boss, Active, Agent, B, CanCharge, Cooldowns, Layers, PowerTokens (+9 more)

### Community 50 - "Community 50"
Cohesion: 0.07
Nodes (18): InterludeDirector, Busy, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel (+10 more)

### Community 51 - "Community 51"
Cohesion: 0.08
Nodes (10): Phase1WorldItem, InteractableId, IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosInteractableItem, InteractableId (+2 more)

### Community 52 - "Community 52"
Cohesion: 0.19
Nodes (3): Phase2PlayerAudit, NoSecret, Result

### Community 53 - "Community 53"
Cohesion: 0.13
Nodes (5): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 54 - "Community 54"
Cohesion: 0.15
Nodes (3): DesignerWorldRevision, KosVisualUpgrade, VisualAudit

### Community 55 - "Community 55"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 56 - "Community 56"
Cohesion: 0.08
Nodes (10): fetch(), main(), main(), prepare(), foley(), render(), write_pcm(), build_meshes() (+2 more)

### Community 57 - "Community 57"
Cohesion: 0.08
Nodes (3): Phase1Content, LayarTerakhir.Audio, LayarTerakhir.Phase1

### Community 59 - "Community 59"
Cohesion: 0.11
Nodes (4): BloodWriting, MelatiInterludeAlleyBuilder, PrologEasySceneRepair, AtmajaWorldText

### Community 60 - "Community 60"
Cohesion: 0.11
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 61 - "Community 61"
Cohesion: 0.12
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 62 - "Community 62"
Cohesion: 0.12
Nodes (21): copy(), sha(), get(), main(), checked(), export(), image_path(), material_record() (+13 more)

### Community 63 - "Community 63"
Cohesion: 0.11
Nodes (11): DeveloperReviewMode, AnyActive, Capture, Fly, Freeze, HelpOpen, InputFieldFocused, Inspector (+3 more)

### Community 64 - "Community 64"
Cohesion: 0.09
Nodes (12): InterludeHub, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockLabel, DateLabel, Hud (+4 more)

### Community 66 - "Community 66"
Cohesion: 0.10
Nodes (4): Phase4SceneTests, Phase4AssetSource, Phase4WorldItem, InteractableId

### Community 67 - "Community 67"
Cohesion: 0.16
Nodes (4): CampaignRouting, ReviewDirectBoot, ReviewDirectBootTests, Slot

### Community 70 - "Community 70"
Cohesion: 0.11
Nodes (10): Phase2Balance, Phase2Lastri, Attached, B, Head, HoldProgress, InFrame, Learning (+2 more)

### Community 73 - "Community 73"
Cohesion: 0.16
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 74 - "Community 74"
Cohesion: 0.14
Nodes (9): BugReportMetadata, ReviewBugCapture, Busy, DefaultDirectory, LastJsonPath, LastPngPath, SelectionDto, VectorDto (+1 more)

### Community 75 - "Community 75"
Cohesion: 0.16
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 76 - "Community 76"
Cohesion: 0.11
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 77 - "Community 77"
Cohesion: 0.19
Nodes (18): child(), clusters(), encoded(), geometry(), objects(), optimize_file(), position(), protected_vertices() (+10 more)

### Community 80 - "Community 80"
Cohesion: 0.12
Nodes (5): AbandonedLocationLightingTests, Phase5Lighting, AbandonedLocationLighting, NightActive, UnpoweredLightCount

### Community 81 - "Community 81"
Cohesion: 0.15
Nodes (16): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+8 more)

### Community 82 - "Community 82"
Cohesion: 0.18
Nodes (3): GoldenHudBuilder, PhoneGlyph, StoryMapGraphic

### Community 83 - "Community 83"
Cohesion: 0.09
Nodes (11): RitualProvider, CameraLocked, CameraStatus, CanBeginLive, RitualCameraPower, Viewers, IPhoneChapterContent, CameraLocked (+3 more)

### Community 84 - "Community 84"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 85 - "Community 85"
Cohesion: 0.10
Nodes (14): Phase4Balance, Phase4Ritual, Active, B, Exposed, Recovering, Remaining, State (+6 more)

### Community 86 - "Community 86"
Cohesion: 0.12
Nodes (9): FirstPersonController, CrouchAmount, CursorLockRequested, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld (+1 more)

### Community 88 - "Community 88"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 90 - "Community 90"
Cohesion: 0.10
Nodes (9): FoundationAuditCoroutineHost, Phase3Gate, IsOpen, Phase3UvPresentation, Phase4UvPresentation, Phase5SpectralPresentation, ClockAmbience, DoubleLeafGate (+1 more)

### Community 91 - "Community 91"
Cohesion: 0.16
Nodes (5): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape, Phase5Soundscape

### Community 92 - "Community 92"
Cohesion: 0.20
Nodes (3): GameInputConfig, Instance, SystemShortcutHeld

### Community 93 - "Community 93"
Cohesion: 0.12
Nodes (7): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 94 - "Community 94"
Cohesion: 0.15
Nodes (6): Phase4Banaspati, B, LungeProgress, Pursuing, State, W

### Community 95 - "Community 95"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 96 - "Community 96"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 97 - "Community 97"
Cohesion: 0.19
Nodes (10): Phase4Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, PhoneClockRunning (+2 more)

### Community 98 - "Community 98"
Cohesion: 0.18
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 100 - "Community 100"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 102 - "Community 102"
Cohesion: 0.15
Nodes (10): SceneAudioProfile, PresentationProfile, Phase1Balance, Phase3Balance, Phase3EvpSource, Phase3VoiceTag, Ambient, Authentic (+2 more)

### Community 103 - "Community 103"
Cohesion: 0.21
Nodes (9): Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 104 - "Community 104"
Cohesion: 0.21
Nodes (9): Phase3Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone, Ready (+1 more)

### Community 107 - "Community 107"
Cohesion: 0.21
Nodes (6): ReviewObjectInspector, Active, CurrentReport, HasHit, HitPoint, Selected

### Community 108 - "Community 108"
Cohesion: 0.23
Nodes (3): AnimatedRakaAvatar, CinematicCrouch, Installing

### Community 110 - "Community 110"
Cohesion: 0.18
Nodes (5): PrologKosSceneBuilder, PrologMilestoneValidator, InteractionInteractor, ContextResolver, CurrentTarget

### Community 111 - "Community 111"
Cohesion: 0.20
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 112 - "Community 112"
Cohesion: 0.18
Nodes (7): is_arm(), main(), delete_fill_tagged(), membrane_faces(), split_to(), sha(), short_name()

### Community 113 - "Community 113"
Cohesion: 0.14
Nodes (4): Phase3Ids, Phase3Content, Phase3Flare, Landed

### Community 115 - "Community 115"
Cohesion: 0.16
Nodes (7): ReviewPhaseNavigator, ExcursionActive, ExcursionTarget, Loading, OriginScene, TempDirectory, Target

### Community 117 - "Community 117"
Cohesion: 0.22
Nodes (3): Phase4ForestHazard, Phase4Sound, Phase4Soundscape

### Community 118 - "Community 118"
Cohesion: 0.19
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 120 - "Community 120"
Cohesion: 0.18
Nodes (8): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps

### Community 121 - "Community 121"
Cohesion: 0.24
Nodes (3): ReviewFlyCamera, Active, Eye

### Community 123 - "Community 123"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 124 - "Community 124"
Cohesion: 0.29
Nodes (3): Phase3Sound, Phase3Soundscape, Voice

### Community 125 - "Community 125"
Cohesion: 0.17
Nodes (12): Phase3WeweState, Ambush, Chase, Dormant, Investigate, Landing, Loiter, Patrol (+4 more)

### Community 126 - "Community 126"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 127 - "Community 127"
Cohesion: 0.29
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 131 - "Community 131"
Cohesion: 0.22
Nodes (7): Phase3FloorKind, FragileDrop, FragileFatal, IllusionVoid, Solid, Phase3FloorTile, Collapsed

### Community 132 - "Community 132"
Cohesion: 0.18
Nodes (11): BanaspatiState, Chase, Dormant, Exposed, Investigate, Merging, Patrol, Recover (+3 more)

### Community 134 - "Community 134"
Cohesion: 0.35
Nodes (8): append(), cmd_backfill(), cmd_log(), cmd_report(), existing_keys(), load(), main(), utcnow()

### Community 135 - "Community 135"
Cohesion: 0.29
Nodes (5): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild, BuildTag, SceneSpec

### Community 136 - "Community 136"
Cohesion: 0.36
Nodes (3): Frame, LocationLightingAudit, Receipt

### Community 138 - "Community 138"
Cohesion: 0.22
Nodes (9): Phase2BossState, Exposed, Inactive, PowerRecovery, Prep, Recovery, Resolved, Track (+1 more)

### Community 139 - "Community 139"
Cohesion: 0.22
Nodes (8): Phase2Drawer, Prompt, State, Phase2DrawerState, Closed, Gas, Spent, Telegraph

### Community 140 - "Community 140"
Cohesion: 0.22
Nodes (9): Phase4RitualState, CoreExposed, CoreRecovery, CoreTravel, Exposed, Hunt, Inactive, Recovery (+1 more)

### Community 144 - "Community 144"
Cohesion: 0.25
Nodes (8): Phase3BossState, Exposed, Inactive, Prep, Recovery, Resolved, Stalk, VictoryPending

### Community 145 - "Community 145"
Cohesion: 0.36
Nodes (4): box(), cylinder(), finish(), tube()

### Community 146 - "Community 146"
Cohesion: 0.29
Nodes (7): EndingId, BAD, NONE, NORMAL, SECRET, TRUE, Phase5EndingShot

### Community 147 - "Community 147"
Cohesion: 0.48
Nodes (3): RakaPresentationRevisionBuilder, StrideMetric, StrideMetrics

### Community 148 - "Community 148"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 149 - "Community 149"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 150 - "Community 150"
Cohesion: 0.29
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 151 - "Community 151"
Cohesion: 0.29
Nodes (7): DocPaper, BloodyNote, DiaryNote, Newsprint, Notebook, RedEnvelope, WillNote

### Community 153 - "Community 153"
Cohesion: 0.33
Nodes (6): LastriInvestigationState, Attached, Detached, Dormant, Fading, Telegraph

### Community 155 - "Community 155"
Cohesion: 0.47
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 156 - "Community 156"
Cohesion: 0.60
Nodes (3): Metric, NyonyaMotionRevisionBuilder, Report

### Community 158 - "Community 158"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 159 - "Community 159"
Cohesion: 0.40
Nodes (5): RoomDoorState, CasedOpen, Chained, Closed, DebrisBlocked

### Community 162 - "Community 162"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **687 isolated node(s):** `ARRevealed`, `BasementEntry`, `BasementStealth`, `Chase`, `ClockUnlocked` (+682 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 1092 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **40 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 38` to `Community 0`, `Community 3`, `Community 4`, `Community 136`, `Community 8`, `Community 10`, `Community 12`, `Community 142`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 23`, `Community 24`, `Community 26`, `Community 27`, `Community 29`, `Community 30`, `Community 31`, `Community 33`, `Community 34`, `Community 35`, `Community 39`, `Community 41`, `Community 42`, `Community 43`, `Community 44`, `Community 50`, `Community 51`, `Community 54`, `Community 70`, `Community 73`, `Community 78`, `Community 79`, `Community 80`, `Community 82`, `Community 86`, `Community 90`, `Community 91`, `Community 92`, `Community 94`, `Community 97`, `Community 103`, `Community 104`?**
  _High betweenness centrality (0.194) - this node is a cross-community bridge._
- **What connects `ARRevealed`, `BasementEntry`, `BasementStealth` to the rest of the system?**
  _687 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05121412803532009 - nodes in this community are weakly interconnected._
- **Why does `CampaignState` connect `Community 45` to `Community 1`, `Community 4`, `Community 5`, `Community 7`, `Community 12`, `Community 16`, `Community 24`, `Community 26`, `Community 28`, `Community 30`, `Community 37`, `Community 41`, `Community 47`, `Community 57`, `Community 66`, `Community 67`, `Community 75`, `Community 83`, `Community 87`, `Community 90`, `Community 97`, `Community 103`, `Community 104`, `Community 110`, `Community 127`?**
  _High betweenness centrality (0.060) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.03618756371049949 - nodes in this community are weakly interconnected._
- **Why does `Phase2Director` connect `Community 12` to `Community 0`, `Community 136`, `Community 8`, `Community 139`, `Community 14`, `Community 15`, `Community 18`, `Community 20`, `Community 23`, `Community 24`, `Community 26`, `Community 38`, `Community 44`, `Community 45`, `Community 51`, `Community 52`, `Community 59`, `Community 61`, `Community 70`, `Community 78`, `Community 80`, `Community 83`, `Community 86`, `Community 90`, `Community 91`, `Community 122`?**
  _High betweenness centrality (0.041) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.0463828314295604 - nodes in this community are weakly interconnected._
