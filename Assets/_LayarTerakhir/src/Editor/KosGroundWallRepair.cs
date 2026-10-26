#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static class KosGroundWallRepair
    {
        public static void Apply()
        {
            var scene=EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var root=GameObject.Find("Kos_Building_Corridors").transform;
            foreach(int side in new[]{-1,1})
            {
                string name=side<0?"GroundHallEndWall_West":"GroundHallEndWall_East";
                var wall=root.Find(name);
                if(!wall)
                {
                    // Reuse the existing rendered plaster section and solid
                    // collider, rather than adding an invisible barrier.
                    var source=root.Cast<Transform>().First(t=>t.name=="HallWallEndPier"&&Mathf.Sign(t.localPosition.x)==side&&t.localPosition.y>3);
                    wall=UnityEngine.Object.Instantiate(source,root);wall.name=name;
                }
                wall.localPosition=new Vector3(side*1.5f,1.45f,11.5f);
                wall.localRotation=Quaternion.identity;wall.localScale=new Vector3(.18f,2.9f,1);
                wall.gameObject.SetActive(true);wall.GetComponent<Renderer>().enabled=true;
                var collider=wall.GetComponent<BoxCollider>();collider.enabled=true;collider.isTrigger=false;
            }
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
            Debug.Log("KOS_GROUND_WALL_REPAIR two visible solid sections, z=11..12; stair entrance preserved");
        }
    }
}
#endif
