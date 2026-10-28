using LayarTerakhir.Audio;
using LayarTerakhir.Core;
using LayarTerakhir.Player;
using LayarTerakhir.Player;
using LayarTerakhir.Prolog;
using UnityEngine;

namespace LayarTerakhir.Interlude
{
    // Lightweight daytime traffic: two scooters looping the main road in
    // their own lanes. No physics combat, no collisions with the player;
    // lanes are offset from sidewalks and parked cars. Engine loops reuse
    // the existing departure clip at ambient volume (project doppler rule).
    public sealed class InterludeTraffic : MonoBehaviour
    {
        private sealed class Bike
        {
            public Transform root;
            public AudioSource engine;
            public float z, speed, lane;
            public int direction;
        }

        private readonly System.Collections.Generic.List<Bike> bikes = new System.Collections.Generic.List<Bike>();
        private KosCorridorDirector corridor;
        private readonly System.Collections.Generic.List<Material> riderMaterials=new System.Collections.Generic.List<Material>();

        public static void Install(KosCorridorDirector corridor)
        {
            var existing = corridor.GetComponent<InterludeTraffic>();
            if (existing) return;
            var traffic = corridor.gameObject.AddComponent<InterludeTraffic>();
            traffic.corridor = corridor;
        }

        private void Start()
        {
            var source = GameObject.Find("StreetParkedMotor0");
            if (!source) source = corridor.motor ? corridor.motor.gameObject : null;
            if (!source) return;
            Spawn(source, -1.8f, -1, 7f);
            Spawn(source, 1f, 1, 6f);
        }

        private void Spawn(GameObject source, float lane, int direction, float speed)
        {
            var bike = Object.Instantiate(source);
            bike.name = "InterludeTrafficMotor";
            bike.transform.SetParent(corridor.transform, false);
            foreach (var item in bike.GetComponentsInChildren<Prolog.KosTravelItem>(true)) Object.Destroy(item);
            foreach (var item in bike.GetComponentsInChildren<InterludeWorldItem>(true)) Object.Destroy(item);
            foreach (var c in bike.GetComponentsInChildren<Collider>(true)) {c.enabled=false;Object.Destroy(c);}
            if(!AddRider(bike)) {Object.Destroy(bike);return;}
            bike.transform.SetPositionAndRotation(new Vector3(lane,0,direction<0?-8f:-60f),Quaternion.Euler(0,direction<0?180:0,0));
            var engine = bike.AddComponent<AudioSource>();
            engine.playOnAwake = false;
            engine.loop = true;
            engine.volume = .1f;
            engine.dopplerLevel = 0;
            var profile = corridor.audioProfile;
            if (profile) { engine.outputAudioMixerGroup = profile.effectsBus; engine.clip = profile.departureEngine; }
            engine.Play();
            bikes.Add(new Bike { root = bike.transform, engine = engine, z = direction < 0 ? -8f : -60f, speed = speed, lane = lane, direction = direction });
        }

        private bool AddRider(GameObject bike)
        {
            var prefab=Resources.Load<GameObject>("RakaCharacter");
            if(!prefab){Debug.LogError("Traffic requires the existing seated character rig");return false;}
            var rider=Object.Instantiate(prefab,bike.transform,false);rider.name="NeighborhoodTrafficRider";
            var avatar=rider.GetComponent<AnimatedRakaAvatar>();var motion=rider.GetComponent<Animation>();
            if(!avatar||!motion||!motion.GetClip("Riding")){Object.Destroy(rider);return false;}
            avatar.enabled=false;avatar.handset.SetActive(false);
            foreach(var renderer in rider.GetComponentsInChildren<Renderer>(true))
            {
                if(renderer.name=="HandsetScreen")renderer.gameObject.SetActive(false);
                if(renderer.name.StartsWith("RakaTShirt"))renderer.enabled=false;
                if(renderer.name=="RakaJacketOutfit")
                {
                    renderer.enabled=true;var mats=renderer.sharedMaterials;
                    for(int i=0;i<mats.Length;i++)
                    {
                        var material=new Material(mats[i]);
                        material.color=bikes.Count==0?new Color(.35f,.42f,.31f):new Color(.28f,.34f,.44f);
                        mats[i]=material;riderMaterials.Add(material);
                    }
                    renderer.sharedMaterials=mats;
                }
                renderer.renderingLayerMask=1;
            }
            motion.Play("Riding");motion["Riding"].time=0;motion.Sample();
            var fit=rider.AddComponent<MotorcycleRiderAlignment>();fit.Fit(bike.transform);
            // Constant seated pose follows the bike's transform; no animation/IK
            // work per frame and no player input, inventory or camera components.
            motion.enabled=false;
            return true;
        }
        private void Update()
        {
            if (UiFocus.HardModal || UiFocus.Menu) return;
            foreach (var bike in bikes)
            {
                bike.z += bike.direction * bike.speed * Time.deltaTime;
                if (bike.z < -60f && bike.direction < 0) bike.z = -8f;
                if (bike.z > -8f && bike.direction > 0) bike.z = -60f;
                bike.root.position = new Vector3(bike.lane, bike.root.position.y, bike.z);
                bike.root.rotation = Quaternion.Euler(0, bike.direction < 0 ? 180 : 0, 0);
            }
        }

        private void OnDestroy()
        {
            foreach (var bike in bikes)
                if (bike.root) Object.Destroy(bike.root.gameObject);
            bikes.Clear();
            foreach(var material in riderMaterials)if(material)Object.Destroy(material);
            riderMaterials.Clear();
        }
    }
}
