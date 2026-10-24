using System;
using System.Collections.Generic;
using System.Linq;

namespace LayarTerakhir.Core
{
    [Serializable]
    public sealed class Phase3Data
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

    public static class Phase3Ids
    {
        public const string Seal = "SEAL_PABRIK_GULA_SINDANG_LAUT";
        public const string Destination = "Desa Mati Karang Setra";
        public const string Sling = "P3_GENDONGAN_TUMBAL", EVP = "P3_EVP_NINA_BOBO", Rajah = "P3_RAJAH_ANAK";
        public const string Doll = "P3_BONEKA_JERAMI", Sickle = "P3_SABIT_RAJAH", Cloth = "P3_SELENDANG_BATIK";
        public const string Secret = "SECRET_P3_PELAT_BURUH", Shoe = "P3_SEPATU_DAMAR";
        public static readonly string[] Evidence = { Sling, EVP, Rajah };
        public static readonly string[] Relics = { Doll, Sickle, Cloth };
        public static readonly string[] SafeAnchors = { "HUB", "YARD", "MILL", "BOILER", "OFFICE", "BAGASSE", "ALTAR" };
        public static bool AllEvidence(CampaignData d) => Evidence.All(d.evidence_ids.Contains);
        public static bool AllRelics(CampaignData d) => Relics.All(d.inventory.Contains);
        public static bool RitualReady(CampaignData d) => AllEvidence(d) && AllRelics(d) && d.phase3.Completed("P3_JOURNAL_CONFIRMED");
        public static bool IsFlare(string id) => id.StartsWith("P3_FLARE_") || id.StartsWith("P2_FLARE_") || id == "FLARE_INDUSTRI";
        public static void NormalizeAliases(CampaignData d)
        {
            string[] oldIds = { "P3_AR_GENDONGAN", "P3_EVP_NINABOBO", "P3_UV_RAJAH_ANAK", "P3_BONEKA_JERAMI_TEBU", "P3_SABIT_TEBU_RAJAH", "P3_SELENDANG_BATIK_TERBAKAR", "P3_DOC_MANDOR_1999", "P3_GEN_ACCESS_01_ON" };
            string[] newIds = { Sling, EVP, Rajah, Doll, Sickle, Cloth, "P3_DOC_MANDOR", "P3_ACCESS_GENERATOR" };
            foreach (var list in new[] { d.inventory, d.evidence_ids, d.relic_ids, d.phase3.completed_event_ids })
                for (int i=0;i<oldIds.Length;i++)
                    if(list.Remove(oldIds[i]) && !list.Contains(newIds[i])) list.Add(newIds[i]);
        }
    }
}
