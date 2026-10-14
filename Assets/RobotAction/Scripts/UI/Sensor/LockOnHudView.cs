using RobotAction.Core.Sensor;
using UnityEngine;

namespace RobotAction.UI.Sensor
{
    public sealed class LockOnHudView : MonoBehaviour
    {
        private readonly Vector3 OffScreenPosition = new Vector3(-9999f,-9999f,-9999f);

        [SerializeField] private RectTransform _lockOnReticle;
        [SerializeField] private Camera _camera;

        private IAutoLockSensorStatus _autoLockSensorStatus;

        private void Update()
        {
            if(_autoLockSensorStatus.HasLockOnTarget)
            {
                _lockOnReticle.position = _camera.WorldToScreenPoint(_autoLockSensorStatus.CurrentLockOnPosition);
            }
            else
            {
                _lockOnReticle.position = OffScreenPosition;
            }
        }

        public void Initialized(IAutoLockSensorStatus autoLockSensorStatus)
        {
            _autoLockSensorStatus = autoLockSensorStatus;
        }
    }
}
