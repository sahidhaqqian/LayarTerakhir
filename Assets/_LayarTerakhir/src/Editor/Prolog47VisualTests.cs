#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

namespace LayarTerakhir.Editor.Tests
{
    // Posed gameplay-camera review, explicitly separate from walking/interaction tests.
    // No synthetic lights, replacement materials or screenshot colour corrections.
    public sealed class Prolog47VisualTests
    {
        [UnitySetUp] public IEnumerator SetUp()
        {
            SessionState.SetString("Prolog47.PreviousSave", Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY") ?? "");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", Path.Combine(Path.GetTempPath(), "Prolog47-" + Guid.NewGuid()));
            EditorSceneManager.OpenScene(TestContext.CurrentContext.Test.Name.StartsWith("Kos") ? PrologKosSceneBuilder.ScenePath : AtmajaSceneBuilder.ScenePath);
            yield return new EnterPlayMode();
            yield return new WaitForSeconds(2);
        }
        [UnityTearDown] public IEnumerator TearDown()
        {
            Time.timeScale = 1;
            yield return new ExitPlayMode();
            string save = Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY", SessionState.GetString("Prolog47.PreviousSave", ""));
            if (!string.IsNullOrEmpty(save) && Directory.Exists(save)) Directory.Delete(save, true);
            UiFocus.Cinematic = UiFocus.Menu = UiFocus.Clock = UiFocus.PhoneAiming = false;
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene);
        }
        [UnityTest] public IEnumerator AtmajaGameplayCameraReview() => RecoveryRuntimeScenario.Run(Atmaja());
        [UnityTest] public IEnumerator KosGameplayCameraReview() => RecoveryRuntimeScenario.Run(Kos());
        [UnityTest] public IEnumerator AtmajaChainAndCupDetails()=>RecoveryRuntimeScenario.Run(Details());
        private IEnumerator Details()
        {
            var d=Object.FindFirstObjectByType<AtmajaDirector>();var player=d.player;var rig=player.GetComponent<ShoulderCameraRig>();
            player.MovementLocked=true;player.SendMessage("OnApplicationFocus",true);rig.SetPerspective(PlayerPerspective.FirstPerson);
            var phone=SmartphoneController.Instance;phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.batteryPercent=90;phone.SyncVisualState();d.phoneView.Apps.Open(PhoneApp.Camera);
            d.PlacePlayer(d.P(0,-1.65f,.10f),Quaternion.identity);Aim(player,d.P(0,-.23f,1.65f));rig.RefreshCameraPose();yield return null;yield return null;
            yield return Capture(d.phoneView.playerCamera,"17-front-door-chain");
            d.PlacePlayer(d.P(-4.05f,.9f,3.83f),Quaternion.identity);Aim(player,d.upper.cupAim.position);rig.RefreshCameraPose();yield return null;yield return null;
            d.phoneView.SetViewfinder(true);yield return new WaitForSeconds(.5f);
            yield return Capture(d.phoneView.playerCamera,"33-cup-aim-closeup");
            d.phoneView.SetViewfinder(false);yield return new WaitForSeconds(.3f);
            var porcelain=d.upper.childCup.GetComponentsInChildren<Renderer>().Single(r=>r.name=="Fine cracked porcelain");
            var fracture=d.upper.childCup.GetComponentsInChildren<Renderer>().Single(r=>r.name=="Irregular glazed crack");
            var normal=Vector3.ProjectOnPlane(fracture.bounds.center-porcelain.bounds.center,Vector3.up).normalized;
            var approach=porcelain.bounds.center+normal*.75f;approach.y=3.83f;
            d.PlacePlayer(approach,Quaternion.identity);Aim(player,fracture.bounds.center);rig.RefreshCameraPose();yield return null;yield return null;
            yield return Capture(d.phoneView.playerCamera,"33-ceramic-fracture-detail",()=>
            {
                var camera=d.phoneView.playerCamera;camera.transform.position=fracture.bounds.center+normal*.20f+Vector3.up*.06f;camera.transform.LookAt(fracture.bounds.center);
            });
            d.PlacePlayer(d.P(3.1f,6.1f,3.83f),Quaternion.identity);Aim(player,d.P(2.9f,5.55f,3.834f));rig.RefreshCameraPose();yield return null;yield return null;
            yield return Capture(d.phoneView.playerCamera,"22-blood-stain-on-upper-wood-floor");
        }
        private IEnumerator Atmaja()
        {
            Assert.That(SystemInfo.graphicsDeviceType, Is.Not.EqualTo(UnityEngine.Rendering.GraphicsDeviceType.Null), "Visual review requires a renderer");
            var d = Object.FindFirstObjectByType<AtmajaDirector>();
            var player = d.player; var eye = d.phoneView.playerCamera;
            player.MovementLocked = true;
            var rig = player.GetComponent<ShoulderCameraRig>(); rig.SetPerspective(PlayerPerspective.FirstPerson);
            var phone = SmartphoneController.Instance;
            phone.hasSmartphoneInInventory = phone.arModuleUnlocked = phone.isPhoneRaised = phone.isScreenOn = true;
            phone.batteryPercent = 90; phone.currentScanMode = ScanVisualMode.Normal; phone.SyncVisualState();
            d.phoneView.Apps.Open(PhoneApp.Camera);
            CampaignState.Instance.Data.upgrades.Add("MATABATIN_AR");
            foreach (string id in new[]{"EVENT_P0_MIRROR_SCARE", "EVENT_P0_ROPE_SCARE", "EVENT_P0_CURTAIN_OPEN"}) CampaignState.Instance.AddInventoryItem(id);
            d.upper.RestoreWorld();
            Inventory("atmaja", d.transform);
            IEnumerator Shot(string name, Vector3 foot, Vector3 target)
            {
                d.PlacePlayer(foot, Quaternion.identity);
                Aim(player, target); rig.RefreshCameraPose();
                yield return null; yield return null;
                yield return Capture(eye, name);
            }
            yield return Shot("16-kitchen-window", d.P(6.9f,10.4f,.04f), d.P(5.7f,10.4f,1.5f));
            yield return Shot("20-damar-notebook", d.P(2.3f,8.0f,.54f), GameObject.Find("DamarSurveyInsideDining").transform.position);
            yield return Shot("17-well-and-kitchen", d.P(4.6f,9.8f,.36f), d.P(2.85f,11.2f,.95f));
            yield return Shot("18-dining-room", d.P(5.25f,7.8f,.54f), d.P(3.7f,4.8f,1.25f));
            yield return Shot("18-ground-doorways", d.P(-2.7f,2.8f,.54f), d.P(-3.55f,5.1f,2.6f));
            yield return Shot("23-upper-corridor", d.P(-4.8f,5.75f,3.83f), d.P(.2f,5.2f,5.35f));
            phone.isPhoneRaised=phone.isScreenOn=true;phone.SyncVisualState();
            phone.currentScanMode = ScanVisualMode.AR;
            yield return Shot("24-ar-vent", d.P(-3.0f,5.95f,3.83f), d.P(-3.0f,5.2f,4.5f));
            phone.currentScanMode = ScanVisualMode.Normal;
            yield return Shot("25-family-photo", d.P(-2.1f,5.9f,3.83f), d.P(-2.1f,5.1f,5.25f));
            yield return Shot("31-child-bedroom", d.P(-4.8f,4.65f,3.83f), d.P(-3.8f,2.1f,4.45f));
            yield return Shot("33-cup-detail",d.P(-4.92f,1.15f,3.83f),d.upper.cupAim.position);
            yield return Shot("29-notebook-lamp-cup", d.P(-5.25f,1.8f,3.83f), d.P(-5.25f,.55f,4.6f));
            yield return Shot("34-bathroom-scratches", d.P(-.7f,3.25f,3.83f), d.P(-2.285f,3.25f,4.94f));
            yield return Shot("36-blood-bathtub", d.P(-1.1f,2.8f,3.83f), d.P(-1.1f,.8f,4.2f));
            yield return Shot("37-master-room", d.P(1.2f,2.5f,3.83f), d.P(4.0f,1.8f,4.6f));
            yield return Shot("38-balcony-lamp", d.P(4.3f,4.8f,3.83f), d.P(3.2f,4.9f,6.1f));
            yield return Shot("35-floor-with-flashlight",d.P(-1.1f,3.7f,3.83f),d.P(-1.1f,2.0f,3.83f));
            var floors=Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None).Where(r=>r.name.Contains("Floor")||r.name.Contains("Tile")).ToArray();
            var floorMaterials=floors.SelectMany(r=>r.sharedMaterials).Select(m=>m.GetInstanceID()).ToArray();
            phone.isPhoneRaised=false;phone.SyncVisualState();
            yield return Capture(eye,"35-same-floor-without-flashlight");
            Assert.That(floors.SelectMany(r=>r.sharedMaterials).Select(m=>m.GetInstanceID()).ToArray(),Is.EqualTo(floorMaterials),"Lighting must never swap the floor material");
            phone.isPhoneRaised=true;phone.SyncVisualState();
            yield return Shot("42-cellar-room", d.P(1.4f,4.2f,-2.79f), d.P(4.8f,5.5f,-1.3f));
            yield return Shot("43-cellar-exit", d.P(1.35f,3.0f,-2.79f), d.P(1.025f,.69f,-.2f));
        }
        private IEnumerator Kos()
        {
            var player = Object.FindFirstObjectByType<FirstPersonController>();
            player.MovementLocked = true; UiFocus.Cinematic = false;
            var director = Object.FindFirstObjectByType<KamarKosPrologDirector>(); if(director)director.StopAllCoroutines();
            var rig = player.GetComponent<ShoulderCameraRig>(); rig.SetPerspective(PlayerPerspective.FirstPerson);
            var eye = player.GetComponent<InteractionInteractor>().playerCamera;
            Inventory("kos", player.transform);
            var fan = GameObject.Find("KosDeskFan");
            if(!fan)fan=Object.FindObjectsByType<Transform>(FindObjectsSortMode.None).FirstOrDefault(t=>t.name.Contains("DeskFan"))?.gameObject;
            Assert.That(fan, Is.Not.Null);
            var bounds = KosRoomCorrections.BoundsOf(fan);
            yield return Capture(eye, "01-bedroom-fan", () => { eye.transform.position = new Vector3(.5f,1.6f,-.8f);eye.transform.LookAt(bounds.center); });
            yield return SceneManager.LoadSceneAsync("Assets/_LayarTerakhir/assets/Scenes/Prolog_KoridorKos.unity");
            yield return new WaitForSeconds(2);
            player = Object.FindFirstObjectByType<FirstPersonController>(); player.MovementLocked = true;
            rig = player.GetComponent<ShoulderCameraRig>(); rig.SetPerspective(PlayerPerspective.FirstPerson);
            eye = player.GetComponent<InteractionInteractor>().playerCamera;
            var corridor=Object.FindFirstObjectByType<KosCorridorDirector>();
            var phone=SmartphoneController.Instance;phone.hasSmartphoneInInventory=phone.isPhoneRaised=phone.isScreenOn=true;phone.batteryPercent=90;phone.SyncVisualState();
            Inventory("corridor", player.transform);
            IEnumerator Shot(string name,Vector3 foot,Vector3 target)
            {
                var cc=player.GetComponent<CharacterController>();cc.enabled=false;player.transform.position=corridor.BuildingPoint(foot);cc.enabled=true;
                Aim(player,corridor.BuildingPoint(target));rig.RefreshCameraPose();yield return null;yield return null;yield return Capture(eye,name);
            }
            yield return Shot("03-kos-door-slippers",new Vector3(0,3.22f,5.4f),new Vector3(-1.4f,4.2f,4.5f));
            yield return Shot("04-sandal-detail",new Vector3(-.15f,3.22f,2.80f),new Vector3(-1.15f,3.33f,2.20f));
            yield return Shot("05-stair-sign",new Vector3(0,3.22f,11.2f),new Vector3(-1.4f,4.95f,12.15f));
            yield return Shot("06-exit-sign",new Vector3(0,.02f,10.5f),new Vector3(2.54f,1.85f,12.05f));
            yield return Shot("07-kitchen-dining",new Vector3(-2.0f,.02f,4.5f),new Vector3(-4.8f,1,4.5f));
            yield return Shot("08-bathroom-sign",new Vector3(2.1f,.02f,4.3f),new Vector3(2.82f,1.5f,5.2f));
            var bath=GameObject.Find("BathroomPRIADoor").GetComponent<HingedDoor>();bath.SetOpenImmediate(true);
            yield return Shot("09-bathroom-fixtures",new Vector3(3.6f,.02f,3.2f),new Vector3(5.55f,.8f,3.3f));
            yield return Shot("10-kitchen-fixtures",new Vector3(-2.2f,.02f,4.8f),new Vector3(-4.3f,.9f,3.8f));
            yield return Shot("11-parking-night",new Vector3(0,.02f,-2.4f),new Vector3(-2.7f,.8f,-5));
            var scenery=Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None).Select(r=>r.GetInstanceID()).OrderBy(i=>i).ToArray();
            LayarTerakhir.Interlude.InterludeExterior.ApplyDay(eye);
            yield return Shot("47-parking-morning",new Vector3(0,.02f,-2.4f),new Vector3(-2.7f,.8f,-5));
            yield return Shot("47-street-morning",new Vector3(0,.02f,-7.5f),new Vector3(0,3,-15));
            Assert.That(RenderSettings.skybox.name,Is.EqualTo("PrologDaySky"));
            LayarTerakhir.Interlude.InterludeExterior.ApplyDusk(eye);
            yield return Shot("47-street-dusk",new Vector3(0,.02f,-7.5f),new Vector3(0,3,-15));
            LayarTerakhir.Interlude.InterludeExterior.ApplyDay(eye);
            yield return Shot("47-street-next-morning",new Vector3(0,.02f,-7.5f),new Vector3(0,3,-15));
            Assert.That(RenderSettings.skybox.name,Is.EqualTo("PrologDaySky"));
            Assert.That(Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None).Select(r=>r.GetInstanceID()).OrderBy(i=>i).ToArray(),Is.EqualTo(scenery),"Scenery remains loaded across all lighting transitions");
        }
        private static void Aim(FirstPersonController player, Vector3 target)
        {
            Vector3 delta=target-player.cameraPivot.position;Vector3 flat=delta;flat.y=0;
            if(flat.sqrMagnitude>.0001f)player.transform.rotation=Quaternion.LookRotation(flat);
            player.SetViewPitch(-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg);
        }
        private static string Folder => "Audit/Visuals/Prolog47-2026-10-30/"+(Environment.GetEnvironmentVariable("PROLOG47_CAPTURE_LABEL")??"After");
        internal static IEnumerator Capture(Camera eye,string name,Action pose=null)
        {
            yield return RecoveryVisualEvidence.AfterPresentation(()=>
            {
                var pos=eye.transform.position;var rot=eye.transform.rotation;
                var overlays=Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Where(c=>c.isRootCanvas&&c.renderMode==RenderMode.ScreenSpaceOverlay&&c.gameObject.activeInHierarchy).ToArray();
                try
                {
                    pose?.Invoke();
                    // Editor batch mode has no regularly repainted GameView.
                    // Render the handset feed/grip textures before compositing them;
                    // reading an unrendered RenderTexture produces bogus hand shapes.
                    foreach(var dependent in Object.FindObjectsByType<Camera>(FindObjectsSortMode.None))
                        if(dependent!=eye&&dependent.enabled&&dependent.targetTexture)
                            RenderPipeline.SubmitRenderRequest(dependent,new RenderPipeline.StandardRequest{destination=dependent.targetTexture});
                    foreach(var canvas in overlays){canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=eye;canvas.planeDistance=eye.nearClipPlane+.02f;}
                    Canvas.ForceUpdateCanvases();KosRenderDiagnosis.Capture(eye,name,false,Folder);
                }
                finally
                {
                    foreach(var canvas in overlays){canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.worldCamera=null;}
                    eye.transform.SetPositionAndRotation(pos,rot);Canvas.ForceUpdateCanvases();
                }
            });
        }
        [Serializable] private class Entry { public string name,path;public Vector3 position,size;public string[] materials; }
        [Serializable] private class Survey { public string gpu,platform;public bool windowsConfirmed;public Entry[] objects; }
        private static void Inventory(string name,Transform unused)
        {
            string PathOf(Transform t)=>t.parent?PathOf(t.parent)+"/"+t.name:t.name;
            Directory.CreateDirectory(Folder);
            var entries=Object.FindObjectsByType<Renderer>(FindObjectsInactive.Include,FindObjectsSortMode.None).Where(r=>r.gameObject.scene==SceneManager.GetActiveScene()).Select(r=>new Entry{name=r.name,path=PathOf(r.transform),position=r.bounds.center,size=r.bounds.size,materials=r.sharedMaterials.Select(m=>m?m.name:"null").ToArray()}).ToArray();
            File.WriteAllText(Folder+"/"+name+"-inventory.json",JsonUtility.ToJson(new Survey{gpu=SystemInfo.graphicsDeviceName,platform=Application.platform.ToString(),windowsConfirmed=false,objects=entries},true));
        }
    }
}
#endif
