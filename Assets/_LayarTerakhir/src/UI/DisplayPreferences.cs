using System;
using UnityEngine;

namespace LayarTerakhir.UI
{
    // Borderless mode avoids unsupported exclusive-display modes on laptops.
    // The game owns Alt+Enter; automatic native switching is disabled in the build.
    public static class DisplayPreferences
    {
        private const string ModeKey="LT.Display.Fullscreen";
        private static int windowWidth=1280,windowHeight=720;
        private static int lastToggleFrame=-1;
        private static bool Isolated=>!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("LAYAR_TERAKHIR_TEST_SAVE_DIRECTORY"));
        public static bool ShouldRestore(bool editor,string[] args,bool isolated)
        {
            if(editor||isolated)return false;
            foreach(string arg in args)
                if(arg.EndsWith("Audit",StringComparison.OrdinalIgnoreCase)||arg.Equals("-batchmode",StringComparison.OrdinalIgnoreCase)||
                    arg.StartsWith("-screen-",StringComparison.OrdinalIgnoreCase)||arg.Equals("-nographics",StringComparison.OrdinalIgnoreCase))return false;
            return true;
        }
        public static void Restore()
        {
            if(!ShouldRestore(Application.isEditor,Environment.GetCommandLineArgs(),Isolated))return;
            windowWidth=PlayerPrefs.GetInt("LT.Display.WindowWidth",1280);windowHeight=PlayerPrefs.GetInt("LT.Display.WindowHeight",720);
            Apply(PlayerPrefs.GetInt(ModeKey,1)==1,false);
        }
        public static void Toggle()
        {
            // Enter can also submit the focused UI button in the same input frame.
            if(lastToggleFrame==Time.frameCount)return;lastToggleFrame=Time.frameCount;
            if(!Screen.fullScreen){windowWidth=Screen.width;windowHeight=Screen.height;}
            Apply(!Screen.fullScreen,true);
        }
        private static void Apply(bool fullscreen,bool persist)
        {
            int maxWidth=Screen.currentResolution.width,maxHeight=Screen.currentResolution.height;
            if(maxWidth<=0)maxWidth=1920;if(maxHeight<=0)maxHeight=1080;
            int width=fullscreen?maxWidth:Mathf.Clamp(windowWidth,Mathf.Min(640,maxWidth),maxWidth);
            int height=fullscreen?maxHeight:Mathf.Clamp(windowHeight,Mathf.Min(360,maxHeight),maxHeight);
            Screen.SetResolution(width,height,fullscreen?FullScreenMode.FullScreenWindow:FullScreenMode.Windowed);
            if(persist&&!Isolated&&!Application.isEditor)
            {
                PlayerPrefs.SetInt(ModeKey,fullscreen?1:0);PlayerPrefs.SetInt("LT.Display.WindowWidth",windowWidth);
                PlayerPrefs.SetInt("LT.Display.WindowHeight",windowHeight);PlayerPrefs.Save();
            }
        }
    }
}
