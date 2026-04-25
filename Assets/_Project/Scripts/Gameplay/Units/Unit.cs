using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    [RequireComponent(typeof(Movement))]
    public class Unit : MonoBehaviour
    {
        private Movement _movement;

        private void Awake()
        {
            _movement = GetComponent<Movement>();
        }

        public void Initialize(Transform guildPoint, Transform gatherGoldPoint)
        {
            _movement.Initialize(guildPoint, gatherGoldPoint);
        }
    }
}
