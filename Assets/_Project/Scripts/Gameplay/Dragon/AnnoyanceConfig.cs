using UnityEngine;

namespace _Project.Scripts.Gameplay.Dragon
{
    [CreateAssetMenu(fileName = "AnnoyanceConfig", menuName = "Game Resources/Configs/AnnoyanceConfig")]
    public class AnnoyanceConfig : ScriptableObject
    {
        [field: SerializeField] public float MaxValue { get; private set; } = 100f;

        [field: SerializeField, Range(0f, 1f)] public float GoldClickChance { get; private set; } = 0.25f;
        [field: SerializeField] public float GoldClickAmount { get; private set; } = 5f;

        [field: SerializeField, Range(0f, 1f)] public float MiningCompletedChance { get; private set; } = 0.5f;
        [field: SerializeField] public float MiningCompletedAmount { get; private set; } = 10f;

        [field: SerializeField] public float MovementTickInterval { get; private set; } = 1f;
        [field: SerializeField, Range(0f, 1f)] public float MovementTickChance { get; private set; } = 0.1f;
        [field: SerializeField] public float MovementTickAmount { get; private set; } = 2f;

        [field: SerializeField] public float FillResetDelay { get; private set; } = 1.5f;
    }
}
