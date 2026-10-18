#if UNITY_EDITOR || DEVELOPMENT_BUILD
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
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LayarTerakhir.Diagnostics
{
    // Explicit opt-in, isolated save, real standalone framebuffer. No changes to normal play.
    public sealed class AtmajaPlayerAudit:MonoBehaviour
    {
        [Serializable] private class Receipt
        {
            public string utc,platform,gpu,scene,saveDirectory;
            public bool actualStandalonePlayer,completed,checkpointSaved,transitionFromKos,acceleratedStoryTimers;
            public int width,height;public string[] screenshots,checks;
        }
        private static string output;
        private readonly List<string> shots=new List<string>(),checks=new List<string>();
        private AtmajaDirector d;private Camera playerCamera;private bool failed,done;private float started;
        private Receipt receipt;private Keyboard keyboard;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-atmajaAudit");if(i<0||i+1>=args.Length)return;
            output=Path.GetFullPath(args[i+1]);Directory.CreateDirectory(output);
            var save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            var data=new CampaignData{checkpoint_id="PROLOG_KOS_COMPLETE",wallet_bound=5000000};
            data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA","POWERBANK_KECIL_KOS"});
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),JsonUtility.ToJson(data,true));
            Application.runInBackground=true;
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            var go=new GameObject("AtmajaPlayerAudit");DontDestroyOnLoad(go);go.AddComponent<AtmajaPlayerAudit>();
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.logMessageReceived+=OnLog;}
        private void OnLog(string message,string stack,LogType type){if(type==LogType.Error||type==LogType.Exception){failed=true;checks.Add("ERROR "+message);}}
        private void Update(){if(!done&&(failed||Time.realtimeSinceStartup-started>750))Finish(false);}
        private void OnDestroy(){Application.logMessageReceived-=OnLog;if(keyboard!=null)InputSystem.RemoveDevice(keyboard);}
        private IEnumerator Start()
        {
            yield return null;
            receipt=new Receipt{utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,
                actualStandalonePlayer=!Application.isEditor,saveDirectory=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY"),width=Screen.width,height=Screen.height,acceleratedStoryTimers=true};
            Time.timeScale=4;Time.maximumDeltaTime=2;Time.fixedDeltaTime=.08f;
            var kos=FindFirstObjectByType<KamarKosPrologDirector>();
            Check(kos&&kos.currentStage==KosPrologStage.ReadyToDepart,"Real Kos checkpoint restored at the approved departure");
            GameInputConfig.Instance.vaultKey=KeyCode.Y;
            kos.kosDoorItem.Interact();
            float until=Time.realtimeSinceStartup+200; // Software-rendering texture upload is not a gameplay timeout.
            while(SceneManager.GetActiveScene().name!=PrologSceneFlow.AtmajaScene&&Time.realtimeSinceStartup<until)yield return null;
            receipt.transitionFromKos=SceneManager.GetActiveScene().name==PrologSceneFlow.AtmajaScene;
            Check(receipt.transitionFromKos,"Actual async Kos -> Atmaja scene transition completed");if(!receipt.transitionFromKos){Finish(false);yield break;}
            yield return new WaitForSeconds(2);
            d=FindFirstObjectByType<AtmajaDirector>();playerCamera=d.player.GetComponent<InteractionInteractor>().playerCamera;receipt.scene=SceneManager.GetActiveScene().path;
            Check(GameInputConfig.Instance.vaultKey==KeyCode.Y,"A remapped Kos input binding survives the actual departure transition");
            yield return Shot("01-gate-arrival");
            Item(AtmajaAction.Gate).Interact();yield return new WaitForSeconds(1.2f);
            Pose(4.8f,-6.4f,1.6f,4.8f,-4.2f,.75f);yield return Shot("02-damar-scooter");
            Check(ReferenceEquals(d.player.GetComponent<InteractionInteractor>().CurrentTarget,Item(AtmajaAction.Survey)),"Scooter is a real first-hit raycast target");
            Item(AtmajaAction.Survey).Interact();yield return Shot("03-survey-document");HudController.Instance.documentCloseButton.onClick.Invoke();
            Check(d.HasSurvey,"Closing the exact Damar survey commits document #2");
            var phone=SmartphoneController.Instance;phone.isPhoneRaised=true;phone.isScreenOn=true;phone.SyncVisualState();yield return null;
            yield return Shot("04-start-live");d.phoneView.LiveButton.onClick.Invoke();yield return null;
            Check(phone.isBroadcastConnected&&d.Viewers==14,"MULAI LIVE button starts fourteen viewers");
            Pose(8.2f,1.0f,1.6f,8.2f,5.5f,1.2f);yield return new WaitForSeconds(.2f);
            long paid=CampaignState.Instance.Data.wallet_bound;yield return new WaitForSeconds(.2f);
            Check(paid==5500000&&CampaignState.Instance.Data.wallet_bound==paid,"Side chat/500k donation is saved exactly once");
            Pose(8.2f,4.1f,1.6f,8.2f,5.5f,1.1f);yield return Shot("05-fallen-kamboja");
            var cc=d.player.GetComponent<CharacterController>();
            d.PlacePlayer(d.P(8.2f,4.4f,.06f),Quaternion.LookRotation(d.north));d.player.enabled=false;
            cc.height=d.player.standingHeight;cc.center=Vector3.up*cc.height*.5f;cc.Move(d.north*3);
            Check(d.LocalPosition(d.player.transform.position).y<5.5f,"Standing CharacterController is physically stopped by the fallen limb");
            d.PlacePlayer(d.P(8.2f,4.4f,.06f),Quaternion.LookRotation(d.north));
            keyboard=InputSystem.AddDevice<Keyboard>();InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));d.player.enabled=true;yield return null;
            d.player.enabled=false;cc.height=d.player.crouchHeight;cc.center=Vector3.up*cc.height*.5f;
            cc.Move(d.north*1.1f);yield return null;cc.Move(d.north*1.3f);yield return null;
            Check(d.TreeCleared&&d.LocalPosition(d.player.transform.position).y>6.4f,"Crouched physical capsule crosses the limb and earns the real route flag");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());
            Pose(7.6f,10.975f,1.6f,6,10.975f,1.5f);yield return Shot("06-kitchen-window");
            d.PlacePlayer(d.vaultOutside.position,d.vaultOutside.rotation);d.player.enabled=false;
            yield return d.Vault();d.player.enabled=false;
            Check(d.Stage==AtmajaStage.Kitchen&&d.kitchenShutter.activeSelf&&Vector3.Distance(d.player.transform.position,d.vaultInside.position)<.2f,"Vault lands inside kitchen, slams window and saves checkpoint");
            Pose(5.0f,10.9f,1.7f,2.85f,11.2f,.8f);yield return Shot("07-kitchen-well");
            Item(AtmajaAction.KitchenDoor).Interact();yield return new WaitForSeconds(1.2f);
            Pose(3.6f,7.5f,1.13f,3.61f,6.77f,.85f);playerCamera.transform.LookAt(d.damarPhone.transform.position);yield return Shot("08-damar-backpack");
            Check(ReferenceEquals(d.player.GetComponent<InteractionInteractor>().CurrentTarget,Item(AtmajaAction.DamarPhone)),"Cracked phone on the backpack is reachable by the real crouched raycast");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));d.player.enabled=true;yield return null;d.player.enabled=false;
            Item(AtmajaAction.DamarPhone).Interact();InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
            Check(phone.arModuleUnlocked&&CampaignState.Instance.HasInventoryItem("MATABATIN_APK"),"Damar chip unlocks AR on Raka's one phone");
            Pose(3.7f,5.8f,1.62f,3.7f,4.05f,1.4f);phone.isPhoneRaised=true;phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;phone.SyncVisualState();yield return null;
            foreach(var renderer in d.arFigure.GetComponentsInChildren<Renderer>())Debug.Log("ATMAJA_AR_RENDERER "+renderer.name+" bounds="+renderer.bounds+" layer="+renderer.gameObject.layer+" enabled="+renderer.enabled);
            yield return Shot("09-ar-dining-figure");
            Check(d.arFigure.activeSelf&&(playerCamera.cullingMask&(1<<AtmajaPhoneView.ARLayer))==0&&d.phoneView.FeedCamera.enabled,"AR apparition renders only in the active phone feed");
            phone.isScreenOn=false;phone.SyncVisualState();yield return null;
            Check(phone.isBroadcastConnected&&!d.phoneView.FeedCamera.enabled&&!d.arFigure.activeSelf,"Dark screen keeps broadcast connected but stops extra rendering");
            phone.isScreenOn=true;phone.SyncVisualState();
            Pose(-1.6f,5.7f,1.6f,-1.6f,7.76f,2.05f);yield return Shot("10-ar-clock-clue");
            Item(AtmajaAction.ClockHands).Interact();Item(AtmajaAction.Clock).Interact();
            Button("UnlockPadlockBtn").onClick.Invoke();Check(!CampaignState.Instance.HasInventoryItem("KUNCI_TANGGA_TENGAH"),"Wrong clock time never grants the stair key");
            foreach(var pair in new[]{(0,3),(1,4),(2,5)})for(int n=0;n<pair.Item2;n++)Button("DigitUp_"+pair.Item1).onClick.Invoke();
            Button("UnlockPadlockBtn").onClick.Invoke();yield return Shot("11-newspaper-1998");HudController.Instance.documentCloseButton.onClick.Invoke();
            Check(CampaignState.Instance.HasInventoryItem("KUNCI_TANGGA_TENGAH"),"03:45 unlocks the clock drawer/key and original newspaper");
            Pose(-3.55f,6.0f,1.6f,-3.55f,7.7f,1.7f);Item(AtmajaAction.StairDoor).Interact();yield return Shot("12-ground-floor-checkpoint");
            receipt.checkpointSaved=CampaignState.Instance.LoadCheckpoint()&&CampaignState.Instance.Data.checkpoint_id=="PROLOG_ATMAJA_STAIRS_UNLOCKED";
            Check(receipt.checkpointSaved,"Continuation checkpoint reloads without restarting Kos or paying twice");
            Finish(!failed);
        }
        private AtmajaItem Item(AtmajaAction action)=>FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).First(x=>x.action==action);
        private Button Button(string name)=>HudController.Instance.padlockPanel.GetComponentsInChildren<Button>(true).Single(b=>b.name==name);
        private void Pose(float x,float depth,float eyeY,float tx,float tz,float ty)
        {d.player.enabled=false;d.PlacePlayer(d.P(x,depth,.54f),Quaternion.LookRotation(d.north));playerCamera.transform.position=d.P(x,depth,eyeY);playerCamera.transform.LookAt(d.P(tx,tz,ty));Physics.SyncTransforms();}
        private IEnumerator Shot(string name)
        {yield return new WaitForSeconds(.35f);yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;yield return null;shots.Add(name+".png");}
        private void Check(bool pass,string label){checks.Add((pass?"PASS ":"FAIL ")+label);Debug.Log("ATMAJA_PLAYER_AUDIT "+checks[checks.Count-1]);if(!pass)failed=true;}
        private void Finish(bool success)
        {if(done)return;done=true;StopAllCoroutines();receipt=receipt??new Receipt();receipt.completed=success;receipt.screenshots=shots.ToArray();receipt.checks=checks.ToArray();File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(receipt,true)+"\n");Debug.Log("ATMAJA_PLAYER_AUDIT completed="+success+" shots="+shots.Count);Application.Quit(success?0:1);}
    }
}
#endif
