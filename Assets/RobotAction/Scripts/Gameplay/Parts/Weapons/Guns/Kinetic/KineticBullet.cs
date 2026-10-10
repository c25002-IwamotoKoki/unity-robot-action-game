using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Parts.Weapons.Guns
{
    [RequireComponent(typeof(Rigidbody))]
    public class KineticBullet : BulletBase
    {
        private Transform _owner;
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
            //先に_ownerがnullの時にはじいているのは自傷防止チェックの際の例外防止のため
            if (_owner == null || _isReleased || other.transform == _owner) return;
      
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
            _owner = context.Owner;
            _baseAttackPower = context.BaseAttackPower;
            _lifeTime = context.LifeTime;
            _rigidbody.AddForce(transform.forward * context.Speed, ForceMode.Impulse);
        }

    }
}
