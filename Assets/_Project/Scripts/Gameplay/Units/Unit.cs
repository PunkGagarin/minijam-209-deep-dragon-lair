using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    [RequireComponent(typeof(Movement))]
    public class Unit : MonoBehaviour
    {
        private Movement _movement;
        private bool _isDead;
        
        [SerializeField]
        private UnitType _unitType;

        public event System.Action<Unit> OnMiningCompleted = delegate { };
        public event System.Action<Unit, UnitType> OnReturnedToGuild = delegate { };
        public event System.Action<Unit> OnDied = delegate { };

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

        public void Initialize(Transform guildPoint, Transform gatherGoldPoint, float mineTime, float yCrowOffset = 0f)
        {
            _movement.Initialize(guildPoint, gatherGoldPoint, mineTime, yCrowOffset);
        }

        public void SetGatherPoint(Transform gatherGoldPoint)
        {
            _movement.SetGatherPoint(gatherGoldPoint);
        }

        public void Die()
        {
            if (_isDead)
                return;

            _isDead = true;
            OnDied.Invoke(this);
            Destroy(gameObject);
        }

        private void HandleMiningCompleted() => OnMiningCompleted.Invoke(this);

        private void HandleReachedGuild() => OnReturnedToGuild.Invoke(this, _unitType);
    }
}
