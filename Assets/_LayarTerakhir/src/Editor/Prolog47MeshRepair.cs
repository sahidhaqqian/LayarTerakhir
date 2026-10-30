#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Clip only proven intersecting polygons. Source FBXs/material groups stay intact.
    internal static class Prolog47MeshRepair
    {
        private struct Vertex
        {
            public Vector3 p,n,local;public Vector2 uv;
            public static Vertex Lerp(Vertex a,Vertex b,float t)=>new Vertex{p=Vector3.Lerp(a.p,b.p,t),n=Vector3.Lerp(a.n,b.n,t).normalized,local=Vector3.Lerp(a.local,b.local,t),uv=Vector2.Lerp(a.uv,b.uv,t)};
        }
        internal static void Subtract(MeshFilter filter,AtmajaDirector director,Bounds cut,string name)
        {
            Rebuild(filter,director,name,polygon=>
            {
                var output=new List<List<Vertex>>();var inside=polygon;
                for(int axis=0;axis<3&&inside.Count>=3;axis++)
                {
                    float lo=cut.min[axis],hi=cut.max[axis];
                    var outside=Clip(inside,axis,lo,false,false);if(outside.Count>=3)output.Add(outside);
                    inside=Clip(inside,axis,lo,true);if(inside.Count<3)break;
                    outside=Clip(inside,axis,hi,true,false);if(outside.Count>=3)output.Add(outside);
                    inside=Clip(inside,axis,hi,false);
                }
                return output;
            });
        }
        internal static void RemoveUpwardFinish(MeshFilter filter,AtmajaDirector director,float height,string name)
        {
            Rebuild(filter,director,name,polygon=>
            {
                var normal=Vector3.Cross(polygon[1].local-polygon[0].local,polygon[2].local-polygon[0].local).normalized;
                if(Mathf.Abs(normal.y)>.98f&&polygon.TrueForAll(v=>Mathf.Abs(v.local.y-height)<.004f))
                {
                    // Preserve the underside at the lower plane; this only removes
                    // the coincident top triangles hidden beneath the finish.
                    var worldNormal=filter.transform.TransformDirection(polygon[0].n);
                    if(worldNormal.y>.9f)return new List<List<Vertex>>();
                }
                return new List<List<Vertex>>{polygon};
            });
        }
        private static List<Vertex> Clip(List<Vertex> poly,int axis,float plane,bool greater,bool inclusive=true)
        {
            var result=new List<Vertex>();if(poly.Count==0)return result;
            var previous=poly[poly.Count-1];float prev=(previous.local[axis]-plane)*(greater?1:-1);
            foreach(var current in poly)
            {
                float value=(current.local[axis]-plane)*(greater?1:-1);
                if((inclusive?prev>=-1e-6f:prev>1e-6f)!=(inclusive?value>=-1e-6f:value>1e-6f))result.Add(Vertex.Lerp(previous,current,prev/(prev-value)));
                if(inclusive?value>=-1e-6f:value>1e-6f)result.Add(current);previous=current;prev=value;
            }
            return result;
        }
        private static void Rebuild(MeshFilter filter,AtmajaDirector director,string name,Func<List<Vertex>,List<List<Vertex>>> operation)
        {
            var source=filter.sharedMesh;var positions=source.vertices;var normals=source.normals;var uvs=source.uv;
            var verts=new List<Vector3>();var norms=new List<Vector3>();var uv=new List<Vector2>();var submeshes=new List<int[]>();
            Vertex At(int i)
            {
                var world=filter.transform.TransformPoint(positions[i]);var xy=director.LocalPosition(world);
                return new Vertex{p=positions[i],n=normals.Length==positions.Length?normals[i]:Vector3.up,uv=uvs.Length==positions.Length?uvs[i]:Vector2.zero,local=new Vector3(xy.x,world.y,xy.y)};
            }
            for(int sm=0;sm<source.subMeshCount;sm++)
            {
                var indices=source.GetTriangles(sm);var output=new List<int>();
                for(int i=0;i<indices.Length;i+=3)
                {
                    var polygons=operation(new List<Vertex>{At(indices[i]),At(indices[i+1]),At(indices[i+2])});
                    foreach(var polygon in polygons)
                    {
                        int start=verts.Count;foreach(var v in polygon){verts.Add(v.p);norms.Add(v.n);uv.Add(v.uv);}
                        for(int j=1;j<polygon.Count-1;j++){if(Vector3.Cross(polygon[j].p-polygon[0].p,polygon[j+1].p-polygon[0].p).sqrMagnitude<1e-14f)continue;output.Add(start);output.Add(start+j);output.Add(start+j+1);}
                    }
                }
                submeshes.Add(output.ToArray());
            }
            var mesh=new Mesh{name=name,indexFormat=IndexFormat.UInt32};mesh.SetVertices(verts);mesh.SetNormals(norms);mesh.SetUVs(0,uv);mesh.subMeshCount=submeshes.Count;
            for(int i=0;i<submeshes.Count;i++)mesh.SetTriangles(submeshes[i],i);mesh.RecalculateTangents();mesh.RecalculateBounds();
            const string folder="Assets/_LayarTerakhir/assets/Art/Generated/Models/Prolog47/";Directory.CreateDirectory(folder);
            string path=folder+name+".asset";var existing=AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if(existing){EditorUtility.CopySerialized(mesh,existing);Object.DestroyImmediate(mesh);mesh=existing;}else AssetDatabase.CreateAsset(mesh,path);
            filter.sharedMesh=mesh;PrefabUtility.RecordPrefabInstancePropertyModifications(filter);
        }
    }
}
#endif
