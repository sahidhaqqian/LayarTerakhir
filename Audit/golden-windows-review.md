# Windows Golden reference review

Extract the whole Windows folder and launch LayarTerakhir.exe. Startup is Prolog_KamarKos.
Review candidate only: the headless host cannot approve Windows visuals, listening balance or Iris Xe performance.
Current composition: a 5 × 3.8 m Indonesian city kos, with sleep/work/streaming in one room,
a folded portable green screen stored beside the desk, budget webcam, and intentional everyday clutter/wear.

Manual controls: WASD, mouse, E interact, Tab phone, F screen, Ctrl crouch, Shift sprint,
B powerbank. Follow the letter, phone, calendar/sticky-note clue (148), drawer/jacket, then door.
F12 writes a real screenshot including HUD to the game's persistent Screenshots directory.
Normal play uses the existing personal save/checkpoint behavior; do not delete an existing save to test.

Optional isolated automated smoke (PowerShell, from extracted Windows directory):

```powershell
.\LayarTerakhir.exe -goldenAudit "$env:TEMP\LayarTerakhir-GoldenReview" -logFile "$env:TEMP\LayarTerakhir-GoldenReview.log"
```

This flag creates a fresh IsolatedSave-GUID subfolder, never reads/deletes the personal checkpoint,
poses the actual gameplay camera, captures 13 raw screenshots with HUD, records 14 seconds of listener
output to WAV, invokes existing item/modal callbacks, solves the actual padlock buttons and verifies
the departure checkpoint. The player exits with code 0 on success / 1 on failure. Inspect player-audit.json.
This is not manual movement coverage, a performance benchmark or proof that every screenshot looks correct.
No flag means no audit component, automatic screenshots, WAV recording or save-path override.

Acceptance checklist:

- Traverse bed, desk, storage and door, including crouch/headroom; verify no snagging/floating props.
- Confirm the folded green screen leaves the bed view open, and the phone/drawer remain accessible.
- Inspect the gamepad/clothing on the bed, thermos/notepad on the stool, newspaper/socks and subtle worn floor.
- Check readable dark corners, motivated practical lights, no pink shaders or window glare.
- Read full letter and scroll the Damar transcript to its final line; confirm modal close and wrong/right codes.
- Check minimal HUD at 1280×720, 1920×1080 and 16:10; no clipped objective/phone controls/subtitles.
- Listen to rain, PC and room tone; no conspicuous loop seams, clipping or overpowering mix.
- Verify phone vibration stops on pickup, modal ducking, knocks, foley, donation and departure fades.
- Run both jacket/drawer orders, save/reload checkpoint, phone screen/battery controls.
- Measure frame pacing and CPU/GPU time on i5-1155G7 / Iris Xe / 8 GB. Target ≥30 FPS; not yet claimed.

Damar's voice note remains the existing text transcript, not a newly recorded performance. Linux/Mesa
captures in the repository are useful reference evidence, not Windows rendering or target-hardware approval.
Asset-Credits.txt includes the CC BY 4.0 motorcycle attribution and modification notice.
Barlow-OFL.txt accompanies the bundled fonts. Retain both when redistributing this build.
