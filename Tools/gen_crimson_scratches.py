#!/usr/bin/env python3
"""Retexture the Meshy crimson-rupture relief toward the designer reference
(cakaran_yang_aku_inginkan.jpg) 1:1.

Goal (from R2 asset/cakaran_yang_aku_inginkan.jpg):
- two-tone wall: dirty off-white plaster above, brown wood planks below
- dense overlapping fans of LONG tapered claw gouges, dark dried-blood
  maroon-black with wet gloss only on the thickest pools
- each gouge: ragged wide dug-in start (blood pool), multiple fine inner
  striations, sharp tapered point at the end — NEVER a uniform worm tube
  with a white dotted centre line
- vertical gravity drips + splatter

v3: polygon-tapered gouges (no ellipse-tube beading), subtle plaster,
broken specular dots only.
Deterministic (fixed seed).
"""
import math
import random
from pathlib import Path

from PIL import Image, ImageDraw, ImageFilter

W, H = 2048, 1312
SEED = 19980521
ROOT = Path(__file__).resolve().parent.parent
RDIR = ROOT / "Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture"
ALBEDO_OUT = RDIR / "CrimsonRuptureWall_albedo.png"
MR_OUT = RDIR / "CrimsonRuptureWall_mask.png"

SPLIT = int(H * 0.20)

DRIED = (105, 16, 12)      # outer dried smear
BODY = (172, 28, 20)     # gouge body
DARK = (70, 12, 9)       # deep centre
STRIA = (72, 12, 9)      # inner fine lines
SPEC = (255, 225, 200)  # sparse wet glints


def bezier(p0, p1, p2, t):
    u = 1 - t
    return (
        u * u * p0[0] + 2 * u * t * p1[0] + t * t * p2[0],
        u * u * p0[1] + 2 * u * t * p1[1] + t * t * p2[1],
    )


def build_base(rng):
    # Match the REAL bathroom wall behind the panel (see scratch-proof shots):
    # dirty plaster strip on top blending into the plaster above, dark
    # red-brown tiles below. No wood, no brick patch: those belong to the
    # goal photo's room, not this wall; a mismatched bg reads as a floating
    # rectangle instead of clawed wall.
    base = Image.new("RGBA", (W, H), (168, 158, 142, 255))
    over = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    dl = ImageDraw.Draw(over)
    for _ in range(200):
        x, y = rng.uniform(0, W), rng.uniform(0, SPLIT)
        rx, ry = rng.uniform(14, 70), rng.uniform(8, 36)
        tone = rng.choice([(150, 141, 126), (188, 180, 164), (130, 120, 106), (160, 148, 132)])
        dl.ellipse([x - rx, y - ry, x + rx, y + ry], fill=tone + (14,))
    over = over.filter(ImageFilter.GaussianBlur(20))
    base = Image.alpha_composite(base, over)
    dl = ImageDraw.Draw(base, "RGBA")
    for _ in range(30):
        x, y = rng.uniform(0, W), rng.uniform(0, SPLIT)
        dl.line([x, y, x + rng.uniform(-80, 80), y + rng.uniform(-30, 30)],
                fill=(110, 102, 88, 255), width=1)
    # dark red-brown tiles like the wall (8 cols x 7 rows + grout)
    cols, rows = 8, 7
    tw, th = W / cols, (H - SPLIT) / rows
    for r in range(rows):
        for c in range(cols):
            x0, y0 = c * tw, SPLIT + r * th
            shade = (112 + rng.randint(-10, 10), 42 + rng.randint(-7, 7), 34 + rng.randint(-6, 6))
            dl.rectangle([x0 + 2, y0 + 2, x0 + tw - 2, y0 + th - 2], fill=shade + (255,))
            for _ in range(8):
                x, y = rng.uniform(x0, x0 + tw), rng.uniform(y0, y0 + th)
                dl.line([x, y, x + rng.uniform(20, 90), y + rng.uniform(-2, 2)],
                        fill=(70, 24, 20, 255), width=1)
            dl.rectangle([x0 + 2, y0 + 2, x0 + tw - 2, y0 + 2 + 8], fill=(112, 44, 36, 90))
    for r in range(rows + 1):
        dl.line([0, SPLIT + r * th, W, SPLIT + r * th], fill=(46, 16, 14, 255), width=4)
    for c in range(cols + 1):
        dl.line([c * tw, SPLIT, c * tw, H], fill=(46, 16, 14, 255), width=4)
    dl.rectangle([0, SPLIT - 5, W, SPLIT + 3], fill=(70, 60, 50, 255))
    # overall grime
    grime = Image.new("L", (W, H), 0)
    gd = ImageDraw.Draw(grime)
    for _ in range(100):
        x, y = rng.uniform(0, W), rng.uniform(0, H)
        rx, ry = rng.uniform(60, 280), rng.uniform(40, 160)
        gd.ellipse([x - rx, y - ry, x + rx, y + ry], fill=rng.randint(10, 34))
    grime = grime.filter(ImageFilter.GaussianBlur(40))
    dark = Image.new("RGBA", (W, H), (38, 28, 22, 255))
    base = Image.composite(dark, base, grime)
    return base


