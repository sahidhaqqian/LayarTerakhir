using System;
using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Phase1;

namespace LayarTerakhir.Core
{
    [Serializable]
    public sealed class Phase5Data
    {
        public List<string> completed_event_ids = new List<string>();
        public List<Phase1DynamoUse> recovery_last_uses = new List<Phase1DynamoUse>();
        public List<Phase1MoneyReceipt> money_ledger = new List<Phase1MoneyReceipt>();
        public float elapsed_gameplay_seconds, incense_ready_at;
        public string safe_anchor = "GATE";
        public bool Completed(string id) => completed_event_ids.Contains(id);
        public bool Add(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || Completed(id)) return false;
            completed_event_ids.Add(id); return true;
        }
    }

    public static class Phase5Ids
    {
        public const string Secret = "SECRET_P5_PITA_PERTAMA", Server = "P5_EVIDENCE_SERVER_AR",
            Contract = "P5_EVIDENCE_CONTRACT", Crowd = "P5_EVIDENCE_EVP_CROWD", Cup = "P5_CAWAN_AIR_PASAR";
        public static readonly string[] Evidence = { Server, Contract, Crowd };
        public static readonly string[] Seals = { Phase1Ids.Seal, Phase2Ids.Seal, Phase3Ids.Seal, Phase4Ids.Seal };
        public static readonly string[] Anchors = { "GATE", "S1", "S2", "S3", "STAGE", "MIRROR", "ARENA_WEST", "ARENA_EAST", "PORTAL", "ENDING" };
        public static bool FourSeals(CampaignData d) => d != null && Seals.All(d.location_seals.Contains);
        public static bool Eligible(CampaignData d) => d != null && CampaignRouting.IsPhase4(d) &&
            d.checkpoint_id == "PHASE4_CP8" && d.phase4.Completed("P4_COMPLETE") && FourSeals(d);
        public static bool AllEvidence(CampaignData d) => Evidence.All(d.evidence_ids.Contains);
        public static int CoreCount(CampaignData d) => Enumerable.Range(1, 3).Count(n => d.phase5.Completed("P5_CORE_" + n + "_BROKEN"));
        public static int CableCount(CampaignData d) => Enumerable.Range(1, 3).Count(n => d.phase5.Completed("P5_CABLE_" + n + "_CUT"));
        public static int ArchiveCount(CampaignData d) => Phase1Ids.Secrets.Count(d.secret_items.Contains);
        public static bool ArchiveComplete(CampaignData d) => Phase1Ids.Secrets.All(d.secret_items.Contains);
        public static bool HasWaterVessel(CampaignData d) => d.inventory.Contains(Phase4Ids.Kendi) || d.inventory.Contains(Cup);
        public static bool FinalChoice(CampaignData d) => d.phase5.Completed("P5_FINAL_CHOICE_ENTER") && d.ending == EndingId.NONE;
        public static bool CapacitorSpent(CampaignData d) => d.ending == EndingId.BAD || d.ending == EndingId.TRUE || d.ending == EndingId.SECRET;
        public static float CapacitorPercent(CampaignData d) => d.phase5.Completed("P5_FINAL_CHOICE_ENTER") && !CapacitorSpent(d) ? 5 : 0;
        public static bool CapacitorAvailable(CampaignData d) => FinalChoice(d) && CapacitorPercent(d) == 5;
        public static bool IsFlare(string id) => id != null && (id.StartsWith("P5_FLARE_") || id.StartsWith("P3_FLARE_") || id.StartsWith("P2_FLARE_") || id == "FLARE_MERAH_DARURAT" || id == "FLARE");
        public static bool IsIncense(string id) => id != null && (id.StartsWith("P5_KEMENYAN_") || Phase4Ids.IsIncense(id));
    }
}
