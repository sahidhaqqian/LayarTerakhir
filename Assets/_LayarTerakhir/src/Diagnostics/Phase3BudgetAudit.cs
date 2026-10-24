#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
using UnityEngine;
namespace LayarTerakhir.Diagnostics
{
    public sealed partial class Phase3PlayerAudit
    {
        // Explicit synthetic pre-boss fixture, never counted as an investigation playthrough.
        private IEnumerator BudgetRoute()
        {
            Check(d.Campaign.TryCommit("PHASE3_CP4",data=>
            {
                foreach(var id in Phase3Ids.Evidence)if(!data.evidence_ids.Contains(id))data.evidence_ids.Add(id);
                foreach(var id in Phase3Ids.Relics){if(!data.inventory.Contains(id))data.inventory.Add(id);if(!data.relic_ids.Contains(id))data.relic_ids.Add(id);}
                data.phase3.Add("P3_JOURNAL_CONFIRMED");data.phase3.safe_anchor="ALTAR";data.battery=0;
            }),"Explicit synthetic fixture seeds required evidence and relics with zero battery");
            d.RestoreWorld();d.Phone.RestoreBattery(0);yield return Camera(raised:false);
            for(int i=0;i<3;i++){yield return Walk(new Vector3(72.1f,.04f,21+i*2));yield return Use("ALTAR_"+i);}
            yield return Walk(new Vector3(71,.04f,24));yield return Use("DECOY");
            Check(d.boss.Active&&d.boss.Recovering&&d.Phone.batteryPercent==0,"Real ritual interaction accepts zero-energy CP5 without granting a recharge");
            string path=Path.Combine(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY"),"layar_terakhir_save.json");
            string baseline=File.ReadAllText(path);int misses=0,resets=0;
            while(misses<20)
            {
                if(d.boss.Recovering)
                {
                    yield return Camera(raised:false);
                    if(d.Phone.batteryPercent<d.Phone.blitzCostPercent+1)
                    {
                        if(d.boss.PowerTokens==0)
                        {
                            yield return Walk(new Vector3(70.5f,.04f,13.2f));
                            if(!d.AlcoveClosed)yield return Use("ALCOVE_DOOR");
                            yield return Wait(()=>d.boss.ResetCooldown<=0,25,"Manual reset cooldown expires on the production clock");
                            yield return HoldUse("ALCOVE_RESET",8.3f);resets++;
                            Check(d.boss.PowerTokens==1,"Closed alcove crank restores exactly one shared power token");
                            yield return HoldUse("POWER_0",8.3f);
                            yield return Use("ALCOVE_DOOR");
                        }
                        else
                        {
                            yield return Walk(new Vector3(71,.04f,31.4f));yield return HoldUse("POWER_1",8.3f);
                        }
                        Check(d.Phone.batteryPercent>=d.Phone.blitzCostPercent&&d.boss.PowerTokens==0,"Real held charger restores energy and consumes the shared token once");
                    }
                    yield return Walk(new Vector3(71,.04f,24));
                    yield return Wait(()=>d.boss.Remaining<=0&&d.Phone.BlitzCooldownRemaining<=0,40,"Recovery remains available until deliberate restart");
                    yield return Use("DECOY");Check(d.boss.State==Phase3BossState.Stalk,"Same unsolved cycle can restart after a miss");
                }
                yield return Camera();Look(d.player.cameraPivot.position+Vector3.up*50);
                float energy=d.Phone.batteryPercent;yield return PressMouse();misses++;
                Check(d.Phone.batteryPercent<=energy-14.9f&&d.boss.Layers==0&&d.boss.Spent.All(x=>!x),"Miss "+misses+" spends Blitz energy without a seal or a destroyed generator");
                yield return Wait(()=>d.boss.Recovering||d.Phone.BlitzCooldownRemaining<=0,10,"Miss respects cooldown or enters resource recovery");
            }
            Check(resets>0&&File.ReadAllText(path)==baseline,"Twenty misses use manual reset while on-disk CP5 remains byte-identical");
            Check(!d.Campaign.SaveCheckpointAtomic("ILLEGAL_BOSS_SAVE"),"Global save cannot persist transient twenty-miss state");
            d.boss.Abort();Check(d.Campaign.LoadCheckpoint(),"Aborting the attempt restores the production checkpoint file");
            Check(d.Campaign.Data.battery==0&&Phase3Ids.AllRelics(d.Campaign.Data)&&d.Campaign.Data.checkpoint_id=="PHASE3_CP5","Retry restores exact zero-energy CP5, relics and unconsumed inventory");
        }
    }
}
#endif
