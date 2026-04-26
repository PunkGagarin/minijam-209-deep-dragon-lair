using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using _Project.Scripts.Audio.Domain;
using _Project.Scripts.Gameplay.Gold;
using _Project.Scripts.Gameplay.Units;
using Cysharp.Threading.Tasks;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class AnnoyancePresenter : IInitializable, IDisposable
    {
        [Inject] private AnnoyanceService _service;
        [Inject] private AnnoyanceConfig _config;
        [Inject] private AnnoyanceView _view;
        [Inject] private CaveGoldView _caveGoldView;
        [Inject] private UnitService _unitService;
        [Inject] private AudioService _audio;
        
        private readonly HashSet<Unit> _subscribed = new();
        private GoldPile _subscribedPile;
        private CancellationTokenSource _cts;

        public void Initialize()
        {
            _cts = new CancellationTokenSource();

            _caveGoldView.OnActiveGoldPileChanged += HandleActiveGoldPileChanged;
            _unitService.OnChanged += SyncUnitSubscriptions;
            _service.OnChanged += UpdateView;
            _service.OnFilled += HandleFilled;

            BindToGoldPile(_caveGoldView.ActiveGoldPile);
            SyncUnitSubscriptions();
            MovementTickLoop(_cts.Token).Forget();
            UpdateView();
        }

        public void Dispose()
        {
            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

            _caveGoldView.OnActiveGoldPileChanged -= HandleActiveGoldPileChanged;
            BindToGoldPile(null);

            if (_unitService != null)
                _unitService.OnChanged -= SyncUnitSubscriptions;

            if (_service != null)
            {
                _service.OnChanged -= UpdateView;
                _service.OnFilled -= HandleFilled;
            }

            foreach (Unit unit in _subscribed)
            {
                if (unit != null)
                    unit.OnMiningCompleted -= HandleUnitMiningCompleted;
            }
            _subscribed.Clear();
        }

        private void HandleActiveGoldPileChanged(GoldPile goldPile) => BindToGoldPile(goldPile);

        private void BindToGoldPile(GoldPile goldPile)
        {
            if (_subscribedPile != null)
                _subscribedPile.OnClicked -= HandleGoldClicked;

            _subscribedPile = goldPile;

            if (_subscribedPile != null)
                _subscribedPile.OnClicked += HandleGoldClicked;
        }

        private void HandleGoldClicked(GoldPile _)
        {
            _audio.PlaySound(Sounds.coin);
            if (Random.value < _config.GoldClickChance)
                _service.Add(_config.GoldClickAmount);
        }

        private void HandleUnitMiningCompleted(Unit _)
        {
            if (Random.value < _config.MiningCompletedChance)
                _service.Add(_config.MiningCompletedAmount);
        }

        private void HandleFilled() => WaitAndReset(_cts.Token).Forget();

        private async UniTaskVoid WaitAndReset(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_config.FillResetDelay), cancellationToken: token);
            _service.Reset();
        }

        private void SyncUnitSubscriptions()
        {
            IReadOnlyList<Unit> current = _unitService.Units;

            foreach (Unit unit in current)
            {
                if (unit == null)
                    continue;

                if (_subscribed.Add(unit))
                    unit.OnMiningCompleted += HandleUnitMiningCompleted;
            }

            List<Unit> removed = _subscribed.Where(u => u == null || !current.Contains(u)).ToList();
            foreach (Unit unit in removed)
            {
                if (unit != null)
                    unit.OnMiningCompleted -= HandleUnitMiningCompleted;
                _subscribed.Remove(unit);
            }
        }

        private async UniTaskVoid MovementTickLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_config.MovementTickInterval), cancellationToken: token);

                if (_service.IsFilled)
                    continue;

                IReadOnlyList<Unit> units = _unitService.Units;
                for (int i = 0; i < units.Count; i++)
                {
                    if (Random.value < _config.MovementTickChance)
                        _service.Add(_config.MovementTickAmount);
                }
            }
        }

        private void UpdateView() => _view.SetFill(_service.Normalized);
    }
}
