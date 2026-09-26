using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    [CreateAssetMenu(fileName = "EnemySarchData", menuName = "Scriptable Objects/EnemySarchData")]
    public class EnemySearchData : ScriptableObject
    {
        [SerializeField]private float _searchRange;
        [SerializeField,Min(1)]private int _maxSearchCount;
        [SerializeField]private Vector3 _searchPositionOffset;
        [SerializeField]private LayerMask _searchLayer;

        public float SearchRange => _searchRange;
        public int MaxSearchCount => _maxSearchCount;
        public Vector3 SearchPositionOffset => _searchPositionOffset;
        public LayerMask SearchLayer => _searchLayer;

    }
}
