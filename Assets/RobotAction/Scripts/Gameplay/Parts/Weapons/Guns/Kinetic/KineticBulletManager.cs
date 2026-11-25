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
            public MeshRenderer MeshRenderer;
            public float CurrnetLifeTime;
        }

        private readonly KineticGunData _gunData;
        private readonly StackPool<MeshRenderer> _pool;
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

            if(!prefab.TryGetComponent(out MeshRenderer meshRenderer))
            {
                throw new System.ArgumentException($"{prefab.name}にmeshRendererがありません");
            }

            _pool = new StackPool<MeshRenderer>(
                meshRenderer,
                maxOnFieldBullet,
                autoSetActive:false,
                (item,_) => item.enabled = false
            );


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
                    //二重返却防止
                    continue;
                }

                if (TryBulletHitProcess(activeBullet.Transform,deltaTime))
                {
                    ReleaseBulletByIndex(i);
                }
            }
        }

        public void Fire(float deltaTime)
        {
            if (!_pool.CanRent) return;

            var meshRenderer = _pool.Rent();

            if (meshRenderer == null) return;

            Transform bullet = meshRenderer.transform;

            bullet.SetPositionAndRotation(_muzzlePoint.position, _muzzlePoint.rotation);

            if(TryBulletHitProcess(bullet,deltaTime))
            {
                _pool.Release(meshRenderer);
                return;
            }

            meshRenderer.enabled = true;

            var activeBullet = new ActiveBullet()
            {
                Transform = bullet,
                MeshRenderer = meshRenderer,
                CurrnetLifeTime = 0
            };

            _activeBulletList.Add(activeBullet);
        }

        private void ReleaseBulletByIndex(int index)
        {
            MeshRenderer meshRenderer = _activeBulletList[index].MeshRenderer;
            meshRenderer.enabled = false;
            _pool.Release(meshRenderer);

            int lastIndex = _activeBulletList.Count - 1;

            _activeBulletList[index] = _activeBulletList[lastIndex];

            _activeBulletList.RemoveAt(lastIndex);
        }

        private bool TryBulletHitProcess(Transform bullet,float deltaTime)
        {
            //命中判定に使う弾の大きさはモデルの差異による破綻を防ぐため判定用の弾の基準半径は1fに固定します。
            float checkRadius = 1f + _gunData.BulletSpeed * deltaTime;

            LayerMask hitMask = _attackTargetLayer | _gunData.ObstacleLayer;

            int hitCount = Physics.OverlapSphereNonAlloc(
                bullet.position,
                checkRadius,
                _hitColliders,
                hitMask
            );

            if (hitCount == 0) return false;

            SortHitCollidersByProximity(bullet,hitCount);

            //現段階で複数にヒットする予定はないため最も近い敵に対してダメージチェックを行う
            if (_hitColliders[0].TryGetComponent(out IDamageable damageable))
            {
                damageable.GetDamage(_gunData.BulletBaseAttackPower);
            }

            return true;
        }

        public void SetAttackTargetLayer(LayerMask layer)
        {
            _attackTargetLayer = layer;
        }

        private void SortHitCollidersByProximity(Transform bullet,int hitCount)
        {
            for (int i = 0; i < hitCount - 1; i++)
            {
                for (int j = i; j < hitCount; j++)
                {
                    float distanceSqrI = (_hitColliders[i].transform.position - bullet.position).sqrMagnitude;
                    float distanceSqrJ = (_hitColliders[j].transform.position - bullet.position).sqrMagnitude;

                    if(distanceSqrJ < distanceSqrI)
                    {
                        var temp = _hitColliders[i];
                        _hitColliders[i] = _hitColliders[j];
                        _hitColliders[j] = temp;
                    }
                }
            }
        }
    }
}
