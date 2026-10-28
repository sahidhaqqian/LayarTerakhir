using System;
using LayarTerakhir.Audio;
using UnityEngine;

namespace LayarTerakhir.Core
{
    [CreateAssetMenu(menuName="LayarTerakhir/Presentation profile")]
    public sealed class PresentationProfile:ScriptableObject
    {
        public Font bodyFont;
        public Material phoneFeedMaterial;
        public SceneAudioProfile audio;
        public VoiceLine[] voices;
        [Serializable] public struct VoiceLine { public string speaker,text; public AudioClip clip; }
        public AudioClip Voice(string speaker,string text)
        {
            if(voices==null)return null;
            foreach(var line in voices)if(line.speaker==speaker&&line.text==text)return line.clip;
            return null;
        }
        public static PresentationProfile Load()=>Resources.Load<PresentationProfile>("LayarPresentation");
    }
    // Separate hard modals from phone browsing: TAB/F remain usable while browsing apps.
    public static class UiFocus
    {
        public static bool Menu,Clock,Phone,Cinematic,PhoneAiming;
        public static bool ApplicationFocused = true;
        public static bool HardModal=>Menu||Clock;
        // Apps own the pointer, not the player's legs. World-object clicks still stay isolated.
        public static bool BlocksMovement=>!ApplicationFocused||HardModal||Cinematic;
        public static bool BlocksWorld=>!ApplicationFocused||HardModal||Phone||Cinematic;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Reset(){Menu=Clock=Phone=Cinematic=PhoneAiming=false;ApplicationFocused=true;}
    }
}
