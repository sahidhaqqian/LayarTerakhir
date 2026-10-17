using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace LayarTerakhir.UI
{
    public static class UiWidgets
    {
        public static Font Font;
        public static GameObject Rect(string name,Transform parent,Vector2 min,Vector2 max)
        {
            var go=new GameObject(name,typeof(RectTransform));var rt=go.GetComponent<RectTransform>();rt.SetParent(parent,false);rt.anchorMin=min;rt.anchorMax=max;rt.offsetMin=rt.offsetMax=Vector2.zero;return go;
        }
        public static GameObject Panel(string name,Transform parent,Vector2 min,Vector2 max,Color color)
        {var go=Rect(name,parent,min,max);go.AddComponent<Image>().color=color;return go;}
        public static Text Text(string name,Transform parent,string words,Vector2 min,Vector2 max,int size=24,TextAnchor anchor=TextAnchor.MiddleLeft)
        {
            var go=Rect(name,parent,min,max);var t=go.AddComponent<Text>();t.font=Font?Font:Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");t.fontSize=size;t.text=words;t.color=new Color(.88f,.86f,.80f);t.alignment=anchor;t.raycastTarget=false;t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Truncate;t.resizeTextForBestFit=true;t.resizeTextMinSize=12;t.resizeTextMaxSize=size;return t;
        }
        public static Button Button(string name,Transform parent,string words,Vector2 min,Vector2 max,UnityAction action)
        {
            var go=Panel(name,parent,min,max,new Color(.105f,.12f,.12f,.97f));var b=go.AddComponent<Button>();b.targetGraphic=go.GetComponent<Image>();var colors=b.colors;colors.highlightedColor=new Color(.68f,.45f,.35f);colors.pressedColor=new Color(.45f,.24f,.18f);b.colors=colors;
            Text("Label",go.transform,words,new Vector2(.04f,0),new Vector2(.96f,1),24,TextAnchor.MiddleCenter);b.onClick.AddListener(action);return b;
        }
        public static Text ScrollText(string name,Transform parent,string words,Vector2 min,Vector2 max,int size=21)
        {
            var host=Panel(name+"Scroll",parent,min,max,new Color(0,0,0,.01f));var scroll=host.AddComponent<ScrollRect>();scroll.horizontal=false;scroll.vertical=true;scroll.scrollSensitivity=30;scroll.movementType=ScrollRect.MovementType.Clamped;
            var viewport=Rect("Viewport",host.transform,Vector2.zero,Vector2.one);viewport.AddComponent<RectMask2D>();
            var label=Text(name,viewport.transform,words,new Vector2(0,1),Vector2.one,size,TextAnchor.UpperLeft);var content=label.rectTransform;content.pivot=new Vector2(.5f,1);
            label.resizeTextForBestFit=false;
            var fit=label.gameObject.AddComponent<ContentSizeFitter>();fit.verticalFit=ContentSizeFitter.FitMode.PreferredSize;
            scroll.viewport=viewport.GetComponent<RectTransform>();scroll.content=content;return label;
        }
        public static Slider Slider(string name,Transform parent,string words,Vector2 min,Vector2 max,float low,float high,float value,UnityAction<float> changed)
        {
            var go=Rect(name,parent,min,max);Text("Label",go.transform,words,Vector2.zero,new Vector2(.43f,1),23);
            var track=Panel("Track",go.transform,new Vector2(.46f,.4f),new Vector2(.97f,.6f),new Color(.2f,.22f,.21f));
            var fill=Panel("Fill",track.transform,Vector2.zero,Vector2.one,new Color(.57f,.31f,.24f));
            var handleArea=Rect("HandleArea",go.transform,new Vector2(.46f,.15f),new Vector2(.97f,.85f));
            var handle=Panel("Handle",handleArea.transform,Vector2.zero,Vector2.one,new Color(.82f,.73f,.58f));handle.GetComponent<RectTransform>().sizeDelta=new Vector2(14,0);
            var s=go.AddComponent<Slider>();s.fillRect=fill.GetComponent<RectTransform>();s.handleRect=handle.GetComponent<RectTransform>();s.targetGraphic=handle.GetComponent<Image>();s.minValue=low;s.maxValue=high;s.value=value;s.onValueChanged.AddListener(changed);return s;
        }
        public static void Line(string name,Transform parent,Vector2 a,Vector2 b,Color color,float width=3)
        {
            var go=Panel(name,parent,Vector2.zero,Vector2.zero,color);var rt=go.GetComponent<RectTransform>();rt.anchorMin=rt.anchorMax=new Vector2(.5f,.5f);rt.anchoredPosition=(a+b)*.5f;rt.sizeDelta=new Vector2(Vector2.Distance(a,b),width);rt.localRotation=Quaternion.Euler(0,0,Mathf.Atan2(b.y-a.y,b.x-a.x)*Mathf.Rad2Deg);
        }
    }
}
