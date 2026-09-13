using UnityEngine;

namespace RobotAction.Gameplay.Sensors
{
    [CreateAssetMenu(fileName = "AutoLockSensorData", menuName = "Scriptable Objects/AutoLockSensorData")]
    public class AutoLockSensorData : ScriptableObject
    {
        [SerializeField, Min(1)] private int _maxSearch;
        [SerializeField] private float _searchRange;
        [SerializeField] private Vector3 _searchPositionOffset;
        [SerializeField] private float _searchCoolTime;

        public int MaxSearch => _maxSearch;
        public float SearchRange => _searchRange;
        public Vector3 SearchPositionOffset => _searchPositionOffset;
        public float SearchCoolTime => _searchCoolTime;
    }
}
