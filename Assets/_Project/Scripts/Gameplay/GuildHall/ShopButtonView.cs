using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.Gameplay.GuildHall
{
    public class ShopButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _costText;
        [SerializeField] private TMP_Text _statText;
        [SerializeField] private float _shakeDuration = 0.4f;
        [SerializeField] private float _shakeStrength = 10f;
        [SerializeField] private int _shakeVibrato = 20;

        public event Action OnClicked = delegate { };

        private Tween _shakeTween;
        private Color _buttonNormalColor;
        private Color _buttonHighlightedColor;
        private Color _buttonSelectedColor;
        private bool _originalColorsCaptured;

        private void Awake()
        {
            CaptureOriginalColors();
            _button.onClick.AddListener(() => OnClicked.Invoke());
        }

        private void CaptureOriginalColors()
        {
            if (_originalColorsCaptured)
                return;

            ColorBlock initialColors = _button.colors;
            _buttonNormalColor = initialColors.normalColor;
            _buttonHighlightedColor = initialColors.highlightedColor;
            _buttonSelectedColor = initialColors.selectedColor;
            _originalColorsCaptured = true;
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveAllListeners();
        }

        public void SetCostText(int cost) => _costText.text = cost.ToString();

        public void SetStatText(string text) => _statText.text = text;

        public void SetAppearance(bool canAfford)
        {
            CaptureOriginalColors();
            ColorBlock colors = _button.colors;
            Color disabled = colors.disabledColor;
            colors.normalColor = canAfford ? _buttonNormalColor : disabled;
            colors.highlightedColor = canAfford ? _buttonHighlightedColor : disabled;
            colors.selectedColor = canAfford ? _buttonSelectedColor : disabled;
            _button.colors = colors;
        }

        public void PlayInsufficientFundsShake()
        {
            _shakeTween?.Kill(complete: true);
            _shakeTween = _button.transform.DOShakePosition(_shakeDuration, _shakeStrength, _shakeVibrato);
        }
    }
}
