#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Diagnostics
{
    // Deliberately posed visual comparisons. These captures do not prove traversal or frame rate.
    public sealed class GraphicsReferenceAudit : MonoBehaviour
    {
        [Serializable] private class Receipt
        {
            public string utc, gpu, method="Fixed player/camera poses, isolated save, frozen story; native framebuffer. No traversal/FPS claim.";
            public bool actualStandalonePlayer, completed;
            public int width,height;
            public string[] screenshots,errors;
        }
        private static string output;
        private readonly List<string> shots=new List<string>(),errors=new List<string>();
        private bool done;private float started;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-graphicsAudit");if(i<0||i+1>=args.Length)return;
            output=Path.GetFullPath(args[i+1]);Directory.CreateDirectory(output);
            string save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            var data=new CampaignData{checkpoint_id="PROLOG_KOS_PREPARATION",battery=95};
            data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA"});
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),JsonUtility.ToJson(data,true));
            Application.runInBackground=true;var go=new GameObject("GraphicsReferenceAudit");DontDestroyOnLoad(go);go.AddComponent<GraphicsReferenceAudit>();
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.logMessageReceived+=OnLog;}
        private void OnLog(string message,string stack,LogType type){if(type==LogType.Error||type==LogType.Exception)errors.Add(message);}
        private void Update(){if(!done&&Time.realtimeSinceStartup-started>300)Finish(false);}
        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(3);
            Time.timeScale=0;UiFocus.Cinematic=false;
            var p=FindFirstObjectByType<FirstPersonController>();
            yield return Pose(p.transform.position,p.transform.position+new Vector3(.7f,1.2f,2),PlayerPerspective.FirstPerson,false);
            yield return Shot("01-kos-room");
            Time.timeScale=1;yield return SceneManager.LoadSceneAsync(PrologSceneFlow.CorridorScene);yield return new WaitForSecondsRealtime(2);Time.timeScale=0;UiFocus.Cinematic=false;
            var corridor=FindFirstObjectByType<KosCorridorDirector>();
            yield return Pose(corridor.BuildingPoint(new Vector3(.15f,3.35f,2.7f)),corridor.BuildingPoint(new Vector3(0,4.7f,12)),PlayerPerspective.OverShoulder,false);
            yield return Shot("02-kos-corridor");
            yield return Pose(new Vector3(0,.04f,-14),new Vector3(0,1.65f,-65),PlayerPerspective.OverShoulder,false);
            yield return Shot("03-melati-road");
            yield return Pose(new Vector3(0,.04f,-14),new Vector3(-16,3,-.5f),PlayerPerspective.OverShoulder,false);
            yield return Shot("04-kos-exterior");
            yield return Pose(new Vector3(1,.04f,-22),new Vector3(8,1.7f,-24),PlayerPerspective.FirstPerson,true);
            yield return Shot("05-warung-phone");
            Time.timeScale=1;yield return SceneManager.LoadSceneAsync(PrologSceneFlow.AtmajaScene);yield return new WaitForSecondsRealtime(2);Time.timeScale=0;UiFocus.Cinematic=false;
            var d=FindFirstObjectByType<AtmajaDirector>();
            yield return Pose(d.P(0,-8,.02f),d.P(0,1.5f,3.2f),PlayerPerspective.OverShoulder,false);
            yield return Shot("06-atmaja-exterior");
            yield return Pose(d.P(5.35f,5.8f,.54f),d.P(3.7f,2.15f,1.7f),PlayerPerspective.FirstPerson,true);
            yield return Shot("07-atmaja-phone");
            yield return Pose(d.P(-2.8f,5.6f,3.9f),d.P(4.8f,5.6f,5.2f),PlayerPerspective.OverShoulder,true);
            yield return Shot("08-atmaja-upper");Finish(errors.Count==0);
        }
        private IEnumerator Pose(Vector3 position,Vector3 target,PlayerPerspective perspective,bool phone)
        {
            var p=FindFirstObjectByType<FirstPersonController>();p.enabled=false;
            var cc=p.GetComponent<CharacterController>();cc.enabled=false;p.transform.position=position;cc.enabled=true;
            var delta=target-(position+Vector3.up*1.65f);p.transform.rotation=Quaternion.LookRotation(Vector3.ProjectOnPlane(delta,Vector3.up));p.SetViewPitch(-Mathf.Atan2(delta.y,Vector3.ProjectOnPlane(delta,Vector3.up).magnitude)*Mathf.Rad2Deg);
            var rig=p.GetComponent<ShoulderCameraRig>();rig.FirstPersonOverride=false;rig.SetPerspective(perspective);rig.RefreshCameraPose();
            var device=SmartphoneController.Instance;device.hasSmartphoneInInventory=true;device.isPhoneRaised=phone;device.isScreenOn=true;device.SyncVisualState();
            var view=FindFirstObjectByType<AtmajaPhoneView>();if(phone&&view&&view.Apps)view.Apps.Open(PhoneApp.Camera);
            Physics.SyncTransforms();for(int f=0;f<8;f++)yield return null;
        }
        private IEnumerator Shot(string name)
        {yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;yield return new WaitForEndOfFrame();shots.Add(name+".png");Debug.Log("GRAPHICS_AUDIT "+name);}
        private void Finish(bool success)
        {
            if(done)return;done=true;
            File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(new Receipt{utc=DateTime.UtcNow.ToString("o"),gpu=SystemInfo.graphicsDeviceName,actualStandalonePlayer=!Application.isEditor,completed=success,width=Screen.width,height=Screen.height,screenshots=shots.ToArray(),errors=errors.ToArray()},true)+"\n");Application.Quit(success?0:1);
        }
        private void OnDestroy(){Application.logMessageReceived-=OnLog;}
    }
}
#endif
