#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase2;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Text.RegularExpressions;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase2RitualTransactionTests
    {
        private CampaignState c;private string folder,previous;
        [SetUp]public void Setup()
        {
            previous=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");folder=Path.Combine(Path.GetTempPath(),"P2-Ritual-"+Guid.NewGuid());
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            c=new GameObject("Campaign").AddComponent<CampaignState>();typeof(CampaignState).GetMethod("Awake",BindingFlags.NonPublic|BindingFlags.Instance).Invoke(c,null);c.StartNewGame();
            var d=c.Data;d.chapter_id=2;d.checkpoint_id="PHASE2_CP4";d.phase1.Add("P1_COMPLETE");d.location_seals.Add(Phase1Ids.Seal);
            d.phase2.safe_anchor="NURSE";d.phase2.Add("P2_DEPART_KOS");d.phase2.Add("P2_JOURNAL_CONFIRMED");
            d.evidence_ids.AddRange(Phase2Ids.Evidence);d.inventory.AddRange(Phase2Ids.Relics);d.relic_ids.AddRange(Phase2Ids.Relics);
            d.inventory.AddRange(new[]{"P2_FLARE_A","P2_FLARE_B","P2_FLARE_C","P2_POWERBANK_PASAR_GAIB_A","P2_KEMENYAN_A"});d.sanity=0;d.wallet_legitimate=1234;d.wallet_bound=6000;d.battery=44;
        }
        [TearDown]public void Cleanup()
        {EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",previous);if(Directory.Exists(folder))Directory.Delete(folder,true);}
        [TestCase(0)][TestCase(1)][TestCase(14)][TestCase(15)][TestCase(29)][TestCase(44)][TestCase(45)][TestCase(80)][TestCase(100)]
        public void CP5PreservesEveryEnergyBoundaryAndRelicsWithoutStartingAttempt(float battery)
        {
            c.Data.battery=battery;Assert.That(Phase2Campaign.PrepareRitual(c),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.battery,Is.EqualTo(battery));Assert.That(c.Data.phase2.safe_anchor,Is.EqualTo("OR_PREP"));Assert.That(Phase2Ids.RitualReady(c.Data),Is.True);
            Assert.That(c.Data.phase2.Completed("P2_TABLE_INTERACT"),Is.False);Assert.That(c.Data.phase2.Completed("P2_BOSS_WIN"),Is.False);
        }
        [TestCase("journal")][TestCase("evidence")][TestCase("relic")]
        public void TableGateRejectsMissingExactRequirementsWithoutWriting(string missing)
        {
            if(missing=="journal")c.Data.phase2.completed_event_ids.Remove("P2_JOURNAL_CONFIRMED");
            if(missing=="evidence"){c.Data.evidence_ids.Remove(Phase2Ids.EVP);c.Data.evidence_ids.Add("OTHER_EVIDENCE");}
            if(missing=="relic")c.Data.inventory.Remove(Phase2Ids.Gelang);
            string before=JsonUtility.ToJson(c.Data);Assert.That(Phase2Campaign.PrepareRitual(c),Is.False);Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before));
        }
        [Test]public void TwentyFailedAttemptsAlwaysRestoreExactCP5AndBlockEveryOrdinaryWrite()
        {
            Assert.That(Phase2Campaign.PrepareRitual(c),Is.True);string snapshot=JsonUtility.ToJson(c.Data);
            for(int attempt=0;attempt<20;attempt++)
            {
                object owner=new object();Assert.That(c.BeginCheckpointAttempt(owner),Is.True);c.Data.battery=0;c.Data.inventory.Remove("P2_POWERBANK_PASAR_GAIB_A");
                Assert.That(c.TryCommit("BAD",d=>d.wallet_legitimate+=99),Is.False);Assert.That(c.SaveCheckpointAtomic("BAD"),Is.False);
                Assert.That(c.ArchiveDocument("BAD","BAD"),Is.False);Assert.That(Phase2Campaign.Finish(c),Is.False);
                Assert.That(Phase2Campaign.WinRitual(c,new object(),3,new[]{"P2_FLARE_A"}),Is.False);
                c.EndCheckpointAttempt(owner);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(snapshot));
            }
        }
        [TestCase(0)][TestCase(1)][TestCase(2)][TestCase(4)]
        public void IncompleteOrDuplicateCycleCountsCannotAwardSeal(int layers)
        {Phase2Campaign.PrepareRitual(c);object owner=new object();c.BeginCheckpointAttempt(owner);Assert.That(Phase2Campaign.WinRitual(c,owner,layers,Array.Empty<string>()),Is.False);Assert.That(c.Data.location_seals,Does.Not.Contain(Phase2Ids.Seal));c.EndCheckpointAttempt(owner);}
        [Test]public void CP6AtomicallyConsumesOnlyUsedFlaresAndMovesRelicsWhilePreservingEconomyAndSanity()
        {
            Phase2Campaign.PrepareRitual(c);object owner=new object();c.BeginCheckpointAttempt(owner);c.Data.battery=0;
            Assert.That(Phase2Campaign.WinRitual(c,owner,3,new[]{"P2_FLARE_B"}),Is.True);c.EndCheckpointAttempt(owner);Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.location_seals.Count(id=>id==Phase2Ids.Seal),Is.EqualTo(1));Assert.That(c.Data.inventory.Intersect(Phase2Ids.Relics),Is.Empty);
            Assert.That(c.Data.relic_ids,Is.EquivalentTo(Phase2Ids.Relics));Assert.That(c.Data.inventory,Does.Not.Contain("P2_FLARE_B"));
            Assert.That(c.Data.inventory,Does.Contain("P2_FLARE_A"));Assert.That(c.Data.inventory,Does.Contain("P2_FLARE_C"));Assert.That(c.Data.inventory,Does.Contain("P2_KEMENYAN_A"));
            Assert.That(c.Data.battery,Is.Zero);Assert.That(c.Data.sanity,Is.Zero);Assert.That(c.Data.wallet_legitimate,Is.EqualTo(1234));Assert.That(c.Data.wallet_bound,Is.EqualTo(6000));
        }
        [Test]public void FailedCP6WriteLeavesDiskCP5AndCanRetrySameVictoryWithoutHalfSeal()
        {
            Phase2Campaign.PrepareRitual(c);string disk=File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json"));object owner=new object();c.BeginCheckpointAttempt(owner);
            string blocked=Path.Combine(folder,"blocked");File.WriteAllText(blocked,"disk failure fixture");Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",blocked);
            LogAssert.Expect(LogType.Error,new Regex("Gagal menyimpan checkpoint secara atomik"));
            Assert.That(Phase2Campaign.WinRitual(c,owner,3,new[]{"P2_FLARE_A"}),Is.False);Assert.That(Phase2Ids.RitualReady(c.Data),Is.True);Assert.That(c.Data.location_seals,Does.Not.Contain(Phase2Ids.Seal));
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);Assert.That(File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json")),Is.EqualTo(disk));
            Assert.That(Phase2Campaign.WinRitual(c,owner,3,new[]{"P2_FLARE_A"}),Is.True);c.EndCheckpointAttempt(owner);c.LoadCheckpoint();Assert.That(c.Data.phase2.Completed("P2_BOSS_WIN"),Is.True);
        }
        [Test]public void CP7RequiresEpilogueAndRetriesAtomicFailureWithoutDuplicatingSealOrMoney()
        {
            Phase2Campaign.PrepareRitual(c);var owner=new object();c.BeginCheckpointAttempt(owner);Phase2Campaign.WinRitual(c,owner,3,Array.Empty<string>());c.EndCheckpointAttempt(owner);
            Assert.That(Phase2Campaign.Finish(c),Is.False);Assert.That(c.TryCommit("PHASE2_CP6",d=>d.phase2.Add("P2_EPILOGUE")),Is.True);
            string blocked=Path.Combine(folder,"blocked");File.WriteAllText(blocked,"write failure");Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",blocked);
            LogAssert.Expect(LogType.Error,new Regex("Gagal menyimpan checkpoint secara atomik"));
            Assert.That(Phase2Campaign.Finish(c),Is.False);Assert.That(c.Data.phase2.Completed("P2_COMPLETE"),Is.False);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);Assert.That(Phase2Campaign.Finish(c),Is.True);Assert.That(Phase2Campaign.Finish(c),Is.False);Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE2_CP7"));Assert.That(c.Data.phase2.next_destination,Is.EqualTo(Phase2Ids.Destination));Assert.That(c.Data.location_seals.Count,Is.EqualTo(2));
            Assert.That(c.Data.wallet_legitimate,Is.EqualTo(1234));Assert.That(c.Data.sanity,Is.Zero);Assert.That(Phase2Campaign.InvestigationCheckpoint(c.Data,"PHASE2_CP3"),Is.EqualTo("PHASE2_CP7"));
        }
    }
}
#endif
