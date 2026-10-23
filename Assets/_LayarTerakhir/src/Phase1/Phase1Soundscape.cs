using System;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using UnityEngine;

namespace LayarTerakhir.Phase1
{
    [Serializable] public sealed class Phase1Sound { public string id; public AudioClip clip; public float gain = .6f; }
    public sealed class Phase1Soundscape : MonoBehaviour
    {
        public Phase1Director director;
        public SceneAudioProfile routing;
        public Phase1Sound[] sounds;
        private AudioSource[] pool;
        private AudioSource cry, rain, room;
        private AudioLowPassFilter rainFilter;
        private int cursor;
        private float nextDrip, targetRoom = .08f;
        private void Start()
        {
            pool = new AudioSource[8];
            for (int i = 0; i < pool.Length; i++) pool[i] = Source("RusunawaFoley" + i, false);
            rain = Source("RainOverOpenVoid", true); rain.clip = routing.rain; rain.loop = true; rain.volume = .25f; rain.spatialBlend = 0; rain.Play();
            rainFilter = rain.gameObject.AddComponent<AudioLowPassFilter>();
            room = Source("ConcreteRoomTone", true); room.clip = routing.roomTone; room.loop = true; room.volume = .08f; room.spatialBlend = 0; room.Play();
            cry = Source("KuntilanakInverseCry", false); cry.loop = true; cry.spatialBlend = .65f; cry.rolloffMode = AudioRolloffMode.Custom;
            cry.SetCustomCurve(AudioSourceCurveType.CustomRolloff, AnimationCurve.Linear(0, 1, 1, 1));
            nextDrip = Time.time + 4;
        }
        private AudioSource Source(string id, bool ambient)
        {
            var go = new GameObject(id, typeof(AudioSource)); go.transform.SetParent(transform, false);
            var source = go.GetComponent<AudioSource>(); source.playOnAwake = false; source.spatialBlend = 1; source.dopplerLevel = 0;
            source.minDistance = 2; source.maxDistance = 28; source.rolloffMode = AudioRolloffMode.Linear;
            source.outputAudioMixerGroup = ambient ? routing.ambienceBus : routing.effectsBus;
            return source;
        }
        public void Play(string id, Vector3 position)
        {
            if (pool == null) return;
            var sound = Array.Find(sounds, s => s.id == id);
            if (sound == null || !sound.clip) return;
            if (id == "Kunti") { cry.clip = sound.clip; cry.transform.position = position; cry.volume = sound.gain; cry.Play(); return; }
            var source = pool[cursor++ % pool.Length]; source.transform.position = position;
            source.clip = sound.clip; source.volume = sound.gain; source.pitch = id == "Foot" ? UnityEngine.Random.Range(.92f, 1.05f) : 1; source.Play();
        }
        public void SetKuntiDistance(float distance)
        {
            if (!cry) return;
            cry.transform.position = director.world.kuntilanak.transform.position + Vector3.up * 1.5f;
            // The intentional inverse-distance clue has its own bounded curve.
            cry.volume = Mathf.Lerp(.07f, .46f, Mathf.InverseLerp(2, 30, distance));
        }
        public void StopKunti() { if (cry) cry.Stop(); }
        public void SetThreat(Vector3 position, bool hunting, bool stunned) { targetRoom = hunting && !stunned ? .14f : .08f; }
        public void VictimEcho() { Play("Echo", director.world.Anchor("SIGNBOARD").position); }
        private void Update()
        {
            if (!director.Ready || !rain || UiFocus.Menu) return;
            Vector3 p = director.player.transform.position;
            bool open = Mathf.Abs(p.x) > 25 || Mathf.Abs(p.z) > 21;
            rain.volume = Mathf.Lerp(rain.volume, open ? .28f : .11f, Time.deltaTime * 2);
            rainFilter.cutoffFrequency = Mathf.Lerp(rainFilter.cutoffFrequency, open ? 16000 : 2600, Time.deltaTime * 2);
            room.volume = Mathf.Lerp(room.volume, targetRoom, Time.deltaTime);
            if (Time.time > nextDrip && !director.Busy)
            { nextDrip = Time.time + UnityEngine.Random.Range(5, 11); Play("Drip", p + new Vector3(3, 2, 5)); }
        }
    }
}
