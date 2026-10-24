using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.Phase1;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Phase3
{
    public sealed partial class Phase3Director
    {
        private GameObject overlay;
        private readonly List<string> chatHistory=new List<string>();
        public void StartLive()
        {
            if(!CanBeginLive||!Commit("P3_START_LIVE",d=>{d.wallet_bound+=3333333;d.phase3.money_ledger.Add(new Phase1MoneyReceipt{id="P3_DONATION_YARD",amount=3333333,bound=true});}))return;
            Phone.isBroadcastConnected=true;
            Chat("pabrik_hantu","Gelap banget bang. Senter HP tidak tembus sampai ujung ruangan.");
            Chat("rina_cirebon","Hati-hati lubang gilingan di bawah lantai kayu!");
            Chat("Penonton_Terakhir • Rp3.333.333","Sahabatmu kedinginan di dekat tungku utama, Raka. Dia sudah memanggil namamu. Cepat lari ke sana kalau kau peduli.");
            Hud.EnqueueSubtitle("Raka","Ekspedisi segel ketiga. Gue di Pabrik Gula Sindang Laut. Semalam kalian liat sendiri rekaman Damar diikat di ruang tungku. Bantu gue pantau kalau ada yang janggal.",10);
        }
        private void Chat(string who,string line)
        {chatHistory.Add(who+": "+line);if(Phone.isBroadcastConnected)phoneView.ShowChapterChat(string.Join("\n\n",chatHistory.Skip(Mathf.Max(0,chatHistory.Count-3))));}
        public bool ConfirmJournal()
        {
            if(!Phase3Ids.AllEvidence(Campaign.Data))return false;
            bool ok=Commit("P3_JOURNAL_CONFIRMED",null,"PHASE3_CP4",SafeAnchor);
            if(ok)Hud.ShowBroadcastNotification("WEWE GOMBEL TERIDENTIFIKASI • metode ritual tersimpan.",5);
            return ok;
        }
        public bool PopulateApp(PhoneApp app,Transform parent)
        {
            if(app==PhoneApp.Journal)
            {
                string body="NYANYIAN DARI KETEL UAP\n\n"+string.Join("\n",Phase3Ids.Evidence.Select((id,i)=>(Campaign.Data.evidence_ids.Contains(id)?"[✓] ":"[ ] ")+new[]{"Jejak Gendongan Tumbal — AR","Frekuensi EVP Nina Bobo — segmen ASLI","Rajah Penyembunyi Anak — UV"}[i]))+
                    "\n\n"+string.Join("\n",Phase3Ids.Relics.Select(id=>(Campaign.Data.relic_ids.Contains(id)?"[✓] ":"[ ] ")+Phase3Content.ItemLabel(id)))+
                    "\n\n"+ArchiveStatus()+"\n\n"+(Completed("P3_JOURNAL_CONFIRMED")?Phase3Content.Method:"Baca catatan mandor dan pola EVP. Konfirmasi identitas setelah tiga bukti lengkap.");
                UiWidgets.ScrollText("P3Journal",parent,body,new Vector2(.05f,.36f),new Vector2(.95f,.9f),22);
                var c=PhoneSkin.Button("P3Confirm",parent,Completed("P3_JOURNAL_CONFIRMED")?"IDENTITAS TERSIMPAN":"KONFIRMASI IDENTITAS",new Vector2(.05f,.23f),new Vector2(.95f,.34f),()=>{if(ConfirmJournal())phoneView.Apps.Open(PhoneApp.Journal);});
                c.interactable=Phase3Ids.AllEvidence(Campaign.Data)&&!Completed("P3_JOURNAL_CONFIRMED");
                PhoneSkin.Button("P3Documents",parent,"Dokumen & Arsip Saksi ›",new Vector2(.05f,.09f),new Vector2(.95f,.21f),()=>ShowDocuments(parent));return true;
            }
            string text;
            if(app==PhoneApp.Map)text="PABRIK GULA SINDANG LAUT\n\nHalaman Timbangan → Gilingan\nJalur pipa kanan → G-ACC → Ayunan\nBangsal Ketel: dua rute antar-boiler\nTangga → Kantor Mandor L2 / balkon servis\nL1 barat → Gudang Ampas\nAula: N / S / E mengelilingi tungku\nSesudah segel: gerbang servis → lorong lori → motor\n\nSilang UV = rapuh. Suara ASLI bukan jaminan lantai aman.";
            else if(app==PhoneApp.Inventory)text="BARANG BAWAAN\n\n"+string.Join("\n",Campaign.Data.inventory.Where(id=>!boss.UsedFlares.Contains(id)).Select(Phase3Content.ItemLabel).Where(x=>x!=null).Select(x=>"• "+x))+"\n\nKemenyan disimpan untuk Babak 4; tidak dikonsumsi di pabrik.";
            else if(app==PhoneApp.Messages)text="RIWAYAT SIARAN LOKAL\n\n"+string.Join("\n\n",chatHistory)+(Completed("P3_START_LIVE")?"\n\nPenonton_Terakhir • Rp3.333.333 — dana terikat\nJangan langsung berlari ke suara Damar.":"")+(Completed("P3_EPILOGUE_MESSAGE")?"\n\n"+Phase3Content.FinalMessage:"");
            else return false;
            UiWidgets.ScrollText("P3App",parent,text,new Vector2(.05f,.1f),new Vector2(.95f,.9f),22);return true;
        }
        private string ArchiveStatus()=>"Arsip Babak 3: "+(Campaign.Data.secret_items.Contains(Phase3Ids.Secret)?"1/1":"0/1")+" • Arsip Saksi "+Phase1Ids.Secrets.Count(Campaign.Data.secret_items.Contains)+"/5";
        private void ShowDocuments(Transform parent)
        {
            var card=PhoneSkin.Card("P3Documents",parent,new Vector2(.01f,.08f),new Vector2(.99f,.93f),PhoneSkin.Ink);
            string text=string.Join("\n\n————\n\n",Campaign.Data.documents.Select(x=>x.title+"\n\n"+x.body));
            if(Campaign.Data.secret_items.Contains(Phase3Ids.Secret))text+="\n\n"+Phase3Content.Archive;
            UiWidgets.ScrollText("StoredDocuments",card.transform,text,new Vector2(.05f,.15f),new Vector2(.95f,.97f),22);
            PhoneSkin.Button("Back",card.transform,"‹ Kembali",new Vector2(.05f,.02f),new Vector2(.95f,.13f),()=>Destroy(card));
        }
        private void RemoveOverlay(){if(overlay)Destroy(overlay);overlay=null;UiFocus.Clock=false;}
        public void CloseOverlay(){bool complete=Completed("P3_COMPLETE");RemoveOverlay();if(complete)GameShell.Instance.OpenMain();}
        public void OpenExitPanel()
        {
            if(!Completed("P3_EPILOGUE")||Completed("P3_COMPLETE")){Hud.ShowBroadcastNotification("Masih ada yang belum selesai di pabrik.",4);return;}
            RemoveOverlay();overlay=UiWidgets.Panel("P3ExitPanel",Hud.GetComponentInChildren<Canvas>().transform,new Vector2(.18f,.22f),new Vector2(.82f,.78f),new Color(.014f,.022f,.025f,.99f));
            UiWidgets.Text("Title",overlay.transform,"TINGGALKAN PABRIK GULA",new Vector2(.06f,.75f),new Vector2(.94f,.95f),30);
            UiWidgets.Text("Status",overlay.transform,ArchiveStatus()+"\n\nTujuan: "+Phase3Ids.Destination+"\nBalkon servis masih dapat diperiksa sebelum pergi.",new Vector2(.06f,.33f),new Vector2(.94f,.73f),25);
            UiWidgets.Button("Back",overlay.transform,"Kembali Menjelajah",new Vector2(.06f,.18f),new Vector2(.94f,.3f),CloseOverlay);
            UiWidgets.Button("ContinuePhase4",overlay.transform,"Lanjutkan",new Vector2(.06f,.03f),new Vector2(.94f,.15f),()=>ConfirmExit());
            UiFocus.Clock=true;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
        }
        public bool ConfirmExit()
        {
            if(!overlay||overlay.name!="P3ExitPanel"||!Phase3Campaign.Finish(Campaign))
            {Hud.ShowBroadcastNotification("Keberangkatan belum tersimpan. Coba Lanjutkan lagi; posisi dan layar tetap.",5);return false;}
            RemoveOverlay();soundscape.Cue("Shutter",player.transform.position);Hud.SetFadeAlpha(1);ShowHandoff();return true;
        }
        private void ShowHandoff()
        {
            RemoveOverlay();overlay=UiWidgets.Panel("P3Complete",Hud.GetComponentInChildren<Canvas>().transform,Vector2.zero,Vector2.one,Color.black);
            UiWidgets.Text("Title",overlay.transform,"NYANYIAN DARI KETEL UAP",new Vector2(.18f,.62f),new Vector2(.82f,.76f),38);
            UiWidgets.Text("Next",overlay.transform,"SELESAI\n\nSegel ketiga tersimpan.\nTujuan berikutnya: "+Phase3Ids.Destination,new Vector2(.18f,.35f),new Vector2(.82f,.60f),27);
            UiWidgets.Button("Menu",overlay.transform,"Menu utama",new Vector2(.35f,.18f),new Vector2(.65f,.30f),CloseOverlay);
            UiFocus.Clock=true;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;player.MovementLocked=true;
        }
    }
}
