using System;
using System.Linq;
using LayarTerakhir.Core;

namespace LayarTerakhir.Phase1
{
    public enum Phase1Stage { Arrival, Pump, Passage102, Mailbox, Lift, Kuntilanak, Investigation, Boss, Epilogue, Return, Complete }
    public static class Phase1Ids
    {
        public const string Secret = "SECRET_P1_NEGATIF_TARI", Keris = "P1_KERIS", Key = "P1_LIFT_KEY";
        public const string Powerbank = "P1_POWERBANK_MAILBOX_207", Seal = "SEAL_RUSUNAWA_GRIYA_ASRI";
        public const string Claw = "P1_CLAW_402", EVP = "P1_EVP_BATHROOM", Aura = "P1_AURA_KERIS";
        public static readonly string[] Evidence = { Claw, EVP, Aura };
        public static readonly string[] Secrets = { Secret, "SECRET_P2_LABEL_PASIEN", "SECRET_P3_PELAT_BURUH", "SECRET_P4_CERMIN_SAKSI", "SECRET_P5_PITA_PERTAMA" };
        public static bool AllEvidence(CampaignData data) => Evidence.All(data.evidence_ids.Contains);
        public static int EvidenceCount(CampaignData data) => Evidence.Count(data.evidence_ids.Contains);
        public static bool Ready(CampaignData data) => AllEvidence(data) && data.phase1.Completed("P1_JOURNAL_CONFIRMED") && data.inventory.Contains(Keris);
        public static Phase1Stage Stage(CampaignData data, bool bossAttempt = false)
        {
            var p = data.phase1;
            if (p.Completed("P1_COMPLETE")) return Phase1Stage.Complete;
            if (p.Completed("P1_EPILOGUE")) return Phase1Stage.Return;
            if (p.Completed("P1_BOSS_WIN")) return Phase1Stage.Epilogue;
            if (bossAttempt) return Phase1Stage.Boss;
            if (p.Completed("P1_CUT_CHAIN")) return Phase1Stage.Investigation;
            if (p.Completed("P1_LIFT_RIDE")) return Phase1Stage.Kuntilanak;
            if (p.Completed("P1_MAILBOX_207")) return Phase1Stage.Lift;
            if (p.Completed("P1_CROSS_102")) return Phase1Stage.Mailbox;
            if (p.Completed("P1_ENTER_PUMP")) return Phase1Stage.Passage102;
            if (p.Completed("P1_START_LIVE")) return Phase1Stage.Pump;
            return Phase1Stage.Arrival;
        }
        public static bool Grant(CampaignData data, string eventId, Action<CampaignData> action)
        {
            if (!data.phase1.Add(eventId)) return false;
            action?.Invoke(data); return true;
        }
        public static void Donation(CampaignData data, string receipt, long amount, bool bound)
        {
            if (data.phase1.money_ledger.Exists(e => e.id == receipt)) return;
            data.phase1.money_ledger.Add(new Phase1MoneyReceipt { id = receipt, amount = amount, bound = bound });
            if (bound) data.wallet_bound += amount; else data.wallet_legitimate += amount;
        }
        public static bool InWindow(float now, float opened, float duration = 5) => now >= opened && now - opened < duration;
        // A full dynamo cycle must never be consumed for no energy (QA-31/52).
        public static bool FullChargeWasted(float batteryPercent) => batteryPercent >= 99f;
        public static float EmergencyChargeResult(float batteryPercent) => UnityEngine.Mathf.Min(10f, UnityEngine.Mathf.Max(batteryPercent, 10f));
        // The journal is a deduction tool, not a walkthrough: locations and the
        // weakness are only written down after Raka has actually found them.
        public static string JournalText(CampaignData data)
        {
            string Check(string id, string found, string missing) => (data.evidence_ids.Contains(id) ? "[✓] " + found : "[ ] " + missing) + "\n";
            return "JURNAL KLENIK v2.0\n" + (AllEvidence(data) ? "GENDERUWO ARANG" : "PENUNGGU LANTAI EMPAT — BELUM TERIDENTIFIKASI") + "\n\n" +
                Check(Claw, "Jejak fisik — cakaran besar, Kamar 402", "Jejak fisik (cakaran) — belum ditemukan") +
                Check(EVP, "Suara gaib — rekaman EVP kamar mandi", "Suara gaib (rekaman) — belum ditemukan") +
                Check(Aura, "Aura kematian — tungku dapur umum", "Aura kematian (lensa AR) — belum ditemukan") + "\n" +
                (AllEvidence(data) ? "GENDERUWO ARANG\nPerisai kabutnya runtuh oleh sorot lampu halogen koridor. Saat tubuhnya terekspos, bidik kepalanya dengan Blitz.\n\n" : "Tiga bukti berbeda diperlukan untuk identifikasi.\n\n") +
                (data.phase1.Completed("P1_JOURNAL_CONFIRMED") ? "Identitas dikonfirmasi.\n" : "Identitas belum dikonfirmasi.\n") +
                "\nARSIP SAKSI\nBabak ini: " + (data.secret_items.Contains(Secret) ? "1/1 — Negatif Tari" : "0/1 — belum ditemukan") +
                "\nCampaign: " + Secrets.Count(data.secret_items.Contains) + "/5";
        }
    }

    // All chapter-only tuning is explicit; the player/global phone defaults are unchanged.

}
