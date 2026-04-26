using System;

using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldPresenter : IInitializable, IDisposable
    {
        [Inject] private GoldService _goldService;
        [Inject] private GoldView _goldView;
        [Inject] private CaveGoldView _caveGoldView;

        private GoldPile _subscribedPile;

        public void Initialize()
        {
            _caveGoldView.OnActiveGoldPileChanged += HandleActiveGoldPileChanged;
            _goldService.OnAmountChanged += UpdateView;

            BindToGoldPile(_caveGoldView.ActiveGoldPile);
            UpdateView();
        }

        public void Dispose()
        {
            _caveGoldView.OnActiveGoldPileChanged -= HandleActiveGoldPileChanged;
            _goldService.OnAmountChanged -= UpdateView;
            BindToGoldPile(null);
        }

        private void HandleActiveGoldPileChanged(GoldPile goldPile) => BindToGoldPile(goldPile);

        private void BindToGoldPile(GoldPile goldPile)
        {
            if (_subscribedPile != null)
                _subscribedPile.OnClicked -= HandleGoldClicked;

            _subscribedPile = goldPile;

            if (_subscribedPile != null)
                _subscribedPile.OnClicked += HandleGoldClicked;
        }

        private void HandleGoldClicked(GoldPile _) => _goldService.CollectFromClick();

        private void UpdateView() => _goldView.SetAmount(_goldService.CurrentAmount);
    }
}
