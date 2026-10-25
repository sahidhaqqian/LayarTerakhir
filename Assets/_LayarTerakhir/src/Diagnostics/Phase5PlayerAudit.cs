#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase5;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
namespace LayarTerakhir.Diagnostics
{
    [DefaultExecutionOrder(-10000)]
    public sealed class Phase5PlayerAudit : MonoBehaviour
    {
        private static string output, final, resumeSource;
        private static bool finalFixture;
        private Phase5Director d;
        private Keyboard keyboard; private Mouse mouse;
        private readonly List<string> checks=new List<string>(),screens=new List<string>();
        private bool done, raised, expectedDeath; private float started, walked, trace;
        [Serializable] private class Result {public bool completed,standalone,rendered;public string utc,platform,gpu,method,resumedFrom;public float walkedMeters;public string[] checks,screenshots;}
        private static string Arg(string key){var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,key);return i>=0&&i+1<a.Length?a[i+1]:null;}
        private static bool Has(string key)=>Array.IndexOf(Environment.GetCommandLineArgs(),key)>=0;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] private static void Install()
        {
            output=Arg("-phase5Audit");if(output==null)return;output=Path.GetFullPath(output);Directory.CreateDirectory(output);final=Arg("-phase5Final");finalFixture=final!=null;
            string folder=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(folder);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            resumeSource=Arg("-phase5Resume");
            var fixture=resumeSource!=null?JsonUtility.FromJson<CampaignData>(File.ReadAllText(resumeSource)):Phase5ReviewFixture.Boundary();
            if(resumeSource==null)
            {fixture.battery=fixture.sanity=0;fixture.wallet_legitimate=0;fixture.inventory.RemoveAll(id=>Phase5Ids.IsFlare(id)||Phase5Ids.IsIncense(id)||id.Contains("POWERBANK"));}
            if(final=="SECRET")fixture.secret_items=Phase1Ids.Secrets.ToList();
            File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"),JsonUtility.ToJson(fixture,true));
            var go=new GameObject("Phase5PlayerAudit");DontDestroyOnLoad(go);go.AddComponent<Phase5PlayerAudit>();
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.targetFrameRate=60;Application.logMessageReceived+=Log;}
        private void Log(string msg,string stack,LogType kind){if(!done&&(kind==LogType.Error||kind==LogType.Exception)){checks.Add("ERROR "+msg);Finish(false);}}
        private void Update()
        {
            if(d&&d.Ready&&Time.realtimeSinceStartup>trace){trace=Time.realtimeSinceStartup+5;Debug.Log("P5_TRACE zone="+d.Zone+" player="+d.player.transform.position+" core="+Phase5Ids.CoreCount(d.Campaign.Data)+" boss="+d.world.bossRoot.transform.position+" exposed="+d.boss.Exposed+" source="+(d.SelectedSource?d.SelectedSource.sourceId:"none")+" scan="+d.CameraCanScan+" battery="+d.Phone.batteryPercent);}
            if(!done&&d&&d.Dead&&!expectedDeath){checks.Add("FAIL unexpected player death; committed checkpoint retained");Finish(false);}
            if(!done&&Time.realtimeSinceStartup-started>2400){checks.Add("FAIL timeout");Finish(false);}
        }
        private IEnumerator Start()
        {
            foreach(var dev in InputSystem.devices.ToArray())if(dev is Keyboard||dev is Mouse)InputSystem.DisableDevice(dev);
            keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();keyboard.MakeCurrent();mouse.MakeCurrent();
            yield return Drive(Route());if(!done)Finish(true);
        }
        private IEnumerator Drive(IEnumerator routine)
        {
            while(!done){bool more;object current=null;try{more=routine.MoveNext();if(more)current=routine.Current;}catch(Exception e){checks.Add("FAIL "+e);Finish(false);yield break;}if(!more)yield break;if(current is IEnumerator nested)yield return Drive(nested);else yield return current;}
        }
        private void Check(bool ok,string label){checks.Add((ok?"PASS ":"FAIL ")+label);Debug.Log("P5_AUDIT "+checks.Last());if(!ok)throw new Exception(label);}
        private IEnumerator Wait(Func<bool> test,float seconds,string label){float end=Time.realtimeSinceStartup+seconds;while(!test()&&!done&&Time.realtimeSinceStartup<end)yield return null;Check(test(),label);}
        private IEnumerator Ready()
        {yield return Wait(()=>FindFirstObjectByType<Phase5Director>()&&FindFirstObjectByType<Phase5Director>().Ready,240,"Pasar Setan restored from committed checkpoint");d=FindFirstObjectByType<Phase5Director>();yield return new WaitForSeconds(.3f);}
        private IEnumerator Route()
        {
            var c=CampaignState.Instance;Check(c&&c.LoadCheckpoint(),"Actual Phase4 CP8 fixture loads");
            Check(Phase5Ids.Eligible(c.Data)||resumeSource!=null&&CampaignRouting.IsPhase5(c.Data),"Valid Phase4 boundary or explicitly resumed prior native checkpoint");
            if(final!=null){c.Data.battery=100;Phase5ReviewFixture.Prepare(c,true);c.Data.battery=0;Check(c.SaveCheckpointAtomic(c.Data.checkpoint_id),"Final-choice isolated fixture persists zero battery");}
            GameShell.Instance.Continue();yield return Ready();c=d.Campaign;
            if(Has("-phase5Visual")){yield return Visual();yield break;}
            if(final!=null){yield return FinalRoute();yield break;}
            if(d.Completed("P5_ARENA_LOCK")){yield return BossRoute();final="NORMAL";yield return FinalRoute();yield break;}
            if(!d.Completed("P5_S2_CLEAR"))
            {
            Check(c.Data.battery==0&&c.Data.sanity==0&&c.Data.wallet_legitimate==0,"Exact zero-resource carry-over without forced refill or Kos visit");
            yield return Walk(new Vector3(-4,0,5.2f));yield return Use("DYNAMO_GATE",8.3f);Check(d.Phone.batteryPercent>24,"Real held dynamo restores phone from zero");
            yield return Walk(new Vector3(4,0,5.2f));yield return Use("PRAYER_GATE",10.3f);Check(c.Data.sanity>=14,"Free physical prayer works at zero Sanity");
            yield return Walk(new Vector3(7.5f,0,13.6f));yield return Camera();yield return Press(GameAction.ToggleScreen);yield return new WaitForSeconds(3.3f);
            Check(d.Completed("P5_SECRET_CHAIR_SILENCE"),"Actual screen-off silence opens tape puzzle");yield return Press(GameAction.ToggleScreen);yield return Camera(ScanVisualMode.AR);yield return Aim(d.world.drawerFocus.position,true);yield return new WaitForSeconds(1.3f);
            Check(d.Completed("P5_SECRET_AR_DRAWER"),"Actual AR feed reveals drawer");yield return Aim(d.world.sources.First(s=>s.sourceId=="P5_TAPE_TRUE").transform.position,true);yield return Hold(GameAction.ModeEVP,6.5f);
            Check(d.Completed("P5_SECRET_EVP_TRUE"),"Held EVP authentic tape receipt");yield return Use("TAPE");Check(c.Data.secret_items.Contains(Phase5Ids.Secret),"Tape saved before success notification");
            yield return Walk(new Vector3(0,0,50.5f));yield return Use("START_SECTORS");yield return Walk(new Vector3(0,0,88.4f));yield return Camera();yield return Press(GameAction.ModeThermo);yield return Aim(d.world.doors[1].position,true);yield return new WaitForSeconds(2.3f);Check(d.ThermalReadout.Contains("22"),"True frozen door has actual Thermo reading");yield return Use("DOOR_1");
            yield return Walk(new Vector3(6,0,143));yield return Walk(new Vector3(10,0,143.7f));yield return Use("REFILL",4.3f);yield return Walk(new Vector3(6,0,148));yield return Use("POUR");Check(d.Completed("P5_BRIDGE_TORCH_OFF"),"Existing Kendi and free water clear bridge");
            }
            if(!d.Completed("P5_S3_CLEAR"))
            {
            if(d.Phone.isScreenOn)yield return Press(GameAction.ToggleScreen);
            yield return Walk(new Vector3(5.5f,0,184),true);yield return Walk(new Vector3(-5.5f,0,184),true);yield return Walk(new Vector3(-5.5f,0,156),true);yield return Walk(new Vector3(-8,0,156));yield return Camera(ScanVisualMode.AR);yield return Use("SEWING_PORTAL");yield return Wait(()=>d.Completed("P5_S3_CLEAR")&&!d.ActionBusy,10,"Physical AR mirror crosses to auction stage");
            }
            if(!c.Data.evidence_ids.Contains(Phase5Ids.Contract))
            {
            yield return Walk(new Vector3(-8,0,209.7f));yield return Camera();if(!d.ThermoActive)yield return Press(GameAction.ModeThermo);yield return Aim(d.world.contract.position,true);yield return new WaitForSeconds(2.3f);yield return Camera(ScanVisualMode.UV);yield return Aim(d.world.contract.position,true);yield return new WaitForSeconds(2.3f);Check(c.Data.evidence_ids.Contains(Phase5Ids.Contract),"Contract needs both Thermo and UV");
            }
            if(!c.Data.evidence_ids.Contains(Phase5Ids.Server))
            {
            yield return Walk(new Vector3(0,0,218.8f));yield return Camera(ScanVisualMode.AR);yield return Aim(d.world.server.position,true);yield return new WaitForSeconds(1.4f);yield return Click();Check(c.Data.evidence_ids.Contains(Phase5Ids.Server),"AR server evidence uses actual feed capture");
            }
            yield return Walk(new Vector3(9,0,209.7f));yield return Camera();yield return Aim(d.world.crowd.position,true);
            Debug.Log("P5_EVP_DIAGNOSTIC scan="+d.CameraCanScan+" aim="+d.Aim(d.world.crowd,15,35)+" distance="+Vector3.Distance(d.player.transform.position+Vector3.up,d.world.crowd.position)+" source="+(d.SelectedSource?d.SelectedSource.sourceId:"none"));
            yield return Hold(GameAction.ModeEVP,6.5f);Check(Phase5Ids.AllEvidence(c.Data),"Three independent proofs gathered by physical route");
            raised=false;MouseState(Vector2.zero);yield return null;yield return null;yield return Press(GameAction.OpenJournal);yield return null;
            var confirm=FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).First(b=>b.name=="P5Confirm");confirm.onClick.Invoke();
            Check(d.Completed("P5_JOURNAL_CONFIRMED"),"Existing phone journal confirms clues separately");d.phoneView.Apps.Stow();
            yield return Walk(new Vector3(0,0,206.2f));yield return Use("START_ARENA");
            var button=FindObjectsByType<UnityEngine.UI.Button>(FindObjectsSortMode.None).FirstOrDefault(b=>b.name=="Begin");Check(button,"Arena warning has explicit confirm button");button.onClick.Invoke();yield return null;Check(d.Completed("P5_ARENA_LOCK"),"CP6 locks backtracking only after explicit warning");
            yield return BossRoute();final="NORMAL";yield return FinalRoute();
        }
        private IEnumerator BossRoute()
        {
            if(Phase5Ids.CoreCount(d.Campaign.Data)==0)
            {
            yield return ArenaRecovery(false);
            yield return Walk(new Vector3(-9,0,218.2f),true);yield return Wait(()=>Vector3.Distance(d.world.bossRoot.transform.position,d.world.generator.position)<5.5f,35,"Wujud asli physically enters generator zone");yield return Use("GENERATOR");yield return Camera();yield return Aim(d.world.bossHead.position,true);yield return Click();Check(Phase5Ids.CoreCount(d.Campaign.Data)==1,"One actual Blitz breaks first core");
            yield return Reload();Check(Phase5Ids.CoreCount(d.Campaign.Data)==1,"Core1 survives real scene reload");
            }
            if(Phase5Ids.CoreCount(d.Campaign.Data)==1)
            {
            yield return Walk(new Vector3(-11,0,212.5f),true);yield return Use("LANTERN_0");yield return Camera();yield return Press(GameAction.ModeSelfie);if(!d.ThermoActive)yield return Press(GameAction.ModeThermo);yield return Wait(()=>d.boss.RearCold,35,"Real threat reaches cold rear range before deploying light");yield return Press(GameAction.UseFlare);
            yield return Wait(()=>d.boss.Exposed,30,"Reusable lantern and Selfie Thermo expose second core without Flare");yield return Aim(d.world.bossHead.position,true);yield return Click();Check(Phase5Ids.CoreCount(d.Campaign.Data)==2,"Actual Selfie Blitz breaks second core");
            expectedDeath=true;yield return Wait(()=>d.Dead,32,"Actual exposed phase3 wave causes death without cover");
            yield return Reload();expectedDeath=false;Check(Phase5Ids.CoreCount(d.Campaign.Data)==2,"Death and Continue in phase3 retain both previous cores");
            }
            yield return Walk(new Vector3(-6,0,210));raised=false;MouseState(Vector2.zero);yield return Press(GameAction.ToggleScreen);yield return Hold(GameAction.Crouch,48);
            for(int i=1;i<=3;i++){if(d.Completed("P5_CABLE_"+i+"_CUT"))continue;yield return Walk(d.world.cableTargets[i-1].position+Vector3.back*1.7f);yield return Camera(ScanVisualMode.AR);yield return Use("CABLE_"+i,2.3f);Check(d.Completed("P5_CABLE_"+i+"_CUT"),"Cable"+i+" commits CP7 independently");}
            yield return Reload();Check(Phase5Ids.CableCount(d.Campaign.Data)==3&&Phase5Ids.CoreCount(d.Campaign.Data)==2,"All cables persist without replaying previous cores");
            yield return ArenaRecovery(true);yield return Walk(new Vector3(0,0,218));yield return Camera();yield return Wait(()=>d.boss.Exposed,25,"Final core window retries without resetting cables");yield return Aim(d.world.bossChest.position,true);yield return Click();yield return Wait(()=>d.FinalChoice,10,"Third Blitz leads to saved FINAL_CHOICE CP8");
        }
        private IEnumerator ArenaRecovery(bool east)
        {
            yield return Walk(east?new Vector3(13.5f,0,232.5f):new Vector3(-14.5f,0,216.4f),true);
            Vector3 direction=east?Vector3.back:Vector3.forward;
            yield return Aim(d.player.cameraPivot.position+direction*5,false);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));yield return new WaitForSeconds(.5f);
            Check(d.player.IsCrouching,"Actual crouch fits physical recovery canopy");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl,Key.W));yield return new WaitForSeconds(2.1f);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl));yield return null;
            yield return Use(east?"DYNAMO_ARENA_E":"DYNAMO_ARENA_W",8.3f);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftCtrl,Key.S));yield return new WaitForSeconds(2.1f);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
        }
        private IEnumerator FinalRoute()
        {
            var c=d.Campaign;Check(d.FinalChoice&&c.Data.ending==EndingId.NONE,"Only FINAL_CHOICE permits an ending");float battery=c.Data.battery;
            if(final=="NORMAL")
            {yield return Walk(d.world.edge.position+Vector3.right);yield return Hold(GameAction.ToggleScreen,4.6f);Check(c.Data.ending==EndingId.NONE,"Normal hold released before five seconds cancels");yield return Hold(GameAction.ToggleScreen,5.3f);}
            else if(final=="BAD")
            {yield return Walk(new Vector3(0,0,216));yield return Camera();yield return Aim(d.world.damarFocus.position,true);yield return Click();Check(c.Data.ending==EndingId.NONE,"Bad rejects premature LMB before two seconds");yield return new WaitForSeconds(2.8f);yield return Click();}
            else
            {
                yield return Walk(new Vector3(0,0,218));yield return Use("DAMAR");yield return Walk(new Vector3(12,0,220));yield return Wait(()=>d.Completed("P5_DAMAR_AT_PORTAL"),8,"Damar physically escorted to portal");
                if(final=="SECRET")
                {yield return Walk(new Vector3(15,0,220));yield return Use("ARCHIVE",3.3f);Check(c.Data.archive_prepared&&c.Data.ending==EndingId.NONE,"Five archive validation does not select Secret");yield return Camera();yield return Aim(d.world.archiveMirror.position,true);yield return Click();}
                else
                {yield return Camera();yield return Aim(d.world.bossHead.position-Vector3.right*28,false);yield return Press(GameAction.ModeSelfie);yield return Press(GameAction.ModeAR);yield return new WaitForSeconds(1.8f);yield return Press(GameAction.ModeUV);yield return new WaitForSeconds(1.8f);yield return Aim(d.world.bossHead.position,true);yield return Click();}
            }
            Check(c.Data.ending.ToString()==final,"Requested ending locked before cutscene");Check(c.Data.battery==battery,"Ending capacitor never changes persistent phone battery");
            var chosen=c.Data.ending;Check(!Phase5Campaign.LockEnding(c,EndingId.BAD),"A second ending input cannot overwrite the lock");
            SceneManager.LoadScene(CampaignRouting.MarketScene);yield return null;yield return Ready();Check(d.Campaign.Data.ending==chosen,"Crash-style Continue replays the same locked ending");
            yield return Wait(()=>d.Completed("P5_COMPLETE"),150,"Cutscene completion commits ending CP9");Check(d.Campaign.Data.checkpoint_id=="PHASE5_CP9","Final ending persists in CP9");
            File.WriteAllText(Path.Combine(output,"final-save.json"),JsonUtility.ToJson(d.Campaign.Data,true));
        }
        private IEnumerator Reload(){raised=false;MouseState(Vector2.zero);GameShell.Instance.Continue();yield return null;yield return Ready();}
        private void MouseState(Vector2 delta,bool click=false){var state=new UnityEngine.InputSystem.LowLevel.MouseState{delta=delta};if(raised)state=state.WithButton(MouseButton.Right);if(click)state=state.WithButton(MouseButton.Left);InputSystem.QueueStateEvent(mouse,state);}
        private IEnumerator Aim(Vector3 target,bool lens)
        {
            for(int i=0;i<36;i++)
            {
                var cam=lens?d.phoneView.FeedCamera:d.player.GetComponent<InteractionInteractor>().playerCamera;
                Vector3 ray=target-cam.transform.position;float yaw=Vector3.SignedAngle(Vector3.ProjectOnPlane(cam.transform.forward,Vector3.up),Vector3.ProjectOnPlane(ray,Vector3.up),Vector3.up);
                float pitch=-Mathf.Atan2(ray.y,new Vector2(ray.x,ray.z).magnitude)*Mathf.Rad2Deg;float current=Mathf.DeltaAngle(0,d.player.cameraPivot.localEulerAngles.x);
                Vector2 angles=new Vector2(Mathf.Clamp(yaw*.6f,-20,20),Mathf.Clamp((current-pitch)*.6f,-15,15));
                MouseState(angles/(GameInputConfig.Instance.mouseSensitivity*.1f));yield return null;
                if(Mathf.Abs(yaw)<.1f&&Mathf.Abs(current-pitch)<.1f)break;
            }
            MouseState(Vector2.zero);yield return null;yield return null;
        }
        private IEnumerator Walk(Vector3 target,bool sprint=false)
        {
            raised=false;MouseState(Vector2.zero);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;yield return null;
            if(d.Phone.isPhoneRaised)yield return Press(GameAction.TogglePhone);
            Check(NavMesh.SamplePosition(d.player.transform.position,out var a,2,NavMesh.AllAreas)&&NavMesh.SamplePosition(target,out _,2,NavMesh.AllAreas),"Nav endpoints exist "+target);
            NavMesh.SamplePosition(target,out var b,2,NavMesh.AllAreas);var path=new NavMeshPath();NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path);Check(path.status==NavMeshPathStatus.PathComplete,"Connected physical path "+target);
            foreach(var corner in path.corners.Skip(1))
            {
                Vector3 flat=corner;flat.y=d.player.cameraPivot.position.y;yield return Aim(flat,false);float end=Time.realtimeSinceStartup+100,stuck=0;Vector3 previous=d.player.transform.position;
                while(Vector2.Distance(new Vector2(d.player.transform.position.x,d.player.transform.position.z),new Vector2(corner.x,corner.z))>.3f)
                {
                    if(d.Dead||Time.realtimeSinceStartup>end)throw new Exception("Caught or stalled before "+corner+" at "+d.player.transform.position);
                    Vector3 ray=corner-d.player.transform.position;ray.y=0;float yaw=Vector3.SignedAngle(d.player.transform.forward,ray,Vector3.up);
                    MouseState(new Vector2(Mathf.Clamp(yaw*.65f,-18,18)/(GameInputConfig.Instance.mouseSensitivity*.1f),0));InputSystem.QueueStateEvent(keyboard,sprint?new KeyboardState(Key.W,Key.LeftShift):new KeyboardState(Key.W));
                    float distance=Vector3.Distance(previous,d.player.transform.position);walked+=distance;stuck=distance<.002f?stuck+Time.unscaledDeltaTime:0;previous=d.player.transform.position;
                    if(stuck>4)throw new Exception("Physical player capsule blocked at "+previous+" toward "+corner);
                    yield return null;
                }
                InputSystem.QueueStateEvent(keyboard,new KeyboardState());MouseState(Vector2.zero);yield return new WaitForSeconds(.2f);
            }
        }
        private IEnumerator Camera(ScanVisualMode mode=ScanVisualMode.Normal)
        {
            // F intentionally suppresses held RMB until release. Wake the screen
            // while RMB is released, then make a fresh hold like the player.
            raised=false;MouseState(Vector2.zero);yield return null;yield return null;
            if(!d.Phone.isScreenOn)yield return Press(GameAction.ToggleScreen);
            d.phoneView.Apps.Stow();yield return null;
            raised=true;MouseState(Vector2.zero);yield return null;yield return null;
            if(d.Phone.isSelfieCamera)yield return Press(GameAction.ModeSelfie);
            if(d.Phone.currentScanMode!=mode)yield return Press(mode==ScanVisualMode.UV?GameAction.ModeUV:mode==ScanVisualMode.AR?GameAction.ModeAR:d.Phone.currentScanMode==ScanVisualMode.UV?GameAction.ModeUV:GameAction.ModeAR);
            yield return new WaitForSeconds(.25f);
            Check(d.CameraCanScan,"Fresh RMB hold opens powered camera after screen-off");
        }
        private IEnumerator Use(string id,float hold=0)
        {
            var item=FindObjectsByType<Phase5WorldItem>(FindObjectsSortMode.None).Where(i=>i.id==id).OrderBy(i=>Vector3.Distance(i.transform.position,d.player.transform.position)).FirstOrDefault();Check(item,"Physical interaction exists "+id);
            if(Vector3.Distance(d.player.transform.position+Vector3.up,item.transform.position)>2.4f)
            {
                bool wasRaised=raised;Vector3? stance=null;float best=float.MaxValue;
                NavMesh.SamplePosition(d.player.transform.position,out var from,2,NavMesh.AllAreas);
                for(int i=0;i<72;i++)
                {
                    float angle=(i%24)*Mathf.PI*2/24;float radius=1.5f+(i/24)*.4f;Vector3 probe=item.transform.position+new Vector3(Mathf.Sin(angle)*radius,-item.transform.position.y,Mathf.Cos(angle)*radius);
                    if(!NavMesh.SamplePosition(probe,out var at,.3f,NavMesh.AllAreas)||Mathf.Abs(at.position.y-d.player.transform.position.y)>.4f||!d.ClearLine(at.position+Vector3.up*1.3f,item.transform.position))continue;
                    var path=new NavMeshPath();NavMesh.CalculatePath(from.position,at.position,NavMesh.AllAreas,path);if(path.status!=NavMeshPathStatus.PathComplete)continue;
                    float score=0;for(int j=1;j<path.corners.Length;j++)score+=Vector3.Distance(path.corners[j-1],path.corners[j]);
                    if(score<best){best=score;stance=at.position;}
                }
                Check(stance.HasValue,"Reachable nearby physical interaction stance "+id);
                yield return Walk(stance.Value);if(wasRaised){raised=true;MouseState(Vector2.zero);yield return new WaitForSeconds(.3f);}
            }
            yield return Aim(item.transform.position,false);yield return new WaitForSeconds(.2f);var interactor=d.player.GetComponent<InteractionInteractor>();
            if(interactor.CurrentTarget?.InteractableId!=id)Debug.Log("P5_TARGET_DIAGNOSTIC "+id+" actual="+interactor.CurrentTarget?.InteractableId+" player="+d.player.transform.position+" point="+item.transform.position+" hits="+string.Join(" / ",Physics.RaycastAll(interactor.playerCamera.transform.position,interactor.playerCamera.transform.forward,5,~0,QueryTriggerInteraction.Collide).OrderBy(h=>h.distance).Select(h=>h.collider.name+":"+h.collider.GetComponentInParent<IInteractable>()?.InteractableId)));
            Check(interactor.CurrentTarget?.InteractableId==id,"Actual reticle acquires "+id);if(hold>0)yield return Hold(GameAction.Interact,hold);else yield return Press(GameAction.Interact);
        }
        private Key KeyFor(GameAction action){var k=GameInputConfig.Instance.GetKeyForAction(action);return (Key)Enum.Parse(typeof(Key),k==KeyCode.LeftControl?"LeftCtrl":k.ToString());}
        private IEnumerator Press(GameAction action){InputSystem.QueueStateEvent(keyboard,new KeyboardState(KeyFor(action)));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;yield return null;}
        private IEnumerator Hold(GameAction action,float seconds){InputSystem.QueueStateEvent(keyboard,new KeyboardState(KeyFor(action)));yield return new WaitForSeconds(seconds);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;yield return null;}
        private IEnumerator Click(){MouseState(Vector2.zero,true);yield return null;MouseState(Vector2.zero);yield return null;yield return null;}
        private IEnumerator Visual()
        {
            Check(SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null,"Graphical standalone composition renderer");d.enabled=false;d.boss.enabled=false;d.replicas.enabled=false;d.player.enabled=false;
            var shots=new[]{("01-market",new Vector3(0,.1f,5),new Vector3(0,2,28)),("02-archive-stall",new Vector3(4,.1f,11),new Vector3(8,1.4f,16)),("03-frozen-corridor",new Vector3(0,.1f,65),new Vector3(0,2,89)),("04-bamboo-bridge",new Vector3(6,.1f,102),new Vector3(6,1.3f,135)),("05-sewing",new Vector3(9,.1f,157),new Vector3(8,1.4f,179)),("06-auction",new Vector3(-5,.1f,204),new Vector3(0,3,224)),("07-witness",new Vector3(12,.1f,217),new Vector3(20,2,220))};
            foreach(var s in shots){var cc=d.player.GetComponent<CharacterController>();cc.enabled=false;d.player.transform.position=s.Item2;cc.enabled=true;var delta=s.Item3-d.player.cameraPivot.position;d.player.transform.rotation=Quaternion.LookRotation(Vector3.ProjectOnPlane(delta,Vector3.up));d.player.cameraPivot.localRotation=Quaternion.Euler(-Mathf.Atan2(delta.y,new Vector2(delta.x,delta.z).magnitude)*Mathf.Rad2Deg,0,0);d.player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();yield return new WaitForSeconds(.8f);yield return Screenshot(s.Item1);}
            d.world.bossRoot.SetActive(true);d.world.bossRoot.GetComponent<NavMeshAgent>().enabled=false;
            UiFocus.Cinematic=true;var rig=d.player.GetComponent<ShoulderCameraRig>();rig.CinematicVisible=false;
            var camera=d.phoneView.playerCamera;var origin=new Vector3(-7,2.1f,217);camera.transform.SetPositionAndRotation(origin,Quaternion.LookRotation(d.world.bossHead.position-origin));
            yield return new WaitForSeconds(.8f);yield return Screenshot("08-boss-posed");
            foreach(var ending in new[]{EndingId.BAD,EndingId.NORMAL,EndingId.TRUE,EndingId.SECRET})
            {
                var shot=d.endingShots.First(x=>x.ending==ending&&x.epilogueSet>=0);
                d.world.epilogueRoot.SetActive(true);for(int j=0;j<d.world.epilogueSets.Length;j++)d.world.epilogueSets[j].SetActive(j==shot.epilogueSet);
                d.GetComponent<Phase5Lighting>().Apply(shot.epilogueSet==1);
                camera.transform.SetPositionAndRotation(shot.from.position,Quaternion.LookRotation(shot.look.position-shot.from.position));
                yield return new WaitForSeconds(.8f);yield return Screenshot("09-epilogue-"+ending.ToString().ToLowerInvariant()+"-posed");
            }
        }
        private IEnumerator Screenshot(string name){yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;screens.Add(name+".png");}
        private void Finish(bool ok)
        {
            if(done)return;done=true;Application.logMessageReceived-=Log;
            File.WriteAllText(Path.Combine(output,"result.json"),JsonUtility.ToJson(new Result{completed=ok,standalone=!Application.isEditor,rendered=SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null,utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,walkedMeters=walked,resumedFrom=resumeSource,method=Has("-phase5Visual")?"Posed composition fixtures, not gameplay acceptance":finalFixture?"Prepared FINAL_CHOICE fixture; actual input, controller collision, cutscene restart and CP9 persistence":"Actual zero-resource Phase4 handoff; real keyboard/mouse locomotion, interactions, scanning and boss route",checks=checks.ToArray(),screenshots=screens.ToArray()},true)+"\n");Application.Quit(ok?0:1);
        }
    }
}
#endif
