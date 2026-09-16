using RobotAction.Gameplay.Scriptables;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [RequireComponent(typeof(KineticMagazine))]
    public abstract class KineticGunBase : GunBase
    {
        [SerializeField] private KineticGunData _data;
        [SerializeField] private Transform _muzzleTransform;
        private KineticMagazine _magazine;

        protected override float FireRate => _data.FireRate;

        protected virtual void Awake()
        {
            if(!TryGetComponent(out _magazine))
            {
                Debug.LogError("KineticMagazineÇéÊìæÇ≈Ç´Ç‹ÇπÇÒÇ≈ÇµÇΩÅB");
            }

            if(_magazine == null)
            {
                Debug.LogError("MagazineÇ™nullÇ≈Ç∑");
            }       
        }

        private void Start()
        {
            _magazine.Setup(_data);
        }

        public override void SetShootTarget(Vector3 position)
        {
            _muzzleTransform.LookAt(position);
        }

        public override void Shoot()
        {
            if(!_isShot)
            {
                return;
            }

            var bulletContext = new BulletContext(_data.BulletLifeTime,
                                                  _data.BulletSpeed,
                                                  _data.BulletBaseAttackPower);

            _magazine.SpawnBullet(bulletContext,
                                 _muzzleTransform.position,
                                 _muzzleTransform.rotation);

            _isShot = false;
        }
    }
}
