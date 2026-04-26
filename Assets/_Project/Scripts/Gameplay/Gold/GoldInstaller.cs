using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldInstaller : MonoInstaller
    {
        [SerializeField] private GoldView _goldView;
        [SerializeField] private GoldReserveView _reserveView;
        [SerializeField] private GoldReserveConfig _reserveConfig;

        public override void InstallBindings()
        {
            Container.Bind<GoldModel>()
                .FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<GoldService>()
                .FromNew().AsSingle();

            Container.Bind<GoldView>()
                .FromInstance(_goldView).AsSingle();

            Container.Bind<CaveGoldView>()
                .FromComponentInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<GoldPresenter>()
                .FromNew().AsSingle().NonLazy();

            Container.Bind<GoldReserveConfig>()
                .FromInstance(_reserveConfig).AsSingle();

            Container.Bind<GoldReserveModel>()
                .FromNew().AsSingle();

            Container.Bind<GoldReserveView>()
                .FromInstance(_reserveView).AsSingle();

            Container.BindInterfacesAndSelfTo<GoldReserveService>()
                .FromNew().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<GoldReservePresenter>()
                .FromNew().AsSingle().NonLazy();
        }
    }
}
