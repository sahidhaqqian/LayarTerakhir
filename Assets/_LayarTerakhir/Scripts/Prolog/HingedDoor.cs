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
        public bool IsOpen=>opened;
        private AudioSource source;
        public string InteractableId=>name;
        public bool CanInteract()=>!moving&&!UiFocus.BlocksWorld&&!HudController.Instance.IsModalOpen;
        public string GetInteractionPrompt()=>locked?"Pintu kamar tertutup":opened?"Tutup pintu":label;
        public void Interact()
        {
            if(!CanInteract())return;
            if(locked){HudController.Instance.ShowBroadcastNotification("Kamar penghuni lain. Pintunya terkunci.",3);return;}
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
            for(float t=0;t<1;t+=Time.deltaTime){hinge.localRotation=Quaternion.Slerp(from,to,Mathf.SmoothStep(0,1,t));yield return null;}
            hinge.localRotation=to;moving=false;
        }
    }
}
