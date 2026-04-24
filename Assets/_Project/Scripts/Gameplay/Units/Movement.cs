using System.Threading;

using Cysharp.Threading.Tasks;

using UnityEngine;

namespace _Project.Scripts.Gameplay.Units
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Transform _pointA;
        [SerializeField] private Transform _pointB;
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _delayAtB = 1f;
        [SerializeField] private float _delayAtA = 1f;

        private CancellationTokenSource _cts;

        private void OnEnable()
        {
            _cts = new CancellationTokenSource();
            PatrolLoop(_cts.Token).Forget();
        }

        private void OnDisable()
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        private async UniTaskVoid PatrolLoop(CancellationToken token)
        {
            bool goingToB = true;

            while (!token.IsCancellationRequested)
            {
                Transform target = goingToB ? _pointB : _pointA;
                float delay = goingToB ? _delayAtB : _delayAtA;

                await MoveToPoint(target, token);
                await UniTask.Delay(System.TimeSpan.FromSeconds(delay), cancellationToken: token);

                goingToB = !goingToB;
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