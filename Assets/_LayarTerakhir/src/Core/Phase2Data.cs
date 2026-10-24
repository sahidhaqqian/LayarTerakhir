using System;
using System.Collections.Generic;
using System.Linq;

namespace LayarTerakhir.Core
{
    [Serializable]
    public sealed class Phase2Data
    {
        public List<string> completed_event_ids = new List<string>();
        public List<string> dynamo_full_charge_used_ids = new List<string>();
        public List<Phase1DynamoUse> dynamo_emergency_last_uses = new List<Phase1DynamoUse>();
        public List<Phase1MoneyReceipt> money_ledger = new List<Phase1MoneyReceipt>();
        public float elapsed_gameplay_seconds;
        public string safe_anchor = "HUB", next_destination = "";
        public bool Completed(string id) => completed_event_ids.Contains(id);
        public bool Add(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || Completed(id)) return false;
            completed_event_ids.Add(id); return true;
        }
    }

    public static class Phase2Ids
    {
        public const string Freeze = "P2_FREEZE_POLI03", Aura = "P2_AURA_TUMBAL_C3", EVP = "P2_EVP_INKUBATOR04";
        public const string Gunting = "P2_GUNTING_TALI_PUSAR", Gelang = "P2_GELANG_BAYI_LASTRI", Blood = "P2_BOTOL_DARAH_SEGEL";
        public const string Key = "P2_KUNCI_TERALIS_L2", ArchiveKey = "P2_KUNCI_ARSIP", Secret = "SECRET_P2_LABEL_PASIEN";
        public static readonly string[] Evidence = { Freeze, Aura, EVP };
        public static readonly string[] Relics = { Gunting, Gelang, Blood };
        public static bool AllEvidence(CampaignData d) => Evidence.All(d.evidence_ids.Contains);
        public static int EvidenceCount(CampaignData d) => Evidence.Count(d.evidence_ids.Contains);
        public static bool AllRelics(CampaignData d) => Relics.All(d.inventory.Contains);
        public static bool RitualReady(CampaignData d) => AllEvidence(d) && AllRelics(d) && d.phase2.Completed("P2_JOURNAL_CONFIRMED");
    }
}
