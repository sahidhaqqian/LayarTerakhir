using System;
using LayarTerakhir.Audio;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    [Serializable] public sealed class Phase4Sound { public string id; public AudioClip clip; }
    public sealed class Phase4Soundscape : MonoBehaviour
    {
        public Phase4Director director;
        public SceneAudioProfile routing;
        public Phase4Sound[] sounds;
        private AudioSource forest, fire, spirit, heartbeat, score, market;
        private readonly AudioSource[] cues = new AudioSource[8];
        private int next;
        private float sanity = 100, duck, nextKnock;
        private bool paused;
        private void Start()
        {
            forest = Source("Bamboo_Wind", false); fire = Source("Banaspati_Fire", true);
            spirit = Source("Well_EVP", true); heartbeat = Source("Fear_Heartbeat", false); score = Source("KarangSetra_Underscore", false);
            market=Source("PasarSetan_BeyondGate",true);market.transform.position=director.world.Anchor("BOUNDARY").position;
            Loop(forest, "Ambient"); Loop(fire, "Fire"); Loop(spirit, "Spirit"); Loop(heartbeat, "Heartbeat"); Loop(score, "Score");
            Loop(market,"Market");
            spirit.transform.position = director.world.wellMantra.position; spirit.maxDistance = 9;
            for (int i = 0; i < cues.Length; i++) cues[i] = Source("KarangSetra_Cue_" + i, true);
        }
        private AudioSource Source(string name, bool spatial)
        {
            var go = new GameObject(name); go.transform.SetParent(transform, false); var s = go.AddComponent<AudioSource>();
            s.playOnAwake = false; s.spatialBlend = spatial ? 1 : 0; s.minDistance = 1.5f; s.maxDistance = 24;
            s.dopplerLevel = 0; s.priority = spatial ? 48 : 150;
            if (routing) s.outputAudioMixerGroup = spatial ? routing.effectsBus : routing.ambienceBus;
            return s;
        }
        private AudioClip Clip(string id) { foreach (var s in sounds) if (s.id == id) return s.clip; return null; }
        private void Loop(AudioSource s, string id) { s.clip = Clip(id); s.loop = true; s.volume = 0; if (s.clip) s.Play(); }
        public void SetSanity(float value, bool clarity) => sanity = clarity ? 100 : value;
        private void Update()
        {
            if (!forest) return;
            bool pause = !director.ClockRunning;
            if (pause == paused) return;
            paused = pause;
            foreach (var s in GetComponentsInChildren<AudioSource>()) { if (pause) s.Pause(); else s.UnPause(); }
        }
        public void Tick(float dt)
        {
            if (!forest) return;
            duck = Mathf.Max(0, duck - dt);
            bool won = director.Completed("P4_BOSS_WIN");
            float reduced = duck > 0 || director.RecordingEVP ? .3f : 1;
            forest.volume = Mathf.MoveTowards(forest.volume, .12f * reduced, dt * .3f);
            fire.transform.position = director.world.weakPoint.position;
            fire.volume = director.world.fireVisual.activeInHierarchy ? .48f * reduced : 0;
            spirit.volume = won ? .025f : director.RecordingEVP ? .44f : .11f;
            heartbeat.volume = Mathf.MoveTowards(heartbeat.volume, sanity < 40 && !won ? Mathf.Lerp(.26f, .09f, sanity / 40) : 0, dt * .2f);
            score.volume = Mathf.MoveTowards(score.volume, director.Completed("P4_BANASPATI_REVEAL")?0:.045f * reduced, dt * .1f);
            market.volume=Mathf.MoveTowards(market.volume,director.Completed("P4_FIFTH_GATE_REVEAL")?.38f:0,dt*.2f);
            if (director.Elapsed > nextKnock && Vector3.Distance(director.player.transform.position, director.world.secretKnock.position) < 9)
            { nextKnock = director.Elapsed + 9; Cue("Knock", director.world.secretKnock.position); }
        }
        public void Cue(string id, Vector3 at)
        {
            var clip = Clip(id); if (!clip || !cues[0]) return;
            var s = cues[next++ % cues.Length]; s.transform.position = at; s.volume = id=="Whisper"?.16f:id=="Incense"?.18f:.55f; s.pitch = 1; s.clip = clip; s.Play(); duck = Mathf.Min(4, clip.length);
        }
    }
}
