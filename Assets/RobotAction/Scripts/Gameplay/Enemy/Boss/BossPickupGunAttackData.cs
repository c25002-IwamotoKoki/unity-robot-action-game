using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    [CreateAssetMenu(fileName = "BossPickupGunAttackData", menuName = "Scriptable Objects/BossPickupGunAttackData")]
    public class BossPickupGunAttackData : ScriptableObject
    {
        [Header("銃が動的に変わるためインスペクター上であらかじめ攻撃範囲を設定")]
        [SerializeField] private float _attackRange;
        [SerializeField] private float _pickupRange;
        [Header("GunEquipPointsの数より少なく設定してください")]
        [SerializeField, Min(1)] private int _maxPickupCount = 1;
        [SerializeField, Min(1)] private int _maxSearchCount = 1;
        [SerializeField] private LayerMask _searchLayer;
        [SerializeField] private LayerMask _attackTargetLayer;

        public float AttackRange => _attackRange;
        public float PickupRange => _pickupRange;
        public int MaxPickupCount => _maxPickupCount;
        public int MaxSearchCount => _maxSearchCount;
        public LayerMask SearchLayer => _searchLayer;
        public LayerMask AttackTargetLayer => _attackTargetLayer;
    }
}
