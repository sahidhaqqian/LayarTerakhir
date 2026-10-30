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
    // Small corrections to the furnished scene. No historical scene regeneration.
    public static class PrologEasySceneRepair
    {
        private static GameObject Find(string name)=>Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None).FirstOrDefault(t=>t.name==name)?.gameObject;
        private static void Delete(string name){var go=Find(name);if(go)Object.DestroyImmediate(go);}
        private static void RotateCentred(GameObject go,float yaw)
        {
            if(!go)throw new InvalidOperationException("Missing reviewed object");
            Vector3 center=KosVisualV2.BoundsOf(go).center;
            go.transform.rotation=Quaternion.Euler(0,yaw,0);
            go.transform.position+=center-KosVisualV2.BoundsOf(go).center;
        }
        private static void Save()
        {
            var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
        }
        public static void Apply()
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            RotateCentred(Find("KosDeskFan"),162);Save();

            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            Delete("FloorTwoWayfinding");Delete("GroundWayfinding");
            var yard=Find("KosYard");yard.transform.localPosition=new Vector3(0,-.08f,-4);yard.transform.localScale=new Vector3(11,.16f,8);
            var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();
            foreach(string gender in new[]{"WANITA","PRIA"})
            {
                Delete("Bathroom"+gender+"Sign");Delete("Bathroom"+gender+"Pictogram");
                var door=Find("Bathroom"+gender+"Door");
                var go=new GameObject("Bathroom"+gender+"Pictogram",typeof(TextMesh));
                go.transform.SetPositionAndRotation(door.transform.position+Vector3.up*.45f-corridor.buildingRoot.right*.045f,Quaternion.LookRotation(corridor.buildingRoot.right));
                var text=go.GetComponent<TextMesh>();text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.text=gender=="WANITA"?"♀":"♂";
                text.fontSize=120;text.characterSize=.018f;text.anchor=TextAnchor.MiddleCenter;text.alignment=TextAlignment.Center;text.color=new Color(.85f,.86f,.80f);
                go.GetComponent<MeshRenderer>().sharedMaterial=text.font.material;
                go.transform.SetParent(door.transform,true);
            }
            Save();

            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();
            foreach(var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(t=>t.name.StartsWith("PortraitDosa")).ToArray())
                if(t&&(t.name.StartsWith("PortraitDosa2")||t.name.StartsWith("PortraitDosa3")||t.name.Contains("_Plaque")||t.name.Contains("_BloodAR")||t.name.Contains("_Gouge")))Object.DestroyImmediate(t.gameObject);
            d.upper.portraitBloodAR=Array.Empty<GameObject>();
            foreach(var t in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t=>t.name=="CorrLintelN"))
            {
                float x=-Mathf.Abs(d.LocalPosition(t.position).x),bottom=x<-3?5.92f:5.82f;
                t.position=d.P(x,5.2f,(bottom+6.83f)/2);t.localScale=new Vector3(x<-3?1.1f:1.0f,6.83f-bottom,.18f);
                t.name=x<-3?"ChildDoorHeader47":"BathDoorHeader47";
            }
            var header=Find("FrontSittingDoorLintel");
            if(header){header.transform.position=d.P(-1.4f,2.5f,(2.71f+3.68f)/2);var size=header.transform.localScale;size.y=3.68f-2.71f;header.transform.localScale=size;}
            RotateCentred(Find("ChildDeskLamp"),180);Save();
            Delete("KitchenWindowGlimmerAnchor");
            var marker=new GameObject("KitchenWindowGlimmerAnchor").transform;marker.SetParent(Find("Atmaja_Environment_Art").transform,true);marker.position=d.P(6.10f,10.975f,1.45f);
            ARPhotoGlimmer.Attach(marker,d.upper,"EVENT_P0_KITCHEN_ENTERED",false,true,false);Save();
            Debug.Log("PROLOG_EASY_REPAIR_APPLIED — furnished scenes patched, Raka assets unchanged");
        }
    }
}
#endif
