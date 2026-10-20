using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.ObjectPool;
using System.Collections.Generic;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    public class KineticBulletManager
    {
        private struct ActiveBullet
        {
            public Transform Transform;
            public float CurrnetLifeTime;
        }

        private readonly KineticGunData _gunData;
        private readonly StackPool<Transform> _pool;
        private readonly Transform _muzzlePoint;
        private readonly List<ActiveBullet> _activeBulletList;
        private readonly Collider[] _hitColliders;

        //今回のプロジェクトでは武器の持ち主が変わるため攻撃対象レイヤーはreadonlyで保持しません。
        private LayerMask _attackTargetLayer;

        public KineticBulletManager(
            KineticGunData gunData,
            GameObject prefab,
            Transform muzzlePoint,
            LayerMask layer
        )
        {
            _gunData = gunData;
            _muzzlePoint = muzzlePoint;
            _attackTargetLayer = layer;
            //Poolの初期化に使うためFireRate/BulletLifeTimeでフィールド上に存在するための数を算出
            int maxOnFieldBullet = Mathf.FloorToInt(_gunData.BulletLifeTime / _gunData.FireRate);
            _pool = new StackPool<Transform>(prefab.transform, maxOnFieldBullet);
            _activeBulletList = new List<ActiveBullet>(maxOnFieldBullet);
            _hitColliders = new Collider[_gunData.MaxHitCount];
        }

        public void Tick(float deltaTime)
        {
            //安全かつ軽量にBulletをPoolに返却するため逆順ループ
            for (int i = _activeBulletList.Count - 1; i >= 0; i--)
            {
                var activeBullet = _activeBulletList[i];

                activeBullet.Transform.position += _gunData.BulletSpeed * deltaTime * activeBullet.Transform.forward;
                activeBullet.CurrnetLifeTime += deltaTime;

                _activeBulletList[i] = activeBullet;

                if (_activeBulletList[i].CurrnetLifeTime >= _gunData.BulletLifeTime)
                {
                    ReleaseBulletByIndex(i);
                }

                if (BulletHitCheck(activeBullet.Transform))
                {
                    ReleaseBulletByIndex(i);
                }
            }
        }

        public void Fire()
        {
            if (!_pool.CanRent) return;

            Transform bullet = _pool.Rent();

            if (bullet == null) return;

            bullet.SetPositionAndRotation(_muzzlePoint.position, _muzzlePoint.rotation);

            var activeBullet = new ActiveBullet()
            {
                Transform = bullet,
                CurrnetLifeTime = 0
            };

            _activeBulletList.Add(activeBullet);
        }

        private void ReleaseBulletByIndex(int index)
        {
            _pool.Release(_activeBulletList[index].Transform);

            int lastIndex = _activeBulletList.Count - 1;

            _activeBulletList[index] = _activeBulletList[lastIndex];

            _activeBulletList.RemoveAt(lastIndex);
        }

        private bool BulletHitCheck(Transform bullet)
        {
            //3で割って大きさを平均化
            float bulletRadius = (bullet.localScale.x + bullet.localScale.y + bullet.localScale.z) / 3;

            int hitCount = Physics.OverlapSphereNonAlloc(
                bullet.position,
                bulletRadius,
                _hitColliders,
                _attackTargetLayer
            );

            for (int i = 0; i < hitCount; i++)
            {
                if (_hitColliders[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.GetDamage(_gunData.BulletBaseAttackPower);
                    return true;
                }
            }

            return false;
        }

        public void SetAttackTargetLayer(LayerMask layer)
        {
            _attackTargetLayer = layer;
        }
    }
}
