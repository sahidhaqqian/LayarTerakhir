#if UNITY_EDITOR
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    // Designer Nov: every must-take item carries a small gold blink so
    // players notice it. Pins the marker itself (created, above the item,
    // idempotent) — visibility follows the item at runtime.
    public sealed class PickupBlinkTests
    {
        [Test]
        public void MustTakeItemCarriesABlinkAboveIt()
        {
            var item = GameObject.CreatePrimitive(PrimitiveType.Cube);
            item.name = "BlinkProbeItem";
            item.transform.position = new Vector3(0, .5f, 0);
            item.transform.localScale = new Vector3(.2f, .1f, .2f);
            try
            {
                var kos = item.AddComponent<KosInteractableItem>();
                kos.isInteractable = true;
                var first = PickupBlink.EnsureFor(item, "PROBE_FLAG");
                Assert.That(first, Is.Not.Null, "Blink is created for a must-take item");
                var top = item.GetComponent<Collider>().bounds.max.y;
                Assert.That(first.transform.position.y, Is.GreaterThan(top), "Blink floats above the item, not inside it");
                Assert.That(first.transform.position.y - top, Is.LessThan(.5f), "Blink hugs the item — a small hint, not a beacon");
                var second = PickupBlink.EnsureFor(item, "PROBE_FLAG");
                Assert.That(second, Is.SameAs(first), "Re-ensure returns the same blink, never a duplicate");
                Assert.That(item.GetComponentsInChildren<PickupBlink>(true).Length, Is.EqualTo(1), "Exactly one blink per item");
                var glow = first.GetComponent<MeshRenderer>();
                Assert.That(glow && glow.sharedMaterial, Is.Not.Null, "Blink has a real material, not a missing pink stub");
            }
            finally { Object.DestroyImmediate(item); }
        }
    }
}
#endif
