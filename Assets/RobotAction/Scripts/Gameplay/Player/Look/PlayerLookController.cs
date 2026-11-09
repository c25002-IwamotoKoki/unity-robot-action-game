using UnityEngine;

namespace RobotAction.Gameplay.Player.Look
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

            Vector3 horizontalDirection = new Vector3(direction.x,0,direction.z);

            if(horizontalDirection.sqrMagnitude > 0.00001f)
            {
                Quaternion targetYaw = Quaternion.LookRotation(horizontalDirection);
                _owner.rotation = Quaternion.Slerp(_owner.rotation,targetYaw,_data.TargetLookSpeed * deltaTime);
            }
        }

        public void UpdateYawRotation(float rawInputX,float deltaTime,bool isMouseLook)
        {
            if (deltaTime <= 0 || Mathf.Abs(rawInputX) <= _data.LookDeadZone) return;

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
