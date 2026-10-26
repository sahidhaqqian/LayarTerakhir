#if UNITY_EDITOR
using System.Linq;
using LayarTerakhir.Prolog;
using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace LayarTerakhir.Editor.Tests
{
    public sealed class KosGroundWallTests
    {
        [TearDown] public void Clear()=>EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        private static void Place(CharacterController cc,Vector3 position)
        {cc.enabled=false;cc.transform.position=position;cc.enabled=true;Physics.SyncTransforms();}
        [TestCase(-1,1.75f)] [TestCase(1,1.75f)]
        [TestCase(-1,.78f)] [TestCase(1,.78f)]
        public void RearGroundRoomBreachIsClosedForTheActualCapsule(int side,float height)
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            var d=Object.FindFirstObjectByType<KosCorridorDirector>();var root=d.buildingRoot;
            var wall=root.Find(side<0?"GroundHallEndWall_West":"GroundHallEndWall_East");Assert.That(wall,Is.Not.Null);
            var renderer=wall.GetComponent<MeshRenderer>();var collider=wall.GetComponent<BoxCollider>();
            Assert.That(renderer.enabled&&collider.enabled&&!collider.isTrigger,Is.True,"A visible, solid wall must close the breach");
            var source=root.Cast<Transform>().First(t=>t.name=="HallWallEndPier");
            Assert.That(renderer.sharedMaterial,Is.SameAs(source.GetComponent<Renderer>().sharedMaterial));
            var cc=d.player.GetComponent<CharacterController>();cc.height=height;cc.center=Vector3.up*(height*.5f);
            var start=root.TransformPoint(new Vector3(0,.04f,11.3f));var step=root.right*side*.1f;
            // Reproduce the original missing section, then restore the fix.
            collider.enabled=false;Place(cc,start);for(int i=0;i<25;i++)cc.Move(step);
            if(side<0)Assert.That(side*root.InverseTransformPoint(cc.transform.position).x,Is.GreaterThan(1.8f),"The reported west gap really admits the player");
            // The east opening is also visibly incomplete, although its nearby
            // stair return already catches a full-radius capsule.
            Assert.That(Physics.Raycast(root.TransformPoint(new Vector3(side*.8f,height*.6f,11.3f)),root.right*side,1.4f,~0,QueryTriggerInteraction.Ignore),Is.False,"Missing masonry exposes the room");
            collider.enabled=true;Place(cc,start);for(int i=0;i<25;i++)cc.Move(step);
            Assert.That(side*root.InverseTransformPoint(cc.transform.position).x,Is.LessThan(1.35f),"Standing and crouched players must stay outside the room");
            Place(cc,root.TransformPoint(new Vector3(side*2.5f,.04f,11.3f)));for(int i=0;i<25;i++)cc.Move(-step);
            Assert.That(side*root.InverseTransformPoint(cc.transform.position).x,Is.GreaterThan(1.65f),"Collision also closes the reverse route");
        }
        [Test] public void StairEntranceStillAcceptsTheStandingCapsule()
        {
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);var d=Object.FindFirstObjectByType<KosCorridorDirector>();var cc=d.player.GetComponent<CharacterController>();
            cc.height=1.75f;cc.center=Vector3.up*.875f;Place(cc,d.BuildingPoint(new Vector3(0,.04f,12.4f)));
            for(int i=0;i<18;i++)cc.Move(d.buildingRoot.right*.1f);
            Assert.That(d.buildingRoot.InverseTransformPoint(cc.transform.position).x,Is.GreaterThan(1.65f),"The masonry must stop before the stairs");
        }
    }
}
#endif
