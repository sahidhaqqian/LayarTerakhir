#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Audio;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class GoldenSceneValidation
    {
        [Serializable] private sealed class Record
        {
            public string utc,scene;
            public int failures,triangles,renderers,reachableGridCells,textureMaximum,audioClips;
            public long sourcePcmBytes;
            public string[] checks;
        }
        public static void Run(){EditorSceneManager.OpenScene(PrologKosSceneBuilder.ScenePath);Validate();}
        public static void Validate()
        {
            var baseReport=KosSceneValidation.Validate();
            var checks=new System.Collections.Generic.List<string>();int failures=0;
            void Check(bool value,string name){checks.Add((value?"PASS ":"FAIL ")+name);if(!value){failures++;Debug.LogError("GOLDEN_VALIDATION "+name);}}
            var rig=Object.FindFirstObjectByType<KosSceneAudio>();
            Check(rig&&rig.profile&&rig.player,"Scene audio profile and player bindings");
            var sources=rig.GetComponentsInChildren<AudioSource>();
            Check(sources.Length==9&&sources.All(s=>s.outputAudioMixerGroup&&!s.playOnAwake&&s.dopplerLevel==0),"All nine audio sources routed; no Doppler/autoplay");
            Check(rig.room&&rig.roof&&rig.computer&&new[]{rig.room,rig.roof,rig.computer}.All(l=>l.gain>0&&l.gain<.8f&&l.GetComponent<AudioSource>().clip),"Three real ambient loops with bounded gains and fades");
            var clips=Directory.GetFiles("Assets/_LayarTerakhir/Audio/Prepared","*.wav").Select(AssetDatabase.LoadAssetAtPath<AudioClip>).ToArray();
            Check(clips.Length==15&&clips.All(c=>c&&c.length>0&&c.channels==1&&c.frequency==32000),"Fifteen valid mono 32k clips");
            Check(Object.FindFirstObjectByType<KamarKosPrologDirector>().environmentAudio==rig,"Existing story director invokes audio rig");
            var hud=Object.FindFirstObjectByType<HudController>();var p=hud.GetComponent<HudPresentation>();
            Check(Object.FindObjectsByType<HudController>(FindObjectsSortMode.None).Length==1,"Exactly one HUD controller");
            Check(p&&p.style&&p.style.bodyFont&&p.style.emphasisFont&&p.digitDisplays.Length==3&&p.digitDisplays.All(t=>t),"HUD style, bundled fonts and three padlock digits");
            Check(p.documentScroll&&p.documentScroll.content==hud.documentBodyText.rectTransform&&p.documentScroll.viewport.GetComponent<RectMask2D>(),"Long documents scroll inside a clipped viewport");
            Check(hud.GetComponentInChildren<CanvasScaler>().referenceResolution==new Vector2(1600,900),"Resolution-scaled shared HUD prefab");
            Check(!hud.documentPanel.activeSelf&&!hud.padlockPanel.activeSelf&&hud.blackFadeImage.color.a==0,"No modal/debug presentation serialized open");
            var stool=GameObject.Find("BedsideWornStool");var b=KosVisualV2.BoundsOf(stool);
            Check(Mathf.Abs(b.min.y-KosVisualV2.FloorY)<.005f&&stool.GetComponent<BoxCollider>().bounds.Contains(b.center),"New stool grounded with fitted collision");
            Check(b.max.x<KosVisualV2.BoundsOf(GameObject.Find("BedsideMat")).max.x&&b.max.z<-.99f,"Bedside stool stays away from room centre and phone route");
            var rug=GameObject.Find("BedsideMat");
            float up=rug.GetComponentsInChildren<MeshFilter>().SelectMany(f=>f.sharedMesh.normals.Select(n=>f.transform.TransformDirection(n).y)).Average();
            Check(up>.9f,"Bed mat actual imported surface normals face the ceiling, not the floor");
            foreach(string path in new[]{GoldenHudBuilder.PrefabPath,"Assets/_LayarTerakhir/Production/Prefabs/KosEnvironmentalAudio.prefab","Assets/_LayarTerakhir/Production/Prefabs/GoldenPracticalLighting.prefab"})
                Check(AssetDatabase.LoadAssetAtPath<GameObject>(path),"Reusable prefab "+Path.GetFileName(path));
            var report=new Record{utc=DateTime.UtcNow.ToString("o"),scene=baseReport.scene,failures=failures,triangles=baseReport.triangles,renderers=baseReport.renderers,
                reachableGridCells=baseReport.reachableGridCells,textureMaximum=baseReport.textureMaximum,audioClips=clips.Length,
                sourcePcmBytes=clips.Sum(c=>(long)c.samples*c.channels*2),checks=checks.ToArray()};
            File.WriteAllText("Audit/golden-scene-validation.json",JsonUtility.ToJson(report,true)+"\n");
            if(failures>0)throw new Exception("Golden validation failed: "+failures);
            Debug.Log("GOLDEN_VALIDATION PASS extraChecks="+checks.Count+" baseChecks="+baseReport.checks.Length);
        }
    }
    namespace Tests
    {
        public sealed class GoldenSceneTests
        {
            [Test] public void GoldenSceneKeepsReachableGameplayAndWiredProductionAssets()=>GoldenSceneValidation.Run();
        }
    }
}
#endif
