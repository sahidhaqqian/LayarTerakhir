#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Phase2;
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
    public static partial class Phase2SceneBuilder
    {
        public const string ScenePath="Assets/_LayarTerakhir/assets/Scenes/Babak2_RSUD.unity";
        private const string Art="Assets/_LayarTerakhir/assets/Art/Phase2/";
        private const string Imported=Art+"Imported/";
        private static Phase2Director d;
        private static Transform environment;
        private static readonly List<Phase2Anchor> anchors=new List<Phase2Anchor>();
        private static readonly List<Phase2Room> rooms=new List<Phase2Room>();
        private static readonly List<Phase2ThermalTarget> thermal=new List<Phase2ThermalTarget>();
        private static readonly List<Phase2Drawer> drawers=new List<Phase2Drawer>();
        private static readonly Dictionary<string,Material> materials=new Dictionary<string,Material>();
        private static readonly HashSet<string> usedModels=new HashSet<string>();

        [MenuItem("LayarTerakhir/Phase 2/Build Session 1 Hospital")]
        public static void Build()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            PrepareMaterials();
            var kos=EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var sourcePlayer=Object.FindFirstObjectByType<FirstPersonController>();
            var sourceSystems=GameObject.Find("GameSystems");
            if(!sourcePlayer||!sourceSystems)throw new InvalidOperationException("Golden Kos core systems unavailable");
            var sourceVolume=Object.FindFirstObjectByType<Volume>();var profile=sourceVolume?sourceVolume.sharedProfile:null;
            var events=Object.FindFirstObjectByType<EventSystem>();
            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);SceneManager.SetActiveScene(scene);
            var systems=Object.Instantiate(sourceSystems);systems.name="RSUD_GameSystems";
            Object.DestroyImmediate(systems.GetComponent<KamarKosPrologDirector>());
            foreach(var audio in systems.GetComponentsInChildren<KosSceneAudio>(true))Object.DestroyImmediate(audio.gameObject);
            var player=Object.Instantiate(sourcePlayer.gameObject);player.name="Player_RakaWijaya";
            if(events&&!events.transform.IsChildOf(sourceSystems.transform))Object.Instantiate(events.gameObject).name="EventSystem";
            EditorSceneManager.CloseScene(kos,true);
            d=systems.AddComponent<Phase2Director>();d.player=player.GetComponent<FirstPersonController>();d.world=systems.AddComponent<Phase2World>();
            var existingView=systems.GetComponent<AtmajaPhoneView>();d.phoneView=existingView?existingView:systems.AddComponent<AtmajaPhoneView>();
            d.phoneView.playerCamera=player.GetComponent<InteractionInteractor>().playerCamera;d.phoneView.feedMaterial=PresentationProfile.Load().phoneFeedMaterial;
            d.phoneView.playerCamera.farClipPlane=85;d.phoneView.playerCamera.clearFlags=CameraClearFlags.SolidColor;d.phoneView.playerCamera.backgroundColor=new Color(.012f,.018f,.025f);
            string config=Art+"Phase2Balance.asset";
            d.balance=AssetDatabase.LoadAssetAtPath<Phase2Balance>(config);
            if(!d.balance){d.balance=ScriptableObject.CreateInstance<Phase2Balance>();AssetDatabase.CreateAsset(d.balance,config);}
            d.lastri=systems.AddComponent<Phase2Lastri>();d.lastri.director=d;
            d.soundscape=systems.AddComponent<Phase2Soundscape>();d.soundscape.director=d;
            d.soundscape.routing=AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/assets/Audio/KosAudioProfile.asset");
            d.soundscape.sounds=Sounds();
            var signs=systems.AddComponent<AtmajaWorldText>();signs.includeARText=true;
            signs.materialTemplate=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/CompletionWorldText.mat");
            environment=new GameObject("RSUD_Lama_SidoWaras").transform;
            anchors.Clear();rooms.Clear();thermal.Clear();drawers.Clear();usedModels.Clear();
            Architecture();InvestigationObjects();DressHospital();Lastri();Lighting(profile);
            d.world.anchors=anchors.ToArray();d.world.rooms=rooms.ToArray();d.world.thermalTargets=thermal.ToArray();d.world.drawers=drawers.ToArray();
            // The existing navigation package bakes physical, multi-floor geometry.
            // Gate colliders are runtime owners, so exclude them only during baking.
            var gates=new[]{d.world.basementDoor,d.world.stairGrille,d.world.inkubatorWall};
            foreach(var gate in gates)gate.SetActive(false);
            d.world.dutyDoor.SetOpenImmediate(true);
            var navigation=environment.gameObject.AddComponent<NavMeshSurface>();navigation.collectObjects=CollectObjects.Children;
            navigation.useGeometry=NavMeshCollectGeometry.PhysicsColliders;navigation.layerMask=~((1<<25)|(1<<26)|(1<<29)|(1<<30)|(1<<31)|(1<<2));
            navigation.overrideVoxelSize=true;navigation.voxelSize=.06f;
            var settings=navigation.GetBuildSettings();settings.agentRadius=.3f;settings.agentHeight=1.75f;settings.agentClimb=.3f;
            var sources=new List<NavMeshBuildSource>();
            UnityEngine.AI.NavMeshBuilder.CollectSources(environment,navigation.layerMask,NavMeshCollectGeometry.PhysicsColliders,0,new List<NavMeshBuildMarkup>(),sources);
            navigation.navMeshData=UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(settings,sources,
                new Bounds(new Vector3(18,1,10),new Vector3(60,20,60)),Vector3.zero,Quaternion.identity);
            navigation.AddData();
            string navPath=Art+"RSUDNavMesh.asset";
            if(AssetDatabase.LoadAssetAtPath<UnityEngine.AI.NavMeshData>(navPath))AssetDatabase.DeleteAsset(navPath);
            if(!navigation.navMeshData)throw new InvalidOperationException("RSUD NavMesh bake failed");
            AssetDatabase.CreateAsset(navigation.navMeshData,navPath);
            foreach(var gate in gates)gate.SetActive(true);
            player.transform.SetPositionAndRotation(d.world.Anchor("IGD").position,d.world.Anchor("IGD").rotation);
            foreach(var behaviour in scene.GetRootGameObjects().SelectMany(go=>go.GetComponentsInChildren<MonoBehaviour>(true)))
                if(!behaviour||!MonoScript.FromMonoBehaviour(behaviour))throw new InvalidOperationException("Missing RSUD Unity script");
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene,ScenePath);
            EditorBuildSettings.scenes=EditorBuildSettings.scenes.Where(s=>s.path!=ScenePath).Concat(new[]{new EditorBuildSettingsScene(ScenePath,true)}).ToArray();
            AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/phase2-scene-content.json",JsonUtility.ToJson(new SceneRecord{scene=ScenePath,newModelPaths=usedModels.OrderBy(x=>x).ToArray(),
                anchors=anchors.Select(a=>a.id).ToArray(),thermalTargets=thermal.Select(t=>t.id).ToArray(),drawerCount=drawers.Count,
                floorElevations=new[]{-4f,0f,4f},scope="Session 1: investigation only; no boss / epilogue / P2 seal grant"},true)+"\n");
            Debug.Log("PHASE2_SESSION1_SCENE_BUILT models="+usedModels.Count+" thermal="+thermal.Count+" drawers="+drawers.Count);
        }
        [Serializable]private sealed class SceneRecord{public string scene,scope;public string[] newModelPaths,anchors,thermalTargets;public int drawerCount;public float[] floorElevations;}
        private static GameObject Prop(string name,string path,Vector3 p,Vector3 size,float yaw=0,bool collision=true)
        {
            var source=AssetDatabase.LoadAssetAtPath<GameObject>(Imported+path+".fbx");
            if(!source)throw new InvalidOperationException("Missing newly sourced model: "+path);
            usedModels.Add(Imported+path+".fbx");
            var go=(GameObject)PrefabUtility.InstantiatePrefab(source);
            if(path=="Additional/Models/Safe")PrefabUtility.UnpackPrefabInstance(go,PrefabUnpackMode.Completely,InteractionMode.AutomatedAction);
            go.name=name;
            var root=new GameObject(name+"_Placement");root.transform.SetParent(environment,false);
            var fitting=new GameObject(name+"_Fit").transform;fitting.SetParent(root.transform,false);go.transform.SetParent(fitting,false);
            var rs=go.GetComponentsInChildren<Renderer>();if(rs.Length==0)throw new InvalidOperationException("Empty source model "+path);
            Bounds bounds=rs[0].bounds;foreach(var r in rs.Skip(1))bounds.Encapsulate(r.bounds);
            // Some source uploads use Z-up geometry. Correct the imported assembly,
            // then fit via an identity wrapper. Scaling an FBX's rotated root swaps
            // height/depth and can turn a wall into a giant horizontal collider.
            bool upright=path.StartsWith("Madduck/")&&size.y>Mathf.Max(size.x,size.z)*1.2f&&bounds.size.y<Mathf.Max(bounds.size.x,bounds.size.z)*.8f;
            if(upright)
            {
                bool xUp=bounds.size.x>bounds.size.z;
                go.transform.Rotate(xUp?Vector3.forward:Vector3.right,xUp?90:-90,Space.World);
                bounds=rs[0].bounds;foreach(var r in rs.Skip(1))bounds.Encapsulate(r.bounds);
            }
            if(path=="Kenney/Stairs")
            {go.transform.Rotate(Vector3.up,90,Space.World);bounds=rs[0].bounds;foreach(var r in rs.Skip(1))bounds.Encapsulate(r.bounds);}
            if(path=="Autopsy/Models/AutopsyTable"&&bounds.size.z>bounds.size.x)
            {go.transform.Rotate(Vector3.up,90,Space.World);bounds=rs[0].bounds;foreach(var r in rs.Skip(1))bounds.Encapsulate(r.bounds);}
            Vector3 scale=new Vector3(size.x/Mathf.Max(.001f,bounds.size.x),size.y/Mathf.Max(.001f,bounds.size.y),size.z/Mathf.Max(.001f,bounds.size.z));
            fitting.localScale=scale;
            foreach(var r in rs)
            {
                r.sharedMaterials=r.sharedMaterials.Select(m=>ResolveMaterial(m,path)).ToArray();r.shadowCastingMode=ShadowCastingMode.On;
                r.reflectionProbeUsage=ReflectionProbeUsage.Simple;
            }
            bounds=rs[0].bounds;foreach(var r in rs.Skip(1))bounds.Encapsulate(r.bounds);
            fitting.position-=new Vector3(bounds.center.x,bounds.min.y,bounds.center.z);
            root.transform.SetPositionAndRotation(p,Quaternion.Euler(0,yaw,0));
            if(collision)
            {
                foreach(var mesh in go.GetComponentsInChildren<MeshFilter>())
                {var renderer=mesh.GetComponent<Renderer>();if(path.StartsWith("Neonatal/")&&renderer.sharedMaterials.All(m=>m.GetFloat("_Surface")>.5f))continue;var collider=mesh.gameObject.AddComponent<MeshCollider>();collider.sharedMesh=mesh.sharedMesh;}
            }
            return root;
        }
        private static GameObject M(string id,Vector3 p,Vector3 size,float yaw=0,bool collision=true)=>Prop(id,"Madduck/Models/"+id,p,size,yaw,collision);
        private static GameObject Medical(string name,string source,Vector3 p,Vector3 size,float yaw=0,bool collision=true)=>Prop(name,"Autopsy/Models/"+source,p,size,yaw,collision);
        private static GameObject PH(string id,Vector3 p,Vector3 size,float yaw=0,bool collision=true)=>Prop(id,"PolyHaven/"+id+"/"+id,p,size,yaw,collision);
        private static Transform Point(string name,Vector3 p,Transform parent=null,float yaw=0)
        {var go=new GameObject(name);go.transform.SetParent(parent?parent:environment,false);go.transform.SetPositionAndRotation(p,Quaternion.Euler(0,yaw,0));return go.transform;}
        private static void Anchor(string id,Vector3 p,float yaw=0)=>anchors.Add(new Phase2Anchor{id=id,point=Point("Anchor_"+id,p,null,yaw)});
        private static void Room(string id,float x0,float x1,float z0,float z1,float y)=>rooms.Add(new Phase2Room{id=id,bounds=new Bounds(new Vector3((x0+x1)/2,y+1.35f,(z0+z1)/2),new Vector3(x1-x0,2.8f,z1-z0))});
        private static Phase2WorldItem Item(GameObject go,string id,string prompt,Vector3? point=null)
        {
            var root=go;if(point.HasValue){root=Point(id+"_Interaction",point.Value,go.transform).gameObject;}
            var item=root.AddComponent<Phase2WorldItem>();item.director=d;item.id=id;item.prompt=prompt;
            if(!root.GetComponent<Collider>()) {var collider=root.AddComponent<BoxCollider>();collider.size=new Vector3(.34f,.28f,.22f);collider.isTrigger=true;}
            return item;
        }
        private static void Text(string name,string text,Vector3 p,float yaw=0,float size=.14f,int layer=0)
        {
            var go=Point(name,p,null,yaw).gameObject;go.layer=layer;
            var tm=go.AddComponent<TextMesh>();tm.text=text;tm.font=PresentationProfile.Load().bodyFont;tm.fontSize=64;tm.characterSize=size*.16f;
            tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;tm.color=new Color(.72f,.76f,.70f);
            go.GetComponent<MeshRenderer>().sharedMaterial=tm.font.material;
        }
        private static Phase2ThermalTarget Thermal(string id,Vector3 p,int temperature,float range)
        {var t=Point("Thermal_"+id,p).gameObject.AddComponent<Phase2ThermalTarget>();t.id=id;t.temperature=temperature;t.range=range;thermal.Add(t);return t;}
        private static Phase2Sound[] Sounds()
        {
            string folder="Assets/_LayarTerakhir/assets/Audio/Phase2/";
            var a=new List<Phase2Sound>();
            foreach(string id in new[]{"Moan","Breath","Refrigerator","Exterior","Echo","Hiss","Baby"})
                a.Add(new Phase2Sound{id=id,clip=AssetDatabase.LoadAssetAtPath<AudioClip>(folder+id+".wav")});
            a.Add(new Phase2Sound{id="EVP",clip=AssetDatabase.LoadAssetAtPath<AudioClip>(folder+"Echo.wav")});
            a.Add(new Phase2Sound{id="Gas",clip=AssetDatabase.LoadAssetAtPath<AudioClip>(folder+"Hiss.wav")});
            // Shared device UI/latch sounds are existing core presentation, not hospital art.
            var profile=PresentationProfile.Load().audio;
            a.Add(new Phase2Sound{id="Latch",clip=profile.latch});a.Add(new Phase2Sound{id="Thermo",clip=profile.notification});return a.ToArray();
        }
        public static Bounds WorldSkinBounds(SkinnedMeshRenderer skin)
        {
            var mesh=skin.sharedMesh;var vertices=mesh.vertices;var weights=mesh.boneWeights;var binds=mesh.bindposes;
            var matrices=skin.bones.Select((bone,i)=>bone.localToWorldMatrix*binds[i]).ToArray();
            Bounds bounds=new Bounds();bool first=true;
            for(int i=0;i<vertices.Length;i++)
            {
                var w=weights[i];var v=vertices[i];Vector3 p=matrices[w.boneIndex0].MultiplyPoint3x4(v)*w.weight0+
                    matrices[w.boneIndex1].MultiplyPoint3x4(v)*w.weight1+matrices[w.boneIndex2].MultiplyPoint3x4(v)*w.weight2+matrices[w.boneIndex3].MultiplyPoint3x4(v)*w.weight3;
                if(first){bounds=new Bounds(p,Vector3.zero);first=false;}else bounds.Encapsulate(p);
            }
            return bounds;
        }
        private static void Lastri()
        {
            var source=AssetDatabase.LoadAssetAtPath<GameObject>(Art+"Characters/Models/Lastri.fbx");
            if(!source)throw new InvalidOperationException("Designer Merah conversion missing");
            var root=new GameObject("Lastri_KuntilanakMerah");root.transform.SetParent(environment,false);root.transform.position=d.world.lastriHome.position;
            var visual=new GameObject("Lastri_VisualFit").transform;visual.SetParent(root.transform,false);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(source);model.transform.SetParent(visual,false);
            foreach(var part in root.GetComponentsInChildren<Transform>(true))part.gameObject.layer=Phase2Lastri.VisibilityLayer;
            foreach(var r in model.GetComponentsInChildren<Renderer>())r.sharedMaterial=materials["LastriSurface"];
            var anim=model.GetComponent<Animation>()??model.AddComponent<Animation>();
            foreach(var clip in AssetDatabase.LoadAllAssetsAtPath(Art+"Characters/Models/Lastri.fbx").OfType<AnimationClip>())
            {if(clip.name.StartsWith("__"))continue;clip.wrapMode=WrapMode.Loop;string name=clip.name.Split('|').Last();anim.AddClip(clip,name);if(name=="Idle")anim.clip=clip;}
            anim.playAutomatically=true;anim.wrapMode=WrapMode.Loop;
            if(!anim["Idle"])throw new InvalidOperationException("Lastri must expose the existing Idle clip");
            anim.Play("Idle");anim.Sample();
            Bounds SkinBounds()
            {
                Bounds bounds=new Bounds();bool first=true;
                foreach(var skin in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {skin.updateWhenOffscreen=true;var box=WorldSkinBounds(skin);if(first){bounds=box;first=false;}else bounds.Encapsulate(box);}
                if(first)throw new InvalidOperationException("Lastri has no skinned geometry");return bounds;
            }
            var body=SkinBounds();visual.localScale=Vector3.one*(1.8f/body.size.y);body=SkinBounds();
            visual.position+=new Vector3(root.transform.position.x-body.center.x,root.transform.position.y-body.min.y,root.transform.position.z-body.center.z);
            Debug.Log("P2_LASTRI_FIT height=1.8 scale="+visual.localScale+" body="+SkinBounds());
            var agent=root.AddComponent<NavMeshAgent>();agent.radius=.24f;agent.height=1.8f;agent.speed=3.3f;agent.acceleration=9;agent.angularSpeed=300;
            agent.stoppingDistance=.08f;agent.updateRotation=false;agent.obstacleAvoidanceType=ObstacleAvoidanceType.HighQualityObstacleAvoidance;
            d.world.lastri=root;d.lastri.agent=agent;
        }
    }
    public sealed class Phase2AssetImport : AssetPostprocessor
    {
        private void OnPreprocessModel()
        {
            if(!assetPath.StartsWith("Assets/_LayarTerakhir/assets/Art/Phase2/"))return;
            var importer=(ModelImporter)assetImporter;importer.isReadable=true;
            bool actor=assetPath.EndsWith("Lastri.fbx");importer.importAnimation=actor;
            importer.animationType=actor?ModelImporterAnimationType.Legacy:ModelImporterAnimationType.None;
            importer.materialImportMode=ModelImporterMaterialImportMode.ImportStandard;
        }
        private void OnPreprocessTexture()
        {
            if(!assetPath.StartsWith("Assets/_LayarTerakhir/assets/Art/Phase2/"))return;
            var t=(TextureImporter)assetImporter;t.maxTextureSize=2048;t.mipmapEnabled=true;t.anisoLevel=4;
            bool normal=assetPath.Contains("Normal");t.textureType=normal?TextureImporterType.NormalMap:TextureImporterType.Default;
            t.sRGBTexture=!normal&&!assetPath.Contains("MetalSmooth")&&!assetPath.Contains("MetalRough")&&!assetPath.Contains("ARM")&&!assetPath.Contains("Occlusion");
        }
    }
}
#endif
