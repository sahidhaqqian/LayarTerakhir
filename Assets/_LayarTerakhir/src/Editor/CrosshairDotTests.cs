#if UNITY_EDITOR
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEngine;

namespace LayarTerakhir.Editor.Tests
{
    // Designer Nov: the center dot must be ROUND, 8px — never the default
    // square Image. Pins the generated sprite shape directly.
    public sealed class CrosshairDotTests
    {
        [Test]
        public void CenterDotSpriteIsRound()
        {
            var sprite = HudController.CircleSprite();
            Assert.That(sprite, Is.Not.Null, "Round dot sprite exists");
            var tex = sprite.texture;
            // Corners of a round dot are fully transparent...
            Assert.That(tex.GetPixel(0, 0).a, Is.LessThan(.05f), "Sprite corner is transparent (round, not square)");
            Assert.That(tex.GetPixel(tex.width - 1, 0).a, Is.LessThan(.05f), "Sprite corner is transparent (round, not square)");
            Assert.That(tex.GetPixel(0, tex.height - 1).a, Is.LessThan(.05f), "Sprite corner is transparent (round, not square)");
            // ...while the middle is solid.
            Assert.That(tex.GetPixel(tex.width / 2, tex.height / 2).a, Is.GreaterThan(.95f), "Sprite center is solid");
            // Edge midpoint is solid too — no diamond, no cross.
            Assert.That(tex.GetPixel(tex.width / 2, 0).a, Is.GreaterThan(.5f), "Sprite edge midpoint is covered");
            Assert.That(tex.GetPixel(0, tex.height / 2).a, Is.GreaterThan(.5f), "Sprite edge midpoint is covered");
        }
    }
}
#endif
