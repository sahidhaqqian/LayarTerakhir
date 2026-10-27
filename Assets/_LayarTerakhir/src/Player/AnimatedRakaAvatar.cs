using UnityEngine;
using LayarTerakhir.Core;
using LayarTerakhir.UI;

namespace LayarTerakhir.Player
{
    // Visual-only animation. CharacterController and story traversals retain movement ownership.
    public sealed class AnimatedRakaAvatar : MonoBehaviour
    {
        public Animation motion;
        public GameObject handset;
        public bool sourcedLocomotion;
        public float walkStrideSpeed=1.42f,runStrideSpeed=3.81f,crouchStrideSpeed=.52f,lowCrouchStrideSpeed=.405f;
        public bool Installing {get;set;}
        public bool CinematicCrouch {get;set;}
        private string current;
        private Renderer[] jacket,shirt;
        private Renderer handsetScreen;
        private float playbackSpeed = 1;
        private float handsetVisible;
        private bool handsetGraspVisible;
        private Transform[] thighs, shins, feet;
        private Transform pelvis;
        private Transform upperArmR, forearmR, handR, headBone;
        private PhoneDesktop phoneDesktop;
        private float[] soleHeights;
        private Quaternion[] flatFootRotations;
        private void Awake()
        {
            var renderers=GetComponentsInChildren<Renderer>(true);
            jacket=System.Array.FindAll(renderers,r=>r.name=="RakaJacketOutfit");
            shirt=System.Array.FindAll(renderers,r=>r.name.StartsWith("RakaTShirt"));
            handsetScreen=System.Array.Find(renderers,r=>r.name=="HandsetScreen");
            PrepareFootContacts();
        }

        private bool armBonesCached;

        public void PrepareFootContacts()
        {
            if(feet==null)
            {
                var bones=GetComponentsInChildren<Transform>(true);
                pelvis=System.Array.Find(bones,b=>b.name=="Hips");
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
            if(!armBonesCached)
            {
                armBonesCached=true;
                var bones=GetComponentsInChildren<Transform>(true);
                upperArmR=System.Array.Find(bones,b=>b.name=="UpperArm_R");
                forearmR=System.Array.Find(bones,b=>b.name=="Forearm_R")??System.Array.Find(bones,b=>b.name=="LowerArm_R");
                handR=System.Array.Find(bones,b=>b.name=="Hand_R");
                headBone=System.Array.Find(bones,b=>b.name=="Head");
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
            if(sourcedLocomotion&&pelvis&&feet[0]&&feet[1])
            {
                // A crouch has no aerial phase. Retargeting heel/toe contacts
                // and blending loops can lift both flat soles at once. Settle
                // only the visual pelvis onto the lower supporting foot;
                // leave the CharacterController/root and headroom unchanged.
                float supportLift=float.PositiveInfinity;
                for(int i=0;i<2;i++)supportLift=Mathf.Min(supportLift,feet[i].position.y-(transform.position.y+soleHeights[i]*transform.lossyScale.y));
                if(supportLift>0)pelvis.position-=Vector3.up*Mathf.Min(supportLift,.12f);
            }
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
                Quaternion authoredFootRotation=feet[i].rotation;
                MotorcycleRiderAlignment.Solve(thighs[i],shins[i],feet[i],target,pole);
                feet[i].rotation=sourcedLocomotion?authoredFootRotation:transform.rotation*flatFootRotations[i];
            }
        }

        // Post-clip corrections run after legacy animation sampling (LateUpdate)
        // and before the rig's own LateUpdate adjustments.
        private void LateUpdate()
        {
            if(!motion||current==null)return;
            bool locomotion=current.EndsWith("Walk")||current.EndsWith("Run");
            bool crouchBlend=false;
            if(locomotion)
                foreach(AnimationState state in motion)
                    if(state.enabled&&state.weight>.001f&&state.name.Contains("Crouch")){crouchBlend=true;break;}
            // Walk/run knee plane: keep the baked stride and foot plants, but pin
            // the knee pole outward so knock-kneed clips read as a natural gait.
            // Crouch blending stays owned by AlignCrouchFootContacts.
            if(locomotion&&!crouchBlend)SpreadKnees();
            ApplyCallPose();
        }

        private void SpreadKnees()
        {
            PrepareFootContacts();
            if(pelvis==null)return;
            for(int i=0;i<2;i++)
            {
                if(!thighs[i]||!shins[i]||!feet[i])continue;
                Vector3 target=feet[i].position;
                Vector3 pole=thighs[i].position+transform.right*(i==0?-1f:1f)*.45f+transform.forward*.25f+transform.up*.1f;
                Quaternion authoredFootRotation=feet[i].rotation;
                MotorcycleRiderAlignment.Solve(thighs[i],shins[i],feet[i],target,pole);
                feet[i].rotation=sourcedLocomotion?authoredFootRotation:transform.rotation*flatFootRotations[i];
            }
        }

        // Phone calls hold the handset to the ear instead of the generic
        // hold-up-to-look clip: solve the right arm to the character's own
        // ear side with a down-back elbow so the elbow stops flaring out.
        private void ApplyCallPose()
        {
            if(phoneDesktop==null)phoneDesktop=Object.FindFirstObjectByType<PhoneDesktop>();
            if(phoneDesktop==null||!phoneDesktop.CallActive||current==null||!current.StartsWith("Phone"))return;
            if(!armBonesCached)PrepareFootContacts();
            if(!upperArmR||!forearmR||!handR||!headBone)return;
            float sideSign=Mathf.Sign(transform.InverseTransformPoint(handR.position).x);
            if(sideSign==0)sideSign=1;
            Vector3 ear=headBone.position+headBone.right*sideSign*.09f+headBone.up*.02f+transform.forward*.04f;
            Vector3 pole=upperArmR.position+transform.up*-.35f-transform.right*sideSign*.15f+transform.forward*-.15f;
            MotorcycleRiderAlignment.Solve(upperArmR,forearmR,handR,ear,pole);
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
            if (handsetScreen) handsetScreen.gameObject.SetActive(handsetGraspVisible);
            bool moving = speed > (IsLocomotion(current) ? .055f : .12f) && speed < 12;
            bool running = speed > (current != null && current.EndsWith("Run") ? 2.9f : 3.25f);
            string clip = Installing ? "InstallCard" : riding ? "Riding" : crouching ? (moving ? "CrouchWalk" : "Crouch") : moving ? (running ? "Run" : "Walk") : "Idle";
            if (crouching && lowCrouch && !Installing && !riding) clip = moving ? "LowCrouchWalk" : "LowCrouch";
            if (heldPose && !riding && !Installing) clip = "Phone" + clip;
            var state = motion[clip]; if (!state) return;
            float strideScale = Mathf.Max(.1f, Mathf.Abs(transform.localScale.x));
            float targetSpeed = !moving || riding || Installing ? 1 : Mathf.Clamp(speed / ((crouching ? lowCrouch ? lowCrouchStrideSpeed : crouchStrideSpeed : running ? runStrideSpeed : walkStrideSpeed) * strideScale), .12f, 2.2f);
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
