using RobotAction.Gameplay.Energy;
using UnityEngine;
using UnityEngine.Windows;

namespace RobotAction.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMover : MonoBehaviour
    {
        private const float DeadZoneSqr = 0.01f;

        [SerializeField] private PlayerMoverData _data;

        private EnergyCore _energyCore;
        private Rigidbody _rigidbody;
        private Vector3 _currentMoveDirection;
        private bool _isBoosting;
        private bool _isHovering;

        private void Awake()
        {
            TryGetComponent(out _rigidbody);

            _rigidbody.maxLinearVelocity = _data.DefaultMaxSpeed;
        }

        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;

            if (_isHovering && _energyCore.TryCosume(_data.MoveEnergyCost * fixedDeltaTime))
            {
                _rigidbody.AddForce(_data.MoveSpeed * transform.up, ForceMode.Force);
            }

            if (_currentMoveDirection.sqrMagnitude < DeadZoneSqr)
            {
                return;
            }

            if (!_isBoosting && _energyCore.TryCosume(_data.MoveEnergyCost * fixedDeltaTime))
            {
                _rigidbody.AddForce(_currentMoveDirection * _data.MoveSpeed, ForceMode.Force);
            }

            if (_isBoosting && _energyCore.TryCosume(_data.BoostEnergyCost * fixedDeltaTime))
            {
                _rigidbody.AddForce(_data.BoostSpeed * _currentMoveDirection, ForceMode.Force);
            }

            if (!_isBoosting && _rigidbody.maxLinearVelocity != _data.DefaultMaxSpeed)
            {
                _rigidbody.maxLinearVelocity = Mathf.MoveTowards(_rigidbody.maxLinearVelocity,
                                                                 _data.BoostMaxSpeed,
                                                                 _data.MaxSpeedDeceleration * fixedDeltaTime);
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
            _rigidbody.maxLinearVelocity = _data.BoostMaxSpeed;

            if (_energyCore.TryCosume(_data.BoostEnergyCost))
            {
                _rigidbody.AddForce(_data.BoostSpeed * _currentMoveDirection,
                                    ForceMode.Impulse);
            }
        }
    }
}