def tapered_poly(pts, widths):
    """Left edge forward + right edge backward + sharp tip."""
    n = len(pts)
    left, right = [], []
    for i in range(n):
        x, y = pts[i]
        x2, y2 = pts[min(i + 1, n - 1)]
        x1, y1 = pts[max(i - 1, 0)]
        dx, dy = x2 - x1, y2 - y1
        L = math.hypot(dx, dy) or 1.0
        nx, ny = -dy / L, dx / L
        w = widths[i]
        left.append((x + nx * w, y + ny * w))
        right.append((x - nx * w, y - ny * w))
    # sharp tip: extend last point along direction
    x2, y2 = pts[-1]
    x1, y1 = pts[-2]
    dx, dy = x2 - x1, y2 - y1
    L = math.hypot(dx, dy) or 1.0
    tip = (x2 + dx / L * 10, y2 + dy / L * 10)
    return left + [tip] + list(reversed(right))


def pool(layer, mask, cx, cy, rng, rmax=26):
    md = ImageDraw.Draw(mask)
    # irregular dug-in blob: dark mass + torn lobes, NO clean ellipse ring
    for _ in range(10):
        x, y = cx + rng.uniform(-26, 26), cy + rng.uniform(-20, 20)
        rx, ry = rng.uniform(14, 42), rng.uniform(10, 30)
        rot = rng.uniform(0, 180)
        layer.ellipse([x - rx, y - ry, x + rx, y + ry], fill=DRIED + (235,))
        md.ellipse([x - rx, y - ry, x + rx, y + ry], fill=110)
    for _ in range(7):
        x, y = cx + rng.uniform(-16, 16), cy + rng.uniform(-14, 14)
        r = rng.uniform(9, 24)
        layer.ellipse([x - r, y - r, x + r, y + r], fill=BODY + (255,))
        md.ellipse([x - r, y - r, x + r, y + r], fill=130)
    for _ in range(5):
        x, y = cx + rng.uniform(-12, 12), cy + rng.uniform(-10, 10)
        r = rng.uniform(5, 13)
        layer.ellipse([x - r, y - r, x + r, y + r], fill=DARK + (255,))


def streak(layer, mask, p0, p1, p2, w0, rng, alpha=255):
    approx = math.hypot(p2[0] - p0[0], p2[1] - p0[1])
    n = max(48, int(approx / 3.0))
    pts = [bezier(p0, p1, p2, i / (n - 1)) for i in range(n)]
    # ragged taper: wide at dig-in, sharp point at end — smoothed so edges
    # tear (goal) instead of sawtooth jitter
    raw = [rng.uniform(0.90, 1.10) for _ in range(n)]
    sm = []
    for i in range(n):
        s = 0.0
        c = 0
        for k in range(-2, 3):
            j = min(max(i + k, 0), n - 1)
            s += raw[j]
            c += 1
        sm.append(s / c)
    widths = []
    for i in range(n):
        t = i / (n - 1)
        base_w = w0 * (1 - t) ** 0.62 + 0.4
        widths.append(max(0.4, base_w * sm[i]))
    widths[-1] = 0.4
    widths[-2] = max(0.6, widths[-2] * 0.4)

    layer.polygon(tapered_poly(pts, [w * 1.30 + 2.0 for w in widths]), fill=DRIED + (200,))
    layer.polygon(tapered_poly(pts, widths), fill=BODY + (alpha,))
    m = ImageDraw.Draw(mask)
    m.polygon(tapered_poly(pts, widths), fill=62)
    # deep centre narrower, only on the dug half
    half = int(n * 0.62)
    layer.polygon(tapered_poly(pts[:half], [w * 0.55 for w in widths[:half]]),
                  fill=DARK + (alpha,))
    # 2-3 fine inner striations (thin dark tears inside the gouge)
    for off_frac, lw in [(-0.28, 1.6), (0.05, 1.3), (0.32, 1.8)]:
        line = []
        for i in range(4, int(n * 0.9)):
            x, y = pts[i]
            x2, y2 = pts[min(i + 1, n - 1)]
            x1, y1 = pts[max(i - 1, 0)]
            dx, dy = x2 - x1, y2 - y1
            L = math.hypot(dx, dy) or 1.0
            nx, ny = -dy / L, dx / L
            line.append((x + nx * widths[i] * off_frac, y + ny * widths[i] * off_frac))
        if len(line) > 2:
            layer.line(line, fill=STRIA + (alpha,), width=int(lw + 1))
    # sparse wet glints: a few tiny dots on the pool half only, never a line
    for i in rng.sample(range(4, half), min(6, half - 4)):
        if rng.random() < 0.4:
            continue
        x, y = pts[i]
        r = rng.uniform(1.0, 2.6)
        layer.ellipse([x - r, y - r, x + r, y + r], fill=SPEC + (110,))


