# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1989 nodes · 7014 edges · 78 communities (57 shown, 21 thin omitted)
- Extraction: 95% EXTRACTED · 5% INFERRED · 0% AMBIGUOUS · INFERRED: 352 edges (avg confidence: 0.85)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `c08587bc`
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
- Community 77

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 80 edges
2. `HudController` - 79 edges
3. `AtmajaPlayTests` - 63 edges
4. `LayarTerakhir.Prolog` - 62 edges
5. `AtmajaChaseSequence` - 56 edges
6. `FirstPersonController` - 53 edges
7. `KamarKosPrologDirector` - 51 edges
8. `AtmajaBasementHorror` - 51 edges
9. `AtmajaPhoneView` - 50 edges
10. `LayarTerakhir.Player` - 48 edges

## Surprising Connections (you probably didn't know these)
- `KosSceneAudio` --references--> `SceneAudioProfile`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Audio/KosSceneAudio.cs → Assets/_LayarTerakhir/Scripts/Audio/SceneAudioProfile.cs
- `KosSceneAudio` --references--> `FirstPersonController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Audio/KosSceneAudio.cs → Assets/_LayarTerakhir/Scripts/Player/FirstPersonController.cs
- `HudPresentation` --references--> `KosSceneAudio`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/UI/HudPresentation.cs → Assets/_LayarTerakhir/Scripts/Audio/KosSceneAudio.cs
- `GoldenPlayerAudit` --references--> `KamarKosPrologDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Diagnostics/GoldenPlayerAudit.cs → Assets/_LayarTerakhir/Scripts/Prolog/KamarKosPrologDirector.cs
- `KamarKosPrologDirector` --references--> `KosInteractableItem`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/KamarKosPrologDirector.cs → Assets/_LayarTerakhir/Scripts/Prolog/KosInteractableItem.cs

## Import Cycles
- None detected.

## Communities (78 total, 21 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.08
Nodes (12): GoldenLinuxBuild, Record, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core, LayarTerakhir.UI (+4 more)

### Community 1 - "Community 1"
Cohesion: 0.05
Nodes (13): PrologKosSceneBuilder, PrologMilestoneValidator, AmbientAudioLoop, LevelMultiplier, KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration (+5 more)

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (17): VoiceBank, KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, SceneAudioProfile, PresentationProfile, VoiceLine (+9 more)

### Community 3 - "Community 3"
Cohesion: 0.11
Nodes (5): CompactKosPolish, GoldenSceneValidation, Record, KosVisualV2, FloorY

### Community 4 - "Community 4"
Cohesion: 0.08
Nodes (20): asset(), download(), load_meta(), asset(), download(), asset(), fetch(), dialogue_literals() (+12 more)

### Community 5 - "Community 5"
Cohesion: 0.07
Nodes (12): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+4 more)

### Community 6 - "Community 6"
Cohesion: 0.10
Nodes (9): FoundationPlayAuditTests, FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld, KosInteractableItem (+1 more)

### Community 7 - "Community 7"
Cohesion: 0.08
Nodes (15): AtmajaChaseSequence, AwaitingBlitz, Campaign, FollowsPlayerInBasement, Hud, IsGateRun, IsPursuing, IsStunned (+7 more)

### Community 8 - "Community 8"
Cohesion: 0.12
Nodes (8): CutVertex, DesignerReviewBuilder, MeshRow, MeshSurvey, SurveyItem, SurveyReport, VoiceEntry, ARPhotoGlimmer

### Community 9 - "Community 9"
Cohesion: 0.11
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 10 - "Community 10"
Cohesion: 0.09
Nodes (6): BloodWriting, DesignerWorldRevision, MelatiNeighborhoodBuilder, PlaytestReviewValidation, Record, AtmajaWorldText

### Community 11 - "Community 11"
Cohesion: 0.08
Nodes (24): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+16 more)

### Community 12 - "Community 12"
Cohesion: 0.10
Nodes (8): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsHidden, IsHunting, Phone

### Community 13 - "Community 13"
Cohesion: 0.07
Nodes (6): export(), meshes(), box(), cylinder(), finish(), tube()

### Community 16 - "Community 16"
Cohesion: 0.13
Nodes (13): AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey, Hud, LiveConnected (+5 more)

### Community 17 - "Community 17"
Cohesion: 0.11
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 18 - "Community 18"
Cohesion: 0.13
Nodes (7): AtmajaSynth, AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 19 - "Community 19"
Cohesion: 0.17
Nodes (3): AtmajaReviewDressing, KosVisualUpgrade, VisualAudit

### Community 20 - "Community 20"
Cohesion: 0.17
Nodes (4): Authoring, PrologCompletionBuilder, TempShaftProbe, AtmajaEvidenceAtmosphere

### Community 21 - "Community 21"
Cohesion: 0.17
Nodes (3): AtmajaSceneBuilder, Manifest, MaterialRecord

### Community 23 - "Community 23"
Cohesion: 0.14
Nodes (8): CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath, TempSaveFilePath

### Community 24 - "Community 24"
Cohesion: 0.12
Nodes (22): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+14 more)

### Community 25 - "Community 25"
Cohesion: 0.11
Nodes (10): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds() (+2 more)

### Community 26 - "Community 26"
Cohesion: 0.14
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 27 - "Community 27"
Cohesion: 0.14
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 29 - "Community 29"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 30 - "Community 30"
Cohesion: 0.18
Nodes (4): DesignerReviewPlayerAudit, Hud, Phone, Receipt

