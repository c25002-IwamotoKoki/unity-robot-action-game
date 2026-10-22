using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    public sealed class BossController : MonoBehaviour, IDamageable
    {
        //ボスがプレイヤーを見失うなどは考えていないためそのままつかんでおきます
        [SerializeField] private Transform _player;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        private float _currentHealth;

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        private void Update()
        {
            RotateTwordsToPlayer();

            transform.position += _moveSpeed * Time.deltaTime * transform.forward;
        }

        public void GetDamage(float damage)
        {
            _currentHealth -= damage;
        }

        private void RotateTwordsToPlayer()
        {
            Vector3 direction = _player.position - transform.position;

            Quaternion lookDirection = Quaternion.LookRotation(direction);

            lookDirection.x = 0;
            lookDirection.z = 0;

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookDirection,
                _rotateSpeed * Time.deltaTime
            );
        }
    }
}
