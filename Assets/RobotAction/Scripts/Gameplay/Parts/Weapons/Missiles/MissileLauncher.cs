using RobotAction.Gameplay.ObjectPool;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Missiles
{
    public class MissileLauncher : WeaponPartBase
    {
        //ミサイルは曲がり、有効射程距離が理論値より下がるためAttackRangeの指定にこの変数を使う
        private const float EffectiveAttackRangeRatio = 0.8f;

        [SerializeField] private MissileLauncherData _data;
        [SerializeField] private Transform _muzzlePoint;

        private StackPool<MissileBullet> _bulletPool;
        private float _fireCoolTimer;
        private bool _isFired;

        private bool CanFire => RemainingUseCount > 0;
        public override int MaxUseCount => _data.MaxAmmo;

        protected override void OnAwake()
        {
            RemainingUseCount = _data.MaxAmmo;

            float straightTime = _data.BulletToHomingDelay;
            float homingTime = _data.BulletLifeTime - _data.BulletToHomingDelay;
            float straightMoveDistance = straightTime * _data.BulletFlightSpeed;
            float homingMoveDistance = homingTime * _data.BulletOnHomingSpeed;
            //ホーミング前後の移動距離を足してAttackRangeを算出
            //カーブを考慮して理論値より小さめにAttackRangeを設定
            AttackRange = (straightMoveDistance + homingMoveDistance) * EffectiveAttackRangeRatio;

            int maxOnFieldBulletCount = Mathf.CeilToInt(_data.BulletLifeTime / _data.FireRate);

            _bulletPool = new StackPool<MissileBullet>(
                _data.BulletPrefab,
                maxOnFieldBulletCount,
                (item, pool) => item.Initialize(pool,_data)
            );

            base.OnAwake();
        }

        private void Update()
        {
            if(_isFired)
            {
                _fireCoolTimer += Time.deltaTime;

                if(_fireCoolTimer >= _data.FireRate)
                {
                    _isFired = false;
                    _fireCoolTimer = 0;
                }
            }
        }

        public override void Attack()
        {
            if (_isFired || !CanFire || !_bulletPool.CanRent)
            {
                return;
            }

            var bullet = _bulletPool.Rent();
            bullet.transform.SetPositionAndRotation(_muzzlePoint.position,_muzzlePoint.rotation);

            bullet.SetAttackTargetLayer(AttackTargetLayer);
            bullet.Fire(CurrentTarget);
            AudioService?.PlaySE(_data.ShotSEId);

            RemainingUseCount--;
            InvokeOnWeaponStatusChangedEvent();
            _isFired = true;
        }
    }
}
