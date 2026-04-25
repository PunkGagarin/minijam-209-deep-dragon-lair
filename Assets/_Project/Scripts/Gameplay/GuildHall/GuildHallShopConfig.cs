using UnityEngine;

namespace _Project.Scripts.Gameplay.GuildHall
{
    [CreateAssetMenu(fileName = "GuildHallShopConfig", menuName = "Game Resources/Configs/GuildHallShop")]
    public class GuildHallShopConfig : ScriptableObject
    {
        [field: SerializeField] public int BaseCost { get; private set; } = 10;
        [field: SerializeField] public float CostMultiplier { get; private set; } = 1.5f;
        [field: SerializeField] public int BonusPerUpgrade { get; private set; } = 1;

        [field: SerializeField] public int UnitBaseCost { get; private set; } = 50;
        [field: SerializeField] public float UnitCostMultiplier { get; private set; } = 2f;
    }
}
