using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase2
{
    [DefaultExecutionOrder(-15)]
    public sealed partial class Phase2Director : MonoBehaviour, IPhoneChapterContent, IContextInteractionResolver
    {
        public FirstPersonController player;
        public Phase2World world;
        public Phase2Balance balance;
        public AtmajaPhoneView phoneView;
        public Phase2Lastri lastri;
        public Phase2Soundscape soundscape;
        public Phase2Boss boss;
        public bool VideoPlaying { get; private set; }
        public bool StoryPlaying { get; private set; }
        public bool Ready { get; private set; }
        public bool Dead { get; private set; }
        public bool Hidden { get; private set; }
        public bool ThermoActive { get; private set; }
        public bool RecordingEVP { get; private set; }
        public float EvpProgress { get; private set; }
        public int Temperature { get; private set; } = 24;
        public string ThermalTargetId => thermalTarget ? thermalTarget.id : "";
        public bool CameraLocked => false; // F / hiding remains available during every investigation attach.
        public bool CanBeginLive => Ready && !Dead && Phone.batteryPercent > 0 && !Completed("P2_START_LIVE");
        public int Viewers => Completed("P2_BOSS_WIN") ? 310500 : 42300;
        public CampaignState Campaign => CampaignState.Instance;
        public SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        public string CurrentRoom => world.Room(player.transform.position);
        public bool IsQuiet => CurrentRoom == "INKUBATOR" || CurrentRoom == "NURSE" ||
            CurrentRoom == "DUTY" && world.dutyDoor && !world.dutyDoor.IsOpen && !lastri.Attached;
        public bool ClockRunning => Ready && !Dead && !UiFocus.HardModal && !UiFocus.Cinematic && !GameShell.IsPaused && !Hud.IsModalOpen && Time.timeScale > 0;
        public bool CameraCanScan => ClockRunning && Phone.hasSmartphoneInInventory && Phone.isPhoneRaised && Phone.isScreenOn &&
            Phone.batteryPercent > 0 && phoneView && phoneView.IsCameraApp && (!UiFocus.Phone || phoneView.ViewfinderActive) && !Hidden;
        public string CameraStatus => RecordingEVP ? $"EVP • {EvpProgress:0.0}/{balance.evpSeconds:0}s" :
            (Phone.isSelfieCamera ? "LENSA DEPAN / SELFIE" : "LENSA BELAKANG") +
            (ThermoActive ? $" • THERMOKLENIK {Temperature}°C" : "") + (lastri.Attached ? $" • LASTRI {lastri.TouchMeter:0}% / MAJU {lastri.HoldProgress:0.0}s" : "");
        public bool OwnsInteraction => Hidden;
        private Phase2WorldItem hidingItem;
        private Vector3 beforeHide;
        private Quaternion beforeHideRotation;
        private Phase2ThermalTarget thermalTarget;
        private float thermalDwell, auraDwell, wallDwell, wallGrace, safeEchoDwell, gasUntil;
        private string previousRoom, objective;
        private bool wallRevealed, charging;
        private float originalWalk, originalRun, originalCrouch;

        private IEnumerator Start()
        {
            while (GameShell.AwaitingStart || GameShell.MenuSceneLoading) yield return null;
            if (!Campaign.LoadCheckpoint() || !CampaignRouting.IsPhase2(Campaign.Data) || !Completed("P2_DEPART_KOS"))
            { GameShell.Instance?.ReturnAfterDeath("Checkpoint RSUD belum tersedia. Save sebelumnya tidak diubah."); yield break; }
            Phone.ChapterContent = this; player.GetComponent<InteractionInteractor>().ContextResolver = this;
            Phone.hasSmartphoneInInventory = Phone.arModuleUnlocked = Phone.selfieModuleUnlocked = true;
            Phone.batteryPercent = Campaign.Data.battery; Phone.isScreenOn = Phone.batteryPercent > 0;
            Phone.isPhoneRaised = false; Phone.isSelfieCamera = false; Phone.currentScanMode = ScanVisualMode.Normal;
            Phone.isBroadcastConnected = Completed("P2_START_LIVE") || Completed("P2_EPILOGUE_CHAT"); Phone.SyncVisualState();
            originalWalk = player.walkSpeed; originalRun = player.sprintSpeed; originalCrouch = player.crouchSpeed;
            RestoreWorld(); Ready = true; Hud.SetFadeAlpha(0);
            if(!Completed("P2_ARRIVE_IGD")&&Commit("P2_ARRIVE_IGD"))
                Hud.EnqueueSubtitle("Raka", "RSUD Sido Waras. 23:50. Pintu IGD masih terbuka...", 5);
            RefreshObjective();
        }
        public bool Completed(string id) => Campaign && Campaign.Data.phase2.Completed(id);
        public bool Commit(string id, Action<CampaignData> mutate = null, string checkpoint = null, string anchor = null)
        {
            if (Completed(id)) return true;
            string cp = Phase2Campaign.InvestigationCheckpoint(Campaign.Data, checkpoint);
            bool result = Campaign.TryCommit(cp, d =>
            {
                d.phase2.Add(id); mutate?.Invoke(d);
                if (anchor != null && !d.phase2.Completed("P2_BOSS_WIN")) d.phase2.safe_anchor = cp=="PHASE2_CP5" ? "OR_PREP" : anchor;
            });
            if (!result) Hud.ShowBroadcastNotification("Penyimpanan gagal. Progres belum berubah; coba lagi.", 5);
            else { RefreshWorldItems(); RefreshObjective(); }
            return result;
        }
        public void RestoreWorld()
        {
            Dead = Hidden = ThermoActive = RecordingEVP = wallRevealed = false;
            thermalDwell = auraDwell = wallGrace = safeEchoDwell = gasUntil = EvpProgress = 0; thermalTarget = null;
            UiFocus.Cinematic = UiFocus.Phone = UiFocus.Clock = false; player.MovementLocked = false;
            player.GetComponent<ShoulderCameraRig>().FirstPersonOverride = false;
            MovePlayer(world.Anchor(Campaign.Data.phase2.safe_anchor));
            foreach (var drawer in world.drawers) drawer.Restore();
            if (world.dutyDoor) world.dutyDoor.SetOpenImmediate(true);
            if (world.safeDoor) world.safeDoor.SetOpenImmediate(Completed("P2_NURSE_SAFE_OPEN"));
            foreach (var light in world.corridorLights) if (light) light.enabled = true;
            Phone.RestoreBattery(Campaign.Data.battery);
            RefreshWorldItems(); lastri.Restore(); boss.Restore(); previousRoom = "";
            if (Completed("P2_COMPLETE")) ShowPhase3Handoff();
        }
        private void RefreshWorldItems()
        {
            world.basementDoor.SetActive(!Completed("P2_KEYPAD_1603"));
            world.stairGrille.SetActive(!Completed("P2_STAIR_L2_UNLOCK"));
            bool crossed = Completed("P2_CROSS_INKUBATOR_WALL");
            bool temporaryOpening=wallRevealed&&(wallGrace>0||CurrentRoom=="INKUBATOR"||Vector3.Distance(player.transform.position+Vector3.up,world.wallTarget.position)<2);
            world.inkubatorWall.SetActive(!crossed&&!temporaryOpening); world.inkubatorVeil.SetActive(!crossed);
            bool opened = Completed("P2_C3_OPEN");
            world.c3Contents.SetActive(opened);
            world.c3Chain.SetActive(!Completed("P2_DRAWER_CHAIN_BREAK"));
            world.gunting.SetActive(opened && !Completed("P2_TAKE_GUNTING"));
            world.aura.SetActive(opened); world.gelang.SetActive(!Completed("P2_TAKE_GELANG"));
            world.blood.SetActive(Completed("P2_NURSE_SAFE_OPEN") && !Completed("P2_TAKE_BOTOL"));
            if(world.safeDoor)world.safeDoor.SetOpenAnimated(Completed("P2_NURSE_SAFE_OPEN"));
            world.safePanel.SetActive(!Completed("P2_NURSE_SAFE_OPEN"));
            world.secret.SetActive(!Campaign.Data.secret_items.Contains(Phase2Ids.Secret));
            bool blocked = !Completed("P2_BOSS_WIN") && !Phase2Ids.AllRelics(Campaign.Data);
            world.operationFog.SetActive(blocked);
            var gate = world.operationFog.GetComponent<Collider>(); if (gate) gate.enabled = blocked;
            if(world.operationDoor)world.operationDoor.SetOpenAnimated(!blocked && !boss.Active);
        }
        private void Update()
        {
            if(Ready&&!Dead&&player.transform.position.y< -10)
            {
                if(boss.Active){boss.Abort();Campaign.LoadCheckpoint();}
                RestoreWorld();Hud.ShowBroadcastNotification("Kembali ke titik aman terakhir. Progres dan daya HP tetap tersimpan.",4);return;
            }
            if (!ClockRunning) { if(Ready&&lastri)lastri.SuspendClock();if(Ready&&boss)boss.Suspend(); return; }
            float dt = Time.deltaTime; Campaign.Data.phase2.elapsed_gameplay_seconds += dt;
            phoneView.RefreshLensPose();
            string room = CurrentRoom;
            if (room != previousRoom)
            {
                if (room == "MORGUE" && !Completed("P2_MORGUE_ENTER") &&
                    !Commit("P2_MORGUE_ENTER", null, "PHASE2_CP3", "BASEMENT")) return;
                if(room=="INKUBATOR"&&!Completed("P2_EVP_DISCOVERED")&&Commit("P2_EVP_DISCOVERED"))
                { Hud.ShowBroadcastNotification("Tangis bertumpuk dari enam ranjang. Cari sumber dengan kamera, lalu tahan rekaman EVP: "+GameInputConfig.Instance.GetActionLabel(GameAction.ModeEVP),7); }
                if(room=="L2"&&!Completed("P2_L2_ARRIVE"))Commit("P2_L2_ARRIVE",null,null,"NURSE");
                previousRoom = room;
            }
            if (boss.Active) boss.Tick(dt);
            else if (!Completed("P2_BOSS_WIN")) lastri.Tick(dt); // Fatal deadlines resolve before ordinary E.
            if (Completed("P2_BOSS_WIN") && !Completed("P2_EPILOGUE") && !StoryPlaying && !storyRetryRequired && Phone.batteryPercent > 0) StartCoroutine(Epilogue());
            if (boss.Active && GameInputConfig.Instance.GetActionDown(GameAction.UseFlare)) boss.DropFlare();
            if (Dead) return;
            if (!Completed("P2_BOSS_WIN")) foreach (var drawer in world.drawers) drawer.Tick(dt);
            float movement = GasActive ? balance.formalinMoveMultiplier : 1;
            player.walkSpeed = originalWalk * movement; player.sprintSpeed = originalRun * movement; player.crouchSpeed = originalCrouch * movement;
            if (GameInputConfig.Instance.GetActionDown(GameAction.ModeThermo) && Phone.isPhoneRaised && Phone.isScreenOn)
            { ThermoActive = !ThermoActive; phoneView.Apps.Open(PhoneApp.Camera); }
            if(ThermoActive&&!Completed("P2_THERMO_TUTORIAL"))Commit("P2_THERMO_TUTORIAL");
            UpdateThermal(dt); UpdateAR(dt); UpdateEVP(dt); RefreshObjective();
            if(!lastri.Attached && !boss.Active)phoneView.ShowChapterReadout(ThermoActive?$"THERMOKLENIK  {Temperature}°C"+(thermalTarget?"\n"+thermalTarget.id:""):RecordingEVP?$"EVP {EvpProgress:0.0}/{balance.evpSeconds:0}s":"",Color.white);
            soundscape?.Tick(dt);
        }
        public bool GasActive => Campaign && Campaign.Data.phase2.elapsed_gameplay_seconds < gasUntil;
        public void ApplyFormalin(Vector3 at)
        {
            if (Completed("P2_BOSS_WIN") || Vector3.Distance(at, player.transform.position + Vector3.up) > balance.formalinRadius) return;
            gasUntil = Campaign.Data.phase2.elapsed_gameplay_seconds + balance.formalinSeconds;
            Hud.ShowBroadcastNotification("FORMALIN — menjauh dari laci. Napas tersendat; bukan luka fatal.", 5);
            lastri.CallFromFormalin(); phoneView.Glitch(balance.formalinSeconds,.35f); soundscape?.Cue("Gas", at);
        }
        public bool MorgueCleared => Campaign.HasInventoryItem(Phase2Ids.Gunting) &&
            Campaign.Data.evidence_ids.Contains(Phase2Ids.Aura) && Completed("P2_AUTOPSY_REPORT");
        public string SafeAnchor => CurrentRoom == "INKUBATOR" || CurrentRoom == "NURSE" || CurrentRoom == "L2" ? "NURSE" :
            CurrentRoom == "MORGUE" || CurrentRoom == "ARCHIVE" || CurrentRoom == "DUTY" ? "BASEMENT" : "IGD";
        public bool ClearLine(Vector3 from, Vector3 to, Transform target = null)
        {
            Vector3 delta = to - from;
            foreach (var hit in Physics.RaycastAll(from, delta.normalized, Mathf.Max(0, delta.magnitude-.035f),
                ~((1 << 26) | (1 << 29) | (1 << 25) | (1 << 2) | (1 << 31)), QueryTriggerInteraction.Ignore))
            {
                if (hit.transform.IsChildOf(player.transform) || target && hit.transform.IsChildOf(target)) continue;
                return false;
            }
            return true;
        }
        public bool Aim(Vector3 point, float range, float angle = 20, Transform target = null)
        {
            if (!phoneView.FeedCamera) return false;
            var cam = phoneView.FeedCamera; Vector3 delta = point - cam.transform.position, v = cam.WorldToViewportPoint(point);
            return delta.magnitude <= range && v.z > cam.nearClipPlane && v.x >= 0 && v.x <= 1 && v.y >= 0 && v.y <= 1 &&
                Vector3.Angle(cam.transform.forward, delta) <= angle && ClearLine(cam.transform.position, point, target);
        }
        public void MovePlayer(Transform anchor)
        {
            var capsule = player.GetComponent<CharacterController>(); capsule.enabled = false;
            player.transform.SetPositionAndRotation(anchor.position,anchor.rotation); capsule.enabled = true;
            player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
        private void RefreshObjective()
        {
            if (!Ready) return;
            string next = Completed("P2_COMPLETE") ? "Babak 2 selesai. Tujuan berikutnya: " + Phase2Ids.Destination :
                Completed("P2_EPILOGUE") ? "Tinggalkan RSUD lewat motor di pelataran IGD. Opsional: kembali ke arsip basement." :
                Completed("P2_BOSS_WIN") ? "Segel kedua tersimpan. Isi daya manual di generator bila HP mati; periksa pesan Damar." :
                boss.Active ? boss.Status : !Campaign.Data.evidence_ids.Contains(Phase2Ids.Freeze) ? "Baca Buku Jaga di IGD. Cari poli kandungan dengan ThermoKlenik." :
                !Completed("P2_KEYPAD_1603") ? "Gabungkan suhu tanpa minus dan nomor poli. Buka panel pintu basement." :
                !Completed("P2_AUTOPSY_REPORT") ? "Turun ke kamar mayat. Baca laporan di meja otopsi; pantau belakang lewat Selfie." :
                !MorgueCleared ? "Cari laci lebih dingin dari −20°C. Ambil pusaka dan foto aura dengan AR." :
                !Completed("P2_STAIR_L2_UNLOCK") ? "Gunakan kunci dari laporan otopsi untuk membuka teralis tangga lantai dua." :
                !Campaign.Data.evidence_ids.Contains(Phase2Ids.EVP) ? "Bangsal Bersalin: tembus bukaan Inkubator dengan AR. Rekam EVP dari sumber yang tepat." :
                !Phase2Ids.AllRelics(Campaign.Data) ? "Lengkapi tiga pusaka. Periksa Inkubator 04 dan brankas Ruang Jaga Suster." :
                !Completed("P2_JOURNAL_CONFIRMED") ? "Tiga bukti sudah lengkap. Konfirmasikan identitas secara sadar di Jurnal Klenik." :
                "Investigasi lengkap. Menuju pintu Ruang Operasi Bersalin Utama.";
            if (next == objective) return; objective = next; Hud.SetObjective(next);
        }
        public void Die(string cause)
        {
            if (Dead) return; Debug.Log("P2_FAILURE "+cause); Dead = true; RecordingEVP = false; player.MovementLocked = true;
            boss.Abort();
            Phone.isPhoneRaised = false; Phone.SyncVisualState(); phoneView.SetViewfinder(false);
            Hud.SetFadeAlpha(1); GameShell.Instance?.ReturnAfterDeath(cause + " — Continue memuat checkpoint terakhir, tanpa mengisi baterai.");
        }
        private void OnDestroy()
        {
            if(boss)boss.Abort();
            if (Phone && ReferenceEquals(Phone.ChapterContent,this)) Phone.ChapterContent = null;
            if (player) { player.walkSpeed=originalWalk; player.sprintSpeed=originalRun; player.crouchSpeed=originalCrouch; }
            if (overlay) RemoveOverlay();
        }
    }
}
