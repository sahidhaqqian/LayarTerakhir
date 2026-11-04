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
                case "Repair_SideTableWood":
                    customTex = "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/wooden_table_02/wooden_table_02_diff_1k.jpg";
                    color = Color.white; smooth = .3f; break;
                case "Repair_ClockWood":
                    customTex = "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/grandfather_clock/vintage_grandfather_clock_01_diff_1k.jpg";
                    color = Color.white; smooth = .3f; break;
                case "Repair_VentRust":
                    customTex = "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/metal_grate_rusty/metal_grate_rusty_diff_1k.jpg";
                    color = Color.white; metal = .38f; smooth = .52f; break;
                case "Repair_Ceramic":
                    color = new Color(.86f, .84f, .78f); smooth = .76f; break;
                case "Repair_CeramicEdge":
                    color = new Color(.22f, .028f, .022f); smooth = .32f; break;
                case "Repair_CupCeramic":
                    color = new Color(.86f, .84f, .77f); smooth = .54f; break;
                case "Repair_CupCrack":
                    color = new Color(.14f, .075f, .045f); smooth = .10f; break;
                case "Repair_RottenCeramic":
                    color = new Color(.52f, .50f, .36f); smooth = .18f; break;
                case "Repair_RotStain":
                    color = new Color(.10f, .085f, .045f); smooth = .75f; break;
                case "Repair_RottenRice":
                    color = new Color(.55f, .53f, .40f); smooth = .35f; break;
                case "Repair_RottenMeat":
                    color = new Color(.30f, .13f, .08f); smooth = .30f; break;
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
                case "BathroomSet_TinyTreats":
                    customTex = "Assets/_LayarTerakhir/assets/Art/Imported/BathroomSet/TinyTreats/tiny_treats_texture_1.png";
                    color = Color.white; smooth = .5f; break;
                case "BathroomSet_Bucket":
                    color = Color.white; smooth = .5f; break;
                case "BathroomSet_Paper":
                    color = new Color(.93f, .92f, .88f); smooth = .55f; break;
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
            ApplyPrologSkin();
            ApplyKosBathroomRound();
            ApplyRumahAtmajaDiningFix();
            ApplyRumahAtmajaAnnexSeal();
            Debug.Log("PROLOG47_COMPLETE_REVISION_APPLIED all 3 Prolog scenes updated and saved");
        }

        public static void ApplyStageOne()
        {
            ConfigureImports(); ApplySimple(); ApplyFurniture();
        }

        public static void ApplyRemaining()
        {
            ConfigureImports(); ApplyFurniture(); ApplyKosFacilities(); ApplyArchitecture(); ApplyDetails(); ApplyLadder(); ApplyFinishProps(); ApplyKosBathroomRound(); ApplyRumahAtmajaDiningFix(); ApplyRumahAtmajaAnnexSeal();
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
            foreach (string root in new[] { "Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities", "Assets/_LayarTerakhir/assets/Art/Imported/BathroomSet", Texs })
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
            // Upgrade Raka's bedroom sandals to Swallow-style IndonesianFlipFlops without dotted sole ring.
            // Idempotent: the upgraded pair root already parents an IndonesianFlipFlops model, so later
            // runs must not delete and recreate it (that churns fileIDs and breaks the opening-scene guard).
            var roomSandals = Find("KosSandals");
            if (roomSandals && !roomSandals.GetComponentsInChildren<Transform>(true).Any(t => t && t.name == "IndonesianFlipFlops"))
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
            LayarTerakhir.Player.PickupBlink.EnsureFor(diary, "DOC_PROLOG_04_DIARY_BIMA");
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

            Quaternion rot = root.rotation;
            // Procedural instead of the KosPanelDoor FBX (its headless import
            // yields empty instances, leaving invisible doors): slab plus
            // recessed-look inset panels, wood finish.
            // Panel pintu kost MODERN (biru-abu datar, bukan kayu) sesuai
            // request 5deb9284: KosModernDoor, bukan House_MAT_WoodDoor.
            var woodDoor = AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/KosModernDoor.mat");
            if (!woodDoor)
            {
                woodDoor = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                woodDoor.SetColor("_BaseColor", new Color(.24f, .28f, .31f));
                woodDoor.SetFloat("_Smoothness", .5f);
                AssetDatabase.CreateAsset(woodDoor, "Assets/_LayarTerakhir/assets/Art/Atmaja/Materials/KosModernDoor.mat");
                EditorUtility.SetDirty(woodDoor);
            }
            var vsize = alongZ ? new Vector3(.055f, height, width) : new Vector3(width, height, .055f);
            var visual = new GameObject("ModernDoorVisual"); visual.transform.SetParent(leaf.transform, false);
            visual.transform.SetPositionAndRotation(center, rot);
            void Slab(Vector3 lp, Vector3 s)
            {
                var p = GameObject.CreatePrimitive(PrimitiveType.Cube);
                p.transform.SetParent(visual.transform, false);
                p.transform.localPosition = lp; p.transform.localScale = s;
                if (woodDoor) p.GetComponent<Renderer>().sharedMaterial = woodDoor;
                Object.DestroyImmediate(p.GetComponent<Collider>());
            }
            Slab(Vector3.zero, vsize);
            float faceOff = .0275f + .006f;
            Vector3 faceA = vsize.x < vsize.z ? new Vector3(faceOff, 0, 0) : new Vector3(0, 0, faceOff);
            Vector3 faceS = vsize.x < vsize.z ? new Vector3(.012f, height * .78f, Mathf.Max(width - .12f, .3f)) : new Vector3(Mathf.Max(width - .12f, .3f), height * .78f, .012f);
            Slab(faceA, faceS); Slab(-faceA, faceS);

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

            // Toilets back their tanks against the east wall so the bowl opening faces WEST into the cubicle doorway.
            // (Native bowl faces -Z; yaw +90 turns it to -X. Yaw -90 was proven reversed in player captures.)
            foreach (float z in new[] { 3.30f, 5.20f })
            {
                Imported("Toilet_Round_A", "KosPorcelainToilet_" + z, root, new Vector3(5.52f, 0, z), new Vector3(.45f, .78f, .73f), Quaternion.Euler(0, 90, 0));
                Imported("Shower", "KosWallShower_" + z, root, new Vector3(5.61f, .88f, z - .38f), new Vector3(.26f, 1.32f, .28f), Quaternion.Euler(0, 90, 0));
            }
            Imported("Sink_A", "KosWashBasin", root, new Vector3(2.30f, 0, 5.70f), new Vector3(.53f, .85f, .38f), Quaternion.Euler(0, 180, 0));

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

            // Fridge doors face EAST into the kitchen (forward), never north toward the dining table.
            var fridge = Imported("Fridge", "KosRefrigerator", root, new Vector3(-4.34f, 0, 5.32f), new Vector3(.68f, 1.66f, .68f), Quaternion.Euler(0, 180, 0));
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
                    // 1-to-1 weight/bone transfer requires identical topology.
                    // A repair sculpt baked for an older rig must never be forced
                    // onto a re-rigged hunter (exploded skinning, blown budget).
                    if (source.sharedMesh.vertexCount != target.sharedMesh.vertexCount)
                    {
                        Debug.LogWarning("PROLOG47_SKIN_SKIPPED repair sculpt " + source.sharedMesh.vertexCount + " verts does not match hunter " + target.sharedMesh.vertexCount + " verts; keeping designer surface");
                        continue;
                    }
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
            // Bug 10: KosKitchenWashBasin backs against the z = 2.5 wall (never floating mid-room).
            Imported("Sink_A", "KosKitchenWashBasin", root, new Vector3(-3.15f, 0, 2.85f), new Vector3(.53f, .85f, .44f), Quaternion.identity);
            Imported("Sink_A", "KosWashBasin", root, new Vector3(2.30f, 0, 5.70f), new Vector3(.53f, .85f, .38f), Quaternion.Euler(0, 180, 0));
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
            // The Bug-42 sweep above also removes the WIRED chase egress
            // floor (PrologCompletionBuilder.ChaseLayout). Restore its exact
            // shape, material and wiring so the finale keeps working.
            {
                var egress = GameObject.CreatePrimitive(PrimitiveType.Cube);
                egress.name = "CellarClosingStairLanding";
                egress.transform.SetParent(environment, true);
                egress.transform.SetPositionAndRotation(d.P(1.0f, 1.4f, 3.75f), Quaternion.identity);
                egress.transform.localScale = new Vector3(.9f, .12f, 1.9f);
                egress.GetComponent<Renderer>().sharedMaterial = Old("House_MAT_WoodDoor");
                egress.SetActive(false);
                if (d.chase) d.chase.cellarEgressFloor = egress;
            }

            // Scatter varied CC0 wooden/military crates in cellar so basement doesn't look sparse or cloned.
            // Designer Nov: the military crate stays out of the eastern
            // north-south lane (tucked by the north-east crates) so Raka can
            // walk from the middle of the room to the second wardrobe.
            // It sits against CellarExtraCrate_1, clear of lane-main x4.2-5.
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "CellarExtraCrate_1", environment,
                d.P(4.6f, 6.2f, -2.79f), Quaternion.Euler(0, 22, 0), new Vector3(.68f, .52f, .52f), Material("Repair_BedWood"));
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/old_military_crate/old_military_crate.fbx", "CellarExtraCrate_2", environment,
                d.P(4.6f, 5.6f, -2.79f), Quaternion.Euler(0, -18, 0), new Vector3(.76f, .44f, .48f), Material("Repair_BedWood"));

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
                // Keep the east-side walk lane capsule-clear: the pair must
                // flank the escape-window view with a ~1.5 m gap, never gate it.
                ProjectAsset("Assets/_LayarTerakhir/assets/Art/Review/Models/LodShrub.fbx", "EscapeWindowCourtyardShrub_1", environment,
                    outPos + Vector3.right * 1.6f, Quaternion.Euler(0, 35, 0), new Vector3(1.25f, .95f, 1.25f));
                ProjectAsset("Assets/_LayarTerakhir/assets/Art/Review/Models/LodShrub.fbx", "EscapeWindowCourtyardShrub_2", environment,
                    outPos - Vector3.right * 1.7f, Quaternion.Euler(0, 140, 0), new Vector3(1.35f, 1.05f, 1.35f));
                // The LOD shrub pivot sits off-centre, so FitBox wraps foliage
                // that leans over the walk lane (capsule radius .32). Keep the
                // woody core solid and let the soft foliage edge stay passable,
                // then walk the root west until the lane AABB test clears.
                var shrub2 = Find("EscapeWindowCourtyardShrub_2");
                var shrubCol = shrub2 ? shrub2.GetComponent<BoxCollider>() : null;
                if (shrubCol)
                {
                    shrubCol.center = new Vector3(shrubCol.center.x * .5f, shrubCol.center.y, shrubCol.center.z * .5f);
                    var tight = shrubCol.size; tight.x *= .62f; tight.z *= .62f; shrubCol.size = tight;
                    int guard = 0;
                    while (guard++ < 8 && shrubCol.bounds.max.x > -8.62f) shrub2.transform.position += Vector3.right * -.12f;
                    EditorUtility.SetDirty(shrub2);
                }
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

        [MenuItem("LayarTerakhir/Prolog/Apply well cover only")]
        public static void ApplyWellCoverOnly()
        {
            // ONLY the courtyard well: round jointed cover + fine chain.
            // Nothing else is touched.
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var well = Find("WellCover");
            if (!well) throw new InvalidOperationException("Missing WellCover holder");
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
            Save();
            Debug.Log("WELL_COVER_APPLIED round cover + fine chain only");
        }

        [MenuItem("LayarTerakhir/Prolog/Diagnose decal")]
        public static void DiagnoseDecal()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture/CrimsonGoalDecal.png");
            if (!tex) throw new InvalidOperationException("decal texture missing");
            Debug.Log("DECAL_TEX w=" + tex.width + " h=" + tex.height + " format=" + tex.format);
            var q = GameObject.Find("CrimsonScratchRelief");
            var r = q ? q.GetComponent<Renderer>() : null;
            Debug.Log("DECAL_QUAD found=" + (q != null) + " pos=" + (q ? q.transform.position.ToString() : "?") + " mat=" + (r && r.sharedMaterial ? r.sharedMaterial.name : "?") + " queue=" + (r && r.sharedMaterial ? r.sharedMaterial.renderQueue : -1));
            if (r) Debug.Log("DECAL_BOUNDS center=" + r.bounds.center + " size=" + r.bounds.size + " visible=" + r.isVisible + " meshVerts=" + (q.GetComponent<MeshFilter>().sharedMesh ? q.GetComponent<MeshFilter>().sharedMesh.vertexCount : -1) + " mainTex=" + (r.sharedMaterial && r.sharedMaterial.mainTexture ? r.sharedMaterial.mainTexture.name + "_" + r.sharedMaterial.mainTexture.width + "x" + r.sharedMaterial.mainTexture.height : "null"));
            // No-save visibility probe: flat unlit red on the same quad.
            if (r)
            {
                var probe = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
                probe.SetColor("_BaseColor", Color.red);
                r.sharedMaterial = probe;
            }
            var camGo = GameObject.Find("ScratchProofCam");
            // No-save capture with the red probe material.
            var target = GameObject.Find("BathScratchTarget");
            var pg = new GameObject("DiagCam", typeof(Camera));
            var pc = pg.GetComponent<Camera>();
            pc.enabled = false; pc.nearClipPlane = .05f; pc.farClipPlane = 120f;
            pc.clearFlags = CameraClearFlags.SolidColor; pc.backgroundColor = Color.black;
            pg.transform.SetPositionAndRotation(target.transform.position + target.transform.forward * 1.05f, Quaternion.LookRotation(-target.transform.forward));
            KosRenderDiagnosis.Capture(pc, "scratch-diag-red", false, "Audit/Visuals/House-Sweep");
            Object.DestroyImmediate(pg);
            // Occlusion test: nudge a scratch-clone toward the room and shoot it.
            var probe2 = GameObject.CreatePrimitive(PrimitiveType.Quad);
            var probeMat2 = new Material(Shader.Find("Universal Render Pipeline/Unlit"));
            probeMat2.SetColor("_BaseColor", Color.green);
            probe2.GetComponent<Renderer>().sharedMaterial = probeMat2;
            probe2.transform.SetPositionAndRotation(q.transform.position + target.transform.forward * 0.30f, q.transform.rotation);
            probe2.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            var pg2 = new GameObject("DiagCam2", typeof(Camera));
            var pc2 = pg2.GetComponent<Camera>();
            pc2.enabled = false; pc2.nearClipPlane = .05f; pc2.farClipPlane = 120f;
            pc2.clearFlags = CameraClearFlags.SolidColor; pc2.backgroundColor = Color.black;
            pg2.transform.SetPositionAndRotation(target.transform.position + target.transform.forward * 1.05f, Quaternion.LookRotation(-target.transform.forward));
            KosRenderDiagnosis.Capture(pc2, "scratch-diag-red-near", false, "Audit/Visuals/House-Sweep");
            Object.DestroyImmediate(pg2); Object.DestroyImmediate(probe2);
            Debug.Log("DECAL_DIAGNOSED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Capture scratch proof")]
        public static void CaptureScratchProof()
        {
            // No-save photo of the bathroom claw wall (goal: cakaran_yang_aku_inginkan.jpg).
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var target = GameObject.Find("BathScratchTarget");
            if (!target) throw new InvalidOperationException("BathScratchTarget missing");
            Vector3 fwd = target.transform.forward;
            Vector3 up = Vector3.up;
            Vector3 side = Vector3.Cross(up, fwd).normalized;
            var shots = new (string, Vector3, Vector3)[]
            {
                ("scratch-proof-wide", target.transform.position + fwd * 1.9f + up * 0.12f, target.transform.position),
                ("scratch-proof-close", target.transform.position + fwd * 1.05f + up * 0.03f, target.transform.position),
                ("scratch-proof-angle", target.transform.position + fwd * 1.4f + side * 1.1f + up * 0.15f, target.transform.position),
            };
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("ScratchProofCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 6f; fill.intensity = 1.4f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("SCRATCH_PROOF_CAPTURED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Capture well proof")]
        public static void CaptureWellProof()
        {
            // No-save photo of the courtyard well (round cover + chain).
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("WellProofCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = 1.6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var target = d.P(2.85f, 11.2f, 1.10f);
            var shots = new (string, Vector3, Vector3)[]
            {
                ("28-well-proof", target + new Vector3(1.3f, 1.1f, 0.6f), target),
                ("29-well-proof-b", target + new Vector3(-1.3f, 1.1f, 0.6f), target),
                ("30-well-proof-c", target + new Vector3(0f, 1.3f, 1.6f), target),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("WELL_PROOF_CAPTURED no-save");
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

            // Designer Nov: the designer's own Meshy crimson-rupture relief
            // replaces the flat scratch decal — real torn ridges under the
            // flashlight, painted to the reference (bloody fan slashes over
            // the bathroom's own tile).
            RebuildScratchRelief(d, root);

            // Bug 24 (revisit, designer Nov): rusted exact-fit crowbar-gated vent.
            RebuildRustedVent(d, root);

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

            // Bug 39 (revisit, designer Nov): the family feast looked fresh and its
            // food floated above the plates. Strip every fresh item and relay a
            // spoiled spread: stained plates, moldy CC0 bread/apple (Audit/
            // designer-review-asset-sources.json), dead CC0 street rats draped
            // over the dishes, spilled rot + a tipped pot down the middle, and
            // black stink-smoke hanging over the table. Nothing floats: plates
            // ground to the tabletop, food and rats ground to their plates.
            foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t =>
                t && (t.name.StartsWith("AddedDinner_") || t.name.StartsWith("AddedDish_") || t.name.StartsWith("DiningCenter_") ||
                      t.name == "MouldyPlate" || t.name.StartsWith("RottenDinner_") || t.name.StartsWith("DeadRatOnPlate_") || t.name == "RottenStinkSmoke")).ToArray())
            {
                if (t) Object.DestroyImmediate(t.gameObject);
            }
            BuildRottenFeast(d, root);

            Save();
            Debug.Log("PROLOG47_DETAILS_APPLIED arterial handprints, 5-finger scratches, compact duct vent, fine chains, abundant dining table");
        }

        private static void BuildRottenFeast(AtmajaDirector d, Transform root)
        {
            const float topY = 1.42f;
            var stain = Material("Repair_RotStain");
            // 12 stained place settings down both sides of the table. Rows start
            // 0.35 inside the table ends: the old 3.0 start sat exactly on the
            // table edge and left the end plates hanging half off the wood.
            for (int i = 0; i < 12; i++)
            {
                float x = i % 2 == 0 ? 3.28f : 4.15f, depth = 3.35f + (i / 2) * .55f;
                var p = d.P(x, depth, 1.49f);
                var plate = MeshObject("AddedDish_" + i, root, p, Plate(), Material("Repair_RottenCeramic"));
                // Designer Nov: ZERO gap — the plate foot sinks 2mm into the
                // wood so it never hovers above the tabletop.
                Ground(plate, topY - .002f);
                float plateTop = SafeBounds(plate).max.y;
                // Rot juice ring on the plate: the food sits IN the stain, never floats.
                var ring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                ring.name = "AddedDish_StainRing_" + i;
                ring.transform.SetParent(root, true);
                Object.DestroyImmediate(ring.GetComponent<Collider>());
                ring.transform.position = new Vector3(p.x, plateTop + .0015f, p.z);
                ring.transform.localScale = new Vector3(.20f, .003f, .20f);
                ring.GetComponent<Renderer>().sharedMaterial = stain;
                // Four visibly different spoiled dishes, not twelve identical ones:
                // moldy bread, rotten apple, stale rice (Kenney CC0 rice-ball),
                // spoiled sausage (Kenney CC0 sausage). Everything grounded to
                // its own plate so nothing floats.
                GameObject food;
                switch (i % 4)
                {
                    case 0: food = DesignerReviewBuilder.ReviewModel("AddedDinner_" + i, "RottenApple", root, p + Vector3.up * .05f, new Vector3(.15f, .08f, .13f), false); break;
                    case 1: food = DesignerReviewBuilder.ReviewModel("AddedDinner_" + i, "MoldyBread", root, p + Vector3.up * .05f, new Vector3(.15f, .08f, .13f), false); break;
                    case 2: food = ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/InterludeStreet/kenney-food-kit/rice-ball.fbx", "AddedDinner_" + i, root, p + Vector3.up * .05f, Quaternion.Euler(0, i * 37, 0), new Vector3(.11f, .07f, .11f), Material("Repair_RottenRice")); break;
                    default: food = ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/InterludeStreet/kenney-food-kit/sausage.fbx", "AddedDinner_" + i, root, p + Vector3.up * .05f, Quaternion.Euler(0, i * 37, 0), new Vector3(.16f, .05f, .06f), Material("Repair_RottenMeat")); break;
                }
                if (!food) throw new Exception("Rotten feast food missing for seat " + i);
                // ProjectAsset fits a BoxCollider; the feast carries none (keeps
                // interaction rays and the crawl route clean, like before).
                foreach (var c in food.GetComponentsInChildren<Collider>(true)) Object.DestroyImmediate(c);
                food.transform.Rotate(Vector3.up, i * 37);
                // Designer Nov: ZERO gap — food sinks 1mm into its own plate
                // so no daylight shows between the meal and the dish.
                Ground(food, plateTop - .001f);
            }
            // Rat carcasses draped over five of the dishes + two down the middle.
            int[] ratSeats = { 1, 4, 6, 9, 11 };
            foreach (int i in ratSeats)
            {
                float x = i % 2 == 0 ? 3.28f : 4.15f, depth = 3.35f + (i / 2) * .55f;
                var p = d.P(x, depth, 1.49f);
                var plate = Find("AddedDish_" + i);
                float rest = plate ? SafeBounds(plate).max.y - .001f : topY;
                var rat = DesignerReviewBuilder.ReviewModel("AddedDinner_Rat_" + i, "DeadStreetRat", root, p + Vector3.up * .06f, new Vector3(.20f, .06f, .09f), false);
                rat.transform.Rotate(Vector3.up, i * 53 + 20);
                // Belly-up: roll so the carcass reads dead, not parked.
                rat.transform.Rotate(Vector3.forward, 155);
                Ground(rat, rest);
            }
            // Middle of the table: spilled rot, a tipped-over pot, more carcasses.
            for (int k = 0; k < 3; k++)
            {
                var spill = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                spill.name = "AddedDish_Spill_" + k;
                spill.transform.SetParent(root, true);
                Object.DestroyImmediate(spill.GetComponent<Collider>());
                spill.transform.position = d.P(3.71f + (k - 1) * .12f, 3.55f + k * 1.05f, topY);
                spill.transform.localScale = new Vector3(.30f - k * .05f, .004f, .24f - k * .03f);
                spill.GetComponent<Renderer>().sharedMaterial = stain;
                Ground(spill, topY - .001f);
            }
            var pot = ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/Prolog47Facilities/pot_enamel_01/pot_enamel_01.fbx", "AddedDish_TippedPot", root,
                d.P(3.71f, 4.35f, 1.49f), Quaternion.Euler(0, 18, 0), new Vector3(.30f, .21f, .26f));
            if (pot)
            {
                // Knocked over: lying on its side in the middle spill.
                pot.transform.rotation = Quaternion.Euler(0, 18, 90) * Quaternion.LookRotation(d.north);
                Ground(pot, topY - .002f);
            }
            for (int k = 0; k < 2; k++)
            {
                var p = d.P(3.68f + k * .22f, 5.30f + k * .55f, 1.49f);
                var rat = DesignerReviewBuilder.ReviewModel("AddedDinner_RatCenter_" + k, "DeadStreetRat", root, p + Vector3.up * .06f, new Vector3(.20f, .06f, .09f), false);
                rat.transform.Rotate(Vector3.up, k * 120 + 40);
                rat.transform.Rotate(Vector3.forward, 155);
                Ground(rat, topY + .001f);
            }
            // Designer Nov: SMALL stink-wisps on 2 dishes only — not a big
            // plume over the whole table. Each wisp sits just above its own
            // plate (≈12-20cm puffs, slow rise) so the rest stays clear.
            Delete("RottenStinkSmoke");
            Delete("RottenStinkSmoke_0");
            Delete("RottenStinkSmoke_1");
            var mist = AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Phase2/Materials/OperationMist.mat");
            if (!mist) throw new Exception("OperationMist material missing for rotten smoke");
            // The Kenney wisp is too faint for a stink plume (mean alpha <10%),
            // so the smoke gets its own dense procedural puff (generated PNG,
            // no authored model) over the same particle shader.
            var puffPath = "Assets/_LayarTerakhir/assets/Art/Generated/Materials/Prolog47/Repair_SmokePuff.mat";
            var puff = AssetDatabase.LoadAssetAtPath<Material>(puffPath);
            if (!puff)
            {
                puff = new Material(mist);
                AssetDatabase.CreateAsset(puff, puffPath);
            }
            var puffTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47/SmokePuff.png");
            if (!puffTex) throw new Exception("SmokePuff texture missing");
            puff.SetTexture("_BaseMap", puffTex);
            EditorUtility.SetDirty(puff);
            // Two smoking dishes: stale rice (seat 2) + spoiled sausage
            // (seat 7). Both are rat-free so the wisp reads cleanly.
            int[] smokeSeats = { 2, 7 };
            for (int s = 0; s < smokeSeats.Length; s++)
            {
                int i = smokeSeats[s];
                float sx = i % 2 == 0 ? 3.28f : 4.15f, sdepth = 3.35f + (i / 2) * .55f;
                var smoke = new GameObject(s == 0 ? "RottenStinkSmoke" : "RottenStinkSmoke_" + s);
                smoke.transform.SetParent(root, true);
                smoke.transform.position = d.P(sx, sdepth, topY + .20f);
                var ps = smoke.AddComponent<ParticleSystem>();
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                var main = ps.main;
                main.loop = true; main.playOnAwake = true; main.duration = 2.5f;
                main.startLifetime = new ParticleSystem.MinMaxCurve(1.2f, 1.9f);
                main.startSpeed = new ParticleSystem.MinMaxCurve(.02f, .05f);
                main.startSize = new ParticleSystem.MinMaxCurve(.10f, .20f);
                // Near-black but not pure black: the wisp must read against the
                // lit cloth. No color-over-lifetime multiplier (it crushed the
                // tint to invisible); the puff texture's own falloff shapes it.
                main.startColor = new Color(.13f, .13f, .13f, .75f);
                main.maxParticles = 16;
                main.simulationSpace = ParticleSystemSimulationSpace.World;
                // Gentle buoyant rise: too strong a negative gravity fires the
                // wisp metres above the table and out of every frame.
                main.gravityModifier = -.015f;
                var emission = ps.emission;
                emission.rateOverTime = 4f;
                var shape = ps.shape;
                shape.shapeType = ParticleSystemShapeType.Box;
                shape.scale = new Vector3(.12f, .06f, .12f);
                // No color-over-lifetime: its dark gradient multiplied every tint
                // to invisible, even opaque white. The puff alpha falloff alone
                // shapes each wisp.
                var pr = smoke.GetComponent<ParticleSystemRenderer>();
                pr.sharedMaterial = puff;
                pr.renderMode = ParticleSystemRenderMode.Billboard;
                // Zero sorting offset: a large fudge re-sorted the plume behind
                // the room and hid it from every capture.
                pr.sortingFudge = 0;
            }
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

            // The concrete shell is owned by the facade band (exterior kept, phantom
            // interior floor opened) with this same shaft hole cut into it; the
            // ladder loop must not replace its cutout mesh reference.
            var cut = new Bounds(new Vector3(1.025f, 1.61f, 1.4f), new Vector3(.95f, 4.45f, 1.70f));
            foreach (var f in root.GetComponentsInChildren<MeshFilter>().Where(f => f.name == "DiningDirtyTile" || f.name == "CellarCeiling" || f.name == "GroundFloorCeiling_1" || f.name == "Static__House_MAT_BrickBase").ToArray())
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

            // Designer Nov: the basement needs a way back up outside the
            // chase — a visible iron grab-handle at the ladder base. The
            // chase finale keeps its own urgent climb; this one is for
            // free exploration. Idempotent.
            Delete("CellarClimbHandle");
            var grip = GameObject.CreatePrimitive(PrimitiveType.Cube);
            grip.name = "CellarClimbHandle";
            grip.transform.SetParent(root, true);
            grip.transform.SetPositionAndRotation(d.P(1.025f, 0.7f, -1.9f), Quaternion.LookRotation(d.north));
            grip.transform.localScale = new Vector3(.34f, .44f, .08f);
            grip.GetComponent<Renderer>().sharedMaterial = Material("Repair_Iron");
            var climbItem = grip.AddComponent<AtmajaItem>();
            climbItem.director = d; climbItem.action = AtmajaAction.CellarClimb; climbItem.label = "Naiki tangga besi";

            RebuildCellarWalkability(d, root);

            // The shaft cut above replaces the concrete mesh (losing the
            // facade-cutout reference); rebuild the band from the pristine FBX
            // and cut the same shaft opening into the SAME cutout asset.
            AtmajaStairRevisionBuilder.ReapplyFacadeBand(d, cut);
            Save();
            Debug.Log("PROLOG47_LADDER_APPLIED visible anchored iron ladder, sealed shaft collar and matching climb route");
        }

        // Designer Nov: shave the invisible sewing-bench collision back from
        // the spawn approach (visual bench, machine and powerbank stay put)
        // so walking to Damar's powerbank never hits thin air. Idempotent.
        private static void RebuildCellarWalkability(AtmajaDirector d, Transform root)
        {
            var top = Find("BenchTopCollision");
            if (top)
            {
                top.transform.SetPositionAndRotation(d.P(3.85f, 6.575f, top.transform.position.y), Quaternion.LookRotation(d.north));
                var s = top.transform.localScale; s.x = 1.9f; top.transform.localScale = s;
            }
            foreach (var leg in FindAll("BenchLeg"))
            {
                var lp = d.LocalPosition(leg.transform.position);
                if (lp.x < 2.85f) leg.transform.position += d.east * (3.0f - lp.x);
            }
            // Same military-crate tuck as the architecture scatter: keep the
            // eastern north-south lane open even when only the ladder pass runs.
            var mil = Find("CellarExtraCrate_2");
            if (mil) mil.transform.SetPositionAndRotation(d.P(4.15f, 4.75f, -2.79f), Quaternion.Euler(0, -18, 0));
        }

        private static Material DecalMaterial(string name, bool scratch, bool spatter = false)        {
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

        private static GameObject ImportedBath(string file, string name, Transform root, Vector3 foot, Vector3 size, Quaternion rotation, Material overrideMat)
        {
            foreach (var prior in root.GetComponentsInChildren<Transform>(true).Where(t => t.name == name).ToArray())
                Object.DestroyImmediate(prior.gameObject);
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/BathroomSet/" + file);
            if (!asset) throw new InvalidOperationException("Missing CC0 bathroom source " + file);
            var go = new GameObject(name);
            go.transform.SetParent(root, false);
            go.transform.localRotation = rotation;
            var child = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            child.transform.SetParent(go.transform, false);
            if (overrideMat)
                foreach (var renderer in child.GetComponentsInChildren<Renderer>())
                    renderer.sharedMaterials = Enumerable.Repeat(overrideMat, renderer.sharedMaterials.Length).ToArray();
            KosVisualV2.Resize(go, size);
            var point = root.TransformPoint(foot);
            KosVisualV2.SetCenter(go, point + Vector3.up * size.y * .5f);
            FitCollider(go);
            return go;
        }

        // Designer round Nov 2026 (goal.md): leftover dining wall, open bathroom foyer
        // with only PRIA/WANITA doors, sealed main entrance, expanded natural bathrooms.
        // goal.md Nov (Rumah Atmaja): the LongDiningTable FBX instance renders as a tiny
        // chair-sized blob, so the Bug-39 feast floats in mid-air ("meja gak ke load").
        // Replace the visual with a procedural top+legs matched to DiningTableTopCollision
        // (surface y=1.42), reseat every feast item onto it, and lift the brass clock
        // hands (plus ashtray) out of the CornerSideTable volume ("jarum jam" hidden).
        // Idempotent: deletes its own visuals first, never touches colliders or items.
        public static void ApplyRumahAtmajaDiningFix()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var env = Find("Atmaja_Environment_Art").transform;
            var wood = Old("House_MAT_WoodDoor");
            const float topY = 1.42f;

            // The bundled LongDiningTable.fbx carries a broken node transform
            // (Lcl Rotation -90 X, Lcl Scaling 100) and imports as a tiny blob.
            // Use the CC0 Poly Haven Dining Table instead (see
            // Audit/dining-table-cc0-sources.json): rustic wood + cloth, resized
            // to the Bug-39 feast footprint (surface y=1.42).
            Delete("LongDiningTable");
            foreach (string n in new[] { "DiningTableTop47", "DiningTableLeg47_0", "DiningTableLeg47_1", "DiningTableLeg47_2", "DiningTableLeg47_3" })
                Delete(n);
            // NOTE: keep the root unrotated and yaw the FBX child instead, so
            // Resize/SetCenter/FitCollider stay axis-aligned with the world.
            // (A BoxCollider fitted under a yawed root maps its extents swapped
            // into the world and invisibly walls off the east route.)
            var table = new GameObject("LongDiningTable");
            table.transform.SetParent(env, false);
            table.transform.SetPositionAndRotation(d.P(3.7f, 4.9f, .52f), Quaternion.identity);
            var tableAsset = AssetDatabase.LoadAssetAtPath<GameObject>(
                "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/dining_table/dining_table_1k.fbx");
            if (!tableAsset) throw new Exception("CC0 dining table FBX missing");
            var tableChild = (GameObject)PrefabUtility.InstantiatePrefab(tableAsset);
            tableChild.transform.SetParent(table.transform, false);
            tableChild.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
            KosVisualV2.Resize(table, new Vector3(1.5f, .9f, 3.8f));
            KosVisualV2.SetCenter(table, d.P(3.7f, 4.9f, .52f) + Vector3.up * .45f);
            // Sole physics stays in the invisible DiningTableTopCollision box;
            // a collider on the visual would double-block the crouch route.
            if (table.GetComponent<Collider>()) Object.DestroyImmediate(table.GetComponent<Collider>());
            // The invisible top collision must match the visual exactly, otherwise
            // its overhangs block capsules where nothing is seen.
            {
                var topCol = Find("DiningTableTopCollision");
                if (topCol)
                {
                    topCol.transform.position = d.P(3.7f, 4.9f, 1.42f);
                    topCol.transform.localScale = new Vector3(1.5f, .075f, 3.8f);
                }
                foreach (var n in new[] { "DiningTableLegCollision" })
                    foreach (var go in env.GetComponentsInChildren<Transform>(true).Where(t => t && t.name == n).ToArray())
                        Object.DestroyImmediate(go.GetComponent<Collider>());
                foreach (float x in new[] { 3.09f, 4.31f }) foreach (float z in new[] { 2.9f, 6.9f })
                {
                    var leg = new GameObject("DiningTableLegCollision");
                    leg.transform.SetParent(env, false);
                    leg.transform.position = d.P(x, z, .95f);
                    leg.transform.localScale = new Vector3(.09f, .86f, .09f);
                    leg.AddComponent<BoxCollider>();
                }
            }

            // Reseat the feast onto the real surface: plates, spills and the tipped
            // pot rest on topY. Food and rat carcasses sit on their plates (not
            // the tabletop), so they are deliberately skipped here — grounding
            // them to topY would sink them into the dishes and make them float.
            // Designer Nov: ZERO gap — seat 2mm INTO the top so the plate foot
            // never hovers above the wood at glancing angles.
            foreach (var t in env.GetComponentsInChildren<Transform>(true).Where(t =>
                t && t.name.StartsWith("AddedDish_")).ToArray())
            {
                var b = SafeBounds(t.gameObject);
                t.position += Vector3.up * (topY - .002f - b.min.y);
            }

            // Clock hands + ashtray rest on the side table, not inside it.
            // CornerSideTable.fbx carries the same broken node transform as the old
            // dining table, so the visual is the CC0 Poly Haven Wooden Table 02
            // (small vintage side table; batch receipt in asset-download-manifest).
            Delete("CornerSideTable");
            var side = ProjectAsset(
                "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/wooden_table_02/wooden_table_02.fbx",
                "CornerSideTable", env, d.P(-5.25f, 1.15f, .52f), Quaternion.identity,
                new Vector3(.9f, .75f, .9f));
            if (side == null) throw new Exception("CC0 side table failed to project");
            // The Poly Haven export does not resolve its textures on import, so wire
            // the CC0 diffuse + normal explicitly (idempotent: same asset, re-set).
            {
                const string sideDir = "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/wooden_table_02/";
                var norImport = AssetImporter.GetAtPath(sideDir + "wooden_table_02_nor_gl_1k.jpg") as TextureImporter;
                if (norImport && norImport.textureType != TextureImporterType.NormalMap)
                {
                    norImport.textureType = TextureImporterType.NormalMap;
                    norImport.SaveAndReimport();
                }
                var sideMat = Material("Repair_SideTableWood");
                var sideNor = AssetDatabase.LoadAssetAtPath<Texture2D>(sideDir + "wooden_table_02_nor_gl_1k.jpg");
                if (sideNor) { sideMat.SetTexture("_BumpMap", sideNor); sideMat.SetFloat("_BumpScale", .55f); sideMat.EnableKeyword("_NORMALMAP"); EditorUtility.SetDirty(sideMat); }
                foreach (var r in side.GetComponentsInChildren<Renderer>())
                    r.sharedMaterials = Enumerable.Repeat(sideMat, r.sharedMaterials.Length).ToArray();
            }
            {
                float surface = SafeBounds(side).max.y;
                var cxz = SafeBounds(side).center;
                var hands = Find("BrassClockHands");
                if (hands) hands.transform.position = new Vector3(cxz.x, surface + .015f, cxz.z);
                var tray = Find("OldAshtray");
                if (tray) tray.transform.position = new Vector3(tray.transform.position.x, surface + .008f, tray.transform.position.z);
            }

            // Grandfather clock: the bundled FBX renders invisibly in play (blades +
            // pendulum buried at the case base), so rebuild the case from the CC0
            // Poly Haven Vintage Grandfather Clock 01 (Audit/grandfather-clock-
            // cc0-sources.json). Shared with ApplyRumahAtmajaClockVentCrowbar.
            RebuildCC0Clock(d, env);

            // The dining hinge must be serialized, not only runtime-added.
            var hinge = Find("KitchenDoorHinge");
            if (hinge && !hinge.GetComponent<LayarTerakhir.Prolog.HingedDoor>())
            {
                var hd = hinge.AddComponent<LayarTerakhir.Prolog.HingedDoor>();
                hd.hinge = hinge.transform; hd.angle = -95; hd.label = "Buka pintu ruang makan";
            }
            // The west-shifted radio cabinet (local x -1.25) seals the door-to-hands
            // desire line for a 0.32-radius capsule ("meja menghalangi jarum jam").
            // Park it flush against the sitting partition so the flow stays clear.
            // Delta is recomputed from the live position, so reruns converge.
            {
                var cab0 = Find("FamilyRadioCabinet");
                if (cab0)
                {
                    var cl = d.LocalPosition(cab0.transform.position);
                    var target = d.P(.35f, 1.72f, 0f) - d.P(cl.x, cl.y, 0f);
                    target.y = 0f;
                    if (target.magnitude > .01f)
                        foreach (var t in env.GetComponentsInChildren<Transform>(true).Where(t =>
                            t && (t.name == "FamilyRadioCabinet" || t.name == "FamilyRadioBody" || t.name == "FamilyCabinetPanel" || t.name == "RadioSpeakerGrille")).ToArray())
                            t.position += target;
                }
            }
            // WornWoodChair.fbx shares the 13m house-shell mesh, so the four
            // feast chairs + scare chair render as plank walls boxing the table.
            // Replace their visuals procedurally (positions/facing kept).
            // NOTE: Old("House_MAT_WoodDoor") resolves null (only .001-.004
            // variants exist), so use the guaranteed repair wood instead.
            {
                var chairWood = Material("Repair_SideTableWood");
                foreach (var tr in env.GetComponentsInChildren<Transform>(true).Where(t => t && (t.name == "WornDiningChair" || t.name == "EmptyARScareChair")).ToArray())
                {
                    var nm = tr.name; var pos = tr.position; var rot = tr.rotation;
                    Object.DestroyImmediate(tr.gameObject);
                    var c = new GameObject(nm); c.transform.SetParent(env, false);
                    c.transform.SetPositionAndRotation(pos, rot);
                    void Part(Vector3 lp, Vector3 s)
                    {
                        var p = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        p.transform.SetParent(c.transform, false);
                        p.transform.localPosition = lp; p.transform.localScale = s;
                        p.GetComponent<Renderer>().sharedMaterial = chairWood;
                    }
                    Part(new Vector3(0, .45f, 0), new Vector3(.46f, .07f, .46f));
                    Part(new Vector3(0, .72f, -.225f), new Vector3(.46f, .55f, .07f));
                    foreach (float lx in new[] { -.19f, .19f }) foreach (float lz in new[] { -.19f, .19f })
                        Part(new Vector3(lx, .225f, lz), new Vector3(.07f, .45f, .07f));
                }
            }
            Save();
            Debug.Log("PROLOG47_RUMAH_ATMAJA_DINING_FIX_APPLIED table surface, feast seating, clock hands");
        }

        // Designer Nov: the sitting-room clock never renders (bundled FBX hides its
        // works at the case base) and the child-room vent wears a brand-new,
        // ill-fitting cover that cannot be pried open. Rebuild the clock from the
        // CC0 Poly Haven Vintage Grandfather Clock 01, rust the vent grille to fit
        // its duct exactly behind a crowbar-gated hinge, and ensure the CC0 crowbar
        // in the storage nook. Idempotent.
        public static void ApplyRumahAtmajaClockVentCrowbar()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var env = Find("Atmaja_Environment_Art").transform;
            RebuildCC0Clock(d, env);
            RebuildRustedVent(d, env);
            EnsureCrowbar(d, env);
            Save();
            Debug.Log("PROLOG47_RUMAH_ATMAJA_CLOCK_VENT_CROWBAR_APPLIED cc0 clock, rusted vent, crowbar");
        }

        private static void RebuildCC0Clock(AtmajaDirector d, Transform env)
        {
            const string clockDir = "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/grandfather_clock/";
            var norImport = AssetImporter.GetAtPath(clockDir + "vintage_grandfather_clock_01_nor_gl_1k.jpg") as TextureImporter;
            if (norImport && norImport.textureType != TextureImporterType.NormalMap)
            {
                norImport.textureType = TextureImporterType.NormalMap;
                norImport.SaveAndReimport();
            }
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(clockDir + "vintage_grandfather_clock_01_1k.fbx");
            if (!asset) throw new Exception("CC0 grandfather clock FBX missing: " + clockDir);
            Delete("GrandfatherClock");
            Delete("ClockDial");
            var clockPos = d.P(-5.58f, 5.8f, .52f);
            var clock = new GameObject("GrandfatherClock");
            clock.transform.SetParent(env, false);
            clock.transform.SetPositionAndRotation(clockPos, Quaternion.LookRotation(d.east));
            var child = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            child.transform.SetParent(clock.transform, false);
            KosVisualV2.SetCenter(clock, clockPos + Vector3.up * 1.1f);
            var wood = Material("Repair_ClockWood");
            var nor = AssetDatabase.LoadAssetAtPath<Texture2D>(clockDir + "vintage_grandfather_clock_01_nor_gl_1k.jpg");
            if (nor) { wood.SetTexture("_BumpMap", nor); wood.SetFloat("_BumpScale", .55f); wood.EnableKeyword("_NORMALMAP"); EditorUtility.SetDirty(wood); }
            // The imported glass renders opaque and hides the dial + hands, so the
            // door pane becomes a faint custom sheet: readable with a flashlight.
            var glassMat = Material("Repair_ClockGlass");
            glassMat.SetFloat("_Surface", 1f);
            glassMat.SetFloat("_Blend", 0f);
            glassMat.SetFloat("_SrcBlend", 5f);
            glassMat.SetFloat("_DstBlend", 10f);
            glassMat.SetFloat("_ZWrite", 0f);
            glassMat.SetFloat("_Smoothness", .9f);
            glassMat.SetFloat("_Metallic", 0f);
            glassMat.DisableKeyword("_ALPHATEST_ON");
            glassMat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            glassMat.SetColor("_BaseColor", new Color(.85f, .88f, .92f, .09f));
            glassMat.renderQueue = 3000;
            EditorUtility.SetDirty(glassMat);
            foreach (var r in clock.GetComponentsInChildren<Renderer>())
            {
                var cm = r.sharedMaterials;
                for (int ci = 0; ci < cm.Length; ci++)
                {
                    if (!cm[ci]) continue;
                    cm[ci] = cm[ci].name.IndexOf("glass", StringComparison.OrdinalIgnoreCase) >= 0 ? glassMat : wood;
                }
                r.sharedMaterials = cm;
            }
            var hour = clock.GetComponentsInChildren<Transform>(true).First(t => t.name == "vintage_grandfather_clock_01_HourHand");
            hour.name = "ClockHourBlade__CC0";
            var minute = clock.GetComponentsInChildren<Transform>(true).First(t => t.name == "vintage_grandfather_clock_01_MinuteHand");
            minute.name = "ClockMinuteBlade__CC0";
            d.clockFaceNormal = d.east;
            d.clockDialCentre = hour.position;
            d.clockHour = hour;
            d.clockMinute = minute;
            var pend = new GameObject("ClockPendulum__");
            pend.transform.SetParent(clock.transform, false);
            pend.transform.localPosition = new Vector3(0, 1.45f, .05f);
            var rod = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            rod.name = "PendulumRod";
            rod.transform.SetParent(pend.transform, false);
            rod.transform.localPosition = new Vector3(0, -.475f, 0);
            rod.transform.localScale = new Vector3(.02f, .95f, .02f);
            rod.GetComponent<Renderer>().sharedMaterial = Old("Detail_AgedBrass");
            Object.DestroyImmediate(rod.GetComponent<Collider>());
            var bob = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            bob.name = "PendulumBob";
            bob.transform.SetParent(pend.transform, false);
            bob.transform.localPosition = new Vector3(0, -.95f, 0);
            bob.transform.localRotation = Quaternion.Euler(90, 0, 0);
            bob.transform.localScale = new Vector3(.18f, .03f, .18f);
            bob.GetComponent<Renderer>().sharedMaterial = Old("Detail_AgedBrass");
            Object.DestroyImmediate(bob.GetComponent<Collider>());
            var drawerBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
            drawerBox.name = "ClockDrawer__";
            drawerBox.transform.SetParent(clock.transform, false);
            drawerBox.transform.localPosition = new Vector3(0, .35f, .21f);
            drawerBox.transform.localScale = new Vector3(.4f, .18f, .06f);
            drawerBox.GetComponent<Renderer>().sharedMaterial = Old("House_MAT_WoodDoor");
            Object.DestroyImmediate(drawerBox.GetComponent<Collider>());
            d.clockDrawer = drawerBox.transform;
            FitCollider(clock);
            var item = clock.GetComponent<AtmajaItem>();
            if (!item) item = clock.AddComponent<AtmajaItem>();
            item.director = d;
            item.action = AtmajaAction.Clock;
            item.label = "Periksa jam bandul tua";
            var tick = clock.GetComponent<ClockAmbience>();
            if (!tick) tick = clock.AddComponent<ClockAmbience>();
            tick.pendulum = pend.transform;
            tick.pivot = new Vector3(0, 1.45f, .05f);
            tick.axis = Vector3.forward;
            var src = clock.GetComponent<AudioSource>();
            if (!src) src = clock.AddComponent<AudioSource>();
            tick.source = src;
            src.playOnAwake = false;
            src.dopplerLevel = 0;
            src.spatialBlend = 1;
            src.minDistance = .8f;
            src.maxDistance = 7;
            src.rolloffMode = AudioRolloffMode.Linear;
            src.outputAudioMixerGroup = d.audioProfile.effectsBus;
        }

        // Designer Nov + fix: bathroom claw wall is a FLAT 1.5m x 0.95m panel
        // painted 1:1 to the goal photo (R2 asset/cakaran_yang_aku_inginkan.jpg).
        // Idempotent.
        public static void ApplyScratchRelief()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var root = Find("Atmaja_Environment_Art").transform;
            RebuildScratchRelief(d, root);
            Save();
            Debug.Log("PROLOG47_SCRATCH_RELIEF_APPLIED designer rupture relief, reference-matched paint");
        }

        // Designer Nov: the cellar is a big but empty box — its gameplay all
        // sits in the west corner. Carve an interconnected maze inside the
        // existing 12x12 footprint (no new digging): a spine wall and a cross
        // wall with staggered gaps form a full loop, the flooded south room
        // gets puddle + crates, and Damar's handycam hides in the south-east
        // nook so it is earned, not stumbled upon. West room, chase rail,
        // ladder and audit anchors are untouched.
        public static void ApplyBasementBranches()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var root = Find("Atmaja_Environment_Art").transform;
            RebuildBasementBranches(d, root);
            Save();
            Debug.Log("PROLOG47_BASEMENT_BRANCHES_APPLIED maze loop, flooded storage, hidden handycam");
        }

        // Review-only capture: one framebuffer per basement room with a
        // temporary neutral fill light on the camera so dust/rust reads even
        // in the dim horror grade. Never saved into the scene.
        // Full-house visual sweep: one framebuffer per room plus straight-up
        // ceiling views, to catch floaters, blocked furniture and gate
        // oddities in the CURRENT scene. Review-only, never saved.
        public static void CaptureAtmajaSweep()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            const string folder = "Audit/Visuals/House-Sweep";
            System.IO.Directory.CreateDirectory(folder);
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = 1.6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            // All camera spots sit on audit-walked capsule-clear waypoints.
            var shots = new (string, Vector3, Vector3)[]
            {
                ("01-gate", d.P(0f, -13.5f, 1.7f), d.P(0f, -6f, 2.5f)),
                ("02-courtyard", d.P(0f, -10f, 1.6f), d.P(0f, -4f, 2f)),
                ("03-kitchen", d.P(4.5f, 9f, 2f), d.P(2.85f, 11.2f, .3f)),
                ("04-dining", d.P(3.7f, 7.5f, 2f), d.P(3.7f, 4.9f, 1f)),
                ("05-dining-up", d.P(4.0f, 6.0f, 1.2f), d.P(4.0f, 6.0f, 3.5f)),
                ("06-sitting", d.P(-1.4f, 4.1f, 2.1f), d.P(-4.5f, 3f, 1f)),
                ("07-sitting-up", d.P(-3.4f, 3.9f, 1.2f), d.P(-3.4f, 3.9f, 3.5f)),
                ("08-stairs", d.P(-3.55f, 8.2f, 2.1f), d.P(1.05f, 7f, 2.5f)),
                ("09-upper-corridor", d.P(1.3f, 5.75f, 5.4f), d.P(-3.3f, 5.75f, 4.8f)),
                ("10-child-room", d.P(-3f, 3.5f, 5.2f), d.P(-4f, 2.5f, 4.2f)),
                ("15-child-doorway", d.P(-4.65f, 4.6f, 5.2f), d.P(-3.6f, 2.8f, 4.2f)),
                ("16-bath-doorway", d.P(-1.1f, 5.6f, 5.2f), d.P(-1.1f, 3.2f, 4.2f)),
                ("17-side-up", d.P(8.2f, 2f, 1.2f), d.P(6f, 4f, 6f)),
                ("18-gate-close", d.P(0f, -11f, 1.6f), d.P(0f, -9.5f, 1.4f)),
                ("11-bathroom", d.P(-0.5f, 4.3f, 5.2f), d.P(-1.3f, 3f, 4.2f)),
                ("12-balcony-rope", d.P(3.4f, 5f, 5.4f), d.P(2.5f, 5.5f, 6f)),
                ("13-master", d.P(1.3f, 4.5f, 5.4f), d.P(.5f, 2.5f, 4f)),
                ("14-master-up", d.P(.8f, 2.6f, 4.6f), d.P(.8f, 2.6f, 6.5f)),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("HOUSE_SWEEP_CAPTURED " + shots.Length + " views -> " + folder);
        }

        [MenuItem("LayarTerakhir/Prolog/Find stretched geometry")]
        public static void FindStretchedGeometry()
        {
            // Where do the Static__House parts live, and what spans the
            // child room / bathroom with long triangles?
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var hunter = d.basement.nyonya;
            Debug.Log("STRETCH_HUNTER active=" + hunter.activeSelf + " pos=" + hunter.transform.position.ToString("F2"));
            var bodies = hunter.GetComponentsInChildren<SkinnedMeshRenderer>(true).Where(r => r.name == "NyonyaDesignerBody").ToArray();
            Debug.Log("STRETCH_HUNTERBODYCOUNT " + bodies.Length);
            foreach (var body in bodies)
            {
                Debug.Log("STRETCH_HUNTERMESH meshBounds=" + body.sharedMesh.bounds.size.ToString("F2") + " renderBounds=" + body.bounds.size.ToString("F2") + " verts=" + body.sharedMesh.vertexCount + " active=" + body.gameObject.activeInHierarchy);
                Debug.Log("STRETCH_HUNTERBONES count=" + body.bones.Length + " rootscale=" + body.transform.root.GetComponentInChildren<Transform>().localScale.ToString("F2"));
                foreach (var b in new[] { "Hips", "Head", "Hand_R", "Foot_L" })
                {
                    var t = body.bones.FirstOrDefault(x => x && x.name == b);
                    if (t) Debug.Log("STRETCH_HUNTERBONE " + b + " local=" + t.localPosition.ToString("F2") + " world=" + t.position.ToString("F2"));
                    else Debug.Log("STRETCH_HUNTERBONE " + b + " MISSING");
                }
                var wps = body.sharedMesh.bindposes;
                Debug.Log("STRETCH_BINDPOSE0 Hips-equivalent magnitude=" + wps[0].MultiplyPoint3x4(Vector3.zero).magnitude.ToString("F2"));
            }
            var root = Find("Atmaja_Environment_Art").transform;
            foreach (var f in root.GetComponentsInChildren<MeshFilter>(true))
                if (f.name.StartsWith("Static__House") || f.name.StartsWith("KitchenGlass__House"))
                    Debug.Log("STRETCH_HOUSEPART " + f.name + " parent=" + ParentChain(f.transform));
            // child room approx d.P x -5..-2.5, bath x -2..0 (director space);
            // streaks run wall to wall at eye height, so flag wide renderers
            // in the upper-floor band instead of guessing rooms.
            foreach (var r in root.GetComponentsInChildren<Renderer>(true))
            {
                var b = r.bounds;
                if (b.size.x > 2.5f && b.size.y < 1.2f && b.center.y > 3.5f && b.center.y < 6.5f)
                    Debug.Log("STRETCH_WIDE " + r.name + " " + b.center.ToString("F2") + " size " + b.size.ToString("F2") + " parent=" + ParentChain(r.transform));
            }
            Debug.Log("STRETCH_SCAN_DONE");
        }
        private static string ParentChain(Transform t)
        {
            var chain = t.name;
            while (t.parent != null) { t = t.parent; chain = t.name + "/" + chain; }
            return chain;
        }
        [MenuItem("LayarTerakhir/Prolog/Hide suspects and capture")]
        public static void HideSuspectsAndCapture()
        {
            // Binary search for the streak source: hide curtain, louvers,
            // quilt and blanket, capture child+bath, and QUIT WITHOUT
            // SAVING so the scene file never changes.
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            foreach (var n in new[] { "BathCurtain", "ChildBlanket" })
            {
                var go = GameObject.Find(n);
                if (go) go.SetActive(false);
            }
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (t.name == "InteriorWoodLouver" || t.name == "Draped stitched cotton blanket")
                    t.gameObject.SetActive(false);
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = 1.6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var shots = new (string, Vector3, Vector3)[]
            {
                ("20-child-nohide-test", d.P(-3f, 3.5f, 5.2f), d.P(-4f, 2.5f, 4.2f)),
                ("21-bath-nohide-test", d.P(-0.5f, 4.3f, 5.2f), d.P(-1.3f, 3f, 4.2f)),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("HIDE_TEST_CAPTURED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Verify fixes capture")]
        public static void CaptureVerifyFixes()
        {
            // Verifikasi Crimson (sudah di-save via ApplyScratchRelief) +
            // simulasi hasil rebuild balkon (rail/beam di-hide, no-save).
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                if (t.name == "RailTop" || t.name == "RailBaluster" || t.name == "VoidBeam")
                {
                    var r = t.GetComponent<Renderer>();
                    if (r) r.enabled = false;
                }
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = 1.6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var shots = new (string, Vector3, Vector3)[]
            {
                ("22-child-verify", d.P(-3f, 3.5f, 5.2f), d.P(-4f, 2.5f, 4.2f)),
                ("23-balcony-lookup-verify", d.P(10f, 5.5f, 1.5f), d.P(3.7f, 4.3f, 5.5f)),
                ("24-gate-close-verify", d.P(0f, -11f, 1.6f), d.P(0f, -9.5f, 1.4f)),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("VERIFY_FIXES_CAPTURED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Diagnose gate capture")]
        public static void CaptureGateDiag()
        {
            // Gerbang dari samping + dari dalam + balkon redup (no-save).
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = .35f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var shots = new (string, Vector3, Vector3)[]
            {
                ("25-gate-side", d.P(-6f, -10f, 1.6f), d.P(0f, -10f, 1.4f)),
                ("26-gate-from-inside", d.P(0f, -8f, 1.6f), d.P(0f, -11f, 1.6f)),
                ("27-balcony-dim", d.P(10f, 5.5f, 1.5f), d.P(3.7f, 4.3f, 5.5f)),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("GATE_DIAG_CAPTURED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Diagnose gate leaves")]
        public static void CaptureLeafDiag()
        {
            // Tiap daun gerbang tegak lurus + 2 sudut pohon (no-save).
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = .6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var shots = new (string, Vector3, Vector3)[]
            {
                ("28-leaf-left", d.P(0.74f, -13f, 1.4f), d.P(0.74f, -10f, 1.4f)),
                ("29-leaf-right", d.P(-0.74f, -13f, 1.4f), d.P(-0.74f, -10f, 1.4f)),
                ("30-tree-west", d.P(7f, 2.5f, 2f), d.P(3.7f, 4f, 5.2f)),
                ("31-tree-south", d.P(9f, -1f, 2f), d.P(3.7f, 4f, 5.2f)),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("LEAF_DIAG_CAPTURED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Diagnose dining walls")]
        public static void CaptureDiningDiag()
        {
            // Sembunyikan 8 dinding tersangka (no-save), capture meja makan
            // + sudut jam/radio untuk identifikasi dinding ruang jam.
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            foreach (var n in new[] { "StairEnclosureFrontEast", "StairEnclosureEast", "StairHalfLandingPier", "StairHalfLanding", "StairFlightUpper", "StairFlightLower", "FrontSittingPartitionEast", "DiningVaultBlocker" })
            {
                var go = GameObject.Find(n);
                if (go) go.SetActive(false);
            }
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = .6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var shots = new (string, Vector3, Vector3)[]
            {
                ("32-dining-freed", d.P(3.7f, 7.5f, 2f), d.P(3.7f, 4.9f, 1f)),
                ("33-clock-corner", d.P(-3.5f, 1.15f, 1.6f), d.P(-5.25f, 1.15f, .8f)),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(s.Item2, Quaternion.LookRotation(s.Item3 - s.Item2));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("DINING_DIAG_CAPTURED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Capture hunter proof")]
        public static void CaptureHunterProof()
        {
            // Basement hunter close-up, no-save. Proves the body exists.
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var hunter = d && d.basement ? d.basement.nyonya : null;
            if (!hunter) { Debug.Log("HUNTER_PROOF_MISSING no nyonya bound"); return; }
            bool wasActive = hunter.activeSelf;
            hunter.SetActive(true);
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HunterCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 70f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = 1.6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var hb = KosVisualV2.BoundsOf(hunter);
            Debug.Log("HUNTER_PROOF bounds " + hb.center.ToString("F2") + " size " + hb.size.ToString("F2"));
            Vector3 focus, camPos;
            if (hb.size.magnitude > 20f)
            {
                focus = hunter.transform.position + Vector3.up * 1f;
                camPos = focus + new Vector3(2f, .5f, 2f);
                Debug.Log("HUNTER_PROOF bounds insane, framing holder instead");
            }
            else { focus = hb.center; camPos = hb.center + new Vector3(1.5f, .3f, 1.5f); }
            camGo.transform.SetPositionAndRotation(camPos, Quaternion.LookRotation(focus - camPos));
            KosRenderDiagnosis.Capture(cam, "hunter-proof", false, folder);
            Object.DestroyImmediate(camGo);
            Debug.Log("HUNTER_PROOF_CAPTURED no-save");
        }
    [MenuItem("LayarTerakhir/Prolog/Diagnose quilt hide")]
        public static void CaptureQuiltHide()
        {
            // Binary search: hide ChildBlanket, capture child room, no-save.
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var q = GameObject.Find("ChildBlanket");
            if (q) q.SetActive(false);
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = .6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            camGo.transform.SetPositionAndRotation(d.P(-3f, 3.5f, 5.2f), Quaternion.LookRotation(d.P(-4f, 2.5f, 4.2f) - d.P(-3f, 3.5f, 5.2f)));
            KosRenderDiagnosis.Capture(cam, "34-quilt-hidden-test", false, folder);
            Object.DestroyImmediate(camGo);
            Debug.Log("QUILT_HIDE_CAPTURED no-save");
        }
    [MenuItem("LayarTerakhir/Prolog/Diagnose dining walls 2")]
        public static void CaptureDiningDiag2()
        {
            // Varian A: sembunyikan shell, B: sembunyikan kursi, C: sitting.
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            const string folder = "Audit/Visuals/House-Sweep";
            var camGo = new GameObject("HouseSweepCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 9f; fill.intensity = .6f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var hidden = new System.Collections.Generic.List<GameObject>();
            void Hide(System.Func<string, bool> pred)
            {
                foreach (var t in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                    if (pred(t.name)) { t.gameObject.SetActive(false); hidden.Add(t.gameObject); }
            }
            void Unhide() { foreach (var g in hidden) if (g) g.SetActive(true); hidden.Clear(); }
            void Shot(string name, Vector3 from, Vector3 to)
            {
                camGo.transform.SetPositionAndRotation(from, Quaternion.LookRotation(to - from));
                KosRenderDiagnosis.Capture(cam, name, false, folder);
            }
            var diningFrom = d.P(3.7f, 7.5f, 2f); var diningTo = d.P(3.7f, 4.9f, 1f);
            Hide(n => n == "RumahAtmaja_House");
            Shot("34-dining-noshell", diningFrom, diningTo);
            Unhide();
            Hide(n => n.Contains("Chair"));
            Shot("35-dining-nochairs", diningFrom, diningTo);
            Unhide();
            Shot("36-sitting-wide", d.P(-1.4f, 4.1f, 2.1f), d.P(-4.5f, 3f, 1f));
            Object.DestroyImmediate(camGo);
            Debug.Log("DINING_DIAG2_CAPTURED no-save");
        }

        [MenuItem("LayarTerakhir/Prolog/Ensure courtyard ground")]
        public static void EnsureCourtyardGround()
        {
            // West courtyard lost all ground colliders (review subdivision
            // deleted its parts; the spanning slab stayed disabled). Lay four
            // explicit boxes covering the slab rect minus the south well hole
            // (x[-3.28,-2.42] z[-11.63,-10.77] stays OPEN for the well).
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var env = Find("Atmaja_Environment_Art").transform;
            foreach (var n in new[] { "CourtyardGroundWest", "CourtyardGroundEast", "CourtyardGroundNorth", "CourtyardGroundSouth" }) Delete(n);
            void Part(string n, Vector3 c, Vector3 s)
            {
                var g = new GameObject(n); g.transform.SetParent(env, false); g.transform.position = c;
                var b = g.AddComponent<BoxCollider>(); b.center = Vector3.zero; b.size = s;
            }
            const float y = -.025f, h = .05f;
            Part("CourtyardGroundWest", new Vector3(-7.89f, y, -1f), new Vector3(9.22f, h, 31f));
            Part("CourtyardGroundEast", new Vector3(5.04f, y, -1f), new Vector3(14.92f, h, 31f));
            Part("CourtyardGroundNorth", new Vector3(-2.85f, y, 1.865f), new Vector3(.86f, h, 25.27f));
            Part("CourtyardGroundSouth", new Vector3(-2.85f, y, -14.065f), new Vector3(.86f, h, 4.87f));
            Save();
            Debug.Log("COURTYARD_GROUND_APPLIED 4 parts, well hole stays open");
        }

        public static void CaptureHouseExterior()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            const string folder = "Audit/Visuals/House-Restore";
            System.IO.Directory.CreateDirectory(folder);
            var camGo = new GameObject("HouseReviewCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 120f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var shots = new (string, float, float, float, float, float, float)[]
            {
                ("01-courtyard-arrival", 0f, -13.5f, 0f, -6f, 2.5f, 0f),
                ("02-side-passage", 8.2f, -2f, 4f, 6f, 1.5f, 0f),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(
                    d.P(s.Item2, s.Item3, 1.7f),
                    Quaternion.LookRotation(d.P(s.Item4, s.Item5, s.Item6) - d.P(s.Item2, s.Item3, 1.7f)));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("HOUSE_REVIEW_CAPTURED " + shots.Length + " views -> " + folder);
        }

        public static void CaptureBasementRooms()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            const string folder = "Audit/Visuals/Basement-Review";
            System.IO.Directory.CreateDirectory(folder);
            var camGo = new GameObject("BasementReviewCam", typeof(Camera));
            var cam = camGo.GetComponent<Camera>();
            cam.enabled = false; cam.nearClipPlane = .05f; cam.farClipPlane = 60f;
            cam.clearFlags = CameraClearFlags.SolidColor; cam.backgroundColor = Color.black;
            var fill = camGo.AddComponent<Light>();
            fill.type = LightType.Point; fill.range = 7f; fill.intensity = 1.0f;
            fill.color = new Color(1f, 1f, 1f); fill.shadows = LightShadows.None;
            var shots = new (string, float, float, float, float, float, float)[]
            {
                ("01-west-original", 2.2f, 6.3f, 3.7f, 6.5f, -1.9f, 0f),
                ("02-emergence-curtain", 2.2f, 5.0f, 3.3f, 6.0f, -1.8f, 0f),
                ("03-spine-lane", 6.0f, 4.5f, 6.5f, 7.5f, -1.9f, 0f),
                ("04-r1-archive", 12.5f, 6.5f, 14.6f, 10.2f, -2.0f, 0f),
                ("05-r1-props", 12.3f, 5.8f, 12.8f, 3.2f, -2.5f, 0f),
                ("06-r2-crypt", 13.5f, 12.8f, 11.0f, 15.3f, -2.6f, 0f),
                ("07-r2-handycam", 13.8f, 14.0f, 14.8f, 15.2f, -2.5f, 0f),
                ("08-r3-flooded", 3.5f, 9.0f, 5.6f, 11.5f, -2.5f, 0f),
            };
            foreach (var s in shots)
            {
                camGo.transform.SetPositionAndRotation(
                    d.P(s.Item2, s.Item3, -1.19f),
                    Quaternion.LookRotation(d.P(s.Item4, s.Item5, s.Item6) - d.P(s.Item2, s.Item3, -1.19f)));
                KosRenderDiagnosis.Capture(cam, s.Item1, false, folder);
            }
            Object.DestroyImmediate(camGo);
            Debug.Log("BASEMENT_REVIEW_CAPTURED " + shots.Length + " rooms -> " + folder);
        }

        private static GameObject MazeBox(string name, Transform root, Vector3 pos, Vector3 size, Material mat, bool solid = true)
        {
            Delete(name);
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(root, true);
            go.transform.SetPositionAndRotation(pos, Quaternion.LookRotation(Vector3.forward));
            go.transform.localScale = size;
            go.GetComponent<Renderer>().sharedMaterial = mat;
            go.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            if (!solid) Object.DestroyImmediate(go.GetComponent<Collider>());
            return go;
        }

        private static void RebuildBasementBranches(AtmajaDirector d, Transform root)
        {
            var wallMat = Find("CellarWallW").GetComponent<Renderer>().sharedMaterial;
            var floorMat = Find("CellarFloor").GetComponent<Renderer>().sharedMaterial;
            var ceilMat = Find("CellarCeiling").GetComponent<Renderer>().sharedMaterial;
            // Open the south wall into the flooded store (local x 2.0-3.0).
            // Delete-first: without this every re-apply stacks another
            // identical wall pair (17x found in the wild).
            foreach (string dead in new[] { "CellarWallS", "CellarWallS_W", "CellarWallS_E" })
                Delete(dead);
            var wallRot = Find("CellarWallN") ? Find("CellarWallN").transform.rotation : Quaternion.identity;
            var wallS1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallS1.name = "CellarWallS_W";
            wallS1.transform.SetParent(root, true);
            wallS1.transform.SetPositionAndRotation(d.P(1.1f, 6.9f, -1.575f), wallRot);
            wallS1.transform.localScale = new Vector3(1.8f, 2.45f, .16f);
            wallS1.GetComponent<Renderer>().sharedMaterial = wallMat;
            var wallS2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallS2.name = "CellarWallS_E";
            wallS2.transform.SetParent(root, true);
            wallS2.transform.SetPositionAndRotation(d.P(4.3f, 6.9f, -1.575f), wallRot);
            wallS2.transform.localScale = new Vector3(2.6f, 2.45f, .16f);
            wallS2.GetComponent<Renderer>().sharedMaterial = wallMat;
            var wood = Material("Repair_BedWood");
            var iron = Material("Repair_Iron");
            // Spine wall (local east 6.5, gaps at depth 4.0-5.0 and 8.5-9.5).
            MazeBox("MazeSpineW1", root, d.P(6.5f, 1.5f, -1.575f), new Vector3(.16f, 2.45f, 5.0f), wallMat);
            MazeBox("MazeSpineW2", root, d.P(6.5f, 6.75f, -1.575f), new Vector3(.16f, 2.45f, 3.5f), wallMat);
            MazeBox("MazeSpineW3", root, d.P(6.5f, 10.5f, -1.575f), new Vector3(.16f, 2.45f, 2.0f), wallMat);
            // Cross wall (local depth 7.2, gap at east 9.0-10.0).
            MazeBox("MazeCrossS1", root, d.P(7.75f, 7.2f, -1.575f), new Vector3(2.5f, 2.45f, .16f), wallMat);
            MazeBox("MazeCrossS2", root, d.P(11.0f, 7.2f, -1.575f), new Vector3(2.0f, 2.45f, .16f), wallMat);
            // West cap of the flooded south room.
            MazeBox("MazeSouthWestWall", root, d.P(0.2f, 8.05f, -1.575f), new Vector3(.16f, 2.45f, 2.3f), wallMat);
            // Flooded storage: dark glossy puddle (visual only, walkable).
            var puddle = MazeBox("MazeFloodPuddle", root, d.P(2.5f, 8.0f, -2.79f), new Vector3(2.0f, .012f, 1.4f), null, false);
            var floodMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            floodMat.SetColor("_BaseColor", new Color(.04f, .06f, .07f));
            floodMat.SetFloat("_Smoothness", .95f);
            floodMat.SetFloat("_Metallic", .1f);
            EditorUtility.SetDirty(floodMat);
            puddle.GetComponent<Renderer>().sharedMaterial = floodMat;
            // Crates dressing the new lanes (kept clear of walk lines).
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "MazeCrateA", root,
                d.P(1.0f, 8.4f, -2.79f), Quaternion.Euler(0, 12, 0), new Vector3(.55f, .55f, .55f), wood);
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "MazeCrateB", root,
                d.P(2.3f, 8.5f, -2.79f), Quaternion.Euler(0, -9, 0), new Vector3(.55f, .55f, .55f), wood);
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/old_military_crate/old_military_crate.fbx", "MazeCrateC", root,
                d.P(8.5f, 6.9f, -2.79f), Quaternion.Euler(0, 30, 0), new Vector3(.6f, .5f, .5f), wood);
            // Handycam hideout: low crate in the south-east nook, flanked.
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "MazeHandycamCrate", root,
                d.P(10.8f, 8.8f, -2.79f), Quaternion.identity, new Vector3(.5f, .5f, .5f), wood);
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "MazeNookFlank", root,
                d.P(10.5f, 7.4f, -2.79f), Quaternion.Euler(0, -14, 0), new Vector3(.5f, .9f, .5f), wood);
            var cam = Find("DamarHandycam");
            if (cam) cam.transform.SetPositionAndRotation(d.P(10.8f, 8.8f, -2.2f), Quaternion.LookRotation(d.north));
            // Archive shelf dressing the north-east room.
            var shelf = MazeBox("MazeArchiveShelf", root, d.P(8.5f, 4.5f, -1.9f), new Vector3(1.8f, 1.8f, .45f), iron);
            MazeBox("MazeArchiveTop", root, d.P(8.5f, 4.5f, -1.15f), new Vector3(1.7f, .3f, .4f), wood);
            // Third wardrobe so the far half has its own hiding spot.
            var ward = ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/painted_wooden_cabinet_02/painted_wooden_cabinet_02.fbx", "CellarWardrobe3", root,
                d.P(11.3f, 10.5f, -2.79f), Quaternion.Euler(0, -90, 0), new Vector3(.95f, 1.9f, .45f));
            var wardItem = ward.AddComponent<AtmajaItem>();
            wardItem.director = d; wardItem.action = AtmajaAction.WardrobeHide; wardItem.label = "Sembunyi di lemari kayu #3";
            d.basement.wardrobeCol3 = ward.GetComponent<BoxCollider>();
            d.basement.hideAnchor3 = AnchorAt(root, "HideAnchor3", d.P(10.5f, 10.5f, -2.79f), d.P(11.3f, 10.5f, 0));
            d.basement.exitAnchor3 = AnchorAt(root, "ExitAnchor3", d.P(10.0f, 9.7f, -2.79f), d.P(11.3f, 10.5f, 0));
            // Hunter patrol covers the new loop (existing indices preserved).
            // Rebuilt from scratch each run: stale maze posts from previous
            // applies are removed, never accumulated.
            foreach (var old in root.GetComponentsInChildren<Transform>(true).Where(t => t && (t.name.StartsWith("MazePatrol") || t.name.StartsWith("MazeBranchPatrol"))).ToArray())
                Object.DestroyImmediate(old.gameObject);
            var wps = new List<Transform>(d.basement.waypoints.Where(t => t && !t.name.StartsWith("MazePatrol") && !t.name.StartsWith("MazeBranchPatrol")));
            int bi = 0;
            foreach (var pt in new[] { d.P(6.5f, 9.0f, -2.79f), d.P(8.0f, 9.3f, -2.79f), d.P(10.0f, 8.4f, -2.79f), d.P(8.5f, 5.0f, -2.79f) })
            {
                var t = new GameObject("MazePatrol" + bi++).transform;
                t.SetParent(root, true);
                t.position = pt;
                t.rotation = Quaternion.LookRotation(d.P(2.9f, 3.5f, 0) - pt);
                wps.Add(t);
            }
            d.basement.waypoints = wps.ToArray();
            // Dim lights for the new branches.
            BranchLight(root, "MazeGapLight", d.P(6.5f, 9.0f, -1.7f), .55f, 5f, new Color(.72f, .76f, .65f));
            BranchLight(root, "MazeNookLight", d.P(10.0f, 8.4f, -1.7f), .6f, 6f, new Color(1f, .85f, .6f));
            // Designer Nov: the maze grows to three rooms, each with its own
            // hiding locker. R1 east annex (x 12-16) opens through the east
            // wall gaps; R2 south crypt (x 8-16, z 12.5-16.5) hides the
            // handycam in its far corner; R3 flooded store gets a locker.
            Delete("ExpandedCellarEastWall");
            MazeBox("MazeEastWall_N", root, d.P(12f, 2.3f, -1.575f), new Vector3(.16f, 2.45f, 3.4f), wallMat);
            MazeBox("MazeEastWall_M", root, d.P(12f, 6.25f, -1.575f), new Vector3(.16f, 2.45f, 2.5f), wallMat);
            MazeBox("MazeEastWall_S", root, d.P(12f, 10.5f, -1.575f), new Vector3(.16f, 2.45f, 4.0f), wallMat);
            // R1 east annex shell.
            MazeBox("MazeR1Floor", root, d.P(14f, 7.5f, -2.825f), new Vector3(4.1f, .05f, 10.1f), floorMat);
            MazeBox("MazeR1Ceil", root, d.P(14f, 7.5f, 0f), new Vector3(4.1f, 1.0f, 10.1f), ceilMat);
            MazeBox("MazeR1East", root, d.P(16f, 7.5f, -1.575f), new Vector3(.16f, 2.45f, 10.1f), wallMat);
            MazeBox("MazeR1North", root, d.P(14f, 2.5f, -1.575f), new Vector3(4.16f, 2.45f, .16f), wallMat);
            MazeBox("MazeR1South_W", root, d.P(12.5f, 12.5f, -1.575f), new Vector3(1.0f, 2.45f, .16f), wallMat);
            MazeBox("MazeR1South_E", root, d.P(15f, 12.5f, -1.575f), new Vector3(2.0f, 2.45f, .16f), wallMat);
            MazeBox("MazeR1PillarA", root, d.P(13.2f, 5.2f, -1.575f), new Vector3(.5f, 2.45f, .5f), wallMat);
            MazeBox("MazeR1PillarB", root, d.P(14.6f, 7.8f, -1.575f), new Vector3(.5f, 2.45f, .5f), wallMat);
            BranchLight(root, "MazeR1Light", d.P(14f, 7.5f, -1.7f), .55f, 6f, new Color(.72f, .76f, .65f));
            // R2 south crypt shell.
            MazeBox("MazeR2Floor", root, d.P(12f, 14.5f, -2.825f), new Vector3(8.1f, .05f, 4.1f), floorMat);
            MazeBox("MazeR2Ceil", root, d.P(12f, 14.5f, 0f), new Vector3(8.1f, 1.0f, 4.1f), ceilMat);
            MazeBox("MazeR2West", root, d.P(8f, 14.5f, -1.575f), new Vector3(.16f, 2.45f, 4.1f), wallMat);
            MazeBox("MazeR2East", root, d.P(16f, 14.5f, -1.575f), new Vector3(.16f, 2.45f, 4.1f), wallMat);
            MazeBox("MazeR2South", root, d.P(12f, 16.5f, -1.575f), new Vector3(8.26f, 2.45f, .16f), wallMat);
            MazeBox("MazeR2North_W", root, d.P(10.5f, 12.5f, -1.575f), new Vector3(5.0f, 2.45f, .16f), wallMat);
            MazeBox("MazeR2North_E", root, d.P(15f, 12.5f, -1.575f), new Vector3(2.0f, 2.45f, .16f), wallMat);
            BranchLight(root, "MazeR2Light", d.P(12f, 14.5f, -1.7f), .6f, 7f, new Color(.72f, .76f, .65f));
            BranchLight(root, "MazeCamLight", d.P(14.8f, 14.0f, -1.7f), .5f, 5f, new Color(1f, .85f, .6f));
            // R3 flooded store: west + south caps (north is WallS, east the spine).
            MazeBox("MazeR3West", root, d.P(0.2f, 9.7f, -1.575f), new Vector3(.16f, 2.45f, 5.6f), wallMat);
            MazeBox("MazeR3South", root, d.P(3.35f, 12.5f, -1.575f), new Vector3(6.3f, 2.45f, .16f), wallMat);
            BranchLight(root, "MazeR3Light", d.P(3.0f, 9.5f, -1.7f), .55f, 6f, new Color(.72f, .76f, .65f));
            // Handycam moves to the crypt far corner, on its own crate.
            Delete("MazeHandycamCrate"); Delete("MazeNookFlank");
            // The legacy textile-chest row hugs the east wall and seals both
            // new gaps: re-stack it along the crypt south wall instead.
            foreach (var old in FindAll("CellarTextileChest")) Object.DestroyImmediate(old);
            foreach (var old in FindAll("TextileChestLid")) Object.DestroyImmediate(old);
            var chestSpots = new[] { new Vector3(12.5f, 0f, 15.9f), new Vector3(13.3f, 0f, 15.9f), new Vector3(14.5f, 0f, 15.9f), new Vector3(15.3f, 0f, 15.9f) };
            for (int ci = 0; ci < chestSpots.Length; ci++)
            {
                var cp = d.P(chestSpots[ci].x, chestSpots[ci].z, -2.35f);
                MazeBox("MazeTextileChest_" + ci, root, cp, new Vector3(.65f, .9f, .9f), wood);
                MazeBox("MazeTextileLid_" + ci, root, cp + Vector3.up * .47f, new Vector3(.69f, .05f, .94f), wood);
            }
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "MazeHandycamCrate", root,
                d.P(14.8f, 15.2f, -2.79f), Quaternion.identity, new Vector3(.5f, .5f, .5f), wood);
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "MazeCryptFlank", root,
                d.P(14.0f, 15.4f, -2.79f), Quaternion.Euler(0, -14, 0), new Vector3(.6f, .5f, .5f), wood);
            ProjectAsset("Assets/_LayarTerakhir/assets/Art/Phase3/Imported/PolyHaven/wooden_crate_02/wooden_crate_02.fbx", "MazeCryptBox", root,
                d.P(11.0f, 16.0f, -2.79f), Quaternion.Euler(0, 20, 0), new Vector3(.55f, .55f, .55f), wood);
            var cam2 = Find("DamarHandycam");
            if (cam2) cam2.transform.SetPositionAndRotation(d.P(14.8f, 15.2f, -2.2f), Quaternion.LookRotation(d.north));
            // Damar's torn bag at the maze mouth: the breadcrumb to the camera.
            var bag = MazeBox("DamarBagProp", root, d.P(7.5f, 4.2f, -2.62f), new Vector3(.45f, .3f, .28f), Material("Repair_BookCloth"), false);
            var strap = GameObject.CreatePrimitive(PrimitiveType.Cube);
            strap.name = "DamarBagStrap"; strap.transform.SetParent(bag.transform, false);
            strap.transform.localPosition = Vector3.up * .18f; strap.transform.localScale = new Vector3(.2f, .06f, .3f);
            strap.GetComponent<Renderer>().sharedMaterial = Material("Repair_Iron");
            Object.DestroyImmediate(strap.GetComponent<Collider>());
            var bagItem = bag.AddComponent<AtmajaItem>();
            bagItem.director = d; bagItem.action = AtmajaAction.DamarBag; bagItem.label = "Periksa tas ransel Damar";
            // One hiding locker per new room.
            var wardA = ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/painted_wooden_cabinet_02/painted_wooden_cabinet_02.fbx", "MazeWardrobeA", root,
                d.P(15.3f, 3.5f, -2.79f), Quaternion.Euler(0, -90, 0), new Vector3(.95f, 1.9f, .45f));
            var wardAItem = wardA.AddComponent<AtmajaItem>();
            wardAItem.director = d; wardAItem.action = AtmajaAction.WardrobeHide; wardAItem.label = "Sembunyi di lemari kayu #4";
            d.basement.wardrobeCol4 = wardA.GetComponent<BoxCollider>();
            d.basement.hideAnchor4 = AnchorAt(root, "HideAnchor4", d.P(14.4f, 3.5f, -2.79f), d.P(15.3f, 3.5f, 0));
            d.basement.exitAnchor4 = AnchorAt(root, "ExitAnchor4", d.P(14.2f, 4.4f, -2.79f), d.P(15.3f, 3.5f, 0));
            var wardB = ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/painted_wooden_cabinet_02/painted_wooden_cabinet_02.fbx", "MazeWardrobeB", root,
                d.P(8.6f, 15.8f, -2.79f), Quaternion.identity, new Vector3(.95f, 1.9f, .45f));
            var wardBItem = wardB.AddComponent<AtmajaItem>();
            wardBItem.director = d; wardBItem.action = AtmajaAction.WardrobeHide; wardBItem.label = "Sembunyi di lemari kayu #5";
            d.basement.wardrobeCol5 = wardB.GetComponent<BoxCollider>();
            d.basement.hideAnchor5 = AnchorAt(root, "HideAnchor5", d.P(9.5f, 15.8f, -2.79f), d.P(8.6f, 15.8f, 0));
            d.basement.exitAnchor5 = AnchorAt(root, "ExitAnchor5", d.P(9.5f, 14.9f, -2.79f), d.P(8.6f, 15.8f, 0));
            var wardC = ProjectAsset("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/painted_wooden_cabinet_02/painted_wooden_cabinet_02.fbx", "MazeWardrobeC", root,
                d.P(1.0f, 10.0f, -2.79f), Quaternion.identity, new Vector3(.95f, 1.9f, .45f));
            var wardCItem = wardC.AddComponent<AtmajaItem>();
            wardCItem.director = d; wardCItem.action = AtmajaAction.WardrobeHide; wardCItem.label = "Sembunyi di lemari kayu #6";
            d.basement.wardrobeCol6 = wardC.GetComponent<BoxCollider>();
            d.basement.hideAnchor6 = AnchorAt(root, "HideAnchor6", d.P(1.9f, 10.0f, -2.79f), d.P(1.0f, 10.0f, 0));
            d.basement.exitAnchor6 = AnchorAt(root, "ExitAnchor6", d.P(1.9f, 9.2f, -2.79f), d.P(1.0f, 10.0f, 0));
            // Patrol the new rooms too (existing indices preserved). The old
            // kitchen-well shaft drops through R3 at (2.85,11.2), so the R3
            // post stands east of it, clear of the stones.
            foreach (var pt in new[] { d.P(13.0f, 6.0f, -2.79f), d.P(14.5f, 9.0f, -2.79f), d.P(10.0f, 13.5f, -2.79f), d.P(14.0f, 14.0f, -2.79f), d.P(3.2f, 8.5f, -2.79f), d.P(4.5f, 10.5f, -2.79f) })
            {
                var t = new GameObject("MazeBranchPatrol" + bi++).transform;
                t.SetParent(root, true);
                t.position = pt;
                t.rotation = Quaternion.LookRotation(d.P(2.9f, 3.5f, 0) - pt);
                wps.Add(t);
            }
            d.basement.waypoints = wps.ToArray();
            RebuildMazeRoom1(d, root);
            RebuildMazeDoors(d, root);
            RebuildMazeHorror(d, root);
            RebuildBasementAtmosphere(d, root);
        }

        // Designer Nov: basement emergence staging + wayfinding breadcrumbs.
        // Tirai putih tempat Nyonya muncul (spec Adegan 5), robekan kain tas
        // Damar sebagai penunjuk jalan ke crypt (diegetik, tanpa sebut lokasi
        // di catatan), dan rembesan dinding R3 banjir. Semua visual-only
        // (non-colliding) agar sapu 87 segmen CellarWalk47 tetap bersih.
        private static void RebuildBasementAtmosphere(AtmajaDirector d, Transform root)
        {
            var linen = Material("Repair_Linen");
            var cloth = Material("Repair_BookCloth");
            // Tirai emergence: dua panel kain putih + batang kayu di belakang
            // spawn Nyonya, di timur lane spawn-bench agar tak memblokir.
            Delete("MazeEmergeRod");
            var rod = MazeBox("MazeEmergeRod", root, d.P(3.3f, 6.0f, -1.05f), new Vector3(.95f, .04f, .04f), Material("Repair_BedWood"), false);
            rod.transform.rotation = Quaternion.LookRotation(d.east);
            for (int i = 0; i < 2; i++)
            {
                string nm = "MazeEmergeCurtain_" + i;
                Delete(nm);
                var panel = GameObject.CreatePrimitive(PrimitiveType.Quad);
                panel.name = nm;
                panel.transform.SetParent(root, true);
                panel.transform.SetPositionAndRotation(
                    d.P(3.05f + i * .5f, 6.0f, -1.85f),
                    Quaternion.LookRotation(-d.east));
                panel.transform.localScale = new Vector3(.45f, 1.6f, 1f);
                Object.DestroyImmediate(panel.GetComponent<Collider>());
                panel.GetComponent<Renderer>().sharedMaterial = linen;
                panel.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
            // Breadcrumb: robekan kain tas Damar dari mulut maze ke crypt.
            var crumbs = new (float, float, float)[] { (7.0f, 5.8f, 20f), (10.5f, 5.8f, -15f), (13.5f, 8.0f, 35f), (13.5f, 12.5f, -30f) };
            for (int ci = 0; ci < crumbs.Length; ci++)
            {
                string nm = "MazeDamarScrap_" + ci;
                Delete(nm);
                var q = GameObject.CreatePrimitive(PrimitiveType.Quad);
                q.name = nm;
                q.transform.SetParent(root, true);
                q.transform.SetPositionAndRotation(
                    d.P(crumbs[ci].Item1, crumbs[ci].Item2, -2.785f),
                    Quaternion.Euler(90, 0, crumbs[ci].Item3));
                q.transform.localScale = new Vector3(.18f, .12f, 1f);
                Object.DestroyImmediate(q.GetComponent<Collider>());
                q.GetComponent<Renderer>().sharedMaterial = cloth;
                q.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
            // Rembesan R3: dua streak basah vertikal di dinding barat.
            var wet = MazeFlatMat("MazeWetStreak", new Color(.10f, .13f, .12f), 0f, .85f);
            var streaks = new (float, float)[] { (8.5f, 1.1f), (10.0f, 1.4f) };
            for (int si = 0; si < streaks.Length; si++)
            {
                Quad("MazeWetStreak_" + si, root,
                    d.P(0.30f, streaks[si].Item1, -1.9f + streaks[si].Item2 * .1f),
                    Quaternion.LookRotation(d.east),
                    new Vector2(.32f, 1.15f), wet);
            }
            // Ditinggalkan berdebu + karatan tua: dust sheets di atas
            // crate/meja/peti (tipis, non-colliding, offset anti z-fight),
            // rust streaks vertikal di tiang/engsel besi, dan dust motes
            // melayang per ruangan (particle beige lambat, visual-only).
            var dust = MazeFlatMat("MazeDustSheet", new Color(.62f, .58f, .48f), 0f, .05f);
            var dustTops = new (string, float, float, float, float)[]
            {
                ("MazeHandycamCrate", 14.8f, 15.2f, .52f, .42f),
                ("MazeCryptBox", 11.0f, 16.0f, .57f, .47f),
                ("MazeTextileLid_0", 12.5f, 15.9f, .69f, .94f),
                ("MazeWoodBox_0", 15.35f, 9.0f, .55f, .55f),
            };
            for (int di = 0; di < dustTops.Length; di++)
            {
                var dt = dustTops[di];
                Quad("MazeDust_" + di, root,
                    d.P(dt.Item2, dt.Item3, -2.79f + .55f + .006f),
                    Quaternion.Euler(90, 0, di * 17f),
                    new Vector2(dt.Item4 * .92f, dt.Item5 * .92f), dust);
            }
            // Dust di atas meja riset: di sela kertas, bukan menutupinya.
            Quad("MazeDustTable", root,
                d.P(14.35f, 10.35f, -2.79f + .782f),
                Quaternion.Euler(90, 0, 8f),
                new Vector2(.30f, .18f), dust);
            // Dust caps di atas semua lemari persembunyian (6): tanda
            // ruangan ditinggalkan, terlihat saat senter menyapu ke atas.
            var lockerTops = new (float, float)[]
            {
                (2.6f, 1.3f), (4.95f, 1.05f), (11.3f, 10.5f),
                (15.3f, 3.5f), (8.6f, 15.8f), (1.0f, 10.0f),
            };
            for (int li = 0; li < lockerTops.Length; li++)
            {
                Quad("MazeDustLocker_" + li, root,
                    d.P(lockerTops[li].Item1, lockerTops[li].Item2, -2.79f + 1.906f),
                    Quaternion.Euler(90, 0, li * 23f),
                    new Vector2(.86f, .38f), dust);
            }
            var rust = MazeFlatMat("MazeRustStreak", new Color(.36f, .20f, .10f), .35f, .5f);
            var rustSpots = new (float, float, float)[]
            {
                (14.05f, 2.75f, 0f), (14.95f, 2.75f, 0f),
                (12.0f, 4.0f, 90f), (13.0f, 12.5f, 90f),
                (2.85f, 11.2f, 0f), (5.1f, 2.2f, 0f),
            };
            for (int ri = 0; ri < rustSpots.Length; ri++)
            {
                var rs = rustSpots[ri];
                Quad("MazeRust_" + ri, root,
                    d.P(rs.Item1, rs.Item2, -1.7f),
                    Quaternion.Euler(0, rs.Item3, 0),
                    new Vector2(.18f, .9f), rust);
            }
            // Dust motes per ruangan: beige lambat, 12 partikel, tanpa collision.
            var mist = AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Phase2/Materials/OperationMist.mat");
            var moteSpots = new (string, float, float)[]
            {
                ("MazeDustWest", 2.9f, 3.5f), ("MazeDustR1", 14.0f, 7.5f),
                ("MazeDustR2", 12.0f, 14.5f), ("MazeDustR3", 3.0f, 9.5f),
            };
            foreach (var ms in moteSpots)
            {
                Delete(ms.Item1);
                var mo = new GameObject(ms.Item1);
                mo.transform.SetParent(root, true);
                mo.transform.position = d.P(ms.Item2, ms.Item3, -1.6f);
                var ps = mo.AddComponent<ParticleSystem>();
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                var main = ps.main;
                main.loop = true; main.playOnAwake = true; main.duration = 4f;
                main.startLifetime = new ParticleSystem.MinMaxCurve(2.5f, 4f);
                main.startSpeed = new ParticleSystem.MinMaxCurve(.01f, .04f);
                main.startSize = new ParticleSystem.MinMaxCurve(.008f, .025f);
                main.startColor = new Color(.78f, .74f, .64f, .5f);
                main.maxParticles = 12;
                main.simulationSpace = ParticleSystemSimulationSpace.World;
                main.gravityModifier = -.005f;
                var emission = ps.emission;
                emission.rateOverTime = 3f;
                var shape = ps.shape;
                shape.shapeType = ParticleSystemShapeType.Box;
                shape.scale = new Vector3(1.5f, 1.0f, 1.0f);
                var pr = mo.GetComponent<ParticleSystemRenderer>();
                pr.sharedMaterial = mist;
                pr.renderMode = ParticleSystemRenderMode.Billboard;
            }
        }

        // Designer Nov: the secret cellar gets its horror dressing, spread so
        // no two rooms repeat an asset. R1 archive: research table with
        // scattered papers, unlabeled wooden boxes, a standing rack of
        // smashed jars plus a toppled rack with bottle shards. R2 crypt: a
        // dried-blood star circle ringed by five candles around a black tome.
        // R3 flooded store: a black trash-bag nest (14) with stink smoke over
        // a half-hidden shrouded corpse and a blood pool. Idempotent.
        private static void RebuildMazeHorror(AtmajaDirector d, Transform root)
        {
            var wood = Material("Repair_BedWood");
            var paper = Material("Repair_Paper");
            var cloth = Material("Repair_BookCloth");
            var linen = Material("Repair_Linen");
            var glassGrey = MazeFlatMat("MazeGlassGrey", new Color(.42f, .44f, .40f), 0f, .5f);
            var glassGreen = MazeFlatMat("MazeGlassGreen", new Color(.13f, .26f, .14f), 0f, .55f);
            // Ritual circle decal (dried blood on transparency).
            var ritualPath = Mats + "MazeRitualCircle.mat";
            var ritual = AssetDatabase.LoadAssetAtPath<Material>(ritualPath);
            if (!ritual)
            {
                ritual = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(ritual, ritualPath);
            }
            ritual.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(Texs + "RitualCircleBlood.png"));
            ritual.SetColor("_BaseColor", Color.white);
            ritual.SetFloat("_Surface", 1);
            ritual.SetFloat("_SrcBlend", 5);
            ritual.SetFloat("_DstBlend", 10);
            ritual.SetFloat("_ZWrite", 0);
            ritual.SetFloat("_Cull", 0);
            ritual.SetFloat("_Smoothness", .3f);
            ritual.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            ritual.renderQueue = 3000;
            ritual.SetOverrideTag("RenderType", "Transparent");
            EditorUtility.SetDirty(ritual);
            // Candle flame (emissive, no real light).
            var flamePath = Mats + "MazeFlame.mat";
            var flameMat = AssetDatabase.LoadAssetAtPath<Material>(flamePath);
            if (!flameMat)
            {
                flameMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(flameMat, flamePath);
            }
            flameMat.SetColor("_BaseColor", new Color(.05f, .02f, .01f));
            flameMat.SetColor("_EmissionColor", new Color(1.5f, .6f, .15f));
            flameMat.EnableKeyword("_EMISSION");
            EditorUtility.SetDirty(flameMat);
            var waxMat = MazeFlatMat("MazeCandleWax", new Color(.86f, .82f, .72f), 0f, .4f);
            var bagMat = MazeFlatMat("MazeTrashBag", new Color(.02f, .02f, .025f), 0f, .55f);

            // ---- R2 crypt: star circle, five candles, black tome ----
            Quad("MazeRitualCircle", root, d.P(11f, 15.3f, -2.795f), Quaternion.Euler(90, 0, 0), new Vector2(2.4f, 2.4f), ritual);
            var candleSpots = new[] { (11.00f, 14.22f), (9.97f, 14.97f), (10.36f, 16.17f), (11.64f, 16.17f), (12.03f, 14.97f) };
            for (int ci = 0; ci < candleSpots.Length; ci++)
            {
                Delete("MazeCandle_" + ci); Delete("MazeFlame_" + ci);
                var stick = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                stick.name = "MazeCandle_" + ci;
                stick.transform.SetParent(root, true);
                stick.transform.position = d.P(candleSpots[ci].Item1, candleSpots[ci].Item2, -2.79f + .11f);
                stick.transform.localScale = new Vector3(.07f, .22f, .07f);
                stick.GetComponent<Renderer>().sharedMaterial = waxMat;
                stick.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(stick.GetComponent<Collider>());
                var flame = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                flame.name = "MazeFlame_" + ci;
                flame.transform.SetParent(root, true);
                flame.transform.position = d.P(candleSpots[ci].Item1, candleSpots[ci].Item2, -2.79f + .27f);
                flame.transform.localScale = new Vector3(.05f, .09f, .05f);
                flame.GetComponent<Renderer>().sharedMaterial = flameMat;
                flame.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(flame.GetComponent<Collider>());
            }
            Delete("MazeRitualBook"); Delete("MazeRitualPages");
            var tome = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tome.name = "MazeRitualBook";
            tome.transform.SetParent(root, true);
            tome.transform.position = d.P(11f, 15.3f, -2.79f + .025f);
            tome.transform.localScale = new Vector3(.32f, .05f, .24f);
            tome.GetComponent<Renderer>().sharedMaterial = cloth;
            tome.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            var pages = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pages.name = "MazeRitualPages";
            pages.transform.SetParent(root, true);
            pages.transform.position = d.P(11f, 15.3f, -2.79f + .07f);
            pages.transform.localScale = new Vector3(.28f, .04f, .20f);
            pages.GetComponent<Renderer>().sharedMaterial = paper;
            pages.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            Object.DestroyImmediate(pages.GetComponent<Collider>());

            // ---- R3 flooded store: trash-bag nest, shroud, pool, stink ----
            for (int b = 0; b < 14; b++)
            {
                Delete("MazeTrashBag_" + b); Delete("MazeTrashKnot_" + b);
                bool top = b >= 9;
                int k = top ? b - 9 : b;
                float ang = (top ? 20f + k * 72f : k * 40f) * Mathf.Deg2Rad;
                float rr = top ? .25f : .42f + (k % 2) * .08f;
                float sc = (top ? .48f : .55f) + (k % 3) * .04f;
                var bag = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                bag.name = "MazeTrashBag_" + b;
                bag.transform.SetParent(root, true);
                bag.transform.position = d.P(5.6f + Mathf.Cos(ang) * rr, 11.5f + Mathf.Sin(ang) * rr, -2.79f + .20f * sc + (top ? .38f : 0f));
                bag.transform.localScale = new Vector3(.55f * sc, .42f * sc, .55f * sc);
                bag.transform.rotation = Quaternion.Euler(0, k * 37f, 0);
                bag.GetComponent<Renderer>().sharedMaterial = bagMat;
                bag.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                if (top)
                {
                    var knot = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    knot.name = "MazeTrashKnot_" + b;
                    knot.transform.SetParent(bag.transform, false);
                    knot.transform.localPosition = Vector3.up * .55f;
                    knot.transform.localScale = new Vector3(.18f, .3f, .18f);
                    knot.GetComponent<Renderer>().sharedMaterial = bagMat;
                    knot.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                    Object.DestroyImmediate(knot.GetComponent<Collider>());
                }
            }
            Delete("MazeShroud");
            var shroud = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            shroud.name = "MazeShroud";
            shroud.transform.SetParent(root, true);
            shroud.transform.SetPositionAndRotation(d.P(5.5f, 11.3f, -2.79f + .18f), Quaternion.Euler(0, 10f, 90f));
            shroud.transform.localScale = new Vector3(.36f, .55f, .36f);
            shroud.GetComponent<Renderer>().sharedMaterial = linen;
            shroud.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            Quad("MazeBloodPool", root, d.P(5.3f, 11.1f, -2.795f), Quaternion.Euler(90, 20, 0), new Vector2(.9f, .7f), DecalMaterial("BloodSpatter", false, true));
            // Stink smoke: same proven puff rig as the feast, sickly green.
            Delete("MazeStinkSmoke");
            var mist = AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Phase2/Materials/OperationMist.mat");
            if (!mist) throw new InvalidOperationException("OperationMist material missing for stink smoke");
            var stinkPath = Mats + "MazeStinkPuff.mat";
            var stink = AssetDatabase.LoadAssetAtPath<Material>(stinkPath);
            if (!stink)
            {
                stink = new Material(mist);
                AssetDatabase.CreateAsset(stink, stinkPath);
            }
            var puffTex = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47/SmokePuff.png");
            if (!puffTex) throw new InvalidOperationException("SmokePuff texture missing");
            stink.SetTexture("_BaseMap", puffTex);
            EditorUtility.SetDirty(stink);
            var stinkGo = new GameObject("MazeStinkSmoke");
            stinkGo.transform.SetParent(root, true);
            stinkGo.transform.position = d.P(5.6f, 11.5f, -2.2f);
            var ps = stinkGo.AddComponent<ParticleSystem>();
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            var main = ps.main;
            main.loop = true; main.playOnAwake = true; main.duration = 3f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(1.5f, 2.2f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(.02f, .05f);
            main.startSize = new ParticleSystem.MinMaxCurve(.12f, .24f);
            main.startColor = new Color(.35f, .42f, .28f, .7f);
            main.maxParticles = 20;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.gravityModifier = -.015f;
            var emission = ps.emission;
            emission.rateOverTime = 5f;
            var shape = ps.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(.5f, .15f, .5f);
            var pr = stinkGo.GetComponent<ParticleSystemRenderer>();
            pr.sharedMaterial = stink;
            pr.renderMode = ParticleSystemRenderMode.Billboard;
            pr.sortingFudge = 0;

            // ---- R1 archive: research table, wood boxes, smashed racks ----
            Delete("MazeResearchTable");
            var tableTop = GameObject.CreatePrimitive(PrimitiveType.Cube);
            tableTop.name = "MazeResearchTable";
            tableTop.transform.SetParent(root, true);
            tableTop.transform.position = d.P(14.6f, 10.2f, -2.79f + .75f);
            tableTop.transform.localScale = new Vector3(.9f, .06f, .6f);
            tableTop.GetComponent<Renderer>().sharedMaterial = wood;
            tableTop.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            for (int li = 0; li < 4; li++)
            {
                Delete("MazeTableLeg_" + li);
                var leg = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leg.name = "MazeTableLeg_" + li;
                leg.transform.SetParent(root, true);
                leg.transform.position = d.P(14.6f + (li % 2 == 0 ? -.4f : .4f), 10.2f + (li < 2 ? -.25f : .25f), -2.79f + .36f);
                leg.transform.localScale = new Vector3(.06f, .72f, .06f);
                leg.GetComponent<Renderer>().sharedMaterial = wood;
                leg.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(leg.GetComponent<Collider>());
            }
            float[] paperYaw = { 0f, 15f, -12f, 30f, -25f };
            for (int pi = 0; pi < paperYaw.Length; pi++)
            {
                Delete("MazeResearchPaper_" + pi);
                var sheet = GameObject.CreatePrimitive(PrimitiveType.Cube);
                sheet.name = "MazeResearchPaper_" + pi;
                sheet.transform.SetParent(root, true);
                sheet.transform.SetPositionAndRotation(
                    d.P(14.6f + ((pi * 37) % 5 - 2) * .09f, 10.2f + ((pi * 53) % 3 - 1) * .1f, -2.79f + .782f + pi * .0015f),
                    Quaternion.Euler(0, paperYaw[pi], 0));
                sheet.transform.localScale = new Vector3(.21f, .004f, .3f);
                sheet.GetComponent<Renderer>().sharedMaterial = paper;
                sheet.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(sheet.GetComponent<Collider>());
            }
            var boxSpots = new[] { (15.35f, 9.0f, .55f, 0f), (15.55f, 9.35f, .5f, 0f), (15.3f, 8.95f, .45f, .55f) };
            for (int wi = 0; wi < boxSpots.Length; wi++)
            {
                Delete("MazeWoodBox_" + wi);
                var wb = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wb.name = "MazeWoodBox_" + wi;
                wb.transform.SetParent(root, true);
                wb.transform.SetPositionAndRotation(
                    d.P(boxSpots[wi].Item1, boxSpots[wi].Item2, -2.79f + boxSpots[wi].Item3 / 2 + boxSpots[wi].Item4),
                    Quaternion.Euler(0, wi * 9f, 0));
                wb.transform.localScale = Vector3.one * boxSpots[wi].Item3;
                wb.GetComponent<Renderer>().sharedMaterial = wood;
                wb.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
            // Standing rack of smashed jars (north wall) + toppled rack (SE).
            Delete("MazeStandingRack");
            var rack = new GameObject("MazeStandingRack");
            rack.transform.SetParent(root, true);
            rack.transform.position = d.P(14.5f, 2.75f, -2.79f);
            foreach (float px in new[] { -.55f, .55f })
            {
                var post = GameObject.CreatePrimitive(PrimitiveType.Cube);
                post.name = "MazeRackPost";
                post.transform.SetParent(rack.transform, false);
                post.transform.localPosition = new Vector3(px, .9f, 0);
                post.transform.localScale = new Vector3(.08f, 1.8f, .08f);
                post.GetComponent<Renderer>().sharedMaterial = wood;
                post.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(post.GetComponent<Collider>());
            }
            for (int si = 0; si < 3; si++)
            {
                var shelf = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shelf.name = "MazeRackShelf";
                shelf.transform.SetParent(rack.transform, false);
                shelf.transform.localPosition = new Vector3(0, .35f + si * .6f, 0);
                shelf.transform.localScale = new Vector3(1.2f, .06f, .4f);
                shelf.GetComponent<Renderer>().sharedMaterial = wood;
                shelf.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
            for (int ji = 0; ji < 6; ji++)
            {
                Delete("MazeJar_" + ji);
                var jar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                jar.name = "MazeJar_" + ji;
                jar.transform.SetParent(rack.transform, false);
                int shelfI = ji / 2;
                jar.transform.localPosition = new Vector3(ji % 2 == 0 ? -.3f : .3f, .35f + shelfI * .6f + .12f, 0);
                jar.transform.localRotation = Quaternion.Euler(ji % 2 == 0 ? 72f : -64f, ji * 31f, ji % 3 == 0 ? 18f : -12f);
                jar.transform.localScale = new Vector3(.09f, .18f, .09f);
                jar.GetComponent<Renderer>().sharedMaterial = glassGrey;
                jar.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(jar.GetComponent<Collider>());
            }
            for (int gi = 0; gi < 3; gi++)
            {
                Delete("MazeJarShard_" + gi);
                var shard = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shard.name = "MazeJarShard_" + gi;
                shard.transform.SetParent(rack.transform, false);
                shard.transform.localPosition = new Vector3(-.2f + gi * .2f, .98f, .08f);
                shard.transform.localRotation = Quaternion.Euler(0, gi * 47f, 0);
                shard.transform.localScale = new Vector3(.05f, .012f, .04f);
                shard.GetComponent<Renderer>().sharedMaterial = glassGrey;
                shard.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(shard.GetComponent<Collider>());
            }
            Delete("MazeFallenRack");
            var fallen = new GameObject("MazeFallenRack");
            fallen.transform.SetParent(root, true);
            fallen.transform.position = d.P(14.8f, 11.5f, -2.79f);
            foreach (float rz in new[] { -.2f, .2f })
            {
                var rail = GameObject.CreatePrimitive(PrimitiveType.Cube);
                rail.name = "MazeFallenRail";
                rail.transform.SetParent(fallen.transform, false);
                rail.transform.localPosition = new Vector3(0, .04f, rz);
                rail.transform.localScale = new Vector3(1.2f, .08f, .08f);
                rail.GetComponent<Renderer>().sharedMaterial = wood;
                rail.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
            for (int fi = 0; fi < 2; fi++)
            {
                var plank = GameObject.CreatePrimitive(PrimitiveType.Cube);
                plank.name = "MazeFallenPlank";
                plank.transform.SetParent(fallen.transform, false);
                plank.transform.localPosition = new Vector3(-.3f + fi * .6f, .1f, 0);
                plank.transform.localRotation = Quaternion.Euler(0, fi == 0 ? 12f : -9f, 0);
                plank.transform.localScale = new Vector3(.25f, .04f, .6f);
                plank.GetComponent<Renderer>().sharedMaterial = wood;
                plank.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(plank.GetComponent<Collider>());
            }
            for (int bi2 = 0; bi2 < 5; bi2++)
            {
                Delete("MazeBottleShard_" + bi2);
                var bs = GameObject.CreatePrimitive(PrimitiveType.Cube);
                bs.name = "MazeBottleShard_" + bi2;
                bs.transform.SetParent(fallen.transform, false);
                bs.transform.localPosition = new Vector3(-.5f + bi2 * .25f, .012f, -.35f + (bi2 % 2) * .7f);
                bs.transform.localRotation = Quaternion.Euler(0, bi2 * 53f, 0);
                bs.transform.localScale = new Vector3(.04f + (bi2 % 3) * .02f, .012f, .05f);
                bs.GetComponent<Renderer>().sharedMaterial = glassGreen;
                bs.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(bs.GetComponent<Collider>());
            }
            for (int ni = 0; ni < 2; ni++)
            {
                Delete("MazeBottleNeck_" + ni);
                var neck = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                neck.name = "MazeBottleNeck_" + ni;
                neck.transform.SetParent(fallen.transform, false);
                neck.transform.localPosition = new Vector3(.35f - ni * .7f, .05f, .1f - ni * .3f);
                neck.transform.localRotation = Quaternion.Euler(70f, 0, 20f * (ni == 0 ? 1f : -1f));
                neck.transform.localScale = new Vector3(.07f, .14f, .07f);
                neck.GetComponent<Renderer>().sharedMaterial = glassGreen;
                neck.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(neck.GetComponent<Collider>());
            }
        }

        private static Material MazeFlatMat(string name, Color color, float metallic, float smooth)
        {
            string path = Mats + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!mat)
            {
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.SetColor("_BaseColor", color);
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Smoothness", smooth);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        // Designer Nov: every maze room gets a real wooden plank door in its
        // main doorway — never a bare corridor gap. Leaves serialize OPEN
        // (flat into their rooms, clear of every walk lane); Raka toggles
        // them with E while Nyonya shoulders through via MazeDoor.
        private static void RebuildMazeDoors(AtmajaDirector d, Transform root)
        {
            var wood = Material("Repair_BedWood");
            string ipath = Mats + "MazeDoorIron.mat";
            var iron = AssetDatabase.LoadAssetAtPath<Material>(ipath);
            if (!iron)
            {
                iron = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(iron, ipath);
            }
            // Abandoned-house iron: rusted brown, half metal, low gloss.
            iron.SetColor("_BaseColor", new Color(.38f, .22f, .13f));
            iron.SetFloat("_Metallic", .55f);
            iron.SetFloat("_Smoothness", .35f);
            EditorUtility.SetDirty(iron);
            var east = d.east; east.y = 0; east.Normalize();
            var north = d.north; north.y = 0; north.Normalize();
            // R3 flooded store: WallS opening x2.0-3.0, leaf opens south in.
            MazeDoorLeaf("MazeDoorR3", root, d, d.P(2.0f, 6.9f, -2.79f), east, north, wood, iron);
            // R1 archive: east-wall north gap z4.0-5.0, leaf opens east in.
            MazeDoorLeaf("MazeDoorR1", root, d, d.P(12f, 4.0f, -2.79f), north, east, wood, iron);
            // R2 crypt: north opening x13-14; leaf parks NORTH into R1 so the
            // crypt-west lane behind it stays clear.
            MazeDoorLeaf("MazeDoorR2", root, d, d.P(13f, 12.5f, -2.79f), east, -north, wood, iron);
        }

        // jambFloor = hinge-side floor point; closedL = leaf direction when
        // shut (along the wall); openDir = where the leaf parks when open.
        private static void MazeDoorLeaf(string name, Transform root, AtmajaDirector d, Vector3 jambFloor, Vector3 closedL, Vector3 openDir, Material wood, Material iron)
        {
            const float openingW = 1.0f, leafW = .92f, leafH = 2.1f;
            Delete(name); Delete(name + "Hinge"); Delete(name + "Frame");
            var L0 = closedL; L0.y = 0; L0.Normalize();
            var O0 = openDir; O0.y = 0; O0.Normalize();
            // Hinge yaw with local +X = closed leaf direction.
            var hinge = new GameObject(name + "Hinge").transform;
            hinge.SetParent(root, true);
            hinge.SetPositionAndRotation(jambFloor, Quaternion.LookRotation(Vector3.Cross(L0, Vector3.up), Vector3.up));
            var leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leaf.name = name;
            leaf.transform.SetParent(hinge, false);
            leaf.transform.localPosition = new Vector3(leafW / 2 - .02f, leafH / 2, 0);
            leaf.transform.localScale = new Vector3(leafW, leafH, .06f);
            leaf.GetComponent<Renderer>().sharedMaterial = wood;
            leaf.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            // Ledged planks + rusty knob ride the hinge beside the leaf (visual
            // only, meter units — never scaled children of the leaf slab).
            foreach (float fy in new[] { .65f, 1.45f })
            {
                var batten = GameObject.CreatePrimitive(PrimitiveType.Cube);
                batten.name = name + "Batten";
                batten.transform.SetParent(hinge, false);
                batten.transform.localPosition = new Vector3(leafW / 2 - .02f, fy, .045f);
                batten.transform.localScale = new Vector3(.82f, .12f, .03f);
                batten.GetComponent<Renderer>().sharedMaterial = wood;
                batten.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                Object.DestroyImmediate(batten.GetComponent<Collider>());
            }
            var knob = GameObject.CreatePrimitive(PrimitiveType.Cube);
            knob.name = name + "Knob";
            knob.transform.SetParent(hinge, false);
            knob.transform.localPosition = new Vector3(leafW - .12f, 1.0f, .07f);
            knob.transform.localScale = new Vector3(.06f, .12f, .08f);
            knob.GetComponent<Renderer>().sharedMaterial = iron;
            knob.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            Object.DestroyImmediate(knob.GetComponent<Collider>());
            var hd = leaf.AddComponent<HingedDoor>();
            hd.hinge = hinge;
            hd.label = "Buka pintu kayu";
            hd.locked = false;
            hd.angle = Mathf.Clamp(Vector3.SignedAngle(L0, O0, Vector3.up), -105f, 105f);
            var frame = new GameObject(name + "Frame").transform;
            frame.SetParent(root, true);
            frame.SetPositionAndRotation(jambFloor, hinge.rotation);
            foreach (float lx in new[] { .04f, openingW - .04f })
            {
                var jamb = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jamb.name = name + "Jamb";
                jamb.transform.SetParent(frame, false);
                jamb.transform.localPosition = new Vector3(lx, leafH / 2, 0);
                jamb.transform.localScale = new Vector3(.08f, leafH, .12f);
                jamb.GetComponent<Renderer>().sharedMaterial = wood;
                jamb.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            }
            var lintel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            lintel.name = name + "Lintel";
            lintel.transform.SetParent(frame, false);
            lintel.transform.localPosition = new Vector3(openingW / 2, leafH + .175f, 0);
            lintel.transform.localScale = new Vector3(openingW, .35f, .12f);
            lintel.GetComponent<Renderer>().sharedMaterial = wood;
            lintel.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            var mazedoor = hinge.gameObject.AddComponent<MazeDoor>();
            mazedoor.door = hd;
            // Serialize shut for consistency; MazeDoor.Awake opens at runtime.
            hd.SetOpenImmediate(false);
        }

        // Designer Nov: R1 east archive, dressed with realistic CC0 Poly Haven
        // props (Audit/maze-dressing-cc0-sources.json) — real-world scale,
        // authored URP materials over their diffuse + normal maps. R2/R3
        // follow in later slices. Idempotent.
        private static void RebuildMazeRoom1(AtmajaDirector d, Transform root)
        {
            const string PH = "Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/";
            foreach (string nor in new[]
            {
                PH + "cardboard_box_01/cardboard_box_01_nor_gl_1k.exr",
                PH + "vintage_suitcase/vintage_suitcase_01_nor_gl_1k.png",
                PH + "vintage_suitcase/vintage_suitcase_02_nor_gl_1k.png",
                PH + "cement_bag/cement_bag_nor_gl_1k.exr",
            })
            {
                var ni = AssetImporter.GetAtPath(nor) as TextureImporter;
                if (ni && ni.textureType != TextureImporterType.NormalMap)
                {
                    ni.textureType = TextureImporterType.NormalMap;
                    ni.SaveAndReimport();
                }
            }
            var cardMat = MazePropMaterial("MazeCardboard", PH + "cardboard_box_01/cardboard_box_01_diff_1k.jpg",
                PH + "cardboard_box_01/cardboard_box_01_nor_gl_1k.exr", 0f, .2f, .5f);
            var caseMat1 = MazePropMaterial("MazeSuitcase1", PH + "vintage_suitcase/vintage_suitcase_01_diff_1k.png",
                PH + "vintage_suitcase/vintage_suitcase_01_nor_gl_1k.png", .15f, .25f, .45f);
            var caseMat2 = MazePropMaterial("MazeSuitcase2", PH + "vintage_suitcase/vintage_suitcase_02_diff_1k.png",
                PH + "vintage_suitcase/vintage_suitcase_02_nor_gl_1k.png", .15f, .25f, .45f);
            var cementMat = MazePropMaterial("MazeCementBag", PH + "cement_bag/cement_bag_diff_1k.jpg",
                PH + "cement_bag/cement_bag_nor_gl_1k.exr", 0f, .1f, .35f);
            var box1 = MazeProp(PH + "cardboard_box_01/cardboard_box_01_1k.fbx", "MazeCardboardBox_1", root,
                d.P(13.0f, 3.0f, -2.79f), 12f, new[] { cardMat });
            float stacked = SafeBounds(box1).max.y + .002f;
            var box1b = SafeBounds(box1);
            MazeProp(PH + "cardboard_box_01/cardboard_box_01_1k.fbx", "MazeCardboardBox_2", root,
                new Vector3(box1b.center.x, stacked, box1b.center.z), -8f, new[] { cardMat });
            MazeProp(PH + "vintage_suitcase/vintage_suitcase_1k.fbx", "MazeSuitcasePair", root,
                d.P(15.3f, 6.0f, -2.79f), 90f, new[] { caseMat1, caseMat2 });
            MazeProp(PH + "cement_bag/cement_bag_1k.fbx", "MazeCementBag", root,
                d.P(12.6f, 3.4f, -2.79f), -20f, new[] { cementMat });
        }

        // Designer Nov: Rumah Atmaja has stood abandoned for decades — every
        // prop ships dusty (matte + desaturating beige veil) unless it is
        // iron, which rusts instead (handled per iron prop, not here).
        private static Material MazePropMaterial(string name, string diff, string nor, float metallic, float smooth, float dust = .45f)
        {
            string path = Mats + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!mat)
            {
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(diff));
            if (!string.IsNullOrEmpty(nor))
            {
                var n = AssetDatabase.LoadAssetAtPath<Texture2D>(nor);
                if (n) { mat.SetTexture("_BumpMap", n); mat.SetFloat("_BumpScale", 1f); mat.EnableKeyword("_NORMALMAP"); }
            }
            mat.SetColor("_BaseColor", Color.Lerp(Color.white, new Color(.78f, .74f, .66f), Mathf.Clamp01(dust)));
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Smoothness", smooth);
            EditorUtility.SetDirty(mat);
            return mat;
        }

        // Real-world scale (no Resize distortion): the CC0 FBX are modeled in
        // meters, so the holder just takes the authored yaw, grounds to the
        // floor and fits a collider. Multi-material sets map by slot name.
        private static GameObject MazeProp(string fbxPath, string name, Transform root, Vector3 basePos, float yawDeg, Material[] mats)
        {
            Delete(name);
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(fbxPath);
            if (!asset) throw new InvalidOperationException("Missing CC0 prop " + fbxPath);
            var holder = new GameObject(name);
            holder.transform.SetParent(root, true);
            holder.transform.position = basePos;
            var child = (GameObject)PrefabUtility.InstantiatePrefab(asset);
            child.transform.SetParent(holder.transform, false);
            // Yaw the mesh child, never the holder: FitCollider maps world
            // bounds into holder-local, so a yawed holder swaps the collider
            // extents into the world (the 90° suitcase blocked its lane).
            child.transform.localRotation = Quaternion.Euler(0, yawDeg, 0);
            // Recentre on the holder regardless of FBX pivot placement.
            var pre = SafeBounds(holder);
            Vector3 shift = holder.transform.position - pre.center; shift.y = 0;
            child.transform.position += shift;
            foreach (var r in child.GetComponentsInChildren<Renderer>())
            {
                var slots = r.sharedMaterials;
                var mapped = new Material[slots.Length];
                for (int i = 0; i < slots.Length; i++)
                {
                    string sn = slots[i] ? slots[i].name : "";
                    int pick = 0;
                    if (mats.Length > 1 && (sn.EndsWith("02") || sn.EndsWith("_02") || sn.Contains("02"))) pick = 1;
                    mapped[i] = mats[Mathf.Clamp(pick, 0, mats.Length - 1)];
                }
                r.sharedMaterials = mapped;
            }
            Ground(holder, basePos.y);
            FitCollider(holder);
            return holder;
        }

        private static Transform AnchorAt(Transform root, string name, Vector3 pos, Vector3 look)
        {
            Delete(name);
            var t = new GameObject(name).transform;
            t.SetParent(root, true);
            var dir = look - pos; dir.y = 0;
            t.SetPositionAndRotation(pos, Quaternion.LookRotation(dir.sqrMagnitude > .01f ? dir : Vector3.forward));
            return t;
        }

        private static void BranchLight(Transform root, string name, Vector3 pos, float intensity, float range, Color color)
        {
            Delete(name);
            var lamp = MazeBox(name, root, pos, new Vector3(.3f, .06f, .12f), Material("Repair_Iron"), false);
            var light = new GameObject(name + "Glow", typeof(Light));
            light.transform.SetParent(lamp.transform, true);
            light.transform.localPosition = Vector3.down * .1f;
            var l = light.GetComponent<Light>();
            l.type = LightType.Point; l.range = range; l.intensity = intensity; l.color = color; l.shadows = LightShadows.None;
        }

        private static void RebuildScratchRelief(AtmajaDirector d, Transform root)
        {
            const string dir = "Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture/";
            var norImport = AssetImporter.GetAtPath(dir + "CrimsonRuptureWall_normal.jpg") as TextureImporter;
            if (norImport && norImport.textureType != TextureImporterType.NormalMap)
            {
                norImport.textureType = TextureImporterType.NormalMap;
                norImport.SaveAndReimport();
            }
            // Environment texture budget (AtmajaSceneValidation): cap hero
            // maps at 1024 like every other env asset.
            foreach (string tex in new[] { "CrimsonRuptureWall_albedo.png", "CrimsonRuptureWall_normal.jpg", "CrimsonRuptureWall_mask.png" })
            {
                var ti = AssetImporter.GetAtPath(dir + tex) as TextureImporter;
                if (ti && ti.maxTextureSize > 1024)
                {
                    ti.maxTextureSize = 1024;
                    ti.SaveAndReimport();
                }
            }
            // Authored material: opaque relief, wet gloss only on the slashes
            // (mask alpha), zero metal everywhere — mask R is 0 by construction.
            string mPath = Mats + "CrimsonRuptureWall.mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(mPath);
            if (!mat)
            {
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, mPath);
            }
            // Transparent claw decal ONLY: goal-photo blood strokes with
            // alpha, stuck on the REAL bathroom tile wall — the wall itself
            // is untouched (no painted poster background). Thin box holder
            // (quads get culled, boxes render). Meshy FBX stays in-repo.
            var decal = AssetDatabase.LoadAssetAtPath<Texture2D>(dir + "CrimsonGoalDecal.png");
            if (!decal) throw new InvalidOperationException("Missing goal claw decal (run Tools/extract_goal_claws.py)");
            mat.SetTexture("_BaseMap", decal);
            mat.SetTexture("_BumpMap", null); mat.SetFloat("_BumpScale", 0f); mat.DisableKeyword("_NORMALMAP");
            mat.SetTexture("_MetallicGlossMap", null); mat.DisableKeyword("_METALLICSPECGLOSSMAP");
            mat.SetColor("_BaseColor", Color.white);
            mat.SetFloat("_Metallic", 0f);
            mat.SetFloat("_Surface", 1f);
            mat.SetFloat("_SrcBlend", 5f);
            mat.SetFloat("_DstBlend", 10f);
            mat.SetFloat("_ZWrite", 0f);
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            mat.renderQueue = 3000;
            mat.SetOverrideTag("RenderType", "Transparent");
            mat.SetFloat("_Smoothness", 0.25f);
            mat.SetFloat("_SpecularHighlights", 0f);
            mat.SetFloat("_EnvironmentReflections", 0f);
            mat.SetFloat("_GlossyReflections", 1f);
            mat.SetFloat("_Cull", 2f);
            EditorUtility.SetDirty(mat);

            Delete("BathWallScratches");
            // Spread the designer's Gemini claw image: 4 decal boards on the
            // west wall (varied size/roll) so the bathroom carries dozens of
            // claw strokes, not one panel. Thin BOXES, not Quads: flat quads
            // have zero-thickness bounds and get culled into invisibility
            // (proven by the red-Unlit probe test). Front faces float off
            // the tiles; the wall itself is untouched.
            var east = d.east; east.y = 0; east.Normalize();
            Vector3 center = d.P(-2.27f, 2.9f, 4.94f);
            var decalSpecs = new (string, Vector3, Vector2, float)[]
            {
                ("CrimsonScratchRelief", new Vector3(2.275f, 4.94f, -2.9f), new Vector2(2.9f, 1.62f), 0f),
                ("CrimsonScratchRelief2", new Vector3(2.275f, 5.32f, -4.42f), new Vector2(1.6f, 0.89f), 180f),
                ("CrimsonScratchRelief3", new Vector3(2.275f, 4.32f, -1.42f), new Vector2(1.8f, 1.0f), 14f),
                ("CrimsonScratchRelief4", new Vector3(2.275f, 4.18f, -4.28f), new Vector2(1.5f, 0.84f), -18f),
            };
            foreach (var sp in decalSpecs)
            {
                Delete(sp.Item1);
                var holder = GameObject.CreatePrimitive(PrimitiveType.Cube);
                holder.name = sp.Item1;
                holder.transform.SetParent(root, true);
                holder.transform.SetPositionAndRotation(sp.Item2, Quaternion.LookRotation(east) * Quaternion.Euler(0, 0, sp.Item4));
                holder.transform.localScale = new Vector3(sp.Item3.x, sp.Item3.y, 0.02f);
                Object.DestroyImmediate(holder.GetComponent<Collider>());
                holder.GetComponent<Renderer>().sharedMaterial = mat;
                holder.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            }
            // Re-seat the AR trace + aim + inspect target on the relief face.
            Vector3 face = d.P(-2.27f, 2.9f, 4.94f);
            if (d.upper.scratchAim) d.upper.scratchAim.SetParent(root, true);
            Delete("BathWallARTrace");
            var trace = new GameObject("BathWallARTrace");
            trace.transform.SetParent(root, true);
            trace.transform.SetPositionAndRotation(face, Quaternion.LookRotation(east));
            d.upper.bathScratchesAR = trace;
            if (!d.upper.scratchAim) d.upper.scratchAim = new GameObject("ScratchAim").transform;
            d.upper.scratchAim.SetParent(trace.transform, true);
            d.upper.scratchAim.position = face;
            Delete("BathScratchTarget");
            var target = new GameObject("BathScratchTarget", typeof(BoxCollider), typeof(AtmajaItem));
            target.transform.SetParent(root, true);
            target.transform.SetPositionAndRotation(center, Quaternion.LookRotation(east));
            target.GetComponent<BoxCollider>().size = new Vector3(2.9f, 1.62f, .06f);
            var item = target.GetComponent<AtmajaItem>();
            item.director = d; item.action = AtmajaAction.BathScratches; item.label = "Periksa bekas cakaran di dinding";
        }

        private static void RebuildRustedVent(AtmajaDirector d, Transform root)
        {
            var norImport = AssetImporter.GetAtPath("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/metal_grate_rusty/metal_grate_rusty_nor_gl_1k.jpg") as TextureImporter;
            if (norImport && norImport.textureType != TextureImporterType.NormalMap)
            {
                norImport.textureType = TextureImporterType.NormalMap;
                norImport.SaveAndReimport();
            }
            if (d.upper.ventMouth) d.upper.ventMouth.SetParent(root, true);
            foreach (string oldV in new[] { "VentCabinet", "VentHoleDark", "VentHoleAR", "VentGrille", "VentGrilleDoor", "VentGrilleHinge", "VentDuctFrame", "VentDuctTrim_Top", "VentDuctTrim_Bottom", "VentDuctTrim_Left", "VentDuctTrim_Right", "VentBackDark_Top", "VentBackDark_Bottom" })
                Delete(oldV);
            d.upper.ventCabinet = null;
            // Designer Nov: the black duct trims and backing slabs are gone —
            // no dark clutter around the hole. The rusted grille grows to the
            // full wall opening (x -3.45..-2.55, y 3.84..4.88) so the prized
            // hole reads as one big enterable vent, never a slit.
            var hinge = new GameObject("VentGrilleHinge").transform;
            hinge.SetParent(root, true);
            hinge.SetPositionAndRotation(d.P(-3.45f, 5.24f, 4.36f), Quaternion.LookRotation(d.north));
            var cover = Model("VentGrille", "VentGrilleDoor", hinge, d.P(-3.0f, 5.24f, 4.36f), Quaternion.LookRotation(d.north));
            KosVisualV2.Resize(cover, new Vector3(.90f, 1.02f, .05f));
            KosVisualV2.SetCenter(cover, d.P(-3.0f, 5.24f, 4.36f));
            // Rust it: the authored mesh ships with flat grey iron. Dress every face
            // in the CC0 rusty grate so it reads decades old, never brand new.
            var rust = Material("Repair_VentRust");
            rust.SetColor("_BaseColor", new Color(.66f, .57f, .48f));
            var norGl = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/metal_grate_rusty/metal_grate_rusty_nor_gl_1k.jpg");
            if (norGl) { rust.SetTexture("_BumpMap", norGl); rust.SetFloat("_BumpScale", .6f); rust.EnableKeyword("_NORMALMAP"); EditorUtility.SetDirty(rust); }
            foreach (var r in cover.GetComponentsInChildren<Renderer>())
                r.sharedMaterials = Enumerable.Repeat(rust, r.sharedMaterials.Length).ToArray();
            FitCollider(cover);
            var vent = cover.AddComponent<HingedDoor>();
            vent.hinge = hinge;
            vent.angle = -105;
            vent.requiresCrouchAndAR = false;
            vent.label = "Congkel ventilasi berkarat";
            vent.locked = true;
            vent.lockedPrompt = "Berkarat — cari sesuatu buat mencongkelnya";
            vent.lockedDialogue = "Berkarat parah... terkunci. Aku harus mencari sesuatu untuk membukanya.";
            // Designer Nov: prying rips the grille off for good — persisted,
            // restored as gone, never a door that swings open and shut.
            vent.pryFlag = "EVENT_P0_VENT_PRIZED";
            vent.pryToast = "Ventilasi dijebol — teralis karatnya copot.";
            d.upper.ventDoor = vent;
            if (d.upper.ventMouth)
            {
                d.upper.ventMouth.position = d.P(-3.0f, 5.26f, 4.36f);
                foreach (var oldG in d.upper.ventMouth.GetComponentsInChildren<ARPhotoGlimmer>(true))
                    if (oldG) Object.DestroyImmediate(oldG.gameObject);
                ARPhotoGlimmer.Attach(d.upper.ventMouth, d.upper, "EVENT_P0_CHAIN1_BROKEN", true, true, false);
                d.upper.ventHoleAR = d.upper.ventMouth.GetComponentInChildren<ARPhotoGlimmer>(true).gameObject;
            }
        }

        private static void EnsureCrowbar(AtmajaDirector d, Transform env)
        {
            var norImport = AssetImporter.GetAtPath("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/crowbar/crowbar_01_nor_gl_1k.jpg") as TextureImporter;
            if (norImport && norImport.textureType != TextureImporterType.NormalMap)
            {
                norImport.textureType = TextureImporterType.NormalMap;
                norImport.SaveAndReimport();
            }
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven/crowbar/crowbar_01_1k.fbx");
            if (!asset) throw new Exception("CC0 crowbar FBX missing");
            var existing = Find("StorageCrowbar");
            if (existing && existing.GetComponentInChildren<MeshRenderer>(true))
            {
                d.crowbar = existing;
            }
            else
            {
                Delete("StorageCrowbar");
                var crowRoot = new GameObject("StorageCrowbar");
                crowRoot.transform.SetParent(env, false);
                var crowChild = (GameObject)PrefabUtility.InstantiatePrefab(asset);
                crowChild.transform.SetParent(crowRoot.transform, false);
                d.crowbar = crowRoot;
            }
            // Hidden under the lower stair flight (designer Nov): tucked in the
            // flight's shadow at its south foot, out of the open nook floor.
            d.crowbar.transform.SetPositionAndRotation(d.P(-0.6f, 7.9f, .56f), Quaternion.Euler(0, 35, 0));
            var item = d.crowbar.GetComponent<AtmajaItem>();
            if (!item) item = d.crowbar.AddComponent<AtmajaItem>();
            item.director = d;
            item.action = AtmajaAction.Crowbar;
            item.label = "Ambil linggis tua";
            // Normalize every run, not only on create: the bar must lie flat on
            // the floor under the stairs even if an earlier pass left it
            // standing on end or out on the open nook floor.
            for (int i = 0; i < 2; i++)
            {
                var cb = SafeBounds(d.crowbar);
                if (cb.size.y > cb.size.x && cb.size.y > cb.size.z)
                    d.crowbar.transform.rotation = d.crowbar.transform.rotation * Quaternion.Euler(90, 0, 0);
                else break;
            }
            Ground(d.crowbar, .5f);
            FitCollider(d.crowbar);
            LayarTerakhir.Player.PickupBlink.EnsureFor(d.crowbar, "ITEM_P0_CROWBAR");
        }

        // goal.md Nov: the kitchen south wall is a bare collision skeleton (piers +
        // lintel + invisible annex leaf) with walk-through gaps at local x 3.5/4.5,
        // and the KitchenCounter vanished. Seal with a visible wall + back door leaf
        // at the architect's x=4.0 opening, and restore the counter. Idempotent.
        public static void ApplyRumahAtmajaAnnexSeal()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var env = Find("Atmaja_Environment_Art").transform;
            var plaster = Old("House_MAT_PlasterDamaged");
            var wood = Old("House_MAT_WoodDoor");
            foreach (string n in new[] { "AnnexSouthWall_W", "AnnexSouthWall_E", "AnnexBackDoor", "AnnexBackDoorJamb_W", "AnnexBackDoorJamb_E", "AnnexBackDoorHeader" })
                Delete(n);
            var west = GameObject.CreatePrimitive(PrimitiveType.Cube);
            west.name = "AnnexSouthWall_W";
            west.transform.SetParent(env, false);
            west.transform.position = d.P(2.75f, 12.25f, 1.6f);
            west.transform.localScale = new Vector3(1.7f, 3.2f, .18f);
            west.GetComponent<Renderer>().sharedMaterial = plaster;
            var east = GameObject.CreatePrimitive(PrimitiveType.Cube);
            east.name = "AnnexSouthWall_E";
            east.transform.SetParent(env, false);
            east.transform.position = d.P(5.25f, 12.25f, 1.6f);
            east.transform.localScale = new Vector3(1.7f, 3.2f, .18f);
            east.GetComponent<Renderer>().sharedMaterial = plaster;
            var leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
            leaf.name = "AnnexBackDoor";
            leaf.transform.SetParent(env, false);
            leaf.transform.position = d.P(4.0f, 12.25f, 1.55f);
            leaf.transform.localScale = new Vector3(1.0f, 2.1f, .1f);
            leaf.GetComponent<Renderer>().sharedMaterial = wood;
            foreach (float x in new[] { 3.44f, 4.56f })
            {
                var jamb = GameObject.CreatePrimitive(PrimitiveType.Cube);
                jamb.name = "AnnexBackDoorJamb_" + (x < 4f ? "W" : "E");
                jamb.transform.SetParent(env, false);
                jamb.transform.position = d.P(x, 12.25f, 1.6f);
                jamb.transform.localScale = new Vector3(.12f, 3.2f, .18f);
                jamb.GetComponent<Renderer>().sharedMaterial = wood;
            }
            var header = GameObject.CreatePrimitive(PrimitiveType.Cube);
            header.name = "AnnexBackDoorHeader";
            header.transform.SetParent(env, false);
            header.transform.position = d.P(4.0f, 12.25f, 2.9f);
            header.transform.localScale = new Vector3(1.24f, .5f, .18f);
            header.GetComponent<Renderer>().sharedMaterial = plaster;
            Delete("KitchenCounter");
            var counter = GameObject.CreatePrimitive(PrimitiveType.Cube);
            counter.name = "KitchenCounter";
            counter.transform.SetParent(env, false);
            counter.transform.position = d.P(5.55f, 11.85f, .85f);
            counter.transform.localScale = new Vector3(.65f, 1f, .5f);
            counter.GetComponent<Renderer>().sharedMaterial = Old("House_MAT_PlasterLower");
            Save();
            Debug.Log("PROLOG47_RUMAH_ATMAJA_ANNEX_SEAL_APPLIED south wall, back door, counter");
        }

        public static void ApplyKosBathroomRound()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var root = Find("Kos_Building_Corridors").transform;

            // 1. Dining: the merged Kitchen+Dining portal (z 3.5..8) must not carry the
            // old corridor wainscot/chair-rail strips — they read as a leftover half wall.
            foreach (var t in root.GetComponentsInChildren<Transform>(true).Where(t =>
                t && (t.name == "HallSageWainscot" || t.name == "HallChairRail")).ToArray())
            {
                var lp = root.InverseTransformPoint(t.position);
                if (Mathf.Abs(lp.x + 1.39f) < .06f && Mathf.Abs(lp.z - 6f) < .06f && lp.y < 2f)
                    Object.DestroyImmediate(t.gameObject);
            }

            // 1b. Dining hole: the kitchen west wall (x=-4.8, ends z=6.05) and the dining outer
            // wall (x=-6.25, starts z=5.925) leave a diagonal notch open to the night sky.
            // Seal it with a full-height jog wall so the dining reads enclosed.
            Delete("DiningWestJogSeal");
            {
                var back = Find("DAPUR BERSAMABack");
                var seal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                seal.name = "DiningWestJogSeal";
                seal.transform.SetParent(root, false);
                seal.transform.localPosition = new Vector3(-5.55f, 1.45f, 6.0f);
                seal.transform.localScale = new Vector3(1.7f, 2.9f, .18f);
                var rend = seal.GetComponent<Renderer>();
                var backMat = back ? back.GetComponent<Renderer>().sharedMaterial : null;
                rend.sharedMaterial = backMat ? backMat : (Old("House_MAT_PlasterDamaged") ?? Material("Repair_Ceramic"));
            }
            // 2. Bathroom: no entrance door — keep only the PRIA + WANITA cubicle doors.
            foreach (string gone in new[] { "KAMAR MANDIDoor", "KAMAR MANDIHinge", "BathPocketDoorTrack" })
                Delete(gone);

            // 3. Main entrance: exactly one framed leaf that seals the opening (no see-through gaps).
            // Hinge on the EAST jamb, leaf swings OUTWARD to the EAST (left when entering) per designer.
            var hinge = Find("MainEntranceHinge");
            var director = Object.FindFirstObjectByType<KosCorridorDirector>();
            if (hinge)
            {
                foreach (var v in hinge.GetComponentsInChildren<Transform>(true).Where(t => t && t.name == "ModernDoorVisual").ToArray())
                    Object.DestroyImmediate(v.gameObject);
                foreach (var hd in hinge.GetComponentsInChildren<HingedDoor>(true).ToArray())
                    Object.DestroyImmediate(hd);
                var oldLeaf = Find("KosMainEntranceDoor");
                if (oldLeaf) Object.DestroyImmediate(oldLeaf);
                hinge.transform.SetParent(root, false);
                hinge.transform.localPosition = new Vector3(.9f, 0, -1.03f);
                hinge.transform.localRotation = Quaternion.identity;
                hinge.transform.localScale = Vector3.one;
                var leaf = GameObject.CreatePrimitive(PrimitiveType.Cube);
                leaf.name = "KosMainEntranceDoor";
                leaf.transform.SetParent(hinge.transform, false);
                leaf.transform.localPosition = new Vector3(-.9f, 1.16f, 0);
                leaf.transform.localScale = new Vector3(1.8f, 2.32f, .065f);
                var door = leaf.AddComponent<HingedDoor>();
                door.hinge = hinge.transform;
                door.label = "Buka pintu utama kos";
                door.angle = -105;
                door.locked = false;
                door.SetOpenImmediate(false);
                if (director) director.entranceDoor = door;
                UpgradeDoorToModernPanel(door, root);
                door.SetOpenImmediate(false);
            }

            // Kitchen sink backs against the south wall instead of floating mid-room.
            var kitchenBasin = Find("KosKitchenWashBasin");
            if (kitchenBasin)
            {
                var p = kitchenBasin.transform.localPosition;
                kitchenBasin.transform.localPosition = new Vector3(-3.15f, p.y, 2.85f);
            }

            // 4. Expanded natural bathrooms (CC0 downloads, see Audit/bathroom-set-cc0-sources.json).
            var treatsMat = Material("BathroomSet_TinyTreats");
            var bucketMat = Material("BathroomSet_Bucket");
            bucketMat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Imported/BathroomSet/Bucket3/Bucket3Albedo.png"));
            var bucketNormal = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Imported/BathroomSet/Bucket3/Bucket3_Normal.png");
            if (bucketNormal) { bucketMat.SetTexture("_BumpMap", bucketNormal); bucketMat.EnableKeyword("_NORMALMAP"); }
            var bucketMask = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/_LayarTerakhir/assets/Art/Imported/BathroomSet/Bucket3/Bucket3_mask.png");
            if (bucketMask)
            {
                bucketMat.SetTexture("_MetallicGlossMap", bucketMask);
                bucketMat.SetTexture("_OcclusionMap", bucketMask);
                bucketMat.EnableKeyword("_METALLICSPECGLOSSMAP");
                bucketMat.EnableKeyword("_OCCLUSIONMAP");
                bucketMat.SetFloat("_Smoothness", 1f);
            }
            EditorUtility.SetDirty(bucketMat);
            var paperMat = Material("BathroomSet_Paper");

            // Shared wash foyer: mirror + shelf + cup + brushes + toiletry bottle on the north wall by the basin.
            // (Shelf + dispenser raised: designer found the dispenser too low and facing the wall.)
            ImportedBath("TinyTreats/mirror.fbx", "KosBathroomMirror", root, new Vector3(2.30f, 1.50f, 5.895f), new Vector3(.4f, .5f, .06f), Quaternion.Euler(0, 180, 0), treatsMat);
            ImportedBath("TinyTreats/wall_shelf.fbx", "KosBathroomShelf", root, new Vector3(2.30f, 1.32f, 5.80f), new Vector3(.65f, .05f, .2f), Quaternion.identity, treatsMat);
            ImportedBath("TinyTreats/toothbrush_cup.fbx", "KosToothbrushCup", root, new Vector3(2.08f, 1.35f, 5.80f), new Vector3(.08f, .1f, .08f), Quaternion.identity, treatsMat);
            ImportedBath("TinyTreats/toothbrush_blue.fbx", "KosToothbrushBlue", root, new Vector3(2.24f, 1.35f, 5.80f), new Vector3(.17f, .025f, .03f), Quaternion.Euler(0, 90, 0), treatsMat);
            ImportedBath("TinyTreats/toothbrush_pink.fbx", "KosToothbrushPink", root, new Vector3(2.40f, 1.35f, 5.80f), new Vector3(.17f, .025f, .03f), Quaternion.Euler(0, 90, 0), treatsMat);
            ImportedBath("TinyTreats/container_bathroom_C_blue.fbx", "KosOdolBottle", root, new Vector3(2.55f, 1.35f, 5.80f), new Vector3(.07f, .11f, .07f), Quaternion.Euler(0, 180, 0), treatsMat);

            // Per-cubicle toilet paper; toilets keep facing the doorway.
            // (Designer: buckets were ugly and tub water removed — KosBathTub_*/KosBathBucket_* deleted.)
            // (Stale KosBathWater_* boxes from the earlier iteration are superseded by the modelled tub water.)
            foreach (string stale in new[] { "KosBathWater_PRIA", "KosBathWater_WANITA", "KosBathTub_PRIA", "KosBathTub_WANITA", "KosBathBucket_PRIA", "KosBathBucket_WANITA" }) Delete(stale);
            foreach (float z in new[] { 3.30f, 5.20f })
            {
                bool pria = z < 4f;
                float paperZ = pria ? 2.70f : 5.80f;
                ImportedBath("ToiletPaper/toiletpaper.fbx", "KosToiletPaper_" + (pria ? "PRIA" : "WANITA"), root, new Vector3(5.0f, 1.15f, paperZ), new Vector3(.13f, .13f, .11f), Quaternion.identity, paperMat);
            }

            // Cubicles were pitch black; one soft practical light each.
            foreach (var spec in new[] { ("KosBathroomLight_PRIA", 3.3f), ("KosBathroomLight_WANITA", 5.2f) })
            {
                var old = Find(spec.Item1);
                if (old) Object.DestroyImmediate(old);
                var go = new GameObject(spec.Item1, typeof(Light));
                go.transform.SetParent(root, false);
                go.transform.localPosition = new Vector3(4.5f, 2.35f, spec.Item2);
                var light = go.GetComponent<Light>();
                light.type = LightType.Point;
                light.intensity = 1.4f;
                light.range = 3.5f;
                light.color = new Color(1f, .86f, .66f);
                light.shadows = LightShadows.None;
            }

            Save();
            Debug.Log("KOS_BATHROOM_ROUND_APPLIED open foyer, sealed entrance, expanded natural bathrooms");
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
