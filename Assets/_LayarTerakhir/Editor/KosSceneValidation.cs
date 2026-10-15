#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class KosSceneValidation
    {
        public static readonly string[] FloorModels = { "vintage_day_bed", "wooden_table_02", "painted_wooden_chair_01",
            "bookcaseClosedDoors", "painted_wooden_cabinet_02", "OldPCTower", "RefillWaterGallon",
            "plastic_bottle_gallon", "trashcan", "cardboardBoxOpen", "plastic_container", "wicker_basket_02", "KosSandals", "KosBackpack" };
        [Serializable] public class Check { public string subject, kind, detail; public bool passed; public float measured; }
        [Serializable] public class Report
        {
            public string scene, utc; public int failures, triangles, renderers, textureMaximum, reachableGridCells;
            public Vector3 roomDimensions; public Check[] checks;
        }
        private static void Add(List<Check> checks,string subject,string kind,bool ok,float measured,string detail)
            => checks.Add(new Check {subject=subject,kind=kind,passed=ok,measured=measured,detail=detail});

        public static void Run()
        {
            EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
            Validate();
        }
        public static Report Validate(bool throwOnFailure=true)
        {
            Physics.SyncTransforms();
            var checks=new List<Check>();
            var floor=GameObject.Find("Floor_4x3m").GetComponent<Collider>().bounds;
            float floorY=KosVisualV2.FloorY;
            var report=new Report { scene=UnityEngine.SceneManagement.SceneManager.GetActiveScene().path,utc=DateTime.UtcNow.ToString("o"),
                roomDimensions=new Vector3(floor.size.x,GameObject.Find("Ceiling_4x3m").GetComponent<Collider>().bounds.min.y-floor.max.y,floor.size.z)};
            Add(checks,"Room","dimensions",Mathf.Abs(floor.size.x-6)<.001f&&Mathf.Abs(floor.size.z-4.5f)<.001f,floor.size.x*floor.size.z,"Actual floor collider: "+floor.size);
            var grounded=new List<GameObject>();
            foreach(var name in FloorModels)
            {
                var go=GameObject.Find(name);
                if(!go){Add(checks,name,"model",false,0,"Missing expected furnished object");continue;}
                grounded.Add(go);var b=KosVisualV2.BoundsOf(go);float gap=b.min.y-floorY;
                Add(checks,name,"floor-contact",gap>=-.008f&&gap<=.012f,gap,"Transformed renderer minimum minus actual tile surface (metres)");
                Add(checks,name,"room-containment",b.min.x>=floor.min.x+.025f&&b.max.x<=floor.max.x-.025f&&b.min.z>=floor.min.z+.025f&&b.max.z<=floor.max.z-.025f,0,"Actual mesh bounds "+b);
            }
            // Footprints of floor-standing furniture; a computer tower deliberately underneath its desk is tested against the legs separately.
            for(int a=0;a<grounded.Count;a++)for(int b=a+1;b<grounded.Count;b++)
            {
                var first=grounded[a];var second=grounded[b];
                if((first.name=="wooden_table_02"&&second.name=="OldPCTower")||(second.name=="wooden_table_02"&&first.name=="OldPCTower"))continue;
                var aa=KosVisualV2.BoundsOf(first);var bb=KosVisualV2.BoundsOf(second);
                float ox=Mathf.Min(aa.max.x,bb.max.x)-Mathf.Max(aa.min.x,bb.min.x);
                float oz=Mathf.Min(aa.max.z,bb.max.z)-Mathf.Max(aa.min.z,bb.min.z);
                if(ox>.005f&&oz>.005f)Add(checks,first.name+" / "+second.name,"major-intersection",ox<.025f||oz<.025f,Mathf.Min(ox,oz),"Actual world-space footprints overlap; allowance is 25mm");
            }
            var pc=GameObject.Find("OldPCTower");
            var chair=GameObject.Find("painted_wooden_chair_01");var chairBounds=KosVisualV2.BoundsOf(chair);
            var high=KosVisualV2.MeshVertices(chair).Where(v=>v.y>chairBounds.min.y+.65f).ToArray();
            var back=high.Length>0?new Vector3(high.Average(v=>v.x),0,high.Average(v=>v.z))-new Vector3(chairBounds.center.x,0,chairBounds.center.z):Vector3.zero;
            var towardDesk=KosVisualV2.BoundsOf(GameObject.Find("wooden_table_02")).center-chairBounds.center;towardDesk.y=0;
            Add(checks,"DeskChair","backrest-direction",high.Length>0&&Vector3.Dot(back.normalized,towardDesk.normalized)<-.2f,Vector3.Dot(back.normalized,towardDesk.normalized),"Upper mesh vertices must put the backrest on the side away from the desk");
            float seat=KosVisualV2.SurfaceHeight(chair,chairBounds.center.x,chairBounds.center.z)-floorY;
            Add(checks,"DeskChair","seat-height",seat>.35f&&seat<.55f,seat,"Actual mesh surface at the seat centre; detects toppled/stretched chair orientation");
            foreach(string leg in new[]{"KakiMejaKiri","KakiMejaKanan"})
                Add(checks,"PC / "+leg,"desk-leg-clearance",!KosVisualV2.BoundsOf(pc).Intersects(GameObject.Find(leg).GetComponent<Collider>().bounds),0,"Compare tower mesh bounds with actual desk-leg collider");
            foreach(string name in new[]{"computerKeyboard","computerMouse","binder_notebook","EnamelMug","desk_lamp_arm_01","StickyNote"})
                CheckSurface(checks,GameObject.Find(name),GameObject.Find("wooden_table_02"));
            CheckSurface(checks,GameObject.Find("Smartphone"),GameObject.Find("vintage_day_bed"));
            CheckSurface(checks,GameObject.Find("KosDeskFan"),GameObject.Find("FanWallShelf"));
            CheckSurface(checks,GameObject.Find("KosRouter"),GameObject.Find("painted_wooden_cabinet_02"));
            GameObject bookSupport=GameObject.Find("painted_wooden_cabinet_02");
            foreach(var t in GameObject.Find("Kos_Environment_Art").transform.Cast<Transform>().Where(t=>t.name=="books").OrderBy(t=>KosVisualV2.BoundsOf(t.gameObject).min.y))
            {
                if(KosVisualV2.BoundsOf(t.gameObject).center.x<0)CheckSurface(checks,t.gameObject,GameObject.Find("wooden_bookshelf_worn"));
                else { CheckSurface(checks,t.gameObject,bookSupport); bookSupport=t.gameObject; }
            }

            var art=GameObject.Find("Kos_Environment_Art");
            foreach(var filter in art.GetComponentsInChildren<MeshFilter>())
            {
                var renderer=filter.GetComponent<Renderer>();if(!renderer||!renderer.enabled)continue;
                Add(checks,filter.name,"mesh-reference",filter.sharedMesh,0,"Visible MeshFilter must reference an imported/generated mesh");
                if(filter.sharedMesh)for(int s=0;s<filter.sharedMesh.subMeshCount;s++)report.triangles+=(int)filter.sharedMesh.GetIndexCount(s)/3;
            }
            foreach(var renderer in art.GetComponentsInChildren<Renderer>().Where(r=>r.enabled))
            {
                report.renderers++;
                foreach(var mat in renderer.sharedMaterials)
                {
                    Add(checks,renderer.name,"material-reference",mat&&mat.shader&&mat.shader.name!="Hidden/InternalErrorShader",0,mat?mat.name:"Missing material");
                    if(mat)foreach(string property in mat.GetTexturePropertyNames())
                    {
                        var texture=mat.GetTexture(property);if(!texture)continue;
                        report.textureMaximum=Mathf.Max(report.textureMaximum,texture.width,texture.height);
                    }
                }
                var t=renderer.transform;
                var scale=t.lossyScale;
                Add(checks,t.name,"finite-transform",Finite(t.position)&&Finite(t.eulerAngles)&&Finite(scale)&&Mathf.Min(scale.x,scale.y,scale.z)>0,scale.magnitude,"Actual world position/rotation/scale must be finite and scale positive");
            }
            foreach(var go in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects())
            foreach(var t in go.GetComponentsInChildren<Transform>(true))
                Add(checks,t.name,"missing-scripts",GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)==0,0,"Serialized component references");

            var player=Object.FindFirstObjectByType<FirstPersonController>();
            var controller=player.GetComponent<CharacterController>();
            var grid=ReachableGrid(floor,controller,player.transform.position,floorY);
            report.reachableGridCells=grid.Count;
            Add(checks,"Player","pathway",grid.Count>30,grid.Count,"Flood-fill actual capsule-clear floor cells from player spawn; 25cm grid");
            var interactor=player.GetComponent<InteractionInteractor>();
            foreach(var item in Object.FindObjectsByType<KosInteractableItem>(FindObjectsSortMode.None))
            {
                var collider=item.GetComponent<Collider>();
                Add(checks,item.itemId,"interaction-references",collider&&item.director,0,"Existing item ID, collider and director binding");
                if(!collider)continue;
                var visual=item.GetComponentsInChildren<Renderer>().Where(r=>r.enabled).ToArray();
                if(visual.Length>0)
                {
                    var b=visual[0].bounds;foreach(var r in visual.Skip(1))b.Encapsulate(r.bounds);
                    var expanded=collider.bounds;expanded.Expand(.055f);
                    Add(checks,item.itemId,"raycast-geometry-alignment",expanded.Contains(b.center),Vector3.Distance(collider.bounds.center,b.center),"Visible geometry centre must fall within the actual interaction collider (+27.5mm)");
                }
                int views=0;float nearest=float.PositiveInfinity;
                foreach(var p in grid)
                {
                    var eye=p+Vector3.up*player.cameraStandingY;
                    var delta=collider.bounds.center-eye;float distance=delta.magnitude;
                    if(distance>interactor.interactionDistance)continue;
                    var hits=Physics.RaycastAll(eye,delta.normalized,interactor.interactionDistance,interactor.interactableMask,QueryTriggerInteraction.Collide)
                        .Where(h=>!h.collider.transform.IsChildOf(player.transform)).OrderBy(h=>h.distance).ToArray();
                    if(hits.Length>0&&hits[0].collider.GetComponentInParent<KosInteractableItem>()==item){views++;nearest=Mathf.Min(nearest,distance);}
                }
                Add(checks,item.itemId,"reachable-interaction",views>0,views,"Unobstructed first raycast hit from reachable standing-player cells; closest eye distance "+nearest);
            }
            // Test actual apron regions for door approach, bed access, desk access and wardrobe access.
            foreach(string name in new[]{"KosDoor","KasurKos","MejaKerjaPC","LemariPakaian"})
            {
                var target=GameObject.Find(name);
                if(!target && name=="KosDoor")target=Object.FindFirstObjectByType<KamarKosPrologDirector>().kosDoorItem.gameObject;
                var b=target.GetComponent<Collider>().bounds;
                float nearest=grid.Min(p=>Vector2.Distance(new Vector2(p.x,p.z),new Vector2(b.center.x,b.center.z)));
                Add(checks,name,"approach-clearance",nearest<1.1f,nearest,"Nearest capsule-clear reachable floor cell to actual collider centre");
            }
            var door=Object.FindFirstObjectByType<KamarKosPrologDirector>().kosDoorItem.GetComponent<Collider>().bounds;
            var left=GameObject.Find("SouthLeftLimewash").GetComponent<Renderer>().bounds;
            var right=GameObject.Find("SouthRightLimewash").GetComponent<Renderer>().bounds;
            Add(checks,"DoorOpening","wall-clearance",left.max.x<=door.min.x+.02f&&right.min.x>=door.max.x-.02f,right.min.x-left.max.x,"Measure opening between actual wall meshes against door interaction bounds");
            var lintel=GameObject.Find("DoorLintelPlaster").GetComponent<Renderer>().bounds;
            Add(checks,"DoorLintel","gap-free-join",lintel.min.x<=left.max.x+.005f&&lintel.max.x>=right.min.x-.005f,lintel.size.x,"Actual lintel must bridge both jambs without open vertical slits");
            Add(checks,"Art","triangle-budget",report.triangles<=160000,report.triangles,"Visible scene meshes, actual submesh index counts");
            Add(checks,"Art","texture-budget",report.textureMaximum<=1024,report.textureMaximum,"Actual bound texture dimensions");
            report.checks=checks.ToArray();report.failures=checks.Count(c=>!c.passed);
            File.WriteAllText("Audit/kos-v2-scene-validation.json",JsonUtility.ToJson(report,true)+"\n");
            foreach(var failure in checks.Where(c=>!c.passed))Debug.LogWarning("KOS_V2_VALIDATION "+JsonUtility.ToJson(failure));
            Debug.Log("KOS_V2_VALIDATION checks="+checks.Count+" failures="+report.failures+" triangles="+report.triangles+" renderers="+report.renderers);
            if(throwOnFailure&&report.failures>0)throw new Exception("Kos scene validation failed; inspect Audit/kos-v2-scene-validation.json");
            return report;
        }
        private static bool Finite(Vector3 v)=>float.IsFinite(v.x)&&float.IsFinite(v.y)&&float.IsFinite(v.z);
        private static void CheckSurface(List<Check> checks,GameObject prop,GameObject support)
        {
            var b=KosVisualV2.BoundsOf(prop);
            try
            {
                float surface=KosVisualV2.SurfaceHeight(support,b.center.x,b.center.z);float gap=b.min.y-surface;
                Add(checks,prop.name,"surface-contact",gap>=-.008f&&gap<=.012f,gap,"Actual transformed triangle under prop on "+support.name);
            }
            catch(Exception e){Add(checks,prop.name,"surface-contact",false,0,e.Message);}
        }
        private static List<Vector3> ReachableGrid(Bounds floor,CharacterController controller,Vector3 spawn,float floorY)
        {
            const float step=.25f;float radius=controller.radius;float height=controller.height;
            var free=new Dictionary<Vector2Int,Vector3>();
            int w=Mathf.CeilToInt(floor.size.x/step);int d=Mathf.CeilToInt(floor.size.z/step);
            for(int x=0;x<w;x++)for(int z=0;z<d;z++)
            {
                var p=new Vector3(floor.min.x+step*(x+.5f),floorY+.025f,floor.min.z+step*(z+.5f));
                var overlaps=Physics.OverlapCapsule(p+Vector3.up*radius,p+Vector3.up*(height-radius),radius,~0,QueryTriggerInteraction.Ignore);
                if(overlaps.All(c=>c!=controller&&!c.transform.IsChildOf(controller.transform)))free[new Vector2Int(x,z)]=p;
            }
            if(free.Count==0)return new List<Vector3>();
            var start=free.OrderBy(k=>Vector3.SqrMagnitude(k.Value-spawn)).First().Key;
            var visited=new HashSet<Vector2Int>{start};var queue=new Queue<Vector2Int>();queue.Enqueue(start);
            var directions=new[]{Vector2Int.up,Vector2Int.down,Vector2Int.left,Vector2Int.right};
            while(queue.Count>0)
            {
                var p=queue.Dequeue();
                foreach(var dir in directions)
                {
                    var n=p+dir;if(visited.Contains(n)||!free.ContainsKey(n))continue;
                    // A swept capsule also prevents squeezing diagonally through gaps smaller than the controller.
                    var delta=free[n]-free[p];
                    var hits=Physics.CapsuleCastAll(free[p]+Vector3.up*radius,free[p]+Vector3.up*(height-radius),radius,delta.normalized,delta.magnitude,~0,QueryTriggerInteraction.Ignore);
                    if(hits.Any(h=>h.collider!=controller&&!h.collider.transform.IsChildOf(controller.transform)))continue;
                    visited.Add(n);queue.Enqueue(n);
                }
            }
            return visited.Select(p=>free[p]).ToList();
        }
    }
    namespace Tests
    {
        public class KosVisualV2Tests
        {
            [Test] public void FurnishedSceneHasGroundContactClearRoutesAndReachableInteractions()
            {
                EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
                var report=KosSceneValidation.Validate(false);
                Assert.That(report.failures,Is.Zero,string.Join("\n",report.checks.Where(c=>!c.passed).Select(c=>c.subject+": "+c.kind+" "+c.detail)));
            }
            [Test] public void PlacementValidationDetectsActualFloatingFurniture()
            {
                EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
                var chair=GameObject.Find("painted_wooden_chair_01");chair.transform.position+=Vector3.up*.15f;
                var report=KosSceneValidation.Validate(false);
                Assert.That(report.checks.Any(c=>c.subject==chair.name&&c.kind=="floor-contact"&&!c.passed),Is.True);
                EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);
                KosSceneValidation.Validate();
            }
        }
    }
}
#endif
