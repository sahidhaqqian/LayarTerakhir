#if UNITY_EDITOR
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    // Designer Nov: the blink lives ON the item itself (a faint brightness
    // pulse), never a floating star above it. Pins the marker itself.
    public sealed class PickupBlinkTests
    {
        [Test]
        public void MustTakeItemPulsesOnItsOwnBody()
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
                Assert.That(first.transform == item.transform, "Blink sits on the item itself, not as a floating child");
                var second = PickupBlink.EnsureFor(item, "PROBE_FLAG");
                Assert.That(second, Is.SameAs(first), "Re-ensure returns the same blink, never a duplicate");
                // No floating star rigs above the item.
                foreach (var r in item.GetComponentsInChildren<MeshRenderer>(true))
                    Assert.That(r.gameObject.name, Is.Not.EqualTo("PickupBlink"), "No floating star above the item");
                // Peak of the pulse brightens the body's own material.
                first.ApplyPulse(1f);
                var block = new MaterialPropertyBlock();
                item.GetComponent<Renderer>().GetPropertyBlock(block);
                var mat = item.GetComponent<Renderer>().sharedMaterial;
                string prop = mat.HasProperty("_BaseColor") ? "_BaseColor" : "_Color";
                var pulsed = block.GetColor(prop);
                var rest = mat.GetColor(prop);
                Assert.That(pulsed.grayscale, Is.GreaterThan(rest.grayscale), "Peak pulse brightens the item body");
                first.ClearPulse();
                // Story gate: hidden while the thing cannot be taken yet.
                first.availableWhen = () => false;
                Assert.That(first.ShouldShow(), Is.False, "Blink stays off until the item can be taken");
                first.availableWhen = () => true;
                Assert.That(first.ShouldShow(), Is.True, "Blink lights once the item can be taken");
            }
            finally { Object.DestroyImmediate(item); }
        }
    }
}
#endif
