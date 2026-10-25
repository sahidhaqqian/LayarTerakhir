#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase5;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class Phase5SceneTests
    {
        private Phase5Director d;
        [OneTimeSetUp] public void Open()
        {
            EditorSceneManager.OpenScene(Phase5SceneBuilder.ScenePath);d=Object.FindFirstObjectByType<Phase5Director>();
            Physics.SyncTransforms();
        }
        [OneTimeTearDown] public void Close()=>EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        [Test] public void SerializedSystemsAreSingleAndBound()
        {
            Assert.That(Object.FindObjectsByType<CampaignState>(FindObjectsSortMode.None).Length,Is.EqualTo(1));
            Assert.That(d.player&&d.phoneView&&d.world&&d.boss&&d.replicas&&d.soundscape&&d.balance,Is.True);
            Assert.That(d.world.lanternPosts.Length,Is.EqualTo(3));Assert.That(d.world.cableTargets.Length,Is.EqualTo(3));
            Assert.That(d.world.lanternVisuals.All(x=>x),Is.True);
            Assert.That(d.endingShots.Select(s=>s.ending).Distinct().Count(),Is.EqualTo(4));
            Assert.That(d.endingShots.All(s=>s.from&&s.to&&s.look&&s.seconds>0),Is.True);
        }
        [Test] public void AtLeast100NewActualModelSourcesArePlaced()
        {
            var markers=Object.FindObjectsByType<Phase5AssetSource>(FindObjectsInactive.Include,FindObjectsSortMode.None);
            Assert.That(markers.Select(m=>m.sourceId).Distinct().Count(),Is.GreaterThanOrEqualTo(100));
            Assert.That(markers.All(m=>m.GetComponentsInChildren<Renderer>(true).Length>0),Is.True);
            var invalid=markers.SelectMany(m=>m.GetComponentsInChildren<Renderer>(true)).SelectMany(r=>r.sharedMaterials).Where(m=>!m||!m.shader||m.shader.name=="Hidden/InternalErrorShader").ToArray();
            Assert.That(invalid,Is.Empty,"All downloaded renderers have resolved URP materials");
        }
        [Test] public void NoPreviousChapterEnvironmentDependency()
        {
            var deps=AssetDatabase.GetDependencies(Phase5SceneBuilder.ScenePath);
            Assert.That(deps.Where(p=>Enumerable.Range(1,4).Any(i=>p.Contains("/Art/Phase"+i+"/"))),Is.Empty);
        }
        [Test] public void PhysicalRecoveryAndToolsExistBeforeAndInsideLock()
        {
            var items=Object.FindObjectsByType<Phase5WorldItem>(FindObjectsInactive.Include,FindObjectsSortMode.None);
            Assert.That(items.Count(i=>i.id.StartsWith("DYNAMO_")),Is.GreaterThanOrEqualTo(3));
            Assert.That(items.Count(i=>i.id.StartsWith("PRAYER_")),Is.GreaterThanOrEqualTo(3));
            Assert.That(items.Count(i=>i.id.StartsWith("LANTERN_")),Is.EqualTo(3));
            Assert.That(items.Count(i=>i.id.StartsWith("CABLE_")),Is.EqualTo(3));
            Assert.That(items.All(i=>i.director==d&&i.GetComponent<Collider>()),Is.True);
        }
        [Test] public void LayerBudgetAndAuthoredHumanAvatarsAreValid()
        {
            Assert.That(d.world.faceMirrors.Length,Is.LessThanOrEqualTo(24));
            Assert.That(d.world.cableVisuals.All(g=>g.GetComponentsInChildren<Renderer>(true).All(r=>r.gameObject.layer==29)),Is.True);
            var humans=new[]{d.world.damar,d.replicas.actors[0],d.replicas.actors[1]};
            foreach(var h in humans){var a=h.GetComponentInChildren<Animator>(true);Assert.That(a&&a.avatar&&a.avatar.isValid&&a.avatar.isHuman,Is.True,h.name);Assert.That(a.runtimeAnimatorController,Is.Not.Null);}
        }
        [TestCase(0,4)][TestCase(0,62)][TestCase(6,108)][TestCase(6,138)][TestCase(6,147)]
        [TestCase(9,164)][TestCase(-9,172)][TestCase(0,205)][TestCase(-10,218)][TestCase(11,230)][TestCase(17,220)]
        public void WalkableSurfacesHaveActualSolidGround(float x,float z)
        {
            bool hit=Physics.Raycast(new Vector3(x,1,z),Vector3.down,out var floor,2,~((1<<24)|(1<<25)|(1<<26)|(1<<29)|(1<<31)),QueryTriggerInteraction.Ignore);
            Assert.That(hit,Is.True,"Ground at "+x+","+z);Assert.That(floor.normal.y,Is.GreaterThan(.8f));
        }
        [TestCase(0,4,0,48)][TestCase(0,62,0,88)][TestCase(6,103,6,144)]
        [TestCase(9,157,9,186)][TestCase(-9,186,-9,156)][TestCase(-14.5f,216.4f,-9,220)][TestCase(-14.5f,216.4f,13.5f,232.5f)]
        public void LocalNavigationIsConnected(float x,float z,float tx,float tz)
        {
            Assert.That(NavMesh.SamplePosition(new Vector3(x,.1f,z),out var a,2,NavMesh.AllAreas),Is.True);
            Assert.That(NavMesh.SamplePosition(new Vector3(tx,.1f,tz),out var b,2,NavMesh.AllAreas),Is.True);
            var path=new NavMeshPath();NavMesh.CalculatePath(a.position,b.position,NavMesh.AllAreas,path);
            Assert.That(path.status,Is.EqualTo(NavMeshPathStatus.PathComplete));
        }
        [Test] public void PlankCollapseHasNoInvisibleFloorUnderIt()
        {
            foreach(var p in d.world.collapsePlanks)
            {
                p.SetActive(false);Physics.SyncTransforms();
                Assert.That(Physics.Raycast(p.transform.position+Vector3.up,Vector3.down,2,~0,QueryTriggerInteraction.Ignore),Is.False,p.name);
                p.SetActive(true);
            }
        }
        [Test] public void SavedAnchorsDoNotSpawnStandingCapsulesInsideGeometry()
        {
            foreach(var a in d.world.anchors)
            {
                var pos=a.point.position;
                var obstacles=Physics.OverlapCapsule(pos+Vector3.up*.35f,pos+Vector3.up*1.4f,.26f,~((1<<24)|(1<<25)|(1<<26)|(1<<29)|(1<<31)),QueryTriggerInteraction.Ignore)
                    .Where(c=>!c.transform.IsChildOf(d.player.transform)).ToArray();
                Assert.That(obstacles,Is.Empty,"Anchor "+a.id+" clips "+string.Join(",",obstacles.Select(c=>c.name)));
            }
        }
        [Test] public void RecoveryCeilingAllowsCrouchButExcludesStanding()
        {
            var roofs=Object.FindObjectsByType<BoxCollider>(FindObjectsSortMode.None).Where(c=>c.name.Contains("RecoveryLowCanopy_")).ToArray();
            Assert.That(roofs.Length,Is.EqualTo(2));
            foreach(var roof in roofs){Assert.That(roof.bounds.min.y,Is.GreaterThan(d.player.crouchHeight+.1f));Assert.That(roof.bounds.min.y,Is.LessThan(d.player.standingHeight));}
        }
        [TestCase("TAPE")][TestCase("DOOR_1")][TestCase("REFILL")][TestCase("POUR")]
        [TestCase("SEWING_PORTAL")][TestCase("START_ARENA")][TestCase("GENERATOR")][TestCase("ARCHIVE")]
        [TestCase("CABLE_1")][TestCase("CABLE_2")][TestCase("CABLE_3")]
        public void CriticalInteractionHasAReachableUnoccludedStance(string id)
        {
            var item=Object.FindObjectsByType<Phase5WorldItem>(FindObjectsInactive.Include,FindObjectsSortMode.None).First(i=>i.id==id);
            bool tapeActive=d.world.tape.activeSelf;if(id=="TAPE")d.world.tape.SetActive(true);Physics.SyncTransforms();
            bool found=false;int mask=~((1<<24)|(1<<25)|(1<<26)|(1<<29)|(1<<31));
            for(int n=0;n<96&&!found;n++)
            {
                float angle=(n%24)*Mathf.PI*2/24,radius=1.2f+(n/24)*.4f;
                var candidate=new Vector3(item.transform.position.x+Mathf.Sin(angle)*radius,.08f,item.transform.position.z+Mathf.Cos(angle)*radius);
                if(!NavMesh.SamplePosition(candidate,out var at,.3f,NavMesh.AllAreas)||Mathf.Abs(at.position.y)>.35f)continue;
                foreach(float eye in new[]{1.55f,.92f})
                {
                    Vector3 start=at.position+Vector3.up*eye,ray=item.transform.position-start;
                    var hits=Physics.RaycastAll(start,ray.normalized,ray.magnitude+.4f,mask,QueryTriggerInteraction.Collide).OrderBy(h=>h.distance).ToArray();
                    if(hits.Length>0&&hits[0].collider.GetComponentInParent<Phase5WorldItem>()==item&&Vector3.Distance(at.position+Vector3.up,item.transform.position)<2.6f){found=true;break;}
                }
            }
            if(id=="TAPE")d.world.tape.SetActive(tapeActive);
            Assert.That(found,Is.True,"No accessible first-hit reticle stance for "+id);
        }
        [Test] public void CrowdEvpEmitterIsInFrontOfItsPhysicalScreen()
        {
            Assert.That(NavMesh.SamplePosition(new Vector3(9,.08f,209.7f),out var at,2,NavMesh.AllAreas),Is.True);
            var ray=d.world.crowd.position-(at.position+Vector3.up*1.55f);
            Assert.That(Physics.Raycast(at.position+Vector3.up*1.55f,ray.normalized,ray.magnitude-.05f,~((1<<24)|(1<<25)|(1<<26)|(1<<29)|(1<<31)),QueryTriggerInteraction.Ignore),Is.False,"EVP cannot require recording through the screen collider");
            Assert.That(Vector3.Distance(at.position+Vector3.up,d.world.crowd.position),Is.LessThan(3));
        }
        [TestCase(-14.5f,216,217.94f)][TestCase(13.5f,233,231.1f)]
        public void RecoveryOverhangPhysicallyBlocksTallBossAtToolStance(float x,float outside,float inside)
        {
            var from=new Vector3(x,3.5f,outside);var to=new Vector3(x,.65f,inside);
            Assert.That(Physics.Raycast(from,(to-from).normalized,out var hit,Vector3.Distance(from,to),~((1<<24)|(1<<25)|(1<<26)|(1<<29)|(1<<31)),QueryTriggerInteraction.Ignore),Is.True);
            Assert.That(hit.collider.name.Contains("RecoveryLowCanopy_"),Is.True,hit.collider.name);
        }
        [Test] public void DamarHasAuthoredHumanIdleWalkInsteadOfReplicaMotion()
        {
            var motion=d.world.damar.GetComponent<Phase5HumanMotion>();Assert.That(motion&&motion.animator,Is.True);
            Assert.That(motion.animator.runtimeAnimatorController.animationClips.Any(c=>c.name.Split('|').Last()=="Idle_Loop"&&c.humanMotion),Is.True);
            Assert.That(motion.animator.runtimeAnimatorController.animationClips.Any(c=>c.name.Split('|').Last()=="Walk_Loop"&&c.humanMotion),Is.True);
            Assert.That(motion.animator.runtimeAnimatorController.animationClips.All(c=>!c.name.Contains("CINEMA_4D")),Is.True);
        }
        [Test] public void FrozenDoorsAreVisibleFromBothSidesAndSolidWhenClosed()
        {
            foreach(var door in d.world.doorLeaves)
            {
                Assert.That(door.GetComponent<BoxCollider>(),Is.Not.Null);
                Assert.That(door.GetComponentsInChildren<Renderer>().SelectMany(r=>r.sharedMaterials).All(m=>m.GetFloat("_Cull")==0),Is.True);
            }
        }
        [Test] public void EndingDurationsRemainMandatory()
        {Assert.That(d.balance.normalHold,Is.EqualTo(5));Assert.That(d.balance.badDwell,Is.GreaterThanOrEqualTo(2));Assert.That(d.balance.syncSegment,Is.GreaterThanOrEqualTo(1.5f));}
        [Test] public void BuildContainsExactlyOneMarketScene()=>Assert.That(EditorBuildSettings.scenes.Count(s=>s.enabled&&s.path==Phase5SceneBuilder.ScenePath),Is.EqualTo(1));
    }
}
#endif
