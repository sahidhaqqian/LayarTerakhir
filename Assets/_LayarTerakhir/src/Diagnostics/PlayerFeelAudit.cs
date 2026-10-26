#if UNITY_EDITOR || DEVELOPMENT_BUILD
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
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Diagnostics
{
    // Opt-in presentation fixture only. Uses real input/CharacterController in the
    // existing street. A fixed simulation delta makes response measurable on llvmpipe;
    // this is not a hardware FPS or unassisted playtest claim.
    public sealed class PlayerFeelAudit : MonoBehaviour
    {
        [Serializable] private class Sample
        {
            public string segment,clip;public float time,speed,height,eyeHeight,phoneRaise,phase,inputMagnitude,commandSpeed;
        }
        [Serializable] private class Receipt
        {
            public string utc,gpu,method="Actual keyboard input and CharacterController in existing Melati; isolated synthetic devices ignore desktop focus; fixed 1/30 simulation delta. No hardware FPS claim.";
            public bool completed,actualStandalonePlayer,checkpointSaved;
            public float standingFootClearance;
            public float crouchBodyHeight,crouchBottom;
            public float crouchWalkMaximumFootError,crouchWalkMaximumHeadroomOverage;
            public float lowCrouchBodyHeight,lowWalkBodyHeight,lowPhoneBodyHeight,lowCrouchBottom,lowWalkBottom,lowPhoneBottom;
            public int width,height;public string[] screenshots,checks;public Sample[] samples;
        }
        private static string output;
        private float measuredSupportY;
        private string measuredLowestBone;
        private readonly List<string> checks=new List<string>(),shots=new List<string>();
        private readonly List<Sample> samples=new List<Sample>();
        private FirstPersonController player;private ShoulderCameraRig rig;private AtmajaPhoneView view;
        private Keyboard keyboard;private Mouse mouse;private Vector3 previous;private bool done;private float started,clock,normalBodyHeight,normalBottom,normalWalkFootError,normalWalkHeadroomOverage,lowBodyHeight,lowWalkHeight,lowPhoneHeight,lowBottom,lowWalkBottom,lowPhoneBottom,standingBottom,measuredHeadroom;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var args=Environment.GetCommandLineArgs();int at=Array.IndexOf(args,"-feelAudit");if(at<0||at+1>=args.Length)return;
            output=Path.GetFullPath(args[at+1]);Directory.CreateDirectory(output);
            string save=Path.Combine(output,"IsolatedSave-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(save);
            Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",save);
            var data=new CampaignData{checkpoint_id="PROLOG_KOS_YARD",battery=95};data.inventory.AddRange(new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA"});
            File.WriteAllText(Path.Combine(save,"layar_terakhir_save.json"),JsonUtility.ToJson(data,true));
            var go=new GameObject("PlayerFeelAudit");DontDestroyOnLoad(go);go.AddComponent<PlayerFeelAudit>();Application.runInBackground=true;
        }
        private void Awake(){started=Time.realtimeSinceStartup;Application.logMessageReceived+=OnLog;}
        private void OnLog(string text,string stack,LogType type){if(!done&&(type==LogType.Error||type==LogType.Exception)){checks.Add("FAIL "+text);Finish(false);}}
        private void Update(){if(!done&&Time.realtimeSinceStartup-started>900){checks.Add("FAIL Fixture timeout");Finish(false);}}
        private IEnumerator Start()
        {
            while(SceneManager.GetActiveScene().name!=PrologSceneFlow.CorridorScene)yield return null;
            yield return new WaitForSecondsRealtime(2);
            // The headless window may not own desktop focus. Isolate only this opt-in
            // fixture's devices; normal play retains the project's input/focus policy.
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            foreach(var device in InputSystem.devices)if(device is Keyboard||device is Mouse)InputSystem.DisableDevice(device);
            keyboard=InputSystem.AddDevice<Keyboard>();mouse=InputSystem.AddDevice<Mouse>();
            InputSystem.EnableDevice(keyboard);InputSystem.EnableDevice(mouse);keyboard.MakeCurrent();mouse.MakeCurrent();
            Time.timeScale=1;Time.captureDeltaTime=1f/30;
            player=FindFirstObjectByType<FirstPersonController>();rig=player.GetComponent<ShoulderCameraRig>();view=FindFirstObjectByType<AtmajaPhoneView>();
            FindFirstObjectByType<KosCorridorDirector>().enabled=false;
            var cc=player.GetComponent<CharacterController>();cc.enabled=false;player.transform.position=new Vector3(0,.04f,-14);player.transform.rotation=Quaternion.Euler(0,180,0);cc.enabled=true;Physics.SyncTransforms();
            UiFocus.Menu=UiFocus.Clock=UiFocus.Cinematic=false;player.MovementLocked=false;player.SetViewPitch(8);rig.SetPerspective(PlayerPerspective.OverShoulder);
            Check(player.enabled&&cc.enabled&&GameInputConfig.Instance&&!HudController.Instance.IsModalOpen,"Runtime controller, input configuration and non-modal world are ready");
            Check(keyboard.enabled&&Keyboard.current==keyboard,"Injected keyboard is enabled and current");
            var phone=SmartphoneController.Instance;phone.hasSmartphoneInInventory=true;phone.isPhoneRaised=false;phone.isScreenOn=true;phone.SyncVisualState();view.Apps.Open(PhoneApp.Camera);
            previous=player.transform.position;
            yield return Frames("settle",12);
            var handsetParts=rig.Avatar.GetComponentsInChildren<Renderer>(true).Where(r=>r.name=="HandheldPhone"||r.name=="HandsetScreen").ToArray();
            Check(handsetParts.Length==2&&handsetParts.All(r=>!r.gameObject.activeInHierarchy),"Stowed handset hides both actual case and glass; no floating phone part beside the empty hand");
            standingBottom=VisibleBodyBounds().x;
            yield return Portrait("00-raka-standing-front-review-light",false);
            Check(Mathf.Abs(standingBottom)<.025f,"Standing avatar feet meet the actual road surface without moving the capsule");
            yield return Frames("walk-start",3,Key.W);yield return Shot("01-walk-start");
            yield return Frames("walk",25,Key.W);
            Check(samples.Exists(s=>s.segment=="walk"&&s.speed>1.65f),"Walking reaches the original speed cap");
            yield return Shot("02-walk-stride");
            yield return Frames("walk-stop",9);yield return Shot("03-walk-stop");
            yield return Frames("run",30,Key.W,Key.LeftShift);yield return Shot("04-running");
            yield return Frames("run-stop",9);
            yield return Frames("crouch-down",15,Key.LeftCtrl);yield return Shot("05-crouch-settled");
            yield return Portrait("05b-raka-crouch-front-review-light",true);
            var normalBody=VisibleBodyBounds();normalBodyHeight=normalBody.y;normalBottom=normalBody.x;
            Check(normalBodyHeight>.6f&&normalBodyHeight<measuredHeadroom+.005f&&Mathf.Abs(normalBottom)<.025f,"Normal crouch mesh fits its actual capsule headroom and road surface");
            yield return Frames("crouch-walk",25,Key.LeftCtrl,Key.W);yield return Shot("06-crouch-stride");
            Check(normalWalkFootError<.025f&&normalWalkHeadroomOverage<.005f,"Every sampled normal crouch stride stays on the road and inside capsule headroom");
            yield return Frames("stand-up",15);yield return Shot("07-standing-again");
            Check(!player.IsCrouching&&Mathf.Abs(cc.height-player.standingHeight)<.02f,"Crouch release restores the existing standing capsule");
            Check(Mathf.Abs(player.walkSpeed-1.7f)<.001f&&Mathf.Abs(player.sprintSpeed-4.2f)<.001f&&Mathf.Abs(player.crouchSpeed-.8f)<.001f,"Existing walk/run/crouch speed caps retained");
            // Exercise the existing Atmaja dimensions without making another scene.
            float height=player.crouchHeight,eyeHeight=player.cameraCrouchY;
            player.crouchHeight=.78f;player.cameraCrouchY=.62f;
            yield return Frames("low-crouch",15,Key.LeftCtrl);yield return Shot("06a-atmaja-low-crouch");
            var lowBounds=VisibleBodyBounds();lowBodyHeight=lowBounds.y;lowBottom=lowBounds.x;
            Check(cc.height<.80f&&player.cameraPivot.localPosition.y<.72f&&lowBodyHeight>.6f&&lowBodyHeight<measuredHeadroom+.005f&&Mathf.Abs(lowBottom)<.025f,"Atmaja low capsule has a matching crouch mesh, floor and eye height");
            yield return Frames("low-crouch-walk",25,Key.LeftCtrl,Key.W);yield return Shot("06b-atmaja-low-stride");
            Check(samples.FindLast(s=>s.segment=="low-crouch-walk").clip=="LowCrouchWalk","Low Atmaja movement uses its authored deep crouch clip");
            var walkBounds=VisibleBodyBounds();lowWalkHeight=walkBounds.y;lowWalkBottom=walkBounds.x;
            Check(lowWalkHeight>.6f&&lowWalkHeight<measuredHeadroom+.005f&&Mathf.Abs(lowWalkBottom)<.025f,"Baked low-crouch stride stays on the floor and below capsule headroom");
            yield return Frames("low-phone-raise",1,Key.LeftCtrl,Key.Tab);yield return Frames("low-phone-hold",12,Key.LeftCtrl);yield return Shot("06c-atmaja-low-phone");
            var phoneBounds=VisibleBodyBounds();lowPhoneHeight=phoneBounds.y;lowPhoneBottom=phoneBounds.x;
            Check(phone.isPhoneRaised&&view.FeedCamera.enabled&&lowPhoneHeight>.6f&&lowPhoneHeight<measuredHeadroom+.005f&&Mathf.Abs(lowPhoneBottom)<.025f,"Low crouch keeps the held phone below capsule headroom and feet on the floor");
            yield return Frames("low-phone-stow",12,Key.Tab);
            player.crouchHeight=height;player.cameraCrouchY=eyeHeight;yield return Frames("low-stand-up",15);
            yield return Frames("phone-raise",1,Key.Tab);yield return Shot("08-phone-raising");
            yield return Frames("phone-hold",12);yield return Shot("09-phone-shoulder");
            Check(phone.isPhoneRaised&&view.RaiseAmount>.99f&&view.FeedCamera.enabled,"Phone reaches its held pose and keeps the camera feed");
            Check(rig.Avatar.GetComponent<AnimatedRakaAvatar>().handset.activeInHierarchy,"World handset is visible once the authored grasp has settled");
            Check(handsetParts.All(r=>r.gameObject.activeInHierarchy),"Held handset shows its case and glass together");
            Check(Vector3.Cross(view.FeedCamera.transform.forward,(rig.AimPoint-view.FeedCamera.transform.position).normalized).magnitude<.01f,"Shoulder framing and phone share the physical aim line");
            yield return Frames("perspective",1,Key.V);yield return Frames("phone-first-person",10);yield return Shot("10-phone-first-person");
            Check(rig.IsFirstPerson&&Vector3.Distance(player.GetComponent<InteractionInteractor>().playerCamera.transform.position,player.cameraPivot.position)<.01f,"First-person still uses the player's eyes");
            var hand=player.GetComponent<InteractionInteractor>().playerCamera.transform.Find("ExistingHandsetDepthMask");
            Check(hand&&hand.gameObject.activeInHierarchy,"First-person displays the authored Raka hand");
            // The supplied VRM has one combined forearm/skin surface instead of
            // the previous model's separate jacket sleeve renderer.
            var sleeve=Array.Find(hand.GetComponentsInChildren<Renderer>(),r=>r.enabled&&(r.name.Contains("Jacket")||r.name=="PhoneHand_VisibleSkin"));
            var gripCamera=player.GetComponent<InteractionInteractor>().playerCamera.transform.Find("RakaPhoneGripCamera").GetComponent<Camera>();
            var sleevePosition=sleeve?gripCamera.WorldToViewportPoint(sleeve.bounds.center):Vector3.zero;
            Check(sleeve&&sleevePosition.z>0&&sleevePosition.x>.65f&&sleevePosition.y<.7f,"First-person sleeve enters from the lower right and leaves the sightline clear");
            view.Apps.Open(PhoneApp.Journal);
            InputSystem.QueueStateEvent(mouse,new MouseState{buttons=2});
            yield return Frames("hold-focus",12);yield return Shot("10a-held-landscape-viewfinder");
            Check(view.ViewfinderActive&&view.IsCameraApp,"Held right mouse forces the open journal into the camera");
            var focusRect=view.Panel.GetComponent<RectTransform>();
            float framedWidth=(focusRect.anchorMax.x-focusRect.anchorMin.x)*Screen.width;
            float framedHeight=(focusRect.anchorMax.y-focusRect.anchorMin.y)*Screen.height;
            Check(framedWidth>Screen.width*.5f&&focusRect.anchorMin.x>0&&focusRect.anchorMax.x<1&&Mathf.Abs(framedWidth/framedHeight-AtmajaPhoneView.LandscapeHandsetAspect)<.03f,"Focused camera has a readable landscape frame and space for the physical hands without stretching the handset");
            view.Apps.Open(PhoneApp.Inventory);yield return Frames("focus-overrides-menu",2);
            Check(view.IsCameraApp&&view.ViewfinderActive,"Held aim remains authoritative over app changes");
            InputSystem.QueueStateEvent(mouse,new MouseState());yield return Frames("release-focus",4);yield return Shot("10b-release-restores-journal");
            Check(!view.ViewfinderActive&&phone.isPhoneRaised&&view.Apps.CurrentApp==PhoneApp.Journal,"Releasing right mouse restores the prior handset and application");
            view.Apps.Open(PhoneApp.Camera);
            HudController.Instance.OpenDocumentModal("Presentation fixture","Document/phone continuity check.",()=>{});
            yield return Frames("document",5);yield return Shot("11-document-keeps-phone");
            Check(phone.isPhoneRaised&&view.Panel.activeSelf&&view.FeedCamera.enabled,"Reading preserves the held phone and live feed");HudController.Instance.CloseDocumentModal();
            yield return Frames("phone-lower",1,Key.Tab);yield return Shot("12-phone-lowering");
            yield return Frames("phone-stowed",12);Check(!phone.isPhoneRaised&&!view.Panel.activeSelf&&!rig.Avatar.GetComponent<AnimatedRakaAvatar>().handset.activeSelf,"Phone lowers completely without retaining input focus");
            Check(samples.Exists(s=>s.segment=="walk-start"&&s.speed>.01f&&s.speed<1.65f),"Actual walk input accelerates below the speed cap");
            Check(samples.Exists(s=>s.segment=="walk"&&s.speed>1.65f),"Walking reaches the original speed cap");
            Check(samples.Exists(s=>s.segment=="run"&&s.speed>4.1f),"Running reaches the original speed cap");
            Check(samples.FindLast(s=>s.segment=="run-stop").speed<.03f,"Run braking settles within nine simulation frames");
            Check(samples.Exists(s=>s.segment=="crouch-walk"&&s.speed>.7f&&s.height<1.05f),"Crouch movement uses the existing low capsule and speed");
            Check(CampaignState.Instance.SaveCheckpointAtomic(CampaignState.Instance.Data.checkpoint_id)&&CampaignState.Instance.LoadCheckpoint(),"Checkpoint save/load remains available after presentation changes");
            Finish(true);
        }
        private IEnumerator Frames(string segment,int count,params Key[] keys)
        {
            keyboard.MakeCurrent();InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
            for(int i=0;i<count;i++)
            {
                yield return null;yield return new WaitForEndOfFrame();
                if(segment=="crouch-walk")
                {
                    var body=VisibleBodyBounds();normalWalkFootError=Mathf.Max(normalWalkFootError,Mathf.Abs(body.x));
                    normalWalkHeadroomOverage=Mathf.Max(normalWalkHeadroomOverage,body.y-measuredHeadroom);
                    if(Mathf.Abs(body.x)>.02f)Debug.Log($"CROUCH_CONTACT frame={i} bottom={body.x} bone={measuredLowestBone} rootY={player.transform.position.y} supportY={measuredSupportY} visualOffset={rig.VisualGroundOffset} scale={rig.Avatar.lossyScale.y}");
                }
                var cc=player.GetComponent<CharacterController>();Vector3 delta=player.transform.position-previous;delta.y=0;previous=player.transform.position;
                string clip="";float weight=-1,phase=0;var anim=rig.Avatar.GetComponent<Animation>();
                foreach(AnimationState state in anim)if(state.enabled&&state.weight>weight){weight=state.weight;clip=state.name;phase=state.normalizedTime;}
                clock+=Time.deltaTime;samples.Add(new Sample{segment=segment,time=clock,speed=delta.magnitude/Mathf.Max(.001f,Time.deltaTime),height=cc.height,eyeHeight=player.cameraPivot.localPosition.y,phoneRaise=view.RaiseAmount,clip=clip,phase=phase,inputMagnitude=GameInputConfig.Instance?GameInputConfig.Instance.GetMovementInput().magnitude:0,commandSpeed=player.PlanarVelocity.magnitude});
            }
        }
        private IEnumerator Shot(string name)
        {
            // Frames already yielded EndOfFrame. Capture that exact rendered state,
            // without consuming another unscaled UI-transition frame on llvmpipe.
            var image=ScreenCapture.CaptureScreenshotAsTexture();
            File.WriteAllBytes(Path.Combine(output,name+".png"),image.EncodeToPNG());Destroy(image);
            shots.Add(name+".png");yield return null;yield return new WaitForEndOfFrame();previous=player.transform.position;
        }
        private IEnumerator Portrait(string name,bool crouched)
        {
            // Opt-in geometry review only; the normal route captures retain
            // real gameplay lighting. Never install this light in normal play.
            var camera=Camera.main;var position=camera.transform.position;var rotation=camera.transform.rotation;
            bool active=rig.enabled,locked=player.MovementLocked;
            var canvases=FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            var states=canvases.Select(c=>c.enabled).ToArray();
            rig.enabled=false;player.MovementLocked=true;
            foreach(var canvas in canvases)canvas.enabled=false;
            Vector3 focus=rig.Avatar.position+Vector3.up*(crouched?.55f:1.05f);
            camera.transform.position=focus+rig.Avatar.forward*(crouched?2.0f:2.7f)+rig.Avatar.right*.45f+Vector3.up*.12f;
            camera.transform.LookAt(focus);
            var lightObject=new GameObject("FixtureOnly_RakaPortraitKey");var light=lightObject.AddComponent<Light>();
            light.transform.position=camera.transform.position;light.type=LightType.Point;light.intensity=1.5f;light.range=4;
            yield return new WaitForEndOfFrame();yield return new WaitForEndOfFrame();
            var image=ScreenCapture.CaptureScreenshotAsTexture();File.WriteAllBytes(Path.Combine(output,name+".png"),image.EncodeToPNG());Destroy(image);shots.Add(name+".png");
            Destroy(lightObject);camera.transform.SetPositionAndRotation(position,rotation);
            for(int i=0;i<canvases.Length;i++)canvases[i].enabled=states[i];
            rig.enabled=active;player.MovementLocked=locked;previous=player.transform.position;
        }

        private Vector2 VisibleBodyBounds()
        {
            // Use the actual support surface, not the capsule root (which includes skin clearance).
            RaycastHit ground=default;bool found=false;
            foreach(var hit in Physics.RaycastAll(player.transform.position+Vector3.up*.25f,Vector3.down,2,~0,QueryTriggerInteraction.Ignore))
                if(!hit.transform.IsChildOf(player.transform)&&hit.normal.y>.5f&&(!found||hit.distance<ground.distance)){ground=hit;found=true;}
            if(!found)throw new InvalidOperationException("No support surface under player-feel fixture");
            measuredSupportY=ground.point.y;measuredHeadroom=player.GetComponent<CharacterController>().bounds.max.y-ground.point.y;
            float top=0,bottom=float.PositiveInfinity;var baked=new Mesh();
            SkinnedMeshRenderer lowestRenderer=null;int lowestVertex=0;
            foreach(var renderer in rig.Avatar.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if(!renderer.enabled||!renderer.gameObject.activeInHierarchy)continue;
                // Compensate imported FBX renderer scale before TransformPoint.
                renderer.BakeMesh(baked,true);
                var vertices=baked.vertices;
                for(int i=0;i<vertices.Length;i++){float y=renderer.transform.TransformPoint(vertices[i]).y-ground.point.y;top=Mathf.Max(top,y);if(y<bottom){bottom=y;lowestRenderer=renderer;lowestVertex=i;}}
                baked.Clear();
            }
            if(lowestRenderer)measuredLowestBone=lowestRenderer.bones[lowestRenderer.sharedMesh.boneWeights[lowestVertex].boneIndex0].name;
            Destroy(baked);return new Vector2(bottom,top);
        }
        // Collect independent presentation failures in one route. A failed
        // check still fails the receipt/exit code; fatal exceptions stop early.
        private void Check(bool passed,string text){checks.Add((passed?"PASS ":"FAIL ")+text);}
        private void Finish(bool success)
        {
            if(done)return;success=success&&!checks.Any(c=>c.StartsWith("FAIL "));done=true;
            File.WriteAllText(Path.Combine(output,"player-audit.json"),JsonUtility.ToJson(new Receipt{utc=DateTime.UtcNow.ToString("o"),gpu=SystemInfo.graphicsDeviceName,completed=success,actualStandalonePlayer=!Application.isEditor,checkpointSaved=success,standingFootClearance=standingBottom,crouchBodyHeight=normalBodyHeight,crouchBottom=normalBottom,crouchWalkMaximumFootError=normalWalkFootError,crouchWalkMaximumHeadroomOverage=normalWalkHeadroomOverage,lowCrouchBodyHeight=lowBodyHeight,lowWalkBodyHeight=lowWalkHeight,lowPhoneBodyHeight=lowPhoneHeight,lowCrouchBottom=lowBottom,lowWalkBottom=lowWalkBottom,lowPhoneBottom=lowPhoneBottom,width=Screen.width,height=Screen.height,screenshots=shots.ToArray(),checks=checks.ToArray(),samples=samples.ToArray()},true)+"\n");
            Time.captureDeltaTime=0;Application.Quit(success?0:1);
        }
        private void OnDestroy(){Application.logMessageReceived-=OnLog;}
    }
}
#endif
