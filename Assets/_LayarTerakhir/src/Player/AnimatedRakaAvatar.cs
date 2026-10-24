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
        public bool CinematicCrouch {get;set;}
        private string current;
        private Renderer[] jacket,shirt;
        private float playbackSpeed = 1;
        private float handsetVisible;
        private bool handsetGraspVisible;
        private Transform[] thighs, shins, feet;
        private float[] soleHeights;
        private Quaternion[] flatFootRotations;
        private void Awake()
        {
            var renderers=GetComponentsInChildren<Renderer>(true);
            jacket=System.Array.FindAll(renderers,r=>r.name=="RakaJacketOutfit");
            shirt=System.Array.FindAll(renderers,r=>r.name.StartsWith("RakaTShirt"));
            PrepareFootContacts();
        }

        public void PrepareFootContacts()
        {
            if(feet!=null)return;
            var bones=GetComponentsInChildren<Transform>(true);
            thighs=new Transform[2];shins=new Transform[2];feet=new Transform[2];
            soleHeights=new float[2];flatFootRotations=new Quaternion[2];
            for(int i=0;i<2;i++)
            {
                string side=i==0?"L":"R";
                thighs[i]=System.Array.Find(bones,b=>b.name=="Thigh_"+side);
                shins[i]=System.Array.Find(bones,b=>b.name=="Shin_"+side);
                feet[i]=System.Array.Find(bones,b=>b.name=="Foot_"+side);
                if(!feet[i])continue;
                soleHeights[i]=transform.InverseTransformPoint(feet[i].position).y;
                flatFootRotations[i]=Quaternion.Inverse(transform.rotation)*feet[i].rotation;
            }
        }

        // Legacy quaternion crossfades do not preserve the baked foot contacts.
        // Keep the visual ankles above their bind-pose sole height and solve the
        // same two-bone chain already used by the rider. Never move the capsule.
        public void AlignCrouchFootContacts(bool crouching,bool lowCrouch)
        {
            if(!motion)return;
            bool blendingCrouch=crouching;
            foreach(AnimationState state in motion)
                if(state.enabled&&state.weight>.001f&&state.name.Contains("Crouch"))blendingCrouch=true;
            if(!blendingCrouch)return;
            PrepareFootContacts();
            for(int i=0;i<2;i++)
            {
                if(!thighs[i]||!shins[i]||!feet[i])continue;
                Vector3 target=feet[i].position;
                target.y=Mathf.Max(target.y,transform.position.y+soleHeights[i]*transform.lossyScale.y);
                // Retain the authored knee plane, including its turn/stride.
                // A fixed pole can roll baggy trousers under a deep-squat thigh.
                Vector3 pole=shins[i].position;
                if(Vector3.ProjectOnPlane(pole-thighs[i].position,target-thighs[i].position).sqrMagnitude<.000001f)
                    pole=transform.TransformPoint(new Vector3(i==0?.18f:-.18f,lowCrouch?1:.524f,1.2f));
                MotorcycleRiderAlignment.Solve(thighs[i],shins[i],feet[i],target,pole);
                feet[i].rotation=transform.rotation*flatFootRotations[i];
            }
        }

        public void Present(float speed, bool crouching, bool phone, bool riding, Vector2 direction = default, bool lowCrouch = false)
        {
            if (!motion) return;
            RefreshOutfit();
            crouching |= CinematicCrouch;
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
