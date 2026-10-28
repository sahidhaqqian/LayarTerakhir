namespace LayarTerakhir.Core
{
    // Pure hide-and-listen rules shared by every locker/closet/stall beat.
    // No scene state here so EditMode tests can pin the fairness contract:
    // threat audio must fade with distance, and the bar must drain to empty
    // only when the hunter has genuinely walked away.
    public static class HidingAwareness
    {
        // Danger fill 0..1 for the threat bar: 1 while the threat is on top
        // of the hiding spot, draining through yellow to green as it leaves,
        // 0 when it is gone. Urgent (inspection/chase) pins the bar full.
        public static float ProximityFill(bool urgent, float distance, float fullRadius, float emptyRadius)
        {
            if (urgent) return 1f;
            if (distance <= fullRadius) return 1f;
            if (distance >= emptyRadius) return 0f;
            return 1f - (distance - fullRadius) / (emptyRadius - fullRadius);
        }

        // Linear falloff so receding footsteps genuinely fade; silent past audibleRadius.
        public static float ThreatGain(float distance, float audibleRadius, float maxGain)
        {
            if (distance >= audibleRadius) return 0f;
            float t = 1f - distance / audibleRadius;
            if (t < 0f) t = 0f;
            if (t > 1f) t = 1f;
            return maxGain * t;
        }
    }
}
