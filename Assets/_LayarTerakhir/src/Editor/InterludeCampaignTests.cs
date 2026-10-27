#if UNITY_EDITOR
using System;
using System.IO;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Interlude;
using LayarTerakhir.Phase1;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class InterludeCampaignTests
    {
        private string folder, previous;
        private CampaignState campaign;
        [SetUp]public void Setup()
        {
            previous=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            folder=Path.Combine(Path.GetTempPath(),"LT-IN-"+Guid.NewGuid());Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",folder);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);campaign=new GameObject("Campaign").AddComponent<CampaignState>();
            typeof(CampaignState).GetMethod("Awake",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(campaign,null);campaign.StartNewGame();
        }
        [TearDown]public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",previous);
            if(Directory.Exists(folder))Directory.Delete(folder,true);
        }
        private void AtPrologComplete(float battery=62)
        {
            campaign.Data.chapter_id=0;campaign.Data.checkpoint_id="PROLOG_COMPLETE";
            campaign.Data.battery=battery;
            foreach(var id in new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA","POWERBANK_KECIL_KOS","DOMPET_RAKA","DIARY_RAKA","MATABATIN_APK","PROLOG_COMPLETE"})campaign.AddInventoryItem(id);
            campaign.Data.wallet_bound=25450000;
        }
        private void EnterDay()
        {
            AtPrologComplete();
            Assert.That(InterludeCampaign.EnterInterlude(campaign),Is.True);
            Assert.That(InterludeCampaign.NoteBeat(campaign,InterludeIds.RatnaTalk),Is.True);
            Assert.That(InterludeCampaign.NoteBeat(campaign,InterludeIds.HookRead),Is.True);
        }
        [Test]public void EnterInterludeKeepsChapterZeroAndPreservesCarryOver()
        {
            AtPrologComplete(62);
            Assert.That(InterludeCampaign.EnterInterlude(campaign),Is.True);
            Assert.That(campaign.Data.checkpoint_id,Is.EqualTo(InterludeIds.MorningCheckpoint));
            Assert.That(campaign.Data.chapter_id,Is.Zero);
            Assert.That(campaign.Data.battery,Is.EqualTo(62));
            Assert.That(campaign.Data.wallet_bound,Is.EqualTo(25450000));
            Assert.That(CampaignRouting.ContinueScene(campaign.Data),Is.EqualTo("Prolog_KamarKos"));
            Assert.That(campaign.HasPhaseEntry(),Is.True);
            Assert.That(InterludeIds.IsInterludeData(campaign.Data),Is.True);
        }
        [Test]public void EnterInterludeRejectsWrongCheckpointWithoutMutation()
        {
            campaign.Data.chapter_id=0;campaign.Data.checkpoint_id="PROLOG_KOS_CORRIDOR";
            string before=JsonUtility.ToJson(campaign.Data);
            Assert.That(InterludeCampaign.EnterInterlude(campaign),Is.False);
            Assert.That(JsonUtility.ToJson(campaign.Data),Is.EqualTo(before));
            campaign.Data.checkpoint_id="PHASE1_CP1";campaign.Data.chapter_id=1;
            Assert.That(InterludeCampaign.EnterInterlude(campaign),Is.False);
        }
        [Test]public void BeatsAreIdempotentAndGateRest()
        {
            AtPrologComplete();InterludeCampaign.EnterInterlude(campaign);
            Assert.That(InterludeCampaign.ReadyToRest(campaign.Data),Is.False);
            Assert.That(InterludeCampaign.MissingForRest(campaign.Data),Does.Contain("Bu Haji"));
            InterludeCampaign.NoteBeat(campaign,InterludeIds.RatnaTalk);
            Assert.That(InterludeCampaign.MissingForRest(campaign.Data),Does.Contain("HP"));
            Assert.That(InterludeCampaign.RestUntilNight(campaign),Is.False);
            InterludeCampaign.NoteBeat(campaign,InterludeIds.HookRead);
            Assert.That(InterludeCampaign.ReadyToRest(campaign.Data),Is.True);
            int count=campaign.Data.inventory.Count;
            InterludeCampaign.NoteBeat(campaign,InterludeIds.RatnaTalk);
            Assert.That(campaign.Data.inventory.Count,Is.EqualTo(count));
        }
        [Test]public void RestUntilNightSnapshotsEveningAndRefillsBattery()
        {
            EnterDay();campaign.Data.battery=41;
            Assert.That(InterludeCampaign.RestUntilNight(campaign),Is.True);
            Assert.That(campaign.Data.checkpoint_id,Is.EqualTo(InterludeIds.ReadyCheckpoint));
            Assert.That(campaign.Data.battery,Is.EqualTo(100));
            Assert.That(InterludeCampaign.RestUntilNight(campaign),Is.False);
            Assert.That(campaign.LoadCheckpoint(),Is.True);
            Assert.That(campaign.Data.checkpoint_id,Is.EqualTo(InterludeIds.ReadyCheckpoint));
            Assert.That(InterludeCampaign.ReadyToRest(campaign.Data),Is.True);
        }
        [Test]public void DepartFromReadyEntersPhase1WithFullBattery()
        {
            EnterDay();
            Assert.That(Phase1Director.PrepareNextNight(campaign),Is.False,"Day is not a departure checkpoint");
            InterludeCampaign.RestUntilNight(campaign);
            Assert.That(Phase1Director.PrepareNextNight(campaign),Is.True);
            Assert.That(campaign.Data.chapter_id,Is.EqualTo(1));
            Assert.That(campaign.Data.checkpoint_id,Is.EqualTo("PHASE1_CP1"));
            Assert.That(campaign.Data.battery,Is.EqualTo(100));
            Assert.That(CampaignRouting.ContinueScene(campaign.Data),Is.EqualTo(CampaignRouting.RusunawaScene));
            Assert.That(Phase1Director.PrepareNextNight(campaign),Is.False,"Retry must not re-grant");
        }
        [Test]public void PrologDirectHandoffStillWorksForLegacySaves()
        {
            AtPrologComplete(77);
            Assert.That(Phase1Director.PrepareNextNight(campaign),Is.True);
            Assert.That(campaign.Data.chapter_id,Is.EqualTo(1));
            Assert.That(campaign.Data.battery,Is.EqualTo(100));
        }
        [Test]public void HubChargeIsExplicitOnceAndSurvivesReload()
        {
            EnterDay();campaign.Data.battery=10;
            Assert.That(InterludeCampaign.ChargeHub(campaign),Is.True);
            Assert.That(campaign.Data.battery,Is.EqualTo(100));
            campaign.Data.battery=32;campaign.SaveCheckpointAtomic(campaign.Data.checkpoint_id);
            campaign.LoadCheckpoint();
            Assert.That(InterludeCampaign.ChargeHub(campaign),Is.False);
            Assert.That(campaign.Data.battery,Is.EqualTo(32));
        }
        [Test]public void ReloadDuringInterludeRestoresBeatProgress()
        {
            EnterDay();
            InterludeCampaign.NoteBeat(campaign,InterludeIds.WarungVisit);
            campaign.Data.checkpoint_id=InterludeIds.DayCheckpoint;
            campaign.SaveCheckpointAtomic(InterludeIds.DayCheckpoint);
            Assert.That(campaign.LoadCheckpoint(),Is.True);
            Assert.That(campaign.Data.checkpoint_id,Is.EqualTo(InterludeIds.DayCheckpoint));
            Assert.That(InterludeCampaign.HasBeat(campaign.Data,InterludeIds.RatnaTalk),Is.True);
            Assert.That(InterludeCampaign.HasBeat(campaign.Data,InterludeIds.WarungVisit),Is.True);
            Assert.That(InterludeCampaign.ReadyToRest(campaign.Data),Is.True);
        }
        [Test]public void CorridorSceneKeepsEveryObjectTheDaytimeDirectorNeeds()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LayarTerakhir.Editor.KosCorridorBuilder.ScenePath);
            var corridor=UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Prolog.KosCorridorDirector>();
            Assert.That(corridor,Is.Not.Null);
            Assert.That(corridor.player,Is.Not.Null);
            Assert.That(corridor.spawnLanding,Is.Not.Null);
            Assert.That(corridor.spawnGround,Is.Not.Null);
            Assert.That(corridor.motor,Is.Not.Null);
            Assert.That(corridor.gateHinge,Is.Not.Null);
            Assert.That(corridor.buildingRoot,Is.Not.Null);
            Assert.That(corridor.entranceDoor,Is.Not.Null);
            var vendor=GameObject.Find("BuSriShopkeeper");
            Assert.That(vendor,Is.Not.Null,"Daytime NPCs are runtime clones of the warung vendor");
            bool headFound=false;
            foreach(var t in vendor.GetComponentsInChildren<Transform>(true))if(t.name=="Head")headFound=true;
            Assert.That(headFound,Is.True);
            var motion=vendor.GetComponent<Animation>();
            Assert.That(motion && motion.GetClip("Idle"),Is.Not.Null,"Cloned NPCs idle on the vendor Idle clip");
            Assert.That(UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Prolog.MelatiShop>(),Is.Not.Null);
            Assert.That(UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Prolog.MelatiNeighborhood>(),Is.Not.Null);
        }
        [Test]public void KosRoomSceneKeepsEveryObjectTheMorningHubNeeds()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LayarTerakhir.Editor.PrologKosSceneBuilder.ScenePath);
            var prolog=UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Prolog.KamarKosPrologDirector>();
            Assert.That(prolog,Is.Not.Null);
            Assert.That(prolog.pcMonitorItem,Is.Not.Null);
            Assert.That(prolog.kosDoorItem,Is.Not.Null);
            Assert.That(GameObject.Find("Golden_Workstation"),Is.Not.Null);
            Assert.That(GameObject.Find("vintage_day_bed"),Is.Not.Null);
            Assert.That(GameObject.Find("PhoneCharger") ?? GameObject.Find("OldWallSocket"),Is.Not.Null);
        }
        [Test]public void ExteriorDressingBuildsCompleteStreetComposition()
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LayarTerakhir.Editor.KosCorridorBuilder.ScenePath);
            var corridor=UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Prolog.KosCorridorDirector>();
            Assert.That(corridor,Is.Not.Null);
            LayarTerakhir.Interlude.InterludeExterior.DressStreet(corridor);
            try
            {
                Assert.That(GameObject.Find("InterludeGapuraBeam"),Is.Not.Null,"gapura arch anchors the lane entrance");
                Assert.That(GameObject.Find("InterludeSpandukUtama"),Is.Not.Null,"RT spanduk spans the road");
                Assert.That(GameObject.Find("InterludeWartegSign"),Is.Not.Null,"warteg sign present");
                Assert.That(GameObject.Find("InterludeWartegCaseBase"),Is.Not.Null,"warteg etalase present");
                int carts=0,parked=0,signs=0,labels=0;
                foreach(var t in UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None))
                {
                    if(t.name=="CartSign")carts++;
                    if(t.name.StartsWith("InterludeParkedMotor"))parked++;
                    if(t.name.StartsWith("InterludeSign")&&!t.name.EndsWith("Text"))signs++;
                    var label=t.GetComponent<TextMesh>();
                    if(label&&t.name.StartsWith("Interlude")){labels++;Assert.That(label.font,Is.Not.Null,"label resolves scene font: "+t.name);Assert.That(label.text.Length,Is.GreaterThan(0));}
                }
                Assert.That(carts,Is.EqualTo(2),"two PKL carts with signs");
                Assert.That(parked,Is.EqualTo(3),"parked motorbike row");
                Assert.That(signs,Is.GreaterThanOrEqualTo(4),"shop boards on facades");
                Assert.That(labels,Is.GreaterThanOrEqualTo(8),"all exterior text resolves a font");
                var gapura=GameObject.Find("InterludeGapuraPillar");
                Assert.That(gapura.GetComponent<Collider>(),Is.Not.Null,"landmark blocks walking");
            }
            finally
            {
                var residue = new System.Collections.Generic.List<GameObject>();
                foreach (var t in UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
                {
                    if (t == null) continue;
                    string n;
                    try { n = t.name; } catch { continue; }
                    if (!n.StartsWith("Interlude")) continue;
                    var p = t.parent;
                    bool parentKept = false;
                    try { parentKept = p != null && p.name.StartsWith("Interlude"); } catch { }
                    if (!parentKept) residue.Add(t.gameObject);
                }
                foreach (var go in residue) UnityEngine.Object.DestroyImmediate(go);
            }
            Assert.That(GameObject.Find("InterludeGapuraBeam"),Is.Null,"dressing leaves no residue in the authored scene");
        }
    }
}
#endif
