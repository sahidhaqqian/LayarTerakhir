#if UNITY_EDITOR || DEVELOPMENT_BUILD
using System.Text;
using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LayarTerakhir.Review
{
    // Pure helpers shared by the review modules. No scene side effects, fully unit-testable.
    public static class ReviewHierarchy
    {
        public static string BuildPath(Transform target)
        {
            if (!target) return "<none>";
            var sb = new StringBuilder(target.name);
            for (var p = target.parent; p; p = p.parent) sb.Insert(0, p.name + "/");
            return sb.ToString();
        }

        public static string FormatVector(Vector3 v)
        {
            return "(" + v.x.ToString("F2") + ", " + v.y.ToString("F2") + ", " + v.z.ToString("F2") + ")";
        }

        public static string FormatTransform(Transform t)
        {
            if (!t) return "<none>";
            return "pos " + FormatVector(t.position) + " | rot " + FormatVector(t.rotation.eulerAngles)
                + " | scale " + FormatVector(t.localScale);
        }

        public static string DescribeRenderer(GameObject go)
        {
            if (!go) return "renderer: <none>";
            var r = go.GetComponent<Renderer>();
            if (!r) r = go.GetComponentInChildren<Renderer>();
            if (!r) return "renderer: none";
            var sb = new StringBuilder("renderer: " + r.GetType().Name);
            sb.Append(r.enabled ? " (enabled)" : " (disabled)");
            sb.Append(" | bounds " + FormatVector(r.bounds.center) + " size " + FormatVector(r.bounds.size));
            var mats = r.sharedMaterials;
            sb.Append(" | materials: ");
            if (mats == null || mats.Length == 0) sb.Append("none");
            else
            {
                for (int i = 0; i < mats.Length; i++)
                {
                    if (i > 0) sb.Append(", ");
                    sb.Append(mats[i] ? mats[i].name : "<missing>");
                }
            }
            return sb.ToString();
        }

        public static string DescribeCollider(GameObject go)
        {
            if (!go) return "collider: <none>";
            var c = go.GetComponent<Collider>();
            if (!c) c = go.GetComponentInChildren<Collider>();
            if (!c) return "collider: none";
            return "collider: " + c.GetType().Name + (c.enabled ? " (enabled)" : " (disabled)")
                + (c.isTrigger ? " [trigger]" : "")
                + " | bounds " + FormatVector(c.bounds.center) + " size " + FormatVector(c.bounds.size);
        }

        public static string SelectionReport(GameObject go, Vector3 hitPoint, bool hasHit)
        {
            if (!go) return "Belum ada objek terpilih. Aktifkan F9 lalu klik objek.";
            var sb = new StringBuilder();
            sb.AppendLine("Nama: " + go.name);
            sb.AppendLine("Path: " + BuildPath(go.transform));
            sb.AppendLine("Scene: " + go.scene.name + " (" + (go.scene.isLoaded ? "loaded" : "not loaded") + ")");
            sb.AppendLine("Aktif: " + (go.activeInHierarchy ? "ya" : "tidak")
                + " | Layer: " + LayerMask.LayerToName(go.layer) + " (" + go.layer + ") | Tag: " + go.tag);
            sb.AppendLine("Transform: " + FormatTransform(go.transform));
            if (hasHit) sb.AppendLine("Titik klik: " + FormatVector(hitPoint));
            sb.AppendLine(DescribeRenderer(go));
            sb.AppendLine(DescribeCollider(go));
            return sb.ToString();
        }

        public static string PhaseLabel(CampaignData data)
        {
            if (data == null) return "campaign?";
            if (CampaignRouting.IsPhase5(data)) return "Phase 5";
            if (CampaignRouting.IsPhase4(data)) return "Phase 4";
            if (CampaignRouting.IsPhase3(data)) return "Phase 3";
            if (CampaignRouting.IsPhase2(data)) return "Phase 2";
            if (CampaignRouting.IsPhase1(data)) return "Phase 1";
            return data.chapter_id == 0 ? "Prolog/Interlude" : "chapter " + data.chapter_id;
        }

        public static string ActiveSceneName()
        {
            try { return SceneManager.GetActiveScene().name; }
            catch { return "?"; }
        }
    }
}
#endif
