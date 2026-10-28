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
        [Test]public void AuthoredStreetCompositionIsComplete()
        {
            // Zone A/B/C are authored into the scene file by
            // MelatiInterludeAlleyBuilder; runtime only adds NPCs/traffic.
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LayarTerakhir.Editor.KosCorridorBuilder.ScenePath);
            var corridor = UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Prolog.KosCorridorDirector>();
            Assert.That(corridor, Is.Not.Null);
            // Transform.Find: GameObject.Find skips the inactive group.
            var kampung = GameObject.Find("KampungMelati");
            var alleyGroup = kampung ? kampung.transform.Find("InterludeAlley") : null;
            Assert.That(alleyGroup, Is.Not.Null, "daytime dressing must live under one group");
            Assert.That(alleyGroup.gameObject.activeSelf, Is.False, "group stays OFF in the saved scene so the night prolog route is untouched");
            int alley = 0, warteg = 0, carts = 0, parked = 0, wires = 0, labels = 0;
            foreach (var t in UnityEngine.Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (t == null) continue;
                string n; try { n = t.name; } catch { continue; }
                if (n.StartsWith("AlleyHouse")) alley++;
                if (n == "WartegBanner") warteg++;
                if (n == "GerobakSign") carts++;
                if (n.StartsWith("StreetParkedMotor") || n == "StreetParkedSedan" || n == "StreetAngkot") parked++;
                if (n == "InterludeWire") wires++;
                var label = t.GetComponent<TextMesh>();
                if (label && (n.StartsWith("Alley") || n.StartsWith("Warteg") || n.StartsWith("Gerobak") || n.StartsWith("Street")))
                { labels++; Assert.That(label.font, Is.Not.Null, "authored label resolves a font: " + n); }
            }
            Assert.That(alley, Is.GreaterThanOrEqualTo(8), "Zone A gang has authored facades on both sides");
            Assert.That(warteg, Is.EqualTo(1), "Zone C warteg banner facade present");
            Assert.That(carts, Is.EqualTo(2), "two PKL carts with painted signs");
            Assert.That(parked, Is.GreaterThanOrEqualTo(6), "parked motors, sedan and angkot on the street");
            Assert.That(wires, Is.GreaterThanOrEqualTo(8), "catenary wiring grounds the street");
            Assert.That(labels, Is.GreaterThanOrEqualTo(6), "authored signage resolves fonts");
        }
        [Test]public void KosToStreetRouteStaysWalkable()
        {
            // No-trap regression: sample the interlude walking route and fail
            // if any environmental collider swallows a standing capsule.
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene(LayarTerakhir.Editor.KosCorridorBuilder.ScenePath);
            var kampung = GameObject.Find("KampungMelati");
            var alleyGroup = kampung ? kampung.transform.Find("InterludeAlley") : null;
            Assert.That(alleyGroup, Is.Not.Null, "daytime group must exist before testing the daytime route");
            alleyGroup.gameObject.SetActive(true);
            Vector3[] route =
            {
                // Gate, south around the new north facades, east along the
                // kos street into the authored alley mouth, then to the road.
                new Vector3(-16.5f, 1f, -6.5f), new Vector3(-17.5f, 1f, -9f), new Vector3(-16f, 1f, -12f),
                new Vector3(-11f, 1f, -11.75f),
                new Vector3(-7f, 1f, -11.75f), new Vector3(-4f, 1f, -11.75f), new Vector3(-1f, 1f, -13f),
                new Vector3(0f, 1f, -17f), new Vector3(2f, 1f, -21f), new Vector3(5.4f, 1f, -24f),
                new Vector3(4.6f, 1f, -28f), new Vector3(0f, 1f, -33f)
            };
            Physics.SyncTransforms();
            foreach (var p in route)
            {
                var hits = Physics.OverlapCapsule(p + Vector3.up * .4f, p + Vector3.up * 1.4f, .3f);
                Assert.That(hits.Length, Is.EqualTo(0), "route blocked at " + p);
            }
        }
    }
}
#endif
