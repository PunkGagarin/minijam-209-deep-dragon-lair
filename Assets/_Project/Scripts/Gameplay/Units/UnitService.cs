using System;
using System.Collections.Generic;

using _Project.Scripts.Gameplay.Gem;
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
        [Inject] private GemService _gemService;

        private readonly List<Unit> _units = new();
        private readonly List<Unit> _crows = new();

        public event Action OnChanged = delegate { };
        public event Action<Unit, int> OnGemDropped = delegate { };

        public IReadOnlyList<Unit> Units => _units;
        public int Count => _units.Count;
        public int CrowCount => _crows.Count;
        public int CurrentUnitCost =>
            Mathf.RoundToInt(_shopConfig.UnitBaseCost * Mathf.Pow(_shopConfig.UnitCostMultiplier, _units.Count));        
        public int CurrentCrowCost =>
            Mathf.RoundToInt(_shopConfig.CrowUnitBaseCost * Mathf.Pow(_shopConfig.CrowUnitBaseCostMultiplier, _crows.Count));

        public float MoveSpeedMultiplier { get; private set; } = 1f;
        public int GoldPerTripBonus { get; private set; } = 0;
        public int GoldPerCrowTripBonus { get; private set; } = 0;
        public float GemDropChance { get; private set; } = 0f;
        public int GemDropAmount { get; private set; } = 1;

        public bool CanPurchaseUnit => _spawner.CanSpawn;

        public bool TryPurchaseUnit()
        {
            if (!CanPurchaseUnit)
                return false;

            int currentCost = CurrentUnitCost;
            if (!_goldService.TrySpend(currentCost))
                return false;

            Unit unit = _spawner.SpawnUnit(_unitConfig.MineTime, UnitType.Unit);
            unit.GetComponent<Movement>().SetSpeedMultiplier(MoveSpeedMultiplier);

            SubscribeToUnit(unit, UnitType.Unit);
            _units.Add(unit);
            OnChanged.Invoke();
            return true;
        }
        
        public bool TryPurchaseCrow()
        {
            if (!CanPurchaseUnit)
                return false;

            int currentCost = CurrentCrowCost;
            if (!_gemService.TrySpend(currentCost))
                return false;

            Unit unit = _spawner.SpawnUnit(0f, UnitType.Crow);
            unit.GetComponent<Movement>().SetSpeedMultiplier(5f);

            SubscribeToUnit(unit, UnitType.Crow);
            _crows.Add(unit);
            OnChanged.Invoke();
            return true;
        }

        public void UpgradeMoveSpeed(float bonusPerUpgrade)
        {
            MoveSpeedMultiplier *= 1f + bonusPerUpgrade;
            foreach (Unit unit in _units)
                unit.GetComponent<Movement>().SetSpeedMultiplier(MoveSpeedMultiplier);
        }

        public void UpgradeGoldPerTrip(int bonus) => GoldPerTripBonus += bonus;
        public void UpgradeGoldPerCrowTrip(int bonus) => GoldPerCrowTripBonus += bonus;

        public void UpgradeGemDropChance(float delta) => GemDropChance = Mathf.Clamp01(GemDropChance + delta);

        public void UpgradeGemDropAmount(int delta) => GemDropAmount += delta;

        public void SetGatherPoint(Transform gatherPoint)
        {
            if (gatherPoint == null)
                throw new ArgumentNullException(nameof(gatherPoint));

            _spawner.SetGatherGoldPoint(gatherPoint);

            foreach (Unit unit in _units)
                unit.SetGatherPoint(gatherPoint);
            
            foreach (Unit unit in _crows)
                unit.SetGatherPoint(gatherPoint);
        }

        private void SubscribeToUnit(Unit unit, UnitType unitType)
        {
            unit.OnReturnedToGuild += HandleUnitReturnedToGuild;
            unit.OnDied += HandleUnitDied;
        }

        private void HandleUnitReturnedToGuild(Unit unit, UnitType unitType)
        {
            var goldPerTrip = 0;
            var tripBonus = 0;

            if (unitType == UnitType.Unit)
            {
                goldPerTrip = _unitConfig.BaseGoldPerTrip;
                tripBonus = GoldPerTripBonus;
            }
            else
            {
                goldPerTrip = _unitConfig.BaseCrowGoldPerTrip;
                tripBonus = GoldPerCrowTripBonus;
            }
            
            
            _goldService.CollectFromUnit(goldPerTrip + tripBonus);

            if (GemDropChance <= 0f || UnityEngine.Random.value >= GemDropChance)
                return;

            _gemService.CollectFromUnit(GemDropAmount);
            OnGemDropped.Invoke(unit, GemDropAmount);
        }

        private void HandleUnitDied(Unit unit)
        {
            unit.OnReturnedToGuild -= HandleUnitReturnedToGuild;
            unit.OnDied -= HandleUnitDied;

            if (_units.Remove(unit))
                OnChanged.Invoke();
        }
    }

    public enum UnitType
    {
        Unit = 1,
        Crow = 2
    }
}
