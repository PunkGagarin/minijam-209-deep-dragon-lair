using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class DragonInstaller : MonoInstaller
    {
        [SerializeField] private AnnoyanceView _view;
        [SerializeField] private AnnoyanceConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<AnnoyanceConfig>()
                .FromInstance(_config).AsSingle();

            Container.Bind<AnnoyanceModel>()
                .FromNew().AsSingle();

            Container.Bind<AnnoyanceView>()
                .FromInstance(_view).AsSingle();

            Container.BindInterfacesAndSelfTo<AnnoyanceService>()
                .FromNew().AsSingle().NonLazy();

            Container.BindInterfacesAndSelfTo<AnnoyancePresenter>()
                .FromNew().AsSingle().NonLazy();
        }
    }
}
