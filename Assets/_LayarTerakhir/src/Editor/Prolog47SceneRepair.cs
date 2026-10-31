#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Deliberate patches to the furnished scenes. Never regenerates the approved Kos.
    public static class Prolog47SceneRepair
    {
        private const string Models = "Assets/_LayarTerakhir/assets/Art/Original/Models/Prolog47/";
        private const string Texs = "Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47/";
        private const string Mats = "Assets/_LayarTerakhir/assets/Art/Generated/Materials/Prolog47/";

        private static GameObject Find(string name) =>
            Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault(t => t.name == name)?.gameObject;

        private static GameObject[] FindAll(string name) =>
            Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(t => t && t.name == name).Select(t => t.gameObject).ToArray();

        private static void Delete(string name)
        {
            foreach (var go in FindAll(name))
                if (go) Object.DestroyImmediate(go);
        }

        private static Material Old(string name) =>
            AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/" + name + ".mat");

        private static Material Material(string name)
        {
            Directory.CreateDirectory(Mats);
            string source = null;
            string customTex = null;
            Color color = Color.white;
            float smooth = .22f, metal = 0f;
            switch (name)
            {
                case "Repair_BedWood":
                    source = "Upper_PineTable"; color = new Color(.46f, .36f, .26f); smooth = .30f; break;
                case "Repair_Linen":
                case "Repair_Mattress":
                    source = "Upper_BedFabric"; color = new Color(.78f, .75f, .66f); smooth = .06f; break;
                case "Repair_Blanket":
                    source = "Upper_BedFabric"; color = new Color(.23f, .32f, .35f); smooth = .08f; break;
                case "Repair_BookCloth":
                    source = "Upper_BedFabric"; color = new Color(.22f, .18f, .16f); smooth = .06f; break;
                case "Repair_Paper":
                    customTex = Texs + "AgedNotebookPaperDiffuse.png"; color = Color.white; smooth = .05f; break;
                case "Repair_Ceramic":
                    color = new Color(.86f, .84f, .78f); smooth = .76f; break;
                case "Repair_CeramicEdge":
                    color = new Color(.22f, .028f, .022f); smooth = .32f; break;
                case "Repair_CupCeramic":
                    color = new Color(.86f, .84f, .77f); smooth = .54f; break;
                case "Repair_CupCrack":
                    color = new Color(.14f, .075f, .045f); smooth = .10f; break;
                case "Repair_SandalFootbed":
                    customTex = Texs + "SwallowFootbedDiffuse.png"; color = Color.white; smooth = .12f; break;
                case "Repair_SandalRubber":
                    color = new Color(.11f, .24f, .36f); smooth = .14f; break;
                case "Repair_SandalStrap":
                    color = new Color(.14f, .35f, .60f); smooth = .22f; break;
                case "Repair_SandalStrapGreen":
                    color = new Color(.13f, .42f, .26f); smooth = .22f; break;
                case "Repair_SandalStrapRed":
                    color = new Color(.52f, .14f, .14f); smooth = .22f; break;
                case "Repair_Iron":
                    color = new Color(.26f, .28f, .28f); metal = .72f; smooth = .46f; break;
                case "Repair_Chrome":
                    color = new Color(.74f, .76f, .78f); metal = .88f; smooth = .82f; break;
                case "Repair_Brass":
                    color = new Color(.48f, .40f, .24f); metal = .78f; smooth = .56f; break;
                case "Repair_DoorPaint":
                    source = "Upper_PineTable"; color = new Color(.50f, .42f, .33f); smooth = .28f; break;
                case "Repair_DoorInset":
                    source = "Upper_PineTable"; color = new Color(.42f, .35f, .27f); smooth = .26f; break;
                case "Repair_NumberBacking":
                    color = new Color(.11f, .10f, .09f); metal = .15f; smooth = .35f; break;
                case "Repair_ApplianceEnamel":
                    color = new Color(.72f, .75f, .77f); metal = .22f; smooth = .60f; break;
                case "Repair_BathFloorTile":
                    source = "concrete_floor_damaged_01"; color = new Color(.66f, .69f, .68f); smooth = .46f; break;
            }
            string path = Mats + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!mat)
            {
                mat = source != null && Old(source)
                    ? new Material(Old(source))
                    : new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, path);
            }
            else if (source != null && Old(source))
            {
                var srcMat = Old(source);
                if (srcMat.GetTexture("_BaseMap")) mat.SetTexture("_BaseMap", srcMat.GetTexture("_BaseMap"));
                if (srcMat.GetTexture("_BumpMap")) { mat.SetTexture("_BumpMap", srcMat.GetTexture("_BumpMap")); mat.EnableKeyword("_NORMALMAP"); }
            }
            if (customTex != null)
            {
                var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(customTex);
                if (tex) mat.SetTexture("_BaseMap", tex);
            }
            mat.SetColor("_BaseColor", color);
            mat.SetFloat("_Smoothness", smooth);
            mat.SetFloat("_Metallic", metal);
            if (name == "Repair_CupCeramic")
            {
                mat.SetColor("_EmissionColor", new Color(.022f, .020f, .018f));
                mat.EnableKeyword("_EMISSION");
            }
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static GameObject Model(string model, string name, Transform parent, Vector3 position, Quaternion rotation)
        {
            var root = new GameObject(name);
            root.transform.SetParent(parent, true);
            root.transform.SetPositionAndRotation(position, rotation);
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(Models + model + ".fbx");
            if (!asset) throw new InvalidOperationException("Missing authored model " + model);
            var child = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            child.transform.SetParent(root.transform, false);
            foreach (var renderer in child.GetComponentsInChildren<Renderer>())
            {
                renderer.sharedMaterials = renderer.sharedMaterials.Select(m =>
                {
                    string mName = m ? m.name : "Repair_Iron";
                    if (model == "BloodBathTub" && mName == "Repair_Iron") return Material("Repair_Chrome");
                    return Material(mName);
                }).ToArray();
            }
            if (name.EndsWith("BedSet"))
                foreach (var part in child.GetComponentsInChildren<Transform>())
                    if (part.name == "Quilted mattress")
                        part.name = name.StartsWith("Child") ? "ChildMattress" : "MasterMattress";
            return root;
        }

        private static GameObject ProjectAsset(string fbxPath, string name, Transform parent, Vector3 position, Quaternion rotation, Vector3 targetSize, Material overrideMat = null)
        {
            Delete(name);
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
            if (!asset) return null;
            var root = new GameObject(name);
            root.transform.SetParent(parent, true);
            root.transform.SetPositionAndRotation(position, rotation);
            var child = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            child.transform.SetParent(root.transform, false);
            if (overrideMat)
            {
                foreach (var r in child.GetComponentsInChildren<Renderer>())
                    r.sharedMaterials = Enumerable.Repeat(overrideMat, r.sharedMaterials.Length).ToArray();
            }
            KosVisualV2.Resize(root, targetSize);
            KosVisualV2.SetCenter(root, position + Vector3.up * targetSize.y * .5f);
            FitCollider(root);
            return root;
        }

        private static bool HasVisibleGeometry(GameObject go) =>
            go && go.activeInHierarchy && go.GetComponentsInChildren<Renderer>().Any(r => r && r.enabled && !(r is LineRenderer));

        private static Bounds SafeBounds(GameObject go)
        {
            var activeRs = go.GetComponentsInChildren<Renderer>().Where(r => r && r.enabled && !(r is LineRenderer)).ToArray();
            if (activeRs.Length > 0)
            {
                var b = activeRs[0].bounds;
                foreach (var r in activeRs.Skip(1)) b.Encapsulate(r.bounds);
                return b;
            }
            var rs = go.GetComponentsInChildren<Renderer>(true).Where(r => r && !(r is LineRenderer)).ToArray();
            if (rs.Length > 0)
            {
                var b = rs[0].bounds;
                foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds);
                return b;
            }
            var cols = go.GetComponentsInChildren<Collider>(true);
            if (cols.Length > 0)
            {
                var b = cols[0].bounds;
                foreach (var c in cols.Skip(1)) b.Encapsulate(c.bounds);
                return b;
            }
            return new Bounds(go.transform.position, new Vector3(.9f, 2.1f, .06f));
        }

        private static void Ground(GameObject root, float y) =>
            root.transform.position += Vector3.up * (y - SafeBounds(root).min.y);

        private static void FitCollider(GameObject root)
        {
            var col = root.GetComponent<BoxCollider>();
            if (!col) col = root.AddComponent<BoxCollider>();
            KosVisualV2.FitBox(col, SafeBounds(root));
        }

        private static void RotateCentred(GameObject root, float yaw)
        {
            if (!root || !HasVisibleGeometry(root)) return;
            var centre = SafeBounds(root).center;
            root.transform.rotation = Quaternion.Euler(0, yaw, 0);
            root.transform.position += centre - SafeBounds(root).center;
        }

        // Replace default ZTest=Always Font Material on 3D TextMeshes with depth-tested LayarTerakhir/WorldText
        // and attach AtmajaWorldText so dynamic font atlas rebuilds NEVER bleed X-ray through floors or walls!
        private static void FixDepthTestedTextMeshes()
        {
            Directory.CreateDirectory(Mats);
            var shader = Shader.Find("LayarTerakhir/WorldText") ?? Shader.Find("GUI/3D Text Shader");
            Material templateMat = null;
            foreach (var tm in Object.FindObjectsByType<TextMesh>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (!tm) continue;
                var mr = tm.GetComponent<MeshRenderer>();
                var fnt = tm.font ? tm.font : Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
                if (!mr || !fnt || !fnt.material) continue;
                string safeName = fnt.name.Replace(' ', '_');
                string mPath = Mats + "DepthText_" + safeName + ".mat";
                var dMat = AssetDatabase.LoadAssetAtPath<Material>(mPath);
                if (!dMat)
                {
                    dMat = shader ? new Material(shader) : new Material(fnt.material);
                    AssetDatabase.CreateAsset(dMat, mPath);
                }
                if (shader && dMat.shader != shader) dMat.shader = shader;
                dMat.mainTexture = fnt.material.mainTexture;
                dMat.color = Color.white;
                if (dMat.HasProperty("_Color")) dMat.SetColor("_Color", Color.white);
                if (dMat.HasProperty("_ZTest")) dMat.SetInt("_ZTest", (int)CompareFunction.LessEqual);
                if (dMat.HasProperty("_ZWrite")) dMat.SetInt("_ZWrite", 0);
                EditorUtility.SetDirty(dMat);
                mr.sharedMaterial = dMat;
                mr.shadowCastingMode = ShadowCastingMode.Off;
                mr.receiveShadows = false;
                if (!templateMat) templateMat = dMat;
            }
            if (templateMat)
            {
                var wt = Object.FindFirstObjectByType<AtmajaWorldText>(FindObjectsInactive.Include);
                if (!wt)
                {
                    var host = Find("Kos_Building_Corridors") ?? Find("Atmaja_Environment_Art") ?? Find("KosRoomRoot");
                    if (!host) host = new GameObject("PrologWorldTextDepthGuard");
                    wt = host.GetComponent<AtmajaWorldText>() ?? host.AddComponent<AtmajaWorldText>();
                }
                wt.materialTemplate = templateMat;
                EditorUtility.SetDirty(wt);
            }
        }

        private static void RefreshHudNotebookTheme()
        {
            var hud = Object.FindFirstObjectByType<HudController>(FindObjectsInactive.Include);
            if (!hud || !hud.documentPanel) return;
            var img = hud.documentPanel.GetComponent<Image>();
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_LayarTerakhir/Resources/PrologNotebookSheet.png");
            if (img && sprite)
            {
                img.sprite = sprite;
                img.type = Image.Type.Simple;
                img.color = Color.white;
                EditorUtility.SetDirty(img);
            }
        }

        private static void Save()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
        }

        public static void ApplyCompleteRevision()
        {
            ConfigureImports();
            ApplySimple();
            ApplyKosFacilities();
            ApplyReviewCorrections();
            ApplyFurniture();
            ApplyArchitecture();
            ApplyDetails();
            ApplyFinishProps();
            ApplyLadder();
            ApplyPrologSkin();
            Debug.Log("PROLOG47_COMPLETE_REVISION_APPLIED all 3 Prolog scenes updated and saved");
        }

        public static void ApplyStageOne()
        {
            ConfigureImports(); ApplySimple(); ApplyFurniture();
        }

        public static void ApplyRemaining()
        {
            ConfigureImports(); ApplyFurniture(); ApplyKosFacilities(); ApplyArchitecture(); ApplyDetails(); ApplyLadder(); ApplyFinishProps();
        }

        private static void ConfigureImports()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            const string nbPath = "Assets/_LayarTerakhir/Resources/PrologNotebookSheet.png";
            if (AssetImporter.GetAtPath(nbPath) is TextureImporter nbImp)
            {
                nbImp.textureType = TextureImporterType.Sprite;
                nbImp.spriteImportMode = SpriteImportMode.Single;
                nbImp.sRGBTexture = true;
                nbImp.alphaIsTransparency = true;
                nbImp.mipmapEnabled = false;
                nbImp.maxTextureSize = 1024;
                nbImp.SaveAndReimport();
            }
            foreach (string root in new[] { "Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities", Texs })
            {
                if (!Directory.Exists(root)) continue;
                foreach (string file in Directory.GetFiles(root, "*", SearchOption.AllDirectories).Where(p => p.EndsWith(".png") || p.EndsWith(".jpg")))
                {
                    string path = file.Replace('\\', '/');
                    var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (!importer) continue;
                    bool normal = path.Contains("_Normal.") || path.Contains("_nor_gl_");
                    bool mask = path.Contains("_Mask.") || path.Contains("_mask.");
                    importer.textureType = normal ? TextureImporterType.NormalMap : TextureImporterType.Default;
                    importer.sRGBTexture = !normal && !mask;
                    importer.maxTextureSize = 1024;
                    importer.mipmapEnabled = true;
                    importer.anisoLevel = 2;
                    if (path.EndsWith("HandprintAndScratches.png") || path.Contains("Restroom"))
                    {
                        importer.alphaIsTransparency = true;
                        importer.wrapMode = TextureWrapMode.Clamp;
                    }
                    importer.SaveAndReimport();
                }
            }
        }

        public static void ApplySimple()
        {
            // 1. Prolog_KamarKos.unity
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            RotateCentred(Find("KosDeskFan"), 162);
            // Upgrade Raka's bedroom sandals to Swallow-style IndonesianFlipFlops without dotted sole ring
            var roomSandals = Find("KosSandals");
            if (roomSandals)
            {
                var b = SafeBounds(roomSandals);
                var p = roomSandals.transform.parent;
                var rot = roomSandals.transform.rotation;
                Object.DestroyImmediate(roomSandals);
                var pair = Model("IndonesianFlipFlops", "KosSandals", p, b.center, rot);
                KosVisualV2.Resize(pair, new Vector3(.24f, .052f, .275f));
                KosVisualV2.SetCenter(pair, b.center);
                Ground(pair, b.min.y);
            }
            // Clean shredded floating wire/spring submeshes on anyLodDeskLamp inside KamarKos
            CleanShreddedLampWires();
            FixDepthTestedTextMeshes();
            RefreshHudNotebookTheme();
            Save();

            // 2. Prolog_KoridorKos.unity
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            Delete("FloorTwoWayfinding");
            Delete("GroundWayfinding");
            var yard = Find("KosYard");
            if (yard)
            {
                yard.transform.localPosition = new Vector3(0, -.08f, -4.1f);
                yard.transform.localScale = new Vector3(13.6f, .16f, 8.6f);
            }
            FixDepthTestedTextMeshes();
            RefreshHudNotebookTheme();
            Save();

            // 3. Prolog_RumahAtmaja.unity
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var upper = Object.FindFirstObjectByType<AtmajaUpperDirector>();
            // Bug 25: Delete PortraitDosa2/3 AND ScratchedFamilyPhotograph2/3 so ONLY ONE family photo remains!
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(t => t && (t.name.StartsWith("PortraitDosa") || t.name.StartsWith("ScratchedFamilyPhotograph"))).ToArray())
            {
                if (!t) continue;
                if (t.name.StartsWith("PortraitDosa2") || t.name.StartsWith("PortraitDosa3") ||
                    t.name == "ScratchedFamilyPhotograph2" || t.name == "ScratchedFamilyPhotograph3" ||
                    t.name.Contains("_Plaque") || t.name.Contains("_BloodAR") || t.name.Contains("_Gouge"))
                {
                    Object.DestroyImmediate(t.gameObject);
                }
            }
            upper.portraitBloodAR = Array.Empty<GameObject>();
            var d = upper.director;
            // Center & fit ScratchedFamilyPhotograph1 strictly inside PortraitDosa1 wooden frame using Atmaja local coordinates!
            var frame1 = Find("PortraitDosa1");
            var photo1 = Find("ScratchedFamilyPhotograph1");
            var envRoot = Find("Atmaja_Environment_Art");
            if (frame1)
            {
                frame1.transform.rotation = Quaternion.LookRotation(-d.north);
                KosVisualV2.Resize(frame1, new Vector3(.62f, .78f, .06f));
                KosVisualV2.SetCenter(frame1, d.P(-2.05f, 5.29f, 5.32f));
                // Darken blank inner canvas of PortraitDosa1 so no bright white border pokes around the photograph
                foreach (var r in frame1.GetComponentsInChildren<Renderer>(true))
                {
                    var mats = r.sharedMaterials;
                    for (int mi = 0; mi < mats.Length; mi++)
                        if (mats[mi] && mats[mi].name.Contains("Canvas")) mats[mi] = Material("Repair_BookCloth");
                    r.sharedMaterials = mats;
                }
            }
            if (photo1)
            {
                if (envRoot) photo1.transform.SetParent(envRoot.transform, true);
                photo1.transform.SetPositionAndRotation(d.P(-2.05f, 5.326f, 5.32f), Quaternion.LookRotation(-d.north));
                photo1.transform.localScale = new Vector3(.48f, .62f, 1f);
            }
            foreach (var lintel in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t => t.name == "CorrLintelN"))
            {
                float localX = d.LocalPosition(lintel.position).x;
                float x = -Mathf.Abs(localX), bottom = x < -3 ? 5.92f : 5.82f;
                lintel.position = d.P(x, 5.2f, (bottom + 6.83f) / 2);
                lintel.localScale = new Vector3(x < -3 ? 1.1f : 1.0f, 6.83f - bottom, .18f);
            }
            CleanShreddedLampWires();
            RotateCentred(Find("ChildDeskLamp"), 180);
            FixDepthTestedTextMeshes();
            RefreshHudNotebookTheme();
            Save();
            Debug.Log("PROLOG47_SIMPLE_APPLIED no furnished scene regenerated");
        }

        private static void CleanShreddedLampWires()
        {
            foreach (var mf in Object.FindObjectsByType<MeshFilter>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToArray())
            {
                if (!mf || !mf.sharedMesh) continue;
                string mn = mf.sharedMesh.name.ToLowerInvariant();
                string on = mf.name.ToLowerInvariant();
                if ((mn.Contains("loddesklamp") || on.Contains("wire") || on.Contains("spring")) &&
                    (mn.Contains("wire") || mn.Contains("spring")))
                {
                    var r = mf.GetComponent<Renderer>();
                    if (r) r.enabled = false;
                }
            }
        }

        public static void ApplyFurniture()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var environment = Find("Atmaja_Environment_Art").transform;

            // Bug 31 & 32: Delete the backwards clipping ChildShelf + ChildBooks, and place textured ChildBedSet cleanly in that corner!
            foreach (string name in new[] { "ChildBed", "ChildMattress", "ChildPillow", "ChildBlanket", "ChildWardrobe", "ChildToyBlocks", "ChildToyBall", "ChildBedSet", "ChildShelf", "ChildBooks", "BimaToyTrainCar" })
                Delete(name);
            var bed = Model("SingleBedSet", "ChildBedSet", environment, d.P(-3.22f, 1.24f, 3.83f), Quaternion.LookRotation(-d.north));
            Ground(bed, 3.83f);
            FitCollider(bed);

            // Bug 20 & 29: OpenDiary with real AgedNotebookPaperDiffuse.png grounded flush on ChildDesk
            var childDesk = Find("ChildDesk");
            float deskTopY = childDesk ? SafeBounds(childDesk).max.y + .001f : 4.553f;
            foreach (string name in new[] { "ChildDiaryBook", "ChildNotebook" }) Delete(name);
            var diary = Model("OpenDiary", "ChildDiaryBook", environment, d.P(-5.25f, .60f, deskTopY), Quaternion.LookRotation(d.north));
            Ground(diary, deskTopY);
            FitCollider(diary);
            var item = diary.AddComponent<AtmajaItem>();
            item.director = d; item.action = AtmajaAction.Diary; item.label = "Baca buku harian Bima";
            Ink(diary.transform, "Buku harian Bima\n\nIbu sekarang sering\nngomong sendiri di\nruang jahit bawah\ntanah. Tangannya\nselalu megang\ngunting taman.", -.076f);
            Ink(diary.transform, "Tadi malam Ibu\nbikinin aku susu.\nBaunya aneh, kayak\nkapur barus campur\nkarat. Kata Ibu, kami\nnggak bakal sakit-\nsakit lagi.", .076f);

            // Bug 33: CrackedMilkCup with unmistakable front & back chipped V-notch and dark fracture line flush on ChildDesk
            Delete("ChildMilkCup");
            var cup = Model("CrackedMilkCup", "ChildMilkCup", environment, d.P(-4.92f, .63f, deskTopY), Quaternion.LookRotation(-d.north));
            Ground(cup, deskTopY);
            FitCollider(cup);
            item = cup.AddComponent<AtmajaItem>();
            item.director = d; item.action = AtmajaAction.CupPhoto; item.label = "Periksa cangkir susu retak";
            d.upper.childCup = cup;
            d.upper.cupAim.position = SafeBounds(cup).center;

            // Bug 36: Smooth curved porcelain BloodBathTub with chrome dual-handle mixer tap + slim draped curtain
            Delete("BathTub");
            var tub = Model("BloodBathTub", "BathTub", environment, d.P(-1.1f, .93f, 3.83f), Quaternion.LookRotation(d.north));
            Ground(tub, 3.83f);
            var hull = tub.AddComponent<BoxCollider>();
            hull.center = new Vector3(0, .27f, 0);
            hull.size = new Vector3(1.74f, .54f, .78f);

            var water = d.upper.bathWater;
            water.transform.SetPositionAndRotation(d.P(-1.1f, .93f, 4.26f), Quaternion.LookRotation(d.north));
            water.transform.localScale = new Vector3(1.48f, .008f, .58f);
            var waterMat = AssetDatabase.LoadAssetAtPath<Material>(Mats + "BloodWater.mat");
            if (!waterMat)
            {
                waterMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(waterMat, Mats + "BloodWater.mat");
            }
            waterMat.SetColor("_BaseColor", new Color(.22f, .012f, .010f));
            waterMat.SetFloat("_Smoothness", .93f);
            waterMat.SetFloat("_Metallic", .05f);
            water.GetComponent<Renderer>().sharedMaterial = waterMat;
            EditorUtility.SetDirty(waterMat);

            // Slim BathCurtainFold so it looks like thin fabric instead of a thick concrete block
            var curtainFold = Find("BathCurtainFold");
            if (curtainFold)
            {
                var ls = curtainFold.transform.localScale;
                curtainFold.transform.localScale = new Vector3(Mathf.Min(ls.x, .16f), ls.y, Mathf.Min(ls.z, .04f));
                var cr = curtainFold.GetComponent<Renderer>();
                if (cr) cr.sharedMaterial = Material("Repair_Linen");
            }

            var survey = Find("DamarSurveyInsideDining");
            if (survey)
            {
                Delete("DamarSurveyPaper");
                var col = survey.GetComponent<Collider>();
                if (survey.GetComponent<Renderer>()) Object.DestroyImmediate(survey.GetComponent<Renderer>());
                if (survey.GetComponent<MeshFilter>()) Object.DestroyImmediate(survey.GetComponent<MeshFilter>());
                Vector3 position = survey.transform.position;
                survey.transform.localScale = Vector3.one;
                var paper = Model("OpenDiary", "DamarSurveyPaper", survey.transform, position, Quaternion.LookRotation(d.north));
                Ink(paper.transform, "CATATAN DAMAR\n\nRumah Atmaja.\nGerbang depan\nmacet. Cari jalan\nmelalui dapur.\n\nJangan datang\nsendirian.", -.076f);
                Ink(paper.transform, "Survei lokasi\n\nKeluarga Atmaja\nmeninggalkan rumah\nini sejak tragedi\ntahun 1998.\n\nPeriksa catatan\ndi dalam rumah.", .076f);
                if (col is BoxCollider box) KosVisualV2.FitBox(box, SafeBounds(paper));
            }
            CleanShreddedLampWires();
            Save();
            Debug.Log("PROLOG47_FURNITURE_APPLIED authored cloth, paper, ceramic and hollow tub");
        }

        private static void UpgradeDoorToModernPanel(HingedDoor door, Transform root)
        {
            if (!door) return;
            var leaf = door.gameObject;
            foreach (var t in leaf.GetComponentsInChildren<Transform>(true).Where(t =>
                t && t != leaf.transform && (t.name == "ModernDoorVisual" || t.name.Contains("PanelInset") || t.name.Contains("RecessedPanel") ||
                t.name.Contains("Pictogram") || t.name.Contains("LogoPlaque"))).ToArray())
            {
                Object.DestroyImmediate(t.gameObject);
            }

            var bounds = SafeBounds(leaf);
            if (bounds.size.sqrMagnitude <= .01f) return;
            Vector3 localCenter = root.InverseTransformPoint(bounds.center);
            Vector3 localMin = root.InverseTransformPoint(bounds.min);
            Vector3 localMax = root.InverseTransformPoint(bounds.max);
            Vector3 localSize = new Vector3(Mathf.Abs(localMax.x - localMin.x), Mathf.Abs(localMax.y - localMin.y), Mathf.Abs(localMax.z - localMin.z));

            bool alongZ = localSize.z >= localSize.x || leaf.name.StartsWith("KosUpperDoor_") || leaf.name.StartsWith("KosGroundDoor_") || leaf.name.Contains("Bathroom");
            float rawSpan = alongZ ? localSize.z : localSize.x;
            float width = leaf.name.Contains("Bathroom") ? .92f : Mathf.Clamp(rawSpan > .2f ? rawSpan : Mathf.Max(localSize.x, localSize.z), .72f, 1.85f);
            float height = localSize.y > 1.2f ? localSize.y : 2.12f;
            Vector3 center = root.TransformPoint(localCenter);

            if (leaf.GetComponent<Renderer>()) Object.DestroyImmediate(leaf.GetComponent<Renderer>());
            if (leaf.GetComponent<MeshFilter>()) Object.DestroyImmediate(leaf.GetComponent<MeshFilter>());
            leaf.transform.localScale = Vector3.one;

            var col = leaf.GetComponent<BoxCollider>();
            if (!col) col = leaf.AddComponent<BoxCollider>();
            col.center = leaf.transform.InverseTransformPoint(center);
            col.size = alongZ ? new Vector3(.06f, height, width) : new Vector3(width, height, .06f);

            Quaternion rot = root.rotation * (alongZ ? Quaternion.Euler(0, 90, 0) : Quaternion.identity);
            var visual = Model("KosPanelDoor", "ModernDoorVisual", leaf.transform, center - Vector3.up * (height * .5f), rot);
            KosVisualV2.Resize(visual, alongZ ? new Vector3(.055f, height, width) : new Vector3(width, height, .055f));
            KosVisualV2.SetCenter(visual, center);

            foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t => t && (t.name == leaf.name + "Handle" || (leaf.name.Contains("MainEntrance") && t.name.StartsWith("MainDoorHandle")))).ToArray())
                if (t) Object.DestroyImmediate(t.gameObject);
        }

        private static void AttachRestroomPlaque(GameObject doorObj, string plaqueName, string texFile)
        {
            if (!doorObj) return;
            var hd = doorObj.GetComponent<HingedDoor>();
            if (hd && hd.hinge) hd.SetOpenImmediate(false);
            foreach (var old in doorObj.GetComponentsInChildren<Transform>(true).Where(t =>
                t && t != doorObj.transform && (t.name == plaqueName || t.name.Contains("LogoPlaque") || t.name.Contains("Pictogram"))).ToArray())
            {
                Object.DestroyImmediate(old.gameObject);
            }
            var q = GameObject.CreatePrimitive(PrimitiveType.Quad);
            q.name = plaqueName;
            q.transform.SetParent(doorObj.transform, false);
            Object.DestroyImmediate(q.GetComponent<Collider>());
            q.transform.localPosition = new Vector3(-.036f, .42f, 0f);
            q.transform.localRotation = Quaternion.Euler(0, 90, 0);
            q.transform.localScale = new Vector3(.23f, .29f, 1f);

            string mPath = Mats + plaqueName + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(mPath);
            if (!mat)
            {
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, mPath);
            }
            mat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Texs + texFile));
            mat.SetColor("_BaseColor", Color.white);
            mat.SetFloat("_Smoothness", .34f);
            mat.SetFloat("_Metallic", .02f);
            mat.SetFloat("_Cull", 0f);
            EditorUtility.SetDirty(mat);
            q.GetComponent<Renderer>().sharedMaterial = mat;
        }

        public static void ApplyKosFacilities()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var root = Find("Kos_Building_Corridors").transform;

            foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t =>
                t && (t.name.StartsWith("TenantMotor_") || t.name == "KosStoneWorktop" || t.name == "KosCounterCabinet" ||
                      t.name.StartsWith("KosKitchenCabinet_") || t.name == "KosLpgTank" || t.name == "KosBrassPan" ||
                      t.name.StartsWith("KosDiningBowl_") || t.name == "KosStreetThresholdCurb" || t.name == "OpenKitchenDiningHeader")).ToArray())
            {
                if (t) Object.DestroyImmediate(t.gameObject);
            }

            // Bug 7: Delete floating '3' number, floating handle, duplicate frames/signs, and corridor divider pier between Kitchen & Dining!
            foreach (string stray in new[]
            {
                "KosGroundDoor_-1.5_2Number", "KosGroundDoor_-1.5_2Handle", "KosGroundDoor_-1.5_2Frame", "KosGroundDoor_-1.5_2Lintel",
                "KosGroundDoor_-1.5_2", "SharedDiningDoorHinge", "DAPUR BERSAMAHinge", "SharedDiningSign", "DAPUR BERSAMASign",
                "RestroomMaleLogoPlaque", "RestroomFemaleLogoPlaque", "BathroomPRIAPictogram", "BathroomWANITAPictogram"
            })
            {
                Delete(stray);
            }

            // Remove corridor divider wall pier between Kitchen (z ~ 3..6) and Dining (z ~ 6..9) on west corridor wall (x ~ -1.5)
            foreach (var t in root.GetComponentsInChildren<Transform>(true).ToArray())
            {
                if (!t || t == root) continue;
                Vector3 lp = root.InverseTransformPoint(t.position);
                // Partition wall at z ~ 6 separating kitchen & dining inside x < -1.5
                if ((t.name == "RoomPartition" || t.name == "DAPUR BERSAMASide") && Mathf.Abs(lp.z - 6f) < .15f && lp.x < -1.5f && lp.y < 3.1f)
                {
                    Object.DestroyImmediate(t.gameObject);
                    continue;
                }
                // Corridor pier splitting the kitchen doorway and dining doorway along x ~ -1.5, z in [4.2 .. 7.8]
                if ((t.name == "HallWallPier" || t.name == "DAPUR BERSAMAFrontLow" || t.name == "DAPUR BERSAMAFrontHigh" || t.name == "DAPUR BERSAMALintel" || t.name == "HallDoorHeader") &&
                    Mathf.Abs(lp.x - (-1.5f)) < .35f && lp.z > 3.6f && lp.z < 8.4f && lp.y < 3.1f)
                {
                    Object.DestroyImmediate(t.gameObject);
                }
            }
            // Add one clean continuous upper header beam spanning the merged Kitchen + Dining open portal
            var openHeader = GameObject.CreatePrimitive(PrimitiveType.Cube);
            openHeader.name = "OpenKitchenDiningHeader";
            openHeader.transform.SetParent(root, false);
            openHeader.transform.localPosition = new Vector3(-1.5f, 2.86f, 6.0f);
            openHeader.transform.localScale = new Vector3(.18f, .72f, 5.8f);
            openHeader.GetComponent<Renderer>().sharedMaterial = Old("House_MAT_PlasterDamaged") ?? Material("Repair_Ceramic");

            // Bug 8: Snap BathroomPRIAHinge/Door (z in [2.84..3.76]) and BathroomWANITAHinge/Door (z in [4.74..5.66]) squarely into BathroomFrontPartition (x = 2.82)!
            foreach (var spec in new[] { ("BathroomPRIADoor", "BathroomPRIAHinge", 2.84f), ("BathroomWANITADoor", "BathroomWANITAHinge", 4.74f) })
            {
                var bDoor = Find(spec.Item1);
                var bHinge = Find(spec.Item2);
                if (bDoor && bHinge)
                {
                    bHinge.transform.SetParent(root, false);
                    bHinge.transform.localScale = Vector3.one;
                    bHinge.transform.localRotation = Quaternion.identity;
                    bHinge.transform.localPosition = new Vector3(2.82f, 1.06f, spec.Item3);
                    bDoor.transform.SetParent(bHinge.transform, false);
                    bDoor.transform.localScale = Vector3.one;
                    bDoor.transform.localRotation = Quaternion.identity;
                    bDoor.transform.localPosition = new Vector3(0f, 0f, .46f);
                    var hd = bDoor.GetComponent<HingedDoor>();
                    if (hd) { hd.hinge = bHinge.transform; hd.SetOpenImmediate(false); }
                    var bc = bDoor.GetComponent<BoxCollider>() ?? bDoor.AddComponent<BoxCollider>();
                    bc.center = Vector3.zero;
                    bc.size = new Vector3(.06f, 2.12f, .92f);
                }
            }

            // Bug 3: Close ALL Kos doors flush in their frames before upgrading so saved-open doors never end up misaligned!
            foreach (var door in root.GetComponentsInChildren<HingedDoor>(true))
            {
                if (!door) continue;
                if (!door.hinge) door.hinge = door.transform;
                door.SetOpenImmediate(false);
                if (door.name.StartsWith("KosUpperDoor_") || door.name.StartsWith("KosGroundDoor_") ||
                    door.name.Contains("Bathroom") || door.name.Contains("KAMAR MANDI") || door.name.Contains("MainEntrance"))
                {
                    UpgradeDoorToModernPanel(door, root);
                }
            }
            // Also upgrade non-hinged KAMAR MANDIDoor / KosMainEntranceDoor mesh objects if they lack HingedDoor
            foreach (string extraDoor in new[] { "KAMAR MANDIDoor", "KosMainEntranceDoor" })
            {
                var dObj = Find(extraDoor);
                if (dObj && !dObj.GetComponent<HingedDoor>() && dObj.GetComponent<Renderer>())
                {
                    var hd = dObj.AddComponent<HingedDoor>();
                    hd.hinge = dObj.transform;
                    hd.locked = true;
                    UpgradeDoorToModernPanel(hd, root);
                }
            }

            // Dark number backing plates behind remaining room numbers so flashlight never washes them out
            foreach (var num in root.GetComponentsInChildren<TextMesh>(true).Where(tm => tm && tm.name.EndsWith("Number")).ToArray())
            {
                foreach (var oldPlate in num.GetComponentsInChildren<Transform>(true).Where(t => t && t.name == "NumberPlate").ToArray())
                    Object.DestroyImmediate(oldPlate.gameObject);
                var plate = GameObject.CreatePrimitive(PrimitiveType.Cube);
                plate.name = "NumberPlate";
                plate.transform.SetParent(num.transform, false);
                Object.DestroyImmediate(plate.GetComponent<Collider>());
                plate.transform.localPosition = new Vector3(0, 0, .02f);
                plate.transform.localScale = new Vector3(.26f / Mathf.Max(.05f, num.transform.lossyScale.x), .18f / Mathf.Max(.05f, num.transform.lossyScale.y), .012f / Mathf.Max(.05f, num.transform.lossyScale.z));
                plate.GetComponent<Renderer>().sharedMaterial = Material("Repair_NumberBacking");
            }

            // Bug 8: Attach opaque depth-tested Male & Female logo plaques directly onto BathroomPRIADoor & BathroomWANITADoor!
            AttachRestroomPlaque(Find("BathroomPRIADoor"), "BathroomPRIAPictogram", "RestroomMalePlaque.png");
            AttachRestroomPlaque(Find("BathroomWANITADoor"), "BathroomWANITAPictogram", "RestroomFemalePlaque.png");

            // Bug 4: Tenant slippers (upgraded further in ApplyFinishProps)
            var slippers = root.GetComponentsInChildren<Transform>(true)
                .Where(t => t.name == "TenantSlipper" || t.name == "TenantFlipFlops")
                .Select(t => t.name == "TenantSlipper" ? t.position : SafeBounds(t.gameObject).center - Vector3.forward * .055f)
                .ToArray();
            foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t => t.name == "TenantSlipper" || t.name == "SlipperStrap" || t.name == "TenantFlipFlops").ToArray())
                Object.DestroyImmediate(t.gameObject);
            var spots = new List<Vector3>();
            foreach (var p in slippers) if (!spots.Any(x => Vector3.Distance(x, p) < .20f)) spots.Add(p);
            for (int si = 0; si < spots.Count; si++)
            {
                var pair = Model("IndonesianFlipFlops", "TenantFlipFlops", root, spots[si], Quaternion.Euler(0, (si % 3 - 1) * 14f, 0));
                ApplySandalVariant(pair, si);
                KosVisualV2.Resize(pair, new Vector3(.24f, .052f, .275f));
                KosVisualV2.SetCenter(pair, spots[si] + Vector3.forward * .055f);
                Ground(pair, spots[si].y > 1.5f ? 3.302f : .002f);
            }

            // Bug 9: Kos Bathrooms — non-metallic URP porcelain toilets (ZERO black holes!), unburied wall showers facing into stalls, ceramic tile floor!
            foreach (string name in new[] { "SquatToiletBase", "ToiletBowl", "SecondSquatToilet", "BathroomWaterTank", "SecondWaterTank", "ShowerRiser", "ShowerHead", "SharedKitchenCounter", "Stove", "KitchenSink" })
                foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t => t.name == name).ToArray())
                    Object.DestroyImmediate(t.gameObject);

            foreach (float z in new[] { 3.30f, 5.20f })
            {
                Imported("Toilet_Round_A", "KosPorcelainToilet_" + z, root, new Vector3(5.52f, 0, z), new Vector3(.45f, .78f, .73f), Quaternion.Euler(0, -90, 0));
                Imported("Shower", "KosWallShower_" + z, root, new Vector3(5.61f, .88f, z - .38f), new Vector3(.26f, 1.32f, .28f), Quaternion.Euler(0, 90, 0));
            }
            Imported("Sink_A", "KosWashBasin", root, new Vector3(2.05f, 0, 5.66f), new Vector3(.53f, .85f, .38f), Quaternion.Euler(0, 180, 0));

            // Apply bright ceramic bathroom tile material to bathroom floor meshes so bathroom floors aren't pitch-black!
            var bathTileMat = Material("Repair_BathFloorTile");
            if (bathTileMat)
            {
                foreach (var r in root.GetComponentsInChildren<MeshRenderer>(true))
                {
                    Vector3 lp = root.InverseTransformPoint(r.bounds.center);
                    if (lp.x > 2.4f && lp.z > 1.8f && lp.z < 6.6f && lp.y < .25f && r.bounds.size.y < .35f)
                        r.sharedMaterial = bathTileMat;
                }
            }

            // Bug 10: Kos Shared Kitchen — real CC0 wooden cabinetry (NO primitive cubes!), LPG tank, unburied sink, realistic enamel fridge, utensils!
            Imported("electric_stove", "KosElectricStove", root, new Vector3(-4.28f, 0, 3.12f), new Vector3(.64f, .86f, .64f), Quaternion.Euler(0, 90, 0));
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/small_lpg_tank/small_lpg_tank.fbx", "KosLpgTank", root,
                root.TransformPoint(new Vector3(-3.72f, 0, 3.16f)), Quaternion.Euler(0, 35, 0), new Vector3(.30f, .44f, .30f));

            var fridge = Imported("Fridge", "KosRefrigerator", root, new Vector3(-4.34f, 0, 5.32f), new Vector3(.68f, 1.66f, .68f), Quaternion.Euler(0, 90, 0));
            foreach (var r in fridge.GetComponentsInChildren<Renderer>())
                r.sharedMaterials = Enumerable.Repeat(Material("Repair_ApplianceEnamel"), r.sharedMaterials.Length).ToArray();

            // Two real CC0 wooden kitchen base cabinets along west wall (x = -4.36, z = 3.78 & 4.54, height 0.86m)
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/modern_wooden_cabinet/modern_wooden_cabinet.fbx", "KosKitchenCabinet_1", root,
                root.TransformPoint(new Vector3(-4.36f, 0, 3.78f)), Quaternion.Euler(0, 90, 0), new Vector3(.64f, .86f, .76f), Material("Repair_BedWood"));
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/modern_wooden_cabinet/modern_wooden_cabinet.fbx", "KosKitchenCabinet_2", root,
                root.TransformPoint(new Vector3(-4.36f, 0, 4.54f)), Quaternion.Euler(0, 90, 0), new Vector3(.64f, .86f, .76f), Material("Repair_BedWood"));

            Imported("vintage_electric_kettle", "KosKitchenKettle", root, new Vector3(-4.36f, .865f, 3.64f), new Vector3(.24f, .27f, .23f), Quaternion.Euler(0, 40, 0));
            Imported("pot_enamel_01", "KosCookingPot", root, new Vector3(-4.26f, .865f, 3.14f), new Vector3(.30f, .21f, .26f), Quaternion.identity);
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/brass_pan_01/brass_pan_01.fbx", "KosBrassPan", root,
                root.TransformPoint(new Vector3(-4.28f, .865f, 2.94f)), Quaternion.Euler(0, 20, 0), new Vector3(.26f, .07f, .36f), Material("Repair_Brass"));
            Imported("wooden_cutting_board", "KosCuttingBoard", root, new Vector3(-4.30f, .865f, 4.14f), new Vector3(.28f, .025f, .36f), Quaternion.identity);
            Imported("wooden_spoon", "KosCookingSpoon", root, new Vector3(-4.28f, .89f, 4.18f), new Vector3(.04f, .025f, .25f), Quaternion.Euler(0, 25, 0));
            Imported("metal_jug", "KosWaterJug", root, new Vector3(-4.34f, .865f, 4.62f), new Vector3(.20f, .27f, .20f), Quaternion.identity);
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/InterludeStreet/kenney-food-kit/bowl-soup.fbx", "KosDiningBowl_1", root,
                root.TransformPoint(new Vector3(-4.32f, .865f, 4.38f)), Quaternion.identity, new Vector3(.18f, .08f, .18f), Material("Repair_Ceramic"));

            // Bugs 11 & 12: Spacious Kos motor parking (5 varied motorcycles in left & right bays at z = -3.7, ZERO gate clipping!) + concrete street curb!
            var curb = GameObject.CreatePrimitive(PrimitiveType.Cube);
            curb.name = "KosStreetThresholdCurb";
            curb.transform.SetParent(root, false);
            curb.transform.localPosition = new Vector3(0, .025f, -8.35f);
            curb.transform.localScale = new Vector3(13.8f, .07f, .34f);
            curb.GetComponent<Renderer>().sharedMaterial = Old("House_MAT_Concrete") ?? Material("Repair_Ceramic");

            var rakaMotor = Find("RakaMotor");
            var damarScooterAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DamarScooter.fbx");
            Vector3[] motorSlots =
            {
                new Vector3(-4.85f, 0f, -3.75f),
                new Vector3(-3.65f, 0f, -3.82f),
                new Vector3(-2.45f, 0f, -3.72f),
                new Vector3(2.65f, 0f, -3.76f),
                new Vector3(3.95f, 0f, -3.84f),
            };
            float[] motorYaws = { 164f, 174f, 168f, 192f, 184f };
            Color[] motorColors =
            {
                new Color(.14f, .15f, .17f), // Matte black
                new Color(.42f, .09f, .10f), // Honda maroon red
                new Color(.11f, .18f, .30f), // Yamaha dark navy
                new Color(.58f, .60f, .62f), // Metallic silver
                new Color(.12f, .24f, .18f), // Deep forest green
            };
            for (int i = 0; i < motorSlots.Length; i++)
            {
                GameObject copy;
                if (i % 2 == 1 && damarScooterAsset)
                {
                    copy = (GameObject)PrefabUtility.InstantiatePrefab(damarScooterAsset);
                    copy.transform.SetParent(root, false);
                }
                else
                {
                    copy = Object.Instantiate(rakaMotor, root);
                }
                copy.name = "TenantMotor_" + (i + 1);
                foreach (var c in copy.GetComponentsInChildren<MonoBehaviour>(true)) Object.DestroyImmediate(c);
                foreach (var light in copy.GetComponentsInChildren<Light>(true)) Object.DestroyImmediate(light.gameObject);
                float baseY = rakaMotor ? rakaMotor.transform.localPosition.y : 0f;
                copy.transform.localPosition = new Vector3(motorSlots[i].x, baseY, motorSlots[i].z);
                copy.transform.localRotation = Quaternion.Euler(0, motorYaws[i], 0);
                Ground(copy, 0.002f);

                foreach (var renderer in copy.GetComponentsInChildren<Renderer>())
                {
                    var mats = renderer.sharedMaterials;
                    for (int m = 0; m < mats.Length; m++)
                    {
                        if (mats[m] && (mats[m].name.Contains("Body") || mats[m].name.Contains("Paint") || mats[m].name.Contains("Scooter")))
                        {
                            var tint = new Material(mats[m]);
                            tint.SetColor("_BaseColor", motorColors[i]);
                            string path = Mats + "TenantMotor" + i + "_" + m + ".mat";
                            var old = AssetDatabase.LoadAssetAtPath<Material>(path);
                            if (old) { EditorUtility.CopySerialized(tint, old); Object.DestroyImmediate(tint); tint = old; }
                            else AssetDatabase.CreateAsset(tint, path);
                            mats[m] = tint;
                        }
                    }
                    renderer.sharedMaterials = mats;
                }
            }

            // InterludeAlley fixes: replace 6 primitive black cube AlleySandals with Swallow flip-flops, and remove floating mid-air wires!
            var alley = Find("InterludeAlley");
            if (alley)
            {
                alley.SetActive(true);
                int asIdx = 0;
                foreach (var cubeSandal in alley.GetComponentsInChildren<Transform>(true).Where(t => t && t.name == "AlleySandals").ToArray())
                {
                    if (!cubeSandal || !cubeSandal.gameObject.activeInHierarchy || cubeSandal.GetComponentsInChildren<Transform>(true).Length > 3) continue; // already upgraded
                    var sb = SafeBounds(cubeSandal.gameObject);
                    var sp = cubeSandal.parent;
                    var srot = cubeSandal.rotation;
                    Object.DestroyImmediate(cubeSandal.gameObject);
                    var replacement = Model("IndonesianFlipFlops", "AlleySandals", sp, sb.center, srot);
                    ApplySandalVariant(replacement, asIdx++);
                    KosVisualV2.Resize(replacement, new Vector3(.24f, .052f, .275f));
                    KosVisualV2.SetCenter(replacement, sb.center);
                    Ground(replacement, sb.min.y);
                }
                // Bug 12: Remove unattached mid-air power wire segments that float without poles
                foreach (var wire in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                    .Where(t => t && (t.name == "InterludeWire" || t.name == "MelatiPowerCable")).ToArray())
                {
                    var r = wire.GetComponent<Renderer>();
                    if (r) r.enabled = false;
                }
            }

            if (!Find("SharedYardWaterGallon"))
            {
                var gallon = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Original/Models/RefillWaterGallon.fbx"));
                gallon.name = "SharedYardWaterGallon";
                gallon.transform.SetParent(root, false);
                gallon.transform.localPosition = new Vector3(-2.3f, 0, 3.35f);
                foreach (var r in gallon.GetComponentsInChildren<Renderer>())
                    r.sharedMaterials = r.sharedMaterials.Select(m => m.name.Contains("Water") ? Material("Repair_SandalStrap") : Material("Repair_Iron")).ToArray();
            }

            DaySky("PrologDaySky", new Color(.5f, .65f, .8f), 1.1f);
            DaySky("PrologDuskSky", new Color(.65f, .38f, .27f), .7f);
            if (!Object.FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None).Any(l => l.type == LightType.Directional))
            {
                var sun = new GameObject("MelatiSun", typeof(Light));
                sun.transform.rotation = Quaternion.Euler(45, -30, 0);
                var light = sun.GetComponent<Light>();
                light.type = LightType.Directional; light.intensity = .035f;
                light.color = new Color(.6f, .7f, 1); light.shadows = LightShadows.Soft;
            }
            FixDepthTestedTextMeshes();
            Save();
            Debug.Log("PROLOG47_KOS_FACILITIES_APPLIED selected CC0 fixtures, open kitchen/dining, non-clipping 5-motor lot and persistent scenery");
        }

        private static void ApplySandalVariant(GameObject pair, int index)
        {
            string strapMat = (index % 3) switch
            {
                1 => "Repair_SandalStrapGreen",
                2 => "Repair_SandalStrapRed",
                _ => "Repair_SandalStrap",
            };
            foreach (var r in pair.GetComponentsInChildren<Renderer>())
            {
                r.sharedMaterials = r.sharedMaterials.Select(m =>
                    m && m.name.Contains("Strap") ? Material(strapMat) :
                    m && m.name.Contains("Footbed") ? Material("Repair_SandalFootbed") :
                    Material("Repair_SandalRubber")).ToArray();
            }
        }

        public static void ApplyFinishProps()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            int idx = 0;
            foreach (var pair in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t => t.name == "TenantFlipFlops").ToArray())
            {
                var bounds = SafeBounds(pair.gameObject);
                var parent = pair.parent;
                var rotation = pair.rotation;
                var replacement = Model("IndonesianFlipFlops", "TenantFlipFlops", parent, bounds.center, rotation);
                ApplySandalVariant(replacement, idx++);
                KosVisualV2.Resize(replacement, new Vector3(.24f, .052f, .275f));
                KosVisualV2.SetCenter(replacement, bounds.center);
                Ground(replacement, bounds.center.y > 1 ? 3.302f : .002f);
                Object.DestroyImmediate(pair.gameObject);
            }
            FixDepthTestedTextMeshes();
            Save();

            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var cup = Find("ChildMilkCup");
            if (cup)
            {
                var childDesk = Find("ChildDesk");
                float deskTopY = childDesk ? SafeBounds(childDesk).max.y + .001f : 4.553f;
                var center = SafeBounds(cup).center;
                // Rotate so 'Irregular glazed crack' faces straight toward the player approaching Bima's desk (+d.north)
                cup.transform.rotation = Quaternion.LookRotation(-d.north);
                KosVisualV2.SetCenter(cup, center);
                Ground(cup, deskTopY);
                d.upper.cupAim.position = SafeBounds(cup).center;
            }
            var scratches = Find("BathWallScratches");
            if (scratches) scratches.transform.rotation = Quaternion.LookRotation(-d.east);
            foreach (var stain in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t => t.name.StartsWith("BloodFloor_") && d.LocalPosition(t.position).x > 0))
                stain.position = d.P(2.9f, 5.55f, 3.834f);

            foreach (string name in new[] { "BloodHandprint", "WallScratches", "BloodSpatter" })
            {
                var mat = AssetDatabase.LoadAssetAtPath<Material>(Mats + name + ".mat");
                if (!mat) continue;
                mat.SetFloat("_BlendModePreserveSpecular", 0);
                mat.SetFloat("_SpecularHighlights", 0);
                mat.SetFloat("_EnvironmentReflections", 0);
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
                mat.EnableKeyword("_ENVIRONMENTREFLECTIONS_OFF");
                EditorUtility.SetDirty(mat);
            }
            Save();
        }

        public static void ApplyAcceptanceReview() { ApplyReviewCorrections(); ApplyArchitecture(); ApplyPrologSkin(); }

        public static void ApplyPrologSkin()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(Models + "NyonyaPrologSurfaceRepair.fbx");
            var sample = (GameObject)PrefabUtility.InstantiatePrefab(asset);

            // Create a Prolog-only surface material with balanced roughness & warm pale tone so her face never blows out white under close flashlight!
            var baseSurf = AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Materials/NyonyaDesignerSurface.mat");
            Material prologSurf = null;
            if (baseSurf)
            {
                string surfPath = Mats + "NyonyaPrologSurface.mat";
                prologSurf = AssetDatabase.LoadAssetAtPath<Material>(surfPath);
                if (!prologSurf)
                {
                    prologSurf = new Material(baseSurf);
                    AssetDatabase.CreateAsset(prologSurf, surfPath);
                }
                else
                {
                    EditorUtility.CopySerialized(baseSurf, prologSurf);
                }
                prologSurf.SetColor("_BaseColor", new Color(.89f, .87f, .84f, 1f));
                prologSurf.SetFloat("_Smoothness", .14f);
                prologSurf.SetFloat("_BumpScale", .55f);
                EditorUtility.SetDirty(prologSurf);
            }

            try
            {
                var source = sample.GetComponentsInChildren<SkinnedMeshRenderer>(true).Single();
                // Apply to ALL Nyonya AtmajaThreatPresentation instances in Prolog (basement hunter + chase figure if separate)
                foreach (var pose in Object.FindObjectsByType<AtmajaThreatPresentation>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                {
                    if (!pose || !pose.authoredMotion) continue;
                    var target = pose.authoredMotion.GetComponentsInChildren<SkinnedMeshRenderer>(true).FirstOrDefault(r => r.name == "NyonyaDesignerBody");
                    if (!target) continue;
                    var bones = pose.authoredMotion.GetComponentsInChildren<Transform>(true).GroupBy(t => t.name).ToDictionary(g => g.Key, g => g.First());
                    target.sharedMesh = source.sharedMesh;
                    target.bones = source.bones.Select(b => bones[b.name]).ToArray();
                    target.rootBone = bones[source.rootBone.name];
                    target.localBounds = source.localBounds;
                    target.updateWhenOffscreen = true;
                    target.quality = SkinQuality.Bone4;
                    if (prologSurf) target.sharedMaterial = prologSurf;
                    EditorUtility.SetDirty(target);
                    PrefabUtility.RecordPrefabInstancePropertyModifications(target);
                }
            }
            finally
            {
                Object.DestroyImmediate(sample);
            }
            Save();
            Debug.Log("PROLOG47_SKIN_RECOVERED 1-to-1 VRM weights applied; zero torso-to-arm bleed; shared phases untouched");
        }

        public static void ApplyFinalReview() { ApplyReviewCorrections(); ApplyArchitecture(); }
        public static void ApplyGeometryReview() { ApplyArchitecture(); ApplyLadder(); }

        public static void ApplyReviewCorrections()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var root = Find("Kos_Building_Corridors").transform;
            var bucket = Find("SharedWaterBucket");
            if (bucket) bucket.transform.localPosition = new Vector3(-3.65f, 0, 5.45f);
            // Bug 10: Place KosKitchenWashBasin at z = 3.36 facing +Z into the kitchen so it NEVER Z-fights inside the z = 3.0 wall!
            Imported("Sink_A", "KosKitchenWashBasin", root, new Vector3(-3.15f, 0, 3.36f), new Vector3(.53f, .85f, .44f), Quaternion.identity);
            Imported("Sink_A", "KosWashBasin", root, new Vector3(2.05f, 0, 5.66f), new Vector3(.53f, .85f, .38f), Quaternion.Euler(0, 180, 0));
            Save();
        }

        public static void ApplyArchitecture()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var environment = Find("Atmaja_Environment_Art").transform;

            GameObject Wall(string name, float x, float depth, float y, Vector3 size, Material customMat = null)
            {
                Delete(name);
                var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.name = name;
                go.transform.SetParent(environment, true);
                go.transform.SetPositionAndRotation(d.P(x, depth, y), Quaternion.LookRotation(d.north));
                go.transform.localScale = size;
                go.GetComponent<Renderer>().sharedMaterial = customMat ? customMat : Old("House_MAT_PlasterDamaged");
                return go;
            }

            // Bugs 18 & 23: Extend ALL short ground-floor partitions & CellarShaft walls flush to ground ceiling (y = 3.82f)
            var lintel = Find("FrontSittingDoorLintel");
            if (lintel)
            {
                lintel.transform.position = d.P(-1.4f, 2.5f, (2.71f + 3.82f) / 2f);
                var size = lintel.transform.localScale;
                size.y = 3.82f - 2.71f;
                lintel.transform.localScale = size;
            }
            foreach (string gWall in new[] { "FrontSittingPartitionEast", "FrontSittingPartitionWest", "CellarShaftE", "CellarShaftW" })
            {
                var gw = Find(gWall);
                if (!gw) continue;
                var b = SafeBounds(gw);
                if (b.max.y < 3.80f && b.max.y > 2.2f)
                {
                    float bottom = b.min.y;
                    float newH = 3.82f - bottom;
                    var ls = gw.transform.localScale;
                    float ratio = newH / Mathf.Max(.1f, b.size.y);
                    gw.transform.localScale = new Vector3(ls.x, ls.y * ratio, ls.z);
                    gw.transform.position += Vector3.up * ((bottom + 3.82f) * .5f - SafeBounds(gw).center.y);
                }
            }
            // Extend upper-floor room partitions flush to upper ceiling (y = 6.83f)
            foreach (string uWall in new[] { "ChildBathWall", "BathMasterWall" })
            {
                var uw = Find(uWall);
                if (!uw) continue;
                var b = SafeBounds(uw);
                if (b.max.y < 6.80f && b.min.y > 3.5f)
                {
                    float bottom = 3.82f;
                    float newH = 6.83f - bottom;
                    var ls = uw.transform.localScale;
                    float ratio = newH / Mathf.Max(.1f, b.size.y);
                    uw.transform.localScale = new Vector3(ls.x, ls.y * ratio, ls.z);
                    uw.transform.position += Vector3.up * ((bottom + 6.83f) * .5f - SafeBounds(uw).center.y);
                }
            }

            var east = Find("MasterEastWall");
            if (east)
            {
                east.transform.position = d.P(2.4f, (3.25f + 5.2f) / 2f, (3.82f + 6.83f) / 2f);
                east.transform.localScale = new Vector3(.16f, 3.01f, 1.95f);
            }
            Wall("MasterExtensionEastWall", 5.75f, 1.64f, 5.325f, new Vector3(.16f, 3.01f, 3.28f));
            Wall("MasterExtensionNorthWall", 4.075f, .08f, 5.325f, new Vector3(3.35f, 3.01f, .16f));
            Wall("MasterExtensionBalconyWall", 4.075f, 3.25f, 5.325f, new Vector3(3.35f, 3.01f, .16f));

            // Bug 31 & 37: Master Bedroom — use rich 2K PBR carved dark-wood GothicBed_01 (or textured DoubleBedSet) and unclip MasterDresser!
            foreach (string name in new[] { "MasterBed", "MasterMattress", "MasterPillow", "MasterBedSet" }) Delete(name);
            var gothicAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Phase5/Imported/Models/GothicBed_01.fbx");
            var gothicMat = AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Phase5/Materials/GothicBed_01_P5_GothicBed_01_GothicBed_01.mat");
            GameObject masterBed;
            if (gothicAsset && gothicMat)
            {
                masterBed = new GameObject("MasterBedSet");
                masterBed.transform.SetParent(environment, true);
                masterBed.transform.SetPositionAndRotation(d.P(4.25f, 1.28f, 3.83f), Quaternion.LookRotation(-d.north));
                var child = (GameObject)PrefabUtility.InstantiatePrefab(gothicAsset);
                child.name = "MasterMattress";
                child.transform.SetParent(masterBed.transform, false);
                foreach (var r in child.GetComponentsInChildren<Renderer>())
                    r.sharedMaterials = Enumerable.Repeat(gothicMat, r.sharedMaterials.Length).ToArray();
                KosVisualV2.Resize(masterBed, new Vector3(1.72f, 1.28f, 2.08f));
                KosVisualV2.SetCenter(masterBed, d.P(4.25f, 1.28f, 4.47f));
                Ground(masterBed, 3.83f);
                FitCollider(masterBed);
            }
            else
            {
                masterBed = Model("DoubleBedSet", "MasterBedSet", environment, d.P(4.25f, 1.28f, 3.83f), Quaternion.LookRotation(-d.north));
                Ground(masterBed, 3.83f);
                FitCollider(masterBed);
            }

            void Move(string name, Vector3 delta) { var go = Find(name); if (go) go.transform.position += delta; }
            var night = Find("MasterNightstand");
            if (night)
            {
                // Rotate MasterNightstand to face south (+d.north) and close its protruding drawer so it never juts across MasterBedSet!
                night.transform.rotation = Quaternion.LookRotation(d.north);
                foreach (var dr in night.GetComponentsInChildren<Transform>(true))
                {
                    if (dr && dr != night.transform && dr.name.ToLowerInvariant().Contains("drawer"))
                        dr.localPosition = new Vector3(0f, dr.localPosition.y, 0f);
                }
                KosVisualV2.SetCenter(night, d.P(2.90f, .44f, 4.13f));
                Ground(night, 3.83f);
                FitCollider(night);
                var clock = Find("MasterAlarmClock");
                if (clock)
                {
                    var nb = SafeBounds(night);
                    clock.transform.rotation = Quaternion.LookRotation(d.north);
                    KosVisualV2.SetCenter(clock, new Vector3(nb.center.x, nb.max.y + .10f, nb.center.z));
                    Ground(clock, nb.max.y);
                }
            }
            // Bug 37: Move MasterDresser to the east wall (x = 5.32, depth = 2.55) facing west so it NEVER clips under the bed headboard!
            var dresser = Find("MasterDresser");
            if (dresser)
            {
                Vector3 oldPos = SafeBounds(dresser).center;
                dresser.transform.rotation = Quaternion.LookRotation(-d.east);
                KosVisualV2.SetCenter(dresser, d.P(5.28f, 2.45f, oldPos.y));
                Ground(dresser, 3.83f);
                FitCollider(dresser);
                Vector3 deltaD = SafeBounds(dresser).center - oldPos;
                Move("MasterCandle", deltaD);
            }
            var rocker = Find("MasterRocker");
            if (rocker)
            {
                var center = d.P(1.85f, 3.3f, 4.32f);
                rocker.transform.rotation = Quaternion.LookRotation(-d.north);
                KosVisualV2.SetCenter(rocker, center);
                Ground(rocker, 3.83f);
                FitCollider(rocker);
            }
            Delete("MasterPortrait");
            // Ensure NO MasterRoom desk/table/shelf clips into MasterBedSet (e.g. writing table on right side of bed in 37-master-room)!
            if (masterBed)
            {
                var mb = SafeBounds(masterBed);
                mb.Expand(-.03f);
                foreach (var tr in environment.GetComponentsInChildren<Transform>(true).ToArray())
                {
                    if (!tr || tr.parent != environment || tr == masterBed.transform) continue;
                    string tn = tr.name;
                    if (tn.Contains("Wall") || tn.Contains("Floor") || tn.Contains("Ceiling") || tn.Contains("Slab") || tn == "MasterNightstand" || tn == "MasterAlarmClock") continue;
                    var tb = SafeBounds(tr.gameObject);
                    if (tb.min.y > 3.7f && tb.max.y < 5.3f && mb.Intersects(tb))
                    {
                        Vector3 shift = d.north * .52f;
                        tr.position += shift;
                        // Move any tabletop document sitting right on top of this table together with it!
                        foreach (var doc in environment.GetComponentsInChildren<AtmajaItem>(true))
                        {
                            if (doc && doc.transform != tr && Vector3.Distance(doc.transform.position, tb.center + Vector3.up * (tb.extents.y)) < .55f)
                                doc.transform.position += shift;
                        }
                    }
                }
            }

            // Bug 38: Replace VoidChandelier if it has shredded floating triangles with clean Chandelier_02.fbx!
            var voidChand = Find("VoidChandelier");
            var cleanChandAsset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Phase5/Imported/Models/Chandelier_02.fbx");
            if (voidChand && cleanChandAsset)
            {
                var cb = SafeBounds(voidChand);
                var cp = voidChand.transform.parent;
                var cLight = voidChand.GetComponentInChildren<Light>(true);
                if (cLight) cLight.transform.SetParent(cp, true);
                Object.DestroyImmediate(voidChand);
                var newChand = new GameObject("VoidChandelier");
                newChand.transform.SetParent(cp, true);
                newChand.transform.position = cb.center;
                var cBody = (GameObject)PrefabUtility.InstantiatePrefab(cleanChandAsset);
                cBody.transform.SetParent(newChand.transform, false);
                var brassMat = Old("Upper_ChandelierBrass") ?? Material("Repair_Brass");
                foreach (var r in cBody.GetComponentsInChildren<Renderer>())
                    r.sharedMaterials = Enumerable.Repeat(brassMat, r.sharedMaterials.Length).ToArray();
                KosVisualV2.Resize(newChand, new Vector3(.88f, .95f, .88f));
                KosVisualV2.SetCenter(newChand, cb.center);
                if (cLight) cLight.transform.SetParent(newChand.transform, true);
            }

            var wood = Find("UpperFloor_South");
            if (wood) Prolog47MeshRepair.Subtract(wood.GetComponent<MeshFilter>(), d, new Bounds(new Vector3(-1.1f, 3.82f, 2.60f), new Vector3(2.60f, 1, 5.10f)), "UpperFloorOutsideBathroom");
            foreach (var go in environment.GetComponentsInChildren<MeshFilter>().Where(f => f.name.StartsWith("UpperSlab_") || f.name.StartsWith("GroundFloorCeiling_") || f.name == "BalconyDeck" || f.name == "BalconyWalk"))
                Prolog47MeshRepair.RemoveUpwardFinish(go, d, 3.82f, "FinishSupport_" + go.name);
            foreach (var f in environment.GetComponentsInChildren<MeshFilter>().Where(f => f.name == "Static__House_MAT_TrimTeak"))
                foreach (int side in new[] { -1, 1 })
                    Prolog47MeshRepair.Subtract(f, d, new Bounds(new Vector3(side * 1.12f, 4.27f, .60f), new Vector3(.20f, .20f, 1.10f)), "HouseTrimOutsideRooms");

            // Bug 42: Open cellar + delete floating white PVC pipes (RagRod & StairExitRag) & stray ceiling landing block!
            foreach (string name in new[] { "CellarSpineNorth", "CellarSpineSouth", "CellarReturnPartition", "RagRod", "StairExitRag", "CellarClosingStairLanding" })
                Delete(name);
            if (d.basement) d.basement.ragOriginal = null;

            // Scatter varied CC0 wooden/military crates in cellar so basement doesn't look sparse or cloned
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "CellarExtraCrate_1", environment,
                d.P(4.6f, 6.2f, -2.79f), Quaternion.Euler(0, 22, 0), new Vector3(.68f, .52f, .52f), Material("Repair_BedWood"));
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/old_military_crate/old_military_crate.fbx", "CellarExtraCrate_2", environment,
                d.P(4.4f, 3.1f, -2.79f), Quaternion.Euler(0, -18, 0), new Vector3(.76f, .44f, .48f), Material("Repair_BedWood"));

            // Bug 45 & 46: Texture/dress WestCollapsedMasonry_0..3 outside the escape window so opening the window reveals dark brick & foliage instead of 4 raw grey cubes!
            var brickMat = Old("House_MAT_BrickBase") ?? Old("House_MAT_PlasterDamaged");
            for (int mi = 0; mi < 4; mi++)
            {
                var wm = Find("WestCollapsedMasonry_" + mi);
                if (!wm) continue;
                var wr = wm.GetComponent<Renderer>();
                if (wr && brickMat) wr.sharedMaterial = brickMat;
                // Lower huge monolith cubes into low ruined garden boundary wall + add shrubs
                var wb = SafeBounds(wm);
                if (wb.size.y > 1.4f)
                {
                    var ls = wm.transform.localScale;
                    wm.transform.localScale = new Vector3(ls.x, ls.y * .45f, ls.z);
                    Ground(wm, 0f);
                }
            }
            if (d.chase && d.chase.windowExitSpot)
            {
                Vector3 outPos = d.chase.windowExitSpot.position + d.chase.windowExitSpot.forward * 2.4f;
                outPos.y = 0f;
                ProjectAsset("Assets/_LayarTerakhir/assets/Art/Review/Models/LodShrub.fbx", "EscapeWindowCourtyardShrub_1", environment,
                    outPos + Vector3.right * 1.1f, Quaternion.Euler(0, 35, 0), new Vector3(1.25f, .95f, 1.25f));
                ProjectAsset("Assets/_LayarTerakhir/assets/Art/Review/Models/LodShrub.fbx", "EscapeWindowCourtyardShrub_2", environment,
                    outPos - Vector3.right * 1.2f, Quaternion.Euler(0, 140, 0), new Vector3(1.35f, 1.05f, 1.35f));
            }

            // Bug 44: Flush KitchenEmergencyCharger neatly into lower kitchen baseboard & ensure KitchenVaultContext never renders as a floating black rectangle!
            var kCharger = Find("KitchenEmergencyCharger");
            if (kCharger)
            {
                kCharger.transform.SetPositionAndRotation(d.P(5.90f, 10.15f, .72f), Quaternion.LookRotation(d.north));
                kCharger.transform.localScale = new Vector3(.035f, .14f, .095f);
                var kr = kCharger.GetComponent<Renderer>();
                if (kr) kr.sharedMaterial = Material("Repair_NumberBacking");
            }
            var kvCtx = Find("KitchenVaultContext");
            if (kvCtx && kvCtx.GetComponent<Renderer>()) kvCtx.GetComponent<Renderer>().enabled = false;
            if (d.kitchenShutter)
            {
                d.kitchenShutter.transform.SetPositionAndRotation(d.P(5.96f, 10.975f, 1.54f), Quaternion.LookRotation(d.north));
                d.kitchenShutter.transform.localScale = new Vector3(.08f, 1.28f, 1.12f);
                var sr = d.kitchenShutter.GetComponent<Renderer>();
                if (sr) sr.sharedMaterial = Material("Repair_DoorPaint");
            }
            // Hide/subtract any stray floating black/dark-wood decorative slab perched high to the top-left of the kitchen escape window
            var highKitchenCut = new Bounds(new Vector3(5.72f, 2.34f, 11.98f), new Vector3(1.15f, 1.02f, .96f));
            foreach (var f in environment.GetComponentsInChildren<MeshFilter>(true).Where(f => f && (f.name == "Static__House_MAT_WoodDoor" || f.name == "KitchenGlass__House_MAT_WoodDoor")).ToArray())
                Prolog47MeshRepair.Subtract(f, d, highKitchenCut, "KitchenHighFloatingBox_" + f.name);
            foreach (var r in environment.GetComponentsInChildren<MeshRenderer>(true))
            {
                if (!r || r.gameObject == d.kitchenShutter || r.name.Contains("Wall") || r.name.Contains("Lintel") || r.name.Contains("Ceiling") || r.name.Contains("Frame")) continue;
                Vector3 lp = d.LocalPosition(r.bounds.center);
                if (lp.x > 5.0f && lp.x < 6.5f && lp.y > 9.7f && lp.y < 12.5f && r.bounds.center.y > 1.85f && r.bounds.size.x < .8f && r.bounds.size.z < 1.6f)
                    r.enabled = false;
            }

            Save();
            Debug.Log("PROLOG47_ARCHITECTURE_APPLIED full-height walls, Gothic master bed, clean chandelier, open cellar without floating PVC pipes, dressed escape courtyard");
        }

        public static void ApplyDetails()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var root = Find("Atmaja_Environment_Art").transform;

            // Bug 22: Organic arterial-red bloody handprints strictly on solid plaster wall segments (NEVER overlapping ChildDoor, VentGrille, PortraitDosa1, BathroomDoor, or MasterDoor!)
            foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t =>
                t && (t.name == "BloodHandprint" || t.name == "BloodFinger" || t.name.StartsWith("BloodyHand_") || t.name.StartsWith("BloodFloor_"))).ToArray())
            {
                if (t) Object.DestroyImmediate(t.gameObject);
            }
            var hand = DecalMaterial("BloodHandprint", false);
            hand.SetColor("_BaseColor", new Color(.74f, .68f, .68f, .94f));
            EditorUtility.SetDirty(hand);
            var scratches = DecalMaterial("WallScratches", true);

            (float x, float h, float roll, float sx, float sy)[] handSpecs =
            {
                (-5.32f, 5.04f, -22f,  .24f, .35f), // solid plaster west of ChildDoor
                (-3.68f, 4.68f,  15f, -.22f, .33f), // solid plaster between ChildDoor and VentGrille
                (-2.54f, 4.56f, -29f,  .23f, .35f), // solid plaster between VentGrille and PortraitDosa1
                (-0.32f, 5.06f,  11f, -.24f, .36f), // solid plaster east of BathroomDoor (never ON BathroomDoor!)
                ( 0.34f, 4.82f, -17f,  .22f, .34f), // solid plaster west of MasterDoor
            };
            for (int i = 0; i < handSpecs.Length; i++)
            {
                var sp = handSpecs[i];
                var q = Quad("BloodyHand_" + i, root, d.P(sp.x, 5.305f, sp.h), Quaternion.LookRotation(-d.north) * Quaternion.Euler(0, 0, sp.roll), new Vector2(Mathf.Abs(sp.sx), sp.sy), hand);
                if (sp.sx < 0) q.transform.localScale = new Vector3(-Mathf.Abs(sp.sx), sp.sy, 1f);
            }

            var stain = DecalMaterial("BloodSpatter", false, true);
            foreach (var p in new[] { d.P(-3.8f, 5.75f, 3.833f), d.P(-2.95f, 5.52f, 3.833f), d.P(-1.7f, 5.8f, 3.833f), d.P(2.9f, 5.55f, 3.834f) })
                Quad("BloodFloor_" + p.x.ToString("F2"), root, p, Quaternion.Euler(90, p.x * 37f, 0), new Vector2(.76f, .88f), stain);

            // Bug 34: 5-finger desperate bloody wall scratches in bathroom + delete any stray grey cube on the floor beneath it!
            if (d.upper.scratchAim) d.upper.scratchAim.SetParent(root, true);
            Delete("BathScratchesAR"); Delete("BathScratchTarget"); Delete("BathWallScratches"); Delete("BathWallARTrace");
            Vector3 scratchPos = d.P(-2.285f, 3.25f, 4.94f);
            // Delete any unnamed/stray small grey cube sitting on the bathroom floor directly beneath BathWallScratches
            foreach (var r in root.GetComponentsInChildren<MeshRenderer>(true).ToArray())
            {
                if (!r) continue;
                Vector3 lp = d.LocalPosition(r.bounds.center);
                if (lp.x < -1.85f && lp.x > -2.45f && lp.y > 2.75f && lp.y < 3.75f && r.bounds.center.y > 3.80f && r.bounds.center.y < 4.35f &&
                    r.bounds.size.x < .65f && r.bounds.size.z < .65f && !r.name.Contains("Floor") && !r.name.Contains("Wall") && !r.name.Contains("Tub") && !r.name.Contains("Bucket"))
                {
                    Object.DestroyImmediate(r.gameObject);
                }
            }
            var scratch = Quad("BathWallScratches", root, scratchPos, Quaternion.LookRotation(-d.east), new Vector2(.48f, .74f), scratches);
            var trace = new GameObject("BathWallARTrace");
            trace.transform.SetParent(root, true);
            trace.transform.SetPositionAndRotation(scratch.transform.position, scratch.transform.rotation);
            d.upper.bathScratchesAR = trace;
            if (!d.upper.scratchAim) d.upper.scratchAim = new GameObject("ScratchAim").transform;
            d.upper.scratchAim.SetParent(trace.transform, true);
            d.upper.scratchAim.position = scratch.transform.position;
            var target = new GameObject("BathScratchTarget", typeof(BoxCollider), typeof(AtmajaItem));
            target.transform.SetParent(root, true);
            target.transform.SetPositionAndRotation(scratch.transform.position, scratch.transform.rotation);
            target.GetComponent<BoxCollider>().size = new Vector3(.48f, .74f, .02f);
            var item = target.GetComponent<AtmajaItem>();
            item.director = d; item.action = AtmajaAction.BathScratches; item.label = "Periksa bekas cakaran di dinding";

            // Bug 24: Compact crawlable recessed VentGrille (0.76m x 0.56m near floor) + HOLLOW 4-bar sheet-metal duct bezel (so opening VentGrille reveals Bima's bedroom!) + white AR glint!
            if (d.upper.ventMouth) d.upper.ventMouth.SetParent(root, true);
            foreach (string oldV in new[] { "VentCabinet", "VentHoleDark", "VentHoleAR", "VentGrilleHinge", "VentDuctFrame", "VentDuctTrim_Top", "VentDuctTrim_Bottom", "VentDuctTrim_Left", "VentDuctTrim_Right" })
                Delete(oldV);
            d.upper.ventCabinet = null;
            void VentTrim(string tName, float tx, float ty, Vector3 tScale)
            {
                var bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bar.name = tName;
                bar.transform.SetParent(root, true);
                Object.DestroyImmediate(bar.GetComponent<Collider>());
                bar.transform.SetPositionAndRotation(d.P(tx, 5.24f, ty), Quaternion.LookRotation(d.north));
                bar.transform.localScale = tScale;
                bar.GetComponent<Renderer>().sharedMaterial = Material("Repair_NumberBacking");
            }
            VentTrim("VentDuctTrim_Top", -3.0f, 4.465f, new Vector3(.80f, .035f, .14f));
            VentTrim("VentDuctTrim_Bottom", -3.0f, 3.895f, new Vector3(.80f, .035f, .14f));
            VentTrim("VentDuctTrim_Left", -3.385f, 4.18f, new Vector3(.035f, .58f, .14f));
            VentTrim("VentDuctTrim_Right", -2.615f, 4.18f, new Vector3(.035f, .58f, .14f));

            var hinge = new GameObject("VentGrilleHinge").transform;
            hinge.SetParent(root, true);
            hinge.SetPositionAndRotation(d.P(-3.38f, 5.315f, 3.82f), Quaternion.LookRotation(d.north));
            var cover = Model("VentGrille", "VentGrille", hinge, d.P(-3.0f, 5.315f, 4.18f), Quaternion.LookRotation(d.north));
            KosVisualV2.Resize(cover, new Vector3(.76f, .56f, .048f));
            KosVisualV2.SetCenter(cover, d.P(-3.0f, 5.315f, 4.18f));
            FitCollider(cover);
            var vent = cover.AddComponent<HingedDoor>();
            vent.hinge = hinge; vent.angle = -105; vent.requiresCrouchAndAR = true; vent.label = "Buka ventilasi";
            d.upper.ventDoor = vent;
            if (d.upper.ventMouth)
            {
                d.upper.ventMouth.position = d.P(-3.0f, 5.26f, 4.18f);
                foreach (var oldG in d.upper.ventMouth.GetComponentsInChildren<ARPhotoGlimmer>(true))
                    if (oldG) Object.DestroyImmediate(oldG.gameObject);
                ARPhotoGlimmer.Attach(d.upper.ventMouth, d.upper, "EVENT_P0_CHAIN1_BROKEN", true, true, false);
                d.upper.ventHoleAR = d.upper.ventMouth.GetComponentInChildren<ARPhotoGlimmer>(true).gameObject;
            }

            // Bug 16: Kitchen entry window — place KitchenWindowGlimmerAnchor right inside the visible window opening!
            var kWinAnchor = Find("KitchenWindowGlimmerAnchor");
            if (!kWinAnchor)
            {
                kWinAnchor = new GameObject("KitchenWindowGlimmerAnchor");
                kWinAnchor.transform.SetParent(root, true);
            }
            kWinAnchor.transform.position = d.P(5.92f, 10.72f, 1.48f);
            foreach (var oldG in kWinAnchor.GetComponentsInChildren<ARPhotoGlimmer>(true))
                if (oldG) Object.DestroyImmediate(oldG.gameObject);
            ARPhotoGlimmer.Attach(kWinAnchor.transform, d.upper, "EVENT_P0_ENTERED_HOUSE", false, true, false);

            // Bug 17: Fine-link realistic iron chains on FrontDoorRustChain, WellRustChain, AND upstairs MasterSealChain0..2!
            var front = Find("FrontDoorRustChain");
            var centre = front ? SafeBounds(front).center : d.P(0, -.23f, 1.65f);
            Delete("FrontDoorRustChain");
            Chain("FrontDoorRustChain", root, centre, d.east, Vector3.down * .065f, 1.30f);

            for (int ci = 0; ci < d.upper.chainLinks.Length; ci++)
            {
                var linkObj = d.upper.chainLinks[ci];
                if (!linkObj) continue;
                var lb = SafeBounds(linkObj);
                Vector3 lCenter = lb.size.sqrMagnitude > .001f ? lb.center : linkObj.transform.position;
                foreach (var childT in linkObj.GetComponentsInChildren<Transform>(true).Where(t => t != linkObj.transform).ToArray())
                    Object.DestroyImmediate(childT.gameObject);
                linkObj.transform.localScale = Vector3.one;
                linkObj.transform.rotation = Quaternion.identity;
                linkObj.transform.position = lCenter;
                var mf = linkObj.GetComponent<MeshFilter>();
                if (!mf) mf = linkObj.AddComponent<MeshFilter>();
                var mr = linkObj.GetComponent<MeshRenderer>();
                if (!mr) mr = linkObj.AddComponent<MeshRenderer>();
                Vector3 along = (d.east + Vector3.up * ((ci - 1) * .28f)).normalized;
                mf.sharedMesh = BuildChainMesh("MasterSealChainLinks_" + ci, along, Vector3.down * .045f, .92f);
                mr.sharedMaterial = Old("House_MAT_RustMetal") ?? Material("Repair_Iron");
            }

            var well = Find("WellCover");
            if (well)
            {
                foreach (var t in well.GetComponentsInChildren<Transform>(true).Where(t => t != well.transform).ToArray())
                    if (t) Object.DestroyImmediate(t.gameObject);
                var mesh = new Mesh { name = "Circular jointed well cover" };
                var vertices = new List<Vector3>();
                var triangles = new List<int>();
                const float radius = .50f;
                for (int board = 0; board < 7; board++)
                {
                    float z0 = -radius + board * (radius * 2 / 7) + .003f, z1 = -radius + (board + 1) * (radius * 2 / 7) - .003f;
                    var polygon = new List<Vector3>();
                    for (int k = 0; k <= 6; k++) { float z = Mathf.Lerp(z0, z1, k / 6f); polygon.Add(new Vector3(-Mathf.Sqrt(Mathf.Max(0, radius * radius - z * z)), 0, z)); }
                    for (int k = 6; k >= 0; k--) { float z = Mathf.Lerp(z0, z1, k / 6f); polygon.Add(new Vector3(Mathf.Sqrt(Mathf.Max(0, radius * radius - z * z)), 0, z)); }
                    int first = vertices.Count, n = polygon.Count;
                    vertices.AddRange(polygon.Select(p => p + Vector3.up * .018f));
                    vertices.AddRange(polygon.Select(p => p - Vector3.up * .018f));
                    for (int k = 1; k < n - 1; k++) triangles.AddRange(new[] { first, first + k, first + k + 1, first + n, first + n + k + 1, first + n + k });
                    for (int k = 0; k < n; k++) { int j = (k + 1) % n; triangles.AddRange(new[] { first + k, first + n + k, first + n + j, first + k, first + n + j, first + j }); }
                }
                mesh.SetVertices(vertices); mesh.SetTriangles(triangles, 0);
                mesh.SetUVs(0, vertices.Select(v => new Vector2(v.x, v.z)).ToList());
                mesh.RecalculateNormals(); mesh.RecalculateBounds();
                MeshObject("WellCoverBoards47", well.transform, d.P(2.85f, 11.2f, 1.10f), StoreMesh(mesh, "CircularWellCover"), Old("House_MAT_WoodDoor"));
                Chain("WellRustChain", well.transform, d.P(2.85f, 11.2f, 1.137f), d.east, Vector3.zero, .94f);
            }

            // Bug 36: Blood water surface fitted smoothly inside the curved porcelain bathtub bowl
            var water = d.upper.bathWater;
            var waterMesh = new Mesh { name = "Blood water follows inner bowl" };
            var wv = new List<Vector3> { Vector3.zero };
            var wt = new List<int>();
            for (int i = 0; i < 64; i++)
            {
                float a = i * Mathf.PI * 2 / 64;
                wv.Add(new Vector3(
                    Mathf.Sign(Mathf.Cos(a)) * Mathf.Pow(Mathf.Abs(Mathf.Cos(a)), .60f) * .74f,
                    0,
                    Mathf.Sign(Mathf.Sin(a)) * Mathf.Pow(Mathf.Abs(Mathf.Sin(a)), .64f) * .285f));
            }
            for (int i = 0; i < 64; i++) wt.AddRange(new[] { 0, (i + 1) % 64 + 1, i + 1 });
            waterMesh.SetVertices(wv); waterMesh.SetTriangles(wt, 0);
            waterMesh.RecalculateNormals(); waterMesh.RecalculateBounds();
            water.GetComponent<MeshFilter>().sharedMesh = StoreMesh(waterMesh, "BathBloodSurface");
            water.transform.localScale = Vector3.one;

            // Bug 39: Abundant abandoned family feast across LongDiningTable (center serving pots, soup bowls, jug, tray, 12 place settings!)
            foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t =>
                t && (t.name.StartsWith("AddedDinner_") || t.name.StartsWith("AddedDish_") || t.name.StartsWith("DiningCenter_") ||
                      t.name == "MouldyPlate" || t.name.StartsWith("RottenDinner_") || t.name.StartsWith("DeadRatOnPlate_"))).ToArray())
            {
                if (t) Object.DestroyImmediate(t.gameObject);
            }
            for (int i = 0; i < 12; i++)
            {
                float x = i % 2 == 0 ? 3.28f : 4.15f, depth = 3.0f + (i / 2) * .61f;
                var p = d.P(x, depth, 1.49f);
                var food = DesignerReviewBuilder.ReviewModel("AddedDinner_" + i, i % 3 == 0 ? "RottenApple" : "MoldyBread", root, p + Vector3.up * .04f, new Vector3(.16f, .08f, .14f), false);
                food.transform.Rotate(Vector3.up, i * 37);
                KosVisualV2.SetCenter(food, p + Vector3.up * .04f);
                MeshObject("AddedDish_" + i, root, p, Plate(), Material("Repair_Ceramic"));
            }
            // Center-spine communal serving vessels along x = 3.715, depth = 3.15 .. 5.95
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities/pot_enamel_01/pot_enamel_01.fbx", "DiningCenter_EnamelPot", root,
                d.P(3.71f, 3.30f, 1.49f), Quaternion.Euler(0, 18, 0), new Vector3(.30f, .21f, .26f));
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/ceramic_pot/ceramic_pot.fbx", "DiningCenter_CeramicTureen", root,
                d.P(3.72f, 4.15f, 1.49f), Quaternion.Euler(0, 75, 0), new Vector3(.26f, .22f, .26f));
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities/metal_jug/metal_jug.fbx", "DiningCenter_WaterJug", root,
                d.P(3.70f, 4.85f, 1.49f), Quaternion.Euler(0, -40, 0), new Vector3(.19f, .26f, .19f));
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/brass_pan_01/brass_pan_01.fbx", "DiningCenter_ServingPan", root,
                d.P(3.72f, 5.55f, 1.49f), Quaternion.Euler(0, 110, 0), new Vector3(.28f, .08f, .36f), Material("Repair_Brass"));
            for (int bi = 0; bi < 4; bi++)
            {
                ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/InterludeStreet/kenney-food-kit/bowl-soup.fbx", "DiningCenter_SoupBowl_" + bi, root,
                    d.P(3.71f + (bi % 2 == 0 ? -.11f : .11f), 3.72f + bi * .68f, 1.49f), Quaternion.Euler(0, bi * 47, 0), new Vector3(.17f, .075f, .17f), Material("Repair_Ceramic"));
            }

            Save();
            Debug.Log("PROLOG47_DETAILS_APPLIED arterial handprints, 5-finger scratches, compact duct vent, fine chains, abundant dining table");
        }

        public static void ApplyLadder()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var root = Find("Atmaja_Environment_Art").transform;
            Delete("CellarExitLadder"); Delete("CellarLadderRoute"); Delete("ShaftRimCollar");
            for (int k = 0; k < 5; k++) Delete("LadderWallBracket_" + k);

            const string source = "Assets/_LayarTerakhir/assets/Art/Phase3/Imported/Modules/modular_fire_escape/modular_fire_escape_ladder_bottom.fbx";
            var ladder = new GameObject("CellarExitLadder");
            ladder.transform.SetParent(root, true);
            var body = (GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(source));
            body.transform.SetParent(ladder.transform, false);
            ladder.transform.rotation = Quaternion.LookRotation(d.north);
            foreach (var r in ladder.GetComponentsInChildren<Renderer>())
                r.sharedMaterials = Enumerable.Repeat(Material("Repair_Iron"), r.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(ladder, new Vector3(.64f, 6.66f, .075f));
            KosVisualV2.SetCenter(ladder, d.P(1.025f, .64f, .55f));

            // Bug 43: Iron wall-anchor brackets tying the ladder rails flush into the north shaft masonry + masonry rim collar sealing upper floor gap!
            for (int bi = 0; bi < 4; bi++)
            {
                float by = -1.8f + bi * 1.65f;
                var brk = GameObject.CreatePrimitive(PrimitiveType.Cube);
                brk.name = "LadderWallBracket_" + bi;
                brk.transform.SetParent(ladder.transform, true);
                Object.DestroyImmediate(brk.GetComponent<Collider>());
                brk.transform.SetPositionAndRotation(d.P(1.025f, .58f, by), Quaternion.LookRotation(d.north));
                brk.transform.localScale = new Vector3(.66f, .045f, .14f);
                brk.GetComponent<Renderer>().sharedMaterial = Material("Repair_Iron");
            }
            var collar = GameObject.CreatePrimitive(PrimitiveType.Cube);
            collar.name = "ShaftRimCollar";
            collar.transform.SetParent(root, true);
            Object.DestroyImmediate(collar.GetComponent<Collider>());
            collar.transform.SetPositionAndRotation(d.P(1.025f, .52f, 3.68f), Quaternion.LookRotation(d.north));
            collar.transform.localScale = new Vector3(1.15f, .34f, .18f);
            collar.GetComponent<Renderer>().sharedMaterial = Old("House_MAT_BrickBase") ?? Material("Repair_Iron");

            var route = new GameObject("CellarLadderRoute").transform;
            route.SetParent(d.transform, false);
            var points = new List<Transform>();
            var positions = new[]
            {
                d.upper.trapdoorTop.position,
                d.P(1.025f, 1.18f, 3.50f),
                d.P(1.025f, 1.18f, 2.5f),
                d.P(1.025f, 1.18f, 1.5f),
                d.P(1.025f, 1.18f, .5f),
                d.P(1.025f, 1.18f, -.5f),
                d.P(1.025f, 1.18f, -1.5f),
                d.P(1.025f, 1.18f, -2.78f),
                d.basement.spawnBasement.position
            };
            for (int i = 0; i < positions.Length; i++)
            {
                var p = new GameObject("LadderClimb_" + i).transform;
                p.SetParent(route, true);
                p.position = positions[i];
                points.Add(p);
            }
            d.upper.cellarLadderRoute = points.ToArray();

            var cut = new Bounds(new Vector3(1.025f, 1.61f, 1.4f), new Vector3(.95f, 4.45f, 1.70f));
            foreach (var f in root.GetComponentsInChildren<MeshFilter>().Where(f => f.name == "DiningDirtyTile" || f.name == "CellarCeiling" || f.name == "GroundFloorCeiling_1" || f.name == "Static__House_MAT_BrickBase" || f.name == "Static__House_MAT_Concrete").ToArray())
                Prolog47MeshRepair.Subtract(f, d, cut, "ShaftOpening_" + f.name);

            var priorCollision = Find("LadderShaftFloorCollision");
            if (priorCollision)
                foreach (var piece in priorCollision.GetComponentsInChildren<BoxCollider>())
                    foreach (var original in root.GetComponentsInChildren<BoxCollider>().Where(b => b.name + "_OutsideShaft" == piece.name && !b.transform.IsChildOf(priorCollision.transform)))
                        original.enabled = true;
            Delete("LadderShaftFloorCollision");
            var collision = new GameObject("LadderShaftFloorCollision").transform;
            collision.SetParent(root, true);
            foreach (var box in root.GetComponentsInChildren<BoxCollider>().ToArray())
            {
                var b = box.bounds;
                if (!box.enabled || b.size.x < 2 || b.size.z < 2 || b.size.y > 1.4f || b.min.y < -.61f || b.max.y > 3.91f) continue;
                var center = d.LocalPosition(b.center);
                float x0 = center.x - b.extents.x, x1 = center.x + b.extents.x, z0 = center.y - b.extents.z, z1 = center.y + b.extents.z;
                const float cx0 = .55f, cx1 = 1.5f, cz0 = .55f, cz1 = 2.25f;
                if (x1 <= cx0 || x0 >= cx1 || z1 <= cz0 || z0 >= cz1) continue;
                box.enabled = false;
                void Part(float a, float c, float e, float f)
                {
                    if (c - a < .001f || f - e < .001f) return;
                    var go = new GameObject(box.name + "_OutsideShaft", typeof(BoxCollider));
                    go.transform.SetParent(collision, true);
                    go.transform.SetPositionAndRotation(d.P((a + c) / 2, (e + f) / 2, b.center.y), Quaternion.LookRotation(d.north));
                    go.GetComponent<BoxCollider>().size = new Vector3(c - a, b.size.y, f - e);
                }
                Part(x0, Mathf.Min(cx0, x1), z0, z1);
                Part(Mathf.Max(cx1, x0), x1, z0, z1);
                Part(Mathf.Max(x0, cx0), Mathf.Min(x1, cx1), z0, Mathf.Min(cz0, z1));
                Part(Mathf.Max(x0, cx0), Mathf.Min(x1, cx1), Mathf.Max(cz1, z0), z1);
            }
            var cabinet = Find("FamilyRadioCabinet");
            if (cabinet && d.LocalPosition(cabinet.transform.position).x > 0)
                foreach (var t in root.GetComponentsInChildren<Transform>().Where(t => t.name == "FamilyRadioCabinet" || t.name == "FamilyRadioBody" || t.name == "FamilyCabinetPanel" || t.name == "RadioSpeakerGrille"))
                    t.position -= d.east * 1.9f;

            Save();
            Debug.Log("PROLOG47_LADDER_APPLIED visible anchored iron ladder, sealed shaft collar and matching climb route");
        }

        private static Material DecalMaterial(string name, bool scratch, bool spatter = false)
        {
            string path = Mats + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!mat) { mat = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(mat, path); }
            mat.SetFloat("_BlendModePreserveSpecular", 0);
            mat.SetFloat("_SpecularHighlights", 0);
            mat.SetFloat("_EnvironmentReflections", 0);
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.EnableKeyword("_SPECULARHIGHLIGHTS_OFF");
            mat.EnableKeyword("_ENVIRONMENTREFLECTIONS_OFF");
            mat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(
                spatter ? "Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities/Blood/blood_splatter_01_base.png"
                        : Texs + "HandprintAndScratches.png"));
            mat.SetTextureScale("_BaseMap", spatter ? Vector2.one : new Vector2(scratch ? .385f : .61f, 1));
            mat.SetTextureOffset("_BaseMap", new Vector2(scratch ? .615f : 0, 0));
            // Keep true arterial crimson blood colour from HandprintAndScratches.png instead of muddying into brown-black!
            mat.SetColor("_BaseColor", spatter ? new Color(.58f, .10f, .09f) : Color.white);
            mat.SetFloat("_Surface", 1);
            mat.SetFloat("_SrcBlend", 5);
            mat.SetFloat("_DstBlend", 10);
            mat.SetFloat("_ZWrite", 0);
            mat.SetFloat("_Cull", 0);
            mat.SetFloat("_Smoothness", .18f);
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = 3000;
            mat.SetOverrideTag("RenderType", "Transparent");
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static GameObject Quad(string name, Transform root, Vector3 pos, Quaternion rotation, Vector2 size, Material mat)
        {
            Delete(name);
            var q = GameObject.CreatePrimitive(PrimitiveType.Quad);
            q.name = name;
            q.transform.SetParent(root, true);
            q.transform.SetPositionAndRotation(pos, rotation);
            q.transform.localScale = new Vector3(size.x, size.y, 1);
            Object.DestroyImmediate(q.GetComponent<Collider>());
            q.GetComponent<Renderer>().sharedMaterial = mat;
            q.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            return q;
        }

        private static Mesh StoreMesh(Mesh mesh, string name)
        {
            Directory.CreateDirectory("Assets/_LayarTerakhir/assets/Art/Generated/Models/Prolog47");
            string path = "Assets/_LayarTerakhir/assets/Art/Generated/Models/Prolog47/" + name + ".asset";
            var prior = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (prior) { EditorUtility.CopySerialized(mesh, prior); Object.DestroyImmediate(mesh); return prior; }
            AssetDatabase.CreateAsset(mesh, path);
            return mesh;
        }

        private static GameObject MeshObject(string name, Transform parent, Vector3 position, Mesh mesh, Material mat)
        {
            Delete(name);
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(parent, true);
            go.transform.position = position;
            go.GetComponent<MeshFilter>().sharedMesh = mesh;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            return go;
        }

        private static Mesh Plate()
        {
            string path = "Assets/_LayarTerakhir/assets/Art/Generated/Models/Prolog47/ConcaveDish.asset";
            var existing = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (existing) return existing;
            var v = new List<Vector3>();
            var t = new List<int>();
            var profile = new[] { new Vector2(0, 0), new Vector2(.075f, 0), new Vector2(.115f, .008f), new Vector2(.14f, .025f), new Vector2(.143f, .021f), new Vector2(.11f, .002f), new Vector2(.07f, -.008f), new Vector2(0, -.008f) };
            foreach (var ring in profile) for (int i = 0; i < 48; i++) { float a = i * Mathf.PI * 2 / 48; v.Add(new Vector3(Mathf.Cos(a) * ring.x, ring.y, Mathf.Sin(a) * ring.x)); }
            for (int j = 0; j < profile.Length - 1; j++) for (int i = 0; i < 48; i++) { int a = j * 48 + i, b = j * 48 + (i + 1) % 48; t.AddRange(new[] { a, a + 48, b + 48, a, b + 48, b }); }
            var mesh = new Mesh { name = "Turned enamel dish" };
            mesh.SetVertices(v); mesh.SetTriangles(t, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
            return StoreMesh(mesh, "ConcaveDish");
        }

        private static Mesh BuildChainMesh(string storeName, Vector3 along, Vector3 sag, float length)
        {
            var v = new List<Vector3>();
            var t = new List<int>();
            int links = Mathf.RoundToInt(length / .04f);
            Vector3 up = Mathf.Abs(Vector3.Dot(along.normalized, Vector3.up)) > .9f ? Vector3.forward : Vector3.up;
            for (int link = 0; link < links; link++)
            {
                float ratio = link / (float)Mathf.Max(1, links - 1);
                var c = along * ((ratio - .5f) * length) + sag * (1 - Mathf.Pow(ratio * 2 - 1, 2));
                var side = Quaternion.AngleAxis(link % 2 * 90, along) * up;
                int start = v.Count;
                for (int i = 0; i < 20; i++)
                {
                    float a = i * Mathf.PI * 2 / 20;
                    Vector3 ring = along * (Mathf.Cos(a) * .024f) + side * (Mathf.Sin(a) * .0115f);
                    Vector3 radial = (along * Mathf.Cos(a) + side * Mathf.Sin(a)).normalized;
                    Vector3 normal = Vector3.Cross(along, side).normalized;
                    for (int k = 0; k < 6; k++)
                    {
                        float b = k * Mathf.PI * 2 / 6;
                        v.Add(c + ring + (radial * Mathf.Cos(b) + normal * Mathf.Sin(b)) * .0032f);
                    }
                }
                for (int i = 0; i < 20; i++)
                    for (int k = 0; k < 6; k++)
                    {
                        int a = start + i * 6 + k, b = start + ((i + 1) % 20) * 6 + k, c0 = start + ((i + 1) % 20) * 6 + (k + 1) % 6, e = start + i * 6 + (k + 1) % 6;
                        t.AddRange(new[] { a, b, c0, a, c0, e });
                    }
            }
            var mesh = new Mesh { name = storeName + " links in metres" };
            mesh.SetVertices(v); mesh.SetTriangles(t, 0); mesh.RecalculateNormals(); mesh.RecalculateBounds();
            return StoreMesh(mesh, storeName);
        }

        private static void Chain(string name, Transform parent, Vector3 position, Vector3 along, Vector3 sag, float length)
        {
            MeshObject(name, parent, position, BuildChainMesh(name + "Links", along, sag, length), Old("House_MAT_RustMetal"));
        }

        private static GameObject Imported(string id, string name, Transform root, Vector3 foot, Vector3 size, Quaternion rotation)
        {
            foreach (var prior in root.GetComponentsInChildren<Transform>(true).Where(t => t.name == name).ToArray())
                Object.DestroyImmediate(prior.gameObject);
            string folder = "Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities/" + id + "/";
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(folder + id + ".fbx");
            if (!asset) throw new InvalidOperationException("Missing CC0 source " + id);
            var go = new GameObject(name);
            go.transform.SetParent(root, false);
            go.transform.localRotation = rotation;
            var child = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            child.transform.SetParent(go.transform, false);
            bool isPorcelain = id == "Toilet_Round_A" || id == "Sink_A";
            foreach (var renderer in child.GetComponentsInChildren<Renderer>())
            {
                var materials = renderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    string key = id == "Toilet_Round_A" ? (materials[i] && materials[i].name.Contains("Seat") ? "Toilet_Round_Seat" : "Toilet_Round") : id;
                    string diffuse = folder + key + (isPorcelain ? "_Diffuse.png" : "_diff_1k.jpg");
                    var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(diffuse);
                    string path = Mats + "CC0_" + key + ".mat";
                    var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                    if (!mat) { mat = new Material(Shader.Find("Universal Render Pipeline/Lit")); AssetDatabase.CreateAsset(mat, path); }
                    // Toilet_Round_Diffuse.png contains unbaked RGB(0,0,0) black UV rectangles on the tank;
                    // use pure glossy URP porcelain albedo + normal map for Toilet_Round_A so zero black rectangles appear!
                    bool useTex = texture && id != "Toilet_Round_A";
                    mat.SetTexture("_BaseMap", useTex ? texture : null);
                    var normal = AssetDatabase.LoadAssetAtPath<Texture2D>(folder + key + (isPorcelain ? "_Normal.png" : "_nor_gl_1k.jpg"));
                    if (normal) { mat.SetTexture("_BumpMap", normal); mat.EnableKeyword("_NORMALMAP"); mat.SetFloat("_BumpScale", .45f); }

                    if (isPorcelain)
                    {
                        // Bug 9: NEVER plug HDRP _Mask.png directly into URP _MetallicGlossMap/_OcclusionMap (caused black rectangular holes on toilet tank!)
                        mat.SetTexture("_MetallicGlossMap", null);
                        mat.SetTexture("_OcclusionMap", null);
                        mat.DisableKeyword("_METALLICSPECGLOSSMAP");
                        mat.DisableKeyword("_OCCLUSIONMAP");
                        mat.SetColor("_BaseColor", useTex ? new Color(.94f, .94f, .92f) : new Color(.91f, .91f, .88f));
                        mat.SetFloat("_Metallic", 0f);
                        mat.SetFloat("_Smoothness", key.Contains("Seat") ? .62f : .84f);
                    }
                    else
                    {
                        mat.SetColor("_BaseColor", texture ? Color.white : new Color(.64f, .65f, .62f));
                        mat.SetFloat("_Metallic", id == "Shower" ? .82f : 0f);
                        mat.SetFloat("_Smoothness", id == "Shower" ? .72f : .35f);
                        var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(folder + key + "_mask.png");
                        if (mask)
                        {
                            mat.SetTexture("_MetallicGlossMap", mask);
                            mat.SetTexture("_OcclusionMap", mask);
                            mat.EnableKeyword("_METALLICSPECGLOSSMAP");
                            mat.EnableKeyword("_OCCLUSIONMAP");
                            mat.SetFloat("_Smoothness", 1f);
                        }
                    }
                    materials[i] = mat;
                    EditorUtility.SetDirty(mat);
                }
                renderer.sharedMaterials = materials;
            }
            KosVisualV2.Resize(go, size);
            var point = root.TransformPoint(foot);
            KosVisualV2.SetCenter(go, point + Vector3.up * size.y * .5f);
            FitCollider(go);
            return go;
        }

        private static void DaySky(string name, Color tint, float exposure)
        {
            const string folder = "Assets/_LayarTerakhir/assets/Resources/";
            Directory.CreateDirectory(folder);
            string path = folder + name + ".mat";
            var sky = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!sky) { sky = new Material(Shader.Find("Skybox/Procedural")); AssetDatabase.CreateAsset(sky, path); }
            sky.SetColor("_SkyTint", tint);
            sky.SetFloat("_Exposure", exposure);
            sky.SetFloat("_AtmosphereThickness", 1.2f);
            EditorUtility.SetDirty(sky);
        }

        private static void Ink(Transform parent, string words, float x)
        {
            var go = new GameObject("PrintedPage", typeof(RectTransform), typeof(Canvas));
            go.transform.SetParent(parent, false);
            go.transform.localPosition = new Vector3(x, .035f, 0);
            go.transform.localRotation = Quaternion.Euler(90, 0, 0);
            go.transform.localScale = Vector3.one * .00035f;
            var rect = go.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(360, 520);
            go.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            var text = new GameObject("Ink", typeof(RectTransform), typeof(Text));
            text.transform.SetParent(go.transform, false);
            var tr = text.GetComponent<RectTransform>();
            tr.anchorMin = Vector2.zero; tr.anchorMax = Vector2.one;
            tr.offsetMin = tr.offsetMax = Vector2.zero;
            var ink = text.GetComponent<Text>();
            ink.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf");
            ink.text = words; ink.fontSize = 27;
            ink.color = new Color(.16f, .11f, .08f);
            ink.raycastTarget = false;
            ink.alignment = TextAnchor.UpperLeft;
        }
    }
}
#endif
