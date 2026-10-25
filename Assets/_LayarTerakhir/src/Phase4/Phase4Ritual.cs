using System.Linq;
using LayarTerakhir.Core;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public enum Phase4RitualState { Inactive, Hunt, Exposed, Recovery, CoreTravel, CoreExposed, CoreRecovery, Resolved }
    public sealed class Phase4Ritual : MonoBehaviour
    {
        public Phase4Director director;
        public Phase4RitualState State { get; private set; }
        public Phase4Torch TemporaryTorch { get; private set; }
        public float Remaining => Mathf.Max(0, timer);
        public bool Active => State != Phase4RitualState.Inactive && State != Phase4RitualState.Resolved;
        public bool Exposed => State == Phase4RitualState.Exposed || State == Phase4RitualState.CoreExposed;
        public bool Recovering => State == Phase4RitualState.Recovery || State == Phase4RitualState.CoreRecovery;
        private float timer, pourReadyAt;
        private Phase4World W => director.world;
        private Phase4Balance B => director.balance;
        public string Status => State == Phase4RitualState.Resolved ? "SEGEL LOKASI KEEMPAT TERSIMPAN" :
            Exposed ? "PERISAI PECAH • " + Remaining.ToString("0.0") + "s • BIDIK INTI" :
            director.Completed("P4_SOUTH_OFF") ? "INTI MENUJU PENDOPO • BLITZ PENUTUP KEEMPAT" :
            Phase4Ids.LocalHits(director.Campaign.Data) == 3 ? "TIGA IKATAN PUTUS • PADAMKAN OBOR ASLI SELATAN" :
            "IKATAN " + Phase4Ids.LocalHits(director.Campaign.Data) + "/3 • N/E/W BEBAS URUTAN • CEK RAJAH UV" + (Recovering ? "\nJEDA PEMULIHAN " + Remaining.ToString("0") + "s" : "");
        public void Restore()
        {
            TemporaryTorch = null; timer = 0;
            State = director.Completed("P4_BOSS_WIN") ? Phase4RitualState.Resolved : director.Completed("P4_SOUTH_OFF") ? Phase4RitualState.CoreTravel :
                director.Completed("P4_RITUAL_TRIGGER") ? Phase4RitualState.Recovery : Phase4RitualState.Inactive;
            if (State == Phase4RitualState.Recovery) timer = B.restoreGrace;
        }
        public void Begin() { State = Phase4RitualState.Recovery; timer = 12; TemporaryTorch = null; director.entity.RitualBegan(); }
        public void Tick(float dt)
        {
            if (!Active) return;
            timer -= dt;
            if (State == Phase4RitualState.Exposed || State == Phase4RitualState.CoreExposed)
            {
                director.phoneView.ShowChapterTarget(W.weakPoint.position, true);
                if (timer <= 0) Recover(State == Phase4RitualState.CoreExposed);
                return;
            }
            director.phoneView.ShowChapterTarget(Vector3.zero, false);
            if (State == Phase4RitualState.Recovery && timer <= 0) State = Phase4RitualState.Hunt;
            if (State == Phase4RitualState.CoreTravel)
            {
                director.entity.MergeAtPendopo(dt);
                if (Vector3.Distance(W.banaspati.transform.position, W.Anchor("PENDOPO").position) < 1 &&
                    Vector3.Distance(director.player.transform.position, W.Anchor("PENDOPO").position) < B.aimRange)
                    ExposeCore();
            }
            if (State == Phase4RitualState.CoreRecovery && timer <= 0 && director.Phone.batteryPercent >= director.Phone.blitzCostPercent &&
                Vector3.Distance(director.player.transform.position, W.Anchor("PENDOPO").position) < B.aimRange) ExposeCore();
        }
        public bool Pour(Phase4Torch torch)
        {
            if (!torch || !Active || Exposed || Recovering || director.Elapsed < pourReadyAt) return false;
            if (torch.PermanentlyOff || torch.sector == 3 && Phase4Ids.LocalHits(director.Campaign.Data) < 3)
            { director.Hud.ShowBroadcastNotification("Ikatan penjuru ini belum terbuka atau telah selesai.", 4); return false; }
            if (director.Campaign.Data.phase4.water_units == 0)
            { director.Hud.ShowBroadcastNotification("KENDI KOSONG • Sumur Barat dan saluran ruwatan dapat diisi ulang gratis.", 4); return false; }
            // This guard applies equally to all three candidates, never reveals truth.
            // A five-second exposure must be physically reachable without teleporting the boss.
            if (Vector3.Distance(director.player.cameraPivot.position, W.weakPoint.position) > B.aimRange - .5f ||
                !director.ClearLine(director.player.cameraPivot.position, W.weakPoint.position, W.banaspati.transform))
            { director.Hud.ShowBroadcastNotification("Pancing api mendekat ke penjuru ini sebelum menyiram. Berlindung sambil memeriksa rajah.", 4); return false; }
            if (!Phase4Campaign.Pour(director.Campaign, torch.sector, torch.candidate, director.SafeAnchor))
            { director.SaveError(); return false; }
            pourReadyAt = director.Elapsed + 1.5f;
            director.soundscape.Cue("Water", torch.transform.position);
            if (!torch.Authentic)
            {
                director.soundscape.Cue("FalseTorch", torch.transform.position);
                director.Hud.ShowBroadcastNotification("RAJAH SILANG • api palsu menyambar; Sanity berkurang 40. Sumur tetap dapat diisi ulang.", 5);
                director.entity.RecoilFromFalseTorch(); return true;
            }
            if (torch.sector == 3)
            {
                State = Phase4RitualState.CoreTravel; TemporaryTorch = null; director.entity.BeginMerge();
                director.Hud.EnqueueSubtitle("Raka", "Empat obornya padam. Intinya ditarik balik ke Pendopo!", 6);
            }
            else { TemporaryTorch = torch; State = Phase4RitualState.Exposed; timer = B.exposureSeconds; director.entity.Expose(); }
            director.RefreshWorld(); return true;
        }
        public bool Capture()
        {
            if (director.Completed("P4_BOSS_WIN")) return false;
            if (!Active) return false;
            bool valid = Exposed && !director.Phone.isSelfieCamera && director.Aim(W.weakPoint, B.aimRange, B.aimAngle, W.banaspati.transform);
            int target = valid ? State == Phase4RitualState.CoreExposed ? 3 : TemporaryTorch.sector : -1;
            float before = director.Phone.batteryPercent;
            if (!director.Phone.TryCaptureEnergy(true))
            { director.Hud.ShowBroadcastNotification("Blitz perlu 15% daya dan jeda 6 detik. Dinamo manual tersedia pada jalur perlindungan.", 4); return true; }
            if (!Phase4Campaign.Shot(director.Campaign, before, director.Phone.batteryPercent, target, target == 3 ? "PORTAL" : director.SafeAnchor))
            { director.Phone.RestoreBattery(before); director.SaveError(); return true; }
            director.phoneView.CaptureFlash(); director.soundscape.Cue("Shutter", director.player.transform.position);
            if (!valid) { director.Hud.ShowBroadcastNotification("Bidikan belum mengenai inti yang terbuka. Sektor yang telah tersimpan tetap selesai.", 4); return true; }
            if (target == 3)
            {
                State = Phase4RitualState.Resolved; TemporaryTorch = null; director.entity.Resolve();
                director.RefreshWorld(); director.Hud.ShowBroadcastNotification("SEGEL LOKASI KEEMPAT TERSIMPAN • empat Blitz berhasil", 6);
            }
            else
            {
                director.Hud.ShowBroadcastNotification("IKATAN " + Phase4Ids.Sectors[target] + " TERSIMPAN • " + Phase4Ids.LocalHits(director.Campaign.Data) + "/3", 5);
                Recover(false);
            }
            return true;
        }
        private void Recover(bool core)
        {
            TemporaryTorch = null; timer = B.recoverySeconds; State = core ? Phase4RitualState.CoreRecovery : Phase4RitualState.Recovery;
            director.entity.Recover(); director.RefreshWorld();
        }
        private void ExposeCore()
        {
            State = Phase4RitualState.CoreExposed; timer = B.exposureSeconds;
            W.suro.SetActive(false); W.mergeSuro.transform.position = W.banaspati.transform.position;
            W.mergeSuro.SetActive(true); director.entity.Expose();
        }
        public void RequestCoreWindow()
        {
            if (director.Completed("P4_SOUTH_OFF") && State == Phase4RitualState.CoreRecovery && timer <= 0) ExposeCore();
        }
    }
}
