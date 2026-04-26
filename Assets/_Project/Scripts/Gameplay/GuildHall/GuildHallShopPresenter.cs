using System;

using _Project.Scripts.Gameplay.Currencies;
using _Project.Scripts.Gameplay.Dragon;
using _Project.Scripts.Gameplay.Gem;
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
        [Inject] private GemService _gemService;

        private int _goldPerClickUpgradeLevel = 0;
        private int _moveSpeedUpgradeLevel = 0;
        private int _noiseReductionUpgradeLevel = 0;
        private int _unitGoldUpgradeLevel = 0;
        private int _gemChanceUpgradeLevel = 0;
        private int _gemQuantityUpgradeLevel = 0;
        private int _crowGoldUpgradeLevel = 0;

        private bool _gemUpgradesUnlocked;

        private int CurrentCost =>
            Mathf.RoundToInt(_config.BaseCost * Mathf.Pow(_config.CostMultiplier, _goldPerClickUpgradeLevel));

        private int CurrentMoveSpeedCost =>
            Mathf.RoundToInt(_config.MoveSpeedBaseCost * Mathf.Pow(_config.MoveSpeedCostMultiplier, _moveSpeedUpgradeLevel));

        private int CurrentNoiseReductionCost =>
            Mathf.RoundToInt(_config.NoiseReductionBaseCost * Mathf.Pow(_config.NoiseReductionCostMultiplier, _noiseReductionUpgradeLevel));

        private int CurrentUnitGoldCost =>
            Mathf.RoundToInt(_config.UnitGoldBaseCost * Mathf.Pow(_config.UnitGoldCostMultiplier, _unitGoldUpgradeLevel));

        private int CurrentGemChanceCost =>
            Mathf.RoundToInt(_config.GemChanceBaseCost * Mathf.Pow(_config.GemChanceCostMultiplier, _gemChanceUpgradeLevel));

        private int CurrentGemQuantityCost =>
            Mathf.RoundToInt(_config.GemQuantityBaseCost * Mathf.Pow(_config.GemQuantityCostMultiplier, _gemQuantityUpgradeLevel));
        private int CurrentCrowGoldCost =>
            Mathf.RoundToInt(_config.CrowGoldBaseCost * Mathf.Pow(_config.CrowGoldCostMultiplier, _crowGoldUpgradeLevel));

        public void Initialize()
        {
            _guildHall.OnClicked += HandleBuildingClicked;
            _shopView.UpgradeGoldPerClickButton.OnClicked += HandleUpgrade;
            _shopView.BuyUnitButton.OnClicked += HandleBuyUnit;
            _shopView.UpgradeMoveSpeedButton.OnClicked += HandleUpgradeMoveSpeed;
            _shopView.UpgradeNoiseReductionButton.OnClicked += HandleUpgradeNoiseReduction;
            _shopView.UpgradeUnitGoldButton.OnClicked += HandleUpgradeUnitGold;
            _shopView.UpgradeGemChanceButton.OnClicked += HandleUpgradeGemChance;
            _shopView.UpgradeGemQuantityButton.OnClicked += HandleUpgradeGemQuantity;
            _shopView.BuyCrowButton.OnClicked += HandleBuyCrow;
            _shopView.CrowGoldButton.OnClicked += HandleUpgradeCrowGold;
            _shopView.OnCloseClicked += HandleClose;
            _goldService.OnAmountChanged += UpdateView;
            _unitService.OnChanged += UpdateView;

            _gemService.OnAmountChanged += HandleGemAmountChanged;
            ApplyGemUnlockState();

            UpdateView();
        }

        private void HandleGemAmountChanged()
        {
            if (_gemUpgradesUnlocked || _gemService.CurrentAmount <= 0)
                return;

            ApplyGemUnlockState();
            UpdateView();
        }

        private void ApplyGemUnlockState()
        {
            _gemUpgradesUnlocked = _gemService.CurrentAmount > 0;

            foreach (var button in _shopView.GetButtonsByCurrency(CurrencyType.Gem))
            {
                button.gameObject.SetActive(_gemService.CurrentAmount > 0);
            }
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
            if (_goldService.CurrentAmount < _unitService.CurrentUnitCost)
            {
                _shopView.BuyUnitButton.PlayInsufficientFundsShake();
                return;
            }

            _unitService.TryPurchaseUnit();
        }
        
        private void HandleBuyCrow()
        {
            if (_gemService.CurrentAmount < _unitService.CurrentCrowCost)
            {
                _shopView.BuyUnitButton.PlayInsufficientFundsShake();
                return;
            }

            _unitService.TryPurchaseCrow();
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
        
        private void HandleUpgradeCrowGold()
        {
            if (!_gemService.TrySpend(CurrentCrowGoldCost))
            {
                _shopView.UpgradeUnitGoldButton.PlayInsufficientFundsShake();
                return;
            }

            _crowGoldUpgradeLevel++;
            _unitService.UpgradeGoldPerCrowTrip(_config.CrowGoldBonusPerUpgrade);
            UpdateView();
        }

        private void HandleUpgradeGemChance()
        {
            if (!TrySpendVia(_shopView.UpgradeGemChanceButton, CurrentGemChanceCost))
            {
                _shopView.UpgradeGemChanceButton.PlayInsufficientFundsShake();
                return;
            }

            _gemChanceUpgradeLevel++;
            _unitService.UpgradeGemDropChance(_config.GemChanceBonusPerUpgrade);
            UpdateView();
        }

        private void HandleUpgradeGemQuantity()
        {
            if (!TrySpendVia(_shopView.UpgradeGemQuantityButton, CurrentGemQuantityCost))
            {
                _shopView.UpgradeGemQuantityButton.PlayInsufficientFundsShake();
                return;
            }

            _gemQuantityUpgradeLevel++;
            _unitService.UpgradeGemDropAmount(_config.GemQuantityBonusPerUpgrade);
            UpdateView();
        }

        private bool TrySpendVia(ShopButtonView button, int amount) => button.Currency switch
        {
            CurrencyType.Gold => _goldService.TrySpend(amount),
            CurrencyType.Gem => _gemService.TrySpend(amount),
            _ => throw new ArgumentOutOfRangeException(nameof(button), button.Currency, null),
        };

        private int GetAmountVia(ShopButtonView button) => button.Currency switch
        {
            CurrencyType.Gold => _goldService.CurrentAmount,
            CurrencyType.Gem => _gemService.CurrentAmount,
            _ => throw new ArgumentOutOfRangeException(nameof(button), button.Currency, null),
        };

        private void UpdateView()
        {
            _shopView.UpgradeGoldPerClickButton.SetStatText($"+{_goldService.CurrentBonusPerClick + 1}");
            _shopView.UpgradeGoldPerClickButton.SetCostText(CurrentCost);
            _shopView.UpgradeGoldPerClickButton.SetAppearance(_goldService.CurrentAmount >= CurrentCost);

            _shopView.BuyUnitButton.SetStatText(_unitService.Count.ToString());
            _shopView.BuyUnitButton.SetCostText(_unitService.CurrentUnitCost);
            _shopView.BuyUnitButton.SetAppearance(
                _unitService.CanPurchaseUnit && _goldService.CurrentAmount >= _unitService.CurrentUnitCost);

            _shopView.UpgradeMoveSpeedButton.SetStatText($"x{_unitService.MoveSpeedMultiplier:0.00}");
            _shopView.UpgradeMoveSpeedButton.SetCostText(CurrentMoveSpeedCost);
            _shopView.UpgradeMoveSpeedButton.SetAppearance(_goldService.CurrentAmount >= CurrentMoveSpeedCost);

            _shopView.UpgradeNoiseReductionButton.SetStatText($"-{(1f - _annoyanceService.NoiseMultiplier) * 100f:0}%");
            _shopView.UpgradeNoiseReductionButton.SetCostText(CurrentNoiseReductionCost);
            _shopView.UpgradeNoiseReductionButton.SetAppearance(_goldService.CurrentAmount >= CurrentNoiseReductionCost);

            _shopView.UpgradeUnitGoldButton.SetStatText($"+{_unitService.GoldPerTripBonus}");
            _shopView.UpgradeUnitGoldButton.SetCostText(CurrentUnitGoldCost);
            _shopView.UpgradeUnitGoldButton.SetAppearance(_goldService.CurrentAmount >= CurrentUnitGoldCost);

            _shopView.UpgradeGemChanceButton.SetStatText($"{_unitService.GemDropChance * 100f:0}%");
            _shopView.UpgradeGemChanceButton.SetCostText(CurrentGemChanceCost);
            _shopView.UpgradeGemChanceButton.SetAppearance(GetAmountVia(_shopView.UpgradeGemChanceButton) >= CurrentGemChanceCost);

            _shopView.UpgradeGemQuantityButton.SetStatText($"+{_unitService.GemDropAmount}");
            _shopView.UpgradeGemQuantityButton.SetCostText(CurrentGemQuantityCost);
            _shopView.UpgradeGemQuantityButton.SetAppearance(GetAmountVia(_shopView.UpgradeGemQuantityButton) >= CurrentGemQuantityCost);
            
            _shopView.BuyCrowButton.SetStatText(_unitService.CrowCount.ToString());
            _shopView.BuyCrowButton.SetCostText(_unitService.CurrentCrowCost);
            _shopView.BuyCrowButton.SetAppearance(_gemService.CurrentAmount >= _unitService.CurrentCrowCost);
            
            _shopView.CrowGoldButton.SetStatText($"+{_unitService.GoldPerCrowTripBonus}");
            _shopView.CrowGoldButton.SetCostText(CurrentCrowGoldCost);
            _shopView.CrowGoldButton.SetAppearance(_gemService.CurrentAmount >= CurrentCrowGoldCost);
        }
    }
}
