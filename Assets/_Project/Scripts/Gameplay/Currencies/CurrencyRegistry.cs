using System.Collections.Generic;

namespace _Project.Scripts.Gameplay.Currencies
{
    public class CurrencyRegistry
    {
        private readonly Dictionary<CurrencyType, ICurrencyWallet> _wallets = new();

        public void Register(CurrencyType currency, ICurrencyWallet wallet) => _wallets[currency] = wallet;

        public ICurrencyWallet Get(CurrencyType currency) => _wallets[currency];

        public int GetAmount(CurrencyType currency) => _wallets[currency].CurrentAmount;

        public bool TrySpend(CurrencyType currency, int amount) => _wallets[currency].TrySpend(amount);
    }
}
