# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 2609 nodes · 9171 edges · 100 communities (76 shown, 24 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 502 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `f5f2ae7d`
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
- Community 82
- Community 83
- Community 84
- Community 85
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
- Community 99

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 94 edges
2. `AtmajaPhoneView` - 87 edges
3. `Phase1Director` - 86 edges
4. `HudController` - 80 edges
5. `ShoulderCameraRig` - 80 edges
6. `AtmajaPlayTests` - 78 edges
7. `LayarTerakhir.Prolog` - 71 edges
8. `LayarTerakhir.Player` - 69 edges
9. `AtmajaChaseSequence` - 66 edges
10. `FirstPersonController` - 63 edges

## Surprising Connections (you probably didn't know these)
- `add_phone()` --calls--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `actions()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --calls--> `export_phone_hand()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `build()` --indirect_call--> `hand_frame()`  [INFERRED]
  Tools/prepare_designer_vrm.py → Tools/author_prolog_characters.py
- `AtmajaBasementHorror` --references--> `AtmajaChaseSequence`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/src/Prolog/AtmajaChaseSequence.cs

## Import Cycles
- None detected.

## Communities (100 total, 24 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.06
Nodes (17): GoldenLinuxBuild, Record, GoldenSceneValidation, Record, GoldenSceneTests, Phase1Content, KosDialogue, LayarTerakhir.Editor (+9 more)

### Community 2 - "Community 2"
Cohesion: 0.07
Nodes (28): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), fetch() (+20 more)

### Community 3 - "Community 3"
Cohesion: 0.11
Nodes (3): CompactKosPolish, KosVisualV2, FloorY

### Community 4 - "Community 4"
Cohesion: 0.11
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaUpperBuilder

### Community 5 - "Community 5"
Cohesion: 0.06
Nodes (26): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+18 more)

### Community 6 - "Community 6"
Cohesion: 0.10
Nodes (4): DressingReceipt, Phase1SceneBuilder, PropPlacement, ImportedSceneReceipt

### Community 7 - "Community 7"
Cohesion: 0.10
Nodes (10): FoundationAuditCoroutineHost, FoundationPlayAuditTests, FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked (+2 more)

### Community 8 - "Community 8"
Cohesion: 0.09
Nodes (9): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection, AtmajaWorldText (+1 more)

### Community 9 - "Community 9"
Cohesion: 0.12
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 10 - "Community 10"
Cohesion: 0.09
Nodes (13): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsFinalFlash, IsGateRun (+5 more)

### Community 11 - "Community 11"
Cohesion: 0.09
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 12 - "Community 12"
Cohesion: 0.08
Nodes (26): solve(), evaluate(), jacobian(), residual(), add_phone(), apparition(), bind(), bind_nyonya_shears() (+18 more)

### Community 13 - "Community 13"
Cohesion: 0.13
Nodes (8): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, KosRoomSurvey

### Community 14 - "Community 14"
Cohesion: 0.06
Nodes (18): AtmajaPhoneView, Apps, Chapter, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, FocusHandsVisible (+10 more)

### Community 15 - "Community 15"
Cohesion: 0.12
Nodes (6): KosVisualUpgrade, VisualAudit, TempShaftProbe, KosInteractableItem, InteractableId, KosRoomCorrections

### Community 16 - "Community 16"
Cohesion: 0.11
Nodes (14): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+6 more)

### Community 17 - "Community 17"
Cohesion: 0.07
Nodes (17): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+9 more)

### Community 18 - "Community 18"
Cohesion: 0.08
Nodes (12): PresentationProfile, AtmajaReviewMoments, Hud, Phone, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance (+4 more)

### Community 19 - "Community 19"
Cohesion: 0.09
Nodes (3): foley(), main(), snapshot()

### Community 20 - "Community 20"
Cohesion: 0.12
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (11): ball(), box(), cylinder(), mat(), tube(), export(), meshes(), box() (+3 more)

