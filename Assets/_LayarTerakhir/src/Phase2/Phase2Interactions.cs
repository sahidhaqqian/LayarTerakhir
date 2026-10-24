using System.Linq;
using LayarTerakhir.Phase1;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase2
{
    public sealed partial class Phase2Director
    {
        private float beforeHideHeight;
        private Vector3 beforeHideCentre,beforeHideEye;
        public bool CanInteract(Phase2WorldItem item) => Ready && !Dead && !UiFocus.HardModal && !UiFocus.Cinematic && !Hud.IsModalOpen &&
            (!Hidden || item == hidingItem);
        public string ItemPrompt(Phase2WorldItem item)
        {
            if (Hidden && item == hidingItem) return "Keluar dari persembunyian";
            var drawer = item.GetComponent<Phase2Drawer>();
            if (drawer) return drawer.Prompt;
            if (item.id == "RITUAL_TABLE") return boss.Active ? boss.Status : Completed("P2_BOSS_WIN") ? "Meja operasi — segel Lastri tersimpan" : "Letakkan tiga pusaka — mulai ritual";
            if (item.id == "OR_DOOR") return Completed("P2_BOSS_WIN") || Phase2Ids.AllRelics(Campaign.Data) ? "Ruang Operasi — periksa persiapan ritual" : "Pintu berkabut — tiga pusaka belum lengkap";
            return item.prompt;
        }
        public IInteractable ResolveContextInteraction() => Hidden ? hidingItem : null;
        public void Interact(Phase2WorldItem item)
        {
            if (!CanInteract(item)) return;
            var drawer = item.GetComponent<Phase2Drawer>();
            if (drawer) { drawer.Interact(); return; }
            switch (item.id)
            {
                case "LOG": Read("P2_NURSE_LOG",Phase2Content.LogTitle,Phase2Content.Log); break;
                case "POLI_BOARD": Read("P2_POLI_BOARD","Jadwal Poliklinik","Poli 01 — Umum\nPoli 02 — Anak\nPoli 03 — Kandungan & Kebidanan\nPoli 04 — Gigi"); break;
                case "BASEMENT_PANEL":
                    if (Completed("P2_KEYPAD_1603")) break;
                    Keypad("1603",()=>Commit("P2_KEYPAD_1603",null,"PHASE2_CP2","IGD")); break;
                case "BASEMENT_EXIT":
                    if (Completed("P2_KEYPAD_1603")) world.basementDoor.SetActive(false);
                    break;
                case "TERALI":
                    if (!Campaign.HasInventoryItem(Phase2Ids.Key)) Hud.ShowBroadcastNotification("Teralis memerlukan kunci lantai dua. Periksa instalasi kamar mayat.",4);
                    else Commit("P2_STAIR_L2_UNLOCK",null,"PHASE2_CP4","IGD");
                    break;
                case "AUTOPSY": Read("P2_AUTOPSY_REPORT",Phase2Content.AutopsyTitle,Phase2Content.Autopsy,
                    d=>{ if (!d.inventory.Contains(Phase2Ids.Key)) d.inventory.Add(Phase2Ids.Key); }); break;
                case "ARCHIVE_NOTE": Read("P2_ARCHIVE_NOTE","Catatan perawat",Phase2Content.SecretNote); break;
                case "VICTIM_LIST": Read("P2_VICTIM_LIST","Daftar Jenazah Bayi — Bangsal Bersalin 2001",VictimList()); break;
                case "ARCHIVE_KEY": Take(item,"P2_TAKE_ARCHIVE_KEY",Phase2Ids.ArchiveKey); break;
                case "D1":
                    if (!Campaign.HasInventoryItem(Phase2Ids.ArchiveKey)) { Hud.ShowBroadcastNotification("Laci arsip terkunci. Papan kunci ada di gudang ini.",4); break; }
                    if (!Campaign.Data.secret_items.Contains(Phase2Ids.Secret) && Commit("P2_SECRET_PICKUP",d=>d.secret_items.Add(Phase2Ids.Secret),"PHASE2_CP4",SafeAnchor))
                        Hud.ShowBroadcastNotification("LABEL PASIEN TANPA NAMA TERSIMPAN • Arsip Saksi " + Phase1Ids.Secrets.Count(Campaign.Data.secret_items.Contains) + "/5",6);
                    break;
                case "GUNTING": Take(item,"P2_TAKE_GUNTING",Phase2Ids.Gunting,true); break;
                case "GELANG": Take(item,"P2_TAKE_GELANG",Phase2Ids.Gelang,true); break;
                case "BLOOD": Take(item,"P2_TAKE_BOTOL",Phase2Ids.Blood,true); break;
                case "DAMAR": Read("P2_DAMAR_RECORDER",Phase2Content.DamarTitle,Phase2Content.Damar); break;
                case "NURSE_NOTE": Read("P2_NURSE_NOTE",Phase2Content.NurseTitle,Phase2Content.Nurse); break;
                case "BIRTH_BOARD": Read("P2_BIRTH_BOARD","Penempatan bayi — Oktober 2001","01 — R.\n02 — S.\n03 — N.\n04 — L.\n05 — A.\n06 — D."); break;
                case "SAFE": if (!Completed("P2_NURSE_SAFE_OPEN")) Keypad("2604",()=>Commit("P2_NURSE_SAFE_OPEN",null,"PHASE2_CP4",SafeAnchor)); break;
                case "SERVICE_TRACE": Hud.EnqueueSubtitle("Raka","Gantungan kamera Damar... ada goresan menuju lantai atas. Terowongannya dirantai.",6); break;
                case "HIDE_TABLE_A": case "HIDE_TABLE_B": case "HIDE_LOCKER": ToggleHide(item); break;
                case "DYNAMO_IGD": case "DYNAMO_DUTY": case "DYNAMO_NURSE": StartCoroutine(Charge(item)); break;
                case "OR_DOOR":
                    if(Phase2Ids.AllRelics(Campaign.Data)||Completed("P2_BOSS_WIN"))
                    {if(!Completed("P2_BOSS_WIN"))Commit("P2_INVESTIGATION_COMPLETE",null,null,"NURSE");Hud.ShowBroadcastNotification("Ruang Operasi. Periksa meja; ritual hanya dimulai lewat interaksi sadar.",4);}
                    else Hud.ShowBroadcastNotification("Pintu berkabut. Lengkapi tiga pusaka.",4);
                    break;
                case "RITUAL_TABLE": if(boss.Active)boss.ContinueCycle();else if(!Completed("P2_BOSS_WIN"))boss.Begin();else if(!Completed("P2_EPILOGUE")){if(Phone.batteryPercent>0&&!StoryPlaying){storyRetryRequired=false;StartCoroutine(Epilogue());}else Hud.ShowBroadcastNotification("Isi daya di generator untuk menampilkan rekaman Damar.",4);} break;
                case "LAMP_0": case "LAMP_1": case "LAMP_2": boss.UseLamp(item.id[item.id.Length-1]-'0'); break;
                case "GENERATOR_NE": case "GENERATOR_SW": StartCoroutine(ChargeArena(item)); break;
                case "EXIT_MOTOR": OpenExitPanel(); break;
            }
        }
        private void Take(Phase2WorldItem item,string receipt,string id,bool relic = false)
        {
            if (!Commit(receipt,d=>
            {
                if (!d.inventory.Contains(id)) d.inventory.Add(id);
                if (relic && !d.relic_ids.Contains(id)) d.relic_ids.Add(id);
            },"PHASE2_CP4",SafeAnchor)) return;
            Hud.ShowBroadcastNotification(item.prompt+" — tersimpan",3);
        }
        private void Read(string receipt,string title,string text,System.Action<CampaignData> mutate = null)
        {
            if (!CanOpenHardModal()) return;
            if (!Commit(receipt,d=>
            {
                mutate?.Invoke(d);
                if (!d.documents.Exists(doc=>doc.title==title)) d.documents.Add(new PhoneDocument(title,text));
                string doc=receipt switch {"P2_NURSE_LOG"=>"P2_DOC_NURSE_LOG","P2_AUTOPSY_REPORT"=>"P2_DOC_AUTOPSY",
                    "P2_DAMAR_RECORDER"=>"P2_DOC_DAMAR_RECORDER","P2_NURSE_NOTE"=>"P2_DOC_NURSE_NOTE","P2_ARCHIVE_NOTE"=>"P2_CLUE_MORGUE_NOTE",_=>null};
                if(doc!=null&&!d.inventory.Contains(doc))d.inventory.Add(doc);
            },null,SafeAnchor)) return;
            Hud.OpenDocumentModal(title,text);
        }
        private bool CanOpenHardModal()
        {
            if (!lastri.Attached && !boss.Active) return true;
            Hud.ShowBroadcastNotification("Ada yang menempel. Lepaskan lewat Selfie sambil maju, atau sembunyi + matikan layar sebelum membaca.",5); return false;
        }
        private void Keypad(string code,System.Func<bool> commit)
        {
            if (!CanOpenHardModal()) return;
            Hud.OpenPadlockModal(code,()=>{ if (commit()) soundscape?.Cue("Latch",player.transform.position); });
            var heading=Hud.padlockPanel.transform.Find("PadlockHeader")?.GetComponent<UnityEngine.UI.Text>();
            if(heading)heading.text=code=="1603"?"AKSES BASEMENT · 4 DIGIT":"BRANKAS OBAT · 4 DIGIT";
            var separator=Hud.padlockPanel.transform.Find("DateSeparator");if(separator)separator.gameObject.SetActive(false);
            Hud.padlockFeedbackText.text="Atur angka sesuai petunjuk yang sudah kamu temukan.";
        }
        private void ToggleHide(Phase2WorldItem item)
        {
            if (Hidden)
            {
                var capsule=player.GetComponent<CharacterController>(); capsule.enabled=false;
                capsule.height=beforeHideHeight;capsule.center=beforeHideCentre;player.cameraPivot.localPosition=beforeHideEye;
                player.transform.SetPositionAndRotation(beforeHide,beforeHideRotation); capsule.enabled=true;
                Hidden=false; hidingItem=null; player.MovementLocked=false;
                player.GetComponent<ShoulderCameraRig>().FirstPersonOverride=false;
            }
            else
            {
                var anchor=item.transform.Find("HideAnchor"); if (!anchor) return;
                beforeHide=player.transform.position;beforeHideRotation=player.transform.rotation;
                var capsule=player.GetComponent<CharacterController>();
                beforeHideHeight=capsule.height;beforeHideCentre=capsule.center;beforeHideEye=player.cameraPivot.localPosition;
                capsule.height=.65f;capsule.center=Vector3.up*.325f;
                var eye=player.cameraPivot.localPosition;eye.y=.53f;player.cameraPivot.localPosition=eye;
                hidingItem=item;Hidden=true;MovePlayer(anchor);player.MovementLocked=true;
                player.GetComponent<ShoulderCameraRig>().FirstPersonOverride=true;
                Hud.ShowBroadcastNotification("Tersembunyi. Matikan layar dengan F. Tunggu napas menjauh. E selalu bisa keluar.",5);
            }
            player.GetComponent<ShoulderCameraRig>().RefreshCameraPose();
        }
        private readonly System.Collections.Generic.Dictionary<string,float> chargeProgress = new System.Collections.Generic.Dictionary<string,float>();
        private IEnumerator Charge(Phase2WorldItem item)
        {
            if (charging) yield break;
            var p=Campaign.Data.phase2;bool full=!p.dynamo_full_charge_used_ids.Contains(item.id);
            var last=p.dynamo_emergency_last_uses.Find(e=>e.id==item.id);
            if (!full && (Phone.batteryPercent>=10 || last!=null && p.elapsed_gameplay_seconds-last.at_gameplay_seconds<balance.crankCooldown))
            { Hud.ShowBroadcastNotification("Dinamo darurat: daya di bawah 10%; jeda 120 detik setelah pengisian terakhir.",4);yield break; }
            charging=true;float needed=full?balance.dynamoSeconds:balance.crankSeconds;
            if (!chargeProgress.ContainsKey(item.id)) chargeProgress[item.id]=0;
            while (!Dead && Vector3.Distance(player.transform.position+Vector3.up,item.transform.position)<2.6f && GameInputConfig.Instance.GetActionHeld(GameAction.Interact))
            {
                if (lastri.Attached) { Hud.ShowBroadcastNotification("Lepaskan Lastri atau bersembunyi dahulu. Putaran dinamo tersimpan selama sesi.",4);break; }
                if (ClockRunning) chargeProgress[item.id]+=Time.deltaTime;
                Hud.SetInteractionPrompt($"MEMUTAR DINAMO • {chargeProgress[item.id]:0}/{needed:0} detik");
                if (chargeProgress[item.id]>=needed)
                {
                    float amount=full?Mathf.Min(100,Phone.batteryPercent+balance.dynamoAmount):10;
                    if (Campaign.TryCommit(Campaign.Data.checkpoint_id,d=>
                    {
                        d.battery=amount;
                        if (full) d.phase2.dynamo_full_charge_used_ids.Add(item.id);
                        else
                        {
                            var r=d.phase2.dynamo_emergency_last_uses.Find(e=>e.id==item.id);
                            if(r==null)d.phase2.dynamo_emergency_last_uses.Add(new Phase1DynamoUse{id=item.id,at_gameplay_seconds=d.phase2.elapsed_gameplay_seconds});
                            else r.at_gameplay_seconds=d.phase2.elapsed_gameplay_seconds;
                        }
                    })) { Phone.RestoreBattery(amount); chargeProgress[item.id]=0;Hud.ShowBroadcastNotification("Pengisian dinamo tersimpan.",3); }
                    else Hud.ShowBroadcastNotification("Pengisian belum tersimpan. Coba lagi.",4);
                    break;
                }
                yield return null;
            }
            charging=false;
        }
        private static string VictimList() => "DAFTAR JENAZAH BAYI — BANGSAL BERSALIN 2001\n01 A\n02 B\n03 C\n04 D\n05 E\n06 F\n07 G\n08 H\n09 I\n10 J\n11 K\n12 L\n13 M\n14 N\n15 O\n16 P\n17 [                         ]";
    }
}
