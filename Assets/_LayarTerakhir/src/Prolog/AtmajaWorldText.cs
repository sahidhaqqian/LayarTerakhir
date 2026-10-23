using System.Collections.Generic;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    // TextMesh's GUI font shader draws mirrored text through walls. Keep ordinary
    // plaques depth-tested and one-sided, while tracking Unity's dynamic font atlas.
    public sealed class AtmajaWorldText : MonoBehaviour
    {
        public Material materialTemplate;
        public bool includeARText;
        private readonly Dictionary<Font, Material> materials = new Dictionary<Font, Material>();
        private void OnEnable() { Font.textureRebuilt += Rebuilt; }
        private void OnDisable() { Font.textureRebuilt -= Rebuilt; }
        private void Start()
        {
            if (!materialTemplate) return;
            foreach (var text in FindObjectsByType<TextMesh>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (!text.font || !includeARText&&text.gameObject.layer == AtmajaPhoneView.ARLayer) continue;
                text.font.RequestCharactersInTexture(text.text, text.fontSize, text.fontStyle);
                if (!materials.TryGetValue(text.font, out var material))
                {
                    material = new Material(materialTemplate); material.SetTexture("_MainTex", text.font.material.mainTexture);
                    materials.Add(text.font, material);
                }
                text.GetComponent<Renderer>().sharedMaterial = material;
            }
        }
        private void Rebuilt(Font font)
        { if (materials.TryGetValue(font, out var material)) material.SetTexture("_MainTex", font.material.mainTexture); }
        private void OnDestroy() { foreach (var material in materials.Values) if (material) Destroy(material); }
    }
}
