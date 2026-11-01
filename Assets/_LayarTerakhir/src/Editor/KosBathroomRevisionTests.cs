#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    // Designer rounds Nov 2026 (goal.md): dining notch sealed, exit swings east/outward,
    // toilets face their doors, buckets/tubs removed, dispenser raised+turned, fridge faces kitchen.
    // EditMode pins only.
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
            // Hinge on the EAST jamb, swings outward east (left when entering).
            var hingeLocal = Local(door.hinge);
            Assert.That(hingeLocal.x,Is.EqualTo(.9f).Within(.05f),"Hinge x=" + hingeLocal.x);
            Assert.That(door.angle,Is.LessThan(0f),"Swing angle=" + door.angle);
            door.SetOpenImmediate(false);
            // Open pose must land OUTSIDE (z < -1) on the EAST side (x > 0).
            door.hinge.localRotation = Quaternion.Euler(0, door.angle, 0);
            var openCenter = Local(leaf.transform);
            door.hinge.localRotation = Quaternion.identity;
            door.SetOpenImmediate(false);
            Assert.That(openCenter.z,Is.LessThan(-1f),"Open leaf stays outside, z=" + openCenter.z);
            Assert.That(openCenter.x,Is.GreaterThan(0f),"Open leaf swings east/left, x=" + openCenter.x);
            var bounds = leaf.GetComponent<Collider>().bounds;
            var root = Root();
            var lo = root.InverseTransformPoint(bounds.min);
            var hi = root.InverseTransformPoint(bounds.max);
            Assert.That(lo.x,Is.LessThanOrEqualTo(-.85f),"Leaf covers the left jamb gap, min.x=" + lo.x);
            Assert.That(hi.x,Is.GreaterThanOrEqualTo(.85f),"Leaf covers the right jamb gap, max.x=" + hi.x);
            Assert.That(hi.y,Is.GreaterThanOrEqualTo(2.28f),"Leaf seals the top slit, max.y=" + hi.y);
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
                Assert.That(yaw,Is.EqualTo(90f).Within(.5f),"Toilet bowl faces the cubicle door (-X), yaw=" + yaw);
            }
        }

        [Test] public void DiningWestNotchIsSealed()
        {
            var seal = GameObject.Find("DiningWestJogSeal");
            Assert.That(seal,Is.Not.Null);
            var b = seal.GetComponent<Collider>().bounds;
            var root = Root();
            var lo = root.InverseTransformPoint(b.min);
            var hi = root.InverseTransformPoint(b.max);
            Assert.That(lo.x,Is.LessThanOrEqualTo(-6.2f),"Seal reaches the outer wall, min.x=" + lo.x);
            Assert.That(hi.x,Is.GreaterThanOrEqualTo(-4.85f),"Seal reaches the kitchen wall, max.x=" + hi.x);
            Assert.That(hi.y,Is.GreaterThanOrEqualTo(2.8f),"Seal full height, max.y=" + hi.y);
        }

        [Test] public void FridgeFacesTheKitchenNotTheDiningTable()
        {
            var fridge = GameObject.Find("KosRefrigerator");
            Assert.That(fridge,Is.Not.Null);
            float yaw = ((fridge.transform.eulerAngles.y % 360) + 360) % 360;
            Assert.That(yaw,Is.EqualTo(180f).Within(.5f),"Fridge faces east into the kitchen, yaw=" + yaw);
        }

        [Test] public void ExpandedBathroomFixturesArePresent()
        {
            foreach (string name in new[]
            {
                "KosBathroomMirror","KosBathroomShelf","KosToothbrushCup",
                "KosToothbrushBlue","KosToothbrushPink","KosOdolBottle",
                "KosToiletPaper_PRIA","KosToiletPaper_WANITA",
                "KosBathroomLight_PRIA","KosBathroomLight_WANITA","KosKitchenWashBasin","KosWashBasin",
            }) Assert.That(GameObject.Find(name),Is.Not.Null,name);
            // Designer: ugly buckets and tub water removed.
            foreach (string gone in new[]
            {
                "KosBathTub_PRIA","KosBathTub_WANITA",
                "KosBathBucket_PRIA","KosBathBucket_WANITA",
                "KosBathWater_PRIA","KosBathWater_WANITA",
            }) Assert.That(GameObject.Find(gone),Is.Null,gone + " removed");
            // Raised dispenser station: shelf above the old height, bottle turned to face the room.
            var shelf = Local(GameObject.Find("KosBathroomShelf").transform);
            Assert.That(shelf.y,Is.GreaterThan(1.25f),"Shelf raised, y=" + shelf.y);
            var bottle = GameObject.Find("KosOdolBottle");
            float bottleYaw = ((bottle.transform.eulerAngles.y % 360) + 360) % 360;
            Assert.That(bottleYaw,Is.EqualTo(180f).Within(.5f),"Dispenser faces the room, yaw=" + bottleYaw);
            // Kitchen basin backs against the south wall instead of floating mid-room.
            var basin = Local(GameObject.Find("KosKitchenWashBasin").transform);
            Assert.That(basin.z,Is.LessThan(3f),"Basin z=" + basin.z);
        }
    }
}
#endif
