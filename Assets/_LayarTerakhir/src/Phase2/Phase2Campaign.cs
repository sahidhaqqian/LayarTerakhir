using System;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;

namespace LayarTerakhir.Phase2
{
    // Uses the single campaign transaction/rollback and checkpoint file.
    public static class Phase2Campaign
    {
        public static bool EnterHub(CampaignState campaign)
        {
            if (!campaign || campaign.Data.chapter_id != 1 || !campaign.Data.phase1.Completed("P1_COMPLETE") ||
                !campaign.Data.location_seals.Contains(Phase1Ids.Seal)) return false;
            return campaign.TryCommit("PHASE2_HUB", d =>
            {
                d.chapter_id = 2; d.phase2 = new Phase2Data(); d.phase2.Add("P2_ARRIVE_KOS");
            });
        }
        public static void Unlock(CampaignData d)
        {
            d.phase2.Add("P2_UPGRADE_UNLOCK");
            foreach (string id in new[] { "SELFIE_MODE", "THERMOKLENIK_APK" })
                if (!d.upgrades.Contains(id)) d.upgrades.Add(id);
        }
        public static bool Depart(CampaignState campaign)
        {
            if (!campaign || !CampaignRouting.IsPhase2(campaign.Data) || campaign.Data.phase2.Completed("P2_DEPART_KOS")) return false;
            return campaign.TryCommit("PHASE2_CP1", d =>
            { Unlock(d); d.phase2.Add("P2_DEPART_KOS"); d.phase2.safe_anchor = "IGD"; });
        }
        public static bool Buy(CampaignState campaign, string pack)
        {
            long price; string[] items;
            switch (pack)
            {
                case "POWERBANK": price = 500000; items = new[] { "P2_POWERBANK_PASAR_GAIB_A", "P2_POWERBANK_PASAR_GAIB_B" }; break;
                case "KEMENYAN": price = 350000; items = new[] { "P2_KEMENYAN_A", "P2_KEMENYAN_B" }; break;
                case "FLARE": price = 750000; items = new[] { "P2_FLARE_A", "P2_FLARE_B", "P2_FLARE_C" }; break;
                default: return false;
            }
            string receipt = "P2_SHOP_PACK_" + pack;
            if (!campaign || !CampaignRouting.IsPhase2(campaign.Data) || campaign.Data.phase2.Completed("P2_DEPART_KOS") ||
                campaign.Data.phase2.Completed(receipt) || campaign.Data.wallet_legitimate < price) return false;
            return campaign.TryCommit(campaign.Data.checkpoint_id, d =>
            {
                d.wallet_legitimate -= price; d.spent += price; d.inventory.AddRange(items); d.phase2.Add(receipt);
                d.phase2.money_ledger.Add(new Phase1MoneyReceipt { id = receipt, amount = price, bound = false });
            });
        }
        public static bool ChargeHub(CampaignState campaign)
        {
            if (!campaign || !CampaignRouting.IsPhase2(campaign.Data) || campaign.Data.phase2.Completed("P2_DEPART_KOS") ||
                campaign.Data.phase2.Completed("P2_KOS_CHARGED")) return false;
            return campaign.TryCommit(campaign.Data.checkpoint_id, d => { d.battery = 100; d.phase2.Add("P2_KOS_CHARGED"); });
        }
        public static string InvestigationCheckpoint(CampaignData d, string desired)
        {
            // Receipts are authoritative; optional early evidence can create CP4 before CP2/3.
            if (d.checkpoint_id == "PHASE2_CP4" && (desired == "PHASE2_CP2" || desired == "PHASE2_CP3")) return d.checkpoint_id;
            return desired ?? d.checkpoint_id;
        }
    }
}
