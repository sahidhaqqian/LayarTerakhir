using System;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public sealed class AtmajaChaseSequence : MonoBehaviour
    {
        public const string ChaseCheckpoint = "PROLOG_CHASE_START";
        private enum Run { Idle, Playback, Breaking, Pursuit, WindowStandoff, Stunned, GateRun, Ending, Complete }
        public AtmajaDirector director;
        public AtmajaBasementHorror basement;
        public Transform spawnChase, spawnStreet, windowBlockSpot, windowExitSpot, gateSpot, houseLookSpot;
        public Transform[] rail = new Transform[0];
        public Transform cellarExitApproach, slideExit, diningVaultStart, diningVaultEnd;
        public GameObject slideGate, diningBlocker, windowContextLeaf, chaserFigure, streetFigure, endingTitle, bigMirror, mirrorShards;
        public GameObject cellarEgressFloor;
        public GameObject[] fallenFurniture = new GameObject[0];
        public Light[] corridorLights = new Light[0];
        public Material mirrorCrackedMat;
        public Light lightning;
        public Transform handycamScreenSpot;
        public AtmajaHandycamPlayback recording;
        public PrologFinaleView finale;
        public bool AwaitingBlitz => run == Run.WindowStandoff;
        public bool IsStunned => run == Run.Stunned || run == Run.GateRun && Time.time < stunEndsAt;
        public bool IsGateRun => run == Run.GateRun;
        public bool Traversing { get; private set; }
        public bool IsPursuing => run == Run.Pursuit;
        public bool CanCatchPlayer() => chaserFigure && director && director.player && basement
            && Vector3.Distance(chaserFigure.transform.position,director.player.transform.position)<.95f
            && basement.CanSeePlayer(2);
        private Run run;
        private float stunTimer, stunEndsAt, blitzWindow, railDistance, dangerGrace, dragTimer;
        private bool climbedOut, vaultedTable;
        private float[] railLengths;
        private AudioSource music;
        private Camera playerCamera;
        private float baseFov;
        private bool fovInit;
        private AudioClip scrape;
        private CampaignState Campaign => CampaignState.Instance;
        private SmartphoneController Phone => SmartphoneController.Instance;
        private HudController Hud => HudController.Instance;
        private bool Has(string id) => Campaign && Campaign.HasInventoryItem(id);
        private string Key(GameAction action) => GameInputConfig.Instance.GetActionLabel(action);
        private bool Commit(string id, string checkpoint, Action change = null)
        {
            if (Has(id)) return true;
            string before = JsonUtility.ToJson(Campaign.Data); Campaign.AddInventoryItem(id); change?.Invoke();
            if (Campaign.SaveCheckpointAtomic(checkpoint)) return true;
            JsonUtility.FromJsonOverwrite(before, Campaign.Data); Hud.ShowBroadcastNotification("Checkpoint gagal disimpan. Coba interaksi kembali.", 4); return false;
        }
        private void Sfx(AudioClip clip, Vector3 position, float gain)
        { if (director.foleySource && clip) { director.foleySource.transform.position = position; director.foleySource.PlayOneShot(clip, gain); } }
        private void Ui(AudioClip clip, float gain) { if (director.notificationSource && clip) director.notificationSource.PlayOneShot(clip, gain); }
        public void RestoreWorld()
        {
            StopAllCoroutines(); if (recording) recording.StopPlayback(); if (finale) finale.ResetView();
            bool started = Has("EVENT_P0_CHASE_STARTED"), done = Has("PROLOG_COMPLETE");
            run = Run.Idle; Traversing = false; vaultedTable = false; climbedOut = false; railDistance = -6; stunEndsAt=0;
            if (Time.timeScale > 0) Time.timeScale = 1;
            if (slideGate) slideGate.SetActive(false);
            if(cellarEgressFloor)cellarEgressFloor.SetActive(started&&!done);
            if (diningBlocker) diningBlocker.SetActive(started && !done);
            foreach (var piece in fallenFurniture) if (piece) piece.SetActive(started && !done);
            if (windowContextLeaf) windowContextLeaf.SetActive(true);
            if (streetFigure) streetFigure.SetActive(false);
            if (endingTitle) endingTitle.SetActive(false);
            if (bigMirror) bigMirror.SetActive(!started);
            if (mirrorShards) mirrorShards.SetActive(started);
            foreach (var light in corridorLights) if (light) light.enabled = true;
            director.player.MovementLocked = false; director.phoneView.CinematicHandsetVisible = false; director.phoneView.ChatFrozen = false;
            if (!playerCamera) playerCamera = director.player.GetComponent<InteractionInteractor>().playerCamera;
            if (playerCamera) { if (!fovInit) { baseFov = playerCamera.fieldOfView; fovInit = true; } playerCamera.fieldOfView = baseFov; }
            BuildRail();
            if (done)
            {
                run = Run.Complete; Music(false); if (chaserFigure) chaserFigure.SetActive(false);
                director.player.MovementLocked = true; UiFocus.Cinematic = true;
                finale.ShowTitle(true);
            }
            else if (Has("EVENT_P0_ENDING_STARTED")) StartCoroutine(Ending());
            else if (started) ResumeChase();
            else { if (chaserFigure && !Has("EVENT_P0_BASEMENT_ENTERED")) chaserFigure.SetActive(false); Music(false); }
        }
        private void BuildRail()
        {
            railLengths = new float[rail.Length];
            for (int i = 1; i < rail.Length; i++) railLengths[i] = railLengths[i - 1] + Vector3.Distance(rail[i - 1].position, rail[i].position);
        }
        private float ClosestProgress(Vector3 position)
        {
            float best = float.PositiveInfinity, progress = 0;
            for (int i = 0; i < rail.Length - 1; i++)
            {
                Vector3 a = rail[i].position, delta = rail[i + 1].position - a;
                float t = Mathf.Clamp01(Vector3.Dot(position - a, delta) / Mathf.Max(.001f, delta.sqrMagnitude));
                float distance = (position - (a + delta * t)).sqrMagnitude;
                if (distance < best) { best = distance; progress = Mathf.Lerp(railLengths[i], railLengths[i + 1], t); }
            }
            return progress;
        }
        private Vector3 RailPoint(float distance)
        {
            if (rail.Length == 0) return director.player.transform.position;
            for (int i = 1; i < rail.Length; i++) if (distance <= railLengths[i])
                return Vector3.Lerp(rail[i - 1].position, rail[i].position, Mathf.InverseLerp(railLengths[i - 1], railLengths[i], distance));
            return rail[rail.Length - 1].position;
        }
        private void ResumeChase()
        {
            run = Run.Breaking; railDistance = -6; dangerGrace = 3; climbedOut = vaultedTable = false;
            director.SetStage(AtmajaStage.Chase); director.SetBusy(false);
            if (chaserFigure) { chaserFigure.SetActive(true); chaserFigure.transform.position = RailPoint(0); }
            Music(true); StartCoroutine(BreakBeat());
        }
        private void Music(bool on)
        {
            if (on && !music)
            {
                music = gameObject.AddComponent<AudioSource>(); music.playOnAwake = false; music.loop = true; music.spatialBlend = 0;
                music.volume = .24f; music.clip = AtmajaSynth.ChaseLoop(); music.outputAudioMixerGroup = director.audioProfile.effectsBus;
            }
            if (music) { if (on && !music.isPlaying) music.Play(); else if (!on) music.Stop(); }
        }
        public string Objective()
        {
            if (run == Run.WindowStandoff) return $"[{Key(GameAction.CaptureOrBlitz)}] TEMBAKKAN BLITZ!";
            if (run == Run.Stunned) return $"[{Key(GameAction.Vault)}] Terobos jendela dapur!";
            if (run == Run.GateRun) return "Lari ke gerbang! Merunduk melewati dahan kamboja.";
            if (run == Run.Ending || run == Run.Complete) return "Malam pertama selesai. Damar masih di balik cermin.";
            if (!climbedOut) return "LARI! Kembali ke tangga batu di sudut ruang jahit!";
            return "Turuni tangga tengah! Kembali ke jendela dapur!";
        }
        public void PlayHandycamVideo()
        {
            if (run == Run.Playback || Has("EVENT_P0_CHASE_STARTED")) return;
            StartCoroutine(Playback());
        }
        private IEnumerator Playback()
        {
            run = Run.Playback; director.SetBusy(true); director.player.MovementLocked = true; UiFocus.Cinematic = true;
            yield return recording.Play();
            // Playback completion and chase checkpoint form one transaction. Quitting earlier replays the video.
            bool saved = Commit("EVENT_P0_CHASE_STARTED", ChaseCheckpoint, () => Campaign.AddInventoryItem("EVENT_P0_HANDYCAM_PLAYED"));
            UiFocus.Cinematic = false; director.player.MovementLocked = false; director.SetBusy(false);
            if (!saved) { run = Run.Idle; yield break; }
            StartChase();
        }
        public void StartChase()
        {
            if (bigMirror) bigMirror.SetActive(false); if (mirrorShards) mirrorShards.SetActive(true);
            if (diningBlocker) diningBlocker.SetActive(true); foreach (var piece in fallenFurniture) if (piece) piece.SetActive(true);
            if (basement.ragOriginal) basement.ragOriginal.SetActive(false);
            if (basement.handycamObj) basement.handycamObj.SetActive(false);
            if (basement.humSource) basement.humSource.Stop();
            basement.StopAllCoroutines();
            Sfx(AtmajaSynth.Noise(.6f, .5f), director.player.transform.position, .6f);
            Hud.PlayPriorityDialogue("Nyonya Sri Atmaja", "AAAAAARGH!!!", 2.5f);
            director.phoneView.AddChat("@agung_gaming", "LARI BANG LARI JANGAN NOLEH BELAKANG");
            ResumeChase();
        }
        private IEnumerator BreakBeat() { yield return new WaitForSeconds(1.7f); if (run == Run.Breaking) { run = Run.Pursuit; director.Objective(); } }
        private void Update()
        {
            if (!director || !director.player || !Campaign || !Hud || director.Busy || UiFocus.HardModal || Hud.IsModalOpen || Time.timeScale<=0) return;
            if (run == Run.WindowStandoff) { StandoffTick(); return; }
            if (run == Run.Stunned)
            {
                stunTimer -= Time.deltaTime;
                if (GameInputConfig.Instance.GetActionDown(GameAction.Vault) && Vector3.Distance(director.player.transform.position, director.vaultInside.position) < 3.8f)
                    StartCoroutine(EscapeWindow());
                else if (stunTimer <= 0) Fail("Terlambat melewati jendela setelah Blitz. Terobos segera saat dia terpaku.");
                return;
            }
            if (run != Run.Pursuit && run != Run.GateRun) return;
            float dt = Time.deltaTime; dangerGrace = Mathf.Max(0, dangerGrace - dt);
            if (chaserFigure && rail.Length > 1 && !IsStunned)
            {
                Vector3 before = chaserFigure.transform.position;
                railDistance += 3.45f * dt;
                // The hunter follows the authored 3D route, including stairs, rather than snapping to the player's floor.
                chaserFigure.transform.position = RailPoint(Mathf.Max(0, railDistance));
                Vector3 direction = chaserFigure.transform.position - before; direction.y = 0;
                if (direction.sqrMagnitude > .00001f) chaserFigure.transform.rotation = Quaternion.Slerp(chaserFigure.transform.rotation, Quaternion.LookRotation(direction), dt * 6);
                dragTimer -= dt; if (dragTimer <= 0) { dragTimer = .48f; if (!scrape) scrape = AtmajaSynth.Scrape(.3f); Sfx(scrape, chaserFigure.transform.position, .3f); }
                // Adjacent stair flights/furniture must not cause a capture through
                // solid geometry merely because their root positions are close.
                if (dangerGrace <= 0 && CanCatchPlayer()) { Fail("Nyonya Atmaja menyusulmu. Terus bergerak menuju dapur."); return; }
            }
            if (run == Run.GateRun)
            {
                if (Vector3.Distance(director.player.transform.position, gateSpot.position) < 2.8f) StartCoroutine(Ending());
                return;
            }
            if (!climbedOut && cellarExitApproach && Vector3.Distance(director.player.transform.position, cellarExitApproach.position) < 1.3f)
            {
                finale.Prompt($"[{Key(GameAction.Sprint)}] NAIKI TANGGA BATU");
                if (GameInputConfig.Instance.GetActionHeld(GameAction.Sprint) || GameInputConfig.Instance.GetActionDown(GameAction.Interact)) StartCoroutine(ClimbOut());
            }
            else finale.Prompt(null);
            if(climbedOut&&Vector3.Distance(director.player.transform.position,director.upper.trapdoorTop.position)<1.8f)
                finale.Prompt($"[{Key(GameAction.Crouch)}] MERUNDUK! PINTU JEBAKAN MENUTUP");
            if (climbedOut && !vaultedTable && diningVaultStart && Vector3.Distance(director.player.transform.position, diningVaultStart.position) < 1.6f)
            {
                finale.Prompt($"[{Key(GameAction.Vault)}] LOMPATI MEJA");
                if (GameInputConfig.Instance.GetActionDown(GameAction.Vault)) StartCoroutine(VaultTable());
            }
            bool upper = director.player.transform.position.y > 3;
            if (playerCamera) playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, upper ? baseFov + (GameShell.ReducedGlitch ? 1 : 3 + Mathf.Sin(Time.time * 2.4f)) : baseFov, dt * 3);
            foreach (var light in corridorLights) if (light && light.enabled && Vector3.Distance(director.player.transform.position, light.transform.position - Vector3.up * 1.4f) < 1.5f)
            { light.enabled = false; Sfx(director.audioProfile.latch, light.transform.position, .5f); }
            if (climbedOut && vaultedTable && director.player.transform.position.y < 1.2f && Vector3.Distance(director.player.transform.position, director.vaultInside.position) < 2.6f) BeginStandoff();
        }
        private IEnumerator ClimbOut()
        {
            Traversing = true; director.SetBusy(true); director.player.MovementLocked = true; finale.Prompt(null);
            var cc = director.player.GetComponent<CharacterController>(); cc.enabled = false;
            Vector3 start = director.player.transform.position, top = director.upper.trapdoorTop.position;
            if (slideGate) slideGate.SetActive(true);
            if(cellarEgressFloor)cellarEgressFloor.SetActive(true);
            for (float t = 0; t < 1; t += Time.deltaTime / 2.2f)
            { director.player.transform.position = Vector3.Lerp(start, top, Mathf.SmoothStep(0, 1, t)); yield return null; }
            director.player.transform.position = top;
            director.player.transform.rotation=director.upper.trapdoorTop.rotation;director.player.SetViewPitch(7);
            cc.enabled = true; director.player.MovementLocked = false; director.SetBusy(false); Traversing = false; climbedOut = true;
            // The real low slab requires crouching/slide to leave the shaft.
            finale.Prompt($"[{Key(GameAction.Crouch)}] MERUNDUK! PINTU JEBAKAN MENUTUP");
            railDistance = ClosestProgress(top) - 14; dangerGrace = 4; director.Objective(); Physics.SyncTransforms();
        }
        private IEnumerator VaultTable()
        {
            yield return Traverse(diningVaultEnd.position, .85f, .65f); vaultedTable = true; finale.Prompt(null);
            Sfx(director.audioProfile.latch, diningVaultEnd.position, .65f);
        }
        private IEnumerator Traverse(Vector3 end, float seconds, float arc)
        {
            Traversing = true; director.SetBusy(true); director.player.MovementLocked = true;
            var cc = director.player.GetComponent<CharacterController>(); cc.enabled = false; Vector3 start = director.player.transform.position;
            for (float t = 0; t < 1; t += Time.deltaTime / seconds)
            { director.player.transform.position = Vector3.Lerp(start, end, Mathf.SmoothStep(0, 1, t)) + Vector3.up * Mathf.Sin(t * Mathf.PI) * arc; yield return null; }
            director.player.transform.position = end; cc.enabled = true; director.player.MovementLocked = false; director.SetBusy(false); Traversing = false; Physics.SyncTransforms();
        }
        private void BeginStandoff()
        {
            run = Run.WindowStandoff; blitzWindow = 3; Time.timeScale = .15f; finale.Prompt($"[{Key(GameAction.CaptureOrBlitz)}] TEMBAKKAN FLASH SEKARANG!");
            Phone.isPhoneRaised = Phone.isScreenOn = true; Phone.currentScanMode = ScanVisualMode.Normal; Phone.SyncVisualState();
            director.phoneView.Apps.Open(PhoneApp.Camera); director.player.MovementLocked = true;
            if (chaserFigure) chaserFigure.transform.SetPositionAndRotation(windowBlockSpot.position, windowBlockSpot.rotation);
            Vector3 direction=windowBlockSpot.position+Vector3.up*1.45f-director.player.cameraPivot.position;
            Vector3 flat=direction;flat.y=0;
            if(flat.sqrMagnitude>.001f)director.player.transform.rotation=Quaternion.LookRotation(flat);
            director.player.SetViewPitch(-Mathf.Atan2(direction.y,flat.magnitude)*Mathf.Rad2Deg);
            var rig=director.player.GetComponent<ShoulderCameraRig>();if(rig){rig.FirstPersonOverride=true;rig.RefreshCameraPose();}
            Ui(AtmajaSynth.Tone(2.8f, 400, 2200, .2f), .5f); director.Objective();
        }
        private void StandoffTick()
        {
            blitzWindow -= Time.unscaledDeltaTime;
            if (GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz) && TryEmergencyBlitz()) return;
            if (blitzWindow <= 0) Fail("Blitz terlambat. Saat dia menghadang jendela, tembakkan Flash kamera.");
        }
        public bool TryEmergencyBlitz()
        {
            if (run != Run.WindowStandoff || Phone.batteryPercent < Phone.blitzCostPercent) return false;
            Phone.ConsumeBattery(Phone.blitzCostPercent); run = Run.Stunned; stunTimer = 4; stunEndsAt=Time.time+4; Time.timeScale = 1;
            director.player.MovementLocked = false;
            var rig=director.player.GetComponent<ShoulderCameraRig>();if(rig)rig.FirstPersonOverride=false;
            StartCoroutine(finale.Flash());
            Sfx(AtmajaSynth.Shutter(), director.player.transform.position, .8f);
            Hud.PlayPriorityDialogue("Nyonya Sri Atmaja", "AAAAAARGH!!!", 2);
            finale.Prompt($"[{Key(GameAction.Vault)}] TEROBOS JENDELA!"); director.Objective(); return true;
        }
        private IEnumerator EscapeWindow()
        {
            director.kitchenShutter.SetActive(false); if (windowContextLeaf) windowContextLeaf.SetActive(false);
            if (chaserFigure) chaserFigure.transform.position = windowBlockSpot.position - director.north * 1.3f;
            Sfx(AtmajaSynth.Noise(.45f, .45f), director.vaultInside.position, .6f);
            yield return Traverse(windowExitSpot.position, 1.05f, .35f);
            director.player.transform.rotation=windowExitSpot.rotation;director.player.SetViewPitch(4);
            run = Run.GateRun; dangerGrace = 4; railDistance = ClosestProgress(windowExitSpot.position) - 6;
            var gate = director.gateHinge.GetComponent<DoubleLeafGate>(); if (gate) gate.SetImmediate(true);
            finale.Prompt(null); director.Objective();
        }
        private void Fail(string reason)
        {
            Time.timeScale = 1; director.player.MovementLocked = false;
            if (playerCamera) playerCamera.fieldOfView = baseFov;
            director.FailToCheckpoint(ChaseCheckpoint, reason);
        }
        private IEnumerator Ending()
        {
            run = Run.Ending; Time.timeScale = 1; Music(false); finale.Prompt(null);
            if (chaserFigure) chaserFigure.SetActive(false);
            director.SetStage(AtmajaStage.EndingVista); director.SetBusy(true); director.player.MovementLocked = true; UiFocus.Cinematic = true;
            if (!Commit("EVENT_P0_ENDING_STARTED", "PROLOG_ATMAJA_ENDING", () => {
                if (Campaign.AddInventoryItem("EVENT_P0_FINAL_SAWER"))
                {
                    long total = Campaign.Data.wallet_bound + Campaign.Data.wallet_legitimate + Campaign.Data.spent;
                    Campaign.AddBoundDonation(Math.Max(0, 25450000 - total));
                }
                Campaign.AddInventoryItem("EVENT_P0_FINAL_CONTRACT");
            })) { director.SetBusy(false); UiFocus.Cinematic = false; run = Run.GateRun; yield break; }
            director.PlacePlayer(spawnStreet.position, spawnStreet.rotation);
            Phone.isPhoneRaised = false; Phone.SyncVisualState();
            var camera = director.player.GetComponent<InteractionInteractor>().playerCamera;
            Vector3 at = spawnStreet.position + Vector3.up * .65f;
            Vector3 look = streetFigure ? streetFigure.transform.position + Vector3.up : director.P(1, 0, 5);
            camera.transform.SetPositionAndRotation(at, Quaternion.LookRotation(look - at));
            Hud.PlayPriorityDialogue("Raka", "*terengah-engah* ...Gue... selamat... Damar... gue bakal balik buat lo...", 6);
            yield return new WaitForSeconds(3);
            if (streetFigure) streetFigure.SetActive(true);
            for (int i = 0; i < 2; i++)
            {
                if (lightning) lightning.intensity = 3.5f;
                Sfx(director.audioProfile.knock, spawnStreet.position + director.north * 15, .55f);
                yield return new WaitForSeconds(.16f); if (lightning) lightning.intensity = 0; yield return new WaitForSeconds(.85f);
            }
            Phone.isBroadcastConnected = true; Phone.isPhoneRaised = Phone.isScreenOn = true; Phone.SyncVisualState();
            director.phoneView.CinematicHandsetVisible = true; director.phoneView.Apps.Open(PhoneApp.Camera);
            string[] authors = { "@viral_horor_id", "@kuyang_watch", "@tiktok_repost", "@intan_permata", "@maya_sari", "@rudi_tabuti" };
            string[] chat = { "GILA INI LIVE TERGILA TAHUN INI", "ITU TADI BENERAN HANTU WOI", "VIRALIN KE TIKTOK SEKARANG!", "bang Damar masih di dalem kaca??", "54 RIBU ORANG NONTON BANG", "jangan balik bang jangan balik" };
            for (int i = 0; i < 12; i++) { director.phoneView.AddChat(authors[i % authors.Length], chat[i % chat.Length]); yield return new WaitForSeconds(.22f); }
            director.phoneView.ChatFrozen = true;
            finale.ShowContract(director.phoneView.Panel.transform); Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
            yield return new WaitUntil(() => finale.ContractRead);
            finale.CloseContract(); director.phoneView.CinematicHandsetVisible = false;
            Sfx(AtmajaSynth.Shutter(), director.player.transform.position, .9f);
            yield return Hud.FadeBlack(0, 1, .45f);
            bool saved = Commit("PROLOG_COMPLETE", "PROLOG_COMPLETE");
            run = Run.Complete; director.Objective(); finale.ShowTitle(saved);
        }
        private void OnDestroy()
        {
            if (Time.timeScale > 0 && Time.timeScale < .5f) Time.timeScale = 1;
            if (music && music.clip) Destroy(music.clip);
        }
    }
}
