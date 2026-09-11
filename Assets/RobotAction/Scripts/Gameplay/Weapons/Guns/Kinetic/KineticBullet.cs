using RobotAction.Gameplay.Interfaces;
using UnityEngine;
using UnityEngine.Pool;

namespace RobotAction.Gameplay.Weapons.Guns
{
    [RequireComponent(typeof(Rigidbody))]
    public class KineticBullet : BulletBase
    {
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
                OwnerPool.Release(this);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.GetDamage(_baseAttackPower);
                _lifeTime = 0;
                OwnerPool.Release(this);
            }
        }

        public override void Shoot(in BulletContext context)
        {
            _baseAttackPower = context.BaseAttackPower;
            _lifeTime = context.LifeTime;
            _rigidbody.AddForce(transform.forward * context.Speed, ForceMode.Impulse);
        }

    }
}
