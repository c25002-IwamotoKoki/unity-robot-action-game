using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [RequireComponent(typeof(Rigidbody))]
    public class KineticBullet : BulletBase
    {
        private LayerMask _attackTargetLayer;
        private Rigidbody _rigidbody;
        private float _baseAttackPower;
        private float _lifeTime;
        private float _lifeTimer;

        private void Awake()
        {
            TryGetComponent(out _rigidbody);
        }

        private void Update()
        {
            _lifeTimer += Time.deltaTime;

            if (_lifeTimer >= _lifeTime)
            {
                _lifeTimer = 0;

                if(!_isReleased)
                {
                    OwnerPool.Release(this);
                    _isReleased = true;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            //other.gameObject.LayerはIndex番号でありLayerMaskとの比較ができないため
            //シフトでビットマスクに変換した後ビット積(&)によって_attackTargetLayerが含まれているかどうかを判定しています
            LayerMask otherLayerMask = 1 << other.gameObject.layer;
            if (_isReleased || ( otherLayerMask & _attackTargetLayer) == 0) return;

            if (other.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.GetDamage(_baseAttackPower);          
            }

            //何らかのオブジェクトに着弾したためPoolに返却
            OwnerPool.Release(this);
            _isReleased = true;
        }

        public override void OnGet()
        {
            _rigidbody.linearVelocity = Vector3.zero;
            base.OnGet();
        }

        public override void Fire(in BulletContext context)
        {
            _baseAttackPower = context.BaseAttackPower;
            _lifeTime = context.LifeTime;
            _attackTargetLayer = context.AttackTargetLayer;
            _rigidbody.AddForce(transform.forward * context.Speed, ForceMode.Impulse);
        }

    }
}
