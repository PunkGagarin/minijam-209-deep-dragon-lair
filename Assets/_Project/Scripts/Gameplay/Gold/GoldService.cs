using System;

using _Project.Scripts.Gameplay.Currencies;

using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldService : ICurrencyWallet
    {
        [Inject] private GoldModel _goldModel;
        [Inject] private GoldReserveService _reserve;

        public event Action OnAmountChanged = delegate { };

        public int CurrentAmount => _goldModel.Amount;
        public int CurrentBonusPerClick => _goldModel.BonusPerClick;

        public void CollectFromClick(int baseAmount = 1)
        {
            int total = Mathf.RoundToInt((baseAmount + _goldModel.BonusPerClick) * _goldModel.Multiplier);
            int actual = _reserve.TryConsume(total);
            if (actual <= 0)
                return;

            _goldModel.Add(actual);
            OnAmountChanged.Invoke();
        }

        public void CollectFromUnit(int amount)
        {
            int actual = _reserve.TryConsume(amount);
            if (actual <= 0)
                return;

            _goldModel.Add(actual);
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
