using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Gameplay.Units
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _speed = 3f;
        [FormerlySerializedAs("_delayAtA")]
        [SerializeField] private float _delayAtGuild = 1f;
        [FormerlySerializedAs("_delayAtB")]
        [SerializeField] private float _delayAtGather = 1f;

        [FormerlySerializedAs("_pointA")]
        [SerializeField]
        private Transform _guildPoint;
        [FormerlySerializedAs("_pointB")]
        [SerializeField]
        private Transform _gatherGoldPoint;

        private CancellationTokenSource _cts;

        public void Initialize(Transform guildPoint, Transform gatherGoldPoint)
        {
            if (guildPoint == null || gatherGoldPoint == null)
            {
                Debug.LogError($"{nameof(Movement)} on {name} cannot initialize because route points are missing.", this);
                return;
            }

            _guildPoint = guildPoint;
            _gatherGoldPoint = gatherGoldPoint;

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
                Transform target = goingToGather ? _gatherGoldPoint : _guildPoint;
                float delay = goingToGather ? _delayAtGather : _delayAtGuild;

                await MoveToPoint(target, token);
                await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);

                goingToGather = !goingToGather;
            }
        }

        private async UniTask MoveToPoint(Transform target, CancellationToken token)
        {
            while (transform.position != target.position && !token.IsCancellationRequested)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }
    }
}
