using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Sensors;
using RobotAction.Gameplay.Parts.Weapons.Guns;
using UnityEngine;
using RobotAction.Gameplay.Parts.Weapons;

namespace RobotAction.Gameplay.Player
{
    public class PlayerController : MonoBehaviour,IDamageable
    {
        private const float BoostDeadZoneSqr = 0.01f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private AutoLockSensorData _autoLockSensorData;
        [SerializeField] private WeaponPartsHandler _rightWeaponPartsHandler;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _hoverSpeed;
        [SerializeField] private float _defaultMaxSpeed;
        [SerializeField] private float _boostSpeed;
        [SerializeField] private float _boostMaxSpeed;
        [SerializeField] private float _maxSpeedDeceleration;
        [SerializeField] private float _speedBoostDuration;
        [SerializeField] private float _health;

        private PlayerInputReader _inputReader;
        private TargetBuffer _targetBuffer;
        private AutoLockSensor _autoLockSensor;
        private bool _isAttacking;
        private bool _isHovering;
        private bool _isBoosting;

        private void Awake()
        {
            _inputReader = new PlayerInputReader(new PlayerInputActions());
            _targetBuffer = new TargetBuffer();
            _autoLockSensor = new(_autoLockSensorData,_targetBuffer);

            _rigidbody.maxLinearVelocity = _defaultMaxSpeed;
        }

        private void OnEnable()
        {
            _inputReader.OnBoost += OnBoost;
            _inputReader.OnAttack += ShootGun;
            _inputReader.OnHover += OnHover;
            _inputReader.OnRightEquip += OnRightEquip;
            _inputReader.OnRightUnequip += OnRightUnequip;
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

            if(_isHovering)
            {
                _rigidbody.AddForce(transform.up * _hoverSpeed,
                                    ForceMode.Force);
            }

            if (_isBoosting)
            {
                _rigidbody.maxLinearVelocity = Mathf.MoveTowards(_rigidbody.maxLinearVelocity,
                                                                 _defaultMaxSpeed,
                                                                 _maxSpeedDeceleration * Time.fixedDeltaTime);

                if (_rigidbody.maxLinearVelocity == _defaultMaxSpeed)
                {
                    _isBoosting = false;
                }
            }

        }

        private void Update()
        {
            _autoLockSensor?.Tick(Time.deltaTime,transform.position);

            if(_isAttacking)
            {
                _rightWeaponPartsHandler.Attack();
            }
        }

        private void OnDisable()
        {
            _inputReader.OnBoost -= OnBoost;
            _inputReader.OnAttack -= ShootGun;
            _inputReader.OnHover -= OnHover;
            _inputReader.OnRightEquip -= OnRightEquip;
            _inputReader.OnRightUnequip -= OnRightUnequip;
            _inputReader.Dispose();
        }

        public void GetDamage(float damage)
        {      
            _health -= damage;
        }

        private void OnBoost()
        {
            Boost();
        }

        private void Boost()
        {
            Vector2 input = _inputReader.MoveDirection;

            if (input == Vector2.zero)
            {
                return;
            }

            _rigidbody.maxLinearVelocity = _boostMaxSpeed;

            input = input.sqrMagnitude > BoostDeadZoneSqr ? input.normalized : input;

            Vector3 boostVector = transform.forward * input.y + transform.right * input.x;

            _rigidbody.AddForce(_boostSpeed * boostVector,
                                ForceMode.Impulse);

            _isBoosting = true;
        }

        private void ShootGun(bool isShootActive)
        {
            _isAttacking = isShootActive;          
        }

        private void OnHover(bool isHovering)
        {
            _isHovering = isHovering;
        }

        private void OnRightEquip()
        {
            _rightWeaponPartsHandler.TryPickUpNearlyWeapon();
        }

        private void OnRightUnequip()
        {
            _rightWeaponPartsHandler.Unequip();
        }
    }
}
