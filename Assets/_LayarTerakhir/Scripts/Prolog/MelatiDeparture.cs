using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public sealed class MelatiDeparture : MonoBehaviour,IInteractable
    {
        public MelatiNeighborhood neighborhood;
        public string InteractableId=>"MELATI_ATMAJA_DEPARTURE";
        public bool CanInteract()=>neighborhood&&neighborhood.Reached&&!neighborhood.Busy&&!UiFocus.BlocksWorld;
        public string GetInteractionPrompt()=>"Ikuti jalan menuju Rumah Atmaja";
        public void Interact(){if(CanInteract())neighborhood.WalkToAtmaja();}
    }
}
