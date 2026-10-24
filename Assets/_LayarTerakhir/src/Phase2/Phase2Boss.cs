using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;

namespace LayarTerakhir.Phase2
{
    public enum Phase2BossState { Inactive, Prep, Track, Exposed, Recovery, PowerRecovery, VictoryPending, Resolved }
    // One owner of the existing Lastri actor/agent. Investigation yields ownership during the ritual.
    public sealed class Phase2Boss : MonoBehaviour
    {
        public Phase2Director director;
        public Phase2BossState State { get; private set; }
        public int Layers { get; private set; }
        public bool Active => State != Phase2BossState.Inactive && State != Phase2BossState.Resolved;
        public bool Recovering => State == Phase2BossState.Prep || State == Phase2BossState.Recovery || State == Phase2BossState.PowerRecovery;
        public bool Exposed => State == Phase2BossState.Exposed;
        public bool CanCharge => Recovering || State == Phase2BossState.Resolved;
        public float Remaining => Mathf.Max(0,timer);
        public float TouchMeter { get; private set; }
        public bool[] Spent { get; } = new bool[3];
        public float[] Cooldowns { get; } = new float[3];
        public readonly List<string> UsedFlares = new List<string>();
        private Phase2Balance B => director.balance;
        private Phase2World W => director.world;
        private NavMeshAgent Agent => director.lastri.agent;
        private readonly List<Vector3> trail = new List<Vector3>(256);
        private readonly List<float> times = new List<float>(256);
        private readonly float[] bursts = new float[3], illusionTimers = new float[2];
        private float timer, elapsed, idle, hair, turn, repath, activeFlareTime, flareDwell, contactGrace, shardDelay;
        private int source = -1, pendingShards;
        private Vector3 previous, flarePosition;
        private GameObject flare;
        private bool armed, warning, secondWarning;
        private float intro;
        public string Status => State switch
        {
            Phase2BossState.Exposed => $"PERISAI HANCUR • {Remaining:0.0}s\nBIDIK KEPALA • BLITZ {Layers}/3",
            Phase2BossState.Track => $"SEGEL {Layers}/3 • PANTAU SELFIE\n{(TouchMeter>=100?"RAMBUT MENYENTUH BAHU!":$"SENTUHAN {TouchMeter:0}%")}",
            Phase2BossState.VictoryPending => "SEGEL SIAP • PENYIMPANAN GAGAL\nE DI MEJA: COBA SIMPAN KEMENANGAN",
            _ => $"{(State==Phase2BossState.Prep?"PERSIAPAN RITUAL":"JEDA PEMULIHAN")} • {Remaining:0}s\nGENERATOR: TAHAN E • MEJA: LANJUT SADAR"
        };
        public void Restore()
        {
            Abort();director.phoneView.ShowChapterTarget(Vector3.zero,false); Agent.baseOffset=0;intro=0; Layers=0; UsedFlares.Clear(); elapsed=0;
            for(int i=0;i<3;i++){Spent[i]=false;Cooldowns[i]=bursts[i]=0;W.ritualLights[i].enabled=false;}
            if(flare)Destroy(flare);flare=null;activeFlareTime=0;
            SetActor(false,false);SetIllusions(false);W.bossMist.SetActive(false);
            State=director.Completed("P2_BOSS_WIN")?Phase2BossState.Resolved:Phase2BossState.Inactive;
            W.arenaLock.SetActive(false);W.tableRelics.SetActive(State==Phase2BossState.Resolved);W.burntCloth.SetActive(State==Phase2BossState.Resolved);
            for(int i=0;i<W.breakableGlass.Length;i++)W.breakableGlass[i].SetActive(State!=Phase2BossState.Resolved);
            foreach(var light in W.arenaAmbient)if(light)light.enabled=true;
        }
        public bool Begin()
        {
            if(Active||State==Phase2BossState.Resolved||!director.NearTable())return false;
            if(!Phase2Campaign.PrepareRitual(director.Campaign))
            { HudController.Instance.ShowBroadcastNotification("Ritual belum dimulai. Lengkapi Jurnal dan tiga pusaka; pastikan checkpoint dapat disimpan.",5);return false; }
            if(!director.Campaign.BeginCheckpointAttempt(this))return false;
            director.lastri.SuspendClock();Layers=0;UsedFlares.Clear();W.tableRelics.SetActive(true);W.arenaLock.SetActive(true);W.operationDoor.SetOpenAnimated(false);intro=3;
            foreach(var light in W.arenaAmbient)if(light)light.enabled=false;
            Agent.Warp(W.bossHome.position);SetActor(true,false);Motion("Windup");
            EnterRecovery(Phase2BossState.Prep,B.bossPrepSeconds);
            director.soundscape.Cue("Moan",W.bossHome.position);
            HudController.Instance.EnqueueSubtitle("Raka","Gunting... gelang... darah segel. Aku kembalikan semuanya di sini.",7);
            return true;
        }
        public bool ContinueCycle()
        {
            if(!director.NearTable())return false;
            if(State==Phase2BossState.VictoryPending)return CommitVictory();
            if(!Recovering||timer>0||director.Phone.batteryPercent<director.Phone.blitzCostPercent||director.Phone.BlitzCooldownRemaining>0)
            { HudController.Instance.ShowBroadcastNotification("Isi daya di generator. Tunggu jeda selesai, lalu tekan E lagi di meja untuk melanjutkan.",4);return false; }
            State=Phase2BossState.Track;timer=0;idle=hair=turn=TouchMeter=0;armed=warning=secondWarning=false;
            contactGrace=B.telegraphSeconds+B.bossTurnGrace;previous=director.player.transform.position;
            trail.Clear();times.Clear();trail.Add(previous);times.Add(elapsed);source=-1;
            SetActor(true,false);W.bossMist.SetActive(true);SetIllusions(Layers>0);
            director.soundscape.Cue("Moan",W.lastri.transform.position);Motion("Walk");
            HudController.Instance.ShowBroadcastNotification("PANTAU SELFIE • tetap bergerak. Pancing Lastri ke sorotan, tekan sakelar, lalu Blitz ketika perisainya pecah.",6);
            return true;
        }
        public void Tick(float dt)
        {
            if(!Active)return;
            elapsed+=dt;timer-=dt;contactGrace=Mathf.Max(0,contactGrace-dt);
            for(int i=0;i<3;i++)
            {Cooldowns[i]=Mathf.Max(0,Cooldowns[i]-dt);bursts[i]=Mathf.Max(0,bursts[i]-dt);W.ritualLights[i].enabled=bursts[i]>0;W.ritualLights[i].intensity=GameShell.ReducedGlitch?7:18;}
            if(activeFlareTime>0){activeFlareTime-=dt;if(activeFlareTime<=0&&flare)Destroy(flare);}
            if(Recovering){if(intro>0){intro=Mathf.Max(0,intro-dt);Agent.baseOffset=intro*1.2f/3;SetActor(true,intro>0);Motion("Windup");}else{Agent.baseOffset=0;Retreat(dt);}Present();return;}
            if(State==Phase2BossState.VictoryPending){Stop();Present();return;}
            if(Exposed)
            {
                Stop();if(timer<=0){SetActor(true,false);W.bossMist.SetActive(true);EnterRecovery(Phase2BossState.Recovery,B.bossRecoverySeconds);}
                Present();return;
            }
            // Energy recovery happens before any fatal resolution, even when a miss consumed the last charge.
            if(director.Phone.batteryPercent<director.Phone.blitzCostPercent)
            {EnterRecovery(Phase2BossState.PowerRecovery,B.bossRecoverySeconds);return;}
            Vector3 player=director.player.transform.position,delta=player-previous;delta.y=0;previous=player;
            if(trail.Count==0||Vector3.Distance(trail[trail.Count-1],player)>.12f)
            {trail.Add(player);times.Add(elapsed);if(trail.Count>256){trail.RemoveAt(0);times.RemoveAt(0);}}
            Agent.baseOffset=.18f+Mathf.Sin(elapsed*2.3f)*.02f;
            Motion("Idle");Follow(TrailTarget(),dt);TickIllusions(dt);
            for(int i=0;i<3;i++)if(bursts[i]>0&&!Spent[i]&&InsideBeam(W.lastri.transform.position,i))
            {Expose(i);return;}
            if(activeFlareTime>0&&Vector3.Distance(W.lastri.transform.position,flarePosition)<=B.flareRadius)
            {flareDwell+=dt;if(flareDwell>=B.flareDwell){Expose(-1);return;}}else flareDwell=0;
            Vector3 ghost=W.lastri.transform.position-player;ghost.y=0;
            if(!armed&&contactGrace<=0&&ghost.magnitude<5&&Vector3.Dot(director.player.transform.forward,ghost.normalized)<0)armed=true;
            if(!armed){Present();return;}
            bool watched=director.CameraCanScan&&director.Phone.isSelfieCamera&&director.lastri.FramesLastri();
            TouchMeter=Mathf.Clamp(TouchMeter+(watched?-B.touchFall:B.bossTouchRise)*dt,0,100);
            idle=delta.magnitude/Mathf.Max(dt,.001f)<.15f?idle+dt:0;
            if(idle>B.bossIdleSeconds&&!warning){warning=true;director.soundscape.Cue("Breath",player);HudController.Instance.ShowBroadcastNotification("LASTRI MENERJANG — BERGERAK SEKARANG!",B.bossLungeSeconds);}
            if(idle<=B.bossIdleSeconds)warning=false;
            if(idle>=B.bossIdleSeconds+B.bossLungeSeconds){director.Die("GRAB — tetap bergerak memutari meja selama melacak Lastri.");return;}
            turn=Vector3.Angle(director.player.transform.forward,ghost)<B.turnAngle?turn+dt:0;
            if(turn>=B.turnHold){director.Die("POUNCE — gunakan Selfie. Menoleh aman hanya saat PERISAI HANCUR.");return;}
            hair=TouchMeter>=100?hair+dt:0;
            if(TouchMeter>=100&&!secondWarning){secondWarning=true;director.soundscape.Cue("Breath",player);}
            if(TouchMeter<100)secondWarning=false;
            if(hair>=B.hairSeconds){director.Die("GRAB — Lastri terlalu lama keluar dari pantauan Selfie.");return;}
            Present();
        }
        private Vector3 TrailTarget()
        {
            float remaining=B.bossTrailDistance;int end=trail.Count-1;
            while(end>0&&times[end]>elapsed-B.bossTrailDelay)end--;
            for(int i=end;i>0;i--)
            {float step=Vector3.Distance(trail[i],trail[i-1]);if(step>=remaining)return Vector3.Lerp(trail[i],trail[i-1],remaining/Mathf.Max(.001f,step))-director.player.transform.right*.58f;remaining-=step;}
            return trail[0]-director.player.transform.forward*remaining-director.player.transform.right*.58f;
        }
        private void Follow(Vector3 target,float dt)
        {
            if(!Agent.isOnNavMesh)return;
            repath-=dt;if(repath<=0&&NavMesh.SamplePosition(target,out var hit,.75f,NavMesh.AllAreas))
            {repath=.15f;Agent.isStopped=false;Agent.speed=B.bossStalkSpeed;Agent.SetDestination(hit.position);}
            Vector3 look=director.player.transform.position-W.lastri.transform.position;look.y=0;
            if(look.sqrMagnitude>.01f)W.lastri.transform.rotation=Quaternion.Slerp(W.lastri.transform.rotation,Quaternion.LookRotation(look),dt*5);
        }
        private void Retreat(float dt)
        {
            Follow(W.bossRetreat.position,dt);
            if(Vector3.Distance(W.lastri.transform.position,W.bossRetreat.position)<.4f){Stop();Motion("Idle");}
        }
        public bool InsideBeam(Vector3 feet,int index)
        {
            Vector3 centre=W.beamCentres[index].position;Vector3 delta=feet-centre;delta.y=0;
            return delta.magnitude<=W.beamRadii[index]&&director.ClearLine(centre+Vector3.up*2.4f,feet+Vector3.up*1.4f);
        }
        public bool UseLamp(int index)
        {
            if(State!=Phase2BossState.Track||index<0||index>=3||Spent[index]||Cooldowns[index]>0)return false;
            Cooldowns[index]=B.lampCooldownSeconds;bursts[index]=B.lampBurstSeconds;W.ritualLights[index].enabled=true;
            director.soundscape.Cue("Latch",W.beamCentres[index].position);
            for(int i=0;i<2;i++)if(W.illusions[i].activeSelf&&InsideBeam(W.illusions[i].transform.position,index))PopIllusion(i);
            if(InsideBeam(W.lastri.transform.position,index))Expose(index);
            return true;
        }
        public bool DropFlare()
        {
            if(State!=Phase2BossState.Track)return false;
            string id=director.Campaign.Data.inventory.FirstOrDefault(x=>x.StartsWith("P2_FLARE_")&&!UsedFlares.Contains(x));
            if(id==null){HudController.Instance.ShowBroadcastNotification("Flare habis. Tiga lampu arena tetap dapat digunakan.",3);return false;}
            if(flare)Destroy(flare);UsedFlares.Add(id);flarePosition=director.player.transform.position;activeFlareTime=B.flareSeconds;flareDwell=0;
            flare=Instantiate(W.flareTemplate,flarePosition,Quaternion.identity);flare.SetActive(true);return true;
        }
        private void Expose(int lamp)
        {
            if(State!=Phase2BossState.Track)return;
            State=Phase2BossState.Exposed;timer=B.bossExposedSeconds;source=lamp;Stop();SetActor(true,true);W.bossMist.SetActive(false);SetIllusions(false);Motion("Stun");
            director.soundscape.Cue("Scream",W.lastri.transform.position);
            HudController.Instance.ShowBroadcastNotification("PERISAI HANCUR — bidik kepala. Blitz Selfie atau balik badan dengan kamera belakang!",B.bossExposedSeconds);Present();
        }
        public bool Capture()
        {
            if(!Active)return false;
            bool valid=Exposed&&director.Aim(director.lastri.Head,B.bossAimRange,B.bossAimAngle)&&
                (!director.Phone.isSelfieCamera||director.lastri.FramesLastri());
            if(!director.Phone.TryCaptureEnergy(true))
            {HudController.Instance.ShowBroadcastNotification("Blitz perlu daya 15% dan cooldown 6 detik. Generator tersedia pada jeda.",3);return true;}
            director.phoneView.CaptureFlash();
            if(!valid){HudController.Instance.ShowBroadcastNotification("Blitz meleset atau perisai masih utuh. Lampu tidak rusak.",3);return true;}
            Layers++;if(source>=0){Spent[source]=true;bursts[source]=0;W.ritualLights[source].enabled=false;}
            SetActor(true,false);
            if(Layers==3){State=Phase2BossState.VictoryPending;SetActor(false,false);W.bossMist.SetActive(false);CommitVictory();}
            else{pendingShards=2;shardDelay=2;director.soundscape.Cue("Hiss",W.lastri.transform.position);EnterRecovery(Phase2BossState.Recovery,B.bossRecoverySeconds);}
            return true;
        }
        public bool CommitVictory()
        {
            if(State!=Phase2BossState.VictoryPending)return false;
            if(!Phase2Campaign.WinRitual(director.Campaign,this,Layers,UsedFlares))
            {HudController.Instance.ShowBroadcastNotification("Segel belum tersimpan. E di meja untuk coba lagi; Continue tetap memuat CP5 utuh.",6);return false;}
            director.Campaign.EndCheckpointAttempt(this);director.phoneView.ShowChapterTarget(Vector3.zero,false);State=Phase2BossState.Resolved;W.arenaLock.SetActive(false);W.burntCloth.SetActive(true);
            foreach(var light in W.arenaAmbient)if(light)light.enabled=true;
            SetActor(false,false);SetIllusions(false);W.bossMist.SetActive(false);Stop();
            director.soundscape.Cue("Seal",W.ritualTable.position);director.soundscape.Resolve();StartCoroutine(AbsorbAsh());director.RefreshAfterRitual();return true;
        }
        private void EnterRecovery(Phase2BossState state,float seconds)
        {
            State=state;timer=seconds;armed=false;TouchMeter=idle=turn=hair=0;SetActor(true,false);SetIllusions(false);W.bossMist.SetActive(false);Motion("Walk");
            for(int i=0;i<3;i++)bursts[i]=0;
        }
        private void TickIllusions(float dt)
        {
            for(int i=0;i<2&&Layers>0;i++)
            {
                illusionTimers[i]-=dt;var illusion=W.illusions[i];
                if(!illusion.activeSelf&&illusionTimers[i]<=0)illusion.SetActive(true);
                if(!illusion.activeSelf)continue;
                Vector3 p=director.player.transform.position-director.player.transform.forward*(i==0?3.6f:4.5f)+director.player.transform.right*(i==0?-1.2f:1.5f);
                if(NavMesh.SamplePosition(p,out var hit,.8f,NavMesh.AllAreas))illusion.transform.position=Vector3.MoveTowards(illusion.transform.position,hit.position,dt*2.3f);
                Vector3 look=director.player.transform.position-illusion.transform.position;look.y=0;if(look.sqrMagnitude>.01f)illusion.transform.rotation=Quaternion.LookRotation(look);
                if(Vector3.Distance(illusion.transform.position,director.player.transform.position)<=B.illusionPopRange)PopIllusion(i);
            }
        }
        private void PopIllusion(int i){W.illusions[i].SetActive(false);illusionTimers[i]=B.illusionRespawnSeconds;director.soundscape.Cue("Scream",W.illusions[i].transform.position);}
        private void SetIllusions(bool active)
        {for(int i=0;i<2;i++){W.illusions[i].SetActive(active);illusionTimers[i]=0;if(active)W.illusions[i].transform.position=W.bossHome.position+Vector3.forward*2+Vector3.right*(i==0?-1.2f:1.2f);}}
        public int Temperature()
        {
            if(!Active||!director.CameraCanScan||!director.Phone.isSelfieCamera)return 24;
            float best=B.thermalAngle;int temperature=24;
            for(int i=-1;i<2;i++)
            {
                var actor=i<0?W.lastri:W.illusions[i];if(!actor.activeInHierarchy||(i<0&&State==Phase2BossState.VictoryPending))continue;
                Vector3 head=actor.transform.position+Vector3.up*1.5f,delta=head-director.phoneView.FeedCamera.transform.position;
                float angle=Vector3.Angle(director.phoneView.FeedCamera.transform.forward,delta);
                if(angle<=best&&director.Aim(head,B.bossAimRange,B.thermalAngle)){best=angle;temperature=i<0?-20:-6;}
            }
            return temperature;
        }
        private void Present()
        {
            if(shardDelay>0){shardDelay-=Time.deltaTime;if(shardDelay<=0&&pendingShards>0)
            {for(int i=(Layers-1)*2;i<Mathf.Min(Layers*2,W.breakableGlass.Length);i++){W.breakableGlass[i].SetActive(false);W.glassBursts[i].Play();}pendingShards=0;director.soundscape.Cue("Glass",W.ritualTable.position);}}
            director.phoneView.ShowChapterTarget(director.lastri.Head,Exposed,director.Aim(director.lastri.Head,B.bossAimRange,B.bossAimAngle));
            if(director.Phone.isPhoneRaised&&director.Phone.isScreenOn)
                director.phoneView.ShowChapterReadout((director.ThermoActive?$"THERMOKLENIK {director.Temperature}°C\n":"")+Status,Exposed?new Color(1,.3f,.22f):Color.white);
        }
        private System.Collections.IEnumerator AbsorbAsh()
        {
            var ash=Instantiate(W.bossMist);ash.name="Lastri_Seal_Ash";ash.transform.SetParent(null,true);ash.transform.position=W.lastri.transform.position+Vector3.up;
            foreach(var t in ash.GetComponentsInChildren<Transform>())t.gameObject.layer=0;
            var particles=ash.GetComponent<ParticleSystem>();var main=particles.main;main.startColor=new Color(.75f,.78f,.74f,.4f);main.simulationSpace=ParticleSystemSimulationSpace.Local;main.startLifetime=1.2f;
            ash.SetActive(true);particles.Play();float time=0;Vector3 start=ash.transform.position;
            while(time<1.6f){time+=Time.deltaTime;ash.transform.position=Vector3.Lerp(start,director.phoneView.FeedCamera.transform.position,time/1.6f);ash.transform.localScale=Vector3.one*Mathf.Lerp(1,.04f,time/1.6f);yield return null;}
            Destroy(ash);
        }
        private void SetActor(bool visible,bool exposed)
        {foreach(var r in W.lastri.GetComponentsInChildren<Renderer>(true)){r.enabled=visible;r.gameObject.layer=exposed?0:Phase2Lastri.VisibilityLayer;}}
        private void Motion(string name)
        {var a=W.lastri.GetComponentInChildren<Animation>();if(a&&a[name]&&!a.IsPlaying(name))a.CrossFade(name,.25f);}
        private void Stop(){if(Agent&&Agent.isOnNavMesh){Agent.isStopped=true;Agent.ResetPath();}}
        public void Suspend(){Stop();}
        public void Abort(){director.Campaign?.EndCheckpointAttempt(this);Stop();State=Phase2BossState.Inactive;}
        private void OnDestroy(){if(director&&director.Campaign)director.Campaign.EndCheckpointAttempt(this);}
    }
}
