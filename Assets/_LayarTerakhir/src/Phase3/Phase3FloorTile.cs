using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public enum Phase3FloorKind { Solid, FragileFatal, FragileDrop, IllusionVoid }
    public sealed class Phase3FloorTile : MonoBehaviour
    {
        public Phase3Director director;
        public Phase3FloorKind kind;
        public Bounds footBounds;
        public GameObject surface,uvMark;
        public bool dynamicCandidate,armed=true;
        public Transform lowerLanding;
        private float telegraph,warning;
        public bool Collapsed { get; private set; }
        public bool CanArm(Vector3 player) => dynamicCandidate&&Vector3.Distance(player,footBounds.center)>=director.balance.dynamicSpawnDistance&&
            !footBounds.Contains(player)&&kind!=Phase3FloorKind.Solid;
        public void Restore(){Collapsed=false;telegraph=warning=0;armed=!dynamicCandidate;if(surface)surface.SetActive(true);if(uvMark)uvMark.SetActive(armed);}
        public void Arm(){if(!CanArm(director.player.transform.position))return;armed=true;if(uvMark)uvMark.SetActive(true);}
        public void Tick(float dt)
        {
            if(Collapsed||!armed||kind==Phase3FloorKind.Solid||kind==Phase3FloorKind.IllusionVoid||director.Completed("P3_BOSS_WIN"))return;
            var p=director.player.transform.position;
            var edge=footBounds;edge.Expand(new Vector3(1.6f,0,1.6f));
            if(edge.Contains(p+Vector3.up*.05f)&&warning<=0)
            {warning=1;director.soundscape.Cue("Creak",transform.position);}
            warning=Mathf.Max(0,warning-dt);
            bool on=footBounds.Contains(p+Vector3.up*.05f);
            if(!on){telegraph=0;return;}
            if(telegraph<=0)director.soundscape.Cue("Creak",transform.position);
            telegraph+=dt;
            bool learning=director.CurrentRoom=="MILL"&&director.Elapsed<director.MillLearningUntil;
            if(telegraph<(learning?director.balance.learnTelegraph:director.balance.fragileTelegraph))return;
            Collapsed=true;if(surface)surface.SetActive(false);if(uvMark)uvMark.SetActive(false);
            director.soundscape.Cue("Collapse",transform.position);
            if(kind==Phase3FloorKind.FragileDrop&&lowerLanding)
            {director.wewe.Hear(lowerLanding.position,10);}
            // Fatal tiles remove their real supporting collider; the authored pit catches the physical fall.
        }
    }
}
