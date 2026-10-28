using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace LayarTerakhir.Phase1
{
    // Local fixture lights only: ritual, phone and transmission lights retain their ownership.
    // Limits overlapping realtime floors without reducing environment/texture quality.
    public sealed class Phase1LightBudget : MonoBehaviour
    {
        public Transform player;
        public bool isolateFloors;
        public Light[] externallyControlledLights;
        private Light[] fixtures;
        private bool[] authoredShadows;
        private float[] baseIntensity;
        private float next;
        private readonly List<Material> localMaterials=new List<Material>();
        private void Start()
        {
            fixtures=FindObjectsByType<Light>(FindObjectsSortMode.None).Where(l=>l.name.EndsWith("Light")&&l.type==LightType.Point&&
                (externallyControlledLights==null||!externallyControlledLights.Contains(l))).ToArray();
            authoredShadows=fixtures.Select(l=>l.shadows!=LightShadows.None).ToArray();
            baseIntensity=fixtures.Select(l=>l.intensity).ToArray();
            foreach(var renderer in player.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                // Scene-local tuning of the supplied VRM; never overwrite the designer asset.
                foreach(var material in renderer.materials)
                    {localMaterials.Add(material);if(material.HasProperty("_Smoothness"))material.SetFloat("_Smoothness",.32f);}
            }
        }
        private void Update()
        {
            if(!player||fixtures==null)return;
            // The L3 corridor neon is authored "nearly dead": a failing-ballast
            // flicker (dropouts + uneven hum) sells the sealed floor before any
            // threat appears. Deterministic per-fixture hash, no allocation.
            for(int i=0;i<fixtures.Length;i++)
            {
                var light=fixtures[i];
                if(!light||!light.enabled||!light.name.StartsWith("L3FailingFluorescent"))continue;
                float step=Mathf.Floor(Time.unscaledTime*9f);
                float n=Mathf.Abs(Mathf.Sin(i*127.1f+step*311.7f)*43758.5453f)%1f;
                light.intensity=baseIntensity[i]*(n<.16f?.22f:.72f+.28f*n);
            }
            if(Time.unscaledTime<next)return;next=Time.unscaledTime+.5f;
            for(int i=0;i<fixtures.Length;i++)
            {
                var light=fixtures[i];if(!light)continue;
                Vector3 delta=light.transform.position-player.position;
                bool sameFloor=!isolateFloors||Mathf.FloorToInt((light.transform.position.y-.5f)/4)==Mathf.FloorToInt((player.position.y+.5f)/4);
                light.enabled=sameFloor&&delta.sqrMagnitude<30*30&&Mathf.Abs(delta.y)<5;
                light.shadows=authoredShadows[i]&&delta.sqrMagnitude<14*14?LightShadows.Soft:LightShadows.None;
            }
        }
        private void OnDestroy()
        {
            foreach(var material in localMaterials)if(material)Destroy(material);
        }
    }
}
