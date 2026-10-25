using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed partial class Phase5Director
    {
        private int syncStage;
        private float syncTime, badAimTime, normalTime, escortSaveRetry, endingSaveRetry;
        private bool archiveMode, escortActive, endingPlaying;
        private float previousWalk, previousSprint, previousCrouch;
        private Vector3 escortVelocity;
        public string FinalReadout => archiveMode ? "MODE ARSIP SAKSI • REKAM CERMIN BERBIDIKAN EMAS" :
            "KAPASITOR RITUAL 5% • " + (badAimTime >= balance.badDwell ? "TUMBAL — BIDIKAN KONTRAK PADA DAMAR" : syncStage == 5 ? "SINKRONISASI 100% — BIDIK INTI DI SELFIE" :
            syncStage > 0 ? "SINKRONISASI " + Mathf.RoundToInt((syncStage == 1 ? 0 : syncStage == 2 ? syncTime / balance.syncSegment * .5f : syncStage == 3 ? .5f : .5f + syncTime / balance.syncSegment * .5f) * 100) + "%" : "TIGA AKSI DASAR TERSEDIA • BACA JURNAL");
        private string FinalHint => archiveMode ? "Kesaksian siap. Jepret Cermin Saksi berbidikan emas; Damar bukan target dalam Mode Arsip." :
            Completed("P5_DAMAR_AT_PORTAL") ? "Damar di portal. Selfie → AR stabil → UV stabil → Blitz inti, atau pilih pengorbanan di tepi panggung. Jurnal menjelaskan semua aksi." :
            "Pilihan akhir tanpa batas waktu. Papah Damar ke portal, bidik kontrak pada Damar, atau putus siaran dari tepi panggung. Baca konsekuensi di Jurnal.";
        private void ResetEndingInput()
        { syncStage = 0; syncTime = badAimTime = normalTime = 0; archiveMode = escortActive = false; }
        private bool NearPortal => Vector3.Distance(player.transform.position, world.portal.position) <= 3.5f;
        private bool NearEdge => Vector3.Distance(player.transform.position, world.edge.position) <= 3.5f;
        private void TickFinalChoice(float dt)
        {
            var input = GameInputConfig.Instance;
            if (escortActive)
            {
                Vector3 desired = player.transform.position - player.transform.right * .62f - player.transform.forward * .08f;
                world.damar.transform.position = Vector3.SmoothDamp(world.damar.transform.position, desired, ref escortVelocity, .16f);
                world.damarInteraction.position = world.damar.transform.position + Vector3.up;
                world.damar.transform.rotation = Quaternion.Slerp(world.damar.transform.rotation, player.transform.rotation, dt * 5);
                if (NearPortal && Elapsed >= escortSaveRetry)
                {
                    escortSaveRetry = Elapsed + 5;
                    if (Phase5Campaign.HelpDamar(Campaign)) { StopEscort(); RefreshWorld(); Hud.EnqueueSubtitle("Damar", "Gue di sini, Ka. Kali ini kita pulang bareng.", 6); }
                    else SaveError();
                }
            }
            bool bad = !archiveMode && CameraCanScan && !Phone.isSelfieCamera && Aim(world.damarFocus, 7, 18, world.damar.transform);
            badAimTime = bad ? badAimTime + dt : 0;
            if (badAimTime >= balance.badDwell) phoneView.ShowChapterTarget(world.damarFocus.position, true);
            if (NearEdge)
            {
                float beforeHold = normalTime;
                normalTime = input.GetActionHeld(GameAction.ToggleScreen) ? normalTime + dt : 0;
                if (normalTime > 0) Hud.SetObjective("PUTUS SIARAN • " + Mathf.Clamp01(normalTime / balance.normalHold).ToString("P0") + " • lepas untuk batal. Raka tertinggal; Damar dan penonton selamat.");
                else if (beforeHold > 0) objective = null;
                if (normalTime >= balance.normalHold) { TryChooseEnding(EndingId.NORMAL); return; }
            }
            else { if (normalTime > 0) objective = null; normalTime = 0; }
            if (archiveMode) { phoneView.ShowChapterTarget(world.archiveMirror.position, true); return; }
            if (!Completed("P5_DAMAR_AT_PORTAL") || !NearPortal) { syncStage = 0; syncTime = 0; return; }
            if (input.GetActionDown(GameAction.ModeSelfie))
            {
                syncStage = 1; syncTime = 0; return;
            }
            if (input.GetActionDown(GameAction.ModeAR))
            {
                if (syncStage == 1 && Phone.isSelfieCamera) { syncStage = 2; syncTime = 0; }
                else { syncStage = 0; syncTime = 0; }
                return;
            }
            if (input.GetActionDown(GameAction.ModeUV))
            {
                if (syncStage == 3 && Phone.isSelfieCamera) { syncStage = 4; syncTime = 0; }
                else { syncStage = 0; syncTime = 0; }
                return;
            }
            if (!CameraCanScan || !Phone.isSelfieCamera) { syncStage = 0; syncTime = 0; return; }
            if (syncStage == 2 && Phone.currentScanMode == ScanVisualMode.AR || syncStage == 4 && Phone.currentScanMode == ScanVisualMode.UV)
            {
                syncTime += dt;
                if (syncTime >= balance.syncSegment) { syncStage++; syncTime = 0; }
            }
            if (syncStage == 5) phoneView.ShowChapterTarget(world.bossHead.position, true);
        }
        private void CaptureEnding()
        {
            if (!FinalChoice) return;
            if (archiveMode)
            {
                if (Aim(world.archiveMirror, 8, 24)) TryChooseEnding(EndingId.SECRET);
                else Hud.ShowBroadcastNotification("Mode Arsip hanya merekam Cermin Saksi. Damar bukan target sah.", 4);
                return;
            }
            if (!Phone.isSelfieCamera && badAimTime >= balance.badDwell + balance.badLabelMinimum && Aim(world.damarFocus, 7, 18, world.damar.transform))
            { TryChooseEnding(EndingId.BAD); return; }
            if (Phone.isSelfieCamera && syncStage == 5 && Completed("P5_DAMAR_AT_PORTAL") && NearPortal && Aim(world.bossHead, 22, 30, world.bossRoot.transform))
            { TryChooseEnding(EndingId.TRUE); return; }
            Hud.ShowBroadcastNotification("Belum ada aksi akhir yang disahkan. Baca Jurnal; kapasitor tetap utuh.", 4);
        }
        private void ToggleEscort()
        {
            if (!FinalChoice || Completed("P5_DAMAR_AT_PORTAL")) return;
            if (escortActive) { StopEscort(); return; }
            escortActive = true; previousWalk = player.walkSpeed; previousSprint = player.sprintSpeed; previousCrouch = player.crouchSpeed;
            player.walkSpeed = player.sprintSpeed = balance.escortSpeed; player.crouchSpeed = Mathf.Min(previousCrouch, balance.escortSpeed);
            Hud.EnqueueSubtitle("Raka", "Pegang bahu gue, Dam. Kita ke cermin itu.", 5);
        }
        private void StopEscort()
        {
            if (!escortActive) return;
            escortActive = false; player.walkSpeed = previousWalk; player.sprintSpeed = previousSprint; player.crouchSpeed = previousCrouch;
        }
        private void InteractArchive(Phase5WorldItem item)
        {
            if (archiveMode) { archiveMode = false; syncStage = 0; Hud.ShowBroadcastNotification("Mode Arsip dibatalkan. Pilihan akhir tetap terbuka.", 4); return; }
            if (!Phase5Ids.ArchiveComplete(Campaign.Data)) { Hud.ShowBroadcastNotification(ArchiveStatus(), 8); return; }
            if (!Campaign.Data.archive_prepared && !Phase5Campaign.ValidateArchive(Campaign)) { SaveError(); return; }
            if (!FinalChoice) { Hud.ShowBroadcastNotification("LIMA ARSIP TERVERIFIKASI. Persiapan tidak memilih ending; keputusan tetap di tanganmu setelah Damar bebas.", 7); return; }
            if (!Completed("P5_DAMAR_AT_PORTAL")) { Hud.ShowBroadcastNotification("Bantu Damar ke portal sebelum memutar seluruh kesaksian.", 5); return; }
            StartCoroutine(HoldWorld(item, balance.archiveHold, "PUTAR LIMA KESAKSIAN", () =>
            {
                archiveMode = true; syncStage = 0; badAimTime = 0;
                Phone.isPhoneRaised = Phone.isScreenOn = true; Phone.isSelfieCamera = false; Phone.SyncVisualState(); phoneView.Apps.Open(PhoneApp.Camera);
                Hud.EnqueueSubtitle("Arsip Saksi", "Tari. Lastri dan anaknya. Sembilan anak buruh. Warga Karang Setra. Saksi tidak membeli korban.", 9);
                Hud.ShowBroadcastNotification("MODE ARSIP • arahkan ke Cermin Saksi dan jepret untuk merekam kesaksian.", 6); return true;
            }));
        }
        private bool TryChooseEnding(EndingId ending)
        {
            if (Elapsed < endingSaveRetry) return false;
            if (!Phase5Campaign.LockEnding(Campaign, ending)) { endingSaveRetry = Elapsed + 1; SaveError(); return false; }
            StopEscort(); StartCoroutine(PlayEnding()); return true;
        }
        private IEnumerator PlayEnding()
        {
            if (endingPlaying) yield break;
            endingPlaying = true; StopEscort(); boss.Suspend(); replicas.Suspend(); player.MovementLocked = true;
            UiFocus.Cinematic = true; Phone.isPhoneRaised = false; Phone.SyncVisualState(); phoneView.SetViewfinder(false);
            var rig = player.GetComponent<LayarTerakhir.Player.ShoulderCameraRig>(); rig.CinematicVisible = true;
            var ending = Campaign.Data.ending;
            if (!Completed("P5_COMPLETE"))
            {
                yield return PlayEndingShots(ending);
                while (!Phase5Campaign.Complete(Campaign))
                {
                    SaveError(); Hud.EnqueueSubtitle("Sistem", "Ending belum tersimpan. Kosongkan ruang penyimpanan, lalu tekan interaksi untuk mencoba lagi.", 8);
                    while (!GameInputConfig.Instance.GetActionDown(GameAction.Interact)) yield return null;
                    yield return null;
                }
            }
            ShowCredits(ending);
        }
    }
}
