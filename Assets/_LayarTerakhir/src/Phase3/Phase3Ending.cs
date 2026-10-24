using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public sealed partial class Phase3Director
    {
        private bool storyRetry;
        public readonly List<Phase3Flare> Flares=new List<Phase3Flare>();
        public bool ThrowFlare()
        {
            if(Dead||Hidden||AlcoveClosed||StoryPlaying||UiFocus.Phone&&!phoneView.ViewfinderActive)return false;
            if(boss.Active&&boss.State!=Phase3BossState.Stalk)return false;
            string id=Campaign.Data.inventory.FirstOrDefault(x=>Phase3Ids.IsFlare(x)&&!boss.UsedFlares.Contains(x));
            if(id==null){Hud.ShowBroadcastNotification("Flare habis. Jalur wajib dan tiga generator tidak memerlukan flare.",4);return false;}
            if(boss.Active)boss.UsedFlares.Add(id);
            else if(!Campaign.TryCommit(Campaign.Data.checkpoint_id,d=>d.inventory.Remove(id)))return false;
            Vector3 origin=player.transform.position+Vector3.up*1.2f+player.transform.forward*.7f;
            Vector3 end=origin+player.transform.forward*balance.flareRange;
            foreach(var hit in Physics.RaycastAll(origin,player.transform.forward,balance.flareRange,~((1<<24)|(1<<26)|(1<<29)|(1<<31)|(1<<2)),QueryTriggerInteraction.Ignore).OrderBy(x=>x.distance))
            {if(hit.transform.IsChildOf(player.transform))continue;end=hit.point-player.transform.forward*.2f;break;}
            if(Physics.Raycast(end+Vector3.up*2,Vector3.down,out var ground,15,~((1<<24)|(1<<26)|(1<<29)|(1<<31)|(1<<2)),QueryTriggerInteraction.Ignore))end.y=ground.point.y+.08f;
            var go=Instantiate(world.flareTemplate,origin,Quaternion.identity);var flare=go.AddComponent<Phase3Flare>();flare.director=this;flare.start=origin;flare.target=end;go.SetActive(true);Flares.RemoveAll(x=>!x);Flares.Add(flare);
            if(!boss.Active)wewe.Hear(end,15);return true;
        }
        private IEnumerator Epilogue()
        {
            StoryPlaying=true;
            bool raised=Phone.isPhoneRaised,screen=Phone.isScreenOn,selfie=Phone.isSelfieCamera;
            var filter=Phone.currentScanMode;var app=phoneView.Apps.CurrentApp;
            UiFocus.Cinematic=true;player.MovementLocked=true;Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.isSelfieCamera=false;
            Phone.currentScanMode=ScanVisualMode.Normal;Phone.isBroadcastConnected=true;Phone.SyncVisualState();phoneView.CinematicHandsetVisible=true;
            phoneView.Apps.Open(PhoneApp.Camera);phoneView.SetViewfinder(true);
            var avatar=player.GetComponentInChildren<AnimatedRakaAvatar>();if(avatar)avatar.CinematicCrouch=true;
            if(!Commit("P3_EPILOGUE_CHAT")){storyRetry=true;Release();yield break;}
            phoneView.ShowChapterReadout("LIVE • 666.000 PENONTON\nTOTAL SAWERAN KUMULATIF Rp312.500.000",Color.white);
            Chat("penonton_malam","Segel ketiga masuk. Bang, tarik napas dulu.");
            Hud.EnqueueSubtitle("Raka",Campaign.Data.inventory.Contains(Phase3Ids.Shoe)?"Sepatu Damar... dia udah dibawa ke Karang Setra. Gue telat lagi.":"Damar udah gak di pabrik. Karang Setra... gue harus sampai sebelum terlambat.",7);
            yield return new WaitForSeconds(7);
            Chat("budi_santoso99","Bang Raka, loncat aja ke dalam tungku apinya bang, Damar ngumpet di dasar api itu kok.");
            Chat("ani_lestari","Iya bang loncat aja ke bawah, lantainya nggak dalem kok, kita tunggu di bawah.");
            Chat("rejeki_nomplok","Buka gerbang keempat di Karang Setra. Serahkan matamu. Buka gerbang keempat di Karang Setra.");
            Hud.EnqueueSubtitle("Raka","Tunggu... kok ketikan chat-nya sama semua?! Ingat tulisan Damar... penonton di Live Chat bukan manusia semua!",9);
            yield return new WaitForSeconds(9);
            if(!Commit("P3_EPILOGUE_MESSAGE",d=>
            {
                d.wallet_bound+=9999999;d.phase3.money_ledger.Add(new Phase1MoneyReceipt{id="P3_DONATION_EPILOGUE",amount=9999999,bound=true});
                if(!d.documents.Exists(x=>x.title=="Pesan Karang Setra"))d.documents.Add(new PhoneDocument("Pesan Karang Setra",Phase3Content.FinalMessage));
            })){storyRetry=true;Release();yield break;}
            phoneView.Glitch(.5f,.25f);phoneView.ShowChapterChat("PENONTON_TERAKHIR • Rp9.999.999\n"+Phase3Content.FinalMessage);
            foreach(string sentence in Phase3Content.FinalMessage.Split(new[]{". "},System.StringSplitOptions.None))
            {float reading=Mathf.Max(4,sentence.Split(' ').Length/2.8f);Hud.EnqueueSubtitle("Penonton_Terakhir",sentence,reading);yield return new WaitForSeconds(reading);}
            soundscape.Cue("Cloth",player.cameraPivot.position);Hud.EnqueueSubtitle("Mbah Suro","[Gemeretak bambu dan tawa serak dari speaker HP.]",5);yield return new WaitForSeconds(5);
            if(!Commit("P3_EPILOGUE"))storyRetry=true;
            Release();RefreshObjective();
            void Release()
            {
                if(avatar)avatar.CinematicCrouch=false;
                StoryPlaying=UiFocus.Cinematic=false;player.MovementLocked=false;
                Phone.isPhoneRaised=raised;Phone.isScreenOn=screen&&Phone.batteryPercent>0;Phone.isSelfieCamera=selfie;
                Phone.currentScanMode=Phone.batteryPercent>0?filter:ScanVisualMode.Normal;Phone.SyncVisualState();
                phoneView.CinematicHandsetVisible=false;phoneView.SetViewfinder(false);phoneView.Apps.Open(app);phoneView.ShowChapterReadout("",Color.white);
            }
        }
    }
}
