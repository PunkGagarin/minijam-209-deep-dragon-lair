using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private Unit _unitPrefab;
        [SerializeField] private Transform _guildPoint;
        [SerializeField] private Transform _gatherGoldPoint;

        public bool CanSpawn => _unitPrefab != null && _guildPoint != null && _gatherGoldPoint != null;

        public Unit SpawnUnit(float mineTime)
        {
            if (_unitPrefab == null)
            {
                Debug.LogError($"{nameof(UnitSpawner)} on {name} cannot spawn a unit because Unit prefab is not assigned.", this);
                return null;
            }

            if (_guildPoint == null || _gatherGoldPoint == null)
            {
                Debug.LogError(
                    $"{nameof(UnitSpawner)} on {name} cannot spawn a unit because route points are not assigned.",
                    this);
                return null;
            }

            Unit unit = Instantiate(_unitPrefab, _guildPoint.position, Quaternion.identity);
            unit.Initialize(_guildPoint, _gatherGoldPoint, mineTime);
            return unit;
        }
    }
}
