using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public sealed class Phase3Flare : MonoBehaviour
    {
        public Phase3Director director;
        public Vector3 start,target;
        public bool Eligible=true;
        public bool Landed { get; private set; }
        private float age;
        private void Update()
        {
            if(!director||!director.ClockRunning)return;
            age+=Time.deltaTime;
            if(age<.65f){float t=age/.65f;transform.position=Vector3.Lerp(start,target,t)+Vector3.up*Mathf.Sin(t*Mathf.PI)*1.3f;}
            else{Landed=true;transform.position=target;}
            if(age>director.balance.flareSeconds)Destroy(gameObject);
        }
    }
}
