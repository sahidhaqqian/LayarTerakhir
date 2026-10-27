#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Diagnostics
{
    // Review launchers (Review-Phase1..5.cmd) skip the Main Menu and continue
    // straight into the requested phase. Normal startup, New Game, and every
    // *Audit driver keep the Main Menu flow untouched.
    public sealed class ReviewDirectBoot : MonoBehaviour
    {
        // Returns the requested review phase (1-5), or 0 when this launch is
        // not a phase review. Audit drivers always win over review flags.
        public static int ReviewPhaseFor(string[] args)
        {
            if (args == null || args.Length == 0) return 0;
            if (HasAudit(args)) return 0;
            for (int phase = 1; phase <= 5; phase++)
                if (Array.IndexOf(args, "-phase" + phase + "Review") >= 0) return phase;
            return 0;
        }

        // Same audit definition as GameShell: any *Audit arg except -reviewAudit.
        private static bool HasAudit(string[] args) =>
            Array.Exists(args, a => a != "-reviewAudit" && a.EndsWith("Audit", StringComparison.Ordinal));

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            if (ReviewPhaseFor(Environment.GetCommandLineArgs()) <= 0) return;
            var go = new GameObject("ReviewDirectBoot");
            DontDestroyOnLoad(go);
            go.AddComponent<ReviewDirectBoot>();
        }

        private void Start()
        {
            try
            {
                // Fires once, after the boot scene (menu + campaign slot) is ready.
                // GameShell.Continue runs the exact production checkpoint path, so
                // progression, handoffs and isolated saves behave like a manual click.
                if (ReviewPhaseFor(Environment.GetCommandLineArgs()) > 0 &&
                    GameShell.Instance && GameShell.AwaitingStart && CampaignState.Instance)
                    GameShell.Instance.Continue();
            }
            finally
            {
                Destroy(gameObject);
            }
        }
    }
}
#endif
