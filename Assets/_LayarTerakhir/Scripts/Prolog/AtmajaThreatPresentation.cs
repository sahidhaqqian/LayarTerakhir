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
        public ParticleSystem blitzSmoke;
        private Quaternion bodyBase, weaponBase;
        private Vector3 bodyPosition, weaponPosition, previous;
        private float stride;
        private bool ready, wasStunned;
        private Quaternion leftArmBase,rightArmBase,leftLegBase,rightLegBase,headBase;
        private void Start()
        {
            if (!body || !weapon) return;
            bodyBase = body.localRotation; weaponBase = weapon.localRotation;
            bodyPosition = body.localPosition; weaponPosition = weapon.localPosition;
            previous = transform.position; ready = true;
            if(HasCharacterRig){leftArmBase=leftArm.localRotation;rightArmBase=rightArm.localRotation;leftLegBase=leftLeg.localRotation;rightLegBase=rightLeg.localRotation;headBase=head.localRotation;}
        }
        private void LateUpdate()
        {
            if (!ready || !director || !director.chase) return;
            var chase = director.chase;
            bool windup = chase.AwaitingBlitz, recoil = chase.IsStunned;
            float moved = Vector3.Distance(previous, transform.position); previous = transform.position;
            if (moved < .6f) stride += moved * 9;
            float motion = moved > .001f ? 1 : 0;
            Quaternion lean = Quaternion.Euler(recoil ? -19 : windup ? 12 : Mathf.Sin(stride) * 2 * motion, 0, recoil ? 13 : Mathf.Sin(stride * .5f) * 2 * motion);
            body.localRotation = Quaternion.Slerp(body.localRotation, bodyBase * lean, Time.unscaledDeltaTime * 5);
            body.localPosition = bodyPosition + Vector3.up * (Mathf.Sin(stride * 2) * .004f * motion);
            if(HasCharacterRig)
            {
                float swing=Mathf.Sin(stride)*12*motion;
                leftArm.localRotation=Quaternion.Slerp(leftArm.localRotation,leftArmBase*Quaternion.Euler(recoil?-28:-swing,0,recoil?-18:0),Time.unscaledDeltaTime*6);
                rightArm.localRotation=Quaternion.Slerp(rightArm.localRotation,rightArmBase*Quaternion.Euler(windup?-115:recoil?-38:swing*.65f,0,windup?-17:0),Time.unscaledDeltaTime*5);
                leftLeg.localRotation=leftLegBase*Quaternion.Euler(swing,0,0);rightLeg.localRotation=rightLegBase*Quaternion.Euler(-swing,0,0);
                head.localRotation=headBase*Quaternion.Euler(Mathf.Sin(Time.time*2.5f)*.8f,0,Mathf.Sin(stride*.5f)*.7f);
                weapon.localPosition=weaponPosition;weapon.localRotation=weaponBase;
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
