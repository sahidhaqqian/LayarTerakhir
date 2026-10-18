using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LayarTerakhir.Prolog
{
    // Authored interaction beats share the live handset and never replace the saved camera preference.
    public sealed class AtmajaReviewMoments : MonoBehaviour
    {
        private AtmajaDirector director;
        private GameObject installation;
        private Light installationLight;
        private SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        private void Awake() { director = GetComponent<AtmajaDirector>(); }
        private void Lock(bool locked)
        {
            director.SetBusy(locked);director.player.MovementLocked=locked;
            UiFocus.Cinematic=locked;UiFocus.Phone=false;
            director.phoneView.CinematicHandsetVisible=locked;
        }
        private IEnumerator LookAt(Vector3 target,float duration,bool requireAR=false)
        {
            var player=director.player;var start=player.transform.rotation;
            var delta=target-player.cameraPivot.position;var flat=Vector3.ProjectOnPlane(delta,Vector3.up);
            if(flat.sqrMagnitude<.001f)yield break;
            var end=Quaternion.LookRotation(flat);float pitch=-Mathf.Atan2(delta.y,flat.magnitude)*Mathf.Rad2Deg;
            float original=Mathf.DeltaAngle(0,player.cameraPivot.localEulerAngles.x);
            for(float t=0;t<duration;t+=Time.deltaTime)
            {
                // The first apparition guides the gaze, but lowering/stowing the phone interrupts it immediately.
                if(requireAR&&(!Phone.isPhoneRaised||!Phone.isScreenOn||Phone.currentScanMode!=ScanVisualMode.AR||!director.phoneView.IsCameraApp))yield break;
                float k=Mathf.SmoothStep(0,1,t/duration);player.transform.rotation=Quaternion.Slerp(start,end,k);player.SetViewPitch(Mathf.Lerp(original,pitch,k));player.GetComponent<ShoulderCameraRig>()?.RefreshCameraPose();yield return null;
            }
            player.transform.rotation=end;player.SetViewPitch(pitch);
        }
        public IEnumerator HearWell()
        {
            if(director.Busy)yield break;
            Lock(true);
            Hud.PlayPriorityWorldDialogue("Bisikan","Haus... bukakan...",3.2f,director.P(2.85f,11.2f,.25f));
            yield return LookAt(director.P(2.85f,11.2f,1.03f),.8f);
            yield return new WaitForSeconds(1.4f);Lock(false);
        }
        public IEnumerator CallDamar()
        {
            Lock(true);float call=Hud.PlayPriorityDialogue("Raka","Damar! Damar!",2.3f);
            yield return new WaitForSeconds(call);
            Hud.PlayPriorityWorldDialogue("Damar • dari lantai atas","AAARGH! Raka...!",3.5f,director.P(1.1f,5.6f,4.8f));
            yield return LookAt(director.P(1.1f,5.6f,4.8f),1.1f);
            yield return new WaitForSeconds(2.2f);
            Hud.EnqueueSubtitle("Raka","Dam?! Suaranya dari atas...",3);Lock(false);
        }
        public IEnumerator Install(GameObject damagedPhone)
        {
            Lock(true);var rig=director.player.GetComponent<ShoulderCameraRig>();
            bool first=rig&&rig.FirstPersonOverride;if(rig)rig.FirstPersonOverride=true;
            var avatar=rig&&rig.Avatar?rig.Avatar.GetComponent<AnimatedRakaAvatar>():null;
            if(avatar)avatar.Installing=true;if(rig)rig.CinematicVisible=true;
            Phone.isPhoneRaised=true;Phone.isScreenOn=true;Phone.SyncVisualState();director.phoneView.Apps.Open(PhoneApp.Camera);
            // Back out from under the table through the real capsule before handling the card.
            var local=director.LocalPosition(director.player.transform.position);
            var exit=director.P(local.x>=3.7f?5.25f:2.12f,6.18f,.54f);
            var controller=director.player.GetComponent<CharacterController>();
            for(float t=0;t<2.2f&&Vector3.ProjectOnPlane(exit-director.player.transform.position,Vector3.up).magnitude>.08f;t+=Time.deltaTime)
            {
                var delta=Vector3.ProjectOnPlane(exit-director.player.transform.position,Vector3.up);
                controller.Move(Vector3.ClampMagnitude(delta,Time.deltaTime*1.4f)+Vector3.down*.02f);
                if(rig)rig.RefreshCameraPose();yield return null;
            }
            var camera=director.player.GetComponent<InteractionInteractor>().playerCamera.transform;
            var left=rig&&rig.Avatar?System.Array.Find(rig.Avatar.GetComponentsInChildren<Transform>(true),t=>t.name=="Hand_L"):null;
            var right=rig&&rig.Avatar?System.Array.Find(rig.Avatar.GetComponentsInChildren<Transform>(true),t=>t.name=="Hand_R"):null;
            var glow=new GameObject("HandsetInstallationGlow");installationLight=glow.AddComponent<Light>();
            installationLight.type=LightType.Point;installationLight.color=new Color(.76f,.90f,1);installationLight.intensity=1.1f;installationLight.range=1.5f;installationLight.shadows=LightShadows.None;
            FrameInstallation(camera,left,right);
            var original=damagedPhone.transform.position;var rotation=damagedPhone.transform.rotation;
            var hold=left?left.position:camera.TransformPoint(new Vector3(-.14f,-.12f,.40f));
            var phoneCollider=damagedPhone.GetComponent<Collider>();if(phoneCollider)phoneCollider.enabled=false;
            for(float t=0;t<1;t+=Time.deltaTime)
            {FrameInstallation(camera,left,right);if(left)hold=left.position+director.player.transform.forward*.035f;damagedPhone.transform.position=Vector3.Lerp(original,hold,Mathf.SmoothStep(0,1,t));damagedPhone.transform.rotation=Quaternion.Slerp(rotation,Quaternion.LookRotation(-camera.forward)*Quaternion.Euler(90,0,0),t);yield return null;}
            var chip=GameObject.CreatePrimitive(PrimitiveType.Cube);chip.name="DamarMemoryCard";
            Destroy(chip.GetComponent<Collider>());chip.transform.localScale=new Vector3(.018f,.002f,.025f);
            var material=new Material(Shader.Find("Universal Render Pipeline/Lit"));material.color=new Color(.15f,.13f,.07f);chip.GetComponent<Renderer>().sharedMaterial=material;
            var contact=GameObject.CreatePrimitive(PrimitiveType.Cube);Destroy(contact.GetComponent<Collider>());contact.name="CardGoldContacts";contact.transform.SetParent(chip.transform,false);contact.transform.localScale=new Vector3(.8f,1.03f,.4f);contact.transform.localPosition=new Vector3(0,0,.22f);
            var gold=new Material(material);gold.color=new Color(.71f,.52f,.18f);contact.GetComponent<Renderer>().sharedMaterial=gold;
            for(float t=0;t<1.4f;t+=Time.deltaTime)
            {FrameInstallation(camera,left,right);float k=Mathf.SmoothStep(0,1,t/1.4f);if(left)damagedPhone.transform.position=left.position;chip.transform.position=Vector3.Lerp(hold,right?right.position+Vector3.up*.12f:camera.TransformPoint(new Vector3(.21f,-.16f,.34f)),k)+camera.up*Mathf.Sin(k*Mathf.PI)*.04f;chip.transform.rotation=camera.rotation;yield return null;}
            Destroy(chip);Destroy(material);Destroy(gold);damagedPhone.SetActive(false);
            installation=PhoneSkin.Card("MataBatinInstaller",director.phoneView.Panel.transform,new Vector2(.045f,.09f),new Vector2(.955f,.91f),new Color(.025f,.055f,.044f));
            PhoneSkin.Icon("MataBatin",installation.transform,new Vector2(.32f,.66f),new Vector2(.68f,.86f),new Color(.45f,.93f,.62f));
            UiWidgets.Text("APKName",installation.transform,"MataBatin.apk",new Vector2(.06f,.54f),new Vector2(.94f,.65f),30,TextAnchor.MiddleCenter);
            var status=UiWidgets.Text("InstallStatus",installation.transform,"Kartu SD terhubung",new Vector2(.07f,.29f),new Vector2(.93f,.48f),23,TextAnchor.MiddleCenter);
            var track=PhoneSkin.Card("InstallProgressTrack",installation.transform,new Vector2(.12f,.22f),new Vector2(.88f,.24f),new Color(.09f,.17f,.12f));
            var progress=UiWidgets.Panel("InstallProgress",track.transform,Vector2.zero,new Vector2(0,1),new Color(.35f,.87f,.52f)).GetComponent<RectTransform>();
            for(float t=0;t<3.6f;t+=Time.deltaTime)
            {FrameInstallation(camera,left,right);float k=Mathf.Clamp01(t/3.6f);progress.anchorMax=new Vector2(k,1);status.text=k<.35f?"Memeriksa paket...":k<.8f?"Menginstal aplikasi... "+Mathf.RoundToInt(k*100)+"%":"Menghubungkan Lensa AR ke Kamera...";yield return null;}
            status.text="Terpasang\nKamera terhubung";progress.anchorMax=Vector2.one;
            yield return new WaitForSeconds(.8f);Destroy(installation);installation=null;Destroy(installationLight.gameObject);installationLight=null;
            if(avatar)avatar.Installing=false;if(rig){rig.FirstPersonOverride=first;rig.CinematicVisible=false;}if(phoneCollider)phoneCollider.enabled=true;Lock(false);
        }
        private void FrameInstallation(Transform camera,Transform left,Transform right)
        {
            Vector3 hands=left&&right?(left.position+right.position)*.5f:director.player.transform.position+Vector3.up*.85f;
            float side=director.LocalPosition(director.player.transform.position).x>=3.7f?1:-1;
            Vector3 outside=director.east*side;
            camera.position=hands+outside*.58f+director.north*.16f+Vector3.up*.34f;
            camera.LookAt(hands+Vector3.up*.045f);camera.LookAt(hands+Vector3.up*.045f+camera.right*.09f);
            if(installationLight)installationLight.transform.position=hands+outside*.18f+Vector3.up*.20f;
        }
        public IEnumerator FirstARScare()
        {
            director.phoneView.Glitch(1.5f,1);
            if(director.arFigure)yield return LookAt(director.arFigure.transform.position+Vector3.up*1.18f,.45f,true);
            Hud.PlayPriorityDialogue("Raka","Siapa... yang duduk di situ?",3);
            yield return null;
        }
        private void LateUpdate()
        {
            if(!director||!director.arFigure||!director.arFigure.activeSelf)return;
            Vector3 delta=director.player.transform.position-director.arFigure.transform.position;delta.y=0;
            if(delta.sqrMagnitude>.1f)director.arFigure.transform.rotation=Quaternion.LookRotation(delta);
        }
        public IEnumerator OpenWell()
        {
            if(director.Busy)yield break;Lock(true);
            yield return LookAt(director.P(2.85f,11.2f,.9f),.5f);
            var boards=GameObject.Find("WellCover");
            if(boards)
            {
                var rotation=boards.transform.rotation;
                for(float t=0;t<1;t+=Time.deltaTime){boards.transform.rotation=rotation*Quaternion.Euler(-95*Mathf.SmoothStep(0,1,t),0,0);yield return null;}
                boards.transform.rotation=rotation*Quaternion.Euler(-95,0,0);
            }
            Hud.PlayPriorityDialogue("Raka","Apa...?! Lepas!",3);director.phoneView.Glitch(4,1);
            var cc=director.player.GetComponent<CharacterController>();cc.enabled=false;
            var start=director.player.transform.position;var rim=director.P(2.85f,11.2f,1.02f);var inside=director.P(2.85f,11.2f,-3.8f);
            var rig=director.player.GetComponent<ShoulderCameraRig>();if(rig)rig.FirstPersonOverride=true;
            for(float t=0;t<2.2f;t+=Time.deltaTime)
            {float k=t/2.2f;director.player.transform.position=k<.4f?Vector3.Lerp(start,rim,k/.4f):Vector3.Lerp(rim,inside,Mathf.Pow((k-.4f)/.6f,1.6f));director.player.SetViewPitch(Mathf.Lerp(35,-60,k));if(rig)rig.RefreshCameraPose();yield return null;}
            director.player.transform.position=inside;director.player.SetViewPitch(-60);if(rig)rig.RefreshCameraPose();
            yield return Hud.FadeBlack(0,1,.35f);
            director.FailToCheckpoint("PROLOG_ATMAJA_KITCHEN","Sesuatu menarik Raka ke dasar sumur.");
        }
        private void OnDestroy(){if(installation)Destroy(installation);if(installationLight)Destroy(installationLight.gameObject);}
    }
}
