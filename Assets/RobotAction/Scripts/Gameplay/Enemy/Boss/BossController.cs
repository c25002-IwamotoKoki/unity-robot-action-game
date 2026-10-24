using RobotAction.Gameplay.Combat;
using UnityEngine;

namespace RobotAction.Gameplay.Enemy.Boss
{
    public sealed class BossController : MonoBehaviour, IDamageable
    {
        //ボスがプレイヤーを見失うなどは考えていないためそのままつかんでおきます
        [SerializeField] private Transform _player;
        [SerializeField] private BossAttackBase[] _attacks;
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotateSpeed;

        private IDamageable _playerDamageable;
        private float _currentHealth;

        private void Awake()
        {
            if (!_player.TryGetComponent(out _playerDamageable))
            {
                Debug.LogWarning("インスペクターから指定したPlayerにIDamageableが実装されていません");
            }

            foreach (var attack in _attacks)
            {
                attack.SetPlayer(_player, _playerDamageable);
            }

            _currentHealth = _maxHealth;
        }

        private void Update()
        {
            RotateTwordsToPlayer();

            transform.position += _moveSpeed * Time.deltaTime * transform.forward;

            float toPlayerDistanceSqr = (_player.position - transform.position).sqrMagnitude;

            foreach (var attack in _attacks)
            {
                if (attack.CanAttack(toPlayerDistanceSqr))
                {
                    attack.Execute();
                    break;
                }
            }
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
