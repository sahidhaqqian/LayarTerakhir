#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Final, idempotent dressing pass over the approved prolog scene.
    public static class PrologReviewRevision
    {
        public const string Art = "Assets/_LayarTerakhir/assets/Art/PrologRevision/";
        private static AtmajaDirector d;
        private static Transform root;
        private static Transform[] Objects() => SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g => g.GetComponentsInChildren<Transform>(true)).ToArray();
        private static GameObject Find(string name) => Objects().FirstOrDefault(t => t.name == name)?.gameObject;
        private static Vector3 P(float x,float z,float y) => d.P(x,z,y);
        private static Material OldMat(string name) => AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/"+name+".mat");
        private static void Remove(params string[] names)
        { foreach(var t in Objects().Where(t=>names.Contains(t.name)).ToArray())if(t)Object.DestroyImmediate(t.gameObject); }
        private static Material Material(string name,Color color,string diffuse=null,string normal=null,float smooth=.18f)
        {
            string path=Art+"Materials/"+name+".mat";
            var m=AssetDatabase.LoadAssetAtPath<Material>(path);
            if(!m){m=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(m,path);}
            m.color=color;m.SetFloat("_Smoothness",smooth);
            if(diffuse!=null)m.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Textures/"+diffuse));
            if(normal!=null){m.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Textures/"+normal));m.EnableKeyword("_NORMALMAP");m.SetFloat("_BumpScale",.55f);}
            EditorUtility.SetDirty(m);return m;
        }
        private static GameObject Box(string name,float x,float z,float y,Vector3 size,Material material,bool solid=true)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;go.transform.SetParent(root,false);
            go.transform.SetPositionAndRotation(P(x,z,y),Quaternion.LookRotation(d.north));go.transform.localScale=size;
            go.GetComponent<Renderer>().sharedMaterial=material;if(!solid)Object.DestroyImmediate(go.GetComponent<Collider>());return go;
        }
        private static void Fit(GameObject go)
        {
            foreach(var collider in go.GetComponents<Collider>())Object.DestroyImmediate(collider);
            KosVisualV2.FitBox(go.AddComponent<BoxCollider>(),KosVisualV2.BoundsOf(go));
        }
        private static void Place(GameObject go,Vector3 center,Vector3? size=null)
        {if(!go)return;if(size.HasValue)KosVisualV2.Resize(go,size.Value);KosVisualV2.SetCenter(go,center);if(go.GetComponent<BoxCollider>())Fit(go);}
        private static GameObject Model(string name,string asset,Vector3 center,Vector3 size,Material material,float yaw=0,bool solid=true)
        {
            var go=new GameObject(name);go.transform.SetParent(root,false);go.transform.rotation=Quaternion.LookRotation(d.north)*Quaternion.Euler(0,yaw,0);
            var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(Art+"Models/"+asset+".fbx");
            if(!prefab)throw new Exception("Missing prolog revision model "+asset);
            var mesh=(GameObject)PrefabUtility.InstantiatePrefab(prefab);mesh.transform.SetParent(go.transform,false);
            foreach(var r in mesh.GetComponentsInChildren<Renderer>())r.sharedMaterials=r.sharedMaterials.Select(_=>material).ToArray();
            KosVisualV2.Resize(go,size);KosVisualV2.SetCenter(go,center);if(solid)Fit(go);return go;
        }
        [MenuItem("LayarTerakhir/Prolog/Apply playtest bugs 28–46")]
        public static void Apply()
        {
            Directory.CreateDirectory(Art+"Materials");
            AssetDatabase.Refresh();
            foreach(string guid in AssetDatabase.FindAssets("t:Texture2D",new[]{Art+"Textures"}))
            {
                var importer=(TextureImporter)AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid));
                importer.textureType=importer.assetPath.Contains("nor_gl")||importer.assetPath.Contains("_norm")?TextureImporterType.NormalMap:TextureImporterType.Default;
                importer.maxTextureSize=importer.assetPath.EndsWith("RealWallClawPhoto.jpg")?4096:1024;importer.mipmapEnabled=true;importer.alphaIsTransparency=importer.assetPath.EndsWith("ClawScratch.png");importer.SaveAndReimport();
            }
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();
            // Revision objects with serialized gameplay references are recreated below.
            Remove("PrologReviewRevision");root=new GameObject("PrologReviewRevision").transform;
            ChildRoom();Bathroom();MasterRoom();Dining();Basement();
            if(!d.basement.nyonya.GetComponentsInChildren<SkinnedMeshRenderer>(true).Any(r=>r.name=="NyonyaDesignerBody"))DesignerVrmBuilder.ApplyNyonya(d);
            Physics.SyncTransforms();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            File.WriteAllText("Audit/prolog-review-authoring.json",JsonUtility.ToJson(new Receipt{
                utc=DateTime.UtcNow.ToString("o"),masterWidth=4.65f,basementWidth=11.8f,basementDepth=11.9f,
                importedBeds=2,servingFood=Find("PrologServingFood").transform.childCount,wallScratches=d.upper.bathScratchesOnWall,visibleLadder=Find("CellarAccessLadder")!=null},true)+"\n");
            Debug.Log("PROLOG_REVIEW_28_46_APPLIED");
        }
        [Serializable] private class Receipt{public string utc;public float masterWidth,basementWidth,basementDepth;public int importedBeds,servingFood;public bool wallScratches,visibleLadder;}
        public static void ApplyAndCapture()
        {
            Apply();
            string folder="Audit/Visuals/Prolog-28-46";Directory.CreateDirectory(folder);
            d.upper.bathWater.SetActive(true);d.upper.bathCurtain.SetActive(false);d.upper.bathScratchesAR.SetActive(true);
            var go=new GameObject("RevisionReviewCamera",typeof(Camera),typeof(Light));var camera=go.GetComponent<Camera>();
            camera.fieldOfView=65;camera.nearClipPlane=.04f;camera.farClipPlane=60;
            var light=go.GetComponent<Light>();light.type=LightType.Spot;light.range=16;light.spotAngle=95;light.intensity=3;light.shadows=LightShadows.Soft;
            var frames=new[]{
                ("child-bed",P(-5.35f,4.3f,5.6f),P(-3.2f,1.5f,4.45f)),
                ("notebook-lamp-cup",P(-4.6f,1.95f,5.4f),P(-5.32f,.60f,4.60f)),
                ("bathroom",P(-.8f,3.5f,5.5f),P(-1.1f,.9f,4.3f)),
                ("wall-scratches",P(-.9f,3.7f,5f),P(-2.29f,3.95f,4.9f)),
                ("master",P(1.2f,4.65f,5.65f),P(3.3f,1.65f,4.65f)),
                ("cellar-ladder",P(1.05f,2.25f,-1.1f),P(1.02f,1.25f,.2f)),
                ("cellar-open-space",P(5f,5.8f,-1.2f),P(10f,9.5f,-1.5f)),
                ("dining",P(2.3f,6.2f,3f),P(3.7f,4.5f,1.5f))};
            var target=new RenderTexture(960,720,24);camera.targetTexture=target;
            foreach(var frame in frames)
            {
                go.transform.position=frame.Item2;go.transform.LookAt(frame.Item3);camera.Render();
                var prior=RenderTexture.active;RenderTexture.active=target;var pixels=new Texture2D(960,720,TextureFormat.RGB24,false);
                pixels.ReadPixels(new Rect(0,0,960,720),0,0);pixels.Apply();File.WriteAllBytes(folder+"/"+frame.Item1+".png",pixels.EncodeToPNG());
                RenderTexture.active=prior;Object.DestroyImmediate(pixels);
            }
            foreach(string name in new[]{"ChildBedSet","BathTub","CellarAccessLadder"})
                foreach(var f in Find(name).GetComponentsInChildren<MeshFilter>())
                    Debug.Log("REVISION_MODEL "+name+"/"+f.name+" mesh="+f.sharedMesh.bounds+" rotation="+f.transform.rotation.eulerAngles+" local="+f.transform.localEulerAngles);
            Vector3 probe=P(-1.1f,2.6f,3.82f);
            foreach(var f in Objects().Select(t=>t.GetComponent<MeshFilter>()).Where(f=>f&&f.sharedMesh))
            {
                var r=f.GetComponent<Renderer>();if(!r||!r.enabled||r.bounds.min.x>probe.x||r.bounds.max.x<probe.x||r.bounds.min.z>probe.z||r.bounds.max.z<probe.z||r.bounds.min.y>3.9f||r.bounds.max.y<3.7f)continue;
                var vertices=f.sharedMesh.vertices;var triangles=f.sharedMesh.triangles;
                for(int i=0;i<triangles.Length;i+=3)
                {
                    var a=f.transform.TransformPoint(vertices[triangles[i]]);var b=f.transform.TransformPoint(vertices[triangles[i+1]]);var c=f.transform.TransformPoint(vertices[triangles[i+2]]);
                    if(Mathf.Abs(a.y-b.y)<.002f&&Mathf.Abs(a.y-c.y)<.002f&&a.y>3.7f&&a.y<3.9f)
                    {Debug.Log("REVISION_FLOOR "+f.name+" y="+a.y+" asset="+AssetDatabase.GetAssetPath(f.sharedMesh));break;}
                }
            }
            camera.targetTexture=null;Object.DestroyImmediate(target);Object.DestroyImmediate(go);
            Debug.Log("PROLOG_REVIEW_EDITOR_CAPTURES "+frames.Length);
        }
        private static void ChildRoom()
        {
            Remove("ChildWardrobe","ChildShelf","ChildBooks","ChildBed","ChildMattress","ChildPillow","ChildBlanket","BimaToyTrainCar","ChildToyBlocks","ChildToyBall");
            var bedding=Material("VintageBedding",Color.white,"vintage_day_bed_diff_1k.jpg","vintage_day_bed_nor_gl_1k.jpg");
            Model("ChildBedSet","vintage_day_bed",P(-3.12f,1.43f,4.405f),new Vector3(1.12f,1.13f,2.10f),bedding,270);
            // Preserve the diary's interaction root; replace only the block visual.
            var book=Find("ChildDiaryBook");var r=book.GetComponent<Renderer>();if(r)r.enabled=false;
            Remove("BimaNotebookVisual");
            var notebook=Material("Notebook",Color.white,"binder_notebook_diff_1k.jpg","binder_notebook_nor_gl_1k.jpg");
            var visual=Model("BimaNotebookVisual","binder_notebook",P(-5.3f,.6f,4.59f),new Vector3(.25f,.045f,.32f),notebook,0,false);
            visual.transform.SetParent(book.transform,true);Fit(book);
            var lamp=Find("ChildDeskLamp");
            if(lamp)
            {
                var center=KosVisualV2.BoundsOf(lamp).center;
                lamp.transform.rotation=Quaternion.LookRotation(d.north);KosVisualV2.SetCenter(lamp,center);
                var light=Find("ChildStudyPool")??new GameObject("ChildStudyPool",typeof(Light));light.transform.SetParent(lamp.transform,true);
                light.transform.position=P(-5.6f,.57f,4.91f);light.transform.LookAt(P(-5.3f,.65f,4.54f));
                var l=light.GetComponent<Light>();l.type=LightType.Spot;l.spotAngle=70;l.range=1.6f;l.intensity=.65f;l.color=new Color(1,.84f,.63f);l.shadows=LightShadows.None;
            }
            ReplaceChildCup();
        }
        private static void ReplaceChildCup()
        {
            const string path=Art+"Models/CrackedPorcelainCup.fbx";
            var importer=AssetImporter.GetAtPath(path) as ModelImporter;
            if(!importer)throw new Exception("Run Tools/author_cracked_porcelain_cup.py in Blender first");
            importer.importAnimation=importer.importCameras=importer.importLights=false;
            importer.isReadable=true;importer.meshCompression=ModelImporterMeshCompression.Off;
            importer.materialImportMode=ModelImporterMaterialImportMode.ImportStandard;importer.SaveAndReimport();
            Remove("BimaMilkCupVisual","BimaMilkCupMesh","BimaCrackedCup");
            var cup=d.upper.childCup;if(cup.GetComponent<Renderer>())cup.GetComponent<Renderer>().enabled=false;
            var materials=new System.Collections.Generic.Dictionary<string,Material>{
                {"CeramicGlaze",Material("BimaPorcelainGlaze",new Color(.78f,.76f,.69f),null,null,.70f)},
                {"CeramicCore",Material("BimaPorcelainCore",new Color(.59f,.53f,.43f),null,null,.20f)},
                {"CeramicFracture",Material("BimaPorcelainFracture",new Color(.095f,.061f,.037f),null,null,.08f)},
                {"SpoiledMilk",Material("BimaSpoiledMilk",new Color(.68f,.66f,.53f),null,null,.50f)}};
            var mug=new GameObject("BimaCrackedCup");mug.transform.SetParent(root,false);mug.transform.rotation=Quaternion.LookRotation(d.north);
            var body=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(path));body.transform.SetParent(mug.transform,false);
            foreach(var renderer in body.GetComponentsInChildren<Renderer>())
                renderer.sharedMaterials=renderer.sharedMaterials.Select(m=>materials.TryGetValue(m.name,out var material)?material:throw new Exception("Unknown original cup material "+m.name)).ToArray();
            // Preserve authored proportions. Independently fitting X/Y/Z would
            // distort the thin wall, circular opening and sub-millimetre fracture.
            mug.transform.localScale*=.095f/KosVisualV2.BoundsOf(mug).size.y;
            KosVisualV2.SetCenter(mug,P(-5.02f,.62f,4.6085f));
            mug.transform.SetParent(cup.transform,true);
            var shell=mug.GetComponentsInChildren<MeshRenderer>().Single(r=>r.name=="PorcelainShell");
            d.upper.cupAim.position=shell.bounds.center;Fit(cup);
        }
        [MenuItem("LayarTerakhir/Prolog/Apply original Blender cup (bug 33)")]
        public static void ApplyCupOnly()
        {
            Directory.CreateDirectory(Art+"Materials");AssetDatabase.Refresh();
            var scene=EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();
            root=Find("PrologReviewRevision").transform;ReplaceChildCup();
            Physics.SyncTransforms();EditorSceneManager.MarkSceneDirty(scene);EditorSceneManager.SaveScene(scene);AssetDatabase.SaveAssets();
            Debug.Log("PROLOG_ORIGINAL_CUP_APPLIED");
        }
        public static void ApplyCupOnlyAndCapture()
        {
            ApplyCupOnly();
            var atmosphere=d.GetComponent<AtmajaEvidenceAtmosphere>();if(atmosphere&&atmosphere.cupAura)atmosphere.cupAura.enabled=false;
            string folder="Audit/Visuals/Prolog-Cup-Unity";Directory.CreateDirectory(folder);
            var go=new GameObject("CupSceneReviewCamera",typeof(Camera),typeof(Light));var camera=go.GetComponent<Camera>();
            camera.fieldOfView=44;camera.nearClipPlane=.004f;camera.farClipPlane=25;
            var light=go.GetComponent<Light>();light.type=LightType.Spot;light.range=3;light.spotAngle=85;light.shadows=LightShadows.None;
            var frames=new[]{("cup-close",P(-5.0f,.94f,4.79f),P(-5.02f,.62f,4.615f)),
                ("cup-crack",P(-5.065f,.80f,4.71f),P(-5.02f,.655f,4.635f)),
                ("desk-context",P(-4.6f,1.95f,5.4f),P(-5.32f,.60f,4.60f))};
            var target=new RenderTexture(960,720,24);camera.targetTexture=target;
            foreach(var frame in frames)
            {
                go.transform.position=frame.Item2;go.transform.LookAt(frame.Item3);
                light.intensity=.45f*(frame.Item3-frame.Item2).sqrMagnitude;
                camera.Render();camera.Render();
                var prior=RenderTexture.active;RenderTexture.active=target;var pixels=new Texture2D(960,720,TextureFormat.RGB24,false);
                pixels.ReadPixels(new Rect(0,0,960,720),0,0);pixels.Apply();File.WriteAllBytes(folder+"/"+frame.Item1+".png",pixels.EncodeToPNG());
                RenderTexture.active=prior;Object.DestroyImmediate(pixels);
            }
            camera.targetTexture=null;Object.DestroyImmediate(target);Object.DestroyImmediate(go);
            Debug.Log("PROLOG_ORIGINAL_CUP_CAPTURES "+frames.Length);
        }
        private static void Bathroom()
        {
            // Concrete slabs keep collision; the single finish above owns rendering.
            foreach(var t in Objects().Where(t=>t.name.StartsWith("UpperSlab_")))
                foreach(var r in t.GetComponents<Renderer>())r.enabled=false;
            // The old ground-floor ceiling also has a top face at exactly 3.82m.
            // Keep its underside and collider; remove only that hidden duplicate face.
            foreach(var ceiling in Objects().Where(t=>t.name.StartsWith("GroundFloorCeiling_")))
            {
                var filter=ceiling.GetComponent<MeshFilter>();if(!filter||!filter.sharedMesh)continue;
                string path=Art+"Models/"+ceiling.name+"_NoTop.asset";
                var mesh=AssetDatabase.LoadAssetAtPath<Mesh>(path);
                if(!mesh)
                {
                    mesh=Object.Instantiate(filter.sharedMesh);mesh.name=ceiling.name+"_NoTop";
                    var vertices=mesh.vertices;
                    for(int sub=0;sub<mesh.subMeshCount;sub++)
                    {
                        var input=mesh.GetTriangles(sub);var output=new System.Collections.Generic.List<int>();
                        for(int i=0;i<input.Length;i+=3)
                        {
                            var a=filter.transform.TransformPoint(vertices[input[i]]);var b=filter.transform.TransformPoint(vertices[input[i+1]]);var c=filter.transform.TransformPoint(vertices[input[i+2]]);
                            if(Mathf.Abs(a.y-3.82f)<.004f&&Mathf.Abs(b.y-3.82f)<.004f&&Mathf.Abs(c.y-3.82f)<.004f)continue;
                            output.AddRange(new[]{input[i],input[i+1],input[i+2]});
                        }
                        mesh.SetTriangles(output,sub);
                    }
                    mesh.RecalculateBounds();AssetDatabase.CreateAsset(mesh,path);
                }
                filter.sharedMesh=mesh;
            }
            Remove("UpperFloor_South");var wood=OldMat("Upper_WoodFloor");
            Box("UpperFloor_ChildFinish",-3.2f,2.925f,3.8075f,new Vector3(1.6f,.025f,5.75f),wood,false);
            Box("UpperFloor_MasterEdgeFinish",.375f,2.925f,3.8075f,new Vector3(.35f,.025f,5.75f),wood,false);
            Box("UpperFloor_BathDoorFinish",-1.1f,5.475f,3.8075f,new Vector3(2.6f,.025f,.65f),wood,false);
            Remove("BathTub");
            var porcelain=Material("BathtubEnamel",new Color(.79f,.78f,.70f),"BathTubeP1.jpg",null,.32f);
            Model("BathTub","ClawfootBathtub",P(-1.1f,.87f,4.15f),new Vector3(1.8f,.64f,.88f),porcelain);
            var water=d.upper.bathWater;
            water.transform.SetParent(Find("Atmaja_Environment_Art").transform,true);water.transform.SetPositionAndRotation(P(-1.1f,.87f,4.34f),Quaternion.LookRotation(d.north));
            water.transform.localScale=new Vector3(1.55f,.018f,.67f);
            const string surfacePath=Art+"Models/BloodWaterSurface.asset";
            var surface=AssetDatabase.LoadAssetAtPath<Mesh>(surfacePath);
            if(!surface)
            {
                const int sides=48;var vertices=new Vector3[sides+1];var uv=new Vector2[sides+1];var triangles=new int[sides*3];
                uv[0]=new Vector2(.5f,.5f);
                for(int i=0;i<sides;i++)
                {
                    float angle=i*Mathf.PI*2/sides;vertices[i+1]=new Vector3(Mathf.Cos(angle)*.5f,0,Mathf.Sin(angle)*.5f);uv[i+1]=new Vector2(vertices[i+1].x+.5f,vertices[i+1].z+.5f);
                    triangles[i*3]=0;triangles[i*3+1]=(i+1)%sides+1;triangles[i*3+2]=i+1;
                }
                surface=new Mesh{name="OvalBloodWaterSurface"};surface.vertices=vertices;surface.uv=uv;surface.triangles=triangles;surface.RecalculateNormals();surface.RecalculateBounds();AssetDatabase.CreateAsset(surface,surfacePath);
            }
            water.GetComponent<MeshFilter>().sharedMesh=surface;
            var blood=Material("BathBloodWater",new Color(.22f,.004f,.008f),null,null,.86f);blood.SetFloat("_EnvironmentReflections",0);blood.EnableKeyword("_ENVIRONMENTREFLECTIONS_OFF");blood.EnableKeyword("_EMISSION");blood.SetColor("_EmissionColor",new Color(.055f,.001f,.002f));EditorUtility.SetDirty(blood);water.GetComponent<Renderer>().sharedMaterial=blood;
            d.upper.bathWater=water;
            // Fixed wall clue, at a child's reach; opening the door cannot move it.
            var scratches=d.upper.bathScratchesAR;
            scratches.transform.SetParent(Find("Atmaja_Environment_Art").transform,true);
            foreach(var child in scratches.transform.Cast<Transform>().Where(t=>t!=d.upper.scratchAim).ToArray())Object.DestroyImmediate(child.gameObject);
            foreach(var r in scratches.GetComponents<Renderer>())Object.DestroyImmediate(r);
            scratches.transform.localScale=Vector3.one;
            scratches.transform.SetPositionAndRotation(P(-2.275f,3.95f,4.9f),Quaternion.LookRotation(-d.east));
            var material=Material("WallClawMarks",new Color(.27f,.014f,.008f));
            material.shader=Shader.Find("LayarTerakhir/Wall Scratch Decal");material.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Art+"Textures/RealWallClawPhoto.jpg"));
            EditorUtility.SetDirty(material);
            for(int i=0;i<3;i++)
            {
                var quad=GameObject.CreatePrimitive(PrimitiveType.Quad);quad.name="WallClawDecal";Object.DestroyImmediate(quad.GetComponent<Collider>());
                quad.transform.SetParent(scratches.transform,false);quad.transform.localPosition=new Vector3((i-1)*.18f,(i%2)*.21f,0);
                quad.transform.localScale=new Vector3(.29f,.43f,1);quad.transform.localRotation=Quaternion.Euler(0,0,-17+i*13);
                quad.layer=AtmajaPhoneView.ARLayer;quad.GetComponent<Renderer>().sharedMaterial=material;
            }
            d.upper.bathScratchesOnWall=true;d.upper.scratchAim.SetParent(scratches.transform,false);d.upper.scratchAim.localPosition=Vector3.zero;
            var target=Find("BathScratchTarget");target.transform.SetPositionAndRotation(scratches.transform.position,scratches.transform.rotation);
            var item=target.GetComponent<AtmajaItem>();item.label="Periksa bekas cakaran di dinding";
        }
        private static void MasterRoom()
        {
            Remove("MasterEastWall","MasterBed","MasterMattress","MasterPillow","CorrLintelN");
            var plaster=OldMat("House_MAT_PlasterDamaged");
            Box("MasterEastWallExpanded",4.85f,1.68f,5.325f,new Vector3(.16f,3.01f,3.36f),plaster);
            Box("MasterBalconySeparation",3.625f,3.32f,5.325f,new Vector3(2.45f,3.01f,.16f),plaster);
            Box("MasterEntrySideWall",2.4f,4.3f,5.325f,new Vector3(.16f,3.01f,1.8f),plaster);
            var bedding=AssetDatabase.LoadAssetAtPath<Material>(Art+"Materials/VintageBedding.mat");
            Model("MasterBedSet","vintage_day_bed",P(3.55f,1.43f,4.405f),new Vector3(1.7f,1.13f,2.15f),bedding,270);
            Place(Find("MasterNightstand"),P(2.30f,.65f,4.13f));Place(Find("MasterAlarmClock"),P(2.3f,.65f,4.53f));
            Place(Find("MasterDresser"),P(4.43f,2.9f,4.45f));Place(Find("MasterCandle"),P(4.43f,2.9f,4.85f));
            var chair=Find("MasterRocker");chair.transform.rotation=Quaternion.LookRotation(-d.north);Place(chair,P(2.02f,3.8f,4.32f));
            Place(Find("MasterPortrait"),P(4.75f,1.45f,5.4f));
            Place(Find("MasterToolChest"),P(4.3f,.42f,4.05f));
            // The rolled carpet belongs over the trapdoor, away from the bed.
            Place(d.upper.carpetRoll,P(1f,1.4f,3.90f),new Vector3(.9f,.16f,.5f));
        }
        private static void Dining()
        {
            var foodRoot=new GameObject("PrologServingFood").transform;foodRoot.SetParent(root,false);
            var plate=Objects().First(t=>t.name=="MouldyPlate").gameObject;
            var bread=Objects().First(t=>t.name.StartsWith("RottenDinner_")).gameObject;
            var apple=Objects().First(t=>t.name=="RottenDinner_1").gameObject;
            foreach(var place in new[]{new Vector2(3.43f,3.65f),new Vector2(4.03f,4.85f),new Vector2(3.45f,5.7f)})
            {
                var dish=Object.Instantiate(plate,foodRoot);dish.name="ServingPlatter";KosVisualV2.Resize(dish,new Vector3(.45f,.045f,.38f));KosVisualV2.SetCenter(dish,P(place.x,place.y,1.48f));
                for(int i=0;i<6;i++)
                {
                    var food=Object.Instantiate(i%2==0?bread:apple,foodRoot);food.name="ServingFood";
                    float a=i*Mathf.PI/3;KosVisualV2.Resize(food,new Vector3(.12f,.08f,.11f));
                    KosVisualV2.SetCenter(food,P(place.x+Mathf.Cos(a)*.12f,place.y+Mathf.Sin(a)*.10f,1.54f+(i%2)*.025f));
                }
            }
            foreach(var t in Objects().Where(t=>t.name.StartsWith("RottenDinner_")).ToArray())
                for(int i=0;i<2;i++){var food=Object.Instantiate(i==0?bread:apple,foodRoot);food.name="DinnerSideFood";KosVisualV2.SetCenter(food,KosVisualV2.BoundsOf(t.gameObject).center+d.north*((i==0?-1:1)*.07f)-d.east*.09f);}
        }
        private static void Basement()
        {
            Remove("CellarSpineNorth","CellarSpineSouth","CellarReturnPartition");
            Place(Find("CellarCrateB"),P(10.9f,4.8f,-2.50f));
            Place(Find("CellarCrateD"),P(10.9f,6.4f,-2.50f));
            Place(Find("CellarCrateE"),P(10.9f,8f,-2.50f));
            // Low edge storage leaves broad, connected space to explore and evade.
            foreach(var name in new[]{"ArchiveRowA","ArchiveRowB"}){var go=Find(name);if(go)Place(go,P(name.EndsWith("A")?3.0f:4.4f,7.8f,-1.8f));}
            // The old radio cupboard occupied the newly visible vertical shaft.
            // Move the complete decorative group rather than cutting its furniture.
            var radio=Find("FamilyRadioCabinet");
            if(radio)
            {
                Vector3 shift=P(-1.6f,1.2f,.99f)-KosVisualV2.BoundsOf(radio).center;
                var temporary=GameObject.CreatePrimitive(PrimitiveType.Cube);var cube=temporary.GetComponent<MeshFilter>().sharedMesh;
                foreach(var t in Objects().Where(t=>new[]{"FamilyRadioCabinet","FamilyRadioBody","FamilyCabinetPanel","RadioSpeakerGrille"}.Contains(t.name)))
                {
                    t.position+=shift;
                    var filter=t.GetComponent<MeshFilter>();if(filter&&AssetDatabase.GetAssetPath(filter.sharedMesh).Contains("ShaftOpening_Family"))filter.sharedMesh=cube;
                }
                Object.DestroyImmediate(temporary);
            }
            CutClimbShaft();
            var steel=OldMat("Upper_SteelRust");
            Model("CellarAccessLadder","CellarAccessLadder",P(1.02f,1.25f,.51f),new Vector3(.66f,6.6f,.105f),steel,0,false);
            // The ladder occupies the existing narrow shaft, with its foot visible from the room.
            d.chase.cellarExitApproach.position=P(1.02f,1.95f,-2.79f);
            var points=new System.Collections.Generic.List<Transform>{d.upper.trapdoorTop};
            for(int i=0;i<=22;i++)
            {
                var point=new GameObject("CellarClimbPoint"+i).transform;point.SetParent(root,false);
                point.position=P(1.02f,1.58f,Mathf.Lerp(3.72f,-2.79f,i/22f));points.Add(point);
            }
            var foot=new GameObject("CellarClimbFoot").transform;foot.SetParent(root,false);foot.position=P(1.02f,1.95f,-2.79f);points.Add(foot);
            d.upper.cellarClimbPath=points.ToArray();
            var light=new GameObject("CellarLadderBeacon",typeof(Light));light.transform.SetParent(root,false);light.transform.position=P(1.02f,1.5f,-.8f);
            var l=light.GetComponent<Light>();l.type=LightType.Point;l.range=4;l.intensity=.8f;l.color=new Color(.84f,.68f,.40f);l.shadows=LightShadows.None;
        }
        private struct CutVertex
        {
            public Vector3 p,n;public Vector2 uv;public Vector4 tangent;
            public static CutVertex Lerp(CutVertex a,CutVertex b,float t)=>new CutVertex{p=Vector3.Lerp(a.p,b.p,t),n=Vector3.Lerp(a.n,b.n,t),uv=Vector2.Lerp(a.uv,b.uv,t),tangent=Vector4.Lerp(a.tangent,b.tangent,t)};
        }
        private static System.Collections.Generic.List<CutVertex> Clip(System.Collections.Generic.List<CutVertex> polygon,Func<CutVertex,float> side)
        {
            var output=new System.Collections.Generic.List<CutVertex>();if(polygon.Count==0)return output;
            var previous=polygon[polygon.Count-1];float before=side(previous);
            foreach(var current in polygon)
            {float after=side(current);if((before>=0)!=(after>=0))output.Add(CutVertex.Lerp(previous,current,before/(before-after)));if(after>=0)output.Add(current);previous=current;before=after;}
            return output;
        }
        private static void CutClimbShaft()
        {
            // Only render surfaces are cut: the game-owned ladder traversal already
            // disables the capsule. The trapdoor and its closing floor retain control.
            Vector3 center=P(1.025f,1.4f,0);float minX=center.x-.475f,maxX=center.x+.475f,minZ=center.z-.85f,maxZ=center.z+.85f;
            foreach(var filter in Objects().Select(t=>t.GetComponent<MeshFilter>()).Where(f=>f&&f.sharedMesh&&!f.transform.IsChildOf(root)).ToArray())
            {
                if(filter.name.StartsWith("Family")||filter.name.StartsWith("Radio"))continue;
                if(filter.transform.IsChildOf(d.upper.trapdoorLid.transform)||filter.transform.IsChildOf(d.upper.carpetRoll.transform))continue;
                var renderer=filter.GetComponent<Renderer>();
                if(!renderer||!renderer.enabled||renderer.bounds.max.y<-.6f||renderer.bounds.min.y>3.83f||renderer.bounds.min.x>maxX||renderer.bounds.max.x<minX||renderer.bounds.min.z>maxZ||renderer.bounds.max.z<minZ)continue;
                var source=filter.sharedMesh;var positions=source.vertices;var normals=source.normals;var uvs=source.uv;var tangents=source.tangents;
                var vertices=new System.Collections.Generic.List<CutVertex>();var submeshes=new System.Collections.Generic.List<int[]>();bool changed=false;
                for(int sub=0;sub<source.subMeshCount;sub++)
                {
                    var indices=new System.Collections.Generic.List<int>();var triangles=source.GetTriangles(sub);
                    for(int i=0;i<triangles.Length;i+=3)
                    {
                        var triangle=new System.Collections.Generic.List<CutVertex>();
                        for(int j=0;j<3;j++){int id=triangles[i+j];triangle.Add(new CutVertex{p=filter.transform.TransformPoint(positions[id]),n=normals.Length==positions.Length?normals[id]:Vector3.up,uv=uvs.Length==positions.Length?uvs[id]:Vector2.zero,tangent=tangents.Length==positions.Length?tangents[id]:new Vector4(1,0,0,1)});}
                        var a=triangle[0].p;var b=triangle[1].p;var c=triangle[2].p;
                        bool horizontal=Mathf.Abs(a.y-b.y)<.001f&&Mathf.Abs(a.y-c.y)<.001f&&a.y>-.6f&&a.y<3.83f;
                        var remaining=triangle;var fragments=new System.Collections.Generic.List<System.Collections.Generic.List<CutVertex>>();
                        var inside=horizontal?Clip(Clip(Clip(Clip(triangle,v=>v.p.x-minX),v=>maxX-v.p.x),v=>v.p.z-minZ),v=>maxZ-v.p.z):null;
                        if(inside!=null&&inside.Count>=3&&Vector3.Cross(inside[1].p-inside[0].p,inside[2].p-inside[0].p).sqrMagnitude>1e-12f)
                        {
                            changed=true;fragments.Add(Clip(remaining,v=>minX-v.p.x));remaining=Clip(remaining,v=>v.p.x-minX);
                            fragments.Add(Clip(remaining,v=>v.p.x-maxX));remaining=Clip(remaining,v=>maxX-v.p.x);
                            fragments.Add(Clip(remaining,v=>minZ-v.p.z));remaining=Clip(remaining,v=>v.p.z-minZ);
                            fragments.Add(Clip(remaining,v=>v.p.z-maxZ));
                        }
                        else fragments.Add(triangle);
                        foreach(var polygon in fragments)for(int j=1;j+1<polygon.Count;j++)foreach(var v in new[]{polygon[0],polygon[j],polygon[j+1]})
                        {indices.Add(vertices.Count);var local=v;local.p=filter.transform.InverseTransformPoint(v.p);vertices.Add(local);}
                    }
                    submeshes.Add(indices.ToArray());
                }
                if(!changed)continue;
                var mesh=new Mesh{name=source.name+"_ClimbShaft",indexFormat=UnityEngine.Rendering.IndexFormat.UInt32};
                mesh.SetVertices(vertices.Select(v=>v.p).ToList());mesh.SetNormals(vertices.Select(v=>v.n).ToList());mesh.SetUVs(0,vertices.Select(v=>v.uv).ToList());mesh.SetTangents(vertices.Select(v=>v.tangent).ToList());
                mesh.subMeshCount=submeshes.Count;for(int s=0;s<submeshes.Count;s++)mesh.SetTriangles(submeshes[s],s);mesh.RecalculateBounds();
                string path=Art+"Models/ShaftOpening_"+filter.name.Replace(' ','_')+".asset";var saved=AssetDatabase.LoadAssetAtPath<Mesh>(path);
                if(saved){EditorUtility.CopySerialized(mesh,saved);Object.DestroyImmediate(mesh);mesh=saved;}else AssetDatabase.CreateAsset(mesh,path);
                filter.sharedMesh=mesh;if(PrefabUtility.IsPartOfPrefabInstance(filter))PrefabUtility.RecordPrefabInstancePropertyModifications(filter);
            }
        }
    }
}
#endif
