using LayarTerakhir.Player;
using UnityEngine;
namespace LayarTerakhir.Phase2
{
    public sealed class Phase2WorldItem : MonoBehaviour, IInteractable
    {
        public Phase2Director director;
        public Phase2Hub hub;
        public string id, prompt;
        public string InteractableId => id;
        public string GetInteractionPrompt() => director ? director.ItemPrompt(this) : prompt;
        public bool CanInteract() => director ? director.CanInteract(this) : hub && hub.Ready;
        public void Interact() { if (CanInteract()) { if (director) director.Interact(this); else hub.Interact(id); } }
    }
}
