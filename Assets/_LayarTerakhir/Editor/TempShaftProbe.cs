#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using LayarTerakhir.Prolog;
namespace LayarTerakhir.Editor
{
    public static class TempShaftProbe
    {
        public static void Run()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();
            var root=GameObject.Find("Atmaja_Environment_Art");
            var cc=d.player.GetComponent<CharacterController>();
            var report=new List<string>();
            report.Add("CC radius="+cc.radius+" height="+cc.height);
            foreach(var t in root.GetComponentsInChildren<Transform>(true))
            {
                var local=d.LocalPosition(t.position);
                if(local.x>-3.5f&&local.x<4.5f&&local.y>-1f&&local.y<7.6f&&t.position.y<3.9f)
                {
                    var r=t.GetComponent<Renderer>();var size=r?r.bounds.size.ToString():(t.GetComponent<Collider>()?((Collider)t.GetComponent<Collider>()).bounds.size.ToString():"-");
                    report.Add($"GF local=({local.x:F2},{local.y:F2}) worldY={t.position.y:F2} size={size} {t.name}");
                }
            }
            bool Clear(Vector3 point,float height)
            {var blocked=Physics.OverlapCapsule(point+Vector3.up*cc.radius,point+Vector3.up*(height-cc.radius),cc.radius,~0,QueryTriggerInteraction.Ignore).Where(c=>c!=cc&&!c.transform.IsChildOf(d.player.transform)).ToArray();return blocked.Length==0;}
            for(float depth=.4f;depth<7.2f;depth+=.4f)
            {
                var line="depth "+depth.ToString("F1")+" x:";
                for(float x=2.4f;x>-3.5f;x-=.4f)line+=Clear(d.P(x,depth,.54f),d.player.standingHeight)?".":"X";
                report.Add(line);
            }
            for(float depth=6.6f;depth<7.6f;depth+=.2f)
            {
                var line="depth "+depth.ToString("F1")+" x:";
                for(float x=2.4f;x>-3.5f;x-=.4f)line+=Clear(d.P(x,depth,3.9f),d.player.standingHeight)?".":"X";
                report.Add(line);
            }
            System.IO.File.WriteAllLines("/root/Projects/LayarTerakhir/Audit/shaft-probe.txt",report);
            Debug.Log("SHAFT_PROBE lines="+report.Count);
        }
    }
}
#endif
