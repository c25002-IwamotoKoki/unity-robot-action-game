using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Parts.Weapons;
using RobotAction.Gameplay.Sensors;
using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    public class PlayerController : MonoBehaviour,IDamageable
    {
        private const float BoostDeadZoneSqr = 0.01f;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private AutoLockSensorData _autoLockSensorData;
        [SerializeField] private WeaponPartsHandler _rightWeaponHandler;
        [SerializeField] private WeaponPartsHandler _leftWeaponHandler;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _hoverSpeed;
        [SerializeField] private float _defaultMaxSpeed;
        [SerializeField] private float _boostSpeed;
        [SerializeField] private float _boostMaxSpeed;
        [SerializeField] private float _maxSpeedDeceleration;
        [SerializeField] private float _health;

        private PlayerInputReader _inputReader;
        private TargetBuffer _targetBuffer;
        private AutoLockSensor _autoLockSensor;
        private bool _isRightAttacking;
        private bool _isLeftAttacking;
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
            _inputReader.OnBoost += HandleBoost;
            _inputReader.OnHover += HandleHover;

            _inputReader.OnRightAttack += HandleRightAttack;
            _inputReader.OnLeftAttack += HandleLeftAttack;

            _inputReader.OnRightEquip += HandleRightEquip;
            _inputReader.OnRightUnequip += HandleRightUnequip;
            _inputReader.OnLeftEquip += HandleLeftEquip;
            _inputReader.OnLeftUnequip += HandleLeftUnequip;
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

            if(_isRightAttacking)
            {

                if(_targetBuffer.HasTarget)
                {
                    _rightWeaponHandler.SetTarget(_targetBuffer.DetectedTargets[0].position);
                }

                _rightWeaponHandler.Attack();
            }

            if(_isLeftAttacking)
            {
                if (_targetBuffer.HasTarget)
                {
                    _leftWeaponHandler.SetTarget(_targetBuffer.DetectedTargets[0].position);
                }

                _leftWeaponHandler.Attack();
            }
        }

        private void OnDisable()
        {
            _inputReader.OnBoost -= HandleBoost;
            _inputReader.OnHover -= HandleHover;

            _inputReader.OnRightAttack -= HandleRightAttack;
            _inputReader.OnLeftAttack -= HandleLeftAttack;

            _inputReader.OnRightEquip -= HandleRightEquip;
            _inputReader.OnRightUnequip -= HandleRightUnequip;
            _inputReader.OnLeftEquip -= HandleLeftEquip;
            _inputReader.OnLeftUnequip -= HandleLeftUnequip;

            _inputReader.Dispose();
        }

        public void GetDamage(float damage)
        {      
            _health -= damage;
        }

        private void HandleBoost()
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

        private void HandleRightAttack(bool isAttacking)
        {
            _isRightAttacking = isAttacking;          
        }

        private void HandleLeftAttack(bool isAttacking)
        {
            _isLeftAttacking = isAttacking;
        }

        private void HandleHover(bool isHovering)
        {
            _isHovering = isHovering;
        }

        private void HandleRightEquip()
        {
            _rightWeaponHandler.TryPickUpNearlyWeapon();
        }

        private void HandleRightUnequip()
        {
            _rightWeaponHandler.Unequip();
        }

        public void HandleLeftEquip()
        {
            _leftWeaponHandler.TryPickUpNearlyWeapon();
        }

        public void HandleLeftUnequip()
        {
            _leftWeaponHandler.Unequip();
        }
    }
}
