#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    // One-shot visual proof for the designer: the small gold blink above
    // must-take items. EditMode rigs don't run LateUpdate, so each blink is
    // pinned at full brightness facing the proof camera before rendering.
    // Run headful under xvfb (no -nographics).
    public static class PickupBlinkPhoto
    {
        public static void CaptureProof()
        {
            // Kos: the red ibu-kos letter under the room door.
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            Physics.SyncTransforms();
            var surat = GameObject.Find("Interactable_SuratIbuKos");
            if (surat)
            {
                var blink = PickupBlink.EnsureFor(surat);
                var cam = new GameObject("BlinkProofCam").AddControllerCamera();
                var b = BoundsOf(surat);
                cam.transform.position = b.center + new Vector3(.35f, .75f, .55f);
                cam.transform.rotation = Quaternion.LookRotation((b.center - cam.transform.position).normalized);
                blink.ClearPulse();
                KosRenderDiagnosis.Capture(cam, "kos-surat-blink-off", false, "Audit/Visuals/PickupBlinkProof");
                blink.ApplyPeakForPhoto();
                KosRenderDiagnosis.Capture(cam, "kos-surat-blink", false, "Audit/Visuals/PickupBlinkProof");
                Object.DestroyImmediate(cam.gameObject);
            }
            else Debug.Log("BLINK_PHOTO kos surat MISSING");

            // Atmaja: the crowbar hidden under the lower stair flight.
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            Physics.SyncTransforms();
            var crow = GameObject.Find("StorageCrowbar");
            if (crow)
            {
                var blink = PickupBlink.EnsureFor(crow, "ITEM_P0_CROWBAR");
                var d = Object.FindFirstObjectByType<AtmajaDirector>();
                var cam = new GameObject("BlinkProofCam").AddComponent<Camera>();
                cam.nearClipPlane = .05f; cam.farClipPlane = 70;
                cam.backgroundColor = RenderSettings.fogColor;
                cam.clearFlags = CameraClearFlags.SolidColor;
                var b = BoundsOf(crow);
                cam.transform.position = d.P(.55f, 7.1f, 1.6f);
                cam.transform.rotation = Quaternion.LookRotation((b.center - cam.transform.position).normalized);
                blink.ClearPulse();
                KosRenderDiagnosis.Capture(cam, "atmaja-crowbar-blink-off", false, "Audit/Visuals/PickupBlinkProof");
                blink.ApplyPeakForPhoto();
                KosRenderDiagnosis.Capture(cam, "atmaja-crowbar-blink", false, "Audit/Visuals/PickupBlinkProof");
                Object.DestroyImmediate(cam.gameObject);
            }
            else Debug.Log("BLINK_PHOTO crowbar MISSING");
            Debug.Log("BLINK_PHOTO_DONE");
        }

        private static Bounds BoundsOf(GameObject go)
        {
            var rs = go.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled).ToArray();
            var b = rs[0].bounds;
            foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds);
            return b;
        }

        private static Camera AddControllerCamera(this GameObject go)
        {
            var cam = go.AddComponent<Camera>();
            cam.nearClipPlane = .05f; cam.farClipPlane = 70;
            cam.backgroundColor = RenderSettings.fogColor;
            cam.clearFlags = CameraClearFlags.SolidColor;
            return cam;
        }
    }
}
#endif
