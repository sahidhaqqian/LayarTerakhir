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
            // Standing locomotion is retargeted offline using each leg's bend
            // plane. Re-solving it against a fixed lateral pole twists the knees.
            ApplyCallPose();
        }

        [SerializeField,HideInInspector]private bool callContactCached;
        [SerializeField,HideInInspector]private Quaternion phoneFrameInHand,headFrameInBone;
        [SerializeField,HideInInspector]private Vector3 screenOffsetInHand,headCentreInBone;
        [SerializeField,HideInInspector]private float headHalfWidth,callSide;
        private float callBlend;
        private float browseBlend;

        // Derive contact frames from the actual skinned handset/head. Moving the
        // wrist to an assumed ear point ignores the grip offset and twists it.
#if UNITY_EDITOR
        // Bake metadata in the editor; imported body meshes stay non-readable in
        // players. No CPU mesh copies or bone-weight reads are needed at runtime.
        public bool BakeCallContactProfile(AnimatedRakaAvatar recipient)
        {
            Awake();
            var clip=motion?motion.GetClip("PhoneIdle"):null;if(!clip)return false;
            clip.SampleAnimation(gameObject,0);handset.SetActive(true);
            callContactCached=false;if(!CacheCallContact())return false;
            recipient.callContactCached=true;recipient.phoneFrameInHand=phoneFrameInHand;recipient.headFrameInBone=headFrameInBone;
            recipient.screenOffsetInHand=screenOffsetInHand;recipient.headCentreInBone=headCentreInBone;
            recipient.headHalfWidth=headHalfWidth;recipient.callSide=callSide;return true;
        }
        private bool CacheCallContact()
        {
            if(!handset||!handR||!headBone)return false;
            var phone=handset.GetComponent<SkinnedMeshRenderer>();
            var glass=handsetScreen;
            var skin=System.Array.Find(GetComponentsInChildren<SkinnedMeshRenderer>(),r=>r.name=="VisibleSkin");
            if(!phone||!glass||!skin)return false;
            var mesh=new Mesh();
            try
            {
                phone.BakeMesh(mesh,true);var corners=new System.Collections.Generic.List<Vector3>();
                foreach(var v in mesh.vertices)
                {
                    Vector3 point=phone.transform.TransformPoint(v);
                    if(!corners.Exists(p=>(p-point).sqrMagnitude<1e-10f))corners.Add(point);
                }
                if(corners.Count!=8)return false;
                var edges=new System.Collections.Generic.List<Vector3>();
                var candidates=corners.GetRange(1,7);candidates.Sort((a,b)=>(a-corners[0]).sqrMagnitude.CompareTo((b-corners[0]).sqrMagnitude));
                foreach(var point in candidates)
                {
                    Vector3 edge=point-corners[0];
                    if(edges.TrueForAll(e=>Mathf.Abs(Vector3.Dot(e.normalized,edge.normalized))<.01f))edges.Add(edge);
                }
                if(edges.Count!=3)return false;
                edges.Sort((a,b)=>a.sqrMagnitude.CompareTo(b.sqrMagnitude));
                Vector3 centre=Vector3.zero;foreach(var point in corners)centre+=point/8;
                mesh.Clear();Vector3[] glassVertices;
                if(glass is SkinnedMeshRenderer skinnedGlass){skinnedGlass.BakeMesh(mesh,true);glassVertices=mesh.vertices;}
                else{var filter=glass.GetComponent<MeshFilter>();if(!filter||!filter.sharedMesh)return false;glassVertices=filter.sharedMesh.vertices;}
                Vector3 screen=Vector3.zero;
                foreach(var v in glassVertices)screen+=glass.transform.TransformPoint(v)/glassVertices.Length;
                Vector3 normal=edges[0].normalized,up=edges[2].normalized;
                if(Vector3.Dot(normal,screen-centre)<0)normal=-normal;
                if(Vector3.Dot(up,transform.up)<0)up=-up;
                phoneFrameInHand=Quaternion.Inverse(handR.rotation)*Quaternion.LookRotation(normal,up);
                screenOffsetInHand=Quaternion.Inverse(handR.rotation)*(screen-handR.position)/Mathf.Abs(transform.lossyScale.x);
                mesh.Clear();skin.BakeMesh(mesh,true);var vertices=mesh.vertices;var weights=skin.sharedMesh.boneWeights;
                int headIndex=System.Array.IndexOf(skin.bones,headBone);if(headIndex<0)return false;
                Bounds bounds=default;bool found=false;
                for(int i=0;i<vertices.Length;i++)
                {
                    var w=weights[i];float headWeight=(w.boneIndex0==headIndex?w.weight0:0)+(w.boneIndex1==headIndex?w.weight1:0)+(w.boneIndex2==headIndex?w.weight2:0)+(w.boneIndex3==headIndex?w.weight3:0);
                    if(headWeight<.5f)continue;
                    var point=transform.InverseTransformPoint(skin.transform.TransformPoint(vertices[i]));
                    // Head weights also own hanging hair/hood geometry below
                    // the skull. Ear contact uses the skull above its bone pivot.
                    if(point.y<transform.InverseTransformPoint(headBone.position).y)continue;
                    if(!found){bounds=new Bounds(point,Vector3.zero);found=true;}else bounds.Encapsulate(point);
                }
                if(!found)return false;
                headCentreInBone=headBone.InverseTransformPoint(transform.TransformPoint(bounds.center));
                headHalfWidth=bounds.extents.x;
                headFrameInBone=Quaternion.Inverse(headBone.rotation)*transform.rotation;
                callSide=Mathf.Sign(transform.InverseTransformPoint(upperArmR.position).x);
                callContactCached=true;return true;
            }
            finally{DestroyImmediate(mesh);}
        }
