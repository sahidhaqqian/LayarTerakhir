using System.Collections.Generic;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    // Readable attack/recoil poses independent of AI navigation and checkpoint state.
    public sealed class AtmajaThreatPresentation : MonoBehaviour
    {
        public AtmajaDirector director;
        public Transform body, weapon;
        public Transform leftArm,rightArm,leftLeg,rightLeg,head;
        public bool HasCharacterRig=>leftArm&&rightArm;
        public bool ExternalStun { get; set; }
        public ParticleSystem blitzSmoke;
        public Animation authoredMotion;
        [Min(.1f)] public float walkStrideSpeed=.9f, runStrideSpeed=2f;
        public string CurrentMotionClip=>currentClip;
        public void EmitPhotoSmoke(int count)
        {
            if(!blitzSmoke)return;
            var main=blitzSmoke.main;main.maxParticles=Mathf.Max(main.maxParticles,180);
            main.startSize=new ParticleSystem.MinMaxCurve(.16f,.48f);
            main.startSpeed=new ParticleSystem.MinMaxCurve(.18f,.55f);
            main.startLifetime=new ParticleSystem.MinMaxCurve(1.1f,1.9f);
            main.startColor=new ParticleSystem.MinMaxGradient(new Color(.84f,.86f,.82f,.62f),new Color(.58f,.62f,.60f,.42f));
            if(!blitzSmoke.isPlaying)blitzSmoke.Play();
            blitzSmoke.Emit(Mathf.Clamp(count,1,60));
        }
        private Quaternion bodyBase, weaponBase;
        private Vector3 bodyPosition, weaponPosition, previous;
        private float stride;
        private bool ready, wasStunned;
        private Quaternion leftArmBase,rightArmBase,leftLegBase,rightLegBase,headBase;
        private string currentClip;
        private float movementSpeed, movementHold;
        private bool running;
        public float SampledHuntPoseTime {get;private set;}
        private readonly List<Material> woundMaterials=new List<Material>();
        private void OnEnable()
        {
            previous=transform.position;movementSpeed=movementHold=0;running=false;currentClip=null;
        }
        private void Start()
        {
            ApplyBloodWounds();
            if (!body) return;
            bodyBase = body.localRotation;bodyPosition = body.localPosition;
            if(weapon){weaponBase = weapon.localRotation;weaponPosition = weapon.localPosition;}
            if(authoredMotion)
            {
                authoredMotion.cullingType=AnimationCullingType.AlwaysAnimate;
                foreach(var skin in authoredMotion.GetComponentsInChildren<SkinnedMeshRenderer>(true))skin.updateWhenOffscreen=true;
            }
            previous = transform.position; ready = true;
            if(HasCharacterRig){leftArmBase=leftArm.localRotation;rightArmBase=rightArm.localRotation;leftLegBase=leftLeg.localRotation;rightLegBase=rightLeg.localRotation;headBase=head.localRotation;}
        }
        private void ApplyBloodWounds()
        {
            var texture=Resources.Load<Texture2D>("PlayerPresentation/NyonyaWound");
            if(!texture)return;
            var clones=new Dictionary<Material,Material>();
            foreach(var renderer in GetComponentsInChildren<Renderer>(true))
            {
                var materials=renderer.sharedMaterials;
                for(int i=0;i<materials.Length;i++)
                {
                    var source=materials[i];if(!source)continue;
                    string name=source.name;
                    if(!(name.Contains("Sri_Cloth")||name.Contains("Sri_Batik")||name.Contains("Sri_Skin")))continue;
                    if(!clones.TryGetValue(source,out var copy))
                    {
                        copy=new Material(source){name=source.name+" (wounds)"};
                        copy.SetTexture("_BaseMap",texture);
                        copy.SetColor("_BaseColor",source.GetColor("_BaseColor"));
                        clones.Add(source,copy);woundMaterials.Add(copy);
                    }
                    materials[i]=copy;
                }
                renderer.sharedMaterials=materials;
            }
        }
        private void OnDestroy(){foreach(var material in woundMaterials)if(material)Destroy(material);}
        private void LateUpdate()
        {
            if (!ready || !director || !director.chase) return;
            var chase = director.chase;
            bool windup = chase.AwaitingBlitz && movementSpeed < .08f, recoil = chase.IsStunned || ExternalStun;
            bool hunting = director.basement && director.basement.IsHunting || chase.IsPursuing;
            Vector3 displacement=transform.position-previous;displacement.y=0;previous=transform.position;
            float moved=displacement.magnitude,dt=Time.deltaTime;
            // Checkpoint relocation is not a step. Keep navigation and collision
            // entirely owned by the existing AI; only read its planar movement.
            if(moved>.6f){moved=0;movementSpeed=movementHold=0;}
            float velocity=dt>.0001f?moved/dt:0;
            movementSpeed=Mathf.Lerp(movementSpeed,velocity,1-Mathf.Exp(-12*dt));
            if(moved>.0001f)movementHold=.18f;else movementHold=Mathf.Max(0,movementHold-dt);
            bool moving=movementHold>0&&movementSpeed>.08f;
            running=moving&&(running?movementSpeed>1.55f:movementSpeed>1.8f);
            if (moved < .6f) stride += moved * 9;
            float motion = moving ? 1 : 0;
            float poseTime = Time.time;
            if(authoredMotion)
            {
                string clip=recoil?"Stun":windup?"Windup":moving?(running?"Run":"Walk"):"Idle";
                var state=authoredMotion[clip];
                if(state)
                {
                    state.wrapMode=recoil||windup?WrapMode.ClampForever:WrapMode.Loop;
                    bool gait=clip=="Walk"||clip=="Run";
                    float speed=gait?Mathf.Clamp(movementSpeed/Mathf.Max(.1f,clip=="Run"?runStrideSpeed:walkStrideSpeed),.35f,2.4f):1f;
                    if(currentClip!=clip||!authoredMotion.IsPlaying(clip))
                    {
                        var old=string.IsNullOrEmpty(currentClip)?null:authoredMotion[currentClip];
                        bool oldGait=currentClip=="Walk"||currentClip=="Run";
                        float phase=oldGait&&old?Mathf.Repeat(old.normalizedTime,1):0;
                        authoredMotion.CrossFade(clip,recoil||windup?.07f:.2f);
                        state.normalizedTime=gait&&oldGait?phase:0;currentClip=clip;
                    }
                    state.speed=speed;
                    SampledHuntPoseTime=state.time;
                }
                if(blitzSmoke&&recoil&&!wasStunned)blitzSmoke.Play();wasStunned=recoil;
                return;
            }
            Quaternion lean = Quaternion.Euler(recoil ? -19 : windup ? 12 : Mathf.Sin(stride) * 2 * motion, 0, recoil ? 13 : Mathf.Sin(stride * .5f) * 2 * motion);
            body.localRotation = Quaternion.Slerp(body.localRotation, bodyBase * lean, Time.unscaledDeltaTime * 5);
            body.localPosition = bodyPosition + Vector3.up * (Mathf.Sin(stride * 2) * .004f * motion);
            if(HasCharacterRig)
            {
                float swing=Mathf.Sin(stride)*12*motion;
                float shear=hunting?Mathf.Sin(poseTime*4.6f)*17f:0;
                leftArm.localRotation=Quaternion.Slerp(leftArm.localRotation,leftArmBase*Quaternion.Euler(recoil?-28:-swing,0,recoil?-18:0),Time.unscaledDeltaTime*6);
                rightArm.localRotation=Quaternion.Slerp(rightArm.localRotation,rightArmBase*Quaternion.Euler(windup?-115:recoil?-38:swing*.65f-shear*.35f,0,windup?-17:shear*.2f),Time.unscaledDeltaTime*5);
                leftLeg.localRotation=leftLegBase*Quaternion.Euler(swing,0,0);rightLeg.localRotation=rightLegBase*Quaternion.Euler(-swing,0,0);
                float headTilt=hunting?68f+Mathf.Sin(poseTime*2.1f)*11f:0;
                head.localRotation=headBase*Quaternion.Euler(Mathf.Sin(poseTime*2.5f)*.8f,0,headTilt+Mathf.Sin(stride*.5f)*.7f);
                if(weapon){weapon.localPosition=weaponPosition;weapon.localRotation=weaponBase*Quaternion.Euler(0,0,shear);}
            }
            else if(weapon)
            {
                weapon.localPosition = Vector3.Lerp(weapon.localPosition, weaponPosition + (windup ? Vector3.up * .65f : recoil ? Vector3.down * .2f : Vector3.zero), Time.unscaledDeltaTime * 5);
                weapon.localRotation = Quaternion.Slerp(weapon.localRotation, weaponBase * Quaternion.Euler(windup ? -105 : recoil ? 35 : Mathf.Sin(stride) * 8, 0, windup ? -25 : 0), Time.unscaledDeltaTime * 5);
            }
            if (blitzSmoke && recoil && !wasStunned) blitzSmoke.Play();
            wasStunned = recoil;
        }
    }
}
