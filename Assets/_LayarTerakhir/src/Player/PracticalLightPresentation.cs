using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Player
{
    // Presentation only: no change to flashlight availability, battery or detection.
    public sealed class PracticalLightPresentation : MonoBehaviour
    {
        public const uint ThreatRenderingLayer=1u<<8;
        private Texture2D cookie;
        public static void ConfigureBeam(Light source)
        {
            if(!source||source.cookie||source.GetComponent<PracticalLightPresentation>())return;
            var presentation=source.gameObject.AddComponent<PracticalLightPresentation>();
            presentation.cookie=CreateCookie();source.cookie=presentation.cookie;
        }
        public static Texture2D CreateCookie()
        {
            const int size=256;
            var texture=new Texture2D(size,size,TextureFormat.RGBA32,false,true){name="Soft optical beam",wrapMode=TextureWrapMode.Clamp,filterMode=FilterMode.Bilinear};
            var pixels=new Color32[size*size];
            for(int y=0;y<size;y++)for(int x=0;x<size;x++)
            {
                float px=(x+.5f)/size*2-1,py=(y+.5f)/size*2-1;
                float radius=Mathf.Sqrt(px*px+py*py);
                float edge=1-Mathf.SmoothStep(.68f,.98f,radius);
                float hotspot=Mathf.Lerp(.56f,1,Mathf.Exp(-radius*radius*3.6f));
                byte value=(byte)Mathf.RoundToInt(edge*hotspot*255);
                pixels[y*size+x]=new Color32(value,value,value,value);
            }
            texture.SetPixels32(pixels);texture.Apply(false,false);return texture;
        }
        public static void AddThreatRim(GameObject subject)
        {
            if(!subject||subject.transform.Find("ThreatSilhouette"))return;
            foreach(var renderer in subject.GetComponentsInChildren<Renderer>(true))renderer.renderingLayerMask|=ThreatRenderingLayer;
            var go=new GameObject("ThreatSilhouette",typeof(Light));go.transform.SetParent(subject.transform,false);
            go.transform.localPosition=new Vector3(-.5f,1.5f,-.55f);
            var light=go.GetComponent<Light>();light.type=LightType.Point;light.range=2.7f;
            light.intensity=.8f;light.color=new Color(.53f,.68f,.65f);light.shadows=LightShadows.None;
            light.GetUniversalAdditionalLightData().renderingLayers=(RenderingLayerMask)ThreatRenderingLayer;
        }
        private void OnDestroy(){if(cookie)Destroy(cookie);}
    }
}
