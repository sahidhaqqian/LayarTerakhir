#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Editor-only dressing pass: DreadOut-level visual atmosphere & expanded 6.0m x 4.5m kos layout.
    public static class KosVisualUpgrade
    {
        private const string Art = "Assets/_LayarTerakhir/assets/Art";
        private const string Generated = Art + "/Generated";
        private static Transform root;
        private static Material wood, metal, plastic, cream, lowerWall, paper, fabric, brass;

        [MenuItem("LayarTerakhir/Rebuild & Upgrade Kamar Kos (DreadOut Edition)")]
        public static void RebuildAndUpgradeMenu()
        {
            PrologKosSceneBuilder.BuildSceneHeadless();
            Apply();
        }

        [MenuItem("LayarTerakhir/Apply Kos Visual Upgrade Only")]
        public static void ApplyMenu()
        {
            Apply();
        }

        public static void Apply()
        {
            Directory.CreateDirectory(Generated + "/Materials");
            Directory.CreateDirectory(Generated + "/Meshes");
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            ConfigureImports();
            var scene = EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);

            // Pastikan ukuran collider ruangan sudah menggunakan skala 6.0m x 4.5m x 3.1m
            UpgradeRoomGreyboxDimensions();

            var existing = GameObject.Find("Kos_Environment_Art");
            if (existing) Object.DestroyImmediate(existing);
            root = new GameObject("Kos_Environment_Art").transform;
            var room = GameObject.Find("KamarKos_4x3m_Greybox") ?? GameObject.Find("KamarKos_4x3m");
            room.name = "KamarKos_4x3m";
            root.SetParent(room.transform, false);

            foreach (string name in new[] { "Floor_4x3m", "Ceiling_4x3m", "Wall_North", "Wall_South", "Wall_East", "Wall_West", "KasurKos", "MejaKerjaPC", "KakiMejaKiri", "KakiMejaKanan", "LemariPakaian" })
            {
                var found = GameObject.Find(name);
                if (found && found.GetComponent<Renderer>())
                    found.GetComponent<Renderer>().enabled = false;
            }

            foreach (var item in Object.FindObjectsByType<KosInteractableItem>(FindObjectsSortMode.None))
            {
                item.GetComponent<Renderer>().enabled = false;
                foreach (Transform child in item.transform.Cast<Transform>().ToArray())
                    if (new[] { "computerScreen", "MonitorDashboard", "CalendarAugust", "StickyNote", "RentWarning", "cabinetBedDrawer", "CombinationPadlock", "doorwayFront", "HangingJacket", "Smartphone" }.Contains(child.name))
                        Object.DestroyImmediate(child.gameObject);
            }

            var originalPhone = Object.FindFirstObjectByType<SmartphoneController>();
            foreach (Transform child in originalPhone.handPhoneMeshRoot.transform.Cast<Transform>().ToArray())
                if (child.name == "Smartphone" || child.name == "SmartphoneUpright") Object.DestroyImmediate(child.gameObject);

            // Material PBR dengan palet warna lebih kelam, lembap, dan kontras khas DreadOut
            wood = Pbr("DoorAndWardrobeWood", "wood_table_worn", new Color(.58f, .51f, .40f), smoothness: .42f, bumpScale: 1.15f);
            metal = Pbr("DarkOxidisedMetal", "rusty_metal", new Color(.24f, .26f, .24f), smoothness: .38f, bumpScale: 1.25f);
            plastic = Plain("OldComputerPlastic", new Color(.08f, .095f, .095f), .08f, .32f);
            brass = Plain("WornBrass", new Color(.46f, .31f, .12f), .78f, .45f);
            cream = Pbr("OldLimewash", "painted_plaster_wall", new Color(.58f, .59f, .50f), smoothness: .25f, bumpScale: 1.10f);
            lowerWall = Pbr("PeelingSeaGreenPaint", "worn_plaster_wall", new Color(.26f, .36f, .30f), smoothness: .28f, bumpScale: 1.30f);
            paper = Plain("PaperEdges", new Color(.65f, .60f, .44f), 0, .12f);
            fabric = Pbr("FadedGreenJacket", "denim_fabric", new Color(.32f, .40f, .26f), smoothness: .18f, bumpScale: 1.0f);
            fabric.SetFloat("_Cull", 0);

            Architecture();
            Furniture();
            Interactables();
            Clutter();
            Lighting();
            ConfigurePipeline();

            var camera = Object.FindFirstObjectByType<Camera>();
            camera.nearClipPlane = .05f;
            camera.fieldOfView = 70f;
            camera.transform.localRotation = Quaternion.Euler(8f, 0, 0);
            camera.farClipPlane = 40f;
            var camData = camera.GetUniversalAdditionalCameraData();
            camData.renderPostProcessing = true;
            camData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
            camData.antialiasingQuality = AntialiasingQuality.High;
            camera.allowHDR = true;

            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            ValidateArt();
            Debug.Log("KOS_ART Applied DreadOut-style visual upgrade and expanded 6.0x4.5m room layout.");
        }

        private static void UpgradeRoomGreyboxDimensions()
        {
            SetTransform("Floor_4x3m", new Vector3(0f, -0.05f, 0f), new Vector3(6.0f, 0.1f, 4.5f));
            SetTransform("Ceiling_4x3m", new Vector3(0f, 3.15f, 0f), new Vector3(6.0f, 0.1f, 4.5f));
            SetTransform("Wall_North", new Vector3(0f, 1.55f, 2.30f), new Vector3(6.0f, 3.1f, 0.1f));
            SetTransform("Wall_South", new Vector3(0f, 1.55f, -2.30f), new Vector3(6.0f, 3.1f, 0.1f));
            SetTransform("Wall_East", new Vector3(3.05f, 1.55f, 0f), new Vector3(0.1f, 3.1f, 4.5f));
            SetTransform("Wall_West", new Vector3(-3.05f, 1.55f, 0f), new Vector3(0.1f, 3.1f, 4.5f));

            SetTransform("MejaKerjaPC", new Vector3(-1.25f, 0.72f, 1.90f), new Vector3(1.3f, 0.06f, 0.65f));
            SetTransform("KakiMejaKiri", new Vector3(-1.85f, 0.35f, 1.90f), new Vector3(0.08f, 0.7f, 0.6f));
            SetTransform("KakiMejaKanan", new Vector3(-0.65f, 0.35f, 1.90f), new Vector3(0.08f, 0.7f, 0.6f));
            SetTransform("KasurKos", new Vector3(-2.35f, 0.22f, -0.95f), new Vector3(1.1f, 0.44f, 1.8f));
            SetTransform("LemariPakaian", new Vector3(2.55f, 1.0f, 1.55f), new Vector3(0.75f, 2.0f, 0.9f));

            var d = Object.FindFirstObjectByType<KamarKosPrologDirector>();
            if (d != null)
            {
                if (d.pcMonitorItem) d.pcMonitorItem.transform.position = new Vector3(-1.25f, 1.02f, 2.00f);
                if (d.stickyNoteItem) d.stickyNoteItem.transform.position = new Vector3(-0.87f, 0.77f, 1.80f);
                if (d.lockedDrawerItem) d.lockedDrawerItem.transform.position = new Vector3(-0.90f, 0.56f, 1.57f);
                if (d.wallCalendarItem) d.wallCalendarItem.transform.position = new Vector3(0.95f, 1.45f, 2.23f);
                if (d.suratKosItem) d.suratKosItem.transform.position = new Vector3(1.25f, 0.02f, -2.07f);
                if (d.smartphoneBedItem) d.smartphoneBedItem.transform.position = new Vector3(-2.15f, 0.46f, -0.85f);
                if (d.kosDoorItem) d.kosDoorItem.transform.position = new Vector3(1.25f, 1.05f, -2.23f);
                if (d.jacketItem) d.jacketItem.transform.position = new Vector3(1.55f, 1.35f, -2.17f);
            }
        }

        private static void SetTransform(string objectName, Vector3 position, Vector3 scale)
        {
            var go = GameObject.Find(objectName);
            if (!go) return;
            go.transform.position = position;
            go.transform.localScale = scale;
        }

        private static void ConfigureImports()
        {
            foreach (string path in Directory.GetFiles(Art, "*.fbx", SearchOption.AllDirectories))
            {
                var importer = (ModelImporter)AssetImporter.GetAtPath(path);
                importer.importCameras = false;
                importer.importLights = false;
                importer.importAnimation = false;
                importer.addCollider = false;
                importer.materialImportMode = ModelImporterMaterialImportMode.None;
                importer.meshCompression = ModelImporterMeshCompression.Low;
                importer.isReadable = false;
                importer.SaveAndReimport();
            }
            foreach (string path in Directory.GetFiles(Art, "*", SearchOption.AllDirectories).Where(p => p.EndsWith(".png") || p.EndsWith(".jpg")))
            {
                var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                importer.maxTextureSize = 2048;
                importer.mipmapEnabled = true;
                importer.anisoLevel = 8;
                importer.filterMode = FilterMode.Trilinear;
                importer.textureCompression = TextureImporterCompression.CompressedHQ;
                importer.sRGBTexture = !path.Contains("_mask") && !path.Contains("_nor_gl_");
                importer.textureType = path.Contains("_nor_gl_") ? TextureImporterType.NormalMap : TextureImporterType.Default;
                importer.wrapMode = path.Contains("Original/Textures") ? TextureWrapMode.Clamp : TextureWrapMode.Repeat;
                importer.SaveAndReimport();
            }
        }

        private static Material Plain(string name, Color color, float metallic = 0, float smoothness = .2f)
        {
            string path = Generated + "/Materials/" + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (!mat)
            {
                mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                AssetDatabase.CreateAsset(mat, path);
            }
            mat.SetColor("_BaseColor", color);
            mat.SetFloat("_Metallic", metallic);
            mat.SetFloat("_Smoothness", smoothness);
            mat.enableInstancing = true;
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static Material Pbr(string name, string asset, Color? tint = null, float smoothness = .68f, float bumpScale = 1.15f)
        {
            var mat = Plain(name, tint ?? Color.white, 0, smoothness);
            string folder = Art + "/Imported/PolyHaven/" + asset;
            string diffuse = Directory.GetFiles(folder, "*_diff_*jpg").Single();
            mat.SetTexture("_BaseMap", AssetDatabase.LoadAssetAtPath<Texture2D>(diffuse));
            string normal = Directory.GetFiles(folder, "*_nor_gl_*jpg").FirstOrDefault();
            if (normal != null)
            {
                mat.SetTexture("_BumpMap", AssetDatabase.LoadAssetAtPath<Texture2D>(normal));
                mat.SetFloat("_BumpScale", bumpScale);
                mat.EnableKeyword("_NORMALMAP");
            }
            var mask = AssetDatabase.LoadAssetAtPath<Texture2D>(folder + "/" + asset + "_mask.png");
            mat.SetTexture("_MetallicGlossMap", mask);
            mat.SetTexture("_OcclusionMap", mask);
            mat.SetFloat("_OcclusionStrength", 1.0f);
            mat.EnableKeyword("_METALLICSPECGLOSSMAP");
            mat.EnableKeyword("_OCCLUSIONMAP");
            EditorUtility.SetDirty(mat);
            return mat;
        }

        private static Material Print(string name, bool emissive = false, bool transparent = false)
        {
            var mat = Plain(name, Color.white, 0, .14f);
            var texture = AssetDatabase.LoadAssetAtPath<Texture2D>(Art + "/Original/Textures/" + name + ".png");
            mat.SetTexture("_BaseMap", texture);
            if (emissive)
            {
                mat.SetTexture("_EmissionMap", texture);
                mat.SetColor("_EmissionColor", new Color(0.75f, 0.92f, 1.0f) * 1.15f);
                mat.EnableKeyword("_EMISSION");
            }
            if (transparent)
            {
                mat.SetFloat("_Surface", 1);
                mat.SetFloat("_SrcBlend", (float)BlendMode.SrcAlpha);
                mat.SetFloat("_DstBlend", (float)BlendMode.OneMinusSrcAlpha);
                mat.SetFloat("_ZWrite", 0);
                mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
                mat.renderQueue = 3000;
            }
            return mat;
        }

        private static Bounds BoundsOf(GameObject go)
        {
            var renderers = go.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) throw new Exception("Imported model has no renderer: " + go.name);
            var bounds = renderers[0].bounds;
            foreach (var renderer in renderers.Skip(1)) bounds.Encapsulate(renderer.bounds);
            return bounds;
        }

        private static GameObject Model(string path, string name, Vector3 center, Vector3 size, float yaw, Material material, Transform parent = null)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (!prefab) throw new Exception("Missing imported model: " + path);
            var holder = new GameObject(name);
            holder.transform.SetParent(root, false);
            var model = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            model.transform.SetParent(holder.transform, false);
            model.transform.localRotation = Quaternion.Euler(0, yaw, 0) * model.transform.localRotation;
            foreach (var collider in model.GetComponentsInChildren<Collider>()) Object.DestroyImmediate(collider);
            foreach (var renderer in model.GetComponentsInChildren<Renderer>())
            {
                renderer.sharedMaterials = Enumerable.Repeat(material, renderer.sharedMaterials.Length).ToArray();
                if (renderer.name.Contains("Dark coffee")) renderer.sharedMaterial = Plain("CoffeeSurface", new Color(.025f, .012f, .006f), 0, .75f);
                renderer.shadowCastingMode = ShadowCastingMode.On;
                renderer.receiveShadows = true;
            }
            var bounds = BoundsOf(holder);
            holder.transform.localScale = new Vector3(size.x / bounds.size.x, size.y / bounds.size.y, size.z / bounds.size.z);
            holder.transform.position = center - BoundsOf(holder).center;
            if (parent) holder.transform.SetParent(parent, true);
            return holder;
        }

        private static GameObject Ph(string id, Vector3 center, Vector3 size, float yaw = 0, Transform parent = null) =>
            Model(Art + "/Imported/PolyHaven/" + id + "/" + id + ".fbx", id, center, size, yaw, Pbr(id, id), parent);
        private static GameObject Ken(string id, Vector3 center, Vector3 size, Material material, float yaw = 0, Transform parent = null) =>
            Model(Art + "/Imported/Kenney/" + id + ".fbx", id, center, size, yaw, material, parent);
        private static GameObject Own(string id, Vector3 center, Vector3 size, Material material, float yaw = 0, Transform parent = null) =>
            Model(Art + "/Original/Models/" + id + ".fbx", id, center, size, yaw, material, parent);

        private static GameObject Box(string name, Vector3 center, Vector3 size, Material material)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(root, false);
            go.transform.position = center;
            go.transform.localScale = size;
            Object.DestroyImmediate(go.GetComponent<Collider>());
            go.GetComponent<Renderer>().sharedMaterial = material;
            return go;
        }

        private static GameObject Quad(string name, Vector3 center, Vector2 size, Vector3 rotation, Material material, Vector2? tiles = null, Transform parent = null)
        {
            string path = Generated + "/Meshes/" + name + ".asset";
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (!mesh)
            {
                mesh = new Mesh { name = name };
                AssetDatabase.CreateAsset(mesh, path);
            }
            Vector2 uv = tiles ?? Vector2.one;
            mesh.Clear();
            mesh.vertices = new[] { new Vector3(-.5f, -.5f, 0), new Vector3(.5f, -.5f, 0), new Vector3(-.5f, .5f, 0), new Vector3(.5f, .5f, 0) };
            mesh.triangles = new[] { 0, 2, 1, 2, 3, 1 };
            mesh.uv = new[] { Vector2.zero, new Vector2(uv.x, 0), new Vector2(0, uv.y), uv };
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            EditorUtility.SetDirty(mesh);
            var go = new GameObject(name, typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(root, false);
            go.transform.SetPositionAndRotation(center, Quaternion.Euler(rotation));
            go.transform.localScale = new Vector3(size.x, size.y, 1);
            go.GetComponent<MeshFilter>().sharedMesh = mesh;
            var renderer = go.GetComponent<Renderer>();
            renderer.sharedMaterial = material;
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            if (parent) go.transform.SetParent(parent, true);
            return go;
        }

        private static void Architecture()
        {
            // Lantai keramik 6.0m x 4.5m dengan pantulan lembap halus & Plafon 3.1m
            var tileMat = Pbr("CeramicTiles", "floor_tiles_04", new Color(.64f, .66f, .58f), smoothness: .72f, bumpScale: 1.2f);
            Quad("CeramicTileFloor", new Vector3(0, .006f, 0), new Vector2(6.0f, 4.5f), new Vector3(90, 0, 0), tileMat, new Vector2(4.5f, 3.375f));
            Quad("LimewashedCeiling", new Vector3(0, 3.098f, 0), new Vector2(6.0f, 4.5f), new Vector3(-90, 0, 0), cream, new Vector2(3.0f, 2.25f));

            // Dinding Utara & Barat (Bentang 6.0m dan 4.5m)
            Wall("North", new Vector3(0, 0, 2.248f), 6.0f, 0);
            Wall("West", new Vector3(-2.998f, 0, 0), 4.5f, 270);

            // Dinding Timur dengan ceruk jendela malam
            Quad("EastDado", new Vector3(2.998f, .55f, 0), new Vector2(4.5f, 1.1f), new Vector3(0, 90, 0), lowerWall, new Vector2(2.25f, .55f));
            Quad("EastAboveWindow", new Vector3(2.998f, 2.65f, 0), new Vector2(4.5f, .9f), new Vector3(0, 90, 0), cream, new Vector2(2.25f, .45f));
            Quad("EastNorthOfWindow", new Vector3(2.998f, 1.65f, 1.125f), new Vector2(2.25f, 1.1f), new Vector3(0, 90, 0), cream);
            Quad("EastSouthOfWindow", new Vector3(2.998f, 1.65f, -1.675f), new Vector2(1.15f, 1.1f), new Vector3(0, 90, 0), cream);

            // Dinding Selatan dengan bukaan pintu di x = 1.25f
            Wall("SouthLeft", new Vector3(-.605f, 0, -2.248f), 3.59f, 180);
            Wall("SouthRight", new Vector3(2.355f, 0, -2.248f), 1.29f, 180);
            Quad("DoorLintelPlaster", new Vector3(1.25f, 2.61f, -2.248f), new Vector2(.92f, .98f), new Vector3(0, 180, 0), cream);

            // Skirting (Lis Kaki Dinding) & Balok Kayu Plafon Kos Tua
            var skirting = Pbr("WornSkirting", "wood_table_worn", new Color(.32f, .29f, .22f), smoothness: .3f);
            Box("NorthSkirting", new Vector3(0, .055f, 2.22f), new Vector3(6.0f, .11f, .04f), skirting);
            Box("WestSkirting", new Vector3(-2.97f, .055f, 0), new Vector3(.04f, .11f, 4.5f), skirting);
            Box("EastSkirting", new Vector3(2.97f, .055f, 0), new Vector3(.04f, .11f, 4.5f), skirting);
            Box("SouthSkirtingLeft", new Vector3(-.605f, .055f, -2.22f), new Vector3(3.59f, .11f, .04f), skirting);
            Box("SouthSkirtingRight", new Vector3(2.355f, .055f, -2.22f), new Vector3(1.29f, .11f, .04f), skirting);

            // Balok kayu melintang di plafon (khas bangunan kos tua Indonesia)
            for (int i = 0; i < 6; i++)
                Box("CeilingTimber_" + i, new Vector3(-2.5f + i * 1.0f, 3.065f, 0), new Vector3(.06f, .07f, 4.5f), skirting);
            Box("CeilingCrossBeam_North", new Vector3(0, 3.065f, 1.1f), new Vector3(6.0f, .06f, .06f), skirting);
            Box("CeilingCrossBeam_South", new Vector3(0, 3.065f, -1.1f), new Vector3(6.0f, .06f, .06f), skirting);

            // Bercak Lembap & Jamur Dinding (DampStain) di berbagai sudut ruangan
            var grime = Print("DampStain", false, true);
            Quad("NorthDampPatch", new Vector3(-1.1f, 1.65f, 2.237f), new Vector2(3.2f, 2.75f), Vector3.zero, grime);
            Quad("NorthEastDampPatch", new Vector3(1.8f, 2.15f, 2.236f), new Vector2(1.9f, 1.65f), Vector3.zero, grime);
            Quad("WestDampPatch", new Vector3(-2.985f, 1.55f, -.45f), new Vector2(3.1f, 2.6f), new Vector3(0, 270, 0), grime);
            Quad("SouthDampPatch", new Vector3(-1.4f, 1.45f, -2.237f), new Vector2(2.4f, 2.2f), new Vector3(0, 180, 0), grime);

            // Jendela Timur & Teralis Besi (Penghasil Bayangan Dramatis)
            Ken("wallWindow", new Vector3(3.01f, 1.65f, -.55f), new Vector3(.12f, 1.12f, 1.10f), wood, 90);
            var night = Plain("NightWindowGlass", new Color(.015f, .045f, .065f), .35f, .78f);
            night.SetColor("_EmissionColor", new Color(.04f, .11f, .18f));
            night.EnableKeyword("_EMISSION");
            Quad("DarkNightBeyondWindow", new Vector3(3.08f, 1.65f, -.55f), new Vector2(1.1f, 1.1f), new Vector3(0, 90, 0), night);
            for (int i = 0; i < 5; i++)
            {
                var bar = Box("WindowSecurityBar", new Vector3(2.915f, 1.65f, -.98f + i * .215f), new Vector3(.02f, .96f, .018f), metal);
                bar.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.On;
            }
            Box("WindowSill", new Vector3(2.93f, 1.09f, -.55f), new Vector3(.24f, .035f, 1.2f), wood);

            // Poster, Tata Tertib Kos & Keset Pintu
            Quad("ChannelPoster", new Vector3(-2.985f, 1.86f, -1.15f), new Vector2(.48f, .66f), new Vector3(0, 270, 0), Print("ChannelPoster"));
            Quad("BoardingHouseRules", new Vector3(.38f, 1.6f, -2.237f), new Vector2(.30f, .38f), new Vector3(0, 180, 0), Print("KosRules"));
            Quad("WovenDoorMat", new Vector3(1.25f, .009f, -1.72f), new Vector2(.75f, .42f), new Vector3(90, 0, 0), Print("WovenMat"));
        }

        private static void Wall(string name, Vector3 pos, float width, float yaw)
        {
            Quad(name + "Dado", pos + Vector3.up * .52f, new Vector2(width, 1.04f), new Vector3(0, yaw, 0), lowerWall, new Vector2(width * .5f, .52f));
            Quad(name + "Limewash", pos + Vector3.up * 2.07f, new Vector2(width, 2.06f), new Vector3(0, yaw, 0), cream, new Vector2(width * .5f, 1.03f));
        }

        private static void Furniture()
        {
            // Kasur di sudut Barat Daya
            Ph("vintage_day_bed", new Vector3(-2.35f, .45f, -.95f), new Vector3(1.08f, .90f, 1.8f), 90);

            // Meja Kerja PC & Kursi di dinding Utara
            Ph("wooden_table_02", new Vector3(-1.25f, .375f, 1.90f), new Vector3(1.30f, .75f, .65f));
            Ph("painted_wooden_chair_01", new Vector3(-1.58f, .43f, 1.45f), new Vector3(.40f, .86f, .46f), 180);
            var seat = new GameObject("ChairSeatCollision", typeof(BoxCollider));
            seat.transform.SetParent(root, false);
            seat.transform.position = new Vector3(-1.58f, .25f, 1.56f);
            seat.GetComponent<BoxCollider>().size = new Vector3(.38f, .5f, .38f);

            // Lemari Pakaian & Kabinet di sudut Timur Laut
            Ken("bookcaseClosedDoors", new Vector3(2.55f, .975f, 1.55f), new Vector3(.72f, 1.95f, .82f), wood);
            Ph("painted_wooden_cabinet_02", new Vector3(1.65f, .60f, 2.07f), new Vector3(.45f, 1.20f, .30f));
            Solid("CupboardCollision", new Vector3(1.65f, .60f, 2.07f), new Vector3(.45f, 1.20f, .30f));

            // Rak Gantung Dinding Barat & Perangkat PC
            Ph("wooden_bookshelf_worn", new Vector3(-2.88f, 1.94f, .95f), new Vector3(.21f, .64f, .68f), 90);
            Ph("desk_lamp_arm_01", new Vector3(-1.75f, .99f, 1.98f), new Vector3(.28f, .48f, .26f), 30);
            Own("OldPCTower", new Vector3(-1.49f, .215f, 2.04f), new Vector3(.19f, .42f, .38f), plastic, 180);
            Ken("computerKeyboard", new Vector3(-1.39f, .770f, 1.74f), new Vector3(.40f, .025f, .14f), plastic);
            Ken("computerMouse", new Vector3(-.92f, .775f, 1.73f), new Vector3(.055f, .035f, .095f), plastic);
            Ken("speakerSmall", new Vector3(-1.61f, .82f, 2.06f), new Vector3(.09f, .14f, .085f), plastic);
            Ken("speakerSmall", new Vector3(-.80f, .82f, 2.06f), new Vector3(.09f, .14f, .085f), plastic);
        }

        private static void Interactables()
        {
            var d = Object.FindFirstObjectByType<KamarKosPrologDirector>();
            Ken("computerScreen", new Vector3(-1.25f, .99f, 1.97f), new Vector3(.57f, .45f, .15f), plastic, 0, d.pcMonitorItem.transform);
            Quad("MonitorDashboard", new Vector3(-1.25f, 1.05f, 1.887f), new Vector2(.49f, .279f), Vector3.zero, Print("MonitorDashboard", true), null, d.pcMonitorItem.transform);
            Quad("CalendarAugust", new Vector3(.95f, 1.45f, 2.208f), new Vector2(.38f, .52f), Vector3.zero, Print("CalendarAugust"), null, d.wallCalendarItem.transform);
            Quad("StickyNote", new Vector3(-.87f, .783f, 1.80f), new Vector2(.14f, .14f), new Vector3(90, 0, -8), Print("StickyNote"), null, d.stickyNoteItem.transform);
            Quad("RentWarning", new Vector3(1.25f, .033f, -2.07f), new Vector2(.28f, .22f), new Vector3(90, 0, -5), Print("RentWarning"), null, d.suratKosItem.transform);
            Ken("cabinetBedDrawer", new Vector3(-.90f, .56f, 1.69f), new Vector3(.42f, .22f, .28f), wood, 0, d.lockedDrawerItem.transform);
            Own("CombinationPadlock", new Vector3(-.90f, .55f, 1.528f), new Vector3(.05f, .085f, .025f), brass, 0, d.lockedDrawerItem.transform);
            Ken("doorwayFront", new Vector3(1.25f, 1.06f, -2.23f), new Vector3(.98f, 2.12f, .10f), wood, 180, d.kosDoorItem.transform);
            Own("HangingJacket", new Vector3(1.55f, 1.35f, -2.155f), new Vector3(.42f, .65f, .105f), fabric, 180, d.jacketItem.transform);
            Own("Smartphone", new Vector3(-2.15f, .465f, -.85f), new Vector3(.10f, .018f, .18f), plastic, 0, d.smartphoneBedItem.transform);
            var phone = Object.FindFirstObjectByType<SmartphoneController>();
            phone.handPhoneMeshRoot.GetComponent<Renderer>().enabled = false;
            Own("SmartphoneUpright", phone.handPhoneMeshRoot.transform.position, new Vector3(.08f, .15f, .02f), plastic, 0, phone.handPhoneMeshRoot.transform);
        }

        private static void Clutter()
        {
            Ph("binder_notebook", new Vector3(-1.60f, .782f, 1.81f), new Vector3(.27f, .028f, .16f), -12);
            Own("EnamelMug", new Vector3(-.67f, .80f, 2.04f), new Vector3(.11f, .10f, .085f), Plain("StainedEnamel", new Color(.63f, .63f, .48f), .1f, .45f));
            Own("RefillWaterGallon", new Vector3(2.62f, .27f, -1.25f), new Vector3(.29f, .54f, .29f), Plain("BlueRefillPlastic", new Color(.065f, .25f, .34f), .1f, .55f));
            Solid("WaterGallonCollision", new Vector3(2.62f, .27f, -1.25f), new Vector3(.27f, .54f, .27f));
            Quad("RefillWaterLabel", new Vector3(2.62f, .24f, -1.405f), new Vector2(.18f, .09f), Vector3.zero, Print("WaterLabel"));
            Ph("plastic_bottle_gallon", new Vector3(2.72f, .155f, -1.78f), new Vector3(.16f, .31f, .13f), -25);
            Ken("trashcan", new Vector3(-.42f, .15f, 2.02f), new Vector3(.22f, .3f, .22f), metal);
            Ken("cardboardBoxOpen", new Vector3(2.45f, .13f, .35f), new Vector3(.45f, .26f, .3f), Plain("Cardboard", new Color(.34f, .26f, .15f)));
            Solid("CardboardBoxCollision", new Vector3(2.45f, .13f, .35f), new Vector3(.43f, .26f, .28f));

            var book = Plain("FadedBookCovers", new Color(.21f, .24f, .20f));
            for (int i = 0; i < 4; i++)
                Ken("books", new Vector3(1.65f, .48f + i * .18f, 2.04f), new Vector3(.29f, .13f, .16f), i % 2 == 0 ? book : paper);
            Ken("books", new Vector3(-2.81f, 1.85f, .95f), new Vector3(.12f, .20f, .44f), book, 90);

            for (int i = 0; i < 3; i++)
                Quad("DiscardedPaper" + i, new Vector3(.35f + i * .35f, .011f, 1.25f - i * .28f), new Vector2(.19f, .25f), new Vector3(90, 0, i * 32), i == 1 ? Print("NoodlePacket") : Print("KosRules"));

            var shade = Print("ContactShade", false, true);
            Quad("BedContactShade", new Vector3(-2.35f, .008f, -.95f), new Vector2(1.25f, 2.0f), new Vector3(90, 0, 0), shade);
            Quad("CupboardContactShade", new Vector3(2.55f, .008f, 1.55f), new Vector2(.95f, 1.05f), new Vector3(90, 0, 0), shade);

            Box("OldWallSocket", new Vector3(-2.25f, .42f, 2.22f), new Vector3(.105f, .12f, .025f), Plain("AgedSwitchPlastic", new Color(.64f, .59f, .44f)));
            for (int i = 0; i < 2; i++) Box("SocketPinHole", new Vector3(-2.266f + i * .033f, .42f, 2.202f), new Vector3(.009f, .012f, .005f), plastic);
            Cable("ComputerPowerCable", new[] { new Vector3(-1.49f, .1f, 2.21f), new Vector3(-1.75f, .025f, 2.21f), new Vector3(-2.18f, .025f, 2.21f), new Vector3(-2.25f, .12f, 2.21f), new Vector3(-2.25f, .42f, 2.20f) }, .007f);
            Cable("CeilingFlex", new[] { new Vector3(-.15f, 3.10f, -.05f), new Vector3(-.15f, 2.82f, -.05f) }, .007f);

            var bulb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bulb.name = "BareWarmBulb";
            bulb.transform.SetParent(root, false);
            bulb.transform.position = new Vector3(-.15f, 2.79f, -.05f);
            bulb.transform.localScale = new Vector3(.07f, .10f, .07f);
            Object.DestroyImmediate(bulb.GetComponent<Collider>());
            var glow = Plain("WarmBulbGlass", new Color(.95f, .76f, .45f));
            glow.SetColor("_EmissionColor", new Color(2.4f, 1.35f, .42f));
            glow.EnableKeyword("_EMISSION");
            bulb.GetComponent<Renderer>().sharedMaterial = glow;
        }

        private static void Solid(string name, Vector3 center, Vector3 size)
        {
            var go = new GameObject(name, typeof(BoxCollider));
            go.transform.SetParent(root, false);
            go.transform.position = center;
            go.GetComponent<BoxCollider>().size = size;
        }

        private static void Cable(string name, Vector3[] points, float width)
        {
            var go = new GameObject(name, typeof(LineRenderer));
            go.transform.SetParent(root, false);
            var line = go.GetComponent<LineRenderer>();
            line.positionCount = points.Length;
            line.SetPositions(points);
            line.widthMultiplier = width;
            line.numCornerVertices = 3;
            line.numCapVertices = 3;
            line.sharedMaterial = plastic;
            line.generateLightingData = true;
        }

        private static Light Lamp(string name, Vector3 position, Vector3 target, Color color, float intensity, float range, float cone, bool shadows = false)
        {
            var go = new GameObject(name, typeof(Light));
            go.transform.SetParent(root, false);
            go.transform.position = position;
            go.transform.LookAt(target);
            var light = go.GetComponent<Light>();
            light.type = LightType.Spot;
            light.color = color;
            light.intensity = intensity;
            light.range = range;
            light.spotAngle = cone;
            light.innerSpotAngle = cone * .62f;
            light.shadows = shadows ? LightShadows.Soft : LightShadows.None;
            light.shadowResolution = LightShadowResolution.High;
            light.shadowBias = .018f;
            light.shadowNormalBias = .12f;
            return light;
        }

        private static void Lighting()
        {
            GameObject.Find("RoomCeilingLight").GetComponent<Light>().enabled = false;
            var monitor = GameObject.Find("PCMonitorGlow").GetComponent<Light>();
            monitor.transform.position = new Vector3(-1.25f, 1.05f, 1.78f);
            monitor.intensity = .48f;
            monitor.range = 2.65f;
            monitor.color = new Color(.28f, .72f, .78f);
            monitor.shadows = LightShadows.None;

            // 1. Bohlam Plafon Remang (Menciptakan Pool of Light di Tengah & Sudut Kamar Gelap)
            Lamp("WeakTungstenCeiling", new Vector3(-.15f, 2.78f, -.05f), new Vector3(-.15f, 0, -.05f), new Color(1f, .68f, .34f), 3.2f, 7.2f, 138, true);

            // 2. Cahaya Bulan Dingin Khas DreadOut Menembus Teralis Jendela (Dengan Bayangan Teralis)
            Lamp("ColdWindowSpill", new Vector3(3.15f, 1.95f, -.55f), new Vector3(-1.8f, .25f, -.25f), new Color(.22f, .48f, .68f), 2.85f, 8.2f, 96, true);

            // 3. Lampu Meja Fokus pada Area Kerja & Gembok Laci
            Lamp("DeskLampPool", new Vector3(-1.68f, 1.16f, 1.98f), new Vector3(-1.10f, .73f, 1.75f), new Color(1f, .66f, .32f), 1.25f, 2.6f, 92, false);

            // 4. Fill Bounce Lembut & Aksen Dingin Sudut Bawah
            var fillObject = new GameObject("WarmWallBounce", typeof(Light));
            fillObject.transform.SetParent(root, false);
            fillObject.transform.position = new Vector3(-.15f, 2.25f, -.05f);
            var fill = fillObject.GetComponent<Light>();
            fill.type = LightType.Point;
            fill.color = new Color(.55f, .52f, .42f);
            fill.range = 6.5f;
            fill.intensity = .38f;
            fill.shadows = LightShadows.None;

            // 5. Reflection Probe Agar Pantulan Lantai Keramik, Kaca, dan Besi Tampak Nyata
            var probeGo = new GameObject("KosReflectionProbe", typeof(ReflectionProbe));
            probeGo.transform.SetParent(root, false);
            probeGo.transform.position = new Vector3(0f, 1.55f, 0f);
            var probe = probeGo.GetComponent<ReflectionProbe>();
            probe.mode = ReflectionProbeMode.Realtime;
            probe.refreshMode = ReflectionProbeRefreshMode.OnAwake;
            probe.timeSlicingMode = ReflectionProbeTimeSlicingMode.AllFacesAtOnce;
            probe.size = new Vector3(6.2f, 3.3f, 4.7f);
            probe.intensity = 0.85f;
            probe.boxProjection = true;

            // Ambient & Kabut Atmosferik Horor
            RenderSettings.skybox = null;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.11f, .15f, .18f);
            RenderSettings.ambientEquatorColor = new Color(.08f, .10f, .11f);
            RenderSettings.ambientGroundColor = new Color(.035f, .04f, .045f);
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogColor = new Color(.025f, .042f, .052f);
            RenderSettings.fogDensity = .042f;

            // Post-Processing Setara DreadOut (ACES + Split Toning + Film Grain + Chromatic Aberration + Vignette)
            string path = Generated + "/KosAtmosphere.asset";
            var profile = AssetDatabase.LoadAssetAtPath<VolumeProfile>(path);
            if (!profile)
            {
                profile = ScriptableObject.CreateInstance<VolumeProfile>();
                AssetDatabase.CreateAsset(profile, path);
            }
            foreach (var component in profile.components.ToArray())
            {
                profile.components.Remove(component);
                Object.DestroyImmediate(component, true);
            }

            var tone = profile.Add<Tonemapping>(true);
            tone.mode.Override(TonemappingMode.ACES);

            var color = profile.Add<ColorAdjustments>(true);
            color.postExposure.Override(.45f);
            color.contrast.Override(18f);
            color.saturation.Override(-18f);
            color.colorFilter.Override(new Color(.94f, .97f, 1.0f));

            var split = profile.Add<SplitToning>(true);
            split.shadows.Override(new Color(.16f, .27f, .34f));
            split.highlights.Override(new Color(.64f, .54f, .38f));
            split.balance.Override(-18f);

            var smh = profile.Add<ShadowsMidtonesHighlights>(true);
            smh.shadows.Override(new Vector4(.88f, .93f, .98f, -.04f));
            smh.midtones.Override(new Vector4(.96f, .98f, 1.0f, 0f));
            smh.highlights.Override(new Vector4(1.0f, .96f, .88f, .05f));

            var bloom = profile.Add<Bloom>(true);
            bloom.intensity.Override(.32f);
            bloom.threshold.Override(.92f);
            bloom.scatter.Override(.62f);

            var vignette = profile.Add<Vignette>(true);
            vignette.color.Override(new Color(.01f, .02f, .03f));
            vignette.intensity.Override(.36f);
            vignette.smoothness.Override(.48f);

            var grain = profile.Add<FilmGrain>(true);
            grain.type.Override(FilmGrainLookup.Medium1);
            grain.intensity.Override(.32f);
            grain.response.Override(.72f);

            var ca = profile.Add<ChromaticAberration>(true);
            ca.intensity.Override(.075f);

            foreach (var c in profile.components)
                if (!AssetDatabase.Contains(c)) AssetDatabase.AddObjectToAsset(c, profile);
            EditorUtility.SetDirty(profile);

            var obj = new GameObject("KosGlobalAtmosphere", typeof(Volume));
            obj.transform.SetParent(root, false);
            var volume = obj.GetComponent<Volume>();
            volume.isGlobal = true;
            volume.priority = 10;
            volume.sharedProfile = profile;
        }

        private static void ConfigurePipeline()
        {
            string rendererPath = Generated + "/KosRenderer.asset", pipelinePath = Generated + "/KosPipeline.asset";
            if (!File.Exists(rendererPath)) AssetDatabase.CopyAsset("Assets/Settings/PC_Renderer.asset", rendererPath);
            if (!File.Exists(pipelinePath)) AssetDatabase.CopyAsset("Assets/Settings/PC_RPAsset.asset", pipelinePath);
            var renderer = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(rendererPath);
            var so = new SerializedObject(renderer);
            so.FindProperty("m_RenderingMode").intValue = 0;
            so.ApplyModifiedPropertiesWithoutUndo();
            foreach (var feature in renderer.rendererFeatures)
            {
                var settings = new SerializedObject(feature);
                var downsample = settings.FindProperty("m_Settings.Downsample");
                if (downsample != null) downsample.boolValue = false;
                var intensity = settings.FindProperty("m_Settings.Intensity");
                if (intensity != null) intensity.floatValue = 1.15f;
                var radius = settings.FindProperty("m_Settings.Radius");
                if (radius != null) radius.floatValue = .28f;
                settings.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(feature);
            }
            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(pipelinePath);
            var p = new SerializedObject(pipeline);
            p.FindProperty("m_RendererDataList").GetArrayElementAtIndex(0).objectReferenceValue = renderer;
            p.FindProperty("m_ShadowDistance").floatValue = 16;
            p.FindProperty("m_AdditionalLightsShadowmapResolution").intValue = 2048;
            p.FindProperty("m_MainLightShadowmapResolution").intValue = 2048;
            p.FindProperty("m_SoftShadowsSupported").boolValue = true;
            p.FindProperty("m_RequireOpaqueTexture").boolValue = false;
            p.FindProperty("m_MSAA").intValue = 2;
            p.ApplyModifiedPropertiesWithoutUndo();
            QualitySettings.renderPipeline = pipeline;
            GraphicsSettings.defaultRenderPipeline = pipeline;
            EditorUtility.SetDirty(pipeline);
            EditorUtility.SetDirty(renderer);
        }

        public static void ValidateArt()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            if (scene.path != PrologKosSceneBuilder.ScenePath) EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            var art = GameObject.Find("KamarKos_4x3m");
            if (!GameObject.Find("Kos_Environment_Art")) throw new Exception("Environment art is absent.");
            int triangles = 0;
            foreach (var filter in art.GetComponentsInChildren<MeshFilter>())
            {
                if (!filter.GetComponent<Renderer>().enabled) continue;
                if (!filter.sharedMesh) throw new Exception("Missing mesh: " + filter.name);
                for (int i = 0; i < filter.sharedMesh.subMeshCount; i++) triangles += (int)filter.sharedMesh.GetIndexCount(i) / 3;
            }
            foreach (var renderer in art.GetComponentsInChildren<Renderer>())
                foreach (var mat in renderer.enabled ? renderer.sharedMaterials : new Material[0])
                    if (!mat || !mat.shader || mat.shader.name == "Hidden/InternalErrorShader") throw new Exception("Invalid material: " + renderer.name);
            foreach (var item in Object.FindObjectsByType<KosInteractableItem>(FindObjectsSortMode.None))
                if (!item.GetComponent<Collider>() || !item.director) throw new Exception("Broken interaction: " + item.itemId);
            var report = new VisualAudit
            {
                triangles = triangles,
                renderers = art.GetComponentsInChildren<Renderer>().Count(r => r.enabled),
                lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None).Count(l => l.enabled),
                interactables = Object.FindObjectsByType<KosInteractableItem>(FindObjectsSortMode.None).Length,
                maxTextureSize = 2048
            };
            File.WriteAllText("Audit/visual-environment-validation.json", JsonUtility.ToJson(report, true) + "\n");
            if (triangles > 160000) throw new Exception("Environment exceeds integrated GPU triangle budget: " + triangles);
            Debug.Log("KOS_ART Validation triangles=" + triangles + " renderers=" + report.renderers + " interactables=" + report.interactables);
        }

        [Serializable]
        private class VisualAudit
        {
            public int triangles, renderers, lights, interactables, maxTextureSize;
        }
    }
}
#endif

