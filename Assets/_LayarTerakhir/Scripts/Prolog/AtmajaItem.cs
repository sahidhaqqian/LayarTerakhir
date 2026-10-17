using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public enum AtmajaAction { Gate,Survey,FrontDoor,KitchenDoor,DamarPhone,Well,ClockHands,Clock,StairDoor,Charge,Window,CupPhoto,Diary,BathCurtain,BathScratches,WillLetter,RopePhoto,CarpetLift,Trapdoor,Powerbank,Handycam,WardrobeHide }
    public sealed class AtmajaItem:MonoBehaviour,IInteractable
    {
        public AtmajaDirector director;
        public AtmajaAction action;
        public string label;
        public string InteractableId=>"ATMAJA_"+action;
        public string GetInteractionPrompt()=>label;
        public bool CanInteract()=>enabled&&director&&!director.Busy&&!(HudController.Instance&&HudController.Instance.IsModalOpen);
        public void Interact(){if(CanInteract())director.Interact(this);}
    }
}
