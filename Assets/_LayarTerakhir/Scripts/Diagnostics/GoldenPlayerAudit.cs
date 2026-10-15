#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LayarTerakhir.Diagnostics
{
    // Opt-in standalone smoke test. Normal play never creates this component or changes its save path.
    public sealed class GoldenPlayerAudit : MonoBehaviour
    {
        [Serializable] private sealed class Receipt
        {
            public string utc,platform,gpu,graphicsApi,scene,saveDirectory;
            public bool actualStandalonePlayer,completed,checkpointSaved,phoneVibrationStarted,phoneVibrationStopped,documentsOverflow;
            public string[] screenshots,checks;
            public int width,height;
            public float mixRms,mixPeak;
            public float wallSeconds,gameSeconds;
            public int frames;
            public bool acceleratedStoryTimers;
        }
        private static string output;
        private readonly List<string> shots=new List<string>();
        private readonly List<string> checks=new List<string>();
        private KamarKosPrologDirector director;
        private HudController hud;
        private FirstPersonController player;
        private Camera playerCamera;
        private Receipt receipt;
        private GoldenAudioTap tap;
        private bool failed,done;
        private float started;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-goldenAudit");
            if(i<0||i+1>=args.Length)return;
            output=Path.GetFullPath(args[i+1]);Directory.CreateDirectory(output);
            Application.runInBackground=true; // Xvfb has no window manager to keep the game focused.
            // Fresh isolated directory even if the supplied evidence directory already exists. Never delete any saves.
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N")));
            var go=new GameObject("GoldenPlayerAudit");DontDestroyOnLoad(go);go.AddComponent<GoldenPlayerAudit>();
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.logMessageReceived+=OnLog;}
        private void OnDestroy(){Application.logMessageReceived-=OnLog;}
        private void OnLog(string message,string trace,LogType type)
        {if(type==LogType.Error||type==LogType.Exception){failed=true;checks.Add("ERROR "+message);}}
        private void Update(){if(!done&&(failed||Time.realtimeSinceStartup-started>600))Finish(false);}
        private IEnumerator Start()
        {
            yield return null;
            director=FindFirstObjectByType<KamarKosPrologDirector>();hud=HudController.Instance;
            player=FindFirstObjectByType<FirstPersonController>();playerCamera=player.GetComponent<InteractionInteractor>().playerCamera;
            receipt=new Receipt{utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,
                graphicsApi=SystemInfo.graphicsDeviceType.ToString(),scene=SceneManager.GetActiveScene().path,actualStandalonePlayer=!Application.isEditor,
                saveDirectory=Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY"),width=Screen.width,height=Screen.height};
            Debug.Log("GOLDEN_PLAYER_AUDIT Starting; actual framebuffer "+Screen.width+"x"+Screen.height+"; GPU="+receipt.gpu);
            tap=playerCamera.gameObject.AddComponent<GoldenAudioTap>();
            yield return new WaitForSeconds(4);
            yield return Shot("01-opening");
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-goldenAuditFast")>=0)
            {
                // Only the explicit software-renderer smoke: same authored callbacks, faster story waits.
                // Never used for audio timing, manual physics or target-hardware performance acceptance.
                receipt.acceleratedStoryTimers=true;Time.timeScale=4;Time.maximumDeltaTime=2;Time.fixedDeltaTime=.08f;
            }
            yield return WaitStage(KosPrologStage.WaitReadSuratIbuKos);
            player.enabled=false; // Posed gameplay camera, not a claim of manual navigation.
            View(new Vector3(1.50f,1.6f,-1.38f),new Vector3(-.95f,.95f,.22f));
            yield return Shot("02-wide-room");
            View(new Vector3(.90f,1.5f,.20f),director.pcMonitorItem.GetComponent<Collider>().bounds.center);
            yield return Shot("03-workstation");
            View(new Vector3(1.0f,1.5f,-.15f),new Vector3(2.45f,1.5f,-.55f));
            yield return Shot("04-window");
            View(new Vector3(-.40f,1.5f,-1.45f),new Vector3(-1.65f,.36f,-.40f));
            yield return Shot("05-bedside");
            View(new Vector3(.95f,1.6f,-.75f),director.suratKosItem.GetComponent<Collider>().bounds.center);
            yield return Shot("06-letter-prompt");
            Check(ReferenceEquals(player.GetComponent<InteractionInteractor>().CurrentTarget,director.suratKosItem),"Letter is the first interaction ray hit in compact layout");
            director.suratKosItem.Interact();Check(hud.documentPanel.activeSelf,"Letter opens actual document modal");
            yield return Shot("07-letter");hud.documentCloseButton.onClick.Invoke();
            yield return WaitStage(KosPrologStage.WaitPickupSmartphone);
            var rig=director.environmentAudio;receipt.phoneVibrationStarted=rig.HasPhoneVibration;Check(receipt.phoneVibrationStarted,"Existing phone stage starts positional vibration");
            View(new Vector3(-.8f,1.6f,-.98f),director.smartphoneBedItem.GetComponent<Collider>().bounds.center);yield return null;
            Check(ReferenceEquals(player.GetComponent<InteractionInteractor>().CurrentTarget,director.smartphoneBedItem),"Bedside phone remains raycast-accessible with the stored backdrop and visible clutter");
            director.smartphoneBedItem.Interact();receipt.phoneVibrationStopped=!rig.HasPhoneVibration;Check(receipt.phoneVibrationStopped,"Phone pickup stops vibration");
            yield return new WaitForSeconds(.5f);
            var scroll=hud.GetComponent<HudPresentation>().documentScroll;
            receipt.documentsOverflow=scroll.content.rect.height>scroll.viewport.rect.height;
            Check(receipt.documentsOverflow,"Long Damar transcript exceeds viewport and is scrollable");
            yield return Shot("08-phone-document-top");scroll.verticalNormalizedPosition=0;
            yield return Shot("09-phone-document-bottom");hud.documentCloseButton.onClick.Invoke();
            yield return WaitStage(KosPrologStage.PreparationAndDrawerPuzzle);
            View(new Vector3(.80f,1.6f,.10f),new Vector3(2.12f,1.05f,1.15f));
            yield return Shot("10-donation-and-phone");
            director.lockedDrawerItem.Interact();Button("UnlockPadlockBtn").onClick.Invoke();
            Check(!director.isDrawerUnlocked&&hud.padlockPanel.activeSelf,"Wrong code stays locked");
            yield return Shot("11-wrong-code");
            var code=new[]{1,4,8};for(int slot=0;slot<3;slot++)for(int n=0;n<code[slot];n++)Button("DigitUp_"+slot).onClick.Invoke();
            yield return Shot("12-correct-code");Button("UnlockPadlockBtn").onClick.Invoke();
            Check(director.isDrawerUnlocked&&director.hasMotorKey&&director.hasSpareBattery&&!hud.IsModalOpen,"148 callback grants existing key and powerbank");
            director.jacketItem.Interact();Check(director.currentStage==KosPrologStage.ReadyToDepart,"Jacket completes preparation");
            yield return new WaitForSeconds(2.5f);
            yield return Shot("13-ready-to-depart");
            director.kosDoorItem.Interact();yield return new WaitForSeconds(4);
            receipt.checkpointSaved=CampaignState.Instance.Data.checkpoint_id=="PROLOG_KOS_COMPLETE"&&CampaignState.Instance.LoadCheckpoint();
            Check(receipt.checkpointSaved&&director.currentStage==KosPrologStage.Completed&&player.MovementLocked,"Departure locks movement and saves/loads isolated checkpoint");
            Check(hud.blackFadeImage.color.a>.99f,"Authored departure fades fully to black (not a rendering failure)");
            Finish(!failed);
        }
        private Button Button(string name)=>Array.Find(hud.padlockPanel.GetComponentsInChildren<Button>(true),b=>b.name==name);
        private IEnumerator WaitStage(KosPrologStage stage)
        {float until=Time.time+30;while(director.currentStage!=stage&&Time.time<until)yield return null;Check(director.currentStage==stage,"Stage "+stage);}
        private void Check(bool pass,string label){checks.Add((pass?"PASS ":"FAIL ")+label);Debug.Log("GOLDEN_PLAYER_AUDIT "+checks[checks.Count-1]);if(!pass)failed=true;}
        private void View(Vector3 eye,Vector3 target)
        {
            var controller=player.GetComponent<CharacterController>();controller.enabled=false;
            player.transform.position=new Vector3(eye.x,.03f,eye.z);playerCamera.transform.position=eye;playerCamera.transform.LookAt(target);
            Physics.SyncTransforms();
        }
        private IEnumerator Shot(string name)
        {
            yield return new WaitForSeconds(.35f);yield return new WaitForEndOfFrame();
            string path=Path.Combine(output,name+".png");ScreenCapture.CaptureScreenshot(path);
            yield return null;yield return null;shots.Add(Path.GetFileName(path));
        }
        private void Finish(bool success)
        {
            if(done)return;done=true;StopAllCoroutines();
            if(receipt==null)receipt=new Receipt();
            receipt.completed=success;receipt.screenshots=shots.ToArray();receipt.checks=checks.ToArray();
            receipt.wallSeconds=Time.realtimeSinceStartup-started;receipt.gameSeconds=Time.time;receipt.frames=Time.frameCount;
            if(tap){var levels=tap.Save(Path.Combine(output,"actual-opening-mix.wav"));receipt.mixRms=levels.x;receipt.mixPeak=levels.y;}
            File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(receipt,true)+"\n");
            Debug.Log("GOLDEN_PLAYER_AUDIT completed="+success+" shots="+shots.Count+" output="+output);
            Application.Quit(success?0:1);
        }
    }
    // Captures the real listener's first 14 seconds. No audio-thread IO, allocations or synthesized stand-in.
    public sealed class GoldenAudioTap : MonoBehaviour
    {
        private float[] samples;
        private volatile int written;
        private int rate,channelCount;
        private void Awake(){rate=AudioSettings.outputSampleRate;samples=new float[rate*2*14];}
        private void OnAudioFilterRead(float[] data,int channels)
        {
            if(samples==null)return;channelCount=channels;
            int count=Math.Min(data.Length,samples.Length-written);if(count<=0)return;
            Array.Copy(data,0,samples,written,count);written+=count;
        }
        public Vector2 Save(string path)
        {
            int count=written;if(count==0)return Vector2.zero;double squared=0;float peak=0;
            using(var file=new BinaryWriter(File.Create(path)))
            {
                file.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));file.Write(36+count*2);file.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));
                file.Write(16);file.Write((short)1);file.Write((short)channelCount);file.Write(rate);file.Write(rate*channelCount*2);file.Write((short)(channelCount*2));file.Write((short)16);
                file.Write(System.Text.Encoding.ASCII.GetBytes("data"));file.Write(count*2);
                for(int i=0;i<count;i++){float s=samples[i];squared+=s*s;peak=Mathf.Max(peak,Mathf.Abs(s));file.Write((short)(Mathf.Clamp(s,-1,1)*32767));}
            }
            return new Vector2((float)Math.Sqrt(squared/count),peak);
        }
    }
}
#endif
