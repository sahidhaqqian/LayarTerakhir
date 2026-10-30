using System.Collections.Generic;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;
namespace LayarTerakhir.Prolog
{
    public sealed class ExplorationCommentary:MonoBehaviour
    {
        private struct Zone {public string id,line;public Bounds bounds;public Zone(string i,string l,Vector3 p,Vector3 size){id=i;line=l;bounds=new Bounds(p,size);}}
        private readonly List<Zone> zones=new List<Zone>();
        private AtmajaDirector atmaja;
        private float nextComment;
        private void Start()
        {
            atmaja=FindFirstObjectByType<AtmajaDirector>();
            if(atmaja)
            {
                zones.Add(new Zone("ATMAJA_CHILD","Buku hariannya masih ditinggal begitu aja. Anak ini waktu itu sempat minta tolong?",new Vector3(-4.2f,4.6f,2.6f),new Vector3(3.4f,2.2f,4.8f)));
                zones.Add(new Zone("ATMAJA_UPPER_BATH","Lantai basah... padahal kerannya mati. Gue nggak suka ruangan ini.",new Vector3(-1.1f,4.6f,2.6f),new Vector3(2.3f,2.2f,4.8f)));
                zones.Add(new Zone("ATMAJA_MASTER","Bau kain tua di sini makin kuat. Damar sempat sampai kamar ini nggak, ya?",new Vector3(1.3f,4.6f,2.5f),new Vector3(1.8f,2.2f,4.7f)));
                zones.Add(new Zone("ATMAJA_BALCONY","Anginnya kencang, tapi yang bergerak cuma tali itu...",new Vector3(3.7f,4.6f,4.3f),new Vector3(2.3f,2.2f,1.5f)));
                zones.Add(new Zone("ATMAJA_UPPER_HALL","Jejak tangan ini masih kelihatan merah. Jangan-jangan Damar baru lewat sini.",new Vector3(-1.7f,4.6f,5.8f),new Vector3(8.8f,2.2f,1.1f)));
                zones.Add(new Zone("ATMAJA_BASEMENT","Ada suara dari arah meja-meja itu. Pelan-pelan, Ka... jangan bikin bunyi.",new Vector3(2.9f,-2.0f,3.7f),new Vector3(5.2f,2.2f,5.9f)));
                zones.Add(new Zone("ATMAJA_STORAGE","Debunya tebal... tapi ada bekas tangan baru di lemari ini.",new Vector3(.85f,1.7f,6.6f),new Vector3(1.2f,3,2)));
                zones.Add(new Zone("ATMAJA_KITCHEN","Bau air sumur sama kayu basah. Dam, lo masuk lewat sini juga?",new Vector3(4,1.7f,11),new Vector3(4,3,3)));
                zones.Add(new Zone("ATMAJA_DINING","Damar? Ini Raka. Kalau lo denger, jawab gue...",new Vector3(3.7f,1.7f,5.5f),new Vector3(4,3,6.3f)));
                zones.Add(new Zone("ATMAJA_LOUNGE","Jamnya masih jalan. Siapa yang ngurus rumah kosong ini?",new Vector3(-3.5f,1.7f,4.8f),new Vector3(5,3,5.5f)));
                zones.Add(new Zone("ATMAJA_FRONTROOM","Dingin banget di ruangan ini. Kayak baru ada orang lewat.",new Vector3(-3.5f,1.7f,1.2f),new Vector3(5,3,2.2f)));
                zones.Add(new Zone("ATMAJA_EAST","Dindingnya lembap... Jangan sampai sinyal HP ikut hilang di sini.",new Vector3(8.2f,1.2f,2.6f),new Vector3(3,3,3)));
                zones.Add(new Zone("ATMAJA_YARD","Motornya Damar. Tapi kok rumahnya sepi banget?",new Vector3(0,1.2f,-4),new Vector3(17,3,9)));
            }
            else if(FindFirstObjectByType<KosCorridorDirector>())
            {
                zones.Add(new Zone("KOS_BATH_WOMEN","Handuknya masih lembap. Pasti ada yang habis mandi tadi.",new Vector3(3.85f,1,5.2f),new Vector3(1.7f,2.4f,1.5f)));
                zones.Add(new Zone("KOS_BATH_MEN","Keran baknya netes lagi. Anak-anak kos belum ada yang benerin.",new Vector3(3.85f,1,3.3f),new Vector3(1.7f,2.4f,1.5f)));
                zones.Add(new Zone("KOS_KITCHEN","Masih ada bekas masak tadi sore. Biasanya jam segini ada yang bikin kopi.",new Vector3(-3.2f,1,4.25f),new Vector3(3,2.5f,3.1f)));
                zones.Add(new Zone("KOS_DINING","Meja makan kosong. Yang lain pasti udah tidur.",new Vector3(-3.9f,1,7.5f),new Vector3(4.4f,2.5f,2.7f)));
                zones.Add(new Zone("KOS_BATH","Kerannya belum rapat. Nanti gue ingetin anak-anak kos.",new Vector3(3.2f,1,4.25f),new Vector3(3,2.5f,3.2f)));
                zones.Add(new Zone("KOS_GROUND","Bu Haji kayaknya udah masuk kamar. Jangan berisik sampai bangunin beliau.",new Vector3(0,1,6),new Vector3(2.4f,2.4f,11)));
                zones.Add(new Zone("KOS_UPPER","Semua pintu udah ketutup. Cuma gue yang masih keluyuran jam segini.",new Vector3(0,4.4f,6),new Vector3(2.4f,2.4f,12)));
                zones.Add(new Zone("KOS_YARD","Motor masih basah. Dam, tunggu gue...",new Vector3(0,1,-4.2f),new Vector3(9,3,5.5f)));
            }
        }
        private void Update()
        {
            if(Time.time<nextComment||!CampaignState.Instance||!HudController.Instance||UiFocus.Cinematic||UiFocus.HardModal||HudController.Instance.IsModalOpen)return;
            if(atmaja&&(atmaja.Busy||atmaja.Stage>=AtmajaStage.Chase||(atmaja.basement&&(atmaja.basement.IsHidden||atmaja.basement.IsHunting))))return;
            var view=FindFirstObjectByType<AtmajaPhoneView>();if(view&&view.Apps&&view.Apps.IsCalling)return;
            Vector3 p=transform.position;if(atmaja){var local=atmaja.LocalPosition(p);p=new Vector3(local.x,p.y-atmaja.origin.y,local.y);}
            foreach(var zone in zones)
            {
                string id="COMMENT_"+zone.id;
                if(!zone.bounds.Contains(p)||CampaignState.Instance.HasInventoryItem(id))continue;
                CampaignState.Instance.AddInventoryItem(id);HudController.Instance.EnqueueSubtitle("Raka",zone.line,4.5f);nextComment=Time.time+8;break;
            }
        }
    }
}
