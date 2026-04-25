using System;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldService
    {
        [Inject] private GoldModel _goldModel;

        public event Action OnAmountChanged = delegate { };

        public int CurrentAmount => _goldModel.Amount;
        public int CurrentBonusPerClick => _goldModel.BonusPerClick;

        public void Collect(int baseAmount = 1)
        {
            int total = Mathf.RoundToInt((baseAmount + _goldModel.BonusPerClick) * _goldModel.Multiplier);
            _goldModel.Add(total);
            OnAmountChanged.Invoke();
        }

        public bool TrySpend(int amount)
        {
            if (_goldModel.Amount < amount)
                return false;

            _goldModel.Spend(amount);
            OnAmountChanged.Invoke();
            return true;
        }

        public void UpgradeBonusPerClick(int bonus) => _goldModel.AddBonusPerClick(bonus);
    }
}
