using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.GuildHall
{
    public class GuildHallInstaller : MonoInstaller
    {
        [SerializeField] private GuildHall _guildHall;
        [SerializeField] private GuildHallShopView _shopView;
        [SerializeField] private GuildHallShopConfig _config;

        public override void InstallBindings()
        {
            Container.Bind<GuildHall>()
                .FromInstance(_guildHall).AsSingle();

            Container.Bind<GuildHallShopView>()
                .FromInstance(_shopView).AsSingle();

            Container.Bind<GuildHallShopConfig>()
                .FromInstance(_config).AsSingle();

            Container.BindInterfacesAndSelfTo<GuildHallShopPresenter>()
                .FromNew().AsSingle()
                .NonLazy();
        }
    }
}
