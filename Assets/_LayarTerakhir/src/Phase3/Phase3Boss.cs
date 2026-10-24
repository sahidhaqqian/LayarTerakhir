using System.Collections.Generic;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Phase3
{
    public enum Phase3BossState { Inactive,Prep,Stalk,Exposed,Recovery,VictoryPending,Resolved }
    public sealed class Phase3Boss : MonoBehaviour
    {
        public Phase3Director director;
        public Phase3BossState State { get; private set; }
        public int Layers { get; private set; }
        public int PowerTokens { get; private set; }=1;
        public float ResetCooldown { get; private set; }
        public bool Active=>State!=Phase3BossState.Inactive&&State!=Phase3BossState.Resolved;
        public bool Recovering=>State==Phase3BossState.Prep||State==Phase3BossState.Recovery;
        public bool CanCharge=>Recovering||State==Phase3BossState.Resolved;
        public bool PullActive { get; private set; }
        public bool ResolvingVisual { get; private set; }
        public bool[] Spent { get; }=new bool[3];
        public float[] Cooldowns { get; }=new float[3];
        public readonly List<string> UsedFlares=new List<string>();
        public float Remaining=>Mathf.Max(0,timer);
        public string Status=>State==Phase3BossState.Exposed?$"PERISAI HANCUR • {Remaining:0.0}s\nBIDIK DADA / WAJAH • SEGEL {Layers}/3":
            State==Phase3BossState.Stalk?$"SEGEL {Layers}/3 • PANCING KE CAHAYA"+(PullActive?"\nPUTUS TARIKAN: UV SIMPUL / SOROTAN / FLARE":""):
            State==Phase3BossState.VictoryPending?"SEGEL BELUM TERSIMPAN • E DI KARUNG: COBA SIMPAN LAGI":$"JEDA {Remaining:0}s • RAK DAYA MANUAL\nE DI KARUNG UNTUK LANJUT • JATAH DAYA {PowerTokens}";
        private Phase3World W=>director.world;
        private Phase3Balance B=>director.balance;
        private NavMeshAgent Agent=>director.wewe.agent;
        private float timer,grab,pullTime,pullDwell,repath,sourceDwell;
        private int source=-1;
        private readonly float[] bursts=new float[3];
        private Phase3Flare exposedFlare;
        private Phase3Flare dwellingFlare;
        private bool pullArmed;
        public void Restore()
        {
            StopAllCoroutines();ResolvingVisual=false;Abort();Layers=0;UsedFlares.Clear();PowerTokens=1;ResetCooldown=0;PullActive=false;
            for(int i=0;i<3;i++){Spent[i]=false;Cooldowns[i]=bursts[i]=0;W.ritualLights[i].enabled=false;}
            State=director.Completed("P3_BOSS_WIN")?Phase3BossState.Resolved:Phase3BossState.Inactive;
            W.arenaLock.SetActive(false);W.mist.SetActive(false);W.clothKnot.SetActive(false);W.serviceDoor.GetComponent<Phase3Gate>().SetOpen(true,true);
            director.phoneView.ShowChapterTarget(Vector3.zero,false);
        }
        public bool Begin()
        {
            if(Active||State==Phase3BossState.Resolved||!NearDecoy()||!director.Altar.All(x=>x)||!Phase3Campaign.PrepareRitual(director.Campaign))
            {HudController.Instance.ShowBroadcastNotification("Pasang tiga pusaka, lengkapi bukti dan konfirmasi Jurnal. Ritual menunggu checkpoint berhasil disimpan.",5);return false;}
            if(!director.Campaign.BeginCheckpointAttempt(this))return false;
            director.wewe.Suspend();Agent.enabled=true;
            if(!Agent.Warp(W.bossHome.position)){director.Campaign.EndCheckpointAttempt(this);Agent.enabled=false;return false;}
            W.wewe.SetActive(true);W.arenaLock.SetActive(true);W.serviceGate.GetComponent<Phase3Gate>().SetOpen(false);W.hallGate.GetComponent<Phase3Gate>().SetOpen(false);Layers=0;UsedFlares.Clear();PowerTokens=1;
            EnterRecovery(true);director.soundscape.Cue("Impact",W.bossHome.position);
            HudController.Instance.EnqueueSubtitle("Raka","Damar?! Dam, bertahan gue lepasin ikatan lo—... ini karung?!",8);
            return true;
        }
        public bool NearDecoy()=>Vector3.Distance(director.player.transform.position+Vector3.up,W.decoy.position)<=2.25f&&director.ClearLine(director.player.transform.position+Vector3.up,W.decoy.position,W.decoy.parent);
        public bool ContinueCycle()
        {
            if(!NearDecoy())return false;
            if(State==Phase3BossState.VictoryPending)return CommitVictory();
            if(!Recovering||timer>0||director.Phone.batteryPercent<director.Phone.blitzCostPercent||director.Phone.BlitzCooldownRemaining>0||director.AlcoveClosed)
            {HudController.Instance.ShowBroadcastNotification("Isi daya manual. Tunggu jeda, buka alcove, lalu E di karung untuk lanjut.",4);return false;}
            State=Phase3BossState.Stalk;grab=pullTime=pullDwell=sourceDwell=0;source=-1;pullArmed=Layers==2;PullActive=false;W.mist.SetActive(true);W.clothKnot.SetActive(false);
            foreach(var tile in W.floors)if(tile.dynamicCandidate&&tile.kind==Phase3FloorKind.FragileFatal)tile.Arm();
            return true;
        }
        public void Tick(float dt)
        {
            if(!Active)return;
            timer-=dt;ResetCooldown=Mathf.Max(0,ResetCooldown-dt);
            for(int i=0;i<3;i++){Cooldowns[i]=Mathf.Max(0,Cooldowns[i]-dt);bursts[i]=Mathf.Max(0,bursts[i]-dt);W.ritualLights[i].enabled=bursts[i]>0;}
            if(State==Phase3BossState.VictoryPending){Stop();Present();return;}
            if(Recovering){Follow(W.bossRetreat.position,B.patrolSpeed,dt);Present();return;}
            if(State==Phase3BossState.Exposed)
            {
                Stop();if(timer<=0)EnterRecovery(false);Present();return;
            }
            if(director.Phone.batteryPercent<director.Phone.blitzCostPercent&&!PullActive)
            {EnterRecovery(false);return;}
            if(director.AlcoveClosed&&director.InAlcove){EnterRecovery(false);return;}
            Follow(director.player.transform.position,B.stalkSpeed,dt);
            if(pullArmed&&director.player.transform.position.y<1&&Vector3.Distance(W.wewe.transform.position,director.player.transform.position)<=B.aimRange&&director.ClearLine(director.wewe.Head,director.player.cameraPivot.position,W.wewe.transform))
            {
                pullArmed=false;PullActive=true;pullTime=pullDwell=0;W.clothKnot.SetActive(true);
                director.soundscape.Cue("Cloth",director.player.transform.position);
                HudController.Instance.EnqueueSubtitle("Suara Tiruan — Ibu Raka / Damar","Raka... sini, Nak... Ka, tolong...",7);
            }
            for(int i=0;i<3;i++)if(bursts[i]>0&&!Spent[i])
            {
                if(PullActive&&KnotInBeam(i))BreakPull();
                if(InsideBeam(W.wewe.transform.position,i)){Expose(i,null);return;}
            }
            Phase3Flare nearby=null;
            foreach(var flare in director.Flares.Where(f=>f&&f.Landed&&f.Eligible))
            {
                if(PullActive&&Vector3.Distance(flare.transform.position,W.clothKnot.transform.position)<B.flareRadius)BreakPull();
                if(Vector3.Distance(flare.transform.position,W.wewe.transform.position)<B.flareRadius&&director.ClearLine(flare.transform.position+Vector3.up*.2f,director.wewe.Head,W.wewe.transform))nearby=flare;
            }
            if(nearby!=dwellingFlare){sourceDwell=0;dwellingFlare=nearby;}
            if(nearby){sourceDwell+=dt;if(sourceDwell>=B.flareDwell){Expose(-1,nearby);return;}}
            else sourceDwell=0;
            if(PullActive)
            {
                pullTime+=dt;
                Vector3 facing=director.player.transform.position-W.wewe.transform.position;facing.y=0;
                W.clothKnot.transform.position=W.wewe.transform.position+Vector3.up*1.4f+facing.normalized*.55f;
                bool uv=director.CameraCanScan&&director.Phone.currentScanMode==LayarTerakhir.Smartphone.ScanVisualMode.UV&&director.Aim(W.clothKnot.transform.position,B.aimRange,25,W.wewe.transform);
                pullDwell=uv?pullDwell+dt:0;
                if(pullDwell>=B.pullUvSeconds)BreakPull();
                if(PullActive&&pullTime>B.pullTelegraph)
                {
                    Vector3 inward=W.pullCentre.position-director.player.transform.position;inward.y=0;
                    director.player.GetComponent<CharacterController>().Move(inward.normalized*B.pullSpeed*dt);
                    float rim=Mathf.Max(0,Mathf.Abs(inward.x)-W.furnacePit.extents.x,Mathf.Abs(inward.z)-W.furnacePit.extents.z);
                    if(rim<=.65f&&pullTime>=B.pullLimit){director.Die("PULL_IN — gendongan menarik melewati pagar tungku");return;}
                }
            }
            float distance=Vector3.Distance(W.wewe.transform.position,director.player.transform.position);
            bool contact=distance<=B.grabRange&&director.ClearLine(director.wewe.Head,director.player.transform.position+Vector3.up,W.wewe.transform);
            if(contact)
            {
                if(grab<=0){director.soundscape.Cue("Cloth",director.player.transform.position);HudController.Instance.ShowBroadcastNotification("KAIN MENYAMBAR — MENJAUH!",1);}
                grab+=dt;if(grab>=B.grabTelegraph){director.Die("CLOTH_GRAB — terlalu dekat di luar exposure");return;}
            }
            else grab=0;
            Present();
        }
        public bool InsideBeam(Vector3 feet,int index)
        {
            Vector3 delta=feet-W.beams[index].position;delta.y=0;
            var light=W.ritualLights[index];Vector3 target=feet+Vector3.up*1.5f,ray=target-light.transform.position;
            return delta.magnitude<=B.beamRadius&&ray.magnitude<=light.range&&Vector3.Angle(light.transform.forward,ray)<=light.spotAngle*.5f&&director.ClearLine(light.transform.position,target,W.wewe.transform);
        }
        private bool KnotInBeam(int index)
        =>InsideBeam(W.clothKnot.transform.position-Vector3.up*1.5f,index);
        public bool UseLamp(int index)
        {
            if(State!=Phase3BossState.Stalk||index<0||index>=3||Spent[index]||Cooldowns[index]>0)return false;
            if(Vector3.Distance(director.player.transform.position+Vector3.up,W.levers[index].position+Vector3.up*.6f)>1.8f ||
                !director.ClearLine(director.player.transform.position+Vector3.up,W.levers[index].position+Vector3.up*.6f,W.levers[index]))return false;
            Cooldowns[index]=B.beamCooldown;bursts[index]=B.beamSeconds;W.ritualLights[index].enabled=true;
            director.soundscape.Cue("Generator",W.beams[index].position);
            if(PullActive&&KnotInBeam(index))BreakPull();
            if(InsideBeam(W.wewe.transform.position,index))Expose(index,null);
            return true;
        }
        private void Expose(int index,Phase3Flare flare)
        {
            if(State!=Phase3BossState.Stalk)return;
            source=index;exposedFlare=flare;State=Phase3BossState.Exposed;timer=B.exposedSeconds;BreakPull();Stop();W.mist.SetActive(false);
            director.soundscape.Cue("Impact",W.wewe.transform.position);Present();
        }
        private void BreakPull(){PullActive=pullArmed=false;W.clothKnot.SetActive(false);pullDwell=0;}
        public bool Capture()
        {
            if(!Active)return false;
            if(director.AlcoveClosed)return true;
            bool valid=State==Phase3BossState.Exposed&&director.Aim(director.wewe.Head,B.aimRange,B.aimAngle,W.wewe.transform);
            if(!director.Phone.TryCaptureEnergy(true)){HudController.Instance.ShowBroadcastNotification("Blitz membutuhkan 15% dan cooldown 6 detik. Rak daya tersedia selama jeda.",4);return true;}
            director.phoneView.CaptureFlash();
            if(!valid){HudController.Instance.ShowBroadcastNotification("Blitz tidak mengenai titik rentan. Generator belum korslet.",3);return true;}
            Layers++;if(source>=0){Spent[source]=true;bursts[source]=0;W.ritualLights[source].enabled=false;}
            if(exposedFlare)exposedFlare.Eligible=false;
            if(Layers==3){State=Phase3BossState.VictoryPending;Stop();W.wewe.SetActive(false);CommitVictory();}
            else{PowerTokens=1;EnterRecovery(false);}
            return true;
        }
        public bool CommitVictory()
        {
            if(State!=Phase3BossState.VictoryPending)return false;
            if(!Phase3Campaign.WinRitual(director.Campaign,this,Layers,UsedFlares))
            {HudController.Instance.ShowBroadcastNotification("Segel belum tersimpan. E pada karung untuk coba lagi; Continue tetap memuat CP5 utuh.",6);return false;}
            director.Campaign.EndCheckpointAttempt(this);State=Phase3BossState.Resolved;W.wewe.SetActive(false);W.mist.SetActive(false);W.arenaLock.SetActive(false);W.serviceGate.GetComponent<Phase3Gate>().SetOpen(true);
            foreach(var tile in W.floors)tile.Restore();
            director.RefreshWorld();director.soundscape.Resolve();director.phoneView.ShowChapterTarget(Vector3.zero,false);StartCoroutine(SealVisual());return true;
        }
        private IEnumerator SealVisual()
        {
            ResolvingVisual=true;director.wewe.ResolveForSeal();
            Vector3 scale=W.wewe.transform.localScale;W.wewe.SetActive(true);W.mist.SetActive(true);
            var particles=W.mist.GetComponent<ParticleSystem>();var main=particles.main;main.startColor=new Color(1.5f,1.3f,1,.75f);particles.Play();
            director.soundscape.Cue("Impact",W.wewe.transform.position);
            float t=0;
            while(t<1.6f)
            {
                if(director.ClockRunning){t+=Time.deltaTime;W.wewe.transform.localScale=scale*Mathf.Lerp(1,.02f,Mathf.SmoothStep(0,1,t/1.6f));}
                yield return null;
            }
            W.wewe.SetActive(false);W.mist.SetActive(false);W.wewe.transform.localScale=scale;ResolvingVisual=false;
        }
        private void EnterRecovery(bool prep)
        {State=prep?Phase3BossState.Prep:Phase3BossState.Recovery;timer=prep?B.prepSeconds:B.recoverySeconds;grab=0;BreakPull();W.mist.SetActive(false);}
        private void Follow(Vector3 point,float speed,float dt)
        {if(!Agent.enabled||!Agent.isOnNavMesh)return;repath-=dt;if(repath<=0){repath=.15f;Agent.isStopped=false;Agent.speed=speed;Agent.SetDestination(point);}}
        private void Stop(){if(director&&director.wewe&&Agent&&Agent.enabled&&Agent.isOnNavMesh){Agent.isStopped=true;Agent.ResetPath();}}
        private void Present()
        {director.phoneView.ShowChapterTarget(PullActive?W.clothKnot.transform.position:director.wewe.Head,State==Phase3BossState.Exposed||PullActive);director.phoneView.ShowChapterReadout(Status,State==Phase3BossState.Exposed?new Color(1,.45f,.28f):Color.white);}
        public void SpendPower(){if(Active)PowerTokens=Mathf.Max(0,PowerTokens-1);}
        public void ResetPower(){if(Recovering&&ResetCooldown<=0){PowerTokens=1;ResetCooldown=B.resetCooldown;}}
        public void TakeRackFlare()
        {
            if(!Active||director.Campaign.Data.inventory.Contains("P3_FLARE_RACK"))return;
            director.Campaign.Data.inventory.Add("P3_FLARE_RACK");
        }
        public void Suspend()=>Stop();
        public void Abort(){director.Campaign?.EndCheckpointAttempt(this);Stop();State=Phase3BossState.Inactive;}
        private void OnDestroy(){if(director&&director.Campaign)director.Campaign.EndCheckpointAttempt(this);}
    }
}
