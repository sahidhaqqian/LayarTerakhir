#if UNITY_EDITOR
using System;
using System.Linq;
using System.IO;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Deliberate patches to the furnished scenes. Never regenerates the approved Kos.
    public static class Prolog47SceneRepair
    {
        private const string Models="Assets/_LayarTerakhir/assets/Art/Original/Models/Prolog47/";
        private const string Mats="Assets/_LayarTerakhir/assets/Art/Generated/Materials/Prolog47/";
        private static GameObject Find(string name)=>Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None).FirstOrDefault(t=>t.name==name)?.gameObject;
        private static void Delete(string name){var go=Find(name);if(go)Object.DestroyImmediate(go);}
        private static Material Old(string name)=>AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/"+name+".mat");
        private static Material Material(string name)
        {
            Directory.CreateDirectory(Mats);
            string source=null;Color color=Color.white;float smooth=.22f,metal=0;
            switch(name)
            {
                case "Repair_BedWood":source="Upper_PineTable";color=new Color(.42f,.34f,.25f);break;
                case "Repair_Linen":case "Repair_Mattress":color=new Color(.60f,.58f,.49f);smooth=.03f;break;
                case "Repair_Blanket":color=new Color(.18f,.23f,.22f);smooth=.03f;break;
                case "Repair_BookCloth":color=new Color(.17f,.22f,.20f);smooth=.02f;break;
                case "Repair_Paper":color=new Color(.74f,.70f,.57f);smooth=.02f;break;
                case "Repair_Ceramic":color=new Color(.78f,.75f,.67f);smooth=.38f;break;
                case "Repair_CeramicEdge":color=new Color(.12f,.085f,.06f);smooth=.08f;break;
                case "Repair_CupCeramic":color=new Color(.82f,.80f,.72f);smooth=.34f;break;
                case "Repair_CupCrack":color=new Color(.16f,.105f,.065f);smooth=.08f;break;
                case "Repair_SandalRubber":color=new Color(.11f,.16f,.17f);smooth=.08f;break;
                case "Repair_SandalStrap":color=new Color(.78f,.80f,.73f);smooth=.18f;break;
                case "Repair_Iron":color=new Color(.18f,.20f,.19f);metal=.65f;break;
                case "Repair_Brass":color=new Color(.40f,.34f,.22f);metal=.75f;smooth=.5f;break;
                case "Repair_DoorPaint":color=new Color(.46f,.51f,.44f);smooth=.25f;break;
                case "Repair_DoorInset":color=new Color(.40f,.45f,.39f);smooth=.25f;break;
            }
            string path=Mats+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!mat){mat=source!=null&&Old(source)?new Material(Old(source)):new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,path);}
            if(name=="Repair_Linen"||name=="Repair_Mattress"||name=="Repair_Blanket")mat.SetTexture("_BaseMap",null);
            mat.SetColor("_BaseColor",color);mat.SetFloat("_Smoothness",smooth);mat.SetFloat("_Metallic",metal);
            if(name=="Repair_CupCeramic"){mat.SetColor("_EmissionColor",new Color(.025f,.023f,.020f));mat.EnableKeyword("_EMISSION");}
            EditorUtility.SetDirty(mat);return mat;
        }
        private static GameObject Model(string model,string name,Transform parent,Vector3 position,Quaternion rotation)
        {
            var root=new GameObject(name);root.transform.SetParent(parent,true);root.transform.SetPositionAndRotation(position,rotation);
            var asset=AssetDatabase.LoadAssetAtPath<GameObject>(Models+model+".fbx");if(!asset)throw new InvalidOperationException("Missing authored model "+model);
            var child=(GameObject)PrefabUtility.InstantiatePrefab(asset);child.transform.SetParent(root.transform,false);
            foreach(var renderer in child.GetComponentsInChildren<Renderer>())renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>Material(m.name)).ToArray();
            if(name.EndsWith("BedSet"))foreach(var part in child.GetComponentsInChildren<Transform>())if(part.name=="Quilted mattress")part.name=name.StartsWith("Child")?"ChildMattress":"MasterMattress";
            return root;
        }
        private static void Ground(GameObject root,float y){root.transform.position+=Vector3.up*(y-KosVisualV2.BoundsOf(root).min.y);}
        private static void FitCollider(GameObject root){var col=root.GetComponent<BoxCollider>();if(!col)col=root.AddComponent<BoxCollider>();KosVisualV2.FitBox(col,KosVisualV2.BoundsOf(root));}
        private static void RotateCentred(GameObject root,float yaw)
        {
            if(!root)return;var centre=KosVisualV2.BoundsOf(root).center;root.transform.rotation=Quaternion.Euler(0,yaw,0);root.transform.position+=centre-KosVisualV2.BoundsOf(root).center;
        }
        private static void Save(){var scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();}

        public static void ApplyStageOne()
        {
            ConfigureImports();ApplySimple();ApplyFurniture();
        }
        public static void ApplyRemaining()
        {
            ConfigureImports();ApplyFurniture();ApplyKosFacilities();ApplyArchitecture();ApplyDetails();ApplyLadder();ApplyFinishProps();
        }
        private static void ConfigureImports()
        {
            AssetDatabase.Refresh();
            foreach(string root in new[]{"Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities","Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47"})
            foreach(string file in Directory.GetFiles(root,"*",SearchOption.AllDirectories).Where(p=>p.EndsWith(".png")||p.EndsWith(".jpg")))
            {
                string path=file.Replace('\\','/');var importer=AssetImporter.GetAtPath(path) as TextureImporter;if(!importer)continue;
                bool normal=path.Contains("_Normal.")||path.Contains("_nor_gl_");bool mask=path.Contains("_Mask.")||path.Contains("_mask.");
                importer.textureType=normal?TextureImporterType.NormalMap:TextureImporterType.Default;importer.sRGBTexture=!normal&&!mask;importer.maxTextureSize=1024;importer.mipmapEnabled=true;importer.anisoLevel=2;
                if(path.EndsWith("HandprintAndScratches.png")){importer.alphaIsTransparency=true;importer.wrapMode=TextureWrapMode.Clamp;}
                importer.SaveAndReimport();
            }
        }

        public static void ApplySimple()
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            RotateCentred(Find("KosDeskFan"),162);Save();
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            Delete("FloorTwoWayfinding");Delete("GroundWayfinding");
            var yard=Find("KosYard");yard.transform.localPosition=new Vector3(0,-.08f,-4);yard.transform.localScale=new Vector3(11,.16f,8);
            Save();
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var upper=Object.FindFirstObjectByType<AtmajaUpperDirector>();
            foreach(var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(t=>t.name.StartsWith("PortraitDosa")).ToArray())
                if(t&&(t.name.StartsWith("PortraitDosa2")||t.name.StartsWith("PortraitDosa3")||t.name.Contains("_Plaque")||t.name.Contains("_BloodAR")||t.name.Contains("_Gouge")))Object.DestroyImmediate(t.gameObject);
            upper.portraitBloodAR=Array.Empty<GameObject>();
            var d=upper.director;
            foreach(var lintel in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t=>t.name=="CorrLintelN"))
            {
                float localX=d.LocalPosition(lintel.position).x;
                float x=-Mathf.Abs(localX), bottom=x<-3?5.92f:5.82f;
                lintel.position=d.P(x,5.2f,(bottom+6.83f)/2);
                lintel.localScale=new Vector3(x<-3?1.1f:1.0f,6.83f-bottom,.18f);
            }
            RotateCentred(Find("ChildDeskLamp"),180);
            Save();Debug.Log("PROLOG47_SIMPLE_APPLIED no furnished scene regenerated");
        }

        public static void ApplyFurniture()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();var environment=Find("Atmaja_Environment_Art").transform;
            foreach(string name in new[]{"ChildBed","ChildMattress","ChildPillow","ChildBlanket","ChildWardrobe","ChildToyBlocks","ChildToyBall","ChildBedSet"})Delete(name);
            var bed=Model("SingleBedSet","ChildBedSet",environment,d.P(-3.10f,1.30f,3.83f),Quaternion.LookRotation(-d.north));Ground(bed,3.83f);FitCollider(bed);
            foreach(string name in new[]{"ChildDiaryBook","ChildNotebook"})Delete(name);
            var diary=Model("OpenDiary","ChildDiaryBook",environment,d.P(-5.25f,.60f,4.57f),Quaternion.LookRotation(d.north));
            FitCollider(diary);var item=diary.AddComponent<AtmajaItem>();item.director=d;item.action=AtmajaAction.Diary;item.label="Baca buku harian Bima";
            Ink(diary.transform,"Buku harian Bima\n\nIbu sekarang sering\nngomong sendiri di\nruang jahit bawah\ntanah. Tangannya\nselalu megang\ngunting taman.",-.076f);
            Ink(diary.transform,"Tadi malam Ibu\nbikinin aku susu.\nBaunya aneh, kayak\nkapur barus campur\nkarat. Kata Ibu, kami\nnggak bakal sakit-\nsakit lagi.",.076f);
            Delete("ChildMilkCup");
            var cup=Model("CrackedMilkCup","ChildMilkCup",environment,d.P(-4.92f,.63f,4.57f),Quaternion.LookRotation(-d.north));Ground(cup,4.57f);FitCollider(cup);
            item=cup.AddComponent<AtmajaItem>();item.director=d;item.action=AtmajaAction.CupPhoto;item.label="Periksa cangkir susu retak";
            d.upper.childCup=cup;d.upper.cupAim.position=KosVisualV2.BoundsOf(cup).center;
            var tubOld=Find("BathTub");if(tubOld)Object.DestroyImmediate(tubOld);
            var tub=Model("BloodBathTub","BathTub",environment,d.P(-1.1f,.93f,3.83f),Quaternion.LookRotation(d.north));Ground(tub,3.83f);
            // Separate rim/wall collision leaves the real bowl open for blood water.
            var hull=tub.AddComponent<BoxCollider>();hull.center=new Vector3(0,.27f,0);hull.size=new Vector3(1.75f,.54f,.8f);
            var water=d.upper.bathWater;water.transform.SetPositionAndRotation(d.P(-1.1f,.93f,4.27f),Quaternion.LookRotation(d.north));water.transform.localScale=new Vector3(1.49f,.008f,.59f);
            var waterMat=AssetDatabase.LoadAssetAtPath<Material>(Mats+"BloodWater.mat");if(!waterMat){waterMat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(waterMat,Mats+"BloodWater.mat");}
            waterMat.SetColor("_BaseColor",new Color(.17f,.013f,.009f));waterMat.SetFloat("_Smoothness",.85f);water.GetComponent<Renderer>().sharedMaterial=waterMat;EditorUtility.SetDirty(waterMat);
            var survey=Find("DamarSurveyInsideDining");
            if(survey)
            {
                Delete("DamarSurveyPaper");
                var note=survey.GetComponent<AtmajaItem>();var col=survey.GetComponent<Collider>();Object.DestroyImmediate(survey.GetComponent<Renderer>());Object.DestroyImmediate(survey.GetComponent<MeshFilter>());
                Vector3 position=survey.transform.position;survey.transform.localScale=Vector3.one;
                var paper=Model("OpenDiary","DamarSurveyPaper",survey.transform,position,Quaternion.LookRotation(d.north));
                Ink(paper.transform,"CATATAN DAMAR\n\nRumah Atmaja.\nGerbang depan\nmacet. Cari jalan\nmelalui dapur.\n\nJangan datang\nsendirian.",-.076f);
                Ink(paper.transform,"Survei lokasi\n\nKeluarga Atmaja\nmeninggalkan rumah\nini sejak tragedi\ntahun 1998.\n\nPeriksa catatan\ndi dalam rumah.",.076f);
                if(col is BoxCollider box)KosVisualV2.FitBox(box,KosVisualV2.BoundsOf(paper));
            }
            Save();Debug.Log("PROLOG47_FURNITURE_APPLIED authored cloth, paper, ceramic and hollow tub");
        }

        public static void ApplyKosFacilities()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var root=Find("Kos_Building_Corridors").transform;
            foreach(var t in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name.StartsWith("TenantMotor_")||t.name=="KosStoneWorktop"||t.name=="KosCounterCabinet").ToArray())if(t)Object.DestroyImmediate(t.gameObject);
            foreach(var door in root.GetComponentsInChildren<HingedDoor>(true).Where(h=>h.name.StartsWith("KosUpperDoor_")||h.name.StartsWith("KosGroundDoor_")))
            {
                var leaf=door.gameObject;var center=leaf.transform.position;
                if(leaf.GetComponent<Renderer>())Object.DestroyImmediate(leaf.GetComponent<Renderer>());
                if(leaf.GetComponent<MeshFilter>())Object.DestroyImmediate(leaf.GetComponent<MeshFilter>());
                leaf.transform.localScale=Vector3.one;
                var col=leaf.GetComponent<BoxCollider>();col.size=new Vector3(.06f,2.10f,.90f);
                foreach(var t in leaf.GetComponentsInChildren<Transform>().Where(t=>t.name=="ModernDoorVisual").ToArray())Object.DestroyImmediate(t.gameObject);
                var visual=Model("KosPanelDoor","ModernDoorVisual",leaf.transform,center-Vector3.up*1.05f,Quaternion.Euler(0,90,0));
                foreach(var t in root.GetComponentsInChildren<Transform>().Where(t=>t.name==leaf.name+"Handle").ToArray())Object.DestroyImmediate(t.gameObject);
            }
            var slippers=root.GetComponentsInChildren<Transform>(true).Where(t=>t.name=="TenantSlipper"||t.name=="TenantFlipFlops").Select(t=>t.name=="TenantSlipper"?t.position:KosVisualV2.BoundsOf(t.gameObject).center-Vector3.forward*.055f).ToArray();
            foreach(var t in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name=="TenantSlipper"||t.name=="SlipperStrap"||t.name=="TenantFlipFlops").ToArray())Object.DestroyImmediate(t.gameObject);
            var spots=new System.Collections.Generic.List<Vector3>();
            foreach(var p in slippers)if(!spots.Any(x=>Vector3.Distance(x,p)<.20f))spots.Add(p);
            foreach(var p in spots)
            {
                var asset=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Original/Models/KosSandals.fbx");
                var pair=(GameObject)PrefabUtility.InstantiatePrefab(asset);pair.name="TenantFlipFlops";pair.transform.SetParent(root,true);
                foreach(var r in pair.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(Material(r.name.Contains("Strap")?"Repair_SandalStrap":"Repair_SandalRubber"),r.sharedMaterials.Length).ToArray();
                KosVisualV2.Resize(pair,new Vector3(.24f,.055f,.28f));KosVisualV2.SetCenter(pair,p+Vector3.forward*.055f);
            }
            Delete("SharedDiningDoorHinge");Delete("DAPUR BERSAMAHinge");
            foreach(var t in root.GetComponentsInChildren<Transform>().Where(t=>(t.name=="RoomPartition"||t.name=="DAPUR BERSAMASide")&&Mathf.Abs(root.InverseTransformPoint(t.position).z-6)<.03f&&root.InverseTransformPoint(t.position).x< -1.5f&&root.InverseTransformPoint(t.position).y<3).ToArray())Object.DestroyImmediate(t.gameObject);
            foreach(string name in new[]{"SquatToiletBase","ToiletBowl","SecondSquatToilet","BathroomWaterTank","SecondWaterTank","ShowerRiser","ShowerHead","SharedKitchenCounter","Stove","KitchenSink"})
                foreach(var t in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name==name).ToArray())Object.DestroyImmediate(t.gameObject);
            foreach(float z in new[]{3.30f,5.20f})
            {
                Imported("Toilet_Round_A","KosPorcelainToilet_"+z,root,new Vector3(5.55f,0,z),new Vector3(.45f,.78f,.73f),Quaternion.Euler(0,90,0));
                Imported("Shower","KosWallShower_"+z,root,new Vector3(6.10f,.95f,z-.35f),new Vector3(.24f,1.35f,.25f),Quaternion.Euler(0,90,0));
            }
            Imported("Sink_A","KosWashBasin",root,new Vector3(2.05f,0,5.75f),new Vector3(.53f,.85f,.35f),Quaternion.Euler(0,180,0));
            Imported("electric_stove","KosElectricStove",root,new Vector3(-4.25f,0,2.98f),new Vector3(.65f,.87f,.65f),Quaternion.identity);
            Imported("Fridge","KosRefrigerator",root,new Vector3(-4.36f,0,5.18f),new Vector3(.67f,1.65f,.66f),Quaternion.Euler(0,90,0));
            var counter=GameObject.CreatePrimitive(PrimitiveType.Cube);counter.name="KosStoneWorktop";counter.transform.SetParent(root,false);counter.transform.localPosition=new Vector3(-4.42f,.82f,4.07f);counter.transform.localScale=new Vector3(.68f,.08f,1.3f);counter.GetComponent<Renderer>().sharedMaterial=Old("House_MAT_Concrete");
            // Authored support is architectural cabinetry; visible appliances are source meshes.
            foreach(float z in new[]{3.55f,4.6f})
            {var support=GameObject.CreatePrimitive(PrimitiveType.Cube);support.name="KosCounterCabinet";support.transform.SetParent(root,false);support.transform.localPosition=new Vector3(-4.42f,.39f,z);support.transform.localScale=new Vector3(.61f,.78f,.25f);support.GetComponent<Renderer>().sharedMaterial=Material("Repair_DoorPaint");}
            Imported("vintage_electric_kettle","KosKitchenKettle",root,new Vector3(-4.42f,.87f,3.64f),new Vector3(.25f,.28f,.24f),Quaternion.identity);
            Imported("pot_enamel_01","KosCookingPot",root,new Vector3(-4.24f,.88f,3.01f),new Vector3(.32f,.22f,.26f),Quaternion.identity);
            Imported("wooden_cutting_board","KosCuttingBoard",root,new Vector3(-4.35f,.87f,4.18f),new Vector3(.28f,.025f,.36f),Quaternion.identity);
            Imported("wooden_spoon","KosCookingSpoon",root,new Vector3(-4.33f,.90f,4.21f),new Vector3(.04f,.025f,.25f),Quaternion.identity);
            Imported("metal_jug","KosWaterJug",root,new Vector3(-4.37f,.87f,4.53f),new Vector3(.20f,.27f,.20f),Quaternion.identity);
            var motor=Find("RakaMotor");
            for(int i=0;i<4;i++)
            {
                var copy=Object.Instantiate(motor,root);copy.name="TenantMotor_"+(i+1);
                foreach(var c in copy.GetComponentsInChildren<MonoBehaviour>())Object.DestroyImmediate(c);
                foreach(var light in copy.GetComponentsInChildren<Light>())Object.DestroyImmediate(light.gameObject);
                copy.transform.localPosition=new Vector3(-4.6f+i*1.15f,motor.transform.localPosition.y,-5.0f+(i%2)*.08f);
                copy.transform.localRotation=Quaternion.Euler(0,166+i*6,0);
                foreach(var renderer in copy.GetComponentsInChildren<Renderer>())
                {var mats=renderer.sharedMaterials;for(int m=0;m<mats.Length;m++)if(mats[m]&&mats[m].name.Contains("Body")){var tint=new Material(mats[m]);tint.SetColor("_BaseColor",Color.HSVToRGB(.05f+i*.19f,.32f,.22f));string path=Mats+"TenantMotor"+i+"_"+m+".mat";var old=AssetDatabase.LoadAssetAtPath<Material>(path);if(old){EditorUtility.CopySerialized(tint,old);Object.DestroyImmediate(tint);tint=old;}else AssetDatabase.CreateAsset(tint,path);mats[m]=tint;}renderer.sharedMaterials=mats;}
            }
            var alley=Find("InterludeAlley");if(alley)alley.SetActive(true);
            if(!Find("SharedYardWaterGallon"))
            {
                var gallon=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Original/Models/RefillWaterGallon.fbx"));gallon.name="SharedYardWaterGallon";gallon.transform.SetParent(root,false);gallon.transform.localPosition=new Vector3(-2.2f,0,2.6f);
                foreach(var r in gallon.GetComponentsInChildren<Renderer>())r.sharedMaterials=r.sharedMaterials.Select(m=>m.name.Contains("Water")?Material("Repair_DoorPaint"):Material("Repair_Iron")).ToArray();
            }
            DaySky("PrologDaySky",new Color(.5f,.65f,.8f),1.1f);DaySky("PrologDuskSky",new Color(.65f,.38f,.27f),.7f);
            if(!Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None).Any(l=>l.type==LightType.Directional))
            {var sun=new GameObject("MelatiSun",typeof(Light));sun.transform.rotation=Quaternion.Euler(45,-30,0);var light=sun.GetComponent<Light>();light.type=LightType.Directional;light.intensity=.035f;light.color=new Color(.6f,.7f,1);light.shadows=LightShadows.Soft;}
            Save();Debug.Log("PROLOG47_KOS_FACILITIES_APPLIED selected CC0 fixtures, open kitchen/dining and persistent scenery");
        }
        public static void ApplyFinishProps()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            foreach(var pair in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t=>t.name=="TenantFlipFlops").ToArray())
            {
                var bounds=KosVisualV2.BoundsOf(pair.gameObject);var parent=pair.parent;var rotation=pair.rotation;
                var replacement=Model("IndonesianFlipFlops","TenantFlipFlops",parent,bounds.center,rotation);
                KosVisualV2.Resize(replacement,new Vector3(.24f,.055f,.28f));KosVisualV2.SetCenter(replacement,bounds.center);Ground(replacement,bounds.center.y>1?3.302f:.002f);
                Object.DestroyImmediate(pair.gameObject);
            }
            Save();EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();var cup=Find("ChildMilkCup");
            var center=KosVisualV2.BoundsOf(cup).center;cup.transform.rotation=Quaternion.LookRotation(-d.north);KosVisualV2.SetCenter(cup,center);d.upper.cupAim.position=center;
            Find("BathWallScratches").transform.rotation=Quaternion.LookRotation(-d.east);
            foreach(var stain in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t=>t.name.StartsWith("BloodFloor_")&&d.LocalPosition(t.position).x>0))stain.position=d.P(2.9f,5.55f,3.834f);
            foreach(string name in new[]{"BloodHandprint","WallScratches","BloodSpatter"})
            {
                var mat=AssetDatabase.LoadAssetAtPath<Material>(Mats+name+".mat");mat.SetFloat("_BlendModePreserveSpecular",0);mat.SetFloat("_SpecularHighlights",0);mat.SetFloat("_EnvironmentReflections",0);mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");mat.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");mat.EnableKeyword("_ENVIRONMENTREFLECTIONS_OFF");EditorUtility.SetDirty(mat);
            }
            Save();
        }
        public static void ApplyAcceptanceReview(){ApplyReviewCorrections();ApplyArchitecture();ApplyPrologSkin();}
        public static void ApplyPrologSkin()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();var pose=d.basement.nyonya.GetComponent<AtmajaThreatPresentation>();
            var asset=AssetDatabase.LoadAssetAtPath<GameObject>(Models+"NyonyaPrologSurfaceRepair.fbx");
            var sample=(GameObject)PrefabUtility.InstantiatePrefab(asset);
            try
            {
                var source=sample.GetComponentsInChildren<SkinnedMeshRenderer>(true).Single();
                var target=pose.authoredMotion.GetComponentsInChildren<SkinnedMeshRenderer>(true).Single(r=>r.name=="NyonyaDesignerBody");
                var bones=pose.authoredMotion.GetComponentsInChildren<Transform>(true).GroupBy(t=>t.name).ToDictionary(g=>g.Key,g=>g.First());
                target.sharedMesh=source.sharedMesh;target.bones=source.bones.Select(b=>bones[b.name]).ToArray();
                target.rootBone=bones[source.rootBone.name];target.localBounds=source.localBounds;target.updateWhenOffscreen=true;target.quality=SkinQuality.Bone4;
                PrefabUtility.RecordPrefabInstancePropertyModifications(target);
            }
            finally{Object.DestroyImmediate(sample);}
            Save();Debug.Log("PROLOG47_SKIN_RECOVERED original supplied surface; existing scene skeleton and clips retained; shared phases untouched");
        }
        public static void ApplyFinalReview(){ApplyReviewCorrections();ApplyArchitecture();}
        public static void ApplyGeometryReview(){ApplyArchitecture();ApplyLadder();ApplyLadder();}
        public static void ApplyReviewCorrections()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var root=Find("Kos_Building_Corridors").transform;
            var bucket=Find("SharedWaterBucket");if(bucket)bucket.transform.localPosition=new Vector3(-3.65f,0,5.45f);
            Imported("Sink_A","KosKitchenWashBasin",root,new Vector3(-3.05f,0,2.85f),new Vector3(.53f,.85f,.45f),Quaternion.Euler(0,180,0));
            Imported("Sink_A","KosWashBasin",root,new Vector3(2.05f,0,5.75f),new Vector3(.53f,.85f,.35f),Quaternion.Euler(0,180,0));
            Save();
        }
        public static void ApplyArchitecture()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();var environment=Find("Atmaja_Environment_Art").transform;
            GameObject Wall(string name,float x,float depth,float y,Vector3 size)
            {Delete(name);var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;go.transform.SetParent(environment,true);go.transform.SetPositionAndRotation(d.P(x,depth,y),Quaternion.LookRotation(d.north));go.transform.localScale=size;go.GetComponent<Renderer>().sharedMaterial=Old("House_MAT_PlasterDamaged");return go;}
            var lintel=Find("FrontSittingDoorLintel");if(lintel){lintel.transform.position=d.P(-1.4f,2.5f,(2.71f+3.68f)/2);var size=lintel.transform.localScale;size.y=3.68f-2.71f;lintel.transform.localScale=size;}
            var east=Find("MasterEastWall");if(east){east.transform.position=d.P(2.4f,(3.25f+5.2f)/2,(3.82f+6.83f)/2);east.transform.localScale=new Vector3(.16f,3.01f,1.95f);}
            Wall("MasterExtensionEastWall",5.75f,1.64f,5.325f,new Vector3(.16f,3.01f,3.28f));
            Wall("MasterExtensionNorthWall",4.075f,.08f,5.325f,new Vector3(3.35f,3.01f,.16f));
            Wall("MasterExtensionBalconyWall",4.075f,3.25f,5.325f,new Vector3(3.35f,3.01f,.16f));
            foreach(string name in new[]{"MasterBed","MasterMattress","MasterPillow","MasterBedSet"})Delete(name);
            var bed=Model("DoubleBedSet","MasterBedSet",environment,d.P(4.45f,1.26f,3.83f),Quaternion.LookRotation(-d.north));Ground(bed,3.83f);FitCollider(bed);
            void Move(string name,Vector3 delta){var go=Find(name);if(go)go.transform.position+=delta;}
            var night=Find("MasterNightstand");if(night){Vector3 delta=d.P(3.23f,1.0f,4.3f)-KosVisualV2.BoundsOf(night).center;Move("MasterNightstand",delta);Move("MasterAlarmClock",delta);}
            var rocker=Find("MasterRocker");if(rocker){var center=d.P(1.85f,3.3f,4.32f);rocker.transform.rotation=Quaternion.LookRotation(-d.north);KosVisualV2.SetCenter(rocker,center);FitCollider(rocker);}
            Delete("MasterPortrait"); // Keep only the one family portrait in the corridor.
            var wood=Find("UpperFloor_South");if(wood)Prolog47MeshRepair.Subtract(wood.GetComponent<MeshFilter>(),d,new Bounds(new Vector3(-1.1f,3.82f,2.60f),new Vector3(2.60f,1,5.10f)),"UpperFloorOutsideBathroom");
            foreach(var go in environment.GetComponentsInChildren<MeshFilter>().Where(f=>f.name.StartsWith("UpperSlab_")||f.name.StartsWith("GroundFloorCeiling_")||f.name=="BalconyDeck"||f.name=="BalconyWalk"))
                Prolog47MeshRepair.RemoveUpwardFinish(go,d,3.82f,"FinishSupport_"+go.name);
            foreach(var f in environment.GetComponentsInChildren<MeshFilter>().Where(f=>f.name=="Static__House_MAT_TrimTeak"))
            {
                foreach(int side in new[]{-1,1})Prolog47MeshRepair.Subtract(f,d,new Bounds(new Vector3(side*1.12f,4.27f,.60f),new Vector3(.20f,.20f,1.10f)),"HouseTrimOutsideRooms");
            }
            foreach(string name in new[]{"CellarSpineNorth","CellarSpineSouth","CellarReturnPartition"})Delete(name);
            Save();Debug.Log("PROLOG47_ARCHITECTURE_APPLIED bathroom floor isolated, master extension, open cellar");
        }
        public static void ApplyDetails()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();var root=Find("Atmaja_Environment_Art").transform;
            foreach(var t in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name=="BloodHandprint"||t.name=="BloodFinger"||t.name.StartsWith("BloodyHand_")||t.name.StartsWith("BloodFloor_")).ToArray())if(t)Object.DestroyImmediate(t.gameObject);
            var hand=DecalMaterial("BloodHandprint",false);var scratches=DecalMaterial("WallScratches",true);
            for(int i=0;i<7;i++)
            {
                float x=-4.7f+i*.95f;var q=Quad("BloodyHand_"+i,root,d.P(x,5.305f,4.7f+(i%3)*.18f),Quaternion.LookRotation(-d.north)*Quaternion.Euler(0,0,-16+i*7),new Vector2(.24f,.36f),hand);
            }
            var stain=DecalMaterial("BloodSpatter",false,true);
            foreach(var p in new[]{d.P(-3.8f,5.75f,3.833f),d.P(-1.8f,5.8f,3.833f),d.P(2.9f,5.55f,3.834f)})Quad("BloodFloor_"+p.x,root,p,Quaternion.Euler(90,0,0),new Vector2(.72f,.86f),stain);
            Delete("BathScratchesAR");Delete("BathScratchTarget");Delete("BathWallScratches");
            var scratch=Quad("BathWallScratches",root,d.P(-2.285f,3.25f,4.94f),Quaternion.LookRotation(-d.east),new Vector2(.42f,.68f),scratches);
            var trace=new GameObject("BathWallARTrace");trace.transform.SetParent(root,true);trace.transform.SetPositionAndRotation(scratch.transform.position,scratch.transform.rotation);d.upper.bathScratchesAR=trace;
            d.upper.scratchAim.SetParent(trace.transform,true);d.upper.scratchAim.position=scratch.transform.position;
            var target=new GameObject("BathScratchTarget",typeof(BoxCollider),typeof(AtmajaItem));target.transform.SetParent(root,true);target.transform.SetPositionAndRotation(scratch.transform.position,scratch.transform.rotation);target.GetComponent<BoxCollider>().size=new Vector3(.42f,.68f,.015f);
            var item=target.GetComponent<AtmajaItem>();item.director=d;item.action=AtmajaAction.BathScratches;item.label="Periksa bekas cakaran di dinding";
            Delete("VentCabinet");Delete("VentHoleDark");Delete("VentHoleAR");Delete("VentGrilleHinge");d.upper.ventCabinet=null;
            var hinge=new GameObject("VentGrilleHinge").transform;hinge.SetParent(root,true);hinge.SetPositionAndRotation(d.P(-3.45f,5.30f,3.82f),Quaternion.LookRotation(d.north));
            var cover=Model("VentGrille","VentGrille",hinge,d.P(-3.0f,5.30f,4.36f),Quaternion.LookRotation(d.north));
            KosVisualV2.Resize(cover,new Vector3(.90f,1.04f,.09f));KosVisualV2.SetCenter(cover,d.P(-3.0f,5.30f,4.36f));FitCollider(cover);
            var vent=cover.AddComponent<HingedDoor>();vent.hinge=hinge;vent.angle=105;vent.requiresCrouchAndAR=true;vent.label="Buka ventilasi";d.upper.ventDoor=vent;
            ARPhotoGlimmer.Attach(d.upper.ventMouth,d.upper,"EVENT_P0_CHAIN1_BROKEN",true,true,false);
            d.upper.ventHoleAR=d.upper.ventMouth.GetComponentInChildren<ARPhotoGlimmer>(true).gameObject;
            var front=Find("FrontDoorRustChain");var centre=front?KosVisualV2.BoundsOf(front).center:d.P(0,-.23f,1.65f);Delete("FrontDoorRustChain");
            Chain("FrontDoorRustChain",root,centre,d.east,Vector3.down*.065f,1.30f);
            var well=Find("WellCover");if(well)
            {
                foreach(var t in well.GetComponentsInChildren<Transform>(true).Where(t=>t!=well.transform).ToArray())if(t)Object.DestroyImmediate(t.gameObject);
                var mesh=new Mesh{name="Circular jointed well cover"};var vertices=new System.Collections.Generic.List<Vector3>();var triangles=new System.Collections.Generic.List<int>();
                const float radius=.50f;
                for(int board=0;board<7;board++)
                {
                    float z0=-radius+board*(radius*2/7)+.003f,z1=-radius+(board+1)*(radius*2/7)-.003f;
                    var polygon=new System.Collections.Generic.List<Vector3>();
                    for(int k=0;k<=6;k++){float z=Mathf.Lerp(z0,z1,k/6f);polygon.Add(new Vector3(-Mathf.Sqrt(Mathf.Max(0,radius*radius-z*z)),0,z));}
                    for(int k=6;k>=0;k--){float z=Mathf.Lerp(z0,z1,k/6f);polygon.Add(new Vector3(Mathf.Sqrt(Mathf.Max(0,radius*radius-z*z)),0,z));}
                    int first=vertices.Count,n=polygon.Count;vertices.AddRange(polygon.Select(p=>p+Vector3.up*.018f));vertices.AddRange(polygon.Select(p=>p-Vector3.up*.018f));
                    for(int k=1;k<n-1;k++){triangles.AddRange(new[]{first,first+k,first+k+1,first+n,first+n+k+1,first+n+k});}
                    for(int k=0;k<n;k++){int j=(k+1)%n;triangles.AddRange(new[]{first+k,first+n+k,first+n+j,first+k,first+n+j,first+j});}
                }
                mesh.SetVertices(vertices);mesh.SetTriangles(triangles,0);mesh.SetUVs(0,vertices.Select(v=>new Vector2(v.x,v.z)).ToList());mesh.RecalculateNormals();mesh.RecalculateBounds();
                MeshObject("WellCoverBoards47",well.transform,d.P(2.85f,11.2f,1.10f),StoreMesh(mesh,"CircularWellCover"),Old("House_MAT_WoodDoor"));
                Chain("WellRustChain",well.transform,d.P(2.85f,11.2f,1.137f),d.east,Vector3.zero,.94f);
            }
            var water=d.upper.bathWater;var waterMesh=new Mesh{name="Blood water follows inner bowl"};var wv=new System.Collections.Generic.List<Vector3>{Vector3.zero};var wt=new System.Collections.Generic.List<int>();
            for(int i=0;i<64;i++){float a=i*Mathf.PI*2/64;wv.Add(new Vector3(Mathf.Sign(Mathf.Cos(a))*Mathf.Pow(Mathf.Abs(Mathf.Cos(a)),.55f)*.74f,0,Mathf.Sign(Mathf.Sin(a))*Mathf.Pow(Mathf.Abs(Mathf.Sin(a)),.55f)*.285f));}
            for(int i=0;i<64;i++)wt.AddRange(new[]{0,(i+1)%64+1,i+1});waterMesh.SetVertices(wv);waterMesh.SetTriangles(wt,0);waterMesh.RecalculateNormals();waterMesh.RecalculateBounds();water.GetComponent<MeshFilter>().sharedMesh=StoreMesh(waterMesh,"BathBloodSurface");water.transform.localScale=Vector3.one;
            foreach(var t in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name.StartsWith("AddedDinner_")||t.name.StartsWith("AddedDish_")||t.name=="MouldyPlate"||t.name.StartsWith("RottenDinner_")||t.name.StartsWith("DeadRatOnPlate_")).ToArray())if(t)Object.DestroyImmediate(t.gameObject);
            for(int i=0;i<12;i++)
            {
                float x=i%2==0?3.28f:4.15f,depth=3.0f+(i/2)*.61f;var p=d.P(x,depth,1.49f);
                // Two scanned CC0 foods, varied portions on genuine concave dishes.
                var food=DesignerReviewBuilder.ReviewModel("AddedDinner_"+i,i%3==0?"RottenApple":"MoldyBread",root,p+Vector3.up*.04f,new Vector3(.16f,.08f,.14f),false);
                food.transform.Rotate(Vector3.up,i*37);KosVisualV2.SetCenter(food,p+Vector3.up*.04f);
                MeshObject("AddedDish_"+i,root,p,Plate(),Material("Repair_Ceramic"));
            }
            Save();Debug.Log("PROLOG47_DETAILS_APPLIED physical handprints, wall scratches, vent, round cover, real-sized chains, fuller table");
        }
        public static void ApplyLadder()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();var root=Find("Atmaja_Environment_Art").transform;
            Delete("CellarExitLadder");Delete("CellarLadderRoute");
            const string source="Assets/_LayarTerakhir/assets/Art/Phase3/Imported/Modules/modular_fire_escape/modular_fire_escape_ladder_bottom.fbx";
            var ladder=new GameObject("CellarExitLadder");ladder.transform.SetParent(root,true);
            var body=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(source));body.transform.SetParent(ladder.transform,false);
            ladder.transform.rotation=Quaternion.LookRotation(d.north);
            foreach(var r in ladder.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(Material("Repair_Iron"),r.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(ladder,new Vector3(.66f,6.66f,.075f));KosVisualV2.SetCenter(ladder,d.P(1.025f,.69f,.55f));
            // Top-to-bottom climb route follows the visible ladder instead of a fade teleport.
            var route=new GameObject("CellarLadderRoute").transform;route.SetParent(d.transform,false);
            var points=new System.Collections.Generic.List<Transform>();
            var positions=new[]{d.upper.trapdoorTop.position,d.P(1.025f,1.18f,3.50f),d.P(1.025f,1.18f,2.5f),d.P(1.025f,1.18f,1.5f),d.P(1.025f,1.18f,.5f),d.P(1.025f,1.18f,-.5f),d.P(1.025f,1.18f,-1.5f),d.P(1.025f,1.18f,-2.78f),d.basement.spawnBasement.position};
            for(int i=0;i<positions.Length;i++){var p=new GameObject("LadderClimb_"+i).transform;p.SetParent(route,true);p.position=positions[i];points.Add(p);}d.upper.cellarLadderRoute=points.ToArray();
            var cut=new Bounds(new Vector3(1.025f,1.61f,1.4f),new Vector3(.95f,4.45f,1.70f));
            foreach(var f in root.GetComponentsInChildren<MeshFilter>().Where(f=>f.name=="DiningDirtyTile"||f.name=="CellarCeiling"||f.name=="GroundFloorCeiling_1"||f.name=="Static__House_MAT_BrickBase"||f.name=="Static__House_MAT_Concrete").ToArray())
                Prolog47MeshRepair.Subtract(f,d,cut,"ShaftOpening_"+f.name);
            var priorCollision=Find("LadderShaftFloorCollision");
            if(priorCollision)foreach(var piece in priorCollision.GetComponentsInChildren<BoxCollider>())
                foreach(var original in root.GetComponentsInChildren<BoxCollider>().Where(b=>b.name+"_OutsideShaft"==piece.name&&!b.transform.IsChildOf(priorCollision.transform)))original.enabled=true;
            Delete("LadderShaftFloorCollision");var collision=new GameObject("LadderShaftFloorCollision").transform;collision.SetParent(root,true);
            foreach(var box in root.GetComponentsInChildren<BoxCollider>().ToArray())
            {
                var b=box.bounds;if(!box.enabled||b.size.x<2||b.size.z<2||b.size.y>1.4f||b.min.y<-.61f||b.max.y>3.91f)continue;
                var center=d.LocalPosition(b.center);float x0=center.x-b.extents.x,x1=center.x+b.extents.x,z0=center.y-b.extents.z,z1=center.y+b.extents.z;
                const float cx0=.55f,cx1=1.5f,cz0=.55f,cz1=2.25f;
                if(x1<=cx0||x0>=cx1||z1<=cz0||z0>=cz1)continue;box.enabled=false;
                void Part(float a,float c,float e,float f)
                {
                    if(c-a<.001f||f-e<.001f)return;
                    var go=new GameObject(box.name+"_OutsideShaft",typeof(BoxCollider));go.transform.SetParent(collision,true);go.transform.SetPositionAndRotation(d.P((a+c)/2,(e+f)/2,b.center.y),Quaternion.LookRotation(d.north));go.GetComponent<BoxCollider>().size=new Vector3(c-a,b.size.y,f-e);
                }
                Part(x0,Mathf.Min(cx0,x1),z0,z1);Part(Mathf.Max(cx1,x0),x1,z0,z1);Part(Mathf.Max(x0,cx0),Mathf.Min(x1,cx1),z0,Mathf.Min(cz0,z1));Part(Mathf.Max(x0,cx0),Mathf.Min(x1,cx1),Mathf.Max(cz1,z0),z1);
            }
            var cabinet=Find("FamilyRadioCabinet");
            if(cabinet&&d.LocalPosition(cabinet.transform.position).x>0)
                foreach(var t in root.GetComponentsInChildren<Transform>().Where(t=>t.name=="FamilyRadioCabinet"||t.name=="FamilyRadioBody"||t.name=="FamilyCabinetPanel"||t.name=="RadioSpeakerGrille"))t.position-=d.east*1.9f;
            Save();Debug.Log("PROLOG47_LADDER_APPLIED visible full-height CC0 ladder and matching climb route");
        }
        private static Material DecalMaterial(string name,bool scratch,bool spatter=false)
        {
            string path=Mats+name+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,path);}
            mat.SetFloat("_BlendModePreserveSpecular",0);mat.SetFloat("_SpecularHighlights",0);mat.SetFloat("_EnvironmentReflections",0);mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");mat.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");mat.EnableKeyword("_ENVIRONMENTREFLECTIONS_OFF");
            mat.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(spatter?"Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities/Blood/blood_splatter_01_base.png":"Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47/HandprintAndScratches.png"));
            mat.SetTextureScale("_BaseMap",spatter?Vector2.one:new Vector2(scratch?.385f:.61f,1));mat.SetTextureOffset("_BaseMap",new Vector2(scratch?.615f:0,0));
            mat.SetColor("_BaseColor",scratch?new Color(.65f,.60f,.52f):new Color(.45f,.27f,.22f));mat.SetFloat("_Surface",1);mat.SetFloat("_SrcBlend",5);mat.SetFloat("_DstBlend",10);mat.SetFloat("_ZWrite",0);mat.SetFloat("_Cull",0);mat.SetFloat("_Smoothness",.12f);mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");mat.renderQueue=3000;mat.SetOverrideTag("RenderType","Transparent");EditorUtility.SetDirty(mat);return mat;
        }
        private static GameObject Quad(string name,Transform root,Vector3 pos,Quaternion rotation,Vector2 size,Material mat)
        {
            Delete(name);var q=GameObject.CreatePrimitive(PrimitiveType.Quad);q.name=name;q.transform.SetParent(root,true);q.transform.SetPositionAndRotation(pos,rotation);q.transform.localScale=new Vector3(size.x,size.y,1);Object.DestroyImmediate(q.GetComponent<Collider>());q.GetComponent<Renderer>().sharedMaterial=mat;q.GetComponent<Renderer>().shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;return q;
        }
        private static Mesh StoreMesh(Mesh mesh,string name)
        {
            string path="Assets/_LayarTerakhir/assets/Art/Generated/Models/Prolog47/"+name+".asset";var prior=AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if(prior){EditorUtility.CopySerialized(mesh,prior);Object.DestroyImmediate(mesh);return prior;}AssetDatabase.CreateAsset(mesh,path);return mesh;
        }
        private static GameObject MeshObject(string name,Transform parent,Vector3 position,Mesh mesh,Material mat)
        {
            Delete(name);var go=new GameObject(name,typeof(MeshFilter),typeof(MeshRenderer));go.transform.SetParent(parent,true);go.transform.position=position;go.GetComponent<MeshFilter>().sharedMesh=mesh;go.GetComponent<Renderer>().sharedMaterial=mat;return go;
        }
        private static Mesh Plate()
        {
            string path="Assets/_LayarTerakhir/assets/Art/Generated/Models/Prolog47/ConcaveDish.asset";var existing=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(existing)return existing;
            var v=new System.Collections.Generic.List<Vector3>();var t=new System.Collections.Generic.List<int>();var profile=new[]{new Vector2(0,0),new Vector2(.075f,0),new Vector2(.115f,.008f),new Vector2(.14f,.025f),new Vector2(.143f,.021f),new Vector2(.11f,.002f),new Vector2(.07f,-.008f),new Vector2(0,-.008f)};
            foreach(var ring in profile)for(int i=0;i<48;i++){float a=i*Mathf.PI*2/48;v.Add(new Vector3(Mathf.Cos(a)*ring.x,ring.y,Mathf.Sin(a)*ring.x));}
            for(int j=0;j<profile.Length-1;j++)for(int i=0;i<48;i++){int a=j*48+i,b=j*48+(i+1)%48;t.AddRange(new[]{a,a+48,b+48,a,b+48,b});}
            var mesh=new Mesh{name="Turned enamel dish"};mesh.SetVertices(v);mesh.SetTriangles(t,0);mesh.RecalculateNormals();mesh.RecalculateBounds();return StoreMesh(mesh,"ConcaveDish");
        }
        private static void Chain(string name,Transform parent,Vector3 position,Vector3 along,Vector3 sag,float length)
        {
            var v=new System.Collections.Generic.List<Vector3>();var t=new System.Collections.Generic.List<int>();int links=Mathf.RoundToInt(length/.04f);
            Vector3 up=Vector3.up;
            for(int link=0;link<links;link++)
            {
                float ratio=link/(float)Mathf.Max(1,links-1);var c=along*((ratio-.5f)*length)+sag*(1-Mathf.Pow(ratio*2-1,2));var side=Quaternion.AngleAxis(link%2*90,along)*up;
                int start=v.Count;
                for(int i=0;i<24;i++)
                {
                    float a=i*Mathf.PI*2/24;Vector3 ring=along*(Mathf.Cos(a)*.025f)+side*(Mathf.Sin(a)*.012f);Vector3 radial=(along*Mathf.Cos(a)+side*Mathf.Sin(a)).normalized;Vector3 normal=Vector3.Cross(along,side).normalized;
                    for(int k=0;k<6;k++){float b=k*Mathf.PI*2/6;v.Add(c+ring+(radial*Mathf.Cos(b)+normal*Mathf.Sin(b))*.0032f);}
                }
                for(int i=0;i<24;i++)for(int k=0;k<6;k++){int a=start+i*6+k,b=start+((i+1)%24)*6+k,c0=start+((i+1)%24)*6+(k+1)%6,e=start+i*6+(k+1)%6;t.AddRange(new[]{a,b,c0,a,c0,e});}
            }
            var mesh=new Mesh{name=name+" links in metres"};mesh.SetVertices(v);mesh.SetTriangles(t,0);mesh.RecalculateNormals();mesh.RecalculateBounds();MeshObject(name,parent,position,StoreMesh(mesh,name+"Links"),Old("House_MAT_RustMetal"));
        }
        private static GameObject Imported(string id,string name,Transform root,Vector3 foot,Vector3 size,Quaternion rotation)
        {
            foreach(var prior in root.GetComponentsInChildren<Transform>(true).Where(t=>t.name==name).ToArray())Object.DestroyImmediate(prior.gameObject);
            string folder="Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities/"+id+"/";
            var asset=AssetDatabase.LoadAssetAtPath<GameObject>(folder+id+".fbx");if(!asset)throw new InvalidOperationException("Missing CC0 source "+id);
            var go=new GameObject(name);go.transform.SetParent(root,false);go.transform.localRotation=rotation;
            var child=(GameObject)PrefabUtility.InstantiatePrefab(asset);child.transform.SetParent(go.transform,false);
            foreach(var renderer in child.GetComponentsInChildren<Renderer>())
            {
                var materials=renderer.sharedMaterials;
                for(int i=0;i<materials.Length;i++)
                {
                    string key=id=="Toilet_Round_A"?(materials[i]&&materials[i].name.Contains("Seat")?"Toilet_Round_Seat":"Toilet_Round"):id;
                    string diffuse=folder+key+(id=="Sink_A"||id=="Toilet_Round_A"?"_Diffuse.png":"_diff_1k.jpg");
                    var texture=AssetDatabase.LoadAssetAtPath<Texture2D>(diffuse);
                    string path=Mats+"CC0_"+key+".mat";var mat=AssetDatabase.LoadAssetAtPath<Material>(path);
                    if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,path);}
                    mat.SetColor("_BaseColor",texture?Color.white:new Color(.64f,.65f,.62f));mat.SetFloat("_Smoothness",.35f);mat.SetTexture("_BaseMap",texture);
                    if(id=="Shower"){mat.SetFloat("_Metallic",.8f);mat.SetFloat("_Smoothness",.65f);}
                    var normal=AssetDatabase.LoadAssetAtPath<Texture2D>(folder+key+(id=="Sink_A"||id=="Toilet_Round_A"?"_Normal.png":"_nor_gl_1k.jpg"));
                    var mask=AssetDatabase.LoadAssetAtPath<Texture2D>(folder+key+(id=="Sink_A"||id=="Toilet_Round_A"?"_Mask.png":"_mask.png"));
                    if(normal){mat.SetTexture("_BumpMap",normal);mat.EnableKeyword("_NORMALMAP");mat.SetFloat("_BumpScale",.5f);}
                    if(mask){mat.SetTexture("_MetallicGlossMap",mask);mat.SetTexture("_OcclusionMap",mask);mat.EnableKeyword("_METALLICSPECGLOSSMAP");mat.EnableKeyword("_OCCLUSIONMAP");mat.SetFloat("_Smoothness",1);}
                    materials[i]=mat;EditorUtility.SetDirty(mat);
                }
                renderer.sharedMaterials=materials;
            }
            KosVisualV2.Resize(go,size);var point=root.TransformPoint(foot);KosVisualV2.SetCenter(go,point+Vector3.up*size.y*.5f);FitCollider(go);return go;
        }
        private static void DaySky(string name,Color tint,float exposure)
        {
            const string folder="Assets/_LayarTerakhir/assets/Resources/";Directory.CreateDirectory(folder);
            string path=folder+name+".mat";var sky=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!sky){sky=new Material(Shader.Find("Skybox/Procedural"));AssetDatabase.CreateAsset(sky,path);}
            sky.SetColor("_SkyTint",tint);sky.SetFloat("_Exposure",exposure);sky.SetFloat("_AtmosphereThickness",1.2f);EditorUtility.SetDirty(sky);
        }
        private static void Ink(Transform parent,string words,float x)
        {
            var go=new GameObject("PrintedPage",typeof(RectTransform),typeof(Canvas));go.transform.SetParent(parent,false);go.transform.localPosition=new Vector3(x,.035f,0);go.transform.localRotation=Quaternion.Euler(90,0,0);go.transform.localScale=Vector3.one*.00035f;
            var rect=go.GetComponent<RectTransform>();rect.sizeDelta=new Vector2(360,520);go.GetComponent<Canvas>().renderMode=RenderMode.WorldSpace;
            var text=new GameObject("Ink",typeof(RectTransform),typeof(Text));text.transform.SetParent(go.transform,false);var tr=text.GetComponent<RectTransform>();tr.anchorMin=Vector2.zero;tr.anchorMax=Vector2.one;tr.offsetMin=tr.offsetMax=Vector2.zero;
            var ink=text.GetComponent<Text>();ink.font=AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf");ink.text=words;ink.fontSize=27;ink.color=new Color(.20f,.16f,.11f);ink.raycastTarget=false;ink.alignment=TextAnchor.UpperLeft;
        }
    }
}
#endif
