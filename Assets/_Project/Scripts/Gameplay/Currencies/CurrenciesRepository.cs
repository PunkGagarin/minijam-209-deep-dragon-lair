using _Project.Scripts.GameplayData.Repositories;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Currencies
{
    [CreateAssetMenu(fileName = "CurrenciesRepository", menuName = "Game Resources/Repositories/Currencies")]
    public class CurrenciesRepository : Repository<CurrencyDefinition>
    {
        public CurrencyDefinition Find(CurrencyType type) => Definitions.Find(d => d.Type == type);

        public Sprite GetIcon(CurrencyType type) => Find(type).Icon;
    }
}
