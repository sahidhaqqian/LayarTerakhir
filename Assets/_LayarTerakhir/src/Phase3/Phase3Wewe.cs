using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Phase3
{
    public enum Phase3WeweState { Dormant,Reveal,Roof,Landing,Patrol,Investigate,Search,Chase,Loiter,Ambush,Resolved }
    // Authored truss/landing anchors plus the existing floor NavMesh. No 3D moving navmesh.
    public sealed class Phase3Wewe : MonoBehaviour
    {
        public Phase3Director director;
        public NavMeshAgent agent;
        public Phase3WeweState State { get; private set; }
        public bool Pursuing=>State==Phase3WeweState.Chase||State==Phase3WeweState.Investigate||State==Phase3WeweState.Search;
        private Phase3World W=>director.world;
        private Phase3Balance B=>director.balance;
        public Vector3 Head=>W.wewe.transform.position+Vector3.up*2.2f;
        private float timer,learn,grab,repath,roofClock,lure,leaveHide,searchDelay=-1;
        private bool officeSearch,detectedHide;
        private Phase3EvpSource mimic;
        private Transform visual;
        private Quaternion visualBind;
        private float footDistance;
        private int point,roofPoint;
        private Vector3 lastKnown,landing;
        public void Restore()
        {
            State=director.Completed("P3_BOSS_WIN")?Phase3WeweState.Resolved:Phase3WeweState.Dormant;
            timer=grab=repath=roofClock=leaveHide=0;officeSearch=false;learn=B.learnWindow;lure=B.lureCooldown;searchDelay=-1;
            if(agent.enabled&&agent.isOnNavMesh)agent.ResetPath();agent.enabled=false;
            W.wewe.SetActive(State!=Phase3WeweState.Resolved);
            W.wewe.transform.position=W.roof[0].position;
            Motion();
            if(director.Completed("P3_EVIDENCE_EVP")&&!director.Completed("P3_EVP_SEARCH_DONE"))QueueOfficeSearch();
        }
        public void Suspend(){if(agent.enabled&&agent.isOnNavMesh)agent.isStopped=true;}
        public void QueueOfficeSearch(){if(!director.Completed("P3_EVP_SEARCH_DONE"))searchDelay=B.searchDelay;}
        public void Hear(Vector3 point,float radius)
        {
            if(State==Phase3WeweState.Dormant||State==Phase3WeweState.Resolved||Vector3.Distance(point,W.wewe.transform.position)>radius)return;
            lastKnown=point;
            if(State==Phase3WeweState.Roof){BeginLanding();return;}
            if(State!=Phase3WeweState.Reveal&&State!=Phase3WeweState.Landing){State=Phase3WeweState.Investigate;timer=6;}
        }
        public void Tick(float dt)
        {
            if(State==Phase3WeweState.Resolved)return;
            if(State==Phase3WeweState.Dormant)
            {
                if(director.CurrentRoom!="BOILER"&&director.CurrentRoom!="OFFICE"&&director.CurrentRoom!="BAGASSE"&&director.CurrentRoom!="BALCONY")return;
                State=Phase3WeweState.Reveal;timer=3;learn=B.learnWindow;
                director.soundscape.Cue("Impact",W.wewe.transform.position);
                HudController.Instance.ShowBroadcastNotification("WEWE GOMBEL — KEBAL. Periksa EVP sebelum mengikuti suara. Silang UV berarti lantai rapuh. Loker dan tungku mati memutus pandangan.",10);
                return;
            }
            learn=Mathf.Max(0,learn-dt);timer-=dt;roofClock+=dt;lure-=dt;leaveHide=Mathf.Max(0,leaveHide-dt);
            if(searchDelay>=0)
            {
                searchDelay-=dt;
                if(searchDelay<=0){lastKnown=W.Anchor("OFFICE").position;officeSearch=true;BeginLanding();searchDelay=-1;}
            }
            if(State==Phase3WeweState.Reveal){if(timer<=0){State=Phase3WeweState.Roof;roofClock=0;}return;}
            if(State==Phase3WeweState.Roof)
            {
                agent.enabled=false;
                var target=W.roof[roofPoint].position;W.wewe.transform.position=Vector3.MoveTowards(W.wewe.transform.position,target,B.ceilingSpeed*dt);
                if(Vector3.Distance(W.wewe.transform.position,target)<.15f)roofPoint=(roofPoint+1)%W.roof.Length;
                if(roofClock>12&&Vector3.Distance(W.wewe.transform.position,director.player.transform.position)>B.revealDistance)BeginLanding();
                return;
            }
            if(State==Phase3WeweState.Landing)
            {
                agent.enabled=false;
                if(Vector3.Distance(landing,director.player.transform.position)<3){State=Phase3WeweState.Roof;roofClock=0;return;}
                Vector3 travel=new Vector3(landing.x,Mathf.Max(6,W.wewe.transform.position.y),landing.z);
                Vector3 horizontal=W.wewe.transform.position-landing;horizontal.y=0;
                W.wewe.transform.position=Vector3.MoveTowards(W.wewe.transform.position,horizontal.magnitude>.1f?travel:landing,dt*2);
                if(Vector3.Distance(W.wewe.transform.position,landing)<.05f)
                {
                    agent.enabled=true;
                    if(!agent.Warp(landing)){agent.enabled=false;State=Phase3WeweState.Roof;return;}
                    director.soundscape.Cue("Impact",landing);State=officeSearch?Phase3WeweState.Search:Phase3WeweState.Patrol;timer=officeSearch?18:8;roofClock=0;
                }
                return;
            }
            Vector3 p=director.player.transform.position,eye=p+Vector3.up*.8f;
            float distance=Vector3.Distance(W.wewe.transform.position,p);
            bool line=director.ClearLine(Head,eye,W.wewe.transform);
            bool screen=director.Phone.isPhoneRaised&&director.Phone.isScreenOn&&director.Phone.batteryPercent>0;
            // Hearing the illuminated screen at the doorway is different from seeing through its walls.
            bool doorway=director.Hidden&&director.ClearLine(Head,director.HidingEntrance+Vector3.up*.8f);
            if(director.Hidden)
            {
                detectedHide=screen&&distance<B.screenRadius&&doorway;
                if(detectedHide){State=Phase3WeweState.Chase;lastKnown=director.HidingEntrance;}
                else if(!line&&distance<8&&leaveHide<=0)
                {
                    if(State!=Phase3WeweState.Loiter){State=Phase3WeweState.Loiter;timer=B.loiterSeconds;}
                    if(timer>0){Suspend();return;}
                    State=Phase3WeweState.Patrol;leaveHide=20;
                    float farthest=-1;
                    for(int i=0;i<W.patrol.Length;i++)
                    {float score=(W.patrol[i].position-p).sqrMagnitude;if(score>farthest){farthest=score;point=i;}}
                    Follow(W.patrol[point].position,B.patrolSpeed,dt);return;
                }
            }
            else detectedHide=false;
            var input=LayarTerakhir.Core.GameInputConfig.Instance;
            bool moving=input.GetActionHeld(LayarTerakhir.Core.GameAction.MoveForward)||input.GetActionHeld(LayarTerakhir.Core.GameAction.MoveBackward)||input.GetActionHeld(LayarTerakhir.Core.GameAction.MoveLeft)||input.GetActionHeld(LayarTerakhir.Core.GameAction.MoveRight);
            float noise=moving?(director.player.IsCrouching?2:input.GetActionHeld(LayarTerakhir.Core.GameAction.Sprint)?8:4):0;
            bool detected=detectedHide||line&&(screen&&distance<B.screenRadius||distance<8&&Vector3.Dot(W.wewe.transform.forward,(p-W.wewe.transform.position).normalized)>.3f||distance<noise);
            if(detected&&learn<=0){State=Phase3WeweState.Chase;lastKnown=p;timer=5;}
            if(State==Phase3WeweState.Chase)
            {
                Follow(lastKnown,B.chaseSpeed,dt);
                if(!detected&&timer<=0){State=Phase3WeweState.Search;timer=6;}
                if((line||detectedHide)&&Vector3.Distance(W.wewe.transform.position,director.Hidden?director.HidingEntrance:p)<=B.grabRange)
                {
                    if(grab<=0){director.soundscape.Cue("Cloth",p);HudController.Instance.ShowBroadcastNotification("KAIN MENYAMBAR — MENJAUH!",1);}
                    grab+=dt;if(grab>=B.grabTelegraph){director.Die("CLOTH_GRAB — tertangkap gendongan Wewe");return;}
                }
                else grab=0;
            }
            else if(State==Phase3WeweState.Investigate||State==Phase3WeweState.Search)
            {
                Follow(lastKnown,B.patrolSpeed,dt);
                if(timer<=0)
                {
                    officeSearch=false;
                    if(director.Completed("P3_EVIDENCE_EVP")&&!director.Completed("P3_EVP_SEARCH_DONE"))director.Commit("P3_EVP_SEARCH_DONE");
                    State=Phase3WeweState.Patrol;point=(point+1)%W.patrol.Length;timer=8;
                }
            }
            else if(State==Phase3WeweState.Ambush)
            {
                Follow(lastKnown,B.patrolSpeed,dt);
                if(timer<=0)State=Phase3WeweState.Patrol;
            }
            else
            {
                State=Phase3WeweState.Patrol;
                Follow(W.patrol[point].position,B.patrolSpeed*(learn>0?B.learnSpeed:1),dt);
                if(Vector3.Distance(W.wewe.transform.position,W.patrol[point].position)<.6f)point=(point+1)%W.patrol.Length;
                if(roofClock>35&&distance>12){Suspend();agent.enabled=false;State=Phase3WeweState.Roof;roofClock=0;}
            }
            if(lure<=0&&!Pursuing&&learn<=0)
            {
                lure=B.lureCooldown;
                Transform destination=null;
                foreach(var a in W.patrol)if(Vector3.Distance(a.position,p)>=15){destination=a;break;}
                if(!destination)return;
                lastKnown=destination.position;State=Phase3WeweState.Ambush;timer=12;
                if(!mimic)
                {
                    var go=new GameObject("AuthoredWeweMimicEVP");go.transform.SetParent(transform);
                    mimic=go.AddComponent<Phase3EvpSource>();mimic.sourceId="WEWE_MIMIC";mimic.tag=Phase3VoiceTag.Mimic;mimic.priority=4;mimic.subtitle="Tiruan Damar dari balik ketel";
                    var old=W.sources;W.sources=new Phase3EvpSource[old.Length+1];System.Array.Copy(old,W.sources,old.Length);W.sources[old.Length]=mimic;
                }
                mimic.transform.position=lastKnown+Vector3.up;
                director.soundscape.Cue("Mimic",mimic.transform.position);
                HudController.Instance.EnqueueSubtitle("Suara Tiruan — Damar","Ka... sini. Gue di balik ketel...",5);
                foreach(var tile in W.floors)if(tile.dynamicCandidate)tile.Arm();
            }
            Motion();
        }
        private void BeginLanding()
        {
            foreach(var a in W.patrol)
            {
                if(Vector3.Distance(a.position,director.player.transform.position)<B.revealDistance)continue;
                if(!NavMesh.SamplePosition(a.position,out var hit,.5f,NavMesh.AllAreas))continue;
                landing=hit.position;agent.enabled=false;
                // Only descend from directly over a pre-authored clear landing.
                State=Phase3WeweState.Landing;return;
            }
            State=Phase3WeweState.Roof;roofClock=0;
        }
        public void Follow(Vector3 position,float speed,float dt)
        {
            if(!agent.enabled||!agent.isOnNavMesh)return;
            repath-=dt;if(repath<=0){repath=.2f;agent.isStopped=false;agent.speed=speed;agent.SetDestination(position);}
        }
        public void ResolveForSeal(){State=Phase3WeweState.Resolved;Suspend();agent.enabled=false;var body=W.wewe.GetComponent<CapsuleCollider>();if(body)body.enabled=false;}
        private void LateUpdate()
        {
            if(!director||!W||!W.wewe||!W.wewe.activeSelf)return;
            Motion();
            if(director.ClockRunning&&agent.enabled&&agent.isOnNavMesh&&State!=Phase3WeweState.Resolved)
            {
                footDistance+=agent.velocity.magnitude*Time.deltaTime;
                if(footDistance>=1.1f){footDistance%=1.1f;director.soundscape.Footstep(W.wewe.transform.position);}
            }
        }
        public void Motion()
        {
            if(State==Phase3WeweState.Resolved)return;
            if(!visual){visual=W.wewe.transform.Find("SourceFit");if(visual)visualBind=visual.localRotation;}
            bool ceiling=!director.boss.Active&&(State==Phase3WeweState.Roof||State==Phase3WeweState.Reveal);
            if(visual)visual.localRotation=Quaternion.Slerp(visual.localRotation,visualBind*(ceiling?Quaternion.Euler(0,0,180):Quaternion.identity),1-Mathf.Exp(-5*Time.deltaTime));
            var body=W.wewe.GetComponent<CapsuleCollider>();if(body)body.enabled=director.boss.Active||!ceiling&&State!=Phase3WeweState.Landing;
            var anim=W.wewe.GetComponentInChildren<Animation>();if(!anim)return;
            if(!anim.isPlaying)anim.Play();
            bool frozen=director.boss&&director.boss.State==Phase3BossState.Exposed;
            float speed=frozen?0:agent.enabled&&agent.isOnNavMesh?Mathf.Clamp(agent.velocity.magnitude/Mathf.Max(.1f,B.patrolSpeed),.4f,1.3f):.6f;
            foreach(AnimationState clip in anim)clip.speed=speed;
        }
    }
}
