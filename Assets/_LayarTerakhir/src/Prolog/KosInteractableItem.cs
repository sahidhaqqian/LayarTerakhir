using LayarTerakhir.Player;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public class KosInteractableItem : MonoBehaviour, IInteractable
    {
        public string itemId;
        public string promptLabel;
        public bool isInteractable = true;
        public KamarKosPrologDirector director;

        public string InteractableId => itemId;

        public string GetInteractionPrompt()
        {
            return promptLabel;
        }

        public bool CanInteract()
        {
            return isInteractable && gameObject.activeInHierarchy;
        }

        public void Interact()
        {
            if (director != null)
            {
                director.OnObjectInteracted(this);
            }
        }
    }
}
