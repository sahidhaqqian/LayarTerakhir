using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase2
{
    public sealed partial class Phase2Director
    {
        private void UpdateThermal(float dt)
        {
            Temperature = CurrentRoom == "MORGUE" || CurrentRoom == "ARCHIVE" || CurrentRoom == "DUTY" ? 12 : 24;
            Phase2ThermalTarget selected = null; float nearest = balance.thermalAngle;
            if (ThermoActive && CameraCanScan)
            {
                foreach (var target in world.thermalTargets)
                {
                    if (!target || !target.gameObject.activeInHierarchy || Phone.isSelfieCamera) continue;
                    Vector3 point = target.transform.position;
                    float angle = Vector3.Angle(phoneView.FeedCamera.transform.forward, point-phoneView.FeedCamera.transform.position);
                    if (angle <= nearest && Aim(point,target.range,balance.thermalAngle)) { selected = target; nearest = angle; }
                }
                if (Phone.isSelfieCamera && lastri.Attached && Aim(lastri.Head,3,balance.thermalAngle)) Temperature = -20;
            }
            if (selected != thermalTarget) { thermalTarget = selected; thermalDwell = 0; }
            if (!selected) return;
            Temperature = selected.temperature; thermalDwell += dt;
            if (selected.id == "POLI03" && thermalDwell >= balance.poliDwell) AddEvidence(Phase2Ids.Freeze,"P2_EVIDENCE_FREEZE");
            if (selected.id == "C3" && thermalDwell >= balance.drawerDwell && !Completed("P2_DRAWER_CHAIN_BREAK"))
            {
                if (Commit("P2_DRAWER_CHAIN_BREAK", null, "PHASE2_CP4", SafeAnchor))
                    Hud.ShowBroadcastNotification("C3 • −24°C. Rantai beku meretak. Pembacaan tersimpan sebagai petunjuk.", 5);
            }
        }
        private bool AddEvidence(string id, string receipt)
        {
            if (Campaign.Data.evidence_ids.Contains(id)) return true;
            bool result = Commit(receipt, d => { if (!d.evidence_ids.Contains(id)) d.evidence_ids.Add(id); }, "PHASE2_CP4", SafeAnchor);
            if (result) Hud.ShowBroadcastNotification("BUKTI KLENIK TERSIMPAN • " + Phase2Ids.EvidenceCount(Campaign.Data) + "/3", 4);
            return result;
        }
        private void UpdateAR(float dt)
        {
            bool ar = CameraCanScan && !Phone.isSelfieCamera && Phone.currentScanMode == ScanVisualMode.AR;
            auraDwell = ar && Completed("P2_C3_OPEN") && Aim(world.auraTarget.position,3,22) ? auraDwell+dt : 0;
            if (!Completed("P2_CROSS_INKUBATOR_WALL"))
            {
                bool aimed = ar && Aim(world.wallTarget.position,5,28,world.inkubatorWall.transform);
                wallDwell = aimed ? wallDwell+dt : 0;
                if (wallDwell >= .5f) { wallRevealed = true; wallGrace = 3; }
                else wallGrace = Mathf.Max(0,wallGrace-dt);
                bool occupied = Vector3.Distance(player.transform.position+Vector3.up,world.wallTarget.position) < 2;
                bool open = wallRevealed && (wallGrace > 0 || occupied || CurrentRoom == "INKUBATOR");
                world.inkubatorWall.SetActive(!open);
                if (CurrentRoom == "INKUBATOR" && wallRevealed)
                    Commit("P2_CROSS_INKUBATOR_WALL",null,"PHASE2_CP4","NURSE");
            }
            if (!Completed("P2_SAFE_ECHO_SEEN"))
            {
                safeEchoDwell = ar && Aim(world.safeEchoTarget.position,3,25) ? safeEchoDwell+dt : 0;
                if (safeEchoDwell >= 1.5f && Commit("P2_SAFE_ECHO_SEEN",null,"PHASE2_CP4",SafeAnchor))
                { Hud.EnqueueSubtitle("Gema 2001", "Suster menekan tombol: 2 — 6 — 0 — 4.",6); soundscape?.Cue("Echo",world.safeEchoTarget.position); }
            }
        }
        private void UpdateEVP(float dt)
        {
            if (GameInputConfig.Instance.GetActionDown(GameAction.ModeEVP))
            {
                if (RecordingEVP) { RecordingEVP = false; EvpProgress = 0; return; }
                if (CameraCanScan && !Phone.isSelfieCamera) { RecordingEVP = true; EvpProgress = 0; }
            }
            if (!RecordingEVP) return;
            if (!CameraCanScan || Phone.isSelfieCamera || !Aim(world.evpTarget.position,balance.evpRange,25))
            { RecordingEVP = false; EvpProgress = 0; return; }
            EvpProgress += dt;
            if (EvpProgress < balance.evpSeconds) return;
            RecordingEVP = false; EvpProgress = 0;
            if (AddEvidence(Phase2Ids.EVP,"P2_EVIDENCE_EVP"))
            { Hud.EnqueueSubtitle("EVP — Tidak Dikenal",Phase2Content.EVP,8); soundscape?.Cue("EVP",world.evpTarget.position); }
        }
        public void Capture()
        {
            if (!CameraCanScan) return;
            if (auraDwell >= balance.auraDwell && !Campaign.Data.evidence_ids.Contains(Phase2Ids.Aura))
            {
                // Evidence mutation and the one photo debit share the same disk transaction.
                // No shutter success or energy loss is shown if persistence fails.
                float cost = Phone.photoCostPercent;
                if (Phone.batteryPercent < cost) { Hud.ShowBroadcastNotification("Daya belum cukup untuk menyimpan foto. Cari dinamo.",3); return; }
                float after = Phone.batteryPercent-cost;
                if (Commit("P2_EVIDENCE_AURA",d =>
                { d.evidence_ids.Add(Phase2Ids.Aura); d.battery=after; },"PHASE2_CP4",SafeAnchor))
                { Phone.RestoreBattery(after); phoneView.CaptureFlash(); Hud.ShowBroadcastNotification("BUKTI 2 • Residu aura tumbal tersimpan.",4); }
                return;
            }
            if (Phone.TryCaptureEnergy(false))
            { phoneView.CaptureFlash(); Hud.ShowBroadcastNotification(lastri.Attached ? "Lastri belum bisa disegel. Cari tiga bukti dan tiga pusaka." : "Foto biasa. Bukti aura memerlukan AR dan bidikan stabil.",3); }
        }
    }
}
