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
        private float huntPoseClock;
        public float SampledHuntPoseTime {get;private set;}
        private readonly List<Material> woundMaterials=new List<Material>();
        private void Start()
        {
            ApplyBloodWounds();
            if (!body || !weapon) return;
            bodyBase = body.localRotation; weaponBase = weapon.localRotation;
            bodyPosition = body.localPosition; weaponPosition = weapon.localPosition;
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
            bool windup = chase.AwaitingBlitz, recoil = chase.IsStunned || ExternalStun;
            bool hunting = director.basement && director.basement.IsHunting || chase.IsPursuing;
            float moved = Vector3.Distance(previous, transform.position); previous = transform.position;
            if (moved < .6f) stride += moved * 9;
            float motion = moved > .001f ? 1 : 0;
            // Her body advances in 12 fps steps with an asymmetric shears arm.
            float poseTime = hunting ? Mathf.Floor(Time.unscaledTime * 12f) / 12f : Time.unscaledTime;
            if(authoredMotion)
            {
                string clip=recoil?"Stun":windup?"Windup":motion>0?(chase.IsPursuing||chase.IsGateRun?"Run":"Walk"):"Idle";
                var state=authoredMotion[clip];
                if(state)
                {
                    if(weapon)weapon.localRotation=weaponBase;
                    state.wrapMode=recoil||windup?WrapMode.ClampForever:WrapMode.Loop;
                    float speed=clip=="Walk"||clip=="Run"?Mathf.Clamp(moved/Mathf.Max(.001f,Time.deltaTime)/(clip=="Run"?3.2f:.9f),.65f,1.5f):1f;
                    if(currentClip!=clip||!authoredMotion.IsPlaying(clip)){authoredMotion.CrossFade(clip,.18f);currentClip=clip;huntPoseClock=0;}
                    if(hunting&&!recoil&&!windup)
                    {
                        huntPoseClock+=Time.deltaTime*speed;
                        SampledHuntPoseTime=Mathf.Floor(huntPoseClock*12f)/12f;
                        state.speed=0;state.time=SampledHuntPoseTime;
                        authoredMotion.Sample();
                        // Authored clips already contain the tilted head. Sample
                        // their pose without doubling that bend on the live rig.
                        if(weapon)weapon.localRotation*=Quaternion.Euler(0,0,Mathf.Sin(poseTime*4.6f)*17f);
                    }
                    else state.speed=speed;
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
                weapon.localPosition=weaponPosition;weapon.localRotation=weaponBase*Quaternion.Euler(0,0,shear);
            }
            else
            {
                weapon.localPosition = Vector3.Lerp(weapon.localPosition, weaponPosition + (windup ? Vector3.up * .65f : recoil ? Vector3.down * .2f : Vector3.zero), Time.unscaledDeltaTime * 5);
                weapon.localRotation = Quaternion.Slerp(weapon.localRotation, weaponBase * Quaternion.Euler(windup ? -105 : recoil ? 35 : Mathf.Sin(stride) * 8, 0, windup ? -25 : 0), Time.unscaledDeltaTime * 5);
            }
            if (blitzSmoke && recoil && !wasStunned) blitzSmoke.Play();
            wasStunned = recoil;
        }
    }
}
