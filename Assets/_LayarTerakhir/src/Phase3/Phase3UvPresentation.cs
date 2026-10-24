using LayarTerakhir.Smartphone;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public sealed class Phase3UvPresentation : MonoBehaviour
    {
        public Phase3Director director;
        public Renderer[] marks;
        private void LateUpdate()
        {
            if(!director||!director.Ready)return;
            var phone=director.Phone;bool uv=phone.isPhoneRaised&&phone.isScreenOn&&phone.batteryPercent>0&&phone.currentScanMode==ScanVisualMode.UV;
            Vector3 eye=director.player.cameraPivot.position;
            foreach(var mark in marks)if(mark)mark.enabled=uv&&(mark.bounds.center-eye).sqrMagnitude<=64;
            if(phone.phoneFlashlight)
            {
                phone.phoneFlashlight.color=uv?new Color(.63f,.38f,1):new Color(.88f,.94f,1);
                phone.phoneFlashlight.range=uv?8:16;
            }
        }
    }
}
