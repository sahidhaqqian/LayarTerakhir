"""Prolog-only mesh recovery from the supplied Nyonya VRM; shared original remains intact.
Blender -b -t 2 --python-exit-code 1 --python this_file.py
Retain current rest skeleton/actions. Recover original supplied VRM surface,
weld coincident positions before conservative decimation; interpolate repaired
continuous source weights. Never run original project asset-generation scripts.
"""
import bpy,json,numpy as np,time,math,hashlib
from pathlib import Path
from mathutils import Matrix,Vector
from mathutils.kdtree import KDTree
ROOT=Path(__file__).resolve().parents[1]
P=ROOT/'Builds/QA/PrologSurfaceRepair';P.mkdir(parents=True,exist_ok=True)
CURRENT=ROOT/'Assets/_LayarTerakhir/assets/Art/DesignerCharacters/Models/NyonyaDesigner.fbx'
ORIGINAL=ROOT/'Assets/_LayarTerakhir/assets/LayarTerakhir/Nyonya.vrm'
OUT=ROOT/'Assets/_LayarTerakhir/assets/Art/Original/Models/Prolog47/NyonyaPrologSurfaceRepair.fbx';started=time.monotonic();report={}
def note(step,**values):print('NYONYA_PROLOG',step,json.dumps(values,default=float),'elapsed',round(time.monotonic()-started,2),flush=True)
def digest(path):
 with path.open('rb') as f:return hashlib.file_digest(f,'sha256').hexdigest()
def action_signature(action):
 rows=[]
 for c in action.fcurves:
  rows.append((c.data_path,c.array_index,[(tuple(k.co),tuple(k.handle_left),tuple(k.handle_right),k.interpolation) for k in c.keyframe_points]))
 return hashlib.sha256(repr(rows).encode()).hexdigest()
def points(obj):
 ev=obj.evaluated_get(bpy.context.evaluated_depsgraph_get());me=ev.to_mesh();co=np.empty(len(me.vertices)*3);me.vertices.foreach_get('co',co);co=co.reshape((-1,3));m=np.asarray(ev.matrix_world);co=co@m[:3,:3].T+m[:3,3];ev.to_mesh_clear();return co
bpy.ops.wm.read_factory_settings(use_empty=True);bpy.ops.import_scene.fbx(filepath=str(CURRENT))
rig=next(o for o in bpy.context.scene.objects if o.type=='ARMATURE');old=next(o for o in bpy.context.scene.objects if o.type=='MESH');old.animation_data_clear()
actions=list(bpy.data.actions);fps=bpy.context.scene.render.fps;fpsbase=bpy.context.scene.render.fps_base
original_signatures={a.name.rsplit('|',1)[-1]:action_signature(a) for a in actions}
rig.animation_data.action=None
for b in rig.pose.bones:b.matrix_basis=Matrix.Identity(4)
bpy.context.view_layer.update();rest=points(old);oldmatrix=old.matrix_world.copy();oldname=old.name
report['input']=dict(model=str(CURRENT),modelSHA256=digest(CURRENT),original=str(ORIGINAL),originalSHA256=digest(ORIGINAL),currentVertices=len(old.data.vertices),currentPolygons=len(old.data.polygons),fps=fps,fpsBase=fpsbase,bones=len(rig.data.bones),actionSignatures=original_signatures)
# Repair automated discontinuities without touching source coordinates or clips.
group_names=[g.name for g in old.vertex_groups];weights=np.zeros((len(rest),len(group_names)))
for v in old.data.vertices:
 for g in v.groups:weights[v.index,g.group]=g.weight
kd=KDTree(len(rest))
for i,p in enumerate(rest):kd.insert(p,i)
kd.balance();kernel=[]
for p in rest:
 near=sorted(kd.find_range(p,.045),key=lambda n:n[2])[:96];ids=np.asarray([n[1] for n in near]);d=np.asarray([n[2] for n in near]);f=np.exp(-.5*(d/.018)**2);f/=f.sum();kernel.append((ids,f))
for repeat in range(2):
 result=np.zeros_like(weights)
 for i,(ids,f) in enumerate(kernel):result[i]=f@weights[ids]
 weights=result
# Complete only existing majority-shears islands, never a coordinate slice.
adj=[[] for _ in rest]
for e in old.data.edges:
 a,b=e.vertices;adj[a].append(b);adj[b].append(a)
remaining=set(range(len(rest)));weapon=set();shears=group_names.index('Shears')
while remaining:
 first=next(iter(remaining));component=[first];pending=[first];remaining.remove(first)
 while pending:
  a=pending.pop()
  for b in adj[a]:
   if b in remaining:remaining.remove(b);component.append(b);pending.append(b)
 held=sum(any(g.group==shears and g.weight>.5 for g in old.data.vertices[i].groups) for i in component)
 if held and held/len(component)>=.35:
  for i in component:weapon.add(i);weights[i]=0;weights[i,shears]=1
