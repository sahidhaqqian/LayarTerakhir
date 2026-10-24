using UnityEngine;
namespace LayarTerakhir.Phase2
{
    [CreateAssetMenu(menuName = "LayarTerakhir/Phase 2 balance")]
    public sealed class Phase2Balance : ScriptableObject
    {
        public float poliDwell = 1.5f, drawerDwell = 1, auraDwell = 1, evpSeconds = 6;
        public float thermalAngle = 15, drawerRange = 1.5f, poliRange = 3, evpRange = 2;
        public float learnSeconds = 30, holdSeconds = 5, holdMinSpeed = .8f;
        public float touchRise = 10, touchFall = 25, hairSeconds = 2, turnHold = .25f, turnAngle = 50;
        public float moanInterval = 20, clearedInterval = 45, telegraphSeconds = 3, hideSeconds = 6;
        public float l2Interval = 12, l2Telegraph = 2.5f, l2TouchRise = 12, darkMultiplier = 1.5f, flickerSeconds = 2;
        public float uiGrace = 4, inFrameRatio = .3f, occlusionGrace = .25f, trailDistance = 1.7f;
        public float formalinTelegraph = 1.5f, formalinSeconds = 6, formalinRadius = 2, formalinMoveMultiplier = .8f;
        public float dynamoSeconds = 8, dynamoAmount = 25, crankSeconds = 45, crankCooldown = 120;
        public float bossPrepSeconds = 30, bossRecoverySeconds = 20, bossExposedSeconds = 5;
        public float bossStalkSpeed = 2.1f, bossTrailDistance = 2.5f, bossTrailDelay = .8f;
        public float bossIdleSeconds = 3, bossLungeSeconds = 1, bossTouchRise = 15, bossTurnGrace = .5f;
        public float lampBurstSeconds = 2, lampCooldownSeconds = 12, operationRadius = 3, uvRadius = 2;
        public float flareRadius = 3, flareSeconds = 10, flareDwell = .3f;
        public float illusionRespawnSeconds = 6, illusionPopRange = 1.5f, bossAimRange = 8, bossAimAngle = 20;
        public float generatorSeconds = 8, generatorAmount = 25, generatorRadius = 1.6f;
    }
}
