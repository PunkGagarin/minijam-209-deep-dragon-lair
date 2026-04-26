using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Gameplay.Units
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _delayAtGuild = 1f;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private Transform _guildPoint;
        private Transform _gatherGoldPoint;

        private CancellationTokenSource _cts;
        private float _mineTime;
        private float _speedMultiplier = 1f;

        public void SetSpeedMultiplier(float multiplier) => _speedMultiplier = multiplier;

        public event System.Action OnMiningCompleted = delegate { };
        public event System.Action OnReachedGuild = delegate { };

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

            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new CancellationTokenSource();
            PatrolLoop(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            if (_cts == null)
                return;

            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        private async UniTaskVoid PatrolLoop(CancellationToken token)
        {
            bool goingToGather = true;

            while (!token.IsCancellationRequested)
            {
                if (goingToGather)
                {
                    var xRandomBuffer = Random.Range(-1.5f, 1.5f);
                    var yRandomBuffer = Random.Range(-.2f, .2f);
                    await MoveToPoint(_gatherGoldPoint.position + new Vector3(xRandomBuffer, yRandomBuffer, 0f), token);
                    await UniTask.Delay(System.TimeSpan.FromSeconds(_mineTime), cancellationToken: token);
                    OnMiningCompleted.Invoke();
                }
                else
                {
                    await MoveToPoint(_guildPoint.position, token);
                    OnReachedGuild.Invoke();
                    await UniTask.Delay(System.TimeSpan.FromSeconds(_delayAtGuild), cancellationToken: token);
                }

                goingToGather = !goingToGather;
            }
        }

        private async UniTask MoveToPoint(Vector3 target, CancellationToken token)
        {
            float dx = target.x - transform.position.x;
            if (Mathf.Abs(dx) > Mathf.Epsilon && _spriteRenderer != null)
                _spriteRenderer.flipX = dx < 0f;

            while (transform.position != target && !token.IsCancellationRequested)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, _speed * _speedMultiplier * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    }
}
