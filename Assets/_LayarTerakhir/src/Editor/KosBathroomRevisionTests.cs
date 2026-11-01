#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    // Designer round Nov 2026 (goal.md): dining leftover wall, open bathroom foyer,
    // sealed main entrance, expanded natural bathrooms. EditMode pins only.
    public sealed class KosBathroomRevisionTests
    {
        [SetUp] public void Setup() =>
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);

        [TearDown] public void Cleanup() =>
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);

        private static Transform Root() =>
            GameObject.Find("Kos_Building_Corridors").transform;

        private static Vector3 Local(Transform t) =>
            Root().InverseTransformPoint(t.position);

        [Test] public void BathroomFoyerStaysOpenWithOnlyTwoGenderDoors()
        {
            Assert.That(GameObject.Find("KAMAR MANDIDoor"),Is.Null,"No entrance door into the bathroom");
            Assert.That(GameObject.Find("KAMAR MANDIHinge"),Is.Null);
            Assert.That(GameObject.Find("BathPocketDoorTrack"),Is.Null);
            foreach (string gender in new[] { "PRIA", "WANITA" })
            {
                var door = GameObject.Find("Bathroom" + gender + "Door");
                Assert.That(door,Is.Not.Null,gender + " cubicle door remains");
                var hinge = GameObject.Find("Bathroom" + gender + "Hinge");
                Assert.That(hinge,Is.Not.Null,gender + " hinge remains");
                Assert.That(door.GetComponent<HingedDoor>(),Is.Not.Null);
            }
            // Foyer portal frame (sides + header + sign) stays as open architecture.
            Assert.That(GameObject.Find("KAMAR MANDIFrontLow"),Is.Not.Null);
            Assert.That(GameObject.Find("KAMAR MANDIFrontHigh"),Is.Not.Null);
            Assert.That(GameObject.Find("KAMAR MANDILintel"),Is.Not.Null);
        }

        [Test] public void DiningPortalHasNoFloatingTrim()
        {
            var root = Root();
            var strays = root.GetComponentsInChildren<Transform>(true).Where(t =>
                t && (t.name == "HallSageWainscot" || t.name == "HallChairRail") &&
                Mathf.Abs(root.InverseTransformPoint(t.position).x + 1.39f) < .06f &&
                Mathf.Abs(root.InverseTransformPoint(t.position).z - 6f) < .06f &&
                root.InverseTransformPoint(t.position).y < 2f).ToArray();
            Assert.That(strays,Is.Empty,"No wainscot/chair-rail floats across the merged Kitchen+Dining portal, got: " +
                string.Join(",",strays.Select(t=>t.name)));
        }

        [Test] public void MainEntranceLeafSealsItsFrame()
        {
            var leaf = GameObject.Find("KosMainEntranceDoor");
            Assert.That(leaf,Is.Not.Null);
            var doors = leaf.GetComponentsInChildren<HingedDoor>(true);
            Assert.That(doors.Length,Is.EqualTo(1),"Exactly one operable entrance leaf");
            var door = doors[0];
            Assert.That(door.hinge,Is.Not.Null);
            Assert.That(door.locked,Is.False);
            door.SetOpenImmediate(false);
            var bounds = leaf.GetComponent<Collider>().bounds;
            var root = Root();
            var lo = root.InverseTransformPoint(bounds.min);
            var hi = root.InverseTransformPoint(bounds.max);
            Assert.That(lo.x,Is.LessThanOrEqualTo(-.85f),"Leaf covers the left jamb gap, min.x=" + lo.x);
            Assert.That(hi.x,Is.GreaterThanOrEqualTo(.85f),"Leaf covers the right jamb gap, max.x=" + hi.x);
            var corridor = Object.FindFirstObjectByType<KosCorridorDirector>();
            Assert.That(corridor.entranceDoor,Is.SameAs(door));
        }

        [Test] public void PorcelainToiletsFaceTheirDoorways()
        {
            foreach (float z in new[] { 3.30f, 5.20f })
            {
                var toilet = GameObject.Find("KosPorcelainToilet_" + z);
                Assert.That(toilet,Is.Not.Null);
                float yaw = ((toilet.transform.eulerAngles.y % 360) + 360) % 360;
                Assert.That(yaw,Is.EqualTo(270f).Within(.5f),"Toilet bowl faces the cubicle door (-X), yaw=" + yaw);
            }
        }

        [Test] public void ExpandedBathroomFixturesArePresent()
        {
            foreach (string name in new[]
            {
                "KosBathroomMirror","KosBathroomShelf","KosToothbrushCup",
                "KosToothbrushBlue","KosToothbrushPink","KosOdolBottle",
                "KosBathTub_PRIA","KosBathTub_WANITA",
                "KosBathBucket_PRIA","KosBathBucket_WANITA","KosToiletPaper_PRIA","KosToiletPaper_WANITA",
                "KosBathroomLight_PRIA","KosBathroomLight_WANITA","KosKitchenWashBasin","KosWashBasin",
            }) Assert.That(GameObject.Find(name),Is.Not.Null,name);
            // Modelled tubs hold their own visible water below the rim.
            foreach (string tag in new[] { "PRIA", "WANITA" })
            {
                var tub = GameObject.Find("KosBathTub_" + tag).GetComponent<Collider>().bounds;
                var inner = GameObject.Find("KosBathTub_" + tag).GetComponentsInChildren<Renderer>(true)
                    .Select(r => r.bounds).Where(b => b.size.magnitude < tub.size.magnitude * .95f &&
                        tub.Contains(b.min) && tub.Contains(b.max)).ToArray();
                Assert.That(inner.Length,Is.GreaterThanOrEqualTo(1),tag + " tub holds modelled water");
                var water = inner[0];
                Assert.That(water.min.y,Is.GreaterThan(tub.min.y + .05f));
                Assert.That(water.max.y,Is.LessThan(tub.max.y - .02f));
                Assert.That(water.center.x,Is.EqualTo(tub.center.x).Within(.1f));
            }
            // Kitchen basin backs against the south wall instead of floating mid-room.
            var basin = Local(GameObject.Find("KosKitchenWashBasin").transform);
            Assert.That(basin.z,Is.LessThan(3f),"Basin z=" + basin.z);
        }
    }
}
#endif
