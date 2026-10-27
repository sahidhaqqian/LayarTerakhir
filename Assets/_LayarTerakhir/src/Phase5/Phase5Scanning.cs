using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
using LayarTerakhir.Smartphone;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed partial class Phase5Director
    {
        public Phase3EvpSource SelectedSource { get; private set; }
        public float EvpProgress { get; private set; }
        public string ThermalReadout { get; private set; } = "+27°C";
        private float thermalDwell, uvDwell, arDwell, drawerDwell, evpHeld, chairSilence, readoutAt;
        private Transform thermalTarget;
        private Phase3EvpSource recordingSource;
        private bool evpWasHeld, evpFinished;
        private void ResetScanning()
        {
            thermalDwell = uvDwell = arDwell = drawerDwell = evpHeld = chairSilence = EvpProgress = 0;
            evpWasHeld = evpFinished = false; thermalTarget = null; recordingSource = SelectedSource = null;
        }
        private void UpdateScanning(float dt)
        {
            var input = GameInputConfig.Instance;
            bool scan = CameraCanScan && !input.GetActionDown(GameAction.ToggleScreen);
            bool rear = scan && !Phone.isSelfieCamera;
            ThermalReadout = "+27°C";
            Transform looking = null;
            if (scan && ThermoActive)
            {
                if (Zone == "S1")
                {
                    for (int i = 0; i < world.doors.Length; i++)
                        if (Aim(world.doors[i], 12, 20, world.doorLeaves[i].transform))
                        { looking = world.doors[i]; ThermalReadout = i == 1 ? "−22°C • BEKU" : "+50°C • PANAS"; break; }
                }
                if (rear && Completed("P5_S3_CLEAR") && Aim(world.contract, balance.opticalRange, 25))
                { looking = world.contract; ThermalReadout = "-- °C • SUHU DI LUAR RENTANG"; }
                if (boss.Phase == 2 && boss.RearCold) ThermalReadout = "−30°C • DI BELAKANG LEHER";
            }
            if (thermalTarget != looking) { thermalTarget = looking; thermalDwell = 0; }
            if (looking) thermalDwell += dt;
            if (thermalDwell >= balance.thermalDwell && Zone == "S1" && looking != null && world.doors.Length > 2 && looking == world.doors[1] && !Completed("P5_S1_THERMO_OK"))
                Receipt("P5_S1_THERMO_OK");
            if (thermalDwell >= balance.thermalDwell && looking == world.contract && !Completed("P5_CONTRACT_THERMO_OK"))
                Receipt("P5_CONTRACT_THERMO_OK", "STAGE");
            bool uv = rear && Phone.currentScanMode == ScanVisualMode.UV && Completed("P5_S3_CLEAR") && Aim(world.contract, balance.opticalRange, 25);
            uvDwell = uv ? uvDwell + dt : 0;
            if (uvDwell >= balance.thermalDwell && !Completed("P5_CONTRACT_UV_OK")) Receipt("P5_CONTRACT_UV_OK", "STAGE");
            if (Completed("P5_CONTRACT_THERMO_OK") && Completed("P5_CONTRACT_UV_OK") && !Campaign.Data.evidence_ids.Contains(Phase5Ids.Contract)) AwardEvidence(Phase5Ids.Contract, false);
            arDwell = rear && Phone.currentScanMode == ScanVisualMode.AR && Completed("P5_S3_CLEAR") && Aim(world.server, balance.opticalRange, 25) ? arDwell + dt : 0;
            bool chairNear = Vector3.Distance(player.transform.position, world.chair.position) < 3;
            if (!Phone.isScreenOn && chairNear && !Completed("P5_SECRET_CHAIR_SILENCE"))
            {
                chairSilence += dt;
                if (chairSilence >= balance.silenceSeconds) Receipt("P5_SECRET_CHAIR_SILENCE");
            }
            else if (!Completed("P5_SECRET_CHAIR_SILENCE")) chairSilence = 0;
            bool drawerSeen = rear && Completed("P5_SECRET_CHAIR_SILENCE") && Phone.currentScanMode == ScanVisualMode.AR && Aim(world.drawerFocus, 6, 30);
            drawerDwell = drawerSeen ? drawerDwell + dt : 0;
            if (drawerDwell >= balance.photoDwell && !Completed("P5_SECRET_AR_DRAWER")) Receipt("P5_SECRET_AR_DRAWER");
            SelectedSource = null; float score = float.MinValue;
            if (rear)
                foreach (var source in world.sources)
                {
                    if (!source || !source.gameObject.activeInHierarchy || !Aim(source.transform, source.range, 35)) continue;
                    float value = source.FocusScore(player.transform.position + Vector3.up);
                    if (value > score) { score = value; SelectedSource = source; }
                }
            bool held = input.GetActionHeld(GameAction.ModeEVP);
            if (held && !evpWasHeld) { evpHeld = EvpProgress = 0; evpFinished = false; recordingSource = null; }
            if (held)
            {
                evpHeld += dt;
                if (evpHeld > .25f && !evpFinished)
                {
                    Analyzer = true;
                    bool valid = rear && SelectedSource && Vector3.Distance(player.transform.position + Vector3.up, SelectedSource.transform.position) <= balance.evpRadius;
                    if (!valid || recordingSource && recordingSource != SelectedSource) { EvpProgress = 0; recordingSource = null; RecordingEVP = false; }
                    else
                    {
                        RecordingEVP = true; recordingSource = SelectedSource; EvpProgress += dt;
                        if (EvpProgress >= balance.evpSeconds)
                        {
                            Hud.EnqueueSubtitle(recordingSource.tag == Phase3VoiceTag.Authentic ? "EVP — ASLI" : "EVP — TIRUAN", recordingSource.subtitle, 7);
                            bool success = true;
                            if (recordingSource.sourceId == "P5_TAPE_TRUE" && recordingSource.tag == Phase3VoiceTag.Authentic && Completed("P5_SECRET_AR_DRAWER")) success = Receipt("P5_SECRET_EVP_TRUE");
                            if (recordingSource.sourceId == "P5_CROWD_TRUE" && recordingSource.tag == Phase3VoiceTag.Authentic && Completed("P5_S3_CLEAR")) success = AwardEvidence(Phase5Ids.Crowd, false);
                            if (success) evpFinished = true;
                            RecordingEVP = false;
                        }
                    }
                }
            }
            if (!held && evpWasHeld)
            {
                if (evpHeld <= .25f) Analyzer = !Analyzer;
                EvpProgress = 0; RecordingEVP = false; recordingSource = null;
            }
            evpWasHeld = held;
            if (Elapsed >= readoutAt)
            {
                readoutAt = Elapsed + .1f;
                string read = "SANITY " + Campaign.Data.sanity.ToString("0") + "% • AIR " + Campaign.Data.phase4.water_units + "/3";
                if (ThermoActive && scan) read += "\nTHERMOKLENIK " + ThermalReadout + (looking && thermalDwell < balance.thermalDwell ? " • STABILKAN BIDIKAN" : "");
                if (Analyzer && scan)
                {
                    read += "\nEVP v2 • " + (SelectedSource ? SelectedSource.tag == Phase3VoiceTag.Authentic ? "ASLI ~──~──" : SelectedSource.tag == Phase3VoiceTag.Mimic ? "TIRUAN /\\/\\/\\" : "LATAR ─────" : "LATAR ─────");
                    if (SelectedSource) read += "\n" + SelectedSource.subtitle;
                    if (RecordingEVP) read += "\nREKAM " + EvpProgress.ToString("0.0") + "/" + balance.evpSeconds.ToString("0") + "s";
                }
                if (Completed("P5_ARENA_LOCK")) read += "\n" + (FinalChoice ? FinalReadout : boss.Status);
                phoneView.ShowChapterReadout(read, Color.white);
            }
        }
        private bool AwardEvidence(string id, bool photograph)
        {
            if (!Phase5Campaign.Evidence(Campaign, id, photograph)) { SaveError(); return false; }
            if (photograph) { Phone.RestoreBattery(Campaign.Data.battery); phoneView.CaptureFlash(); }
            Hud.ShowBroadcastNotification("BUKTI KLENIK TERSIMPAN • " + Phase5Ids.Evidence.Count(Campaign.Data.evidence_ids.Contains) + "/3", 5);
            RefreshObjective(); return true;
        }
        public void Capture()
        {
            var input = GameInputConfig.Instance;
            if (!CameraCanScan || ActionBusy || RecordingEVP || input.GetActionHeld(GameAction.ModeEVP) || input.GetActionDown(GameAction.Interact) ||
                input.GetActionDown(GameAction.ToggleScreen) || input.GetActionDown(GameAction.UseIncense)) return;
            if (FinalChoice) { CaptureEnding(); return; }
            if (boss.Active) { boss.Capture(); return; }
            if (!Phone.isSelfieCamera && Phone.currentScanMode == ScanVisualMode.AR && arDwell >= balance.photoDwell && !Campaign.Data.evidence_ids.Contains(Phase5Ids.Server))
            { AwardEvidence(Phase5Ids.Server, true); return; }
            if (Phone.TryCaptureEnergy(false))
            { phoneView.CaptureFlash(); Hud.ShowBroadcastNotification("Jepretan biasa. Bukti Server perlu AR; Kontrak perlu Thermo dan UV; Pilar Layar direkam dengan EVP.", 4); }
        }
    }
}
