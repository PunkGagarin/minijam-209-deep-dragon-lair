using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.EndGame
{
    public class EndGameInstaller : MonoInstaller
    {
        [SerializeField] private EndGameUI _ui;

        public override void InstallBindings()
        {
            Container.Bind<EndGameUI>()
                .FromInstance(_ui).AsSingle();
        }
    }
}
