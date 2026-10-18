using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using UnityEngine;
using UnityEngine.Rendering;

namespace LayarTerakhir.Prolog
{
    // A small, depth-tested glint belongs to the photographable object in the AR feed.
    public sealed class ARPhotoGlimmer : MonoBehaviour
    {
        public AtmajaUpperDirector upper;
        public string completedFlag;
        private MeshRenderer glow;
        private Material material;
        private Mesh mesh;
        public static void Attach(Transform anchor,AtmajaUpperDirector upper,string flag)
        {
            if(!anchor||anchor.GetComponentInChildren<ARPhotoGlimmer>(true))return;
            var go=new GameObject("ARPhotoGlimmer",typeof(MeshFilter),typeof(MeshRenderer));
            go.transform.SetParent(anchor,false);go.layer=AtmajaPhoneView.ARLayer;
            var glimmer=go.AddComponent<ARPhotoGlimmer>();glimmer.upper=upper;glimmer.completedFlag=flag;
        }
        private void Start()
        {
            mesh=new Mesh{name="AR glint"};mesh.vertices=new[]{new Vector3(0,.055f,0),new Vector3(.017f,0,0),new Vector3(0,-.055f,0),new Vector3(-.017f,0,0),new Vector3(-.04f,0,0),new Vector3(0,.009f,0),new Vector3(.04f,0,0),new Vector3(0,-.009f,0)};
            mesh.triangles=new[]{0,1,2,0,2,3,4,5,6,4,6,7};mesh.RecalculateBounds();
            GetComponent<MeshFilter>().sharedMesh=mesh;glow=GetComponent<MeshRenderer>();
            material=new Material(Shader.Find("Universal Render Pipeline/Unlit"));material.SetFloat("_Cull",0);glow.sharedMaterial=material;
            glow.shadowCastingMode=ShadowCastingMode.Off;glow.receiveShadows=false;
        }
        private void LateUpdate()
        {
            if(!glow||!upper)return;var phone=SmartphoneController.Instance;var campaign=CampaignState.Instance;
            bool show=phone&&phone.isPhoneRaised&&phone.isScreenOn&&phone.currentScanMode==ScanVisualMode.AR&&campaign&&!campaign.HasInventoryItem(completedFlag);
            glow.enabled=show;if(!show)return;
            var camera=upper.director.phoneView.FeedCamera;if(camera)transform.rotation=camera.transform.rotation;
            float pulse=Mathf.Pow(Mathf.Max(0,Mathf.Sin(Time.time*3.1f)),8);
            transform.localScale=Vector3.one*(.26f+pulse*.74f);material.SetColor("_BaseColor",new Color(.19f,.4f,.29f)*(1+pulse*3));
        }
        private void OnDestroy(){if(material)Destroy(material);if(mesh)Destroy(mesh);}
    }
}
