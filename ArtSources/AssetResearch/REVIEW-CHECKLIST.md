# Independent review checklist - before any Unity integration

Required by `deepseek.md`: *"Every asset must be independently reviewed before Unity integration."*
This drop deliberately does **not** touch `Assets/`, `ProjectSettings/` or `Packages/`.
Tick every box per asset and record the reviewer name/date; only approved assets may be copied
into `Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/<id>/`.

## Per-asset gate

| # | Check | How |
| --- | --- | --- |
| 1 | Source page opens and still states CC0 | Open the `sourcePage` link from `manifest/ASSET-MANIFEST.md`; compare with `evidence/polyhaven-license.txt` |
| 2 | File integrity | `python3 ArtSources/AssetResearch/tools/verify_sources.py` exits 0 (re-downloads metadata, re-hashes files) |
| 3 | Model opens and is undamaged | Import the FBX into a scratch Unity project (or Blender); compare bounds against `manifest/measured-bounds.json` |
| 4 | Textures resolve | diff/nor_gl/arm JPGs bind; OpenGL (+Y) normal imported as Normal Map; ARM repacked to URP R metalness / G AO / A smoothness |
| 5 | Lightweight enough | Tri count as listed; recommended import: texture preset 512 px, mipmaps on, anisotropy 2, desktop compression |
| 6 | Import hygiene | No lights/cameras/animation; no automatic mesh colliders; low mesh compression; materials authored as URP/Lit, never plugging ARM straight into the metallic slot |
| 7 | Fits the room identity | Modest Indonesian kos styling; no luxury, no randomly decorative props; matches neighbours' PBR look |
| 8 | Placement validity | Rests on a real surface, no floating, simple collider fit to transformed bounds; player route (25 cm capsule) stays clear |
| 9 | Budget | Whole-room target is <=160k visible triangles and 1024 px max bound texture; do not place the full set if it breaks the budget |
| 10 | No gameplay impact | Interaction IDs, puzzle flow, save/load and the departure flow untouched; the drop adds only visual props |
| 11 | Evidence recorded | Review decision + reviewer + date noted in the project audit trail (`Audit/asset-credits.md` follow-up when integrated) |

## Initial review status (researcher pass only)

All 10 remaining assets passed the **source-level** review during research:

- 10/10 fetched with API MD5 match; SHA-256 recorded (`manifest/verification-report.json`).
- 10/10 local previews rendered from the downloaded FBX (`assets/<id>/preview.png`).
- License evidence captured from the original site (`evidence/polyhaven-license.txt`).
- Candidate rejection log present in `manifest/ASSET-MANIFEST.md`.
- Review cleanup 2026-10-15: industrial_pastic_container, retro_multimeter and cassette_player
  were rejected by the asset review and removed from this drop (files, textures, previews);
  verified they were referenced by no production content before deletion.

The in-engine checks (rows 3-10) require Unity and are **not** claimed as done here.
Actual integration remains a separate, reviewed step by the environment owner.

## Reviewer log

| Asset | Reviewer | Date | Decision | Notes |
| --- | --- | --- | --- | --- |
| (fill in) | | | | |
