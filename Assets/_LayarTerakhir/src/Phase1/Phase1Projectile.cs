using LayarTerakhir.Core;
using UnityEngine;
namespace LayarTerakhir.Phase1
{
    public sealed class Phase1Projectile : MonoBehaviour
    {
        private Phase1Director director; private Vector3 velocity; private float life;
        private readonly RaycastHit[] hits = new RaycastHit[12];
        public void Initialize(Phase1Director owner, Vector3 target)
        { director = owner; velocity = (target - transform.position).normalized * 10; }
        private void Update()
        {
            if (UiFocus.HardModal || UiFocus.Cinematic || !director || director.Dead || director.Busy) return;
            if (!director.BossActive || director.threats.Exposed || (life += Time.deltaTime) > 6) { Destroy(gameObject); return; }
            Vector3 step = velocity * Time.deltaTime;
            int count = Physics.SphereCastNonAlloc(transform.position, .14f, step.normalized, hits, step.magnitude, ~(1 << 29 | 1 << 26 | 1 << 2), QueryTriggerInteraction.Ignore);
            for (int i = 0; i < count; i++)
            {
                if (hits[i].transform.IsChildOf(director.world.genderuwo.transform)) continue;
                if (hits[i].transform.IsChildOf(director.player.transform)) director.Die("Puing beton mengenai Raka. Dengarkan ancang-ancang dan berlindung.");
                director.soundscape?.Play("Impact", transform.position); Destroy(gameObject); return;
            }
            transform.position += step; transform.Rotate(new Vector3(100, 180, 60) * Time.deltaTime);
        }
    }
}
