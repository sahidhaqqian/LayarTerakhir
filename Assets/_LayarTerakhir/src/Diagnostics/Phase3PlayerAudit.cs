#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase3;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
namespace LayarTerakhir.Diagnostics
{
 public sealed partial class Phase3PlayerAudit:MonoBehaviour
 {
  private static string output;
  private Phase3Director d;
  private Keyboard keyboard;private Mouse mouse;
  private readonly List<string> checks=new List<string>(),screenshots=new List<string>();
  private bool done;private float started,walked,traceClock;
  [Serializable]private class Result{public bool completed,actualStandalonePlayer,rendered;public string utc,platform,gpu,method;public float walkedMeters,simulationTimeScale;public string[] checks,screenshots;}
  public static CampaignData ReviewFixture()
  {
   var data=new CampaignData{chapter_id=3,checkpoint_id="PHASE3_HUB",battery=37,sanity=23,wallet_legitimate=0,wallet_bound=0};
   data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","JAKET_RAKA","MATABATIN_APK"});
   data.location_seals.AddRange(new[]{Phase1Ids.Seal,Phase2Ids.Seal});data.phase1.Add("P1_COMPLETE");
   foreach(string id in new[]{"P2_DEPART_KOS","P2_BOSS_WIN","P2_EPILOGUE","P2_COMPLETE"})data.phase2.Add(id);
   data.upgrades.AddRange(new[]{"AR_FULL","SELFIE_MODE","THERMOKLENIK_APK"});data.phase2.next_destination=Phase2Ids.Destination;data.phase3.Add("P3_ARRIVE_KOS");return data;
  }
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]private static void Install()
  {
   var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-phase3Audit");bool review=Array.IndexOf(args,"-phase3Review")>=0;
   if(i<0&&!review)return;bool audit=i>=0&&i+1<args.Length;
   string save;
   if(audit){output=Path.GetFullPath(args[i+1]);Directory.CreateDirectory(output);save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));}
   else save=Path.Combine(Application.persistentDataPath,"Phase3Review");
   Directory.CreateDirectory(save);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
   string path=Path.Combine(save,"layar_terakhir_save.json");if(audit||!File.Exists(path)){var fixture=ReviewFixture();if(review&&!audit)fixture.wallet_legitimate=2100000;File.WriteAllText(path,JsonUtility.ToJson(fixture,true));}
   if(!audit)return;
   var go=new GameObject("Phase3PlayerAudit");DontDestroyOnLoad(go);go.AddComponent<Phase3PlayerAudit>();InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
  }
  private void Awake(){started=Time.realtimeSinceStartup;Application.targetFrameRate=60;if(Has("-phase3Fast"))Time.timeScale=2;Application.logMessageReceived+=Log;}
  private static bool Has(string arg)=>Array.IndexOf(Environment.GetCommandLineArgs(),arg)>=0;
  private void Log(string message,string trace,LogType kind){if(!done&&(kind==LogType.Error||kind==LogType.Exception)){checks.Add("ERROR "+message);Finish(false);}}
  private void Update(){
   if(d&&d.Ready&&Time.realtimeSinceStartup>=traceClock)
   {traceClock=Time.realtimeSinceStartup+2;Debug.Log("P3_TRACE room="+d.CurrentRoom+" p="+d.player.transform.position+" ghost="+d.world.wewe.transform.position+" state="+d.wewe.State+" scan="+d.CameraCanScan+" selected="+(d.SelectedSource?d.SelectedSource.sourceId:"none")+" tag="+d.SelectedTag+" record="+d.RecordingEVP+" progress="+d.EvpProgress+" battery="+d.Phone.batteryPercent+" clock="+d.ClockRunning);}
   if(!done&&Time.realtimeSinceStartup-started>1500){checks.Add("FAIL timeout");Finish(false);}}
  private IEnumerator Start()
  {
   foreach(var device in InputSystem.devices)if(device is Keyboard||device is Mouse)InputSystem.DisableDevice(device);
   keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();keyboard.MakeCurrent();mouse.MakeCurrent();
   yield return Drive(Route());if(!done)Finish(true);
  }
  private IEnumerator Drive(IEnumerator routine)
  {
   while(!done)
   {
    bool moved=false;object current=null;
    try{moved=routine.MoveNext();if(moved)current=routine.Current;}catch(Exception e){checks.Add("FAIL "+e);Finish(false);yield break;}
    if(!moved)yield break;if(current is IEnumerator child)yield return Drive(child);else yield return current;
   }
  }
  private IEnumerator Wait(Func<bool> predicate,float seconds,string label){float end=Time.realtimeSinceStartup+seconds;while(!predicate()&&!done&&Time.realtimeSinceStartup<end)yield return null;Check(predicate(),label);}
  private void Check(bool success,string label){checks.Add((success?"PASS ":"FAIL ")+label);Debug.Log("P3_AUDIT "+checks.Last());if(!success)throw new Exception(label);}
  private IEnumerator Route()
  {
   yield return Wait(()=>FindFirstObjectByType<Phase3Hub>()&&FindFirstObjectByType<Phase3Hub>().Ready,300,"Existing Golden Kos starts P3 hub with Prolog guarded");
   var hub=FindFirstObjectByType<Phase3Hub>();var c=CampaignState.Instance;
   Check(c.Data.battery==37&&c.Data.sanity==23&&c.Data.wallet_legitimate==0,"Isolated fixture carry-over remains exact before manual outlet");
   Check(!Phase3Campaign.Buy(c,"POWERBANK"),"Zero wallet cannot buy optional supplies");
   Check(Phase3Campaign.ChargeHub(c),"Physical preparation outlet transaction stores one 100 percent charge");
   Check(!Phase3Campaign.ChargeHub(c),"Preparation outlet cannot duplicate its receipt");
   Check(Phase3Campaign.Depart(c),"Atomic departure unlocks mandatory UV and EVP without purchase");
   SceneManager.sceneLoaded+=Loaded;
   SceneManager.LoadScene(CampaignRouting.FactoryScene);yield return null;
   SceneManager.sceneLoaded-=Loaded;
   yield return Wait(()=>FindFirstObjectByType<Phase3Director>()&&FindFirstObjectByType<Phase3Director>().Ready,300,"Factory standalone scene starts from real campaign checkpoint");
   d=FindFirstObjectByType<Phase3Director>();yield return new WaitForSeconds(.5f);d.player.enabled=false;
   if(Has("-phase3Visual")){yield return Visual();yield break;}
   if(Has("-phase3Budget")){yield return BudgetRoute();yield break;}
   yield return FullRoute();
  }
  private void Loaded(Scene scene,LoadSceneMode mode)
  {
   if(scene.name!=CampaignRouting.FactoryScene)return;
   Debug.Log("P3_SCENE_ACTIVATED lights="+FindObjectsByType<Light>(FindObjectsSortMode.None).Length+" cameras="+FindObjectsByType<Camera>(FindObjectsSortMode.None).Length);
  }
  private IEnumerator Camera(ScanVisualMode mode=ScanVisualMode.Normal,bool raised=true)
  {
   d.Phone.isScreenOn=d.Phone.batteryPercent>0;d.Phone.isSelfieCamera=false;d.Phone.SyncVisualState();
   if(raised){InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Right));yield return null;yield return null;}
   if(d.Phone.currentScanMode!=mode&&d.Phone.batteryPercent>0)
   {
    var action=mode==ScanVisualMode.Normal?(d.Phone.currentScanMode==ScanVisualMode.AR?GameAction.ModeAR:GameAction.ModeUV):mode==ScanVisualMode.AR?GameAction.ModeAR:GameAction.ModeUV;
    yield return Press(action);
   }
   if(!raised)
   {
    InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;yield return null;
    d.phoneView.Apps.Stow();d.phoneView.SetViewfinder(false);
   }
   yield return null;
  }
  private IEnumerator Hold(GameAction action,float seconds)
  {
   var key=(Key)Enum.Parse(typeof(Key),GameInputConfig.Instance.GetKeyForAction(action).ToString());InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return new WaitForSeconds(seconds);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;yield return null;
  }
  private IEnumerator Shot(string name)
  {
   if(SystemInfo.graphicsDeviceType==UnityEngine.Rendering.GraphicsDeviceType.Null)yield break;
   yield return new WaitForEndOfFrame();string p=Path.Combine(output,name+".png");ScreenCapture.CaptureScreenshot(p);yield return null;screenshots.Add(name+".png");
  }
  private IEnumerator Visual()
  {
   Check(SystemInfo.graphicsDeviceType!=UnityEngine.Rendering.GraphicsDeviceType.Null,"Actual graphical renderer available for posed composition review");
   d.enabled=false;d.wewe.agent.enabled=false;d.world.wewe.SetActive(false);d.phoneView.playerCamera.enabled=true;
   foreach(var gate in new[]{d.world.accessGate,d.world.hallGate,d.world.serviceGate})gate.GetComponent<Phase3Gate>().SetOpen(true,true);
   var views=new[]{
    ("01-yard",new Vector3(13,.04f,-13),new Vector3(13,4,1)),
    ("02-mill",new Vector3(18,.04f,8),new Vector3(9,1.5f,22)),
    ("03-sling",new Vector3(20,.04f,26.8f),new Vector3(21,2,29)),
    ("04-boiler",new Vector3(27,.04f,24),new Vector3(46,3,20)),
    ("05-office",new Vector3(56,4.04f,33),new Vector3(47,5.3f,36)),
    ("06-bagasse",new Vector3(31,.04f,37),new Vector3(25,1.8f,35)),
    ("07-balcony",new Vector3(47,4.04f,44),new Vector3(59,5,46)),
    ("08-arena",new Vector3(72,.04f,29),new Vector3(84,2,24)),
    ("09-stair",new Vector3(54,.04f,18),new Vector3(57,3,28))};
   foreach(var v in views)
   {
    var cc=d.player.GetComponent<CharacterController>();cc.enabled=false;d.player.transform.position=v.Item2;cc.enabled=true;
    Look(v.Item3);yield return Camera(ScanVisualMode.Normal,false);yield return new WaitForSeconds(.6f);yield return Shot(v.Item1+"-normal-fixture");
    if(v.Item1 is "02-mill" or "06-bagasse" or "07-balcony")
    {yield return Camera(ScanVisualMode.UV);Look(v.Item3);yield return new WaitForSeconds(.5f);yield return Shot(v.Item1+"-uv-fixture");}
   }
   var capsule=d.player.GetComponent<CharacterController>();capsule.enabled=false;d.player.transform.position=new Vector3(43,.04f,20);capsule.enabled=true;
   d.world.wewe.SetActive(true);d.world.wewe.transform.position=new Vector3(43,.04f,25);d.world.wewe.transform.rotation=Quaternion.Euler(0,180,0);
   yield return Camera();Look(d.wewe.Head);yield return new WaitForSeconds(.6f);yield return Shot("10-wewe-artist-model-fixture");
   Check(screenshots.Count>=12,"Normal and UV images captured for all mandatory factory areas; posed fixtures, not gameplay or GPU benchmark");
  }
        private void Look(Vector3 target)
        {
            Vector3 delta=target-d.player.cameraPivot.position;float yaw=Mathf.Atan2(delta.x,delta.z)*Mathf.Rad2Deg;
            d.player.transform.rotation=Quaternion.Euler(0,yaw,0);
            float pitch=-Mathf.Atan2(delta.y,new Vector2(delta.x,delta.z).magnitude)*Mathf.Rad2Deg;
            d.player.cameraPivot.localRotation=Quaternion.Euler(pitch,0,0);d.player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();d.phoneView.RefreshLensPose();
        }
        private IEnumerator Walk(Vector3 destination)
        {
            if(!NavMesh.SamplePosition(destination,out var end,1.5f,NavMesh.AllAreas))throw new Exception("No nav destination "+destination);
            var path=new NavMeshPath();if(!NavMesh.CalculatePath(d.player.transform.position,end.position,NavMesh.AllAreas,path)||path.status!=NavMeshPathStatus.PathComplete)
                throw new Exception("No physical route to "+destination);
            var capsule=d.player.GetComponent<CharacterController>();
            foreach(var corner in path.corners.Skip(1))
            {
                float timeout=Time.realtimeSinceStartup+90;Debug.Log("P2_ROUTE corner="+corner+" from="+d.player.transform.position);
                while(Vector3.Distance(new Vector3(d.player.transform.position.x,corner.y,d.player.transform.position.z),corner)>.12f)
                {
                    if(d.Dead)throw new Exception("Player died on capsule route at "+d.player.transform.position+" toward "+corner);
                    if(Time.realtimeSinceStartup>timeout)throw new Exception("Capsule route blocked at "+d.player.transform.position+" toward "+corner);
                    Vector3 delta=corner-d.player.transform.position;delta.y=0;
                    Look(d.player.cameraPivot.position+delta.normalized*4);
                    float speed=d.boss.Active&&d.boss.State==Phase3BossState.Stalk||d.wewe.Pursuing?4.2f:1.7f;
                    float step=Mathf.Min(delta.magnitude,speed*Time.deltaTime);
                    Vector3 before=d.player.transform.position;capsule.Move(delta.normalized*step+Vector3.down*3*Time.deltaTime);walked+=Vector3.Distance(before,d.player.transform.position);
                    yield return null;
                }
            }
        }
        private Phase3WorldItem FindInteractionItem(string id)
        {
            Vector3 Centre(Phase3WorldItem item)
            {var renderers=item.GetComponentsInChildren<Renderer>();if(renderers.Length==0)return item.transform.position;var bounds=renderers[0].bounds;foreach(var r in renderers.Skip(1))bounds.Encapsulate(r.bounds);return bounds.center;}
            return FindObjectsByType<Phase3WorldItem>(FindObjectsSortMode.None).Where(i=>i.id==id).OrderBy(i=>Vector3.Distance(d.player.cameraPivot.position,Centre(i))).FirstOrDefault();
        }
        private IEnumerator Use(string id)
        {
            var item=FindInteractionItem(id);
            if(!item)throw new Exception("Missing live item "+id);
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;
            yield return AimInteraction(item);
            yield return Press(GameAction.Interact);yield return null;
        }
        private IEnumerator AimInteraction(Phase3WorldItem item)
        {
            Vector3 target=item.transform.position;
            var renderers=item.GetComponentsInChildren<Renderer>();
            if(renderers.Length>0){var bounds=renderers[0].bounds;foreach(var r in renderers.Skip(1))bounds.Encapsulate(r.bounds);target=bounds.center;}
            Look(target);
            // Align the visible OTS reticle as a player does. Trigger-only markers
            // do not define the shoulder rig's physical-surface convergence distance.
            var interactor=d.player.GetComponent<InteractionInteractor>();
            for(int i=0;i<16;i++)
            {
                var cam=interactor.playerCamera.transform;
                Vector3 local=cam.InverseTransformDirection(target-cam.position);
                float yaw=Mathf.Atan2(local.x,local.z)*Mathf.Rad2Deg;
                float pitch=-Mathf.Atan2(local.y,new Vector2(local.x,local.z).magnitude)*Mathf.Rad2Deg;
                d.player.transform.rotation=Quaternion.Euler(0,d.player.transform.eulerAngles.y+yaw*.8f,0);
                d.player.cameraPivot.localRotation=Quaternion.Euler(Mathf.DeltaAngle(0,d.player.cameraPivot.localEulerAngles.x)+pitch*.8f,0,0);
                d.player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();yield return null;
                if(i>=3&&interactor.CurrentTarget?.InteractableId==item.id)break;
            }
            if(interactor.CurrentTarget?.InteractableId!=item.id)
            {
                var cam=interactor.playerCamera.transform;
                Debug.Log("P2_INTERACTION_HITS "+string.Join(" / ",Physics.RaycastAll(cam.position,cam.forward,5,~0,QueryTriggerInteraction.Collide).OrderBy(h=>h.distance).Select(h=>h.collider.name+" "+h.point+" parent="+h.collider.GetComponentInParent<IInteractable>()?.InteractableId)));
            }
            Check(interactor.CurrentTarget?.InteractableId==item.id,"Visible reticle targets "+item.id+" through real interaction ray");
        }
        private IEnumerator Press(GameAction action)
        {
            var code=GameInputConfig.Instance.GetKeyForAction(action);string name=code==KeyCode.LeftControl?"LeftCtrl":code.ToString();
            if(!Enum.TryParse<Key>(name,out var key))throw new Exception("Unsupported audit key "+code);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;
        }
        private IEnumerator PressMouse()
        {InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Right).WithButton(MouseButton.Left));yield return null;InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Right));yield return null;}
  private void Finish(bool ok)
  {
   if(done)return;done=true;File.WriteAllText(Path.Combine(output,"result.json"),JsonUtility.ToJson(new Result{completed=ok,actualStandalonePlayer=!Application.isEditor,utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,rendered=SystemInfo.graphicsDeviceType!=UnityEngine.Rendering.GraphicsDeviceType.Null,walkedMeters=walked,simulationTimeScale=Time.timeScale,method=Has("-phase3Budget")?"Explicit synthetic boss-only zero-battery fixture. Real held charging, alcove reset, twenty missed Blitz shots and on-disk CP5 rollback; no claim of investigation playthrough, human pacing or Windows acceptance.":Has("-phase3Visual")?"Isolated P2-complete fixture, production hub/departure, followed by explicitly posed/frozen composition views. Not traversal, Windows playtest or GPU acceptance.":"Isolated P2-complete fixture; directed native capsule traversal with production interactions and campaign writes. Not human playtest, pacing or Windows GPU acceptance.",checks=checks.ToArray(),screenshots=screenshots.ToArray()},true));Application.Quit(ok?0:2);
  }
  private void OnDestroy(){Application.logMessageReceived-=Log;}
 }
}
#endif