### Community 31 - "Community 31"
Cohesion: 0.12
Nodes (12): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, Avatar, CinematicVisible, CurrentDistance, FirstPersonOverride (+4 more)

### Community 32 - "Community 32"
Cohesion: 0.11
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsDocumentOpen, IsHandsetNotification, IsModalOpen

### Community 34 - "Community 34"
Cohesion: 0.15
Nodes (5): Receipt, RiderPresentationAudit, KosCorridorDirector, Busy, CanDepart

### Community 35 - "Community 35"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 36 - "Community 36"
Cohesion: 0.10
Nodes (14): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, DisplayedChatCount, FeedCamera, GlitchStrength, HasGraphicsDevice (+6 more)

### Community 37 - "Community 37"
Cohesion: 0.14
Nodes (3): foley(), render(), write_pcm()

### Community 38 - "Community 38"
Cohesion: 0.18
Nodes (3): AtmajaReviewMoments, Hud, Phone

### Community 40 - "Community 40"
Cohesion: 0.26
Nodes (3): GoldenHudBuilder, HudStyle, StoryMapGraphic

### Community 41 - "Community 41"
Cohesion: 0.13
Nodes (12): CampaignData, EndingType, Bad, None, Normal, Secret, True, PhoneDocument (+4 more)

### Community 43 - "Community 43"
Cohesion: 0.16
Nodes (9): cloth(), ellipsoid(), finish(), mesh(), tube(), cube(), cyl(), finish() (+1 more)

### Community 44 - "Community 44"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 45 - "Community 45"
Cohesion: 0.20
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 46 - "Community 46"
Cohesion: 0.17
Nodes (5): MelatiShop, InteractableId, IsOpen, MelatiShopkeeper, InteractableId

### Community 47 - "Community 47"
Cohesion: 0.26
Nodes (4): AtmajaSceneValidation, Report, KosRoomSurvey, KosRoomCorrections

### Community 49 - "Community 49"
Cohesion: 0.21
Nodes (6): ScanVisualMode, AR, Normal, UV, SmartphoneController, Instance

### Community 50 - "Community 50"
Cohesion: 0.15
Nodes (5): FoundationAuditCoroutineHost, AtmajaThreatPresentation, HasCharacterRig, ClockAmbience, NeighborhoodRain

### Community 54 - "Community 54"
Cohesion: 0.18
Nodes (5): MelatiDeparture, InteractableId, MelatiNeighborhood, Busy, Reached

### Community 58 - "Community 58"
Cohesion: 0.25
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 59 - "Community 59"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 60 - "Community 60"
Cohesion: 0.29
Nodes (3): AnimatedRakaAvatar, Installing, MotorcycleRiderAlignment

### Community 61 - "Community 61"
Cohesion: 0.20
Nodes (10): Run, Breaking, Complete, Ending, GateRun, Idle, Playback, Pursuit (+2 more)

### Community 62 - "Community 62"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 63 - "Community 63"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 64 - "Community 64"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 65 - "Community 65"
Cohesion: 0.33
Nodes (5): ball(), box(), cylinder(), mat(), tube()

### Community 66 - "Community 66"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 67 - "Community 67"
Cohesion: 0.36
Nodes (4): box(), cylinder(), material(), tube()

### Community 68 - "Community 68"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 69 - "Community 69"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 74 - "Community 74"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **233 isolated node(s):** `LevelMultiplier`, `CallPanicEvents`, `DepartingFootstepCount`, `HasPhoneVibration`, `OpeningKnockCount` (+228 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 435 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **21 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaPhoneView` connect `Community 36` to `Community 0`, `Community 1`, `Community 3`, `Community 6`, `Community 9`, `Community 10`, `Community 11`, `Community 14`, `Community 16`, `Community 18`, `Community 21`, `Community 27`, `Community 28`, `Community 30`, `Community 31`, `Community 32`, `Community 37`, `Community 38`, `Community 49`, `Community 50`, `Community 51`, `Community 58`, `Community 75`?**
  _High betweenness centrality (0.238) - this node is a cross-community bridge._
- **What connects `LevelMultiplier`, `CallPanicEvents`, `DepartingFootstepCount` to the rest of the system?**
  _233 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07671561282589152 - nodes in this community are weakly interconnected._
- **Why does `AtmajaDirector` connect `Community 16` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 7`, `Community 10`, `Community 12`, `Community 14`, `Community 18`, `Community 19`, `Community 20`, `Community 21`, `Community 23`, `Community 28`, `Community 30`, `Community 31`, `Community 32`, `Community 33`, `Community 34`, `Community 36`, `Community 38`, `Community 39`, `Community 44`, `Community 49`, `Community 50`, `Community 57`, `Community 58`, `Community 75`?**
  _High betweenness centrality (0.071) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.05300207039337474 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 32` to `Community 0`, `Community 1`, `Community 3`, `Community 6`, `Community 10`, `Community 11`, `Community 12`, `Community 14`, `Community 16`, `Community 18`, `Community 19`, `Community 26`, `Community 28`, `Community 30`, `Community 36`, `Community 38`, `Community 40`, `Community 45`, `Community 49`, `Community 50`, `Community 53`, `Community 70`?**
  _High betweenness centrality (0.059) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.05970149253731343 - nodes in this community are weakly interconnected._
