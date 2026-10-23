# Phone camera presentation revision

Designer reference: `r2:layarterakhir/Layarterakhir/dreadout-referensi/dread9.png`. Viewed locally for composition only; not copied into game assets. Reference SHA-256: `52b987de4eb6e383b080a2c5811e35ae9ee6ee1c9ee249ae424ea8dcfabb4b3b`.

RMB remains a hold action and forces the Camera app while held. A temporary first-person override preserves the selected third-person/first-person setting and independent hiding/cinematic overrides. The view shows a framed landscape handset with a 16:9 feed and two instances of a downloaded CC0 rigged hand source; the second grasp is mirrored by transform. The shared designer body remains in place. Earlier baked grasps produced claw-like fingers or intersections, so the first-person presentation uses para/MakeHuman CC0 FPS arms posed with the source rig. The original topology/UVs are retained. Source provenance and conversion are in `phase1-sourced-hand.json`; no original hand artwork was modeled or painted. Portrait app use, AR/photo/EVP, camera locks, and battery costs remain on the same handset implementation.

Use native captures and current regression receipts to assess the result. Mirroring an existing grasp is not a claim of matching the anatomy or graphic quality of the reference.

The existing handset mesh is reused as a colorless depth mask for the grasp presentation. This keeps fingers behind the handset hidden and front fingers in front of the bezel. `PhoneGripDepth.shader` is rendering code that writes no color, not a newly modeled/drawn visual asset. A transparent grip render preserves the existing handset app/AR/shutter UI.
