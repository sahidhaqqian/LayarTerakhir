#if UNITY_EDITOR
using System;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Diagnostics;
using LayarTerakhir.Phase2;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Phase5;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    // Review launchers must bypass the Main Menu only in review mode.
    // Normal startup and every audit driver must keep the menu flow.
    public sealed class ReviewDirectBootTests
    {
        private string folder, previous;
        private string Slot => Path.Combine(folder, "layar_terakhir_save.json");
        [SetUp] public void Setup()
        {
            previous = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            folder = Path.Combine(Path.GetTempPath(), "ReviewBoot-" + Guid.NewGuid());
            Directory.CreateDirectory(folder);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", folder);
        }
        [TearDown] public void Cleanup()
        {
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", previous);
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
            if (CampaignState.Instance) UnityEngine.Object.DestroyImmediate(CampaignState.Instance.gameObject);
        }
        private CampaignState Load(string json)
        {
            File.WriteAllText(Slot, json);
            var c = new GameObject("Campaign").AddComponent<CampaignState>();
            Assert.That(c.LoadCheckpoint(), Is.True, "Review entry save must load and migrate");
            return c;
        }
        [Test] public void Phase1EntryRoutesToRusunawa()
        {
            var c = Load(File.ReadAllText("Builds/Windows/Phase1Review-Start.json"));
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(CampaignRouting.RusunawaScene));
        }
        [Test] public void Phase2EntryRoutesToKosHub()
        {
            var c = Load(JsonUtility.ToJson(Phase2PlayerAudit.ReviewFixture(), true));
            Assert.That(CampaignRouting.IsPhase2(c.Data), Is.True);
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(GameShell.KosScene));
        }
        [Test] public void Phase3EntryRoutesToKosHub()
        {
            var c = Load(JsonUtility.ToJson(Phase3PlayerAudit.ReviewFixture(), true));
            Assert.That(CampaignRouting.IsPhase3(c.Data), Is.True);
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(GameShell.KosScene));
        }
        [Test] public void Phase4EntryRoutesToKosHub()
        {
            var c = Load(JsonUtility.ToJson(Phase4PlayerAudit.ReviewFixture(), true));
            Assert.That(CampaignRouting.IsPhase4(c.Data), Is.True);
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(GameShell.KosScene));
        }
        [Test] public void Phase5EntryHandsOffToMarket()
        {
            var c = Load(File.ReadAllText(Path.Combine(Application.dataPath, "Resources/Phase5ReviewBoundary.json")));
            Assert.That(Phase5Ids.Eligible(c.Data), Is.True, "Boundary must pass the production P4->P5 gate");
            Assert.That(Phase5Campaign.Enter(c), Is.True, "Production handoff must commit");
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(CampaignRouting.MarketScene));
        }
        [TestCase("-phase1Review", 1)]
        [TestCase("-phase2Review", 2)]
        [TestCase("-phase3Review", 3)]
        [TestCase("-phase4Review", 4)]
        [TestCase("-phase5Review", 5)]
        public void ReviewFlagSelectsPhase(string flag, int expected)
        {
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", flag }), Is.EqualTo(expected));
        }

        [Test]
        public void NormalStartupHasNoReviewPhase()
        {
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe" }), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(null), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new string[0]), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-phase6Review" }), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-phase2review" }), Is.Zero);
        }

        [Test]
        public void AuditDriversWinOverReviewFlags()
        {
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-phase1Audit", "out" }), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-phase2Audit", "out" }), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-phase3Review", "-phase3Audit", "out" }), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-phase4Review", "-phase4Audit", "out" }), Is.Zero);
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-phase5Audit", "out" }), Is.Zero);
            // -reviewAudit keeps the Main Menu flow by design; it is not a phase review.
            Assert.That(ReviewDirectBoot.ReviewPhaseFor(new[] { "LayarTerakhir.exe", "-reviewAudit", "out" }), Is.Zero);
        }
    }
}
#endif
