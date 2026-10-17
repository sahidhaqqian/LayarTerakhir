# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1734 nodes · 5912 edges · 77 communities (53 shown, 24 thin omitted)
- Extraction: 96% EXTRACTED · 4% INFERRED · 0% AMBIGUOUS · INFERRED: 257 edges (avg confidence: 0.84)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `cbc10371`
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
- Community 68
- Community 69
- Community 70
- Community 71
- Community 72
- Community 73
- Community 74

## God Nodes (most connected - your core abstractions)
1. `AtmajaDirector` - 78 edges
2. `HudController` - 75 edges
3. `AtmajaChaseSequence` - 56 edges
4. `FirstPersonController` - 53 edges
5. `KamarKosPrologDirector` - 53 edges
6. `AtmajaBasementHorror` - 50 edges
7. `LayarTerakhir.Prolog` - 50 edges
8. `ShoulderCameraRig` - 48 edges
9. `AtmajaPlayTests` - 43 edges
10. `LayarTerakhir.Player` - 43 edges

## Surprising Connections (you probably didn't know these)
- `GameShell` --references--> `GameAction`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/UI/GameShell.cs → Assets/_LayarTerakhir/Scripts/Core/GameInputConfig.cs
- `AtmajaBasementHorror` --references--> `CampaignState`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Core/CampaignState.cs
- `AtmajaBasementHorror` --references--> `AtmajaChaseSequence`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaChaseSequence.cs
- `AtmajaBasementHorror` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs
- `AtmajaBasementHorror` --references--> `AtmajaUpperDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaBasementHorror.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaUpperDirector.cs

## Import Cycles
- None detected.

## Communities (77 total, 24 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.07
Nodes (20): GoldenLinuxBuild, Record, GoldenSceneValidation, Record, GoldenSceneTests, TempShaftProbe, ScanVisualMode, AR (+12 more)

### Community 2 - "Community 2"
Cohesion: 0.08
Nodes (14): AtmajaChaseSequence, AwaitingBlitz, Campaign, Hud, IsGateRun, IsPursuing, IsStunned, Phone (+6 more)

### Community 3 - "Community 3"
Cohesion: 0.11
Nodes (15): InteractionInteractor, CurrentTarget, AtmajaDirector, Busy, Campaign, CanBeginLive, CheckpointRestores, HasSurvey (+7 more)

### Community 4 - "Community 4"
Cohesion: 0.13
Nodes (3): FoundationPlayAuditTests, SmartphoneController, Instance

### Community 5 - "Community 5"
Cohesion: 0.09
Nodes (8): HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsHandsetNotification, IsModalOpen, HudPresentation

### Community 6 - "Community 6"
Cohesion: 0.13
Nodes (5): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, DesignerWorldRevision

### Community 7 - "Community 7"
Cohesion: 0.11
Nodes (10): KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, SceneAudioProfile, PresentationProfile, VoiceLine, HingedDoor (+2 more)

### Community 8 - "Community 8"
Cohesion: 0.12
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 9 - "Community 9"
Cohesion: 0.14
Nodes (5): PlaytestReviewValidation, Record, Authoring, PrologCompletionBuilder, AtmajaEvidenceAtmosphere

### Community 12 - "Community 12"
Cohesion: 0.11
Nodes (7): DisplayPreferences, Isolated, GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 13 - "Community 13"
Cohesion: 0.12
Nodes (5): PrologCompletionAudit, Campaign, Hud, Phone, Receipt

### Community 14 - "Community 14"
Cohesion: 0.12
Nodes (8): AtmajaBasementHorror, Campaign, Detection, Hud, InspectingLocker, IsHidden, IsHunting, Phone

### Community 15 - "Community 15"
Cohesion: 0.12
Nodes (7): PrologPresentationBuilder, Receipt, AnimatedRakaAvatar, AtmajaThreatPresentation, HasCharacterRig, PhoneGlyph, StoryMapGraphic

### Community 16 - "Community 16"
Cohesion: 0.12
Nodes (12): asset(), download(), asset(), fetch(), fetch(), main(), sha(), fetch() (+4 more)

### Community 18 - "Community 18"
Cohesion: 0.14
Nodes (9): CampaignData, CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath (+1 more)

### Community 19 - "Community 19"
Cohesion: 0.10
Nodes (9): FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld, KosCorridorDirector, Busy (+1 more)

### Community 20 - "Community 20"
Cohesion: 0.17
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 21 - "Community 21"
Cohesion: 0.17
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (4): box(), cyl(), finish(), tube()

### Community 23 - "Community 23"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 24 - "Community 24"
Cohesion: 0.20
Nodes (6): AtmajaUpperDirector, ArActive, Campaign, ChainsBroken, Hud, Phone

### Community 25 - "Community 25"
Cohesion: 0.16
Nodes (20): actions(), add_armature(), at(), joint(), mapped(), asset_mesh(), copy_texture(), create_mesh() (+12 more)

### Community 27 - "Community 27"
Cohesion: 0.09
Nodes (23): AtmajaAction, BathCurtain, BathScratches, CarpetLift, Charge, Clock, ClockHands, CupPhoto (+15 more)

### Community 28 - "Community 28"
Cohesion: 0.12
Nodes (3): foley(), render(), write_pcm()

### Community 29 - "Community 29"
Cohesion: 0.14
Nodes (4): DesignerRevisionTests, KosVisualV2Tests, DoubleLeafGate, IsOpen

### Community 30 - "Community 30"
Cohesion: 0.09
Nodes (22): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+14 more)

### Community 31 - "Community 31"
Cohesion: 0.19
Nodes (4): BloodWriting, GoldenHudBuilder, AtmajaWorldText, HudStyle

### Community 32 - "Community 32"
Cohesion: 0.14
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 33 - "Community 33"
Cohesion: 0.15
Nodes (6): IInteractable, InteractableId, AtmajaItem, InteractableId, KosTravelItem, InteractableId

### Community 36 - "Community 36"
Cohesion: 0.17
Nodes (5): asset(), download(), load_meta(), dialogue_literals(), export()

### Community 38 - "Community 38"
Cohesion: 0.12
Nodes (6): AtmajaSceneValidation, Report, AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 39 - "Community 39"
Cohesion: 0.15
Nodes (12): PlayerPerspective, FirstPerson, OverShoulder, ShoulderCameraRig, Avatar, CinematicVisible, CurrentDistance, FirstPersonOverride (+4 more)

### Community 41 - "Community 41"
Cohesion: 0.16
Nodes (9): cloth(), ellipsoid(), finish(), mesh(), tube(), cube(), cyl(), finish() (+1 more)

### Community 42 - "Community 42"
Cohesion: 0.14
Nodes (8): box(), cylinder(), finish(), tube(), box(), cylinder(), material(), tube()

### Community 45 - "Community 45"
Cohesion: 0.11
Nodes (18): AtmajaStage, ARRevealed, BasementEntry, BasementStealth, Chase, ClockUnlocked, Courtyard, DamarPhone (+10 more)

### Community 48 - "Community 48"
Cohesion: 0.12
Nodes (11): AtmajaPhoneView, Apps, ChatFrozen, CinematicHandsetVisible, FeedCamera, HasGraphicsDevice, Hud, IsCameraApp (+3 more)

### Community 49 - "Community 49"
Cohesion: 0.22
Nodes (3): GoldenAudioTap, GoldenPlayerAudit, Receipt

### Community 51 - "Community 51"
Cohesion: 0.16
Nodes (4): FoundationAuditCoroutineHost, AmbientAudioLoop, LevelMultiplier, ClockAmbience

### Community 55 - "Community 55"
Cohesion: 0.17
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 56 - "Community 56"
Cohesion: 0.23
Nodes (4): AtmajaHandycamPlayback, IsPlaying, Progress, AtmajaMirrorReflection

### Community 57 - "Community 57"
Cohesion: 0.49
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 58 - "Community 58"
Cohesion: 0.42
Nodes (3): Check, KosSceneValidation, Report

### Community 59 - "Community 59"
Cohesion: 0.20
Nodes (10): Run, Breaking, Complete, Ending, GateRun, Idle, Playback, Pursuit (+2 more)

### Community 60 - "Community 60"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 61 - "Community 61"
Cohesion: 0.31
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 62 - "Community 62"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 63 - "Community 63"
Cohesion: 0.33
Nodes (5): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric

### Community 65 - "Community 65"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 66 - "Community 66"
Cohesion: 0.33
Nodes (5): ball(), box(), cylinder(), mat(), tube()

### Community 69 - "Community 69"
Cohesion: 0.29
Nodes (7): Mind, Done, Dormant, Emerge, Patrol, Pursue, Suspicion

### Community 70 - "Community 70"
Cohesion: 0.29
Nodes (7): KosPrologStage, Completed, PreparationAndDrawerPuzzle, ReadyToDepart, WaitPickupSmartphone, WaitReadSuratIbuKos, WakeUpAndKnocks

### Community 71 - "Community 71"
Cohesion: 0.33
Nodes (6): EndingType, Bad, None, Normal, Secret, True

### Community 73 - "Community 73"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **217 isolated node(s):** `AR`, `Normal`, `UV`, `FloorY`, `Isolated` (+212 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 392 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **24 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaDirector` connect `Community 3` to `Community 0`, `Community 1`, `Community 4`, `Community 5`, `Community 6`, `Community 7`, `Community 9`, `Community 11`, `Community 14`, `Community 18`, `Community 19`, `Community 21`, `Community 24`, `Community 29`, `Community 33`, `Community 45`, `Community 47`, `Community 48`, `Community 51`, `Community 54`, `Community 56`, `Community 67`, `Community 74`?**
  _High betweenness centrality (0.072) - this node is a cross-community bridge._
- **What connects `AR`, `Normal`, `UV` to the rest of the system?**
  _217 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.07110833072672297 - nodes in this community are weakly interconnected._
- **Why does `AtmajaChaseSequence` connect `Community 2` to `Community 0`, `Community 1`, `Community 67`, `Community 9`, `Community 10`, `Community 11`, `Community 12`, `Community 14`, `Community 48`, `Community 17`, `Community 51`, `Community 54`, `Community 56`, `Community 59`?**
  _High betweenness centrality (0.050) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.14530612244897959 - nodes in this community are weakly interconnected._
- **Why does `FirstPersonController` connect `Community 19` to `Community 0`, `Community 3`, `Community 4`, `Community 5`, `Community 6`, `Community 7`, `Community 9`, `Community 11`, `Community 20`, `Community 21`, `Community 26`, `Community 32`, `Community 44`, `Community 49`, `Community 50`, `Community 51`, `Community 58`, `Community 60`, `Community 64`?**
  _High betweenness centrality (0.038) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.08163265306122448 - nodes in this community are weakly interconnected._
