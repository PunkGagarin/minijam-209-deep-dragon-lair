using _Project.Scripts.Gameplay.Gold;

using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.GuildHall
{
    public class GuildHallShopPresenter : IInitializable
    {
        [Inject] private GuildHall _guildHall;
        [Inject] private GuildHallShopView _shopView;
        [Inject] private GoldService _goldService;
        [Inject] private GuildHallShopConfig _config;

        private int _upgradeLevel = 0;

        private int CurrentCost =>
            Mathf.RoundToInt(_config.BaseCost * Mathf.Pow(_config.CostMultiplier, _upgradeLevel));

        public void Initialize()
        {
            _guildHall.OnClicked += HandleBuildingClicked;
            _shopView.OnUpgradeClicked += HandleUpgrade;
            _shopView.OnCloseClicked += HandleClose;
            _goldService.OnAmountChanged += UpdateView;

            UpdateView();
        }

        private void HandleBuildingClicked(GuildHall _) => _shopView.Show();

        private void HandleClose() => _shopView.Hide();

        private void HandleUpgrade()
        {
            if (!_goldService.TrySpend(CurrentCost))
            {
                _shopView.PlayInsufficientFundsShake();
                return;
            }

            _upgradeLevel++;
            _goldService.UpgradeBonusPerClick(_config.BonusPerUpgrade);
            UpdateView();
        }

        private void UpdateView()
        {
            _shopView.SetBonusGoldPerClickText(_goldService.CurrentBonusPerClick);
            _shopView.SetCostText(CurrentCost);
            _shopView.SetUpgradeButtonAppearance(_goldService.CurrentAmount >= CurrentCost);
        }
    }
}
