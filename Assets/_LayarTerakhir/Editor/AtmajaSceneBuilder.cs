#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class AtmajaSceneBuilder
    {
        public const string ScenePath="Assets/_LayarTerakhir/Scenes/Prolog_RumahAtmaja.unity";
        private const string Art="Assets/_LayarTerakhir/Art/Atmaja/";
        [Serializable] public class MaterialRecord{public string name,albedo,normal,mask;public float[] color;public float roughness,metallic;public bool alphaClip;}
        [Serializable] public class Manifest{public MaterialRecord[] materials;public string protectedGoldenSceneSha256;}
        private static Manifest manifest;
        internal static Transform environment,house;
        internal static AtmajaDirector director;
        internal static Vector3 origin,east,north;
        internal static Vector3 P(float x,float depth,float y)=>origin+east*x+north*depth+Vector3.up*y;
        internal static Quaternion Facing(Vector3 dir)=>Quaternion.LookRotation(dir,Vector3.up);
        private static Transform Named(Transform root,string name)=>root.GetComponentsInChildren<Transform>(true).Single(t=>t.name==name);

        [MenuItem("LayarTerakhir/Prolog/Build Atmaja continuation (preserve Golden Kos)")]
        public static void Build()
        {
            manifest=JsonUtility.FromJson<Manifest>(File.ReadAllText("Audit/atmaja-art-manifest.json"));
            Directory.CreateDirectory(Art+"Materials");AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);Imports();
            var golden=EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var sourcePlayer=Object.FindFirstObjectByType<FirstPersonController>();
            var sourceSystems=GameObject.Find("GameSystems");
            var sourceEvents=Object.FindFirstObjectByType<EventSystem>();
            var profile=Object.FindFirstObjectByType<Volume>().sharedProfile;
            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);SceneManager.SetActiveScene(scene);
            var systems=Object.Instantiate(sourceSystems);systems.name="GameSystems";
            var player=Object.Instantiate(sourcePlayer.gameObject);player.name="Player_RakaWijaya";
            Object.DestroyImmediate(systems.GetComponent<KamarKosPrologDirector>());
            var oldAudio=systems.GetComponentInChildren<KosSceneAudio>();if(oldAudio)Object.DestroyImmediate(oldAudio.gameObject);
            if(sourceEvents&&!sourceEvents.transform.IsChildOf(sourceSystems.transform)){var ev=Object.Instantiate(sourceEvents.gameObject);ev.name="EventSystem";}
            EditorSceneManager.CloseScene(golden,true); // Never save/recompose the approved room.
            director=systems.AddComponent<AtmajaDirector>();director.player=player.GetComponent<FirstPersonController>();
            director.player.crouchHeight=.78f;director.player.cameraCrouchY=.62f; // Under-table crawl, scene-local tuning.
            director.audioProfile=AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/Audio/KosAudioProfile.asset");
            environment=new GameObject("Atmaja_Environment_Art").transform;
            house=Model("RumahAtmaja","RumahAtmaja_House",Vector3.zero).transform;
            origin=Named(house,"AXIS_Origin").position;
            east=(Named(house,"AXIS_East").position-origin).normalized;
            north=(Named(house,"AXIS_North").position-origin).normalized;
            Debug.Log("ATMAJA_AXES origin="+origin+" east="+east+" north="+north);
            director.origin=origin;director.east=east;director.north=north;
            foreach(var marker in house.GetComponentsInChildren<Transform>().Where(t=>t.name.StartsWith("COL_")))
            {
                // FBX empty-node scale is in centimetres while its imported translation is in metres.
                // Uniform 1cm local size reproduces the source hull, including its rotated axes.
                var c=marker.gameObject.AddComponent<BoxCollider>();c.size=Vector3.one*.01f;
            }
            ShellAndGarden();ExteriorStoryObjects();AtmajaReviewDressing.Build();Lighting(profile);Audio();
            director.spawnCourtyard=Anchor("Spawn_Courtyard",P(0,-12.2f,.06f),P(0,1,3.2f));
            director.spawnKitchen=Anchor("Spawn_Kitchen",P(5.0f,11.0f,.37f),P(3.0f,10.2f,1.3f));
            director.spawnDining=Anchor("Spawn_Dining",P(4.8f,7.7f,.53f),P(3.6f,5.7f,1.4f));
            director.spawnStairs=Anchor("Spawn_Stairs",P(-2.3f,5.0f,.53f),P(-4.8f,6.0f,1.5f));
            player.transform.SetPositionAndRotation(director.spawnCourtyard.position,director.spawnCourtyard.rotation);
            director.player.cameraPivot.localRotation=Quaternion.identity;
            var camera=player.GetComponent<InteractionInteractor>().playerCamera;camera.transform.localRotation=Quaternion.identity;camera.farClipPlane=65;camera.backgroundColor=RenderSettings.fogColor;
            var view=systems.AddComponent<AtmajaPhoneView>();view.director=director;view.playerCamera=camera;director.phoneView=view;
            string feedPath=Art+"Materials/PhoneFeed.mat";view.feedMaterial=AssetDatabase.LoadAssetAtPath<Material>(feedPath);
            if(!view.feedMaterial){view.feedMaterial=new Material(Shader.Find("LayarTerakhir/AtmajaPhoneFeed"));AssetDatabase.CreateAsset(view.feedMaterial,feedPath);}
            var vault=player.AddComponent<AtmajaVault>();vault.director=director;
            var volume=new GameObject("AtmajaPostProcessing",typeof(Volume)).GetComponent<Volume>();volume.isGlobal=true;volume.sharedProfile=profile;
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene,ScenePath);AssetDatabase.SaveAssets();
            EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(PrologKosSceneBuilder.ScenePath,true),new EditorBuildSettingsScene(ScenePath,true)};
            Debug.Log("ATMAJA_BUILD Saved exterior + ground floor; approved Golden scene not written.");
        }
        private static void Imports()
        {
            foreach(string path in Directory.GetFiles(Art+"Textures"))
            {
                if(path.EndsWith(".meta"))continue;
                var ti=AssetImporter.GetAtPath(path) as TextureImporter;if(ti==null)continue;
                ti.textureType=path.Contains("nor_gl")||path.Contains("Normal")?TextureImporterType.NormalMap:TextureImporterType.Default;
                ti.sRGBTexture=path.Contains("_diff_")||path.Contains("BaseColor")||path.EndsWith("leaf.png")||path.EndsWith("flower.png");
                ti.maxTextureSize=path.Contains("House")||path.Contains("plaster")||path.Contains("roof")||path.Contains("mud")||path.Contains("path")||path.Contains("concrete")?1024:512;
                ti.textureCompression=TextureImporterCompression.Compressed;ti.mipmapEnabled=true;ti.anisoLevel=2;ti.alphaIsTransparency=path.EndsWith("leaf.png")||path.EndsWith("flower.png");ti.SaveAndReimport();
            }
            foreach(string path in Directory.GetFiles(Art+"Models","*.fbx"))
            {var mi=(ModelImporter)AssetImporter.GetAtPath(path);mi.importAnimation=false;mi.importCameras=false;mi.importLights=false;mi.addCollider=false;mi.preserveHierarchy=true;mi.materialImportMode=ModelImporterMaterialImportMode.None;mi.meshCompression=ModelImporterMeshCompression.Low;mi.SaveAndReimport();}
            foreach(var record in manifest.materials)
            {
                var color=string.IsNullOrEmpty(record.albedo)?new Color(record.color[0],record.color[1],record.color[2],record.color[3]):Color.white;
                var mat=Plain(record.name,color,1-record.roughness);mat.SetFloat("_Metallic",record.metallic);
                if(!string.IsNullOrEmpty(record.albedo))mat.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(record.albedo));
                if(!string.IsNullOrEmpty(record.normal)){mat.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(record.normal));mat.SetFloat("_BumpScale",.55f);mat.EnableKeyword("_NORMALMAP");}
                if(!string.IsNullOrEmpty(record.mask))
                {var mask=AssetDatabase.LoadAssetAtPath<Texture2D>(record.mask);mat.SetTexture("_MetallicGlossMap",mask);mat.SetTexture("_OcclusionMap",mask);mat.SetFloat("_Smoothness",.65f);mat.SetFloat("_OcclusionStrength",.7f);mat.EnableKeyword("_METALLICSPECGLOSSMAP");mat.EnableKeyword("_OCCLUSIONMAP");}
                if(record.alphaClip){mat.SetFloat("_AlphaClip",1);mat.SetFloat("_Cutoff",.45f);mat.SetFloat("_Cull",0);mat.EnableKeyword("_ALPHATEST_ON");mat.renderQueue=2450;}
                if(record.name.Contains("HeadlightLens")){mat.EnableKeyword("_EMISSION");mat.SetColor("_EmissionColor",new Color(.7f,.64f,.42f)*1.4f);}
                EditorUtility.SetDirty(mat);
            }
        }
        internal static Material Plain(string name,Color tint,float smooth=.18f)
        {string path=Art+"Materials/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}m.SetColor("_BaseColor",tint);m.SetFloat("_Smoothness",smooth);m.enableInstancing=true;EditorUtility.SetDirty(m);return m;}
        internal static Material Mat(string name)=>AssetDatabase.LoadAssetAtPath<Material>(Art+"Materials/"+name+".mat");
        internal static GameObject Model(string id,string name,Vector3 pos)
        {
            var go=new GameObject(name);go.transform.SetParent(environment,false);
            var instance=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Art+"Models/"+id+".fbx"));instance.transform.SetParent(go.transform,false);
            foreach(var r in go.GetComponentsInChildren<Renderer>())
            {
                string meshName=r.GetComponent<MeshFilter>()?r.GetComponent<MeshFilter>().sharedMesh.name:r.name;
                string key=r.name.Contains("__")?r.name:meshName;
                string materialName=key.Substring(key.IndexOf("__",StringComparison.Ordinal)+2);
                var material=Mat(materialName);
                if(!material)throw new Exception("Missing Atmaja material mapping: "+r.name+" mesh="+meshName+" key="+materialName);
                r.sharedMaterials=Enumerable.Repeat(material,r.sharedMaterials.Length).ToArray();if(materialName.Contains("FRANG_Leaf")||materialName.Contains("FRANG_Flower"))r.shadowCastingMode=ShadowCastingMode.Off;
            }
            go.transform.position=pos;return go;
        }
        internal static void Ground(GameObject go,float y=0)
        {go.transform.position+=Vector3.up*(y+.004f-KosVisualV2.BoundsOf(go).min.y);}
        internal static GameObject Box(string name,Vector3 pos,Vector3 size,Material material,bool solid=true)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;go.transform.SetParent(environment,false);go.transform.SetPositionAndRotation(pos,Facing(north));go.transform.localScale=size;
            go.GetComponent<Renderer>().sharedMaterial=material;if(!solid)Object.DestroyImmediate(go.GetComponent<Collider>());return go;
        }
        internal static GameObject Surface(string name,float x,float depth,float y,float w,float d,Material material,bool solid=true)
        {
            var go=Box(name,P(x,depth,y-.025f),new Vector3(w,.05f,d),material,solid);
            var local=new Material(material){name=name+"_Tiled"};local.SetTextureScale("_BaseMap",new Vector2(w*.5f,d*.5f));
            string path=Art+"Materials/"+name+"_Tiled.mat";var prior=AssetDatabase.LoadAssetAtPath<Material>(path);if(prior){EditorUtility.CopySerialized(local,prior);Object.DestroyImmediate(local);local=prior;}else AssetDatabase.CreateAsset(local,path);
            go.GetComponent<Renderer>().sharedMaterial=local;return go;
        }
        private static Transform Anchor(string name,Vector3 pos,Vector3 look)
        {var t=new GameObject(name).transform;t.SetParent(director.transform,false);var direction=look-pos;direction.y=0;t.SetPositionAndRotation(pos,Facing(direction));return t;}
        internal static AtmajaItem Item(GameObject root,AtmajaAction action,string label)
        {var item=root.AddComponent<AtmajaItem>();item.director=director;item.action=action;item.label=label;return item;}
        internal static void FitCollider(GameObject go)
        {
            // Use root-local geometry bounds. A rotated world AABB swapped chair/clock dimensions.
            Bounds bounds=default;bool first=true;
            foreach(var f in go.GetComponentsInChildren<MeshFilter>(true))
            {
                if(!f.sharedMesh)continue;var b=f.sharedMesh.bounds;
                for(int i=0;i<8;i++)
                {
                    var local=b.center+Vector3.Scale(b.extents,new Vector3((i&1)==0?-1:1,(i&2)==0?-1:1,(i&4)==0?-1:1));
                    var p=go.transform.InverseTransformPoint(f.transform.TransformPoint(local));if(first){bounds=new Bounds(p,Vector3.zero);first=false;}else bounds.Encapsulate(p);
                }
            }
            var collider=go.AddComponent<BoxCollider>();collider.center=bounds.center;collider.size=bounds.size;
        }
        private static void ShellAndGarden()
        {
            Surface("WetCourtyard",0,1,0,25,31,Mat("brown_mud_03"));
            Surface("FrontPath",0,-6,.014f,2.2f,8,Mat("mossy_cobblestone"));
            Surface("EastMuddyPath",8,5,.012f,3,17,Mat("stony_dirt_path"));
            Surface("Street",0,-13,.01f,25,4,Mat("dirty_concrete"));
            var wall=Mat("House_MAT_PlasterLower");var cap=Mat("House_MAT_Concrete");
            foreach(float x in new[]{-10.3f,10.3f}){Box("BoundaryWall",P(x,2,1.15f),new Vector3(.32f,2.3f,25),wall);Box("MossyWallCap",P(x,2,2.32f),new Vector3(.43f,.12f,25),cap);}
            Box("BackBoundary",P(0,14.4f,1.15f),new Vector3(20.8f,2.3f,.32f),wall);
            foreach(float x in new[]{-5.9f,5.9f})Box("FrontBoundary",P(x,-10,1.05f),new Vector3(8.6f,2.1f,.32f),wall);
            foreach(float x in new[]{-1.65f,1.65f}){Box("GatePillar",P(x,-10,1.65f),new Vector3(.48f,3.3f,.5f),wall);Box("GatePillarCap",P(x,-10,3.32f),new Vector3(.60f,.12f,.64f),cap);}
            var gate=Model("large_iron_gate","RustyIronGate",P(0,-10,0));KosVisualV2.SetCenter(gate,P(0,-10,1.47f));Ground(gate);FitCollider(gate);
            director.gateHinge=new GameObject("GateHinge").transform;director.gateHinge.SetParent(environment,false);director.gateHinge.position=P(-1.48f,-10,0);gate.transform.SetParent(director.gateHinge,true);
            Item(gate,AtmajaAction.Gate,"Buka gerbang besi");
            for(int i=0;i<6;i++)
            {
                var shrub=Model("shrub_03","OvergrownShrub_"+i,P(i<3?-7.8f:8.9f,-7+i%3*4,0));
                KosVisualV2.Resize(shrub,new Vector3(1.8f,.85f,1.1f));Ground(shrub);
            }
            for(int i=0;i<9;i++)
            {var grass=Model("grass_bermuda_01","GrassClump_"+i,P(i%2==0?-8:9.5f,-8+i*2.4f,0));KosVisualV2.Resize(grass,new Vector3(.7f,.24f,.7f));Ground(grass);}
            for(int i=0;i<2;i++)
            {var fern=Model("fern_02","Fern_"+i,P(i==0?5.3f:-4.7f,-5,0));KosVisualV2.Resize(fern,new Vector3(.8f,.65f,.75f));Ground(fern);}
            var tree=Model("FallenFrangipani","FallenKamboja",P(6.4f,5.5f,0));Ground(tree);
            // One capsule follows the raised horizontal limb. Root/soil are separate low hulls.
            var limb=new GameObject("KambojaCrouchLimb",typeof(CapsuleCollider));limb.transform.SetParent(environment,false);limb.transform.SetPositionAndRotation(P(8.2f,5.5f,1.41f),Facing(north));
            var c=limb.GetComponent<CapsuleCollider>();c.direction=0;c.height=4.2f;c.radius=.18f;
            Box("KambojaRootCollision",P(6.8f,5.1f,.46f),new Vector3(.6f,.9f,.9f),Mat("Tree_FRANG_Bark")).GetComponent<Renderer>().enabled=false;
            var chair=Model("WornWoodChair","AbandonedPorchChair",P(-2.5f,-1.0f,.45f));chair.transform.rotation=Facing(-north);Ground(chair,.45f);FitCollider(chair);
            // The west passage is visibly obstructed, not an invisible wrong-way trigger.
            for(int i=0;i<4;i++)
            {
                float height=1.6f+(i%2)*.45f;
                Box("WestCollapsedMasonry_"+i,P(-9.7f+i*.9f,1.0f,height*.5f),new Vector3(.96f,height,.75f),Mat("House_MAT_BrickBase"));
            }
            var brace=Box("WestFallenFenceBrace",P(-8.3f,.55f,1.1f),new Vector3(3.8f,.13f,.15f),Mat("House_MAT_WoodDoor"));brace.transform.Rotate(north,18,Space.World);
            for(int i=0;i<2;i++){var pot=Model("planter_pot_clay","PorchClayPot_"+i,P(i==0?-4.8f:4.8f,-.9f,0));Ground(pot);FitCollider(pot);}
        }
        private static void ExteriorStoryObjects()
        {
            var scooter=Model("DamarScooter","DamarParkedScooter",P(4.8f,-4.2f,0));scooter.transform.rotation=Quaternion.AngleAxis(180,Vector3.up)*Quaternion.AngleAxis(-9,north)*scooter.transform.rotation;Ground(scooter);FitCollider(scooter);
            Item(scooter,AtmajaAction.Survey,"Periksa jok motor Damar");
            var lens=scooter.GetComponentsInChildren<Renderer>().Single(r=>r.name.Contains("HeadlightLens"));
            LightAt("DimScooterHeadlight",lens.bounds.center,LightType.Spot,1.5f,7,new Color(.86f,.80f,.57f),P(6,-.2f,.6f));
            var front=Box("ChainedFrontDoor",P(0,-.10f,1.65f),new Vector3(1.55f,2.5f,.12f),Mat("House_MAT_WoodDoor"));Item(front,AtmajaAction.FrontDoor,"Periksa rantai pintu depan");
            Chain(P(0,-.23f,1.65f),1.2f,false);
            var port=Box("ScooterEmergencyUSB",scooter.GetComponent<BoxCollider>().bounds.center+east*.55f,new Vector3(.13f,.12f,.12f),Mat("Scooter_MOTO_DarkPlastic"));Item(port,AtmajaAction.Charge,"Isi daya darurat • USB motor");
            director.vaultOutside=Anchor("KitchenVaultOutside",P(7.6f,10.975f,.04f),P(4,10.975f,1.5f));
            director.vaultInside=Anchor("KitchenVaultInside",P(5.15f,10.975f,.37f),P(2.5f,10.975f,1.4f));
            // A solid context guard prevents generic jump/crouch shortcuts; only the contextual vault
            // temporarily disables the player capsule and passes through this window.
            var window=Box("KitchenVaultContext",P(6,10.975f,1.60f),new Vector3(.3f,1.45f,1.15f),Mat("House_MAT_WoodDoor"));window.GetComponent<Renderer>().enabled=false;
            Item(window,AtmajaAction.Window,"Jendela dapur • gunakan tombol Vault");
            var sign=Plaque("AtmajaNameplate","KELUARGA\nATMAJA",P(-2.5f,-10.20f,1.5f),.45f);sign.transform.rotation=Facing(north);
            director.kitchenShutter=Box("KitchenWindowSlamsShut",P(6,10.975f,1.54f),new Vector3(.12f,1.3f,1.14f),Mat("House_MAT_WoodDoor"));director.kitchenShutter.SetActive(false);
        }
        private static void Interior()
        {
            Surface("DiningDirtyTile",0,4.5f,.512f,11.6f,8.8f,Mat("concrete_floor_damaged_01"));
            Surface("KitchenDirtyTile",4,10.6f,.352f,3.7f,2.9f,Mat("concrete_floor_damaged_01"));
            var wall=Mat("House_MAT_PlasterDamaged");
            Box("DiningDividerSouth",P(1.6f,1.6f,2.2f),new Vector3(.18f,3.4f,3.2f),wall);
            Box("DiningDividerNorth",P(1.6f,6.7f,2.2f),new Vector3(.18f,3.4f,4.2f),wall);
            Box("DiningDividerLintel",P(1.6f,3.9f,3.45f),new Vector3(.18f,.9f,1.4f),wall);
            var hinge=new GameObject("KitchenDoorHinge").transform;hinge.SetParent(environment,false);hinge.position=P(3.4f,9, .35f);
            var leaf=Box("KitchenDiningDoor",P(4,9,1.50f),new Vector3(1.2f,2.3f,.10f),Mat("House_MAT_WoodDoor"));leaf.transform.SetParent(hinge,true);director.kitchenDoor=hinge;Item(leaf,AtmajaAction.KitchenDoor,"Buka pintu ruang makan");
            var well=GameObject.CreatePrimitive(PrimitiveType.Cylinder);well.name="OldChainedKitchenWell";well.transform.SetParent(environment,false);well.transform.position=P(2.85f,11.2f,.72f);well.transform.localScale=new Vector3(1.05f,.36f,1.05f);well.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_BrickBase");
            Item(well,AtmajaAction.Well,"Periksa sumur tua");
            for(int i=0;i<4;i++)Box("RottenWellBoard",P(2.85f+(i-1.5f)*.23f,11.2f,1.09f),new Vector3(.21f,.045f,1.10f),Mat("House_MAT_WoodDoor"));
            Chain(P(2.85f,11.2f,1.14f),1.1f,true);
            var bucket=Model("wooden_bucket_01","KitchenBucket",P(3.6f,11.9f,.35f));Ground(bucket,.35f);FitCollider(bucket);
            Box("KitchenCounter",P(5.55f,11.85f,.85f),new Vector3(.65f,1, .5f),Mat("House_MAT_PlasterLower"));
            var charger=Box("KitchenEmergencyCharger",P(5.76f,10.1f,1.35f),new Vector3(.15f,.20f,.09f),Mat("House_MAT_WoodFrameWhite"));Item(charger,AtmajaAction.Charge,"Isi daya darurat • dapur");
            // Banquet table / four worn chairs are kitbashed from the already licensed Golden furniture.
            var table=Model("LongDiningTable","LongDiningTable",P(3.7f,6.1f,.52f));
            var top=Box("DiningTableTopCollision",P(3.7f,6.1f,1.42f),new Vector3(1.45f,.075f,3.1f),Mat("House_MAT_WoodDoor"));top.GetComponent<Renderer>().enabled=false;
            foreach(float x in new[]{3.09f,4.31f})foreach(float z in new[]{4.73f,7.47f})Box("DiningTableLegCollision",P(x,z,.95f),new Vector3(.09f,.86f,.09f),Mat("House_MAT_WoodDoor")).GetComponent<Renderer>().enabled=false;
            foreach(float z in new[]{5.2f,6.9f})foreach(float x in new[]{2.5f,4.9f})
            {var chair=Model("plastic_monobloc_chair_01","WornDiningChair",P(x,z,.52f));Ground(chair,.52f);FitCollider(chair);}
            for(int i=0;i<5;i++)
            {var plate=GameObject.CreatePrimitive(PrimitiveType.Cylinder);plate.name="MouldyPlate";plate.transform.SetParent(environment,false);plate.transform.position=P(3.35f+(i%2)*.65f,5.0f+i*.49f,1.47f);plate.transform.localScale=new Vector3(.23f,.008f,.23f);plate.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_Concrete");Object.DestroyImmediate(plate.GetComponent<Collider>());}
            var bag=new GameObject("DamarTornBackpack");bag.transform.SetParent(environment,false);
            var bagModel=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/Art/Original/Models/KosBackpack.fbx"));bagModel.transform.SetParent(bag.transform,false);
            foreach(var renderer in bag.GetComponentsInChildren<Renderer>())renderer.sharedMaterials=Enumerable.Repeat(Plain("DamarBagCanvas",new Color(.12f,.14f,.12f)),renderer.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(bag,new Vector3(.43f,.32f,.31f));KosVisualV2.SetCenter(bag,P(3.6f,6.8f,.675f));FitCollider(bag);
            Box("DriedBloodOnStrap",P(3.62f,6.63f,.73f),new Vector3(.045f,.18f,.012f),Plain("DriedBlood",new Color(.18f,.055f,.035f)),false);
            float phoneY=KosVisualV2.SurfaceHeight(bag,P(3.61f,6.77f,0).x,P(3.61f,6.77f,0).z)+.011f;
            director.damarPhone=Box("DamarCrackedPhone",P(3.61f,6.77f,phoneY),new Vector3(.075f,.015f,.145f),Plain("CrackedPhoneGlass",new Color(.15f,.22f,.19f),.55f));Item(director.damarPhone,AtmajaAction.DamarPhone,"Ambil kartu aplikasi dari HP Damar");
            var cracks=new GameObject("DamarScreenCracks",typeof(LineRenderer));cracks.transform.SetParent(director.damarPhone.transform,true);var line=cracks.GetComponent<LineRenderer>();line.widthMultiplier=.0015f;line.positionCount=5;line.sharedMaterial=Mat("House_MAT_WoodFrameWhite");line.SetPositions(new[]{P(3.58f,6.71f,phoneY+.008f),P(3.62f,6.76f,phoneY+.008f),P(3.60f,6.78f,phoneY+.008f),P(3.63f,6.82f,phoneY+.008f),P(3.59f,6.80f,phoneY+.008f)});
            var lounge=Box("OldLoungeTable",P(-1.0f,2.6f,.93f),new Vector3(1.2f,.10f,.65f),Mat("House_MAT_WoodDoor"));
            foreach(float x in new[]{-1.5f,-.5f})foreach(float z in new[]{2.4f,2.8f})Box("LoungeTableLeg",P(x,z,.72f),new Vector3(.08f,.4f,.08f),Mat("House_MAT_WoodDoor"));
            director.clockHands=Box("BrassClockHands",P(-1,2.6f,1.0f),new Vector3(.18f,.025f,.03f),Plain("ClockBrass",new Color(.46f,.32f,.12f),.45f));Item(director.clockHands,AtmajaAction.ClockHands,"Ambil Jarum Jam Kuningan");
            var clock=Model("GrandfatherClock","GrandfatherClock",P(-4.85f,5.5f,.52f));FitCollider(clock);Item(clock,AtmajaAction.Clock,"Pasang dan atur jarum jam bandul");
            director.clockDialCentre=P(-4.85f,5.29f,2.30f);
            director.clockHour=clock.GetComponentsInChildren<Transform>().Single(t=>t.name.StartsWith("ClockHourBlade__"));
            director.clockMinute=clock.GetComponentsInChildren<Transform>().Single(t=>t.name.StartsWith("ClockMinuteBlade__"));
            director.clockDrawer=clock.GetComponentsInChildren<Transform>().Single(t=>t.name.StartsWith("ClockDrawer__"));
            var face=Plaque("ClockDial","XII\nIX          III\nVI",P(-4.85f,5.28f,2.30f),.36f);face.transform.rotation=Facing(north);
            var door=Box("LockedCentralStairDoor",P(-3.55f,7.7f,1.8f),new Vector3(1.25f,2.6f,.14f),Mat("House_MAT_WoodDoor"));Item(door,AtmajaAction.StairDoor,"Buka pintu tangga tengah");
            Box("StairHallWall",P(-3.55f,7.9f,2.2f),new Vector3(4.5f,3.4f,.16f),wall);
            director.arClue=Plaque("ARDeathTimeClue","WAKTU KEMATIAN KAMI\nADALAH KUNCI.\nIII : XLV",P(-1.6f,7.76f,2.05f),.40f);director.arClue.transform.rotation=Facing(north);Layer(director.arClue,AtmajaPhoneView.ARLayer);director.arClue.SetActive(false);
            var endChair=Model("plastic_monobloc_chair_01","EmptyARScareChair",P(3.7f,4.05f,.52f));endChair.transform.rotation=Quaternion.Euler(0,180,0);Ground(endChair,.52f);FitCollider(endChair);
            director.arFigure=Model("ARSeatedAtmaja","AR_SeatedSriAtmaja",P(3.7f,4.05f,.52f));director.arFigure.transform.rotation=Quaternion.Euler(0,180,0);Layer(director.arFigure,AtmajaPhoneView.ARLayer);director.arFigure.SetActive(false);
        }
        private static void Layer(GameObject go,int layer){foreach(var t in go.GetComponentsInChildren<Transform>(true))t.gameObject.layer=layer;}
        private static GameObject Plaque(string name,string words,Vector3 pos,float scale)
        {
            var root=new GameObject(name,typeof(TextMesh));root.transform.SetParent(environment,false);root.transform.position=pos;
            var text=root.GetComponent<TextMesh>();text.text=words;text.font=AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/UI/Fonts/Barlow-Regular.ttf");text.fontSize=48;text.characterSize=scale/10;text.anchor=TextAnchor.MiddleCenter;text.alignment=TextAlignment.Center;text.color=new Color(.72f,.68f,.52f);
            root.GetComponent<MeshRenderer>().sharedMaterial=text.font.material;return root;
        }
        internal static void Chain(Vector3 pos,float length,bool horizontal)
        {
            // Eight linked, low-poly native torus rings, one combined static mesh/material.
            var verts=new System.Collections.Generic.List<Vector3>();var tris=new System.Collections.Generic.List<int>();
            for(int ring=0;ring<9;ring++)for(int i=0;i<12;i++)for(int j=0;j<5;j++)
            {
                float a=i*Mathf.PI/6,b=j*Mathf.PI*2/5;var v=new Vector3((.065f+.012f*Mathf.Cos(b))*Mathf.Cos(a),.012f*Mathf.Sin(b),(.10f+.012f*Mathf.Cos(b))*Mathf.Sin(a));
                if(ring%2==1)v=Quaternion.Euler(0,0,70)*v;
                v+=Vector3.right*(ring-4)*length/8;
                verts.Add(v);int n=ring*60+i*5+j,ni=ring*60+((i+1)%12)*5+j,nj=ring*60+i*5+(j+1)%5,nij=ring*60+((i+1)%12)*5+(j+1)%5;
                tris.AddRange(new[]{n,ni,nj,ni,nij,nj});
            }
            var mesh=new Mesh{name="AtmajaChain"};mesh.SetVertices(verts);mesh.SetTriangles(tris,0);mesh.RecalculateNormals();
            string path=Art+"Models/"+(horizontal?"WellChain":"DoorChain")+".asset";var old=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(old){EditorUtility.CopySerialized(mesh,old);Object.DestroyImmediate(mesh);mesh=old;}else AssetDatabase.CreateAsset(mesh,path);
            var go=new GameObject(horizontal?"WellRustChain":"FrontDoorRustChain",typeof(MeshFilter),typeof(MeshRenderer));go.transform.SetParent(environment,false);go.transform.position=pos;go.transform.rotation=Facing(north)*(horizontal?Quaternion.identity:Quaternion.Euler(90,0,0));go.GetComponent<MeshFilter>().sharedMesh=mesh;go.GetComponent<MeshRenderer>().sharedMaterial=Mat("House_MAT_RustMetal");
        }
        private static Light LightAt(string name,Vector3 pos,LightType type,float power,float range,Color color,Vector3 look)
        {var go=new GameObject(name,typeof(Light));go.transform.SetParent(environment,false);go.transform.SetPositionAndRotation(pos,Facing(look-pos));var light=go.GetComponent<Light>();light.type=type;light.intensity=power;light.range=range;light.color=color;light.shadows=LightShadows.None;light.spotAngle=62;return light;}
        private static void Lighting(VolumeProfile profile)
        {
            RenderSettings.skybox=null;RenderSettings.ambientMode=AmbientMode.Flat;RenderSettings.ambientLight=new Color(.023f,.030f,.040f);RenderSettings.ambientIntensity=1;
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogColor=new Color(.035f,.052f,.073f);RenderSettings.fogDensity=.018f;
            var moon=LightAt("CloudedMoon",P(0,0,20),LightType.Directional,.15f,50,new Color(.51f,.65f,.84f),P(-5,8,0));moon.shadows=LightShadows.Soft;moon.shadowStrength=.85f;
            var fixture=Model("security_light","PorchSecurityFixture",P(.7f,-.2f,3.2f));
            var material=new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));material.SetColor("_BaseColor",new Color(.55f,.65f,.7f,.25f));material.SetFloat("_Surface",1);material.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);material.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);material.SetFloat("_ZWrite",0);material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");material.renderQueue=3000;
            string path=Art+"Materials/Rain.mat";var old=AssetDatabase.LoadAssetAtPath<Material>(path);if(old){EditorUtility.CopySerialized(material,old);Object.DestroyImmediate(material);material=old;}else AssetDatabase.CreateAsset(material,path);
            // Outdoor-only emitter footprints: no rain through the house roof and no particle collisions.
            Rain("FrontCourtyardRain",P(0,-6,13),new Vector3(20,8,.1f),300,material);
            Rain("EastPassageRain",P(8.2f,6,13),new Vector3(2.8f,16,.1f),120,material);
        }
        private static void Rain(string name,Vector3 position,Vector3 footprint,float rate,Material material)
        {
            var rain=new GameObject(name,typeof(ParticleSystem));rain.transform.SetParent(environment,false);rain.transform.position=position;rain.transform.rotation=Quaternion.Euler(90,0,0);
            var ps=rain.GetComponent<ParticleSystem>();var main=ps.main;main.startLifetime=.9f;main.startSpeed=15;main.startSize=.025f;main.maxParticles=(int)rate+20;main.startColor=new Color(.55f,.65f,.72f,.25f);main.simulationSpace=ParticleSystemSimulationSpace.World;
            var emission=ps.emission;emission.rateOverTime=rate;var shape=ps.shape;shape.shapeType=ParticleSystemShapeType.Box;shape.scale=footprint;
            var renderer=rain.GetComponent<ParticleSystemRenderer>();renderer.renderMode=ParticleSystemRenderMode.Stretch;renderer.velocityScale=.02f;renderer.lengthScale=7;renderer.sharedMaterial=material;
        }
        private static void Audio()
        {
            AudioSource Source(string name,UnityEngine.Audio.AudioMixerGroup bus,float spatial)
            {var go=new GameObject(name,typeof(AudioSource));go.transform.SetParent(director.transform,false);var s=go.GetComponent<AudioSource>();s.outputAudioMixerGroup=bus;s.playOnAwake=false;s.dopplerLevel=0;s.spatialBlend=spatial;s.minDistance=1;s.maxDistance=16;s.rolloffMode=AudioRolloffMode.Linear;return s;}
            var profile=director.audioProfile;
            director.rainSource=Source("CourtyardRainAmbience",profile.ambienceBus,0);director.rainSource.clip=profile.rain;director.rainSource.loop=true;director.rainSource.volume=.3f;
            var loop=director.rainSource.gameObject.AddComponent<AmbientAudioLoop>();loop.gain=.3f;loop.fadeSeconds=1.5f;
            director.foleySource=Source("AtmajaPositionalFoley",profile.effectsBus,1);
            director.notificationSource=Source("AtmajaPhoneNotifications",profile.uiBus,0);
            director.stepsSource=Source("AtmajaFootsteps",profile.effectsBus,.65f);
        }
    }
}
#endif
