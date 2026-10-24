using System;
using System.Collections.Generic;

namespace LayarTerakhir.Core
{
    // Chapter-local receipts in the existing campaign, never a second save slot.
    [Serializable]
    public sealed class Phase1Data
    {
        public List<string> completed_event_ids = new List<string>();
        public List<string> dynamo_full_charge_used_ids = new List<string>();
        public List<Phase1DynamoUse> dynamo_emergency_last_uses = new List<Phase1DynamoUse>();
        public List<Phase1MoneyReceipt> money_ledger = new List<Phase1MoneyReceipt>();
        public Phase1Challenge signboard_challenge;
        public float elapsed_gameplay_seconds;
        public string safe_anchor = "ARRIVAL";
        public string next_destination = "";
        public bool Completed(string id) => completed_event_ids.Contains(id);
        public bool Add(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || Completed(id)) return false;
            completed_event_ids.Add(id); return true;
        }
    }
    public enum Phase1Challenge { Unoffered, Offered, Succeeded, Expired }
    [Serializable] public sealed class Phase1DynamoUse { public string id; public float at_gameplay_seconds; }
    [Serializable] public sealed class Phase1MoneyReceipt { public string id; public long amount; public bool bound; }

    public static class CampaignRouting
    {
        public const string RusunawaScene = "Babak1_Rusunawa";
        public const string HospitalScene = "Babak2_RSUD";
        public static bool IsPhase2(CampaignData data) => data != null && data.chapter_id == 2 &&
            data.checkpoint_id != null && data.checkpoint_id.StartsWith("PHASE2_", StringComparison.Ordinal);
        public static bool IsPhase1(CampaignData data) => data != null && data.chapter_id == 1 &&
            data.checkpoint_id != null && data.checkpoint_id.StartsWith("PHASE1_", StringComparison.Ordinal);
        // Unknown chapters must not be sent through a director that starts a fresh campaign.
        public static string ContinueScene(CampaignData data) => IsPhase2(data) ?
            (data.phase2.Completed("P2_DEPART_KOS") ? HospitalScene : "Prolog_KamarKos") : IsPhase1(data) ? RusunawaScene :
            data != null && data.chapter_id == 0 ? "Prolog_KamarKos" : null;
    }
}
