using System.Linq;
using System.Collections;
using System.Collections.Generic;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public sealed partial class Phase3Director
    {
        private bool charging,cranking,alcoveClosed;
        private int accessCranks;
        private float chargeArenaProgress,resetProgress;
        private readonly Dictionary<string,float> dynamoProgress=new Dictionary<string,float>();
        private float hideHeight;
        private Vector3 hideCentre,hideEye;
        public bool AlcoveClosed=>alcoveClosed;
        public bool InAlcove=>world.alcove.Contains(player.transform.position+Vector3.up);
        public bool CanInteract(Phase3WorldItem item)=>Ready&&!Dead&&!RecordingEVP&&!GameInputConfig.Instance.GetActionHeld(GameAction.ModeEVP)&&!GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen)&&!UiFocus.HardModal&&!UiFocus.Cinematic&&!Hud.IsModalOpen&&(!Hidden||item==hidingItem);
        public string ItemPrompt(Phase3WorldItem item)
        {
            if(Hidden&&item==hidingItem)return "Keluar dari persembunyian";
            if(item.id=="G_ACC")return Completed("P3_ACCESS_GENERATOR")?"Generator akses aktif — gerbang terbuka":$"Engkol generator akses • {accessCranks}/3";
            if(item.id=="DECOY")return boss.Active?boss.Status:Completed("P3_BOSS_WIN")?"Segel ketiga tersimpan":"Damar? Periksa ikatan sosok itu";
            if(item.id.StartsWith("LAMP_"))
            {
                int i=item.id[item.id.Length-1]-'0';
                if(Completed("P3_BOSS_WIN"))return "Lampu ritual padam — segel tersimpan";
                if(boss.Spent[i])return "Generator korslet — gunakan sorotan lain";
                if(boss.Cooldowns[i]>0)return $"Generator mendingin • {boss.Cooldowns[i]:0}s";
                return boss.State==Phase3BossState.Stalk?"Nyalakan sorotan — pancing Wewe ke cahaya":"Generator siap — menunggu ritual";
            }
            if(item.id=="LOCKER_09"&&Campaign.Data.secret_items.Contains(Phase3Ids.Secret))return "Loker 09 — pelat telah disimpan";
            if(item.id.StartsWith("POWER_")&&boss.Active&&boss.PowerTokens==0)return "Jatah rak daya habis — tutup alcove dan engkol reset";
            return item.prompt;
        }
        public IInteractable ResolveContextInteraction()=>Hidden?hidingItem:null;
        public void Interact(Phase3WorldItem item)
        {
            if(!CanInteract(item))return;
            string id=item.id;
            if(id.StartsWith("ALTAR_")){PlaceAltar(id[id.Length-1]-'0');return;}
            if(id.StartsWith("LAMP_")){boss.UseLamp(id[id.Length-1]-'0');return;}
            if(id.StartsWith("DYNAMO_")){StartCoroutine(ChargeDynamo(item));return;}
            if(id.StartsWith("POWER_")){StartCoroutine(ChargeArena(item));return;}
            switch(id)
            {
                case "G_ACC":StartCoroutine(CrankAccess(item));break;
                case "MANDOR":Read("P3_DOC_MANDOR","Buku Catatan Mandor Pabrik — 1999",Phase3Content.Mandor);break;
                case "DOLL":Take("P3_TAKE_BONEKA",Phase3Ids.Doll);break;
                case "SICKLE":Take("P3_TAKE_SABIT",Phase3Ids.Sickle);break;
                case "CLOTH":Take("P3_TAKE_SELENDANG",Phase3Ids.Cloth);break;
                case "DAMAR_CARD":Read("P3_DOC_DAMAR_CARD","Kartu Pers Damar — tulisan arang",Phase3Content.Damar);break;
                case "DAMAR_SHOE":Commit("P3_TAKE_SHOE",d=>{if(!d.inventory.Contains(Phase3Ids.Shoe))d.inventory.Add(Phase3Ids.Shoe);},null,SafeAnchor);break;
                case "HIDE_LOCKER":case "HIDE_FURNACE":ToggleHide(item);break;
                case "VALVE_09":StartCoroutine(OpenValve(item));break;
                case "VALVE_01":case "VALVE_08":Hud.ShowBroadcastNotification("Katup jalur uap. Bukan sumber ketukan itu.",3);break;
                case "LOCKER_09":
                    if(!Completed("P3_VALVE_09")){Hud.ShowBroadcastNotification("Loker terkunci. Ada ketukan dari ventilasi balkon.",4);break;}
                    if(!Campaign.Data.secret_items.Contains(Phase3Ids.Secret)&&Commit("P3_SECRET_PICKUP",d=>d.secret_items.Add(Phase3Ids.Secret),null,SafeAnchor))
                        Hud.ShowBroadcastNotification("PELAT BURUH 09 TERSIMPAN • Arsip Saksi "+LayarTerakhir.Phase1.Phase1Ids.Secrets.Count(Campaign.Data.secret_items.Contains)+"/5",6);
                    break;
                case "DECOY":if(boss.Active)boss.ContinueCycle();else if(!Completed("P3_BOSS_WIN"))boss.Begin();else if(!Completed("P3_EPILOGUE")&&Phone.batteryPercent>0&&!StoryPlaying){storyRetry=false;StartCoroutine(Epilogue());}break;
                case "HALL_GATE":Hud.ShowBroadcastNotification(Phase3Ids.RitualReady(Campaign.Data)?"Aula terbuka. Pasang tiga pusaka lalu periksa sosok terikat.":"Lengkapi tiga bukti, konfirmasi Jurnal, dan bawa tiga pusaka.",4);break;
                case "SERVICE_EXIT":Hud.ShowBroadcastNotification(Completed("P3_BOSS_WIN")?"Lorong lori menuju halaman.":"Gerbang servis dilas. Terpisah dari alcove pemulihan.",4);break;
                case "ALCOVE_DOOR":
                    if(!InAlcove){Hud.ShowBroadcastNotification("Masuk alcove servis dahulu sebelum menutup pintu dari dalam.",3);break;}
                    alcoveClosed=!alcoveClosed;world.serviceDoor.GetComponent<Phase3Gate>().SetOpen(!alcoveClosed);break;
                case "ALCOVE_RESET":StartCoroutine(ResetPower(item));break;
                case "FLARE_RACK":boss.TakeRackFlare();break;
                case "EXIT_MOTOR":OpenExitPanel();break;
            }
        }
        private void PlaceAltar(int index)
        {
            if(index<0||index>=3||boss.Active||Completed("P3_BOSS_WIN"))return;
            if(!Campaign.Data.inventory.Contains(Phase3Ids.Relics[index])){Hud.ShowBroadcastNotification("Pusaka dudukan ini belum dibawa.",3);return;}
            Altar[index]=!Altar[index];RefreshWorld();
        }
        private void Take(string receipt,string id)
        {
            string cp=CurrentRoom=="MILL"?Campaign.Data.checkpoint_id:"PHASE3_CP4";
            if(Commit(receipt,d=>{if(!d.inventory.Contains(id))d.inventory.Add(id);if(!d.relic_ids.Contains(id))d.relic_ids.Add(id);},cp,SafeAnchor))
                Hud.ShowBroadcastNotification(Phase3Content.ItemLabel(id)+" — tersimpan",4);
        }
        private void Read(string receipt,string title,string text,System.Action<CampaignData> mutate=null)
        {
            if(boss.Active)return;
            if(Commit(receipt,d=>{mutate?.Invoke(d);if(!d.documents.Exists(x=>x.title==title))d.documents.Add(new PhoneDocument(title,text));if(!d.inventory.Contains(receipt))d.inventory.Add(receipt);},null,SafeAnchor))
            {
                if(wewe.Pursuing)Hud.ShowBroadcastNotification("Catatan tersimpan. Baca ulang di Jurnal setelah aman.",4);
                else Hud.OpenDocumentModal(title,text);
            }
        }
        private IEnumerator CrankAccess(Phase3WorldItem item)
        {
            if(cranking||Completed("P3_ACCESS_GENERATOR"))yield break;
            cranking=true;float progress=0;
            while(!Dead&&progress<balance.crankSeconds&&Near(item,2.6f))
            {if(ClockRunning)progress+=Time.deltaTime;yield return null;}
            if(progress>=balance.crankSeconds)
            {
                accessCranks++;
                if(accessCranks>=3&&!Commit("P3_ACCESS_GENERATOR"))accessCranks=2;
                soundscape.Cue("Generator",item.transform.position);
            }
            cranking=false;
        }
        private bool Near(Phase3WorldItem item,float range)
        {
            // The visible collider is authoritative: downloaded mesh origins can sit
            // behind the alcove trim even when the physical interaction ray is clear.
            var focused=player.GetComponent<InteractionInteractor>();
            if(focused&&object.ReferenceEquals(focused.CurrentTarget,item))return true;
            return Vector3.Distance(player.transform.position+Vector3.up,item.transform.position)<range&&ClearLine(player.transform.position+Vector3.up,item.transform.position,item.transform);
        }
        private IEnumerator OpenValve(Phase3WorldItem item)
        {
            if(Completed("P3_VALVE_09"))yield break;
            if(!Completed("P3_SECRET_CLUE")||!Completed("P3_SECRET_UV_ROUTE")){Hud.ShowBroadcastNotification("Verifikasi ketukan EVP, lalu periksa jejak UV di balkon.",4);yield break;}
            float t=0;
            while(!Dead&&Near(item,2.6f)&&GameInputConfig.Instance.GetActionHeld(GameAction.Interact)&&t<1)
            {if(ClockRunning)t+=Time.deltaTime;yield return null;}
            if(t>=1)Commit("P3_VALVE_09",null,null,SafeAnchor);
        }
        private void ToggleHide(Phase3WorldItem item)
        {
            var capsule=player.GetComponent<CharacterController>();
            if(Hidden)
            {
                capsule.enabled=false;capsule.height=hideHeight;capsule.center=hideCentre;player.cameraPivot.localPosition=hideEye;
                player.transform.SetPositionAndRotation(beforeHide,beforeHideRotation);capsule.enabled=true;
                Hidden=false;hidingItem=null;player.MovementLocked=false;player.GetComponent<ShoulderCameraRig>().FirstPersonOverride=false;
            }
            else
            {
                var a=item.transform.Find("HideAnchor");if(!a)return;
                beforeHide=player.transform.position;beforeHideRotation=player.transform.rotation;hideHeight=capsule.height;hideCentre=capsule.center;hideEye=player.cameraPivot.localPosition;
                capsule.height=.65f;capsule.center=Vector3.up*.325f;var eye=hideEye;eye.y=.53f;player.cameraPivot.localPosition=eye;
                Hidden=true;hidingItem=item;MovePlayer(a);player.MovementLocked=true;player.GetComponent<ShoulderCameraRig>().FirstPersonOverride=true;
                Hud.ShowBroadcastNotification("Matikan layar dengan "+GameInputConfig.Instance.GetActionLabel(GameAction.ToggleScreen)+". Tunggu langkah menjauh. E untuk keluar.",6);
            }
            player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
        private IEnumerator ChargeDynamo(Phase3WorldItem item)
        {
            if(charging||boss.Active)yield break;
            var p=Campaign.Data.phase3;bool full=!p.dynamo_full_charge_used_ids.Contains(item.id);
            var last=p.dynamo_emergency_last_uses.Find(x=>x.id==item.id);
            if(!full&&(Phone.batteryPercent>=10||last!=null&&Elapsed-last.at_gameplay_seconds<balance.emergencyCooldown))
            {Hud.ShowBroadcastNotification("Dinamo darurat: daya di bawah 10%, jeda 120 detik setelah pengisian sukses.",4);yield break;}
            charging=true;if(!dynamoProgress.ContainsKey(item.id))dynamoProgress[item.id]=0;
            float need=full?balance.dynamoSeconds:balance.emergencySeconds;
            while(!Dead&&Near(item,2.6f)&&GameInputConfig.Instance.GetActionHeld(GameAction.Interact))
            {
                if(wewe.Pursuing||GameInputConfig.Instance.GetActionDown(GameAction.ToggleScreen))break;
                if(ClockRunning)dynamoProgress[item.id]+=Time.deltaTime;
                Hud.SetInteractionPrompt($"ENGKOL HP • {dynamoProgress[item.id]:0.0}/{need:0}s");
                if(dynamoProgress[item.id]>=need)
                {
                    float after=full?Mathf.Min(100,Phone.batteryPercent+25):10;
                    if(Campaign.TryCommit(Campaign.Data.checkpoint_id,d=>
                    {
                        d.battery=after;
                        if(full)d.phase3.dynamo_full_charge_used_ids.Add(item.id);
                        else{var r=d.phase3.dynamo_emergency_last_uses.Find(x=>x.id==item.id);if(r==null)d.phase3.dynamo_emergency_last_uses.Add(new Phase1DynamoUse{id=item.id,at_gameplay_seconds=Elapsed});else r.at_gameplay_seconds=Elapsed;}
                    })){Phone.RestoreBattery(after);dynamoProgress[item.id]=0;}break;
                }
                yield return null;
            }
            charging=false;
        }
        private IEnumerator ChargeArena(Phase3WorldItem item)
        {
            if(charging||!boss.CanCharge||boss.PowerTokens<=0&&!Completed("P3_BOSS_WIN"))yield break;
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"P3_CHARGE begin {item.id} near={Near(item,2.6f)} token={boss.PowerTokens} energy={Phone.batteryPercent}");
#endif
            charging=true;
            while(!Dead&&boss.CanCharge&&Near(item,2.6f)&&GameInputConfig.Instance.GetActionHeld(GameAction.Interact))
            {
                if(ClockRunning)chargeArenaProgress+=Time.deltaTime;
                Hud.SetInteractionPrompt($"RAK DAYA MANUAL • {chargeArenaProgress:0.0}/{balance.powerSeconds:0}s");
                if(chargeArenaProgress>=balance.powerSeconds)
                {
                    float after=Mathf.Min(100,Phone.batteryPercent+balance.powerAmount);
                    if(boss.Active||Campaign.TryCommit(Campaign.Data.checkpoint_id,d=>d.battery=after))
                    {Phone.RestoreBattery(after);boss.SpendPower();chargeArenaProgress=0;}break;
                }
                yield return null;
            }
            #if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"P3_CHARGE end {item.id} near={Near(item,2.6f)} token={boss.PowerTokens} energy={Phone.batteryPercent} progress={chargeArenaProgress} held={GameInputConfig.Instance.GetActionHeld(GameAction.Interact)}");
#endif
            charging=false;
        }
        private IEnumerator ResetPower(Phase3WorldItem item)
        {
            if(charging||!boss.Recovering||!InAlcove||!AlcoveClosed||boss.ResetCooldown>0)yield break;
            charging=true;
            while(!Dead&&boss.Recovering&&AlcoveClosed&&Near(item,2.6f)&&GameInputConfig.Instance.GetActionHeld(GameAction.Interact))
            {
                if(ClockRunning)resetProgress+=Time.deltaTime;
                Hud.SetInteractionPrompt($"RESET RAK DAYA • {resetProgress:0.0}/{balance.resetSeconds:0}s");
                if(resetProgress>=balance.resetSeconds){boss.ResetPower();resetProgress=0;break;}
                yield return null;
            }
            charging=false;
        }
    }
}
