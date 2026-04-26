using System;

using Zenject;

namespace _Project.Scripts.Gameplay.Gem
{
    public class GemPresenter : IInitializable, IDisposable
    {
        [Inject] private GemService _gemService;
        [Inject] private GemView _gemView;

        public void Initialize()
        {
            _gemService.OnAmountChanged += UpdateView;
            UpdateView();
        }

        public void Dispose()
        {
            _gemService.OnAmountChanged -= UpdateView;
        }

        private void UpdateView() => _gemView.SetAmount(_gemService.CurrentAmount);
    }
}