### Community 22 - "Community 22"
Cohesion: 0.11
Nodes (8): VoiceLine, AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 23 - "Community 23"
Cohesion: 0.07
Nodes (16): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, AimPoint, Avatar, AvatarOpacity, CinematicVisible (+8 more)

### Community 24 - "Community 24"
Cohesion: 0.11
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 25 - "Community 25"
Cohesion: 0.11
Nodes (7): BloodWriting, MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, Phase1SceneBuilder, ARPhotoGlimmer

### Community 26 - "Community 26"
Cohesion: 0.09
Nodes (14): Phase1Threats, Balance, BossStatus, Exposed, FreezeKunti, IsChasing, Kunti, KuntiManifested (+6 more)

### Community 27 - "Community 27"
Cohesion: 0.10
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 28 - "Community 28"
Cohesion: 0.08
Nodes (7): dialogue_literals(), export(), opening_structure_sha256(), verify_opening_source(), DialogueRecordingTests, GraphifyDiscoveryTests, OpeningPresentationIdentityTests

### Community 29 - "Community 29"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 30 - "Community 30"
Cohesion: 0.08
Nodes (17): Phase1Director, BossActive, Busy, CameraCanScan, CameraLocked, CameraStatus, Campaign, CanBeginLive (+9 more)

### Community 31 - "Community 31"
Cohesion: 0.11
Nodes (5): CampaignData, PhoneDocument, Phase1Data, Phase1MoneyReceipt, Phase1Ids

### Community 32 - "Community 32"
Cohesion: 0.09
Nodes (5): CampaignRouting, Phase1DynamoUse, AdditionalPropManifest, AdditionalPropSource, Phase1CampaignTests

### Community 34 - "Community 34"
Cohesion: 0.15
Nodes (7): MotorcycleRiderAlignment, PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer, UiWidgets

### Community 35 - "Community 35"
Cohesion: 0.15
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 37 - "Community 37"
Cohesion: 0.11
Nodes (10): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+2 more)

### Community 38 - "Community 38"
Cohesion: 0.10
Nodes (8): AmbientAudioLoop, LevelMultiplier, AtmajaThreatPresentation, ExternalStun, HasCharacterRig, SampledHuntPoseTime, AtmajaVault, ClockAmbience

### Community 39 - "Community 39"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 40 - "Community 40"
Cohesion: 0.19
Nodes (3): DesignerVrmBuilder, PrologPresentationBuilder, Receipt

### Community 41 - "Community 41"
Cohesion: 0.17
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 42 - "Community 42"
Cohesion: 0.18
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 43 - "Community 43"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 44 - "Community 44"
Cohesion: 0.13
Nodes (7): VoiceBank, DesignerWorldRevision, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, DoubleLeafGate, IsOpen

### Community 46 - "Community 46"
Cohesion: 0.20
Nodes (3): DreadoutVisualRevision, Report, SceneReport

### Community 47 - "Community 47"
Cohesion: 0.14
Nodes (9): ScanVisualMode, AR, Normal, UV, SmartphoneController, BlitzCooldownRemaining, CameraLocked, ChapterContent (+1 more)

### Community 48 - "Community 48"
Cohesion: 0.13
Nodes (5): ThreatAudioOcclusion, Occluded, Phase1Projectile, Phase1Sound, Phase1Soundscape

### Community 49 - "Community 49"
Cohesion: 0.15
Nodes (7): IContextInteractionResolver, OwnsInteraction, IInteractable, InteractableId, InteractionInteractor, ContextResolver, CurrentTarget

### Community 52 - "Community 52"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 54 - "Community 54"
Cohesion: 0.20
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 56 - "Community 56"
Cohesion: 0.13
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 57 - "Community 57"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 58 - "Community 58"
Cohesion: 0.18
Nodes (5): main(), prepare(), foley(), render(), write_pcm()

### Community 60 - "Community 60"
Cohesion: 0.22
Nodes (3): SceneAudioProfile, GoldenSceneProduction, Phase1Balance

### Community 62 - "Community 62"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 63 - "Community 63"
Cohesion: 0.21
Nodes (7): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, KosVisualCapture, VisualTelemetryReport

