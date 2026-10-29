#if UNITY_EDITOR
using System;
using System.IO;
using LayarTerakhir.Interlude;
using LayarTerakhir.Prolog;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class PrologEnvironmentContinuityRevision
    {
        public static void Apply()
        {
            Sky("InterludeDaySky", new Color(.50f, .58f, .68f), new Color(.62f, .68f, .74f), 1f, 1.2f);
            Sky("InterludeDuskSky", new Color(.65f, .43f, .35f), new Color(.30f, .22f, .20f), 1.5f, .85f);
            MelatiInterludeAlleyBuilder.Apply();
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            MelatiStreetRevision.RestoreKosLot(Object.FindFirstObjectByType<KosCorridorDirector>());
            var group = GameObject.Find("KampungMelati")?.transform.Find("InterludeAlley");
            if (!group) throw new Exception("Permanent street scenery is missing");
            group.gameObject.SetActive(true);
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
            AssetDatabase.SaveAssets();
            Debug.Log("PROLOG_CONTINUITY_AUTHORED: permanent street scenery active; day/dusk skies saved");
        }

        private static void Sky(string name, Color tint, Color ground, float atmosphere, float exposure)
        {
            string path = "Assets/Resources/" + name + ".mat";
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            var shader = Shader.Find("Skybox/Procedural");
            if (!shader) throw new Exception("Procedural sky shader missing");
            if (!mat) { mat = new Material(shader); AssetDatabase.CreateAsset(mat, path); }
            mat.shader = shader;
            mat.SetColor("_SkyTint", tint);
            mat.SetColor("_GroundColor", ground);
            mat.SetFloat("_AtmosphereThickness", atmosphere);
            mat.SetFloat("_Exposure", exposure);
            mat.SetFloat("_SunSize", .04f);
            mat.SetFloat("_SunDisk", 2);
            mat.EnableKeyword("_SUNDISK_HIGH_QUALITY");
            EditorUtility.SetDirty(mat);
        }

        public static void Capture()
        {
            try
            {
                const string output = "Audit/Visuals/Prolog-Continuity";
                Directory.CreateDirectory(output);
                EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
                InterludeExterior.EnsureNeighborhood(Object.FindFirstObjectByType<KosCorridorDirector>());
                var camera = new GameObject("ContinuityCapture").AddComponent<Camera>();
                camera.clearFlags = CameraClearFlags.Skybox;
                camera.farClipPlane = 150;
                foreach (string time in new[] { "night", "morning", "dusk" })
                {
                    if (time == "morning") InterludeExterior.ApplyDay(camera);
                    if (time == "dusk") InterludeExterior.ApplyDusk(camera);
                    camera.transform.position = new Vector3(-5, 1.6f, -11.75f);
                    camera.transform.LookAt(new Vector3(-14, 3.5f, -12));
                    KosRenderDiagnosis.Capture(camera, "_warmup", false, output);
                    KosRenderDiagnosis.Capture(camera, time + "-alley", false, output);
                    camera.transform.position = new Vector3(0, 1.6f, -30);
                    camera.transform.LookAt(new Vector3(0, 4, -70));
                    KosRenderDiagnosis.Capture(camera, time + "-street", false, output);
                }
                File.Delete(output + "/_warmup.png");
                Debug.Log("PROLOG_CONTINUITY_CAPTURE: six real editor renders completed");
                EditorApplication.Exit(0);
            }
            catch (Exception e) { Debug.LogException(e); EditorApplication.Exit(1); }
        }
    }
}
#endif
