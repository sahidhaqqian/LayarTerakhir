#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Incremental presentation pass: never reconstruct the approved rooms, road, stairs or gates.
    public static class DreadoutVisualRevision
    {
        private const string Art="Assets/_LayarTerakhir/assets/Art/";
        private const string Looks=Art+"Review/Lighting/";
        [Serializable] private class SceneReport
        {
            public string scene,collisionBefore,collisionAfter,profile;
            public int lights,cameras;public bool collisionUnchanged;
        }
        [Serializable] private class Report
        {
            public string utc;public bool passed;public SceneReport[] scenes;
            public string method="Serialized authoring checks; visual quality requires native captures. Original collider geometry and bindings retained.";
        }
        public static void Apply()
        {
            Directory.CreateDirectory(Looks);AssetDatabase.Refresh();ImportRoadMaps();Pipeline();
            var reports=new List<SceneReport>();
            foreach(var path in ScenePaths())
            {
                var scene=EditorSceneManager.OpenScene(path);string before=CollisionFingerprint();
                bool melati=path==KosCorridorBuilder.ScenePath,atmaja=path==AtmajaSceneBuilder.ScenePath;
                Cameras();Profile(atmaja?"Atmaja":melati?"Melati":"Kos",atmaja?.36f:melati?.18f:.12f);
                if(melati)Melati();if(atmaja)Atmaja();
                string after=CollisionFingerprint();if(before!=after)throw new InvalidOperationException("Presentation changed collision in "+scene.name);
                EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
                reports.Add(new SceneReport{scene=scene.name,collisionBefore=before,collisionAfter=after,collisionUnchanged=true,profile=Looks+(atmaja?"Atmaja":melati?"Melati":"Kos")+"ReferenceLook.asset",lights=Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None).Length,cameras=Object.FindObjectsByType<Camera>(FindObjectsInactive.Include,FindObjectsSortMode.None).Length});
            }
            AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/dreadout-look-authoring.json",JsonUtility.ToJson(new Report{utc=DateTime.UtcNow.ToString("o"),passed=true,scenes=reports.ToArray()},true)+"\n");
            Validate();Debug.Log("DREADOUT_LOOK_APPLIED HDR grading, smooth cloud sky, wet asphalt, warm practicals and cold abandoned interiors; collision unchanged");
        }
        private static string[] ScenePaths()=>new[]{PrologKosSceneBuilder.ScenePath,KosCorridorBuilder.ScenePath,AtmajaSceneBuilder.ScenePath};
        private static void Pipeline()
        {
            var pipeline=AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(Art+"Generated/KosPipeline.asset");
            pipeline.supportsHDR=true;pipeline.hdrColorBufferPrecision=HDRColorBufferPrecision._64Bits;
            pipeline.colorGradingMode=ColorGradingMode.HighDynamicRange;pipeline.colorGradingLutSize=64;
            pipeline.renderScale=1;pipeline.shadowDistance=38;pipeline.shadowCascadeCount=2;
            pipeline.mainLightShadowmapResolution=2048;pipeline.additionalLightsShadowmapResolution=2048;
            pipeline.maxAdditionalLightsCount=8;EditorUtility.SetDirty(pipeline);
            var renderer=AssetDatabase.LoadAssetAtPath<UniversalRendererData>(Art+"Generated/KosRenderer.asset");
            renderer.renderingMode=RenderingMode.ForwardPlus;
            foreach(var feature in renderer.rendererFeatures)
            {
                if(feature.GetType().Name!="ScreenSpaceAmbientOcclusion")continue;
                var serialized=new SerializedObject(feature);serialized.FindProperty("m_Settings.Intensity").floatValue=.6f;
                serialized.FindProperty("m_Settings.DirectLightingStrength").floatValue=.12f;
                serialized.FindProperty("m_Settings.Radius").floatValue=.22f;serialized.ApplyModifiedPropertiesWithoutUndo();
            }
            EditorUtility.SetDirty(renderer);
        }
        private static void Cameras()
        {
            foreach(var camera in Object.FindObjectsByType<Camera>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                camera.allowHDR=true;var data=camera.GetUniversalAdditionalCameraData();
                data.dithering=true;data.stopNaN=true;data.antialiasing=AntialiasingMode.SubpixelMorphologicalAntiAliasing;
                data.antialiasingQuality=AntialiasingQuality.High;data.renderPostProcessing=true;
            }
        }
        private static T Component<T>(VolumeProfile p) where T:VolumeComponent=>p.TryGet<T>(out var c)?c:p.Add<T>(true);
        private static void Profile(string name,float exposure)
        {
            string path=Looks+name+"ReferenceLook.asset";
            var profile=AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if(!profile){profile=ScriptableObject.CreateInstance<VolumeProfile>();AssetDatabase.CreateAsset(profile,path);}
            var tone=Component<Tonemapping>(profile);tone.mode.Override(TonemappingMode.Neutral);
            var color=Component<ColorAdjustments>(profile);color.postExposure.Override(exposure);color.contrast.Override(6);color.saturation.Override(-14);
            var white=Component<WhiteBalance>(profile);white.temperature.Override(name=="Kos"?2:-4);white.tint.Override(-1);
            var range=Component<ShadowsMidtonesHighlights>(profile);range.shadows.Override(new Vector4(.97f,.99f,1,.008f));range.midtones.Override(new Vector4(1,1,1,0));range.highlights.Override(new Vector4(1,.99f,.96f,0));
            var bloom=Component<Bloom>(profile);bloom.intensity.Override(.12f);bloom.threshold.Override(1.1f);bloom.scatter.Override(.5f);
            var vignette=Component<Vignette>(profile);vignette.intensity.Override(.20f);vignette.smoothness.Override(.65f);vignette.color.Override(new Color(.012f,.014f,.018f));
            var grain=Component<FilmGrain>(profile);grain.type.Override(FilmGrainLookup.Thin1);grain.intensity.Override(.085f);grain.response.Override(.7f);
            foreach(var c in profile.components){c.active=true;EditorUtility.SetDirty(c);if(!AssetDatabase.Contains(c))AssetDatabase.AddObjectToAsset(c,profile);}
            var volumes=Object.FindObjectsByType<Volume>(FindObjectsSortMode.None).Where(v=>v.isGlobal).ToArray();
            if(volumes.Length!=1)throw new InvalidOperationException("Expected one authored global volume in "+name+", found "+volumes.Length);
            volumes[0].sharedProfile=profile;volumes[0].weight=1;EditorUtility.SetDirty(profile);
        }
        private static void Melati()
        {
            Ambient(new Color(.22f,.24f,.27f),new Color(.13f,.145f,.16f),new Color(.06f,.057f,.05f),.0075f,new Color(.028f,.034f,.042f));
            var road=AssetDatabase.LoadAssetAtPath<Material>(Art+"Review/Materials/MelatiWetRoad.mat");
            road.SetColor("_BaseColor",new Color(.48f,.50f,.52f));
            road.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Review/Textures/asphalt_02_diff_1k.jpg"));
            road.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Review/Textures/asphalt_02_nor_gl_1k.jpg"));
            var mask=AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Review/Textures/asphalt_02_wet_mask.png");road.SetTexture("_MetallicGlossMap",mask);road.SetTexture("_OcclusionMap",mask);
            road.SetFloat("_Smoothness",.88f);road.SetFloat("_BumpScale",.42f);road.SetFloat("_OcclusionStrength",.35f);
            road.SetTextureScale("_BaseMap",Vector2.one*.51f);EditorUtility.SetDirty(road);
            Color[] paints={new Color(.94f,.88f,.74f),new Color(.70f,.82f,.75f),new Color(.90f,.73f,.62f),new Color(.78f,.84f,.87f)};
            foreach(var guid in AssetDatabase.FindAssets("t:Material",new[]{Art+"Review/Materials"}))
            {
                var m=AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(guid));if(!m.name.StartsWith("Melati_Type",StringComparison.Ordinal))continue;
                int variant=int.Parse(m.name.Split('_')[1].Substring(4));
                if(m.name.Contains("Plaster"))m.SetColor("_BaseColor",paints[variant%paints.Length]);
                if(m.name.Contains("Concrete"))m.SetColor("_BaseColor",new Color(.70f,.69f,.64f));
                m.SetFloat("_BumpScale",.42f);m.SetFloat("_OcclusionStrength",.45f);m.SetFloat("_Smoothness",.20f);EditorUtility.SetDirty(m);
            }
            foreach(var light in Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                if(light.name=="MelatiOvercastMoon"){light.intensity=.22f;light.color=new Color(.62f,.70f,.78f);light.shadowStrength=.85f;}
                if(light.name=="MelatiStreetLight")
                {
                    light.transform.rotation=Quaternion.LookRotation(new Vector3(0,.15f,light.transform.position.z)-light.transform.position);
                    light.intensity=24;light.range=16;light.spotAngle=100;light.innerSpotAngle=68;light.color=new Color(1,.83f,.61f);Shadow(light);Fixture(light,new Vector3(.52f,.065f,.22f));
                }
                if(light.name=="PorchPractical"){light.intensity=3.8f;light.range=6.2f;light.color=new Color(1,.85f,.65f);Shadow(light);Fixture(light,new Vector3(.12f,.16f,.12f));}
                if(light.name=="WarungWarmLight"){light.intensity=10;light.range=8;light.color=new Color(1,.90f,.73f);}
            }
            var ground=GameObject.Find("MelatiNeighborhoodGround");
            var dirt=CopyMaterial("MelatiDampSoil",Art+"Atmaja/Materials/stony_dirt_path.mat");dirt.SetColor("_BaseColor",new Color(.52f,.48f,.40f));dirt.SetFloat("_Smoothness",.15f);ground.GetComponent<Renderer>().sharedMaterial=dirt;
            var window=CopyMaterial("MelatiWindowWarm",Art+"Review/Materials/Review_HouseGlass.mat");window.SetColor("_BaseColor",new Color(.22f,.19f,.13f));window.EnableKeyword("_EMISSION");window.SetColor("_EmissionColor",new Color(.09f,.065f,.032f));EditorUtility.SetDirty(window);
            var darkWindow=AssetDatabase.LoadAssetAtPath<Material>(Art+"Review/Materials/Review_HouseGlass.mat");
            // Reset both variants so applying this pass again produces the same window pattern.
            int house=0;
            foreach(var t in GameObject.Find("KampungMelati").GetComponentsInChildren<Transform>().Where(t=>t.name.StartsWith("DownloadedHouse_Type_")))
            {
                var selected=house++%3==0?darkWindow:window;
                foreach(var r in t.GetComponentsInChildren<Renderer>())r.sharedMaterials=r.sharedMaterials.Select(m=>m&&(m.name=="Review_HouseGlass"||m.name=="MelatiWindowWarm")?selected:m).ToArray();
            }
            EditorUtility.SetDirty(dirt);
        }
        private static void Atmaja()
        {
            Ambient(new Color(.23f,.25f,.28f),new Color(.12f,.13f,.14f),new Color(.05f,.048f,.045f),.010f,new Color(.045f,.052f,.06f));
            foreach(var light in Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                if(light.name=="CloudedMoon"){light.intensity=.32f;light.color=new Color(.55f,.64f,.73f);light.shadowStrength=.85f;}
                if(light.name=="CorridorLight"){light.intensity=2.6f;light.color=new Color(.95f,.84f,.67f);Shadow(light);}
                if(light.name=="ChildRoomWindowSpill"){light.intensity=3.8f;light.color=new Color(.57f,.68f,.80f);}
                if(light.name=="CellarTeplokLight"){light.intensity=1.8f;light.color=new Color(1,.70f,.41f);Shadow(light);}
                if(light.name=="ChildRoomLight"||light.name=="MasterRoomLight")Shadow(light);
            }
        }
        private static void Ambient(Color sky,Color equator,Color ground,float density,Color fog)
        {RenderSettings.reflectionIntensity=.18f;RenderSettings.ambientMode=AmbientMode.Trilight;RenderSettings.ambientSkyColor=sky;RenderSettings.ambientEquatorColor=equator;RenderSettings.ambientGroundColor=ground;RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;RenderSettings.fogDensity=density;RenderSettings.fogColor=fog;}
        private static void Shadow(Light light)
        {light.shadows=LightShadows.Soft;light.shadowBias=.035f;light.shadowNormalBias=.22f;light.shadowResolution=LightShadowResolution.Low;var data=new SerializedObject(light.GetUniversalAdditionalLightData());data.FindProperty("m_AdditionalLightsShadowResolutionTier").intValue=0;data.ApplyModifiedPropertiesWithoutUndo();}
        private static Material CopyMaterial(string name,string source)
        {string path=Looks+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);if(!m){m=new Material(AssetDatabase.LoadAssetAtPath<Material>(source)){name=name};AssetDatabase.CreateAsset(m,path);}return m;}
        private static void Fixture(Light light,Vector3 size)
        {
            if(light.transform.Find("VisibleLampDiffuser"))return;
            string path=Looks+"PracticalDiffuser.mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));m.SetColor("_BaseColor",new Color(.82f,.73f,.56f));m.EnableKeyword("_EMISSION");m.SetColor("_EmissionColor",new Color(2.0f,1.60f,1.02f));AssetDatabase.CreateAsset(m,path);}
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name="VisibleLampDiffuser";Object.DestroyImmediate(go.GetComponent<Collider>());go.transform.SetParent(light.transform,false);go.transform.localPosition=new Vector3(0,0,-.05f);go.transform.localScale=size;go.GetComponent<Renderer>().sharedMaterial=m;
        }
        private static void ImportRoadMaps()
        {
            foreach(string name in new[]{"asphalt_02_diff_1k.jpg","asphalt_02_nor_gl_1k.jpg","asphalt_02_wet_mask.png"})
            {
                string path=Art+"Review/Textures/"+name;var importer=(TextureImporter)AssetImporter.GetAtPath(path);
                importer.textureType=name.Contains("nor_gl")?TextureImporterType.NormalMap:TextureImporterType.Default;
                importer.sRGBTexture=name.Contains("diff");importer.maxTextureSize=1024;importer.anisoLevel=8;importer.mipmapEnabled=true;importer.wrapMode=TextureWrapMode.Repeat;importer.SaveAndReimport();
            }
        }
        private static string CollisionFingerprint()
        {
            var rows=new List<string>();
            foreach(var c in Object.FindObjectsByType<Collider>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                var p=c.transform;string name=p.name;while(p.parent){p=p.parent;name=p.name+"/"+name;}
                rows.Add(name+"|"+EditorJsonUtility.ToJson(c)+"|"+c.transform.localToWorldMatrix.ToString("R"));
            }
            rows.Sort(StringComparer.Ordinal);using(var sha=SHA256.Create())return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(string.Join("\n",rows)))).Replace("-","").ToLowerInvariant();
        }
        public static void Validate()
        {
            var pipeline=AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(Art+"Generated/KosPipeline.asset");
            if(!pipeline.supportsHDR||pipeline.colorGradingMode!=ColorGradingMode.HighDynamicRange||pipeline.shadowDistance<30)throw new InvalidOperationException("Incomplete reference pipeline");
            foreach(var path in ScenePaths())
            {
                var scene=EditorSceneManager.OpenScene(path);var player=Object.FindFirstObjectByType<FirstPersonController>();
                if(!player||!player.GetComponent<InteractionInteractor>().playerCamera.allowHDR)throw new InvalidOperationException("Missing HDR exploration camera "+scene.name);
                var volume=Object.FindObjectsByType<Volume>(FindObjectsSortMode.None).Single(v=>v.isGlobal);
                if(!AssetDatabase.GetAssetPath(volume.sharedProfile).StartsWith(Looks,StringComparison.Ordinal))throw new InvalidOperationException("Unbound reference profile "+scene.name);
                foreach(var root in scene.GetRootGameObjects())foreach(var t in root.GetComponentsInChildren<Transform>(true))if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)>0)throw new InvalidOperationException("Missing script "+t.name);
                foreach(var r in Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include,FindObjectsSortMode.None))foreach(var m in r.sharedMaterials)if(!m||!m.shader||m.shader.name=="Hidden/InternalErrorShader")throw new InvalidOperationException("Missing material "+r.name);
                if(path==KosCorridorBuilder.ScenePath)MelatiStreetRevision.Validate();
            }
            Debug.Log("DREADOUT_LOOK_VALIDATED all three scenes, HDR cameras, profiles, materials, scripts, preserved pedestrian street");
        }
    }
}
#endif
