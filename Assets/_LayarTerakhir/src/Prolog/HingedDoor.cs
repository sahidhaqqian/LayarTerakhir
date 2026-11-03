using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Prolog
{
    public sealed class HingedDoor:MonoBehaviour,IInteractable
    {
        public Transform hinge;public string label="Buka pintu";public bool locked;public string lockedPrompt="Pintu kamar tertutup";public float angle=95;private bool opened,moving;
        public string lockedDialogue;
        public bool requiresCrouchAndAR;
        // Designer Nov: one-way pry (rusted vent). When set, the first
        // unlocked interact rips the leaf off for good — persisted via the
        // flag, restored as gone — instead of swinging open/closed forever.
        public string pryFlag;
        public string pryToast;
        public bool slides;public Vector3 slideOffset=Vector3.forward*1.6f;
        private Vector3 closedPosition;private Quaternion closedRotation;private bool positionCaptured;
        public bool IsOpen=>opened;
        private AudioSource source;
        public string InteractableId=>name;
        public bool CanInteract()=>!moving&&!UiFocus.HardModal&&!UiFocus.Cinematic&&!(HudController.Instance&&HudController.Instance.IsModalOpen)&&(!requiresCrouchAndAR||VentReady());
        private bool VentReady()
        {
            var player=FindFirstObjectByType<FirstPersonController>();var phone=LayarTerakhir.Smartphone.SmartphoneController.Instance;
            return player&&player.IsCrouching&&phone&&phone.isPhoneRaised&&phone.isScreenOn&&phone.currentScanMode==LayarTerakhir.Smartphone.ScanVisualMode.AR;
        }
        public string GetInteractionPrompt()=>requiresCrouchAndAR?string.Empty:locked?lockedPrompt:opened?"Tutup pintu":label;
        public void Interact()
        {
            if(!CanInteract())return;
            CapturePosition();
            if(locked)
            {
                // Designer Nov: a prized/rusted vent must TELL the player a tool
                // is needed. Pry-gated leaves show Raka's first-person line as a
                // subtitle AND a persistent toast so the hint is never missed.
                if(!string.IsNullOrEmpty(pryFlag))
                {
                    if(HudController.Instance)
                    {
                        if(!string.IsNullOrEmpty(lockedDialogue))HudController.Instance.EnqueueSubtitle("Raka",lockedDialogue,4);
                        HudController.Instance.ShowBroadcastNotification(!string.IsNullOrEmpty(lockedPrompt)?lockedPrompt:"Cari alat untuk membukanya.",4);
                    }
                    return;
                }
                if(!string.IsNullOrEmpty(lockedDialogue))HudController.Instance.EnqueueSubtitle("Raka",lockedDialogue,4);else HudController.Instance.ShowBroadcastNotification("Pintunya terkunci.",3);return;
            }
            if(!string.IsNullOrEmpty(pryFlag))
            {
                var campaign=LayarTerakhir.Core.CampaignState.Instance;
                if(campaign&&!campaign.HasInventoryItem(pryFlag))
                {
                    campaign.AddInventoryItem(pryFlag);
                    campaign.SaveCheckpointAtomic(campaign.Data.checkpoint_id);
                    if(!string.IsNullOrEmpty(pryToast))HudController.Instance.ShowBroadcastNotification(pryToast,4);
                    gameObject.SetActive(false);
                    return;
                }
            }
            opened=!opened;StartCoroutine(Swing());
        }
        private IEnumerator Swing()
        {
            moving=true;
            var profile=PresentationProfile.Load()?.audio;
            if(profile)
            {
                if(!source){source=gameObject.AddComponent<AudioSource>();source.playOnAwake=false;source.spatialBlend=.9f;source.minDistance=1.5f;source.maxDistance=8;source.outputAudioMixerGroup=profile.effectsBus;source.dopplerLevel=0;}
                if(profile.latch)source.PlayOneShot(profile.latch,.55f);
            }
            var from=hinge.localRotation;var to=closedRotation*Quaternion.Euler(0,opened?angle:0,0);
            var start=hinge.localPosition;var end=closedPosition+(opened?slideOffset:Vector3.zero);
            for(float t=0;t<1;t+=Time.deltaTime){float k=Mathf.SmoothStep(0,1,t);if(slides)hinge.localPosition=Vector3.Lerp(start,end,k);else hinge.localRotation=Quaternion.Slerp(from,to,k);yield return null;}
            if(slides)hinge.localPosition=end;else hinge.localRotation=to;moving=false;
        }
        private void CapturePosition(){if(!positionCaptured&&hinge){closedPosition=hinge.localPosition;closedRotation=hinge.localRotation;positionCaptured=true;}}
        public void SetOpenImmediate(bool open)
        {
            CapturePosition();opened=open;moving=false;
            if(slides)hinge.localPosition=closedPosition+(open?slideOffset:Vector3.zero);
            else hinge.localRotation=closedRotation*Quaternion.Euler(0,open?angle:0,0);
        }
        public void SetOpenAnimated(bool open)
        {
            if(opened==open)return;CapturePosition();StopAllCoroutines();opened=open;
            if(isActiveAndEnabled)StartCoroutine(Swing());else SetOpenImmediate(open);
        }
    }
}
