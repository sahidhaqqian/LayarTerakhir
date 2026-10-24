using System.Collections.Generic;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;

namespace LayarTerakhir.Phase2
{
    public enum LastriInvestigationState { Dormant, Telegraph, Attached, Fading, Detached }
    public sealed class Phase2Lastri : MonoBehaviour
    {
        // 27 belongs to the Prolog mirror. 25 is unused by the four previous scenes/code.
        public const int VisibilityLayer = 25;
        public Phase2Director director;
        public NavMeshAgent agent;
        public LastriInvestigationState State { get; private set; }
        public bool Attached => State == LastriInvestigationState.Attached;
        public float TouchMeter { get; private set; }
        public float HoldProgress { get; private set; }
        public bool Learning { get; private set; }
        public bool InFrame { get; private set; }
        public Vector3 Head => director.world.lastri.transform.position + Vector3.up * 1.5f;
        private Phase2Balance B => director.balance;
        private readonly List<Vector3> trail = new List<Vector3>(192);
        private float timer, interval, learningTime, turnTime, hairTime, hideTime, uiGrace, occlusion, repath;
        private Vector3 previousPlayer;
        private bool firstAttachDone, uiGraceUsed, wasJournal, lastTurnCone, inL2, hairWarned, wasSuspended;
        private int darkSegment = -1;
        private float flicker;
        private Renderer[] renderers;
        private UnityEngine.Animation motion;
        private MaterialPropertyBlock properties;
        public void Restore()
        {
            State=LastriInvestigationState.Dormant; TouchMeter=HoldProgress=timer=turnTime=hairTime=hideTime=uiGrace=occlusion=repath=0;
            firstAttachDone=Learning=uiGraceUsed=lastTurnCone=false; interval=B.telegraphSeconds; learningTime=0;
            inL2=director.player.transform.position.y>2;
            var home=inL2?director.world.lastriHomeL2:director.world.lastriHome;
            if(agent && NavMesh.SamplePosition(home.position,out var hit,2,NavMesh.AllAreas)) agent.Warp(hit.position);
            if(agent && agent.isOnNavMesh){agent.isStopped=true;agent.ResetPath();}
            previousPlayer=director.player.transform.position; trail.Clear();trail.Add(previousPlayer);
            renderers=director.world.lastri.GetComponentsInChildren<Renderer>(true);properties=new MaterialPropertyBlock();
            motion=director.world.lastri.GetComponentInChildren<UnityEngine.Animation>();
            darkSegment=-1;flicker=0;SetVisible(false);
        }
        private void SetVisible(bool visible)
        { if(renderers!=null)foreach(var r in renderers)r.enabled=visible; }
        public void Tick(float dt)
        {
            var player=director.player;
            if(wasSuspended){wasSuspended=false;previousPlayer=player.transform.position;if(State==LastriInvestigationState.Telegraph)director.soundscape?.Cue("Moan",director.world.lastri.transform.position);}
            Vector3 delta=player.transform.position-previousPlayer;delta.y=0;previousPlayer=player.transform.position;
            bool forward=delta.magnitude/Mathf.Max(.001f,dt)>=B.holdMinSpeed && Vector3.Dot(delta.normalized,player.transform.forward)>.65f;
            bool l2=player.transform.position.y>2;
            string room=director.CurrentRoom;
            bool danger=room=="MORGUE"||room=="ARCHIVE"||room=="DUTY"||room=="L2";
            if (director.IsQuiet || !danger)
            {
                if (Attached || State==LastriInvestigationState.Telegraph) Detach();
                if(State==LastriInvestigationState.Fading)Fade(dt);
                else{State=LastriInvestigationState.Dormant;timer=0;SetVisible(false);StopAgent();}
                trail.Clear();trail.Add(player.transform.position);return;
            }
            if(l2!=inL2)
            {
                // Fixed, authored, nonfatal floor entry, never an attached teleport behind the player.
                inL2=l2;State=LastriInvestigationState.Dormant;timer=0;TouchMeter=0;
                var home=l2?director.world.lastriHomeL2:director.world.lastriHome;
                if(agent && NavMesh.SamplePosition(home.position,out var floor,2,NavMesh.AllAreas))agent.Warp(floor.position);
                trail.Clear();trail.Add(player.transform.position);SetVisible(false);
            }
            if(trail.Count==0 || Vector3.Distance(trail[trail.Count-1],player.transform.position)>.12f)
            {trail.Add(player.transform.position);if(trail.Count>192)trail.RemoveAt(0);}
            if(l2)Darken(dt);
            if(State==LastriInvestigationState.Fading){Fade(dt);return;}
            timer+=dt;
            if(State==LastriInvestigationState.Dormant)
            { if(timer>=(l2?B.l2Interval:firstAttachDone?(director.MorgueCleared?B.clearedInterval:B.moanInterval):B.telegraphSeconds))BeginTelegraph(l2?B.l2Telegraph:B.telegraphSeconds);return; }
            if(State==LastriInvestigationState.Detached)
            { if(timer>=interval)BeginTelegraph(l2?B.l2Telegraph:B.telegraphSeconds);return; }
            Vector3 target=TrailPoint(B.trailDistance);
            bool pathValid=Follow(target,dt);
            if(State==LastriInvestigationState.Telegraph)
            {
                float distance=Vector3.Distance(player.transform.position,director.world.lastri.transform.position);
                if(timer>=interval && pathValid && distance>=.85f && distance<=3 &&
                    director.ClearLine(player.transform.position+Vector3.up,Head))Attach();
                return;
            }
            if(!Attached)return;
            bool journal=director.Phone.isPhoneRaised&&director.Phone.isScreenOn&&director.phoneView.Apps.CurrentApp!=PhoneApp.Camera;
            if(journal&&!wasJournal&&!uiGraceUsed){uiGrace=B.uiGrace;uiGraceUsed=true;}
            wasJournal=journal;uiGrace=Mathf.Max(0,uiGrace-dt);
            bool framed=director.CameraCanScan&&director.Phone.isSelfieCamera&&FramesLastri();
            if(framed)occlusion=0;else occlusion+=dt;
            InFrame=framed||occlusion<=B.occlusionGrace&&InFrame;
            if(InFrame && forward)HoldProgress+=dt;
            if(HoldProgress>=B.holdSeconds){Detach();return;}
            if(director.Hidden&&!director.Phone.isScreenOn){hideTime+=dt;if(hideTime>=B.hideSeconds){Detach();return;}}
            else hideTime=0;
            if(Learning)
            {
                learningTime+=dt;
                if(learningTime>=B.learnSeconds)
                {Learning=false;TouchMeter=hairTime=turnTime=0;BeginTelegraph(B.telegraphSeconds);return;}
            }
            float multiplier=director.GasActive?1.5f:1;
            if(l2&&darkSegment>=0)multiplier*=B.darkMultiplier;
            if(!director.Hidden) TouchMeter=Mathf.Clamp(TouchMeter+(InFrame?-B.touchFall:(l2?B.l2TouchRise:B.touchRise)*multiplier)*dt,0,100);
            Vector3 toGhost=director.world.lastri.transform.position-player.transform.position;toGhost.y=0;
            bool turn=toGhost.sqrMagnitude>.01f&&Vector3.Angle(player.transform.forward,toGhost)<B.turnAngle;
            if(turn&&!lastTurnCone)
            { TouchMeter=Mathf.Min(100,TouchMeter+20);director.phoneView.Glitch(.25f,.5f); }
            lastTurnCone=turn;
            turnTime=turn?turnTime+dt:0;
            if(!Learning && turnTime>=B.turnHold && !director.Hidden){director.Die("POUNCE — jangan menoleh langsung saat Lastri menempel. Gunakan Selfie.");return;}
            if(TouchMeter>=100&&!InFrame&&!director.Hidden&&uiGrace<=0)hairTime+=dt;else hairTime=0;
            if(TouchMeter>=100&&!hairWarned&&!Learning)
            {hairWarned=true;HudController.Instance.ShowBroadcastNotification("RAMBUT MENYENTUH BAHU — pantau Selfie atau sembunyi sekarang!",B.hairSeconds);director.soundscape?.Cue("Breath",director.player.transform.position);}
            if(TouchMeter<100)hairWarned=false;
            if(!Learning&&hairTime>=B.hairSeconds){director.Die("GRAB — rambutnya menyentuh bahu. Pantau lewat Selfie sambil maju, atau sembunyi dan matikan layar.");return;}
            director.phoneView.ShowChapterReadout((director.ThermoActive?$"THERMOKLENIK  {director.Temperature}°C\n":"")+
                (Learning?"JANGAN MENOLEH • JENDELA BELAJAR\n":"")+
                (TouchMeter>=100?"RAMBUT MENYENTUH BAHU!":InFrame?$"SELFIE • MAJU {HoldProgress:0.0}/{B.holdSeconds:0}s":$"PANTAU BELAKANG • {TouchMeter:0}%"),TouchMeter>=80?new Color(1,.27f,.2f):Color.white);
        }
        private void BeginTelegraph(float seconds)
        {
            State=LastriInvestigationState.Telegraph;timer=0;interval=seconds;SetVisible(true);
            director.soundscape?.Cue("Moan",director.world.lastri.transform.position);
            HudController.Instance.ShowBroadcastNotification("Hembusan dingin di tengkuk. Kamera belakang tidak melihatnya. " +
                GameInputConfig.Instance.GetActionLabel(GameAction.ModeSelfie)+" Selfie • tetap maju. Jangan menoleh langsung.",5);
        }
        private void Attach()
        {
            State=LastriInvestigationState.Attached;timer=turnTime=hairTime=hideTime=uiGrace=occlusion=0;
            TouchMeter=HoldProgress=0;InFrame=uiGraceUsed=lastTurnCone=false;wasJournal=false;
            if(!firstAttachDone){Learning=true;learningTime=0;firstAttachDone=true;director.ScareReceipt();}
            director.soundscape?.Cue("Breath",director.world.lastri.transform.position);
        }
        private void Detach()
        { State=LastriInvestigationState.Fading;timer=0;TouchMeter=HoldProgress=hairTime=turnTime=0;Learning=false;InFrame=false;StopAgent(); }
        private void Fade(float dt)
        {
            timer+=dt;
            if(timer>=2)
            {SetVisible(false);State=LastriInvestigationState.Detached;timer=0;interval=inL2?B.l2Interval:director.MorgueCleared?B.clearedInterval:B.moanInterval;}
        }
        private void StopAgent(){if(agent&&agent.isOnNavMesh){agent.isStopped=true;agent.ResetPath();}}
        private Vector3 TrailPoint(float behind)
        {
            if(trail.Count<2)return director.world.lastri.transform.position;
            for(int i=trail.Count-1;i>0;i--)
            {
                float length=Vector3.Distance(trail[i],trail[i-1]);
                if(length>=behind)
                {
                    Vector3 tangent=trail[i]-trail[i-1];tangent.y=0;
                    Vector3 shoulder=Vector3.Cross(Vector3.up,tangent.normalized)*-.58f;
                    Vector3 candidate=Vector3.Lerp(trail[i],trail[i-1],behind/Mathf.Max(.001f,length))+shoulder;
                    return NavMesh.SamplePosition(candidate,out var side,.7f,NavMesh.AllAreas)?side.position:trail[i-1];
                }
                behind-=length;
            }
            return trail[0];
        }
        private bool Follow(Vector3 target,float dt)
        {
            if(!agent||!agent.isOnNavMesh)return false;
            repath-=dt;
            if(repath<=0)
            {repath=.18f;agent.isStopped=false;agent.SetDestination(target);}
            if(agent.velocity.sqrMagnitude>.03f)director.world.lastri.transform.rotation=Quaternion.Slerp(director.world.lastri.transform.rotation,
                Quaternion.LookRotation(director.player.transform.position-director.world.lastri.transform.position,Vector3.up),dt*5);
            string pose=agent.velocity.sqrMagnitude>.03f?"Walk":"Idle";
            if(motion&&motion[pose]&&!motion.IsPlaying(pose))motion.CrossFade(pose,.2f);
            return !agent.pathPending&&agent.pathStatus==NavMeshPathStatus.PathComplete;
        }
        public bool FramesLastri()
        {
            var cam=director.phoneView.FeedCamera;if(!cam)return false;
            Vector3 centre=director.world.lastri.transform.position+Vector3.up*1.1f;
            Vector3 right=cam.transform.right*.3f,up=Vector3.up*.65f;
            float minX=float.PositiveInfinity,minY=float.PositiveInfinity,maxX=float.NegativeInfinity,maxY=float.NegativeInfinity;
            int visible=0;
            for(int x=-1;x<=1;x++)for(int y=-1;y<=1;y++)
            {
                Vector3 p=centre+right*x+up*y,v=cam.WorldToViewportPoint(p);
                if(v.z<=cam.nearClipPlane)return false;
                minX=Mathf.Min(minX,v.x);maxX=Mathf.Max(maxX,v.x);minY=Mathf.Min(minY,v.y);maxY=Mathf.Max(maxY,v.y);
                if(v.x>=0&&v.x<=1&&v.y>=0&&v.y<=1&&director.ClearLine(cam.transform.position,p))visible++;
            }
            float area=(maxX-minX)*(maxY-minY);
            float clipped=Mathf.Max(0,Mathf.Min(1,maxX)-Mathf.Max(0,minX))*Mathf.Max(0,Mathf.Min(1,maxY)-Mathf.Max(0,minY));
            return area>.000001f&&clipped/area>=B.inFrameRatio&&visible>=3;
        }
        public void CallFromFormalin()
        { if(State==LastriInvestigationState.Detached||State==LastriInvestigationState.Dormant)BeginTelegraph(B.formalinTelegraph); }
        public void SuspendClock()
        {wasSuspended=true;StopAgent();if(State==LastriInvestigationState.Telegraph)timer=0;}
        private void Darken(float dt)
        {
            int segment=Mathf.Clamp(Mathf.FloorToInt(director.player.transform.position.x/12),0,2);
            if(segment!=darkSegment){darkSegment=segment;flicker=0;director.soundscape?.Cue("Hiss",director.player.transform.position+Vector3.up*2);}
            flicker+=dt;
            if(segment<director.world.corridorLights.Length && director.world.corridorLights[segment])
                director.world.corridorLights[segment].enabled=flicker<B.flickerSeconds&&Mathf.Sin(flicker*33)>0;
        }
    }
}
