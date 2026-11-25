using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public abstract class KineticGunBase : GunBase
    {
        [SerializeField] private KineticGunData _data;
        [SerializeField] private Transform _muzzleTransform;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private ParticleSystem _shotParticle;

        public override int MaxUseCount =>  _data.MaxAmmo;
        protected override float FireRate => _data.FireRate;

        private KineticBulletManager _bulletManager;

        protected override void OnAwake()
        {
            _bulletManager = new KineticBulletManager(_data,_bulletPrefab,_muzzleTransform,AttackTargetLayer);

            RemainingUseCount = _data.MaxAmmo;
            InvokeOnWeaponStatusChangedEvent();

            if(_shotParticle == null)
            {
                Debug.LogWarning("[KineticGunBase]: ShotParticleÇ™ñ¢ê›íËÇ≈Ç∑");
                return;
            }
        }

        protected override void Update()
        {
            base.Update();
            _bulletManager.Tick(Time.deltaTime);
        }

        public override void SetTarget(Vector3 position)
        {
            _muzzleTransform.LookAt(position);
        }

        public override void ResetTarget()
        {
            _muzzleTransform.rotation = transform.rotation;
        }

        public override void Fire()
        {
            if (_isFired || !CanFire) return;

            _bulletManager.Fire(Time.deltaTime);

            _isFired = true;
            RemainingUseCount--;
            InvokeOnWeaponStatusChangedEvent();
            PlayShotSE(_data.ShotSEId);

            if (_shotParticle == null) return;

            _shotParticle.Clear();
            _shotParticle.Stop();
            _shotParticle.Play();
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

        public override void SetAttackTargetLayer(LayerMask targetLayer)
        {
            base.SetAttackTargetLayer(targetLayer);
            _bulletManager.SetAttackTargetLayer(targetLayer);
        }
    }
}
