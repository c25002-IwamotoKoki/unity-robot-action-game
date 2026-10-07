using UnityEngine;

namespace RobotAction.Gameplay.Sensors
{
    [CreateAssetMenu(fileName = "AutoLockSensorData", menuName = "Scriptable Objects/AutoLockSensorData")]
    public class AutoLockSensorData : ScriptableObject
    {
        [SerializeField, Min(1)] private int _maxSearch;
        [SerializeField] private float _searchRange;
        [SerializeField] private float _searchOffset;
        [SerializeField] private float _searchCoolTime;
        [SerializeField] private LayerMask _searchLayer;

        public int MaxSearch => _maxSearch;
        public float SearchRange => _searchRange;
        public float SearchOffset => _searchOffset;
        public float SearchCoolTime => _searchCoolTime;
        public LayerMask SearchLayer => _searchLayer;
    }
}
