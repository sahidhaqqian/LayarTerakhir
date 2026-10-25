using UnityEngine;
namespace LayarTerakhir.Phase5
{
    // Drives the existing Animator using newly licensed, authored Idle/Walk clips.
    // No procedural keyframes or additional gameplay movement owner.
    public sealed class Phase5HumanMotion : MonoBehaviour
    {
        public Animator animator;
        private Vector3 previous;
        private bool sampled;
        private static readonly int MoveSpeed=Animator.StringToHash("MoveSpeed");
        private void OnEnable(){previous=transform.position;sampled=false;}
        private void LateUpdate()
        {
            Vector3 delta=transform.position-previous;delta.y=0;previous=transform.position;
            float speed=sampled&&delta.magnitude<2&&Time.deltaTime>0?delta.magnitude/Time.deltaTime:0;sampled=true;
            if(animator)animator.SetFloat(MoveSpeed,Mathf.Clamp(speed,0,2),.12f,Time.deltaTime);
        }
    }
}
