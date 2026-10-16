# Windows Golden reference review

Extract the whole Windows folder and launch LayarTerakhir.exe. Startup is Prolog_KamarKos.
Review candidate only: the headless host cannot approve Windows visuals, listening balance or Iris Xe performance.
Current composition: a 5 × 3.8 m Indonesian city kos, with sleep/work/streaming in one room,
a folded portable green screen stored beside the desk, budget webcam, and intentional everyday clutter/wear.

Manual controls: WASD, mouse, E interact, Tab phone, F screen, Ctrl crouch, Shift sprint,
B powerbank. Follow the letter, automatic Damar call, calendar/sticky-note deduction,
four-wheel date lock, visible desktop motor key, powerbank and hanging jacket, then the room door.
The room door leads to floor 2 of the shared kos; departure to Atmaja is only through the yard gate.
F12 writes a real screenshot including HUD to the game's persistent Screenshots directory.
Normal play uses the existing personal save/checkpoint behavior; do not delete an existing save to test.

Optional isolated automated smoke (PowerShell, from extracted Windows directory):

```powershell
.\LayarTerakhir.exe -reviewAudit "$env:TEMP\LayarTerakhir-ReviewedProlog" -logFile "$env:TEMP\LayarTerakhir-ReviewedProlog.log"
```

This flag creates a fresh IsolatedSave-GUID subfolder, never reads/deletes the personal checkpoint,
poses some compact-room views, then uses continuous CharacterController walking to gather the
remaining preparation items, exit the room, descend both stairs and traverse Atmaja. It captures
41 raw screenshots with HUD, records up to 45 seconds of actual listener output, solves real UI
buttons and verifies checkpoint/Continue without touching personal saves. Story timers are accelerated
after the bed wake, so it is not natural dialogue pacing/acting acceptance. The player exits with
code 0 on success / 1 on failure. Inspect player-audit.json. The old `-goldenAudit` is a retired 3-digit diagnostic.
This is not manual movement coverage, a performance benchmark or proof that every screenshot looks correct.
No flag means no audit component, automatic screenshots, WAV recording or save-path override.

Acceptance checklist:

- Traverse bed, desk, storage and door, including crouch/headroom; verify no snagging/floating props.
- Confirm the folded green screen leaves the bed view open, and the phone/drawer remain accessible.
- Inspect the gamepad/clothing on the bed, thermos/notepad on the stool, newspaper/socks and subtle worn floor.
- Check readable dark corners, motivated practical lights, no pink shaders or window glare.
- Read the letter; confirm Damar's call starts automatically and no transcript/play document appears.
- Check messages, Bank balances, item-only Tas and the downloaded map with fictional destination pins.
- Test wrong/right four-wheel combinations; close/cancel repeatedly and confirm world input recovers.
- Check minimal HUD at 1280×720, 1920×1080 and 16:10; no clipped objective/phone controls/subtitles.
- Listen to rain, PC and room tone; no conspicuous loop seams, clipping or overpowering mix.
- Verify phone vibration stops on pickup, modal ducking, knocks, foley, donation and departure fades.
- Confirm loud knocks turn Raka toward the door and the letter only then slides in; a ringtone precedes pickup.
- Listen for Damar's panicked delivery/distress cry, cutoff, message delay and Raka's contract reaction.
- Keep apps open while walking with WASD; hold right mouse to look. Test all three local phone contacts.
- Retrieve wallet/diary with the powerbank; read the diary in Jurnal. Neither grants extra money.
- Try walking toward the stair's inner drop; physical guards must stop it. Inspect the closed room volumes.
- Open the real main building door before entering the porch/yard; inspect the facade looking back.
- Release CTRL while directly below the Atmaja tabletop: stay crouched, never pop onto the table.
- Run both jacket/drawer orders, save/reload checkpoint, phone screen/battery controls.
- Measure frame pacing and CPU/GPU time on i5-1155G7 / Iris Xe / 8 GB. Target ≥30 FPS; not yet claimed.

Raka, Damar and the well whisper use separate neural synthetic voices. The landlady only leaves a letter.
Listen to full lines at normal time scale; ASR diagnostics are not acting-quality approval. Linux/Mesa
captures are useful reference evidence, not Windows rendering or target-hardware approval.
Asset-Credits.txt includes the CC BY 4.0 motorcycle attribution and modification notice.
Barlow-OFL.txt accompanies the bundled fonts. Retain both when redistributing this build.
