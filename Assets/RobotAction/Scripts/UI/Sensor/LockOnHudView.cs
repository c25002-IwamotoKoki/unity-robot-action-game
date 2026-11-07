using RobotAction.Core.Sensor;
using UnityEngine;

namespace RobotAction.UI.Sensor
{
    public sealed class LockOnHudView : MonoBehaviour
    {
        private readonly Vector3 OffScreenPosition = new Vector3(-9999f,-9999f,-9999f);

        [SerializeField] private RectTransform _lockOnReticle;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _reticleSmoothDamp;

        private IAutoLockSensorStatus _autoLockSensorStatus;
        private Vector3 _dampVelocity;

        private void LateUpdate()
        {
            if (_autoLockSensorStatus.HasLockOnTarget)
            {
                Vector3 targetPosition = _camera.WorldToScreenPoint(_autoLockSensorStatus.CurrentLockOnPosition);

                if (targetPosition.z < 0) return;

                _lockOnReticle.position = Vector3.SmoothDamp(
                    _lockOnReticle.position,
                    targetPosition,
                    ref _dampVelocity,
                    _reticleSmoothDamp
                );
            }
            else
            {
                _lockOnReticle.position = OffScreenPosition;
                _dampVelocity = Vector3.zero;
            }
        }

        public void Initialized(IAutoLockSensorStatus autoLockSensorStatus)
        {
            _autoLockSensorStatus = autoLockSensorStatus;
        }
    }
}
