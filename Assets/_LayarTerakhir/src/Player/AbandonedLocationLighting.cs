using System.Collections.Generic;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase2;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Phase5;
using LayarTerakhir.Smartphone;
using LayarTerakhir.Prolog;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Player
{
    // Presentation only. Puzzle controllers retain ownership of powered ritual
    // lights; no battery, visibility/detection, checkpoint or inventory changes.
    [DefaultExecutionOrder(900)]
    public sealed class AbandonedLocationLighting : MonoBehaviour
    {
        private readonly HashSet<Light> powered = new HashSet<Light>();
        private readonly List<Light> unpowered = new List<Light>();
        private readonly List<Light> epilogueLights = new List<Light>();
        private bool configured, dawn, outage;
        public int UnpoweredLightCount => unpowered.Count;
        public bool NightActive => configured && outage && !dawn;

        public static bool IsAbandoned(string scene) =>
            scene == "Prolog_RumahAtmaja" || scene == "Babak1_Rusunawa" ||
            scene == "Babak2_RSUD" || scene == "Babak3_PabrikGula" ||
            scene == "Babak4_KarangSetra" || scene == "Babak5_PasarSetan";

        public static void Install(GameObject player)
        {
            if (player && !player.GetComponent<AbandonedLocationLighting>())
                player.AddComponent<AbandonedLocationLighting>();
        }

        private void Start() => Configure();
        private void Keep(Light light) { if (light) powered.Add(light); }
        private void Keep(IEnumerable<Light> lights) { if (lights != null) foreach (var light in lights) Keep(light); }

        public void Configure()
        {
            if (configured) return;
            configured = true;
            outage=IsAbandoned(gameObject.scene.name);
            var p1 = FindFirstObjectByType<Phase1Director>();
            var p2 = FindFirstObjectByType<Phase2Director>();
            var p3 = FindFirstObjectByType<Phase3Director>();
            var p4 = FindFirstObjectByType<Phase4Director>();
            var p5 = FindFirstObjectByType<Phase5Director>();
            var phone=FindFirstObjectByType<SmartphoneController>();
            if(phone)Keep(phone.phoneFlashlight);
            if (p1) Keep(p1.world.halogenLights);
            if (p2) Keep(p2.world.ritualLights);
            if (p3) { Keep(p3.world.ritualLights); Keep(p3.world.accessLight); }
            if (p4)
            {
                Keep(p4.world.entityLight);
                foreach (var torch in p4.world.torches) if (torch) Keep(torch.practical);
            }
            if (p5) { Keep(p5.world.generatorLight); Keep(p5.world.droppedLight); }
            foreach (var light in FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (light.gameObject.scene != gameObject.scene || light.transform.IsChildOf(transform)) continue;
                if(phone&&phone.phoneFlashlight&&light.transform.IsChildOf(phone.phoneFlashlight.transform))continue;
                if (powered.Contains(light)) continue;
                if(light.name=="RakaMotorHeadlight"||light.cullingMask==(1<<AtmajaHandycamPlayback.RecordingLayer))continue;
                // Imported street/fixture point lights used to request dozens
                // of cubemap shadows, starving the phone's moving shadow map.
                if(light.type!=LightType.Directional&&
                    !(gameObject.scene.name=="Prolog_KamarKos"&&light.name.Contains("WeakTungsten")))
                    light.shadows=LightShadows.None;
                if(!outage)continue;
                // These are a later inhabited studio / authored ending sets,
                // not electrical fixtures in the abandoned market. Inactive
                // set roots already prevent them illuminating exploration.
                if(p5&&p5.world.epilogueRoot&&light.transform.IsChildOf(p5.world.epilogueRoot.transform))
                {if(light.enabled)epilogueLights.Add(light);continue;}
                if (light.type == LightType.Directional)
                {
                    // Moonlight is faint, shadowed and does not replace the phone indoors.
                    light.intensity = gameObject.scene.name == "Babak4_KarangSetra" ? .055f : .035f;
                    continue;
                }
                string name = light.name.ToLowerInvariant();
                if (name.Contains("teplok") || name.Contains("candle") || name.Contains("flame") ||
                    name.Contains("ember") || name.StartsWith("ward_lantern"))
                {
                    light.intensity = Mathf.Min(light.intensity, .16f);
                    light.range = Mathf.Min(light.range, 3);
                    continue;
                }
                // Actor-only rim lights are not room illumination.
                uint renderingLayers=light.GetUniversalAdditionalLightData().renderingLayers;
                if((renderingLayers&1u)==0&&(renderingLayers&((1u<<7)|(1u<<8)))!=0)continue;
                if ((light.cullingMask & ~(1 << ShoulderCameraRig.AvatarLayer | 1 << ShoulderCameraRig.PhoneGripLayer)) == 0)
                    continue;
                unpowered.Add(light);
            }
            if(outage)ApplyNightAmbient();
            EnforceOutage();
        }

        private void ApplyNightAmbient()
        {
            bool outdoors = gameObject.scene.name == "Babak4_KarangSetra";
            Color ambient = outdoors ? new Color(.012f, .016f, .022f) : new Color(.007f, .009f, .013f);
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = ambient;
            RenderSettings.ambientIntensity = 1;
            var probe = new SphericalHarmonicsL2(); probe.AddAmbientLight(ambient);
            RenderSettings.ambientProbe = probe;
            RenderSettings.reflectionIntensity = .12f;
        }

        public void EnterDawn()
        {
            dawn = true;
            foreach (var light in epilogueLights) if (light) light.enabled = true;
        }

        public void EnforceOutage()
        {
            // Chapter startup can finish asynchronously after our Start and
            // restore its old ambient probe. Retain the night presentation
            // until the existing ending explicitly enters dawn.
            if(NightActive&&(RenderSettings.ambientMode!=AmbientMode.Flat||
                RenderSettings.ambientLight.maxColorComponent>.03f||RenderSettings.reflectionIntensity>.121f))
                ApplyNightAmbient();
            // Existing flicker/budget controllers may enable an old fluorescent
            // again. Reassert only unpowered fixtures, never gameplay lights.
            foreach (var light in unpowered) if (light && (!dawn || !epilogueLights.Contains(light))) light.enabled = false;
        }

        private void LateUpdate() { if (configured) EnforceOutage(); }
    }
}
