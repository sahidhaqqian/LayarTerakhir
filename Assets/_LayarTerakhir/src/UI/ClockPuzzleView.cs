using System;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using UnityEngine;
using UnityEngine.UI;
namespace LayarTerakhir.UI
{
    public sealed class ClockPuzzleView:MonoBehaviour
    {
        public int Hour {get;private set;}=12;public int Minute {get;private set;}
        private GameObject panel;private Text readout,feedback;private Action solved;
        public void Open(Action onSolved)
        {
            solved=onSolved;Hour=12;Minute=0;UiFocus.Clock=true;
            var canvas=HudController.Instance.GetComponentInChildren<Canvas>().transform;
            panel=UiWidgets.Panel("ClockTimePanel",canvas,new Vector2(.22f,.18f),new Vector2(.78f,.82f),new Color(.035f,.03f,.025f,.99f));panel.transform.SetAsLastSibling();
            UiWidgets.Text("ClockTitle",panel.transform,"JAM BANDUL / PASANG JARUM",new Vector2(.06f,.80f),new Vector2(.94f,.96f),30,TextAnchor.MiddleCenter);
            readout=UiWidgets.Text("ClockReadout",panel.transform,"12 : 00",new Vector2(.15f,.47f),new Vector2(.85f,.75f),64,TextAnchor.MiddleCenter);
            UiWidgets.Button("HourMinus",panel.transform,"− JAM",new Vector2(.08f,.32f),new Vector2(.29f,.45f),()=>AdjustHour(-1));
            UiWidgets.Button("HourPlus",panel.transform,"JAM +",new Vector2(.31f,.32f),new Vector2(.49f,.45f),()=>AdjustHour(1));
            UiWidgets.Button("MinuteMinus",panel.transform,"− MENIT",new Vector2(.51f,.32f),new Vector2(.71f,.45f),()=>AdjustMinute(-5));
            UiWidgets.Button("MinutePlus",panel.transform,"MENIT +",new Vector2(.73f,.32f),new Vector2(.93f,.45f),()=>AdjustMinute(5));
            feedback=UiWidgets.Text("ClockFeedback",panel.transform,"Cocokkan waktu dengan petunjuk di rumah.",new Vector2(.06f,.19f),new Vector2(.94f,.30f),22,TextAnchor.MiddleCenter);
            UiWidgets.Button("SetClockTime",panel.transform,"PASANG & ATUR",new Vector2(.10f,.04f),new Vector2(.55f,.16f),Confirm);
            UiWidgets.Button("CancelClockTime",panel.transform,"BATAL",new Vector2(.61f,.04f),new Vector2(.90f,.16f),Close);
            FindFirstObjectByType<FirstPersonController>()?.SetCursorLock(false);
        }
        public void AdjustHour(int step){Hour=(Hour-1+step+12)%12+1;Refresh();}
        public void AdjustMinute(int step){Minute=(Minute+step+60)%60;Refresh();}
        private void Refresh(){if(readout)readout.text=$"{Hour:00} : {Minute:00}";}
        public void Confirm()
        {
            if(Hour!=3||Minute!=45){feedback.text="Mekanisme belum bergerak. Waktu ini bukan kuncinya.";return;}
            var callback=solved;Close();callback?.Invoke();
        }
        public void Close(){UiFocus.Clock=false;if(panel)Destroy(panel);solved=null;FindFirstObjectByType<FirstPersonController>()?.SetCursorLock(true);}
        private void OnDestroy(){UiFocus.Clock=false;if(panel)Destroy(panel);}
    }
}
