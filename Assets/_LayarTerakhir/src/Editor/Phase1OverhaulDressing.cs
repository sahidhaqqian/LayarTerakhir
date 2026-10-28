#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    public static partial class Phase1SceneBuilder
    {
        [Serializable] private sealed class OverhaulPlacement
        {
            public string id, role, area, source;
            public Vector3 supportPoint, centre, size;
            public bool physicalCollider;
        }
        [Serializable] private sealed class OverhaulReceipt
        {
            public int minimumDistinctModels = 100, distinctModels;
            public string utc;
            public OverhaulPlacement[] placements;
        }
        private static readonly List<OverhaulPlacement> overhaulProps = new List<OverhaulPlacement>();

        // Places one downloaded CC0 model on a real support surface. Missing
        // downloads (multi-material skips at fetch time) are recorded, never
        // replaced by authored or primitive geometry.
        private static void Place(string area, string id, string role, Vector3 support, float maxSize,
            bool collision = false, float yaw = 0, Vector3? normal = null, float pitch = 0)
        {
            string path = Imported + id + "/" + id + ".fbx";
            if (!AssetDatabase.LoadAssetAtPath<GameObject>(path))
            {
                Debug.LogWarning("P1_OVERHAUL_SKIP_MISSING " + id);
                return;
            }
            var n = normal ?? Vector3.up;
            var root = SourceProp(id, "P1Overhaul_" + id + "__" + role, support, 1, collision, maxDimension: maxSize);
            root.transform.rotation = Quaternion.Euler(pitch, yaw, 0);
            var renderers = root.GetComponentsInChildren<Renderer>(); var bounds = renderers[0].bounds;
            foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);
            float extent = Vector3.Dot(new Vector3(Mathf.Abs(n.x), Mathf.Abs(n.y), Mathf.Abs(n.z)), bounds.extents);
            root.transform.position += n * (extent + .006f);
            overhaulProps.Add(new OverhaulPlacement
            {
                id = id, role = role, area = area, source = "https://polyhaven.com/a/" + id,
                supportPoint = support, centre = root.transform.position, size = bounds.size, physicalCollider = collision
            });
        }

        private static void OverhaulDressing()
        {
            overhaulProps.Clear(); current = Group("Phase1VisualOverhaulAssets");
            // ---------- Exterior and arrival ----------
            Place("exterior", "concrete_road_barrier", "ParkingBarrierWest", new Vector3(-18, 0, -28), 2.2f, true, 15);
            Place("exterior", "concrete_road_barrier_02", "ParkingBarrierEast", new Vector3(24, 0, -26), 2.2f, true, -10);
            Place("exterior", "modular_chainlink_fence", "CompoundFenceSegment", new Vector3(31, 0, -14), 3f, true, 90);
            Place("exterior", "large_iron_gate", "CompoundIronGate", new Vector3(0, 0, -36.8f), 2.7f, false, 0);
            Place("exterior", "street_lamp_01", "ParkingLampWest", new Vector3(-22, 0, -33), 4.6f);
            Place("exterior", "street_lamp_02", "ParkingLampEast", new Vector3(19, 0, -36), 4.6f);
            Place("exterior", "fire_hydrant", "HydrantByFacade", new Vector3(12, 0, -23.5f), .8f, true);
            Place("exterior", "old_tyre", "AbandonedTyre", new Vector3(28, 0, -18), .75f, true, 40);
            Place("exterior", "rusted_wheel_rim_01", "RustedWheelRim", new Vector3(26.8f, 0, -19.5f), .55f, true);
            Place("exterior", "wooden_barrels_01", "GuardBarrels", new Vector3(-13.5f, 0, -21.5f), 1f, true);
            Place("exterior", "Barrel_01", "PumpYardBarrel", new Vector3(-33, 0, -19), 1f, true);
            Place("exterior", "Barrel_02", "EastYardBarrel", new Vector3(33, 0, -30), 1f, true);
            Place("exterior", "barrel_03", "WestYardBarrel", new Vector3(-35, 0, -6), 1f, true);
            Place("exterior", "planter_box_01", "SignPlanterA", new Vector3(8.5f, 0, -22.6f), 1.1f, true);
            Place("exterior", "planter_box_02", "SignPlanterB", new Vector3(-1, 0, -22.6f), 1.1f, true);
            Place("exterior", "exterior_aircon_unit", "FacadeAirconUnit", new Vector3(-18, 0, -19.4f), 1f, true);
            Place("exterior", "modular_electricity_poles", "YardPowerPole", new Vector3(34, 0, -38), 6.5f);
            Place("exterior", "security_light", "GuardSecurityLight", new Vector3(-5.4f, 2.9f, -23), .5f, false, 90, Vector3.right);
            Place("exterior", "covered_car", "CoveredAbandonedCar", new Vector3(22, 0, -31.5f), 4.4f, true, 90);
            Place("exterior", "garden_hose_wall_mounted_01", "GuardGardenHose", new Vector3(-10.42f, 1.5f, -25.5f), .8f, false, 0, Vector3.right);
            Place("exterior", "planter_box_03", "SignPlanterC", new Vector3(-8, 0, -22.6f), 1.1f, true);
            Place("exterior", "planter_pot_clay", "FacadeClayPlanter", new Vector3(2.5f, 0, -22.3f), .55f, true);
            Place("exterior", "seeding_tray_01", "PlanterSeedTray", new Vector3(8.5f, .55f, -22.6f), .5f);
            Place("exterior", "old_military_crate", "PumpYardCrate", new Vector3(-33.5f, 0, -16.5f), 1f, true, 25);
            Place("exterior", "wooden_military_crate", "WestYardCrate", new Vector3(-34.5f, 0, 3), 1f, true, -15);
            Place("exterior", "compost_bag_02", "CourtyardCompostBag", new Vector3(11.5f, 0, 7.8f), .7f, true);
            // ---------- Floor 1: pump and service ----------
            Place("floor1", "modular_industrial_pipes_01", "PumpIndustrialPipes", new Vector3(-30.6f, 0, -13.5f), 3f, true, 90);
            Place("floor1", "modular_pipes", "PumpPipeRun", new Vector3(-25.6f, 0, -14.4f), 2.6f, true);
            Place("floor1", "modular_pipes_plastic_01", "PumpPlasticPipes", new Vector3(-26, .4f, -8.5f), 1.8f);
            Place("floor1", "modular_airduct_rectangular_01", "PumpCeilingDuct", new Vector3(-28, 2.85f, -11), 3f, false, 0, Vector3.down);
            Place("floor1", "modular_airduct_circular_01", "PumpRoundDuct", new Vector3(-25.9f, 2.8f, -9), 2f, false, 0, Vector3.down);
            Place("floor1", "old_military_compressor", "PumpCompressor", new Vector3(-30.2f, 0, -9.6f), 1.2f, true, 90);
            Place("floor1", "portable_welding_cart", "PumpWeldingCart", new Vector3(-25.7f, 0, -13.6f), 1.1f, true, -20);
            Place("floor1", "bench_vice_01", "WorkbenchVice", new Vector3(-29.2f, .88f, -8.95f), .35f);
            Place("floor1", "drill_press_01", "PumpDrillPress", new Vector3(-30.6f, 0, -11.8f), 1.6f, true, 90);
            Place("floor1", "metal_tool_chest", "PumpToolChest", new Vector3(-27, 0, -14.55f), 1.1f, true);
            Place("floor1", "tool_cart", "PumpToolCart", new Vector3(-24.9f, 0, -13.8f), 1.1f, true, 15);
            Place("floor1", "hand_truck", "PumpHandTruck", new Vector3(-31.2f, 0, -14.6f), 1.3f, true, -80);
            Place("floor1", "wooden_ladder", "PumpWallLadder", new Vector3(-24.7f, 0, -14.7f), 2.6f, true, 180);
            Place("floor1", "ladder_sectioned_01", "GalleryLadderL1", new Vector3(-16.2f, 0, 12.35f), 2.8f, true, 180);
            Place("floor1", "crowbar_01", "WorkbenchCrowbar", new Vector3(-28.6f, .88f, -8.55f), .45f, false, 30, Vector3.up, 90);
            Place("floor1", "pipe_wrench", "WorkbenchPipeWrench", new Vector3(-29.5f, .88f, -8.5f), .4f);
            Place("floor1", "sledgehammer_01", "PumpSledgehammer", new Vector3(-27.4f, 0, -8.35f), 1f, true, 65);
            Place("floor1", "korean_fire_extinguisher_01", "ServiceExtinguisher", new Vector3(-23.7f, 0, -12.6f), .55f, true);
            Place("floor1", "steel_frame_shelves_01", "PumpPartsShelves", new Vector3(-30.8f, 0, -6.5f), 1.9f, true, 90);
            Place("floor1", "metal_stool_01", "PumpStool", new Vector3(-28.2f, 0, -10.2f), .55f, true);
            Place("floor1", "watering_can_metal_01", "PumpWateringCan", new Vector3(-26.4f, 0, -14.8f), .4f, true);
            // ---------- Floor 2: residential and archive ----------
            Place("floor2", "plastic_monobloc_chair_01", "CorridorMonoblocChair", new Vector3(-14, 4, -12.3f), .85f, true, 160);
            Place("floor2", "painted_wooden_chair_01", "GalleryChairNorth", new Vector3(13, 4, 12.3f), .9f, true, -150);
            Place("floor2", "painted_wooden_chair_02", "GalleryChairWest", new Vector3(-6, 4, 12.35f), .9f, true, 180);
            Place("floor2", "painted_wooden_table", "GallerySideTable", new Vector3(14.2f, 4, 12.2f), .85f, true);
            Place("floor2", "painted_wooden_sofa", "ArchiveSofa", new Vector3(7.5f, 4, 15.2f), 1.1f, true, 180);
            Place("floor2", "painted_wooden_cabinet", "ArchiveCabinetPainted", new Vector3(.6f, 4, 19.3f), 1.7f, true, 180);
            Place("floor2", "painted_wooden_cabinet_02", "ArchiveCabinetSecond", new Vector3(9.6f, 4, 19.2f), 1.7f, true, 180);
            Place("floor2", "painted_wooden_shelves", "ArchiveWallShelves", new Vector3(-1.5f, 5.2f, 19.9f), 1.2f, false, 180, Vector3.back);
            Place("floor2", "painted_wooden_nightstand", "ArchiveNightstand", new Vector3(4.6f, 4, 14.6f), .65f, true);
            Place("floor2", "wooden_bookshelf_worn", "ArchiveBookshelf", new Vector3(8.2f, 4, 19.45f), 1.8f, true, 180);
            Place("floor2", "vintage_day_bed", "ArchiveDayBed", new Vector3(-.5f, 4, 14.2f), 1f, true, 90);
            Place("floor2", "old_bed_frame", "AbandonedBedFrame203", new Vector3(6.2f, 4, 17.8f), 1.1f, true, -90);
            Place("floor2", "television_02", "ArchiveOldTelevision", new Vector3(9.2f, 4, 13.8f), .8f, true, -140);
            Place("floor2", "boombox", "ArchiveBoombox", new Vector3(6.3f, 5.4f, 18.9f), .4f);
            Place("floor2", "vintage_suitcase", "CorridorSuitcaseEast", new Vector3(16.35f, 4, -6), .7f, true);
            Place("floor2", "wicker_basket_02", "GalleryBasketWest", new Vector3(-16.35f, 4, 6), .55f, true);
            Place("floor2", "plastic_crate_01", "GalleryCrateEast", new Vector3(16.4f, 4, 9), .55f, true);
            Place("floor2", "plastic_crate_03", "GalleryCrateWest", new Vector3(-16.4f, 4, -9), .55f, true);
            Place("floor2", "standing_chalkboard_01", "ManagementChalkboard", new Vector3(7.6f, 4, -12.35f), 1.5f, true, 180);
            Place("floor2", "sungka_board", "NightstandSungka", new Vector3(4.6f, 4.68f, 14.6f), .45f);
            Place("floor2", "SchoolChair_01", "CorridorSchoolChair", new Vector3(16.35f, 4, -2), .85f, true, -90);
            Place("floor2", "SchoolDesk_01", "ArchiveSchoolDesk", new Vector3(2.2f, 4, 16.5f), .8f, true, 90);
            Place("floor2", "steel_frame_shelves_02", "ArchiveSteelShelves", new Vector3(-4.5f, 4, 19.2f), 1.9f, true, 180);
            Place("floor2", "round_wooden_table_01", "GalleryRoundTable", new Vector3(-14.5f, 4, 12.2f), .8f, true);
            // ---------- Floor 3: cleaning niche and sealed corridor ----------
            Place("floor3", "WetFloorSign_01", "WetFloorSign", new Vector3(-10, 8, -12.2f), .65f, true, 20);
            Place("floor3", "dustpan", "CleaningDustpan", new Vector3(4.3f, 8, -12.6f), .35f);
            Place("floor3", "plunger", "CleaningPlunger", new Vector3(5.9f, 8, -12.6f), .55f, true);
            Place("floor3", "all_purpose_cleaner", "TrolleyCleanerBottle", new Vector3(1.8f, 8.78f, -12.05f), .28f);
            Place("floor3", "multi_cleaner_bottle", "TrolleyCleanerSecond", new Vector3(2.25f, 8.78f, -12.25f), .28f);
            Place("floor3", "multi_cleaner_5_litre", "CleaningCanister", new Vector3(6.6f, 8, -12.55f), .5f, true);
            Place("floor3", "drain_cleaner", "CorridorDrainCleaner", new Vector3(-14, 8, -12.55f), .3f, true);
            Place("floor3", "cleaner_tin_01", "CorridorCleanerTin", new Vector3(12, 8, -12.55f), .3f, true);
            Place("floor3", "wooden_broom", "CleaningWoodenBroom", new Vector3(8.2f, 8, -12.62f), 1.4f, true, 12);
            Place("floor3", "spray_paint_bottles", "LiftSprayPaint", new Vector3(-18.5f, 8, -12.5f), .35f, true);
            Place("floor3", "industrial_storage_cart", "CleaningStorageCart", new Vector3(16, 8, -12.35f), 1.2f, true, 180);
            Place("floor3", "hanging_industrial_lamp", "CorridorIndustrialLamp", new Vector3(-4, 11.35f, -11), 1f, false, 0, Vector3.down);
            Place("floor3", "industrial_caged_sconce", "CorridorCagedSconce", new Vector3(0, 10.4f, -9.18f), .5f, false, 0, Vector3.back);
            Place("floor3", "pull_chain_light_socket", "CorridorPullSocket", new Vector3(8, 11.32f, -11), .4f, false, 0, Vector3.down);
            Place("floor3", "lightbulb_led", "CorridorBareBulb", new Vector3(8, 11.02f, -11), .18f, false, 0, Vector3.down);
            Place("floor3", "security_camera_01", "LiftSecurityCamera", new Vector3(-20.2f, 11f, -9.4f), .4f, false, 45, Vector3.back);
            Place("floor3", "metal_stool_02", "CleaningStoolL3", new Vector3(9.5f, 8, -12.5f), .55f, true);
            // ---------- Floor 4: rooms, communal spaces, ritual, arena ----------
            Place("floor4", "GothicCabinet_01", "K404GothicCabinet", new Vector3(-3.6f, 12, -19.2f), 2f, true, 0);
            Place("floor4", "GothicBed_01", "K401GothicBed", new Vector3(-12.5f, 12, 17.5f), 1.4f, true, 90);
            Place("floor4", "GothicCommode_01", "K402Commode", new Vector3(.2f, 12, 19.2f), 1.1f, true, 180);
            Place("floor4", "gothic_coffee_table", "K404RitualTable", new Vector3(2.6f, 12, -15.4f), .6f, true, 20);
            Place("floor4", "brass_candleholders", "AltarCandleholders", new Vector3(-.55f, 13.1f, -17.95f), .5f);
            Place("floor4", "wooden_candlestick", "AltarCandlestick", new Vector3(.55f, 13.1f, -17.55f), .48f);
            Place("floor4", "brass_diya_lantern", "AltarDiyaLantern", new Vector3(0, 13.1f, -18.1f), .38f);
            Place("floor4", "Lantern_01", "K403Lantern", new Vector3(10.6f, 12, 14.4f), .45f, true);
            Place("floor4", "vintage_oil_lamp", "K402OilLamp", new Vector3(-3.2f, 12.47f, 18.2f), .35f);
            Place("floor4", "ornate_mirror_01", "K404OrnateMirror", new Vector3(3.2f, 13.7f, -19.88f), 1.3f, false, 0, Vector3.forward);
            Place("floor4", "hanging_picture_frame_01", "L4FrameNorth", new Vector3(-14, 13.9f, 12.86f), .8f, false, 0, Vector3.back);
            Place("floor4", "hanging_picture_frame_02", "L4FrameSouth", new Vector3(14, 13.9f, -12.86f), .8f, false, 0, Vector3.forward);
            Place("floor4", "hanging_picture_frame_03", "BathroomFrame", new Vector3(-19.5f, 13.9f, 5.3f), .7f, false, 0, Vector3.back);
            Place("floor4", "standing_picture_frame_01", "K403StandingFrame", new Vector3(6, 12, 14.1f), .8f, true, -30);
            Place("floor4", "fancy_picture_frame_01", "K402FancyFrame", new Vector3(-2, 13.9f, 19.9f), .8f, false, 0, Vector3.back);
            Place("floor4", "antique_ceramic_vase_01", "K404AntiqueVase", new Vector3(-1.9f, 12, -18.6f), .6f, true);
            Place("floor4", "ceramic_vase_01", "K401Vase", new Vector3(-7, 12, 19.2f), .55f, true);
            Place("floor4", "ceramic_vase_02", "K403Vase", new Vector3(11, 12, 19.3f), .55f, true);
            Place("floor4", "brass_vase_01", "K404BrassVase", new Vector3(1.5f, 12, -18.7f), .55f, true);
            Place("floor4", "carved_wooden_elephant", "CommodeElephant", new Vector3(.2f, 13f, 19.2f), .3f);
            Place("floor4", "tea_set_01", "KitchenTeaSet", new Vector3(20.1f, 12.86f, 7f), .35f);
            Place("floor4", "pot_enamel_01", "KitchenEnamelPot", new Vector3(19.55f, 12.86f, 7.6f), .3f);
            Place("floor4", "electric_stove", "KitchenElectricStove", new Vector3(18.1f, 12, 4.2f), .65f, true, -90);
            Place("floor4", "brass_pot_01", "KitchenBrassPot", new Vector3(23.1f, 12, 3.8f), .45f, true);
            Place("floor4", "brass_pot_02", "KilnBrassPot", new Vector3(20.9f, 12.72f, 6.25f), .4f);
            Place("floor4", "jug_01", "BathroomJug", new Vector3(-19, 12, 4.5f), .4f, true);
            Place("floor4", "wooden_bowl_01", "KitchenBowl", new Vector3(20.65f, 12.86f, 7.55f), .25f);
            Place("floor4", "wooden_bowl_02", "RitualTableBowl", new Vector3(2.6f, 12.5f, -15.4f), .25f);
            Place("floor4", "WoodenChair_01", "K403WoodenChair", new Vector3(10.8f, 12, 16.2f), .9f, true, -120);
            Place("floor4", "small_wooden_table_01", "K401SideTable", new Vector3(-8.6f, 12, 19.2f), .7f, true);
            Place("floor4", "drawer_cabinet", "K402DrawerCabinet", new Vector3(1.8f, 12, 19.3f), 1.1f, true, 180);
            Place("floor4", "side_table_01", "K404SideTable", new Vector3(-2.2f, 12, -15.6f), .65f, true);
            Place("floor4", "dining_table", "KitchenDiningTable", new Vector3(19.5f, 12, 2.2f), .95f, true, 90);
            // ---------- Shared architecture / stairs / signage ----------
            Place("shared", "modular_metal_gutter", "RoofGutterRun", new Vector3(0, 15.75f, -20.45f), 7f, false);
            Place("shared", "modular_electric_cables", "FacadeCableRun", new Vector3(-20, 1.2f, -20.45f), 2.5f, true, 0, Vector3.back);
            Place("shared", "rollershutter_door", "PumpRollerShutter", new Vector3(-33.5f, 0, -14), 2.6f, true, 90);
            Place("shared", "rollershutter_window_01", "FacadeShutterWindow", new Vector3(-16, 1.6f, -20.3f), 1.4f, false, 0, Vector3.back);
            Place("shared", "security_camera_02", "GuardExteriorCamera", new Vector3(-10.4f, 2.85f, -21.25f), .4f, false, -45, Vector3.back);
            Place("shared", "industrial_wall_lamp", "StairwellWallLamp", new Vector3(21.25f, 10.6f, -5.5f), .5f, false, 0, Vector3.left);
            Place("shared", "industrial_wall_sconce", "K102StairSconce", new Vector3(-20.9f, 2.6f, 5), .5f, false, 0, Vector3.right);
            Place("shared", "modern_ceiling_lamp_01", "L2CeilingLamp", new Vector3(0, 7.55f, -11), .7f, false, 0, Vector3.down);
            Place("shared", "mantel_clock_01", "ArchiveMantelClock", new Vector3(4.7f, 5.4f, 18.9f), .35f);
            Place("shared", "alarm_clock_01", "K403AlarmClock", new Vector3(9, 12.87f, 16.4f), .2f);
            Place("shared", "book_encyclopedia_set_01", "ArchiveEncyclopedias", new Vector3(9.8f, 5.5f, 19f), .4f);
            Place("shared", "decorative_book_set_01", "K403BookSet", new Vector3(9.5f, 12.87f, 16.9f), .35f);
            Place("shared", "binder_notebook", "GuardDeskBinder", new Vector3(-8.1f, .97f, -23.35f), .25f);
            Place("shared", "clipboard", "PumpWorkbenchClipboard", new Vector3(-28.75f, .89f, -8.95f), .25f, false, 20, Vector3.up, 90);
            Place("shared", "postcard_set_01", "NoticeboardPostcards", new Vector3(1, 5.45f, -12.66f), .3f, false, 0, Vector3.forward);
            Place("shared", "megaphone_01", "GuardMegaphone", new Vector3(-8, .52f, -21.9f), .35f);
            Place("shared", "gate_latch_01", "EntryGateLatch", new Vector3(-3.1f, 1.25f, -36.65f), .3f, false, 0, Vector3.forward);
            Place("shared", "water_manhole_cover", "ParkingManhole", new Vector3(-2, .01f, -25), 1f);
            File.WriteAllText("Audit/phase1-overhaul-prop-placement.json", JsonUtility.ToJson(new OverhaulReceipt
            {
                distinctModels = overhaulProps.Select(p => p.id).Distinct().Count(),
                utc = DateTime.UtcNow.ToString("o"), placements = overhaulProps.ToArray()
            }, true) + "\n");
            current = environment;
        }
    }
}
#endif
