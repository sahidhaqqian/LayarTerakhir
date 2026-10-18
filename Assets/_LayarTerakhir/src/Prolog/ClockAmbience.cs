using UnityEngine;
namespace LayarTerakhir.Prolog
{
    public sealed class ClockAmbience:MonoBehaviour
    {
        public AudioSource source;
        public Transform pendulum;
        public Vector3 pivot,axis=Vector3.forward;
        private Vector3 restPosition;private Quaternion restRotation;private float next;private AudioClip tick;
        private void Start()
        {
            if(pendulum){restPosition=pendulum.localPosition;restRotation=pendulum.localRotation;}
            const int rate=22050;var samples=new float[1323];var noise=new System.Random(345);
            for(int i=0;i<samples.Length;i++){float t=i/(float)rate;samples[i]=(Mathf.Sin(i*.46f)*.38f+(float)(noise.NextDouble()*2-1)*.22f)*Mathf.Exp(-t*100);}
            tick=AudioClip.Create("Clock_Tick_Tock",samples.Length,1,rate,false);tick.SetData(samples,0);
        }
        private void Update()
        {
            if(Time.timeScale==0)return;
            if(pendulum)
            {
                pendulum.localPosition=restPosition;pendulum.localRotation=restRotation;
                pendulum.RotateAround(transform.TransformPoint(pivot),transform.TransformDirection(axis),Mathf.Sin(Time.time*Mathf.PI)*3);
            }
            if(source&&Time.time>=next){next=Time.time+1;source.pitch=((int)Time.time%2==0)?1:.78f;source.PlayOneShot(tick,.32f);}
        }
        private void OnDestroy(){if(tick)Destroy(tick);}
    }
}
