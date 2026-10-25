using LayarTerakhir.Smartphone;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed class Phase5SpectralPresentation : MonoBehaviour
    {
        public Phase5Director director;
        private void LateUpdate()
        {
            if (!director || !director.Ready || !director.phoneView.FeedCamera) return;
            var phone = director.Phone; bool screen = phone.isPhoneRaised && phone.isScreenOn && phone.HasCameraPower;
            var main = director.phoneView.playerCamera; var feed = director.phoneView.FeedCamera;
            main.cullingMask &= ~((1 << 24) | (1 << 29) | (1 << 25));
            int spectral = (screen && phone.currentScanMode == ScanVisualMode.UV ? 1 << 24 : 0) |
                (screen && phone.currentScanMode == ScanVisualMode.AR ? 1 << 29 : 0) |
                (screen && phone.isSelfieCamera && director.boss.Phase == 2 ? 1 << 31 : 0);
            feed.cullingMask = (feed.cullingMask & ~((1 << 24) | (1 << 29) | (1 << 25))) | spectral;
            foreach (var mark in director.world.uvMarks) if (mark) mark.SetActive(screen && phone.currentScanMode == ScanVisualMode.UV);
            foreach (var mark in director.world.arMarks) if (mark) mark.SetActive(screen && phone.currentScanMode == ScanVisualMode.AR);
            if (phone.phoneFlashlight) phone.phoneFlashlight.color = screen && phone.currentScanMode == ScanVisualMode.UV ? new Color(.58f, .36f, 1) : new Color(.88f, .94f, 1);
            // Only the rear-stalking apparition belongs to existing Selfie-only layer25.
            int layer = director.boss.Active && director.boss.Phase == 2 ? 25 : 0;
            foreach (var renderer in director.world.bossRoot.GetComponentsInChildren<Renderer>(true)) renderer.gameObject.layer = layer;
        }
    }
}
