using UnityEngine;
namespace LayarTerakhir.Phase3
{
    [CreateAssetMenu(menuName="LayarTerakhir/Phase3 Balance")]
    public sealed class Phase3Balance : ScriptableObject
    {
        public float photoDwell=1, evpTap=.25f, evpRange=2, evpSeconds=6, realSeconds=2, secretRecordSeconds=1.5f;
        public float fakeSegment=8, realSegment=3, analyzerRange=15, searchDelay=3.5f;
        public float crankSeconds=1.5f, dynamoSeconds=8, emergencySeconds=45, emergencyCooldown=120;
        public float fragileTelegraph=.6f, learnTelegraph=1.2f, learnWindow=30, dynamicSpawnDistance=6, safeWidth=1.4f;
        public float patrolSpeed=1.2f, ceilingSpeed=1.8f, chaseSpeed=3, grabRange=1.4f, grabTelegraph=.8f, loiterSeconds=6;
        public float screenRadius=12, revealDistance=12, lureCooldown=25, learnSpeed=.7f;
        public float prepSeconds=30, recoverySeconds=20, exposedSeconds=5, stalkSpeed=2;
        public float beamRadius=2.5f, beamSeconds=2.5f, beamCooldown=12, aimRange=8, aimAngle=20;
        public float powerSeconds=8, powerAmount=25, resetSeconds=8, resetCooldown=20;
        public float flareRange=8, flareSeconds=20, flareRadius=3, flareDwell=.3f;
        public float pullSpeed=1, pullLimit=12, pullUvSeconds=1, pullTelegraph=1.5f;
    }
}
