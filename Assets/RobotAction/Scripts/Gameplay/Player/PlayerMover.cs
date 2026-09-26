using RobotAction.Gameplay.Energy;
using UnityEngine;
using UnityEngine.Windows;

namespace RobotAction.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMover : MonoBehaviour
    {
        private const float DeadZoneSqr = 0.01f;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _moveEnergyCost;
        [SerializeField] private float _boostSpeed;
        [SerializeField] private float _boostEnergyCost;
        [SerializeField] private float _defaultMaxSpeed;
        [SerializeField] private float _boostMaxSpeed;
        [SerializeField] private float _maxSpeedDeceleration;

        private EnergyCore _energyCore;
        private Rigidbody _rigidbody;
        private Vector3 _currentMoveDirection;
        private bool _isBoosting;
        private bool _isHovering;

        private void Awake()
        {
            TryGetComponent(out _rigidbody);

            _rigidbody.maxLinearVelocity = _defaultMaxSpeed;
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;

            if (_isHovering && _energyCore.TryCosume(_moveEnergyCost * fixedDeltaTime))
            {
                _rigidbody.AddForce(_moveSpeed * transform.up, ForceMode.Force);
            }

            if (_currentMoveDirection.sqrMagnitude < DeadZoneSqr)
            {
                return;
            }

            if (!_isBoosting && _energyCore.TryCosume(_moveEnergyCost * fixedDeltaTime))
            {
                _rigidbody.AddForce(_currentMoveDirection * _moveSpeed, ForceMode.Force);
            }

            if (_isBoosting && _energyCore.TryCosume(_boostEnergyCost * fixedDeltaTime))
            {
                _rigidbody.AddForce(_boostSpeed * _currentMoveDirection, ForceMode.Force);
            }

            if (!_isBoosting && _rigidbody.maxLinearVelocity != _defaultMaxSpeed)
            {
                _rigidbody.maxLinearVelocity = Mathf.MoveTowards(_rigidbody.maxLinearVelocity,
                                                                 _boostMaxSpeed,
                                                                 _maxSpeedDeceleration * fixedDeltaTime);
            }
        }

        public void SetEnergyCore(EnergyCore energyCore)
        {
            _energyCore = energyCore;
        }

        public void SetBoostState(bool isBoosting)
        {
            _isBoosting = isBoosting;
        }

        public void SetHoverState(bool isHovering)
        {
            _isHovering = isHovering;
        }

        public void SetMoveDirection(Vector3 moveDirection)
        {
            _currentMoveDirection = moveDirection;
        }

        public void BoostImpulse()
        {
            _rigidbody.maxLinearVelocity = _boostMaxSpeed;

            if (_energyCore.TryCosume(_boostEnergyCost))
            {
                _rigidbody.AddForce(_boostSpeed * _currentMoveDirection,
                                    ForceMode.Impulse);
            }
        }
    }
}
