using System;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LayarTerakhir.UI
{
    public sealed class GameShell:MonoBehaviour
    {
        public static GameShell Instance {get;private set;}
        public static bool AwaitingStart=>Instance&&Instance.awaiting;
        public static bool MenuSceneLoading=>Instance&&Instance.loading;
        public static bool IsPaused=>UiFocus.Menu;
        public static bool Subtitles=true,ReducedGlitch=false;
        private GameObject canvas,page;private bool awaiting,settings,confirmNew,loading;private float previousScale=1;
        private GameAction? remapping;private Text remapLabel;private string error="";
        public const string KosScene="Prolog_KamarKos";
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            var go=new GameObject("GameShell");DontDestroyOnLoad(go);go.AddComponent<GameShell>();
        }
        private void Awake()
        {
            if(Instance){Destroy(gameObject);return;}Instance=this;
            DisplayPreferences.Restore();
            var args=Environment.GetCommandLineArgs();bool audit=Array.Exists(args,a=>a!="-reviewAudit"&&a.EndsWith("Audit",StringComparison.Ordinal));
            awaiting=!Application.isEditor&&!audit;
            var profile=PresentationProfile.Load();UiWidgets.Font=profile?profile.bodyFont:null;
            canvas=new GameObject("GameMenuCanvas",typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));canvas.transform.SetParent(transform,false);
            var c=canvas.GetComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;c.sortingOrder=4000;
            var scaler=canvas.GetComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1600,900);
            SceneManager.sceneLoaded+=SceneReady;
            if(string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")))
            {AudioListener.volume=PlayerPrefs.GetFloat("LT.Volume",.85f);Subtitles=PlayerPrefs.GetInt("LT.Subtitles",1)==1;ReducedGlitch=PlayerPrefs.GetInt("LT.ReducedGlitch",0)==1;}
            if(awaiting)OpenMain();else canvas.SetActive(false);
        }
        private void SceneReady(Scene scene,LoadSceneMode mode)
        {
            UiFocus.Clock=UiFocus.Phone=UiFocus.Cinematic=false;
            var player=FindFirstObjectByType<FirstPersonController>();var hud=HudController.Instance;
            if(player&&hud&&!FindFirstObjectByType<AtmajaPhoneView>())
            {
                var view=hud.gameObject.AddComponent<AtmajaPhoneView>();view.playerCamera=player.GetComponent<InteractionInteractor>().playerCamera;
                var profile=PresentationProfile.Load();if(profile)view.feedMaterial=profile.phoneFeedMaterial;
            }
            if(GameInputConfig.Instance&&string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")))
            {
                var saved=PlayerPrefs.GetString("LT.Bindings","");if(!string.IsNullOrEmpty(saved))JsonUtility.FromJsonOverwrite(saved,GameInputConfig.Instance);
                GameInputConfig.Instance.UpgradePerspectiveBinding();
                GameInputConfig.Instance.mouseSensitivity=PlayerPrefs.GetFloat("LT.Sensitivity",2);
            }
            if(awaiting)Draw();
        }
        private void Update()
        {
            var keys=Keyboard.current;
            if(!remapping.HasValue&&keys!=null&&(keys.leftAltKey.isPressed||keys.rightAltKey.isPressed)&&keys.enterKey.wasPressedThisFrame)
            {DisplayPreferences.Toggle();if(settings)StartCoroutine(RefreshDisplayLabel());return;}
            if(remapping.HasValue&&Keyboard.current!=null)
            {
                foreach(var key in Keyboard.current.allKeys)
                {
                    if(!key.wasPressedThisFrame||key.keyCode==Key.Escape)continue;
                    string name=key.keyCode==Key.LeftCtrl?"LeftControl":key.keyCode==Key.RightCtrl?"RightControl":key.keyCode.ToString();
                    if(Enum.TryParse(name,true,out KeyCode code))
                    {GameInputConfig.Instance.RemapAction(remapping.Value,code);SaveSettings();remapping=null;Draw();break;}
                }
            }
            if(Keyboard.current!=null&&Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if(remapping.HasValue){remapping=null;Draw();return;}
                if(settings){settings=false;Draw();return;}
                if(UiFocus.Clock){var hub=FindFirstObjectByType<LayarTerakhir.Phase2.Phase2Hub>();if(hub){hub.Close();return;}var p2=FindFirstObjectByType<LayarTerakhir.Phase2.Phase2Director>();if(p2){p2.CloseOverlay();return;}var shop=FindFirstObjectByType<MelatiShop>();if(shop&&shop.IsOpen)shop.Close();else FindFirstObjectByType<ClockPuzzleView>()?.Close();return;}
                var hud=HudController.Instance;
                if(hud&&hud.documentPanel.activeSelf){hud.CloseDocumentModal();return;}
                if(hud&&hud.padlockPanel.activeSelf){hud.ClosePadlockModal();return;}
                if(!awaiting){if(UiFocus.Menu)Resume();else Pause();}
            }
        }
        public void OpenMain()
        {awaiting=true;settings=false;confirmNew=false;Open();}
        public void ReturnAfterDeath(string reason)
        {
            error=reason;UiFocus.Cinematic=UiFocus.Phone=UiFocus.Clock=false;
            OpenMain();
        }
        public void Pause(){if(awaiting)return;settings=false;Open();}
        private void OnApplicationFocus(bool focused)
        {
            if (!focused && !awaiting && !loading && !UiFocus.Menu &&
                string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY"))) Pause();
        }
        private void Open()
        {
            if(!UiFocus.Menu)previousScale=Time.timeScale;
            UiFocus.Menu=true;Time.timeScale=0;AudioListener.pause=true;Cursor.lockState=CursorLockMode.None;Cursor.visible=true;canvas.SetActive(true);Draw();
        }
        public void Resume()
        {if(awaiting)return;UiFocus.Menu=false;settings=false;canvas.SetActive(false);Time.timeScale=previousScale>0?previousScale:1;AudioListener.pause=false;var finale=FindFirstObjectByType<PrologFinaleView>();bool pointer=UiFocus.Phone||(finale&&finale.WantsPointer);Cursor.lockState=pointer?CursorLockMode.None:CursorLockMode.Locked;Cursor.visible=pointer;}
        private void Draw()
        {
            if(page)Destroy(page);
            page=UiWidgets.Panel("MenuPage",canvas.transform,Vector2.zero,Vector2.one,new Color(.012f,.019f,.021f,.96f));
            UiWidgets.Text("Eyebrow",page.transform,"MALAM KERAMAT  /  CERITA RAKA WIJAYA",new Vector2(.08f,.82f),new Vector2(.91f,.90f),19);
            UiWidgets.Text("Title",page.transform,settings?"PENGATURAN":awaiting?"LAYAR TERAKHIR":"JEDA",new Vector2(.075f,.67f),new Vector2(.94f,.83f),64);
            UiWidgets.Text("Status",page.transform,error,new Vector2(.08f,.08f),new Vector2(.92f,.15f),22);
            if(settings){DrawSettings();return;}
            if(confirmNew)
            {
                UiWidgets.Text("Confirm",page.transform,"Mulai cerita dari bangun di kos?\nSave sebelumnya akan diarsipkan, bukan dihapus.",new Vector2(.08f,.42f),new Vector2(.8f,.62f),28);
                MenuButton("ConfirmNew","MULAI CERITA BARU",.32f,StartNew);MenuButton("CancelNew","KEMBALI",.21f,()=>{confirmNew=false;Draw();});return;
            }
            if(awaiting)
            {
                MenuButton("NewGame","NEW GAME",.52f,()=>{confirmNew=true;Draw();});
                var b=MenuButton("Continue","CONTINUE • CHECKPOINT TERAKHIR",.41f,Continue);
                b.interactable=CampaignState.Instance&&CampaignState.Instance.HasValidCheckpoint;
                MenuButton("Settings","SETTINGS",.30f,()=>{settings=true;Draw();});MenuButton("Exit","EXIT",.19f,Exit);
            }
            else
            {
                MenuButton("Resume","LANJUTKAN",.52f,Resume);MenuButton("Settings","SETTINGS",.41f,()=>{settings=true;Draw();});
                MenuButton("MainMenu","MENU UTAMA",.30f,()=>{OpenMain();StartCoroutine(LoadKos(true));});MenuButton("Exit","EXIT",.19f,Exit);
                UiWidgets.Text("CheckpointHint",page.transform,"Continue melanjutkan checkpoint tersimpan, bukan posisi kamera saat ini.",new Vector2(.4f,.18f),new Vector2(.9f,.36f),21);
            }
        }
        private Button MenuButton(string name,string title,float y,UnityEngine.Events.UnityAction callback)=>UiWidgets.Button(name,page.transform,title,new Vector2(.08f,y),new Vector2(.38f,y+.075f),callback);
        public void StartNew()
        {
            if(loading)return;
            if(!CampaignState.Instance||!CampaignState.Instance.BeginNewCampaignPreservingPrevious()){error="Save tidak dapat diarsipkan. Cerita lama tidak ditimpa.";Draw();return;}
            error="";loading=true;awaiting=false;confirmNew=false;Resume();StartCoroutine(LoadKos(false));
        }
        public void Continue()
        {
            if(loading)return;
            if(!CampaignState.Instance||!CampaignState.Instance.LoadCheckpoint()){error="Checkpoint tidak tersedia atau rusak.";Draw();return;}
            string scene=CampaignRouting.ContinueScene(CampaignState.Instance.Data);
            if(string.IsNullOrEmpty(scene)||!Application.CanStreamedLevelBeLoaded(scene)){error="Babak checkpoint ini belum tersedia. Save tetap disimpan.";Draw();return;}
            error="";loading=true;awaiting=false;Resume();StartCoroutine(LoadSavedScene(scene));
        }
        private IEnumerator LoadSavedScene(string scene)
        {var op=SceneManager.LoadSceneAsync(scene);while(!op.isDone)yield return null;loading=false;}
        private IEnumerator LoadKos(bool keepMenu)
        {loading=true;var op=SceneManager.LoadSceneAsync(KosScene);while(!op.isDone)yield return null;loading=false;if(keepMenu)OpenMain();}
        public void ShowSettings(){settings=true;Open();}
        private void DrawSettings()
        {
            UiWidgets.Slider("MasterVolume",page.transform,"Volume",new Vector2(.08f,.57f),new Vector2(.56f,.64f),0,1,AudioListener.volume,v=>{AudioListener.volume=v;SaveSettings();});
            UiWidgets.Slider("Sensitivity",page.transform,"Sensitivitas mouse",new Vector2(.08f,.48f),new Vector2(.56f,.55f),.25f,5,GameInputConfig.Instance?GameInputConfig.Instance.mouseSensitivity:2,v=>{if(GameInputConfig.Instance)GameInputConfig.Instance.mouseSensitivity=v;SaveSettings();});
            UiWidgets.Button("Subtitles",page.transform,"SUBTITLE: "+(Subtitles?"AKTIF":"MATI"),new Vector2(.08f,.38f),new Vector2(.30f,.44f),()=>{Subtitles=!Subtitles;SaveSettings();Draw();});
            UiWidgets.Button("Glitch",page.transform,"GLITCH: "+(ReducedGlitch?"REDUKSI":"NORMAL"),new Vector2(.32f,.38f),new Vector2(.56f,.44f),()=>{ReducedGlitch=!ReducedGlitch;SaveSettings();Draw();});
            UiWidgets.Button("DisplayMode",page.transform,"LAYAR: "+(Screen.fullScreen?"FULLSCREEN (BORDERLESS)":"WINDOWED")+"  ↔",new Vector2(.08f,.29f),new Vector2(.56f,.35f),()=>{DisplayPreferences.Toggle();StartCoroutine(RefreshDisplayLabel());});
            var rig=FindFirstObjectByType<ShoulderCameraRig>();var perspective=rig?rig.SelectedPerspective:ShoulderCameraRig.SavedPerspective;
            UiWidgets.Button("PerspectiveMode",page.transform,"KAMERA: "+(perspective==PlayerPerspective.OverShoulder?"ORANG KETIGA • BAHU":"ORANG PERTAMA"),new Vector2(.08f,.205f),new Vector2(.56f,.265f),()=>{if(rig)rig.TogglePerspective();Draw();});
            var actions=new[]{GameAction.Interact,GameAction.Crouch,GameAction.Vault,GameAction.TogglePhone,GameAction.ToggleScreen,GameAction.ModeAR,GameAction.UsePowerbank,GameAction.Sprint,GameAction.TogglePerspective};
            for(int i=0;i<actions.Length;i++)
            {
                var action=actions[i];float y=.59f-i*.052f;
                UiWidgets.Button("Remap_"+action,page.transform,action+"   ["+(GameInputConfig.Instance?GameInputConfig.Instance.GetKeyForAction(action).ToString():"-")+"]",new Vector2(.63f,y),new Vector2(.92f,y+.043f),()=>{remapping=action;if(remapLabel)remapLabel.text="Tekan tombol baru untuk "+action+" • ESC batal";});
            }
            string cameraKey=GameInputConfig.Instance?GameInputConfig.Instance.GetActionLabel(GameAction.TogglePerspective):"V";
            remapLabel=UiWidgets.Text("RemapHint",page.transform,cameraKey+" ganti sudut pandang • Alt+Enter fullscreen",new Vector2(.08f,.03f),new Vector2(.58f,.08f),20);
            MenuButton("BackSettings","KEMBALI",.105f,()=>{settings=false;Draw();});
        }
        private IEnumerator RefreshDisplayLabel(){yield return null;yield return null;if(settings)Draw();}
        private static void SaveSettings()
        {
            if(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY")))return;
            PlayerPrefs.SetFloat("LT.Volume",AudioListener.volume);PlayerPrefs.SetFloat("LT.Sensitivity",GameInputConfig.Instance?GameInputConfig.Instance.mouseSensitivity:2);
            PlayerPrefs.SetInt("LT.Subtitles",Subtitles?1:0);PlayerPrefs.SetInt("LT.ReducedGlitch",ReducedGlitch?1:0);
            if(GameInputConfig.Instance)PlayerPrefs.SetString("LT.Bindings",JsonUtility.ToJson(GameInputConfig.Instance));PlayerPrefs.Save();
        }
        private static void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying=false;
#else
            Application.Quit();
#endif
        }
        private void OnDestroy(){SceneManager.sceneLoaded-=SceneReady;if(Instance==this){Instance=null;UiFocus.Menu=false;AudioListener.pause=false;}}
    }
}
