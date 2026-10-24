#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
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
using Object=UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static partial class Phase3SceneBuilder
    {
        public const string ScenePath="Assets/_LayarTerakhir/assets/Scenes/Babak3_PabrikGula.unity";
        private const string Art="Assets/_LayarTerakhir/assets/Art/Phase3/",Imported=Art+"Imported/";
        private static Phase3Director d;
        private static Transform env;
        private static readonly List<Phase3Anchor> anchors=new List<Phase3Anchor>();
        private static readonly List<Phase3Zone> zones=new List<Phase3Zone>();
        private static readonly List<Phase3FloorTile> floors=new List<Phase3FloorTile>();
        private static readonly List<Phase3EvpSource> sources=new List<Phase3EvpSource>();
        private static readonly Vector3[] arenaHoles={new Vector3(78.5f,0,13.5f),new Vector3(84.5f,0,32.5f),new Vector3(90.5f,0,13.5f),new Vector3(75.5f,0,32.5f)};
        private static readonly List<Usage> usage=new List<Usage>();
        [Serializable]private class Usage{public string name,source,category;public Vector3 position,size;public float yaw;}
        [Serializable]private class SceneManifest{public string scene;public Usage[] placements;public string[] anchors;public int uniqueProps;public string scope;}
        [MenuItem("LayarTerakhir/Phase 3/Build Complete Factory")]
        public static void Build()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);PrepareMaterials();
            var kos=EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var p=Object.FindFirstObjectByType<FirstPersonController>();var systems=GameObject.Find("GameSystems");var events=Object.FindFirstObjectByType<EventSystem>();
            if(!p||!systems)throw new InvalidOperationException("Golden Kos systems unavailable");
            var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Additive);SceneManager.SetActiveScene(scene);
            var core=Object.Instantiate(systems);core.name="SindangLaut_GameSystems";
            Object.DestroyImmediate(core.GetComponent<KamarKosPrologDirector>());
            foreach(var a in core.GetComponentsInChildren<KosSceneAudio>(true))Object.DestroyImmediate(a.gameObject);
            var player=Object.Instantiate(p.gameObject);player.name="Player_RakaWijaya";
            if(events&&!events.transform.IsChildOf(systems.transform))Object.Instantiate(events.gameObject).name="EventSystem";
            EditorSceneManager.CloseScene(kos,true);
            d=core.AddComponent<Phase3Director>();d.player=player.GetComponent<FirstPersonController>();d.world=core.AddComponent<Phase3World>();
            d.phoneView=core.GetComponent<AtmajaPhoneView>()??core.AddComponent<AtmajaPhoneView>();d.phoneView.playerCamera=player.GetComponent<InteractionInteractor>().playerCamera;
            d.phoneView.feedMaterial=PresentationProfile.Load().phoneFeedMaterial;d.phoneView.playerCamera.farClipPlane=160;
            var cameraData=d.phoneView.playerCamera.GetUniversalAdditionalCameraData();cameraData.renderPostProcessing=true;
            cameraData.antialiasing=AntialiasingMode.SubpixelMorphologicalAntiAliasing;cameraData.antialiasingQuality=AntialiasingQuality.High;
            string bpath=Art+"Phase3Balance.asset";d.balance=AssetDatabase.LoadAssetAtPath<Phase3Balance>(bpath);
            if(!d.balance){d.balance=ScriptableObject.CreateInstance<Phase3Balance>();AssetDatabase.CreateAsset(d.balance,bpath);}
            d.wewe=core.AddComponent<Phase3Wewe>();d.wewe.director=d;d.boss=core.AddComponent<Phase3Boss>();d.boss.director=d;
            d.soundscape=core.AddComponent<Phase3Soundscape>();d.soundscape.director=d;
            d.soundscape.routing=AssetDatabase.LoadAssetAtPath<SceneAudioProfile>("Assets/_LayarTerakhir/assets/Audio/KosAudioProfile.asset");d.soundscape.sounds=Sounds();
            env=new GameObject("Pabrik_Gula_Sindang_Laut").transform;anchors.Clear();zones.Clear();floors.Clear();sources.Clear();usage.Clear();
            Architecture();Investigation();Arena();Dressing();Actor();Lighting();
            var uv=core.AddComponent<Phase3UvPresentation>();uv.director=d;uv.marks=env.GetComponentsInChildren<Renderer>(true).Where(r=>r.gameObject.layer==24).ToArray();
            var labels=core.AddComponent<AtmajaWorldText>();labels.includeARText=true;
            string textMat=Art+"Materials/Phase3WorldText.mat";var textMaterial=AssetDatabase.LoadAssetAtPath<Material>(textMat);
            if(!textMaterial){textMaterial=new Material(Shader.Find("LayarTerakhir/WorldText"));AssetDatabase.CreateAsset(textMaterial,textMat);}
            labels.materialTemplate=textMaterial;
            d.world.anchors=anchors.ToArray();d.world.zones=zones.ToArray();d.world.floors=floors.ToArray();d.world.sources=sources.ToArray();
            var gates=new[]{d.world.accessGate,d.world.hallGate,d.world.arenaLock,d.world.serviceGate,d.world.serviceDoor};foreach(var gate in gates)gate.SetActive(false);
            var navigation=env.gameObject.AddComponent<NavMeshSurface>();navigation.collectObjects=CollectObjects.Children;
            navigation.useGeometry=NavMeshCollectGeometry.PhysicsColliders;navigation.layerMask=~((1<<24)|(1<<26)|(1<<29)|(1<<31)|(1<<2));navigation.overrideVoxelSize=true;navigation.voxelSize=.12f;
            var settings=navigation.GetBuildSettings();settings.agentRadius=.4f;settings.agentHeight=d.wewe.agent.height+.05f;settings.agentClimb=.3f;
            var navSources=new List<NavMeshBuildSource>();UnityEngine.AI.NavMeshBuilder.CollectSources(env,navigation.layerMask,NavMeshCollectGeometry.PhysicsColliders,0,new List<NavMeshBuildMarkup>(),navSources);
            // Fragile boards remain physical player choices, but are never safe AI
            // navigation. Removing support at runtime cannot leave a path over air.
            for(int i=0;i<navSources.Count;i++)
            {
                var source=navSources[i];
                if(source.component&&floors.Any(tile=>tile.kind!=Phase3FloorKind.IllusionVoid&&tile.kind!=Phase3FloorKind.Solid&&source.component.transform.IsChildOf(tile.surface.transform)))
                {source.area=1;navSources[i]=source;}
            }
            navigation.navMeshData=UnityEngine.AI.NavMeshBuilder.BuildNavMeshData(settings,navSources,new Bounds(new Vector3(42,1,18),new Vector3(150,40,100)),Vector3.zero,Quaternion.identity);
            if(!navigation.navMeshData)throw new InvalidOperationException("P3 navigation bake failed");
            navigation.AddData();string nav=Art+"FactoryNavMesh.asset";if(AssetDatabase.LoadAssetAtPath<NavMeshData>(nav))AssetDatabase.DeleteAsset(nav);AssetDatabase.CreateAsset(navigation.navMeshData,nav);
            foreach(var gate in gates)gate.SetActive(true);d.world.serviceDoor.GetComponent<Phase3Gate>().SetOpen(true,true);
            player.transform.SetPositionAndRotation(d.world.Anchor("YARD").position,d.world.Anchor("YARD").rotation);
            foreach(var behaviour in scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<MonoBehaviour>(true)))if(!behaviour||!MonoScript.FromMonoBehaviour(behaviour))throw new InvalidOperationException("Missing P3 script");
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene,ScenePath);
            EditorBuildSettings.scenes=EditorBuildSettings.scenes.Where(x=>x.path!=ScenePath).Concat(new[]{new EditorBuildSettingsScene(ScenePath,true)}).ToArray();AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/phase3-scene-content.json",JsonUtility.ToJson(new SceneManifest{scene=ScenePath,placements=usage.ToArray(),anchors=anchors.Select(x=>x.id).ToArray(),uniqueProps=usage.Where(x=>x.category=="PROP").Select(x=>x.source).Distinct().Count(),scope="Five scenes of Phase 3 in one connected factory; shared Raka/phone/motor, entirely new factory environment"},true)+"\n");
            Debug.Log("PHASE3_FACTORY_BUILT props="+usage.Where(x=>x.category=="PROP").Select(x=>x.source).Distinct().Count()+" placements="+usage.Count);
        }
        public static void BuildLinuxReview()
        {
            PlayerSettings.SetScriptingBackend(UnityEditor.Build.NamedBuildTarget.Standalone,ScriptingImplementation.Mono2x);
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=EditorBuildSettings.scenes.Where(s=>s.enabled).Select(s=>s.path).ToArray(),target=BuildTarget.StandaloneLinux64,locationPathName="Builds/LinuxPhase3/LayarTerakhir.x86_64",options=BuildOptions.Development});
            System.IO.File.WriteAllText("Audit/phase3-linux-build.json",JsonUtility.ToJson(new BuildRecord{result=report.summary.result.ToString(),utc=DateTime.UtcNow.ToString("o"),errors=report.summary.totalErrors,warnings=report.summary.totalWarnings,bytes=report.summary.totalSize},true)+"\n");
            if(report.summary.result!=UnityEditor.Build.Reporting.BuildResult.Succeeded||report.summary.totalErrors!=0)throw new Exception("P3 native player build failed or reported errors");
        }
        public static void BuildAndLinuxReview(){Build();BuildLinuxReview();}
        [Serializable]private class BuildRecord{public string result,utc;public int errors,warnings;public ulong bytes;}
        private static GameObject Prop(string name,string path,Vector3 at,Vector3 size,float yaw=0,bool collision=true,string category="PROP",Vector3? rotate=null)
        {
            var model=AssetDatabase.LoadAssetAtPath<GameObject>(Imported+path+".fbx");if(!model)throw new InvalidOperationException("Missing new P3 model "+path);
            var root=new GameObject(name);root.transform.SetParent(env,false);var fit=new GameObject("SourceFit").transform;fit.SetParent(root.transform,false);
            var go=(GameObject)PrefabUtility.InstantiatePrefab(model);go.transform.SetParent(fit,false);if(rotate.HasValue)go.transform.Rotate(rotate.Value,Space.World);
            var renderers=go.GetComponentsInChildren<Renderer>();if(renderers.Length==0)throw new InvalidOperationException("Empty P3 source "+path);
            Bounds bounds=renderers[0].bounds;foreach(var r in renderers.Skip(1))bounds.Encapsulate(r.bounds);
            fit.localScale=category=="CHARACTER"?Vector3.one*(size.y/Mathf.Max(.001f,bounds.size.y)):new Vector3(size.x/Mathf.Max(.001f,bounds.size.x),size.y/Mathf.Max(.001f,bounds.size.y),size.z/Mathf.Max(.001f,bounds.size.z));
            foreach(var r in renderers){r.sharedMaterials=r.sharedMaterials.Select(m=>MaterialFor(m?m.name:"Surface",path)).ToArray();r.shadowCastingMode=ShadowCastingMode.On;}
            bounds=renderers[0].bounds;foreach(var r in renderers.Skip(1))bounds.Encapsulate(r.bounds);
            fit.position-=new Vector3(bounds.center.x,bounds.min.y,bounds.center.z);root.transform.SetPositionAndRotation(at,Quaternion.Euler(0,yaw,0));
            if(collision)foreach(var mesh in go.GetComponentsInChildren<MeshFilter>()){var c=mesh.gameObject.AddComponent<MeshCollider>();c.sharedMesh=mesh.sharedMesh;}
            foreach(var t in root.GetComponentsInChildren<Transform>())GameObjectUtility.SetStaticEditorFlags(t.gameObject,StaticEditorFlags.BatchingStatic);
            usage.Add(new Usage{name=name,source=Imported+path+".fbx",category=category,position=at,size=size,yaw=yaw});return root;
        }
        private static GameObject PH(string id,Vector3 p,Vector3 size,float yaw=0,bool collision=true)=>Prop(id,"PolyHaven/"+id+"/"+id,p,size,yaw,collision);
        private static GameObject Converted(string id,Vector3 p,Vector3 size,float yaw=0,bool collision=true)=>Prop(id,"Converted/Models/"+id,p,size,yaw,collision);
        private static GameObject Module(string group,string id,Vector3 p,Vector3 size,float yaw=0,bool collision=true,Vector3? rotate=null)=>Prop(id,"Modules/"+group+"/"+id,p,size,yaw,collision,"ARCHITECTURE",rotate);
        private static Transform Point(string name,Vector3 at,Transform parent=null,float yaw=0)
        {var go=new GameObject(name);go.transform.SetParent(parent?parent:env,false);go.transform.SetPositionAndRotation(at,Quaternion.Euler(0,yaw,0));return go.transform;}
        private static void Anchor(string id,Vector3 at,float yaw=0)=>anchors.Add(new Phase3Anchor{id=id,point=Point("Anchor_"+id,at,null,yaw)});
        private static void Zone(string id,Vector3 centre,Vector3 size)=>zones.Add(new Phase3Zone{id=id,bounds=new Bounds(centre,size)});
        private static Phase3WorldItem Item(GameObject target,string id,string prompt,Vector3? marker=null)
        {
            var item=target.AddComponent<Phase3WorldItem>();item.director=d;item.id=id;item.prompt=prompt;
            if(marker.HasValue){var child=Point(id+"_Interaction",marker.Value,target.transform);var c=child.gameObject.AddComponent<SphereCollider>();c.radius=.18f;c.isTrigger=true;}
            else if(!target.GetComponentInChildren<Collider>()){var c=target.AddComponent<BoxCollider>();c.size=new Vector3(.5f,1,.5f);c.center=Vector3.up*.5f;}
            return item;
        }
        private static void Label(string name,string text,Vector3 p,float yaw=0,float size=.14f,int layer=0)
        {
            var go=Point(name,p,null,yaw).gameObject;go.layer=layer;var t=go.AddComponent<TextMesh>();t.text=text;t.font=PresentationProfile.Load().bodyFont;t.fontSize=64;t.characterSize=size*.16f;t.anchor=TextAnchor.MiddleCenter;t.alignment=TextAlignment.Center;t.color=layer==24?new Color(.78f,.45f,1):new Color(.74f,.74f,.65f);go.GetComponent<Renderer>().sharedMaterial=t.font.material;
        }
        private static Phase3EvpSource Emitter(string id,Vector3 p,Phase3VoiceTag tag,string subtitle,int priority=1,bool intercom=false)
        {var s=Point("EVP_"+id,p).gameObject.AddComponent<Phase3EvpSource>();s.sourceId=id;s.tag=tag;s.priority=priority;s.subtitle=subtitle;s.intercom=intercom;sources.Add(s);return s;}
        private static void Floor(float x0,float x1,float z0,float z1,float y,string surface="factory_wall")
        {
            for(float x=x0;x<x1-.01f;x+=3)for(float z=z0;z<z1-.01f;z+=3)
            {
                float w=Mathf.Min(3,x1-x),h=Mathf.Min(3,z1-z);
                Vector3 centre=new Vector3(x+w/2,y,z+h/2);
                bool arenaHole=Mathf.Abs(y)<.01f&&arenaHoles.Any(p=>Vector3.Distance(p,centre)<.1f);
                bool balconyHole=Mathf.Abs(y-4)<.01f&&(Vector3.Distance(centre,new Vector3(58.5f,4,43.5f))<.1f||Vector3.Distance(centre,new Vector3(39.5f,4,31.5f))<.1f);
                if(arenaHole||balconyHole)continue;
                var go=Module("modular_factory_facade","wall_standard_standard_01",new Vector3(x+w/2,y-.12f,z+h/2),new Vector3(w,.12f,h),0,false,new Vector3(y>=9?90:-90,0,0));
                foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterial=Surface(surface);
                var c=go.AddComponent<BoxCollider>();c.size=new Vector3(w,.12f,h);c.center=Vector3.up*.06f;
            }
        }
        private static void Wall(Vector3 p,float width,float height,float yaw=0,bool windows=false)
        {
            for(float x=-width/2;x<width/2-.01f;x+=3)
            {
                float span=Mathf.Min(3,width/2-x);Vector3 at=p+Quaternion.Euler(0,yaw,0)*Vector3.right*(x+span/2);
                string id=windows?"wall_window_centered_large_01":"wall_standard_standard_01";
                var go=Module("modular_factory_facade",id,at,new Vector3(span,height,.25f),yaw,false);
                if(!windows)foreach(var renderer in go.GetComponentsInChildren<Renderer>())
                {
                    string key="WallBrick_"+span.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture)+"_"+height.ToString("0.##",System.Globalization.CultureInfo.InvariantCulture);
                    if(!mats.TryGetValue(key,out var wallMaterial))
                    {
                        string path=Art+"Materials/"+key+".mat";wallMaterial=AssetDatabase.LoadAssetAtPath<Material>(path);
                        if(!wallMaterial){wallMaterial=new Material(Surface("red_brick_03"));AssetDatabase.CreateAsset(wallMaterial,path);}
                        wallMaterial.CopyPropertiesFromMaterial(Surface("red_brick_03"));
                        wallMaterial.SetTextureScale("_BaseMap",new Vector2(span/3,height/3));mats[key]=wallMaterial;
                        EditorUtility.SetDirty(wallMaterial);
                    }
                    renderer.sharedMaterial=wallMaterial;
                }
                var c=go.AddComponent<BoxCollider>();c.center=Vector3.up*height/2;c.size=new Vector3(span,height,.25f);
                if(windows)Module("modular_factory_facade","window_centered_large_01",at+Vector3.up*height*.34f,new Vector3(span*.73f,height*.56f,.25f),yaw,false);
            }
        }
        private static void Architecture()
        {
            Floor(-3,27,-20,3,0,"concrete_floor_02");Floor(0,24,0,6,0);Floor(0,24,28,32,0);Floor(15,24,6,28,0,"metal_grate_rusty");Floor(0,4,6,28,0,"metal_grate_rusty");
            Wall(new Vector3(12,0,-20),30,2.5f);Wall(new Vector3(-3,0,-8.5f),23,2.5f,90);Wall(new Vector3(27,0,-12.5f),15,2.5f,90);
            // The false bridge has real gaps over an 8m-deep grinding pit.
            Floor(5,10,6,12,0,"brown_planks_09");Floor(5,10,20,28,0,"brown_planks_09");
            Floor(0,24,0,32,-8);Wall(new Vector3(0,-8,16),32,18,90);Wall(new Vector3(24,-8,10),20,18,90);Wall(new Vector3(24,0,29),6,10,90);
            Wall(new Vector3(12,0,32),24,10);Wall(new Vector3(5,0,0),10,10);Wall(new Vector3(21,0,0),6,10);
            Wall(new Vector3(14,4.5f,0),8,5.5f);
            foreach(float x in new[]{3f,7f,21f,24f})Module("modular_factory_facade","window_centered_large_01",new Vector3(x,5.8f,-.18f),new Vector3(2.1f,2.5f,.28f),0,false);
            for(int x=0;x<24;x+=3)Module("modular_factory_facade","cornice01_standard_standard_01",new Vector3(x+1.5f,9.7f,-.05f),new Vector3(3,.35f,.5f),0,false);
            Floor(24,64,8,42,0,"concrete_floor_02");Wall(new Vector3(44,0,8),40,9);
            Wall(new Vector3(34,0,42),20,9,0,true);Wall(new Vector3(53,0,42),10,9,0,true);
            Wall(new Vector3(46,0,42),4,4);Wall(new Vector3(46,7.5f,42),4,1.5f);
            Wall(new Vector3(63,0,42),2,9);Floor(58,64,39,42,0,"metal_grate_rusty");Floor(42,64,42,48,0,"concrete_floor_02");Stairs(new Vector3(62.5f,0,43),new Vector3(1.4f,4,8),90);Wall(new Vector3(64,0,13),10,9,90);Wall(new Vector3(64,0,35),14,9,90);
            // Office and service balcony are a separate, connected upper level.
            Floor(42,60,30,42,4,"brown_planks_09");Floor(42,62,42,48,4,"metal_grate_rusty");Floor(60,64,46,48,4,"metal_grate_rusty");Wall(new Vector3(42,0,45),6,4,90);Wall(new Vector3(53,0,48),22,2.5f);
            Wall(new Vector3(42,4,37.5f),9,4,90,true);Wall(new Vector3(60,4,37),14,4,90,true);Wall(new Vector3(51,4,48),18,2);
            Stairs(new Vector3(57,0,24),new Vector3(1.8f,4,12),90);Stairs(new Vector3(39,0,25),new Vector3(1.8f,4,12),90);
            Floor(38,44,30,34,4,"metal_grate_rusty");
            // Bagasse has two human-sized exits, never a locked dead end.
            Wall(new Vector3(24,0,34),16,7,90);Wall(new Vector3(30,0,40),12,7);Wall(new Vector3(34,0,30),4,5,90);
            Floor(64,70,22,27,0,"metal_grate_rusty");
            // Separate service return follows the outside southern wall all the way to the yard.
            Floor(12,96,-5,0,0,"metal_grate_rusty");Floor(94,99,-5,24,0,"metal_grate_rusty");Floor(92,99,22,27,0,"metal_grate_rusty");
            Wall(new Vector3(96.5f,0,28),12,8);Wall(new Vector3(96.5f,0,-5),12,3);Wall(new Vector3(99,0,10),30,4,90);
            Wall(new Vector3(58.5f,0,-5),63,1.5f);Wall(new Vector3(60.5f,0,0),67,1.5f);Wall(new Vector3(96.5f,0,27),7,3);
            Zone("OFFICE",new Vector3(51,5.5f,36),new Vector3(18,3,12));Zone("BALCONY",new Vector3(52,5.5f,45),new Vector3(22,3,6));Zone("BAGASSE",new Vector3(29,1.5f,34),new Vector3(10,3,12));
            Zone("ARENA",new Vector3(82,3,24),new Vector3(28,8,28));Zone("BOILER",new Vector3(44,1.5f,25),new Vector3(40,3,34));Zone("MILL",new Vector3(12,1.5f,16),new Vector3(24,3,32));
            Anchor("YARD",new Vector3(13,.04f,-12));Anchor("MILL",new Vector3(13,.04f,3));Anchor("BOILER",new Vector3(27,.04f,24),90);Anchor("OFFICE",new Vector3(55,4.04f,32),0);Anchor("BAGASSE",new Vector3(32,.04f,29),0);Anchor("ALTAR",new Vector3(71,.04f,24),90);
            Anchor("LURE_MILL",new Vector3(7,.1f,24));
            // Roof support and cornice are downloaded architectural pieces, no authored primitive art.
            for(int x=0;x<64;x+=6)Module("modular_factory_facade","cornice01_standard_standard_01",new Vector3(x+3,9.5f,32),new Vector3(6,.3f,.4f));
            Floor(0,24,0,32,10,"corrugated_iron_02");Floor(24,64,8,42,10,"corrugated_iron_02");
            Label("FactorySign","PABRIK GULA SINDANG LAUT\n1912 — 1999",new Vector3(14,5.4f,-.3f),0,.42f);
            Label("Weighbridge","HALAMAN TIMBANGAN TEBU\nPOS TIMBANG",new Vector3(2,2,-6),0,.22f);
            Label("BoilerSign","BANGSAL KETEL UAP",new Vector3(24.4f,3.5f,24),90,.24f);
            Label("OfficeSign","KANTOR MANDOR\nARSIP 1999",new Vector3(52,6.5f,30),0,.2f);
        }
        private static void Stairs(Vector3 p,Vector3 size,float rotation)
        {
            var assembly=Module("modular_fire_escape","modular_fire_escape_stairs",p,size,0,false,new Vector3(0,rotation,0));
            // Continuous tread collision for the artist's open grating: avoids NavMesh losing thin steel strips.
            // These are collision proxies only, never replacement visual meshes.
            int count=24;float rise=size.y/count,run=size.z/count;
            for(int i=0;i<count;i++)
            {
                var tread=new GameObject("TreadCollision_"+i);tread.transform.SetParent(assembly.transform,false);
                tread.transform.localPosition=new Vector3(0,rise*i,-size.z/2+run*(i+.5f));
                var collider=tread.AddComponent<BoxCollider>();collider.size=new Vector3(size.x,rise,run+.015f);collider.center=Vector3.up*rise/2;
            }
        }
        private static Phase3Sound[] Sounds()
        {
            string folder="Assets/_LayarTerakhir/assets/Audio/Phase3/";
            var mapping=new[]{("Ambient","Factory"),("Machinery","Shop_0"),("Spirit","Searching"),("Impact","mechanical_explosion"),("Collapse","mechanical_explosion"),("Creak","squeakyclick1"),("Cloth","Ghost_3"),("Mimic","Ghost_1"),("Humming","ghostly_humming"),("AuthenticVoice","Ghost_0"),("Knock","clank1"),("Step","clank1"),("Generator","mechanical1"),("Shutter","clank1")};
            return mapping.Select(x=>new Phase3Sound{id=x.Item1,clip=AssetDatabase.LoadAssetAtPath<AudioClip>(folder+x.Item2+".ogg")}).ToArray();
        }
    }
}
#endif
