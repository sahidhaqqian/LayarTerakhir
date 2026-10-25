using System.Linq;
using LayarTerakhir.Core;
using UnityEngine;
using UnityEngine.AI;
namespace LayarTerakhir.Phase5
{
    public sealed class Phase5Boss : MonoBehaviour
    {
        public Phase5Director director;
        public GameObject waveVisual;
        public Transform waveOrigin;
        public Transform[] threadMeshes;
        public int Phase { get; private set; }
        public bool Active => director.Ready && director.Completed("P5_ARENA_LOCK") && !director.FinalChoice && Phase5Ids.CoreCount(director.Campaign.Data) < 3;
        public bool Exposed => Active && director.Elapsed < exposureUntil;
        public bool RearCold => Phase == 2 && Vector3.Distance(P.position, W.bossRoot.transform.position) <= B.rearColdRange &&
            Vector3.Dot(P.forward, (W.bossRoot.transform.position - P.position).normalized) < -.1f;
        public string Status => "INTI " + Phase5Ids.CoreCount(director.Campaign.Data) + "/3 • " +
            (Exposed ? "PERISAI TERBUKA • BLITZ " + Mathf.Max(0, exposureUntil - director.Elapsed).ToString("0.0") + "s" :
            Phase == 1 ? "UV: BENANG KONTINU → SOROT GENERATOR → BLITZ KEPALA" :
            Phase == 2 ? "SELFIE + THERMO −30°C → FLARE / LENTERA → BLITZ" :
            "AR: KABEL " + Phase5Ids.CableCount(director.Campaign.Data) + "/3 • LAYAR MATI + COVER SAAT API");
        private Phase5World W => director.world;
        private Phase5Balance B => director.balance;
        private Transform P => director.player.transform;
        private NavMeshAgent agent;
        private float phaseStarted, exposureUntil, generatorReady, repathAt, nextCoreRetry, lungeStarted = -1, contactGrace, lastContact = -100;
        private float lightUntil;
        private int carriedLantern = -1, wavesShown;
        private readonly float[] lanternReady = new float[3], shadowReady = new float[3];
        private Vector3 lightPosition;
        public void Restore(bool resetTools = true)
        {
            Phase = Mathf.Clamp(Phase5Ids.CoreCount(director.Campaign.Data) + 1, 1, 3);
            phaseStarted = director.Elapsed; exposureUntil = generatorReady = nextCoreRetry = repathAt = 0;
            if(resetTools){lightUntil=0;carriedLantern=-1;for(int i=0;i<3;i++){lanternReady[i]=0;W.lanternVisuals[i].transform.SetParent(W.lanternPosts[i],true);W.lanternVisuals[i].transform.position=W.lanternPosts[i].position+Vector3.up*.6f;}}
            lungeStarted = -1; contactGrace = director.Elapsed + B.restoreGrace; lastContact = -100; wavesShown = 0;
            agent = W.bossRoot.GetComponent<NavMeshAgent>();
            bool active = director.Completed("P5_ARENA_LOCK") && !director.FinalChoice && Phase5Ids.CoreCount(director.Campaign.Data) < 3;
            W.bossRoot.SetActive(active || director.FinalChoice);
            for (int i = 0; i < W.clones.Length; i++) W.clones[i].SetActive(active && Phase == 1);
            foreach (var light in W.stageLights) light.enabled = !active || Phase == 1;
            W.generatorLight.enabled = false; W.droppedLight.enabled = false; if (waveVisual) waveVisual.SetActive(false);
            if (active && agent) { agent.enabled = true; if (agent.isOnNavMesh) agent.isStopped = false; }
            SetPhasePose();
        }
        private void SetPhasePose()
        {
            // Licensed cloth/camera assembly is transformed as an apparition; no
            // character mesh or animation is authored by this controller.
            W.bossRoot.transform.localScale = Vector3.one * (Phase == 2 ? .68f : 1);
            if (Phase == 3)
            {
                var center = new Vector3(0,.05f,228);
                if (agent && agent.enabled && agent.isOnNavMesh) agent.Warp(center);
                else W.bossRoot.transform.position = center;
            }
        }
        public void Suspend() { if (agent && agent.enabled && agent.isOnNavMesh) agent.isStopped = true; }
        private void Go(Vector3 target, float dt)
        {
            if (!agent || !agent.enabled || !agent.isOnNavMesh) return;
            agent.isStopped = false; agent.speed = B.bossSpeed;
            if (director.Elapsed < repathAt) return; repathAt = director.Elapsed + .2f;
            if (NavMesh.SamplePosition(target, out var hit, 3, NavMesh.AllAreas)) agent.SetDestination(hit.position);
        }
        public void Tick(float dt)
        {
            if (!Active) { Suspend(); return; }
            int actual = Mathf.Clamp(Phase5Ids.CoreCount(director.Campaign.Data) + 1, 1, 3);
            if (Phase != actual) Restore(false);
            W.droppedLight.enabled = director.Elapsed < lightUntil;
            W.droppedLight.transform.position = lightPosition + Vector3.up * .4f;
            for(int i=0;i<3;i++)if(carriedLantern!=i&&director.Elapsed>=lanternReady[i]&&director.Elapsed>=lightUntil)
            {W.lanternVisuals[i].transform.SetParent(W.lanternPosts[i],true);W.lanternVisuals[i].transform.position=W.lanternPosts[i].position+Vector3.up*.6f;}
            UpdateThreads();
            if (Exposed) { Suspend(); W.generatorLight.enabled = Phase == 1; director.phoneView.ShowChapterTarget((Phase == 3 ? W.bossChest : W.bossHead).position, true); return; }
            W.generatorLight.enabled = false; director.phoneView.ShowChapterTarget(Vector3.zero, false);
            if (Phase == 1) PhaseOne(dt);
            else if (Phase == 2) PhaseTwo(dt);
            else PhaseThree(dt);
        }
        private void PhaseOne(float dt)
        {
            Go(P.position, dt);
            for (int i = 0; i < W.clones.Length; i++)
            {
                var clone = W.clones[i];
                if (!clone.activeSelf && director.Elapsed >= shadowReady[i]) clone.SetActive(true);
                if (!clone.activeSelf) continue;
                var at = clone.transform.position;
                var delta = P.position - at; delta.y = 0;
                if (delta.magnitude > 1.4f) clone.transform.position += delta.normalized * .8f * dt;
                if (delta.sqrMagnitude > .01f) clone.transform.rotation = Quaternion.Slerp(clone.transform.rotation, Quaternion.LookRotation(delta), dt * 2);
                if (delta.magnitude < 1.5f && director.Elapsed > contactGrace && director.ClearLine(clone.transform.position + Vector3.up * 2.5f, P.position + Vector3.up * .65f, clone.transform))
                { director.Penalty(B.shadowTouchLoss, "Bayangan layar pecah. Benang terputus bukan wujud asli."); clone.SetActive(false); shadowReady[i] = director.Elapsed + B.shadowRespawn; }
            }
            ContactCheck();
        }
        private void PhaseTwo(float dt)
        {
            if (director.Elapsed - phaseStarted < B.rearStalkSeconds) { Suspend(); return; }
            Go(P.position - P.forward * 1.7f, dt);
            if (director.Elapsed < lightUntil && Vector3.Distance(lightPosition, W.bossRoot.transform.position) <= B.flareRadius && RearCold &&
                director.CameraCanScan && director.Phone.isSelfieCamera && director.ThermoActive)
            { OpenWindow(); return; }
            if (RearCold && director.Elapsed > contactGrace)
            {
                if (lungeStarted < 0) { lungeStarted = director.Elapsed; director.Hud.EnqueueSubtitle("Penonton_Terakhir", "[Napas membeku tepat di belakang leher. Rahang lensa membuka.]", 3); director.soundscape.Cue("Lunge", W.bossHead.position); }
                if (director.Elapsed - lungeStarted >= B.lungeTelegraph) Contact();
            }
            else lungeStarted = -1;
        }
        private void PhaseThree(float dt)
        {
            Suspend();
            if (Phase5Ids.CableCount(director.Campaign.Data) == 3)
            {
                if (waveVisual) waveVisual.SetActive(false);
                if (director.Elapsed >= nextCoreRetry) OpenWindow();
                return;
            }
            float cycle = B.waveTelegraph + B.waveSweep + B.waveGap;
            float elapsed = director.Elapsed - phaseStarted;
            int index = Mathf.FloorToInt(elapsed / cycle);
            float local = elapsed - index * cycle;
            if (index >= B.waveCount) { if (waveVisual) waveVisual.SetActive(false); return; }
            if (wavesShown <= index)
            {
                wavesShown = index + 1;
                director.Hud.EnqueueSubtitle("Panggung", "[Api melesat dari tepi. Matikan layar dan jongkok di balik peti.]", B.waveTelegraph + B.waveSweep);
                director.soundscape.Cue("FireWarning", waveOrigin.position);
            }
            bool sweep = local >= B.waveTelegraph && local < B.waveTelegraph + B.waveSweep;
            if (waveVisual) { waveVisual.SetActive(sweep); waveVisual.transform.position = waveOrigin.position + Vector3.right * Mathf.Clamp01((local - B.waveTelegraph) / B.waveSweep) * 38; }
            if (!sweep || director.Elapsed < contactGrace) return;
            float front = waveOrigin.position.x + Mathf.Clamp01((local - B.waveTelegraph) / B.waveSweep) * 38;
            if (Mathf.Abs(P.position.x - front) > 1.5f) return;
            Vector3 from = new Vector3(waveOrigin.position.x, 1.2f, P.position.z);
            bool blocked = !director.ClearLine(from, P.position + Vector3.up * .65f);
            bool protectedByCover = director.player.IsCrouching && !director.Phone.isScreenOn && blocked;
            if (!protectedByCover) director.Retry("Sapuan api menembus jalur terbuka. Berlindung di sisi peti yang membelakangi sumber api.");
        }
        public bool ActivateGenerator()
        {
            if (!Active || Phase != 1 || director.Elapsed < generatorReady || Exposed) return false;
            if (Vector3.Distance(W.bossRoot.transform.position, W.generator.position) > B.generatorRadius)
            { director.Hud.ShowBroadcastNotification("Sakelar siap. Pancing wujud dengan benang kontinu ke lingkar sorot generator.", 4); return false; }
            generatorReady = director.Elapsed + B.generatorCooldown; OpenWindow(); director.soundscape.Cue("Generator", W.generator.position); return true;
        }
        private void OpenWindow()
        {
            exposureUntil = director.Elapsed + B.exposureSeconds; nextCoreRetry = exposureUntil + B.coreRetry;
            lungeStarted = -1; director.soundscape.Cue("Shield", W.bossHead.position);
            director.Hud.EnqueueSubtitle("MataBatin", "PERISAI TERBUKA — satu Blitz pada inti!", B.exposureSeconds);
        }
        public bool Capture()
        {
            if (!Active) return false;
            if (!Exposed) { director.Hud.ShowBroadcastNotification("Perisai masih utuh. Buka melalui alat arena; foto biasa tidak memecahkan inti.", 3); return true; }
            var phone = director.Phone;
            bool hit = (Phase == 2 ? phone.isSelfieCamera : !phone.isSelfieCamera) && director.Aim(Phase == 3 ? W.bossChest : W.bossHead, 12, 28, W.bossRoot.transform);
            float before = phone.batteryPercent;
            if (!phone.TryCaptureEnergy(true)) { director.Hud.ShowBroadcastNotification("Blitz membutuhkan 15% dan cooldown 6 detik. Dinamo ada di sisi panggung.", 4); return true; }
            float after = phone.batteryPercent;
            bool committed = Phase5Campaign.RecordShot(director.Campaign, before, after, hit ? Phase : 0);
            phone.RestoreBattery(director.Campaign.Data.battery);
            if (!committed) { phone.RestoreBattery(before); director.SaveError(); exposureUntil = director.Elapsed + B.exposureSeconds; return true; }
            director.phoneView.CaptureFlash(); exposureUntil = 0;
            director.soundscape.Cue(hit ? "Core" : "Miss", W.bossHead.position);
            if (hit) { director.Hud.ShowBroadcastNotification("INTI " + Phase + " PECAH — CHECKPOINT TERSIMPAN", 5); director.RefreshWorld(); Restore(false); }
            else { director.Hud.ShowBroadcastNotification("Bidikan meleset. Perisai kembali; kesempatan bisa diulang dan dinamo tetap tersedia.", 5); nextCoreRetry = director.Elapsed + B.coreRetry; }
            return true;
        }
        public bool TakeLantern(int index)
        {
            if (index < 0 || index >= 3 || carriedLantern >= 0 || director.Elapsed < lanternReady[index])
            { director.Hud.ShowBroadcastNotification("Lentera belum siap atau masih dibawa. Tiang pulih setelah 15 detik.", 3); return false; }
            carriedLantern = index; W.lanternVisuals[index].transform.SetParent(P,false);W.lanternVisuals[index].transform.localPosition=new Vector3(.25f,.2f,-.05f); director.Hud.ShowBroadcastNotification("LENTERA PANGGUNG DIBAWA • " + GameInputConfig.Instance.GetActionLabel(GameAction.UseFlare) + " untuk menjatuhkan. Gratis dan dapat dipakai ulang.", 5); return true;
        }
        public bool DropLight()
        {
            if (director.Elapsed < lightUntil) return false;
            bool flare = director.Campaign.Data.inventory.Any(Phase5Ids.IsFlare);
            if (!flare && carriedLantern < 0)
            {
                for (int i = 0; i < W.lanternPosts.Length; i++)
                    if (Vector3.Distance(P.position, W.lanternPosts[i].position) < 2.6f && director.Elapsed >= lanternReady[i]) { carriedLantern = i; break; }
            }
            if (flare)
            { if (!Phase5Campaign.UseFlare(director.Campaign)) { director.SaveError(); return false; } }
            else if (carriedLantern >= 0) { lanternReady[carriedLantern] = director.Elapsed + B.lanternCooldown; W.lanternVisuals[carriedLantern].transform.SetParent(director.world.transform,true);W.lanternVisuals[carriedLantern].transform.position=P.position+Vector3.up*.05f;carriedLantern = -1; }
            else { director.Hud.ShowBroadcastNotification("Flare habis. Ambil Lentera Panggung di salah satu dari tiga tiang.", 4); return false; }
            lightPosition = P.position; lightUntil = director.Elapsed + B.flareDuration;
            W.droppedLight.enabled = true; director.soundscape.Cue("Light", lightPosition); return true;
        }
        private void ContactCheck()
        {
            if (Vector3.Distance(P.position, W.bossRoot.transform.position) > 1.5f || director.Elapsed < contactGrace || !director.ClearLine(W.bossHead.position, P.position + Vector3.up * .65f, W.bossRoot.transform)) { lungeStarted = -1; return; }
            if (lungeStarted < 0) { lungeStarted = director.Elapsed; director.Hud.EnqueueSubtitle("Penonton_Terakhir", "[Lensa mengatup. Menjauh dari jangkauan tangannya!]", 3); }
            if (director.Elapsed - lungeStarted >= B.lungeTelegraph) Contact();
        }
        private void Contact()
        {
            director.Penalty(B.contactSanityLoss, "Terkaman Sang Pemilik Layar. Putus jarak dan berlindung.");
            if (director.Elapsed - lastContact < B.contactWindow) director.Retry("Dua terkaman dalam satu perburuan.");
            lastContact = director.Elapsed; contactGrace = director.Elapsed + B.contactIFrame; lungeStarted = -1;
        }
        private void UpdateThreads()
        {
            if (threadMeshes == null) return;
            for (int i = 0; i < threadMeshes.Length; i++)
            {
                var t = threadMeshes[i]; if (!t) continue;
                t.gameObject.SetActive(Phase == 1 && director.Phone.isScreenOn && director.Phone.currentScanMode == LayarTerakhir.Smartphone.ScanVisualMode.UV);
                Vector3 from = (i == 0 ? W.bossHead.position : W.clones[(i - 1) % W.clones.Length].transform.position + Vector3.up * 2.8f);
                Vector3 to = W.damarFocus.position;
                t.position = from; t.rotation = Quaternion.LookRotation(to - from);
                t.localScale = new Vector3(.035f, .035f, Vector3.Distance(from, to));
            }
        }
    }
}
