using System.Collections;
using UnityEngine;
namespace LayarTerakhir.Prolog
{
    public sealed class DoubleLeafGate : MonoBehaviour
    {
        public Transform leftLeaf,rightLeaf;
        public bool IsOpen {get;private set;}
        public void SetImmediate(bool open)
        {
            IsOpen=open;
            leftLeaf.localRotation=Quaternion.Euler(0,open?-100:0,0);
            rightLeaf.localRotation=Quaternion.Euler(0,open?100:0,0);
        }
        public IEnumerator Open()
        {
            var left=leftLeaf.localRotation;var right=rightLeaf.localRotation;
            for(float t=0;t<1;t+=Time.deltaTime/1.3f)
            {
                float k=Mathf.SmoothStep(0,1,t);
                leftLeaf.localRotation=Quaternion.Slerp(left,Quaternion.Euler(0,-100,0),k);
                rightLeaf.localRotation=Quaternion.Slerp(right,Quaternion.Euler(0,100,0),k);
                yield return null;
            }
            SetImmediate(true);
        }
    }
}
