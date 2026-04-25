using System.Collections.Generic;
using System.Threading;

using _Project.Scripts.Gameplay.Units;

using Cysharp.Threading.Tasks;

using UnityEngine;

using Zenject;

namespace _Project.Scripts.Gameplay.Dragon
{
    public class Fireball : MonoBehaviour
    {
        [SerializeField] private float _speed = 8f;
        [SerializeField] private float _explosionRadius = 1.5f;

        [Inject] private UnitService _units;

        public void Launch(Vector3 targetPoint)
        {
            LaunchAsync(targetPoint, this.GetCancellationTokenOnDestroy()).Forget();
        }

        private async UniTaskVoid LaunchAsync(Vector3 targetPoint, CancellationToken token)
        {
            while (transform.position != targetPoint && !token.IsCancellationRequested)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPoint, _speed * Time.deltaTime);
                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (token.IsCancellationRequested)
                return;

            Explode(targetPoint);
            Destroy(gameObject);
        }

        private void Explode(Vector3 center)
        {
            float r2 = _explosionRadius * _explosionRadius;
            List<Unit> snapshot = new List<Unit>(_units.Units);
            foreach (Unit unit in snapshot)
            {
                if (unit == null)
                    continue;

                if ((unit.transform.position - center).sqrMagnitude <= r2)
                    unit.Die();
            }
        }
    }
}
