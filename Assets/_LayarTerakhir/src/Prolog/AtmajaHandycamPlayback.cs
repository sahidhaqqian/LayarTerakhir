using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace LayarTerakhir.Prolog
{
    // An original, in-engine recording: the market behind the mirror, reaching hands,
    // and Damar's camera being dragged in. No external video/service or second phone.
    public sealed class AtmajaHandycamPlayback : MonoBehaviour
    {
        public const int RecordingLayer = 28;
        public const string DamarLine = "Kacanya... kacanya bukan mantulin ruangan ini, Ka! Di dalem kaca itu ada pasar malam... mereka narik kaki gue—RAKA TOLONGIN GUE!";
        public Camera recordingCamera;
        public GameObject recordingSet;
        public Transform cameraStart, cameraEnd;
        public Transform[] reachingHands = new Transform[0];
        public bool IsPlaying { get; private set; }
        public float Progress { get; private set; }
        private RenderTexture recording;
        private GameObject overlay;
        private Text timecode, caption;
        private Vector3[] handOrigins;
        public IEnumerator Play()
        {
            IsPlaying = true; Progress = 0;
            var hud = HudController.Instance;
            float duration = Mathf.Max(15, hud.PlayPriorityDialogue("Damar (rekaman)", DamarLine, 15));
            hud.subtitleText.enabled = false;
            recordingSet.SetActive(true);
            handOrigins = new Vector3[reachingHands.Length];
            for (int i = 0; i < reachingHands.Length; i++) handOrigins[i] = reachingHands[i].localPosition;
            BuildOverlay();
            recordingCamera.transform.SetPositionAndRotation(cameraStart.position, cameraStart.rotation);
            recordingCamera.enabled = SystemInfo.graphicsDeviceType != GraphicsDeviceType.Null;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                Progress = Mathf.Clamp01(t / duration);
                float pull = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(.57f, .95f, Progress));
                float shake = GameShell.ReducedGlitch ? .003f : Mathf.Lerp(.008f, .055f, pull);
                recordingCamera.transform.position = Vector3.Lerp(cameraStart.position, cameraEnd.position, pull) +
                    cameraStart.right * Mathf.Sin(t * 19) * shake + Vector3.up * Mathf.Sin(t * 13) * shake;
                recordingCamera.transform.rotation = Quaternion.Slerp(cameraStart.rotation, cameraEnd.rotation, pull) *
                    Quaternion.Euler(Mathf.Sin(t * 8) * shake * 20, Mathf.Sin(t * 11) * shake * 15, pull * -13);
                for (int i = 0; i < reachingHands.Length; i++)
                {
                    float reach = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(.35f + i % 3 * .055f, .75f + i % 2 * .06f, Progress));
                    reachingHands[i].localPosition = handOrigins[i] + Vector3.back * reach * 1.3f;
                    reachingHands[i].localRotation = Quaternion.Euler(Mathf.Sin(t * 2.8f + i) * 5, 0, Mathf.Sin(t * 3 + i) * 9);
                }
                timecode.text = $"21:37:{6 + Mathf.FloorToInt(t):00}   •   17 / MTS";
                caption.text = Progress < .22f ? "Kacanya... kacanya bukan mantulin ruangan ini, Ka!" : Progress < .54f ? "Di dalem kaca itu ada pasar malam..." : Progress < .79f ? "Mereka narik kaki gue—" : "RAKA TOLONGIN GUE!";
                yield return null;
            }
            StopPlayback();
        }
        private void BuildOverlay()
        {
            if (overlay) Destroy(overlay);
            overlay = new GameObject("HandycamRecording", typeof(Canvas), typeof(CanvasScaler));
            var canvas = overlay.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay; canvas.sortingOrder = 110;
            var scale = overlay.GetComponent<CanvasScaler>(); scale.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scale.referenceResolution = new Vector2(1600, 900);
            UiWidgets.Panel("RecordingSurround", overlay.transform, Vector2.zero, Vector2.one, Color.black);
            var frame = UiWidgets.Rect("RecordedImage", overlay.transform, new Vector2(.08f, .17f), new Vector2(.92f, .91f));
            var image = frame.AddComponent<RawImage>(); image.raycastTarget = false;
            recording = new RenderTexture(640, 360, 16, RenderTextureFormat.ARGB32) { name = "DamarHandycamRecording", antiAliasing = 1 };
            if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Null) recording.Create();
            recordingCamera.targetTexture = recording; image.texture = recording;
            var aspect = frame.AddComponent<AspectRatioFitter>(); aspect.aspectMode = AspectRatioFitter.AspectMode.FitInParent; aspect.aspectRatio = 16f / 9;
            for (int i = 0; i < 36; i++)
            {
                var line = UiWidgets.Panel("TapeScanline", frame.transform, new Vector2(0, i / 36f), new Vector2(1, i / 36f + .0018f), new Color(0, 0, 0, .12f));
                line.GetComponent<Image>().raycastTarget = false;
            }
            UiWidgets.Text("RecordingLabel", overlay.transform, "REKAMAN TERAKHIR  /  KAMERA DAMAR", new Vector2(.08f, .925f), new Vector2(.92f, .98f), 22);
            var rec = UiWidgets.Text("RecordIndicator", frame.transform, "● REC", new Vector2(.025f, .88f), new Vector2(.30f, .97f), 26); rec.color = new Color(.85f, .12f, .10f);
            timecode = UiWidgets.Text("Timecode", frame.transform, "", new Vector2(.50f, .88f), new Vector2(.97f, .97f), 24, TextAnchor.MiddleRight);
            caption = UiWidgets.Text("DamarCaption", overlay.transform, "", new Vector2(.10f, .04f), new Vector2(.9f, .145f), 30, TextAnchor.MiddleCenter);
        }
        public void StopPlayback()
        {
            IsPlaying = false;
            if (recordingCamera) { recordingCamera.enabled = false; recordingCamera.targetTexture = null; }
            if (recordingSet) recordingSet.SetActive(false);
            if (overlay) { overlay.SetActive(false); Destroy(overlay); }
            if (recording) { recording.Release(); Destroy(recording); recording = null; }
            if (HudController.Instance) HudController.Instance.subtitleText.enabled = true;
            if (handOrigins != null) for (int i = 0; i < reachingHands.Length; i++) if(reachingHands[i]) reachingHands[i].localPosition = handOrigins[i];
        }
        private void OnDestroy() { StopPlayback(); }
    }
}
