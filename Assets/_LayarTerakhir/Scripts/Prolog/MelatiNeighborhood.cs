using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public sealed class MelatiNeighborhood : MonoBehaviour
    {
        public const string Checkpoint="PROLOG_KAMPUNG_MELATI";
        public KosCorridorDirector corridor;
        public Transform streetSpawn,parkedMotor,departure;
        public bool Busy {get;private set;}
        public bool Reached=>CampaignState.Instance&&CampaignState.Instance.HasInventoryItem("EVENT_P0_NEIGHBORHOOD_REACHED");
        public bool CanRide=>Reached&&!Busy&&corridor&&!corridor.Busy&&Vector3.Distance(corridor.player.transform.position,corridor.motor.position)<3.2f;
        private IEnumerator Start()
        {
            yield return null;yield return null;
            if(Reached&&CampaignState.Instance.Data.checkpoint_id==Checkpoint)
            {
                corridor.motor.SetPositionAndRotation(parkedMotor.position,parkedMotor.rotation);
                var cc=corridor.player.GetComponent<CharacterController>();cc.enabled=false;corridor.player.transform.SetPositionAndRotation(streetSpawn.position,streetSpawn.rotation);cc.enabled=true;
                corridor.gateHinge.GetComponent<DoubleLeafGate>()?.SetImmediate(true);
            }
        }
        public IEnumerator Arrive()
        {
            var player=corridor.player;var rig=player.GetComponent<ShoulderCameraRig>();
            if(corridor.engine)corridor.engine.Stop();
            var start=player.transform.position;player.transform.SetParent(null,true);if(rig)rig.Riding=false;
            for(float t=0;t<1;t+=Time.deltaTime/.8f)
            {player.transform.position=Vector3.Lerp(start,streetSpawn.position,Mathf.SmoothStep(0,1,t));yield return null;}
            player.transform.SetPositionAndRotation(streetSpawn.position,streetSpawn.rotation);
            player.GetComponent<CharacterController>().enabled=true;player.enabled=true;player.MovementLocked=false;
            if(rig){rig.CinematicVisible=false;rig.RefreshCameraPose();}
            UiFocus.Cinematic=false;
            CampaignState.Instance.AddInventoryItem("EVENT_P0_NEIGHBORHOOD_REACHED");CampaignState.Instance.SaveCheckpointAtomic(Checkpoint);
            HudController.Instance.EnqueueSubtitle("Raka","Warung Bu Sari masih buka. Jalan ke Atmaja lewat ujung kompleks ini...",5);
        }
        public void RequestRide(){if(CanRide)StartCoroutine(Ride());}
        private IEnumerator Ride()
        {
            Busy=true;UiFocus.Cinematic=true;UiFocus.Phone=false;
            var player=corridor.player;var rig=player.GetComponent<ShoulderCameraRig>();var phone=SmartphoneController.Instance;
            phone.isPhoneRaised=false;phone.SyncVisualState();player.MovementLocked=true;player.enabled=false;player.GetComponent<CharacterController>().enabled=false;
            player.transform.SetParent(corridor.motor,true);player.transform.localPosition=new Vector3(0,-.45f,0);player.transform.localRotation=Quaternion.identity;
            if(rig){rig.Riding=true;rig.CinematicVisible=true;}
            var camera=player.GetComponent<InteractionInteractor>().playerCamera.transform;
            if(corridor.engine){corridor.engine.clip=corridor.audioProfile.departureEngine;corridor.engine.loop=true;corridor.engine.Play();}
            var start=corridor.motor.position;var end=new Vector3(0,start.y,-104);
            for(float t=0;t<1;t+=Time.deltaTime/18)
            {
                corridor.motor.position=Vector3.Lerp(start,end,Mathf.SmoothStep(0,1,t));corridor.motor.rotation=Quaternion.LookRotation(Vector3.back);
                camera.position=corridor.motor.TransformPoint(new Vector3(-1.1f,1.7f,-2.35f));camera.LookAt(corridor.motor.position+Vector3.up*.8f);
                yield return null;
            }
            yield return ContinueToAtmaja();
        }
        public void WalkToAtmaja()
        {if(!Busy&&Reached)StartCoroutine(ContinueToAtmaja());}
        private IEnumerator ContinueToAtmaja()
        {
            Busy=true;UiFocus.Cinematic=true;corridor.player.MovementLocked=true;
            CampaignState.Instance.SaveCheckpointAtomic("PROLOG_TRAVEL_ATMAJA");
            yield return HudController.Instance.FadeBlack(0,1,1.1f);yield return PrologSceneFlow.LoadAtmaja();
        }
    }
}
