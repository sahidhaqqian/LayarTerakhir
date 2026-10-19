using System.Collections;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Smartphone;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Audio
{
    // Playback attached to existing story events; no independent story timers or new gameplay triggers.
    public sealed class KosSceneAudio : MonoBehaviour
    {
        public SceneAudioProfile profile;
        public AmbientAudioLoop room, roof, computer;
        public AudioSource doorSource, itemSource, uiSource, phoneSource, stepsSource, departureSource;
        public FirstPersonController player;
        private bool departing;
        private bool lastRaised,lastScreen;
        public bool HasPhoneVibration => phoneSource && phoneSource.isPlaying;
        public int OpeningKnockCount {get;private set;}
        public int DepartingFootstepCount {get;private set;}
        public int RingtoneStarts {get;private set;}
        public int CallPanicEvents {get;private set;}
        private AudioSource ringtoneSource;
        private AudioClip Authored(string id)=>Resources.Load<AudioClip>("KosAudio/"+id);

        private void Start()
        {
            if(profile)
            {
                if(room){room.gain=profile.roomGain;room.fadeSeconds=profile.fadeSeconds;}
                if(roof){roof.gain=profile.rainGain;roof.fadeSeconds=profile.fadeSeconds;}
                if(computer){computer.gain=profile.pcGain;computer.fadeSeconds=profile.fadeSeconds;}
            }
            if(player)PlayerFootsteps.Install(player,profile,stepsSource);
            var phone=SmartphoneController.Instance;
            if(phone){lastRaised=phone.isPhoneRaised;lastScreen=phone.isScreenOn;}
        }
        private void Update()
        {
            if(!profile)return;
            if(!departing)
            {
                float duck=HudController.Instance && HudController.Instance.IsModalOpen ? .55f : 1;
                if(room)room.LevelMultiplier=duck;
                if(roof)roof.LevelMultiplier=duck;
                if(computer)computer.LevelMultiplier=duck;
            }
            var phone=SmartphoneController.Instance;
            if(phone && phone.hasSmartphoneInInventory)
            {
                if(phone.isPhoneRaised!=lastRaised || phone.isScreenOn!=lastScreen)PlayUI(profile.phoneSwitch);
                lastRaised=phone.isPhoneRaised;lastScreen=phone.isScreenOn;
            }
        }
        public void PlayUI(AudioClip clip=null)
        {if(uiSource&&profile)uiSource.PlayOneShot(clip?clip:profile.uiClick,profile.uiGain);}
        public void PlayItem(string kind,Vector3 position)
        {
            if(!profile||!itemSource)return;
            var clip=kind=="cloth"||kind=="paper"?profile.cloth:kind=="unlock"?profile.unlock:profile.uiClick;
            itemSource.transform.position=position;itemSource.PlayOneShot(clip,profile.effectsGain);
        }
        public void SetPhoneVibration(bool active)
        {
            if(!phoneSource)return;
            if(active && phoneSource.clip && !phoneSource.isPlaying)phoneSource.Play();
            else if(!active)phoneSource.Stop();
            if(active)
            {
                if(!ringtoneSource)
                {
                    ringtoneSource=phoneSource.gameObject.AddComponent<AudioSource>();ringtoneSource.playOnAwake=false;
                    ringtoneSource.outputAudioMixerGroup=profile.effectsBus;ringtoneSource.spatialBlend=.6f;
                    ringtoneSource.minDistance=2;ringtoneSource.maxDistance=9;ringtoneSource.dopplerLevel=0;
                    ringtoneSource.clip=Authored("IncomingCall");ringtoneSource.loop=true;ringtoneSource.volume=.65f;
                }
                if(ringtoneSource.clip&&!ringtoneSource.isPlaying){ringtoneSource.Play();RingtoneStarts++;}
            }
            else if(ringtoneSource)ringtoneSource.Stop();
        }
        public void PlayKnocks()=>StartCoroutine(Knocks());
        public IEnumerator PlayOpeningKnocks(System.Action firstKnock=null,System.Action afterKnocks=null)=>Knocks(firstKnock,afterKnocks);
        private IEnumerator Knocks(System.Action firstKnock=null,System.Action afterKnocks=null)
        {
            if(!doorSource||!profile||!profile.knock)yield break;
            var origin=doorSource.transform.position;
            doorSource.minDistance=5;doorSource.maxDistance=20;doorSource.volume=1;doorSource.pitch=1;
            var hardKnock=Authored("HardDoorKnock");
            for(int i=0;i<3;i++)
            {OpeningKnockCount++;doorSource.PlayOneShot(hardKnock?hardKnock:profile.knock,.95f);if(i==0)firstKnock?.Invoke();yield return new WaitForSeconds(.64f);}
            afterKnocks?.Invoke();
            yield return new WaitForSeconds(.55f);
            for(int i=0;i<6&&profile.tileSteps.Length>0;i++)
            {
                doorSource.transform.position=origin+Vector3.left*(i*.65f);
                DepartingFootstepCount++;doorSource.PlayOneShot(profile.tileSteps[i%profile.tileSteps.Length],.65f*Mathf.Pow(.76f,i));
                yield return new WaitForSeconds(.52f);
            }
            doorSource.transform.position=origin;
        }
        public IEnumerator PlayCallPanic()
        {
            CallPanicEvents++;
            var drag=Authored("CallMetalScrape");if(drag)uiSource.PlayOneShot(drag,.65f);
            yield return new WaitForSeconds(.55f);
            // The scream is an original, separately rendered Damar performance, not a caption.
            var scream=PresentationProfile.Load()?.Voice("Damar",LayarTerakhir.UI.KosDialogue.DamarScream);
            if(scream){uiSource.PlayOneShot(scream,.85f);yield return new WaitForSeconds(scream.length*.72f);}
            var glass=Authored("CallGlassCutoff");if(glass)uiSource.PlayOneShot(glass,.65f);
            yield return new WaitForSeconds(.65f);
        }
        public void NotifyDonation() { if(uiSource){uiSource.PlayOneShot(profile.phoneVibration,.8f);uiSource.PlayOneShot(profile.notification,.35f);} }
        public void Depart()
        {
            if(departing)return;departing=true;
            SetPhoneVibration(false);doorSource.PlayOneShot(profile.latch,.7f);
            if(departureSource && profile.departureEngine)departureSource.PlayOneShot(profile.departureEngine,.65f);
            StartCoroutine(FadeRoom());
        }
        private IEnumerator FadeRoom()
        {
            float elapsed=0;
            while(elapsed<3.5f)
            {
                elapsed+=Time.unscaledDeltaTime;float level=1-Mathf.Clamp01(elapsed/3.5f);
                if(room)room.LevelMultiplier=level;if(computer)computer.LevelMultiplier=level;
                if(roof)roof.LevelMultiplier=Mathf.Lerp(.5f,1,level);
                yield return null;
            }
        }
    }
}
