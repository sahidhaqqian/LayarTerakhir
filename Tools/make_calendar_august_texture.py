"""Regenerate the Raka's-room wall calendar texture (CalendarAugust.png).

Design (Docs/Prolog.MD): August marks the 17th as Merdeka with the note
"Ibu bertambah usia tiga hari sebelumnya. Jangan lupa telepon." The player
must DEDUCE the 14th — so the sheet rings 17 and never prints the answer.
An older texture rang the 14th and printed "Ultah Ibu 14 - 08", which
short-circuits the padlock puzzle and contradicts the modal + tests.

Layout mirrors the previous sheet (red AGUSTUS/08 header band, SEN..MIN
columns with August 1st on Monday, Sundays in red) so the wall visual,
the modal paper and the deduction note all agree. Deterministic output.
"""
from pathlib import Path

from PIL import Image, ImageDraw, ImageFont

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Assets/_LayarTerakhir/assets/Art/Original/Textures/CalendarAugust.png'

W, H = 512, 700
BAND_H = 118
RED = (148, 22, 18)
INK = (38, 34, 28)
SUNDAY = (158, 26, 22)
CREAM = (206, 194, 164)
RULE = (90, 70, 60)


def font(size, bold=False):
    for p in (f'/usr/share/fonts/truetype/dejavu/DejaVuSans{("-Bold" if bold else "")}.ttf',):
        try:
            return ImageFont.truetype(p, size)
        except OSError:
            continue
    return ImageFont.load_default()


img = Image.new('RGB', (W, H), CREAM)
px = img.load()
for y in range(H):
    for x in range(W):
        grain = ((x * 7 + y * 13) % 11) - 5
        r, g, b = px[x, y]
        px[x, y] = (max(0, r + grain), max(0, g + grain), max(0, b + grain))
d = ImageDraw.Draw(img)
d.rectangle([0, 0, W, BAND_H], fill=RED)
d.text((28, 22), 'AGUSTUS', font=font(64, True), fill=(238, 228, 205))
d.text((W - 118, 28), '08', font=font(58, True), fill=(238, 228, 205))
d.line([0, BAND_H, W, BAND_H], fill=RULE, width=3)

cols = ['SEN', 'SEL', 'RAB', 'KAM', 'JUM', 'SAB', 'MIN']
cw = W / 7
y0 = BAND_H + 34
for i, c in enumerate(cols):
    d.text((i * cw + 14, y0), c, font=font(23, True),
           fill=SUNDAY if c == 'MIN' else INK)
y0 += 52
for day in range(1, 32):
    n = day - 1
    cx = (n % 7) * cw + 30
    cy = y0 + (n // 7) * 62
    sunday = (n % 7) == 6
    d.text((cx, cy), str(day), font=font(34, True),
           fill=SUNDAY if sunday else INK)
    if day == 17:
        d.ellipse([cx - 8, cy - 4, cx + 62, cy + 52], outline=RED, width=5)

note = ['Ibu bertambah usia', 'tiga hari sebelumnya!']
ny = y0 + 5 * 62 + 6
for j, line in enumerate(note):
    d.text((60, ny + j * 40), line, font=font(30), fill=RED)
d.line([40, ny + 88, W - 40, ny + 88], fill=RULE, width=3)
img.save(OUT)
print('CALENDAR_TEXTURE', OUT.relative_to(ROOT), OUT.stat().st_size, 'bytes')
