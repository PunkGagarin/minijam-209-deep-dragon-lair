using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldPresenter : IInitializable
    {
        [Inject] private GoldService _goldService;
        [Inject] private GoldView _goldView;
        [Inject] private GoldPile _goldPile;

        public void Initialize()
        {
            _goldPile.OnClicked += HandleGoldClicked;
            _goldService.OnAmountChanged += UpdateView;
            UpdateView();
        }

        private void HandleGoldClicked(GoldPile _) => _goldService.Collect();

        private void UpdateView() => _goldView.SetAmount(_goldService.CurrentAmount);
    }
}
