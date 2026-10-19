using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.UI;
using UnityEngine;

namespace LayarTerakhir.Audio
{
    public enum FootstepSurface { Tile, Stone, Earth, Wood }

    // A single owner in every existing scene, sampled after the visual gait.
    [DefaultExecutionOrder(200)]
    public sealed class PlayerFootsteps : MonoBehaviour
    {
        private FirstPersonController player;
        private CharacterController capsule;
        private AnimatedRakaAvatar avatar;
        private AudioSource source;
        private SceneAudioProfile profile;
        private AudioClip[][] banks;
        private Vector3 previous;
        private float phase,contact,fallbackDistance;
        private bool hasPhase;
        private int lastClip=-1;
        private FootstepSurface lastSurface;
        private readonly RaycastHit[] hits=new RaycastHit[32];
        private readonly System.Random variation=new System.Random();
        public int PlayedSteps {get;private set;}
        public FootstepSurface LastSurface {get;private set;}
        public static PlayerFootsteps Install(FirstPersonController owner,SceneAudioProfile audio,AudioSource supplied=null)
        {
            var steps=owner.GetComponent<PlayerFootsteps>();if(steps)return steps;
            steps=owner.gameObject.AddComponent<PlayerFootsteps>();steps.player=owner;steps.profile=audio;
            steps.capsule=owner.GetComponent<CharacterController>();steps.previous=owner.transform.position;
            steps.source=supplied;
            if(!steps.source)
            {
                var go=new GameObject("PlayerSurfaceSteps",typeof(AudioSource));go.transform.SetParent(owner.transform,false);
                steps.source=go.GetComponent<AudioSource>();steps.source.playOnAwake=false;
                steps.source.spatialBlend=.6f;steps.source.minDistance=1;steps.source.maxDistance=5;
                steps.source.rolloffMode=AudioRolloffMode.Linear;steps.source.dopplerLevel=0;
                if(audio)steps.source.outputAudioMixerGroup=audio.effectsBus;
            }
            steps.banks=new[]{audio?audio.tileSteps:null,Resources.LoadAll<AudioClip>("PlayerPresentation/Steps/Stone"),Resources.LoadAll<AudioClip>("PlayerPresentation/Steps/Earth"),Resources.LoadAll<AudioClip>("PlayerPresentation/Steps/Wood")};
            return steps;
        }
        private void LateUpdate()
        {
            if(!player||!profile||!source)return;
            var delta=transform.position-previous;previous=transform.position;delta.y=0;
            var rig=player.GetComponent<ShoulderCameraRig>();
            bool blocked=!player.enabled||player.MovementLocked||UiFocus.Cinematic||UiFocus.HardModal||UiFocus.Menu||
                (HudController.Instance&&HudController.Instance.IsModalOpen)||!capsule.enabled||!capsule.isGrounded||delta.sqrMagnitude>1||delta.sqrMagnitude<.000001f;
            if(blocked){hasPhase=false;fallbackDistance=0;return;}
            if(!avatar&&rig&&rig.Avatar)avatar=rig.Avatar.GetComponent<AnimatedRakaAvatar>();
            bool step=false;
            if(avatar&&avatar.TryGetFootstepPhase(out float next,out float reverseContact))
            {
                float offset=hasPhase&&next<phase?reverseContact:0;
                if(hasPhase&&Mathf.Abs(next-phase)<.7f&&Mathf.Approximately(contact,reverseContact))step=CrossedContact(phase,next,offset);
                phase=next;contact=reverseContact;hasPhase=true;
            }
            else
            {
                // Fallback for a missing legacy rig, never a second sound path.
                hasPhase=false;fallbackDistance+=delta.magnitude;
                float stride=player.IsCrouching?.43f:player.IsSprinting?.8f:.65f;
                if(fallbackDistance>=stride){step=true;fallbackDistance%=stride;}
            }
            if(!step)return;
            int count=Physics.RaycastNonAlloc(transform.position+Vector3.up*.2f,Vector3.down,hits,.55f,~0,QueryTriggerInteraction.Ignore);
            var results=count==hits.Length?Physics.RaycastAll(transform.position+Vector3.up*.2f,Vector3.down,.55f,~0,QueryTriggerInteraction.Ignore):hits;
            if(results!=hits)count=results.Length;
            Collider floor=null;float nearest=float.PositiveInfinity;Vector3 at=transform.position;
            for(int i=0;i<count;i++)if(results[i].collider&&!results[i].transform.IsChildOf(transform)&&results[i].normal.y>.5f&&results[i].distance<nearest)
            {floor=results[i].collider;nearest=results[i].distance;at=results[i].point;}
            if(!floor)return;
            LastSurface=SurfaceFor(floor);
            var clips=banks[(int)LastSurface];if(clips==null||clips.Length==0)clips=profile.tileSteps;
            if(clips==null||clips.Length==0)return;
            int index=variation.Next(clips.Length);
            if(LastSurface==lastSurface&&clips.Length>1&&index==lastClip)index=(index+1+variation.Next(clips.Length-1))%clips.Length;
            lastClip=index;lastSurface=LastSurface;
            source.transform.position=at+Vector3.up*.1f;
            source.pitch=.97f+(float)variation.NextDouble()*.06f;
            source.PlayOneShot(clips[index],(player.IsCrouching?.10f:player.IsSprinting?.26f:.20f)*(.92f+(float)variation.NextDouble()*.16f));
            PlayedSteps++;
        }
        public static bool CrossedContact(float before,float after,float offset)
        {
            return after>=before?Mathf.FloorToInt((after-offset)*2)>Mathf.FloorToInt((before-offset)*2):
                Mathf.CeilToInt((after-offset)*2)<Mathf.CeilToInt((before-offset)*2);
        }
        public static FootstepSurface SurfaceFor(Collider floor)
        {
            var renderer=floor.GetComponent<Renderer>();
            string material=renderer&&renderer.sharedMaterial?renderer.sharedMaterial.name:"";
            string label=(floor.name+" "+material).ToLowerInvariant();
            if(label.Contains("wood")||label.Contains("timber")||label.Contains("plank"))return FootstepSurface.Wood;
            if(label.Contains("grass")||label.Contains("soil")||label.Contains("mud")||label.Contains("earth")||label.Contains("neighborhoodground"))return FootstepSurface.Earth;
            if(label.Contains("asphalt")||label.Contains("road")||label.Contains("street")||label.Contains("concrete")||label.Contains("cellar")||label.Contains("courtyard")||label.Contains("paving"))return FootstepSurface.Stone;
            return FootstepSurface.Tile;
        }
    }
}
