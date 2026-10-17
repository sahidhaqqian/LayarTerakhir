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
            string Blockers(Vector3 point,float height)
            {
                var hits=Physics.OverlapCapsule(point+Vector3.up*cc.radius,point+Vector3.up*(height-cc.radius),cc.radius,~0,QueryTriggerInteraction.Ignore)
                    .Where(c=>c!=cc&&!c.transform.IsChildOf(d.player.transform)).ToArray();
                return hits.Length==0?"clear":string.Join(" | ",hits.Select(c=>c.name+"@"+c.bounds.center.ToString("F2")));
            }
            void Sample(string label,Vector3 p){report.Add($"{label} world=({p.x:F2},{p.y:F2},{p.z:F2}) stand=[{Blockers(p,d.player.standingHeight)}]");}
            Sample("hall-approach",d.P(-3.5f,6.6f,.53f));
            Sample("entry-platform",d.P(-3.5f,7.5f,.62f));
            for(int i=0;i<=8;i++){float t=i/8f;Sample("flight1-"+i,d.P(Mathf.Lerp(-3.0f,.2f,t),7.5f,Mathf.Lerp(.78f,1.98f,t)));}
            Sample("mid-landing",d.P(.6f,7.1f,2.08f));
            for(int i=0;i<=9;i++){float t=i/9f;Sample("flight2-"+i,d.P(Mathf.Lerp(.8f,-3.0f,t),6.7f,Mathf.Lerp(2.26f,3.80f,t)));}
            Sample("top-landing",d.P(-3.25f,6.45f,3.88f));
            System.IO.File.WriteAllLines("/root/Projects/LayarTerakhir/Audit/stair-probe.txt",report);
            Debug.Log("STAIR_PROBE lines="+report.Count);
        }
    }
}
#endif