note('source_weights_ready',rigidWeaponVertices=len(weapon))
existing=set(bpy.context.scene.objects);bpy.ops.import_scene.gltf(filepath=str(ORIGINAL));imported=set(bpy.context.scene.objects)-existing
source_rig=next(o for o in imported if o.type=='ARMATURE');mesh=next(o for o in imported if o.type=='MESH' and any(m.type=='ARMATURE' for m in o.modifiers))
# Match the exact normalization used to author current Nyonya, retaining rest shape.
co=np.empty(len(mesh.data.vertices)*3);mesh.data.vertices.foreach_get('co',co);co=co.reshape((-1,3));m=np.asarray(mesh.matrix_world);world=co@m[:3,:3].T+m[:3,3]
low=float(world[:,2].min());height=float(world[:,2].max()-low);scale=1.64/height
feet=[(source_rig.matrix_world@source_rig.data.bones[n].head_local).x for n in ['leftFoot','rightFoot']];centre=sum(feet)/2
world[:,0]-=centre;world[:,2]-=low;world*=scale
inverse=np.asarray(oldmatrix.inverted());local=world@inverse[:3,:3].T+inverse[:3,3]
mesh.data.vertices.foreach_set('co',local.reshape(-1));mesh.parent=None;mesh.matrix_world=oldmatrix
for mod in list(mesh.modifiers):mesh.modifiers.remove(mod)
mesh.animation_data_clear()
report['surface']=dict(originalVertices=len(mesh.data.vertices),originalPolygons=len(mesh.data.polygons),normalizationScale=scale,normalizationCentreX=centre,normalizationLowZ=low)
# Discard importer widgets/rig only, leaving target rest skeleton and actions.
for obj in imported:
 if obj!=mesh:bpy.data.objects.remove(obj,do_unlink=True)
for action in list(bpy.data.actions):
 if action not in actions:bpy.data.actions.remove(action)
bpy.context.scene.render.fps=fps;bpy.context.scene.render.fps_base=fpsbase
bpy.ops.object.select_all(action='DESELECT');mesh.select_set(True);bpy.context.view_layer.objects.active=mesh
note('original_loaded',**report['surface'])
bpy.ops.object.mode_set(mode='EDIT');bpy.ops.mesh.select_all(action='SELECT');bpy.ops.mesh.remove_doubles(threshold=.00001);bpy.ops.object.mode_set(mode='OBJECT')
report['surface']['weldedVertices']=len(mesh.data.vertices);report['surface']['weldedPolygons']=len(mesh.data.polygons)
note('welded',vertices=len(mesh.data.vertices),polygons=len(mesh.data.polygons))
mod=mesh.modifiers.new('PrologConservativeSurfaceBudget','DECIMATE');mod.ratio=min(1,180000/len(mesh.data.polygons));mod.use_collapse_triangulate=True;bpy.ops.object.modifier_apply(modifier=mod.name);mesh.data.update()
report['surface']['outputVertices']=len(mesh.data.vertices);report['surface']['outputPolygons']=len(mesh.data.polygons)
note('decimated',vertices=len(mesh.data.vertices),polygons=len(mesh.data.polygons))
for poly in mesh.data.polygons:poly.use_smooth=True
# Interpolate smooth current-rig binding over the recovered original surface.
mesh.vertex_groups.clear()
for name in group_names:mesh.vertex_groups.new(name=name)
nearest=[];co=points(mesh)
for v,p in zip(mesh.data.vertices,co):
 near=kd.find_n(p,8);ids=np.asarray([n[1] for n in near]);d=np.asarray([n[2] for n in near]);nearest.append(float(d.min()));f=np.exp(-.5*(d/.02)**2);f/=f.sum();w=f@weights[ids]
 top=np.argsort(w)[-4:];vals=w[top];vals/=vals.sum()
 for ix,value in zip(top,vals):
  if value>.00001:mesh.vertex_groups[int(ix)].add([v.index],float(value),'REPLACE')
report['surface']['transferDistanceMetres']=dict(mean=float(np.mean(nearest)),p95=float(np.quantile(nearest,.95)),max=float(np.max(nearest)))
mesh.data.materials.clear()
for mat in old.data.materials:mesh.data.materials.append(mat)
# Maintain the existing node names and imported model transform contract.
old.name='ReferenceSkin_NotExported';mesh.name=oldname;mesh.data.name=old.data.name
mod=mesh.modifiers.new('CharacterSkin','ARMATURE');mod.object=rig;mesh.parent=old.parent;mesh.matrix_world=oldmatrix
for a in actions:
 assert action_signature(a)==original_signatures[a.name.rsplit('|',1)[-1]], 'Changed action channels'
 a.name=a.name.rsplit('|',1)[-1]
rig.animation_data.action=next(a for a in actions if a.name=='Idle');bpy.context.scene.frame_set(1);bpy.context.view_layer.update()
bpy.ops.object.select_all(action='DESELECT');rig.select_set(True);mesh.select_set(True);bpy.context.view_layer.objects.active=rig
note('exporting',transfer=report['surface']['transferDistanceMetres'])
bpy.ops.export_scene.fbx(filepath=str(OUT),use_selection=True,object_types={'MESH','ARMATURE','EMPTY'},axis_forward='-Z',axis_up='Y',bake_anim=True,bake_anim_use_all_actions=True,bake_anim_use_nla_strips=False,bake_anim_simplify_factor=0,add_leaf_bones=False,path_mode='STRIP')
report['output']=dict(path=str(OUT),sha256=digest(OUT),bytes=OUT.stat().st_size,buildSeconds=time.monotonic()-started,actions=[dict(name=a.name,frames=list(a.frame_range),channels=len(a.fcurves),signature=action_signature(a)) for a in actions],rigidWeaponVertices=len(weapon))
(P/'nyonya-prolog-surface-report.json').write_text(json.dumps(report,indent=2)+'\n')
# Isolated editable review source, not copied into the game.
old.hide_render=True;old.hide_viewport=True;bpy.ops.wm.save_as_mainfile(filepath=str(P/'NyonyaPrologSurfaceRepair.blend'))
note('complete',output=str(OUT),seconds=report['output']['buildSeconds'])
