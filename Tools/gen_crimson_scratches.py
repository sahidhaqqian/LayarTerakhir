#!/usr/bin/env python3
"""Retexture the designer's Meshy crimson-rupture relief toward the reference
(cakaran_yang_aku_inginkan.jpg): dense fan clusters of long tapered bloody
claw slashes with drips over the bathroom's own dirty tile, glossy wet
slashes on a rough wall.

The Meshy GLB supplies ONLY the relief mesh (+ its normal map, saved
alongside); albedo/roughness are re-authored here so the panel reads as
clawed bathroom tile instead of a flat maroon slab.

Deterministic (fixed seed). Outputs (next to the GLB):
  Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture/CrimsonRuptureWall_albedo.png
  Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture/CrimsonRuptureWall_mask.png
    (R = metallic 0 everywhere, A = smoothness: rough tile ~0.2, wet slashes ~0.75)
  (normal map = meshy normal extracted from the GLB, saved alongside as
  CrimsonRuptureWall_normal.jpg)

Canvas 2048x1312 matches the relief UV aspect (~1.9m x 1.22m).
"""
import math
import random
from pathlib import Path

from PIL import Image, ImageDraw

W, H = 2048, 1312
SEED = 19980521
ROOT = Path(__file__).resolve().parent.parent
RDIR = ROOT / "Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture"
TILE = ROOT / "Assets/_LayarTerakhir/assets/Art/Atmaja/Textures/dirty_tiles_diff_1k.jpg"
ALBEDO_OUT = RDIR / "CrimsonRuptureWall_albedo.png"
MR_OUT = RDIR / "CrimsonRuptureWall_mask.png"

CORE = (22, 3, 3)
BODY = (88, 10, 9)
EDGE = (128, 18, 13)
HOT = (190, 46, 28)


def bezier(p0, p1, p2, t):
    u = 1 - t
    return (
        u * u * p0[0] + 2 * u * t * p1[0] + t * t * p2[0],
        u * u * p0[1] + 2 * u * t * p1[1] + t * t * p2[1],
    )


def streak(layer, mask, p0, p1, p2, w0, rng, alpha=255, hot_thread=True):
    import math as _m

    approx_len = _m.hypot(p2[0] - p0[0], p2[1] - p0[1])
    n = max(48, int(approx_len / 2.5))
    pts = [bezier(p0, p1, p2, i / (n - 1)) for i in range(n)]

    def width_at(t):
        return max(1.4, w0 * (1 - t) ** 0.8 + rng.uniform(-0.5, 0.5))

    for i in range(0, n, 3):
        t = i / (n - 1)
        r = width_at(t) * 1.1 + 4
        x, y = pts[i]
        layer.ellipse([x - r, y - r, x + r, y + r], fill=(20, 2, 2, int(alpha * 0.5)))
        mask.ellipse([x - r, y - r, x + r, y + r], fill=110)
    for i in range(n):
        t = i / (n - 1)
        r = width_at(t)
        x, y = pts[i]
        col = BODY if t < 0.75 else EDGE
        layer.ellipse([x - r, y - r, x + r, y + r], fill=col + (alpha,))
        mask.ellipse([x - r, y - r, x + r, y + r], fill=255)
    for i in range(int(n * 0.8)):
        t = i / (n - 1)
        r = max(1.0, width_at(t) * 0.45)
        x, y = pts[i]
        layer.ellipse([x - r, y - r, x + r, y + r], fill=CORE + (alpha,))
    if hot_thread:
        for i in range(6, n - 8):
            x, y = pts[i]
            layer.ellipse(
                [x - 1.1, y - 2.2, x + 1.1, y + 0.4],
                fill=HOT + (int(alpha * 0.35),),
            )


def drip(layer, mask, x, y, length, w, rng, alpha=235):
    n = max(6, int(length / 9))
    cx = x
    for i in range(n):
        t = i / (n - 1)
        cx += rng.uniform(-2.2, 2.2)
        cy = y + t * length
        r = w * (1 - t * 0.55)
        layer.ellipse([cx - r, cy - r, cx + r, cy + r], fill=BODY + (alpha,))
        mask.ellipse([cx - r, cy - r, cx + r, cy + r], fill=220)
    layer.ellipse(
        [cx - w - 1, y + length - w - 1, cx + w + 1, y + length + w + 1],
        fill=CORE + (alpha,),
    )


