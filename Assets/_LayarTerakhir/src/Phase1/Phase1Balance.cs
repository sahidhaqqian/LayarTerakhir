using UnityEngine;
namespace LayarTerakhir.Phase1
{
    [UnityEngine.CreateAssetMenu(menuName = "LayarTerakhir/Phase 1 balance")]
    public sealed class Phase1Balance : UnityEngine.ScriptableObject
    {
        public float kuntilanakSpeed = 2.4f, kuntilanakCatchRadius = 1.2f, unwatchedGrace = 1.5f, chainSeconds = 2;
        public float patrolSpeed = 1.3f, chaseSpeed = 3, emergencyStunSeconds = 3;
        public float exposedSeconds = 5, flashRange = 8, beamRadius = 2.5f;
        public float evpSeconds = 7, bathroomGraceSeconds = 3.5f, bathroomSearchSeconds = 9;
        public float tuyulDrainPerSecond = 2, tuyulSeconds = 10;
        public float dynamoSeconds = 8, dynamoAmount = 25, crankSeconds = 45, crankCooldownSeconds = 120;
        public float arenaChargeSeconds = 8, arenaChargeAmount = 25, bossRecoverySeconds = 12;
    }
}
