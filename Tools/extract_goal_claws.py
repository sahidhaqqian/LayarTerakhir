#!/usr/bin/env python3
"""Extract the designer's own claw strokes from the goal photo
(R2 asset/cakaran_yang_aku_inginkan.jpg) into a transparent decal that is
stuck flat on the bathroom tile wall.

Method: blood pixels are strongly red-dominant AND dark; plaster/wood/shadow
are not. bloodness = (R-G)*(R-B) with a soft ramp, blurred for feathered
edges. RGB keeps the photo's own blood color (darkened slightly for the dim
bathroom); everything else is transparent so OUR tile wall shows through.

Deterministic. Output:
  Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture/CrimsonGoalDecal.png
"""
from pathlib import Path

import numpy as np
from PIL import Image, ImageFilter

ROOT = Path(__file__).resolve().parent.parent
GOAL = Path("/tmp/opencode/cakaran_yang_aku_inginkan.jpg")
OUT = ROOT / "Assets/_LayarTerakhir/assets/Art/Imported/Meshy/CrimsonRupture/CrimsonGoalDecal.png"

# Crop to the claw mass (drop far edges: door frame left, pot corner right).
# Goal is 1376x768; fans span roughly x 60..1300, y 60..740.
BOX = (40, 30, 1330, 760)


def main():
    img = Image.open(GOAL).convert("RGB").crop(BOX)
    a = np.asarray(img).astype(np.float32)
    R, G, B = a[:, :, 0], a[:, :, 1], a[:, :, 2]
    lum = 0.2126 * R + 0.7152 * G + 0.0722 * B
    m = (R - G) * (R - B)  # blood ~10k, wood ~2k, plaster ~0
    mx = np.maximum(np.maximum(R, G), B)
    mn = np.minimum(np.minimum(R, G), B)
    sat = (mx - mn) / np.maximum(mx, 1)  # blood ~0.7+, veil ~0.15
    alpha = np.clip((m - 350) / (1100 - 350), 0, 1)
    alpha *= np.clip((sat - 0.35) / (0.60 - 0.35), 0, 1)
    # reject bright non-blood (plaster highlights) and near-black crush
    alpha *= np.clip((200 - lum) / 120, 0, 1)
    am = Image.fromarray((alpha * 255).astype(np.uint8), "L").filter(ImageFilter.GaussianBlur(1.0))
    rgb = np.asarray(img).astype(np.float32)
    rgb *= 0.95
    out = Image.merge("RGBA", (*Image.fromarray(rgb.astype(np.uint8)).split(), am))
    OUT.parent.mkdir(parents=True, exist_ok=True)
    out.save(OUT)
    print("wrote", OUT, out.size, "opaque%%=%.1f" % (100 * (np.asarray(am) > 12).mean()))


if __name__ == "__main__":
    main()
