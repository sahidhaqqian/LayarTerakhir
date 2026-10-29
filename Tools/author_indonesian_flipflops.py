import bpy, math, bmesh
from mathutils import Vector
from pathlib import Path
ROOT=Path(__file__).resolve().parents[1]
MODEL=ROOT/"Assets/_LayarTerakhir/assets/Art/Original/Models/IndonesianFlipFlop.fbx"
SOURCE=ROOT/"Assets/_LayarTerakhir/assets/ArtSources/Prolog/IndonesianFlipFlop.blend"
PREVIEW=ROOT/"Audit/Visuals/Prolog-Acceptance/flipflop-model.png"
for target in [MODEL,SOURCE,PREVIEW]:target.parent.mkdir(parents=True,exist_ok=True)
bpy.ops.object.select_all(action='SELECT');bpy.ops.object.delete(use_global=False)
def mat(name,color,rough):
 m=bpy.data.materials.new(name);m.diffuse_color=(*color,1);m.use_nodes=True;m.node_tree.nodes['Principled BSDF'].inputs['Base Color'].default_value=(*color,1);m.node_tree.nodes['Principled BSDF'].inputs['Roughness'].default_value=rough;return m
rubber=mat('FlipFlopRubber',(.035,.045,.037),.84);strap=mat('FlipFlopBlueStrap',(.065,.19,.29),.66);edge=mat('FlipFlopEdge',(.08,.09,.075),.82)
outline=[(.014,.132),(.036,.119),(.047,.085),(.046,.043),(.039,-.006),(.034,-.063),(.029,-.104),(.018,-.126),(-.003,-.134),(-.023,-.123),(-.035,-.096),(-.04,-.042),(-.043,.007),(-.05,.054),(-.049,.093),(-.033,.123),(-.012,.134)]
controls=outline;outline=[]
for i in range(len(controls)):
 p0=Vector(controls[(i-1)%len(controls)]);p1=Vector(controls[i]);p2=Vector(controls[(i+1)%len(controls)]);p3=Vector(controls[(i+2)%len(controls)])
 for j in range(5):
  t=j/5;v=.5*((2*p1)+(-p0+p2)*t+(2*p0-5*p1+4*p2-p3)*t*t+(-p0+3*p1-3*p2+p3)*t*t*t);outline.append(tuple(v))
n=len(outline);verts=[(x,y,z) for z in [0,.013] for x,y in outline];faces=[tuple(range(n-1,-1,-1)),tuple(range(n,2*n))]+[(i,(i+1)%n,(i+1)%n+n,i+n) for i in range(n)]
mesh=bpy.data.meshes.new('AnatomicalRubberSole');mesh.from_pydata(verts,[],faces);mesh.update();bm=bmesh.new();bm.from_mesh(mesh);bmesh.ops.recalc_face_normals(bm,faces=list(bm.faces));bm.to_mesh(mesh);bm.free();sole=bpy.data.objects.new('RoundedRubberSole',mesh);bpy.context.collection.objects.link(sole);sole.data.materials.append(rubber);sole.data.materials.append(edge)
for f in sole.data.polygons:f.material_index=0 if f.index==1 else 1
bpy.context.view_layer.objects.active=sole;sole.select_set(True);bevel=sole.modifiers.new('RoundedMouldedRubberEdge','BEVEL');bevel.width=.0025;bevel.segments=4;bpy.ops.object.modifier_apply(modifier=bevel.name)
for p in sole.data.polygons:p.use_smooth=True
wn=sole.modifiers.new('WeightedSurfaceNormals','WEIGHTED_NORMAL');bpy.ops.object.modifier_apply(modifier=wn.name);sole.select_set(False)
def tube(name,points,radius):
 c=bpy.data.curves.new(name,'CURVE');c.dimensions='3D';c.use_fill_caps=True;c.resolution_u=12;c.bevel_depth=radius;c.bevel_resolution=3;s=c.splines.new('BEZIER');s.bezier_points.add(len(points)-1)
 for p,co in zip(s.bezier_points,points):p.co=co;p.handle_left_type=p.handle_right_type='AUTO'
 o=bpy.data.objects.new(name,c);bpy.context.collection.objects.link(o);o.data.materials.append(strap);bpy.context.view_layer.objects.active=o;o.select_set(True);bpy.ops.object.convert(target='MESH');o.select_set(False);return o
for sign in [-1,1]:tube('FlexibleYStrap_'+str(sign),[(0,.070,.028),(sign*.017,.052,.045),(sign*.032,.020,.040),(sign*.041,-.017,.016)],.0055)
tube('ToePost',[(0,.070,.011),(0,.070,.028)],.0032)
models=[o for o in bpy.context.scene.objects if o.type=='MESH']
bpy.ops.object.select_all(action='DESELECT')
for o in models:o.select_set(True)
bpy.context.view_layer.objects.active=sole
bpy.ops.wm.save_as_mainfile(filepath=str(SOURCE))
bpy.ops.export_scene.fbx(filepath=str(MODEL),use_selection=True,object_types={'MESH'},add_leaf_bones=False,bake_anim=False,axis_forward='-Z',axis_up='Y')
# Preview from a walking-height inspection angle.
scene=bpy.context.scene;scene.render.engine='CYCLES';scene.cycles.samples=32;scene.cycles.use_denoising=False
scene.world.color=(.22,.22,.22)
for loc,power,size in [((-.4,-.3,.8),3,.5),((.4,.3,.6),2,.4)]:
 bpy.ops.object.light_add(type='AREA',location=loc);o=bpy.context.object;o.data.energy=power;o.data.shape='DISK';o.data.size=size;o.rotation_euler=(Vector((0,0,.01))-o.location).to_track_quat('-Z','Y').to_euler()
bpy.ops.object.camera_add(location=(.29,-.36,.39));cam=bpy.context.object;cam.rotation_euler=(Vector((0,0,.02))-cam.location).to_track_quat('-Z','Y').to_euler();cam.data.type='ORTHO';cam.data.ortho_scale=.38;scene.camera=cam
scene.render.resolution_x=700;scene.render.resolution_y=700;scene.render.resolution_percentage=100;scene.render.filepath=str(PREVIEW);scene.render.film_transparent=True;bpy.ops.render.render(write_still=True)
