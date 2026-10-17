#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using LayarTerakhir.Prolog;
namespace LayarTerakhir.Editor
{
    // Diagnostic: walks the intended ground->upper stair route and reports capsule blockers by name.
    public static class AtmajaStairProbe
    {
        public static void Run()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();
            var cc=d.player.GetComponent<CharacterController>();
            var report=new List<string>();
            report.Add($"CC radius={cc.radius} height={cc.height} stepOffset={cc.stepOffset} slopeLimit={cc.slopeLimit}");
            report.Add("leafCount="+Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,FindObjectsSortMode.None).Count(t=>t.name=="LockedCentralStairDoor"));
            string Blockers(Vector3 point,float height)
            {
                var hits=Physics.OverlapCapsule(point+Vector3.up*cc.radius,point+Vector3.up*(height-cc.radius),cc.radius,~0,QueryTriggerInteraction.Ignore)
                    .Where(c=>c!=cc&&!c.transform.IsChildOf(d.player.transform)).ToArray();
                return hits.Length==0?"clear":string.Join(" | ",hits.Select(c=>c.name+"@"+c.bounds.center.ToString("F2")));
            }
            void Sample(string label,Vector3 p){report.Add($"{label} world=({p.x:F2},{p.y:F2},{p.z:F2}) stand=[{Blockers(p,d.player.standingHeight)}]");}
            var leaf=d.stairDoorLeaf;
            if(leaf)
            {
                var col=leaf.GetComponent<Collider>();
                report.Add($"leaf BEFORE pos={leaf.transform.position.ToString("F2")} collider={col!=null&&col.enabled} bounds={leaf.GetComponent<Renderer>().bounds}");
            }
            else report.Add("leaf=NULL");
            for(int i=0;i<=8;i++){float dep=5.4f+i*.1f;Sample("cross-closed-"+i,d.P(-2.4f,dep,.55f));}
            if(leaf)
            {
                leaf.transform.position=d.P(-3.55f,7.7f,-0.85f);
                var col=leaf.GetComponent<Collider>();if(col)col.enabled=false;
                Physics.SyncTransforms();
                report.Add($"leaf AFTER pos={leaf.transform.position.ToString("F2")} collider={leaf.GetComponent<Collider>()!=null&&leaf.GetComponent<Collider>().enabled}");
            }
            for(int i=0;i<=8;i++){float dep=5.4f+i*.1f;Sample("cross-open-"+i,d.P(-2.4f,dep,.55f));}
            Sample("chamber-1",d.P(-2.4f,6.1f,.55f));
            Sample("chamber-2",d.P(-3.5f,6.4f,.56f));
            Sample("hall-approach",d.P(-3.5f,6.6f,.53f));
            Sample("entry-platform",d.P(-3.5f,7.5f,.62f));
            for(int i=0;i<=8;i++){float t=i/8f;Sample("flight1-"+i,d.P(Mathf.Lerp(-3.0f,.2f,t),7.45f,Mathf.Lerp(.78f,1.98f,t)));}
            Sample("mid-landing",d.P(.6f,7.35f,2.08f));
            for(int i=0;i<=9;i++){float t=i/9f;Sample("flight2-"+i,d.P(Mathf.Lerp(.8f,-3.0f,t),7.0f,Mathf.Lerp(2.26f,3.80f,t)));}
            Sample("top-landing",d.P(-3.25f,6.45f,3.88f));
            // Dynamic replication of the capsule walk across the entry platform onto flight one.
            Physics.autoSimulation=false;
            try
            {
                var pc=d.player;pc.transform.SetPositionAndRotation(d.P(-3.5f,7.5f,.62f),Quaternion.identity);
                var pcc=pc.GetComponent<CharacterController>();pcc.enabled=true;Physics.SyncTransforms();
                var target=d.P(-2.7f,7.45f,.8f);
                for(int f=0;f<60;f++)
                {
                    var delta=target-pc.transform.position;delta.y=0;
                    var flags=pcc.Move(Vector3.ClampMagnitude(delta,.16f)+Vector3.down*(f%7==6?0f:.18f));
                    Physics.Simulate(.02f);Physics.SyncTransforms();
                    if(f%5==0||f==59)
                    {
                        var hits=Physics.OverlapSphere(pc.transform.position+Vector3.up*.8f,.9f,~0,QueryTriggerInteraction.Collide).Where(c=>c!=pcc&&!c.transform.IsChildOf(pc.transform)).Select(c=>c.name+"@"+c.bounds.center.ToString("F2"));
                        report.Add($"dyn-{f} pos={pc.transform.position.ToString("F3")} flags={flags} hits=[{string.Join(" | ",hits)}]");
                    }
                }
            }
            finally{Physics.autoSimulation=true;}
            System.IO.File.WriteAllLines("/root/Projects/LayarTerakhir/Audit/stair-probe.txt",report);
            Debug.Log("STAIR_PROBE lines="+report.Count);
        }
    }
}
#endif
