using System;
using System.Linq;
using LayarTerakhir.Core;
using UnityEngine;

namespace LayarTerakhir.Phase4
{
    // Chapter adapter to the existing campaign. Every accepted resource action
    // has one atomic write; no second save slot or long arena write barrier.
    public static class Phase4Campaign
    {
        public static string Checkpoint(CampaignData d, string desired = null)
        {
            if (d.phase4.Completed("P4_COMPLETE")) return "PHASE4_CP8";
            if (d.phase4.Completed("P4_BOSS_WIN")) return "PHASE4_CP7";
            if (Phase4Ids.LocalHits(d) > 0 || d.phase4.Completed("P4_SOUTH_OFF")) return "PHASE4_CP6";
            if (d.phase4.Completed("P4_RITUAL_TRIGGER")) return "PHASE4_CP5";
            int old = Number(d.checkpoint_id), next = Number(desired);
            return next > old ? desired : d.checkpoint_id;
        }
        private static int Number(string value) => value != null && value.StartsWith("PHASE4_CP") &&
            int.TryParse(value.Substring(9), out int number) ? number : 0;
        public static string Anchor(CampaignData d, string requested)
            => d.phase4.Completed("P4_COMPLETE") ? "BOUNDARY" : d.phase4.Completed("P4_BOSS_WIN") ? "PORTAL" : requested;
        public static bool EnterHub(CampaignState c)
        {
            if (!c || c.Data.chapter_id != 3 || !Phase4Ids.Eligible(c.Data)) return false;
            return c.TryCommit("PHASE4_HUB", d => { d.chapter_id = 4; d.phase4 = new Phase4Data(); d.phase4.Add("P4_ARRIVE_KOS"); });
        }
        public static void Unlock(CampaignData d)
        {
            d.phase4.Add("P4_MODULE_CALIBRATED");
            foreach (var id in new[] { "SANITY_MONITOR", "FILTER_ANTI_SENGKALA", "UV_RAJAH_V2" })
                if (!d.upgrades.Contains(id)) d.upgrades.Add(id);
        }
        public static bool Depart(CampaignState c)
        {
            if (!c || !CampaignRouting.IsPhase4(c.Data) || c.Data.phase4.Completed("P4_DEPART_KOS")) return false;
            return c.TryCommit("PHASE4_CP1", d => { Unlock(d); d.phase4.Add("P4_DEPART_KOS"); d.phase4.safe_anchor = "ENTRY"; });
        }
        public static bool ChargeHub(CampaignState c)
        {
            if (!c || !CampaignRouting.IsPhase4(c.Data) || c.Data.phase4.Completed("P4_KOS_CHARGE_USED") || c.Data.phase4.Completed("P4_DEPART_KOS")) return false;
            return c.TryCommit(c.Data.checkpoint_id, d => { d.battery = 100; d.phase4.Add("P4_KOS_CHARGE_USED"); });
        }
        public static bool RestHub(CampaignState c)
        {
            if (!c || !CampaignRouting.IsPhase4(c.Data) || c.Data.phase4.Completed("P4_KOS_REST_USED") || c.Data.phase4.Completed("P4_DEPART_KOS")) return false;
            return c.TryCommit(c.Data.checkpoint_id, d => { d.sanity = 100; d.phase4.Add("P4_KOS_REST_USED"); });
        }
        public static bool Buy(CampaignState c, string pack)
        {
            long price; int count; string kind;
            switch (pack)
            {
                case "KEMENYAN": price = 875000; count = 5; kind = "KEMENYAN"; break;
                case "POWERBANK": price = 750000; count = 3; kind = "POWERBANK"; break;
                case "WATER": price = 400000; count = 2; kind = "GARAM"; break;
                default: return false;
            }
            string receipt = "P4_SHOP_" + pack;
            if (!c || !CampaignRouting.IsPhase4(c.Data) || c.Data.phase4.Completed("P4_DEPART_KOS") ||
                c.Data.phase4.Completed(receipt) || c.Data.wallet_legitimate < price) return false;
            return c.TryCommit(c.Data.checkpoint_id, d =>
            {
                d.wallet_legitimate -= price; d.spent += price; d.phase4.Add(receipt);
                for (int i = 0; i < count; i++) d.inventory.Add("P4_" + kind + "_" + (char)('A' + i));
                d.phase4.money_ledger.Add(new Phase1MoneyReceipt { id = receipt, amount = price, bound = false });
            });
        }
        public static bool Receipt(CampaignState c, string id, string anchor, string desired = null, Action<CampaignData> mutation = null)
        {
            if (!c || !CampaignRouting.IsPhase4(c.Data)) return false;
            if (c.Data.phase4.Completed(id)) return true;
            return c.TryCommit(Checkpoint(c.Data, desired), d =>
            {
                d.phase4.Add(id); mutation?.Invoke(d); d.phase4.safe_anchor = Anchor(d, anchor);
            });
        }
        public static bool Evidence(CampaignState c, string id, string anchor, bool photograph)
        {
            if (!c || !Phase4Ids.Evidence.Contains(id)) return false;
            if (c.Data.evidence_ids.Contains(id)) return true;
            if (photograph && c.Data.battery < 1) return false;
            return Receipt(c, id, anchor, "PHASE4_CP4", d => { d.evidence_ids.Add(id); if (photograph) d.battery -= 1; });
        }
        public static bool Relic(CampaignState c, string id, string anchor)
        {
            if (!c || !Phase4Ids.Relics.Contains(id)) return false;
            return Receipt(c, "P4_TAKE_" + id, anchor, "PHASE4_CP4", d =>
            {
                if (!d.inventory.Contains(id)) d.inventory.Add(id);
                if (!d.relic_ids.Contains(id)) d.relic_ids.Add(id);
            });
        }
        public static bool ConfirmJournal(CampaignState c, string anchor) => c && Phase4Ids.AllEvidence(c.Data) &&
            Receipt(c, "P4_JOURNAL_CONFIRMED", anchor, "PHASE4_CP4");
        public static bool BeginRitual(CampaignState c, int preset)
        {
            if (!c || preset < 0 || preset > 2 || !c.Data.phase4.Completed("P4_PENDOPO_REACHED") || !c.Data.phase4.Completed("P4_BANASPATI_REVEAL") || !Phase4Ids.RitualReady(c.Data) || !Phase4Ids.AllRelics(c.Data) || c.Data.phase4.Completed("P4_RITUAL_TRIGGER")) return false;
            return c.TryCommit("PHASE4_CP5", d =>
            {
                d.phase4.Add("P4_OFFERINGS_PLACED"); d.phase4.Add("P4_RITUAL_TRIGGER"); d.phase4.torch_layout_seed = preset;
                d.inventory.Remove(Phase4Ids.Blade); d.inventory.Remove(Phase4Ids.Soil); d.phase4.safe_anchor = "PENDOPO";
                // Offering never invents water or duplicates the Kendi instance.
            });
        }
        public static bool Pour(CampaignState c, int sector, int candidate, string anchor)
        {
            if (!c || sector < 0 || sector > 3 || candidate < 0 || candidate > 2 ||
                !c.Data.phase4.Completed("P4_RITUAL_TRIGGER") || c.Data.phase4.Completed("P4_BOSS_WIN") ||
                !c.Data.inventory.Contains(Phase4Ids.Kendi) || c.Data.phase4.water_units <= 0 ||
                (sector < 3 && Phase4Ids.SectorDone(c.Data, sector)) ||
                (sector == 3 && (Phase4Ids.LocalHits(c.Data) != 3 || c.Data.phase4.Completed("P4_SOUTH_OFF")))) return false;
            bool authentic = candidate == Phase4Ids.AuthenticIndex(c.Data.phase4.torch_layout_seed, sector);
            return c.TryCommit(sector == 3 && authentic ? "PHASE4_CP6" : Checkpoint(c.Data), d =>
            {
                d.phase4.water_units--; d.phase4.water_action_sequence++; d.phase4.safe_anchor = anchor;
                if (!authentic) d.sanity = Mathf.Max(0, d.sanity - 40);
                if (sector == 3 && authentic) { d.phase4.Add("P4_SOUTH_OFF"); d.phase4.safe_anchor = "PENDOPO"; }
            });
        }
        // The existing phone owns cooldown/cost. Restore its pre-shot value only
        // for the transaction snapshot, then commit the exact already-paid value.
        public static bool Shot(CampaignState c, float before, float after, int target, string anchor)
        {
            if (!c || !c.Data.phase4.Completed("P4_RITUAL_TRIGGER") || c.Data.phase4.Completed("P4_BOSS_WIN") ||
                before < 15 || !Mathf.Approximately(after, before - 15) || target < -1 || target > 3) return false;
            if (target >= 0 && target < 3 && Phase4Ids.SectorDone(c.Data, target)) return false;
            if (target == 3 && (Phase4Ids.LocalHits(c.Data) != 3 || !c.Data.phase4.Completed("P4_SOUTH_OFF"))) return false;
            c.Data.battery = before;
            string cp = target == 3 ? "PHASE4_CP7" : target >= 0 ? "PHASE4_CP6" : Checkpoint(c.Data);
            return c.TryCommit(cp, d =>
            {
                d.battery = after; d.phase4.safe_anchor = anchor;
                if (target >= 0 && target < 3) d.phase4.Add("P4_BLITZ_" + Phase4Ids.Sectors[target]);
                if (target == 3)
                {
                    d.phase4.Add("P4_BLITZ_CORE"); d.phase4.Add("P4_BOSS_WIN"); d.phase4.safe_anchor = "PORTAL";
                    if (!d.location_seals.Contains(Phase4Ids.Seal)) d.location_seals.Add(Phase4Ids.Seal);
                }
            });
        }
        public static bool Refill(CampaignState c, string anchor)
        {
            if (!c || !c.Data.inventory.Contains(Phase4Ids.Kendi) || c.Data.phase4.water_units >= 3) return false;
            return c.TryCommit(Checkpoint(c.Data), d => { d.phase4.water_units = 3; d.phase4.safe_anchor = Anchor(d, anchor); });
        }
        public static bool Salt(CampaignState c, string anchor)
        {
            if (!c || !c.Data.inventory.Contains(Phase4Ids.Kendi) || c.Data.phase4.water_units >= 3) return false;
            string item = c.Data.inventory.Find(id => id.StartsWith("P4_GARAM_")); if (item == null) return false;
            return c.TryCommit(Checkpoint(c.Data), d => { d.inventory.Remove(item); d.phase4.water_units = 3; d.phase4.safe_anchor = Anchor(d, anchor); });
        }
        public static bool Incense(CampaignState c, string anchor)
        {
            if (!c || c.Data.phase4.elapsed_gameplay_seconds < c.Data.phase4.incense_ready_at || c.Data.sanity >= 100) return false;
            string item = c.Data.inventory.Find(Phase4Ids.IsIncense); if (item == null) return false;
            return c.TryCommit(Checkpoint(c.Data), d =>
            { d.inventory.Remove(item); d.sanity = 100; d.phase4.incense_ready_at = d.phase4.elapsed_gameplay_seconds + 12; d.phase4.safe_anchor = Anchor(d, anchor); });
        }
        public static bool Recover(CampaignState c, string sourceId, string anchor, bool battery, float cooldown = 20)
        {
            if (!c || (battery ? c.Data.battery >= 100 : c.Data.sanity >= 100)) return false;
            var old = c.Data.phase4.recovery_last_uses.Find(x => x.id == sourceId);
            if (old != null && c.Data.phase4.elapsed_gameplay_seconds - old.at_gameplay_seconds < cooldown) return false;
            return c.TryCommit(Checkpoint(c.Data), d =>
            {
                if (battery) d.battery = Mathf.Min(100, d.battery + 25); else d.sanity = Mathf.Min(100, d.sanity + 15);
                var use = d.phase4.recovery_last_uses.Find(x => x.id == sourceId);
                if (use == null) d.phase4.recovery_last_uses.Add(new Phase1DynamoUse { id = sourceId, at_gameplay_seconds = d.phase4.elapsed_gameplay_seconds });
                else use.at_gameplay_seconds = d.phase4.elapsed_gameplay_seconds;
                d.phase4.safe_anchor = Anchor(d, anchor);
            });
        }
        public static bool CollectMirror(CampaignState c, string anchor)
        {
            if (!c || !new[] { "P4_SECRET_KNOCK_VERIFIED", "P4_SECRET_EYE_VERIFIED", "P4_SECRET_WHEEL_ALIGNED" }.All(c.Data.phase4.Completed)) return false;
            if (c.Data.secret_items.Contains(Phase4Ids.Secret)) return true;
            return Receipt(c, "P4_SECRET_PICKUP", anchor, null, d => d.secret_items.Add(Phase4Ids.Secret));
        }
        public static bool Finish(CampaignState c)
        {
            if (!c || !c.Data.phase4.Completed("P4_BOSS_WIN") || !c.Data.phase4.Completed("P4_FIFTH_GATE_REVEAL") ||
                !c.Data.location_seals.Contains(Phase4Ids.Seal) || c.Data.phase4.Completed("P4_COMPLETE")) return false;
            return c.TryCommit("PHASE4_CP8", d => { d.phase4.Add("P4_COMPLETE"); d.phase4.next_destination = Phase4Ids.Destination; d.phase4.safe_anchor = "BOUNDARY"; });
        }
    }
}
