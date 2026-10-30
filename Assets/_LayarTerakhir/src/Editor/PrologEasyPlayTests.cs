#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class PrologEasyPlayTests
    {
        private Keyboard keys;private Mouse mouse;
        private const string Folder="Audit/Visuals/PrologEasy-2026-10-30";
        [UnitySetUp] public IEnumerator SetUp()
        {
            SessionState.SetString("Easy.PreviousSave",Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")??"");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",Path.Combine(Path.GetTempPath(),"PrologEasy-"+Guid.NewGuid()));
            RecoveryTestInputSettings.Prepare("Easy.Input");
            EditorSceneManager.OpenScene(TestContext.CurrentContext.Test.Name.StartsWith("Kos")?PrologKosSceneBuilder.ScenePath:AtmajaSceneBuilder.ScenePath);
            yield return new EnterPlayMode();
            var settings=Object.Instantiate(InputSystem.settings);settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;InputSystem.settings=settings;
            keys=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();Time.timeScale=5;
            yield return new WaitForSeconds(2);
            Object.FindFirstObjectByType<FirstPersonController>().SendMessage("OnApplicationFocus",true);
            Assert.That(SystemInfo.graphicsDeviceType,Is.Not.EqualTo(GraphicsDeviceType.Null),"These acceptance checks require actual rendering");
        }
        [UnityTearDown] public IEnumerator TearDown()
        {
            if(keys!=null)InputSystem.RemoveDevice(keys);if(mouse!=null)InputSystem.RemoveDevice(mouse);
            RecoveryTestInputSettings.Restore("Easy.Input");Time.timeScale=1;yield return new ExitPlayMode();RecoveryTestInputSettings.RestoreEditorConfiguration("Easy.Input");
            var directory=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",SessionState.GetString("Easy.PreviousSave",""));
            if(!string.IsNullOrEmpty(directory)&&Directory.Exists(directory))Directory.Delete(directory,true);
            UiFocus.ApplicationFocused=true;UiFocus.Menu=UiFocus.Clock=UiFocus.Phone=UiFocus.PhoneAiming=UiFocus.Cinematic=false;EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
        [UnityTest] public IEnumerator KosSecondDamarCallImmediatelyOpensMessages()=>RecoveryRuntimeScenario.Run(SecondCall());
        [UnityTest] public IEnumerator KosFanSignsBathroomIconsAndYardReview()=>RecoveryRuntimeScenario.Run(KosVisuals());
        [UnityTest] public IEnumerator LiveBeforeFrontDoorAndHomeDisablesAR()=>RecoveryRuntimeScenario.Run(LiveAndAR());
        [UnityTest] public IEnumerator PhoneMenuLeftDragAndRelease()=>RecoveryRuntimeScenario.Run(Drag());
        [UnityTest] public IEnumerator UpperHeadersPhotographLampAndChatReview()=>RecoveryRuntimeScenario.Run(AtmajaVisuals());
        [UnityTest] public IEnumerator WardrobeMeterFollowsDistanceAndHidesOnExit()=>RecoveryRuntimeScenario.Run(Meter());
        [UnityTest] public IEnumerator WindowHasNoInstructionsAndStillAcceptsPhotosAndSpace()=>RecoveryRuntimeScenario.Run(Window());
        private IEnumerator Press(Key key){InputSystem.QueueStateEvent(keys,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keys,new KeyboardState());yield return null;}
        private static AtmajaDirector Director=>Object.FindFirstObjectByType<AtmajaDirector>();
        private IEnumerator SecondCall()
        {
            var d=Object.FindFirstObjectByType<KamarKosPrologDirector>();d.StopAllCoroutines();UiFocus.Cinematic=false;
            if(HudController.Instance.IsModalOpen)HudController.Instance.CloseDocumentModal();
            d.currentStage=KosPrologStage.WaitPickupSmartphone;d.smartphoneBedItem.isInteractable=true;d.smartphoneBedItem.gameObject.SetActive(true);
            var player=Object.FindFirstObjectByType<FirstPersonController>();Place(player,new Vector3(.5f,.02f,-.6f),Vector3.forward*2+Vector3.up);
            d.smartphoneBedItem.Interact();yield return null;
            var view=Object.FindFirstObjectByType<AtmajaPhoneView>();Assert.That(view.Apps.CallActive,Is.True,"Real phone pickup starts the second Damar call");
            yield return new WaitForSeconds(.8f);
            yield return Capture(view.playerCamera,"02-call-hand-reference",()=>{view.playerCamera.transform.position=player.transform.position+Vector3.up*1.55f+player.transform.forward*1.2f+player.transform.right*.75f;view.playerCamera.transform.LookAt(player.transform.position+Vector3.up*1.45f);});
            float deadline=Time.realtimeSinceStartup+35;while(view.Apps.CallActive&&Time.realtimeSinceStartup<deadline)yield return null;
            Assert.That(view.Apps.CallActive,Is.False);Assert.That(view.Apps.CurrentApp,Is.EqualTo(PhoneApp.Messages));
            Assert.That(SmartphoneController.Instance.isPhoneRaised&&SmartphoneController.Instance.isScreenOn,Is.True);
            Assert.That(CampaignState.Instance.Data.wallet_bound,Is.EqualTo(5000000));
            Assert.That(GameObject.Find("PhoneNotification"),Is.Not.Null,"A visible handset notification accompanies the message transition");
            yield return Capture(view.playerCamera,"02-messages-immediately-after-call");
        }
        private IEnumerator KosVisuals()
        {
            var d=Object.FindFirstObjectByType<KamarKosPrologDirector>();d.StopAllCoroutines();UiFocus.Cinematic=false;
            var player=Object.FindFirstObjectByType<FirstPersonController>();var rig=player.GetComponent<ShoulderCameraRig>();rig.SetPerspective(PlayerPerspective.FirstPerson);
            var eye=player.GetComponent<InteractionInteractor>().playerCamera;
            Place(player,new Vector3(.3f,.02f,-.65f),KosVisualV2.BoundsOf(GameObject.Find("KosDeskFan")).center);yield return new WaitForSecondsRealtime(.3f);yield return Capture(eye,"01-fan-facing-room");
            yield return SceneManager.LoadSceneAsync(KosCorridorBuilder.ScenePath);yield return new WaitForSeconds(2);
            var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();player=corridor.player;eye=player.GetComponent<InteractionInteractor>().playerCamera;
            player.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.FirstPerson);
            Assert.That(GameObject.Find("FloorTwoWayfinding"),Is.Null);Assert.That(GameObject.Find("GroundWayfinding"),Is.Null);
            IEnumerator Shot(string name,Vector3 feet,Vector3 target)
            {Place(player,corridor.BuildingPoint(feet),corridor.BuildingPoint(target));yield return null;yield return null;yield return Capture(eye,name);}
            yield return Shot("05-no-floor-two-label",new Vector3(0,3.31f,11.2f),new Vector3(-1.44f,4.95f,12.15f));
            yield return Shot("06-no-ground-label",new Vector3(0,.04f,10.5f),new Vector3(2.54f,1.85f,12.05f));
            var bath=GameObject.Find("BathroomWANITADoor").GetComponent<HingedDoor>();
            var icon=GameObject.Find("BathroomWANITAPictogram");Assert.That(icon.transform.IsChildOf(bath.transform),Is.True);
            yield return Shot("08-bathroom-symbols-closed",new Vector3(2.05f,.04f,4.25f),new Vector3(2.82f,1.55f,4.45f));
            yield return Shot("08-male-bathroom-symbol",new Vector3(2.05f,.04f,3.3f),new Vector3(2.82f,1.5f,3.3f));
            var old=icon.transform.position;bath.Interact();yield return new WaitForSeconds(1.2f);
            Assert.That(Vector3.Distance(old,icon.transform.position),Is.GreaterThan(.2f),"The symbol follows the opening leaf");
            Assert.That(GameObject.Find("BathroomWANITASign"),Is.Null);Assert.That(GameObject.Find("BathroomPRIASign"),Is.Null);
            yield return Shot("08-bathroom-symbol-follows-door",new Vector3(2.05f,.04f,4.25f),new Vector3(3.1f,1.45f,5.2f));
            var yard=GameObject.Find("KosYard").GetComponent<Renderer>().bounds;var street=GameObject.Find("KosStreet").GetComponent<Renderer>().bounds;
            Assert.That(Mathf.Min(yard.max.z,street.max.z)-Mathf.Max(yard.min.z,street.min.z),Is.LessThan(.001f),"Coplanar ground surfaces meet without overlap");
            yield return Shot("12-yard-street-seam",new Vector3(0,.04f,-6.3f),new Vector3(0,0,-9.0f));
        }
        private IEnumerator LiveAndAR()
        {
            var d=Director;var phone=SmartphoneController.Instance;
            Assert.That(CampaignState.Instance.HasInventoryItem("EVENT_P0_FRONT_DOOR_TRIED"),Is.False);
            phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);yield return null;
            Assert.That(d.phoneView.LiveButton.interactable,Is.True);d.phoneView.LiveButton.onClick.Invoke();yield return null;
            Assert.That(phone.isBroadcastConnected&&d.LiveConnected,Is.True);
            yield return Capture(d.phoneView.playerCamera,"15-live-before-front-door");
            phone.arModuleUnlocked=true;phone.currentScanMode=ScanVisualMode.AR;yield return null;yield return Press(Key.Z);
            Assert.That(d.phoneView.Apps.CurrentApp,Is.EqualTo(PhoneApp.Home));Assert.That(phone.currentScanMode,Is.EqualTo(ScanVisualMode.Normal));
            yield return Capture(d.phoneView.playerCamera,"19-home-clears-ar");
            phone.currentScanMode=ScanVisualMode.AR;d.phoneView.Apps.Open(PhoneApp.Camera);d.phoneView.Apps.Open(PhoneApp.Map);yield return null;
            Assert.That(phone.currentScanMode,Is.EqualTo(ScanVisualMode.Normal));Assert.That(GameObject.Find("NeighbourhoodMap").GetComponent<CanvasRenderer>(),Is.Not.Null);
            yield return Capture(d.phoneView.playerCamera,"19-map-clears-ar");
            Place(d.player,d.P(7.2f,10.975f,.04f),d.P(5.8f,10.975f,1.4f));d.player.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.FirstPerson);
            d.phoneView.Apps.Open(PhoneApp.Camera);d.player.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.OverShoulder);yield return new WaitForSecondsRealtime(.4f);
            var window=Object.FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).Single(i=>i.action==AtmajaAction.Window);
            Assert.That(window.GetInteractionPrompt(),Is.Empty);window.Interact();yield return null;
            Assert.That(HudController.Instance.interactionPromptText.text,Does.Not.Contain("Jendela").IgnoreCase);
            yield return Capture(d.phoneView.playerCamera,"16-window-glint-without-key-prompt");
        }
        private IEnumerator Drag()
        {
            Time.timeScale=1;var d=Director;var phone=SmartphoneController.Instance;
            phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Home);yield return null;yield return null;
            yield return new WaitForSecondsRealtime(.4f);
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(100,300)});yield return null;var before=d.player.transform.rotation;
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(100,300),buttons=1});yield return null;
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(150,300),delta=new Vector2(50,0),buttons=1});yield return null;yield return null;
            Assert.That(Quaternion.Angle(before,d.player.transform.rotation),Is.GreaterThan(1));
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(150,300)});yield return null;var stopped=d.player.transform.rotation;
            InputSystem.QueueStateEvent(mouse,new MouseState{position=new Vector2(200,300),delta=new Vector2(50,0)});yield return null;yield return null;
            Assert.That(Quaternion.Angle(stopped,d.player.transform.rotation),Is.LessThan(.1f));Assert.That(Cursor.lockState,Is.EqualTo(CursorLockMode.None));
            yield return Capture(d.phoneView.playerCamera,"14-phone-home-after-left-drag");
        }
        private IEnumerator AtmajaVisuals()
        {
            var d=Director;var phone=SmartphoneController.Instance;
            phone.hasSmartphoneInInventory=phone.arModuleUnlocked=phone.isPhoneRaised=phone.isScreenOn=true;phone.batteryPercent=90;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            var player=d.player;player.GetComponent<ShoulderCameraRig>().SetPerspective(PlayerPerspective.OverShoulder);
            IEnumerator Shot(string name,Vector3 feet,Vector3 target)
            {Place(player,feet,target);yield return new WaitForSecondsRealtime(.35f);yield return Capture(d.phoneView.playerCamera,name);}
            yield return Shot("18-ground-door-header",d.P(-1.4f,3.7f,.54f),d.P(-1.4f,2.5f,2.95f));
            yield return Shot("23-child-and-bath-headers",d.P(-3.1f,5.95f,3.83f),d.P(-3.0f,5.2f,6.05f));
            yield return Shot("23-child-door-upper-seal",d.P(-4.4f,5.85f,3.83f),d.P(-4.4f,5.2f,5.8f));
            yield return Shot("23-bath-door-upper-seal",d.P(-1.1f,5.85f,3.83f),d.P(-1.1f,5.2f,5.75f));
            yield return Shot("25-single-family-photograph",d.P(-2.05f,6.1f,3.83f),d.P(-2.05f,5.31f,5.35f));
            Assert.That(GameObject.Find("PortraitDosa1"),Is.Not.Null);Assert.That(GameObject.Find("PortraitDosa2"),Is.Null);Assert.That(GameObject.Find("PortraitDosa3"),Is.Null);
            phone.currentScanMode=ScanVisualMode.AR;yield return Shot("26-27-ar-photograph-without-labels",d.P(-2.05f,6.1f,3.83f),d.P(-2.05f,5.31f,5.35f));
            Assert.That(Object.FindObjectsByType<TextMesh>(FindObjectsInactive.Include,FindObjectsSortMode.None).Any(t=>t.text.Contains("DOSA PERTAMA")||t.text.Contains("DOSA KEDUA")||t.text.Contains("DOSA KETIGA")||t.text.Contains("DIA MELIHAT")),Is.False);
            phone.currentScanMode=ScanVisualMode.Normal;
            yield return Shot("30-desk-lamp-facing-desk",d.P(-5.3f,1.8f,3.83f),d.P(-5.3f,.55f,4.65f));
            var factory=typeof(AtmajaPhoneView).GetMethod("RoomComments",BindingFlags.NonPublic|BindingFlags.Static);
            var comments=(string[])factory.Invoke(null,new object[]{"child"});
            Assert.That(comments.Any(s=>s.ToLowerInvariant().Contains("mainan")||s.ToLowerInvariant().Contains("boneka")),Is.False);
            phone.isBroadcastConnected=true;d.SetStage(AtmajaStage.EvidenceHunt);yield return new WaitForSeconds(4);
            yield return Capture(d.phoneView.playerCamera,"28-child-room-live-chat");
            yield return Shot("38-no-floating-block-by-chandelier",d.P(3.4f,5.75f,3.83f),d.P(4.4f,5.2f,6.35f));
        }
        private IEnumerator Meter()
        {
            var d=Director;var c=CampaignState.Instance;var phone=SmartphoneController.Instance;
            foreach(var id in new[]{"SMARTPHONE_RAKA","MATABATIN_APK","EVENT_P0_GATE_OPEN","EVENT_P0_LIVE_STARTED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN","EVENT_P0_STAIRS_UNLOCKED","EVENT_P0_UPPER_REACHED","EVENT_P0_CHAIN1_BROKEN","EVENT_P0_CHAIN2_BROKEN","EVENT_P0_CHAIN3_BROKEN","EVENT_P0_MASTER_OPEN","EVENT_P0_CARPET_LIFTED","EVENT_P0_TRAPDOOR_OPEN","EVENT_P0_BASEMENT_ENTERED","EVENT_P0_POWER_GLITCH","EVENT_P0_BASEMENT_SAWER","EVENT_P0_POWERBANK_TAKEN"})c.AddInventoryItem(id);
            c.Data.battery=80;Assert.That(c.SaveCheckpointAtomic("PROLOG_BASEMENT_ENTRY"),Is.True);d.RestoreWorld();
            var b=d.basement;d.PlacePlayer(b.exitAnchor1.position,b.exitAnchor1.rotation);d.player.enabled=false;
            phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();b.wardrobeCol1.GetComponent<AtmajaItem>().Interact();yield return null;
            Assert.That(b.IsHidden,Is.True);yield return Press(Key.F);Assert.That(phone.isScreenOn,Is.False);
            b.enabled=false;Time.timeScale=1;
            var tick=typeof(AtmajaBasementHorror).GetMethod("HiddenTick",BindingFlags.Instance|BindingFlags.NonPublic);
            foreach(var tier in new[]{(12f,4,"safe-green"),(6f,8,"medium-green-yellow"),(2.55f,12,"near-all-colors")})
            {
                b.nyonya.transform.position=b.hideAnchor1.position+d.east*tier.Item1;tick.Invoke(b,new object[]{0f});
                var root=GameObject.Find("PrologThreatMeter");Assert.That(root,Is.Not.Null);
                var segments=root.GetComponentsInChildren<Image>();Assert.That(segments.Count(i=>i.color.maxColorComponent>.5f),Is.EqualTo(tier.Item2));
                Assert.That(root.GetComponent<RectTransform>().sizeDelta.x,Is.LessThanOrEqualTo(30));
                yield return Capture(d.phoneView.playerCamera,"40-"+tier.Item3);
            }
            b.enabled=true;yield return Press(Key.E);Assert.That(b.IsHidden,Is.False);Assert.That(GameObject.Find("PrologThreatMeter"),Is.Null,"Meter hides when the player leaves");
        }
        private IEnumerator Window()
        {
            Time.timeScale=1;var d=Director;var phone=SmartphoneController.Instance;
            CampaignState.Instance.AddInventoryItem("EVENT_P0_LIVE_STARTED");phone.isBroadcastConnected=true;phone.batteryPercent=80;
            d.chase.StartChase();yield return new WaitForSeconds(1.8f);d.player.enabled=false;
            d.PlacePlayer(d.vaultInside.position-d.east*1.5f,d.vaultInside.rotation);
            typeof(AtmajaChaseSequence).GetMethod("BeginStandoff",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(d.chase,null);
            yield return new WaitForSecondsRealtime(2.7f);
            Assert.That(d.chase.AwaitingPhotos,Is.True);Assert.That(d.chase.Objective(),Is.Empty);
            AssertNoWindowInstructions();yield return Capture(d.phoneView.playerCamera,"45-window-without-face-her-prompt");
            for(int i=0;i<3;i++)
            {d.phoneView.FeedCamera.transform.LookAt(d.chase.chaserFigure.transform.position+Vector3.up*1.45f);Assert.That(d.chase.TryPhoto(),Is.True);yield return new WaitForSecondsRealtime(.35f);}
            yield return new WaitForSecondsRealtime(1.5f);Assert.That(d.chase.ThreatDefeated,Is.True);Assert.That(d.chase.Objective(),Is.Empty);AssertNoWindowInstructions();
            yield return Capture(d.phoneView.playerCamera,"45-window-without-escape-key-prompt");
        }
        private static void AssertNoWindowInstructions()=>Assert.That(Object.FindObjectsByType<Text>(FindObjectsSortMode.None).Any(t=>t.gameObject.activeInHierarchy&&(t.text.ToUpperInvariant().Contains("HADAPI DIA")||t.text.ToUpperInvariant().Contains("TEROBOS JENDELA"))),Is.False);
        private static void Place(FirstPersonController player,Vector3 feet,Vector3 target)
        {
            var cc=player.GetComponent<CharacterController>();cc.enabled=false;player.transform.position=feet;cc.enabled=true;player.MovementLocked=true;
            Vector3 direction=target-player.cameraPivot.position;var flat=direction;flat.y=0;if(flat.sqrMagnitude>.001f)player.transform.rotation=Quaternion.LookRotation(flat);
            player.SetViewPitch(-Mathf.Atan2(direction.y,flat.magnitude)*Mathf.Rad2Deg);player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
        private static IEnumerator Capture(Camera eye,string name,Action pose=null)
        {
            yield return RecoveryVisualEvidence.AfterPresentation(()=>
            {
                var pos=eye.transform.position;var rot=eye.transform.rotation;
                var overlays=Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(c=>c.isRootCanvas&&c.renderMode==RenderMode.ScreenSpaceOverlay&&c.gameObject.activeInHierarchy).ToArray();
                try
                {
                    pose?.Invoke();Canvas.ForceUpdateCanvases();
                    foreach(var camera in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))if(camera!=eye&&camera.enabled&&camera.targetTexture)RenderPipeline.SubmitRenderRequest(camera,new RenderPipeline.StandardRequest{destination=camera.targetTexture});
                    foreach(var canvas in overlays){canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=eye;canvas.planeDistance=eye.nearClipPlane+.02f;}
                    Canvas.ForceUpdateCanvases();var metric=KosRenderDiagnosis.Capture(eye,name,false,Folder);
                    File.WriteAllText(Folder+"/render-context.json",JsonUtility.ToJson(new RenderContext{utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,windowsConfirmed=false,posedGameplayCamera=true,width=1280,height=720},true));
                }
                finally{foreach(var canvas in overlays){canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.worldCamera=null;}eye.transform.SetPositionAndRotation(pos,rot);Canvas.ForceUpdateCanvases();}
            });
        }
        [Serializable] private class RenderContext{public string utc,platform,gpu;public bool windowsConfirmed,posedGameplayCamera;public int width,height;}
    }
}
#endif
