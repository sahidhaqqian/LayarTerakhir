using UnityEngine;

namespace LayarTerakhir.Player
{
    // Visual-only animation. CharacterController and story traversals retain movement ownership.
    public sealed class AnimatedRakaAvatar : MonoBehaviour
    {
        public Animation motion;
        public GameObject handset;
        private string current;

        public void Present(float speed, bool crouching, bool phone, bool riding)
        {
            if (!motion) return;
            if (handset) handset.SetActive(phone && !riding);
            bool moving = speed > .12f && speed < 12;
            string clip = riding ? "Riding" : crouching ? (moving ? "CrouchWalk" : "Crouch") : moving ? (speed > 3.25f ? "Run" : "Walk") : "Idle";
            if (phone && !riding) clip = "Phone" + clip;
            var state = motion[clip]; if (!state) return;
            state.speed = !moving || riding ? 1 : Mathf.Clamp(speed / (crouching ? .85f : clip.EndsWith("Run") ? 3.2f : 1.4f), .65f, 1.8f);
            if (clip == current && motion.IsPlaying(clip)) return;
            state.wrapMode = WrapMode.Loop;
            motion.CrossFade(clip, .16f); current = clip;
        }
    }
}
