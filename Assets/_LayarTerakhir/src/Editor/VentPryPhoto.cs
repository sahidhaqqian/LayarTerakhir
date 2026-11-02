#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    // One-shot visual proof for the designer: the prized vent. The grille
    // is ripped off for good (persisted flag) — the still shows the open
    // dark duct in its flat trim, with no cover blocking the hole.
    // Run headful under xvfb (no -nographics). Never saves the scene.
    public static class VentPryPhoto
    {
        public static void CaptureProof()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            Physics.SyncTransforms();
            var d = Object.FindFirstObjectByType<LayarTerakhir.Prolog.AtmajaDirector>();
            var cover = GameObject.Find("VentGrilleDoor");
            if (!cover) { Debug.Log("VENT_PHOTO cover MISSING"); return; }
            bool wasActive = cover.activeSelf;
            cover.SetActive(false);
            var cam = new GameObject("VentPryCam").AddComponent<Camera>();
            cam.nearClipPlane = .05f; cam.farClipPlane = 70;
            cam.backgroundColor = RenderSettings.fogColor;
            cam.clearFlags = CameraClearFlags.SolidColor;
            var spot = new GameObject("VentPrySpot").AddComponent<Light>();
            spot.type = LightType.Spot; spot.intensity = 4f; spot.range = 9;
            spot.spotAngle = 60; spot.color = new Color(1f, .93f, .8f);
            var hole = d.P(-3.0f, 5.24f, 4.2f);
            cam.transform.position = d.P(-3.0f, 6.5f, 4.25f);
            cam.transform.rotation = Quaternion.LookRotation((hole - cam.transform.position).normalized);
            spot.transform.position = d.P(-3.0f, 6.3f, 4.6f);
            spot.transform.rotation = Quaternion.LookRotation((hole - spot.transform.position).normalized);
            KosRenderDiagnosis.Capture(cam, "vent-prized-open", false, "Audit/Visuals/VentPryProof");
            cover.SetActive(wasActive);
            Object.DestroyImmediate(cam.gameObject);
            Object.DestroyImmediate(spot.gameObject);
            Debug.Log("VENT_PHOTO_DONE");
        }
    }
}
#endif
