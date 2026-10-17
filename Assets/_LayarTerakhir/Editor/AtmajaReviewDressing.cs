#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEngine;
using static LayarTerakhir.Editor.AtmajaSceneBuilder;
namespace LayarTerakhir.Editor
{
    public static class AtmajaReviewDressing
    {
        public static void Build()
        {
            var wood=Mat("House_MAT_WoodDoor");var plaster=Mat("House_MAT_PlasterDamaged");
            Surface("DiningDirtyTile",0,4.5f,.512f,11.6f,8.8f,Mat("concrete_floor_damaged_01"));
            Surface("KitchenDirtyTile",4,10.6f,.352f,3.7f,2.9f,Mat("concrete_floor_damaged_01"));
            Box("GroundFloorCeiling_0",P(-4.925f,4.5f,3.75f),new Vector3(1.85f,.14f,8.9f),Mat("House_MAT_Concrete"));
            Box("GroundFloorCeiling_1",P(-1.15f,2.925f,3.75f),new Vector3(5.7f,.14f,5.75f),Mat("House_MAT_Concrete"));
            Box("GroundFloorCeiling_2",P(2.05f,4.5f,3.75f),new Vector3(.7f,.14f,8.9f),Mat("House_MAT_Concrete"));
            Box("GroundFloorCeiling_3",P(3.7f,1.725f,3.75f),new Vector3(2.6f,.14f,3.35f),Mat("House_MAT_Concrete"));
            Box("GroundFloorCeiling_4",P(3.7f,7.075f,3.75f),new Vector3(2.6f,.14f,3.75f),Mat("House_MAT_Concrete"));
            Box("GroundFloorCeiling_5",P(5.425f,4.5f,3.75f),new Vector3(.85f,.14f,8.9f),Mat("House_MAT_Concrete"));
            Box("KitchenCeiling",P(4,10.6f,2.95f),new Vector3(3.8f,.13f,3.0f),Mat("House_MAT_Concrete"));
            Box("DiningDividerSouth",P(1.6f,1.6f,2.2f),new Vector3(.18f,3.4f,3.2f),plaster);
            Box("DiningDividerNorth",P(1.6f,6.7f,2.2f),new Vector3(.18f,3.4f,4.2f),plaster);
            Box("DiningDividerLintel",P(1.6f,3.9f,3.45f),new Vector3(.18f,.9f,1.4f),plaster);
            var hinge=new GameObject("KitchenDoorHinge").transform;hinge.SetParent(environment,false);hinge.position=P(3.4f,9,.35f);
            var leaf=Box("KitchenDiningDoor",P(4,9,1.5f),new Vector3(1.2f,2.3f,.10f),wood);leaf.transform.SetParent(hinge,true);director.kitchenDoor=hinge;Item(leaf,AtmajaAction.KitchenDoor,"Buka pintu ruang makan");
            var well=GameObject.CreatePrimitive(PrimitiveType.Cylinder);well.name="OldChainedKitchenWell";well.transform.SetParent(environment,false);well.transform.position=P(2.85f,11.2f,.72f);well.transform.localScale=new Vector3(1.05f,.36f,1.05f);well.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_BrickBase");Item(well,AtmajaAction.Well,"Periksa sumur tua");
            for(int i=0;i<4;i++)Box("RottenWellBoard",P(2.85f+(i-1.5f)*.23f,11.2f,1.09f),new Vector3(.21f,.045f,1.10f),wood);
            Chain(P(2.85f,11.2f,1.14f),1.1f,true);var bucket=Model("wooden_bucket_01","KitchenBucket",P(3.6f,11.9f,.35f));Ground(bucket,.35f);FitCollider(bucket);
            Existing("painted_wooden_cabinet_02","KitchenStorage",P(5.45f,11.7f,.88f),new Vector3(.8f,1.05f,.48f));
            var charger=Box("KitchenEmergencyCharger",P(5.76f,10.1f,1.35f),new Vector3(.15f,.20f,.09f),Mat("House_MAT_WoodFrameWhite"));Item(charger,AtmajaAction.Charge,"Isi daya darurat • dapur");
            Model("LongDiningTable","LongDiningTable",P(3.7f,4.9f,.52f));
            var top=Box("DiningTableTopCollision",P(3.7f,4.9f,1.42f),new Vector3(1.45f,.075f,4.4f),wood);top.GetComponent<Renderer>().enabled=false;
            foreach(float x in new[]{3.09f,4.31f})foreach(float z in new[]{2.86f,6.94f})Box("DiningTableLegCollision",P(x,z,.95f),new Vector3(.09f,.86f,.09f),wood).GetComponent<Renderer>().enabled=false;
            foreach(float z in new[]{3.7f,5.5f})foreach(float x in new[]{2.75f,4.65f})Chair("WornDiningChair",P(x,z,.52f),P(3.7f,z,.52f));
            Chair("EmptyARScareChair",P(3.7f,2.15f,.52f),P(3.7f,4.9f,.52f));
            for(int i=0;i<6;i++)
            {
                var plate=GameObject.CreatePrimitive(PrimitiveType.Cylinder);plate.name="MouldyPlate";plate.transform.SetParent(environment,false);plate.transform.position=P(3.35f+(i%2)*.65f,3.1f+i*.60f,1.47f);plate.transform.localScale=new Vector3(.23f,.008f,.23f);plate.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_Concrete");Object.DestroyImmediate(plate.GetComponent<Collider>());
            }
            var bag=ExistingOriginal("KosBackpack","DamarTornBackpack",P(3.85f,6.2f,.675f),new Vector3(.43f,.32f,.31f));
            foreach(var r in bag.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(Plain("DamarBagCanvas",new Color(.10f,.13f,.11f),.03f),r.sharedMaterials.Length).ToArray();
            Box("DriedBloodOnStrap",P(3.86f,6.03f,.73f),new Vector3(.045f,.18f,.012f),Plain("DriedBlood",new Color(.18f,.035f,.022f)),false);
            float phoneY=KosVisualV2.SurfaceHeight(bag,P(3.86f,6.17f,0).x,P(3.86f,6.17f,0).z)+.015f;
            director.damarPhone=Box("DamarCrackedPhone",P(3.86f,6.17f,phoneY),new Vector3(.075f,.015f,.145f),Plain("CrackedPhoneGlass",new Color(.12f,.18f,.15f),.55f));Item(director.damarPhone,AtmajaAction.DamarPhone,"Ambil kartu aplikasi dari HP Damar");
            var cracks=new GameObject("DamarScreenCracks",typeof(LineRenderer));cracks.transform.SetParent(director.damarPhone.transform,true);var line=cracks.GetComponent<LineRenderer>();line.widthMultiplier=.0018f;line.positionCount=5;line.sharedMaterial=Mat("House_MAT_WoodFrameWhite");line.SetPositions(new[]{P(3.83f,6.11f,phoneY+.009f),P(3.87f,6.16f,phoneY+.009f),P(3.85f,6.18f,phoneY+.009f),P(3.88f,6.22f,phoneY+.009f),P(3.84f,6.20f,phoneY+.009f)});
            // Vestibule/front sitting room is a real side exploration space; main circulation stays clear.
            Box("FrontSittingPartitionWest",P(-4,2.5f,2.1f),new Vector3(4,3.2f,.16f),plaster);
            Box("FrontSittingPartitionEast",P(.35f,2.5f,2.1f),new Vector3(2.3f,3.2f,.16f),plaster);
            Box("FrontSittingDoorLintel",P(-1.4f,2.5f,3.3f),new Vector3(1.25f,.7f,.16f),plaster);
            Door("FrontSittingDoor",P(-2,2.5f,.52f),P(-1.4f,2.5f,1.62f),new Vector3(1.2f,2.2f,.09f),"Buka ruang tamu depan");
            var corner=Model("CornerSideTable","CornerSideTable",P(-5.05f,1.15f,.52f));FitCollider(corner);
            director.clockHands=Box("BrassClockHands",P(-5.05f,1.15f,1.30f),new Vector3(.18f,.025f,.03f),Mat("Detail_AgedBrass"));Item(director.clockHands,AtmajaAction.ClockHands,"Ambil Jarum Jam Kuningan");
            var ashtray=GameObject.CreatePrimitive(PrimitiveType.Cylinder);ashtray.name="OldAshtray";ashtray.transform.SetParent(environment,false);ashtray.transform.position=P(-4.85f,1.1f,1.30f);ashtray.transform.localScale=new Vector3(.12f,.015f,.12f);ashtray.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_Concrete");Object.DestroyImmediate(ashtray.GetComponent<Collider>());
            Chair("FrontRoomWoodChair",P(-4.25f,.8f,.52f),P(-4.25f,2,.52f));Existing("wooden_bookshelf_worn","FrontRoomBookcase",P(-3.35f,.45f,1.25f),new Vector3(1.0f,1.45f,.35f));
            var clock=Model("GrandfatherClock","GrandfatherClock",P(-5.58f,5.8f,.52f));clock.transform.rotation=Facing(east);FitCollider(clock);Item(clock,AtmajaAction.Clock,"Periksa jam bandul tua");
            director.clockFaceNormal=east;director.clockDialCentre=clock.transform.position+east*.184f+Vector3.up*1.78f;
            director.clockHour=clock.GetComponentsInChildren<Transform>().Single(t=>t.name.StartsWith("ClockHourBlade__"));director.clockMinute=clock.GetComponentsInChildren<Transform>().Single(t=>t.name.StartsWith("ClockMinuteBlade__"));director.clockDrawer=clock.GetComponentsInChildren<Transform>().Single(t=>t.name.StartsWith("ClockDrawer__"));
            var tick=clock.AddComponent<ClockAmbience>();tick.pendulum=clock.GetComponentsInChildren<Transform>().Single(t=>t.name.StartsWith("ClockPendulum__"));tick.pivot=new Vector3(0,1.4f,.15f);tick.axis=Vector3.forward;
            tick.source=clock.AddComponent<AudioSource>();tick.source.playOnAwake=false;tick.source.dopplerLevel=0;tick.source.spatialBlend=1;tick.source.minDistance=.8f;tick.source.maxDistance=7;tick.source.rolloffMode=AudioRolloffMode.Linear;tick.source.outputAudioMixerGroup=director.audioProfile.effectsBus;
            var stair=Box("LockedCentralStairDoor",P(-3.55f,7.7f,1.8f),new Vector3(1.25f,2.6f,.14f),wood);Item(stair,AtmajaAction.StairDoor,"Buka pintu tangga tengah");
            Box("StairHallWall_W",P(-5.04f,7.9f,2.1f),new Vector3(1.73f,3.2f,.16f),plaster);
            Box("StairHallWall_E",P(-.71f,7.9f,2.1f),new Vector3(4.43f,3.2f,.16f),plaster);
            Box("StairHallLintel",P(-3.55f,7.9f,3.4f),new Vector3(1.25f,.6f,.16f),plaster);
            director.arClue=BloodWriting.Build(environment,P(-5.0f,7.817f,2.0f),east,Vector3.up,-north);director.arClue.SetActive(false);
            director.arFigure=Model("ARSeatedAtmaja","AR_SeatedSriAtmaja",P(3.7f,2.15f,.52f));director.arFigure.transform.rotation=Facing(north);foreach(var t in director.arFigure.GetComponentsInChildren<Transform>(true))t.gameObject.layer=AtmajaPhoneView.ARLayer;director.arFigure.SetActive(false);
        }
        private static void Chair(string name,Vector3 where,Vector3 target){var c=Model("WornWoodChair",name,where);c.transform.rotation=Facing((target-where).normalized);Ground(c,where.y);FitCollider(c);}
        private static void Door(string name,Vector3 pivot,Vector3 center,Vector3 size,string label)
        {
            var h=new GameObject(name+"Hinge").transform;h.SetParent(environment,false);h.position=pivot;var leaf=Box(name,center,size,Mat("House_MAT_WoodDoor"));leaf.transform.SetParent(h,true);var d=leaf.AddComponent<HingedDoor>();d.hinge=h;d.label=label;d.angle=-95;
        }
        private static GameObject Existing(string id,string name,Vector3 center,Vector3 size)=>ExistingPath("Assets/_LayarTerakhir/Art/Imported/PolyHaven/"+id+"/"+id+".fbx",name,center,size);
        private static GameObject ExistingOriginal(string id,string name,Vector3 center,Vector3 size)=>ExistingPath("Assets/_LayarTerakhir/Art/Original/Models/"+id+".fbx",name,center,size);
        private static GameObject ExistingPath(string path,string name,Vector3 center,Vector3 size)
        {
            var root=new GameObject(name);root.transform.SetParent(environment,false);var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));model.transform.SetParent(root.transform,false);
            foreach(var r in root.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(Mat("House_MAT_WoodDoor"),r.sharedMaterials.Length).ToArray();KosVisualV2.Resize(root,size);KosVisualV2.SetCenter(root,center);FitCollider(root);return root;
        }
    }
}
#endif
