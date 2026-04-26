using _Project.Scripts.Gameplay.Gem;
using _Project.Scripts.Gameplay.Gold;

using Zenject;

namespace _Project.Scripts.Gameplay.Currencies
{
    public class CurrencyRegistryInitializer : IInitializable
    {
        [Inject] private CurrencyRegistry _registry;
        [Inject] private GoldService _goldService;
        [Inject] private GemService _gemService;

        public void Initialize()
        {
            _registry.Register(CurrencyType.Gold, _goldService);
            _registry.Register(CurrencyType.Gem, _gemService);
        }
    }
}
