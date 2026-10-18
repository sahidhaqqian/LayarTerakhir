using System.Collections;
using LayarTerakhir.Core;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    // The neighborhood is part of the kos grounds, reached on foot before travel.
    public sealed class MelatiNeighborhood : MonoBehaviour
    {
        public const string Checkpoint="PROLOG_KAMPUNG_MELATI";
        public KosCorridorDirector corridor;
        public Transform streetSpawn,parkedMotor,departure;
        public bool Busy {get;private set;}
        public bool Reached=>CampaignState.Instance&&CampaignState.Instance.HasInventoryItem("EVENT_P0_NEIGHBORHOOD_REACHED");
        private IEnumerator Start()
        {
            yield return null;yield return null;
            if(CampaignState.Instance.Data.checkpoint_id!=Checkpoint)yield break;
            var cc=corridor.player.GetComponent<CharacterController>();cc.enabled=false;
            corridor.player.transform.SetPositionAndRotation(streetSpawn.position,streetSpawn.rotation);cc.enabled=true;
        }
        private void Update()
        {
            if(Reached||Busy||!corridor||corridor.Busy||corridor.player.transform.position.z> -12)return;
            CampaignState.Instance.AddInventoryItem("EVENT_P0_NEIGHBORHOOD_REACHED");
            CampaignState.Instance.SaveCheckpointAtomic(Checkpoint);
        }
        public void WalkToAtmaja(){if(!Busy)StartCoroutine(ContinueToAtmaja());}
        private IEnumerator ContinueToAtmaja()
        {
            Busy=true;UiFocus.Cinematic=true;corridor.player.MovementLocked=true;
            CampaignState.Instance.SaveCheckpointAtomic("PROLOG_TRAVEL_ATMAJA");
            yield return LayarTerakhir.UI.HudController.Instance.FadeBlack(0,1,1.1f);
            yield return PrologSceneFlow.LoadAtmaja();
        }
    }
}
