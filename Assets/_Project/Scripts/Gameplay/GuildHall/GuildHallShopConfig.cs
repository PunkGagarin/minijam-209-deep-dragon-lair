using UnityEngine;

namespace _Project.Scripts.Gameplay.GuildHall
{
    [CreateAssetMenu(fileName = "GuildHallShopConfig", menuName = "Game Resources/Configs/GuildHallShop")]
    public class GuildHallShopConfig : ScriptableObject
    {
        [field: Header("Gold Per Click Upgrade")]
        [field: SerializeField] public int BaseCost { get; private set; } = 10;
        [field: SerializeField] public float CostMultiplier { get; private set; } = 1.5f;
        [field: SerializeField] public int BonusPerUpgrade { get; private set; } = 1;

        [field: Header("Buy Unit")]
        [field: SerializeField] public int UnitBaseCost { get; private set; } = 50;
        [field: SerializeField] public float UnitCostMultiplier { get; private set; } = 2f;

        [field: Header("Move Speed Upgrade")]
        [field: SerializeField] public int MoveSpeedBaseCost { get; private set; } = 15;
        [field: SerializeField] public float MoveSpeedCostMultiplier { get; private set; } = 1.6f;
        [field: SerializeField, Range(0f, 1f)] public float MoveSpeedBonusPerUpgrade { get; private set; } = 0.15f;

        [field: Header("Noise Reduction Upgrade")]
        [field: SerializeField] public int NoiseReductionBaseCost { get; private set; } = 25;
        [field: SerializeField] public float NoiseReductionCostMultiplier { get; private set; } = 1.7f;
        [field: SerializeField, Range(0f, 0.9f)] public float NoiseReductionPerUpgrade { get; private set; } = 0.1f;

        [field: Header("Gold Per Trip Upgrade")]
        [field: SerializeField] public int UnitGoldBaseCost { get; private set; } = 30;
        [field: SerializeField] public float UnitGoldCostMultiplier { get; private set; } = 1.6f;
        [field: SerializeField] public int UnitGoldBonusPerUpgrade { get; private set; } = 1;

        [field: Header("Gem Drop Chance Upgrade")]
        [field: SerializeField] public int GemChanceBaseCost { get; private set; } = 100;
        [field: SerializeField] public float GemChanceCostMultiplier { get; private set; } = 1.8f;
        [field: SerializeField, Range(0f, 1f)] public float GemChanceBonusPerUpgrade { get; private set; } = 0.02f;

        [field: Header("Gem Drop Quantity Upgrade")]
        [field: SerializeField] public int GemQuantityBaseCost { get; private set; } = 250;
        [field: SerializeField] public float GemQuantityCostMultiplier { get; private set; } = 2f;
        [field: SerializeField] public int GemQuantityBonusPerUpgrade { get; private set; } = 1;
    }
}
