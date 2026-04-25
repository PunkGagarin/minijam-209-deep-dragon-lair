using System;
using System.Collections.Generic;

using _Project.Scripts.Gameplay.GuildHall;
using _Project.Scripts.Gameplay.Gold;

using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Units
{
    public class UnitService
    {
        [Inject] private UnitSpawner _spawner;
        [Inject] private GuildHallShopConfig _shopConfig;
        [Inject] private UnitConfig _unitConfig;
        [Inject] private GoldService _goldService;

        private readonly List<Unit> _units = new();

        public event Action OnChanged = delegate { };

        public IReadOnlyList<Unit> Units => _units;
        public int Count => _units.Count;
        public int CurrentCost =>
            Mathf.RoundToInt(_shopConfig.UnitBaseCost * Mathf.Pow(_shopConfig.UnitCostMultiplier, _units.Count));

        public bool CanPurchaseUnit => _spawner.CanSpawn;

        public bool TryPurchaseUnit()
        {
            if (!CanPurchaseUnit)
                return false;

            int currentCost = CurrentCost;
            if (!_goldService.TrySpend(currentCost))
                return false;

            Unit unit = _spawner.SpawnUnit(_unitConfig.MineTime);

            SubscribeToUnit(unit);
            _units.Add(unit);
            OnChanged.Invoke();
            return true;
        }

        private void SubscribeToUnit(Unit unit)
        {
            unit.OnReturnedToGuild += HandleUnitReturnedToGuild;
            unit.OnDied += HandleUnitDied;
        }

        private void HandleUnitReturnedToGuild(Unit unit)
        {
            _goldService.CollectFromUnit(_unitConfig.BaseGoldPerTrip);
        }

        private void HandleUnitDied(Unit unit)
        {
            unit.OnReturnedToGuild -= HandleUnitReturnedToGuild;
            unit.OnDied -= HandleUnitDied;

            if (_units.Remove(unit))
                OnChanged.Invoke();
        }
    }
}
