#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;
namespace LayarTerakhir.Editor
{
    public static class PlaytestReviewValidation
    {
        [Serializable] public class Record {public string utc;public int failures;public string[] checks;}
        public static void Run()
        {
            var checks=new List<string>();int failures=0;
            void Check(bool pass,string label){checks.Add((pass?"PASS ":"FAIL ")+label);if(!pass){failures++;Debug.LogWarning("REVIEW_VALIDATION "+label);}}
            EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);var d=Object.FindFirstObjectByType<AtmajaDirector>();Physics.SyncTransforms();
            var tree=GameObject.Find("FallenKamboja");var treeBounds=KosVisualV2.BoundsOf(tree);float minEast=Mathf.Min(d.LocalPosition(treeBounds.min).x,d.LocalPosition(treeBounds.max).x);
            Debug.Log("REVIEW_TREE_BOUNDS "+treeBounds+" minEast="+minEast);
            foreach(var renderer in tree.GetComponentsInChildren<Renderer>())Debug.Log("REVIEW_TREE_PART "+renderer.name+" "+renderer.bounds);
            Check(minEast>6.3f,"Tree geometry stays outside the east wall; no root plate in dining chairs");
            var chairs=GameObject.Find("Atmaja_Environment_Art").GetComponentsInChildren<Transform>().Where(t=>t.name=="WornDiningChair").ToArray();
            Check(chairs.Length==4&&chairs.All(t=>Vector3.Dot(t.forward,(d.P(3.7f,d.LocalPosition(t.position).y,t.position.y)-t.position).normalized)>.95f),"Four wooden dining chairs face the table");
            Check(!GameObject.Find("Atmaja_Environment_Art").GetComponentsInChildren<Transform>().Any(t=>t.name.Contains("plastic_monobloc")),"No plastic-chair models remain in Atmaja");
            var clock=GameObject.Find("GrandfatherClock");Check(Vector3.Dot(clock.transform.forward,d.east)>.95f&&Mathf.Abs(d.LocalPosition(clock.transform.position).x+5.58f)<.01f,"Clock faces into room with rear flush against west wall");
            var ticking=d.clockHour.GetComponentInParent<ClockAmbience>();Check(ticking&&ticking.source&&ticking.source.outputAudioMixerGroup,"Clock has positional, mixer-routed ticking");
            Check(Object.FindObjectsByType<HingedDoor>(FindObjectsSortMode.None).Length==2,"Both exploration room doors survive scene serialization");
            var clue=d.arClue;Check(clue.GetComponent<MeshFilter>()&&!clue.GetComponent<TextMesh>()&&clue.GetComponent<Renderer>().sharedMaterial.GetColor("_BaseColor").r>.3f,"AR clue is red blood-stroke geometry, not floating font UI");
            Check(Mathf.Abs(d.LocalPosition(clue.transform.position).y-7.817f)<.001f,"Blood writing is attached to the wall plane");
            Check(!Object.FindObjectsByType<Light>(FindObjectsSortMode.None).Any(l=>l.name=="KitchenPractical"||l.name=="LivingFaintPractical"||l.name=="DiningColdSpill"),"No unmotivated interior fill lights; phone lights exploration");
            var cc=d.player.GetComponent<CharacterController>();
            bool Clear(Vector3 point,float height)
            {var blocked=Physics.OverlapCapsule(point+Vector3.up*cc.radius,point+Vector3.up*(height-cc.radius),cc.radius,~0,QueryTriggerInteraction.Ignore).Where(c=>c!=cc&&!c.transform.IsChildOf(d.player.transform)).ToArray();if(blocked.Length>0)Debug.Log("REVIEW_BLOCKERS "+point+" "+string.Join(",",blocked.Select(c=>c.name)));return blocked.Length==0;}
            d.kitchenDoor.localRotation=Quaternion.Euler(0,-95,0);Physics.SyncTransforms();
            Vector2[] path={new Vector2(5.15f,10.975f),new Vector2(5.15f,9.8f),new Vector2(4.1f,9.8f),new Vector2(4.1f,8.4f),new Vector2(5.35f,8),new Vector2(5.35f,1.3f),new Vector2(2.1f,1.3f),new Vector2(2.1f,4.1f),new Vector2(.8f,4.1f),new Vector2(-1.4f,4.1f),new Vector2(-3,5.3f),new Vector2(-4.7f,5.8f)};
            bool route=true;
            for(int i=1;i<path.Length;i++)
            {
                int count=Mathf.CeilToInt(Vector2.Distance(path[i-1],path[i])/.12f);
                // The .16m kitchen plinth is a legal step; raise the capsule within its radius.
                for(int j=0;j<=count;j++){var p=Vector2.Lerp(path[i-1],path[i],j/(float)count);if(!Clear(d.P(p.x,p.y,p.y>9.62f?.37f:.54f),d.player.standingHeight)){route=false;Debug.LogWarning("REVIEW_ROUTE_BLOCKED "+p);break;}}
            }
            Check(route,"Continuous standing route from kitchen around dining-table sides to lounge/clock; no tabletop waypoints");
            Check(Clear(d.P(4.6f,6.2f,.54f),d.player.crouchHeight),"Crouched access beside the backpack, without jumping onto furniture");
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();
            Check(corridor&&corridor.spawnLanding.position.y>3&&corridor.spawnGround.position.y<.1f,"Kos starts on floor 2 with a ground-floor checkpoint");
            Check(GameObject.Find("DAPUR BERSAMAFloor")&&GameObject.Find("KAMAR MANDIFloor"),"Shared kitchen and bathroom exist downstairs");
            Check(GameObject.Find("StairA_0")&&GameObject.Find("StairA_9")&&GameObject.Find("StairB_0")&&GameObject.Find("StairB_9"),"Two complete flights connect the kos floors");
            Check(corridor.motor&&corridor.gateHinge&&corridor.rideExit.position.z<-10,"Motorcycle and gate-to-street departure path are bound");
            Check(corridor.motorHeadlight&&!corridor.motorHeadlight.enabled&&corridor.motorHeadlight.transform.IsChildOf(corridor.motor),"Departure headlight is attached to the real scooter lens and starts off");
            Check(Object.FindObjectsByType<HingedDoor>(FindObjectsSortMode.None).Length>=14&&Object.FindObjectsByType<KosTravelItem>(FindObjectsSortMode.None).Length==2,"Closed kos doors, shared doors and travel interactions survive serialization");
            foreach(var t in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)))
                Check(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)==0,"No missing corridor scripts: "+t.name);
            var profile=PresentationProfile.Load();Check(profile&&profile.voices.Length>=14&&profile.voices.All(v=>v.clip&&v.clip.length>.3f),"Neural dialogue WAV bank is complete and serialized in build");
            Check(profile&&!profile.voices.Any(v=>v.speaker=="Bu Haji Ratna")&&profile.voices.Any(v=>v.text==LayarTerakhir.UI.KosDialogue.Trapped),"Landlady only leaves a letter; trapped-window dialogue remains voiced");
            File.WriteAllText("Audit/playtest-review-validation.json",JsonUtility.ToJson(new Record{utc=DateTime.UtcNow.ToString("o"),failures=failures,checks=checks.ToArray()},true)+"\n");
            if(failures>0)throw new Exception("Playtest review validation failed: "+failures);
        }
    }
    namespace Tests
    {
        public sealed class PlaytestReviewTests
        {
            [Test]public void ReviewedGeometryAndBothFloorKosBindings()=>PlaytestReviewValidation.Run();
            [TestCase("PROLOG_KOS_CORRIDOR")][TestCase("PROLOG_KOS_YARD")][TestCase("PROLOG_TRAVEL_ATMAJA")]
            public void CorridorSavesResumeTheBoardingHouse(string id)=>Assert.That(PrologSceneFlow.IsCorridorCheckpoint(id),Is.True);
        }
    }
}
#endif
