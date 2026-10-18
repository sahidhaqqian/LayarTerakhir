#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LayarTerakhir.Core
{
    // Development-only playtest evidence. F12 captures the real final frame, including HUD/post processing.
    public sealed class DevelopmentScreenshotCapture : MonoBehaviour
    {
        private bool busy;
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
            if(FindFirstObjectByType<DevelopmentScreenshotCapture>())return;
            var go=new GameObject("DevelopmentScreenshotCapture");DontDestroyOnLoad(go);go.AddComponent<DevelopmentScreenshotCapture>();
        }
        private void Start()
        {
            if(Array.IndexOf(Environment.GetCommandLineArgs(),"-kosCapture")>=0)StartCoroutine(StartupCapture());
        }
        private IEnumerator StartupCapture(){yield return new WaitForSecondsRealtime(8);yield return Capture();}
        private void Update()
        {
            if(!busy&&Keyboard.current!=null&&Keyboard.current.f12Key.wasPressedThisFrame)StartCoroutine(Capture());
        }
        private IEnumerator Capture()
        {
            busy=true;
            yield return new WaitForEndOfFrame();
            string directory=Path.Combine(Application.persistentDataPath,"Screenshots");Directory.CreateDirectory(directory);
            string path=Path.Combine(directory,"Kos-"+DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff")+".png");
            ScreenCapture.CaptureScreenshot(path);
            Debug.Log("KOS_SCREENSHOT "+path+" | "+SystemInfo.graphicsDeviceName+" | "+Screen.width+"x"+Screen.height);
            busy=false;
        }
    }
}
#endif
