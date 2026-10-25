using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase4
{
    [DefaultExecutionOrder(-15)]
    public sealed partial class Phase4Director : MonoBehaviour, IPhoneChapterContent, IPhoneChapterClock, IContextInteractionResolver
    {
        public FirstPersonController player;
        public AtmajaPhoneView phoneView;
        public Phase4World world;
        public Phase4Balance balance;
        public Phase4Banaspati entity;
        public Phase4Ritual ritual;
        public Phase4Soundscape soundscape;
        public bool Ready { get; private set; }
        public bool Dead { get; private set; }
        public bool Hidden { get; private set; }
        public bool RecordingEVP { get; private set; }
        public bool Analyzer { get; private set; }
        public bool ThermoActive { get; private set; }
        public bool ClarityActive => Elapsed < clarityUntil;
        public bool StoryPlaying { get; private set; }
        public float EvpProgress { get; private set; }
        public CampaignState Campaign => CampaignState.Instance;
        public SmartphoneController Phone => SmartphoneController.Instance;
        public HudController Hud => HudController.Instance;
        public Phase4Data State => Campaign.Data.phase4;
        public float Elapsed => State.elapsed_gameplay_seconds;
        public string CurrentZone => world.Zone(player.transform.position);
        public bool ClockRunning => Ready && !Dead && !UiFocus.HardModal && !UiFocus.Cinematic && !Hud.IsModalOpen && Time.timeScale > 0 &&
            (Application.isFocused || Application.isBatchMode || Phase4DiagnosticsActive);
        public bool PhoneClockRunning => ClockRunning;
        private static bool Phase4DiagnosticsActive => Debug.isDebugBuild && Environment.GetCommandLineArgs().Contains("-phase4Audit");
        public bool CameraCanScan => ClockRunning && !Hidden && Phone.isPhoneRaised && Phone.isScreenOn && Phone.batteryPercent > 0 && phoneView.IsCameraApp;
        public bool OwnsInteraction => Hidden;
        public bool CameraLocked => false;
        public bool CanBeginLive => Ready && !Completed("P4_START_LIVE") && !Completed("P4_COMPLETE") && Phone.batteryPercent > 0;
        public int Viewers => Completed("P4_BOSS_WIN") ? 999999 : Completed("P4_START_LIVE") ? 780000 : 0;
        public string CameraStatus => "23:53 • " + (Phone.currentScanMode == ScanVisualMode.UV ? "UV RAJAH v2" : Phone.currentScanMode == ScanVisualMode.AR ? "LENSA AR" : "KAMERA") +
            " • SANITY " + Campaign.Data.sanity.ToString("0") + "%";
        public string SafeAnchor => Completed("P4_COMPLETE") ? "BOUNDARY" : Completed("P4_BOSS_WIN") ? "PORTAL" :
            CurrentZone == "NORTH" || CurrentZone == "EAST" || CurrentZone == "WEST" || CurrentZone == "SOUTH" ? CurrentZone :
            Completed("P4_PENDOPO_REACHED") ? "PENDOPO" : Completed("P4_BAMBOO_BRANCH_1") ? "J1" : "ENTRY";
        public bool[] Preview { get; } = new bool[3];
        public float Temperature { get; private set; } = 27;
        private float thermalDwell, arDwell, eyeDwell, evpHeld;
        private bool evpWasHeld, evpFinished;
        private string objective = "";
        private float nextZoneRetry, nextReadout;
        private float nextLiveRetry, nextStoryRetry, nextHandoffRetry;
        private Phase4WorldItem hidingItem;
        private Vector3 hideEntrance;
        private Quaternion hideRotation;
        public Vector3 HidingEntrance => Hidden ? hideEntrance : player.transform.position;

        private IEnumerator Start()
        {
            while (GameShell.AwaitingStart || GameShell.MenuSceneLoading) yield return null;
            if (!Campaign.LoadCheckpoint() || !CampaignRouting.IsPhase4(Campaign.Data) || !Completed("P4_DEPART_KOS"))
            { GameShell.Instance?.ReturnAfterDeath("Checkpoint Karang Setra belum tersedia. Save sebelumnya tetap utuh."); yield break; }
            Phone.ChapterContent = this; player.GetComponent<InteractionInteractor>().ContextResolver = this;
            Phone.hasSmartphoneInInventory = Phone.arModuleUnlocked = Phone.uvModuleUnlocked = Phone.selfieModuleUnlocked = true;
            Phone.isPhoneRaised = Phone.isSelfieCamera = false; Phone.currentScanMode = ScanVisualMode.Normal;
            Phone.isScreenOn = Campaign.Data.battery > 0; Phone.RestoreBattery(Campaign.Data.battery);
            Phone.isBroadcastConnected = Completed("P4_START_LIVE");
            RestoreWorld(); Ready = true; Hud.SetFadeAlpha(0); ApplyAvatarMaterials(); RefreshObjective();
            Receipt("P4_ARRIVE_FOREST", "ENTRY");
            if(CanBeginLive) { StartLive(); nextLiveRetry=Elapsed+5; }
        }
        public bool Completed(string id) => Campaign && Campaign.Data.phase4.Completed(id);
        public bool Receipt(string id, string anchor = null, string checkpoint = null, Action<CampaignData> mutation = null)
        {
            bool ok = Phase4Campaign.Receipt(Campaign, id, anchor ?? SafeAnchor, checkpoint, mutation);
            if (ok) { RefreshWorld(); RefreshObjective(); } else SaveError();
            return ok;
        }
        public void RestoreWorld()
        {
            Hidden = Dead = RecordingEVP = StoryPlaying = Analyzer = ThermoActive = false;
            thermalDwell = arDwell = eyeDwell = evpHeld = EvpProgress = 0; evpWasHeld = evpFinished = false;
            UiFocus.Cinematic = UiFocus.Clock = UiFocus.Phone = false; player.MovementLocked = false;
            player.GetComponent<ShoulderCameraRig>().FirstPersonOverride = false;
            MovePlayer(world.Anchor(State.safe_anchor)); for (int i = 0; i < 3; i++) Preview[i] = false;
            ritual.Restore(); entity.Restore(); RefreshWorld();
            if (Completed("P4_COMPLETE")) ShowBoundary();
        }
        public void RefreshWorld()
        {
            bool started = Completed("P4_RITUAL_TRIGGER"), won = Completed("P4_BOSS_WIN");
            world.kendi.SetActive(!Campaign.Data.relic_ids.Contains(Phase4Ids.Kendi));
            world.blade.SetActive(!Campaign.Data.relic_ids.Contains(Phase4Ids.Blade));
            world.soil.SetActive(!Campaign.Data.relic_ids.Contains(Phase4Ids.Soil));
            world.mirror.SetActive(Completed("P4_SECRET_WHEEL_ALIGNED") && !Campaign.Data.secret_items.Contains(Phase4Ids.Secret));
            world.hiddenDrawer.transform.localPosition = Completed("P4_SECRET_WHEEL_ALIGNED") ? Vector3.forward * .32f : Vector3.zero;
            world.illusionWall.SetActive(!Campaign.Data.evidence_ids.Contains(Phase4Ids.Aura));
            world.glasses.SetActive(!Completed("P4_DAMAR_GLASSES"));
            world.portalCover.SetActive(!won); world.portalStairs.SetActive(won);
            for (int i = 0; i < 3; i++) world.offerings[i].SetActive(i == 0 ? Preview[i] && !started : started || Preview[i]);
            foreach (var torch in world.torches) torch.Present(started, ritual.TemporaryTorch == torch);
            if (won) { world.fireVisual.SetActive(false); world.banaspati.SetActive(false); world.suro.SetActive(false); world.mergeSuro.SetActive(false); }
        }
        private void Update()
        {
            if (!Ready || Dead) return;
            if (!ClockRunning) { entity.Suspend(); return; }
            float dt = Time.deltaTime; State.elapsed_gameplay_seconds += dt; phoneView.RefreshLensPose();
            if(CanBeginLive && Elapsed>=nextLiveRetry){nextLiveRetry=Elapsed+5;StartLive();}
            if (player.transform.position.y < -12) { MovePlayer(world.Anchor(SafeAnchor)); return; }
            TickChat(); UpdateZones(); UpdateSanity(dt); UpdateScanning(dt); ritual.Tick(dt); entity.Tick(dt); soundscape.Tick(dt);
            var input = GameInputConfig.Instance;
            if (input.GetActionDown(GameAction.ModeThermo) && Phone.isPhoneRaised && Phone.isScreenOn) ThermoActive = !ThermoActive;
            if (input.GetActionDown(GameAction.UseIncense)) UseIncense();
            if (input.GetActionDown(GameAction.UseFlare)) Hud.ShowBroadcastNotification("Api asing ditolak desa. Simpan flare untuk perjalanan berikutnya.", 4);
            if (Completed("P4_BOSS_WIN") && !Completed("P4_FIFTH_GATE_REVEAL") && !StoryPlaying && Phone.batteryPercent > 0 && Elapsed>=nextStoryRetry) { nextStoryRetry=Elapsed+5;StartCoroutine(Epilogue()); }
            if (Completed("P4_COMPLETE") && !Phase4DiagnosticsActive && Elapsed >= nextHandoffRetry &&
                Vector3.Distance(player.transform.position, world.Anchor("BOUNDARY").position) < 1.7f)
            {
                nextHandoffRetry = Elapsed + 5;
                if (UnityEngine.Application.CanStreamedLevelBeLoaded(CampaignRouting.MarketScene) &&
                    LayarTerakhir.Phase5.Phase5Campaign.Enter(Campaign))
                {
                    player.MovementLocked = true;
                    UnityEngine.SceneManagement.SceneManager.LoadScene(CampaignRouting.MarketScene); return;
                }
                SaveError();
            }
            RefreshObjective();
        }
        private void UpdateZones()
        {
            if (Elapsed < nextZoneRetry) return; nextZoneRetry = Elapsed + .5f;
            string zone = CurrentZone;
            if (zone == "J1" && !Completed("P4_ENTER_BAMBOO")) Receipt("P4_ENTER_BAMBOO", "J1", "PHASE4_CP2");
            for (int i = 1; i <= 3; i++)
                if (zone == "J" + i && !Completed("P4_BAMBOO_BRANCH_" + i) && (i == 1 || Completed("P4_BAMBOO_BRANCH_" + (i - 1))))
                { if (Receipt("P4_BAMBOO_BRANCH_" + i, "J1")) QueueBranchChat(i); }
            if (zone == "PENDOPO" && !Completed("P4_PENDOPO_REACHED"))
                Receipt("P4_PENDOPO_REACHED", "PENDOPO", "PHASE4_CP3");
            if (Completed("P4_PENDOPO_REACHED") && !Completed("P4_BANASPATI_REVEAL") && Vector3.Distance(player.transform.position, world.Anchor("PENDOPO").position) < 10)
            {
                if (Receipt("P4_BANASPATI_REVEAL", "PENDOPO"))
                { entity.Reveal(); Hud.EnqueueSubtitle("MataBatin.apk", "BANASPATI & ARWAH MBAH SURO — KEBAL. JANGAN GUNAKAN FLARE. CARI TIGA BUKTI DAN PUSAKA.", 9); soundscape.Cue("Reveal", world.suro.transform.position); }
            }
        }
        public bool ClearLine(Vector3 from, Vector3 to, Transform target = null, bool ar = false)
        {
            Vector3 delta = to - from; int mask = ~((1 << 24) | (1 << 26) | (1 << 29) | (1 << 2) | (1 << 31));
            if (ar) mask &= ~(1 << 30);
            foreach (var hit in Physics.RaycastAll(from, delta.normalized, Mathf.Max(0, delta.magnitude - .04f), mask, QueryTriggerInteraction.Ignore))
                if (!hit.transform.IsChildOf(player.transform) && !(target && hit.transform.IsChildOf(target))) return false;
            return true;
        }
        public bool Aim(Transform target, float range, float angle = 25, Transform ignore = null, bool ar = false)
        {
            var camera = phoneView.FeedCamera; if (!camera || !target) return false;
            Vector3 delta = target.position - camera.transform.position, view = camera.WorldToViewportPoint(target.position);
            return Vector3.Distance(player.cameraPivot.position,target.position) <= range && view.z > camera.nearClipPlane && view.x >= 0 && view.x <= 1 && view.y >= 0 && view.y <= 1 &&
                Vector3.Angle(camera.transform.forward, delta) <= angle && ClearLine(camera.transform.position, target.position, ignore, ar);
        }
        public void MovePlayer(Transform anchor)
        {
            var capsule = player.GetComponent<CharacterController>(); capsule.enabled = false;
            player.transform.SetPositionAndRotation(anchor.position, anchor.rotation); capsule.enabled = true;
            player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
        public void Die(string cause)
        {
            if (Dead || Completed("P4_BOSS_WIN")) return;
            Dead = true; entity.Suspend(); player.MovementLocked = true; Phone.isPhoneRaised = false; Phone.SyncVisualState();
            phoneView.SetViewfinder(false); Debug.Log("P4_FAILURE " + cause);
            GameShell.Instance?.ReturnAfterDeath(cause + " — Continue memulihkan sektor dan checkpoint yang sudah tersimpan.");
        }
        public void SaveError() => Hud.ShowBroadcastNotification("Progres belum tersimpan. Coba lagi setelah aman.", 4);
        public void RefreshObjective()
        {
            if (!Ready) return;
            string text = Completed("P4_COMPLETE") ? "Pasar Setan — di balik gerbang kelima. Malam ini belum berakhir." :
                Completed("P4_FIFTH_GATE_REVEAL") ? "Gerbang kelima terbuka. Periksa Arsip Saksi, lalu turun melalui tangga Pendopo." :
                Completed("P4_BOSS_WIN") ? "Segel keempat tersimpan. Isi daya jika layar mati, lalu baca pesan Penonton_Terakhir." :
                Completed("P4_SOUTH_OFF") ? "Keempat obor padam. Kembali ke Pendopo: satu Blitz penutup pada inti gabungan." :
                ritual.Active ? ritual.Status : !Completed("P4_PENDOPO_REACHED") ? "Hutan Bambu: ikuti jejak Damar. Verifikasi tiga simpang dengan tanda dunia; chat bisa berbohong." :
                !Phase4Ids.AllEvidence(Campaign.Data) ? "Selidiki Utara (Thermo+66°C), Timur (foto AR), dan Barat (EVP). Ambil tiga pusaka secara terpisah." :
                !Phase4Ids.AllRelics(Campaign.Data) ? "Lengkapi Kendi, Belati Perak, dan Kantong Tanah; air di Sumur Barat." :
                !Completed("P4_JOURNAL_CONFIRMED") ? "Tiga bukti lengkap. Konfirmasi identitas di Jurnal Klenik." :
                "Pasang tiga pusaka pada Batu Ruwatan, kemudian aktifkan ritual secara sadar.";
            if (text != objective) { objective = text; Hud.SetObjective(text); }
        }
        private void OnDestroy()
        {
            if (Phone && ReferenceEquals(Phone.ChapterContent, this)) Phone.ChapterContent = null;
            if (overlay) RemoveOverlay(); DisposeAvatarMaterials();
        }
    }
}
