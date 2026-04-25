using System;

using Zenject;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class AnnoyanceService : IInitializable
    {
        [Inject] private AnnoyanceModel _model;
        [Inject] private AnnoyanceConfig _config;

        public event Action OnChanged = delegate { };
        public event Action OnFilled = delegate { };

        public float Normalized => _model.Normalized;
        public bool IsFilled { get; private set; }

        public void Initialize()
        {
            _model.SetMax(_config.MaxValue);
        }

        public void Add(float amount)
        {
            if (IsFilled)
                return;

            _model.Add(amount);
            OnChanged.Invoke();

            if (_model.Current >= _model.Max)
            {
                IsFilled = true;
                OnFilled.Invoke();
            }
        }

        public void Reset()
        {
            _model.Reset();
            IsFilled = false;
            OnChanged.Invoke();
        }
    }
}
