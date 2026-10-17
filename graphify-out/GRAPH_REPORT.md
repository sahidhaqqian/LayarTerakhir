# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1286 nodes · 4238 edges · 52 communities (36 shown, 16 thin omitted)
- Extraction: 97% EXTRACTED · 3% INFERRED · 0% AMBIGUOUS · INFERRED: 133 edges (avg confidence: 0.83)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `da358ce0`
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
- Community 50

## God Nodes (most connected - your core abstractions)
1. `HudController` - 75 edges
2. `AtmajaDirector` - 68 edges
3. `FirstPersonController` - 57 edges
4. `KamarKosPrologDirector` - 53 edges
5. `PhoneDesktop` - 42 edges
6. `CampaignState` - 40 edges
7. `ReviewPlayerAudit` - 39 edges
8. `AtmajaPhoneView` - 39 edges
9. `InteractionInteractor` - 38 edges
10. `LayarTerakhir.Prolog` - 38 edges

## Surprising Connections (you probably didn't know these)
- `KosSceneAudio` --references--> `FirstPersonController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Audio/KosSceneAudio.cs → Assets/_LayarTerakhir/Scripts/Player/FirstPersonController.cs
- `HudPresentation` --references--> `KosSceneAudio`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/UI/HudPresentation.cs → Assets/_LayarTerakhir/Scripts/Audio/KosSceneAudio.cs
- `GoldenPlayerAudit` --references--> `KamarKosPrologDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Diagnostics/GoldenPlayerAudit.cs → Assets/_LayarTerakhir/Scripts/Prolog/KamarKosPrologDirector.cs
- `AtmajaPhoneView` --references--> `PhoneDesktop`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaPhoneView.cs → Assets/_LayarTerakhir/Scripts/UI/PhoneDesktop.cs
- `PhoneDesktop` --references--> `SmartphoneController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/UI/PhoneDesktop.cs → Assets/_LayarTerakhir/Scripts/Smartphone/SmartphoneController.cs

## Import Cycles
- None detected.

## Communities (52 total, 16 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.09
Nodes (15): GoldenLinuxBuild, Record, GoldenSceneValidation, Record, GoldenSceneTests, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio (+7 more)

### Community 1 - "Community 1"
Cohesion: 0.06
Nodes (13): AtmajaSceneValidation, Report, AtmajaSceneTests, PlaytestReviewTests, KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration (+5 more)

### Community 2 - "Community 2"
Cohesion: 0.07
Nodes (11): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+3 more)

### Community 3 - "Community 3"
Cohesion: 0.06
Nodes (23): AmbientAudioLoop, LevelMultiplier, IInteractable, InteractableId, AtmajaAction, Charge, Clock, ClockHands (+15 more)

### Community 4 - "Community 4"
Cohesion: 0.15
Nodes (4): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord

### Community 5 - "Community 5"
Cohesion: 0.06
Nodes (15): DevelopmentScreenshotCapture, UiFocus, BlocksMovement, BlocksWorld, HardModal, GoldenAudioTap, GoldenPlayerAudit, Receipt (+7 more)

### Community 6 - "Community 6"
Cohesion: 0.13
Nodes (12): AtmajaDirector, Busy, Campaign, CanBeginLive, HasSurvey, Hud, LiveConnected, Phone (+4 more)

### Community 8 - "Community 8"
Cohesion: 0.11
Nodes (7): KosCorridorBuilder, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, SceneAudioProfile, PresentationProfile, VoiceLine

### Community 9 - "Community 9"
Cohesion: 0.09
Nodes (23): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+15 more)

### Community 10 - "Community 10"
Cohesion: 0.13
Nodes (10): PhoneDesktop, CallActive, ContractReceived, CurrentApp, DialedContact, IsCalling, Phone, TravelSelection (+2 more)

### Community 12 - "Community 12"
Cohesion: 0.15
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 13 - "Community 13"
Cohesion: 0.10
Nodes (8): BloodWriting, HudController, CurrentDialogueSpeaker, CurrentObjective, Instance, IsDialoguePlaying, IsHandsetNotification, IsModalOpen

### Community 14 - "Community 14"
Cohesion: 0.14
Nodes (9): asset(), download(), asset(), fetch(), fetch(), freesound(), polyhaven(), record() (+1 more)

### Community 15 - "Community 15"
Cohesion: 0.10
Nodes (4): export(), foley(), render(), write_pcm()

### Community 16 - "Community 16"
Cohesion: 0.15
Nodes (5): Receipt, ReviewPlayerAudit, Hud, Phone, View

### Community 18 - "Community 18"
Cohesion: 0.11
Nodes (8): box(), cylinder(), finish(), tube(), box(), cylinder(), material(), tube()

### Community 19 - "Community 19"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 21 - "Community 21"
Cohesion: 0.12
Nodes (15): CampaignData, CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath (+7 more)

### Community 22 - "Community 22"
Cohesion: 0.10
Nodes (9): AtmajaPhoneView, Apps, FeedCamera, HasGraphicsDevice, Hud, IsCameraApp, LiveButton, Panel (+1 more)

### Community 23 - "Community 23"
Cohesion: 0.13
Nodes (8): PrologKosSceneBuilder, PrologMilestoneValidator, FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 24 - "Community 24"
Cohesion: 0.12
Nodes (3): DesignerRevisionTests, DoubleLeafGate, IsOpen

### Community 27 - "Community 27"
Cohesion: 0.21
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 30 - "Community 30"
Cohesion: 0.33
Nodes (3): FoundationPlayAuditTests, KosInteractableItem, InteractableId

### Community 31 - "Community 31"
Cohesion: 0.17
Nodes (5): InteractionInteractor, CurrentTarget, KosCorridorDirector, Busy, CanDepart

### Community 32 - "Community 32"
Cohesion: 0.14
Nodes (5): KosRoomSurvey, VisualAudit, PlaytestReviewValidation, Record, ClockAmbience

### Community 34 - "Community 34"
Cohesion: 0.20
Nodes (6): ScanVisualMode, AR, Normal, UV, SmartphoneController, Instance

### Community 35 - "Community 35"
Cohesion: 0.17
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 36 - "Community 36"
Cohesion: 0.19
Nodes (3): ShoulderCameraRig, Avatar, CurrentDistance

### Community 38 - "Community 38"
Cohesion: 0.44
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 41 - "Community 41"
Cohesion: 0.20
Nodes (10): AtmajaStage, ARRevealed, ClockUnlocked, Courtyard, DamarPhone, Kitchen, Live, SidePassage (+2 more)

### Community 42 - "Community 42"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 44 - "Community 44"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 45 - "Community 45"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 46 - "Community 46"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 47 - "Community 47"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 50 - "Community 50"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

## Knowledge Gaps
- **140 isolated node(s):** `CanContinue`, `CallPanicEvents`, `DepartingFootstepCount`, `HasPhoneVibration`, `OpeningKnockCount` (+135 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 281 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **16 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaDirector` connect `Community 6` to `Community 0`, `Community 3`, `Community 4`, `Community 7`, `Community 8`, `Community 11`, `Community 13`, `Community 16`, `Community 21`, `Community 22`, `Community 23`, `Community 24`, `Community 25`, `Community 26`, `Community 29`, `Community 31`, `Community 32`, `Community 34`, `Community 41`, `Community 48`?**
  _High betweenness centrality (0.067) - this node is a cross-community bridge._
- **What connects `CanContinue`, `CallPanicEvents`, `DepartingFootstepCount` to the rest of the system?**
  _140 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.09025389025389026 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 13` to `Community 32`, `Community 0`, `Community 34`, `Community 3`, `Community 5`, `Community 6`, `Community 7`, `Community 8`, `Community 9`, `Community 12`, `Community 16`, `Community 49`, `Community 20`, `Community 22`, `Community 23`, `Community 30`, `Community 31`?**
  _High betweenness centrality (0.061) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.059395801331285206 - nodes in this community are weakly interconnected._
- **Why does `GameShell` connect `Community 27` to `Community 0`, `Community 5`, `Community 7`, `Community 9`, `Community 49`, `Community 22`, `Community 31`?**
  _High betweenness centrality (0.056) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.07180851063829788 - nodes in this community are weakly interconnected._
