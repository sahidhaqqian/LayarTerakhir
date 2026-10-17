using UnityEngine;
using UnityEngine.UI;

namespace LayarTerakhir.UI
{
    // Offline, resolution-independent fictional neighbourhood map. No map service/API or asset scan.
    public sealed class StoryMapGraphic : MaskableGraphic
    {
        private VertexHelper vh;
        protected override void OnPopulateMesh(VertexHelper helper)
        {
            helper.Clear();vh=helper;
            Quad(Vector2.zero,Vector2.one,new Color(.89f,.88f,.83f));
            for(int x=0;x<7;x++)for(int y=0;y<9;y++)
            {
                float xx=.015f+x*.146f,yy=.015f+y*.112f;
                Quad(new Vector2(xx,yy),new Vector2(xx+.119f,yy+.081f),
                    (x==5&&y>4)||(x==1&&y==3)?new Color(.66f,.76f,.60f):new Color(.80f,.79f,.73f));
                Quad(new Vector2(xx+.013f,yy+.012f),new Vector2(xx+.09f,yy+.055f),new Color(.85f,.84f,.79f));
            }
            for(int i=0;i<18;i++)
            {
                float y=i/17f;float next=(i+1)/17f;
                Stroke(new Vector2(.82f+Mathf.Sin(y*6)*.10f,y),new Vector2(.82f+Mathf.Sin(next*6)*.10f,next),.036f,new Color(.52f,.70f,.75f));
            }
            for(int i=0;i<7;i++)Stroke(new Vector2(0,.09f+i*.146f),new Vector2(1,.09f+i*.146f),.018f,new Color(.99f,.98f,.94f));
            for(int i=0;i<6;i++)Stroke(new Vector2(.065f+i*.18f,0),new Vector2(.065f+i*.18f,1),.018f,new Color(.99f,.98f,.94f));
            Stroke(new Vector2(.24f,0),new Vector2(.24f,1),.045f,new Color(.97f,.93f,.80f));
            Stroke(new Vector2(0,.52f),new Vector2(1,.52f),.045f,new Color(.97f,.93f,.80f));
            Vector2[] route={new Vector2(.24f,.23f),new Vector2(.24f,.52f),new Vector2(.60f,.52f),new Vector2(.60f,.76f),new Vector2(.68f,.76f)};
            for(int i=1;i<route.Length;i++)Stroke(route[i-1],route[i],.030f,Color.white);
            for(int i=1;i<route.Length;i++)Stroke(route[i-1],route[i],.018f,new Color(.15f,.40f,.75f));
        }
        private Vector2 Pixel(Vector2 point){var r=rectTransform.rect;return r.min+Vector2.Scale(point,r.size);}
        private void Quad(Vector2 min,Vector2 max,Color color)
        {
            int start=vh.currentVertCount;vh.AddVert(Pixel(min),color,Vector2.zero);vh.AddVert(Pixel(new Vector2(min.x,max.y)),color,Vector2.zero);
            vh.AddVert(Pixel(max),color,Vector2.zero);vh.AddVert(Pixel(new Vector2(max.x,min.y)),color,Vector2.zero);vh.AddTriangle(start,start+1,start+2);vh.AddTriangle(start,start+2,start+3);
        }
        private void Stroke(Vector2 from,Vector2 to,float width,Color color)
        {
            Vector2 a=Pixel(from),b=Pixel(to),n=(b-a).normalized;Vector2 side=new Vector2(-n.y,n.x)*rectTransform.rect.width*width*.5f;
            int start=vh.currentVertCount;vh.AddVert(a-side,color,Vector2.zero);vh.AddVert(a+side,color,Vector2.zero);vh.AddVert(b+side,color,Vector2.zero);vh.AddVert(b-side,color,Vector2.zero);vh.AddTriangle(start,start+1,start+2);vh.AddTriangle(start,start+2,start+3);
        }
    }
}
