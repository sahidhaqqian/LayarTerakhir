using LayarTerakhir.Prolog;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace LayarTerakhir.Interlude
{
    // Daytime presentation helpers for the interlude. Street architecture
    // lives authored in the corridor scene (MelatiInterludeAlleyBuilder);
    // this keeps runtime kos-yard dressing, lighting profiles, and
    // edit-mode-safe destroy helpers.
    public static class InterludeExterior
    {
        public static void DressYard(KosCorridorDirector corridor)
        {
            var root = corridor.buildingRoot;
            var poleMat = new Material(Shader.Find("Universal Render Pipeline/Lit")) { color = new Color(.30f, .24f, .16f) };
            foreach (float z in new[] { -6.5f, -1.5f })
            {
                var pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                pole.name = "InterludeClothesPole";
                pole.transform.SetParent(root, false);
                pole.transform.localPosition = new Vector3(3.8f, .9f, z);
                pole.transform.localScale = new Vector3(.07f, 1.8f, .07f);
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
            galon.transform.localPosition = new Vector3(-2.2f, .55f, 2.6f);
            galon.transform.localScale = new Vector3(.32f, .8f, .32f);
            var aqua = new Material(Shader.Find("Universal Render Pipeline/Lit")) { color = new Color(.45f, .70f, .85f, .75f) };
            galon.GetComponent<Renderer>().sharedMaterial = aqua;
            DropCollider(galon);
            var broom = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            broom.name = "InterludeBroom";
            broom.transform.SetParent(root, false);
            broom.transform.localPosition = new Vector3(-2.3f, .7f, 5.6f);
            broom.transform.localScale = new Vector3(.04f, 1.4f, .04f);
            broom.GetComponent<Renderer>().sharedMaterial = poleMat;
            DropCollider(broom);
        }

        public static void ApplyDay(Camera eye)
        {
            SetExposure(.45f);
            foreach (var light in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
            {
                if (light.type == LightType.Directional)
                {
                    light.color = new Color(1f, .96f, .88f);
                    light.intensity = 1.15f;
                    light.transform.rotation = Quaternion.Euler(50, -30, 0);
                }
                else light.enabled = light.name == "WartegCaseLight";
            }
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = .0025f;
            RenderSettings.fogColor = new Color(.62f, .68f, .74f);
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.55f, .60f, .66f);
            RenderSettings.ambientEquatorColor = new Color(.38f, .40f, .42f);
            RenderSettings.ambientGroundColor = new Color(.22f, .21f, .20f);
            if (eye) eye.backgroundColor = new Color(.62f, .68f, .74f);
        }

        public static void ApplyDusk(Camera eye)
        {
            SetExposure(.45f);
            foreach (var light in Object.FindObjectsByType<Light>(FindObjectsSortMode.None))
            {
                if (light.type == LightType.Directional)
                {
                    light.color = new Color(.95f, .45f, .30f);
                    light.intensity = .5f;
                    light.transform.rotation = Quaternion.Euler(12, -70, 0);
                }
                else if (light.name.Contains("StreetLight") || light.name.Contains("WarungWarmLight") || light.name == "WartegCaseLight") light.enabled = true;
            }
            RenderSettings.fog = true;
            RenderSettings.fogMode = FogMode.ExponentialSquared;
            RenderSettings.fogDensity = .008f;
            RenderSettings.fogColor = new Color(.30f, .22f, .20f);
            RenderSettings.ambientMode = AmbientMode.Trilight;
            RenderSettings.ambientSkyColor = new Color(.22f, .16f, .15f);
            RenderSettings.ambientEquatorColor = new Color(.15f, .12f, .11f);
            RenderSettings.ambientGroundColor = new Color(.09f, .08f, .07f);
            if (eye) eye.backgroundColor = new Color(.30f, .22f, .20f);
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