def fan(layer, mask, cx, cy, angle_deg, slashes, length, spread_deg, rng):
    base = math.radians(angle_deg)
    for _ in range(slashes):
        off = math.radians(rng.uniform(-spread_deg, spread_deg) / 2)
        a = base + off
        ln = length * rng.uniform(0.72, 1.12)
        dx, dy = math.cos(a), math.sin(a)
        bow = rng.uniform(-0.16, 0.16)
        px, py = -dy, dx
        p0 = (cx + rng.uniform(-14, 14), cy + rng.uniform(-14, 14))
        p2 = (p0[0] + dx * ln, p0[1] + dy * ln)
        p1 = (
            (p0[0] + p2[0]) / 2 + px * ln * bow,
            (p0[1] + p2[1]) / 2 + py * ln * bow,
        )
        w0 = rng.uniform(9, 20)
        streak(layer, mask, p0, p1, p2, w0, rng, alpha=rng.randint(225, 255))
        if rng.random() < 0.45:
            drip(
                layer,
                mask,
                p2[0] + rng.uniform(-6, 6),
                p2[1],
                rng.uniform(50, 190),
                rng.uniform(3, 6.5),
                rng,
            )


def main():
    rng = random.Random(SEED)
    # Base: the bathroom's own dirty tile so the relief blends into the wall.
    tile = Image.open(TILE).convert("RGB")
    base = Image.new("RGB", (W, H))
    for ty in range(0, H, 1024):
        for tx in range(0, W, 1024):
            base.paste(tile, (tx, ty))
    base = base.crop((0, 0, W, H))

    slashes = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    smask = Image.new("L", (W, H), 0)
    dl, dm = ImageDraw.Draw(slashes), ImageDraw.Draw(smask)

    # old-blood wash so clusters sit in stained tile
    for _ in range(34):
        x, y = rng.uniform(0, W), rng.uniform(0, H)
        rx, ry = rng.uniform(120, 320), rng.uniform(80, 200)
        dl.ellipse(
            [x - rx, y - ry, x + rx, y + ry], fill=(58, 7, 6, rng.randint(16, 40))
        )

    clusters = [
        (260, 400, -30, 10, 560, 24),
        (560, 860, -56, 9, 520, 22),
        (820, 360, 70, 10, 600, 26),
        (1080, 800, 94, 9, 560, 24),
        (1330, 420, -62, 10, 590, 26),
        (1580, 800, 40, 9, 540, 28),
        (1830, 380, -76, 9, 500, 24),
        (1960, 900, 64, 8, 460, 26),
        (120, 900, -48, 8, 480, 24),
    ]
    for cx, cy, ang, n, ln, sp in clusters:
        fan(dl, dm, cx, cy, ang, n, ln, sp, rng)
    for _ in range(24):
        x, y = rng.uniform(80, W - 80), rng.uniform(80, H - 80)
        a = math.radians(rng.uniform(0, 360))
        ln = rng.uniform(180, 420)
        dx, dy = math.cos(a), math.sin(a)
        bow = rng.uniform(-0.2, 0.2)
        p0 = (x, y)
        p2 = (x + dx * ln, y + dy * ln)
        p1 = ((p0[0] + p2[0]) / 2 - dy * ln * bow, (p0[1] + p2[1]) / 2 + dx * ln * bow)
        streak(dl, dm, p0, p1, p2, rng.uniform(6, 13), rng, alpha=rng.randint(200, 240))

    albedo = Image.alpha_composite(base.convert("RGBA"), slashes).convert("RGB")

    # Unity URP Lit _MetallicGlossMap: R = metallic (0 everywhere — blood
    # and tile are dielectric), A = smoothness (dull tile ~0.2, wet
    # glossy slashes ~0.75).
    smooth = Image.new("L", (W, H), 51)
    gloss = smask.point(lambda v: 51 + int(v / 255 * 140) if v else 51)
    from PIL import ImageChops

    smooth = ImageChops.lighter(smooth, gloss)
    mr = Image.new("RGBA", (W, H), (0, 0, 0, 255))
    mr.putalpha(smooth)
    # R stays 0 (metallic 0); RGB unused by the mask.

    RDIR.mkdir(parents=True, exist_ok=True)
    albedo.save(ALBEDO_OUT)
    mr.save(MR_OUT)
    print("wrote", ALBEDO_OUT, albedo.size)
    print("wrote", MR_OUT, mr.size)


if __name__ == "__main__":
    main()
