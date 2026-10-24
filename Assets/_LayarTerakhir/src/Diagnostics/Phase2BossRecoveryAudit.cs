#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase2;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Diagnostics
{
    public sealed partial class Phase2PlayerAudit
    {
        // Explicit isolated scenario fixtures. Pose changes are labeled; these are not traversal evidence.
        private void FixturePose(Vector3 position)
        {var cc=d.player.GetComponent<CharacterController>();cc.enabled=false;d.player.transform.position=position;cc.enabled=true;}
        private IEnumerator BossRecoveryScenarios()
        {
            Check(d.Campaign.Data.inventory.Count(x=>x.StartsWith("P2_FLARE_"))==3,"Explicit recovery fixture has three test flares without a shop/economy grant");
            FixturePose(new Vector3(46,4.03f,22));
            Check(!d.boss.Begin()&&d.Campaign.Data.checkpoint_id=="PHASE2_CP4","Calling table from outside hand range cannot create CP5");
            FixturePose(d.world.Anchor("OR_PREP").position);yield return Use("RITUAL_TABLE");
            yield return Walk(new Vector3(46.4f,4.03f,21.5f));yield return HoldUse("GENERATOR_NE",8.5f);
            yield return Walk(new Vector3(39.4f,4.03f,18));yield return Wait(()=>d.boss.Remaining<=0,45,"Recovery fixture waits actual preparation clock");
            yield return Use("RITUAL_TABLE");yield return Camera(true,false,false);float before=d.Phone.batteryPercent;
            yield return PressMouse();yield return null;
            Check(d.boss.Layers==0&&d.Phone.batteryPercent<before-14.9f&&!d.boss.Spent.Any(x=>x),"Shielded Blitz costs global 15 points without seal or lamp loss");
            Check(d.boss.State==Phase2BossState.PowerRecovery&&!d.Dead,"Low energy during TRACK enters safe POWER_RECOVERY before any fatal");
            yield return Camera(true,true,false);yield return new WaitForSeconds(.5f);
            Check(d.boss.Recovering,"Raising Selfie and Thermo never silently ends recovery");
            d.Phone.isScreenOn=false;d.Phone.SyncVisualState();
            yield return Walk(new Vector3(46.4f,4.03f,21.5f));before=d.Phone.batteryPercent;yield return HoldUse("GENERATOR_NE",8.5f);
            Check(Mathf.Abs(d.Phone.batteryPercent-(before+25))<1,"Previously used arena generator remains repeatable above exploration cap10");
            yield return Walk(new Vector3(39.4f,4.03f,18));yield return Wait(()=>d.boss.Remaining<=0,35,"Recovery retains minimum time and conscious continuation");yield return Use("RITUAL_TABLE");
            FixturePose(new Vector3(44.5f,4.03f,18));d.lastri.agent.Warp(new Vector3(42,4.03f,17));
            yield return Use("LAMP_0");Check(d.boss.Exposed,"Posed real geometry fixture opens LO exposure through E");
            yield return Camera(false,false,false);Look(d.player.cameraPivot.position+Vector3.right*4);yield return null;before=d.Phone.batteryPercent;
            yield return PressMouse();float after=d.Phone.batteryPercent;yield return PressMouse();
            Check(d.boss.Layers==0&&Mathf.Abs(after-(before-15))<.2f&&Mathf.Abs(d.Phone.batteryPercent-after)<.2f,"Rear shot facing away misses; rapid second click obeys shared 6s cooldown");
            float window=d.boss.Remaining;GameShell.Instance.Pause();yield return new WaitForSecondsRealtime(.4f);
            Check(Mathf.Abs(window-d.boss.Remaining)<.08f,"Pause freezes the actual remaining five-second window");GameShell.Instance.Resume();
            yield return new WaitForSeconds(6);
            Check(d.boss.State==Phase2BossState.Recovery&&!d.boss.Spent[0]&&d.boss.Layers==0,"Miss/timeout restores shield while preserving environmental light");
            yield return Walk(new Vector3(39.4f,4.03f,18));yield return Wait(()=>d.boss.Remaining<=0,35,"Timeout recovery can be resumed instead of exhausting a lamp");
            typeof(Phase2Boss).GetProperty("Layers").SetValue(d.boss,1); // explicit cycle-2 identification fixture, never a disk receipt.
            yield return Use("RITUAL_TABLE");Debug.Log("P2_DRILL_CYCLE state="+d.boss.State+" energy="+d.Phone.batteryPercent+" remaining="+d.boss.Remaining+" illusions="+string.Join(",",d.world.illusions.Select(x=>x.activeSelf+":"+Vector3.Distance(x.transform.position,d.player.transform.position))));Check(d.boss.State==Phase2BossState.Track&&d.world.illusions.All(x=>x.activeSelf),"Cycle-2 fixture contains exactly two nonphysical illusions");
            d.enabled=false;d.boss.Suspend();FixturePose(new Vector3(42,4.03f,20));d.lastri.agent.Warp(new Vector3(43.6f,4.03f,17));
            d.world.illusions[0].transform.position=new Vector3(39,4.2f,17);d.world.illusions[1].transform.position=new Vector3(46,4.2f,20);
            yield return Camera(true,true,false);
            Vector3 away=d.player.cameraPivot.position-d.lastri.Head;away.y=0;Look(d.player.cameraPivot.position+away);yield return null;yield return null;d.phoneView.RefreshLensPose();
            Check(d.boss.Temperature()==-20,"Posed real reticle/LOS fixture selects original Lastri at -20C");
            away=d.player.cameraPivot.position-(d.world.illusions[0].transform.position+Vector3.up*1.5f);away.y=0;Look(d.player.cameraPivot.position+away);yield return null;yield return null;d.phoneView.RefreshLensPose();
            Check(d.boss.Temperature()==-6,"Selecting a different illusion in the same Selfie feed reads -6C instead of nearest real ghost");
            d.lastri.agent.Warp(new Vector3(46,4.03f,22));d.world.illusions[0].transform.position=new Vector3(42,4.2f,17);d.boss.Cooldowns[0]=0;
            Check(d.boss.UseLamp(0)&&!d.boss.Exposed&&!d.boss.Spent[0]&&!d.world.illusions[0].activeSelf,"Light on an illusion clears it without breaking real shield or consuming source");
            Check(d.boss.Cooldowns[0]>11.9f,"Illusion-only light use receives 12-second cooldown");
            int flares=d.Campaign.Data.inventory.Count(x=>x.StartsWith("P2_FLARE_"));
            Check(d.boss.DropFlare()&&d.boss.UsedFlares.Count==1&&d.Campaign.Data.inventory.Count(x=>x.StartsWith("P2_FLARE_"))==flares,"Dropped flare is consumed in attempt RAM while CP5 inventory remains intact");
            d.boss.Abort();Check(d.Campaign.LoadCheckpoint(),"Recovery scenario restores actual frozen checkpoint file");d.enabled=true;d.RestoreWorld();
            Check(d.Phone.batteryPercent==0&&!d.boss.Spent.Any(x=>x)&&d.boss.UsedFlares.Count==0&&d.Campaign.Data.inventory.Count(x=>x.StartsWith("P2_FLARE_"))==3,"Retry resets battery exactly, all three lamps and every failed-attempt flare");
        }
    }
}
#endif
