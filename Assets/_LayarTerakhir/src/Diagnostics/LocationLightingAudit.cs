#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Phase1;
using LayarTerakhir.Phase2;
using LayarTerakhir.Phase3;
using LayarTerakhir.Phase4;
using LayarTerakhir.Phase5;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Diagnostics
{
    // Posed, opt-in rendering review. This does not claim traversal/puzzle QA.
    public sealed class LocationLightingAudit : MonoBehaviour
    {
        [Serializable] private sealed class Frame { public string scene, screenshot; public bool phone, beamEnabled; public int unpoweredFixtures; public float meanLuminance; }
        [Serializable] private sealed class Receipt { public bool completed; public string scope="Posed standalone lighting review; directors paused, private save; not a traversal or FPS test"; public Frame[] frames; public string[] errors, fixtureNotes; }
        private static string output;
        private static string requestedScene;
        private readonly List<Frame> frames=new List<Frame>();
        private readonly List<string> errors=new List<string>();
        private readonly List<string> fixtureNotes=new List<string>();
        private float started;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int at=Array.IndexOf(args,"-lightingAudit");if(at<0||at+1>=args.Length)return;
            output=Path.GetFullPath(args[at+1]);Directory.CreateDirectory(output);
            int sceneAt=Array.IndexOf(args,"-lightingScene");requestedScene=sceneAt>=0&&sceneAt+1<args.Length?args[sceneAt+1]:null;
            if(requestedScene!=null&&!AbandonedLocationLighting.IsAbandoned(requestedScene))throw new ArgumentException("Not an abandoned review scene: "+requestedScene);
            string save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            var data=new CampaignData{checkpoint_id="PROLOG_KOS_YARD",battery=95};data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","JAKET_RAKA"});
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),JsonUtility.ToJson(data,true));
            var go=new GameObject("LocationLightingAudit");DontDestroyOnLoad(go);go.AddComponent<LocationLightingAudit>();Application.runInBackground=true;
        }
        private void Awake(){started=Time.realtimeSinceStartup;SceneManager.sceneLoaded+=PauseStory;Application.logMessageReceived+=OnLog;}
        private void OnLog(string message,string stack,LogType type)
        {
            // Paused directors do not register their runtime navigation. Keep
            // this exact fixture limitation visible; never present it as AI QA.
            if(message=="Failed to create agent because there is no valid NavMesh")
            {fixtureNotes.Add(message+" (paused story fixture; navigation is not under test)");return;}
            if(type==LogType.Error||type==LogType.Exception)errors.Add(message);
        }
        private void PauseStory(Scene scene,LoadSceneMode mode)
        {
            foreach(var root in scene.GetRootGameObjects())foreach(var script in root.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if(!script)continue;string name=script.GetType().Name;
                if(name.EndsWith("Director")||name.EndsWith("Threats")||name.EndsWith("Brain"))script.enabled=false;
            }
        }
        private IEnumerator Start()
        {
            while(!FindFirstObjectByType<FirstPersonController>())yield return null;
            foreach(string scene in requestedScene!=null?new[]{requestedScene}:new[]{"Prolog_RumahAtmaja","Babak1_Rusunawa","Babak2_RSUD","Babak3_PabrikGula","Babak4_KarangSetra","Babak5_PasarSetan"})
            {
                yield return SceneManager.LoadSceneAsync(scene);yield return null;yield return null;
                var player=FindFirstObjectByType<FirstPersonController>();var rig=player.GetComponent<ShoulderCameraRig>();
                var phone=SmartphoneController.Instance;var policy=player.GetComponent<AbandonedLocationLighting>();
                Transform target=null;
                if(scene=="Babak1_Rusunawa")target=FindFirstObjectByType<Phase1Director>().world.evpPoint;
                if(scene=="Babak2_RSUD")target=FindFirstObjectByType<Phase2Director>().world.evpTarget;
                if(scene=="Babak3_PabrikGula")target=FindFirstObjectByType<Phase3Director>().world.slingTarget;
                if(scene=="Babak4_KarangSetra")target=FindFirstObjectByType<Phase4Director>().world.wellMantra;
                if(scene=="Babak5_PasarSetan")target=FindFirstObjectByType<Phase5Director>().world.contract;
                var cc=player.GetComponent<CharacterController>();cc.enabled=false;
                if(target&&NavMesh.SamplePosition(target.position-Vector3.forward*2, out var hit,4,NavMesh.AllAreas))player.transform.position=hit.position+Vector3.up*.05f;
                cc.enabled=true;Physics.SyncTransforms();
                UiFocus.Menu=UiFocus.Clock=UiFocus.Cinematic=false;player.MovementLocked=true;
                if(target){Vector3 direction=target.position-(player.transform.position+Vector3.up*1.5f);player.transform.rotation=Quaternion.Euler(0,Quaternion.LookRotation(direction).eulerAngles.y,0);player.SetViewPitch(-Mathf.Atan2(direction.y,new Vector2(direction.x,direction.z).magnitude)*Mathf.Rad2Deg);}
                rig.SetPerspective(PlayerPerspective.FirstPerson);rig.RefreshCameraPose();
                var view=FindFirstObjectByType<AtmajaPhoneView>();if(view)view.enabled=false;
                foreach(var canvas in FindObjectsByType<Canvas>(FindObjectsSortMode.None))canvas.enabled=false;
                phone.hasSmartphoneInInventory=true;phone.isScreenOn=true;
                foreach(bool on in new[]{false,true})
                {
                    phone.isPhoneRaised=on;phone.SyncVisualState();policy.Configure();policy.EnforceOutage();
                    for(int i=0;i<4;i++)yield return new WaitForEndOfFrame();
                    var image=ScreenCapture.CaptureScreenshotAsTexture();string name=scene+(on?"-phone-on.png":"-phone-off.png");
                    double sum=0;foreach(var pixel in image.GetPixels32())sum+=(.2126*pixel.r+.7152*pixel.g+.0722*pixel.b)/255;
                    File.WriteAllBytes(Path.Combine(output,name),image.EncodeToPNG());
                    bool beam=phone.phoneFlashlight&&phone.phoneFlashlight.enabled;
                    if(beam!=on)errors.Add(scene+" flashlight did not follow phone state "+on);
                    frames.Add(new Frame{scene=scene,screenshot=name,phone=on,beamEnabled=beam,unpoweredFixtures=policy.UnpoweredLightCount,meanLuminance=(float)(sum/(image.width*image.height))});Destroy(image);
                }
            }
            Finish(true);
        }
        private void Update(){if(Time.realtimeSinceStartup-started>600)Finish(false);}
        private void Finish(bool success){success=success&&errors.Count==0;File.WriteAllText(Path.Combine(output,"lighting-review.json"),JsonUtility.ToJson(new Receipt{completed=success,frames=frames.ToArray(),errors=errors.ToArray(),fixtureNotes=fixtureNotes.ToArray()},true)+"\n");Application.Quit(success?0:1);enabled=false;}
        private void OnDestroy(){SceneManager.sceneLoaded-=PauseStory;Application.logMessageReceived-=OnLog;}
    }
}
#endif
