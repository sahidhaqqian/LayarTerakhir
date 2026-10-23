using System.Collections.Generic;
using UnityEngine;

namespace LayarTerakhir.Phase1
{
    // Authored room/loop graph with the *large* actor's clearance, scoped to this level.
    // Dynamic altar walls are probed at path time; no stale route through sealed 404.
    public sealed class Phase1Navigator : MonoBehaviour
    {
        private Transform[] nodes; private Transform actor, player;
        private readonly List<Vector3> route = new List<Vector3>();
        private Vector3 lastTarget; private float nextPath;
        private readonly RaycastHit[] hits = new RaycastHit[24];
        public void Configure(Transform[] points, Transform body, Transform playerBody) { nodes = points; actor = body; player=playerBody; Invalidate(); }
        public void Invalidate() { route.Clear(); nextPath = 0; }
        private bool Clear(Vector3 a, Vector3 b)
        {
            Vector3 delta = b - a; delta.y = 0;
            int count = Physics.CapsuleCastNonAlloc(a + Vector3.up * .62f, a + Vector3.up * 1.95f, .50f,
                delta.normalized, hits, delta.magnitude, ~(1 << 26 | 1 << 29 | 1 << 2), QueryTriggerInteraction.Ignore);
            // The target capsule is not a wall. Including it makes every candidate
            // goal unreachable and silently degrades pursuit to straight-line movement.
            var contacts=hits;
            if(count==hits.Length){contacts=Physics.CapsuleCastAll(a+Vector3.up*.62f,a+Vector3.up*1.95f,.50f,delta.normalized,delta.magnitude,~(1<<26|1<<29|1<<2),QueryTriggerInteraction.Ignore);count=contacts.Length;}
            for (int i = 0; i < count; i++)
                if (!contacts[i].transform.IsChildOf(actor)&&(!player||!contacts[i].transform.IsChildOf(player))) return false;
            return true;
        }
        public void Step(Vector3 target, float distance)
        {
            if (!actor || nodes == null) return;
            target.y = actor.position.y;
            if (Time.time >= nextPath && (route.Count == 0 || Vector3.Distance(lastTarget, target) > 1))
            { BuildPath(target); nextPath = Time.time + .65f; lastTarget = target; }
            Vector3 destination = route.Count > 0 ? route[0] : target;
            Vector3 next = Vector3.MoveTowards(actor.position, destination, distance);
            if (Clear(actor.position, next)) actor.position = next;
            else { route.Clear(); nextPath = 0; }
            if (route.Count > 0 && Vector3.Distance(actor.position, route[0]) < .18f) route.RemoveAt(0);
        }
        private void BuildPath(Vector3 target)
        {
            route.Clear();
            if (Clear(actor.position, target)) { route.Add(target); return; }
            int count = nodes.Length; var cost = new float[count]; var parent = new int[count]; var closed = new bool[count];
            for (int i = 0; i < count; i++) { cost[i] = Clear(actor.position, nodes[i].position) ? Vector3.Distance(actor.position, nodes[i].position) : float.PositiveInfinity; parent[i] = -1; }
            int goal = -1;
            for (int iteration = 0; iteration < count; iteration++)
            {
                int best = -1; float score = float.PositiveInfinity;
                for (int i = 0; i < count; i++) if (!closed[i] && cost[i] < score) { best = i; score = cost[i]; }
                if (best < 0) break;
                closed[best] = true;
                if (Clear(nodes[best].position, target)) { goal = best; break; }
                for (int j = 0; j < count; j++)
                {
                    if (closed[j] || j == best) continue;
                    float candidate = cost[best] + Vector3.Distance(nodes[best].position, nodes[j].position);
                    if (candidate < cost[j] && Clear(nodes[best].position, nodes[j].position)) { cost[j] = candidate; parent[j] = best; }
                }
            }
            if (goal < 0) return;
            for (int at = goal; at >= 0; at = parent[at]) route.Insert(0, nodes[at].position);
            route.Add(target);
        }
    }
}
