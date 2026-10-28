#if UNITY_EDITOR || DEVELOPMENT_BUILD
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using UnityEngine;
using UnityEngine.EventSystems;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace LayarTerakhir.Review
{
    // Click-to-inspect for environment objects, including runtime-generated ones.
    // Highlighting uses a standalone line box; target materials are never touched.
    public sealed class ReviewObjectInspector : MonoBehaviour
    {
        public bool Active { get; private set; }
        public GameObject Selected { get; private set; }
        public Vector3 HitPoint { get; private set; }
        public bool HasHit { get; private set; }

        private FirstPersonController player;
        private InteractionInteractor interactor;
        private bool savedController, savedInteractor;
        private GameObject highlight;
        private Material highlightMaterial;
        private Mesh lineBoxMesh;
        private float pulse;

        public string CurrentReport => ReviewHierarchy.SelectionReport(Selected, HitPoint, HasHit);

        public bool Enable(FirstPersonController target)
        {
            if (Active) return true;
            player = target ? target : FindFirstObjectByType<FirstPersonController>();
            if (player) interactor = player.GetComponent<InteractionInteractor>();
            // On-foot inspection freezes input so clicks select objects instead of
            // interacting; the rig stays on so the normal camera is kept.
            savedController = player && player.enabled;
            savedInteractor = interactor && interactor.enabled;
            if (player) player.enabled = false;
            if (interactor) interactor.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Active = true;
            return true;
        }

        public void Disable()
        {
            if (!Active) return;
            Active = false;
            ClearSelection();
            if (player) player.enabled = savedController;
            if (interactor) interactor.enabled = savedInteractor;
            if (player) player.SetCursorLock(!UiFocus.BlocksWorld);
            else { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
            player = null;
            interactor = null;
        }

        public void ExitForSceneLoad()
        {
            Active = false;
            Selected = null;
            DestroyHighlight();
            player = null;
            interactor = null;
        }

        public void ClearSelection()
        {
            Selected = null;
            HasHit = false;
            DestroyHighlight();
        }

        public Camera PickCamera()
        {
            var fly = GetComponent<ReviewFlyCamera>();
            if (fly && fly.Active && fly.Eye) return fly.Eye;
            if (interactor && interactor.playerCamera) return interactor.playerCamera;
            if (player)
            {
                var i = player.GetComponent<InteractionInteractor>();
                if (i && i.playerCamera) return i.playerCamera;
            }
            return Camera.main;
        }

        private void OnDisable()
        {
            if (Active) Disable();
            else DestroyHighlight();
        }

        private void Update()
        {
            if (!Active) return;
#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (EventSystem.current && EventSystem.current.IsPointerOverGameObject()) return;
                Pick(Mouse.current.position.ReadValue());
            }
#endif
            UpdateHighlight();
        }

        public void Pick(Vector2 screenPos)
        {
            var cam = PickCamera();
            if (!cam) return;
            Ray ray = cam.ScreenPointToRay(new Vector3(screenPos.x, screenPos.y, 0f));
            var hits = Physics.RaycastAll(ray, 500f, ~0, QueryTriggerInteraction.Collide);
            System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
            foreach (var h in hits)
            {
                if (!h.collider) continue;
                var go = h.collider.gameObject;
                if (IsReviewOwned(go.transform)) continue;
                Select(go, h.point, true);
                return;
            }
            // Fallback for procedural meshes without colliders: nearest bounds hit.
            var renderers = FindObjectsByType<Renderer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            Renderer best = null;
            float bestDist = float.PositiveInfinity;
            foreach (var r in renderers)
            {
                if (!r || !r.enabled || IsReviewOwned(r.transform)) continue;
                if (r.bounds.IntersectRay(ray, out float dist) && dist < bestDist && dist <= 500f)
                {
                    bestDist = dist;
                    best = r;
                }
            }
            if (best) Select(best.gameObject, ray.GetPoint(bestDist), true);
        }

        // Test seam: same path as a mouse pick, minus the raycast.
        public void SelectDirect(GameObject go, Vector3 point) => Select(go, point, true);

        private void Select(GameObject go, Vector3 point, bool hasHit)
        {
            Selected = go;
            HitPoint = point;
            HasHit = hasHit;
            EnsureHighlight();
            UpdateHighlight();
            var mode = GetComponent<DeveloperReviewMode>();
            if (mode) mode.RefreshInspectorPanel();
        }

        private bool IsReviewOwned(Transform t)
        {
            for (var p = t; p; p = p.parent)
                if (p == transform.root && p.GetComponent<DeveloperReviewMode>()) return true;
            return t == highlight?.transform || (highlight && t.IsChildOf(highlight.transform));
        }

        private void EnsureHighlight()
        {
            if (highlight) return;
            highlight = new GameObject("ReviewSelectionBox");
            highlight.transform.SetParent(transform, false);
            var filter = highlight.AddComponent<MeshFilter>();
            filter.sharedMesh = LineBoxMesh();
            var renderer = highlight.AddComponent<MeshRenderer>();
            highlightMaterial = BuildLineMaterial();
            renderer.sharedMaterial = highlightMaterial;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
            renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        }

        private Mesh LineBoxMesh()
        {
            if (lineBoxMesh) return lineBoxMesh;
            var m = new Mesh { name = "ReviewLineBox" };
            m.vertices = new[]
            {
                new Vector3(-.5f, -.5f, -.5f), new Vector3(.5f, -.5f, -.5f),
                new Vector3(.5f, -.5f, .5f), new Vector3(-.5f, -.5f, .5f),
                new Vector3(-.5f, .5f, -.5f), new Vector3(.5f, .5f, -.5f),
                new Vector3(.5f, .5f, .5f), new Vector3(-.5f, .5f, .5f),
            };
            m.SetIndices(new[]
            {
                0, 1, 1, 2, 2, 3, 3, 0, 4, 5, 5, 6, 6, 7, 7, 4, 0, 4, 1, 5, 2, 6, 3, 7,
            }, MeshTopology.Lines, 0);
            m.UploadMeshData(true);
            lineBoxMesh = m;
            return m;
        }

        private static Material BuildLineMaterial()
        {
            Shader shader = Shader.Find("Universal Render Pipeline/Unlit")
                ?? Shader.Find("Unlit/Color") ?? Shader.Find("Sprites/Default");
            var mat = shader ? new Material(shader) : new Material(Shader.Find("Hidden/InternalErrorShader"));
            mat.name = "ReviewSelectionLine";
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", Color.yellow);
            if (mat.HasProperty("_Color")) mat.SetColor("_Color", Color.yellow);
            return mat;
        }

        private void UpdateHighlight()
        {
            if (!highlight) return;
            if (!Selected)
            {
                highlight.SetActive(false);
                return;
            }
            Bounds bounds;
            var renderer = Selected.GetComponent<Renderer>() ?? Selected.GetComponentInChildren<Renderer>();
            var collider = Selected.GetComponent<Collider>() ?? Selected.GetComponentInChildren<Collider>();
            if (renderer) bounds = renderer.bounds;
            else if (collider) bounds = collider.bounds;
            else bounds = new Bounds(Selected.transform.position, Vector3.one * 0.5f);
            if (bounds.size.sqrMagnitude < 0.0001f) bounds.size = Vector3.one * 0.25f;
            highlight.SetActive(true);
            highlight.transform.SetPositionAndRotation(bounds.center, Quaternion.identity);
            highlight.transform.localScale = bounds.size + Vector3.one * 0.05f;
            pulse += Time.unscaledDeltaTime * 4f;
            float glow = 0.75f + 0.25f * Mathf.Sin(pulse);
            if (highlightMaterial)
            {
                var c = Color.Lerp(Color.yellow, Color.white, 1f - glow);
                if (highlightMaterial.HasProperty("_BaseColor")) highlightMaterial.SetColor("_BaseColor", c);
                if (highlightMaterial.HasProperty("_Color")) highlightMaterial.SetColor("_Color", c);
            }
        }

        private void DestroyHighlight()
        {
            if (highlightMaterial) DestroyObject(highlightMaterial);
            highlightMaterial = null;
            if (lineBoxMesh) DestroyObject(lineBoxMesh);
            lineBoxMesh = null;
            if (highlight) DestroyObject(highlight);
            highlight = null;
        }

        private static void DestroyObject(UnityEngine.Object obj)
        {
            if (Application.isPlaying) Destroy(obj);
            else DestroyImmediate(obj);
        }
    }
}
#endif
