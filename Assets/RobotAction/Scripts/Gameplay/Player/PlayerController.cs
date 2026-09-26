using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Energy;
using RobotAction.Gameplay.Parts.Weapons;
using RobotAction.Gameplay.Sensors;
using System.Collections;
using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerMover _playerMover;
        [SerializeField] private EnergyCoreData _energyCoreData;
        [SerializeField] private AutoLockSensorData _autoLockSensorData;
        [SerializeField] private WeaponPartsHandler _rightWeaponHandler;
        [SerializeField] private WeaponPartsHandler _leftWeaponHandler;
        [SerializeField] private float _mouseLookSensitivity;
        [SerializeField] private float _gamePadLookSensitivity;
        [SerializeField] private float _targetLookSpeed;
        [SerializeField] private float _health;
        [SerializeField, Min(0.2f)] private float _trackTargetCoolDown = 0.2f;

        private PlayerInputReader _inputReader;
        private TargetBuffer _targetBuffer;
        private EnergyCore _energyCore;
        private AutoLockSensor _autoLockSensor;
        private WaitForSeconds _trackTargetWait;
        private int _selectTargetNum;
        private bool _isRightAttacking;
        private bool _isLeftAttacking;
        private bool _isTracking;
        private bool _isLockOn;

        private void Awake()
        {
            _inputReader = new PlayerInputReader(new PlayerInputActions());
            _targetBuffer = new TargetBuffer();
            _energyCore = new EnergyCore(_energyCoreData);
            _playerMover.SetEnergyCore(_energyCore);
            _autoLockSensor = new(transform,_autoLockSensorData, _targetBuffer);

            _trackTargetWait = new WaitForSeconds(_trackTargetCoolDown * Time.deltaTime);
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

        private void Update()
        {
            _autoLockSensor?.Tick(Time.deltaTime, transform.position);
            _energyCore?.Tick(Time.deltaTime);

            Vector2 rawInput = _inputReader.MoveDirection;
            Vector3 moveDirection = transform.forward * rawInput.y + transform.right * rawInput.x;

            _playerMover.SetMoveDirection(moveDirection);

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

        private void HandleBoost(bool isBoosting)
        {
            if(isBoosting)
            {
                _playerMover.BoostImpulse();
            }

            _playerMover.SetBoostState(isBoosting);
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
            _playerMover.SetHoverState(isHovering);
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
