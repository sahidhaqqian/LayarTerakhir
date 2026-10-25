#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase5;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.TestTools;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase5CampaignTests
    {
        private CampaignState c;
        private string folder, previous;
        private string Slot => Path.Combine(folder, "layar_terakhir_save.json");
        [SetUp] public void Setup()
        {
            previous = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            folder = Path.Combine(Path.GetTempPath(), "P5-" + Guid.NewGuid()); Directory.CreateDirectory(folder);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", folder);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            c = new GameObject("Campaign").AddComponent<CampaignState>(); typeof(CampaignState).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(c, null);
            File.Copy("Audit/phase4-native-final-save.json", Slot);
            Assert.That(c.LoadCheckpoint(), Is.True, "Real Phase4 native-route boundary save must migrate");
        }
        [TearDown] public void Cleanup()
        { EditorSceneManager.NewScene(NewSceneSetup.EmptyScene); Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", previous); if (Directory.Exists(folder)) Directory.Delete(folder, true); }
        private void Enter() { Assert.That(Phase5Campaign.Enter(c), Is.True); }
        private void Stage()
        {
            Enter(); Assert.That(Phase5Campaign.EnterSectors(c), Is.True); Assert.That(Phase5Campaign.ClearSector(c, 1), Is.True);
            Assert.That(Phase5Campaign.EnsureVessel(c), Is.True); c.Data.phase4.water_units = 1; Assert.That(Phase5Campaign.Pour(c), Is.True);
            Assert.That(Phase5Campaign.ClearSector(c, 2), Is.True); Assert.That(Phase5Campaign.ClearSector(c, 3), Is.True);
        }
        private void Arena()
        {
            Stage(); c.Data.battery = 100;
            Assert.That(Phase5Campaign.Evidence(c, Phase5Ids.Server, true), Is.True);
            Phase5Campaign.Receipt(c, "P5_CONTRACT_THERMO_OK"); Phase5Campaign.Receipt(c, "P5_CONTRACT_UV_OK");
            Assert.That(Phase5Campaign.Evidence(c, Phase5Ids.Contract), Is.True); Assert.That(Phase5Campaign.Evidence(c, Phase5Ids.Crowd), Is.True);
            Assert.That(Phase5Campaign.ConfirmJournal(c), Is.True); Assert.That(Phase5Campaign.BeginArena(c), Is.True);
        }
        private void Core(int n) { float before = c.Data.battery; c.Data.battery -= 15; Assert.That(Phase5Campaign.RecordShot(c, before, before - 15, n), Is.True); }
        private void Final()
        { Arena(); Core(1); Core(2); for (int i = 1; i <= 3; i++) Assert.That(Phase5Campaign.CutCable(c, i), Is.True); Core(3); Assert.That(Phase5Campaign.EnterFinal(c), Is.True); }
        private void AllArchive()
        { foreach (string id in Phase1Ids.Secrets) if (!c.Data.secret_items.Contains(id)) c.Data.secret_items.Add(id); }
        private void FailWrite(Action action)
        {
            Directory.CreateDirectory(Slot + ".tmp");
            LogAssert.Expect(LogType.Error, new Regex("Gagal menyimpan checkpoint secara atomik")); action();
            Directory.Delete(Slot + ".tmp");
        }
        [Test] public void ActualP4BoundaryMigratesAndHandoffPreservesAllGlobalCarryOver()
        {
            string inventory = string.Join("|", c.Data.inventory), archive = string.Join("|", c.Data.secret_items), seals = string.Join("|", c.Data.location_seals);
            float battery = c.Data.battery, sanity = c.Data.sanity; long wallet = c.Data.wallet_legitimate, bound = c.Data.wallet_bound, spent = c.Data.spent;
            Assert.That(c.Data.schema_version, Is.EqualTo(6)); Assert.That(Phase5Ids.Eligible(c.Data), Is.True); Enter();
            Assert.That(c.Data.battery, Is.EqualTo(battery)); Assert.That(c.Data.sanity, Is.EqualTo(sanity)); Assert.That(c.Data.wallet_legitimate, Is.EqualTo(wallet));
            Assert.That(c.Data.wallet_bound, Is.EqualTo(bound)); Assert.That(c.Data.spent, Is.EqualTo(spent)); Assert.That(string.Join("|", c.Data.inventory), Is.EqualTo(inventory));
            Assert.That(string.Join("|", c.Data.secret_items), Is.EqualTo(archive)); Assert.That(string.Join("|", c.Data.location_seals), Is.EqualTo(seals));
            Assert.That(c.Data.checkpoint_id, Is.EqualTo("PHASE5_CP1")); Assert.That(CampaignRouting.ContinueScene(c.Data), Is.EqualTo(CampaignRouting.MarketScene));
        }
        [TestCase(0)][TestCase(1)][TestCase(2)][TestCase(3)] public void MissingAnyOfTheFourSealsDeniesHandoff(int n)
        { c.Data.location_seals.Remove(Phase5Ids.Seals[n]); Assert.That(Phase5Campaign.Enter(c), Is.False); Assert.That(c.Data.chapter_id, Is.EqualTo(4)); }
        [Test] public void P4CompletionAndCP8AreBothRequired()
        { c.Data.phase4.completed_event_ids.Remove("P4_COMPLETE"); Assert.That(Phase5Campaign.Enter(c), Is.False); c.Data.phase4.Add("P4_COMPLETE"); c.Data.checkpoint_id = "PHASE4_CP7"; Assert.That(Phase5Campaign.Enter(c), Is.False); }
        [Test] public void FailedHandoffLeavesMemoryAndDiskAtP4Boundary()
        { string before = JsonUtility.ToJson(c.Data), disk = File.ReadAllText(Slot); FailWrite(() => Assert.That(Phase5Campaign.Enter(c), Is.False)); Assert.That(JsonUtility.ToJson(c.Data), Is.EqualTo(before)); Assert.That(File.ReadAllText(Slot), Is.EqualTo(disk)); }
        [Test] public void OpeningDonationNeverBecomesSpendableAndCannotRepeatAfterReload()
        { Enter(); long bound = c.Data.wallet_bound, wallet = c.Data.wallet_legitimate; Assert.That(Phase5Campaign.StartLive(c), Is.True); Assert.That(c.LoadCheckpoint(), Is.True); Assert.That(Phase5Campaign.StartLive(c), Is.False); Assert.That(c.Data.wallet_bound, Is.EqualTo(bound + 66666666)); Assert.That(c.Data.wallet_legitimate, Is.EqualTo(wallet)); }
        [TestCase("POWERBANK",750000,3)][TestCase("FLARE",600000,4)][TestCase("KEMENYAN",700000,4)][TestCase("CHARGE",100000,0)]
        public void ShopIsAtomicUniqueAndNeverDebitsBoundFunds(string pack, long cost, int count)
        { Enter(); c.Data.wallet_legitimate = cost; long bound = c.Data.wallet_bound; int before = c.Data.inventory.Count; Assert.That(Phase5Campaign.Buy(c, pack), Is.True); Assert.That(Phase5Campaign.Buy(c, pack), Is.False); Assert.That(c.LoadCheckpoint(), Is.True); Assert.That(c.Data.wallet_legitimate, Is.Zero); Assert.That(c.Data.spent, Is.GreaterThanOrEqualTo(cost)); Assert.That(c.Data.wallet_bound, Is.EqualTo(bound)); Assert.That(c.Data.inventory.Count, Is.EqualTo(before + count)); }
        [Test] public void ZeroWalletDoesNotGateSectorsEvidenceBossOrFreeRecovery()
        { c.Data.wallet_legitimate = 0; c.Data.battery = c.Data.sanity = 0; c.Data.inventory.RemoveAll(id => Phase5Ids.IsFlare(id) || Phase5Ids.IsIncense(id) || id.Contains("POWERBANK")); StageAfterZeroFixture(); }
        private void StageAfterZeroFixture()
        {
            Enter(); Assert.That(Phase5Campaign.Buy(c,"POWERBANK"),Is.False);
            Assert.That(Phase5Campaign.Recover(c,"DYNAMO_GATE","GATE",true,20),Is.True); Assert.That(c.Data.battery,Is.EqualTo(25));
            Assert.That(Phase5Campaign.Recover(c,"PRAYER_GATE","GATE",false,30),Is.True); Assert.That(c.Data.sanity,Is.EqualTo(15));
            Assert.That(Phase5Campaign.EnterSectors(c),Is.True); Assert.That(Phase5Campaign.ClearSector(c,1),Is.True);
            Assert.That(Phase5Campaign.EnsureVessel(c),Is.True); c.Data.phase4.water_units=0; Assert.That(Phase5Campaign.Pour(c),Is.False); Assert.That(Phase5Campaign.Refill(c),Is.True); Assert.That(Phase5Campaign.Pour(c),Is.True);
            Assert.That(Phase5Campaign.ClearSector(c,2),Is.True); Assert.That(Phase5Campaign.ClearSector(c,3),Is.True); Assert.That(c.Data.wallet_legitimate,Is.Zero);
        }
        [Test] public void FailedPurchaseRollsBackLedgerMoneyAndAllInstances()
        { Enter(); c.Data.wallet_legitimate=1000000; string before=JsonUtility.ToJson(c.Data); FailWrite(()=>Assert.That(Phase5Campaign.Buy(c,"FLARE"),Is.False)); Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before)); }
        [Test] public void SectorOrderingAndCheckpointsNeverRegressOnBacktrackReceipts()
        { Enter(); Assert.That(Phase5Campaign.ClearSector(c,3),Is.False); Phase5Campaign.EnterSectors(c); Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP2")); Phase5Campaign.ClearSector(c,1); Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP3")); Phase5Campaign.Receipt(c,"P5_SECRET_CHAIR_SILENCE","GATE"); Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP3")); Assert.That(c.LoadCheckpoint(),Is.True); Assert.That(c.Data.phase5.Completed("P5_S1_CLEAR"),Is.True); }
        [Test] public void KendiAndVolumeHaveOneOwnerWithNoFallbackDuplicate()
        { Enter(); Phase5Campaign.EnterSectors(c); Phase5Campaign.ClearSector(c,1); c.Data.phase4.water_units=2; Assert.That(Phase5Campaign.EnsureVessel(c),Is.True); Assert.That(c.Data.inventory.Count(x=>x==Phase4Ids.Kendi),Is.EqualTo(1)); Assert.That(c.Data.inventory,Does.Not.Contain(Phase5Ids.Cup)); Assert.That(Phase5Campaign.Pour(c),Is.True); Assert.That(c.Data.phase4.water_units,Is.EqualTo(1)); Assert.That(Phase5Campaign.Pour(c),Is.False); Assert.That(c.Data.phase4.water_units,Is.EqualTo(1)); }
        [Test] public void MissingKendiGetsOneFreeCupAndSameWaterCounter()
        { Enter(); Phase5Campaign.EnterSectors(c); Phase5Campaign.ClearSector(c,1); c.Data.inventory.Remove(Phase4Ids.Kendi); Assert.That(Phase5Campaign.EnsureVessel(c),Is.True); Assert.That(Phase5Campaign.EnsureVessel(c),Is.True); Assert.That(c.Data.inventory.Count(x=>x==Phase5Ids.Cup),Is.EqualTo(1)); Assert.That(Phase5Campaign.Refill(c),Is.True); Assert.That(Phase5Campaign.Pour(c),Is.True); Assert.That(c.Data.phase4.water_units,Is.EqualTo(2)); }
        [Test] public void FailedPourDoesNotSpendWaterOrOpenGate()
        { Stage(); c.Data.phase5.completed_event_ids.Remove("P5_BRIDGE_TORCH_OFF"); c.Data.phase5.completed_event_ids.Remove("P5_S2_CLEAR"); c.Data.phase5.completed_event_ids.Remove("P5_S3_CLEAR"); c.Data.phase4.water_units=2; string before=JsonUtility.ToJson(c.Data); FailWrite(()=>Assert.That(Phase5Campaign.Pour(c),Is.False)); Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before)); }
        [Test] public void ContractNeedsBothSubobservationsAndEvidenceCannotDuplicate()
        { Stage(); Assert.That(Phase5Campaign.Evidence(c,Phase5Ids.Contract),Is.False); Phase5Campaign.Receipt(c,"P5_CONTRACT_UV_OK"); Assert.That(Phase5Campaign.Evidence(c,Phase5Ids.Contract),Is.False); Phase5Campaign.Receipt(c,"P5_CONTRACT_THERMO_OK"); Assert.That(Phase5Campaign.Evidence(c,Phase5Ids.Contract),Is.True); Assert.That(Phase5Campaign.Evidence(c,Phase5Ids.Contract),Is.True); Assert.That(c.Data.evidence_ids.Count(x=>x==Phase5Ids.Contract),Is.EqualTo(1)); }
        [Test] public void ArenaCannotStartFromInspectingJournalOrIncompleteEvidence()
        { Stage(); Assert.That(Phase5Campaign.BeginArena(c),Is.False); Assert.That(Phase5Campaign.ConfirmJournal(c),Is.False); Assert.That(c.Data.phase5.Completed("P5_ARENA_LOCK"),Is.False); }
        [Test] public void FailedArenaCommitDoesNotActivateBossOrCloseBacktrack()
        { Arena(); c.Data.phase5.completed_event_ids.Remove("P5_ARENA_LOCK"); string before=JsonUtility.ToJson(c.Data); FailWrite(()=>Assert.That(Phase5Campaign.BeginArena(c),Is.False)); Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before)); }
        [TestCase(1)][TestCase(2)] public void ReloadAfterEachCoreKeepsCP7AndPreviousPhases(int count)
        { Arena(); for(int i=1;i<=count;i++)Core(i); Assert.That(c.LoadCheckpoint(),Is.True); Assert.That(Phase5Ids.CoreCount(c.Data),Is.EqualTo(count)); Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP7")); Assert.That(c.Data.battery,Is.EqualTo(99-15*count)); }
        [Test] public void ThirdPhaseDeathRetainsTwoCoresAndEveryCommittedCable()
        { Arena(); Core(1); Core(2); Phase5Campaign.CutCable(c,1); Phase5Campaign.CutCable(c,3); Assert.That(c.LoadCheckpoint(),Is.True); Assert.That(Phase5Ids.CoreCount(c.Data),Is.EqualTo(2)); Assert.That(Phase5Ids.CableCount(c.Data),Is.EqualTo(2)); Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP7")); Assert.That(Phase5Campaign.BeginArena(c),Is.False); }
        [Test] public void CoreOrderAndCablePrerequisitesCannotBeSkipped()
        { Arena(); Assert.That(Phase5Campaign.RecordShot(c,99,84,2),Is.False); Assert.That(Phase5Campaign.CutCable(c,1),Is.False); Core(1); Core(2); Assert.That(Phase5Campaign.RecordShot(c,69,54,3),Is.False); }
        [Test] public void ExactlyThreeSuccessfulCoreBlitzCostsOnceAndNeverAddsFifthLocationSeal()
        { Final(); Assert.That(Phase5Ids.CoreCount(c.Data),Is.EqualTo(3)); Assert.That(c.Data.battery,Is.EqualTo(54)); Assert.That(c.Data.location_seals.Count,Is.EqualTo(4)); Assert.That(Phase5Campaign.RecordShot(c,54,39,3),Is.False); Assert.That(c.Data.battery,Is.EqualTo(54)); }
        [Test] public void FailedCoreCommitRestoresBatteryAndEntireProgressSnapshot()
        { Arena(); string before=JsonUtility.ToJson(c.Data),disk=File.ReadAllText(Slot); c.Data.battery=84; FailWrite(()=>Assert.That(Phase5Campaign.RecordShot(c,99,84,1),Is.False)); Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before)); Assert.That(File.ReadAllText(Slot),Is.EqualTo(disk)); }
        [Test] public void MissCanRetryWithoutLosingEarlierCoreOrDoubleSpending()
        { Arena(); Core(1); c.Data.battery=69; Assert.That(Phase5Campaign.RecordShot(c,84,69,0),Is.True); Assert.That(c.LoadCheckpoint(),Is.True); Assert.That(c.Data.battery,Is.EqualTo(69)); Assert.That(Phase5Ids.CoreCount(c.Data),Is.EqualTo(1)); Core(2); Assert.That(c.Data.battery,Is.EqualTo(54)); }
        [Test] public void FailedCableCommitDoesNotCreateReceipt()
        { Arena(); Core(1); Core(2); string before=JsonUtility.ToJson(c.Data); FailWrite(()=>Assert.That(Phase5Campaign.CutCable(c,1),Is.False)); Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before)); }
        [Test] public void StaleTemporaryFileFromCrashCannotCreateUncommittedCore()
        { Arena(); string old=File.ReadAllText(Slot); Core(1); string candidate=File.ReadAllText(Slot); File.WriteAllText(Slot,old); File.WriteAllText(Slot+".tmp",candidate); Assert.That(c.LoadCheckpoint(),Is.True); Assert.That(Phase5Ids.CoreCount(c.Data),Is.Zero); Assert.That(c.Data.battery,Is.EqualTo(99)); }
        [Test] public void SecretTapeRequiresPuzzleAndCommitsBeforeSuccessfulPickup()
        { Enter(); Assert.That(Phase5Campaign.CollectTape(c),Is.False); foreach(string id in new[]{"P5_SECRET_CHAIR_SILENCE","P5_SECRET_AR_DRAWER","P5_SECRET_EVP_TRUE"})Phase5Campaign.Receipt(c,id); Assert.That(Phase5Campaign.CollectTape(c),Is.True); Assert.That(c.LoadCheckpoint(),Is.True); Assert.That(c.Data.secret_items,Does.Contain(Phase5Ids.Secret)); int n=c.Data.secret_items.Count; Assert.That(Phase5Campaign.CollectTape(c),Is.True); Assert.That(c.Data.secret_items.Count,Is.EqualTo(n)); }
        [Test] public void FailedTapePickupDoesNotAddSecretId()
        { Enter(); foreach(string id in new[]{"P5_SECRET_CHAIR_SILENCE","P5_SECRET_AR_DRAWER","P5_SECRET_EVP_TRUE"})Phase5Campaign.Receipt(c,id); string before=JsonUtility.ToJson(c.Data); FailWrite(()=>Assert.That(Phase5Campaign.CollectTape(c),Is.False)); Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before)); }
        [Test] public void FiveUnknownIdsDoNotUnlockSecretOrValidation()
        { Final(); c.Data.secret_items.Clear(); c.Data.secret_items.AddRange(new[]{"a","b","c","d","e"}); Assert.That(Phase5Ids.ArchiveCount(c.Data),Is.Zero); Assert.That(Phase5Campaign.ValidateArchive(c),Is.False); Assert.That(Phase5Campaign.LockEnding(c,EndingId.SECRET),Is.False); }
        [Test] public void ValidationAtFinalChoiceDoesNotChooseSecretAndBasicActionsRemain()
        { Final(); AllArchive(); Assert.That(Phase5Campaign.ValidateArchive(c),Is.True); Assert.That(c.Data.ending,Is.EqualTo(EndingId.NONE)); Assert.That(Phase5Campaign.LockEnding(c,EndingId.NORMAL),Is.True); }
        [TestCase(EndingId.BAD)][TestCase(EndingId.NORMAL)][TestCase(EndingId.TRUE)][TestCase(EndingId.SECRET)]
        public void EndingIsSavedBeforeCinematicAndCrashRestoresSameLockedEnding(EndingId ending)
        { Final(); if(ending==EndingId.TRUE||ending==EndingId.SECRET)Assert.That(Phase5Campaign.HelpDamar(c),Is.True); if(ending==EndingId.SECRET){AllArchive();Assert.That(Phase5Campaign.ValidateArchive(c),Is.True);} float battery=c.Data.battery; Assert.That(Phase5Campaign.LockEnding(c,ending),Is.True); Assert.That(c.LoadCheckpoint(),Is.True); Assert.That(c.Data.ending,Is.EqualTo(ending)); Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP8")); Assert.That(c.Data.phase5.Completed("P5_COMPLETE"),Is.False); Assert.That(c.Data.battery,Is.EqualTo(battery)); Assert.That(Phase5Campaign.LockEnding(c,ending==EndingId.BAD?EndingId.TRUE:EndingId.BAD),Is.False); Assert.That(Phase5Campaign.Complete(c),Is.True); Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP9")); }
        [TestCase(EndingId.BAD)][TestCase(EndingId.NORMAL)][TestCase(EndingId.TRUE)][TestCase(EndingId.SECRET)]
        public void EveryEndingIsRejectedBeforeFinalChoice(EndingId ending)
        { Arena(); AllArchive(); Assert.That(Phase5Campaign.LockEnding(c,ending),Is.False); Assert.That(c.Data.ending,Is.EqualTo(EndingId.NONE)); }
        [Test] public void FailedEndingLockPreservesChoiceCapacitorWalletAndBattery()
        { Final(); string before=JsonUtility.ToJson(c.Data),disk=File.ReadAllText(Slot); FailWrite(()=>Assert.That(Phase5Campaign.LockEnding(c,EndingId.BAD),Is.False)); Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before)); Assert.That(File.ReadAllText(Slot),Is.EqualTo(disk)); Assert.That(Phase5Ids.CapacitorAvailable(c.Data),Is.True); Assert.That(Phase5Campaign.LockEnding(c,EndingId.NORMAL),Is.True); }
        [Test] public void TwoEndingInputsAndDoubleClickOnlyCommitOneEnumAndNoExtraCost()
        { Final(); float battery=c.Data.battery; Assert.That(Phase5Campaign.LockEnding(c,EndingId.BAD),Is.True); Assert.That(Phase5Campaign.LockEnding(c,EndingId.NORMAL),Is.False); Assert.That(Phase5Campaign.LockEnding(c,EndingId.BAD),Is.False); Assert.That(c.Data.battery,Is.EqualTo(battery)); Assert.That(c.Data.ending,Is.EqualTo(EndingId.BAD)); }
        [Test] public void CrashBeforeEndingReplacementDoesNotLockTemporaryCandidate()
        { Final(); string old=File.ReadAllText(Slot); Phase5Campaign.LockEnding(c,EndingId.BAD); string candidate=File.ReadAllText(Slot); File.WriteAllText(Slot,old);File.WriteAllText(Slot+".tmp",candidate);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.ending,Is.EqualTo(EndingId.NONE));Assert.That(Phase5Ids.FinalChoice(c.Data),Is.True); }
        [Test] public void FailedCP8EntryKeepsThirdCoreAtRollingCP7()
        { Arena(); Core(1);Core(2);for(int i=1;i<=3;i++)Phase5Campaign.CutCable(c,i);Core(3);string before=JsonUtility.ToJson(c.Data);FailWrite(()=>Assert.That(Phase5Campaign.EnterFinal(c),Is.False));Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before));Assert.That(c.Data.checkpoint_id,Is.EqualTo("PHASE5_CP7"));Assert.That(Phase5Ids.FinalChoice(c.Data),Is.False); }
        [Test] public void FailedCP9LeavesLockedEndingForCinematicReplayNotFalseCompletion()
        { Final();Phase5Campaign.LockEnding(c,EndingId.NORMAL);string before=JsonUtility.ToJson(c.Data);FailWrite(()=>Assert.That(Phase5Campaign.Complete(c),Is.False));Assert.That(JsonUtility.ToJson(c.Data),Is.EqualTo(before));Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.ending,Is.EqualTo(EndingId.NORMAL));Assert.That(c.Data.phase5.Completed("P5_COMPLETE"),Is.False); }
        [TestCase(0)][TestCase(1)][TestCase(14)][TestCase(15)][TestCase(29)][TestCase(44)][TestCase(60)][TestCase(100)]
        public void ArenaSnapshotsRetainExactBatteryAndZeroSanityWithFreeDynamo(float battery)
        { Arena();c.Data.battery=battery;c.Data.sanity=0;c.Data.wallet_legitimate=0;c.Data.inventory.RemoveAll(id=>Phase5Ids.IsFlare(id)||Phase5Ids.IsIncense(id)||id.Contains("POWERBANK"));Assert.That(c.SaveCheckpointAtomic("PHASE5_CP6"),Is.True);Assert.That(c.LoadCheckpoint(),Is.True);Assert.That(c.Data.battery,Is.EqualTo(battery));Assert.That(c.Data.sanity,Is.Zero);if(battery<100)Assert.That(Phase5Campaign.Recover(c,"DYNAMO_ARENA_W","ARENA_WEST",true,20),Is.True); }
        [Test] public void ZeroBatteryFinalChoiceUsesRitualPowerWithoutRewritingPersistentBattery()
        { Final();c.Data.battery=c.Data.sanity=0;var phone=new GameObject("Phone").AddComponent<SmartphoneController>();typeof(SmartphoneController).GetMethod("Awake",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(phone,null);phone.ChapterContent=new RitualProvider(c);phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.RestoreBattery(0);Assert.That(phone.HasCameraPower,Is.True);Assert.That(phone.DisplayBatteryPercent,Is.EqualTo(5));Assert.That(phone.batteryPercent,Is.Zero);Assert.That(c.Data.battery,Is.Zero);typeof(SmartphoneController).GetMethod("UpdateBatteryDrain",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(phone,null);Assert.That(c.Data.battery,Is.Zero);Assert.That(Phase5Campaign.LockEnding(c,EndingId.BAD),Is.True);Assert.That(c.Data.battery,Is.Zero);Assert.That(phone.RitualCameraPower,Is.False); }
        private sealed class RitualProvider:IPhoneChapterContent,IPhoneRitualPower
        { private readonly CampaignState c;public RitualProvider(CampaignState c){this.c=c;}public bool RitualCameraPower=>Phase5Ids.FinalChoice(c.Data);public int Viewers=>1000000;public bool CanBeginLive=>false;public bool CameraLocked=>false;public string CameraStatus=>"RITUAL";public void StartLive(){}public void Capture(){}public bool PopulateApp(PhoneApp app,Transform page)=>false; }
    }
}
#endif
