using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using UnityEngine;
using UnityEngine.Rendering;

namespace LayarTerakhir.Prolog
{
    // A small, depth-tested glint belongs to the photographable object in the AR feed.
    // A slow dark wisp drifts behind it so players can tell which object the
    // ghost leaves its mark on: blink for attention, smoke for dread.
    public sealed class ARPhotoGlimmer : MonoBehaviour
    {
        public AtmajaUpperDirector upper;
        public string completedFlag;
        public bool arOnly=true,whiteGlint,showWisp=true;
        private MeshRenderer glow;
        private MeshRenderer wisp;
        private Material material;
        private Material wispMaterial;
        private Mesh mesh;
        private Mesh wispMesh;
        public static void Attach(Transform anchor,AtmajaUpperDirector upper,string flag,bool arOnly=true,bool white=false,bool smoke=true)
        {
            if(!anchor||anchor.GetComponentInChildren<ARPhotoGlimmer>(true))return;
            var go=new GameObject("ARPhotoGlimmer",typeof(MeshFilter),typeof(MeshRenderer));
            go.transform.SetParent(anchor,false);go.layer=arOnly?AtmajaPhoneView.ARLayer:0;
            var glimmer=go.AddComponent<ARPhotoGlimmer>();glimmer.upper=upper;glimmer.completedFlag=flag;glimmer.arOnly=arOnly;glimmer.whiteGlint=white;glimmer.showWisp=smoke;
        }
        private void Start()
        {
            mesh=new Mesh{name="AR glint"};mesh.vertices=new[]{new Vector3(0,.055f,0),new Vector3(.017f,0,0),new Vector3(0,-.055f,0),new Vector3(-.017f,0,0),new Vector3(-.04f,0,0),new Vector3(0,.009f,0),new Vector3(.04f,0,0),new Vector3(0,-.009f,0)};
            mesh.triangles=new[]{0,1,2,0,2,3,4,5,6,4,6,7};mesh.RecalculateBounds();
            GetComponent<MeshFilter>().sharedMesh=mesh;glow=GetComponent<MeshRenderer>();
            material=new Material(Shader.Find("Universal Render Pipeline/Unlit"));material.SetFloat("_Cull",0);glow.sharedMaterial=material;
            glow.shadowCastingMode=ShadowCastingMode.Off;glow.receiveShadows=false;
            // Dark smoke wisp: a soft quad behind the glint, same layer, unlit.
            wispMesh=new Mesh{name="AR wisp"};wispMesh.vertices=new[]{new Vector3(-.05f,.09f,0),new Vector3(.05f,.09f,0),new Vector3(.05f,-.09f,0),new Vector3(-.05f,-.09f,0)};
            wispMesh.triangles=new[]{0,1,2,0,2,3};wispMesh.RecalculateNormals();wispMesh.RecalculateBounds();
            var wispGo=new GameObject("ARPhotoWisp",typeof(MeshFilter),typeof(MeshRenderer));
            wispGo.transform.SetParent(transform,false);wispGo.transform.localPosition=Vector3.zero;wispGo.layer=AtmajaPhoneView.ARLayer;
            wispGo.GetComponent<MeshFilter>().sharedMesh=wispMesh;wisp=wispGo.GetComponent<MeshRenderer>();
            // Sprites/Default is an always-included unlit alpha-blended shader,
            // so the smoke body blends reliably without URP keyword setup.
            var spriteShader=Shader.Find("Sprites/Default")??Shader.Find("Universal Render Pipeline/Unlit");
            wispMaterial=new Material(spriteShader);
            wisp.sharedMaterial=wispMaterial;
            wisp.shadowCastingMode=ShadowCastingMode.Off;wisp.receiveShadows=false;
        }
        private void LateUpdate()
        {
            if(!glow||!upper){return;}var phone=SmartphoneController.Instance;var campaign=CampaignState.Instance;
            bool show=campaign&&!campaign.HasInventoryItem(completedFlag)&&(!arOnly||phone&&phone.isPhoneRaised&&phone.isScreenOn&&phone.currentScanMode==ScanVisualMode.AR);
            if(!arOnly)show&=upper.director.Stage<AtmajaStage.Chase&&Vector3.Distance(transform.position,upper.director.player.transform.position)<5;
            glow.enabled=show;if(wisp)wisp.enabled=show&&showWisp;if(!show)return;
            var camera=upper.director.phoneView.FeedCamera;if(camera)transform.rotation=camera.transform.rotation;
            float t=Time.time;
            // Subtle 2 Hz blink so the mark reads as supernatural, not static UI.
            float blink=(Mathf.Sin(t*Mathf.PI*2*2f)>0)?1f:.25f;
            float pulse=Mathf.Pow(Mathf.Max(0,Mathf.Sin(t*3.1f)),8);
            transform.localScale=Vector3.one*(whiteGlint?.24f+pulse*.16f:.26f+pulse*.74f);material.SetColor("_BaseColor",(whiteGlint?Color.white:new Color(.19f,.4f,.29f))*(1+pulse*3)*blink);
            // Dark smoke: slow rise, sideways drift and breathing opacity.
            if(wisp)
            {
                wisp.transform.localPosition=new Vector3(Mathf.Sin(t*1.3f)*.03f,.04f+Mathf.Sin(t*.9f)*.02f,.012f);
                wisp.transform.localScale=new Vector3(1+Mathf.Sin(t*1.7f)*.15f,1.2f+Mathf.Sin(t*1.1f)*.2f,1);
                float smoke=.16f+.10f*Mathf.Sin(t*1.1f+1f);
                wispMaterial.color=new Color(.02f,.015f,.03f,Mathf.Clamp01(smoke)*blink);
            }
        }
        private void OnDestroy(){if(material)Destroy(material);if(mesh)Destroy(mesh);if(wispMaterial)Destroy(wispMaterial);if(wispMesh)Destroy(wispMesh);}
    }
}
