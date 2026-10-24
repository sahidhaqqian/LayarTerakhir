using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public enum Phase3VoiceTag { Ambient, Authentic, Mimic }
    // Fictional authored emitter metadata; no microphone or inferred real-world classification.
    public sealed class Phase3EvpSource : MonoBehaviour
    {
        public string sourceId,subtitle;
        public new Phase3VoiceTag tag;
        public bool intercom;
        public int priority;
        public float range=15;
        // Proximity wins; author priority only breaks close ties. A distant vent cannot steal an intercom recording.
        public float FocusScore(Vector3 listener)=>tag==Phase3VoiceTag.Ambient?-1000:priority*.05f-Vector3.Distance(transform.position,listener);
        public Phase3VoiceTag Tag(float time,Phase3Balance b) => intercom ?
            time%(b.fakeSegment+b.realSegment)>=b.fakeSegment ? Phase3VoiceTag.Authentic:Phase3VoiceTag.Mimic:tag;
    }
}
