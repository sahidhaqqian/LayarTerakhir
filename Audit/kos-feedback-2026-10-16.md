# Kos feedback revision — 16 October 2026

Status: implemented, freshly tested, built and uploaded to R2. This report supersedes only the Kos/phone/audio
parts of `playtest-fixes-2026-10-16.md`; earlier passing tests are not evidence that these new reports
were already fixed.

## Requested changes

1. Replace formant speech with independently voiced Indonesian neural Raka/Damar/well whisper;
   improve delivery and retain rendered clips for offline playback. Landlady has no dialogue now.
2. Fit the streaming dashboard to the monitor's measured front glass, including its 7.55° tilt.
3. Four-digit date lock, shown as two pairs, with four physical wheels. Deduce the birthday from
   a personal note and August/Independence-Day calendar clue; no document prints the combination.
4. Three real door knocks, a pause, then receding footsteps. No stage-direction subtitle.
5. Phone vibration/red LED without a vibration caption. Ratna only leaves her signed letter.
6. Pickup directly opens the phone call screen and starts Damar; no play-message document/button.
7. Handset proportions/bezel, rounded glass, status strip, original vector app icons, home screen,
   call/timer/history and incoming contract message inside Pesan.
8. Licensed downloaded Jakarta street map, fictional Kos/Atmaja pins and a route overlay; gate-only
   departure remains. Map aspect is preserved and source attribution is visible in the phone.
9. Bank app reads campaign balances and spending, with Indonesian currency formatting. Tas is items only.
10. Physical-object inspection lowers the phone and releases only phone focus. Preparation objective
    lists missing items. Regression walks continuously from drawer to key/jacket/door, not forced Ready.
11. Visible jacket moved to a reachable hook; visible separate motor key on the desktop; drawer gives
    only a powerbank. Runtime text consistently calls it a powerbank.

## Diagnosed causes

- The old jacket was at the far right of the door wall; progress silently required its pickup.
- Phone browsing owns world input, but inspecting/closing world-object modals did not lower it.
  Existing regression helpers manually cleared phone focus, concealing that interaction problem.
- The old dashboard was at world z=1.317; actual inclined glass is around z=1.400, so the image
  visibly hovered. `KosRoomSurvey` measures actual mesh vertices, not only the stand's bounding box.
- The old drawer awarded both keys and powerbank, while its UI/model still had three wheels.

All room corrections are scoped runtime/additive changes: the approved `.unity` scene is not rebuilt
or rewritten. Original GUIDs, prefab references, floor plan, existing art and previous saves are retained.

## Verification

Fresh verification:

- **55/55** Unity tests passed, no skipped/failed cases: `golden-tests.xml` and
  `golden-test-invocation.json`. Tests cover both item orders, actual first-hit interaction,
  continuous drawer→desktop key→jacket→door walking, four-wheel callbacks, no play-message modal,
  bank/item separation and recovery of world input after the drawer closes.
- Actual standalone Linux/Mesa run completed with **35 raw 640×360 screenshots**, real menu,
  bed wake, automatic Damar call, messages/Bank/Tas, monitor, jacket, four wheels, corridor/stairs,
  physical gate crossing and Atmaja AR/clock/Continue regressions. See
  `Visuals/Review-Linux/player-audit.json` and `review-player-invocation.json`.
  All 152 checks passed over 122.61 m of movement; maximum interior foot height was 0.592 m.
  The actual opening listener mix records 41.32 seconds with peak 0.518, without clipping.
  Story timers are accelerated after wake; this is not natural VO pacing or hardware acceptance.
- Three separate original synthetic voice samples plus **15 current dialogue clips**,
  **105.159 seconds** total. Raw-clip maximum peak 0.710785; no formant/eSpeak voice is referenced
  by the current profile and no landlady speech exists in it. ASR diagnostics in
  `voice-sample-asr.json` / `neural-voice-asr.json` were used to improve a few short lines;
  slang/name/whisper recognition is imperfect and is not an acting-quality pass.
- Windows x86_64 Mono Development build succeeded; complete 300-file ZIP passes CRC/completeness
  checks. `Builds/LayarTerakhir-Windows-x86_64-Development.zip`, **158,521,886 bytes**.
  SHA-256: `023615ae2ae5890a9da65aa2c3e81b6fa145055c6625672d0c08907386da6286`.
- Uploaded exactly that ZIP to `r2:layarterakhir/Layarterakhir/`; **full remote readback hash
  equals the local hash**, recorded in `r2-windows-build-upload.json`. A recoverable R2 501 on
  the first upload/check attempt was followed by a successful rclone retry; readback verified bytes.
  Prior Windows output/ZIP is preserved under `Builds/Previous/before-golden-20261016-070419/`.
- Golden native Kos `.unity` SHA remains
  `4b73b117b832fb753b92d3de885c163bc1dd3f6225101867f7b4e2437c8b793a`.
  Automatic unrelated URP serialization noise was restored after all Editor builds finished.

Use NEW GAME to see the revised intro/puzzle; existing checkpoints remain compatible and the
previous save is archived, not deleted. No personal checkpoint was used by tests/audits.

Voice/model/map sources and licensing decisions: `kos-feedback-source-review.md`.
Actual Windows/Iris Xe movement, subjective listening/acting quality and hardware frame pacing
require manual acceptance on the target machine.
