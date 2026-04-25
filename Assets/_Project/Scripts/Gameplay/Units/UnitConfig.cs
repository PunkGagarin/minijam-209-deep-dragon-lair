using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    [CreateAssetMenu(fileName = "UnitConfig", menuName = "Game Resources/Configs/UnitConfig")]
    public class UnitConfig : ScriptableObject
    {
        [field: SerializeField] public int BaseGoldPerTrip { get; private set; } = 5;
        [field: SerializeField] public float MineTime { get; private set; } = 2f;
    }
}
