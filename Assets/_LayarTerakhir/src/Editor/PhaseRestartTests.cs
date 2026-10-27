#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase2;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Phase5;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    // Pause-menu RESTART returns to the current phase entry and resets its
    // gameplay state, in normal play and in Phase Review Mode. Progression
    // from earlier phases must survive the round trip.
    public sealed class PhaseRestartTests
    {
        private CampaignState c;
        private string folder, previous;
        private string Slot => Path.Combine(folder, "layar_terakhir_save.json");
        [SetUp] public void Setup()
        {
            previous = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            folder = Path.Combine(Path.GetTempPath(), "Restart-" + Guid.NewGuid()); Directory.CreateDirectory(folder);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", folder);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            c = new GameObject("Campaign").AddComponent<CampaignState>();
            typeof(CampaignState).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(c, null);
        }
        [TearDown] public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", previous);
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
        }
        private void RestartToEntry(string checkpoint, int chapter)
        {
            Assert.That(c.RestorePhaseEntry(), Is.True, "Entry snapshot must restore");
            Assert.That(c.Data.checkpoint_id, Is.EqualTo(checkpoint));
            Assert.That(c.Data.chapter_id, Is.EqualTo(chapter));
            Assert.That(c.LoadCheckpoint(), Is.True, "Restored entry must persist on disk");
            Assert.That(c.Data.checkpoint_id, Is.EqualTo(checkpoint));
        }
        [Test] public void PrologRestartReturnsToKosWake()
        {
            Assert.That(c.BeginNewCampaignPreservingPrevious(), Is.True);
            Assert.That(c.HasPhaseEntry(), Is.True, "New Game must freeze the prolog entry");
            c.Data.inventory.Add("MID_PROLOG_ITEM"); c.SaveCheckpointAtomic("PROLOG_ATMAJA_KITCHEN");
            RestartToEntry("PROLOG_KOS_START", 0);
            Assert.That(c.Data.inventory, Is.Empty);
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(GameShell.KosScene));
        }
        [Test] public void Phase1RestartReturnsToRusunawaArrival()
        {
            Assert.That(c.BeginNewCampaignPreservingPrevious(), Is.True);
            c.Data.checkpoint_id = "PROLOG_COMPLETE"; Assert.That(c.SaveCheckpointAtomic("PROLOG_COMPLETE"), Is.True);
            Assert.That(Phase1Director.PrepareNextNight(c), Is.True);
            Assert.That(c.HasPhaseEntry(), Is.True, "P1 entry must be frozen");
            c.Data.phase1.Add("P1_MID"); c.Data.battery = 50; c.SaveCheckpointAtomic("PHASE1_CP3");
            RestartToEntry("PHASE1_CP1", 1);
            Assert.That(c.Data.phase1.Completed("P1_MID"), Is.False);
            Assert.That(c.Data.battery, Is.EqualTo(100));
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(CampaignRouting.RusunawaScene));
        }
        [Test] public void Phase2RestartReturnsToKosHubArrival()
        {
            c.Data.chapter_id = 1; c.Data.checkpoint_id = "PHASE1_CP7";
            c.Data.phase1.Add("P1_COMPLETE"); c.Data.location_seals.Add(Phase1Ids.Seal);
            Assert.That(Phase2Campaign.EnterHub(c), Is.True);
            Assert.That(c.HasPhaseEntry(), Is.True, "P2 entry must be frozen");
            Assert.That(Phase2Campaign.Depart(c), Is.True, "Departure simulates hub progress");
            RestartToEntry("PHASE2_HUB", 2);
            Assert.That(c.Data.phase2.Completed("P2_DEPART_KOS"), Is.False);
            Assert.That(c.Data.location_seals, Does.Contain(Phase1Ids.Seal), "Earlier seals survive");
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(GameShell.KosScene));
        }
        [Test] public void Phase3RestartReturnsToKosHubArrival()
        {
            c.Data.chapter_id = 2; c.Data.checkpoint_id = "PHASE2_CP7"; c.Data.phase2.Add("P2_COMPLETE");
            c.Data.location_seals.AddRange(new[] { Phase1Ids.Seal, Phase2Ids.Seal });
            Assert.That(Phase3Campaign.EnterHub(c), Is.True);
            Assert.That(Phase3Campaign.Depart(c), Is.True, "Departure simulates hub progress");
            RestartToEntry("PHASE3_HUB", 3);
            Assert.That(c.Data.phase3.Completed("P3_DEPART_KOS"), Is.False);
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(GameShell.KosScene));
        }
        [Test] public void Phase4RestartReturnsToKosHubArrival()
        {
            c.Data.chapter_id = 3; c.Data.checkpoint_id = "PHASE3_CP9"; c.Data.phase3.Add("P3_COMPLETE");
            c.Data.location_seals.AddRange(new[] { Phase1Ids.Seal, Phase2Ids.Seal, Phase3Ids.Seal });
            Assert.That(Phase4Campaign.EnterHub(c), Is.True);
            Assert.That(Phase4Campaign.Depart(c), Is.True, "Departure simulates hub progress");
            RestartToEntry("PHASE4_HUB", 4);
            Assert.That(c.Data.phase4.Completed("P4_DEPART_KOS"), Is.False);
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(GameShell.KosScene));
        }
        [Test] public void Phase5RestartReturnsToMarketGate()
        {
            File.Copy(Path.Combine(Application.dataPath, "Resources/Phase5ReviewBoundary.json"), Slot);
            Assert.That(c.LoadCheckpoint(), Is.True);
            Assert.That(Phase5Campaign.Enter(c), Is.True);
            Assert.That(c.HasPhaseEntry(), Is.True, "P5 entry must be frozen");
            Assert.That(Phase5Campaign.EnterSectors(c), Is.True, "Sector entry simulates market progress");
            RestartToEntry("PHASE5_CP1", 5);
            Assert.That(c.Data.phase5.Completed("P5_ENTER_SECTORS"), Is.False);
            Assert.That(Phase5Ids.FourSeals(c.Data), Is.True, "All four seals survive");
            Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(CampaignRouting.MarketScene));
        }
        [Test] public void MissingEntryDisablesRestartWithoutTouchingProgress()
        {
            c.StartNewGame();
            Assert.That(c.HasPhaseEntry(), Is.False);
            Assert.That(c.RestorePhaseEntry(), Is.False);
            Assert.That(c.Data.checkpoint_id, Is.EqualTo("PROLOG_KOS_START"));
        }
        [Test] public void ReviewEntrySnapshotNeverOverwrites()
        {
            c.Data.chapter_id = 2; c.Data.checkpoint_id = "PHASE2_HUB"; c.Data.phase2.Add("P2_ARRIVE_KOS");
            Assert.That(c.SaveCheckpointAtomic("PHASE2_HUB"), Is.True);
            c.SnapshotPhaseEntry(false);
            c.Data.checkpoint_id = "PHASE2_CP5"; Assert.That(c.SaveCheckpointAtomic("PHASE2_CP5"), Is.True);
            c.SnapshotPhaseEntry(false);
            RestartToEntry("PHASE2_HUB", 2);
        }
    }
}
#endif
