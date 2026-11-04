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
            var diningHinge=hinge.gameObject.AddComponent<LayarTerakhir.Prolog.HingedDoor>();diningHinge.hinge=hinge;diningHinge.angle=-95;diningHinge.label="Buka pintu ruang makan";
            var well=GameObject.CreatePrimitive(PrimitiveType.Cylinder);well.name="OldChainedKitchenWell";well.transform.SetParent(environment,false);well.transform.position=P(2.85f,11.2f,.72f);well.transform.localScale=new Vector3(1.05f,.36f,1.05f);well.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_BrickBase");Item(well,AtmajaAction.Well,"Periksa sumur tua");
            for(int i=0;i<4;i++)Box("RottenWellBoard",P(2.85f+(i-1.5f)*.23f,11.2f,1.09f),new Vector3(.21f,.045f,1.10f),wood);
            Chain(P(2.85f,11.2f,1.14f),1.1f,true);var bucket=Model("wooden_bucket_01","KitchenBucket",P(3.6f,11.9f,.35f));Ground(bucket,.35f);FitCollider(bucket);
            Existing("painted_wooden_cabinet_02","KitchenStorage",P(5.45f,11.7f,.88f),new Vector3(.8f,1.05f,.48f));
            var charger=Box("KitchenEmergencyCharger",P(5.76f,10.1f,1.35f),new Vector3(.15f,.20f,.09f),Mat("House_MAT_WoodFrameWhite"));Item(charger,AtmajaAction.Charge,"Isi daya darurat • dapur");
            // The bundled LongDiningTable.fbx carries a broken node transform, so the
            // visual table is the CC0 Poly Haven Dining Table (rustic wood + cloth),
            // matched to DiningTableTopCollision (surface y=1.42). Repair enforces
            // the same on saved scenes.
            var dtable=new GameObject("LongDiningTable");dtable.transform.SetParent(environment,false);dtable.transform.SetPositionAndRotation(P(3.7f,4.9f,.52f),Quaternion.identity);
            var dchild=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/dining_table/dining_table_1k.fbx"));dchild.transform.SetParent(dtable.transform,false);dchild.transform.rotation=Quaternion.Euler(0,90,0);
            KosVisualV2.Resize(dtable,new Vector3(1.5f,.9f,3.8f));KosVisualV2.SetCenter(dtable,P(3.7f,4.9f,.52f)+Vector3.up*.45f);
            var top=Box("DiningTableTopCollision",P(3.7f,4.9f,1.42f),new Vector3(1.5f,.075f,3.8f),wood);top.GetComponent<Renderer>().enabled=false;
            foreach(float x in new[]{3.09f,4.31f})foreach(float z in new[]{2.9f,6.9f})Box("DiningTableLegCollision",P(x,z,.95f),new Vector3(.09f,.86f,.09f),wood).GetComponent<Renderer>().enabled=false;
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
            var corner=new GameObject("CornerSideTable");corner.transform.SetParent(environment,false);corner.transform.SetPositionAndRotation(P(-5.25f,1.15f,.52f),Quaternion.identity);
            var cchild=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/wooden_table_02/wooden_table_02.fbx"));cchild.transform.SetParent(corner.transform,false);
            KosVisualV2.Resize(corner,new Vector3(.9f,.75f,.9f));KosVisualV2.SetCenter(corner,P(-5.25f,1.15f,.52f)+Vector3.up*.375f);FitCollider(corner);
            float cornerTop=KosVisualV2.BoundsOf(corner).max.y;
            director.clockHands=Box("BrassClockHands",new Vector3(corner.transform.position.x,cornerTop+.015f,corner.transform.position.z),new Vector3(.18f,.025f,.03f),Mat("Detail_AgedBrass"));Item(director.clockHands,AtmajaAction.ClockHands,"Ambil Jarum Jam Kuningan");
            var ashtray=GameObject.CreatePrimitive(PrimitiveType.Cylinder);ashtray.name="OldAshtray";ashtray.transform.SetParent(environment,false);ashtray.transform.position=new Vector3(P(-4.85f,1.1f,0).x,cornerTop+.008f,P(-4.85f,1.1f,0).z);ashtray.transform.localScale=new Vector3(.12f,.015f,.12f);ashtray.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_Concrete");Object.DestroyImmediate(ashtray.GetComponent<Collider>());
            Chair("FrontRoomWoodChair",P(-4.25f,.8f,.52f),P(-4.25f,2,.52f));Existing("wooden_bookshelf_worn","FrontRoomBookcase",P(-3.35f,.45f,1.25f),new Vector3(1.0f,1.45f,.35f));
            // Small storage room off the lounge, not another mandatory story key.
            Box("StorageWall",P(-.15f,6.65f,2.05f),new Vector3(.15f,3.1f,2.4f),plaster);
            Box("StorageFrontA",P(.15f,5.45f,2.05f),new Vector3(.7f,3.1f,.15f),plaster);
            Box("StorageFrontB",P(1.45f,5.45f,2.05f),new Vector3(.28f,3.1f,.15f),plaster);
            Box("StorageLintel",P(.85f,5.45f,3.15f),new Vector3(1.1f,.9f,.15f),plaster);
            Door("StorageDoor",P(.35f,5.45f,.52f),P(.85f,5.45f,1.62f),new Vector3(1,2.2f,.09f),"Buka ruang penyimpanan");
            Existing("painted_wooden_cabinet_02","LoungeStorage",P(.8f,7.25f,1.15f),new Vector3(.95f,1.25f,.45f));
            // A worker's CC0 crowbar hidden under the lower stair flight — pries
            // the rusted child-room vent. Repair ensures the same on saved scenes.
            var crowRoot=new GameObject("StorageCrowbar");crowRoot.transform.SetParent(environment,false);
            var crowChild=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/crowbar/crowbar_01_1k.fbx"));crowChild.transform.SetParent(crowRoot.transform,false);
            crowRoot.transform.SetPositionAndRotation(P(-.6f,7.9f,.56f),Quaternion.Euler(0,35,0));
            for(int fi=0;fi<2;fi++){var fb=KosVisualV2.BoundsOf(crowRoot);if(fb.size.y>fb.size.x&&fb.size.y>fb.size.z)crowRoot.transform.rotation=crowRoot.transform.rotation*Quaternion.Euler(90,0,0);else break;}
            crowRoot.transform.position+=Vector3.up*(.5f-KosVisualV2.BoundsOf(crowRoot).min.y);
            FitCollider(crowRoot);director.crowbar=crowRoot;Item(crowRoot,AtmajaAction.Crowbar,"Ambil linggis tua");LayarTerakhir.Player.PickupBlink.EnsureFor(crowRoot,"ITEM_P0_CROWBAR");
            // The bundled GrandfatherClock.fbx renders invisibly in play (its blades
            // + pendulum sit buried at the case base), so the case is the CC0 Poly
            // Haven Vintage Grandfather Clock 01 (2.2 m, dial + hands included;
            // Audit/grandfather-clock-cc0-sources.json). Hands are rebound by
            // renaming the skinned pivots to the ClockHourBlade__ convention.
            var clockPos=P(-5.58f,5.8f,.52f);
            var clock=new GameObject("GrandfatherClock");clock.transform.SetParent(environment,false);clock.transform.SetPositionAndRotation(clockPos,Facing(east));
            var clockChild=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/grandfather_clock/vintage_grandfather_clock_01_1k.fbx"));clockChild.transform.SetParent(clock.transform,false);
            FixClockNormalImport();
            KosVisualV2.SetCenter(clock,clockPos+Vector3.up*1.1f);
            var clockWood=Plain("ClockWoodCC0",Color.white);
            clockWood.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/grandfather_clock/vintage_grandfather_clock_01_diff_1k.jpg"));
            var clockNor=AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/grandfather_clock/vintage_grandfather_clock_01_nor_gl_1k.jpg");
            if(clockNor){clockWood.SetTexture("_BumpMap",clockNor);clockWood.SetFloat("_BumpScale",.55f);clockWood.EnableKeyword("_NORMALMAP");}
            var clockGlass=Plain("ClockGlassCC0",new Color(.85f,.88f,.92f,.09f));
            clockGlass.SetFloat("_Surface",1f);clockGlass.SetFloat("_Blend",0f);clockGlass.SetFloat("_SrcBlend",5f);clockGlass.SetFloat("_DstBlend",10f);clockGlass.SetFloat("_ZWrite",0f);
            clockGlass.SetFloat("_Smoothness",.9f);clockGlass.SetFloat("_Metallic",0f);
            clockGlass.DisableKeyword("_ALPHATEST_ON");clockGlass.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");clockGlass.renderQueue=3000;
            foreach(var r in clock.GetComponentsInChildren<Renderer>())
            {
                var cm=r.sharedMaterials;
                for(int ci=0;ci<cm.Length;ci++)
                {
                    if(!cm[ci])continue;
                    cm[ci]=cm[ci].name.IndexOf("glass",System.StringComparison.OrdinalIgnoreCase)>=0?clockGlass:clockWood;
                }
                r.sharedMaterials=cm;
            }
            var hourNode=clock.GetComponentsInChildren<Transform>().Single(t=>t.name=="vintage_grandfather_clock_01_HourHand");hourNode.name="ClockHourBlade__CC0";
            var minuteNode=clock.GetComponentsInChildren<Transform>().Single(t=>t.name=="vintage_grandfather_clock_01_MinuteHand");minuteNode.name="ClockMinuteBlade__CC0";
            director.clockFaceNormal=east;director.clockDialCentre=hourNode.position;
            director.clockHour=hourNode;director.clockMinute=minuteNode;
            var pendPivot=new GameObject("ClockPendulum__");pendPivot.transform.SetParent(clock.transform,false);pendPivot.transform.localPosition=new Vector3(0,1.45f,.05f);
            var rod=GameObject.CreatePrimitive(PrimitiveType.Cylinder);rod.name="PendulumRod";rod.transform.SetParent(pendPivot.transform,false);rod.transform.localPosition=new Vector3(0,-.475f,0);rod.transform.localScale=new Vector3(.02f,.95f,.02f);rod.GetComponent<Renderer>().sharedMaterial=Mat("Detail_AgedBrass");Object.DestroyImmediate(rod.GetComponent<Collider>());
            var bob=GameObject.CreatePrimitive(PrimitiveType.Cylinder);bob.name="PendulumBob";bob.transform.SetParent(pendPivot.transform,false);bob.transform.localPosition=new Vector3(0,-.95f,0);bob.transform.localRotation=Quaternion.Euler(90,0,0);bob.transform.localScale=new Vector3(.18f,.03f,.18f);bob.GetComponent<Renderer>().sharedMaterial=Mat("Detail_AgedBrass");Object.DestroyImmediate(bob.GetComponent<Collider>());
            var clockDrawerBox=GameObject.CreatePrimitive(PrimitiveType.Cube);clockDrawerBox.name="ClockDrawer__";clockDrawerBox.transform.SetParent(clock.transform,false);clockDrawerBox.transform.localPosition=new Vector3(0,.35f,.21f);clockDrawerBox.transform.localScale=new Vector3(.4f,.18f,.06f);clockDrawerBox.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_WoodDoor");Object.DestroyImmediate(clockDrawerBox.GetComponent<Collider>());
            director.clockDrawer=clockDrawerBox.transform;
            // FitCollider only reads MeshFilters (absent on this skinned CC0 rig),
            // so fit from the renderer bounds instead.
            var clockCol=clock.AddComponent<BoxCollider>();
            var clockBounds=KosVisualV2.BoundsOf(clock);
            clockCol.center=clock.transform.InverseTransformPoint(clockBounds.center);
            clockCol.size=clockBounds.size;
            Item(clock,AtmajaAction.Clock,"Periksa jam bandul tua");
            var tick=clock.AddComponent<ClockAmbience>();tick.pendulum=pendPivot.transform;tick.pivot=new Vector3(0,1.45f,.05f);tick.axis=Vector3.forward;
            tick.source=clock.AddComponent<AudioSource>();tick.source.playOnAwake=false;tick.source.dopplerLevel=0;tick.source.spatialBlend=1;tick.source.minDistance=.8f;tick.source.maxDistance=7;tick.source.rolloffMode=AudioRolloffMode.Linear;tick.source.outputAudioMixerGroup=director.audioProfile.effectsBus;
            var stair=Box("LockedCentralStairDoor",P(-3.55f,7.7f,1.8f),new Vector3(1.25f,2.6f,.14f),wood);Item(stair,AtmajaAction.StairDoor,"Buka pintu tangga tengah");
            Box("StairHallWall_W",P(-5.04f,7.9f,2.1f),new Vector3(1.73f,3.2f,.16f),plaster);
            Box("StairHallWall_E",P(-.71f,7.9f,2.1f),new Vector3(4.43f,3.2f,.16f),plaster);
            Box("StairHallLintel",P(-3.55f,7.9f,3.4f),new Vector3(1.25f,.6f,.16f),plaster);
            director.arClue=BloodWriting.Build(environment,P(-5.76f,4.6f,2.17f),north,Vector3.up,east);director.arClue.SetActive(false);
            director.arFigure=Model("ARSeatedAtmaja","AR_SeatedSriAtmaja",P(3.7f,2.15f,.52f));director.arFigure.transform.rotation=Facing(north);foreach(var t in director.arFigure.GetComponentsInChildren<Transform>(true))t.gameObject.layer=AtmajaPhoneView.ARLayer;director.arFigure.SetActive(false);
        }
        private static void FixClockNormalImport()
        {
            var ti=AssetImporter.GetAtPath("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/grandfather_clock/vintage_grandfather_clock_01_nor_gl_1k.jpg") as TextureImporter;
            if(ti&&ti.textureType!=TextureImporterType.NormalMap){ti.textureType=TextureImporterType.NormalMap;ti.SaveAndReimport();}
            var cti=AssetImporter.GetAtPath("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/crowbar/crowbar_01_nor_gl_1k.jpg") as TextureImporter;
            if(cti&&cti.textureType!=TextureImporterType.NormalMap){cti.textureType=TextureImporterType.NormalMap;cti.SaveAndReimport();}
            var vti=AssetImporter.GetAtPath("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/metal_grate_rusty/metal_grate_rusty_nor_gl_1k.jpg") as TextureImporter;
            if(vti&&vti.textureType!=TextureImporterType.NormalMap){vti.textureType=TextureImporterType.NormalMap;vti.SaveAndReimport();}
        }
        private static void Chair(string name,Vector3 where,Vector3 target)
        {
            // WornWoodChair.fbx shares the 13m house-shell mesh (renders as plank
            // walls boxing the table): build the chair procedurally instead.
            var c=new GameObject(name);c.transform.SetParent(environment,false);
            c.transform.SetPositionAndRotation(where,Facing((target-where).normalized));
            var wood=Plain("RepairDiningChairWood",new Color(.32f,.22f,.13f),.6f);
            void Part(Vector3 lp,Vector3 s){var p=GameObject.CreatePrimitive(PrimitiveType.Cube);p.transform.SetParent(c.transform,false);p.transform.localPosition=lp;p.transform.localScale=s;p.GetComponent<Renderer>().sharedMaterial=wood;}
            Part(new Vector3(0,.45f,0),new Vector3(.46f,.07f,.46f));
            Part(new Vector3(0,.72f,-.225f),new Vector3(.46f,.55f,.07f));
            foreach(float lx in new[]{-.19f,.19f})foreach(float lz in new[]{-.19f,.19f})Part(new Vector3(lx,.225f,lz),new Vector3(.07f,.45f,.07f));
            Ground(c,where.y);FitCollider(c);
        }
        private static void Door(string name,Vector3 pivot,Vector3 center,Vector3 size,string label)
        {
            var h=new GameObject(name+"Hinge").transform;h.SetParent(environment,false);h.position=pivot;var leaf=Box(name,center,size,Mat("House_MAT_WoodDoor"));leaf.transform.SetParent(h,true);var d=leaf.AddComponent<HingedDoor>();d.hinge=h;d.label=label;d.angle=-95;
        }
        private static GameObject Existing(string id,string name,Vector3 center,Vector3 size)=>ExistingPath("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/"+id+"/"+id+".fbx",name,center,size);
        private static GameObject ExistingOriginal(string id,string name,Vector3 center,Vector3 size)=>ExistingPath("Assets/_LayarTerakhir/assets/Art/Original/Models/"+id+".fbx",name,center,size);
        private static GameObject ExistingPath(string path,string name,Vector3 center,Vector3 size)
        {
            var root=new GameObject(name);root.transform.SetParent(environment,false);var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));model.transform.SetParent(root.transform,false);
            foreach(var r in root.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(Mat("House_MAT_WoodDoor"),r.sharedMaterials.Length).ToArray();KosVisualV2.Resize(root,size);KosVisualV2.SetCenter(root,center);FitCollider(root);return root;
        }
    }
}
#endif
