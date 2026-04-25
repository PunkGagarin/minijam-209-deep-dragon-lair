using _Project.Scripts.Gameplay.Gold;
using _Project.Scripts.Gameplay.Units;

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
        [Inject] private UnitSpawner _unitSpawner;

        private int _upgradeLevel = 0;
        private int _unitCount = 0;

        private int CurrentCost =>
            Mathf.RoundToInt(_config.BaseCost * Mathf.Pow(_config.CostMultiplier, _upgradeLevel));

        private int UnitCurrentCost =>
            Mathf.RoundToInt(_config.UnitBaseCost * Mathf.Pow(_config.UnitCostMultiplier, _unitCount));

        public void Initialize()
        {
            _guildHall.OnClicked += HandleBuildingClicked;
            _shopView.UpgradeGoldPerClickButton.OnClicked += HandleUpgrade;
            _shopView.BuyUnitButton.OnClicked += HandleBuyUnit;
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
                _shopView.UpgradeGoldPerClickButton.PlayInsufficientFundsShake();
                return;
            }

            _upgradeLevel++;
            _goldService.UpgradeBonusPerClick(_config.BonusPerUpgrade);
            UpdateView();
        }

        private void HandleBuyUnit()
        {
            if (_unitCount >= _config.MaxUnits)
                return;

            if (!_goldService.TrySpend(UnitCurrentCost))
            {
                _shopView.BuyUnitButton.PlayInsufficientFundsShake();
                return;
            }

            _unitCount++;
            _unitSpawner.SpawnUnit();
            UpdateView();
        }

        private void UpdateView()
        {
            _shopView.UpgradeGoldPerClickButton.SetStatText($"+{_goldService.CurrentBonusPerClick + 1}");
            _shopView.UpgradeGoldPerClickButton.SetCostText(CurrentCost);
            _shopView.UpgradeGoldPerClickButton.SetAppearance(_goldService.CurrentAmount >= CurrentCost);

            bool canBuyUnit = _unitCount < _config.MaxUnits;
            _shopView.BuyUnitButton.SetStatText($"{_unitCount}/{_config.MaxUnits}");
            _shopView.BuyUnitButton.SetCostText(UnitCurrentCost);
            _shopView.BuyUnitButton.SetAppearance(canBuyUnit && _goldService.CurrentAmount >= UnitCurrentCost);
        }
    }
}
