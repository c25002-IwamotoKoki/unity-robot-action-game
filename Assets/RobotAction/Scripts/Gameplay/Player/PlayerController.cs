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
        [SerializeField] private WeaponPartsHandler _weaponPartsHandler;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _hoverSpeed;
        [SerializeField] private float _boostSpeed;
        [SerializeField] private float _health;

        private PlayerInputReader _inputReader;
        private TargetBuffer _targetBuffer;
        private AutoLockSensor _autoLockSensor;
        private bool _isAttacking;
        private bool _isHovering;

        private void Awake()
        {
            _inputReader = new PlayerInputReader(new PlayerInputActions());
            _targetBuffer = new TargetBuffer();
            _autoLockSensor = new(_autoLockSensorData,_targetBuffer);
        }

        private void OnEnable()
        {
            _inputReader.OnBoost += OnBoost;
            _inputReader.OnAttack += ShootGun;
            _inputReader.OnHover += OnHover;
            _inputReader.OnEquip += OnEquip;
            _inputReader.OnUnequip += OnUnequip;
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

        }

        private void Update()
        {
            _autoLockSensor?.Tick(Time.deltaTime,transform.position);

            if(_isAttacking)
            {
                _weaponPartsHandler.Attack();
            }
        }

        private void OnDisable()
        {
            _inputReader.OnBoost -= OnBoost;
            _inputReader.OnAttack -= ShootGun;
            _inputReader.OnHover -= OnHover;
            _inputReader.OnEquip -= OnEquip;
            _inputReader.OnUnequip -= OnUnequip;
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
            input = input.sqrMagnitude > BoostDeadZoneSqr ? input.normalized : input;

            _rigidbody.AddForce(_boostSpeed * new Vector3(input.x, 0, input.y),
                                ForceMode.Impulse);
        }

        private void ShootGun(bool isShootActive)
        {
            _isAttacking = isShootActive;          
        }

        private void OnHover(bool isHovering)
        {
            _isHovering = isHovering;
        }

        private void OnEquip()
        {
            _weaponPartsHandler.TryPickUpNearlyWeapon();
        }

        private void OnUnequip()
        {
            _weaponPartsHandler.Purge();
        }
    }
}
