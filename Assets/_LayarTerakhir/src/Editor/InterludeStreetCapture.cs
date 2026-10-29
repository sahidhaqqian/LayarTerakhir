#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Interlude;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    // Synchronous street review: opens the corridor scene in the editor,
    // applies daytime lighting, poses a capture camera and renders real
    // frames via render-request readback. No PlayMode, no game logic —
    // immune to the runtime main-thread stall seen on llvmpipe players.
    public static class InterludeStreetCapture
    {
        private struct View { public string name; public Vector3 pos, look; }

        public static void Gameplay()
        {
            try
            {
                Directory.CreateDirectory("Audit/Visuals/Interlude-Street");
                EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
                var corridor = Object.FindFirstObjectByType<KosCorridorDirector>();
                if (!corridor) throw new Exception("KosCorridorDirector missing");
                // Permanent street scenery is shared with the night prolog.
                var kampung = GameObject.Find("KampungMelati");
                var alley = kampung ? kampung.transform.Find("InterludeAlley") : null;
                if (!alley) throw new Exception("InterludeAlley group missing");
                alley.gameObject.SetActive(true);
                Physics.SyncTransforms();
                var cam = new GameObject("InterludeCaptureCamera").AddComponent<Camera>();
                cam.farClipPlane = 130;
                cam.nearClipPlane = .1f;
                InterludeExterior.ApplyDay(cam);
                // Warmup: the first synchronous render after scene load can
                // produce wrong (blown-out) output before shaders/pipeline
                // state settle. Discard it so every kept shot is real.
                KosRenderDiagnosis.Capture(cam, "_warmup", false, "Audit/Visuals/Interlude-Street");
                try { System.IO.File.Delete("Audit/Visuals/Interlude-Street/_warmup.png"); } catch { }
                if (System.Environment.GetEnvironmentVariable("INTERLUDE_NOVOLUME") == "1")
                {
                    foreach (var v in Object.FindObjectsByType<UnityEngine.Rendering.Volume>(FindObjectsSortMode.None)) v.enabled = false;
                    Debug.Log("INTERLUDE_STREET_CAPTURE volumes disabled");
                }
                var spots = new List<View>
                {
                    new View { name = "01-yard-gate", pos = corridor.BuildingPoint(new Vector3(0, 1.5f, -3f)), look = corridor.BuildingPoint(new Vector3(.5f, 1f, -6.5f)) },
                    new View { name = "02-alley-west", pos = new Vector3(-14f, 1.5f, -11.75f), look = new Vector3(-8f, 1.2f, -11.75f) },
                    new View { name = "03-alley-east", pos = new Vector3(-5f, 1.5f, -11.75f), look = new Vector3(2f, 1.2f, -14f) },
                    new View { name = "04-street-warung", pos = new Vector3(2f, 1.6f, -18f), look = new Vector3(8.4f, 1.2f, -24f) },
                    new View { name = "05-warteg-front", pos = new Vector3(3.5f, 1.6f, -27f), look = new Vector3(7f, 1.4f, -30f) },
                    new View { name = "06-street-north", pos = new Vector3(0, 1.6f, -30f), look = new Vector3(0, 1.4f, -70f) },
                    new View { name = "07-gerobak", pos = new Vector3(-2f, 1.5f, -22f), look = new Vector3(-4.4f, 1f, -20f) },
                    new View { name = "08-warteg-etalase", pos = new Vector3(4.6f, 1.5f, -28.6f), look = new Vector3(6.5f, 1.4f, -30f) }
                };
                var metrics = new List<KosRenderDiagnosis.Metric>();
                string only = System.Environment.GetEnvironmentVariable("INTERLUDE_SHOT");
                foreach (var v in spots)
                {
                    if (!string.IsNullOrEmpty(only) && v.name != only) continue;
                    cam.transform.position = v.pos;
                    cam.transform.LookAt(v.look);
                    metrics.Add(KosRenderDiagnosis.Capture(cam, v.name, false, "Audit/Visuals/Interlude-Street"));
                }
                File.WriteAllText("Audit/Visuals/Interlude-Street/metrics.json",
                    JsonUtility.ToJson(new Report { gpu = SystemInfo.graphicsDeviceName, shots = metrics.ToArray() }, true) + "\n");
                Debug.Log("INTERLUDE_STREET_CAPTURE done shots=" + metrics.Count);
                EditorApplication.Exit(0);
            }
            catch (Exception e) { Debug.LogException(e); EditorApplication.Exit(1); }
        }

        [Serializable] private class Report { public string gpu; public KosRenderDiagnosis.Metric[] shots; }
    }
}
#endif
