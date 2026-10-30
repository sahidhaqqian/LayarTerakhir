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
            Assert.That(d.upper.ventDoor.CanInteract(),Is.False,"Standing in normal camera cannot open the vent");
            device.currentScanMode=ScanVisualMode.AR;
            Assert.That(d.upper.ventDoor.CanInteract(),Is.False,"AR still requires crouching");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));yield return new WaitForSeconds(.6f);
            Assert.That(p.IsCrouching,Is.True);Assert.That(d.upper.ventDoor.CanInteract(),Is.True);
            p.enabled=false;var target=d.upper.ventDoor.GetComponent<Collider>().bounds.center;var delta=target-p.cameraPivot.position;var flat=Vector3.ProjectOnPlane(delta,Vector3.up);p.transform.rotation=Quaternion.LookRotation(flat);p.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);p.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
            yield return null;yield return null;
            var eye=d.phoneView.playerCamera;Physics.SyncTransforms();
            yield return Prolog47VisualTests.Capture(eye,"24-vent-interaction-ray");
            var hits=Physics.RaycastAll(eye.transform.position,eye.transform.forward,3).OrderBy(h=>h.distance).Select(h=>h.collider.name+":"+h.distance.ToString("F3"));
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(d.upper.ventDoor),"Vent ray: "+string.Join(",",hits));
            yield return Press(Key.E);yield return new WaitForSeconds(1.1f);Assert.That(d.upper.ventDoor.IsOpen,Is.True);
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
