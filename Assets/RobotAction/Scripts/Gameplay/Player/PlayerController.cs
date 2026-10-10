using RobotAction.Core;
using RobotAction.Gameplay.Combat;
using RobotAction.Gameplay.Energy;
using RobotAction.Gameplay.Parts.Weapons;
using RobotAction.Gameplay.Sensors;
using System;
using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class PlayerController : MonoBehaviour, IDamageable, IPlayerStatus
    {
        public event Action OnDied;
        public event Action<PlayerHealthInfo> OnHealthChanged;
        public event Action<PlayerEnergyInfo> OnEnergyChanged
        {
            add => _energyCore.OnEnergyChanged += value;
            remove => _energyCore.OnEnergyChanged -= value;
        }

        [SerializeField] private PlayerMoverData _playerMoverData;
        [SerializeField] private EnergyCoreData _energyCoreData;
        [SerializeField] private AutoLockSensorData _autoLockSensorData;
        [SerializeField] private PlayerLookData _lookData;
        [SerializeField] private WeaponPartsHandler _rightWeaponHandler;
        [SerializeField] private WeaponPartsHandler _leftWeaponHandler;
        [SerializeField] private float _maxHealth;

        public float MaxHealth => _maxHealth;
        public float CurrentHealth { get; private set; }
        public float MaxEnergy => _energyCore.MaxEnergy;
        public float CurrentEnergy => _energyCore.CurrentEnergy;
        public WeaponPartsHandler RightWeaponPartsHandler => _rightWeaponHandler;
        public WeaponPartsHandler LeftWeaponPartsHandler => _leftWeaponHandler;

        private PlayerInputReader _inputReader;
        private TargetBuffer _targetBuffer;
        private PlayerMover _mover;
        private EnergyCore _energyCore;
        private AutoLockSensor _autoLockSensor;
        private PlayerLookController _lookController;
        private int _selectTargetNum;
        private bool _isRightAttacking;
        private bool _isLeftAttacking;
        private bool _isLockOn;

        private void Awake()
        {
            _inputReader = new PlayerInputReader(new PlayerInputActions());

            CurrentHealth = _maxHealth;

            _energyCore = new EnergyCore(_energyCoreData);

            TryGetComponent(out Rigidbody rigidbody);
            _mover = new PlayerMover(owner: transform, _playerMoverData, rigidbody, _energyCore);

            _targetBuffer = new TargetBuffer();
            _autoLockSensor = new(transform, _autoLockSensorData, _targetBuffer);

            _lookController = new PlayerLookController(_lookData,owner:transform);
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
            _mover?.OnFixedUpdate(Time.fixedDeltaTime);
        }

        private void Update()
        {
            _autoLockSensor?.Tick(Time.deltaTime, transform.position);
            _energyCore?.Tick(Time.deltaTime);

            Vector2 rawInput = _inputReader.MoveDirection;
            Vector3 moveDirection = transform.forward * rawInput.y + transform.right * rawInput.x;

            _mover.SetMoveDirection(moveDirection);

            if (_isRightAttacking)
            {
                if (_targetBuffer.HasTarget)
                {
                    var target = _targetBuffer.DetectedTargets[_selectTargetNum];
                    _rightWeaponHandler.SetTarget(target.position);
                }

                _rightWeaponHandler.Attack();
            }

            if (_isLeftAttacking)
            {
                if (_targetBuffer.HasTarget)
                {
                    var target = _targetBuffer.DetectedTargets[_selectTargetNum];
                    _leftWeaponHandler.SetTarget(target.position);
                }

                _leftWeaponHandler.Attack();
            }          
        }

        private void LateUpdate()
        {
            if (_isLockOn && _targetBuffer.HasTarget)
            {
                _selectTargetNum = Mathf.Clamp(_selectTargetNum, 0, _targetBuffer.DetectedTargets.Count - 1);

                var target = _targetBuffer.DetectedTargets[_selectTargetNum];
                _lookController.TrackingTarget(target, Time.deltaTime);
            }
            else
            {
                _lookController.UpdateYawRotation(_inputReader.LookValue.x, Time.deltaTime, _inputReader.IsMouseLook);
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

            _inputReader.OnLockOn -= HandleLockOn;

            _inputReader.OnSwitchFartherTarget -= HandleSwitchRightTarget;
            _inputReader.OnSwitchCloserTarget -= HandleSwitchLeftTarget;

            _inputReader.Dispose();
        }

        public void GetDamage(float damage)
        {
            CurrentHealth -= damage;

            var HealthInfo = new PlayerHealthInfo(MaxHealth, CurrentHealth);

            OnHealthChanged?.Invoke(HealthInfo);

            if(CurrentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }

        private void HandleBoost(bool isBoosting)
        {
            if (isBoosting)
            {
                _mover.BoostImpulse();
            }

            _mover.SetBoostState(isBoosting);
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
            _mover.SetHoverState(isHovering);
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
            if (_isLockOn && _targetBuffer.HasTarget)
            {
                _selectTargetNum++;
                _selectTargetNum = Mathf.Clamp(_selectTargetNum,
                                         0,
                                         _targetBuffer.DetectedTargets.Count - 1);
            }
        }

        private void HandleSwitchLeftTarget()
        {
            if (_isLockOn && _targetBuffer.HasTarget)
            {
                _selectTargetNum--;
                _selectTargetNum = Mathf.Clamp(
                    _selectTargetNum,
                    0,
                    _targetBuffer.DetectedTargets.Count - 1
                );
            }
        }
    }
}
