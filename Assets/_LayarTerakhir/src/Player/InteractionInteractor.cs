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
        public IContextInteractionResolver ContextResolver { get; set; }

        private void Update()
        {
            if (UiFocus.HardModal || UiFocus.Cinematic || (HudController.Instance != null && HudController.Instance.IsModalOpen))
            {
                CurrentTarget = null;
                if (HudController.Instance) HudController.Instance.SetInteractionPrompt(string.Empty);
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
            if(ContextResolver!=null&&ContextResolver.OwnsInteraction)
            {
                CurrentTarget=ContextResolver.ResolveContextInteraction();
                string key=GameInputConfig.Instance?GameInputConfig.Instance.GetActionLabel(GameAction.Interact):"E";
                HudController.Instance?.SetInteractionPrompt(CurrentTarget!=null?$"[{key}] {CurrentTarget.GetInteractionPrompt()}":"");
                return;
            }
            if (playerCamera == null)
                return;

            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            float cameraOffset = Vector3.Distance(playerCamera.transform.position, transform.position + Vector3.up * 1.3f);
            float rayLength = interactionDistance + cameraOffset;
            int hitCount = Physics.RaycastNonAlloc(ray, _hits, rayLength, interactableMask, QueryTriggerInteraction.Collide);
            RaycastHit[] hits = _hits;
            if (hitCount == _hits.Length)
            {
                hits = Physics.RaycastAll(ray, rayLength, interactableMask, QueryTriggerInteraction.Collide);
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
                if (interactable != null && interactable.CanInteract() &&
                    Vector3.Distance(transform.position + Vector3.up, hit.point) <= interactionDistance)
                {
                    CurrentTarget = interactable;
                    if (HudController.Instance != null)
                    {
                        string source=interactable.GetInteractionPrompt();
                        var key=GameInputConfig.Instance?GameInputConfig.Instance.interactKey:KeyCode.E;
                        if(source!=_lastPromptSource||key!=_lastPromptKey)
                        {
                            _lastPromptSource=source;_lastPromptKey=key;
                            _cachedPrompt=string.IsNullOrEmpty(source)?string.Empty:$"[{key}] {source}";
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
