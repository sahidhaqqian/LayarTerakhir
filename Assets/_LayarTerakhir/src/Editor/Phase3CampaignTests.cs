#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase3;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase3CampaignTests
    {
        private CampaignState c;private string folder,previous;
        [SetUp] public void Setup()
        {
            previous=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");folder=Path.Combine(Path.GetTempPath(),"P3-"+Guid.NewGuid());Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);c=new GameObject("Campaign").AddComponent<CampaignState>();typeof(CampaignState).GetMethod("Awake",BindingFlags.NonPublic|BindingFlags.Instance).Invoke(c,null);c.StartNewGame();
            var d=c.Data;d.chapter_id=3;d.checkpoint_id="PHASE3_CP4";d.phase1.Add("P1_COMPLETE");d.phase2.Add("P2_COMPLETE");d.location_seals.AddRange(new[]{Phase1Ids.Seal,Phase2Ids.Seal});
            d.phase3.safe_anchor="BOILER";d.phase3.Add("P3_DEPART_KOS");d.phase3.Add("P3_JOURNAL_CONFIRMED");d.evidence_ids.AddRange(Phase3Ids.Evidence);d.relic_ids.AddRange(Phase3Ids.Relics);d.inventory.AddRange(Phase3Ids.Relics);
            d.inventory.AddRange(new[]{"P3_FLARE_A","P3_FLARE_B","P2_FLARE_C","P3_POWERBANK_A","P3_KEMENYAN_A"});d.battery=44;d.sanity=23;d.wallet_legitimate=1234;d.wallet_bound=6000;
        }
        [TearDown] public void Cleanup(){EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",previous);if(Directory.Exists(folder))Directory.Delete(folder,true);}
        [Test] public void NewGameDoesNotInheritPhase3AndSavedContinueRemainsExplicit()
        {
            Assert.That(c.SaveCheckpointAtomic("PHASE3_CP4"),Is.True);
            c.StartNewGame();Assert.That(c.Data.chapter_id,Is.Zero);Assert.That(c.Data.phase3.completed_event_ids,Is.Empty);
            Assert.That(c.Data.evidence_ids.Intersect(Phase3Ids.Evidence),Is.Empty);
            Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.chapter_id,Is.EqualTo(3));
        }
        [Test] public void NearbyIntercomWinsOverDistantHigherPriorityVent()
        {
            var intercom=new GameObject("Intercom").AddComponent<Phase3EvpSource>();intercom.priority=10;intercom.tag=Phase3VoiceTag.Mimic;intercom.transform.position=new Vector3(0,1,1.4f);
            var vent=new GameObject("Vent").AddComponent<Phase3EvpSource>();vent.priority=20;vent.tag=Phase3VoiceTag.Authentic;vent.transform.position=new Vector3(0,1,9);
            Assert.That(intercom.FocusScore(Vector3.up),Is.GreaterThan(vent.FocusScore(Vector3.up)));
            Assert.That(vent.FocusScore(new Vector3(0,1,8)),Is.GreaterThan(intercom.FocusScore(new Vector3(0,1,8))));
        }
        [Test] public void InvalidFactoryAnchorIsRejectedAndExplicitBackupRetainsProgress()
        {
            c.Data.upgrades.AddRange(new[]{"UV_RAJAH","EVP_V2"});c.Data.secret_items.Add(Phase3Ids.Secret);
            Assert.That(c.SaveCheckpointAtomic("PHASE3_CP4"),Is.True);Assert.That(c.SaveCheckpointAtomic("PHASE3_CP4"),Is.True);
            var broken=JsonUtility.FromJson<CampaignData>(JsonUtility.ToJson(c.Data));broken.phase3.safe_anchor="NONEXISTENT_LEDGE";
            File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"),JsonUtility.ToJson(broken));
            Assert.That(c.LoadCheckpoint(),Is.False);Assert.That(c.LoadBackupCheckpoint(),Is.True);Assert.That(c.Data.phase3.safe_anchor,Is.EqualTo("BOILER"));
            Assert.That(c.Data.upgrades,Does.Contain("EVP_V2"));Assert.That(c.Data.secret_items.Count(x=>x==Phase3Ids.Secret),Is.EqualTo(1));
        }
        [Test] public void ShopWriteFailureRollsBackBalanceItemsAndReceiptTogether()
        {
            c.Data.chapter_id=2;c.Data.checkpoint_id="PHASE2_CP7";Assert.That(Phase3Campaign.EnterHub(c),Is.True);
            c.Data.inventory.RemoveAll(id=>id.StartsWith("P3_"));c.Data.wallet_legitimate=750000;Assert.That(c.SaveCheckpointAtomic("PHASE3_HUB"),Is.True);
            string before=JsonUtility.ToJson(c.Data),disk=File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json"));
            string blocked=Path.Combine(folder,"blocked-write");File.WriteAllText(blocked,"fault");Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",blocked);
            LogAssert.Expect(LogType.Error,new Regex("Gagal menyimpan checkpoint secara atomik"));Assert.That(Phase3Campaign.Buy(c,"POWERBANK"),Is.False);
            Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before));Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            Assert.That(File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json")),Is.EqualTo(disk));
        }
        [TestCase(0)][TestCase(1)][TestCase(14)][TestCase(15)][TestCase(29)][TestCase(44)][TestCase(45)][TestCase(80)][TestCase(100)]
        public void CP5PreservesExactEnergyAndInventory(float battery)
        {
            c.Data.battery=battery;Assert.That(Phase3Campaign.PrepareRitual(c),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.battery,Is.EqualTo(battery));Assert.That(c.Data.phase3.safe_anchor,Is.EqualTo("ALTAR"));Assert.That(Phase3Ids.RitualReady(c.Data),Is.True);Assert.That(c.Data.sanity,Is.EqualTo(23));
        }
        [TestCase("journal")][TestCase("evidence")][TestCase("relic")]
        public void ExactGateRequirementsCannotBeSubstituted(string missing)
        {
            if(missing=="journal")c.Data.phase3.completed_event_ids.Remove("P3_JOURNAL_CONFIRMED");
            if(missing=="evidence"){c.Data.evidence_ids.Remove(Phase3Ids.EVP);c.Data.evidence_ids.Add("P3_FAKE");}
            if(missing=="relic")c.Data.inventory.Remove(Phase3Ids.Doll);
            string before=JsonUtility.ToJson(c.Data);Assert.That(Phase3Campaign.PrepareRitual(c),Is.False);Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before));
        }
        [Test] public void TwentyAbortedAttemptsRestoreWholeCP5AndBlockEveryOrdinaryWrite()
        {
            Assert.That(Phase3Campaign.PrepareRitual(c),Is.True);string snapshot=JsonUtility.ToJson(c.Data);
            for(int i=0;i<20;i++)
            {
                object owner=new object();Assert.That(c.BeginCheckpointAttempt(owner),Is.True);c.Data.battery=0;c.Data.inventory.Remove("P3_POWERBANK_A");
                Assert.That(c.TryCommit("BAD",d=>d.wallet_legitimate+=99),Is.False);Assert.That(c.SaveCheckpointAtomic("BAD"),Is.False);Assert.That(c.ArchiveDocument("BAD","BAD"),Is.False);
                Assert.That(Phase3Campaign.WinRitual(c,new object(),3,new[]{"P3_FLARE_A"}),Is.False);c.EndCheckpointAttempt(owner);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(snapshot));
            }
        }
        [Test] public void VictoryConsumesOnlyAttemptFlaresAndKeepsOldChapterSealsEconomyAndSanity()
        {
            Phase3Campaign.PrepareRitual(c);object owner=new object();c.BeginCheckpointAttempt(owner);c.Data.battery=0;
            Assert.That(Phase3Campaign.WinRitual(c,owner,3,new[]{"P3_FLARE_B","P2_FLARE_C"}),Is.True);c.EndCheckpointAttempt(owner);Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.location_seals,Is.EquivalentTo(new[]{Phase1Ids.Seal,Phase2Ids.Seal,Phase3Ids.Seal}));Assert.That(c.Data.inventory.Intersect(Phase3Ids.Relics),Is.Empty);
            Assert.That(c.Data.inventory,Does.Contain("P3_FLARE_A"));Assert.That(c.Data.inventory,Does.Not.Contain("P3_FLARE_B"));Assert.That(c.Data.inventory,Does.Not.Contain("P2_FLARE_C"));Assert.That(c.Data.inventory,Does.Contain("P3_KEMENYAN_A"));
            Assert.That(c.Data.battery,Is.Zero);Assert.That(c.Data.sanity,Is.EqualTo(23));Assert.That(c.Data.wallet_legitimate,Is.EqualTo(1234));Assert.That(c.Data.wallet_bound,Is.EqualTo(6000));
        }
        [Test] public void CP6WriteFaultRollsBackEveryFieldAndCanRetry()
        {
            Phase3Campaign.PrepareRitual(c);string disk=File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json"));object owner=new object();c.BeginCheckpointAttempt(owner);
            string blocked=Path.Combine(folder,"blocked");File.WriteAllText(blocked,"disk failure");Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",blocked);
            LogAssert.Expect(LogType.Error,new Regex("Gagal menyimpan checkpoint secara atomik"));Assert.That(Phase3Campaign.WinRitual(c,owner,3,new[]{"P3_FLARE_A"}),Is.False);Assert.That(Phase3Ids.RitualReady(c.Data),Is.True);Assert.That(c.Data.location_seals,Does.Not.Contain(Phase3Ids.Seal));
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);Assert.That(File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json")),Is.EqualTo(disk));Assert.That(Phase3Campaign.WinRitual(c,owner,3,new[]{"P3_FLARE_A"}),Is.True);c.EndCheckpointAttempt(owner);
        }
        [Test] public void ExitNeedsEpilogueAndCP7FailureDoesNotCompleteOrDuplicate()
        {
            Phase3Campaign.PrepareRitual(c);object owner=new object();c.BeginCheckpointAttempt(owner);Phase3Campaign.WinRitual(c,owner,3,Array.Empty<string>());c.EndCheckpointAttempt(owner);
            Assert.That(Phase3Campaign.Finish(c),Is.False);c.TryCommit("PHASE3_CP6",d=>d.phase3.Add("P3_EPILOGUE"));
            string blocked=Path.Combine(folder,"blocked");File.WriteAllText(blocked,"fault");Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",blocked);LogAssert.Expect(LogType.Error,new Regex("Gagal menyimpan checkpoint secara atomik"));
            Assert.That(Phase3Campaign.Finish(c),Is.False);Assert.That(c.Data.phase3.Completed("P3_COMPLETE"),Is.False);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            Assert.That(Phase3Campaign.Finish(c),Is.True);Assert.That(Phase3Campaign.Finish(c),Is.False);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE3_CP7"));Assert.That(c.Data.phase3.next_destination,Is.EqualTo(Phase3Ids.Destination));
        }
        [TestCase(1)][TestCase(2)][TestCase(3)] public void LegacySchemasMigrateWithoutLosingCampaign(int schema)
        {
            var d=c.Data;d.schema_version=schema;d.chapter_id=2;d.checkpoint_id="PHASE2_CP7";d.inventory.Add("EVENT_P0_CHAIN3_BROKEN");d.secret_items.Add(Phase3Ids.Secret);
            Directory.CreateDirectory(folder);File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"),JsonUtility.ToJson(d));Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.schema_version,Is.EqualTo(5));Assert.That(c.Data.battery,Is.EqualTo(44));Assert.That(c.Data.inventory,Does.Contain("EVENT_P0_CHAIN3_BROKEN"));Assert.That(c.Data.secret_items,Does.Contain(Phase3Ids.Secret));
        }
        [Test] public void AliasesNormalizeWithoutDoubleEvidenceOrRelics()
        {
            c.Data.inventory.Add("P3_BONEKA_JERAMI_TEBU");c.Data.evidence_ids.Add("P3_AR_GENDONGAN");Assert.That(c.SaveCheckpointAtomic("PHASE3_CP4"),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.evidence_ids.Count(x=>x==Phase3Ids.Sling),Is.EqualTo(1));Assert.That(c.Data.evidence_ids,Does.Not.Contain("P3_AR_GENDONGAN"));Assert.That(c.Data.inventory.Count(x=>x==Phase3Ids.Doll),Is.EqualTo(1));
        }
        [Test] public void EnterHubRequiresRealPreviousChapterReceiptsAndBothSeals()
        {
            c.Data.chapter_id=2;c.Data.checkpoint_id="PHASE2_CP7";c.Data.phase2.completed_event_ids.Remove("P2_COMPLETE");Assert.That(Phase3Campaign.EnterHub(c),Is.False);
            c.Data.phase2.Add("P2_COMPLETE");c.Data.location_seals.Remove(Phase2Ids.Seal);Assert.That(Phase3Campaign.EnterHub(c),Is.False);c.Data.location_seals.Add(Phase2Ids.Seal);
            Assert.That(Phase3Campaign.EnterHub(c),Is.True);Assert.That(CampaignRouting.ContinueScene(c.Data),Is.EqualTo("Prolog_KamarKos"));Assert.That(c.Data.sanity,Is.EqualTo(23));
        }
        [Test] public void ZeroWalletUnlockAndExactDepartureDoNotInventResources()
        {
            c.Data.chapter_id=2;c.Data.checkpoint_id="PHASE2_CP7";Phase3Campaign.EnterHub(c);c.Data.wallet_legitimate=0;c.Data.battery=14;
            Assert.That(Phase3Campaign.Buy(c,"FLARE"),Is.False);Assert.That(Phase3Campaign.Depart(c),Is.True);Assert.That(c.Data.battery,Is.EqualTo(14));Assert.That(c.Data.upgrades,Does.Contain("UV_RAJAH"));Assert.That(CampaignRouting.ContinueScene(c.Data),Is.EqualTo(CampaignRouting.FactoryScene));
        }
        [TestCase("POWERBANK",750000,3)][TestCase("FLARE",1000000,4)][TestCase("KEMENYAN",350000,2)]
        public void PackPurchaseIsOneTransactionAndOneReceipt(string pack,long price,int count)
        {
            c.Data.chapter_id=2;c.Data.checkpoint_id="PHASE2_CP7";Phase3Campaign.EnterHub(c);c.Data.inventory.RemoveAll(id=>id.StartsWith("P3_"));c.Data.wallet_legitimate=2100000;int before=c.Data.inventory.Count;
            Assert.That(Phase3Campaign.Buy(c,pack),Is.True);Assert.That(c.Data.inventory.Count-before,Is.EqualTo(count));Assert.That(Phase3Campaign.Buy(c,pack),Is.False);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.wallet_legitimate,Is.EqualTo(2100000-price));
        }
        [TestCase("PHASE3_CP4","PHASE3_CP2")][TestCase("PHASE3_CP4","PHASE3_CP3")][TestCase("PHASE3_CP5","PHASE3_CP4")]
        public void BacktrackingCannotRegressCheckpoint(string old,string desired)
        {c.Data.checkpoint_id=old;Assert.That(Phase3Campaign.InvestigationCheckpoint(c.Data,desired),Is.EqualTo(old));}
        [TestCase(0,Phase3VoiceTag.Mimic)][TestCase(7.9f,Phase3VoiceTag.Mimic)][TestCase(8,Phase3VoiceTag.Authentic)][TestCase(10.99f,Phase3VoiceTag.Authentic)][TestCase(11,Phase3VoiceTag.Mimic)]
        public void IntercomMetadataIsDeterministicAndNeverUsesMicrophone(float seconds,Phase3VoiceTag expected)
        {var s=new GameObject("EVP").AddComponent<Phase3EvpSource>();s.intercom=true;var b=ScriptableObject.CreateInstance<Phase3Balance>();Assert.That(s.Tag(seconds,b),Is.EqualTo(expected));UnityEngine.Object.DestroyImmediate(b);}
    }
}
#endif
