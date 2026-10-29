#if UNITY_EDITOR
using System;
using System.Linq;
using System.IO;
using LayarTerakhir.Interlude;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class PrologAcceptanceRepair
    {
        public static void RepairAndCapture()
        {RestoreStreet();RepairSharedEntranceAndSlippers();PrologReviewRevision.ApplyAndCapture();CaptureStaticAudit();}
        public static void RepairBathroomSignAndCapture()
        {
            var scene=EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);var title=GameObject.Find("KAMAR MANDISign");if(title)Object.DestroyImmediate(title);
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
            var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();var camera=Camera.main;
            GameObject.Find("KAMAR MANDIDoor").GetComponent<HingedDoor>().SetOpenImmediate(true);
            var eye=corridor.BuildingPoint(new Vector3(.15f,1.7f,4.25f));var aim=corridor.BuildingPoint(new Vector3(2.82f,1.55f,4.25f));camera.transform.SetPositionAndRotation(eye,Quaternion.LookRotation(aim-eye));
            KosRenderDiagnosis.Capture(camera,"bug08-outer-bathroom-no-floating-title",false,"Audit/Visuals/Prolog-Acceptance");
        }
        public static void RepairFinalBatch()
        {
            RepairBathroomDoorInteraction();var scene=SceneManager.GetActiveScene();var d=Object.FindFirstObjectByType<KosCorridorDirector>();
            KosVisualV2.SetCenter(GameObject.Find("RealCleaner"),d.BuildingPoint(new Vector3(-4.6f,.15f,5.85f)));KosVisualV2.SetCenter(GameObject.Find("RealPlunger"),d.BuildingPoint(new Vector3(-4.6f,.25f,6.08f)));
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
            RakaPresentationRevisionBuilder.BakeCallContacts();PrologReviewRevision.Apply();
            var director=Object.FindFirstObjectByType<AtmajaDirector>();var camera=Camera.main;
            camera.transform.SetPositionAndRotation(director.P(-1.4f,3.6f,2.15f),Quaternion.LookRotation(director.P(-1.4f,2.5f,2.7f)-director.P(-1.4f,3.6f,2.15f)));
            KosRenderDiagnosis.Capture(camera,"bug18-ground-header",false,"Audit/Visuals/Prolog-Acceptance");
        }
        public static void RepairBathroomDoorInteraction()
        {
            var scene=EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            foreach(string title in new[]{"WANITA","PRIA"}){var panel=GameObject.Find("Bathroom"+title+"Door");if(!panel.GetComponent<BoxCollider>())panel.AddComponent<BoxCollider>();}
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);
        }
        public static void RepairFacilitiesAndCapture(){RepairSharedEntranceAndSlippers();CaptureStaticAudit();}
        public static void RepairSharedEntranceAndSlippers()
        {
            var scene=EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var objects=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).ToArray();
            foreach(var t in objects.Where(t=>(t.name.StartsWith("KosGroundDoor_-1.5_2")&&!t.name.EndsWith("Frame")&&!t.name.EndsWith("Lintel"))||new[]{"SlipperStrapL","SlipperStrapR","SlipperToePost","BathroomWaterTank","BathroomTankLid","RealVeggieBox","SharedKitchenCounterFront","KitchenCounterTopFront"}.Contains(t.name)).ToArray())if(t)Object.DestroyImmediate(t.gameObject);
            foreach(var t in objects.Where(t=>t&&(t.name=="RoomPartition"||t.name=="DAPUR BERSAMASide")&&t.localPosition.x<0&&Mathf.Abs(t.localPosition.y-1.4f)<.1f&&Mathf.Abs(t.localPosition.z-6)<.02f).ToArray())Object.DestroyImmediate(t.gameObject);
            foreach(var name in new[]{"SharedKitchenCounter","KitchenCounterTop"})
            {
                var counter=GameObject.Find(name);counter.transform.localPosition=new Vector3(-4.2f,name=="SharedKitchenCounter"?.45f:.92f,5.075f);
                var scale=counter.transform.localScale;scale.z=1.15f;counter.transform.localScale=scale;
                var front=Object.Instantiate(counter,counter.transform.parent);front.name=name+"Front";front.transform.localPosition=new Vector3(-4.2f,counter.transform.localPosition.y,3.575f);scale.z=.45f;front.transform.localScale=scale;
            }
            foreach(var food in objects.Where(t=>t&&(t.name=="SharedDiningFood"||t.name=="SharedDiningFruit")).ToArray())
            {
                if(food.name=="SharedDiningFood")KosCorridorBuilder.ServeKosBreakfast(food.parent,KosVisualV2.BoundsOf(food.gameObject).center+new Vector3(.035f,0,0),0);
                Object.DestroyImmediate(food.gameObject);
            }
            foreach(string gender in new[]{"WANITA","PRIA"})
            {
                var panel=GameObject.Find("Bathroom"+gender+"Door").transform;
                foreach(var t in panel.GetComponentsInChildren<Transform>(true).Where(t=>t.name.Contains("Icon")||t.name.EndsWith("Pictogram")).ToArray())if(t)Object.DestroyImmediate(t.gameObject);
                KosCorridorBuilder.AttachBathroomPictogram(gender,panel,Object.FindFirstObjectByType<KosCorridorDirector>().BuildingPoint(new Vector3(2.733f,1.78f,gender=="WANITA"?5.2f:3.3f)),Quaternion.Euler(0,90,0));
            }
            foreach(var spec in new[]{("RealTissueW",5.88f),("RealTissueP",4.70f)})KosVisualV2.SetCenter(GameObject.Find(spec.Item1),Object.FindFirstObjectByType<KosCorridorDirector>().BuildingPoint(new Vector3(5.15f,.85f,spec.Item2)));
            KosVisualV2.SetCenter(GameObject.Find("RealCleaner"),Object.FindFirstObjectByType<KosCorridorDirector>().BuildingPoint(new Vector3(-4.6f,.15f,5.85f)));
            KosVisualV2.SetCenter(GameObject.Find("RealPlunger"),Object.FindFirstObjectByType<KosCorridorDirector>().BuildingPoint(new Vector3(-4.6f,.25f,6.08f)));
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Original/Models/IndonesianFlipFlop.fbx");
            if(!prefab)throw new Exception("The Blender flip-flop model has not imported");
            Material Rubber(string name,Color color)
            {
                string path=PrologReviewRevision.Art+"Materials/"+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
                if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,path);}mat.color=color;mat.SetFloat("_Smoothness",.12f);EditorUtility.SetDirty(mat);return mat;
            }
            var rubber=Rubber("FlipFlopRubber",new Color(.035f,.045f,.037f));var blue=Rubber("FlipFlopBlueStrap",new Color(.065f,.19f,.29f));var edge=Rubber("FlipFlopEdge",new Color(.08f,.09f,.075f));
            var toilet=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/RealToilet.mat");toilet.color=new Color(.84f,.83f,.79f);toilet.SetTexture("_BaseMap",null);toilet.SetTexture("_BumpMap",null);toilet.SetTexture("_MetallicGlossMap",null);toilet.DisableKeyword("_NORMALMAP");toilet.DisableKeyword("_METALLICSPECGLOSSMAP");toilet.SetFloat("_Metallic",0);toilet.SetFloat("_Smoothness",.48f);EditorUtility.SetDirty(toilet);
            GameObject.Find("KAMAR MANDITiledWainscot").transform.localPosition=new Vector3(6.14f,.55f,4.25f);
            var mirror=GameObject.Find("BathroomMirror").transform;mirror.localPosition=new Vector3(3.8f,1.6f,5.89f);mirror.localRotation=Quaternion.identity;mirror.localScale=new Vector3(.4f,.5f,.03f);
            foreach(var spec in new[]{("RealToiletW",5.2f),("RealToiletP",3.2f)})
            {
                var toiletObject=GameObject.Find(spec.Item1);toiletObject.transform.localRotation=Quaternion.Euler(0,90,0);
                KosVisualV2.SetCenter(toiletObject,Object.FindFirstObjectByType<KosCorridorDirector>().BuildingPoint(new Vector3(5.65f,.4f,spec.Item2)));
            }
            var sink=GameObject.Find("RealBathSink");KosVisualV2.Resize(sink,new Vector3(.55f,.85f,.3f));KosVisualV2.SetCenter(sink,Object.FindFirstObjectByType<KosCorridorDirector>().BuildingPoint(new Vector3(3.8f,.425f,5.75f)));
            GameObject Slipper()
            {
                var go=new GameObject("TenantSlipper");go.transform.rotation=Quaternion.Euler(0,90,0);
                var model=(GameObject)PrefabUtility.InstantiatePrefab(prefab);model.transform.SetParent(go.transform,false);
                foreach(var r in model.GetComponentsInChildren<Renderer>())r.sharedMaterials=r.sharedMaterials.Select(m=>m.name.Contains("BlueStrap")?blue:m.name.Contains("Edge")?edge:rubber).ToArray();
                KosVisualV2.Resize(go,new Vector3(.27f,.045f,.10f));KosVisualV2.SetCenter(go,Vector3.zero);return go;
            }
            var template=Slipper();PrefabUtility.SaveAsPrefabAsset(template,"Assets/Resources/IndonesianFlipFlop.prefab");Object.DestroyImmediate(template);
            var tenantPositions=objects.Where(t=>t&&t.name=="TenantSlipper").Select(t=>t.position).ToArray();
            foreach(var old in objects.Where(t=>t&&(t.name=="TenantSlipper"||t.name=="InterludeSandals")).ToArray())
            {
                if(old.name=="InterludeSandals"&&tenantPositions.Any(p=>Vector3.Distance(p,old.position)<.5f)){Object.DestroyImmediate(old.gameObject);continue;}
                var bounds=KosVisualV2.BoundsOf(old.gameObject);var go=Slipper();go.name=old.name;go.transform.SetParent(old.parent,true);
                KosVisualV2.SetCenter(go,new Vector3(bounds.center.x,bounds.min.y+.0225f,bounds.center.z));Object.DestroyImmediate(old.gameObject);
            }
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();Debug.Log("PROLOG_SHARED_ENTRANCE_AND_SLIPPERS_REPAIRED");
        }
        public static void CaptureStaticAudit()
        {
            const string folder="Audit/Visuals/Prolog-Acceptance";Directory.CreateDirectory(folder);
            void Shot(string name,Vector3 eye,Vector3 aim)
            {
                var camera=Camera.main;if(!camera)throw new Exception("Gameplay camera missing for "+name);
                camera.transform.SetPositionAndRotation(eye,Quaternion.LookRotation(aim-eye));
                KosRenderDiagnosis.Capture(camera,name,false,folder);
            }
            Vector3 Centre(string name)=>KosVisualV2.BoundsOf(GameObject.Find(name)).center;
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var fan=Centre("KosDeskFan");Shot("bug01-fan-context",Camera.main.transform.position,fan);Shot("bug01-fan-detail",fan+new Vector3(0,.1f,-1.2f),fan);
            var sandals=Centre("KosSandals");Shot("bedroom-sandals-source",sandals+new Vector3(.45f,.9f,.55f),sandals);
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();
            Vector3 BP(float x,float y,float z)=>corridor.BuildingPoint(new Vector3(x,y,z));
            Shot("bug03-06-upper-doors",BP(0,4.95f,2.8f),BP(-1.45f,4.4f,1.5f));
            Shot("bug05-06-stair-landing",BP(.55f,4.95f,14.8f),BP(.55f,4.8f,17));
            Shot("bug06-lower-exit",BP(.55f,1.7f,14.8f),BP(.55f,1.6f,17));
            Shot("bug07-open-shared-kitchen",BP(0,1.7f,6.5f),BP(-3.5f,1.2f,7.5f));
            Shot("bug10-kitchen",BP(-1.95f,1.7f,3.4f),BP(-4.05f,1.0f,4.7f));
            Shot("bug09-bathroom",BP(3.4f,1.65f,5.2f),BP(5.65f,.50f,5.2f));
            Shot("bug08-bathroom-signs",BP(2.0f,1.7f,5.2f),BP(2.82f,1.78f,5.2f));
            Shot("bug08-men-sign",BP(2.0f,1.7f,3.3f),BP(2.82f,1.78f,3.3f));
            GameObject.Find("BathroomWANITADoor").GetComponent<HingedDoor>().SetOpenImmediate(true);
            Shot("bug08-open-door-sign",BP(2.0f,1.7f,5.2f),Centre("BathroomWANITAPlaque"));
            var slipperObject=Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).First(t=>t.name=="TenantSlipper"&&t.localPosition.x<0&&t.position.y<1);var slipper=KosVisualV2.BoundsOf(slipperObject.gameObject).center;Shot("bug04-corridor-slippers",slipper+new Vector3(.45f,.9f,.55f),slipper);
            Shot("bug07-kitchen-to-dining",BP(-3,1.7f,5.1f),BP(-3,1.3f,8.4f));
            foreach(var name in new[]{"TenantMotor_0","TenantMotor_2","RakaMotor"}){var centre=Centre(name);Shot("bug11-12-"+name,centre+new Vector3(name=="TenantMotor_2"?-2.6f:2.6f,1.5f,2.6f),centre+Vector3.up*.45f);}
            Shot("bug47-night-street",new Vector3(0,1.6f,-30),new Vector3(0,4,-70));
            InterludeExterior.ApplyDay(Camera.main);Shot("bug47-morning-street",new Vector3(0,1.6f,-30),new Vector3(0,4,-70));
            InterludeExterior.ApplyDusk(Camera.main);Shot("bug47-dusk-street",new Vector3(0,1.6f,-30),new Vector3(0,4,-70));
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);var d=Object.FindFirstObjectByType<AtmajaDirector>();
            Shot("bug17-well-chain",d.P(4.2f,9.8f,2),d.P(2.85f,11.2f,1.12f));
            var chain=Centre("FrontDoorRustChain");Shot("bug17-front-door-chain",chain-d.north*1.5f+Vector3.up*.3f,chain);
            Shot("bug18-ground-header",d.P(-1.4f,3.6f,2.15f),d.P(-1.4f,2.5f,2.7f));
            Shot("bug22-blood-trail",d.P(-2.8f,6.05f,5.45f),d.P(-.7f,5.35f,4.75f));
            var hand=Centre("BloodHandprintF2");Shot("bug22-handprint-detail",hand+d.north*1.1f+Vector3.up*.15f,hand);
            var stain=Centre("BloodStainFloor");Shot("bug22-floor-blood",stain+d.north*.7f+Vector3.up*1.2f,stain);
            Shot("bug23-child-header",d.P(-3,5.95f,5.45f),d.P(-4.4f,5.2f,5.85f));
            Shot("bug23-bath-header",d.P(-1.1f,6,5.45f),d.P(-1.1f,5.2f,5.85f));
            Shot("bug25-27-family-photo",d.P(.1f,6,5.45f),d.P(.1f,5.365f,5.35f));
            Shot("bug32-child-corner",d.P(-5.1f,3.9f,5.45f),d.P(-3.15f,1.4f,4.45f));
            Shot("bug38-noose-lamp",d.P(1.8f,5.85f,5.45f),d.upper.ropeAim.position+Vector3.up*.8f);
            Debug.Log("PROLOG_ACCEPTANCE_STATIC_CAPTURES_COMPLETE");
        }
        // Recover only the permanent environment lost by the corridor rebuild.
        // The furnished rooms, doors and revised parking remain in the current scene.
        public static void RestoreStreet()
        {
            var scene=EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();
            var neighborhood=GameObject.Find("KampungMelati");
            if(!neighborhood)
            {
                var permanent=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/MelatiPermanentNeighborhood.prefab");
                if(permanent){neighborhood=(GameObject)PrefabUtility.InstantiatePrefab(permanent);neighborhood.name="KampungMelati";}
            }
            if(!neighborhood)
            {
                const string legacyPath="Assets/_LayarTerakhir/assets/Scenes/Prolog_AuditLegacyKos.unity";
                var legacy=EditorSceneManager.OpenScene(legacyPath,OpenSceneMode.Additive);
                var original=legacy.GetRootGameObjects().Single(g=>g.name=="KampungMelati");
                neighborhood=Object.Instantiate(original);neighborhood.name=original.name;
                SceneManager.MoveGameObjectToScene(neighborhood,scene);
                // Instantiate remaps internal references. Rebind source-scene
                // references to the matching live scene objects before closing it.
                foreach(var component in neighborhood.GetComponentsInChildren<Component>(true).Where(c=>c))
                {
                    var serialized=new SerializedObject(component);var property=serialized.GetIterator();
                    while(property.Next(true))
                    {
                        if(property.propertyType!=SerializedPropertyType.ObjectReference)continue;
                        var reference=property.objectReferenceValue;
                        var sourceObject=reference is Component c?c.gameObject:reference as GameObject;
                        if(!sourceObject||sourceObject.scene!=legacy)continue;
                        var match=scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).FirstOrDefault(t=>t.name==sourceObject.name);
                        property.objectReferenceValue=match?(reference is Component?match.GetComponent(reference.GetType()):match.gameObject):null;
                    }
                    serialized.ApplyModifiedPropertiesWithoutUndo();
                }
                EditorSceneManager.CloseScene(legacy,true);SceneManager.SetActiveScene(scene);
            }
            neighborhood.SetActive(true);
            var street=neighborhood.GetComponent<MelatiNeighborhood>();
            street.corridor=corridor;corridor.rideExit=street.parkedMotor;
            foreach(var t in scene.GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).Where(t=>new[]{"NeighbourBoundary","StreetEndBoundary","YardBoundary","KosGateBoundary"}.Contains(t.name)).ToArray())
                Object.DestroyImmediate(t.gameObject);
            if(Mathf.Abs(corridor.buildingRoot.position.x+16)>.01f)MelatiStreetRevision.RestoreKosLot(corridor);
            InterludeExterior.EnsureNeighborhood(corridor);
            if(!AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/MelatiPermanentNeighborhood.prefab"))
            {
                street.corridor=null;PrefabUtility.SaveAsPrefabAsset(neighborhood,"Assets/Resources/MelatiPermanentNeighborhood.prefab");street.corridor=corridor;
            }
            Physics.SyncTransforms();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            Debug.Log("PROLOG_ACCEPTANCE_STREET_RESTORED renderers="+neighborhood.GetComponentsInChildren<Renderer>().Length);
        }
    }
}
#endif
