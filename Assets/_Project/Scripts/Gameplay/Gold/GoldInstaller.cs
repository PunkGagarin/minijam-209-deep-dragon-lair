using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldInstaller : MonoInstaller
    {
        [SerializeField] private GoldView _goldView;
        [SerializeField] private GoldPile _goldPile;

        public override void InstallBindings()
        {
            Container.Bind<GoldModel>()
                .FromNew().AsSingle();

            Container.Bind<GoldService>()
                .FromNew().AsSingle();

            Container.Bind<GoldView>()
                .FromInstance(_goldView).AsSingle();

            Container.Bind<GoldPile>()
                .FromInstance(_goldPile).AsSingle();

            Container.BindInterfacesAndSelfTo<GoldPresenter>()
                .FromNew().AsSingle().NonLazy();
        }
    }
}
