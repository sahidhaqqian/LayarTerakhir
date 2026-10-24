using System;
using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;

namespace LayarTerakhir.Phase3
{
    // Uses the single campaign transaction/rollback and checkpoint file.
    public static class Phase3Campaign
    {
        public static bool EnterHub(CampaignState campaign)
        {
            if (!campaign || campaign.Data.chapter_id != 2 || !campaign.Data.phase2.Completed("P2_COMPLETE") ||
                (!campaign.Data.location_seals.Contains(Phase1Ids.Seal) || !campaign.Data.location_seals.Contains(Phase2Ids.Seal))) return false;
            return campaign.TryCommit("PHASE3_HUB", d =>
            {
                d.chapter_id = 3; d.phase3 = new Phase3Data(); d.phase3.Add("P3_ARRIVE_KOS");
            });
        }
        public static void Unlock(CampaignData d)
        {
            d.phase3.Add("P3_UPGRADE_UNLOCK");
            foreach (string id in new[] { "UV_RAJAH", "EVP_V2" })
                if (!d.upgrades.Contains(id)) d.upgrades.Add(id);
        }
        public static bool Depart(CampaignState campaign)
        {
            if (!campaign || !CampaignRouting.IsPhase3(campaign.Data) || campaign.Data.phase3.Completed("P3_DEPART_KOS")) return false;
            return campaign.TryCommit("PHASE3_CP1", d =>
            { Unlock(d); d.phase3.Add("P3_DEPART_KOS"); d.phase3.safe_anchor = "YARD"; });
        }
        public static bool Buy(CampaignState campaign, string pack)
        {
            long price; string[] items;
            switch (pack)
            {
                case "POWERBANK": price = 750000; items = new[] { "P3_POWERBANK_A", "P3_POWERBANK_B", "P3_POWERBANK_C" }; break;
                case "KEMENYAN": price = 350000; items = new[] { "P3_KEMENYAN_A", "P3_KEMENYAN_B" }; break;
                case "FLARE": price = 1000000; items = new[] { "P3_FLARE_A", "P3_FLARE_B", "P3_FLARE_C", "P3_FLARE_D" }; break;
                default: return false;
            }
            string receipt = "P3_SHOP_PACK_" + pack;
            if (!campaign || !CampaignRouting.IsPhase3(campaign.Data) || campaign.Data.phase3.Completed("P3_DEPART_KOS") ||
                campaign.Data.phase3.Completed(receipt) || campaign.Data.wallet_legitimate < price) return false;
            return campaign.TryCommit(campaign.Data.checkpoint_id, d =>
            {
                d.wallet_legitimate -= price; d.spent += price; d.inventory.AddRange(items); d.phase3.Add(receipt);
                d.phase3.money_ledger.Add(new Phase1MoneyReceipt { id = receipt, amount = price, bound = false });
            });
        }
        public static bool ChargeHub(CampaignState campaign)
        {
            if (!campaign || !CampaignRouting.IsPhase3(campaign.Data) || campaign.Data.phase3.Completed("P3_DEPART_KOS") ||
                campaign.Data.phase3.Completed("P3_KOS_CHARGED")) return false;
            return campaign.TryCommit(campaign.Data.checkpoint_id, d => { d.battery = 100; d.phase3.Add("P3_KOS_CHARGED"); });
        }
        public static string InvestigationCheckpoint(CampaignData d, string desired)
        {
            if (d.phase3.Completed("P3_COMPLETE")) return "PHASE3_CP7";
            if (d.phase3.Completed("P3_BOSS_WIN")) return "PHASE3_CP6";
            if (d.checkpoint_id == "PHASE3_CP5") return d.checkpoint_id;
            // Backtracking receipts update data at the latest checkpoint; never regress anchors.
            if (int.TryParse(d.checkpoint_id.Replace("PHASE3_CP", ""), out int oldCp) &&
                desired != null && int.TryParse(desired.Replace("PHASE3_CP", ""), out int nextCp) && nextCp < oldCp)
                return d.checkpoint_id;
            return desired ?? d.checkpoint_id;
        }
        public static bool PrepareRitual(CampaignState campaign)
        {
            if (!campaign || !CampaignRouting.IsPhase3(campaign.Data) || campaign.WritesBlocked ||
                campaign.Data.phase3.Completed("P3_BOSS_WIN") || !Phase3Ids.RitualReady(campaign.Data)) return false;
            return campaign.TryCommit("PHASE3_CP5", d => d.phase3.safe_anchor = "ALTAR");
        }
        public static bool WinRitual(CampaignState campaign, object owner, int layers, IEnumerable<string> flares)
        {
            if (!campaign || !campaign.WritesBlocked || layers != 3 || !Phase3Ids.RitualReady(campaign.Data) ||
                campaign.Data.phase3.Completed("P3_BOSS_WIN")) return false;
            var used = flares.Distinct().ToArray();
            if (used.Any(id => !Phase3Ids.IsFlare(id) || !campaign.Data.inventory.Contains(id))) return false;
            return campaign.TryCommit("PHASE3_CP6", d =>
            {
                d.phase3.Add("P3_BOSS_WIN"); d.phase3.safe_anchor = "ALTAR";
                foreach (var id in Phase3Ids.Relics) d.inventory.Remove(id);
                foreach (var id in used) d.inventory.Remove(id);
                if (!d.location_seals.Contains(Phase3Ids.Seal)) d.location_seals.Add(Phase3Ids.Seal);
            }, owner);
        }
        public static bool Finish(CampaignState campaign)
        {
            if (!campaign || campaign.WritesBlocked || !campaign.Data.phase3.Completed("P3_EPILOGUE") ||
                !campaign.Data.phase3.Completed("P3_BOSS_WIN") || !campaign.Data.location_seals.Contains(Phase3Ids.Seal) ||
                campaign.Data.phase3.Completed("P3_COMPLETE")) return false;
            return campaign.TryCommit("PHASE3_CP7", d =>
            { d.phase3.Add("P3_COMPLETE"); d.phase3.next_destination = Phase3Ids.Destination; d.phase3.safe_anchor = "YARD"; });
        }
    }
}
