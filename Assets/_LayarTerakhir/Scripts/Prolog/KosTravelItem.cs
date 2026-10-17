using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Prolog
{
    public sealed class KosTravelItem:MonoBehaviour,IInteractable
    {
        public KosCorridorDirector director;public string label="Pilih tujuan perjalanan";
        public string InteractableId=>name;
        public bool CanInteract()=>director&&!director.Busy&&!UiFocus.HardModal&&!UiFocus.Cinematic&&!(HudController.Instance&&HudController.Instance.IsModalOpen);
        public string GetInteractionPrompt()=>label;
        public void Interact(){if(CanInteract())director.OpenMap();}
    }
}
