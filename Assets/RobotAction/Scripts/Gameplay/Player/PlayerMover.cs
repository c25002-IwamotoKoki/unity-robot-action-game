using UnityEngine;
using UnityEngine.Windows;

namespace RobotAction.Gameplay.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMover : MonoBehaviour
    {
        private const float DeadZoneSqr = 0.01f;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _hoverSpeed;
        [SerializeField] private float _defaultMaxSpeed;
        [SerializeField] private float _boostImpulseSpeed;
        [SerializeField] private float _boostForceSpeed;
        [SerializeField] private float _boostMaxSpeed;
        [SerializeField] private float _maxSpeedDeceleration;

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
            if (_isHovering)
            {
                _rigidbody.AddForce(_hoverSpeed * transform.up, ForceMode.Force);
            }

            if (_currentMoveDirection.sqrMagnitude < DeadZoneSqr)
            {
                return;
            }

            if(_currentMoveDirection != Vector3.zero)
            {
                _rigidbody.AddForce(_currentMoveDirection * _moveSpeed,ForceMode.Force);
            }

            if(_isBoosting)
            {
                _rigidbody.AddForce(_boostForceSpeed * _currentMoveDirection, ForceMode.Force);
            }
            
            if(!_isBoosting && _rigidbody.maxLinearVelocity != _defaultMaxSpeed)
            {
                _rigidbody.maxLinearVelocity = Mathf.MoveTowards(_rigidbody.maxLinearVelocity,
                                                                _boostMaxSpeed,
                                                                _maxSpeedDeceleration * Time.fixedDeltaTime);
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
            _rigidbody.maxLinearVelocity = _boostMaxSpeed;

            _rigidbody.AddForce(_boostImpulseSpeed * _currentMoveDirection,
                                ForceMode.Impulse);
        }
    }
}
