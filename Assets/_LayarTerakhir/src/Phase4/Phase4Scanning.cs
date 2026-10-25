using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
using LayarTerakhir.Smartphone;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public sealed partial class Phase4Director
    {
        public Phase3EvpSource SelectedSource { get; private set; }
        private Phase3EvpSource recordSource;
        private Phase4Torch inspectedTorch;
        private float torchDwell;
        private void UpdateScanning(float dt)
        {
            var input = GameInputConfig.Instance;
            bool rear = CameraCanScan && !Phone.isSelfieCamera && !input.GetActionDown(GameAction.ToggleScreen);
            Temperature = 27;
            bool hot = rear && ThermoActive && Aim(world.thermalTarget, balance.opticalRange, balance.opticalAngle, world.thermalOcclusionRoot);
            thermalDwell = hot ? thermalDwell + dt : 0;
            if (hot) Temperature = 66;
            else if (rear && ThermoActive && ritual.Active)
                foreach (var torch in world.torches)
                    if (Aim(torch.focus, 12, 15, torch.transform)) { Temperature = 60; break; }
            if (thermalDwell >= balance.thermalDwell && !Campaign.Data.evidence_ids.Contains(Phase4Ids.Thermo))
                AwardEvidence(Phase4Ids.Thermo, "NORTH", false);
            arDwell = rear && Phone.currentScanMode == ScanVisualMode.AR && Aim(world.arTarget, balance.opticalRange, balance.opticalAngle, world.arOcclusionRoot, true) ? arDwell + dt : 0;
            bool eye = rear && Phone.currentScanMode == ScanVisualMode.UV && Completed("P4_SECRET_KNOCK_VERIFIED") && Aim(world.secretEye, 6, 25);
            eyeDwell = eye ? eyeDwell + dt : 0;
            if (eyeDwell >= 1 && !Completed("P4_SECRET_EYE_VERIFIED")) Receipt("P4_SECRET_EYE_VERIFIED", "WEST");
            Phase4Torch seenTorch = null;
            if (rear && ritual.Active && Phone.currentScanMode == ScanVisualMode.UV)
                foreach (var t in world.torches) if (Aim(t.focus, balance.opticalRange, 20, t.transform)) { seenTorch = t; break; }
            if (seenTorch != inspectedTorch) { inspectedTorch = seenTorch; torchDwell = 0; }
            if (inspectedTorch) torchDwell += dt; else torchDwell = 0;
            SelectedSource = null; float score = -1000;
            if (rear)
                foreach (var source in world.sources)
                {
                    if (!source || !source.gameObject.activeInHierarchy || !Aim(source.transform, source.range, 35)) continue;
                    float value = source.FocusScore(player.transform.position + Vector3.up);
                    if (value > score) { score = value; SelectedSource = source; }
                }
            bool held = input.GetActionHeld(GameAction.ModeEVP);
            if (held && !evpWasHeld) { evpHeld = EvpProgress = 0; evpFinished = false; recordSource = null; }
            if (held)
            {
                evpHeld += dt;
                if (evpHeld > balance.evpTap && !evpFinished)
                {
                    Analyzer = true;
                    bool valid = rear && SelectedSource && SelectedSource.tag == Phase3VoiceTag.Authentic &&
                        (SelectedSource.sourceId == "WELL_MANTRA" || SelectedSource.sourceId == "WELL_KNOCK") &&
                        Vector3.Distance(player.transform.position + Vector3.up, SelectedSource.transform.position) <= balance.evpRange;
                    if (!valid || recordSource && recordSource != SelectedSource) { RecordingEVP = false; EvpProgress = 0; recordSource = null; }
                    else
                    {
                        RecordingEVP = true; recordSource = SelectedSource; EvpProgress += dt;
                        float need = recordSource.sourceId == "WELL_KNOCK" ? balance.knockSeconds : balance.evpSeconds;
                        if (EvpProgress >= need)
                        {
                            if (recordSource.sourceId == "WELL_KNOCK")
                            {
                                if (Receipt("P4_SECRET_KNOCK_VERIFIED", "WEST"))
                                    Hud.EnqueueSubtitle("EVP — ASLI", "[Tiga ketukan dari relung kering.] Mata yang tertutup menyimpan kesaksian.", 7);
                            }
                            else AwardEvidence(Phase4Ids.Mantra, "WEST", false);
                            evpFinished = true; RecordingEVP = false;
                        }
                    }
                }
            }
            if (!held && evpWasHeld)
            {
                if (evpHeld <= balance.evpTap) Analyzer = !Analyzer;
                EvpProgress = 0; RecordingEVP = false; recordSource = null;
            }
            evpWasHeld = held;
            if (Elapsed >= nextReadout)
            {
                nextReadout = Elapsed + .1f;
                string text = "SANITY " + Campaign.Data.sanity.ToString("0") + "% • AIR " + State.water_units + "/3";
                if (ThermoActive && rear) text += "\nTHERMOKLENIK " + (Temperature > 0 ? "+" : "") + Temperature.ToString("0") + "°C";
                if (Analyzer && rear)
                {
                    string tag = SelectedSource ? SelectedSource.tag == Phase3VoiceTag.Authentic ? "ASLI ~──~──" : SelectedSource.tag == Phase3VoiceTag.Mimic ? "TIRUAN /\\/\\/\\" : "LATAR ─────" : "LATAR ─────";
                    text += "\nEVP v2 • " + tag + (RecordingEVP ? "\nREKAM " + EvpProgress.ToString("0.0") + "s" : "");
                    if (SelectedSource && Vector3.Distance(player.transform.position + Vector3.up, SelectedSource.transform.position) > balance.evpRange)
                        text += "\nDEKATI SUMBER • maksimal " + balance.evpRange.ToString("0.0") + "m";
                }
                if (ritual.Active) text += "\n" + ritual.Status;
                if (inspectedTorch && torchDwell >= balance.uvDwell) text += "\nRAJAH STABIL • periksa bentuk simbol di dunia";
                phoneView.ShowChapterReadout(text, Campaign.Data.sanity <= 40 ? new Color(1, .66f, .45f) : Color.white);
            }
        }
        private bool AwardEvidence(string id, string anchor, bool photo)
        {
            bool ok = Phase4Campaign.Evidence(Campaign, id, anchor, photo);
            if (ok)
            {
                if (photo) { Phone.RestoreBattery(Campaign.Data.battery); phoneView.CaptureFlash(); }
                Hud.ShowBroadcastNotification("BUKTI KLENIK TERSIMPAN • " + Phase4Ids.Evidence.Count(Campaign.Data.evidence_ids.Contains) + "/3", 5);
                RefreshWorld(); RefreshObjective();
            }
            else SaveError();
            return ok;
        }
        public void Capture()
        {
            var input = GameInputConfig.Instance;
            if (!CameraCanScan || ActionBusy || RecordingEVP || input.GetActionHeld(GameAction.ModeEVP) || input.GetActionDown(GameAction.Interact) ||
                input.GetActionDown(GameAction.ToggleScreen) || input.GetActionDown(GameAction.UseIncense)) return;
            if (ritual.Capture()) return;
            if (arDwell >= balance.photoDwell && !Campaign.Data.evidence_ids.Contains(Phase4Ids.Aura))
            { AwardEvidence(Phase4Ids.Aura, "EAST", true); return; }
            if (Phone.TryCaptureEnergy(false))
            { phoneView.CaptureFlash(); Hud.ShowBroadcastNotification("Jepretan biasa — belum ada bukti baru. Bukti altar memerlukan AR dan bidikan stabil.", 3); }
        }
    }
}
