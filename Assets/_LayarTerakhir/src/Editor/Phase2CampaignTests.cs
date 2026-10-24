#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase2;
using LayarTerakhir.Smartphone;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase2CampaignTests
    {
        private string folder,previous;
        private CampaignState campaign;
        [SetUp]public void Setup()
        {
            previous=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            folder=Path.Combine(Path.GetTempPath(),"LT-P2-"+Guid.NewGuid());Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);campaign=new GameObject("Campaign").AddComponent<CampaignState>();
            typeof(CampaignState).GetMethod("Awake",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(campaign,null);campaign.StartNewGame();
        }
        [TearDown]public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",previous);
            if(Directory.Exists(folder))Directory.Delete(folder,true);
        }
        private void CompletedP1()
        {
            campaign.Data.chapter_id=1;campaign.Data.checkpoint_id="PHASE1_CP7";
            campaign.Data.phase1.Add("P1_COMPLETE");campaign.Data.location_seals.Add(Phase1Ids.Seal);
        }
        [Test]public void AtomicPhotoOrRestoreAtZeroTurnsScreenOffAndRecoveryDoesNotAutoRaiseIt()
        {
            var phone=new GameObject("Existing handset").AddComponent<LayarTerakhir.Smartphone.SmartphoneController>();
            phone.isPhoneRaised=phone.isScreenOn=true;phone.currentScanMode=ScanVisualMode.AR;
            phone.RestoreBattery(0);
            Assert.That(phone.isScreenOn,Is.False,"A zero-energy photo must permit screen-off hiding");
            Assert.That(phone.currentScanMode,Is.EqualTo(ScanVisualMode.Normal));
            phone.isPhoneRaised=false;phone.RestoreBattery(10);
            Assert.That(phone.isPhoneRaised,Is.False);Assert.That(phone.isScreenOn,Is.False);
            Assert.That(phone.batteryPercent,Is.EqualTo(10));
        }
        [TestCase(0)][TestCase(1)][TestCase(14)][TestCase(67)][TestCase(100)]
        public void HubAndDeparturePreserveActualCarryOverAndNeverGrantCharge(float battery)
        {
            CompletedP1();campaign.Data.battery=battery;campaign.Data.sanity=0;campaign.Data.wallet_legitimate=321;campaign.Data.wallet_bound=12345;
            campaign.Data.inventory.Add("P0_ITEM");campaign.Data.evidence_ids.Add("P1_EVIDENCE");campaign.Data.secret_items.Add(Phase1Ids.Secret);
            Assert.That(Phase2Campaign.EnterHub(campaign),Is.True);Assert.That(campaign.Data.checkpoint_id,Is.EqualTo("PHASE2_HUB"));
            Assert.That(CampaignRouting.ContinueScene(campaign.Data),Is.EqualTo("Prolog_KamarKos"));
            Assert.That(Phase2Campaign.Depart(campaign),Is.True);Assert.That(campaign.LoadCheckpoint(),Is.True);
            Assert.That(campaign.Data.battery,Is.EqualTo(battery));Assert.That(campaign.Data.sanity,Is.Zero);
            Assert.That(campaign.Data.wallet_legitimate,Is.EqualTo(321));Assert.That(campaign.Data.wallet_bound,Is.EqualTo(12345));
            Assert.That(campaign.Data.inventory,Is.EquivalentTo(new[]{"P0_ITEM"}));Assert.That(campaign.Data.secret_items,Does.Contain(Phase1Ids.Secret));
            Assert.That(campaign.Data.upgrades,Does.Contain("SELFIE_MODE"));Assert.That(campaign.Data.upgrades,Does.Contain("THERMOKLENIK_APK"));
            Assert.That(CampaignRouting.ContinueScene(campaign.Data),Is.EqualTo("Babak2_RSUD"));
            Assert.That(Phase2Campaign.Depart(campaign),Is.False);Assert.That(Phase2Campaign.EnterHub(campaign),Is.False);
        }
        [TestCase(true,false)][TestCase(false,true)][TestCase(false,false)]
        public void HubRejectsMissingPrerequisitesWithoutMutation(bool complete,bool seal)
        {
            campaign.Data.chapter_id=1;if(complete)campaign.Data.phase1.Add("P1_COMPLETE");if(seal)campaign.Data.location_seals.Add(Phase1Ids.Seal);
            string before=JsonUtility.ToJson(campaign.Data);Assert.That(Phase2Campaign.EnterHub(campaign),Is.False);Assert.That(JsonUtility.ToJson(campaign.Data),Is.EqualTo(before));
        }
        [Test]public void HubChargeIsExplicitOnceAndDoesNotRepeatAfterLoadOrDeparture()
        {
            CompletedP1();campaign.Data.battery=0;Phase2Campaign.EnterHub(campaign);
            Assert.That(Phase2Campaign.ChargeHub(campaign),Is.True);campaign.Data.battery=32;campaign.SaveCheckpointAtomic(campaign.Data.checkpoint_id);
            campaign.LoadCheckpoint();Assert.That(Phase2Campaign.ChargeHub(campaign),Is.False);Phase2Campaign.Depart(campaign);campaign.LoadCheckpoint();
            Assert.That(campaign.Data.battery,Is.EqualTo(32));Assert.That(Phase2Campaign.ChargeHub(campaign),Is.False);
        }
        [Test]public void ShopUsesLegitimateWalletAndUniqueInstancesWithNoRepeatPurchase()
        {
            CompletedP1();campaign.Data.wallet_bound=9000000;Phase2Campaign.EnterHub(campaign);
            Assert.That(Phase2Campaign.Buy(campaign,"POWERBANK"),Is.False);
            campaign.Data.wallet_legitimate=1600000;
            foreach(string pack in new[]{"POWERBANK","KEMENYAN","FLARE"})Assert.That(Phase2Campaign.Buy(campaign,pack),Is.True);
            Assert.That(campaign.Data.wallet_legitimate,Is.Zero);Assert.That(campaign.Data.spent,Is.EqualTo(1600000));Assert.That(campaign.Data.wallet_bound,Is.EqualTo(9000000));
            Assert.That(campaign.LoadCheckpoint(),Is.True);Assert.That(campaign.Data.inventory.Count(id=>id.StartsWith("P2_POWERBANK")),Is.EqualTo(2));
            Assert.That(campaign.Data.inventory.Count,Is.EqualTo(7));Assert.That(campaign.Data.phase2.money_ledger.Count,Is.EqualTo(3));
            Assert.That(Phase2Campaign.Buy(campaign,"POWERBANK"),Is.False);
        }
        [Test]public void FailedValidationRollsBackWholeChapterTransactionAndDisk()
        {
            CompletedP1();Phase2Campaign.EnterHub(campaign);string before=JsonUtility.ToJson(campaign.Data);
            Assert.That(campaign.TryCommit("PHASE2_CP4",d=>{d.phase2.Add("BAD");d.inventory.Add("BAD");d.phase2.safe_anchor="";}),Is.False);
            Assert.That(JsonUtility.ToJson(campaign.Data),Is.EqualTo(before));campaign.LoadCheckpoint();Assert.That(campaign.Data.checkpoint_id,Is.EqualTo("PHASE2_HUB"));
        }
        [Test]public void SchemaTwoMigrationPreservesEveryP1AndSharedField()
        {
            CompletedP1();campaign.Data.battery=19;campaign.Data.phase1.safe_anchor="PARKING";campaign.Data.phase1.dynamo_full_charge_used_ids.Add("PUMP");
            campaign.Data.secret_items.Add(Phase1Ids.Secret);campaign.Data.documents.Add(new PhoneDocument("P1","test"));campaign.Data.inventory.Add("P0_FLAG");
            campaign.Data.phase1.money_ledger.Add(new Phase1MoneyReceipt{id="P1_TX",amount=22});
            Directory.CreateDirectory(folder);string json=JsonUtility.ToJson(campaign.Data).Replace("\"schema_version\":3","\"schema_version\":2");
            File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"),json);
            Assert.That(campaign.LoadCheckpoint(),Is.True);Assert.That(campaign.Data.schema_version,Is.EqualTo(3));
            Assert.That(campaign.Data.phase1.Completed("P1_COMPLETE"),Is.True);Assert.That(campaign.Data.phase1.safe_anchor,Is.EqualTo("PARKING"));
            Assert.That(campaign.Data.phase1.money_ledger[0].amount,Is.EqualTo(22));Assert.That(campaign.Data.phase1.dynamo_full_charge_used_ids,Does.Contain("PUMP"));
            Assert.That(campaign.Data.battery,Is.EqualTo(19));Assert.That(campaign.Data.documents[0].title,Is.EqualTo("P1"));Assert.That(campaign.Data.inventory,Does.Contain("P0_FLAG"));
            Assert.That(campaign.Data.secret_items,Does.Contain(Phase1Ids.Secret));Assert.That(campaign.Data.phase2.completed_event_ids,Is.Empty);
        }
        [TestCase("PHASE2_CP2")][TestCase("PHASE2_CP3")]
        public void EarlierGateCannotRegressCheckpointAfterOutOfOrderEvidence(string earlier)
        {campaign.Data.checkpoint_id="PHASE2_CP4";Assert.That(Phase2Campaign.InvestigationCheckpoint(campaign.Data,earlier),Is.EqualTo("PHASE2_CP4"));}
        [Test]public void JournalRequiresExactThreeEvidenceIdsAndExplicitReceipt()
        {
            campaign.Data.inventory.AddRange(Phase2Ids.Relics);campaign.Data.evidence_ids.AddRange(new[]{Phase2Ids.Freeze,Phase2Ids.Aura,"UNRELATED"});
            Assert.That(Phase2Ids.AllEvidence(campaign.Data),Is.False);Assert.That(Phase2Ids.RitualReady(campaign.Data),Is.False);
            campaign.Data.evidence_ids.Add(Phase2Ids.EVP);Assert.That(Phase2Ids.AllEvidence(campaign.Data),Is.True);
            Assert.That(Phase2Ids.RitualReady(campaign.Data),Is.False);campaign.Data.phase2.Add("P2_JOURNAL_CONFIRMED");Assert.That(Phase2Ids.RitualReady(campaign.Data),Is.True);
        }
        [Test]public void UniqueReceiptAndSecretValidationProtectsAgainstDoubleReward()
        {
            campaign.Data.chapter_id=2;campaign.Data.checkpoint_id="PHASE2_CP4";
            campaign.Data.secret_items.Add(Phase2Ids.Secret);Assert.That(campaign.SaveCheckpointAtomic("PHASE2_CP4"),Is.True);
            Assert.That(campaign.TryCommit("PHASE2_CP4",d=>d.secret_items.Add(Phase2Ids.Secret)),Is.False);
            Assert.That(campaign.Data.secret_items.Count,Is.EqualTo(1));
        }
        [Test]public void FutureSchemaCannotBeDowngradedOrLoadedAsNewGame()
        {
            Directory.CreateDirectory(folder);File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"),"{\"schema_version\":99}");
            Assert.That(campaign.LoadCheckpoint(),Is.False);Assert.That(campaign.Data.chapter_id,Is.Zero);
        }
    }
}
#endif
