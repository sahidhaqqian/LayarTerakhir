using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public sealed partial class Phase3Director
    {
        private void UpdateInvestigation(float dt)
        {
            bool rear=CameraCanScan&&!Phone.isSelfieCamera&&!GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen);
            slingDwell=rear&&Completed("P3_ACCESS_GENERATOR")&&Phone.currentScanMode==ScanVisualMode.AR&&Aim(world.slingTarget.position,5,22,world.sling.transform)?slingDwell+dt:0;
            rajahDwell=rear&&Phone.currentScanMode==ScanVisualMode.UV&&Aim(world.rajahTarget.position,5,22)?rajahDwell+dt:0;
            SelectedSource=null;float best=-1000;
            if(rear)
            {
                foreach(var s in world.sources)
                {
                    if(!s||!s.gameObject.activeInHierarchy||!Aim(s.transform.position,s.range,38,s.transform))continue;
                    float score=s.FocusScore(player.transform.position+Vector3.up);
                    if(score>best){best=score;SelectedSource=s;}
                }
            }
            var input=GameInputConfig.Instance;bool held=input.GetActionHeld(GameAction.ModeEVP);
            if(held&&!evpWasHeld){evpHeld=EvpProgress=evpReal=0;evpFinished=false;recordSource=null;}
            if(held)
            {
                evpHeld+=dt;
                if(evpHeld>balance.evpTap&&!evpFinished)
                {
                    Analyzer=true;
                    bool valid=rear&&SelectedSource&&(SelectedSource.sourceId=="INTERCOM"||SelectedSource.sourceId=="VENT09")&&
                        Vector3.Distance(player.transform.position+Vector3.up,SelectedSource.transform.position)<=balance.evpRange;
                    if(!valid||recordSource&&recordSource!=SelectedSource){RecordingEVP=false;EvpProgress=evpReal=0;recordSource=null;}
                    else
                    {
                        recordSource=SelectedSource;RecordingEVP=true;EvpProgress+=dt;
                        if(SelectedTag==Phase3VoiceTag.Authentic)evpReal+=dt;
                        float need=recordSource.sourceId=="VENT09"?balance.secretRecordSeconds:balance.evpSeconds;
                        if(EvpProgress>=need)
                        {
                            bool authentic=evpReal>=(recordSource.sourceId=="VENT09"?balance.secretRecordSeconds-.03f:balance.realSeconds);
                            if(authentic)
                            {
                                if(recordSource.sourceId=="VENT09")
                                {if(Commit("P3_SECRET_CLUE",null,null,SafeAnchor))Hud.EnqueueSubtitle("EVP — ASLI","[Tiga ketukan dari ventilasi.] Nomor sembilan tidak pulang.",7);}
                                else if(AddEvidence(Phase3Ids.EVP,"P3_EVIDENCE_EVP"))
                                {Hud.EnqueueSubtitle("EVP — ASLI","...pulang... jangan tutup...",6);wewe.QueueOfficeSearch();}
                            }
                            else Hud.ShowBroadcastNotification("REKAMAN TIRUAN — tunggu rintihan ASLI. Rekam ulang tanpa penalti.",5);
                            RecordingEVP=false;evpFinished=true;
                        }
                    }
                }
            }
            if(!held&&evpWasHeld)
            {
                if(evpHeld<=balance.evpTap)Analyzer=!Analyzer;
                RecordingEVP=false;EvpProgress=evpReal=0;recordSource=null;
            }
            evpWasHeld=held;
            if(Phone.currentScanMode==ScanVisualMode.UV&&!Completed("P3_UV_TUTORIAL")&&!boss.Active)Commit("P3_UV_TUTORIAL");
            if(rear&&Phone.currentScanMode==ScanVisualMode.UV&&Completed("P3_SECRET_CLUE")&&Aim(world.secretUvTarget.position,8,30)&&!Completed("P3_SECRET_UV_ROUTE"))
                Commit("P3_SECRET_UV_ROUTE",null,null,SafeAnchor);
            if(!boss.Active)
            {
                string label=SelectedTag==Phase3VoiceTag.Authentic?"ASLI   ~──~──~──":SelectedTag==Phase3VoiceTag.Mimic?"TIRUAN   /\\/\\/\\/\\":"LATAR   ─────────";
                phoneView.ShowChapterReadout(Analyzer&&CameraCanScan?"EVP v2 • "+label+(SelectedSource?"\n"+SelectedSource.subtitle:"")+(RecordingEVP?$"\nREKAM {EvpProgress:0.0}s • ASLI {evpReal:0.0}s":""):"",SelectedTag==Phase3VoiceTag.Authentic?new Color(.4f,1,.65f):SelectedTag==Phase3VoiceTag.Mimic?new Color(1,.35f,.3f):Color.white);
            }
        }
        private bool AddEvidence(string id,string receipt)
        {
            if(Campaign.Data.evidence_ids.Contains(id))return true;
            // Mill receipts remain at CP2/3, never create premature CP4 then regress to CP3.
            string cp=CurrentRoom=="MILL"?Campaign.Data.checkpoint_id:"PHASE3_CP4";
            bool ok=Commit(receipt,d=>d.evidence_ids.Add(id),cp,SafeAnchor);
            if(ok)Hud.ShowBroadcastNotification("BUKTI KLENIK TERSIMPAN • "+Phase3Ids.Evidence.Count(Campaign.Data.evidence_ids.Contains)+"/3",4);
            return ok;
        }
        public void Capture()
        {
            if(!CameraCanScan||RecordingEVP||GameInputConfig.Instance.GetActionHeld(GameAction.ModeEVP)||GameInputConfig.Instance.GetActionDown(GameAction.Interact)||GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen))return;
            if(boss.Capture())return;
            string id=slingDwell>=balance.photoDwell?Phase3Ids.Sling:rajahDwell>=balance.photoDwell?Phase3Ids.Rajah:null;
            if(id!=null&&!Campaign.Data.evidence_ids.Contains(id))
            {
                if(Phone.batteryPercent<Phone.photoCostPercent)return;
                float after=Phone.batteryPercent-Phone.photoCostPercent;
                string cp=CurrentRoom=="MILL"?Campaign.Data.checkpoint_id:"PHASE3_CP4";
                if(Commit(id==Phase3Ids.Sling?"P3_EVIDENCE_GENDONGAN":"P3_EVIDENCE_RAJAH",d=>{d.evidence_ids.Add(id);d.battery=after;},cp,SafeAnchor))
                {Phone.RestoreBattery(after);phoneView.CaptureFlash();Hud.ShowBroadcastNotification("Foto bukti tersimpan di Jurnal.",4);}
                return;
            }
            if(Phone.TryCaptureEnergy(false))
            {phoneView.CaptureFlash();Hud.ShowBroadcastNotification("Foto biasa. Bukti memerlukan filter yang benar dan bidikan stabil.",3);}
        }
    }
}
