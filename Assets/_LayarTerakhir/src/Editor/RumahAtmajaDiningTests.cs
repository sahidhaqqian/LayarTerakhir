#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    // goal.md Nov: meja ruang makan, jarum jam, pintu dan dinding Rumah Atmaja.
    public sealed class RumahAtmajaDiningTests
    {
        private static GameObject Find(string name) =>
            Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .FirstOrDefault(t => t && t.name == name)?.gameObject;

        private static AtmajaDirector Open()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            Physics.SyncTransforms();
            return Object.FindFirstObjectByType<AtmajaDirector>();
        }

        private static Bounds VisualBounds(GameObject go)
        {
            var rs = go.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled && !(r is LineRenderer)).ToArray();
            Assert.That(rs.Length, Is.GreaterThan(0), go.name + " has no enabled renderers");
            var b = rs[0].bounds;
            foreach (var r in rs.Skip(1)) b.Encapsulate(r.bounds);
            return b;
        }

        [Test]
        public void DiningTableSurfaceSupportsTheFeast()
        {
            Open();
            var table = Find("LongDiningTable");
            Assert.That(table && table.activeInHierarchy, Is.True, "CC0 dining table exists and is active");
            var tb = VisualBounds(table);
            Assert.That(tb.max.y, Is.EqualTo(1.42f).Within(.03f), "Table surface at collision height");
            Assert.That(tb.size.x, Is.GreaterThan(1.2f), "Tabletop spans the place settings");
            Assert.That(tb.size.z, Is.GreaterThan(3.0f), "Tabletop spans the feast length");
            var tcol = Find("DiningTableTopCollision");
            Assert.That(tcol, Is.Not.Null, "Invisible table collision box exists");
            var cb = tcol.GetComponent<Collider>().bounds;
            Assert.That(Mathf.Abs(cb.size.x - tb.size.x), Is.LessThan(.12f), "Collision matches visual width (no invisible overhang)");
            Assert.That(Mathf.Abs(cb.size.z - tb.size.z), Is.LessThan(.12f), "Collision matches visual length");
            var mats = table.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled)
                .SelectMany(r => r.sharedMaterials).Where(m => m).Distinct().ToArray();
            Assert.That(mats.Length, Is.GreaterThan(0), "Table has real materials");
            foreach (var m in mats)
                Assert.That(m.shader && m.shader.name != "Hidden/InternalErrorShader", Is.True, "Table material shader is valid: " + m.name);
            Assert.That(mats.Any(m => m.HasProperty("_BaseMap") && m.GetTexture("_BaseMap")), Is.True, "Table has its CC0 diffuse wired, not flat gray");
            var feast = Object.FindObjectsByType<Transform>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Where(t => t && (t.name.StartsWith("AddedDinner_") || t.name.StartsWith("AddedDish_") || t.name.StartsWith("DiningCenter_"))).ToArray();
            Assert.That(feast.Length, Is.AtLeast(20), "Bug-39 feast survives");
            foreach (var t in feast)
            {
                var b = VisualBounds(t.gameObject);
                Assert.That(b.min.y, Is.AtLeast(tb.max.y - .02f), t.name + " is not sunk into the table");
                Assert.That(b.min.y, Is.AtMost(tb.max.y + .10f), t.name + " rests on the table, not floating");
                Assert.That(b.center.x, Is.EqualTo(tb.center.x).Within(tb.extents.x + .3f), t.name + " sits over the table");
            }
        }

        [Test]
        public void ClockHandsRestAboveTheirTable()
        {
            var d = Open();
            var side = GameObject.Find("CornerSideTable");
            var hands = GameObject.Find("BrassClockHands");
            Assert.That(side && hands, Is.True, "Side table and brass hands both exist");
            var sb = VisualBounds(side);
            var hp = hands.transform.position;
            Assert.That(sb.Contains(hp), Is.False, "Hands are not buried inside the table mesh");
            Assert.That(hp.y - sb.max.y, Is.InRange(.005f, .06f), "Hands rest on the tabletop");
            Assert.That(Mathf.Abs(hp.x - sb.center.x), Is.LessThan(sb.extents.x), "Hands are over the table");
            var tray = GameObject.Find("OldAshtray");
            if (tray) Assert.That(tray.transform.position.y - sb.max.y, Is.InRange(0f, .05f), "Ashtray rests on the tabletop");
            Assert.That(d.clockHands, Is.SameAs(hands), "Director still binds the same hands object");
            var sideMats = side.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled)
                .SelectMany(r => r.sharedMaterials).Where(m => m).Distinct().ToArray();
            Assert.That(sideMats.Any(m => m.HasProperty("_BaseMap") && m.GetTexture("_BaseMap")), Is.True, "Side table has its CC0 diffuse wired");
        }

        [Test]
        public void DiningDoorHingeSurvivesSerialization()
        {
            var d = Open();
            var hinge = GameObject.Find("KitchenDoorHinge");
            var leaf = GameObject.Find("KitchenDiningDoor");
            Assert.That(hinge && leaf, Is.True, "Dining hinge and leaf exist");
            var motion = hinge.GetComponent<HingedDoor>();
            Assert.That(motion, Is.Not.Null, "Dining hinge is serialized, not only runtime-added");
            var lb = VisualBounds(leaf);
            var hp = hinge.transform.position;
            float edgeGap = Mathf.Min(Mathf.Abs(lb.min.x - hp.x), Mathf.Abs(lb.max.x - hp.x));
            Assert.That(edgeGap, Is.LessThan(.3f), "Leaf edge meets the hinge axis");
            foreach (string n in new[] { "FrontSittingDoor", "LockedCentralStairDoor", "ChildBedroomDoor", "BathroomDoor", "MasterBedroomDoor", "GrandfatherClock" })
                Assert.That(GameObject.Find(n), Is.Not.Null, n + " loads");
            Assert.That(d.clockHour && d.clockMinute, Is.True, "Clock blades survive for the time puzzle");
        }

        [Test]
        public void SittingRadioClearsHandsDesireLine()
        {
            var d = Open();
            var radio = GameObject.Find("FamilyRadioCabinet");
            Assert.That(radio, Is.Not.Null, "Radio cabinet exists");
            Assert.That(d.LocalPosition(radio.transform.position).x, Is.GreaterThan(-.5f), "Radio is parked east, out of the door-to-hands flow");
            GameObject.Find("FrontSittingDoor").GetComponent<HingedDoor>().SetOpenImmediate(true);
            Physics.SyncTransforms();
            var cc = d.player.GetComponent<CharacterController>();
            var bad = new System.Collections.Generic.List<string>();
            Vector3 a = d.P(-1.4f, 2.5f, .54f), b = d.P(-4.3f, 1.5f, .54f);
            for (int i = 0; i <= 20; i++)
            {
                var p = Vector3.Lerp(a, d.P(-1.4f, 1.5f, .54f), i / 20f);
                if (!ClearAt(p)) bad.Add("seg1:" + d.LocalPosition(p));
            }
            for (int i = 0; i <= 20; i++)
            {
                var p = Vector3.Lerp(d.P(-1.4f, 1.5f, .54f), b, i / 20f);
                if (!ClearAt(p)) bad.Add("seg2:" + d.LocalPosition(p));
            }
            Assert.That(bad, Is.Empty, "Door-to-hands desire line blocked at " + string.Join(" ", bad));
            bool ClearAt(Vector3 p)
            {
                var hits = Physics.OverlapCapsule(p + Vector3.up * cc.radius, p + Vector3.up * (cc.height - cc.radius), cc.radius, ~0, QueryTriggerInteraction.Ignore)
                    .Where(c => c != cc && !c.transform.IsChildOf(d.player.transform)).ToArray();
                return hits.Length == 0;
            }
        }

        [Test]
        public void PrologDoorsAndClockLoadWithValidMaterials()
        {
            Open();
            foreach (string n in new[] { "KitchenDiningDoor", "FrontSittingDoor", "LockedCentralStairDoor", "GrandfatherClock", "ChildBedroomDoor", "BathroomDoor", "MasterBedroomDoor" })
            {
                var go = Find(n);
                Assert.That(go && go.activeInHierarchy, Is.True, n + " loads");
                var mats = go.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled)
                    .SelectMany(r => r.sharedMaterials).Where(m => m).Distinct().ToArray();
                Assert.That(mats.Length, Is.GreaterThan(0), n + " has real materials");
                foreach (var m in mats)
                    Assert.That(m.shader && m.shader.name != "Hidden/InternalErrorShader", Is.True, n + " material shader valid: " + m.name);
            }
        }

        [Test]
        public void GrandfatherClockHandsAreOnTheDial()
        {
            var d = Open();
            var clock = GameObject.Find("GrandfatherClock");
            Assert.That(clock, Is.Not.Null);
            var parts = clock.GetComponentsInChildren<Transform>(true);
            var hour = parts.FirstOrDefault(t => t && t.name.StartsWith("ClockHourBlade__"));
            var minute = parts.FirstOrDefault(t => t && t.name.StartsWith("ClockMinuteBlade__"));
            var pend = parts.FirstOrDefault(t => t && t.name.StartsWith("ClockPendulum__"));
            Assert.That(hour && minute && pend, Is.True, "Clockwork parts exist");
            Assert.That(Vector3.Distance(hour.position, d.clockDialCentre), Is.LessThan(.15f), "Hour hand sits on the dial");
            Assert.That(Vector3.Distance(minute.position, d.clockDialCentre), Is.LessThan(.15f), "Minute hand sits on the dial");
            Assert.That(pend.position.y, Is.GreaterThan(.9f).And.LessThan(2.2f), "Pendulum hangs inside the case");
            Assert.That(Vector3.Distance(new Vector3(pend.position.x, 0, pend.position.z), new Vector3(d.clockDialCentre.x, 0, d.clockDialCentre.z)), Is.LessThan(.6f), "Pendulum hangs below the dial");
            var tick = clock.GetComponentInParent<ClockAmbience>() ?? clock.GetComponentInChildren<ClockAmbience>(true);
            Assert.That(tick && tick.pendulum && tick.source, Is.True, "Ticking ambience is wired");
        }

        [Test]
        public void AnnexSouthWallSealedAndCounterRestored()
        {
            var d = Open();
            foreach (string n in new[] { "AnnexSouthWall_W", "AnnexSouthWall_E", "AnnexBackDoor", "AnnexBackDoorJamb_W", "AnnexBackDoorJamb_E", "AnnexBackDoorHeader", "KitchenCounter" })
            {
                var go = GameObject.Find(n);
                Assert.That(go && go.activeInHierarchy, Is.True, n + " exists");
                Assert.That(go.GetComponentsInChildren<Renderer>(true).Any(r => r && r.enabled), Is.True, n + " renders");
            }
            var leaf = GameObject.Find("AnnexBackDoor");
            var lb = VisualBounds(leaf);
            var lx0 = d.LocalPosition(new Vector3(lb.min.x, 0, 0)).x;
            var lx1 = d.LocalPosition(new Vector3(lb.max.x, 0, 0)).x;
            Assert.That(Mathf.Min(lx0, lx1), Is.AtMost(3.5f + .15f), "Back door covers the west gap");
            Assert.That(Mathf.Max(lx0, lx1), Is.AtLeast(4.5f - .15f), "Back door covers the east gap");
            int samples = 0, blocked = 0;
            for (float x = 2f; x <= 6f; x += .5f)
                for (float y = .6f; y <= 2.8f; y += .7f)
                {
                    samples++;
                    if (Physics.Linecast(d.P(x, 11.4f, y), d.P(x, 13.4f, y), out _, ~0, QueryTriggerInteraction.Ignore)) blocked++;
                }
            Assert.That(blocked, Is.EqualTo(samples), "South kitchen wall is fully sealed");
        }

        [Test]
        public void StairDoorwayWallSeal()
        {
            var d = Open();
            foreach (string n in new[] { "StairEnclosureFrontWest", "StairEnclosureFrontEast", "StairEnclosureDoorHeader", "LockedCentralStairDoor" })
                Assert.That(GameObject.Find(n), Is.Not.Null, n + " seals the stair front");
            var leaf = GameObject.Find("LockedCentralStairDoor");
            var lb = VisualBounds(leaf);
            var lx0 = d.LocalPosition(new Vector3(lb.min.x, 0, 0)).x;
            var lx1 = d.LocalPosition(new Vector3(lb.max.x, 0, 0)).x;
            float lo = Mathf.Min(lx0, lx1), hi = Mathf.Max(lx0, lx1);
            Assert.That(lo, Is.AtMost(-4.175f + .12f), "Leaf covers the west jamb");
            Assert.That(hi, Is.AtLeast(-2.925f - .12f), "Leaf covers the east jamb");
            Assert.That(lb.min.y, Is.AtMost(.6f), "Leaf reaches the floor");
            int x0 = 0;
            for (float x = -4.6f; x <= -2.4f; x += .35f)
                for (float y = .6f; y <= 2.8f; y += .7f)
                {
                    bool blocked = Physics.Linecast(d.P(x, 5.6f, y), d.P(x, 7.0f, y), out _, ~0, QueryTriggerInteraction.Ignore);
                    Assert.That(blocked, Is.True, "Stair front sealed at x=" + x.ToString("F2") + " y=" + y.ToString("F1"));
                    x0++;
                }
            Assert.That(x0, Is.GreaterThan(20), "Seal grid actually sampled");
        }

        [Test]
        public void GrandfatherClockIsCC0AndVisible()
        {
            var d = Open();
            var clock = GameObject.Find("GrandfatherClock");
            Assert.That(clock && clock.activeInHierarchy, Is.True, "CC0 clock exists and is active");
            var cb = VisualBounds(clock);
            Assert.That(cb.size.y, Is.InRange(1.9f, 2.4f), "Clock reads as a full tall case, not a buried blob");
            Assert.That(cb.min.y, Is.LessThan(.7f), "Clock case reaches the floor");
            var mats = clock.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled)
                .SelectMany(r => r.sharedMaterials).Where(m => m).Distinct().ToArray();
            Assert.That(mats.Any(m => m.HasProperty("_BaseMap") && m.GetTexture("_BaseMap") && m.GetTexture("_BaseMap").name.Contains("vintage_grandfather")), Is.True, "Clock wears its CC0 diffuse, not flat grey");
            Assert.That(mats.Any(m => m && m.name.Contains("ClockGlass") && m.renderQueue == 3000), Is.True, "Door glass is a faint sheet so the dial stays readable");
            var hour = clock.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t && t.name.StartsWith("ClockHourBlade__"));
            var minute = clock.GetComponentsInChildren<Transform>(true).FirstOrDefault(t => t && t.name.StartsWith("ClockMinuteBlade__"));
            Assert.That(hour && minute, Is.True, "CC0 hand pivots are rebound");
            Assert.That(Vector3.Distance(hour.position, d.clockDialCentre), Is.LessThan(.15f), "Hour hand sits on the dial");
            var pend = GameObject.Find("ClockPendulum__");
            Assert.That(pend, Is.Not.Null, "Pendulum hangs inside the CC0 case");
            var pb = VisualBounds(pend);
            Assert.That(pb.size.y, Is.GreaterThan(.5f), "Pendulum rod+ball read as one swinging piece");
            Assert.That(d.clockHour, Is.SameAs(hour), "Director binds the CC0 hour pivot");
        }

        [Test]
        public void VentCoverIsRustedAndFitsItsDuct()
        {
            Open();
            var cover = GameObject.Find("VentGrilleDoor");
            Assert.That(cover && cover.activeInHierarchy, Is.True, "Vent grille leaf exists");
            var cb = VisualBounds(cover);
            Assert.That(cb.size.x, Is.EqualTo(.76f).Within(.05f), "Grille width matches the duct opening");
            Assert.That(cb.size.y, Is.EqualTo(.56f).Within(.05f), "Grille height matches the duct opening");
            var hinge = GameObject.Find("VentGrilleHinge");
            Assert.That(hinge, Is.Not.Null, "Grille hinge exists");
            Assert.That(Mathf.Abs(hinge.transform.position.y - cb.center.y), Is.LessThan(.12f), "Hinge sits at grille height, not the floor");
            var mats = cover.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled)
                .SelectMany(r => r.sharedMaterials).Where(m => m).Distinct().ToArray();
            Assert.That(mats.Length, Is.GreaterThan(0), "Grille has real materials");
            Assert.That(mats.Any(m => m.HasProperty("_BaseMap") && m.GetTexture("_BaseMap") && m.GetTexture("_BaseMap").name.Contains("metal_grate_rusty")), Is.True, "Grille wears the CC0 rust, not brand-new grey");
            foreach (string n in new[] { "VentDuctTrim_Top", "VentDuctTrim_Bottom", "VentDuctTrim_Left", "VentDuctTrim_Right", "VentBackDark_Top", "VentBackDark_Bottom" })
                Assert.That(GameObject.Find(n), Is.Not.Null, n + " seals the duct frame");
            var door = cover.GetComponent<LayarTerakhir.Prolog.HingedDoor>();
            Assert.That(door, Is.Not.Null, "Grille swings on a real hinge");
            Assert.That(door.locked, Is.True, "Grille starts locked until the crowbar is found");
        }

        [Test]
        public void CrowbarWaitsInTheStorageNook()
        {
            var d = Open();
            var crow = GameObject.Find("StorageCrowbar");
            Assert.That(crow && crow.activeInHierarchy, Is.True, "CC0 crowbar waits in storage");
            var cb = VisualBounds(crow);
            float longHoriz = Mathf.Max(cb.size.x, cb.size.z);
            Assert.That(longHoriz, Is.InRange(.4f, .8f), "Crowbar lies flat as a full pry bar, not a speck");
            Assert.That(cb.min.y, Is.EqualTo(.5f).Within(.12f), "Crowbar rests on the nook floor");
            var mats = crow.GetComponentsInChildren<Renderer>(true).Where(r => r && r.enabled)
                .SelectMany(r => r.sharedMaterials).Where(m => m).Distinct().ToArray();
            Assert.That(mats.Any(m => m.HasProperty("_BaseMap") && m.GetTexture("_BaseMap") && m.GetTexture("_BaseMap").name.Contains("crowbar")), Is.True, "Crowbar wears its CC0 diffuse");
            var item = crow.GetComponent<LayarTerakhir.Prolog.AtmajaItem>();
            Assert.That(item, Is.Not.Null, "Crowbar is actually pickable");
            Assert.That(item.action, Is.EqualTo(LayarTerakhir.Prolog.AtmajaAction.Crowbar), "Crowbar pickup is wired");
            Assert.That(d.crowbar, Is.SameAs(crow), "Director binds the crowbar");
        }
    }
}
#endif
