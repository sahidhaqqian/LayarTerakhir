#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    // One-shot visual proof for the designer: Raka's wardrobe facing + the
    // wall calendar, straight from the saved Kos scene. Run headful under
    // xvfb (no -nographics). Saves under Audit/Visuals/KosWardrobeProof.
    public static class KosPhoto
    {
        public static void CaptureProof()
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            Physics.SyncTransforms();
            var cam = new GameObject("ProofCam").AddComponent<Camera>();
            cam.nearClipPlane = .05f;
            cam.farClipPlane = 70;
            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;
            var spot = new GameObject("ProofSpot").AddComponent<Light>();
            spot.type = LightType.Spot;
            spot.intensity = 6f;
            spot.range = 9;
            spot.spotAngle = 65;
            spot.color = new Color(1f, .93f, .8f);

            var wardrobe = GameObject.Find("MeshyBedroomWardrobe");
            if (wardrobe)
            {
                var b = KosVisualV2.BoundsOf(wardrobe);
                spot.transform.position = b.center + Vector3.up * 1.6f + Vector3.right * .6f;
                spot.transform.rotation = Quaternion.LookRotation((b.center - spot.transform.position).normalized);
                cam.transform.position = new Vector3(b.center.x - 2.2f, 1.6f, b.center.z - .6f);
                cam.transform.rotation = Quaternion.LookRotation((b.center + Vector3.up * .1f - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "kos-wardrobe-facing", false, "Audit/Visuals/KosWardrobeProof");
                Debug.Log("KOS_PHOTO wardrobe yaw=" + wardrobe.transform.rotation.eulerAngles.y.ToString("F1"));
            }

            var cal = GameObject.Find("CalendarAugust");
            if (cal)
            {
                var b = KosVisualV2.BoundsOf(cal);
                spot.transform.position = b.center + Vector3.up * .8f + Vector3.forward * .8f;
                spot.transform.rotation = Quaternion.LookRotation((b.center - spot.transform.position).normalized);
                cam.transform.position = b.center + Vector3.forward * 1.1f + Vector3.up * .15f;
                cam.transform.rotation = Quaternion.LookRotation((b.center - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "kos-wall-calendar", false, "Audit/Visuals/KosWardrobeProof");
            }

            // Designer bug proof: monitor back on the desk + chair seated south of it.
            var desk = GameObject.Find("wooden_table_02");
            if (desk)
            {
                var b = KosVisualV2.BoundsOf(desk);
                spot.transform.position = b.center + Vector3.up * 1.8f + Vector3.forward * -.8f;
                spot.transform.rotation = Quaternion.LookRotation((b.center - spot.transform.position).normalized);
                cam.transform.position = new Vector3(b.center.x - .2f, 1.55f, b.center.z - 1.9f);
                cam.transform.rotation = Quaternion.LookRotation((b.center + Vector3.up * .1f - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "kos-desk-monitor", false, "Audit/Visuals/KosWardrobeProof");
                Debug.Log("KOS_PHOTO desk shot done");
            }

            // Designer bug proof: security bars + sill back on the east window, nothing mid-room.
            var glass = GameObject.Find("DarkNightBeyondWindow");
            if (glass)
            {
                var b = KosVisualV2.BoundsOf(glass);
                spot.transform.position = b.center + Vector3.up * 1.2f + Vector3.right * -1.2f;
                spot.transform.rotation = Quaternion.LookRotation((b.center - spot.transform.position).normalized);
                cam.transform.position = new Vector3(b.center.x - 1.9f, 1.6f, b.center.z + .3f);
                cam.transform.rotation = Quaternion.LookRotation((b.center - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "kos-window-bars", false, "Audit/Visuals/KosWardrobeProof");
                Debug.Log("KOS_PHOTO window shot done");
            }

            Object.DestroyImmediate(spot.gameObject);
            Object.DestroyImmediate(cam.gameObject);
            Debug.Log("KOS_PHOTO_DONE");
        }
    }
}
#endif
