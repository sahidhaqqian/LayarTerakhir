using System;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.UI;
namespace LayarTerakhir.Phase5
{
    public sealed partial class Phase5Director
    {
        private GameObject holdPanel;
        public bool CanInteract(Phase5WorldItem item)
        {
            if (!Ready || Dead || ActionBusy || RecordingEVP || UiFocus.HardModal || UiFocus.Cinematic || Hud.IsModalOpen || Campaign.Data.ending != EndingId.NONE ||
                GameInputConfig.Instance.GetActionHeld(GameAction.ModeEVP) || GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen)) return false;
            if (item.id.StartsWith("CABLE_")) return boss.Phase == 3 && CameraCanScan && !Phone.isSelfieCamera && Phone.currentScanMode == ScanVisualMode.AR && !Completed("P5_CABLE_" + item.id.Substring(6) + "_CUT");
            if (item.id == "TAPE") return Completed("P5_SECRET_EVP_TRUE") && !Campaign.Data.secret_items.Contains(Phase5Ids.Secret);
            if (item.id == "BACK_MIRROR" || item.id == "GATE_RETURN") return !Completed("P5_ARENA_LOCK");
            return true;
        }
        public string Prompt(Phase5WorldItem item)
        {
            if (item.id == "POUR") return "Siram Obor Santet • air " + Campaign.Data.phase4.water_units + "/3";
            if (item.id == "DAMAR" && FinalChoice) return Completed("P5_DAMAR_AT_PORTAL") ? "Damar aman di portal" : escortActive ? "Lepaskan Damar sebentar" : "Papah Damar ke portal cermin";
            if (item.id == "ARCHIVE") return "Cermin Saksi • Arsip " + Phase5Ids.ArchiveCount(Campaign.Data) + "/5" + (FinalChoice ? " • tahan untuk kesaksian" : " • validasi");
            return item.prompt;
        }
        public void Interact(Phase5WorldItem item)
        {
            if (!CanInteract(item)) return;
            string id = item.id;
            if (id.StartsWith("DYNAMO_")) { StartCoroutine(HoldWorld(item, balance.chargeHold, "ENGKOL DINAMO", () => {
                bool ok = Phase5Campaign.Recover(Campaign, id, SafeAnchor, true, balance.chargeCooldown); if (ok) { Phone.RestoreBattery(Campaign.Data.battery); Phone.isScreenOn = true; Phone.SyncVisualState(); } return ok; })); return; }
            if (id.StartsWith("PRAYER_")) { StartCoroutine(HoldWorld(item, balance.prayerHold, "TENANGKAN DIRI • ALTAR DOA", () => Phase5Campaign.Recover(Campaign, id, SafeAnchor, false, balance.prayerCooldown))); return; }
            if (id.StartsWith("CABLE_")) { int n = int.Parse(id.Substring(6)); StartCoroutine(HoldWorld(item, balance.cableHold, "PUTUS KABEL ROH " + n, () => {
                bool ok = Phase5Campaign.CutCable(Campaign, n); if (ok) { RefreshWorld(); soundscape.Cue("Cable", item.transform.position); } return ok; }, () => CameraCanScan && !Phone.isSelfieCamera && Phone.currentScanMode == ScanVisualMode.AR)); return; }
            if (id.StartsWith("LANTERN_")) { boss.TakeLantern(int.Parse(id.Substring(8))); return; }
            if (id.StartsWith("HIDE_"))
            { Hud.ShowBroadcastNotification("Matikan layar, jongkok, dan masuk di balik penutup. Perlindungan ditentukan dinding dan arah pandang replika.", 5); return; }
            switch (id)
            {
                case "SHOP": OpenShop(); break;
                case "RULES": ReadDocument("P5_DOC_RULES", "Kertas Aturan Pasar Setan", Phase5Content.Rules); break;
                case "MANUSCRIPT": ReadDocument("P5_DOC_SURO_MANUSCRIPT", "Manuskrip Rahasia Mbah Suro", Phase5Content.Manuscript); break;
                case "START_SECTORS":
                    if (Completed("P5_S3_CLEAR")) StartCoroutine(GateReturnToStage());
                    else if (Phase5Campaign.EnterSectors(Campaign)) { RefreshWorld(); SectorChat(1); world.sectorBlockers[0].SetActive(false); }
                    else SaveError(); break;
                case "DOOR_0": case "DOOR_2": StartCoroutine(HotDoor()); break;
                case "DOOR_1":
                    if (!Completed("P5_S1_THERMO_OK")) { Hud.ShowBroadcastNotification("Pintu Tengah −22°C belum terverifikasi. Stabilkan Thermo pada tiga pintu; Selfie menahan replika.", 5); break; }
                    if (Phase5Campaign.ClearSector(Campaign, 1)) { RefreshWorld(); SectorChat(2); }
                    else SaveError(); break;
                case "CUP":
                    if (Phase5Campaign.EnsureVessel(Campaign)) Hud.ShowBroadcastNotification(Phase5Ids.HasWaterVessel(Campaign.Data) ? "Wadah air tersedia. Isi ulang di titik samping gerbang." : "Periksa wadah air.", 4); else SaveError(); break;
                case "REFILL":
                    if (!Phase5Ids.HasWaterVessel(Campaign.Data) && !Phase5Campaign.EnsureVessel(Campaign)) { SaveError(); break; }
                    StartCoroutine(HoldWorld(item, balance.refillHold, "ISI KENDI / CAWAN", () => Phase5Campaign.Refill(Campaign))); break;
                case "POUR":
                    if (Phase5Campaign.Pour(Campaign)) { RefreshWorld(); soundscape.Cue("Water", item.transform.position); }
                    else Hud.ShowBroadcastNotification(Campaign.Data.phase4.water_units == 0 ? "KENDI KOSONG • titik air ada di samping gerbang." : "Obor sudah padam atau wadah belum diambil.", 4); break;
                case "SEWING_PORTAL": StartCoroutine(CrossMirror(false)); break;
                case "BACK_MIRROR": StartCoroutine(CrossMirror(true)); break;
                case "GATE_RETURN": StartCoroutine(GateReturnToStage()); break;
                case "TAPE":
                    if (Phase5Campaign.CollectTape(Campaign)) { RefreshWorld(); Hud.ShowBroadcastNotification("PITA REKAMAN PERTAMA TERSIMPAN • Arsip Saksi " + Phase5Ids.ArchiveCount(Campaign.Data) + "/5", 7); }
                    else SaveError(); break;
                case "DAMAR": if (FinalChoice) ToggleEscort(); else StageDialogue(); break;
                case "ARCHIVE": InteractArchive(item); break;
                case "START_ARENA": OpenArenaWarning(); break;
                case "GENERATOR": boss.ActivateGenerator(); break;
            }
        }
        private IEnumerator HoldWorld(Phase5WorldItem item, float seconds, string label, Func<bool> complete, Func<bool> additional = null)
        {
            if (ActionBusy) yield break;
            ActionBusy = true; float elapsed = 0;
            ShowHold(label);
            while (elapsed < seconds && !Dead)
            {
                if (!GameInputConfig.Instance.GetActionHeld(GameAction.Interact) || !item ||
                    Vector3.Distance(player.transform.position + Vector3.up, item.transform.position) > 3.5f || additional != null && !additional()) break;
                if (ClockRunning) elapsed += Time.deltaTime;
                UpdateHold(label, elapsed / seconds); yield return null;
            }
            bool finished = elapsed >= seconds;
            if (finished && !complete()) SaveError();
            if (holdPanel) Destroy(holdPanel); holdPanel = null; ActionBusy = false; RefreshObjective();
        }
        private void ShowHold(string label)
        {
            if (holdPanel) Destroy(holdPanel);
            holdPanel = UiWidgets.Panel("P5WorldHold", Hud.GetComponentInChildren<Canvas>().transform, new Vector2(.33f, .12f), new Vector2(.67f, .2f), new Color(.018f, .023f, .027f, .9f));
            UiWidgets.Text("Progress", holdPanel.transform, label, new Vector2(.04f, .08f), new Vector2(.96f, .92f), 21);
        }
        private void UpdateHold(string label, float progress)
        {
            if (holdPanel) holdPanel.GetComponentInChildren<Text>().text = label + " • " + Mathf.Clamp01(progress).ToString("P0") + "\nLepas " + GameInputConfig.Instance.GetActionLabel(GameAction.Interact) + " untuk batal";
        }
        private IEnumerator BurnIncense()
        {
            if (ActionBusy || Campaign.Data.inventory.Find(Phase5Ids.IsIncense) == null || Campaign.Data.sanity >= 100 || Elapsed < State.incense_ready_at)
            { Hud.ShowBroadcastNotification("Kemenyan tidak tersedia atau belum siap. Altar Doa tetap dapat dipakai gratis.", 4); yield break; }
            ActionBusy = true; float t = 0;
            soundscape.Cue("Incense", player.transform.position); Hud.EnqueueSubtitle("Raka", "Gue harus tenang. Yang di layar belum tentu nyata.", 4);
            while (t < balance.incenseHold && !Dead) { if (ClockRunning) t += Time.deltaTime; yield return null; }
            if (!Dead && Phase5Campaign.Incense(Campaign)) clarityUntil = Elapsed + balance.incenseClarity;
            else if (!Dead) SaveError();
            ActionBusy = false;
        }
        private void ReadDocument(string id, string title, string body)
        {
            if (!Campaign.ArchiveDocument(title, body, id)) { SaveError(); return; }
            Hud.OpenDocumentModal(title, body);
        }
    }
}
