using UnityEngine;

namespace RobotAction.Gameplay.Scriptables
{
    [CreateAssetMenu(fileName = "KineticGunData", menuName = "Scriptable Objects/KineticGunData")]
    public class KineticGunData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private float _fireRate;
        [SerializeField] private float _bulletLifeTime;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private float _bulletBaseAttackPower;

        public string Name => _name;
        public float FireRate => _fireRate;
        public float BulletLifeTime => _bulletLifeTime;
        public float BulletSpeed => _bulletSpeed;
        public float BulletBaseAttackPower => _bulletBaseAttackPower;

    }
}
