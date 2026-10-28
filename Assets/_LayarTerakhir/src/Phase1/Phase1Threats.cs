using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Phase1
{
    public enum KuntiState { Dormant, Manifested, Pursuit, Resolved }
    public sealed class Phase1Threats : MonoBehaviour
    {
        public Phase1Director director;
        public KuntiState Kunti { get; private set; }
        public bool KuntiPursuing => Kunti == KuntiState.Pursuit;
        public bool KuntiManifested => Kunti == KuntiState.Manifested || KuntiPursuing;
        public bool FreezeKunti { get; set; }
        public bool IsChasing { get; private set; }
        public bool Recovering => director.BossActive && recovery > 0;
        public bool Exposed => activeLight >= 0 && exposedRemaining > 0;
        public int SealHits { get; private set; }
        public string BossStatus => Exposed ? $"PERISAI PECAH — BIDIK KEPALA {exposedRemaining:0.0}s" : Recovering ? "RECOVERY — TITIK DAYA AMAN" : $"SEGEL {SealHits}/3 — PANCING KE HALOGEN";
        private float unwatched, stun, reactionGrace, sensorTick, searchTime, recovery, attackTimer, exposedRemaining, kuntiStun;
        private float bathroomTimer = -1, tuyulTime = -1, nextTuyul, lastFoot;
        private bool tuyulBoss, bathroomReached;
        private int patrolIndex, activeLight = -1;
        private readonly bool[] lightsCompleted = new bool[3];
        private readonly float[] lampCooldown = new float[3];
        private Vector3 lastKnown;
        private Phase1Navigator navigation;
        private Phase1World World => director.world;
        private Phase1Balance Balance => director.balance;
        private SmartphoneController Phone => director.Phone;
        private Vector3 PlayerPosition => director.player.transform.position;
        public void Restore()
        {
            Kunti = director.Completed("P1_CUT_CHAIN") ? KuntiState.Resolved : KuntiState.Dormant;
            FreezeKunti = IsChasing = false; unwatched = stun = searchTime = 0;
            SealHits = 0; activeLight = -1; exposedRemaining = 0;
            for (int i = 0; i < 3; i++) { lightsCompleted[i] = false; lampCooldown[i] = 0; World.halogenLights[i].enabled = false; }
            World.kuntilanak.SetActive(false); World.tuyul.SetActive(false); tuyulTime = -1; bathroomTimer = -1;
            World.genderuwo.transform.position = World.genderuwoSpawn.position;
            World.genderuwo.SetActive(director.Completed("P1_GENDER_MANIFEST") && !director.Completed("P1_BOSS_WIN"));
            HudController.Instance.SetThreatBar(null);
            navigation = GetComponent<Phase1Navigator>();
            if (!navigation) navigation = gameObject.AddComponent<Phase1Navigator>();
            navigation.Configure(World.patrolPoints, World.genderuwo.transform,director.player.transform);
            patrolIndex = 0; reactionGrace = 6; attackTimer = 8;
        }
        public void BeginHaunting()
        {
            // The cry arrives before any visual: the L3 beat starts as audio, never a warning label.
            if (Kunti != KuntiState.Dormant || !World.kuntilanak) return;
            director.soundscape?.Play("Kunti", World.kuntilanakSpawn.position);
        }
        public void ManifestKunti()
        {
            if (Kunti != KuntiState.Dormant) return;
            Kunti = KuntiState.Manifested;
            // Look-back reveal: she stands behind the player's current heading, inside the corridor.
            Vector3 flat = director.player.transform.forward; flat.y = 0;
            if (flat.sqrMagnitude < .01f) flat = Vector3.back;
            flat.Normalize();
            Vector3 behind = director.player.transform.position - flat * 8;
            behind.x = Mathf.Clamp(behind.x, -19, 19); behind.y = World.kuntilanakSpawn.position.y; behind.z = -11;
            World.kuntilanak.transform.position = behind;
            director.Commit("P1_KUNTI_SAWER",data=>Phase1Ids.Donation(data,"P1_KUNTI_SAWER",3000000,true));
            World.kuntilanak.SetActive(true);
            Motion(World.kuntilanak,"Idle");
            // Raka hurriedly raises the camera toward her; the player still turns on their own.
            var phone = director.Phone;
            phone.isPhoneRaised = true; phone.isScreenOn = phone.batteryPercent > 0; phone.currentScanMode = ScanVisualMode.Normal; phone.SyncVisualState();
            director.phoneView.Apps.Open(LayarTerakhir.UI.PhoneApp.Camera);
            HudController.Instance.EnqueueSubtitle("Raka", "...di belakang gue. Jangan bergerak...", 4);
            HudController.Instance.ShowBroadcastNotification("Tahan kamera ke arahnya.", 5);
            director.soundscape?.Play("Manifest", behind);
        }
        public void BeginKuntiPursuit() { Kunti = KuntiState.Pursuit; unwatched = 0; }
        public void ResolveKunti()
        {             Kunti = KuntiState.Resolved; FreezeKunti = false; World.kuntilanak.SetActive(false); director.soundscape?.StopKunti(); kuntiStun = 0; }
        public bool CheckKuntiCatch()
        {
            if (!KuntiPursuing || FreezeKunti || kuntiStun > 0) return false;
            if (Vector3.Distance(World.kuntilanak.transform.position, PlayerPosition) > Balance.kuntilanakCatchRadius) return false;
            if (!director.ClearLine(World.kuntilanak.transform.position+Vector3.up,PlayerPosition+Vector3.up)) return false;
            director.Die("Kuntilanak sudah menjangkau Raka."); return true;
        }
        public void ManifestGender()
        { World.genderuwo.SetActive(true); reactionGrace = 5; director.soundscape?.Play("Manifest", World.genderuwo.transform.position); }
        public void BeginBathroomAmbush()
        {
            if (director.Completed("P1_EVP_AMBUSH_DONE") || bathroomTimer >= 0) return;
            bathroomTimer = 0; bathroomReached = false;
            director.soundscape?.Play("Impact", World.evpPoint.position);
            HudController.Instance.ShowBroadcastNotification("PINTU KAMAR MANDI DIDOBRAK! Sembunyi di bilik dan matikan layar HP.", 5);
        }
        public void BeginBoss()
        {
            World.genderuwo.SetActive(true); reactionGrace = 2; recovery = Balance.bossRecoverySeconds;
            attackTimer = 4; nextTuyul = 25; navigation.Invalidate();
            director.soundscape?.Play("Manifest", World.genderuwo.transform.position);
        }
        private void Update()
        {
            if (!director.Ready || director.Dead || director.Busy || UiFocus.HardModal || UiFocus.Cinematic || HudController.Instance.IsModalOpen) return;
            float dt = Time.deltaTime;
            director.phoneView.ShowChapterTarget(World.genderuwo.transform.position+Vector3.up*2.13f,Exposed);
            TickKunti(dt); TickTuyul(dt);
            if (!World.genderuwo.activeSelf) return;
            reactionGrace = Mathf.Max(0, reactionGrace - dt); stun = Mathf.Max(0, stun - dt);
            if (director.BossActive) TickBoss(dt); else TickStealth(dt);
            director.soundscape?.SetThreat(World.genderuwo.transform.position, IsChasing || director.BossActive, stun > 0);
        }
        private void TickKunti(float dt)
        {
            if (!KuntiManifested) return;
            Vector3 pos = World.kuntilanak.transform.position;
            Vector3 face = PlayerPosition - pos; face.y = 0;
            if (face.sqrMagnitude > .01f) World.kuntilanak.transform.rotation = Quaternion.Slerp(World.kuntilanak.transform.rotation, Quaternion.LookRotation(face), dt * 6);
            director.soundscape?.SetKuntiDistance(Vector3.Distance(pos, PlayerPosition));
            kuntiStun = Mathf.Max(0, kuntiStun - dt);
            if (kuntiStun > 0) { Motion(World.kuntilanak, "Stun"); return; }
            if (!KuntiPursuing || FreezeKunti) return;
            if (CheckKuntiCatch()) return;
            bool watched = director.CameraCanScan &&
                (director.Aim(pos + Vector3.up * 1.55f, 45, .05f) || director.Aim(pos + Vector3.up * 1.1f, 45, .05f));
            unwatched = watched ? 0 : unwatched + dt;
            if (unwatched > Balance.unwatchedGrace)
            { director.Die("Pandangan kamera terputus terlalu lama. Mundur sambil pantau Kuntilanak."); return; }
            pos.x = Mathf.MoveTowards(pos.x, PlayerPosition.x, Balance.kuntilanakSpeed * dt);
            World.kuntilanak.transform.position = pos;
            Motion(World.kuntilanak, "Walk",Balance.kuntilanakSpeed/1.7f);
        }
        private void TickStealth(float dt)
        {
            if (stun > 0) { Motion(World.genderuwo, "Stun"); return; }
            if (director.Hidden)
            {
                float distance = World.genderuwo.activeSelf ? Vector3.Distance(World.genderuwo.transform.position, PlayerPosition) : 99f;
                HudController.Instance.SetThreatBar(LayarTerakhir.Core.HidingAwareness.ProximityFill(IsChasing, distance, 7f, 18f));
            }
            else HudController.Instance.SetThreatBar(null);
            if (bathroomTimer >= 0)
            {
                bathroomTimer += dt;
                if (bathroomTimer < Balance.bathroomGraceSeconds) { Motion(World.genderuwo, "Windup"); return; }
                if (!bathroomReached)
                {
                    MoveGender(World.Anchor("BATH_ENTRY").position, Balance.chaseSpeed, dt);
                    bathroomReached = Vector3.Distance(World.genderuwo.transform.position, World.Anchor("BATH_ENTRY").position) < 1;
                    if (!bathroomReached) return;
                }
                if (director.Hidden && Phone.isPhoneRaised && Phone.isScreenOn && Phone.batteryPercent > 0)
                { director.Die("Cahaya layar membocorkan bilik persembunyian."); return; }
                if (!director.Hidden && Vector3.Distance(PlayerPosition, World.evpPoint.position) < 4 && reactionGrace <= 0)
                {
                    if(director.ClearLine(World.genderuwo.transform.position+Vector3.up,PlayerPosition+Vector3.up))DetectPlayer();
                }
                if(IsChasing&&!director.Hidden)
                {
                    MoveGender(lastKnown,Balance.chaseSpeed,dt);
                    if(ContactReachesPlayer()) { director.Die("Genderuwo menangkap Raka di kamar mandi.");return; }
                }
                if (bathroomTimer > Balance.bathroomGraceSeconds + Balance.bathroomSearchSeconds)
                { if (director.Commit("P1_EVP_AMBUSH_DONE")) { bathroomTimer = -1; IsChasing = false; searchTime = 0; patrolIndex = 2; } }
                return;
            }
            sensorTick -= dt;
            if (sensorTick <= 0)
            {
                sensorTick = .15f;
                Vector3 eye = World.genderuwo.transform.position + Vector3.up * 2.1f;
                Vector3 target = PlayerPosition + Vector3.up;
                float distance = Vector3.Distance(eye, target);
                bool screen = Phone.isPhoneRaised && Phone.isScreenOn && Phone.batteryPercent > 0;
                float range = screen ? 13 : director.player.IsSprinting ? 9 : 6;
                bool inFront = Vector3.Angle(World.genderuwo.transform.forward, target - eye) < 58;
                if (reactionGrace <= 0 && distance < range && director.ClearLine(eye, target) &&
                    (!director.Hidden || screen) && (inFront || screen && distance < 9 || director.player.IsSprinting && distance < 6)) DetectPlayer();
            }
            if (IsChasing)
            {
                searchTime -= dt;
                if (director.Hidden && (!Phone.isScreenOn || !Phone.isPhoneRaised)) searchTime = Mathf.Min(searchTime, 2.5f);
                MoveGender(lastKnown, Balance.chaseSpeed, dt);
                if (ContactReachesPlayer())
                { director.Die("Genderuwo menangkap Raka. Matikan layar dan putuskan garis pandangnya."); return; }
                if (searchTime <= 0) IsChasing = false;
            }
            else
            {
                Vector3 target = World.patrolPoints[patrolIndex].position;
                MoveGender(target, Balance.patrolSpeed, dt);
                if (Vector3.Distance(World.genderuwo.transform.position, target) < .75f) patrolIndex = (patrolIndex + 1) % Mathf.Min(8, World.patrolPoints.Length);
            }
        }
        private bool ContactReachesPlayer()=>reactionGrace<=0&&!director.Hidden&&
            Vector3.Distance(World.genderuwo.transform.position,PlayerPosition)<1.25f&&
            director.ClearLine(World.genderuwo.transform.position+Vector3.up,PlayerPosition+Vector3.up);
        private void DetectPlayer() { IsChasing = true; lastKnown = PlayerPosition; searchTime = 6; }
        private void MoveGender(Vector3 target, float speed, float dt)
        {
            Vector3 before = World.genderuwo.transform.position;
            navigation.Step(target, speed * dt);
            Vector3 delta = World.genderuwo.transform.position - before; delta.y = 0;
            if (delta.sqrMagnitude > .00001f)
                World.genderuwo.transform.rotation = Quaternion.Slerp(World.genderuwo.transform.rotation, Quaternion.LookRotation(delta), dt * 7);
            Motion(World.genderuwo, delta.sqrMagnitude < .00001f ? "Idle" : speed > 2 ? "Run" : "Walk",delta.sqrMagnitude < .00001f?1:speed/(speed>2?5.8f:2.35f));
            if (Time.time - lastFoot > (speed > 2 ? .43f : .75f) && delta.sqrMagnitude > .00001f)
            { lastFoot = Time.time; director.soundscape?.Play("Foot", World.genderuwo.transform.position); }
        }
        public bool TryCapture()
        {
            if (tuyulTime >= 0 && Phone.currentScanMode == ScanVisualMode.AR && director.Aim(World.tuyul.transform.position + Vector3.up * .6f, 7, .12f))
            {
                if (!Phone.TryCaptureEnergy(true)) { InsufficientFlash(); return true; }
                director.phoneView.CaptureFlash(); FinishTuyul(false); return true;
            }
            // A close-range photograph startles her briefly: a fair window to back away.
            if (KuntiManifested && kuntiStun <= 0 && director.Aim(World.kuntilanak.transform.position + Vector3.up * 1.5f, Balance.kuntiPhotoRangeM, .18f))
            {
                if (!Phone.TryCaptureEnergy(false)) { InsufficientFlash(); return true; }
                director.phoneView.CaptureFlash();
                kuntiStun = Balance.kuntiPhotoStunSeconds;
                Motion(World.kuntilanak, "Stun");
                HudController.Instance.ShowBroadcastNotification("Dia terkejut — mundur sekarang.", 2);
                return true;
            }
            if (!World.genderuwo.activeSelf || !director.Aim(World.genderuwo.transform.position + Vector3.up * 2.13f, Balance.flashRange, .22f)) return false;
            if (!Phone.TryCaptureEnergy(true)) { InsufficientFlash(); return true; }
            director.phoneView.CaptureFlash();
            if (!director.BossActive) { stun = Balance.emergencyStunSeconds; IsChasing = false; return true; }
            if (!Exposed || lightsCompleted[activeLight]) { HudController.Instance.ShowBroadcastNotification("PERISAI UTUH — pancing ke halogen dahulu.", 3); return true; }
            lightsCompleted[activeLight] = true; World.halogenLights[activeLight].enabled = false;
            director.soundscape?.Play("Glass", World.halogenLights[activeLight].transform.position);
            activeLight = -1; exposedRemaining = 0; SealHits++; recovery = Balance.bossRecoverySeconds;
            if (SealHits == 3) director.CommitBossVictory();
            return true;
        }
        private void InsufficientFlash() => HudController.Instance.ShowBroadcastNotification(Phone.BlitzCooldownRemaining > 0 ? $"Blitz pulih dalam {Phone.BlitzCooldownRemaining:0.0}s." : "Blitz memerlukan 15% daya. Gunakan powerbank atau titik daya.", 3);
        public void UseHalogen(string id)
        {
            if (!director.BossActive || Exposed) return;
            int index = id == "HALOGEN_N" ? 0 : id == "HALOGEN_E" ? 1 : 2;
            if (lightsCompleted[index] || lampCooldown[index] > 0) { HudController.Instance.ShowBroadcastNotification("Halogen belum tersedia.", 2); return; }
            Vector3 beam = World.halogenTargets[index].position;
            Vector3 delta = World.genderuwo.transform.position - beam; delta.y = 0;
            if (delta.magnitude > Balance.beamRadius || !director.ClearLine(World.halogenLights[index].transform.position, World.genderuwo.transform.position + Vector3.up * 1.4f))
            { HudController.Instance.ShowBroadcastNotification("Pancing tubuh Genderuwo ke kerucut sorotan, lalu tarik tuas.", 3); return; }
            activeLight = index; exposedRemaining = Balance.exposedSeconds; World.halogenLights[index].enabled = true;
            director.soundscape?.Play("Halogen", beam); Motion(World.genderuwo, "Stun");
        }
        private void TickBoss(float dt)
        {
            for (int i = 0; i < 3; i++) lampCooldown[i] = Mathf.Max(0, lampCooldown[i] - dt);
            if (Exposed)
            {
                exposedRemaining -= dt;
                if (exposedRemaining <= 0) { World.halogenLights[activeLight].enabled = false; lampCooldown[activeLight] = 3; activeLight = -1; recovery = 2; }
                return; // No projectiles, movement, or Tuyul drain during the five-second opening.
            }
            if (recovery > 0) { recovery -= dt; Motion(World.genderuwo, "Stun"); return; }
            attackTimer -= dt; nextTuyul -= dt;
            if (attackTimer <= 0)
            { attackTimer = 8; StartCoroutine(ThrowDebris()); return; }
            if (nextTuyul <= 0 && tuyulTime < 0) { nextTuyul = 30; BeginTuyul(true); }
            float distance = Vector3.Distance(World.genderuwo.transform.position, PlayerPosition);
            MoveGender(PlayerPosition, Balance.chaseSpeed, dt);
            if (reactionGrace <= 0 && distance < 1.25f && director.ClearLine(World.genderuwo.transform.position + Vector3.up, PlayerPosition + Vector3.up))
                director.Die("Genderuwo menangkap Raka di arena. Jaga jarak dan gunakan koridor loop.");
        }
        private IEnumerator ThrowDebris()
        {
            Vector3 aimed = PlayerPosition + Vector3.up;
            recovery = 1.5f; Motion(World.genderuwo, "Windup");
            director.soundscape?.Play("Windup", World.genderuwo.transform.position);
            yield return new WaitForSeconds(1.5f);
            if (!director.BossActive || director.Dead || director.Busy || Exposed) yield break;
            var debris = Instantiate(World.Anchor("DEBRIS_TEMPLATE").gameObject);
            debris.name = "TelegraphedConcreteDebris"; debris.SetActive(true);
            debris.transform.position = World.genderuwo.transform.position + Vector3.up * 1.9f;
            var projectile = debris.AddComponent<Phase1Projectile>(); projectile.Initialize(director, aimed);
            recovery = Balance.bossRecoverySeconds;
        }
        public void BeginTuyul(bool boss)
        {
            if (tuyulTime >= 0 || director.Completed("P1_TUYUL_L2") && !boss) return;
            tuyulBoss = boss; tuyulTime = 0;
            Vector3 pos = PlayerPosition + director.player.transform.forward * 3 + director.player.transform.right * .7f;
            World.tuyul.transform.position = pos; World.tuyul.SetActive(true);
            Motion(World.tuyul,"Idle");
            director.soundscape?.Play("Tuyul", pos);
            HudController.Instance.ShowBroadcastNotification("Baterai turun! Cekikikan dari dekat kaki. Cari lewat AR, lalu Blitz.", 5);
        }
        private void TickTuyul(float dt)
        {
            if (tuyulTime < 0 || Exposed) return;
            tuyulTime += dt; Phone.ConsumeBattery(Balance.tuyulDrainPerSecond * dt);
            if (tuyulTime >= Balance.tuyulSeconds) FinishTuyul(true);
        }
        private void FinishTuyul(bool steal)
        {
            string powerbank = steal ? director.Campaign.Data.inventory.Find(id => id.StartsWith("P1_POWERBANK_") || id == "POWERBANK_BESAR" || id == "POWERBANK_KECIL_KOS") : null;
            if (!tuyulBoss && !director.Commit("P1_TUYUL_L2", d => { if (powerbank != null) d.inventory.Remove(powerbank); }))
            { tuyulTime = Balance.tuyulSeconds; return; }
            if (tuyulBoss && powerbank != null) director.Campaign.Data.inventory.Remove(powerbank);
            World.tuyul.SetActive(false); tuyulTime = -1;
        }
        private static void Motion(GameObject actor, string clip,float playback=1)
        {
            var animation = actor.GetComponentInChildren<Animation>();
            if (animation && animation.GetClip(clip))
            {animation[clip].speed=Mathf.Clamp(playback,.35f,1.5f);if(!animation.IsPlaying(clip))animation.CrossFade(clip,.2f);}
        }
    }

}
