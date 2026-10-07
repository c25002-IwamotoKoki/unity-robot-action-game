using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [RequireComponent(typeof(KineticMagazine))]
    public abstract class KineticGunBase : GunBase
    {
        [SerializeField] private KineticGunData _data;
        [SerializeField] private Transform _muzzleTransform;

        public override int MaxUseCount =>  _data.MaxAmmo;
        protected override float FireRate => _data.FireRate;

        private KineticMagazine _magazine;

        protected override void OnAwake()
        {
            if (!TryGetComponent(out _magazine))
            {
                Debug.LogError("KineticMagazineÇéÊìæÇ≈Ç´Ç‹ÇπÇÒÇ≈ÇµÇΩÅB");
            }

            RemainingUseCount = _data.MaxAmmo;
            InvokeOnWeaponStatusChangedEvent();
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
            if (_isFired || !CanFire) return;

            var bulletContext = new BulletContext(owner:transform.root,
                                                  _data.BulletLifeTime,
                                                  _data.BulletSpeed,
                                                  _data.BulletBaseAttackPower);

            _magazine.SpawnBullet(bulletContext,
                                 _muzzleTransform.position,
                                 _muzzleTransform.rotation);

            _isFired = true;
            RemainingUseCount--;
            InvokeOnWeaponStatusChangedEvent();
        }

        public override void Equip(Transform owner)
        {
            AttackRange = CalculateAttackRange();
            base.Equip(owner);
        }

        public float CalculateAttackRange()
        {
            //êÖïΩìäéÀÇÃîÚãóó£ = èâë¨ * sqrt(2 * çÇÇ≥ / èdóÕâ¡ë¨ìx)
            float gravity = Mathf.Abs(Physics.gravity.y);
            float bulletFlightTime = Mathf.Sqrt(2 * _muzzleTransform.position.y / gravity) * _data.BulletLifeTime;
            return _data.BulletSpeed * bulletFlightTime;
        }
    }
}
