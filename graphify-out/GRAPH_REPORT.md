# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 2960 nodes · 10509 edges · 102 communities (80 shown, 22 thin omitted)
- Extraction: 94% EXTRACTED · 6% INFERRED · 0% AMBIGUOUS · INFERRED: 637 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `98b02090`
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
- Community 99
- Community 100
- Community 101

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 100 edges
2. `AtmajaPhoneView` - 98 edges
3. `HudController` - 83 edges
4. `ShoulderCameraRig` - 81 edges
5. `Phase2Director` - 81 edges
6. `AtmajaPlayTests` - 78 edges
7. `LayarTerakhir.Prolog` - 74 edges
8. `LayarTerakhir.Player` - 71 edges
9. `FirstPersonController` - 69 edges
10. `LayarTerakhir.Core` - 69 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `PrologCompletionAudit` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Diagnostics/PrologCompletionAudit.cs → Assets/_LayarTerakhir/src/Core/CampaignState.cs

## Import Cycles
- None detected.

## Communities (102 total, 22 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.06
Nodes (14): Phase2AssetImport, Phase1Content, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Phase2, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Phase1 (+6 more)

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (37): asset(), download(), asset(), download(), asset(), fetch(), dialogue_literals(), export() (+29 more)

### Community 3 - "Community 3"
Cohesion: 0.04
Nodes (14): box(), cyl(), finish(), tube(), box(), cylinder(), finish(), tube() (+6 more)

### Community 4 - "Community 4"
Cohesion: 0.11
Nodes (5): CompactKosPolish, GoldenSceneValidation, Record, KosVisualV2, FloorY

### Community 5 - "Community 5"
Cohesion: 0.06
Nodes (26): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+18 more)

### Community 6 - "Community 6"
Cohesion: 0.06
Nodes (27): Phase2Director, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive, ClockRunning, CurrentRoom (+19 more)

### Community 7 - "Community 7"
Cohesion: 0.09
Nodes (6): KosVisualUpgrade, VisualAudit, MelatiStreetRevision, Report, TempShaftProbe, KosRoomCorrections

### Community 8 - "Community 8"
Cohesion: 0.09
Nodes (12): FoundationPlayAuditTests, KosInteractableItem, InteractableId, ScanVisualMode, AR, Normal, UV, SmartphoneController (+4 more)

### Community 9 - "Community 9"
Cohesion: 0.08
Nodes (13): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+5 more)

### Community 10 - "Community 10"
Cohesion: 0.08
Nodes (11): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+3 more)

### Community 11 - "Community 11"
Cohesion: 0.06
Nodes (16): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, KosTravelItem, InteractableId, MelatiDeparture, InteractableId (+8 more)

### Community 12 - "Community 12"
Cohesion: 0.06
Nodes (21): AtmajaMirrorReflection, AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, ClockText, DateText (+13 more)

### Community 13 - "Community 13"
Cohesion: 0.08
Nodes (9): load_meta(), convert(), main(), prepare(), foley(), render(), write_pcm(), main() (+1 more)

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (16): AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests, Phase1Ids, Phase1Stage, Arrival, Boss, Complete (+8 more)

### Community 15 - "Community 15"
Cohesion: 0.09
Nodes (9): PresentationProfile, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification (+1 more)

### Community 16 - "Community 16"
Cohesion: 0.07
Nodes (27): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+19 more)

### Community 17 - "Community 17"
Cohesion: 0.15
Nodes (3): PhoneSkin, Rounded, UiWidgets

### Community 18 - "Community 18"
Cohesion: 0.07
Nodes (19): PlayerFeelAudit, Receipt, Sample, PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint (+11 more)

### Community 19 - "Community 19"
Cohesion: 0.13
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 20 - "Community 20"
Cohesion: 0.10
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 21 - "Community 21"
Cohesion: 0.08
Nodes (4): DesignerRevisionTests, GoldenSceneTests, Phase2SceneTests, PlayerFeelTests

### Community 22 - "Community 22"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 24 - "Community 24"
Cohesion: 0.15
Nodes (5): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt, Phase1Anchor

### Community 25 - "Community 25"
Cohesion: 0.07
Nodes (18): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cylinder() (+10 more)

