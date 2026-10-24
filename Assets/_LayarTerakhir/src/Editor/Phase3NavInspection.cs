#if UNITY_EDITOR
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Editor
{
 public static class Phase3NavInspection
 {
  public static void Run()
  {
   EditorSceneManager.OpenScene(Phase3SceneBuilder.ScenePath);Physics.SyncTransforms();
   foreach(var go in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))if(go.name=="modular_fire_escape_stairs")
   {
    foreach(var r in go.GetComponentsInChildren<Renderer>())Debug.Log("P3_STAIR_BOUNDS "+r.bounds+" path="+r.transform.position+" rot="+r.transform.eulerAngles+" scale="+r.transform.lossyScale);
   }
   foreach(var c in Object.FindObjectsByType<Camera>(FindObjectsInactive.Include,FindObjectsSortMode.None))Debug.Log("P3_CAMERA "+c.name+" "+c.pixelWidth+"x"+c.pixelHeight+" target="+c.targetTexture);
   foreach(var l in Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None))Debug.Log("P3_LIGHT "+l.name+" "+l.type+" shadows="+l.shadows+" range="+l.range);
   foreach(var g in Object.FindObjectsByType<MeshFilter>(FindObjectsSortMode.None).Where(f=>f.sharedMesh).GroupBy(f=>f.sharedMesh).OrderByDescending(g=>g.Key.triangles.Length*g.Count()).Take(12))Debug.Log("P3_MESH_LOAD "+g.Key.name+" instances="+g.Count()+" triangles="+g.Key.triangles.Length/3);
   long triangles=0;
   foreach(var f in Object.FindObjectsByType<MeshFilter>(FindObjectsSortMode.None))if(f.sharedMesh)triangles+=f.sharedMesh.triangles.LongLength/3;
   Debug.Log("P3_SCENE_TRIANGLES "+triangles);
   var stair=System.Linq.Enumerable.First(Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None),g=>g.name=="modular_fire_escape_stairs");
   var filter=stair.GetComponentInChildren<MeshFilter>();var vertices=System.Linq.Enumerable.Select(filter.sharedMesh.vertices,v=>filter.transform.TransformPoint(v)).ToArray();
   foreach(float height in new[]{.1f,3.9f})
   {
    var v=System.Linq.Enumerable.Where(vertices,v=>height<1?v.y<height:v.y>height).ToArray();
    Debug.Log("P3_STAIR_END height="+height+" count="+v.Length+" x="+System.Linq.Enumerable.Average(v,q=>q.x)+" z="+System.Linq.Enumerable.Average(v,q=>q.z));
   }
   foreach(float x in new[]{39f,57f})for(float z=17;z<=33;z+=1)
   {
    var hits=Physics.RaycastAll(new Vector3(x,8,z),Vector3.down,10,~((1<<24)|(1<<26)|(1<<29)|(1<<31)|(1<<2)),QueryTriggerInteraction.Ignore);
    foreach(var h in hits)if(h.normal.y>.4f)Debug.Log("P3_STAIR_PROFILE x="+x+" z="+z+" height="+h.point.y+" mesh="+h.collider.name);
    if(NavMesh.SamplePosition(new Vector3(x,4,z),out var n,1,NavMesh.AllAreas))Debug.Log("P3_STAIR_NAV x="+x+" z="+z+" point="+n.position);
   }
  }
 }
}
#endif
