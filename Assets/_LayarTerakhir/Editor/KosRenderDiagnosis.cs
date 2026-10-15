#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    [InitializeOnLoad]
    public static class KosRenderDiagnosis
    {
        static KosRenderDiagnosis()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool("KosV2.Diagnose", false)) return;
                double ready = EditorApplication.timeSinceStartup + 5;
                EditorApplication.CallbackFunction tick = null;
                tick = () =>
                {
                    if (EditorApplication.timeSinceStartup < ready) return;
                    EditorApplication.update -= tick;
                    SessionState.SetBool("KosV2.Diagnose", false);
                    try { Diagnose(); EditorApplication.Exit(0); }
                    catch (Exception e) { Debug.LogException(e); EditorApplication.Exit(1); }
                };
                EditorApplication.update += tick;
            };
        }

        public static void Run()
        {
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", Path.Combine(Path.GetTempPath(), "KosV2-Diagnosis-" + Guid.NewGuid()));
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            Directory.CreateDirectory("Audit/Visuals/V2-Diagnosis");
            var groups = GameObject.Find("Kos_Environment_Art").transform.Cast<Transform>()
                .Where(t => t.GetComponentsInChildren<Renderer>().Any()).Select(t =>
                {
                    var rs = t.GetComponentsInChildren<Renderer>();
                    var b = rs[0].bounds;
                    foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds);
                    return new Entry { name = t.name, position = t.position, rotation = t.eulerAngles, scale = t.lossyScale, min = b.min, max = b.max };
                }).ToArray();
            File.WriteAllText("Audit/kos-v2-before-bounds.json", JsonUtility.ToJson(new BoundsReport { objects = groups }, true));
            SessionState.SetBool("KosV2.Diagnose", true);
            EditorApplication.EnterPlaymode();
        }

        private static void Diagnose()
        {
            var camera = Camera.main;
            camera.transform.position = new Vector3(2.45f, 1.7f, -1.75f);
            camera.transform.LookAt(new Vector3(-1.35f, 1.05f, 1.25f));
            var pipeline = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
            var serialized = new SerializedObject(pipeline);
            var lightMode = serialized.FindProperty("m_AdditionalLightsRenderingMode");
            int original = lightMode.intValue;
            var metrics = new System.Collections.Generic.List<Metric>();
            metrics.Add(Capture(camera, "01-original-HDR", true));
            metrics.Add(Capture(camera, "02-original-sRGB", false));
            camera.GetUniversalAdditionalCameraData().renderPostProcessing = false;
            metrics.Add(Capture(camera, "03-no-post", false));
            camera.GetUniversalAdditionalCameraData().renderPostProcessing = true;
            var profile = GameObject.Find("KosGlobalAtmosphere").GetComponent<Volume>().profile;
            profile.TryGet<SplitToning>(out var split);
            split.active = false;
            metrics.Add(Capture(camera, "04-no-split-toning", false));
            profile.TryGet<ShadowsMidtonesHighlights>(out var smh);
            smh.active = false;
            metrics.Add(Capture(camera, "05-no-split-or-SMH", false));
            var bulb = GameObject.Find("BareWarmBulb").GetComponent<Renderer>();
            bulb.shadowCastingMode = ShadowCastingMode.Off;
            metrics.Add(Capture(camera, "06-no-bulb-shadow", false));
            lightMode.intValue = original; serialized.ApplyModifiedPropertiesWithoutUndo();
            File.WriteAllText("Audit/Visuals/V2-Diagnosis/metrics.json", JsonUtility.ToJson(new Diagnosis {
                gpu = SystemInfo.graphicsDeviceName, originalAdditionalLightsMode = original,
                originalModeName = ((LightRenderingMode)original).ToString(), shots = metrics.ToArray()
            }, true));
        }

        public static Metric Capture(Camera camera, string name, bool hdr = false, string folder = "Audit/Visuals/V2-Diagnosis")
        {
            Directory.CreateDirectory(folder);
            var previousActive = RenderTexture.active;
            var previousTarget = camera.targetTexture;
            float previousAspect = camera.aspect;
            var rt = new RenderTexture(1280, 720, 24, hdr ? RenderTextureFormat.DefaultHDR : RenderTextureFormat.ARGB32,
                hdr ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
            var texture = new Texture2D(1280, 720, TextureFormat.RGB24, false, false);
            try
            {
                rt.Create();
                camera.aspect = 16f / 9f;
                RenderPipeline.SubmitRenderRequest(camera, new RenderPipeline.StandardRequest { destination = rt });
                RenderTexture.active = rt;
                texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
                texture.Apply();
                File.WriteAllBytes(folder + "/" + name + ".png", texture.EncodeToPNG());
                double sum = 0, squares = 0; int dark = 0;
                var pixels = texture.GetPixels32();
                foreach (var p in pixels)
                {
                    double l = (.2126 * p.r + .7152 * p.g + .0722 * p.b) / 255;
                    sum += l; squares += l * l; if (l < .08) dark++;
                }
                float mean = (float)(sum / pixels.Length);
                var metric = new Metric { name = name, cameraPosition=camera.transform.position,cameraForward=camera.transform.forward,
                    mean = mean, contrast = (float)Math.Sqrt(squares / pixels.Length - mean * mean), darkRatio = (float)dark / pixels.Length };
                Debug.Log("KOS_V2_CAPTURE " + JsonUtility.ToJson(metric));
                return metric;
            }
            finally
            {
                camera.targetTexture = previousTarget; camera.aspect = previousAspect;
                RenderTexture.active = previousActive; rt.Release();
                Object.DestroyImmediate(texture); Object.DestroyImmediate(rt);
            }
        }

        [Serializable] public class Entry { public string name; public Vector3 position, rotation, scale, min, max; }
        [Serializable] public class BoundsReport { public Entry[] objects; }
        [Serializable] public class Metric { public string name; public Vector3 cameraPosition,cameraForward; public float mean, contrast, darkRatio; }
        [Serializable] public class Diagnosis { public string gpu, originalModeName; public int originalAdditionalLightsMode; public Metric[] shots; }
    }
}
#endif
