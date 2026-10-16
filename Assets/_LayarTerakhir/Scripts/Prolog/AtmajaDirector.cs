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
    public enum AtmajaStage { Courtyard,SurveyRead,Live,SidePassage,Kitchen,DamarPhone,ARRevealed,ClockUnlocked,StairCheckpoint }
    public sealed class AtmajaDirector:MonoBehaviour
    {
        public const string SurveyId="DOC_PROLOG_02_SURVEI_DAMAR";
        public const string SurveyText="Lokasi: Eks Kediaman Keluarga Atmaja (Kosong sejak tragedi 1998).\n\n1. Pintu depan digembok rantai dari dalam, jangan buang tenaga lewat depan.\n\n2. Gue nemu jalan masuk lewat jendela dapur di lorong samping timur, tapi harus merunduk lewat bawah pohon tumbang.\n\n3. Catatan tambahan: Ada yang nggak beres sama aplikasi MataBatin.apk di HP gue. Setiap gue arahin ke jendela lantai 2, indikator sensornya mentok merah.";
        public const string LiveIntro="Oke... tes, satu dua. Malam semuanya, balik lagi di Malam Keramat. Malam ini gue nggak sendirian... maksudnya, gue lagi nyusulin Damar yang hilang kontak di Rumah Tua Atmaja. Buat yang baru gabung, bantu pantau layar ya.";
        public FirstPersonController player;
        public Transform gateHinge,kitchenDoor,spawnCourtyard,spawnKitchen,spawnDining,spawnStairs,vaultOutside,vaultInside;
        public GameObject kitchenShutter,damarPhone,clockHands,arFigure,arClue;
        public Transform clockHour,clockMinute,clockDrawer;
        public Vector3 clockDialCentre;
        public Vector3 clockFaceNormal=Vector3.forward;
        public AtmajaPhoneView phoneView;
        public SceneAudioProfile audioProfile;
        public AudioSource rainSource,foleySource,notificationSource,stepsSource;
        public Vector3 origin,east=Vector3.right,north=Vector3.forward;
        public AtmajaStage Stage {get;private set;}
        public bool Busy {get;private set;}
        public bool TreeCleared=>Has("EVENT_P0_TREE_CROUCHED");
        public bool HasSurvey=>Has(SurveyId);
        public bool LiveConnected=>Has("EVENT_P0_LIVE_STARTED");
        public int Viewers=>Has("EVENT_P0_AR_REVEALED")?1420:Has("EVENT_P0_KITCHEN_ENTERED")?250:14;
        private bool underTree,wellHeard;
        private bool clockTimeShown;
        private AudioClip arStatic;
        private Vector3 previousPosition;
        private float walked;
        private int footstep;
        private CampaignState Campaign=>CampaignState.Instance;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        private HudController Hud=>HudController.Instance;
        private bool Has(string id)=>Campaign&&Campaign.HasInventoryItem(id);
        private IEnumerator Start()
        {
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
            RestoreWorld();previousPosition=player.transform.position;
            if(!Has("EVENT_P0_ATMAJA_ARRIVED"))Commit("EVENT_P0_ATMAJA_ARRIVED","PROLOG_ATMAJA_COURTYARD");
            Hud.SetFadeAlpha(1);yield return Hud.FadeBlack(1,0,1.4f);
            Objective();
        }
        public Vector3 P(float x,float depth,float height)=>origin+east*x+north*depth+Vector3.up*height;
        public Vector2 LocalPosition(Vector3 p)=>new Vector2(Vector3.Dot(p-origin,east),Vector3.Dot(p-origin,north));
        public void RestoreWorld()
        {
            gateHinge.localRotation=Quaternion.Euler(0,Has("EVENT_P0_GATE_OPEN")?-95:0,0);
            bool inside=Has("EVENT_P0_KITCHEN_ENTERED");kitchenShutter.SetActive(inside);
            if(kitchenDoor)kitchenDoor.localRotation=Quaternion.Euler(0,Has("EVENT_P0_DINING_OPEN")?-95:0,0);
            if(damarPhone)damarPhone.SetActive(!Has("MATABATIN_APK"));
            if(clockHands)clockHands.SetActive(!Has("ITEM_P0_CLOCK_HANDS"));
            if(clockHour)clockHour.gameObject.SetActive(Has("EVENT_P0_CLOCK_UNLOCKED"));
            if(clockMinute)clockMinute.gameObject.SetActive(Has("EVENT_P0_CLOCK_UNLOCKED"));
            if(Has("EVENT_P0_CLOCK_UNLOCKED"))ShowClockTime();
            var spawn=Has("EVENT_P0_STAIRS_UNLOCKED")?spawnStairs:Has("MATABATIN_APK")?spawnDining:inside?spawnKitchen:spawnCourtyard;
            PlacePlayer(spawn.position,spawn.rotation);
            Stage=Has("EVENT_P0_STAIRS_UNLOCKED")?AtmajaStage.StairCheckpoint:Has("EVENT_P0_CLOCK_UNLOCKED")?AtmajaStage.ClockUnlocked:Has("EVENT_P0_AR_REVEALED")?AtmajaStage.ARRevealed:Has("MATABATIN_APK")?AtmajaStage.DamarPhone:inside?AtmajaStage.Kitchen:TreeCleared?AtmajaStage.SidePassage:LiveConnected?AtmajaStage.Live:HasSurvey?AtmajaStage.SurveyRead:AtmajaStage.Courtyard;
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
            if(LiveConnected&&HasSurvey&&!Has("EVENT_P0_KITCHEN_ENTERED"))
            {
                if(local.x>6.4f&&local.y>-.8f&&!Has("EVENT_P0_SIDE_CHAT"))SideChat();
                if(local.x>7.0f&&local.y>4.9f&&local.y<6.2f&&player.IsCrouching&&player.GetComponent<CharacterController>().height<1.10f)underTree=true;
                if(underTree&&local.y>6.4f&&!TreeCleared)
                    if(Commit("EVENT_P0_TREE_CROUCHED","PROLOG_ATMAJA_COURTYARD")){Stage=AtmajaStage.SidePassage;Objective();}
            }
            if(Phone&&Phone.arModuleUnlocked)
            {
                bool ar=Phone.isPhoneRaised&&Phone.isScreenOn&&Phone.currentScanMode==ScanVisualMode.AR&&phoneView.IsCameraApp;
                if(arFigure)arFigure.SetActive(ar);
                if(arClue)arClue.SetActive(ar);
                if(ar&&!Has("EVENT_P0_AR_REVEALED"))RevealAR();
                if(ar&&!Has("EVENT_P0_AR_CLUE_READ"))ObserveClue();
            }
            if(Has("EVENT_P0_KITCHEN_ENTERED")&&!wellHeard&&Vector3.Distance(player.transform.position,P(2.7f,11.2f,.35f))<2)
            {wellHeard=true;Hud.EnqueueSubtitle("SFX","[KLOTAK... KLOTAK... Suara ember timba ditarik dari dasar sumur]",3);Hud.EnqueueSubtitle("Bisikan","Haus... bukakan...",3);Sfx(audioProfile.knock,P(2.7f,11.2f,.4f),.4f);}
            if(!modal&&Phone&&Phone.isPhoneRaised&&Phone.isScreenOn&&GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz))TryPhotographWell();
            var delta=player.transform.position-previousPosition;previousPosition=player.transform.position;delta.y=0;
            if(delta.sqrMagnitude>1||modal||player.MovementLocked)return;
            walked+=delta.magnitude;
            if(walked>.9f&&player.GetComponent<CharacterController>().isGrounded)
            {walked=0;stepsSource.transform.position=player.transform.position;stepsSource.PlayOneShot(audioProfile.tileSteps[footstep++%audioProfile.tileSteps.Length],player.IsCrouching?.10f:.22f);}
        }
        public void Interact(AtmajaItem item)
        {
            switch(item.action)
            {
                case AtmajaAction.Gate:
                    if(!Has("EVENT_P0_GATE_OPEN")){Campaign.AddInventoryItem("EVENT_P0_GATE_OPEN");StartCoroutine(Swing(gateHinge,-95));Sfx(audioProfile.latch,item.transform.position,.65f);}break;
                case AtmajaAction.Survey:
                    Hud.EnqueueSubtitle("Raka","Itu motornya Damar... Mesinnya udah dingin, berarti dia udah di dalem sejak jam sembilan tadi.",5);
                    Sfx(audioProfile.cloth,item.transform.position,.4f);
                    Hud.OpenDocumentModal("CATATAN SURVEI DAMAR • DOKUMEN #2",SurveyText,()=>{if(Commit(SurveyId,"PROLOG_ATMAJA_COURTYARD")){Stage=LiveConnected?AtmajaStage.Live:AtmajaStage.SurveyRead;Objective();}});break;
                case AtmajaAction.FrontDoor:Hud.ShowBroadcastNotification("Pintu depan digembok rantai dari dalam.\nCatatan Damar: masuk lewat jendela dapur di lorong samping timur.",6);break;
                case AtmajaAction.KitchenDoor:
                    if(Commit("EVENT_P0_DINING_OPEN","PROLOG_ATMAJA_KITCHEN")){StartCoroutine(Swing(kitchenDoor,-95));Objective();}break;
                case AtmajaAction.DamarPhone:TakeDamarPhone();break;
                case AtmajaAction.Well:Hud.ShowBroadcastNotification("Sumur tua ditutup papan lapuk dan dililit rantai.\nSorot celah papan dengan kamera HP.",5);break;
                case AtmajaAction.ClockHands:
                    if(Commit("ITEM_P0_CLOCK_HANDS","PROLOG_MATABATIN_ACQUIRED")){clockHands.SetActive(false);Hud.ShowBroadcastNotification("Jarum Jam Kuningan diambil.",3);Objective();}break;
                case AtmajaAction.Clock:ClockPuzzle();break;
                case AtmajaAction.StairDoor:
                    if(!Has("EVENT_P0_CLOCK_UNLOCKED")){Hud.ShowBroadcastNotification("Pintu tangga terkunci. Periksa jam bandul dan dinding di sampingnya dengan AR.",5);break;}
                    if(Commit("EVENT_P0_STAIRS_UNLOCKED","PROLOG_ATMAJA_STAIRS_UNLOCKED")){Stage=AtmajaStage.StairCheckpoint;Objective();Hud.ShowBroadcastNotification("Checkpoint tersimpan • Lantai 1 selesai.\nLantai 2 belum termasuk build playtest ini.",8);}break;
                case AtmajaAction.Charge:if(Phone.batteryPercent<35)StartCoroutine(Charge());break;
                case AtmajaAction.Window:Hud.ShowBroadcastNotification($"[{GameInputConfig.Instance.vaultKey}] Panjat jendela dapur\n"+(TreeCleared?"Dekati jendela dan hadap ke dalam.":"Baca catatan, mulai live, dan lewati kamboja sambil merunduk."),4);break;
            }
        }
        public bool StartLive()
        {
            if(!HasSurvey||!Phone.isPhoneRaised||!Phone.isScreenOn||Busy)return false;
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
            Hud.ShowBroadcastNotification("Penonton_Terakhir • SAWER Rp500.000\nJangan buang waktu di teras. Tas sahabatmu ada di dekat meja makan. Ambil HP-nya.",10);
            notificationSource.PlayOneShot(audioProfile.notification,.55f);
        }
        public bool CanVault()=>!Busy&&TreeCleared&&LiveConnected&&HasSurvey&&!Has("EVENT_P0_KITCHEN_ENTERED");
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
        }
        private IEnumerator Swing(Transform hinge,float angle)
        {var from=hinge.localRotation;var to=Quaternion.Euler(0,angle,0);float t=0;while(t<1){t+=Time.deltaTime;hinge.localRotation=Quaternion.Slerp(from,to,Mathf.SmoothStep(0,1,t));yield return null;}}
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
            if(!player.IsCrouching){Hud.ShowBroadcastNotification("Merunduk untuk meraih HP Damar di kolong meja.",4);return;}
            if(!Commit("MATABATIN_APK","PROLOG_MATABATIN_ACQUIRED",()=>Campaign.Data.upgrades.Add("MATABATIN_AR")))return;
            damarPhone.SetActive(false);Phone.arModuleUnlocked=true;Stage=AtmajaStage.DamarPhone;
            Hud.EnqueueSubtitle("Raka","Ini HP-nya Damar... Layarnya pecah, tapi kartu memorinya masih kebaca. Gue pindahin dulu aplikasinya ke HP gue.",7);Objective();
        }
        private void RevealAR()
        {
            if(!Commit("EVENT_P0_AR_REVEALED","PROLOG_MATABATIN_ACQUIRED"))return;
            Stage=AtmajaStage.ARRevealed;
            // Original short, enveloped static burst, routed through the existing UI mixer.
            if(!arStatic)
            {
                const int rate=32000;var samples=new float[rate/2];var noise=new System.Random(1998);
                for(int i=0;i<samples.Length;i++){float t=i/(float)samples.Length; samples[i]=(float)(noise.NextDouble()*2-1)*.22f*Mathf.Sin(t*Mathf.PI)*(1+.25f*Mathf.Sin(i*.047f));}
                arStatic=AudioClip.Create("MataBatin_Static",samples.Length,1,rate,false);arStatic.SetData(samples,0);
            }
            notificationSource.PlayOneShot(arStatic,.65f);
            phoneView.AddChat("@agung_gaming","ANJIR APAAN TUH DI UJUNG MEJA BARUSAN?!");
            phoneView.AddChat("@siti_nurhaliza","ITU BUKAN SETTINGAN WOI KEPALANYA MIRING!");
            phoneView.AddChat("@horor_hunter","Bang Raka lari bang! Itu filter kameranya nangkep penunggu situ!");Objective();
        }
        public bool TryPhotographWell()
        {
            if(Has("EVENT_P0_WELL_BONUS")||!Has("EVENT_P0_KITCHEN_ENTERED")||Phone.batteryPercent<1)return false;
            var camera=player.GetComponent<InteractionInteractor>().playerCamera;var target=P(2.7f,11.2f,.75f);var delta=target-camera.transform.position;
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
            if(!Has("MATABATIN_APK")){Hud.ShowBroadcastNotification("Cari HP Damar dan pasang modul kameranya terlebih dahulu.",4);return;}
            if(!Has("ITEM_P0_CLOCK_HANDS")){Hud.ShowBroadcastNotification("Jarum jam copot. Cari Jarum Jam Kuningan di meja tamu.",5);return;}
            if(Has("EVENT_P0_CLOCK_UNLOCKED")){if(!Has("DOC_PROLOG_03_KORAN_1998"))OpenNewspaper();else Hud.ShowBroadcastNotification("Jam menunjukkan 03:45. Kunci Pintu Tangga Tengah sudah diambil.",4);return;}
            if(!Has("EVENT_P0_AR_CLUE_READ")){Hud.ShowBroadcastNotification("Belum tahu waktu yang benar. Periksa dinding rumah melalui kamera AR.",4);return;}
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
            var camera=player.GetComponent<InteractionInteractor>().playerCamera;var delta=arClue.transform.position-camera.transform.position;
            if(delta.magnitude>3.4f||Vector3.Dot(camera.transform.forward,delta.normalized)<.86f)return;
            if(Physics.Raycast(camera.transform.position,delta.normalized,Mathf.Max(0,delta.magnitude-.03f),~0,QueryTriggerInteraction.Ignore))return;
            if(Commit("EVENT_P0_AR_CLUE_READ","PROLOG_MATABATIN_ACQUIRED"))Hud.ShowBroadcastNotification("Petunjuk dinding dicatat di Jurnal HP.",3);
        }
        private void OpenNewspaper()=>Hud.OpenDocumentModal("KLIPING KORAN 1998 • DOKUMEN #3","TRAGEDI KELUARGA ATMAJA: Satu Keluarga Ditemukan Tewas Mengenaskan. Polisi menduga sang ibu, Nyonya Sri Atmaja (38), mengalami depresi berat dan gangguan jiwa akibat mengikuti aliran klenik sebelum meracuni anak-anaknya dan mengakhiri hidupnya sendiri di lantai dua pada tengah malam.",()=>{Commit("DOC_PROLOG_03_KORAN_1998","PROLOG_MATABATIN_ACQUIRED");Objective();});
        private void ShowClockTime()
        {
            if(clockTimeShown)return;clockTimeShown=true;
            if(clockHour){clockHour.gameObject.SetActive(true);clockHour.RotateAround(clockDialCentre,clockFaceNormal,112.5f);}
            if(clockMinute){clockMinute.gameObject.SetActive(true);clockMinute.RotateAround(clockDialCentre,clockFaceNormal,270);}
            if(clockDrawer)clockDrawer.position+=clockFaceNormal*.18f;
        }
        public void Objective()
        {
            string text=Stage==AtmajaStage.StairCheckpoint?"Checkpoint Lantai 1 • Kelanjutan Lantai 2 belum tersedia dalam build ini.":Has("EVENT_P0_CLOCK_UNLOCKED")?"Gunakan Kunci Pintu Tangga Tengah.":Has("EVENT_P0_AR_CLUE_READ")?"Cari komponen yang hilang, lalu cocokkan mekanisme jam dengan petunjuk.":Has("EVENT_P0_AR_REVEALED")?"Jelajahi ruang tamu dan ruangan depan. Cari petunjuk lewat kamera.":Has("MATABATIN_APK")?"Buka aplikasi Kamera, lalu aktifkan MataBatin AR.":Has("EVENT_P0_DINING_OPEN")?"Periksa ruang makan. Cari barang milik Damar.":Has("EVENT_P0_KITCHEN_ENTERED")?"Jendela terkunci. Cari jalan ke ruangan lain.":TreeCleared?"Dekati jendela dapur yang pecah. Gunakan tombol Vault.":LiveConnected?"Cari jalan masuk di sisi timur rumah.":HasSurvey?"Buka Kamera di aplikasi HP, lalu mulai live.":"23:52 WIB • Cari jejak Damar di halaman rumah.";
            Hud.SetObjective(text);
        }
        private void Sfx(AudioClip clip,Vector3 where,float gain)
        {if(!foleySource||!clip)return;foleySource.transform.position=where;foleySource.PlayOneShot(clip,gain);}
        private void OnDestroy(){if(arStatic)Destroy(arStatic);}
    }
}
