#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor
{
    public static class AtmajaSceneValidation
    {
        [Serializable] public class Report {public string utc,goldenSceneSha256;public int triangles,renderers,textureMaximum,failures;public string[] checks;}
        public static void Run(){EditorSceneManager.OpenScene(AtmajaSceneBuilder.ScenePath);Validate();}
        public static Report Validate(bool throwOnFailure=true)
        {
            var checks=new List<string>();int failures=0;
            void Check(bool value,string label){checks.Add((value?"PASS ":"FAIL ")+label);if(!value){failures++;Debug.LogWarning("ATMAJA_VALIDATION "+label);}}
            var d=Object.FindFirstObjectByType<AtmajaDirector>();Physics.SyncTransforms();
            Check(d&&d.player&&d.phoneView&&d.gateHinge&&d.vaultInside&&d.vaultOutside&&d.kitchenShutter,"Director/player/phone/traversal bindings");
            Check(Object.FindObjectsByType<HudController>(FindObjectsSortMode.None).Length==1,"Exactly one shared Golden HUD");
            Check(Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Length==1,"Exactly one AudioListener");
            Check(d.audioProfile&&new[]{d.rainSource,d.foleySource,d.stepsSource,d.notificationSource}.All(s=>s&&s.outputAudioMixerGroup&&!s.playOnAwake&&s.dopplerLevel==0),"Four real mixer-routed sources, no autoplay/Doppler");
            var cc=d.player.GetComponent<CharacterController>();
            bool Clear(Vector3 p,float height)
            {
                var blockers=Physics.OverlapCapsule(p+Vector3.up*cc.radius,p+Vector3.up*(height-cc.radius),cc.radius,~0,QueryTriggerInteraction.Ignore).Where(c=>c!=cc&&!c.transform.IsChildOf(d.player.transform)).ToArray();
                if(blockers.Length>0)Debug.Log("ATMAJA_CAPSULE_BLOCKED "+p+" h="+height+" by "+string.Join("; ",blockers.Take(4).Select(c=>c.name+" "+c.bounds)));
                return blockers.Length==0;
            }
            // Stair route ignores the treads/landings the feet legitimately rest on, plus the locked door that opens.
            bool StairClear(Vector3 p)
            {
                var blockers=Physics.OverlapCapsule(p+Vector3.up*cc.radius,p+Vector3.up*(cc.height-cc.radius),cc.radius,~0,QueryTriggerInteraction.Ignore)
                    .Where(c=>c!=cc&&!c.transform.IsChildOf(d.player.transform))
                    .Where(c=>!c.name.StartsWith("StairStep")&&!c.name.StartsWith("StairEntryPlatform")&&!c.name.StartsWith("StairMidLanding")&&!c.name.StartsWith("StairTopLanding")&&!c.name.StartsWith("LockedCentralStairDoor")).ToArray();
                if(blockers.Length>0)Debug.Log("ATMAJA_STAIR_BLOCKED "+p+" by "+string.Join("; ",blockers.Select(c=>c.name+" "+c.bounds)));
                return blockers.Length==0;
            }
            Check(Clear(d.spawnCourtyard.position,cc.height),"Arrival standing capsule is clear");
            Check(Clear(d.spawnKitchen.position,cc.height),"Kitchen landing standing capsule is clear");
            Check(Clear(d.spawnDining.position,cc.height),"MataBatin checkpoint standing capsule is clear");
            var window=GameObject.Find("KitchenVaultContext").GetComponent<BoxCollider>();
            Check(window.enabled&&!window.isTrigger,"Kitchen opening cannot bypass the contextual story vault with a generic jump");
            var under=d.P(8.2f,5.5f,.06f);
            Check(!Clear(under,d.player.standingHeight),"Kamboja physically blocks standing, not just an objective trigger");
            Check(Clear(under,d.player.crouchHeight),"Actual crouching capsule fits under the kamboja");
            Check(Clear(d.P(8.2f,4.2f,.06f),d.player.standingHeight)&&Clear(d.P(8.2f,7.0f,.06f),d.player.standingHeight),"Both sides of the crouch obstacle have standing clearance");
            // Sample the physical east route, using the crouch profile only through the limb.
            Check(Enumerable.Range(0,30).All(i=>Clear(d.P(8.2f,-.5f+i*.4f,.06f),i>=12&&i<=18?d.player.crouchHeight:d.player.standingHeight)),"Continuous east-side capsule-clear route to the kitchen approach");
            var root=GameObject.Find("Atmaja_Environment_Art");int tris=0,texture=0;
            foreach(var f in root.GetComponentsInChildren<MeshFilter>(true))
            {Check(f.sharedMesh,"Mesh reference: "+f.name);if(f.sharedMesh)for(int s=0;s<f.sharedMesh.subMeshCount;s++)tris+=(int)f.sharedMesh.GetIndexCount(s)/3;}
            var renderers=root.GetComponentsInChildren<Renderer>(true);
            foreach(var r in renderers)
            foreach(var mat in r.sharedMaterials)
            {
                Check(mat&&mat.shader&&mat.shader.name!="Hidden/InternalErrorShader","Material: "+r.name);
                if(mat)foreach(var property in mat.GetTexturePropertyNames()){var t=mat.GetTexture(property);if(t)texture=Mathf.Max(texture,t.width,t.height);}
            }
            foreach(var t in UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(g=>g.GetComponentsInChildren<Transform>(true)))
                Check(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject)==0,"No missing scripts: "+t.name);
            Check(tris<300000,"Two-story scene art (ground floor, upper floor, basement, chase, AR) remains under 300k triangles");
            Check(texture<=1024,"Bound artwork textures at most 1024px");
            Check(d.arFigure.layer==AtmajaPhoneView.ARLayer&&d.arClue.layer==AtmajaPhoneView.ARLayer&&!d.arFigure.activeSelf&&!d.arClue.activeSelf,"AR apparition/clue are isolated and start hidden");
            Check(d.upper&&d.basement&&d.chase&&d.upper.director==d&&d.basement.director==d&&d.chase.director==d,"Upper, basement and chase systems wired to the director");
            Check(d.upper&&d.upper.childDoor&&d.upper.masterDoor&&d.upper.chainLinks.Length==3,"Scene 4 upper-floor doors and three sin chains present");
            Check(d.basement&&d.basement.nyonya&&d.basement.waypoints.Length>=4&&d.chase&&d.chase.chaserFigure&&d.chase.rail.Length>=20,"Scene 5-7 hunter, patrol route and chase rail present");
            Check(d.upper.spawnUpper&&Clear(d.upper.spawnUpper.position,cc.height),"Upper-floor landing standing capsule is clear");
            var stairRoute=new[]{d.P(-2.4f,5.5f,.53f),d.P(-2.4f,6.4f,.56f),d.P(-3.5f,6.4f,.56f),d.P(-3.5f,7.5f,.62f),d.P(-2.7f,7.45f,.82f),d.P(-1.6f,7.45f,1.44f),d.P(-0.5f,7.45f,1.95f),d.P(.2f,7.45f,2.08f),d.P(.2f,7.0f,2.3f),d.P(-0.6f,7.0f,2.6f),d.P(-1.6f,7.0f,3.06f),d.P(-2.7f,7.0f,3.7f),d.P(-3.25f,6.45f,3.88f)};
            Check(stairRoute.All(StairClear),"Ground-to-upper stair route is continuously capsule-clear");
            Check(d.basement.spawnBasement&&Clear(d.basement.spawnBasement.position,cc.height),"Basement spawn standing capsule is clear");
            Check(d.chase.windowExitSpot&&Clear(d.chase.windowExitSpot.position,cc.height),"Chase window-exit standing capsule is clear");
            Check(d.chase.spawnStreet&&Clear(d.chase.spawnStreet.position,cc.height),"Ending street spawn standing capsule is clear");
            var figureBounds=KosVisualV2.BoundsOf(d.arFigure);
            Debug.Log("ATMAJA_AR_BOUNDS figure="+figureBounds+" clue="+d.arClue.GetComponent<Renderer>().bounds);
            Check(figureBounds.size.y>1&&figureBounds.size.y<2,"AR figure is human-scale, not an FBX unit-conversion error");
            Check(d.phoneView.feedMaterial&&d.phoneView.feedMaterial.shader.name=="LayarTerakhir/AtmajaPhoneFeed","Serialized phone-feed shader included in the build");
            Check(EditorBuildSettings.scenes.Length>=2&&EditorBuildSettings.scenes[0].path==PrologKosSceneBuilder.ScenePath&&EditorBuildSettings.scenes[1].path==AtmajaSceneBuilder.ScenePath,"Build starts at approved Kos and includes Atmaja second");
            string hash;
            using(var sha=SHA256.Create())hash=BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(PrologKosSceneBuilder.ScenePath))).Replace("-","").ToLowerInvariant();
            var protectedData=JsonUtility.FromJson<AtmajaSceneBuilder.Manifest>(File.ReadAllText("Audit/atmaja-art-manifest.json"));
            Check(hash==protectedData.protectedGoldenSceneSha256,"Approved Golden scene bytes preserved exactly");
            var report=new Report{utc=DateTime.UtcNow.ToString("o"),triangles=tris,renderers=renderers.Length,textureMaximum=texture,failures=failures,goldenSceneSha256=hash,checks=checks.ToArray()};
            File.WriteAllText("Audit/atmaja-scene-validation.json",JsonUtility.ToJson(report,true)+"\n");
            Debug.Log("ATMAJA_VALIDATION checks="+checks.Count+" failures="+failures+" tris="+tris+" renderers="+renderers.Length);
            if(throwOnFailure&&failures>0)throw new Exception("Atmaja scene validation failed: "+failures);return report;
        }
    }
    namespace Tests
    {
        public sealed class AtmajaSceneTests
        {
            [Test] public void AtmajaSceneKeepsGoldenAndHasPhysicalTraversalAndCompleteMaterials()=>AtmajaSceneValidation.Run();
            [TestCase("PROLOG_ATMAJA_COURTYARD")][TestCase("PROLOG_ATMAJA_KITCHEN")][TestCase("PROLOG_MATABATIN_ACQUIRED")]
            public void ContinuationCheckpointRoutingIsExplicit(string id)=>Assert.That(PrologSceneFlow.IsAtmajaCheckpoint(id),Is.True);
            [Test] public void KosCheckpointNeverSkipsApprovedOpening()=>Assert.That(PrologSceneFlow.IsAtmajaCheckpoint("PROLOG_KOS_COMPLETE"),Is.False);
        }
    }
}
#endif
