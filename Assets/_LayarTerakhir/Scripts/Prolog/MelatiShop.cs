using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LayarTerakhir.Prolog
{
    public sealed class MelatiShop : MonoBehaviour,IInteractable
    {
        private GameObject menu;private Text balance,feedback;private bool raised;
        public bool IsOpen=>menu;
        public string InteractableId=>"WARUNG_BU_SARI";
        public bool CanInteract()=>!UiFocus.BlocksWorld&&!IsOpen;
        public string GetInteractionPrompt()=>"Belanja di Warung Bu Sari";
        public void Interact()
        {
            if(!CanInteract())return;
            raised=SmartphoneController.Instance.isPhoneRaised;SmartphoneController.Instance.isPhoneRaised=false;SmartphoneController.Instance.SyncVisualState();
            UiFocus.Clock=true;
            var hud=HudController.Instance;menu=PhoneSkin.Card("WarungBuSari",hud.GetComponentInChildren<Canvas>().transform,new Vector2(.16f,.15f),new Vector2(.77f,.85f),PhoneSkin.Ink);
            UiWidgets.Text("ShopTitle",menu.transform,"WARUNG BU SARI",new Vector2(.06f,.83f),new Vector2(.94f,.96f),42);
            balance=UiWidgets.Text("ShopBalance",menu.transform,"",new Vector2(.06f,.72f),new Vector2(.94f,.82f),26);
            string[] names={"Powerbank kecil  •  Rp45.000","Air mineral  •  Rp6.000","Mi instan siap makan  •  Rp12.000"};
            string[] ids={"POWERBANK_KECIL_KOS","AIR_MINERAL","BEKAL_MI"};long[] prices={45000,6000,12000};
            for(int i=0;i<names.Length;i++){int index=i;float y=.54f-i*.14f;PhoneSkin.Button("ShopBuy_"+ids[i],menu.transform,names[i],new Vector2(.06f,y),new Vector2(.94f,y+.105f),()=>Buy(ids[index],prices[index]));}
            feedback=UiWidgets.Text("ShopFeedback",menu.transform,"Malam, Mas. Mau bekal apa?",new Vector2(.06f,.13f),new Vector2(.94f,.25f),23);
            PhoneSkin.Button("ShopClose",menu.transform,"SELESAI BELANJA",new Vector2(.06f,.025f),new Vector2(.94f,.115f),Close);
            UpdateBalance();Cursor.lockState=CursorLockMode.None;Cursor.visible=true;
        }
        public bool Buy(string id,long price)
        {
            // Prices and stock are fixed by the shop; callers cannot inject a cheaper transaction.
            long expected=id=="POWERBANK_KECIL_KOS"?45000:id=="AIR_MINERAL"?6000:id=="BEKAL_MI"?12000:0;
            if(!IsOpen||expected==0||price!=expected)return false;
            var campaign=CampaignState.Instance;
            if(campaign.HasInventoryItem(id)){feedback.text="Barang itu masih ada di tasmu, Mas.";return false;}
            if(campaign.Data.wallet_legitimate+campaign.Data.wallet_bound<price){feedback.text="Uangnya belum cukup, Mas.";return false;}
            string before=JsonUtility.ToJson(campaign.Data);long legitimate=System.Math.Min(campaign.Data.wallet_legitimate,price);
            campaign.Data.wallet_legitimate-=legitimate;campaign.Data.wallet_bound-=price-legitimate;campaign.Data.spent+=price;campaign.AddInventoryItem(id);
            if(!campaign.SaveCheckpointAtomic(campaign.Data.checkpoint_id)){JsonUtility.FromJsonOverwrite(before,campaign.Data);feedback.text="Pembelian belum tersimpan. Dana dikembalikan.";return false;}
            feedback.text="Sudah masuk tas. Hati-hati di jalan, Mas.";UpdateBalance();return true;
        }
        private void UpdateBalance(){var d=CampaignState.Instance.Data;balance.text="Saldo Rp"+(d.wallet_legitimate+d.wallet_bound).ToString("N0",new System.Globalization.CultureInfo("id-ID"));}
        public void Close()
        {
            if(menu){menu.SetActive(false);Destroy(menu);menu=null;}UiFocus.Clock=false;
            var phone=SmartphoneController.Instance;if(phone){phone.isPhoneRaised=raised;phone.SyncVisualState();}
            Cursor.lockState=CursorLockMode.Locked;Cursor.visible=false;
        }
        private void OnDestroy(){if(menu)Destroy(menu);UiFocus.Clock=false;}
    }
}
