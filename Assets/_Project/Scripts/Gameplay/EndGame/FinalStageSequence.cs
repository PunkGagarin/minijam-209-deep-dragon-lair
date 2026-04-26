using System;
using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.EndGame
{
    public class FinalStageSequence : MonoBehaviour
    {
        [SerializeField] private GameObject _animatedRoot;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _animationStateName;
        [SerializeField] private float _uiDelay = 1f;

        [Inject] private EndGameUI _endGameUi;

        private bool _isRunning;

        public async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _endGameUi.Hide();

            if (_animatedRoot != null)
                _animatedRoot.SetActive(true);

            if (_animator != null)
            {
                _animator.Rebind();
                _animator.Update(0f);

                if (string.IsNullOrWhiteSpace(_animationStateName))
                    _animator.Play(0, 0, 0f);
                else
                    _animator.Play(_animationStateName, 0, 0f);
            }

            if (_uiDelay > 0f)
                await UniTask.Delay(TimeSpan.FromSeconds(_uiDelay), cancellationToken: cancellationToken);

            _endGameUi.Show();
        }
    }
}
