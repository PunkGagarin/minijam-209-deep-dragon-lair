using System;

using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldReserveService : IInitializable
    {
        [Inject] private GoldReserveModel _model;
        [Inject] private GoldReserveConfig _config;

        public event Action OnChanged = delegate { };
        public event Action OnDepleted = delegate { };

        public bool IsDepleted { get; private set; }
        public float Normalized => _model.Normalized;
        public int Current => _model.Current;
        public int Max => _model.Max;

        public void Initialize()
        {
            _model.SetMax(_config.MaxValue);
        }

        public int TryConsume(int requested)
        {
            if (IsDepleted || requested <= 0)
                return 0;

            int taken = _model.Consume(requested);
            if (taken > 0)
                OnChanged.Invoke();

            if (_model.Current <= 0)
            {
                IsDepleted = true;
                OnDepleted.Invoke();
            }

            return taken;
        }
    }
}
