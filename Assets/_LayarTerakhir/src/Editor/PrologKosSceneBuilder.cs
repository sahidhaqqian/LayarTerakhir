#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Editor
{
    public static class PrologKosSceneBuilder
    {
        public const string SceneDirectory = "Assets/_LayarTerakhir/assets/Scenes";
        public const string ScenePath = "Assets/_LayarTerakhir/assets/Scenes/Prolog_KamarKos.unity";

        // Dimensi baru kamar kos yang lebih lega (6.0m x 4.5m, tinggi 3.1m)
        public const float RoomWidth = 6.0f;
        public const float RoomDepth = 4.5f;
        public const float RoomHeight = 3.1f;

        [MenuItem("LayarTerakhir/Build Prolog Kamar Kos Scene")]
        public static void BuildSceneMenu()
        {
            BuildSceneHeadless();
        }

        public static void BuildSceneHeadless()
        {
            if (!Directory.Exists(SceneDirectory))
            {
                Directory.CreateDirectory(SceneDirectory);
            }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(0.06f, 0.08f, 0.10f);

            GameObject systemsRoot = new GameObject("GameSystems");
            systemsRoot.AddComponent<GameInputConfig>();
            systemsRoot.AddComponent<CampaignState>();
            HudController hud = systemsRoot.AddComponent<HudController>();
            hud.EnsureRuntimeUIBuilt();
            KamarKosPrologDirector director = systemsRoot.AddComponent<KamarKosPrologDirector>();

            GameObject roomRoot = new GameObject("KamarKos_4x3m_Greybox");

            // Arsitektur Dasar 6.0m x 4.5m x 3.1m
            CreateBox("Floor_4x3m", roomRoot.transform, new Vector3(0f, -0.05f, 0f), new Vector3(RoomWidth, 0.1f, RoomDepth), new Color(0.22f, 0.20f, 0.18f));
            CreateBox("Ceiling_4x3m", roomRoot.transform, new Vector3(0f, RoomHeight + 0.05f, 0f), new Vector3(RoomWidth, 0.1f, RoomDepth), new Color(0.25f, 0.25f, 0.25f));
            CreateBox("Wall_North", roomRoot.transform, new Vector3(0f, RoomHeight * 0.5f, (RoomDepth * 0.5f) + 0.05f), new Vector3(RoomWidth, RoomHeight, 0.1f), new Color(0.32f, 0.31f, 0.28f));
            CreateBox("Wall_South", roomRoot.transform, new Vector3(0f, RoomHeight * 0.5f, -(RoomDepth * 0.5f) - 0.05f), new Vector3(RoomWidth, RoomHeight, 0.1f), new Color(0.32f, 0.31f, 0.28f));
            CreateBox("Wall_East", roomRoot.transform, new Vector3((RoomWidth * 0.5f) + 0.05f, RoomHeight * 0.5f, 0f), new Vector3(0.1f, RoomHeight, RoomDepth), new Color(0.30f, 0.29f, 0.27f));
            CreateBox("Wall_West", roomRoot.transform, new Vector3(-(RoomWidth * 0.5f) - 0.05f, RoomHeight * 0.5f, 0f), new Vector3(0.1f, RoomHeight, RoomDepth), new Color(0.30f, 0.29f, 0.27f));

            // Perabot Utama (ditata merapat ke dinding agar tengah ruangan lega)
            CreateBox("MejaKerjaPC", roomRoot.transform, new Vector3(-1.25f, 0.72f, 1.90f), new Vector3(1.3f, 0.06f, 0.65f), new Color(0.28f, 0.18f, 0.10f));
            CreateBox("KakiMejaKiri", roomRoot.transform, new Vector3(-1.85f, 0.35f, 1.90f), new Vector3(0.08f, 0.7f, 0.6f), new Color(0.20f, 0.13f, 0.08f));
            CreateBox("KakiMejaKanan", roomRoot.transform, new Vector3(-0.65f, 0.35f, 1.90f), new Vector3(0.08f, 0.7f, 0.6f), new Color(0.20f, 0.13f, 0.08f));
            CreateBox("KasurKos", roomRoot.transform, new Vector3(-2.35f, 0.22f, -0.95f), new Vector3(1.1f, 0.44f, 1.8f), new Color(0.35f, 0.38f, 0.44f));
            CreateBox("LemariPakaian", roomRoot.transform, new Vector3(2.55f, 1.0f, 1.55f), new Vector3(0.75f, 2.0f, 0.9f), new Color(0.26f, 0.17f, 0.11f));

            GameObject roomLightObj = new GameObject("RoomCeilingLight");
            roomLightObj.transform.SetParent(roomRoot.transform, false);
            roomLightObj.transform.position = new Vector3(0f, 2.95f, 0f);
            Light roomLight = roomLightObj.AddComponent<Light>();
            roomLight.type = LightType.Point;
            roomLight.range = 8.5f;
            roomLight.intensity = 0.85f;
            roomLight.color = new Color(1.0f, 0.88f, 0.72f);

            GameObject pcLightObj = new GameObject("PCMonitorGlow");
            pcLightObj.transform.SetParent(roomRoot.transform, false);
            pcLightObj.transform.position = new Vector3(-1.25f, 1.05f, 1.82f);
            Light pcLight = pcLightObj.AddComponent<Light>();
            pcLight.type = LightType.Point;
            pcLight.range = 2.8f;
            pcLight.intensity = 0.65f;
            pcLight.color = new Color(0.45f, 0.75f, 1.0f);

            director.pcMonitorItem = CreateInteractableBox(
                "Interactable_PCMonitor",
                roomRoot.transform,
                new Vector3(-1.25f, 1.02f, 2.00f),
                new Vector3(0.55f, 0.36f, 0.06f),
                new Color(0.10f, 0.16f, 0.24f),
                "PC_MONITOR",
                "Periksa Monitor PC (Kanal Malam Keramat)",
                director
            );

            director.stickyNoteItem = CreateInteractableBox(
                "Interactable_StickyNote",
                roomRoot.transform,
                new Vector3(-0.87f, 0.77f, 1.80f),
                new Vector3(0.14f, 0.02f, 0.14f),
                new Color(0.95f, 0.90f, 0.30f),
                "STICKY_NOTE",
                "Baca Sticky Note di Meja PC",
                director
            );

            director.lockedDrawerItem = CreateInteractableBox(
                "Interactable_LaciMejaTerkunci",
                roomRoot.transform,
                new Vector3(-0.90f, 0.56f, 1.57f),
                new Vector3(0.42f, 0.22f, 0.08f),
                new Color(0.36f, 0.22f, 0.12f),
                "LACI_MEJA",
                "Buka Laci Meja Kerja (Gembok Angka 3 Digit)",
                director
            );

            director.wallCalendarItem = CreateInteractableBox(
                "Interactable_KalenderDinding",
                roomRoot.transform,
                new Vector3(0.95f, 1.45f, 2.23f),
                new Vector3(0.38f, 0.52f, 0.03f),
                new Color(0.90f, 0.88f, 0.85f),
                "KALENDER_DINDING",
                "Periksa Kalender Dinding (Sebelah Lemari)",
                director
            );

            director.suratKosItem = CreateInteractableBox(
                "Interactable_SuratIbuKos",
                roomRoot.transform,
                new Vector3(1.25f, 0.02f, -2.07f),
                new Vector3(0.28f, 0.02f, 0.22f),
                new Color(0.85f, 0.18f, 0.18f),
                "SURAT_KOS",
                "Ambil Kertas Merah di Bawah Pintu Kamar",
                director
            );

            director.smartphoneBedItem = CreateInteractableBox(
                "Interactable_SmartphoneDiKasur",
                roomRoot.transform,
                new Vector3(-2.15f, 0.46f, -0.85f),
                new Vector3(0.12f, 0.03f, 0.22f),
                new Color(0.12f, 0.12f, 0.15f),
                "SMARTPHONE_BED",
                "Ambil Smartphone di Atas Kasur",
                director
            );

            GameObject redLedObj = new GameObject("SmartphoneRedLED");
            redLedObj.transform.SetParent(director.smartphoneBedItem.transform, false);
            redLedObj.transform.localPosition = new Vector3(0f, 0.12f, 0f);
            Light redLed = redLedObj.AddComponent<Light>();
            redLed.type = LightType.Point;
            redLed.color = new Color(1.0f, 0.12f, 0.08f);
            redLed.range = 1.8f;
            redLed.intensity = 2.4f;
            redLed.enabled = false;
            director.phoneBedRedLed = redLed;

            director.kosDoorItem = CreateInteractableBox(
                "Interactable_PintuKamarKos",
                roomRoot.transform,
                new Vector3(1.25f, 1.05f, -2.23f),
                new Vector3(0.92f, 2.1f, 0.08f),
                new Color(0.33f, 0.20f, 0.12f),
                "PINTU_KOS",
                "Buka Pintu Kamar Kos (Berangkat ke Rumah Atmaja)",
                director
            );

            director.jacketItem = CreateInteractableBox(
                "Interactable_JaketDiBalikPintu",
                roomRoot.transform,
                new Vector3(1.55f, 1.35f, -2.17f),
                new Vector3(0.32f, 0.65f, 0.10f),
                new Color(0.16f, 0.24f, 0.18f),
                "JAKET_PINTU",
                "Ambil Jaket di Balik Pintu Kamar",
                director
            );

            GameObject playerObj = new GameObject("Player_RakaWijaya");
            playerObj.tag = "Player";
            playerObj.transform.position = new Vector3(-0.4f, 0.05f, 0.25f);
            playerObj.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            CharacterController cc = playerObj.AddComponent<CharacterController>();
            cc.radius = 0.32f;
            cc.height = 1.75f;
            cc.center = new Vector3(0f, 0.875f, 0f);

            FirstPersonController fpp = playerObj.AddComponent<FirstPersonController>();

            GameObject camPivot = new GameObject("CameraPivot");
            camPivot.transform.SetParent(playerObj.transform, false);
            camPivot.transform.localPosition = new Vector3(0f, 1.6f, 0f);
            fpp.cameraPivot = camPivot.transform;

            GameObject camObj = new GameObject("MainCamera");
            camObj.tag = "MainCamera";
            camObj.transform.SetParent(camPivot.transform, false);
            Camera mainCam = camObj.AddComponent<Camera>();
            mainCam.nearClipPlane = 0.05f;
            mainCam.fieldOfView = 68f;
            camObj.AddComponent<AudioListener>();

            InteractionInteractor interactor = playerObj.AddComponent<InteractionInteractor>();
            interactor.playerCamera = mainCam;

            SmartphoneController phoneCtrl = playerObj.AddComponent<SmartphoneController>();
            GameObject handPhoneMesh = CreateBox(
                "HandSmartphoneRig",
                camObj.transform,
                new Vector3(0.24f, -0.18f, 0.38f),
                new Vector3(0.08f, 0.15f, 0.02f),
                new Color(0.10f, 0.10f, 0.12f),
                withCollider: false
            );
            handPhoneMesh.transform.localPosition = new Vector3(0.24f, -0.18f, 0.38f);
            phoneCtrl.handPhoneMeshRoot = handPhoneMesh;
            handPhoneMesh.SetActive(false);

            GameObject flashObj = new GameObject("SmartphoneFlashlight");
            flashObj.transform.SetParent(handPhoneMesh.transform, false);
            flashObj.transform.localPosition = new Vector3(0f, 0f, 0.05f);
            Light flashLight = flashObj.AddComponent<Light>();
            flashLight.type = LightType.Spot;
            flashLight.spotAngle = 58f;
            flashLight.range = 12f;
            flashLight.intensity = 1.6f;
            flashLight.enabled = false;
            phoneCtrl.phoneFlashlight = flashLight;

            EditorSceneManager.SaveScene(scene, ScenePath);
            RegisterSceneInBuildSettings(ScenePath);
            AssetDatabase.SaveAssets();
            Debug.Log($"[PrologKosSceneBuilder] Scene Kamar Kos (6.0x4.5m) berhasil dibuat dan disimpan di: {ScenePath}");
        }

        private static void RegisterSceneInBuildSettings(string scenePath)
        {
            var updated = new List<EditorBuildSettingsScene> { new EditorBuildSettingsScene(scenePath, true) };
            foreach (var scene in EditorBuildSettings.scenes)
                if (scene.path != scenePath) updated.Add(scene);
            EditorBuildSettings.scenes = updated.ToArray();
        }

        public static void RepairFoundationSceneHeadless()
        {
            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var phone = Object.FindFirstObjectByType<SmartphoneController>();
            phone.handPhoneMeshRoot.transform.localPosition = new Vector3(0.24f, -0.18f, 0.38f);
            phone.handPhoneMeshRoot.SetActive(false);
            phone.phoneFlashlight.enabled = false;
            Object.FindFirstObjectByType<FirstPersonController>().transform.position = new Vector3(-0.4f, 0.05f, 0.25f);
            EditorSceneManager.SaveScene(scene);
            RegisterSceneInBuildSettings(ScenePath);
            AssetDatabase.SaveAssets();
        }

        private static KosInteractableItem CreateInteractableBox(
            string name,
            Transform parent,
            Vector3 position,
            Vector3 scale,
            Color color,
            string itemId,
            string prompt,
            KamarKosPrologDirector director)
        {
            GameObject go = CreateBox(name, parent, position, scale, color, withCollider: true);
            KosInteractableItem item = go.AddComponent<KosInteractableItem>();
            item.itemId = itemId;
            item.promptLabel = prompt;
            item.director = director;
            return item;
        }

        private static GameObject CreateBox(
            string name,
            Transform parent,
            Vector3 position,
            Vector3 scale,
            Color color,
            bool withCollider = true)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent, false);
            go.transform.position = position;
            go.transform.localScale = scale;

            if (!withCollider)
            {
                Collider col = go.GetComponent<Collider>();
                if (col != null)
                    Object.DestroyImmediate(col);
            }

            Renderer rend = go.GetComponent<Renderer>();
            if (rend != null)
            {
                Shader shader = Shader.Find("Universal Render Pipeline/Lit");
                if (shader == null) shader = Shader.Find("Standard");
                if (shader != null)
                {
                    Material mat = new Material(shader);
                    mat.color = color;
                    if (mat.HasProperty("_BaseColor"))
                        mat.SetColor("_BaseColor", color);
                    rend.sharedMaterial = mat;
                }
            }

            return go;
        }
    }
}
#endif

