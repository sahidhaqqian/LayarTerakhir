using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed class Phase5WorldItem : MonoBehaviour, LayarTerakhir.Player.IInteractable
    {
        public Phase5Director director;
        public string id, prompt;
        public string InteractableId => id;
        public bool CanInteract() => director && director.CanInteract(this);
        public string GetInteractionPrompt() => director ? director.Prompt(this) : prompt;
        public void Interact() { if (CanInteract()) director.Interact(this); }
    }
}
