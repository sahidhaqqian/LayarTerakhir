using System;
using System.Collections.Generic;
using System.Linq;

namespace LayarTerakhir.Core
{
    [Serializable]
    public sealed class Phase4Data
    {
        public List<string> completed_event_ids = new List<string>();
        public List<Phase1DynamoUse> recovery_last_uses = new List<Phase1DynamoUse>();
        public List<Phase1MoneyReceipt> money_ledger = new List<Phase1MoneyReceipt>();
        public float elapsed_gameplay_seconds, incense_ready_at;
        public int water_units, water_action_sequence, torch_layout_seed = -1;
        public string safe_anchor = "HUB", next_destination = "";
        public bool Completed(string id) => completed_event_ids.Contains(id);
        public bool Add(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || Completed(id)) return false;
            completed_event_ids.Add(id); return true;
        }
    }

    public static class Phase4Ids
    {
        public const string Seal = "SEAL_DESA_MATI_KARANG_SETRA", Destination = "Pasar Setan";
        public const string Thermo = "P4_EVIDENCE_THERMO_66", Aura = "P4_EVIDENCE_AR_ALTAR", Mantra = "P4_EVIDENCE_EVP_4DIR";
        public const string Kendi = "P4_RELIC_KENDI", Blade = "P4_RELIC_BELATI", Soil = "P4_RELIC_TANAH";
        public const string Secret = "SECRET_P4_CERMIN_SAKSI";
        public static readonly string[] Evidence = { Thermo, Aura, Mantra };
        public static readonly string[] Relics = { Kendi, Blade, Soil };
        public static readonly string[] Sectors = { "N", "E", "W", "S" };
        public static readonly string[] Anchors = { "HUB", "ENTRY", "J1", "PENDOPO", "NORTH", "EAST", "WEST", "SOUTH", "PORTAL", "BOUNDARY" };
        public static bool AllEvidence(CampaignData d) => Evidence.All(d.evidence_ids.Contains);
        public static bool AllRelics(CampaignData d) => Relics.All(d.inventory.Contains);
        public static bool Eligible(CampaignData d) => d != null && d.phase3.Completed("P3_COMPLETE") &&
            new[] { Phase1.Phase1Ids.Seal, Phase2Ids.Seal, Phase3Ids.Seal }.All(d.location_seals.Contains);
        public static int LocalHits(CampaignData d) => Sectors.Take(3).Count(s => d.phase4.Completed("P4_BLITZ_" + s));
        public static int HitCount(CampaignData d) => LocalHits(d) + (d.phase4.Completed("P4_BOSS_WIN") ? 1 : 0);
        public static bool SectorDone(CampaignData d, int sector) => sector >= 0 && sector < 3 && d.phase4.Completed("P4_BLITZ_" + Sectors[sector]);
        public static int AuthenticIndex(int seed, int sector)
        {
            int[,] layouts = { { 1, 0, 2, 0 }, { 0, 2, 1, 2 }, { 2, 1, 0, 1 } };
            return layouts[Math.Max(0, Math.Min(2, seed)), Math.Max(0, Math.Min(3, sector))];
        }
        public static bool IsIncense(string id) => id != null &&
            (id.StartsWith("P4_KEMENYAN_") || id.StartsWith("P3_KEMENYAN_") || id.StartsWith("P2_KEMENYAN_") || id == "KEMENYAN_BAKAR" || id == "KEMENYAN");
        public static bool RitualReady(CampaignData d) => AllEvidence(d) && d.phase4.Completed("P4_JOURNAL_CONFIRMED") &&
            (AllRelics(d) || d.phase4.Completed("P4_OFFERINGS_PLACED"));
    }
}
