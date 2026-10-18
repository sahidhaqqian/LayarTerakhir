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
    // Opt-in continuation audit. A documented completed-ground-floor save is seeded;
    // stairs, rooms and escape use continuous capsule substeps and real action bindings.
    public sealed class PrologCompletionAudit : MonoBehaviour
    {
        [Serializable] private class Receipt
        {
            public string utc, platform, gpu, initialCheckpoint, navigationMethod;
            public bool actualStandalonePlayer, completed, checkpointSaved;
            public int width, height;
            public float walkedMeters;
            public string[] checks, screenshots;
        }
        private static string output;
        private static bool chaseOnly;
        private readonly List<string> checks = new List<string>(), shots = new List<string>();
        private Receipt receipt;
        private bool done;
        private float started, heartbeat;
        private AtmajaDirector d;
        private FirstPersonController player;
        private Camera eye;
        private Keyboard keyboard;
        private Mouse mouse;
        private Vector3 lastAim;
        private bool aimed;
        private HudController Hud => HudController.Instance;
        private SmartphoneController Phone => SmartphoneController.Instance;
        private CampaignState Campaign => CampaignState.Instance;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args = Environment.GetCommandLineArgs(); int i = Array.IndexOf(args, "-prologAudit"); if (i < 0 || i + 1 >= args.Length) return;
            output = Path.GetFullPath(args[i + 1]); Directory.CreateDirectory(output);
            chaseOnly=Array.IndexOf(args,"-prologChaseOnly")>=0;
            string save = Path.Combine(output, "IsolatedSave-" + Guid.NewGuid().ToString("N")); Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", save);
            var data = new CampaignData { checkpoint_id = "PROLOG_ATMAJA_STAIRS_UNLOCKED", battery = 63, wallet_bound = 500000, wallet_legitimate = 150000, spent = 5000000 };
            data.inventory.AddRange(new[] { "SMARTPHONE_RAKA", "KUNCI_MOTOR", "JAKET_RAKA", "POWERBANK_KECIL_KOS", "EVENT_PAID_RENT", "EVENT_PAID_HOSPITAL", "EVENT_CALLED_IBU", "EVENT_CALLED_RATNA", "EVENT_P0_CONTRACT_RECEIVED", "EVENT_P0_CONTRACT_READ", "EVENT_P0_GATE_OPEN", "EVENT_P0_ATMAJA_ARRIVED", AtmajaDirector.SurveyId, "EVENT_P0_LIVE_STARTED", "EVENT_P0_SIDE_CHAT", "EVENT_P0_TREE_CROUCHED", "EVENT_P0_KITCHEN_ENTERED", "EVENT_P0_DINING_OPEN", "EVENT_P0_WELL_BONUS", "MATABATIN_APK", "EVENT_P0_AR_REVEALED", "EVENT_P0_AR_CLUE_READ", "ITEM_P0_CLOCK_HANDS", "EVENT_P0_CLOCK_UNLOCKED", "KUNCI_TANGGA_TENGAH", "DOC_PROLOG_03_KORAN_1998", "EVENT_P0_STAIRS_UNLOCKED" });
            if(chaseOnly)
            {
                data.checkpoint_id=AtmajaChaseSequence.ChaseCheckpoint;data.battery=80;data.wallet_bound=2500000;
                data.inventory.AddRange(new[]{"EVENT_P0_UPPER_REACHED","EVENT_P0_CHAIN1_BROKEN","EVENT_P0_CHAIN2_BROKEN","EVENT_P0_CHAIN3_BROKEN","EVENT_P0_MASTER_OPEN","EVENT_P0_CARPET_LIFTED","EVENT_P0_TRAPDOOR_OPEN","EVENT_P0_BASEMENT_ENTERED","EVENT_P0_POWER_GLITCH","EVENT_P0_BASEMENT_SAWER","EVENT_P0_POWERBANK_TAKEN",AtmajaBasementHorror.HandycamId,"EVENT_P0_HANDYCAM_PLAYED","EVENT_P0_CHASE_STARTED"});
            }
            data.upgrades.Add("MATABATIN_AR"); File.WriteAllText(Path.Combine(save, "layar_terakhir_save.json"), JsonUtility.ToJson(data, true));
            Application.runInBackground = true; InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            var go = new GameObject("PrologCompletionAudit"); DontDestroyOnLoad(go); go.AddComponent<PrologCompletionAudit>();
        }
        private void Awake() { started = Time.realtimeSinceStartup; Application.logMessageReceived += OnLog; }
        private void OnLog(string text, string trace, LogType type) { if (type == LogType.Error || type == LogType.Exception) { checks.Add("ERROR " + text); Finish(false); } }
        private void Update()
        {
            if (Time.realtimeSinceStartup > heartbeat) { heartbeat = Time.realtimeSinceStartup + 20; Debug.Log("PROLOG_AUDIT_PROGRESS stage=" + (d ? d.Stage.ToString() : "loading") + " frame=" + Time.frameCount); }
            if (!done && Time.realtimeSinceStartup - started > 1800) { checks.Add("FAIL Audit timeout"); Finish(false); }
        }
        private IEnumerator Start()
        {
            receipt = new Receipt { utc = DateTime.UtcNow.ToString("o"), platform = Application.platform.ToString(), gpu = SystemInfo.graphicsDeviceName,
                actualStandalonePlayer = !Application.isEditor, width = Screen.width, height = Screen.height,
                initialCheckpoint = chaseOnly?"PROLOG_CHASE_START (isolated escape retry fixture)":"PROLOG_ATMAJA_STAIRS_UNLOCKED (isolated fixture with completed Kos/ground-floor progress)",
                navigationMethod = "Continuous CharacterController.Move <=0.12m substeps using deltaTime and the player's walk/crouch/sprint speeds, actual E/Ctrl/Space/F/LMB bindings, game-owned cellar/vault traversals; time frozen during framebuffer captures. Directed path verification, not manual pacing/Windows performance acceptance." };
            keyboard = InputSystem.AddDevice<Keyboard>(); mouse = InputSystem.AddDevice<Mouse>();
            yield return Drive(Scenario());
            if (!done) Finish(true);
        }
        private IEnumerator Scenario()
        {
            yield return Wait(() => SceneManager.GetActiveScene().name == PrologSceneFlow.AtmajaScene && FindFirstObjectByType<AtmajaDirector>(), 120, "Continue routes completed ground floor directly to Atmaja");
            yield return new WaitForSeconds(1.7f); Bind(); player.enabled = false;
            if(chaseOnly)goto Escape;
            Check(d.Stage == AtmajaStage.StairCheckpoint, "Continuation starts at the real stair checkpoint");
            var hybrid=player.GetComponent<ShoulderCameraRig>();
            Check(hybrid.SelectedPerspective==PlayerPerspective.OverShoulder,"Exploration defaults to over-the-shoulder third person");
            Check(hybrid.Avatar.GetComponent<AnimatedRakaAvatar>()&&hybrid.Avatar.GetComponent<Animation>().GetClipCount()==12,"Standalone loads the skinned Raka resource and twelve motion clips including memory-card interaction");
            // Controlled closed-leaf inspection inside the unlocked continuation fixture.
            // This checks presentation without replaying/re-paying the completed ground floor.
            yield return Walk(d.P(-3.55f,5.1f,.54f));d.stairDoorMotion.SetOpenImmediate(false);Physics.SyncTransforms();
            Aim(d.stairDoorLeaf.GetComponent<Collider>().bounds.center);yield return Shot("00-opaque-door-conceals-stairwell");
            d.stairDoorMotion.SetOpenImmediate(true);Physics.SyncTransforms();
            foreach(var p in new[]{d.P(-3.55f,5.1f,.54f),d.P(-3.55f,8.2f,.54f),d.P(-1.2f,8.2f,1.4f)})yield return Walk(p);
            Aim(d.P(.8f,8.2f,2.7f));yield return Shot("00a-rebuilt-lower-stair-flight");
            foreach(var p in new[]{d.P(1.05f,8.2f,2.2f),d.P(1.05f,6.925f,2.2f),d.P(-.8f,6.925f,3.1f)})yield return Walk(p);
            Aim(d.P(-3.3f,6.925f,4.55f));yield return Shot("00b-supported-return-stair-flight");
            foreach(var p in new[]{d.P(-3.3f,6.925f,3.82f),d.P(-3.3f,5.75f,3.82f)})yield return Walk(p);
            Check(Campaign.HasInventoryItem("EVENT_P0_UPPER_REACHED"), "Walking both flights triggers the upper-floor checkpoint");
            Aim(d.P(1.1f,5.1f,5.35f)); yield return Shot("01-upper-corridor-and-seals");
            var standingPosition=player.transform.position;long cameraWallet=Campaign.Data.wallet_bound;
            yield return ChangePerspective();Check(hybrid.IsFirstPerson&&Vector3.Distance(eye.transform.position,player.cameraPivot.position)<.01f,"V places the world camera at Raka's eyes");
            Aim(d.P(1.1f,5.1f,5.35f));yield return Shot("01a-first-person-same-corridor");
            Check(Vector3.Distance(player.transform.position,standingPosition)<.08f&&Campaign.Data.wallet_bound==cameraWallet,"Changing perspective preserves player position and wallet");
            yield return ChangePerspective();Check(!hybrid.IsFirstPerson,"Second V restores the shoulder view");
            yield return Walk(d.P(-2.05f,5.80f,3.83f));
            Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            Aim(d.P(-2.05f,5.365f,5.35f));yield return Shot("01b-scratched-family-photograph");
            yield return Walk(d.P(-3.0f,5.75f,3.83f)); RaiseAR(); yield return Crouch(true); Aim(d.upper.ventMouth.position); yield return Shot("02-ar-broken-vent");
            yield return Walk(d.P(-3.0f,4.45f,3.83f)); yield return Crouch(false);
            Aim(d.P(-3.65f,2.6f,4.4f));yield return Shot("02a-child-bedroom-layout");
            var mattress=GameObject.Find("ChildMattress").GetComponent<Renderer>().bounds;
            Check(mattress.size.y<.20f&&mattress.min.y>4.3f&&mattress.max.y<4.7f,"Bima's mattress lies horizontally on the bed frame");
            foreach (var p in new[] { d.P(-4.8f,4.45f,3.83f),d.P(-5.1f,3.6f,3.83f),d.P(-4.65f,2.0f,3.83f),d.P(-4.65f,1.65f,3.83f) }) yield return Walk(p);
            yield return Use(Item(AtmajaAction.Diary));
            Check(Phone.isPhoneRaised&&Phone.isScreenOn&&d.phoneView.Panel.activeSelf&&d.phoneView.FeedCamera.enabled,"Reading Bima's diary keeps the held handset and live camera feed visible");
            yield return Shot("03-bima-diary"); Hud.CloseDocumentModal();
            RaiseAR(); Aim(d.upper.cupAim.position); yield return null; yield return null; yield return ClickMouse();
            Check(d.upper.ChainsBroken == 1, "Actual phone lens photographs the milk cup and breaks chain one"); yield return Shot("04-first-evidence");
            foreach (var p in new[] { d.P(-5.1f,3.6f,3.83f),d.P(-4.8f,4.45f,3.83f),d.P(-4.4f,4.45f,3.83f) }) yield return Walk(p);
            yield return Use(d.upper.childDoor); yield return new WaitForSeconds(1.1f); yield return Walk(d.P(-4.4f,5.75f,3.83f));
            yield return Walk(d.P(-1.1f,5.75f,3.83f)); yield return Use(d.upper.bathDoor); yield return new WaitForSeconds(1.1f);
            yield return ChangePerspective();Check(hybrid.IsFirstPerson,"Bathroom interactions and AR photography also run in first person");
            yield return Walk(d.P(-1.1f,4.4f,3.83f)); Aim(d.upper.mirrorSpot.position); yield return new WaitForSeconds(.3f); yield return Shot("05-bathroom-mirror");
            yield return Walk(d.P(-1.1f,3.0f,3.83f)); yield return Use(Item(AtmajaAction.BathCurtain)); yield return Shot("06-open-bath-curtain");
            RaiseAR(); yield return Walk(d.P(-1.1f,3.8f,3.83f)); Aim(d.upper.scratchAim.position); yield return null; yield return null; yield return ClickMouse();
            Check(d.upper.ChainsBroken == 2, "AR scratches follow the opened bathroom door and photo breaks chain two"); yield return Shot("07-second-evidence");
            yield return ChangePerspective();Check(!hybrid.IsFirstPerson,"Third-person exploration resumes after the second photograph");
            yield return Walk(d.P(-1.1f,5.75f,3.83f)); yield return Walk(d.P(3.4f,5.75f,3.83f)); yield return Walk(d.P(3.4f,4.25f,3.83f));
            yield return Use(Item(AtmajaAction.WillLetter)); yield return Shot("08-nyonya-last-letter"); Hud.CloseDocumentModal();
            RaiseAR(); Aim(d.upper.ropeAim.position); yield return null; yield return null; yield return ClickMouse();
            Check(d.upper.ChainsBroken == 3 && !d.upper.masterDoor.locked, "Three photographed tragedies unlock the master bedroom"); yield return Shot("09-third-evidence-balcony");
            yield return Walk(d.P(3.4f,5.75f,3.83f)); yield return Walk(d.P(1.3f,5.75f,3.83f)); yield return new WaitForSeconds(1.1f);
            foreach (var p in new[] { d.P(1.1f,4.25f,3.83f),d.P(.8f,4.25f,3.83f),d.P(.8f,2.6f,3.83f) }) yield return Walk(p);
            yield return Use(Item(AtmajaAction.CarpetLift)); yield return Shot("10-master-trapdoor"); yield return Use(Item(AtmajaAction.Trapdoor));
            yield return Wait(() => !d.Busy && d.Stage >= AtmajaStage.BasementEntry, 30, "Trapdoor traverse enters the cellar");
            player.enabled = false; Check(Phone.batteryPercent <= 12.01f && Phone.batteryPercent > 11, "Scripted power disturbance locks indicator to twelve percent once");
            Check(Campaign.HasInventoryItem("EVENT_P0_BASEMENT_SAWER"), "Cellar donation is included in the atomic entry checkpoint");
            Aim(d.P(4.3f,3.9f,-1.4f)); yield return Shot("11-cellar-emergence");
            yield return Walk(d.basement.exitAnchor1.position); Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.SyncVisualState();
            yield return Use(d.basement.wardrobeCol1.GetComponent<AtmajaItem>());
            Check(d.basement.IsHidden && player.MovementLocked, "Actual wardrobe interaction locks movement inside the slats");
            Check(hybrid.IsFirstPerson&&hybrid.SelectedPerspective==PlayerPerspective.OverShoulder,"Wardrobe uses a temporary first-person closeup without changing the selected view");
            yield return Press(Key.F); Check(!Phone.isScreenOn, "Screen-off binding works while concealed");
            yield return Wait(() => d.basement.InspectingLocker, 90, "Hunter reaches the occupied wardrobe"); yield return Shot("12-locker-inspection-screen-off");
            yield return Wait(() => !d.basement.InspectingLocker, 15, "Four-second inspection completes with the phone dark");
            yield return new WaitForSeconds(3);yield return Press(Key.E); Check(!d.basement.IsHidden && !player.MovementLocked, "Player can leave the wardrobe after the hunter passes");
            Check(!hybrid.IsFirstPerson,"Leaving the wardrobe restores the selected shoulder perspective");
            yield return Crouch(true);
            foreach(var p in new[]{d.P(1.45f,4.0f,-2.79f),d.P(1.45f,5.4f,-2.79f),d.P(2.1f,5.45f,-2.79f)})yield return Walk(p);
            yield return Crouch(false);yield return Use(Item(AtmajaAction.Powerbank));
            Check(Phone.batteryPercent>=79.5f,"Large powerbank restores charge and preserves the carried small pack");
            Check(Campaign.HasInventoryItem("POWERBANK_KECIL_KOS"),"Carried powerbank is not deleted by the battery tutorial");
            Phone.isPhoneRaised=true;Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);Aim(d.basement.powerbankObj.transform.position);yield return Shot("13-damar-powerbank");
            yield return Crouch(true);yield return Walk(d.P(1.45f,5.45f,-2.79f));yield return Use(Item(AtmajaAction.Handycam));
            yield return Wait(()=>d.chase.recording.IsPlaying&&d.chase.recording.Progress>.2f,30,"Handycam plays the original visual recording");yield return Shot("14-recording-market-behind-mirror");
            yield return Wait(()=>d.chase.recording.Progress>.72f,30,"Pale hands reach through the recorded mirror");yield return Shot("15-recording-damar-dragged-in");
            Escape:
            yield return Wait(()=>d.chase.IsPursuing,30,"Recording ends in an atomic chase checkpoint");
            Check(Campaign.Data.checkpoint_id==AtmajaChaseSequence.ChaseCheckpoint,"Escape can be retried without replaying the full prolog");
            Check(d.chase.FollowsPlayerInBasement,"After the handycam, the hunter navigates toward Raka in the basement");
            Check(Mathf.Abs(d.chase.chaserFigure.transform.position.y-player.transform.position.y)<.5f,"Handycam pursuit begins on the player's basement floor");
            yield return Walk(d.P(1.4f,4.3f,-2.79f));
            // The handycam is reached crouched. Release Ctrl in the clear aisle
            // before sprinting; keeping the disabled controller's crouch latch
            // here incorrectly simulated crawling away from the chase.
            yield return Crouch(false);
            foreach(var p in new[]{d.P(1.4f,2.8f,-2.79f),d.chase.cellarExitApproach.position})yield return Walk(p);
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.LeftShift));yield return null;
            yield return Wait(()=>d.chase.Traversing,10,"Sprint initiates the cellar stair escape");yield return Wait(()=>!d.chase.Traversing,15,"Cellar ascent reaches the closing trapdoor");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return Crouch(true);yield return Shot("16-slide-under-closing-trapdoor");
            yield return Walk(d.P(1.0f,2.5f,3.83f));yield return Walk(d.P(.8f,2.7f,3.83f));yield return Crouch(false);
            foreach(var p in new[]{d.P(.8f,4.25f,3.83f),d.P(1.1f,4.7f,3.83f),d.P(1.3f,5.75f,3.83f),d.P(-3.3f,5.75f,3.83f),d.P(-3.3f,6.925f,3.83f),d.P(1.05f,6.925f,2.2f),d.P(1.05f,8.2f,2.2f),d.P(-3.55f,8.2f,.54f),d.P(-3.55f,5.1f,.54f)})yield return Walk(p);
            Aim(d.P(-1.4f,4.1f,1.6f));yield return Shot("17-chase-down-the-stairs");
            foreach(var p in new[]{d.P(-1.4f,4.1f,.54f),d.P(.8f,4.1f,.54f),d.P(2.1f,4.1f,.54f),d.P(2.1f,1.3f,.54f),d.P(5.35f,1.3f,.54f),d.chase.diningVaultStart.position})yield return Walk(p);
            Aim(d.chase.diningVaultEnd.position+Vector3.up);yield return Shot("18-overturned-dining-furniture");yield return Press(Key.Space);
            yield return Wait(()=>!d.chase.Traversing&&d.player.transform.position.z!=0,10,"Vault animation finishes");
            Check(Vector3.Distance(player.transform.position,d.chase.diningVaultEnd.position)<.25f,"Space actually carries Raka across the fallen table");
            foreach(var p in new[]{d.P(4.1f,8.4f,.54f),d.P(4.1f,9.8f,.37f),d.P(4.9f,10.5f,.37f)})yield return Walk(p);
            yield return Wait(()=>d.chase.AwaitingBlitz,10,"Nyonya blocks the kitchen window in slow motion");
            Check(player.GetComponent<ShoulderCameraRig>().IsFirstPerson,"Emergency Blitz uses the first-person aiming closeup");Aim(d.chase.windowBlockSpot.position+Vector3.up*1.3f);yield return Shot("19-emergency-blitz-standoff");
            float battery=Phone.batteryPercent;yield return ClickMouse();Check(d.chase.IsStunned&&Time.timeScale==1,"Real left click fires the emergency Blitz and restores normal time");
            Check(Mathf.Abs(battery-Phone.batteryPercent-15)<.25f,"Emergency Blitz costs fifteen percent once");yield return Press(Key.Space);
            yield return Wait(()=>d.chase.IsGateRun,10,"Space breaks through the kitchen window into the side yard");yield return Shot("20-kitchen-window-escape");
            foreach(var p in new[]{d.P(8.2f,7.0f,.06f)})yield return Walk(p);yield return Crouch(true);yield return Walk(d.P(8.2f,4.1f,.06f));yield return Crouch(false);
            foreach(var p in new[]{d.P(8.2f,-3,.06f),d.P(0,-3,.06f),d.P(0,-8.3f,.06f)})yield return Walk(p);
            yield return Wait(()=>d.Stage==AtmajaStage.EndingVista,15,"Reaching the gate begins the street ending");yield return new WaitForSeconds(3.2f);yield return Shot("21-street-and-second-floor-silhouette");
            yield return Wait(()=>GameObject.Find("AcknowledgeFinalContract"),30,"Live chat freezes and the final donor message appears on the handset");yield return Shot("22-final-contract-on-handset");
            Check(d.Viewers==54820,"Final livestream reaches 54,820 viewers");
            GameObject.Find("AcknowledgeFinalContract").GetComponent<Button>().onClick.Invoke();
            yield return Wait(()=>d.chase.finale.IsCompletionVisible,15,"The complete prolog ends with the title and menu");yield return Shot("23-prolog-complete-title");
            Check(Campaign.Data.checkpoint_id=="PROLOG_COMPLETE"&&Campaign.HasInventoryItem("PROLOG_COMPLETE"),"Prolog completion is persisted");
            Check(Campaign.Data.wallet_bound+Campaign.Data.wallet_legitimate+Campaign.Data.spent==25450000,"Cumulative donations include the well bonus and preserve previous payments");
            Check(Campaign.Data.location_seals.Count==0,"The prolog grants no location seal");
            Check(d.CheckpointRestores==0,"The complete continuation and escape succeed without a hidden checkpoint reset");
            long wallet=Campaign.Data.wallet_bound;Check(Campaign.LoadCheckpoint(),"Completed checkpoint reloads");d.RestoreWorld();yield return null;yield return null;
            Check(d.chase.finale.IsCompletionVisible&&Campaign.Data.wallet_bound==wallet,"Continue restores the completed title without repeating the final payout");yield return Shot("24-continue-completed-prolog");
            receipt.checkpointSaved=true;
        }
        private void Bind(){d=FindFirstObjectByType<AtmajaDirector>();player=d.player;eye=player.GetComponent<InteractionInteractor>().playerCamera;}
        private IEnumerator ChangePerspective(){player.enabled=true;yield return Press(Key.V);player.enabled=false;}
        private void RaiseAR(){Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.currentScanMode=ScanVisualMode.AR;Phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);}
        private void Aim(Vector3 target)
        {
            lastAim=target;aimed=true;
            Vector3 delta=target-player.cameraPivot.position;Vector3 flat=delta;flat.y=0;if(flat.sqrMagnitude>.001f)player.transform.rotation=Quaternion.LookRotation(flat);
            player.cameraPivot.rotation=Quaternion.LookRotation(delta);var rig=player.GetComponent<ShoulderCameraRig>();if(rig)rig.RefreshCameraPose();
            // World interaction uses the shoulder camera ray; phone photographs use the
            // independent handset lens at the pivot. Account for the shoulder parallax.
            eye.transform.LookAt(target);
        }
        private IEnumerator Walk(Vector3 target)
        {
            if(d.chase.IsPursuing)Debug.Log($"PROLOG_CHASE_WALK from={player.transform.position} target={target} crouch={player.IsCrouching} time={Time.time} speed={player.crouchSpeed}/{player.sprintSpeed}");
            var cc=player.GetComponent<CharacterController>();player.enabled=false;int stalled=0;aimed=false;int restores=d.CheckpointRestores;
            for(int frame=0;frame<700;frame++)
            {
                if(d.Stage==AtmajaStage.EndingVista&&UiFocus.Cinematic){Check(true,"Gate approach hands control to the street ending");yield break;}
                if(d.chase.AwaitingBlitz){Check(true,"The window standoff locks movement and hands control to the emergency Blitz");yield break;}
                if(d.CheckpointRestores!=restores)Check(false,"Checkpoint reset while walking toward "+target);
                var delta=target-player.transform.position;delta.y=0;if(delta.magnitude<.12f)break;
                Vector3 before=player.transform.position;
                player.transform.rotation=Quaternion.LookRotation(delta);player.cameraPivot.localRotation=Quaternion.Euler(7,0,0);
                float speed=player.IsCrouching?player.crouchSpeed:(d.chase.IsPursuing||d.chase.IsGateRun)?player.sprintSpeed:player.walkSpeed;
                float stepBudget=speed*Time.deltaTime;
                while(stepBudget>.0001f){delta=target-player.transform.position;delta.y=0;if(delta.magnitude<.06f)break;float step=Mathf.Min(.12f,stepBudget);cc.Move(Vector3.ClampMagnitude(delta,step)+Vector3.down*.10f);stepBudget-=step;}
                receipt.walkedMeters+=Vector3.Distance(before,player.transform.position);
                if(Vector3.Distance(before,player.transform.position)<.002f)stalled++;else stalled=0;
                if(stalled==3)
                {
                    Vector3 at=player.transform.position+delta.normalized*.18f;
                    var hits=Physics.OverlapCapsule(at+Vector3.up*.32f,at+Vector3.up*(cc.height-.32f),.36f,~0,QueryTriggerInteraction.Ignore);
                    Debug.Log("PROLOG_STALL_EARLY feet="+player.transform.position+" height="+cc.height+" stage="+d.Stage+" hits="+string.Join("; ",hits.Where(b=>!b.transform.IsChildOf(player.transform)).Select(b=>b.name+" "+b.bounds)));
                }
                player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
                if(stalled>12)break;yield return null;
            }
            var remaining=target-player.transform.position;remaining.y=0;
            if(remaining.magnitude>=.17f)
            {
                Vector3 next=player.transform.position+remaining.normalized*.18f;
                var blocks=Physics.OverlapCapsule(next+Vector3.up*.32f,next+Vector3.up*(cc.height-.32f),.36f,~0,QueryTriggerInteraction.Ignore);
                Debug.Log("PROLOG_ROUTE_BLOCKED stage="+d.Stage+" checkpoint="+Campaign.Data.checkpoint_id+" hits="+string.Join("; ",blocks.Where(b=>!b.transform.IsChildOf(player.transform)).Select(b=>b.name+" "+b.bounds)));
            }
            Check(remaining.magnitude<.17f,"Capsule route reaches "+target+" from "+player.transform.position);
        }
        private AtmajaItem Item(AtmajaAction action)=>FindObjectsByType<AtmajaItem>(FindObjectsInactive.Include,FindObjectsSortMode.None).First(i=>i.action==action);
        private IEnumerator Use(IInteractable item)
        {
            var component=(Component)item;var col=component.GetComponent<Collider>();Vector3 target=col?col.bounds.center:component.transform.position;
            Aim(target);yield return null;yield return null;
            var debugHits=Physics.RaycastAll(eye.transform.position,eye.transform.forward,8,~0,QueryTriggerInteraction.Collide).OrderBy(h=>h.distance);
            Debug.Log("PROLOG_INTERACTION_RAY "+component.name+" player="+player.transform.position+" target="+target+" stage="+d.Stage+" restores="+d.CheckpointRestores+" active="+component.gameObject.activeInHierarchy+" => "+string.Join("; ",debugHits.Select(h=>h.collider.name+" "+h.distance)));
            var interactor=player.GetComponent<InteractionInteractor>();Check(ReferenceEquals(interactor.CurrentTarget,item),"Actual interaction ray reaches "+component.name+" (hit "+(interactor.CurrentTarget?.GetInteractionPrompt()??"nothing")+")");
            yield return Press(Key.E);
        }
        private IEnumerator Crouch(bool on)
        {
            InputSystem.QueueStateEvent(keyboard,on?new KeyboardState(Key.LeftCtrl):new KeyboardState());player.enabled=true;
            // Wait for the real stance transition instead of inserting a fixed stop
            // on both sides of the trapdoor. That test-only pause spent the chase lead.
            var capsule=player.GetComponent<CharacterController>();float target=on?player.crouchHeight:player.standingHeight;
            float until=Time.realtimeSinceStartup+5;
            do{yield return null;}while((player.IsCrouching!=on||Mathf.Abs(capsule.height-target)>.05f)&&Time.realtimeSinceStartup<until);
            player.enabled=false;
            Check(player.IsCrouching==on,"Crouch binding sets "+on);
            Check(Mathf.Abs(capsule.height-target)<=.05f,"Real capsule completes the requested stance");
        }
        private IEnumerator Press(Key key){InputSystem.QueueStateEvent(keyboard,new KeyboardState(key));yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
        private IEnumerator ClickMouse(){InputSystem.QueueStateEvent(mouse,new MouseState{buttons=1});yield return null;InputSystem.QueueStateEvent(mouse,new MouseState());yield return null;}
        private IEnumerator Wait(Func<bool> condition,float timeout,string label)
        {float until=Time.realtimeSinceStartup+timeout;while(!condition()&&Time.realtimeSinceStartup<until)yield return null;Check(condition(),label);}
        private IEnumerator Shot(string name)
        {
            var flash=GameObject.Find("BlitzFlash")?.GetComponent<Image>();
            while(flash&&flash.color.a>.02f&&!UiFocus.Cinematic)yield return null;
            if(player&&!UiFocus.Cinematic){if(aimed)Aim(lastAim);else player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();}
            float scale=Time.timeScale;Time.timeScale=0;yield return null;yield return new WaitForEndOfFrame();
            string path=Path.Combine(output,name+".png");ScreenCapture.CaptureScreenshot(path);yield return null;yield return new WaitForEndOfFrame();Time.timeScale=scale;
            shots.Add(name+".png");Debug.Log("PROLOG_AUDIT_SHOT "+name);
        }
        private void Check(bool value,string label)
        {checks.Add((value?"PASS ":"FAIL ")+label);Debug.Log("PROLOG_AUDIT "+checks[checks.Count-1]);if(!value)throw new Exception(label);}
        private IEnumerator Drive(IEnumerator scenario)
        {
            var stack=new Stack<IEnumerator>();stack.Push(scenario);
            while(stack.Count>0&&!done)
            {
                bool more;object current=null;
                try{more=stack.Peek().MoveNext();if(more)current=stack.Peek().Current;}
                catch(Exception e){checks.Add("FAIL "+e);Finish(false);yield break;}
                if(!more){stack.Pop();continue;}if(current is IEnumerator nested)stack.Push(nested);else yield return current;
            }
        }
        private void Finish(bool success)
        {
            if(done)return;done=true;
            if(receipt!=null){receipt.completed=success;receipt.screenshots=shots.ToArray();receipt.checks=checks.ToArray();File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(receipt,true)+"\n");}
            Debug.Log("PROLOG_AUDIT_FINISHED "+success);Application.Quit(success?0:1);
        }
        private void OnDestroy(){Application.logMessageReceived-=OnLog;if(keyboard!=null)InputSystem.RemoveDevice(keyboard);if(mouse!=null)InputSystem.RemoveDevice(mouse);}
    }
}
#endif
