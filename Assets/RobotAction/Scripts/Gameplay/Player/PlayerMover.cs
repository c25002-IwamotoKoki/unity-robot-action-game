using RobotAction.Gameplay.Energy;
using UnityEngine;

namespace RobotAction.Gameplay.Player
{
    public class PlayerMover
    {
        private const float DeadZoneSqr = 0.01f;

        private readonly Transform _owner;
        private readonly IPlayerMoverData _data;
        private readonly EnergyCore _energyCore;
        private readonly Rigidbody _rigidbody;

        private Vector3 _currentMoveDirection;
        private bool _isBoosting;
        private bool _isHovering;

        public PlayerMover(Transform owner,
                           IPlayerMoverData data,
                           Rigidbody rigidbody,
                           EnergyCore energyCore)
        {
            _owner = owner;
            _data = data;
            _rigidbody = rigidbody;
            _rigidbody.maxLinearVelocity = _data.DefaultMaxSpeed;
            _energyCore = energyCore;
        }

        public void OnFixedUpdate(float fixedDeltaTime)
        {
            if (_isHovering && _energyCore.TryCosume(_data.MoveEnergyCost * fixedDeltaTime))
            {
                _rigidbody.AddForce(_data.MoveSpeed * _owner.up, ForceMode.Force);
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
