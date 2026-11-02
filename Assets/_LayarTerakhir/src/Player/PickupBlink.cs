using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.Rendering;

namespace LayarTerakhir.Player
{
    // Small gold blink floating above every must-take item (Kos supplies,
    // Damar's phone, clock hands, crowbar, basement gear, readable evidence).
    // A quiet 2 Hz brightness blink + gentle bob, billboarded to the camera.
    // Hides itself once the item is taken: parent deactivated, IInteractable
    // no longer interactable, or the completedFlag present in the campaign.
    public sealed class PickupBlink : MonoBehaviour
    {
        public string completedFlag;
        private MeshRenderer glow;
        private Material material;
        private Mesh mesh;
        private IInteractable target;
        private Vector3 baseLocal;
        private float seed;

        // Idempotent: safe to call from builders, repairs and directors alike.
        // Always re-snaps above the item so stale scenes self-correct.
        public static PickupBlink EnsureFor(GameObject item, string completedFlag = null)
        {
            if (!item) return null;
            var existing = item.GetComponentInChildren<PickupBlink>(true);
            if (existing)
            {
                if (!string.IsNullOrEmpty(completedFlag)) existing.completedFlag = completedFlag;
                existing.target = item.GetComponentInParent<IInteractable>() ?? item.GetComponent<IInteractable>();
                existing.SnapAbove();
                existing.EnsureBuilt();
                return existing;
            }
            var go = new GameObject("PickupBlink", typeof(MeshFilter), typeof(MeshRenderer));
            go.transform.SetParent(item.transform, false);
            var blink = go.AddComponent<PickupBlink>();
            blink.completedFlag = completedFlag;
            blink.target = item.GetComponentInParent<IInteractable>() ?? item.GetComponent<IInteractable>();
            blink.seed = Random.Range(0f, 10f);
            blink.SnapAbove();
            blink.EnsureBuilt();
            return blink;
        }

        private void Awake()
        {
            seed = Random.Range(0f, 10f);
            EnsureBuilt();
        }

        private void EnsureBuilt()
        {
            if (glow && material && mesh) return;
            var mf = GetComponent<MeshFilter>(); if (!mf) mf = gameObject.AddComponent<MeshFilter>();
            glow = GetComponent<MeshRenderer>(); if (!glow) glow = gameObject.AddComponent<MeshRenderer>();
            if (mf.sharedMesh) mesh = mf.sharedMesh;
            else
            {
                mesh = new Mesh { name = "Pickup blink" };
                mesh.vertices = new[] { new Vector3(0, .055f, 0), new Vector3(.017f, 0, 0), new Vector3(0, -.055f, 0), new Vector3(-.017f, 0, 0), new Vector3(-.04f, 0, 0), new Vector3(0, .009f, 0), new Vector3(.04f, 0, 0), new Vector3(0, -.009f, 0) };
                mesh.triangles = new[] { 0, 1, 2, 0, 2, 3, 4, 5, 6, 4, 6, 7 };
                mesh.RecalculateBounds();
                mf.sharedMesh = mesh;
            }
            if (glow.sharedMaterial) material = glow.sharedMaterial;
            else
            {
                var shader = Shader.Find("Universal Render Pipeline/Unlit") ?? Shader.Find("Sprites/Default");
                material = new Material(shader);
                glow.sharedMaterial = material;
            }
            glow.shadowCastingMode = ShadowCastingMode.Off;
            glow.receiveShadows = false;
        }

        private void SnapAbove()
        {
            float top = float.NegativeInfinity;
            var col = GetComponentInParent<Collider>();
            if (col) top = col.bounds.max.y;
            else foreach (var r in GetComponentsInParent<Renderer>()) if (r && r.enabled) top = Mathf.Max(top, r.bounds.max.y);
            if (float.IsNegativeInfinity(top)) top = transform.parent ? transform.parent.position.y + .1f : .3f;
            var parent = transform.parent;
            if (parent)
            {
                var anchor = new Vector3(parent.position.x, top + .16f, parent.position.z);
                baseLocal = parent.InverseTransformPoint(anchor);
            }
            else baseLocal = new Vector3(0, top + .16f, 0);
            transform.localPosition = baseLocal;
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

        private void LateUpdate()
        {
            if (!glow) EnsureBuilt();
            bool show = !Taken() && (target == null || target.CanInteract());
            glow.enabled = show;
            if (!show) return;
            var cam = Camera.main;
            if (cam) transform.rotation = cam.transform.rotation;
            float t = Time.time + seed;
            float blink = (Mathf.Sin(t * Mathf.PI * 2 * 2f) > 0) ? 1f : .3f;
            float pulse = Mathf.Pow(Mathf.Max(0, Mathf.Sin(t * 3.1f)), 6);
            transform.localScale = Vector3.one * (1.6f + pulse * .7f);
            transform.localPosition = baseLocal + new Vector3(0, Mathf.Sin(t * 2.2f) * .02f, 0);
            material.SetColor("_BaseColor", new Color(1f, .72f, .18f) * (1 + pulse * 2) * blink);
            if (material.HasProperty("_Color")) material.SetColor("_Color", new Color(1f, .78f, .25f, 1));
        }

        // EditMode photo rigs don't run LateUpdate: pin the blink at full
        // brightness facing the proof camera before rendering.
        public void ShowForPhoto(Camera cam)
        {
            EnsureBuilt();
            SnapAbove();
            if (cam) transform.rotation = cam.transform.rotation;
            transform.localScale = Vector3.one * 1.6f;
            material.SetColor("_BaseColor", new Color(1f, .72f, .18f) * 3);
            if (material.HasProperty("_Color")) material.SetColor("_Color", new Color(1f, .72f, .18f, 1));
            glow.enabled = true;
        }

        private void OnDestroy()
        {
            if (material) Destroy(material);
            if (mesh) Destroy(mesh);
        }
    }
}
