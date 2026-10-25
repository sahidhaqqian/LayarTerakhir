using UnityEngine;
using UnityEngine.Rendering;
namespace LayarTerakhir.Phase4
{
    // Explicit ambient SH avoids inheriting the daytime Kos sky or a zero probe
    // when an unbaked additive authoring scene becomes a standalone player scene.
    public sealed class Phase4Lighting : MonoBehaviour
    {
        public Camera playerCamera;
        public Color ambient = new Color(.36f, .42f, .49f);
        public Color night = new Color(.12f, .17f, .22f);
        private void OnEnable() => Apply();
        private void Start() => Apply();
        private void Apply()
        {
            RenderSettings.skybox = null; RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = ambient; RenderSettings.ambientIntensity = 1;
            var sh = new SphericalHarmonicsL2(); sh.AddAmbientLight(ambient); RenderSettings.ambientProbe = sh;
            RenderSettings.fog = true; RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = .012f; RenderSettings.fogColor = night;
            if (playerCamera) { playerCamera.clearFlags = CameraClearFlags.SolidColor; playerCamera.backgroundColor = night; }
        }
    }
}
