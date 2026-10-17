using System;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public sealed class AtmajaBasementHorror:MonoBehaviour
    {
        public const string PowerbankId="POWERBANK_BESAR";
        public const string HandycamId="ITEM_P0_HANDYCAM";
        private enum Mind { Dormant,Emerge,Patrol,Suspicion,Inspect,Pursue,Done }
        public AtmajaDirector director;
        public AtmajaUpperDirector upper;
        public AtmajaChaseSequence chase;
        public Transform spawnBasement;
        public Transform[] waypoints=new Transform[0];
        public Transform inspectAnchor1,inspectAnchor2;
        public GameObject nyonya,shearsProp,ragOriginal,teplokFlame;
        public Light teplokLight;
        public AudioSource humSource;
        public Transform hideAnchor1,hideAnchor2;
        public GameObject powerbankObj,handycamObj;
        public Collider wardrobeCol1,wardrobeCol2;
        public Transform powerbankSpot,handycamSpot;
        private Mind mind=Mind.Dormant;
        private int waypointIndex;
        private float waypointPause,detect,suspicionTimer,inspectTimer,giveUpTimer,dragTimer;
        private Vector3 suspicionPoint,inspectPoint;
        private bool lockerCheckRunning;
        private Transform hiddenIn;
        private ParticleSystem breath;
        private AudioClip scrapeClip;
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
            bool entered=Has("EVENT_P0_BASEMENT_ENTERED");
            bool played=Has("EVENT_P0_HANDYCAM_PLAYED");
            bool chased=Has("EVENT_P0_CHASE_STARTED");
            if(nyonya)nyonya.SetActive(entered&&!played);
            if(ragOriginal)ragOriginal.SetActive(entered&&!chased);
            if(teplokFlame)teplokFlame.SetActive(!played);
            if(teplokLight)teplokLight.enabled=!played;
            if(powerbankObj)powerbankObj.SetActive(entered&&!Has(PowerbankId)&&!Has("EVENT_P0_POWERBANK_TAKEN"));
            if(handycamObj)handycamObj.SetActive(entered&&!Has(HandycamId));
            if(humSource)humSource.enabled=entered&&!played;
            hiddenIn=null;mind=!entered?Mind.Dormant:played||chased?Mind.Done:Mind.Patrol;
            if(wardrobeCol1)wardrobeCol1.enabled=true;
            if(wardrobeCol2)wardrobeCol2.enabled=true;
            detect=0;suspicionTimer=0;lockerCheckRunning=false;
            if(director.player)director.player.MovementLocked=false;
            if(breath)Breath(false);
            if(nyonya&&waypoints.Length>0){nyonya.transform.position=waypoints[0].position;}
        }
        public string Objective()
        {
            if(hiddenIn)return "Tetap diam di dalam lemari. Matikan layar HP!";
            if(!Has("EVENT_P0_POWERBANK_TAKEN"))return "Ambil Powerbank Besar Damar di meja jahit sisi kiri. Jangan tertangkap.";
            if(!Has(HandycamId))return "Ambil Handycam Damar di depan cermin besar. Merangkaklah lewat kolong meja.";
            return "Putar rekaman terakhir di handycam.";
        }
        public void Handle(AtmajaItem item)
        {
            if(item.action==AtmajaAction.Powerbank)
            {
                if(Has("EVENT_P0_POWERBANK_TAKEN"))return;
                if(Commit("EVENT_P0_POWERBANK_TAKEN","PROLOG_BASEMENT_ENTRY",()=>{Campaign.AddInventoryItem(PowerbankId);}))
                {
                    // Scripted tutorial refill lands exactly on 80%, consuming the big pack.
                    Campaign.Data.inventory.Remove(PowerbankId);
                    Phone.batteryPercent=80;Campaign.Data.battery=80;Phone.isScreenOn=true;Phone.SyncVisualState();
                    RestoreWorld();Sfx(director.audioProfile.unlock,item.transform.position,.6f);
                    Hud.ShowBroadcastNotification("Powerbank Besar Damar diambil. Baterai 80%. Cadangan daya dipulihkan.",5);
                    Hud.EnqueueSubtitle("Raka","80 persen... Oke, masih bisa lanjut.",3);
                    director.phoneView.AddChat("@rudi_tabuti","ALHAMDULILLAH IDUP LAGI BANG");
                    director.Objective();
                }
            }
            else if(item.action==AtmajaAction.Handycam)
            {
                if(!Has("EVENT_P0_POWERBANK_TAKEN")){Hud.ShowBroadcastNotification("Ambil powerbank di meja jahit dulu. Baterai 12% tidak cukup memutar video.",5);return;}
                if(Has(HandycamId))return;
                if(Commit(HandycamId,"PROLOG_BASEMENT_ENTRY")){RestoreWorld();chase.PlayHandycamVideo();}
            }
            else if(item.action==AtmajaAction.WardrobeHide)
            {
                if(hiddenIn==null)EnterWardrobe(item.transform);
                else ExitWardrobe();
            }
        }
        private void Breath(bool on)
        {
            if(on&&!breath)
            {
                var cam=director.player.GetComponent<InteractionInteractor>().playerCamera;
                var shader=Shader.Find("Universal Render Pipeline/Particles/Unlit")??Shader.Find("Particles/Standard Unlit");
                if(!cam||!shader)return;
                var go=new GameObject("BreathVapor",typeof(ParticleSystem));go.transform.SetParent(cam.transform,false);
                breath=go.GetComponent<ParticleSystem>();var main=breath.main;
                main.loop=true;main.startLifetime=1.1f;main.startSpeed=.10f;main.startSize=new ParticleSystem.MinMaxCurve(.02f,.045f);
                main.startColor=new Color(1,1,1,.10f);main.simulationSpace=ParticleSystemSimulationSpace.Local;main.maxParticles=24;main.gravityModifier=-.012f;
                var emission=breath.emission;emission.rateOverTime=2.4f;
                var shape=breath.shape;shape.shapeType=ParticleSystemShapeType.Cone;shape.angle=16;shape.radius=.02f;shape.rotation=new Vector3(118,0,0);
                var renderer=go.GetComponent<ParticleSystemRenderer>();renderer.material=new Material(shader);renderer.material.SetColor("_BaseColor",new Color(1,1,1,.10f));
                renderer.shadowCastingMode=UnityEngine.Rendering.ShadowCastingMode.Off;renderer.receiveShadows=false;
                breath.Play();
            }
            if(breath){var emission=breath.emission;emission.enabled=on;}
        }
        private void EnterWardrobe(Transform wardrobe)
        {
            var anchor=Vector3.Distance(director.player.transform.position,hideAnchor1.position)<Vector3.Distance(director.player.transform.position,hideAnchor2.position)?hideAnchor1:hideAnchor2;
            hiddenIn=wardrobe;director.PlacePlayer(anchor.position,anchor.rotation);director.player.MovementLocked=true;
            var col=anchor==hideAnchor1?wardrobeCol1:wardrobeCol2;if(col)col.enabled=false;
            Hud.ShowBroadcastNotification("Bersembunyi di lemari. Tetap diam...",3);
        }
        private void ExitWardrobe()
        {
            var wardrobe=hiddenIn;
            if(wardrobeCol1)wardrobeCol1.enabled=true;
            if(wardrobeCol2)wardrobeCol2.enabled=true;
            hiddenIn=null;director.player.MovementLocked=false;
            var exit=(wardrobe?wardrobe.position:director.player.transform.position)+director.north*1.1f;exit.y=spawnBasement.position.y;
            director.PlacePlayer(exit,Quaternion.LookRotation(-director.north));
        }
        private void Update()
        {
            if(!director||!director.player||!Campaign||!Hud||director.Busy||UiFocus.HardModal)return;
            // Basement entry detection: capsule drops below the slab through the open trapdoor.
            if(director.Stage==AtmajaStage.MasterOpen&&director.player.transform.position.y<.2f)
            {
                if(!Commit("EVENT_P0_BASEMENT_ENTERED","PROLOG_BASEMENT_ENTRY"))return;
                // Scripted one-time power glitch: previous charge kept as reserve, indicator locked to 12%.
                if(!Has("EVENT_P0_POWER_GLITCH"))
                    Commit("EVENT_P0_POWER_GLITCH","PROLOG_BASEMENT_ENTRY",()=>{Campaign.AddInventoryItem("RESERVE_BATTERY_"+Mathf.RoundToInt(Phone.batteryPercent));Phone.batteryPercent=12;Campaign.Data.battery=12;});
                Phone.SyncVisualState();RestoreWorld();EnterBasementBeats();return;
            }
            // Cold cellar breath vapor while the stealth beat is live.
            Breath(director.Stage==AtmajaStage.BasementEntry||director.Stage==AtmajaStage.BasementStealth);
            if(mind==Mind.Dormant||mind==Mind.Done||!nyonya||!nyonya.activeSelf)return;
            if(hiddenIn==null&&director.player.MovementLocked)return;
            float dt=Mathf.Min(Time.deltaTime,.05f);
            dragTimer-=dt;
            if(dragTimer<=0){dragTimer=.62f;if(!scrapeClip)scrapeClip=AtmajaSynth.Scrape(.42f);Sfx(scrapeClip,nyonya.transform.position,.30f);}
            if(teplokLight&&teplokLight.enabled)teplokLight.intensity=1.35f+Mathf.Sin(Time.time*13)*.18f+Mathf.Sin(Time.time*47)*.07f;
            if(humSource)humSource.volume=Mathf.Clamp01(1.4f-Vector3.Distance(nyonya.transform.position,director.player.transform.position)/9f)*.5f;
            if(hiddenIn!=null){HiddenTick(dt);return;}
            SenseTick(dt);
            switch(mind)
            {
                case Mind.Patrol:PatrolTick(dt);break;
                case Mind.Suspicion:SuspicionTick(dt);break;
                case Mind.Inspect:InspectTick(dt);break;
                case Mind.Pursue:PursueTick(dt);break;
            }
            float dist=Vector3.Distance(nyonya.transform.position,director.player.transform.position);
            if(dist<1.15f){director.FailToCheckpoint("PROLOG_BASEMENT_ENTRY","Tertangkap! Nyonya Sri Atmaja menyeretmu ke dalam kegelapan.");}
        }
        private void EnterBasementBeats()
        {
            director.SetStage(AtmajaStage.BasementEntry);director.Objective();
            Hud.EnqueueSubtitle("Raka","Sial, ada gangguan daya! Gue harus cari powerbank cadangan di tas peralatan Damar atau HP ini mati total!",5);
            director.phoneView.AddChat("@rudi_tabuti","BANG BATERAINYA TINGGAL 12 PERSEN BANG!");
            director.phoneView.AddChat("@maya_sari","Jangan mati dulu bang kameranya plis gue takut banget liat ruangan bawahnya!");
            Hud.ShowBroadcastNotification("Penonton_Terakhir • SAWER Rp2.000.000\nDia sudah bangun. Jangan berani-berani memotret wajahnya kalau kau belum punya segel, atau lehermu yang akan menggantikan posisinya.",10);
            Campaign.AddBoundDonation(2000000);Ui(director.audioProfile.notification,.55f);
            StartCoroutine(Emerge());
        }
        private IEnumerator Emerge()
        {
            mind=Mind.Emerge;yield return new WaitForSeconds(1.2f);
            // The teplok lamp gutters out before she appears; the cellar goes black.
            if(teplokFlame)teplokFlame.SetActive(false);
            if(teplokLight)teplokLight.enabled=false;
            Ui(AtmajaSynth.Tone(.5f,240,90,.35f),.5f);
            yield return new WaitForSeconds(.85f);
            Hud.ShowBroadcastNotification("[BAHAYA KRITIS: ENTITAS BELUM TERIDENTIFIKASI — KEBAL TERHADAP SERANGAN KAMERA! SEMBUNYI DAN MATIKAN CAHAYA LAYAR!]",7);
            Ui(AtmajaSynth.Tone(1.4f,180,70,.5f),.6f);
            yield return new WaitForSeconds(1f);
            if(waypoints.Length>0)nyonya.transform.position=waypoints[0].position;
            mind=Mind.Patrol;director.SetStage(AtmajaStage.BasementStealth);director.Objective();
        }
        private float NoiseRadius()
        {
            var p=director.player;
            bool moving=(p.transform.position-_lastPos).sqrMagnitude>1e-6f;_lastPos=p.transform.position;
            if(!moving)return 0;
            if(p.IsSprinting)return 9;if(p.IsCrouching)return 1.6f;return 3.5f;
        }
        private Vector3 _lastPos;
        private bool CanSee(float range)
        {
            var from=nyonya.transform.position+Vector3.up*1.45f;
            var to=director.player.transform.position+Vector3.up*(director.player.IsCrouching?.6f:1.2f);
            float dist=Vector3.Distance(from,to);if(dist>range)return false;
            var dir=(to-from).normalized;
            if(Vector3.Dot(nyonya.transform.forward,dir)<.45f&&dist>1.6f)return false;
            int mask=~((1<<29)|(1<<2));if(Physics.Linecast(from,to,mask,QueryTriggerInteraction.Ignore))return false;
            return true;
        }
        private void SenseTick(float dt)
        {
            float dist=Vector3.Distance(nyonya.transform.position,director.player.transform.position);
            float range=Phone.isPhoneRaised&&Phone.isScreenOn?9f:5f;
            bool seen=CanSee(range);float noise=NoiseRadius();
            bool heard=noise>0&&dist<noise&&!(director.player.IsCrouching&&dist>noise*.5f);
            if(seen){detect+=dt*.9f;suspicionPoint=director.player.transform.position;}
            else if(heard){detect+=dt*.45f;suspicionPoint=director.player.transform.position;}
            else detect-=dt*.3f;
            detect=Mathf.Clamp01(detect);
            float flee=Vector3.Distance(suspicionPoint,director.player.transform.position);
            if(detect>=1&&mind!=Mind.Pursue){mind=Mind.Pursue;giveUpTimer=6f;Ui(AtmajaSynth.Tone(.9f,300,900,.5f),.6f);Hud.ShowBroadcastNotification("DIA MELIHATMU. LARI DAN SEMBUNYI!",3);}
            else if(detect>.35f&&mind==Mind.Patrol){mind=Mind.Suspicion;suspicionTimer=3f;}
            if(flee>9)detect=Mathf.Min(detect,detect-dt*.5f);
        }
        private void StepToward(Vector3 target,float speed,float dt)
        {
            var pos=nyonya.transform.position;var delta=target-pos;delta.y=0;
            if(delta.magnitude<.05f)return;
            nyonya.transform.rotation=Quaternion.Slerp(nyonya.transform.rotation,Quaternion.LookRotation(delta.normalized),dt*4);
            nyonya.transform.position=pos+delta.normalized*Mathf.Min(speed*dt,delta.magnitude);
            if(shearsProp)shearsProp.transform.rotation=Quaternion.Euler(Mathf.Sin(Time.time*4.2f)*14,0,0);
        }
        private void PatrolTick(float dt)
        {
            if(waypoints.Length==0)return;
            if(waypointPause>0){waypointPause-=dt;return;}
            var target=waypoints[waypointIndex%waypoints.Length];
            StepToward(target.position,1.05f,dt);
            if(Vector3.Distance(nyonya.transform.position,target.position)<.3f)
            {
                // Locker inspections at the archive aisle ends keep the wardrobes meaningful.
                if(target==inspectAnchor1||target==inspectAnchor2){mind=Mind.Inspect;inspectPoint=target.position;inspectTimer=4f;return;}
                waypointPause=1.2f+waypointIndex%2;waypointIndex++;
            }
        }
        private void SuspicionTick(float dt)
        {
            StepToward(suspicionPoint,1.5f,dt);suspicionTimer-=dt;
            if(Vector3.Distance(nyonya.transform.position,suspicionPoint)<.5f||suspicionTimer<=0){mind=Mind.Patrol;detect=Mathf.Min(detect,.3f);}
        }
        private void InspectTick(float dt)
        {
            nyonya.transform.rotation=Quaternion.Slerp(nyonya.transform.rotation,Quaternion.LookRotation((inspectPoint-nyonya.transform.position).normalized),dt*2);
            inspectTimer-=dt;if(inspectTimer<=0){mind=Mind.Patrol;waypointIndex++;}
        }
        private void PursueTick(float dt)
        {
            StepToward(director.player.transform.position,3.4f,dt);giveUpTimer-=dt;
            if(!CanSee(12)&&giveUpTimer<=0){mind=Mind.Patrol;detect=.25f;}
        }
        private void HiddenTick(float dt)
        {
            // Locker check: she stops at the wardrobe and peers through the slats for 4 seconds.
            float dist=Vector3.Distance(nyonya.transform.position,hiddenIn.position);
            if(dist<2.2f&&!lockerCheckRunning)StartCoroutine(LockerCheck());
            detect=Mathf.Max(0,detect-dt*.4f);
        }
        private IEnumerator LockerCheck()
        {
            lockerCheckRunning=true;
            Hud.ShowBroadcastNotification("Tekan [F] untuk MEMATIKAN LAYAR HP sekarang agar cahaya tidak bocor!",4);
            Ui(AtmajaSynth.Noise(4f,.12f),.5f);
            float t=0;bool failed=false;
            while(t<4f)
            {
                if(Phone.isScreenOn){failed=true;break;}
                t+=Time.deltaTime;yield return null;
            }
            lockerCheckRunning=false;
            if(failed){director.FailToCheckpoint("PROLOG_BASEMENT_ENTRY","Cahaya HP bocor dari celah lemari. Dia menemukanmu.");yield break;}
            Ui(director.audioProfile.unlock,.4f);
            if(hiddenIn!=null)ExitWardrobe();
        }
    }
}