### Community 64 - "Community 64"
Cohesion: 0.17
Nodes (7): PlaytestReviewValidation, Record, KosCorridorDirector, Busy, CanDepart, KosTravelItem, InteractableId

### Community 65 - "Community 65"
Cohesion: 0.12
Nodes (14): IPhoneChapterContent, CameraLocked, CameraStatus, CanBeginLive, Viewers, PhoneApp, Bank, Call (+6 more)

### Community 66 - "Community 66"
Cohesion: 0.26
Nodes (4): Check, KosSceneValidation, Report, KosVisualV2Tests

### Community 67 - "Community 67"
Cohesion: 0.18
Nodes (3): PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 68 - "Community 68"
Cohesion: 0.19
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 69 - "Community 69"
Cohesion: 0.18
Nodes (4): Phase1Anchor, Phase1World, Phase1WorldItem, InteractableId

### Community 72 - "Community 72"
Cohesion: 0.17
Nodes (12): Phase1Stage, Arrival, Boss, Complete, Epilogue, Investigation, Kuntilanak, Lift (+4 more)

### Community 73 - "Community 73"
Cohesion: 0.17
Nodes (12): Run, Breaking, Complete, Ending, EscapeReady, FinalFlash, GateRun, Idle (+4 more)

### Community 75 - "Community 75"
Cohesion: 0.24
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 76 - "Community 76"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 77 - "Community 77"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 78 - "Community 78"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 82 - "Community 82"
Cohesion: 0.39
Nodes (3): HingedDoor, InteractableId, IsOpen

### Community 83 - "Community 83"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 84 - "Community 84"
Cohesion: 0.36
Nodes (4): box(), cylinder(), finish(), tube()

### Community 89 - "Community 89"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 90 - "Community 90"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 91 - "Community 91"
Cohesion: 0.33
Nodes (6): EndingType, Bad, None, Normal, Secret, True

### Community 93 - "Community 93"
Cohesion: 0.40
Nodes (5): Phase1Challenge, Expired, Offered, Succeeded, Unoffered

### Community 94 - "Community 94"
Cohesion: 0.40
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 96 - "Community 96"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 97 - "Community 97"
Cohesion: 0.40
Nodes (5): KuntiState, Dormant, Manifested, Pursuit, Resolved

## Knowledge Gaps
- **316 isolated node(s):** `AwaitingBlitz`, `AwaitingPhotos`, `Campaign`, `FollowsPlayerInBasement`, `Hud` (+311 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 586 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 14` to `Community 0`, `Community 1`, `Community 3`, `Community 4`, `Community 5`, `Community 6`, `Community 7`, `Community 8`, `Community 10`, `Community 11`, `Community 15`, `Community 16`, `Community 17`, `Community 18`, `Community 20`, `Community 22`, `Community 25`, `Community 26`, `Community 27`, `Community 30`, `Community 38`, `Community 40`, `Community 41`, `Community 42`, `Community 53`, `Community 54`, `Community 64`, `Community 66`, `Community 79`, `Community 86`, `Community 88`?**
  _High betweenness centrality (0.246) - this node is a cross-community bridge._
- **What connects `AwaitingBlitz`, `AwaitingPhotos`, `Campaign` to the rest of the system?**
  _316 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.06131989557403922 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 37` to `Community 0`, `Community 38`, `Community 7`, `Community 6`, `Community 48`, `Community 17`, `Community 22`, `Community 59`, `Community 60`?**
  _High betweenness centrality (0.063) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.10470453241537579 - nodes in this community are weakly interconnected._
- **Why does `Phase1Director` connect `Community 30` to `Community 0`, `Community 33`, `Community 32`, `Community 3`, `Community 36`, `Community 69`, `Community 6`, `Community 66`, `Community 38`, `Community 9`, `Community 10`, `Community 14`, `Community 16`, `Community 81`, `Community 48`, `Community 86`, `Community 25`, `Community 26`?**
  _High betweenness centrality (0.063) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.07236544549977386 - nodes in this community are weakly interconnected._
