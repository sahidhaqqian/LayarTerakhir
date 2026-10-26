using System.Collections.Generic;
using LayarTerakhir.Prolog;
using UnityEngine;
using UnityEngine.Rendering;
namespace LayarTerakhir.Phase5
{
    // Standalone scenes need an explicit SH probe; an unbaked additive editor
    // scene otherwise inherits a black ambient probe despite its color setting.
    [DefaultExecutionOrder(100)]
    public sealed class Phase5Lighting : MonoBehaviour
    {
        public Camera playerCamera;
        private readonly List<Material> playerMaterials=new List<Material>();
        private void OnDestroy(){foreach(var material in playerMaterials)if(material)Destroy(material);}
        private void OnEnable()=>Apply();
        private void Start()=>Apply();
        public void TunePlayerMaterials(Transform avatar)
        {
            if(!avatar)return;
            foreach(var renderer in avatar.GetComponentsInChildren<Renderer>(true))
            {
                var shared=renderer.sharedMaterials;var local=new Material[shared.Length];
                for(int i=0;i<shared.Length;i++)
                {
                    if(!shared[i])continue;var material=local[i]=new Material(shared[i]);playerMaterials.Add(material);
                    if(material.HasProperty("_Metallic")){material.SetFloat("_Metallic",0);material.DisableKeyword("_METALLICSPECGLOSSMAP");}
                    if(material.HasProperty("_Smoothness"))material.SetFloat("_Smoothness",.20f);
                    if(material.HasProperty("_Glossiness"))material.SetFloat("_Glossiness",.20f);
                }
                renderer.sharedMaterials=local;
            }
        }
        public void Apply(bool dawn=false)
        {
            if(dawn)FindFirstObjectByType<LayarTerakhir.Player.AbandonedLocationLighting>()?.EnterDawn();
            Color ambient=dawn?new Color(.39f,.38f,.35f):new Color(.23f,.20f,.24f);
            Color sky=dawn?new Color(.28f,.31f,.32f):new Color(.12f,.037f,.052f);
            RenderSettings.skybox=null;RenderSettings.ambientMode=AmbientMode.Flat;
            RenderSettings.ambientLight=ambient;RenderSettings.ambientIntensity=1;
            var sh=new SphericalHarmonicsL2();sh.AddAmbientLight(ambient);RenderSettings.ambientProbe=sh;
            RenderSettings.fog=true;RenderSettings.fogMode=FogMode.ExponentialSquared;
            RenderSettings.fogDensity=dawn?.006f:.018f;RenderSettings.fogColor=sky;
            RenderSettings.reflectionIntensity=.45f;
            if(playerCamera){playerCamera.clearFlags=CameraClearFlags.SolidColor;playerCamera.backgroundColor=sky;}
            var phone=GetComponent<AtmajaPhoneView>();
            if(phone&&phone.FeedCamera){phone.FeedCamera.clearFlags=CameraClearFlags.SolidColor;phone.FeedCamera.backgroundColor=sky;}
        }
    }
}
