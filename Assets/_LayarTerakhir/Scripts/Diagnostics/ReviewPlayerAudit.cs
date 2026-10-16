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
    // Directed standalone regression, never active in normal play. Kos item framing is posed;
    // corridor/stairs/Atmaja walks use small continuous CharacterController.Move steps, not poses.
    public sealed class ReviewPlayerAudit:MonoBehaviour
    {
        [Serializable] private class Receipt
        {
            public string utc,platform,gpu,saveDirectory,navigationMethod;
            public bool actualStandalonePlayer,completed,checkpointSaved,transitionFromKos,motorPassedGate,previousSaveArchived;
            public bool acceleratedStoryTimers;
            public int width,height;public float walkedMeters,maximumInteriorFootY,mixRms,mixPeak;
            public string[] screenshots,checks;
        }
        private static string output,priorSlot;
        private readonly List<string> shots=new List<string>(),checks=new List<string>();
        private Receipt receipt;private bool failed,done,insideAtmaja;private float started,nextHeartbeat;
        private FirstPersonController player;private Camera eye;private CharacterController capsule;
        private AtmajaDirector d;private Keyboard keyboard;private GoldenAudioTap tap;
        private KosCorridorDirector watchedCorridor;
        private HudController Hud=>HudController.Instance;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        private AtmajaPhoneView View=>FindFirstObjectByType<AtmajaPhoneView>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-reviewAudit");if(i<0||i+1>=args.Length)return;
            output=Path.GetFullPath(args[i+1]);Directory.CreateDirectory(output);
            string save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            priorSlot=JsonUtility.ToJson(new CampaignData{checkpoint_id="PROLOG_ATMAJA_KITCHEN",wallet_bound=12345},true);
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),priorSlot);
            Application.runInBackground=true;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            var go=new GameObject("ReviewPlayerAudit");DontDestroyOnLoad(go);go.AddComponent<ReviewPlayerAudit>();
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.SetStackTraceLogType(LogType.Log,StackTraceLogType.None);Application.logMessageReceived+=OnLog;}
        private void OnLog(string message,string stack,LogType type)
        {if(type==LogType.Error||type==LogType.Exception){failed=true;checks.Add("ERROR "+message);}}
        private void Update()
        {
            if(receipt!=null&&watchedCorridor&&watchedCorridor.motor&&watchedCorridor.motor.position.z<-7.5f)receipt.motorPassedGate=true;
            if(Time.realtimeSinceStartup>nextHeartbeat){nextHeartbeat=Time.realtimeSinceStartup+15;Debug.Log("REVIEW_PROGRESS frame="+Time.frameCount+" gameTime="+Time.time+" scale="+Time.timeScale+" speaker="+(Hud?Hud.CurrentDialogueSpeaker:"loading"));}
            if(!done&&(failed||Time.realtimeSinceStartup-started>2400))Finish(false);
        }
        private void OnDestroy(){Application.logMessageReceived-=OnLog;if(keyboard!=null)InputSystem.RemoveDevice(keyboard);}
        private void BindPlayer()
        {
            player=FindFirstObjectByType<FirstPersonController>();capsule=player.GetComponent<CharacterController>();
            eye=player.GetComponent<InteractionInteractor>().playerCamera;
        }
        private IEnumerator Start()
        {
            yield return null;yield return null;
            receipt=new Receipt{utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,
                actualStandalonePlayer=!Application.isEditor,width=Screen.width,height=Screen.height,
                saveDirectory=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY"),
                navigationMethod="Posed compact Golden-room interactions; continuous <=0.16m CharacterController substeps (4/frame) in kos corridors/stairs and Atmaja. Game-owned wake/vault/motor cinematics. Shot capture freezes game time briefly; story waits accelerated after wake. Not manual, natural VO timing, or hardware performance acceptance."};
            keyboard=InputSystem.AddDevice<Keyboard>();BindPlayer();
            Check(GameShell.AwaitingStart&&UiFocus.Menu&&Time.timeScale==0,"Standalone starts at actual main menu, not a room spawn");
            Check(Cursor.lockState==CursorLockMode.None&&Button("Continue").interactable,"Main menu has an unlocked cursor and validates Continue");
            yield return Shot("01-main-menu");Click("Settings");yield return null;
            var slider=FindObjectsByType<Slider>(FindObjectsSortMode.None).First(s=>s.name=="MasterVolume");slider.value=.8f;
            Check(Mathf.Abs(AudioListener.volume-.8f)<.01f,"Settings volume slider controls the real listener");
            yield return Shot("02-settings");Click("BackSettings");yield return null;Click("NewGame");yield return null;Click("ConfirmNew");
            yield return Wait(()=>!GameShell.MenuSceneLoading&&FindFirstObjectByType<KamarKosPrologDirector>()!=null,90,"New Game scene load");
            BindPlayer();tap=eye.gameObject.AddComponent<GoldenAudioTap>();
            Time.maximumDeltaTime=2;Time.fixedDeltaTime=.08f;
            receipt.previousSaveArchived=Directory.GetFiles(Path.Combine(receipt.saveDirectory,"ArchivedCampaigns")).Any(p=>File.ReadAllText(p)==priorSlot);
            Check(receipt.previousSaveArchived,"New Game preserves the complete previous save in ArchivedCampaigns");
            yield return new WaitForSeconds(1.2f);Check(UiFocus.Cinematic,"New Game plays the bed wake cinematic with input owned");yield return Shot("03-bed-wake");
            receipt.acceleratedStoryTimers=true;Time.timeScale=4;
            var kos=FindFirstObjectByType<KamarKosPrologDirector>();
            yield return Wait(()=>kos.environmentAudio.OpeningKnockCount>0,150,"Physical door knocks start after Raka's opening line");
            yield return Shot("04-door-knocks-no-caption");
            yield return Wait(()=>kos.currentStage==KosPrologStage.WaitReadSuratIbuKos,60,"Opening completes before letter interaction");
            Check(kos.environmentAudio.OpeningKnockCount==3&&kos.environmentAudio.DepartingFootstepCount==6&&!Hud.HasPlayedVoice("Bu Haji Ratna"),"Three actual knocks precede six departing footsteps; no landlady VO");
            if(tap){var levels=tap.Save(Path.Combine(output,"actual-opening-mix.wav"));receipt.mixRms=levels.x;receipt.mixPeak=levels.y;}
            yield return KosUse(kos.suratKosItem,new Vector3(.85f,.05f,-.25f));yield return Shot("05-landlady-letter");Hud.documentCloseButton.onClick.Invoke();
            yield return Wait(()=>kos.currentStage==KosPrologStage.WaitPickupSmartphone,20,"Phone wakes after warning letter");
            yield return KosUse(kos.smartphoneBedItem,new Vector3(-.8f,.05f,-.98f));
            yield return Wait(()=>Hud.HasPlayedVoice("Damar"),40,"Damar voice clip starts through the real audio source");
            Check(View.Apps.CallActive&&View.Apps.CurrentApp==PhoneApp.Call&&!Hud.IsModalOpen,"Pickup directly connects Damar call; no play-message document");
            yield return Shot("06a-automatic-damar-call");
            yield return Wait(()=>kos.currentStage==KosPrologStage.PreparationAndDrawerPuzzle,90,"Damar call finishes and contract arrives on handset");
            yield return Shot("07-phone-messages");
            Phone.isPhoneRaised=true;Phone.SyncVisualState();View.Apps.Open(PhoneApp.Home);yield return Shot("06-phone-apps");
            View.Apps.Open(PhoneApp.Bank);yield return null;Check(GameObject.Find("BankBalance").GetComponent<Text>().text.Contains("5.000.000"),"Bank app reads real contract funds");yield return Shot("07b-phone-bank");
            View.Apps.Open(PhoneApp.Inventory);yield return null;var inventory=GameObject.Find("InventoryEntries").GetComponent<Text>().text;Check(inventory.Contains("Smartphone Raka")&&!inventory.Contains("Rp"),"Phone inventory contains items, not donation balances");yield return Shot("07c-phone-inventory");
            View.Apps.Open(PhoneApp.Journal);yield return null;Check(GameObject.Find("JournalEntries").GetComponent<Text>().text.Contains("Surat peringatan kos"),"Phone journal app reads the actual discovered letter");
            Stow();
            yield return KosUse(kos.stickyNoteItem,new Vector3(.85f,.05f,-.05f));Check(!Hud.documentBodyText.text.Contains("1408"),"Sticky-note clue does not print the answer");Hud.documentCloseButton.onClick.Invoke();
            yield return KosUse(kos.wallCalendarItem,new Vector3(.85f,.05f,.5f));Check(!Hud.documentBodyText.text.Contains("14 - 08"),"Calendar requires deduction instead of printing padlock code");Hud.documentCloseButton.onClick.Invoke();
            Aim(kos.pcMonitorItem.transform.position);yield return Shot("07d-flush-monitor-and-desktop-key");
            yield return KosUse(kos.lockedDrawerItem,new Vector3(.95f,.05f,-.65f));
            Click("UnlockPadlockBtn");Check(!kos.isDrawerUnlocked,"Wrong drawer code stays locked");
            for(int slot=0;slot<4;slot++)for(int n=0;n<new[]{1,4,0,8}[slot];n++)Click("DigitUp_"+slot);
            yield return Shot("07e-four-wheel-padlock");Click("UnlockPadlockBtn");yield return null;
            Check(!UiFocus.Phone&&!Hud.IsModalOpen&&!player.MovementLocked&&!kos.hasMotorKey,"Drawer closes without trapping world input; keys not hidden in drawer");
            yield return Walk(new Vector3(.4f,.05f,-.15f));yield return Use(kos.motorKeyItem,kos.motorKeyItem.GetComponent<Collider>().bounds.center);
            yield return Walk(new Vector3(1.05f,.05f,-.4f));Aim(kos.jacketItem.GetComponent<Collider>().bounds.center);yield return Shot("07f-visible-jacket");
            yield return Use(kos.jacketItem,kos.jacketItem.GetComponent<Collider>().bounds.center);
            Check(kos.currentStage==KosPrologStage.ReadyToDepart&&CampaignState.Instance.Data.wallet_bound==5000000,"Preparation grants real motor items and donation only once");
            yield return Walk(new Vector3(.9f,.05f,-.6f));yield return Use(kos.kosDoorItem,kos.kosDoorItem.GetComponent<Collider>().bounds.center);
            yield return Wait(()=>SceneManager.GetActiveScene().name==PrologSceneFlow.CorridorScene,120,"Room door leads to second-floor corridor");
            yield return new WaitForSeconds(.8f);BindPlayer();player.enabled=false;
            var corridor=FindFirstObjectByType<KosCorridorDirector>();
            watchedCorridor=corridor;
            Check(player.transform.position.y>3,"Leaving Raka's room arrives on floor 2, not Atmaja");
            yield return Shot("08-kos-floor-two");
            // The map is accessible anywhere but cannot teleport the player from upstairs.
            yield return Press(Key.M);yield return null;Check(View.Apps.CurrentApp==PhoneApp.Map&&!View.Apps.TravelSelection,"Map opens upstairs but departure is gate-only");Stow();
            yield return Walk(new Vector3(-.60f,3.3f,12.7f));Aim(new Vector3(-.60f,1.9f,16.4f));yield return Shot("09-kos-stairs");
            yield return Walk(new Vector3(-.60f,1.65f,16.4f));yield return Walk(new Vector3(1.75f,1.65f,16.4f));
            yield return Walk(new Vector3(1.75f,0,12.6f));yield return Walk(new Vector3(1.75f,0,11.5f));yield return Walk(new Vector3(0,0,11.5f));
            Check(player.transform.position.y<.15f,"Both complete stair flights are physically descended without teleporting");
            yield return Walk(new Vector3(0,0,4.25f));
            var kitchen=GameObject.Find("DAPUR BERSAMADoor").GetComponent<HingedDoor>();yield return Use(kitchen,kitchen.GetComponent<Collider>().bounds.center);
            yield return new WaitForSeconds(1.2f);yield return Walk(new Vector3(-2.7f,0,4.25f));Aim(new Vector3(-4.2f,1,4.8f));yield return Shot("10-kos-shared-kitchen");
            yield return Walk(new Vector3(0,0,4.25f));var bath=GameObject.Find("KAMAR MANDIDoor").GetComponent<HingedDoor>();yield return Use(bath,bath.GetComponent<Collider>().bounds.center);
            yield return new WaitForSeconds(1.2f);yield return Walk(new Vector3(2.5f,0,4.25f));Aim(new Vector3(4.1f,.6f,5.25f));yield return Shot("11-kos-shared-bathroom");
            yield return Walk(new Vector3(0,0,4.25f));yield return Walk(new Vector3(0,0,-4.7f));Aim(corridor.motor.position+Vector3.up*.2f);
            yield return Use(corridor.motor.GetComponent<KosTravelItem>(),corridor.motor.GetComponent<Collider>().bounds.center);
            Check(View.Apps.TravelSelection&&corridor.CanDepart,"Motor interaction opens a valid destination map at the kos gate");yield return Shot("12-departure-map");
            // Cinematic movement needs multiple real samples even on a very slow software renderer.
            Time.timeScale=1;Time.maximumDeltaTime=.5f;Click("AtmajaDestination");yield return new WaitForSeconds(3);yield return Shot("13-mounting-motorcycle");
            Check(corridor.motorHeadlight&&corridor.motorHeadlight.enabled,"Motor engine sequence turns on the real lens-mounted headlight");
            yield return Wait(()=>receipt.motorPassedGate,150,"Motorcycle actually crosses kos gate");
            yield return Shot("14-riding-through-gate");
            yield return Wait(()=>SceneManager.GetActiveScene().name==PrologSceneFlow.AtmajaScene,150,"Road departure transitions to Atmaja");
            receipt.transitionFromKos=true;yield return new WaitForSeconds(1.1f);BindPlayer();player.enabled=false;d=FindFirstObjectByType<AtmajaDirector>();
            // Faster remaining narration waits only. CC walking is frame-stepped and never uses this scale.
            receipt.acceleratedStoryTimers=true;Time.timeScale=3;Time.maximumDeltaTime=.5f;
            Aim(d.P(0,-7.0f,1.2f));yield return Shot("15-atmaja-arrival");
            yield return Walk(d.P(0,-11.4f,.06f));yield return Use(Item(AtmajaAction.Gate),Item(AtmajaAction.Gate).GetComponent<Collider>().bounds.center);
            yield return new WaitForSeconds(1.2f);yield return Walk(d.P(0,-6.2f,.06f));yield return Walk(d.P(4.8f,-6.4f,.06f));
            yield return Use(Item(AtmajaAction.Survey),Item(AtmajaAction.Survey).GetComponent<Collider>().bounds.center);yield return Shot("16-damar-survey");Hud.documentCloseButton.onClick.Invoke();
            Phone.isPhoneRaised=true;Phone.isScreenOn=true;Phone.SyncVisualState();View.Apps.Open(PhoneApp.Camera);yield return null;
            View.LiveButton.onClick.Invoke();yield return null;Check(Phone.isBroadcastConnected&&d.Viewers==14,"Camera app starts the real 14-viewer live broadcast");Stow();
            yield return Walk(d.P(8.2f,-6.4f,.06f));yield return Walk(d.P(8.2f,4.15f,.06f));Phone.isPhoneRaised=true;Phone.isScreenOn=true;View.Apps.Open(PhoneApp.Camera);Phone.SyncVisualState();Aim(d.P(8.2f,5.5f,1.2f));yield return Shot("17-natural-kamboja-right-route");
            Check(CampaignState.Instance.Data.wallet_bound==5500000,"Side-passage donation is applied once during a real walk");
            yield return Walk(d.P(8.2f,6.8f,.06f),true);
            Check(d.LocalPosition(player.transform.position).y<5.1f,"Standing capsule is stopped by tree limb");
            yield return Crouch(true);yield return Walk(d.P(8.2f,6.9f,.06f));
            Check(d.TreeCleared,"Physical crouch passage earns the canonical tree route flag");yield return Crouch(false);
            yield return Walk(d.P(8.2f,10.975f,.06f));yield return Walk(d.vaultOutside.position);
            Aim(Vector3.Lerp(d.vaultOutside.position,d.vaultInside.position,.65f)+Vector3.up*1.3f);yield return Shot("18-kitchen-window");
            yield return Press(Key.Space);yield return Wait(()=>d.Stage==AtmajaStage.Kitchen&&!d.Busy,15,"Contextual vault enters kitchen");player.enabled=false;insideAtmaja=true;
            Check(d.kitchenShutter.activeSelf,"Kitchen window visibly shuts and return route is sealed");
            Phone.isPhoneRaised=true;Phone.isScreenOn=true;View.Apps.Open(PhoneApp.Camera);Phone.SyncVisualState();yield return null;
            Aim(d.P(2.85f,11.2f,.8f));yield return Shot("19-dark-kitchen-well");
            yield return Walk(d.P(5.15f,9.8f,.37f));
            yield return Use(Item(AtmajaAction.KitchenDoor),Item(AtmajaAction.KitchenDoor).GetComponent<Collider>().bounds.center);yield return new WaitForSeconds(1.2f);
            yield return Walk(d.P(4.1f,9.8f,.37f));yield return Walk(d.P(4.1f,8.4f,.54f));yield return Walk(d.P(5.35f,8,.54f));yield return Walk(d.P(5.35f,6.2f,.54f));
            Aim(d.P(3.7f,4.9f,1.3f));yield return Shot("20-wood-chairs-clear-aisle");
            yield return Crouch(true);yield return Walk(d.P(4.6f,6.2f,.54f));
            yield return Use(Item(AtmajaAction.DamarPhone),d.damarPhone.GetComponent<Collider>().bounds.center);yield return Shot("21-crouched-backpack-access");
            Check(Phone.arModuleUnlocked&&CampaignState.Instance.HasInventoryItem("MATABATIN_APK"),"Damar phone reached from the side, below table, using crouch and real first-hit interaction");
            yield return Walk(d.P(5.35f,6.2f,.54f));yield return Crouch(false);
            yield return Walk(d.P(5.35f,1.3f,.54f));yield return Walk(d.P(2.1f,1.3f,.54f));yield return Walk(d.P(2.1f,4.1f,.54f));
            Check(receipt.maximumInteriorFootY<.7f,"Dining route crossed both sides and end with feet on floor, never on tabletop");
            Phone.isPhoneRaised=true;Phone.isScreenOn=true;View.Apps.Open(PhoneApp.Camera);Phone.currentScanMode=ScanVisualMode.AR;Phone.SyncVisualState();Aim(d.P(3.7f,2.15f,1.4f));yield return Shot("22-ar-figure");
            Check(d.arFigure.activeSelf&&(eye.cullingMask&(1<<AtmajaPhoneView.ARLayer))==0&&View.FeedCamera.enabled,"AR apparition is phone-only, with the normal world camera excluding its layer");
            Phone.isScreenOn=false;Phone.SyncVisualState();yield return null;Check(Phone.isBroadcastConnected&&!View.FeedCamera.enabled,"Turning screen off preserves live but disables additional camera rendering");
            Phone.isScreenOn=true;Phone.SyncVisualState();
            yield return Walk(d.P(.8f,4.1f,.54f));yield return Walk(d.P(-1.4f,4.1f,.54f));yield return Walk(d.P(-1.4f,3.3f,.54f));
            var sitting=GameObject.Find("FrontSittingDoor").GetComponent<HingedDoor>();yield return Use(sitting,sitting.GetComponent<Collider>().bounds.center);yield return new WaitForSeconds(1.2f);
            yield return Walk(d.P(-1.4f,1.5f,.54f));yield return Walk(d.P(-4.4f,1.5f,.54f));Aim(d.clockHands.transform.position);yield return Shot("23-corner-table-exploration");
            yield return Use(Item(AtmajaAction.ClockHands),d.clockHands.GetComponent<Collider>().bounds.center);
            yield return Walk(d.P(-1.4f,1.5f,.54f));yield return Walk(d.P(-1.4f,4.1f,.54f));yield return Walk(d.P(-1.75f,5.5f,.54f));Aim(d.arClue.transform.position);
            yield return Wait(()=>CampaignState.Instance.HasInventoryItem("EVENT_P0_AR_CLUE_READ"),15,"Player aims AR at actual wall blood clue");yield return Shot("24-red-wall-ar-writing");
            yield return new WaitForEndOfFrame();var frame=ScreenCapture.CaptureScreenshotAsTexture();int redPixels=0;
            for(int y=(int)(frame.height*.23f);y<frame.height*.68f;y++)for(int x=(int)(frame.width*.72f);x<frame.width*.95f;x++)
            {var c=frame.GetPixel(x,y);if(c.r>.18f&&c.r>c.g*1.8f&&c.r>c.b*1.8f)redPixels++;}
            Destroy(frame);Check(redPixels>20,"Actual phone framebuffer contains blood-red clue strokes (pixels="+redPixels+")");
            Phone.currentScanMode=ScanVisualMode.Normal;Phone.SyncVisualState();
            yield return Walk(d.P(-4.7f,5.8f,.54f));Aim(d.clockDialCentre);yield return Shot("25-wall-clock-radial-numerals");
            var tick=d.clockHour.GetComponentInParent<ClockAmbience>();Check(tick&&tick.source&&tick.source.outputAudioMixerGroup,"Wall clock's positional tick source survives standalone scene loading");
            yield return Use(Item(AtmajaAction.Clock),d.clockDialCentre);Click("SetClockTime");
            Check(!CampaignState.Instance.HasInventoryItem("KUNCI_TANGGA_TENGAH"),"Clock controls reject an incorrect time");
            for(int n=0;n<3;n++)Click("HourPlus");for(int n=0;n<9;n++)Click("MinutePlus");yield return Shot("26-clock-time-controls");Click("SetClockTime");yield return null;
            Check(Hud.documentPanel.activeSelf,"03:45 opens clock drawer and original newspaper");yield return Shot("27-newspaper");Hud.documentCloseButton.onClick.Invoke();
            yield return Walk(d.P(-3.55f,6.3f,.54f));yield return Use(Item(AtmajaAction.StairDoor),Item(AtmajaAction.StairDoor).GetComponent<Collider>().bounds.center);
            yield return Shot("28-stair-checkpoint");receipt.checkpointSaved=CampaignState.Instance.Data.checkpoint_id=="PROLOG_ATMAJA_STAIRS_UNLOCKED"&&CampaignState.Instance.LoadCheckpoint();
            Check(receipt.checkpointSaved,"End-of-scope checkpoint reloads without repaying donations");
            yield return Press(Key.Escape);Check(UiFocus.Menu&&Time.timeScale==0,"ESC opens an actual pause menu and stops game time");yield return Shot("29-pause-menu");
            Click("MainMenu");yield return Wait(()=>!GameShell.MenuSceneLoading&&GameShell.AwaitingStart,120,"Pause menu returns to main menu");yield return null;Click("Continue");
            yield return Wait(()=>SceneManager.GetActiveScene().name==PrologSceneFlow.AtmajaScene,150,"Continue routes saved Atmaja checkpoint through startup");yield return new WaitForSeconds(1);
            Check(FindFirstObjectByType<AtmajaDirector>().Stage==AtmajaStage.StairCheckpoint&&CampaignState.Instance.Data.wallet_bound==5500000,"Continue restores reached stage and exact wallet");
            Finish(!failed);
        }
        private void Stow(){Phone.isPhoneRaised=false;Phone.SyncVisualState();UiFocus.Phone=false;}
        private AtmajaItem Item(AtmajaAction action)=>FindObjectsByType<AtmajaItem>(FindObjectsSortMode.None).First(x=>x.action==action);
        private Button Button(string name)=>FindObjectsByType<Button>(FindObjectsSortMode.None).First(b=>b.name==name&&b.gameObject.activeInHierarchy);
        private void Click(string name){var b=Button(name);Check(b.interactable,"UI button: "+name);b.onClick.Invoke();}
        private void Aim(Vector3 target){eye.transform.LookAt(target);Physics.SyncTransforms();}
        private IEnumerator Press(params Key[] keys)
        {InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));yield return null;yield return null;InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return null;}
        private IEnumerator Use(IInteractable item,Vector3 target)
        {
            UiFocus.Phone=false;Aim(target);yield return null;yield return null;
            var interactor=player.GetComponent<InteractionInteractor>();
            Check(ReferenceEquals(interactor.CurrentTarget,item),"First-hit reachable: "+item.InteractableId);
            if(!ReferenceEquals(interactor.CurrentTarget,item))
            {if(Physics.Raycast(eye.transform.position,eye.transform.forward,out var hit,3))Debug.LogWarning("REVIEW_EXPECTED_TARGET blocked by "+hit.collider.name+" distance="+hit.distance);yield break;}
            yield return Press(Key.E);
        }
        private IEnumerator KosUse(KosInteractableItem item,Vector3 feet)
        {
            // Golden compact-room layout has a separate established manual collision regression.
            Stow();player.enabled=false;capsule.enabled=false;player.transform.position=feet;capsule.enabled=true;
            eye.transform.localPosition=Vector3.zero;player.cameraPivot.localPosition=Vector3.up*player.cameraStandingY;
            yield return Use(item,item.GetComponent<Collider>().bounds.center);
        }
        private IEnumerator Crouch(bool on)
        {
            InputSystem.QueueStateEvent(keyboard,on?new KeyboardState(Key.LeftCtrl):new KeyboardState());player.enabled=true;
            // Preserve the view between enabling the normal posture controller and directed walking.
            yield return new WaitForSeconds(.6f);player.enabled=false;
            Check(player.IsCrouching==on&&(!on||capsule.height<1.08f),on?"Normal CTRL input lowers physical capsule":"Normal crouch release returns to standing");
        }
        private IEnumerator Walk(Vector3 target,bool expectBlocked=false)
        {
            player.enabled=false;float until=Time.realtimeSinceStartup+35+Vector3.Distance(target,player.transform.position)*12;int stalled=0;
            while(Time.realtimeSinceStartup<until)
            {
                var delta=target-player.transform.position;delta.y=0;if(delta.magnitude<.10f)break;
                Aim(target+Vector3.up*(player.IsCrouching?.85f:1.6f));
                for(int sub=0;sub<4;sub++)
                {
                    delta=target-player.transform.position;delta.y=0;if(delta.magnitude<.10f)break;var before=player.transform.position;
                    capsule.Move(Vector3.ClampMagnitude(delta,.16f)+Vector3.down*.18f);
                    float distance=Vector3.Distance(new Vector3(before.x,0,before.z),new Vector3(player.transform.position.x,0,player.transform.position.z));
                    receipt.walkedMeters+=distance;if(insideAtmaja)receipt.maximumInteriorFootY=Mathf.Max(receipt.maximumInteriorFootY,player.transform.position.y);
                    if(distance<.003f)stalled++;else stalled=0;
                }
                if(stalled>18)break;yield return null;
            }
            var remain=target-player.transform.position;remain.y=0;
            bool reached=remain.magnitude<.16f;
            if(expectBlocked){Check(!reached,"Physical obstacle blocks standing path");yield break;}
            Check(reached,"Continuous capsule waypoint "+target+" reached at "+player.transform.position);
        }
        private IEnumerator Wait(Func<bool> predicate,float seconds,string label)
        {float until=Time.realtimeSinceStartup+seconds;while(!predicate()&&Time.realtimeSinceStartup<until)yield return null;Check(predicate(),label);}
        private IEnumerator Shot(string name)
        {float scale=Time.timeScale;Time.timeScale=0;yield return new WaitForSecondsRealtime(.35f);yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;yield return null;shots.Add(name+".png");Time.timeScale=scale;}
        private void Check(bool pass,string label){checks.Add((pass?"PASS ":"FAIL ")+label);Debug.Log("REVIEW_PLAYER "+checks[checks.Count-1]);if(!pass)failed=true;}
        private void Finish(bool success)
        {
            if(done)return;done=true;StopAllCoroutines();receipt=receipt??new Receipt();receipt.completed=success;receipt.checks=checks.ToArray();receipt.screenshots=shots.ToArray();
            File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(receipt,true)+"\n");Debug.Log("REVIEW_PLAYER completed="+success+" shots="+shots.Count);Application.Quit(success?0:1);
        }
    }
}
#endif
