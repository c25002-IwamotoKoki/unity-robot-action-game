using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Missiles
{
    [CreateAssetMenu(fileName = "MissileData", menuName = "Scriptable Objects/MissileData")]
    public class MissileLauncherData : ScriptableObject
    {
        [SerializeField] private MissileBullet _bulletPrefab;
        [SerializeField] private float _attackPower;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _bulletFlightSpeed;
        [SerializeField] private float _bulletOnHomingSpeed;
        [SerializeField, Range(0f, 1f)] private float _bulletHomingRate = 0.5f;
        [SerializeField] private float _bulletToHomingDelay;
        [Header("HormingDelayなども考慮してLifeTimeを指定する必要あり")]
        [SerializeField] private float _bulletLifeTime;
        [SerializeField] private float _blastRadius;
        [SerializeField,Min(1)] private int _maxBlastHitCount = 1;
        [SerializeField] private int _maxAmmo;
        [Header("ミサイルは障害物に当たっても爆発するため保持")]
        [SerializeField] private LayerMask _obstacleLayer;
        [SerializeField] private AudioClip _shotSE;

        public MissileBullet BulletPrefab => _bulletPrefab;
        public float AttackPower => _attackPower;
        public float FireRate => _fireRate;
        public float BulletFlightSpeed => _bulletFlightSpeed;
        public float BulletOnHomingSpeed => _bulletOnHomingSpeed;
        public float BulletHomingRate => _bulletHomingRate;
        public float BulletToHomingDelay => _bulletToHomingDelay;
        public float BulletLifeTime => _bulletLifeTime;
        public float BlastRadius => _blastRadius;
        public int MaxBlastHitCount => _maxBlastHitCount;
        public int MaxAmmo => _maxAmmo;
        public LayerMask ObstacleLayer => _obstacleLayer;
        public string ShotSEId => _shotSE.name;
    }
}
