using LayarTerakhir.Player;
using UnityEngine;

namespace LayarTerakhir.Interlude
{
    // Stationary daytime resident. Cloned at runtime from the licensed
    // warung vendor model with a retinted outfit; no new art is authored.
    public sealed class InterludeNpc : MonoBehaviour, IInteractable
    {
        public InterludeDirector director;
        public string id, prompt;
        public Transform head;
        public Transform player;
        private Quaternion rest;
        private bool settled;

        public string InteractableId => id;
        public string GetInteractionPrompt() => prompt;
        public bool CanInteract() => director && director.Ready && director.CanUse(id);
        public void Interact() { if (CanInteract()) director.Interact(id); }

        private void Start()
        {
            var motion = GetComponent<Animation>();
            if (motion) motion.Play("Idle");
            if (head) rest = head.localRotation;
            settled = true;
        }

        private void LateUpdate()
        {
            if (!settled || !head || !player) return;
            Vector3 direction = player.position + Vector3.up * 1.45f - head.position;
            if (direction.magnitude > 6) return;
            Vector3 local = transform.InverseTransformDirection(direction);
            float yaw = Mathf.Clamp(Mathf.Atan2(local.x, local.z) * Mathf.Rad2Deg, -28, 28);
            head.localRotation = rest * Quaternion.Euler(0, yaw, 0);
        }
    }
}
