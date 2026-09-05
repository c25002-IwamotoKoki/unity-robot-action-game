using RobotAction.Gameplay.Interfaces;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyRushAttack :  MonoBehaviour,IEnemyAttack
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _rushDuration;
        [SerializeField] private float _rushSpeed;
        [SerializeField] private float _coolTime;

        private float _coolTimer;
        private bool _isCoolDown;
        private float _currentDamage;
        private float _rushDurationTimer;
        private bool _isRushing;

        private void Update()
        {
            if(_isCoolDown)
            {
                _coolTimer += Time.deltaTime;

                if(_coolTimer >= _coolTime)
                {
                    _coolTimer = 0;
                    _isCoolDown = false;
                }
            }

            if(_isRushing)
            {
                _rushDurationTimer += Time.deltaTime;

                if(_rushDurationTimer >= _rushDuration)
                {
                    _rushDurationTimer = 0;
                    _isRushing = false;
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!_isRushing)
            {
                return;
            }

            if(collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.GetDamage(_currentDamage);
            }
        }

        public void Attack(in EnemyAttackContext context)
        {
            if(context.TargetTransform == null || _isCoolDown)
            {
                return;
            }

            _currentDamage = context.BaseDamage;

            Vector3 toTargetDirection = (context.TargetTransform.position - transform.position);

            toTargetDirection.y = 0;

            float toTargetAngle = Mathf.Atan2(toTargetDirection.x, 
                                              toTargetDirection.z) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0,toTargetAngle,0);

            _rigidbody.AddForce(transform.forward * _rushSpeed, ForceMode.Impulse);

            _isCoolDown = true;
            _isRushing = true;
        }
    }
}
