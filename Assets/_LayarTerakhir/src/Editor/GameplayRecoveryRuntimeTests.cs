#if UNITY_EDITOR
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using LayarTerakhir.Core;
using LayarTerakhir.Interlude;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.TestTools;
using Object=UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    public sealed class GameplayRecoveryRuntimeTests
    {
        private Keyboard keyboard;private Mouse mouse;
        [UnitySetUp]public IEnumerator SetUp()
        {
            SessionState.SetString("Recovery.PreviousSave",Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")??"");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",Path.Combine(Path.GetTempPath(),"LT-Recovery-"+Guid.NewGuid()));
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
            var campaign=new GameObject("RecoveryCampaignSeed").AddComponent<CampaignState>();
            typeof(CampaignState).GetMethod("Awake",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(campaign,null);
            campaign.StartNewGame();campaign.Data.chapter_id=0;campaign.Data.checkpoint_id="PROLOG_COMPLETE";
            foreach(var id in new[]{"SMARTPHONE_RAKA","JAKET_RAKA","KUNCI_MOTOR","MATABATIN_APK","PROLOG_COMPLETE"})campaign.AddInventoryItem(id);
            Assert.That(InterludeCampaign.EnterInterlude(campaign),Is.True);
            InterludeCampaign.NoteBeat(campaign,InterludeIds.RatnaTalk);InterludeCampaign.NoteBeat(campaign,InterludeIds.HookRead);
            Assert.That(campaign.SaveCheckpointAtomic(InterludeIds.DayCheckpoint),Is.True);
            Object.DestroyImmediate(campaign.gameObject);
            RecoveryTestInputSettings.Prepare("Recovery.Input");
            EditorSceneManager.OpenScene(KosCorridorBuilder.ScenePath);
            yield return new EnterPlayMode();
            SessionState.SetString("Recovery.Input",JsonUtility.ToJson(InputSystem.settings));
            var settings=Object.Instantiate(InputSystem.settings);settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            settings.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
            InputSystem.settings=settings;keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();
            Time.timeScale=2;yield return null;
        }
        [UnityTearDown]public IEnumerator TearDown()
        {
            if(keyboard!=null)InputSystem.RemoveDevice(keyboard);if(mouse!=null)InputSystem.RemoveDevice(mouse);
            RecoveryTestInputSettings.Restore("Recovery.Input");
            Time.timeScale=1;yield return new ExitPlayMode();
            RecoveryTestInputSettings.RestoreEditorConfiguration("Recovery.Input");
            UiFocus.ApplicationFocused=true;UiFocus.PhoneAiming=false;
            string folder=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",SessionState.GetString("Recovery.PreviousSave",""));
            if(!string.IsNullOrEmpty(folder)&&Directory.Exists(folder))Directory.Delete(folder,true);
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
        [UnityTest]public IEnumerator DaytimeCapsuleReachesShopsAndTrafficHasSeatedRiders()=>RecoveryRuntimeScenario.Run(DaytimeScenario());
        private IEnumerator DaytimeScenario()
        {
            var director=Object.FindFirstObjectByType<InterludeDirector>();
            float deadline=Time.realtimeSinceStartup+20;
            while((!director||!director.Ready)&&Time.realtimeSinceStartup<deadline){director=Object.FindFirstObjectByType<InterludeDirector>();yield return null;}
            Assert.That(director&&director.Ready,Is.True,"Daytime startup must finish within the bounded fixture");
            var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();var player=corridor.player;var cc=player.GetComponent<CharacterController>();
            var rig=player.GetComponent<ShoulderCameraRig>();var camera=player.GetComponent<InteractionInteractor>().playerCamera;
            player.SendMessage("OnApplicationFocus",true);player.enabled=false;
            Place(cc,new Vector3(-16,.22f,-4.5f));
            Vector3[] route={new Vector3(-16,0,-7),new Vector3(-16,0,-11.75f),new Vector3(-11,0,-11.75f),
                new Vector3(-7,0,-11.75f),new Vector3(-4,0,-11.75f),new Vector3(0,0,-14.5f),new Vector3(0,0,-24),new Vector3(5.25f,0,-24)};
            int leg=0;
            foreach(var point in route)
            {
                yield return Walk(cc,point);
                if(leg++==1)
                {
                    yield return RecoveryVisualEvidence.CaptureFrame(camera,"04a-open-kos-gate-runtime",()=>camera.transform.SetPositionAndRotation(new Vector3(-18,1.7f,-10.8f),Quaternion.LookRotation(new Vector3(-16,1,-6.5f)-new Vector3(-18,1.7f,-10.8f))));
                }
            }
            Assert.That(Object.FindFirstObjectByType<MelatiShopkeeper>().CanInteract(),Is.True);
            AssertNpcRay(player.transform.position+Vector3.up*1.5f,GameObject.Find("BuSriShopkeeper").transform);
            rig.RefreshCameraPose();yield return RecoveryVisualEvidence.CaptureFrame(camera,"05-warung-customer-approach");
            foreach(var point in new[]{new Vector3(0,0,-24),new Vector3(0,0,-28.9f),new Vector3(5.4f,0,-28.9f),new Vector3(7.8f,0,-28.9f)})yield return Walk(cc,point);
            rig.RefreshCameraPose();yield return RecoveryVisualEvidence.CaptureFrame(camera,"06-warteg-doorway-interior");
            foreach(var point in new[]{new Vector3(5.4f,0,-28.9f),new Vector3(0,0,-28.9f),new Vector3(0,0,-17),new Vector3(4.2f,0,-17)})yield return Walk(cc,point);
            var vendor=Object.FindObjectsByType<InterludeNpc>(FindObjectsSortMode.None).Single(n=>n.id=="VENDOR");
            Assert.That(vendor.transform.position.x,Is.GreaterThan(6),"World coordinates must not be added to the kos building offset twice");
            Assert.That(vendor.CanInteract(),Is.True);AssertNpcRay(player.transform.position+Vector3.up*1.5f,vendor.transform);
            rig.RefreshCameraPose();yield return RecoveryVisualEvidence.CaptureFrame(camera,"07-gerobak-and-reachable-vendor");
            var stairs=GameObject.Find("InterludeStairsReturn").GetComponent<BoxCollider>();Assert.That(stairs.isTrigger,Is.True);
            Place(cc,corridor.BuildingPoint(new Vector3(0,.22f,11.3f)));
            yield return Walk(cc,corridor.BuildingPoint(new Vector3(0,0,12.6f)));
            yield return Walk(cc,corridor.BuildingPoint(new Vector3(1.75f,0,12.6f)));
            var traffic=Object.FindObjectsByType<InterludeTraffic>(FindObjectsSortMode.None).Single();traffic.enabled=false;
            var bikes=Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).Where(t=>t.name=="InterludeTrafficMotor").ToArray();
            Assert.That(bikes.Length,Is.EqualTo(2));
            foreach(var bike in bikes)
            {
                var rider=bike.GetComponentInChildren<AnimatedRakaAvatar>(true);Assert.That(rider,Is.Not.Null);
                var bones=rider.GetComponentsInChildren<Transform>(true);var seat=bike.GetComponentsInChildren<Transform>().Single(t=>t.name=="RiderSeatSurface");
                Assert.That(Vector3.Distance(bones.Single(t=>t.name=="Hips").position,seat.position+Vector3.up*.075f),Is.LessThan(.025f));
                foreach(string side in new[]{"L","R"})
                {
                    var hand=bones.Single(t=>t.name=="Hand_"+side);
                    var grip=bike.GetComponentsInChildren<Transform>().Where(t=>t.name.StartsWith("RiderGrip")).OrderBy(g=>Vector3.Distance(g.position,hand.position)).First();
                    // The wrist joint is behind the palm/fingers. Contact is a
                    // property of the rendered hand surface, not that joint.
                    var body=rider.GetComponentsInChildren<SkinnedMeshRenderer>().Single(r=>r.name=="VisibleSkin");
                    var handBones=new System.Collections.Generic.HashSet<int>();
                    for(int i=0;i<body.bones.Length;i++)if(body.bones[i].name=="Hand_"+side||body.bones[i].name.StartsWith("Finger")&&body.bones[i].name.EndsWith("_"+side))handBones.Add(i);
                    var mesh=new Mesh();body.BakeMesh(mesh,true);var vertices=mesh.vertices;var weights=body.sharedMesh.boneWeights;float gap=float.PositiveInfinity;
                    for(int i=0;i<vertices.Length;i++)
                    {
                        var w=weights[i];float weight=(handBones.Contains(w.boneIndex0)?w.weight0:0)+(handBones.Contains(w.boneIndex1)?w.weight1:0)+(handBones.Contains(w.boneIndex2)?w.weight2:0)+(handBones.Contains(w.boneIndex3)?w.weight3:0);
                        if(weight>.5f)gap=Mathf.Min(gap,Vector3.Distance(body.transform.TransformPoint(vertices[i]),grip.position));
                    }
                    Object.Destroy(mesh);
                    Debug.Log("TRAFFIC_HAND_CONTACT side="+side+" surfaceGap="+gap+" wristGap="+Vector3.Distance(grip.position,hand.position));
                    Assert.That(gap,Is.LessThan(.035f),"Both rendered traffic hands must touch the handlebar envelope");
                }
                Assert.That(bike.GetComponentsInChildren<Collider>().All(c=>!c.enabled||c.isTrigger),Is.True);
            }
            bikes[0].SetPositionAndRotation(new Vector3(-1.8f,0,-21),Quaternion.Euler(0,180,0));
            yield return RecoveryVisualEvidence.CaptureFrame(camera,"08-traffic-motor-with-rider",()=>camera.transform.SetPositionAndRotation(new Vector3(-4.5f,1.6f,-24.5f),Quaternion.LookRotation(new Vector3(-1.8f,1,-21)-new Vector3(-4.5f,1.6f,-24.5f))));
            yield return RecoveryVisualEvidence.CaptureFrame(camera,"08b-full-scale-sedan",()=>camera.transform.SetPositionAndRotation(new Vector3(-1,1.55f,-41),Quaternion.LookRotation(new Vector3(2.7f,.75f,-38)-new Vector3(-1,1.55f,-41))));
            var campaign=CampaignState.Instance;Assert.That(campaign.Data.chapter_id,Is.Zero);
            Assert.That(campaign.SaveCheckpointAtomic(campaign.Data.checkpoint_id),Is.True);Assert.That(campaign.LoadCheckpoint(),Is.True);
            Assert.That(InterludeIds.IsInterludeData(campaign.Data),Is.True);Assert.That(campaign.HasInventoryItem(InterludeIds.RatnaTalk),Is.True);
            // Verify the actual player controller drives walk/run/turn/stop clips.
            Place(cc,new Vector3(0,.22f,-33));player.enabled=true;Time.timeScale=1;
            for(int phase=0;phase<4;phase++)
            {
                InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.W));yield return new WaitForSeconds(.2f);
                yield return RecoveryVisualEvidence.CaptureFrame(camera,"09-walk-phase-"+phase);
                yield return RecoveryVisualEvidence.CaptureFrame(camera,"09-walk-fullbody-"+phase,()=>
                {
                    var inspection=player.transform.position+player.transform.forward*2.6f+player.transform.right*1.5f+Vector3.up*1.1f;
                    camera.transform.SetPositionAndRotation(inspection,Quaternion.LookRotation(player.transform.position+Vector3.up*.9f-inspection));camera.fieldOfView=45;
                });
            }
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.W,Key.LeftShift));yield return new WaitForSeconds(.7f);
            Assert.That(player.IsSprinting,Is.True);yield return RecoveryVisualEvidence.CaptureFrame(camera,"10-running");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState(Key.A));
            InputSystem.QueueStateEvent(mouse,new MouseState{delta=new Vector2(80,0)});yield return new WaitForSeconds(.3f);
            yield return RecoveryVisualEvidence.CaptureFrame(camera,"11-turning");
            InputSystem.QueueStateEvent(keyboard,new KeyboardState());yield return new WaitForSeconds(.4f);
            Assert.That(player.PlanarVelocity.magnitude,Is.LessThan(.05f));yield return RecoveryVisualEvidence.CaptureFrame(camera,"12-stopped");
        }
        private static void Place(CharacterController cc,Vector3 point)
        {cc.enabled=false;cc.transform.SetPositionAndRotation(point,Quaternion.identity);cc.enabled=true;Physics.SyncTransforms();}
        private static IEnumerator Walk(CharacterController cc,Vector3 target)
        {
            for(int step=0;step<700;step++)
            {
                Vector3 delta=target-cc.transform.position;delta.y=0;
                if(delta.magnitude<.1f)break;
                cc.transform.rotation=Quaternion.LookRotation(delta);
                cc.Move(Vector3.ClampMagnitude(delta,.12f)+Vector3.down*.08f);
                if(step%8==0)yield return null;
            }
            Vector3 remaining=target-cc.transform.position;remaining.y=0;
            Assert.That(remaining.magnitude,Is.LessThan(.15f),"Actual capsule blocked before "+target+" at "+cc.transform.position);
            var floor=Physics.RaycastAll(cc.transform.position+Vector3.up*.3f,Vector3.down,.65f,~0,QueryTriggerInteraction.Ignore);
            Assert.That(floor.Any(h=>!h.transform.IsChildOf(cc.transform)&&h.normal.y>.5f),Is.True,"No support under the actual capsule at "+cc.transform.position);
            Assert.That(cc.transform.position.y,Is.InRange(-.05f,.45f),"No stepping onto props/fall-through");
        }
        private static void AssertNpcRay(Vector3 origin,Transform npc)
        {
            Vector3 target=npc.position+Vector3.up*1.45f;Vector3 delta=target-origin;
            Assert.That(delta.magnitude,Is.LessThan(3),"Interaction must be reachable from customer space");
            var hit=Physics.RaycastAll(origin,delta.normalized,delta.magnitude+.2f,~0,QueryTriggerInteraction.Collide)
                .Where(h=>!h.transform.GetComponentInParent<FirstPersonController>()).OrderBy(h=>h.distance).FirstOrDefault();
            Assert.That(hit.collider&&hit.transform.IsChildOf(npc),Is.True,"Interaction ray blocked by "+(hit.collider?hit.collider.name:"no NPC collider"));
        }
    }
}
#endif
