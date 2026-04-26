using System;

namespace _Project.Scripts.Gameplay.Currencies
{
    public interface ICurrencyWallet
    {
        int CurrentAmount { get; }
        event Action OnAmountChanged;
        bool TrySpend(int amount);
    }
}
