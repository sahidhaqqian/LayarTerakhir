using UnityEngine;
namespace LayarTerakhir.Phase5
{
    [CreateAssetMenu(menuName = "LayarTerakhir/Phase5 Balance")]
    public sealed class Phase5Balance : ScriptableObject
    {
        public float gateGazeDrain = .06f, sectorGazeDrain = .05f, arenaGazeDrain = .04f;
        public float gateFloor = 50, sectorFloor = 45, arenaFloor = 45;
        public float chargeHold = 8, prayerHold = 10, prayerCooldown = 30, chargeCooldown = 20, refillHold = 4;
        public float thermalDwell = 2, photoDwell = 1, evpSeconds = 6, evpRadius = 3, opticalRange = 8;
        public float silenceSeconds = 3, archiveHold = 3, cableHold = 2, incenseHold = 2, incenseClarity = 20;
        public float s1GhostSpeed = 1.3f, selfieLook = 1, selfiePause = 4, selfieCooldown = 6;
        public float sriSpeed = 1.4f, giantSpeed = 1.8f, hearingWalk = 4, hearingSprint = 9, hearingCrouch = 1.5f, screenRange = 12;
        public float searchTime = 6, lungeTelegraph = 1.2f, contactSanityLoss = 25, contactIFrame = 3, contactWindow = 10, restoreGrace = 8;
        public float hotDoorTelegraph = 1, hotDoorLoss = 25, plankTelegraph = 1, plankLoss = 10;
        public float bossSpeed = 2.2f, shadowTouchLoss = 15, shadowRespawn = 6, generatorRadius = 6, generatorCooldown = 12, exposureSeconds = 5;
        public float rearStalkSeconds = 6, rearColdRange = 2.2f, flareRadius = 4, flareDuration = 10, lanternCooldown = 15;
        public float waveTelegraph = 2, waveSweep = 4, waveGap = 12, coreRetry = 8;
        public int waveCount = 3;
        public float badDwell = 2, badLabelMinimum = .5f, normalHold = 5, syncSegment = 1.5f, escortSpeed = 1.1f;
    }
}
