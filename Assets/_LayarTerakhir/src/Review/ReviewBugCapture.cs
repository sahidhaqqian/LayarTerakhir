#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using UnityEngine;

namespace LayarTerakhir.Review
{
    // F10 bug reports: rendered-view PNG plus a JSON sidecar with scene, phase,
    // camera and selection data. The review overlay is hidden during capture.
    public sealed class ReviewBugCapture : MonoBehaviour
    {
        [Serializable]
        public sealed class VectorDto
        {
            public float x, y, z;
            public VectorDto() { }
            public VectorDto(Vector3 v) { x = v.x; y = v.y; z = v.z; }
        }

        [Serializable]
        public sealed class SelectionDto
        {
            public string name;
            public string hierarchyPath;
            public string scene;
            public VectorDto position;
            public VectorDto rotationEuler;
            public VectorDto scale;
            public string renderer;
            public string collider;
        }

        [Serializable]
        public sealed class BugReportMetadata
        {
            public string timestampUtc;
            public string scene;
            public string phase;
            public int chapterId;
            public string checkpointId;
            public VectorDto cameraPosition;
            public VectorDto cameraRotationEuler;
            public SelectionDto selection;
            public string description;
            public string resolution;
            public string platform;
            public bool flyActive;
            public bool worldFrozen;
        }

        public bool Busy { get; private set; }
        public string LastPngPath { get; private set; }
        public string LastJsonPath { get; private set; }

        public static string DefaultDirectory =>
            Path.Combine(Application.persistentDataPath, "BugReports");

        public static string BuildBaseName(DateTime utc) =>
            "Bug-" + utc.ToString("yyyyMMdd-HHmmss-fff");

        public static BugReportMetadata BuildMetadata(string scene, CampaignData data,
            Vector3 camPos, Vector3 camRot, GameObject selected, string description,
            string resolution, bool flyActive, bool worldFrozen)
        {
            var meta = new BugReportMetadata
            {
                timestampUtc = DateTime.UtcNow.ToString("o"),
                scene = scene ?? "?",
                phase = ReviewHierarchy.PhaseLabel(data),
                chapterId = data != null ? data.chapter_id : -1,
                checkpointId = data != null ? data.checkpoint_id : "?",
                cameraPosition = new VectorDto(camPos),
                cameraRotationEuler = new VectorDto(camRot),
                description = description ?? "",
                resolution = resolution ?? "",
                platform = Application.platform.ToString(),
                flyActive = flyActive,
                worldFrozen = worldFrozen,
            };
            if (selected)
            {
                meta.selection = new SelectionDto
                {
                    name = selected.name,
                    hierarchyPath = ReviewHierarchy.BuildPath(selected.transform),
                    scene = selected.scene.name,
                    position = new VectorDto(selected.transform.position),
                    rotationEuler = new VectorDto(selected.transform.rotation.eulerAngles),
                    scale = new VectorDto(selected.transform.localScale),
                    renderer = ReviewHierarchy.DescribeRenderer(selected),
                    collider = ReviewHierarchy.DescribeCollider(selected),
                };
            }
            return meta;
        }

        // Pure file seam used by tests: writes PNG bytes plus pretty JSON sidecar.
        public static void WriteReport(byte[] pngBytes, BugReportMetadata meta,
            string directory, string baseName, out string pngPath, out string jsonPath)
        {
            if (pngBytes == null || pngBytes.Length == 0)
                throw new ArgumentException("PNG bytes missing", nameof(pngBytes));
            if (meta == null) throw new ArgumentNullException(nameof(meta));
            if (string.IsNullOrEmpty(directory)) throw new ArgumentException("directory missing", nameof(directory));
            if (string.IsNullOrEmpty(baseName)) throw new ArgumentException("baseName missing", nameof(baseName));
            Directory.CreateDirectory(directory);
            pngPath = Path.Combine(directory, baseName + ".png");
            jsonPath = Path.Combine(directory, baseName + ".json");
            File.WriteAllBytes(pngPath, pngBytes);
            File.WriteAllText(jsonPath, JsonUtility.ToJson(meta, true));
        }

        public void Capture(string description)
        {
            if (Busy) return;
            StartCoroutine(CaptureRoutine(description ?? ""));
        }

        private IEnumerator CaptureRoutine(string description)
        {
            Busy = true;
            var mode = GetComponent<DeveloperReviewMode>();
            try
            {
                var cam = ResolveCamera();
                if (!cam)
                {
                    if (mode) mode.Toast("Bug capture gagal: tidak ada kamera aktif.");
                    yield break;
                }
                if (mode) mode.SetOverlayVisible(false);
                yield return new WaitForEndOfFrame();

                int w = Mathf.Max(2, Screen.width);
                int h = Mathf.Max(2, Screen.height);
                var rt = RenderTexture.GetTemporary(w, h, 24, RenderTextureFormat.ARGB32);
                var prevTarget = cam.targetTexture;
                var prevAllowHDR = cam.allowHDR;
                Texture2D tex = null;
                try
                {
                    cam.targetTexture = rt;
                    cam.allowHDR = false;
                    cam.Render();
                    RenderTexture.active = rt;
                    tex = new Texture2D(w, h, TextureFormat.RGB24, false);
                    tex.ReadPixels(new Rect(0, 0, w, h), 0, 0, false);
                    tex.Apply(false);
                }
                finally
                {
                    cam.targetTexture = prevTarget;
                    cam.allowHDR = prevAllowHDR;
                    RenderTexture.active = null;
                    RenderTexture.ReleaseTemporary(rt);
                }

                byte[] png = tex.EncodeToPNG();
                Destroy(tex);
                var campaign = CampaignState.Instance;
                var inspector = GetComponent<ReviewObjectInspector>();
                var fly = GetComponent<ReviewFlyCamera>();
                var freeze = GetComponent<ReviewWorldFreeze>();
                var meta = BuildMetadata(ReviewHierarchy.ActiveSceneName(),
                    campaign ? campaign.Data : null,
                    cam.transform.position, cam.transform.rotation.eulerAngles,
                    inspector ? inspector.Selected : null, description,
                    w + "x" + h, fly && fly.Active, freeze && freeze.Frozen);
                WriteReport(png, meta, DefaultDirectory, BuildBaseName(DateTime.UtcNow),
                    out string pngPath, out string jsonPath);
                LastPngPath = pngPath;
                LastJsonPath = jsonPath;
                if (mode) mode.Toast("Laporan tersimpan: " + Path.GetFileName(pngPath));
                Debug.Log("[ReviewMode] Bug report saved: " + pngPath + " + " + jsonPath);
            }
            finally
            {
                if (mode) mode.SetOverlayVisible(true);
                Busy = false;
            }
        }

        private Camera ResolveCamera()
        {
            var fly = GetComponent<ReviewFlyCamera>();
            if (fly && fly.Active && fly.Eye) return fly.Eye;
            var player = FindFirstObjectByType<FirstPersonController>();
            if (player)
            {
                var i = player.GetComponent<InteractionInteractor>();
                if (i && i.playerCamera) return i.playerCamera;
            }
            return Camera.main;
        }

        public static void OpenReportsFolder()
        {
            string dir = DefaultDirectory;
            try { Directory.CreateDirectory(dir); } catch { return; }
#if UNITY_EDITOR
            UnityEditor.EditorUtility.RevealInFinder(dir);
#else
            try { Application.OpenURL("file://" + dir); } catch { }
#endif
        }
    }
}
#endif
