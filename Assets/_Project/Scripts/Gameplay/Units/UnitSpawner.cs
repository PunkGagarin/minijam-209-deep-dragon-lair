using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField]
        private Unit _unitPrefab;

        [SerializeField]
        private Unit _crowPrefab;

        [SerializeField]
        private Transform _guildPoint;

        [SerializeField]
        private float _yCrowOffset = 1f;

        [SerializeField]
        private Transform _gatherGoldPoint;

        public bool CanSpawn =>
            _unitPrefab != null && _crowPrefab != null && _guildPoint != null && _gatherGoldPoint != null;

        public void SetGatherGoldPoint(Transform gatherGoldPoint)
        {
            _gatherGoldPoint = gatherGoldPoint;
        }

        public Unit SpawnUnit(float mineTime, UnitType type)
        {
            if (_unitPrefab == null)
            {
                Debug.LogError(
                    $"{nameof(UnitSpawner)} on {name} cannot spawn a unit because Unit prefab is not assigned.", this);
                return null;
            }

            if (_guildPoint == null || _gatherGoldPoint == null)
            {
                Debug.LogError(
                    $"{nameof(UnitSpawner)} on {name} cannot spawn a unit because route points are not assigned.",
                    this);
                return null;
            }
            Unit unit = null;
            if (type == UnitType.Unit)
            {
                unit = Instantiate(_unitPrefab, _guildPoint.position, Quaternion.identity);
                unit.Initialize(_guildPoint, _gatherGoldPoint, mineTime);
            }
            else
            {
                unit = Instantiate(_crowPrefab, _guildPoint.position + new Vector3(0f, _yCrowOffset, 0f), Quaternion.identity);
                unit.Initialize(_guildPoint, _gatherGoldPoint, mineTime, _yCrowOffset);
            }
            return unit;
        }
    }
}