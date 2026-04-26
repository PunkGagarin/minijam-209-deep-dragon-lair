using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.Units
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _delayAtGuild = 1f;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Animator _animator;
        [SerializeField] private string _miningAnimationName;
        [SerializeField] private string _walkAnimationName;

        private Transform _guildPoint;
        private Transform _gatherGoldPoint;

        private CancellationTokenSource _cts;
        private bool _isGoingToGather = true;
        private float _mineTime;
        private float _speedMultiplier = 1f;

        public void SetSpeedMultiplier(float multiplier) => _speedMultiplier = multiplier;

        public event Action OnMiningCompleted = delegate { };
        public event Action OnReachedGuild = delegate { };

        public void Initialize(Transform guildPoint, Transform gatherGoldPoint, float mineTime)
        {
            if (guildPoint == null || gatherGoldPoint == null)
            {
                Debug.LogError($"{nameof(Movement)} on {name} cannot initialize because route points are missing.", this);
                return;
            }

            _guildPoint = guildPoint;
            _gatherGoldPoint = gatherGoldPoint;
            _mineTime = mineTime;
            _isGoingToGather = true;

            RestartPatrolLoop();
        }

        public void SetGatherPoint(Transform gatherGoldPoint)
        {
            if (gatherGoldPoint == null)
            {
                Debug.LogError($"{nameof(Movement)} on {name} cannot update gather point because it is missing.", this);
                return;
            }

            _gatherGoldPoint = gatherGoldPoint;

            if (_cts != null)
                RestartPatrolLoop();
        }

        private void OnDisable()
        {
            if (_cts == null)
                return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private void RestartPatrolLoop()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            PatrolLoop(_cts.Token).Forget();
        }

        private async UniTaskVoid PatrolLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (_isGoingToGather)
                    {
                        float xRandomBuffer = Random.Range(-1.5f, 1.5f);
                        float yRandomBuffer = Random.Range(-0.2f, 0.2f);
                        Vector3 gatherOffset = new Vector3(xRandomBuffer, yRandomBuffer, 0f);
                        await MoveToPoint(() => _gatherGoldPoint.position + gatherOffset, token);
                        if (_animator != null && !string.IsNullOrEmpty(_miningAnimationName)) 
                            _animator.Play(_miningAnimationName);
                        await UniTask.Delay(TimeSpan.FromSeconds(_mineTime), cancellationToken: token);
                        if (_animator != null && !string.IsNullOrEmpty(_walkAnimationName)) 
                            _animator.Play(_walkAnimationName);
                        OnMiningCompleted.Invoke();
                    }
                    else
                    {
                        await MoveToPoint(() => _guildPoint.position, token);
                        OnReachedGuild.Invoke();
                        await UniTask.Delay(TimeSpan.FromSeconds(_delayAtGuild), cancellationToken: token);
                    }

                    _isGoingToGather = !_isGoingToGather;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        private async UniTask MoveToPoint(Func<Vector3> targetProvider, CancellationToken token)
        {
            if (_animator != null && !string.IsNullOrEmpty(_walkAnimationName)) 
                _animator.Play(_walkAnimationName);
            while (!token.IsCancellationRequested)
            {
                Vector3 target = targetProvider.Invoke();
                float dx = target.x - transform.position.x;
                if (Mathf.Abs(dx) > Mathf.Epsilon && _spriteRenderer != null)
                    _spriteRenderer.flipX = dx < 0f;

                if (transform.position == target)
                    break;

                transform.position = Vector3.MoveTowards(transform.position, target, _speed * _speedMultiplier * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    }
}
