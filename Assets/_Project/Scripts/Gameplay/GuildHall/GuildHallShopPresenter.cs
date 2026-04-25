using _Project.Scripts.Gameplay.Dragon;
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
        [Inject] private UnitService _unitService;
        [Inject] private AnnoyanceService _annoyanceService;

        private int _goldPerClickUpgradeLevel = 0;
        private int _moveSpeedUpgradeLevel = 0;
        private int _noiseReductionUpgradeLevel = 0;
        private int _unitGoldUpgradeLevel = 0;

        private int CurrentCost =>
            Mathf.RoundToInt(_config.BaseCost * Mathf.Pow(_config.CostMultiplier, _goldPerClickUpgradeLevel));

        private int CurrentMoveSpeedCost =>
            Mathf.RoundToInt(_config.MoveSpeedBaseCost * Mathf.Pow(_config.MoveSpeedCostMultiplier, _moveSpeedUpgradeLevel));

        private int CurrentNoiseReductionCost =>
            Mathf.RoundToInt(_config.NoiseReductionBaseCost * Mathf.Pow(_config.NoiseReductionCostMultiplier, _noiseReductionUpgradeLevel));

        private int CurrentUnitGoldCost =>
            Mathf.RoundToInt(_config.UnitGoldBaseCost * Mathf.Pow(_config.UnitGoldCostMultiplier, _unitGoldUpgradeLevel));

        public void Initialize()
        {
            _guildHall.OnClicked += HandleBuildingClicked;
            _shopView.UpgradeGoldPerClickButton.OnClicked += HandleUpgrade;
            _shopView.BuyUnitButton.OnClicked += HandleBuyUnit;
            _shopView.UpgradeMoveSpeedButton.OnClicked += HandleUpgradeMoveSpeed;
            _shopView.UpgradeNoiseReductionButton.OnClicked += HandleUpgradeNoiseReduction;
            _shopView.UpgradeUnitGoldButton.OnClicked += HandleUpgradeUnitGold;
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

        private void HandleUpgradeMoveSpeed()
        {
            if (!_goldService.TrySpend(CurrentMoveSpeedCost))
            {
                _shopView.UpgradeMoveSpeedButton.PlayInsufficientFundsShake();
                return;
            }

            _moveSpeedUpgradeLevel++;
            _unitService.UpgradeMoveSpeed(_config.MoveSpeedBonusPerUpgrade);
            UpdateView();
        }

        private void HandleUpgradeNoiseReduction()
        {
            if (!_goldService.TrySpend(CurrentNoiseReductionCost))
            {
                _shopView.UpgradeNoiseReductionButton.PlayInsufficientFundsShake();
                return;
            }

            _noiseReductionUpgradeLevel++;
            _annoyanceService.UpgradeNoiseReduction(_config.NoiseReductionPerUpgrade);
            UpdateView();
        }

        private void HandleUpgradeUnitGold()
        {
            if (!_goldService.TrySpend(CurrentUnitGoldCost))
            {
                _shopView.UpgradeUnitGoldButton.PlayInsufficientFundsShake();
                return;
            }

            _unitGoldUpgradeLevel++;
            _unitService.UpgradeGoldPerTrip(_config.UnitGoldBonusPerUpgrade);
            UpdateView();
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

            _shopView.UpgradeMoveSpeedButton.SetStatText($"x{_unitService.MoveSpeedMultiplier:0.00}");
            _shopView.UpgradeMoveSpeedButton.SetCostText(CurrentMoveSpeedCost);
            _shopView.UpgradeMoveSpeedButton.SetAppearance(_goldService.CurrentAmount >= CurrentMoveSpeedCost);

            _shopView.UpgradeNoiseReductionButton.SetStatText($"-{(1f - _annoyanceService.NoiseMultiplier) * 100f:0}%");
            _shopView.UpgradeNoiseReductionButton.SetCostText(CurrentNoiseReductionCost);
            _shopView.UpgradeNoiseReductionButton.SetAppearance(_goldService.CurrentAmount >= CurrentNoiseReductionCost);

            _shopView.UpgradeUnitGoldButton.SetStatText($"+{_unitService.GoldPerTripBonus}");
            _shopView.UpgradeUnitGoldButton.SetCostText(CurrentUnitGoldCost);
            _shopView.UpgradeUnitGoldButton.SetAppearance(_goldService.CurrentAmount >= CurrentUnitGoldCost);
        }
    }
}
