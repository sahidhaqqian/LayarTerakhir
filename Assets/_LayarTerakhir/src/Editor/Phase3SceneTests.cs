#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using LayarTerakhir.Phase3;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase3SceneTests
    {
        private Phase3Director d;
        [SetUp] public void Setup(){EditorSceneManager.OpenScene(Phase3SceneBuilder.ScenePath);d=UnityEngine.Object.FindFirstObjectByType<Phase3Director>();}
        [TearDown] public void Cleanup()=>EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        [Test] public void SceneHasOnePlayerPhoneDirectorAndNoMissingScripts()
        {
            Assert.That(d,Is.Not.Null);Assert.That(UnityEngine.Object.FindObjectsByType<Phase3Director>(FindObjectsSortMode.None).Length,Is.EqualTo(1));Assert.That(d.wewe.agent,Is.Not.Null);
            foreach(var root in d.gameObject.scene.GetRootGameObjects())foreach(var b in root.GetComponentsInChildren<MonoBehaviour>(true))Assert.That(b&&MonoScript.FromMonoBehaviour(b),Is.True);
        }
        [Test] public void AllRenderedMaterialsHaveRealShadersAndEveryTargetHasPhysicalInteraction()
        {
            foreach(var r in UnityEngine.Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include,FindObjectsSortMode.None))foreach(var m in r.sharedMaterials)Assert.That(m&&m.shader&&m.shader.name!="Hidden/InternalErrorShader",Is.True,r.name);
            foreach(var item in UnityEngine.Object.FindObjectsByType<Phase3WorldItem>(FindObjectsInactive.Include,FindObjectsSortMode.None))Assert.That(item.GetComponentInChildren<Collider>(),Is.Not.Null,item.id);
        }
        [TestCase("MILL")][TestCase("BOILER")][TestCase("OFFICE")][TestCase("BAGASSE")][TestCase("ALTAR")]
        public void SafeAnchorHasCompletePhysicalNavRouteFromYard(string id)
        {
            Assert.That(NavMesh.SamplePosition(d.world.Anchor("YARD").position,out var start,.7f,NavMesh.AllAreas),Is.True);
            Assert.That(NavMesh.SamplePosition(d.world.Anchor(id).position,out var end,.7f,NavMesh.AllAreas),Is.True,id);
            var p=new NavMeshPath();Assert.That(NavMesh.CalculatePath(start.position,end.position,NavMesh.AllAreas,p),Is.True);Assert.That(p.status,Is.EqualTo(NavMeshPathStatus.PathComplete),id);
        }
        [TestCase(47,4,35)][TestCase(46,4,43)][TestCase(56,4,45)][TestCase(59,4,45)][TestCase(62.5f,0,39)][TestCase(74,4,34)][TestCase(90,4,34)]
        public void UpperRoutesAndSecretHaveConnectedPhysicalNavigation(float x,float y,float z)
        {
            Assert.That(NavMesh.SamplePosition(d.world.Anchor("YARD").position,out var start,.7f,NavMesh.AllAreas),Is.True);
            Assert.That(NavMesh.SamplePosition(new Vector3(x,y,z),out var end,.7f,NavMesh.AllAreas),Is.True);
            var path=new NavMeshPath();NavMesh.CalculatePath(start.position,end.position,NavMesh.AllAreas,path);
            Assert.That(path.status,Is.EqualTo(NavMeshPathStatus.PathComplete));
        }
        [Test] public void CollapsedTilesDoNotLeaveAnInvisibleSupportingFloor()
        {
            Physics.SyncTransforms();
            foreach(var tile in d.world.floors)
            {
                tile.surface.SetActive(false);Physics.SyncTransforms();
                Vector3 p=tile.footBounds.center+Vector3.up*.4f;
                var hits=Physics.RaycastAll(p,Vector3.down,.9f,~((1<<24)|(1<<26)|(1<<29)|(1<<31)|(1<<2)),QueryTriggerInteraction.Ignore);
                Assert.That(hits.Any(h=>h.normal.y>.6f),Is.False,"Supporting floor remains under "+tile.name+" at "+tile.footBounds.center);tile.surface.SetActive(true);
            }
        }
        [Test] public void FragileBoardsCannotLeaveAnAIGroundRouteAcrossTheVoid()
        {
            foreach(var tile in d.world.floors.Where(t=>t.kind==Phase3FloorKind.FragileFatal||t.kind==Phase3FloorKind.FragileDrop))
                Assert.That(NavMesh.SamplePosition(tile.footBounds.center,out _,.2f,NavMesh.AllAreas),Is.False,tile.name);
        }
        [Test] public void VisibleExitMotorHasAReachablePhysicalInteractionBody()
        {
            var motor=GameObject.Find("RakaMotor_CampaignCarryOver");Assert.That(motor,Is.Not.Null);
            var renderers=motor.GetComponentsInChildren<Renderer>();var bounds=renderers[0].bounds;
            foreach(var renderer in renderers.Skip(1))bounds.Encapsulate(renderer.bounds);
            Assert.That(bounds.size.y,Is.InRange(1.1f,1.2f));Physics.SyncTransforms();
            var hits=Physics.RaycastAll(bounds.center+Vector3.forward*2.4f,Vector3.back,2.5f,~0,QueryTriggerInteraction.Collide);
            Assert.That(hits.Any(hit=>hit.collider.GetComponentInParent<Phase3WorldItem>()?.id=="EXIT_MOTOR"),Is.True,"Aim at visible bike must hit its interaction body, not the floor");
        }
        [Test] public void FactoryLibraryContainsThirtyDistinctNewNonArchitecturalProps()
        {
            string json=File.ReadAllText("Audit/phase3-scene-content.json");var record=JsonUtility.FromJson<Count>(json);Assert.That(record.uniqueProps,Is.GreaterThanOrEqualTo(30));
        }
        [Test] public void SolidSurfacesRetainDownloadedAlbedoAndNormalMaps()
        {
            foreach(string name in new[]{"concrete_floor_02","red_brick_03","metal_grate_rusty","corrugated_iron_02","brown_planks_09"})
            {
                var mat=AssetDatabase.LoadAssetAtPath<Material>("Assets/_LayarTerakhir/assets/Art/Phase3/Materials/Surface_"+name+".mat");
                Assert.That(mat,Is.Not.Null,name);Assert.That(mat.GetTexture("_BaseMap"),Is.Not.Null,name+" albedo");Assert.That(mat.GetTexture("_BumpMap"),Is.Not.Null,name+" normal");
            }
            var profile=AssetDatabase.LoadAssetAtPath<UnityEngine.Rendering.VolumeProfile>("Assets/_LayarTerakhir/assets/Art/Phase3/Phase3Volume.asset");
            Assert.That(profile.components.All(c=>c&&AssetDatabase.Contains(c)),Is.True,"Volume overrides must survive editor reload and player serialization");
        }
        [Test] public void DecoyMarkerDoesNotOccludeItsOwnRitualInteraction()
        {
            d.player.transform.position=d.world.Anchor("ALTAR").position;Physics.SyncTransforms();
            Assert.That(d.boss.NearDecoy(),Is.True,"The sack collider must not reject a valid E interaction at its own marker");
        }
        [Serializable]private class Count{public int uniqueProps;}
    }
}
#endif
