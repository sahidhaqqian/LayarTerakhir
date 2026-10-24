using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase3
{
    [DefaultExecutionOrder(-15)]
    public sealed partial class Phase3Director : MonoBehaviour,IPhoneChapterContent,IContextInteractionResolver
    {
        public FirstPersonController player;
        public Phase3World world;
        public Phase3Balance balance;
        public AtmajaPhoneView phoneView;
        public Phase3Wewe wewe;
        public Phase3Boss boss;
        public Phase3Soundscape soundscape;
        public bool Ready { get; private set; }
        public bool Dead { get; private set; }
        public bool Hidden { get; private set; }
        public bool StoryPlaying { get; private set; }
        public bool Analyzer { get; private set; }
        public bool RecordingEVP { get; private set; }
        public bool ThermoActive { get; private set; }
        public float EvpProgress { get; private set; }
        public float Elapsed => Campaign.Data.phase3.elapsed_gameplay_seconds;
        public CampaignState Campaign => CampaignState.Instance;
        public SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        public string CurrentRoom => world.Room(player.transform.position);
        public bool ClockRunning => Ready&&!Dead&&!UiFocus.HardModal&&!UiFocus.Cinematic&&!Hud.IsModalOpen&&Time.timeScale>0;
        public bool CameraCanScan => ClockRunning&&Phone.isPhoneRaised&&Phone.isScreenOn&&Phone.batteryPercent>0&&phoneView.IsCameraApp&&(!UiFocus.Phone||phoneView.ViewfinderActive)&&!Hidden;
        public bool CanBeginLive => Ready&&!Dead&&!boss.Active&&!Completed("P3_BOSS_WIN")&&Phone.batteryPercent>0&&!Completed("P3_START_LIVE");
        public bool CameraLocked => false;
        public int Viewers => Completed("P3_BOSS_WIN")?666000:185000;
        public string CameraStatus => "23:54 • "+(Phone.currentScanMode==ScanVisualMode.UV?"UV RAJAH":Phone.currentScanMode==ScanVisualMode.AR?"LENSA AR":"KAMERA")+(Analyzer?" • EVP v2":"")+(ThermoActive?" • THERMO 26°C":"");
        public bool OwnsInteraction => Hidden;
        public bool[] Altar { get; } = new bool[3];
        private Phase3WorldItem hidingItem;
        private Vector3 beforeHide;
        public Vector3 HidingEntrance => Hidden?beforeHide:player.transform.position;
        public Transform HidingCover => hidingItem?hidingItem.transform:null;
        public float MillLearningUntil { get; private set; }
        private Quaternion beforeHideRotation;
        private string previousRoom="",objective="";
        private float slingDwell,rajahDwell,evpHeld,evpReal;
        private bool evpWasHeld,evpFinished;
        private bool millCuePlayed;
        private Phase3EvpSource recordSource;
        public Phase3EvpSource SelectedSource { get; private set; }
        public Phase3VoiceTag SelectedTag => SelectedSource?SelectedSource.Tag(Elapsed,balance):Phase3VoiceTag.Ambient;

        private IEnumerator Start()
        {
            while(GameShell.AwaitingStart||GameShell.MenuSceneLoading)yield return null;
            if(!Campaign.LoadCheckpoint()||!CampaignRouting.IsPhase3(Campaign.Data)||!Completed("P3_DEPART_KOS"))
            {GameShell.Instance?.ReturnAfterDeath("Checkpoint pabrik belum tersedia. Save lama tetap utuh.");yield break;}
            Phone.ChapterContent=this;player.GetComponent<InteractionInteractor>().ContextResolver=this;
            Phone.hasSmartphoneInInventory=Phone.arModuleUnlocked=Phone.uvModuleUnlocked=Phone.selfieModuleUnlocked=true;
            Phone.isPhoneRaised=Phone.isSelfieCamera=false;Phone.currentScanMode=ScanVisualMode.Normal;
            Phone.isScreenOn=Campaign.Data.battery>0;Phone.isBroadcastConnected=Completed("P3_START_LIVE")||Completed("P3_EPILOGUE_CHAT");Phone.RestoreBattery(Campaign.Data.battery);
            RestoreWorld();Ready=true;Hud.SetFadeAlpha(0);
            Commit("P3_ARRIVE_PABRIK");RefreshObjective();
        }
        public bool Completed(string id)=>Campaign&&Campaign.Data.phase3.Completed(id);
        public string SafeAnchor => CurrentRoom=="OFFICE"||CurrentRoom=="BALCONY"?"OFFICE":CurrentRoom=="BAGASSE"?"BAGASSE":
            CurrentRoom=="BOILER"?"BOILER":CurrentRoom=="ARENA"?"ALTAR":CurrentRoom=="MILL"?"MILL":"YARD";
        public bool Commit(string id,Action<CampaignData> mutate=null,string checkpoint=null,string anchor=null)
        {
            if(Completed(id))return true;
            string cp=Phase3Campaign.InvestigationCheckpoint(Campaign.Data,checkpoint);
            bool saved=Campaign.TryCommit(cp,d=>
            {
                d.phase3.Add(id);mutate?.Invoke(d);
                if(anchor!=null&&!d.phase3.Completed("P3_BOSS_WIN"))d.phase3.safe_anchor=cp=="PHASE3_CP5"?"ALTAR":anchor;
            });
            if(saved){RefreshWorld();RefreshObjective();}
            else Hud.ShowBroadcastNotification("Penyimpanan gagal. Progres belum berubah; coba lagi.",5);
            return saved;
        }
        public void RestoreWorld()
        {
            Hidden=Dead=RecordingEVP=Analyzer=StoryPlaying=false;EvpProgress=evpHeld=evpReal=slingDwell=rajahDwell=0;
            evpWasHeld=evpFinished=false;recordSource=null;previousRoom="";
            MillLearningUntil=Elapsed+balance.learnWindow;
            millCuePlayed=false;
            UiFocus.Cinematic=UiFocus.Clock=UiFocus.Phone=false;player.MovementLocked=false;
            player.GetComponent<ShoulderCameraRig>().FirstPersonOverride=false;
            MovePlayer(world.Anchor(Campaign.Data.phase3.safe_anchor));
            for(int i=0;i<3;i++)Altar[i]=false;
            foreach(var tile in world.floors)tile.Restore();
            RefreshWorld();wewe.Restore();boss.Restore();
            if(Completed("P3_COMPLETE"))ShowHandoff();
        }
        public void RefreshWorld()
        {
            bool access=Completed("P3_ACCESS_GENERATOR"),win=Completed("P3_BOSS_WIN");
            GateOpen(world.accessGate,access);world.accessLight.enabled=access;world.sling.SetActive(access);
            GateOpen(world.hallGate,(win||Phase3Ids.RitualReady(Campaign.Data))&&!(boss&&boss.Active));
            world.arenaLock.SetActive(boss&&boss.Active);GateOpen(world.serviceGate,win);
            world.doll.SetActive(access&&!Completed("P3_TAKE_BONEKA"));world.sickle.SetActive(!Completed("P3_TAKE_SABIT"));world.cloth.SetActive(!Completed("P3_TAKE_SELENDANG"));
            world.secret.SetActive(Completed("P3_VALVE_09")&&!Campaign.Data.secret_items.Contains(Phase3Ids.Secret));
            world.card.SetActive(!Completed("P3_DOC_DAMAR_CARD"));world.shoe.SetActive(!Campaign.Data.inventory.Contains(Phase3Ids.Shoe));
            for(int i=0;i<3;i++)world.altarRelics[i].SetActive(win||Altar[i]);
        }
        private void GateOpen(GameObject go,bool open)=>go.GetComponent<Phase3Gate>().SetOpen(open,!Ready);
        private void Update()
        {
            if(!Ready||Dead)return;
            if(player.transform.position.y< -2)
            {if(!Completed("P3_BOSS_WIN"))Die("FALL — lantai rapuh runtuh");else MovePlayer(world.Anchor("ALTAR"));return;}
            if(!ClockRunning){wewe.Suspend();boss.Suspend();return;}
            float dt=Time.deltaTime;Campaign.Data.phase3.elapsed_gameplay_seconds+=dt;phoneView.RefreshLensPose();
            string room=CurrentRoom;
            if(room!=previousRoom)
            {
                if(room=="MILL"&&!Completed("P3_ENTER_GILINGAN")&&!Commit("P3_ENTER_GILINGAN",null,"PHASE3_CP2","MILL"))return;
                if(room=="MILL")MillLearningUntil=Elapsed+balance.learnWindow;
                if(room=="BOILER"&&!Completed("P3_ENTER_BOILER")&&!Commit("P3_ENTER_BOILER",null,"PHASE3_CP3","BOILER"))return;
                previousRoom=room;
            }
            if(room=="MILL"&&player.transform.position.z>8&&!millCuePlayed&&(Completed("P3_DECOY_VOICE1")||Commit("P3_DECOY_VOICE1")))
            {
                millCuePlayed=true;
                Hud.EnqueueSubtitle("Suara Tiruan — Damar","RAKA! KA, GUE DI SINI! TOLONGIN GUE, TALI GUE MAU PUTUS KE BAWAH MESIN!",8);
                Hud.ShowBroadcastNotification("TIRUAN TERDETEKSI • cek EVP dan UV. Silang berarti lantai rapuh. Jalur pipa kanan kokoh.",10);
                soundscape.Cue("Mimic",world.Anchor("LURE_MILL").position);
            }
            if(room=="MILL"&&player.transform.position.x>15&&player.transform.position.z>24&&!Completed("P3_CROSS_CATWALK")&&Commit("P3_CROSS_CATWALK"))
            {Chat("dedi_kobuzer","Itu suara Damar palsu! Untung dicek pakai frekuensi.");Chat("anak_teknik","Jalur tengah rapuh; baca silang di UV.");}
            if(boss.Active)boss.Tick(dt);else if(!Completed("P3_BOSS_WIN"))wewe.Tick(dt);
            foreach(var tile in world.floors)tile.Tick(dt);
            if(Dead)return;
            if(GameInputConfig.Instance.GetActionDown(GameAction.UseFlare))ThrowFlare();
            if(GameInputConfig.Instance.GetActionDown(GameAction.ModeThermo)&&Phone.isPhoneRaised&&Phone.isScreenOn)ThermoActive=!ThermoActive;
            UpdateInvestigation(dt);soundscape.Tick(dt);RefreshObjective();
            if(Completed("P3_BOSS_WIN")&&!boss.ResolvingVisual&&!Completed("P3_EPILOGUE")&&!StoryPlaying&&!storyRetry&&Phone.batteryPercent>0)StartCoroutine(Epilogue());
        }
        public bool ClearLine(Vector3 from,Vector3 to,Transform target=null)
        {
            Vector3 delta=to-from;
            foreach(var h in Physics.RaycastAll(from,delta.normalized,Mathf.Max(0,delta.magnitude-.04f),~((1<<24)|(1<<26)|(1<<29)|(1<<2)|(1<<31)),QueryTriggerInteraction.Ignore))
                if(!h.transform.IsChildOf(player.transform)&&!(target&&h.transform.IsChildOf(target)))return false;
            return true;
        }
        public bool Aim(Vector3 point,float range,float angle=20,Transform target=null)
        {
            var cam=phoneView.FeedCamera;if(!cam)return false;Vector3 delta=point-cam.transform.position,v=cam.WorldToViewportPoint(point);
            return delta.magnitude<=range&&v.z>cam.nearClipPlane&&v.x>=0&&v.x<=1&&v.y>=0&&v.y<=1&&Vector3.Angle(cam.transform.forward,delta)<=angle&&ClearLine(cam.transform.position,point,target);
        }
        public void MovePlayer(Transform a)
        {var c=player.GetComponent<CharacterController>();c.enabled=false;player.transform.SetPositionAndRotation(a.position,a.rotation);c.enabled=true;player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();}
        public void Die(string cause)
        {
            if(Dead)return;Dead=true;boss.Abort();player.MovementLocked=true;Phone.isPhoneRaised=false;Phone.SyncVisualState();
            phoneView.SetViewfinder(false);Hud.SetFadeAlpha(1);Debug.Log("P3_FAILURE "+cause);GameShell.Instance?.ReturnAfterDeath(cause+" — Continue memuat snapshot terakhir tanpa isi daya otomatis.");
        }
        public void RefreshObjective()
        {
            if(!Ready)return;
            string next=Completed("P3_COMPLETE")?"Babak 3 selesai. Tujuan: "+Phase3Ids.Destination:
                Completed("P3_EPILOGUE")?"Kembali ke motor di halaman timbangan. Balkon servis masih dapat diperiksa.":
                Completed("P3_BOSS_WIN")?"Segel ketiga tersimpan. Isi daya manual bila HP mati, lalu baca pesan.":
                boss.Active?boss.Status:!Completed("P3_ACCESS_GENERATOR")?"Gudang Gilingan: periksa sumber EVP, baca silang UV; ikuti pipa kanan dan engkol generator akses tiga kali.":
                !Campaign.Data.evidence_ids.Contains(Phase3Ids.Sling)?"Foto ayunan dengan AR. Boneka dan Catatan Mandor adalah objek terpisah.":
                !Phase3Ids.AllEvidence(Campaign.Data)?"Bangsal Ketel: rekam segmen ASLI interkom Kantor Mandor; foto rajah Gudang Ampas lewat UV.":
                !Phase3Ids.AllRelics(Campaign.Data)?"Lengkapi tiga pusaka: boneka, sabit mandor, dan selendang di Gudang Ampas.":
                !Completed("P3_JOURNAL_CONFIRMED")?"Tiga bukti lengkap. Konfirmasi identitas secara sadar di Jurnal Klenik.":
                "Aula Tungku: pasang tiga pusaka pada dudukan, lalu periksa sosok yang terikat.";
            if(objective!=next){objective=next;Hud.SetObjective(next);}
        }
        private void OnDestroy()
        {if(boss)boss.Abort();if(Phone&&ReferenceEquals(Phone.ChapterContent,this))Phone.ChapterContent=null;if(overlay)RemoveOverlay();}
    }
}
