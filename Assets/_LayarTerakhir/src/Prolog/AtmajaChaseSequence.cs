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
        private enum Run { Idle, Playback, Breaking, Pursuit, WindowStandoff, FinalFlash, EscapeReady, Stunned, GateRun, Ending, Complete }
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
        public const int RequiredPhotos=3;
        public bool AwaitingBlitz => run == Run.WindowStandoff;
        public bool AwaitingPhotos => run == Run.WindowStandoff;
        public bool IsFinalFlash => run == Run.FinalFlash;
        public bool IsStunned => run == Run.Stunned || run == Run.GateRun && Time.time < stunEndsAt || run==Run.Pursuit&&Time.time<pursuitStunUntil;
        public bool IsGateRun => run == Run.GateRun;
        public bool ThreatDefeated => run==Run.EscapeReady||run==Run.GateRun||run==Run.Ending||run==Run.Complete||
            run==Run.FinalFlash&&chaserFigure&&!chaserFigure.activeInHierarchy;
        public int PhotosTaken => photosTaken;
        public bool Traversing { get; private set; }
        public bool IsPursuing => run == Run.Pursuit;
        public bool FollowsPlayerInBasement => run == Run.Pursuit && !climbedOut;
        public bool CanCatchPlayer() => !IsStunned && chaserFigure && chaserFigure.activeInHierarchy && director && director.player && basement
            && Vector3.Distance(chaserFigure.transform.position,director.player.transform.position)<.95f
            && basement.CanSeePlayer(2);
        private Run run;
        private float stunEndsAt, railDistance, dangerGrace, dragTimer;
        private int photosTaken;
        private float photoReadyAt;
        private float pursuitStunUntil,standoffGrace;
        private ParticleSystem photoSmoke;
        private Material photoSmokeMaterial;
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
            run = Run.Idle; Traversing = false; vaultedTable = false; climbedOut = false; railDistance = -6; stunEndsAt=pursuitStunUntil=0;
            SetPlayerCollision(false);
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
            run = Run.Breaking; railDistance = -6; dangerGrace = 5; climbedOut = vaultedTable = false;
            director.SetStage(AtmajaStage.Chase); director.SetBusy(false);
            if (chaserFigure) { chaserFigure.SetActive(true); chaserFigure.transform.position = RailPoint(0); }
            if(director.phoneView)director.phoneView.AddChatBurst(new[]{"@kopi_susu99","@rizky_pratama_","@bocil_kematian"},new[]{"LARI BANG JANGAN NENGOK!","DIA DI BELAKANG LU!","TANGGA BANG CEPET!"},4);
            Music(true); StartCoroutine(BreakBeat());
        }
        private void Music(bool on)
        {
            if (on && !music)
            {
                music = gameObject.AddComponent<AudioSource>(); music.playOnAwake = false; music.loop = true; music.spatialBlend = 0;
                music.volume = .42f; music.clip = AtmajaSynth.ChaseLoop(); music.outputAudioMixerGroup = director.audioProfile.effectsBus;
            }
            if (music) { if (on && !music.isPlaying) music.Play(); else if (!on) music.Stop(); }
        }
        public string Objective()
        {
            if (run == Run.WindowStandoff || run == Run.EscapeReady || run == Run.Stunned) return string.Empty;
            if (run == Run.GateRun) return "Keluar menuju gerbang. Merunduk melewati dahan kamboja.";
            if (run == Run.Ending || run == Run.Complete) return "Malam pertama selesai. Damar masih di balik cermin.";
            if (!climbedOut) return "LARI! Kembali ke tangga besi di sudut ruang jahit!";
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
            if (run == Run.EscapeReady)
            {
                if (GameInputConfig.Instance.GetActionDown(GameAction.Vault) && Vector3.Distance(director.player.transform.position, director.vaultInside.position) < 3.8f)
                    StartCoroutine(EscapeWindow());
                return;
            }
            if (run != Run.Breaking && run != Run.Pursuit && run != Run.GateRun) return;
            if (GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz))
            {
                if (!TryPursuitBlitz() && Phone && Phone.isPhoneRaised && Phone.isScreenOn && Phone.HasCameraPower &&
                    director.phoneView && director.phoneView.IsCameraApp && (!UiFocus.Phone || director.phoneView.ViewfinderActive))
                {
                    if (!Phone.RitualCameraPower && Phone.batteryPercent > 0f)
                        Phone.ConsumeBattery(Mathf.Min(Phone.batteryPercent, Phone.photoCostPercent));
                    director.phoneView.CaptureFlash();
                }
            }
            if(chaserFigure)
            {
                var presentation=chaserFigure.GetComponent<AtmajaThreatPresentation>();
                if(presentation)presentation.ExternalStun=IsStunned;
            }
            if (run == Run.Breaking) return;
            float dt = Time.deltaTime; dangerGrace = Mathf.Max(0, dangerGrace - dt);
            if (chaserFigure && chaserFigure.activeInHierarchy && rail.Length > 1 && !IsStunned)
            {
                Vector3 before = chaserFigure.transform.position;
                if(FollowsPlayerInBasement)
                    basement.PursueForChase(director.player.transform.position,3.45f,dt);
                else
                {
                    // Above the cellar, follow the authored stairs behind Raka.
                    float playerProgress=ClosestProgress(director.player.transform.position);
                    railDistance=Mathf.MoveTowards(railDistance,Mathf.Max(0,playerProgress-.35f),3.45f*dt);
                    chaserFigure.transform.position=RailPoint(Mathf.Max(0,railDistance));
                }
                Vector3 direction = chaserFigure.transform.position - before; direction.y = 0;
                if(Vector3.Distance(chaserFigure.transform.position,director.player.transform.position)<2)
                {direction=director.player.transform.position-chaserFigure.transform.position;direction.y=0;}
                if (direction.sqrMagnitude > .00001f) chaserFigure.transform.rotation = Quaternion.Slerp(chaserFigure.transform.rotation, Quaternion.LookRotation(direction), dt * 6);
                dragTimer -= dt; if (dragTimer <= 0) { dragTimer = .48f; if (!scrape) scrape = AtmajaSynth.Scrape(.3f); basement.PlayThreatScrape(scrape,.3f); }
                // Adjacent stair flights/furniture must not cause a capture through
                // solid geometry merely because their root positions are close.
                if (dangerGrace <= 0 && CanCatchPlayer())
                {
                    Debug.Log($"ATMAJA_CHASE_CAPTURE player={director.player.transform.position} hunter={chaserFigure.transform.position} crouching={director.player.IsCrouching} basement={FollowsPlayerInBasement}");
                    Fail("Nyonya Atmaja menyusulmu. Terus bergerak menuju dapur."); return;
                }
            }
            if (run == Run.GateRun)
            {
                if (Vector3.Distance(director.player.transform.position, gateSpot.position) < 2.8f) StartCoroutine(Ending());
                return;
            }
            if (!climbedOut && cellarExitApproach && Vector3.Distance(director.player.transform.position, cellarExitApproach.position) < 1.3f)
            {
                finale.Prompt($"[{Key(GameAction.Sprint)}] NAIKI TANGGA BESI");
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
            if (slideGate) slideGate.SetActive(true);
            if(cellarEgressFloor)cellarEgressFloor.SetActive(true);
            var ascent = director.upper!=null ? director.upper.CellarAscentPath() : null;
            if (ascent != null && ascent.Length > 1)
            {
                // Climb the same visible flight Raka came down: tread by tread
                // with the camera tilted up, never a straight-line teleport.
                // Chase pace is urgent, so the ascent stays quicker than the descent.
                Hud.EnqueueSubtitle("Raka","Naik... naik... jangan nengok!",3);
                float bob = 0, pitch = 0;
                for (int seg = 0; seg < ascent.Length; seg++)
                {
                    Vector3 target = ascent[seg];
                    while (Vector3.Distance(director.player.transform.position, target) > .08f)
                    {
                        float step = 4.5f * Time.deltaTime;
                        Vector3 pos = Vector3.MoveTowards(director.player.transform.position, target, step);
                        bob += step * 7f;
                        director.player.transform.position = pos;
                        Vector3 look = seg + 1 < ascent.Length ? ascent[seg + 1] : director.upper.trapdoorTop.position + Vector3.up * 1.2f;
                        Vector3 flat = look - pos; flat.y = 0;
                        if(director.upper.cellarLadderRoute.Length>1&&seg>0&&seg<ascent.Length-1)flat=-director.north;
                        if (flat.sqrMagnitude > .01f) director.player.transform.rotation = Quaternion.Slerp(director.player.transform.rotation, Quaternion.LookRotation(flat), Time.deltaTime * 6);
                        pitch = Mathf.MoveTowards(pitch, seg < ascent.Length - 1 ? -22 : 5, 40 * Time.deltaTime);
                        director.player.SetViewPitch(pitch+Mathf.Sin(bob)*.6f);
                        yield return null;
                    }
                }
            }
            else
            {
                Vector3 start = director.player.transform.position, top = director.upper.trapdoorTop.position;
                for (float t = 0; t < 1; t += Time.deltaTime / 2.2f)
                { director.player.transform.position = Vector3.Lerp(start, top, Mathf.SmoothStep(0, 1, t)); yield return null; }
            }
            director.player.transform.position = director.upper.trapdoorTop.position;
            director.player.transform.rotation=director.upper.trapdoorTop.rotation;director.player.SetViewPitch(7);
            cc.enabled = true; director.player.MovementLocked = false; director.SetBusy(false); Traversing = false; climbedOut = true;
            // The real low slab requires crouching/slide to leave the shaft.
            finale.Prompt($"[{Key(GameAction.Crouch)}] MERUNDUK! PINTU JEBAKAN MENUTUP");
            railDistance = ClosestProgress(director.upper.trapdoorTop.position) - 14; dangerGrace = 4; director.Objective(); Physics.SyncTransforms();
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
            run = Run.WindowStandoff; photosTaken = 0; photoReadyAt = 0; Time.timeScale = .5f;
            standoffGrace=Time.unscaledTime+3;
            Phone.isPhoneRaised = Phone.isScreenOn = true; Phone.currentScanMode = ScanVisualMode.Normal; Phone.SyncVisualState();
            director.phoneView.Apps.Open(PhoneApp.Camera);
            director.player.MovementLocked = false;
            if (chaserFigure)
            {
                var pose=chaserFigure.GetComponent<AtmajaThreatPresentation>();if(pose)pose.ExternalStun=false;
                SetPlayerCollision(true);
                chaserFigure.transform.SetPositionAndRotation(windowBlockSpot.position,windowBlockSpot.rotation);
                Physics.SyncTransforms();
            }
            Vector3 direction=windowBlockSpot.position+Vector3.up*1.45f-director.player.cameraPivot.position;
            Vector3 flat=direction;flat.y=0;
            if(flat.sqrMagnitude>.001f)director.player.transform.rotation=Quaternion.LookRotation(flat);
            director.player.SetViewPitch(-Mathf.Atan2(direction.y,flat.magnitude)*Mathf.Rad2Deg);
            var rig=director.player.GetComponent<ShoulderCameraRig>();if(rig){rig.FirstPersonOverride=true;rig.RefreshCameraPose();}
            Hud.PlayPriorityDialogue("Raka","Gue harus lawan dia.",3);
            finale.Prompt(null);
            Ui(AtmajaSynth.Tone(2.8f, 400, 2200, .2f), .5f);
            if(director.phoneView)director.phoneView.AddChatBurst(new[]{"@maya_sari","@rudi_tabuti","@intan_permata"},new[]{"FOTO DIA BANG!","JANGAN BERHENTI MOTRET!","ASAPNYA NUTUPIN KAMERA!"});
            director.Objective();
        }
        private void StandoffTick()
        {
            if(!Phone.isPhoneRaised||!Phone.isScreenOn){Phone.isPhoneRaised=Phone.isScreenOn=true;Phone.SyncVisualState();}
            if(director.phoneView&&!director.phoneView.IsCameraApp)director.phoneView.Apps.Open(PhoneApp.Camera);
            var pose=chaserFigure?chaserFigure.GetComponent<AtmajaThreatPresentation>():null;
            if(chaserFigure&&(!pose||!pose.ExternalStun))
            {
                Vector3 direction=director.player.transform.position-chaserFigure.transform.position;direction.y=0;
                if(direction.sqrMagnitude>.01f)
                {
                    chaserFigure.transform.rotation=Quaternion.Slerp(chaserFigure.transform.rotation,Quaternion.LookRotation(direction),Time.deltaTime*4);
                    float step=Mathf.Min(.55f*Time.deltaTime,Mathf.Max(0,direction.magnitude-.65f));
                    bool clear=true;
                    foreach(var hit in Physics.CapsuleCastAll(chaserFigure.transform.position+Vector3.up*.3f,chaserFigure.transform.position+Vector3.up*1.4f,.23f,direction.normalized,step,~((1<<29)|(1<<2)),QueryTriggerInteraction.Ignore))
                        if(!hit.transform.IsChildOf(chaserFigure.transform)&&!hit.transform.IsChildOf(director.player.transform)){clear=false;break;}
                    if(clear)chaserFigure.transform.position+=direction.normalized*step;
                }
                if(Time.unscaledTime>standoffGrace&&Vector3.Distance(chaserFigure.transform.position,director.player.transform.position)<.75f&&CanCatchPlayer()){Fail("Nyonya Atmaja menangkapmu di dekat jendela.");return;}
            }
            if (Time.unscaledTime < photoReadyAt) return;
            if (GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz)) TryPhoto();
        }
        private void SetPlayerCollision(bool ignore)
        {
            if(!chaserFigure||!director||!director.player)return;
            var playerCollider=director.player.GetComponent<CharacterController>();
            foreach(var collider in chaserFigure.GetComponentsInChildren<Collider>(true))Physics.IgnoreCollision(playerCollider,collider,ignore);
        }
        public bool TryPursuitBlitz()
        {
            if((run!=Run.Breaking&&run!=Run.Pursuit&&run!=Run.GateRun)||IsStunned||!Phone||!Phone.isPhoneRaised||!Phone.isScreenOn||!Phone.HasCameraPower)return false;
            if(!director.phoneView||!director.phoneView.IsCameraApp||!director.phoneView.FramesTarget(chaserFigure,11f))return false;
            if(!Phone.RitualCameraPower&&Phone.batteryPercent>0f)
                Phone.ConsumeBattery(Mathf.Min(Phone.batteryPercent,Phone.blitzCostPercent));
            pursuitStunUntil=Time.time+1.85f;dangerGrace=Mathf.Max(dangerGrace,2.1f);
            var pose=chaserFigure?chaserFigure.GetComponent<AtmajaThreatPresentation>():null;
            if(pose){pose.ExternalStun=true;if(pose.blitzSmoke)pose.blitzSmoke.Play();pose.EmitPhotoSmoke(24);}
            director.phoneView.CaptureFlash();return true;
        }
        public bool TryPhoto()
        {
            if (run != Run.WindowStandoff) return false;
            if (!Phone.isPhoneRaised || !Phone.isScreenOn) return false;
            if (!director.phoneView || !director.phoneView.IsCameraApp) return false;
            if (!director.phoneView.FramesTarget(chaserFigure,10f)) return false;
            if (Time.unscaledTime < photoReadyAt) return false;
            photoReadyAt = Time.unscaledTime + .3f;
            photosTaken++;standoffGrace=Mathf.Max(standoffGrace,Time.unscaledTime+1.25f);
            director.phoneView.CaptureFlash();
            var pose = chaserFigure ? chaserFigure.GetComponent<AtmajaThreatPresentation>() : null;
            PuffSmoke(photosTaken >= RequiredPhotos ? 70 : 34);
            if (pose)
            {
                pose.ExternalStun = true;
                pose.EmitPhotoSmoke(photosTaken >= RequiredPhotos ? 40 : 18);
                StartCoroutine(ReleasePhotoStun(pose,photosTaken));
            }
            if (director.phoneView)
            {
                string[][] lines =
                {
                    new[]{"FOTO PERTAMA KENA BANG!","ASAPNYA MULAI TEBEL!","DIA MUNDUR SEDIKIT!"},
                    new[]{"LAGI BANG SATU LAGI!","KAMERA JANGAN TURUN!","TUBUHNYA UDAH NGEROKOK!"},
                    new[]{"FOTO KETIGA! HABIS DIA!","DIA ILANG BANG!","JALUR KE JENDELA SUDAH KOSONG!"}
                };
                director.phoneView.AddChatBurst(new[]{"@rizky_pratama_","@kopi_susu99","@bocil_kematian"},lines[Mathf.Clamp(photosTaken-1,0,lines.Length-1)]);
            }
            if (photosTaken >= RequiredPhotos) StartCoroutine(CompletePhotoRitual());
            else director.Objective();
            return true;
        }
        private IEnumerator ReleasePhotoStun(AtmajaThreatPresentation pose,int shot)
        {
            yield return new WaitForSeconds(.28f);
            if (pose&&photosTaken==shot&&run==Run.WindowStandoff) pose.ExternalStun = false;
        }
        private IEnumerator CompletePhotoRitual()
        {
            run = Run.FinalFlash; Time.timeScale = 1;
            director.player.MovementLocked = true;
            Phone.batteryPercent = 10f;
            if (Campaign) Campaign.Data.battery = 10f;
            Phone.isScreenOn = true;
            Phone.SyncVisualState();
            Hud.ShowBroadcastNotification("FLASH BEAM! Ruangan terang sekejap. Baterai tersisa 10%.", 5);
            if(director.phoneView)director.phoneView.AddChatBurst(new[]{"@rizky_pratama_","@kopi_susu99","@bocil_kematian"},new[]{"FLASH BEAM GILA!","RUANGANNYA PUTIH BANG!","BATERAI TINGGAL 10%!"});
            PuffSmoke(90);
            var pose = chaserFigure ? chaserFigure.GetComponent<AtmajaThreatPresentation>() : null;
            if (pose) { pose.ExternalStun = true; pose.EmitPhotoSmoke(60); }
            StartCoroutine(finale.FlashBeam());
            StartCoroutine(FlashBeamLights());
            Ui(AtmajaSynth.Tone(1.1f, 1600, 40, .5f), .65f);
            Hud.PlayPriorityDialogue("Nyonya Sri Atmaja", "AAAAAARGH...", 1.6f);
            yield return new WaitForSecondsRealtime(.55f);
            if (chaserFigure) chaserFigure.SetActive(false);
            yield return new WaitForSecondsRealtime(.75f);
            director.player.MovementLocked = false;
            var rig=director.player.GetComponent<ShoulderCameraRig>();if(rig)rig.FirstPersonOverride=false;
            run = Run.EscapeReady;
            finale.Prompt(null);
            director.Objective();
        }
        private IEnumerator FlashBeamLights()
        {
            var ambientColour = RenderSettings.ambientLight;
            var ambientIntensity = RenderSettings.ambientIntensity;
            var burst = new GameObject("FinalFlashBeamBurst", typeof(Light));
            burst.transform.position = director.player.transform.position + Vector3.up * 1.65f;
            var point = burst.GetComponent<Light>();
            point.type = LightType.Point; point.range = 42f; point.intensity = 18f;
            point.color = new Color(1f, .97f, .90f); point.shadows = LightShadows.None;
            var windowBurst = new GameObject("FinalFlashBeamWindow", typeof(Light));
            windowBurst.transform.position = windowBlockSpot ? windowBlockSpot.position + Vector3.up * 1.4f : director.player.transform.position + director.north * 3f + Vector3.up * 1.4f;
            var windowPoint = windowBurst.GetComponent<Light>();
            windowPoint.type = LightType.Point; windowPoint.range = 32f; windowPoint.intensity = 14f;
            windowPoint.color = new Color(1f, .95f, .86f); windowPoint.shadows = LightShadows.None;
            RenderSettings.ambientLight = new Color(.82f, .82f, .78f);
            RenderSettings.ambientIntensity = 1.35f;
            for (float t = 0; t < 1f; t += Time.unscaledDeltaTime / 1.05f)
            {
                float fade = (1f - t) * (1f - t);
                point.intensity = 18f * fade;
                windowPoint.intensity = 14f * fade;
                RenderSettings.ambientIntensity = Mathf.Lerp(ambientIntensity, 1.35f, fade);
                yield return null;
            }
            RenderSettings.ambientLight = ambientColour;
            RenderSettings.ambientIntensity = ambientIntensity;
            if (burst) Destroy(burst);
            if (windowBurst) Destroy(windowBurst);
        }
        private void PuffSmoke(int count)
        {
            if(!photoSmoke)BuildPhotoSmoke();
            if(!photoSmoke)return;
            if(chaserFigure)photoSmoke.transform.position=chaserFigure.transform.position+Vector3.up*1.1f;
            photoSmoke.Emit(Mathf.Clamp(count,1,90));
        }
        private void BuildPhotoSmoke()
        {
            if(photoSmoke||!chaserFigure)return;
            var texture=Resources.Load<Texture2D>("PlayerPresentation/Smoke/whitePuff06");
            var shader=Shader.Find("Universal Render Pipeline/Particles/Unlit");
            if(!texture||!shader)return;
            photoSmokeMaterial=new Material(shader){name="Nyonya Photo Ritual Smoke"};
            photoSmokeMaterial.SetTexture("_BaseMap",texture);
            photoSmokeMaterial.SetColor("_BaseColor",new Color(.88f,.90f,.86f,.62f));
            photoSmokeMaterial.SetFloat("_Surface",1);photoSmokeMaterial.SetFloat("_Blend",0);
            photoSmokeMaterial.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            photoSmokeMaterial.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            photoSmokeMaterial.SetInt("_ZWrite",0);photoSmokeMaterial.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            photoSmokeMaterial.renderQueue=3000;
            var go=new GameObject("NyonyaPhotoSmoke",typeof(ParticleSystem));
            go.transform.SetParent(chaserFigure.transform,false);
            photoSmoke=go.GetComponent<ParticleSystem>();photoSmoke.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            var main=photoSmoke.main;main.loop=false;main.playOnAwake=false;main.duration=2.2f;
            main.startLifetime=new ParticleSystem.MinMaxCurve(1.2f,2.3f);
            main.startSpeed=new ParticleSystem.MinMaxCurve(.22f,.85f);
            main.startSize=new ParticleSystem.MinMaxCurve(.4f,1.25f);
            main.startRotation=new ParticleSystem.MinMaxCurve(0f,Mathf.PI*2f);
            main.startColor=new ParticleSystem.MinMaxGradient(new Color(.9f,.91f,.87f,.55f),new Color(.52f,.55f,.53f,.34f));
            main.maxParticles=260;main.simulationSpace=ParticleSystemSimulationSpace.World;main.gravityModifier=-.012f;
            var emission=photoSmoke.emission;emission.rateOverTime=0;
            var shape=photoSmoke.shape;shape.shapeType=ParticleSystemShapeType.Sphere;shape.radius=.24f;
            var noise=photoSmoke.noise;noise.enabled=true;noise.strength=.22f;noise.frequency=.28f;noise.scrollSpeed=.18f;noise.damping=true;
            var renderer=photoSmoke.GetComponent<ParticleSystemRenderer>();
            renderer.sharedMaterial=photoSmokeMaterial;renderer.renderMode=ParticleSystemRenderMode.Billboard;
            renderer.sortMode=ParticleSystemSortMode.Distance;renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;renderer.receiveShadows=false;
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
            if (Time.timeScale > 0 && Time.timeScale < 1f) Time.timeScale = 1;
            if (music && music.clip) Destroy(music.clip);
            if (photoSmokeMaterial) Destroy(photoSmokeMaterial);
        }
    }
}
