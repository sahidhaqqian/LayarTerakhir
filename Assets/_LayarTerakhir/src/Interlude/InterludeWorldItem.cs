using LayarTerakhir.Player;
using UnityEngine;

namespace LayarTerakhir.Interlude
{
    public sealed class InterludeWorldItem : MonoBehaviour, IInteractable
    {
        public InterludeHub hub;
        public InterludeDirector director;
        public string id, prompt;
        public string InteractableId => id;
        public string GetInteractionPrompt() => prompt;
        public bool CanInteract()
        {
            if (hub != null) return hub.Ready && hub.CanUse(id);
            if (director != null) return director.Ready && director.CanUse(id);
            return false;
        }
        public void Interact()
        {
            if (!CanInteract()) return;
            if (hub != null) hub.Interact(id);
            else if (director != null) director.Interact(id);
        }
    }
}
