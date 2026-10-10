using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    [CreateAssetMenu(fileName = "PlayerLookData", menuName = "Player/LookData")]
    public class PlayerLookData : ScriptableObject
    {
        [SerializeField] private float _mouseLookSensitivity;
        [SerializeField] private float _gamePadLookSensitivity;
        [SerializeField] private float _targetLookSpeed;
        [SerializeField] private float _lookDeadZone = 0.2f;

        public float MouseLookSensitivity => _mouseLookSensitivity;
        public float GamePadLookSensitivity => _gamePadLookSensitivity;
        public float TargetLookSpeed => _targetLookSpeed;
        public float LookDeadZone => _lookDeadZone;
    }
}
