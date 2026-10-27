using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LayarTerakhir.Prolog
{
    public sealed class PrologFinaleView : MonoBehaviour
    {
        public const string FinalContract = "Satu malam selesai. Empat segel harus kau kumpulkan sebelum gerbang kelima terbuka. Sekarang kau tahu di mana sahabatmu berada. Kalau kau berani mematikan siaranmu besok malam, kepala Damar yang akan tampil di layar kameramu. Istirahatlah di kosmu... Besok malam pukul 23:59, kita bertemu di Rusunawa Terbengkalai Griya Asri.";
        public bool ContractRead { get; private set; }
        public bool IsCompletionVisible { get; private set; }
        public bool IsContractVisible { get; private set; }
        public bool WantsPointer=>IsCompletionVisible||IsContractVisible;
        private GameObject overlay, page;
        private Image flash;
        private Text prompt;
        private GameObject CanvasRoot()
        {
            if (overlay) return overlay;
            overlay = new GameObject("PrologFinaleCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var c = overlay.GetComponent<Canvas>(); c.renderMode = RenderMode.ScreenSpaceOverlay; c.sortingOrder = 120;
            var s = overlay.GetComponent<CanvasScaler>(); s.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; s.referenceResolution = new Vector2(1600, 900);
            return overlay;
        }
        public void ResetView()
        {
            StopAllCoroutines(); ContractRead = IsCompletionVisible = false;
            IsContractVisible=false;
            if (overlay) { overlay.SetActive(false); Destroy(overlay); overlay = null; }
            prompt=null;flash=null;page=null;
        }
        public void Prompt(string text)
        {
            CanvasRoot();
            if (!prompt)
            {
                prompt = UiWidgets.Text("EscapePrompt", overlay.transform, "", new Vector2(.12f, .30f), new Vector2(.88f, .48f), 35, TextAnchor.MiddleCenter);
                var shadow = prompt.gameObject.AddComponent<Shadow>(); shadow.effectColor = Color.black; shadow.effectDistance = new Vector2(2, -2);
            }
            prompt.text = text ?? "";
        }
        public IEnumerator Flash()
        {
            CanvasRoot();
            if (!flash) { flash = UiWidgets.Panel("BlitzFlash", overlay.transform, Vector2.zero, Vector2.one, Color.white).GetComponent<Image>(); flash.raycastTarget = false; }
            float duration = GameShell.ReducedGlitch ? .22f : .48f;
            for (float t = 0; t < duration; t += Time.deltaTime) { flash.color = new Color(1, 1, 1, Mathf.Lerp(GameShell.ReducedGlitch ? .4f : 1, 0, t / duration)); yield return null; }
            flash.color = Color.clear;
        }
        public IEnumerator FlashBeam()
        {
            CanvasRoot();
            if (!flash) { flash = UiWidgets.Panel("BlitzFlash", overlay.transform, Vector2.zero, Vector2.one, Color.white).GetComponent<Image>(); flash.raycastTarget = false; }
            float duration = GameShell.ReducedGlitch ? .75f : 1.35f;
            for (float t = 0; t < duration; t += Time.unscaledDeltaTime)
            {
                float remaining = 1f - t / duration;
                float flicker = .9f + .1f * Mathf.Sin(Time.unscaledTime * 74f);
                flash.color = new Color(1f, .98f, .92f, Mathf.Clamp01(remaining * 1.7f) * flicker);
                yield return null;
            }
            flash.color = Color.clear;
        }
        public void ShowContract(Transform handset)
        {
            ContractRead = false;
            IsContractVisible=true;
            if (page) Destroy(page);
            // The contract appears on Raka's single handset, with a scrollable message.
            page = UiWidgets.Panel("FinalTransmission", handset, new Vector2(.04f, .085f), new Vector2(.96f, .915f), new Color(.035f, .006f, .009f));
            UiWidgets.Text("Sender", page.transform, "PENONTON_TERAKHIR", new Vector2(.06f, .83f), new Vector2(.94f, .97f), 23).color = new Color(.92f, .25f, .20f);
            UiWidgets.ScrollText("FinalContractBody", page.transform, FinalContract, new Vector2(.07f, .18f), new Vector2(.93f, .82f), 23);
            UiWidgets.Button("AcknowledgeFinalContract", page.transform, "SIMPAN PESAN", new Vector2(.07f, .025f), new Vector2(.93f, .145f), () => ContractRead = true);
        }
        public void CloseContract() { IsContractVisible=false;if (page) { page.SetActive(false); Destroy(page); } }
        public void ShowTitle(bool saved)
        {
            CanvasRoot(); CloseContract(); IsCompletionVisible = true; Prompt(null);
            if (page) Destroy(page);
            page = UiWidgets.Panel("PrologComplete", overlay.transform, Vector2.zero, Vector2.one, new Color(.006f, .007f, .009f));
            UiWidgets.Text("PrologEyebrow", page.transform, "MALAM PERTAMA DI RUMAH ATMAJA", new Vector2(.12f, .68f), new Vector2(.88f, .74f), 23, TextAnchor.MiddleCenter).color = new Color(.52f, .49f, .44f);
            UiWidgets.Text("TitleDrop", page.transform, "LAYAR TERAKHIR", new Vector2(.05f, .47f), new Vector2(.95f, .66f), 104, TextAnchor.MiddleCenter).color = new Color(.64f, .045f, .035f);
            UiWidgets.Text("PrologStatus", page.transform, saved ? "PROLOG SELESAI" : "PENYIMPANAN BELUM BERHASIL", new Vector2(.15f, .38f), new Vector2(.85f, .44f), 25, TextAnchor.MiddleCenter);
            UiWidgets.Text("NextNight", page.transform, "Damar masih di balik cermin.\nPagi berikutnya: kos — hirup udara pagi sebelum malam Rusunawa", new Vector2(.15f, .24f), new Vector2(.85f, .35f), 26, TextAnchor.MiddleCenter);
            UiWidgets.Text("StreamTotals", page.transform, "54.820 PENONTON   /   TOTAL SAWERAN Rp25.450.000", new Vector2(.1f, .76f), new Vector2(.9f, .82f), 19, TextAnchor.MiddleCenter).color = new Color(.44f, .45f, .44f);
            UiWidgets.Button("PrologMainMenu", page.transform, "MENU UTAMA", new Vector2(.37f, .12f), new Vector2(.63f, .195f), () => GameShell.Instance.OpenMain()).interactable = saved;
            if(saved&&UnityEngine.Application.CanStreamedLevelBeLoaded("Prolog_KamarKos"))
                UiWidgets.Button("ContinuePhase1",page.transform,"PAGI BERIKUTNYA — KEMBALI KE KOS",new Vector2(.25f,.025f),new Vector2(.75f,.10f),()=>
                {
                    if(!LayarTerakhir.Interlude.InterludeCampaign.EnterInterlude(LayarTerakhir.Core.CampaignState.Instance))return;
                    Time.timeScale=1;AudioListener.pause=false;LayarTerakhir.Core.UiFocus.Cinematic=LayarTerakhir.Core.UiFocus.Phone=false;
                    UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Prolog_KamarKos");
                });
            if (!saved) UiWidgets.Button("RetryFinalSave", page.transform, "COBA SIMPAN LAGI", new Vector2(.37f, .025f), new Vector2(.63f, .10f), () => {
                var campaign = CampaignState.Instance; campaign.AddInventoryItem("PROLOG_COMPLETE");
                bool success = campaign.SaveCheckpointAtomic("PROLOG_COMPLETE");
                if (!success) campaign.Data.inventory.Remove("PROLOG_COMPLETE"); ShowTitle(success);
            });
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
        }
        private void OnDestroy() { if (overlay) Destroy(overlay); }
    }
}
