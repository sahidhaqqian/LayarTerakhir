using LayarTerakhir.Player;
using UnityEngine;
namespace LayarTerakhir.Phase4
{
    public sealed class Phase4WorldItem : MonoBehaviour, IInteractable
    {
        public Phase4Director director;
        public Phase4Hub hub;
        public string id, prompt;
        public string InteractableId => id;
        public bool CanInteract() => director ? director.CanInteract(this) : hub && hub.Ready;
        public string GetInteractionPrompt() => director ? director.Prompt(this) : prompt;
        public void Interact() { if (!CanInteract()) return; if (director) director.Interact(this); else hub.Interact(id); }
    }
}
