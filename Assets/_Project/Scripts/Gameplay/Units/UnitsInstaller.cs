using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitsInstaller : MonoInstaller
    {
        [SerializeField] private UnitSpawner _spawner;
        [SerializeField] private UnitView _unitView;

        public override void InstallBindings()
        {
            Container.Bind<UnitSpawner>()
                .FromInstance(_spawner).AsSingle();

            Container.Bind<UnitView>()
                .FromInstance(_unitView).AsSingle();

            Container.Bind<UnitService>()
                .FromNew().AsSingle();

            Container.BindInterfacesAndSelfTo<UnitPresenter>()
                .FromNew().AsSingle().NonLazy();
        }
    }
}