#endif
        private void ApplyCallPose()
        {
            if(phoneDesktop==null)phoneDesktop=Object.FindFirstObjectByType<PhoneDesktop>();
            bool inPhoneState=current!=null&&current.StartsWith("Phone");
            bool calling=phoneDesktop&&phoneDesktop.IsCalling&&inPhoneState;
            callBlend=Mathf.MoveTowards(callBlend,calling?1:0,Time.deltaTime/.24f);
            bool portraitBrowse=inPhoneState&&!calling&&!UiFocus.PhoneAiming&&(phoneDesktop==null||phoneDesktop.CurrentApp!=PhoneApp.Camera||UiFocus.Phone);
            browseBlend=Mathf.MoveTowards(browseBlend,portraitBrowse?1:0,Time.deltaTime/.22f);
            if(callBlend<=0&&browseBlend<=0)return;
            if(!armBonesCached)PrepareFootContacts();
            if(!upperArmR||!forearmR||!handR||!headBone||!callContactCached)return;
            float scale=Mathf.Abs(transform.lossyScale.x);
            if(browseBlend>0)
            {
                Quaternion bodyFrame=transform.rotation;
                Vector3 bRight=bodyFrame*Vector3.right,bUp=bodyFrame*Vector3.up,bFwd=bodyFrame*Vector3.forward;
                Vector3 screenTarget=upperArmR.position-bUp*(.21f*scale)+bFwd*(.26f*scale)-bRight*callSide*(.055f*scale);
                Vector3 screenNormal=(-bFwd*.76f+bUp*.65f).normalized;
                Vector3 phoneUp=(bUp*.76f+bFwd*.65f).normalized;
                Quaternion browseHandRot=Quaternion.LookRotation(screenNormal,phoneUp)*Quaternion.Inverse(phoneFrameInHand);
                Vector3 browseWrist=screenTarget-browseHandRot*(screenOffsetInHand*scale);
                Vector3 browsePole=upperArmR.position-bUp*(.36f*scale)-bFwd*(.06f*scale)+bRight*callSide*(.07f*scale);
                var bRot=Quaternion.Slerp(handR.rotation,browseHandRot,browseBlend);
                MotorcycleRiderAlignment.Solve(upperArmR,forearmR,handR,Vector3.Lerp(handR.position,browseWrist,browseBlend),browsePole);
                handR.rotation=bRot;
            }
            if(callBlend>0)
            {
                Quaternion headFrame=headBone.rotation*headFrameInBone;
                Vector3 right=headFrame*Vector3.right,up=headFrame*Vector3.up;
                Vector3 ear=headBone.TransformPoint(headCentreInBone)+right*callSide*(headHalfWidth*scale+.004f);
                Quaternion handRotation=Quaternion.LookRotation(-right*callSide,up)*Quaternion.Inverse(phoneFrameInHand);
                Vector3 wrist=ear-handRotation*(screenOffsetInHand*scale);
                Vector3 pole=upperArmR.position-up*.35f+(headFrame*Vector3.forward)*.15f+right*callSide*.1f;
                var rotation=Quaternion.Slerp(handR.rotation,handRotation,callBlend);
                MotorcycleRiderAlignment.Solve(upperArmR,forearmR,handR,Vector3.Lerp(handR.position,wrist,callBlend),pole);
                handR.rotation=rotation;
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
