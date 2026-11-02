#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    // One-shot visual proof for the designer: renders the CC0 clock, the rusted
    // vent and the crowbar straight from the saved scene and saves PNGs under
    // Audit/Visuals/ClockVentProof. Run headful under xvfb (no -nographics).
    public static class ClockPhoto
    {
        public static void CaptureProof()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<LayarTerakhir.Prolog.AtmajaDirector>();
            Physics.SyncTransforms();
            var cam = new GameObject("ProofCam").AddComponent<Camera>();
            cam.nearClipPlane = .05f;
            cam.farClipPlane = 70;
            cam.backgroundColor = RenderSettings.fogColor;
            cam.clearFlags = CameraClearFlags.SolidColor;

            var hour = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault(t => t && t.name.StartsWith("ClockHourBlade__"));
            if (hour)
            {
                cam.transform.position = hour.position + d.east * 1.5f + Vector3.up * .1f;
                cam.transform.rotation = Quaternion.LookRotation(-d.east + Vector3.down * .06f);
                KosRenderDiagnosis.Capture(cam, "clock-cc0-front", false, "Audit/Visuals/ClockVentProof");
                var pend = GameObject.Find("ClockPendulum__");
                Vector3 look = pend ? pend.transform.position : hour.position + Vector3.down * .8f;
                cam.transform.position = hour.position + d.east * 1.1f + Vector3.down * .5f;
                cam.transform.rotation = Quaternion.LookRotation((look - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "clock-pendulum-glass", false, "Audit/Visuals/ClockVentProof");
            }

            var cover = GameObject.Find("VentGrilleDoor");
            if (cover)
            {
                var b = KosVisualV2.BoundsOf(cover);
                cam.transform.position = d.P(-3.0f, 6.15f, 4.18f);
                cam.transform.rotation = Quaternion.LookRotation((b.center - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "vent-rusted-cover", false, "Audit/Visuals/ClockVentProof");
            }

            var crow = GameObject.Find("StorageCrowbar");
            if (crow)
            {
                var b = KosVisualV2.BoundsOf(crow);
                var crowSpot = new GameObject("ProofCrowSpot").AddComponent<Light>();
                crowSpot.type = LightType.Spot;
                crowSpot.intensity = 3;
                crowSpot.range = 6;
                crowSpot.spotAngle = 55;
                crowSpot.color = new Color(1f, .93f, .8f);
                crowSpot.transform.position = b.center + Vector3.up * 1.3f;
                crowSpot.transform.rotation = Quaternion.LookRotation((b.center - crowSpot.transform.position).normalized);
                cam.transform.position = b.center + Vector3.up * .95f;
                cam.transform.rotation = Quaternion.LookRotation(Vector3.down);
                KosRenderDiagnosis.Capture(cam, "crowbar-storage", false, "Audit/Visuals/ClockVentProof");
                Object.DestroyImmediate(crowSpot.gameObject);
            }

            Object.DestroyImmediate(cam.gameObject);
            Debug.Log("CLOCK_PHOTO_DONE");
        }

        public static void CaptureProofLit()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<LayarTerakhir.Prolog.AtmajaDirector>();
            Physics.SyncTransforms();
            var hour = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault(t => t && t.name.StartsWith("ClockHourBlade__"));
            var cam = new GameObject("ProofCam").AddComponent<Camera>();
            cam.nearClipPlane = .05f;
            cam.farClipPlane = 70;
            cam.backgroundColor = RenderSettings.fogColor;
            cam.clearFlags = CameraClearFlags.SolidColor;
            var spot = new GameObject("ProofSpot").AddComponent<Light>();
            spot.type = LightType.Spot;
            spot.intensity = 2.2f;
            spot.range = 8;
            spot.spotAngle = 40;
            spot.color = new Color(1f, .93f, .8f);
            if (hour)
            {
                spot.transform.position = hour.position + d.east * 1.6f + Vector3.up * .25f;
                spot.transform.rotation = Quaternion.LookRotation((hour.position - spot.transform.position).normalized);
                cam.transform.position = hour.position + d.east * .75f + Vector3.up * .02f;
                cam.transform.rotation = Quaternion.LookRotation((-d.east + Vector3.down * .02f).normalized);
                KosRenderDiagnosis.Capture(cam, "clock-dial-lit", false, "Audit/Visuals/ClockVentProof");
            }
            Object.DestroyImmediate(spot.gameObject);
            Object.DestroyImmediate(cam.gameObject);
            Debug.Log("CLOCK_PHOTO_LIT_DONE");
        }

        public static void CaptureProofGlassTest()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<LayarTerakhir.Prolog.AtmajaDirector>();
            Physics.SyncTransforms();
            var hour = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault(t => t && t.name.StartsWith("ClockHourBlade__"));
            // Do NOT save: this only diagnoses whether the imported glass hides dial+hands.
            var glass = new Material(Shader.Find("Universal Render Pipeline/Lit"));
            glass.SetFloat("_Surface", 1);
            glass.SetFloat("_SrcBlend", 5);
            glass.SetFloat("_DstBlend", 10);
            glass.SetFloat("_ZWrite", 0);
            glass.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            glass.SetColor("_BaseColor", new Color(1, 0, 0, .15f));
            glass.renderQueue = 3000;
            foreach (var r in GameObject.Find("GrandfatherClock").GetComponentsInChildren<Renderer>())
            {
                var mats = r.sharedMaterials;
                for (int i = 0; i < mats.Length; i++)
                    if (mats[i] && mats[i].name.IndexOf("glass", System.StringComparison.OrdinalIgnoreCase) >= 0)
                        mats[i] = glass;
                r.sharedMaterials = mats;
            }
            var cam = new GameObject("ProofCam").AddComponent<Camera>();
            cam.nearClipPlane = .05f;
            cam.farClipPlane = 70;
            cam.backgroundColor = RenderSettings.fogColor;
            cam.clearFlags = CameraClearFlags.SolidColor;
            var spot = new GameObject("ProofSpot").AddComponent<Light>();
            spot.type = LightType.Spot;
            spot.intensity = 2.2f;
            spot.range = 8;
            spot.spotAngle = 40;
            spot.color = new Color(1f, .93f, .8f);
            if (hour)
            {
                spot.transform.position = hour.position + d.east * 1.6f + Vector3.up * .25f;
                spot.transform.rotation = Quaternion.LookRotation((hour.position - spot.transform.position).normalized);
                cam.transform.position = hour.position + d.east * .75f + d.north * .45f + Vector3.up * .02f;
                cam.transform.rotation = Quaternion.LookRotation((hour.position - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "clock-glass-test", false, "Audit/Visuals/ClockVentProof");
            }
            Object.DestroyImmediate(spot.gameObject);
            Object.DestroyImmediate(cam.gameObject);
            Debug.Log("CLOCK_PHOTO_GLASS_DONE");
        }
    }
}
#endif
