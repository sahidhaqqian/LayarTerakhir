#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using LayarTerakhir.Prolog;
using LayarTerakhir.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class DesignerReviewBuilder
    {
        private const string Art="Assets/_LayarTerakhir/assets/Art/Review/";
        private static AtmajaDirector d;private static Transform root;
        private static IEnumerable<Transform> Objects()=>SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true));
        private static GameObject Find(string name)=>Objects().FirstOrDefault(t=>t.name==name)?.gameObject;
        private static Material Mat(string name)=>AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/"+name+".mat");
        private static Vector3 P(float x,float depth,float height)=>d.P(x,depth,height);
        private static void Remove(params string[] names){foreach(string name in names)foreach(var obj in Objects().Where(t=>t.name==name).ToArray())if(obj)Object.DestroyImmediate(obj.gameObject);}
        [Serializable] private class SurveyItem{public string name;public Vector3 center,size;}
        [Serializable] private class SurveyReport{public SurveyItem[] colliders;}
        public static void Survey()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();
            Physics.SyncTransforms();
            var items=Object.FindObjectsByType<Collider>(FindObjectsInactive.Include,FindObjectsSortMode.None).Select(c=>new SurveyItem{name=c.name,center=c.bounds.center,size=c.bounds.size}).ToArray();
            File.WriteAllText("Audit/designer-review-scene-survey.json",JsonUtility.ToJson(new SurveyReport{colliders=items},true));
            Debug.Log("REVIEW_SURVEY_COMPLETE "+items.Length);
        }
        private static string StairSignature()
        {
            var stair=Find("AtmajaStairRevision");if(!stair)throw new Exception("Approved stair assembly is missing");
            var records=stair.GetComponentsInChildren<Transform>(true).OrderBy(t=>t.name).Select(t=>t.name+"|"+t.position.ToString("F5")+"|"+t.rotation.ToString("F5")+"|"+t.localScale.ToString("F5")+"|"+string.Join(",",t.GetComponents<Collider>().Select(c=>EditorJsonUtility.ToJson(c)))+"|"+string.Join(",",t.GetComponents<MeshFilter>().Select(f=>AssetDatabase.GetAssetPath(f.sharedMesh))));
            using var sha=SHA256.Create();return BitConverter.ToString(sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(string.Join("\n",records)))).Replace("-","").ToLowerInvariant();
        }
        [MenuItem("LayarTerakhir/Prolog/Apply designer review from goal.md")]
        public static void Apply()
        {
            Directory.CreateDirectory(Art+"Materials");ImportMaterials();BindReviewVoices();PrologPresentationBuilder.RebuildRakaOnly();
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();
            string stairs=StairSignature();RestoreWellFloorPrimitives();Remove("DesignerReviewEnvironment");root=new GameObject("DesignerReviewEnvironment").transform;
            root.SetParent(Find("Atmaja_Environment_Art").transform,false);
            FurnishHome();DressPlates();RefineChildRoom();OpenDiningAisles();ExpandBasement();BuildWell();ApplyFurnitureLods();
            if(!d.GetComponent<AtmajaReviewMoments>())d.gameObject.AddComponent<AtmajaReviewMoments>();
            if(stairs!=StairSignature())throw new Exception("Designer review modified the protected stairs");
            WriteMeshSurvey();Physics.SyncTransforms();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/designer-review-stairs-preserved.json","{\"unchanged\":true,\"signature\":\""+stairs+"\"}\n");
            MelatiNeighborhoodBuilder.Apply();AssetDatabase.SaveAssets();Debug.Log("DESIGNER_REVIEW_ENVIRONMENT_APPLIED stairs preserved");
        }
        [Serializable] private class MeshRow{public string name,path;public int triangles;}
        [Serializable] private class MeshSurvey{public int sceneTriangles;public MeshRow[] meshes;}
        private static void WriteMeshSurvey()
        {
            var rows=new List<MeshRow>();
            foreach(var filter in Objects().Select(t=>t.GetComponent<MeshFilter>()).Where(f=>f&&f.sharedMesh))
            {int tris=0;for(int s=0;s<filter.sharedMesh.subMeshCount;s++)tris+=(int)filter.sharedMesh.GetIndexCount(s)/3;rows.Add(new MeshRow{name=filter.name,path=AssetDatabase.GetAssetPath(filter.sharedMesh),triangles=tris});}
            File.WriteAllText("Audit/designer-review-scene-meshes.json",JsonUtility.ToJson(new MeshSurvey{sceneTriangles=rows.Sum(r=>r.triangles),meshes=rows.OrderByDescending(r=>r.triangles).ToArray()},true));
        }
        [Serializable] private class VoiceBank{public VoiceEntry[] entries;}
        [Serializable] private class VoiceEntry{public string speaker,text,path;}
        public static void BindReviewVoices()
        {
            var profile=LayarTerakhir.Core.PresentationProfile.Load();var bank=JsonUtility.FromJson<VoiceBank>(File.ReadAllText("Audit/prolog-voice-manifest.json"));
            profile.voices=bank.entries.Select(e=>new LayarTerakhir.Core.PresentationProfile.VoiceLine{speaker=e.speaker,text=e.text,clip=AssetDatabase.LoadAssetAtPath<AudioClip>(e.path)}).ToArray();
            if(profile.voices.Any(v=>!v.clip))throw new Exception("A reviewed dialogue clip is missing");
            foreach(var entry in bank.entries)
            {var importer=(AudioImporter)AssetImporter.GetAtPath(entry.path);importer.forceToMono=true;var settings=importer.defaultSampleSettings;settings.loadType=AudioClipLoadType.CompressedInMemory;settings.compressionFormat=AudioCompressionFormat.Vorbis;settings.quality=.7f;settings.sampleRateSetting=AudioSampleRateSetting.OverrideSampleRate;settings.sampleRateOverride=22050;importer.defaultSampleSettings=settings;importer.SaveAndReimport();}
            EditorUtility.SetDirty(profile);
        }
        private static Material Surface(string name,Color color,string texture=null,float smooth=.18f)
        {
            string path=Art+"Materials/"+name+".mat";var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}
            m.color=color;m.SetFloat("_Smoothness",smooth);
            if(texture!=null)m.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(texture));
            EditorUtility.SetDirty(m);return m;
        }
        public static void ImportMaterials()
        {
            Directory.CreateDirectory(Art+"Materials");
            foreach(string guid in AssetDatabase.FindAssets("t:Texture2D",new[]{Art+"Textures"}))
            {var importer=(TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));importer.maxTextureSize=1024;importer.mipmapEnabled=true;importer.textureCompression=TextureImporterCompression.Compressed;importer.textureType=importer.assetPath.Contains("_nor_gl_")?TextureImporterType.NormalMap:TextureImporterType.Default;importer.SaveAndReimport();}
            foreach(string source in new[]{"painted_wooden_sofa","street_rat","food_apple_01","hamburger_buns"})
            {
                var m=Surface("Review_"+source,source.Contains("food")||source.Contains("buns")?new Color(.38f,.40f,.19f):Color.white,Art+"Textures/"+source+"_diff_1k.jpg");
                m.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Textures/"+source+"_nor_gl_1k.jpg"));m.EnableKeyword("_NORMALMAP");m.SetFloat("_BumpScale",.55f);
                var arm=AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Textures/"+source+"_arm_1k.jpg");
                if(arm){var mask=AtmajaStairRevisionBuilder.UpperMask(arm);m.SetTexture("_MetallicGlossMap",mask);m.SetTexture("_OcclusionMap",mask);m.SetFloat("_Smoothness",1);m.EnableKeyword("_METALLICSPECGLOSSMAP");m.EnableKeyword("_OCCLUSIONMAP");}
            }
            var fresh=Surface("Review_FreshApple",Color.white,Art+"Textures/food_apple_01_diff_1k.jpg");fresh.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Textures/food_apple_01_nor_gl_1k.jpg"));fresh.EnableKeyword("_NORMALMAP");
            Surface("Review_Ceramic",new Color(.58f,.57f,.43f),null,.28f);Surface("Review_Mold",new Color(.075f,.105f,.045f));Surface("Review_Crack",new Color(.13f,.12f,.075f));
            foreach(var pair in new[]{("Review_HouseWall","House_MAT_PlasterDamaged"),("Review_HouseRoof","House_MAT_RoofTile"),("Review_HouseWood","House_MAT_WoodDoor"),("Review_HouseConcrete","House_MAT_Concrete")})
            {
                var template=Mat(pair.Item2);if(!template&&pair.Item1.Contains("Roof"))template=Mat("House_MAT_RoofTiles");
                var m=Surface(pair.Item1,Color.white);if(template)EditorUtility.CopySerialized(template,m);m.name=pair.Item1;EditorUtility.SetDirty(m);
            }
            Surface("Review_HouseGlass",new Color(.075f,.105f,.11f),null,.7f);
            foreach(string guid in AssetDatabase.FindAssets("t:Model",new[]{Art+"Models"}))
            {var importer=(ModelImporter)AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));importer.importAnimation=false;importer.importCameras=false;importer.importLights=false;importer.materialImportMode=ModelImporterMaterialImportMode.ImportStandard;importer.SaveAndReimport();}
        }
        public static GameObject ReviewModel(string name,string model,Transform parent,Vector3 center,Vector3 size,bool collision=true)
        {
            var go=new GameObject(name);go.transform.SetParent(parent,false);var asset=AssetDatabase.LoadAssetAtPath<GameObject>(Art+"Models/"+model+".fbx");if(!asset)throw new Exception("Missing downloaded review model "+model);
            var body=(GameObject)PrefabUtility.InstantiatePrefab(asset);body.transform.SetParent(go.transform,false);
            foreach(var renderer in body.GetComponentsInChildren<Renderer>())renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>AssetDatabase.LoadAssetAtPath<Material>(Art+"Materials/"+System.Text.RegularExpressions.Regex.Replace(m.name,@"\.\d+$","")+".mat")??throw new Exception("Review material missing "+m.name)).ToArray();
            if(size!=Vector3.zero)KosVisualV2.Resize(go,size);KosVisualV2.SetCenter(go,center);
            if(collision)KosVisualV2.FitBox(go.AddComponent<BoxCollider>(),KosVisualV2.BoundsOf(go));return go;
        }
        private static GameObject Box(string name,Vector3 center,Vector3 size,Material material,bool collision=true)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;go.transform.SetParent(root,false);go.transform.SetPositionAndRotation(center,Quaternion.LookRotation(d.north));go.transform.localScale=size;go.GetComponent<Renderer>().sharedMaterial=material;if(!collision)Object.DestroyImmediate(go.GetComponent<Collider>());return go;
        }
        private static void Center(string name,float x,float depth,float height,Vector3? size=null)
        {var go=Find(name);if(!go)return;if(size.HasValue)KosVisualV2.Resize(go,size.Value);KosVisualV2.SetCenter(go,P(x,depth,height));var box=go.GetComponent<BoxCollider>();if(box)KosVisualV2.FitBox(box,KosVisualV2.BoundsOf(go));}
        private static void FurnishHome()
        {
            var main=ReviewModel("FamilyLivingSofa","AtmajaFamilySofa",root,P(-3.4f,3.0f,1.07f),new Vector3(2.5f,1.1f,.85f));main.transform.Rotate(Vector3.up,180);KosVisualV2.SetCenter(main,P(-3.4f,3.0f,1.07f));
            var side=ReviewModel("FamilyReadingSofa","AtmajaFamilySofa",root,P(-5.3f,3.6f,1.02f),new Vector3(1.7f,1.0f,.65f));side.transform.Rotate(Vector3.up,-90);KosVisualV2.SetCenter(side,P(-5.3f,3.6f,1.02f));
            Box("FamilyLivingRug",P(-3.4f,3.9f,.54f),new Vector3(3.5f,.018f,2.1f),Mat("Upper_RedCarpet"),false);
            Box("FamilyCoffeeTable",P(-3.4f,3.75f,1.02f),new Vector3(1.2f,.08f,.42f),Mat("Upper_PineTable"));
            foreach(float x in new[]{-3.89f,-2.91f})foreach(float z in new[]{3.62f,3.88f})Box("FamilyCoffeeTableLeg",P(x,z,.77f),new Vector3(.055f,.46f,.055f),Mat("House_MAT_WoodDoor"),false);
            Box("FamilyRadioCabinet",P(.65f,1.2f,.99f),new Vector3(.72f,.94f,1.4f),Mat("House_MAT_WoodDoor"));
            foreach(float z in new[]{.75f,1.25f,1.65f})Box("FamilyCabinetPanel",P(.26f,z,1.0f),new Vector3(.025f,.59f,.32f),Mat("Upper_PineTable"),false);
            Box("FamilyRadioBody",P(.55f,1.1f,1.63f),new Vector3(.34f,.28f,.5f),Mat("Upper_HandycamBody"));
            foreach(float z in new[]{.96f,1.24f})Box("RadioSpeakerGrille",P(.35f,z,1.64f),new Vector3(.015f,.18f,.17f),Mat("Upper_SteelRust"),false);
            // The far chair is visibly empty without AR, as required by the scripted scare.
            Remove("SeatedDollOnEndChair");
            if(d.arFigure)
            {d.arFigure.transform.position=P(3.7f,2.15f,.52f);var bounds=KosVisualV2.BoundsOf(d.arFigure);d.arFigure.transform.position+=Vector3.up*(.54f-bounds.min.y);foreach(var t in d.arFigure.GetComponentsInChildren<Transform>(true))t.gameObject.layer=AtmajaPhoneView.ARLayer;}
        }
        public static void RefineLounge()
        {
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();root=Find("DesignerReviewEnvironment").transform;
            string stairs=StairSignature();
            foreach(var child in root.Cast<Transform>().Where(t=>t.name.StartsWith("Family")||t.name.StartsWith("Radio")).ToArray())Object.DestroyImmediate(child.gameObject);
            FurnishHome();Physics.SyncTransforms();
            if(stairs!=StairSignature())throw new Exception("Lounge refinement modified the protected stairs");
            WriteMeshSurvey();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/designer-review-stairs-preserved.json","{\"unchanged\":true,\"signature\":\""+stairs+"\"}\n");
            Debug.Log("DESIGNER_LOUNGE_REFINED inward-facing sofas and continuous floor-level corridor");
        }
        private static void DressPlates()
        {
            var plates=Objects().Where(t=>t.name=="MouldyPlate").ToArray();foreach(var t in plates)Object.DestroyImmediate(t.gameObject);
            var positions=Enumerable.Range(0,6).Select(i=>P(3.35f+(i%2)*.65f,3.1f+i*.60f,1.47f)).ToArray();
            for(int i=0;i<positions.Length;i++)
            {
                Vector3 p=positions[i];ReviewModel("MouldyPlate","MoldyCeramicPlate",root,p,new Vector3(.30f,.04f,.30f),false);
                ReviewModel("RottenDinner_"+i,i%2==0?"MoldyBread":"RottenApple",root,p+d.east*.055f+Vector3.up*.028f,new Vector3(.11f,.07f,.09f),false);
                ReviewModel("DeadRatOnPlate_"+i,"DeadStreetRat",root,p-d.east*.027f+Vector3.up*.0205f,new Vector3(.19f,.055f,.085f),false);
            }
        }
        public static void RepairChildRoom()
        {
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();root=Find("DesignerReviewEnvironment").transform;
            string stairs=StairSignature();Remove("BimaToyTrainCar","ARVentFracture");RefineChildRoom();
            if(stairs!=StairSignature())throw new Exception("Child-room repair modified protected stairs");
            EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
        }
        private static void RefineChildRoom()
        {
            Center("ChildShelf",-3.25f,.40f,4.52f,new Vector3(1.1f,1.4f,.28f));Center("ChildBooks",-3.25f,.40f,4.93f,new Vector3(.8f,.28f,.20f));
            Center("ChildWardrobe",-2.99f,1.20f,4.84f,new Vector3(.87f,2.04f,.55f));
            Center("ChildBed",-3.55f,2.85f,4.145f,new Vector3(1.08f,.63f,2.05f));Center("ChildMattress",-3.55f,2.85f,4.49f,new Vector3(.98f,.16f,1.92f));Center("ChildPillow",-3.55f,2.20f,4.63f,new Vector3(.56f,.12f,.33f));
            Center("ChildStudyChair",-5.27f,1.55f,4.25f);Center("ChildCarpet",-4.48f,2.70f,3.84f,new Vector3(1.35f,.025f,2.1f));
            Center("ChildDiaryBook",-5.32f,.62f,4.587f);Center("ChildDeskLamp",-5.65f,.48f,4.76f);
            Remove("ChildNotebook");DressChildCup();DrapeChildQuilt();
            Remove("ChildToyBlocks","ChildToyBall");
            for(int i=0;i<3;i++)Box("BimaToyTrainCar",P(-4.12f+i*.16f,3.75f,3.875f),new Vector3(.13f,.11f,.08f),Mat(i==0?"House_MAT_WoodDoor":"Upper_BookCover"),false);
            // Ventilasi berlubang asli + titik putih berkedip saat AR (bug 24).
            var vent=d.upper.ventHoleAR;if(vent){foreach(var text in vent.GetComponentsInChildren<TextMesh>(true))Object.DestroyImmediate(text);foreach(var renderer in vent.GetComponents<Renderer>())Object.DestroyImmediate(renderer);}
            Remove("ARVentFracture","VentBlinkAR");
            if(vent)
            {
                // Kisi horizontal dengan celah udara sungguhan, bukan tanda X.
                for(int s=0;s<5;s++)
                {
                    var slat=Box("VentSlat",vent.transform.position,new Vector3(.7f,.07f,.03f),Mat("House_MAT_WoodFrameWhite"),false);
                    slat.transform.SetParent(vent.transform,false);slat.transform.localPosition=new Vector3(0,-.36f+s*.18f,0);
                }
                // Titik putih kecil berkedip (AR): logika kedip di AtmajaUpperDirector.
                var glowMat=Surface("VentBlinkWhite",Color.white);glowMat.EnableKeyword("_EMISSION");glowMat.SetColor("_EmissionColor",Color.white*2.5f);EditorUtility.SetDirty(glowMat);
                var blink=new GameObject("VentBlinkAR");blink.transform.SetParent(vent.transform,false);blink.transform.localPosition=Vector3.zero;
                for(int b=0;b<3;b++)
                {
                    var dot=GameObject.CreatePrimitive(PrimitiveType.Quad);dot.name="VentBlinkDot";dot.transform.SetParent(blink.transform,false);
                    dot.transform.localPosition=new Vector3((b-1)*.2f,.1f,-.02f);dot.transform.localRotation=Quaternion.identity;
                    dot.GetComponent<Renderer>().sharedMaterial=glowMat;Object.DestroyImmediate(dot.GetComponent<Collider>());
                    foreach(var t in dot.GetComponentsInChildren<Transform>(true))t.gameObject.layer=AtmajaPhoneView.ARLayer;
                }
                d.upper.ventBlink=blink;
            }
            var scratches=d.upper.bathScratchesAR;
            if(scratches)
            {
                Remove("SmallBloodyFingermark");
                foreach(var text in scratches.GetComponentsInChildren<TextMesh>(true))Object.DestroyImmediate(text);
                for(int i=0;i<12;i++)
                {var cut=Box("SmallBloodyFingermark",scratches.transform.position,new Vector3(.006f,.15f+(i%3)*.025f,.002f),Mat("Upper_BloodSmear"),false);cut.transform.SetParent(scratches.transform,false);cut.transform.localPosition=new Vector3((i%4-.15f)*.025f-.04f,(i/4)*.12f-.12f,0);cut.transform.localRotation=Quaternion.Euler(0,0,15-i*2);cut.layer=AtmajaPhoneView.ARLayer;}
            }
        }
        private static void DressChildCup()
        {
            var cup=d.upper.childCup;var renderer=cup.GetComponent<Renderer>();if(renderer)renderer.enabled=false;
            foreach(var child in cup.transform.Cast<Transform>().Where(t=>t.name=="BimaMilkCupVisual").ToArray())Object.DestroyImmediate(child.gameObject);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Original/Models/EnamelMug.fbx"));model.name="BimaMilkCupVisual";model.transform.SetParent(cup.transform,false);
            var glaze=Surface("ChildCupGlaze",new Color(.63f,.61f,.49f),null,.34f);var milk=Surface("ChildSpoiledMilk",new Color(.52f,.49f,.34f),null,.2f);
            foreach(var part in model.GetComponentsInChildren<Renderer>())part.sharedMaterials=part.sharedMaterials.Select(m=>m.name.Contains("Coffee")?milk:glaze).ToArray();
            KosVisualV2.Resize(model,new Vector3(.095f,.10f,.085f));KosVisualV2.SetCenter(model,P(-5,.62f,4.615f));
        }
        private static void DrapeChildQuilt()
        {
            var quilt=Find("ChildBlanket");quilt.transform.SetPositionAndRotation(P(-3.55f,3.25f,4.585f),Quaternion.LookRotation(d.north));quilt.transform.localScale=Vector3.one;
            const int columns=24,rows=7;var vertices=new List<Vector3>();var uv=new List<Vector2>();var triangles=new List<int>();
            for(int layer=0;layer<2;layer++)for(int z=0;z<=rows;z++)for(int x=0;x<=columns;x++)
            {float across=(x/(float)columns-.5f)*1.16f,depth=(z/(float)rows-.5f)*.92f;float edge=Mathf.Clamp01((Mathf.Abs(across)-.43f)/.15f);float y=-Mathf.Sin(edge*Mathf.PI*.5f)*.16f+Mathf.Sin(across*48+depth*9)*.004f-layer*.014f;vertices.Add(new Vector3(across,y,depth));uv.Add(new Vector2(x/(float)columns,z/(float)rows));}
            int count=(columns+1)*(rows+1);
            for(int z=0;z<rows;z++)for(int x=0;x<columns;x++)
            {int a=z*(columns+1)+x,b=a+1,c=a+columns+1,e=c+1;triangles.AddRange(new[]{a,c,b,b,c,e,a+count,b+count,c+count,b+count,e+count,c+count});}
            var mesh=new Mesh{name="ChildDrapedQuilt"};mesh.SetVertices(vertices);mesh.SetUVs(0,uv);mesh.SetTriangles(triangles,0);mesh.RecalculateNormals();mesh.RecalculateTangents();mesh.RecalculateBounds();
            string path=Art+"Models/ChildDrapedQuilt.asset";var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(saved){EditorUtility.CopySerialized(mesh,saved);Object.DestroyImmediate(mesh);mesh=saved;}else AssetDatabase.CreateAsset(mesh,path);
            quilt.GetComponent<MeshFilter>().sharedMesh=mesh;quilt.GetComponent<Renderer>().sharedMaterial=Mat("Upper_BedFabric");
        }
        private static void OpenDiningAisles()
        {
            // Open plan living/dining keeps both table aisles accessible without widening the stairs.
            Remove("DiningDividerSouth","DiningDividerNorth","DiningDividerLintel");
            foreach(var chair in Objects().Where(t=>t.name=="WornDiningChair"))
            {var p=d.LocalPosition(chair.position);if(p.x>4.3f)KosVisualV2.SetCenter(chair.gameObject,P(4.65f,p.y,1.069f));}
        }
        private static Transform Anchor(string name,float x,float depth,float y)
        {var go=new GameObject(name);go.transform.SetParent(root,false);go.transform.position=P(x,depth,y);return go.transform;}
        private static void ExpandBasement()
        {
            Remove("CellarWallE","CellarWallS");var stone=Mat("Upper_ConcretePaint");
            Center("CellarFloor",6.1f,6.55f,-2.8125f,new Vector3(11.8f,.025f,11.9f));
            Center("CellarCeiling",2.9f,3.75f,0,new Vector3(5.4f,1.0f,6.3f));
            foreach(string name in new[]{"CellarFloor","CellarCeiling"}){var go=Find(name);if(!go.GetComponent<BoxCollider>())go.AddComponent<BoxCollider>();}
            Box("ExpandedCellarEastCeiling",P(8.8f,6.55f,-.325f),new Vector3(6.4f,.35f,11.9f),stone);
            Box("ExpandedCellarSouthCeiling",P(2.9f,9.7f,-.325f),new Vector3(5.4f,.35f,5.6f),stone);
            Center("CellarWallN",6.1f,.6f,-1.575f,new Vector3(11.8f,2.45f,.15f));Center("CellarWallW",.2f,6.55f,-1.575f,new Vector3(.15f,2.45f,11.9f));
            Box("ExpandedCellarEastWall",P(12,6.55f,-1.575f),new Vector3(.15f,2.45f,11.9f),stone);
            Box("ExpandedCellarSouthWall",P(6.1f,12.5f,-1.575f),new Vector3(11.8f,2.45f,.15f),stone);
            Box("CellarSpineNorth",P(6.1f,3.35f,-1.575f),new Vector3(.16f,2.45f,2.5f),stone);
            Box("CellarSpineSouth",P(6.1f,9.0f,-1.575f),new Vector3(.16f,2.45f,3.6f),stone);
            Box("CellarReturnPartition",P(8.7f,6.3f,-1.575f),new Vector3(.16f,2.45f,5.8f),stone);
            for(int i=0;i<4;i++)
            {Box("CellarTextileChest",P(11.35f,3+i*2.0f,-2.35f),new Vector3(.65f,.90f,.9f),Mat("House_MAT_WoodDoor"));Box("TextileChestLid",P(11.35f,3+i*2.0f,-1.88f),new Vector3(.69f,.05f,.94f),Mat("Upper_PineTable"));}
            var extra=new[]{Anchor("CellarExpandedPatrol1",7.2f,6,-2.79f),Anchor("CellarExpandedPatrol2",7.2f,11.5f,-2.79f),Anchor("CellarExpandedPatrol3",10.1f,11.5f,-2.79f),Anchor("CellarExpandedPatrol4",10.1f,2.1f,-2.79f),Anchor("CellarExpandedPatrol5",7.2f,2.1f,-2.79f)};
            d.basement.waypoints=d.basement.waypoints.Where(t=>t&&!t.name.StartsWith("CellarExpandedPatrol")).Concat(extra).ToArray();
            foreach(var p in new[]{P(7.3f,6,-.7f),P(10.1f,10.6f,-.7f),P(10.1f,2.0f,-.7f)})
            {var lamp=Box("CellarUtilityFixture",p,new Vector3(.55f,.06f,.14f),Mat("Upper_SteelRust"),false);var light=new GameObject("CellarUtilityLight",typeof(Light));light.transform.SetParent(lamp.transform,true);light.transform.position=p-Vector3.up*.10f;var l=light.GetComponent<Light>();l.type=LightType.Point;l.range=5;l.intensity=.55f;l.color=new Color(.72f,.76f,.65f);l.shadows=LightShadows.None;}
        }
        private static void BuildWell()
        {
            var well=Find("OldChainedKitchenWell");if(well){var renderer=well.GetComponent<Renderer>();if(renderer)renderer.enabled=false;var collider=well.GetComponent<Collider>();if(collider)collider.enabled=false;}
            var cover=new GameObject("WellCover");cover.transform.SetParent(root,false);cover.transform.SetPositionAndRotation(P(2.85f,11.73f,1.09f),Quaternion.LookRotation(d.north));
            Remove("RottenWellBoard","WellRustChain");
            // Tutup bulat pas lingkaran sumur, bukan papan kotak yang melewati bibir.
            var lid=GameObject.CreatePrimitive(PrimitiveType.Cylinder);lid.name="WellRoundCover";lid.transform.SetParent(cover.transform,true);lid.transform.SetPositionAndRotation(P(2.85f,11.2f,1.09f),Quaternion.LookRotation(d.north));lid.transform.localScale=new Vector3(1.1f,.03f,1.1f);lid.GetComponent<Renderer>().sharedMaterial=Mat("House_MAT_WoodDoor");
            var chain=new GameObject("WellRustChain",typeof(MeshFilter),typeof(MeshRenderer));chain.transform.SetParent(cover.transform,true);chain.transform.SetPositionAndRotation(P(2.85f,11.2f,1.14f),Quaternion.LookRotation(d.north));chain.GetComponent<MeshFilter>().sharedMesh=AssetDatabase.LoadAssetAtPath<Mesh>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/WellChain.asset");chain.GetComponent<MeshRenderer>().sharedMaterial=Mat("House_MAT_RustMetal");
            for(int i=0;i<20;i++)
            {
                float a=i*Mathf.PI*2/20;var block=Box("WellStoneRing",P(2.85f+Mathf.Cos(a)*.50f,11.2f+Mathf.Sin(a)*.50f,.73f),new Vector3(.20f,.72f,.19f),Mat("House_MAT_BrickBase"));block.transform.rotation=Quaternion.LookRotation(d.north)*Quaternion.Euler(0,-a*Mathf.Rad2Deg,0);var item=block.AddComponent<AtmajaItem>();item.director=d;item.action=AtmajaAction.Well;item.label="Geser papan penutup sumur";
                Box("WellInnerStone",P(2.85f+Mathf.Cos(a)*.48f,11.2f+Mathf.Sin(a)*.48f,-1.9f),new Vector3(.17f,5.2f,.17f),Mat("House_MAT_BrickBase")).transform.rotation=block.transform.rotation;
            }
            Box("WellBlackDepth",P(2.85f,11.2f,-4.5f),new Vector3(1.0f,.1f,1.0f),Mat("Upper_HandycamBody"),false);
            foreach(var board in cover.GetComponentsInChildren<Collider>())
            {var item=board.gameObject.AddComponent<AtmajaItem>();item.director=d;item.action=AtmajaAction.Well;item.label="Geser papan penutup sumur";}
            CutWellOpening();
        }
        private static void RestoreWellFloorPrimitives()
        {
            var temporary=GameObject.CreatePrimitive(PrimitiveType.Cube);var cube=temporary.GetComponent<MeshFilter>().sharedMesh;
            foreach(var filter in Objects().Select(t=>t.GetComponent<MeshFilter>()).Where(f=>f&&f.sharedMesh&&new[]{"CellarCeiling","CellarFloor","WetCourtyard","KitchenDirtyTile"}.Contains(f.name)&&AssetDatabase.GetAssetPath(f.sharedMesh).StartsWith(Art+"Models/WellOpening_")))filter.sharedMesh=cube;
            Object.DestroyImmediate(temporary);
        }
        private static void RestoreWellFloorColliders()
        {
            Remove("KitchenWellFloorWest","KitchenWellFloorEast","KitchenWellFloorNorth","KitchenWellFloorSouth");
            foreach(var plan in new[]{("WetCourtyard",new Vector3(0,-.025f,-1),new Vector3(25,.05f,31)),("KitchenDirtyTile",P(4,10.6f,.327f),new Vector3(3.7f,.05f,2.9f)),("COL_PLINTH_annex",P(4,10.6f,.17f),new Vector3(4.4f,.34f,3.6f))})
            {
                var go=Find(plan.Item1);if(!go)throw new Exception("Well floor collision source missing "+plan.Item1);
                var collider=go.GetComponent<BoxCollider>();if(!collider)collider=go.AddComponent<BoxCollider>();
                var world=new Bounds(plan.Item2,plan.Item3);var local=new Bounds(go.transform.InverseTransformPoint(world.min),Vector3.zero);
                for(int corner=0;corner<8;corner++)local.Encapsulate(go.transform.InverseTransformPoint(world.center+Vector3.Scale(world.extents,new Vector3((corner&1)==0?-1:1,(corner&2)==0?-1:1,(corner&4)==0?-1:1))));
                collider.center=local.center;collider.size=local.size;
            }
        }
        private static void ApplyFurnitureLods()
        {
            foreach(var pair in new[]{("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/desk_lamp_arm_01/desk_lamp_arm_01.fbx","LodDeskLamp"),("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/wooden_bookshelf_worn/wooden_bookshelf_worn.fbx","LodBookshelf"),("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/shrub_03.fbx","LodShrub")})
            {
                var meshes=AssetDatabase.LoadAllAssetsAtPath(Art+"Models/"+pair.Item2+".fbx").OfType<Mesh>().ToArray();
                foreach(var filter in Objects().Select(t=>t.GetComponent<MeshFilter>()).Where(f=>f&&f.sharedMesh&&AssetDatabase.GetAssetPath(f.sharedMesh)==pair.Item1))
                {
                    var mesh=meshes.FirstOrDefault(m=>m.name==filter.name||m.name==filter.sharedMesh.name);
                    if(!mesh)throw new Exception("Missing review LOD mesh "+filter.name+" / "+filter.sharedMesh.name);
                    // FBX unit conversion can differ between source exporters. Preserve each original local bound.
                    var original=filter.sharedMesh.bounds;var bounds=mesh.bounds;var copy=Object.Instantiate(mesh);var vertices=copy.vertices;
                    var scale=new Vector3(original.size.x/Mathf.Max(.00001f,bounds.size.x),original.size.y/Mathf.Max(.00001f,bounds.size.y),original.size.z/Mathf.Max(.00001f,bounds.size.z));
                    for(int i=0;i<vertices.Length;i++)vertices[i]=original.center+Vector3.Scale(vertices[i]-bounds.center,scale);
                    copy.vertices=vertices;copy.RecalculateBounds();copy.name=filter.sharedMesh.name+"_ReviewLOD";
                    string path=Art+"Models/"+pair.Item2+"_"+filter.name.Replace(' ','_')+".asset";var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(saved){EditorUtility.CopySerialized(copy,saved);Object.DestroyImmediate(copy);copy=saved;}else AssetDatabase.CreateAsset(copy,path);
                    filter.sharedMesh=copy;if(PrefabUtility.IsPartOfPrefabInstance(filter))PrefabUtility.RecordPrefabInstancePropertyModifications(filter);
                }
            }
        }
        private struct CutVertex
        {
            public Vector3 p,n;public Vector2 uv;public Vector4 tangent;
            public static CutVertex Lerp(CutVertex a,CutVertex b,float t)=>new CutVertex{p=Vector3.Lerp(a.p,b.p,t),n=Vector3.Lerp(a.n,b.n,t),uv=Vector2.Lerp(a.uv,b.uv,t),tangent=Vector4.Lerp(a.tangent,b.tangent,t)};
        }
        private static List<CutVertex> Clip(List<CutVertex> polygon,Func<CutVertex,float> side)
        {
            var output=new List<CutVertex>();if(polygon.Count==0)return output;
            var previous=polygon[polygon.Count-1];float before=side(previous);
            foreach(var current in polygon)
            {float after=side(current);if((before>=0)!=(after>=0))output.Add(CutVertex.Lerp(previous,current,before/(before-after)));if(after>=0)output.Add(current);previous=current;before=after;}
            return output;
        }
        private static void CutWellOpening()
        {
            var center=P(2.85f,11.2f,.35f);float minX=center.x-.43f,maxX=center.x+.43f,minZ=center.z-.43f,maxZ=center.z+.43f;
            foreach(var filter in Objects().Select(t=>t.GetComponent<MeshFilter>()).Where(f=>f&&f.sharedMesh).ToArray())
            {
                var renderer=filter.GetComponent<Renderer>();if(filter.name.StartsWith("Well"))continue;
                if(!renderer||renderer.bounds.max.y< -3f||renderer.bounds.min.y>.60f||renderer.bounds.min.x>maxX||renderer.bounds.max.x<minX||renderer.bounds.min.z>maxZ||renderer.bounds.max.z<minZ)continue;
                var source=filter.sharedMesh;var positions=source.vertices;var normals=source.normals;var uvs=source.uv;var tangents=source.tangents;
                var vertices=new List<CutVertex>();var submeshes=new List<int[]>();bool changed=false;
                for(int sub=0;sub<source.subMeshCount;sub++)
                {
                    var indices=new List<int>();var triangles=source.GetTriangles(sub);
                    for(int i=0;i<triangles.Length;i+=3)
                    {
                        var triangle=new List<CutVertex>();
                        for(int j=0;j<3;j++){int id=triangles[i+j];triangle.Add(new CutVertex{p=filter.transform.TransformPoint(positions[id]),n=normals.Length==positions.Length?normals[id]:Vector3.up,uv=uvs.Length==positions.Length?uvs[id]:Vector2.zero,tangent=tangents.Length==positions.Length?tangents[id]:new Vector4(1,0,0,1)});}
                        var a=triangle[0].p;var b=triangle[1].p;var c=triangle[2].p;
                        bool horizontal=Mathf.Abs(a.y-b.y)<.001f&&Mathf.Abs(a.y-c.y)<.001f&&a.y> -3f&&a.y<.60f;
                        var remaining=triangle;var fragments=new List<List<CutVertex>>();
                        if(horizontal)
                        {
                            var inside=Clip(Clip(Clip(Clip(triangle,v=>v.p.x-minX),v=>maxX-v.p.x),v=>v.p.z-minZ),v=>maxZ-v.p.z);
                            if(inside.Count>=3&&Vector3.Cross(inside[1].p-inside[0].p,inside[2].p-inside[0].p).sqrMagnitude>1e-12f)
                            {
                                changed=true;
                                fragments.Add(Clip(remaining,v=>minX-v.p.x));remaining=Clip(remaining,v=>v.p.x-minX);
                                fragments.Add(Clip(remaining,v=>v.p.x-maxX));remaining=Clip(remaining,v=>maxX-v.p.x);
                                fragments.Add(Clip(remaining,v=>minZ-v.p.z));remaining=Clip(remaining,v=>v.p.z-minZ);
                                fragments.Add(Clip(remaining,v=>v.p.z-maxZ));
                            }
                            else fragments.Add(triangle);
                        }
                        else fragments.Add(triangle);
                        foreach(var polygon in fragments)
                            for(int j=1;j+1<polygon.Count;j++)foreach(var v in new[]{polygon[0],polygon[j],polygon[j+1]}){indices.Add(vertices.Count);var local=v;local.p=filter.transform.InverseTransformPoint(v.p);vertices.Add(local);}
                    }
                    submeshes.Add(indices.ToArray());
                }
                if(!changed)continue;
                var mesh=new Mesh{name=source.name+"_WellOpening",indexFormat=UnityEngine.Rendering.IndexFormat.UInt32};mesh.SetVertices(vertices.Select(v=>v.p).ToList());mesh.SetNormals(vertices.Select(v=>v.n).ToList());mesh.SetUVs(0,vertices.Select(v=>v.uv).ToList());mesh.SetTangents(vertices.Select(v=>v.tangent).ToList());mesh.subMeshCount=submeshes.Count;for(int s=0;s<submeshes.Count;s++)mesh.SetTriangles(submeshes[s],s);mesh.RecalculateBounds();
                string path=Art+"Models/WellOpening_"+filter.name.Replace(' ','_')+".asset";var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);if(saved){EditorUtility.CopySerialized(mesh,saved);Object.DestroyImmediate(mesh);mesh=saved;}else AssetDatabase.CreateAsset(mesh,path);
                filter.sharedMesh=mesh;PrefabUtility.RecordPrefabInstancePropertyModifications(filter);
            }
            RestoreWellFloorColliders();Physics.SyncTransforms();
            foreach(var col in Object.FindObjectsByType<BoxCollider>(FindObjectsSortMode.None).Where(c=>c.bounds.max.y> -3f&&c.bounds.max.y<.60f&&c.bounds.min.x<minX&&c.bounds.max.x>maxX&&c.bounds.min.z<minZ&&c.bounds.max.z>maxZ).ToArray())
            {
                var bounds=col.bounds;Object.DestroyImmediate(col);
                CollisionPart("KitchenWellFloorWest",new Vector3((bounds.min.x+minX)/2,bounds.center.y,bounds.center.z),new Vector3(minX-bounds.min.x,bounds.size.y,bounds.size.z));
                CollisionPart("KitchenWellFloorEast",new Vector3((bounds.max.x+maxX)/2,bounds.center.y,bounds.center.z),new Vector3(bounds.max.x-maxX,bounds.size.y,bounds.size.z));
                CollisionPart("KitchenWellFloorNorth",new Vector3(center.x,bounds.center.y,(bounds.min.z+minZ)/2),new Vector3(.86f,bounds.size.y,minZ-bounds.min.z));
                CollisionPart("KitchenWellFloorSouth",new Vector3(center.x,bounds.center.y,(bounds.max.z+maxZ)/2),new Vector3(.86f,bounds.size.y,bounds.max.z-maxZ));
            }
        }
        private static void CollisionPart(string name,Vector3 position,Vector3 size)
        {
            var part=new GameObject(name,typeof(BoxCollider));part.transform.SetParent(root.parent,false);part.transform.position=position;
            part.GetComponent<BoxCollider>().size=size;
        }
    }
}
#endif
