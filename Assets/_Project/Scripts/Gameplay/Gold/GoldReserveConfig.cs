using UnityEngine;

namespace _Project.Scripts.Gameplay.Gold
{
    [CreateAssetMenu(fileName = "GoldReserveConfig", menuName = "Game Resources/Configs/GoldReserveConfig")]
    public class GoldReserveConfig : ScriptableObject
    {
        [field: SerializeField] public int MaxValue { get; private set; } = 1000;
    }
}
