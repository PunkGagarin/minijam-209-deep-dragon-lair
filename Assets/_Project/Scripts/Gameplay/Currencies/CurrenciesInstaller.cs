using Zenject;

namespace _Project.Scripts.Gameplay.Currencies
{
    public class CurrenciesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<CurrencyRegistry>()
                .FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<CurrencyRegistryInitializer>()
                .FromNew().AsSingle().NonLazy();
        }
    }
}
