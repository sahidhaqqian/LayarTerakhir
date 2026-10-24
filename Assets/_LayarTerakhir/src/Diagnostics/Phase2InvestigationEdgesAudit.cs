#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using LayarTerakhir.Phase2;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using LayarTerakhir.UI;
namespace LayarTerakhir.Diagnostics
{
    public sealed partial class Phase2PlayerAudit
    {
        private void RestoreMorgueFixture()
        {
            Check(d.Campaign.TryCommit("PHASE2_CP3",data=>
            {data.phase2.Add("P2_KEYPAD_1603");data.phase2.Add("P2_MORGUE_ENTER");data.phase2.safe_anchor="BASEMENT";data.battery=80;}),"Explicit investigation edge fixture saves a safe CP3 without fabricated relics");
            d.RestoreWorld();d.player.enabled=false;
            d.Phone.isPhoneRaised=false;d.Phone.isScreenOn=false;d.Phone.SyncVisualState();
        }
        private IEnumerator InvestigationEdges()
        {
            RestoreMorgueFixture();FixturePose(new Vector3(27.1f,-3.97f,19.5f));
            var b2=d.world.drawers.Single(x=>x.id=="B2");float energy=d.Phone.batteryPercent;
            yield return Use("B2");yield return new WaitForSeconds(.15f);yield return Use("B2");
            Check(b2.State==Phase2DrawerState.Closed&&!d.GasActive,"B2 at -18C can be closed during real 1.5s formalin telegraph");
            yield return Use("B2");yield return new WaitForSeconds(1.7f);
            Check(b2.State==Phase2DrawerState.Gas&&d.GasActive&&!d.Dead&&Mathf.Abs(d.Phone.batteryPercent-energy)<.01f,"Warm B2 emits nonfatal gas without battery damage");
            Check(Mathf.Abs(d.player.walkSpeed-1.36f)<.1f,"Formalin reduces existing movement speed to 0.8 instead of adding health damage");
            yield return new WaitForSeconds(6.3f);Check(!d.GasActive&&!d.Dead,"Formalin dissipates after six seconds without a fatal state");
            RestoreMorgueFixture();FixturePose(new Vector3(20,-3.97f,21));Look(d.player.cameraPivot.position+Vector3.forward*4);
            yield return Wait(()=>d.lastri.Attached,30,"Real NavMesh investigation attach reaches the posed morgue player");
            Check(d.lastri.Learning,"First attach starts a nonfatal learning window");
            Look(d.lastri.Head);yield return new WaitForSeconds(.6f);
            Check(!d.Dead&&d.lastri.Learning,"Turning directly during learning does not cause POUNCE");
            Vector3 away=d.player.cameraPivot.position-d.lastri.Head;away.y=0;Look(d.player.cameraPivot.position+away);
            yield return Camera(true,false,false);yield return Walk(new Vector3(20,-3.97f,25.5f));
            float accumulated=d.lastri.HoldProgress;Check(accumulated>1&&d.lastri.Attached,"Actual forward capsule travel accumulates Selfie hold");
            yield return new WaitForSeconds(.4f);Check(Mathf.Abs(d.lastri.HoldProgress-accumulated)<.12f,"Standing still pauses rather than resets accumulated hold");
            yield return Camera(false,false,false);yield return new WaitForSeconds(.5f);
            Check(d.lastri.HoldProgress>=accumulated-.12f&&d.lastri.HoldProgress<accumulated+.12f,"Leaving Selfie frame preserves previous hold progress");
            yield return Detach();Check(!d.lastri.Learning,"First completed forward hold ends learning early");
            InputSystem.QueueStateEvent(mouse,new MouseState());d.Phone.isPhoneRaised=d.Phone.isScreenOn=false;d.Phone.SyncVisualState();
            yield return Wait(()=>d.lastri.Attached,35,"Normal cleared-learning investigation reattaches after its authored interval");
            Check(!d.lastri.Learning,"Second attach is fully armed outside learning");
            away=d.player.cameraPivot.position-d.lastri.Head;away.y=0;Look(d.player.cameraPivot.position+away);
            float attachedAt=d.Campaign.Data.phase2.elapsed_gameplay_seconds;
            yield return new WaitForSeconds(11.05f);Check(!d.Dead,"Normal unwatched attach allows the full meter rise before GRAB deadline");
            yield return Wait(()=>d.Dead,3,"Real unwatched meter plus hair-touch produces GRAB");
            float fatalAfter=d.Campaign.Data.phase2.elapsed_gameplay_seconds-attachedAt;
            Check(fatalAfter>=11.7f&&fatalAfter<=12.7f,"Normal attach GRAB occurs near 10s meter plus 2s hair, independently of the 3s telegraph");
            GameShell.Instance.Continue();yield return Wait(()=>FindFirstObjectByType<Phase2Director>()&&FindFirstObjectByType<Phase2Director>().Ready&&!FindFirstObjectByType<Phase2Director>().Dead,300,"Death/Continue restores actual CP3 scene");
            d=FindFirstObjectByType<Phase2Director>();d.player.enabled=false;float snapshot=d.Phone.batteryPercent;
            for(int retry=0;retry<10;retry++)
            {d.Phone.RestoreBattery(0);Check(d.Campaign.LoadCheckpoint(),"CP3 checkpoint read #"+(retry+1));d.RestoreWorld();Check(Mathf.Abs(d.Phone.batteryPercent-snapshot)<.001f,"CP3 retry restores exact snapshot without top-up #"+(retry+1));}
            Check(d.Campaign.TryCommit("PHASE2_CP4",data=>{data.phase2.Add("P2_STAIR_L2_UNLOCK");data.phase2.safe_anchor="NURSE";}),"L2 fixture reuses existing staircase receipt and checkpoint");
            FixturePose(new Vector3(20,4.03f,18));d.lastri.Restore();yield return Walk(new Vector3(23,4.03f,18));
            yield return Wait(()=>d.lastri.Attached,35,"Existing investigation brain attaches on L2 with its aggressive profile");
            Check(!d.world.corridorLights[1].enabled,"L2 segment actually switches off after flicker while emergency lights remain separate");
            d.Phone.RestoreBattery(0);FixturePose(new Vector3(18,4.03f,12));yield return null;yield return null;
            Check(d.CurrentRoom=="NURSE"&&!d.lastri.Attached&&!d.Dead,"Zero-energy quiet-zone crossing detaches without requiring Selfie or screen power");
        }
    }
}
#endif
