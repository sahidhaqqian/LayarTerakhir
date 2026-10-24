using UnityEngine;
namespace LayarTerakhir.Phase2
{
    public enum Phase2DrawerState { Closed, Telegraph, Gas, Spent }
    public sealed class Phase2Drawer : MonoBehaviour
    {
        public Phase2Director director;
        public string id;
        public int temperature;
        public Transform tray;
        public Vector3 slideOffset = Vector3.back * .65f;
        public Phase2DrawerState State { get; private set; }
        private float timer, blend;
        private Vector3 closed;
        private bool captured;
        public string Prompt => id=="C3" ? director.Completed("P2_C3_OPEN") ? "C3 — laci aman terbuka" :
            director.Completed("P2_DRAWER_CHAIN_BREAK") ? "C3 — tarik laci pendingin" : "C3 — rantai beku, periksa sensor" :
            State==Phase2DrawerState.Telegraph ? id+" — TUTUP LACI, desis formalin!" : id+" — tarik laci pendingin";
        public void Restore()
        {
            if (!captured && tray) { closed=tray.localPosition;captured=true; }
            State=Phase2DrawerState.Closed; timer=blend=0;
            if (tray) tray.localPosition=closed;
        }
        public void Interact()
        {
            if(director.Completed("P2_BOSS_WIN"))return;
            if (id=="C3")
            { if (director.Completed("P2_DRAWER_CHAIN_BREAK")) director.Commit("P2_C3_OPEN",null,"PHASE2_CP4",director.SafeAnchor); return; }
            if (State==Phase2DrawerState.Telegraph) { State=Phase2DrawerState.Closed;timer=0;return; }
            if (State!=Phase2DrawerState.Closed) return;
            State=Phase2DrawerState.Telegraph;timer=0;
            director.soundscape?.Cue("Hiss",transform.position);
        }
        public void Tick(float dt)
        {
            if (id=="C3") { blend=Mathf.MoveTowards(blend,director.Completed("P2_C3_OPEN")?1:0,dt*2); }
            else
            {
                timer+=dt;
                if (State==Phase2DrawerState.Telegraph && timer>=director.balance.formalinTelegraph)
                { State=Phase2DrawerState.Gas;timer=0;director.ApplyFormalin(transform.position); }
                if (State==Phase2DrawerState.Gas && timer>=director.balance.formalinSeconds) State=Phase2DrawerState.Spent;
                blend=Mathf.MoveTowards(blend,State==Phase2DrawerState.Closed?0:1,dt*2);
            }
            if (tray) tray.localPosition=closed+slideOffset*Mathf.SmoothStep(0,1,blend);
        }
    }
}
