#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Diagnostics
{
    // Targeted visual fixture. The full opening audit separately covers real travel selection.
    public sealed class RiderPresentationAudit : MonoBehaviour
    {
        [Serializable] private class Receipt
        {
            public string utc,platform,gpu,navigationMethod;
            public bool actualStandalonePlayer,completed,checkpointSaved;
            public int width,height;
            public string[] checks,screenshots;
            public Vector3 playerFeet,motorOrigin,avatarOrigin,headPosition;
        }
        private static string output;
        private readonly List<string> checks=new List<string>(),shots=new List<string>();
        private Receipt receipt;private bool done;private float started;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-riderAudit");if(i<0||i+1>=args.Length)return;
            output=Path.GetFullPath(args[i+1]);Directory.CreateDirectory(output);
            string save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            var data=new CampaignData{checkpoint_id="PROLOG_KOS_YARD",battery=80,wallet_bound=5000000};
            data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA","EVENT_P0_CONTRACT_RECEIVED","EVENT_P0_CONTRACT_READ"});
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),JsonUtility.ToJson(data,true));
            Application.runInBackground=true;
            Time.maximumDeltaTime=.5f;
            var go=new GameObject("RiderPresentationAudit");DontDestroyOnLoad(go);go.AddComponent<RiderPresentationAudit>();
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.logMessageReceived+=OnLog;}
        private void OnLog(string message,string trace,LogType type){if(type==LogType.Exception||type==LogType.Error){checks.Add("ERROR "+message);Finish(false);}}
        private void Update(){if(!done&&Time.realtimeSinceStartup-started>300){checks.Add("FAIL rider fixture timeout");Finish(false);}}
        private IEnumerator Start()
        {
            receipt=new Receipt{utc=DateTime.UtcNow.ToString("o"),platform=Application.platform.ToString(),gpu=SystemInfo.graphicsDeviceName,
                width=Screen.width,height=Screen.height,actualStandalonePlayer=!Application.isEditor,
                navigationMethod="Isolated completed-kos-yard save; pose player beside motor, invoke the existing contextual departure, capture the real mount/ride and arrival. Max delta time .5s for the software-rendering host. Targeted rider presentation fixture, not a manual/full opening playthrough."};
            yield return Wait(()=>FindFirstObjectByType<KosCorridorDirector>()!=null,60,"Yard checkpoint routes into the kos corridor");if(done)yield break;
            yield return new WaitForSeconds(1);
            var d=FindFirstObjectByType<KosCorridorDirector>();var p=d.player;var cc=p.GetComponent<CharacterController>();
            cc.enabled=false;p.transform.position=d.motor.position+d.motor.right*.9f;
            p.transform.position=new Vector3(p.transform.position.x,.08f,p.transform.position.z);cc.enabled=true;
            p.transform.rotation=d.motor.rotation;yield return null;
            Check(d.CanDepart,"Contextual travel accepts the player beside the motor");d.RequestDeparture();
            yield return new WaitForSeconds(3.2f);
            var rig=p.GetComponent<ShoulderCameraRig>();var eye=p.GetComponent<InteractionInteractor>().playerCamera;
            yield return Wait(()=>rig.Riding,10,"The real mount enters the authored rider pose");if(done)yield break;
            yield return new WaitForSeconds(.3f);
            Transform head=rig.Avatar.GetComponentsInChildren<Transform>(true).First(t=>t.name=="Head");
            receipt.playerFeet=p.transform.position;receipt.motorOrigin=d.motor.position;receipt.avatarOrigin=rig.Avatar.position;receipt.headPosition=head.position;
            Check(rig.CinematicVisible&&UiFocus.Cinematic,"Motor cinematic owns the camera while showing Raka");
            Check(head.position.y>d.motor.position.y+1,"The seated head clears the motor instead of sinking into the saddle");
            var view=eye.WorldToViewportPoint(head.position);Check(view.z>eye.nearClipPlane&&view.x>0&&view.x<1&&view.y>0&&view.y<1,"Rider head is inside the actual shot");
            var toHead=head.position-eye.transform.position;
            Check(Physics.RaycastAll(eye.transform.position,toHead.normalized,toHead.magnitude,~(1<<ShoulderCameraRig.AvatarLayer),QueryTriggerInteraction.Ignore).All(h=>h.transform.IsChildOf(d.motor)||h.transform.IsChildOf(p.transform)),"Porch columns and walls do not obstruct the mounted rider shot");
            Check(rig.Avatar.GetComponentsInChildren<Renderer>().Where(r=>!r.name.StartsWith("RakaTShirt")).All(r=>r.enabled&&r.shadowCastingMode==UnityEngine.Rendering.ShadowCastingMode.On),"Rider renderers are visible during the cinematic");
            var bones=rig.Avatar.GetComponentsInChildren<Transform>(true);
            var hips=bones.First(t=>t.name=="Hips");var seat=d.motor.GetComponentsInChildren<Renderer>().First(r=>r.name.Contains("SeatVinyl"));
            Check(Mathf.Abs(hips.position.y-seat.bounds.max.y-.075f)<.04f,"The pelvis sits on the actual closed saddle");
            foreach(string side in new[]{"L","R"})
            {
                var wrist=bones.First(t=>t.name=="Hand_"+side);var grips=d.motor.GetComponentsInChildren<Transform>().Where(t=>t.name.StartsWith("RiderGrip")).ToArray();
                var body=d.motor.GetComponentsInChildren<MeshFilter>().First(f=>f.name.Contains("DarkPlastic"));
                // The Blender receipt verifies triangle-surface distances. Player mesh CPU data stays disabled.
                Check(grips.Length==2&&Vector3.Distance(grips[0].position,grips[1].position)>.35f&&grips.All(g=>body.GetComponent<Renderer>().bounds.Contains(g.position)),"Two authored grip contacts remain inside the visible control assembly");
                Check(grips.Min(g=>Vector3.Distance(wrist.position,g.position))<.14f,"Riding "+side+" hand reaches a physical handlebar grip");
            }
            yield return Shot("01-mounted-rider");
            yield return Wait(()=>d&&d.motor.position.z<-7.5f,30,"The rider and motorcycle pass through the kos gate");if(done)yield break;
            yield return Shot("02-rider-through-gate");
            Check(d.Busy,"The same departure continues directly to Atmaja without dismounting in Melati");
            yield return Shot("03-continuous-road-departure");
            yield return Wait(()=>SceneManager.GetActiveScene().name==PrologSceneFlow.AtmajaScene,120,"Travel arrives in Atmaja");if(done)yield break;
            yield return new WaitForSeconds(1.7f);
            var arrival=FindFirstObjectByType<FirstPersonController>().GetComponent<ShoulderCameraRig>();
            Check(!arrival.Riding&&arrival.Avatar.localPosition==Vector3.zero,"Arrival restores the normal walking model origin");
            receipt.checkpointSaved=CampaignState.Instance.Data.checkpoint_id=="PROLOG_ATMAJA_COURTYARD";
            Check(receipt.checkpointSaved,"Arrival saves the real Atmaja courtyard checkpoint");yield return Shot("04-arrival-standing");Finish(true);
        }
        private IEnumerator Wait(Func<bool> condition,float seconds,string label)
        {float until=Time.realtimeSinceStartup+seconds;while(!done&&!condition()&&Time.realtimeSinceStartup<until)yield return null;if(!done)Check(condition(),label);}
        private IEnumerator Shot(string name)
        {float scale=Time.timeScale;Time.timeScale=0;yield return null;yield return new WaitForEndOfFrame();ScreenCapture.CaptureScreenshot(Path.Combine(output,name+".png"));yield return null;yield return new WaitForEndOfFrame();Time.timeScale=scale;shots.Add(name+".png");}
        private void Check(bool value,string label){checks.Add((value?"PASS ":"FAIL ")+label);Debug.Log("RIDER_AUDIT "+checks[checks.Count-1]);if(!value)Finish(false);}
        private void Finish(bool success)
        {if(done)return;done=true;if(receipt!=null){receipt.completed=success;receipt.checks=checks.ToArray();receipt.screenshots=shots.ToArray();File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(receipt,true)+"\n");}Application.Quit(success?0:1);}
        private void OnDestroy(){Application.logMessageReceived-=OnLog;}
    }
}
#endif
