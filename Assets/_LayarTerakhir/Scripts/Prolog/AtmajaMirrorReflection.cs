using LayarTerakhir.Player;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Prolog
{
    // Only the bathroom mirror camera sees layer 27. The ordinary world never sees its apparition.
    public sealed class AtmajaMirrorReflection : MonoBehaviour
    {
        public const int ReflectionLayer = 27;
        public AtmajaDirector director;
        public Transform mirrorPlane;
        public Renderer surface;
        public Material reflectionMaterial;
        private Camera reflectionCamera;
        private RenderTexture reflection;
        private Material instance;
        private void Start()
        {
            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null) return;
            reflection = new RenderTexture(256, 384, 16) { name = "AtmajaBathroomReflection", antiAliasing = 1 }; reflection.Create();
            instance = new Material(reflectionMaterial); instance.SetTexture("_BaseMap", reflection); instance.SetTextureScale("_BaseMap", new Vector2(-1, 1)); instance.SetTextureOffset("_BaseMap", new Vector2(1, 0)); surface.sharedMaterial = instance;
            var go = new GameObject("BathroomMirrorCamera", typeof(Camera)); go.transform.SetParent(transform, false);
            reflectionCamera = go.GetComponent<Camera>(); reflectionCamera.enabled = false; reflectionCamera.targetTexture = reflection;
            reflectionCamera.clearFlags = CameraClearFlags.SolidColor; reflectionCamera.backgroundColor = new Color(.015f, .022f, .027f); reflectionCamera.nearClipPlane = .12f; reflectionCamera.farClipPlane = 12;
            reflectionCamera.fieldOfView = 65; reflectionCamera.aspect = 2f / 3;
            reflectionCamera.cullingMask = ~(1 << AtmajaPhoneView.ARLayer | 1 << AtmajaHandycamPlayback.RecordingLayer);
            reflectionCamera.GetUniversalAdditionalCameraData().renderPostProcessing = false; reflectionCamera.GetUniversalAdditionalCameraData().renderShadows = false;
        }
        private void LateUpdate()
        {
            if (!reflectionCamera || !director || !director.player) return;
            var source = director.player.GetComponent<InteractionInteractor>().playerCamera;
            bool active = director.Stage >= AtmajaStage.StairLanding && director.Stage < AtmajaStage.BasementEntry &&
                Vector3.Distance(director.player.transform.position, mirrorPlane.position) < 3.2f && Time.frameCount % 2 == 0;
            reflectionCamera.enabled = active;
            if (!active) return;
            Vector3 normal = mirrorPlane.forward, eye = source.transform.position;
            reflectionCamera.transform.position = eye - 2 * Vector3.Dot(eye - mirrorPlane.position, normal) * normal;
            Vector3 forward = Vector3.Reflect(source.transform.forward, normal), up = Vector3.Reflect(source.transform.up, normal);
            reflectionCamera.transform.rotation = Quaternion.LookRotation(forward, up);
            // Keep the mirror from recursively drawing its own surface.
            surface.gameObject.layer = 2; reflectionCamera.cullingMask &= ~(1 << 2);
        }
        private void OnDestroy()
        {
            if (reflectionCamera) Destroy(reflectionCamera.gameObject);
            if (reflection) { reflection.Release(); Destroy(reflection); }
            if (instance) Destroy(instance);
        }
    }
}
