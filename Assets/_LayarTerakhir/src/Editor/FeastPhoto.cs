#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor
{
    // One-shot visual proof for the designer: renders the rotten dining spread
    // (stained plates, moldy food, dead rats, spills, black smoke) straight
    // from the saved scene into Audit/Visuals/RottenFeastProof. Run headful
    // under xvfb (no -nographics).
    public static class FeastPhoto
    {
        public static void ProbeSmokeFrame()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<LayarTerakhir.Prolog.AtmajaDirector>();
            var env = GameObject.Find("Atmaja_Environment_Art").transform;
            Debug.Log("FEAST_FRAME envrot=" + env.rotation.eulerAngles + " envscale=" + env.lossyScale + " east=" + d.east + " north=" + d.north);
            var pre = GameObject.Find("RottenStinkSmoke");
            Debug.Log("FEAST_FRAME smoke=" + (pre ? "found rot=" + pre.transform.rotation.eulerAngles + " scale=" + pre.transform.lossyScale : "MISSING"));
            if (pre)
            {
                var pr = pre.GetComponent<ParticleSystemRenderer>();
                Debug.Log("FEAST_FRAME renderer=" + (pr ? "found mat=" + (pr.sharedMaterial ? pr.sharedMaterial.name + "/" + pr.sharedMaterial.shader.name : "NULL") + " mode=" + pr.renderMode + " layer=" + pr.gameObject.layer : "NULL"));
            }
        }
        public static void CaptureProof()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d = Object.FindFirstObjectByType<LayarTerakhir.Prolog.AtmajaDirector>();
            Physics.SyncTransforms();
            // Pre-roll the small stink-wisps so the stills show what players
            // see after a few seconds in the room (EditMode does not auto-run it).
            var pres = Object.FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None).Where(s=>s.name.StartsWith("RottenStinkSmoke")).ToList();
            foreach (var pps in pres)
            {
                pps.Simulate(3.5f, true, true);
                var parts = new ParticleSystem.Particle[pps.particleCount];
                int n = pps.GetParticles(parts);
                string samp = "";
                for (int k = 0; k < Mathf.Min(3, n); k++)
                    samp += string.Format("[{0:F2},{1:F2},{2:F2} s={3:F2}]", parts[k].position.x, parts[k].position.y, parts[k].position.z, parts[k].GetCurrentSize(pps));
                Debug.Log("FEAST_SMOKE_DIAG " + pps.name + " alive=" + pps.particleCount + " emitting=" + pps.emission.rateOverTime.constant + " pos=" + pps.transform.position + " parts=" + samp);
            }
            var cam = new GameObject("ProofCam").AddComponent<Camera>();
            cam.nearClipPlane = .05f;
            cam.farClipPlane = 70;
            cam.backgroundColor = RenderSettings.fogColor;
            cam.clearFlags = CameraClearFlags.SolidColor;
            var spot = new GameObject("ProofSpot").AddComponent<Light>();
            spot.type = LightType.Spot;
            spot.intensity = 4f;
            spot.range = 9;
            spot.spotAngle = 60;
            spot.color = new Color(1f, .93f, .8f);
            spot.transform.position = d.P(3.71f, 4.5f, 3.4f);
            spot.transform.rotation = Quaternion.LookRotation(Vector3.down + d.east * .15f);

            // Wide: whole spoiled table with smoke.
            var table = GameObject.Find("LongDiningTable");
            if (table)
            {
                var b = KosVisualV2.BoundsOf(table);
                cam.transform.position = d.P(5.6f, 4.5f, 2.6f);
                cam.transform.rotation = Quaternion.LookRotation((b.center + Vector3.up * .3f - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "rotten-feast-wide", false, "Audit/Visuals/RottenFeastProof");
            }
            // Close-up: a dish with a rat carcass on it.
            var rat = GameObject.Find("AddedDinner_Rat_4") ?? GameObject.Find("AddedDinner_Rat_1");
            if (rat)
            {
                var b = KosVisualV2.BoundsOf(rat);
                spot.transform.position = b.center + Vector3.up * 1.4f;
                spot.transform.rotation = Quaternion.LookRotation(Vector3.down);
                cam.transform.position = b.center + Vector3.up * .55f + d.east * .55f;
                cam.transform.rotation = Quaternion.LookRotation((b.center - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "rotten-feast-rat-closeup", false, "Audit/Visuals/RottenFeastProof");
            }
            // Low along the table: spills + tipped pot + smoke columns.
            var pot = GameObject.Find("AddedDish_TippedPot");
            if (pot)
            {
                var b = KosVisualV2.BoundsOf(pot);
                cam.transform.position = d.P(3.71f, 2.6f, 2.1f);
                cam.transform.rotation = Quaternion.LookRotation((b.center - cam.transform.position).normalized);
                KosRenderDiagnosis.Capture(cam, "rotten-feast-spill-smoke", false, "Audit/Visuals/RottenFeastProof");
            }

            Object.DestroyImmediate(spot.gameObject);
            Object.DestroyImmediate(cam.gameObject);
            Debug.Log("FEAST_PHOTO_DONE");
        }
    }
}
#endif
