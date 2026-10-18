#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace LayarTerakhir.Editor
{
    public static class MelatiResidenceFactory
    {
        private static readonly string[] types={"a","b","d","f","h","k","m","p","i","l","n","q"};
        private static int next;
        private const string Art="Assets/_LayarTerakhir/assets/Art/";
        public static void Reset(){next=0;}
        public static void Build(Transform parent,Vector3 position,int side,int index,bool rear)
        {
            int variant=next++%types.Length;
            var house=new GameObject("MelatiResidence_"+position.x+"_"+position.z).transform;house.SetParent(parent,false);
            var source=AssetDatabase.LoadAssetAtPath<GameObject>(Art+"Review/Models/MelatiHouse_"+types[variant]+".fbx");
            if(!source)throw new InvalidOperationException("Missing downloaded house type "+types[variant]);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(source);model.name="DownloadedHouse_Type_"+types[variant];model.transform.SetParent(house,false);
            var front=position.z==37?Vector3.back:new Vector3(-side,0,0);model.transform.rotation=Quaternion.LookRotation(front)*model.transform.rotation;
            var bounds=KosVisualV2.BoundsOf(model);float scale=Mathf.Min((rear?7.4f:7.6f)/bounds.size.x,(rear?9f:9.6f)/bounds.size.z);
            model.transform.localScale*=scale;bounds=KosVisualV2.BoundsOf(model);model.transform.position+=position-new Vector3(bounds.center.x,bounds.min.y,bounds.center.z);
            foreach(var renderer in model.GetComponentsInChildren<Renderer>())renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>MaterialFor(m.name,variant)).ToArray();
            bounds=KosVisualV2.BoundsOf(model);
            var foundation=GameObject.CreatePrimitive(PrimitiveType.Cube);foundation.name="HouseFoundation";foundation.transform.SetParent(house,false);foundation.transform.position=new Vector3(bounds.center.x,-.04f,bounds.center.z);foundation.transform.localScale=new Vector3(bounds.size.x*.94f,.16f,bounds.size.z*.94f);foundation.GetComponent<Renderer>().sharedMaterial=MaterialFor("Concrete",variant);
            var hull=house.gameObject.AddComponent<BoxCollider>();hull.center=bounds.center;hull.size=new Vector3(bounds.size.x*.94f,bounds.size.y,bounds.size.z*.94f);
            var doorstep=GameObject.CreatePrimitive(PrimitiveType.Cube);doorstep.name="ResidenceDoorstep";doorstep.transform.SetParent(house,false);doorstep.transform.position=position+front*(Mathf.Abs(front.x)>.5f?bounds.extents.x:bounds.extents.z)+Vector3.up*.06f;
            doorstep.transform.localScale=Mathf.Abs(front.x)>.5f?new Vector3(.55f,.12f,1.8f):new Vector3(1.8f,.12f,.55f);doorstep.GetComponent<Renderer>().sharedMaterial=MaterialFor("Concrete",variant);
            if(!rear)
            {
                var lamp=new GameObject("PorchPractical",typeof(Light));lamp.transform.SetParent(house,false);lamp.transform.position=doorstep.transform.position+Vector3.up*2.4f;
                var light=lamp.GetComponent<Light>();light.type=LightType.Point;light.intensity=2.8f;light.range=5;light.color=new Color(1,.77f,.49f);light.shadows=LightShadows.None;
            }
        }
        private static Material MaterialFor(string region,int variant)
        {
            string source=region.Contains("Roof")?"House_MAT_RoofTiles":region.Contains("Wood")?"House_MAT_WoodDoor":region.Contains("Glass")?null:region.Contains("Concrete")?"House_MAT_Concrete":"House_MAT_PlasterDamaged";
            if(source==null)return AssetDatabase.LoadAssetAtPath<Material>(Art+"Review/Materials/Review_HouseGlass.mat");
            string path=Art+"Review/Materials/Melati_Type"+variant+"_"+source+".mat";var material=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!material)
            {
                material=new Material(AssetDatabase.LoadAssetAtPath<Material>(Art+"Atmaja/Materials/"+source+".mat"));material.name="Melati_Type"+variant+"_"+source;
                if(source=="House_MAT_PlasterDamaged")
                {
                    Color[] colors={new Color(.67f,.64f,.51f),new Color(.42f,.50f,.43f),new Color(.61f,.49f,.40f),new Color(.51f,.54f,.57f)};
                    material.SetColor("_BaseColor",colors[variant%colors.Length]);
                }
                material.SetFloat("_Smoothness",.22f);AssetDatabase.CreateAsset(material,path);
            }
            return material;
        }
    }
}
#endif
