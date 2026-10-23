#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    public static partial class Phase1SceneBuilder
    {
        [Serializable] private sealed class PropPlacement
        {
            public string id,role,source;
            public Vector3 supportPoint,supportNormal,centre,size;
            public bool physicalCollider;
        }
        [Serializable] private sealed class DressingReceipt
        {
            public int minimumDistinctModels=30,distinctModels;
            public string utc;
            public PropPlacement[] placements;
        }
        private static readonly List<PropPlacement> additionalProps=new List<PropPlacement>();
        private static void Dress(string id,string role,Vector3 support,float maxSize,bool collision=false,float yaw=0,Vector3? normal=null,float pitch=0)
        {
            var n=normal??Vector3.up;
            var root=SourceProp(id,"P1NewProp_"+id+"__"+role,support,1,collision,maxDimension:maxSize);
            root.transform.rotation=Quaternion.Euler(pitch,yaw,0);
            var renderers=root.GetComponentsInChildren<Renderer>();var bounds=renderers[0].bounds;
            foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
            float extent=Vector3.Dot(new Vector3(Mathf.Abs(n.x),Mathf.Abs(n.y),Mathf.Abs(n.z)),bounds.extents);
            root.transform.position+=n*(extent+.006f);
            additionalProps.Add(new PropPlacement{id=id,role=role,source="https://polyhaven.com/a/"+id,supportPoint=support,supportNormal=n,
                centre=root.transform.position,size=bounds.size,physicalCollider=collision});
        }
        private static void AdditionalDressing()
        {
            additionalProps.Clear();current=Group("ThirtyAdditionalDownloadedProps");
            // Pos satpam: supplies belong on the existing desk or beneath it.
            Dress("medical_box","GuardFirstAid",new Vector3(-9.45f,0,-22.7f),.38f,true);
            Dress("stationery_supplies","GuardDeskStationery",new Vector3(-8.55f,.93f,-23.15f),.24f);
            Dress("small_plastic_torch","GuardDeskTorch",new Vector3(-7.45f,.93f,-23.15f),.22f);
            Dress("wall_clock","GuardRearWallClock",new Vector3(-8,2.25f,-21.425f),.34f,false,180,Vector3.back);
            // Maintenance tools sit on a usable support surface, away from the pump approach.
            SourceProp("metal_office_desk","PumpServiceWorkbench",new Vector3(-29,.44f,-8.7f),.88f);
            Dress("metal_toolbox","PumpServiceToolbox",new Vector3(-29,0,-8.75f),.5f,true);
            Dress("adjustable_wrench","WorkbenchAdjustableWrench",new Vector3(-29.7f,.88f,-8.8f),.27f,pitch:90);
            Dress("combination_wrench","WorkbenchCombinationWrench",new Vector3(-29.35f,.88f,-8.8f),.24f);
            Dress("cross_pein_hammer","WorkbenchHammer",new Vector3(-28.95f,.88f,-8.8f),.3f,pitch:90);
            Dress("measuring_tape_01","WorkbenchMeasuringTape",new Vector3(-28.55f,.88f,-8.8f),.1f);
            Dress("utility_box_01","PumpWallServiceBox",new Vector3(-28,1.8f,-8.15f),.42f,false,180,Vector3.back);
            Dress("cement_bag","PumpRepairSupplies",new Vector3(-30.5f,0,-10),.65f,true);
            Dress("oil_tin","PumpLubricant",new Vector3(-30.5f,0,-10.85f),.3f,true);
            Dress("metal_jerrycan_green","PumpFuelCan",new Vector3(-30.5f,0,-11.5f),.48f,true);
            // Place clutter against the room edge, leaving the authored corridor centre clear.
            Dress("plastic_crate_02","L3CleaningCrate",new Vector3(10.3f,8,-12.55f),.55f,true);
            Dress("cardboard_box_01","ArchiveStorageBox",new Vector3(-2,4,18.8f),.6f,true);
            Dress("ceramic_pot","ResidentsEmptyPlanter",new Vector3(-13.5f,4,12.25f),.28f,true);
            Dress("potted_plant_04","ResidentsCourtyardPlant",new Vector3(16.3f,0,-6),.58f,true);
            Dress("wicker_basket_01","ResidentsLaundryBasket",new Vector3(-10,4,12.3f),.5f,true);
            // Bathroom supplies stay outside the EVP centre and hiding entrance.
            Dress("plastic_broom","BathroomCleaningBroom",new Vector3(-17.6f,12,-.8f),1.15f,true,15);
            Dress("rubber_boots","BathroomMaintenanceBoots",new Vector3(-21.4f,12,-.8f),.32f,true);
            Dress("folding_wooden_stool","BathroomWashStool",new Vector3(-18.2f,12,4.9f),.45f,true);
            // A second preparation surface keeps all new utensils away from the keris sightline.
            SourceProp("metal_office_desk","CommunalKitchenPrepDesk",new Vector3(20.1f,12.43f,7.3f),.86f);
            Dress("brass_pan_01","KitchenCookingPan",new Vector3(20.1f,12.86f,7.25f),.29f);
            Dress("vintage_electric_kettle","KitchenKettle",new Vector3(20.65f,12.86f,7.3f),.28f);
            Dress("wooden_cutting_board","KitchenCuttingBoard",new Vector3(19.65f,12.86f,7.3f),.3f);
            Dress("carved_wooden_plate","KitchenPlate",new Vector3(19.25f,12.86f,7.3f),.22f);
            Dress("metal_jug","KitchenWaterJug",new Vector3(20.4f,12.86f,7.55f),.2f);
            Dress("small_lpg_tank","KitchenFuelCylinder",new Vector3(23.2f,12,6.4f),.58f,true);
            Dress("metal_stool_03","KitchenWorkStool",new Vector3(18.5f,12,6.7f),.5f,true);
            Dress("ceiling_fan","K403UnpoweredFan",new Vector3(8.5f,15.525f,16.7f),.95f,false,0,Vector3.down);
            Dress("fire_alarm","L4FireSafetyRemnant",new Vector3(16.89f,14.5f,7),.24f,false,-90,Vector3.left);
            int count=additionalProps.Select(p=>p.id).Distinct().Count();
            if(count<30)throw new InvalidOperationException("Need at least 30 distinct additional downloaded prop assets actually placed");
            File.WriteAllText("Audit/phase1-additional-prop-placement.json",JsonUtility.ToJson(new DressingReceipt{distinctModels=count,utc=DateTime.UtcNow.ToString("o"),placements=additionalProps.ToArray()},true)+"\n");
            current=environment;
        }
    }
}
#endif
