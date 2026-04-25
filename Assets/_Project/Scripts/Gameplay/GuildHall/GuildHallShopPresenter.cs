using _Project.Scripts.Gameplay.Gold;
using _Project.Scripts.Gameplay.Units;

using Zenject;

namespace _Project.Scripts.Gameplay.GuildHall
{
    public class GuildHallShopPresenter : IInitializable
    {
        [Inject] private GuildHall _guildHall;
        [Inject] private GuildHallShopView _shopView;
        [Inject] private GoldService _goldService;
        [Inject] private GuildHallShopConfig _config;
        [Inject] private UnitService _unitService;

        private int _goldPerClickUpgradeLevel = 0;

        private int CurrentCost =>
            UnityEngine.Mathf.RoundToInt(_config.BaseCost * UnityEngine.Mathf.Pow(_config.CostMultiplier, _goldPerClickUpgradeLevel));

        public void Initialize()
        {
            _guildHall.OnClicked += HandleBuildingClicked;
            _shopView.UpgradeGoldPerClickButton.OnClicked += HandleUpgrade;
            _shopView.BuyUnitButton.OnClicked += HandleBuyUnit;
            _shopView.OnCloseClicked += HandleClose;
            _goldService.OnAmountChanged += UpdateView;
            _unitService.OnChanged += UpdateView;

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

            _goldPerClickUpgradeLevel++;
            _goldService.UpgradeBonusPerClick(_config.BonusPerUpgrade);
            UpdateView();
        }

        private void HandleBuyUnit()
        {
            if (_goldService.CurrentAmount < _unitService.CurrentCost)
            {
                _shopView.BuyUnitButton.PlayInsufficientFundsShake();
                return;
            }

            _unitService.TryPurchaseUnit();
        }

        private void UpdateView()
        {
            _shopView.UpgradeGoldPerClickButton.SetStatText($"+{_goldService.CurrentBonusPerClick + 1}");
            _shopView.UpgradeGoldPerClickButton.SetCostText(CurrentCost);
            _shopView.UpgradeGoldPerClickButton.SetAppearance(_goldService.CurrentAmount >= CurrentCost);

            _shopView.BuyUnitButton.SetStatText(_unitService.Count.ToString());
            _shopView.BuyUnitButton.SetCostText(_unitService.CurrentCost);
            _shopView.BuyUnitButton.SetAppearance(
                _unitService.CanPurchaseUnit && _goldService.CurrentAmount >= _unitService.CurrentCost);
        }
    }
}
