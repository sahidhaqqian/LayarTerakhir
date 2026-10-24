using System.Linq;
using LayarTerakhir.Phase1;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase2
{
    public sealed partial class Phase2Director
    {
        private bool arenaCharging, storyRetryRequired;
        private ScanVisualMode storyReturnMode;
        private PhoneApp storyReturnApp;
        private float arenaChargeProgress;
        public bool NearTable() => world.ritualTable && Vector3.Distance(player.transform.position+Vector3.up,world.ritualTable.position)<=2.25f &&
            ClearLine(player.transform.position+Vector3.up,world.ritualTable.position,world.ritualTable);
        public void RefreshAfterRitual(){RefreshWorldItems();RefreshObjective();}
        private IEnumerator ChargeArena(Phase2WorldItem item)
        {
            if(arenaCharging||!boss.CanCharge||StoryPlaying)yield break;
            arenaCharging=true;
            while(!Dead&&boss.CanCharge&&GameInputConfig.Instance.GetActionHeld(GameAction.Interact))
            {
                Vector3 hand=player.transform.position+Vector3.up;
                if(Vector3.Distance(hand,item.transform.position)>balance.generatorRadius||!ClearLine(hand,item.transform.position,item.transform))break;
                if(ClockRunning)arenaChargeProgress+=Time.deltaTime;
                Hud.SetInteractionPrompt($"GENERATOR MANUAL • {arenaChargeProgress:0.0}/{balance.generatorSeconds:0}s");
                if(arenaChargeProgress>=balance.generatorSeconds)
                {
                    float battery=Mathf.Min(100,Phone.batteryPercent+balance.generatorAmount);
                    bool stored=boss.Active||Campaign.TryCommit(Campaign.Data.checkpoint_id,d=>d.battery=battery);
                    if(stored){Phone.RestoreBattery(battery);arenaChargeProgress=0;Hud.ShowBroadcastNotification("Generator +25 daya. Bisa diputar kembali selama jeda.",4);}
                    else Hud.ShowBroadcastNotification("Daya belum tersimpan. Coba lagi.",4);
                    break;
                }
                yield return null;
            }
            arenaCharging=false;
        }
        private IEnumerator Epilogue()
        {
            StoryPlaying=true;storyReturnMode=Phone.currentScanMode;storyReturnApp=phoneView.Apps.CurrentApp;Phone.currentScanMode=ScanVisualMode.Normal;
            bool raised=Phone.isPhoneRaised,selfie=Phone.isSelfieCamera,screen=Phone.isScreenOn;
            player.GetComponentInChildren<AnimatedRakaAvatar>().CinematicCrouch=true;
            player.MovementLocked=true;UiFocus.Cinematic=true;Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.isSelfieCamera=false;
            Phone.isBroadcastConnected=true;Phone.SyncVisualState();phoneView.CinematicHandsetVisible=true;phoneView.Apps.Open(PhoneApp.Camera);phoneView.SetViewfinder(true);
            Vector3 eyeBefore=player.cameraPivot.localPosition;
            for(float lowering=0;lowering<.35f;lowering+=Time.deltaTime){var eye=eyeBefore;eye.y=Mathf.Lerp(eyeBefore.y,player.cameraCrouchY,Mathf.SmoothStep(0,1,lowering/.35f));player.cameraPivot.localPosition=eye;yield return null;}
            Hud.EnqueueSubtitle("Raka","Lastri... segel kedua sudah masuk. Damar, lu masih di sana?",6);
            if(!Commit("P2_EPILOGUE_CHAT")){storyRetryRequired=true;ReleaseStory(raised,selfie,screen);yield break;}
            Chat("tretan_kw","DEMI ALLAH GUE NONTON SAMBIL TUTUP MATA PAS DIA MUNCUL DI KAMERA SELFIE!");
            Chat("jurnal_risa_fan","Itu barusan segel lokasi kedua masuk ke HP?! Bang Raka udah dapet ratusan juta tapi nyawanya beneran di ujung tanduk!");
            phoneView.ShowChapterReadout("LIVE • 310.500 PENONTON\nTOTAL SAWERAN KUMULATIF Rp164.800.000",Color.white);
            yield return new WaitForSeconds(6);
            if(!Commit("P2_DAMAR_VIDEO",d=>
            {
                d.wallet_bound+=6666666;d.phase2.money_ledger.Add(new Phase1MoneyReceipt{id="P2_DONATION_DAMAR",amount=6666666,bound=true});
                if(!d.documents.Exists(x=>x.title=="Video tungku Damar"))d.documents.Add(new PhoneDocument("Video tungku Damar",Phase2Content.DamarVideoDescription+"\n\n"+Phase2Content.FinalMessage));
            })){storyRetryRequired=true;ReleaseStory(raised,selfie,screen);yield break;}
            VideoPlaying=true;world.videoSet.SetActive(true);phoneView.Glitch(.5f,.25f);
            phoneView.ShowChapterChat("PENONTON_TERAKHIR • Rp6.666.666\nKomentar dikunci. REKAMAN MASUK.");
            phoneView.ShowChapterReadout("DAMAR • SINDANG LAUT\nREKAMAN 00:08",new Color(1,.38f,.3f));
            Hud.EnqueueSubtitle("Rekaman Damar","[Dengusan dari balik tungku. Rantai diseret di lantai pabrik. Damar terikat di samping tungku yang mati.]",8);
            soundscape.Cue("Chain",player.cameraPivot.position);soundscape.Cue("Breath",player.cameraPivot.position);
            yield return new WaitForSeconds(8);
            VideoPlaying=false;world.videoSet.SetActive(false);
            phoneView.ShowChapterChat("PENONTON_TERAKHIR\n"+Phase2Content.FinalMessage);
            foreach(string sentence in Phase2Content.FinalMessage.Split(new[]{". "},System.StringSplitOptions.None))
            {float reading=Mathf.Max(4,sentence.Split(' ').Length/2.8f);Hud.EnqueueSubtitle("Penonton_Terakhir",sentence,reading);yield return new WaitForSeconds(reading);}
            if(!Commit("P2_EPILOGUE")){storyRetryRequired=true;ReleaseStory(raised,selfie,screen);yield break;}
            ReleaseStory(raised,selfie,screen);RefreshObjective();
            Hud.ShowBroadcastNotification("Tinggalkan RSUD lewat motor di pelataran IGD. Arsip basement masih dapat diperiksa.",7);
        }
        public void ReplayDamarVideo()
        {if(!StoryPlaying&&Completed("P2_EPILOGUE")&&!boss.Active&&!Dead)StartCoroutine(ReplayVideo());}
        private IEnumerator ReplayVideo()
        {
            StoryPlaying=true;storyReturnMode=Phone.currentScanMode;storyReturnApp=phoneView.Apps.CurrentApp;
            bool raised=Phone.isPhoneRaised,selfie=Phone.isSelfieCamera,screen=Phone.isScreenOn;
            if(Phone.batteryPercent<=0){StoryPlaying=false;Hud.ShowBroadcastNotification("Isi daya manual untuk memutar video.",4);yield break;}
            Phone.currentScanMode=ScanVisualMode.Normal;UiFocus.Cinematic=true;player.MovementLocked=true;phoneView.CinematicHandsetVisible=true;
            Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.isSelfieCamera=false;Phone.SyncVisualState();phoneView.Apps.Open(PhoneApp.Camera);phoneView.SetViewfinder(true);
            VideoPlaying=true;world.videoSet.SetActive(true);soundscape.Cue("Chain",player.cameraPivot.position);
            phoneView.ShowChapterReadout("ARSIP VIDEO DAMAR • 00:08",Color.white);
            yield return new WaitForSeconds(8);ReleaseStory(raised,selfie,screen);
        }
        private void ReleaseStory(bool raised,bool selfie,bool screen)
        {
            player.GetComponentInChildren<AnimatedRakaAvatar>().CinematicCrouch=false;
            VideoPlaying=StoryPlaying=false;world.videoSet.SetActive(false);UiFocus.Cinematic=false;player.MovementLocked=false;
            Phone.isPhoneRaised=raised;Phone.isSelfieCamera=selfie;Phone.isScreenOn=screen&&Phone.batteryPercent>0;Phone.SyncVisualState();
            Phone.currentScanMode=Phone.batteryPercent>0?storyReturnMode:ScanVisualMode.Normal;phoneView.Apps.Open(storyReturnApp);
            phoneView.CinematicHandsetVisible=false;phoneView.SetViewfinder(false);phoneView.ShowChapterReadout("",Color.white);
        }
        public void OpenExitPanel()
        {
            if(!Completed("P2_EPILOGUE")||Completed("P2_COMPLETE"))
            {Hud.ShowBroadcastNotification("Masih ada yang belum selesai di bangsal bersalin.",4);return;}
            CloseOverlay();overlay=UiWidgets.Panel("P2ExitPanel",Hud.GetComponentInChildren<Canvas>().transform,new Vector2(.2f,.24f),new Vector2(.8f,.76f),new Color(.014f,.022f,.025f,.99f));
            UiWidgets.Text("Title",overlay.transform,"TINGGALKAN RSUD",new Vector2(.06f,.75f),new Vector2(.94f,.94f),30);
            UiWidgets.Text("Archive",overlay.transform,"Arsip Babak 2: "+(Campaign.Data.secret_items.Contains(Phase2Ids.Secret)?"1/1":"0/1")+" • Arsip Saksi "+Phase1Ids.Secrets.Count(Campaign.Data.secret_items.Contains)+"/5\n\nTujuan berikutnya: "+Phase2Ids.Destination+"\nArsip yang terlewat masih bisa dicari di basement sebelum berangkat.",new Vector2(.06f,.32f),new Vector2(.94f,.74f),25);
            UiWidgets.Button("Back",overlay.transform,"Kembali menjelajah",new Vector2(.06f,.17f),new Vector2(.94f,.29f),CloseOverlay);
            UiWidgets.Button("ContinuePhase3",overlay.transform,"Lanjutkan",new Vector2(.06f,.03f),new Vector2(.94f,.15f),()=>ConfirmExit());
            UiFocus.Clock=true;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
        }
        public bool ConfirmExit()
        {
            if(!overlay||overlay.name!="P2ExitPanel"||!Phase2Campaign.Finish(Campaign))
            {Hud.ShowBroadcastNotification("Keberangkatan belum tersimpan. Pilih Lanjutkan untuk mencoba lagi.",4);return false;}
            CloseOverlay();soundscape.Cue("Shutter",player.transform.position);Hud.SetFadeAlpha(1);ShowPhase3Handoff();return true;
        }
        private void ShowPhase3Handoff()
        {
            RemoveOverlay();overlay=UiWidgets.Panel("P2Complete",Hud.GetComponentInChildren<Canvas>().transform,Vector2.zero,Vector2.one,Color.black);
            UiWidgets.Text("Title",overlay.transform,"DIAGNOSIS TENGAH MALAM",new Vector2(.2f,.63f),new Vector2(.8f,.75f),38);
            UiWidgets.Text("Next",overlay.transform,"SELESAI\n\nSegel kedua tersimpan.\nTujuan berikutnya: "+Phase2Ids.Destination,new Vector2(.2f,.36f),new Vector2(.8f,.61f),27);
            UiWidgets.Button("Menu",overlay.transform,"Menu utama",new Vector2(.35f,.2f),new Vector2(.65f,.3f),()=>{CloseOverlay();GameShell.Instance.OpenMain();});
            UiFocus.Clock=true;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;player.MovementLocked=true;
        }
    }
}
