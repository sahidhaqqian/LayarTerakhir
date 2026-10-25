#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase4;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.TestTools;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase4SceneTests
    {
        private Phase4Director d;
        [SetUp]public void Setup(){EditorSceneManager.OpenScene(Phase4SceneBuilder.ScenePath);d=UnityEngine.Object.FindFirstObjectByType<Phase4Director>();}
        [TearDown]public void Cleanup(){EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);}
        [Test]public void ChapterUsesExactlyOneExistingPlayerPhoneCampaignAndOneDirector()
        {
            Assert.That(d,Is.Not.Null);Assert.That(UnityEngine.Object.FindObjectsByType<FirstPersonController>(FindObjectsSortMode.None).Length,Is.EqualTo(1));Assert.That(UnityEngine.Object.FindObjectsByType<SmartphoneController>(FindObjectsSortMode.None).Length,Is.EqualTo(1));Assert.That(UnityEngine.Object.FindObjectsByType<CampaignState>(FindObjectsSortMode.None).Length,Is.EqualTo(1));
            foreach(var root in d.gameObject.scene.GetRootGameObjects())foreach(var b in root.GetComponentsInChildren<MonoBehaviour>(true))Assert.That(b&&MonoScript.FromMonoBehaviour(b),Is.True,root.name);
        }
        [Test]public void AtLeastOneHundredDistinctNewDownloadedModelsAreActuallyPlacedWithPurpose()
        {
            var tags=UnityEngine.Object.FindObjectsByType<Phase4AssetSource>(FindObjectsInactive.Include,FindObjectsSortMode.None);Assert.That(tags.Select(t=>t.sourceId).Distinct().Count(),Is.GreaterThanOrEqualTo(100));
            foreach(var t in tags){Assert.That(t.sourcePath,Does.StartWith("Assets/_LayarTerakhir/assets/Art/Phase4/Imported/"));Assert.That(File.Exists(t.sourcePath),Is.True,t.sourcePath);Assert.That(string.IsNullOrWhiteSpace(t.purpose),Is.False);Assert.That(t.GetComponentsInChildren<Renderer>(true).Length,Is.GreaterThan(0));}
        }
        [Test]public void TwelveTorchesAreFourSectorsOfThreeAndAllHaveRealPhysicalTargets()
        {Assert.That(d.world.torches.Length,Is.EqualTo(12));foreach(int s in Enumerable.Range(0,4)){var ts=d.world.torches.Where(t=>t.sector==s).ToArray();Assert.That(ts.Select(t=>t.candidate),Is.EquivalentTo(new[]{0,1,2}));foreach(var t in ts){Assert.That(t.focus&&t.flame&&t.authenticMark&&t.decoyMark&&t.practical,Is.True);Assert.That(t.GetComponent<Phase4WorldItem>().GetComponent<Collider>(),Is.Not.Null);}}}
        [Test]public void AllTwelvePouringApproachesStayOutsideThreatRepellingWards()
        {foreach(var t in d.world.torches)Assert.That(d.world.InWard(t.transform.position+Vector3.back*1.3f),Is.False,"Threat must be able to approach sector "+t.sector+" candidate "+t.candidate);}
        [TestCase("J1")][TestCase("PENDOPO")][TestCase("NORTH")][TestCase("EAST")][TestCase("WEST")][TestCase("SOUTH")]
        public void AllMandatoryAreasHaveCompletePhysicalNavigationFromForestEntry(string id)
        {Assert.That(NavMesh.SamplePosition(d.world.Anchor("ENTRY").position,out var a,.8f,NavMesh.AllAreas),Is.True);Assert.That(NavMesh.SamplePosition(d.world.Anchor(id).position,out var b,.8f,NavMesh.AllAreas),Is.True,id);var p=new NavMeshPath();NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,p);Assert.That(p.status,Is.EqualTo(NavMeshPathStatus.PathComplete),id);}
        [UnityTest]public IEnumerator AuthoredStoneStepsConnectPortalToTheRealUndergroundBoundary()
        {
            d.world.portalCover.SetActive(false);d.world.portalStairs.SetActive(true);
            Assert.That(NavMesh.SamplePosition(d.world.Anchor("PORTAL").position,out var a,.8f,NavMesh.AllAreas),Is.True);
            Assert.That(NavMesh.SamplePosition(d.world.Anchor("BOUNDARY").position,out var b,.8f,NavMesh.AllAreas),Is.True);
            var p=new NavMeshPath();
            for(int frame=0;frame<60;frame++)
            {
                NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,p);if(p.status==NavMeshPathStatus.PathComplete)break;
                EditorApplication.QueuePlayerLoopUpdate();yield return null;
            }
            Assert.That(p.status,Is.EqualTo(NavMeshPathStatus.PathComplete));
        }
        [Test]public void AllNewPhotogrammetrySourcesKeepDownloadedAlbedoAfterSerialization()
        {string[] heroes={"Joglo","BambooSet","Banyan","Keris","StoneWell","HumanSkeleton","Robe","ElderHead","ForestStairs"};foreach(var source in UnityEngine.Object.FindObjectsByType<Phase4AssetSource>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(s=>!heroes.Contains(s.sourceId)))Assert.That(source.GetComponentsInChildren<Renderer>(true).Any(r=>r.sharedMaterials.Any(m=>m&&m.HasProperty("_BaseMap")&&m.GetTexture("_BaseMap"))),Is.True,source.sourceId);}
        [Test]public void FlatRelicsAndCongklakBoardsKeepPlausiblePhysicalDimensions()
        {
            foreach(var source in UnityEngine.Object.FindObjectsByType<Phase4AssetSource>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(s=>s.sourceId=="Keris"||s.sourceId.StartsWith("sungka_board")))
            {var rs=source.GetComponentsInChildren<Renderer>(true);var b=rs[0].bounds;foreach(var r in rs.Skip(1))b.Encapsulate(r.bounds);Assert.That(Mathf.Max(b.size.x,b.size.y,b.size.z),Is.LessThan(1.5f),source.name);}
        }
        [Test]public void AltarPhotoHasARealClearARLineFromTheGubukEntrance()
        {Physics.SyncTransforms();Assert.That(d.ClearLine(new Vector3(29,2.4f,-.2f),d.world.arTarget.position,d.world.arOcclusionRoot,true),Is.True);}
        [Test]public void ThermalGraveHasARealClearLineFromItsReachableApproach()
        {
            Physics.SyncTransforms();Assert.That(NavMesh.SamplePosition(new Vector3(0,0,26.5f),out var p,2,NavMesh.AllAreas),Is.True);
            var from=p.position+Vector3.up*1.6f;
            Assert.That(d.ClearLine(from,d.world.thermalTarget.position,d.world.thermalOcclusionRoot),Is.True,OpticalHits(from,d.world.thermalTarget.position));
        }
        [TestCase(0)][TestCase(1)][TestCase(2)][TestCase(3)]
        public void EveryTorchCanBeInspectedWithoutUnrelatedPropOcclusion(int sector)
        {
            Physics.SyncTransforms();foreach(var torch in d.world.torches.Where(t=>t.sector==sector))
            {var from=torch.transform.position+Vector3.back*1.3f+Vector3.up*1.6f;Assert.That(d.ClearLine(from,torch.focus.position,torch.transform),Is.True,torch.name+" / "+OpticalHits(from,torch.focus.position));}
        }
        private static string OpticalHits(Vector3 from,Vector3 to)
        {return string.Join(" / ",Physics.RaycastAll(from,(to-from).normalized,Vector3.Distance(from,to)-.04f,~((1<<24)|(1<<26)|(1<<29)|(1<<2)|(1<<31)),QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance).Select(h=>h.collider.name+" ["+h.collider.GetComponentInParent<Phase4AssetSource>()?.purpose+"] at="+h.point));}
        [TestCase(0)][TestCase(1)][TestCase(2)]
        public void EveryOfferingCanBeTargetedWithoutTheRitualTriggerCoveringIt(int slot)
        {
            Physics.SyncTransforms();var item=UnityEngine.Object.FindObjectsByType<Phase4WorldItem>(FindObjectsSortMode.None).Single(i=>i.id=="ALTAR_"+slot);
            Assert.That(NavMesh.SamplePosition(new Vector3(-2+slot*2,0,-1.3f),out var stance,2,NavMesh.AllAreas),Is.True);
            Assert.That(Vector3.Distance(stance.position+Vector3.up,item.transform.position),Is.LessThanOrEqualTo(d.player.GetComponent<InteractionInteractor>().interactionDistance-.2f),"The real standing space must be within interaction reach");
            var from=stance.position+Vector3.up*1.6f;var delta=item.transform.position-from;
            var hits=Physics.RaycastAll(from,delta.normalized,delta.magnitude+.2f,~0,QueryTriggerInteraction.Collide).OrderBy(h=>h.distance).ToArray();
            Assert.That(hits,Is.Not.Empty);Assert.That(hits[0].collider.GetComponentInParent<Phase4WorldItem>(),Is.EqualTo(item),string.Join(" / ",hits.Select(h=>h.collider.name)));
        }
        [Test]public void GroundFinishedFacePointsUpToReceiveMoonAndPracticalLight()
        {
            var ground=UnityEngine.Object.FindObjectsByType<Phase4AssetSource>(FindObjectsSortMode.None).Single(s=>s.purpose=="Forest_Ground");
            float up=0,down=0;
            foreach(var mf in ground.GetComponentsInChildren<MeshFilter>())
            {
                var v=mf.sharedMesh.vertices;var t=mf.sharedMesh.triangles;
                for(int i=0;i<t.Length;i+=3){var n=Vector3.Cross(mf.transform.TransformPoint(v[t[i+1]])-mf.transform.TransformPoint(v[t[i]]),mf.transform.TransformPoint(v[t[i+2]])-mf.transform.TransformPoint(v[t[i]]));if(n.y>0)up+=n.magnitude;else down+=n.magnitude;}
            }
            Assert.That(up,Is.GreaterThan(down*2),"The broad ground face must receive light from above");
        }
        [Test]public void PendopoApproachRoutesAroundTheCrankInsteadOfClimbingAbovePlayerStepHeight()
        {
            Assert.That(NavMesh.SamplePosition(new Vector3(-6,0,-6),out var a,.8f,NavMesh.AllAreas),Is.True);
            Assert.That(NavMesh.SamplePosition(new Vector3(.5f,0,-3.2f),out var b,.8f,NavMesh.AllAreas),Is.True);
            var p=new NavMeshPath();NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,p);
            Assert.That(p.status,Is.EqualTo(NavMeshPathStatus.PathComplete));
            Assert.That(p.corners.Max(v=>v.y),Is.LessThan(.35f),"A floor route must not climb the 90cm crank");
        }
        [Test]public void ForestSurfaceUsesReadableRepeatsRatherThanThousandsOfTiles()
        {
            var ground=UnityEngine.Object.FindObjectsByType<Phase4AssetSource>(FindObjectsSortMode.None).Single(s=>s.purpose=="Forest_Ground");
            var mf=ground.GetComponentInChildren<MeshFilter>();var uv=mf.sharedMesh.uv;var scale=mf.GetComponent<Renderer>().sharedMaterial.GetTextureScale("_BaseMap");
            Assert.That((uv.Max(v=>v.x)-uv.Min(v=>v.x))*scale.x,Is.InRange(10,70));
            Assert.That((uv.Max(v=>v.y)-uv.Min(v=>v.y))*scale.y,Is.InRange(10,70));
        }
        [TestCase(-28,-.5f,false)][TestCase(-31,1.5f,true)]
        public void WellAuthenticRecordingsAreInRangeFromReachableGround(float x,float z,bool secret)
        {
            Assert.That(NavMesh.SamplePosition(new Vector3(x,0,z),out var spot,2,NavMesh.AllAreas),Is.True);
            Assert.That(Vector3.Distance(spot.position+Vector3.up,secret?d.world.secretKnock.position:d.world.wellMantra.position),Is.LessThanOrEqualTo(d.balance.evpRange));
        }
        [Test]public void NarrowRoadTextureHasMetreScaleDetailAlongBothPhysicalAxes()
        {
            var source=UnityEngine.Object.FindObjectsByType<Phase4AssetSource>(FindObjectsSortMode.None).Single(s=>s.purpose=="Village_CenterSouth");
            var mf=source.GetComponentInChildren<MeshFilter>();var mesh=mf.sharedMesh;var v=mesh.vertices;var uv=mesh.uv;var t=mesh.triangles;float best=0;int at=0;
            for(int i=0;i<t.Length;i+=3){float area=Vector3.Cross(mf.transform.TransformVector(v[t[i+1]]-v[t[i]]),mf.transform.TransformVector(v[t[i+2]]-v[t[i]])).magnitude;if(area>best){best=area;at=i;}}
            var a=mf.transform.TransformVector(v[t[at+1]]-v[t[at]]);var b=mf.transform.TransformVector(v[t[at+2]]-v[t[at]]);var x=uv[t[at+1]]-uv[t[at]];var y=uv[t[at+2]]-uv[t[at]];float determinant=x.x*y.y-x.y*y.x;
            var u=(a*y.y-b*x.y)/determinant;var w=(b*x.x-a*y.x)/determinant;var scale=mf.GetComponent<Renderer>().sharedMaterial.GetTextureScale("_BaseMap");
            Assert.That(u.magnitude/Mathf.Abs(scale.x),Is.InRange(1.25f,3.5f),"Road detail along U must not stretch to20m");Assert.That(w.magnitude/Mathf.Abs(scale.y),Is.InRange(1.25f,3.5f),"Road detail along V must not compress to25cm");
        }
        [Test]public void GubukApproachUsesRealStepsInsteadOfTheSixtyFiveCentimetreLedge()
        {
            Assert.That(NavMesh.SamplePosition(new Vector3(24,0,-4),out var a,1,NavMesh.AllAreas),Is.True);
            Assert.That(NavMesh.SamplePosition(new Vector3(29,.65f,-.1f),out var b,1,NavMesh.AllAreas),Is.True);
            var path=new NavMeshPath();NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path);Assert.That(path.status,Is.EqualTo(NavMeshPathStatus.PathComplete));
            foreach(var corner in path.corners.Where(p=>p.y>.35f&&p.z<.5f))Assert.That(corner.x,Is.InRange(27.5f,30.5f),"Raised entrance must go through the real stair opening");
        }
        [Test]public void PortalCoverReallyOpensAndDoesNotLeaveAnInvisibleFloor()
        {d.world.portalCover.SetActive(false);Physics.SyncTransforms();var hits=Physics.RaycastAll(new Vector3(0,.2f,7),Vector3.down,.9f,~0,QueryTriggerInteraction.Ignore);Assert.That(hits.Any(h=>h.normal.y>.5f),Is.False,string.Join(",",hits.Select(h=>h.collider.name)));}
        [Test]public void EverySectorOffersAccessibleBatterySanityAndWaterWithoutMoney()
        {var items=UnityEngine.Object.FindObjectsByType<Phase4WorldItem>(FindObjectsInactive.Include,FindObjectsSortMode.None);foreach(var s in new[]{"NORTH","EAST","WEST","SOUTH"})foreach(var prefix in new[]{"CHARGE_","SHRINE_","REFILL_"})Assert.That(items.Any(i=>i.id==prefix+s),Is.True,prefix+s);Assert.That(d.world.wards.Length,Is.GreaterThanOrEqualTo(7));}
        [Test]public void IllusionHasTheExistingARMaskAndDoesNotEraseOtherCollisions()
        {foreach(var t in d.world.illusionWall.GetComponentsInChildren<Transform>())Assert.That(t.gameObject.layer,Is.EqualTo(30));Assert.That(d.world.illusionWall.GetComponentsInChildren<Collider>().Length,Is.GreaterThan(0));Assert.That(d.world.arOcclusionRoot,Is.Not.EqualTo(d.world.illusionWall.transform));Assert.That(d.world.thermalOcclusionRoot,Is.Not.EqualTo(GameObject.Find("Desa_Mati_Karang_Setra").transform));}
        [Test]public void EveryHideHasOpaquePhysicalCoverAndEveryInteractionHasACollider()
        {foreach(var i in UnityEngine.Object.FindObjectsByType<Phase4WorldItem>(FindObjectsInactive.Include,FindObjectsSortMode.None)){Assert.That(i.GetComponentInChildren<Collider>(),Is.Not.Null,i.id);if(i.id.StartsWith("HIDE_"))Assert.That(i.transform.Find("HideInside"),Is.Not.Null,i.id);}}
        [Test]public void PBRMapsAndVolumeOverridesSurviveSerialization()
        {
            foreach(string id in new[]{"forest_ground_04","bamboo_wall_03","wood_planks_dirt","burned_ground_01","aerial_rocks_04"}){var m=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Phase4/Materials/Ground_"+id+".mat");Assert.That(m,Is.Not.Null,id);Assert.That(m.GetTexture("_BaseMap"),Is.Not.Null,id);Assert.That(m.GetTexture("_BumpMap"),Is.Not.Null,id);}
            var profile=AssetDatabase.LoadAssetAtPath<VolumeProfile>("Assets/_LayarTerakhir/assets/Art/Phase4/KarangSetraVolume.asset");Assert.That(profile.components.All(c=>c&&AssetDatabase.Contains(c)),Is.True);
        }
        [Test]public void NoMissingShadersOrMissingAudioAndHazardsAlwaysHaveAWarning()
        {
            foreach(var r in UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include,FindObjectsSortMode.None))foreach(var m in r.sharedMaterials)Assert.That(m&&m.shader&&m.shader.name!="Hidden/InternalErrorShader",Is.True,r.name);
            foreach(var id in new[]{"Fire","Heartbeat","Water","Ambient","Spirit"})Assert.That(d.soundscape.sounds.First(s=>s.id==id).clip,Is.Not.Null,id);
            foreach(var h in UnityEngine.Object.FindObjectsByType<Phase4ForestHazard>(FindObjectsSortMode.None))Assert.That(h.warning&&h.warningSeconds>=1,Is.True);
        }
    }
}
#endif
