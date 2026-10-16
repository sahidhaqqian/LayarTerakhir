#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    // Hand-drawn stroke geometry, not a floating UI font. All vertices sit on the wall plane.
    public static class BloodWriting
    {
        private static readonly Dictionary<char,string> strokes=new Dictionary<char,string>{
            ['W']="0,1 .2,0 .5,.65 .8,0 1,1",['A']="0,0 .5,1 1,0;.2,.4 .8,.4",['K']="0,0 0,1;1,1 0,.45 1,0",['T']="0,1 1,1;.5,1 .5,0",
            ['U']="0,1 0,.2 .2,0 .8,0 1,.2 1,1",['E']="1,1 0,1 0,0 1,0;0,.5 .75,.5",['M']="0,0 0,1 .5,.4 1,1 1,0",['I']=".5,0 .5,1",
            ['N']="0,0 0,1 1,0 1,1",['D']="0,0 0,1 .6,1 1,.7 1,.3 .6,0 0,0",['L']="0,1 0,0 1,0",['H']="0,0 0,1;1,0 1,1;0,.5 1,.5",
            ['C']="1,.9 .75,1 .2,1 0,.8 0,.2 .2,0 .75,0 1,.1",['X']="0,0 1,1;0,1 1,0",['V']="0,1 .5,0 1,1",[':']=".5,.7 .5,.73;.5,.25 .5,.28"};
        public static GameObject Build(Transform parent,Vector3 origin,Vector3 right,Vector3 up,Vector3 normal)
        {
            var vertices=new List<Vector3>();var triangles=new List<int>();var random=new System.Random(3451998);
            void Segment(Vector2 a,Vector2 b,float width)
            {
                Vector2 tangent=(b-a).normalized;var across=new Vector2(-tangent.y,tangent.x)*width*.5f;int s=vertices.Count;
                foreach(var p in new[]{a+across,a-across,b+across,b-across})vertices.Add(right*p.x+up*p.y+normal*(.0035f+(float)random.NextDouble()*.001f));
                triangles.AddRange(new[]{s,s+2,s+1,s+2,s+3,s+1});
            }
            string[] lines={"WAKTU KEMATIAN KAMI","ADALAH KUNCI","III : XLV"};
            for(int row=0;row<lines.Length;row++)
            {
                string words=lines[row];float advance=.079f,start=-words.Length*advance*.5f;
                for(int c=0;c<words.Length;c++)
                {
                    if(!strokes.TryGetValue(words[c],out var glyph))continue;
                    float lean=((float)random.NextDouble()-.5f)*.014f,baseY=.21f-row*.235f+((float)random.NextDouble()-.5f)*.016f;
                    foreach(var stroke in glyph.Split(';'))
                    {
                        var pairs=stroke.Split(' ');Vector2 previous=Vector2.zero;
                        for(int k=0;k<pairs.Length;k++)
                        {
                            var xy=pairs[k].Split(',');float x=float.Parse(xy[0],CultureInfo.InvariantCulture),y=float.Parse(xy[1],CultureInfo.InvariantCulture);
                            var p=new Vector2(start+c*advance+x*.058f+lean*y,baseY+y*.13f);
                            if(k>0)Segment(previous,p,.012f+(float)random.NextDouble()*.005f);previous=p;
                        }
                        if(random.NextDouble()<.18)Segment(previous,previous+new Vector2(.003f,-.035f-(float)random.NextDouble()*.04f),.004f);
                    }
                }
            }
            var mesh=new Mesh{name="Blood smeared handwriting"};mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.RecalculateBounds();var normals=new Vector3[vertices.Count];for(int i=0;i<normals.Length;i++)normals[i]=normal;mesh.normals=normals;
            string path="Assets/_LayarTerakhir/Art/Atmaja/Models/ARBloodWriting.asset";var old=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(old){EditorUtility.CopySerialized(mesh,old);UnityEngine.Object.DestroyImmediate(mesh);mesh=old;}else AssetDatabase.CreateAsset(mesh,path);
            // AR-only unlit ink: Lit material validation stripped the emissive keyword,
            // turning blood almost black under the deliberately dark house lighting.
            var material=AtmajaSceneBuilder.Plain("ARBlood",new Color(.42f,.003f,.001f),.24f);material.shader=Shader.Find("Universal Render Pipeline/Unlit");material.shaderKeywords=Array.Empty<string>();material.SetFloat("_Cull",0);EditorUtility.SetDirty(material);
            var go=new GameObject("ARDeathTimeClue",typeof(MeshFilter),typeof(MeshRenderer));go.transform.SetParent(parent,false);go.transform.position=origin;go.layer=29;go.GetComponent<MeshFilter>().sharedMesh=mesh;go.GetComponent<MeshRenderer>().sharedMaterial=material;go.GetComponent<MeshRenderer>().shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;return go;
        }
    }
}
#endif
