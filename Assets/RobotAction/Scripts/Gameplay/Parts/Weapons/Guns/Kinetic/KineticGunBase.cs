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

        protected override void OnAwake()
        {
            if (!TryGetComponent(out _magazine))
            {
                Debug.LogError("KineticMagazine‚ğæ“¾‚Å‚«‚Ü‚¹‚ñ‚Å‚µ‚½B");
            }
        }

        private void Start()
        {
            _magazine.Setup(_data);
        }

        public override void SetTarget(Vector3 position)
        {
            _muzzleTransform.LookAt(position);
        }

        public override void Fire()
        {
            if(_isFired)
            {
                return;
            }

            var bulletContext = new BulletContext(owner:transform.root,
                                                  _data.BulletLifeTime,
                                                  _data.BulletSpeed,
                                                  _data.BulletBaseAttackPower);

            _magazine.SpawnBullet(bulletContext,
                                 _muzzleTransform.position,
                                 _muzzleTransform.rotation);

            _isFired = true;
        }

        public override void Equip(Transform owner)
        {
            AttackRange = CalculateAttackRange();
            base.Equip(owner);
        }

        public float CalculateAttackRange()
        {
            //…•½“ŠË‚Ì”ò‹——£ = ‰‘¬ * sqrt(2 * ‚‚³ / d—Í‰Á‘¬“x)
            float gravity = Mathf.Abs(Physics.gravity.y);
            float bulletFlightTime = Mathf.Sqrt(2 * _muzzleTransform.position.y / gravity) * _data.BulletLifeTime;
            return _data.BulletSpeed * bulletFlightTime;
        }
    }
}
