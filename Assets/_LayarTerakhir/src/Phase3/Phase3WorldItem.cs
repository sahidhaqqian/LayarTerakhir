using LayarTerakhir.Player;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public sealed class Phase3WorldItem : MonoBehaviour, IInteractable
    {
        public Phase3Director director;
        public Phase3Hub hub;
        public string id, prompt;
        public string InteractableId => id;
        public string GetInteractionPrompt() => director ? director.ItemPrompt(this) : prompt;
        public bool CanInteract() => director ? director.CanInteract(this) : hub && hub.Ready;
        public void Interact() { if (CanInteract()) { if (director) director.Interact(this); else hub.Interact(id); } }
    }
}
