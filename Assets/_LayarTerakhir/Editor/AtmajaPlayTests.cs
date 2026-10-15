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
        private IEnumerator Exterior()
        {
            yield return new WaitForSeconds(2);
            var d=Find<AtmajaDirector>();var phone=SmartphoneController.Instance;var campaign=CampaignState.Instance;
            Assert.That(d.StartLive(),Is.False,"Cannot broadcast before reading Damar's survey");
            Item(AtmajaAction.Survey).Interact();Assert.That(HudController.Instance.documentBodyText.text,Is.EqualTo(AtmajaDirector.SurveyText));CloseDocument();
            yield return Press(Key.Tab);Assert.That(phone.isPhoneRaised,Is.True);
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
            Item(AtmajaAction.ClockHands).Interact();Item(AtmajaAction.Clock).Interact();
            Button("UnlockPadlockBtn").onClick.Invoke();Assert.That(campaign.HasInventoryItem("KUNCI_TANGGA_TENGAH"),Is.False);
            for(int i=0;i<3;i++)for(int n=0;n<i+3;n++)Button("DigitUp_"+i).onClick.Invoke();
            Button("UnlockPadlockBtn").onClick.Invoke();Assert.That(HudController.Instance.documentBodyText.text,Does.Contain("TRAGEDI KELUARGA ATMAJA"));
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
