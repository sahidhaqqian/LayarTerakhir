#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using LayarTerakhir.Phase4;
namespace LayarTerakhir.Editor
{
 public static class Phase4MaterialProbe
 {
  [Serializable]private class Row{public string source,renderer,mesh;public string[] materials,embedded,importer;}
  [Serializable]private class Report{public Row[] rows;}
  public static void Inspect()
  {
   var rows=new List<Row>();
   foreach(string id in new[]{"standing_picture_frame_01","spinning_wheel_01","wooden_lantern_01","stone_fire_pit","jug_01"})
   {
    string path="Assets/_LayarTerakhir/assets/Art/Phase4/Imported/Models/"+id+".fbx";var go=AssetDatabase.LoadAssetAtPath<GameObject>(path);
    var embedded=AssetDatabase.LoadAllAssetsAtPath(path).OfType<Material>().Select(m=>m.name+" shader="+m.shader.name+" texture="+m.mainTexture).ToArray();
    var ser=new SerializedObject(AssetImporter.GetAtPath(path));var prop=ser.GetIterator();var metadata=new List<string>();
    while(prop.NextVisible(true))if(prop.propertyPath.Contains("SourceMaterials")||prop.propertyPath.Contains("sourceMaterial"))metadata.Add(prop.propertyPath+" "+(prop.propertyType==SerializedPropertyType.String?prop.stringValue:prop.propertyType.ToString()));
    foreach(var r in go.GetComponentsInChildren<Renderer>())rows.Add(new Row{source=id,renderer=r.name,mesh=r.GetComponent<MeshFilter>()?.sharedMesh?.name,materials=r.sharedMaterials.Select(m=>m?m.name+" path="+AssetDatabase.GetAssetPath(m):"NULL").ToArray(),embedded=embedded,importer=metadata.ToArray()});
   }
   File.WriteAllText("Builds/ArtSources/Phase4/unity-material-probe.json",JsonUtility.ToJson(new Report{rows=rows.ToArray()},true));
  }
  public static void InspectWorld()
  {
   EditorSceneManager.OpenScene(Phase4SceneBuilder.ScenePath);
   var report=new List<string>();
   foreach(var source in UnityEngine.Object.FindObjectsByType<Phase4AssetSource>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(s=>s.purpose=="Forest_Ground"||s.purpose=="Pendopo ruwatan"||s.purpose=="Beringin makam").Take(3))
   foreach(var r in source.GetComponentsInChildren<MeshRenderer>(true).Take(3))
   {
    var mesh=r.GetComponent<MeshFilter>().sharedMesh;var vertices=mesh.vertices;var tris=mesh.triangles;Vector3 sum=Vector3.zero;float area=0,up=0,down=0;
    for(int i=0;i<tris.Length;i+=3){var a=r.transform.TransformPoint(vertices[tris[i]]);var b=r.transform.TransformPoint(vertices[tris[i+1]]);var c=r.transform.TransformPoint(vertices[tris[i+2]]);var n=Vector3.Cross(b-a,c-a);sum+=n;area+=n.magnitude;if(n.y>0)up+=n.magnitude;else down+=n.magnitude;}
    report.Add(source.purpose+" / "+r.name+" bounds="+r.bounds+" triangleArea="+area+" up="+up+" down="+down+" normalSum="+sum);
    foreach(var m in r.sharedMaterials)report.Add("MAT "+m.name+" color="+m.GetColor("_BaseColor")+" albedo="+m.GetTexture("_BaseMap")+" tiling="+m.GetTextureScale("_BaseMap")+" metal="+m.GetFloat("_Metallic")+" smooth="+m.GetFloat("_Smoothness"));
   }
   var d=UnityEngine.Object.FindFirstObjectByType<Phase4Director>();var cc=d.player.GetComponent<CharacterController>();
   var centre=new Vector3(-4.23f,.08f,-5.71f)+cc.center;var ext=Vector3.up*Mathf.Max(0,cc.height*.5f-cc.radius);Physics.SyncTransforms();
   report.Add("CAPSULE radius="+cc.radius+" height="+cc.height+" center="+cc.center+" skin="+cc.skinWidth);
   foreach(var c in Physics.OverlapCapsule(centre-ext,centre+ext,cc.radius+.15f,~0,QueryTriggerInteraction.Ignore).Where(c=>!c.transform.IsChildOf(d.player.transform)))report.Add("OVERLAP "+c.name+" bounds="+c.bounds+" source="+c.GetComponentInParent<Phase4AssetSource>()?.purpose);
   File.WriteAllLines("Builds/ArtSources/Phase4/world-material-probe.txt",report);
  }
  public static void InspectAltarLine()
  {
   EditorSceneManager.OpenScene(Phase4SceneBuilder.ScenePath);Physics.SyncTransforms();
   var d=UnityEngine.Object.FindFirstObjectByType<Phase4Director>();var lines=new List<string>();
   foreach(float y in new[]{2.2f,2.4f,2.6f})
   {
    var from=new Vector3(29,y,-.2f);var delta=d.world.arTarget.position-from;
    lines.Add("AR from="+from+" target="+d.world.arTarget.position+" ignoredRoot="+d.world.arOcclusionRoot.name+" clear="+d.ClearLine(from,d.world.arTarget.position,d.world.arOcclusionRoot,true));
    foreach(var hit in Physics.RaycastAll(from,delta.normalized,delta.magnitude-.04f,~((1<<24)|(1<<26)|(1<<29)|(1<<2)|(1<<31)|(1<<30)),QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance))
     lines.Add(hit.collider.name+" layer="+hit.collider.gameObject.layer+" at="+hit.point+" bounds="+hit.collider.bounds+" ignored="+hit.transform.IsChildOf(d.world.arOcclusionRoot)+" source="+hit.collider.GetComponentInParent<Phase4AssetSource>()?.purpose);
   }
   File.WriteAllLines("Builds/ArtSources/Phase4/altar-line-probe.txt",lines);
  }
 }
}
#endif
