"""Blender: repair the reviewed playtest artwork, preserving staging and Golden assets.
blender -b --python Tools/repair_playtest_art.py
"""
from pathlib import Path
import bpy, bmesh, math, json
from mathutils import Vector
ROOT=Path(__file__).resolve().parents[1]
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Atmaja/Models'
MANIFEST=ROOT/'Audit/atmaja-art-manifest.json'
data=json.loads(MANIFEST.read_text())
WOOD='House_MAT_WoodDoor';BRASS='Detail_AgedBrass';INK='Detail_ClockInk'
def clear():bpy.ops.wm.read_factory_settings(use_empty=True)
def material(name):return bpy.data.materials.get(name) or bpy.data.materials.new(name)
def box(name,p,s,mat=WOOD,bevel=.008):
    bpy.ops.mesh.primitive_cube_add(size=1,location=p);o=bpy.context.object;o.name=name;o.scale=s
    bpy.ops.object.transform_apply(location=False,rotation=False,scale=True);o.data.materials.append(material(mat))
    if bevel:
        m=o.modifiers.new('Worn edge','BEVEL');m.width=bevel;m.segments=2;bpy.ops.object.modifier_apply(modifier=m.name)
    return o
def cylinder(name,p,r,d,mat,rot=(0,0,0)):
    bpy.ops.mesh.primitive_cylinder_add(vertices=24,radius=r,depth=d,location=p,rotation=rot);o=bpy.context.object;o.name=name;o.data.materials.append(material(mat));return o
def tube(name,points,r,mat):
    c=bpy.data.curves.new(name,'CURVE');c.dimensions='3D';c.bevel_depth=r;c.bevel_resolution=1
    s=c.splines.new('POLY');s.points.add(len(points)-1)
    for v,p in zip(s.points,points):v.co=(*p,1)
    o=bpy.data.objects.new(name,c);bpy.context.collection.objects.link(o);o.data.materials.append(material(mat));return o
def export(name,uv=True):
    for o in list(bpy.context.scene.objects):
        if o.type in ('CURVE','FONT'):
            bpy.ops.object.select_all(action='DESELECT');o.select_set(True);bpy.context.view_layer.objects.active=o;bpy.ops.object.convert(target='MESH')
    # Bake arbitrary card/object rotations before joining, so renderer bounds match actual geometry.
    for o in list(bpy.context.scene.objects):
        if o.type=='MESH':
            matrix=o.matrix_world.copy();o.data.transform(matrix)
            from mathutils import Matrix
            o.matrix_world=Matrix.Identity(4)
    groups={}
    for o in list(bpy.context.scene.objects):
        if o.type!='MESH':continue
        role='ClockPendulum' if o.name.startswith('ClockPendulum') else o.name if o.name in ('ClockHourBlade','ClockMinuteBlade','ClockDrawer') else 'Static'
        groups.setdefault((role,o.data.materials[0].name),[]).append(o)
    for (role,mat),objects in groups.items():
        bpy.ops.object.select_all(action='DESELECT')
        for o in objects:o.select_set(True)
        bpy.context.view_layer.objects.active=objects[0]
        if len(objects)>1:bpy.ops.object.join()
        o=bpy.context.object;o.name=role+'__'+mat
        if uv:
            if not o.data.uv_layers:o.data.uv_layers.new()
            layer=o.data.uv_layers.active.data
            for poly in o.data.polygons:
                n=o.matrix_world.to_3x3()@poly.normal;axis=max(range(3),key=lambda i:abs(n[i]));axes=[i for i in range(3) if i!=axis]
                for li in poly.loop_indices:
                    p=o.matrix_world@o.data.vertices[o.data.loops[li].vertex_index].co;layer[li].uv=(p[axes[0]],p[axes[1]])
    bpy.ops.object.select_all(action='SELECT')
    path=OUT/(name+'.fbx')
    bpy.ops.export_scene.fbx(filepath=str(path),object_types={'MESH'},bake_anim=False,axis_forward='-Z',axis_up='Y',path_mode='STRIP',add_leaf_bones=False)
    meshes=[o for o in bpy.context.scene.objects if o.type=='MESH']
    record={'name':name,'path':str(path.relative_to(ROOT)),'triangles':sum(sum(len(p.vertices)-2 for p in o.data.polygons) for o in meshes),'renderers':len(meshes),'revision':'2026-10-16 playtest repair'}
    data['models']=[m for m in data['models'] if m['name']!=name]+[record]
    print('REVIEW_ART',record)

clear()
# Deliberately known facing: the sitter faces Blender -Y (Unity +Z).
for x in (-.205,.205):
    for y in (-.195,.195):box('Tapered chair leg',(x,y,.23),(.055,.055,.46))
    box('Seat side rail',(x,0,.42),(.045,.44,.065))
    box('Tall back post',(x,.205,.75),(.055,.055,.68))
for y in (-.20,.20):box('Cross rail',(0,y,.41),(.44,.045,.065))
for i in range(5):box('Solid wooden seat slat',(0,-.18+i*.09,.465),(.48,.082,.035))
for z in (.68,.82,1.0):box('Curved back slat',(0,.212,z),(.43,.035,.075))
for x in (-.205,.205):box('Low side stretcher',(x,0,.18),(.024,.40,.028))
export('WornWoodChair')

