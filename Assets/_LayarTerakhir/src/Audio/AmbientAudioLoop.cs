using UnityEngine;

namespace LayarTerakhir.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class AmbientAudioLoop : MonoBehaviour
    {
        [Range(0,1)] public float gain=.5f;
        public float fadeSeconds=1.5f;
        public float LevelMultiplier { get; set; }=1f;
        private AudioSource source;
        private void Awake()
        {
            source=GetComponent<AudioSource>();source.playOnAwake=false;source.loop=true;source.volume=0;
        }
        private void Start() { if(source.clip)source.Play(); }
        private void Update()
        {
            source.volume=Mathf.MoveTowards(source.volume,gain*LevelMultiplier,Time.unscaledDeltaTime/Mathf.Max(.05f,fadeSeconds));
        }
        private void OnDisable() { if(source)source.Stop(); }
    }
}
