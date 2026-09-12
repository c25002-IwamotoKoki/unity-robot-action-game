using RobotAction.Gameplay.Interfaces;
using RobotAction.Gameplay.Weapons.Guns;
using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    public class PlayerController : MonoBehaviour,IDamageable
    {
        private const float BoostDeadZoneSqr = 0.01f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private GunBase _gun;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _boostSpeed;
        [SerializeField] private float _health;

        private PlayerInputReader _inputReader;

        private void Awake()
        {
            _inputReader = new PlayerInputReader(new PlayerInputActions());
        }

        private void OnEnable()
        {
            _inputReader.OnBoost += Boost;
            _inputReader.OnAttack += ShootGun;
        }

        private void FixedUpdate()
        {
            Vector2 input = _inputReader.MoveDirection;

            if (input.x != 0)
            {
                _rigidbody.AddForce(input.x * _moveSpeed * transform.right,
                                    ForceMode.Force);
            }

            if (input.y != 0)
            {
                _rigidbody.AddForce(input.y * _moveSpeed * transform.forward,
                                    ForceMode.Force);
            }
        }

        private void OnDisable()
        {
            _inputReader.OnBoost -= Boost;
            _inputReader.OnAttack -= ShootGun;
            _inputReader.Dispose();
        }

        public void GetDamage(float damage)
        {      
            _health -= damage;
        }

        private void Boost()
        {
            Vector2 input = _inputReader.MoveDirection;
            input = input.sqrMagnitude > BoostDeadZoneSqr ? input.normalized : input;

            _rigidbody.AddForce(_boostSpeed * new Vector3(input.x, 0, input.y),
                                ForceMode.Impulse);
        }

        private void ShootGun(bool isShoot)
        {
            _gun.Shoot();
        }
    }
}
