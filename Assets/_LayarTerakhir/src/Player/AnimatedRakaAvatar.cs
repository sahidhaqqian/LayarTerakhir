using UnityEngine;
using LayarTerakhir.Core;

namespace LayarTerakhir.Player
{
    // Visual-only animation. CharacterController and story traversals retain movement ownership.
    public sealed class AnimatedRakaAvatar : MonoBehaviour
    {
        public Animation motion;
        public GameObject handset;
        public bool Installing {get;set;}
        private string current;
        private Renderer[] jacket,shirt;
        private float playbackSpeed = 1;
        private float handsetVisible;
        private bool handsetGraspVisible;
        private void Awake()
        {
            var renderers=GetComponentsInChildren<Renderer>(true);
            jacket=System.Array.FindAll(renderers,r=>r.name=="RakaJacketOutfit");
            shirt=System.Array.FindAll(renderers,r=>r.name.StartsWith("RakaTShirt"));
        }

        public void Present(float speed, bool crouching, bool phone, bool riding, Vector2 direction = default, bool lowCrouch = false)
        {
            if (!motion) return;
            RefreshOutfit();
            handsetVisible = Mathf.MoveTowards(handsetVisible, phone && !riding ? 1 : 0, Time.deltaTime / .22f);
            bool heldPose = phone || handsetVisible > .02f;
            // Display the world handset only after the grasp has been sampled.
            // Keep that grasp while lowering, until the handset leaves the hand.
            bool graspReady = current != null && current.StartsWith("Phone") && motion[current] != null && motion[current].weight > .98f;
            handsetGraspVisible = heldPose && !riding && (handsetGraspVisible || graspReady);
            if (handset) handset.SetActive(handsetGraspVisible);
            bool moving = speed > (IsLocomotion(current) ? .055f : .12f) && speed < 12;
            bool running = speed > (current != null && current.EndsWith("Run") ? 2.9f : 3.25f);
            string clip = Installing ? "InstallCard" : riding ? "Riding" : crouching ? (moving ? "CrouchWalk" : "Crouch") : moving ? (running ? "Run" : "Walk") : "Idle";
            if (crouching && lowCrouch && !Installing && !riding) clip = moving ? "LowCrouchWalk" : "LowCrouch";
            if (heldPose && !riding && !Installing) clip = "Phone" + clip;
            var state = motion[clip]; if (!state) return;
            float strideScale = Mathf.Max(.1f, Mathf.Abs(transform.localScale.x));
            float targetSpeed = !moving || riding || Installing ? 1 : Mathf.Clamp(speed / ((crouching ? lowCrouch ? .405f : .52f : running ? 3.81f : 1.42f) * strideScale), .12f, 2.2f);
            if (moving && direction.y < -.0001f) targetSpeed = -targetSpeed;
            playbackSpeed = Mathf.MoveTowards(playbackSpeed, targetSpeed, 12 * Time.deltaTime);
            state.speed = playbackSpeed;
            if (clip == current && motion.IsPlaying(clip)) return;
            state.wrapMode = Installing ? WrapMode.ClampForever : WrapMode.Loop;
            bool phaseMatch = IsLocomotion(clip) && IsLocomotion(current) && motion[current] != null;
            float phase = phaseMatch ? Mathf.Repeat(motion[current].normalizedTime, 1) : 0;
            motion.CrossFade(clip, crouching || heldPose ? .24f : .18f);
            if (phaseMatch) state.normalizedTime = phase;
            current = clip;
        }
        private static bool IsLocomotion(string clip) => clip != null && (clip.EndsWith("Walk") || clip.EndsWith("Run"));
        public bool TryGetFootstepPhase(out float phase, out float reverseContact, out bool reverse)
        {
            phase=0;reverseContact=.58f;reverse=playbackSpeed<0;
            if(!motion||!IsLocomotion(current)||!motion.IsPlaying(current))return false;
            var state=motion[current];if(!state)return false;
            phase=Mathf.Repeat(state.normalizedTime,1);
            // Authored stance starts at 0/.5, not .25/.75. In reverse playback
            // the landing is at the opposite end of the planted interval.
            reverseContact=current.EndsWith("Run")?.46f:current.Contains("Crouch")?.64f:.58f;
            return true;
        }
        public void RefreshOutfit(bool visible=true)
        {
            bool wearingJacket=CampaignState.Instance&&CampaignState.Instance.HasInventoryItem("JAKET_RAKA");
            foreach(var renderer in jacket)renderer.enabled=visible&&wearingJacket;
            foreach(var renderer in shirt)renderer.enabled=visible&&!wearingJacket;
        }
    }
}
