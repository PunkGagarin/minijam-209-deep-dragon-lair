using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Gold
{
    public class GoldService
    {
        [Inject] private GoldModel _goldModel;

        public int CurrentAmount => _goldModel.Amount;

        public void Collect(int baseAmount = 1)
        {
            int total = Mathf.RoundToInt(baseAmount * _goldModel.Multiplier);
            _goldModel.Add(total);
        }
    }
}
