#if UNITY_EDITOR
using System;
using System.IO;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Editor
{
    [InitializeOnLoad]
    public static class KosVisualCapture
    {
        [Serializable]
        public class ShotMetrics
        {
            public string shotName;
            public float meanLuminance;
            public float stdDevLuminance;
            public float darkPixelRatio;      // Rasio area gelap (< 0.08) khas horor
            public float highlightPixelRatio; // Rasio area cahaya/bloom (> 0.75)
            public float tealVsWarmBalance;   // Positif = dominan hangat, Negatif = dominan dingin/teal
            public string critiqueHint;
        }

        [Serializable]
        public class VisualTelemetryReport
        {
            public string gpuName;
            public bool postProcessingActive;
            public ShotMetrics[] shots;
        }

        static KosVisualCapture()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool("KosVisualCapture.Play", false)) return;
                double ready = EditorApplication.timeSinceStartup + 3.5;
                EditorApplication.CallbackFunction callback = null;
                callback = () =>
                {
                    if (EditorApplication.timeSinceStartup < ready) return;
                    EditorApplication.update -= callback;
                    SessionState.SetBool("KosVisualCapture.Play", false);
                    try
                    {
                        Directory.CreateDirectory("Audit/Visuals");
                        var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                        var camData = camera.GetUniversalAdditionalCameraData();
                        camData.renderPostProcessing = true;

                        var director = UnityEngine.Object.FindFirstObjectByType<KamarKosPrologDirector>();
                        var phone = UnityEngine.Object.FindFirstObjectByType<SmartphoneController>();

                        var metricsList = new System.Collections.Generic.List<ShotMetrics>();

                        // 1. Sudut Pandang Awal Pemain Bangun (Startup)
                        metricsList.Add(Shot(camera, "01-gameplay-startup"));

                        // 2. Wide Shot Seluruh Kamar dari Sudut Tenggara (Menilai Keluasan & Kontras)
                        camera.transform.position = new Vector3(2.45f, 1.70f, -1.75f);
                        camera.transform.LookAt(new Vector3(-1.35f, 1.05f, 1.25f));
                        metricsList.Add(Shot(camera, "02-gameplay-wide-room"));

                        // 3. Close-up Meja Kerja PC, Lampu Meja, & Gembok Laci
                        camera.transform.position = new Vector3(-0.35f, 1.45f, 0.65f);
                        camera.transform.LookAt(new Vector3(-1.25f, 0.90f, 1.95f));
                        metricsList.Add(Shot(camera, "03-gameplay-desk-pc"));

                        // 4. Jendela Malam, Teralis Besi & Spill Cahaya Bulan (Khas DreadOut)
                        camera.transform.position = new Vector3(0.45f, 1.50f, -0.20f);
                        camera.transform.LookAt(new Vector3(2.95f, 1.55f, -0.55f));
                        metricsList.Add(Shot(camera, "04-gameplay-window-moonlight"));

                        // 5. Area Kasur & LED Merah Smartphone Menyala
                        if (director != null && director.phoneBedRedLed != null)
                            director.phoneBedRedLed.enabled = true;
                        camera.transform.position = new Vector3(-0.85f, 1.35f, -0.35f);
                        camera.transform.LookAt(new Vector3(-2.25f, 0.50f, -0.90f));
                        metricsList.Add(Shot(camera, "05-gameplay-bed-red-led"));

                        // 6. Simulasi Senter Smartphone Menyala ke Arah Pintu & Jaket
                        camera.transform.position = new Vector3(-0.10f, 1.60f, -0.45f);
                        camera.transform.LookAt(new Vector3(1.30f, 1.15f, -2.15f));
                        if (phone != null && phone.phoneFlashlight != null)
                        {
                            phone.handPhoneMeshRoot.SetActive(true);
                            phone.phoneFlashlight.enabled = true;
                            phone.phoneFlashlight.transform.position = camera.transform.position;
                            phone.phoneFlashlight.transform.rotation = camera.transform.rotation;
                        }
                        metricsList.Add(Shot(camera, "06-gameplay-door-flashlight"));

                        var report = new VisualTelemetryReport
                        {
                            gpuName = SystemInfo.graphicsDeviceName,
                            postProcessingActive = camData.renderPostProcessing,
                            shots = metricsList.ToArray()
                        };
                        File.WriteAllText("Audit/Visuals/visual-metrics.json", JsonUtility.ToJson(report, true) + "\n");

                        Debug.Log("KOS_CAPTURE ActualEditorPlayMode=True GPU=" + SystemInfo.graphicsDeviceName);
                        EditorApplication.Exit(0);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogException(ex);
                        EditorApplication.Exit(1);
                    }
                };
                EditorApplication.update += callback;
            };
        }

        public static void Gameplay()
        {
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", Path.Combine(Path.GetTempPath(), "KosVisualCapture-" + Guid.NewGuid()));
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            KosVisualUpgrade.ValidateArt();
            Physics.SyncTransforms();
            SessionState.SetBool("KosVisualCapture.Play", true);
            EditorApplication.EnterPlaymode();
        }

        public static void Before() => Capture("before");
        public static void After() => Capture("after");

        private static void Capture(string prefix)
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            int frames = 0;
            EditorApplication.CallbackFunction callback = null;
            callback = () =>
            {
                if (++frames < 30) return;
                EditorApplication.update -= callback;
                try
                {
                    var camera = UnityEngine.Object.FindFirstObjectByType<Camera>();
                    Directory.CreateDirectory("Audit/Visuals");
                    Shot(camera, prefix + "-startup");
                    camera.transform.position = new Vector3(2.45f, 1.70f, -1.75f);
                    camera.transform.LookAt(new Vector3(-1.35f, 1.05f, 1.25f));
                    Shot(camera, prefix + "-room");
                    camera.transform.position = new Vector3(-0.10f, 1.60f, -0.45f);
                    camera.transform.LookAt(new Vector3(1.30f, 1.15f, -2.15f));
                    Shot(camera, prefix + "-door");
                    Debug.Log("KOS_CAPTURE Completed " + prefix + " GPU=" + SystemInfo.graphicsDeviceName);
                    EditorApplication.Exit(0);
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    EditorApplication.Exit(1);
                }
            };
            EditorApplication.update += callback;
        }

        private static ShotMetrics Shot(Camera camera, string name)
        {
            var target = new RenderTexture(1280, 720, 24, RenderTextureFormat.DefaultHDR);
            target.Create();
            camera.aspect = 1280f / 720f;
            camera.targetTexture = target;
            camera.Render();
            RenderPipeline.SubmitRenderRequest(camera, new RenderPipeline.StandardRequest { destination = target });
            RenderTexture.active = target;
            var texture = new Texture2D(1280, 720, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(0, 0, 1280, 720), 0, 0);
            texture.Apply();
            File.WriteAllBytes("Audit/Visuals/" + name + ".png", texture.EncodeToPNG());

            ShotMetrics metrics = AnalyzeTexture(texture, name);

            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(texture);
            UnityEngine.Object.DestroyImmediate(target);
            Debug.Log($"KOS_CAPTURE Saved {name} | MeanLum={metrics.meanLuminance:F3} Contrast={metrics.stdDevLuminance:F3} Critique={metrics.critiqueHint}");
            return metrics;
        }

        private static ShotMetrics AnalyzeTexture(Texture2D tex, string name)
        {
            Color32[] pixels = tex.GetPixels32();
            int total = pixels.Length;
            double sumLum = 0, sumSqLum = 0, sumWarmMinusCool = 0;
            int darkCount = 0, brightCount = 0;

            for (int i = 0; i < total; i += 4) // Subsample setiap 4 piksel agar cepat
            {
                float r = pixels[i].r / 255f;
                float g = pixels[i].g / 255f;
                float b = pixels[i].b / 255f;
                float lum = 0.2126f * r + 0.7152f * g + 0.0722f * b;
                sumLum += lum;
                sumSqLum += lum * lum;
                sumWarmMinusCool += (r - b);
                if (lum < 0.08f) darkCount++;
                if (lum > 0.75f) brightCount++;
            }

            int sampled = total / 4;
            float mean = (float)(sumLum / sampled);
            float variance = Mathf.Max(0f, (float)(sumSqLum / sampled) - (mean * mean));
            float stdDev = Mathf.Sqrt(variance);
            float darkRatio = (float)darkCount / sampled;
            float brightRatio = (float)brightCount / sampled;

            string hint = "PASS: Atmosfer chiaroscuro sesuai target DreadOut.";
            if (mean > 0.32f)
                hint = "TOO_BRIGHT: Ruangan terlalu terang/flat. Kurangi ambient light atau intensitas lampu plafon.";
            else if (mean < 0.05f)
                hint = "TOO_DARK: Ruangan terlalu gelap buta. Naikkan sedikit postExposure atau lampu aksen.";
            else if (stdDev < 0.09f)
                hint = "LOW_CONTRAST: Gambar pucat/kurang dramatis. Naikkan contrast di ColorAdjustments atau tajamkan rasio lampu spot vs shadow.";
            else if (darkRatio < 0.25f)
                hint = "LACKS_DEEP_SHADOWS: Sudut ruangan kurang gelap. Tingkatkan Vignette atau fogDensity.";

            return new ShotMetrics
            {
                shotName = name,
                meanLuminance = mean,
                stdDevLuminance = stdDev,
                darkPixelRatio = darkRatio,
                highlightPixelRatio = brightRatio,
                tealVsWarmBalance = (float)(sumWarmMinusCool / sampled),
                critiqueHint = hint
            };
        }
    }
}
#endif
