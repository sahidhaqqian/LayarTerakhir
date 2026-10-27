using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed partial class Phase5Director
    {
        private float hotDoorReady, nextSectorCommit;
        private void UpdateSectors(float dt)
        {
            if (Elapsed < nextSectorCommit) return;
            nextSectorCommit = Elapsed + .4f;
            // Sector exits are actual authored crossings, never arbitrary chat receipts.
            // S1 additionally requires Thermo verification of the middle door; do not auto-clear
            // on zone entry without it (prevents bypassing the Thermo/Selfie puzzle).
            if (Zone == "S2" && !Completed("P5_S1_CLEAR") && Completed("P5_ENTER_SECTORS") && Completed("P5_S1_THERMO_OK"))
            { if (Phase5Campaign.ClearSector(Campaign, 1)) { RefreshWorld(); SectorChat(2); } else SaveError(); }
            if (Zone == "S3" && !Completed("P5_S2_CLEAR") && Completed("P5_BRIDGE_TORCH_OFF"))
            { if (Phase5Campaign.ClearSector(Campaign, 2)) { RefreshWorld(); SectorChat(3); } else SaveError(); }
        }
        private IEnumerator HotDoor()
        {
            if (Elapsed < hotDoorReady || ActionBusy) yield break;
            ActionBusy = true; hotDoorReady = Elapsed + 4;
            Hud.EnqueueSubtitle("Pintu Cermin", "[Api mengembang di balik kaca panas.] Mundur!", 4);
            soundscape.Cue("HotDoor", player.transform.position + player.transform.forward * 3);
            float t = 0; while (t < balance.hotDoorTelegraph) { if (ClockRunning) t += Time.deltaTime; yield return null; }
            Penalty(balance.hotDoorLoss, "Pintu panas palsu. Periksa suhu dan stensil rangka pintu.");
            MovePlayer(world.Anchor("S1")); replicas.Restore(); ActionBusy = false;
        }
        private IEnumerator CrossMirror(bool backToGate)
        {
            if (ActionBusy || Completed("P5_ARENA_LOCK")) yield break;
            if (!backToGate && (!CameraCanScan || Phone.currentScanMode != ScanVisualMode.AR))
            { Hud.ShowBroadcastNotification("Permukaan cermin biasa. Gunakan Lensa AR untuk melihat jalan di baliknya.", 4); yield break; }
            bool ok = backToGate ? Receipt("P5_RETURN_MIRROR_DISCOVERED") : Phase5Campaign.ClearSector(Campaign, 3);
            if (!ok) { SaveError(); yield break; }
            ActionBusy = true; player.MovementLocked = true;
            soundscape.Cue("Mirror", player.transform.position); Hud.SetFadeAlpha(1); yield return new WaitForSecondsRealtime(.35f);
            MovePlayer(world.Anchor(backToGate ? "GATE" : "STAGE"));
            player.MovementLocked = false; ActionBusy = false; Hud.SetFadeAlpha(0); RefreshWorld();
            if (!backToGate) StageDialogue();
        }
        private IEnumerator GateReturnToStage()
        {
            if (ActionBusy || !Completed("P5_S3_CLEAR") || Completed("P5_ARENA_LOCK")) yield break;
            ActionBusy = true; player.MovementLocked = true; Hud.SetFadeAlpha(1); yield return new WaitForSecondsRealtime(.35f);
            MovePlayer(world.Anchor("MIRROR")); player.MovementLocked = false; ActionBusy = false; Hud.SetFadeAlpha(0);
        }
        public void RetrySector(string reason)
        {
            Penalty(balance.contactSanityLoss, reason);
            // Failure restores the committed snapshot via Continue; no free charge or resets to CP1.
            Retry(reason);
        }
    }
}
