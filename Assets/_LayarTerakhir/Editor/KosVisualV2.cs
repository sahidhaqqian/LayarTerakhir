#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Incremental corrections to the furnished scene. Never rebuilds the greybox or removes V1 models.
    public static class KosVisualV2
    {
        private const string Art = "Assets/_LayarTerakhir/Art";
        private const string Mats = Art + "/Generated/Materials/";
        private static Transform details;
        public static Bounds BoundsOf(GameObject go)
        {
            var rs = go.GetComponentsInChildren<Renderer>().Where(r => r.enabled && !(r is LineRenderer)).ToArray();
            if (rs.Length == 0) throw new InvalidOperationException("No visible geometry: " + go.name);
            var b = rs[0].bounds;
            foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds);
            return b;
        }
        private static GameObject Find(string name)
        {
            // FBX roots often share a name with their scene holder. Always choose the explicit art holder.
            var art=GameObject.Find("Kos_Environment_Art");
            var holder=art ? art.transform.Find(name) ?? art.transform.Find("Kos_V2_Details/"+name) : null;
            return holder ? holder.gameObject : GameObject.Find(name) ?? throw new Exception("Missing scene object: " + name);
        }
        private static void ResetPrefabRoot(GameObject holder,float yaw)
        {
            var model=holder.transform.GetChild(0);
            var source=PrefabUtility.GetCorrespondingObjectFromSource(model);
            if(!source)throw new Exception("Expected FBX instance root: "+holder.name);
            model.localPosition=source.localPosition;model.localScale=source.localScale;
            model.localRotation=Quaternion.Euler(0,yaw,0)*source.localRotation;
            holder.transform.rotation=Quaternion.identity;
        }
        private static Material Mat(string name) => AssetDatabase.LoadAssetAtPath<Material>(Mats + name + ".mat");

        [MenuItem("LayarTerakhir/Environment/Apply furnished Kos V2 corrections")]
        public static void Apply()
        {
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            // Only this pass's own additions are regenerated on repeat runs.
            var previous = GameObject.Find("Kos_V2_Details");
            if (previous) Object.DestroyImmediate(previous);
            details = new GameObject("Kos_V2_Details").transform;
            details.SetParent(Find("Kos_Environment_Art").transform, false);
            ConfigureNewImports();
            CorrectArchitecture();
            CorrectFurniture();
            DressRoom();
            LightingAndMaterials();
            foreach (var camera in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
            {
                if (!camera.CompareTag("MainCamera")) continue;
                var data = camera.GetUniversalAdditionalCameraData();
                data.renderPostProcessing = true;
                data.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
                camera.allowHDR = true;
            }
            Physics.SyncTransforms();
            EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            AssetDatabase.SaveAssets();
            KosSceneValidation.Validate();
            Debug.Log("KOS_V2 Applied incremental furnished-room corrections; 6 x 4.5m preserved.");
        }

        private static void ConfigureNewImports()
        {
            var paths = new[] { "Imported/PolyHaven/plastic_container", "Imported/PolyHaven/wicker_basket_02", "Imported/PolyHaven/fabric_pattern_07" };
            foreach (var relative in paths)
            foreach (var path in Directory.GetFiles(Art + "/" + relative))
            {
                if (AssetImporter.GetAtPath(path) is TextureImporter ti)
                {
                    ti.maxTextureSize = relative.Contains("fabric") ? 1024 : 512;
                    ti.textureType = path.Contains("_nor_gl_") ? TextureImporterType.NormalMap : TextureImporterType.Default;
                    ti.sRGBTexture = !path.Contains("_mask") && !path.Contains("_nor_gl_") && !path.Contains("_arm_");
                    ti.mipmapEnabled = true; ti.anisoLevel = 2;
                    ti.textureCompression = TextureImporterCompression.Compressed;
                    ti.SaveAndReimport();
                }
            }
            foreach (var path in Directory.GetFiles(Art, "*.fbx", SearchOption.AllDirectories)
                .Where(p => p.Contains("/Kos") || p.Contains("plastic_container/") || p.Contains("wicker_basket_02/")))
            {
                var mi = (ModelImporter)AssetImporter.GetAtPath(path);
                mi.importCameras = false; mi.importLights = false; mi.importAnimation = false;
                mi.addCollider = false; mi.materialImportMode = ModelImporterMaterialImportMode.None;
                mi.meshCompression = ModelImporterMeshCompression.Low; mi.SaveAndReimport();
            }
        }

        private static void CorrectArchitecture()
        {
            // V1's left wall ended at x=1.19, covering 43cm of the door and leaving a west-corner gap.
            foreach (var name in new[] { "SouthLeftDado", "SouthLeftLimewash", "SouthSkirtingLeft" })
            {
                var t = Find(name).transform; var p = t.position; p.x = -1.12f; t.position = p;
                var s = t.localScale; s.x = 3.76f; t.localScale = s;
            }
            foreach (var name in new[] { "SouthRightDado", "SouthRightLimewash", "SouthSkirtingRight" })
            {
                var t = Find(name).transform; var p = t.position; p.x = 2.37f; t.position = p;
                var s = t.localScale; s.x = 1.26f; t.localScale = s;
            }
            // Keep the recognizable existing window; set the glass just inside the closed structural wall.
            var glass = Find("DarkNightBeyondWindow").transform;
            glass.position = new Vector3(2.992f, 1.65f, -.55f);
            Resize(Find("wallWindow"),new Vector3(.12f,1.12f,1.10f));
            SetCenter(Find("wallWindow"), new Vector3(2.965f, 1.65f, -.55f));
            foreach(var r in Find("wallWindow").GetComponentsInChildren<Renderer>())
                r.sharedMaterials=Enumerable.Repeat(r.name.Contains("glass")?Mat("NightWindowGlass"):r.name.Contains("metal")?Mat("DarkOxidisedMetal"):Mat("DoorAndWardrobeWood"),r.sharedMaterials.Length).ToArray();
            var lintel=Find("DoorLintelPlaster").transform;
            lintel.localScale=new Vector3(.98f,lintel.localScale.y,lintel.localScale.z);
            var shelf = Find("wooden_bookshelf_worn");
            SetCenter(shelf, new Vector3(-2.84f, 1.94f, .95f));
            // A small physical gap prevents the wall and bookshelf backs occupying the same plane.
            var wood = Mat("DoorAndWardrobeWood");
            Rail("CurtainRod", new Vector3(2.80f, 2.29f, -1.24f), new Vector3(2.80f, 2.29f, .14f), .014f, Mat("DarkOxidisedMetal"));
            foreach (float z in new[] { -1.24f, .14f })
                Rail("CurtainRodBracket", new Vector3(2.80f, 2.29f, z), new Vector3(2.98f, 2.29f, z), .008f, wood);
            foreach (float y in new[] { .32f, 1.05f, 1.81f })
                DetailBox("DoorHinge", new Vector3(.78f, y, -2.16f), new Vector3(.045f, .075f, .018f), Mat("WornBrass"));
            Rail("DoorHandleStem", new Vector3(1.63f, .99f, -2.17f), new Vector3(1.63f, .99f, -2.12f), .012f, Mat("WornBrass"));
            Rail("DoorHandleLever", new Vector3(1.63f, .99f, -2.12f), new Vector3(1.54f, .99f, -2.12f), .011f, Mat("WornBrass"));
        }

        private static void CorrectFurniture()
        {
            var bed = Find("vintage_day_bed"); Ground(bed);
            var desk = Find("wooden_table_02");
            SetCenter(desk, new Vector3(-1.25f, .381f, 1.87f)); Ground(desk);
            var chair = Find("painted_wooden_chair_01");
            ResetPrefabRoot(chair,180);
            Resize(chair,new Vector3(.40f,.86f,.46f));
            SetCenter(chair, new Vector3(-1.30f, .436f, 1.13f));
            // Derive which side contains the backrest from the actual high vertices.
            var high = MeshVertices(chair).Where(v => v.y > BoundsOf(chair).min.y + .65f).ToArray();
            if (high.Length > 0 && high.Average(v => v.z) > BoundsOf(chair).center.z)
                chair.transform.RotateAround(BoundsOf(chair).center, Vector3.up, 180);
            chair.transform.RotateAround(BoundsOf(chair).center, Vector3.up, -7);
            Ground(chair);
            var chairB = BoundsOf(chair);
            FitBox(Find("ChairSeatCollision").GetComponent<BoxCollider>(), new Bounds(new Vector3(chairB.center.x, .24f, chairB.center.z), new Vector3(chairB.size.x, .47f, chairB.size.z)));

            var wardrobe = Find("bookcaseClosedDoors");
            ResetPrefabRoot(wardrobe,0);
            wardrobe.transform.rotation = Quaternion.Euler(0, 90, 0);
            Resize(wardrobe, new Vector3(.56f, 1.95f, .96f));
            SetCenter(wardrobe, new Vector3(2.62f, .981f, 1.48f)); Ground(wardrobe);
            FitBox(Find("LemariPakaian").GetComponent<BoxCollider>(), BoundsOf(wardrobe));
            var cupboard = Find("painted_wooden_cabinet_02"); Ground(cupboard);
            FitBox(Find("CupboardCollision").GetComponent<BoxCollider>(), BoundsOf(cupboard));
            FitBox(Find("MejaKerjaPC").GetComponent<BoxCollider>(), new Bounds(new Vector3(-1.25f, .72f, 1.87f), new Vector3(1.30f, .072f, .65f)));
            foreach (var name in new[] { "KakiMejaKiri", "KakiMejaKanan" })
            {
                var c = Find(name).GetComponent<BoxCollider>();
                var b = c.bounds; b.center = new Vector3(b.center.x, b.center.y + .006f, 1.87f); FitBox(c, b);
            }
            // Fit the bed's solid base below the quilt so its phone remains raycast-accessible.
            var bedB = BoundsOf(bed);
            float mattressTop=SurfaceHeight(bed,-2.15f,-.85f)-.015f;
            FitBox(Find("KasurKos").GetComponent<BoxCollider>(), new Bounds(new Vector3(bedB.center.x, (FloorY+mattressTop)*.5f, bedB.center.z), new Vector3(bedB.size.x, mattressTop-FloorY, bedB.size.z)));

            var d = Object.FindFirstObjectByType<KamarKosPrologDirector>();
            var phone = d.smartphoneBedItem.transform.Find("Smartphone").gameObject;
            RestOn(phone, bed, -2.15f, -.85f);
            AlignItem(d.smartphoneBedItem, BoundsOf(phone));
            if (d.phoneBedRedLed) d.phoneBedRedLed.transform.position = BoundsOf(phone).center + Vector3.up * .018f;
            var jacket = d.jacketItem.transform.Find("HangingJacket").gameObject;
            SetCenter(jacket, new Vector3(2.16f, 1.38f, -2.13f));
            AlignItem(d.jacketItem, BoundsOf(jacket));
            Rail("JacketHook", new Vector3(2.16f, 1.72f, -2.23f), new Vector3(2.16f, 1.72f, -2.11f), .007f, Mat("DarkOxidisedMetal"));

            var drawer = d.lockedDrawerItem.transform.Find("cabinetBedDrawer").gameObject;
            SetCenter(drawer, new Vector3(-.90f, .60f, 1.69f));
            var lockGo = d.lockedDrawerItem.transform.Find("CombinationPadlock").gameObject;
            SetCenter(lockGo, new Vector3(-.90f, .59f, 1.528f));
            var drawerB = BoundsOf(drawer); drawerB.Encapsulate(BoundsOf(lockGo)); AlignItem(d.lockedDrawerItem, drawerB);
            var monitorB = BoundsOf(d.pcMonitorItem.transform.Find("computerScreen").gameObject);
            AlignItem(d.pcMonitorItem, monitorB);
            RestOn(Find("computerKeyboard"), desk, -1.32f, 1.70f);
            RestOn(Find("computerMouse"), desk, -.88f, 1.73f);
            RestOn(Find("binder_notebook"), desk, -1.68f, 1.70f);
            RestOn(Find("EnamelMug"), desk, -.70f, 2.04f);
            RestOn(Find("desk_lamp_arm_01"), desk, -1.74f, 2.00f);
            foreach (var go in Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t => t.name == "speakerSmall"))
                RestOn(go.gameObject, desk, go.position.x, go.position.z);
            var note = d.stickyNoteItem.transform.Find("StickyNote").gameObject;
            RestOn(note, desk, -.87f, 1.82f); AlignItem(d.stickyNoteItem, BoundsOf(note), .018f);
            foreach (var name in new[] { "OldPCTower", "RefillWaterGallon", "plastic_bottle_gallon", "trashcan", "cardboardBoxOpen" }) Ground(Find(name));
            SetCenter(Find("OldPCTower"),new Vector3(-1.49f,BoundsOf(Find("OldPCTower")).center.y,2.0f));
            foreach (var pair in new[] { ("WaterGallonCollision", "RefillWaterGallon"), ("CardboardBoxCollision", "cardboardBoxOpen") })
                FitBox(Find(pair.Item1).GetComponent<BoxCollider>(), BoundsOf(Find(pair.Item2)));

            // These were arranged in mid-air behind solid cupboard doors. Reuse every book on real shelf tops.
            var books = Find("Kos_Environment_Art").transform.Cast<Transform>().Where(t => t.name == "books")
                .Select(t => t.gameObject).OrderBy(g => BoundsOf(g).min.y).ToArray();
            GameObject stackSupport=cupboard;
            foreach (var book in books)
            {
                if (BoundsOf(book).center.x < 0)
                {
                    // Horizontal stack sitting on top of the existing wall shelf.
                    Resize(book, new Vector3(.15f, .09f, .42f));
                    RestOn(book, Find("wooden_bookshelf_worn"), -2.84f, .95f);
                }
                else
                {
                    Resize(book, new Vector3(.26f, .075f, .11f));
                    RestOn(book,stackSupport,1.65f,2.145f);
                    stackSupport=book;
                }
            }
            // A forgotten receipt beside the wastebasket; retain the original litter without a repeated diagonal row.
            Find("DiscardedPaper0").transform.position = new Vector3(-.30f, .012f, 1.98f);
            Find("DiscardedPaper1").transform.position = new Vector3(-.37f, .013f, 1.79f);
            Find("DiscardedPaper2").transform.position = new Vector3(-.56f, .012f, 1.91f);
        }

        private static void DressRoom()
        {
            var fabric = Pbr("CurtainCottonV2", "fabric_pattern_07", new Color(.73f, .70f, .58f));
            fabric.SetFloat("_Cull", 0);
            Spawn("Original/Models/KosCurtains.fbx", "KosCurtains", new Vector3(2.78f, 1.58f, -.55f), new Vector3(.08f, 1.44f, 1.30f), 90, fabric);
            var basket = Spawn("Imported/PolyHaven/wicker_basket_02/wicker_basket_02.fbx", "wicker_basket_02", new Vector3(2.57f, .24f, .35f), new Vector3(.40f, .46f, .40f), -12, Pbr("wicker_basket_02", "wicker_basket_02", Color.white));
            Ground(basket); AddSolid(basket);
            // Move the original cardboard shipping box away from the wardrobe approach and basket.
            SetCenter(Find("cardboardBoxOpen"), new Vector3(2.59f, .136f, -.20f)); Ground(Find("cardboardBoxOpen"));
            FitBox(Find("CardboardBoxCollision").GetComponent<BoxCollider>(), BoundsOf(Find("cardboardBoxOpen")));
            var container = Spawn("Imported/PolyHaven/plastic_container/plastic_container.fbx", "plastic_container", new Vector3(-2.36f, .17f, -2.03f), new Vector3(.48f, .29f, .30f), 0, Pbr("plastic_container", "plastic_container", Color.white));
            Ground(container); AddSolid(container);
            var sandals = Spawn("Original/Models/KosSandals.fbx", "KosSandals", new Vector3(.60f, .04f, -1.85f), new Vector3(.25f, .062f, .27f), -13, Plain("SandalRubberV2", new Color(.15f, .19f, .17f)));
            Ground(sandals);
            AssignSlots(sandals, name => name.Contains("Strap") ? Plain("SandalStrapV2", new Color(.28f, .34f, .30f)) : Mat("SandalRubberV2"));
            var bag = Spawn("Original/Models/KosBackpack.fbx", "KosBackpack", new Vector3(-2.65f, .23f, .19f), new Vector3(.31f, .44f, .25f), -22, Mat("FadedGreenJacket"));
            Ground(bag); AddSolid(bag);
            AssignSlots(bag, name => name.Contains("Metal") ? Mat("DarkOxidisedMetal") : Mat("FadedGreenJacket"));
            var fan = Spawn("Original/Models/KosDeskFan.fbx", "KosDeskFan", new Vector3(1.65f, 1.71f, 2.04f), new Vector3(.24f, .42f, .18f), -18, Plain("FanEnamelV2", new Color(.40f, .49f, .44f), .25f));
            // Use a shallow wall shelf beside the calendar; the cupboard top is already in use by books.
            var fanShelf = DetailBox("FanWallShelf", new Vector3(.06f, 1.31f, 2.02f), new Vector3(.50f, .035f, .34f), Mat("DoorAndWardrobeWood"));
            RestOn(fan, fanShelf, .06f, 2.02f);
            AssignSlots(fan, name => name.Contains("Metal") ? Mat("DarkOxidisedMetal") : Mat("FanEnamelV2"));
            Rail("FanShelfLeftBracket", new Vector3(-.10f, 1.14f, 2.23f), new Vector3(-.10f, 1.29f, 1.93f), .012f, Mat("DarkOxidisedMetal"));
            Rail("FanShelfRightBracket", new Vector3(.22f, 1.14f, 2.23f), new Vector3(.22f, 1.29f, 1.93f), .012f, Mat("DarkOxidisedMetal"));
            var router = Spawn("Original/Models/KosRouter.fbx", "KosRouter", new Vector3(1.66f, 1.65f, 2.06f), new Vector3(.17f, .16f, .105f), 0, Plain("RouterPlasticV2", new Color(.48f, .47f, .40f)));
            RestOn(router, Find("painted_wooden_cabinet_02"), 1.66f, 2.06f);
            // Router sits beside, not through, the narrow stack of old books.
            SetCenter(router, new Vector3(1.66f, BoundsOf(router).center.y, 2.00f));
            AssignSlots(router, name => name.Contains("Vent") ? Mat("OldComputerPlastic") : Mat("RouterPlasticV2"));
            var black = Mat("OldComputerPlastic");
            DetailBox("PowerStrip", new Vector3(-1.99f, .028f, 2.10f), new Vector3(.10f, .042f, .25f), Mat("AgedSwitchPlastic"));
            for (int i=0; i<3; i++)
            {
                DetailBox("PowerPlug", new Vector3(-1.99f, .064f, 2.025f+i*.074f), new Vector3(.038f,.04f,.045f), black);
                Rail("PlugCable", new Vector3(-1.99f,.076f,2.025f+i*.074f),new Vector3(-1.79f-i*.09f,.012f,2.20f),.004f,black);
            }
            Rail("MonitorSignalCable",new Vector3(-1.25f,.92f,2.01f),new Vector3(-1.31f,.03f,2.18f),.003f,black);
            Rail("KeyboardCable",new Vector3(-1.32f,.77f,1.78f),new Vector3(-1.34f,.77f,2.17f),.002f,black);
            Rail("MouseCable",new Vector3(-.88f,.78f,1.78f),new Vector3(-1.03f,.77f,2.16f),.002f,black);
            Rail("RouterPowerCable",new Vector3(1.67f,1.23f,2.02f),new Vector3(1.67f,.33f,2.215f),.003f,black);
            DetailBox("RouterWallSocket",new Vector3(1.67f,.32f,2.225f),new Vector3(.10f,.12f,.026f),Mat("AgedSwitchPlastic"));
            DetailBox("RouterAdapter",new Vector3(1.67f,.32f,2.19f),new Vector3(.047f,.06f,.05f),black);
            DetailBox("BedsideSocket",new Vector3(-2.975f,.36f,-.20f),new Vector3(.026f,.12f,.10f),Mat("AgedSwitchPlastic"));
            DetailBox("PhoneCharger",new Vector3(-2.94f,.36f,-.20f),new Vector3(.05f,.05f,.04f),black);
            Cable("LoosePhoneChargeLead", new[] {new Vector3(-2.92f,.36f,-.20f),new Vector3(-2.89f,.015f,-.22f),new Vector3(-2.73f,.015f,-.02f),new Vector3(-2.58f,.016f,-.06f),new Vector3(-2.55f,.10f,-.13f)},.003f,black);
            // The fan's lead follows the wall to the existing socket, clear of all walking routes.
            Cable("FanLead",new[]{new Vector3(.06f,1.34f,2.10f),new Vector3(.06f,.28f,2.21f),new Vector3(-2.25f,.28f,2.21f),new Vector3(-2.25f,.42f,2.20f)},.003f,black);
        }

        private static void LightingAndMaterials()
        {
            var profile = Find("KosGlobalAtmosphere").GetComponent<Volume>().sharedProfile;
            foreach (var component in profile.components) component.active = component is Tonemapping || component is ColorAdjustments || component is Bloom || component is Vignette;
            profile.TryGet<Tonemapping>(out var tone); tone.mode.Override(TonemappingMode.ACES);
            profile.TryGet<ColorAdjustments>(out var color);
            color.postExposure.Override(.25f); color.contrast.Override(4); color.saturation.Override(-8); color.colorFilter.Override(Color.white);
            profile.TryGet<Bloom>(out var bloom); bloom.intensity.Override(.08f); bloom.threshold.Override(1.2f);
            profile.TryGet<Vignette>(out var vignette); vignette.intensity.Override(.16f); vignette.smoothness.Override(.5f);
            EditorUtility.SetDirty(profile);
            var ceiling = Find("WeakTungstenCeiling").GetComponent<Light>();
            ceiling.intensity = 16f; ceiling.range = 8; ceiling.spotAngle = 155;
            ceiling.innerSpotAngle = 95; ceiling.color = new Color(1f, .82f, .60f);
            ceiling.shadowResolution = LightShadowResolution.Medium; ceiling.shadowNormalBias = .05f;
            Find("BareWarmBulb").GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
            var window = Find("ColdWindowSpill").GetComponent<Light>();
            // The V1 source was outside the opaque structural wall. Put it on the inner side of the glazing.
            window.transform.position = new Vector3(2.87f, 1.95f, -.55f);
            window.transform.LookAt(new Vector3(-1.8f, .25f, -.25f));
            window.intensity = 1.8f; window.color = new Color(.55f, .67f, .74f); window.shadows = LightShadows.None;
            var fill = Find("WarmWallBounce").GetComponent<Light>();
            fill.intensity = 4.5f; fill.color = new Color(.78f,.72f,.59f);
            var deskLamp = Find("DeskLampPool").GetComponent<Light>(); deskLamp.intensity = 1.5f; deskLamp.color = new Color(1,.78f,.48f);
            var probe = Find("KosReflectionProbe").GetComponent<ReflectionProbe>();
            probe.mode = ReflectionProbeMode.Baked; probe.enabled = false;
            RenderSettings.fog = false; RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;
            RenderSettings.reflectionIntensity = .15f;
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.16f,.17f,.16f);
            RenderSettings.ambientEquatorColor = new Color(.11f,.12f,.11f);
            RenderSettings.ambientGroundColor = new Color(.06f,.055f,.045f);
            Tune("OldLimewash", new Color(.82f,.80f,.71f), .20f, .55f);
            Tune("PeelingSeaGreenPaint",new Color(.48f,.58f,.47f),.22f,.7f);
            Tune("CeramicTiles",new Color(.85f,.83f,.75f),.35f,.7f);
            Tune("DoorAndWardrobeWood",new Color(.78f,.71f,.59f),.28f,.7f);
            var renderer = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(Art+"/Generated/KosRenderer.asset");
            foreach (var feature in renderer.rendererFeatures)
            {
                var so = new SerializedObject(feature);
                var downsample = so.FindProperty("m_Settings.Downsample"); if(downsample!=null) downsample.boolValue=true;
                var intensity = so.FindProperty("m_Settings.Intensity"); if(intensity!=null) intensity.floatValue=.75f;
                var radius = so.FindProperty("m_Settings.Radius"); if(radius!=null) radius.floatValue=.14f;
                so.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(feature);
            }
            var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(Art+"/Generated/KosPipeline.asset");
            var p = new SerializedObject(pipeline);
            // Serialized enum: Disabled=0, PerPixel=1, PerVertex=2 in this installed URP version.
            p.FindProperty("m_AdditionalLightsRenderingMode").intValue = (int)LightRenderingMode.PerPixel;
            p.FindProperty("m_AdditionalLightsPerObjectLimit").intValue = 4;
            p.FindProperty("m_MSAA").intValue = 1;
            p.FindProperty("m_ShadowDistance").floatValue = 10;
            p.FindProperty("m_AdditionalLightsShadowmapResolution").intValue = 1024;
            p.FindProperty("m_MainLightShadowmapResolution").intValue = 1024;
            p.ApplyModifiedPropertiesWithoutUndo(); EditorUtility.SetDirty(pipeline);
        }

        private static void Tune(string name, Color tint, float smooth, float bump)
        {
            var mat=Mat(name); mat.SetColor("_BaseColor",tint); mat.SetFloat("_Smoothness",smooth);
            mat.SetFloat("_BumpScale",bump); EditorUtility.SetDirty(mat);
        }
        private static Material Plain(string name, Color color, float metallic=0)
        {
            var mat=Mat(name);
            if(!mat){mat=new Material(Shader.Find("Universal Render Pipeline/Lit"));AssetDatabase.CreateAsset(mat,Mats+name+".mat");}
            mat.SetColor("_BaseColor",color);mat.SetFloat("_Smoothness",.22f);mat.SetFloat("_Metallic",metallic);mat.enableInstancing=true;EditorUtility.SetDirty(mat);return mat;
        }
        private static Material Pbr(string name,string asset,Color tint)
        {
            var mat=Plain(name,tint);string dir=Art+"/Imported/PolyHaven/"+asset;
            mat.SetTexture("_BaseMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Directory.GetFiles(dir,"*_diff_*jpg").Single()));
            mat.SetTexture("_BumpMap",AssetDatabase.LoadAssetAtPath<Texture2D>(Directory.GetFiles(dir,"*_nor_gl_*jpg").Single()));
            var mask=AssetDatabase.LoadAssetAtPath<Texture2D>(dir+"/"+asset+"_mask.png");
            mat.SetTexture("_MetallicGlossMap",mask);mat.SetTexture("_OcclusionMap",mask);mat.SetFloat("_OcclusionStrength",.8f);
            mat.EnableKeyword("_NORMALMAP");mat.EnableKeyword("_METALLICSPECGLOSSMAP");mat.EnableKeyword("_OCCLUSIONMAP");return mat;
        }
        private static GameObject Spawn(string path,string name,Vector3 center,Vector3 size,float yaw,Material mat)
        {
            var go=new GameObject(name);go.transform.SetParent(details,false);
            var model=(GameObject)PrefabUtility.InstantiatePrefab(AssetDatabase.LoadAssetAtPath<GameObject>(Art+"/"+path));
            model.transform.SetParent(go.transform,false);go.transform.rotation=Quaternion.Euler(0,yaw,0);
            foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(mat,r.sharedMaterials.Length).ToArray();
            Resize(go,size);SetCenter(go,center);return go;
        }
        private static void AssignSlots(GameObject go,Func<string,Material> material)
        {foreach(var r in go.GetComponentsInChildren<Renderer>())r.sharedMaterials=Enumerable.Repeat(material(r.name),r.sharedMaterials.Length).ToArray();}
        private static GameObject DetailBox(string name,Vector3 center,Vector3 size,Material mat)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cube);go.name=name;Object.DestroyImmediate(go.GetComponent<Collider>());
            go.transform.SetParent(details,false);go.transform.position=center;go.transform.localScale=size;go.GetComponent<Renderer>().sharedMaterial=mat;return go;
        }
        private static void Rail(string name,Vector3 a,Vector3 b,float radius,Material mat)
        {
            var go=GameObject.CreatePrimitive(PrimitiveType.Cylinder);go.name=name;Object.DestroyImmediate(go.GetComponent<Collider>());
            go.transform.SetParent(details,false);go.transform.position=(a+b)*.5f;go.transform.up=(b-a).normalized;
            go.transform.localScale=new Vector3(radius*2,Vector3.Distance(a,b)*.5f,radius*2);go.GetComponent<Renderer>().sharedMaterial=mat;
        }
        private static void Cable(string name,Vector3[] points,float width,Material material)
        {
            var go=new GameObject(name,typeof(LineRenderer));go.transform.SetParent(details,false);var line=go.GetComponent<LineRenderer>();
            line.positionCount=points.Length;line.SetPositions(points);line.widthMultiplier=width;line.numCornerVertices=2;line.numCapVertices=2;line.sharedMaterial=material;line.generateLightingData=true;
        }
        public static void SetCenter(GameObject go,Vector3 center) => go.transform.position+=center-BoundsOf(go).center;
        public static void Resize(GameObject go,Vector3 size)
        {
            var b=BoundsOf(go);var s=go.transform.localScale;
            // Objects are yaw-aligned before fitting; world X/Z swap for 90-degree rotations.
            float yaw=Mathf.Abs(Mathf.Sin(go.transform.eulerAngles.y*Mathf.Deg2Rad));
            var factor=new Vector3(size.x/b.size.x,size.y/b.size.y,size.z/b.size.z);
            if(yaw>.99f){float x=factor.x;factor.x=factor.z;factor.z=x;}
            go.transform.localScale=Vector3.Scale(s,factor);
        }
        public static float FloorY => Find("CeramicTileFloor").GetComponent<Renderer>().bounds.max.y;
        private static void Ground(GameObject go) => go.transform.position+=Vector3.up*(FloorY-BoundsOf(go).min.y+.001f);
        public static void FitBox(BoxCollider collider,Bounds bounds)
        {
            collider.center=collider.transform.InverseTransformPoint(bounds.center);
            var s=collider.transform.lossyScale;
            collider.size=new Vector3(bounds.size.x/Mathf.Abs(s.x),bounds.size.y/Mathf.Abs(s.y),bounds.size.z/Mathf.Abs(s.z));
        }
        private static void AlignItem(KosInteractableItem item,Bounds bounds,float minThickness=.025f)
        {
            // Keep the visible descendants fixed while centering the existing interaction root on its geometry.
            Vector3 delta=bounds.center-item.transform.position;
            item.transform.position+=delta;foreach(Transform child in item.transform)child.position-=delta;
            bounds.size=new Vector3(Mathf.Max(bounds.size.x,minThickness),Mathf.Max(bounds.size.y,minThickness),Mathf.Max(bounds.size.z,minThickness));
            FitBox(item.GetComponent<BoxCollider>(),bounds);
        }
        private static void RestOn(GameObject prop,GameObject support,float x,float z)
        {
            float y=SurfaceHeight(support,x,z);
            var b=BoundsOf(prop);SetCenter(prop,new Vector3(x,y+b.size.y*.5f+.001f,z));
        }
        public static Vector3[] MeshVertices(GameObject go) => go.GetComponentsInChildren<MeshFilter>()
            .Where(f=>f.sharedMesh && f.GetComponent<Renderer>().enabled)
            .SelectMany(f=>f.sharedMesh.vertices.Select(v=>f.transform.TransformPoint(v))).ToArray();
        public static float SurfaceHeight(GameObject go,float x,float z)
        {
            // Intersect actual transformed mesh triangles with a vertical ray; bounds alone do not locate a quilt/shelf.
            float top=float.NegativeInfinity;
            foreach(var f in go.GetComponentsInChildren<MeshFilter>())
            {
                if(!f.sharedMesh || !f.GetComponent<Renderer>().enabled)continue;
                var vertices=f.sharedMesh.vertices;var indices=f.sharedMesh.triangles;
                for(int i=0;i<indices.Length;i+=3)
                {
                    var a=f.transform.TransformPoint(vertices[indices[i]]);var b=f.transform.TransformPoint(vertices[indices[i+1]]);var c=f.transform.TransformPoint(vertices[indices[i+2]]);
                    float det=(b.z-c.z)*(a.x-c.x)+(c.x-b.x)*(a.z-c.z);if(Mathf.Abs(det)<.0000001f)continue;
                    float u=((b.z-c.z)*(x-c.x)+(c.x-b.x)*(z-c.z))/det;
                    float v=((c.z-a.z)*(x-c.x)+(a.x-c.x)*(z-c.z))/det;
                    if(u<-.0001f || v<-.0001f || u+v>1.0001f)continue;
                    top=Mathf.Max(top,u*a.y+v*b.y+(1-u-v)*c.y);
                }
            }
            if(float.IsNegativeInfinity(top))throw new Exception("No mesh surface under prop at "+go.name+" "+x+","+z);
            return top;
        }
        private static void AddSolid(GameObject go) {var collider=go.AddComponent<BoxCollider>();FitBox(collider,BoundsOf(go));}
    }
}
#endif
