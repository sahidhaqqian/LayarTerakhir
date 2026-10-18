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
        private void Awake()
        {
            var renderers=GetComponentsInChildren<Renderer>(true);
            jacket=System.Array.FindAll(renderers,r=>r.name=="RakaJacketOutfit");
            shirt=System.Array.FindAll(renderers,r=>r.name=="RakaTShirt");
        }

        public void Present(float speed, bool crouching, bool phone, bool riding)
        {
            if (!motion) return;
            RefreshOutfit();
            if (handset) handset.SetActive(phone && !riding);
            bool moving = speed > .12f && speed < 12;
            string clip = Installing ? "InstallCard" : riding ? "Riding" : crouching ? (moving ? "CrouchWalk" : "Crouch") : moving ? (speed > 3.25f ? "Run" : "Walk") : "Idle";
            if (phone && !riding && !Installing) clip = "Phone" + clip;
            var state = motion[clip]; if (!state) return;
            state.speed = !moving || riding || Installing ? 1 : Mathf.Clamp(speed / (crouching ? .73f : clip.EndsWith("Run") ? 3.8f : 1.4f), .65f, 2.2f);
            if (clip == current && motion.IsPlaying(clip)) return;
            state.wrapMode = Installing ? WrapMode.ClampForever : WrapMode.Loop;
            motion.CrossFade(clip, .16f); current = clip;
        }
        public void RefreshOutfit(bool visible=true)
        {
            bool wearingJacket=CampaignState.Instance&&CampaignState.Instance.HasInventoryItem("JAKET_RAKA");
            foreach(var renderer in jacket)renderer.enabled=visible&&wearingJacket;
            foreach(var renderer in shirt)renderer.enabled=visible&&!wearingJacket;
        }
    }
}
