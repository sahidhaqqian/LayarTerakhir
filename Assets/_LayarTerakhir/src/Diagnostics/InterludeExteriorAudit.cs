#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Interlude;
using LayarTerakhir.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Diagnostics
{
    // Directed exterior visual review for the daytime interlude.
    // Never active in normal play. Seeds an isolated INTERLUDE_DAY save,
    // walks out of the kos through the gate to the street, and captures
    // posed screenshots from the player's own camera for human review.
    public sealed class InterludeExteriorAudit : MonoBehaviour
    {
        private static string output, save;
        private readonly List<string> shots = new List<string>();
        private readonly List<string> errors = new List<string>();
        private bool done;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args = Environment.GetCommandLineArgs();
            int i = Array.IndexOf(args, "-interludeExteriorAudit");
            if (i < 0 || i + 1 >= args.Length) return;
            output = Path.GetFullPath(args[i + 1]);
            Directory.CreateDirectory(output);
            save = Path.Combine(output, "IsolatedSave-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", save);
            var seed = new CampaignData
            {
                chapter_id = 0,
                checkpoint_id = InterludeIds.DayCheckpoint,
                battery = 80,
                wallet_legitimate = 0,
                wallet_bound = 20000000,
                spent = 5450000,
                schema_version = 6
            };
            seed.inventory.AddRange(new[]
            {
                "SMARTPHONE_RAKA", "KUNCI_MOTOR", "JAKET_RAKA", "POWERBANK_KECIL_KOS",
                "DOMPET_RAKA", "DIARY_RAKA", "MATABATIN_APK", "EVENT_PAID_RENT",
                "EVENT_PAID_HOSPITAL", "EVENT_CALLED_IBU", "EVENT_CALLED_RATNA",
                InterludeIds.MorningCheckpoint, InterludeIds.Woke,
                InterludeIds.RatnaTalk, InterludeIds.HookRead
            });
            File.WriteAllText(Path.Combine(save, "layar_terakhir_save.json"), JsonUtility.ToJson(seed, true));
            var go = new GameObject("InterludeExteriorAudit");
            DontDestroyOnLoad(go);
            go.AddComponent<InterludeExteriorAudit>();
            // Software rasterizer (llvmpipe) cannot afford real-time shadows;
            // composition and dressing read fine without them.
            QualitySettings.shadows = ShadowQuality.Disable;
            QualitySettings.antiAliasing = 0;
            QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
            Debug.Log("INTERLUDE_AUDIT_INSTALL save=" + save);
        }

        private void Awake()
        {
            Application.logMessageReceived += OnLog;
        }

        private void OnLog(string message, string stack, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception) errors.Add(message);
        }

        private void OnDestroy()
        {
            Application.logMessageReceived -= OnLog;
        }

        private IEnumerator Start()
        {
            yield return null;
            Debug.Log("INTERLUDE_AUDIT_START scene=" + SceneManager.GetActiveScene().name);
            yield return WaitFor(() => FindFirstObjectByType<InterludeHub>() != null && FindFirstObjectByType<InterludeHub>().Ready, 60, "hub ready");
            var hub = FindFirstObjectByType<InterludeHub>();
            if (hub == null) { Finish(); yield break; }
            hub.Interact("EXIT");
            yield return WaitFor(() => FindFirstObjectByType<InterludeDirector>() != null && FindFirstObjectByType<InterludeDirector>().Ready, 60, "director ready");
            var director = FindFirstObjectByType<InterludeDirector>();
            if (director == null) { Finish(); yield break; }
            var player = FindFirstObjectByType<FirstPersonController>();
            player.MovementLocked = true;
            foreach (var view in Views(director, player))
            {
                Pose(player, view.pos, view.look);
                yield return new WaitForSecondsRealtime(.8f);
                Shot(view.name);
                yield return new WaitForSecondsRealtime(.2f);
            }
            Finish();
        }

        private struct View
        {
            public string name;
            public Vector3 pos, look;
        }

        private static List<View> Views(InterludeDirector director, FirstPersonController player)
        {
            var corridor = director.GetComponent<LayarTerakhir.Prolog.KosCorridorDirector>();
            System.Func<Vector3, Vector3> B = corridor.BuildingPoint;
            var gate = corridor.gateHinge.position;
            var motor = corridor.motor.position;
            return new List<View>
            {
                new View { name = "01-yard-gate", pos = B(new Vector3(0, 1.5f, -.5f)), look = B(new Vector3(1.5f, 1f, -6f)) },
                new View { name = "02-gate-kos", pos = new Vector3(gate.x, 1.5f, gate.z - 1.5f), look = B(new Vector3(0, 1.2f, -2f)) },
                new View { name = "03-lane-street", pos = new Vector3(-6.8f, 1.5f, -12f), look = new Vector3(0, 1.2f, -20f) },
                new View { name = "04-street-warung", pos = new Vector3(2f, 1.5f, -18f), look = new Vector3(8.4f, 1.2f, -24f) },
                new View { name = "05-warung-counter", pos = new Vector3(5.4f, 1.5f, -24f), look = new Vector3(7f, 1.2f, -24f) },
                new View { name = "06-street-north", pos = new Vector3(0, 1.6f, -30f), look = new Vector3(0, 1.4f, -70f) },
                new View { name = "07-street-south", pos = new Vector3(0, 1.6f, -22f), look = new Vector3(-8f, 1.2f, -12f) },
                new View { name = "08-motor-close", pos = B(new Vector3(.5f, 1.3f, -3f)), look = new Vector3(motor.x, motor.y + .8f, motor.z) }
            };
        }

        private static void Pose(FirstPersonController player, Vector3 pos, Vector3 look)
        {
            var cc = player.GetComponent<CharacterController>();
            cc.enabled = false;
            player.transform.position = pos;
            Vector3 flat = look - pos;
            flat.y = 0;
            if (flat.sqrMagnitude > .001f) player.transform.rotation = Quaternion.LookRotation(flat.normalized);
            float dist = Vector3.Distance(pos, look);
            float pitch = -Mathf.Asin(Mathf.Clamp((look.y - pos.y) / Mathf.Max(.001f, dist), -1f, 1f)) * Mathf.Rad2Deg;
            // Pitch the eye camera itself: the controller owns cameraPivot,
            // so writing the pivot would be overwritten by mouse-look state.
            var eye = player.GetComponent<InteractionInteractor>().playerCamera;
            if (eye) eye.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
            cc.enabled = true;
        }

        private void Shot(string name)
        {
            string path = Path.Combine(output, name + ".png");
            ScreenCapture.CaptureScreenshot(path);
            shots.Add(name + ".png");
            Debug.Log("INTERLUDE_EXTERIOR_SHOT " + path);
        }

        private IEnumerator WaitFor(Func<bool> condition, float seconds, string what)
        {
            float start = Time.realtimeSinceStartup;
            while (!condition())
            {
                if (Time.realtimeSinceStartup - start > seconds) { errors.Add("TIMEOUT " + what); yield break; }
                yield return null;
            }
        }

        private void Finish()
        {
            if (done) return;
            done = true;
            var receipt = new
            {
                utc = DateTime.UtcNow.ToString("o"),
                actualStandalonePlayer = !Application.isEditor,
                completed = errors.Count == 0,
                screenshots = shots.ToArray(),
                errors = errors.ToArray()
            };
            try { File.WriteAllText(Path.Combine(output, "interlude-exterior.json"), JsonUtility.ToJson(receipt, true)); } catch { }
            Debug.Log("INTERLUDE_EXTERIOR_DONE shots=" + shots.Count + " errors=" + errors.Count);
            Application.Quit(errors.Count == 0 ? 0 : 1);
        }
    }
}
#endif
