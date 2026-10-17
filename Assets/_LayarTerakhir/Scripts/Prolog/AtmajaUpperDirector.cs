using System;
using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public static class AtmajaSynth
    {
        public static AudioClip Tone(float seconds,float fromHz,float toHz,float gain,int rate=22050)
        {
            var samples=new float[(int)(seconds*rate)];double phase=0;
            for(int i=0;i<samples.Length;i++)
            {
                double t=i/(double)rate;double f=fromHz+(toHz-fromHz)*(i/(double)samples.Length);
                phase+=2*Math.PI*f/rate;samples[i]=(float)(Math.Sin(phase)*gain*Math.Sin(Math.PI*i/samples.Length));
            }
            var clip=AudioClip.Create("AtmajaSynth_Tone",samples.Length,1,rate,false);clip.SetData(samples,0);return clip;
        }
        public static AudioClip Noise(float seconds,float gain,int rate=22050,uint seed=1998)
        {
            var samples=new float[(int)(seconds*rate)];var random=new System.Random((int)seed);
            for(int i=0;i<samples.Length;i++){double t=i/(double)samples.Length;samples[i]=(float)((random.NextDouble()*2-1)*gain*Math.Sin(Math.PI*Math.Min(1,t*1.15)));}
            var clip=AudioClip.Create("AtmajaSynth_Noise",samples.Length,1,rate,false);clip.SetData(samples,0);return clip;
        }
        public static AudioClip Melody(float[] freqs,float noteSeconds,float gain,int rate=22050)
        {
            int per=(int)(noteSeconds*rate);var samples=new float[per*freqs.Length];double phase=0;
            for(int n=0;n<freqs.Length;n++)for(int i=0;i<per;i++)
            {
                double t=i/(double)per;phase+=2*Math.PI*freqs[n]/rate;
                samples[n*per+i]=(float)((Math.Sin(phase)+.4*Math.Sin(phase*2.01))*.5*gain*Math.Sin(Math.PI*Math.Min(1,t*1.2)));
            }
            var clip=AudioClip.Create("AtmajaSynth_Melody",samples.Length,1,rate,false);clip.SetData(samples,0);return clip;
        }
        public static AudioClip Creak(float seconds,int rate=22050)
        {
            // Rope/timber groan: a slow friction bend with a gritty low rub.
            int n=(int)(seconds*rate);var samples=new float[n];var random=new System.Random(7788);double phase=0;
            for(int i=0;i<n;i++)
            {
                double t=i/(double)n;double f=172+58*Math.Sin(i*.0045);phase+=2*Math.PI*f/rate;
                double wobble=.52+.42*Math.Sin(i*.017)+.14*Math.Sin(i*.091);
                double grit=(random.NextDouble()*2-1)*.09;
                samples[i]=(float)(((Math.Sin(phase)*.5+Math.Sin(phase*2.17)*.18)*wobble*.5+grit)*Math.Sin(Math.PI*Math.Min(1,t*1.12)));
            }
            var clip=AudioClip.Create("AtmajaSynth_Creak",n,1,rate,false);clip.SetData(samples,0);return clip;
        }
        public static AudioClip Drip(int rate=22050)
        {
            // Single ceramic drip: fast downward blip with a tiny tail.
            int n=(int)(.14f*rate);var samples=new float[n];double phase=0;
            for(int i=0;i<n;i++)
            {
                double t=i/(double)n;double f=1380-980*t;phase+=2*Math.PI*f/rate;
                samples[i]=(float)(Math.Sin(phase)*Math.Exp(-9*t)*.65);
            }
            var clip=AudioClip.Create("AtmajaSynth_Drip",n,1,rate,false);clip.SetData(samples,0);return clip;
        }
        public static AudioClip Scrape(float seconds,int rate=22050)
        {
            // Rusted shears dragged across stone: resonant metal rasp over grit.
            int n=(int)(seconds*rate);var samples=new float[n];var random=new System.Random(4242);double phase=0;
            for(int i=0;i<n;i++)
            {
                double t=i/(double)n;double f=1620+270*Math.Sin(i*.021);phase+=2*Math.PI*f/rate;
                double env=Math.Sin(Math.PI*Math.Min(1,t*1.15));
                samples[i]=(float)((((random.NextDouble()*2-1)*.48)+(Math.Sin(phase)*.22))*env);
            }
            var clip=AudioClip.Create("AtmajaSynth_Scrape",n,1,rate,false);clip.SetData(samples,0);return clip;
        }
        public static AudioClip Shutter(int rate=22050)
        {
            // Heavy analog camera shutter: two mechanical clacks.
            int n=(int)(.5f*rate);var samples=new float[n];var random=new System.Random(90210);
            void Click(float at,float dur,float gain)
            {
                int start=(int)(at*rate),len=(int)(dur*rate);
                for(int i=0;i<len&&start+i<n;i++){float t=i/(float)len;samples[start+i]+=((float)random.NextDouble()*2-1)*(float)Math.Exp(-26*t)*gain;}
            }
            Click(0,.06f,.85f);Click(.09f,.09f,.6f);
            var clip=AudioClip.Create("AtmajaSynth_Shutter",n,1,rate,false);clip.SetData(samples,0);return clip;
        }
        public static AudioClip ChaseLoop(int rate=22050,float bpm=150)
        {
            // Original kendang + saron chase bed with a glitch-siren wail. Two 16-step bars.
            float step=60f/bpm/4f;const int steps=32;int n=(int)(steps*step*rate)+rate/2;
            var data=new float[n];var random=new System.Random(1506);
            void Add(float at,float freq,float dur,float gain,float noise,float drop)
            {
                int start=(int)(at*rate),len=(int)(dur*rate);double phase=0;
                for(int i=0;i<len&&start+i<n;i++)
                {
                    float t=i/(float)len;double f=freq*(1+(drop-1)*t);phase+=2*Math.PI*f/rate;
                    float env=(float)Math.Exp(-4.5*t)*(1-t);
                    float s=(float)Math.Sin(phase)*(1-noise)+(float)(random.NextDouble()*2-1)*noise;
                    data[start+i]+=s*env*gain;
                }
            }
            void Metal(float at,float freq,float dur,float gain)
            {
                int start=(int)(at*rate),len=(int)(dur*rate);double p1=0,p2=0,p3=0;
                for(int i=0;i<len&&start+i<n;i++)
                {
                    float t=i/(float)len;float env=(float)Math.Exp(-6*t);
                    p1+=2*Math.PI*freq/rate;p2+=2*Math.PI*freq*2.76/rate;p3+=2*Math.PI*freq*5.4/rate;
                    data[start+i]+=(float)(Math.Sin(p1)+.45*Math.Sin(p2)+.22*Math.Sin(p3))*env*gain;
                }
            }
            void Siren(float at,float dur,float from,float to,float gain)
            {
                int start=(int)(at*rate),len=(int)(dur*rate);double phase=0;
                for(int i=0;i<len&&start+i<n;i++)
                {
                    float t=i/(float)len;double f=from+(to-from)*t;phase+=2*Math.PI*f/rate;
                    data[start+i]+=(float)Math.Sin(phase)*(float)Math.Sin(Math.PI*t)*gain;
                }
            }
            foreach(int s in new[]{0,3,6,10,16,19,22,26})Add(s*step,92,0.22f,.55f,.25f,.62f);
            foreach(int s in new[]{4,7,12,14,20,23,28,30})Add(s*step,235,0.07f,.30f,.60f,.80f);
            foreach(int s in new[]{1,3,5,7,9,11,13,15,17,19,21,23,25,27,29,31})Add(s*step,0,0.03f,.05f,1f,1f);
            float[] saron={392,466.2f,523.3f,466.2f,587.3f,523.3f,466.2f,392};
            int[] saronStep={2,5,8,11,18,21,24,27};
            for(int i=0;i<saron.Length;i++)Metal(saronStep[i]*step,saron[i],.5f,.16f);
            Siren(0,1.0f,320,880,.07f);Siren(16*step,1.0f,360,920,.07f);
            float peak=0;for(int i=0;i<n;i++)peak=Mathf.Max(peak,Mathf.Abs(data[i]));
            if(peak>1)for(int i=0;i<n;i++)data[i]/=peak;
            var clip=AudioClip.Create("AtmajaSynth_ChaseLoop",n,1,rate,false);clip.SetData(data,0);return clip;
        }
    }

    public sealed class AtmajaUpperDirector:MonoBehaviour
    {
        public const string DiaryId="DOC_PROLOG_04_DIARY_BIMA";
        public const string DiaryText="Buku harian Bima, anak bungsu. Tulisannya gemetar.\n\n\"Ibu akhir-akhir ini sering bicara sendiri di ruang jahit bawah tanah sambil memegang gunting taman. Malam ini Ibu membuatkan aku susu hangat, tapi baunya menyengat seperti kapur barus dan karat. Ibu bilang kalau aku minum ini, kami semua tidak akan pernah sakit lagi.\"";
        public const string WillId="DOC_PROLOG_05_WASIAT_NYONYA";
        public const string WillText="Surat wasiat Nyonya Sri Atmaja, tinta cokelat kehitaman.\n\n\"Suamiku berbohong. Utang keluarga kita tidak pernah lunas, dan Mbah Suro bilang rumah ini adalah pintu cermin yang menghubungkan rumah ini ke lima gerbang Panca-Sengkala. Aku sudah menyelamatkan anak-anakku lebih dulu. Sekarang giliranku menjaga rumah ini sampai Penonton Terakhir datang menjemput.\"";
        public AtmajaDirector director;
        public Transform spawnUpper,trapdoorTop;
        public HingedDoor childDoor,bathDoor,masterDoor;
        public Collider ventGate;
        public GameObject[] chainLinks=new GameObject[3];
        public GameObject childCup,bathCurtain,bathWater,mirrorFlashFigure,hangedFlashFigure,carpetRoll,trapdoorLid;
        public GameObject ventCabinet;
        public Transform ropePivot;
        private Quaternion ropeBase;
        private bool ropeInit;
        private AudioClip creakClip,dripClip;
        private float creakTimer,dripTimer;
        public GameObject ventHoleAR,bathScratchesAR;
        public AudioSource doorSong;
        public GameObject[] portraitBloodAR=new GameObject[3];
        public Transform cupAim,scratchAim,ropeAim,ventMouth,mirrorSpot,ropeSpot;
        private CampaignState Campaign=>CampaignState.Instance;
        private SmartphoneController Phone=>SmartphoneController.Instance;
        private HudController Hud=>HudController.Instance;
        private bool Has(string id)=>Campaign&&Campaign.HasInventoryItem(id);
        private bool ArActive=>Phone&&Phone.arModuleUnlocked&&Phone.isPhoneRaised&&Phone.isScreenOn&&Phone.currentScanMode==ScanVisualMode.AR&&director.phoneView&&director.phoneView.IsCameraApp;
        public int ChainsBroken=>(Has("EVENT_P0_CHAIN1_BROKEN")?1:0)+(Has("EVENT_P0_CHAIN2_BROKEN")?1:0)+(Has("EVENT_P0_CHAIN3_BROKEN")?1:0);
        private bool Commit(string id,string checkpoint,Action change=null)
        {
            if(Has(id))return true;
            string before=JsonUtility.ToJson(Campaign.Data);
            Campaign.AddInventoryItem(id);change?.Invoke();
            if(Campaign.SaveCheckpointAtomic(checkpoint))return true;
            JsonUtility.FromJsonOverwrite(before,Campaign.Data);Hud.ShowBroadcastNotification("Checkpoint gagal disimpan. Coba interaksi kembali.",4);return false;
        }
        private void Sfx(AudioClip clip,Vector3 where,float gain){if(!director.foleySource||!clip)return;director.foleySource.transform.position=where;director.foleySource.PlayOneShot(clip,gain);}
        private void Ui(AudioClip clip,float gain){if(director.notificationSource&&clip)director.notificationSource.PlayOneShot(clip,gain);}
        public void RestoreWorld()
        {
            bool c1=Has("EVENT_P0_CHAIN1_BROKEN"),c2=Has("EVENT_P0_CHAIN2_BROKEN"),c3=Has("EVENT_P0_CHAIN3_BROKEN");
            for(int i=0;i<chainLinks.Length;i++)if(chainLinks[i])chainLinks[i].SetActive(!(i==0?c1:i==1?c2:c3));
            if(childCup)childCup.SetActive(!c1);
            if(childDoor)childDoor.locked=!c1;
            if(masterDoor){masterDoor.locked=!c3;masterDoor.hinge.localRotation=Quaternion.Euler(0,c3?-95:0,0);}
            if(bathCurtain)bathCurtain.SetActive(!Has("EVENT_P0_CURTAIN_OPEN"));
            if(bathWater)bathWater.SetActive(Has("EVENT_P0_CURTAIN_OPEN"));
            if(carpetRoll)carpetRoll.SetActive(!Has("EVENT_P0_CARPET_LIFTED"));
            if(trapdoorLid)trapdoorLid.SetActive(Has("EVENT_P0_CARPET_LIFTED")&&!Has("EVENT_P0_TRAPDOOR_OPEN"));
            if(ventGate)ventGate.enabled=true;
            if(mirrorFlashFigure)mirrorFlashFigure.SetActive(false);
            if(hangedFlashFigure)hangedFlashFigure.SetActive(false);
            if(ventHoleAR)ventHoleAR.SetActive(false);
            if(bathScratchesAR)bathScratchesAR.SetActive(false);
            foreach(var p in portraitBloodAR)if(p)p.SetActive(false);
        }
        public string Objective()
        {
            int n=ChainsBroken;
            if(director.Stage==AtmajaStage.StairLanding)return "Naiki tangga. Ikuti jejak tangan berdarah ke lantai 2.";
            if(director.Stage==AtmajaStage.MasterOpen)return Has("EVENT_P0_CARPET_LIFTED")?(Has("EVENT_P0_TRAPDOOR_OPEN")?"Turuni tangga batu ke ruang bawah tanah.":"Buka pintu jebakan di bawah karpet."):"Masuki Kamar Tidur Utama. Periksa karpet merah yang tergulung.";
            return n>=3?"Kembali ke Pintu Kamar Utama yang terbuka.":$"Potret 3 Bukti Tragedi dengan Kamera AR ({n}/3).";
        }
        private void Update()
        {
            if(!director||!director.player||!Campaign||!Hud||director.Busy||UiFocus.HardModal)return;
            if(doorSong)
            {
                bool sing=Has("EVENT_P0_UPPER_REACHED")&&!Has("EVENT_P0_MASTER_OPEN")&&director.Stage<AtmajaStage.BasementEntry;
                if(doorSong.enabled!=sing)doorSong.enabled=sing;
            }
            bool ar=ArActive;
            // Upper-floor AR ink only appears while the player is actually upstairs, so it cannot
            // bleed through the floor and be read from the ground level.
            bool upstairs=director.Stage>=AtmajaStage.StairLanding&&director.Stage<=AtmajaStage.MasterOpen;
            if(ventHoleAR)ventHoleAR.SetActive(ar&&upstairs&&!Has("EVENT_P0_CHAIN1_BROKEN"));
            if(bathScratchesAR)bathScratchesAR.SetActive(ar&&upstairs&&Has("EVENT_P0_CHAIN1_BROKEN")&&!Has("EVENT_P0_CHAIN2_BROKEN"));
            for(int i=0;i<portraitBloodAR.Length;i++)if(portraitBloodAR[i])portraitBloodAR[i].SetActive(ar&&upstairs);
            // Stair landing detection: the pocket door is open and the capsule climbs past the slab.
            if(director.Stage==AtmajaStage.StairCheckpoint&&director.player.transform.position.y>3.4f)
            {
                if(Commit("EVENT_P0_UPPER_REACHED","PROLOG_ATMAJA_UPPER"))
                {director.SetStage(AtmajaStage.StairLanding);director.Objective();Hud.EnqueueSubtitle("Raka","Jejak tangan... berdarah semua. Ya Tuhan...",4);}
            }
            if(director.Stage==AtmajaStage.StairLanding&&director.player.transform.position.y>3.4f&&Math.Abs(director.LocalPosition(director.player.transform.position).y-5.7f)<1.2f)
            {director.SetStage(AtmajaStage.EvidenceHunt);director.Objective();}
            // Child vent pass: AR transparency + crouch walk lets the capsule through the cabinet hole.
            if(ventGate&&ventMouth)
            {
                bool near=Vector3.Distance(director.player.transform.position,ventMouth.position)<2.1f;
                bool pass=ar&&director.player.IsCrouching&&near&&childDoor&&!childDoor.IsOpen;
                ventGate.enabled=!pass;
                if(ventCabinet)ventCabinet.SetActive(!pass);
            }
            if(!Has("EVENT_P0_MIRROR_SCARE")&&mirrorSpot&&mirrorFlashFigure)
            {
                var eye=director.player.GetComponent<InteractionInteractor>().playerCamera;
                var delta=mirrorSpot.position-eye.transform.position;
                if(delta.magnitude<2.2f&&Vector3.Dot(eye.transform.forward,delta.normalized)>.8f)
                {
                    if(Commit("EVENT_P0_MIRROR_SCARE","PROLOG_ATMAJA_UPPER"))StartCoroutine(FlashFigure(mirrorFlashFigure,.7f,"Raka","Di... di belakang gue... Nggak ada siapa-siapa?!"));
                }
            }
            if(!Hud.IsModalOpen&&Phone&&Phone.isPhoneRaised&&Phone.isScreenOn&&GameInputConfig.Instance.GetActionDown(GameAction.CaptureOrBlitz))TryPhotoTick();
            if(ropePivot)
            {
                if(!ropeInit){ropeBase=ropePivot.rotation;ropeInit=true;}
                ropePivot.rotation=ropeBase*Quaternion.Euler(Mathf.Sin(Time.time*.9f)*4,0,0);
                creakTimer-=Time.deltaTime;
                if(creakTimer<=0&&Vector3.Distance(director.player.transform.position,ropePivot.position)<7.5f)
                {
                    creakTimer=2.8f+UnityEngine.Random.value*2.2f;
                    if(!creakClip)creakClip=AtmajaSynth.Creak(1.1f);
                    Sfx(creakClip,ropePivot.position,.32f);
                }
            }
            if(bathWater)
            {
                dripTimer-=Time.deltaTime;
                if(dripTimer<=0&&Vector3.Distance(director.player.transform.position,bathWater.transform.position)<6.5f)
                {
                    dripTimer=1.9f+UnityEngine.Random.value*1.8f;
                    if(!dripClip)dripClip=AtmajaSynth.Drip();
                    Sfx(dripClip,bathWater.transform.position,.4f);
                }
            }
        }
        private IEnumerator FlashFigure(GameObject figure,float seconds,string speaker,string line)
        {
            figure.SetActive(true);Ui(AtmajaSynth.Noise(.6f,.5f),.6f);
            if(!string.IsNullOrEmpty(line))Hud.EnqueueSubtitle(speaker,line,3);
            yield return new WaitForSeconds(seconds);figure.SetActive(false);
        }
        private bool Aimed(Transform aim,float range,float dot)
        {
            if(!aim)return false;
            var eye=director.player.GetComponent<InteractionInteractor>().playerCamera;var delta=aim.position-eye.transform.position;
            if(delta.magnitude>range||Vector3.Dot(eye.transform.forward,delta.normalized)<dot)return false;
            var hits=Physics.RaycastAll(eye.transform.position,delta.normalized,delta.magnitude,~0,QueryTriggerInteraction.Ignore);
            Array.Sort(hits,(a,b)=>a.distance.CompareTo(b.distance));
            var first=Array.Find(hits,h=>!h.transform.IsChildOf(director.player.transform));
            return !first.collider||first.collider.isTrigger||first.transform==aim||first.transform.IsChildOf(aim.parent);
        }
        public void TryPhotoTick()
        {
            if(!ArActive||director.Stage<AtmajaStage.EvidenceHunt||director.Stage>AtmajaStage.MasterOpen)return;
            if(!Has("EVENT_P0_CHAIN1_BROKEN")&&Has(DiaryId)&&Aimed(cupAim,3f,.9f))BreakChain(1,"Cangkir susu retak terpindai aura merah.","EVENT_P0_CHAIN1_BROKEN");
            else if(Has("EVENT_P0_CHAIN1_BROKEN")&&!Has("EVENT_P0_CHAIN2_BROKEN")&&Aimed(scratchAim,2.6f,.88f))BreakChain(2,"Cakaran puluhan jari kecil terpindai jelas.","EVENT_P0_CHAIN2_BROKEN");
            else if(Has("EVENT_P0_CHAIN2_BROKEN")&&!Has("EVENT_P0_CHAIN3_BROKEN")&&Aimed(ropeAim,4.5f,.88f))BreakChain(3,"Tali tambang berayun terpindai. Sesuatu tergantung di sana...","EVENT_P0_CHAIN3_BROKEN");
        }
        private void BreakChain(int n,string toast,string flag)
        {
            if(!Commit(flag,"PROLOG_ATMAJA_UPPER",()=>{Campaign.Data.battery=Mathf.Max(0,Phone.batteryPercent-1);}))return;
            Phone.ConsumeBattery(1);RestoreWorld();Sfx(director.audioProfile.unlock,director.player.transform.position,.7f);
            Ui(AtmajaSynth.Tone(1.2f,140,60,.5f),.6f);
            Hud.ShowBroadcastNotification($"Rantai {n} putus! {toast}",5);
            director.phoneView.AddChat("@horor_hunter","BARUSAN RANTAINYA PUTUS SENDIRI WOI");
            if(n==3){StartCoroutine(FlashFigure(hangedFlashFigure,.5f,"Raka","Dia... tergantung di sana..."));OpenMasterDoor();}
            director.Objective();
        }
        private void OpenMasterDoor()
        {
            if(!Commit("EVENT_P0_MASTER_OPEN","PROLOG_ATMAJA_UPPER"))return;
            if(masterDoor){masterDoor.locked=false;masterDoor.Interact();}
            director.SetStage(AtmajaStage.MasterOpen);Sfx(AtmajaSynth.Tone(1.6f,90,55,.5f),masterDoor?masterDoor.transform.position:director.player.transform.position,.6f);
            Hud.ShowBroadcastNotification("Pintu Kamar Tidur Utama terbuka perlahan...",5);
            director.phoneView.AddChat("@dinda_ayu","bang jangan masuk bang sumpah jangan masuk");
        }
        private IEnumerator DescendToBasement()
        {
            var basement=director.basement;
            if(!basement||!basement.spawnBasement)yield break;
            director.SetBusy(true);director.player.MovementLocked=true;
            yield return Hud.FadeBlack(0,1,.5f);
            director.PlacePlayer(basement.spawnBasement.position,basement.spawnBasement.rotation);
            yield return Hud.FadeBlack(1,0,.8f);
            director.SetBusy(false);director.player.MovementLocked=false;
        }
        public void HandleEvidence(AtmajaItem item)
        {
            switch(item.action)
            {
                case AtmajaAction.CupPhoto:Hud.ShowBroadcastNotification("Cangkir susu retak. Auranya merah di mode AR.\nBaca buku harian dulu, lalu potret dengan Kamera AR.",5);break;
                case AtmajaAction.Diary:Hud.OpenDocumentModal("BUKU HARIAN BIMA • DOKUMEN #4",DiaryText,()=>{if(Commit(DiaryId,"PROLOG_ATMAJA_UPPER")){Hud.ShowBroadcastNotification("Buku harian dibaca. Potret cangkir susu dengan Kamera AR.",4);director.Objective();}});break;
                case AtmajaAction.BathCurtain:
                    if(Commit("EVENT_P0_CURTAIN_OPEN","PROLOG_ATMAJA_UPPER")){RestoreWorld();Hud.EnqueueSubtitle("Raka","Airnya... merah kecokelatan. Kayak karat... atau darah.",4);Sfx(director.audioProfile.cloth,item.transform.position,.4f);}break;
                case AtmajaAction.BathScratches:Hud.ShowBroadcastNotification("Bekas cakaran puluhan jari kecil. Potret dengan Flash Kamera AR.",4);break;
                case AtmajaAction.WillLetter:Hud.OpenDocumentModal("SURAT WASIAT NYONYA • DOKUMEN #5",WillText,()=>{if(Commit(WillId,"PROLOG_ATMAJA_UPPER")){Hud.ShowBroadcastNotification("Surat dibaca. Arahkan kamera AR ke tali tambang di atas.",4);director.Objective();}});break;
                case AtmajaAction.RopePhoto:Hud.ShowBroadcastNotification("Seutas tali tambang menjuntai dan berayun sendiri.\nPotret dengan Kamera AR.",4);break;
            }
        }
        public void HandleMaster(AtmajaItem item)
        {
            if(item.action==AtmajaAction.CarpetLift)
            {
                if(Commit("EVENT_P0_CARPET_LIFTED","PROLOG_ATMAJA_UPPER")){RestoreWorld();Hud.EnqueueSubtitle("Raka","Karpetnya digulung... ada pintu jebakan kayu di bawahnya.",4);Sfx(director.audioProfile.latch,item.transform.position,.5f);director.Objective();}
            }
            else if(item.action==AtmajaAction.Trapdoor)
            {
                if(Has("EVENT_P0_TRAPDOOR_OPEN"))return;
                if(Commit("EVENT_P0_TRAPDOOR_OPEN","PROLOG_ATMAJA_UPPER")){RestoreWorld();Hud.EnqueueSubtitle("Raka","Tangga batu... turun ke bawah tanah. Dingin banget hawanya.",4);Sfx(director.audioProfile.latch,item.transform.position,.6f);director.Objective();StartCoroutine(DescendToBasement());}
            }
        }
        public int ViewersBump()
        {
            if(Has("EVENT_P0_CHAIN3_BROKEN"))return 8800;
            if(Has("EVENT_P0_CHAIN2_BROKEN"))return 5400;
            if(Has("EVENT_P0_CHAIN1_BROKEN"))return 3800;
            if(Has("EVENT_P0_UPPER_REACHED"))return 2600;
            return 0;
        }
    }
}
