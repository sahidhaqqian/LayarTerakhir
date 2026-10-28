#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Diagnostics;
using LayarTerakhir.Phase2;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Phase5;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Review
{
    // In-session phase jumps reusing the review-launcher isolation mechanism:
    // the save directory is redirected to a temp slot, so the real campaign file
    // is never written. Restoring reloads the untouched real checkpoint.
    public sealed class ReviewPhaseNavigator : MonoBehaviour
    {
        public const string SaveDirEnvVar = "LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY";

        [Serializable]
        public sealed class Target
        {
            public string id;
            public string label;
            public string scene;
            public Target(string id, string label, string scene)
            {
                this.id = id;
                this.label = label;
                this.scene = scene;
            }
        }

        public static readonly Target[] Targets =
        {
            new Target("prolog-kos", "Prolog — Kamar Kos", GameShell.KosScene),
            new Target("prolog-koridor", "Prolog — Koridor Kos", "Prolog_KoridorKos"),
            new Target("prolog-atmaja", "Prolog — Rumah Atmaja", "Prolog_RumahAtmaja"),
            new Target("p1", "Phase 1 — Rusunawa", CampaignRouting.RusunawaScene),
            new Target("p2hub", "Phase 2 — Kos Hub", GameShell.KosScene),
            new Target("p2", "Phase 2 — RSUD", CampaignRouting.HospitalScene),
            new Target("p3hub", "Phase 3 — Kos Hub", GameShell.KosScene),
            new Target("p3", "Phase 3 — Pabrik Gula", CampaignRouting.FactoryScene),
            new Target("p4hub", "Phase 4 — Kos Hub", GameShell.KosScene),
            new Target("p4", "Phase 4 — Karang Setra", CampaignRouting.VillageScene),
            new Target("p5", "Phase 5 — Pasar Setan", CampaignRouting.MarketScene),
        };

        public bool ExcursionActive { get; private set; }
        public string ExcursionTarget { get; private set; }
        public string OriginScene { get; private set; }
        public bool Loading { get; private set; }

        private string previousEnvDir;
        private bool previousEnvCaptured;
        private string previousDataJson;

        public static string TempDirectory =>
            Path.Combine(Application.persistentDataPath, "ReviewNav");

        public static Target FindTarget(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            foreach (var t in Targets)
                if (t.id == id) return t;
            return null;
        }

        // Pure fixture construction, no disk access. Depart/handoff post steps run
        // through the production transactions on the temp slot (see ApplyPostStep).
        public static CampaignData BuildBaseFixture(string id)
        {
            switch (id)
            {
                case "prolog-kos":
                case "prolog-koridor":
                case "prolog-atmaja":
                    return new CampaignData();
                case "p1":
                {
                    var d = new CampaignData { chapter_id = 1, checkpoint_id = "PHASE1_CP1", battery = 100f, sanity = 100f };
                    d.inventory.AddRange(new[] { "SMARTPHONE_RAKA", "JAKET_RAKA", "MATABATIN_APK" });
                    d.phase1.Add("P1_ARRIVAL");
                    d.phase1.safe_anchor = "ARRIVAL";
                    return d;
                }
                case "p2hub":
                case "p2":
                    return Phase2PlayerAudit.ReviewFixture();
                case "p3hub":
                case "p3":
                    return Phase3PlayerAudit.ReviewFixture();
                case "p4hub":
                case "p4":
                    return Phase4PlayerAudit.ReviewFixture();
                case "p5":
                    return Phase5ReviewFixture.Boundary();
                default:
                    return null;
            }
        }

        // Production gate into the main scenes. Hub/prolog targets need no step.
        public static bool ApplyPostStep(string id, CampaignState campaign)
        {
            switch (id)
            {
                case "p2": return Phase2Campaign.Depart(campaign);
                case "p3": return Phase3Campaign.Depart(campaign);
                case "p4": return Phase4Campaign.Depart(campaign);
                case "p5": return Phase5Campaign.Enter(campaign);
                default: return true;
            }
        }

        public static bool NeedsPostStep(string id) =>
            id == "p2" || id == "p3" || id == "p4" || id == "p5";

        public bool JumpTo(string id)
        {
            var mode = GetComponent<DeveloperReviewMode>();
            var target = FindTarget(id);
            var campaign = CampaignState.Instance;
            if (target == null || campaign == null)
            {
                if (mode) mode.Toast("Target review tidak dikenal.");
                return false;
            }
            if (Loading)
            {
                if (mode) mode.Toast("Masih memuat scene, tunggu sebentar.");
                return false;
            }
            if (!Application.CanStreamedLevelBeLoaded(target.scene))
            {
                if (mode) mode.Toast("Scene belum tersedia di build ini: " + target.scene);
                return false;
            }
            if (UiFocus.Menu || GameShell.AwaitingStart || GameShell.MenuSceneLoading)
            {
                if (mode) mode.Toast("Tutup menu dulu sebelum pindah scene review.");
                return false;
            }
            if (UiFocus.Cinematic)
            {
                if (mode) mode.Toast("Tunggu sinematik selesai sebelum pindah scene.");
                return false;
            }
            CampaignData fixture;
            try { fixture = BuildBaseFixture(id); }
            catch (Exception ex)
            {
                if (mode) mode.Toast("Fixture review gagal dibuat: " + ex.Message);
                return false;
            }
            if (fixture == null)
            {
                if (mode) mode.Toast("Fixture review tidak tersedia untuk: " + target.label);
                return false;
            }

            // Snapshot the real campaign once per excursion; chained jumps keep it.
            if (!ExcursionActive)
            {
                previousEnvDir = Environment.GetEnvironmentVariable(SaveDirEnvVar);
                previousEnvCaptured = true;
                previousDataJson = JsonUtility.ToJson(campaign.Data);
                OriginScene = ReviewHierarchy.ActiveSceneName();
            }
            string dir = TempDirectory;
            string tempSave = Path.Combine(dir, "layar_terakhir_save.json");
            byte[] tempBackup = null;
            if (ExcursionActive && File.Exists(tempSave))
            {
                try { tempBackup = File.ReadAllBytes(tempSave); } catch { tempBackup = null; }
            }
            try
            {
                if (Directory.Exists(dir))
                    foreach (var f in Directory.GetFiles(dir, "*.json")) File.Delete(f);
                else Directory.CreateDirectory(dir);
                Environment.SetEnvironmentVariable(SaveDirEnvVar, dir);
                File.WriteAllText(tempSave, JsonUtility.ToJson(fixture, true));
            }
            catch (Exception ex)
            {
                AbortJump("Slot review sementara gagal disiapkan: " + ex.Message, tempBackup);
                return false;
            }
            if (!campaign.LoadCheckpoint())
            {
                AbortJump("Fixture review ditolak validator campaign.", tempBackup);
                return false;
            }
            if (NeedsPostStep(id) && !ApplyPostStep(id, campaign))
            {
                AbortJump("Gerbang produksi menolak target: " + target.label, tempBackup);
                return false;
            }

            ExcursionActive = true;
            ExcursionTarget = id;
            if (mode) mode.SuspendForSceneLoad();
            Loading = true;
            try { SceneManager.LoadSceneAsync(target.scene); }
            catch (Exception ex)
            {
                Loading = false;
                AbortJump("Gagal memuat scene: " + ex.Message, tempBackup);
                return false;
            }
            if (mode) mode.Toast("Review: " + target.label + " (save asli tidak diubah)");
            return true;
        }

        public bool RestoreOrigin()
        {
            var mode = GetComponent<DeveloperReviewMode>();
            if (!ExcursionActive)
            {
                if (mode) mode.Toast("Tidak sedang excursion review.");
                return false;
            }
            if (Loading)
            {
                if (mode) mode.Toast("Masih memuat scene, tunggu sebentar.");
                return false;
            }
            var campaign = CampaignState.Instance;
            if (mode) mode.SuspendForSceneLoad();
            RestoreEnvironment();
            try
            {
                if (Directory.Exists(TempDirectory)) Directory.Delete(TempDirectory, true);
            }
            catch { /* best effort */ }
            bool loaded = campaign && campaign.LoadCheckpoint();
            if (!loaded && campaign && !string.IsNullOrEmpty(previousDataJson))
            {
                try
                {
                    JsonUtility.FromJsonOverwrite(previousDataJson, campaign.Data);
                    loaded = true;
                }
                catch { loaded = false; }
            }
            ExcursionActive = false;
            ExcursionTarget = null;
            string scene = string.IsNullOrEmpty(OriginScene) ? GameShell.KosScene : OriginScene;
            Loading = true;
            try { SceneManager.LoadSceneAsync(scene); }
            catch (Exception ex)
            {
                Loading = false;
                if (mode) mode.Toast("Gagal kembali: " + ex.Message);
                return false;
            }
            if (mode) mode.Toast(loaded
                ? "Kembali ke campaign asli (tidak diubah)."
                : "Kembali, tapi checkpoint asli gagal dimuat. Cek save.");
            return true;
        }

        public void ReloadScene()
        {
            var mode = GetComponent<DeveloperReviewMode>();
            if (Loading) return;
            if (mode) mode.SuspendForSceneLoad();
            Loading = true;
            SceneManager.LoadSceneAsync(ReviewHierarchy.ActiveSceneName());
        }

        private void AbortJump(string reason, byte[] tempBackup)
        {
            var campaign = CampaignState.Instance;
            // A failed chained jump stays inside the excursion on the current scene.
            if (ExcursionActive && tempBackup != null && campaign)
            {
                try
                {
                    Environment.SetEnvironmentVariable(SaveDirEnvVar, TempDirectory);
                    File.WriteAllBytes(Path.Combine(TempDirectory, "layar_terakhir_save.json"), tempBackup);
                    campaign.LoadCheckpoint();
                }
                catch { /* memory snapshot below is the backstop */ }
                var mode = GetComponent<DeveloperReviewMode>();
                if (mode) mode.Toast(reason + " Tetap di scene saat ini.");
                Debug.LogWarning("[ReviewMode] " + reason);
                return;
            }
            RestoreEnvironment();
            if (campaign) campaign.LoadCheckpoint();
            // Invariant: while an excursion is active the save dir must stay temp,
            // even if the temp backup itself was lost. Never strand temp data on real.
            if (ExcursionActive && campaign)
            {
                try
                {
                    Environment.SetEnvironmentVariable(SaveDirEnvVar, TempDirectory);
                    Directory.CreateDirectory(TempDirectory);
                    File.WriteAllText(Path.Combine(TempDirectory, "layar_terakhir_save.json"),
                        JsonUtility.ToJson(campaign.Data, true));
                }
                catch { /* read-only from here; restore path still works */ }
            }
            else
            {
                previousDataJson = null;
            }
            var m = GetComponent<DeveloperReviewMode>();
            if (m) m.Toast(reason + " Save asli tidak diubah.");
            Debug.LogWarning("[ReviewMode] " + reason);
        }

        private void RestoreEnvironment()
        {
            if (!previousEnvCaptured) return;
            previousEnvCaptured = false;
            try { Environment.SetEnvironmentVariable(SaveDirEnvVar, previousEnvDir); }
            catch { /* keep going; memory snapshot is the backstop */ }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
        {
            Loading = false;
        }
    }
}
#endif
