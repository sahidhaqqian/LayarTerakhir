using LayarTerakhir.Audio;
using LayarTerakhir.Core;
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
            foreach (var c in bike.GetComponentsInChildren<Collider>(true)) Object.Destroy(c);
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
        }
    }
}
