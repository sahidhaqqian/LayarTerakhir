using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed class Phase5MechanicalPresentation : MonoBehaviour
    {
        public Phase5Director director;
        public Transform wheel;
        public Transform[] mirrors;
        private Vector3[] starts;
        private void Start(){if(mirrors!=null)starts=System.Array.ConvertAll(mirrors,t=>t.localPosition);}
        private void LateUpdate()
        {
            if(!director||!director.ClockRunning)return;
            if(wheel)wheel.Rotate(Vector3.right,-4*Time.deltaTime,Space.Self);
            if(mirrors==null||starts==null)return;
            for(int i=0;i<mirrors.Length;i++)if(mirrors[i])mirrors[i].localPosition=starts[i]+Vector3.up*Mathf.Sin(director.Elapsed*.45f+i*1.17f)*.12f;
        }
    }
}
