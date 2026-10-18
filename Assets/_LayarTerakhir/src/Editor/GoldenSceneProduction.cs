#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LayarTerakhir.Audio;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.Presets;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // An incremental production pass. Never invokes the greybox builder or regenerates V2 furniture.
    public static class GoldenSceneProduction
    {
        private const string Root="Assets/_LayarTerakhir/assets/";
        private const string Art=Root+"Art/";
        private const string Production=Root+"Production/";
        private static Transform dressing;
        private static GameObject Find(string name)
        {
            var art=GameObject.Find("Kos_Environment_Art").transform;
            var t=art.Find(name)??art.Find("Kos_V2_Details/"+name)??art.Find("Golden_Workstation/"+name);
            return t?t.gameObject:GameObject.Find(name)??throw new Exception("Missing "+name);
        }
        [MenuItem("LayarTerakhir/Environment/Apply Golden scene production")]
        public static void Apply()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            Directory.CreateDirectory(Production+"Prefabs");Directory.CreateDirectory(Production+"Presets");
            ConfigureImports();EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            // Preserve the later developer-approved compact layout when using the original production menu.
            if(GameObject.Find("Compact_Kos_Details")){CompactKosPolish.Apply();return;}
            MoveWorkstation();Dress();Lighting();GoldenHudBuilder.Install();Audio();
            Physics.SyncTransforms();EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();AssetDatabase.SaveAssets();
            GoldenSceneValidation.Validate();Debug.Log("GOLDEN_PRODUCTION Applied environment, lighting, audio and shared HUD.");
        }
        private static void ConfigureImports()
        {
            foreach(var folder in new[]{"painted_wooden_stool","dirty_carpet"})
            foreach(var path in Directory.GetFiles(Art+"Imported/PolyHaven/"+folder).Where(p=>!p.EndsWith(".meta")))
            {
                if(AssetImporter.GetAtPath(path) is TextureImporter ti)
                {
                    ti.maxTextureSize=512;ti.textureCompression=TextureImporterCompression.Compressed;
                    ti.textureType=path.Contains("_nor_gl_")?TextureImporterType.NormalMap:TextureImporterType.Default;
                    ti.sRGBTexture=path.Contains("_diff_");ti.mipmapEnabled=true;ti.anisoLevel=2;ti.SaveAndReimport();
                    if(path.Contains("stool")&&(path.Contains("_diff_")||path.Contains("_nor_gl_")))
                    {
                        string preset=Production+"Presets/"+(path.Contains("_diff_")?"PropAlbedo512":"PropNormal512")+".preset";
                        if(!File.Exists(preset))AssetDatabase.CreateAsset(new Preset(ti),preset);
                    }
                }
            }
            foreach(var path in new[]{Art+"Imported/PolyHaven/painted_wooden_stool/painted_wooden_stool.fbx",Art+"Original/Models/KosBedMat.fbx",Art+"Original/Models/KosBudgetMicrophone.fbx"})
            {
                var mi=(ModelImporter)AssetImporter.GetAtPath(path);mi.importAnimation=false;mi.importCameras=false;mi.importLights=false;
                mi.materialImportMode=ModelImporterMaterialImportMode.None;mi.addCollider=false;mi.meshCompression=ModelImporterMeshCompression.Low;mi.SaveAndReimport();
            }
            foreach(var path in Directory.GetFiles(Root+"Audio/Prepared","*.wav"))
            {
                var ai=(AudioImporter)AssetImporter.GetAtPath(path);ai.forceToMono=true;ai.loadInBackground=false;
                var settings=ai.defaultSampleSettings;
                bool ambience=path.Contains("RainRoof")||path.Contains("RoomTone")||path.Contains("PCFan");
                settings.loadType=ambience?AudioClipLoadType.CompressedInMemory:AudioClipLoadType.DecompressOnLoad;
                settings.compressionFormat=ambience?AudioCompressionFormat.Vorbis:AudioCompressionFormat.ADPCM;
                settings.quality=.55f;settings.preloadAudioData=true;settings.sampleRateSetting=AudioSampleRateSetting.OverrideSampleRate;settings.sampleRateOverride=32000;
                ai.defaultSampleSettings=settings;ai.SaveAndReimport();
                string preset=Production+"Presets/"+(ambience?"AmbientMono32k":"ShortSfxMono32k")+".preset";
                if(!File.Exists(preset))AssetDatabase.CreateAsset(new Preset(ai),preset);
            }
        }
        private static void MoveWorkstation()
        {
            var group=GameObject.Find("Golden_Workstation");
            if(!group)
            {
                group=new GameObject("Golden_Workstation");group.transform.SetParent(Find("Kos_Environment_Art").transform,false);
                var director=Object.FindFirstObjectByType<KamarKosPrologDirector>();
                var objects=new[]{"wooden_table_02","painted_wooden_chair_01","OldPCTower","computerKeyboard","computerMouse","binder_notebook","EnamelMug","desk_lamp_arm_01",
                    "MejaKerjaPC","KakiMejaKiri","KakiMejaKanan","ChairSeatCollision","DeskLampPool","PCMonitorGlow","MonitorSignalCable","KeyboardCable","MouseCable"}.Select(Find)
                    .Concat(new[]{director.pcMonitorItem.gameObject,director.stickyNoteItem.gameObject,director.lockedDrawerItem.gameObject})
                    .Concat(Find("Kos_Environment_Art").transform.Cast<Transform>().Where(t=>t.name=="speakerSmall").Select(t=>t.gameObject)).Distinct().ToArray();
                foreach(var go in objects)if(!objects.Any(o=>go!=o&&go.transform.IsChildOf(o.transform)))go.transform.SetParent(group.transform,true);
            }
            // Open a 27cm breathing gap behind the workstation; move physical and interaction roots together.
            float delta=1.60f-KosVisualV2.BoundsOf(Find("wooden_table_02")).center.z;group.transform.position+=Vector3.forward*delta;
            var cable=Find("ComputerPowerCable").GetComponent<LineRenderer>();
            if(cable&&cable.positionCount>0)
            {var b=KosVisualV2.BoundsOf(Find("OldPCTower"));cable.SetPosition(cable.positionCount-1,new Vector3(b.center.x,.12f,b.max.z));}
        }
        private static Material Plain(string name,Color color,float smooth=.2f)
        {
            string path=Art+"Generated/Materials/"+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,path);}
            mat.SetColor("_BaseColor",color);mat.SetFloat("_Smoothness",smooth);mat.enableInstancing=true;EditorUtility.SetDirty(mat);return mat;
        }
        private static Material Pbr(string name,string source,Color tint)
        {
            var mat=Plain(name,tint,.18f);string dir=Art+"Imported/PolyHaven/"+source+"/";
            mat.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(dir+source+"_diff_1k.jpg"));
            mat.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(dir+source+"_nor_gl_1k.jpg"));
            var mask=AssetDatabase.LoadAssetAtPath<Texture2D>(dir+source+"_mask.png");mat.SetTexture("_MetallicGlossMap",mask);mat.SetTexture("_OcclusionMap",mask);
            mat.SetFloat("_OcclusionStrength",.65f);mat.EnableKeyword("_NORMALMAP");mat.EnableKeyword("_METALLICSPECGLOSSMAP");mat.EnableKeyword("_OCCLUSIONMAP");return mat;
        }
        private static GameObject Spawn(string name,string path,Vector3 center,Vector3 size,float yaw,Material mat)
        {
            var go=new GameObject(name);go.transform.SetParent(dressing,false);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Art+path));model.transform.SetParent(go.transform,false);
            go.transform.rotation=Quaternion.Euler(0,yaw,0);
            foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(mat,r.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(go,size);KosVisualV2.SetCenter(go,center);return go;
        }
        private static void Dress()
        {
            var old=GameObject.Find("Golden_Dressing");if(old)Object.DestroyImmediate(old);
            dressing=new GameObject("Golden_Dressing").transform;dressing.SetParent(Find("Kos_Environment_Art").transform,false);
            var stool=Spawn("BedsideWornStool","Imported/PolyHaven/painted_wooden_stool/painted_wooden_stool.fbx",new Vector3(-1.32f,.21f,-1.46f),new Vector3(.32f,.41f,.32f),8,Pbr("GoldenStool","painted_wooden_stool",Color.white));
            stool.transform.position+=Vector3.up*(KosVisualV2.FloorY-KosVisualV2.BoundsOf(stool).min.y+.001f);
            KosVisualV2.FitBox(stool.AddComponent<BoxCollider>(),KosVisualV2.BoundsOf(stool));
            PrefabUtility.SaveAsPrefabAsset(stool,Production+"Prefabs/WornBedsideStool.prefab");
            var mat=Pbr("GoldenBedMat","dirty_carpet",Color.white);mat.SetFloat("_Cull",0);mat.SetFloat("_BumpScale",.3f);mat.SetFloat("_OcclusionStrength",.3f);
            var rug=Spawn("BedsideMat","Original/Models/KosBedMat.fbx",new Vector3(-.97f,.016f,-.63f),new Vector3(1.18f,.006f,.78f),-4,mat);
            rug.transform.position+=Vector3.up*(KosVisualV2.FloorY+.002f-KosVisualV2.BoundsOf(rug).min.y);
            PrefabUtility.SaveAsPrefabAsset(rug,Production+"Prefabs/WornBedMat.prefab");
            var mic=Spawn("BudgetStreamingMicrophone","Original/Models/KosBudgetMicrophone.fbx",new Vector3(-1.74f,1f,1.36f),new Vector3(.115f,.235f,.115f),0,Plain("GoldenMicBody",new Color(.12f,.14f,.13f),.28f));
            foreach(var r in mic.GetComponentsInChildren<Renderer>())if(r.name.Contains("Grille"))r.sharedMaterial=Plain("GoldenMicGrille",new Color(.25f,.27f,.25f),.35f);
            mic.transform.position+=Vector3.up*(KosVisualV2.SurfaceHeight(Find("wooden_table_02"),-1.74f,1.36f)+.001f-KosVisualV2.BoundsOf(mic).min.y);
            PrefabUtility.SaveAsPrefabAsset(mic,Production+"Prefabs/BudgetMicrophone.prefab");
            // Keys share one mesh/renderer. The existing keyboard shell and its material remain intact.
            var keys=new GameObject("KeyboardKeycaps",typeof(MeshFilter),typeof(MeshRenderer));keys.transform.SetParent(dressing,false);
            var keyboard=KosVisualV2.BoundsOf(Find("computerKeyboard"));var combines=new System.Collections.Generic.List<CombineInstance>();
            var cube=GameObject.CreatePrimitive(PrimitiveType.Cube);var cubeMesh=cube.GetComponent<MeshFilter>().sharedMesh;Object.DestroyImmediate(cube);
            for(int row=0;row<4;row++)for(int col=0;col<14;col++)
            {
                var pos=new Vector3(keyboard.min.x+.018f+col*(keyboard.size.x-.036f)/13,keyboard.max.y+.0025f,keyboard.min.z+.012f+row*(keyboard.size.z-.024f)/3);
                combines.Add(new CombineInstance{mesh=cubeMesh,transform=Matrix4x4.TRS(pos,Quaternion.identity,new Vector3(.014f,.005f,.012f))});
            }
            string meshPath=Art+"Generated/GoldenKeyboardKeys.asset";var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
            if(!mesh){mesh=new Mesh();AssetDatabase.CreateAsset(mesh,meshPath);}else mesh.Clear();mesh.CombineMeshes(combines.ToArray());EditorUtility.SetDirty(mesh);
            keys.GetComponent<MeshFilter>().sharedMesh=mesh;keys.GetComponent<Renderer>().sharedMaterial=Plain("GoldenKeycaps",new Color(.19f,.20f,.18f),.25f);
        }
        private static void Lighting()
        {
            Find("WeakTungstenCeiling").GetComponent<Light>().intensity=13.5f;
            Find("WarmWallBounce").GetComponent<Light>().intensity=2.8f;
            Find("PCMonitorGlow").GetComponent<Light>().intensity=.7f;
            var glass=AssetDatabase.LoadAssetAtPath<Material>(Art+"Generated/Materials/NightWindowGlass.mat");
            glass.SetFloat("_Metallic",0);glass.SetFloat("_Smoothness",.2f);glass.SetFloat("_SpecularHighlights",0);glass.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
            glass.SetColor("_EmissionColor",new Color(.012f,.020f,.024f));EditorUtility.SetDirty(glass);
            var lighting=new GameObject("GoldenPracticalLighting");
            foreach(string name in new[]{"WeakTungstenCeiling","WarmWallBounce","ColdWindowSpill","DeskLampPool","PCMonitorGlow"})
            {var clone=Object.Instantiate(Find(name),lighting.transform,true);clone.name=name;}
            PrefabUtility.SaveAsPrefabAsset(lighting,Production+"Prefabs/GoldenPracticalLighting.prefab");Object.DestroyImmediate(lighting);
        }
        private static AudioSource Source(Transform parent,string name,AudioMixerGroup bus,Vector3 position,float spatial=1,float distance=5)
        {
            var go=new GameObject(name);go.transform.SetParent(parent,false);go.transform.position=position;
            var s=go.AddComponent<AudioSource>();s.playOnAwake=false;s.outputAudioMixerGroup=bus;s.spatialBlend=spatial;
            s.rolloffMode=AudioRolloffMode.Linear;s.minDistance=1;s.maxDistance=distance;s.dopplerLevel=0;s.priority=160;return s;
        }
        private static AudioClip Clip(string name)=>AssetDatabase.LoadAssetAtPath<AudioClip>(Root+"Audio/Prepared/"+name+".wav")??throw new Exception("Missing audio "+name);
        private static void Audio()
        {
            string mixerPath=Root+"Audio/GoldenScene.mixer";var mixer=AssetDatabase.LoadAssetAtPath<AudioMixer>(mixerPath);
            if(!mixer)
            {
                var type=AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType("UnityEditor.Audio.AudioMixerController")).FirstOrDefault(t=>t!=null);
                if(type==null)type=Assembly.Load("UnityEditor.AudioModule").GetType("UnityEditor.Audio.AudioMixerController");
                mixer=(AudioMixer)type.GetMethod("CreateMixerControllerAtPath",BindingFlags.Public|BindingFlags.Static).Invoke(null,new object[]{mixerPath});
                var master=type.GetProperty("masterGroup").GetValue(mixer);
                foreach(string name in new[]{"Ambience","Effects","UI"})
                {var group=type.GetMethod("CreateNewGroup").Invoke(mixer,new object[]{name,false});type.GetMethod("AddChildToParent").Invoke(mixer,new[]{group,master});}
                EditorUtility.SetDirty(mixer);AssetDatabase.SaveAssets();
            }
            var profile=AssetDatabase.LoadAssetAtPath<SceneAudioProfile>(Root+"Audio/KosAudioProfile.asset");
            if(!profile){profile=ScriptableObject.CreateInstance<SceneAudioProfile>();AssetDatabase.CreateAsset(profile,Root+"Audio/KosAudioProfile.asset");}
            profile.ambienceBus=mixer.FindMatchingGroups("Ambience").Single();profile.effectsBus=mixer.FindMatchingGroups("Effects").Single();profile.uiBus=mixer.FindMatchingGroups("UI").Single();
            profile.rain=Clip("RainRoof");profile.roomTone=Clip("RoomTone");profile.pcFan=Clip("PCFan");profile.phoneVibration=Clip("PhoneVibration");
            profile.knock=Clip("DoorKnock");profile.cloth=Clip("ClothPickup");profile.latch=Clip("DoorLatch");profile.unlock=Clip("DrawerUnlock");profile.notification=Clip("DonationChime");
            profile.uiClick=Clip("UIConfirm");profile.phoneSwitch=Clip("PhoneSwitch");profile.departureEngine=Clip("DepartureEngine");profile.tileSteps=Enumerable.Range(0,3).Select(i=>Clip("FootstepTile"+i)).ToArray();EditorUtility.SetDirty(profile);
            var old=GameObject.Find("GoldenAudioRig");if(old)Object.DestroyImmediate(old);
            var root=new GameObject("GoldenAudioRig");root.transform.SetParent(GameObject.Find("GameSystems").transform,false);
            var rig=root.AddComponent<KosSceneAudio>();rig.profile=profile;
            rig.room=Loop(root.transform,"RoomTone",profile.roomTone,profile,Vector3.up*1.5f,profile.roomGain,0);
            rig.roof=Loop(root.transform,"RainOnRoof",profile.rain,profile,new Vector3(.3f,2.8f,-.2f),profile.rainGain,.25f);
            rig.computer=Loop(root.transform,"PCFan",profile.pcFan,profile,KosVisualV2.BoundsOf(Find("OldPCTower")).center,profile.pcGain,1);
            var d=Object.FindFirstObjectByType<KamarKosPrologDirector>();
            rig.doorSource=Source(root.transform,"DoorAndCorridor",profile.effectsBus,d.kosDoorItem.transform.position,1,8);
            rig.itemSource=Source(root.transform,"ItemFoley",profile.effectsBus,Vector3.zero,.7f,5);
            rig.uiSource=Source(root.transform,"PhoneAndUI",profile.uiBus,Vector3.zero,0);rig.uiSource.priority=64;
            rig.phoneSource=Source(root.transform,"PhoneVibration",profile.effectsBus,d.smartphoneBedItem.transform.position,1,6);
            rig.phoneSource.clip=profile.phoneVibration;rig.phoneSource.loop=true;rig.phoneSource.volume=.24f;
            rig.stepsSource=Source(root.transform,"Footsteps",profile.effectsBus,Vector3.zero,.6f,5);
            rig.departureSource=Source(root.transform,"MotorcycleBeyondDoor",profile.effectsBus,new Vector3(1.2f,.6f,-2.55f),.65f,10);
            // Prefab has asset-only references. The two scene bindings are applied to its instance below.
            PrefabUtility.SaveAsPrefabAsset(root,Production+"Prefabs/KosEnvironmentalAudio.prefab");
            rig.player=Object.FindFirstObjectByType<FirstPersonController>();d.environmentAudio=rig;
        }
        private static AmbientAudioLoop Loop(Transform root,string name,AudioClip clip,SceneAudioProfile p,Vector3 pos,float gain,float spatial)
        {
            var s=Source(root,name,p.ambienceBus,pos,spatial,6);s.clip=clip;s.loop=true;s.volume=0;
            var loop=s.gameObject.AddComponent<AmbientAudioLoop>();loop.gain=gain;loop.fadeSeconds=p.fadeSeconds;return loop;
        }
    }
}
#endif
