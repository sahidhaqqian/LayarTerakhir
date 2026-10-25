using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Phase5
{
    public sealed class Phase5Replicas : MonoBehaviour
    {
        public Phase5Director director;
        public GameObject[] actors;
        public Transform[] foci;
        private NavMeshAgent[] agents;
        private readonly int[] waypoint = new int[3];
        private readonly float[] searchUntil = new float[3], lungeAt = { -1, -1, -1 };
        private readonly Vector3[] lastSeen = new Vector3[3];
        private float selfieDwell, pausedUntil, selfieReady, graceUntil, lastContact = -100;
        private Phase5Balance B => director.balance;
        public void Restore()
        {
            agents = new NavMeshAgent[actors.Length];
            for (int i = 0; i < actors.Length; i++)
            {
                agents[i] = actors[i].GetComponent<NavMeshAgent>(); waypoint[i] = 0; lungeAt[i] = -1; searchUntil[i] = 0;
                actors[i].SetActive(false);
            }
            selfieDwell = pausedUntil = selfieReady = 0; graceUntil = director.Elapsed + B.restoreGrace; lastContact = -100;
        }
        public void Suspend()
        { if (agents == null) return; foreach (var a in agents) if (a && a.enabled && a.isOnNavMesh) a.isStopped = true; }
        public void Tick(float dt)
        {
            if (agents == null) return;
            bool s1 = director.Zone == "S1" && director.Completed("P5_ENTER_SECTORS") && !director.Completed("P5_S1_CLEAR");
            bool s3 = director.Zone == "S3" && director.Completed("P5_S2_CLEAR") && !director.Completed("P5_S3_CLEAR");
            for (int i = 0; i < actors.Length; i++)
            {
                bool active = i == 0 ? s1 : s3;
                if (actors[i].activeSelf != active)
                {
                    actors[i].SetActive(active);
                    if (active)
                    {
                        Transform first = Path(i)[0]; var a = agents[i];
                        if (a && NavMesh.SamplePosition(first.position, out var hit, 2, NavMesh.AllAreas)) a.Warp(hit.position);
                        graceUntil = director.Elapsed + B.restoreGrace;
                    }
                }
            }
            if (s1) TickGhost(dt);
            if (s3) { TickPatrol(1); TickPatrol(2); }
        }
        private Transform[] Path(int index) => index == 0 ? director.world.ghostWaypoints : index == 1 ? director.world.sriWaypoints : director.world.giantWaypoints;
        private void Go(int index, Vector3 point, float speed)
        {
            var a = agents[index]; if (!a || !a.enabled || !a.isOnNavMesh) return;
            a.isStopped = false; a.speed = speed;
            if (NavMesh.SamplePosition(point, out var hit, 2, NavMesh.AllAreas)) a.SetDestination(hit.position);
        }
        private void TickGhost(float dt)
        {
            bool framed = director.CameraCanScan && director.Phone.isSelfieCamera && director.Aim(foci[0], 12, 35, actors[0].transform);
            selfieDwell = framed ? selfieDwell + dt : 0;
            if (selfieDwell >= B.selfieLook && director.Elapsed >= selfieReady)
            {
                pausedUntil = director.Elapsed + B.selfiePause; selfieReady = director.Elapsed + B.selfieCooldown; selfieDwell = 0;
                director.Hud.EnqueueSubtitle("Replika Memori", "[Sosok membeku dalam frame Selfie. Empat detik untuk memeriksa pintu.]", 4);
            }
            if (director.Elapsed < pausedUntil) { if (agents[0].isOnNavMesh) agents[0].isStopped = true; return; }
            Go(0, director.player.transform.position, B.s1GhostSpeed); Contact(0);
        }
        private void TickPatrol(int index)
        {
            var actor = actors[index].transform; var player = director.player; var delta = player.cameraPivot.position - foci[index].position;
            float range = delta.magnitude; float hear = player.IsSprinting ? B.hearingSprint : player.IsCrouching ? B.hearingCrouch : B.hearingWalk;
            if (director.Campaign.Data.sanity <= 0) hear *= 1.25f;
            var capsule = player.GetComponent<CharacterController>();
            bool clear = director.ClearLine(foci[index].position, player.cameraPivot.position, actor);
            bool lit = director.Phone.isPhoneRaised && director.Phone.isScreenOn && range <= B.screenRange && Vector3.Dot(actor.forward, delta.normalized) > -.25f && clear;
            bool heard = range <= hear && capsule.velocity.sqrMagnitude > .015f;
            if (lit || heard) { lastSeen[index] = player.transform.position; searchUntil[index] = director.Elapsed + B.searchTime; }
            if (director.Elapsed < searchUntil[index]) Go(index, lastSeen[index], index == 1 ? B.sriSpeed : B.giantSpeed);
            else
            {
                var path = Path(index);
                if (Vector3.Distance(actor.position, path[waypoint[index]].position) < 1) waypoint[index] = (waypoint[index] + 1) % path.Length;
                Go(index, path[waypoint[index]].position, index == 1 ? B.sriSpeed : B.giantSpeed);
            }
            Contact(index);
        }
        private void Contact(int index)
        {
            var actor = actors[index].transform; float distance = Vector3.Distance(actor.position, director.player.transform.position);
            if (distance > 1.25f || director.Elapsed < graceUntil || !director.ClearLine(foci[index].position, director.player.cameraPivot.position, actor)) { lungeAt[index] = -1; return; }
            if (lungeAt[index] < 0)
            {
                lungeAt[index] = director.Elapsed;
                director.Hud.EnqueueSubtitle("Replika Memori", index == 0 ? "[Jeritan mendekat dari punggung.]" : "[Gunting bergesek; langkah berat berhenti di dekatmu.]", 3);
                director.soundscape.Cue("Lunge", actor.position);
            }
            if (director.Elapsed - lungeAt[index] < B.lungeTelegraph) return;
            director.Penalty(B.contactSanityLoss, "Sentuhan Replika Memori — putus LOS dan menjauh.");
            Vector3 away = director.player.transform.position - actor.position; away.y = 0;
            if (away.sqrMagnitude > .01f) director.player.GetComponent<CharacterController>().Move(away.normalized * .55f);
            if (director.Elapsed - lastContact < B.contactWindow) director.Retry("CAUGHT_REPLICA — dua kontak dalam sepuluh detik.");
            lastContact = director.Elapsed; graceUntil = director.Elapsed + B.contactIFrame; lungeAt[index] = -1;
        }
    }
}
