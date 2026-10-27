# Rumah Atmaja geometry repair — 2026-10-27

The unchanged 350,000-triangle validator now passes at **347,435 triangles**, down from **376,235** (28,800 fewer; 2,565 below the exclusive limit). No validation budget or counting rule was changed.

| Contribution | Before | After |
| --- | ---: | ---: |
| Static scene meshes, every instance including inactive objects | 258,222 | 258,222 |
| Scene skinned hunter | 21,999 | 21,999 |
| Runtime Raka body | 96,000 | 67,200 |
| Runtime Raka phone and screen | 14 | 14 |
| **Total** | **376,235** | **347,435** |

## Investigation and change

`AtmajaSceneValidation.Validate` sums `GetIndexCount(submesh) / 3` for every `MeshFilter` under `Atmaja_Environment_Art`, every `SkinnedMeshRenderer` under that root, and one `Resources/RakaCharacter` resource. Inactive meshes remain counted. The new read-only `AtmajaGeometryAudit.Run` mirrors those rules and records each instance in [the before inventory](atmaja-geometry-before.json).

Raka contains one body, one 12-triangle handset, and one 2-triangle screen: no duplicated character instance. Its 11 coincident triangle pairs cannot explain the 26,236-triangle overage. The scene audit found only two coincident gate-stile instances belonging to separate moving gate leaves; they were preserved. Other major contributions include the 21,999-triangle hunter, nine wood chairs totaling 19,440, 128 small procedural round meshes totaling 17,920, and the scooter totaling 17,904. Shared meshes count per placed instance, as before.

The cause was the existing 96,000-triangle Raka derivative. The repair reduces lower-detail clothing/body topology while protecting the head/neck, hands/fingers, and feet. It retains **39,737 protected vertex positions and 52,793 complete protected faces**, including their UVs, normals, and skin weights.

`Tools/optimize_raka_geometry.py` transplants only body geometry and its skin-cluster index/weight arrays into the original FBX. It verifies that **11,024 other FBX object records**, including **10,784 animation records**, remain unchanged. Original object IDs, connections, bind transforms, phone geometry, and materials are retained. The retarget authoring tool now applies the same protected reduction so a future regeneration retains the budget fix.

## Preservation and verification

- [Optimization receipt](raka-geometry-optimization.json): body counts, protected data, preserved animation/bind records, original and resulting SHA-256.
- [Pose/surface measurements](raka-geometry-visual-verification.json): all 16 clips at 80 sampled phases; protected animated surface difference **0.0 m**. Body bounds change at most **0.231 mm**. Rest-surface displacement averages **0.544 mm RMS**, with 99% below **2.164 mm**; the largest local clothing-surface difference is **6.439 mm**.
- [Asset preservation](atmaja-geometry-preservation.json): 13,115 of 13,116 original asset files remain byte-identical. Only the Raka FBX changed. All original `.meta`, scene, material, texture, and gameplay-code files are unchanged.
- Baseline rerun: **0/1 passed**, reproducing the original triangle-budget failure.
- First validation after the repair: **1/1 passed**, exit code 0; report: [Atmaja scene validation](atmaja-scene-validation.json).
- Python syntax checks passed for both changed authoring tools.
- Graphify update and freshness/source-scope check passed: **365 code files, 4,449 nodes, 16,888 edges, zero asset files**.

Full regression remains running in a persistent background service, at the user's instruction to commit/push immediately without waiting. Results are not claimed as passed. The invocation record and persistent log directory identify the continuing run.

Full regression evidence is recorded in `Audit/atmaja-geometry-regression-invocation.json` and `/root/unity-logs/atmaja-geometry-20261027/regression.xml`. Persistent Unity and offline verification logs remain in `/root/unity-logs/atmaja-geometry-20261027/`.

## Delivery scope

The user's later instruction cancels Windows building and R2 uploading for this revision. No new Windows build or ZIP was produced and no R2 upload was run. The existing ZIP is preserved. Delivery is tests, Git commit, and push to `main`.
