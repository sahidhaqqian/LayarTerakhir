# Graph Report - LayarTerakhir (local code only)

## Corpus Check
- cluster-only mode — file stats not available

## Summary
- 1208 nodes · 3986 edges · 51 communities (36 shown, 15 thin omitted)
- Extraction: 97% EXTRACTED · 3% INFERRED · 0% AMBIGUOUS · INFERRED: 128 edges (avg confidence: 0.83)
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `b107da48`
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
- Community 49
- Community 50

## God Nodes (most connected - your core abstractions)
1. `HudController` - 74 edges
2. `AtmajaDirector` - 62 edges
3. `FirstPersonController` - 54 edges
4. `KamarKosPrologDirector` - 53 edges
5. `ReviewPlayerAudit` - 39 edges
6. `AtmajaPhoneView` - 38 edges
7. `FoundationPlayAuditTests` - 37 edges
8. `CampaignState` - 36 edges
9. `PhoneDesktop` - 36 edges
10. `InteractionInteractor` - 35 edges

## Surprising Connections (you probably didn't know these)
- `AtmajaSceneBuilder` --references--> `AtmajaDirector`  [EXTRACTED]
  Assets/_LayarTerakhir/Editor/AtmajaSceneBuilder.cs → Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs
- `GoldenPlayerAudit` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Diagnostics/GoldenPlayerAudit.cs → Assets/_LayarTerakhir/Scripts/UI/HudController.cs
- `ReviewPlayerAudit` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Diagnostics/ReviewPlayerAudit.cs → Assets/_LayarTerakhir/Scripts/UI/HudController.cs
- `AtmajaDirector` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaDirector.cs → Assets/_LayarTerakhir/Scripts/UI/HudController.cs
- `AtmajaPhoneView` --references--> `HudController`  [EXTRACTED]
  Assets/_LayarTerakhir/Scripts/Prolog/AtmajaPhoneView.cs → Assets/_LayarTerakhir/Scripts/UI/HudController.cs

## Import Cycles
- None detected.

## Communities (51 total, 15 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.05
Nodes (15): AtmajaReviewDressing, AtmajaSceneBuilder, Manifest, MaterialRecord, AtmajaSceneValidation, Report, CompactKosPolish, GoldenSceneValidation (+7 more)

### Community 1 - "Community 1"
Cohesion: 0.10
Nodes (12): GoldenLinuxBuild, Record, KosDialogue, LayarTerakhir.Editor, LayarTerakhir.Audio, LayarTerakhir.Editor.Tests, LayarTerakhir.Core, LayarTerakhir.UI (+4 more)

### Community 2 - "Community 2"
Cohesion: 0.06
Nodes (27): KosCorridorBuilder, PlaytestReviewValidation, Record, IInteractable, InteractableId, AtmajaAction, Charge, Clock (+19 more)

### Community 3 - "Community 3"
Cohesion: 0.07
Nodes (15): GoldenAudioTap, GoldenPlayerAudit, Receipt, Receipt, ReviewPlayerAudit, Hud, Phone, View (+7 more)

### Community 4 - "Community 4"
Cohesion: 0.07
Nodes (11): BoundsReport, Diagnosis, Entry, KosRenderDiagnosis, Metric, Check, KosSceneValidation, Report (+3 more)

### Community 5 - "Community 5"
Cohesion: 0.12
Nodes (11): AtmajaDirector, Busy, Campaign, HasSurvey, Hud, LiveConnected, Phone, Stage (+3 more)

### Community 6 - "Community 6"
Cohesion: 0.09
Nodes (23): GameAction, CaptureOrBlitz, Crouch, Interact, ModeAR, ModeEVP, ModeSelfie, ModeThermo (+15 more)

### Community 8 - "Community 8"
Cohesion: 0.15
Nodes (6): PhoneDesktop, CallActive, CurrentApp, DialedContact, Phone, TravelSelection

### Community 9 - "Community 9"
Cohesion: 0.13
Nodes (9): asset(), download(), asset(), fetch(), fetch(), freesound(), polyhaven(), record() (+1 more)

### Community 10 - "Community 10"
Cohesion: 0.12
Nodes (7): BloodWriting, HudController, CurrentDialogueSpeaker, Instance, IsDialoguePlaying, IsHandsetNotification, IsModalOpen

### Community 11 - "Community 11"
Cohesion: 0.16
Nodes (3): KamarKosPrologDirector, KosInteractableItem, InteractableId

### Community 13 - "Community 13"
Cohesion: 0.14
Nodes (7): GoldenSceneProduction, PlaytestRevisionBuilder, VoiceEntry, VoiceManifest, SceneAudioProfile, PresentationProfile, VoiceLine

### Community 14 - "Community 14"
Cohesion: 0.11
Nodes (8): box(), cylinder(), finish(), tube(), box(), cylinder(), material(), tube()

### Community 15 - "Community 15"
Cohesion: 0.11
Nodes (5): WeightOnlyLinear, Info, read(), write(), optimize_model()

### Community 16 - "Community 16"
Cohesion: 0.16
Nodes (9): CampaignData, CampaignState, BackupSaveFilePath, Data, HasValidCheckpoint, Instance, SaveDirectory, SaveFilePath (+1 more)

### Community 18 - "Community 18"
Cohesion: 0.18
Nodes (5): GameShell, AwaitingStart, Instance, IsPaused, MenuSceneLoading

