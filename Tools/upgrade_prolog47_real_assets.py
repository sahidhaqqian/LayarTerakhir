"""Generate real CC0-backed textures and upgraded high-detail Prolog47 FBX props.
Run:
  python3 Tools/upgrade_prolog47_real_assets.py --textures
  blender -b -t 2 --python-exit-code 1 --python Tools/upgrade_prolog47_real_assets.py -- --meshes
"""
import sys, os, math, zipfile, io
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
RES_DIR = ROOT / 'Assets/_LayarTerakhir/Resources'
TEX_DIR = ROOT / 'Assets/_LayarTerakhir/assets/Art/Original/Textures/Prolog47'
MOD_DIR = ROOT / 'Assets/_LayarTerakhir/assets/Art/Original/Models/Prolog47'
PAPER_ZIP = ROOT / 'Builds/ArtSources/PrologRealWorld/Paper001_1K-JPG.zip'
HAND_SVG = ROOT / 'Builds/ArtSources/PrologRealWorld/handprint.svg'


def build_textures():
    import numpy as np
    from PIL import Image, ImageDraw, ImageFilter, ImageFont, ImageChops, ImageEnhance

    RES_DIR.mkdir(parents=True, exist_ok=True)
    TEX_DIR.mkdir(parents=True, exist_ok=True)

    # 1. Extract real CC0 Paper001 scan and build PrologNotebookSheet.png + AgedNotebookPaperDiffuse.png
    with zipfile.ZipFile(PAPER_ZIP, 'r') as zf:
        color_name = next(n for n in zf.namelist() if 'Color' in n and n.endswith('.jpg'))
        paper_base = Image.open(io.BytesIO(zf.read(color_name))).convert('RGB')

    # Warm aged paper tone from real scan
    sheet_w, sheet_h = 1024, 720
    paper = paper_base.resize((sheet_w, sheet_h), Image.Resampling.LANCZOS)
    arr = np.asarray(paper, dtype=np.float32)
    # Tint into realistic warm cream vintage Indonesian notebook paper + subtle fiber contrast
    mean_l = arr.mean()
    arr = (arr - mean_l) * 1.35 + mean_l
    arr[:, :, 0] *= 0.96
    arr[:, :, 1] *= 0.91
    arr[:, :, 2] *= 0.77

    # Subtle aged edge darkening & damp corner stain so it feels physical and lived-in
    yy, xx = np.mgrid[0:sheet_h, 0:sheet_w].astype(np.float32)
    nx = (xx / sheet_w) * 2.0 - 1.0
    ny = (yy / sheet_h) * 2.0 - 1.0
    vignette = np.clip(1.0 - 0.24 * (nx**4 + ny**4) - 0.10 * (nx**2 + ny**2), 0.62, 1.0)
    # Slight tea/damp stain near bottom-right & top-right
    stain1 = np.exp(-(((xx - 890) / 140.0) ** 2 + ((yy - 610) / 110.0) ** 2)) * 0.12
    stain2 = np.exp(-(((xx - 920) / 110.0) ** 2 + ((yy - 90) / 85.0) ** 2)) * 0.09
    arr *= (vignette - stain1 - stain2)[:, :, None]
    # Dark desk border / torn spine strip on far left (0..22px)
    spine_mask = np.clip((xx - 14.0) / 10.0, 0.0, 1.0)
    arr = arr * (0.28 + 0.72 * spine_mask[:, :, None])
    sheet = Image.fromarray(np.clip(arr, 0, 255).astype(np.uint8), 'RGB').convert('RGBA')

    draw = ImageDraw.Draw(sheet, 'RGBA')
    # Header double rule + horizontal ruled notebook lines
    draw.line([(98, 118), (976, 118)], fill=(128, 72, 68, 135), width=2)
    draw.line([(98, 123), (976, 123)], fill=(128, 72, 68, 95), width=1)
    for y in range(158, sheet_h - 68, 32):
        draw.line([(28, y), (976, y)], fill=(86, 108, 126, 86), width=2)
    # Classic red vertical left margin line
    draw.line([(100, 28), (100, sheet_h - 28)], fill=(168, 66, 62, 138), width=2)
    draw.line([(105, 28), (105, sheet_h - 28)], fill=(168, 66, 62, 72), width=1)

    # Ring-binder punched holes along left spine with realistic dark backing & paper reinforcement shadow
    for hy in [108, 252, 396, 540, 636] if sheet_h > 680 else [110, 360, 610]:
        if hy > sheet_h - 60:
            continue
        hx = 52
        draw.ellipse([hx - 18, hy - 18, hx + 18, hy + 18], fill=(165, 148, 120, 120))
        draw.ellipse([hx - 13, hy - 13, hx + 13, hy + 13], fill=(22, 18, 15, 238), outline=(92, 78, 60, 200), width=2)

    # Subtle outer page border highlight/shadow
    draw.rectangle([1, 1, sheet_w - 2, sheet_h - 2], outline=(72, 56, 40, 190), width=3)
    sheet.save(RES_DIR / 'PrologNotebookSheet.png')

    # Also save 1024x1024 3D open diary / note page diffuse texture
    p3d = paper_base.resize((1024, 1024), Image.Resampling.LANCZOS)
    a3 = np.asarray(p3d, dtype=np.float32)
    a3 = (a3 - a3.mean()) * 1.3 + a3.mean()
    a3[:, :, 0] *= 0.94
    a3[:, :, 1] *= 0.88
    a3[:, :, 2] *= 0.73
    y3, x3 = np.mgrid[0:1024, 0:1024].astype(np.float32)
    # Center gutter shadow for open book + aged outer edges
    gutter = 1.0 - 0.25 * np.exp(-(((x3 - 512.0) / 26.0) ** 2))
    edge3 = np.clip(1.0 - 0.22 * (((x3 / 512.0 - 1.0) ** 4) + ((y3 / 512.0 - 1.0) ** 4)), 0.65, 1.0)
    a3 *= (gutter * edge3)[:, :, None]
    img3d = Image.fromarray(np.clip(a3, 0, 255).astype(np.uint8), 'RGB')
    d3 = ImageDraw.Draw(img3d, 'RGBA')
    for y in range(110, 940, 34):
        d3.line([(48, y), (484, y)], fill=(86, 106, 122, 92), width=2)
        d3.line([(540, y), (976, y)], fill=(86, 106, 122, 92), width=2)
    d3.line([(96, 60), (96, 964)], fill=(162, 68, 64, 115), width=2)
    d3.line([(588, 60), (588, 964)], fill=(162, 68, 64, 115), width=2)
    img3d.save(TEX_DIR / 'AgedNotebookPaperDiffuse.png')

    # 2. Restroom Male & Female Door Logo Plaques (Bug 8)
    font_path = ROOT / 'Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-SemiBold.ttf'
    if not font_path.exists():
        font_path = ROOT / 'Assets/_LayarTerakhir/assets/UI/Fonts/Barlow-Regular.ttf'
    font = ImageFont.truetype(str(font_path), 54) if font_path.exists() else ImageFont.load_default()

    for kind, label, bg_col in [
        ('Male', 'PRIA', (26, 42, 58)),
        ('Female', 'WANITA', (58, 30, 40)),
    ]:
        pw, ph = 512, 640
        plaque = Image.new('RGBA', (pw, ph), (*bg_col, 255))
        pd = ImageDraw.Draw(plaque)
        cream = (236, 229, 212, 255)
        brass = (182, 158, 112, 255)
        pd.rounded_rectangle([14, 14, pw - 15, ph - 15], radius=28, outline=brass, width=8)
        pd.rounded_rectangle([28, 28, pw - 29, ph - 29], radius=20, outline=(236, 229, 212, 90), width=2)
        # Head circle
        cx = pw // 2
        pd.ellipse([cx - 46, 86, cx + 46, 178], fill=cream)
        if kind == 'Male':
            # Broad shoulders + torso + two legs
            pd.rounded_rectangle([cx - 82, 198, cx + 82, 344], radius=26, fill=cream)
            pd.rounded_rectangle([cx - 82, 226, cx - 56, 362], radius=12, fill=cream)
            pd.rounded_rectangle([cx + 56, 226, cx + 82, 362], radius=12, fill=cream)
            pd.rounded_rectangle([cx - 46, 326, cx - 8, 488], radius=14, fill=cream)
            pd.rounded_rectangle([cx + 8, 326, cx + 46, 488], radius=14, fill=cream)
            # Collar notch accent
            pd.polygon([(cx - 18, 198), (cx + 18, 198), (cx, 226)], fill=(*bg_col, 255))
        else:
            # Shoulders + A-line dress silhouette + arms + legs
            pd.polygon([(cx - 52, 202), (cx + 52, 202), (cx + 98, 386), (cx - 98, 386)], fill=cream)
            pd.ellipse([cx - 56, 196, cx + 56, 244], fill=cream)
            # Angled arms
            pd.line([(cx - 50, 216), (cx - 94, 336)], fill=cream, width=24)
            pd.line([(cx + 50, 216), (cx + 94, 336)], fill=cream, width=24)
            pd.rounded_rectangle([cx - 40, 376, cx - 8, 488], radius=13, fill=cream)
            pd.rounded_rectangle([cx + 8, 376, cx + 40, 488], radius=13, fill=cream)
        pd.line([(68, 518), (pw - 68, 518)], fill=brass, width=3)
        bbox = pd.textbbox((0, 0), label, font=font)
        tw = bbox[2] - bbox[0]
        pd.text(((pw - tw) // 2, 536), label, fill=cream, font=font)
        plaque.save(TEX_DIR / f'Restroom{kind}Plaque.png')

    # 3. Swallow-style Indonesian Flip-Flop texture (Bug 4)
    sw = Image.new('RGB', (512, 512), (235, 232, 222))
    sa = np.asarray(sw, dtype=np.float32)
    rng = np.random.default_rng(4704)
    # Diamond EVA rubber micro-grip pattern + realistic heel/forefoot wear shading
    yy, xx = np.mgrid[0:512, 0:512].astype(np.float32)
    diamond = (np.sin((xx + yy) * 0.35) * np.sin((xx - yy) * 0.35)) * 7.0
    noise = rng.normal(0, 3.0, (512, 512))
    heel_wear = np.exp(-(((xx - 256) / 110.0) ** 2 + ((yy - 380) / 95.0) ** 2)) * 26.0
    ball_wear = np.exp(-(((xx - 256) / 130.0) ** 2 + ((yy - 165) / 105.0) ** 2)) * 22.0
    sa += (diamond + noise - heel_wear - ball_wear)[:, :, None]
    sw_img = Image.fromarray(np.clip(sa, 0, 255).astype(np.uint8), 'RGB')
    sw_img.save(TEX_DIR / 'SwallowFootbedDiffuse.png')

    # 4. Arterial Blood Handprint & 5-Finger Desperate Bloody Wall Scratches (Bugs 22 & 34)
    # Layout matches Prolog47SceneRepair UV split:
    #   Left u = [0.00 .. 0.61]: real photo-traced human palm from handprint.svg in arterial crimson + drips
    #   Right u = [0.615 .. 1.00]: 5 desperate torn plaster & bloody fingernail claw gouges
    import cairosvg
    atlas = np.zeros((1024, 1024, 4), dtype=np.float32)

    svg_png = cairosvg.svg2png(url=str(HAND_SVG), output_width=540, output_height=690)
    svg_img = Image.open(io.BytesIO(svg_png)).convert('RGBA')
    hand_canvas = Image.new('L', (624, 1024), 0)
    hand_canvas.paste(svg_img.split()[3], (42, 74))

    # Add realistic gravity blood drips & smeared wrist drag below palm
    hd = ImageDraw.Draw(hand_canvas)
    for dx, dy0, dlen, dw in [(198, 700, 195, 7), (262, 725, 240, 10), (336, 718, 175, 6), (402, 685, 130, 5), (154, 340, 95, 4)]:
        pts = [(dx, dy0)]
        cx_d = float(dx)
        for step_i in range(1, 9):
            cx_d += float(rng.uniform(-2.2, 2.2))
            pts.append((int(cx_d), int(dy0 + dlen * (step_i / 8.0))))
        hd.line(pts, fill=215, width=dw)
        hd.ellipse([pts[-1][0] - dw, pts[-1][1] - dw, pts[-1][0] + dw, pts[-1][1] + dw + 3], fill=235)

    hm_raw = np.asarray(hand_canvas, dtype=np.float32) / 255.0
    hm_soft = np.asarray(hand_canvas.filter(ImageFilter.GaussianBlur(7)), dtype=np.float32) / 255.0
    y_h, x_h = np.mgrid[0:1024, 0:624].astype(np.float32)
    fine_noise = np.asarray(
        Image.fromarray(rng.uniform(0, 255, (320, 195)).astype(np.uint8)).resize((624, 1024), Image.Resampling.BILINEAR),
        dtype=np.float32,
    ) / 255.0
    coarse_noise = np.asarray(
        Image.fromarray(rng.uniform(0, 255, (64, 40)).astype(np.uint8)).resize((624, 1024), Image.Resampling.BICUBIC),
        dtype=np.float32,
    ) / 255.0

    # Combine crisp photo-traced palm creases with subtle coagulated edge halo & rough plaster pore breakup
    alpha_h = np.clip(hm_raw * (0.68 + 0.42 * fine_noise) + hm_soft * 0.22 * coarse_noise, 0.0, 0.97)
    for _ in range(42):
        sx = int(rng.integers(35, 590))
        sy = int(rng.integers(75, 960))
        sr = int(rng.integers(2, 7))
        rr = (x_h - sx) ** 2 + (y_h - sy) ** 2 <= sr**2
        alpha_h[rr] = np.maximum(alpha_h[rr], float(rng.uniform(0.55, 0.90)))

    # Arterial crimson blood color (brighter fresh arterial center, darker coagulated edges — never brown mud!)
    atlas[:, :624, 0] = 88.0 + 76.0 * hm_raw * (0.55 + 0.45 * coarse_noise)
    atlas[:, :624, 1] = 6.0 + 10.0 * (1.0 - hm_raw)
    atlas[:, :624, 2] = 6.0 + 8.0 * (1.0 - hm_raw)
    atlas[:, :624, 3] = alpha_h * 255.0

    # Right region u = [0.615 .. 1.00] (x = 630 .. 1023, width = 394):
    # 5 desperate curving human fingernail gouges in damp plaster with bloody nail streaks (organic arched rake!)
    sw_w = 1024 - 630
    y_s, x_s = np.mgrid[0:1024, 0:sw_w].astype(np.float32)
    plaster_alpha = np.zeros((1024, sw_w), dtype=np.float32)
    blood_mix = np.zeros((1024, sw_w), dtype=np.float32)

    s_noise = np.asarray(
        Image.fromarray(rng.uniform(0, 255, (256, 98)).astype(np.uint8)).resize((sw_w, 1024), Image.Resampling.BILINEAR),
        dtype=np.float32,
    ) / 255.0
    c_noise = np.asarray(
        Image.fromarray(rng.uniform(0, 255, (48, 24)).astype(np.uint8)).resize((sw_w, 1024), Image.Resampling.BICUBIC),
        dtype=np.float32,
    ) / 255.0

    # Natural 5-finger human hand span clawing diagonally/curving down as fingers tense inward
    nail_specs = [
        (48, 175, 92, 825, 5.2, -18.0),   # Thumb/outer finger
        (114, 96, 146, 892, 6.4, -14.0),  # Index
        (188, 68, 198, 926, 6.8, -8.0),   # Middle (longest)
        (258, 98, 248, 888, 6.2, 6.0),    # Ring
        (326, 168, 294, 818, 5.0, 14.0),  # Pinky converging inward
        # Secondary frantic slip scratches below
        (88, 360, 122, 770, 3.6, -10.0),
        (156, 320, 174, 810, 4.0, -5.0),
        (222, 335, 224, 795, 3.8, 4.0),
        (286, 385, 268, 740, 3.4, 9.0),
    ]
    for x0, y0, x1, y1, hw, camp in nail_specs:
        t = np.clip((y_s - y0) / max(1.0, (y1 - y0)), 0.0, 1.0)
        in_y = (y_s >= y0) & (y_s <= y1)
        tremor = (c_noise - 0.5) * 14.0 + (s_noise - 0.5) * 4.0 + np.sin(t * math.pi) * camp
        cx_line = x0 + (x1 - x0) * t + tremor
        dist = np.abs(x_s - cx_line)
        taper = np.maximum(0.0, np.sin(np.clip(t, 0.0, 1.0) * math.pi)) ** 0.38
        # Intermittent nail pressure skips so it looks clawed by human nails, not ruled lines
        pressure = np.clip(0.55 + 0.65 * c_noise, 0.25, 1.15)
        w_eff = hw * (0.35 + 0.80 * taper) * pressure
        outer = np.clip(1.0 - dist / np.maximum(1.4, w_eff * 1.45), 0.0, 1.0) * in_y
        core = np.clip(1.0 - dist / np.maximum(0.9, w_eff * 0.72), 0.0, 1.0) * in_y
        plaster_alpha = np.maximum(plaster_alpha, outer * (0.55 + 0.55 * s_noise) * pressure)
        blood_mix = np.maximum(blood_mix, core * (0.75 + 0.35 * s_noise))

    plaster_alpha = np.clip(plaster_alpha, 0.0, 0.95)
    blood_mix = np.clip(blood_mix, 0.0, 1.0)
    r_sc = (122.0 * (1.0 - blood_mix) + 112.0 * blood_mix)
    g_sc = (114.0 * (1.0 - blood_mix) + 10.0 * blood_mix)
    b_sc = (102.0 * (1.0 - blood_mix) + 9.0 * blood_mix)
    atlas[:, 630:, 0] = r_sc
    atlas[:, 630:, 1] = g_sc
    atlas[:, 630:, 2] = b_sc
    atlas[:, 630:, 3] = plaster_alpha * 255.0

    Image.fromarray(np.clip(atlas, 0, 255).astype(np.uint8), 'RGBA').save(TEX_DIR / 'HandprintAndScratches.png')
    print('UPGRADE_TEXTURES_OK', [
        str(RES_DIR / 'PrologNotebookSheet.png'),
        str(TEX_DIR / 'AgedNotebookPaperDiffuse.png'),
        str(TEX_DIR / 'RestroomMalePlaque.png'),
        str(TEX_DIR / 'RestroomFemalePlaque.png'),
        str(TEX_DIR / 'SwallowFootbedDiffuse.png'),
        str(TEX_DIR / 'HandprintAndScratches.png'),
    ])


def build_meshes():
    import bpy, bmesh
    from mathutils import Vector, Matrix

    def reset_scene():
        bpy.ops.wm.read_factory_settings(use_empty=True)

    def make_mat(name, color=(0.8, 0.8, 0.8, 1.0)):
        m = bpy.data.materials.get(name) or bpy.data.materials.new(name)
        m.diffuse_color = color
        return m

    def export_fbx(path):
        bpy.ops.export_scene.fbx(
            filepath=str(path),
            use_selection=False,
            object_types={'MESH'},
            axis_forward='-Z',
            axis_up='Y',
            apply_scale_options='FBX_SCALE_ALL',
            path_mode='STRIP',
        )
        print('EXPORTED_FBX', path, path.stat().st_size)

    # -------------------------------------------------------------------------
    # A. Classic Indonesian Swallow Flip-Flops (Bug 4) — NO white sole ring!
    # -------------------------------------------------------------------------
    reset_scene()
    mat_rubber = make_mat('Repair_SandalRubber', (0.12, 0.24, 0.36, 1.0))
    mat_footbed = make_mat('Repair_SandalFootbed', (0.90, 0.89, 0.85, 1.0))
    mat_strap = make_mat('Repair_SandalStrap', (0.14, 0.34, 0.56, 1.0))

    def build_single_sandal(name, sign_x, offset_x, yaw_deg, offset_z):
        # sign_x = +1 for right foot, -1 for left foot
        bm = bmesh.new()
        # Smooth anatomical flip-flop outline (length ~0.265m, width ~0.102m, thickness 0.015m)
        n_pts = 48
        bot_verts = []
        mid_verts = []
        top_verts = []
        for i in range(n_pts):
            a = (i / n_pts) * math.pi * 2.0
            ca, sa = math.cos(a), math.sin(a)
            # Forward is +Y in Blender (becomes -Z/+Z in Unity)
            y = sa * 0.132
            # Wider at forefoot (sa > 0), narrower waist (sa ~ -0.15), rounded heel (sa < -0.4)
            width = 0.046 + 0.008 * math.sin(a) - 0.006 * math.exp(-((sa + 0.12) / 0.32) ** 2)
            # Slight big-toe medial curve
            curve_x = sign_x * (-0.0045 * math.sin(a * 1.0) + 0.003 * (sa ** 2))
            x = ca * width + curve_x
            bv = bm.verts.new((x, y, 0.0))
            mv = bm.verts.new((x * 1.01, y * 1.005, 0.0105))
            tv = bm.verts.new((x * 0.985, y * 0.992, 0.0155))
            bot_verts.append(bv)
            mid_verts.append(mv)
            top_verts.append(tv)

        c_bot = bm.verts.new((0.0, 0.0, 0.0))
        c_top = bm.verts.new((0.0, 0.0, 0.0150))
        uv_layer = bm.loops.layers.uv.new('UVMap')

        for i in range(n_pts):
            j = (i + 1) % n_pts
            # Colored rubber bottom + lower sidewall (mat slot 0)
            f_b = bm.faces.new((c_bot, bot_verts[j], bot_verts[i]))
            f_b.material_index = 0
            f_s1 = bm.faces.new((bot_verts[i], bot_verts[j], mid_verts[j], mid_verts[i]))
            f_s1.material_index = 0
            # Off-white Swallow upper EVA footbed + bevelled top rim (mat slot 1)
            f_s2 = bm.faces.new((mid_verts[i], mid_verts[j], top_verts[j], top_verts[i]))
            f_s2.material_index = 1
            f_t = bm.faces.new((c_top, top_verts[i], top_verts[j]))
            f_t.material_index = 1

        # Planar UV projection for footbed texture
        for f in bm.faces:
            f.smooth = True
            for loop in f.loops:
                co = loop.vert.co
                loop[uv_layer].uv = (co.x / 0.12 + 0.5, co.y / 0.28 + 0.5)

        me = bpy.data.meshes.new(name + '_SoleMesh')
        bm.to_mesh(me)
        bm.free()
        sole_obj = bpy.data.objects.new(name + '_Sole', me)
        sole_obj.data.materials.append(mat_rubber)
        sole_obj.data.materials.append(mat_footbed)
        bpy.context.scene.collection.objects.link(sole_obj)

        # Classic Y-strap + toe post using smooth bevelled curves converted to mesh
        def make_tube(pts, radius, obj_name):
            cu = bpy.data.curves.new(obj_name + '_Curve', 'CURVE')
            cu.dimensions = '3D'
            cu.bevel_depth = radius
            cu.bevel_resolution = 4
            cu.resolution_u = 10
            sp = cu.splines.new('POLY')
            sp.points.add(len(pts) - 1)
            for idx, pt in enumerate(pts):
                sp.points[idx].co = (*pt, 1.0)
            co_obj = bpy.data.objects.new(obj_name, cu)
            bpy.context.scene.collection.objects.link(co_obj)
            bpy.ops.object.select_all(action='DESELECT')
            co_obj.select_set(True)
            bpy.context.view_layer.objects.active = co_obj
            bpy.ops.object.convert(target='MESH')
            co_obj.data.materials.append(mat_strap)
            for p in co_obj.data.polygons:
                p.use_smooth = True
            return co_obj

        toe_x = -sign_x * 0.013
        toe_y = 0.068
        post = make_tube([(toe_x, toe_y, 0.012), (toe_x, toe_y - 0.004, 0.036)], 0.0042, name + '_Post')
        # Left & right Y-strap wings arching smoothly over instep down into sole sides
        wing_in = make_tube([
            (toe_x, toe_y - 0.004, 0.035),
            (-sign_x * 0.025, 0.036, 0.044),
            (-sign_x * 0.038, -0.005, 0.034),
            (-sign_x * 0.044, -0.040, 0.010),
        ], 0.0052, name + '_WingIn')
        wing_out = make_tube([
            (toe_x, toe_y - 0.004, 0.035),
            (sign_x * 0.014, 0.034, 0.045),
            (sign_x * 0.036, -0.008, 0.034),
            (sign_x * 0.045, -0.044, 0.010),
        ], 0.0052, name + '_WingOut')

        bpy.ops.object.select_all(action='DESELECT')
        for o in (sole_obj, post, wing_in, wing_out):
            o.select_set(True)
        bpy.context.view_layer.objects.active = sole_obj
        bpy.ops.object.join()
        sole_obj.location = (offset_x, offset_z, 0.0)
        sole_obj.rotation_euler = (0.0, 0.0, math.radians(yaw_deg))
        return sole_obj

    build_single_sandal('FlipFlop_L', -1.0, -0.064, 5.5, 0.006)
    build_single_sandal('FlipFlop_R', 1.0, 0.064, -7.0, -0.006)
    export_fbx(MOD_DIR / 'IndonesianFlipFlops.fbx')

    # -------------------------------------------------------------------------
    # B. Unmistakable Cracked & Chipped Porcelain Milk Cup (Bug 33)
    # -------------------------------------------------------------------------
    reset_scene()
    mat_cup = make_mat('Repair_CupCeramic', (0.86, 0.84, 0.77, 1.0))
    mat_crack = make_mat('Repair_CupCrack', (0.11, 0.06, 0.04, 1.0))

    bm = bmesh.new()
    segs = 64
    # Lathe profile (r, z) in metres: height ~0.095m, rim radius ~0.044m
    # Include a jagged V-chip notch on BOTH front (angle ~ -pi/2 & +pi/2) so from ANY desk side the chip is obvious!
    outer_profile = [
        (0.000, 0.000),
        (0.029, 0.000),
        (0.031, 0.005),
        (0.034, 0.022),
        (0.039, 0.052),
        (0.0435, 0.088),
        (0.0445, 0.095),  # outer rim
        (0.0405, 0.095),  # inner rim
        (0.0365, 0.054),
        (0.0305, 0.020),
        (0.0260, 0.008),
        (0.000, 0.007),
    ]

    def notch_drop(angle_rad):
        # Two prominent V-chips: main wide V-chip at front (-Y, -90 deg) and secondary chip at (+Y, +90 deg)
        best = 0.0
        for center_a, width_a, depth_z in [(-math.pi * 0.5, 0.24, 0.024), (math.pi * 0.5, 0.20, 0.020), (0.25, 0.14, 0.015)]:
            da = abs((angle_rad - center_a + math.pi) % (2 * math.pi) - math.pi)
            if da < width_a:
                # Jagged V-notch
                t = 1.0 - da / width_a
                jag = 0.88 + 0.12 * math.sin(da * 55.0)
                best = max(best, depth_z * t * jag)
        return best

    rings = []
    for p_idx, (r, z) in enumerate(outer_profile):
        if r == 0.0:
            v = bm.verts.new((0.0, 0.0, z))
            rings.append([v] * segs)
            continue
        ring = []
        for s in range(segs):
            a = (s / segs) * 2.0 * math.pi - math.pi
            drop = notch_drop(a)
            z_cur = z
            if p_idx in (5, 6, 7):  # Rim & upper lip drop into sharp V-chip!
                scale_drop = 1.0 if p_idx in (6, 7) else 0.45
                z_cur = max(0.045, z - drop * scale_drop)
            ring.append(bm.verts.new((math.cos(a) * r, math.sin(a) * r, z_cur)))
        rings.append(ring)

    uv_cup = bm.loops.layers.uv.new('UVMap')
    for p_idx in range(len(outer_profile) - 1):
        r0, r1 = rings[p_idx], rings[p_idx + 1]
        for s in range(segs):
            sn = (s + 1) % segs
            a_mid = ((s + 0.5) / segs) * 2.0 * math.pi - math.pi
            is_chip_bevel = (p_idx == 6) and (notch_drop(a_mid) > 0.0025)
            v00, v01, v11, v10 = r0[s], r0[sn], r1[sn], r1[s]
            verts_set = []
            for vv in (v00, v01, v11, v10):
                if vv not in verts_set:
                    verts_set.append(vv)
            if len(verts_set) >= 3:
                f = bm.faces.new(verts_set)
                # Raw chipped ceramic edge + inner curdled milk bottom ring get dark stained material!
                f.material_index = 1 if (is_chip_bevel or p_idx == 10) else 0
                f.smooth = not is_chip_bevel
                for lp in f.loops:
                    lp[uv_cup].uv = (s / segs, p_idx / len(outer_profile))

    cup_me = bpy.data.meshes.new('CrackedMilkCup_Mesh')
    bm.to_mesh(cup_me)
    bm.free()
    cup_obj = bpy.data.objects.new('PorcelainCup', cup_me)
    cup_obj.data.materials.append(mat_cup)
    cup_obj.data.materials.append(mat_crack)
    bpy.context.scene.collection.objects.link(cup_obj)

    # Ear handle on +X side so front (-Y) and back (+Y) show the handle in silhouette + crack dead-center!
    def add_curve_mesh(pts, rad, mat, cname):
        cu = bpy.data.curves.new(cname + '_C', 'CURVE')
        cu.dimensions = '3D'
        cu.bevel_depth = rad
        cu.bevel_resolution = 4
        sp = cu.splines.new('NURBS')
        sp.points.add(len(pts) - 1)
        for i, p in enumerate(pts):
            sp.points[i].co = (*p, 1.0)
        sp.use_endpoint_u = True
        ob = bpy.data.objects.new(cname, cu)
        bpy.context.scene.collection.objects.link(ob)
        bpy.ops.object.select_all(action='DESELECT')
        ob.select_set(True)
        bpy.context.view_layer.objects.active = ob
        bpy.ops.object.convert(target='MESH')
        ob.data.materials.append(mat)
        for p in ob.data.polygons:
            p.use_smooth = True
        return ob

    handle = add_curve_mesh([
        (0.041, 0.0, 0.076),
        (0.064, 0.0, 0.078),
        (0.070, 0.0, 0.052),
        (0.060, 0.0, 0.028),
        (0.035, 0.0, 0.026),
    ], 0.0048, mat_cup, 'CupHandle')

    # Prominent jagged dark fracture cracks descending from BOTH V-chips down the outer & inner wall!
    crack_objs = []
    for base_ang, sign_zig in [(-math.pi * 0.5, 1.0), (math.pi * 0.5, -1.0), (0.25, 0.8)]:
        c_pts_out = []
        c_pts_branch = []
        steps = [
            (0.073, 0.00),
            (0.060, 0.05 * sign_zig),
            (0.046, -0.04 * sign_zig),
            (0.032, 0.07 * sign_zig),
            (0.018, 0.02 * sign_zig),
        ]
        for zz, da in steps:
            ang = base_ang + da
            # Outer wall radius at height zz
            rr = 0.031 + (0.0445 - 0.031) * (zz / 0.095) + 0.0011
            c_pts_out.append((math.cos(ang) * rr, math.sin(ang) * rr, zz))
        crack_objs.append(add_curve_mesh(c_pts_out, 0.0019, mat_crack, f'CrackLine_{base_ang:.2f}'))
        # Forked branch crack for unmistakable fractured porcelain silhouette
        b_pts = [
            c_pts_out[1],
            (math.cos(base_ang + 0.16 * sign_zig) * 0.0405, math.sin(base_ang + 0.16 * sign_zig) * 0.0405, 0.048),
            (math.cos(base_ang + 0.25 * sign_zig) * 0.0385, math.sin(base_ang + 0.25 * sign_zig) * 0.0385, 0.034),
        ]
        crack_objs.append(add_curve_mesh(b_pts, 0.0014, mat_crack, f'CrackBranch_{base_ang:.2f}'))

    # Broken ceramic shard resting right next to the cup base for extra environmental storytelling!
    bm_s = bmesh.new()
    s1 = bm_s.verts.new((-0.024, -0.052, 0.001))
    s2 = bm_s.verts.new((-0.006, -0.058, 0.001))
    s3 = bm_s.verts.new((-0.013, -0.043, 0.006))
    s4 = bm_s.verts.new((-0.024, -0.052, 0.004))
    s5 = bm_s.verts.new((-0.006, -0.058, 0.004))
    f_top = bm_s.faces.new((s4, s5, s3))
    f_top.material_index = 0
    f_edg = bm_s.faces.new((s1, s2, s5, s4))
    f_edg.material_index = 1
    me_s = bpy.data.meshes.new('CupShardMesh')
    bm_s.to_mesh(me_s)
    bm_s.free()
    shard = bpy.data.objects.new('CupShard', me_s)
    shard.data.materials.append(mat_cup)
    shard.data.materials.append(mat_crack)
    bpy.context.scene.collection.objects.link(shard)

    # Join porcelain parts into 'Fine cracked porcelain' and front fracture into 'Irregular glazed crack'
    # to satisfy Prolog47VisualTests.Details() exact renderer name lookup while keeping front/back fractures!
    handle.data.materials.clear()
    handle.data.materials.append(mat_cup)
    handle.data.materials.append(mat_crack)
    bpy.ops.object.select_all(action='DESELECT')
    for o in (cup_obj, handle, shard):
        o.select_set(True)
    bpy.context.view_layer.objects.active = cup_obj
    bpy.ops.object.join()
    cup_obj.name = 'Fine cracked porcelain'

    bpy.ops.object.select_all(action='DESELECT')
    for o in crack_objs[:2]:
        o.select_set(True)
    bpy.context.view_layer.objects.active = crack_objs[0]
    bpy.ops.object.join()
    crack_objs[0].name = 'Irregular glazed crack'

    bpy.ops.object.select_all(action='DESELECT')
    for o in crack_objs[2:]:
        o.select_set(True)
    bpy.context.view_layer.objects.active = crack_objs[2]
    bpy.ops.object.join()
    crack_objs[2].name = 'Back glazed crack'

    export_fbx(MOD_DIR / 'CrackedMilkCup.fbx')

    # -------------------------------------------------------------------------
    # C. Smooth Curved Porcelain Bathtub + Chrome Dual-Handle Faucet (Bug 36)
    # -------------------------------------------------------------------------
    reset_scene()
    mat_tub = make_mat('Repair_Ceramic', (0.84, 0.82, 0.75, 1.0))
    mat_stain = make_mat('Repair_CeramicEdge', (0.20, 0.04, 0.03, 1.0))
    mat_chrome = make_mat('Repair_Iron', (0.62, 0.64, 0.65, 1.0))
    mat_brass = make_mat('Repair_Brass', (0.48, 0.40, 0.24, 1.0))

    bm_t = bmesh.new()
    t_segs = 72
    # Superellipse stadium cross-section: length X = 1.74m (half = 0.87), width Y = 0.78m (half = 0.39), height Z = 0.54m
    # Smooth curved outer apron, rolled lip rim, sloped inner backrest, blood tide-mark ring!
    tub_rings_spec = [
        # (rx, ry, z, mat_idx)
        (0.82, 0.35, 0.00, 0),   # Skirted plinth base
        (0.84, 0.365, 0.06, 0),  # Recessed toe kick
        (0.865, 0.385, 0.48, 0), # Smooth curved outer apron wall
        (0.885, 0.402, 0.525, 0),# Rolled outer lip
        (0.865, 0.392, 0.545, 0),# Top of rolled rim
        (0.805, 0.340, 0.538, 0),# Inner rim curve
        (0.765, 0.305, 0.455, 1),# Dark crimson coagulated blood tide-mark ring around inner basin!
        (0.752, 0.296, 0.435, 1),# Tide-mark lower edge
        (0.715, 0.272, 0.220, 0),# Sloped inner porcelain basin wall
        (0.665, 0.242, 0.115, 0),# Curved inner cove
        (0.550, 0.190, 0.092, 0),# Inner basin floor outer
        (0.000, 0.000, 0.088, 0),# Basin drain center
    ]
    uv_t = bm_t.loops.layers.uv.new('UVMap')
    t_rings = []
    for rx, ry, zz, _ in tub_rings_spec:
        if rx == 0.0:
            v = bm_t.verts.new((0.0, 0.0, zz))
            t_rings.append([v] * t_segs)
            continue
        ring = []
        for s in range(t_segs):
            a = (s / t_segs) * 2.0 * math.pi
            ca, sa = math.cos(a), math.sin(a)
            # Superellipse exponent 0.62 gives classic rounded-end porcelain bathtub stadium shape!
            sx = math.copysign(abs(ca) ** 0.60, ca) * rx
            sy = math.copysign(abs(sa) ** 0.64, sa) * ry
            ring.append(bm_t.verts.new((sx, sy, zz)))
        t_rings.append(ring)

    for r_i in range(len(tub_rings_spec) - 1):
        r0, r1 = t_rings[r_i], t_rings[r_i + 1]
        midx = tub_rings_spec[r_i][3]
        for s in range(t_segs):
            sn = (s + 1) % t_segs
            vs = []
            for vv in (r0[s], r0[sn], r1[sn], r1[s]):
                if vv not in vs:
                    vs.append(vv)
            if len(vs) >= 3:
                f = bm_t.faces.new(vs)
                f.material_index = midx
                f.smooth = True
                for lp in f.loops:
                    lp[uv_t].uv = (s / t_segs, r_i / len(tub_rings_spec))

    # Bottom cap so underside is sealed
    c_bot_t = bm_t.verts.new((0.0, 0.0, 0.0))
    for s in range(t_segs):
        sn = (s + 1) % t_segs
        f = bm_t.faces.new((c_bot_t, t_rings[0][sn], t_rings[0][s]))
        f.material_index = 0

    tub_me = bpy.data.meshes.new('SmoothPorcelainBathtub')
    bm_t.to_mesh(tub_me)
    bm_t.free()
    tub_obj = bpy.data.objects.new('BathtubBasin', tub_me)
    tub_obj.data.materials.append(mat_tub)
    tub_obj.data.materials.append(mat_stain)
    bpy.context.scene.collection.objects.link(tub_obj)

    # Chrome mixer tap deck at head end (+X = 0.825): escutcheon plate, hot/cold cross handles, curved spout, drain ring!
    bpy.ops.mesh.primitive_cylinder_add(vertices=28, radius=0.032, depth=0.012, location=(0.825, 0.0, 0.551))
    esc = bpy.context.active_object
    esc.scale = (0.7, 3.2, 1.0)
    esc.data.materials.append(mat_chrome)

    for hy_tap in (-0.068, 0.068):
        bpy.ops.mesh.primitive_cylinder_add(vertices=20, radius=0.014, depth=0.055, location=(0.825, hy_tap, 0.578))
        stem = bpy.context.active_object
        stem.data.materials.append(mat_chrome)
        for p in stem.data.polygons:
            p.use_smooth = True
        bpy.ops.mesh.primitive_cylinder_add(vertices=16, radius=0.007, depth=0.062, location=(0.825, hy_tap, 0.604), rotation=(math.pi * 0.5, 0, 0))
        cross1 = bpy.context.active_object
        cross1.data.materials.append(mat_brass)
        bpy.ops.mesh.primitive_cylinder_add(vertices=16, radius=0.007, depth=0.062, location=(0.825, hy_tap, 0.604), rotation=(0, math.pi * 0.5, 0))
        cross2 = bpy.context.active_object
        cross2.data.materials.append(mat_brass)

    # Curved gooseneck faucet spout projecting inward over the tub bowl
    cu_sp = bpy.data.curves.new('TubSpoutCurve', 'CURVE')
    cu_sp.dimensions = '3D'
    cu_sp.bevel_depth = 0.014
    cu_sp.bevel_resolution = 6
    sp_s = cu_sp.splines.new('NURBS')
    spout_pts = [
        (0.825, 0.0, 0.552),
        (0.825, 0.0, 0.635),
        (0.775, 0.0, 0.665),
        (0.715, 0.0, 0.640),
        (0.705, 0.0, 0.595),
    ]
    sp_s.points.add(len(spout_pts) - 1)
    for idx, pt in enumerate(spout_pts):
        sp_s.points[idx].co = (*pt, 1.0)
    sp_s.use_endpoint_u = True
    spout_obj = bpy.data.objects.new('TubFaucetSpout', cu_sp)
    bpy.context.scene.collection.objects.link(spout_obj)
    bpy.ops.object.select_all(action='DESELECT')
    spout_obj.select_set(True)
    bpy.context.view_layer.objects.active = spout_obj
    bpy.ops.object.convert(target='MESH')
    spout_obj.data.materials.append(mat_chrome)
    for p in spout_obj.data.polygons:
        p.use_smooth = True

    # Overflow plate & brass drain strainer
    bpy.ops.mesh.primitive_cylinder_add(vertices=24, radius=0.028, depth=0.008, location=(0.745, 0.0, 0.39), rotation=(0, math.radians(78), 0))
    ovf = bpy.context.active_object
    ovf.data.materials.append(mat_brass)

    export_fbx(MOD_DIR / 'BloodBathTub.fbx')

    # -------------------------------------------------------------------------
    # D. Compact Recessed Wall Crawl Vent Grille (Bug 24)
    # -------------------------------------------------------------------------
    reset_scene()
    mat_iron = make_mat('Repair_Iron', (0.24, 0.26, 0.25, 1.0))
    mat_screw = make_mat('Repair_Brass', (0.42, 0.36, 0.24, 1.0))

    # Realistic crawlable wall HVAC return grille: 0.68m wide (X) x 0.48m tall (Z) x 0.036m deep (Y)
    W, H, D = 0.68, 0.48, 0.036
    frame_w = 0.042
    # Outer bevelled flange frame (4 bars)
    bars = [
        ('FrameTop', (0.0, 0.0, H * 0.5 - frame_w * 0.5), (W, D, frame_w)),
        ('FrameBot', (0.0, 0.0, -H * 0.5 + frame_w * 0.5), (W, D, frame_w)),
        ('FrameLeft', (-W * 0.5 + frame_w * 0.5, 0.0, 0.0), (frame_w, D, H - frame_w * 2)),
        ('FrameRight', (W * 0.5 - frame_w * 0.5, 0.0, 0.0), (frame_w, D, H - frame_w * 2)),
        ('CenterMullion', (0.0, 0.003, 0.0), (0.018, D * 0.65, H - frame_w * 2)),
    ]
    for bname, loc, sz in bars:
        bpy.ops.mesh.primitive_cube_add(size=1.0, location=loc)
        ob = bpy.context.active_object
        ob.name = bname
        ob.scale = sz
        bpy.ops.object.transform_apply(scale=True)
        bev = ob.modifiers.new('Bevel', 'BEVEL')
        bev.width = 0.003
        bev.segments = 2
        bpy.ops.object.modifier_apply(modifier=bev.name)
        ob.data.materials.append(mat_iron)

    # 9 angled horizontal sheet-metal louver slats so you see real angled slots, not prison radiator bars!
    inner_w = W - frame_w * 2.0 - 0.004
    n_slats = 9
    z_min, z_max = -H * 0.5 + frame_w + 0.022, H * 0.5 - frame_w - 0.022
    for i in range(n_slats):
        zz = z_min + (z_max - z_min) * (i / (n_slats - 1))
        bpy.ops.mesh.primitive_cube_add(size=1.0, location=(0.0, 0.0, zz))
        sl = bpy.context.active_object
        sl.name = f'LouverSlat_{i}'
        sl.scale = (inner_w, 0.024, 0.0045)
        sl.rotation_euler = (math.radians(32.0), 0.0, 0.0)
        bpy.ops.object.transform_apply(scale=True, rotation=True)
        sl.data.materials.append(mat_iron)

    # 4 corner mounting screws + latch knob on right edge
    for sx in (-W * 0.5 + 0.021, W * 0.5 - 0.021):
        for sz_c in (-H * 0.5 + 0.021, H * 0.5 - 0.021):
            bpy.ops.mesh.primitive_cylinder_add(vertices=14, radius=0.0065, depth=0.010, location=(sx, -D * 0.5 - 0.002, sz_c), rotation=(math.pi * 0.5, 0, 0))
            sc = bpy.context.active_object
            sc.data.materials.append(mat_screw)
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=(W * 0.5 - 0.022, -D * 0.5 - 0.008, 0.0))
    latch = bpy.context.active_object
    latch.scale = (0.022, 0.014, 0.048)
    bpy.ops.object.transform_apply(scale=True)
    latch.data.materials.append(mat_screw)

    export_fbx(MOD_DIR / 'VentGrille.fbx')


if __name__ == '__main__':
    if '--textures' in sys.argv:
        build_textures()
    if '--meshes' in sys.argv:
        build_meshes()
