using System;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public sealed class AtmajaBasementHorror : MonoBehaviour
    {
        public const string PowerbankId = "POWERBANK_BESAR", HandycamId = "ITEM_P0_HANDYCAM";
        private enum Mind { Dormant, Emerge, Patrol, Suspicion, Pursue, Done }
        public AtmajaDirector director;
        public AtmajaUpperDirector upper;
        public AtmajaChaseSequence chase;
        public Transform spawnBasement;
        public Transform[] waypoints = new Transform[0];
        public Transform inspectAnchor1, inspectAnchor2;
        public GameObject nyonya, shearsProp, ragOriginal, teplokFlame;
        public Light teplokLight;
        public AudioSource humSource;
        public Transform hideAnchor1, hideAnchor2, exitAnchor1, exitAnchor2;
        public GameObject powerbankObj, handycamObj;
        public Collider wardrobeCol1, wardrobeCol2;
        public Transform powerbankSpot, handycamSpot;
        public bool IsHidden => hiddenIn != null;
        public bool InspectingLocker => lockerCheckRunning;
        public float Detection => detect;
        public bool IsHunting => mind == Mind.Pursue;
        public bool IsBlitzStunned => Time.time < stunnedUntil;
        private Mind mind;
        private int waypointIndex;
        private float waypointPause, detect, suspicionTimer, giveUpTimer, dragTimer, hideEnteredAt, inspectElapsed, stunnedUntil;
        private AudioSource nyonyaSource;
        private AudioLowPassFilter nyonyaFilter;
        private Vector3 suspicionPoint, lastPosition;
        private bool lockerCheckRunning, lockerCleared, hiddenWasSeen;
        private float nextHideTime;
        private Transform hiddenIn, hiddenAnchor;
        private ParticleSystem breath;
        private AudioClip scrapeClip;
        private float[,] connections;
        private CampaignState Campaign => CampaignState.Instance;
        private SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        private bool Has(string id) => Campaign && Campaign.HasInventoryItem(id);
        private string Key(GameAction action) => GameInputConfig.Instance.GetActionLabel(action);
        private bool Commit(string id, Action change = null)
        {
            if (Has(id)) return true;
            string before = JsonUtility.ToJson(Campaign.Data);
            Campaign.AddInventoryItem(id); change?.Invoke();
            if (Campaign.SaveCheckpointAtomic("PROLOG_BASEMENT_ENTRY")) return true;
            JsonUtility.FromJsonOverwrite(before, Campaign.Data);
            Phone.batteryPercent = Campaign.Data.battery; Phone.SyncVisualState();
            Hud.ShowBroadcastNotification("Checkpoint gagal disimpan. Coba interaksi kembali.", 4);
            return false;
        }
        private void Ui(AudioClip clip, float gain) { if (director.notificationSource && clip) director.notificationSource.PlayOneShot(clip, gain); }
        private void Sfx(AudioClip clip, Vector3 where, float gain)
        { if (!director.foleySource || !clip) return; director.foleySource.transform.position = where; director.foleySource.PlayOneShot(clip, gain); }
        private void Start()
        {
            if(!nyonya)return;
            var go=new GameObject("NyonyaVoice",typeof(AudioSource),typeof(AudioLowPassFilter));
            go.transform.SetParent(nyonya.transform,false);
            nyonyaSource=go.GetComponent<AudioSource>();nyonyaSource.playOnAwake=false;
            nyonyaSource.spatialBlend=1;nyonyaSource.minDistance=1;nyonyaSource.maxDistance=12;
            nyonyaSource.rolloffMode=AudioRolloffMode.Linear;nyonyaSource.dopplerLevel=0;
            if(director.audioProfile)nyonyaSource.outputAudioMixerGroup=director.audioProfile.effectsBus;
            nyonyaFilter=go.GetComponent<AudioLowPassFilter>();nyonyaFilter.cutoffFrequency=22000;
        }
        private void PlayNyonya(AudioClip clip,float gain)
        {
            if(!clip)return;
            if(!nyonyaSource)Start();
            if(nyonyaSource){nyonyaSource.transform.position=nyonya.transform.position;nyonyaSource.PlayOneShot(clip,gain);}
            else Sfx(clip,nyonya.transform.position,gain);
        }
        // Only loading/retrying resets the hunter. Equipment pickups never teleport her.
        public void RestoreWorld()
        {
            StopAllCoroutines();
            bool entered = Has("EVENT_P0_BASEMENT_ENTERED"), chased = Has("EVENT_P0_CHASE_STARTED");
            hiddenIn = hiddenAnchor = null; lockerCheckRunning = lockerCleared = false;
            mind = !entered ? Mind.Dormant : chased ? Mind.Done : Mind.Patrol;
            waypointIndex = 0; detect = 0; waypointPause = 1.4f; connections = null;
            if (nyonya) nyonya.SetActive(entered && !chased);
            if (ragOriginal) ragOriginal.SetActive(!chased);
            if (teplokFlame) teplokFlame.SetActive(!entered);
            if (teplokLight) teplokLight.enabled = !entered;
            if (powerbankObj) powerbankObj.SetActive(entered && !chased);
            if (handycamObj) handycamObj.SetActive(entered && !chased);
            if (humSource) { humSource.enabled = entered && !chased; if (humSource.enabled) humSource.Play(); }
            if (wardrobeCol1) wardrobeCol1.enabled = true;
            if (wardrobeCol2) wardrobeCol2.enabled = true;
            var rig = director.player.GetComponent<ShoulderCameraRig>(); if (rig) rig.FirstPersonOverride = false;
            director.player.MovementLocked = false; lastPosition = director.player.transform.position; Breath(false);
            if (nyonya && waypoints.Length > 0) nyonya.transform.SetPositionAndRotation(waypoints[0].position, waypoints[0].rotation);
        }
        public string Objective()
        {
            if (IsHidden) return lockerCheckRunning ? $"Matikan layar [{Key(GameAction.ToggleScreen)}]. Jangan bergerak." : $"Dengarkan langkahnya. [{Key(GameAction.Interact)}] keluar lemari.";
            if (!Has("EVENT_P0_POWERBANK_TAKEN")) return "Temukan powerbank Damar di ruang jahit. Gunakan rak dan lemari untuk bersembunyi.";
            if (!Has(HandycamId)) return "Cari jejak Damar di dekat cermin. Merunduk di balik meja saat dia berpatroli.";
            return "Putar rekaman terakhir di handycam Damar.";
        }
        public void Handle(AtmajaItem item)
        {
            if (director.Stage < AtmajaStage.BasementEntry || director.Stage >= AtmajaStage.Chase) return;
            if (item.action == AtmajaAction.Powerbank)
            {
                if (Has("EVENT_P0_POWERBANK_TAKEN"))
                {
                    if (Phone.batteryPercent < 35) StartCoroutine(Recharge());
                    else Hud.ShowBroadcastNotification("Powerbank Damar masih tersambung. Daya HP cukup untuk melanjutkan.", 3);
                    return;
                }
                if (!Commit("EVENT_P0_POWERBANK_TAKEN", () => {
                    float reserve = 0;
                    foreach (string id in Campaign.Data.inventory)
                        if (id.StartsWith("RESERVE_BATTERY_", StringComparison.Ordinal) && float.TryParse(id.Substring(16), out float saved)) reserve = Mathf.Max(reserve, saved);
                    Campaign.Data.battery = Mathf.Max(80, reserve);
                    Campaign.Data.inventory.RemoveAll(id => id.StartsWith("RESERVE_BATTERY_", StringComparison.Ordinal));
                })) return;
                Phone.batteryPercent = Campaign.Data.battery; Phone.isScreenOn = true; Phone.SyncVisualState();
                Sfx(director.audioProfile.unlock, item.transform.position, .5f);
                Hud.ShowBroadcastNotification($"Powerbank Damar tersambung • Baterai {Phone.batteryPercent:0}% • Cadangan dipulihkan.", 5);
                Hud.EnqueueSubtitle("Raka", "80 persen... Oke, masih bisa lanjut.", 3);
                director.phoneView.AddChat("@rudi_tabuti", "ALHAMDULILLAH IDUP LAGI BANG"); director.Objective();
            }
            else if (item.action == AtmajaAction.Handycam)
            {
                if (!Has("EVENT_P0_POWERBANK_TAKEN")) { Hud.ShowBroadcastNotification("HP-nya hampir mati. Cari daya cadangan Damar dulu.", 4); return; }
                if (Phone.batteryPercent < Phone.blitzCostPercent + 3)
                { Hud.ShowBroadcastNotification("Daya terlalu rendah. Sambungkan lagi ke powerbank di meja jahit.", 4); return; }
                // The physical interaction remains available if the game closed during playback.
                if (Commit(HandycamId)) chase.PlayHandycamVideo();
            }
            else if (item.action == AtmajaAction.WardrobeHide && !IsHidden && Time.time >= nextHideTime) EnterWardrobe(item.transform);
        }
        private IEnumerator Recharge()
        {
            director.SetBusy(true); director.player.MovementLocked = true;
            float start = Phone.batteryPercent; Hud.ShowBroadcastNotification("Mengisi daya dari kabel powerbank Damar...", 3);
            for (float t = 0; t < 2.5f; t += Time.deltaTime) { Phone.batteryPercent = Mathf.Lerp(start, 35, t / 2.5f); yield return null; }
            Phone.batteryPercent = Campaign.Data.battery = 35; Phone.isScreenOn = true; Phone.SyncVisualState();
            Campaign.SaveCheckpointAtomic("PROLOG_BASEMENT_ENTRY"); director.player.MovementLocked = false; director.SetBusy(false);
        }
        private void Breath(bool on)
        {
            if (on && !breath)
            {
                var cam = director.player.GetComponent<InteractionInteractor>().playerCamera;
                var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit"); if (!cam || !shader) return;
                var go = new GameObject("BreathVapor", typeof(ParticleSystem)); go.transform.SetParent(cam.transform, false);
                breath = go.GetComponent<ParticleSystem>(); breath.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                var main = breath.main; main.loop = true; main.startLifetime = 1.1f; main.startSpeed = .1f;
                main.startSize = new ParticleSystem.MinMaxCurve(.02f, .045f); main.startColor = new Color(1, 1, 1, .1f);
                main.simulationSpace = ParticleSystemSimulationSpace.Local; main.maxParticles = 24; main.gravityModifier = -.012f;
                var emission = breath.emission; emission.rateOverTime = 2.4f;
                var shape = breath.shape; shape.shapeType = ParticleSystemShapeType.Cone; shape.angle = 16; shape.radius = .02f; shape.rotation = new Vector3(118, 0, 0);
                var renderer = go.GetComponent<ParticleSystemRenderer>(); renderer.material = new Material(shader);
                renderer.material.SetColor("_BaseColor", new Color(1, 1, 1, .1f)); renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; renderer.receiveShadows = false;
                breath.Play();
            }
            if (breath) { var emission = breath.emission; emission.enabled = on; }
        }
        private void EnterWardrobe(Transform wardrobe)
        {
            hiddenAnchor = Vector3.Distance(wardrobe.position, hideAnchor1.position) < Vector3.Distance(wardrobe.position, hideAnchor2.position) ? hideAnchor1 : hideAnchor2;
            hiddenWasSeen = mind == Mind.Pursue && CanSeePlayer(12);
            hiddenIn = wardrobe; hideEnteredAt = Time.time; lockerCleared = false; inspectElapsed = 0;
            var col = hiddenAnchor == hideAnchor1 ? wardrobeCol1 : wardrobeCol2; if (col) col.enabled = false;
            director.PlacePlayer(hiddenAnchor.position, hiddenAnchor.rotation); director.player.MovementLocked = true;
            director.player.cameraPivot.localRotation = Quaternion.identity;
            var rig = director.player.GetComponent<ShoulderCameraRig>(); if (rig) rig.FirstPersonOverride = true;
            Hud.SetInteractionPrompt(""); director.Objective();
        }
        public void ExitWardrobe()
        {
            if (!IsHidden) return;
            Transform exit = hiddenAnchor == hideAnchor1 ? exitAnchor1 : exitAnchor2; if (!exit) return;
            director.PlacePlayer(exit.position, exit.rotation);
            if (wardrobeCol1) wardrobeCol1.enabled = true; if (wardrobeCol2) wardrobeCol2.enabled = true;
            hiddenIn = hiddenAnchor = null; director.player.MovementLocked = false;
            lockerCheckRunning = false; hiddenWasSeen = false;
            // The interactor also sees this E press. Do not immediately enter again
            // when its Update runs after the wardrobe's exit handler.
            nextHideTime = Time.time + .75f;
            var rig = director.player.GetComponent<ShoulderCameraRig>(); if (rig) rig.FirstPersonOverride = false;
            lastPosition = director.player.transform.position; director.Objective();
        }
        private void Update()
        {
            if (!director || !director.player || !Campaign || !Hud || director.Busy || UiFocus.HardModal || Hud.IsModalOpen) return;
            if (IsHidden && Time.time - hideEnteredAt > .3f && GameInputConfig.Instance.GetActionDown(GameAction.Interact))
            { ExitWardrobe(); return; }
            if (director.Stage == AtmajaStage.MasterOpen && director.player.transform.position.y < -.5f)
            {
                if (!Commit("EVENT_P0_BASEMENT_ENTERED", () => {
                    if (!Has("EVENT_P0_POWER_GLITCH"))
                    {
                        Campaign.AddInventoryItem("EVENT_P0_POWER_GLITCH"); Campaign.AddInventoryItem("RESERVE_BATTERY_" + Mathf.RoundToInt(Phone.batteryPercent)); Campaign.Data.battery = 12;
                    }
                    if (Campaign.AddInventoryItem("EVENT_P0_BASEMENT_SAWER")) Campaign.AddBoundDonation(2000000);
                })) return;
                Phone.batteryPercent = Campaign.Data.battery; Phone.SyncVisualState(); RestoreWorld(); EnterBasementBeats(); return;
            }
            Breath(director.Stage == AtmajaStage.BasementEntry || director.Stage == AtmajaStage.BasementStealth);
            if(director.Stage>=AtmajaStage.Chase)return;
            if (mind == Mind.Dormant || mind == Mind.Done || mind == Mind.Emerge || !nyonya || !nyonya.activeSelf) return;
            float dt = Time.deltaTime; dragTimer -= dt;
            if (dragTimer <= 0) { dragTimer = mind == Mind.Pursue ? .35f : .95f; if (!scrapeClip) scrapeClip = AtmajaSynth.Scrape(.42f); PlayNyonya(scrapeClip, .3f); }
            if (nyonyaFilter) nyonyaFilter.cutoffFrequency=Mathf.Lerp(nyonyaFilter.cutoffFrequency,CanSeePlayer(14)?22000f:620f,1-Mathf.Exp(-8*dt));
            var pose = nyonya.GetComponent<AtmajaThreatPresentation>(); if (pose) pose.ExternalStun = IsBlitzStunned;
            if (IsBlitzStunned) return;
            if (humSource) humSource.volume = Mathf.Clamp01(1.2f - Vector3.Distance(nyonya.transform.position, director.player.transform.position) / 9) * .35f;
            if (!IsHidden&&mind==Mind.Pursue&&Phone&&Phone.isPhoneRaised&&Phone.isScreenOn&&director.phoneView&&director.phoneView.IsCameraApp
                &&GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz)&&TryEmergencyBlitz()) return;
            if (IsHidden) HiddenTick(dt);
            else { if (director.player.MovementLocked) return; SenseTick(dt); }
            if (!lockerCheckRunning)
            {
                switch (mind)
                {
                    case Mind.Patrol: PatrolTick(dt); break;
                    case Mind.Suspicion: StepToward(suspicionPoint, 1.35f, dt); suspicionTimer -= dt; if (suspicionTimer <= 0) ReturnToPatrol(); break;
                    case Mind.Pursue:
                        StepToward(IsHidden ? (hiddenAnchor == hideAnchor1 ? exitAnchor1 : exitAnchor2).position : director.player.transform.position, 2.8f, dt);
                        giveUpTimer -= dt; if (!IsHidden && !CanSeePlayer(12) && giveUpTimer <= 0) ReturnToPatrol(); break;
                }
            }
            if (!IsHidden && !IsBlitzStunned && Vector3.Distance(nyonya.transform.position, director.player.transform.position) < .85f && CanSeePlayer(2))
                director.FailToCheckpoint("PROLOG_BASEMENT_ENTRY", "Nyonya Sri Atmaja menemukanmu. Gunakan rak untuk memutus pandangannya.");
        }
        // Flash costs the same 15% battery as the chase QTE, but is available
        // whenever the basement hunter has actually spotted Raka.
        public bool TryEmergencyBlitz()
        {
            if (mind != Mind.Pursue || IsHidden || IsBlitzStunned) return false;
            if (!Phone || !Phone.isPhoneRaised || !Phone.isScreenOn || Phone.batteryPercent < Phone.blitzCostPercent) return false;
            if (!director.phoneView || !director.phoneView.IsCameraApp) return false;
            Phone.ConsumeBattery(Phone.blitzCostPercent);
            stunnedUntil = Time.time + 2f; detect = 0; giveUpTimer = 5;
            var pose = nyonya.GetComponent<AtmajaThreatPresentation>();
            if (pose) { pose.ExternalStun = true; if (pose.blitzSmoke) pose.blitzSmoke.Play(); }
            director.phoneView.CaptureFlash();
            Ui(AtmajaSynth.Tone(.7f, 900, 140, .35f), .5f);
            if(director.phoneView)director.phoneView.AddChatBurst(new[]{"@rizky_pratama_","@kopi_susu99","@bocil_kematian"},new[]{"BLITZ! KENA BANG!","LARI SEKARANG SELAGI KOSONG!","WOW KAMERANYA NYALA TERANG!"});
            Hud.ShowBroadcastNotification("BLITZ! Nyonya terpana 2 detik. Putus pandangan dan lari!", 3);
            return true;
        }
        private void EnterBasementBeats()
        {
            director.SetStage(AtmajaStage.BasementEntry); director.Objective();
            Hud.PlayPriorityDialogue("Raka", "Sial, ada gangguan daya! Gue harus cari powerbank cadangan di tas peralatan Damar atau HP ini mati total!", 6);
            director.phoneView.AddChat("@rudi_tabuti", "BANG BATERAINYA TINGGAL 12 PERSEN BANG!");
            director.phoneView.AddChat("@maya_sari", "Jangan mati dulu bang kameranya plis gue takut banget liat ruangan bawahnya!");
            Hud.ShowBroadcastNotification("Penonton_Terakhir • SAWER Rp2.000.000\nDia sudah bangun. Jangan berani-berani memotret wajahnya kalau kau belum punya segel.", 10);
            Ui(director.audioProfile.notification, .45f); StartCoroutine(Emerge());
        }
        private IEnumerator Emerge()
        {
            mind = Mind.Emerge; if (nyonya) nyonya.SetActive(false); yield return new WaitForSeconds(1.5f);
            if (teplokFlame) teplokFlame.SetActive(false); if (teplokLight) teplokLight.enabled = false;
            Ui(AtmajaSynth.Tone(.5f, 240, 90, .35f), .4f); yield return new WaitForSeconds(.85f);
            if (nyonya) nyonya.SetActive(true);
            if(director.phoneView)director.phoneView.AddChatBurst(new[]{"@kopi_susu99","@dinda_ayu","@bocil_kematian"},new[]{"ITU APA BARUSAN?!","ADA BAYANGAN DI KAMAR JAHIT!","BANG KAMERA NYA BERGETAR!"});
            Hud.ShowBroadcastNotification($"MataBatin • Entitas belum teridentifikasi.\nKamera tidak bisa melukainya. Sembunyi dan matikan layar [{Key(GameAction.ToggleScreen)}].", 7);
            yield return new WaitForSeconds(3); mind = Mind.Patrol; director.SetStage(AtmajaStage.BasementStealth); director.Objective();
        }
        private bool Ignored(Collider col) => col.transform.IsChildOf(nyonya.transform) || col.transform.IsChildOf(director.player.transform) || col == director.player.GetComponent<CharacterController>();
        public bool CanSeePlayer(float range)
        {
            var from = nyonya.transform.position + Vector3.up * 1.42f;
            var to = director.player.transform.position + Vector3.up * (director.player.IsCrouching ? .5f : 1.1f);
            var delta = to - from;
            if (delta.magnitude > range || delta.magnitude > 1.5f && Vector3.Dot(nyonya.transform.forward, delta.normalized) < .42f) return false;
            foreach (var hit in Physics.RaycastAll(from, delta.normalized, delta.magnitude, ~((1 << 29) | (1 << 2)), QueryTriggerInteraction.Ignore)) if (!Ignored(hit.collider)) return false;
            return true;
        }
        private void SenseTick(float dt)
        {
            float distance = Vector3.Distance(nyonya.transform.position, director.player.transform.position);
            bool moving = (director.player.transform.position - lastPosition).sqrMagnitude > .00001f; lastPosition = director.player.transform.position;
            float noise = !moving ? 0 : director.player.IsSprinting ? 8 : director.player.IsCrouching ? 1 : 3;
            bool seen = CanSeePlayer(Phone.isPhoneRaised && Phone.isScreenOn ? 8 : director.player.IsCrouching ? 3.5f : 5.5f), heard = noise > 0 && distance < noise;
            if (seen || heard) suspicionPoint = director.player.transform.position;
            detect = Mathf.Clamp01(detect + dt * (seen ? .8f : heard ? .35f : -.28f));
            if (seen && mind == Mind.Pursue) giveUpTimer = 5;
            if (detect >= 1 && mind != Mind.Pursue)
            {
                mind = Mind.Pursue; giveUpTimer = 5; Ui(AtmajaSynth.Tone(.6f, 250, 650, .3f), .45f); Hud.ShowBroadcastNotification("Dia melihatmu. Putus pandangannya, lalu sembunyi!", 3);
                if(director.phoneView)director.phoneView.AddChatBurst(new[]{"@maya_sari","@rudi_tabuti","@intan_permata"},new[]{"LARI BANG DIA LIAT KE KAMERA!","SEMBUNYI CEPET BANG!","MATIIN LAYAR HP NYA!"});
            }
            else if (detect > .32f && mind == Mind.Patrol) { mind = Mind.Suspicion; suspicionTimer = 3; }
        }
        private bool ClearPath(Vector3 a, Vector3 b)
        {
            var delta = b - a; delta.y = 0; if (delta.magnitude < .03f) return true;
            foreach (var hit in Physics.CapsuleCastAll(a + Vector3.up * .3f, a + Vector3.up * 1.4f, .23f, delta.normalized, delta.magnitude, ~((1 << 29) | (1 << 2)), QueryTriggerInteraction.Ignore)) if (!Ignored(hit.collider)) return false;
            return true;
        }
        private Vector3 NextPathPoint(Vector3 goal)
        {
            Vector3 start = nyonya.transform.position; if (ClearPath(start, goal) || waypoints.Length == 0) return goal;
            int n = waypoints.Length;
            if (connections == null)
            {
                connections = new float[n, n];
                for (int i = 0; i < n; i++) for (int j = 0; j < n; j++) connections[i, j] = i != j && ClearPath(waypoints[i].position, waypoints[j].position) ? Vector3.Distance(waypoints[i].position, waypoints[j].position) : float.PositiveInfinity;
            }
            var cost = new float[n]; var first = new int[n]; var used = new bool[n];
            for (int i = 0; i < n; i++) { cost[i] = ClearPath(start, waypoints[i].position) ? Vector3.Distance(start, waypoints[i].position) : float.PositiveInfinity; first[i] = i; }
            for (int step = 0; step < n; step++)
            {
                int k = -1; float best = float.PositiveInfinity;
                for (int i = 0; i < n; i++) if (!used[i] && cost[i] < best) { k = i; best = cost[i]; }
                if (k < 0) break; used[k] = true;
                for (int j = 0; j < n; j++) if (cost[k] + connections[k, j] < cost[j]) { cost[j] = cost[k] + connections[k, j]; first[j] = first[k]; }
            }
            int route = -1; float total = float.PositiveInfinity;
            for (int i = 0; i < n; i++) if (ClearPath(waypoints[i].position, goal) && cost[i] + Vector3.Distance(waypoints[i].position, goal) < total)
            { route = first[i]; total = cost[i] + Vector3.Distance(waypoints[i].position, goal); }
            return route < 0 ? start : waypoints[route].position;
        }
        public void PursueForChase(Vector3 goal,float speed,float dt)=>StepToward(goal,speed,dt);
        private void StepToward(Vector3 goal, float speed, float dt)
        {
            if (IsBlitzStunned) return;
            Vector3 position = nyonya.transform.position, target = NextPathPoint(goal), delta = target - position; delta.y = 0;
            if (delta.magnitude < .03f) return;
            var next = position + delta.normalized * Mathf.Min(speed * dt, delta.magnitude); if (ClearPath(position, next)) nyonya.transform.position = next;
            nyonya.transform.rotation = Quaternion.Slerp(nyonya.transform.rotation, Quaternion.LookRotation(delta.normalized), dt * 4);
            var pose=nyonya.GetComponent<AtmajaThreatPresentation>();
            if (shearsProp&&(!pose||!pose.HasCharacterRig)) shearsProp.transform.localRotation = Quaternion.Euler(90 + Mathf.Sin(Time.time * 4.2f) * 8, 0, 20);
        }
        private void ReturnToPatrol()
        {
            mind = Mind.Patrol; detect = Mathf.Min(detect, .2f); float best = float.PositiveInfinity;
            for (int i = 0; i < waypoints.Length; i++) if (Vector3.Distance(nyonya.transform.position, waypoints[i].position) < best) { best = Vector3.Distance(nyonya.transform.position, waypoints[i].position); waypointIndex = i; }
        }
        private void PatrolTick(float dt)
        {
            if (waypoints.Length == 0) return; if (waypointPause > 0) { waypointPause -= dt; return; }
            var target = waypoints[waypointIndex % waypoints.Length]; StepToward(target.position, .8f, dt);
            if (Vector3.Distance(nyonya.transform.position, target.position) < .2f) { waypointPause = 1.5f; waypointIndex++; }
        }
        private void HiddenTick(float dt)
        {
            detect = Mathf.Max(0, detect - dt * .4f);
            float distance = Vector3.Distance(nyonya.transform.position, hiddenAnchor.position);
            if (!lockerCleared && !lockerCheckRunning && (distance < 1.8f || hiddenWasSeen && distance < 2.5f))
            {
                lockerCheckRunning = true; inspectElapsed = 0;
                if(director.phoneView)director.phoneView.AddChatBurst(new[]{"@maya_sari","@dinda_ayu","@rudi_tabuti"},new[]{"DIA DEKET LEMARI BANG!","MATIIN LAYAR SEKARANG!","JANGAN NAFAS BANG PLIS!"});
                Hud.ShowBroadcastNotification($"Dia mendekat ke lemari. Matikan layar [{Key(GameAction.ToggleScreen)}] sekarang!", 5); director.Objective();
            }
            if (lockerCheckRunning)
            {
                inspectElapsed += dt; Vector3 toward = hiddenAnchor.position - nyonya.transform.position; toward.y = 0;
                if (toward.sqrMagnitude > .01f) nyonya.transform.rotation = Quaternion.Slerp(nyonya.transform.rotation, Quaternion.LookRotation(toward), dt * 3);
                // Warning precedes the four-second inspection, leaving time for an actual F press.
                if (inspectElapsed > 1.25f && Phone.isPhoneRaised && Phone.isScreenOn) { director.FailToCheckpoint("PROLOG_BASEMENT_ENTRY", "Cahaya HP terlihat melalui kisi-kisi lemari."); return; }
                if (inspectElapsed >= 5.25f)
                {
                    lockerCheckRunning = false; lockerCleared = true; hiddenWasSeen = false; mind = Mind.Patrol;
                    waypointPause = 0;
                    // Leave the occupied wardrobe after checking it; never turn straight
                    // back toward its door while telling the player the footsteps recede.
                    int away=Array.IndexOf(waypoints,hiddenAnchor==hideAnchor1?inspectAnchor2:inspectAnchor1);
                    waypointIndex=away>=0?away:(waypointIndex+1)%Mathf.Max(1,waypoints.Length);
                    Hud.ShowBroadcastNotification("Langkahnya menjauh. Tunggu sampai aman untuk keluar.", 3); director.Objective();
                }
            }
        }
        private void LateUpdate()
        {
            if (IsHidden && Hud && !UiFocus.HardModal && !Hud.IsModalOpen)
                Hud.SetInteractionPrompt("[" + Key(GameAction.Interact) + "] Keluar lemari");
        }
    }
}
