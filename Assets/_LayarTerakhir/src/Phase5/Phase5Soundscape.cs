using System;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    [Serializable] public sealed class Phase5Sound { public string id; public AudioClip clip; }
    public sealed class Phase5Soundscape : MonoBehaviour
    {
        public Phase5Director director;
        public SceneAudioProfile routing;
        public Phase5Sound[] sounds;
        private AudioSource ambience, score, cold, whisper, ending;
        private readonly AudioSource[] cues = new AudioSource[8];
        private int next;
        private float duck, nextMorse;
        private bool paused;
        private AudioClip Clip(string id) { foreach (var s in sounds) if (s.id == id) return s.clip; return null; }
        private AudioSource Source(string name, bool spatial)
        {
            var s = new GameObject(name).AddComponent<AudioSource>(); s.transform.SetParent(transform, false);
            s.playOnAwake = false; s.spatialBlend = spatial ? 1 : 0; s.minDistance = 1.5f; s.maxDistance = 24; s.dopplerLevel = 0; s.priority = spatial ? 40 : 150;
            if (routing) s.outputAudioMixerGroup = spatial ? routing.effectsBus : routing.ambienceBus;
            return s;
        }
        private void Start()
        {
            ambience = Source("PasarSetan_Air", false); score = Source("PasarSetan_Drone", false); cold = Source("Frozen_Hum", true);
            whisper = Source("PilarLayar_Whispers", true); ending = Source("Final_Epilogue", false);
            Loop(ambience, "Ambient", .08f); Loop(score, "Score", .05f); Loop(cold, "Cold", .18f); Loop(whisper, "Crowd", .09f);
            cold.transform.position = director.world.Anchor("S1").position + Vector3.forward * 15;
            whisper.transform.position = director.world.crowd.position; whisper.maxDistance = 10;
            for (int i = 0; i < cues.Length; i++) cues[i] = Source("PasarSetan_Cue_" + i, true);
        }
        private void Loop(AudioSource s, string id, float volume) { s.clip = Clip(id); s.loop = true; s.volume = volume; if (s.clip) s.Play(); }
        private void Update()
        {
            if (!ambience || director.Campaign && director.Campaign.Data.ending != EndingId.NONE) return;
            bool pause = !director.ClockRunning; if (pause == paused) return; paused = pause;
            foreach (var source in GetComponentsInChildren<AudioSource>()) { if (pause) source.Pause(); else source.UnPause(); }
        }
        public void Tick(float dt)
        {
            if (!ambience) return;
            duck = Mathf.Max(0, duck - dt); float lower = duck > 0 || director.RecordingEVP ? .28f : 1;
            ambience.volume = Mathf.MoveTowards(ambience.volume, .09f * lower, dt * .2f);
            score.volume = Mathf.MoveTowards(score.volume, (director.FinalChoice ? .012f : director.boss.Active ? .035f : .055f) * lower, dt * .1f);
            whisper.volume = director.RecordingEVP ? .23f : .07f;
            if (director.Zone == "S2" && director.Elapsed >= nextMorse)
            {
                nextMorse = director.Elapsed + 9;
                foreach (var source in director.world.sources)
                    if (source.sourceId == "P5_BRIDGE_TRUE") { Cue("Knock", source.transform.position); break; }
            }
        }
        public void Cue(string id, Vector3 at)
        {
            var clip = Clip(id); if (!clip || !cues[0]) return;
            var source = cues[next++ % cues.Length]; source.transform.position = at; source.volume = id == "Incense" ? .18f : .48f; source.clip = clip; source.pitch = 1; source.Play(); duck = Mathf.Min(4, clip.length);
        }
        public void Ending(EndingId id)
        {
            if (!ending) return;
            foreach (var source in new[] { ambience, score, cold, whisper }) source.Stop();
            ending.clip = Clip(id == EndingId.BAD ? "EndingBad" : id == EndingId.NORMAL ? "EndingNormal" : id == EndingId.TRUE ? "EndingTrue" : "EndingSecret");
            ending.volume = .16f; ending.loop = true; if (ending.clip) ending.Play();
        }
    }
}
