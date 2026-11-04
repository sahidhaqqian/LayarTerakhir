using UnityEngine;

namespace LayarTerakhir.Prolog
{
    // Designer Nov: maze room doors swing for the hunter — Nyonya pushes
    // them open on approach (Outlast-style), so she is never stuck behind
    // a closed leaf while patrolling or chasing through the maze.
    public sealed class MazeDoor : MonoBehaviour
    {
        public HingedDoor door;
        public float openRadius = 1.6f;
        private AtmajaBasementHorror basement;
        private void Awake()
        {
            if (!door) door = GetComponentInChildren<HingedDoor>();
            basement = FindFirstObjectByType<AtmajaBasementHorror>();
            // Leaves serialize SHUT (the only consistent default: `opened`
            // is not serialized, so a saved-open leaf would restart closed
            // while looking open). The maze always starts with them ajar.
            if (door) door.SetOpenImmediate(true);
        }
        private void Update()
        {
            if (!door || door.IsOpen || !basement || !basement.nyonya) return;
            if (!basement.nyonya.activeInHierarchy) return;
            if (Vector3.Distance(basement.nyonya.transform.position, door.transform.position) < openRadius)
                door.SetOpenAnimated(true);
        }
    }
}
