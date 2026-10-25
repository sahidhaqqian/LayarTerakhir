using UnityEngine;
namespace LayarTerakhir.Phase4
{
    [CreateAssetMenu(menuName = "LayarTerakhir/Phase4 Balance")]
    public sealed class Phase4Balance : ScriptableObject
    {
        public float forestDrain = .1f, villageDrain = .04f, southDrain = .12f;
        public float forestFloor = 35, villageFloor = 45, shelterRate = 3, shelterCap = 70;
        public float photoDwell = 1, thermalDwell = 2, evpTap = .25f, evpSeconds = 6, knockSeconds = 1.5f;
        public float evpRange = 3, opticalRange = 7, opticalAngle = 24, uvDwell = 2;
        public float refillSeconds = 4, chargeSeconds = 8, shrineSeconds = 10, recoveryCooldown = 20;
        public float patrolSpeed = 1.5f, alertSpeed = 2.7f, chaseSpeed = 3.5f, huntSpeed = 3.7f;
        public float hearingWalk = 4, hearingSprint = 9, hearingCrouch = 1.5f, screenRadius = 12;
        public float lungeWarning = 1, lungeRadius = 1.15f, hitIFrame = 3, hitWindow = 10, searchSeconds = 6, restoreGrace = 8;
        public float exposureSeconds = 5, recoverySeconds = 8, aimRange = 8, aimAngle = 25;
        public float southEmberWarning = 1.2f, southEmberRadius = 1.6f, southEmberInterval = 9;
        public int torchPreset;
    }
}
