using System;
using System.Collections.Generic;
using _Project.Scripts.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.GuildHall
{
    public class GuildHallShopView : ContentUi
    {
        [SerializeField] private Button _upgradeGoldPerClickButton;
        [SerializeField] private List<Button> _closeButtons;
        [SerializeField] private TMP_Text _bonusGoldPerClickText;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private float _shakeDuration = 0.4f;
        [SerializeField] private float _shakeStrength = 10f;
        [SerializeField] private int _shakeVibrato = 20;

        public event Action OnUpgradeClicked = delegate { };
        public event Action OnCloseClicked = delegate { };

        private Tween _shakeTween;
        private Color _buttonNormalColor;
        private Color _buttonHighlightedColor;
        private Color _buttonSelectedColor;

        private void Awake()
        {
            ColorBlock initialColors = _upgradeGoldPerClickButton.colors;
            _buttonNormalColor = initialColors.normalColor;
            _buttonHighlightedColor = initialColors.highlightedColor;
            _buttonSelectedColor = initialColors.selectedColor;
            _upgradeGoldPerClickButton.onClick.AddListener(() => OnUpgradeClicked.Invoke());

            foreach (Button closeButton in _closeButtons)
                closeButton.onClick.AddListener(() => OnCloseClicked.Invoke());

            Hide();
        }

        private void OnDestroy()
        {
            _upgradeGoldPerClickButton.onClick.RemoveAllListeners();

            foreach (Button closeButton in _closeButtons)
                closeButton.onClick.RemoveAllListeners();
        }

        public void SetBonusGoldPerClickText(int bonus) => _bonusGoldPerClickText.text = $"+{bonus+1}";

        public void SetCostText(int cost) => _costText.text = cost.ToString();

        public void SetUpgradeButtonAppearance(bool canAfford)
        {
            ColorBlock colors = _upgradeGoldPerClickButton.colors;
            Color disabled = colors.disabledColor;
            colors.normalColor = canAfford ? _buttonNormalColor : disabled;
            colors.highlightedColor = canAfford ? _buttonHighlightedColor : disabled;
            colors.selectedColor = canAfford ? _buttonSelectedColor : disabled;
            _upgradeGoldPerClickButton.colors = colors;
        }

        public void PlayInsufficientFundsShake()
        {
            _shakeTween?.Kill(complete: true);
            _shakeTween = _upgradeGoldPerClickButton.transform
                .DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);
        }
    }
}
