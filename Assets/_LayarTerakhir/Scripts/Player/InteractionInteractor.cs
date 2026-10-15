using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Player
{
    public class InteractionInteractor : MonoBehaviour
    {
        [Header("Raycast Configuration")]
        public Camera playerCamera;
        public float interactionDistance = 2.6f;
        public LayerMask interactableMask = ~0;

        public IInteractable CurrentTarget { get; private set; }
        private readonly RaycastHit[] _hits = new RaycastHit[16];
        private string _lastPromptSource, _cachedPrompt;
        private KeyCode _lastPromptKey;

        private void Update()
        {
            if (HudController.Instance != null && HudController.Instance.IsModalOpen)
            {
                CurrentTarget = null;
                HudController.Instance.SetInteractionPrompt(string.Empty);
                return;
            }

            UpdateRaycastTarget();

            if (CurrentTarget != null && GameInputConfig.Instance != null &&
                GameInputConfig.Instance.GetActionDown(GameAction.Interact))
            {
                CurrentTarget.Interact();
            }
        }

        private void UpdateRaycastTarget()
        {
            CurrentTarget = null;
            if (playerCamera == null)
                return;

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            int hitCount = Physics.RaycastNonAlloc(ray, _hits, interactionDistance, interactableMask, QueryTriggerInteraction.Collide);
            RaycastHit[] hits = _hits;
            if (hitCount == _hits.Length)
            {
                hits = Physics.RaycastAll(ray, interactionDistance, interactableMask, QueryTriggerInteraction.Collide);
                hitCount = hits.Length;
            }
            RaycastHit hit = default;
            bool found = false;
            for (int i = 0; i < hitCount; i++)
            {
                if (hits[i].collider.transform.IsChildOf(transform)) continue;
                if (!found || hits[i].distance < hit.distance)
                {
                    hit = hits[i];
                    found = true;
                }
            }
            if (found)
            {
                IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
                if (interactable != null && interactable.CanInteract())
                {
                    CurrentTarget = interactable;
                    if (HudController.Instance != null)
                    {
                        string source=interactable.GetInteractionPrompt();
                        var key=GameInputConfig.Instance?GameInputConfig.Instance.interactKey:KeyCode.E;
                        if(source!=_lastPromptSource||key!=_lastPromptKey)
                        {
                            _lastPromptSource=source;_lastPromptKey=key;
                            _cachedPrompt=$"[{key}] {source}";
                        }
                        HudController.Instance.SetInteractionPrompt(_cachedPrompt);
                    }
                    return;
                }
            }

            if (HudController.Instance != null)
            {
                HudController.Instance.SetInteractionPrompt(string.Empty);
            }
        }
    }
}
