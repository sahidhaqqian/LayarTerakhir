using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Phase4
{
    public enum BanaspatiState { Dormant, Reveal, Patrol, Investigate, Chase, Search, Recover, Exposed, Merging, Resolved }
    public sealed class Phase4Banaspati : MonoBehaviour
    {
        public Phase4Director director;
        public NavMeshAgent agent;
        public Transform visual;
        public BanaspatiState State { get; private set; }
        public bool Pursuing => State == BanaspatiState.Chase || State == BanaspatiState.Investigate || State == BanaspatiState.Search;
        private Phase4World W => director.world;
        private Phase4Balance B => director.balance;
        private Vector3 known, previousPlayer, emberTarget;
        private int waypoint, contacts;
        private float grace, search, repath, noiseClock, lunge, invulnerable, lastHit = -100, ember = -1, nextEmber;
        public float LungeProgress => lunge;
        public void Restore()
        {
            State = director.Completed("P4_BOSS_WIN") ? BanaspatiState.Resolved : director.Completed("P4_BANASPATI_REVEAL") ? BanaspatiState.Recover : BanaspatiState.Dormant;
            grace = B.restoreGrace; lunge = contacts = 0; invulnerable = 0; previousPlayer = director.player.transform.position;
            W.emberWarning.SetActive(false);
            W.banaspati.SetActive(State != BanaspatiState.Resolved && State != BanaspatiState.Dormant);
            if (State == BanaspatiState.Resolved) { agent.enabled = false; return; }
            agent.enabled = true; agent.areaMask = NavMesh.AllAreas & ~(1 << 3);
            Vector3 start = W.patrol[0].position;
            foreach (var anchor in W.patrol) if (Vector3.Distance(anchor.position, previousPlayer) > 18) { start = anchor.position; break; }
            if (NavMesh.SamplePosition(start, out var hit, 3, agent.areaMask)) agent.Warp(hit.position);
            PatrolVisual();
        }
        public void Reveal()
        {
            W.banaspati.SetActive(true); State = BanaspatiState.Reveal; grace = B.restoreGrace;
            W.suro.SetActive(true); PatrolVisual(); director.soundscape.Cue("FirePass", W.banaspati.transform.position);
        }
        public void RitualBegan() { W.banaspati.SetActive(true); agent.enabled = true; State = BanaspatiState.Recover; grace = 12; }
        public void Tick(float dt)
        {
            if (State == BanaspatiState.Resolved || State == BanaspatiState.Dormant || director.Dead) return;
            invulnerable = Mathf.Max(0, invulnerable - dt); grace = Mathf.Max(0, grace - dt);
            if (director.ritual.Exposed) { State = BanaspatiState.Exposed; Suspend(); return; }
            if (director.Completed("P4_SOUTH_OFF")) { State = BanaspatiState.Merging; return; }
            if (director.ritual.Recovering || grace > 0)
            {
                State = BanaspatiState.Recover;
                Follow(FarthestPatrol(), B.patrolSpeed, dt); return;
            }
            PatrolVisual();
            Vector3 player = director.player.transform.position;
            float distance = Vector3.Distance(player, W.banaspati.transform.position);
            float multiplier = director.Campaign.Data.sanity <= 0 ? 2 : director.Campaign.Data.sanity < 40 ? 1.5f : 1;
            bool line = director.ClearLine(visual.position, director.player.cameraPivot.position, W.banaspati.transform);
            bool ward = W.InWard(player);
            float radius = director.Phone.isScreenOn && director.Phone.isPhoneRaised ? B.screenRadius : 7;
            bool seen = line && distance < radius * multiplier;
            noiseClock -= dt;
            bool moved = Vector3.Distance(player, previousPlayer) > .035f;
            float hearing = director.player.IsCrouching ? B.hearingCrouch : director.player.IsSprinting ? B.hearingSprint : B.hearingWalk;
            bool heard = moved && noiseClock <= 0 && distance < hearing * multiplier * (line ? 1 : .35f);
            bool heartbeat = director.Campaign.Data.sanity < 40 && !director.Hidden && line && distance < 9 * multiplier;
            if (ward)
            {
                lunge = 0; State = BanaspatiState.Recover; grace = Mathf.Max(grace, 2);
                Follow(FarthestPatrol(), B.alertSpeed, dt); previousPlayer = player; return;
            }
            if (seen || heard || heartbeat)
            {
                known = player; search = B.searchSeconds; State = seen ? BanaspatiState.Chase : BanaspatiState.Investigate;
                if (heard) noiseClock = .8f;
            }
            else if (Pursuing)
            {
                State = BanaspatiState.Search; search -= dt;
                if (search <= 0) State = BanaspatiState.Patrol;
            }
            if (State == BanaspatiState.Reveal || State == BanaspatiState.Recover) State = BanaspatiState.Patrol;
            if (State == BanaspatiState.Patrol)
            {
                if (Vector3.Distance(W.banaspati.transform.position, W.patrol[waypoint].position) < 1.2f) waypoint = (waypoint + 1) % W.patrol.Length;
                Follow(W.patrol[waypoint].position, B.patrolSpeed, dt);
            }
            else Follow(known, State == BanaspatiState.Chase ? director.Campaign.Data.sanity <= 0 ? B.huntSpeed : B.chaseSpeed : B.alertSpeed, dt);
            bool contact = distance <= B.lungeRadius && line && invulnerable <= 0;
            if (contact)
            {
                if (lunge <= 0) { director.soundscape.Cue("Lunge", visual.position); director.Hud.ShowBroadcastNotification("BARA MENYAMBAR — MENJAUH ATAU PUTUS PANDANGAN!", 2); }
                lunge += dt;
                if (lunge >= B.lungeWarning) Hit();
            }
            else lunge = 0;
            if (director.ritual.Active && Phase4Ids.LocalHits(director.Campaign.Data) == 3 && director.CurrentZone == "SOUTH") TickEmber(dt);
            else if (ember >= 0) { ember = -1; W.emberWarning.SetActive(false); }
            previousPlayer = player;
        }
        private void Hit()
        {
            float now = director.Elapsed;
            contacts = now - lastHit <= B.hitWindow ? contacts + 1 : 1; lastHit = now;
            invulnerable = B.hitIFrame; lunge = 0;
            if (contacts >= 2) { director.Die("CAUGHT_BANASPATI"); return; }
            director.Campaign.Data.sanity = Mathf.Max(0, director.Campaign.Data.sanity - 25);
            Vector3 away = director.player.transform.position - W.banaspati.transform.position; away.y = 0;
            director.player.GetComponent<CharacterController>().Move(away.normalized * .6f);
            director.Hud.EnqueueSubtitle("Raka", "[Bara menyambar. Langkah terhuyung; masih bisa menjauh.]", 3);
            grace = 1;
        }
        private void TickEmber(float dt)
        {
            if (ember < 0 && director.Elapsed >= nextEmber)
            {
                ember = 0; emberTarget = director.player.transform.position; nextEmber = director.Elapsed + B.southEmberInterval;
                W.emberWarning.transform.position = emberTarget + Vector3.up * .03f; W.emberWarning.SetActive(true);
                director.soundscape.Cue("EmberWarning", emberTarget); director.Hud.EnqueueSubtitle("Mbah Suro", "[Lingkar bara memerah di tanah. MENJAUH!]", 2);
            }
            if (ember >= 0)
            {
                ember += dt;
                if (ember >= B.southEmberWarning)
                {
                    if (Vector3.Distance(director.player.transform.position, emberTarget) < B.southEmberRadius && !W.InWard(director.player.transform.position))
                        director.Campaign.Data.sanity = Mathf.Max(0, director.Campaign.Data.sanity - 30);
                    director.soundscape.Cue("EmberBurst", emberTarget); ember = -1; W.emberWarning.SetActive(false);
                }
            }
        }
        private Vector3 FarthestPatrol()
        {
            Vector3 best = W.patrol[0].position; float distance = 0;
            foreach (var p in W.patrol) { float d = Vector3.Distance(p.position, director.player.transform.position); if (d > distance) { distance = d; best = p.position; } }
            return best;
        }
        private void PatrolVisual() { W.fireVisual.SetActive(true); W.skeletonVisual.SetActive(false); W.entityLight.enabled = true; }
        private void Follow(Vector3 target, float speed, float dt)
        {
            if (!agent || !agent.enabled || !agent.isOnNavMesh) return;
            repath -= dt;
            if (repath <= 0) { repath = .2f; agent.speed = speed; agent.isStopped = false; agent.SetDestination(target); }
        }
        public void Expose()
        { State = BanaspatiState.Exposed; Suspend(); W.fireVisual.SetActive(false); W.skeletonVisual.SetActive(true); W.entityLight.enabled = true; director.soundscape.Cue("ShieldBreak", visual.position); }
        public void Recover() { State = BanaspatiState.Recover; grace = B.recoverySeconds; lunge = 0; PatrolVisual(); }
        public void RecoilFromFalseTorch() { grace = 2; State = BanaspatiState.Recover; lunge = 0; }
        public void BeginMerge() { State = BanaspatiState.Merging; W.skeletonVisual.SetActive(true); W.fireVisual.SetActive(false); }
        public void MergeAtPendopo(float dt) { Follow(W.Anchor("PENDOPO").position, B.chaseSpeed, dt); }
        public void Resolve() { State = BanaspatiState.Resolved; Suspend(); if (agent) agent.enabled = false; W.entityLight.enabled = false; }
        public void Suspend() { if (agent && agent.enabled && agent.isOnNavMesh) { agent.isStopped = true; agent.ResetPath(); } }
        private void LateUpdate()
        {
            if (!visual || !director.ClockRunning) return;
            var p = visual.localPosition; p.y = 1.75f + Mathf.Sin(director.Elapsed * 1.7f) * .12f; visual.localPosition = p;
        }
    }
}
