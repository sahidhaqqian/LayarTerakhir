using LayarTerakhir.Player;
using UnityEngine;

namespace LayarTerakhir.Prolog
{
    public sealed class NeighborhoodRain : MonoBehaviour
    {
        public FirstPersonController player;
        public Material material;
        private ParticleSystem rain;
        private void Start()
        {
            rain=gameObject.AddComponent<ParticleSystem>();rain.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            var main=rain.main;main.loop=true;main.startLifetime=1.2f;main.startSpeed=16;main.startSize=.025f;main.maxParticles=850;
            main.startColor=new Color(.65f,.73f,.80f,.38f);main.simulationSpace=ParticleSystemSimulationSpace.World;
            var emission=rain.emission;emission.rateOverTime=500;var shape=rain.shape;shape.shapeType=ParticleSystemShapeType.Box;shape.scale=new Vector3(24,24,.1f);
            var collision=rain.collision;collision.enabled=true;collision.type=ParticleSystemCollisionType.World;collision.mode=ParticleSystemCollisionMode.Collision3D;collision.quality=ParticleSystemCollisionQuality.Low;collision.lifetimeLoss=1;collision.collidesWith=~(1<<ShoulderCameraRig.AvatarLayer);
            var renderer=rain.GetComponent<ParticleSystemRenderer>();renderer.renderMode=ParticleSystemRenderMode.Stretch;renderer.velocityScale=.03f;renderer.lengthScale=8;renderer.sharedMaterial=material;
            rain.Play();
        }
        private void LateUpdate(){if(player)transform.SetPositionAndRotation(player.transform.position+Vector3.up*12,Quaternion.Euler(90,0,0));}
    }
}
