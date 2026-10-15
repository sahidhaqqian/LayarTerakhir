# Dependencies on the approved Scene 2 collection

This recovery drop REUSES the following CC0 assets that were already approved in
`Scene2_Atmaja_Assets/` (they are referenced, not duplicated where avoidable):

| Used in | Approved asset | Path in Scene2_Atmaja_Assets | License |
| --- | --- | --- | --- |
| House materials | worn_mossy_plasterwall, grey_plaster_03, damaged_plaster, brick_moss_001, concrete_moss, clay_roof_tiles_02, clay_roof_tiles_03, wood_planks_grey, wood_peeling_paint_weathered, rusty_metal_sheet | `Architecture/<id>/` | CC0-1.0 |
| Frangipani root plate | root_cluster_01 | `Vegetation/root_cluster_01/` | CC0-1.0 |

Working copies of the textures needed to rebuild the house ship inside
`01_House/Textures/` (byte-identical copies plus two generated variants:
`clay_roof_tiles_02_diff_1k_weathered.jpg` and `WoodTrimSheet_BaseColor_Teak.png`).
`03_Frangipani_Tree/Sources/` keeps the one new download (`dead_tree_trunk_02`)
plus its textures, so the tree can be rebuilt without touching the old drop.

Nothing in `Scene2_Atmaja_Assets/` was modified or deleted by this task.
