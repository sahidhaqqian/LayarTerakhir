using System.Collections;
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
        private Vector3 previousPosition;
        private float stepDistance;
        private int stepIndex;
        private bool departing;
        private bool lastRaised,lastScreen;
        private CharacterController controller;
        public bool HasPhoneVibration => phoneSource && phoneSource.isPlaying;
        public int OpeningKnockCount {get;private set;}
        public int DepartingFootstepCount {get;private set;}

        private void Start()
        {
            if(profile)
            {
                if(room){room.gain=profile.roomGain;room.fadeSeconds=profile.fadeSeconds;}
                if(roof){roof.gain=profile.rainGain;roof.fadeSeconds=profile.fadeSeconds;}
                if(computer){computer.gain=profile.pcGain;computer.fadeSeconds=profile.fadeSeconds;}
            }
            if(player){previousPosition=player.transform.position;controller=player.GetComponent<CharacterController>();}
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
            if(!player)return;
            var p=player.transform.position;var delta=p-previousPosition;delta.y=0;previousPosition=p;
            if(delta.sqrMagnitude>1f){stepDistance=0;return;} // Teleports/load are not footsteps.
            if(player.MovementLocked || (HudController.Instance && HudController.Instance.IsModalOpen))return;
            if(!controller || !controller.isGrounded)return;
            stepDistance+=delta.magnitude;
            float stride=player.IsCrouching ? .85f : player.IsSprinting ? 1.25f : 1.05f;
            if(stepDistance<stride || profile.tileSteps==null || profile.tileSteps.Length==0)return;
            stepDistance-=stride;stepsSource.transform.position=p+Vector3.up*.1f;
            stepsSource.pitch=1f+(stepIndex%3-1)*.035f;
            stepsSource.PlayOneShot(profile.tileSteps[stepIndex++%profile.tileSteps.Length],player.IsCrouching ? .12f : .23f);
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
        }
        public void PlayKnocks()=>StartCoroutine(Knocks());
        public IEnumerator PlayOpeningKnocks()=>Knocks();
        private IEnumerator Knocks()
        {
            if(!doorSource||!profile||!profile.knock)yield break;
            var origin=doorSource.transform.position;
            doorSource.minDistance=2.5f;doorSource.maxDistance=16;doorSource.volume=1;doorSource.pitch=1;
            for(int i=0;i<3;i++)
            {OpeningKnockCount++;doorSource.PlayOneShot(profile.knock,.95f);yield return new WaitForSeconds(.64f);}
            yield return new WaitForSeconds(.55f);
            for(int i=0;i<6&&profile.tileSteps.Length>0;i++)
            {
                doorSource.transform.position=origin+Vector3.left*(i*.65f);
                DepartingFootstepCount++;doorSource.PlayOneShot(profile.tileSteps[i%profile.tileSteps.Length],.65f*Mathf.Pow(.76f,i));
                yield return new WaitForSeconds(.52f);
            }
            doorSource.transform.position=origin;
        }
        public void NotifyDonation() { if(uiSource)uiSource.PlayOneShot(profile.notification,.7f); }
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
