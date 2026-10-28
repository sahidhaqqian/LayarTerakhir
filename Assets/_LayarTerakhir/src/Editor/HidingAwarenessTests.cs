#if UNITY_EDITOR
using LayarTerakhir.Core;
using NUnit.Framework;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class HidingAwarenessTests
    {
        [Test]public void UrgentPinsBarFullRegardlessOfDistance()
        {
            Assert.That(HidingAwareness.ProximityFill(true, 99f, 2.5f, 10f), Is.EqualTo(1f));
            Assert.That(HidingAwareness.ProximityFill(true, 0f, 2.5f, 10f), Is.EqualTo(1f));
        }
        [Test]public void ContactRangeReadsFull()
        {
            Assert.That(HidingAwareness.ProximityFill(false, 0f, 2.5f, 10f), Is.EqualTo(1f));
            Assert.That(HidingAwareness.ProximityFill(false, 2.5f, 2.5f, 10f), Is.EqualTo(1f));
        }
        [Test]public void GoneRangeReadsEmpty()
        {
            Assert.That(HidingAwareness.ProximityFill(false, 10f, 2.5f, 10f), Is.EqualTo(0f));
            Assert.That(HidingAwareness.ProximityFill(false, 99f, 2.5f, 10f), Is.EqualTo(0f));
        }
        [Test]public void MidRangeDrainsLinearly()
        {
            Assert.That(HidingAwareness.ProximityFill(false, 6.25f, 2.5f, 10f), Is.EqualTo(.5f).Within(.001f));
            float near = HidingAwareness.ProximityFill(false, 4f, 2.5f, 10f);
            float far = HidingAwareness.ProximityFill(false, 8f, 2.5f, 10f);
            Assert.That(near, Is.GreaterThan(far));
            Assert.That(near, Is.GreaterThan(0f).And.LessThan(1f));
        }
        [Test]public void ThreatGainLoudNearSilentFar()
        {
            Assert.That(HidingAwareness.ThreatGain(0f, 12f, .5f), Is.EqualTo(.5f));
            Assert.That(HidingAwareness.ThreatGain(6f, 12f, .5f), Is.EqualTo(.25f).Within(.001f));
            Assert.That(HidingAwareness.ThreatGain(12f, 12f, .5f), Is.EqualTo(0f));
            Assert.That(HidingAwareness.ThreatGain(99f, 12f, .5f), Is.EqualTo(0f));
        }
    }
}
#endif
