#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static partial class KosCorridorBuilder
    {
        public const string ScenePath="Assets/_LayarTerakhir/assets/Scenes/Prolog_KoridorKos.unity";
        private static Transform root;private static KosCorridorDirector d;private static Material wall,floor,wood,metal,doorMat;
        public static void Build()
        {
            var golden=EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);var systems=GameObject.Find("GameSystems");var player=Object.FindFirstObjectByType<FirstPersonController>();var events=Object.FindFirstObjectByType<EventSystem>();var profile=Object.FindFirstObjectByType<Volume>().sharedProfile;
            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);SceneManager.SetActiveScene(scene);
            var sys=Object.Instantiate(systems);sys.name="GameSystems";var p=Object.Instantiate(player.gameObject);p.name="Player_RakaWijaya";Object.DestroyImmediate(sys.GetComponent<KamarKosPrologDirector>());var old=sys.GetComponentInChildren<KosSceneAudio>();if(old)Object.DestroyImmediate(old.gameObject);
            if(events&&!events.transform.IsChildOf(systems.transform)){var ev=Object.Instantiate(events.gameObject);ev.name="EventSystem";}
            EditorSceneManager.CloseScene(golden,true);
            d=sys.AddComponent<KosCorridorDirector>();d.player=p.GetComponent<FirstPersonController>();d.audioProfile=AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/assets/Audio/KosAudioProfile.asset");
            root=new GameObject("Kos_Building_Corridors").transform;
            wall=Material("KosCorridorPlaster",new Color(.64f,.62f,.53f),"Assets/_LayarTerakhir/assets/Art/Generated/Materials/OldLimewash.mat");
            floor=Material("KosCorridorTile",new Color(.55f,.54f,.48f),"Assets/_LayarTerakhir/assets/Art/Generated/Materials/CeramicTiles.mat");
            wood=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/House_MAT_WoodDoor.mat");metal=Material("KosCorridorMetal",new Color(.07f,.08f,.085f));
            // Pintu kost modern (panel datar + kusen gelap), bukan kayu renyot.
            doorMat=Material("KosModernDoor",new Color(.24f,.28f,.31f));doorMat.SetFloat("_Smoothness",.5f);EditorUtility.SetDirty(doorMat);
            Box("UpperHallFloor",new Vector3(0,3.23f,6),new Vector3(2.8f,.14f,14),floor);
            Box("GroundHallFloor",new Vector3(0,-.07f,6),new Vector3(2.8f,.14f,14),floor);
            Box("UpperHallCeiling",new Vector3(0,6.2f,6),new Vector3(2.9f,.13f,14),wall);
            Box("UpperHallFrontWall",new Vector3(0,4.75f,-1),new Vector3(3.1f,2.9f,.18f),wall);
            foreach(float x in new[]{-1.5f,1.5f})
            {
                HallWallWithDoorways(x,3.3f);
                HallWallWithDoorways(x,0);
                Box("UpperHallSkirting",new Vector3(x-Mathf.Sign(x)*.11f,3.4f,6),new Vector3(.045f,.18f,14),metal);
                for(int i=0;i<4;i++)
                {
                    float z=1.5f+i*3;
                    ClosedDoor("KosUpperDoor_"+x+"_"+i,new Vector3(x-Mathf.Sign(x)*.13f,4.38f,z),(x<0?21:25)+i,x<0?Vector3.right:Vector3.left,i==0&&x<0);
                    if(i!=1)ClosedDoor("KosGroundDoor_"+x+"_"+i,new Vector3(x-Mathf.Sign(x)*.13f,1.08f,z),(x<0?1:5)+i,x<0?Vector3.right:Vector3.left,false);
                }
            }
            Box("HallRearWall",new Vector3(0,3.1f,17.2f),new Vector3(5.2f,6.2f,.18f),wall);
            Box("StairWestWall",new Vector3(-1.55f,3.1f,14.5f),new Vector3(.18f,6.2f,5.4f),wall);
            Box("StairEastWall",new Vector3(2.65f,3.1f,14.5f),new Vector3(.18f,6.2f,5.4f),wall);
            Box("StairwellCeiling",new Vector3(.55f,6.2f,14.5f),new Vector3(4.2f,.13f,5.4f),wall);
            Box("TopLanding",new Vector3(.55f,3.23f,12.3f),new Vector3(4.1f,.14f,1.4f),floor);
            Box("MiddleLanding",new Vector3(.55f,1.58f,16.45f),new Vector3(4.1f,.14f,1.3f),floor);
            Box("BottomLanding",new Vector3(.55f,-.07f,12.3f),new Vector3(4.1f,.14f,1.4f),floor);
            for(int i=0;i<10;i++)
            {
                float topA=3.3f-(i+1)*.165f,topB=1.65f-(i+1)*.165f;
                Box("StairA_"+i,new Vector3(-.60f,topA*.5f,13.14f+i*.28f),new Vector3(1.2f,Mathf.Max(.025f,topA),.28f),floor);
                Box("StairB_"+i,new Vector3(1.75f,topB*.5f,15.66f-i*.28f),new Vector3(1.2f,Mathf.Max(.025f,topB),.28f),floor);
            }
            Beam("StairHandrailA",new Vector3(-1.25f,4.2f,13),new Vector3(-1.25f,2.55f,15.8f),.045f);
            Beam("StairHandrailB",new Vector3(2.4f,2.55f,15.8f),new Vector3(2.4f,.9f,13),.045f);
            for(int i=0;i<5;i++)
            {
                float t=i/4f;float z=Mathf.Lerp(13,15.8f,t),y=Mathf.Lerp(3.3f,1.65f,t);
                Beam("StairRailPostA",new Vector3(-1.25f,y,z),new Vector3(-1.25f,y+.9f,z),.038f);
                Beam("StairRailPostB",new Vector3(2.4f,1.65f-y+1.65f,z),new Vector3(2.4f,1.65f-y+2.55f,z),.038f);
            }
            SharedRoom(-1,"DAPUR BERSAMA");SharedRoom(1,"KAMAR MANDI");
            CompleteBuilding();
            // Parkiran diperluas + diisi motor tetangga, bukan cuma motor Raka.
            Box("KosYard",new Vector3(0,-.08f,-5),new Vector3(16f,.16f,10f),floor);
            Box("KosStreet",new Vector3(0,-.12f,-12),new Vector3(16f,.2f,8f),AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/dirty_concrete.mat"));
            foreach(float x in new[]{-8f,8f})Box("YardBoundary",new Vector3(x,1.3f,-4),new Vector3(.2f,2.6f,8),wall);
            foreach(float x in new[]{-5.8f,5.8f})Box("KosGateBoundary",new Vector3(x,1.3f,-7),new Vector3(4.4f,2.6f,.22f),wall);
            var gate=Model("large_iron_gate","KosGate",new Vector3(2.8f,2.5f,.13f),new Vector3(0,1.25f,-7));var hinge=new GameObject("KosGateHinge").transform;hinge.SetParent(root,false);hinge.position=new Vector3(-1.4f,0,-7);gate.transform.SetParent(hinge,true);d.gateHinge=hinge;
            var g=gate.AddComponent<KosTravelItem>();g.director=d;g.label="Pilih tujuan di gerbang kos";
            var motor=Model("DamarScooter","RakaMotor",Vector3.zero,Vector3.zero);motor.transform.rotation=Quaternion.Euler(0,180,0);KosVisualV2.SetCenter(motor,new Vector3(2.25f,.56f,-4.5f));d.motor=motor.transform;
            // Motor tetangga: parkiran terasa seperti kost beneran, jalur tengah tetap lega.
            var parked=new[]{new Vector3(-5.2f,.56f,-4.2f),new Vector3(-3.4f,.56f,-4.8f),new Vector3(4.4f,.56f,-4.3f),new Vector3(6.0f,.56f,-4.9f)};
            for(int bi=0;bi<parked.Length;bi++)
            {
                var neighbour=Model("DamarScooter","TenantMotor_"+bi,Vector3.zero,Vector3.zero);
                neighbour.transform.rotation=Quaternion.Euler(0,180+(bi%2==0?-8:8),0);KosVisualV2.SetCenter(neighbour,parked[bi]);
                var coverTint=new Color(.16f+.03f*bi,.18f,.20f);var coverMat=Material("TenantMotorCover_"+bi,coverTint);
                foreach(var r in neighbour.GetComponentsInChildren<Renderer>())if(r.name.Contains("Body")||r.name.Contains("Cover"))r.sharedMaterials=Enumerable.Repeat(coverMat,r.sharedMaterials.Length).ToArray();
            }
            var m=motor.AddComponent<KosTravelItem>();m.director=d;m.label="Naik motor • pilih tujuan";
            var lamp=new GameObject("RakaMotorHeadlight",typeof(Light));lamp.transform.SetParent(motor.transform,false);
            var lens=motor.GetComponentsInChildren<Renderer>().First(r=>r.sharedMaterial&&r.sharedMaterial.name.Contains("HeadlightLens"));
            lamp.transform.position=lens.bounds.center+motor.transform.forward*.04f;lamp.transform.rotation=motor.transform.rotation;
            d.motorHeadlight=lamp.GetComponent<Light>();d.motorHeadlight.type=LightType.Spot;d.motorHeadlight.range=18;d.motorHeadlight.spotAngle=65;d.motorHeadlight.innerSpotAngle=45;d.motorHeadlight.intensity=5;d.motorHeadlight.color=new Color(.95f,.87f,.69f);d.motorHeadlight.shadows=LightShadows.None;d.motorHeadlight.enabled=false;
            var ride=new GameObject("RakaRideEye").transform;ride.SetParent(motor.transform,true);ride.position=motor.transform.position+Vector3.up*1.4f;ride.rotation=motor.transform.rotation;d.rideAnchor=ride;
            d.rideExit=Anchor("KosStreetDeparture",new Vector3(0,motor.transform.position.y,-12),Vector3.back);
            d.spawnLanding=Anchor("Spawn_KosFloor2",new Vector3(-.70f,3.34f,1.5f),Vector3.forward);d.spawnGround=Anchor("Spawn_KosGround",new Vector3(0,.04f,1),Vector3.back);
            p.transform.SetPositionAndRotation(d.spawnLanding.position,d.spawnLanding.rotation);var camera=p.GetComponent<InteractionInteractor>().playerCamera;camera.backgroundColor=new Color(.018f,.025f,.035f);camera.farClipPlane=50;
            RenderSettings.skybox=null;RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.08f,.085f,.095f);RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogDensity=.016f;RenderSettings.fogColor=new Color(.018f,.025f,.035f);
            for(int level=0;level<2;level++)foreach(float z in new[]{2f,8f,13.5f})Practical(new Vector3(0,2.65f+level*3.3f,z),new Color(.75f,.83f,.76f),2.8f,6.2f);
            Practical(new Vector3(0,3.3f,-.5f),new Color(.88f,.72f,.45f),1.6f,6);
            Box("StairWallLampHousing",new Vector3(-1.39f,2.85f,16.3f),new Vector3(.08f,.32f,.24f),metal);
            var diffuser=Material("KosLampDiffuser",new Color(.73f,.78f,.69f));diffuser.EnableKeyword("_EMISSION");diffuser.SetColor("_EmissionColor",new Color(.73f,.78f,.69f)*1.2f);EditorUtility.SetDirty(diffuser);
            Box("StairWallLampDiffuser",new Vector3(-1.335f,2.85f,16.3f),new Vector3(.02f,.25f,.18f),diffuser);
            var stairLamp=new GameObject("StairWallLampLight",typeof(Light));stairLamp.transform.SetParent(root,false);stairLamp.transform.position=new Vector3(-1.25f,2.85f,16.3f);var sl=stairLamp.GetComponent<Light>();sl.type=LightType.Point;sl.intensity=2.8f;sl.range=5;sl.color=new Color(.83f,.86f,.76f);sl.shadows=LightShadows.None;
            var volume=new GameObject("KosCorridorPost",typeof(Volume)).GetComponent<Volume>();volume.isGlobal=true;volume.sharedProfile=profile;
            d.engine=Source("RakaMotorEngine",d.audioProfile.effectsBus,1);d.engine.transform.position=motor.transform.position;d.rain=Source("KosYardRain",d.audioProfile.ambienceBus,0);d.rain.clip=d.audioProfile.rain;d.rain.loop=true;d.rain.volume=.25f;
            var footsteps=sys.AddComponent<KosSceneAudio>();footsteps.profile=d.audioProfile;footsteps.player=d.player;
            footsteps.stepsSource=Source("KosHallFootsteps",d.audioProfile.effectsBus,.8f);footsteps.uiSource=Source("KosHallHandsetSounds",d.audioProfile.uiBus,0);
            // Unity bakes static batches at build time; moving doors/motor/gate stay unbatched.
            foreach(var renderer in root.GetComponentsInChildren<MeshRenderer>())
                if(!renderer.GetComponentInParent<HingedDoor>()&&!renderer.GetComponentInParent<KosTravelItem>())
                    GameObjectUtility.SetStaticEditorFlags(renderer.gameObject,StaticEditorFlags.BatchingStatic);
            DesignerFacilities();
            EditorSceneManager.SaveScene(scene,ScenePath);AssetDatabase.SaveAssets();
            EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(PrologKosSceneBuilder.ScenePath,true),new EditorBuildSettingsScene(AtmajaSceneBuilder.ScenePath,true),new EditorBuildSettingsScene(ScenePath,true)};
            Debug.Log("KOS_CORRIDOR_BUILD two floors, U stairs, closed rooms, shared kitchen/bath, gate and ride");
        }
        private static Material Material(string name,Color color,string source=null)
        {
            string path="Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);var original=source==null?null:AssetDatabase.LoadAssetAtPath<Material>(source);if(!m){m=original?new Material(original):new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}else if(original)m.CopyPropertiesFromMaterial(original);m.SetColor("_BaseColor",color);EditorUtility.SetDirty(m);return m;
        }
        private static GameObject Box(string name,Vector3 position,Vector3 size,Material mat)
        {
            var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.transform.SetParent(root,false);o.transform.position=position;o.transform.localScale=size;var renderer=o.GetComponent<Renderer>();renderer.sharedMaterial=mat;
            if(mat==floor||mat==wall)
            {
                var block=new MaterialPropertyBlock();float u=size.x<size.z&&size.x<size.y?size.z:size.x;float v=size.y<size.z&&size.y<size.x?size.z:size.y;
                block.SetVector("_BaseMap_ST",new Vector4(u/.45f,v/.45f,0,0));renderer.SetPropertyBlock(block);
            }
            return o;
        }
        private static void Beam(string name,Vector3 a,Vector3 b,float width){var o=Box(name,(a+b)*.5f,new Vector3(width,(b-a).magnitude,width),metal);o.transform.rotation=Quaternion.FromToRotation(Vector3.up,(b-a).normalized);}
        private static Transform Anchor(string name,Vector3 p,Vector3 facing){var t=new GameObject(name).transform;t.SetParent(d.transform,false);t.SetPositionAndRotation(p,Quaternion.LookRotation(facing));return t;}
        private static GameObject Model(string id,string name,Vector3 size,Vector3 center)
        {
            var o=new GameObject(name);o.transform.SetParent(root,false);var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/"+id+".fbx"));model.transform.SetParent(o.transform,false);
            foreach(var r in o.GetComponentsInChildren<Renderer>()){string n=r.name;var mat=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/"+n.Substring(n.IndexOf("__",System.StringComparison.Ordinal)+2)+".mat");r.sharedMaterials=Enumerable.Repeat(mat?mat:wood,r.sharedMaterials.Length).ToArray();}
            if(size!=Vector3.zero)KosVisualV2.Resize(o,size);KosVisualV2.SetCenter(o,center);KosVisualV2.FitBox(o.AddComponent<BoxCollider>(),KosVisualV2.BoundsOf(o));return o;
        }
        private static void ClosedDoor(string name,Vector3 center,int number,Vector3 normal,bool raka)
        {
            // Daun pintu kost modern: panel datar + kick plate, bukan kayu lapuk.
            var leaf=Box(name,center,new Vector3(.09f,2.1f,.90f),doorMat??wood);var door=leaf.AddComponent<HingedDoor>();door.hinge=leaf.transform;door.locked=true;
            Box(name+"InsetTop",center+Vector3.up*.55f,new Vector3(.02f,.7f,.66f),metal);
            Box(name+"KickPlate",center-Vector3.up*.85f,new Vector3(.02f,.25f,.8f),metal);
            foreach(float z in new[]{-.48f,.48f})Box(name+"Frame",center+Vector3.forward*z,new Vector3(.13f,2.18f,.055f),metal);
            Box(name+"Lintel",center+Vector3.up*1.07f,new Vector3(.13f,.07f,.99f),metal);
            Box(name+"Handle",center+normal*.07f+Vector3.forward*.30f,new Vector3(.045f,.08f,.04f),metal);
            Label(name+"Number",(raka?"RAKA / ":"")+number,center+normal*.06f+Vector3.up*.5f,normal,.15f);
        }
        private static void Label(string name,string words,Vector3 pos,Vector3 normal,float scale)
        {var go=new GameObject(name,typeof(TextMesh));go.transform.SetParent(root,false);go.transform.SetPositionAndRotation(pos,Quaternion.LookRotation(-normal));var text=go.GetComponent<TextMesh>();text.font=AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf");text.text=words;text.anchor=TextAnchor.MiddleCenter;text.characterSize=scale/10;text.fontSize=40;text.color=new Color(.75f,.73f,.63f);go.GetComponent<MeshRenderer>().sharedMaterial=text.font.material;}
        private static void SharedRoom(int side,string title)
        {
            float x=side*3.15f;
            var bathTile=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/BathFloor_Tiled.mat");
            var diningTile=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/DiningDirtyTile_Tiled.mat");
            Material roomFloor=side<0?(diningTile?diningTile:floor):(bathTile?bathTile:floor);
            Box(title+"Floor",new Vector3(x,-.06f,4.25f),new Vector3(3.2f,.12f,3.5f),roomFloor);
            Box(title+"Ceiling",new Vector3(x,2.9f,4.25f),new Vector3(3.4f,.10f,3.6f),wall);
            Box(title+"Back",new Vector3(side*4.8f,1.4f,4.25f),new Vector3(.15f,2.8f,3.6f),wall);
            // Dinding keramik setengah badan agar tidak polos kotak semen.
            Box(title+"TiledWainscot",new Vector3(side*4.7f,.55f,4.25f),new Vector3(.04f,1.1f,3.5f),roomFloor);
            foreach(float z in new[]{2.5f,6f})Box(title+"Side",new Vector3(x,1.4f,z),new Vector3(3.4f,2.8f,.15f),wall);
            if(side>0)
            {
                Box(title+"FrontLow",new Vector3(side*1.5f,1.4f,3.0f),new Vector3(.15f,2.8f,1.0f),wall);Box(title+"FrontHigh",new Vector3(side*1.5f,1.4f,5.5f),new Vector3(.15f,2.8f,1.0f),wall);Box(title+"Lintel",new Vector3(side*1.5f,2.55f,4.25f),new Vector3(.15f,.5f,1.5f),wall);
                var hinge=new GameObject(title+"Hinge").transform;hinge.SetParent(root,false);hinge.position=new Vector3(side*1.5f,0,3.5f);var leaf=Box(title+"Door",new Vector3(side*1.5f,1.07f,4.25f),new Vector3(.09f,2.14f,1.5f),doorMat??wood);leaf.transform.SetParent(hinge,true);var door=leaf.AddComponent<HingedDoor>();door.hinge=hinge;door.label="Buka "+title;door.angle=side*90;
            }
            else
            {
                // Dapur bersama terbuka tanpa pintu: bukaan penuh + header, daun
                // diparkir terbuka agar tes lama (DAPUR BERSAMADoor) tetap valid.
                Box(title+"Lintel",new Vector3(side*1.5f,2.62f,4.25f),new Vector3(.15f,.36f,3.5f),wall);
                foreach(float z in new[]{2.65f,5.85f})Box(title+"Jamb",new Vector3(side*1.5f,1.4f,z),new Vector3(.15f,2.8f,.3f),wall);
                var hinge=new GameObject(title+"Hinge").transform;hinge.SetParent(root,false);hinge.position=new Vector3(side*1.5f,0,2.75f);var leaf=Box(title+"Door",new Vector3(side*1.5f,1.07f,3.5f),new Vector3(.09f,2.14f,1.5f),doorMat??wood);leaf.transform.SetParent(hinge,true);var door=leaf.AddComponent<HingedDoor>();door.hinge=hinge;door.label="Buka "+title;door.angle=-95;door.SetOpenImmediate(true);
            }
            Label(title+"Sign",title,new Vector3(side*1.38f,2.44f,4.25f),side<0?Vector3.right:Vector3.left,.12f);
            Practical(new Vector3(x,2.72f,4.25f),new Color(.83f,.84f,.73f),1.1f,4);
            if(side<0)
            {
                // Dapur kost lengkap: counter + kompor + bak cuci + tabung gas + rak piring.
                Box("SharedKitchenCounter",new Vector3(-4.2f,.45f,4.5f),new Vector3(.7f,.9f,2.3f),doorMat??wood);
                Box("KitchenCounterTop",new Vector3(-4.2f,.92f,4.5f),new Vector3(.78f,.05f,2.4f),metal);
                Box("Stove",new Vector3(-4.2f,.97f,4.3f),new Vector3(.6f,.06f,.9f),metal);
                foreach(float bz in new[]{4.1f,4.5f}){var burner=GameObject.CreatePrimitive(PrimitiveType.Cylinder);burner.name="StoveBurner";burner.transform.SetParent(root,false);burner.transform.position=new Vector3(-4.2f,1.01f,bz);burner.transform.localScale=new Vector3(.18f,.02f,.18f);burner.GetComponent<Renderer>().sharedMaterial=metal;Object.DestroyImmediate(burner.GetComponent<Collider>());}
                var pot=GameObject.CreatePrimitive(PrimitiveType.Cylinder);pot.name="KitchenPot";pot.transform.SetParent(root,false);pot.transform.position=new Vector3(-4.2f,1.12f,4.1f);pot.transform.localScale=new Vector3(.28f,.16f,.28f);pot.GetComponent<Renderer>().sharedMaterial=metal;Object.DestroyImmediate(pot.GetComponent<Collider>());
                Box("KitchenSink",new Vector3(-4.2f,.97f,5.2f),new Vector3(.5f,.08f,.5f),metal);
                Box("KitchenSinkBasin",new Vector3(-4.2f,.94f,5.2f),new Vector3(.4f,.06f,.4f),wall);
                var gas=GameObject.CreatePrimitive(PrimitiveType.Cylinder);gas.name="KitchenGasTube";gas.transform.SetParent(root,false);gas.transform.position=new Vector3(-4.2f,.35f,3.6f);gas.transform.localScale=new Vector3(.28f,.7f,.28f);var gasMat=Material("KosGasGreen",new Color(.12f,.38f,.18f));gas.GetComponent<Renderer>().sharedMaterial=gasMat;Object.DestroyImmediate(gas.GetComponent<Collider>());
                Box("KitchenDishRack",new Vector3(-2.4f,1.1f,5.6f),new Vector3(.9f,.04f,.4f),doorMat??wood);
                for(int p=0;p<3;p++){var plate=GameObject.CreatePrimitive(PrimitiveType.Cylinder);plate.name="KitchenRackPlate";plate.transform.SetParent(root,false);plate.transform.position=new Vector3(-2.65f+p*.25f,1.18f,5.6f);plate.transform.localScale=new Vector3(.18f,.015f,.18f);plate.transform.rotation=Quaternion.Euler(0,0,75);plate.GetComponent<Renderer>().sharedMaterial=wall;Object.DestroyImmediate(plate.GetComponent<Collider>());}
                Model("wooden_bucket_01","SharedWaterBucket",new Vector3(.35f,.45f,.35f),new Vector3(-2.1f,.225f,5.4f));
                var dipper=GameObject.CreatePrimitive(PrimitiveType.Cylinder);dipper.name="KitchenGayung";dipper.transform.SetParent(root,false);dipper.transform.position=new Vector3(-2.1f,.5f,5.4f);dipper.transform.localScale=new Vector3(.12f,.1f,.12f);dipper.GetComponent<Renderer>().sharedMaterial=metal;Object.DestroyImmediate(dipper.GetComponent<Collider>());
            }
            else
            {
                // Kamar mandi lengkap: toren + closet jongkok + cermin + gayung + shower.
                Box("BathroomWaterTank",new Vector3(4.1f,.55f,5.25f),new Vector3(.9f,1.1f,.9f),roomFloor);
                Box("BathroomTankLid",new Vector3(4.1f,1.12f,5.25f),new Vector3(.95f,.05f,.95f),metal);
                Box("BathroomMirror",new Vector3(4.75f,1.6f,3.0f),new Vector3(.03f,.5f,.4f),metal);
                Box("SquatToiletBase",new Vector3(3.1f,.09f,3.3f),new Vector3(.55f,.18f,.7f),roomFloor);var bowl=GameObject.CreatePrimitive(PrimitiveType.Cylinder);bowl.name="ToiletBowl";bowl.transform.SetParent(root,false);bowl.transform.position=new Vector3(3.1f,.19f,3.3f);bowl.transform.localScale=new Vector3(.25f,.015f,.35f);bowl.GetComponent<Renderer>().sharedMaterial=metal;
                Model("wooden_bucket_01","BathroomEmber",new Vector3(.35f,.4f,.35f),new Vector3(3.9f,.2f,4.4f));
            }
        }
        private static void Practical(Vector3 pos,Color color,float intensity,float range)
        {
            Box("VisibleFluorescentFixture",pos,new Vector3(.55f,.08f,.16f),metal);Box("VisibleLampDiffuser",pos-Vector3.up*.05f,new Vector3(.49f,.015f,.13f),Material("KosLampDiffuser",new Color(.73f,.78f,.69f)));
            var go=new GameObject("KosPracticalLight",typeof(Light));go.transform.SetParent(root,false);go.transform.position=pos-Vector3.up*.10f;var l=go.GetComponent<Light>();l.type=LightType.Point;l.intensity=intensity;l.range=range;l.color=color;l.shadows=LightShadows.None;
        }
        private static AudioSource Source(string name,UnityEngine.Audio.AudioMixerGroup bus,float spatial){var go=new GameObject(name,typeof(AudioSource));go.transform.SetParent(d.transform,false);var a=go.GetComponent<AudioSource>();a.outputAudioMixerGroup=bus;a.playOnAwake=false;a.dopplerLevel=0;a.spatialBlend=spatial;return a;}
    }
}
#endif
