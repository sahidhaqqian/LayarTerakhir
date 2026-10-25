#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
namespace LayarTerakhir.Diagnostics
{
    // Opt-in isolated save and deterministic standalone input driver. This is not
    // a human playtest, Windows GPU benchmark or proof of artistic acceptance.
    public sealed class Phase4PlayerAudit : MonoBehaviour
    {
        private static string output;
        private Phase4Director d;
        private Keyboard keyboard; private Mouse mouse;
        private readonly List<string> checks=new List<string>(),screens=new List<string>();
        private bool done;private float started,walked,trace;
        [Serializable]private class Result {public bool completed,standalone,rendered;public string utc,platform,gpu,method;public float walkedMeters;public string[] checks,screenshots;}
        public static CampaignData ReviewFixture()
        {
            var c=new CampaignData{chapter_id=4,checkpoint_id="PHASE4_HUB",battery=37,sanity=23,wallet_legitimate=0,wallet_bound=0};
            c.inventory.AddRange(new[]{"SMARTPHONE_RAKA","JAKET_RAKA","MATABATIN_APK"});c.location_seals.AddRange(new[]{Phase1Ids.Seal,Phase2Ids.Seal,Phase3Ids.Seal});
            c.phase1.Add("P1_COMPLETE");c.phase2.Add("P2_COMPLETE");c.phase3.Add("P3_BOSS_WIN");c.phase3.Add("P3_COMPLETE");c.phase3.next_destination=Phase3Ids.Destination;c.phase4.Add("P4_ARRIVE_KOS");
            c.upgrades.AddRange(new[]{"AR_FULL","SELFIE_MODE","THERMOKLENIK_APK","UV_RAJAH","EVP_V2"});return c;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-phase4Audit");bool review=Has("-phase4Review");if(i<0&&!review)return;
            bool audit=i>=0&&i+1<args.Length;string save;
            if(audit){output=Path.GetFullPath(args[i+1]);Directory.CreateDirectory(output);save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));}else save=Path.Combine(Application.persistentDataPath,"Phase4Review");
            Directory.CreateDirectory(save);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);string path=Path.Combine(save,"layar_terakhir_save.json");
            if(audit||!File.Exists(path))File.WriteAllText(path,JsonUtility.ToJson(ReviewFixture(),true));
            if(!audit)return;var go=new GameObject("Phase4PlayerAudit");DontDestroyOnLoad(go);go.AddComponent<Phase4PlayerAudit>();InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
        }
        private static bool Has(string arg)=>Array.IndexOf(Environment.GetCommandLineArgs(),arg)>=0;
        private void Awake(){started=Time.realtimeSinceStartup;Application.targetFrameRate=60;if(Has("-phase4Fast"))Time.timeScale=2;Application.logMessageReceived+=Log;}
        private void Log(string msg,string stack,LogType kind){if(!done&&(kind==LogType.Error||kind==LogType.Exception)){checks.Add("ERROR "+msg);Finish(false);}}
        private void Update()
        {
            if(d&&d.Ready&&Time.realtimeSinceStartup>trace){trace=Time.realtimeSinceStartup+3;Debug.Log("P4_TRACE zone="+d.CurrentZone+" player="+d.player.transform.position+" entity="+d.world.banaspati.transform.position+" AI="+d.entity.State+" ritual="+d.ritual.State+" source="+(d.SelectedSource?d.SelectedSource.sourceId:"none")+" EVP="+d.EvpProgress+" battery="+d.Phone.batteryPercent+" water="+d.State.water_units);}
            if(!done&&Time.realtimeSinceStartup-started>1800){checks.Add("FAIL timeout");Finish(false);}
        }
        private IEnumerator Start()
        {
            foreach(var dev in InputSystem.devices.ToArray())if(dev is Keyboard||dev is Mouse)InputSystem.DisableDevice(dev);
            keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();keyboard.MakeCurrent();mouse.MakeCurrent();
            yield return Drive(Route());if(!done)Finish(true);
        }
        private IEnumerator Drive(IEnumerator r)
        {
            while(!done){bool moved;object current=null;try{moved=r.MoveNext();if(moved)current=r.Current;}catch(Exception e){checks.Add("FAIL "+e);Finish(false);yield break;}if(!moved)yield break;if(current is IEnumerator nested)yield return Drive(nested);else yield return current;}
        }
        private void Check(bool ok,string label){checks.Add((ok?"PASS ":"FAIL ")+label);Debug.Log("P4_AUDIT "+checks.Last());if(!ok)throw new Exception(label);}
        private IEnumerator Wait(Func<bool> test,float seconds,string label){float end=Time.realtimeSinceStartup+seconds;while(!test()&&!done&&Time.realtimeSinceStartup<end)yield return null;Check(test(),label);}
        private IEnumerator Route()
        {
            yield return Wait(()=>FindFirstObjectByType<Phase4Hub>()&&FindFirstObjectByType<Phase4Hub>().Ready,300,"Golden Kos guarded from prolog and starts Phase4 hub");
            var c=CampaignState.Instance;Check(c.Data.battery==37&&c.Data.sanity==23&&c.Data.wallet_legitimate==0,"Carry-over values are exact; no hidden resource reset");
            Check(!Phase4Campaign.Buy(c,"POWERBANK"),"No-wallet fixture cannot purchase supplies");
            FindFirstObjectByType<Phase4Hub>().Interact("CHARGE");Check(c.Data.battery==100,"Hub outlet explicitly charges to100");Check(!Phase4Campaign.ChargeHub(c),"One-time outlet receipt prevents duplicate");
            Check(Phase4Campaign.Depart(c),"Free mandatory modules and atomic CP1 departure");SceneManager.LoadScene(CampaignRouting.VillageScene);yield return null;
            yield return Wait(()=>FindFirstObjectByType<Phase4Director>()&&FindFirstObjectByType<Phase4Director>().Ready,300,"Village starts from actual saved CP1");
            d=FindFirstObjectByType<Phase4Director>();c=CampaignState.Instance;yield return new WaitForSeconds(.5f);d.player.enabled=false;
            if(Has("-phase4Visual")){yield return Visual();yield break;}
            d.StartLive();long bound=c.Data.wallet_bound;d.StartLive();Check(bound==6666000&&c.Data.wallet_bound==bound&&c.Data.wallet_legitimate==0,"Opening bound donation recorded once");
            yield return Walk(new Vector3(0,0,-82));yield return Wait(()=>d.Completed("P4_BAMBOO_BRANCH_1"),5,"J1 saved");
            yield return Walk(new Vector3(12,0,-64));yield return Wait(()=>d.Completed("P4_BAMBOO_BRANCH_2"),5,"J2 saved");
            yield return Walk(new Vector3(-8,0,-46));yield return Wait(()=>d.Completed("P4_BAMBOO_BRANCH_3"),5,"J3 saved");
            yield return Walk(new Vector3(-6,0,-6));yield return Wait(()=>d.Completed("P4_BANASPATI_REVEAL"),5,"Pendopo reveal after all three branches");
            yield return Use("SHRINE_PENDOPO",10.3f);
            yield return Walk(new Vector3(-4.5f,0,1.1f));yield return Use("BOOK");if(d.Hud.IsModalOpen)d.Hud.CloseDocumentModal();
            // Free-order investigation W -> E -> N. Every E/C/LMB is dispatched
            // through the same interactor / handset used by the player.
            yield return Walk(new Vector3(-28,0,-.5f));yield return Camera();Look(d.world.wellMantra.position);yield return Hold(GameAction.ModeEVP,6.6f);
            Check(c.Data.evidence_ids.Contains(Phase4Ids.Mantra),"EVP six-second authentic mantra evidence");
            yield return Walk(new Vector3(-27,0,1));yield return Use("SOIL");Check(c.Data.relic_ids.Contains(Phase4Ids.Soil),"Soil pickup separate from EVP evidence");
            yield return Walk(new Vector3(-31,0,1.5f));yield return Camera();Look(d.world.secretKnock.position);yield return Hold(GameAction.ModeEVP,2.1f);
            Check(d.Completed("P4_SECRET_KNOCK_VERIFIED"),"Secret knock verified independently");
            yield return Camera(ScanVisualMode.UV);Look(d.world.secretEye.position);yield return new WaitForSeconds(1.4f);Check(d.Completed("P4_SECRET_EYE_VERIFIED"),"Physical UV eye verified");
            yield return Use("WHEEL");yield return Use("WHEEL");Check(d.Completed("P4_SECRET_WHEEL_ALIGNED"),"Well wheel opens authored drawer");yield return Use("MIRROR");Check(c.Data.secret_items.Contains(Phase4Ids.Secret),"Optional mirror saved once");
            yield return Walk(new Vector3(29,.65f,-.1f));yield return Camera(ScanVisualMode.AR);Look(d.world.arTarget.position);yield return new WaitForSeconds(1.4f);
            var lens=d.phoneView.FeedCamera;Debug.Log("P4_AR_DIAGNOSTIC mode="+d.Phone.currentScanMode+" raised="+d.Phone.isPhoneRaised+" scan="+d.CameraCanScan+" eyeDistance="+Vector3.Distance(d.player.cameraPivot.position,d.world.arTarget.position)+" lensDistance="+Vector3.Distance(lens.transform.position,d.world.arTarget.position)+" viewport="+lens.WorldToViewportPoint(d.world.arTarget.position)+" aim="+d.Aim(d.world.arTarget,d.balance.opticalRange,d.balance.opticalAngle,d.world.arOcclusionRoot,true));
            yield return Click();
            Check(c.Data.evidence_ids.Contains(Phase4Ids.Aura),"Actual AR feed photograph removes illusion");
            yield return Walk(new Vector3(30,.65f,4.1f));yield return Use("BLADE");yield return Walk(new Vector3(28,.65f,4.1f));yield return Use("DAMAR_NOTE");if(d.Hud.IsModalOpen)d.Hud.CloseDocumentModal();
            yield return Walk(new Vector3(0,0,26.5f));yield return Camera();if(!d.ThermoActive)yield return Press(GameAction.ModeThermo);Look(d.world.thermalTarget.position);yield return new WaitForSeconds(2.5f);
            Check(c.Data.evidence_ids.Contains(Phase4Ids.Thermo),"Stable Thermo+66 gives evidence without a photo");yield return Walk(new Vector3(.8f,0,27.7f));yield return Use("KENDI");
            Check(Phase4Ids.AllEvidence(c.Data)&&Phase4Ids.AllRelics(c.Data),"W E N investigation gathered three independent relics and evidence");
            Check(d.ConfirmJournal(),"Same phone journal confirms identity");
            yield return Walk(new Vector3(-7,0,-5));yield return Use("REFILL_PENDOPO",4.3f);Check(c.Data.phase4.water_units==3,"Free water source fills existing Kendi");
            for(int i=0;i<3;i++){yield return Walk(new Vector3(-2+i*2,0,-1.3f));yield return Use("ALTAR_"+i);}
            yield return Walk(new Vector3(3,0,-2));yield return Use("RITUAL");Check(d.ritual.Active,"CP5 activates with visible three offerings");
            foreach(int sector in new[]{2,0,1})
            {
                if(c.Data.phase4.water_units==0){yield return Walk(new Vector3(-7,0,-5));yield return Use("REFILL_PENDOPO",4.3f);}
                var t=d.world.Torch(sector,Phase4Ids.AuthenticIndex(c.Data.phase4.torch_layout_seed,sector));
                yield return Walk(t.transform.position+new Vector3(0,0,-1.3f));yield return Camera(ScanVisualMode.UV);Look(t.focus.position);yield return new WaitForSeconds(2.2f);
                yield return LureTo(t);
                yield return Wait(()=>!d.ritual.Recovering&&Vector3.Distance(d.phoneView.FeedCamera.transform.position,d.world.weakPoint.position)<7.4f&&d.ClearLine(d.player.cameraPivot.position,d.world.weakPoint.position,d.world.banaspati.transform),30,"Banaspati physically approaches sector "+sector);
                yield return Use(t.GetComponent<Phase4WorldItem>().id);Check(d.ritual.Exposed,"Real water exposes core in sector "+sector);yield return Camera();Look(d.world.weakPoint.position);
                TraceBlitz("sector "+sector);yield return Wait(()=>d.ritual.Exposed&&d.Aim(d.world.weakPoint,d.balance.aimRange,d.balance.aimAngle,d.world.banaspati.transform),1.5f,"Visible exposed core is framed in sector "+sector);yield return Click();
                Check(Phase4Ids.SectorDone(c.Data,sector),"Actual phone Blitz commits sector "+sector);Check(c.LoadCheckpoint()&&Phase4Ids.SectorDone(c.Data,sector),"Reload retains that sector");
                if(sector==2)
                {
                    SceneManager.LoadScene(CampaignRouting.VillageScene);yield return null;
                    yield return Wait(()=>FindFirstObjectByType<Phase4Director>()&&FindFirstObjectByType<Phase4Director>().Ready,120,"Real CP6 scene reload restores the village");
                    d=FindFirstObjectByType<Phase4Director>();c=CampaignState.Instance;d.player.enabled=false;
                    Check(Phase4Ids.SectorDone(c.Data,sector)&&c.Data.secret_items.Contains(Phase4Ids.Secret),"Scene reload retains west hit and optional mirror");
                }
            }
            Check(Phase4Ids.HitCount(c.Data)==3,"Exactly three local Blitz receipts");
            if(c.Data.phase4.water_units==0){yield return Walk(new Vector3(-7,0,-5));yield return Use("REFILL_PENDOPO",4.3f);}
            var south=d.world.Torch(3,Phase4Ids.AuthenticIndex(c.Data.phase4.torch_layout_seed,3));yield return Walk(south.transform.position+new Vector3(0,0,-1.3f));yield return Camera(ScanVisualMode.UV);Look(south.focus.position);
            yield return LureTo(south);
            yield return Wait(()=>!d.ritual.Recovering&&Vector3.Distance(d.player.cameraPivot.position,d.world.weakPoint.position)<7.4f,30,"South pursuit reaches pouring range");yield return Use(south.GetComponent<Phase4WorldItem>().id);
            Check(d.Completed("P4_SOUTH_OFF")&&Phase4Ids.HitCount(c.Data)==3,"South extinguishes with no fifth local shot");
            yield return Walk(new Vector3(-3,0,-5));yield return Wait(()=>d.ritual.Exposed,30,"Core physically merges at Pendopo");yield return Camera();Look(d.world.weakPoint.position);yield return Click();
            Check(d.Completed("P4_BOSS_WIN")&&Phase4Ids.HitCount(c.Data)==4&&c.Data.location_seals.Count==4,"Fourth Blitz commits one fourth location seal");
            yield return Wait(()=>d.Completed("P4_FIFTH_GATE_REVEAL")&&!d.StoryPlaying,15,"Gate epilogue persisted and dialogue completes");yield return Walk(new Vector3(0,0,1));yield return Use("PORTAL");Check(d.ConfirmExit(),"Portal confirmation writesCP8");
            yield return Walk(new Vector3(0,-4,15));Check(d.player.transform.position.y < -3,"Capsule physically descends authored stone steps into the boundary");
            Check(c.LoadCheckpoint()&&c.Data.phase4.safe_anchor=="BOUNDARY"&&c.Data.chapter_id==4,"Continue uses physical same-night boundary");
            Check(c.Data.wallet_legitimate==0&&c.Data.wallet_bound==6666000,"Error wallet display never enters actual ledger");
            Check(c.Data.inventory.All(id=>!id.StartsWith("P4_POWERBANK_")&&!id.StartsWith("P4_KEMENYAN_")&&!id.StartsWith("P4_GARAM_")),"Complete zero-purchase route");
        }
        private IEnumerator Camera(ScanVisualMode mode=ScanVisualMode.Normal)
        {
            d.phoneView.Apps.Stow();d.Phone.isScreenOn=d.Phone.batteryPercent>0;d.Phone.isSelfieCamera=false;d.Phone.SyncVisualState();InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Right));yield return null;yield return null;
            if(d.Phone.currentScanMode!=mode){var action=mode==ScanVisualMode.UV?GameAction.ModeUV:mode==ScanVisualMode.AR?GameAction.ModeAR:d.Phone.currentScanMode==ScanVisualMode.UV?GameAction.ModeUV:GameAction.ModeAR;yield return Press(action);}yield return null;
        }
        private void Look(Vector3 target)
        {
            Vector3 delta=target-d.player.cameraPivot.position;float yaw=Mathf.Atan2(delta.x,delta.z)*Mathf.Rad2Deg;d.player.transform.rotation=Quaternion.Euler(0,yaw,0);float pitch=-Mathf.Atan2(delta.y,new Vector2(delta.x,delta.z).magnitude)*Mathf.Rad2Deg;
            d.player.cameraPivot.localRotation=Quaternion.Euler(pitch,0,0);d.player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();d.phoneView.RefreshLensPose();
        }
        private IEnumerator LureTo(Phase4Torch torch)
        {
            Vector3 stance=torch.transform.position+Vector3.back*1.3f;
            for(int attempt=0;attempt<3;attempt++)
            {
                if(!d.ritual.Recovering&&Vector3.Distance(d.player.cameraPivot.position,d.world.weakPoint.position)<6.5f&&d.ClearLine(d.player.cameraPivot.position,d.world.weakPoint.position,d.world.banaspati.transform))yield break;
                float recoveryEnd=Time.realtimeSinceStartup+20;while(d.ritual.Recovering&&Time.realtimeSinceStartup<recoveryEnd)yield return null;
                var direction=stance-d.world.banaspati.transform.position;direction.y=0;
                var bait=d.world.banaspati.transform.position+direction.normalized*7;
                Debug.Log("P4_LURE walking to visible attention range "+bait);
                yield return Walk(bait,true);yield return Camera(ScanVisualMode.UV);Look(d.world.weakPoint.position);yield return new WaitForSeconds(.35f);
                yield return Walk(stance);yield return Camera(ScanVisualMode.UV);Look(torch.focus.position);
                float end=Time.realtimeSinceStartup+12;
                while(Time.realtimeSinceStartup<end&&!d.Dead)
                {
                    if(Vector3.Distance(d.player.cameraPivot.position,d.world.weakPoint.position)<6.5f&&d.ClearLine(d.player.cameraPivot.position,d.world.weakPoint.position,d.world.banaspati.transform))yield break;
                    yield return null;
                }
            }
        }
        private IEnumerator Walk(Vector3 target,bool stopWhenPursued=false)
        {
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;
            if(!NavMesh.SamplePosition(d.player.transform.position,out var a,2,NavMesh.AllAreas)||!NavMesh.SamplePosition(target,out var b,2,NavMesh.AllAreas))throw new Exception("No navmesh for "+target);
            var path=new NavMeshPath();NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path);Check(path.status==NavMeshPathStatus.PathComplete,"Connected capsule route "+target);
            var cc=d.player.GetComponent<CharacterController>();
            foreach(var corner in path.corners.Skip(1))
            {
                float end=Time.realtimeSinceStartup+120,stuck=0;
                while(Vector2.Distance(new Vector2(d.player.transform.position.x,d.player.transform.position.z),new Vector2(corner.x,corner.z))>.14f)
                {
                    if(stopWhenPursued&&d.entity.Pursuing)yield break;
                    if(d.Dead||Time.realtimeSinceStartup>end)throw new Exception("Capsule blocked/caught "+d.player.transform.position+" toward "+corner);
                    if(d.player.transform.position.y<Mathf.Min(a.position.y,b.position.y)-1)throw new Exception("Capsule fell below authored route before reaching "+target);
                    Vector3 dir=corner-d.player.transform.position;dir.y=0;Look(d.player.cameraPivot.position+dir.normalized*4);float speed=d.entity.Pursuing?4.2f:1.7f;var before=d.player.transform.position;
                    cc.Move(dir.normalized*Mathf.Min(dir.magnitude,speed*Time.deltaTime)+Vector3.down*3*Time.deltaTime);walked+=Vector3.Distance(before,d.player.transform.position);
                    stuck=Vector3.Distance(before,d.player.transform.position)<.001f?stuck+Time.unscaledDeltaTime:0;
                    if(stuck>3){var centre=d.player.transform.TransformPoint(cc.center);var ext=Vector3.up*Mathf.Max(0,cc.height*.5f-cc.radius);Debug.Log("P4_BLOCKED radius="+cc.radius+" height="+cc.height+" corner="+corner+" overlaps="+string.Join(" / ",Physics.OverlapCapsule(centre-ext,centre+ext,cc.radius+.12f,~0,QueryTriggerInteraction.Ignore).Where(x=>!x.transform.IsChildOf(d.player.transform)).Select(x=>x.name+" at="+x.transform.position)));throw new Exception("Physical capsule stalled at "+d.player.transform.position+" toward "+corner);}
                    yield return null;
                }
            }
            float settle=Time.realtimeSinceStartup+1;
            while(!cc.isGrounded&&Time.realtimeSinceStartup<settle)
            {cc.Move(Vector3.down*3*Time.deltaTime);if(d.player.transform.position.y<Mathf.Min(a.position.y,b.position.y)-1)throw new Exception("Capsule fell through authored route at "+target);yield return null;}
        }
        private Phase4WorldItem FindItem(string id)=>FindObjectsByType<Phase4WorldItem>(FindObjectsSortMode.None).Where(i=>i.id==id).OrderBy(i=>Vector3.Distance(i.transform.position,d.player.transform.position)).FirstOrDefault();
        private IEnumerator Use(string id,float hold=0)
        {
            var item=FindItem(id);Check(item,"Physical interaction exists "+id);InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;
            Vector3 target=item.transform.position;var rs=item.GetComponentsInChildren<Renderer>();if(rs.Length>0){var b=rs[0].bounds;foreach(var r in rs.Skip(1))b.Encapsulate(r.bounds);target=b.center;}
            Look(target);var interactor=d.player.GetComponent<InteractionInteractor>();
            for(int i=0;i<20;i++)
            {
                var cam=interactor.playerCamera.transform;Vector3 local=cam.InverseTransformDirection(target-cam.position);float yaw=Mathf.Atan2(local.x,local.z)*Mathf.Rad2Deg;float pitch=-Mathf.Atan2(local.y,new Vector2(local.x,local.z).magnitude)*Mathf.Rad2Deg;
                d.player.transform.Rotate(0,yaw*.8f,0);d.player.cameraPivot.localRotation=Quaternion.Euler(Mathf.DeltaAngle(0,d.player.cameraPivot.localEulerAngles.x)+pitch*.8f,0,0);d.player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();yield return null;
                if(i>2&&interactor.CurrentTarget?.InteractableId==id)break;
            }
            if(interactor.CurrentTarget?.InteractableId!=id)Debug.Log("P4_INTERACTION_HITS player="+d.player.transform.position+" target="+target+" bodyDistance="+Vector3.Distance(d.player.transform.position+Vector3.up,target)+" range="+interactor.interactionDistance+" allowed="+item.CanInteract()+" dead="+d.Dead+" / "+string.Join(" / ",Physics.RaycastAll(interactor.playerCamera.transform.position,interactor.playerCamera.transform.forward,5,~0,QueryTriggerInteraction.Collide).OrderBy(h=>h.distance).Select(h=>h.collider.name+" "+h.point+" id="+h.collider.GetComponentInParent<IInteractable>()?.InteractableId)));
            Check(interactor.CurrentTarget?.InteractableId==id,"Actual reticle acquires "+id);
            if(hold>0)yield return Hold(GameAction.Interact,hold);else yield return Press(GameAction.Interact);yield return null;
        }
        private IEnumerator Press(GameAction action)
        {var code=GameInputConfig.Instance.GetKeyForAction(action);var key=(Key)Enum.Parse(typeof(Key),code==KeyCode.LeftControl?"LeftCtrl":code.ToString());InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
        private IEnumerator Hold(GameAction action,float seconds)
        {var code=GameInputConfig.Instance.GetKeyForAction(action);var key=(Key)Enum.Parse(typeof(Key),code.ToString());InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return new WaitForSeconds(seconds);InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;yield return null;}
        private IEnumerator Click(){InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Right).WithButton(MouseButton.Left));yield return null;InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Right));yield return null;}
        private void TraceBlitz(string stage)
        {
            var lens=d.phoneView.FeedCamera;var target=d.world.weakPoint.position;var from=lens.transform.position;var delta=target-from;
            Debug.Log("P4_BLITZ_DIAGNOSTIC "+stage+" state="+d.ritual.State+" remaining="+d.ritual.Remaining+" scan="+d.CameraCanScan+" selfie="+d.Phone.isSelfieCamera+" eyeDistance="+Vector3.Distance(d.player.cameraPivot.position,target)+" viewport="+lens.WorldToViewportPoint(target)+" angle="+Vector3.Angle(lens.transform.forward,delta)+" aim="+d.Aim(d.world.weakPoint,d.balance.aimRange,d.balance.aimAngle,d.world.banaspati.transform)+" hits="+string.Join(" / ",Physics.RaycastAll(from,delta.normalized,delta.magnitude-.04f,~((1<<24)|(1<<26)|(1<<29)|(1<<2)|(1<<31)),QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance).Select(h=>h.collider.name+" ["+h.collider.GetComponentInParent<Phase4AssetSource>()?.purpose+"] ignored="+h.transform.IsChildOf(d.world.banaspati.transform))));
        }
        private IEnumerator Visual()
        {
            Check(SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null,"Graphical standalone renderer for posed compositions");d.enabled=false;d.entity.agent.enabled=false;d.world.banaspati.SetActive(false);d.phoneView.playerCamera.enabled=true;
            var views=new[]{("01-forest",new Vector3(0,.05f,-98),new Vector3(0,3,-80)),("02-pendopo",new Vector3(-9,.05f,-14),new Vector3(0,3,0)),("03-grave",new Vector3(4,.05f,22),new Vector3(0,2,30)),("04-gubuk",new Vector3(22,.05f,-8),new Vector3(29,2,4)),("05-well",new Vector3(-22,.05f,-6),new Vector3(-29,1.5f,3)),("06-south",new Vector3(-6,.05f,-21),new Vector3(0,2,-29))};
            foreach(var v in views){var cc=d.player.GetComponent<CharacterController>();cc.enabled=false;d.player.transform.position=v.Item2;cc.enabled=true;InputSystem.QueueStateEvent(mouse,new MouseState());Look(v.Item3);yield return new WaitForSeconds(.7f);yield return Screenshot(v.Item1);}
            d.world.suro.SetActive(true);var capsule=d.player.GetComponent<CharacterController>();capsule.enabled=false;d.player.transform.position=new Vector3(0,.05f,-15);capsule.enabled=true;Look(d.world.suro.transform.position+Vector3.up*1.6f);yield return new WaitForSeconds(.5f);yield return Screenshot("07-suro-roof-posed");
            d.world.portalCover.SetActive(false);d.world.portalStairs.SetActive(true);capsule.enabled=false;d.player.transform.position=new Vector3(0,.05f,1.5f);capsule.enabled=true;Look(new Vector3(0,-2,7));yield return new WaitForSeconds(.5f);yield return Screenshot("08-stairs-posed");
            // Render-only fixture: activate downloaded torch art and glyphs in
            // this isolated pose. It is not a ritual/gameplay acceptance run.
            d.State.torch_layout_seed=0;foreach(var torch in d.world.torches)torch.Present(true,false);
            capsule.enabled=false;d.player.transform.position=new Vector3(0,.05f,13.8f);capsule.enabled=true;
            yield return Camera(ScanVisualMode.UV);Look(d.world.Torch(0,1).focus.position);yield return new WaitForSeconds(.8f);
            Check((d.phoneView.FeedCamera.cullingMask&(1<<24))!=0,"Posed UV feed includes authored glyph layer");
            yield return Screenshot("09-uv-torches-posed");
        }
        private IEnumerator Screenshot(string name){yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;screens.Add(name+".png");}
        private void Finish(bool ok)
        {
            if(done)return;done=true;Time.timeScale=1;Application.logMessageReceived-=Log;
            File.WriteAllText(Path.Combine(output,"result.json"),JsonUtility.ToJson(new Result{completed=ok,standalone=!Application.isEditor,rendered=SystemInfo.graphicsDeviceType!=GraphicsDeviceType.Null,utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,walkedMeters=walked,method=Has("-phase4Visual")?"Posed standalone composition fixtures; not gameplay acceptance":"Directed CharacterController/NavMesh route; real E/C/R/Q/T/LMB/RMB, isolated save, live threat; not a human playtest",checks=checks.ToArray(),screenshots=screens.ToArray()},true)+"\n");Application.Quit(ok?0:1);
        }
    }
}
#endif
