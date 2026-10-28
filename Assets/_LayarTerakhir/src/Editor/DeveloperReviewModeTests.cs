#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Review;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    // Developer Review Mode must preserve the player/camera, never touch the real
    // save, and produce complete bug reports. No scene loads; all EditMode-safe.
    public sealed class DeveloperReviewModeTests
    {
        private string folder;
        private string previousEnv;
        private readonly List<GameObject> garbage = new List<GameObject>();

        [SetUp]
        public void Setup()
        {
            previousEnv = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            folder = Path.Combine(Path.GetTempPath(), "DevReview-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(folder);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", folder);
        }

        [TearDown]
        public void Cleanup()
        {
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", previousEnv);
            foreach (var go in garbage)
                if (go) UnityEngine.Object.DestroyImmediate(go);
            garbage.Clear();
            if (CampaignState.Instance) UnityEngine.Object.DestroyImmediate(CampaignState.Instance.gameObject);
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
            Time.timeScale = 1f;
            try { AudioListener.pause = false; } catch { }
        }

        private GameObject Track(GameObject go) { garbage.Add(go); return go; }

        private CampaignState FreshCampaign()
        {
            var go = Track(new GameObject("Campaign"));
            return go.AddComponent<CampaignState>();
        }

        // ---- hierarchy helpers ----

        [Test]
        public void HierarchyPathBuildsFullPath()
        {
            var root = Track(new GameObject("EnvRoot"));
            var mid = new GameObject("Corridor");
            mid.transform.SetParent(root.transform, false);
            var leaf = new GameObject("BrokenLamp");
            leaf.transform.SetParent(mid.transform, false);
            Assert.That(ReviewHierarchy.BuildPath(leaf.transform), Is.EqualTo("EnvRoot/Corridor/BrokenLamp"));
            Assert.That(ReviewHierarchy.BuildPath(null), Is.EqualTo("<none>"));
        }

        [Test]
        public void SelectionReportContainsRequiredFields()
        {
            var go = Track(GameObject.CreatePrimitive(PrimitiveType.Cube));
            go.name = "SuspiciousWall";
            go.transform.position = new Vector3(1.5f, 2f, -3f);
            string report = ReviewHierarchy.SelectionReport(go, Vector3.zero, false);
            Assert.That(report, Does.Contain("SuspiciousWall"));
            Assert.That(report, Does.Contain("Path:"));
            Assert.That(report, Does.Contain("Scene:"));
            Assert.That(report, Does.Contain("1.50"));
            Assert.That(report, Does.Contain("renderer:"));
            Assert.That(report, Does.Contain("collider:"));
        }

        [Test]
        public void SelectionReportWithoutSelectionGuidesDirector()
        {
            Assert.That(ReviewHierarchy.SelectionReport(null, Vector3.zero, false), Does.Contain("F9"));
        }

        [Test]
        public void PhaseLabelMatchesCampaignRouting()
        {
            Assert.That(ReviewHierarchy.PhaseLabel(null), Is.EqualTo("campaign?"));
            Assert.That(ReviewHierarchy.PhaseLabel(new CampaignData()), Is.EqualTo("Prolog/Interlude"));
            var p3 = new CampaignData { chapter_id = 3, checkpoint_id = "PHASE3_CP2" };
            Assert.That(ReviewHierarchy.PhaseLabel(p3), Is.EqualTo("Phase 3"));
        }

        // ---- fly camera math + preservation ----

        [Test]
        public void FlyStepPositionMovesAlongCameraAxes()
        {
            Vector3 next = ReviewFlyCamera.StepPosition(Vector3.zero, Vector3.forward, Vector3.right,
                1f, 1f, 0f, 2f, 1f, 0.5f);
            // Diagonal input is normalized like standard fly rigs.
            Assert.That(next.x, Is.EqualTo(0.7071f).Within(0.001f));
            Assert.That(next.z, Is.EqualTo(0.7071f).Within(0.001f));
            Vector3 straight = ReviewFlyCamera.StepPosition(Vector3.zero, Vector3.forward, Vector3.right,
                0f, 1f, 0f, 2f, 1f, 0.5f);
            Assert.That(straight, Is.EqualTo(Vector3.forward));
            Vector3 up = ReviewFlyCamera.StepPosition(Vector3.zero, Vector3.forward, Vector3.right,
                0f, 0f, 1f, 4f, 1f, 1f);
            Assert.That(up, Is.EqualTo(Vector3.up * 4f));
        }

        [Test]
        public void FlyAdjustSpeedClampsToRange()
        {
            Assert.That(ReviewFlyCamera.AdjustSpeed(1000f, 1f), Is.EqualTo(ReviewFlyCamera.MaxSpeed));
            Assert.That(ReviewFlyCamera.AdjustSpeed(0.01f, -1f), Is.EqualTo(ReviewFlyCamera.MinSpeed));
            Assert.That(ReviewFlyCamera.AdjustSpeed(6f, 0f), Is.EqualTo(6f));
            Assert.That(ReviewFlyCamera.AdjustSpeed(6f, 1f), Is.GreaterThan(6f));
        }

        [Test]
        public void FlyEnableDisablePreservesPlayerAndCamera()
        {
            var playerGo = Track(new GameObject("Raka"));
            playerGo.transform.SetPositionAndRotation(new Vector3(10f, 0f, 5f), Quaternion.Euler(0f, 90f, 0f));
            var player = playerGo.AddComponent<FirstPersonController>();
            var pivot = new GameObject("Pivot").transform;
            pivot.SetParent(playerGo.transform, false);
            pivot.localPosition = new Vector3(0f, 1.6f, 0f);
            player.cameraPivot = pivot;
            var interactor = playerGo.AddComponent<InteractionInteractor>();
            var eyeGo = Track(new GameObject("Eye"));
            var eye = eyeGo.AddComponent<Camera>();
            eye.transform.SetPositionAndRotation(new Vector3(10f, 1.6f, 5f), Quaternion.Euler(-10f, 90f, 0f));
            interactor.playerCamera = eye;

            var flyGo = Track(new GameObject("Fly"));
            var fly = flyGo.AddComponent<ReviewFlyCamera>();
            Assert.That(fly.Enable(player), Is.True, "Fly must engage with a valid eye camera");
            Assert.That(player.enabled, Is.False);
            Assert.That(interactor.enabled, Is.False);

            // Simulate a flight far away from Raka.
            eye.transform.position = new Vector3(-50f, 30f, 99f);
            eye.transform.rotation = Quaternion.Euler(30f, 180f, 0f);
            fly.Disable();

            Assert.That((Vector3)playerGo.transform.position, Is.EqualTo(new Vector3(10f, 0f, 5f)));
            Assert.That(Quaternion.Angle(playerGo.transform.rotation, Quaternion.Euler(0f, 90f, 0f)),
                Is.LessThan(0.01f), "Player rotation must be untouched");
            Assert.That((Vector3)eye.transform.position, Is.EqualTo(new Vector3(10f, 1.6f, 5f)));
            Assert.That(Quaternion.Angle(eye.transform.rotation, Quaternion.Euler(-10f, 90f, 0f)),
                Is.LessThan(0.01f), "Eye rotation must be restored");
            Assert.That(player.enabled, Is.True, "Player controller must be restored");
            Assert.That(interactor.enabled, Is.True, "Interactor must be restored");
        }

        // ---- inspector ----

        [Test]
        public void InspectorEnableDisableRestoresPlayerInput()
        {
            var playerGo = Track(new GameObject("Raka"));
            var player = playerGo.AddComponent<FirstPersonController>();
            var interactor = playerGo.AddComponent<InteractionInteractor>();
            var inspector = Track(new GameObject("Inspector")).AddComponent<ReviewObjectInspector>();
            Assert.That(inspector.Enable(player), Is.True);
            Assert.That(player.enabled, Is.False);
            Assert.That(interactor.enabled, Is.False);
            inspector.Disable();
            Assert.That(player.enabled, Is.True);
            Assert.That(interactor.enabled, Is.True);
        }

        [Test]
        public void InspectorHighlightNeverTouchesTargetMaterials()
        {
            var playerGo = Track(new GameObject("Raka"));
            var player = playerGo.AddComponent<FirstPersonController>();
            playerGo.AddComponent<InteractionInteractor>();
            var target = Track(GameObject.CreatePrimitive(PrimitiveType.Cube));
            int beforeComponents = target.GetComponents<Component>().Length;
            var beforeMaterials = target.GetComponent<Renderer>().sharedMaterials;

            var inspectorGo = Track(new GameObject("Inspector"));
            var inspector = inspectorGo.AddComponent<ReviewObjectInspector>();
            inspector.Enable(player);
            inspector.SelectDirect(target, target.transform.position);

            Assert.That(inspector.Selected, Is.SameAs(target));
            Assert.That(inspector.CurrentReport, Does.Contain(target.name));
            Assert.That(target.GetComponents<Component>().Length, Is.EqualTo(beforeComponents),
                "Highlight must not add components to the target");
            Assert.That(target.GetComponent<Renderer>().sharedMaterials, Is.EqualTo(beforeMaterials),
                "Highlight must not swap target materials");
            Assert.That(inspectorGo.transform.childCount, Is.GreaterThan(0),
                "Highlight box must live under the review rig");

            inspector.ClearSelection();
            Assert.That(inspector.Selected, Is.Null);
            Assert.That(inspectorGo.transform.childCount, Is.EqualTo(0),
                "Highlight box must be destroyed on deselect");
        }

        // ---- bug capture ----

        [Test]
        public void BugMetadataContainsRequiredFields()
        {
            var data = new CampaignData { chapter_id = 2, checkpoint_id = "PHASE2_CP1" };
            var target = Track(new GameObject("HauntedBed"));
            target.transform.position = new Vector3(4f, 0f, 1f);
            var meta = ReviewBugCapture.BuildMetadata("Babak2_RSUD", data,
                new Vector3(1f, 2f, 3f), new Vector3(0f, 90f, 0f), target,
                "ranjang melayang", "1600x900", true, false);
            Assert.That(meta.scene, Is.EqualTo("Babak2_RSUD"));
            Assert.That(meta.phase, Is.EqualTo("Phase 2"));
            Assert.That(meta.checkpointId, Is.EqualTo("PHASE2_CP1"));
            Assert.That(meta.cameraPosition.x, Is.EqualTo(1f));
            Assert.That(meta.selection.hierarchyPath, Does.Contain("HauntedBed"));
            Assert.That(meta.selection.position.z, Is.EqualTo(1f));
            Assert.That(meta.description, Is.EqualTo("ranjang melayang"));
            Assert.That(meta.timestampUtc, Is.Not.Empty);
            Assert.That(meta.flyActive, Is.True);
        }

        [Test]
        public void BugReportWritesPngAndJsonSidecar()
        {
            byte[] png = Convert.FromBase64String(
                "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==");
            var meta = ReviewBugCapture.BuildMetadata("Prolog_KamarKos", new CampaignData(),
                Vector3.zero, Vector3.zero, null, "", "2x2", false, false);
            string dir = Path.Combine(folder, "BugReports");
            ReviewBugCapture.WriteReport(png, meta, dir, "Bug-test", out string pngPath, out string jsonPath);
            Assert.That(File.Exists(pngPath), Is.True);
            Assert.That(File.Exists(jsonPath), Is.True);
            string json = File.ReadAllText(jsonPath);
            Assert.That(json, Does.Contain("Prolog_KamarKos"));
            Assert.That(json, Does.Contain("timestampUtc"));
            Assert.Throws<ArgumentException>(() =>
                ReviewBugCapture.WriteReport(null, meta, dir, "Bug-bad", out _, out _));
        }

        // ---- freeze ----

        [Test]
        public void FreezeRestoresPreviousTimeScale()
        {
            Time.timeScale = 1f;
            var freeze = Track(new GameObject("Freeze")).AddComponent<ReviewWorldFreeze>();
            Assert.That(freeze.Freeze(), Is.True);
            Assert.That(Time.timeScale, Is.EqualTo(0f));
            freeze.Unfreeze();
            Assert.That(Time.timeScale, Is.EqualTo(1f));
            Assert.That(freeze.Frozen, Is.False);
        }

        // ---- phase navigation: fixtures route, real save untouched ----

        [Test]
        public void ReviewTargetsAreUniqueAndNamed()
        {
            var ids = new HashSet<string>();
            Assert.That(ReviewPhaseNavigator.Targets.Length, Is.GreaterThanOrEqualTo(11));
            foreach (var t in ReviewPhaseNavigator.Targets)
            {
                Assert.That(t.scene, Is.Not.Empty, "target " + t.id);
                Assert.That(ids.Add(t.id), Is.True, "duplicate target " + t.id);
                Assert.That(ReviewPhaseNavigator.FindTarget(t.id), Is.SameAs(t));
            }
            Assert.That(ReviewPhaseNavigator.FindTarget("nope"), Is.Null);
        }

        [Test]
        public void ReviewFixturesPassValidatorAndRouteToTargetScenes()
        {
            // Hub targets ride their production fixtures; main scenes ride Depart/handoff.
            var expectations = new Dictionary<string, string>
            {
                { "prolog-kos", GameShell.KosScene },
                { "p1", CampaignRouting.RusunawaScene },
                { "p2hub", GameShell.KosScene },
                { "p2", CampaignRouting.HospitalScene },
                { "p3hub", GameShell.KosScene },
                { "p3", CampaignRouting.FactoryScene },
                { "p4hub", GameShell.KosScene },
                { "p4", CampaignRouting.VillageScene },
                { "p5", CampaignRouting.MarketScene },
            };
            foreach (var kv in expectations)
            {
                var fixture = ReviewPhaseNavigator.BuildBaseFixture(kv.Key);
                Assert.That(fixture, Is.Not.Null, "fixture " + kv.Key);
                File.WriteAllText(Path.Combine(folder, "layar_terakhir_save.json"),
                    JsonUtility.ToJson(fixture, true));
                var campaign = FreshCampaign();
                Assert.That(campaign.LoadCheckpoint(), Is.True, "fixture must validate: " + kv.Key);
                if (ReviewPhaseNavigator.NeedsPostStep(kv.Key))
                    Assert.That(ReviewPhaseNavigator.ApplyPostStep(kv.Key, campaign), Is.True,
                        "production gate must accept: " + kv.Key);
                Assert.That(CampaignRouting.ContinueScene(campaign.Data), Is.EqualTo(kv.Value),
                    "routing " + kv.Key);
                garbage.Add(campaign.gameObject);
                UnityEngine.Object.DestroyImmediate(campaign.gameObject);
            }
        }

        [Test]
        public void ReviewDepartGatesSatisfyDirectorBootChecks()
        {
            foreach (string id in new[] { "p2", "p3", "p4" })
            {
                File.WriteAllText(Path.Combine(folder, "layar_terakhir_save.json"),
                    JsonUtility.ToJson(ReviewPhaseNavigator.BuildBaseFixture(id), true));
                var campaign = FreshCampaign();
                Assert.That(campaign.LoadCheckpoint(), Is.True);
                Assert.That(ReviewPhaseNavigator.ApplyPostStep(id, campaign), Is.True);
                int phase = id == "p2" ? 2 : id == "p3" ? 3 : 4;
                bool gate = phase == 2 ? CampaignRouting.IsPhase2(campaign.Data) && campaign.Data.phase2.Completed("P2_DEPART_KOS")
                    : phase == 3 ? CampaignRouting.IsPhase3(campaign.Data) && campaign.Data.phase3.Completed("P3_DEPART_KOS")
                    : CampaignRouting.IsPhase4(campaign.Data) && campaign.Data.phase4.Completed("P4_DEPART_KOS");
                Assert.That(gate, Is.True, "director boot gate for " + id);
                UnityEngine.Object.DestroyImmediate(campaign.gameObject);
            }
        }

        [Test]
        public void Phase5EnteredFixturePassesMarketGate()
        {
            File.WriteAllText(Path.Combine(folder, "layar_terakhir_save.json"),
                JsonUtility.ToJson(ReviewPhaseNavigator.BuildBaseFixture("p5"), true));
            var campaign = FreshCampaign();
            Assert.That(campaign.LoadCheckpoint(), Is.True);
            Assert.That(ReviewPhaseNavigator.ApplyPostStep("p5", campaign), Is.True);
            Assert.That(CampaignRouting.IsPhase5(campaign.Data), Is.True);
            UnityEngine.Object.DestroyImmediate(campaign.gameObject);
        }

        [Test]
        public void TempSlotWritesNeverTouchTheRealSave()
        {
            string realDir = Path.Combine(folder, "RealPlayer");
            Directory.CreateDirectory(realDir);
            string realSave = Path.Combine(realDir, "layar_terakhir_save.json");
            var original = new CampaignData { chapter_id = 1, checkpoint_id = "PHASE1_CP3", battery = 42f };
            File.WriteAllText(realSave, JsonUtility.ToJson(original, true));
            byte[] before = File.ReadAllBytes(realSave);

            // Excursion writes go to the redirected slot only.
            string tempSave = Path.Combine(folder, "layar_terakhir_save.json");
            File.WriteAllText(tempSave, JsonUtility.ToJson(ReviewPhaseNavigator.BuildBaseFixture("p3"), true));
            var campaign = FreshCampaign();
            Assert.That(campaign.LoadCheckpoint(), Is.True);
            Assert.That(ReviewPhaseNavigator.ApplyPostStep("p3", campaign), Is.True);
            UnityEngine.Object.DestroyImmediate(campaign.gameObject);

            Assert.That(File.ReadAllBytes(realSave), Is.EqualTo(before),
                "The player save outside the redirect must be byte-identical");
        }
    }
}
#endif
