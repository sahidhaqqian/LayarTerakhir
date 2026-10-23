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
        private Light[] fixtures;
        private bool[] authoredShadows;
        private float next;
        private readonly List<Material> localMaterials=new List<Material>();
        private void Start()
        {
            fixtures=FindObjectsByType<Light>(FindObjectsSortMode.None).Where(l=>l.name.EndsWith("Light")&&l.type==LightType.Point).ToArray();
            authoredShadows=fixtures.Select(l=>l.shadows!=LightShadows.None).ToArray();
            foreach(var renderer in player.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                // Scene-local tuning of the supplied VRM; never overwrite the designer asset.
                foreach(var material in renderer.materials)
                    {localMaterials.Add(material);if(material.HasProperty("_Smoothness"))material.SetFloat("_Smoothness",.32f);}
            }
        }
        private void Update()
        {
            if(!player||Time.unscaledTime<next||fixtures==null)return;next=Time.unscaledTime+.5f;
            for(int i=0;i<fixtures.Length;i++)
            {
                var light=fixtures[i];if(!light)continue;
                Vector3 delta=light.transform.position-player.position;
                light.enabled=delta.sqrMagnitude<30*30&&Mathf.Abs(delta.y)<5;
                light.shadows=authoredShadows[i]&&delta.sqrMagnitude<14*14?LightShadows.Soft:LightShadows.None;
            }
        }
        private void OnDestroy()
        {
            foreach(var material in localMaterials)if(material)Destroy(material);
        }
    }
}
