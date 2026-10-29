#if UNITY_EDITOR
using System;
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static class MelatiStreetRevision
    {
        private const string Art="Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/";
        public static void Apply()
        {
            MelatiNeighborhoodBuilder.Apply();
            var scene=EditorSceneManager.GetActiveScene();var d=Object.FindFirstObjectByType<KosCorridorDirector>();
            RestoreKosLot(d);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();Validate();
        }
        // Reapply the approved lot without rebuilding the neighborhood or
        // deleting its permanent street extension.
        public static void RestoreKosLot(KosCorridorDirector d)
        {
            var building=GameObject.Find("Kos_Building_Corridors").transform;
            var offset=new Vector3(-16,0,0)-building.position;building.position+=offset;d.buildingRoot=building;
            foreach(var anchor in new[]{d.spawnLanding,d.spawnGround,d.motor,d.gateHinge,d.rideAnchor})
                if(anchor&&!anchor.IsChildOf(building))anchor.position+=offset;
            d.player.transform.position+=offset;
            var old=building.Find("RestoredKosMasonry");if(old)Object.DestroyImmediate(old.gameObject);
            var shell=new GameObject("RestoredKosMasonry").transform;shell.SetParent(building,false);
            var plaster=AssetDatabase.LoadAssetAtPath<Material>(Art+"KosCorridorPlaster.mat");
            void Wall(string name,Vector3 p,Vector3 size)
            {
                var o=GameObject.CreatePrimitive(PrimitiveType.Cube);o.name=name;o.transform.SetParent(shell,false);o.transform.localPosition=p;o.transform.localScale=size;o.GetComponent<Renderer>().sharedMaterial=plaster;
            }
            foreach(int side in new[]{-1,1})
            {
                Wall("KosYardSideMasonry",new Vector3(side*5.5f,1.3f,-4),new Vector3(.2f,2.6f,8));
                Wall("KosGateMasonryReturn",new Vector3(side*3.55f,1.3f,-7),new Vector3(4.2f,2.6f,.22f));
            }
            foreach(var renderer in building.GetComponentsInChildren<MeshRenderer>(true))
                if(renderer.name.Contains("Wall")||renderer.name.Contains("Partition")||renderer.name.Contains("Pier")||renderer.name.Contains("Header"))
                {renderer.enabled=true;renderer.forceRenderingOff=false;renderer.gameObject.SetActive(true);}
        }
        public static void Validate()
        {
            Physics.SyncTransforms();
            var d=Object.FindFirstObjectByType<KosCorridorDirector>();var buildings=GameObject.Find("KampungMelati").transform.Cast<Transform>().Where(t=>t.name.StartsWith("MelatiResidence_")).ToArray();
            int types=buildings.SelectMany(t=>t.GetComponentsInChildren<Transform>()).Where(t=>t.name.StartsWith("DownloadedHouse_Type_")).Select(t=>t.name).Distinct().Count();
            if(types!=12||buildings.Length<43)throw new Exception("House variety/density failed: "+types+" types, "+buildings.Length+" houses");
            if(!d.buildingRoot||Mathf.Abs(d.buildingRoot.position.x+16)>.01f)throw new Exception("Kos is not on its side lot");
            var geometry=d.buildingRoot.GetComponentsInChildren<Renderer>().Where(r=>r.name!="KosStreet").ToArray();
            if(geometry.Any(r=>r.bounds.max.x> -3.6f))throw new Exception("Kos intrudes into the main road");
            foreach(var model in buildings.SelectMany(t=>t.GetComponentsInChildren<Transform>()).Where(t=>t.name.StartsWith("DownloadedHouse_Type_")))
            {
                var prefab=PrefabUtility.GetCorrespondingObjectFromSource(model.gameObject);
                var residence=model.parent;var hull=residence.GetComponent<BoxCollider>();
                var centre=hull.bounds.center;Vector3 front=Mathf.Abs(centre.z-37)<.1f?Vector3.back:new Vector3(-Mathf.Sign(centre.x),0,0);
                if(!prefab||Quaternion.Angle(model.rotation,Quaternion.LookRotation(front)*prefab.transform.rotation)>.1f)throw new Exception("Imported house axes overwritten: "+model.name);
                var foundation=residence.Find("HouseFoundation").GetComponent<BoxCollider>();
                if(foundation.bounds.min.y> -.08f||foundation.bounds.max.y<0)throw new Exception("House foundation does not contact soil");
            }
            var walls=d.buildingRoot.GetComponentsInChildren<BoxCollider>().Where(c=>c.name=="KosYardSideMasonry"||c.name=="KosGateMasonryReturn").ToArray();
            if(walls.Length!=4||walls.Any(c=>!c.enabled||!c.GetComponent<Renderer>().enabled))throw new Exception("Kos yard masonry missing");
            foreach(var wall in walls)
                if(!Physics.OverlapSphere(wall.bounds.center,.08f,~0,QueryTriggerInteraction.Ignore).Contains(wall))throw new Exception("Kos masonry does not collide");
            var gate=d.BuildingPoint(new Vector3(0,0,-7));
            if(Physics.CheckCapsule(gate+Vector3.up*.4f,gate+Vector3.up*1.4f,.30f,~0,QueryTriggerInteraction.Ignore))throw new Exception("Restored yard walls block the open gate");
            foreach(var renderer in buildings.SelectMany(t=>t.GetComponentsInChildren<Renderer>()))
                if(renderer.sharedMaterials.Any(m=>!m||!m.shader||m.shader.name=="Hidden/InternalErrorShader"))throw new Exception("House material missing: "+renderer.name);
            for(float z=-100;z<=45;z+=1)
            {
                var hits=Physics.RaycastAll(new Vector3(0,2,z),Vector3.down,4,~0,QueryTriggerInteraction.Ignore);
                if(!hits.Any(h=>h.collider.name.StartsWith("MelatiRoad")||h.collider.name=="MelatiCrossStreet"||h.collider.name=="MelatiRearLane"))throw new Exception("Main road is not continuous at z="+z);
                if(Physics.CheckCapsule(new Vector3(0,.4f,z),new Vector3(0,1.4f,z),.30f,~0,QueryTriggerInteraction.Ignore))throw new Exception("Main road blocked at z="+z);
            }
            foreach(var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None))
                if(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)>0)throw new Exception("Missing script "+t.name);
            System.IO.File.WriteAllText("Audit/melati-street-revision-validation.json",JsonUtility.ToJson(new Report{utc=DateTime.UtcNow.ToString("o"),houseCount=buildings.Length,houseTypes=types,kosPosition=d.buildingRoot.position,restoredYardWalls=walls.Length,roadSamples=146,passed=true},true)+"\n");
            Debug.Log("MELATI_STREET_REVISION_PASS houses="+buildings.Length+" types="+types+" walls=4 road=146");
        }
        [Serializable] private class Report{public string utc;public int houseCount,houseTypes,restoredYardWalls,roadSamples;public Vector3 kosPosition;public bool passed;}
    }
}
#endif