def drip(layer, mask, x, y, length, w, rng, alpha=245):
    # thin wobbly gravity runnel with a bulb end — polygon, not beads
    n = max(10, int(length / 12))
    cx, pts, ws = x, [], []
    for i in range(n):
        t = i / (n - 1)
        cx += rng.uniform(-2.0, 2.0)
        pts.append((cx, y + t * length))
        ws.append(max(0.8, w * (1 - t * 0.55)))
    layer.polygon(tapered_poly(pts, ws), fill=BODY + (alpha,))
    ImageDraw.Draw(mask).polygon(tapered_poly(pts, ws), fill=95)
    ex, ey = pts[-1]
    layer.ellipse([ex - w - 1.5, ey - 1.5, ex + w + 1.5, ey + w * 2 + 1.5],
                  fill=DARK + (alpha,))


def fan(layer, mask, cx, cy, angle_deg, slashes, length, spread_deg, rng):
    pool(layer, mask, cx, cy, rng)
    base = math.radians(angle_deg)
    for _ in range(slashes):
        off = math.radians(rng.uniform(-spread_deg, spread_deg) / 2)
        a = base + off
        ln = length * rng.uniform(0.80, 1.12)
        dx, dy = math.cos(a), math.sin(a)
        bow = rng.uniform(-0.12, 0.12)
        px, py = -dy, dx
        p0 = (cx + rng.uniform(-8, 8), cy + rng.uniform(-8, 8))
        p2 = (p0[0] + dx * ln, p0[1] + dy * ln)
        p1 = ((p0[0] + p2[0]) / 2 + px * ln * bow,
              (p0[1] + p2[1]) / 2 + py * ln * bow)
        w0 = rng.uniform(12, 22)
        streak(layer, mask, p0, p1, p2, w0, rng, alpha=rng.randint(240, 255))
        if rng.random() < 0.65:
            drip(layer, mask, p2[0] + rng.uniform(-5, 5), p2[1],
                 rng.uniform(60, 230), rng.uniform(2.6, 5.2), rng)


def main():
    rng = random.Random(SEED)
    base = build_base(rng)

    slashes = Image.new("RGBA", (W, H), (0, 0, 0, 0))
    smask = Image.new("L", (W, H), 0)

    for _ in range(40):
        x, y = rng.uniform(0, W), rng.uniform(0, H)
        rx, ry = rng.uniform(100, 300), rng.uniform(70, 190)
        ImageDraw.Draw(slashes).ellipse(
            [x - rx, y - ry, x + rx, y + ry], fill=(58, 7, 6, rng.randint(14, 34)))

    dl, dm = ImageDraw.Draw(slashes), smask  # dm unused directly; mask drawn via new Draw each streak
    clusters = [
        (480, 520, -140, 16, 620, 55),
        (1560, 500, -25, 16, 640, 55),
        (700, 920, -60, 15, 600, 55),
        (1380, 930, 55, 15, 620, 55),
        (1020, 680, 90, 12, 520, 60),
    ]
    for cx, cy, ang, nn, ln, sp in clusters:
        fan(dl, smask, cx, cy, ang, nn, ln, sp, rng)
    for _ in range(12):
        x, y = rng.uniform(150, W - 150), rng.uniform(150, H - 150)
        a = math.radians(rng.uniform(0, 360))
        ln = rng.uniform(320, 600)
        dx, dy = math.cos(a), math.sin(a)
        bow = rng.uniform(-0.16, 0.16)
        p0 = (x, y)
        p2 = (x + dx * ln, y + dy * ln)
        p1 = ((p0[0] + p2[0]) / 2 - dy * ln * bow, (p0[1] + p2[1]) / 2 + dx * ln * bow)
        streak(dl, smask, p0, p1, p2, rng.uniform(7, 13), rng,
               alpha=rng.randint(225, 250))
    for _ in range(150):
        cx, cy = rng.uniform(0, W), rng.uniform(0, H)
        r = rng.uniform(1.2, 6)
        dl.ellipse([cx - r, cy - r, cx + r, cy + r],
                   fill=BODY + (rng.randint(170, 250),))

    albedo = Image.alpha_composite(base, slashes).convert("RGB")

    smooth = Image.new("L", (W, H), 51)
    gloss = smask.point(lambda v: 45 + int(v / 255 * 70) if v else 45)
    from PIL import ImageChops
    smooth = ImageChops.lighter(smooth, gloss)
    mr = Image.new("RGBA", (W, H), (0, 0, 0, 255))
    mr.putalpha(smooth)

    RDIR.mkdir(parents=True, exist_ok=True)
    albedo.save(ALBEDO_OUT)
    mr.save(MR_OUT)
    print("wrote", ALBEDO_OUT, albedo.size)
    print("wrote", MR_OUT, mr.size)


if __name__ == "__main__":
    main()
