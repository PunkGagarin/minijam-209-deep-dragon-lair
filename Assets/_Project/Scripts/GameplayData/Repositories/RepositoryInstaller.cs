using _Project.Scripts.Gameplay.Currencies;

using UnityEngine;
using Zenject;

namespace _Project.Scripts.GameplayData.Repositories
{
    [CreateAssetMenu(fileName = "Repository Installer", menuName = "Game Resources/Repository Installer")]
    public class RepositoryInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private CurrenciesRepository _currenciesRepository;

        public override void InstallBindings()
        {
            Container.Bind<CurrenciesRepository>()
                .FromInstance(_currenciesRepository)
                .AsSingle();
        }
    }
}
