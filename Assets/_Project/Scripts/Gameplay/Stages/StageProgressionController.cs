using System;
using System.Collections.Generic;
using System.Threading;

using Cysharp.Threading.Tasks;

using Zenject;

using _Project.Scripts.Gameplay.EndGame;
using _Project.Scripts.Gameplay.Gold;

namespace _Project.Scripts.Gameplay.Stages
{
    public class StageProgressionController : IInitializable, IDisposable
    {
        private readonly GoldReserveService _goldReserveService;
        private readonly EndGameCameraReveal _cameraReveal;
        private readonly FinalStageSequence _finalStageSequence;
        private readonly StageProgressionConfig _config;

        private int _currentStageIndex;
        private bool _isTransitionRunning;
        private bool _isFinalSequenceRunning;
        private CancellationTokenSource _transitionCancellation;

        public StageProgressionController(
            GoldReserveService goldReserveService,
            EndGameCameraReveal cameraReveal,
            FinalStageSequence finalStageSequence,
            StageProgressionConfig config)
        {
            _goldReserveService = goldReserveService;
            _cameraReveal = cameraReveal;
            _finalStageSequence = finalStageSequence;
            _config = config;
        }

        public void Initialize()
        {
            IReadOnlyList<StageDefinition> stages = _config.Stages;
            if (stages == null || stages.Count == 0)
                throw new InvalidOperationException("StageProgressionConfig must contain at least one stage.");

            _currentStageIndex = 0;
            _cameraReveal.ApplyImmediate(stages[_currentStageIndex].CameraSettings);
            _goldReserveService.SetStageReserve(stages[_currentStageIndex].RequiredGold);
            _goldReserveService.OnDepleted += OnReserveDepleted;
            _transitionCancellation = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _goldReserveService.OnDepleted -= OnReserveDepleted;
            _transitionCancellation?.Cancel();
            _transitionCancellation?.Dispose();
        }

        private void OnReserveDepleted()
        {
            if (_isTransitionRunning || _isFinalSequenceRunning)
                return;

            StageDefinition currentStage = _config.Stages[_currentStageIndex];
            if (currentStage.IsFinalStage)
            {
                _isFinalSequenceRunning = true;
                PlayFinalSequenceAsync(_transitionCancellation.Token).Forget();
                return;
            }

            if (_currentStageIndex + 1 >= _config.Stages.Count)
                throw new InvalidOperationException("Current stage is not final, but no next stage exists.");

            _isTransitionRunning = true;
            AdvanceToNextStageAsync(_transitionCancellation.Token).Forget();
        }

        private async UniTaskVoid AdvanceToNextStageAsync(CancellationToken cancellationToken)
        {
            try
            {
                int nextStageIndex = _currentStageIndex + 1;
                StageDefinition nextStage = _config.Stages[nextStageIndex];

                await _cameraReveal.PlayReveal(nextStage.CameraSettings, cancellationToken);

                _currentStageIndex = nextStageIndex;
                _goldReserveService.SetStageReserve(nextStage.RequiredGold);
            }
            catch (OperationCanceledException)
            {
            }
            finally
            {
                _isTransitionRunning = false;
            }
        }

        private async UniTaskVoid PlayFinalSequenceAsync(CancellationToken cancellationToken)
        {
            try
            {
                await _finalStageSequence.PlayAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}
