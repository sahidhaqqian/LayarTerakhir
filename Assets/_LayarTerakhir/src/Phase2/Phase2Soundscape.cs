using System;
using LayarTerakhir.Audio;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase2
{
    [Serializable] public sealed class Phase2Sound { public string id; public AudioClip clip; }
    public sealed class Phase2Soundscape : MonoBehaviour
    {
        public Phase2Director director;
        public SceneAudioProfile routing;
        public Phase2Sound[] sounds;
        private AudioSource exterior, refrigerator;
        private AudioSource[] incubatorCries;
        private readonly AudioSource[] telegraphs = new AudioSource[5];
        private int next;
        private float lastThermo;
        private void Start()
        {
            exterior=Source("RSUD_Exterior",false);refrigerator=Source("RSUD_Refrigerator",false);
            for(int i=0;i<telegraphs.Length;i++)telegraphs[i]=Source("RSUD_Telegraph_"+i,true);
            Loop(exterior,"Exterior");Loop(refrigerator,"Refrigerator");
            incubatorCries=new AudioSource[director.world.incubatorSources.Length];
            for(int i=0;i<incubatorCries.Length;i++)
            {
                var cry=Source("RSUD_IncubatorCry_"+(i+1),true);incubatorCries[i]=cry;cry.priority=144;cry.maxDistance=9;
                cry.transform.position=director.world.incubatorSources[i].position;cry.pitch=.96f+i*.014f;
                if(routing)cry.outputAudioMixerGroup=routing.ambienceBus;
                Loop(cry,"Baby");if(cry.clip)cry.time=cry.clip.length*i/6;
            }
        }
        private AudioSource Source(string name,bool spatial)
        {
            var go=new GameObject(name);go.transform.SetParent(transform,false);var source=go.AddComponent<AudioSource>();
            source.playOnAwake=false;source.spatialBlend=spatial?.85f:0;source.minDistance=1.2f;source.maxDistance=18;
            source.rolloffMode=AudioRolloffMode.Logarithmic;source.dopplerLevel=0;source.priority=spatial?32:160;
            if(routing)source.outputAudioMixerGroup=spatial?routing.effectsBus:routing.ambienceBus;
            return source;
        }
        private AudioClip Clip(string id)
        {foreach(var sound in sounds)if(sound.id==id)return sound.clip;return null;}
        private void Loop(AudioSource source,string id)
        {source.clip=Clip(id);source.loop=true;source.volume=0;if(source.clip)source.Play();}
        public void Tick(float dt)
        {
            bool morgue=director.CurrentRoom=="MORGUE"||director.CurrentRoom=="ARCHIVE"||director.CurrentRoom=="DUTY";
            float outdoors=director.Completed("P2_BOSS_WIN")&&!morgue?.42f:morgue?.002f:director.IsQuiet?.045f:director.CurrentRoom=="L2"?.08f:.22f;
            if(exterior)exterior.volume=Mathf.MoveTowards(exterior.volume,outdoors,dt*.8f);
            if(incubatorCries!=null)foreach(var cry in incubatorCries)
                if(cry)cry.volume=Mathf.MoveTowards(cry.volume,director.CurrentRoom=="INKUBATOR"?.11f:0,dt*.2f);
            if(refrigerator)refrigerator.volume=Mathf.MoveTowards(refrigerator.volume,morgue?.27f:.025f,dt*.8f);
            // These sources never pass through the exterior reduction.
            if(director.ThermoActive&&director.CameraCanScan&&director.Temperature<0&&Time.time-lastThermo>.8f)
            {lastThermo=Time.time;Cue("Thermo",director.player.transform.position);}
        }
        public void Resolve(){if(exterior)exterior.volume=.42f;if(refrigerator)refrigerator.volume=.015f;}
        public void Cue(string id,Vector3 position)
        {
            var clip=Clip(id);if(!clip)return;
            var source=telegraphs[next++%telegraphs.Length];if(!source)return;
            source.transform.position=position;source.volume=id=="Moan"?.72f:.48f;source.PlayOneShot(clip);
        }
    }
}
