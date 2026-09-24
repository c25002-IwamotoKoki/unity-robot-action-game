using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Parts.Weapons;
using RobotAction.Gameplay.Sensors;
using System.Collections;
using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
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
        [SerializeField] private float _mouseLookSensitivity;
        [SerializeField] private float _gamePadLookSensitivity;
        [SerializeField] private float _targetLookSpeed;
        [SerializeField] private float _health;
        [SerializeField, Min(0.2f)] private float _trackTargetCoolDown = 0.2f;

        private PlayerInputReader _inputReader;
        private TargetBuffer _targetBuffer;
        private AutoLockSensor _autoLockSensor;
        private WaitForSeconds _trackTargetWait;
        private int _selectTargetNum;
        private bool _isRightAttacking;
        private bool _isLeftAttacking;
        private bool _isHovering;
        private bool _isBoosting;
        private bool _isTracking;
        private bool _isLockOn;

        private void Awake()
        {
            _inputReader = new PlayerInputReader(new PlayerInputActions());
            _targetBuffer = new TargetBuffer();
            _autoLockSensor = new(transform,_autoLockSensorData, _targetBuffer);

            _trackTargetWait = new WaitForSeconds(_trackTargetCoolDown * Time.deltaTime);
            _rigidbody.maxLinearVelocity = _defaultMaxSpeed;
            _isTracking = true;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(TrackingTargetRoutine());
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

            _inputReader.OnLockOn += HandleLockOn;

            _inputReader.OnSwitchFartherTarget += HandleSwitchRightTarget;
            _inputReader.OnSwitchCloserTarget += HandleSwitchLeftTarget;
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

            if (_isHovering)
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
            _autoLockSensor?.Tick(Time.deltaTime, transform.position);

            if (_isRightAttacking)
            {
                if (_targetBuffer.HasTarget)
                {
                    _rightWeaponHandler.SetTarget(_targetBuffer.DetectedTargets[0].position);
                }

                _rightWeaponHandler.Attack();
            }

            if (_isLeftAttacking)
            {
                if (_targetBuffer.HasTarget)
                {
                    _leftWeaponHandler.SetTarget(_targetBuffer.DetectedTargets[0].position);
                }

                _leftWeaponHandler.Attack();
            }

            UpdateLookRotation();
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

            _inputReader.OnLockOn -= HandleLockOn;

            _inputReader.OnSwitchFartherTarget -= HandleSwitchRightTarget;
            _inputReader.OnSwitchCloserTarget -= HandleSwitchLeftTarget;

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

        private IEnumerator TrackingTargetRoutine()
        {
            while (_isTracking)
            {
                if (_targetBuffer.HasTarget && _isLockOn)
                {
                    _selectTargetNum = Mathf.Clamp(_selectTargetNum,
                                                   0,
                                                   _targetBuffer.DetectedTargets.Count - 1);

                    RotateTowardsToTarget(_targetBuffer.DetectedTargets[_selectTargetNum]);
                }

                yield return _trackTargetWait;
            }
            yield break;
        }


        private void RotateTowardsToTarget(Transform target)
        {
            Vector3 direction = (target.position - transform.position).normalized;

            Quaternion rotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  rotation,
                                                  _targetLookSpeed * Time.deltaTime);
        }


        private void UpdateLookRotation()
        {
            if (_isLockOn) return;

            Vector2 rawInput = _inputReader.LookValue;

            if (rawInput.x == 0) return;

            if(_inputReader.IsMouseLook)
            {
                Vector3 angle = transform.localEulerAngles;

                angle.y += rawInput.x * _mouseLookSensitivity;

                transform.eulerAngles = angle;
            }
            else
            {
                Vector3 angle = transform.localEulerAngles;

                angle.y += rawInput.x * _gamePadLookSensitivity * Time.deltaTime;

                transform.eulerAngles = angle;
            }
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

        private void HandleLeftEquip()
        {
            _leftWeaponHandler.TryPickUpNearlyWeapon();
        }

        private void HandleLeftUnequip()
        {
            _leftWeaponHandler.Unequip();
        }

        private void HandleLockOn()
        {
            _isLockOn = !_isLockOn;
        }

        private void HandleSwitchRightTarget()
        {
            if(_isLockOn && _targetBuffer.HasTarget)
            {
                _selectTargetNum++;
                _selectTargetNum = Mathf.Clamp(_selectTargetNum,
                                         0,
                                         _targetBuffer.DetectedTargets.Count - 1);
            }
        }

        private void HandleSwitchLeftTarget()
        {
            if(_isLockOn && _targetBuffer.HasTarget)
            {
                _selectTargetNum--;
                _selectTargetNum = Mathf.Clamp(_selectTargetNum,
                                         0,
                                         _targetBuffer.DetectedTargets.Count-1);
            }
        }
    }
}
