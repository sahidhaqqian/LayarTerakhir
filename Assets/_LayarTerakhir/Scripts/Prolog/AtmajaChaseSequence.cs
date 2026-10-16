using System;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LayarTerakhir.Prolog
{
    public sealed class AtmajaChaseSequence:MonoBehaviour
    {
        public const string ChaseCheckpoint="PROLOG_CHASE_START";
        private enum Run { Idle,Breaking,Pursuit,WindowStandoff,Stunned,GateRun,Ending }
        public AtmajaDirector director;
        public AtmajaBasementHorror basement;
        public Transform spawnChase,spawnStreet,windowBlockSpot,windowExitSpot,gateSpot,houseLookSpot;
        public Transform[] rail=new Transform[0];
        public GameObject slideGate,diningBlocker,windowContextLeaf,chaserFigure,streetFigure,endingTitle,bigMirror,mirrorShards;
        public Material mirrorCrackedMat;
        public Light lightning;
        public Transform handycamScreenSpot;
        private Run run=Run.Idle;
        private int railIndex;
        private float stunTimer,blitzWindow,slowmoLeft;
        private bool blitzFired,climbedOut;
        private Canvas cineCanvas;
        private Image barTop,barBottom;
        private Text cinePrompt;
        private AudioSource music;
        private Camera playerCamera;
        private float baseFov;
        private bool fovInit;
        private CampaignState Campaign=>CampaignState.Instance;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        private HudController Hud=>HudController.Instance;
        private bool Has(string id)=>Campaign&&Campaign.HasInventoryItem(id);
        private bool Commit(string id,string checkpoint,Action change=null)
        {
            if(Has(id))return true;
            string before=JsonUtility.ToJson(Campaign.Data);
            Campaign.AddInventoryItem(id);change?.Invoke();
            if(Campaign.SaveCheckpointAtomic(checkpoint))return true;
            JsonUtility.FromJsonOverwrite(before,Campaign.Data);Hud.ShowBroadcastNotification("Checkpoint gagal disimpan. Coba interaksi kembali.",4);return false;
        }
        private void Ui(AudioClip clip,float gain){if(director.notificationSource&&clip)director.notificationSource.PlayOneShot(clip,gain);}
        private void Sfx(AudioClip clip,Vector3 where,float gain){if(!director.foleySource||!clip)return;director.foleySource.transform.position=where;director.foleySource.PlayOneShot(clip,gain);}
        public void RestoreWorld()
        {
            bool started=Has("EVENT_P0_CHASE_STARTED"),done=Has("PROLOG_COMPLETE");
            run=Run.Idle;railIndex=0;stunTimer=0;blitzFired=false;climbedOut=!started||done;Time.timeScale=1;
            if(slideGate)slideGate.SetActive(false);
            if(diningBlocker)diningBlocker.SetActive(started&&!done);
            if(windowContextLeaf)windowContextLeaf.SetActive(true);
            if(streetFigure)streetFigure.SetActive(done);
            if(endingTitle)endingTitle.SetActive(false);
            if(bigMirror)bigMirror.SetActive(!started||done);
            if(mirrorShards)mirrorShards.SetActive(started&&!done);
            SetBars(false);SetPrompt(null);
            if(director.player)director.player.MovementLocked=false;
            if(!playerCamera&&director.player)playerCamera=director.player.GetComponent<InteractionInteractor>().playerCamera;
            if(playerCamera){if(!fovInit){baseFov=playerCamera.fieldOfView;fovInit=true;}playerCamera.fieldOfView=baseFov;}
            // A caught runner (or a reload mid-chase) must resume the pursuit, not freeze the hunter.
            if(started&&!done)ResumeChase();
            else{if(chaserFigure)chaserFigure.SetActive(false);Music(false);}
        }
        private void ResumeChase()
        {
            run=Run.Breaking;railIndex=0;blitzFired=false;climbedOut=false;stunTimer=0;Time.timeScale=1;
            if(chaserFigure){chaserFigure.SetActive(true);if(rail.Length>0)chaserFigure.transform.position=rail[0].position;}
            Music(true);
            StartCoroutine(BreakBeat());
        }
        private void Music(bool on)
        {
            if(on&&!music)
            {
                var go=new GameObject("ChaseMusic");go.transform.SetParent(director.transform,false);
                music=go.AddComponent<AudioSource>();music.playOnAwake=false;music.loop=true;music.spatialBlend=0;music.volume=.40f;
                music.clip=AtmajaSynth.ChaseLoop();music.outputAudioMixerGroup=director.audioProfile.effectsBus;music.Play();
            }
            else if(!on&&music)music.Stop();
        }
        public string Objective()
        {
            return run switch
            {
                Run.WindowStandoff=>"TEMBAKKAN FLASH KAMERA SEKARANG! [KLIK KIRI]",
                Run.Stunned=>"Dia terpaku 4 detik. LARI LEWAT JENDELA!",
                Run.GateRun=>"Lari menembus gerbang besi ke jalan raya!",
                Run.Ending=>"Prolog Layar Terakhir selesai.",
                _=>"LARI! Jangan berhenti! Ikuti jalur ke jendela dapur!",
            };
        }
        public void PlayHandycamVideo()
        {
            if(!Commit("EVENT_P0_HANDYCAM_PLAYED","PROLOG_BASEMENT_ENTRY"))return;
            director.SetStage(AtmajaStage.HandycamPlayed);StartCoroutine(Playback());
        }
        private IEnumerator Playback()
        {
            director.SetBusy(true);SetBars(true);
            Hud.EnqueueSubtitle("Damar (rekaman)","Kacanya... kacanya bukan mantulin ruangan ini, Ka! Di dalem kaca itu ada pasar malam... mereka narik kaki gue—RAKA TOLONGIN GUE!",9);
            Ui(AtmajaSynth.Noise(6f,.3f),.5f);
            yield return new WaitForSeconds(6f);
            SetBars(false);director.SetBusy(false);
            if(!Commit("EVENT_P0_CHASE_STARTED",ChaseCheckpoint))yield break;
            StartChase();
        }
        public void StartChase()
        {
            director.SetStage(AtmajaStage.Chase);
            if(bigMirror)bigMirror.SetActive(false);
            if(mirrorShards)mirrorShards.SetActive(true);
            Sfx(AtmajaSynth.Noise(1.2f,.7f),director.player.transform.position,.8f);
            Ui(AtmajaSynth.Tone(1f,200,1200,.5f),.6f);
            Hud.EnqueueSubtitle("Nyonya Sri Atmaja","AAAAAARGH!!!",3);
            director.phoneView.AddChat("@agung_gaming","LARI BANG LARI JANGAN NOLEH BELAKANG");
            if(slideGate)slideGate.SetActive(true);
            ResumeChase();
        }
        private IEnumerator BreakBeat()
        {
            yield return new WaitForSeconds(1.6f);
            if(run==Run.Breaking){run=Run.Pursuit;director.Objective();}
        }
        private IEnumerator ClimbOut()
        {
            climbedOut=true;director.player.MovementLocked=true;
            var from=director.player.transform.position;
            var top=director.upper&&director.upper.trapdoorTop?director.upper.trapdoorTop.position:from+Vector3.up*6.6f;
            float t=0;var cc=director.player.GetComponent<CharacterController>();cc.enabled=false;
            while(t<1){t+=Time.deltaTime/1.4f;director.player.transform.position=Vector3.Lerp(from,top,Mathf.SmoothStep(0,1,t));yield return null;}
            cc.enabled=true;director.player.MovementLocked=false;Physics.SyncTransforms();
        }
        private void SetBars(bool on)
        {
            if(!cineCanvas)
            {
                cineCanvas=new GameObject("AtmajaCinematicBars").AddComponent<Canvas>();
                cineCanvas.renderMode=RenderMode.ScreenSpaceOverlay;cineCanvas.sortingOrder=60;
                barTop=Bar("BarTop",1);barBottom=Bar("BarBottom",0);
                var promptGo=new GameObject("CinePrompt",typeof(RectTransform),typeof(Text));promptGo.transform.SetParent(cineCanvas.transform,false);
                cinePrompt=promptGo.GetComponent<Text>();cinePrompt.font=Hud.objectiveText?Hud.objectiveText.font:null;
                cinePrompt.fontSize=34;cinePrompt.alignment=TextAnchor.MiddleCenter;cinePrompt.color=Color.red;
                var rect=cinePrompt.GetComponent<RectTransform>();rect.anchorMin=new Vector2(0,.3f);rect.anchorMax=new Vector2(1,.7f);rect.offsetMin=rect.offsetMax=Vector2.zero;
            }
            barTop.enabled=barBottom.enabled=on;
            cineCanvas.enabled=on||cinePrompt.text.Length>0;
        }
        private Image Bar(string name,float top)
        {
            var go=new GameObject(name,typeof(RectTransform),typeof(Image));go.transform.SetParent(cineCanvas.transform,false);
            var rect=go.GetComponent<RectTransform>();rect.anchorMin=new Vector2(0,top>.5f?.86f:0);rect.anchorMax=new Vector2(1,top>.5f?1:.14f);rect.offsetMin=rect.offsetMax=Vector2.zero;
            var image=go.GetComponent<Image>();image.color=Color.black;return image;
        }
        private void SetPrompt(string text){if(cinePrompt){cinePrompt.text=text??"";if(cineCanvas)cineCanvas.enabled=barTop.enabled||cinePrompt.text.Length>0;}}
        private void Update()
        {
            if(!director||!director.player||!Campaign||!Hud||director.Busy||UiFocus.HardModal)return;
            if(run==Run.Idle||run==Run.Ending)return;
            float dt=Mathf.Min(Time.deltaTime,.05f);
            float playerDist=chaserFigure?Vector3.Distance(chaserFigure.transform.position,director.player.transform.position):99f;
            if(run==Run.Pursuit||run==Run.GateRun)
            {
                if(playerDist<1.15f){director.FailToCheckpoint(ChaseCheckpoint,"Tertangkap! Guntingnya menebas lehermu.");return;}
            }
            switch(run)
            {
                case Run.Pursuit:PursuitTick(dt);break;
                case Run.WindowStandoff:StandoffTick();break;
                case Run.Stunned:StunnedTick(dt);break;
                case Run.GateRun:GateTick(dt);break;
            }
            // Nightmare corridor: the upper hallway stretches with a breathing lens while she runs it.
            if(!playerCamera)playerCamera=director.player.GetComponent<InteractionInteractor>().playerCamera;
            if(playerCamera)
            {
                if(!fovInit){baseFov=playerCamera.fieldOfView;fovInit=true;}
                bool corridor=(run==Run.Pursuit||run==Run.Stunned)&&director.player.transform.position.y>3.2f;
                float target=corridor?baseFov+1.9f+Mathf.Sin(Time.time*2.4f)*1.5f:baseFov;
                playerCamera.fieldOfView=Mathf.Lerp(playerCamera.fieldOfView,target,dt*3f);
            }
            if(run==Run.Pursuit&&Vector3.Distance(director.player.transform.position,windowExitSpot.position)<3.2f)BeginStandoff();
        }
        private void PursuitTick(float dt)
        {
            if(!chaserFigure||rail.Length==0)return;
            // Rail chase: deterministic, corner-safe, and always slightly slower than a sprint.
            while(railIndex<rail.Length-1&&Vector3.Distance(chaserFigure.transform.position,rail[railIndex].position)<.6f)railIndex++;
            var target=rail[Mathf.Min(railIndex+1,rail.Length-1)].position;
            var delta=target-chaserFigure.transform.position;delta.y=0;
            float speed=run==Run.GateRun?3.6f:4.3f;
            if(delta.magnitude>.05f)
            {
                chaserFigure.transform.rotation=Quaternion.Slerp(chaserFigure.transform.rotation,Quaternion.LookRotation(delta.normalized),dt*5);
                float floorY=director.player.transform.position.y;
                var next=chaserFigure.transform.position+delta.normalized*Mathf.Min(speed*dt,delta.magnitude);
                next.y=Mathf.MoveTowards(chaserFigure.transform.position.y,floorY,dt*3);
                chaserFigure.transform.position=next;
            }
            // Trapdoor slide gate: the closing slab blocks standing capsules only.
            if(slideGate&&slideGate.activeSelf)
            {
                if(Vector3.Distance(director.player.transform.position,slideGate.transform.position)>2.4f)slideGate.SetActive(false);
                else if(director.player.transform.position.y>3f&&!director.player.IsCrouching)
                {
                    Hud.ShowBroadcastNotification("Pintu jebakan menutup! Meluncur [CTRL] atau merunduk lewat celah!",2);
                }
            }
            // Cellar climb-out during the pursuit: scripted traverse up the steep stone steps.
            if(run==Run.Pursuit&&!climbedOut&&basement&&basement.spawnBasement)
            {
                var base_=basement.spawnBasement.position+new Vector3(0,0,1.2f);
                if(Vector3.Distance(director.player.transform.position,base_)<1.4f)StartCoroutine(ClimbOut());
            }
            // Dining barricade vault prompt.
            if(diningBlocker&&diningBlocker.activeSelf&&Vector3.Distance(director.player.transform.position,diningBlocker.transform.position)<2.6f)
            {
                Hud.SetInteractionPrompt($"[{GameInputConfig.Instance.GetKeyForAction(GameAction.Vault).ToString().ToUpperInvariant()}] Lompat meja yang roboh");
                if(GameInputConfig.Instance.GetActionDown(GameAction.Vault)){diningBlocker.SetActive(false);Sfx(director.audioProfile.latch, diningBlocker.transform.position,.6f);}
            }
            // Shatter corridor lamps as the player crosses them.
            foreach(var lamp in GameObject.FindGameObjectsWithTag("AtmajaChaseLamp"))
            {
                if(Vector3.Distance(director.player.transform.position,lamp.transform.position)<2.4f)
                {lamp.tag="Untagged";StartCoroutine(ShatterLamp(lamp));}
            }
        }
        private IEnumerator ShatterLamp(GameObject lamp)
        {
            Sfx(AtmajaSynth.Noise(.5f,.6f),lamp.transform.position,.7f);
            foreach(var light in lamp.GetComponentsInChildren<Light>())light.enabled=false;
            lamp.transform.position+=Vector3.down*.5f;yield return new WaitForSeconds(.4f);lamp.SetActive(false);
        }
        private void BeginStandoff()
        {
            run=Run.WindowStandoff;blitzWindow=3f;slowmoLeft=3f;blitzFired=false;
            Time.timeScale=.15f;
            Phone.isPhoneRaised=true;Phone.SyncVisualState();
            if(chaserFigure&&windowBlockSpot){chaserFigure.transform.SetPositionAndRotation(windowBlockSpot.position,windowBlockSpot.rotation);}
            Ui(AtmajaSynth.Tone(2.8f,400,2400,.4f),.6f);
            SetPrompt("[TEKAN KLIK KIRI MOUSE — TEMBAKKAN FLASH KAMERA SEKARANG!]");
            director.Objective();
        }
        private void StandoffTick()
        {
            blitzWindow-=Time.unscaledDeltaTime;slowmoLeft-=Time.unscaledDeltaTime;
            if(slowmoLeft<=0)Time.timeScale=1;
            if(GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz)&&!blitzFired)
            {
                blitzFired=true;
                if(Phone.batteryPercent<Phone.blitzCostPercent){Hud.ShowBroadcastNotification("Baterai tidak cukup untuk Blitz!",3);}
                else
                {
                    Phone.ConsumeBattery(Phone.blitzCostPercent);
                    Sfx(AtmajaSynth.Noise(.8f,.9f),director.player.transform.position,1f);
                    Ui(AtmajaSynth.Tone(.6f,2000,300,.6f),.7f);
                    run=Run.Stunned;stunTimer=4f;Time.timeScale=1;SetPrompt(null);
                    if(windowContextLeaf){var collider=windowContextLeaf.GetComponent<Collider>();if(collider)collider.enabled=false;windowContextLeaf.SetActive(false);}
                    Hud.ShowBroadcastNotification("Blitz mengenai wajahnya! Dia terpaku 4 detik!",3);
                    director.Objective();return;
                }
            }
            if(blitzWindow<=0&&!blitzFired){Time.timeScale=1;SetPrompt(null);director.FailToCheckpoint(ChaseCheckpoint,"Terlambat menembakkan Blitz. Dia menyeretmu kembali.");}
            else if(blitzWindow<=0&&blitzFired){Time.timeScale=1;SetPrompt(null);director.FailToCheckpoint(ChaseCheckpoint,"Blitz gagal. Dia menyeretmu kembali.");}
        }
        private void StunnedTick(float dt)
        {
            stunTimer-=dt;
            if(Vector3.Distance(director.player.transform.position,windowExitSpot.position)<1.2f&&director.player.transform.position.y<1.2f)
            {
                run=Run.GateRun;director.Objective();
                Hud.ShowBroadcastNotification("Raka berguling keluar ke halaman samping! LARI KE GERBANG!",4);
                Sfx(director.audioProfile.latch,director.player.transform.position,.7f);
            }
            else if(stunTimer<=0){run=Run.Pursuit;director.Objective();}
        }
        private void GateTick(float dt)
        {
            PursuitTick(dt);
            if(run!=Run.GateRun)return;
            if(gateSpot&&Vector3.Distance(director.player.transform.position,gateSpot.position)<2.4f)StartCoroutine(Ending());
        }
        private IEnumerator Ending()
        {
            run=Run.Ending;Time.timeScale=1;director.SetStage(AtmajaStage.EndingVista);
            Music(false);
            director.player.MovementLocked=true;SetBars(true);
            director.PlacePlayer(spawnStreet.position,spawnStreet.rotation);
            Hud.EnqueueSubtitle("Raka","*terengah-engah* ...Gue... selamat... Damar... gue bakal balik buat lo...",6);
            yield return new WaitForSeconds(2f);
            // Look back: silhouette in the dark second-floor window, under lightning.
            if(houseLookSpot)director.player.transform.rotation=houseLookSpot.rotation;
            if(streetFigure)streetFigure.SetActive(true);
            for(int i=0;i<3;i++)
            {
                if(lightning){lightning.intensity=8;lightning.transform.rotation=Quaternion.Euler(50,-30+i*20,0);}
                Sfx(AtmajaSynth.Noise(1.6f,.8f,22050,7000u+(uint)i),director.player.transform.position,.8f);
                yield return new WaitForSeconds(.28f);
                if(lightning)lightning.intensity=0;
                yield return new WaitForSeconds(i==2?1.2f:.7f);
            }
            Phone.isBroadcastConnected=true;
            long target=25450000,delta=target-Campaign.Data.wallet_bound;
            if(delta>0&&Commit("EVENT_P0_FINAL_SAWER","PROLOG_COMPLETE",()=>Campaign.AddBoundDonation(delta)))
                Hud.ShowBroadcastNotification($"Penonton_Terakhir • SAWER Rp{delta:N0}\nSatu malam selesai.",8);
            director.phoneView.AddChat("@viral_horor_id","GILA INI LIVE TERGILA TAHUN INI");
            director.phoneView.AddChat("@kuyang_watch","ITU TADI BENERAN HANTU WOI");
            director.phoneView.AddChat("@tiktok_repost","VIRALIN KE TIKTOK SEKARANG!");
            yield return new WaitForSeconds(2.5f);
            Hud.OpenDocumentModal("PENONTON_TERAKHIR","\"Satu malam selesai. Empat segel harus kau kumpulkan sebelum gerbang kelima terbuka. Sekarang kau tahu di mana sahabatmu berada. Kalau kau berani mematikan siaranmu besok malam, kepala Damar yang akan tampil di layar kameramu. Istirahatlah di kosmu... Besok malam pukul 23:59, kita bertemu di Rusunawa Terbengkalai Griya Asri.\"",null);
            yield return new WaitUntil(()=>!Hud.IsModalOpen);
            // Heavy analog shutter before the title drop.
            Sfx(AtmajaSynth.Shutter(),director.player.transform.position,1f);
            Sfx(AtmajaSynth.Noise(.35f,.9f),director.player.transform.position,1f);
            yield return Hud.FadeBlack(0,1,1.2f);
            if(endingTitle)endingTitle.SetActive(true);
            yield return new WaitForSeconds(4f);
            if(Commit("PROLOG_COMPLETE","PROLOG_COMPLETE"))director.Objective();
            SetBars(false);director.player.MovementLocked=false;
        }
    }
}
