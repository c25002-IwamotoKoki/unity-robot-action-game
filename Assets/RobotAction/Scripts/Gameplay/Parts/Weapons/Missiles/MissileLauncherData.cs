using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Missiles
{
    [CreateAssetMenu(fileName = "MissileData", menuName = "Scriptable Objects/MissileData")]
    public class MissileLauncherData : ScriptableObject
    {
        [SerializeField] private MissileBullet _bulletPrefab;
        [SerializeField] private float _attackPower;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _flightSpeed;
        [SerializeField] private float _onHomingSpeed;
        [SerializeField, Range(0f, 1f)] private float _homingRate = 0.5f;
        [SerializeField] private float _toHomingDelay;
        [Header("HormingDelayなども考慮してLifeTimeを指定する必要あり")]
        [SerializeField] private float _bulletLifeTime;
        [SerializeField] private float _blastRadius;
        [SerializeField,Min(1)] private int _maxBlastHitCount = 1;
        [SerializeField] private int _maxAmmo;
        [Header("ミサイルは障害物に当たっても爆発するため保持")]
        [SerializeField] private LayerMask _obstacleLayer;

        public MissileBullet BulletPrefab => _bulletPrefab;
        public float AttackPower => _attackPower;
        public float FireRate => _fireRate;
        public float FlightSpeed => _flightSpeed;
        public float OnHomingSpeed => _onHomingSpeed;
        public float HomingRate => _homingRate;
        public float ToHomingDelay => _toHomingDelay;
        public float BulletLifeTime => _bulletLifeTime;
        public float BlastRadius => _blastRadius;
        public int MaxBlastHitCount => _maxBlastHitCount;
        public int MaxAmmo => _maxAmmo;
        public LayerMask ObstacleLayer => _obstacleLayer;
    }
}
