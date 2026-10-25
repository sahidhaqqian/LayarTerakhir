using System.Collections;
using UnityEngine;
namespace LayarTerakhir.Phase5
{
    public sealed class Phase5UnstablePlank : MonoBehaviour
    {
        public Phase5Director director;
        private bool triggered;
        private void Update()
        {
            if(triggered||!director||!director.ClockRunning||director.Zone!="S2"||director.Completed("P5_S2_CLEAR"))return;
            Vector3 at=director.player.transform.position-transform.position;
            if(Mathf.Abs(at.x)<.75f&&Mathf.Abs(at.z)<.8f&&Mathf.Abs(at.y)<1)StartCoroutine(Collapse());
        }
        private IEnumerator Collapse()
        {
            triggered=true;director.Hud.EnqueueSubtitle("Papan jembatan","[Papan ber-rajah berderit dan mulai retak. Menyingkir!]",3);
            float time=0;while(time<director.balance.plankTelegraph){if(director.ClockRunning)time+=Time.deltaTime;yield return null;}
            var colliders=GetComponentsInChildren<Collider>();var renderers=GetComponentsInChildren<Renderer>();
            foreach(var c in colliders)c.enabled=false;foreach(var r in renderers)r.enabled=false;
            float fall=0;
            while(fall<3&&director.player.transform.position.y>-2.5f){if(director.ClockRunning)fall+=Time.deltaTime;yield return null;}
            if(director.player.transform.position.y<-1.5f)
            {director.Penalty(director.balance.plankLoss,"Papan runtuh. Tambatan terakhir masih aman.");director.MovePlayer(director.world.Anchor("S2"));}
            yield return new WaitForSeconds(3);
            foreach(var c in colliders)c.enabled=true;foreach(var r in renderers)r.enabled=true;triggered=false;
        }
    }
}
