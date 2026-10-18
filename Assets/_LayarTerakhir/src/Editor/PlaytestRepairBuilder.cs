#if UNITY_EDITOR
using System;
using System.Linq;
using LayarTerakhir.Prolog;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class PlaytestRepairBuilder
    {
        public static void Apply()
        {
            PrologPresentationBuilder.RebuildRakaOnly();
            MelatiNeighborhoodBuilder.Apply();
            PolishCamera("Melati");
            EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            DesignerReviewBuilder.RepairChildRoom();
            var d=Object.FindFirstObjectByType<AtmajaDirector>();
            foreach(var light in Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None))
            {
                if(light.name=="CloudedMoon"){light.intensity=.42f;light.shadowStrength=.8f;}
                if(light.name=="CorridorLight"){light.intensity=2.3f;light.range=7;}
            }
            RenderSettings.ambientMode=AmbientMode.Trilight;
            RenderSettings.ambientSkyColor=new Color(.13f,.17f,.23f);
            RenderSettings.ambientEquatorColor=new Color(.055f,.068f,.085f);
            RenderSettings.ambientGroundColor=new Color(.025f,.028f,.034f);
            var old=GameObject.Find("ChildRoomWindowSpill");if(old)Object.DestroyImmediate(old);
            var fill=new GameObject("ChildRoomWindowSpill",typeof(Light));fill.transform.position=d.P(-5.65f,2.8f,5.55f);fill.transform.rotation=Quaternion.LookRotation(d.P(-3.2f,2.4f,4.2f)-fill.transform.position);
            var key=fill.GetComponent<Light>();key.type=LightType.Spot;key.intensity=2.8f;key.range=6;key.spotAngle=105;key.color=new Color(.43f,.58f,.77f);key.shadows=LightShadows.Soft;
            PolishCamera("Atmaja");
            foreach(var p in Object.FindObjectsByType<FirstPersonController>(FindObjectsSortMode.None)){p.walkSpeed=1.7f;p.sprintSpeed=4.2f;p.crouchSpeed=.8f;}
            Physics.SyncTransforms();EditorSceneManager.SaveScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene());AssetDatabase.SaveAssets();
            Debug.Log("PLAYTEST_REPAIRS_APPLIED pedestrian neighborhood, direct motor travel, dressed character, staffed shop, grounded child furniture, readable lighting");
        }
        private static void PolishCamera(string scene)
        {
            var player=Object.FindFirstObjectByType<FirstPersonController>();player.walkSpeed=1.7f;player.sprintSpeed=4.2f;player.crouchSpeed=.8f;
            var camera=player.GetComponent<InteractionInteractor>().playerCamera;camera.allowHDR=true;camera.GetUniversalAdditionalCameraData().antialiasing=AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            foreach(var volume in Object.FindObjectsByType<Volume>(FindObjectsSortMode.None))
            {
                if(!volume.sharedProfile)continue;
                string path="Assets/_LayarTerakhir/assets/Art/Review/"+scene+"Presentation.asset";
                var profile=AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
                if(!profile)
                {
                    profile=ScriptableObject.CreateInstance<VolumeProfile>();AssetDatabase.CreateAsset(profile,path);
                    foreach(var component in volume.sharedProfile.components){var copy=Object.Instantiate(component);profile.components.Add(copy);AssetDatabase.AddObjectToAsset(copy,profile);}
                }
                volume.sharedProfile=profile;
                if(profile.TryGet<FilmGrain>(out var grain))grain.intensity.value=.075f;
                if(profile.TryGet<Vignette>(out var vignette))vignette.intensity.value=.18f;
                if(profile.TryGet<ChromaticAberration>(out var fringe))fringe.intensity.value=.025f;
                if(profile.TryGet<ColorAdjustments>(out var color)){color.postExposure.value=.25f;color.contrast.value=8;color.saturation.value=-8;}
                EditorUtility.SetDirty(profile);
            }
        }
    }
}
#endif
