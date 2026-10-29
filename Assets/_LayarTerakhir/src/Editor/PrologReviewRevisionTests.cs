#if UNITY_EDITOR
using System.Linq;
using System.Reflection;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class PrologReviewRevisionTests
    {
        private AtmajaDirector d;
        private Transform[] objects;
        private GameObject Find(string name)=>objects.First(t=>t.name==name).gameObject;
        [SetUp]public void Open()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);d=Object.FindFirstObjectByType<AtmajaDirector>();
            objects=UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)).ToArray();
        }
        [TearDown]public void Close()=>EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        [Test]public void ChildCommentsDoNotInventToysOrDolls()
        {
            var comments=(string[])typeof(AtmajaPhoneView).GetMethod("RoomComments",BindingFlags.NonPublic|BindingFlags.Static).Invoke(null,new object[]{"child"});
            Assert.That(comments.Length,Is.GreaterThan(0));
            Assert.That(comments.Any(t=>t.ToLowerInvariant().Contains("mainan")||t.ToLowerInvariant().Contains("boneka")),Is.False);
        }
        [Test]public void ChildRoomHasAnImportedCompleteBedAndReadableBookGeometry()
        {
            foreach(string name in new[]{"ChildWardrobe","ChildMattress","ChildPillow","ChildBlanket","ChildToyBlocks","ChildToyBall"})Assert.That(objects.Any(t=>t.name==name),Is.False,name);
            var bed=Find("ChildBedSet");var b=KosVisualV2.BoundsOf(bed);
            Assert.That(b.size.y,Is.GreaterThan(.9f));Assert.That(b.size.z,Is.GreaterThan(1.9f));
            Assert.That(bed.GetComponentsInChildren<MeshFilter>().Any(f=>AssetDatabase.GetAssetPath(f.sharedMesh).Contains("vintage_day_bed")),Is.True);
            var diary=Find("ChildDiaryBook");Assert.That(diary.GetComponent<AtmajaItem>().action,Is.EqualTo(AtmajaAction.Diary));
            Assert.That(diary.GetComponent<Renderer>().enabled,Is.False);
            Assert.That(Find("BimaNotebookVisual").GetComponentsInChildren<MeshFilter>().Sum(f=>f.sharedMesh.vertexCount),Is.GreaterThan(100));
            Assert.That(Quaternion.Angle(Find("ChildDeskLamp").transform.rotation,Quaternion.LookRotation(d.north)),Is.LessThan(.1f),"Repeated authoring must retain the corrected lamp direction");
            var pool=Find("ChildStudyPool").GetComponent<Light>();
            Assert.That(Vector3.Dot(pool.transform.forward,(Find("ChildDiaryBook").transform.position-pool.transform.position).normalized),Is.GreaterThan(.5f));
        }
        [Test]public void BathroomHasOneFloorFinishAndScratchesStayOnTheWall()
        {
            Assert.That(objects.Any(t=>t.name=="UpperFloor_South"),Is.False);
            Assert.That(objects.Where(t=>t.name.StartsWith("UpperSlab_")).SelectMany(t=>t.GetComponents<Renderer>()).All(r=>!r.enabled),Is.True);
            Assert.That(Find("BathFloor").GetComponent<Renderer>().enabled,Is.True);
            Assert.That(d.upper.bathScratchesOnWall,Is.True);Assert.That(d.upper.bathScratchesAR.transform.IsChildOf(d.upper.bathDoor.transform),Is.False);
            Vector3 before=d.upper.scratchAim.position;
            d.upper.bathDoor.SetOpenImmediate(true);
            typeof(AtmajaUpperDirector).GetMethod("SyncScratchTarget",BindingFlags.NonPublic|BindingFlags.Instance).Invoke(d.upper,null);
            Assert.That(Vector3.Distance(before,d.upper.scratchAim.position),Is.LessThan(.001f));
            Assert.That(Find("BathScratchTarget").GetComponent<AtmajaItem>().label,Does.Contain("dinding"));
            Assert.That(d.upper.bathScratchesAR.GetComponentsInChildren<MeshRenderer>(true).Count(),Is.EqualTo(3));
        }
        [Test]public void TubHasAnOpenBasinWithBloodInsideTheRim()
        {
            var tub=Find("BathTub");var bounds=KosVisualV2.BoundsOf(tub);
            Assert.That(tub.GetComponentsInChildren<MeshFilter>().Any(f=>AssetDatabase.GetAssetPath(f.sharedMesh).Contains("ClawfootBathtub")),Is.True);
            d.upper.bathWater.SetActive(true);var water=KosVisualV2.BoundsOf(d.upper.bathWater);
            Assert.That(water.max.y,Is.LessThan(bounds.max.y-.03f));Assert.That(water.min.y,Is.GreaterThan(bounds.min.y+.2f));
            Assert.That(water.size.x,Is.LessThan(bounds.size.x));Assert.That(water.size.z,Is.LessThan(bounds.size.z));
            Assert.That(d.upper.bathWater.GetComponent<Renderer>().sharedMaterial.color.r,Is.GreaterThan(.15f));
            // Mesh raycast, independent of the conservative outer gameplay collider.
            float lowest=float.PositiveInfinity;
            foreach(var f in tub.GetComponentsInChildren<MeshFilter>())
            {
                var temporary=f.gameObject.AddComponent<MeshCollider>();temporary.sharedMesh=f.sharedMesh;
                if(temporary.Raycast(new Ray(bounds.center+Vector3.up,Vector3.down),out var hit,3))lowest=Mathf.Min(lowest,hit.point.y);
                Object.DestroyImmediate(temporary);
            }
            Assert.That(lowest,Is.LessThan(bounds.max.y-.15f),"The centre is a real cavity rather than a solid slab");
        }
        [Test]public void MasterAndCellarHaveClearSpaceAndVisibleAscent()
        {
            Assert.That(d.basement.nyonya.GetComponentsInChildren<SkinnedMeshRenderer>(true).Any(r=>r.name=="NyonyaDesignerBody"),Is.True,"Keep the supplied animated hunter bound to the chase");
            Assert.That(objects.Any(t=>t.name=="CorrLintelN"),Is.False);
            var wall=d.LocalPosition(KosVisualV2.BoundsOf(Find("MasterEastWallExpanded")).center);
            Assert.That(wall.x,Is.GreaterThan(4.7f));
            Assert.That(KosVisualV2.BoundsOf(Find("MasterBedSet")).size.x,Is.GreaterThan(1.5f));
            foreach(string name in new[]{"CellarSpineNorth","CellarSpineSouth","CellarReturnPartition"})Assert.That(objects.Any(t=>t.name==name),Is.False);
            Assert.That(KosVisualV2.BoundsOf(Find("CellarFloor")).size.x,Is.GreaterThan(11));
            Assert.That(d.LocalPosition(KosVisualV2.BoundsOf(Find("FamilyRadioCabinet")).center).x,Is.LessThan(.2f),"Furniture must stay outside the vertical ladder shaft");
            var ladder=KosVisualV2.BoundsOf(Find("CellarAccessLadder"));Assert.That(ladder.min.y,Is.LessThan(-2.7f));Assert.That(ladder.max.y,Is.GreaterThan(3.7f));
            var path=d.upper.CellarDescentPath();Assert.That(path.Length,Is.GreaterThan(20));
            Assert.That(path.Last().y,Is.EqualTo(-2.79f).Within(.02f));
            Assert.That(d.upper.CellarAscentPath().Last(),Is.EqualTo(d.upper.trapdoorTop.position));
            Assert.That(Find("PrologServingFood").transform.childCount,Is.GreaterThanOrEqualTo(30));
        }
        [Test]public void HidingMeterUsesSmallCumulativeGreenYellowRedSegments()
        {
            var hud=Object.FindFirstObjectByType<HudController>();hud.EnsureRuntimeUIBuilt();
            foreach(float danger in new[]{0f,.5f,1f})
            {
                hud.SetThreatBar(danger);
                var meter=hud.GetComponentInChildren<Canvas>().transform.Find("ThreatBar");
                var rect=meter.GetComponent<RectTransform>();Assert.That(rect.anchorMax,Is.EqualTo(new Vector2(1,0)));Assert.That(rect.sizeDelta.x,Is.LessThan(64));
                var segments=meter.GetComponentsInChildren<Image>().Where(i=>i.name.StartsWith("ThreatSegment")).OrderBy(i=>i.name).ToArray();
                Assert.That(segments.Length,Is.EqualTo(9));
                Assert.That(segments.Count(i=>i.color.a>.5f),Is.EqualTo(danger==0?3:danger<1?6:9));
                Assert.That(segments[0].color.g,Is.GreaterThan(segments[0].color.r));Assert.That(segments[8].color.r,Is.GreaterThan(segments[8].color.g));
            }
            hud.SetThreatBar(null);Assert.That(hud.threatBarFill.transform.parent.gameObject.activeSelf,Is.False);
        }
    }
}
#endif
