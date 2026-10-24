#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Editor
{
    public static class Phase2NavigationProbe
    {
        public static void InspectLastri()
        {
            EditorSceneManager.OpenScene(Phase2SceneBuilder.ScenePath);
            var d=UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Phase2.Phase2Director>();
            var root=d.world.lastri;var motion=root.GetComponentInChildren<UnityEngine.Animation>();
            Debug.Log("P2_LASTRI_CLIPS "+string.Join(" / ",UnityEditor.AssetDatabase.LoadAllAssetsAtPath("Assets/_LayarTerakhir/assets/Art/Phase2/Characters/Models/Lastri.fbx").OfType<AnimationClip>().Select(c=>c.name)));
            Debug.Log("P2_LASTRI_COMPONENTS "+string.Join(" / ",root.GetComponentsInChildren<Renderer>(true).Select(r=>r.GetType().Name+" "+r.name+" "+r.bounds)));
            foreach(string clip in new[]{"Idle","Walk"})
            {
                if(motion&&motion[clip]){motion.Play(clip);motion[clip].time=0;motion.Sample();}
                foreach(var skin in root.GetComponentsInChildren<SkinnedMeshRenderer>(true))
                {
                    var mesh=new Mesh();skin.BakeMesh(mesh,false);var vertices=mesh.vertices.Select(v=>skin.transform.TransformPoint(v)).ToArray();
                    var box=new Bounds(vertices[0],Vector3.zero);foreach(var v in vertices)box.Encapsulate(v);
                    var src=skin.sharedMesh;var sourceVertices=src.vertices;var weights=src.boneWeights;var binds=src.bindposes;var matrices=skin.bones.Select((bone,i)=>bone.localToWorldMatrix*binds[i]).ToArray();
                    var manual=new Bounds();bool first=true;
                    for(int i=0;i<sourceVertices.Length;i++)
                    {var w=weights[i];var v=sourceVertices[i];Vector3 point=matrices[w.boneIndex0].MultiplyPoint3x4(v)*w.weight0+matrices[w.boneIndex1].MultiplyPoint3x4(v)*w.weight1+matrices[w.boneIndex2].MultiplyPoint3x4(v)*w.weight2+matrices[w.boneIndex3].MultiplyPoint3x4(v)*w.weight3;if(first){manual=new Bounds(point,Vector3.zero);first=false;}else manual.Encapsulate(point);}
                    Debug.Log("P2_LASTRI_MANUAL "+manual+" bone0="+skin.bones[0].position+" scale="+skin.bones[0].lossyScale);
                    Debug.Log("P2_LASTRI_BOUNDS "+clip+" root="+root.transform.position+" renderer="+skin.bounds+" baked="+box+" scale="+skin.transform.lossyScale+" bones="+skin.bones.Length+" mask="+skin.renderingLayerMask+" layer="+skin.gameObject.layer);
                    UnityEngine.Object.DestroyImmediate(mesh);
                }
            }
        }
        public static void BuildAndInspectSafe(){Phase2SceneBuilder.Build();InspectSafe();}
        public static void InspectSafe()
        {
            EditorSceneManager.OpenScene(Phase2SceneBuilder.ScenePath);
            var d=UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Phase2.Phase2Director>();
            foreach(bool open in new[]{false,true})
            {
                d.world.safeDoor.SetOpenImmediate(open);Physics.SyncTransforms();
                var target=d.world.blood.GetComponentsInChildren<Renderer>(true)[0].bounds.center;
                Debug.Log("P2_SAFE "+open+" blood="+target+" door="+d.world.safeDoor.hinge.childCount.ToString());
                foreach(var from in new[]{new Vector3(22,5.6f,9.1f),new Vector3(22,5.6f,11.9f)})
                    Debug.Log("P2_SAFE_RAY "+from+" "+string.Join(" / ",Physics.RaycastAll(from,(target-from).normalized,(target-from).magnitude,~((1<<26)|(1<<31)),QueryTriggerInteraction.Ignore)
                        .OrderBy(h=>h.distance).Select(h=>h.collider.name+" "+h.point)));
            }
        }
        public static void Run()
        {
            EditorSceneManager.OpenScene(Phase2SceneBuilder.ScenePath);Physics.SyncTransforms();
            foreach(string file in new[]{"Madduck/Models/Tile_Wall","Madduck/Models/Door_1","Madduck/Models/Bed","Madduck/Models/Bench","Madduck/Models/Chair","Kenney/Stairs","Autopsy/Models/AutopsyTable","Neonatal/Models/Neonatal","Additional/Models/Generator"})
            {
                var model=UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Phase2/Imported/"+file+".fbx");
                var obj=UnityEngine.Object.Instantiate(model);var rs=obj.GetComponentsInChildren<Renderer>();var box=rs[0].bounds;foreach(var r in rs.Skip(1))box.Encapsulate(r.bounds);
                Debug.Log("P2_SOURCE_AXES "+file+" rotation="+obj.transform.eulerAngles+" scale="+obj.transform.localScale+" bounds="+box);
                if(file.Contains("Stairs"))foreach(var f in obj.GetComponentsInChildren<MeshFilter>())
                {var verts=f.sharedMesh.vertices;var indices=f.sharedMesh.triangles;var levels=new System.Collections.Generic.List<string>();for(int k=0;k<indices.Length;k+=3){var a=f.transform.TransformPoint(verts[indices[k]]);var b=f.transform.TransformPoint(verts[indices[k+1]]);var c=f.transform.TransformPoint(verts[indices[k+2]]);var normal=Vector3.Cross(b-a,c-a).normalized;if(Mathf.Abs(normal.y)>.99f)levels.Add((normal.y>0?"UP ":"DOWN ")+((a.y+b.y+c.y)/3).ToString("0.000")+" at="+((a+b+c)/3));}Debug.Log("P2_STEP_SURFACES "+string.Join(" / ",levels.Distinct()));}
                UnityEngine.Object.DestroyImmediate(obj);
            }
            var points=new[]{new Vector3(5,0,-10),new Vector3(5,0,-6),new Vector3(5,0,2),new Vector3(1,0,3),
                new Vector3(2,.2f,4.5f),new Vector3(2,1.6f,8),new Vector3(2,3.6f,13),new Vector3(2,4,15),new Vector3(2,4,18),new Vector3(18,4,12),
                new Vector3(33,0,3),new Vector3(34,-.2f,4.5f),new Vector3(34,-2,9),new Vector3(34,-4,15),new Vector3(34,-4,17)};
            foreach(var f in UnityEngine.Object.FindObjectsByType<MeshFilter>(FindObjectsSortMode.None))
            {
                var c=f.GetComponent<Collider>();if(!c||!f.sharedMesh)continue;
                if(c.bounds.Contains(new Vector3(2,4,11.51f))||f.name.Contains("StairFlight"))
                {var r=f.GetComponent<Renderer>();var v=f.sharedMesh.vertices.Select(x=>f.transform.TransformPoint(x)).ToArray();var b=new Bounds(v[0],Vector3.zero);foreach(var vertex in v)b.Encapsulate(vertex);Debug.Log("P2_NAV_MESH "+f.name+" root="+f.transform.parent.name+" pos="+f.transform.parent.position+" render="+r.bounds+" phys="+c.bounds+" vertices="+b);}
            }
            for(int i=0;i<points.Length;i++)
            {
                var p=points[i];bool sample=NavMesh.SamplePosition(p,out var hit,1,NavMesh.AllAreas);
                Debug.Log("P2_NAV_NODE "+i+" "+p+" sample="+sample+" at="+hit.position);
                if(i>0)
                {
                    var previous=points[i-1];NavMesh.SamplePosition(previous,out var from,1,NavMesh.AllAreas);var path=new NavMeshPath();
                    NavMesh.CalculatePath(from.position,hit.position,NavMesh.AllAreas,path);
                    Debug.Log("P2_NAV_EDGE "+(i-1)+"->"+i+" "+path.status+" corners="+string.Join(" / ",path.corners.Select(x=>x.ToString())));
                    Vector3 start=previous+Vector3.up,delta=p-previous;
                    Debug.Log("P2_NAV_SOLIDS "+string.Join(" / ",Physics.RaycastAll(start,delta.normalized,delta.magnitude,~0,QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance).Select(h=>h.collider.name+" "+h.point).Take(8)));
                }
            }
        }
    }
}
#endif
