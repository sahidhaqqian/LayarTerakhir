#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase2;
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
    // Explicit development-only review fixture. No real campaign is seeded or overwritten.
    public sealed class Phase2PlayerAudit : MonoBehaviour
    {
        private static string output;
        private readonly List<string> checks=new List<string>(),screenshots=new List<string>();
        private Phase2Director d;
        private Keyboard keyboard;private Mouse mouse;
        private float started,walked;
        private bool done;
        [Serializable]private sealed class Result
        {public bool completed,actualStandalonePlayer;public string utc,platform,gpu,method;public float walkedMeters,simulationTimeScale;public bool rendered;public string[] checks,screenshots;}
        public static CampaignData ReviewFixture()
        {
            var data=new CampaignData{chapter_id=2,checkpoint_id="PHASE2_HUB",battery=100,sanity=23,wallet_legitimate=0,wallet_bound=0};
            data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","JAKET_RAKA","MATABATIN_APK"});
            data.location_seals.Add(Phase1Ids.Seal);data.phase1.Add("P1_COMPLETE");data.phase1.next_destination="RSUD Lama Sido Waras";
            data.phase2.Add("P2_ARRIVE_KOS");return data;
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int index=Array.IndexOf(args,"-phase2Audit");bool review=Array.IndexOf(args,"-phase2Review")>=0;
            if(index<0&&!review)return;
            bool audit=index>=0&&index+1<args.Length;
            string save;
            if(audit)
            {output=Path.GetFullPath(args[index+1]);Directory.CreateDirectory(output);save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));}
            else save=Path.Combine(Application.persistentDataPath,"Phase2Review");
            Directory.CreateDirectory(save);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            string path=Path.Combine(save,"layar_terakhir_save.json");
            if(audit||!File.Exists(path))File.WriteAllText(path,JsonUtility.ToJson(ReviewFixture(),true));
            if(!audit)return;
            var go=new GameObject("Phase2PlayerAudit");DontDestroyOnLoad(go);go.AddComponent<Phase2PlayerAudit>();
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.targetFrameRate=60;if(Array.IndexOf(Environment.GetCommandLineArgs(),"-phase2AuditFast")>=0)Time.timeScale=2;Application.logMessageReceived+=Log;}
        private void Log(string message,string trace,LogType type)
        {if(!done&&(type==LogType.Error||type==LogType.Exception)){checks.Add("ERROR "+message);Finish(false);}}
        private void Update(){if(!done&&Time.realtimeSinceStartup-started>1200){checks.Add("FAIL route timeout");Finish(false);}}
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
                try{moved=routine.MoveNext();if(moved)current=routine.Current;}
                catch(Exception ex){checks.Add("FAIL "+ex);Finish(false);yield break;}
                if(!moved)yield break;
                if(current is IEnumerator child)yield return Drive(child);else yield return current;
            }
        }
        private IEnumerator Wait(Func<bool> predicate,float seconds,string description)
        {
            float end=Time.realtimeSinceStartup+seconds;
            while(!predicate()&&Time.realtimeSinceStartup<end&&!done)yield return null;
            Check(predicate(),description);
        }
        private void Check(bool condition,string description)
        {checks.Add((condition?"PASS ":"FAIL ")+description);Debug.Log("P2_AUDIT "+checks[checks.Count-1]);if(!condition)throw new InvalidOperationException(description);}
        private IEnumerator Route()
        {
            yield return Wait(()=>FindFirstObjectByType<Phase2Hub>()&&FindFirstObjectByType<Phase2Hub>().Ready,300,"P2 HUB guards Prolog before corrections and starts in existing Kos");
            var hub=FindFirstObjectByType<Phase2Hub>();
            Check(!FindFirstObjectByType<LayarTerakhir.Prolog.KamarKosPrologDirector>().enabled,"Prolog director disabled in P2 hub");
            Check(CampaignState.Instance.Data.sanity==23&&CampaignState.Instance.Data.wallet_legitimate==0,"Actual isolated carry-over, no economy grant");
            yield return Shot("01-kos-day-hub");
            hub.Interact("PC");Check(UiFocus.Clock,"PC preparation menu opens");hub.Close();
            Check(!Phase2Campaign.Buy(CampaignState.Instance,"POWERBANK"),"Zero wallet route needs no purchased powerbank");
            Check(Phase2Campaign.Depart(CampaignState.Instance),"Atomic departure installs fallback modules");
            SceneManager.LoadSceneAsync(CampaignRouting.HospitalScene);
            yield return Wait(()=>FindFirstObjectByType<Phase2Director>()&&FindFirstObjectByType<Phase2Director>().Ready,300,"Continue/depart reaches RSUD scene");
            d=FindFirstObjectByType<Phase2Director>();yield return new WaitForSeconds(.5f);d.player.enabled=false;
            Check(d.Phone.selfieModuleUnlocked&&d.Phone.arModuleUnlocked,"One existing phone with both investigation modules");
            Check(CampaignState.Instance.Data.battery<=100&&CampaignState.Instance.Data.battery>99,"Departure carries actual battery without duplicate grant");
            yield return Shot("02-igd-arrival");
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-phase2ModelAudit")>=0){yield return MerahReview();yield break;}
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-phase2SafeAudit")>=0)
            {
                var capsule=d.player.GetComponent<CharacterController>();capsule.enabled=false;d.player.transform.position=new Vector3(22,4.03f,9.1f);capsule.enabled=true;
                yield return Camera(false,false,false);yield return Use("SAFE");yield return SolveKeypad("2604");yield return new WaitForSeconds(1.2f);
                yield return Shot("safe-open-fixture");yield return Use("BLOOD");Check(CampaignState.Instance.HasInventoryItem(Phase2Ids.Blood),"Real safe interaction exposes collectible blood");yield break;
            }
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-phase2SafetyAudit")>=0){yield return Safety();yield break;}
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-phase2VisualReview")>=0){yield return VisualReview();yield break;}
            yield return Walk(new Vector3(8,.03f,-4.4f));yield return Use("LOG");HudController.Instance.CloseDocumentModal();
            yield return Walk(new Vector3(10,.03f,1.8f));yield return Use("POLI_BOARD");HudController.Instance.CloseDocumentModal();
            Check(d.Completed("P2_NURSE_LOG")&&d.Completed("P2_POLI_BOARD"),"Deduction clues archived without going live");
            yield return Walk(new Vector3(27,.03f,1.3f));yield return Camera(false,true,false);Look(new Vector3(27,1.45f,.2f));
            yield return new WaitForSeconds(2);
            Check(CampaignState.Instance.Data.evidence_ids.Contains(Phase2Ids.Freeze),"Stable actual Thermo aim records -16C evidence while offline");
            yield return Shot("03-poli03-thermoklenik");
            yield return Camera(false,false,false);yield return Walk(new Vector3(33,.03f,2.8f));yield return Use("BASEMENT_PANEL");
            // Production keypad wheels are turned through the public HUD buttons, not direct gate receipts.
            yield return SolveKeypad("1603");Check(d.Completed("P2_KEYPAD_1603"),"Keypad 1603 unlocks real basement gate");
            yield return Walk(new Vector3(34,-3.97f,17));
            yield return Walk(new Vector3(26,-3.97f,15.5f));yield return Walk(new Vector3(21,-3.97f,18));
            yield return Detach();
            yield return Walk(new Vector3(16.6f,-3.97f,17.6f));yield return Use("AUTOPSY");
            Check(CampaignState.Instance.HasInventoryItem(Phase2Ids.Key),"Autopsy document grants independent L2 key atomically");HudController.Instance.CloseDocumentModal();
            yield return Walk(new Vector3(27.1f,-3.97f,22));yield return Detach();
            yield return Walk(new Vector3(27.1f,-3.97f,22));yield return Camera(false,true,false);Look(d.world.c3Target.position);
            yield return new WaitForSeconds(1.4f);Check(d.Completed("P2_DRAWER_CHAIN_BREAK"),"C3 -24C chain only breaks after real sensor dwell");
            yield return Use("C3");yield return new WaitForSeconds(.6f);
            Check(d.Completed("P2_C3_OPEN"),"Physical C3 drawer opened through existing E interactor");
            yield return Camera(false,false,true);Look(d.world.auraTarget.position);yield return new WaitForSeconds(1.3f);
            Check(d.CameraCanScan&&d.ClearLine(d.phoneView.FeedCamera.transform.position,d.world.auraTarget.position),"Opened C3 aura has real camera LOS and framing");
            yield return PressMouse();
            Check(CampaignState.Instance.Data.evidence_ids.Contains(Phase2Ids.Aura),"AR aura photo and its one energy debit persist");
            Check(!CampaignState.Instance.HasInventoryItem(Phase2Ids.Gunting),"Aura evidence does not silently grant pusaka");
            yield return Use("GUNTING");Check(CampaignState.Instance.HasInventoryItem(Phase2Ids.Gunting),"Gunting picked independently");
            yield return Shot("04-morgue-c3");
            yield return Detach();yield return Camera(false,false,false);
            yield return Walk(new Vector3(13,-3.97f,30));yield return Walk(new Vector3(9.6f,-3.97f,29.1f));yield return Use("ARCHIVE_KEY");
            Check(CampaignState.Instance.HasInventoryItem(Phase2Ids.ArchiveKey),"Archive key reached and collected independently");
            yield return Walk(new Vector3(12,-3.97f,31.7f));yield return Use("D1");
            Check(CampaignState.Instance.Data.secret_items.Contains(Phase2Ids.Secret),"Optional D1 secret persists without mandatory scan");
            yield return Detach();yield return Walk(new Vector3(31.7f,-3.97f,18.3f));yield return Walk(new Vector3(34,-3.97f,17));
            yield return Walk(new Vector3(34,.03f,3));yield return Walk(new Vector3(1,.03f,2.7f));yield return Use("TERALI");
            yield return Walk(new Vector3(2,4.03f,15));yield return Walk(new Vector3(17,4.03f,18));yield return Walk(new Vector3(18,4.03f,12));
            Check(d.CurrentRoom=="NURSE","Physical stairs reach safe nurse room without teleporting");
            yield return Use("NURSE_NOTE");HudController.Instance.CloseDocumentModal();
            yield return Walk(new Vector3(20,4.03f,9.2f));yield return Use("BIRTH_BOARD");HudController.Instance.CloseDocumentModal();
            yield return Walk(new Vector3(22,4.03f,9.1f));yield return Use("SAFE");yield return SolveKeypad("2604");yield return new WaitForSeconds(1);yield return Use("BLOOD");
            Check(CampaignState.Instance.HasInventoryItem(Phase2Ids.Blood),"2604 deduction route works before visiting Inkubator");
            yield return Walk(new Vector3(23.2f,4.03f,9.1f));yield return Walk(new Vector3(23.2f,4.03f,12));
            yield return Walk(new Vector3(17,4.03f,18));yield return Walk(new Vector3(13,4.03f,18.5f));
            yield return Camera(false,false,true);Look(d.world.wallTarget.position);yield return new WaitForSeconds(.8f);
            yield return Walk(new Vector3(13,4.03f,22));
            Check(d.Completed("P2_CROSS_INKUBATOR_WALL"),"AR passage crossing latches permanently");
            yield return Walk(new Vector3(10,4.03f,26));yield return Camera(false,false,false);Look(d.world.evpTarget.position);
            yield return Press(GameAction.ModeEVP);yield return new WaitForSeconds(6.4f);
            Check(CampaignState.Instance.Data.evidence_ids.Contains(Phase2Ids.EVP),"Six-second real EVP aim records evidence independently");
            yield return Use("GELANG");yield return Use("DAMAR");HudController.Instance.CloseDocumentModal();
            Check(d.ConfirmJournal(),"Three exact evidence IDs require explicit journal confirmation");
            Check(Phase2Ids.RitualReady(CampaignState.Instance.Data),"All three relics and all three evidence ready without shopping");
            yield return Shot("05-inkubator-investigation");
            d.Phone.RestoreBattery(0);yield return Walk(new Vector3(13,4.03f,18));
            Check(!d.world.inkubatorWall.activeSelf,"Permanent return route stays open at zero battery");
            yield return Walk(new Vector3(18,4.03f,12));
            yield return Shot("06-nurse-safe-room");
            yield return Walk(new Vector3(17,4.03f,18));yield return Walk(new Vector3(34.6f,4.03f,17));yield return Use("OR_DOOR");
            Check(d.Completed("P2_INVESTIGATION_COMPLETE"),"Session 1 endpoint saved before operation-room ritual");d.CloseOverlay();
            Check(!d.Completed("P2_BOSS_WIN")&&!CampaignState.Instance.Data.location_seals.Contains("SEAL_RSUD_LAMA_SIDO_WARAS"),"No Session 2 boss/seal/ending is fabricated");
            yield return Shot("07-operation-door");
            Check(CampaignState.Instance.LoadCheckpoint(),"Last investigation snapshot reloads");
            Check(Phase2Ids.RitualReady(CampaignState.Instance.Data)&&CampaignState.Instance.Data.secret_items.Contains(Phase2Ids.Secret),"Latest snapshot retains relics, evidence, journal and secret");
            Check(CampaignState.Instance.Data.sanity==23&&CampaignState.Instance.Data.wallet_legitimate==0,"Sanity and legitimate money never mutated by P2 investigation");
            yield return SceneManager.LoadSceneAsync(CampaignRouting.HospitalScene);
            yield return Wait(()=>FindFirstObjectByType<Phase2Director>()&&FindFirstObjectByType<Phase2Director>().Ready,300,"Actual scene reload reaches saved investigation anchor");
            d=FindFirstObjectByType<Phase2Director>();
            Check(Vector3.Distance(d.player.transform.position,d.world.Anchor("NURSE").position)<.5f,"Scene restore places capsule at nurse checkpoint");
            Check(d.Phone.batteryPercent==0&&!d.world.inkubatorWall.activeSelf&&Phase2Ids.RitualReady(CampaignState.Instance.Data),"Scene restore retains zero battery, permanent passage and all investigation progress");
        }
        private IEnumerator Safety()
        {
            yield return Camera(false,false,false);
            Check(d.phoneView.ViewfinderActive,"Held RMB raises physical camera while offline");
            Check((d.phoneView.FeedCamera.cullingMask&(1<<Phase2Lastri.VisibilityLayer))==0,"Rear lens excludes Lastri");
            yield return Press(GameAction.ModeSelfie);yield return null;
            Check(d.Phone.isSelfieCamera&&(d.phoneView.FeedCamera.cullingMask&(1<<Phase2Lastri.VisibilityLayer))!=0,"X switches same handset to Lastri-visible front lens");
            yield return Press(GameAction.ToggleScreen);yield return null;
            Check(!d.Phone.isScreenOn&&!d.phoneView.ViewfinderActive,"F cancels held camera without instantly raising it again");
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;
            yield return Camera(false,false,false);yield return Press(GameAction.TogglePhone);yield return null;
            Check(!d.Phone.isPhoneRaised&&!d.phoneView.ViewfinderActive,"Tab stows held camera until RMB release");
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;
            d.Phone.RestoreBattery(0);
            float clock=CampaignState.Instance.Data.phase2.elapsed_gameplay_seconds,battery=d.Phone.batteryPercent;
            GameShell.Instance.Pause();yield return new WaitForSecondsRealtime(.6f);
            Check(Mathf.Abs(CampaignState.Instance.Data.phase2.elapsed_gameplay_seconds-clock)<.1f&&d.Phone.batteryPercent==battery,"Pause freezes investigation clock and energy");GameShell.Instance.Resume();
            yield return Walk(new Vector3(10,.03f,-4));
            yield return HoldUse("DYNAMO_IGD",8.6f);
            Check(Mathf.Abs(d.Phone.batteryPercent-25)<.1f&&CampaignState.Instance.Data.phase2.dynamo_full_charge_used_ids.Contains("DYNAMO_IGD"),"Eight-second held E gives one persisted +25 operation from zero");
            d.Phone.RestoreBattery(0);yield return HoldUse("DYNAMO_IGD",20);
            Check(d.Phone.batteryPercent==0,"Interrupted emergency crank grants no early energy");
            yield return HoldUse("DYNAMO_IGD",26);
            Check(Mathf.Abs(d.Phone.batteryPercent-10)<.1f,"Emergency crank resumes partial progress and caps at ten");
            d.Phone.RestoreBattery(0);yield return HoldUse("DYNAMO_IGD",1);
            Check(d.Phone.batteryPercent==0&&CampaignState.Instance.Data.phase2.dynamo_emergency_last_uses.Count==1,"Emergency cooldown rejects immediate repeat at zero");
            // Explicit safety fixture placement, not a claim of walking through locked gates.
            var capsule=d.player.GetComponent<CharacterController>();capsule.enabled=false;d.player.transform.position=new Vector3(22.2f,-3.97f,17);capsule.enabled=true;
            d.lastri.Restore();Look(new Vector3(10,-2.5f,17));
            yield return Wait(()=>d.lastri.Attached,25,"Normal investigation AI attaches in zero-energy safety fixture");
            yield return Walk(new Vector3(15,-3.97f,17.7f));yield return Use("HIDE_TABLE_A");
            Check(d.Hidden&&capsule.height<.7f,"Existing E interactor enters fitted under-table hiding capsule");
            yield return new WaitForSeconds(6.6f);
            Check(!d.lastri.Attached&&!d.Dead,"Six seconds hidden with dead screen detaches without battery");
            yield return Press(GameAction.Interact);
            Check(!d.Hidden&&capsule.height>1.4f&&!d.player.MovementLocked,"E exits hiding and restores standing capsule without phone");
            yield return Shot("safety-zero-energy-hiding");
            capsule.enabled=false;d.player.transform.position=new Vector3(6,.03f,-12.5f);capsule.enabled=true;
            for(float elapsed=0;elapsed<3;elapsed+=Time.deltaTime){capsule.Move(Vector3.back*1.7f*Time.deltaTime+Vector3.down*3*Time.deltaTime);yield return null;}
            Check(d.player.transform.position.z> -14,"Imported courtyard fence stops real capsule movement at the map edge");
            float energy=d.Phone.batteryPercent;string anchor=CampaignState.Instance.Data.phase2.safe_anchor;
            capsule.enabled=false;d.player.transform.position=new Vector3(50,-11,50);capsule.enabled=true;yield return null;yield return null;
            Check(Vector3.Distance(d.player.transform.position,d.world.Anchor(anchor).position)<.5f&&d.Phone.batteryPercent==energy,"Out-of-bounds fixture recovers to saved anchor without granting energy");
        }
        private IEnumerator HoldUse(string id,float seconds)
        {
            var item=FindObjectsByType<Phase2WorldItem>(FindObjectsSortMode.None).First(i=>i.id==id);
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;yield return AimInteraction(item);
            var code=GameInputConfig.Instance.GetKeyForAction(GameAction.Interact);var key=(Key)Enum.Parse(typeof(Key),code.ToString());
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return new WaitForSeconds(seconds);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;yield return null;
        }
        private IEnumerator VisualReview()
        {
            var views=new[]{("03-poli",new Vector3(17,.03f,2),new Vector3(31,1.4f,2)),
                ("04-morgue",new Vector3(10,-3.97f,16),new Vector3(28,-2.5f,23)),
                ("05-incubator",new Vector3(13,4.03f,21.8f),new Vector3(14,5.2f,27)),
                ("06-nurse",new Vector3(17,4.03f,15),new Vector3(19,5.1f,10))};
            foreach(var v in views)
            {var c=d.player.GetComponent<CharacterController>();c.enabled=false;d.player.transform.position=v.Item2;c.enabled=true;Look(v.Item3);yield return new WaitForSeconds(.5f);yield return Shot(v.Item1);}
            yield return MerahReview();
        }
        private IEnumerator MerahReview()
        {
            d.enabled=false;d.lastri.enabled=false;
            var capsule=d.player.GetComponent<CharacterController>();capsule.enabled=false;d.player.transform.position=new Vector3(18,4.03f,12);capsule.enabled=true;
            Look(new Vector3(18,5.4f,8.5f));yield return Camera(true,false,false);
            var ghost=d.world.lastri.transform;
            ghost.position=d.player.transform.position-d.player.transform.forward*1.7f-d.player.transform.right*.58f;
            d.lastri.agent.Warp(ghost.position);d.lastri.agent.enabled=false;
            ghost.rotation=Quaternion.LookRotation(d.player.transform.position-ghost.position,Vector3.up);
            foreach(var renderer in ghost.GetComponentsInChildren<Renderer>(true))renderer.enabled=true;
            yield return new WaitForSeconds(.5f);
            var feed=d.phoneView.FeedCamera;
            Debug.Log("P2_MODEL_CAMERA selfie="+d.Phone.isSelfieCamera+" chapter="+d.Phone.ChapterContent?.GetType().Name+" cam="+feed.transform.position+" forward="+feed.transform.forward+" mask="+feed.cullingMask+" ghost="+ghost.position);
            foreach(var renderer in ghost.GetComponentsInChildren<Renderer>(true))
                Debug.Log("P2_MODEL_RENDER "+renderer.name+" enabled="+renderer.enabled+" active="+renderer.gameObject.activeInHierarchy+" forceOff="+renderer.forceRenderingOff+" shadows="+renderer.shadowCastingMode+" layer="+renderer.gameObject.layer+" bounds="+renderer.bounds+" viewport="+feed.WorldToViewportPoint(renderer.bounds.center)+" scale="+renderer.transform.lossyScale+" material="+renderer.sharedMaterial.name);
            yield return Shot("07-designer-merah-selfie-fixture");
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
                    float step=Mathf.Min(delta.magnitude,1.7f*Time.deltaTime);
                    Vector3 before=d.player.transform.position;capsule.Move(delta.normalized*step+Vector3.down*3*Time.deltaTime);walked+=Vector3.Distance(before,d.player.transform.position);
                    yield return null;
                }
            }
        }
        private IEnumerator Camera(bool selfie,bool thermo,bool ar)
        {
            d.Phone.isPhoneRaised=true;d.Phone.isScreenOn=d.Phone.batteryPercent>0;d.Phone.isSelfieCamera=selfie;
            d.Phone.currentScanMode=ar?ScanVisualMode.AR:ScanVisualMode.Normal;d.Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            InputSystem.QueueStateEvent(mouse,new MouseState().WithButton(MouseButton.Right));yield return null;yield return null;
            if(d.ThermoActive!=thermo)yield return Press(GameAction.ModeThermo);
        }
        private IEnumerator Detach()
        {
            if(!d.lastri.Attached&&d.lastri.State!=LastriInvestigationState.Telegraph)yield break;
            yield return Camera(true,false,false);
            // Select a physically clear forward/side leg without turning into Lastri.
            // A fixed U-turn at C3 is a legal POUNCE, not a collision failure.
            for(int leg=0;leg<12&&(d.lastri.Attached||d.lastri.State==LastriInvestigationState.Telegraph);leg++)
            {
                Vector3 here=d.player.transform.position,toGhost=d.world.lastri.transform.position-here;toGhost.y=0;
                Vector3 chosen=here;float best=0;
                foreach(float angle in new[]{0f,45f,-45f,80f,-80f,105f,-105f})
                foreach(float length in new[]{7f,4f,2f})
                {
                    Vector3 dir=Quaternion.Euler(0,angle,0)*d.player.transform.forward;
                    if(d.lastri.Attached&&Vector3.Angle(dir,toGhost)<65)continue;
                    if(!NavMesh.SamplePosition(here+dir*length,out var target,.45f,NavMesh.AllAreas)||Mathf.Abs(target.position.y-here.y)>.4f)continue;
                    if(!NavMesh.SamplePosition(here,out var source,.5f,NavMesh.AllAreas)||NavMesh.Raycast(source.position,target.position,out _,NavMesh.AllAreas))continue;
                    var cc=d.player.GetComponent<CharacterController>();
                    bool blocked=Physics.CapsuleCastAll(here+Vector3.up*(cc.radius+.05f),here+Vector3.up*(cc.height-cc.radius),cc.radius+.025f,dir,length,
                        ~((1<<25)|(1<<26)|(1<<29)|(1<<31)),QueryTriggerInteraction.Ignore).Any(h=>!h.transform.IsChildOf(d.player.transform));
                    if(blocked)continue;float score=length-Mathf.Abs(angle)*.015f;
                    if(score>best){best=score;chosen=target.position;}
                }
                if(best<=0)throw new Exception("No safe forward Selfie escape leg at "+here);
                yield return Walk(chosen);
            }
            Check(!d.lastri.Attached,"Selfie + actual forward capsule travel detaches Lastri");
            yield return Camera(false,false,false);
        }
        private IEnumerator Use(string id)
        {
            var item=FindObjectsByType<Phase2WorldItem>(FindObjectsSortMode.None).FirstOrDefault(i=>i.id==id);
            if(!item)throw new Exception("Missing live item "+id);
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;
            yield return AimInteraction(item);
            yield return Press(GameAction.Interact);yield return null;
        }
        private IEnumerator AimInteraction(Phase2WorldItem item)
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
                if(i>=3&&interactor.CurrentTarget==item)break;
            }
            if(interactor.CurrentTarget!=item)
            {
                var cam=interactor.playerCamera.transform;
                Debug.Log("P2_INTERACTION_TRACE id="+item.id+" active="+item.gameObject.activeInHierarchy+" allowed="+item.CanInteract()+" target="+target+" camera="+cam.position+" forward="+cam.forward+" selected="+interactor.CurrentTarget?.InteractableId+" safe="+d.world.safeDoor.IsOpen+" hinge="+d.world.safeDoor.hinge.eulerAngles);
                Debug.Log("P2_INTERACTION_HITS "+string.Join(" / ",Physics.RaycastAll(cam.position,cam.forward,5,~0,QueryTriggerInteraction.Collide).OrderBy(h=>h.distance).Select(h=>h.collider.name+" "+h.point+" parent="+h.collider.GetComponentInParent<IInteractable>()?.InteractableId)));
            }
            Check(interactor.CurrentTarget==item,"Visible reticle targets "+item.id+" through real interaction ray");
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
        private IEnumerator SolveKeypad(string code)
        {
            var hud=HudController.Instance;Check(hud.padlockPanel.activeSelf,"Production keypad modal opened");
            var buttons=hud.padlockPanel.GetComponentsInChildren<UnityEngine.UI.Button>(true);
            for(int i=0;i<code.Length;i++)
            {
                var button=buttons.FirstOrDefault(b=>b.name=="DigitUp_"+i);
                if(!button)throw new Exception("Keypad wheel up button not found: "+string.Join(",",buttons.Select(b=>b.name)));
                for(int n=0;n<code[i]-'0';n++)button.onClick.Invoke();
            }
            buttons.First(b=>b.name=="UnlockPadlockBtn").onClick.Invoke();yield return null;
        }
        private IEnumerator Shot(string id)
        {
            if(SystemInfo.graphicsDeviceType==UnityEngine.Rendering.GraphicsDeviceType.Null)yield break;
            string path=Path.Combine(output,id+".png");yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(path);yield return null;
            screenshots.Add(id+".png");
        }
        private void Finish(bool success)
        {
            if(done)return;done=true;
            File.WriteAllText(Path.Combine(output,"result.json"),JsonUtility.ToJson(new Result{completed=success,actualStandalonePlayer=!Application.isEditor,
                utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,walkedMeters=walked,simulationTimeScale=Time.timeScale,rendered=SystemInfo.graphicsDeviceType!=UnityEngine.Rendering.GraphicsDeviceType.Null,
                method="Opt-in isolated P1-complete fixture. Production campaign transactions, capsule collision, NavMesh route, E/phone inputs and keypad callbacks. Directed smoke; optional headless mode tests physics/input/frustum math without rendered proof, optional 2x test simulation. Separate graphical review, not hands-on QA or Windows GPU performance acceptance.",checks=checks.ToArray(),screenshots=screenshots.ToArray()},true));
            Application.Quit(success?0:2);
        }
        private void OnDestroy(){Application.logMessageReceived-=Log;}
    }
}
#endif
