# Kos immersion and latest goal revision — 16 October 2026

Status: implemented, Unity-tested, standalone-verified, Windows-built and uploaded/readback-verified.

Scope follows the current `goal.md` plus the explicit corridor/main-door request. This is a focused
quality pass on the playable Kos–Atmaja segment, not a claim that all chapters or Atmaja scenes 4–7 exist.

## Changes

- Incoming call has its own original ringtone, separate from the vibrating phone motor. Pickup stops both.
- Three newly authored forceful wooden knocks interrupt the end of Raka's monologue. Raka turns toward
  the door; the red letter is hidden beforehand and slides underneath after the knocks. Departing steps follow.
- Directed retakes keep separate Raka/Damar identities; correct reference-only synthesis mode actually
  applies emotion/style, unlike the old transcript-continuation mode. Metal scrape, Damar's distress cry,
  glass interference and cutoff are staged in that order. After Raka's failed response, 3.5 seconds elapse before the message notification.
  The requested frightened contract reaction is voiced. Subjective acting acceptance is still pending.
- Telepon has Damar, Ibu and Ratna contacts, fictional masked numbers, and local outgoing tone/failure.
  It never calls real phone networks or invents spoken dialogue for Ibu/Ratna.
- Apps retain the pointer but no longer own movement. WASD remains active; hold right mouse to look.
  Menu, documents, lock puzzle and cinematics still block movement appropriately.
- The locked drawer grants the powerbank, personal diary and wallet once. Diary text is readable in
  Jurnal/Tas; no additional cash, lore collectible, required puzzle or answer giveaway is introduced.
- Standing clearance uses overlap against the entire prospective capsule, filtering self/trigger hulls.
  It detects furniture already overlapping the start of the old sphere cast and prevents tabletop popping.
- Fourteen locked tenant rooms have real floor/ceiling/perimeter volumes. Hall walls have framed door
  openings rather than doors pasted over a continuous wall. Shared rooms remain enterable downstairs.
- Both stair flights have inner masonry guards/handrails, readable step nosings, protected top landing,
  closed outer returns and a full foundation. Ground checkpoint requires actual floor contact. A below-world
  recovery is a last resort, not the primary collision fix.
- A framed, collidable main building door must be physically opened before entering the porch/yard.
  Fasad, pitched roof, porch supports, downpipes, meters, vents, wall paint bands, conduit, slippers,
  notices, plants, drainage and neighbouring boundary masses give the kos architectural continuity.
- Existing offline map, gate-only destination selection, motorcycle departure and Atmaja AR/clock route
  are retained. All art comes from current licensed assets or original native geometry/effects.

## Verification and delivery

Fresh Unity suite: **57/57 passed, zero failed/skipped**, `golden-test-invocation.json` and `golden-tests.xml`.
The first run exposed the legacy stair test walking into a newly sealed bedroom wall and overly eager
auto-crouching near furniture affecting interaction aim. The stair route now turns back into the hallway;
standing clearance is only queried when actually attempting to rise. Both item-order regressions pass.
Scene validation has **zero failures across 766 checks**, including six-direction enclosure raycasts
for every tenant room, real main-door references and packaged ringtone/cry/reaction assets.

The fresh standalone retry completed with **exit 0, 167/167 checks passed and 41 raw 640×360 captures**
(`review-player-invocation.json`, `Visuals/Review-Linux/player-audit.json`). It walked **124.53 m**, including
both stair flights, blocked upper drop, closed/open main entrance, motorcycle through gate, actual-table
CTRL release, AR/clock and final Continue. Maximum interior foot height was **0.592 m**, never tabletop.
Continue restored the reached stage and exact wallet; the previous save was archived byte-for-byte.
Evidence comes from fresh Unity invocations and actual standalone captures, not edited mockups. Linux/Mesa software rendering
does not prove Windows/Iris Xe frame pacing, normal dialogue timing or subjective acting quality.

The approved Golden Kos source scene and the existing Atmaja source scene are not rewritten by this pass.
`PlaytestRevisionBuilder.RefineKos` only rebuilds the corridor and presentation references. User-owned
`goal.md`, instruction-file changes and `Bugs/` remain outside this commit.

Runtime source was checkpointed concurrently by another session as `d060e22`; the frozen scene/runtime
hashes for this delivery are in `kos-immersion-build-sources.json`. Later upper-floor asset downloads/work
from that session are not included in this Windows build, and are left untouched/uncommitted by this pass.

Windows x86_64 Mono Development build: **Succeeded, 0 errors, 0 warnings**. All 294 Unity-reported files
and 112 scripting assemblies verified; the full **301-file ZIP** passes CRC/completeness checks.
Archive size **159,037,952 bytes**, SHA-256
`b021db44f1a5622b963239beb99b0a7847c6a1d88f8f9ab926c669b6104c7026`.
Current neural bank: **17 clips, 116.127 seconds**. The final actual opening listener capture has
peak **0.617**, without clipping; this is not a full normal-paced dialogue listening pass.

The exact ZIP was uploaded to `r2:layarterakhir/Layarterakhir/`; full remote readback SHA-256 and
byte count match the local archive (`r2-windows-build-upload.json`). An R2 501 after transfer was
recovered by rclone's second attempt; successful complete-object readback is the acceptance criterion.

The first 960×540 actual standalone run passed all newly requested interactions and reached the Atmaja
stair checkpoint, but its process group exited 143 before final main-menu/Continue completion. Cause
unconfirmed; it is **not counted as a completed full audit**. Seven higher-resolution screenshots and
the observed checks are preserved under `Visuals/Kos-Immersion-960/capture-metadata.json`.
The full 640×360 retry completed successfully, as recorded above; the interrupted higher-resolution run
is supplementary evidence only, not substituted for that completion receipt.

Visual review of actual captures confirms the new corridor detailing, inner stair guard, reachable main
door and porch/facade. Minor visual follow-up: the inside entrance label can show its back face from
outside (world-text depth/backface handling); this does not block traversal and is not hidden by the audit.
Final Windows/Iris Xe lighting, frame pacing and subjective voice acting remain manual acceptance work.
