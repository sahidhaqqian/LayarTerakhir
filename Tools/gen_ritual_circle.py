#!/usr/bin/env python3
"""Paint the dried-blood ritual circle for the R2 crypt: outer ring, inner
ring, five-pointed star, drips and speckles on transparency.

Deterministic (fixed seed). Output:
  Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47/RitualCircleBlood.png
  1024x1024, maps onto a 2.4m floor quad.
"""
import math
import random
from pathlib import Path

from PIL import Image, ImageDraw

S = 1024
SEED = 19661231
OUT = (
    Path(__file__).resolve().parent.parent
    / "Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47/RitualCircleBlood.png"
)

C, R = S / 2, S * 0.46
DARK = (46, 6, 5)
BODY = (104, 14, 10)
EDGE = (138, 24, 14)


def main():
    rng = random.Random(SEED)
    img = Image.new("RGBA", (S, S), (0, 0, 0, 0))
    draw = ImageDraw.Draw(img)

    def ring(radius, width, alpha=235):
        for w in range(width):
            rr = radius - width / 2 + w
            jitter = rng.uniform(-1.2, 1.2)
            draw.ellipse(
                [C - rr + jitter, C - rr, C + rr + jitter, C + rr],
                outline=BODY + (alpha,),
                width=2,
            )
        # dark dried core thread
        draw.ellipse(
            [C - radius, C - radius, C + radius, C + radius],
            outline=DARK + (alpha,),
            width=max(2, width // 3),
        )

    ring(R, 16)
    ring(R * 0.68, 7)

    # five-pointed star, one point up
    pts = []
    for i in range(5):
        a = -math.pi / 2 + i * 2 * math.pi / 5
        pts.append((C + R * 0.92 * math.cos(a), C + R * 0.92 * math.sin(a)))
    order = [0, 2, 4, 1, 3, 0]
    for s in range(5):
        p0, p1 = pts[order[s]], pts[order[s + 1]]
        # ragged stroke: short overlapping segments with jitter
        n = 40
        for i in range(n):
            t0, t1 = i / n, (i + 1) / n
            seg = [
                p0[0] + (p1[0] - p0[0]) * t0 + rng.uniform(-2, 2),
                p0[1] + (p1[1] - p0[1]) * t0 + rng.uniform(-2, 2),
                p0[0] + (p1[0] - p0[0]) * t1 + rng.uniform(-2, 2),
                p0[1] + (p1[1] - p0[1]) * t1 + rng.uniform(-2, 2),
            ]
            draw.line(seg, fill=BODY + (235,), width=rng.randint(7, 11))
        draw.ellipse(
            [p0[0] - 9, p0[1] - 9, p0[0] + 9, p0[1] + 9], fill=DARK + (240,)
        )

    # drips running outward from the ring
    for _ in range(26):
        a = rng.uniform(0, 2 * math.pi)
        ln = rng.uniform(20, 130)
        x0, y0 = C + R * math.cos(a), C + R * math.sin(a)
        x1, y1 = C + (R + ln) * math.cos(a), C + (R + ln) * math.sin(a)
        w = rng.uniform(3, 7)
        draw.line([x0, y0, x1, y1], fill=BODY + (225,), width=int(w))
        draw.ellipse([x1 - w, y1 - w, x1 + w, y1 + w], fill=DARK + (230,))

    # speckles
    for _ in range(240):
        a = rng.uniform(0, 2 * math.pi)
        rr = rng.uniform(R * 0.2, R * 1.08)
        x, y = C + rr * math.cos(a), C + rr * math.sin(a)
        r = rng.uniform(1, 4)
        col = rng.choice([BODY, DARK, EDGE])
        draw.ellipse([x - r, y - r, x + r, y + r], fill=col + (rng.randint(150, 235),))

    OUT.parent.mkdir(parents=True, exist_ok=True)
    img.save(OUT)
    print("wrote", OUT, img.size)


if __name__ == "__main__":
    main()
