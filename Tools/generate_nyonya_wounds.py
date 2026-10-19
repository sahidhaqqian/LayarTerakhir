"""Generate the Nyonya's bloody puncture overlay used by her cloth and skin.

The texture multiplies the existing material colours, so holes stay black while
the surrounding cloth keeps its ivory/batik tone.
"""
import math
import random
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter

ROOT = Path(__file__).resolve().parents[1]
TARGET = ROOT / "Assets/Resources/PlayerPresentation/NyonyaWound.png"
SIZE = 512
random.seed(1998)


def jagged_hole(draw, cx, cy, radius, colour):
    points = []
    for index in range(18):
        angle = index / 18 * 6.28318
        wobble = radius * random.uniform(0.62, 1.18)
        points.append((cx + math.cos(angle) * wobble,
                       cy + math.sin(angle) * wobble))
    draw.polygon(points, fill=colour)


def main():
    image = Image.new("RGB", (SIZE, SIZE), (248, 245, 240))
    draw = ImageDraw.Draw(image)
    rng = random.Random(4242)

    for _ in range(150):
        x, y = rng.randrange(SIZE), rng.randrange(SIZE)
        shade = rng.randrange(210, 250)
        draw.point((x, y), fill=(shade, shade - 3, shade - 8))

    holes = []
    for _ in range(34):
        x = rng.randrange(24, SIZE - 24)
        y = rng.randrange(18, SIZE - 18)
        radius = rng.randrange(8, 30)
        holes.append((x, y, radius))
        # Bruised rim, then the black puncture and a wet inner highlight.
        jagged_hole(draw, x, y, radius * 1.55, (112, 20, 16))
        jagged_hole(draw, x, y, radius * 1.12, (162, 34, 26))
        jagged_hole(draw, x, y, radius * .72, (52, 4, 4))
        jagged_hole(draw, x, y, radius * .38, (4, 0, 0))
        draw.ellipse((x - radius * .18, y - radius * .24,
                      x + radius * .16, y + radius * .10), fill=(92, 12, 10))

        # Drips run down the body from the lower half of each wound.
        drip_x = x + rng.randrange(-radius // 3, radius // 3 + 1)
        length = rng.randrange(radius * 2, radius * 8)
        width = max(2, rng.randrange(radius // 4, radius // 2 + 2))
        for offset in range(0, length, 4):
            taper = 1 - offset / max(1, length)
            colour = (int(120 + 30 * taper), int(12 + 10 * taper), int(8 + 6 * taper))
            draw.line((drip_x, y + radius // 3 + offset,
                       drip_x + rng.choice((-1, 0, 1)), y + radius // 3 + offset + 3),
                      fill=colour, width=max(1, int(width * taper)))
        for _ in range(rng.randrange(2, 7)):
            sx = x + rng.randrange(-radius * 3, radius * 3)
            sy = y + rng.randrange(-radius * 2, radius * 3)
            sr = rng.randrange(1, 4)
            draw.ellipse((sx - sr, sy - sr, sx + sr, sy + sr), fill=(128, 14, 10))

    image = image.filter(ImageFilter.GaussianBlur(.35))
    TARGET.parent.mkdir(parents=True, exist_ok=True)
    image.save(TARGET)
    print(f"Wrote {TARGET.relative_to(ROOT)} with {len(holes)} wounds")


if __name__ == "__main__":
    main()
