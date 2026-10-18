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
    // Focused native presentation proof, seeded at the kitchen. No normal-launch behavior.
    public sealed class DesignerReviewPlayerAudit : MonoBehaviour
    {
        [Serializable] private class Receipt
        {
            public string utc,platform,gpu,navigationMethod;
            public bool actualStandalonePlayer,completed,checkpointSaved;
            public int width,height;public float walkedMeters;
            public string[] checks,screenshots;
        }
        private static string output;private Receipt receipt;private bool done;
        private readonly List<string> checks=new List<string>(),shots=new List<string>();
        private Keyboard keyboard;private AtmajaDirector d;private FirstPersonController player;private Camera eye;
        private SmartphoneController Phone=>SmartphoneController.Instance;private HudController Hud=>HudController.Instance;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int index=Array.IndexOf(args,"-designerAudit");if(index<0||index+1>=args.Length)return;
            output=Path.GetFullPath(args[index+1]);Directory.CreateDirectory(output);
            var save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            var data=new CampaignData{checkpoint_id="PROLOG_ATMAJA_KITCHEN",battery=85,wallet_bound=500000};
            data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA","EVENT_P0_ATMAJA_ARRIVED","EVENT_P0_GATE_OPEN","EVENT_P0_LIVE_STARTED","EVENT_P0_TREE_CROUCHED","EVENT_P0_KITCHEN_ENTERED","EVENT_P0_DINING_OPEN"});
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),JsonUtility.ToJson(data,true));Application.runInBackground=true;
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            var go=new GameObject("DesignerReviewPlayerAudit");DontDestroyOnLoad(go);go.AddComponent<DesignerReviewPlayerAudit>();
        }
        private void Start(){Application.logMessageReceived+=OnLog;keyboard=InputSystem.AddDevice<Keyboard>();InputSystem.EnableDevice(keyboard);StartCoroutine(Drive(Scenario()));}
        private IEnumerator Scenario()
        {
            receipt=new Receipt{utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,actualStandalonePlayer=!Application.isEditor,width=Screen.width,height=Screen.height,navigationMethod="Isolated kitchen checkpoint; continuous CharacterController substeps and real E/CTRL/Q bindings; narration timers accelerated 2x; no FPS/timing claim"};
            yield return Wait(()=>SceneManager.GetActiveScene().name==PrologSceneFlow.AtmajaScene&&FindFirstObjectByType<AtmajaDirector>()&&FindFirstObjectByType<AtmajaPhoneView>().Apps,90,"Load the authored kitchen checkpoint");
            yield return new WaitForSeconds(2);d=FindFirstObjectByType<AtmajaDirector>();Bind();Time.timeScale=2;
            Check(!Phone.arModuleUnlocked,"The seeded kitchen still needs Damar's memory card");
            yield return Walk(d.P(5.15f,9.8f,.37f));yield return Walk(d.P(4.1f,9.8f,.37f));yield return Walk(d.P(4.1f,8.4f,.54f));yield return Walk(d.P(5.35f,6.2f,.54f));
            yield return Crouch(true);yield return Walk(d.P(4.6f,6.2f,.54f));
            yield return Use(Item(AtmajaAction.DamarPhone),d.damarPhone.GetComponent<Collider>().bounds.center);
            yield return Wait(()=>GameObject.Find("DamarMemoryCard"),30,"The interaction shows the extracted physical memory card");yield return new WaitForSeconds(.35f);yield return Shot("01-memory-card-transfer");
            yield return Wait(()=>GameObject.Find("MataBatinInstaller"),30,"The handset displays an actual APK installer");yield return Shot("02-apk-installation");
            yield return Wait(()=>!d.Busy&&Phone.arModuleUnlocked,30,"Installation finishes before the AR module becomes usable");
            Check(CampaignState.Instance.HasInventoryItem("MATABATIN_APK")&&CampaignState.Instance.HasInventoryItem("EVENT_P0_AR_TUTORIAL"),"APK and one-time tutorial are saved");
            yield return Crouch(false);Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            yield return Walk(d.P(5.35f,4.6f,.54f));Aim(d.P(3.7f,4.2f,1.49f));yield return Shot("03-rotten-banquet-plates");
            yield return Walk(d.P(5.35f,1.5f,.54f));Aim(d.P(3.7f,2.15f,1.8f));yield return Shot("04-empty-chair-normal");
            Check(!d.arFigure.activeSelf&&!GameObject.Find("SeatedDollOnEndChair"),"The ordinary chair contains no seated figure");
            yield return Press(Key.Q);yield return new WaitForSeconds(.6f);Check(d.arFigure.activeSelf,"First AR activation reveals the seated broken-neck woman");yield return Shot("05-seated-woman-in-ar");
            Check((eye.cullingMask&(1<<AtmajaPhoneView.ARLayer))==0&&(d.phoneView.FeedCamera.cullingMask&(1<<AtmajaPhoneView.ARLayer))!=0,"The apparition belongs exclusively to the handset AR feed");
            d.phoneView.Apps.Open(PhoneApp.Home);yield return null;Check(GameObject.Find("Icon_MataBatin"),"The installed application has its own icon");yield return Shot("06-installed-matabatin-icon");d.phoneView.Apps.Open(PhoneApp.Camera);
            yield return Walk(d.P(5.35f,7.5f,.54f));yield return Walk(d.P(2.3f,7.5f,.54f));
            var survey=Item(AtmajaAction.Survey);yield return Use(survey,survey.GetComponent<Collider>().bounds.center);
            Check(Hud.IsDocumentOpen&&Phone.isPhoneRaised&&Phone.isScreenOn&&d.phoneView.IsCameraApp,"Reading retains the raised active camera handset");
            Check(CampaignState.Instance.Data.documents.Any(n=>n.body==AtmajaDirector.SurveyText),"The entire document is archived when opened");yield return Shot("07-reading-with-held-phone");Hud.CloseDocumentModal();
            d.phoneView.Apps.Open(PhoneApp.Journal);yield return null;
            var entry=FindObjectsByType<Button>(FindObjectsSortMode.None).First(b=>b.name.StartsWith("JournalEntry_")&&b.GetComponentInChildren<Text>().text.Contains("CATATAN SURVEI DAMAR"));entry.onClick.Invoke();yield return null;
            Check(GameObject.Find("EntryBody").GetComponentInChildren<Text>().text==AtmajaDirector.SurveyText,"Phone notes reopen the exact original text");yield return Shot("08-phone-notes-full-document");
            d.phoneView.Apps.Open(PhoneApp.Camera);yield return Walk(d.P(5.35f,7.5f,.54f));yield return Walk(d.P(5.35f,8.3f,.54f));yield return Walk(d.P(4.1f,8.4f,.54f));yield return Walk(d.P(4.1f,9.8f,.37f));yield return Walk(d.P(3.9f,11.2f,.37f));
            yield return Wait(()=>!d.Busy,20,"Raka turns toward the well whisper");Aim(d.P(2.85f,11.2f,1.08f));yield return Shot("09-well-covered");
            var well=FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).First(i=>i.action==AtmajaAction.Well&&i.name=="RottenWellBoard");yield return Use(well,well.GetComponent<Collider>().bounds.center,true);
            yield return Wait(()=>Quaternion.Angle(GameObject.Find("WellCover").transform.localRotation,Quaternion.LookRotation(d.north))>65,20,"The well cover opens on its hinge");yield return Shot("10-well-open");
            yield return Wait(()=>player.transform.position.y<-.8f,20,"The pull drags the viewpoint inside the hollow shaft");yield return Shot("11-pulled-inside-well");
            yield return Wait(()=>GameShell.AwaitingStart,30,"The well death opens the main menu");Check(UiFocus.Menu&&Time.timeScale==0,"Death waits for a menu choice");yield return Shot("12-death-main-menu");
            var old=d;Click("Continue");yield return Wait(()=>SceneManager.GetActiveScene().name==PrologSceneFlow.AtmajaScene&&FindFirstObjectByType<AtmajaDirector>()!=old,90,"Explicit Continue reloads the saved campaign");yield return new WaitForSeconds(2);d=FindFirstObjectByType<AtmajaDirector>();Bind();
            Check(!GameShell.AwaitingStart&&!d.Busy&&CampaignState.Instance.Data.documents.Any(n=>n.body==AtmajaDirector.SurveyText),"Continue restores control and archived notes without another death during scene loading");yield return Shot("13-continue-after-well");
            receipt.checkpointSaved=CampaignState.Instance.HasValidCheckpoint;Check(Hud.CurrentObjective=="","No task list appears after restoring");Finish(true);
        }
        private void Bind(){player=d.player;eye=player.GetComponent<InteractionInteractor>().playerCamera;player.enabled=false;}
        private AtmajaItem Item(AtmajaAction action)=>FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).First(i=>i.action==action);
        private void Aim(Vector3 point)
        {var delta=point-player.cameraPivot.position;var flat=Vector3.ProjectOnPlane(delta,Vector3.up);player.transform.rotation=Quaternion.LookRotation(flat);player.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();eye.transform.LookAt(point);}
        private IEnumerator Walk(Vector3 target)
        {
            player.enabled=false;var cc=player.GetComponent<CharacterController>();int stuck=0;
            for(int frame=0;frame<800;frame++)
            {
                if(d.Busy){yield return null;continue;}var delta=Vector3.ProjectOnPlane(target-player.transform.position,Vector3.up);if(delta.magnitude<.12f)break;
                var before=player.transform.position;cc.Move(Vector3.ClampMagnitude(delta,.13f)+Vector3.down*.16f);receipt.walkedMeters+=Vector3.ProjectOnPlane(player.transform.position-before,Vector3.up).magnitude;
                stuck=Vector3.Distance(before,player.transform.position)<.003f?stuck+1:0;if(stuck>25)throw new Exception("Capsule blocked: "+player.transform.position+" toward "+target);player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();yield return null;
            }
            Check(Vector3.ProjectOnPlane(target-player.transform.position,Vector3.up).magnitude<.16f,"Continuous capsule reaches "+target);
        }
        private IEnumerator Crouch(bool crouch)
        {
            keyboard.MakeCurrent();InputSystem.QueueStateEvent(keyboard,crouch?new KeyboardState(Key.LeftCtrl):new KeyboardState());player.enabled=true;
            // A few rendered frames are required on the headless software renderer; game-time alone can be one frame.
            yield return null;yield return null;yield return new WaitForSeconds(.6f);
            Debug.Log("DESIGNER_POSTURE requested="+crouch+" actual="+player.IsCrouching+" key="+GameInputConfig.Instance.GetActionLabel(GameAction.Crouch)+" held="+GameInputConfig.Instance.GetActionHeld(GameAction.Crouch)+" enabled="+keyboard.enabled+" device="+keyboard.deviceId+" current="+Keyboard.current?.deviceId+" ownHeld="+keyboard.leftCtrlKey.isPressed+" locked="+player.MovementLocked+" modal="+Hud.IsModalOpen+" menu="+UiFocus.Menu+" cinematic="+UiFocus.Cinematic);
            player.enabled=false;Check(player.IsCrouching==crouch,"Crouch input changes the physical capsule");
        }
        private IEnumerator Press(Key key)
        {InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
        private IEnumerator Use(AtmajaItem item,Vector3 point,bool anyWell=false)
        {
            Aim(point);yield return null;yield return null;var target=player.GetComponent<InteractionInteractor>().CurrentTarget;
            Check(target==item||anyWell&&target is AtmajaItem found&&found.action==AtmajaAction.Well,"The first interaction ray reaches "+item.name);yield return Press(Key.E);
        }
        private IEnumerator Wait(Func<bool> condition,float seconds,string label)
        {float until=Time.realtimeSinceStartup+seconds;while(!condition()&&Time.realtimeSinceStartup<until)yield return null;Check(condition(),label);}
        private IEnumerator Shot(string name)
        {float scale=Time.timeScale;Time.timeScale=0;yield return null;yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;yield return new WaitForEndOfFrame();Time.timeScale=scale;shots.Add(name+".png");}
        private static void Click(string name){var button=GameObject.Find(name)?.GetComponent<Button>();if(!button||!button.interactable)throw new Exception("Missing enabled button "+name);button.onClick.Invoke();}
        private void Check(bool value,string label){checks.Add((value?"PASS ":"FAIL ")+label);Debug.Log("DESIGNER_AUDIT "+checks[checks.Count-1]);if(!value)throw new Exception(label);}
        private IEnumerator Drive(IEnumerator scenario)
        {
            var stack=new Stack<IEnumerator>();stack.Push(scenario);
            while(stack.Count>0&&!done)
            {bool more;object current=null;try{more=stack.Peek().MoveNext();if(more)current=stack.Peek().Current;}catch(Exception e){checks.Add("FAIL "+e);Finish(false);yield break;}if(!more){stack.Pop();continue;}if(current is IEnumerator nested)stack.Push(nested);else yield return current;}
        }
        private void Finish(bool success)
        {if(done)return;done=true;receipt.completed=success;receipt.checks=checks.ToArray();receipt.screenshots=shots.ToArray();File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(receipt,true)+"\n");Application.Quit(success?0:1);}
        private void OnLog(string message,string stack,LogType type){if(!done&&receipt!=null&&(type==LogType.Error||type==LogType.Exception||type==LogType.Assert)){checks.Add("FAIL "+message);Finish(false);}}
        private void OnDestroy(){Application.logMessageReceived-=OnLog;if(keyboard!=null)InputSystem.RemoveDevice(keyboard);}
    }
}
#endif
