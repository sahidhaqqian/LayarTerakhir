using System;
using System.Linq;
using LayarTerakhir.Core;
using UnityEngine;

namespace LayarTerakhir.Phase5
{
    // Receipts live in the existing campaign. No parallel ending/save/water owner.
    public static class Phase5Campaign
    {
        public static string Checkpoint(CampaignData d)
        {
            var p = d.phase5;
            int cp = p.Completed("P5_COMPLETE") ? 9 : p.Completed("P5_FINAL_CHOICE_ENTER") ? 8 :
                Phase5Ids.CoreCount(d) > 0 || Phase5Ids.CableCount(d) > 0 ? 7 : p.Completed("P5_ARENA_LOCK") ? 6 :
                p.Completed("P5_S3_CLEAR") ? 5 : p.Completed("P5_S2_CLEAR") ? 4 : p.Completed("P5_S1_CLEAR") ? 3 : p.Completed("P5_ENTER_SECTORS") ? 2 : 1;
            return "PHASE5_CP" + cp;
        }
        public static bool Enter(CampaignState c)
        {
            if (!c || !Phase5Ids.Eligible(c.Data)) return false;
            return c.TryCommit("PHASE5_CP1", d =>
            {
                d.chapter_id = 5; d.phase5 = new Phase5Data(); d.phase5.Add("P5_ARRIVE_GATE");
                // Battery, Sanity, all inventories, ledgers and four seals carry over unchanged.
            });
        }
        private static bool Active(CampaignState c) => c && CampaignRouting.IsPhase5(c.Data) && c.Data.ending == EndingId.NONE;
        public static bool Receipt(CampaignState c, string id, string anchor = null, Action<CampaignData> mutation = null)
        {
            if (!Active(c) || string.IsNullOrWhiteSpace(id)) return false;
            if (c.Data.phase5.Completed(id)) return true;
            return Transaction(c, d =>
            {
                d.phase5.Add(id); mutation?.Invoke(d);
                if (anchor != null) d.phase5.safe_anchor = anchor;
            });
        }
        // TryCommit takes its checkpoint argument after mutate; compute using a temporary
        // snapshot so checkpoint advancement and its receipt are one replacement on disk.
        private static bool Transaction(CampaignState c, Action<CampaignData> mutate)
        {
            if (!Active(c)) return false;
            var preview = JsonUtility.FromJson<CampaignData>(JsonUtility.ToJson(c.Data));
            mutate(preview); string cp = Checkpoint(preview);
            return c.TryCommit(cp, mutate);
        }
        public static bool StartLive(CampaignState c)
        {
            if (!Active(c) || c.Data.phase5.Completed("P5_LIVE_LOCK_1M")) return false;
            return Transaction(c, d =>
            {
                d.phase5.Add("P5_LIVE_LOCK_1M"); d.wallet_bound += 66666666;
                d.phase5.money_ledger.Add(new Phase1MoneyReceipt { id = "P5_DONATION_GATE", amount = 66666666, bound = true });
            });
        }
        public static bool EnterSectors(CampaignState c) => Active(c) && c.Data.phase5.Completed("P5_ARRIVE_GATE") &&
            Transaction(c, d => { d.phase5.Add("P5_ENTER_SECTORS"); d.phase5.safe_anchor = d.phase5.Completed("P5_S3_CLEAR") ? "STAGE" : "S1"; });
        public static bool ClearSector(CampaignState c, int sector)
        {
            if (!Active(c) || sector < 1 || sector > 3 || !c.Data.phase5.Completed("P5_ENTER_SECTORS") ||
                (sector > 1 && !c.Data.phase5.Completed("P5_S" + (sector - 1) + "_CLEAR")) ||
                (sector == 2 && !c.Data.phase5.Completed("P5_BRIDGE_TORCH_OFF"))) return false;
            if (c.Data.phase5.Completed("P5_S" + sector + "_CLEAR")) return true;
            return Transaction(c, d => { d.phase5.Add("P5_S" + sector + "_CLEAR"); d.phase5.safe_anchor = sector == 1 ? "S2" : sector == 2 ? "S3" : "STAGE"; });
        }
        public static bool Evidence(CampaignState c, string id, bool photo = false)
        {
            if (!Active(c) || !Phase5Ids.Evidence.Contains(id) || !c.Data.phase5.Completed("P5_S3_CLEAR")) return false;
            if (c.Data.evidence_ids.Contains(id)) return true;
            if (photo && c.Data.battery < 1 || id == Phase5Ids.Contract &&
                (!c.Data.phase5.Completed("P5_CONTRACT_THERMO_OK") || !c.Data.phase5.Completed("P5_CONTRACT_UV_OK"))) return false;
            return Transaction(c, d => { d.evidence_ids.Add(id); d.phase5.Add(id); if (photo) d.battery -= 1; d.phase5.safe_anchor = "STAGE"; });
        }
        public static bool ConfirmJournal(CampaignState c) => Active(c) && Phase5Ids.AllEvidence(c.Data) && Phase5Ids.FourSeals(c.Data) &&
            Receipt(c, "P5_JOURNAL_CONFIRMED", "MIRROR");
        public static bool BeginArena(CampaignState c)
        {
            if (!Active(c) || !Phase5Ids.AllEvidence(c.Data) || !Phase5Ids.FourSeals(c.Data) ||
                !c.Data.phase5.Completed("P5_JOURNAL_CONFIRMED") || c.Data.phase5.Completed("P5_ARENA_LOCK")) return false;
            return Transaction(c, d => { d.phase5.Add("P5_ARENA_LOCK"); d.phase5.safe_anchor = "ARENA_WEST"; });
        }
        public static bool RecordShot(CampaignState c, float before, float after, int core)
        {
            if (!Active(c) || !c.Data.phase5.Completed("P5_ARENA_LOCK") || c.Data.phase5.Completed("P5_FINAL_CHOICE_ENTER") ||
                before < 15 || !Mathf.Approximately(after, before - 15) || core < 0 || core > 3) return false;
            int done = Phase5Ids.CoreCount(c.Data);
            if (core > 0 && (core != done + 1 || core == 3 && Phase5Ids.CableCount(c.Data) != 3)) return false;
            c.Data.battery = before;
            return Transaction(c, d =>
            {
                d.battery = after; d.phase5.safe_anchor = "ARENA_WEST";
                if (core > 0) d.phase5.Add("P5_CORE_" + core + "_BROKEN");
                if (core == 3) d.phase5.Add("P5_DAMAR_FREED");
            });
        }
        public static bool CutCable(CampaignState c, int cable)
        {
            if (!Active(c) || cable < 1 || cable > 3 || Phase5Ids.CoreCount(c.Data) != 2 || !c.Data.phase5.Completed("P5_ARENA_LOCK")) return false;
            if (c.Data.phase5.Completed("P5_CABLE_" + cable + "_CUT")) return true;
            return Transaction(c, d => { d.phase5.Add("P5_CABLE_" + cable + "_CUT"); d.phase5.safe_anchor = "ARENA_WEST"; });
        }
        public static bool EnterFinal(CampaignState c)
        {
            if (!Active(c) || Phase5Ids.CoreCount(c.Data) != 3 || Phase5Ids.CableCount(c.Data) != 3) return false;
            if (c.Data.phase5.Completed("P5_FINAL_CHOICE_ENTER")) return true;
            return Transaction(c, d => { d.phase5.Add("P5_FINAL_CHOICE_ENTER"); d.phase5.safe_anchor = "PORTAL"; });
        }
        public static bool ValidateArchive(CampaignState c)
        {
            if (!Active(c) || !Phase5Ids.ArchiveComplete(c.Data) || !c.Data.phase5.Completed("P5_S3_CLEAR")) return false;
            return Receipt(c, "P5_ARCHIVE_VALIDATED", null, d => d.archive_prepared = true);
        }
        public static bool HelpDamar(CampaignState c) => Active(c) && Phase5Ids.FinalChoice(c.Data) && Receipt(c, "P5_DAMAR_AT_PORTAL", "PORTAL");
        public static bool LockEnding(CampaignState c, EndingId ending)
        {
            if (!Active(c) || !Phase5Ids.FinalChoice(c.Data) || ending == EndingId.NONE || !Enum.IsDefined(typeof(EndingId), ending)) return false;
            if ((ending == EndingId.TRUE || ending == EndingId.SECRET) && !c.Data.phase5.Completed("P5_DAMAR_AT_PORTAL")) return false;
            if (ending == EndingId.SECRET && (!Phase5Ids.ArchiveComplete(c.Data) || !c.Data.archive_prepared || !c.Data.phase5.Completed("P5_ARCHIVE_VALIDATED"))) return false;
            return c.TryCommit("PHASE5_CP8", d => { d.ending = ending; d.phase5.Add("P5_ENDING_LOCKED"); d.phase5.safe_anchor = "ENDING"; });
        }
        public static bool Complete(CampaignState c)
        {
            if (!c || !CampaignRouting.IsPhase5(c.Data) || c.Data.ending == EndingId.NONE || !c.Data.phase5.Completed("P5_ENDING_LOCKED")) return false;
            if (c.Data.phase5.Completed("P5_COMPLETE")) return true;
            return c.TryCommit("PHASE5_CP9", d => d.phase5.Add("P5_COMPLETE"));
        }
        public static bool CollectTape(CampaignState c)
        {
            if (!Active(c) || !new[] { "P5_SECRET_CHAIR_SILENCE", "P5_SECRET_AR_DRAWER", "P5_SECRET_EVP_TRUE" }.All(c.Data.phase5.Completed)) return false;
            if (c.Data.secret_items.Contains(Phase5Ids.Secret)) return true;
            return Receipt(c, "P5_SECRET_PICKUP", null, d => d.secret_items.Add(Phase5Ids.Secret));
        }
        public static bool EnsureVessel(CampaignState c)
        {
            if (!Active(c) || !c.Data.phase5.Completed("P5_S1_CLEAR")) return false;
            if (Phase5Ids.HasWaterVessel(c.Data)) return true;
            return Receipt(c, "P5_CUP_COLLECTED", null, d => d.inventory.Add(Phase5Ids.Cup));
        }
        public static bool Refill(CampaignState c)
        {
            if (!Active(c) || !Phase5Ids.HasWaterVessel(c.Data) || c.Data.phase4.water_units >= 3) return false;
            return Transaction(c, d => d.phase4.water_units = 3);
        }
        public static bool Pour(CampaignState c)
        {
            if (!Active(c) || !c.Data.phase5.Completed("P5_S1_CLEAR") || c.Data.phase5.Completed("P5_BRIDGE_TORCH_OFF") ||
                !Phase5Ids.HasWaterVessel(c.Data) || c.Data.phase4.water_units < 1) return false;
            return Receipt(c, "P5_BRIDGE_TORCH_OFF", null, d => { d.phase4.water_units--; d.phase4.water_action_sequence++; });
        }
        public static bool Buy(CampaignState c, string pack)
        {
            long cost; int count; string type;
            switch (pack)
            {
                case "POWERBANK": cost = 750000; count = 3; type = pack; break;
                case "FLARE": cost = 600000; count = 4; type = pack; break;
                case "KEMENYAN": cost = 700000; count = 4; type = pack; break;
                case "CHARGE": cost = 100000; count = 0; type = pack; break;
                default: return false;
            }
            if (!Active(c) || c.Data.phase5.Completed("P5_ARENA_LOCK") || c.Data.phase5.Completed("P5_SHOP_" + pack) || c.Data.wallet_legitimate < cost) return false;
            return Receipt(c, "P5_SHOP_" + pack, null, d =>
            {
                d.wallet_legitimate -= cost; d.spent += cost;
                for (int i = 0; i < count; i++) d.inventory.Add("P5_" + type + "_" + (char)('A' + i));
                if (pack == "CHARGE") d.battery = 100;
                d.phase5.money_ledger.Add(new Phase1MoneyReceipt { id = "P5_SHOP_" + pack, amount = cost, bound = false });
            });
        }
        public static bool Recover(CampaignState c, string source, string anchor, bool battery, float cooldown)
        {
            if (!Active(c) || (battery ? c.Data.battery >= 100 : c.Data.sanity >= 100)) return false;
            var p = c.Data.phase5; var last = p.recovery_last_uses.Find(x => x.id == source);
            if (last != null && p.elapsed_gameplay_seconds - last.at_gameplay_seconds < cooldown) return false;
            return Transaction(c, d =>
            {
                if (battery) d.battery = Mathf.Min(100, d.battery + 25); else d.sanity = Mathf.Min(100, d.sanity + 15);
                var r = d.phase5.recovery_last_uses.Find(x => x.id == source);
                if (r == null) d.phase5.recovery_last_uses.Add(new Phase1DynamoUse { id = source, at_gameplay_seconds = d.phase5.elapsed_gameplay_seconds });
                else r.at_gameplay_seconds = d.phase5.elapsed_gameplay_seconds;
                d.phase5.safe_anchor = anchor;
            });
        }
        public static bool Incense(CampaignState c)
        {
            if (!Active(c) || c.Data.sanity >= 100 || c.Data.phase5.elapsed_gameplay_seconds < c.Data.phase5.incense_ready_at) return false;
            string item = c.Data.inventory.Find(Phase5Ids.IsIncense); if (item == null) return false;
            return Transaction(c, d => { d.inventory.Remove(item); d.sanity = 100; d.phase5.incense_ready_at = d.phase5.elapsed_gameplay_seconds + 12; });
        }
        public static bool UseFlare(CampaignState c)
        {
            if (!Active(c)) return false;
            string item = c.Data.inventory.Find(Phase5Ids.IsFlare); if (item == null) return false;
            return Transaction(c, d => d.inventory.Remove(item));
        }
    }
}
