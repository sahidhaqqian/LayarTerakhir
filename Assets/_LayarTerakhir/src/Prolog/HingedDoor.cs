using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Prolog
{
    public sealed class HingedDoor:MonoBehaviour,IInteractable
    {
        public Transform hinge;public string label="Buka pintu";public bool locked;public float angle=95;private bool opened,moving;
        public string lockedDialogue;
        public bool slides;public Vector3 slideOffset=Vector3.forward*1.6f;
        private Vector3 closedPosition;private bool positionCaptured;
        public bool IsOpen=>opened;
        private AudioSource source;
        public string InteractableId=>name;
        public bool CanInteract()=>!moving&&!UiFocus.HardModal&&!UiFocus.Cinematic&&!(HudController.Instance&&HudController.Instance.IsModalOpen);
        public string GetInteractionPrompt()=>locked?"Pintu kamar tertutup":opened?"Tutup pintu":label;
        public void Interact()
        {
            if(!CanInteract())return;
            CapturePosition();
            if(locked){if(!string.IsNullOrEmpty(lockedDialogue))HudController.Instance.EnqueueSubtitle("Raka",lockedDialogue,4);else HudController.Instance.ShowBroadcastNotification("Pintunya terkunci.",3);return;}
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
            var from=hinge.localRotation;var to=Quaternion.Euler(0,opened?angle:0,0);
            var start=hinge.localPosition;var end=closedPosition+(opened?slideOffset:Vector3.zero);
            for(float t=0;t<1;t+=Time.deltaTime){float k=Mathf.SmoothStep(0,1,t);if(slides)hinge.localPosition=Vector3.Lerp(start,end,k);else hinge.localRotation=Quaternion.Slerp(from,to,k);yield return null;}
            if(slides)hinge.localPosition=end;else hinge.localRotation=to;moving=false;
        }
        private void CapturePosition(){if(!positionCaptured&&hinge){closedPosition=hinge.localPosition;positionCaptured=true;}}
        public void SetOpenImmediate(bool open)
        {
            CapturePosition();opened=open;moving=false;
            if(slides)hinge.localPosition=closedPosition+(open?slideOffset:Vector3.zero);
            else hinge.localRotation=Quaternion.Euler(0,open?angle:0,0);
        }
    }
}
