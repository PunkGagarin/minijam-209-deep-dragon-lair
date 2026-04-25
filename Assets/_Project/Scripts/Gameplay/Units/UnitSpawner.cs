using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _unitPrefab;
        [SerializeField] private Transform _spawnPoint;

        public void SpawnUnit()
        {
            Instantiate(_unitPrefab, _spawnPoint.position, Quaternion.identity);
        }
    }
}
