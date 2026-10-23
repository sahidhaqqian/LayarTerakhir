using LayarTerakhir.Player;
using UnityEngine;
namespace LayarTerakhir.Phase1
{
    public sealed class Phase1WorldItem : MonoBehaviour, IInteractable
    {
        public Phase1Director director;
        public string id, prompt;
        public string InteractableId => id;
        public string GetInteractionPrompt() => director ? director.ItemPrompt(this) : prompt;
        public bool CanInteract() => director && director.CanInteract(this);
        public void Interact() { if (CanInteract()) director.Interact(this); }
    }
}
