#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase3;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Diagnostics
{
    public sealed partial class Phase3PlayerAudit
    {
        private IEnumerator FullRoute()
        {
            d.phoneView.Apps.Open(PhoneApp.Inventory);yield return Camera();
            Check(d.Phone.isPhoneRaised&&d.phoneView.IsCameraApp,"Held real RMB forces camera from the inventory app");
            yield return Camera(ScanVisualMode.Normal,false);Check(!d.Phone.isPhoneRaised&&d.phoneView.Apps.CurrentApp==PhoneApp.Inventory,"Releasing real RMB restores stowed phone and prior app");
            yield return Walk(new Vector3(13,.04f,3));Check(d.Campaign.Data.checkpoint_id=="PHASE3_CP2","CP2 precedes first false voice and mill trap");
            yield return Walk(new Vector3(18,.04f,8));yield return Walk(new Vector3(18,.04f,25));yield return Walk(new Vector3(19.5f,.04f,26));
            for(int i=0;i<3;i++){yield return Use("G_ACC");yield return new WaitForSeconds(1.6f);}
            Check(d.Completed("P3_ACCESS_GENERATOR"),"Three timed real crank interactions open magnetic access");
            Check(d.Campaign.Data.checkpoint_id=="PHASE3_CP2","Access generator does not invent CP3/4 or boss resources");
            yield return Walk(new Vector3(21,.04f,27.3f));yield return Camera(ScanVisualMode.AR);Look(d.world.slingTarget.position);yield return new WaitForSeconds(1.3f);yield return PressMouse();
            Check(d.Campaign.Data.evidence_ids.Contains(Phase3Ids.Sling),"Actual AR camera aim captures sling evidence before doll pickup");
            Check(!d.Campaign.HasInventoryItem(Phase3Ids.Doll),"Sling photo never grants the relic silently");
            yield return Use("DOLL");yield return Use("MANDOR");HudController.Instance.CloseDocumentModal();
            Check(d.Campaign.Data.checkpoint_id=="PHASE3_CP2"&&d.Campaign.HasInventoryItem(Phase3Ids.Doll),"Mill pickups persist immediately at CP2 without premature CP4");
            yield return Camera(ScanVisualMode.Normal,false);yield return Walk(new Vector3(27,.04f,24));
            Check(d.Completed("P3_ENTER_BOILER"),"Physical magnetic gate traversal reaches CP3 boiler landing");
            yield return Walk(new Vector3(56,.04f,17));yield return Walk(new Vector3(55,4.04f,33));yield return Walk(new Vector3(53,4.04f,34.5f));yield return Use("SICKLE");
            Check(d.Campaign.HasInventoryItem(Phase3Ids.Sickle)&&!d.Campaign.Data.evidence_ids.Contains(Phase3Ids.EVP),"Sickle collected before independent intercom recording");
            yield return Walk(new Vector3(47,4.04f,34.4f));yield return Camera();Look(d.world.sources.First(s=>s.sourceId=="INTERCOM").transform.position);
            yield return Hold(GameAction.ModeEVP,.12f);Check(d.Analyzer&&!d.RecordingEVP,"Quick C tap toggles analyzer without recording");
            yield return Hold(GameAction.ModeEVP,.12f);Check(!d.Analyzer,"Second quick C tap toggles the same analyzer off");
            yield return Hold(GameAction.ModeEVP,.9f);Check(!d.RecordingEVP&&!d.Campaign.Data.evidence_ids.Contains(Phase3Ids.EVP),"Releasing interrupted C hold does not award EVP evidence");
            yield return Wait(()=>d.Elapsed%11>=8.05f&&d.Elapsed%11<=8.4f,15,"Authored intercom reaches the real segment after eight seconds of mimic");
            yield return Hold(GameAction.ModeEVP,6.4f);Check(d.Campaign.Data.evidence_ids.Contains(Phase3Ids.EVP),"Held production C records six seconds including two authentic seconds");
            if(!Has("-phase3NoSecret"))
            {
                yield return Camera(ScanVisualMode.Normal,false);yield return Walk(new Vector3(46,4.04f,42.2f));yield return Camera();Look(d.world.sources.First(s=>s.sourceId=="VENT09").transform.position);
                yield return Hold(GameAction.ModeEVP,1.9f);Check(d.Completed("P3_SECRET_CLUE"),"Ventilation knocks recorded independently as authentic source");
                yield return Camera(ScanVisualMode.UV);Look(d.world.secretUvTarget.position);yield return new WaitForSeconds(.4f);Check(d.Completed("P3_SECRET_UV_ROUTE"),"Actual UV aim reveals balcony clue without an extra render camera");
                yield return Camera(ScanVisualMode.Normal,false);yield return Walk(new Vector3(54.5f,4.04f,45.5f));yield return HoldUse("VALVE_09",1.3f);Check(d.Completed("P3_VALVE_09"),"Correct valve opens after clue and real hold");
                yield return Walk(new Vector3(59,4.04f,45.3f));yield return Use("LOCKER_09");Check(d.Campaign.Data.secret_items.Contains(Phase3Ids.Secret),"Optional plate receipt atomically saves one archive ID");
            }
            yield return Camera(ScanVisualMode.Normal,false);yield return Walk(new Vector3(55,4.04f,34));yield return Walk(new Vector3(57,.04f,17));
            yield return Walk(new Vector3(32,.04f,29));yield return Walk(new Vector3(28.5f,.04f,36.8f));
            yield return Camera(ScanVisualMode.UV);Look(d.world.rajahTarget.position);yield return new WaitForSeconds(1.3f);yield return PressMouse();
            Check(d.Campaign.Data.evidence_ids.Contains(Phase3Ids.Rajah),"UV wall evidence uses its real lens filter and camera LOS");
            yield return Camera(ScanVisualMode.Normal,false);yield return Walk(new Vector3(26,.04f,36.8f));yield return Use("CLOTH");yield return Use("DAMAR_CARD");HudController.Instance.CloseDocumentModal();Check(!d.Campaign.HasInventoryItem(Phase3Ids.Shoe),"Reading the press card does not grant Damar shoe");yield return Use("DAMAR_SHOE");
            Check(d.Campaign.HasInventoryItem(Phase3Ids.Cloth)&&d.Campaign.HasInventoryItem(Phase3Ids.Shoe),"Cloth relic and Damar story clues remain separate from UV evidence");
            yield return Walk(new Vector3(32,.04f,29));yield return Walk(new Vector3(61,.04f,24));
            Check(d.ConfirmJournal(),"Explicit journal confirmation requires three exact evidence IDs");
            Check(Phase3Ids.RitualReady(d.Campaign.Data),"All investigation completed without any shop purchase");
            yield return Walk(new Vector3(71.8f,.04f,24));
            for(int i=0;i<3;i++){yield return Walk(new Vector3(72.1f,.04f,21+i*2));yield return Use("ALTAR_"+i);}
            yield return Walk(new Vector3(71,.04f,24));yield return Use("DECOY");Check(d.boss.Active&&d.Campaign.WritesBlocked,"Real decoy interaction writes CP5 before arena owner lock");
            for(int cycle=0;cycle<3;cycle++)
            {
                if(d.Phone.batteryPercent<22){yield return Walk(new Vector3(71,.04f,31.4f));yield return HoldUse("POWER_1",8.3f);}
                yield return Walk(new Vector3(71,.04f,24));yield return Wait(()=>d.boss.Remaining<=0,40,"Preparation or recovery time elapses with manual resources");yield return Use("DECOY");
                Check(d.boss.State==Phase3BossState.Stalk,"Cycle starts only after conscious E and sufficient energy");
                yield return LureLight(cycle);
                yield return Camera();Look(d.wewe.Head);yield return new WaitForSeconds(.12f);yield return PressMouse();
                Check(d.boss.Layers==cycle+1,"One real camera Blitz awards one distinct transient seal layer");
                yield return Camera(ScanVisualMode.Normal,false);
            }
            Check(d.Completed("P3_BOSS_WIN")&&d.Campaign.Data.location_seals.Contains(Phase3Ids.Seal),"Third hit atomically commits CP6 and the third location seal");
            yield return Wait(()=>d.Completed("P3_EPILOGUE"),120,"Epilogue chat infiltration and final message complete without forced exit");
            Check(!d.Completed("P3_COMPLETE")&&d.Campaign.Data.wallet_legitimate==0&&d.Campaign.Data.sanity==23,"Ending restores exploration without inventing money or changing Sanity");
            yield return Walk(new Vector3(95,.04f,24));yield return Walk(new Vector3(96,.04f,-2));yield return Walk(new Vector3(16,.04f,-2));yield return Walk(new Vector3(15,.04f,-13));
            for(int i=0;i<3;i++){yield return Use("EXIT_MOTOR");d.CloseOverlay();Check(!d.Completed("P3_COMPLETE"),"Cancelling exit panel leaves chapter incomplete and player free");}
            yield return Use("EXIT_MOTOR");Check(d.ConfirmExit(),"Motor confirmation stores CP7 before shutter and fade");Check(d.Campaign.LoadCheckpoint(),"Final checkpoint loads from production disk");
            Check(d.Campaign.Data.phase3.next_destination==Phase3Ids.Destination&&d.Campaign.Data.checkpoint_id=="PHASE3_CP7","Final destination is Karang Setra, without creating Chapter 4");
        }
        private IEnumerator HoldUse(string id,float seconds)
        {
            var item=FindInteractionItem(id);
            UnityEngine.InputSystem.InputSystem.QueueStateEvent(mouse,new UnityEngine.InputSystem.LowLevel.MouseState());yield return null;yield return AimInteraction(item);yield return Hold(GameAction.Interact,seconds);
        }
        private IEnumerator LureLight(int i)
        {
            Vector3 centre=d.world.beams[i].position;
            Vector3 toward=(d.world.levers[i].position-centre).normalized;Vector3 inner=centre-toward*1.8f;
            yield return Camera(ScanVisualMode.Normal,false);
            if(i==2)
            {
                yield return Camera(ScanVisualMode.UV);
                yield return Wait(()=>d.boss.PullActive,25,"Final cloth pull begins within visible counter range");
                float deadline=Time.realtimeSinceStartup+4;
                while(d.boss.PullActive&&Time.realtimeSinceStartup<deadline){Look(d.world.clothKnot.transform.position);yield return null;}
                Check(!d.boss.PullActive&&d.boss.Layers==2,"Continuous UV aim breaks final pull without flare or awarding a seal");
                yield return Camera(ScanVisualMode.Normal,false);
                yield return Walk(new Vector3(72,.04f,16));yield return Walk(new Vector3(88.5f,.04f,16));
            }
            yield return Walk(inner);
            yield return Wait(()=>Vector3.Distance(d.world.wewe.transform.position,centre)<2.4f,35,"Physical stalk enters chosen light beam");
            yield return Walk(d.world.levers[i].position-toward*1.5f);yield return Use("LAMP_"+i);
            yield return Wait(()=>d.boss.State==Phase3BossState.Exposed,3,"Light breaks real boss shield without spending source before success");
        }
    }
}
#endif
