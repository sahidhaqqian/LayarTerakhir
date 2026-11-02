using System.Collections.Generic;
using LayarTerakhir.Core;
using UnityEngine;

namespace LayarTerakhir.Player
{
    // Designer Nov: a small FAINT blink directly ON every must-take item —
    // not a star, not floating above it. The item itself gently breathes
    // brighter (~1.2 Hz sine, low amplitude) via per-renderer property
    // blocks, so shared materials are never touched. Hides itself once the
    // item is taken: parent deactivated, IInteractable no longer
    // interactable, or the completedFlag present in the campaign.
    public sealed class PickupBlink : MonoBehaviour
    {
        public string completedFlag;
        public System.Func<bool> availableWhen;
        private IInteractable target;
        private readonly List<Renderer> renderers = new List<Renderer>();
        private readonly List<Color> baseColors = new List<Color>();
        private readonly List<string> baseProps = new List<string>();
        private float seed;
        private bool pulsed;

        // Idempotent: safe to call from builders, repairs and directors alike.
        // availableWhen gates story-locked takes (jacket keys before the
        // preparation stage, handycam before the powerbank): the blink only
        // lights once the thing can actually be taken.
        public static PickupBlink EnsureFor(GameObject item, string completedFlag = null, System.Func<bool> availableWhen = null)
        {
            if (!item) return null;
            // Prefer the blink on the item itself; a floating star child
            // from the previous design is removed, never reused.
            foreach (var legacy in item.GetComponentsInChildren<PickupBlink>(true))
                if (legacy && legacy.gameObject != item) Object.DestroyImmediate(legacy.gameObject);
            var existing = item.GetComponent<PickupBlink>();
            if (existing)
            {
                if (!string.IsNullOrEmpty(completedFlag)) existing.completedFlag = completedFlag;
                if (availableWhen != null) existing.availableWhen = availableWhen;
                existing.target = item.GetComponentInParent<IInteractable>() ?? item.GetComponent<IInteractable>();
                existing.Collect();
                existing.ClearPulse();
                return existing;
            }
            var blink = item.AddComponent<PickupBlink>();
            blink.completedFlag = completedFlag;
            blink.availableWhen = availableWhen;
            blink.target = item.GetComponentInParent<IInteractable>() ?? item.GetComponent<IInteractable>();
            blink.seed = Random.Range(0f, 10f);
            blink.Collect();
            return blink;
        }

        private void Awake()
        {
            seed = Random.Range(0f, 10f);
            Collect();
        }

        private void Collect()
        {
            renderers.Clear(); baseColors.Clear(); baseProps.Clear();
            foreach (var r in GetComponentsInChildren<Renderer>(true))
            {
                if (!r || r is ParticleSystemRenderer) continue;
                var mat = r.sharedMaterial;
                if (!mat) continue;
                string prop = null;
                if (mat.HasProperty("_BaseColor")) prop = "_BaseColor";
                else if (mat.HasProperty("_Color")) prop = "_Color";
                if (prop == null) continue;
                renderers.Add(r); baseColors.Add(mat.GetColor(prop)); baseProps.Add(prop);
                // Old floating-star rigs (Nov batch 1) must not linger next
                // to the new on-item pulse.
                if (r.gameObject != gameObject && r.GetComponent<PickupBlink>() == null && r.gameObject.name == "PickupBlink")
                    Destroy(r.gameObject);
            }
            // Remove a legacy star child left by the previous design.
            foreach (Transform c in transform)
                if (c.name == "PickupBlink" && c.GetComponent<PickupBlink>() == null) Destroy(c.gameObject);
        }

        private bool Taken()
        {
            if (!string.IsNullOrEmpty(completedFlag))
            {
                var campaign = CampaignState.Instance;
                if (campaign && campaign.HasInventoryItem(completedFlag)) return true;
            }
            return false;
        }

        // k in 0..1: 0 = item's normal look, 1 = peak of the faint blink.
        public void ApplyPulse(float k)
        {
            var block = new MaterialPropertyBlock();
            for (int i = 0; i < renderers.Count; i++)
            {
                var r = renderers[i];
                if (!r) continue;
                r.GetPropertyBlock(block);
                block.SetColor(baseProps[i], baseColors[i] * (1f + .35f * k));
                r.SetPropertyBlock(block);
            }
            pulsed = true;
        }

        public void ClearPulse()
        {
            if (!pulsed) return;
            for (int i = 0; i < renderers.Count; i++)
                if (renderers[i]) renderers[i].SetPropertyBlock(null);
            pulsed = false;
        }

        // Deterministic core of LateUpdate so tests can pin the story gate
        // without entering playmode.
        public bool ShouldShow() =>
            !Taken() && (target == null || target.CanInteract()) &&
            (availableWhen == null || availableWhen());

        private void LateUpdate()
        {
            bool show = ShouldShow();
            if (!show) { ClearPulse(); return; }
            float t = Time.time + seed;
            float k = .5f + .5f * Mathf.Sin(t * Mathf.PI * 2 * 1.2f);
            ApplyPulse(k * k);
        }

        private void OnDisable() => ClearPulse();
        private void OnDestroy() => ClearPulse();

        // EditMode photo rigs don't run LateUpdate: pin the peak of the
        // blink before rendering so the still shows what players see.
        public void ApplyPeakForPhoto() => ApplyPulse(1f);
    }
}
