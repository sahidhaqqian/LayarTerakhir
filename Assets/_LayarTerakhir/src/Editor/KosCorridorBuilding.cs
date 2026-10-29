#if UNITY_EDITOR
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    // Closed architectural volumes and authored dressing, not an invisible wall around a hallway.
    public static partial class KosCorridorBuilder
    {
        private static Material lowerPaint,trim,roofing,glass;
        private static void HallWallWithDoorways(float x,float y)
        {
            var gaps=new System.Collections.Generic.List<Vector2>();
            for(int i=0;i<4;i++)
            {
                float z=1.5f+i*3;
                gaps.Add(y<1&&i==1?new Vector2(3.5f,5):new Vector2(z-.5f,z+.5f));
            }
            float from=-1;
            foreach(var gap in gaps)
            {
                Box("HallWallPier",new Vector3(x,y+1.45f,(from+gap.x)*.5f),new Vector3(.18f,2.9f,gap.x-from),wall);
                Box("HallDoorHeader",new Vector3(x,y+2.55f,(gap.x+gap.y)*.5f),new Vector3(.18f,.7f,gap.y-gap.x),wall);
                from=gap.y;
            }
            // The rear ground rooms end at z=12. Stopping at 11 left a
            // walkable breach beside rooms 4/8; z=12..13 is the stair access.
            float end=y<1?12:13;
            if(end>from)Box(y<1?(x<0?"GroundHallEndWall_West":"GroundHallEndWall_East"):"HallWallEndPier",new Vector3(x,y+1.45f,(from+end)*.5f),new Vector3(.18f,2.9f,end-from),wall);
        }
        private static void CompleteBuilding()
        {
            lowerPaint=Material("KosSagePaint",new Color(.23f,.31f,.28f),"Assets/_LayarTerakhir/assets/Art/Generated/Materials/OldLimewash.mat");
            trim=Material("KosAgedIvoryTrim",new Color(.44f,.42f,.34f));
            roofing=Material("KosDarkRoof",new Color(.12f,.10f,.085f),"Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/dirty_concrete.mat");
            glass=Material("KosOpaqueNightGlass",new Color(.025f,.042f,.05f));
            RoomVolumes();StairSafety();MainEntrance();HallDressing();ExteriorEnvelope();
        }
        private static void RoomVolumes()
        {
            var parent=root;
            for(int level=0;level<2;level++)foreach(int side in new[]{-1,1})for(int i=0;i<4;i++)
            {
                if(level==0&&i==1)continue; // Shared rooms already have their own complete shells.
                float y=level*3.3f,z=1.5f+i*3,x=side*3.9f;
                int number=(level==1?(side<0?21:25):(side<0?1:5))+i;
                var cell=new GameObject("RoomCell_F"+(level+1)+"_"+number).transform;cell.SetParent(parent,false);root=cell;
                Box("RoomFloor",new Vector3(x,y-.065f,z),new Vector3(4.7f,.13f,3),floor);
                Box("RoomCeiling",new Vector3(x,y+2.97f,z),new Vector3(4.7f,.14f,3.15f),wall);
                Box("RoomOuterWall",new Vector3(side*6.25f,y+1.45f,z),new Vector3(.18f,2.9f,3.15f),wall);
                foreach(float edge in new[]{z-1.5f,z+1.5f})Box("RoomPartition",new Vector3(x,y+1.45f,edge),new Vector3(4.8f,2.9f,.15f),wall);
                root=parent;
            }
            // Ground slab under the entire stairwell: no hole into the skybox even below flights.
            Box("StairwellFoundation",new Vector3(.55f,-.11f,14.5f),new Vector3(4.25f,.22f,5.6f),floor);
            Box("StairGroundEastReturn",new Vector3(2.08f,1.45f,11.72f),new Vector3(1.15f,2.9f,.15f),wall);
            // Seal the narrow side strip beyond the upper landing, expressed as actual masonry.
            Box("StairUpperEastReturn",new Vector3(2.1f,4.75f,11.67f),new Vector3(1.16f,2.9f,.16f),wall);
        }
        private static void StairSafety()
        {
            // The original outer handrails do not protect the open INNER flight edges.
            // Solid knee walls + a graspable rail follow the same pitch as each flight.
            for(int i=0;i<10;i++)
            {
                float a=3.3f-(i+1)*.165f,b=1.65f-(i+1)*.165f;
                Box("StairInnerGuardA_"+i,new Vector3(.06f,a+.53f,13.14f+i*.28f),new Vector3(.13f,1.06f,.29f),lowerPaint);
                Box("StairInnerGuardB_"+i,new Vector3(1.09f,b+.53f,15.66f-i*.28f),new Vector3(.13f,1.06f,.29f),lowerPaint);
                // Bullnose strips make each step edge readable in the low light.
                Box("StairNosingA_"+i,new Vector3(-.60f,a+.006f,13.01f+i*.28f),new Vector3(1.18f,.012f,.035f),trim);
                Box("StairNosingB_"+i,new Vector3(1.75f,b+.006f,15.79f-i*.28f),new Vector3(1.18f,.012f,.035f),trim);
            }
            Beam("StairInnerHandrailA",new Vector3(.06f,4.34f,13),new Vector3(.06f,2.69f,15.8f),.045f);
            Beam("StairInnerHandrailB",new Vector3(1.09f,2.69f,15.8f),new Vector3(1.09f,1.04f,13),.045f);
            Box("UpperLandingGuard",new Vector3(1.32f,3.85f,13.03f),new Vector3(2.52f,1.1f,.13f),lowerPaint);
            Box("UpperLandingCap",new Vector3(1.32f,4.42f,13.03f),new Vector3(2.58f,.045f,.19f),trim);
            Box("LandingInnerReturn",new Vector3(.57f,2.18f,15.82f),new Vector3(.92f,1.06f,.12f),lowerPaint);
            // Plang wayfinding 02/TURUN & 01/KELUAR dihapus (desainer: jelek, teks melayang).
            Box("StairRearWindowFrame",new Vector3(.55f,4.45f,17.07f),new Vector3(2.8f,1.35f,.08f),metal);
            Box("StairRearNightWindow",new Vector3(.55f,4.45f,17.01f),new Vector3(2.64f,1.20f,.025f),glass);
            for(int i=-2;i<=2;i++)Box("StairWindowBars",new Vector3(.55f+i*.45f,4.45f,16.98f),new Vector3(.024f,1.22f,.025f),trim);
            Box("StairWindowSill",new Vector3(.55f,3.75f,16.97f),new Vector3(2.95f,.09f,.24f),trim);
        }
        private static void MainEntrance()
        {
            // A real front facade, frame, moving panel, handles, transom and exterior porch.
            foreach(int side in new[]{-1,1})
                Box("MainFacadeWall",new Vector3(side*3.62f,1.54f,-1.02f),new Vector3(5.44f,3.08f,.20f),wall);
            Box("MainEntranceLintel",new Vector3(0,2.70f,-1.02f),new Vector3(1.8f,.76f,.20f),wall);
            foreach(float x in new[]{-.9f,.9f})Box("MainEntranceJamb",new Vector3(x,1.17f,-1.02f),new Vector3(.10f,2.34f,.24f),wood);
            Box("MainEntranceFrameTop",new Vector3(0,2.31f,-1.02f),new Vector3(1.9f,.09f,.24f),wood);
            Box("MainEntranceThreshold",new Vector3(0,.015f,-1.02f),new Vector3(1.8f,.03f,.28f),trim);
            var hinge=new GameObject("MainEntranceHinge").transform;hinge.SetParent(root,false);hinge.position=new Vector3(-.84f,0,-1.03f);
            var leaf=Box("KosMainEntranceDoor",new Vector3(0,1.12f,-1.03f),new Vector3(1.68f,2.24f,.065f),wood);leaf.transform.SetParent(hinge,true);
            Object.DestroyImmediate(leaf.GetComponent<Collider>());
            var hull=hinge.gameObject.AddComponent<BoxCollider>();hull.center=new Vector3(.84f,1.12f,0);hull.size=new Vector3(1.68f,2.24f,.065f);
            var door=hinge.gameObject.AddComponent<HingedDoor>();door.hinge=hinge;door.label="Buka pintu utama kos";door.angle=105;d.entranceDoor=door;
            for(int i=0;i<3;i++)
            {
                var panel=Box("MainDoorRecessedPanel",new Vector3(0,.43f+i*.61f,-.988f),new Vector3(1.43f,.48f,.025f),trim);panel.transform.SetParent(hinge,true);
                var inset=Box("MainDoorPanelInset",new Vector3(0,.43f+i*.61f,-.969f),new Vector3(1.32f,.37f,.02f),wood);inset.transform.SetParent(hinge,true);
            }
            foreach(float z in new[]{-.94f,-1.12f})
            {
                var plate=Box("MainDoorHandlePlate",new Vector3(.62f,1.1f,z),new Vector3(.065f,.25f,.018f),metal);plate.transform.SetParent(hinge,true);
                var handle=Box("MainDoorHandle",new Vector3(.56f,1.15f,z+(z> -1? .025f:-.025f)),new Vector3(.17f,.025f,.045f),metal);handle.transform.SetParent(hinge,true);
            }
            Label("MainEntranceSignInside","PINTU UTAMA",new Vector3(0,2.65f,-.905f),Vector3.forward,.15f);
            Box("PorchAwning",new Vector3(0,2.95f,-1.95f),new Vector3(3.7f,.15f,2.1f),roofing);
            foreach(float x in new[]{-1.7f,1.7f})Box("PorchPillar",new Vector3(x,1.4f,-2.73f),new Vector3(.16f,2.8f,.16f),lowerPaint);
            Practical(new Vector3(0,2.72f,-1.9f),new Color(.9f,.71f,.43f),1.9f,6);
            Label("KosFacadeName","RUMAH KOS",new Vector3(0,3.63f,-1.145f),Vector3.back,.30f);
        }
        private static void HallDressing()
        {
            foreach(float y in new[]{0,3.3f})foreach(int side in new[]{-1,1})
            {
                // Split paint panels around doors instead of painting across the openings.
                for(int i=0;i<5;i++)
                {
                    float z=i==0?0:i==4?12.25f:i*3;
                    if(y<1&&(i==1||i==4))continue;
                    float length=i==0?1.9f:i==4?1.5f:1.95f;
                    Box("HallSageWainscot",new Vector3(side*1.398f,y+.59f,z),new Vector3(.018f,1.18f,length),lowerPaint);
                    Box("HallChairRail",new Vector3(side*1.38f,y+1.20f,z),new Vector3(.045f,.045f,length),trim);
                }
                Box("HallCrownMoulding",new Vector3(side*1.39f,y+2.84f,6),new Vector3(.07f,.10f,14),trim);
                for(int i=0;i<4;i++)
                {
                    if(y<1&&i==1)continue;
                    float z=1.5f+i*3;
                    Box("DoorThreshold",new Vector3(side*1.36f,y+.012f,z),new Vector3(.19f,.025f,.92f),trim);
                    // Sendal jepit khas Indonesia: sol karet gelap + tali Y, bukan balok metal.
                    if(i%2==0)for(int foot=0;foot<2;foot++)
                    {
                        var slipper=Box("TenantSlipper",new Vector3(side*1.15f,y+.022f,z+.65f+foot*.17f),new Vector3(.26f,.035f,.11f),metal);
                        var strapBase=slipper.transform.position+Vector3.up*.026f+Vector3.forward*.035f;
                        var strapL=Box("SlipperStrapL",strapBase,new Vector3(.018f,.014f,.10f),trim);
                        strapL.transform.rotation=Quaternion.Euler(0,28,0);
                        var strapR=Box("SlipperStrapR",strapBase,new Vector3(.018f,.014f,.10f),trim);
                        strapR.transform.rotation=Quaternion.Euler(0,-28,0);
                        Box("SlipperToePost",slipper.transform.position+Vector3.up*.026f-Vector3.forward*.012f,new Vector3(.016f,.03f,.016f),trim);
                    }
                    Box("RoomVentFrame",new Vector3(side*1.39f,y+2.48f,z),new Vector3(.08f,.22f,.79f),metal);
                    for(int l=0;l<3;l++)Box("RoomVentLouvre",new Vector3(side*1.34f,y+2.42f+l*.06f,z),new Vector3(.025f,.023f,.71f),trim);
                }
                // Surface conduit is mounted above head height; it never crosses a doorway.
                Box("HallElectricalConduit",new Vector3(side*1.32f,y+2.62f,6),new Vector3(.025f,.025f,13.5f),metal);
                foreach(float z in new[]{2f,8f})Box("HallJunctionBox",new Vector3(side*1.31f,y+2.62f,z),new Vector3(.06f,.12f,.12f),trim);
            }
            foreach(float z in new[]{.2f,3,6,9,11.4f})
                Box("UpperHallCeilingBeam",new Vector3(0,6.03f,z),new Vector3(2.85f,.19f,.12f),trim);
            Box("NoticeboardFrame",new Vector3(1.36f,4.82f,.10f),new Vector3(.08f,.67f,.72f),wood);
            Box("NoticeboardPaper",new Vector3(1.31f,4.82f,.10f),new Vector3(.012f,.55f,.60f),trim);
            Label("NoticeboardRules","JAGA KEBERSIHAN\nTUTUP PINTU\nSETELAH KELUAR",new Vector3(1.30f,4.82f,.10f),Vector3.left,.105f);
            // Meter cabinet beside the exit; no invented narrative interaction.
            Box("ElectricMeterCabinet",new Vector3(-1.32f,1.65f,-.12f),new Vector3(.14f,.55f,.42f),metal);
            Box("ElectricMeterWindow",new Vector3(-1.24f,1.72f,-.12f),new Vector3(.02f,.22f,.27f),glass);
            Label("MeterLabel","kWh",new Vector3(-1.22f,1.52f,-.12f),Vector3.right,.08f);
        }
        private static void ExteriorEnvelope()
        {
            // Cap the room volumes at the facade and rear; the building has a silhouette from outside.
            foreach(int side in new[]{-1,1})
            {
                Box("ExteriorUpperFrontWing",new Vector3(side*3.9f,4.76f,-1.02f),new Vector3(4.8f,3.0f,.20f),wall);
                Box("ExteriorRearWing",new Vector3(side*4.47f,3.1f,12.95f),new Vector3(3.6f,6.2f,.18f),wall);
                Box("ExteriorFrontFloorBand",new Vector3(side*3.25f,3.20f,-1.14f),new Vector3(6.4f,.22f,.12f),lowerPaint);
                var roof=Box("KosPitchedRoof",new Vector3(side*3.3f,6.84f,5.9f),new Vector3(6.85f,.16f,15.1f),roofing);
                roof.transform.rotation=Quaternion.Euler(0,0,side*-12);
                for(int i=0;i<12;i++)
                {
                    var seam=Box("RoofStandingSeam",new Vector3(side*3.3f,6.94f,-1.1f+i*1.25f),new Vector3(6.85f,.045f,.025f),metal);seam.transform.rotation=roof.transform.rotation;
                }
                Box("RainDownpipe",new Vector3(side*6.39f,3.02f,-1.05f),new Vector3(.095f,6.04f,.095f),metal);
                Box("FacadeCornerColumn",new Vector3(side*6.27f,3.1f,-1.10f),new Vector3(.22f,6.2f,.22f),lowerPaint);
            }
            Box("KosRoofRidge",new Vector3(0,7.56f,5.9f),new Vector3(.25f,.12f,15.2f),roofing);
            // Wider foundation closes the front recess behind the external face.
            // Pondasi dibuat lebih rendah dari halaman agar tidak satu bidang (z-fight).
            Box("KosFrontFoundation",new Vector3(0,-.18f,-.55f),new Vector3(12.7f,.26f,1),floor);
            foreach(float x in new[]{-1.7f,1.7f})
            {
                var pot=Model("planter_pot_clay","PorchClayPlanter",new Vector3(.36f,.40f,.36f),new Vector3(x,.20f,-3.18f));
                var fern=Model("fern_02","PorchFern",new Vector3(.55f,.6f,.55f),new Vector3(x,.66f,-3.18f));
                Object.DestroyImmediate(fern.GetComponent<Collider>());
            }
            Box("YardDrainChannel",new Vector3(0,.006f,-3.15f),new Vector3(2.8f,.012f,.12f),metal);
            for(int i=-10;i<=10;i++)Box("DrainGrille",new Vector3(i*.12f,.016f,-3.15f),new Vector3(.018f,.014f,.12f),trim);
            // Jalur setapak diturunkan 1cm dari halaman agar tidak tabrakan bidang.
            Box("YardPath",new Vector3(0,-.04f,-4),new Vector3(1.8f,.03f,5.5f),floor);
            // Low-cost neighbouring masses and a curb, rather than a gate opening onto empty space.
            foreach(int side in new[]{-1,1})
            {
                Box("NeighbourBoundary",new Vector3(side*8.5f,1.8f,-11.5f),new Vector3(3,3.6f,8),lowerPaint);
                Box("StreetCurb",new Vector3(side*5.85f,.05f,-12),new Vector3(.22f,.2f,8),trim);
            }
            Box("StreetEndBoundary",new Vector3(0,1.5f,-16.1f),new Vector3(18,3,.2f),wall);
        }
    }
}
#endif
