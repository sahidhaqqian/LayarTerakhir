using System;
using LayarTerakhir.Core;

namespace LayarTerakhir.Interlude
{
    // Single transaction owner for the interlude. Mirrors Phase2Campaign:
    // every mutation goes through CampaignState.TryCommit on the shared file.
    public static class InterludeCampaign
    {
        public static bool EnterInterlude(CampaignState campaign)
        {
            if (campaign == null || campaign.Data.chapter_id != 0 ||
                campaign.Data.checkpoint_id != "PROLOG_COMPLETE") return false;
            bool entered = campaign.TryCommit(InterludeIds.MorningCheckpoint, d =>
            {
                if (!d.inventory.Contains(InterludeIds.MorningCheckpoint))
                    d.inventory.Add(InterludeIds.MorningCheckpoint);
            });
            if (entered) campaign.SnapshotPhaseEntry();
            return entered;
        }

        public static bool NoteBeat(CampaignState campaign, string beat)
        {
            if (campaign == null || !InterludeIds.IsInterludeData(campaign.Data) ||
                string.IsNullOrWhiteSpace(beat)) return false;
            if (campaign.Data.inventory.Contains(beat)) return true;
            return campaign.TryCommit(campaign.Data.checkpoint_id, d =>
            {
                if (!d.inventory.Contains(beat)) d.inventory.Add(beat);
            });
        }

        public static bool HasBeat(CampaignData data, string beat) =>
            data != null && data.inventory.Contains(beat);

        public static bool ReadyToRest(CampaignData data) =>
            InterludeIds.IsInterludeData(data) &&
            HasBeat(data, InterludeIds.RatnaTalk) && HasBeat(data, InterludeIds.HookRead);

        public static string MissingForRest(CampaignData data)
        {
            if (!HasBeat(data, InterludeIds.RatnaTalk)) return "Sapalah Bu Haji di halaman kos dulu.";
            if (!HasBeat(data, InterludeIds.HookRead)) return "Ada pesan belum dibaca di HP (TAB, lalu Pesan).";
            return string.Empty;
        }

        public static bool ChargeHub(CampaignState campaign)
        {
            if (campaign == null || !InterludeIds.IsInterludeData(campaign.Data) ||
                campaign.Data.inventory.Contains(InterludeIds.Charged)) return false;
            return campaign.TryCommit(campaign.Data.checkpoint_id, d =>
            {
                d.battery = 100;
                if (!d.inventory.Contains(InterludeIds.Charged)) d.inventory.Add(InterludeIds.Charged);
            });
        }

        // Player-controlled time skip: rest in Raka's room once the day's
        // required beats are done. No real-time day/night waiting.
        public static bool RestUntilNight(CampaignState campaign)
        {
            if (campaign == null || !ReadyToRest(campaign.Data) ||
                campaign.Data.checkpoint_id == InterludeIds.ReadyCheckpoint) return false;
            return campaign.TryCommit(InterludeIds.ReadyCheckpoint, d =>
            {
                d.battery = 100;
                if (!d.inventory.Contains(InterludeIds.ReadyCheckpoint))
                    d.inventory.Add(InterludeIds.ReadyCheckpoint);
            });
        }
    }
}