clear()
box('Long dining top',(0,0,.90),(1.45,4.4,.075),bevel=.025)
for x in (-.61,.61):
    box('Long apron',(x,0,.82),(.065,4.15,.11))
    for y in (-2.04,2.04):box('Dining leg',(x,y,.43),(.09,.09,.86))
export('LongDiningTable')

clear()
box('Corner side table top',(0,0,.73),(.72,.52,.055),bevel=.018)
for x in (-.28,.28):
    for y in (-.18,.18):box('Side table leg',(x,y,.35),(.05,.05,.70))
box('Side table lower shelf',(0,0,.23),(.60,.39,.035))
export('CornerSideTable')

clear()
box('Clock back',(0,.10,1.10),(.62,.08,2.12))
for x in (-.28,.28):box('Carved pilaster',(x,0,1.10),(.085,.30,2.08))
for z,w,d in [(.07,.76,.42),(.16,.70,.37),(1.45,.66,.35),(2.10,.72,.38),(2.18,.78,.43)]:box('Clock cornice',(0,0,z),(w,d,.075))
cylinder('Ivory dial',(0,-.17,1.78),.235,.022,'Scooter_SurveyNotebookPaper',(math.pi/2,0,0))
roman=['XII','I','II','III','IV','V','VI','VII','VIII','IX','X','XI']
font=bpy.data.fonts.load('/usr/share/fonts/truetype/dejavu/DejaVuSerif.ttf')
for i,word in enumerate(roman):
    a=i*math.pi/6;curve=bpy.data.curves.new('Painted numeral','FONT');curve.body=word;curve.font=font;curve.size=.049;curve.align_x='CENTER';curve.align_y='CENTER'
    o=bpy.data.objects.new('Dial numeral '+word,curve);bpy.context.collection.objects.link(o);o.location=(.174*math.sin(a),-.184,1.78+.174*math.cos(a));o.rotation_euler=(math.pi/2,0,0);o.data.materials.append(material(INK))
box('ClockHourBlade',(0,-.193,1.835),(.020,.010,.15),BRASS,.003)
box('ClockMinuteBlade',(0,-.203,1.87),(.013,.010,.21),BRASS,.003)
cylinder('Dial hub',(0,-.215,1.78),.023,.02,BRASS,(math.pi/2,0,0))
box('ClockPendulum',(0,-.055,.98),(.016,.022,.64),BRASS)
cylinder('ClockPendulumBob',(0,-.055,.64),.12,.028,BRASS,(math.pi/2,0,0))
box('ClockDrawer',(0,-.02,.28),(.47,.30,.19))
cylinder('Drawer knob',(0,-.185,.28),.020,.035,BRASS,(math.pi/2,0,0))
export('GrandfatherClock')

# Repair the actual culprit: the large root plate crossed the east wall into the dining room.
bpy.ops.wm.open_mainfile(filepath=str(ROOT/'ArtSources/Scene2_Atmaja_Recovery/03_Frangipani_Tree/Blender/FallenFrangipani.blend'))
for o in list(bpy.context.scene.objects):
    if o.type!='MESH':bpy.data.objects.remove(o,do_unlink=True);continue
    is_leaf=o.name.startswith('LEAF');is_flower=o.name.startswith('FLOWER')
    if '_LOD' in o.name and not o.name.endswith('_LOD0'):
        bpy.data.objects.remove(o,do_unlink=True);continue
    if not is_leaf and not is_flower and not any('FRANG_Bark' in m.name for m in o.data.materials):
        bpy.data.objects.remove(o,do_unlink=True);continue
    if is_leaf or is_flower:
        centre=sum((o.matrix_world@v.co for v in o.data.vertices),Vector())/max(1,len(o.data.vertices));inv=o.matrix_world.inverted()
        for v in o.data.vertices:v.co=inv@(centre+(o.matrix_world@v.co-centre)*(.45 if is_leaf else .55))
    else:
        # Clip in world space. Every vertex stays outside the house footprint after placement.
        matrix=o.matrix_world.copy();o.data.transform(matrix)
        from mathutils import Matrix
        o.matrix_world=Matrix.Identity(4);bm=bmesh.new();bm.from_mesh(o.data)
        bmesh.ops.bisect_plane(bm,geom=list(bm.verts)+list(bm.edges)+list(bm.faces),plane_co=(.3,0,0),plane_no=(1,0,0),clear_inner=True,clear_outer=False)
        bm.to_mesh(o.data);bm.free()
        for v in o.data.vertices:
            p=v.co
            if 1.15<p.x<3.9:
                blend=max(0,min(1,(p.x-1.15)/.35,(3.9-p.x)/.35));p.z+=max(0,1.25-p.z)*blend
        if len(o.data.polygons)>600:
            bpy.context.view_layer.objects.active=o;m=o.modifiers.new('Tree budget','DECIMATE');m.ratio=.2;bpy.ops.object.modifier_apply(modifier=m.name)
    for m in o.data.materials:
        if not m.name.startswith('Tree_'):m.name='Tree_'+m.name.replace('.','_')
    # Source meshes' UVs are retained, including leaf cutouts.
export('FallenFrangipani',False)
data['materials']=[m for m in data['materials'] if m['name']!=INK]+[dict(name=INK,color=[.055,.039,.02,1],roughness=.95,metallic=0,alphaClip=False,albedo='',normal='',mask='')]
MANIFEST.write_text(json.dumps(data,indent=2)+'\n')
