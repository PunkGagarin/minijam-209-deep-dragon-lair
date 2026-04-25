using System;
using System.Collections.Generic;
using _Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.GuildHall
{
    public class GuildHallShopView : ContentUi
    {
        [SerializeField] private ShopButtonView _upgradeGoldPerClickButton;
        [SerializeField] private ShopButtonView _buyUnitButton;
        [SerializeField] private ShopButtonView _upgradeMoveSpeedButton;
        [SerializeField] private ShopButtonView _upgradeNoiseReductionButton;
        [SerializeField] private ShopButtonView _upgradeUnitGoldButton;
        [SerializeField] private List<Button> _closeButtons;

        public ShopButtonView UpgradeGoldPerClickButton => _upgradeGoldPerClickButton;
        public ShopButtonView BuyUnitButton => _buyUnitButton;
        public ShopButtonView UpgradeMoveSpeedButton => _upgradeMoveSpeedButton;
        public ShopButtonView UpgradeNoiseReductionButton => _upgradeNoiseReductionButton;
        public ShopButtonView UpgradeUnitGoldButton => _upgradeUnitGoldButton;

        public event Action OnCloseClicked = delegate { };

        private void Awake()
        {
            foreach (Button closeButton in _closeButtons)
                closeButton.onClick.AddListener(() => OnCloseClicked.Invoke());

            Hide();
        }

        private void OnDestroy()
        {
            foreach (Button closeButton in _closeButtons)
                closeButton.onClick.RemoveAllListeners();
        }
    }
}
