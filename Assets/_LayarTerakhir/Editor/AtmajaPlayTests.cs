#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class AtmajaPlayTests
    {
        private Keyboard keyboard;
        [UnitySetUp] public IEnumerator SetUp()
        {
            SessionState.SetString("AtmajaAudit.PreviousSave",Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")??"");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",Path.Combine(Path.GetTempPath(),"AtmajaPlayTest-"+Guid.NewGuid()));
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            yield return new EnterPlayMode();
            SessionState.SetString("AtmajaAudit.Input",JsonUtility.ToJson(InputSystem.settings));
            var settings=Object.Instantiate(InputSystem.settings);
            settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            InputSystem.settings=settings;keyboard=InputSystem.AddDevice<Keyboard>();Time.timeScale=5;
            yield return null;
        }
        [UnityTearDown] public IEnumerator TearDown()
        {
            if(keyboard!=null)InputSystem.RemoveDevice(keyboard);
            var old=InputSystem.settings;var settings=ScriptableObject.CreateInstance<InputSettings>();
            JsonUtility.FromJsonOverwrite(SessionState.GetString("AtmajaAudit.Input","{}"),settings);InputSystem.settings=settings;Object.Destroy(old);
            Time.timeScale=1;yield return new ExitPlayMode();
            var directory=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",SessionState.GetString("AtmajaAudit.PreviousSave",""));
            if(!string.IsNullOrEmpty(directory)&&Directory.Exists(directory))Directory.Delete(directory,true);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
        [UnityTest] public IEnumerator SurveyLiveDonationCrouchAndRemappedVault()=>Run(Exterior());
        [UnityTest] public IEnumerator OnePhoneARClockZeroBatteryAndCheckpointResume()=>Run(Interior());
        [UnityTest] public IEnumerator PhoneAppsExposeCampaignAndAllowStowing()=>Run(PhoneApps());
        [UnityTest] public IEnumerator KosBothStairFlightsAndSharedRoomsAreContinuouslyWalkable()=>Run(KosNavigation());
        [UnityTest] public IEnumerator DiningSidesBackpackAndCornerRoomAreContinuouslyWalkable()=>Run(DiningNavigation());
        [UnityTest] public IEnumerator GroundFloorStairsAreContinuouslyWalkableToUpperFloor()=>Run(StairNavigation());
        private IEnumerator KosNavigation()
        {
            yield return SceneManager.LoadSceneAsync(PrologSceneFlow.CorridorScene);yield return new WaitForSeconds(1);
            var corridor=Find<KosCorridorDirector>();var p=corridor.player;p.enabled=false;
            Assert.That(p.transform.position.y,Is.GreaterThan(3));Assert.That(corridor.CanDepart,Is.False);
            foreach(var point in new[]{new Vector3(-.6f,3.3f,12.7f),new Vector3(-.6f,1.65f,16.4f),new Vector3(1.75f,1.65f,16.4f),new Vector3(1.75f,0,12.6f),new Vector3(0,0,12.6f),new Vector3(0,0,11.5f),new Vector3(0,0,4.25f)})yield return WalkCapsule(p,point);
            Assert.That(p.transform.position.y,Is.LessThan(.15f));
            GameObject.Find("DAPUR BERSAMADoor").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
            yield return WalkCapsule(p,new Vector3(-2.7f,0,4.25f));yield return WalkCapsule(p,new Vector3(0,0,4.25f));
            GameObject.Find("KAMAR MANDIDoor").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
            yield return WalkCapsule(p,new Vector3(2.5f,0,4.25f));yield return WalkCapsule(p,new Vector3(0,0,4.25f));yield return WalkCapsule(p,new Vector3(0,0,.8f));
            Assert.That(corridor.entranceDoor.IsOpen,Is.False);corridor.entranceDoor.Interact();yield return new WaitForSeconds(1.2f);
            Assert.That(corridor.entranceDoor.IsOpen,Is.True);yield return WalkCapsule(p,new Vector3(0,0,-4.7f));
            Assert.That(corridor.CanDepart,Is.True);Assert.That(CampaignState.Instance.Data.checkpoint_id,Is.EqualTo("PROLOG_KOS_YARD"));
        }
        private IEnumerator DiningNavigation()
        {
            yield return new WaitForSeconds(1);var d=Find<AtmajaDirector>();var p=d.player;
            foreach(var id in new[]{AtmajaDirector.SurveyId,"EVENT_P0_LIVE_STARTED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN"})CampaignState.Instance.AddInventoryItem(id);
            d.RestoreWorld();p.enabled=false;
            foreach(var point in new[]{d.P(5.15f,9.8f,.37f),d.P(4.1f,9.8f,.37f),d.P(4.1f,8.4f,.54f),d.P(5.35f,8,.54f),d.P(5.35f,6.2f,.54f)})yield return WalkCapsule(p,point,.7f);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));p.enabled=true;yield return new WaitForSeconds(.6f);p.enabled=false;
            Assert.That(p.IsCrouching,Is.True);Assert.That(p.GetComponent<CharacterController>().height,Is.LessThan(.81f));
            Assert.That(p.GetComponent<CharacterController>().stepOffset,Is.LessThanOrEqualTo(.06f));
            yield return WalkCapsule(p,d.P(4.6f,6.2f,.54f),.7f);
            var eye=p.GetComponent<InteractionInteractor>().playerCamera;eye.transform.LookAt(d.damarPhone.transform.position);yield return null;yield return null;
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(Item(AtmajaAction.DamarPhone)),"Backpack phone must be reachable from the side without a tabletop pose");
            yield return Press(Key.E);Assert.That(CampaignState.Instance.HasInventoryItem("MATABATIN_APK"),Is.True);
            yield return WalkCapsule(p,d.P(5.35f,6.2f,.54f),.7f);InputSystem.QueueStateEvent(keyboard,new KeyboardState());p.enabled=true;yield return new WaitForSeconds(.6f);p.enabled=false;
            foreach(var point in new[]{d.P(5.35f,1.3f,.54f),d.P(2.1f,1.3f,.54f),d.P(2.1f,4.1f,.54f),d.P(.8f,4.1f,.54f),d.P(-1.4f,4.1f,.54f),d.P(-1.4f,3.3f,.54f)})yield return WalkCapsule(p,point,.7f);
            GameObject.Find("FrontSittingDoor").GetComponent<HingedDoor>().Interact();yield return new WaitForSeconds(1.2f);
            yield return WalkCapsule(p,d.P(-1.4f,1.5f,.54f),.7f);yield return WalkCapsule(p,d.P(-4.4f,1.5f,.54f),.7f);
            eye.transform.LookAt(d.clockHands.transform.position);yield return null;yield return null;
            Assert.That(p.GetComponent<InteractionInteractor>().CurrentTarget,Is.SameAs(Item(AtmajaAction.ClockHands)));yield return Press(Key.E);
            Assert.That(CampaignState.Instance.HasInventoryItem("ITEM_P0_CLOCK_HANDS"),Is.True);
        }
        private IEnumerator StairNavigation()
        {
            yield return new WaitForSeconds(1);
            var d=Find<AtmajaDirector>();var p=d.player;var phone=SmartphoneController.Instance;
            foreach(var id in new[]{AtmajaDirector.SurveyId,"EVENT_P0_LIVE_STARTED","EVENT_P0_SIDE_CHAT","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN","EVENT_P0_CLOCK_UNLOCKED","EVENT_P0_STAIRS_UNLOCKED","EVENT_P0_AR_REVEALED","MATABATIN_APK"})CampaignState.Instance.AddInventoryItem(id);
            phone.arModuleUnlocked=true;d.RestoreWorld();p.enabled=false;
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.StairCheckpoint));
            // AR on the ground floor must not reveal the upstairs marks through the ceiling.
            phone.isPhoneRaised=true;phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;d.phoneView.Apps.Open(PhoneApp.Camera);
            yield return null;yield return null;
            Assert.That(d.upper.portraitBloodAR[0].activeSelf,Is.False,"Upstairs AR ink must stay hidden while on the ground floor");
            Assert.That(d.arClue.activeSelf,Is.True,"Ground-floor AR clue is visible downstairs");
            // Walk from the lounge approach, through the unlocked stair door, up both flights to the top landing.
            d.PlacePlayer(d.P(-2.4f,4.9f,.53f),Quaternion.LookRotation(d.north));
            foreach(var point in new[]
            {
                d.P(-2.4f,6.4f,.55f),d.P(-3.5f,6.4f,.56f),d.P(-3.5f,7.5f,.62f),d.P(-2.7f,7.45f,.8f),d.P(-1.6f,7.45f,1.44f),d.P(-0.5f,7.45f,1.95f),
                d.P(.2f,7.45f,2.08f),d.P(.2f,7.0f,2.3f),d.P(-0.6f,7.0f,2.6f),d.P(-1.6f,7.0f,3.06f),d.P(-2.7f,7.0f,3.7f),d.P(-3.25f,6.45f,3.83f)
            })yield return WalkCapsule(p,point,3.95f);
            p.enabled=true;yield return new WaitForSeconds(.6f);p.enabled=false;
            Assert.That(p.transform.position.y,Is.GreaterThan(3.4f),"Stairs must physically lift the capsule to the upper floor");
            yield return new WaitForSeconds(.4f);
            Assert.That(CampaignState.Instance.HasInventoryItem("EVENT_P0_UPPER_REACHED"),Is.True,"Stepping onto the landing must register the upper floor");
            Assert.That(d.upper.portraitBloodAR[0].activeSelf,Is.True,"Upstairs AR ink appears once the player is upstairs");
            Assert.That(d.arClue.activeSelf,Is.False,"Ground-floor AR clue must not bleed into the upper floor");
        }
        private static IEnumerator WalkCapsule(FirstPersonController p,Vector3 target,float maxHeight=float.PositiveInfinity)
        {
            p.enabled=false;var cc=p.GetComponent<CharacterController>();int stalled=0;
            for(int frame=0;frame<500;frame++)
            {
                var delta=target-p.transform.position;delta.y=0;if(delta.magnitude<.1f)break;
                var before=p.transform.position;
                // Drop the ground-stick push once stalled so the capsule is free to climb stair treads.
                cc.Move(Vector3.ClampMagnitude(delta,.16f)+Vector3.down*(stalled>2?0f:.18f));
                Assert.That(p.transform.position.y,Is.LessThanOrEqualTo(maxHeight),"Feet must stay below tabletop height");
                if(Vector3.Distance(before,p.transform.position)<.003f)stalled++;else stalled=0;if(stalled>15)break;yield return null;
            }
            var remaining=target-p.transform.position;remaining.y=0;
            if(remaining.magnitude>=.16f)
            {
                var pos=p.transform.position;
                var hits=Physics.OverlapCapsule(pos+Vector3.up*cc.radius,pos+Vector3.up*(cc.height-cc.radius),cc.radius+.02f,~0,QueryTriggerInteraction.Collide).Where(c=>c!=cc&&!c.transform.IsChildOf(p.transform)).Select(c=>c.name+"@"+c.bounds.center.ToString("F2"));
                Assert.That(remaining.magnitude,Is.LessThan(.16f),"Continuous CC route blocked at "+pos+" on way to "+target+" by ["+string.Join(",",hits)+"]");
            }
        }
        private IEnumerator PhoneApps()
        {
            yield return new WaitForSeconds(2);var view=Find<AtmajaPhoneView>();var phone=SmartphoneController.Instance;
            phone.isPhoneRaised=true;phone.isScreenOn=true;phone.SyncVisualState();view.Apps.Open(PhoneApp.Home);yield return null;
            Assert.That(view.Panel.GetComponentsInChildren<UnityEngine.UI.Button>().Length,Is.GreaterThanOrEqualTo(7));
            CampaignState.Instance.AddInventoryItem(AtmajaDirector.SurveyId);view.Apps.Open(PhoneApp.Journal);yield return null;
            var entries=GameObject.Find("JournalEntries").GetComponent<UnityEngine.UI.Text>();Assert.That(entries.text,Does.Contain("Survei Damar"));
            Assert.That(entries.GetComponentInParent<UnityEngine.UI.ScrollRect>().vertical,Is.True);
            view.Apps.Open(PhoneApp.Inventory);yield return null;Assert.That(GameObject.Find("InventoryEntries").GetComponent<UnityEngine.UI.Text>().text,Does.Contain("Smartphone Raka"));
            Assert.That(UiFocus.Phone,Is.True);yield return Press(Key.Tab);yield return null;
            Assert.That(phone.isPhoneRaised,Is.False);Assert.That(UiFocus.Phone,Is.False);
        }
        private IEnumerator Exterior()
        {
            yield return new WaitForSeconds(2);
            var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;var campaign=CampaignState.Instance;
            Assert.That(d.StartLive(),Is.False,"Cannot broadcast before reading Damar's survey");
            Item(AtmajaAction.Survey).Interact();Assert.That(HudController.Instance.documentBodyText.text,Is.EqualTo(AtmajaDirector.SurveyText));CloseDocument();
            yield return Press(Key.Tab);Assert.That(phone.isPhoneRaised,Is.True);
            d.phoneView.Apps.Open(PhoneApp.Camera);
            d.phoneView.LiveButton.onClick.Invoke();yield return null;
            Assert.That(phone.isBroadcastConnected,Is.True);Assert.That(d.Viewers,Is.EqualTo(14));
            d.PlacePlayer(d.P(8.2f,1,.06f),Quaternion.LookRotation(d.north));d.player.enabled=false;yield return null;yield return null;
            Assert.That(campaign.Data.wallet_bound,Is.EqualTo(5500000));
            d.PlacePlayer(d.P(8.2f,4.4f,.06f),Quaternion.LookRotation(d.north));var cc=d.player.GetComponent<CharacterController>();
            cc.Move(d.north*3);Assert.That(d.LocalPosition(d.player.transform.position).y,Is.LessThan(5.5f));Assert.That(d.CanVault(),Is.False);
            d.PlacePlayer(d.P(8.2f,4.4f,.06f),Quaternion.LookRotation(d.north));
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));d.player.enabled=true;yield return new WaitForSeconds(.5f);
            Assert.That(d.player.IsCrouching,Is.True);d.player.enabled=false;
            cc.height=d.player.crouchHeight;cc.center=Vector3.up*cc.height*.5f;
            cc.Move(d.north*1.1f);yield return null;cc.Move(d.north*1.3f);yield return null;
            Assert.That(d.TreeCleared,Is.True);Assert.That(campaign.Data.wallet_bound,Is.EqualTo(5500000));
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());
            d.PlacePlayer(d.vaultOutside.position,d.vaultOutside.rotation);
            var camera=d.player.GetComponent<InteractionInteractor>().playerCamera;
            camera.transform.LookAt(Vector3.Lerp(d.vaultOutside.position,d.vaultInside.position,.65f)+Vector3.up*1.3f);
            GameInputConfig.Instance.vaultKey=KeyCode.V;
            yield return Press(Key.Space);Assert.That(d.Stage,Is.EqualTo(AtmajaStage.SidePassage));
            yield return Press(Key.V);yield return new WaitForSeconds(1.5f);d.player.enabled=false;
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.Kitchen));Assert.That(d.kitchenShutter.activeSelf,Is.True);
            Assert.That(Vector3.Distance(d.player.transform.position,d.vaultInside.position),Is.LessThan(.3f));
            Assert.That(campaign.Data.checkpoint_id,Is.EqualTo("PROLOG_ATMAJA_KITCHEN"));
            d.PlacePlayer(d.P(5,10.9f,.37f),Quaternion.LookRotation(d.north));
            camera.transform.position=d.P(5,10.9f,1.7f);camera.transform.LookAt(d.P(2.7f,11.2f,.75f));Physics.SyncTransforms();
            Assert.That(d.TryPhotographWell(),Is.True,"Optional bonus needs the actual well sight line");
            Assert.That(campaign.Data.wallet_legitimate,Is.EqualTo(150000));
            Assert.That(d.TryPhotographWell(),Is.False,"No repeated well farming");Assert.That(campaign.Data.wallet_legitimate,Is.EqualTo(150000));
        }
        private IEnumerator Interior()
        {
            yield return new WaitForSeconds(2);
            var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;var campaign=CampaignState.Instance;
            foreach(var id in new[]{AtmajaDirector.SurveyId,"EVENT_P0_LIVE_STARTED","EVENT_P0_SIDE_CHAT","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN"})campaign.AddInventoryItem(id);
            campaign.Data.wallet_bound=5500000;d.RestoreWorld();phone.isBroadcastConnected=true;
            // Exhaust both battery and one-shot powerbank: the real emergency port must still recover play.
            campaign.Data.inventory.Remove("POWERBANK_KECIL_KOS");phone.ConsumeBattery(100);Item(AtmajaAction.Charge).Interact();yield return new WaitForSeconds(3.5f);
            Assert.That(phone.batteryPercent,Is.GreaterThan(34));Assert.That(phone.isScreenOn,Is.True);
            d.PlacePlayer(d.P(3.6f,7.4f,.54f),Quaternion.LookRotation(d.north));
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));yield return new WaitForSeconds(.5f);
            Item(AtmajaAction.DamarPhone).Interact();InputSystem.QueueStateEvent(keyboard,new KeyboardState());
            Assert.That(phone.arModuleUnlocked,Is.True);Assert.That(campaign.Data.upgrades.Count(x=>x=="MATABATIN_AR"),Is.EqualTo(1));
            Assert.That(Object.FindObjectsByType<SmartphoneController>(FindObjectsSortMode.None).Length,Is.EqualTo(1));
            phone.isPhoneRaised=true;phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;yield return null;yield return null;
            Assert.That(d.arFigure.activeSelf,Is.True);Assert.That(d.phoneView.FeedCamera.enabled,Is.EqualTo(d.phoneView.HasGraphicsDevice));
            Assert.That(d.phoneView.playerCamera.cullingMask&(1<<AtmajaPhoneView.ARLayer),Is.Zero);
            phone.isScreenOn=false;yield return null;yield return null;
            Assert.That(d.phoneView.FeedCamera.enabled,Is.False);Assert.That(d.arFigure.activeSelf,Is.False);Assert.That(phone.isBroadcastConnected,Is.True);
            Assert.That(d.phoneView.Panel.GetComponentInChildren<UnityEngine.UI.RawImage>(true).enabled,Is.False);
            phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;d.phoneView.Apps.Open(PhoneApp.Camera);
            d.player.enabled=false;d.PlacePlayer(d.P(-1.3f,4.9f,.54f),Quaternion.LookRotation(d.north));
            var clueCamera=d.player.GetComponent<InteractionInteractor>().playerCamera;clueCamera.transform.position=d.P(-1.3f,4.9f,1.85f);clueCamera.transform.LookAt(d.arClue.transform.position);yield return null;yield return null;
            Assert.That(campaign.HasInventoryItem("EVENT_P0_AR_CLUE_READ"),Is.True,"Clue must actually be aimed at before solving");
            Item(AtmajaAction.ClockHands).Interact();Item(AtmajaAction.Clock).Interact();
            var puzzle=Find<ClockPuzzleView>();puzzle.Confirm();Assert.That(campaign.HasInventoryItem("KUNCI_TANGGA_TENGAH"),Is.False);
            puzzle.AdjustHour(3);puzzle.AdjustMinute(45);puzzle.Confirm();Assert.That(HudController.Instance.documentBodyText.text,Does.Contain("TRAGEDI KELUARGA ATMAJA"));
            // Simulate quitting mid-document: the key save precedes the read receipt.
            yield return SceneManager.LoadSceneAsync(AtmajaSceneBuilder.ScenePath);yield return new WaitForSeconds(2);
            d=Find<AtmajaDirector>();campaign=CampaignState.Instance;phone=SmartphoneController.Instance;
            Assert.That(campaign.HasInventoryItem("DOC_PROLOG_03_KORAN_1998"),Is.False);
            Item(AtmajaAction.Clock).Interact();Assert.That(HudController.Instance.IsModalOpen,Is.True);CloseDocument();
            Item(AtmajaAction.StairDoor).Interact();phone.batteryPercent=12;phone.SyncVisualState();campaign.Data.battery=12;
            Assert.That(campaign.SaveCheckpointAtomic("PROLOG_ATMAJA_STAIRS_UNLOCKED"),Is.True);
            yield return SceneManager.LoadSceneAsync(PrologKosSceneBuilder.ScenePath);
            float deadline=Time.realtimeSinceStartup+15;
            while(SceneManager.GetActiveScene().name!=PrologSceneFlow.AtmajaScene&&Time.realtimeSinceStartup<deadline)yield return null;
            Assert.That(SceneManager.GetActiveScene().name,Is.EqualTo(PrologSceneFlow.AtmajaScene));yield return new WaitForSeconds(2);
            d=Find<AtmajaDirector>();campaign=CampaignState.Instance;
            Assert.That(d.Stage,Is.EqualTo(AtmajaStage.StairCheckpoint));Assert.That(campaign.Data.wallet_bound,Is.EqualTo(5500000));
            Assert.That(campaign.HasInventoryItem("DOC_PROLOG_03_KORAN_1998"),Is.True);Assert.That(SmartphoneController.Instance.arModuleUnlocked,Is.True);
            Assert.That(SmartphoneController.Instance.batteryPercent,Is.InRange(11,12));
            Assert.That(Vector3.Distance(d.player.transform.position,d.spawnStairs.position),Is.LessThan(.3f));
        }
        private IEnumerator Press(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
        private static T Find<T>()where T:Object=>Object.FindFirstObjectByType<T>();
        private static AtmajaItem Item(AtmajaAction action)=>Object.FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).First(x=>x.action==action);
        private static UnityEngine.UI.Button Button(string name)=>FoundationAuditTests.Button(HudController.Instance,name);
        private static void CloseDocument()=>HudController.Instance.documentCloseButton.onClick.Invoke();
        // Execute waits on the player loop, while forwarding NUnit assertion failures to the Editor runner.
        private static IEnumerator Run(IEnumerator scenario)
        {
            var go=new GameObject("AtmajaTestHost");Object.DontDestroyOnLoad(go);var host=go.AddComponent<FoundationAuditCoroutineHost>();bool done=false;Exception failure=null;
            host.StartCoroutine(Drive(scenario,e=>{failure=e;done=true;}));float deadline=Time.realtimeSinceStartup+60;
            while(!done&&Time.realtimeSinceStartup<deadline)yield return null;
            Object.Destroy(go);Assert.That(done,Is.True,"Atmaja runtime scenario timeout");if(failure!=null)throw failure;
        }
        private static IEnumerator Drive(IEnumerator scenario,Action<Exception> complete)
        {
            var stack=new Stack<IEnumerator>();stack.Push(scenario);
            while(stack.Count>0)
            {
                object next=null;Exception failure=null;bool more=false;
                try{more=stack.Peek().MoveNext();if(more)next=stack.Peek().Current;}catch(Exception e){failure=e;}
                if(failure!=null){complete(failure);yield break;}if(!more){stack.Pop();continue;}
                if(next is IEnumerator nested)stack.Push(nested);else yield return next;
            }
            complete(null);
        }
    }
}
#endif
