using UnityEngine;
namespace LayarTerakhir.Phase3
{
    // Presentation adapter on the chapter's downloaded sliding iron door assembly.
    public sealed class Phase3Gate : MonoBehaviour
    {
        public Phase3Director director;
        public Transform leaf;
        public Vector3 openOffset;
        public bool IsOpen { get; private set; }
        public void SetOpen(bool open,bool immediate=false)
        {
            if(IsOpen!=open&&director&&director.Ready)director.soundscape.Cue("Cloth",transform.position);
            IsOpen=open;if(immediate&&leaf)leaf.localPosition=open?openOffset:Vector3.zero;
        }
        private void Update()
        {if(leaf&&director&&director.ClockRunning)leaf.localPosition=Vector3.MoveTowards(leaf.localPosition,IsOpen?openOffset:Vector3.zero,Time.deltaTime*3.5f);}
    }
}
