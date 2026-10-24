using System;
using System.Collections.Generic;
using LayarTerakhir.Audio;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    [Serializable] public sealed class Phase3Sound { public string id; public AudioClip clip; }
    public sealed class Phase3Soundscape : MonoBehaviour
    {
        public Phase3Director director;
        public SceneAudioProfile routing;
        public Phase3Sound[] sounds;
        private AudioSource ambience,machinery,air;
        private readonly AudioSource[] cues=new AudioSource[6];
        private int next;
        private float duckUntil;
        private sealed class Voice {public AudioSource audio;public Phase3VoiceTag tag=Phase3VoiceTag.Ambient;public int pulse=-1;}
        private readonly Dictionary<Phase3EvpSource,Voice> voices=new Dictionary<Phase3EvpSource,Voice>();
        private void Start()
        {
            ambience=Source("Factory_Air",false);machinery=Source("Boiler_Machinery",false);air=Source("Factory_SpiritLayer",false);
            Loop(ambience,"Ambient");Loop(machinery,"Machinery");Loop(air,"Spirit");
            for(int i=0;i<cues.Length;i++)cues[i]=Source("Factory_Telegraph_"+i,true);
        }
        private AudioSource Source(string name,bool spatial)
        {
            var go=new GameObject(name);go.transform.SetParent(transform,false);var s=go.AddComponent<AudioSource>();
            s.playOnAwake=false;s.spatialBlend=spatial?1:0;s.minDistance=1.5f;s.maxDistance=25;s.dopplerLevel=0;s.priority=spatial?24:160;
            s.rolloffMode=AudioRolloffMode.Logarithmic;if(routing)s.outputAudioMixerGroup=spatial?routing.effectsBus:routing.ambienceBus;return s;
        }
        private AudioClip Clip(string id){foreach(var s in sounds)if(s.id==id)return s.clip;return null;}
        private void Loop(AudioSource source,string id){source.clip=Clip(id);source.loop=true;source.volume=0;if(source.clip)source.Play();}
        public void Tick(float dt)
        {
            if(!ambience)return;
            bool won=director.Completed("P3_BOSS_WIN");float duck=Time.time<duckUntil?.2f:1;
            ambience.volume=Mathf.MoveTowards(ambience.volume,(director.CurrentRoom=="YARD"?.2f:.09f)*duck,dt*.5f);
            machinery.volume=Mathf.MoveTowards(machinery.volume,(won?.015f:.19f)*duck,dt*.5f);
            air.volume=Mathf.MoveTowards(air.volume,(won?0:director.wewe.Pursuing?.13f:.035f)*duck,dt*.5f);
            foreach(var emitter in director.world.sources)
            {
                if(!emitter)continue;
                if(!voices.TryGetValue(emitter,out var voice))
                {voice=new Voice{audio=Source("EVP_"+emitter.sourceId,true)};voice.audio.transform.position=emitter.transform.position;voice.audio.maxDistance=emitter.range+4;voices.Add(emitter,voice);}
                var source=voice.audio;source.transform.position=emitter.transform.position;
                bool audible=!won&&emitter.gameObject.activeInHierarchy&&Vector3.Distance(director.player.transform.position,emitter.transform.position)<=source.maxDistance;
                if(!audible){if(source.isPlaying)source.Stop();voice.tag=Phase3VoiceTag.Ambient;continue;}
                if(emitter.sourceId=="VENT09")
                {
                    int pulse=Mathf.FloorToInt(director.Elapsed*2);
                    if(pulse!=voice.pulse&&pulse%12<3){source.clip=Clip("Knock");source.volume=.65f;source.Play();}
                    voice.pulse=pulse;continue;
                }
                var tag=emitter.Tag(director.Elapsed,director.balance);
                if(voice.tag!=tag||!source.isPlaying)
                {
                    source.clip=Clip(tag==Phase3VoiceTag.Authentic?"AuthenticVoice":emitter.intercom?"Humming":"Mimic");
                    source.loop=true;source.volume=emitter.intercom?.5f:.25f;if(source.clip)source.Play();voice.tag=tag;
                }
                if(director.RecordingEVP&&director.SelectedSource==emitter)duckUntil=Time.time+.3f;
            }
        }
        public void Cue(string id,Vector3 at)
        {
            var clip=Clip(id);if(!clip||cues[0]==null)return;
            var s=cues[next++%cues.Length];s.transform.position=at;s.pitch=1;s.volume=.6f;s.clip=clip;s.Play();duckUntil=Time.time+Mathf.Min(clip.length,4);
        }
        public void Footstep(Vector3 at)
        {
            var clip=Clip("Step");if(!clip||cues[0]==null)return;
            var s=cues[next++%cues.Length];s.transform.position=at;s.pitch=.7f;s.volume=.42f;s.clip=clip;s.Play();
        }
        public void Resolve(){duckUntil=Time.time+4;}
    }
}
