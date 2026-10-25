using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase5
{
    [DefaultExecutionOrder(-15)]
    public sealed partial class Phase5Director : MonoBehaviour, IPhoneChapterContent, IPhoneChapterClock, IPhoneRitualPower
    {
        public FirstPersonController player;
        public AtmajaPhoneView phoneView;
        public Phase5World world;
        public Phase5Balance balance;
        public Phase5Boss boss;
        public Phase5Replicas replicas;
        public Phase5Soundscape soundscape;
        public bool Ready { get; private set; }
        public bool Dead { get; private set; }
        public bool ActionBusy { get; private set; }
        public bool ThermoActive { get; private set; }
        public bool Analyzer { get; private set; }
        public bool RecordingEVP { get; private set; }
        public CampaignState Campaign => CampaignState.Instance;
        public SmartphoneController Phone => SmartphoneController.Instance;
        public HudController Hud => HudController.Instance;
        public Phase5Data State => Campaign.Data.phase5;
        public float Elapsed => State.elapsed_gameplay_seconds;
        public string Zone => world.Zone(player.transform.position);
        public bool Completed(string id) => Campaign && State.Completed(id);
        public bool FinalChoice => Campaign && Phase5Ids.FinalChoice(Campaign.Data);
        public bool RitualCameraPower => Ready && !Dead && FinalChoice;
        public bool ClockRunning => Ready && !Dead && !UiFocus.HardModal && !UiFocus.Cinematic && !UiFocus.Clock && !Hud.IsModalOpen && Time.timeScale > 0 &&
            (Application.isFocused || Application.isBatchMode || DiagnosticsActive);
        public static bool DiagnosticsActive => Debug.isDebugBuild && Environment.GetCommandLineArgs().Contains("-phase5Audit");
        public bool PhoneClockRunning => ClockRunning;
        public bool CameraCanScan => ClockRunning && Phone.isPhoneRaised && Phone.isScreenOn && Phone.HasCameraPower && phoneView.IsCameraApp;
        public bool CameraLocked => false;
        public bool CanBeginLive => Ready && !Completed("P5_LIVE_LOCK_1M") && Campaign.Data.ending == EndingId.NONE;
        public int Viewers => 1000000;
        public string CameraStatus => FinalChoice ? "PILIHAN AKHIR • KAPASITOR RITUAL 5%" : "PASAR SETAN • " +
            (Phone.currentScanMode == ScanVisualMode.UV ? "UV RAJAH v2" : Phone.currentScanMode == ScanVisualMode.AR ? "LENSA AR" : "KAMERA") + " • SANITY " + Campaign.Data.sanity.ToString("0") + "%";
        public string SafeAnchor => FinalChoice ? "PORTAL" : Completed("P5_ARENA_LOCK") ? "ARENA_WEST" :
            Zone == "S1" || Zone == "S2" || Zone == "S3" ? Zone : Zone == "MIRROR" ? "MIRROR" : Completed("P5_S3_CLEAR") && Zone == "STAGE" ? "STAGE" : "GATE";
        private float nextLiveRetry, nextFinalRetry, clarityUntil;
        private string objective;

        private IEnumerator Start()
        {
            while (GameShell.AwaitingStart || GameShell.MenuSceneLoading) yield return null;
            if (!Campaign.LoadCheckpoint() || !CampaignRouting.IsPhase5(Campaign.Data))
            { GameShell.Instance?.ReturnAfterDeath("Checkpoint Pasar Setan belum tersedia. Save sebelumnya tetap utuh."); yield break; }
            while (!phoneView.Apps || !phoneView.FeedCamera || !player.GetComponent<ShoulderCameraRig>().Avatar) yield return null;
            Phone.ChapterContent = this;
            Phone.hasSmartphoneInInventory = Phone.arModuleUnlocked = Phone.uvModuleUnlocked = Phone.selfieModuleUnlocked = true;
            Phone.isPhoneRaised = Phone.isSelfieCamera = false; Phone.currentScanMode = ScanVisualMode.Normal;
            Phone.RestoreBattery(Campaign.Data.battery); Phone.isScreenOn = Phone.HasCameraPower;
            Phone.isBroadcastConnected = Completed("P5_LIVE_LOCK_1M");
            RestoreWorld(); Ready = true; GetComponent<Phase5Lighting>()?.Apply(); GetComponent<Phase5Lighting>()?.TunePlayerMaterials(player.GetComponent<ShoulderCameraRig>().Avatar);
            if (Campaign.Data.ending != EndingId.NONE) { StartCoroutine(PlayEnding()); yield break; }
            if (FinalChoice) { Phone.isScreenOn = true; Phone.SyncVisualState(); }
            Hud.SetFadeAlpha(0); RefreshObjective();
            if (CanBeginLive) StartLive();
        }
        public bool Receipt(string id, string anchor = null, Action<CampaignData> mutation = null)
        {
            bool ok = Phase5Campaign.Receipt(Campaign, id, anchor, mutation);
            if (ok) { RefreshWorld(); RefreshObjective(); } else SaveError();
            return ok;
        }
        private void Update()
        {
            if (!Ready || Dead || Campaign.Data.ending != EndingId.NONE) return;
            if (!ClockRunning) { boss.Suspend(); replicas.Suspend(); return; }
            float dt = Time.deltaTime; State.elapsed_gameplay_seconds += dt; phoneView.RefreshLensPose();
            if (CanBeginLive && Elapsed >= nextLiveRetry) { nextLiveRetry = Elapsed + 5; StartLive(); }
            if (player.transform.position.y < -8) { Penalty(balance.plankLoss, "Jatuh dari jembatan; kembali ke tambatan terakhir."); MovePlayer(world.Anchor(SafeAnchor)); }
            var input = GameInputConfig.Instance;
            if (input.GetActionDown(GameAction.ModeThermo) && Phone.isPhoneRaised && Phone.isScreenOn) ThermoActive = !ThermoActive;
            if (input.GetActionDown(GameAction.UseIncense) && !ActionBusy) StartCoroutine(BurnIncense());
            if (input.GetActionDown(GameAction.UseFlare) && !FinalChoice && !ActionBusy) boss.DropLight();
            UpdateGaze(dt); TickChat(dt); UpdateScanning(dt); UpdateSectors(dt); replicas.Tick(dt); boss.Tick(dt);
            if (Phase5Ids.CoreCount(Campaign.Data) == 3 && !Completed("P5_FINAL_CHOICE_ENTER") && Elapsed >= nextFinalRetry)
            {
                nextFinalRetry = Elapsed + 5;
                if (Phase5Campaign.EnterFinal(Campaign)) { Phone.isScreenOn = true; Phone.SyncVisualState(); RefreshWorld(); }
                else SaveError();
            }
            if (FinalChoice) TickFinalChoice(dt);
            soundscape.Tick(dt); RefreshObjective();
        }
        public void RestoreWorld()
        {
            ActionBusy = Dead = RecordingEVP = ThermoActive = Analyzer = false;
            UiFocus.Cinematic = UiFocus.Clock = UiFocus.Phone = false; player.MovementLocked = false;
            MovePlayer(world.Anchor(State.safe_anchor)); ResetScanning(); ResetEndingInput();
            boss.Restore(); replicas.Restore(); RefreshWorld();
        }
        public void RefreshWorld()
        {
            if (!Campaign) return;
            world.tape.SetActive(Completed("P5_SECRET_EVP_TRUE") && !Campaign.Data.secret_items.Contains(Phase5Ids.Secret));
            world.drawer.transform.localPosition = world.drawerRest + (Completed("P5_SECRET_AR_DRAWER") ? world.drawerOffset : Vector3.zero);
            world.bridgeFlame.SetActive(!Completed("P5_BRIDGE_TORCH_OFF")); world.bridgeBlocker.SetActive(!Completed("P5_BRIDGE_TORCH_OFF"));
            world.arenaBlocker.SetActive(Completed("P5_ARENA_LOCK"));
            world.sectorBlockers[0].SetActive(!Completed("P5_ENTER_SECTORS"));
            world.gateBackMirror.SetActive(!Completed("P5_ARENA_LOCK"));
            for (int i = 0; i < world.cableVisuals.Length; i++) world.cableVisuals[i].SetActive(!Completed("P5_CABLE_" + (i + 1) + "_CUT"));
            world.cage.SetActive(Phase5Ids.CableCount(Campaign.Data) < 3);
            if (FinalChoice) { world.bossRoot.SetActive(true); world.bossRoot.transform.position = new Vector3(0, -1.5f, 226); }
            if (Completed("P5_DAMAR_AT_PORTAL")) world.damar.transform.position = world.portal.position - Vector3.right * 1.3f;
            else if (FinalChoice) world.damar.transform.position = new Vector3(0, .05f, 220);
            if (FinalChoice) world.damarInteraction.position = world.damar.transform.position + Vector3.up;
            for (int i = 0; i < world.doorLeaves.Length; i++) world.doorLeaves[i].SetActive(!Completed("P5_S1_CLEAR") || i != 1);
        }
        public bool ClearLine(Vector3 from, Vector3 to, Transform target = null)
        {
            int mask = ~((1 << 24) | (1 << 26) | (1 << 29) | (1 << 2) | (1 << 31));
            var delta = to - from;
            foreach (var hit in Physics.RaycastAll(from, delta.normalized, Mathf.Max(0, delta.magnitude - .05f), mask, QueryTriggerInteraction.Ignore))
                if (!hit.transform.IsChildOf(player.transform) && !(target && hit.transform.IsChildOf(target))) return false;
            return true;
        }
        public bool Aim(Transform target, float range, float angle = 25, Transform ignore = null)
        {
            var cam = phoneView.FeedCamera; if (!cam || !target) return false;
            var point = cam.WorldToViewportPoint(target.position); var delta = target.position - cam.transform.position;
            return Vector3.Distance(player.cameraPivot.position, target.position) <= range && point.z > cam.nearClipPlane && point.x >= 0 && point.x <= 1 && point.y >= 0 && point.y <= 1 &&
                Vector3.Angle(cam.transform.forward, delta) <= angle && ClearLine(cam.transform.position, target.position, ignore);
        }
        public void MovePlayer(Transform anchor)
        {
            var capsule = player.GetComponent<CharacterController>(); capsule.enabled = false;
            player.transform.SetPositionAndRotation(anchor.position, anchor.rotation); capsule.enabled = true;
            player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
        public void Penalty(float amount, string feedback)
        {
            Campaign.Data.sanity = Mathf.Max(0, Campaign.Data.sanity - amount);
            Hud.ShowBroadcastNotification(feedback, 4);
        }
        public void Retry(string reason)
        {
            if (Dead || FinalChoice || Campaign.Data.ending != EndingId.NONE) return;
            Dead = true; replicas.Suspend(); boss.Suspend(); player.MovementLocked = true;
            Phone.isPhoneRaised = false; Phone.SyncVisualState(); phoneView.SetViewfinder(false);
            Debug.Log("P5_FAILURE " + reason); GameShell.Instance?.ReturnAfterDeath(reason + " — Continue memulihkan sektor, inti, dan kabel yang sudah tersimpan.");
        }
        public void SaveError() => Hud.ShowBroadcastNotification("Progres belum tersimpan. Aksi belum disahkan; coba lagi setelah aman.", 5);
        private void RefreshObjective()
        {
            if (!Ready) return;
            string text = FinalChoice ? FinalHint : Completed("P5_ARENA_LOCK") ? boss.Status :
                Completed("P5_JOURNAL_CONFIRMED") ? "Identitas terkonfirmasi. Periksa Arsip Saksi dan cermin jalan pulang; aktifkan panggung saat siap." :
                Completed("P5_S3_CLEAR") ? "Damar di kurungan. Pindai Server (foto AR), Kontrak (Thermo + UV), dan Pilar Layar (EVP); konfirmasi Jurnal." :
                Completed("P5_S2_CLEAR") ? "Ruang Jahit: layar mati, jongkok dan putus LOS di balik lemari/meja. Cermin Rias hanya terbuka lewat AR." :
                Completed("P5_S1_CLEAR") ? "Jembatan: bandingkan dua suara EVP, periksa rajah UV. Isi Kendi/Cawan lalu padamkan satu obor penghalang." :
                Completed("P5_ENTER_SECTORS") ? "Koridor Beku: Selfie menahan replika. Thermo membedakan tiga pintu; verifikasi tanda dunia." :
                "Pasar Setan: dinamo dan Altar Doa gratis. Periksa kios dan kursi saksi sebelum memasuki tiga gerbang kenangan.";
            if (objective != text) { objective = text; Hud.SetObjective(text); }
        }
        private void OnDestroy()
        {
            if (Phone && ReferenceEquals(Phone.ChapterContent, this)) Phone.ChapterContent = null;
            CloseOverlay(); if (holdPanel) Destroy(holdPanel);
        }
    }
}
