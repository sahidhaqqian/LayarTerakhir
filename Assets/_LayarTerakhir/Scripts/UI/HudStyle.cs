using UnityEngine;

namespace LayarTerakhir.UI
{
    [CreateAssetMenu(menuName="LayarTerakhir/HUD style")]
    public sealed class HudStyle : ScriptableObject
    {
        public Font bodyFont, emphasisFont;
        public Vector2 referenceResolution=new Vector2(1600,900);
        public Vector2 safeMargins=new Vector2(64,44);
        public Color text=new Color(.91f,.89f,.81f), muted=new Color(.68f,.68f,.61f);
        public Color accent=new Color(.61f,.47f,.28f), danger=new Color(.74f,.28f,.22f);
        public Color panel=new Color(.045f,.052f,.048f,.93f);
        public Color paper=new Color(.77f,.74f,.64f,.98f), ink=new Color(.13f,.14f,.12f);
        public int bodySize=26, subtitleSize=28, titleSize=32;
        public float fadeDuration=.18f;
    }
}
