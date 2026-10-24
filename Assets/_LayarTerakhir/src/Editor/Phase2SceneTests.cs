#if UNITY_EDITOR
using System;
using System.Linq;
using LayarTerakhir.Phase2;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase2SceneTests
    {
        private Phase2Director d;
        [SetUp]public void Open()
        {EditorSceneManager.OpenScene(Phase2SceneBuilder.ScenePath);d=UnityEngine.Object.FindFirstObjectByType<Phase2Director>();Physics.SyncTransforms();}
        [TearDown]public void Close()=>EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        [Test]public void SerializedInvestigationSceneHasAllOwnersAndPhysicalTargets()
        {
            Assert.That(d,Is.Not.Null);Assert.That(d.world,Is.Not.Null);Assert.That(d.player,Is.Not.Null);Assert.That(d.phoneView,Is.Not.Null);
            Assert.That(d.world.incubatorSources.Length,Is.EqualTo(6));Assert.That(d.world.safePanel,Is.Not.Null);Assert.That(d.balance,Is.Not.Null);Assert.That(d.lastri.agent,Is.Not.Null);Assert.That(d.world.drawers.Length,Is.EqualTo(12));
            foreach(var key in new[]{"IGD","BASEMENT","NURSE"})Assert.That(d.world.anchors.Any(a=>a.id==key&&a.point),Is.True,key);
            foreach(var s in d.soundscape.sounds)Assert.That(s.clip,Is.Not.Null,"Missing cue "+s.id);
            Assert.That(d.world.thermalTargets.Count(t=>t.temperature< -20),Is.EqualTo(1));
            Assert.That(d.world.thermalTargets.Single(t=>t.temperature< -20).id,Is.EqualTo("C3"));
            Assert.That(d.world.thermalTargets.Single(t=>t.id=="POLI03").temperature,Is.EqualTo(-16));
        }
        [Test]public void EveryHospitalMeshUsesNewlyImportedSourcesWithAtLeastThirtyDistinctModels()
        {
            var root=GameObject.Find("RSUD_Lama_SidoWaras");Assert.That(root,Is.Not.Null);
            var paths=root.GetComponentsInChildren<MeshFilter>(true).Select(m=>AssetDatabase.GetAssetPath(m.sharedMesh)).Distinct().ToArray();
            foreach(var path in paths)Assert.That(path.StartsWith("Assets/_LayarTerakhir/assets/Art/Phase2/") || path=="Assets/_LayarTerakhir/assets/Art/Atmaja/Models/DamarScooter.fbx",Is.True,path+" must be new hospital art, except Raka's carried motorcycle");
            Assert.That(paths.Length,Is.GreaterThanOrEqualTo(30));
        }
        [Test]public void RitualSceneHasRealSourcesAndNoGeneratorOrBeamOutsideReach()
        {
            Assert.That(d.boss,Is.Not.Null);Assert.That(d.world.ritualLights.Length,Is.EqualTo(3));Assert.That(d.world.illusions.Length,Is.EqualTo(2));
            Assert.That(d.world.breakableGlass.Length,Is.EqualTo(4));Assert.That(d.world.burntCloth.GetComponentsInChildren<MeshFilter>(true).Length,Is.GreaterThan(0));
            Assert.That(d.world.videoSet.GetComponentsInChildren<SkinnedMeshRenderer>(true).Length,Is.GreaterThan(0));Assert.That(d.world.videoSet.activeSelf,Is.False);
            Assert.That(d.world.operationDoor,Is.Not.Null);Assert.That(d.world.arenaLock.GetComponent<Collider>(),Is.Not.Null);
            for(int i=0;i<3;i++)
            {
                var toggle=UnityEngine.Object.FindObjectsByType<Phase2WorldItem>(FindObjectsSortMode.None).Single(t=>t.id=="LAMP_"+i);
                Vector3 delta=toggle.transform.position-d.world.beamCentres[i].position;delta.y=0;
                Assert.That(delta.magnitude,Is.LessThanOrEqualTo(i==0?4:3),"Switch too far from its source");
                Assert.That(d.boss.InsideBeam(d.world.beamCentres[i].position+Vector3.forward*.7f,i),Is.True,"Source blocked by dressing");
                float headPlane=d.world.beamCentres[i].position.y+1.7f;
                float radiusAtHead=Mathf.Tan(d.world.ritualLights[i].spotAngle*.5f*Mathf.Deg2Rad)*(d.world.ritualLights[i].transform.position.y-headPlane);
                Assert.That(radiusAtHead,Is.GreaterThanOrEqualTo(d.world.beamRadii[i]),"Visible light must reach the head across its gameplay beam");
            }
            foreach(var glass in d.world.breakableGlass)Assert.That(glass.GetComponentsInChildren<Renderer>().Length,Is.GreaterThan(0));
            Assert.That(d.world.videoCamera.position.x,Is.GreaterThan(60),"Transmission is a separate non-playable set");
        }
        [Test]public void MovingDrawerGeometryRemainsInteractableDuringCancelWindow()
        {
            foreach(var drawer in d.world.drawers)
            {
                drawer.tray.localPosition+=drawer.slideOffset;
                foreach(var solid in drawer.tray.GetComponentsInChildren<Collider>())
                    Assert.That(solid.GetComponentInParent<LayarTerakhir.Player.IInteractable>(),Is.SameAs(drawer.GetComponent<Phase2WorldItem>()),drawer.id+" moving geometry must resolve its own E interaction");
            }
        }
        [Test]public void NoMissingScriptsOrDuplicateChapterDirectors()
        {
            Assert.That(UnityEngine.Object.FindObjectsByType<Phase2Director>(FindObjectsSortMode.None).Length,Is.EqualTo(1));
            foreach(var go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
                foreach(var behaviour in go.GetComponentsInChildren<MonoBehaviour>(true))Assert.That(behaviour,Is.Not.Null,go.name);
            Assert.That(UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Prolog.KamarKosPrologDirector>(),Is.Null);
            Assert.That(UnityEngine.Object.FindFirstObjectByType<LayarTerakhir.Phase1.Phase1Director>(),Is.Null);
        }
        [TestCase(18,4,18,39.4f,4,18)][TestCase(39.4f,4,18,46.4f,4,22)][TestCase(39.4f,4,18,37.6f,4,14)]
        [TestCase(39.4f,4,18,44.5f,4,18)][TestCase(39.4f,4,18,43,4,22.5f)][TestCase(39.4f,4,18,43,4,13.5f)]
        [TestCase(5,0,-10,34,-4,17)][TestCase(34,-4,17,18,-4,22)]
        [TestCase(34,-4,17,12,-4,31)][TestCase(34,-4,17,34,-4,24)]
        [TestCase(5,0,-10,18,4,12)][TestCase(18,4,12,13,4,22)]
        [TestCase(13,4,22,10,4,26)][TestCase(18,4,12,34,4,17)]
        public void BakedMultiFloorNavigationConnectsEveryCriticalArea(float ax,float ay,float az,float bx,float by,float bz)
        {
            Assert.That(NavMesh.SamplePosition(new Vector3(ax,ay,az),out var a,1.5f,NavMesh.AllAreas),Is.True,"Start nav surface");
            Assert.That(NavMesh.SamplePosition(new Vector3(bx,by,bz),out var b,1.5f,NavMesh.AllAreas),Is.True,"End nav surface");
            var path=new NavMeshPath();Assert.That(NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path),Is.True);
            Assert.That(path.status,Is.EqualTo(NavMeshPathStatus.PathComplete),"Disconnected floor / inaccessible hospital room");
        }
        [Test]public void LastriVisibilityDoesNotReuseMirrorArOrGripLayers()
        {
            Assert.That(new[]{26,27,28,29,30,31}.Contains(Phase2Lastri.VisibilityLayer),Is.False);
            foreach(var renderer in d.world.lastri.GetComponentsInChildren<Renderer>())Assert.That(renderer.gameObject.layer,Is.EqualTo(Phase2Lastri.VisibilityLayer));
        }
        [Test]public void OpenC3ContentsAreVisibleAboveItsPhysicalTray()
        {
            // Exercise the actual authored translation, not an ignored-wall aim ray.
            var drawer=d.world.drawers.Single(x=>x.id=="C3");drawer.tray.localPosition+=drawer.slideOffset;
            d.world.c3Contents.SetActive(true);Physics.SyncTransforms();
            Assert.That(d.ClearLine(new Vector3(27.1f,-2.35f,22),d.world.auraTarget.position),Is.True,
                "The open cold-storage mesh must not occlude the required AR photo");
        }
        [Test]public void LastriHasExistingNamedMotionAndHumanScaleInTheSavedScene()
        {
            var animation=d.world.lastri.GetComponentInChildren<UnityEngine.Animation>();Assert.That(animation["Idle"],Is.Not.Null);Assert.That(animation["Walk"],Is.Not.Null);
            animation.Play("Idle");animation.Sample();var skin=d.world.lastri.GetComponentInChildren<SkinnedMeshRenderer>();
            var bounds=Phase2SceneBuilder.WorldSkinBounds(skin);
            Assert.That(bounds.size.y,Is.InRange(1.6f,2f));Assert.That(bounds.min.y,Is.EqualTo(d.world.lastri.transform.position.y).Within(.08f));
        }
        [Test]public void IncubatorRetainsSourceGlassAndAnUnoccludedEvpSource()
        {
            var station=GameObject.Find("Inkubator_04_Placement");var glass=station.GetComponentsInChildren<Renderer>().Where(r=>r.sharedMaterials.All(m=>m.GetFloat("_Surface")>.5f)).ToArray();
            Assert.That(glass.Length,Is.GreaterThan(0));foreach(var r in glass){Assert.That(r.GetComponent<Collider>(),Is.Null);Assert.That(r.sharedMaterial.GetColor("_BaseColor").a,Is.LessThan(.3f));}
            Assert.That(d.ClearLine(new Vector3(10,5.6f,26),d.world.evpTarget.position),Is.True);
        }
        [Test]public void ArWallCanRecognizeItsOwnSurfaceButNeverAnInterveningWall()
        {
            Vector3 from=new Vector3(13,5.3f,18.5f),target=d.world.wallTarget.position;
            Assert.That(d.ClearLine(from,target),Is.False,"The authored physical wall really blocks passage");
            Assert.That(d.ClearLine(from,target,d.world.inkubatorWall.transform),Is.True,"AR recognizes the selected wall itself");
            var obstacle=new GameObject("Intervening solid").AddComponent<BoxCollider>();
            obstacle.transform.position=(from+target)*.5f;obstacle.size=new Vector3(.6f,1,.15f);Physics.SyncTransforms();
            Assert.That(d.ClearLine(from,target,d.world.inkubatorWall.transform),Is.False,"Other geometry still occludes the scan");
        }
        [Test]public void SafeDoorOwnsTheImportedLeafAndExposesTheBloodWhenOpen()
        {
            Assert.That(d.world.safeDoor.hinge.GetComponentsInChildren<Renderer>(true).Length,Is.GreaterThan(0),
                "Model-prefab children must be unpacked before reparenting to the hinge");
            var target=d.world.blood.GetComponentsInChildren<Renderer>(true)[0].bounds.center;
            d.world.safeDoor.SetOpenImmediate(false);Physics.SyncTransforms();
            Assert.That(d.ClearLine(new Vector3(22,5.6f,9.1f),target),Is.False);
            d.world.safeDoor.SetOpenImmediate(true);Physics.SyncTransforms();
            Assert.That(d.ClearLine(new Vector3(22,5.6f,9.1f),target),Is.True,
                "The opened front must expose the relic to the real interaction ray");
        }
    }
}
#endif
