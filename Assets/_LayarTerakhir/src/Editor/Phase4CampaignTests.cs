#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase4CampaignTests
    {
        private CampaignState c; private string folder, previous;
        [SetUp] public void Setup()
        {
            previous=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");folder=Path.Combine(Path.GetTempPath(),"P4-"+Guid.NewGuid());Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);c=new GameObject("Campaign").AddComponent<CampaignState>();typeof(CampaignState).GetMethod("Awake",BindingFlags.NonPublic|BindingFlags.Instance).Invoke(c,null);c.StartNewGame();
            c.Data.chapter_id=3;c.Data.checkpoint_id="PHASE3_CP7";c.Data.phase3.Add("P3_COMPLETE");c.Data.phase3.Add("P3_BOSS_WIN");c.Data.location_seals.AddRange(new[]{Phase1Ids.Seal,Phase2Ids.Seal,Phase3Ids.Seal});
            c.Data.battery=37;c.Data.sanity=23;c.Data.wallet_legitimate=0;c.Data.wallet_bound=10;c.Data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","P3_KEMENYAN_A","POWERBANK_LARGE_1"});
        }
        [TearDown] public void Cleanup(){EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",previous);if(Directory.Exists(folder))Directory.Delete(folder,true);}
        private void Depart(){Assert.That(Phase4Campaign.EnterHub(c),Is.True);Assert.That(Phase4Campaign.Depart(c),Is.True);}
        private void Prepared(int seed=0)
        {
            Depart();Phase4Campaign.Receipt(c,"P4_PENDOPO_REACHED","PENDOPO","PHASE4_CP3");Phase4Campaign.Receipt(c,"P4_BANASPATI_REVEAL","PENDOPO");foreach(var id in Phase4Ids.Evidence)Assert.That(Phase4Campaign.Evidence(c,id,"PENDOPO",false),Is.True);
            foreach(var id in Phase4Ids.Relics)Assert.That(Phase4Campaign.Relic(c,id,"PENDOPO"),Is.True);
            Assert.That(Phase4Campaign.ConfirmJournal(c,"PENDOPO"),Is.True);Assert.That(Phase4Campaign.BeginRitual(c,seed),Is.True);
        }
        private void Local(int sector)
        {if(c.Data.phase4.water_units==0)Assert.That(Phase4Campaign.Refill(c,"WEST"),Is.True);Assert.That(Phase4Campaign.Pour(c,sector,Phase4Ids.AuthenticIndex(c.Data.phase4.torch_layout_seed,sector),"PENDOPO"),Is.True);c.Data.battery=80;Assert.That(Phase4Campaign.Shot(c,80,65,sector,"PENDOPO"),Is.True);}
        [Test] public void HubAndDeparturePreserveAllCampaignResourcesWithoutPurchases()
        {
            Depart();Assert.That(c.Data.battery,Is.EqualTo(37));Assert.That(c.Data.sanity,Is.EqualTo(23));Assert.That(c.Data.wallet_legitimate,Is.Zero);Assert.That(c.Data.location_seals.Count,Is.EqualTo(3));
            Assert.That(c.Data.inventory,Does.Contain("P3_KEMENYAN_A"));Assert.That(c.Data.upgrades,Does.Contain("SANITY_MONITOR"));Assert.That(CampaignRouting.ContinueScene(c.Data),Is.EqualTo(CampaignRouting.VillageScene));
        }
        [Test] public void HubSanityRestIsAnExplicitPreparationChoiceAndDoesNotChargeBattery()
        {Phase4Campaign.EnterHub(c);Assert.That(c.Data.sanity,Is.EqualTo(23));Assert.That(Phase4Campaign.RestHub(c),Is.True);Assert.That(c.Data.sanity,Is.EqualTo(100));Assert.That(c.Data.battery,Is.EqualTo(37));Assert.That(Phase4Campaign.RestHub(c),Is.False);}
        [Test] public void MissingThirdSealOrPreviousCompletionCannotEnterPhase4()
        {c.Data.location_seals.Remove(Phase3Ids.Seal);Assert.That(Phase4Campaign.EnterHub(c),Is.False);c.Data.location_seals.Add(Phase3Ids.Seal);c.Data.phase3.completed_event_ids.Remove("P3_COMPLETE");Assert.That(Phase4Campaign.EnterHub(c),Is.False);}
        [Test] public void HubChargeIsExplicitOnceAndCannotBeUsedAfterDeparture()
        {Assert.That(Phase4Campaign.EnterHub(c),Is.True);Assert.That(c.Data.battery,Is.EqualTo(37));Assert.That(Phase4Campaign.ChargeHub(c),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(Phase4Campaign.ChargeHub(c),Is.False);Phase4Campaign.Depart(c);Assert.That(Phase4Campaign.ChargeHub(c),Is.False);}
        [TestCase("KEMENYAN",875000,5)][TestCase("POWERBANK",750000,3)][TestCase("WATER",400000,2)]
        public void ShopDebitReceiptAndUniqueInstancesAreOneIdempotentTransaction(string pack,long price,int count)
        {
            Phase4Campaign.EnterHub(c);c.Data.wallet_legitimate=2000000;int old=c.Data.inventory.Count;Assert.That(Phase4Campaign.Buy(c,pack),Is.True);Assert.That(Phase4Campaign.Buy(c,pack),Is.False);Assert.That(c.LoadCheckpoint(),Is.True);
            Assert.That(c.Data.inventory.Count-old,Is.EqualTo(count));Assert.That(c.Data.inventory.Distinct().Count(),Is.EqualTo(c.Data.inventory.Count));Assert.That(c.Data.wallet_legitimate,Is.EqualTo(2000000-price));Assert.That(c.Data.wallet_bound,Is.EqualTo(10));
        }
        [Test] public void EvidenceAndRelicsRemainSeparateAndARCostsExactlyOne()
        {
            Depart();Assert.That(Phase4Campaign.Relic(c,Phase4Ids.Kendi,"NORTH"),Is.True);Assert.That(c.Data.evidence_ids,Is.Empty);Assert.That(Phase4Campaign.Evidence(c,Phase4Ids.Aura,"EAST",true),Is.True);Assert.That(c.Data.battery,Is.EqualTo(36));
            Assert.That(Phase4Campaign.Evidence(c,Phase4Ids.Aura,"EAST",true),Is.True);Assert.That(c.Data.battery,Is.EqualTo(36));Assert.That(c.Data.evidence_ids.Count,Is.EqualTo(1));Assert.That(c.Data.relic_ids.Count,Is.EqualTo(1));Assert.That(Phase4Campaign.ConfirmJournal(c,"PENDOPO"),Is.False);
        }
        [TestCase(0)][TestCase(1)][TestCase(14)][TestCase(15)][TestCase(29)][TestCase(44)][TestCase(45)][TestCase(80)][TestCase(100)]
        public void CP5PreservesExactBatterySanityAndActualWater(float battery)
        {
            Prepared();c.Data.battery=battery;c.Data.phase4.water_units=0;Assert.That(c.SaveCheckpointAtomic("PHASE4_CP5"),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.battery,Is.EqualTo(battery));Assert.That(c.Data.sanity,Is.EqualTo(23));Assert.That(c.Data.phase4.water_units,Is.Zero);
            Assert.That(c.Data.inventory.Count(x=>x==Phase4Ids.Kendi),Is.EqualTo(1));Assert.That(c.Data.inventory,Does.Not.Contain(Phase4Ids.Blade));Assert.That(c.Data.inventory,Does.Not.Contain(Phase4Ids.Soil));
        }
        [TestCase(0)][TestCase(1)][TestCase(2)] public void TorchLayoutHasExactlyOneRealCandidatePerSectorAndPersists(int preset)
        {Prepared(preset);Assert.That(c.LoadCheckpoint(),Is.True);for(int sector=0;sector<4;sector++)Assert.That(Enumerable.Range(0,3).Count(candidate=>candidate==Phase4Ids.AuthenticIndex(c.Data.phase4.torch_layout_seed,sector)),Is.EqualTo(1));Assert.That(c.Data.phase4.torch_layout_seed,Is.EqualTo(preset));}
        [TestCase(0,1,2)][TestCase(0,2,1)][TestCase(1,0,2)][TestCase(1,2,0)][TestCase(2,0,1)][TestCase(2,1,0)]
        public void AllSixSectorOrdersReloadWithProgressThenExactlyOneCoreSeal(int a,int b,int e)
        {
            Prepared();foreach(int sector in new[]{a,b,e}){Local(sector);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.phase4.Completed("P4_BLITZ_"+Phase4Ids.Sectors[sector]),Is.True);Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE4_CP6"));}
            Assert.That(Phase4Ids.HitCount(c.Data),Is.EqualTo(3));Phase4Campaign.Refill(c,"WEST");Assert.That(Phase4Campaign.Pour(c,3,Phase4Ids.AuthenticIndex(0,3),"SOUTH"),Is.True);Assert.That(Phase4Ids.HitCount(c.Data),Is.EqualTo(3));
            c.Data.battery=80;Assert.That(Phase4Campaign.Shot(c,80,65,3,"PENDOPO"),Is.True);Assert.That(c.Data.location_seals.Count,Is.EqualTo(4));Assert.That(Phase4Ids.HitCount(c.Data),Is.EqualTo(4));Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE4_CP7"));
            Assert.That(Phase4Campaign.Shot(c,65,50,3,"PENDOPO"),Is.False);Assert.That(c.Data.location_seals.Count,Is.EqualTo(4));
        }
        [Test] public void SouthCannotStartEarlyAndLocalOrCoreBlitzNeedsExactPrerequisites()
        {Prepared();Phase4Campaign.Refill(c,"WEST");Assert.That(Phase4Campaign.Pour(c,3,0,"SOUTH"),Is.False);Assert.That(Phase4Campaign.Shot(c,37,22,3,"PENDOPO"),Is.False);Assert.That(Phase4Campaign.Shot(c,14,-1,0,"PENDOPO"),Is.False);Assert.That(Phase4Campaign.Shot(c,37,36,0,"PENDOPO"),Is.False);}
        [Test] public void DecoyConsumesOneWaterAndFortySanityThenUnlimitedFreeRefillsWork()
        {Prepared();Phase4Campaign.Refill(c,"WEST");c.Data.sanity=73;int wrong=(Phase4Ids.AuthenticIndex(0,0)+1)%3;Assert.That(Phase4Campaign.Pour(c,0,wrong,"NORTH"),Is.True);Assert.That(c.Data.sanity,Is.EqualTo(33));Assert.That(c.Data.phase4.water_units,Is.EqualTo(2));Assert.That(Phase4Ids.LocalHits(c.Data),Is.Zero);Assert.That(Phase4Campaign.Refill(c,"WEST"),Is.True);Assert.That(c.Data.phase4.water_units,Is.EqualTo(3));}
        [Test] public void PaidMissAndCoreTimeoutDoNotEraseFinishedSectors()
        {Prepared();Local(2);c.Data.battery=30;Assert.That(Phase4Campaign.Shot(c,30,15,-1,"WEST"),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.battery,Is.EqualTo(15));Assert.That(Phase4Ids.LocalHits(c.Data),Is.EqualTo(1));Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE4_CP6"));}
        [Test] public void IncenseConsumesCarriedInstanceAndRecoveryCooldownUsesPersistedGameClock()
        {
            Depart();Assert.That(Phase4Campaign.Incense(c,"ENTRY"),Is.True);Assert.That(c.Data.sanity,Is.EqualTo(100));Assert.That(c.Data.inventory,Does.Not.Contain("P3_KEMENYAN_A"));c.Data.battery=0;
            Assert.That(Phase4Campaign.Recover(c,"CHARGE_ENTRY","ENTRY",true),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(Phase4Campaign.Recover(c,"CHARGE_ENTRY","ENTRY",true),Is.False);c.Data.phase4.elapsed_gameplay_seconds+=20;Assert.That(Phase4Campaign.Recover(c,"CHARGE_ENTRY","ENTRY",true),Is.True);Assert.That(c.Data.battery,Is.EqualTo(50));
        }
        [Test] public void OptionalSecretRequiresKnockEyeAndWheelAndDoesNotGateBoss()
        {
            Prepared();Assert.That(Phase4Campaign.CollectMirror(c,"WEST"),Is.False);foreach(var id in new[]{"P4_SECRET_KNOCK_VERIFIED","P4_SECRET_EYE_VERIFIED","P4_SECRET_WHEEL_ALIGNED"})Phase4Campaign.Receipt(c,id,"WEST");
            Assert.That(Phase4Campaign.CollectMirror(c,"WEST"),Is.True);Assert.That(Phase4Campaign.CollectMirror(c,"WEST"),Is.True);Assert.That(c.Data.secret_items.Count(x=>x==Phase4Ids.Secret),Is.EqualTo(1));
        }
        [TestCase("purchase")][TestCase("water")][TestCase("hit")][TestCase("incense")][TestCase("recovery")][TestCase("secret")]
        public void DiskWriteFaultRollsBackTheWholeResourceTransactionAndPreservesFile(string action)
        {
            Prepared();c.Data.wallet_legitimate=1000000;c.Data.phase4.water_units=3;c.Data.battery=80;
            foreach(var id in new[]{"P4_SECRET_KNOCK_VERIFIED","P4_SECRET_EYE_VERIFIED","P4_SECRET_WHEEL_ALIGNED"})c.Data.phase4.Add(id);
            if(action=="purchase"){c.Data.phase4.completed_event_ids.Remove("P4_DEPART_KOS");c.Data.phase4.completed_event_ids.Remove("P4_RITUAL_TRIGGER");}
            Assert.That(c.SaveCheckpointAtomic(c.Data.checkpoint_id),Is.True);string before=JsonUtility.ToJson(c.Data),disk=File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json"));
            string blocked=Path.Combine(folder,"blocked");File.WriteAllText(blocked,"fault");Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",blocked);LogAssert.Expect(LogType.Error,new Regex("Gagal menyimpan checkpoint secara atomik"));
            bool result=action=="purchase"?Phase4Campaign.Buy(c,"POWERBANK"):action=="water"?Phase4Campaign.Pour(c,0,0,"NORTH"):action=="hit"?Phase4Campaign.Shot(c,80,65,0,"NORTH"):action=="incense"?Phase4Campaign.Incense(c,"NORTH"):action=="secret"?Phase4Campaign.CollectMirror(c,"WEST"):Phase4Campaign.Recover(c,"CHARGE_NORTH","NORTH",true);
            Assert.That(result,Is.False);Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before));Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);Assert.That(File.ReadAllText(Path.Combine(folder,"layar_terakhir_save.json")),Is.EqualTo(disk));
        }
        [TestCase(1)][TestCase(2)][TestCase(3)][TestCase(4)] public void OldSchemasMigrateWithoutErasingPreviousEvidenceOrInventory(int schema)
        {c.Data.schema_version=schema;c.Data.phase1.Add("P1_COMPLETE");c.Data.secret_items.Add(Phase1Ids.Secrets[0]);var d=JsonUtility.ToJson(c.Data);Directory.CreateDirectory(folder);File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"),d);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.schema_version,Is.EqualTo(5));Assert.That(c.Data.battery,Is.EqualTo(37));Assert.That(c.Data.secret_items,Does.Contain(Phase1Ids.Secrets[0]));Assert.That(c.Data.inventory,Does.Contain("P3_KEMENYAN_A"));}
        [Test] public void CP8IsARealBoundaryOnTheSameNightWithoutAPlaceholderSceneLoadOrKosReset()
        {Prepared();Local(0);Local(1);Local(2);Phase4Campaign.Refill(c,"WEST");Phase4Campaign.Pour(c,3,0,"SOUTH");c.Data.battery=37;Phase4Campaign.Shot(c,37,22,3,"PORTAL");Assert.That(Phase4Campaign.Finish(c),Is.False);Phase4Campaign.Receipt(c,"P4_FIFTH_GATE_REVEAL","PORTAL");Assert.That(Phase4Campaign.Finish(c),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.chapter_id,Is.EqualTo(4));Assert.That(c.Data.battery,Is.EqualTo(22));Assert.That(c.Data.phase4.safe_anchor,Is.EqualTo("BOUNDARY"));Assert.That(CampaignRouting.ContinueScene(c.Data),Is.EqualTo(CampaignRouting.VillageScene));}
    }
}
#endif
