#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase5;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using Unity.AI.Navigation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static partial class Phase5SceneBuilder
    {
        public const string ScenePath = "Assets/_LayarTerakhir/assets/Scenes/Babak5_PasarSetan.unity";
        private const string Art = "Assets/_LayarTerakhir/assets/Art/Phase5/", Imported = Art + "Imported/";
        private static Phase5Director d;
        private static Transform env;
        private static readonly List<Phase5Anchor> anchors = new List<Phase5Anchor>();
        private static readonly List<Phase5Zone> zones = new List<Phase5Zone>();
        private static readonly List<Phase3EvpSource> sources = new List<Phase3EvpSource>();
        private static readonly List<GameObject> uvMarks = new List<GameObject>(), arMarks = new List<GameObject>(), faces = new List<GameObject>();
        private static readonly List<Usage> usage = new List<Usage>();
        private static readonly Dictionary<string,string> modelPaths = new Dictionary<string,string>();
        [Serializable] private sealed class Usage { public string sourceId, purpose, model; public Vector3 position; }
        [Serializable] private sealed class Report { public string scene; public int distinctNewModels; public Usage[] placements; public string[] anchors; }
        [MenuItem("LayarTerakhir/Phase 5/Build Pasar Setan")]
        public static void Build()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport); PrepareMaterials(); PrepareHumans();
            EditorUtility.UnloadUnusedAssetsImmediate(); GC.Collect();
            var kos = EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var original = Object.FindFirstObjectByType<FirstPersonController>(); var systems = GameObject.Find("GameSystems"); var events = Object.FindFirstObjectByType<EventSystem>();
            if (!original || !systems) throw new InvalidOperationException("Approved shared player systems missing");
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive); SceneManager.SetActiveScene(scene);
            var core = Object.Instantiate(systems); core.name = "PasarSetan_GameSystems"; Object.DestroyImmediate(core.GetComponent<KamarKosPrologDirector>());
            foreach (var audio in core.GetComponentsInChildren<KosSceneAudio>(true)) Object.DestroyImmediate(audio.gameObject);
            var player = Object.Instantiate(original.gameObject); player.name = "Player_RakaWijaya";
            if (events && !events.transform.IsChildOf(systems.transform)) Object.Instantiate(events.gameObject).name = "EventSystem";
            EditorSceneManager.CloseScene(kos, true); EditorUtility.UnloadUnusedAssetsImmediate(); GC.Collect();
            d = core.AddComponent<Phase5Director>(); d.player = player.GetComponent<FirstPersonController>(); d.world = core.AddComponent<Phase5World>();
            d.phoneView = core.GetComponent<AtmajaPhoneView>() ?? core.AddComponent<AtmajaPhoneView>(); d.phoneView.playerCamera = player.GetComponent<InteractionInteractor>().playerCamera;
            d.phoneView.feedMaterial = PresentationProfile.Load().phoneFeedMaterial; d.phoneView.playerCamera.farClipPlane = 110;
            d.phoneView.playerCamera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            d.balance = AssetDatabase.LoadAssetAtPath<Phase5Balance>(Art + "Phase5Balance.asset");
            if (!d.balance) { d.balance = ScriptableObject.CreateInstance<Phase5Balance>(); AssetDatabase.CreateAsset(d.balance, Art + "Phase5Balance.asset"); }
            d.boss = core.AddComponent<Phase5Boss>(); d.boss.director = d; d.replicas = core.AddComponent<Phase5Replicas>(); d.replicas.director = d;
            d.soundscape = core.AddComponent<Phase5Soundscape>(); d.soundscape.director = d;
            d.soundscape.routing = AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/assets/Audio/KosAudioProfile.asset");
            d.soundscape.sounds = Sounds();
            env = new GameObject("Pasar_Setan_New_Licensed_Environment").transform;
            anchors.Clear(); zones.Clear(); sources.Clear(); uvMarks.Clear(); arMarks.Clear(); faces.Clear(); usage.Clear(); modelPaths.Clear();
            foreach (string path in Directory.GetFiles(Imported + "Models", "*.fbx")) modelPaths[Path.GetFileNameWithoutExtension(path)] = path;
            Architecture(); Debug.Log("PHASE5_STAGE architecture");
            Investigation(); Debug.Log("PHASE5_STAGE investigation");
            Actors(); Debug.Log("PHASE5_STAGE actors");
            Dressing(); Debug.Log("PHASE5_STAGE dressing");
            Epilogues(); Lighting(); Debug.Log("PHASE5_STAGE epilogues-lighting");
            ValidateDamarIdlePose();
            d.world.anchors = anchors.ToArray(); d.world.zones = zones.ToArray(); d.world.sources = sources.ToArray();
            d.world.uvMarks = uvMarks.ToArray(); d.world.arMarks = arMarks.ToArray(); d.world.faceMirrors = faces.ToArray();
            core.AddComponent<Phase5SpectralPresentation>().director = d;
            var text = core.AddComponent<AtmajaWorldText>(); text.includeARText = true; text.materialTemplate = NewMaterial("P5_WorldText", "LayarTerakhir/WorldText");
            // Bake with all authored doors open. Runtime blockers control access;
            // their colliders are not mistaken for permanently unwalkable routes.
            var blockers = d.world.sectorBlockers.Concat(d.world.doorLeaves).Concat(new[]{d.world.bridgeBlocker,d.world.arenaBlocker}).ToArray();
            foreach (var b in blockers) b.SetActive(false);
            var nav = env.gameObject.AddComponent<NavMeshSurface>(); nav.collectObjects = CollectObjects.Children; nav.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            nav.layerMask = ~((1 << 24) | (1 << 26) | (1 << 29) | (1 << 31) | (1 << 25) | (1 << 2)); nav.overrideVoxelSize = true; nav.voxelSize = .14f;
            nav.BuildNavMesh(); if (!nav.navMeshData) throw new InvalidOperationException("Pasar Setan navigation missing");
            string navPath = Art + "PasarSetanNavMesh.asset"; if (AssetDatabase.LoadAssetAtPath<NavMeshData>(navPath)) AssetDatabase.DeleteAsset(navPath); AssetDatabase.CreateAsset(nav.navMeshData, navPath);
            foreach (var b in blockers) b.SetActive(true); d.world.arenaBlocker.SetActive(false);
            foreach (var actor in d.replicas.actors) actor.SetActive(false); d.world.bossRoot.SetActive(false); foreach (var clone in d.world.clones) clone.SetActive(false);
            player.transform.SetPositionAndRotation(d.world.Anchor("GATE").position, d.world.Anchor("GATE").rotation);
            int count = usage.Select(u => u.sourceId).Distinct().Count(); if (count < 100) throw new InvalidOperationException("At least100 newly licensed placed model IDs required; got " + count);
            EditorSceneManager.MarkSceneDirty(scene); EditorSceneManager.SaveScene(scene, ScenePath);
            EditorBuildSettings.scenes = EditorBuildSettings.scenes.Where(s => s.path != ScenePath).Concat(new[]{new EditorBuildSettingsScene(ScenePath,true)}).ToArray(); AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/phase5-scene-content.json", JsonUtility.ToJson(new Report { scene=ScenePath, distinctNewModels=count, placements=usage.ToArray(), anchors=anchors.Select(a=>a.id).ToArray() },true)+"\n");
            Debug.Log("PHASE5_BUILT distinctModels="+count+" placements="+usage.Count);
        }
        private static Transform Point(string name,Vector3 at,Transform parent=null,float yaw=0)
        {var t=new GameObject(name).transform;t.SetParent(parent?parent:env,false);t.SetPositionAndRotation(at,Quaternion.Euler(0,yaw,0));return t;}
        private static void Anchor(string id,Vector3 at,float yaw=0)=>anchors.Add(new Phase5Anchor{id=id,point=Point("Anchor_"+id,at,null,yaw)});
        private static void Zone(string id,Vector3 center,Vector3 size)=>zones.Add(new Phase5Zone{id=id,bounds=new Bounds(center,size)});
        private static GameObject Model(string id,Vector3 at,float height,float yaw,string purpose,bool collision=false)
            =>Place(id,modelPaths[id],at,Vector3.one*height,yaw,purpose,collision,true);
        private static GameObject Module(string name,Vector3 at,Vector3 size,string purpose,float yaw=0,bool collision=true)
        {
            var parts=new Dictionary<string,string>{{"ChainlinkPanel","modular_chainlink_fence"},{"AirductCircular","modular_airduct_circular_01"},{"AirductRectangular","modular_airduct_rectangular_01"},{"MetalGutter","modular_metal_gutter"},{"PipeSteel","modular_pipes"},{"PipePlastic","modular_pipes_plastic_01"}};
            string source=parts.TryGetValue(name,out string owner)?owner:name.StartsWith("Facade")?"modular_urban_apartments_facade":name=="FortGate"?"modular_fort_01":name=="WitnessHand"?"WitnessHands":name=="StraightCable"?"modular_electric_cables":"projector_screen";
            return Place(source,Imported+"Modules/"+name+".fbx",at,size,yaw,purpose,collision,false);
        }
        private static GameObject Place(string id,string path,Vector3 at,Vector3 size,float yaw,string purpose,bool collision,bool uniform)
        {
            var asset=AssetDatabase.LoadAssetAtPath<GameObject>(path);if(!asset)throw new InvalidOperationException("Missing new model "+path);
            var root=Point(id+"_"+purpose,Vector3.zero).gameObject;var fit=new GameObject("SourceFit").transform;fit.SetParent(root.transform,false);
            var go=(GameObject)PrefabUtility.InstantiatePrefab(asset);go.transform.SetParent(fit,false);
            var renderers=go.GetComponentsInChildren<Renderer>();if(renderers.Length==0)throw new InvalidOperationException("Empty new source "+id);
            Bounds b=renderers[0].bounds;foreach(var r in renderers.Skip(1))b.Encapsulate(r.bounds);
            float extent=b.size.y;if(extent<.001f||Mathf.Max(b.size.x,b.size.z)>extent*5)extent=Mathf.Max(b.size.x,b.size.z);
            fit.localScale=uniform?Vector3.one*(size.y/Mathf.Max(.001f,extent)):new Vector3(size.x/Mathf.Max(.001f,b.size.x),b.size.y<.001f?1:size.y/b.size.y,b.size.z<.001f?1:size.z/b.size.z);
            foreach(var r in renderers){r.sharedMaterials=SourceSlots(id,r).Select(n=>MaterialFor(n,id)).ToArray();r.shadowCastingMode=ShadowCastingMode.On;}
            b=renderers[0].bounds;foreach(var r in renderers.Skip(1))b.Encapsulate(r.bounds);fit.position-=new Vector3(b.center.x,b.min.y,b.center.z);
            root.transform.SetPositionAndRotation(at,Quaternion.Euler(0,yaw,0));
            if(collision)
            {
                foreach(var mf in go.GetComponentsInChildren<MeshFilter>()){var collider=mf.gameObject.AddComponent<MeshCollider>();collider.sharedMesh=mf.sharedMesh;}
                var modifier=root.AddComponent<NavMeshModifier>();modifier.overrideArea=true;modifier.area=1;
            }
            var lods=go.GetComponentsInChildren<LODGroup>();if(lods.Length==0){var lod=root.AddComponent<LODGroup>();lod.SetLODs(new[]{new LOD(.008f,renderers)});lod.RecalculateBounds();}
            var marker=root.AddComponent<Phase5AssetSource>();marker.sourceId=id;marker.sourcePage=id=="FerrisWheel"?"https://sketchfab.com/3d-models/22d13b14e0a4455db9e4fb37cda960cc":"See Audit/phase5-asset-sources.json";marker.purpose=purpose;
            usage.Add(new Usage{sourceId=id,purpose=purpose,model=path,position=at});return root;
        }
        private static void SurfaceOn(GameObject go,string surface,Vector2 repeat)
        {
            var m=Object.Instantiate(Surface(surface));m.name="P5_"+surface+"_"+go.name;m.SetTextureScale("_BaseMap",repeat);m.SetTextureScale("_BumpMap",repeat);m.SetTextureScale("_MetallicGlossMap",repeat);m.SetTextureScale("_OcclusionMap",repeat);
            foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterials=r.sharedMaterials.Select(_=>m).ToArray();
        }
        private static GameObject Floor(string name,Vector3 center,Vector2 size,string surface)
        {
            var floor=Module("FacadeFloor",center,new Vector3(size.x,.01f,size.y),name,0,false);SurfaceOn(floor,surface,size/2);
            var c=floor.AddComponent<BoxCollider>();c.center=new Vector3(0,-.1f,0);c.size=new Vector3(size.x,.2f,size.y);return floor;
        }
        private static GameObject Wall(Vector3 at,Vector2 size,string purpose,float yaw=0,string surface="concrete_wall_007")
        {
            var wall=Module("FacadeWall",at,new Vector3(size.x,size.y,.12f),purpose,yaw,false);SurfaceOn(wall,surface,new Vector2(size.x/3,size.y/3));
            var c=wall.AddComponent<BoxCollider>();c.center=Vector3.up*size.y*.5f;c.size=new Vector3(size.x,size.y,.22f);return wall;
        }
        private static GameObject Blocker(string name,Vector3 at,Vector3 size)
        {var g=Point(name,at).gameObject;var c=g.AddComponent<BoxCollider>();c.size=size;return g;}
        private static Phase5WorldItem Item(string id,Vector3 at,string prompt,Vector3? size=null,Transform parent=null)
        {
            var g=Point("Interaction_"+id,at,parent).gameObject;var c=g.AddComponent<BoxCollider>();c.isTrigger=true;c.size=size??new Vector3(.75f,.6f,.4f);
            var item=g.AddComponent<Phase5WorldItem>();item.director=d;item.id=id;item.prompt=prompt;return item;
        }
        private static void Label(string name,string text,Vector3 at,float yaw=180,float scale=.012f)
        {
            var t=Point("Sign_"+name,at,null,yaw).gameObject.AddComponent<TextMesh>();t.text=text;t.characterSize=scale;t.font=PresentationProfile.Load().bodyFont;t.fontSize=64;t.anchor=TextAnchor.MiddleCenter;t.alignment=TextAlignment.Center;t.color=new Color(.78f,.75f,.63f);t.gameObject.layer=0;t.GetComponent<Renderer>().sharedMaterial=t.font.material;
        }
        private static Light Lamp(string name,Vector3 at,Color color,float power,float range,bool shadows=false)
        {var l=Point(name,at).gameObject.AddComponent<Light>();l.type=LightType.Point;l.color=color;l.intensity=power*8;l.range=range;l.shadows=shadows?LightShadows.Soft:LightShadows.None;return l;}
        private static Phase5Sound[] Sounds()
        {
            string folder=Imported+"Audio";Directory.CreateDirectory(folder);
            var files=Directory.GetFiles(folder,"*.ogg");AudioClip Find(string prefix)=>AssetDatabase.LoadAssetAtPath<AudioClip>(files.FirstOrDefault(f=>Path.GetFileName(f).StartsWith(prefix))??"");
            return new[]{"Ambient","Score","Cold","Crowd","Knock","HotDoor","Mirror","Cable","Incense","Water","Lunge","Reveal","FireWarning","Generator","Shield","Core","Miss","Light","EndingBad","EndingNormal","EndingTrue","EndingSecret"}.Select(id=>new Phase5Sound{id=id,clip=Find(id)}).ToArray();
        }
        public static void BuildLinuxReview()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            EditorUtility.UnloadUnusedAssetsImmediate();GC.Collect();
            PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Standalone,ScriptingImplementation.Mono2x);
            bool defaultApis=PlayerSettings.GetUseDefaultGraphicsAPIs(BuildTarget.StandaloneLinux64);
            var previousApis=PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneLinux64);
            UnityEditor.Build.Reporting.BuildReport report;
            try
            {
                // The Linux review harness uses -force-glcore; compile that API only.
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneLinux64,false);
                PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneLinux64,new[]{GraphicsDeviceType.OpenGLCore});
                report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),target=BuildTarget.StandaloneLinux64,locationPathName="Builds/LinuxPhase5/LayarTerakhir.x86_64",options=BuildOptions.Development});
            }
            finally
            {
                PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneLinux64,previousApis);
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneLinux64,defaultApis);
                AssetDatabase.SaveAssets();
            }
            File.WriteAllText("Audit/phase5-linux-build.json","{\"result\":\""+report.summary.result+"\",\"errors\":"+report.summary.totalErrors+",\"warnings\":"+report.summary.totalWarnings+",\"bytes\":"+report.summary.totalSize+"}\n");
            if(report.summary.result!=UnityEditor.Build.Reporting.BuildResult.Succeeded)throw new InvalidOperationException("Phase5 Linux build failed");
        }
    }
}
#endif
