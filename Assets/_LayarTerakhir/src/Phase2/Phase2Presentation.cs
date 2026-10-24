using System.Collections.Generic;
using System.Linq;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase2
{
    public sealed partial class Phase2Director
    {
        private GameObject overlay;
        private readonly List<string> chatHistory = new List<string>();
        public void StartLive()
        {
            if (!CanBeginLive || !Commit("P2_START_LIVE",d=>
            {
                d.wallet_bound+=666000;
                d.phase2.money_ledger.Add(new Phase1MoneyReceipt{id="P2_DONATION_IGD",amount=666000,bound=true});
            })) return;
            Phone.isBroadcastConnected=true;
            Chat("dokter_jaga_malam","Merinding parah, itu RS Sido Waras yang ruang mayatnya di-seal polisi tahun 2001 kan?!");
            Chat("kunti_bogor","Bang Raka jangan lupa sering-sering nyalain kamera selfie bang, rumornya penunggu situ suka nempel di punggung!");
            Chat("Penonton_Terakhir • Rp666.000","Selamat datang di bangsal kelahiran kembali, Raka. Sahabatmu meninggalkan sesuatu di Kamar Mayat bawah tanah. Tapi hati-hati... Lastri tidak suka dipunggungi.");
            Hud.EnqueueSubtitle("Raka","Ekspedisi segel kedua. Gue udah di RSUD Sido Waras. Damar masih ditahan. Malam ini gue udah pasang sensor suhu sama kamera depan di HP.",9);
        }
        public void ScareReceipt()
        {
            if (!Completed("P2_LASTRI_SCARE1")) Commit("P2_LASTRI_SCARE1",d=>
            { d.wallet_bound+=1333000; d.phase2.money_ledger.Add(new Phase1MoneyReceipt{id="P2_DONATION_MORGUE",amount=1333000,bound=true}); });
            Chat("windah_bocil","ANJIR DI BELAKANG PUNGGUNG LU BANG ADA KUNTI MERAH NEMPEL!");
            Chat("suster_keramas","JANGAN NOLEH KE BELAKANG BANG RAKA! LIAT DARI KAMERA DEPAN AJA TERUS JALAN KE DEPAN!");
            Chat("Penonton_Terakhir • Rp1.333.000","Lihat betapa dekatnya dia dengan lehermu. Cari laci pendingin tempat bayinya disimpan, atau kau yang akan masuk ke laci berikutnya.");
        }
        private void Chat(string author,string line)
        {
            chatHistory.Add(author+": "+line);
            if (Phone.isBroadcastConnected) phoneView.ShowChapterChat(string.Join("\n\n",chatHistory.Skip(Mathf.Max(0,chatHistory.Count-3))));
        }
        public bool ConfirmJournal()
        {
            if (!Phase2Ids.AllEvidence(Campaign.Data)) return false;
            bool result=Commit("P2_JOURNAL_CONFIRMED",null,"PHASE2_CP4",SafeAnchor);
            if(result)Hud.ShowBroadcastNotification("LASTRI TERIDENTIFIKASI • Metode eksorsisme tersimpan di Jurnal.",5);
            return result;
        }
        public bool PopulateApp(PhoneApp app,Transform parent)
        {
            if (app==PhoneApp.Journal)
            {
                string Check(string id,string label)=>(Campaign.Data.evidence_ids.Contains(id)?"[✓] ":"[ ] ")+label;
                string body="JURNAL KLENIK — DIAGNOSIS TENGAH MALAM\n\n"+
                    Check(Phase2Ids.Freeze,"Suhu beku ekstrem — Poli 03 (−16°C)")+"\n"+
                    Check(Phase2Ids.Aura,"Residu aura tumbal — AR")+"\n"+Check(Phase2Ids.EVP,"EVP tangisan bersalin")+
                    "\n\nPUSAKA "+Phase2Ids.Relics.Count(Campaign.HasInventoryItem)+"/3\n"+
                    string.Join("\n",Phase2Ids.Relics.Select((id,i)=>(Campaign.HasInventoryItem(id)?"[✓] ":"[ ] ")+new[]{"Gunting Tali Pusar","Gelang Identitas Bayi Lastri","Botol Darah Segel"}[i]))+
                    "\n\nArsip Babak 2: "+(Campaign.Data.secret_items.Contains(Phase2Ids.Secret)?"1/1":"0/1")+" • Arsip Saksi "+Campaign.Data.secret_items.Count+"/5"+
                    (Completed("P2_DRAWER_CHAIN_BREAK")?"\nPetunjuk: Laci C3 −24°C.":"")+
                    (Completed("P2_SAFE_ECHO_SEEN")?"\nGema brankas obat: 2 — 6 — 0 — 4.":"")+
                    (Completed("P2_JOURNAL_CONFIRMED")?"\n\n"+Phase2Content.Method:"");
                UiWidgets.ScrollText("P2Deduction",parent,body,new Vector2(.05f,.36f),new Vector2(.95f,.90f),22);
                var confirm=PhoneSkin.Button("P2ConfirmIdentity",parent,Completed("P2_JOURNAL_CONFIRMED")?"IDENTITAS TERSIMPAN":"KONFIRMASI IDENTITAS & RITUAL",new Vector2(.05f,.23f),new Vector2(.95f,.34f),()=>
                { if(ConfirmJournal())phoneView.Apps.Open(PhoneApp.Journal); });
                confirm.interactable=Phase2Ids.AllEvidence(Campaign.Data)&&!Completed("P2_JOURNAL_CONFIRMED");
                PhoneSkin.Button("P2Documents",parent,"Dokumen tersimpan ›",new Vector2(.05f,.1f),new Vector2(.95f,.21f),()=>ShowDocuments(parent));
                return true;
            }
            string text;
            if(app==PhoneApp.Map) text="RSUD LAMA SIDO WARAS\n\nL1 — IGD / Poli 01–04\n         Basement: panel elektronik\n         Teralis: tangga Bangsal Bersalin\n\nB1 — Pos Petugas / kamar mayat / arsip\n\nL2 — Inkubator / Ruang Jaga Suster\n          Ruang Operasi Bersalin Utama\n\nTanda ruangan adalah petunjuk rute. Tidak ada panah menuju pusaka.";
            else if(app==PhoneApp.Messages) text="RIWAYAT SIARAN LOKAL\n\n"+string.Join("\n\n",chatHistory)+
                (Completed("P2_START_LIVE")?"\nPenonton_Terakhir • Rp666.000 (dana terikat)\nLastri tidak suka dipunggungi.":"")+
                (Completed("P2_LASTRI_SCARE1")?"\nCari laci pendingin tempat bayinya disimpan. Jangan menoleh langsung.":"");
            else if(app==PhoneApp.Inventory)
            {
                text="BARANG BAWAAN\n\n";
                foreach(var item in Campaign.Data.inventory)
                {var label=Phase2Content.ItemLabel(item);if(label!=null)text+="• "+label+"\n";}
                text+="\nKemenyan Bakar belum dapat digunakan. Flare hanya digunakan pada ritual; tidak terkonsumsi di investigasi.";
            }
            else return false;
            UiWidgets.ScrollText("P2App",parent,text,new Vector2(.05f,.1f),new Vector2(.95f,.9f),22);return true;
        }
        private void ShowDocuments(Transform parent)
        {
            var sheet=PhoneSkin.Card("P2Documents",parent,new Vector2(.01f,.08f),new Vector2(.99f,.93f),PhoneSkin.Ink);
            string text=string.Join("\n\n————\n\n",Campaign.Data.documents.Select(d=>d.title+"\n\n"+d.body));
            UiWidgets.ScrollText("StoredDocuments",sheet.transform,text,new Vector2(.05f,.15f),new Vector2(.95f,.97f),22);
            PhoneSkin.Button("Back",sheet.transform,"‹ Kembali",new Vector2(.05f,.02f),new Vector2(.95f,.13f),()=>Destroy(sheet));
        }
        private void ShowInvestigationGate()
        {
            if(!CanOpenHardModal())return;
            if(!Phase2Ids.AllRelics(Campaign.Data)) { Hud.ShowBroadcastNotification("Kabut tertahan. Tiga pusaka harus dibawa bersama.",4);return; }
            bool ready=Phase2Ids.RitualReady(Campaign.Data);
            if(ready&&!Commit("P2_INVESTIGATION_COMPLETE",null,"PHASE2_CP4","NURSE"))return;
            CloseOverlay();
            overlay=UiWidgets.Panel("P2InvestigationBoundary",Hud.GetComponentInChildren<Canvas>().transform,new Vector2(.2f,.22f),new Vector2(.8f,.78f),new Color(.014f,.022f,.025f,.99f));
            UiWidgets.Text("Title",overlay.transform,"RUANG OPERASI BERSALIN UTAMA",new Vector2(.06f,.73f),new Vector2(.94f,.94f),30);
            UiWidgets.Text("Body",overlay.transform,ready?"Investigasi selesai dan tersimpan. Tiga pusaka, tiga bukti, dan identitas Lastri sudah lengkap.\n\nBatas Sesi 1: ritual di dalam Ruang Operasi dilanjutkan pada Sesi 2. Kamu masih bisa kembali mencari Arsip Saksi.":
                "Tiga pusaka sudah menyingkirkan kabut. Lengkapi tiga bukti dan konfirmasi Jurnal sebelum ritual.\n\nKamu masih bisa kembali ke seluruh area investigasi.",new Vector2(.06f,.25f),new Vector2(.94f,.72f),26);
            UiWidgets.Button("Back",overlay.transform,"Kembali menjelajah",new Vector2(.06f,.07f),new Vector2(.94f,.22f),CloseOverlay);
            UiFocus.Clock=true;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
        }
        public void CloseOverlay() { if(overlay)Destroy(overlay);overlay=null;UiFocus.Clock=false; }
    }
}