### Community 26 - "Community 26"
Cohesion: 0.13
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 27 - "Community 27"
Cohesion: 0.08
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 28 - "Community 28"
Cohesion: 0.09
Nodes (22): add_phone(), apparition(), bind(), bind_nyonya_shears(), build(), canonical(), DesignerHeadroom, digest() (+14 more)

### Community 29 - "Community 29"
Cohesion: 0.10
Nodes (6): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, Phase1SceneBuilder, AtmajaWorldText, NeighborhoodRain

### Community 30 - "Community 30"
Cohesion: 0.10
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 31 - "Community 31"
Cohesion: 0.08
Nodes (10): sample_surface(), solve(), evaluate(), jacobian(), residual(), WeightOnlyLinear, Info, read() (+2 more)

### Community 32 - "Community 32"
Cohesion: 0.08
Nodes (12): GraphicsReferenceAudit, Receipt, PrologKosSceneBuilder, PrologMilestoneValidator, FirstPersonController, CrouchAmount, IsCrouching, IsSliding (+4 more)

### Community 33 - "Community 33"
Cohesion: 0.13
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 34 - "Community 34"
Cohesion: 0.12
Nodes (8): Receipt, ReviewPlayerAudit, Hud, Phone, View, KosCorridorDirector, Busy, CanDepart

### Community 35 - "Community 35"
Cohesion: 0.11
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 36 - "Community 36"
Cohesion: 0.16
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 37 - "Community 37"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 38 - "Community 38"
Cohesion: 0.13
Nodes (4): DesignerVrmBuilder, Phase2NavigationProbe, PrologPresentationBuilder, Receipt

### Community 40 - "Community 40"
Cohesion: 0.11
Nodes (8): CampaignData, CampaignRouting, Phase2Data, Phase2Ids, UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 41 - "Community 41"
Cohesion: 0.11
Nodes (4): AtmajaPlayerAudit, Receipt, Receipt, RiderPresentationAudit

### Community 42 - "Community 42"
Cohesion: 0.17
Nodes (3): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 43 - "Community 43"
Cohesion: 0.14
Nodes (4): AtmajaStairRevisionBuilder, BandVertex, KosRoomSurvey, ARPhotoGlimmer

### Community 44 - "Community 44"
Cohesion: 0.19
Nodes (4): KosCorridorBuilder, HingedDoor, InteractableId, IsOpen

### Community 45 - "Community 45"
Cohesion: 0.10
Nodes (10): Phase2Content, Phase2Hub, CameraLocked, CameraStatus, Campaign, CanBeginLive, Hud, Phone (+2 more)

### Community 47 - "Community 47"
Cohesion: 0.23
Nodes (3): Phase2SceneBuilder, ImportedMaterial, Phase2Anchor

### Community 48 - "Community 48"
Cohesion: 0.13
Nodes (8): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection

### Community 49 - "Community 49"
Cohesion: 0.08
Nodes (12): FoundationAuditCoroutineHost, Phase2ThermalTarget, InteractionInteractor, ContextResolver, CurrentTarget, AtmajaThreatPresentation, ExternalStun, HasCharacterRig (+4 more)

### Community 50 - "Community 50"
Cohesion: 0.09
Nodes (5): opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 51 - "Community 51"
Cohesion: 0.15
Nodes (4): DreadoutVisualRevision, Report, SceneReport, Phase1LightBudget

### Community 52 - "Community 52"
Cohesion: 0.18
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 54 - "Community 54"
Cohesion: 0.18
Nodes (4): Phase1DynamoUse, Phase1Director, Phase1WorldItem, InteractableId

### Community 55 - "Community 55"
Cohesion: 0.17
Nodes (5): GoldenHudBuilder, Phase1Balance, Phase2Balance, HudPresentation, HudStyle

### Community 56 - "Community 56"
Cohesion: 0.13
Nodes (10): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+2 more)

### Community 57 - "Community 57"
Cohesion: 0.20
Nodes (3): Phase1MoneyReceipt, Phase2CampaignTests, Phase2Campaign

### Community 58 - "Community 58"
Cohesion: 0.14
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 60 - "Community 60"
Cohesion: 0.21
Nodes (3): Phase2Director, Phase2WorldItem, InteractableId

