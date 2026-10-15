# Kamar kos Raka — compact sleep/work/stream polish

Historical first compact pass. The folded backdrop, clearer clutter and newest playtest delivery
supersede this report: see [final refinement](kos-final-refinement.md).

15 October 2026. Implements the latest `goal.md` and the requested young city-streamer setting.
Visual/composition pass only: no new gameplay systems or Adegan 2.

## Room and layout

- Final internal floor: **5.0 × 3.8 m (19 m²)**, ceiling **2.8 m**, measured from scene colliders.
  This supersedes the previous 6 × 4.5 m layout. Furniture keeps its original physical size.
- One modest room serves sleeping, working and streaming: bed against the west wall, workstation
  against the north wall, compact storage on the east side, stool at the bed head.
- A **1.70 × 2.04 × 0.42 m** portable stand/green cloth sits behind the streaming chair.
  A small monitor-clip webcam and existing microphone make the setup legible without luxury equipment.
  The stand and cloth have fitted collision; the right aisle and south bedside route remain connected.
- Visible furniture, physical colliders, interaction roots, cables and positional audio emitters move together.
  Existing interaction IDs, story logic, smartphone, puzzle, saves and departure are retained.

## Lived-in detail

Original static props: socks beside the bed, folded T-shirt on the mattress, creased newspapers on
the floor/by the desk and on the bedside stool. Existing sandals, backpack, containers, basket,
loose papers, cup and charger/power leads are regrouped around plausible daily activities.
Clutter avoids the primary circulation route. No new plot documents or interactive systems.

Floor tiles are less glossy, with six subtle tracked-dust/stain patches combined into one mesh.
Existing damp/stained walls, worn furniture and cloth remain. The existing warm bounce light is
repositioned to make the green cloth readable from the entry/bed side; no extra light is added.

## Verification and performance scope

- Scene checks: **1,147 base checks + 21 production checks, zero failures**; all eight interactables
  reachable from the connected **75 capsule-clear navigation grid cells**. Corrected an inverted
  collision predicate in the layout validator and added an independent clear-cell check.
- Art-root inventory: **101,747 triangles, 207 renderers**, texture import maximum **1024**.
  These are inventory figures, not draw calls or FPS. New props use static meshes/shared URP
  materials; cloth reuses an existing normal map and floor wear reuses an existing stain alpha.
  No simulation, active webcam camera, new textures, new audio or gameplay dependencies.
- Runtime fixtures use the recomposed furniture positions, without bypassing the real interaction raycast.
- Final regression run: **35/35 passed**, including actual enter/exit-PlayMode tests, both
  jacket/drawer orders, runtime movement collision, phone, puzzle and checkpoint behavior.
  Receipt: `golden-tests.xml`, completed invocation `golden-test-invocation.json`.
- Actual Linux standalone smoke: **14/14 checks passed**, **13 fresh raw 640 × 360 screenshots**,
  phone first-hit raycast behind the green screen, scrolling transcript, wrong/right 148 code,
  and isolated departure checkpoint saved/loaded. Reviewed opening, wide, workstation and bedside
  captures in `Visuals/Golden-Linux`. Existing bounce now lights the green cloth rather than leaving
  its back black. Linux build succeeded with zero errors/warnings.
- Capture limitations: Mesa/llvmpipe OpenGL software rendering, posed gameplay camera and explicitly
  accelerated story timers after the opening. Not manual-navigation coverage, normal-timing audio
  acceptance, a Windows runtime test, or evidence of target-hardware FPS. Personal saves untouched.

## Windows delivery

- Unity Windows x86_64 / Mono / Development build **Succeeded**, **0 errors / 0 warnings**.
- ZIP: `Builds/LayarTerakhir-Windows-x86_64-Development.zip`, **114,087,056 bytes (108.8 MiB)**.
  Contains the complete 294-file player folder, credits, font license and review/capture guides.
  All 289 Unity-reported files and 112 scripting assemblies verified; full ZIP CRC checked.
- SHA-256: `a900dd70334b63d9038955f568548dedcd1f6e9e9309799560a011e9c4fadc7b`.
  Receipts: `golden-build-summary.json`, `golden-windows-invocation.json`; full file manifest under
  `Builds/Logs/windows-build-verification.json`. Previous build preserved under `Builds/Previous`.
- Windows manual visual/navigation review and Iris Xe frame pacing remain to be checked on Windows;
  see the bundled `Golden-Review-Guide.txt`. This is not a claim of developer acceptance or ≥30 FPS.
