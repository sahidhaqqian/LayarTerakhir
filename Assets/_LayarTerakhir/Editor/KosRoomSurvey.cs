#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static class KosRoomSurvey
    {
        public static void Run()
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            foreach(string name in new[]{"computerScreen","MonitorDashboard","wooden_table_02","HangingJacket","JacketHook","Interactable_PintuKos","Wall_South"})
            {
                var go=GameObject.Find(name);if(!go)continue;var b=KosRoomCorrections.BoundsOf(go);Debug.Log("KOS_SURVEY "+name+" world="+go.transform.position.ToString("F4")+" bounds="+b.ToString("F4"));
                if(name!="computerScreen")continue;
                foreach(var mesh in go.GetComponentsInChildren<MeshFilter>())
                {
                    var all=mesh.sharedMesh.vertices.Select(p=>mesh.transform.TransformPoint(p)).ToArray();var indices=mesh.sharedMesh.triangles;
                    for(int i=0;i<indices.Length;i+=3)
                    {
                        var a=all[indices[i]];var c=all[indices[i+1]];var e=all[indices[i+2]];var cross=Vector3.Cross(c-a,e-a);
                        if(cross.magnitude>.02f&&a.y>b.min.y+b.size.y*.39f)Debug.Log("KOS_SCREEN_TRI "+a.ToString("F4")+" "+c.ToString("F4")+" "+e.ToString("F4")+" normal="+cross.normalized.ToString("F4"));
                    }
                    var points=mesh.sharedMesh.vertices.Select(p=>mesh.transform.TransformPoint(p)).Where(p=>p.y>b.min.y+b.size.y*.4f).ToArray();
                    foreach(var group in points.GroupBy(p=>Mathf.Round(p.z*10000)/10000).OrderByDescending(g=>g.Count()).Take(12))
                        Debug.Log("KOS_GLASS_PLANE z="+group.Key+" vertices="+group.Count()+" x="+group.Min(p=>p.x)+".."+group.Max(p=>p.x)+" y="+group.Min(p=>p.y)+".."+group.Max(p=>p.y));
                }
            }
        }
    }
}
#endif
