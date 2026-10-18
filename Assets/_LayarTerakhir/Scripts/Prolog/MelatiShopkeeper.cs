using UnityEngine;
using LayarTerakhir.Player;

namespace LayarTerakhir.Prolog
{
    public sealed class MelatiShopkeeper : MonoBehaviour,IInteractable
    {
        public Animation motion;
        public Transform head,player;
        private Quaternion rest;
        private MelatiShop shop;
        public string InteractableId=>"BU_SRI";
        public bool CanInteract()=>shop&&shop.CanInteract();
        public string GetInteractionPrompt()=>"Bicara dengan Bu Sri / belanja";
        public void Interact(){if(CanInteract())shop.Interact();}
        private void Start(){shop=GetComponentInParent<MelatiNeighborhood>().GetComponentInChildren<MelatiShop>();rest=head.localRotation;if(motion)motion.Play("Idle");}
        private void LateUpdate()
        {
            if(!head||!player)return;
            Vector3 direction=player.position+Vector3.up*1.45f-head.position;
            if(direction.magnitude>5)return;
            Vector3 local=transform.InverseTransformDirection(direction);
            float yaw=Mathf.Clamp(Mathf.Atan2(local.x,local.z)*Mathf.Rad2Deg,-28,28);
            head.localRotation=rest*Quaternion.Euler(0,yaw,0);
        }
    }
}
