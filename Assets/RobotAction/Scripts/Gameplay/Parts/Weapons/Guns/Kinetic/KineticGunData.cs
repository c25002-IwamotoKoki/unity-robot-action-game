using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [CreateAssetMenu(fileName = "KineticGunData", menuName = "Scriptable Objects/KineticGunData")]
    public class KineticGunData : GunData
    {
        [SerializeField] private float _fireRate;
        [SerializeField] private float _bulletLifeTime;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletBaseAttackPower;
        [SerializeField, Min(1)] private int _maxHitCount = 1;

        public float FireRate => _fireRate;
        public float BulletLifeTime => _bulletLifeTime;
        public float BulletSpeed => _bulletSpeed;
        public float BulletBaseAttackPower => _bulletBaseAttackPower;
        public int MaxHitCount => _maxHitCount;
    }
}
