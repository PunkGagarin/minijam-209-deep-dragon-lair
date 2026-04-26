using _Project.Scripts.Gameplay.Currencies;

using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Gem
{
    public class GemInstaller : MonoInstaller
    {
        [SerializeField] private GemView _gemView;

        public override void InstallBindings()
        {
            Container.Bind<GemModel>()
                .FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<GemService>()
                .FromNew().AsSingle();

            Container.Bind<GemView>()
                .FromInstance(_gemView).AsSingle();

            Container.BindInterfacesAndSelfTo<GemPresenter>()
                .FromNew().AsSingle().NonLazy();
        }
    }
}
