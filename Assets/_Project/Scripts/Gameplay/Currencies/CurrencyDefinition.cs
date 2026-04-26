using _Project.Scripts.GameplayData.Definitions;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Currencies
{
    [CreateAssetMenu(fileName = "CurrencyDefinition", menuName = "Game Resources/Definitions/Currency")]
    public class CurrencyDefinition : Definition
    {
        [field: SerializeField] public CurrencyType Type { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
    }
}
