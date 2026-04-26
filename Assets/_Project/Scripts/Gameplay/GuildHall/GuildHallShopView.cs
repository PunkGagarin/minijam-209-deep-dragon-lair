using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Audio.Domain;
using _Project.Scripts.Gameplay.Currencies;
using _Project.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.Gameplay.GuildHall
{
    public class GuildHallShopView : ContentUi
    {
        [SerializeField] private ShopButtonView _upgradeGoldPerClickButton;
        [SerializeField] private ShopButtonView _buyUnitButton;
        [SerializeField] private ShopButtonView _upgradeMoveSpeedButton;
        [SerializeField] private ShopButtonView _upgradeNoiseReductionButton;
        [SerializeField] private ShopButtonView _upgradeUnitGoldButton;
        [SerializeField] private ShopButtonView _upgradeGemChanceButton;
        [SerializeField] private ShopButtonView _upgradeGemQuantityButton;
        [SerializeField] private List<Button> _closeButtons;
        [SerializeField] private List<ShopButtonView> _shopButtons;
        
        [Inject] private AudioService _audio;

        public ShopButtonView UpgradeGoldPerClickButton => _upgradeGoldPerClickButton;
        public ShopButtonView BuyUnitButton => _buyUnitButton;
        public ShopButtonView UpgradeMoveSpeedButton => _upgradeMoveSpeedButton;
        public ShopButtonView UpgradeNoiseReductionButton => _upgradeNoiseReductionButton;
        public ShopButtonView UpgradeUnitGoldButton => _upgradeUnitGoldButton;
        public ShopButtonView UpgradeGemChanceButton => _upgradeGemChanceButton;
        public ShopButtonView UpgradeGemQuantityButton => _upgradeGemQuantityButton;

        public event Action OnCloseClicked = delegate { };

        private void Awake()
        {
            foreach (Button closeButton in _closeButtons)
                closeButton.onClick.AddListener(() =>
                {
                    _audio.PlaySound(Sounds.buttonClick.ToString());
                    OnCloseClicked.Invoke();
                });

            Hide();
        }

        public List<ShopButtonView> GetButtonsByCurrency(CurrencyType currencyType)
        {
            if (_shopButtons.Count == 0)
            {
                _shopButtons.AddRange(GetComponentsInChildren<ShopButtonView>(true));
            }
            
            return _shopButtons.Where( el => el.Currency == currencyType).ToList();
        }

        private void OnDestroy()
        {
            foreach (Button closeButton in _closeButtons)
                closeButton.onClick.RemoveAllListeners();
        }
    }
}
