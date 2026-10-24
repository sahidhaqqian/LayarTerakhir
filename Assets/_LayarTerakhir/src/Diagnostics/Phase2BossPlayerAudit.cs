#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase2;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace LayarTerakhir.Diagnostics
{
    public sealed partial class Phase2PlayerAudit
    {
        private static bool HasArg(string value)=>Array.IndexOf(Environment.GetCommandLineArgs(),value)>=0;
        private static bool NoSecret=>HasArg("-phase2NoSecret");
        private IEnumerator BossFixture()
        {
            Check(d.Campaign.TryCommit("PHASE2_CP4",data=>
            {
                foreach(string id in Phase2Ids.Evidence)if(!data.evidence_ids.Contains(id))data.evidence_ids.Add(id);
                foreach(string id in Phase2Ids.Relics){if(!data.inventory.Contains(id))data.inventory.Add(id);if(!data.relic_ids.Contains(id))data.relic_ids.Add(id);}
                foreach(string id in new[]{"P2_JOURNAL_CONFIRMED","P2_KEYPAD_1603","P2_STAIR_L2_UNLOCK","P2_CROSS_INKUBATOR_WALL","P2_TAKE_GUNTING","P2_TAKE_GELANG","P2_TAKE_BOTOL"})data.phase2.Add(id);
                if(HasArg("-phase2BossRecoveryAudit"))data.inventory.AddRange(new[]{"P2_FLARE_A","P2_FLARE_B","P2_FLARE_C"});
                data.phase2.safe_anchor="OR_PREP";data.battery=0;
            }),"Explicit boss-only test fixture seeds investigation; not an end-to-end playthrough");
            d.RestoreWorld();d.player.enabled=false;
            if(HasArg("-phase2ArenaVisual")){yield return ArenaVisual();yield break;}
            if(HasArg("-phase2BossRecoveryAudit")){yield return BossRecoveryScenarios();yield break;}
            yield return CompleteChapter();
        }
        private IEnumerator CompleteChapter()
        {
            yield return Camera(false,false,false);d.Phone.RestoreBattery(0);
            yield return Walk(new Vector3(39.4f,4.03f,18));yield return Use("RITUAL_TABLE");
            Check(d.boss.State==Phase2BossState.Prep&&d.Campaign.WritesBlocked&&d.Campaign.Data.checkpoint_id=="PHASE2_CP5","Valid table E saves CP5 before locking runtime attempt");
            Check(!d.Campaign.TryCommit("BAD",x=>x.wallet_legitimate=999),"Boss write barrier rejects unrelated transactions");
            Check(d.Campaign.Data.battery==0&&Phase2Ids.AllRelics(d.Campaign.Data),"Zero-energy CP5 preserves all three inventory relics");
            for(int cycle=0;cycle<3;cycle++)
            {
                // Real capsule travel, E input and eight-second manual hold; no energy grant.
                yield return Walk(new Vector3(46.4f,4.03f,21.5f));float beforeCharge=d.Phone.batteryPercent;yield return HoldUse("GENERATOR_NE",8.5f);
                Check(Mathf.Abs(d.Phone.batteryPercent-Mathf.Min(100,beforeCharge+25))<1,"Arena repeatable generator supplies actual +25 for cycle "+(cycle+1));
                yield return Walk(new Vector3(39.4f,4.03f,18));
                yield return Wait(()=>d.boss.Remaining<=0,45,"Minimum preparation/recovery permits unlimited safe recharge");
                yield return Use("RITUAL_TABLE");Check(d.boss.State==Phase2BossState.Track,"Explicit table E begins cycle "+(cycle+1));
                yield return Camera(true,true,false);
                if(cycle==0)
                {
                    yield return Walk(new Vector3(39.4f,4.03f,15));yield return Walk(new Vector3(44.5f,4.03f,15));yield return Walk(new Vector3(44.5f,4.03f,18));
                }
                else if(cycle==1)
                {
                    yield return Walk(new Vector3(39.4f,4.03f,21.4f));yield return Walk(new Vector3(43,4.03f,21.4f));yield return Walk(new Vector3(43,4.03f,22.5f));
                }
                else
                {
                    yield return Walk(new Vector3(39.4f,4.03f,13.5f));yield return Walk(new Vector3(43,4.03f,13.5f));
                }
                Debug.Log("P2_BOSS_BEAM cycle="+cycle+" player="+d.player.transform.position+" ghost="+d.world.lastri.transform.position+" inside="+d.boss.InsideBeam(d.world.lastri.transform.position,cycle));
                yield return Use("LAMP_"+cycle);yield return Wait(()=>d.boss.Exposed,2.5f,"Actual lamp geometry exposes Lastri for cycle "+(cycle+1));
                Check(!d.world.illusions.Any(x=>x.activeSelf),"Exposure removes both illusions and all attacks");
                if(cycle==0)
                {
                    float remaining=d.boss.Remaining,energy=d.Phone.batteryPercent;Vector3 ghost=d.world.lastri.transform.position;
                    GameShell.Instance.Pause();yield return new WaitForSecondsRealtime(.5f);
                    Check(Mathf.Abs(d.boss.Remaining-remaining)<.08f&&Mathf.Abs(d.Phone.batteryPercent-energy)<.01f&&Vector3.Distance(ghost,d.world.lastri.transform.position)<.02f,"Pause during EXPOSED freezes window, energy and real NavMesh movement");GameShell.Instance.Resume();
                }
                bool selfie=cycle!=1;yield return Camera(selfie,true,false);
                if(selfie)
                {Vector3 away=d.player.cameraPivot.position-d.lastri.Head;away.y=0;Look(d.player.cameraPivot.position+away.normalized*4);}
                else Look(d.lastri.Head);
                yield return null;yield return null;d.phoneView.RefreshLensPose();
                Check(d.Aim(d.lastri.Head,d.balance.bossAimRange,d.balance.bossAimAngle),"Real lens frames exposed head via "+(selfie?"Selfie":"rear camera after turning"));
                if(cycle==2&&HasArg("-phase2ZeroVictory"))
                {d.Phone.RestoreBattery(15);d.Capture();yield return null;}
                else yield return PressMouse();
                Check(d.boss.Layers==cycle+1&&d.boss.Spent[cycle],"One valid Blitz consumes one layer and only its successful lamp");
                yield return Shot("boss-cycle-"+(cycle+1));
                InputSystem.QueueStateEvent(mouse,new UnityEngine.InputSystem.LowLevel.MouseState());yield return null;
                if(cycle<2)
                {
                    Check(!d.Completed("P2_BOSS_WIN")&&!d.Campaign.Data.location_seals.Contains(Phase2Ids.Seal),"One/two hits never persist a location seal");
                    d.Phone.isScreenOn=false;d.Phone.SyncVisualState();
                }
            }
            Check(d.Completed("P2_BOSS_WIN")&&!d.Campaign.WritesBlocked&&!d.world.arenaLock.activeSelf,"Third hit commits CP6 and unlocks physical return route");
            Check(d.Campaign.Data.location_seals.Count(x=>x==Phase2Ids.Seal)==1&&!d.Campaign.Data.inventory.Intersect(Phase2Ids.Relics).Any(),"CP6 contains one seal and all three relics moved to table atomically");
            if(HasArg("-phase2ZeroVictory"))
            {
                yield return new WaitForSeconds(.5f);Check(d.Phone.batteryPercent==0&&!d.StoryPlaying&&!d.Dead,"Exactly-zero CP6 waits safely without auto-charge or automatic epilogue");
                yield return Walk(new Vector3(46.4f,4.03f,21.5f));yield return HoldUse("GENERATOR_NE",8.5f);
                Check(d.Phone.batteryPercent>=24&&d.StoryPlaying,"Post-boss physical generator supplies power for the independent epilogue");
            }
            yield return Wait(()=>d.Completed("P2_EPILOGUE"),50,"Independent epilogue completes without manually starting live in IGD");
            Check(d.Completed("P2_DAMAR_VIDEO")&&d.Viewers==310500&&!d.VideoPlaying&&!UiFocus.Cinematic,"Eight-second Damar video restores player and existing feed ownership");
            Check(d.Campaign.Data.wallet_legitimate==0&&d.Campaign.Data.sanity==23,"Narrative 164.8M never becomes spendable money or changes Sanity");
            long bound=d.Campaign.Data.wallet_bound;d.ReplayDamarVideo();yield return Wait(()=>!d.StoryPlaying,15,"Archived Damar video can replay after epilogue");
            Check(d.Campaign.Data.wallet_bound==bound&&d.Campaign.Data.phase2.completed_event_ids.Count(x=>x=="P2_DAMAR_VIDEO")==1,"Video replay never duplicates donation, seal or progression");
            d.Phone.RestoreBattery(0);d.Phone.isPhoneRaised=false;d.Phone.SyncVisualState();
            yield return Walk(new Vector3(34,4.03f,18));yield return Walk(new Vector3(2,4.03f,15));yield return Walk(new Vector3(1,.03f,3));
            if(NoSecret)
            {
                yield return Walk(new Vector3(34,.03f,3));yield return Walk(new Vector3(34,-3.97f,17));yield return Walk(new Vector3(13,-3.97f,30));
                yield return Walk(new Vector3(12,-3.97f,31.7f));Check(!d.Dead&&!d.lastri.Attached,"After CP6 battery-zero route back to D1 is safe and physically reachable");
                if(HasArg("-phase2PostBossSecret"))yield return Use("D1");
                yield return Walk(new Vector3(34,-3.97f,17));yield return Walk(new Vector3(34,.03f,3));
            }
            yield return Walk(new Vector3(5,.03f,-10));yield return Walk(new Vector3(7.8f,.03f,-11.8f));yield return Use("EXIT_MOTOR");
            Check(UiFocus.Clock&&!d.Completed("P2_COMPLETE"),"Motor opens transient archive panel without completing chapter");
            for(int cancel=0;cancel<3;cancel++)
            {d.CloseOverlay();Check(!d.Completed("P2_COMPLETE"),"Cancel exit leaves exploration and save unchanged #"+(cancel+1));yield return Use("EXIT_MOTOR");}
            if(HasArg("-phase2ExitFault"))
            {
                string directory=System.Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY");string blocked=System.IO.Path.Combine(directory,"blocked-CP7-fixture");System.IO.File.WriteAllText(blocked,"Intentional disk failure fixture");
                Vector3 before=d.player.transform.position;float alpha=HudController.Instance.blackFadeImage.color.a;
                System.Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",blocked);expectedPersistenceError=true;persistenceErrorSeen=false;
                bool completed=d.ConfirmExit();expectedPersistenceError=false;System.Environment.SetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY",directory);
                Check(!completed&&persistenceErrorSeen&&!d.Completed("P2_COMPLETE")&&UiFocus.Clock&&Mathf.Abs(HudController.Instance.blackFadeImage.color.a-alpha)<.001f&&Vector3.Distance(d.player.transform.position,before)<.01f,"Real CP7 write failure preserves open panel, position and fade with no completion");
            }
            Check(d.ConfirmExit(),"Conscious final confirmation writes CP7 before shutter and black");
            Check(d.Campaign.Data.phase2.next_destination==Phase2Ids.Destination&&d.Campaign.Data.checkpoint_id=="PHASE2_CP7","Phase 3 destination is saved without creating a playable Phase 3 scene");
            Check(d.Campaign.LoadCheckpoint(),"Completed chapter reloads from actual disk");
            Check(d.Campaign.Data.battery==0&&d.Campaign.Data.location_seals.Contains(Phase2Ids.Seal)&&d.Completed("P2_COMPLETE"),"CP7 retains zero energy, seal and completion consistently");
            yield return SceneManager.LoadSceneAsync(CampaignRouting.HospitalScene);
            yield return Wait(()=>FindFirstObjectByType<Phase2Director>()&&FindFirstObjectByType<Phase2Director>().Ready,300,"Actual CP7 scene restore rebuilds completed chapter");d=FindFirstObjectByType<Phase2Director>();
            Check(d.boss.State==Phase2BossState.Resolved&&d.Completed("P2_COMPLETE")&&UiFocus.Clock,"Completed restore shows handoff and never respawns Lastri");
            d.CloseOverlay();Check(GameShell.AwaitingStart,"Closing chapter handoff returns to main menu without trapping player in a black world");
        }
        private void TraceRitualVisual()
        {
            var cam=d.phoneView.FeedCamera;Debug.Log("P2_VISUAL_CAMERA pos="+cam.transform.position+" forward="+cam.transform.forward+" mask="+cam.cullingMask+" ghostHead="+cam.WorldToViewportPoint(d.lastri.Head));
            foreach(var r in d.world.lastri.GetComponentsInChildren<SkinnedMeshRenderer>(true))Debug.Log("P2_VISUAL_SKIN name="+r.name+" enabled="+r.enabled+" active="+r.gameObject.activeInHierarchy+" forced="+r.forceRenderingOff+" layer="+r.gameObject.layer+" bounds="+r.bounds+" viewport="+cam.WorldToViewportPoint(r.bounds.center));
        }
        private IEnumerator ArenaVisual()
        {
            d.Phone.RestoreBattery(80);yield return Camera(false,false,false);Look(new Vector3(43,5,18));yield return new WaitForSeconds(2);yield return Shot("arena-before-ritual");
            d.phoneView.CinematicHandsetVisible=true;UiFocus.Cinematic=true;d.phoneView.SetViewfinder(true);d.world.videoSet.SetActive(true);typeof(Phase2Director).GetProperty("VideoPlaying").SetValue(d,true);d.phoneView.RefreshLensPose();yield return new WaitForSeconds(2);yield return Shot("damar-video-set-fixture");
            typeof(Phase2Director).GetProperty("VideoPlaying").SetValue(d,false);d.world.videoSet.SetActive(false);UiFocus.Cinematic=false;d.phoneView.CinematicHandsetVisible=false;
            // Posed visual fixtures only; full-route runs separately prove combat.
            d.phoneView.SetViewfinder(false);FixturePose(d.world.Anchor("OR_PREP").position);Look(d.world.ritualTable.position);d.player.GetComponent<LayarTerakhir.Player.ShoulderCameraRig>().RefreshCameraPose();
            Check(d.boss.Begin(),"Visual fixture starts real ritual then freezes brain for composed screenshots");d.enabled=false;d.boss.Suspend();
            d.lastri.agent.baseOffset=.18f;bool trackWarp=d.lastri.agent.Warp(d.player.transform.position-Vector3.right*2.4f+Vector3.forward*.65f);
            var idle=d.world.lastri.GetComponentInChildren<Animation>();if(idle&&idle["Idle"])idle.Play("Idle");Debug.Log("P2_VISUAL_WARP track="+trackWarp+" root="+d.world.lastri.transform.position);d.world.lastri.transform.LookAt(d.player.transform.position+Vector3.up*.18f);d.world.bossMist.SetActive(true);
            yield return Camera(true,false,false);Look(d.player.cameraPivot.position+Vector3.right*4);d.phoneView.ShowChapterReadout("SELFIE • KABUT SENGKALA",Color.white);yield return new WaitForSeconds(2);TraceRitualVisual();yield return Shot("boss-selfie-track-fixture");
            d.world.bossMist.SetActive(false);FixturePose(new Vector3(44.5f,4.03f,18));bool exposedWarp=d.lastri.agent.Warp(new Vector3(42,4.03f,19));Debug.Log("P2_VISUAL_WARP exposed="+exposedWarp+" root="+d.world.lastri.transform.position);d.world.ritualLights[0].enabled=true;
            foreach(var r in d.world.lastri.GetComponentsInChildren<Renderer>(true)){r.gameObject.layer=0;r.enabled=true;}
            var motion=d.world.lastri.GetComponentInChildren<Animation>();if(motion&&motion["Stun"])motion.CrossFade("Stun",.2f);
            Look(d.player.cameraPivot.position+Vector3.right*4);d.phoneView.RefreshLensPose();d.phoneView.ShowChapterTarget(d.lastri.Head,true);d.phoneView.ShowChapterReadout("PERISAI HANCUR • BIDIK KEPALA",new Color(1,.3f,.2f));
            yield return new WaitForSeconds(2);TraceRitualVisual();yield return Shot("boss-exposed-selfie-fixture");
        }
    }
}
#endif
