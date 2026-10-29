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
        [Test]public void OriginalCupHasARealCavityRecessedFractureAndOpenHandle()
        {
            var cup=Find("BimaCrackedCup");
            var shell=cup.GetComponentsInChildren<MeshFilter>().Single(f=>f.name=="PorcelainShell");
            var handle=cup.GetComponentsInChildren<MeshFilter>().Single(f=>f.name=="PorcelainHandle");
            Assert.That(AssetDatabase.GetAssetPath(shell.sharedMesh),Is.EqualTo(PrologReviewRevision.Art+"Models/CrackedPorcelainCup.fbx"));
            var bounds=shell.GetComponent<Renderer>().bounds;
            Assert.That(bounds.size.y,Is.EqualTo(.095f).Within(.001f));
            Assert.That(bounds.size.x/bounds.size.z,Is.EqualTo(1).Within(.02f),"Preserve the authored circular body");
            Assert.That(cup.GetComponentsInChildren<MeshFilter>().Sum(f=>f.sharedMesh.triangles.Length/3),Is.LessThan(14000));
            var collider=shell.gameObject.AddComponent<MeshCollider>();collider.sharedMesh=shell.sharedMesh;
            Assert.That(collider.Raycast(new Ray(bounds.center+Vector3.up*.15f,Vector3.down),out var bottom,.3f),Is.True);
            Assert.That(bottom.point.y,Is.LessThan(bounds.max.y-.065f),"The mouth opens into the bowl, not a solid cap");
            Object.DestroyImmediate(collider);
            var axis=shell.transform.TransformPoint(Vector3.zero);
            var vertices=shell.sharedMesh.vertices.Select(shell.transform.TransformPoint).ToArray();
            float Radius(Vector3 p)=>Vector2.Distance(new Vector2(p.x,p.z),new Vector2(axis.x,axis.z));
            var renderer=shell.GetComponent<Renderer>();var slots=renderer.sharedMaterials;
            int fracture=System.Array.FindIndex(slots,m=>m.name=="BimaPorcelainFracture");
            int core=System.Array.FindIndex(slots,m=>m.name=="BimaPorcelainCore");
            Assert.That(fracture,Is.GreaterThanOrEqualTo(0));Assert.That(core,Is.GreaterThanOrEqualTo(0));
            var cuts=shell.sharedMesh.GetTriangles(fracture).Select(i=>vertices[i]).Where(v=>v.y>axis.y+.055f&&v.y<axis.y+.087f).ToArray();
            bool recessed=cuts.Any(v=>{
                float outer=vertices.Where(p=>Mathf.Abs(p.y-v.y)<.00005f).Max(Radius);
                float inset=outer-Radius(v);return inset>.00015f&&inset<.0008f;});
            Assert.That(recessed,Is.True,"The crack must indent the ceramic wall, not just darken its material");
            Assert.That(shell.sharedMesh.GetTriangles(core).Select(i=>vertices[i]).Any(v=>v.y<bounds.max.y-.0007f&&v.y>bounds.max.y-.004f),Is.True,"Expose actual chipped ceramic at the rim");
            var handleCollider=handle.gameObject.AddComponent<MeshCollider>();handleCollider.sharedMesh=handle.sharedMesh;
            var hole=axis+cup.transform.right*.052f+Vector3.up*.055f;
            Assert.That(handleCollider.Raycast(new Ray(hole-cup.transform.forward*.05f,cup.transform.forward),out _, .1f),Is.False,"The handle's grip remains open");
            Object.DestroyImmediate(handleCollider);
            var milk=Find("SpoiledMilk").GetComponent<Renderer>().bounds;
            Assert.That(milk.max.y,Is.LessThan(bounds.max.y-.01f),"Milk stays below the chipped lip");
        }
        [Test]public void OriginalCupRestsOnDeskAndRetainsItsEvidenceTarget()
        {
            var cup=Find("BimaCrackedCup");var bounds=KosVisualV2.BoundsOf(cup);
            var desk=KosVisualV2.BoundsOf(Find("ChildDeskTop"));
            Assert.That(bounds.min.y-desk.max.y,Is.InRange(0f,.003f),"The cup must rest on the desktop");
            Assert.That(d.upper.childCup.GetComponent<AtmajaItem>().action,Is.EqualTo(AtmajaAction.CupPhoto));
            Assert.That(d.upper.childCup.GetComponent<Renderer>().enabled,Is.False,"Retire the old primitive visual");
            var target=d.upper.childCup.GetComponent<BoxCollider>();Physics.SyncTransforms();
            Assert.That(target.bounds.Contains(d.upper.cupAim.position),Is.True);
            Vector3 lens=d.upper.cupAim.position+d.north*.5f+Vector3.up*.12f;
            Vector3 ray=d.upper.cupAim.position-lens;
            var hits=Physics.RaycastAll(lens,ray.normalized,ray.magnitude,~0,QueryTriggerInteraction.Ignore).OrderBy(h=>h.distance).ToArray();
            Assert.That(hits.Length,Is.GreaterThan(0));Assert.That(hits[0].collider,Is.SameAs(target),"The photo target is reachable from the front of the desk");
        }
        [Test]public void ChildRoomHasAnImportedCompleteBedAndReadableBookGeometry()
        {
            foreach(string name in new[]{"ChildWardrobe","ChildMattress","ChildPillow","ChildBlanket","ChildToyBlocks","ChildToyBall"})Assert.That(objects.Any(t=>t.name==name),Is.False,name);
            var bed=Find("ChildBedSet");var b=KosVisualV2.BoundsOf(bed);
            Assert.That(b.size.y,Is.GreaterThan(.9f));Assert.That(b.size.z,Is.GreaterThan(1.9f));
            Assert.That(bed.GetComponentsInChildren<MeshFilter>().Any(f=>AssetDatabase.GetAssetPath(f.sharedMesh).Contains("PrologSingleBedWithBedding")),Is.True);
            Assert.That(bed.GetComponentsInChildren<Transform>().Any(t=>t.name.Contains("mattress")),Is.True,"Keep the fitted mattress in the set");
            Assert.That(bed.GetComponentsInChildren<Transform>().Any(t=>t.name.Contains("pillow")),Is.True,"Keep the pillow in the set");
            Assert.That(bed.GetComponentsInChildren<Transform>().Any(t=>t.name.Contains("duvet")),Is.True,"Keep the duvet in the set");
            var diary=Find("ChildDiaryBook");Assert.That(diary.GetComponent<AtmajaItem>().action,Is.EqualTo(AtmajaAction.Diary));
            Assert.That(diary.GetComponent<Renderer>().enabled,Is.False);
            var notebook=Find("BimaNotebookVisual");Assert.That(notebook.GetComponentsInChildren<MeshFilter>().Sum(f=>f.sharedMesh.vertexCount),Is.GreaterThan(100));
            var printedPages=objects.Where(t=>t.name.StartsWith("BimaPrintedPage")).Select(t=>t.GetComponent<Renderer>()).ToArray();
            Assert.That(printedPages.Length,Is.EqualTo(2),"Both pages must carry visible writing");
            Assert.That(printedPages.All(r=>r.sharedMaterial.mainTexture&&r.sharedMaterial.mainTexture.name.StartsWith("BimaDiaryPage")),Is.True,"The physical diary pages must show Bima's note, not blank paper");
            Vector3 lampToBook=Find("ChildDiaryBook").transform.position-Find("ChildDeskLamp").transform.position;lampToBook.y=0;
            Assert.That(Vector3.Dot(Find("ChildDeskLamp").transform.forward,lampToBook.normalized),Is.GreaterThan(.95f),"Repeated authoring must keep the lamp aimed toward the notebook");
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
            Assert.That(d.upper.bathScratchesAR.GetComponentsInChildren<MeshRenderer>(true).Count(),Is.EqualTo(1));
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
            var back=d.LocalPosition(KosVisualV2.BoundsOf(Find("MasterBalconySeparation")).center);
            Assert.That(back.y,Is.GreaterThan(4.2f),"The room's back wall must leave a larger, usable bedroom depth");
            var chair=Find("MasterRocker");var towardBed=Find("MasterBedSet").transform.position-chair.transform.position;towardBed.y=0;
            Assert.That(Vector3.Dot(chair.transform.forward,towardBed.normalized),Is.GreaterThan(.9f),"The chair must face into the room toward the bed");
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
