#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using LayarTerakhir.Prolog;
namespace LayarTerakhir.Editor
{
    // Render fixed viewpoints of the Atmaja scene to PNGs for visual inspection.
    // Run WITHOUT -nographics (needs a real graphics device).
    public static class AtmajaViewCapture
    {
        public static void Run()
        {
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<AtmajaDirector>();
            var cam=Object.FindFirstObjectByType<Camera>();
            var go=new GameObject("DebugCaptureCam");var c=go.AddComponent<Camera>();
            c.CopyFrom(cam);c.enabled=false;
            var dir=Path.Combine(Directory.GetCurrentDirectory(),"Audit/Visuals/Atmaja-Debug");Directory.CreateDirectory(dir);
            var rt=new RenderTexture(960,540,24);c.targetTexture=rt;
            void Shot(string name,Vector3 pos,Vector3 look)
            {
                go.transform.position=pos;
                go.transform.rotation=Quaternion.LookRotation((look-pos).normalized,Vector3.up);
                c.Render();
                RenderTexture.active=rt;var tex=new Texture2D(960,540,TextureFormat.RGB24,false);tex.ReadPixels(new Rect(0,0,960,540),0,0);tex.Apply();RenderTexture.active=null;
                File.WriteAllBytes(Path.Combine(dir,name+".png"),tex.EncodeToPNG());Object.DestroyImmediate(tex);
                Debug.Log("CAPTURED "+name);
            }
            // Hall in front of the stair door.
            Shot("01-hall-to-stairdoor",d.P(-3.5f,5.9f,1.65f),d.P(-3.5f,7.7f,1.7f));
            Shot("02-door-inside",d.P(-3.5f,7.2f,1.65f),d.P(-1.5f,7.5f,1.9f));
            Shot("03-mid-landing",d.P(.2f,7.4f,3.3f),d.P(-2.5f,6.7f,3.9f));
            Shot("04-top-landing-down",d.P(-3.25f,6.4f,5.6f),d.P(1.0f,6.7f,2.0f));
            Shot("05-corridor",d.P(-3.0f,5.6f,5.6f),d.P(2.0f,5.6f,5.6f));
            Shot("06-corridor-stairwell",d.P(-1.0f,5.5f,4.6f),d.P(-1.0f,6.6f,1.0f));
            Shot("07-child-desk",d.P(-4.6f,.9f,5.2f),d.P(-5.0f,.62f,4.62f));
            Shot("08-hall-wide",d.P(-4.6f,5.0f,2.2f),d.P(-2.0f,7.4f,1.2f));
            c.targetTexture=null;Object.DestroyImmediate(go);Object.DestroyImmediate(rt);
        }
    }
}
#endif
