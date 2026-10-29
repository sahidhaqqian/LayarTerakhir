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
        public Transform buildingRoot;
        public Vector3 BuildingPoint(Vector3 local)=>buildingRoot?buildingRoot.TransformPoint(local):local;
        public HingedDoor entranceDoor;
        public bool Busy {get;private set;}
        private bool savedGround;
        public bool CanDepart=>!Busy&&player&&player.transform.position.y<.8f&&
            (Vector3.Distance(player.transform.position,motor.position)<3.2f||Vector3.Distance(player.transform.position,BuildingPoint(new Vector3(0,0,-7)))<3.2f)&&
            CampaignState.Instance.HasInventoryItem("KUNCI_MOTOR")&&CampaignState.Instance.HasInventoryItem("JAKET_RAKA");
        private void Awake()=>LayarTerakhir.Interlude.InterludeExterior.EnsureNeighborhood(this);
        private IEnumerator Start()
        {
            var boot=CampaignState.Instance;boot.LoadCheckpoint();
            if(LayarTerakhir.Interlude.InterludeIds.IsInterludeData(boot.Data))
            {gameObject.AddComponent<LayarTerakhir.Interlude.InterludeDirector>();enabled=false;yield break;}            PlayerFootsteps.Install(player,audioProfile);
            PracticalLightPresentation.ConfigureBeam(motorHeadlight);
            PrologSceneFlow.RestoreBindings();var c=CampaignState.Instance;c.LoadCheckpoint();
            if(!c.HasInventoryItem("SMARTPHONE_RAKA"))
            {c.StartNewGame();foreach(var id in new[]{"SMARTPHONE_RAKA","KUNCI_MOTOR","JAKET_RAKA","POWERBANK_KECIL_KOS"})c.AddInventoryItem(id);c.Data.wallet_bound=5000000;}
            var phone=SmartphoneController.Instance;phone.hasSmartphoneInInventory=true;phone.batteryPercent=c.Data.battery;phone.isPhoneRaised=false;phone.isScreenOn=phone.batteryPercent>0;phone.SyncVisualState();
            savedGround=c.Data.checkpoint_id=="PROLOG_KOS_YARD"||c.Data.checkpoint_id=="PROLOG_TRAVEL_ATMAJA"||c.Data.checkpoint_id==MelatiNeighborhood.Checkpoint;
            gateHinge.GetComponent<DoubleLeafGate>()?.SetImmediate(true);
            var spawn=savedGround?spawnGround:spawnLanding;var cc=player.GetComponent<CharacterController>();cc.enabled=false;player.transform.SetPositionAndRotation(spawn.position,spawn.rotation);cc.enabled=true;
            if(c.Data.checkpoint_id!=MelatiNeighborhood.Checkpoint)c.SaveCheckpointAtomic(savedGround?"PROLOG_KOS_YARD":"PROLOG_KOS_CORRIDOR");
            HudController.Instance.SetFadeAlpha(1);yield return HudController.Instance.FadeBlack(1,0,.45f);
            HudController.Instance.SetObjective("Turun ke lantai bawah, lalu temui motor di gerbang kos. [M] Peta tujuan di HP.");
            if(rain&&rain.clip)rain.Play();
        }
        private void Update()
        {
            if(Busy||!player||UiFocus.HardModal)return;
            if(player.transform.position.y< -2){StartCoroutine(RecoverInsideBuilding());return;}
            if(!savedGround&&player.transform.position.y<.5f&&player.GetComponent<CharacterController>().isGrounded)
            {savedGround=CampaignState.Instance.SaveCheckpointAtomic("PROLOG_KOS_YARD");HudController.Instance.SetObjective("Keluar ke halaman. Periksa peta dan berangkat dari motor/gerbang kos.");}
        }
        private IEnumerator RecoverInsideBuilding()
        {
            Busy=true;player.MovementLocked=true;yield return HudController.Instance.FadeBlack(0,1,.2f);
            var spawn=savedGround?spawnGround:spawnLanding;var cc=player.GetComponent<CharacterController>();cc.enabled=false;
            player.transform.SetPositionAndRotation(spawn.position,spawn.rotation);cc.enabled=true;
            yield return HudController.Instance.FadeBlack(1,0,.3f);player.MovementLocked=false;Busy=false;
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
            var shoulder=player.GetComponent<ShoulderCameraRig>();if(shoulder)shoulder.CinematicVisible=true;
            var start=player.transform.position;var target=motor.position+motor.right*-.55f;
            for(float t=0;t<1;t+=Time.deltaTime/.9f){player.transform.position=Vector3.Lerp(start,target,Mathf.SmoothStep(0,1,t));player.transform.rotation=Quaternion.Slerp(player.transform.rotation,motor.rotation,t);yield return null;}
            var camera=player.GetComponent<InteractionInteractor>().playerCamera.transform;
            var mountStart=camera.position;var mountEnd=motor.TransformPoint(new Vector3(-1.1f,1.7f,-2.35f));
            for(float t=0;t<1;t+=Time.deltaTime/1.5f)
            {camera.position=Vector3.Lerp(mountStart,mountEnd,Mathf.SmoothStep(0,1,t))+Vector3.up*Mathf.Sin(t*Mathf.PI)*.18f;camera.LookAt(motor.position+Vector3.up*.8f);yield return null;}
            player.transform.SetParent(motor,true);player.transform.localRotation=Quaternion.identity;player.transform.localPosition=new Vector3(0,-.45f,0);if(shoulder)shoulder.Riding=true;
            camera.position=mountEnd;camera.LookAt(motor.position+Vector3.up*.8f);
            // First-person riding: the OTS chase cam is replaced by Raka's own
            // viewpoint with his jacket sleeves on the handlebar. The authored
            // riding pose reads unnaturally from behind; from the saddle only
            // the road, the headlight cone and his hands stay visible.
            if(shoulder){shoulder.FirstPersonOverride=true;shoulder.RefreshCameraPose();}
            AddRiderArms(camera);
            if(motorHeadlight)motorHeadlight.enabled=true;if(engine){engine.transform.SetParent(motor,true);engine.transform.position=motor.position;engine.clip=audioProfile.departureEngine;engine.loop=true;engine.pitch=.8f;engine.Play();}
            var pairedGate=gateHinge.GetComponent<DoubleLeafGate>();
            if(pairedGate)yield return pairedGate.Open();
            else{var from=gateHinge.localRotation;for(float t=0;t<1;t+=Time.deltaTime/1.3f){gateHinge.localRotation=Quaternion.Slerp(from,Quaternion.Euler(0,100,0),t);yield return null;}}
            Vector3[] route={motor.position,BuildingPoint(new Vector3(0,motor.position.y,-5.4f)),BuildingPoint(new Vector3(0,motor.position.y,-12)),new Vector3(0,motor.position.y,-12),rideExit.position};
            float total=0;for(int i=1;i<route.Length;i++)total+=Vector3.Distance(route[i-1],route[i]);
            for(float t=0;t<1;t+=Time.deltaTime/8)
            {
                float distance=Mathf.SmoothStep(0,1,t)*total;int segment=1;
                while(segment<route.Length-1&&distance>Vector3.Distance(route[segment-1],route[segment])){distance-=Vector3.Distance(route[segment-1],route[segment]);segment++;}
                Vector3 direction=route[segment]-route[segment-1];motor.position=Vector3.Lerp(route[segment-1],route[segment],distance/Mathf.Max(.001f,direction.magnitude));
                motor.rotation=Quaternion.Slerp(motor.rotation,Quaternion.LookRotation(direction.normalized),Time.deltaTime*4);
                camera.position=motor.TransformPoint(new Vector3(-.55f,1.62f,.55f));camera.LookAt(motor.TransformPoint(new Vector3(-.55f,1.0f,10f)));
                if(engine)engine.pitch=Mathf.Lerp(.8f,1.35f,t);yield return null;
            }
            for(float t=0;t<1;t+=Time.deltaTime/.6f){camera.position=Vector3.Lerp(camera.position,motor.TransformPoint(new Vector3(-.55f,1.62f,.55f)),t);camera.LookAt(motor.TransformPoint(new Vector3(-.55f,1.0f,10f)));yield return null;}
            foreach(Transform child in camera.transform)
            {
                if(child.name=="RakaJacketSleeve"||child.name=="RakaHandOnHandlebar")Object.Destroy(child.gameObject);
            }
            if(shoulder){shoulder.FirstPersonOverride=false;shoulder.Riding=false;shoulder.RefreshCameraPose();}
            CampaignState.Instance.SaveCheckpointAtomic("PROLOG_TRAVEL_ATMAJA");
            yield return HudController.Instance.FadeBlack(0,1,1.1f);
            yield return PrologSceneFlow.LoadAtmaja();
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
