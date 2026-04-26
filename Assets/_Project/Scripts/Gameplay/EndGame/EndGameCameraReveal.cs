using System.Threading;

using UnityEngine;

using Cinemachine;
using Cysharp.Threading.Tasks;

using _Project.Scripts.Gameplay.Stages;

namespace _Project.Scripts.Gameplay.EndGame
{
    public class EndGameCameraReveal : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;

        private Vector2 _fixedBottomLeftCorner;
        private bool _isConfigured;

        private void Awake()
        {
            CacheInitialCameraState();
        }

        private void CacheInitialCameraState()
        {
            if (!TryGetCameraState(out Vector3 position, out float orthographicSize, out float aspect))
            {
                Debug.LogWarning($"{nameof(EndGameCameraReveal)} requires an orthographic Camera or CinemachineVirtualCamera reference.", this);
                return;
            }

            _fixedBottomLeftCorner = new Vector2(position.x - orthographicSize * aspect, position.y - orthographicSize);
            _isConfigured = true;
        }

        public void ApplyImmediate(StageCameraSettings settings)
        {
            if (settings == null)
                return;

            if (!_isConfigured)
                CacheInitialCameraState();

            if (!_isConfigured)
                return;

            float orthographicSize = Mathf.Max(0f, settings.TargetOrthographicSize);
            if (orthographicSize <= 0f)
                return;

            ApplyRevealState(orthographicSize);
        }

        public async UniTask PlayReveal(StageCameraSettings settings, CancellationToken cancellationToken)
        {
            if (settings == null)
                return;

            if (!_isConfigured)
                CacheInitialCameraState();

            if (!_isConfigured)
                return;

            float startSize = GetOrthographicSize();
            float targetSize = Mathf.Max(startSize, settings.TargetOrthographicSize);
            float duration = Mathf.Max(0f, settings.Duration);
            AnimationCurve easing = settings.Easing;

            if (Mathf.Approximately(startSize, targetSize) || duration <= 0f)
            {
                ApplyRevealState(targetSize);
                return;
            }

            float elapsed = 0f;

            while (elapsed < duration && !cancellationToken.IsCancellationRequested)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / duration);
                float easedProgress = easing != null ? easing.Evaluate(progress) : progress;
                float currentSize = Mathf.Lerp(startSize, targetSize, easedProgress);

                ApplyRevealState(currentSize);
                await UniTask.Yield(PlayerLoopTiming.Update, cancellationToken);
            }

            if (!cancellationToken.IsCancellationRequested)
                ApplyRevealState(targetSize);
        }

        private void ApplyRevealState(float orthographicSize)
        {
            float aspect = GetAspect();
            Vector3 currentPosition = GetPosition();

            Vector3 targetPosition = new(
                _fixedBottomLeftCorner.x + orthographicSize * aspect,
                _fixedBottomLeftCorner.y + orthographicSize,
                currentPosition.z);

            if (_virtualCamera != null)
            {
                LensSettings lens = _virtualCamera.m_Lens;
                lens.OrthographicSize = orthographicSize;
                _virtualCamera.m_Lens = lens;
                _virtualCamera.transform.position = targetPosition;
                return;
            }

            if (_camera == null)
                return;

            _camera.orthographicSize = orthographicSize;
            _camera.transform.position = targetPosition;
        }

        private bool TryGetCameraState(out Vector3 position, out float orthographicSize, out float aspect)
        {
            position = GetPosition();
            orthographicSize = GetOrthographicSize();
            aspect = GetAspect();

            return orthographicSize > 0f && aspect > 0f;
        }

        private Vector3 GetPosition()
        {
            if (_virtualCamera != null)
                return _virtualCamera.transform.position;

            if (_camera != null)
                return _camera.transform.position;

            return transform.position;
        }

        private float GetOrthographicSize()
        {
            if (_virtualCamera != null)
                return _virtualCamera.m_Lens.OrthographicSize;

            if (_camera != null && _camera.orthographic)
                return _camera.orthographicSize;

            return 0f;
        }

        private float GetAspect()
        {
            if (_camera != null)
                return _camera.aspect;

            if (Camera.main != null)
                return Camera.main.aspect;

            return 0f;
        }
    }
}
