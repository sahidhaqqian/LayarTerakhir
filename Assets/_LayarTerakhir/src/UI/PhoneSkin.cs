using UnityEngine;
using UnityEngine.UI;
namespace LayarTerakhir.UI
{
    // Code-native handset chrome: one nine-sliced sprite, reusable vector app icons.
    public static class PhoneSkin
    {
        private static Sprite rounded;
        public static readonly Color Ink=new Color(.035f,.05f,.075f),Paper=new Color(.91f,.94f,.96f),Blue=new Color(.19f,.52f,.94f);
        public static Sprite Rounded
        {
            get
            {
                if(rounded)return rounded;
                const int size=64;var texture=new Texture2D(size,size,TextureFormat.RGBA32,false){name="PhoneRoundedCorners",filterMode=FilterMode.Bilinear};var pixels=new Color32[size*size];
                for(int y=0;y<size;y++)for(int x=0;x<size;x++)
                {float dx=Mathf.Max(18-x,0,x-45),dy=Mathf.Max(18-y,0,y-45);pixels[y*size+x]=new Color(1,1,1,Mathf.Clamp01(18.5f-Mathf.Sqrt(dx*dx+dy*dy)));}
                texture.SetPixels32(pixels);texture.Apply(false,true);rounded=Sprite.Create(texture,new Rect(0,0,size,size),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,new Vector4(20,20,20,20));return rounded;
            }
        }
        public static GameObject Card(string name,Transform parent,Vector2 min,Vector2 max,Color color)
        {var go=UiWidgets.Panel(name,parent,min,max,color);var image=go.GetComponent<Image>();image.sprite=Rounded;image.type=Image.Type.Sliced;return go;}
        public static Button Button(string name,Transform parent,string text,Vector2 min,Vector2 max,UnityEngine.Events.UnityAction action,Color? tint=null)
        {
            var go=Card(name,parent,min,max,tint??new Color(.10f,.15f,.22f));var button=go.AddComponent<Button>();button.targetGraphic=go.GetComponent<Image>();button.onClick.AddListener(action);var colors=button.colors;colors.highlightedColor=new Color(.8f,.87f,1);colors.pressedColor=new Color(.55f,.69f,.86f);button.colors=colors;
            UiWidgets.Text("Label",go.transform,text,new Vector2(.06f,.02f),new Vector2(.94f,.98f),23,TextAnchor.MiddleCenter).color=Paper;return button;
        }
        public static void Icon(string kind,Transform parent,Vector2 min,Vector2 max,Color? color=null)
        {var go=UiWidgets.Rect("Icon_"+kind,parent,min,max);go.AddComponent<CanvasRenderer>();var glyph=go.AddComponent<PhoneGlyph>();glyph.kind=kind;glyph.color=color??Paper;glyph.raycastTarget=false;}
    }
    public sealed class PhoneGlyph:MaskableGraphic
    {
        public string kind;private VertexHelper vertices;
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();vertices=vh;
            if(kind=="Camera"){Box(.12f,.23f,.88f,.77f);Circle(.5f,.5f,.19f);Line(.23f,.77f,.34f,.88f);Line(.34f,.88f,.58f,.88f);Line(.58f,.88f,.68f,.77f);}
            else if(kind=="MataBatin")
            {
                // An open lens inside an incomplete eye: the installed application's own mark.
                for(int i=0;i<20;i++){float x=.08f+i*.042f,x2=x+.042f;Line(x,.5f+Mathf.Sin(i*Mathf.PI/20)*.28f,x2,.5f+Mathf.Sin((i+1)*Mathf.PI/20)*.28f);Line(x,.5f-Mathf.Sin(i*Mathf.PI/20)*.28f,x2,.5f-Mathf.Sin((i+1)*Mathf.PI/20)*.28f);}
                Circle(.5f,.5f,.16f);Circle(.5f,.5f,.055f);Line(.5f,.86f,.5f,.97f);Line(.24f,.81f,.18f,.91f);Line(.76f,.81f,.82f,.91f);
            }
            else if(kind=="Map"){Line(.12f,.18f,.36f,.29f);Line(.36f,.29f,.63f,.16f);Line(.63f,.16f,.88f,.28f);Line(.88f,.28f,.88f,.85f);Line(.88f,.85f,.63f,.73f);Line(.63f,.73f,.36f,.85f);Line(.36f,.85f,.12f,.73f);Line(.12f,.73f,.12f,.18f);Line(.36f,.29f,.36f,.85f);Line(.63f,.16f,.63f,.73f);}
            else if(kind=="Messages"){Box(.1f,.28f,.9f,.84f);Line(.1f,.28f,.1f,.1f);Line(.1f,.1f,.3f,.28f);Line(.27f,.65f,.73f,.65f);Line(.27f,.47f,.6f,.47f);}
            else if(kind=="Bank"){Line(.1f,.74f,.5f,.94f);Line(.5f,.94f,.9f,.74f);Line(.1f,.74f,.9f,.74f);Line(.1f,.14f,.9f,.14f);for(int i=0;i<3;i++)Line(.25f+i*.25f,.25f,.25f+i*.25f,.63f);}
            else if(kind=="Inventory"){Box(.2f,.12f,.8f,.72f);Line(.32f,.72f,.32f,.89f);Line(.32f,.89f,.68f,.89f);Line(.68f,.89f,.68f,.72f);Line(.2f,.53f,.8f,.53f);}
            else if(kind=="Journal"){Box(.18f,.1f,.82f,.9f);Line(.33f,.1f,.33f,.9f);Line(.46f,.69f,.7f,.69f);Line(.46f,.52f,.7f,.52f);}
            else if(kind=="Settings"){Circle(.5f,.5f,.26f);Circle(.5f,.5f,.08f);for(int i=0;i<8;i++){float a=i*Mathf.PI*.25f;Line(.5f+Mathf.Cos(a)*.26f,.5f+Mathf.Sin(a)*.26f,.5f+Mathf.Cos(a)*.38f,.5f+Mathf.Sin(a)*.38f);}}
            else if(kind=="Call"){Line(.22f,.83f,.15f,.68f);Line(.15f,.68f,.28f,.41f);Line(.28f,.41f,.56f,.17f);Line(.56f,.17f,.79f,.2f);Line(.79f,.2f,.86f,.32f);Line(.22f,.83f,.37f,.67f);Line(.37f,.67f,.28f,.56f);Line(.59f,.29f,.72f,.39f);Line(.72f,.39f,.86f,.32f);}
            else if(kind=="Home"){Line(.14f,.53f,.5f,.9f);Line(.5f,.9f,.86f,.53f);Line(.25f,.61f,.25f,.13f);Line(.25f,.13f,.75f,.13f);Line(.75f,.13f,.75f,.61f);Box(.43f,.13f,.59f,.43f);}
        }
        private void Box(float x,float y,float x2,float y2){Line(x,y,x2,y);Line(x2,y,x2,y2);Line(x2,y2,x,y2);Line(x,y2,x,y);}
        private void Circle(float x,float y,float radius){for(int i=0;i<32;i++){float a=i*Mathf.PI/16,b=(i+1)*Mathf.PI/16;Line(x+Mathf.Cos(a)*radius,y+Mathf.Sin(a)*radius,x+Mathf.Cos(b)*radius,y+Mathf.Sin(b)*radius);}}
        private void Line(float x,float y,float x2,float y2)
        {
            var r=rectTransform.rect;Vector2 a=new Vector2(r.x+x*r.width,r.y+y*r.height),b=new Vector2(r.x+x2*r.width,r.y+y2*r.height);var n=(b-a).normalized;var side=new Vector2(-n.y,n.x)*Mathf.Min(r.width,r.height)*.025f;
            int start=vertices.currentVertCount;vertices.AddVert(a-side,color,Vector2.zero);vertices.AddVert(a+side,color,Vector2.zero);vertices.AddVert(b+side,color,Vector2.zero);vertices.AddVert(b-side,color,Vector2.zero);vertices.AddTriangle(start,start+1,start+2);vertices.AddTriangle(start,start+2,start+3);
        }
    }
}
