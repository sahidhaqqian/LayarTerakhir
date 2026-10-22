using UnityEngine;

namespace LayarTerakhir.Audio
{
    // Sound follows physical barriers, independently of the hunter's field of view.
    [RequireComponent(typeof(AudioLowPassFilter))]
    public sealed class ThreatAudioOcclusion : MonoBehaviour
    {
        private Transform listener,listenerRoot,emitterRoot;
        private AudioLowPassFilter filter;
        private readonly RaycastHit[] hits=new RaycastHit[24];
        private float nextProbe;
        public bool Occluded {get;private set;}
        public static ThreatAudioOcclusion Install(AudioSource source,Transform ear,Transform player,Transform ghost)
        {
            if(!source)return null;
            var occlusion=source.GetComponent<ThreatAudioOcclusion>();
            if(!occlusion)occlusion=source.gameObject.AddComponent<ThreatAudioOcclusion>();
            occlusion.listener=ear;occlusion.listenerRoot=player;occlusion.emitterRoot=ghost;
            occlusion.filter=occlusion.GetComponent<AudioLowPassFilter>();
            occlusion.filter.cutoffFrequency=22000;
            return occlusion;
        }
        public void Probe()
        {
            if(!listener||!emitterRoot)return;
            Vector3 from=emitterRoot.position+Vector3.up*1.15f;
            Vector3 delta=listener.position-from;
            int mask=~((1<<29)|(1<<2));
            int count=Physics.RaycastNonAlloc(from,delta.normalized,hits,delta.magnitude,mask,QueryTriggerInteraction.Ignore);
            var results=count==hits.Length?Physics.RaycastAll(from,delta.normalized,delta.magnitude,mask,QueryTriggerInteraction.Ignore):hits;
            if(results!=hits)count=results.Length;
            Occluded=false;
            for(int i=0;i<count;i++)
            {
                var t=results[i].transform;
                if(t.IsChildOf(emitterRoot)||(listenerRoot&&t.IsChildOf(listenerRoot)))continue;
                Occluded=true;break;
            }
        }
        private void LateUpdate()
        {
            if(!filter||!listener||!emitterRoot)return;
            if(Time.unscaledTime>=nextProbe){Probe();nextProbe=Time.unscaledTime+.1f;}
            filter.cutoffFrequency=Mathf.Lerp(filter.cutoffFrequency,Occluded?1400f:22000f,1-Mathf.Exp(-10*Time.unscaledDeltaTime));
        }
    }
}
