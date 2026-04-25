using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitsInstaller : MonoInstaller
    {
        [SerializeField] private UnitSpawner _spawner;

        public override void InstallBindings()
        {
            Container.Bind<UnitSpawner>()
                .FromInstance(_spawner).AsSingle();

            Container.Bind<UnitService>()
                .FromNew().AsSingle();
        }
    }
}
