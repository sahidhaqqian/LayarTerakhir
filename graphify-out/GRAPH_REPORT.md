# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 2178 nodes · 7667 edges · 86 communities (59 shown, 27 thin omitted)
- Extraction: 96% EXTRACTED · 4% INFERRED · 0% AMBIGUOUS · INFERRED: 341 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `68856d55`
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
- Community 85

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 108 edges
2. `HudController` - 77 edges
3. `AtmajaPlayTests` - 74 edges
4. `AtmajaPhoneView` - 69 edges
5. `ShoulderCameraRig` - 66 edges
6. `AtmajaChaseSequence` - 64 edges
7. `LayarTerakhir.Prolog` - 61 edges
8. `AtmajaBasementHorror` - 57 edges
9. `LayarTerakhir.Player` - 55 edges
10. `KamarKosPrologDirector` - 48 edges

## Surprising Connections (you probably didn't know these)
- `AtmajaMirrorReflection` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Prolog/AtmajaMirrorReflection.cs → Assets/_LayarTerakhir/src/Prolog/AtmajaDirector.cs
- `KosVisualCapture` --references--> `InteractionInteractor`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Editor/KosVisualCapture.cs → Assets/_LayarTerakhir/src/Player/InteractionInteractor.cs
- `DesignerReviewPlayerAudit` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Diagnostics/DesignerReviewPlayerAudit.cs → Assets/_LayarTerakhir/src/UI/HudController.cs
- `GoldenPlayerAudit` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Diagnostics/GoldenPlayerAudit.cs → Assets/_LayarTerakhir/src/UI/HudController.cs
- `PrologCompletionAudit` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/src/Diagnostics/PrologCompletionAudit.cs → Assets/_LayarTerakhir/src/UI/HudController.cs

## Import Cycles
- None detected.

## Communities (86 total, 27 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (13): GoldenLinuxBuild, Record, TempShaftProbe, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core (+5 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (15): DreadoutVisualRevision, Report, SceneReport, BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric (+7 more)

### Community 3 - "Community 3"
Cohesion: 0.05
Nodes (27): FootstepSurface, Earth, Stone, Tile, Wood, PlayerFootsteps, LastSurface, PlayedSteps (+19 more)

### Community 4 - "Community 4"
Cohesion: 0.06
Nodes (19): CampaignData, EndingType, Bad, None, Normal, Secret, True, PhoneDocument (+11 more)

### Community 5 - "Community 5"
Cohesion: 0.08
Nodes (12): AtmajaChaseSequence, AwaitingBlitz, AwaitingPhotos, Campaign, FollowsPlayerInBasement, Hud, IsGateRun, IsPursuing (+4 more)

### Community 6 - "Community 6"
Cohesion: 0.11
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 7 - "Community 7"
Cohesion: 0.10
Nodes (21): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), fetch() (+13 more)

### Community 8 - "Community 8"
Cohesion: 0.07
Nodes (29): DesignerReviewPlayerAudit, Hud, Phone, Receipt, AtmajaAction, BathCurtain, BathScratches, CarpetLift (+21 more)

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (9): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath (+1 more)

### Community 10 - "Community 10"
Cohesion: 0.09
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 11 - "Community 11"
Cohesion: 0.14
Nodes (7): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry

### Community 12 - "Community 12"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 15 - "Community 15"
Cohesion: 0.06
Nodes (11): ball(), box(), cylinder(), mat(), tube(), box(), cylinder(), finish() (+3 more)

### Community 16 - "Community 16"
Cohesion: 0.09
Nodes (9): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsBlitzStunned, IsHidden, IsHunting (+1 more)

### Community 17 - "Community 17"
Cohesion: 0.13
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 18 - "Community 18"
Cohesion: 0.12
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 19 - "Community 19"
Cohesion: 0.10
Nodes (5): MelatiNeighborhoodBuilder, MelatiResidenceFactory, MelatiStreetRevision, Report, ARPhotoGlimmer

### Community 20 - "Community 20"
Cohesion: 0.18
Nodes (5): GoldenHudBuilder, MotorcycleRiderAlignment, HudPresentation, HudStyle, UiWidgets

### Community 21 - "Community 21"
Cohesion: 0.07
Nodes (16): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, GlitchStrength, HasGraphicsDevice (+8 more)

### Community 22 - "Community 22"
Cohesion: 0.11
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 23 - "Community 23"
Cohesion: 0.16
Nodes (4): AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 24 - "Community 24"
Cohesion: 0.16
Nodes (4): Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere, NeighborhoodRain

### Community 25 - "Community 25"
Cohesion: 0.12
Nodes (23): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+15 more)

### Community 26 - "Community 26"
Cohesion: 0.14
Nodes (16): earth(), highpass(), lowpass(), main(), normalise(), read(), stone(), windowed() (+8 more)

### Community 30 - "Community 30"
Cohesion: 0.13
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 32 - "Community 32"
Cohesion: 0.12
Nodes (8): SceneAudioProfile, PresentationProfile, VoiceLine, VoiceBank, GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest

