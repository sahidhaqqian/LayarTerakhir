using UnityEngine;
using UnityEngine.Audio;

namespace LayarTerakhir.Audio
{
    [CreateAssetMenu(menuName="LayarTerakhir/Scene audio profile")]
    public sealed class SceneAudioProfile : ScriptableObject
    {
        public AudioMixerGroup ambienceBus, effectsBus, uiBus;
        public AudioClip rain, roomTone, pcFan, phoneVibration;
        public AudioClip knock, cloth, latch, unlock, notification, uiClick, phoneSwitch, departureEngine;
        public AudioClip[] tileSteps;
        [Range(0,1)] public float roomGain=.4f, rainGain=.65f, pcGain=.55f;
        [Range(0,1)] public float effectsGain=.65f, uiGain=.4f;
        [Min(.1f)] public float fadeSeconds=1.5f;
    }
}
