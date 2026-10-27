using LayarTerakhir.Core;

namespace LayarTerakhir.Interlude
{
    // Daytime interlude between the Prologue finale and Phase 1.
    // All progress lives in the existing chapter-zero campaign: inventory
    // event flags plus three checkpoints. No schema change, no second save.
    public static class InterludeIds
    {
        public const string MorningCheckpoint = "INTERLUDE_MORNING";
        public const string DayCheckpoint = "INTERLUDE_DAY";
        public const string ReadyCheckpoint = "INTERLUDE_READY";

        public const string RatnaTalk = "EVENT_INTERLUDE_RATNA";
        public const string HookRead = "EVENT_INTERLUDE_HOOK";
        public const string WarungVisit = "EVENT_INTERLUDE_WARUNG";
        public const string NeighborTalk = "EVENT_INTERLUDE_NEIGHBOR";
        public const string AnomalySeen = "EVENT_INTERLUDE_ANOMALY";
        public const string Charged = "EVENT_INTERLUDE_CHARGED";
        public const string Woke = "EVENT_INTERLUDE_WOKE";

        public static bool IsInterlude(string checkpointId) =>
            checkpointId == MorningCheckpoint || checkpointId == DayCheckpoint || checkpointId == ReadyCheckpoint;

        public static bool IsInterludeData(CampaignData data) =>
            data != null && data.chapter_id == 0 && IsInterlude(data.checkpoint_id);
    }
}
