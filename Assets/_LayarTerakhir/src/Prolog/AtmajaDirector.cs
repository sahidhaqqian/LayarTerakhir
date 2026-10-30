using System;
using System.Collections;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public enum AtmajaStage { Courtyard,SurveyRead,Live,SidePassage,Kitchen,DamarPhone,ARRevealed,ClockUnlocked,StairCheckpoint,StairLanding,EvidenceHunt,MasterOpen,BasementEntry,BasementStealth,HandycamPlayed,Chase,EndingVista }
    public sealed class AtmajaDirector:MonoBehaviour
    {
        public const string SurveyId="DOC_PROLOG_02_SURVEI_DAMAR";
        public const string NewspaperText="TRAGEDI KELUARGA ATMAJA: Satu Keluarga Ditemukan Tewas Mengenaskan. Polisi menduga sang ibu, Nyonya Sri Atmaja (38), mengalami depresi berat dan gangguan jiwa akibat mengikuti aliran klenik sebelum meracuni anak-anaknya dan mengakhiri hidupnya sendiri dengan seutas tali di ruang tengah rumah pada tengah malam.";
        public const string SurveyText="Catatan Damar, terlipat di dekat tasnya.\n\n23:17 — Gue sudah di dalam. Kayu jendela tua di samping ternyata masih bisa digeser.\n\n23:24 — Dari arah dapur kedengaran kursi diseret. Gue panggil, nggak ada yang jawab.\n\n23:31 — Aplikasi MataBatin.apk bereaksi ke sesuatu yang nggak kelihatan. Ka, kalau lo baca ini: jangan percaya semua yang kelihatan kosong.";
        public const string LiveIntro="Oke... tes, satu dua. Malam semuanya, balik lagi di Malam Keramat. Malam ini gue nggak sendirian... maksudnya, gue lagi nyusulin Damar yang hilang kontak di Rumah Tua Atmaja. Buat yang baru gabung, bantu pantau layar ya.";
        public FirstPersonController player;
        public Transform gateHinge,kitchenDoor,spawnCourtyard,spawnKitchen,spawnDining,spawnStairs,vaultOutside,vaultInside;
        public GameObject kitchenShutter,damarPhone,clockHands,arFigure,arClue,stairDoorLeaf;
        public HingedDoor stairDoorMotion;
        private HingedDoor kitchenDoorMotion;
        public AtmajaUpperDirector upper;public AtmajaBasementHorror basement;public AtmajaChaseSequence chase;
        public Transform clockHour,clockMinute,clockDrawer;
        public Vector3 clockDialCentre;
        public Vector3 clockFaceNormal=Vector3.forward;
        public AtmajaPhoneView phoneView;
        public SceneAudioProfile audioProfile;
        public AudioSource rainSource,foleySource,notificationSource,stepsSource;
        public Vector3 origin,east=Vector3.right,north=Vector3.forward;
        public AtmajaStage Stage {get;private set;}
        public bool Busy {get;private set;}
        public int CheckpointRestores {get;private set;}
        public bool TreeCleared=>Has("EVENT_P0_TREE_CROUCHED");
        public bool HasSurvey=>Has(SurveyId);
        public bool CanBeginLive=>Has("EVENT_P0_FRONT_DOOR_TRIED")||HasSurvey;
        public bool LiveConnected=>Has("EVENT_P0_LIVE_STARTED");
        public int Viewers
        {
            get
            {
                if(Has("PROLOG_COMPLETE")||Stage==AtmajaStage.EndingVista)return 54820;
                if(Has("EVENT_P0_CHASE_STARTED")||Stage==AtmajaStage.Chase)return 31200;
                if(Has("EVENT_P0_BASEMENT_ENTERED"))return 12400;
                if(upper&&upper.ViewersBump()>0)return upper.ViewersBump();
                return Has("EVENT_P0_AR_REVEALED")?1420:Has("EVENT_P0_KITCHEN_ENTERED")?250:14;
            }
        }
        private bool underTree,wellHeard;
        private bool clockTimeShown;
        private AudioClip arStatic;
        private AtmajaReviewMoments moments;
        private CampaignState Campaign=>CampaignState.Instance;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        private HudController Hud=>HudController.Instance;
        private bool Has(string id)=>Campaign&&Campaign.HasInventoryItem(id);
        private IEnumerator Start()
        {
            moments=GetComponent<AtmajaReviewMoments>();if(!moments)moments=gameObject.AddComponent<AtmajaReviewMoments>();
            PrologSceneFlow.RestoreBindings();
            Campaign.LoadCheckpoint();
            // Direct scene playtesting still uses the real preparation inventory, never a second active phone.
            if(!Has("SMARTPHONE_RAKA"))
            {
                Campaign.StartNewGame();
                foreach(var id in new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA","POWERBANK_KECIL_KOS"})Campaign.AddInventoryItem(id);
                Campaign.Data.wallet_bound=5000000;
            }
            Phone.hasSmartphoneInInventory=true;Phone.batteryPercent=Campaign.Data.battery;
            Phone.isPhoneRaised=false;Phone.isScreenOn=Phone.batteryPercent>0;Phone.isBroadcastConnected=LiveConnected;
            Phone.arModuleUnlocked=Has("MATABATIN_APK");Phone.SyncVisualState();
            RestoreWorld();
            LayarTerakhir.Audio.PlayerFootsteps.Install(player,audioProfile,stepsSource);
            PracticalLightPresentation.AddThreatRim(arFigure);
            if(basement)PracticalLightPresentation.AddThreatRim(basement.nyonya);
            if(upper){PracticalLightPresentation.AddThreatRim(upper.mirrorFlashFigure);PracticalLightPresentation.AddThreatRim(upper.hangedFlashFigure);}
            if(chase){PracticalLightPresentation.AddThreatRim(chase.chaserFigure);PracticalLightPresentation.AddThreatRim(chase.streetFigure);}
            if(!Has("EVENT_P0_ATMAJA_ARRIVED"))Commit("EVENT_P0_ATMAJA_ARRIVED","PROLOG_ATMAJA_COURTYARD");
            Hud.SetFadeAlpha(1);yield return Hud.FadeBlack(1,0,1.4f);
            Objective();
        }
        public Vector3 P(float x,float depth,float height)=>origin+east*x+north*depth+Vector3.up*height;
        public Vector2 LocalPosition(Vector3 p)=>new Vector2(Vector3.Dot(p-origin,east),Vector3.Dot(p-origin,north));
        public void RestoreWorld()
        {
            Busy=false;UiFocus.Cinematic=false;
            if(moments)moments.HideARApparition();
            var rig=player.GetComponent<ShoulderCameraRig>();if(rig)rig.FirstPersonOverride=false;
            var doubleGate=gateHinge.GetComponent<DoubleLeafGate>();
            if(doubleGate)doubleGate.SetImmediate(Has("EVENT_P0_GATE_OPEN"));
            else gateHinge.localRotation=Quaternion.Euler(0,Has("EVENT_P0_GATE_OPEN")?-95:0,0);
            bool inside=Has("EVENT_P0_KITCHEN_ENTERED");kitchenShutter.SetActive(inside);
            EnsureKitchenDoor();
            if(kitchenDoorMotion!=null)kitchenDoorMotion.SetOpenImmediate(Has("EVENT_P0_DINING_OPEN"));
            else if(kitchenDoor)kitchenDoor.localRotation=Quaternion.Euler(0,Has("EVENT_P0_DINING_OPEN")?-95:0,0);
            if(damarPhone)damarPhone.SetActive(!Has("MATABATIN_APK"));
            if(stairDoorMotion)stairDoorMotion.SetOpenImmediate(Has("EVENT_P0_STAIRS_UNLOCKED"));
            if(clockHands)clockHands.SetActive(!Has("ITEM_P0_CLOCK_HANDS"));
            if(clockHour)clockHour.gameObject.SetActive(Has("EVENT_P0_CLOCK_UNLOCKED"));
            if(clockMinute)clockMinute.gameObject.SetActive(Has("EVENT_P0_CLOCK_UNLOCKED"));
            if(Has("EVENT_P0_CLOCK_UNLOCKED"))ShowClockTime();
            var spawn=Has("PROLOG_COMPLETE")&&chase&&chase.spawnStreet?chase.spawnStreet:Has("EVENT_P0_CHASE_STARTED")&&chase&&chase.spawnChase?chase.spawnChase:Has("EVENT_P0_BASEMENT_ENTERED")&&basement&&basement.spawnBasement?basement.spawnBasement:Has("EVENT_P0_UPPER_REACHED")&&upper&&upper.spawnUpper?upper.spawnUpper:Has("EVENT_P0_STAIRS_UNLOCKED")?spawnStairs:Has("MATABATIN_APK")?spawnDining:inside?spawnKitchen:spawnCourtyard;
            PlacePlayer(spawn.position,spawn.rotation);
            Stage=Has("PROLOG_COMPLETE")?AtmajaStage.EndingVista:Has("EVENT_P0_CHASE_STARTED")?AtmajaStage.Chase:Has("EVENT_P0_BASEMENT_ENTERED")?AtmajaStage.BasementStealth:Has("EVENT_P0_UPPER_REACHED")?(upper&&upper.ChainsBroken>=3?AtmajaStage.MasterOpen:AtmajaStage.EvidenceHunt):Has("EVENT_P0_STAIRS_UNLOCKED")?AtmajaStage.StairCheckpoint:Has("EVENT_P0_CLOCK_UNLOCKED")?AtmajaStage.ClockUnlocked:Has("EVENT_P0_AR_REVEALED")?AtmajaStage.ARRevealed:Has("MATABATIN_APK")?AtmajaStage.DamarPhone:inside?AtmajaStage.Kitchen:TreeCleared?AtmajaStage.SidePassage:LiveConnected?AtmajaStage.Live:HasSurvey?AtmajaStage.SurveyRead:AtmajaStage.Courtyard;
            if(upper)upper.RestoreWorld();if(basement)basement.RestoreWorld();if(chase)chase.RestoreWorld();
        }
        public void PlacePlayer(Vector3 position,Quaternion rotation)
        {var cc=player.GetComponent<CharacterController>();cc.enabled=false;player.transform.SetPositionAndRotation(position,rotation);cc.enabled=true;Physics.SyncTransforms();}
        private bool Commit(string id,string checkpoint,Action change=null)
        {
            if(Has(id))return true;
            string before=JsonUtility.ToJson(Campaign.Data);
            Campaign.AddInventoryItem(id);change?.Invoke();
            if(Campaign.SaveCheckpointAtomic(checkpoint))return true;
            JsonUtility.FromJsonOverwrite(before,Campaign.Data);Hud.ShowBroadcastNotification("Checkpoint gagal disimpan. Coba interaksi kembali.",4);return false;
        }
        private void Update()
        {
            if(!player||!Campaign||!Hud||Busy||UiFocus.HardModal)return;
            bool modal=Hud.IsModalOpen;float duck=modal ? .45f : 1;
            if(rainSource)rainSource.GetComponent<AmbientAudioLoop>().LevelMultiplier=duck;
            var local=LocalPosition(player.transform.position);
            if(LiveConnected&&!Has("EVENT_P0_KITCHEN_ENTERED"))
            {
                if(local.x>6.4f&&local.y>-.8f&&!Has("EVENT_P0_SIDE_CHAT"))SideChat();
                if(local.x>7.0f&&local.y>4.9f&&local.y<6.2f&&player.IsCrouching&&player.GetComponent<CharacterController>().height<1.10f)underTree=true;
                if(underTree&&local.y>6.4f&&!TreeCleared)
                    if(Commit("EVENT_P0_TREE_CROUCHED","PROLOG_ATMAJA_COURTYARD")){Stage=AtmajaStage.SidePassage;Objective();}
            }
            if(Phone&&Phone.arModuleUnlocked)
            {
                bool ar=Phone.isPhoneRaised&&Phone.isScreenOn&&Phone.currentScanMode==ScanVisualMode.AR&&phoneView.IsCameraApp;
                if(ar&&!Has("EVENT_P0_AR_REVEALED"))RevealAR();
                if(arFigure)arFigure.SetActive(ar&&moments&&moments.ARApparitionActive);
                if(arClue)arClue.SetActive(ar);
                if(ar&&!Has("EVENT_P0_AR_CLUE_READ"))ObserveClue();
            }
            if(Stage<AtmajaStage.Chase&&Has("EVENT_P0_KITCHEN_ENTERED")&&!wellHeard&&Vector3.Distance(player.transform.position,P(2.7f,11.2f,.35f))<2)
            {wellHeard=true;Hud.EnqueueSubtitle("SFX","[KLOTAK... KLOTAK... Suara ember timba ditarik dari dasar sumur]",3);Hud.EnqueueSubtitle("Bisikan","Haus... bukakan...",3);Sfx(audioProfile.knock,P(2.7f,11.2f,.4f),.4f);StartCoroutine(moments.HearWell());}
            if(!modal&&Phone&&Phone.isPhoneRaised&&Phone.isScreenOn&&phoneView&&phoneView.IsCameraApp&&GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz))TryPhotographWell();
        }
        public void Interact(AtmajaItem item)
        {
            switch(item.action)
            {
                case AtmajaAction.Gate:
                    if(!Has("EVENT_P0_GATE_OPEN")){Campaign.AddInventoryItem("EVENT_P0_GATE_OPEN");StartCoroutine(Swing(gateHinge,-95));Sfx(audioProfile.latch,item.transform.position,.65f);}break;
                case AtmajaAction.Survey:
                    Hud.EnqueueSubtitle("Raka","Tulisan Damar... Dia benar-benar udah masuk ke sini.",4);
                    Sfx(audioProfile.cloth,item.transform.position,.4f);
                    Hud.OpenDocumentModal("CATATAN SURVEI DAMAR • DOKUMEN #2",SurveyText,()=>
                    {
                        string checkpoint=Has("EVENT_P0_KITCHEN_ENTERED")?Campaign.Data.checkpoint_id:"PROLOG_ATMAJA_COURTYARD";
                        if(Commit(SurveyId,checkpoint)){if(!Has("EVENT_P0_KITCHEN_ENTERED"))Stage=LiveConnected?AtmajaStage.Live:AtmajaStage.SurveyRead;Objective();}
                    },SurveyId);break;
                case AtmajaAction.FrontDoor:StartCoroutine(TryFrontDoor(item.transform));break;
                case AtmajaAction.KitchenDoor:
                    EnsureKitchenDoor();
                    if(kitchenDoorMotion==null)break;
                    if(!Has("EVENT_P0_DINING_OPEN"))
                    {if(Commit("EVENT_P0_DINING_OPEN","PROLOG_ATMAJA_KITCHEN")){kitchenDoorMotion.Interact();Objective();}}
                    else kitchenDoorMotion.Interact();
                    break;
                case AtmajaAction.DamarPhone:TakeDamarPhone();break;
                case AtmajaAction.Well:StartCoroutine(moments.OpenWell());break;
                case AtmajaAction.ClockHands:
                    if(Commit("ITEM_P0_CLOCK_HANDS","PROLOG_MATABATIN_ACQUIRED")){clockHands.SetActive(false);Hud.ShowBroadcastNotification("Jarum Jam Kuningan diambil.",3);Objective();}break;
                case AtmajaAction.Clock:ClockPuzzle();break;
                case AtmajaAction.StairDoor:
                    if(!Has("EVENT_P0_CLOCK_UNLOCKED")){Hud.ShowBroadcastNotification("Pintunya terkunci. Ada lubang kunci tua di bawah gagangnya.",5);break;}
                    if(Commit("EVENT_P0_STAIRS_UNLOCKED","PROLOG_ATMAJA_STAIRS_UNLOCKED")){Stage=AtmajaStage.StairCheckpoint;if(stairDoorMotion&&!stairDoorMotion.IsOpen)stairDoorMotion.Interact();Objective();Hud.ShowBroadcastNotification("Kunci berputar. Jalan ke lantai atas sudah terbuka.",5);}break;
                case AtmajaAction.CupPhoto:case AtmajaAction.Diary:case AtmajaAction.BathCurtain:case AtmajaAction.BathScratches:case AtmajaAction.WillLetter:case AtmajaAction.RopePhoto:if(upper)upper.HandleEvidence(item);break;
                case AtmajaAction.CarpetLift:case AtmajaAction.Trapdoor:if(upper)upper.HandleMaster(item);break;
                case AtmajaAction.Powerbank:case AtmajaAction.Handycam:case AtmajaAction.WardrobeHide:if(basement)basement.Handle(item);break;
                case AtmajaAction.Charge:if(Phone.batteryPercent<35)StartCoroutine(Charge());break;
                case AtmajaAction.Window:Hud.ShowBroadcastNotification($"[{GameInputConfig.Instance.vaultKey}] Panjat jendela dapur\n"+(TreeCleared?"Dekati jendela dan hadap ke dalam.":"Mulai live dan cari jalan melewati kamboja."),4);break;
            }
        }
        public bool StartLive()
        {
            if(!CanBeginLive||!Phone.isPhoneRaised||!Phone.isScreenOn||Busy)return false;
            if(!Commit("EVENT_P0_LIVE_STARTED","PROLOG_ATMAJA_COURTYARD"))return false;
            Phone.isBroadcastConnected=true;Phone.SyncVisualState();Stage=AtmajaStage.Live;
            Hud.EnqueueSubtitle("Raka",LiveIntro,10);Objective();return true;
        }
        private void SideChat()
        {
            if(!Commit("EVENT_P0_SIDE_CHAT","PROLOG_ATMAJA_COURTYARD",()=>Campaign.AddBoundDonation(500000)))return;
            phoneView.AddChat("@kopi_susu99","Woi akhirnya live juga bang Raka! Kirain udah pensiun gara-gara sepi penonton wkwk");
            phoneView.AddChat("@dinda_ayu","Kok mukanya tegang banget bang? Itu beneran bang Damar ilang?");
            phoneView.AddChat("@setan_lokal","Eh bentar, itu motornya bang Damar kan di depan? Kok orangnya gak ada?");
            Hud.ShowBroadcastNotification("Penonton_Terakhir • SAWER Rp500.000\nJangan buang waktu. Cari tahu apa yang terjadi pada sahabatmu di dalam rumah itu.",10);
            notificationSource.PlayOneShot(audioProfile.notification,.55f);
        }
        public bool CanVault()=>!Busy&&TreeCleared&&LiveConnected&&!Has("EVENT_P0_KITCHEN_ENTERED");
        public IEnumerator Vault()
        {
            if(!CanVault())yield break;Busy=true;
            player.enabled=false;var cc=player.GetComponent<CharacterController>();cc.enabled=false;
            var start=player.transform.position;var end=vaultInside.position;float t=0;
            while(t<1){t+=Time.deltaTime/1.05f;player.transform.position=Vector3.Lerp(start,end,Mathf.SmoothStep(0,1,t))+Vector3.up*Mathf.Sin(Mathf.Clamp01(t)*Mathf.PI)*.30f;yield return null;}
            player.transform.SetPositionAndRotation(end,vaultInside.rotation);cc.enabled=true;player.enabled=true;Busy=false;
            if(!Commit("EVENT_P0_KITCHEN_ENTERED","PROLOG_ATMAJA_KITCHEN")){PlacePlayer(vaultOutside.position,vaultOutside.rotation);yield break;}
            kitchenShutter.SetActive(true);Stage=AtmajaStage.Kitchen;Sfx(audioProfile.latch,vaultOutside.position,.8f);
            phoneView.AddChat("@intan_permata","Bang tadi pas abang lompat jendela, di kaca lantai 2 kayak ada tirai gerak sendiri sumpah demi Allah!");
            phoneView.AddChat("@bocil_kematian","Gimmick paling itu bang Damar ngumpet di atas.");
            Hud.EnqueueSubtitle("Raka","Anjir! Anginnya kenceng banget... Eh bentar, ini jendelanya kok nyangkut?!",5);Objective();
            Hud.EnqueueSubtitle("Raka",KosDialogue.Trapped,4);
            yield return moments.CallDamar();
        }
        private IEnumerator Swing(Transform hinge,float angle)
        {var gate=hinge.GetComponent<DoubleLeafGate>();if(gate){yield return gate.Open();yield break;}var from=hinge.localRotation;var to=Quaternion.Euler(0,angle,0);float t=0;while(t<1){t+=Time.deltaTime;hinge.localRotation=Quaternion.Slerp(from,to,Mathf.SmoothStep(0,1,t));yield return null;}}
        // The dining door is a real two-way HingedDoor: opening it never traps
        // Raka because the same handle closes it again. Wired at runtime so
        // both veteran and freshly built scenes behave identically.
        private void EnsureKitchenDoor()
        {
            if(kitchenDoorMotion!=null||!kitchenDoor)return;
            kitchenDoorMotion=kitchenDoor.GetComponent<HingedDoor>();
            if(kitchenDoorMotion==null)kitchenDoorMotion=kitchenDoor.gameObject.AddComponent<HingedDoor>();
            kitchenDoorMotion.hinge=kitchenDoor;kitchenDoorMotion.angle=-95;
            kitchenDoorMotion.label="Buka pintu ruang makan";
        }
        private IEnumerator TryFrontDoor(Transform door)
        {
            if(Busy)yield break;Busy=true;player.MovementLocked=true;
            var original=door.localRotation;
            for(float t=0;t<1.6f;t+=Time.deltaTime)
            {
                door.localRotation=original*Quaternion.Euler(0,Mathf.Sin(t*25)*1.4f*Mathf.Sin(t/1.6f*Mathf.PI),0);
                if(t<Time.deltaTime)Sfx(audioProfile.latch,door.position,.85f);
                yield return null;
            }
            door.localRotation=original;
            Campaign.AddInventoryItem("EVENT_P0_FRONT_DOOR_TRIED");
            Hud.EnqueueSubtitle("Raka","Nggak bisa... ada rantai dari dalam. Dam? Damar! Gue harus cari jalan masuk lain.",5);
            player.MovementLocked=false;Busy=false;Objective();
        }
        private IEnumerator Charge()
        {
            Busy=true;player.MovementLocked=true;float start=Phone.batteryPercent;
            Hud.ShowBroadcastNotification("Pengisian daya darurat • 35%",4);
            for(float t=0;t<3;t+=Time.deltaTime){Phone.batteryPercent=Mathf.Lerp(start,35,t/3);Phone.SyncVisualState();yield return null;}
            Phone.batteryPercent=35;Phone.isScreenOn=true;Phone.SyncVisualState();Campaign.Data.battery=35;
            Campaign.SaveCheckpointAtomic(PrologSceneFlow.IsAtmajaCheckpoint(Campaign.Data.checkpoint_id)?Campaign.Data.checkpoint_id:"PROLOG_ATMAJA_COURTYARD");
            Busy=false;player.MovementLocked=false;
        }
        private void TakeDamarPhone()
        {
            if(Busy||Has("MATABATIN_APK"))return;
            // No crouch gate: Raka crouches visually during the scripted grab
            // whether the player approached standing or crouched.
            Hud.PlayPriorityDialogue("Raka","Ini HP-nya Damar... Layarnya pecah, tapi kartu memorinya masih kebaca. Gue pindahin dulu aplikasinya ke HP gue.",7);
            StartCoroutine(ChipTransferSequence());
        }
        private IEnumerator ChipTransferSequence()
        {
            if(!damarPhone)yield break;
            var position=damarPhone.transform.position;var rotation=damarPhone.transform.rotation;
            yield return moments.Install(damarPhone);
            if(!moments.InstallationCompleted)yield break;
            if(!Commit("MATABATIN_APK","PROLOG_MATABATIN_ACQUIRED",()=>Campaign.Data.upgrades.Add("MATABATIN_AR")))
            {damarPhone.transform.SetPositionAndRotation(position,rotation);damarPhone.SetActive(true);yield break;}
            Phone.arModuleUnlocked=true;Stage=AtmajaStage.DamarPhone;
            if(!Has("EVENT_P0_AR_TUTORIAL"))
            {
                Campaign.AddInventoryItem("EVENT_P0_AR_TUTORIAL");Campaign.SaveCheckpointAtomic(Campaign.Data.checkpoint_id);
                Hud.ShowBroadcastNotification("MataBatin terhubung • ["+GameInputConfig.Instance.GetActionLabel(GameAction.ModeAR)+"] Lensa AR • Daya terkuras lebih cepat",6);
            }
            // Scripted beat: Raka steps back with the phone, raises it and points
            // it toward the far end of the table, cueing the AR apparition.
            Phone.isPhoneRaised=true;
            if(Phone.batteryPercent>0)Phone.isScreenOn=true;
            Phone.currentScanMode=ScanVisualMode.AR;Phone.SyncVisualState();
            phoneView.Apps.Open(PhoneApp.Camera);
            Hud.EnqueueSubtitle("Raka","Aplikasinya nyala... gue arahin ke ujung meja.",4);
        }
        private void RevealAR()
        {
            if(!Commit("EVENT_P0_AR_REVEALED",Stage>AtmajaStage.ARRevealed?Campaign.Data.checkpoint_id:"PROLOG_MATABATIN_ACQUIRED"))return;
            if(Stage<AtmajaStage.ARRevealed)Stage=AtmajaStage.ARRevealed;
            // Original short, enveloped static burst, routed through the existing UI mixer.
            if(!arStatic)
            {
                const int rate=32000;var samples=new float[rate/2];var noise=new System.Random(1998);
                for(int i=0;i<samples.Length;i++){float t=i/(float)samples.Length; samples[i]=(float)(noise.NextDouble()*2-1)*.22f*Mathf.Sin(t*Mathf.PI)*(1+.25f*Mathf.Sin(i*.047f));}
                arStatic=AudioClip.Create("MataBatin_Static",samples.Length,1,rate,false);arStatic.SetData(samples,0);
            }
            notificationSource.PlayOneShot(arStatic,.65f);
            StartCoroutine(moments.FirstARScare());
            phoneView.AddChat("@agung_gaming","ANJIR APAAN TUH DI UJUNG MEJA BARUSAN?!");
            phoneView.AddChat("@siti_nurhaliza","ITU BUKAN SETTINGAN WOI KEPALANYA MIRING!");
            phoneView.AddChat("@horor_hunter","Bang Raka lari bang! Itu filter kameranya nangkep penunggu situ!");Objective();
        }
        public bool TryPhotographWell()
        {
            if(Stage>=AtmajaStage.Chase||Has("EVENT_P0_WELL_BONUS")||!Has("EVENT_P0_KITCHEN_ENTERED")||Phone.batteryPercent<1)return false;
            var camera=phoneView&&phoneView.FeedCamera?phoneView.FeedCamera:player.GetComponent<InteractionInteractor>().playerCamera;var target=P(2.7f,11.2f,.75f);var delta=target-camera.transform.position;
            if(delta.magnitude>3||Vector3.Dot(camera.transform.forward,delta.normalized)<.92f)return false;
            var hits=Physics.RaycastAll(camera.transform.position,delta.normalized,delta.magnitude,~0,QueryTriggerInteraction.Ignore);
            Array.Sort(hits,(a,b)=>a.distance.CompareTo(b.distance));
            var first=Array.Find(hits,h=>!h.transform.IsChildOf(player.transform));
            if(first.collider&&!first.collider.name.Contains("Well"))return false;
            if(!Commit("EVENT_P0_WELL_BONUS","PROLOG_ATMAJA_KITCHEN",()=>{Campaign.Data.wallet_legitimate+=150000;Campaign.Data.battery=Mathf.Max(0,Phone.batteryPercent-1);}))return false;
            Phone.ConsumeBattery(1);Hud.ShowBroadcastNotification("SAWER BONUS Rp150.000 • Kamera menyorot celah papan sumur.",5);return true;
        }
        private void ClockPuzzle()
        {
            if(!Has("MATABATIN_APK")){Hud.ShowBroadcastNotification("Ada bagian yang hilang dari jam ini.",4);return;}
            if(!Has("ITEM_P0_CLOCK_HANDS")){Hud.ShowBroadcastNotification("Dua jarumnya hilang dari porosnya.",5);return;}
            if(Has("EVENT_P0_CLOCK_UNLOCKED")){if(!Has("DOC_PROLOG_03_KORAN_1998"))OpenNewspaper();else Hud.ShowBroadcastNotification("Jam menunjukkan 03:45. Kunci Pintu Tangga Tengah sudah diambil.",4);return;}
            if(!Has("EVENT_P0_AR_CLUE_READ")){Hud.ShowBroadcastNotification("Jam ini berhenti. Angka berapa yang seharusnya ditunjukkan?",4);return;}
            var puzzle=GetComponent<ClockPuzzleView>();if(!puzzle)puzzle=gameObject.AddComponent<ClockPuzzleView>();
            puzzle.Open(()=>
            {
                if(!Commit("EVENT_P0_CLOCK_UNLOCKED","PROLOG_MATABATIN_ACQUIRED",()=>Campaign.AddInventoryItem("KUNCI_TANGGA_TENGAH")))return;
                Stage=AtmajaStage.ClockUnlocked;
                ShowClockTime();Sfx(audioProfile.unlock,clockDialCentre,.6f);
                OpenNewspaper();
            });
        }
        private void ObserveClue()
        {
            var camera=phoneView&&phoneView.FeedCamera?phoneView.FeedCamera:player.GetComponent<InteractionInteractor>().playerCamera;var delta=arClue.transform.position-camera.transform.position;
            if(delta.magnitude>3.4f||Vector3.Dot(camera.transform.forward,delta.normalized)<.86f)return;
            var blockers=Physics.RaycastAll(camera.transform.position,delta.normalized,Mathf.Max(0,delta.magnitude-.03f),~0,QueryTriggerInteraction.Ignore);
            if(Array.Exists(blockers,h=>!h.transform.IsChildOf(player.transform)))return;
            if(Commit("EVENT_P0_AR_CLUE_READ","PROLOG_MATABATIN_ACQUIRED"))Hud.ShowBroadcastNotification("Petunjuk dinding dicatat di Jurnal HP.",3);
        }
        private void OpenNewspaper()=>Hud.OpenDocumentModal("KLIPING KORAN 1998 • DOKUMEN #3",NewspaperText,()=>{Commit("DOC_PROLOG_03_KORAN_1998","PROLOG_MATABATIN_ACQUIRED");Objective();},"DOC_PROLOG_03_KORAN_1998");
        private void ShowClockTime()
        {
            if(clockTimeShown)return;clockTimeShown=true;
            if(clockHour){clockHour.gameObject.SetActive(true);clockHour.RotateAround(clockDialCentre,clockFaceNormal,112.5f);}
            if(clockMinute){clockMinute.gameObject.SetActive(true);clockMinute.RotateAround(clockDialCentre,clockFaceNormal,270);}
            if(clockDrawer)clockDrawer.position+=clockFaceNormal*.18f;
        }
        public void Objective()
        {
            string text=Stage>=AtmajaStage.StairLanding?UpperObjective():Stage==AtmajaStage.StairCheckpoint?"Cari keberadaan Damar di bagian rumah berikutnya.":Has("EVENT_P0_CLOCK_UNLOCKED")?"Lanjutkan pencarian Damar.":Has("EVENT_P0_AR_CLUE_READ")?"Pahami hubungan petunjuk dengan benda-benda di rumah.":Has("EVENT_P0_AR_REVEALED")?"Cari tahu apa yang disembunyikan rumah ini.":Has("MATABATIN_APK")?"Coba aplikasi yang ditinggalkan Damar.":Has("EVENT_P0_KITCHEN_ENTERED")?"Cari keberadaan atau petunjuk tentang Damar.":TreeCleared?"Cari celah untuk masuk. [SPASI] untuk memanjat.":LiveConnected?"Temukan jalan masuk lain.":CanBeginLive?"Nyalakan siaran sebelum melanjutkan pencarian.":"23:52 WIB • Cari jejak Damar di sekitar rumah.";
            Hud.SetObjective(text);
        }
        private string UpperObjective()
        {
            if(!upper||!basement||!chase)return "Lantai 2.";
            return Stage switch
            {
                AtmajaStage.StairLanding=>upper.Objective(),
                AtmajaStage.EvidenceHunt=>upper.Objective(),
                AtmajaStage.MasterOpen=>upper.Objective(),
                AtmajaStage.BasementEntry=>basement.Objective(),
                AtmajaStage.BasementStealth=>basement.Objective(),
                AtmajaStage.HandycamPlayed=>chase.Objective(),
                AtmajaStage.Chase=>chase.Objective(),
                AtmajaStage.EndingVista=>"Prolog Layar Terakhir selesai. Istirahatlah di kosmu...",
                _=>"Lantai 2.",
            };
        }
        public void SetBusy(bool value)=>Busy=value;
        public void SetStage(AtmajaStage stage)=>Stage=stage;
        public void FailToCheckpoint(string checkpoint,string reason)
        {
            CheckpointRestores++;
            Debug.Log("ATMAJA_DEATH_MENU "+reason);
            Time.timeScale=1;
            try{if(upper)upper.StopAllCoroutines();}catch{}
            try{if(basement)basement.StopAllCoroutines();}catch{}
            try{if(chase)chase.StopAllCoroutines();}catch{}
            try{StopAllCoroutines();}catch{}
            // Keep the old world inert while Continue asynchronously loads another scene.
            // Otherwise the expired chase QTE can kill Raka again during that transition.
            Busy=true;UiFocus.Cinematic=false;
            player.enabled=true;player.MovementLocked=true;
            player.GetComponent<CharacterController>().enabled=true;
            Hud.PlayPriorityDialogue("","",.01f);Hud.SetFadeAlpha(0);
            var phone=SmartphoneController.Instance;
            if(phone){phone.batteryPercent=Campaign.Data.battery;phone.isPhoneRaised=false;phone.isScreenOn=phone.batteryPercent>0;phone.currentScanMode=Smartphone.ScanVisualMode.Normal;phone.SyncVisualState();}
            // Continue is an explicit menu choice. Death never resumes play by itself.
            if(GameShell.Instance)GameShell.Instance.ReturnAfterDeath(reason);
        }
        private void Sfx(AudioClip clip,Vector3 where,float gain)
        {if(!foleySource||!clip)return;foleySource.transform.position=where;foleySource.PlayOneShot(clip,gain);}
        private void OnDestroy(){if(arStatic)Destroy(arStatic);}
    }
}
