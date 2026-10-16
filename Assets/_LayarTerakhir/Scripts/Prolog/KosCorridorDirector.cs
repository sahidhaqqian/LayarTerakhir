using System.Collections;
using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Prolog
{
    public sealed class KosCorridorDirector:MonoBehaviour
    {
        public FirstPersonController player;public Transform spawnLanding,spawnGround,motor,gateHinge,rideAnchor,rideExit;
        public AudioSource engine,rain;public SceneAudioProfile audioProfile;
        public Light motorHeadlight;
        public bool Busy {get;private set;}
        private bool savedGround;
        public bool CanDepart=>!Busy&&player&&player.transform.position.y<.8f&&
            (Vector3.Distance(player.transform.position,motor.position)<3.2f||Vector3.Distance(player.transform.position,new Vector3(0,0,-7))<3.2f)&&
            CampaignState.Instance.HasInventoryItem("KUNCI_MOTOR")&&CampaignState.Instance.HasInventoryItem("JAKET_RAKA");
        private IEnumerator Start()
        {
            PrologSceneFlow.RestoreBindings();var c=CampaignState.Instance;c.LoadCheckpoint();
            if(!c.HasInventoryItem("SMARTPHONE_RAKA"))
            {c.StartNewGame();foreach(var id in new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA","POWERBANK_KECIL_KOS"})c.AddInventoryItem(id);c.Data.wallet_bound=5000000;}
            var phone=SmartphoneController.Instance;phone.hasSmartphoneInInventory=true;phone.batteryPercent=c.Data.battery;phone.isPhoneRaised=false;phone.isScreenOn=phone.batteryPercent>0;phone.SyncVisualState();
            savedGround=c.Data.checkpoint_id=="PROLOG_KOS_YARD"||c.Data.checkpoint_id=="PROLOG_TRAVEL_ATMAJA";
            var spawn=savedGround?spawnGround:spawnLanding;var cc=player.GetComponent<CharacterController>();cc.enabled=false;player.transform.SetPositionAndRotation(spawn.position,spawn.rotation);cc.enabled=true;
            c.SaveCheckpointAtomic(savedGround?"PROLOG_KOS_YARD":"PROLOG_KOS_CORRIDOR");
            HudController.Instance.SetFadeAlpha(1);yield return HudController.Instance.FadeBlack(1,0,.45f);
            HudController.Instance.SetObjective("Turun ke lantai bawah, lalu temui motor di gerbang kos. [M] Peta tujuan di HP.");
            if(rain&&rain.clip)rain.Play();
        }
        private void Update()
        {
            if(Busy||!player||UiFocus.HardModal)return;
            if(!savedGround&&player.transform.position.y<.5f)
            {savedGround=CampaignState.Instance.SaveCheckpointAtomic("PROLOG_KOS_YARD");HudController.Instance.SetObjective("Keluar ke halaman. Periksa peta dan berangkat dari motor/gerbang kos.");}
        }
        public void OpenMap()
        {
            if(Busy)return;
            var phone=SmartphoneController.Instance;
            if(CanDepart&&phone.batteryPercent<1){phone.batteryPercent=35;CampaignState.Instance.Data.battery=35;HudController.Instance.ShowBroadcastNotification("USB motor • daya darurat 35%",3);}
            var view=FindFirstObjectByType<AtmajaPhoneView>();if(view&&view.Apps)view.Apps.OpenTravelMap();
        }
        public void RequestDeparture()
        {if(!CanDepart)return;StartCoroutine(RideToAtmaja());}
        private IEnumerator RideToAtmaja()
        {
            Busy=true;UiFocus.Cinematic=true;var phone=SmartphoneController.Instance;phone.isPhoneRaised=false;phone.SyncVisualState();UiFocus.Phone=false;
            player.MovementLocked=true;player.enabled=false;var cc=player.GetComponent<CharacterController>();cc.enabled=false;
            var start=player.transform.position;var target=motor.position+motor.right*-.55f;
            for(float t=0;t<1;t+=Time.deltaTime/.9f){player.transform.position=Vector3.Lerp(start,target,Mathf.SmoothStep(0,1,t));player.transform.rotation=Quaternion.Slerp(player.transform.rotation,motor.rotation,t);yield return null;}
            var camera=player.GetComponent<InteractionInteractor>().playerCamera.transform;
            var mountStart=camera.position;var mountEnd=rideAnchor.position;
            for(float t=0;t<1;t+=Time.deltaTime/1.5f)
            {camera.position=Vector3.Lerp(mountStart,mountEnd,Mathf.SmoothStep(0,1,t))+Vector3.up*Mathf.Sin(t*Mathf.PI)*.18f;camera.rotation=motor.rotation*Quaternion.Euler(14,0,Mathf.Sin(t*Mathf.PI)*-12);yield return null;}
            player.transform.SetParent(motor,true);player.transform.localRotation=Quaternion.identity;camera.SetPositionAndRotation(rideAnchor.position,motor.rotation*Quaternion.Euler(5,0,0));
            AddRiderArms(camera);if(motorHeadlight)motorHeadlight.enabled=true;if(engine){engine.transform.SetParent(motor,true);engine.transform.position=motor.position;engine.clip=audioProfile.departureEngine;engine.loop=true;engine.pitch=.8f;engine.Play();}
            var from=gateHinge.localRotation;
            for(float t=0;t<1;t+=Time.deltaTime/1.3f){gateHinge.localRotation=Quaternion.Slerp(from,Quaternion.Euler(0,100,0),t);yield return null;}
            var motorStart=motor.position;var end=rideExit.position;
            for(float t=0;t<1;t+=Time.deltaTime/6)
            {
                // A visible moving motorcycle actually passes the kos gate before the scene changes.
                var turn=Vector3.Lerp(motorStart,new Vector3(0,motorStart.y,-5.4f),Mathf.Clamp01(t*3));
                motor.position=t<.33f?turn:Vector3.Lerp(new Vector3(0,motorStart.y,-5.4f),end,Mathf.SmoothStep(0,1,(t-.33f)/.67f));
                motor.rotation=Quaternion.Slerp(motor.rotation,Quaternion.LookRotation(Vector3.back),Time.deltaTime*3);
                if(engine)engine.pitch=Mathf.Lerp(.8f,1.35f,t);yield return null;
            }
            CampaignState.Instance.SaveCheckpointAtomic("PROLOG_TRAVEL_ATMAJA");yield return HudController.Instance.FadeBlack(0,1,1.1f);yield return PrologSceneFlow.LoadAtmaja();
        }
        private static void AddRiderArms(Transform camera)
        {
            for(int side=-1;side<=1;side+=2)
            {
                var sleeve=GameObject.CreatePrimitive(PrimitiveType.Capsule);sleeve.name="RakaJacketSleeve";sleeve.transform.SetParent(camera,false);sleeve.transform.localPosition=new Vector3(side*.23f,-.32f,.40f);sleeve.transform.localRotation=Quaternion.Euler(60,side*-10,side*-15);sleeve.transform.localScale=new Vector3(.11f,.25f,.11f);Destroy(sleeve.GetComponent<Collider>());
                var m=new Material(Shader.Find("Universal Render Pipeline/Lit"));m.color=new Color(.035f,.045f,.06f);sleeve.GetComponent<Renderer>().material=m;
                var hand=GameObject.CreatePrimitive(PrimitiveType.Sphere);hand.name="RakaHandOnHandlebar";hand.transform.SetParent(camera,false);hand.transform.localPosition=new Vector3(side*.27f,-.26f,.64f);hand.transform.localScale=new Vector3(.075f,.055f,.10f);Destroy(hand.GetComponent<Collider>());var skin=new Material(m){color=new Color(.30f,.19f,.12f)};hand.GetComponent<Renderer>().material=skin;
            }
        }
    }
}
