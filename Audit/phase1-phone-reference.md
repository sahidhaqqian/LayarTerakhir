# Phone camera presentation revision

Designer reference: `r2:layarterakhir/Layarterakhir/dreadout-referensi/dread9.png`. Viewed locally for composition only; not copied into game assets. Reference SHA-256: `52b987de4eb6e383b080a2c5811e35ae9ee6ee1c9ee249ae424ea8dcfabb4b3b`.

RMB remains a hold action and forces the Camera app while held. A temporary first-person override preserves the selected third-person/first-person setting and independent hiding/cinematic overrides. The view shows a framed landscape handset with a 16:9 feed and two grasps baked from the existing clothed CC0 Raka rig; the second grasp is mirrored by transform. The shared designer body remains in place. The previous FPS-arm pose and an attempted designer-VRM hand conversion produced unsuitable finger presentation. The current foreground grasp reuses the existing clothed CC0 Raka geometry and materials. The shared offline geometry fitter calibrates contacts; no new runtime IK, mesh topology or hand artwork is introduced. Provenance, source checksum, geometry tolerances and conversion are in `phase1-sourced-hand.json`. Portrait app use, AR/photo/EVP, camera locks, and battery costs remain on the same handset implementation.

Use native captures and current regression receipts to assess the result. Mirroring an existing grasp is not a claim of matching the anatomy or graphic quality of the reference.

The existing handset mesh is reused as a colorless depth mask for the grasp presentation. This keeps fingers behind the handset hidden and front fingers in front of the bezel. `PhoneGripDepth.shader` is rendering code that writes no color, not a newly modeled/drawn visual asset. A transparent grip render preserves the existing handset app/AR/shutter UI.

Current physical framing: 165 mm width, 1.9 handset aspect, 8 mm depth. A stowed handset raises directly in landscape; world/save notifications move above the frame. Portrait apps keep the earlier portrait grasp. See `phone-camera-presentation-revision.md` for current verification.
