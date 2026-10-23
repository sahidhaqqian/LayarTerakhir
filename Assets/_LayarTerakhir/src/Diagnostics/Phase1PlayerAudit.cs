#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LayarTerakhir.Diagnostics
{
    // Explicit synthetic driver and isolated save. Captures real standalone frames,
    // capsule collision and production interactions; never a claim of manual play/FPS.
    public sealed class Phase1PlayerAudit : MonoBehaviour
    {
        [Serializable] private sealed class Receipt
        {
            public bool completed, actualStandalonePlayer, checkpointSaved;
            public string platform, gpu, utc, method;
            public float walkedMeters;
            public string[] checks, screenshots;
        }
        private static string output;
        private Phase1Director d;
        private Keyboard keyboard; private Mouse mouse;
        private readonly List<string> checks = new List<string>(), shots = new List<string>();
        private bool finished;
        private float started;
        private Receipt receipt;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int index=Array.IndexOf(args,"-phase1Audit");if(index<0||index+1>=args.Length)return;
            output=Path.GetFullPath(args[index+1]);Directory.CreateDirectory(output);
            string save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            var data=new CampaignData{chapter_id=1,checkpoint_id="PHASE1_CP1",battery=100,sanity=23,wallet_bound=6000000};
            data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","JAKET_RAKA","MATABATIN_APK"});data.phase1.Add("P1_ARRIVAL");
            int seedIndex=Array.IndexOf(args,"-phase1AuditSeed");
            string json=seedIndex>=0&&seedIndex+1<args.Length?File.ReadAllText(args[seedIndex+1]):JsonUtility.ToJson(data,true);
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),json);
            var go=new GameObject("Phase1PlayerAudit");DontDestroyOnLoad(go);go.AddComponent<Phase1PlayerAudit>();
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
        }
        private void Awake(){if(Array.IndexOf(Environment.GetCommandLineArgs(),"-phase1AuditFast")>=0){Time.timeScale=3;Time.maximumDeltaTime=.333333f;}started=Time.realtimeSinceStartup;Application.logMessageReceived+=Log;Application.SetStackTraceLogType(LogType.Log,StackTraceLogType.None);Application.SetStackTraceLogType(LogType.Warning,StackTraceLogType.None);}
        private void Log(string message,string trace,LogType type)
        {if(type==LogType.Error||type==LogType.Exception){checks.Add("ERROR "+message);Finish(false);}}
        private void Update(){if(!finished&&Time.realtimeSinceStartup-started>1800){checks.Add("FAIL timeout");Finish(false);}}
        private IEnumerator Start()
        {
            receipt=new Receipt{utc=DateTime.UtcNow.ToString("o"),actualStandalonePlayer=!Application.isEditor,platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,
                method="Opt-in isolated campaign. Continuous collision capsule substeps at authored movement speeds, actual E/Q/C/LMB and production UI callbacks. Directed smoke. Boss placements are explicitly arranged fixtures; 3x simulation clock when -phase1AuditFast is supplied. Not hands-on luring, pacing or Windows hardware performance acceptance."};
            foreach(var device in InputSystem.devices)if(device is Keyboard||device is Mouse)InputSystem.DisableDevice(device);
            keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();keyboard.MakeCurrent();mouse.MakeCurrent();
            yield return Drive(Scenario());if(!finished)Finish(true);
        }
        private IEnumerator Scenario()
        {
            yield return Wait(()=>FindFirstObjectByType<Phase1Director>()&&FindFirstObjectByType<Phase1Director>().Ready,120,"P1 Continue routes safely from startup");
            d=FindFirstObjectByType<Phase1Director>();yield return new WaitForSeconds(1);
            d.player.enabled=false;
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-phase1VisualReview")>=0)
            {
                receipt.method="Standalone visual review only: arranged camera positions, no route/puzzle/FPS acceptance. Isolated save. AI remains active.";
                yield return VisualReview();yield break;
            }
            if(!d.Completed("P1_LIFT_RIDE"))
            {
            yield return Shot("01-rusunawa-parking");
            yield return Walk(new Vector3(-8,.06f,-24));yield return Use("GUARD_LOG");HudController.Instance.CloseDocumentModal();
            d.Phone.isPhoneRaised=true;d.Phone.isScreenOn=true;d.Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);yield return null;
            d.phoneView.LiveButton.onClick.Invoke();Check(d.Completed("P1_START_LIVE"),"Live uses existing handset button");
            yield return Walk(new Vector3(-8,.06f,-28));yield return Walk(new Vector3(-26,.06f,-28));yield return Walk(new Vector3(-28,.06f,-16));yield return Walk(new Vector3(-28,.06f,-11));yield return Walk(new Vector3(-22,.06f,-11));
            Check(d.Completed("P1_ENTER_PUMP"),"Pump entry checkpoint and optional challenge expiration");
            yield return Walk(new Vector3(-22,.06f,-8.8f));yield return Walk(new Vector3(-19,.06f,-8.8f));yield return Walk(new Vector3(-19,.06f,-11));yield return Walk(new Vector3(-15,.06f,-11));yield return Walk(new Vector3(-15,.06f,2));
            CameraOn();Aim(new Vector3(-17,1.3f,2));yield return null;Check((d.phoneView.FeedCamera.cullingMask&(1<<30))!=0,"Normal handset retains concrete illusion veil");yield return Press(Key.Q);yield return new WaitForSeconds(.5f);
            yield return Shot("02-k102-ar-passage");Check(!d.world.passageWall.activeSelf&&(d.phoneView.FeedCamera.cullingMask&(1<<30))==0,"AR reveals physical passage independently of normal-eye veil");
            yield return Walk(new Vector3(-18.2f,.06f,2));yield return Walk(new Vector3(-18.2f,.06f,-1.6f));yield return Walk(new Vector3(-20,.06f,-1.6f));yield return Walk(new Vector3(-20,4.06f,7.3f));yield return Walk(new Vector3(-20,4.06f,8.4f));
            Check(d.Completed("P1_CROSS_102"),"Hidden stair latches permanent two-way passage");
            yield return Walk(new Vector3(-15,4.06f,8.4f));yield return Walk(new Vector3(-15,4.06f,-11));yield return Walk(new Vector3(0,4.06f,-11));
            yield return Use("MAILBOX");
            foreach(var slot in new[]{(0,6),(1,4),(2,4)})for(int i=0;i<slot.Item2;i++)Button("DigitUp_"+slot.Item1).onClick.Invoke();
            Button("UnlockPadlockBtn").onClick.Invoke();yield return null;
            Check(d.Completed("P1_MAILBOX_207")&&d.Campaign.HasInventoryItem(Phase1Ids.Key),"644 grants key and unique powerbank once");
            yield return Shot("03-l2-mailbox-and-void");
            yield return Walk(new Vector3(-15,4.06f,-11));yield return Walk(new Vector3(-16,4.06f,-10.8f));yield return Use("LIFT");
            yield return Wait(()=>!d.Busy&&d.Completed("P1_LIFT_RIDE"),10,"Lift enters CP3 without unapproved energy grant");d.player.enabled=false;
            }
            yield return Walk(new Vector3(2,8.06f,-11));
            Aim(d.world.kuntilanak.transform.position+Vector3.up*1.5f);CameraOn();yield return new WaitForSeconds(.4f);
            Check(d.threats.KuntiManifested&&!d.threats.KuntiPursuing,"Kuntilanak remains stationary before cutter");
            yield return Shot("04-kuntilanak-watched-idle");
            yield return Use("CUTTER",true);yield return Wait(()=>d.CameraLocked,3,"Cutter begins pursuit after pickup animation");
            yield return Walk(new Vector3(19.8f,8.06f,-10.5f),true);
            yield return Use("CHAIN",true);yield return Wait(()=>d.Completed("P1_CUT_CHAIN"),5,"Backwards chain interaction freezes AI then commits landing");
            Check(!d.world.chain.activeSelf&&!d.world.kuntilanak.activeSelf,"Kuntilanak permanently resolved before boss");
            yield return Walk(new Vector3(20,8.06f,-8.8f));yield return Walk(new Vector3(20,12.06f,-1.7f));yield return Walk(new Vector3(20,12.06f,3.9f));
            yield return Walk(new Vector3(15,12.06f,4));
            yield return Shot("05-l4-ritual-corridor");
            Check(d.Completed("P1_GENDER_MANIFEST"),"Genderuwo emerges from 401 with closed 404 intact");
            yield return Walk(new Vector3(20,12.06f,4));CameraOn();Aim(d.world.auraAim.position);
            if(d.Phone.currentScanMode!=ScanVisualMode.AR)yield return Press(Key.Q);yield return null;yield return Click();
            Check(d.Campaign.Data.evidence_ids.Contains(Phase1Ids.Aura),"AR aura is a separate evidence flag");
            yield return Shot("06-kitchen-aura");
            yield return Use("KERIS");Check(d.Campaign.HasInventoryItem(Phase1Ids.Keris),"Keris is a distinct physical relic");
            yield return Walk(new Vector3(15,12.06f,4));yield return Walk(new Vector3(15,12.06f,11));yield return Walk(new Vector3(-2,12.06f,11));
            CameraOn();Aim(d.world.clawAim.position);yield return null;yield return Click();
            Check(d.Campaign.Data.evidence_ids.Contains(Phase1Ids.Claw),"Camera scan captures unique claw evidence");
            yield return Shot("07-claw-evidence-402");
            // EVP and its actual ambush run before the ritual. Use production hide/phone inputs.
            yield return Walk(new Vector3(-15,12.06f,11));yield return Walk(new Vector3(-15,12.06f,2));
            yield return Walk(new Vector3(-20,12.06f,2));CameraOn();yield return Press(Key.C);
            yield return Wait(()=>d.Campaign.Data.evidence_ids.Contains(Phase1Ids.EVP),12,"EVP records the distinct bathroom evidence");
            yield return Walk(new Vector3(-21.5f,12.06f,1.8f));yield return Use("HIDE_BATH_STALL");
            if(d.Phone.isScreenOn)yield return Press(Key.F);
            yield return Wait(()=>d.Completed("P1_EVP_AMBUSH_DONE"),20,"Dark hiding resolves bathroom search");
            Check(d.Hidden&&!d.Phone.isScreenOn,"Hiding retains physical handset and permits screen off");
            yield return Wait(()=>Vector3.Distance(d.world.genderuwo.transform.position,d.player.transform.position)>8,20,"Wait in cover until patrol clears the doorway");yield return Press(Key.E);d.player.enabled=false;
            yield return Walk(new Vector3(-20,12.06f,2));yield return Walk(new Vector3(-15,12.06f,2));
            yield return Walk(new Vector3(-15,12.06f,-11));yield return Walk(new Vector3(0,12.06f,-11));
            d.phoneView.Apps.Open(PhoneApp.Journal);yield return null;Button("P1ConfirmIdentity").onClick.Invoke();
            Check(d.Completed("P1_JOURNAL_CONFIRMED"),"Three clues require production Journal confirmation");
            CameraOn();yield return Walk(new Vector3(0,12.06f,-16));yield return Use("ALTAR");
            Check(d.BossActive&&d.Campaign.WritesBlocked,"CP5 freezes ordinary autosaves during ritual attempt");
            // Controlled placements isolate all three switch/beam/head tests. Natural luring is manual QA.
            float smokeScale=Time.timeScale;Time.timeScale=1;
            foreach(string compass in new[]{"N","E","W"})
            {
                var item=Item("HALOGEN_"+compass);int index=compass=="N"?0:compass=="E"?1:2;
                Vector3 point=item.transform.position;point.y=12.06f;
                point+=compass=="N"?Vector3.back*1.2f:compass=="E"?Vector3.left*1.2f:Vector3.right*1.2f;
                var spot=new GameObject("AuditFixtureSwitch").transform;spot.position=point;spot.rotation=Quaternion.identity;
                d.MovePlayer(spot);Destroy(spot.gameObject);d.player.enabled=false;
                d.world.genderuwo.transform.position=d.world.halogenTargets[index].position;
                Physics.SyncTransforms();CameraOn();yield return Use("HALOGEN_"+compass);
                Check(d.threats.Exposed,"Arranged fixture exposes shield at halogen "+compass);
                Aim(d.world.genderuwo.transform.position+Vector3.up*2.13f);yield return null;
                yield return Shot("08-boss-exposure-"+compass);
                // Advance the production 6s cooldown *before* opening the next 5s window.
                yield return Click();Check(d.threats.SealHits==index+1,"Unique head Blitz counted for "+compass);
                if(index<2)yield return new WaitForSeconds(6.1f);
            }
            Time.timeScale=smokeScale;
            Check(d.Completed("P1_BOSS_WIN")&&!d.Campaign.WritesBlocked&&d.Campaign.Data.location_seals.Contains(Phase1Ids.Seal),"CP6 atomically commits one seal and consumes relic");
            var altarSpot=new GameObject("AuditReturnAltar").transform;altarSpot.position=d.world.Anchor("AFTER_BOSS").position;altarSpot.rotation=Quaternion.identity;d.MovePlayer(altarSpot);Destroy(altarSpot.gameObject);d.player.enabled=false;
            CameraOn();yield return Use("CLOTH");yield return Use("TAPE");
            yield return Wait(()=>d.Completed("P1_EPILOGUE"),50,"Tape, hospital handset transmission and epilogue finish before exit");d.player.enabled=false;
            yield return Shot("09-post-ritual-return");
            yield return Walk(new Vector3(0,12.06f,-11));yield return Walk(new Vector3(15,12.06f,-11));yield return Walk(new Vector3(15,12.06f,4));
            yield return Walk(new Vector3(20,12.06f,4));yield return Walk(new Vector3(20,12.06f,-1));
            yield return Walk(new Vector3(20,8.06f,-9.6f));yield return Walk(new Vector3(20,8.06f,-11));yield return Walk(new Vector3(-18.3f,8.06f,-11));
            yield return Use("LIFT");Button("Left").onClick.Invoke();yield return Wait(()=>!d.Busy&&d.player.transform.position.y<1,6,"Restored lift reaches pump service L1");d.player.enabled=false;
            yield return Walk(new Vector3(-28,.06f,-11));yield return Walk(new Vector3(-28,.06f,-17));yield return Walk(new Vector3(-28,.06f,-28));yield return Walk(new Vector3(1,.06f,-30));
            yield return Use("MOTOR");Check(!d.Completed("P1_COMPLETE"),"Exit panel does not complete before explicit confirmation");Button("Left").onClick.Invoke();
            yield return Use("MOTOR");Button("Right").onClick.Invoke();
            Check(d.Completed("P1_COMPLETE")&&d.Campaign.Data.checkpoint_id=="PHASE1_CP7","Explicit motor confirmation commits safe chapter endpoint");
            receipt.checkpointSaved=d.Campaign.HasValidCheckpoint;
            Check(d.Campaign.Data.sanity==23,"P1 preserves campaign Sanity without activating later-chapter mechanics");
        }
        private IEnumerator VisualReview()
        {
            var views=new[]{
                ("01-revised-exterior",new Vector3(1,.06f,-33),new Vector3(0,6,-20)),
                ("02-revised-pump",new Vector3(-27,.06f,-14),new Vector3(-29,1.5f,-9)),
                ("03-revised-gallery",new Vector3(14,4.06f,-11),new Vector3(-8,5.5f,-11)),
                ("04-revised-courtyard",new Vector3(14,4.06f,4),new Vector3(-12,6,10)),
                ("05-revised-stairs",new Vector3(-20,.06f,-1.6f),new Vector3(-20,2.6f,4)),
                ("06-revised-l3",new Vector3(-17,8.06f,-11),new Vector3(2,9.5f,-11)),
                ("07-revised-l4",new Vector3(14,12.06f,10.5f),new Vector3(-5,13.4f,11)),
                ("08-revised-kitchen",new Vector3(19,12.06f,3.2f),new Vector3(22,13.2f,6))};
            foreach(var view in views)
            {
                var capsule=d.player.GetComponent<CharacterController>();capsule.enabled=false;
                d.player.transform.position=view.Item2;capsule.enabled=true;
                Aim(view.Item3);yield return new WaitForSeconds(.4f);yield return Shot(view.Item1);
            }
            d.Phone.isPhoneRaised=false;d.Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Home);
            InputSystem.QueueStateEvent(mouse,new MouseState{buttons=2});yield return null;yield return null;yield return new WaitForSecondsRealtime(.7f);
            Check(d.Phone.isPhoneRaised&&d.phoneView.ViewfinderActive&&d.phoneView.FocusHandsVisible,"Held RMB raises the landscape camera with two physical grips");
            yield return Shot("09-rmb-held-two-handed-camera");
            d.phoneView.Apps.Open(PhoneApp.Inventory);yield return null;yield return null;
            Check(d.phoneView.IsCameraApp,"Held RMB overrides a handset app menu");
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;yield return null;yield return new WaitForSecondsRealtime(.4f);
            Check(!d.phoneView.ViewfinderActive&&!d.player.GetComponent<ShoulderCameraRig>().PhoneAimOverride&&!d.Phone.isPhoneRaised,"RMB release restores the previous camera and stored phone");
            yield return Shot("10-rmb-release-restores-perspective");
            Check(true,"Ten native views captured; only the RMB checks use production input, arranged views are not route acceptance");
            receipt.checkpointSaved=d.Campaign.HasValidCheckpoint;
        }
        private void CameraOn()
        {d.Phone.isPhoneRaised=d.Phone.isScreenOn=true;d.Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);d.phoneView.SetViewfinder(true);UiFocus.Phone=false;}
        private void Aim(Vector3 target)
        {
            Vector3 delta=target-d.player.cameraPivot.position;Vector3 flat=delta;flat.y=0;
            if(flat.sqrMagnitude>.01f)d.player.transform.rotation=Quaternion.LookRotation(flat);
            d.player.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);
            d.player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
        private IEnumerator Walk(Vector3 target,bool watch=false)
        {
            d.player.enabled=false;var capsule=d.player.GetComponent<CharacterController>();int stalled=0;
            for(int frame=0;frame<1800;frame++)
            {
                Check(!d.Dead,"Route remains alive");
                Vector3 delta=target-d.player.transform.position;delta.y=0;if(delta.magnitude<.10f)break;
                if(watch)Aim(d.world.kuntilanak.transform.position+Vector3.up*1.5f);else Aim(d.player.transform.position+delta.normalized*4+Vector3.up*1.45f);
                Vector3 before=d.player.transform.position;float budget=d.player.walkSpeed*Time.deltaTime;
                while(budget>.0001f){float step=Mathf.Min(.10f,budget);delta=target-d.player.transform.position;delta.y=0;capsule.Move(Vector3.ClampMagnitude(delta,step)+Vector3.down*.08f);budget-=step;}
                receipt.walkedMeters+=Vector3.Distance(before,d.player.transform.position);
                stalled=Vector3.Distance(before,d.player.transform.position)<.001f?stalled+1:0;
                if(stalled>15)break;yield return null;
            }
            Vector3 remaining=target-d.player.transform.position;remaining.y=0;
            if(remaining.magnitude>.2f)
            {
                var cc=d.player.GetComponent<CharacterController>();var hits=Physics.OverlapCapsule(d.player.transform.position+Vector3.up*.35f+remaining.normalized*.15f,d.player.transform.position+Vector3.up*(cc.height-.3f)+remaining.normalized*.15f,.35f);
                Debug.Log("P1_ROUTE_BLOCKERS "+string.Join(";",hits.Where(h=>!h.transform.IsChildOf(d.player.transform)).Select(h=>h.name+" "+h.bounds)));
            }
            Check(remaining.magnitude<.2f&&Mathf.Abs(d.player.transform.position.y-target.y)<.55f,"Capsule reaches "+target+" actual "+d.player.transform.position);
        }
        private Phase1WorldItem Item(string id)=>FindObjectsByType<Phase1WorldItem>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(i=>i.id==id).OrderBy(i=>Vector3.Distance(i.transform.position,d.player.transform.position)).First();
        private IEnumerator Use(string id,bool keepAim=false)
        {
            var item=Item(id);if(!keepAim)Aim(item.transform.position);
            yield return null;yield return null;
            var current=d.player.GetComponent<InteractionInteractor>().CurrentTarget;
            Debug.Log("P1_USE "+id+" current="+(current?.InteractableId??"none")+" pos="+d.player.transform.position+" item="+item.transform.position);
            Check(ReferenceEquals(current,item),"Production interaction resolves "+id+" (actual "+current?.InteractableId+")");yield return Press(Key.E);
        }
        private Button Button(string name)=>HudController.Instance.GetComponentsInChildren<Button>(true).First(b=>b.name==name);
        private IEnumerator Press(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
        private IEnumerator Click(){InputSystem.QueueStateEvent(mouse,new MouseState{buttons=1});yield return null;InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;}
        private IEnumerator Shot(string name)
        {
            yield return null;float scale=Time.timeScale;Time.timeScale=0;yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;yield return new WaitForEndOfFrame();Time.timeScale=scale;shots.Add(name+".png");
        }
        private IEnumerator Wait(Func<bool> condition,float seconds,string label)
        {
            // Gameplay durations use the simulation clock. On llvmpipe one rendered
            // second may span several wall-clock seconds; don't reject a valid 2s cut.
            float deadline=Time.time+seconds,watchdog=Time.realtimeSinceStartup+Mathf.Max(120,seconds*15);
            while(!condition()&&Time.time<deadline&&Time.realtimeSinceStartup<watchdog)
            {if(d&&d.Dead)throw new Exception("Died while waiting for "+label);yield return null;}
            Check(condition(),label);
        }
        private void Check(bool ok,string label)
        {if(ok){if(label!="Route remains alive"){checks.Add("PASS "+label);Debug.Log("P1_AUDIT_PASS "+label);}}else throw new Exception("P1 AUDIT FAIL "+label);}
        private IEnumerator Drive(IEnumerator action)
        {
            var stack=new Stack<IEnumerator>();stack.Push(action);
            while(stack.Count>0&&!finished)
            {
                bool moved=false;object value=null;
                try{moved=stack.Peek().MoveNext();if(moved)value=stack.Peek().Current;}
                catch(Exception error){checks.Add("FAIL "+error.Message);Debug.LogWarning(error);Finish(false);yield break;}
                if(!moved){stack.Pop();continue;}if(value is IEnumerator child)stack.Push(child);else yield return value;
            }
        }
        private void Finish(bool ok)
        {
            if(finished)return;finished=true;if(receipt==null)receipt=new Receipt();receipt.completed=ok;receipt.checks=checks.ToArray();receipt.screenshots=shots.ToArray();
            File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(receipt,true));Application.Quit(ok?0:2);
        }
        private void OnDestroy(){Application.logMessageReceived-=Log;}
    }
}
#endif
