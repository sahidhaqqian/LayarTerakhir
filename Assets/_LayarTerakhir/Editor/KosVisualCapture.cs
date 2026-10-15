#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

namespace LayarTerakhir.Editor
{
    [InitializeOnLoad]
    public static class KosVisualCapture
    {
        static KosVisualCapture()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool("KosVisualCapture.Play", false)) return;
                double ready = EditorApplication.timeSinceStartup + 4;
                EditorApplication.CallbackFunction callback = null;
                callback = () =>
                {
                    if (EditorApplication.timeSinceStartup < ready) return;
                    EditorApplication.update -= callback;
                    SessionState.SetBool("KosVisualCapture.Play", false);
                    try
                    {
                        var camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                        Shot(camera, "gameplay-startup");
                        camera.transform.position = new Vector3(1.65f, 1.65f, -1.15f);
                        camera.transform.LookAt(new Vector3(-.85f, 1.0f, .35f));
                        Shot(camera, "gameplay-room");
                        Debug.Log("KOS_CAPTURE ActualEditorPlayMode=True GPU=" + SystemInfo.graphicsDeviceName);
                        EditorApplication.Exit(0);
                    }
                    catch (Exception ex) { Debug.LogException(ex); EditorApplication.Exit(1); }
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
            var hits = Physics.CapsuleCastAll(new Vector3(.3f, .37f, 0), new Vector3(.3f, 1.48f, 0), .32f, Vector3.forward, 2);
            foreach (var hit in hits) Debug.Log("KOS_MOVEMENT_CLEARANCE " + hit.collider.name + " distance=" + hit.distance);
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
                    camera.transform.position = new Vector3(1.65f, 1.65f, -1.15f);
                    camera.transform.LookAt(new Vector3(-0.85f, 1.0f, 0.35f));
                    Shot(camera, prefix + "-room");
                    camera.transform.position = new Vector3(-0.20f, 1.65f, 0.20f);
                    camera.transform.LookAt(new Vector3(1.0f, 1.15f, -1.4f));
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

        private static void Shot(Camera camera, string name)
        {
            var target = new RenderTexture(1280, 720, 24, RenderTextureFormat.ARGB32);
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
            camera.targetTexture = null;
            RenderTexture.active = null;
            UnityEngine.Object.DestroyImmediate(texture);
            UnityEngine.Object.DestroyImmediate(target);
            Debug.Log("KOS_CAPTURE Saved " + name);
        }
    }
}
#endif