### Community 19 - "Community 19"
Cohesion: 0.15
Nodes (6): KosSceneAudio, CallPanicEvents, DepartingFootstepCount, HasPhoneVibration, OpeningKnockCount, RingtoneStarts

### Community 21 - "Community 21"
Cohesion: 0.20
Nodes (3): GoldenHudBuilder, HudPresentation, HudStyle

### Community 23 - "Community 23"
Cohesion: 0.12
Nodes (4): AtmajaSceneTests, PlaytestReviewTests, PrologSceneFlow, CanContinue

### Community 24 - "Community 24"
Cohesion: 0.30
Nodes (3): PhoneSkin, Rounded, UiWidgets

### Community 26 - "Community 26"
Cohesion: 0.17
Nodes (6): ball(), box(), cloth(), cylinder(), mat(), tube()

### Community 27 - "Community 27"
Cohesion: 0.31
Nodes (8): allowed_source(), check(), digest(), main(), read_json(), snapshot(), update(), validate_graph()

### Community 28 - "Community 28"
Cohesion: 0.20
Nodes (3): foley(), render(), write_pcm()

### Community 29 - "Community 29"
Cohesion: 0.16
Nodes (4): FoundationAuditCoroutineHost, AmbientAudioLoop, LevelMultiplier, ClockAmbience

### Community 30 - "Community 30"
Cohesion: 0.13
Nodes (9): AtmajaPhoneView, Apps, FeedCamera, HasGraphicsDevice, Hud, IsCameraApp, LiveButton, Panel (+1 more)

### Community 31 - "Community 31"
Cohesion: 0.20
Nodes (6): FirstPersonController, IsCrouching, IsSliding, IsSprinting, MovementLocked, PhoneLookHeld

### Community 35 - "Community 35"
Cohesion: 0.20
Nodes (4): UiFocus, BlocksMovement, BlocksWorld, HardModal

### Community 36 - "Community 36"
Cohesion: 0.20
Nodes (10): AtmajaStage, ARRevealed, ClockUnlocked, Courtyard, DamarPhone, Kitchen, Live, SidePassage (+2 more)

### Community 37 - "Community 37"
Cohesion: 0.31
Nodes (3): ClockPuzzleView, Hour, Minute

### Community 38 - "Community 38"
Cohesion: 0.27
Nodes (4): cube(), cyl(), finish(), lathe()

### Community 39 - "Community 39"
Cohesion: 0.31
Nodes (8): checked(), export(), image_path(), material_record(), source_texture(), upstream_image(), visit(), world_bounds()

### Community 40 - "Community 40"
Cohesion: 0.22
Nodes (9): PhoneApp, Bank, Call, Camera, Home, Inventory, Journal, Map (+1 more)

### Community 42 - "Community 42"
Cohesion: 0.36
Nodes (4): box(), cyl(), finish(), tube()

### Community 44 - "Community 44"
Cohesion: 0.33
Nodes (6): EndingType, Bad, None, Normal, Secret, True

### Community 45 - "Community 45"
Cohesion: 0.60
Nodes (3): BuildFileRecord, BuildRecord, WindowsDevelopmentBuild

### Community 49 - "Community 49"
Cohesion: 0.50
Nodes (4): ScanVisualMode, AR, Normal, UV

## Knowledge Gaps
- **133 isolated node(s):** `FloorY`, `CurrentDialogueSpeaker`, `Instance`, `IsDialoguePlaying`, `IsHandsetNotification` (+128 more)
  These have ≤1 connection - possible missing edges. (Counts symbols only; 270 node(s) total have ≤1 connection when file, concept and rationale nodes are included.)
- **15 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `AtmajaDirector` connect `Community 5` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 36`, `Community 34`, `Community 8`, `Community 10`, `Community 13`, `Community 16`, `Community 20`, `Community 22`, `Community 24`, `Community 29`, `Community 30`, `Community 31`?**
  _High betweenness centrality (0.072) - this node is a cross-community bridge._
- **What connects `FloorY`, `CurrentDialogueSpeaker`, `Instance` to the rest of the system?**
  _133 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.05456953642384106 - nodes in this community are weakly interconnected._
- **Why does `HudController` connect `Community 10` to `Community 0`, `Community 33`, `Community 1`, `Community 3`, `Community 5`, `Community 6`, `Community 7`, `Community 13`, `Community 17`, `Community 19`, `Community 21`, `Community 24`, `Community 25`, `Community 29`, `Community 30`?**
  _High betweenness centrality (0.058) - this node is a cross-community bridge._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.10202020202020202 - nodes in this community are weakly interconnected._
- **Why does `FirstPersonController` connect `Community 31` to `Community 0`, `Community 1`, `Community 2`, `Community 3`, `Community 4`, `Community 5`, `Community 7`, `Community 8`, `Community 10`, `Community 11`, `Community 13`, `Community 17`, `Community 18`, `Community 19`, `Community 20`, `Community 24`, `Community 29`, `Community 30`, `Community 33`, `Community 37`, `Community 41`?**
  _High betweenness centrality (0.058) - this node is a cross-community bridge._
- **Should `Community 2` be split into smaller, more focused modules?**
  _Cohesion score 0.05711849957374254 - nodes in this community are weakly interconnected._
