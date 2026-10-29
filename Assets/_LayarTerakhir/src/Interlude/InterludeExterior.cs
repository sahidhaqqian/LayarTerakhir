using LayarTerakhir.Prolog;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Interlude
{
    // Permanent kos scenery is shared by night and day. Only lighting,
    // residents and moving traffic change with the campaign time.
    public static class InterludeExterior
    {
        public static void EnsureNeighborhood(KosCorridorDirector corridor)
        {
            var kampung = GameObject.Find("KampungMelati");
            var alley = kampung ? kampung.transform.Find("InterludeAlley") : null;
            // Kos scene revisions can rebuild the base neighborhood. Keep
            // its permanent extension available independently of that pass.
            if (kampung && !alley)
            {
                var prefab = Resources.Load<GameObject>("MelatiPermanentStreet");
                if (prefab)
                {
                    alley = Object.Instantiate(prefab, kampung.transform, false).transform;
                    alley.name = "InterludeAlley";
                }
            }
            if (alley) alley.gameObject.SetActive(true);
            DressYard(corridor);
        }

        private static void DressYard(KosCorridorDirector corridor)
        {
            if (!corridor || !corridor.buildingRoot || corridor.buildingRoot.Find("KosYardLife")) return;
            var root = new GameObject("KosYardLife").transform;
            root.SetParent(corridor.buildingRoot, false);
            var poleMat = new Material(Shader.Find("Universal Render Pipeline/Lit")) { color = new Color(.30f, .24f, .16f) };
            foreach (float z in new[] { -6.5f, -1.5f })
            {
                var pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                pole.name = "InterludeClothesPole";
                pole.transform.SetParent(root, false);
                pole.transform.localPosition = new Vector3(3.8f, .9f, z);
                pole.transform.localScale = new Vector3(.07f, .9f, .07f);
                pole.GetComponent<Renderer>().sharedMaterial = poleMat;
            }
            var line = new GameObject("InterludeClothesLine").AddComponent<LineRenderer>();
            line.transform.SetParent(root, false);
            line.transform.localPosition = Vector3.zero;
            line.startWidth = line.endWidth = .015f;
            line.positionCount = 2;
            line.useWorldSpace = false;
            line.SetPositions(new[] { new Vector3(3.8f, 1.75f, -6.5f), new Vector3(3.8f, 1.75f, -1.5f) });
            line.sharedMaterial = poleMat;
            Color[] cloths = { new Color(.75f, .78f, .80f), new Color(.55f, .30f, .28f), new Color(.30f, .45f, .55f), new Color(.80f, .72f, .55f) };
            for (int i = 0; i < cloths.Length; i++)
            {
                var cloth = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cloth.name = "InterludeLaundry";
                cloth.transform.SetParent(root, false);
                cloth.transform.localPosition = new Vector3(3.8f, 1.45f, -5.6f + i * 1.1f);
                cloth.transform.localScale = new Vector3(.04f, .55f, .5f);
                var mat = new Material(Shader.Find("Universal Render Pipeline/Lit")) { color = cloths[i] };
                cloth.GetComponent<Renderer>().sharedMaterial = mat;
                DropCollider(cloth);
            }
            var sandalMat = new Material(Shader.Find("Universal Render Pipeline/Lit")) { color = new Color(.12f, .10f, .09f) };
            foreach (var spot in new[] { new Vector3(-1.0f, .03f, 2.1f), new Vector3(1.0f, .03f, 8.0f), new Vector3(-1.0f, .03f, 10.9f) })
                for (int s = -1; s <= 1; s += 2)
                {
                    var sandal = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    sandal.name = "InterludeSandals";
                    sandal.transform.SetParent(root, false);
                    sandal.transform.localPosition = spot + new Vector3(s * .09f, 0, 0);
                    sandal.transform.localScale = new Vector3(.11f, .05f, .28f);
                    sandal.GetComponent<Renderer>().sharedMaterial = sandalMat;
                    DropCollider(sandal);
                }
            var galon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            galon.name = "InterludeGalon";
            galon.transform.SetParent(root, false);
            galon.transform.localPosition = new Vector3(-2.2f, .3f, 2.6f);
            galon.transform.localScale = new Vector3(.32f, .3f, .32f);
            var aqua = new Material(Shader.Find("Universal Render Pipeline/Lit")) { color = new Color(.45f, .70f, .85f, .75f) };
            galon.GetComponent<Renderer>().sharedMaterial = aqua;
            DropCollider(galon);
            var broom = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            broom.name = "InterludeBroom";
            broom.transform.SetParent(root, false);
            broom.transform.localPosition = new Vector3(-2.3f, .7f, 5.6f);
            broom.transform.localScale = new Vector3(.04f, .7f, .04f);
            broom.GetComponent<Renderer>().sharedMaterial = poleMat;
            DropCollider(broom);
        }

        public static void ApplyDay(Camera eye)
        {
            SetExposure(.45f);
            ApplyAtmosphere(eye, false);
            foreach (var light in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
            {
                if (light.type == LightType.Directional)
                {
                    light.enabled = true;
                    RenderSettings.sun = light;
                    light.color = new Color(1f, .96f, .88f);
                    light.intensity = 1.15f;
                    light.transform.rotation = Quaternion.Euler(50, -30, 0);
                }
                else light.enabled = light.name == "WartegCaseLight";
            }
        }

        public static void ApplyDusk(Camera eye)
        {
            SetExposure(.45f);
            ApplyAtmosphere(eye, true);
            foreach (var light in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
            {
                if (light.type == LightType.Directional)
                {
                    light.enabled = true;
                    RenderSettings.sun = light;
                    light.color = new Color(.95f, .45f, .30f);
                    light.intensity = .5f;
                    light.transform.rotation = Quaternion.Euler(12, -70, 0);
                }
                else light.enabled = light.name.Contains("StreetLight") || light.name.Contains("WarungWarmLight") || light.name == "WartegCaseLight";
            }
        }

        // The room keeps its practical lamps, but shares the exterior's time
        // of day. Its scene has no authored sun, so provide window daylight.
        public static void ApplyRoom(Camera eye, bool dusk)
        {
            ApplyAtmosphere(eye, dusk);
            Light sun = null;
            foreach (var light in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
                if (light.type == LightType.Directional) { sun = light; break; }
            if (!sun || sun.type != LightType.Directional)
            {
                sun = new GameObject("InterludeRoomSun").AddComponent<Light>();
                sun.type = LightType.Directional;
                sun.shadows = LightShadows.Soft;
            }
            sun.enabled = true;
            sun.color = dusk ? new Color(.95f, .45f, .30f) : new Color(1f, .96f, .88f);
            sun.intensity = dusk ? .5f : 1.15f;
            sun.transform.rotation = dusk ? Quaternion.Euler(12, -70, 0) : Quaternion.Euler(50, -30, 0);
            RenderSettings.sun = sun;
        }

        private static void ApplyAtmosphere(Camera eye, bool dusk)
        {
            // A camera's backgroundColor is ignored while the authored night
            // skybox is assigned. Replace both the sky and camera clear mode.
            RenderSettings.skybox = Resources.Load<Material>(dusk ? "InterludeDuskSky" : "InterludeDaySky");
            if (eye)
            {
                eye.clearFlags = RenderSettings.skybox ? CameraClearFlags.Skybox : CameraClearFlags.SolidColor;
                eye.backgroundColor = dusk ? new Color(.30f, .22f, .20f) : new Color(.62f, .68f, .74f);
            }
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = dusk ? .008f : .0025f;
            RenderSettings.fogColor = dusk ? new Color(.30f, .22f, .20f) : new Color(.62f, .68f, .74f);
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = dusk ? new Color(.22f, .16f, .15f) : new Color(.55f, .60f, .66f);
            RenderSettings.ambientEquatorColor = dusk ? new Color(.15f, .12f, .11f) : new Color(.38f, .40f, .42f);
            RenderSettings.ambientGroundColor = dusk ? new Color(.09f, .08f, .07f) : new Color(.22f, .21f, .20f);
        }

        internal static void DropCollider(GameObject go)
        {
            var collider = go.GetComponent<Collider>();
            if (!collider) return;
            Drop(collider);
        }

        // The corridor post profile is authored for night (exposure .18).
        // Daytime clones it at runtime so the approved night look is untouched.
        private static void SetExposure(float value)
        {
            var volume = GameObject.Find("KosCorridorPost")?.GetComponent<Volume>();
            if (!volume || !volume.sharedProfile) return;
            if (!volume.profile) volume.profile = Object.Instantiate(volume.sharedProfile);
            if (volume.profile.TryGet<ColorAdjustments>(out var grade)) grade.postExposure.value = value;
        }

        internal static void Drop(Object o)
        {
            if (!o) return;
            if (Application.isPlaying) Object.Destroy(o);
            else Object.DestroyImmediate(o);
        }
    }
}
