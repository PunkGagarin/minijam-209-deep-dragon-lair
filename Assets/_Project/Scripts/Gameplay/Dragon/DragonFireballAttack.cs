using _Project.Scripts.Gameplay.Units;

using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class DragonFireballAttack : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Fireball _fireballPrefab;
        [SerializeField] private DragonAttackEvent _attackEvent;
        [SerializeField] private Animator _animator;

        [Inject] private AnnoyanceService _annoyance;
        [Inject] private UnitService _units;
        [Inject] private DiContainer _container;

        private void OnEnable()
        {
            _annoyance.OnFilled += StartAttacKAnimation;
            _attackEvent.OnAttack += SpitFireball;
        }

        private void OnDisable()
        {
            if (_annoyance != null)
                _annoyance.OnFilled -= StartAttacKAnimation;
            _attackEvent.OnAttack -= SpitFireball;
        }

        public void SpitFireball()
        {
            if (_spawnPoint == null || _fireballPrefab == null)
            {
                Debug.LogError($"{nameof(DragonFireballAttack)} on {name} is missing spawn point or fireball prefab.", this);
                return;
            }

            Unit target = FindNearestUnit();
            if (target == null)
                return;

            Vector3 targetPoint = target.transform.position;
            Fireball fireball = _container.InstantiatePrefabForComponent<Fireball>(
                _fireballPrefab, _spawnPoint.position, Quaternion.identity, null);
            fireball.Launch(targetPoint);
        }

        private void StartAttacKAnimation() => _animator.Play("Attack");

        private Unit FindNearestUnit()
        {
            Unit nearest = null;
            float minSqr = float.MaxValue;
            Vector3 from = _spawnPoint.position;

            foreach (Unit unit in _units.Units)
            {
                if (unit == null)
                    continue;

                float sqr = (unit.transform.position - from).sqrMagnitude;
                if (sqr < minSqr)
                {
                    minSqr = sqr;
                    nearest = unit;
                }
            }

            return nearest;
        }
    }
}
