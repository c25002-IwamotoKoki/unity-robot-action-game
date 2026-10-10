using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    public class PlayerLookController
    {
        private readonly Transform _owner;
        private readonly PlayerLookData _data;

        public PlayerLookController(PlayerLookData data, Transform owner)
        {
            _data = data;
            _owner = owner;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void TrackingTarget(Transform target, float deltaTime)
        {
            RotateTowardsToTarget(target,deltaTime);
        }

        private void RotateTowardsToTarget(Transform target, float deltaTime)
        {
            Vector3 direction = target.position - _owner.position;

            //‹——£‚ª‚Ù‚Ú0‚È‚ç‚Í‚¶‚­
            if (direction.sqrMagnitude < 0.00001f) return;

            Quaternion rotation = Quaternion.LookRotation(direction);

            _owner.rotation = Quaternion.Slerp(
                _owner.rotation,
                rotation,
                _data.TargetLookSpeed * deltaTime
            );
        }

        public void UpdateYawRotation(float rawInputX,float deltaTime,bool isMouseLook)
        {
            if (Mathf.Abs(rawInputX) <= _data.LookDeadZone) return;

            if (isMouseLook)
            {
                Vector3 angle = _owner.localEulerAngles;
                angle.y += rawInputX * _data.MouseLookSensitivity;

                _owner.localEulerAngles = angle;
            }
            else
            {
                Vector3 angle = _owner.localEulerAngles;
                angle.y += rawInputX * _data.GamePadLookSensitivity * deltaTime;

                _owner.localEulerAngles = angle;
            }
        }
    }
}
