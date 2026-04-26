using System;

using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldReservePresenter : IInitializable, IDisposable
    {
        [Inject] private GoldReserveService _service;
        [Inject] private GoldReserveView _view;

        public void Initialize()
        {
            _service.OnChanged += UpdateView;
            UpdateView();
        }

        public void Dispose()
        {
            if (_service != null)
                _service.OnChanged -= UpdateView;
        }

        private void UpdateView() => _view.SetFill(_service.Normalized);
    }
}
