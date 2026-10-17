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
        public string GetInteractionPrompt()
        {
            var campaign=LayarTerakhir.Core.CampaignState.Instance;
            if(action==AtmajaAction.Powerbank&&campaign&&campaign.HasInventoryItem("EVENT_P0_POWERBANK_TAKEN"))return "Sambungkan kembali powerbank Damar";
            if(action==AtmajaAction.Handycam&&campaign&&campaign.HasInventoryItem(AtmajaBasementHorror.HandycamId))return "Putar rekaman terakhir Damar";
            return label;
        }
        public bool CanInteract()=>enabled&&director&&!director.Busy&&(!director.basement||!director.basement.IsHidden)&&director.Stage<AtmajaStage.Chase&&!(HudController.Instance&&HudController.Instance.IsModalOpen);
        public void Interact(){if(CanInteract())director.Interact(this);}
    }
}
