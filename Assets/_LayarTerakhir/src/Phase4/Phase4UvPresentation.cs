using LayarTerakhir.Smartphone;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public sealed class Phase4UvPresentation : MonoBehaviour
    {
        public Phase4Director director;
        public Renderer[] marks;
        private void LateUpdate()
        {
            if (!director || !director.Ready) return;
            var p = director.Phone;
            bool uv = p.isPhoneRaised && p.isScreenOn && p.batteryPercent > 0 && p.currentScanMode == ScanVisualMode.UV;
            foreach (var r in marks) if (r) r.enabled = uv && (r.bounds.center - director.player.cameraPivot.position).sqrMagnitude <= 64;
            if (p.phoneFlashlight) { p.phoneFlashlight.color = uv ? new Color(.63f, .38f, 1) : new Color(.88f, .94f, 1); p.phoneFlashlight.range = uv ? 8 : 16; }
        }
    }
}
