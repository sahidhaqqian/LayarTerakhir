#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase1CampaignTests
    {
        [Serializable] private sealed class AdditionalPropSource { public string id; }
        [Serializable] private sealed class AdditionalPropManifest { public AdditionalPropSource[] records; }
        private string folder, previous;
        private CampaignState campaign;
        [SetUp] public void Setup()
        {
            previous = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            folder = Path.Combine(Path.GetTempPath(), "LT-Phase1-" + Guid.NewGuid());
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", folder);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            campaign = new GameObject("Campaign").AddComponent<CampaignState>();
            typeof(CampaignState).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(campaign, null);
            campaign.StartNewGame();
        }
        [TearDown] public void Cleanup()
        {
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", previous);
            if (Directory.Exists(folder)) Directory.Delete(folder, true);
        }
        [Test] public void SchemaOneMigrationPreservesPrologAndGlobalCollectibles()
        {
            Directory.CreateDirectory(folder);
            File.WriteAllText(Path.Combine(folder,"layar_terakhir_save.json"), "{\"schema_version\":1,\"checkpoint_id\":\"PROLOG_COMPLETE\",\"battery\":31,\"sanity\":12,\"wallet_legitimate\":7000,\"spent\":5000000,\"inventory\":[\"SMARTPHONE_RAKA\",\"EVENT_P0_CHAIN3_BROKEN\"],\"secret_items\":[\"SECRET_P2_LABEL_PASIEN\"]}");
            Assert.That(campaign.LoadCheckpoint(), Is.True);
            Assert.That(campaign.Data.schema_version, Is.EqualTo(4));
            Assert.That(campaign.Data.inventory, Does.Contain("EVENT_P0_CHAIN3_BROKEN"));
            Assert.That(campaign.Data.secret_items, Does.Contain("SECRET_P2_LABEL_PASIEN"));
            Assert.That(campaign.Data.battery, Is.EqualTo(31)); Assert.That(campaign.Data.sanity, Is.EqualTo(12));
            Assert.That(campaign.Data.wallet_legitimate, Is.EqualTo(7000)); Assert.That(campaign.Data.spent, Is.EqualTo(5000000));
        }
        [TestCase(0f)][TestCase(1f)][TestCase(19f)][TestCase(85f)] public void NextNightStartsWithFullBatteryAndPreservesOtherCarryOver(float battery)
        {
            campaign.Data.battery=battery; campaign.Data.sanity=0; campaign.Data.inventory.Add("OLD_KEY");
            Assert.That(campaign.SaveCheckpointAtomic("PROLOG_COMPLETE"), Is.True);
            Assert.That(Phase1Director.PrepareNextNight(campaign),Is.True);
            Assert.That(campaign.Data.battery, Is.EqualTo(100)); Assert.That(campaign.Data.sanity, Is.Zero);
            Assert.That(campaign.Data.inventory, Is.EquivalentTo(new[]{"OLD_KEY"}));
            campaign.Data.battery=37;
            Assert.That(campaign.SaveCheckpointAtomic("PHASE1_CP1"),Is.True);
            Assert.That(Phase1Director.PrepareNextNight(campaign),Is.False);
            Assert.That(campaign.LoadCheckpoint(),Is.True);Assert.That(campaign.Data.battery,Is.EqualTo(37),"Continue is not another overnight charge");
        }
        [TestCase(1,"PHASE1_CP1")][TestCase(1,"PHASE1_CP3")][TestCase(1,"PHASE1_CP5")][TestCase(1,"PHASE1_CP6")][TestCase(1,"PHASE1_CP7")]
        public void EveryPhase1CheckpointRoutesDirectlyToRusunawa(int chapter,string checkpoint)
        { campaign.Data.chapter_id=chapter;campaign.Data.checkpoint_id=checkpoint;Assert.That(CampaignRouting.ContinueScene(campaign.Data),Is.EqualTo(CampaignRouting.RusunawaScene)); }
        [Test] public void UnknownChapterNeverRoutesToKosReset()
        { campaign.Data.chapter_id=5;Assert.That(CampaignRouting.ContinueScene(campaign.Data),Is.Null); }
        [Test] public void FullTransactionRollbackAlsoPreservesLastDiskSnapshot()
        {
            campaign.Data.battery=61; campaign.SaveCheckpointAtomic("PHASE1_CP5");
            string before=JsonUtility.ToJson(campaign.Data);
            Assert.That(campaign.TryCommit("PHASE1_CP6",d=>{d.inventory.Add("BAD_REWARD");d.secret_items.Add(Phase1Ids.Secret);d.wallet_bound=-1;}),Is.False);
            Assert.That(JsonUtility.ToJson(campaign.Data),Is.EqualTo(before));
            Assert.That(campaign.LoadCheckpoint(),Is.True); Assert.That(campaign.Data.checkpoint_id,Is.EqualTo("PHASE1_CP5"));
            Assert.That(campaign.Data.inventory,Does.Not.Contain("BAD_REWARD"));
        }
        [Test] public void BossBarrierBlocksAllOrdinarySaveEntryPointsBeforeMutation()
        {
            campaign.Data.wallet_bound=10000000;campaign.Data.inventory.Add("EVENT_CALLED_IBU");
            campaign.SaveCheckpointAtomic("PHASE1_CP5");string before=JsonUtility.ToJson(campaign.Data);
            object owner=new object();Assert.That(campaign.BeginCheckpointAttempt(owner),Is.True);
            Assert.That(campaign.ArchiveDocument("forbidden","body","DOC"),Is.False);
            Assert.That(campaign.TryPayFamilyBill(true),Is.False);
            Assert.That(campaign.SaveCheckpointAtomic("BAD"),Is.False);
            Assert.That(campaign.TryCommit("BAD",d=>d.inventory.Add("BAD")),Is.False);
            Assert.That(JsonUtility.ToJson(campaign.Data),Is.EqualTo(before));
            Assert.That(campaign.TryCommit("PHASE1_CP6",d=>d.phase1.Add("P1_BOSS_WIN"),owner),Is.True);
            campaign.EndCheckpointAttempt(owner);Assert.That(campaign.WritesBlocked,Is.False);
        }
        [Test] public void OwnerMismatchCannotUnlockFrozenCheckpoint()
        {var owner=new object();campaign.BeginCheckpointAttempt(owner);campaign.EndCheckpointAttempt(new object());Assert.That(campaign.WritesBlocked,Is.True);campaign.EndCheckpointAttempt(owner);}
        [Test] public void SecretFailureKeepsObjectEligibleAndSuccessIsOneId()
        {
            campaign.SaveCheckpointAtomic("PHASE1_CP4");
            Assert.That(campaign.TryCommit("PHASE1_CP4",d=>{d.secret_items.Add(Phase1Ids.Secret);d.battery=-1;}),Is.False);
            Assert.That(campaign.Data.secret_items,Is.Empty);
            Assert.That(campaign.TryCommit("PHASE1_CP4",d=>Phase1Ids.Grant(d,"P1_SECRET_COLLECTED",s=>s.secret_items.Add(Phase1Ids.Secret))),Is.True);
            Assert.That(Phase1Ids.Grant(campaign.Data,"P1_SECRET_COLLECTED",d=>d.secret_items.Add(Phase1Ids.Secret)),Is.False);
            Assert.That(campaign.LoadCheckpoint(),Is.True);Assert.That(campaign.Data.secret_items,Is.EqualTo(new[]{Phase1Ids.Secret}));
        }
        [Test] public void MailboxAndDonationReceiptsCannotDuplicateRewards()
        {
            for(int i=0;i<20;i++)Phase1Ids.Grant(campaign.Data,"P1_MAILBOX_207",d=>{d.inventory.Add(Phase1Ids.Key);d.inventory.Add(Phase1Ids.Powerbank);});
            for(int i=0;i<20;i++)Phase1Ids.Donation(campaign.Data,"P1_SAWER_SIGNBOARD",1000000,false);
            Assert.That(campaign.Data.inventory.Count,Is.EqualTo(2));Assert.That(campaign.Data.wallet_legitimate,Is.EqualTo(1000000));
            Assert.That(campaign.Data.phase1.money_ledger.Count,Is.EqualTo(1));
        }
        [TestCase(0,1,2)][TestCase(0,2,1)][TestCase(1,0,2)][TestCase(1,2,0)][TestCase(2,0,1)][TestCase(2,1,0)]
        public void AllSixEvidenceOrdersStillRequireExplicitJournalAndKeris(int a,int b,int c)
        {
            foreach(int i in new[]{a,b,c})if(!campaign.Data.evidence_ids.Contains(Phase1Ids.Evidence[i]))campaign.Data.evidence_ids.Add(Phase1Ids.Evidence[i]);
            campaign.Data.inventory.Add(Phase1Ids.Keris);
            Assert.That(Phase1Ids.EvidenceCount(campaign.Data),Is.EqualTo(3));Assert.That(Phase1Ids.Ready(campaign.Data),Is.False);
            campaign.Data.phase1.Add("P1_JOURNAL_CONFIRMED");Assert.That(Phase1Ids.Ready(campaign.Data),Is.True);
            campaign.Data.inventory.Remove(Phase1Ids.Keris);Assert.That(Phase1Ids.Ready(campaign.Data),Is.False);
        }
        [Test] public void BacktrackingCannotRegressStageOrWriteAnExitPanelReceipt()
        {
            campaign.Data.phase1.Add("P1_EPILOGUE");campaign.Data.phase1.Add("P1_CROSS_102");
            Assert.That(Phase1Ids.Stage(campaign.Data),Is.EqualTo(Phase1Stage.Return));
            Assert.That(campaign.Data.phase1.completed_event_ids.Any(x=>x.Contains("EXIT_PANEL")||x=="P1_LEAVE_RUSUN"),Is.False);
        }
        [TestCase(14.999f,true)][TestCase(15,false)][TestCase(9.99f,false)]
        public void ExposureHasOneFiveSecondWindow(float now,bool expected)
        {Assert.That(Phase1Ids.InWindow(now,10),Is.EqualTo(expected));}
        [Test] public void InvalidP1ReceiptsAndFutureCooldownTimesAreRejected()
        {
            campaign.Data.phase1.dynamo_emergency_last_uses.Add(new Phase1DynamoUse{id="DYNAMO_L1",at_gameplay_seconds=400});
            Assert.That(campaign.SaveCheckpointAtomic("PHASE1_CP4"),Is.False);
            campaign.Data.phase1.dynamo_emergency_last_uses.Clear();campaign.Data.phase1.completed_event_ids.Add("A");campaign.Data.phase1.completed_event_ids.Add("A");
            Assert.That(campaign.SaveCheckpointAtomic("PHASE1_CP4"),Is.False);
        }
        [Test] public void PursuitUsesRoomNodesWhenPlayerCapsuleIsAtTheGoal()
        {
            var actor=new GameObject("LargeActor").transform;actor.position=new Vector3(0,12,0);
            var player=new GameObject("TargetPlayer");player.transform.position=new Vector3(4,12,0);var capsule=player.AddComponent<CharacterController>();capsule.center=Vector3.up;capsule.height=2;
            var wall=new GameObject("RoomWall");wall.transform.position=new Vector3(2,13.2f,0);wall.AddComponent<BoxCollider>().size=new Vector3(.3f,2.4f,4);
            var a=new GameObject("CornerA").transform;a.position=new Vector3(0,12,3);
            var b=new GameObject("CornerB").transform;b.position=new Vector3(4,12,3);
            Physics.SyncTransforms();var nav=actor.gameObject.AddComponent<Phase1Navigator>();nav.Configure(new[]{a,b},actor,player.transform);
            nav.Step(player.transform.position,.5f);
            Assert.That(actor.position.z,Is.GreaterThan(.45f),"The player's collider must not invalidate every path goal");
            for(int i=0;i<43;i++)nav.Step(player.transform.position,.25f);
            Assert.That(Vector3.Distance(actor.position,player.transform.position),Is.LessThan(.3f),"Route goes around the real wall, not through it");
        }
        [Test] public void ThinApartmentWallPreventsFatalGenderuwoContact()
        {
            var director=new GameObject("Chapter").AddComponent<Phase1Director>();
            director.player=new GameObject("Player").AddComponent<LayarTerakhir.Player.FirstPersonController>();
            director.player.transform.position=new Vector3(1.1f,0,0);
            director.world=director.gameObject.AddComponent<Phase1World>();
            director.world.genderuwo=new GameObject("Genderuwo");director.world.kuntilanak=new GameObject("Kunti");
            var threats=director.gameObject.AddComponent<Phase1Threats>();threats.director=director;
            var contact=typeof(Phase1Threats).GetMethod("ContactReachesPlayer",BindingFlags.Instance|BindingFlags.NonPublic);
            Physics.SyncTransforms();Assert.That(contact.Invoke(threats,null),Is.True,"Unobstructed physical contact remains fatal");
            var wall=new GameObject("ThinApartmentPartition");wall.transform.position=new Vector3(.55f,1,0);
            wall.AddComponent<BoxCollider>().size=new Vector3(.12f,2,2);Physics.SyncTransforms();
            Assert.That(contact.Invoke(threats,null),Is.False,"An actor on the opposite side of the wall cannot catch the player");
            wall.transform.position=Vector3.forward*3;Physics.SyncTransforms();
            Assert.That(contact.Invoke(threats,null),Is.True,"An open passage restores contact");
        }
        [Test] public void RusunawaUsesIndependentEnvironmentSurfacesAtMeterScale()
        {
            EditorSceneManager.OpenScene(Phase1SceneBuilder.ScenePath);
            var environment=GameObject.Find("Rusunawa_GriyaAsri_Environment");Assert.That(environment,Is.Not.Null);
            var panels=environment.GetComponentsInChildren<MeshRenderer>(true).Where(r=>r.name.StartsWith("SourcePanel_")).ToArray();
            Assert.That(panels.Length,Is.GreaterThan(100));
            foreach(var panel in panels)
            {
                var material=panel.sharedMaterial;
                Assert.That(AssetDatabase.GetAssetPath(material.GetTexture("_BaseMap")),Does.StartWith("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/"));
                Assert.That(material.GetTextureScale("_BaseMap").x,Is.LessThanOrEqualTo(3.01f),"Centimeter FBX vertices must not make textures repeat 100x too often");
                Assert.That(AssetDatabase.GetAssetPath(panel.GetComponent<MeshFilter>().sharedMesh),Does.Contain("ConcreteInterior/"));
            }
            Assert.That(GameObject.Find("L3_NorthStructuralFloor"),Is.Not.Null,"The sealed storey must still physically support the floor above");
            foreach(var slab in environment.GetComponentsInChildren<Transform>().Where(t=>t.name=="FacadeSlabEdge"))
            {
                var normals=slab.GetComponentsInChildren<MeshFilter>().SelectMany(f=>f.sharedMesh.normals.Select(n=>f.transform.TransformDirection(n).normalized)).ToArray();
                foreach(var direction in new[]{Vector3.up,Vector3.down,Vector3.left,Vector3.right,Vector3.forward,Vector3.back})
                    Assert.That(normals.Any(n=>Vector3.Dot(n,direction)>.9f),Is.True,"Slabs need all six visible sides, whether the publisher mesh uses one renderer or several");
            }
        }
        [TestCase("BoltCutter_HandReach",.35f,.12f,.75f)]
        [TestCase("K403_UsableDoor",1.97f,2.6f,.12f)]
        [TestCase("DownloadedSafetyChain",1.95f,1.3f,.3f)]
        public void DownloadedPropFitsItsPhysicalWorldDimensions(string name,float x,float y,float z)
        {
            EditorSceneManager.OpenScene(Phase1SceneBuilder.ScenePath);
            var root=GameObject.Find(name);Assert.That(root,Is.Not.Null);
            var renderers=root.GetComponentsInChildren<Renderer>();var bounds=renderers[0].bounds;
            foreach(var renderer in renderers)bounds.Encapsulate(renderer.bounds);
            Assert.That(Vector3.Distance(bounds.size,new Vector3(x,y,z)),Is.LessThan(.02f),"FBX import-axis rotation and scaled parents must not distort source props");
            Assert.That(Vector3.Distance(bounds.center,root.transform.position),Is.LessThan(.02f));
            var collider=root.GetComponent<BoxCollider>();
            if(collider)Assert.That(Vector3.Distance(collider.bounds.size,bounds.size),Is.LessThan(.02f));
        }
        [Test] public void ThirtyAdditionalModelsArePlacedWithoutBlockingGalleryCentres()
        {
            EditorSceneManager.OpenScene(Phase1SceneBuilder.ScenePath);
            var group=GameObject.Find("ThirtyAdditionalDownloadedProps");Assert.That(group,Is.Not.Null);
            var props=group.GetComponentsInChildren<Transform>().Where(t=>t.parent==group.transform&&t.name.StartsWith("P1NewProp_")).ToArray();
            var ids=props.Select(p=>p.name.Substring("P1NewProp_".Length).Split(new[]{"__"},StringSplitOptions.None)[0]).ToArray();
            Assert.That(ids.Distinct().Count(),Is.GreaterThanOrEqualTo(30),"Repeated instances and texture channels do not count as new models");
            var manifest=JsonUtility.FromJson<AdditionalPropManifest>(File.ReadAllText("Audit/phase1-additional-prop-sources.json"));
            Assert.That(ids,Is.EquivalentTo(manifest.records.Select(p=>p.id)),"Every newly downloaded model must actually be placed");
            var oldIds=Directory.GetDirectories("Assets/_LayarTerakhir/assets/Art/Imported/PolyHaven").Select(Path.GetFileName)
                .Concat(Directory.GetFiles("Assets/_LayarTerakhir/assets/Art/Atmaja/Models","*.fbx").Select(Path.GetFileNameWithoutExtension)).Select(s=>s.ToLowerInvariant());
            Assert.That(ids.Intersect(oldIds).Any(),Is.False,"The thirty additions must not reuse Prolog prop sources");
            foreach(var prop in props)
            {
                var filters=prop.GetComponentsInChildren<MeshFilter>();Assert.That(filters.Length,Is.GreaterThan(0));
                Assert.That(filters.All(f=>AssetDatabase.GetAssetPath(f.sharedMesh).StartsWith("Assets/_LayarTerakhir/assets/Art/Phase1/Imported/",StringComparison.Ordinal)),Is.True);
            }
            Physics.SyncTransforms();
            for(int floor=0;floor<4;floor++)
            {
                float y=floor*4;
                var route=new[]{new Vector3(-15,y,-11),new Vector3(15,y,-11),new Vector3(15,y,11),new Vector3(-15,y,11),new Vector3(-15,y,-11)};
                for(int edge=0;edge<4;edge++)for(float t=0;t<=1;t+=.05f)
                {
                    var p=Vector3.Lerp(route[edge],route[edge+1],t);
                    var hits=Physics.OverlapCapsule(p+Vector3.up*.52f,p+Vector3.up*1.75f,.5f,~0,QueryTriggerInteraction.Ignore);
                    foreach(var hit in hits)
                        Assert.That(hit.transform.GetComponentsInParent<Transform>().Any(a=>a.name.StartsWith("P1NewProp_")),Is.False,"New dressing blocks an authored corridor: "+hit.name+" near "+p);
                }
            }
        }
        [Test] public void KitchenRelicHasAnUnobstructedHandReachFromItsApproach()
        {
            EditorSceneManager.OpenScene(Phase1SceneBuilder.ScenePath);
            var relic=GameObject.Find("PecahanKerisPenumbal");Assert.That(relic,Is.Not.Null);
            Vector3 hand=new Vector3(20,13.06f,4),delta=relic.transform.position-hand;
            Physics.SyncTransforms();
            var hits=Physics.RaycastAll(hand,delta.normalized,delta.magnitude+.1f,~0,QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance).ToArray();
            Assert.That(hits.Length,Is.GreaterThan(0));
            Assert.That(hits[0].collider.transform.IsChildOf(relic.transform),Is.True,"A decorative rack/stove must not block the required relic: "+hits[0].collider.name);
        }
    }
}
#endif
