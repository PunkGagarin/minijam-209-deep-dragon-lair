using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    [RequireComponent(typeof(Movement))]
    public class Unit : MonoBehaviour
    {
        private Movement _movement;

        public event System.Action<Unit> OnMiningCompleted = delegate { };
        public event System.Action<Unit> OnReturnedToGuild = delegate { };

        private void Awake()
        {
            _movement = GetComponent<Movement>();
        }

        private void OnEnable()
        {
            _movement.OnMiningCompleted += HandleMiningCompleted;
            _movement.OnReachedGuild += HandleReachedGuild;
        }

        private void OnDisable()
        {
            if (_movement == null)
                return;

            _movement.OnMiningCompleted -= HandleMiningCompleted;
            _movement.OnReachedGuild -= HandleReachedGuild;
        }

        public void Initialize(Transform guildPoint, Transform gatherGoldPoint, float mineTime)
        {
            _movement.Initialize(guildPoint, gatherGoldPoint, mineTime);
        }

        private void HandleMiningCompleted() => OnMiningCompleted.Invoke(this);

        private void HandleReachedGuild() => OnReturnedToGuild.Invoke(this);
    }
}
