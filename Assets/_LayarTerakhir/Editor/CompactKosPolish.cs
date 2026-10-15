#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class CompactKosPolish
    {
        public const float Width=5f,Depth=3.8f;
        private const string Art="Assets/_LayarTerakhir/Art/";
        private static Transform art,details;
        private static GameObject Find(string name)
        {
            var t=art.Find(name)??art.Find("Kos_V2_Details/"+name)??art.Find("Golden_Dressing/"+name)??art.Find("Golden_Workstation/"+name);
            return t?t.gameObject:GameObject.Find(name)??throw new Exception("Missing Kos object: "+name);
        }
        private static GameObject[] Named(params string[] names)=>names.Select(Find).ToArray();
        private static GameObject[] Matching(Func<Transform,bool> predicate)=>art.GetComponentsInChildren<Transform>().Where(predicate).Select(t=>t.gameObject).ToArray();
        [MenuItem("LayarTerakhir/Environment/Polish compact city-streamer Kos")]
        public static void Apply()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            ApplyToOpenScene();
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();AssetDatabase.SaveAssets();
            GoldenSceneValidation.Validate();
            Debug.Log("COMPACT_KOS Applied 5.0 x 3.8m lived-in sleep/work/stream layout; gameplay IDs unchanged.");
        }
        public static void ApplyToOpenScene()
        {
            art=GameObject.Find("Kos_Environment_Art").transform;
            var previous=GameObject.Find("Compact_Kos_Details");if(previous)Object.DestroyImmediate(previous);
            details=new GameObject("Compact_Kos_Details").transform;details.SetParent(art,false);
            Shell();Layout();Dress();Wear();
            var player=Object.FindFirstObjectByType<FirstPersonController>();
            player.transform.SetPositionAndRotation(new Vector3(.87f,.05f,-.82f),Quaternion.Euler(0,-38,0));
            player.cameraPivot.localRotation=Quaternion.identity;
            var camera=player.GetComponent<InteractionInteractor>().playerCamera;
            camera.transform.localRotation=Quaternion.Euler(9,0,0);
            // Position existing emitters with their objects; do not change audio or story logic.
            var d=Object.FindFirstObjectByType<KamarKosPrologDirector>();var audio=d.environmentAudio;
            audio.computer.transform.position=KosVisualV2.BoundsOf(Find("OldPCTower")).center;
            audio.doorSource.transform.position=d.kosDoorItem.transform.position;
            audio.phoneSource.transform.position=d.smartphoneBedItem.transform.position;
            audio.departureSource.transform.position=new Vector3(1.25f,.6f,-2.2f);
            Physics.SyncTransforms();
        }
        private static void Transform(string name,Vector3 pos,Vector3 scale)
        {var t=Find(name).transform;t.position=pos;t.localScale=scale;}
        private static void Shell()
        {
            Transform("Floor_4x3m",new Vector3(0,-.05f,0),new Vector3(Width,.1f,Depth));
            Transform("Ceiling_4x3m",new Vector3(0,2.85f,0),new Vector3(Width,.1f,Depth));
            Transform("Wall_North",new Vector3(0,1.4f,1.95f),new Vector3(Width,2.8f,.1f));
            Transform("Wall_South",new Vector3(0,1.4f,-1.95f),new Vector3(Width,2.8f,.1f));
            Transform("Wall_West",new Vector3(-2.55f,1.4f,0),new Vector3(.1f,2.8f,Depth));
            Transform("Wall_East",new Vector3(2.55f,1.4f,0),new Vector3(.1f,2.8f,Depth));
            Transform("CeramicTileFloor",new Vector3(0,.006f,0),new Vector3(Width,Depth,1));
            Transform("LimewashedCeiling",new Vector3(0,2.798f,0),new Vector3(Width,Depth,1));
            foreach(string face in new[]{"North","West"})
            {
                bool north=face=="North";float span=north?Width:Depth;
                Transform(face+"Dado",new Vector3(north?0:-2.498f,.52f,north?1.898f:0),new Vector3(span,1.04f,1));
                Transform(face+"Limewash",new Vector3(north?0:-2.498f,1.92f,north?1.898f:0),new Vector3(span,1.76f,1));
            }
            Transform("EastDado",new Vector3(2.498f,.55f,0),new Vector3(Depth,1.1f,1));
            Transform("EastAboveWindow",new Vector3(2.498f,2.5f,0),new Vector3(Depth,.6f,1));
            Transform("EastNorthOfWindow",new Vector3(2.498f,1.65f,.95f),new Vector3(1.9f,1.1f,1));
            Transform("EastSouthOfWindow",new Vector3(2.498f,1.65f,-1.5f),new Vector3(.8f,1.1f,1));
            foreach(var side in new[]{("Left",-.87f,3.26f),("Right",2.12f,.76f)})
            {
                Transform("South"+side.Item1+"Dado",new Vector3(side.Item2,.52f,-1.898f),new Vector3(side.Item3,1.04f,1));
                Transform("South"+side.Item1+"Limewash",new Vector3(side.Item2,1.92f,-1.898f),new Vector3(side.Item3,1.76f,1));
                Transform("SouthSkirting"+side.Item1,new Vector3(side.Item2,.055f,-1.87f),new Vector3(side.Item3,.11f,.04f));
            }
            Transform("DoorLintelPlaster",new Vector3(1.25f,2.46f,-1.898f),new Vector3(.98f,.68f,1));
            Transform("NorthSkirting",new Vector3(0,.055f,1.87f),new Vector3(Width,.11f,.04f));
            Transform("WestSkirting",new Vector3(-2.47f,.055f,0),new Vector3(.04f,.11f,Depth));
            Transform("EastSkirting",new Vector3(2.47f,.055f,0),new Vector3(.04f,.11f,Depth));
            for(int i=0;i<6;i++)Transform("CeilingTimber_"+i,new Vector3(-2.35f+i*.94f,2.765f,0),new Vector3(.06f,.07f,Depth));
            foreach(string side in new[]{"North","South"})Transform("CeilingCrossBeam_"+side,new Vector3(0,2.765f,side=="North" ? .95f : -.95f),new Vector3(Width,.06f,.06f));
            Transform("NorthDampPatch",new Vector3(-1,1.5f,1.887f),new Vector3(2.7f,2.45f,1));
            Transform("NorthEastDampPatch",new Vector3(1.45f,1.95f,1.886f),new Vector3(1.6f,1.5f,1));
            Transform("WestDampPatch",new Vector3(-2.485f,1.4f,-.35f),new Vector3(2.7f,2.3f,1));
            Transform("SouthDampPatch",new Vector3(-1.15f,1.3f,-1.887f),new Vector3(2.1f,2.1f,1));
            Transform("ChannelPoster",new Vector3(-2.485f,1.86f,-1.05f),new Vector3(.48f,.66f,1));
            Transform("BoardingHouseRules",new Vector3(.38f,1.6f,-1.887f),new Vector3(.3f,.38f,1));
            Find("BareWarmBulb").transform.position=new Vector3(-.15f,2.49f,-.05f);
            Find("WeakTungstenCeiling").transform.position=new Vector3(-.15f,2.48f,-.05f);
            Find("WeakTungstenCeiling").GetComponent<Light>().intensity=11.5f;
            // Indirect warm bounce from the entry/bed side also lights the back of the portable cloth.
            Find("WarmWallBounce").transform.position=new Vector3(-.60f,1.55f,-1.20f);
            Find("WarmWallBounce").GetComponent<Light>().intensity=1.6f;
            Points("CeilingFlex",new Vector3(-.15f,2.8f,-.05f),new Vector3(-.15f,2.52f,-.05f));
            var floorMat=Find("CeramicTileFloor").GetComponent<Renderer>().sharedMaterial;
            floorMat.SetTextureScale("_BaseMap",new Vector2(Width*.5f,Depth*.5f));
            floorMat.SetTextureScale("_BumpMap",new Vector2(Width*.5f,Depth*.5f));
            floorMat.SetTextureScale("_MetallicGlossMap",new Vector2(Width*.5f,Depth*.5f));
            floorMat.SetTextureScale("_OcclusionMap",new Vector2(Width*.5f,Depth*.5f));
            floorMat.SetFloat("_Smoothness",.22f);floorMat.SetColor("_BaseColor",new Color(.80f,.78f,.69f));EditorUtility.SetDirty(floorMat);
        }
        private static void MoveSet(string anchor,Vector2 centre,params GameObject[] roots)
        {
            var b=KosVisualV2.BoundsOf(Find(anchor));var delta=new Vector3(centre.x-b.center.x,0,centre.y-b.center.z);
            var unique=roots.Distinct().ToArray();
            foreach(var go in unique.Where(g=>!unique.Any(other=>other!=g&&g.transform.IsChildOf(other.transform))))
            {
                go.transform.position+=delta;
                foreach(var line in go.GetComponentsInChildren<LineRenderer>())if(line.useWorldSpace)
                    for(int i=0;i<line.positionCount;i++)line.SetPosition(i,line.GetPosition(i)+delta);
            }
        }
        private static void Place(string name,float x,float z)
        {MoveSet(name,new Vector2(x,z),Find(name));}
        private static void Layout()
        {
            var d=Object.FindFirstObjectByType<KamarKosPrologDirector>();
            // The existing workstation is moved as an assembly, not squashed with the room shell.
            foreach(string name in new[]{"BudgetStreamingMicrophone","KeyboardKeycaps"})Find(name).transform.SetParent(Find("Golden_Workstation").transform,true);
            MoveSet("wooden_table_02",new Vector2(-.45f,1.30f),Find("Golden_Workstation"));
            MoveSet("vintage_day_bed",new Vector2(-1.89f,-.65f),new[]{Find("vintage_day_bed"),Find("KasurKos"),Find("BedContactShade"),d.smartphoneBedItem.gameObject,d.phoneBedRedLed.gameObject});
            MoveSet("bookcaseClosedDoors",new Vector2(2.12f,1.25f),Named("bookcaseClosedDoors","LemariPakaian","CupboardContactShade"));
            var books=art.Cast<Transform>().Where(t=>t.name=="books").Select(t=>t.gameObject).ToArray();
            MoveSet("painted_wooden_cabinet_02",new Vector2(1.40f,1.72f),Named("painted_wooden_cabinet_02","CupboardCollision","KosRouter","RouterPowerCable","RouterWallSocket","RouterAdapter")
                .Concat(books.Where(g=>KosVisualV2.BoundsOf(g).center.x>0)).ToArray());
            MoveSet("wooden_bookshelf_worn",new Vector2(-2.34f,.55f),new[]{Find("wooden_bookshelf_worn")}.Concat(books.Where(g=>KosVisualV2.BoundsOf(g).center.x<0)).ToArray());
            MoveSet("FanWallShelf",new Vector2(.30f,1.67f),Named("FanWallShelf","KosDeskFan","FanShelfLeftBracket","FanShelfRightBracket"));
            MoveSet("wallWindow",new Vector2(2.465f,-.55f),Named("wallWindow","DarkNightBeyondWindow","WindowSill","KosCurtains","CurtainRod")
                .Concat(Matching(t=>t.name=="WindowSecurityBar"||t.name=="CurtainRodBracket")).ToArray());
            var window=Find("ColdWindowSpill").transform;window.position=new Vector3(2.37f,1.95f,-.55f);window.LookAt(new Vector3(-1.4f,.25f,-.25f));
            MoveSet("OldWallSocket",new Vector2(-1.18f,1.87f),new[]{Find("OldWallSocket")}.Concat(Matching(t=>t.name=="SocketPinHole")).ToArray());
            MoveSet("PowerStrip",new Vector2(-1.15f,1.69f),new[]{Find("PowerStrip")}.Concat(Matching(t=>t.name=="PowerPlug"||t.name=="PlugCable")).ToArray());
            MoveSet("BedsideSocket",new Vector2(-2.475f,-.05f),Named("BedsideSocket","PhoneCharger","LoosePhoneChargeLead"));
            MoveSet(d.kosDoorItem.name,new Vector2(1.25f,-1.88f),new[]{d.kosDoorItem.gameObject}.Concat(Matching(t=>t.name.StartsWith("DoorHinge")||t.name.StartsWith("DoorHandle"))).ToArray());
            MoveSet(d.suratKosItem.name,new Vector2(1.25f,-1.72f),d.suratKosItem.gameObject);
            MoveSet(d.jacketItem.name,new Vector2(2.14f,-1.78f),d.jacketItem.gameObject,Find("JacketHook"));
            MoveSet(d.wallCalendarItem.name,new Vector2(.77f,1.88f),d.wallCalendarItem.gameObject);
            d.wallCalendarItem.transform.position+=Vector3.up*(1.79f-d.wallCalendarItem.GetComponent<Collider>().bounds.center.y);
            Place("WovenDoorMat",1.25f,-1.42f);
            MoveSet("RefillWaterGallon",new Vector2(2.10f,-1.13f),Named("RefillWaterGallon","RefillWaterLabel","WaterGallonCollision"));
            Place("plastic_bottle_gallon",2.19f,-1.62f);
            Place("trashcan",.49f,1.67f);
            MoveSet("cardboardBoxOpen",new Vector2(2.12f,-.48f),Named("cardboardBoxOpen","CardboardBoxCollision"));
            Place("wicker_basket_02",2.12f,.13f);Place("plastic_container",-1.94f,-1.69f);Place("KosBackpack",-2.18f,.50f);
            Place("KosSandals",.60f,-1.51f);Place("BedsideWornStool",-1.60f,.49f);
            KosVisualV2.Resize(Find("BedsideMat"),new Vector3(1.05f,.006f,.70f));Place("BedsideMat",-.72f,-.78f);
            Find("DiscardedPaper0").transform.position=new Vector3(.19f,.009f,1.24f);
            Find("DiscardedPaper1").transform.position=new Vector3(.36f,.010f,1.48f);
            Find("DiscardedPaper2").transform.position=new Vector3(-.88f,.010f,1.73f);
            foreach(string name in new[]{"BedsideWornStool","wicker_basket_02","KosBackpack","plastic_container"})KosVisualV2.FitBox(Find(name).GetComponent<BoxCollider>(),KosVisualV2.BoundsOf(Find(name)));
            var pc=KosVisualV2.BoundsOf(Find("OldPCTower"));
            Points("ComputerPowerCable",new Vector3(pc.center.x,.12f,pc.max.z),new Vector3(pc.center.x,.018f,1.78f),new Vector3(-1.14f,.018f,1.78f),new Vector3(-1.18f,.12f,1.87f),new Vector3(-1.18f,.42f,1.86f));
            Points("FanLead",new Vector3(.3f,1.34f,1.75f),new Vector3(.3f,.28f,1.88f),new Vector3(-1.18f,.28f,1.88f),new Vector3(-1.18f,.42f,1.87f));
        }
        private static void Points(string name,params Vector3[] positions)
        {var line=Find(name).GetComponent<LineRenderer>();line.positionCount=positions.Length;line.SetPositions(positions);}
        private static Material Plain(string name,Color tint,float smooth=.18f)
        {
            string path=Art+"Generated/Materials/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}
            m.SetColor("_BaseColor",tint);m.SetFloat("_Smoothness",smooth);m.enableInstancing=true;EditorUtility.SetDirty(m);return m;
        }
        private static Material Existing(string name)=>AssetDatabase.LoadAssetAtPath<Material>(Art+"Generated/Materials/"+name+".mat");
        private static GameObject Spawn(string model,string name,Vector3 size,Vector3 centre,float yaw,Func<string,Material> slots)
        {
            string path=Art+"Original/Models/"+model+".fbx";
            var importer=(ModelImporter)AssetImporter.GetAtPath(path);importer.importAnimation=false;importer.importCameras=false;importer.importLights=false;
            importer.materialImportMode=ModelImporterMaterialImportMode.None;importer.meshCompression=ModelImporterMeshCompression.Low;importer.SaveAndReimport();
            var go=new GameObject(name);go.transform.SetParent(details,false);
            var instance=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));instance.transform.SetParent(go.transform,false);
            go.transform.rotation=Quaternion.Euler(0,yaw,0);
            foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(slots(r.name),r.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(go,size);KosVisualV2.SetCenter(go,centre);return go;
        }
        private static void Rest(GameObject prop,GameObject support,float x,float z)
        {var b=KosVisualV2.BoundsOf(prop);KosVisualV2.SetCenter(prop,new Vector3(x,KosVisualV2.SurfaceHeight(support,x,z)+b.size.y*.5f+.001f,z));}
        private static void Ground(GameObject prop)
        {prop.transform.position+=Vector3.up*(KosVisualV2.FloorY+.001f-KosVisualV2.BoundsOf(prop).min.y);}
        private static void Solid(GameObject parent,string name,Vector3 centre,Vector3 size)
        {var go=new GameObject(name,typeof(BoxCollider));go.transform.SetParent(parent.transform,false);go.transform.position=centre;KosVisualV2.FitBox(go.GetComponent<BoxCollider>(),new Bounds(centre,size));}
        private static void Dress()
        {
            var green=Plain("CompactGreenCloth",new Color(.025f,.48f,.065f),.04f);
            green.SetTexture("_BumpMap",Existing("CurtainCottonV2").GetTexture("_BumpMap"));green.SetFloat("_BumpScale",.12f);green.EnableKeyword("_NORMALMAP");
            var screen=Spawn("KosStreamerGreenScreen","StreamerGreenScreen",new Vector3(1.70f,2.04f,.42f),new Vector3(-.4f,1.02f,-.35f),0,n=>n.Contains("GreenFabric")?green:Existing("DarkOxidisedMetal"));Ground(screen);
            Solid(screen,"GreenClothCollision",new Vector3(-.4f,1.10f,-.35f),new Vector3(1.42f,1.80f,.035f));
            foreach(float x in new[]{-1.17f,.37f})
            {Solid(screen,"BackdropPoleCollision",new Vector3(x,1.02f,-.35f),new Vector3(.045f,2.04f,.045f));Solid(screen,"BackdropFootCollision",new Vector3(x,.023f,-.35f),new Vector3(.12f,.032f,.42f));}
            PrefabUtility.SaveAsPrefabAsset(screen,"Assets/_LayarTerakhir/Production/Prefabs/StreamerGreenScreen.prefab");
            var monitor=KosVisualV2.BoundsOf(Object.FindFirstObjectByType<KamarKosPrologDirector>().pcMonitorItem.gameObject);
            var webcam=Spawn("KosBudgetWebcam","BudgetStreamingWebcam",new Vector3(.085f,.055f,.046f),new Vector3(monitor.center.x,monitor.max.y+.020f,monitor.min.z+.015f),180,n=>n.Contains("Lens")?Plain("CompactWebcamLens",new Color(.018f,.023f,.025f),.7f):Existing("OldComputerPlastic"));
            var lens=webcam.GetComponentsInChildren<Renderer>().Single(r=>r.name.Contains("Lens"));
            if(Vector3.Dot(lens.bounds.center-KosVisualV2.BoundsOf(webcam).center,new Vector3(0,0,-1))<0)webcam.transform.RotateAround(KosVisualV2.BoundsOf(webcam).center,Vector3.up,180);
            var socks=Spawn("KosDroppedSocks","DroppedSocksByBed",new Vector3(.18f,.065f,.23f),new Vector3(-1.17f,.04f,-1.03f),-17,n=>Plain("Compact"+n,n.Contains("Rib")?new Color(.27f,.29f,.26f):new Color(.47f,.46f,.39f)));Ground(socks);
            var news=Spawn("KosOldNewspaper","OldNewspaperByDesk",new Vector3(.38f,.012f,.29f),new Vector3(-1.10f,.018f,.84f),-13,n=>Plain("Compact"+n,n.Contains("Ink")?new Color(.19f,.18f,.15f):new Color(.67f,.63f,.51f),.06f));Ground(news);
            var shirt=Spawn("KosFoldedTshirt","FoldedTshirtOnBed",new Vector3(.30f,.038f,.24f),new Vector3(-2.02f,.5f,-1.13f),12,n=>Plain("Compact"+n,n.Contains("Rib")?new Color(.40f,.42f,.40f):new Color(.30f,.35f,.34f),.08f));Rest(shirt,Find("vintage_day_bed"),-2.02f,-1.13f);
            // A spare page is used on the bedside stool, connecting sleep and work routines.
            var bedsidePaper=Object.Instantiate(news,details,true);bedsidePaper.name="ReadingPaperOnStool";
            KosVisualV2.Resize(bedsidePaper,new Vector3(.23f,.007f,.18f));Rest(bedsidePaper,Find("BedsideWornStool"),-1.60f,.49f);
            var lead=new GameObject("WebcamUSBLead",typeof(LineRenderer));lead.transform.SetParent(details,false);
            var line=lead.GetComponent<LineRenderer>();line.sharedMaterial=Existing("OldComputerPlastic");line.widthMultiplier=.002f;line.positionCount=4;line.generateLightingData=true;
            line.SetPositions(new[]{webcam.transform.position,new Vector3(monitor.center.x+.10f,monitor.max.y,monitor.max.z),new Vector3(monitor.center.x+.14f,.78f,monitor.max.z),new Vector3(monitor.center.x+.10f,.08f,1.65f)});
        }
        private static void Wear()
        {
            var stain=Plain("CompactFloorTrackedDust",new Color(.20f,.16f,.10f,.36f),.02f);
            stain.SetTexture("_BaseMap",Existing("DampStain").GetTexture("_BaseMap"));
            stain.SetFloat("_Surface",1);stain.SetFloat("_SrcBlend",(float)BlendMode.SrcAlpha);stain.SetFloat("_DstBlend",(float)BlendMode.OneMinusSrcAlpha);
            stain.SetFloat("_ZWrite",0);stain.SetFloat("_SpecularHighlights",0);stain.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");stain.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
            stain.renderQueue=3000;stain.SetShaderPassEnabled("ShadowCaster",false);
            var patches=new[]{new Vector4(.83f,-1.31f,.80f,.55f),new Vector4(.8f,-.45f,.48f,.83f),new Vector4(.50f,.37f,.65f,.65f),new Vector4(-.54f,.50f,.85f,.7f),new Vector4(-.87f,-.99f,.6f,.4f),new Vector4(1.93f,-.85f,.5f,.7f)};
            var temporary=GameObject.CreatePrimitive(PrimitiveType.Quad);var quad=temporary.GetComponent<MeshFilter>().sharedMesh;Object.DestroyImmediate(temporary);
            var combine=new CombineInstance[patches.Length];
            for(int i=0;i<patches.Length;i++)
            {
                var p=patches[i];combine[i]=new CombineInstance{mesh=quad,transform=Matrix4x4.TRS(new Vector3(p.x,KosVisualV2.FloorY+.0015f,p.y),Quaternion.Euler(90,i*37,0),new Vector3(p.z,p.w,1))};
            }
            string path=Art+"Generated/CompactFloorWear.asset";var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if(!mesh){mesh=new Mesh();AssetDatabase.CreateAsset(mesh,path);}else mesh.Clear();mesh.CombineMeshes(combine);EditorUtility.SetDirty(mesh);
            var wear=new GameObject("TrackedFloorWear",typeof(MeshFilter),typeof(MeshRenderer));wear.transform.SetParent(details,false);wear.GetComponent<MeshFilter>().sharedMesh=mesh;
            var r=wear.GetComponent<Renderer>();r.sharedMaterial=stain;r.shadowCastingMode=ShadowCastingMode.Off;r.receiveShadows=false;
        }
        public static void InspectLayout()
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var art=GameObject.Find("Kos_Environment_Art").transform;
            var objects=art.Cast<Transform>().SelectMany(t=>t.name=="Kos_V2_Details"||t.name=="Golden_Dressing"||t.name=="Golden_Workstation"?t.Cast<Transform>():new[]{t});
            foreach(var t in objects)
            {
                var r=t.GetComponentsInChildren<Renderer>().Where(x=>x.enabled&&!(x is LineRenderer)).ToArray();
                if(r.Length==0)continue;var b=r[0].bounds;foreach(var rr in r.Skip(1))b.Encapsulate(rr.bounds);
                Debug.Log("COMPACT_LAYOUT "+t.name+" parent="+t.parent.name+" pos="+t.position.ToString("F3")+" size="+t.localScale.ToString("F3")+" bounds="+b.ToString("F3"));
            }
            foreach(var c in Object.FindObjectsByType<Collider>(FindObjectsSortMode.None))
                Debug.Log("COMPACT_COLLIDER "+c.name+" parent="+c.transform.parent?.name+" bounds="+c.bounds.ToString("F3"));
        }
    }
}
#endif
