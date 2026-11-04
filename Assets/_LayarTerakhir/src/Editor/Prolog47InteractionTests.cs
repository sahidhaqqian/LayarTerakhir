#if UNITY_EDITOR
using System.Collections;
using System.Linq;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed partial class AtmajaPlayTests
    {
        [UnityTest] public IEnumerator Prolog47LiveBeforeFrontDoorAndHomeDisablesAR()=>Run(LiveAndAR47());
        [UnityTest] public IEnumerator Prolog47PhoneMenuLeftDragLooksAndReleaseStops()=>Run(PhoneDrag47());
        [UnityTest] public IEnumerator Prolog47PursuitBlitzStillWorksAfterHandycam()=>Run(PursuitBlitz47());
        [UnityTest] public IEnumerator Prolog47WindowAppearsWithoutMovingPlayerThenAdvances()=>Run(Window47());
        [UnityTest] public IEnumerator Prolog47DoorLeavesStayVisibleAndVentNeedsCrouchAR()=>Run(DoorsVent47());
        [UnityTest] public IEnumerator Prolog47RottenFeastSmokePlaysOverTable()=>Run(FeastSmoke47());
        [UnityTest] public IEnumerator Prolog47VentWithoutCrowbarHintsAtTool()=>Run(VentHint47());
        [UnityTest] public IEnumerator Prolog47EvidencePhotosWorkInAnyOrder()=>Run(EvidenceAnyOrder47());
        private IEnumerator EvidenceAnyOrder47()
        {
            yield return new WaitForSeconds(1);
            var d=Find<AtmajaDirector>();var c=CampaignState.Instance;var phone=SmartphoneController.Instance;
            // Designer Nov: evidences must count in ANY order — rope first
            // with no diary read, no curtain, no earlier chains.
            foreach(var id in new[]{"EVENT_P0_GATE_OPEN","EVENT_P0_LIVE_STARTED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN","MATABATIN_APK","EVENT_P0_STAIRS_UNLOCKED","EVENT_P0_UPPER_REACHED"})c.AddInventoryItem(id);
            d.RestoreWorld();d.SetStage(AtmajaStage.EvidenceHunt);d.player.enabled=false;
            phone.arModuleUnlocked=true;phone.currentScanMode=ScanVisualMode.AR;phone.isPhoneRaised=phone.isScreenOn=true;phone.batteryPercent=100;
            d.phoneView.Apps.Open(PhoneApp.Camera);phone.SyncVisualState();yield return null;
            var lens=d.phoneView.FeedCamera;
            // Designer rupture relief replaces the flat decal (mesh owned by
            // the designer, re-textured in-repo toward the reference).
            var relief=GameObject.Find("CrimsonScratchRelief");
            Assert.That(relief,Is.Not.Null,"Designer rupture relief stands on the bathroom wall");
            var rb=KosVisualV2.BoundsOf(relief);
            Assert.That(rb.size.z,Is.GreaterThan(1f),"Relief spans the reference-wide slash field");
            Assert.That(rb.Contains(d.upper.scratchAim.position),Is.True,"Scratch aim sits on the relief face");
            // 1. Rope first: the hanged figure answers the rope photo itself.
            var rope=d.upper.ropeAim;
            lens.transform.position=rope.position-d.north*1.2f+Vector3.up*-.5f;
            lens.transform.LookAt(rope);Physics.SyncTransforms();d.upper.TryPhotoTick();
            Assert.That(c.HasInventoryItem("EVENT_P0_CHAIN3_BROKEN"),Is.True,"Rope photo counts without earlier evidences");
            yield return null;yield return null;
            Assert.That(d.upper.hangedFlashFigure&&d.upper.hangedFlashFigure.activeSelf,Is.True,"Photographing the rope reveals the hanged figure");
            Assert.That(c.HasInventoryItem("EVENT_P0_MASTER_OPEN"),Is.False,"Master stays shut until all three evidences are in");
            // 2. Cup second, still without reading the diary.
            var cup=d.upper.cupAim;
            lens.transform.position=cup.position+d.north*1.2f+Vector3.up*.7f;
            lens.transform.LookAt(cup);Physics.SyncTransforms();d.upper.TryPhotoTick();
            Assert.That(c.HasInventoryItem("EVENT_P0_CHAIN1_BROKEN"),Is.True,"Cup photo counts without the diary");
            // 3. Scratches last — no curtain, no diary, no order.
            var scratch=d.upper.scratchAim;
            lens.transform.position=scratch.position+d.east*1.2f+Vector3.up*.1f;
            lens.transform.LookAt(scratch);Physics.SyncTransforms();d.upper.TryPhotoTick();
            Assert.That(c.HasInventoryItem("EVENT_P0_CHAIN2_BROKEN"),Is.True,"Scratches photo lands last");
            Assert.That(d.upper.ChainsBroken,Is.EqualTo(3));
            Assert.That(c.HasInventoryItem("EVENT_P0_MASTER_OPEN"),Is.True,"All three evidences open the master door whichever lands last");
        }
        private IEnumerator VentHint47()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();
            Time.timeScale=1;UiFocus.Cinematic=false;
            // Designer Nov: trying the rusted grille empty-handed must tell
            // the player a pry tool is needed — never a silent stuck door.
            var vent=d.upper.ventDoor;
            Assert.That(vent,Is.Not.Null,"Vent grille door exists");
            Assert.That(vent.pryFlag,Is.EqualTo("EVENT_P0_VENT_PRIZED"));
            Assert.That(CampaignState.Instance.HasInventoryItem("ITEM_P0_CROWBAR"),Is.False,"Hint path runs before the crowbar is taken");
            vent.locked=true;
            string prompt=vent.GetInteractionPrompt();
            Assert.That(prompt.ToLowerInvariant(),Does.Contain("cari").Or.Contain("alat").Or.Contain("congkel"),
                "Locked vent prompt must hint a tool is needed without naming it, got: "+prompt);
            Assert.That(prompt.ToLowerInvariant(),Does.Not.Contain("linggis"),
                "Locked vent prompt must not spoil the tool name, got: "+prompt);
            Assert.That(vent.lockedDialogue.ToLowerInvariant(),Does.Contain("mencari sesuatu"),
                "Locked vent line must say Raka has to find something, got: "+vent.lockedDialogue);
            Assert.That(vent.lockedDialogue.ToLowerInvariant(),Does.Not.Contain("linggis"),
                "Locked vent line must let the player guess the tool, got: "+vent.lockedDialogue);
            vent.Interact();yield return null;
            Assert.That(CampaignState.Instance.HasInventoryItem("EVENT_P0_VENT_PRIZED"),Is.False,"Empty-handed pry must not rip the grille");
            Assert.That(vent.gameObject.activeInHierarchy,Is.True,"Grille stays on until the crowbar pries it");
        }
        private IEnumerator FeastSmoke47()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();
            Time.timeScale=1;UiFocus.Cinematic=false;
            var smokes=Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None).Where(s=>s.name.StartsWith("RottenStinkSmoke")).ToList();
            Assert.That(smokes.Count,Is.GreaterThanOrEqualTo(1).And.LessThanOrEqualTo(3),"1-3 small smoke wisps on select dishes, not a whole-table plume");
            foreach(var ps in smokes)
            {
                Assert.That(ps.gameObject.activeInHierarchy,Is.True,"Smoke wisp active in play");
                // Designer Nov: wisps are SMALL — 10-20cm puffs right above
                // their own plate, never half-metre clouds over the table.
                Assert.That(ps.main.startSize.constantMax,Is.LessThanOrEqualTo(.25f),"Smoke wisp stays small (<=25cm)");
                Assert.That(ps.shape.scale.magnitude,Is.LessThan(.5f),"Smoke emitter stays pinned to one dish");
                Assert.That(ps.emission.rateOverTime.constant,Is.LessThanOrEqualTo(6f),"Thin wisp, not a chimney");
            }
            var smoke=smokes[0].gameObject;
            var ps0=smokes[0];
            for(int w=0;w<2;w++)
            {
                yield return new WaitForSeconds(2);
                bool paused=false;try{paused=ps0.isPaused;}catch{paused=true;}
                float ptime=-1;try{ptime=ps0.time;}catch{}
                Debug.Log("FEASTPLAY_DIAG t="+w+" playing="+(ps0?ps0.isPlaying.ToString():"DESTROYED")+" paused="+paused+" time="+ptime+" emit="+(ps0?ps0.emission.enabled.ToString():"?")+" rate="+(ps0?ps0.emission.rateOverTime.constant.ToString():"?")+" count="+(ps0?ps0.particleCount.ToString():"?")+" active="+(smoke?smoke.activeInHierarchy.ToString():"?")+" ts="+Time.timeScale);
                var all=Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None);
                string samp="";foreach(var a in all.Take(14))samp+=a.name+":"+a.particleCount+" ";
                Debug.Log("FEASTPLAY_ALLPS total="+all.Length+" "+samp);
            }
            int totalPuffs=smokes.Sum(s=>{try{return s.particleCount;}catch{return 0;}});
            Assert.That(totalPuffs,Is.GreaterThan(3),"Stink-wisps actually emit while playing, not just in the editor");
            yield return new WaitForSeconds(1);
            var lamp=new GameObject("FeastPlayLamp").AddComponent<Light>();
            lamp.type=LightType.Spot;lamp.intensity=30;lamp.range=12;lamp.spotAngle=60;lamp.color=new Color(1f,.93f,.8f);
            lamp.transform.position=d.P(3.71f,4.5f,3.6f);
            lamp.transform.rotation=Quaternion.LookRotation(Vector3.down+d.east*.1f);
            var pot=GameObject.Find("AddedDish_TippedPot");
            Assert.That(pot,Is.Not.Null,"Tipped pot anchors the rotten spread");
            var b=KosVisualV2.BoundsOf(pot);
            // Capture through the player's own camera (the same path that
            // proves rain renders) rather than a fresh camera.
            var p=d.player;
            d.PlacePlayer(d.P(3.71f,2.35f,.54f),Quaternion.identity);
            p.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.FirstPerson);
            var delta=b.center-p.cameraPivot.position;var flat=Vector3.ProjectOnPlane(delta,Vector3.up);
            p.transform.rotation=Quaternion.LookRotation(flat);p.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            yield return null;yield return null;
            var eye=d.phoneView.playerCamera;Physics.SyncTransforms();
            var pr=smoke.GetComponent<ParticleSystemRenderer>();
            var pp=new ParticleSystem.Particle[ps0.particleCount];int nn=ps0.GetParticles(pp);
            string ws="";for(int k=0;k<Mathf.Min(5,nn);k++){ws+="["+pp[k].position.ToString("F2")+" s="+pp[k].GetCurrentSize(ps0).ToString("F2")+"]";}
            Debug.Log("FEASTPLAY_RENDER ren="+pr.enabled+" mat="+(pr.sharedMaterial?pr.sharedMaterial.name:"NULL")+" mask="+eye.cullingMask+" count="+nn+" emitter="+smoke.transform.position.ToString("F2")+" raw="+ws);
            yield return Prolog47VisualTests.Capture(eye,"47-rotten-feast-playmode");
            Object.DestroyImmediate(lamp.gameObject);
        }
        private IEnumerator DoorsVent47()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var p=d.player;
            Time.timeScale=1;UiFocus.Cinematic=false;
            foreach(var door in new[]{GameObject.Find("FrontSittingDoor").GetComponent<HingedDoor>(),d.upper.bathDoor})
            {
                door.locked=false;door.SetOpenImmediate(false);
                var renderers=door.GetComponentsInChildren<Renderer>();var count=renderers.Length;
                Assert.That(count,Is.GreaterThan(0));var position=door.hinge.position;
                door.Interact();yield return new WaitForSeconds(1.1f);
                Assert.That(door.IsOpen,Is.True);Assert.That(door.GetComponentsInChildren<Renderer>().Length,Is.EqualTo(count));
                Assert.That(renderers.All(r=>r&&r.enabled&&r.gameObject.activeInHierarchy),Is.True,"An opening door must keep its leaf visible");
                Assert.That(Vector3.Distance(position,door.hinge.position),Is.LessThan(.001f),"A swinging leaf must retain its hinge position");
                var point=door==d.upper.bathDoor?d.P(-1.1f,5.9f,3.83f):d.P(-1.4f,3.7f,.54f);
                d.PlacePlayer(point,Quaternion.identity);p.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.FirstPerson);
                var phone=SmartphoneController.Instance;phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();
                p.transform.rotation=Quaternion.LookRotation(Vector3.ProjectOnPlane(door.transform.position-p.transform.position,Vector3.up));p.SetViewPitch(0);p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
                yield return Prolog47VisualTests.Capture(d.phoneView.playerCamera,"21-open-"+door.name);
                door.Interact();yield return new WaitForSeconds(1.1f);Assert.That(door.IsOpen,Is.False);
            }
            d.upper.childDoor.SetOpenImmediate(false);d.upper.ventDoor.SetOpenImmediate(false);
            var device=SmartphoneController.Instance;device.arModuleUnlocked=true;device.isPhoneRaised=device.isScreenOn=true;device.currentScanMode=ScanVisualMode.Normal;device.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            d.PlacePlayer(d.P(-3,5.95f,3.83f),Quaternion.LookRotation(-d.north));p.MovementLocked=false;p.enabled=true;
            // The rusted cover pries open standing in normal camera — but only with the crowbar.
            Assert.That(d.upper.ventDoor.CanInteract(),Is.True,"Prying stance is just standing at the grille");
            p.enabled=false;var target=d.upper.ventDoor.GetComponent<Collider>().bounds.center;var delta=target-p.cameraPivot.position;var flat=Vector3.ProjectOnPlane(delta,Vector3.up);p.transform.rotation=Quaternion.LookRotation(flat);p.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            yield return null;yield return null;
            var eye=d.phoneView.playerCamera;Physics.SyncTransforms();
            yield return Prolog47VisualTests.Capture(eye,"24-vent-interaction-ray");
            var hits=Physics.RaycastAll(eye.transform.position,eye.transform.forward,3).OrderBy(h=>h.distance).Select(h=>h.collider.name+":"+h.distance.ToString("F3"));
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(d.upper.ventDoor),"Vent ray: "+string.Join(",",hits));
            yield return Press(Key.E);yield return new WaitForSeconds(1.1f);
            Assert.That(d.upper.ventDoor.IsOpen,Is.False,"Rusted vent stays shut without the crowbar");
            Assert.That(CampaignState.Instance.HasInventoryItem("EVENT_P0_VENT_PRIZED"),Is.False,"Grille is still on without the crowbar");
            Item(AtmajaAction.Crowbar).Interact();
            Assert.That(CampaignState.Instance.HasInventoryItem("ITEM_P0_CROWBAR"),Is.True,"Crowbar pickup registers");
            yield return Press(Key.E);yield return new WaitForSeconds(1.1f);
            // Designer Nov: prying rips the grille off for good — persisted,
            // never a door that swings open and shut again.
            Assert.That(CampaignState.Instance.HasInventoryItem("EVENT_P0_VENT_PRIZED"),Is.True,"Prying registers");
            Assert.That(d.upper.ventDoor.gameObject.activeInHierarchy,Is.False,"Prized grille is gone, not left swinging");
            // Crawling through still needs AR + crouch at the open grille.
            device.currentScanMode=ScanVisualMode.AR;device.SyncVisualState();
            yield return new WaitForSeconds(.3f);
            p.enabled=true;
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));yield return new WaitForSeconds(.6f);
            Assert.That(p.IsCrouching,Is.True);
            p.enabled=false;
            yield return new WaitForSeconds(.3f);
            Assert.That(d.upper.ventGate.enabled,Is.False);yield return Prolog47VisualTests.Capture(d.phoneView.playerCamera,"24-vent-open-ar-crouched");
            yield return WalkCapsule(p,d.P(-3,4.5f,3.83f));
            Assert.That(d.LocalPosition(p.transform.position).y,Is.LessThan(4.7f),"Raka physically passes through the opening");
            device.currentScanMode=ScanVisualMode.Normal;
            d.PlacePlayer(d.P(-5.20f,1.05f,3.83f),Quaternion.identity);
            var cup=d.upper.cupAim.position;var cupDirection=cup-p.cameraPivot.position;var cupFlat=Vector3.ProjectOnPlane(cupDirection,Vector3.up);
            p.transform.rotation=Quaternion.LookRotation(cupFlat);p.SetViewPitch(-Mathf.Atan2(cupDirection.y,cupFlat.magnitude)*Mathf.Rad2Deg);p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            yield return Prolog47VisualTests.Capture(eye,"33-crouched-cup-closeup");
        }
        [UnityTest] public IEnumerator Prolog47TrapdoorDescendsTheVisibleLadder()=>Run(Ladder47());
        [UnityTest] public IEnumerator Prolog47CellarClimbReturnsToMaster()=>Run(CellarClimb47());
        [UnityTest] public IEnumerator Prolog47CellarFloorRoutesStayWalkable()=>Run(CellarWalk47());
        private IEnumerator CellarWalk47()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var p=d.player;
            d.upper.RestoreWorld();
            CampaignState.Instance.AddInventoryItem("EVENT_P0_BASEMENT_ENTERED");
            d.basement.RestoreWorld();d.basement.nyonya.SetActive(false);
            d.SetStage(AtmajaStage.BasementEntry);Physics.SyncTransforms();
            var cc=p.GetComponent<CharacterController>();float r=cc.radius,h=cc.height;
            var blocked="";
            bool Seg(Vector3 a,Vector3 b,string name)
            {
                float len=Vector3.Distance(a,b);int n=Mathf.Max(1,Mathf.CeilToInt(len/.2f));bool ok=true;
                for(int i=0;i<=n;i++)
                {
                    Vector3 q=Vector3.Lerp(a,b,i/(float)n);
                    var hits=Physics.OverlapCapsule(q+Vector3.up*r,q+Vector3.up*(h-r),r,~0,QueryTriggerInteraction.Ignore).Where(c=>c!=cc&&!c.transform.IsChildOf(p.transform)).ToArray();
                    if(hits.Length>0){ok=false;blocked+=name+" @ "+q.ToString("F2")+" by "+string.Join(",",hits.Select(c=>c.name))+"; ";}
                }
                return ok;
            }
            Vector3 P(float x,float z)=>d.P(x,z,-2.79f);
            var tour=new(Vector3,Vector3,string)[]
            {
                (P(1.1f,1.5f),P(1.5f,3.2f),"ladder-mid"),(P(1.5f,3.2f),P(1.5f,5.4f),"mid-west-lane"),
                (P(1.5f,5.4f),P(2.2f,6.3f),"lane-spawn"),(P(2.2f,6.3f),P(3.2f,5.6f),"spawn-bench"),
                (P(3.2f,5.6f),P(1.7f,6.0f),"bench-mirror"),(P(1.7f,6.0f),P(1.5f,2.6f),"mirror-south"),
                (P(1.5f,2.6f),P(1.5f,1.35f),"south-drop"),(P(1.5f,1.35f),P(1.75f,1.35f),"south-lane"),
                (P(1.75f,1.35f),P(1.42f,2.8f),"west-exit1"),(P(1.42f,2.8f),P(1.2f,1.7f),"exit1-approach"),
                (P(1.2f,1.7f),P(1.5f,2.6f),"approach-mid"),(P(1.5f,2.6f),P(1.5f,3.8f),"mid-north"),
                (P(1.5f,3.8f),P(4.2f,3.8f),"mid-east"),(P(4.2f,3.8f),P(4.15f,1.7f),"east-drop"),
                (P(4.15f,1.7f),P(4.05f,1.05f),"ward2-exit2"),(P(4.05f,1.05f),P(4.15f,1.7f),"exit2-back"),
                (P(4.15f,1.7f),P(4.2f,3.8f),"north-return"),(P(4.2f,3.8f),P(1.5f,3.8f),"east-mid-return"),
                (P(1.5f,3.8f),P(1.1f,1.5f),"mid-ladder"),
                (P(2.2f,6.3f),P(2.5f,6.9f),"spawn-arch"),(P(2.5f,6.9f),P(2.5f,7.5f),"arch-flood"),(P(2.5f,7.5f),P(2.1f,5.45f),"arch-back"),
                (P(2.5f,7.5f),P(6.0f,7.7f),"flood-lane"),(P(6.0f,7.7f),P(6.0f,9.0f),"lane-gapS"),
                (P(6.0f,9.0f),P(7.5f,9.0f),"gapS-east"),(P(7.5f,9.0f),P(9.0f,9.2f),"east-lane"),
                (P(9.0f,9.2f),P(9.6f,8.3f),"lane-nook"),(P(9.6f,8.3f),P(9.5f,7.5f),"nook-cross"),
                (P(9.5f,7.5f),P(9.3f,6.2f),"cross-north"),(P(9.3f,6.2f),P(9.3f,6.0f),"cross-south"),
                (P(9.3f,6.0f),P(9.3f,5.8f),"north-room"),(P(9.3f,5.8f),P(7.0f,5.8f),"room-east"),
                (P(7.0f,5.8f),P(7.0f,4.5f),"room-gap"),(P(7.0f,4.5f),P(6.0f,4.5f),"gap-cross"),(P(6.0f,4.5f),P(6.0f,4.2f),"gap-south"),
                (P(6.0f,4.2f),P(5.0f,4.2f),"lane-crate"),(P(5.0f,4.2f),P(4.2f,4.2f),"lane-main"),
                (P(4.2f,3.8f),P(4.2f,4.2f),"main-return"),
                (P(4.2f,4.2f),P(5.0f,4.2f),"lane-main"),(P(5.0f,4.2f),P(6.0f,4.2f),"main-gap"),
                (P(6.0f,4.2f),P(6.0f,4.5f),"gap-approach"),(P(6.0f,4.5f),P(7.0f,4.5f),"gap-east"),
                (P(7.0f,4.5f),P(7.0f,5.8f),"east-north"),(P(7.0f,5.8f),P(9.3f,5.8f),"north-maze"),(P(9.3f,5.8f),P(10.5f,5.8f),"maze-room"),
                (P(10.5f,5.8f),P(10.5f,4.5f),"maze-drop"),(P(10.5f,4.5f),P(11.5f,4.5f),"drop-gap"),
                (P(11.5f,4.5f),P(12.5f,4.5f),"gap-r1"),(P(12.5f,4.5f),P(12.5f,6.5f),"r1-west"),
                (P(12.5f,6.5f),P(13.5f,6.5f),"r1-center"),(P(13.5f,6.5f),P(14.2f,6.5f),"center-warda"),
                (P(14.2f,6.5f),P(14.2f,4.4f),"warda-drop"),(P(14.2f,4.4f),P(13.5f,4.4f),"warda-back"),
                (P(13.5f,4.4f),P(12.5f,4.4f),"back-gap"),(P(12.5f,4.4f),P(12.5f,6.5f),"gap-north"),
                (P(12.5f,6.5f),P(13.5f,6.5f),"north-center"),(P(13.5f,6.5f),P(13.5f,8.0f),"center-south"),(P(13.5f,8.0f),P(13.5f,11.0f),"south-crypt"),
                (P(13.5f,11.0f),P(13.5f,12.5f),"crypt-gate"),(P(13.5f,12.5f),P(13.5f,13.5f),"gate-crypt"),
                (P(13.5f,13.5f),P(10.0f,13.5f),"crypt-west"),(P(10.0f,13.5f),P(9.5f,14.9f),"west-niche"),
                (P(9.5f,14.9f),P(10.5f,14.9f),"niche-back"),(P(10.5f,14.9f),P(13.0f,14.5f),"niche-east"),
                (P(13.0f,14.5f),P(14.0f,14.0f),"east-cam"),(P(14.0f,14.0f),P(13.5f,13.5f),"cam-back"),
                (P(13.5f,13.5f),P(13.5f,12.5f),"crypt-exit"),(P(13.5f,12.5f),P(13.5f,11.0f),"exit-r1"),
                (P(13.5f,11.0f),P(12.5f,11.0f),"r1-west2"),(P(12.5f,11.0f),P(12.5f,8.0f),"west-nook"),
                (P(12.5f,8.0f),P(12.5f,4.5f),"r1-drop"),(P(12.5f,4.5f),P(11.5f,4.5f),"drop-east"),
                (P(11.5f,4.5f),P(10.5f,4.5f),"east-maze"),(P(10.5f,4.5f),P(10.5f,5.8f),"maze-north"),
                (P(10.5f,5.8f),P(9.3f,5.8f),"north-west"),(P(9.3f,5.8f),P(9.5f,7.5f),"west-cross"),
                (P(9.5f,7.5f),P(9.5f,7.9f),"cross-south"),(P(9.5f,7.9f),P(7.0f,9.0f),"south-lane"),
                (P(7.0f,9.0f),P(6.0f,9.0f),"lane-gap"),(P(6.0f,9.0f),P(5.0f,9.0f),"gap-flood"),
                (P(5.0f,9.0f),P(3.5f,9.0f),"flood-mid"),(P(3.5f,9.0f),P(2.4f,9.2f),"mid-ward"),
                (P(2.4f,9.2f),P(2.4f,10.0f),"ward-hide"),(P(2.4f,10.0f),P(2.4f,9.2f),"hide-exit"),
                (P(2.4f,9.2f),P(3.5f,9.2f),"exit-mid"),(P(3.5f,9.2f),P(3.5f,7.5f),"mid-north"),
                (P(3.5f,7.5f),P(2.5f,7.5f),"north-arch"),(P(2.5f,7.5f),P(2.5f,6.9f),"arch-main"),
            };
            bool all=true;
            foreach(var s in tour)all=Seg(s.Item1,s.Item2,s.Item3)&&all;
            Debug.Log("CELLAR_SWEEP r="+r+" h="+h+" blocked=["+blocked+"]");
            Assert.That(all,Is.True,"Cellar floor routes walkable, blockers: "+blocked);
            // Damar's handycam hides in the crypt far corner: reachable and
            // visible from the crypt aisle, not from the main rooms.
            d.PlacePlayer(d.P(14.0f, 14.0f, -2.79f),Quaternion.identity);p.MovementLocked=false;p.enabled=false;
            p.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.FirstPerson);
            var camItem=Item(AtmajaAction.Handycam);
            var cdelta=camItem.GetComponent<Collider>().bounds.center-p.cameraPivot.position;var cflat=Vector3.ProjectOnPlane(cdelta,Vector3.up);
            p.transform.rotation=Quaternion.LookRotation(cflat);p.SetViewPitch(-Mathf.Atan2(cdelta.y,cflat.magnitude)*Mathf.Rad2Deg);p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            yield return null;yield return null;Physics.SyncTransforms();
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(camItem),"Hidden handycam is framed from the crypt aisle");
            // Each hiding locker conceals Raka and lets him back out.
            foreach(var spot in new[]{(14.2f,4.4f,"MazeWardrobeA"),(9.5f,15.8f,"MazeWardrobeB"),(1.9f,10.0f,"MazeWardrobeC")})
            {
                d.PlacePlayer(d.P(spot.Item1,spot.Item2,-2.79f),Quaternion.identity);
                var ward=GameObject.Find(spot.Item3).GetComponent<AtmajaItem>();
                Assert.That(ward,Is.Not.Null,spot.Item3+" stands");
                ward.Interact();yield return null;
                Assert.That(d.basement.IsHidden,Is.True,spot.Item3+" conceals Raka");
                yield return new WaitForSeconds(.4f);
                yield return Press(Key.E);yield return null;
                Assert.That(d.basement.IsHidden,Is.False,"Raka leaves "+spot.Item3);
                yield return new WaitForSeconds(.9f);
            }
            // Damar's bag opens its note.
            d.PlacePlayer(d.P(7.0f, 4.2f, -2.79f),Quaternion.identity);
            var bagItem=GameObject.Find("DamarBagProp").GetComponent<AtmajaItem>();
            Assert.That(bagItem,Is.Not.Null,"Damar's bag waits at the maze mouth");
            bagItem.Interact();yield return null;
            Assert.That(HudController.Instance.documentPanel.gameObject.activeSelf,Is.True,"Bag note opens");
            HudController.Instance.documentCloseButton.onClick.Invoke();yield return null;
        }
        private IEnumerator CellarClimb47()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var p=d.player;var c=CampaignState.Instance;
            foreach(var flag in new[]{"EVENT_P0_CHAIN1_BROKEN","EVENT_P0_CHAIN2_BROKEN","EVENT_P0_CHAIN3_BROKEN","EVENT_P0_CARPET_LIFTED","EVENT_P0_MASTER_OPEN","EVENT_P0_TRAPDOOR_OPEN","EVENT_P0_BASEMENT_ENTERED"})c.AddInventoryItem(flag);
            d.upper.RestoreWorld();d.basement.RestoreWorld();d.basement.nyonya.SetActive(false);
            d.SetStage(AtmajaStage.BasementEntry);
            var climb=Item(AtmajaAction.CellarClimb);
            Assert.That(climb,Is.Not.Null,"Iron grab-handle stands at the ladder base");
            // Designer Nov: the basement needs a way back up outside the
            // chase — the handle must be reachable on foot and climb Raka
            // back to the master bedroom through the visible shaft.
            d.PlacePlayer(d.P(1.6f,1.6f,-2.79f),Quaternion.identity);p.MovementLocked=false;p.enabled=false;
            p.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.FirstPerson);
            var target=climb.GetComponent<Collider>().bounds.center;var delta=target-p.cameraPivot.position;var flat=Vector3.ProjectOnPlane(delta,Vector3.up);
            p.transform.rotation=Quaternion.LookRotation(flat);p.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            yield return null;yield return null;Physics.SyncTransforms();
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(climb),"Grab-handle is visible and reachable from the cellar floor");
            yield return Press(Key.E);
            Assert.That(d.Busy,Is.True,"Grabbing the handle starts the climb");
            float end=Time.time+25;
            while(d.Busy&&Time.time<end)yield return null;
            Assert.That(d.Busy,Is.False,"Climb finishes");
            Assert.That(Vector3.Distance(p.transform.position,d.upper.trapdoorTop.position),Is.LessThan(.3f),"Raka climbs back to the master bedroom");
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.MasterOpen));
            Assert.That(p.GetComponent<CharacterController>().enabled,Is.True);
            Assert.That(p.MovementLocked,Is.False);
        }
        private IEnumerator Ladder47()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var p=d.player;var c=CampaignState.Instance;
            foreach(var flag in new[]{"EVENT_P0_CHAIN1_BROKEN","EVENT_P0_CHAIN2_BROKEN","EVENT_P0_CHAIN3_BROKEN","EVENT_P0_CARPET_LIFTED","EVENT_P0_MASTER_OPEN"})c.AddInventoryItem(flag);
            d.upper.RestoreWorld();d.SetStage(AtmajaStage.MasterOpen);d.PlacePlayer(d.upper.trapdoorTop.position,d.upper.trapdoorTop.rotation);
            var phone=SmartphoneController.Instance;phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();
            Assert.That(d.upper.CellarDescentPath().Length,Is.GreaterThan(5));
            Item(AtmajaAction.Trapdoor).Interact();Assert.That(d.Busy,Is.True);
            float previous=p.transform.position.y;bool captured=false;float end=Time.time+18;
            while(d.Busy&&Time.time<end)
            {
                if(p.transform.position.y<2.5f&&p.transform.position.y>-.5f)
                {
                    Assert.That(d.LocalPosition(p.transform.position).x,Is.InRange(.95f,1.10f));
                    Assert.That(Vector3.Dot(p.transform.forward,-d.north),Is.GreaterThan(.85f),"Face the visible rungs during descent");
                    if(!captured){captured=true;yield return Prolog47VisualTests.Capture(d.phoneView.playerCamera,"43-actual-ladder-descent");}
                }
                previous=p.transform.position.y;yield return null;
            }
            Assert.That(captured,Is.True);Assert.That(d.Busy,Is.False);Assert.That(Vector3.Distance(p.transform.position,d.basement.spawnBasement.position),Is.LessThan(.12f));
            Assert.That(p.GetComponent<CharacterController>().enabled,Is.True);
        }
        private IEnumerator LiveAndAR47()
        {
            yield return new WaitForSeconds(2);var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;
            Assert.That(CampaignState.Instance.HasInventoryItem("EVENT_P0_FRONT_DOOR_TRIED"),Is.False);
            phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);yield return null;
            Assert.That(d.phoneView.LiveButton.interactable,Is.True);d.phoneView.LiveButton.onClick.Invoke();yield return null;
            Assert.That(phone.isBroadcastConnected&&d.LiveConnected,Is.True,"Live starts on the actual phone button before front-door interaction");
            phone.arModuleUnlocked=true;phone.currentScanMode=ScanVisualMode.AR;yield return null;
            yield return Press(Key.Z);
            Assert.That(d.phoneView.Apps.CurrentApp,Is.EqualTo(PhoneApp.Home));Assert.That(phone.currentScanMode,Is.EqualTo(ScanVisualMode.Normal));
            yield return Prolog47VisualTests.Capture(d.phoneView.playerCamera,"19-home-ar-off");
            phone.currentScanMode=ScanVisualMode.AR;d.phoneView.Apps.Open(PhoneApp.Camera);d.phoneView.Apps.Open(PhoneApp.Map);
            Assert.That(phone.currentScanMode,Is.EqualTo(ScanVisualMode.Normal),"Every app departure clears AR, including direct Map");
        }
        private IEnumerator PhoneDrag47()
        {
            yield return new WaitForSeconds(2);Time.timeScale=1;var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;
            phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Home);
            d.player.SendMessage("OnApplicationFocus",true);yield return null;yield return null;
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(100,300)});yield return null;
            var before=d.player.transform.rotation;
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(100,300),buttons=1});yield return null;
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(150,300),delta=new Vector2(50,0),buttons=1});yield return null;yield return null;
            Assert.That(Quaternion.Angle(before,d.player.transform.rotation),Is.GreaterThan(1),"Holding left mouse in the world area rotates the view while Home owns the pointer");
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(150,300)});yield return null;var stopped=d.player.transform.rotation;
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(200,300),delta=new Vector2(50,0)});yield return null;yield return null;
            Assert.That(Quaternion.Angle(stopped,d.player.transform.rotation),Is.LessThan(.1f),"Ordinary menu-pointer motion must not rotate the view after release");
            Assert.That(d.phoneView.Apps.CurrentApp,Is.EqualTo(PhoneApp.Home));Assert.That(Cursor.lockState,Is.EqualTo(CursorLockMode.None));
            yield return Prolog47VisualTests.Capture(d.phoneView.playerCamera,"14-phone-menu-after-left-drag");
        }
        private IEnumerator PursuitBlitz47()
        {
            yield return new WaitForSeconds(2);Time.timeScale=1;var d=SeedBasement();var phone=SmartphoneController.Instance;
            foreach(var id in new[]{"EVENT_P0_BASEMENT_ENTERED","EVENT_P0_POWER_GLITCH","EVENT_P0_BASEMENT_SAWER","EVENT_P0_POWERBANK_TAKEN"})CampaignState.Instance.AddInventoryItem(id);
            d.chase.StartChase();yield return new WaitForSeconds(1.8f);
            d.PlacePlayer(d.P(1.4f,4.15f,-2.79f),Quaternion.LookRotation(d.east));
            d.chase.chaserFigure.transform.position=d.P(4.0f,4.15f,-2.79f);
            phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.batteryPercent=80;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            var rig=d.player.GetComponent<ShoulderCameraRig>();rig.SetPerspective(PlayerPerspective.FirstPerson);
            var eye=d.phoneView.playerCamera;var delta=d.chase.chaserFigure.transform.position+Vector3.up*1.3f-d.player.cameraPivot.position;
            var flat=Vector3.ProjectOnPlane(delta,Vector3.up);d.player.transform.rotation=Quaternion.LookRotation(flat);d.player.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);rig.RefreshCameraPose();yield return null;yield return null;
            Assert.That(d.chase.IsPursuing,Is.True);
            InputSystem.QueueStateEvent(mouse,new MouseState{buttons=1});yield return null;InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;
            Assert.That(d.chase.IsStunned,Is.True);Assert.That(phone.batteryPercent,Is.InRange(64.7f,65.1f));
            var hunter=d.chase.chaserFigure.transform.position;yield return new WaitForSeconds(.5f);
            Assert.That(Vector3.Distance(hunter,d.chase.chaserFigure.transform.position),Is.LessThan(.02f));Assert.That(d.chase.CanCatchPlayer(),Is.False);
            Assert.That(d.chase.PhotosTaken,Is.Zero,"Emergency pursuit blitz does not spend the final window ritual");
            yield return Prolog47VisualTests.Capture(eye,"41-after-handycam-blitz");
            yield return new WaitForSeconds(1.6f);Assert.That(d.chase.IsStunned,Is.False,"The timed stun ends");
        }
        private IEnumerator Window47()
        {
            yield return new WaitForSeconds(2);Time.timeScale=1;var d=SeedBasement();
            d.chase.StartChase();yield return new WaitForSeconds(1.8f);
            var window=d.chase.windowBlockSpot;
            d.PlacePlayer(d.vaultInside.position,d.vaultInside.rotation);
            Vector3 original=d.player.transform.position;
            typeof(AtmajaChaseSequence).GetMethod("BeginStandoff",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(d.chase,null);
            Assert.That(Vector3.Distance(d.chase.chaserFigure.transform.position,window.position),Is.LessThan(.01f));
            yield return null;yield return null;
            Assert.That(Vector3.Distance(original,d.player.transform.position),Is.LessThan(.015f),"Interception never displaces Raka");
            Assert.That(d.chase.Objective(),Is.Empty);
            // Review the same encounter from its usual approach distance inside the kitchen.
            d.PlacePlayer(d.vaultInside.position-d.east*1.7f,Quaternion.LookRotation(d.east));original=d.player.transform.position;
            d.player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            float initial=Vector3.Distance(d.chase.chaserFigure.transform.position,original);
            yield return Prolog47VisualTests.Capture(d.phoneView.playerCamera,"44-window-interception");
            yield return new WaitForSecondsRealtime(1.5f);
            Assert.That(Vector3.Distance(d.chase.chaserFigure.transform.position,original),Is.LessThan(initial-.15f),"Nyonya slowly closes the distance instead of waiting at the window");
            Assert.That(Vector3.Distance(original,d.player.transform.position),Is.LessThan(.015f));
            Assert.That(Object.FindObjectsByType<TextMesh>(FindObjectsSortMode.None).Any(t=>t.text.Contains("HADAPI DIA")||t.text.Contains("TEROBOS JENDELA")),Is.False);
            yield return Prolog47VisualTests.Capture(d.phoneView.playerCamera,"46-window-slow-approach");
        }
    }
}
#endif