### Community 61 - "Community 61"
Cohesion: 0.17
Nodes (4): AmbientAudioLoop, LevelMultiplier, Phase2Sound, Phase2Soundscape

### Community 62 - "Community 62"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 63 - "Community 63"
Cohesion: 0.21
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 64 - "Community 64"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 65 - "Community 65"
Cohesion: 0.12
Nodes (14): IPhoneChapterContent, CameraLocked, CameraStatus, CanBeginLive, Viewers, PhoneApp, Bank, Call (+6 more)

### Community 66 - "Community 66"
Cohesion: 0.25
Nodes (10): export(), extract_materials(), extras(), glbs(), legacy_factors(), madduck(), merah(), neonatal() (+2 more)

### Community 67 - "Community 67"
Cohesion: 0.19
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 68 - "Community 68"
Cohesion: 0.23
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 70 - "Community 70"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 72 - "Community 72"
Cohesion: 0.25
Nodes (4): ThreatAudioOcclusion, Occluded, Phase1Sound, Phase1Soundscape

### Community 73 - "Community 73"
Cohesion: 0.22
Nodes (5): VoiceLine, VoiceBank, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 77 - "Community 77"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 78 - "Community 78"
Cohesion: 0.24
Nodes (4): AtmajaItem, InteractableId, DoubleLeafGate, IsOpen

### Community 80 - "Community 80"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 81 - "Community 81"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 82 - "Community 82"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 83 - "Community 83"
Cohesion: 0.25
Nodes (6): Phase1Challenge, Expired, Offered, Succeeded, Unoffered, Phase1Data

### Community 85 - "Community 85"
Cohesion: 0.32
Nodes (3): PlaytestReviewValidation, Record, PlaytestReviewTests

### Community 88 - "Community 88"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 90 - "Community 90"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 91 - "Community 91"
Cohesion: 0.33
Nodes (6): EndingType, Bad, None, Normal, Secret, True

### Community 93 - "Community 93"
Cohesion: 0.47
Nodes (3): AtmajaHandycamPlayback, IsPlaying, Progress

### Community 95 - "Community 95"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 96 - "Community 96"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

### Community 97 - "Community 97"
Cohesion: 0.40
Nodes (5): Phase2DrawerState, Closed, Gas, Spent, Telegraph

## Knowledge Gaps
- **369 isolated node(s):** `BackupSaveFilePath`, `Data`, `HasValidCheckpoint`, `Instance`, `SaveDirectory` (+364 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 655 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **22 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 12` to `Community 0`, `Community 1`, `Community 4`, `Community 5`, `Community 6`, `Community 7`, `Community 8`, `Community 9`, `Community 11`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 20`, `Community 22`, `Community 23`, `Community 24`, `Community 25`, `Community 26`, `Community 27`, `Community 29`, `Community 30`, `Community 32`, `Community 33`, `Community 34`, `Community 35`, `Community 36`, `Community 38`, `Community 45`, `Community 46`, `Community 48`, `Community 49`, `Community 55`, `Community 59`, `Community 61`, `Community 63`, `Community 68`, `Community 75`, `Community 87`?**
  _High betweenness centrality (0.259) - this node is a cross-community bridge._
- **What connects `BackupSaveFilePath`, `Data`, `HasValidCheckpoint` to the rest of the system?**
  _369 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05667463298053943 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 26` to `Community 0`, `Community 1`, `Community 4`, `Community 7`, `Community 8`, `Community 9`, `Community 10`, `Community 12`, `Community 15`, `Community 16`, `Community 17`, `Community 19`, `Community 20`, `Community 21`, `Community 23`, `Community 32`, `Community 33`, `Community 34`, `Community 36`, `Community 38`, `Community 41`, `Community 42`, `Community 43`, `Community 44`, `Community 49`, `Community 52`, `Community 53`, `Community 61`, `Community 62`, `Community 63`, `Community 67`, `Community 78`, `Community 85`, `Community 86`?**
  _High betweenness centrality (0.072) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.091164095371669 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 56` to `Community 0`, `Community 32`, `Community 33`, `Community 7`, `Community 39`, `Community 76`, `Community 49`, `Community 86`, `Community 23`, `Community 25`, `Community 61`?**
  _High betweenness centrality (0.071) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.0633964429145152 - nodes in this community are weakly interconnected._