### Community 33 - "Community 33"
Cohesion: 0.11
Nodes (13): cloth(), ellipsoid(), finish(), mesh(), tube(), ball(), box(), cloth() (+5 more)

### Community 34 - "Community 34"
Cohesion: 0.16
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 35 - "Community 35"
Cohesion: 0.11
Nodes (5): ExplorationCommentary, Zone, KosCorridorDirector, Busy, CanDepart

### Community 36 - "Community 36"
Cohesion: 0.17
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 37 - "Community 37"
Cohesion: 0.13
Nodes (6): IInteractable, InteractableId, InteractionInteractor, CurrentTarget, MelatiShopkeeper, InteractableId

### Community 38 - "Community 38"
Cohesion: 0.13
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 39 - "Community 39"
Cohesion: 0.16
Nodes (4): BloodWriting, PrologPresentationBuilder, Receipt, AtmajaWorldText

### Community 41 - "Community 41"
Cohesion: 0.19
Nodes (3): PlayerFeelAudit, Receipt, Sample

### Community 47 - "Community 47"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 49 - "Community 49"
Cohesion: 0.15
Nodes (5): PrologFinaleView, ContractRead, IsCompletionVisible, IsContractVisible, WantsPointer

### Community 50 - "Community 50"
Cohesion: 0.15
Nodes (8): FirstPersonController, CrouchAmount, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld, PlanarVelocity

### Community 51 - "Community 51"
Cohesion: 0.20
Nodes (5): dialogue_literals(), export(), opening_structure_sha256(), verify_opening_source(), OpeningPresentationIdentityTests

### Community 52 - "Community 52"
Cohesion: 0.18
Nodes (5): AmbientAudioLoop, LevelMultiplier, HingedDoor, InteractableId, IsOpen

### Community 53 - "Community 53"
Cohesion: 0.16
Nodes (6): ScanVisualMode, AR, Normal, UV, SmartphoneController, Instance

### Community 54 - "Community 54"
Cohesion: 0.23
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 55 - "Community 55"
Cohesion: 0.19
Nodes (4): AtmajaSceneValidation, OpeningBaseline, Report, AtmajaSceneTests

### Community 61 - "Community 61"
Cohesion: 0.18
Nodes (11): Run, Breaking, Complete, Ending, EscapeReady, GateRun, Idle, Playback (+3 more)

### Community 62 - "Community 62"
Cohesion: 0.22
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 63 - "Community 63"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 64 - "Community 64"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 65 - "Community 65"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 66 - "Community 66"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 67 - "Community 67"
Cohesion: 0.25
Nodes (4): GoldenAudioTap, FoundationAuditCoroutineHost, DoubleLeafGate, IsOpen

### Community 69 - "Community 69"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 71 - "Community 71"
Cohesion: 0.29
Nodes (3): PlaytestReviewValidation, Record, ClockAmbience

### Community 72 - "Community 72"
Cohesion: 0.29
Nodes (3): AtmajaThreatPresentation, ExternalStun, HasCharacterRig

### Community 73 - "Community 73"
Cohesion: 0.29
Nodes (3): MelatiShop, InteractableId, IsOpen

### Community 74 - "Community 74"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 75 - "Community 75"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 77 - "Community 77"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 78 - "Community 78"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 80 - "Community 80"
Cohesion: 0.40
Nodes (3): GoldenSceneValidation, Record, GoldenSceneTests

### Community 83 - "Community 83"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **251 isolated node(s):** `IsPlaying`, `Progress`, `CurrentDialogueSpeaker`, `CurrentObjective`, `Instance` (+246 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 467 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **27 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 21` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 6`, `Community 8`, `Community 10`, `Community 12`, `Community 14`, `Community 18`, `Community 19`, `Community 20`, `Community 23`, `Community 27`, `Community 28`, `Community 31`, `Community 33`, `Community 35`, `Community 38`, `Community 40`, `Community 44`, `Community 49`, `Community 52`, `Community 53`, `Community 54`, `Community 67`, `Community 79`?**
  _High betweenness centrality (0.260) - this node is a cross-community bridge._
- **What connects `IsPlaying`, `Progress`, `CurrentDialogueSpeaker` to the rest of the system?**
  _251 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07579418344519015 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 6` to `Community 0`, `Community 1`, `Community 2`, `Community 5`, `Community 8`, `Community 9`, `Community 10`, `Community 11`, `Community 14`, `Community 16`, `Community 18`, `Community 20`, `Community 21`, `Community 22`, `Community 23`, `Community 24`, `Community 27`, `Community 28`, `Community 31`, `Community 35`, `Community 40`, `Community 44`, `Community 47`, `Community 52`, `Community 53`, `Community 54`, `Community 57`, `Community 67`, `Community 71`, `Community 72`?**
  _High betweenness centrality (0.072) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05834464043419267 - nodes in this community are weakly interconnected._
- **Why does `PlayerFootsteps` connect `Community 3` to `Community 0`, `Community 33`, `Community 35`, `Community 67`, `Community 18`, `Community 52`, `Community 27`, `Community 31`?**
  _High betweenness centrality (0.065) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.10847457627118644 - nodes in this community are weakly interconnected._
