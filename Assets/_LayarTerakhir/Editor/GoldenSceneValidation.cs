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
            var bed=KosVisualV2.BoundsOf(GameObject.Find("vintage_day_bed"));
            Check(bed.SqrDistance(b.center)<.25f&&b.min.z>bed.max.z-.025f,"Bedside stool stays beside the bed head, clear of its footprint and phone route");
            var rug=GameObject.Find("BedsideMat");
            float up=rug.GetComponentsInChildren<MeshFilter>().SelectMany(f=>f.sharedMesh.normals.Select(n=>f.transform.TransformDirection(n).y)).Average();
            Check(up>.9f,"Bed mat actual imported surface normals face the ceiling, not the floor");
            var screen=GameObject.Find("StreamerGreenScreen");var screenBounds=KosVisualV2.BoundsOf(screen);
            var chair=KosVisualV2.BoundsOf(GameObject.Find("painted_wooden_chair_01"));
            Check(screenBounds.size.x<.60f&&screenBounds.max.x<-1.2f&&screenBounds.min.z>1.4f&&screen.GetComponentsInChildren<Collider>().Length==1,"Green screen is genuinely folded and stored beside the workstation, not across the bed view");
            var player=Object.FindFirstObjectByType<LayarTerakhir.Player.FirstPersonController>();
            var eye=player.GetComponent<LayarTerakhir.Player.InteractionInteractor>().playerCamera.transform.position;
            Check(!Physics.RaycastAll(eye,(bed.center-eye).normalized,Vector3.Distance(eye,bed.center),~0,QueryTriggerInteraction.Ignore).Any(h=>h.transform.IsChildOf(screen.transform)),"Stored backdrop does not intersect the opening camera-to-bed sightline");
            var webcam=GameObject.Find("BudgetStreamingWebcam");var cameraBounds=KosVisualV2.BoundsOf(webcam);
            var monitor=KosVisualV2.BoundsOf(Object.FindFirstObjectByType<KamarKosPrologDirector>().pcMonitorItem.gameObject);
            var lens=webcam.GetComponentsInChildren<Renderer>().Single(r=>r.name.Contains("Lens"));
            Check(cameraBounds.min.y>=monitor.max.y-.025f&&Vector3.Dot(lens.bounds.center-cameraBounds.center,chair.center-cameraBounds.center)>0,"Budget webcam clips to the monitor and still faces the seating area");
            foreach(var pair in new[]{("FoldedTshirtOnBed","vintage_day_bed"),("ReadingPadOnStool","BedsideWornStool"),("BedsidePlasticThermos","ReadingPadOnStool"),("WiredGamepadOnBed","vintage_day_bed")})
            {
                var prop=KosVisualV2.BoundsOf(GameObject.Find(pair.Item1));
                float gap=prop.min.y-KosVisualV2.SurfaceHeight(GameObject.Find(pair.Item2),prop.center.x,prop.center.z);
                Check(gap>=-.008f&&gap<=.012f,pair.Item1+" rests on the actual mesh surface, not in mid-air");
            }
            var news=GameObject.Find("OldNewspaperByDesk").GetComponentsInChildren<MeshFilter>().Single(f=>f.name=="Newsprint");
            float newsUp=news.sharedMesh.normals.Average(n=>news.transform.TransformDirection(n).y);
            Check(newsUp>.9f,"Decorative newsprint face normals point up toward the room lighting");
            foreach(string name in new[]{"OldNewspaperByDesk","DroppedSocksByBed"})
                Check(Mathf.Abs(KosVisualV2.BoundsOf(GameObject.Find(name)).min.y-KosVisualV2.FloorY)<.005f,name+" remains grounded");
            var shirt=KosVisualV2.BoundsOf(GameObject.Find("FoldedTshirtOnBed"));
            var gamepad=KosVisualV2.BoundsOf(GameObject.Find("WiredGamepadOnBed"));
            var phone=KosVisualV2.BoundsOf(GameObject.Find("Smartphone"));
            Check(!shirt.Intersects(gamepad)&&!shirt.Intersects(phone)&&!gamepad.Intersects(phone),"Bed clothing/controller/phone do not intersect");
            Check(baseReport.triangles<120000&&baseReport.renderers<=212&&Object.FindObjectsByType<Camera>(FindObjectsInactive.Include,FindObjectsSortMode.None).Length==1&&Object.FindObjectsByType<Light>(FindObjectsInactive.Include,FindObjectsSortMode.None).Length==8,"Selective imports remain below 120k art triangles and 212 renderers; one camera/eight authored lights unchanged");
            foreach(string id in new[]{"office_notepads","plastic_thermos","gamepad"})
            {
                var material=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/Art/Generated/Materials/Refined_"+id+".mat");
                Check(material&&material.shader.name=="Universal Render Pipeline/Lit"&&new[]{"_BaseMap","_BumpMap","_MetallicGlossMap","_OcclusionMap"}.All(n=>material.GetTexture(n)&&material.GetTexture(n).width<=512&&material.GetTexture(n).height<=512),id+" has complete URP texture bindings capped at 512px");
            }
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
