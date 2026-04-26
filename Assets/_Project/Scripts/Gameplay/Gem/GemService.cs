using System;

using Zenject;

namespace _Project.Scripts.Gameplay.Gem
{
    public class GemService
    {
        [Inject] private GemModel _gemModel;

        public event Action OnAmountChanged = delegate { };

        public int CurrentAmount => _gemModel.Amount;

        public void CollectFromUnit(int amount)
        {
            if (amount <= 0)
                return;

            _gemModel.Add(amount);
            OnAmountChanged.Invoke();
        }

        public bool TrySpend(int amount)
        {
            if (_gemModel.Amount < amount)
                return false;

            _gemModel.Spend(amount);
            OnAmountChanged.Invoke();
            return true;
        }
    }
}
